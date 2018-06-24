using System;

namespace CGL.Entities
{
    /// <summary>
    /// Base for anything that exists within the game
    /// </summary>
    public abstract class Entity
    {
        #region Properties

        /// <summary>
        /// Graphics used to draw the entity
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// Location where the entity should be drawn
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// X coordinate of the <see cref="Position"/>
        /// </summary>
        /// <exception cref="NullReferenceException"/>
        public int X => Position.X;
        /// <summary>
        /// Y coordinate of the <see cref="Position"/>
        /// </summary>
        /// <exception cref="NullReferenceException"/>
        public int Y => Position.Y;

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a default <see cref="Entity"/> instance
        /// </summary>
        public Entity() { }

        /// <summary>
        /// Generates an <see cref="Entity"/> instance with specified graphics and position
        /// </summary>
        /// <param name="graphics">Graphics used to draw the entity</param>
        /// <param name="position">Location where the entity should be drawn</param>
        /// <exception cref="ArgumentNullException"/>
        public Entity(Graphics graphics, Position position)
        {
            Utils.ThrowIfNull((graphics, nameof(graphics)), (position, nameof(position)));

            Graphics = graphics;
            Position = position;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        /// <param name="obj">The object to compare with the current object</param>
        public override bool Equals(object obj)
            => obj is Entity other && other.Position == Position && other.Graphics == Graphics;

        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator ==(Entity left, Entity right) => Equals(left, right);
        /// <summary>
        /// Determines whether the specified object instances are not considered equal
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        public static bool operator !=(Entity left, Entity right) => !Equals(left, right);

        /// <summary>
        /// Returns the hash code for the value of this instance
        /// </summary>
        public override int GetHashCode() => Graphics.GetHashCode() + Position.GetHashCode();

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        public override string ToString() => $"{Graphics}; Position: {Position}";

        #endregion
    }
}
