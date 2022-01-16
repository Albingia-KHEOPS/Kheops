using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Models.FileModel;
using OP.WSAS400.DTO.SuiviDocuments;
using ALBINGIA.Framework.Common.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using OP.WSAS400.DTO.Common;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.Framework.Common.IOFile;
using System.IO;


namespace OP.DataAccess
{
    public class SuiviDocumentsRepository
    {
        #region Méthodes publiques

        public static SuiviDocumentsListeDto GetListSuiviDocuments(SuiviDocFiltreDto filtreDto, ModeConsultation modeNavig, bool readOnly)
        {
            return GetListSuiviDocPlat(filtreDto, modeNavig, readOnly);
        }

        public static bool GenerateDocuments(string numAffaire, string version, string type, string avenant, string lotId)
        {
            bool isContract = false;
            EacParameter[] param = new EacParameter[4];

            param[0] = new EacParameter("numAffaire", DbType.AnsiStringFixedLength);
            param[0].Value = numAffaire.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("avenant", DbType.AnsiStringFixedLength);
            param[3].Value = avenant;
            

            string sql = @"SELECT PBETA STRRETURNCOL FROM YPOBASE 
WHERE PBIPB= :numAffaire 
AND PBALX = :version 
AND PBTYP = :type 
AND PBAVN = :avenant";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
                isContract = result.FirstOrDefault().StrReturnCol == "V";

            //TODO appel de génération par lot
            return true;
        }

        public static bool PrintDocuments(string lotId, string user)
        {
            //TODO Edition du document
            //select table pour savoir si l'impression a été effectuée avec succès

            DateTime dateNow = DateTime.Now;
            EacParameter[] param = new EacParameter[3];

            param[0] = new EacParameter("user", DbType.AnsiStringFixedLength);
            param[0].Value = user;
            param[1] = new EacParameter("date", DbType.AnsiStringFixedLength);
            param[1].Value = AlbConvert.ConvertDateToInt(dateNow);
            param[2] = new EacParameter("time", DbType.AnsiStringFixedLength);
            param[2].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));

