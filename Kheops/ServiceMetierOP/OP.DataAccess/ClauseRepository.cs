using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.ChoixClauses;
using OP.WSAS400.DTO.Clause;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Concerts.Clause;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.LibelleClauses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Globalization;
using System.Linq;

namespace OP.DataAccess
{
    public class ClauseRepository
    {
        #region Méthodes Publiques

        public static List<ClausierDto> SearchClause(string libelle, string motcle1, string motcle2, string motcle3, string sequence, string rubrique, string sousrubrique, string modaliteAffichage, int date)
        {
            DbParameter[] param = new DbParameter[9];
            param[0] = new EacParameter("P_LIBELLE", libelle);
            param[1] = new EacParameter("P_MOTCLE1", 0);
            param[1].Value = !string.IsNullOrEmpty(motcle1) ? Convert.ToInt32(motcle1) : 0;
            param[2] = new EacParameter("P_MOTCLE2", 0);
            param[2].Value = !string.IsNullOrEmpty(motcle2) ? Convert.ToInt32(motcle2) : 0;
            param[3] = new EacParameter("P_MOTCLE3", 0);
            param[3].Value = !string.IsNullOrEmpty(motcle3) ? Convert.ToInt32(motcle3) : 0;
            param[4] = new EacParameter("P_RUBRIQUE", rubrique);
            param[5] = new EacParameter("P_SOUSRUBRIQUE", sousrubrique);
            param[6] = new EacParameter("P_SEQUENCE", 0);
            param[6].Value = !string.IsNullOrEmpty(sequence) ? Convert.ToInt32(sequence) : 0;
            param[7] = new EacParameter("P_MODE", modaliteAffichage);
            param[8] = new EacParameter("P_DATEOFFRE", 0);
            param[8].Value = date;

            return DbBase.Settings.ExecuteList<ClausierDto>(CommandType.StoredProcedure, "SP_SCLAUSE", param);
        }

        public static ClausierPageDto InitClausier()
        {
            ClausierPageDto model = new ClausierPageDto();
            model.Rubriques = GetListRubrique();
            model.SousRubriques = new List<ParametreDto>();
            model.Sequences = new List<ParametreDto>();
            model.MotsCles1 = GetListMotsCles();
            model.MotsCles2 = GetListMotsCles();
            model.MotsCles3 = GetListMotsCles();
            //model.Contextes = CommonRepository.GetParametres(branche, cible, "PRODU", "QECTX");
            return model;
        }

        public static List<ClausierDto> GetHistoClause(string rubrique, string sousrubrique, string sequence)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("rubrique", DbType.AnsiStringFixedLength);
            param[0].Value = rubrique;
            param[1] = new EacParameter("sousrubrique", DbType.AnsiStringFixedLength);
            param[1].Value = sousrubrique;
            param[2] = new EacParameter("sequence", DbType.Int32);
            param[2].Value = !string.IsNullOrEmpty(sequence) ? Convert.ToInt32(sequence) : 0;

            string sql = @"SELECT KDUID CODE, KDULIB LIBELLE, KDUVER VERSION, KDUDATD DATEDEB, KDUDATF DATEFIN
                            FROM KCLAUSE
                            WHERE KDUNM1 = :rubrique AND KDUNM2 = :sousrubrique AND KDUNM3 = :sequence
                            ORDER BY KDUVER ASC";

            return DbBase.Settings.ExecuteList<ClausierDto>(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Récupère les Clauses
        /// </summary>
        public static List<LibelleClauseDto> GetClausesLibelle(string branche, string cible, string nm1, string nm2, string nm3)
        {
            Boolean first = true;
            string sql = @"SELECT * FROM KCLALIBSPE";
            if (branche != "") {
                if (first)
                {
                 sql += " WHERE  KCLSBRA='"+branche+"'";
                 first = false;
                }
                 else { sql += " AND KCLSBRA='"+branche+"'";
                }
            }

            if (cible != "")
            {
                if (first)
                {
                    sql += " WHERE  KCLSCIB='" + cible + "'";
                    first = false;
                }
                else
                {
                    sql += " AND KCLSCIB='" + cible + "'";
                }
            }


            if (nm1 != "")
            {
                if (first)
                {
                    sql += " WHERE  KCLSNM1='" + nm1+"'";
                    first = false;
                }
                else
                {
                    sql += " AND KCLSNM1='" + nm1 + "'";
                }
            }

            if (nm2 != "")
            {
                if (first)
                {
                    sql += " WHERE  KCLSNM2='" + nm2 + "'";
                    first = false;
                }
                else
                {
                    sql += " AND KCLSNM2='" + nm2 + "'";
                }
            }
            if (nm3 != "")
            {
                if (first)
                {
                    sql += " WHERE  KCLSNM3=" + Int32.Parse(nm3);
                    first = false;
                }
                else
                {
                    sql += " AND KCLSNM3=" + Int32.Parse(nm3);
                }
            }

            return DbBase.Settings.ExecuteList<LibelleClauseDto>(CommandType.Text, sql);
        }
        /// <summary>
        /// Récupère les Branche
        /// </summary>
        public static List<ClauseBrancheDto> GetClausesBranches()
        {

            string sql = @"SELECT TCOD,TPLIB FROM YYYYPAR WHERE TCON ='GENER' AND TFAM = 'BRCHE' AND TCOD != 'PP' AND TCOD != 'ZZ'";
            return DbBase.Settings.ExecuteList<ClauseBrancheDto>(CommandType.Text, sql);
        }

            /// <summary>
            /// Ajout Libelle
            /// </summary>
        public static void AddClauseLibelle(string branche, string cible, string nm1, string nm2, string nm3, string libelle)
        {
            int Nm3 = Int32.Parse(nm3);
            string sql = $@"INSERT INTO  KCLALIBSPE(KCLSBRA,KCLSCIB ,KCLSNM1 , KCLSNM2, KCLSNM3,KCLSLIB) VALUES ('{branche.ToUpper()}','{cible.ToUpper()}','{nm1.ToUpper()}','{nm2.ToUpper()}',{Nm3},'{libelle}')";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
        }
        /// <summary>
        /// Modifier le Libelle
        /// </summary>
        public static void UpdateClauseLibelle(string branche, string cible, string nm1, string nm2, string nm3, string libelle)
        {
            int Nm3 = Int32.Parse(nm3);
            string sql =$@"UPDATE KCLALIBSPE SET KCLSLIB ='{libelle}'  WHERE KCLSBRA='{branche}' AND KCLSCIB='{cible}' AND KCLSNM1='{nm1}' AND KCLSNM2='{nm2}' AND KCLSNM3={Nm3} " ;
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
        }

        /// <summary>
        /// Supprimer le Libelle
        /// </summary>
        public static void DeleteClausesLibelle(string branche, string cible, string nm1, string nm2, string nm3, string libelle)
        {
            int Nm3 = Int32.Parse(nm3);
            string sql = $@"DELETE FROM KCLALIBSPE WHERE KCLSBRA='{branche}' AND KCLSCIB='{cible}' AND KCLSNM1='{nm1}' AND KCLSNM2='{nm2}' AND  KCLSNM3={Nm3} AND KCLSLIB='{libelle}' ";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
        }
        public static string SaveClause(string codeOffre, string version, string type, string etape, string perimetre, string codeRsq, string codeObj, string codeFor, string codeOpt,
                        string contexte, string codeClause, string rubrique, string sousRubrique, string sequence, string versionClause, string numAvenant)
        {
            string retourMsg = string.Empty;
            if (VerifClause(codeOffre, version, type, rubrique, sousRubrique, sequence, contexte, codeRsq, codeObj, codeFor, codeOpt))
                retourMsg = "Clause déjà sélectionnée pour cette étape et ce contexte";
            //UpdateClause(codeOffre, version, type, etape, perimetre, codeRsq, codeObj, codeFor, codeOpt, contexte, rubrique, sousRubrique, sequence, versionClause);
            else InsertClause(codeOffre, version, type, etape, perimetre, codeRsq, codeObj, codeFor, codeOpt, contexte, codeClause, rubrique, sousRubrique, sequence, versionClause, numAvenant);
            return retourMsg;
        }

        public static List<ParametreDto> GetListSousRubriques(string codeRubrique)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("rubrique", DbType.AnsiStringFixedLength);
            param[0].Value = codeRubrique;

            string sql = @"SELECT DISTINCT KDUNM2 CODE, KDUNM2 LIBELLE FROM KCLAUSE WHERE KDUNM1 = :rubrique ORDER BY KDUNM2";
            //codeRubrique);
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);
        }

        public static List<ParametreDto> GetListSequences(string codeRubrique, string codeSousRubrique)
        {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("rubrique", DbType.AnsiStringFixedLength);
            param[0].Value = codeRubrique;
            param[1] = new EacParameter("sousrubrique", DbType.AnsiStringFixedLength);
            param[1].Value = codeSousRubrique;

            string sql = @"SELECT DISTINCT CAST(KDUNM3 AS CHAR(15)) CODE, KDULIR LIBELLE FROM KCLAUSE WHERE KDUNM1 = :rubrique AND KDUNM2 = :sousrubrique ORDER BY CODE";
            //codeRubrique, codeSousRubrique);
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Met à jour l'état titre de la clause en fonction de si elle a été cochée ou non dans choix clause
        /// </summary>
        /// <param name="clauseId"></param>
        /// <param name="etatTitre">"P" si la ligne est cochée, "S" si la ligne n'est pas cochée</param>
        public static string UpdateEtatTitre(string clauseId, string etatTitre)
        {
            //            string sql = string.Format(@"UPDATE KPCLAUSE
            //                                          SET KCASIT = '{0}'
            //                                         WHERE KCAID='{1}'", etatTitre, clauseId);
            //            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, null);

            Int64 idClause = 0;
            Int64.TryParse(clauseId, out idClause);

            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P_IDCLAUSE", 0);
            param[0].Value = idClause;
            param[1] = new EacParameter("P_ETATSITUATION", !string.IsNullOrEmpty(etatTitre) ? etatTitre : string.Empty);
            param[2] = new EacParameter("P_DATENOW", DateTime.Now.ToString("yyyyMMdd"));
            param[3] = new EacParameter("P_ISMODIFAVN", string.Empty);
            param[3].Direction = ParameterDirection.Output;
            param[3].Size = 1;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDATEETATCLAUSE", param);

            return param[3].Value.ToString();
        }

        /// <summary>
        /// Enregistre la clause libre et son texte dans la base de donnée
        /// </summary>
        /// <param name="codeOffreContrat"></param>
        /// <param name="versionOffreContrat"></param>
        /// <param name="typeOffreContrat"></param>
        /// <param name="contexte"></param>
        /// <param name="etape"></param>
        /// <param name="libelleClauseLibre"></param>
        /// <param name="texteClauseLibre"></param>
        public static string EnregistrerClauseLibre(string codeOffreContrat, string versionOffreContrat, string typeOffreContrat, string contexte, string etape, string codeRisque, string codeFormule, string codeOption, string codeObj, string libelleClauseLibre, string texteClauseLibre)
        {
            string retourMsg = string.Empty;
            if (VerifClause(codeOffreContrat, versionOffreContrat, typeOffreContrat, "", "", "0", contexte, codeRisque, codeObj, codeFormule, codeOption))
                return "Clause déjà sélectionnée pour cette étape et ce contexte";
            int pcodeObjet = 0;
            if (!string.IsNullOrEmpty(codeObj))
            {
                Int32.TryParse(codeObj, out pcodeObjet);
                if (pcodeObjet > 0) etape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet);
            }
            DbParameter[] param = new DbParameter[12];
            param[0] = new EacParameter("P_TYPE_OFFRE", typeOffreContrat);
            param[1] = new EacParameter("P_ID_OFFRE", codeOffreContrat.PadLeft(9, ' '));
            param[2] = new EacParameter("P_VERSION_OFFRE", versionOffreContrat);
            param[3] = new EacParameter("P_CONTEXTE", contexte);
            param[4] = new EacParameter("P_ETAPE", etape);
            param[5] = new EacParameter("P_DATE", AlbConvert.ConvertDateToInt(DateTime.Now));
            param[6] = new EacParameter("P_LIBELLE_CLAUSE_LIBRE", libelleClauseLibre);
            param[7] = new EacParameter("P_TEXTE_CLAUSE_LIBRE", texteClauseLibre);
            param[8] = new EacParameter("P_CODERISQUE", !string.IsNullOrEmpty(codeRisque) ? codeRisque : "0");
            param[9] = new EacParameter("P_CODEFORMULE", !string.IsNullOrEmpty(codeFormule) ? codeFormule : "0");
            param[10] = new EacParameter("P_CODEOPTION", !string.IsNullOrEmpty(codeOption) ? codeOption : "0");
            param[11] = new EacParameter("P_CODEOBJ", 0);
            param[11].Value = pcodeObjet;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDCLLI", param);
            return string.Empty;
        }


