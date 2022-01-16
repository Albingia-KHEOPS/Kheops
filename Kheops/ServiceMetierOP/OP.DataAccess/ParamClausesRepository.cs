using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.ParametreClauses;
using System.Data.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Data;
using System.Data;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.DataAccess
{
    public class ParamClausesRepository
    {
        #region Méthodes Publiques

        public static List<ParamListParamDto> LoadListActeGestion(string codeService)
        {
            DbParameter[] param = new DbParameter[1];
            string sql = string.Format(@"SELECT KEGID CODE, KEGACTG PARAM, TPLIL LIBELLE 
                        FROM KALACTG
                            {0}
                        WHERE KEGSERV = :KEGSERV
                        ORDER BY KEGACTG",
                    CommonRepository.BuildJoinYYYYPAR("INNER", "KHEOP", "ACTGS", otherCriteria: " AND TCOD = KEGACTG"));

            param[0] = new EacParameter("KEGSERV", codeService);

            return DbBase.Settings.ExecuteList<ParamListParamDto>(CommandType.Text, sql, param);
        }

        public static ParamActeGestionEtapeDto LoadListEtapes(string codeService, string codeActGes)
        {
            ParamActeGestionEtapeDto model = new ParamActeGestionEtapeDto();

            ParametreDto service = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "GENER", "SERVI", codeService);
            model.CodeService = service.Code;
            model.Service = service.Libelle;

            ParametreDto actGes = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ACTGS", codeActGes);
            model.CodeActeGestion = actGes.Code;
            model.ActeGestion = actGes.Libelle;

            List<ParamListParamDto> etapes = new List<ParamListParamDto>();
            DbParameter[] param = new DbParameter[2];
            string sql = string.Format(@"SELECT KEHID CODE, KEHETAPE PARAM, TPLIL LIBELLE, KEHETORD NUMORDRE 
                        FROM KALETAP
                            {0}
                        WHERE KEHSERV = :KEHSERV AND KEHACTG = :KEHACTG
                        ORDER BY KEHETORD",
                CommonRepository.BuildJoinYYYYPAR("INNER", "KHEOP", "ETAPE", otherCriteria: " AND TCOD = KEHETAPE"));

            param[0] = new EacParameter("KEHSERV", codeService);
            param[1] = new EacParameter("KEHACTG", codeActGes);

            model.ListEtapes = DbBase.Settings.ExecuteList<ParamListParamDto>(CommandType.Text, sql, param);

            return model;
        }

        public static ParamEtapeContexteDto LoadListContextes(string codeService, string codeActGes, string codeEtape)
        {
            ParamEtapeContexteDto model = new ParamEtapeContexteDto();
            ParametreDto service = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "GENER", "SERVI", codeService);
            model.CodeService = service.Code;
            model.Service = service.Libelle;

            ParametreDto actGes = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ACTGS", codeActGes);
            model.CodeActeGestion = actGes.Code;
            model.ActeGestion = actGes.Libelle;

            ParametreDto etape = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ETAPE", codeEtape);
            model.CodeEtape = etape.Code;
            model.Etape = etape.Libelle;

            List<ParamListParamDto> contextes = new List<ParamListParamDto>();
            DbParameter[] param = new DbParameter[3];
            string sql = string.Format(@"SELECT KEIID CODE, KEICTX PARAM, CTX.TPLIL LIBELLE, KEIEMOD EMPLMODIF, KEIAJC AJOUTCLAUSIER, KEIAJT AJOUTLIBRE, 
                                KEIMDT1 CLAUSEMOD1, KEIMDT2 CLAUSEMOD2, KEIMDT3 CLAUSEMOD3, KDULIB LIBMODELCLAUSE, KFEOBS LIBSCRIPT,
                                KEICHI EMPLACEMENT, EMP.TPLIB LIBEMPLACEMENT, KEICHIS SOUSEMPLACEMENT, KEICXI NUMORDONNANCEMENT
                        FROM KALCONT
                            {0}
                            {1}
                            LEFT JOIN KHTSCRIPT ON KFEORIG = 'SCRIPT' AND KFEID1 = KEISCID2
                            LEFT JOIN KCLAUSE ON KEIMDT1 = KDUNM1 AND KEIMDT2 = KDUNM2 AND KEIMDT3 = KDUNM3
                        WHERE KEISERV = :KEISERV AND KEIACTG = :KEIACTG AND KEIETAPE = :KEIETAPE
                        ORDER BY KEICTX",
                        CommonRepository.BuildJoinYYYYPAR("INNER", "KHEOP", "CTX", "CTX", " AND CTX.TCOD = KEICTX"),
                        CommonRepository.BuildJoinYYYYPAR("LEFT", "KHEOP", "EMP", "EMP", " AND EMP.TCOD = KEICHI"));

            param[0] = new EacParameter("KEISERV", codeService);
            param[1] = new EacParameter("KEIACTG", codeActGes);
            param[2] = new EacParameter("KEIETAPE", codeEtape);

            model.ListContextes = DbBase.Settings.ExecuteList<ParamListParamDto>(CommandType.Text, sql, param);
            model.ListContextes.ForEach(c =>
            {
                c.AjoutClausier = c.AjtClausier == "O";
                c.AjoutLibre = c.AjtLibre == "O";
                c.EmplModif = c.EmplacementModif == "O";
                c.NumOrdonnancement = c.NumOrdo.ToString();
            });

            return model;
        }

        public static ParamContexteEGDIDto LoadListEGDI(string codeService, string codeActGes, string codeEtape, string codeContexte)
        {
            ParamContexteEGDIDto model = new ParamContexteEGDIDto();
            ParametreDto service = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "GENER", "SERVI", codeService);
            model.CodeService = service.Code;
            model.Service = service.Libelle;

            ParametreDto actGes = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ACTGS", codeActGes);
            model.CodeActeGestion = actGes.Code;
            model.ActeGestion = actGes.Libelle;

            ParametreDto etape = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ETAPE", codeEtape);
            model.CodeEtape = etape.Code;
            model.Etape = etape.Libelle;

            ParametreDto contexte = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "CTX", codeContexte);
            model.CodeContexte = contexte.Code;
            model.Contexte = contexte.Libelle;


            DbParameter[] param = new DbParameter[4];
            string sql = @"SELECT KEJID CODE, KEJTYPE TYPE, KEJELG PARAM, KEJLIB1 LIBELLE, KEJORD NUMORDRE
                        FROM KALCELG
                        WHERE KEJSERV = :KEJSERV AND KEJACTG = :KEJACTG AND KEJETAPE = :KEJETAPE AND KEJCTX = :KEJCTX
                        ORDER BY KEJORD";
            param[0] = new EacParameter("KEJSERV", codeService);
            param[1] = new EacParameter("KEJACTG", codeActGes);
            param[2] = new EacParameter("KEJETAPE", codeEtape);
            param[3] = new EacParameter("KEJCTX", codeContexte);

            model.ListEGDI = DbBase.Settings.ExecuteList<ParamListParamDto>(CommandType.Text, sql, param);

            return model;
        }

        public static ParamRattachClauseDto LoadRattachClause(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI)
        {
            var model = new ParamRattachClauseDto { Code = Convert.ToInt32(codeEGDI) };

            ParametreDto service = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "GENER", "SERVI", codeService);
            model.CodeService = service.Code;
            model.Service = service.Libelle;

            ParametreDto actGes = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ACTGS", codeActGes);
            model.CodeActeGestion = actGes.Code;
            model.ActeGestion = actGes.Libelle;

            ParametreDto etape = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ETAPE", codeEtape);
            model.CodeEtape = etape.Code;
            model.Etape = etape.Libelle;

            ParametreDto contexte = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "CTX", codeContexte);
            model.CodeContexte = contexte.Code;
            model.Contexte = contexte.Libelle;

            ParamAjoutEGDIDto modelRattach = LoadEGDI(codeService, codeActGes, codeEtape, codeContexte, codeEGDI);
            if (modelRattach != null)
            {
                model.EG = modelRattach.Type == "EG";
                model.DI = modelRattach.Type == "DI";
                model.CodeEGDI = modelRattach.CodeEGDI;
                model.LibelleEGDI = modelRattach.LibelleEGDI;
            }

            model.Clauses = GetListClauses(codeEGDI);

            return model;
        }

        public static ParamAjoutActeGestionDto LoadAjoutActeGestion(string codeService)
        {
            var model = new ParamAjoutActeGestionDto();
            ParametreDto param = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "GENER", "SERVI", codeService);

            model.CodeService = param.Code;
            model.LibelleService = param.Libelle;

            List<string> actesGestion = GetActeGestionByService(codeService);
            model.ActesGestion = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "ACTGS", tCod: actesGestion, notIn: true);

            return model;
        }

        public static void AddActeGestion(string codeService, string codeActeGestion)
        {
            int codeParam = CommonRepository.GetAS400Id("KEGID");
            if (codeParam > 0)
            {
                var param = new DbParameter[3];
                string sql = @"INSERT INTO KALACTG
                        (KEGID, KEGSERV, KEGACTG)
                        VALUES
                        (:KEGID, :KEGSERV, :KEGACTG)";
                param[0] = new EacParameter("KEGID", codeParam);
                param[1] = new EacParameter("KEGSERV", codeService);
                param[2] = new EacParameter("KEGACTG", codeActeGestion);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            else
            {
                throw new Exception("Erreur lors de l'insertion du paramètre.");
            }
        }

        public static ParamAjoutEtapeDto LoadAjoutEtape(string codeService, string codeActGes)
        {
            var model = new ParamAjoutEtapeDto();
            ParametreDto paramServ = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "GENER", "SERVI", codeService);
            model.CodeService = paramServ.Code;
            model.LibelleService = paramServ.Libelle;

            ParametreDto paramActGes = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ACTGS", codeActGes);
            model.CodeActGes = paramActGes.Code;
            model.LibelleActGes = paramActGes.Libelle;

            List<string> etapes = GetEtapeByActeGestion(codeService, codeActGes);
            model.Etapes = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "ETAPE", tCod: etapes, notIn: true);

            model.NumOrd = GetNumOrdreEtape(codeService, codeActGes);

            return model;
        }

        public static void AddEtape(string codeService, string codeActeGestion, string codeEtape, int numOrdre)
        {
            int codeParam = CommonRepository.GetAS400Id("KEHID");
            if (codeParam > 0)
            {
                DbParameter[] param = new DbParameter[6];
                string sql = @"INSERT INTO KALETAP
                        (KEHID, KEHSERV, KEHACTG, KEHKEGID, KEHETAPE, KEHETORD)
                        VALUES
                        (:KEHID, :KEHSERV, :KEHACTG, :KEHKEGID, :KEHETAPE, :KEHETORD)";
                param[0] = new EacParameter("KEHID", codeParam);
                param[1] = new EacParameter("KEHSERV", codeService);
                param[2] = new EacParameter("KEHACTG", codeActeGestion);
                param[3] = new EacParameter("KEHKEGID", GetIdActGesService(codeService, codeActeGestion));
                param[4] = new EacParameter("KEHETAPE", codeEtape);
                param[5] = new EacParameter("KEHETORD", numOrdre);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            else
            {
                throw new Exception("Erreur lors de l'insertion du paramètre.");
            }
        }

        public static ParamAjoutContexteDto LoadAjoutContexte(string codeService, string codeActGes, string codeEtape)
        {
            var model = new ParamAjoutContexteDto();

            model.AjoutClausier = true;
            model.AjoutLibre = true;

            ParametreDto paramServ = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "GENER", "SERVI", codeService);
            model.CodeService = paramServ.Code;
            model.LibelleService = paramServ.Libelle;

            ParametreDto paramActGes = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ACTGS", codeActGes);
            model.CodeActGes = paramActGes.Code;
            model.LibelleActGes = paramActGes.Libelle;

            ParametreDto paramEtape = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ETAPE", codeEtape);
            model.CodeEtape = paramEtape.Code;
            model.LibelleEtape = paramEtape.Libelle;

            List<string> contextes = GetContexteByEtape(codeService, codeActGes, codeEtape);
            model.Contextes = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "CTX", tCod: contextes, notIn: true);

            model.Specificites = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "CTXSP");

            model.ScriptsControle = GetScript("SCRIPT");

            model.ModelesClause1 = ClauseRepository.GetListRubrique();
            model.ModelesClause2 = new List<ParametreDto>();
            model.ModelesClause3 = new List<ParametreDto>();

            model.Emplacements = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "EMP");
            model.SousEmplacements = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "SSEMP");

            return model;
        }

        public static void AddContexte(string idContexte, string codeService, string codeActeGestion, string codeEtape, string codeContexte, bool emplacementModif, bool ajoutClausier, bool ajoutLibre, string scriptControle,
            string rubrique, string sousRubrique, string sequence, string emplacement, string sousEmplacement, string numOrdonnancement, string specificite)
        {
            if (string.IsNullOrEmpty(idContexte))
            {

                int codeParam = CommonRepository.GetAS400Id("KEIID");
                if (codeParam <= 0)
                {
                    throw new Exception("Erreur lors de l'insertion du paramètre.");
                }
                var param = new DbParameter[18];
                string sql = @"INSERT INTO KALCONT
                        (KEIID, KEISERV, KEIACTG, KEIETAPE, KEIKEHID, KEICTX, KEIEMOD, KEIAJC, KEIAJT, KEIMDT1, KEIMDT2, KEIMDT3, KEISCID2, KEICHI, KEICHIS, KEICXI, KEIIMP, KEIEXT )
                        VALUES
                        (:KEIID, :KEISERV, :KEIACTG, :KEIETAPE, :KEIKEHID, :KEICTX, :KEIEMOD, :KEIAJC, :KEIAJT, :KEIMDT1, :KEIMDT2, :KEIMDT3, :KEISCID2, :KEICHI, :KEICHIS, :KEICXI, :KEIIMP, :KEIEXT )";
                param[0] = new EacParameter("KEIID", codeParam);
                param[1] = new EacParameter("KEISERV", codeService);
                param[2] = new EacParameter("KEIACTG", codeActeGestion);
                param[3] = new EacParameter("KEIETAPE", codeEtape);
                param[4] = new EacParameter("KEIKEHID", GetIdEtapeActGes(codeService, codeActeGestion, codeEtape));
                param[5] = new EacParameter("KEICTX", codeContexte);
                param[6] = new EacParameter("KEIEMOD", emplacementModif ? "O" : "N");
                param[7] = new EacParameter("KEIAJC", ajoutClausier ? "O" : "N");
                param[8] = new EacParameter("KEIAJT", ajoutLibre ? "O" : "N");
                param[9] = new EacParameter("KEIMDT1", rubrique);
                param[10] = new EacParameter("KEIMDT2", sousRubrique);
                param[11] = new EacParameter("KEIMDT3", sequence);
                param[12] = new EacParameter("KEISCID2", scriptControle);
                param[13] = new EacParameter("KEICHI", emplacement);
                param[14] = new EacParameter("KEICHIS", sousEmplacement);
                param[15] = new EacParameter("KEICXI", 0);
                param[15].Value = !string.IsNullOrEmpty(numOrdonnancement) ? Convert.ToInt32(numOrdonnancement) : 0;
                param[16] = new EacParameter("KEIIMP", 0);
                param[16].Value = 0;
                param[17] = new EacParameter("KEIEXT", specificite);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            else
            {
                string sql = string.Format(@"UPDATE KALCONT
                                SET KEIEMOD = '{0}', KEIAJC = '{1}', KEIAJT = '{2}', KEIMDT1 = '{3}', KEIMDT2 = '{4}', KEIMDT3 = {5}, KEISCID2 = '{6}',
                                    KEICHI = '{7}', KEICHIS = '{8}', KEICXI = {9}, KEIEXT = '{10}'
                                WHERE KEIID = {11}",
                                                  emplacementModif ? "O" : "N", ajoutClausier ? "O" : "N", ajoutLibre ? "O" : "N", rubrique, sousRubrique,
                                                  !string.IsNullOrEmpty(sequence) ? sequence : "0", scriptControle,
                                                  emplacement, sousEmplacement, !string.IsNullOrEmpty(numOrdonnancement) ? numOrdonnancement : "0", specificite,
                                                  idContexte);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }
        }

        public static ParamAjoutEGDIDto LoadAjoutEGDI(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI)
        {
            var model = new ParamAjoutEGDIDto();
            ParametreDto paramServ = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "GENER", "SERVI", codeService);
            model.CodeService = paramServ.Code;
            model.LibelleService = paramServ.Libelle;

            ParametreDto paramActGes = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ACTGS", codeActGes);
            model.CodeActGes = paramActGes.Code;
            model.LibelleActGes = paramActGes.Libelle;

            var paramEtape = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ETAPE", codeEtape);
            model.CodeEtape = paramEtape.Code;
            model.LibelleEtape = paramEtape.Libelle;

            var paramContexte = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "CTX", codeContexte);
            model.CodeContexte = paramContexte.Code;
            model.LibelleContexte = paramContexte.Libelle;

            model.ModeSaisie = 1;
            model.EG = true;
            model.NumOrd = GetNumOrdreEGDI(codeService, codeActGes, codeEtape, codeContexte);

            if (!string.IsNullOrEmpty(codeEGDI))
            {
                var result = LoadEGDI(codeService, codeActGes, codeEtape, codeContexte, codeEGDI);
                if (result != null)
                {
                    model.Code = Convert.ToInt32(codeEGDI);
                    model.ModeSaisie = 2;
                    model.EG = result.Type == "EG" ? true : false;
                    model.DI = result.Type == "DI" ? true : false;
                    model.CodeEGDI = result.CodeEGDI;
                    model.NumOrd = result.NumOrd;
                    model.LibelleEGDI = result.LibelleEGDI;
                    model.Commentaires = result.Commentaires;
                }
            }

            return model;
        }

        public static void AddEGDI(string codeService, string codeActeGestion, string codeEtape, string codeContext, string type, string codeEGDI, int numOrdre, string libelleEGDI, string commentaire, string code)
        {
            if (string.IsNullOrEmpty(code) || code == "0")
            {
                InsertEGDI(codeService, codeActeGestion, codeEtape, codeContext, type, codeEGDI, numOrdre, libelleEGDI, commentaire);
            }
            else
            {
                UpdateEGDI(type, numOrdre, libelleEGDI, commentaire, code);
            }
        }

        public static void DeleteParam(string etape, string codeParam)
        {
            DbParameter[] param = new DbParameter[2];
            param[0] = new EacParameter("P_ETAPE", etape);
            param[1] = new EacParameter("P_CODEPARAM", codeParam);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELPARAMCLAUSE", param);

            //using (var con = DbBase.Settings.CreateConnection())
            //{
            //    var eacCon = (EacConnection)con;
            //    eacCon.Open();
            //    //var dbTransaction = eacCon.BeginTransaction();
            //    var dbTransaction = new EacTransaction((EacConnection)con);
            //    //throw new Exception("erreur");
            //    try
            //    {
            //        var param = new DbParameter[1];
            //        string sql = string.Empty;
            //        switch (etape)
            //        {
            //            case "ActeGestion":
            //                DeleteParamActeGestion(codeParam);
            //                //DeleteParamActeGestion(dbTransaction, codeParam);
            //                sql = @"DELETE FROM KALACTG WHERE KEGID = :codeParam";
            //                break;
            //            case "Etape":
            //                DeleteParamEtape(codeParam);
            //                sql = "DELETE FROM KALETAP WHERE KEHID = :codeParam";
            //                break;
            //            case "Contexte":
            //                DeleteParamContexte(codeParam);
            //                sql = "DELETE FROM KALCONT WHERE KEIID = :codeParam";
            //                break;
            //            case "EGDI":
            //                sql = "DELETE FROM KALCELG WHERE KEJID = :codeParam";
            //                break;
            //        }
            //        param[0] = new EacParameter("codeParam", codeParam);

            //        if (!string.IsNullOrEmpty(sql))
            //        {
            //            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            //        }
            //        dbTransaction.Commit();
            //    }
            //    catch (Exception)
            //    {
            //        dbTransaction.Rollback();
            //        throw new Exception("erreur transaction");
            //    }
            //}
        }

        public static ParamRattachClauseDto ReloadClauses(string codeEGDI, string restrict)
        {
            var model = new ParamRattachClauseDto
            {
                Clauses = GetListClausesRestrict(codeEGDI, restrict)
            };

            return model;
        }

        public static ParamRattachSaisieDto AttachClause(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI, string type, string codeClause)
        {
            var model = new ParamRattachSaisieDto();

            ParametreDto service = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "GENER", "SERVI", codeService);
            model.CodeAttachService = service.Code;
            model.AttachService = service.Libelle;

            ParametreDto actGes = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ACTGS", codeActGes);
            model.CodeAttachActeGestion = actGes.Code;
            model.AttachActeGestion = actGes.Libelle;

            ParametreDto etape = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ETAPE", codeEtape);
            model.CodeAttachEtape = etape.Code;
            model.AttachEtape = etape.Libelle;

            ParametreDto contexte = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "CTX", codeContexte);
            model.CodeAttachContexte = contexte.Code;
            model.AttachContexte = contexte.Libelle;

            ParamAjoutEGDIDto modelRattach = LoadEGDI(codeService, codeActGes, codeEtape, codeContexte, codeEGDI);
            if (modelRattach != null)
            {
                model.AttachEG = modelRattach.Type == "EG" ? true : false;
                model.AttachDI = modelRattach.Type == "DI" ? true : false;
                model.CodeAttachEGDI = modelRattach.CodeEGDI;
                //model.LibelleEGDI = modelRattach.LibelleEGDI;
            }

            ParamListClausesDto modelClause = GetClause(codeEGDI, codeClause);
            if (modelClause != null)
            {
                model.IdAttachEGDI = modelClause.Code;
                model.Version = modelClause.Version;
                model.ClauseNom1 = modelClause.Nom1;
                model.ClauseNom2 = modelClause.Nom2;
                model.ClauseNom3 = modelClause.Nom3;
                model.LibelleClause = modelClause.Libelle;
                model.AttachOrdre = modelClause.Ordre == 0 ? GetNumOrdreClause(codeEGDI, codeClause) : modelClause.Ordre;
                model.Obligatoire = modelClause.Nature == "O" ? true : false;
                model.Proposee = modelClause.Nature == "P" ? true : false;
                model.Suggeree = modelClause.Nature == "S" ? true : false;
                model.ImpressAnnexe = modelClause.ImpressAnnexe == "O" ? true : false;
                model.CodeAnnexe = modelClause.CodeAnnexe;
                model.StyleWord = CommonRepository.GetAttrFromStr(modelClause.AttributImpress, "style", "=\"", "\"");
                model.Taille = CommonRepository.GetAttrFromStr(modelClause.AttributImpress, "fontsize", "=\"", "\"");
                model.Gras = CommonRepository.GetAttrFromStr(modelClause.AttributImpress, "att", "=\"", "\"").Contains("gras");
                model.Souligne = CommonRepository.GetAttrFromStr(modelClause.AttributImpress, "att", "=\"", "\"").Contains("souligne");
            }

            return model;
        }

        public static int SaveAttachClause(int codeRattach, string codeClause, string codeEGDI, int numOrdre, string nom1, string nom2, string nom3, string nature, string impressAnnexe, string codeAnnexe, string attribut, string version, string user)
        {
            if (codeRattach == 0)
            {
                //insert
                codeRattach = InsertRattachClause(codeClause, codeEGDI, numOrdre, nom1, nom2, nom3, nature, impressAnnexe, codeAnnexe, attribut, version, user);
            }
            else
            {
                //update
                UpdateRattachClause(codeRattach, numOrdre, nature, impressAnnexe, codeAnnexe, attribut, user);
            }

            return codeRattach;
        }

        public static void DeleteAttachClause(string codeEGDI, string codeClause)
        {
            DbParameter[] param = new DbParameter[2];
            string sql = @"DELETE FROM KALCELC WHERE KEPKEJID = :KEPKEJID AND KEPKDUID = :KEPKDUID";

            param[0] = new EacParameter("KEPKEJID", Convert.ToInt64(codeEGDI));
            param[1] = new EacParameter("KEPKDUID", Convert.ToInt64(codeClause));

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static ParamAjoutContexteDto LoadContexte(string codeService, string codeActGes, string codeEtape, string codeContexte)
        {
            var model = new ParamAjoutContexteDto();
            ParametreDto paramServ = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "GENER", "SERVI", codeService);
            model.CodeService = paramServ.Code;
            model.LibelleService = paramServ.Libelle;

            ParametreDto paramActGes = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ACTGS", codeActGes);
            model.CodeActGes = paramActGes.Code;
            model.LibelleActGes = paramActGes.Libelle;

            ParametreDto paramEtape = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "ETAPE", codeEtape);
            model.CodeEtape = paramEtape.Code;
            model.LibelleEtape = paramEtape.Libelle;

            string sql = string.Format(@"SELECT KEIID IDCONTEXTE, KEICTX CODECONTEXTE, KEIEMOD MODIF, KEIAJC AJTCLAUSIER, KEIAJT AJTLIBRE, KEIMDT1 RUBRIQUE, 
                                            KEIMDT2 SOUSRUBRIQUE, KEIMDT3 SEQUENCE, KEISCID2 SCRIPT, KEICHI EMPLACEMENT, KEICHIS SOUSEMPLACEMENT, KEICXI NUMORDONNANCEMENT, KEIEXT CODESPECIFICITE
                                        FROM KALCONT WHERE KEIID = {0}", codeContexte);
            var result = DbBase.Settings.ExecuteList<ParamClauseContextePlatDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                var firstRes = result.FirstOrDefault();

                model.IdContexte = firstRes.IdContexte;

                ParametreDto paramContexte = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "CTX", firstRes.CodeContexte);
                model.CodeContexte = paramContexte != null ? paramContexte.Code : string.Empty;
                model.LibelleContexte = paramContexte != null ? paramContexte.Libelle : string.Empty;

                ParametreDto paramSpecificite = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "CTXSP", firstRes.CodeSpecificite);
                model.CodeSpecificite = paramSpecificite != null ? paramSpecificite.Code : string.Empty;
                model.LibelleSpecificite = paramSpecificite != null ? paramSpecificite.Libelle : string.Empty;

                model.EmplacementModif = firstRes.Modif == "O";
                model.AjoutClausier = firstRes.AjtClausier == "O";
                model.AjoutLibre = firstRes.AjtLibre == "O";
                model.ModeleClause1 = firstRes.Rubrique;
                model.ModelesClause1 = ClauseRepository.GetListRubrique();
                model.ModeleClause2 = firstRes.SousRubrique;
                model.ModelesClause2 = new List<ParametreDto>();
                model.ModeleClause3 = firstRes.Sequence.ToString();
                model.ModelesClause3 = new List<ParametreDto>();

                if (!string.IsNullOrEmpty(firstRes.Rubrique))
                {
                    model.ModelesClause2 = ClauseRepository.GetListSousRubriques(firstRes.Rubrique);
                    if (!string.IsNullOrEmpty(firstRes.SousRubrique))
                        model.ModelesClause3 = ClauseRepository.GetListSequences(firstRes.Rubrique, firstRes.SousRubrique);
                }

                model.ScriptControle = firstRes.Script;
                model.ScriptsControle = GetScript("SCRIPT");

                model.Emplacement = firstRes.Emplacement;
                model.Emplacements = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "EMP");
                model.SousEmplacement = firstRes.SousEmplacement;
                model.SousEmplacements = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "SSEMP");
                model.NumOrdonnancement = firstRes.NumOrdonnancement.ToString();
                model.NumOrdonnancements = new List<ParametreDto>();
                model.Specificite = firstRes.CodeSpecificite;
                model.Specificites = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "CTXSP");


            }

            return model;
        }

        public static void CopyParamClause(string src, string dest)
        {
            var sql = string.Format(@"DELETE FROM {0}/KALACTG", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KALACTG (SELECT * FROM {1}/KALACTG)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"DELETE FROM {0}/KALETAP", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KALETAP (SELECT * FROM {1}/KALETAP)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"DELETE FROM {0}/KALCONT", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KALCONT (SELECT * FROM {1}/KALCONT)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"DELETE FROM {0}/KALCELG", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KALCELG (SELECT * FROM {1}/KALCELG)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"DELETE FROM {0}/KHTSCRIPT", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KHTSCRIPT (SELECT * FROM {1}/KHTSCRIPT)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"DELETE FROM {0}/KCLAUSE", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KCLAUSE (SELECT * FROM {1}/KCLAUSE)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"DELETE FROM {0}/KCLAUDESI", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KCLAUDESI (SELECT * FROM {1}/KCLAUDESI)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"DELETE FROM {0}/KCLAASS", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KCLAASS (SELECT * FROM {1}/KCLAASS)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"DELETE FROM {0}/KMOTCLE", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KMOTCLE (SELECT * FROM {1}/KMOTCLE)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"DELETE FROM {0}/KEDICFG", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KEDICFG (SELECT * FROM {1}/KEDICFG)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

            sql = string.Format(@"DELETE FROM {0}/KCLAUCIB", dest);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            sql = string.Format(@"INSERT INTO {0}/KCLAUCIB (SELECT * FROM {1}/KCLAUCIB)", dest, src);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        #endregion

        #region Méthodes Privées

        /// <summary>
        /// Supprime tous les paramètres liés 
        /// à un acte de gestion.
        /// </summary>
        /// <param name="codeParam">Code acte de gestion.</param>
        private static void DeleteParamActeGestion(string codeParam)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("codeParam", codeParam);
            string sql = @"DELETE FROM KALCELG AS L WHERE EXISTS (
                            SELECT KEJID FROM KALCELG
                                INNER JOIN KALCONT ON KEJKEIID = KEIID
                                INNER JOIN KALETAP ON KEIKEHID = KEHID
                            WHERE KEHKEGID = :codeParam AND KEJID = L.KEJID)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            sql = @"DELETE FROM KALCONT AS L WHERE EXISTS (
                            SELECT KEIID FROM KALCONT 
                                INNER JOIN KALETAP ON KEIKEHID = KEHID
                            WHERE KEHKEGID = :codeParam AND KEIID = L.KEIID)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            sql = @"DELETE FROM KALETAP WHERE KEHKEGID = :codeParam";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Supprime tous les paramètres liés
        /// à une étape.
        /// </summary>
        /// <param name="codeParam">Code étape.</param>
        private static void DeleteParamEtape(string codeParam)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("codeParam", codeParam);
            //string sql = @"DELETE FROM KALCELG WHERE KEJKEIID IN (SELECT KEIID FROM KALCONT WHERE KEIKEHID = :codeParam)";
            string sql = @"DELETE FROM KALCELG AS L WHERE EXISTS (
                            SELECT KEJID FROM KALCELG
                                INNER JOIN KALCONT ON KEJKEIID = KEIID
                            WHERE KEIKEHID = :codeParam AND KEJID = L.KEJID)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            sql = @"DELETE FROM KALCONT WHERE KEIKEHID = :codeParam";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Supprime tous les paramètres liés
        /// à un contexte.
        /// </summary>
        /// <param name="codeParam">Code contexte.</param>
        private static void DeleteParamContexte(string codeParam)
        {
            DbParameter[] param = new DbParameter[1];
            param[0] = new EacParameter("codeParam", codeParam);
            string sql = @"DELETE FROM KALCELG WHERE KEJKEIID = :codeParam";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static List<string> GetActeGestionByService(string codeService)
        {
            List<string> toReturn = null;

            DbParameter[] param = new DbParameter[1];
            string sql = @"SELECT KEGACTG CODE 
                        FROM KALACTG
                        WHERE KEGSERV = :KEGSERV";
            param[0] = new EacParameter("KEGSERV", codeService);

            List<ParametreDto> actesGestion = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);

            if (actesGestion.Count > 0)
            {
                toReturn = new List<string>();
                actesGestion.ForEach(a => toReturn.Add(a.Code));
            }

            return toReturn;
        }

        private static List<string> GetEtapeByActeGestion(string codeService, string codeActGes)
        {
            List<string> toReturn = null;

            DbParameter[] param = new DbParameter[2];
            string sql = @"SELECT KEHETAPE CODE 
                        FROM KALETAP
                        WHERE KEHSERV = :KEHSERV AND KEHACTG = :KEHACTG";
            param[0] = new EacParameter("KEHSERV", codeService);
            param[1] = new EacParameter("KEHACTG", codeActGes);

            List<ParametreDto> etapes = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);

            if (etapes.Count > 0)
            {
                toReturn = new List<string>();
                etapes.ForEach(e => toReturn.Add(e.Code));
            }

            return toReturn;
        }

        private static List<string> GetContexteByEtape(string codeService, string codeActGes, string codeEtape)
        {
            List<string> toReturn = null;

            DbParameter[] param = new DbParameter[3];
            string sql = @"SELECT KEICTX CODE
                        FROM KALCONT
                        WHERE KEISERV = :KEISERV AND KEIACTG = :KEIACTG AND KEIETAPE = :KEIETAPE";
            param[0] = new EacParameter("KEISERV", codeService);
            param[1] = new EacParameter("KEIACTG", codeActGes);
            param[2] = new EacParameter("KEIETAPE", codeEtape);

            List<ParametreDto> contextes = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);

            if (contextes.Count > 0)
            {
                toReturn = new List<string>();
                contextes.ForEach(e => toReturn.Add(e.Code));
            }

            return toReturn;
        }

        private static Int64 GetNumOrdreEtape(string codeService, string codeActGes)
        {
            DbParameter[] param = new DbParameter[2];
            string sql = @"SELECT IFNULL(MAX(KEHETORD) + 10 , 10) NUMORDRE
                        FROM KALETAP 
                        WHERE KEHSERV = :KEHSERV AND KEHACTG = :KEHACTG";
            param[0] = new EacParameter("KEHSERV", codeService);
            param[1] = new EacParameter("KEHACTG", codeActGes);

            var paramListParamDto = DbBase.Settings.ExecuteList<ParamListParamDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (paramListParamDto != null)
                return paramListParamDto.NumOrdre;
            return 0;
        }

        private static Int64 GetIdActGesService(string codeService, string codeActGes)
        {
            DbParameter[] param = new DbParameter[2];
            string sql = @"SELECT KEGID CODE 
                    FROM KALACTG 
                    WHERE KEGSERV = :KEGSERV AND KEGACTG = :KEGACTG";
            param[0] = new EacParameter("KEGSERV", codeService);
            param[1] = new EacParameter("KEGACTG", codeActGes);

            var paramListParamDto = DbBase.Settings.ExecuteList<ParamListParamDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (paramListParamDto != null)
                return paramListParamDto.Code;
            return 0;
        }

        private static Int64 GetIdEtapeActGes(string codeService, string codeActGes, string codeEtape)
        {
            DbParameter[] param = new DbParameter[3];
            string sql = @"SELECT KEHID CODE 
                    FROM KALETAP 
                    WHERE KEHSERV = :KEHSERV AND KEHACTG = :KEHACTG AND KEHETAPE = :KEHETAPE";
            param[0] = new EacParameter("KEHSERV", codeService);
            param[1] = new EacParameter("KEHACTG", codeActGes);
            param[2] = new EacParameter("KEHETAPE", codeEtape);

            var paramListParamDto = DbBase.Settings.ExecuteList<ParamListParamDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (paramListParamDto != null)
                return paramListParamDto.Code;
            return 0;
        }

        private static Int64 GetIdContextEtape(string codeService, string codeActGes, string codeEtape, string codeContexte)
        {
            DbParameter[] param = new DbParameter[4];
            string sql = @"SELECT KEIID CODE 
                    FROM KALCONT 
                    WHERE KEISERV = :KEISERV AND KEIACTG = :KEIACTG AND KEIETAPE = :KEIETAPE AND KEICTX = :KEICTX";
            param[0] = new EacParameter("KEISERV", codeService);
            param[1] = new EacParameter("KEIACTG", codeActGes);
            param[2] = new EacParameter("KEIETAPE", codeEtape);
            param[3] = new EacParameter("KEICTX", codeContexte);

            var paramListParamDto = DbBase.Settings.ExecuteList<ParamListParamDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (paramListParamDto != null)
                return paramListParamDto.Code;
            return 0;
        }

        private static Int64 GetNumOrdreEGDI(string codeService, string codeActGes, string codeEtape, string codeContexte)
        {
            DbParameter[] param = new DbParameter[4];
            string sql = @"SELECT IFNULL(MAX(KEJORD) + 10, 10) NUMORDRE 
                        FROM KALCELG
                        WHERE KEJSERV = :KEJSERV AND KEJACTG = :KEJACTG AND KEJETAPE = :KEJETAPE
                            AND KEJCTX = :KEJCTX";
            param[0] = new EacParameter("KEJSERV", codeService);
            param[1] = new EacParameter("KEJACTG", codeActGes);
            param[2] = new EacParameter("KEJETAPE", codeEtape);
            param[3] = new EacParameter("KEJCTX", codeContexte);

            var paramListParamDto = DbBase.Settings.ExecuteList<ParamListParamDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (paramListParamDto != null)
                return paramListParamDto.NumOrdre;
            return 0;
        }

        /// <summary>
        /// Charge les infos de l'Élément Générateur.
        /// </summary>
        /// <param name="codeService">Code service</param>
        /// <param name="codeActGes">Code acte de gestion</param>
        /// <param name="codeEtape">Code étape</param>
        /// <param name="codeContexte">Code contexte</param>
        /// <param name="codeEGDI">Id EGDI</param>
        /// <returns></returns>
        private static ParamAjoutEGDIDto LoadEGDI(string codeService, string codeActGes, string codeEtape, string codeContexte, string codeEGDI)
        {
            DbParameter[] param = new DbParameter[5];
            string sql = @"SELECT KEJTYPE TYPE, KEJELG CODEEGDI, KEJORD NUMORDRE, KEJLIB1 LIBELLEEGDI, KEJLIB2 COMMENTAIRES 
                        FROM KALCELG
                        WHERE KEJID = :KEJID AND KEJSERV = :KEJSERV AND KEJACTG = :KEJACTG
                            AND KEJETAPE = :KEJETAPE AND KEJCTX = :KEJCTX";
            param[0] = new EacParameter("KEJID", Convert.ToInt32(codeEGDI));
            param[1] = new EacParameter("KEJSERV", codeService);
            param[2] = new EacParameter("KEJACTG", codeActGes);
            param[3] = new EacParameter("KEJETAPE", codeEtape);
            param[4] = new EacParameter("KEJCTX", codeContexte);
            return DbBase.Settings.ExecuteList<ParamAjoutEGDIDto>(CommandType.Text, sql, param).FirstOrDefault();
        }

        private static void InsertEGDI(string codeService, string codeActeGestion, string codeEtape, string codeContext, string type, string codeEGDI, int numOrdre, string libelleEGDI, string commentaire)
        {
            int codeParam = CommonRepository.GetAS400Id("KEJID");
            if (codeParam > 0)
            {
                DbParameter[] param = new DbParameter[11];
                string sql = @"INSERT INTO KALCELG
                        (KEJID, KEJSERV, KEJACTG, KEJETAPE, KEJCTX, KEJKEIID, KEJORD, KEJELG, KEJTYPE, KEJLIB1, KEJLIB2)
                        VALUES
                        (:KEJID, :KEJSERV, :KEJACTG, :KEJETAPE, :KEJCTX, :KEJKEIID, :KEJORD, :KEJELG, :KEJTYPE, :KEJLIB1, :KEJLIB2)";
                param[0] = new EacParameter("KEJID", codeParam);
                param[1] = new EacParameter("KEJSERV", codeService);
                param[2] = new EacParameter("KEJACTG", codeActeGestion);
                param[3] = new EacParameter("KEJETAPE", codeEtape);
                param[4] = new EacParameter("KEJCTX", codeContext);
                param[5] = new EacParameter("KEJKEIID", GetIdContextEtape(codeService, codeActeGestion, codeEtape, codeContext));
                param[6] = new EacParameter("KEJORD", numOrdre);
                param[7] = new EacParameter("KEJELG", codeEGDI);
                param[8] = new EacParameter("KEJTYPE", type);
                param[9] = new EacParameter("KEJLIB1", libelleEGDI);
                param[10] = new EacParameter("KEJLIB2", commentaire);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
            else
            {
                throw new Exception("Erreur lors de l'insertion du paramètre.");
            }
        }

        private static void UpdateEGDI(string type, int numOrdre, string libelleEGDI, string commentaire, string code)
        {
            DbParameter[] param = new DbParameter[5];
            string sql = @"UPDATE KALCELG
                            SET KEJORD = :KEJORD, KEJTYPE = :KEJTYPE, KEJLIB1 = :KEJLIB1, KEJLIB2 = :KEJLIB2
                            WHERE KEJID = :KEJID";
            param[0] = new EacParameter("KEJORD", numOrdre);
            param[1] = new EacParameter("KEJTYPE", type);
            param[2] = new EacParameter("KEJLIB1", libelleEGDI);
            param[3] = new EacParameter("KEJLIB2", commentaire);
            param[4] = new EacParameter("KEJID", Convert.ToInt32(code));
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static ParamListClausesDto GetClause(string codeEGDI, string codeClause)
        {
            DbParameter[] param = new DbParameter[2];
            string sql = @"SELECT KDUID CODE, KDUNM1 NOM1, KDUNM2 NOM2, KDUNM3 NOM3, KDULIB LIBELLE, IFNULL(KEPID, 0) RATTACH, IFNULL(KEPORD, 0) ORDRE,
                            IFNULL(KEPNTA, '') NATURE, IFNULL(KEPIAN, '') IMPRESSANNEXE, IFNULL(KEPIAC, '') CODEANNEXE, IFNULL(KEPAIM, '') ATTRIBUTIMPRESS,
                            KDUVER VERSION
                        FROM KCLAUSE
                            LEFT JOIN KALCELC ON KEPKDUID = KDUID AND KEPKEJID = :KEPKEJID
                        WHERE KDUID = :KDUID";
            param[0] = new EacParameter("KEPKEJID", Convert.ToInt32(codeEGDI));
            param[1] = new EacParameter("KDUID", Convert.ToInt32(codeClause));
            return DbBase.Settings.ExecuteList<ParamListClausesDto>(CommandType.Text, sql, param).FirstOrDefault();
        }

        private static Int64 GetNumOrdreClause(string codeEGDI, string codeClause)
        {
            DbParameter[] param = new DbParameter[4];
            string sql = @"SELECT IFNULL(MAX(KEPORD) + 100, 100) NUMORDRE 
                        FROM KALCELC
                        WHERE KEPKEJID = :KEPKEJID AND KEPKDUID = :KEPKDUID";

            param[0] = new EacParameter("KEPKEJID", Convert.ToInt32(codeEGDI));
            param[1] = new EacParameter("KEPKDUID", Convert.ToInt32(codeClause));

            var paramListParamDto = DbBase.Settings.ExecuteList<ParamListParamDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (paramListParamDto != null)
                return paramListParamDto.NumOrdre;
            return 0;
        }

        private static List<ParamListClausesDto> GetListClauses(string codeEGDI)
        {
            DbParameter[] param = new DbParameter[1];
            string sql = @"SELECT KDUID CODE, KDUNM1 NOM1, KDUNM2 NOM2, KDUNM3 NOM3, KDULIB LIBELLE, IFNULL(KEPID, 0) RATTACH 
                        FROM KCLAUSE
                            LEFT JOIN KALCELC ON KEPKDUID = KDUID AND KEPKEJID = :KEPKEJID";
            param[0] = new EacParameter("KEPKEJID", Convert.ToInt32(codeEGDI));
            return DbBase.Settings.ExecuteList<ParamListClausesDto>(CommandType.Text, sql, param);
        }

        /// <summary>
        /// Récupère la liste des clauses
        /// </summary>
        /// <param name="codeEGDI">Code de l'EG/DI</param>
        /// <param name="restrict">Chaine de restriction</param>
        /// <returns></returns>
        private static List<ParamListClausesDto> GetListClausesRestrict(string codeEGDI, string restrict)
        {
            DbParameter[] param = new DbParameter[1];
            string sql = @"SELECT KDUID CODE, KDUNM1 NOM1, KDUNM2 NOM2, KDUNM3 NOM3, KDULIB LIBELLE, IFNULL(KEPID, 0) RATTACH 
                        FROM KCLAUSE
                            LEFT JOIN KALCELC ON KEPKDUID = KDUID AND KEPKEJID = :KEPKEJID
                            LEFT JOIN KMOTCLE ON KDXID = KDUID
                        WHERE KDUNM1 LIKE :restrict OR KDUNM2 LIKE :restrict OR KDUNM3 LIKE :restrict 
                            OR KDULIB LIKE :restrict OR KDXMOC LIKE :restrict";
            param[0] = new EacParameter("KEPKEJID", Convert.ToInt32(codeEGDI));
            param[0] = new EacParameter("restrict", $"%{restrict}%");

            return DbBase.Settings.ExecuteList<ParamListClausesDto>(CommandType.Text, sql, param);
        }

        private static int InsertRattachClause(string codeClause, string codeEGDI, int numOrdre, string nom1, string nom2, string nom3, string nature, string impressAnnexe, string codeAnnexe, string attribut, string version, string user)
        {
            int codeRattach = CommonRepository.GetAS400Id("KEPID");

            if (codeRattach <= 0)
            {
                throw new Exception("Erreur lors du rattachement de la clause.");
            }
            DateTime dateNow = DateTime.Now;
            DbParameter[] param = new DbParameter[18];
            string sql = @"INSERT INTO KALCELC
                                (KEPID, KEPKEJID, KEPORD, KEPCNM1, KEPCNM2, KEPCNM3, KEPVER, KEPKDUID, KEPNTA, KEPIAN, KEPIAC, KEPAIM, KEPCRU, KEPCRD, KEPCRH, KEPMAJU, KEPMAJD, KEPMAJH)
                                VALUES
                                (:KEPID, :KEPKEJID, :KEPORD, :KEPCNM1, :KEPCNM2, :KEPCNM3, :KEPVER, :KEPKDUID, :KEPNTA, :KEPIAN, :KEPIAC, :KEPAIM, :KEPCRU, :KEPCRD, :KEPCRH, :KEPMAJU, :KEPMAJD, :KEPMAJH)";

            param[0] = new EacParameter("KEIPID", codeRattach);
            param[1] = new EacParameter("KEPKEJID", Convert.ToInt32(codeClause));
            param[2] = new EacParameter("KEPORD", Convert.ToInt64(numOrdre));
            param[3] = new EacParameter("KEPCNM1", nom1);
            param[4] = new EacParameter("KEPCNM2", nom2);
            param[5] = new EacParameter("KEPCNM3", Convert.ToInt32(nom3));
            param[6] = new EacParameter("KEPVER", Convert.ToInt32(version));
            param[7] = new EacParameter("KEPKDUID", Convert.ToInt64(codeEGDI));
            param[8] = new EacParameter("KEPNTA", nature);
            param[9] = new EacParameter("KEPIAN", impressAnnexe);
            param[10] = new EacParameter("KEPIAC", codeAnnexe);
            param[11] = new EacParameter("KEPAIM", attribut);
            param[12] = new EacParameter("KEPCRU", user);
            param[13] = new EacParameter("KEPCRD", AlbConvert.ConvertDateToInt(dateNow));
            param[14] = new EacParameter("KEPCRH", AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow)));
            param[15] = new EacParameter("KEPMAJU", user);
            param[16] = new EacParameter("KEPMAJD", AlbConvert.ConvertDateToInt(dateNow));
            param[17] = new EacParameter("KEPMAJH", AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow)));

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return codeRattach;
        }

        private static void UpdateRattachClause(int codeRattach, int numOrdre, string nature, string impressAnnexe, string codeAnnexe, string attribut, string user)
        {
            DateTime dateNow = DateTime.Now;
            DbParameter[] param = new DbParameter[9];
            string sql = @"UPDATE KALCELC
                                SET KEPORD = :KEPORD, KEPNTA = :KEPNTA, KEPIAN = :KEPIAN, KEPIAC = :KEPIAC, KEPAIM = :KEPAIM, KEPMAJU = :KEPMAJU, KEPMAJD = :KEPMAJD, KEPMAJH = :KEPMAJH
                            WHERE KEPID = :KEPID";

            param[0] = new EacParameter("KEPORD", Convert.ToInt64(numOrdre));
            param[1] = new EacParameter("KEPNTA", nature);
            param[2] = new EacParameter("KEPIAN", impressAnnexe);
            param[3] = new EacParameter("KEPIAC", codeAnnexe);
            param[4] = new EacParameter("KEPAIM", attribut);
            param[5] = new EacParameter("KEPMAJU", user);
            param[6] = new EacParameter("KEPMAJD", AlbConvert.ConvertDateToInt(dateNow));
            param[7] = new EacParameter("KEPMAJH", AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow)));
            param[8] = new EacParameter("KEPID", Convert.ToInt64(codeRattach));

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static List<ParametreDto> GetScript(string origine)
        {
            string sql = string.Format(@"SELECT KFEID1 CODE, KFEOBS LIBELLE FROM KHTSCRIPT WHERE KFEORIG = '{0}'", origine);
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        #endregion

    }
}
