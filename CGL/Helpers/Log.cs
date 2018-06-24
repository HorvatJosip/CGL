using System;
using System.IO;
using System.Text;

namespace CGL
{
    /// <summary>
    /// Used for logging to a file
    /// </summary>
    public static class Log
    {
        #region Fields

        private const int shortLineLength = 25;
        private const int longLineLength = 50;

        #endregion

        #region Properties

        #region Private

        private static string ShortLine => new string(LogChar, shortLineLength);
        private static string LongLine => new string(LogChar, longLineLength);

        #endregion

        #region Public

        /// <summary>
        /// Path to the log file
        /// </summary>
        public static string LogPath { get; set; }

        /// <summary>
        /// Character used for "lines" in the log (e.g. === Title ===)
        /// </summary>
        public static char LogChar { get; set; } = '=';

        /// <summary>
        /// Should the log file be opened once an entry has been written to it
        /// </summary>
        public static bool OpenOnWrite { get; set; }

        #endregion

        #endregion Properties

        #region Constructor

        static Log()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            LogPath = Path.Combine(desktop, "Game Log.txt");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Writes an entry into the log file that contains the date and time when it was written,
        /// a title and the message
        /// </summary>
        /// <param name="title">Title of the entry</param>
        /// <param name="message">Message of the entry</param>
        /// <exception cref="ArgumentNullException"/>
        public static void WriteEntry(string title, string message)
        {
            Utils.ThrowIfNull((title, nameof(title)), (message, nameof(message)));

            try
            {
                var entry = new StringBuilder($"{ShortLine} {DateTime.Now} {ShortLine}")
                                                    .AppendLine()
                                                    .AppendLine();

                entry.AppendLine(title);
                entry.AppendLine(new string(LogChar, title.Length)).AppendLine();

                entry.AppendLine(message).AppendLine();

                WriteLine(entry.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Writes an entry into the log file that describes an exception that occurred
        /// </summary>
        /// <param name="exception">Exception to log</param>
        /// <exception cref="ArgumentNullException"/>
        public static void WriteEntry(Exception exception)
        {
            exception.ThrowIfNull(nameof(exception));

            var entry = new StringBuilder($"{ShortLine} Exception START {ShortLine}")
                                .AppendLine(Environment.NewLine);

            while (exception != null)
            {
                entry
                    .AppendLine($"Error message: {exception.Message}")
                    .AppendLine();
                entry
                    .AppendLine("Stack trace:")
                    .AppendLine(exception.StackTrace);
            }

            entry
                .AppendLine()
                .AppendLine($"{ShortLine} Exception END {ShortLine}")
                .AppendLine();

            WriteEntry("Exception Occurred", entry.ToString());
        }

        /// <summary>
        /// Writes a line of text into the log
        /// </summary>
        /// <param name="line">Text to write to the log</param>
        /// <exception cref="ArgumentNullException"/>
        public static void WriteLine(string line)
        {
            line.ThrowIfNull(nameof(line));

            try
            {
                File.AppendAllText(LogPath, line + Environment.NewLine);

                if (OpenOnWrite)
                    System.Diagnostics.Process.Start(LogPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex?.Message);
            }
        }

        /// <summary>
        /// Clears the contents of the file
        /// </summary>
        public static void Clear()
        {
            try
            {
                File.WriteAllText(LogPath, "");

                if (OpenOnWrite)
                    System.Diagnostics.Process.Start(LogPath);
            }
            catch (Exception ex) { WriteEntry(ex); }
        }

        #endregion
    }
}