        public static string VerifAjout(string etape, string contexte, string typeAjt)
        {
            string sql = string.Format("SELECT COUNT(*) NBLIGN FROM KALCONT WHERE KEISERV = 'PRODU' AND KEIACTG = '**' AND KEIETAPE = '{0}' AND KEICTX = '{1}'", etape, contexte);

            switch (typeAjt)
            {
                case "clausier":
                    sql += " AND KEIAJC = 'O'";
                    break;
                case "libre":
                    sql += " AND KEIAJT = 'O'";
                    break;
                case "piecesjointes":
                    sql += " AND KEIEXT = 'J'";
                    break;
            }

            if (CommonRepository.ExistRow(sql))
            {
                EacParameter[] param = new EacParameter[2];
                param[0] = new EacParameter("step", DbType.AnsiStringFixedLength);
                param[0].Value = etape;
                param[1] = new EacParameter("context", DbType.AnsiStringFixedLength);
                param[1].Value = contexte;

                //sql = string.Format(@"SELECT KEIMDT1 RUBRIQUE, KEIMDT2 SOUSRUBRIQUE, KEIMDT3 SEQUENCE FROM KALCONT WHERE KEISERV = 'PRODU' AND KEIACTG='**' AND KEIETAPE = '{0}' AND KEICTX = '{1}'", etape, contexte);
                sql = @"SELECT KEICHI RUBRIQUE, KEICHIS SOUSRUBRIQUE, KEICXI SEQUENCE FROM KALCONT WHERE KEISERV = 'PRODU' AND KEIACTG='**' AND KEIETAPE = :step AND KEICTX = :context";
                //etape, contexte);
                var result = DbBase.Settings.ExecuteList<ClausierDto>(CommandType.Text, sql, param);
                if (result != null && result.Any())
                {
                    var firstRes = result.FirstOrDefault();
                    if (!string.IsNullOrEmpty(firstRes.Rubrique) || !string.IsNullOrEmpty(firstRes.SousRubrique) || firstRes.Sequence != 0)
                        return string.Format("{0}_{1}_{2}", firstRes.Rubrique, firstRes.SousRubrique, firstRes.Sequence);
                }
                return string.Empty;
            }

            return "ERROR";
        }

        public static string CheckClauseLibre(string codeOffre, string version, string type, string etape, string perimetre, string codeRsq, string codeObj, string codeFor, string codeOpt)
        {
            EacParameter[] param = new EacParameter[9];
            param[0] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("etape", DbType.AnsiStringFixedLength);
            param[3].Value = etape;
            param[4] = new EacParameter("peri", DbType.AnsiStringFixedLength);
            param[4].Value = perimetre;
            param[5] = new EacParameter("rsq", DbType.AnsiStringFixedLength);
            param[5].Value = string.IsNullOrEmpty(codeRsq) ? "0" : codeRsq;
            param[6] = new EacParameter("obj", DbType.AnsiStringFixedLength);
            param[6].Value = string.IsNullOrEmpty(codeObj) ? "0" : codeObj;
            param[7] = new EacParameter("for", DbType.AnsiStringFixedLength);
            param[7].Value = string.IsNullOrEmpty(codeFor) ? "0" : codeFor;
            param[8] = new EacParameter("opt", DbType.AnsiStringFixedLength);
            param[8].Value = string.IsNullOrEmpty(codeOpt) ? "0" : codeOpt;

            string toReturn = string.Empty;
            string sql = @"SELECT KCAID INT64RETURNCOL, KCACTX STRRETURNCOL FROM KPCLAUSE WHERE KCAIPB = :codeoffre AND KCAALX = :version AND KCATYP = :type
                                                AND KCAETAFF = :etape AND KCAPERI = :peri AND KCARSQ = :rsq AND KCAOBJ = :obj AND KCAFOR = :for AND KCAOPT = :opt
                                                AND KCAXTL = 'O' AND KCATYPD = 'G' AND KCATXL = 0";
            //codeOffre.PadLeft(9, ' '),
            //version,
            //type,
            //etape,
            //perimetre,
            //string.IsNullOrEmpty(codeRsq) ? "0" : codeRsq,
            //string.IsNullOrEmpty(codeObj) ? "0" : codeObj,
            //string.IsNullOrEmpty(codeFor) ? "0" : codeFor,
            //string.IsNullOrEmpty(codeOpt) ? "0" : codeOpt);

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                foreach (var clause in result)
                {
                    toReturn += "\n" + result.FirstOrDefault().Int64ReturnCol.ToString() + " - " + result.FirstOrDefault().StrReturnCol;
                }
            }

