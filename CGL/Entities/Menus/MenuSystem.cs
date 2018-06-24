using System;
using System.Collections.Generic;

namespace CGL.Menus
{
    /// <summary>
    /// Manager for <see cref="Menu"/>s
    /// </summary>
    /// <typeparam name="TKey">Key type, unique identifiers for each menu</typeparam>
    public class MenuSystem<TKey>
    {
        private IDictionary<TKey, Menu> menus = new Dictionary<TKey, Menu>();

        /// <summary>
        /// Gets a menu at the given key
        /// </summary>
        /// <param name="key">Key to use to find the menu</param>
        /// <exception cref="ArgumentNullException"/>
        public Menu this[TKey key]
        {
            get
            {
                menus.TryGetValue(key, out Menu menu);

                return menu;
            }
        }

        #region Methods

        /// /// <summary>
        /// Performs an operation on the menus:
        /// <para><see cref="EditOperation.Create"/>: adds a new menu</para>
        /// <para><see cref="EditOperation.Update"/>: updates an existing menu</para>
        /// <para><see cref="EditOperation.Delete"/>: deletes a menu (this doesn't require <paramref name="menu"/> parameter)</para>
        /// <para><see cref="EditOperation.UpdateOrCreate"/>: if the menu already exists, it updates it, 
        /// otherwise, it creates a new one</para>
        /// </summary>
        /// <param name="operation">Edit operation to perform</param>
        /// <param name="menuKey">Key of the menu</param>
        /// <param name="menu">The menu itself</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool EditMenus(EditOperation operation, TKey menuKey, Menu menu)
        {
            menu.ThrowIfNull(nameof(menu));

            switch (operation)
            {
                case EditOperation.Create:
                    return AddMenu(menuKey, menu);
                case EditOperation.Update:
                    return EditMenu(menuKey, menu);
                case EditOperation.Delete:
                    return menus.Remove(menuKey);
                case EditOperation.UpdateOrCreate:
                    return EditMenu(menuKey, menu)
                        ? true
                        : AddMenu(menuKey, menu);
                default:
                    throw new UnknownEnumValueException();
            }
        }

        #region Helpers

        private bool AddMenu(TKey menuKey, Menu menu)
        {
            if (menus.ContainsKey(menuKey))
                return false;

            menus.Add(menuKey, menu);
            return true;
        }

        private bool EditMenu(TKey menuKey, Menu newMenu)
        {
            if (!menus.ContainsKey(menuKey))
                return false;

            menus[menuKey] = newMenu;
            return true;
        }  

        #endregion

        #endregion
    }
}
