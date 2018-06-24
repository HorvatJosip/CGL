using System;
using System.Collections.Generic;

using CGL.Entities;

namespace CGL.Printables
{
    /// <summary>
    /// Area used for printing messages (wrapped, they appear from top to bottom)
    /// </summary>
    public class MessageBoard
    {
        #region Fields

        private Printer printer;
        private List<Text> messages = new List<Text>();

        #endregion

        #region Properties

        /// <summary>
        /// Area that is printed on the screen
        /// </summary>
        public Rectangle DrawableArea { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Generates a <see cref="MessageBoard"/> instance with the given drawable area
        /// </summary>
        /// <param name="drawableArea">Area that is printed on the screen</param>
        /// <exception cref="ArgumentNullException"/>
        public MessageBoard(Rectangle drawableArea)
        {
            drawableArea.ThrowIfNull(nameof(drawableArea));

            DrawableArea = drawableArea;
            printer = new Printer(drawableArea);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new message to the top of the message board. If the message board is full, messages
        /// that aren't displayed are removed
        /// </summary>
        /// <param name="message">Message to add to the message board</param>
        /// <param name="drawMessages">Should the messages be redrawn</param>
        /// <exception cref="ArgumentNullException"/>
        public void AddMessage(Text message, bool drawMessages = true)
        {
            message.ThrowIfNull(nameof(message));

            messages.Add(message);

            if (drawMessages)
                DrawMessages();
        }

        /// <summary>
        /// Clears the drawable area and draws all the messages
        /// </summary>
        public bool DrawMessages()
        {
            printer.ClearDrawableArea();
            
            return printer.DrawEntities(GetMessages(), false);
        }

        private List<Entity> GetMessages()
        {
            var entities = new List<Entity>();
            int wrapped = 0;

            var texts = new List<List<Drawable>>();

            for (int i = messages.Count - 1; i >= 0; i--)
            {
                var message = messages[i];

                message.Orientation = Direction.Right;
                var messagePosition = new Position(DrawableArea.X, DrawableArea.Y + wrapped);

                texts.Add(message.GetWrappedText(DrawableArea, messagePosition, out int wrapCount));
                wrapped += wrapCount + 1;
            }

            foreach (var text in texts)
            {
                if (text == null)
                    messages.RemoveAt(0);
                else
                    entities.AddRange(text);
            }

            return entities;
        }

        #endregion
    }
}
