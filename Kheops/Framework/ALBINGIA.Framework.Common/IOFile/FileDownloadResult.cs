using System;
using System.Web.Mvc;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.Framework.Common.IOFile
{
    public class FileDownloadResult : ContentResult
    {
        #region Constantes
        private const string FICHIER_REQUIS = "Le nom du fichier est requis";
        private const string DONNEES_FICHIER_REQUIS = "Données du fichier est requis";
        #endregion
        #region Variables membres
        private readonly string _fileName;
        private readonly byte[] _fileData;
        #endregion
        #region Méthodes publiques
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="fullName"></param>
        public FileDownloadResult(string fileName, string filePath, string fullName)
        {
            _fileName = fileName;
            _fileData = fullName.GetFileData(filePath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (string.IsNullOrEmpty(_fileName))
                throw new AlbTechException(new Exception(FICHIER_REQUIS));
            if (_fileData == null)
                throw new Exception(DONNEES_FICHIER_REQUIS);
            var contentDisposition = string.Format("filename={0}", _fileName);
            context.HttpContext.Response.AddHeader("Content-Disposition", contentDisposition);
            context.HttpContext.Response.ContentType = "application/force-download";
            context.HttpContext.Response.BinaryWrite(_fileData);
        }
        #endregion
    }
}
