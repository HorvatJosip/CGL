using System;
using System.Collections.Generic;

namespace CGL.Printables
{
    /// <summary>
    /// Collection of <see cref="Drawable"/>s that can be wrapped
    /// </summary>
    public class Text
    {
        private List<Drawable> letters;

        #region Properties

        /// <summary>
        /// Where should the text start to be rendered
        /// </summary>
        public Position StartPosition { get; set; }

        /// <summary>
        /// Content of the text (letters)
        /// </summary>
        public List<Drawable> Content => new List<Drawable>(letters);

        /// <summary>
        /// Text orientation / render direction
        /// </summary>
        public Direction Orientation { get; set; } = Direction.Right;

        /// <summary>
        /// Flag to indicate if the text should be wrapped or not
        /// </summary>
        public bool Wrap { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a default <see cref="Text"/> instance
        /// </summary>
        public Text() { }

        /// <summary>
        /// Generates a <see cref="Text"/> instance with the specified content
        /// </summary>
        /// <param name="content">Content of the text (letters)</param>
        /// <exception cref="ArgumentNullException"/>
        public Text(IEnumerable<Drawable> content) => SetContent(content);

        /// <summary>
        /// Generates a <see cref="Text"/> instance with the specified content
        /// </summary>
        /// <param name="content">Content of the text (letters)</param>
        /// <exception cref="ArgumentNullException"/>
        public Text(params Drawable[] content) => SetContent(content);

        /// <summary>
        /// Generates a <see cref="Text"/> instance with the specified content and its color
        /// </summary>
        /// <param name="content">Content of the text (letters)</param>
        /// <param name="color">Color of the content</param>
        /// <exception cref="ArgumentNullException"/>
        public Text(string content, ConsoleColor color) : this(Position.Zero, content, color) { }

        /// <summary>
        /// Generates a <see cref="Text"/> instance with the specified position, content and its color
        /// </summary>
        /// <param name="startPosition">Where should the text start to be rendered</param>
        /// <param name="content">Content of the text (letters)</param>
        /// <param name="color">Color of the content</param>
        /// <exception cref="ArgumentNullException"/>
        public Text(Position startPosition, string content, ConsoleColor color)
            => SetContent(startPosition, content, color);

        #endregion

        #region Methods

        #region Setting Content

        /// <summary>
        /// Sets the content with a specified color at a specified location
        /// </summary>
        /// <param name="startPosition">Where should the text start to be rendered</param>
        /// <param name="content">Content of the text (letters)</param>
        /// <param name="color">Color of the content</param>
        /// <exception cref="ArgumentNullException"/>
        public void SetContent(Position startPosition, string content, ConsoleColor color)
        {
            startPosition.ThrowIfNull(nameof(startPosition));

            StartPosition = startPosition;
            SetContent(content, color);
        }

        /// <summary>
        /// Sets the content with a specified color
        /// </summary>
        /// <param name="content">Content of the text (letters)</param>
        /// <param name="color">Color of the content</param>
        /// <exception cref="ArgumentNullException"/>
        public void SetContent(string content, ConsoleColor color)
        {
            content.ThrowIfNull(nameof(content));

            letters = new List<Drawable>();
            var position = StartPosition;

            foreach (var letter in content)
            {
                letters.Add(new Drawable(new Graphics(color, letter), position));

                position = position.GetRelativePosition(Orientation, 1);
            }
        }

        /// <summary>
        /// Sets the content to a collection of letters
        /// </summary>
        /// <param name="content">Content of the text (letters)</param>
        /// <exception cref="ArgumentNullException"/>
        public void SetContent(IEnumerable<Drawable> content)
        {
            content.ThrowIfNull(nameof(content));

            letters = new List<Drawable>(content);
        }

        #endregion

        #region Wrapping

        /// <summary>
        /// Wraps the content based on a rectangle
        /// </summary>
        /// <param name="limitingRectangle">Rectangle that is used for wrapping logic</param>
        /// <param name="useStartPositionFromRectangle">Should the <see cref="Rectangle.StartPosition"/> of the
        /// <paramref name="limitingRectangle"/> be used or the <see cref="StartPosition"/> for the
        /// text rendering starting location</param>
        /// <param name="wrapCount">How many lines were wrapped</param>
        /// <exception cref="ArgumentNullException"/>
        public List<Drawable> GetWrappedText(Rectangle limitingRectangle, bool useStartPositionFromRectangle, out int wrapCount)
            => GetWrappedText(
                limitingRectangle,
                useStartPositionFromRectangle ? limitingRectangle?.StartPosition : StartPosition,
                out wrapCount
               );

        /// <summary>
        /// Wraps the content based on a rectangle
        /// </summary>
        /// <param name="limitingRectangle">Rectangle that is used for wrapping logic</param>
        /// <param name="startPosition">Text rendering starting location</param>
        /// <param name="wrapCount">How many lines were wrapped</param>
        /// <exception cref="ArgumentNullException"/>
        public List<Drawable> GetWrappedText(Rectangle limitingRectangle, Position startPosition, out int wrapCount)
        {
            Utils.ThrowIfNull((limitingRectangle, nameof(limitingRectangle)), (startPosition, nameof(startPosition)));

            if (Content == null ||
                Orientation == Direction.None ||
                !startPosition.InsideRectangle(limitingRectangle))
            {
                wrapCount = -1;
                return null;
            }

            var entities = new List<Drawable>();
            var position = startPosition;
            wrapCount = 0;

            foreach (var character in Content)
            {
                entities.Add(new Drawable(character.Graphics, position));

                position = position.GetRelativePosition(Orientation, 1);

                if (!position.InsideRectangle(limitingRectangle))
                {
                    wrapCount++;

                    switch (Orientation)
                    {
                        case Direction.Up:
                        case Direction.Down:
                            position = new Position(startPosition.X + wrapCount, startPosition.Y);
                            break;
                        case Direction.Left:
                        case Direction.Right:
                            position = new Position(startPosition.X, startPosition.Y + wrapCount);
                            break;
                        default:
                            return null;
                    }

                    if (!position.InsideRectangle(limitingRectangle))
                        return entities;
                }
            }

            return entities;
        } 

        #endregion

        #endregion
    }
}
