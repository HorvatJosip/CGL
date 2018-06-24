using System;

using CGL.Entities;

namespace CGL.Board
{
    /// <summary>
    /// Unit on the terrain (<see cref="Board"/>)
    /// </summary>
    public class Tile : Entity
    {
        #region Properties

        /// <summary>
        /// Type of this tile
        /// </summary>
        public TileType Type { get; set; }

        /// <summary>
        /// Object that lies on this tile
        /// </summary>
        public object Contents { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a default <see cref="Tile"/> instance
        /// </summary>
        public Tile() { }

        /// <summary>
        /// Generates a <see cref="Tile"/> instance with the specified type and graphics
        /// </summary>
        /// <param name="type">Type of the tile</param>
        /// <param name="graphics">Tile graphics</param>
        /// <exception cref="ArgumentNullException"/>
        public Tile(TileType type, Graphics graphics)
            : this(type, new Printables.Drawable(graphics, Position.Zero)) { }

        /// <summary>
        /// Generates a <see cref="Tile"/> based on an existing tile, but at a new location
        /// </summary>
        /// <param name="tile">Tile with its type and graphics specified</param>
        /// <param name="newPosition">The new position of the given tile</param>
        /// <exception cref="ArgumentNullException"/>
        public Tile(Tile tile, Position newPosition)
            : this(tile?.Type ?? 0, tile?.Graphics, newPosition) { }

        /// <summary>
        /// Generates a <see cref="Tile"/> instance with the specified type and graphics at a specified location
        /// </summary>
        /// <param name="type">Type of the tile</param>
        /// <param name="graphics">Tile graphics</param>
        /// <param name="position">Tile's location on the map</param>
        /// <exception cref="ArgumentNullException"/>
        public Tile(TileType type, Graphics graphics, Position position)
            : this(type, new Printables.Drawable(graphics, position)) { }

        /// <summary>
        /// Generates a <see cref="Tile"/> instance with the specified type and graphics and position
        /// from an existing entity
        /// </summary>
        /// <param name="type">Type of the tile</param>
        /// <param name="entity">Entity whose graphics and position will be used for this tile</param>
        /// <exception cref="ArgumentNullException"/>
        public Tile(TileType type, Entity entity) : base(entity?.Graphics, entity?.Position) => Type = type;

        #endregion
    }
}