            string sql = @"UPDATE KPDOCLD SET KEMSTU = :user, KEMSTD = :date, KEMSTH = :time, KEMSIT = 'E'";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);


            sql = @"UPDATE KPDOCL SET KELMAJU = :user, KELMAJD =  :date, KELMAJH = :time, KELSTU = :user, KELSTD = :date, KELSTH = :time, KELSIT = 'E'";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return true;
        }

        public static bool ReeditDocument(string idDoc, string user)
        {
            //Récupération des paramètres du document
            GetParamDocument(idDoc);

            //TODO Edition du document
            //select table pour savoir si l'impression a été effectuée avec succès

            return true;
        }

        public static List<FileDescription> GetParamDocument(string idDoc)
        {
            //Récupération des paramètres du document
            string nomDoc = string.Empty;
            string pathDoc = string.Empty;
            // string sql = string.Format("SELECT KEQNOM NOMDOC, KEQCHM CHEMINDOC FROM KPDOC WHERE KEQID = {0}", idDoc);
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("P_IDDOC", DbType.AnsiStringFixedLength);
            param[0].Value = idDoc;
            return DbBase.Settings.ExecuteList<FileDescription>(CommandType.StoredProcedure, "SP_GETPARAMDOCUMENTS", param);

        }

        public static List<string> EditerDocuments(string listeDocIdLogo, string listeDocIdNOLogo)
        {
            List<string> toReturn = new List<string>();
            if (!string.IsNullOrEmpty(listeDocIdLogo) && !string.IsNullOrEmpty(listeDocIdLogo.Replace("|", "").Trim()))
            {
                var lstDocIdLogo = GetParamDocument(listeDocIdLogo);
                if (lstDocIdLogo != null && lstDocIdLogo.Any())
                {
                    foreach (FileDescription doc in lstDocIdLogo)
                    {
                        if (!string.IsNullOrEmpty(doc.FullName))
                        {
                            var pathFileFinal = doc.FullName.Replace(".docx", "ALOGO.pdf").Replace(".doc", "ALOGO.pdf");
                            if (File.Exists(pathFileFinal))
                                toReturn.Add(doc.FullName.Replace(".docx", "ALOGO.pdf").Replace(".doc", "ALOGO.pdf"));
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(listeDocIdNOLogo) && !string.IsNullOrEmpty(listeDocIdNOLogo.Replace("|", "").Trim()))
            {
                var lstDocIdNOLogo = GetParamDocument(listeDocIdNOLogo);
                if (lstDocIdNOLogo != null && lstDocIdNOLogo.Any())
                {
                    foreach (FileDescription doc in lstDocIdNOLogo)
                    {
                        if (!string.IsNullOrEmpty(doc.FullName))
                        {
                            var pathFileFinal = doc.FullName.Replace(".docx", "SLOGO.pdf").Replace(".doc", "SLOGO.pdf");
                            if (File.Exists(pathFileFinal))
                                toReturn.Add(doc.FullName.Replace(".docx", "SLOGO.pdf").Replace(".doc", "SLOGO.pdf"));
                        }
                    }
                }
            }

            return toReturn;
        }

        /// <summary>
        /// Récupération du chemin du doc
        /// </summary>
        public static string GetBlocFullPath(string docId)
        {
            var parameters = new EacParameter[] {
                new EacParameter("id", DbType.AnsiStringFixedLength) { Value = docId }
            };
            string sql = @"SELECT TRIM(KEQCHM) CONCAT TRIM(KEQNOM) STRRETURNCOL FROM KPDOC WHERE KEQID = :id"
                ;
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, parameters);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().StrReturnCol.Replace('/', '\\');
            }

            return string.Empty;
        }

        public static FileDescription OpenPJ(string docId)
        {
            FileDescription model = new FileDescription();
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("docId", DbType.AnsiStringFixedLength);
            param[0].Value = docId;

            string sql = @"SELECT KEQLIB LIBDOC, TRIM(KEQCHM) CONCAT TRIM(KEQNOM) CHEMINDOC, TRIM(KEQNOM) NOMDOC FROM KPDOC 
WHERE KEQID = :docId";
            var result = DbBase.Settings.ExecuteList<FileDescription>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                model = result.FirstOrDefault();
                var fileDescr = ALBINGIA.Framework.Common.IOFile.IOFileManager.GetFileDescription(model.FullName, model.Name);
                model.Size = fileDescr.Size;
                model.Extension = fileDescr.Extension;
                model.FilePath = fileDescr.FilePath;
            }

            return model;
        }

        #endregion

        #region Méthodes privées

        private static SuiviDocumentsListeDto GetListSuiviDocPlat(SuiviDocFiltreDto filtreDto, ModeConsultation modeNavig, bool readOnly)
        {
            SuiviDocumentsListeDto listeLot = new SuiviDocumentsListeDto();

            DbParameter[] param = new DbParameter[21];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = !string.IsNullOrEmpty(filtreDto.CodeOffre.ToUpper()) ? filtreDto.CodeOffre.PadLeft(9, ' ') : string.Empty;
            param[1] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[1].Value = !string.IsNullOrEmpty(filtreDto.Version) ? filtreDto.Version : string.Empty;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = !string.IsNullOrEmpty(filtreDto.Type) ? filtreDto.Type : string.Empty;
            param[3] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[3].Value = !string.IsNullOrEmpty(filtreDto.User) ? filtreDto.User : string.Empty;
            param[4] = new EacParameter("P_AVENANT", DbType.Int32);
            param[4].Value = filtreDto.Avenant;
            param[5] = new EacParameter("P_NUMLOT", DbType.Int32);
            param[5].Value = filtreDto.NumLot;
            param[6] = new EacParameter("P_SITUATION", DbType.AnsiStringFixedLength);
            param[6].Value = !string.IsNullOrEmpty(filtreDto.Situation) ? filtreDto.Situation : string.Empty;
            param[7] = new EacParameter("P_DATEDEBSITUATION", DbType.Int32);
            param[7].Value = filtreDto.DateDebSituation;
            param[8] = new EacParameter("P_DATEFINSITUATION", DbType.Int32);
            param[8].Value = filtreDto.DateFinSituation;
            param[9] = new EacParameter("P_UNITESERVICE", DbType.AnsiStringFixedLength);
            param[9].Value = !string.IsNullOrEmpty(filtreDto.UniteService) ? filtreDto.UniteService : string.Empty;
            param[10] = new EacParameter("P_DATEDEBEDITION", DbType.Int32);
            param[10].Value = filtreDto.DateDebEdition;
            param[11] = new EacParameter("P_DATEFINEDITION", DbType.Int32);
            param[11].Value = filtreDto.DateFinEdition;
            param[12] = new EacParameter("P_TYPEDESTINATAIRE", DbType.AnsiStringFixedLength);
            param[12].Value = !string.IsNullOrEmpty(filtreDto.TypeDestinataire) ? filtreDto.TypeDestinataire : string.Empty;
            param[13] = new EacParameter("P_DESTINATAIRE", DbType.Int32);
            param[13].Value = filtreDto.CodeDestinataire;
            param[14] = new EacParameter("P_INTERLOCUTEUR", DbType.Int32);
            param[14].Value = !string.IsNullOrEmpty(filtreDto.CodeInterlocuteur) ? Convert.ToInt32(filtreDto.CodeInterlocuteur) : 0;
            param[15] = new EacParameter("P_TYPEDOC", DbType.AnsiStringFixedLength);
            param[15].Value = !string.IsNullOrEmpty(filtreDto.TypeDoc) ? filtreDto.TypeDoc : string.Empty;
            param[16] = new EacParameter("P_COURRIERTYPE", DbType.AnsiStringFixedLength);
            param[16].Value = !string.IsNullOrEmpty(filtreDto.CourrierType) ? filtreDto.CourrierType : string.Empty;
            param[17] = new EacParameter("P_WARNING", DbType.AnsiStringFixedLength);
            param[17].Value = !string.IsNullOrEmpty(filtreDto.Warning) ? filtreDto.Warning : string.Empty;
            param[18] = new EacParameter("P_STARTLINE", DbType.Int32);
            param[18].Value = filtreDto.StartLine;
            param[19] = new EacParameter("P_ENDLINE", DbType.Int32);
            param[19].Value = filtreDto.EndLine;
            param[20] = new EacParameter("P_COUNT", DbType.Int32);
            param[20].Value = 0;
            param[20].Direction = ParameterDirection.InputOutput;
            param[20].DbType = DbType.Int32;

            var result = DbBase.Settings.ExecuteList<SuiviDocumentsPlatDto>(CommandType.StoredProcedure, "SP_GETLISTSUIVIDOCPLAT", param);

            if (result != null && result.Any())
            {
                listeLot.SuiviDocumentsListeLot = GetListSuiviDoc(result);
                listeLot.SuiviDocumentsPlat = result;
                listeLot.CountLine = Convert.ToInt32(param[20].Value.ToString());
                listeLot.StartLine = filtreDto.StartLine;
                listeLot.EndLine = filtreDto.EndLine;
                listeLot.PageNumber = filtreDto.PageNumber;
            }
            return listeLot;
        }

        private static List<SuiviDocumentsLotDto> GetListSuiviDoc(List<SuiviDocumentsPlatDto> result)
        {
            List<SuiviDocumentsLotDto> toReturn = new List<SuiviDocumentsLotDto>();

            var lstLot = result.GroupBy(el => el.LotId).Select(l => l.First()).ToList();

            lstLot.ForEach(lot =>
            {
                var resLotDetail = result.FindAll(r => r.LotId == lot.LotId).GroupBy(el => el.LotDetailId).Select(ld => ld.First()).ToList();
                var lstLotsDetails = new List<SuiviDocumentsLotDetailsDto>();
                resLotDetail.ForEach(lotDetail =>
                {
                    var resDoc = result.FindAll(r => r.LotId == lotDetail.LotId && r.LotDetailId == lotDetail.LotDetailId).GroupBy(el => el.DocId).Select(d => d.First()).ToList();
                    var lstDoc = new List<SuiviDocumentsDocDto>();
                    resDoc.ForEach(doc =>
                    {
                        lstDoc.Add(new SuiviDocumentsDocDto
                        {
                            DocId = doc.DocId,
                            TypeDoc = doc.TypeDoc,
                            CodeDoc = doc.CodeDoc,
                            TypeDocLib = doc.TypeDocLib,
                            NomDoc = doc.NomDoc,
                            CheminDoc = doc.CheminDoc,
                            EmptyLine = doc.EmptyLine
                        });
                    });

                    lstLotsDetails.Add(new SuiviDocumentsLotDetailsDto
                    {
                        LotDetailId = lotDetail.LotDetailId,
                        CodeSituation = lotDetail.CodeSituation,
                        DateSituation = lotDetail.DateSituation,
                        HeureSituation = lotDetail.HeureSituation,
                        UserSituation = lotDetail.UtilisateurSituation,
                        TypeDestinataire = lotDetail.TypeDestinataire,
                        CodeDestinataire = lotDetail.CodeDestinataire,
                        NomDestinataire = lotDetail.NomDestinataire,
                        CodeInterlocuteur = lotDetail.CodeInterlocuteur,
                        NomInterlocuteur = lotDetail.NomInterlocuteur,
                        CodeDiffusion = lotDetail.CodeDiffusion,
                        LibDiffusion = lotDetail.LibDiffusion,
                        SuiviDocumentsListDoc = lstDoc
                    });
                });

                toReturn.Add(new SuiviDocumentsLotDto
                {
                    LotId = lot.LotId,
                    LotLibelle = lot.LotLibelle,
                    LotUser = lot.CodeUtilisateur,
                    NomUser = lot.NomUtilisateur,
                    PrenomUser = lot.PrenomUtilisateur,
                    UniteService = lot.UniteService,
                    TypeAffaire = lot.TypeAffaire,
                    CodeOffre = lot.CodeOffre,
                    Version = lot.Version,
                    ActeGestion = lot.ActeGestion,
                    ActeGestionLib = lot.ActeGestionLib,
                    NumInterne = lot.NumInterne,
                    NumExterne = lot.NumExterne,
                    SuiviDocumentsListLotDetail = lstLotsDetails
                });
            });

            return toReturn;
        }




        #endregion
    }
}
