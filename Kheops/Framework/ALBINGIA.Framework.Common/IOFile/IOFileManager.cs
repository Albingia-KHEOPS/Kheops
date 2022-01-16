using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using ALBINGIA.Framework.Common.Models.FileModel;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace ALBINGIA.Framework.Common.IOFile
{
//using File = DummyFile;
//using Directory = DummyFile;

//    internal class DummyFile
//    {
//        internal static void Delete(string fichier)
//        {
//        }

//        internal static void Move(string oldName, string newName)
//        {
//        }

//        internal static bool Exists(string fullPathFileName)
//        {
//            return false;
//        }

//        internal static void Copy(string sourceFile, string targetFile)
//        {
//        }

//        internal static string[] GetFiles(string targetPath)
//        {
//            return new string[0];
//        }
//    }

    public class IOFileManager
    {
        #region Variables membres
        private static readonly string _fileExtension = FileContentManager.GetConfigValue("ExtentionFichier");
        private static readonly string _fileLimitSize = FileContentManager.GetConfigValue("FileSize");
        //private static readonly string _filePath = FileContentManager.GetConfigValue("UploadPathInventory");
        #endregion

        /// <summary>
        /// retourne l'extension d'un fichier à partir d'une chaine
        /// </summary>
        /// <param name="fileName">non du fichier (ou chemin complet)</param>
        /// <returns>extension du fichier</returns>
        public static string GetStrExtension(string fileName)
        {
            var fileParts = fileName.Split(new[] { "." }, StringSplitOptions.None);
            return fileParts.Any() ? string.Concat(".", fileParts.Last()).Trim() : string.Empty;
        }

        /// <summary>
        ///  Description d'un fichier.sans contrôle de taille
        /// </summary>
        /// <param name="targetPath">chemin du fichier cible</param>
        /// <returns></returns>
        public static FileDescription GetFileDescription(string filePath, string fileName)
        {
            FileDescription fileDescr = new FileDescription();
            var fileInfo = new FileInfo(filePath);

            fileDescr = new FileDescription
            {
                Name = fileName,
                FullName = fileInfo.Name,
                Size = fileInfo.Length / 1024,
                WebPath = fileInfo.Name,
                DateCreated = fileInfo.CreationTime,
                FilePath = filePath,
                Extension = fileInfo.Extension
            };

            return fileDescr;
        }

        /// <summary>
        ///  Description des fichiers
        /// </summary>
        /// <param name="targetPath">chemin du fichier cible</param>
        /// <param name="codeFile">code fichier</param>
        /// <param name="strSplit">caractère de split </param>
        /// <returns></returns>
        public static List<FileDescription> GetAllFileDescription(string targetPath, string codeFile, string strSplit)
        {
            var files = Directory.GetFiles(targetPath);
            var filesDirectory = new List<FileDescription>();
            foreach (var item in files)
            {
                var fileInfo = new FileInfo(item);
                if (IsInExtensionFiles(_fileExtension, item) && IsInLimitSize(_fileLimitSize, fileInfo.Length) && fileInfo.Name.Contains(codeFile))
                {
                    var split = new[] { strSplit };
                    var nameFileParts = fileInfo.Name.Split(split, StringSplitOptions.None);

                    filesDirectory.Add(new FileDescription
                    {
                        Name = nameFileParts[nameFileParts.Count() - 1],
                        FullName = fileInfo.Name,
                        Size = fileInfo.Length / 1024,
                        WebPath = fileInfo.Name,
                        DateCreated = fileInfo.CreationTime,
                        FilePath = targetPath
                    });
                }
            }
            return filesDirectory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpFiles"></param>
        /// <param name="targetDirectory"></param>
        /// <param name="prefixFichier"> </param>
        /// <param name="splitChar"> </param>
        /// <returns></returns>
        public static FileDescription UploadFiles(HttpPostedFileBase httpFiles, string targetDirectory, string prefixFichier, string splitChar, string fileName = "")
        {
            FileDescription fileDescription = null;
            var fileParts = httpFiles.FileName.Split('\\');
            string filePath = string.Empty;
            if (string.IsNullOrEmpty(fileName))
                filePath = Path.Combine(targetDirectory
                                              , Path.GetFileName(string.Format("{0}{1}{2}", prefixFichier, splitChar, httpFiles.FileName.Contains("\\") && fileParts.Count() > 0 ? fileParts[fileParts.Count() - 1] : httpFiles.FileName)));
            else
                filePath = Path.Combine(targetDirectory
                                             , fileName);
            //if (httpFiles.ContentLength > 0 && !IsInLimitSize(FileContentManager.GetConfigValue("FileSize"), httpFiles.ContentLength))
            //    throw new AlbFoncException("Le fichier sélectionné dépasse la limite de taille autorisée (500ko)", trace: false, sendMail: false, onlyMessage: true);
            //if (!IsInExtensionFiles(FileContentManager.GetConfigValue("ExtentionFichier"), Path.GetExtension(httpFiles.FileName)))
            //    throw new AlbFoncException("Le format du fichier sélectionné n'est pas autorisé", trace: false, sendMail: false, onlyMessage: true);
            if (httpFiles.ContentLength > 0 && IsInLimitSize(FileContentManager.GetConfigValue("FileSize"), httpFiles.ContentLength)
             && IsInExtensionFiles(FileContentManager.GetConfigValue("ExtentionFichier"), Path.GetExtension(httpFiles.FileName).ToLower()))
            {
                httpFiles.SaveAs(filePath);
                fileDescription = new FileDescription()
                                      {
                                          Name = httpFiles.FileName,
                                          FullName = prefixFichier + splitChar + httpFiles.FileName,
                                          DateCreated = DateTime.Now,
                                          Size = httpFiles.ContentLength / 1024,
                                          WebPath = httpFiles.FileName
                                      };
            }
            return fileDescription;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="file"></param>
        //[Obsolete]
        //public static void DownLoadFile(string file)
        //{
        //    var response = HttpContext.Current.Response;
        //    // Identifier le fichier.
        //    var filename = Path.GetFileName(file);
        //    var physicalPath = AlbOpConstants.UploadPathInventory + "\\" + filename;
        //    response.Clear();
        //    // Spécifier le mime du téléchargement.
        //    response.ContentType = "application/octet-stream";
        //    response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
        //    // Télécharger le fichier.
        //    response.WriteFile(physicalPath);
        //    response.Flush();
        //}

        /// <summary>
        /// Supprime le fichier (file) du repertoire "Inventaire"
        /// </summary>
        /// <param name="file">nom du fichier</param>
        /// <param name="filePath"></param>
        public static void DeleteFile(string file, string filePath)
        {
            var fichier = filePath + "\\" + file;
            File.Delete(fichier);
        }

        /// <summary>
        /// Indique si l'extension du fichier (file) est presente
        /// dans la liste des extensions (filesExtensions).
        /// </summary>
        /// <param name="filesExtensions"></param>
        /// <param name="file"> </param>
        /// <param name="split">Caractère ';' par default</param>
        /// <returns></returns>
        public static bool IsInExtensionFiles(string filesExtensions, String file, Char split = ';')
        {
            return filesExtensions.Replace("*", string.Empty).Split(split).Contains(new FileInfo(file).Extension.ToLower());
        }

        /// <summary>
        /// Indique si le fichier (file) ne dépasse pas la limite (size).
        /// </summary>
        /// <param name="paramSize"> </param>
        /// <param name="fileSize"> </param>
        /// <returns></returns>
        private static bool IsInLimitSize(string paramSize, long fileSize)
        {
            return fileSize <= Convert.ToInt64(paramSize);

        }

        public static void RenameFile(string oldName, string newName)
        {
            File.Move(oldName,newName);
        }

        /// <summary>
        /// Indique si le fichier existe ou pas
        /// </summary>
        /// <param name="fullPathFileName">Nom du fichier avec le chemin</param>
        /// <returns></returns>
        public static bool IsExistFileFullPath(string fullPathFileName)
        {
            return File.Exists(fullPathFileName);
        }
        /// <summary>
        /// Indique la presence du fichier (httpFiles) dans le repertoire (targetDirectory).
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        public static bool IsExistFile(string targetDirectory, string targetFile)
        {
            string completeFilePath = Path.Combine(targetDirectory
                                                  , targetFile);
            return File.Exists(completeFilePath);
        }
        public static bool IsExistDirectory(string targetDirectory)
        {
            return Directory.Exists(targetDirectory);
        }
        public static void CopyFile(string sourceFile, string targetFile)
        {
            File.Copy(sourceFile, targetFile);
        }
    }
}
