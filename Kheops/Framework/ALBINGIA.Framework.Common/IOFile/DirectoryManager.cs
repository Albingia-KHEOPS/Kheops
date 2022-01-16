using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ALBINGIA.Framework.Common.Models.FileModel;

namespace ALBINGIA.Framework.Common.IOFile
{
    public class DirectoryManager
    {
        #region Methodes publiques
        public static string CurrentSaveDocDirectory(string path, string prefixDirectory, int maxFiles, int positionRepertoire)
        {
            var directoryNumber = DirectoryManager.GetNumberDirectory(path, prefixDirectory);
            var oldDirectoryName = prefixDirectory + directoryNumber.ToString(CultureInfo.InvariantCulture).PadLeft(positionRepertoire, '0');
            string directoryName;
            if (directoryNumber == 0)
            {
                directoryName = prefixDirectory + "1".PadLeft(positionRepertoire, '0');
                DirectoryManager.CreateDirectory(path, directoryName);
                return path + directoryName;
            }

            if (DirectoryManager.GetNumberFilesDirectory(path + oldDirectoryName) >= maxFiles)
            {
                directoryNumber++;
                directoryName = prefixDirectory + directoryNumber.ToString(CultureInfo.InvariantCulture).PadLeft(positionRepertoire, '0');
                DirectoryManager.CreateDirectory(path, directoryName);
                return path + directoryName;
            }
            return path + prefixDirectory + directoryNumber.ToString(CultureInfo.InvariantCulture).PadLeft(positionRepertoire, '0');
        }

        public static int GetNumberDirectory(string path, string prefixDirectory)
        {
            var directories = Directory.EnumerateDirectories(path);
            var directoriesList = directories as IList<string> ?? directories.ToList();
            return !directoriesList.Any() ? 0 : directoriesList.Count(elm => elm.Contains(prefixDirectory));
        }
        public static int GetNumberFilesDirectory(string path)
        {
            var files = Directory.EnumerateFiles(path);
            var filesList = files as IList<string> ?? files.ToList();
            return !filesList.Any() ? 0 : filesList.Count();
        }

        public static void CreateDirectory(string path, string directoryName)
        {
            Directory.CreateDirectory(Path.Combine(path, directoryName));
        }

        public static List<FileDescription> GetFileFromFolder(string path, bool subFolders = false)
        {
            var foldersInfo = new DirectoryInfo(path);
            var filesResult = new List<FileDescription>();
            var filesInfo = subFolders
                ? foldersInfo.EnumerateFiles("*", SearchOption.AllDirectories).ToList()
                : foldersInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly).ToList();
            if (!filesInfo.Any())
                return null;

            filesInfo.ForEach(file =>
            {
                if ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    return;
                var fileDesc = new FileDescription {FullName = file.FullName, Name = file.Name};
                filesResult.Add(fileDesc);
            });
            return filesResult;
        }

        #endregion

    }
}
