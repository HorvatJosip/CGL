using CGL.Entities;
using System;

namespace CGL.Characters
{
    /// <summary>
    /// Simple character base (has HP, MP and movement speed)
    /// </summary>
    public abstract class Character : Entity
    {
        #region Properties

        /// <summary>
        /// Health / Hit Points maximum
        /// </summary>
        public int MaxHP { get; set; } = 100;

        /// <summary>
        /// Health / Hit Points
        /// </summary>
        public int HP { get; set; } = 100;

        /// <summary>
        /// Mana Points maximum
        /// </summary>
        public int MaxMP { get; set; } = 100;

        /// <summary>
        /// Mana Points
        /// </summary>
        public int MP { get; set; } = 100;

        /// <summary>
        /// Number of tiles this character can traverse at once
        /// </summary>
        public int MovementSpeed { get; set; } = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a default <see cref="Character"/> instance
        /// </summary>
        public Character() { }

        /// <summary>
        /// Generates a <see cref="Character"/> instance with specified graphics and position
        /// and the default stats
        /// </summary>
        /// <param name="graphics">Character's graphics</param>
        /// <param name="position">Character's position</param>
        /// <exception cref="ArgumentNullException"/>
        public Character(Graphics graphics, Position position) : base(graphics, position)
        {
        }

        /// <summary>
        /// Generates a <see cref="Character"/> instance with specified graphics, position and stats
        /// </summary>
        /// <param name="graphics">Character's graphics</param>
        /// <param name="position">Character's position</param>
        /// <param name="hp">Character's Health Points total</param>
        /// <param name="mana">Character's Mana Points total</param>
        /// <param name="movementSpeed">Character's movement speed</param>
        /// <exception cref="ArgumentNullException"/>
        public Character(Graphics graphics, Position position, int hp, int mana, int movementSpeed)
            : base(graphics, position)
        {
            HP = hp;
            MP = mana;
            MaxHP = hp;
            MaxMP = mana;
            MovementSpeed = movementSpeed;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Causes the character to lose hp. The method returns true if the character is dead
        /// </summary>
        /// <param name="amount">Damage amount that the character will take</param>
        public bool TakeDamage(int amount)
        {
            HP -= amount;
            return HP < 0;
        }

        #endregion
    }
}