            return toReturn;
        }


        public static string SaveClauseLibre(string codeOffre, string version, string type, string codeAvt, string contexte, string etape, string codeRsq, string codeObj, string codeFor, string codeOpt, string emplacement, string sousemplacement, string ordre)
        {
            //SP_SAVECLAUSELIBRE
            DbParameter[] param = new DbParameter[15];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CODEAVT", 0);
            param[3].Value = !string.IsNullOrEmpty(codeAvt) ? Convert.ToInt32(codeAvt) : 0;
            param[4] = new EacParameter("P_CONTEXTE", contexte);
            param[5] = new EacParameter("P_ETAPE", etape);
            param[6] = new EacParameter("P_DATE", 0);
            param[6].Value = AlbConvert.ConvertDateToInt(DateTime.Now);
            param[7] = new EacParameter("P_CODERISQUE", 0);
            param[7].Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0;
            param[8] = new EacParameter("P_CODEFORMULE", 0);
            param[8].Value = !string.IsNullOrEmpty(codeFor) ? Convert.ToInt32(codeFor) : 0;
            param[9] = new EacParameter("P_CODEOPTION", 0);
            param[9].Value = !string.IsNullOrEmpty(codeOpt) ? Convert.ToInt32(codeOpt) : 0;
            param[10] = new EacParameter("P_CODEOBJ", 0);
            param[10].Value = !string.IsNullOrEmpty(codeObj) ? Convert.ToInt32(codeObj) : 0;
            param[11] = new EacParameter("P_EMPLACEMENT", emplacement);
            param[12] = new EacParameter("P_SOUSEMPLACEMENT", sousemplacement);
            param[13] = new EacParameter("P_ORDRE", 0);
            param[13].Value = !string.IsNullOrEmpty(ordre) ? Convert.ToDecimal(ordre) : 0;
            param[14] = new EacParameter("P_IDCLAUSE", 0);
            param[14].Value = 0;
            param[14].Direction = ParameterDirection.Output;
            param[14].DbType = DbType.Int64;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVECLAUSELIBRE", param);

            return param[14].Value.ToString();
        }

        public static void UpdateDocumentLibre(string idClause, string idDoc, string codeObj, string codeAvn)
        {
            EacParameter[] param = new EacParameter[5];
            param[0] = new EacParameter("iddoc", DbType.AnsiStringFixedLength);
            param[0].Value = idDoc;
            param[1] = new EacParameter("codeobj", DbType.AnsiStringFixedLength);
            param[1].Value = !string.IsNullOrEmpty(codeObj) ? codeObj : "0";
            param[2] = new EacParameter("codeavn", DbType.AnsiStringFixedLength);
            param[2].Value = !string.IsNullOrEmpty(codeAvn) ? codeAvn : "0";
            param[3] = new EacParameter("date", DbType.AnsiStringFixedLength);
            param[3].Value = AlbConvert.ConvertDateToInt(DateTime.Now);
            param[4] = new EacParameter("idclause", DbType.AnsiStringFixedLength);
            param[4].Value = idClause;

            string sql = @"UPDATE KPCLAUSE SET KCATXL = :iddoc, KCAXTLM = 'O', KCAOBJ = :codeobj, KCAAVNM = :codeavn, KCAMAJD = :date                                        
                                         WHERE KCAID = :idclause";
            //idClause,
            //idDoc,
            //!string.IsNullOrEmpty(codeObj) ? codeObj : "0",
            //!string.IsNullOrEmpty(codeAvn) ? codeAvn : "0",
            //AlbConvert.ConvertDateToInt(DateTime.Now));

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            param = new EacParameter[1];
            param[0] = new EacParameter("idclause", DbType.AnsiStringFixedLength);
            param[0].Value = idClause;

            //SLA (06.07.2015) : suite retours ACV et vu avec ZBO, cochage d'une clause libre modifiée si elle n'était pas déjà cochée
            string sqlSelect = @"SELECT KCASIT STRRETURNCOL FROM KPCLAUSE WHERE KCAID = :idclause";
            //idClause);
            var resEtat = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlSelect, param);
            if (resEtat != null && resEtat.Any())
            {
                if (!string.IsNullOrEmpty(resEtat.FirstOrDefault().StrReturnCol) && resEtat.FirstOrDefault().StrReturnCol != "V")
                {
                    param = new EacParameter[2];
                    param[0] = new EacParameter("date", DbType.Int32);
                    param[0].Value = AlbConvert.ConvertDateToInt(DateTime.Now);
                    param[1] = new EacParameter("idclause", DbType.AnsiStringFixedLength);
                    param[1].Value = idClause;

                    string sqlUpdateEtat = @"UPDATE KPCLAUSE 
                                                           SET KCASIT = 'V', KCASITD = :date 
                                                           WHERE KCAID = :idclause";
                    //idClause, AlbConvert.ConvertDateToInt(DateTime.Now));
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdateEtat, param);
                }
            }

        }

        public static string CreateDocumentLibre(string codeOffre, string version, string type, string etape, string idClause, string pathDoc, string nameDoc, string createDoc)
        {
            string docName = nameDoc;

            if (string.IsNullOrEmpty(nameDoc))
            {
                var tNameDoc = pathDoc.Split('\\');
                docName = tNameDoc[tNameDoc.Length - 1];
            }

            DbParameter[] param = new DbParameter[9];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_ETAPE", etape);
            param[4] = new EacParameter("P_IDCLAUSE", 0);
            param[4].Value = !string.IsNullOrEmpty(idClause) ? Convert.ToInt32(idClause) : 0;
            param[5] = new EacParameter("P_NAMEDOC", docName);
            param[6] = new EacParameter("P_PATHDOC", pathDoc);
            param[7] = new EacParameter("P_CREATEDOC", createDoc);
            param[8] = new EacParameter("P_IDDOC", 0);
            param[8].Value = 0;
            param[8].Direction = ParameterDirection.Output;
            param[8].DbType = DbType.Int64;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CREATEDOCUMENTCLAUSE", param);

            return param[8].Value.ToString() != "0" ? param[8].Value.ToString() : string.Empty;
        }

        public static string GetClauseFilePath(string clauseId, ModeConsultation modeNavig)
        {
            var sql = string.Format(@"SELECT CASE WHEN KEQCHM IS NOT NULL THEN KEQCHM
                                       	     	  WHEN KERCHM IS NOT NULL THEN KERCHM
                                       	     	  ELSE ''
                                       	     	  END STRRETURNCOL
                                       FROM {1} 
                                       LEFT JOIN KPDOC ON KEQID = KCATXL AND KCATYPD <> 'E'
                                       LEFT JOIN KPDOCEXT ON KERID = KCATXL AND KCATYPD = 'E'
                                       WHERE KCAID = {0}", !string.IsNullOrEmpty(clauseId) ? clauseId : "0",
                                                           CommonRepository.GetPrefixeHisto(modeNavig, "KPCLAUSE"));
            return CommonRepository.GetStrValue(sql);

            //var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            //if (result != null && result.Any())
            //    return result.FirstOrDefault().StrReturnCol;
            //return string.Empty;
        }

        #endregion

        #region Méthodes Privées

        /// <summary>
        /// Récupère la liste des rubriques
        /// </summary>
        public static List<ParametreDto> GetListRubrique()
        {
            string sql = @"SELECT DISTINCT KDUNM1 CODE, KDUNM1 LIBELLE FROM KCLAUSE ORDER BY KDUNM1";
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        /// <summary>
        /// Récupère la liste des sous-rubriques
        /// </summary>
        private static List<ParametreDto> GetListSousRubrique()
        {
            string sql = @"SELECT DISTINCT KDUNM2 CODE, KDUNM2 LIBELLE FROM KCLAUSE ORDER BY KDUNM2";
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        /// <summary>
        /// Récupère la liste des mots clés
        /// </summary>
        private static List<ParametreDto> GetListMotsCles()
        {
            string sql = @"SELECT CAST(KDXID AS CHAR(15)) CODE, KDXMOC LIBELLE FROM KMOTCLE ORDER BY KDXID";
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        /// <summary>
        /// Vérifie si une clause est
        /// déjà présente dans KPCLAUSE
        /// </summary>
        private static bool VerifClause(string codeOffre, string version, string type, string rubrique, string sousrubrique, string sequence, string contexte, string codeRsq, string codeObj, string codeFor, string codeOpt)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM KPCLAUSE
                                                WHERE KCAIPB = '{0}' AND KCAALX = {1} AND KCATYP = '{2}' AND KCACLNM1 = '{3}' AND  KCACLNM2='{4}' AND  KCACLNM3='{5}' AND KCACTX = '{10}' AND KCARSQ = {6} AND KCAOBJ = {7} AND KCAFOR = {8} AND KCAOPT = {9}",
                                         codeOffre.PadLeft(9, ' '), !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0, type, rubrique, sousrubrique, !string.IsNullOrEmpty(sequence) ? Convert.ToInt64(sequence) : 0,
                                         !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0, !string.IsNullOrEmpty(codeObj) ? Convert.ToInt32(codeObj) : 0,
                                         !string.IsNullOrEmpty(codeFor) ? Convert.ToInt32(codeFor) : 0, !string.IsNullOrEmpty(codeOpt) ? Convert.ToInt32(codeOpt) : 0, contexte);
            return CommonRepository.ExistRow(sql);
        }

        /// <summary>
        /// Insère une clause dans KPCLAUSE
        /// </summary>
        private static void InsertClause(string codeOffre, string version, string type, string etape, string perimetre, string codeRsq, string codeObj, string codeFor, string codeOpt,
                        string contexte, string codeClause, string rubrique, string sousrubrique, string sequence, string versionClause, string numAvenant)
        {
            int? dateNow = AlbConvert.ConvertDateToInt(DateTime.Now);

            int numAvn = 0;
            int.TryParse(numAvenant, out numAvn);

            if (codeObj != "0" && !string.IsNullOrEmpty(codeObj))
                etape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet);

            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("step", DbType.AnsiStringFixedLength);
            param[0].Value = etape.Trim();
            param[1] = new EacParameter("context", DbType.AnsiStringFixedLength);
            param[1].Value = contexte.Trim();

            string sqlRecupChapSousChap = @"SELECT KEICHI STRRETURNCOL, KEICHIS STRRETURNCOL2 
                                                          FROM KALCONT
                                                          WHERE KEIETAPE = :step AND KEICTX = :context ";

            var resultChap = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlRecupChapSousChap, param);

            string chapitre = resultChap != null && resultChap.Any() ? !string.IsNullOrEmpty(resultChap.FirstOrDefault().StrReturnCol) ? resultChap.FirstOrDefault().StrReturnCol : string.Empty : string.Empty;
            string sousChapitre = resultChap != null && resultChap.Any() ? !string.IsNullOrEmpty(resultChap.FirstOrDefault().StrReturnCol2) ? resultChap.FirstOrDefault().StrReturnCol2 : string.Empty : string.Empty;

            param = new EacParameter[40];
            param[0] = new EacParameter("idClause", 0);
            param[0].Value = CommonRepository.GetAS400Id("KCAID");
            param[1] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("version", 0);
            param[2].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[3] = new EacParameter("type", type);
            param[4] = new EacParameter("etape", etape);
            param[5] = new EacParameter("perimetre", perimetre);
            param[6] = new EacParameter("codeRsq", 0);
            param[6].Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0;
            param[7] = new EacParameter("codeObj", 0);
            param[7].Value = !string.IsNullOrEmpty(codeObj) ? Convert.ToInt32(codeObj) : 0;
            param[8] = new EacParameter("codeInven", 0);
            param[8].Value = 0;
            param[9] = new EacParameter("numInven", 0);
            param[9].Value = 0;
            param[10] = new EacParameter("codeFor", 0);
            param[10].Value = !string.IsNullOrEmpty(codeFor) ? Convert.ToInt32(codeFor) : 0;
            param[11] = new EacParameter("codeOpt", 0);
            param[11].Value = !string.IsNullOrEmpty(codeOpt) ? Convert.ToInt32(codeOpt) : 0;
            param[12] = new EacParameter("codeGar", "");
            param[13] = new EacParameter("contexte", contexte);
            param[14] = new EacParameter("ajout", "O");
            //param[15] = new EacParameter("nature", "P");
            param[15] = new EacParameter("nature", string.Empty);
            param[16] = new EacParameter("rubrique", rubrique);
            param[17] = new EacParameter("sousRubrique", sousrubrique);
            param[18] = !string.IsNullOrEmpty(sequence) ? new EacParameter("sequence", Convert.ToInt64(sequence)) : new EacParameter("sequence", 0);
            param[19] = new EacParameter("versionClause", 0);
            param[19].Value = !string.IsNullOrEmpty(versionClause) ? Convert.ToInt32(versionClause) : 0;
            param[20] = new EacParameter("clauseLibre", 0);
            param[20].Value = 0;
            param[21] = new EacParameter("clauseMere", 0);
            param[21].Value = 0;
            param[22] = new EacParameter("docImpr", "CP");
            param[23] = new EacParameter("chpImpr", chapitre);
            param[24] = new EacParameter("sschpImpr", sousChapitre);
            param[25] = new EacParameter("numImpr", "0");
            param[26] = new EacParameter("numOrdImpr", "0");
            param[27] = new EacParameter("imprAnnexe", "N");
            param[28] = new EacParameter("codeAnnexe", "");
            param[29] = new EacParameter("codeSituation", "V");
            param[30] = new EacParameter("dateSituation", 0);
            param[30].Value = dateNow;
            param[31] = new EacParameter("avenantCreation", 0);
            param[31].Value = numAvn;
            param[32] = new EacParameter("dateCreation", 0);
            param[32].Value = dateNow;
            param[33] = new EacParameter("avenantModif", 0);
            param[33].Value = numAvn;
            param[34] = new EacParameter("dateModif", 0);
            param[34].Value = dateNow;
            param[35] = new EacParameter("clauseSpecAvt", "N");
            param[36] = new EacParameter("typoClause", "");
            param[37] = new EacParameter("attrImpr", "O");
            param[38] = new EacParameter("actionEnchainee", "");
            param[39] = new EacParameter("codeClause", codeClause);

            string sql = @"INSERT INTO KPCLAUSE
                            (KCAID, KCAIPB, KCAALX, KCATYP, KCAETAFF, KCAPERI, KCARSQ, KCAOBJ, KCAINVEN, KCAINLGN, KCAFOR, KCAOPT, KCAGAR, KCACTX,
                                KCAAJT, KCANTA, KCACLNM1,KCACLNM2,KCACLNM3, KCAVER, KCATXL, KCAMER, KCADOC, KCACHI, KCACHIS, KCAIMP, KCACXI, KCAIAN, 
                                KCAIAC, KCASIT, KCASITD, KCAAVNC, KCACRD, KCAAVNM, KCAMAJD, KCASPA, KCATYPO, KCAAIM, KCATAE,KCAKDUID)
                                VALUES
                            (:idClause, :codeOffre, :version, :type, :etape, :perimetre, :codeRsq, :codeObj, :codeInven, :numInven, :codeFor, :codeOpt, :codeGar, :contexte,
                                :ajout, :nature, :rubrique,:sousRubrique,:sequence, :versionClause, :clauseLibre, :clauseMere, :docImpr, :chpImpr, :sschpImpr, :numImpr, :numOrdImpr, :imprAnnexe, 
                                :codeAnnexe, :codeSituation, :dateSituation, :avenantCreation, :dateCreation, :avenantModif, :dateModif, :clauseSpecAvt, :typoClause, :attrImpr, :actionEnchainee,:codeClause)";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static void UpdateClause(string codeOffre, string version, string type, string etape, string perimetre, string codeRsq, string codeObj, string codeFor, string codeOpt,
                        string contexte, string rubrique, string sousrubrique, string sequence, string versionClause)
        {
            int? dateNow = AlbConvert.ConvertDateToInt(DateTime.Now);

            DbParameter[] param = new DbParameter[12];
            param[0] = new EacParameter("contexte", contexte);
            param[1] = new EacParameter("versionClause", 0);
            param[1].Value = !string.IsNullOrEmpty(versionClause) ? Convert.ToInt32(versionClause) : 0;
            param[2] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
            param[3] = new EacParameter("version", 0);
            param[3].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[4] = new EacParameter("type", type);
            param[5] = new EacParameter("codeRsq", 0);
            param[5].Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0;
            param[6] = new EacParameter("codeObj", 0);
            param[6].Value = !string.IsNullOrEmpty(codeObj) ? Convert.ToInt32(codeObj) : 0;
            param[7] = new EacParameter("codeFor", 0);
            param[7].Value = !string.IsNullOrEmpty(codeFor) ? Convert.ToInt32(codeFor) : 0;
            param[8] = new EacParameter("codeOpt", 0);
            param[8].Value = !string.IsNullOrEmpty(codeOpt) ? Convert.ToInt32(codeOpt) : 0;
            param[9] = new EacParameter("rubrique", rubrique);
            param[10] = new EacParameter("sousRubrique", rubrique);
            param[11] = !string.IsNullOrEmpty(sequence) ? new EacParameter("sequence", Convert.ToInt64(sequence)) : new EacParameter("sequence", 0);

            string sql = @"UPDATE KPCLAUSE
                                SET KCACTX = :contexte, KCAVER = :versionClause
                            WHERE KCAIPB = :codeOffre AND KCAALX = :version AND KCATYP = :type AND KCARSQ = :codeRsq AND KCAOBJ = :codeObj AND KCAFOR = :codeFor AND KCAOPT = :codeOpt AND KCACLNM1 = :rubrique AND KCACLNM2= :sousRubrique AND KCACLNM3= :sequence";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        #endregion

        public static List<ClauseDto> ObtenirClauses(string type, string numeroOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig)
        {
            long sequenceClause = !string.IsNullOrEmpty(sequence) ? Convert.ToInt64(sequence) : 0;
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
            //TODO : voir s'il on utilise HPDOC/HPDOCEXT pour l'historique
            string sql = string.Format(@"SELECT 
	                                CASE TBLCLAUSE.KCAXTL WHEN 'O' THEN 1 ELSE 0 END AS ISCLAUSELIBRE,	                                
	                                TBLCLAUSE.KCAID ID,
                                    TBLCLAUSE.KCARSQ CODERISQUE,KABDESC DESCRISQUE,
	                                TBLCLAUSE.KCAOBJ CODEOBJET,KACDESC DESCOBJET,
	                                TBLCLAUSE.KCAFOR CODEFORMULE,KDADESC DESCFORMULE,KDAALPHA LETTREFORMULE,
	                                TBLCLAUSE.KCACLNM1 RUBRIQUE,
                                    TBLCLAUSE.KCACLNM2 SOUSRUBRIQUE,
                                    TBLCLAUSE.KCACLNM3 SEQUEN,
	                                IFNULL(KDULIB, KEQNOM) LEDES,
	                                TBLCLAUSE.KCAVER NUMEROVERSION,
	                                KDUDATD COLUMNDATEEFFETVERSIONDEBUT,	                                
	                                KDUDATF COLUMNDATEEFFETVERSIONFIN,
	                                TBLCLAUSE.KCAAJT ORIGINE,
	                                TBLCLAUSE.KCAETAFF ETAPE,
	                                TBLCLAUSE.KCACTX CONTEXTE, 
                                    TPLIB CTXLBL,
	                                TBLCLAUSE.KCASIT CODESITUATION, 
	                                TBLCLAUSE.KCASITD DATESITUATION, 
	                                TBLCLAUSE.KCAAVNC NUMEROAVENANTCREATION, 
	                                TBLCLAUSE.KCACRD DATEAVENANTCREATION, 
	                                TBLCLAUSE.KCAAVNM NUMEROAVENANTMODIFICATION, 
	                                TBLCLAUSE.KCAMAJD DATEAVENANTMODIFICATION, 
	                                TBLCLAUSE.KCADOC DOCUMENT, 
	                                TBLCLAUSE.KCACHI CHAPITRE, 
	                                TBLCLAUSE.KCACHIS SOUSCHAPITRE, 
	                                TBLCLAUSE.KCAIMP NUMEROIMPRESSION,
	                                TBLCLAUSE.KCACXI NUMEROORDRE, 
	                                TBLCLAUSE.KCAIAN IMPRESSIONANNEXE, 
	                                TBLCLAUSE.KCAIAC CODEANNEXE, 
	                                TBLCLAUSE.KCATAE ACTIONENCHAINE, 
	                                TBLCLAUSE.KCANTA ETATTITRE, 
	                                TBLCLAUSE.KCAAIM ATTRIBUTVERSION,
	                                '' TEXTECLAUSELIBRE,
                                    TBLCLAUSE.KCAGAR CODEGARANTIE,GADEA LIBELLEGARANTIE,
                                    TBLCLAUSE.KCAKDUID CLAUSEID, TBLCLAUSE.KCATYPD DOCAJOUTE, IFNULL(KERLIB, IFNULL(KDULIB, KEQNOM)) DOCLIB,
                                    TBLCLAUSE.KCAXTLM ISMODIFSTR,
                                    CASE WHEN KEQCHM IS NOT NULL THEN KEQCHM
                                       	 WHEN KERCHM IS NOT NULL THEN KERCHM
                                       	 ELSE ''
                                       	 END CHEMINFICHIER,
                                    CASE WHEN TBLCLAUSE.KCANTA = 'O' THEN CAST(IFNULL(HISTOBASE.PBMJA,0) * 10000 + IFNULL(HISTOBASE.PBMJM,0) * 100 + IFNULL(HISTOBASE.PBMJJ,0) AS NUMERIC(8,0))
                                         WHEN TBLCLAUSE.KCANTA = 'P' AND TBLCLAUSE.KCASIT = 'V' AND TBLCLAUSE.KCATYPD != 'E' THEN CAST(IFNULL(HISTOBASE.PBMJA,0) * 10000 + IFNULL(HISTOBASE.PBMJM,0) * 100 + IFNULL(HISTOBASE.PBMJJ,0) AS NUMERIC(8,0))
                                         WHEN TBLCLAUSE.KCANTA = 'P' AND TBLCLAUSE.KCASIT = 'V' AND TBLCLAUSE.KCATYPD = 'E' THEN TBLCLAUSE.KCASITD
                                         WHEN TBLCLAUSE.KCANTA = 'S' AND TBLCLAUSE.KCASIT = 'V' THEN TBLCLAUSE.KCASITD
                                         WHEN TBLCLAUSE.KCAAJT = 'O' AND TBLCLAUSE.KCASIT = 'V' THEN TBLCLAUSE.KCASITD
                                         ELSE 0
                                         END DATEDETAILAVNCREATION,
                                    HISTOCLAUSE.KCASIT CODESITUATIONHISTO,
                                    HISTOCLAUSE.KCAAVNM NUMAVENANTHISTO
                                FROM {0} TBLCLAUSE
                                {1}
                                LEFT JOIN {2} ON TBLCLAUSE.KCACLNM1 = KCLAUSE.KDUNM1 AND  TBLCLAUSE.KCACLNM2 = KCLAUSE.KDUNM2 AND TBLCLAUSE.KCACLNM3 = KCLAUSE.KDUNM3 AND TBLCLAUSE.KCAKDUID=KCLAUSE.KDUID 
                                LEFT JOIN {3} ON TBLCLAUSE.KCATXL = KEQID AND TBLCLAUSE.KCATYPD <> 'E' 
                                LEFT JOIN {4} ON KABRSQ=TBLCLAUSE.KCARSQ AND  KABIPB=TBLCLAUSE.KCAIPB AND KABALX=TBLCLAUSE.KCAALX  AND KABTYP=TBLCLAUSE.KCATYP {13} 
                                LEFT JOIN {5} ON KDAFOR=TBLCLAUSE.KCAFOR AND KDAIPB=TBLCLAUSE.KCAIPB AND KDAALX=TBLCLAUSE.KCAALX AND KDATYP=TBLCLAUSE.KCATYP {14}
                                LEFT JOIN {6} ON KACOBJ=TBLCLAUSE.KCAOBJ AND KACIPB=TBLCLAUSE.KCAIPB AND KACALX=TBLCLAUSE.KCAALX AND KACTYP=TBLCLAUSE.KCATYP AND KACRSQ = KABRSQ {15}
                                LEFT JOIN {7} ON GAGAR =TBLCLAUSE.KCAGAR
                                LEFT JOIN {8} ON TBLCLAUSE.KCATXL = KERID AND TBLCLAUSE.KCATYPD = 'E' 
                                LEFT JOIN YHPBASE HISTOBASE ON PBIPB = TBLCLAUSE.KCAIPB AND PBALX = TBLCLAUSE.KCAALX AND PBTYP = TBLCLAUSE.KCATYP AND PBAVN = TBLCLAUSE.KCAAVNC  
                            
                                LEFT JOIN HPCLAUSE HISTOCLAUSE ON HISTOCLAUSE.KCAIPB = TBLCLAUSE.KCAIPB AND HISTOCLAUSE.KCATYP = TBLCLAUSE.KCATYP AND HISTOCLAUSE.KCAALX = TBLCLAUSE.KCAALX AND HISTOCLAUSE.KCARSQ = TBLCLAUSE.KCARSQ AND HISTOCLAUSE.KCAOBJ = TBLCLAUSE.KCAOBJ AND HISTOCLAUSE.KCAFOR = TBLCLAUSE.KCAFOR AND HISTOCLAUSE.KCAOPT = TBLCLAUSE.KCAOPT AND HISTOCLAUSE.KCAGAR = TBLCLAUSE.KCAGAR AND HISTOCLAUSE.KCAETAFF = TBLCLAUSE.KCAETAFF AND HISTOCLAUSE.KCACTX = TBLCLAUSE.KCACTX AND HISTOCLAUSE.KCACLNM1 = TBLCLAUSE.KCACLNM1 AND HISTOCLAUSE.KCACLNM2 = TBLCLAUSE.KCACLNM2 AND HISTOCLAUSE.KCACLNM3 = TBLCLAUSE.KCACLNM3 AND HISTOCLAUSE.KCAAVN = {16} 
                                WHERE TBLCLAUSE.KCATYP='{9}' AND TBLCLAUSE.KCAIPB='{10}' AND TBLCLAUSE.KCAALX='{11}' {12}",
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPCLAUSE"),
                     CommonRepository.BuildJoinYYYYPAR("INNER", "KHEOP", "CTX", otherCriteria: " AND TCOD = TBLCLAUSE.KCACTX"),
                     //CommonRepository.GetPrefixeHisto(modeNavig, "KCLAUSE"),
                     "KCLAUSE",
                     //CommonRepository.GetPrefixeHisto(modeNavig, "KPDOC"),
                     "KPDOC",
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                     CommonRepository.GetPrefixeHisto(modeNavig, "KGARAN"),
                     //CommonRepository.GetPrefixeHisto(modeNavig, "KPDOCEXT"),
                     "KPDOCEXT",
                     type, numeroOffre.PadLeft(9, ' '), version,
                     modeNavig == ModeConsultation.Historique ? string.Format(" AND TBLCLAUSE.KCAAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? iCodeAvn : 0) : string.Empty,
                     modeNavig == ModeConsultation.Historique ? " AND KABAVN = TBLCLAUSE.KCAAVN" : string.Empty,
                     modeNavig == ModeConsultation.Historique ? " AND KDAAVN = TBLCLAUSE.KCAAVN" : string.Empty,
                     modeNavig == ModeConsultation.Historique ? " AND KACAVN = TBLCLAUSE.KCAAVN" : string.Empty,
                     iCodeAvn - 1);

            if (!string.IsNullOrEmpty(codeRisque))
                sql += string.Format(@" AND TBLCLAUSE.KCARSQ='{0}'", Convert.ToInt32(codeRisque));
            if (!string.IsNullOrEmpty(codeFormule))
                sql += string.Format(@" AND TBLCLAUSE.KCAFOR='{0}'", Convert.ToInt32(codeFormule));
            if (!string.IsNullOrEmpty(codeOption))
                sql += string.Format(@" AND TBLCLAUSE.KCAOPT='{0}'", Convert.ToInt32(codeOption));
            if (contexte != "Tous")
                sql += string.Format(@" AND TBLCLAUSE.KCACTX='{0}'", contexte);
            if (!string.IsNullOrEmpty(rubrique))
                sql += string.Format(@" AND TBLCLAUSE.KCACLNM1='{0}'", rubrique);
            if (!string.IsNullOrEmpty(sousrubrique))
                sql += string.Format(@" AND TBLCLAUSE.KCACLNM2='{0}'", sousrubrique);
            if (!string.IsNullOrEmpty(sequence))
                sql += string.Format(@" AND TBLCLAUSE.KCACLNM3='{0}'", sequenceClause);
            if (!string.IsNullOrEmpty(idClause))
                sql += string.Format(@" AND TBLCLAUSE.KCAID='{0}'", idClause);

            if (!string.IsNullOrEmpty(filtre) && filtre != "T")
            {
                if (filtre == AlbConstantesMetiers.ToutesSaufObligatoires)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA!='{0}'", "O");
                if (filtre == AlbConstantesMetiers.Obligatoires)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA='{0}'", "O");
                if (filtre == AlbConstantesMetiers.Proposes)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA='{0}'", "P");
                if (filtre == AlbConstantesMetiers.Suggerees)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA='{0}'", "S");
                if (filtre == AlbConstantesMetiers.Ajoutees)
                    sql += string.Format(@" AND TBLCLAUSE.KCAAJT='{0}'", "O");
            }

            if (!string.IsNullOrEmpty(etape))
            {
                AlbConstantesMetiers.Etapes enumEtape = AlbEnumInfoValue.GetEnumValue< AlbConstantesMetiers.Etapes>(etape);
                switch (enumEtape)
                {
                    case AlbConstantesMetiers.Etapes.Garantie:
                        sql += string.Format(@" AND (TBLCLAUSE.KCAETAFF ='{0}' OR TBLCLAUSE.KCAETAFF ='{1}')", AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option), AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie));
                        break;
                    case AlbConstantesMetiers.Etapes.Risque:
                        sql += string.Format(@" AND (TBLCLAUSE.KCAETAFF = '{0}' OR TBLCLAUSE.KCAETAFF = '{1}')", AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque), AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet));
                        break;
                    default:
                        sql += string.Format(@" AND TBLCLAUSE.KCAETAFF ='{0}'", etape);
                        break;
                }
            }
            sql += " ORDER BY TBLCLAUSE.KCAETAFF, TBLCLAUSE.KCARSQ, TBLCLAUSE.KCAOBJ, TBLCLAUSE.KCAFOR";
            var toReturn = DbBase.Settings.ExecuteList<ClauseDto>(CommandType.Text, sql);
            if (toReturn.Any())
            {
                foreach (var clause in toReturn) {
                    clause.DateEffetVersionDebut = AlbConvert.ConvertIntToDate(clause.ColumnDateEffetVersionDebut);
                    clause.DateEffetVersionFin = AlbConvert.ConvertIntToDate(clause.ColumnDateEffetVersionFin);
                    MapToClause(iCodeAvn, modeNavig, clause);
                }


                if (toReturn.Count == 1 && toReturn.FirstOrDefault().Id > 0)
                {

                    string sqlHisto = string.Format(@"SELECT IFNULL((HISTOCLAUSE.KCAAVNC),-1) INT64RETURNCOL, IFNULL((HISTOCLAUSE.KCACRD),0) DATEDEBRETURNCOL, HISTOCLAUSE.KCASIT SITUATION
                                                      FROM {0} TBLCLAUSE
                                                      LEFT JOIN HPCLAUSE HISTOCLAUSE 
                                                      ON HISTOCLAUSE.KCAIPB = TBLCLAUSE.KCAIPB AND HISTOCLAUSE.KCATYP = TBLCLAUSE.KCATYP AND HISTOCLAUSE.KCAALX = TBLCLAUSE.KCAALX AND HISTOCLAUSE.KCARSQ = TBLCLAUSE.KCARSQ AND HISTOCLAUSE.KCAOBJ = TBLCLAUSE.KCAOBJ AND HISTOCLAUSE.KCAFOR = TBLCLAUSE.KCAFOR AND HISTOCLAUSE.KCAOPT = TBLCLAUSE.KCAOPT AND HISTOCLAUSE.KCAGAR = TBLCLAUSE.KCAGAR AND HISTOCLAUSE.KCAETAFF = TBLCLAUSE.KCAETAFF AND HISTOCLAUSE.KCACTX = TBLCLAUSE.KCACTX AND HISTOCLAUSE.KCACLNM1 = TBLCLAUSE.KCACLNM1 AND HISTOCLAUSE.KCACLNM2 = TBLCLAUSE.KCACLNM2 AND HISTOCLAUSE.KCACLNM3 = TBLCLAUSE.KCACLNM3                                        
                                                      WHERE TBLCLAUSE.KCAID = {1} order by HISTOCLAUSE.KCAAVNC desc fetch first 1 rows only", CommonRepository.GetPrefixeHisto(modeNavig, "KPCLAUSE"), toReturn.FirstOrDefault().Id);

                    var detailHisto = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlHisto);
                    if (detailHisto != null && detailHisto.Any())
                    {
                        //Si un résultat a été trouvé et qu'il n'est pas égale à -1
                        if (detailHisto.FirstOrDefault().Int64ReturnCol > -1 && detailHisto.FirstOrDefault().Situation == "V")
                        {
                            toReturn.FirstOrDefault().NumAvnDetailClauseAjout = detailHisto.FirstOrDefault().Int64ReturnCol.ToString();
                            int iDateCreationDetailAvn = 0;
                            int.TryParse(detailHisto.FirstOrDefault().DateDebReturnCol.ToString(), out iDateCreationDetailAvn);
                            toReturn.FirstOrDefault().DateDetailAvnCreation = AlbConvert.ConvertIntToDate(iDateCreationDetailAvn);
                        }

                    }

                    if (string.IsNullOrEmpty(toReturn.FirstOrDefault().NumAvnDetailClauseAjout) && toReturn.FirstOrDefault().CodeSituation == "V")
                    {
                        toReturn.FirstOrDefault().NumAvnDetailClauseAjout = codeAvn;

                    }

                }
            }
            return toReturn;
        }


        public static List<ClausesContextDto> ObtenirContextesClauses(string type, string numeroOffre, string version, string codeAvn, string etape, string rubrique, string sousRubrique, string sequence, string codeRisque, string codeFormule, string codeOption)
        {
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);

            var param = new List<EacParameter>() {
            new EacParameter("avn", DbType.Int32) {Value = iCodeAvn - 1},
            new EacParameter("type", DbType.AnsiStringFixedLength) {Value = type},
            new EacParameter("codeoffre", DbType.AnsiStringFixedLength) {Value = numeroOffre.PadLeft(9, ' ')},
            new EacParameter("version", DbType.AnsiStringFixedLength) {Value = version}
            };

            string sql = @"SELECT ORIGINEFILTRE,CONTEXTE, ETATTITRE, TPLIB CTXLBL 
                                        FROM V_LISTCONTEXTESTD 
                                        LEFT JOIN HPCLAUSE HISTOCLAUSE ON HISTOCLAUSE.KCAIPB = CODEOFRRE  AND HISTOCLAUSE.KCATYP = TYP  AND HISTOCLAUSE.KCAALX = versionoffre  AND HISTOCLAUSE.KCARSQ = CODERISQUE
                                        AND HISTOCLAUSE.KCAOBJ = CODEOBJET  AND HISTOCLAUSE.KCAFOR = CODEFORMULE  AND HISTOCLAUSE.KCAOPT = CODEOPT AND HISTOCLAUSE.KCAGAR = CODEGARANTIE 
                                        AND HISTOCLAUSE.KCAETAFF = ETAPE AND HISTOCLAUSE.KCACTX = CONTEXTE 
                                        AND HISTOCLAUSE.KCACLNM1 = RUBRIQUE AND HISTOCLAUSE.KCACLNM2 = SOUSRUBRIQUE AND HISTOCLAUSE.KCACLNM3 = SEQUEN AND HISTOCLAUSE.KCAAVN = :avn
                                        INNER JOIN YYYYPAR CTX ON CTX.TCON = 'KHEOP' 
                                        AND CTX.TFAM = 'CTX' AND CTX.TCOD = CONTEXTE 
                                        WHERE TYP=:type AND CODEOFRRE=:codeoffre AND versionoffre=:version";
            //iCodeAvn - 1, type, numeroOffre.PadLeft(9, ' '), version);

            if (!string.IsNullOrEmpty(codeRisque))
            {
                sql += @" AND CODERISQUE =:rsq";
                param.Add(new EacParameter("rsq", DbType.Int32) { Value = Convert.ToInt32(codeRisque) });

            }
            //Convert.ToInt32(codeRisque));
            if (!string.IsNullOrEmpty(codeFormule))
            {
                sql += @" AND CODEFORMULE=:for";
                param.Add(new EacParameter("for", DbType.Int32) { Value = Convert.ToInt32(codeFormule) });

            }
            //Convert.ToInt32(codeFormule));
            if (!string.IsNullOrEmpty(codeOption))
            {
                sql += @" AND CODEOPT=:opt";
                param.Add(new EacParameter("opt", DbType.AnsiStringFixedLength) { Value = Convert.ToInt32(codeOption) });
            }
            //Convert.ToInt32(codeOption));
            if (!string.IsNullOrEmpty(rubrique))
            {
                sql += @" AND RUBRIQUE=:rubrique";
                param.Add(new EacParameter("rubrique", DbType.AnsiStringFixedLength) { Value = rubrique });

            }
            //rubrique);
            if (!string.IsNullOrEmpty(sousRubrique))
            {
                sql += @" AND SOUSRUBRIQUE=:sousrubrique";
                param.Add(new EacParameter("sousrubrique", DbType.AnsiStringFixedLength) { Value = sousRubrique });
            }
            //sousRubrique);
            if (!string.IsNullOrEmpty(sequence))
            {
                sql += @" AND SEQUEN=:seq";
                param.Add(new EacParameter("seq", DbType.AnsiStringFixedLength) { Value = sequence });

            }
            //sequence);
            if (!string.IsNullOrEmpty(etape))
            {
                sql += @" AND ETAPE=:step";
                param.Add(new EacParameter("step", DbType.AnsiStringFixedLength) { Value = etape });

            }
            //etape);

            var toReturn = DbBase.Settings.ExecuteList<ClausesContextDto>(CommandType.Text, sql, param);
            return toReturn;
        }

        public static void SauvegarderEtatTitre(List<ClauseDto> clauses)
        {
            if (clauses != null)
            {
                string sql = string.Empty;
                foreach (ClauseDto cls in clauses)
                {
                    EacParameter[] param = new EacParameter[2];
                    param[0] = new EacParameter("etat", DbType.AnsiStringFixedLength);
                    param[0].Value = cls.EtatTitre;
                    param[1] = new EacParameter("id", DbType.Int64);
                    param[1].Value = cls.Id;

                    sql = @"UPDATE KPCLAUSE SET KCANTA=:etat WHERE KCAID=:id";
                    //cls.Id, cls.EtatTitre);
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                }
            }
        }

        public static void SupprimerClause(string id)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("id", DbType.AnsiStringFixedLength);
            param[0].Value = id;

            //Récupération du chemin du document de la clause si il existe
            string sql = @"SELECT KEQCHM STRRETURNCOL FROM KPDOC WHERE KEQID = (SELECT KCATXL FROM KPCLAUSE WHERE KCAID = :id)";
            //id);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                //Suppression de la référence du document
                sql = @"DELETE FROM KPDOC 
                                  WHERE KEQID IN (SELECT KCATXL FROM KPCLAUSE WHERE KCAID=:id)";
                //id);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                //Suppression physique du document
                string fullPath = result.FirstOrDefault().StrReturnCol;
                if (!string.IsNullOrEmpty(fullPath))
                {
                    var tabStr = fullPath.Split('\\');
                    string fileName = tabStr != null && tabStr.Length > 0 ? tabStr[tabStr.Length - 1] : string.Empty;
                    string path = fullPath.Substring(0, fullPath.Length - fileName.Length);

                    if (!IOFileManager.IsExistDirectory(path))
                        throw new ALBINGIA.Framework.Common.AlbingiaExceptions.AlbFoncException("Le dossier cible n'existe pas", trace: false, sendMail: false, onlyMessage: true);
                    if (IOFileManager.IsExistFile(path, fileName))
                        IOFileManager.DeleteFile(fileName, path);
                }
            }
            //Suppression de la clause
            sql = @"DELETE FROM KPCLAUSE WHERE KCAID=:id";
            //id);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            //            string sql = string.Empty;
            //            sql = string.Format(@"DELETE FROM KPCLATXT 
            //                                WHERE KFOID IN (SELECT KCATXL FROM KPCLAUSE WHERE KCAID='{0}')", id);
            //            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            //            sql = string.Format(@"DELETE FROM KPCLAUSE WHERE KCAID='{0}'", id);
            //            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        public static void UpdateTextClauseLibre(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeObj)
        {
            string text = !string.IsNullOrEmpty(texteClauseLibre) ? texteClauseLibre.Trim() : string.Empty;
            string titre = titreClauseLibre.Trim();

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("title", DbType.AnsiStringFixedLength);
            param[0].Value = titre;
            param[1] = new EacParameter("text", DbType.AnsiStringFixedLength);
            param[1].Value = text;
            param[2] = new EacParameter("idclause", DbType.AnsiStringFixedLength);
            param[2].Value = clauseId;

            string sql = @"UPDATE KPCLATXT 
                           SET KFODESC = :title, KFOTXT=:text
                           WHERE  KFOID IN (SELECT KCATXL FROM KPCLAUSE WHERE KCAID =:idclause)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            param = new EacParameter[2];
            param[0] = new EacParameter("codeObj", 0);
            param[0].Value = !string.IsNullOrEmpty(codeObj) ? Convert.ToInt32(codeObj) : 0;
            param[1] = new EacParameter("clauseId", 0);
            param[1].Value = Convert.ToInt32(clauseId);
            sql = @"UPDATE KPCLAUSE SET KCAOBJ = :codeObj WHERE KCAID = :clauseId";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        public static void UpdateTextClauseLibreOffreSimp(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeRisque, string codeObj, string codeFormule, string codeOption)
        {
            string text = !string.IsNullOrEmpty(texteClauseLibre) ? texteClauseLibre.Trim() : string.Empty;
            string titre = titreClauseLibre.Trim();

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("title", DbType.AnsiStringFixedLength);
            param[0].Value = titre;
            param[1] = new EacParameter("text", DbType.AnsiStringFixedLength);
            param[1].Value = text;
            param[2] = new EacParameter("idclause", DbType.AnsiStringFixedLength);
            param[2].Value = clauseId;

            string sql = @"UPDATE KPCLATXT 
                           SET KFODESC = :title, KFOTXT=:text
                           WHERE  KFOID IN (SELECT KCATXL FROM KPCLAUSE WHERE KCAID =:idclause)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            param = new EacParameter[5];
            param[0] = new EacParameter("codeObj", 0);
            param[0].Value = !string.IsNullOrEmpty(codeObj) ? Convert.ToInt32(codeObj) : 0;
            param[1] = new EacParameter("codeRisque", 0);
            param[1].Value = !string.IsNullOrEmpty(codeRisque) ? Convert.ToInt32(codeRisque) : 0;
            param[2] = new EacParameter("codeFormule", 0);
            param[2].Value = !string.IsNullOrEmpty(codeFormule) ? Convert.ToInt32(codeFormule) : 0;
            param[3] = new EacParameter("codeOption", 0);
            param[3].Value = !string.IsNullOrEmpty(codeOption) ? Convert.ToInt32(codeOption) : 0;
            param[4] = new EacParameter("clauseId", 0);
            param[4].Value = Convert.ToInt32(clauseId);
            sql = @"UPDATE KPCLAUSE SET KCAOBJ = :codeObj,
                                        KCARSQ = :codeRisque,
                                        KCAFOR = :codeFormule,
                                        KCAOPT = :codeOption
                                        WHERE KCAID = :clauseId";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static ClauseLibreViewerDto GetInfoClauseLibreViewer(string codeOffre, string version, string type, string codeRsq, string clauseId, string etape, string contexte)
        {
            ClauseLibreViewerDto model = new ClauseLibreViewerDto();
            //                                                KDUDATD DATEDEBUT, KDUDATF DATEFIN,
            string sql = "";

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("idclause", DbType.AnsiStringFixedLength);
            param[0].Value = clauseId;

            if (contexte == "clausier")
                sql = @"SELECT KCACTX CONTEXTE, TPLIB LIBCONTEXTE, KCACHI EMPLACEMENT, KCACHIS SOUSEMPLACEMENT, KCACXI ORDRE, 
                                                KCAIPB CODEOFFRE, KCAALX VERSION, KCATYP TYPE, KCARSQ CODERSQ, KCAOBJ CODEOBJ, KCATXL IDDOC,
                                                KCATYPD TYPEAJOUT,
                                                CASE WHEN KERLIB IS NOT NULL THEN KERLIB
                                                ELSE TRIM(IFNULL(TRIM(KDUNM1) CONCAT '-' CONCAT TRIM(KDUNM2) CONCAT '-' CONCAT TRIM(KDUNM3) CONCAT ' - ' CONCAT KDULIB, KEQNOM))
                                                END LEDES,
                                                KDUDATD DATEDEBUT, KDUDATF DATEFIN,
                                                CASE WHEN IFNULL(KDUID, 0) = 0 THEN 'O' ELSE 'N' END USERAJT
                                            FROM KCLAUSE 
                                                LEFT JOIN KPCLAUSE ON KCACLNM1 = KCLAUSE.KDUNM1 AND  KCACLNM2 = KCLAUSE.KDUNM2 AND KCACLNM3 = KCLAUSE.KDUNM3 AND KCAKDUID=KCLAUSE.KDUID 
                                                LEFT JOIN KPDOC ON KCATXL = KEQID AND KCATYPD <> 'E'
                                                LEFT JOIN KPDOCEXT ON KERID = KCATXL AND KCATYPD = 'E'
	                                            LEFT JOIN YYYYPAR ON TCON = 'PRODU' AND TFAM = 'QECTX' AND TCOD = KCACTX
                                            WHERE KDUID = :idclause";
            //clauseId);
            else
                sql = @"SELECT KCACTX CONTEXTE, TPLIB LIBCONTEXTE, KCACHI EMPLACEMENT, KCACHIS SOUSEMPLACEMENT, KCACXI ORDRE, 
                                                KCAIPB CODEOFFRE, KCAALX VERSION, KCATYP TYPE, KCARSQ CODERSQ, KCAOBJ CODEOBJ, KCATXL IDDOC,
                                                KCATYPD TYPEAJOUT,
                                                CASE WHEN KERLIB IS NOT NULL THEN KERLIB
                                                ELSE TRIM(IFNULL(TRIM(KDUNM1) CONCAT '-' CONCAT TRIM(KDUNM2) CONCAT '-' CONCAT TRIM(KDUNM3) CONCAT ' - ' CONCAT KDULIB, KEQNOM))
                                                END LEDES,
                                                CASE WHEN IFNULL(KDUID, 0) = 0 THEN 'O' ELSE 'N' END USERAJT
                                            FROM KPCLAUSE 
                                                LEFT JOIN KCLAUSE ON KCACLNM1 = KCLAUSE.KDUNM1 AND  KCACLNM2 = KCLAUSE.KDUNM2 AND KCACLNM3 = KCLAUSE.KDUNM3 AND KCAKDUID=KCLAUSE.KDUID 
                                                LEFT JOIN KPDOC ON KCATXL = KEQID AND KCATYPD <> 'E'
                                                LEFT JOIN KPDOCEXT ON KERID = KCATXL AND KCATYPD = 'E'
	                                            LEFT JOIN YYYYPAR ON TCON = 'PRODU' AND TFAM = 'QECTX' AND TCOD = KCACTX
                                            WHERE KCAID = :idclause";
            //clauseId);

            var result = DbBase.Settings.ExecuteList<ClauseLibreViewerDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                model = result.FirstOrDefault();
                if (model.CodeRisque > 0)
                {
                    model.AppliqueA = GetLibelleRsqObj(model.CodeOffre, model.Version, model.Type, model.CodeRisque, model.CodeObjet);
                    model.Risque = RisqueRepository.GetRisque(model.CodeOffre, model.Version.ToString(), model.Type, model.CodeRisque.ToString());
                }
                contexte = model.Contexte;
            }
            if (model.Risque == null && !string.IsNullOrEmpty(codeRsq))
            {
                model.CodeRisque = Convert.ToInt32(codeRsq);
                model.Risque = RisqueRepository.GetRisque(codeOffre, version, type, codeRsq);
            }
            model.Emplacements = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "EMP");  //GetListRubrique();// CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "QECTX");


            sql = string.Format(@"SELECT KEIEMOD STRRETURNCOL FROM KALCONT WHERE KEISERV = 'PRODU' AND KEIACTG='**' AND KEIETAPE = '{0}' AND KEICTX = '{1}'", etape, contexte);
            model.Modifiable = CommonRepository.GetStrValue(sql) == "O";

            return model;
        }
        private static string GetLibelleRsqObj(string codeOffre, Int32 version, string type, Int32 codeRsq, Int32 codeObj)
        {
            string sql = string.Empty;

            List<EacParameter> param = new List<EacParameter>()
            {
                new EacParameter("type", DbType.AnsiStringFixedLength) { Value = type },
                new EacParameter("codeoffre", DbType.AnsiStringFixedLength) { Value = codeOffre },
                new EacParameter("version", DbType.Int32) { Value = version },
                new EacParameter("rsq", DbType.Int32){ Value = codeRsq},
            };

            if (codeObj > 0)
            {
                sql = @"SELECT KACDESC STRRETURNCOL FROM KPOBJ WHERE KACTYP = :type AND KACIPB = :codeoffre AND KACALX = :version AND KACRSQ = :rsq AND KACOBJ = :obj";
                param.Add(new EacParameter("obj", DbType.Int32) { Value = codeObj });
                //type, codeOffre, version, codeRsq, codeObj);
            }
            else
            {
                sql = @"SELECT KABDESC STRRETURNCOL FROM KPRSQ WHERE KABTYP = :type AND KABIPB = :codeoffre AND KABALX = :version AND KABRSQ = :rsq";
            }

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return string.Format("{0} - {1}", codeObj > 0 ? codeObj : codeRsq, result.FirstOrDefault().StrReturnCol);
            }

            return string.Empty;
        }

        /// <summary>
        /// Récupère la listes de pièces jointes
        /// </summary>
        public static ChoixClausePieceJointeDto GetListPiecesJointes(string codeOffre, string version, string type, string codeRisque, string codeObjet, string etape, string contexte)
        {
            ChoixClausePieceJointeDto model = new ChoixClausePieceJointeDto { PiecesJointes = new List<ChoixClauseListePieceJointeDto>() };

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sql = @"SELECT DISTINCT KERID PIECEID, KERCRD PIECEDATEAJT, KERACTG PIECEACTEGES, KERAVN AVENANT, KERLIB LIBELLE, KERCHM CHEMIN, KERNOM NOMPIECE, KERREF REFPIECE
                            FROM KPDOCEXT
                                LEFT JOIN YYYYPAR ON TCON = 'KHEOP' AND TFAM = 'ACTGS'
                            WHERE KERIPB = :codeOffre AND KERALX = :version AND KERTYP = :type
                                AND KERID NOT IN (SELECT KCATXL FROM KPCLAUSE WHERE KCAIPB = :codeoffre AND KCAALX = :version AND KCATYP = :type AND KCATYPD = 'E')";
            //codeOffre.Trim(), version, type);

            var result = DbBase.Settings.ExecuteList<ChoixClauseListePieceJointeDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                result.ForEach(m => { m.DateAjout = AlbConvert.ConvertIntToDate(m.PieceDateAjt); });
                model.PiecesJointes = result;
            }

            sql = string.Format(@"SELECT KEIEMOD STRRETURNCOL FROM KALCONT WHERE KEISERV = 'PRODU' AND KEIACTG='**' AND KEIETAPE = '{0}' AND KEICTX = '{1}'", etape, contexte);
            model.Modifiable = CommonRepository.GetStrValue(sql) == "O";

            model.CodeRisque = !string.IsNullOrEmpty(codeRisque) ? Convert.ToInt32(codeRisque) : 0;
            if (model.CodeRisque > 0)
            {
                model.Risque = GetLibelleRsqObj(codeOffre, !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0, type, !string.IsNullOrEmpty(codeRisque) ? Convert.ToInt32(codeRisque) : 0, 0);
                model.AppliqueA = GetLibelleRsqObj(codeOffre, !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0, type, !string.IsNullOrEmpty(codeRisque) ? Convert.ToInt32(codeRisque) : 0, !string.IsNullOrEmpty(codeObjet) ? Convert.ToInt32(codeObjet) : 0);
                model.ObjetsRisqueAll = RisqueRepository.GetRisque(codeOffre, version, type, codeRisque);
            }

            return model;
        }
        /// <summary>
        /// Sauvegarde les pièces jointes sélectionnées pour les clause
        /// </summary>
        public static void SavePiecesJointes(string codeOffre, string version, string type, string contexte, string etape, string codeRsq, string codeObj,
            string codeFor, string codeOpt, string emplacement, string sousemplacement, string ordre, string piecesjointesid)
        {
            DateTime? dateNow = DateTime.Now;

            DbParameter[] param = new DbParameter[14];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_CONTEXTE", contexte);
            param[4] = new EacParameter("P_ETAPE", etape);
            param[5] = new EacParameter("P_DATE", 0);
            param[5].Value = AlbConvert.ConvertDateToInt(dateNow);
            param[6] = new EacParameter("P_CODERISQUE", 0);
            param[6].Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0;
            param[7] = new EacParameter("P_CODEOBJET", 0);
            param[7].Value = !string.IsNullOrEmpty(codeObj) ? Convert.ToInt32(codeObj) : 0;
            param[8] = new EacParameter("P_CODEFORMULE", 0);
            param[8].Value = !string.IsNullOrEmpty(codeFor) ? Convert.ToInt32(codeFor) : 0;
            param[9] = new EacParameter("P_CODEOPTION", 0);
            param[9].Value = !string.IsNullOrEmpty(codeOpt) ? Convert.ToInt32(codeOpt) : 0;
            param[10] = new EacParameter("P_EMPLACEMENT", emplacement);
            param[11] = new EacParameter("P_SOUSEMPLACEMENT", sousemplacement);
            param[12] = new EacParameter("P_ORDRE", 0);
            param[12].Value = !string.IsNullOrEmpty(ordre) ? Convert.ToInt32(ordre) : 0;
            param[13] = new EacParameter("P_PIECESJOINTESID", piecesjointesid);


            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEPIECESJOINTES", param);
        }

        public static void SaveClauseEntete(string idClause, string emplacement, string sousemplacement, string ordre)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("emp", DbType.AnsiStringFixedLength);
            param[0].Value = emplacement.Trim();
            param[1] = new EacParameter("sousemp", DbType.AnsiStringFixedLength);
            param[1].Value = sousemplacement.Trim();
            param[2] = new EacParameter("ordre", DbType.Decimal);
            param[2].Value = !string.IsNullOrEmpty(ordre) ? Convert.ToDecimal(ordre, CultureInfo.GetCultureInfo("en-US")) : 0;
            param[3] = new EacParameter("idclause", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(idClause) ? Convert.ToInt32(idClause) : 0;


            string sql = @"UPDATE KPCLAUSE
                                         SET KCACHI = :emp,
                                              KCACHIS = :sousemp,
                                              KCACXI = :ordre
                                         WHERE KCAID = :idclause";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Enregistrement des clauses libres à partir de l'outil MAGNETIC
        /// </summary>
        public static void SaveClauseMagnetic(string codeAffaire, string version, string type, int idDoc, string acteGes, string etape, string nameClause, string fileName,
            int idClause, string emplacement, string sousemplacement, string ordre, string contexte, bool isModif)
        {
            #region Enregistrement du document dans KPDOC
            if (idDoc == 0)
            {
                idDoc = CommonRepository.GetAS400Id("KEQID");

                EacParameter[] paramIns = new EacParameter[11];
                paramIns[0] = new EacParameter("idDoc", DbType.Int32);
                paramIns[0].Value = idDoc;
                paramIns[1] = new EacParameter("codeAffaire", DbType.AnsiStringFixedLength);
                paramIns[1].Value = codeAffaire.PadLeft(9, ' ');
                paramIns[2] = new EacParameter("version", DbType.Int32);
                paramIns[2].Value = Convert.ToInt32(version);
                paramIns[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
                paramIns[3].Value = type;
                paramIns[4] = new EacParameter("service", DbType.AnsiStringFixedLength);
                paramIns[4].Value = "PRODU";
                paramIns[5] = new EacParameter("acteGes", DbType.AnsiStringFixedLength);
                paramIns[5].Value = acteGes;
                paramIns[6] = new EacParameter("etape", DbType.AnsiStringFixedLength);
                paramIns[6].Value = etape;
                paramIns[7] = new EacParameter("typeDoc", DbType.AnsiStringFixedLength);
                paramIns[7].Value = "LIBRE";
                paramIns[8] = new EacParameter("ajout", DbType.AnsiStringFixedLength);
                paramIns[8].Value = "O";
                paramIns[9] = new EacParameter("name", DbType.AnsiStringFixedLength);
                paramIns[9].Value = nameClause;
                paramIns[10] = new EacParameter("fileName", DbType.AnsiStringFixedLength);
                paramIns[10].Value = fileName;

                string sqlIns = @"INSERT INTO KPDOC
                                    (KEQID, KEQIPB, KEQALX, KEQTYP, KEQSERV, KEQACTG, KEQETAP, KEQTDOC, KEQAJT, KEQNOM, KEQCHM)
                                VALUES
                                    (:idDoc, :codeAffaire, :version, :type, :service, :acteGes, :etape, :typeDoc, :ajout, :name, :fileName)";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlIns, paramIns);
            }
            else
            {
                EacParameter[] paramUpd = new EacParameter[3];
                paramUpd[0] = new EacParameter("name", DbType.AnsiStringFixedLength);
                paramUpd[0].Value = nameClause;
                paramUpd[1] = new EacParameter("fileName", DbType.AnsiStringFixedLength);
                paramUpd[1].Value = fileName;
                paramUpd[2] = new EacParameter("idDoc", DbType.Int32);
                paramUpd[2].Value = idDoc;

                string sqlUpd = @"UPDATE KPDOC SET KEQNOM = :name, KEQCHM = :fileName WHERE KEQID = :idDoc";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpd, paramUpd);
            }
            #endregion

            #region Enregistrement de la clause dans KPCLAUSE
            if (idClause > 0)
            {
                EacParameter[] paramUpd = new EacParameter[7];
                paramUpd[0] = new EacParameter("idDoc", DbType.Int32);
                paramUpd[0].Value = idDoc;
                paramUpd[1] = new EacParameter("modif", DbType.AnsiStringFixedLength);
                paramUpd[1].Value = isModif ? "O" : "N";
                paramUpd[2] = new EacParameter("context", DbType.AnsiStringFixedLength);
                paramUpd[2].Value = contexte.ToUpper();
                paramUpd[3] = new EacParameter("emplacement", DbType.AnsiStringFixedLength);
                paramUpd[3].Value = emplacement.ToUpper();
                paramUpd[4] = new EacParameter("sousemplacement", DbType.AnsiStringFixedLength);
                paramUpd[4].Value = sousemplacement.ToUpper();
                paramUpd[5] = new EacParameter("ordre", DbType.Double);
                paramUpd[5].Value = Convert.ToDouble(ordre);
                paramUpd[6] = new EacParameter("idClause", DbType.Int32);
                paramUpd[6].Value = idClause;

                string sqlUpd = @"UPDATE KPCLAUSE 
                                    SET KCATXL = :idDoc, KCAXTLM = :modif, KCACTX = :context, KCACHI = :emplacement, KCACHIS = :sousemplacement, KCACXI = :ordre ,KCASIT ='V'
                                    WHERE KCAID = :idClause";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpd, paramUpd);
            }
            #endregion
        }

        public static string GetClauseFileName(string clauseId)
        {
            var sql =
                @"SELECT TRIM(KDUNM1) CONCAT '_' CONCAT TRIM(KDUNM2) CONCAT '_' CONCAT RIGHT(REPEAT('0', 3) || TRIM(KDUNM3), 3) CONCAT '_' CONCAT RIGHT(REPEAT('0', 3) || TRIM(KDUVER), 3) CONCAT '.docx' STRRETURNCOL
                            FROM KCLAUSE
                            WHERE KDUID = " + clauseId;
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result == null || !result.Any()) return string.Empty;
            var firstOrDefault = result.FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.StrReturnCol : string.Empty;
        }

        #region Liste Clauses
        public static List<ClauseDto> ObtenirBaseInfosClauses(string type, string numeroOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig)
        {
            long sequenceClause = !string.IsNullOrEmpty(sequence) ? Convert.ToInt64(sequence) : 0;
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);
           
            string sql = string.Format(@"SELECT 
CASE TBLCLAUSE.KCAXTL WHEN 'O' THEN 1 ELSE 0 END AS ISCLAUSELIBRE,
TBLCLAUSE.KCAID ID,
TBLCLAUSE.KCARSQ CODERISQUE,KABDESC DESCRISQUE,
TBLCLAUSE.KCAOBJ CODEOBJET,KACDESC DESCOBJET,
TBLCLAUSE.KCAFOR CODEFORMULE,KDADESC DESCFORMULE,KDAALPHA LETTREFORMULE,
TBLCLAUSE.KCACLNM1 RUBRIQUE,
TBLCLAUSE.KCACLNM2 SOUSRUBRIQUE,
TBLCLAUSE.KCACLNM3 SEQUEN,
IFNULL(CLASPE.KCLSLIB, IFNULL(KDULIB, KEQNOM)) LEDES,
TBLCLAUSE.KCAVER NUMEROVERSION,
TBLCLAUSE.KCAAJT ORIGINE,
TBLCLAUSE.KCAETAFF ETAPE,
TBLCLAUSE.KCACTX CONTEXTE, 
TPLIB CTXLBL,
TBLCLAUSE.KCASIT CODESITUATION, 
TBLCLAUSE.KCASITD DATESITUATION, 
TBLCLAUSE.KCAAVNC NUMEROAVENANTCREATION, 
TBLCLAUSE.KCACRD DATEAVENANTCREATION, 
TBLCLAUSE.KCAAVNM NUMEROAVENANTMODIFICATION, 
TBLCLAUSE.KCAMAJD DATEAVENANTMODIFICATION, 
TBLCLAUSE.KCADOC DOCUMENT, 
TBLCLAUSE.KCACHI CHAPITRE, 
TBLCLAUSE.KCACHIS SOUSCHAPITRE, 
TBLCLAUSE.KCATAE ACTIONENCHAINE, 
TBLCLAUSE.KCANTA ETATTITRE, 	                               
'' TEXTECLAUSELIBRE,
TBLCLAUSE.KCAGAR CODEGARANTIE,GADEA LIBELLEGARANTIE,
TBLCLAUSE.KCAKDUID CLAUSEID, TBLCLAUSE.KCATYPD DOCAJOUTE, IFNULL(KERLIB, IFNULL(CLASPE.KCLSLIB, IFNULL(KDULIB, KEQNOM))) DOCLIB,
TBLCLAUSE.KCAXTLM ISMODIFSTR,
CASE WHEN KEQCHM IS NOT NULL THEN KEQCHM
        WHEN KERCHM IS NOT NULL THEN KERCHM
        ELSE ''
        END CHEMINFICHIER,
CASE WHEN TBLCLAUSE.KCANTA = 'O' THEN CAST(IFNULL(HISTOBASE.PBMJA,0) * 10000 + IFNULL(HISTOBASE.PBMJM,0) * 100 + IFNULL(HISTOBASE.PBMJJ,0) AS NUMERIC(8,0))
        WHEN TBLCLAUSE.KCANTA = 'P' AND TBLCLAUSE.KCASIT = 'V' AND TBLCLAUSE.KCATYPD != 'E' THEN CAST(IFNULL(HISTOBASE.PBMJA,0) * 10000 + IFNULL(HISTOBASE.PBMJM,0) * 100 + IFNULL(HISTOBASE.PBMJJ,0) AS NUMERIC(8,0))
        WHEN TBLCLAUSE.KCANTA = 'P' AND TBLCLAUSE.KCASIT = 'V' AND TBLCLAUSE.KCATYPD = 'E' THEN TBLCLAUSE.KCASITD
        WHEN TBLCLAUSE.KCANTA = 'S' AND TBLCLAUSE.KCASIT = 'V' THEN TBLCLAUSE.KCASITD
        WHEN TBLCLAUSE.KCAAJT = 'O' AND TBLCLAUSE.KCASIT = 'V' THEN TBLCLAUSE.KCASITD
        ELSE 0
        END DATEDETAILAVNCREATION,
HISTOCLAUSE.KCASIT CODESITUATIONHISTO,
HISTOCLAUSE.KCAAVNM NUMAVENANTHISTO
FROM {0} TBLCLAUSE
{1}
LEFT JOIN {2} ON TBLCLAUSE.KCACLNM1 = KCLAUSE.KDUNM1 AND  TBLCLAUSE.KCACLNM2 = KCLAUSE.KDUNM2 AND TBLCLAUSE.KCACLNM3 = KCLAUSE.KDUNM3 AND TBLCLAUSE.KCAKDUID=KCLAUSE.KDUID 
LEFT JOIN {3} ON TBLCLAUSE.KCATXL = KEQID AND TBLCLAUSE.KCATYPD <> 'E' 
LEFT JOIN {4} ON KABRSQ=TBLCLAUSE.KCARSQ AND  KABIPB=TBLCLAUSE.KCAIPB AND KABALX=TBLCLAUSE.KCAALX  AND KABTYP=TBLCLAUSE.KCATYP {13} 
LEFT JOIN {5} ON KDAFOR=TBLCLAUSE.KCAFOR AND KDAIPB=TBLCLAUSE.KCAIPB AND KDAALX=TBLCLAUSE.KCAALX AND KDATYP=TBLCLAUSE.KCATYP {14}
LEFT JOIN {6} ON KACOBJ=TBLCLAUSE.KCAOBJ AND KACIPB=TBLCLAUSE.KCAIPB AND KACALX=TBLCLAUSE.KCAALX AND KACTYP=TBLCLAUSE.KCATYP AND KACRSQ = KABRSQ {15}
LEFT JOIN {7} ON GAGAR =TBLCLAUSE.KCAGAR
LEFT JOIN {8} ON TBLCLAUSE.KCATXL = KERID AND TBLCLAUSE.KCATYPD = 'E' 
LEFT JOIN YHPBASE HISTOBASE ON HISTOBASE.PBIPB = TBLCLAUSE.KCAIPB AND HISTOBASE.PBALX = TBLCLAUSE.KCAALX AND HISTOBASE.PBTYP = TBLCLAUSE.KCATYP AND HISTOBASE.PBAVN = TBLCLAUSE.KCAAVNC                              
LEFT JOIN HPCLAUSE HISTOCLAUSE ON HISTOCLAUSE.KCAIPB = TBLCLAUSE.KCAIPB AND HISTOCLAUSE.KCATYP = TBLCLAUSE.KCATYP 
AND HISTOCLAUSE.KCAALX = TBLCLAUSE.KCAALX AND HISTOCLAUSE.KCARSQ = TBLCLAUSE.KCARSQ AND HISTOCLAUSE.KCAOBJ = TBLCLAUSE.KCAOBJ 
AND HISTOCLAUSE.KCAFOR = TBLCLAUSE.KCAFOR AND HISTOCLAUSE.KCAOPT = TBLCLAUSE.KCAOPT AND HISTOCLAUSE.KCAGAR = TBLCLAUSE.KCAGAR 
AND HISTOCLAUSE.KCAETAFF = TBLCLAUSE.KCAETAFF AND HISTOCLAUSE.KCACTX = TBLCLAUSE.KCACTX 
AND HISTOCLAUSE.KCAID = TBLCLAUSE.KCAID
AND HISTOCLAUSE.KCAAVN = {16} 
AND HISTOCLAUSE.KCAVER = TBLCLAUSE.KCAVER
INNER JOIN YPOBASE BASE ON BASE.PBIPB = TBLCLAUSE.KCAIPB AND BASE.PBALX = TBLCLAUSE.KCAALX AND BASE.PBTYP = TBLCLAUSE.KCATYP
INNER JOIN KPENT ENTETE ON BASE.PBIPB = ENTETE.KAAIPB AND BASE.PBALX = ENTETE.KAAALX AND BASE.PBTYP = ENTETE.KAATYP
LEFT JOIN KCLALIBSPE CLASPE ON CLASPE.KCLSBRA = BASE.PBBRA AND CLASPE.KCLSCIB = ENTETE.KAACIBLE AND CLASPE.KCLSNM1 = TBLCLAUSE.KCACLNM1 AND CLASPE.KCLSNM2 = TBLCLAUSE.KCACLNM2 AND CLASPE.KCLSNM3 = TBLCLAUSE.KCACLNM3
WHERE TBLCLAUSE.KCATYP = '{9}' AND TBLCLAUSE.KCAIPB = '{10}' AND TBLCLAUSE.KCAALX = '{11}' {12}",
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPCLAUSE"),
                     CommonRepository.BuildJoinYYYYPAR("INNER", "KHEOP", "CTX", otherCriteria: " AND TCOD = TBLCLAUSE.KCACTX"),
                     "KCLAUSE",
                     "KPDOC",
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                     CommonRepository.GetPrefixeHisto(modeNavig, "KGARAN"),
                     "KPDOCEXT",
                     type, numeroOffre.PadLeft(9, ' '), version,
                     modeNavig == ModeConsultation.Historique ? string.Format(" AND TBLCLAUSE.KCAAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? iCodeAvn : 0) : string.Empty,
                     modeNavig == ModeConsultation.Historique ? " AND KABAVN = TBLCLAUSE.KCAAVN" : string.Empty,
                     modeNavig == ModeConsultation.Historique ? " AND KDAAVN = TBLCLAUSE.KCAAVN" : string.Empty,
                     modeNavig == ModeConsultation.Historique ? " AND KACAVN = TBLCLAUSE.KCAAVN" : string.Empty,
                     iCodeAvn - 1);

            if (!string.IsNullOrEmpty(codeRisque))
                sql += string.Format(@" AND TBLCLAUSE.KCARSQ='{0}'", Convert.ToInt32(codeRisque));
            if (!string.IsNullOrEmpty(codeFormule))
                sql += string.Format(@" AND TBLCLAUSE.KCAFOR='{0}'", Convert.ToInt32(codeFormule));
            if (!string.IsNullOrEmpty(codeOption))
                sql += string.Format(@" AND TBLCLAUSE.KCAOPT='{0}'", Convert.ToInt32(codeOption));
            if (contexte != "Tous")
                sql += string.Format(@" AND TBLCLAUSE.KCACTX='{0}'", contexte);
            if (!string.IsNullOrEmpty(rubrique))
                sql += string.Format(@" AND TBLCLAUSE.KCACLNM1='{0}'", rubrique);
            if (!string.IsNullOrEmpty(sousrubrique))
                sql += string.Format(@" AND TBLCLAUSE.KCACLNM2='{0}'", sousrubrique);
            if (!string.IsNullOrEmpty(sequence))
                sql += string.Format(@" AND TBLCLAUSE.KCACLNM3='{0}'", sequenceClause);
            if (!string.IsNullOrEmpty(idClause))
                sql += string.Format(@" AND TBLCLAUSE.KCAID='{0}'", idClause);

            if (!string.IsNullOrEmpty(filtre) && filtre != "T")
            {
                if (filtre == AlbConstantesMetiers.ToutesSaufObligatoires)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA!='{0}'", "O");
                if (filtre == AlbConstantesMetiers.Obligatoires)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA='{0}'", "O");
                if (filtre == AlbConstantesMetiers.Proposes)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA='{0}'", "P");
                if (filtre == AlbConstantesMetiers.Suggerees)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA='{0}'", "S");
                if (filtre == AlbConstantesMetiers.Ajoutees)
                    sql += string.Format(@" AND TBLCLAUSE.KCAAJT='{0}'", "O");
            }

            if (!string.IsNullOrEmpty(etape))
            {
                AlbConstantesMetiers.Etapes enumEtape = AlbEnumInfoValue.GetEnumValue<AlbConstantesMetiers.Etapes>(etape);
                switch (enumEtape)
                {
                    case AlbConstantesMetiers.Etapes.Garantie:
                        sql += string.Format(@" AND (TBLCLAUSE.KCAETAFF ='{0}' OR TBLCLAUSE.KCAETAFF ='{1}')", AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option), AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie));
                        break;
                    case AlbConstantesMetiers.Etapes.Risque:
                        sql += string.Format(@" AND (TBLCLAUSE.KCAETAFF = '{0}' OR TBLCLAUSE.KCAETAFF = '{1}')", AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque), AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet));
                        break;
                    default:
                        sql += string.Format(@" AND TBLCLAUSE.KCAETAFF ='{0}'", etape);
                        break;
                }
            }
            sql += " ORDER BY TBLCLAUSE.KCAETAFF, TBLCLAUSE.KCARSQ, TBLCLAUSE.KCAOBJ, TBLCLAUSE.KCAFOR";
            var toReturn = DbBase.Settings.ExecuteList<ClauseDto>(CommandType.Text, sql);
            if (toReturn.Any())
            {
                foreach (var clause in toReturn) {
                    MapToClause(iCodeAvn, modeNavig, clause: clause);
                }

                var firstClause = toReturn.FirstOrDefault();
                if (toReturn.Count == 1 && firstClause?.Id > 0)
                {
                    string sqlHisto = string.Format(@"
SELECT IFNULL((HISTOCLAUSE.KCAAVNC),-1) INT64RETURNCOL, IFNULL((HISTOCLAUSE.KCACRD),0) DATEDEBRETURNCOL, HISTOCLAUSE.KCASIT SITUATION 
FROM {0} TBLCLAUSE 
LEFT JOIN HPCLAUSE HISTOCLAUSE 
ON HISTOCLAUSE.KCAIPB = TBLCLAUSE.KCAIPB AND HISTOCLAUSE.KCAVER = TBLCLAUSE.KCAVER 
AND HISTOCLAUSE.KCATYP = TBLCLAUSE.KCATYP AND HISTOCLAUSE.KCAALX = TBLCLAUSE.KCAALX 
AND HISTOCLAUSE.KCARSQ = TBLCLAUSE.KCARSQ AND HISTOCLAUSE.KCAOBJ = TBLCLAUSE.KCAOBJ 
AND HISTOCLAUSE.KCAFOR = TBLCLAUSE.KCAFOR AND HISTOCLAUSE.KCAOPT = TBLCLAUSE.KCAOPT 
AND HISTOCLAUSE.KCAGAR = TBLCLAUSE.KCAGAR AND HISTOCLAUSE.KCAETAFF = TBLCLAUSE.KCAETAFF 
AND HISTOCLAUSE.KCACTX = TBLCLAUSE.KCACTX AND HISTOCLAUSE.KCACLNM1 = TBLCLAUSE.KCACLNM1 
AND HISTOCLAUSE.KCACLNM2 = TBLCLAUSE.KCACLNM2 AND HISTOCLAUSE.KCACLNM3 = TBLCLAUSE.KCACLNM3 
WHERE TBLCLAUSE.KCAID = {1} order by HISTOCLAUSE.KCAAVNC desc fetch first 1 rows only", CommonRepository.GetPrefixeHisto(modeNavig, "KPCLAUSE"), firstClause.Id);

                    var detailHisto = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlHisto);
                    if (detailHisto?.Any() ?? false)
                    {
                        // Si un résultat a été trouvé et qu'il n'est pas égale à -1
                        var detail = detailHisto.FirstOrDefault();
                        if (detail.Int64ReturnCol > -1 && detail.Situation == "V")
                        {
                            firstClause.NumAvnDetailClauseAjout = detail.Int64ReturnCol.ToString();
                            int.TryParse(detail.DateDebReturnCol.ToString(), out int iDateCreationDetailAvn);
                            firstClause.DateDetailAvnCreation = AlbConvert.ConvertIntToDate(iDateCreationDetailAvn);
                        }

                    }

                    if (string.IsNullOrEmpty(toReturn.FirstOrDefault().NumAvnDetailClauseAjout) && toReturn.FirstOrDefault().CodeSituation == "V")
                    {
                        firstClause.NumAvnDetailClauseAjout = codeAvn;
                    }
                    toReturn[0] = firstClause;
                }
            }
            return toReturn;
        }

        private static void MapToClause(int iCodeAvn, ModeConsultation modeNavig, ClauseDto clause) {
            clause.DateSituation = AlbConvert.ConvertIntToDate(clause.ColumnDateSituation);
            clause.DateAvenantCreation = AlbConvert.ConvertIntToDate(clause.ColumnDateAvenantCreation);
            clause.DateAvenantModification = AlbConvert.ConvertIntToDate(clause.ColumnDateAvenantModification);
            int iDateCreationDetailAvn = 0;
            int.TryParse(clause.ColumnDateAvenantCreation.ToString(), out iDateCreationDetailAvn);
            clause.DateDetailAvnCreation = AlbConvert.ConvertIntToDate(iDateCreationDetailAvn);
            clause.isModeHisto = modeNavig == ModeConsultation.Historique;
            clause.IsCheck = clause.CodeSituation == "V";
            clause.IsCheckHistorique = clause.CodeSituationHisto == "V";
            clause.IsClauseLibre = clause.ClauseLibre == 1;
            clause.Edition = clause.Document.Trim();
            clause.Edition = clause.Chapitre.Trim() + "/" + clause.SousChapitre.Trim();
            clause.EmplacementComplet = clause.Edition;
            clause.Origine = clause.Origine == "O" ? "Utilisateur" : "Système";
            if (clause.DocAjoute == "E") {
                clause.Origine = "PJ";
                clause.Titre = clause.DocLib;
            }
            if (clause.IsClauseLibre)
                clause.IsModif = clause.IsModifStr == "O";
            clause.IsModifAvenant = iCodeAvn > 0 && clause.NumeroAvenantModification == iCodeAvn;
        }

        /// <summary>
        /// Détails clauses : Ecran Détails du clause
        /// </summary>
        /// <param name="type"></param>
        /// <param name="numeroOffre"></param>
        /// <param name="version"></param>
        /// <param name="codeAvn"></param>
        /// <param name="etape"></param>
        /// <param name="contexte"></param>
        /// <param name="rubrique"></param>
        /// <param name="sousrubrique"></param>
        /// <param name="sequence"></param>
        /// <param name="idClause"></param>
        /// <param name="codeRisque"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeOption"></param>
        /// <param name="filtre"></param>
        /// <param name="modeNavig"></param>
        /// <returns></returns>
        public static ClauseDto DetailsClauses(string type, string numeroOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig)
        {
            long sequenceClause = !string.IsNullOrEmpty(sequence) ? Convert.ToInt64(sequence) : 0;
            int iCodeAvn = 0;
            int.TryParse(codeAvn, out iCodeAvn);

            string sql = string.Format(@"
SELECT 
    CASE TBLCLAUSE.KCAXTL WHEN 'O' THEN 1 ELSE 0 END AS ISCLAUSELIBRE,	
    TBLCLAUSE.KCAID ID,
    TBLCLAUSE.KCACLNM1 RUBRIQUE,
    TBLCLAUSE.KCACLNM2 SOUSRUBRIQUE,
    TBLCLAUSE.KCACLNM3 SEQUEN,
    IFNULL(KDULIB, KEQNOM) LEDES, 
	TBLCLAUSE.KCAVER NUMEROVERSION,
	KDUDATD COLUMNDATEEFFETVERSIONDEBUT,
	KDUDATF COLUMNDATEEFFETVERSIONFIN,
	TBLCLAUSE.KCAAJT ORIGINE,
	TBLCLAUSE.KCAETAFF ETAPE,
	TBLCLAUSE.KCACTX CONTEXTE, 
	TBLCLAUSE.KCASIT CODESITUATION, 
	TBLCLAUSE.KCASITD DATESITUATION, 
	TBLCLAUSE.KCAAVNC NUMEROAVENANTCREATION, 
	TBLCLAUSE.KCACRD DATEAVENANTCREATION, 
	TBLCLAUSE.KCAAVNM NUMEROAVENANTMODIFICATION, 
	TBLCLAUSE.KCAMAJD DATEAVENANTMODIFICATION, 
    '' TEXTECLAUSELIBRE,
	TBLCLAUSE.KCADOC DOCUMENT, 
	TBLCLAUSE.KCACHI CHAPITRE, 
	TBLCLAUSE.KCACHIS SOUSCHAPITRE, 
	TBLCLAUSE.KCAIMP NUMEROIMPRESSION,
	TBLCLAUSE.KCACXI NUMEROORDRE, 
	TBLCLAUSE.KCAIAN IMPRESSIONANNEXE, 
    TBLCLAUSE.KCATAE ACTIONENCHAINE, 
	TBLCLAUSE.KCATYPD DOCAJOUTE,
	IFNULL(KERLIB, IFNULL(KDULIB, KEQNOM)) DOCLIB
FROM {0} TBLCLAUSE
{1}
LEFT JOIN {2} ON TBLCLAUSE.KCACLNM1 = KCLAUSE.KDUNM1 AND  TBLCLAUSE.KCACLNM2 = KCLAUSE.KDUNM2 AND TBLCLAUSE.KCACLNM3 = KCLAUSE.KDUNM3 AND TBLCLAUSE.KCAKDUID=KCLAUSE.KDUID 
LEFT JOIN {3} ON TBLCLAUSE.KCATXL = KEQID AND TBLCLAUSE.KCATYPD <> 'E' 
LEFT JOIN {4} ON KABRSQ=TBLCLAUSE.KCARSQ AND  KABIPB=TBLCLAUSE.KCAIPB AND KABALX=TBLCLAUSE.KCAALX  AND KABTYP=TBLCLAUSE.KCATYP {13} 
LEFT JOIN {5} ON KDAFOR=TBLCLAUSE.KCAFOR AND KDAIPB=TBLCLAUSE.KCAIPB AND KDAALX=TBLCLAUSE.KCAALX AND KDATYP=TBLCLAUSE.KCATYP {14}
LEFT JOIN {6} ON KACOBJ=TBLCLAUSE.KCAOBJ AND KACIPB=TBLCLAUSE.KCAIPB AND KACALX=TBLCLAUSE.KCAALX AND KACTYP=TBLCLAUSE.KCATYP AND KACRSQ = KABRSQ {15}
LEFT JOIN {7} ON GAGAR =TBLCLAUSE.KCAGAR
LEFT JOIN {8} ON TBLCLAUSE.KCATXL = KERID AND TBLCLAUSE.KCATYPD = 'E' 
LEFT JOIN YHPBASE HISTOBASE ON PBIPB = TBLCLAUSE.KCAIPB AND PBALX = TBLCLAUSE.KCAALX AND PBTYP = TBLCLAUSE.KCATYP AND PBAVN = TBLCLAUSE.KCAAVNC  
LEFT JOIN HPCLAUSE HISTOCLAUSE ON HISTOCLAUSE.KCAIPB = TBLCLAUSE.KCAIPB AND HISTOCLAUSE.KCATYP = TBLCLAUSE.KCATYP 
AND HISTOCLAUSE.KCAALX = TBLCLAUSE.KCAALX AND HISTOCLAUSE.KCARSQ = TBLCLAUSE.KCARSQ 
AND HISTOCLAUSE.KCAOBJ = TBLCLAUSE.KCAOBJ AND HISTOCLAUSE.KCAFOR = TBLCLAUSE.KCAFOR 
AND HISTOCLAUSE.KCAOPT = TBLCLAUSE.KCAOPT AND HISTOCLAUSE.KCAGAR = TBLCLAUSE.KCAGAR 
AND HISTOCLAUSE.KCAETAFF = TBLCLAUSE.KCAETAFF AND HISTOCLAUSE.KCACTX = TBLCLAUSE.KCACTX 
AND HISTOCLAUSE.KCACLNM1 = TBLCLAUSE.KCACLNM1 AND HISTOCLAUSE.KCACLNM2 = TBLCLAUSE.KCACLNM2 
AND HISTOCLAUSE.KCACLNM3 = TBLCLAUSE.KCACLNM3 AND HISTOCLAUSE.KCAAVN = {16} 
AND HISTOCLAUSE.KCAVER = TBLCLAUSE.KCAVER 
WHERE TBLCLAUSE.KCATYP='{9}' AND TBLCLAUSE.KCAIPB='{10}' AND TBLCLAUSE.KCAALX='{11}' {12}",
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPCLAUSE"),
                     CommonRepository.BuildJoinYYYYPAR("INNER", "KHEOP", "CTX", otherCriteria: " AND TCOD = TBLCLAUSE.KCACTX"),
                     "KCLAUSE",
                     "KPDOC",
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                     CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                     CommonRepository.GetPrefixeHisto(modeNavig, "KGARAN"),
                     "KPDOCEXT",
                     type, numeroOffre.PadLeft(9, ' '), version,
                     modeNavig == ModeConsultation.Historique ? string.Format(" AND TBLCLAUSE.KCAAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? iCodeAvn : 0) : string.Empty,
                     modeNavig == ModeConsultation.Historique ? " AND KABAVN = TBLCLAUSE.KCAAVN" : string.Empty,
                     modeNavig == ModeConsultation.Historique ? " AND KDAAVN = TBLCLAUSE.KCAAVN" : string.Empty,
                     modeNavig == ModeConsultation.Historique ? " AND KACAVN = TBLCLAUSE.KCAAVN" : string.Empty,
                     iCodeAvn - 1);

            if (!string.IsNullOrEmpty(codeRisque))
                sql += string.Format(@" AND TBLCLAUSE.KCARSQ='{0}'", Convert.ToInt32(codeRisque));
            if (!string.IsNullOrEmpty(codeFormule))
                sql += string.Format(@" AND TBLCLAUSE.KCAFOR='{0}'", Convert.ToInt32(codeFormule));
            if (!string.IsNullOrEmpty(codeOption))
                sql += string.Format(@" AND TBLCLAUSE.KCAOPT='{0}'", Convert.ToInt32(codeOption));
            if (contexte != "Tous")
                sql += string.Format(@" AND TBLCLAUSE.KCACTX='{0}'", contexte);
            if (!string.IsNullOrEmpty(rubrique))
                sql += string.Format(@" AND TBLCLAUSE.KCACLNM1='{0}'", rubrique);
            if (!string.IsNullOrEmpty(sousrubrique))
                sql += string.Format(@" AND TBLCLAUSE.KCACLNM2='{0}'", sousrubrique);
            if (!string.IsNullOrEmpty(sequence))
                sql += string.Format(@" AND TBLCLAUSE.KCACLNM3='{0}'", sequenceClause);
            if (!string.IsNullOrEmpty(idClause))
                sql += string.Format(@" AND TBLCLAUSE.KCAID='{0}'", idClause);

            if (!string.IsNullOrEmpty(filtre) && filtre != "T")
            {
                if (filtre == AlbConstantesMetiers.ToutesSaufObligatoires)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA!='{0}'", "O");
                if (filtre == AlbConstantesMetiers.Obligatoires)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA='{0}'", "O");
                if (filtre == AlbConstantesMetiers.Proposes)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA='{0}'", "P");
                if (filtre == AlbConstantesMetiers.Suggerees)
                    sql += string.Format(@" AND TBLCLAUSE.KCANTA='{0}'", "S");
                if (filtre == AlbConstantesMetiers.Ajoutees)
                    sql += string.Format(@" AND TBLCLAUSE.KCAAJT='{0}'", "O");
            }

            if (!string.IsNullOrEmpty(etape))
            {
                AlbConstantesMetiers.Etapes enumEtape = AlbEnumInfoValue.GetEnumValue<AlbConstantesMetiers.Etapes>(etape);
                switch (enumEtape)
                {
                    case AlbConstantesMetiers.Etapes.Garantie:
                        sql += string.Format(@" AND (TBLCLAUSE.KCAETAFF ='{0}' OR TBLCLAUSE.KCAETAFF ='{1}')", AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option), AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie));
                        break;
                    case AlbConstantesMetiers.Etapes.Risque:
                        sql += string.Format(@" AND (TBLCLAUSE.KCAETAFF = '{0}' OR TBLCLAUSE.KCAETAFF = '{1}')", AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque), AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet));
                        break;
                    default:
                        sql += string.Format(@" AND TBLCLAUSE.KCAETAFF ='{0}'", etape);
                        break;
                }
            }

            var toReturn = DbBase.Settings.ExecuteList<ClauseDto>(CommandType.Text, sql).FirstOrDefault();
            if (toReturn != null)
            {
                toReturn.DateEffetVersionDebut = AlbConvert.ConvertIntToDate(toReturn.ColumnDateEffetVersionDebut);
                toReturn.DateEffetVersionFin = AlbConvert.ConvertIntToDate(toReturn.ColumnDateEffetVersionFin);
                toReturn.DateSituation = AlbConvert.ConvertIntToDate(toReturn.ColumnDateSituation);
                toReturn.DateAvenantCreation = AlbConvert.ConvertIntToDate(toReturn.ColumnDateAvenantCreation);
                toReturn.DateAvenantModification = AlbConvert.ConvertIntToDate(toReturn.ColumnDateAvenantModification);

                int iDateCreationDetailAvn = 0;
                int.TryParse(toReturn.ColumnDateAvenantCreation.ToString(), out iDateCreationDetailAvn);
                toReturn.DateDetailAvnCreation = AlbConvert.ConvertIntToDate(iDateCreationDetailAvn);

                toReturn.IsClauseLibre = toReturn.ClauseLibre == 1;

                toReturn.Origine = toReturn.Origine == "O" ? "Utilisateur" : "Système";
                if (toReturn.DocAjoute == "E")
                {
                    toReturn.Origine = "PJ";
                    toReturn.Titre = toReturn.DocLib;
                }

                if (toReturn.Id > 0)
                {
                    string sqlHisto = string.Format(@"
SELECT IFNULL((HISTOCLAUSE.KCAAVNC),-1) INT64RETURNCOL, IFNULL((HISTOCLAUSE.KCACRD),0) DATEDEBRETURNCOL, HISTOCLAUSE.KCASIT SITUATION 
FROM {0} TBLCLAUSE 
LEFT JOIN HPCLAUSE HISTOCLAUSE 
ON HISTOCLAUSE.KCAIPB = TBLCLAUSE.KCAIPB AND HISTOCLAUSE.KCATYP = TBLCLAUSE.KCATYP 
AND HISTOCLAUSE.KCAALX = TBLCLAUSE.KCAALX AND HISTOCLAUSE.KCARSQ = TBLCLAUSE.KCARSQ 
AND HISTOCLAUSE.KCAOBJ = TBLCLAUSE.KCAOBJ AND HISTOCLAUSE.KCAFOR = TBLCLAUSE.KCAFOR 
AND HISTOCLAUSE.KCAOPT = TBLCLAUSE.KCAOPT AND HISTOCLAUSE.KCAGAR = TBLCLAUSE.KCAGAR 
AND HISTOCLAUSE.KCAETAFF = TBLCLAUSE.KCAETAFF AND HISTOCLAUSE.KCACTX = TBLCLAUSE.KCACTX 
AND HISTOCLAUSE.KCACLNM1 = TBLCLAUSE.KCACLNM1 AND HISTOCLAUSE.KCACLNM2 = TBLCLAUSE.KCACLNM2 
AND HISTOCLAUSE.KCACLNM3 = TBLCLAUSE.KCACLNM3 AND HISTOCLAUSE.KCAVER = TBLCLAUSE.KCAVER 
WHERE TBLCLAUSE.KCAID = {1} order by HISTOCLAUSE.KCAAVNC desc fetch first 1 rows only", CommonRepository.GetPrefixeHisto(modeNavig, "KPCLAUSE"), toReturn.Id);

                    var detailHisto = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlHisto);
                    if (detailHisto != null && detailHisto.Any())
                    {
                        //Si un résultat a été trouvé et qu'il n'est pas égale à -1
                        if (detailHisto.FirstOrDefault().Int64ReturnCol > -1 && detailHisto.FirstOrDefault().Situation == "V")
                        {
                            toReturn.NumAvnDetailClauseAjout = detailHisto.FirstOrDefault().Int64ReturnCol.ToString();
                            int iDateCreatDetailAvn = 0;
                            int.TryParse(detailHisto.FirstOrDefault().DateDebReturnCol.ToString(), out iDateCreatDetailAvn);
                            toReturn.DateDetailAvnCreation = AlbConvert.ConvertIntToDate(iDateCreatDetailAvn);
                        }
                    }

                    if (string.IsNullOrEmpty(toReturn.NumAvnDetailClauseAjout) && toReturn.CodeSituation == "V")
                    {
                        toReturn.NumAvnDetailClauseAjout = codeAvn;

                    }

                }
            }
            return toReturn;
        }
        #endregion
    }



}
