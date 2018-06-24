using System;

namespace CGL
{
    /// <summary>
    /// 2D boundaries
    /// </summary>
    public class Bounds2D
    {
        #region Properties

        /// <summary>
        /// Bounds on the X axis
        /// </summary>
        public Bounds X { get; set; }

        /// <summary>
        /// Bounds on the Y axis
        /// </summary>
        public Bounds Y { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a default <see cref="Bounds2D"/> instance
        /// </summary>
        public Bounds2D() { }

        /// <summary>
        /// Generates a <see cref="Bounds2D"/> instance with specified x and y axis bounds
        /// </summary>
        /// <param name="xBounds">Bounds on the X axis</param>
        /// <param name="yBounds">Bounds on the Y axis</param>
        /// <exception cref="ArgumentNullException"/>
        public Bounds2D(Bounds xBounds, Bounds yBounds)
        {
            Utils.ThrowIfNull((xBounds, nameof(xBounds)), (yBounds, nameof(yBounds)));

            X = xBounds;
            Y = yBounds;
        }

        /// <summary>
        /// Generates a <see cref="Bounds2D"/> instance with specified x and y axis bounds
        /// </summary>
        /// <param name="minX">Minimum X axis value that is allowed</param>
        /// <param name="maxX">Maximum X axis value that is allowed</param>
        /// <param name="minY">Minimum Y axis value that is allowed</param>
        /// <param name="maxY">Maximum Y axis value that is allowed</param>
        public Bounds2D(int minX, int maxX, int minY, int maxY)
        {
            X = new Bounds(minX, maxX);
            Y = new Bounds(minY, maxY);
        }

        /// <summary>
        /// Generates a <see cref="Bounds2D"/> instance with specified x and y axis bounds
        /// </summary>
        /// <param name="rectangle">Rectangle that represents 2D bounds</param>
        /// <exception cref="ArgumentNullException"/>
        public Bounds2D(Rectangle rectangle)
        {
            rectangle.ThrowIfNull(nameof(rectangle));

            X = new Bounds(rectangle.X, rectangle.Right);
            Y = new Bounds(rectangle.Y, rectangle.Bottom);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        public override bool Equals(object obj)
            => obj is Bounds2D other && other.X == X && other.Y == Y;

        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator ==(Bounds2D left, Bounds2D right) => Equals(left, right);
        /// <summary>
        /// Determines whether the specified object instances are not considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator !=(Bounds2D left, Bounds2D right) => !Equals(left, right);

        /// <summary>
        /// Returns the hash code for the value of this instance
        /// </summary>
        public override int GetHashCode() => X?.GetHashCode() ?? 0 + Y?.GetHashCode() ?? 0;

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        public override string ToString() => $"X bounds: {X}; Y bounds: {Y}";

        #endregion
    }
}
