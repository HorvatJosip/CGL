using System;
using System.Collections.Generic;
using System.Linq;

using CGL.Board;
using CGL.Entities;
using CGL.Printables;

namespace CGL
{
    /// <summary>
    /// Collection of various helpful utilities
    /// </summary>
    public static class Utils
    {
        #region Properties

        /// <summary>
        /// Random Number Generator
        /// </summary>
        public static Random Rng { get; } = new Random();

        #endregion

        #region Methods

        #region Input

        /// <summary>
        /// Asks the user to enter an integer until they enter one that is within the given range
        /// </summary>
        /// <param name="message">Input description</param>
        /// <param name="min">Minimum integer that the user can enter</param>
        /// <param name="max">Maximum integer that the user can enter</param>
        /// <exception cref="ArgumentNullException"/>
        public static int ReadInt(string message, int min = int.MinValue, int max = int.MaxValue)
        {
            message.ThrowIfNull(nameof(message));

            message += (min == int.MinValue && max == int.MaxValue)
                ? ": "
                : $" ({min} - {max}): ";

            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int input) && input.Between(min, max))
                    return input;
            }
        }

        /// <summary>
        /// Asks the user to enter a value of type <typeparamref name="T"/>. It uses
        /// the Parse method that takes in a string as a parameter so in order to use this method, type
        /// <typeparamref name="T"/> has to have a static method like <see cref="int.Parse(string)"/>
        /// </summary>
        /// <typeparam name="T">Type that the user should enter</typeparam>
        /// <param name="message"></param>
        /// <param name="allowedOptions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        public static T Read<T>(string message, params T[] allowedOptions)
        {
            ThrowIfNull((message, nameof(message)), (allowedOptions, nameof(allowedOptions)));

            var method = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });
            var methodParams = method.GetParameters();

            if (!(
                    method != null &&
                    method.ReturnType == typeof(T) &&
                    methodParams.Length == 1 &&
                    methodParams[0].ParameterType == typeof(string)
                ))
                return default(T);

            while (true)
            {
                Console.Write(message + ": ");

                try
                {
                    T input = (T)method.Invoke(null, new object[] { Console.ReadLine() });

                    if (allowedOptions.Contains(input))
                        return input;
                }
                catch (Exception ex)
                {
                    Log.WriteEntry(ex);
                }
            }
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draws text onto the screen
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <param name="resetColor">Should the color be reset after the drawing is finished</param>
        /// <exception cref="ArgumentNullException"/>
        public static bool Draw(IEnumerable<Drawable> text, bool resetColor)
        {
            text.ThrowIfNull(nameof(text));

            try
            {
                foreach (var letter in text)
                    Draw(letter, false);

                if (resetColor)
                    Console.ResetColor();

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ex);
                return false;
            }
        }

        /// <summary>
        /// Draws an entity onto the screen
        /// </summary>
        /// <param name="entity">Entity to draw</param>
        /// <param name="resetColor">Should the color be reset after the drawing is finished</param>
        /// <exception cref="ArgumentNullException"/>
        public static bool Draw(Entity entity, bool resetColor)
        {
            entity.ThrowIfNull(nameof(entity));

            try
            {
                Console.SetCursorPosition(entity.X, entity.Y);
                Draw(entity.Graphics, resetColor);

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ex);
                return false;
            }
        }

        /// <summary>
        /// Draws graphics object onto the screen
        /// </summary>
        /// <param name="graphics">Graphics object to draw</param>
        /// <param name="resetColor">Should the color be reset after the drawing is finished</param>
        /// <exception cref="ArgumentNullException"/>
        public static bool Draw(Graphics graphics, bool resetColor)
        {
            graphics.ThrowIfNull(nameof(graphics));

            try
            {
                Console.ForegroundColor = graphics.Color;
                Console.Write(graphics.Character);

                if (resetColor)
                    Console.ResetColor();

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ex);
                return false;
            }
        }

        /// <summary>
        /// Draws text onto the screen in a certain color
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <param name="color">Drawing color</param>
        /// <param name="resetColor">Should the color be reset after the drawing is finished</param>
        /// <param name="appendNewLine">Should a new line be added after the text is drawn</param>
        /// <exception cref="ArgumentNullException"/>
        public static bool Draw(string text, ConsoleColor color, bool resetColor, bool appendNewLine = true)
        {
            text.ThrowIfNull(nameof(text));

            try
            {
                Console.ForegroundColor = color;

                if (appendNewLine)
                    Console.WriteLine(text);
                else
                    Console.Write(text);

                if (resetColor)
                    Console.ResetColor();

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ex);
                return false;
            }
        }

        /// <summary>
        /// Prints an underlined message
        /// </summary>
        /// <param name="addLineBefore">Should a line be added before the message</param>
        /// <param name="addLineAfter">Should a line be added after the message</param>
        /// <param name="message">Message to print</param>
        /// <param name="underline">Character used to underline the message</param>
        /// <param name="messageColor">Color of the message</param>
        /// <param name="underlineColor">Color of the underline character</param>
        /// <exception cref="ArgumentNullException"/>
        public static void PrintUnderlined(bool addLineBefore, bool addLineAfter, string message, char underline = '=',
            ConsoleColor messageColor = default(ConsoleColor), ConsoleColor underlineColor = default(ConsoleColor))
        {
            message.ThrowIfNull(message);

            if (addLineBefore)
                Console.WriteLine();

            foreach (char c in message) Draw(new Graphics(messageColor, c), false);
            Console.WriteLine();

            var underlineGraphics = new Graphics(underlineColor, underline);
            for (int i = 0; i < message.Length; i++) Draw(underlineGraphics, false);
            Console.WriteLine();

            if (addLineAfter)
                Console.WriteLine();
        }

        /// <summary>
        /// Prints an underlined message
        /// </summary>
        /// <param name="addLineBefore">Should a line be added before the message</param>
        /// <param name="addLineAfter">Should a line be added after the message</param>
        /// <param name="message">Message to print</param>
        /// <param name="underline">Character used to underline the message</param>
        /// <exception cref="ArgumentNullException"/>
        public static void PrintUnderlined(bool addLineBefore, bool addLineAfter, string message, char underline = '=')
            => PrintUnderlined(addLineBefore, addLineAfter, message, underline, Console.ForegroundColor, Console.ForegroundColor);

        /// <summary>
        /// Prints an underlined message
        /// </summary>
        /// <param name="message">Message to print</param>
        /// <param name="underline">Character used to underline the message</param>
        /// <exception cref="ArgumentNullException"/>
        public static void PrintUnderlined(string message, char underline = '=')
            => PrintUnderlined(true, false, message, underline);

        #endregion

        #region Generation

        /// <summary>
        /// Generates a random enum value from a given enum type
        /// </summary>
        /// <typeparam name="TEnum">An enum type</typeparam>
        /// <param name="exclusions">Optionally passed in collection of unwanted generated values</param>
        public static TEnum GetRandomEnumValue<TEnum>(params TEnum[] exclusions)
        {
            if (!typeof(TEnum).IsEnum)
                return default(TEnum);

            var values = (TEnum[])Enum.GetValues(typeof(TEnum));

            while (true)
            {
                var value = values[Rng.Next(values.Length)];

                if (!exclusions.Contains(value))
                    return value;
            }
        }

        /// <summary>
        /// Generates a chunk of walkable tiles in based on the given parameters that can be surrounded by a border
        /// </summary>
        /// <param name="graphics">Walkable tile graphics</param>
        /// <param name="startX">Where to start the tiles on the X axis</param>
        /// <param name="startY">Where to start the tiles on the Y axis</param>
        /// <param name="width">Width of the tile chunk</param>
        /// <param name="height">Height of the tile chunk</param>
        /// <param name="borderGraphics">Border tile graphics (if you don't want a border, leave this null)</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable<Entity> GenerateTileChunk(Graphics graphics, int startX, int startY, int width, int height,
            Graphics borderGraphics = null)
        {
            graphics.ThrowIfNull(nameof(graphics));

            var entities = new List<Entity>();

            var endX = startX + width - 1;
            var endY = startY + height - 1;

            for (int i = startX; i <= endX; i++)
                for (int j = startY; j <= endY; j++)
                {
                    var position = new Position(i, j);

                    var tile = borderGraphics != null && (i == startX || i == endX || j == startY || j == endY)
                        ? new Tile(TileType.None, borderGraphics, position)
                        : new Tile(TileType.Walkable, graphics, position);

                    entities.Add(tile);
                }

            return entities;
        }

        /// <summary>
        /// Generates a chunk of walkable tiles in based on the given parameters that can be surrounded by a border
        /// </summary>
        /// <param name="graphics">Walkable tile graphics</param>
        /// <param name="tileChunkArea">Area where the tile chunk will be generated</param>
        /// <param name="borderGraphics">Border tile graphics (if you don't want a border, leave this null)</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable<Entity> GenerateTileChunk(Graphics graphics, Rectangle tileChunkArea, Graphics borderGraphics = null)
        {
            ThrowIfNull((graphics, nameof(graphics)), (tileChunkArea, nameof(tileChunkArea)));

            return GenerateTileChunk(graphics, tileChunkArea.X, tileChunkArea.Y, tileChunkArea.Width, tileChunkArea.Height, borderGraphics);
        }

        #endregion

        /// <summary>
        /// Helper for throwing <see cref="ArgumentNullException"/> on multiple parameters
        /// </summary>
        /// <param name="paramsToTest">Parameters to check for null values</param>
        /// <exception cref="ArgumentNullException"/>
        public static void ThrowIfNull(params (object paramToTest, string parameterName)[] paramsToTest)
        {
            paramsToTest.ThrowIfNull(nameof(paramsToTest));

            foreach (var (paramToTest, parameterName) in paramsToTest)
                paramToTest.ThrowIfNull(parameterName);
        }

        #endregion Methods
    }
}
