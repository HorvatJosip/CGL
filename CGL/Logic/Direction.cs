namespace CGL
{
    /// <summary>
    /// Flags that define sides or directions
    /// </summary>
    [System.Flags]
    public enum Direction
    {
        /// <summary>
        /// No side or direction
        /// </summary>
        None = 0,

        /// <summary>
        /// Up or forward
        /// </summary>
        Up = 1 << 0,

        /// <summary>
        /// Down or backward
        /// </summary>
        Down = 1 << 1,

        /// <summary>
        /// Left or towards the left side
        /// </summary>
        Left = 1 << 2,

        /// <summary>
        /// Right or towards the right side
        /// </summary>
        Right = 1 << 3
    }
}
