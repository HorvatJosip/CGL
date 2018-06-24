using System;

namespace CGL.Menus
{
    /// <summary>
    /// Menu option that has text (a label) as its representation
    /// </summary>
    public class LabelledMenuOption : MenuOption
    {
        #region Constructor

        /// <summary>
        /// Generates a <see cref="LabelledMenuOption"/> instance with the given label and action
        /// </summary>
        /// <param name="label">Label of the menu option</param>
        /// <param name="action">Action to perform when this option is chosen</param>
        /// <exception cref="ArgumentNullException"/>
        public LabelledMenuOption(string label, Action action) : base(label, action)
        {
        } 

        #endregion
    }
}
