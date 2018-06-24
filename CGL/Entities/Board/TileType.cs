namespace CGL.Board
{
    /// <summary>
    /// Flags that define the type of a certain <see cref="Tile"/>
    /// </summary>
    [System.Flags]
    public enum TileType
    {
        /// <summary>
        /// No type
        /// </summary>
        None = 0,

        /// <summary>
        /// Entities can walk on the tile
        /// </summary>
        Walkable = 1 << 0,

        /// <summary>
        /// Entities can gather something from the tile
        /// </summary>
        Gatherable = 1 << 1,
        
        /// <summary>
        /// Something happens when an entity hits the tile
        /// </summary>
        Interactable = 1 << 2,

        /// <summary>
        /// For custom logic
        /// </summary>
        Custom = 1 << 3
    }
}
