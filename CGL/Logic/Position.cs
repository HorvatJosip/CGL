using System;

namespace CGL
{
    /// <summary>
    /// Position / Location / Point
    /// </summary>
    public class Position
    {
        #region Properties

        /// <summary>
        /// (0, 0)
        /// </summary>
        public static Position Zero { get; } = new Position(0, 0);

        /// <summary>
        /// Location on the X axis
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Location on the Y axis
        /// </summary>
        public int Y { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a default <see cref="Position"/> instance
        /// </summary>
        public Position() { }

        /// <summary>
        /// Generates a <see cref="Position"/> instance with specified x and y locations
        /// </summary>
        /// <param name="x">Location on the X axis</param>
        /// <param name="y">Location on the Y axis</param>
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Methods

        #region Boundary Checks

        /// <summary>
        /// Checks if the current position is within the specified rectangle
        /// </summary>
        /// <param name="rectangle">Rectangle to check if it contains the current position</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool InsideRectangle(Rectangle rectangle)
        {
            rectangle.ThrowIfNull(nameof(rectangle));

            return
                X.Between(rectangle.X, rectangle.Right) &&
                Y.Between(rectangle.Y, rectangle.Bottom);
        }

        /// <summary>
        /// Checks if a position relative to the current one is within the specified boundaries
        /// </summary>
        /// <param name="rectangle">Rectangle to check if it contains the relative position</param>
        /// <param name="direction">Direction in which the movement should occurr</param>
        /// <param name="amount">How many tiles should the movement cover</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool InsideRectangleAfterMovement(Rectangle rectangle, Direction direction, int amount)
        {
            rectangle.ThrowIfNull(nameof(rectangle));

            return GetRelativePosition(direction, amount).InsideRectangle(rectangle);
        }

        /// <summary>
        /// Checks if the current position is within the specified boundaries
        /// </summary>
        /// <param name="bounds">Bounds to check if they contain the current position</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool InsideBounds(Bounds2D bounds)
        {
            bounds.ThrowIfNull(nameof(bounds));

            return
                X.Between(bounds.X.Min, bounds.X.Max) &&
                Y.Between(bounds.Y.Min, bounds.Y.Max);
        }

        /// <summary>
        /// Checks if a position relative to the current one is within the specified boundaries
        /// </summary>
        /// <param name="bounds">Bounds to check if they contain the relative position</param>
        /// <param name="direction">Direction in which the movement should occurr</param>
        /// <param name="amount">How many tiles should the movement cover</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool InsideBoundsAfterMovement(Bounds2D bounds, Direction direction, int amount)
        {
            bounds.ThrowIfNull(nameof(bounds));

            return GetRelativePosition(direction, amount).InsideBounds(bounds);
        }

        #endregion

        #region Movement

        /// <summary>
        /// Moves the current position in a random direction by a specific amount
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="UnknownEnumValueException"/>
        public void MoveInRandomDirection(int amount) => Move(Utils.GetRandomEnumValue<Direction>(), amount);

        /// <summary>
        /// Moves the current position in a specific direction by a specific amount
        /// </summary>
        /// <param name="direction">Direction in which to move the current position</param>
        /// <param name="amount">How many tiles to move</param>
        /// <exception cref="UnknownEnumValueException"/>
        public void Move(Direction direction, int amount)
        {
            var newPosition = GetRelativePosition(direction, amount);

            X = newPosition.X;
            Y = newPosition.Y;
        }

        /// <summary>
        /// Gets position relative to the current position (this object isn't changed,
        /// new position instance is created based on it)
        /// </summary>
        /// <param name="direction">Direction in which the movement should occurr</param>
        /// <param name="amount">How many tiles should the movement cover</param>
        /// <exception cref="UnknownEnumValueException"/>
        public Position GetRelativePosition(Direction direction, int amount)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Position(X, Y - amount);
                case Direction.Down:
                    return new Position(X, Y + amount);
                case Direction.Left:
                    return new Position(X - amount, Y);
                case Direction.Right:
                    return new Position(X + amount, Y);
                default:
                    throw new UnknownEnumValueException();
            }
        }

        /// <summary>
        /// Gets position relative to the current position (this object isn't changed,
        /// new position instance is created based on it)
        /// </summary>
        /// <param name="xMovement">Amount to move on the x axis</param>
        /// <param name="yMovement">Amount to move on the y axis</param>
        public Position GetRelativePosition(int xMovement, int yMovement)
            => new Position(X + xMovement, Y + yMovement);

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        public override bool Equals(object obj)
            => obj is Position other && other.X == X && other.Y == Y;

        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator ==(Position left, Position right) => Equals(left, right);
        /// <summary>
        /// Determines whether the specified object instances are not considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator !=(Position left, Position right) => !Equals(left, right);

        /// <summary>
        /// Returns the hash code for the value of this instance
        /// </summary>
        public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode();

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        public override string ToString() => $"({X}, {Y})";

        #endregion

        #endregion Methods
    }
}
