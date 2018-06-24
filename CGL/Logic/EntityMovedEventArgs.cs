using System;

using CGL.Board;
using CGL.Entities;

namespace CGL
{
    /// <summary>
    /// Event arguments for events that occurr on some type of entity movement
    /// </summary>
    public class EntityMovementEventArgs
    {
        #region Properties

        /// <summary>
        /// Entity that caused the movement
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// Tile that the entity was moved on or from
        /// </summary>
        public Tile Tile { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Generates a <see cref="EntityMovementEventArgs"/> instance with the entity that caused the movement
        /// and the tile that it was moved on or that it moved from
        /// </summary>
        /// <param name="entity">Entity that caused the movement</param>
        /// <param name="tile">Tile that the entity was moved on or from</param>
        /// <exception cref="ArgumentNullException"/>
        public EntityMovementEventArgs(Entity entity, Tile tile)
        {
            entity.ThrowIfNull(nameof(entity));

            Entity = entity;
            Tile = tile;
        } 

        #endregion
    }
}
