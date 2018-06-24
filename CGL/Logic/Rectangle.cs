using System;

namespace CGL
{
    /// <summary>
    /// Shape with a starting point, width and height
    /// </summary>
    public class Rectangle
    {
        #region Properties

        /// <summary>
        /// Position where the rectangle's top left corner is (usually where the drawing begins)
        /// </summary>
        public Position StartPosition { get; set; }
        /// <summary>
        /// Width of the rectangle
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height of the rectangle
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// X coordinate of the <see cref="StartPosition"/>
        /// </summary>
        public int X => StartPosition.X;
        /// <summary>
        /// Y coordinate of the <see cref="StartPosition"/>
        /// </summary>
        public int Y => StartPosition.Y;
        /// <summary>
        /// X coordinate of the right side of the rectangle
        /// </summary>
        public int Right => StartPosition.X + Width - 1;
        /// <summary>
        /// Y coordinate of the bottom side of the rectangle
        /// </summary>
        public int Bottom => StartPosition.Y + Height - 1;

        /// <summary>
        /// Top left point of the rectangle
        /// </summary>
        public Position TopLeft => StartPosition;
        /// <summary>
        /// Top Right point of the rectangle
        /// </summary>
        public Position TopRight => new Position(Right, Y);
        /// <summary>
        /// Bottom left point of the rectangle
        /// </summary>
        public Position BottomLeft => new Position(X, Bottom);
        /// <summary>
        /// Bottom Right point of the rectangle
        /// </summary>
        public Position BottomRight => new Position(Right, Bottom);

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a default <see cref="Rectangle"/> instance
        /// </summary>
        public Rectangle() { }

        /// <summary>
        /// Generates a <see cref="Rectangle"/> instance with the specified start position and size
        /// </summary>
        /// <param name="startX">X coordinate of the start position</param>
        /// <param name="startY">Y coordinate of the start position</param>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <exception cref="ArgumentNullException"/>
        public Rectangle(int startX, int startY, int width, int height)
            : this(new Position(startX, startY), width, height) { }

        /// <summary>
        /// Generates a <see cref="Rectangle"/> instance with the specified start position and size
        /// </summary>
        /// <param name="startPosition">Position where the rectangle's top left corner is 
        /// (usually where the drawing begins)</param>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <exception cref="ArgumentNullException"/>
        public Rectangle(Position startPosition, int width, int height)
        {
            startPosition.ThrowIfNull(nameof(startPosition));

            StartPosition = startPosition;
            Width = width;
            Height = height;
        }

        #endregion

        #region Methods

        #region Overlap Checks

        /// <summary>
        /// Checks if a side of the other rectangle is inside the current one
        /// </summary>
        /// <param name="other">Other rectangle</param>
        /// <param name="side">Side to check</param>
        /// <param name="coordinateInclusion">How should the check be performed</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool SideIn(Rectangle other, Direction side, InclusionOptions coordinateInclusion)
        {
            other.ThrowIfNull(nameof(other));

            switch (side)
            {
                case Direction.Up:
                    if (!Y.Between(other.Y, other.Bottom, coordinateInclusion))
                        return false;
                    break;
                case Direction.Down:
                    if (!Bottom.Between(other.Y, other.Bottom, coordinateInclusion))
                        return false;
                    break;
                case Direction.Left:
                    if (!X.Between(other.X, other.Right, coordinateInclusion))
                        return false;
                    break;
                case Direction.Right:
                    if (!Right.Between(other.X, other.Right, coordinateInclusion))
                        return false;
                    break;
                default:
                    throw new UnknownEnumValueException();
            }

            return OtherAxisCheck(other, side, coordinateInclusion);
        }

        private bool OtherAxisCheck(Rectangle other, Direction side, InclusionOptions coordinateInclusion)
        {
            other.ThrowIfNull(nameof(other));

            switch (side)
            {
                case Direction.Up:
                case Direction.Down:
                    switch (coordinateInclusion)
                    {
                        case InclusionOptions.BothInclusive:
                            return X >= other.X && Right <= other.Right;
                        case InclusionOptions.OnlyLeftInclusive:
                            return X >= other.X && Right < other.Right;
                        case InclusionOptions.OnlyRightInclusive:
                            return X > other.X && Right <= other.Right;
                        case InclusionOptions.BothExclusive:
                            return X > other.X && Right < other.Right;
                        default:
                            throw new UnknownEnumValueException();
                    }
                case Direction.Left:
                case Direction.Right:
                    switch (coordinateInclusion)
                    {
                        case InclusionOptions.BothInclusive:
                            return Y >= other.Y && Bottom <= other.Bottom;
                        case InclusionOptions.OnlyLeftInclusive:
                            return Y >= other.Y && Bottom < other.Bottom;
                        case InclusionOptions.OnlyRightInclusive:
                            return Y > other.Y && Bottom <= other.Bottom;
                        case InclusionOptions.BothExclusive:
                            return Y > other.Y && Bottom < other.Bottom;
                        default:
                            throw new UnknownEnumValueException();
                    }
                default:
                    throw new UnknownEnumValueException();
            }
        }

