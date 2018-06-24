using System;
using System.Collections.Generic;

namespace CGL.Printables
{
    /// <summary>
    /// Area with fixed and dynamic texts (e.g. player stats)
    /// </summary>
    /// <typeparam name="TKey">Key type, unique identifiers for each text</typeparam>
    public class PrintableArea<TKey>
    {
        #region Fields

        private Printer printer;
        private IDictionary<TKey, Text> changableText = new Dictionary<TKey, Text>();
        private IDictionary<TKey, Text> fixedText = new Dictionary<TKey, Text>();

        #endregion

        #region Properties

        #region Indexers

        /// <summary>
        /// Gets a text from one of the collections
        /// </summary>
        /// <param name="key">Key for the certain collection</param>
        /// <param name="getChangableText">Should the changable text collection be used or the fixed text collection</param>
        /// <exception cref="ArgumentNullException"/>
        public Text this[TKey key, bool getChangableText = true]
        {
            get
            {
                Text text;

                if (getChangableText)
                    changableText.TryGetValue(key, out text);
                else
                    fixedText.TryGetValue(key, out text);

                return text;
            }
        }

        #endregion

        /// <summary>
        /// Area that is printed on the screen
        /// </summary>
        public Rectangle DrawableArea { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Generates a <see cref="PrintableArea{TKey}"/> instance with the given drawable area.
        /// </summary>
        /// <param name="drawableArea">Area that is printed on the screen</param>
        /// <exception cref="ArgumentNullException"/>
        public PrintableArea(Rectangle drawableArea)
        {
            drawableArea.ThrowIfNull(nameof(drawableArea));

            DrawableArea = drawableArea;
            printer = new Printer(drawableArea);
        }

        #endregion

        #region Methods

        #region Editing

        /// <summary>
        /// Performs an operation on the changable text collection:
        /// <para><see cref="EditOperation.Create"/>: adds a new text</para>
        /// <para><see cref="EditOperation.Update"/>: updates an existing text</para>
        /// <para><see cref="EditOperation.Delete"/>: deletes a text object</para>
        /// <para><see cref="EditOperation.UpdateOrCreate"/>: if the text already exists, it updates it, 
        /// otherwise, it creates a new one</para>
        /// </summary>
        /// <param name="operation">Edit operation to perform</param>
        /// <param name="key">Unique identifier of the text object</param>
        /// <param name="text">Text to edit (not needed for deletion)</param>
        /// <param name="draw">Should the text collection be redrawn after the edit</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool EditChangableText(EditOperation operation, TKey key, Text text, bool draw)
            => EditText(changableText, operation, key, text, draw);

        /// <summary>
        /// Performs an operation on the fixed text collection:
        /// <para><see cref="EditOperation.Create"/>: adds a new text</para>
        /// <para><see cref="EditOperation.Update"/>: updates an existing text</para>
        /// <para><see cref="EditOperation.Delete"/>: deletes a text object</para>
        /// <para><see cref="EditOperation.UpdateOrCreate"/>: if the text already exists, it updates it, 
        /// otherwise, it creates a new one</para>
        /// </summary>
        /// <param name="operation">Edit operation to perform</param>
        /// <param name="key">Unique identifier of the text object</param>
        /// <param name="text">Text to edit (not needed for deletion)</param>
        /// <param name="draw">Should the text collection be redrawn after the edit</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool EditFixedText(EditOperation operation, TKey key, Text text, bool draw)
            => EditText(fixedText, operation, key, text, draw);

        private bool EditText(IDictionary<TKey, Text> dic, EditOperation operation, TKey key, Text text, bool draw)
        {
            Utils.ThrowIfNull((dic, nameof(dic)), (text, nameof(text)));

            switch (operation)
            {
                case EditOperation.Create:
                    return AddText(dic, key, text, draw);
                case EditOperation.Update:
                    return UpdateText(dic, key, text, draw);
                case EditOperation.Delete:
                    return dic.Remove(key);
                case EditOperation.UpdateOrCreate:
                    return UpdateText(dic, key, text, draw)
                    ? true
                    : AddText(dic, key, text, draw);
                default:
                    throw new UnknownEnumValueException();
            }
        }

        private bool AddText(IDictionary<TKey, Text> dic, TKey key, Text text, bool draw)
        {
            if (!text.Wrap &&
                !new Rectangle(text.StartPosition, text.Content.Count, 1).FullyInside(DrawableArea, InclusionOptions.BothInclusive))
                    return false;

            dic.Add(key, text);

            if (draw)
                Utils.Draw(text.Content, false);

            return true;
        }

        private bool UpdateText(IDictionary<TKey, Text> dic, TKey key, Text text, bool draw)
        {
            if (!text.Wrap &&
                !new Rectangle(text.StartPosition, text.Content.Count, 1).FullyInside(DrawableArea, InclusionOptions.BothInclusive) ||
                !dic.ContainsKey(key))
                return false;

            dic[key] = text;

            if (draw)
                Utils.Draw(text.Content, false);

            return true;
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Redraws both text collections
        /// </summary>
        /// <param name="clearDrawableArea">Should the drawable area be cleared before the redraw</param>
        public bool RedrawAll(bool clearDrawableArea) => Redraw(clearDrawableArea, true) && Redraw(false, false);

        /// <summary>
        /// Redraws a text collection
        /// </summary>
        /// <param name="clearDrawableArea">Should the drawable area be cleared before the redraw</param>
        /// <param name="changableText">Should the changable text collection be used or the fixed text collection</param>
        public bool Redraw(bool clearDrawableArea, bool changableText = true)
            => DrawText(changableText ? this.changableText : fixedText, clearDrawableArea);

        private bool DrawText(IDictionary<TKey, Text> dic, bool clearDrawableArea)
        {
            dic.ThrowIfNull(nameof(dic));

            if (clearDrawableArea)
                printer.ClearDrawableArea();

            foreach (var kvp in dic)
                if (!Utils.Draw(kvp.Value.Content, false))
                    return false;

            return true;
        }

        #endregion

        #endregion
    }
}
