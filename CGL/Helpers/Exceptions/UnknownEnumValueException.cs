using System;

namespace CGL
{
    /// <summary>
    /// Exception that should be thrown when an unknown enum value was specified
    /// </summary>
    public class UnknownEnumValueException : Exception
    {
        #region Constructors

        /// <summary>
        /// Generates a default <see cref="UnknownEnumValueException"/> instance
        /// </summary>
        public UnknownEnumValueException() : base("The specified enum value doesn't exist.")
        {
        }

        /// <summary>
        /// Generates a <see cref="UnknownEnumValueException"/> instance with a specified message
        /// </summary>
        public UnknownEnumValueException(string message) : base(message)
        {
        }

        #endregion
    }
}