        /// <summary>
        /// Checks if other rectangle is fully inside the current one
        /// </summary>
        /// <param name="other">Other rectangle</param>
        /// <param name="coordinateInclusion">How should the check be performed</param>
        /// <exception cref="ArgumentNullException"/>
        public bool FullyInside(Rectangle other, InclusionOptions coordinateInclusion)
            => Inside(other, true, coordinateInclusion) ==
            (Direction.Up | Direction.Down | Direction.Left | Direction.Right);

        /// <summary>
        /// Checks which sides of the other rectangle are inside the current one
        /// </summary>
        /// <param name="other">Other rectangle</param>
        /// <param name="fullSideInsideCheck">Should the side be qualified as "inside"
        /// if it is fully in or if it partially in</param>
        /// <param name="coordinateInclusion">How should the check be performed</param>
        /// <exception cref="ArgumentNullException"/>
        public Direction Inside(Rectangle other, bool fullSideInsideCheck, InclusionOptions coordinateInclusion)
        {
            other.ThrowIfNull(nameof(other));

            var direction = Direction.None;

            bool leftSideIn = SideIn(other, Direction.Left, coordinateInclusion);
            bool rightSideIn = SideIn(other, Direction.Right, coordinateInclusion);
            bool topSideIn = SideIn(other, Direction.Up, coordinateInclusion);
            bool bottomSideIn = SideIn(other, Direction.Down, coordinateInclusion);

            bool topAndBottomIn = topSideIn && bottomSideIn;
            bool leftAndRightIn = leftSideIn && rightSideIn;

            if (!fullSideInsideCheck || topAndBottomIn)
            {
                if (leftSideIn)
                    direction |= Direction.Left;

                if (rightSideIn)
                    direction |= Direction.Right;
            }
            if (!fullSideInsideCheck || leftAndRightIn)
            {
                if (topSideIn)
                    direction |= Direction.Up;
                if (bottomSideIn)
                    direction |= Direction.Down;
            }

            return direction;
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the rectangle at the <see cref="StartPosition"/> with the given graphics
        /// </summary>
        /// <param name="graphics">Graphics to draw the rectangle with</param>
        /// <exception cref="ArgumentNullException"/>
        public void Draw(Graphics graphics) => Draw(StartPosition, graphics);

        /// <summary>
        /// Draws the rectangle at the specified position with the given graphics
        /// </summary>
        /// <param name="startPosition">Position where the rectangle should be drawn</param>
        /// <param name="graphics">Graphics to draw the rectangle with</param>
        /// <exception cref="ArgumentNullException"/>
        public void Draw(Position startPosition, Graphics graphics)
        {
            Utils.ThrowIfNull((startPosition, nameof(startPosition)), (graphics, nameof(graphics)));

            Console.SetCursorPosition(startPosition.X, startPosition.Y);
            Utils.Draw(new string(graphics.Character, Width), graphics.Color, true, false);
            for (int y = startPosition.Y; y <= startPosition.Y + Height - 2; y++)
            {
                Console.SetCursorPosition(startPosition.X, y);
                Utils.Draw(graphics, true);
                Console.SetCursorPosition(startPosition.X + Width - 1, y);
                Utils.Draw(graphics, true);
            }
            Console.SetCursorPosition(startPosition.X, startPosition.Y + Height - 1);
            Utils.Draw(new string(graphics.Character, Width), graphics.Color, true, false);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        public override bool Equals(object obj)
            => obj is Rectangle rect && rect.StartPosition == StartPosition && rect.Width == Width && rect.Height == Height;

        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator ==(Rectangle left, Rectangle right) => Equals(left, right);
        /// <summary>
        /// Determines whether the specified object instances are not considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator !=(Rectangle left, Rectangle right) => !Equals(left, right);

        /// <summary>
        /// Returns the hash code for the value of this instance
        /// </summary>
        public override int GetHashCode()
            => StartPosition.GetHashCode() + Width.GetHashCode() + Height.GetHashCode();

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        public override string ToString()
            => $"Start: {StartPosition}; Size: {Width} x {Height}"; 

        #endregion

        #endregion
    }
}
