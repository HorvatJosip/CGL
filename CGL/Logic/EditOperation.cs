namespace CGL
{
    /// <summary>
    /// Operations usually done on the collections
    /// </summary>
    public enum EditOperation
    {
        /// <summary>
        /// Create / Add
        /// </summary>
        Create,

        /// <summary>
        /// Update / Edit
        /// </summary>
        Update,

        /// <summary>
        /// Delete / Remove
        /// </summary>
        Delete,

        /// <summary>
        /// If it exists, update / edit it, otherwise, create / add it
        /// </summary>
        UpdateOrCreate
    }
}
