using System;

namespace CGL
{
    /// <summary>
    /// 1D boundaries
    /// </summary>
    public class Bounds
    {
        #region Properties

        /// <summary>
        /// Minimum value that is allowed
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// Maximum value that is allowed
        /// </summary>
        public int Max { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a default <see cref="Bounds"/> instance
        /// </summary>
        public Bounds() { }

        /// <summary>
        /// Generates a <see cref="Bounds"/> instance with specified minimum and maximum
        /// </summary>
        /// <param name="min">Minimum value that is allowed</param>
        /// <param name="max">Maximum value that is allowed</param>
        public Bounds(int min, int max)
        {
            Min = min;
            Max = max;
        }

        #endregion

        #region Methods

        #region Operators

        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="bounds">Subtrahend</param>
        /// <param name="amount">Amount to subtract</param>
        /// <exception cref="ArgumentNullException"/>
        public static Bounds operator -(Bounds bounds, int amount)
        {
            bounds.ThrowIfNull(nameof(bounds));

            return new Bounds(
                bounds.Min - amount,
                bounds.Max - amount
            );
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="bounds">Addend</param>
        /// <param name="amount">Amount to add</param>
        /// <exception cref="ArgumentNullException"/>
        public static Bounds operator +(Bounds bounds, int amount)
        {
            bounds.ThrowIfNull(nameof(bounds));

            return new Bounds(
                bounds.Min + amount,
                bounds.Max + amount
            );
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="bounds">Multiplier</param>
        /// <param name="amount">Amount to multiply by</param>
        /// <exception cref="ArgumentNullException"/>
        public static Bounds operator *(Bounds bounds, int amount)
        {
            bounds.ThrowIfNull(nameof(bounds));

            return new Bounds(
                bounds.Min * amount,
                bounds.Max * amount
            );
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="bounds">Divisor</param>
        /// <param name="amount">Amount to divide by</param>
        /// <exception cref="ArgumentNullException"/>
        public static Bounds operator /(Bounds bounds, int amount)
        {
            bounds.ThrowIfNull(nameof(bounds));

            return new Bounds(
                bounds.Min / amount,
                bounds.Max / amount
            );
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        public override bool Equals(object obj)
            => obj is Bounds other && other.Min == Min && other.Max == Max;

        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator ==(Bounds left, Bounds right) => Equals(left, right);
        /// <summary>
        /// Determines whether the specified object instances are not considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator !=(Bounds left, Bounds right) => !Equals(left, right);

        /// <summary>
        /// Returns the hash code for the value of this instance
        /// </summary>
        public override int GetHashCode() => Min.GetHashCode() + Max.GetHashCode();

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        public override string ToString() => $"Min: {Min}, Max: {Max}"; 

        #endregion

        #endregion
    }
}
