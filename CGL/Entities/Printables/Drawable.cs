using System;

using CGL.Entities;

namespace CGL.Printables
{
    /// <summary>
    /// An object that just derives from the abstract class <see cref="Entity"/>
    /// for drawing purposes. It doesn't implement any special logic.
    /// </summary>
    public class Drawable : Entity
    {
        #region Constructors

        /// <summary>
        /// Generates a default <see cref="Drawable"/> instance
        /// </summary>
        public Drawable() { }

        /// <summary>
        /// Generates a <see cref="Drawable"/> instance with the specified graphics and position
        /// </summary>
        /// <param name="graphics">Graphics of the letter</param>
        /// <param name="position">The letter's location</param>
        /// <exception cref="ArgumentNullException"/>
        public Drawable(Graphics graphics, Position position) : base(graphics, position)
        {
        } 

        #endregion
    }
}
