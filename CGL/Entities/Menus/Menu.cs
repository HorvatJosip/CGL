using System;
using System.Collections.Generic;

namespace CGL.Menus
{
    /// <summary>
    /// List of options that the user can pick
    /// </summary>
    public class Menu
    {
        //private Rectangle renderedArea;
        private Menu parent;

        #region Properties

        /// <summary>
        /// Parent of the current menu, for example, Main menu can be parent of the Options menu
        /// </summary>
        public Menu Parent
        {
            get => parent;
            set
            {
                bool remove = false, add = false;

                if (parent != null && value == null)
                {
                    parent = null;
                    remove = true;
                }
                else if (parent == null && value != null)
                    add = true;
                else if (parent != null && value != null)
                {
                    remove = true;
                    add = true;
                }
                else return;

                if (remove)
                    Options.RemoveAt(Options.Count - 1);

                if (add)
                {
                    Type menuOptionType = (Options != null && Options.Count > 0)
                        ? Options[0].GetType()
                        : typeof(LabelledMenuOption);

                    Options.Add((MenuOption)Activator.CreateInstance(menuOptionType, $"Go back to {value.Title}", (Action)value.Print));
                }
            }
        }

        /// <summary>
        /// List of options that the user can choose from
        /// </summary>
        public List<MenuOption> Options { get; set; } = new List<MenuOption>();

        /// <summary>
        /// Title of the menu
        /// </summary>
        public string Title { get; set; } = "My Menu";

        #endregion

        #region Constructors

        /// <summary>
        /// Generates a <see cref="Menu"/> instance with no options to choose from
        /// </summary>
        public Menu() { }

        //public Menu(Rectangle renderedArea) => this.renderedArea = renderedArea;

        /// <summary>
        /// Generates a <see cref="Menu"/> instance with specified options
        /// </summary>
        /// <param name="options">List of options that the user can choose from</param>
        /// <exception cref="ArgumentNullException"/>
        public Menu(/*Rectangle renderedArea, */IEnumerable<MenuOption> options)
        /*: this(renderedArea)*/
        {
            options.ThrowIfNull(nameof(options));

            Options = new List<MenuOption>(options);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the console and prints the menu using the
        /// <see cref="Extensions.ChooseOption{T}(IEnumerable{T}, string, bool, string, bool)"/> method
        /// and executes the chosen option
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public void Print()
        {
            Console.Clear();

            int choice = Options.ChooseOption(Title, true);

            Options[choice].Action?.Invoke();
        }

        #endregion
    }
}
