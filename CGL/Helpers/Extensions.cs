using System;
using System.Collections.Generic;
using System.Linq;

namespace CGL
{
    /// <summary>
    /// Collection of helpful extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Throw helper for parameters that throws <see cref="ArgumentNullException"/> if the
        /// parameter's value is null
        /// </summary>
        /// <typeparam name="T">Parameter's type</typeparam>
        /// <param name="parameterToTest">Parameter's value</param>
        /// <param name="parameterName">Parameter's name</param>
        /// <exception cref="ArgumentNullException"/>
        public static void ThrowIfNull<T>(this T parameterToTest, string parameterName)
        {
            if (parameterName == null)
                throw new ArgumentNullException(nameof(parameterName));

            if (parameterToTest == null)
                throw new ArgumentNullException(parameterName);
        }

        #region Update

        /// <summary>
        /// Returns an updated collection based on a condition
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to update</param>
        /// <param name="item">Item to put instead of the one that met the condition</param>
        /// <param name="condition">Condition for the update</param>
        /// <param name="updateOnlyOneRecord">Should only the first record that matches the condition be updated</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable<T> Update<T>(this IEnumerable<T> collection, T item, Func<T, bool> condition, bool updateOnlyOneRecord = false)
        {
            Utils.ThrowIfNull((collection, nameof(collection)), (item, nameof(item)), (condition, nameof(condition)));

            var list = collection.ToList();

            list.Update(item, condition, updateOnlyOneRecord);

            return list;
        }

        /// <summary>
        /// Updates the list based on a condition and returns how many items were updated
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to update</param>
        /// <param name="item">Item to put instead of the one that met the condition</param>
        /// <param name="condition">Condition for the update</param>
        /// <param name="updateOnlyOneRecord">Should only the first record that matches the condition be updated</param>
        /// <exception cref="ArgumentNullException"/>
        public static int Update<T>(this IList<T> collection, T item, Func<T, bool> condition, bool updateOnlyOneRecord = false)
        {
            Utils.ThrowIfNull((collection, nameof(collection)), (item, nameof(item)), (condition, nameof(condition)));

            int updates = 0;

            for (int i = 0; i < collection.Count(); i++)
                if (condition(collection[i]))
                {
                    collection[i] = item;
                    updates++;

                    if (updateOnlyOneRecord)
                        return 1;
                }

            return updates;
        }

        #endregion

        #region Between

        /// <summary>
        /// Determines if a number is in the interval
        /// </summary>
        /// <param name="number">Number to test</param>
        /// <param name="left">Left side of the interval</param>
        /// <param name="right">Right side of the interval</param>
        /// <param name="options">Interval inclusion option</param>
        /// <exception cref="UnknownEnumValueException"/>
        public static bool Between(this double number, double left, double right,
            InclusionOptions options = InclusionOptions.BothInclusive)
        {
            switch (options)
            {
                case InclusionOptions.BothInclusive:
                    return number >= left && number <= right;
                case InclusionOptions.OnlyLeftInclusive:
                    return number >= left && number < right;
                case InclusionOptions.OnlyRightInclusive:
                    return number > left && number <= right;
                case InclusionOptions.BothExclusive:
                    return number > left && number < right;
                default:
                    throw new UnknownEnumValueException();
            }
        }

        /// <summary>
        /// Determines if a number is in the interval
        /// </summary>
        /// <param name="number">Number to test</param>
        /// <param name="left">Left side of the interval</param>
        /// <param name="right">Right side of the interval</param>
        /// <param name="options">Interval inclusion option</param>
        /// <exception cref="UnknownEnumValueException"/>
        public static bool Between(this int number, double left, double right,
            InclusionOptions options = InclusionOptions.BothInclusive)
            => Between((double)number, left, right, options);

        #endregion

        /// <summary>
        /// Menu that allows the user to select one of the options. Keys that are used to interact with the menu:
        /// <para><see cref="ConsoleKey.Enter"/>: confirms the choice and returns the chosen option as index or ordinal number 
        /// (based on the parameter <paramref name="index"/>)</para>
        /// <para><see cref="ConsoleKey.W"/> or <see cref="ConsoleKey.UpArrow"/>: Move up</para>
        /// <para><see cref="ConsoleKey.S"/> or <see cref="ConsoleKey.DownArrow"/>: Move down</para>
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="options">Collection with options (ToString() is used for displaying the options)</param>
        /// <param name="title">Menu header</param>
        /// <param name="index">Should the index be returned or the ordinal number of the chosen option</param>
        /// <param name="message">Optional message to display under the title / header</param>
        /// <param name="clearAfter">Should the console be cleared after an option is chosen</param>
        /// <exception cref="ArgumentNullException"/>
        public static int ChooseOption<T>(this IEnumerable<T> options, string title, bool index, string message = null, bool clearAfter = true)
        {
            Utils.ThrowIfNull((options, nameof(options)), (title, nameof(title)));

            int choice = 0;
            int numOptions = options.Count();

            while (true)
            {

                Console.Clear();
                Utils.PrintUnderlined(title);

                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine();
                    Console.WriteLine(message);
                    Console.WriteLine();
                }

                for (int i = 0; i < numOptions; i++)
                    Utils.Draw(
                        $"{options.ElementAt(i)}",
                        i == choice
                            ? ConsoleColor.Cyan
                            : Console.ForegroundColor,
                        true
                    );

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        if (clearAfter)
                            Console.Clear();

                        return index ? choice : choice + 1;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (choice < numOptions - 1)
                            choice++;
                        break;

                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if (choice > 0)
                            choice--;
                        break;
                }

            }
        }
    }
}
