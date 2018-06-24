using System;

namespace CGL
{
    /// <summary>
    /// Colored character on the console
    /// </summary>
    public class Graphics
    {
        #region Properties

        /// <summary>
        /// Drawing color
        /// </summary>
        public ConsoleColor Color { get; set; }

        /// <summary>
        /// Character that will be drawn
        /// </summary>
        public char Character { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a default <see cref="Graphics"/> instance
        /// </summary>
        public Graphics() { }

        /// <summary>
        /// Generates a <see cref="Graphics"/> instance with specified character and its color
        /// </summary>
        /// <param name="color">Drawing color</param>
        /// <param name="character">Character that will be drawn</param>
        public Graphics(ConsoleColor color, char character)
        {
            Color = color;
            Character = character;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        public override bool Equals(object obj)
            => obj is Graphics other && other.Color == Color && other.Character == Character;

        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator ==(Graphics left, Graphics right) => Equals(left, right);
        /// <summary>
        /// Determines whether the specified object instances are not considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator !=(Graphics left, Graphics right) => !Equals(left, right);

        /// <summary>
        /// Returns the hash code for the value of this instance
        /// </summary>
        public override int GetHashCode() => Color.GetHashCode() + Character.GetHashCode();

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        public override string ToString() => $"{Character} (Color: {Color})";

        #endregion
    }
}
