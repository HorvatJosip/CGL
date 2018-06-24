using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CGL.DataPersistence
{
    /// <summary>
    /// Used for managing saves (using <see cref="FileManager"/> to manage save files)
    /// </summary>
    /// <typeparam name="T">Type of object that holds save information</typeparam>
    public class SaveSystem<T>
    {
        #region Fields

        private string fileExtension;
        private Dictionary<string, T> saveFiles = new Dictionary<string, T>();

        #endregion

        /// <summary>
        /// Deserialized objects from files (in <see cref="FileManager.DirectoryPath"/> with extension given at constructor)
        /// </summary>
        public List<T> Saves => saveFiles.Values.ToList();

        #region Constructors

        /// <summary>
        /// Generates a <see cref="SaveSystem{T}"/> instance with the specified file extension.
        /// Save files in the default directory (<see cref="FileManager.DirectoryPath"/>) with the given extension
        /// are loaded into <see cref="Saves"/> collection.
        /// </summary>
        /// <param name="fileExtension">File extensions for the save files</param>
        /// <exception cref="ArgumentNullException"/>
        public SaveSystem(string fileExtension) : this(null, fileExtension) { }

        /// <summary>
        /// Generates a <see cref="SaveSystem{T}"/> instance with the specified directory path and file extension.
        /// Save files in the specified directory (that overrides <see cref="FileManager.DirectoryPath"/> value) with the 
        /// given extension are loaded into <see cref="Saves"/> collection.
        /// </summary>
        /// <param name="directoryPath">Path to the directory that stores save files</param>
        /// <param name="fileExtension">File extensions for the save files</param>
        /// <exception cref="ArgumentNullException"/>
        public SaveSystem(string directoryPath, string fileExtension)
        {
            fileExtension.ThrowIfNull(nameof(fileExtension));

            if (fileExtension.Contains('.'))
                fileExtension = fileExtension.Substring(fileExtension.LastIndexOf('.') + 1);

            this.fileExtension = fileExtension;

            if (directoryPath != null)
            {
                try
                {
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    FileManager.DirectoryPath = directoryPath;
                }
                catch (Exception ex) { Log.WriteEntry(ex); }
            }

            var files = Directory.GetFiles(FileManager.DirectoryPath, $"*.{fileExtension}");

            foreach (var file in files)
            {
                try
                {
                    saveFiles.Add(file, FileManager.LoadData<T>(file));
                }
                catch { Log.WriteLine($"Failed to load data from {file}."); }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an operation on the save files:
        /// <para><see cref="EditOperation.Create"/>: adds a new save</para>
        /// <para><see cref="EditOperation.Update"/>: updates an existing save</para>
        /// <para><see cref="EditOperation.Delete"/>: deletes a save (this doesn't require <paramref name="data"/> parameter)</para>
        /// <para><see cref="EditOperation.UpdateOrCreate"/>: if the save file already exists, it updates it, 
        /// otherwise, it creates a new one</para>
        /// </summary>
        /// <param name="operation">Edit operation to perform</param>
        /// <param name="fileName">Name of the file to save the data to</param>
        /// <param name="data">Data that is serialized to the file (not required for deleting a save)</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnknownEnumValueException"/>
        public bool EditSaves(EditOperation operation, string fileName, T data)
        {
            Utils.ThrowIfNull((fileName, nameof(fileName)), (data, nameof(data)));

            if (fileName.Contains('.'))
                fileName = fileName.Substring(fileName.IndexOf('.') + 1);

            fileName += $".{fileExtension}";

            var path = FileManager.GetFilePath(fileName);

            switch (operation)
            {
                case EditOperation.Create:
                    return AddSave(path, data);
                case EditOperation.Update:
                    return UpdateSave(path, data);
                case EditOperation.Delete:
                    return RemoveSave(path);
                case EditOperation.UpdateOrCreate:
                    if (saveFiles.ContainsKey(path))
                        return UpdateSave(path, data);
                    else
                        return AddSave(path, data);
                default:
                    throw new UnknownEnumValueException();
            }
        }

        #region Saving

        /// <summary>
        /// Serializes the data from saves to their respectful file paths.
        /// <para>This method should be called only when you want to serialize all of the saves</para>
        /// </summary>
        public bool Save()
        {
            foreach (var saveFile in saveFiles)
                if (!FileManager.SaveData(saveFile.Key, saveFile.Value))
                    return false;

            return true;
        }

        /// <summary>
        /// Serializes a save at the specific index in the <see cref="Saves"/> collection.
        /// </summary>
        /// <param name="saveIndex">Index of the save to serialize</param>
        /// <exception cref="UnknownEnumValueException"/>
        public bool Save(int saveIndex)
        {
            if (saveFiles.Count == 0 || !saveIndex.Between(0, saveFiles.Count))
                return false;

            var saveFile = saveFiles.ElementAt(saveIndex);

            return FileManager.SaveData(saveFile.Key, saveFile.Value);
        } 

        #endregion

        #region Helpers

        private bool UpdateSave(string path, T data)
        {
            Utils.ThrowIfNull((path, nameof(path)), (data, nameof(data)));

            if (!saveFiles.ContainsKey(path))
                return false;

            saveFiles[path] = data;
            return true;
        }

        private bool AddSave(string path, T data)
        {
            Utils.ThrowIfNull((path, nameof(path)), (data, nameof(data)));

            if (saveFiles.ContainsKey(path))
                return false;

            if (FileManager.SaveData(path, data))
            {
                saveFiles.Add(path, data);
                return true;
            }

            return false;
        }

        private bool RemoveSave(string path)
        {
            path.ThrowIfNull(nameof(path));

            if (!saveFiles.ContainsKey(path))
                return false;

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    saveFiles.Remove(path);
                    return true;
                }
                catch (Exception ex) { Log.WriteEntry(ex); }
            }

            return false;
        }

        #endregion

        #endregion
    }
}
