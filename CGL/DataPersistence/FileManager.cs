using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CGL.DataPersistence
{
    /// <summary>
    /// Used for defining working directory and serializing / deserializing inside it
    /// </summary>
    public static class FileManager
    {
        private static BinaryFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// Path to the directory where the files are located
        /// </summary>
        public static string DirectoryPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        #region Methods

        /// <summary>
        /// Gets a file path based on a given file name or file path.
        /// <para>If a path to a file is passed in, it is just returned, otherwise
        /// it will be combined with the <see cref="DirectoryPath"/>.</para>
        /// </summary>
        /// <param name="file">File name or file path</param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public static string GetFilePath(string file)
        {
            file.ThrowIfNull(nameof(file));

            return File.Exists(file)
                   ? file
                   : Path.Combine(DirectoryPath, file);
        }

        /// <summary>
        /// Loads serialized data from a file
        /// </summary>
        /// <typeparam name="T">Type of the object that is serialized in the given file</typeparam>
        /// <param name="file">File name or file path</param>
        /// <exception cref="ArgumentNullException"/>
        public static T LoadData<T>(string file)
        {
            file.ThrowIfNull(nameof(file));

            try
            {
                using (var stream = File.OpenRead(GetFilePath(file)))
                {
                    var data = formatter.Deserialize(stream);

                    return data is T loadedData ? loadedData : default(T);
                }
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ex);
                return default(T);
            }
        }

        /// <summary>
        /// Serializes an object to a file
        /// </summary>
        /// <param name="file">File name or file path</param>
        /// <param name="data">Object with data to serialize (make sure it is marked with <see cref="SerializableAttribute"/>)</param>
        /// <exception cref="ArgumentNullException"/>
        public static bool SaveData(string file, object data)
        {
            Utils.ThrowIfNull((file, nameof(file)), (data, nameof(data)));

            try
            {
                using (var stream = File.OpenWrite(GetFilePath(file)))
                {
                    formatter.Serialize(stream, data);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.WriteEntry(ex);
                return false;
            }
        }

        #endregion
    }
}
