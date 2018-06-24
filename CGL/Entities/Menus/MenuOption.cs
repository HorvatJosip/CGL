using System;

namespace CGL.Menus
{
    /// <summary>
    /// An option inside the <see cref="Menu"/>
    /// </summary>
    public abstract class MenuOption
    {
        #region Properties

        /// <summary>
        /// Label of the menu option
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Action that is executed when the menu option is chosen
        /// </summary>
        public Action Action { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Generates a <see cref="MenuOption"/> instance with the given label and action
        /// </summary>
        /// <param name="label">Label of the menu option</param>
        /// <param name="action">Action that is executed when the menu option is chosen</param>
        /// <exception cref="ArgumentNullException"/>
        public MenuOption(string label, Action action)
        {
            Utils.ThrowIfNull((label, nameof(label)), (action, nameof(action)));

            Label = label;
            Action = action;
        }

        #endregion

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        public override string ToString() => Label;
    }
}
