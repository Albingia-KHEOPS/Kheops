using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.IOFile;
using OP.WSAS400.DTO.DocumentGestion;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace OP.DataAccess
{
    public class CopieDocRepository : RepositoryBase
    {
        internal static readonly string SelectDocsToCopy = @"
SELECT KHQOLDC , KHQCODE , KHQNOMD , KHQTABLE , KHQIPB , KHQTYP , KHQALX 
FROM KPCOPDC
WHERE ( KHQIPB , KHQALX , KHQTYP , KHQAVN ) = ( :IPB , :ALX , :TYP , :AVN )";
        internal static readonly string DeleteDocsToCopy = @"DELETE FROM KPCOPDC WHERE ( KHQIPB , KHQALX , KHQTYP , KHQAVN ) = ( :IPB , :ALX , :TYP , :AVN )";

        public CopieDocRepository(IDbConnection connection) : base(connection) { }

        #region Méthodes publiques

        public static void CopierDocuments(string code, string version, string type, string avenant)
        {
            //Récupération des références des fichiers à copier
            var documentsToCopy = GetListeDocumentsACopier(code, version, type, avenant);

            //copie physique des documents
            foreach (CopieDocDto doc in documentsToCopy)
            {
                if (string.IsNullOrEmpty(doc.OldCheminComplet) || string.IsNullOrEmpty(doc.NewCheminComplet))
                {
                    continue;
                }

                var oldPathFull = Path.GetFullPath(doc.OldCheminComplet);
                var newPathFull = Path.GetFullPath(doc.NewCheminComplet);

                if (string.Compare(oldPathFull, newPathFull, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    //Don't copy if source and destination are the same
                    continue;
                }

                var pathNewFile = Path.GetDirectoryName(newPathFull);
                if (!IOFileManager.IsExistDirectory(pathNewFile))
                {
                    // Ensure Directory
                    Directory.CreateDirectory(pathNewFile);
                }

                if (IOFileManager.IsExistFileFullPath(newPathFull))
                {
                    // remove before overwritting
                    IOFileManager.DeleteFile(Path.GetFileName(newPathFull), pathNewFile);
                }

                try
                {
                    // try to copy
                    IOFileManager.CopyFile(oldPathFull, newPathFull);
                }
                catch (Exception)
                {
                    // Suppression du ficher dans la table KPDOCEXT
                    if (!string.IsNullOrEmpty(doc.Code) && doc.Code.Trim().StartsWith("CV"))
                    {
                        string delFile = string.Format("DELETE FROM {0} WHERE {1} = {2}", doc.TableCible, doc.TableCible.Trim() == "KPDOCEXT" ? "KERID" : "KEQID", doc.NewGuid);
                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, delFile);
                    }
                    //throw;
                }

            }

            //Effacer les références des fichiers à copier
            EffacerListeDocumentsACopier(code, version, type, avenant);
        }
        #endregion

        #region Méthodes privées
        private static void EffacerListeDocumentsACopier(string code, string version, string type, string avenant)
        {
            string sql = string.Format(@"DELETE 
                                         FROM KPCOPDC
                                         WHERE KHQIPB='{0}' AND KHQALX = {1} AND KHQTYP = '{2}' AND KHQAVN = {3} ", code.PadLeft(9, ' '), version, type, avenant);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        private static List<CopieDocDto> GetListeDocumentsACopier(string code, string version, string type, string avenant)
        {
            string sql = string.Format(@"SELECT KHQOLDC, KHQCODE, KHQNOMD, KHQTABLE,KHQIPB,KHQTYP,KHQALX 
                                         FROM KPCOPDC
                                         WHERE KHQIPB='{0}' AND KHQALX = {1} AND KHQTYP = '{2}' AND KHQAVN = {3} ", code.PadLeft(9, ' '), version, type, avenant);
            return DbBase.Settings.ExecuteList<CopieDocDto>(CommandType.Text, sql);
        }



        #endregion

        public IEnumerable<CopieDocDto> GetDocuments(Folder folder) {
            return Fetch<CopieDocDto>(SelectDocsToCopy, folder.CodeOffre.ToIPB(), folder.Version, folder.Type, folder.NumeroAvenant);
        }

        public void CopyDocList(Folder folder, IEnumerable<CopieDocDto> documentsToCopy) {
            //copie physique des documents
            foreach (CopieDocDto doc in documentsToCopy) {
                if (string.IsNullOrEmpty(doc.OldCheminComplet) || string.IsNullOrEmpty(doc.NewCheminComplet)) {
                    continue;
                }

                var oldPathFull = Path.GetFullPath(doc.OldCheminComplet);
                var newPathFull = Path.GetFullPath(doc.NewCheminComplet);

                if (string.Compare(oldPathFull, newPathFull, StringComparison.InvariantCultureIgnoreCase) == 0) {
                    //Don't copy if source and destination are the same
                    continue;
                }

                var pathNewFile = Path.GetDirectoryName(newPathFull);
                if (!IOFileManager.IsExistDirectory(pathNewFile)) {
                    // Ensure Directory
                    Directory.CreateDirectory(pathNewFile);
                }

                if (IOFileManager.IsExistFileFullPath(newPathFull)) {
                    // remove before overwritting
                    IOFileManager.DeleteFile(Path.GetFileName(newPathFull), pathNewFile);
                }

                try {
                    // try to copy
                    IOFileManager.CopyFile(oldPathFull, newPathFull);
                }
                catch (Exception) {
                    if (!string.IsNullOrEmpty(doc.Code) && doc.Code.Trim().StartsWith("CV")) {
                        // Suppression du ficher dans la table KPDOCEXT
                        string idColumn = doc.TableCible.Trim() == "KPDOCEXT" ? "KERID" : "KEQID";
                        using (var options = new DbExecOptions(this.connection == null) {
                            DbConnection = this.connection,
                            SqlText = $"DELETE FROM {doc.TableCible} WHERE {idColumn} = :ID ;"
                        }) {
                            options.BuildParameters(doc.NewGuid);
                            options.Exec();
                        }
                    }
                }
            }
        }

        public void DeleteDocuments(Folder folder) {
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = DeleteDocsToCopy
            }) {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type, folder.NumeroAvenant);
                options.Exec();
            }
        }
    }
}
