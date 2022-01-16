using Albingia.Kheops.OP.Domain.Formule;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Condition;
using OP.WSAS400.DTO.Engagement;
using OP.WSAS400.DTO.MatriceFormule;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OP.WSAS400.DTO.RefExprComplexe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EasycomClient;
using System.Globalization;
using System.Linq;
using static DataAccess.Helpers.OutilsHelper;

namespace OP.DataAccess {
    public class ConditionRepository {

        #region Infos contrat

        public static LCIFranchiseDto GetLCIFranchise(string arg_NumOffre, string arg_Version, string arg_TypeOffre, string codeAvn, string codeRisque, AlbConstantesMetiers.ExpressionComplexe arg_typeVue, AlbConstantesMetiers.TypeAppel arg_typeAppel, ModeConsultation modeNavig) {
            LCIFranchiseDto lciFranchiseDto = new LCIFranchiseDto();
            switch (arg_typeAppel) {
                case AlbConstantesMetiers.TypeAppel.Generale:
                    lciFranchiseDto = GetLCIFranchiseGenerale(arg_NumOffre, arg_Version, arg_TypeOffre, codeAvn, arg_typeVue, modeNavig);
                    break;
                case AlbConstantesMetiers.TypeAppel.Risque:
                    lciFranchiseDto = GetLCIFranchiseRisque(arg_NumOffre, arg_Version, arg_TypeOffre, codeAvn, codeRisque, arg_typeVue, modeNavig);
                    break;
            }
            return lciFranchiseDto;
        }

        public static string AssietteGet(string arg_NumOffre, string arg_Version) {
            string toReturn = string.Empty;

            var param = new EacParameter[2];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = arg_NumOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = !string.IsNullOrEmpty(arg_Version) ? Convert.ToInt32(arg_Version) : 0;

            string sql = @"SELECT 1 NBLIGN FROM YPRTENT WHERE JDIPB = :codeOffre AND JDALX = :version FETCH FIRST 1 ROW ONLY";
            if (CommonRepository.ExistRowParam(sql, param)) {
                sql = @"SELECT JDSHT FROM YPRTENT WHERE JDIPB = :codeoffre AND JDALX = :version";
                var result = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param);
                if (result != null)
                    toReturn = result.ToString();
            }

            return toReturn;
        }


        #endregion

        #region Condition

        public static List<EnsembleGarantieDto> LstEnsembleLigne(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig, bool isReadOnly, string conditionId = "") {
            (bool isModeAvt, DateTime? dateEffetAvt, DateTime? dateEffet) = GetInfosBaseAvn(codeOffre, version, type, codeAvn, modeNavig);
            DateTime? dDebRsq = GetInfosRisque(codeOffre, version, type, codeFormule, codeOption, modeNavig);
            var resultObjPortee = GetInfosObjets(codeOffre, version, type, codeFormule, codeOption, modeNavig);

            List<EnsembleGarantieDto> model = new List<EnsembleGarantieDto>();
            List<EnsembleGarantiePlatDto> modelPlat = new List<EnsembleGarantiePlatDto>();

            if (modeNavig == ModeConsultation.Historique) {
                modelPlat = DbBase.Settings.ExecuteList<EnsembleGarantiePlatDto>(
                    CommandType.StoredProcedure,
                    "SP_LGNCONDHIST",
                    new[] {
                        new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength) { Value = codeOffre.ToIPB() },
                        new EacParameter("P_VERSION", DbType.Int32) { Value = int.TryParse(version, out int i) ? i : default },
                        new EacParameter("P_TYPE", DbType.AnsiStringFixedLength) { Value = type },
                        new EacParameter("P_CODEFORMULE", DbType.Int32) { Value = int.TryParse(codeFormule, out i) ? i : default },
                        new EacParameter("P_CODEOPTION", DbType.Int32) { Value = int.TryParse(codeOption, out i) ? i : default },
                        new EacParameter("P_CONDITIONID", DbType.Int32) { Value = int.TryParse(conditionId, out i) ? i : default },
                        new EacParameter("P_AVENANT", DbType.Int32) { Value = int.TryParse(codeAvn, out i) ? i : default }
                    });
            }
            else {
                modelPlat = DbBase.Settings.ExecuteList<EnsembleGarantiePlatDto>(
                    CommandType.StoredProcedure,
                    $"SP_LGNCOND{(isReadOnly ? "READONLY" : string.Empty)}",
                    new[] {
                        new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength) { Value = codeOffre.ToIPB() },
                        new EacParameter("P_VERSION", DbType.Int32) { Value = int.TryParse(version, out int i) ? i : default },
                        new EacParameter("P_TYPE", DbType.AnsiStringFixedLength) { Value = type },
                        new EacParameter("P_CODEFORMULE", DbType.Int32) { Value = int.TryParse(codeFormule, out i) ? i : default },
                        new EacParameter("P_CODEOPTION", DbType.Int32) { Value = int.TryParse(codeOption, out i) ? i : default },
                        new EacParameter("P_CONDITIONID", DbType.Int32) { Value = int.TryParse(conditionId, out i) ? i : default }
                    });
            }

            if (modelPlat != null && modelPlat.Count > 0) {
                var lstGarantie = modelPlat.GroupBy(el => new { el.Code, el.IdSequence }).Select(g => g.First()).ToList();
                bool allowGareat = lstGarantie.Any(g => g.AttentatGareat.AsBoolean() ?? false);
                foreach (var garantie in lstGarantie) {
                    EnsembleGarantieDto garan = BuildLineEnsembleGarantie(isModeAvt, dateEffetAvt, dateEffet, dDebRsq, resultObjPortee, model, modelPlat, garantie, allowGareat);
                    model.Add(garan);
                }
                if (!string.IsNullOrEmpty(conditionId) && model.Any()) {
                    return model;
                }
                List<GarantieVoletBloc> lstVoletBloc = GetListVoletBlocGarantie(codeOffre, version, type, codeFormule, codeOption, modeNavig, isReadOnly);

                //---------Volet
                var lstVolet = lstVoletBloc.FindAll(el => el.Type == "V");
                //---------Bloc
                var lstBloc = lstVoletBloc.FindAll(el => el.Type == "B" && lstVolet.Exists(v => el.CodeVolet == v.CodeVolet));

                var lstRes = new List<EnsembleGarantieDto>();
                //----------Nive1
                var lstNiveau1 = model.FindAll(el => el.Niveau == "1" && lstBloc.Exists(m => el.CodeBloc == Convert.ToString(m.Code)));
                //----------Nive2
                var lstNiveau2 = model.FindAll(el => el.Niveau == "2" && lstBloc.Exists(m => el.CodeBloc == Convert.ToString(m.Code)) && lstNiveau1.Exists(m => el.Pere == m.Sequence));
                //----------Nive3
                var lstNiveau3 = model.FindAll(el => el.Niveau == "3" && lstBloc.Exists(m => el.CodeBloc == Convert.ToString(m.Code)) && lstNiveau2.Exists(m => el.Pere == m.Sequence));
                //----------Niveau 4
                var lstNiveau4 = model.FindAll(el => el.Niveau == "4" && lstBloc.Exists(m => el.CodeBloc == Convert.ToString(m.Code)) && lstNiveau3.Exists(m => el.Pere == m.Sequence));

                if (lstNiveau4 != null)
                    lstNiveau4.FindAll(el => lstNiveau3.Exists(n3 => el.Pere == n3.Sequence)).ForEach(elm => lstRes.Add(elm));
                if (lstNiveau3 != null)
                    lstNiveau3.FindAll(el => lstNiveau2.Exists(n2 => el.Pere == n2.Sequence)).ForEach(elm => lstRes.Add(elm));
                if (lstNiveau2 != null)
                    lstNiveau2.FindAll(el => lstNiveau1.Exists(n1 => el.Pere == n1.Sequence)).ForEach(elm => lstRes.Add(elm));
                if (lstNiveau1 != null)
                    lstNiveau1.ForEach(elm => lstRes.Add(elm));

                return lstRes.OrderBy(c => c.TriVolet).ThenBy(c => c.TriBloc).ThenBy(c => c.TriDate).ThenBy(c => c.TriGar).ThenBy(c => c.Code).ToList();
            }
            return model;
        }

        public static ConditionComplexeDto GetDetailExprComplexe(string codeOffre, string version, string codeExpr, string typeExpr) {
            ConditionComplexeDto result = new ConditionComplexeDto();
            var param = new EacParameter[3];

            string sql = string.Empty;
            if (typeExpr == "LCI") {
                sql = @"SELECT KDIID IDEXPR, KDIDESC LIBEXPR, KDJID IDDETAIL, KDJORDRE ORDRE, KDJLCVAL VALEUR, KDJLCVAU UNITE, KDJLCBASE TYPE, 
                        KDJLOVAL VALEURCONCURRENCE, KDJLOVAU UNITECONCURRENCE, KDJLOBASE TYPECONCURRENCE, KADDESI DESCEXPR, KDILCE CODEEXPR, 'LCI' TYPEEXPR,
                        '' MINI, '' MAXI, '' DEBUT, '' FIN 
                    FROM KPEXPLCI 
                        LEFT JOIN KPEXPLCID ON KDIID = KDJKDIID
                        LEFT JOIN KPDESI ON KDIDESI = KADCHR
                    WHERE KDIIPB = :KDIIPB AND KDIALX = :KDIALX AND KDILCE = :KDILCE";
                param[0] = new EacParameter("KDIIPB", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("KDIALX", DbType.AnsiStringFixedLength);
                param[1].Value = version;
                param[2] = new EacParameter("KDILCE", DbType.AnsiStringFixedLength);
                param[2].Value = codeExpr;
            }
            else if (typeExpr == "Franchise") {
                sql = @"SELECT KDKID IDEXPR, KDKDESC LIBEXPR, KDLID IDDETAIL, KDLORDRE ORDRE, KDLFHVAL VALEUR, KDLFHVAU UNITE, KDLFHBASE TYPE, 
                        KDLFHMINI MINI, KDLFHMAXI MAXI, KDLLIMDEB DEBUT, KDLLIMFIN FIN, KADDESI DESCEXPR, KDKFHE CODEEXPR, 'Franchise' TYPEEXPR,
                        '' VALEURCONCURRENCE, '' UNITECONCURRENCE, '' TYPECONCURRENCE
                    FROM KPEXPFRH 
                        LEFT JOIN KPEXPFRHD ON KDKID = KDLKDKID
                        LEFT JOIN KPDESI ON KDKDESI = KADCHR
                    WHERE KDKIPB = :KDKIPB AND KDKALX = :KDKALX AND KDKFHE = :KDKFHE";
                param[0] = new EacParameter("KDKIPB", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("KDKALX", DbType.AnsiStringFixedLength);
                param[1].Value = version;
                param[2] = new EacParameter("KDKFHE", DbType.AnsiStringFixedLength);
                param[2].Value = codeExpr;
            }
            var res = !string.IsNullOrEmpty(sql) ? DbBase.Settings.ExecuteList<ConditionComplexeLineDto>(CommandType.Text, sql, param).FirstOrDefault() : null;

            if (res != null) {
                MapConditionComplexe(result, res);
            }

            return result;
        }

        public static RisqueDto GetListRisque(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig) {
            RisqueDto risque = new RisqueDto();

            var param = new List<EacParameter>() {
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(version)),
                new EacParameter("type", type),
                new EacParameter("codeFormule", Convert.ToInt32(codeFormule)),
                new EacParameter("codeOption", Convert.ToInt32(codeOption))
            };
            if (modeNavig == ModeConsultation.Historique) {
                param.Add(new EacParameter("codeAvn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }


            string sql = string.Format(@"SELECT KABRSQ CODERSQ, KABDESC DESCRSQ, KACOBJ CODEOBJ, KACDESC DESCOBJ FROM {0}
	                    LEFT JOIN {1} ON KDDTYP = KABTYP AND KDDIPB = KABIPB AND KDDALX = KABALX AND KDDRSQ = KABRSQ {4}
	                    LEFT JOIN {2} ON KDDTYP = KACTYP AND KDDIPB = KACIPB AND KDDALX = KACALX AND KDDRSQ = KACRSQ AND (KDDOBJ = KACOBJ OR (KDDOBJ = 0 AND 1 = 1)) {5}
                    WHERE KDDIPB = :codeOffre AND KDDALX = :version AND KDDTYP = :type AND KDDFOR = :codeFormule AND KDDOPT = :codeOption {3}",
                modeNavig == ModeConsultation.Historique ? "HPOPTAP" : "KPOPTAP",
                CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                modeNavig == ModeConsultation.Historique ? " AND KDDAVN = :codeAvn" : string.Empty,
                modeNavig == ModeConsultation.Historique ? " AND KDDAVN = KABAVN" : string.Empty,
                modeNavig == ModeConsultation.Historique ? " AND KDDAVN = KACAVN" : string.Empty);

            var result = DbBase.Settings.ExecuteList<RisqueObjetPlatDto>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0) {
                var lstRsq = result.GroupBy(el => el.CodeRsq).Select(r => r.First()).FirstOrDefault();
                risque.Code = Convert.ToInt32(lstRsq.CodeRsq);
                risque.Descriptif = lstRsq.DescRsq;
                risque.Objets = new List<ObjetDto>();

                var lstObj = result.FindAll(o => o.CodeRsq == lstRsq.CodeRsq);
                foreach (var obj in lstObj) {
                    risque.Objets.Add(new ObjetDto {
                        Code = Convert.ToInt32(obj.CodeObj),
                        Descriptif = obj.DescObj
                    });
                }
            }

            return risque;
        }

        public static void ResetExprCompCondition(string codeCondition, string oldFRHExpr, string oldLCIExpr) {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("P_CODECONDITION", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(codeCondition) ? Convert.ToInt32(codeCondition) : 0;
            param[1] = new EacParameter("P_OLDFRHEXPR", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(oldFRHExpr) ? Convert.ToInt32(oldFRHExpr) : 0;
            param[2] = new EacParameter("P_OLDLCIEXPR", DbType.Int32);
            param[2].Value = !string.IsNullOrEmpty(oldLCIExpr) ? Convert.ToInt32(oldLCIExpr) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_RESETEXPRCOMPCONDITION", param);
        }

        public static bool DeleteCondition(string codeCondition) {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("KDGID", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(codeCondition) ? Convert.ToInt32(codeCondition) : 0;

            string sql = @"DELETE FROM KPGARTAW WHERE KDGID = :KDGID";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return true;
        }

        public static int SaveCondition(LigneGarantieDto conditionDto, string codeAvn) {
            EacParameter[] param = new EacParameter[20];
            param[0] = new EacParameter("P_CODEGARANTIE", DbType.Int32);
            param[0].Value = string.IsNullOrEmpty(conditionDto.Code) ? 0 : Convert.ToInt32(conditionDto.Code.Split('_')[1]);
            param[1] = new EacParameter("P_CODETARIF", DbType.Int32);
            param[1].Value = string.IsNullOrEmpty(conditionDto.Code) ? 0 : Convert.ToInt32(conditionDto.Code.Split('_')[0]);
            param[2] = new EacParameter("P_NUMTAR", DbType.Int32);
            param[2].Value = Convert.ToInt32(conditionDto.NumeroTarif);
            param[3] = new EacParameter("P_LCIVALEUR", DbType.Decimal);
            param[3].Value = !string.IsNullOrEmpty(conditionDto.LCIValeur) ? Convert.ToDecimal(conditionDto.LCIValeur) : 0;
            param[4] = new EacParameter("P_LCIUNIT", DbType.AnsiStringFixedLength);
            param[4].Value = conditionDto.LCIUnite == "UM" ? "D" : conditionDto.LCIUnite;
            param[5] = new EacParameter("P_LCITYPE", DbType.AnsiStringFixedLength);
            param[5].Value = conditionDto.LCIType;
            param[6] = new EacParameter("P_LCICOMPLEXE", DbType.Int32);
            param[6].Value = Convert.ToInt32(conditionDto.LienLCIComplexe.Split('¤')[0]);
            param[7] = new EacParameter("P_FRHVALEUR", DbType.Decimal);
            param[7].Value = !string.IsNullOrEmpty(conditionDto.FranchiseValeur) ? Convert.ToDecimal(conditionDto.FranchiseValeur) : 0;
            param[8] = new EacParameter("P_FRHUNIT", DbType.AnsiStringFixedLength);
            param[8].Value = conditionDto.FranchiseUnite == "UM" ? "D" : conditionDto.FranchiseUnite;
            param[9] = new EacParameter("P_FRHTYPE", DbType.AnsiStringFixedLength);
            param[9].Value = conditionDto.FranchiseType;
            param[10] = new EacParameter("P_FRHCOMPLEXE", DbType.Int32);
            param[10].Value = Convert.ToInt32(conditionDto.LienFRHComplexe.Split('¤')[0]);
            param[11] = new EacParameter("P_TXHTVALEUR", DbType.Decimal);
            param[11].Value = !string.IsNullOrEmpty(conditionDto.TauxForfaitHTValeur) ? Convert.ToDecimal(conditionDto.TauxForfaitHTValeur) : 0;
            param[12] = new EacParameter("P_TXHTUNIT", DbType.AnsiStringFixedLength);
            param[12].Value = conditionDto.TauxForfaitHTUnite == "UM" ? "D" : conditionDto.TauxForfaitHTUnite;
            param[13] = new EacParameter("P_TXHTMINI", DbType.Decimal);
            param[13].Value = !string.IsNullOrEmpty(conditionDto.TauxForfaitHTMinimum) ? Convert.ToDecimal(conditionDto.TauxForfaitHTMinimum) : 0;
            param[14] = new EacParameter("P_ASSVALEUR", DbType.Decimal);
            param[14].Value = !string.IsNullOrEmpty(conditionDto.AssietteValeur) ? Convert.ToDecimal(conditionDto.AssietteValeur) : 0;
            param[15] = new EacParameter("P_ASSUNIT", DbType.AnsiStringFixedLength);
            param[15].Value = conditionDto.AssietteUnite;
            param[16] = new EacParameter("P_ASSTYPE", DbType.AnsiStringFixedLength);
            param[16].Value = conditionDto.AssietteType;
            param[17] = new EacParameter("P_TRAITEMENT", DbType.AnsiStringFixedLength);
            param[17].Value = string.IsNullOrEmpty(conditionDto.MAJ) ? "U" : conditionDto.MAJ;
            param[18] = new EacParameter("P_CODEAVN", DbType.Int32);
            param[18].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
            param[19] = new EacParameter("P_NEWCODETARIF", 0);
            param[19].Direction = ParameterDirection.InputOutput;
            param[19].DbType = DbType.Int32;
            param[19].Value = 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DUPLCON", param);

            return Convert.ToInt32(param[19].Value.ToString());
        }

        public static List<ConditionComplexeDto> RecupConditionComplexe(string codeOffre, string version, string type, string typeExpr) {
            List<ConditionComplexeDto> toReturn = new List<ConditionComplexeDto>();
            string sql = string.Empty;

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            switch (typeExpr) {
                case "LCI":
                    sql = @"SELECT KDIID GUIDID, KDILCE CODEEXPR, KDIDESC LIBEXPR, KDIORI ORIGINE, KDIMODI MODIFEXPR
                                FROM KPEXPLCI
                                WHERE KDIIPB = :codeOffre AND KDIALX = :version AND KDITYP = :type
                            ORDER BY CODEEXPR";
                    break;
                case "Franchise":
                    sql = @"SELECT KDKID GUIDID, KDKFHE CODEEXPR, KDKDESC LIBEXPR, KDKORI ORIGINE, KDKMODI MODIFEXPR
                                FROM KPEXPFRH
                                WHERE KDKIPB = :codeOffre AND KDKALX = :version AND KDKTYP = :type
                            ORDER BY CODEEXPR";
                    break;
            }

            if (!string.IsNullOrEmpty(sql)) {
                var result = DbBase.Settings.ExecuteList<ExprComplexeDetailDto>(CommandType.Text, sql, param);
                if (result != null && result.Any()) {
                    foreach (var item in result) {
                        ConditionComplexeDto paramCond = new ConditionComplexeDto();
                        int? temp;
                        AlbNullableInt.TryParse(item.GuidId.ToString().Trim(), out temp);

                        paramCond.Id = item.GuidId.ToString();
                        paramCond.Code = item.CodeExpr;
                        paramCond.Libelle = item.LibExpr;
                        paramCond.Modifiable = item.ModifExpr != "N";
                        paramCond.Utilise = GetUtiliseEXPCOMP(temp, typeExpr);
                        paramCond.Origine = item.Origine;

                        toReturn.Add(paramCond);
                    }
                }
            }

            return toReturn;
        }

        public static void SuppressionExpression(string argType, string argTypeAppel, string argIdExpression, string argIdCondition) {
            if (NbExpressions(argType, argTypeAppel, argIdExpression, argIdCondition)) {
                EacParameter[] param = new EacParameter[1];
                param[0] = new EacParameter("argIdExpression", DbType.Int32);
                param[0].Value = !string.IsNullOrEmpty(argIdExpression) ? Convert.ToInt32(argIdExpression) : 0;

                string sql = string.Empty;
                if (argType == "LCI")
                    sql = "DELETE FROM KPEXPLCI WHERE KDIID = :argIdExpression";
                else
                    sql = "DELETE FROM KPEXPFRH WHERE KDKID = :argIdExpression";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                if (argType == "LCI")
                    sql = "UPDATE KPGARTAW SET KDGKDIID = 0 WHERE KDGKDIID = :argIdExpression";
                else
                    sql = "UPDATE KPGARTAW SET KDGKDKID = 0  WHERE KDGKDKID = :argIdExpression";

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
            }
        }

        public static ConditionComplexeDto RecupDetailComplexe(string argCodeOffre, string argVersion, string argTypeOffre, string argCode, string argType) {
            ConditionComplexeDto result = new ConditionComplexeDto();
            string sql = string.Empty;
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = argCodeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(argVersion) ? Convert.ToInt32(argVersion) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = argTypeOffre;
            param[3] = new EacParameter("code", DbType.AnsiStringFixedLength);
            param[3].Value = argCode;


            if (argType.Trim() == "LCI") {
                sql = @"SELECT KDIID IDEXPR, KDIDESC LIBEXPR, KDJID IDDETAIL, KDJORDRE ORDRE, KDJLCVAL VALEUR, KDJLCVAU UNITE, KDJLCBASE TYPE,
                                KDJLOVAL VALEURCONCURRENCE, KDJLOVAU UNITECONCURRENCE, KDJLOBASE TYPECONCURRENCE, KADDESI DESCEXPR 
                            FROM (KPEXPLCI A LEFT JOIN KPEXPLCID B ON A.KDIID = B.KDJKDIID) LEFT JOIN KPDESI C ON A.KDIDESI = C.KADCHR 
                            WHERE KDIIPB = :codeOffre AND KDIALX = :version AND KDITYP = :type AND KDILCE= :code 
                            ORDER BY ORDRE";
            }
            else if (argType.Trim() == "Franchise") {
                sql = @"SELECT KDKID IDEXPR, KDKDESC LIBEXPR, KDLID IDDETAIL, KDLORDRE ORDRE, KDLFHVAL VALEUR, KDLFHVAU UNITE, KDLFHBASE TYPE,
                                KDLFHMINI MINI, KDLFHMAXI MAXI, KDLLIMDEB DEBUT, KDLLIMFIN FIN, KADDESI DESCEXPR 
                            FROM (KPEXPFRH A LEFT JOIN KPEXPFRHD B ON A.KDKID = B.KDLKDKID) LEFT JOIN KPDESI C ON A.KDKDESI = C.KADCHR 
                            WHERE KDKIPB = :codeOffre AND KDKALX = :version AND KDKTYP = :type AND KDKFHE = :code
                            ORDER BY ORDRE";
            }

            var listComplexes = DbBase.Settings.ExecuteList<ConditionComplexeLineDto>(CommandType.Text, sql, param);
            if (listComplexes != null && listComplexes.Any()) {
                result = new ConditionComplexeDto();
                foreach (var item in listComplexes) {
                    result.Type = argType;
                    result.Code = argCode;
                    result.Id = item.Id.ToString(CultureInfo.InvariantCulture).Trim();
                    result.Libelle = !string.IsNullOrEmpty(item.Libelle) ? item.Libelle.Trim() : string.Empty;
                    result.Descriptif = !string.IsNullOrEmpty(item.Descriptif) ? item.Descriptif.Trim() : string.Empty;
                    LigneGarantieDto paramGar = new LigneGarantieDto();
                    paramGar.Code = item.IdDetail.ToString(CultureInfo.InvariantCulture).Trim();
                    if (argType == "LCI") {
                        paramGar.LCIValeur = item.Valeur.ToString(CultureInfo.InvariantCulture).Trim();
                        paramGar.LCIUnite = string.IsNullOrEmpty(item.Unite) ? string.Empty : item.Unite.Trim();
                        paramGar.LCIType = string.IsNullOrEmpty(item.Type) ? string.Empty : item.Type.Trim();
                        paramGar.ConcurrenceValeur = item.ValeurConcurrence.ToString(CultureInfo.InvariantCulture).Trim();
                        paramGar.ConcurrenceUnite = string.IsNullOrEmpty(item.UniteConcurrence) ? string.Empty : item.UniteConcurrence.Trim();
                        paramGar.ConcurrenceType = string.IsNullOrEmpty(item.TypeConcurrence) ? string.Empty : item.TypeConcurrence.Trim();
                    }
                    else if (argType == "Franchise") {
                        paramGar.FranchiseValeur = item.Valeur.ToString(CultureInfo.InvariantCulture).Trim();
                        paramGar.FranchiseUnite = string.IsNullOrEmpty(item.Unite) ? string.Empty : item.Unite.Trim();
                        paramGar.FranchiseType = string.IsNullOrEmpty(item.Type) ? string.Empty : item.Type.Trim();
                        int resultOut;
                        paramGar.FranchiseMini = item.Mini.ToString(CultureInfo.InvariantCulture).Trim();
                        paramGar.FranchiseMaxi = item.Maxi.ToString(CultureInfo.InvariantCulture).Trim();
                        paramGar.FranchiseDebut = AlbConvert.ConvertIntToDate(int.TryParse(item.Debut.ToString(CultureInfo.InvariantCulture), out resultOut) ? resultOut : 0);
                        paramGar.FranchiseFin = AlbConvert.ConvertIntToDate(int.TryParse(item.Fin.ToString(CultureInfo.InvariantCulture), out resultOut) ? resultOut : 0);

                    }
                    result.LstLigneGarantie.Add(paramGar);
                }
            }
            return result;
        }

        public static string EnregistreExpressionDetail(ConditionComplexeDto argExpComp, string argTypeOffre, string argType, string argCodeOffre, string argVersion, int? argIdExpression, string argLibelle, string argDescriptif) {
            if (argType == "LCI") {
                if (argIdExpression == null) {
                    argIdExpression = CreationExpressionLCI(argTypeOffre, argCodeOffre, argVersion, argLibelle, argDescriptif);
                }
                EnregistrementLCI(argTypeOffre, argCodeOffre, argVersion, argExpComp, argIdExpression, argLibelle, argDescriptif);
            }
            else if (argType == "Franchise") {
                if (argIdExpression == null) {
                    argIdExpression = CreationExpressionFranchise(argTypeOffre, argCodeOffre, argVersion, argLibelle, argDescriptif);
                }
                EnregistrementFranchise(argTypeOffre, argCodeOffre, argVersion, argExpComp, argIdExpression, argLibelle, argDescriptif);
            }
            return argIdExpression == null ? string.Empty : argIdExpression.ToString();
        }

        public static void EnregistrementExpCompPourCond(string argType, string argCodeCondition, string argCodeExpression) {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("codeExpr", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(argCodeExpression) ? Convert.ToInt32(argCodeExpression) : 0;
            param[1] = new EacParameter("codeCondition", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(argCodeCondition) ? Convert.ToInt32(argCodeCondition) : 0;

            string sql = string.Empty;
            if (argType == "LCI")
                sql = "UPDATE KPGARTAW SET KDGKDIID = :codeExpr WHERE KDGID = :codeCondition";
            else
                sql = "UPDATE KPGARTAW SET KDGKDKID = :codeExpr WHERE KDGID = :codeCondition";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static void SuppressionDetailLCI(string argId) {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("code", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(argId) ? Convert.ToInt32(argId) : 0;

            string sql = "DELETE FROM KPEXPLCID WHERE KDJID = :code";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static List<EngagementPeriodeDto> IsInHpeng(string argId) {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = argId.PadLeft(9, ' ');

            string sql = "SELECT KDOID CODE FROM kpeng k WHERE k.KDOIPB = :P_CODEOFFRE AND NOT EXISTS(SELECT * FROM hpeng h WHERE k.KDOID = h.KDOID AND k.KDOIPB = h.KDOIPB)";
            return DbBase.Settings.ExecuteList<EngagementPeriodeDto>(CommandType.Text, sql, param);


        }


        public static void SuppressionDetailFranchise(string argId)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("code", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(argId) ? Convert.ToInt32(argId) : 0;

            string sql = "DELETE FROM KPEXPFRHD WHERE KDLID = :code";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        /// <summary>
        /// Récupération de la liste de filtre des garanties 
        /// pour les conditions tarifaires de garantie
        /// </summary>
        public static List<ParametreDto> GetFiltreCondition(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption,
            string typeFiltre, ModeConsultation modeNavig, string codeGarantie = "", string codeVolet = "", string codeBloc = "", string niveau = "") {
            if (modeNavig == ModeConsultation.Historique) {
                EacParameter[] param = new EacParameter[11];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("P_VERSION", DbType.Int32);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[2].Value = type;
                param[3] = new EacParameter("P_CODEAVENANT", DbType.Int32);
                param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
                param[4] = new EacParameter("P_CODEFORMULE", DbType.Int32);
                param[4].Value = Convert.ToInt32(codeFormule);
                param[5] = new EacParameter("P_CODEOPTION", DbType.Int32);
                param[5].Value = Convert.ToInt32(codeOption);
                param[6] = new EacParameter("P_TYPEFILTRE", DbType.AnsiStringFixedLength);
                param[6].Value = typeFiltre;
                param[7] = new EacParameter("P_CODEGARANTIE", DbType.AnsiStringFixedLength);
                param[7].Value = codeGarantie;
                param[8] = new EacParameter("P_CODEVOLET", DbType.Int32);
                param[8].Value = !string.IsNullOrEmpty(codeVolet) ? Convert.ToInt32(codeVolet) : 0;
                param[9] = new EacParameter("P_CODEBLOC", DbType.Int32);
                param[9].Value = !string.IsNullOrEmpty(codeBloc) ? Convert.ToInt32(codeBloc) : 0;
                param[10] = new EacParameter("P_NIVEAU", DbType.Int32);
                param[10].Value = !string.IsNullOrEmpty(niveau) ? Convert.ToInt32(niveau) : 0;

                return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.StoredProcedure, "SP_GETCONDHIST", param);
            }
            else {
                EacParameter[] param = new EacParameter[10];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("P_VERSION", DbType.Int32);
                param[1].Value = Convert.ToInt32(version);
                param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[2].Value = type;
                param[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
                param[3].Value = Convert.ToInt32(codeFormule);
                param[4] = new EacParameter("P_CODEOPTION", DbType.Int32);
                param[4].Value = Convert.ToInt32(codeOption);
                param[5] = new EacParameter("P_TYPEFILTRE", DbType.AnsiStringFixedLength);
                param[5].Value = typeFiltre;
                param[6] = new EacParameter("P_CODEGARANTIE", DbType.AnsiStringFixedLength);
                param[6].Value = codeGarantie;
                param[7] = new EacParameter("P_CODEVOLET", DbType.Int32);
                param[7].Value = !string.IsNullOrEmpty(codeVolet) ? Convert.ToInt32(codeVolet) : 0;
                param[8] = new EacParameter("P_CODEBLOC", DbType.Int32);
                param[8].Value = !string.IsNullOrEmpty(codeBloc) ? Convert.ToInt32(codeBloc) : 0;
                param[9] = new EacParameter("P_NIVEAU", DbType.Int32);
                param[9].Value = !string.IsNullOrEmpty(niveau) ? Convert.ToInt32(niveau) : 0;

                return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.StoredProcedure, "SP_GETCOND", param);
            }
        }

        /// <summary>
        /// Retourne le libelle de la formule
        /// </summary>
        /// <param name="codeOffre">Code offre.</param>
        /// <param name="version">Version.</param>
        /// <param name="codeFormule">Code formule.</param>
        /// <returns></returns>
        public static string GetLibelleFormule(string codeOffre, string version, string type, string codeAvn, string codeFormule, ModeConsultation modeNavig) {
            string toReturn = string.Empty;

            var param = new List<EacParameter> {
                new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(version)),
                new EacParameter("type", type),
                new EacParameter("codeFor", Convert.ToInt32(codeFormule))
            };
            if (modeNavig == ModeConsultation.Historique) {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string sql = string.Format(@"SELECT KDAALPHA LETTRELIB, KDADESC LIBELLE
                            FROM {0} WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type AND KDAFOR = :codeFor {1}",
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                            modeNavig == ModeConsultation.Historique ? " AND KDAAVN = :avn" : string.Empty);
            var list = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (list != null && list.Any()) {
                foreach (var item in list) {
                    toReturn = item.LettreLib + " - " + item.Libelle;
                }
            }
            return toReturn;
        }

        public static (int avnOptCreate, int avnOptModif)? GetOptionStateAvn(string ipb, int alx, int opt, int formule) {
            var list = DbBase.Settings.ExecuteList<(int, int)>(
                CommandType.Text,
                $"SELECT KDBAVE , KDBAVG FROM KPOPT WHERE ( KDBIPB , KDBALX , KDBOPT , KDBFOR ) = ( :{nameof(ipb)}, :{nameof(alx)} , :{nameof(opt)} , :{nameof(formule)} ) ",
                new[] {
                    new EacParameter(nameof(ipb), ipb.ToIPB()),
                    new EacParameter(nameof(alx), DbType.Int32) { Value = alx },
                    new EacParameter(nameof(opt), DbType.Int32) { Value = opt },
                    new EacParameter(nameof(formule), DbType.Int32) { Value = formule }
            });

            if (list?.Any() != true) {
                return null;
            }
            return list.First();
        }

        /// <summary>
        /// Retourne le texte s'applique à
        /// </summary>
        /// <param name="codeOffre">Code offre.</param>
        /// <param name="version">Version.</param>
        /// <param name="codeFormule">Code formule.</param>
        /// <param name="codeOption">Code option.</param>
        /// <returns></returns>
        public static string GetAppliqueA(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig) {
            string toReturn = string.Empty;

            List<DtoCommon> risquesCodes = PoliceRepository.ObtenirRisquesCodes(modeNavig, codeOffre, Convert.ToInt32(version), type, codeAvn);

            string rsqobjApplique = ObtenirRisqueObjetFormule(codeOffre, version, type, codeAvn, codeFormule, codeOption, modeNavig);

            if (!string.IsNullOrEmpty(rsqobjApplique)) {

                int nbObjApplique = -1;
                if (rsqobjApplique.Split(';').Length > 1) {
                    nbObjApplique = rsqobjApplique.Split(';')[1].Split('_').Count();
                }

                if (risquesCodes.Count == 1) {
                    if (risquesCodes[0].NbLigne == nbObjApplique || nbObjApplique == -1) {
                        toReturn = "à l'ensemble du risque" + risquesCodes[0].Code;
                    }
                    else {
                        toReturn = "à " + nbObjApplique + " objet" + (nbObjApplique > 1 ? "(s)" : "") + " du risque" + risquesCodes[0].Code;
                    }
                }
                else {
                    DtoCommon rsq = risquesCodes.FirstOrDefault(r => r.Code == rsqobjApplique.Split(';')[0]);
                    if (rsq.NbLigne == nbObjApplique || nbObjApplique == -1) {
                        toReturn = "à l'ensemble du risque" + rsq.Code;
                    }
                    else {
                        toReturn = "à " + nbObjApplique + " objet" + (nbObjApplique > 1 ? "(s)" : "") + " du risque" + rsq.Code;
                    }
                }
            }
            return toReturn;
        }

        public static string ObtenirRisqueObjetFormule(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig) {
            var toReturn = string.Empty;
            var codeRisque = string.Empty;
            var codeObjets = string.Empty;

            var param = new List<EacParameter>{
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(version)),
                new EacParameter("type", type),
                new EacParameter("codeFor", !string.IsNullOrEmpty(codeFormule) ? Convert.ToInt32(codeFormule) : 0),
                new EacParameter("codeOpt", !string.IsNullOrEmpty(codeOption) ? Convert.ToInt32(codeOption) : 0)
            };
            if (modeNavig == ModeConsultation.Historique) {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string sql = string.Format(@"SELECT DISTINCT KDDRSQ CODERISQUE, KDDOBJ CODEOBJET FROM {0}
                                       WHERE KDDIPB = :codeOffre AND KDDALX = :version AND KDDTYP = :type AND KDDFOR = :codeFor AND KDDOPT = :codeOpt {1}",
                                       CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTAP"),
                                       modeNavig == ModeConsultation.Historique ? " AND KDDAVN = :avn" : string.Empty);
            var list = DbBase.Settings.ExecuteList<MatriceFormuleLineDto>(CommandType.Text, sql, param);
            if (list != null && list.Any()) {
                foreach (var item in list) {
                    codeRisque = item.CodeRisque.ToString();
                    if (item.CodeObjet.ToString() != "0")
                        codeObjets += "_" + item.CodeObjet.ToString();
                }
            }

            if (!string.IsNullOrEmpty(codeRisque)) {
                toReturn = codeRisque;
                if (!string.IsNullOrEmpty(codeObjets)) {
                    toReturn += ";" + codeObjets.Substring(1);
                }
            }

            return toReturn;
        }

        #region Méthodes privées

        /// <summary>
        /// Récupération des infos des objets
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeOption"></param>
        /// <param name="modeNavig"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="resultObjPortee"></param>
        private static List<DtoCommon> GetInfosObjets(string codeOffre, string version, string type, string codeFormule, string codeOption, ModeConsultation modeNavig) {
            var param = new EacParameter[5];
            param[0] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[0].Value = type;
            param[1] = new EacParameter("codeFor", DbType.Int32);
            param[1].Value = Convert.ToInt32(codeFormule);
            param[2] = new EacParameter("codeOpt", DbType.Int32);
            param[2].Value = Convert.ToInt32(codeOption);
            param[3] = new EacParameter("codeAffaire", DbType.AnsiStringFixedLength);
            param[3].Value = codeOffre.PadLeft(9, ' ');
            param[4] = new EacParameter("version", DbType.Int32);
            param[4].Value = Convert.ToInt32(version);

            string sql = string.Format(@"SELECT JGOBJ CODEID1, JGVDA * 100000000 + JGVDM * 1000000 + JGVDJ * 10000 + JGVDH DATEDEBRETURNCOL,
                            KDFKDEID ID, KDFGAN STRRETURNCOL
                        FROM {0}
                            LEFT JOIN {1} ON KDFTYP = :type AND KDFIPB = JGIPB AND KDFALX = JGALX AND KDFFOR = :codeFor AND KDFOPT = :codeOpt AND KDFRSQ = JGRSQ AND KDFOBJ = JGOBJ
                        WHERE JGIPB = :codeAffaire AND JGALX = :version",
                        CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"), CommonRepository.GetPrefixeHisto(modeNavig, "KPGARAP"));

            var resultObjPortee = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            return resultObjPortee;
        }

        /// <summary>
        /// Récupération des infos du risque de lka formule
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeOption"></param>
        /// <param name="modeNavig"></param>
        /// <param name="dDebRsq"></param>
        /// <param name="param"></param>
        private static DateTime? GetInfosRisque(string codeOffre, string version, string type, string codeFormule, string codeOption, ModeConsultation modeNavig) {
            DateTime? dDebRsq = null;
            var paramArray = new EacParameter[5];
            paramArray[0] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramArray[0].Value = type;
            paramArray[1] = new EacParameter("codeAffaire", DbType.AnsiStringFixedLength);
            paramArray[1].Value = codeOffre.PadLeft(9, ' ');
            paramArray[2] = new EacParameter("version", DbType.Int32);
            paramArray[2].Value = Convert.ToInt32(version);
            paramArray[3] = new EacParameter("codeFor", DbType.Int32);
            paramArray[3].Value = Convert.ToInt32(codeFormule);
            paramArray[4] = new EacParameter("codeOpt", DbType.Int32);
            paramArray[4].Value = Convert.ToInt32(codeOption);

            string sqlDdebRsq = string.Format(@"SELECT IFNULL(MIN(JEVDA * 100000000 + JEVDM * 1000000 + JEVDJ * 10000 + JEVDH), 0) DATEDEBRETURNCOL
                                                FROM {0} 
                                                INNER JOIN {1}  ON JEIPB = KDDIPB AND JEALX = KDDALX AND JERSQ = KDDRSQ 
                                                WHERE KDDTYP = :type AND KDDIPB = :codeAffaire AND KDDALX = :version AND KDDFOR = :codeFor AND KDDOPT = :codeOpt",
            CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTAP"), CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"));
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlDdebRsq, paramArray);
            if (result != null && result.Any()) {
                dDebRsq = AlbConvert.ConvertIntToDateHour(result.FirstOrDefault().DateDebReturnCol);
            }
            return dDebRsq;
        }

        /// <summary>
        /// Recuperation des Informations Base/Avenant
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <param name="modeNavig"></param>
        /// <param name="isModeAvt"></param>
        /// <param name="dateEffetAvt"></param>
        /// <param name="dateEffet"></param>
        /// <param name="sql"></param>
        private static (bool isModeAvt, DateTime? dateEffetAvt, DateTime? dateEffet) GetInfosBaseAvn(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig) {
            bool isModeAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0";
            DateTime? dateEffetAvt = null;
            DateTime? dateEffet = null;
            var isModeHisto = modeNavig == ModeConsultation.Historique;
            string sql = $@"SELECT PBAVA * 10000 + PBAVM * 100 + PBAVJ INT64RETURNCOL,
                                               IFNULL(PBEFA * 100000000 + PBEFM * 1000000 + PBEFJ * 10000 + PBEFH, 0) DATEDEBRETURNCOL  
                                            FROM {CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE")} WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type ";
            if (isModeHisto) {
                sql += "AND PBAVN =  :codeAvn";
            }
            var paramValues = new { codeOffre = codeOffre.ToIPB(), version, type, codeAvn = (!string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) };
            var paramDate = MakeParamsDynamic(sql, paramValues, true);

            var resultDate = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramDate);
            if (resultDate != null & resultDate.Any()) {
                dateEffetAvt = AlbConvert.ConvertIntToDate(Convert.ToInt32(resultDate.FirstOrDefault().Int64ReturnCol));
                dateEffet = AlbConvert.ConvertIntToDateHour(resultDate.FirstOrDefault().DateDebReturnCol);
            }
            return (isModeAvt, dateEffetAvt, dateEffet);
        }

        private static EnsembleGarantieDto BuildLineEnsembleGarantie(bool isModeAvt, DateTime? dateEffetAvt, DateTime? dateEffet, DateTime? dDebRsq, List<DtoCommon> resultObjPortee, List<EnsembleGarantieDto> model, List<EnsembleGarantiePlatDto> modelPlat, EnsembleGarantiePlatDto garantie, bool allowGareat) {
            EnsembleGarantieDto garan = new EnsembleGarantieDto {
                Id = garantie.IdLigne2.ToString(),
                IdSequence = garantie.IdSequence.ToString(),
                Code = !string.IsNullOrEmpty(garantie.Code) ? garantie.Code.ToString() : string.Empty,
                Description = !string.IsNullOrEmpty(garantie.Description) ? garantie.Description.Trim() : string.Empty,
                Couleur1 = !string.IsNullOrEmpty(garantie.Couleur1) ? garantie.Couleur1.Trim() : string.Empty,
                Couleur2 = !string.IsNullOrEmpty(garantie.Couleur2) ? garantie.Couleur2.Trim() : string.Empty,
                Niveau = garantie.Niveau.ToString(),
                Pere = garantie.Pere.ToString(),
                Sequence = garantie.IdSequence.ToString(),
                Origine = garantie.Origine.ToString(),
                CodeBloc = garantie.CodeBloc.ToString(),
                CVolet = garantie.CVolet.ToString(),
                LVolet = !string.IsNullOrEmpty(garantie.LVolet) ? garantie.LVolet : string.Empty,
                CBloc = garantie.CBloc.ToString(),
                LBloc = !string.IsNullOrEmpty(garantie.LBloc) ? garantie.LBloc : string.Empty,
                ReadOnly = true,
                TriVolet = garantie.TriVolet,
                TriBloc = garantie.TriBloc,
                TriDate = garantie.TriDate,
                TriGar = garantie.TriGar,
                IsAttentatGareat = garantie.AttentatGareat.AsBoolean() ?? false,
                IsGarantieSortie = isModeAvt ? FormuleRepository.IsGaranSortie(garantie.IdLigne2.ToString(), dateEffetAvt, resultObjPortee, dDebRsq, dateEffet, Convert.ToInt32(garantie.DateFin), Convert.ToInt32(garantie.HeureFin), Convert.ToInt32(garantie.Duree), garantie.UnitDuree) : false
            };

            if (!garan.IsGarantieSortie && garantie.Pere > 0) {
                var garanPere = model.Find(el => el.IdSequence == garantie.Pere.ToString());
                if (garanPere != null) {
                    garan.IsGarantieSortie = garanPere.IsGarantieSortie;
                }
            }

            LigneGarantieDto ligne = new LigneGarantieDto();
            var lstTarif = modelPlat.FindAll(el => el.IdSequence == garantie.IdSequence && el.Code == garantie.Code);
            string oui = Booleen.Oui.AsCode();
            foreach (var tarif in lstTarif) {
                bool isReadonly =
                    tarif.FranchiseLectureSeule.Trim() != oui
                    && tarif.LciLectureSeule.Trim() != oui
                    && tarif.AssietteLectureSeule.Trim() != oui
                    && tarif.PrimeLectureSeule.Trim() != oui;

                ligne = new LigneGarantieDto {
                    Code = string.Format("{0}_{1}", tarif.IdLigne1, tarif.IdLigne2),
                    NumeroTarif = tarif.NumeroTarif.ToString(),
                    LCIValeur = tarif.LciValeur == 0 ? "" : tarif.LciValeur.ToString("N", AlbConvert.AppCulture),
                    LCIUnite = tarif.LciUnite.Trim(),
                    LCIType = tarif.LciType.Trim(),
                    LCILectureSeule = tarif.LciLectureSeule.Trim(),
                    LCIObligatoire = tarif.LciObligatoire.Trim(),
                    LienLCIComplexe = string.Format("{0}¤{1}", tarif.LienLCI, tarif.CodeComplexeLCI.Trim()),
                    LibLCIComplexe = tarif.LibLCIComplexe,
                    FranchiseValeur = tarif.FranchiseValeur == 0 ? "" : tarif.FranchiseValeur.ToString("N", AlbConvert.AppCulture),
                    FranchiseUnite = tarif.FranchiseUnite.Trim(),
                    FranchiseType = tarif.FranchiseType.Trim(),
                    FranchiseLectureSeule = tarif.FranchiseLectureSeule.Trim(),
                    FranchiseObligatoire = tarif.FranchiseObligatoire.Trim(),
                    LienFRHComplexe = string.Format("{0}¤{1}", tarif.LienFRH, tarif.CodeComplexeFRH.Trim()),
                    LibFRHComplexe = tarif.LibFRHComplexe,
                    AssietteValeur = tarif.AssietteValeur == 0 ? "" : tarif.AssietteValeur.ToString("N", AlbConvert.AppCulture),
                    AssietteUnite = tarif.AssietteValeur == 0 ? "" : tarif.AssietteUnite.Trim(),
                    AssietteType = tarif.AssietteValeur == 0 ? "" : tarif.AssietteType.Trim(),
                    AssietteLectureSeule = tarif.AssietteAlimAuto == "A" || tarif.AssietteAlimAuto == "B" || tarif.ReportInven == oui ? Booleen.Non.AsCode() : tarif.AssietteLectureSeule.Trim(),
                    AssietteObligatoire = tarif.AssietteObligatoire.Trim(),
                    TauxForfaitHTValeurOrigine = tarif.PrimeValeurOrigine == 0 ? "" : tarif.PrimeValeurOrigine.ToString($"N4", AlbConvert.AppCulture),
                    TauxForfaitHTValeur = tarif.PrimeValeur == 0 ? "" : tarif.PrimeValeur.ToString($"N4", AlbConvert.AppCulture),
                    TauxForfaitHTUnite = tarif.PrimeValeur == 0 ? "" : tarif.PrimeUnite.Trim(),
                    TauxForfaitHTMinimum = tarif.PrimeMini == 0 ? "" : tarif.PrimeMini.ToString("N", AlbConvert.AppCulture),
                    TauxForfaitHTLectureSeule = tarif.PrimeLectureSeule.Trim(),
                    TauxForfaitHTObligatoire = tarif.PrimeObligatoire.Trim(),
                    TauxPrimeAlim = tarif.AssietteAlimAuto.Trim() == "B" || tarif.AssietteAlimAuto.Trim() == "C",
                    Niveau = tarif.Niveau.ToString(),
                    CVolet = tarif.CVolet.ToString(),
                    CBloc = tarif.CBloc.ToString(),
                    ReadOnly = isReadonly
                };

                if (tarif.Code == Garantie.CodeGareatAttent && allowGareat) {
                    ligne.PrimeValeur = new Valeurs {
                        ValeurActualise = decimal.TryParse(ligne.TauxForfaitHTValeur, out var d) && d > decimal.Zero ? d : 0,
                        ValeurOrigine = decimal.TryParse(ligne.TauxForfaitHTValeurOrigine, out d) && d > decimal.Zero ? d : 0
                    };
                }
                garan.LstLigneGarantie.Add(ligne);

                if (!isReadonly && garan.ReadOnly) {
                    garan.ReadOnly = false;
                }
            }

            return garan;
        }

        private static void MapConditionComplexe(ConditionComplexeDto result, ConditionComplexeLineDto res) {
            result.Id = "";
        }

        private static List<GarantieVoletBloc> GetListVoletBlocGarantie(string codeOffre, string version, string type, string codeFormule, string codeOption, ModeConsultation modeNavig, bool isReadOnly) {
            string table = string.Empty;

            if (modeNavig == ModeConsultation.Historique) {
                table = "HPOPTD";
            }
            else {
                table = "KPOPTD";
            }
            string sql = string.Format(@"SELECT KDCID CODE, KDCTENG TYPE, KDCKAKID CODEVOLET, KDCKAEID CODEBLOC, KDCKARID CODEMODELE
                    FROM {0}
                    WHERE KDCIPB = :KDCIPB AND KDCALX = :KDCALX AND KDCTYP = :KDCTYP AND KDCFOR = :KDCFOR AND KDCOPT = :KDCOPT AND KDCFLAG = :KDCFLAG", table);

            EacParameter[] param = new EacParameter[6];
            param[0] = new EacParameter("KDCIPB", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("KDCALX", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("KDCTYP", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("KDCFOR", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeFormule) ? Convert.ToInt32(codeFormule) : 0;
            param[4] = new EacParameter("KDCOPT", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codeOption) ? Convert.ToInt32(codeOption) : 0;
            param[5] = new EacParameter("KDCFLAG", DbType.Int32);
            param[5].Value = 1;

            return DbBase.Settings.ExecuteList<GarantieVoletBloc>(CommandType.Text, sql, param);
        }

        private static bool GetUtiliseEXPCOMP(int? code, string argType) {
            bool toReturn = false;

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("code", DbType.Int32);
            param[0].Value = code.HasValue ? code.Value : 0;

            if (argType == AlbConstantesMetiers.ExpressionComplexe.LCI.ToString()) {
                if (!toReturn)
                    toReturn = CommonRepository.ExistRowParam(@"SELECT COUNT(*) NBLIGN FROM KPGARTAW WHERE KDGKDIID = :code", param);
                if (!toReturn)
                    toReturn = CommonRepository.ExistRowParam(@"SELECT COUNT(*) NBLIGN FROM KPGARTAR WHERE KDGKDIID = :code", param);
                if (!toReturn)
                    toReturn = CommonRepository.ExistRowParam(@"SELECT COUNT(*) NBLIGN FROM KPENT WHERE KAAKDIID = :code", param);
                if (!toReturn)
                    toReturn = CommonRepository.ExistRowParam(@"SELECT COUNT(*) NBLIGN FROM KPRSQ WHERE KABKDIID = :code", param);
            }
            if (argType == AlbConstantesMetiers.ExpressionComplexe.Franchise.ToString()) {
                if (!toReturn)
                    toReturn = CommonRepository.ExistRowParam(@"SELECT COUNT(*) NBLIGN FROM KPGARTAW WHERE KDGKDKID = :code", param);
                if (!toReturn)
                    toReturn = CommonRepository.ExistRowParam(@"SELECT COUNT(*) NBLIGN FROM KPGARTAR WHERE KDGKDKID = :code", param);
                if (!toReturn)
                    toReturn = CommonRepository.ExistRowParam(@"SELECT COUNT(*) NBLIGN FROM KPENT WHERE KAAKDKID = :code", param);
                if (!toReturn)
                    toReturn = CommonRepository.ExistRowParam(@"SELECT COUNT(*) NBLIGN FROM KPRSQ WHERE KABKDKID = :code", param);
            }

            return toReturn;
        }

        private static bool NbExpressions(string argType, string argTypeAppel, string argIdExpression, string argIdCondition) {
            bool autorisationSuppression = false;
            string sql = string.Empty;

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("idExpr", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(argIdExpression) ? Convert.ToInt32(argIdExpression) : 0;

            if (string.IsNullOrEmpty(argTypeAppel)) {
                if (argType == AlbConstantesMetiers.ExpressionComplexe.LCI.ToString())
                    sql = @"SELECT KDGKDEID ID FROM KPGARTAW WHERE KDGKDIID = :idExpr";
                else
                    sql = "SELECT KDGKDEID ID FROM KPGARTAW WHERE KDGKDKID = :idExpr";
            }
            else {
                if (argTypeAppel == AlbConstantesMetiers.TypeAppel.Generale.ToString()) {
                    if (argType == AlbConstantesMetiers.ExpressionComplexe.LCI.ToString())
                        sql = "SELECT * FROM KPENT WHERE KAAKDIID = :idExpr";
                    else if (argType == AlbConstantesMetiers.ExpressionComplexe.Franchise.ToString())
                        sql = "SELECT * FROM KPENT WHERE KAAKDKID = :idExpr";
                }
                else if (argTypeAppel == AlbConstantesMetiers.TypeAppel.Risque.ToString()) {
                    if (argType == AlbConstantesMetiers.ExpressionComplexe.LCI.ToString())
                        sql = "SELECT * FROM KPRSQ WHERE KABKDIID = :idExpr";
                    else if (argType == AlbConstantesMetiers.ExpressionComplexe.Franchise.ToString())
                        sql = "SELECT * FROM KPRSQ WHERE KABKDKID = :idExpr";
                }
            }

            var result = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);
            if (result != null && result.Count == 0)
                autorisationSuppression = true;
            if (result != null && result.Count == 1) {
                foreach (var item in result) {
                    if (!string.IsNullOrEmpty(item.Id.ToString()) && item.Id.ToString().Trim() == argIdCondition) {
                        autorisationSuppression = true;
                    }
                }
            }
            return autorisationSuppression;
        }

        private static void EnregistrementLCI(string argTypeOffre, string argCodeOffre, string argVersion, ConditionComplexeDto argExpComp, int? argIdExpression, string argLibelle, string argDescriptif) {
            long chronoDesi = 0;
            string sql = string.Empty;

            EacParameter[] paramSel = new EacParameter[1];
            paramSel[0] = new EacParameter("idExpr", DbType.Int32);
            paramSel[0].Value = argIdExpression.HasValue ? argIdExpression.Value : 0;

            sql = string.Format(@"SELECT KDIDESI ID FROM KPEXPLCI WHERE KDIID = :idExpr");
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramSel);
            if (result != null && result.Any() && result.FirstOrDefault().Id > 0) {
                chronoDesi = result.FirstOrDefault().Id;
                EacParameter[] paramUpd = new EacParameter[2];
                paramUpd[0] = new EacParameter("desi", DbType.AnsiStringFixedLength);
                paramUpd[0].Value = argDescriptif;
                paramUpd[1] = new EacParameter("chrono", DbType.Int64);
                paramUpd[1].Value = chronoDesi;

                sql = @"UPDATE KPDESI SET KADDESI = :desi WHERE KADCHR = :chrono";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramUpd);
            }
            else {
                chronoDesi = CommonRepository.GetAS400Id("KADCHR");
                EacParameter[] paramIns = new EacParameter[8];
                paramIns[0] = new EacParameter("chrono", DbType.Int64);
                paramIns[0].Value = chronoDesi;
                paramIns[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
                paramIns[1].Value = argTypeOffre;
                paramIns[2] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                paramIns[2].Value = argCodeOffre.PadLeft(9, ' ');
                paramIns[3] = new EacParameter("version", DbType.Int32);
                paramIns[3].Value = !string.IsNullOrEmpty(argVersion) ? Convert.ToInt32(argVersion) : 0;
                paramIns[4] = new EacParameter("perimetre", DbType.AnsiStringFixedLength);
                paramIns[4].Value = string.Empty;
                paramIns[5] = new EacParameter("codeRsq", DbType.Int32);
                paramIns[5].Value = 0;
                paramIns[6] = new EacParameter("codeObj", DbType.Int32);
                paramIns[6].Value = 0;
                paramIns[7] = new EacParameter("desi", DbType.AnsiStringFixedLength);
                paramIns[7].Value = argDescriptif;

                sql = @"INSERT INTO KPDESI (KADCHR, KADTYP, KADIPB, KADALX, KADPERI, KADRSQ, KADOBJ, KADDESI)
                                        VALUES
                                                (:chrono, :type, :codeOffre, :version, :perimetre, :codeRsq, :codeObj, :desi)";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramIns);
            }

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("desi", DbType.AnsiStringFixedLength);
            param[0].Value = argLibelle;
            param[1] = new EacParameter("chrono", DbType.Int64);
            param[1].Value = chronoDesi;
            param[2] = new EacParameter("idExpr", DbType.Int32);
            param[2].Value = argIdExpression.HasValue ? argIdExpression.Value : 0;
            sql = @"UPDATE KPEXPLCI SET KDIDESC = :desi, KDIDESI = :chrono WHERE KDIID = :idExpr";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (argExpComp == null) {
                return;
            }
            foreach (LigneGarantieDto Ligne in argExpComp.LstLigneGarantie) {
                if (!string.IsNullOrEmpty(Ligne.UniteLCINew))
                    Ligne.LCIUnite = Ligne.UniteLCINew;
                if (!string.IsNullOrEmpty(Ligne.TypeLCINew))
                    Ligne.LCIType = Ligne.TypeLCINew;
                if (!string.IsNullOrEmpty(Ligne.UniteConcurrence))
                    Ligne.ConcurrenceUnite = Ligne.UniteConcurrence;
                if (!string.IsNullOrEmpty(Ligne.TypeConcurrence))
                    Ligne.ConcurrenceType = Ligne.TypeConcurrence;


                EacParameter[] paramProc = new EacParameter[8];
                paramProc[0] = new EacParameter("P_ID", DbType.Int32);
                paramProc[0].Value = Ligne.Id != null ? Ligne.Id : 0; ;
                paramProc[1] = new EacParameter("P_EXPRESSION", DbType.Int32);
                paramProc[1].Value = argIdExpression != null ? argIdExpression : 0;
                paramProc[2] = new EacParameter("P_LCIVALEUR", DbType.Decimal);
                paramProc[2].Value = !string.IsNullOrEmpty(Ligne.LCIValeur) ? Convert.ToDecimal(Ligne.LCIValeur) : 0;
                paramProc[3] = new EacParameter("P_LCIUNITE", DbType.AnsiStringFixedLength);
                paramProc[3].Value = !string.IsNullOrEmpty(Ligne.LCIUnite) ? Ligne.LCIUnite : string.Empty;
                paramProc[4] = new EacParameter("P_LCITYPE", DbType.AnsiStringFixedLength);
                paramProc[4].Value = !string.IsNullOrEmpty(Ligne.LCIType) ? Ligne.LCIType : string.Empty;
                paramProc[5] = new EacParameter("P_CONCURVALEUR", DbType.Decimal);
                paramProc[5].Value = !string.IsNullOrEmpty(Ligne.ConcurrenceValeur) ? Convert.ToDecimal(Ligne.ConcurrenceValeur) : 0;
                paramProc[6] = new EacParameter("P_CONCURUNITE", DbType.AnsiStringFixedLength);
                paramProc[6].Value = !string.IsNullOrEmpty(Ligne.ConcurrenceUnite) ? Ligne.ConcurrenceUnite : string.Empty;
                paramProc[7] = new EacParameter("P_CONCURTYPE", DbType.AnsiStringFixedLength);
                paramProc[7].Value = !string.IsNullOrEmpty(Ligne.ConcurrenceType) ? Ligne.ConcurrenceType : string.Empty;

                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SCPXLCI", paramProc);
            }
        }

        private static void EnregistrementFranchise(string argTypeOffre, string argCodeOffre, string argVersion, ConditionComplexeDto argExpComp, int? argIdExpression, string argLibelle, string argDescriptif) {
            long chronoDesi = 0;
            string sql = string.Empty;
            EacParameter[] paramSel = new EacParameter[1];
            paramSel[0] = new EacParameter("idExpr", DbType.Int32);
            paramSel[0].Value = argIdExpression.HasValue ? argIdExpression.Value : 0;

            sql = @"SELECT KDKDESI ID FROM KPEXPFRH WHERE KDKID = :idExpr";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramSel);
            if (result != null && result.Any() && result.FirstOrDefault().Id > 0) {
                chronoDesi = result.FirstOrDefault().Id;
                EacParameter[] paramUpd = new EacParameter[2];
                paramUpd[0] = new EacParameter("desi", DbType.AnsiStringFixedLength);
                paramUpd[0].Value = argDescriptif;
                paramUpd[1] = new EacParameter("chrono", DbType.Int64);
                paramUpd[1].Value = chronoDesi;
                sql = @"UPDATE KPDESI SET KADDESI = :desi WHERE KADCHR = :chrono";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramUpd);
            }
            else {
                chronoDesi = CommonRepository.GetAS400Id("KADCHR");
                EacParameter[] paramIns = new EacParameter[8];
                paramIns[0] = new EacParameter("chrono", DbType.Int64);
                paramIns[0].Value = chronoDesi;
                paramIns[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
                paramIns[1].Value = argTypeOffre;
                paramIns[2] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                paramIns[2].Value = argCodeOffre.PadLeft(9, ' ');
                paramIns[3] = new EacParameter("version", DbType.Int32);
                paramIns[3].Value = !string.IsNullOrEmpty(argVersion) ? Convert.ToInt32(argVersion) : 0;
                paramIns[4] = new EacParameter("perimetre", DbType.AnsiStringFixedLength);
                paramIns[4].Value = string.Empty;
                paramIns[5] = new EacParameter("codeRsq", DbType.Int32);
                paramIns[5].Value = 0;
                paramIns[6] = new EacParameter("codeObj", DbType.Int32);
                paramIns[6].Value = 0;
                paramIns[7] = new EacParameter("desi", DbType.AnsiStringFixedLength);
                paramIns[7].Value = argDescriptif;
                sql = @"INSERT INTO KPDESI (KADCHR, KADTYP, KADIPB, KADALX, KADPERI, KADRSQ, KADOBJ, KADDESI)
                            VALUES
                        (:chrono, :type, :codeOffre, :version, :perimetre, :codeRsq, :codeObj, :desi)";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramIns);
            }

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("desi", DbType.AnsiStringFixedLength);
            param[0].Value = argLibelle;
            param[1] = new EacParameter("chrono", DbType.Int64);
            param[1].Value = chronoDesi;
            param[2] = new EacParameter("idExpr", DbType.Int32);
            param[2].Value = argIdExpression.HasValue ? argIdExpression.Value : 0;
            sql = @"UPDATE KPEXPFRH SET KDKDESC = :desi, KDKDESI = :chrono WHERE KDKID = :idExpr";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            if (argExpComp == null) {
                return;
            }
            foreach (LigneGarantieDto Ligne in argExpComp.LstLigneGarantie) {
                if (!string.IsNullOrEmpty(Ligne.UniteFranchiseNew))
                    Ligne.FranchiseUnite = Ligne.UniteFranchiseNew;
                if (!string.IsNullOrEmpty(Ligne.TypeFranchiseNew))
                    Ligne.FranchiseType = Ligne.TypeFranchiseNew;

                EacParameter[] paramProc = new EacParameter[9];
                paramProc[0] = new EacParameter("P_ID", DbType.Int32);
                paramProc[0].Value = Ligne.Id != null ? Ligne.Id : 0;
                paramProc[1] = new EacParameter("P_EXPRESSION", DbType.Int32);
                paramProc[1].Value = argIdExpression != null ? argIdExpression : 0;
                paramProc[2] = new EacParameter("P_FRHVALEUR", DbType.Decimal);
                paramProc[2].Value = !string.IsNullOrEmpty(Ligne.FranchiseValeur) ? Convert.ToDecimal(Ligne.FranchiseValeur) : 0;
                paramProc[3] = new EacParameter("P_FRHUNITE", DbType.AnsiStringFixedLength);
                paramProc[3].Value = !string.IsNullOrEmpty(Ligne.FranchiseUnite) ? Ligne.FranchiseUnite : string.Empty;
                paramProc[4] = new EacParameter("P_FRHTYPE", DbType.AnsiStringFixedLength);
                paramProc[4].Value = !string.IsNullOrEmpty(Ligne.FranchiseType) ? Ligne.FranchiseType : string.Empty;
                paramProc[5] = new EacParameter("P_FRHMINI", DbType.Decimal);
                paramProc[5].Value = !string.IsNullOrEmpty(Ligne.FranchiseMini) ? Convert.ToDecimal(Ligne.FranchiseMini) : 0;
                paramProc[6] = new EacParameter("P_FRHMAXI", DbType.Decimal);
                paramProc[6].Value = !string.IsNullOrEmpty(Ligne.FranchiseMaxi) ? Convert.ToDecimal(Ligne.FranchiseMaxi) : 0;
                paramProc[7] = new EacParameter("P_FRHDATEDEB", DbType.Int32);
                paramProc[7].Value = Ligne.FranchiseDebut != null ? AlbConvert.ConvertDateToInt(Ligne.FranchiseDebut) : 0;
                paramProc[8] = new EacParameter("P_FRHDATEFIN", DbType.Int32);
                paramProc[8].Value = Ligne.FranchiseFin != null ? AlbConvert.ConvertDateToInt(Ligne.FranchiseFin) : 0;

                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SCPXFRH", paramProc);
            }
        }

        private static int CreationExpressionLCI(string argType, string argCodeOffre, string argVersion, string argLibelle, string argDescriptif) {
            string sql = string.Empty;
            int id = CommonRepository.GetAS400Id("KADCHR");

            EacParameter[] paramIns = new EacParameter[5];
            paramIns[0] = new EacParameter("idDesi", DbType.Int32);
            paramIns[0].Value = id;
            paramIns[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramIns[1].Value = argType;
            paramIns[2] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramIns[2].Value = argCodeOffre.PadLeft(9, ' ');
            paramIns[3] = new EacParameter("version", DbType.Int32);
            paramIns[3].Value = !string.IsNullOrEmpty(argVersion) ? Convert.ToInt32(argVersion) : 0;
            paramIns[4] = new EacParameter("descr", DbType.AnsiStringFixedLength);
            paramIns[4].Value = argDescriptif;

            sql = @"INSERT INTO KPDESI (KADCHR, KADTYP, KADIPB, KADALX, KADDESI) 
                                values(:idDesi, :type, :codeOffre, :version, :descr)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramIns);

            string code = "";
            EacParameter[] paramSel = new EacParameter[5];
            paramSel[0] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramSel[0].Value = argType;
            paramSel[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramSel[1].Value = argCodeOffre.Trim().PadLeft(9, ' ');
            paramSel[2] = new EacParameter("version", DbType.Int32);
            paramSel[2].Value = !string.IsNullOrEmpty(argVersion) ? Convert.ToInt32(argVersion) : 0;
            paramSel[3] = new EacParameter("codeExpr1", DbType.AnsiStringFixedLength);
            paramSel[3].Value = "EXP";
            paramSel[4] = new EacParameter("codeExpr2", DbType.AnsiStringFixedLength);
            paramSel[4].Value = "LL";

            sql = @"SELECT IFNULL(MAX(CAST(SUBSTRING(KDILCE,3) AS INT))+1, 1) ID 
                        FROM KPEXPLCI
                        WHERE KDITYP = :type AND KDIIPB = :codeOffre AND KDIALX = :version
                            AND SUBSTRING(KDILCE, 1, 3) <> :codeExpr1 AND SUBSTRING(KDILCE, 1, 2) = :codeExpr2";
            var list = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, paramSel);
            if (list != null && list.Any()) {
                foreach (var item in list)
                    code = "LL" + (item.Id.ToString().Trim()).PadLeft(3, '0');
            }

            int IdExpression = CommonRepository.GetAS400Id("KDIID");
            paramIns = new EacParameter[9];
            paramIns[0] = new EacParameter("idExpr", DbType.Int32);
            paramIns[0].Value = IdExpression;
            paramIns[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramIns[1].Value = argType;
            paramIns[2] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramIns[2].Value = argCodeOffre.PadLeft(9, ' ');
            paramIns[3] = new EacParameter("version", DbType.Int32);
            paramIns[3].Value = !string.IsNullOrEmpty(argVersion) ? Convert.ToInt32(argVersion) : 0;
            paramIns[4] = new EacParameter("codeExpr", DbType.AnsiStringFixedLength);
            paramIns[4].Value = code;
            paramIns[5] = new EacParameter("libExpr", DbType.AnsiStringFixedLength);
            paramIns[5].Value = argLibelle;
            paramIns[6] = new EacParameter("idDesi", DbType.Int32);
            paramIns[6].Value = id;
            paramIns[7] = new EacParameter("origine", DbType.AnsiStringFixedLength);
            paramIns[7].Value = "S";
            paramIns[8] = new EacParameter("modif", DbType.AnsiStringFixedLength);
            paramIns[8].Value = "O";

            sql = @"INSERT INTO KPEXPLCI (KDIID, KDITYP, KDIIPB, KDIALX, KDILCE, KDIDESC, KDIDESI, KDIORI, KDIMODI)
                               VALUES(:idExpr, :type, :codeOffre, :version, :codeExpr, :libExpr, :idDesi, :origine, :modif)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramIns);
            return IdExpression;
        }

        private static int CreationExpressionFranchise(string argType, string argCodeOffre, string argVersion, string argLibelle, string argDescriptif) {
            string sql = string.Empty;
            int id = CommonRepository.GetAS400Id("KADCHR");

            EacParameter[] paramIns = new EacParameter[5];
            paramIns[0] = new EacParameter("idDesi", DbType.Int32);
            paramIns[0].Value = id;
            paramIns[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramIns[1].Value = argType;
            paramIns[2] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramIns[2].Value = argCodeOffre.PadLeft(9, ' ');
            paramIns[3] = new EacParameter("version", DbType.Int32);
            paramIns[3].Value = !string.IsNullOrEmpty(argVersion) ? Convert.ToInt32(argVersion) : 0;
            paramIns[4] = new EacParameter("descr", DbType.AnsiStringFixedLength);
            paramIns[4].Value = argDescriptif;

            sql = @"INSERT INTO KPDESI (KADCHR, KADTYP, KADIPB, KADALX, KADDESI) 
                                VALUES(:idDesi, :type, :codeOffre, :version, :descr)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramIns);

            string code = "";
            EacParameter[] paramSel = new EacParameter[5];
            paramSel[0] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramSel[0].Value = argType;
            paramSel[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramSel[1].Value = argCodeOffre.Trim().PadLeft(9, ' ');
            paramSel[2] = new EacParameter("version", DbType.Int32);
            paramSel[2].Value = !string.IsNullOrEmpty(argVersion) ? Convert.ToInt32(argVersion) : 0;
            paramSel[3] = new EacParameter("codeExpr1", DbType.AnsiStringFixedLength);
            paramSel[3].Value = "EXP";
            paramSel[4] = new EacParameter("codeExpr2", DbType.AnsiStringFixedLength);
            paramSel[4].Value = "LF";

            sql = @"SELECT IFNULL(MAX(CAST(SUBSTRING(KDKFHE,3) AS INT))+1, 1) ID FROM KPEXPFRH
                                        WHERE KDKTYP = :type AND KDKIPB = :codeOffre AND KDKALX = :version
                                            AND SUBSTRING(KDKFHE, 1, 3) <> :codeExpr1 AND SUBSTRING(KDKFHE, 1, 2) = :codeExpr2";
            var list = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, paramSel);
            if (list != null && list.Any()) {
                foreach (var item in list)
                    code = "LF" + (item.Id.ToString().Trim()).PadLeft(3, '0');
            }

            int IdExpression = CommonRepository.GetAS400Id("KDKID");
            paramIns = new EacParameter[9];
            paramIns[0] = new EacParameter("idExpr", DbType.Int32);
            paramIns[0].Value = IdExpression;
            paramIns[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramIns[1].Value = argType;
            paramIns[2] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramIns[2].Value = argCodeOffre.PadLeft(9, ' ');
            paramIns[3] = new EacParameter("version", DbType.Int32);
            paramIns[3].Value = !string.IsNullOrEmpty(argVersion) ? Convert.ToInt32(argVersion) : 0;
            paramIns[4] = new EacParameter("codeExpr", DbType.AnsiStringFixedLength);
            paramIns[4].Value = code;
            paramIns[5] = new EacParameter("libExpr", DbType.AnsiStringFixedLength);
            paramIns[5].Value = argLibelle;
            paramIns[6] = new EacParameter("idDesi", DbType.Int32);
            paramIns[6].Value = id;
            paramIns[7] = new EacParameter("origine", DbType.AnsiStringFixedLength);
            paramIns[7].Value = "S";
            paramIns[8] = new EacParameter("modif", DbType.AnsiStringFixedLength);
            paramIns[8].Value = "O";

            sql = @"INSERT INTO KPEXPFRH (KDKID, KDKTYP, KDKIPB, KDKALX, KDKFHE, KDKDESC, KDKDESI, KDKORI, KDKMODI)
                               VALUES(:idExpr, :type, :codeOffre, :version, :codeExpr, :libExpr, :idDesi, :origine, :modif)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramIns);
            return IdExpression;
        }

        #endregion

        #endregion

        #region LCIFranchise

        #region Méthodes publiques

        public static void EnregistrementExpCompGeneraleRisque(string codeOffre, string version, string typeOffre, string codeAvn, string codeFormule, string codeOption, string codeRisque, string codeExpression, string unite, AlbConstantesMetiers.ExpressionComplexe typeVue, AlbConstantesMetiers.TypeAppel typeAppel, ModeConsultation modeNavig) {
            if (string.IsNullOrEmpty(codeRisque)) {
                string rsqobjApplique = ConditionRepository.ObtenirRisqueObjetFormule(codeOffre, version, typeOffre, codeAvn, codeFormule, codeOption, modeNavig);
                codeRisque = "0";
                if (!string.IsNullOrEmpty(rsqobjApplique) && rsqobjApplique.Split(';').Length > 0)
                    codeRisque = rsqobjApplique.Split(';')[0];
            }

            EacParameter[] param = new EacParameter[8];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = typeOffre;
            param[3] = new EacParameter("P_CODERISQUE", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeRisque);
            param[4] = new EacParameter("P_CODEEXPR", DbType.Int32);
            param[4].Value = Convert.ToInt32(codeExpression);
            param[5] = new EacParameter("P_TYPEAPPEL", DbType.AnsiStringFixedLength);
            param[5].Value = typeAppel.ToString();
            param[6] = new EacParameter("P_TYPEVUE", DbType.AnsiStringFixedLength);
            param[6].Value = typeVue.ToString();
            param[7] = new EacParameter("P_UNITE", DbType.AnsiStringFixedLength);
            param[7].Value = unite;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEXPCOMPGENRSQ", param);

        }
        public static void InfoSpecRisqueLCIFranchiseSet(string codeOffre, string version, string typeOffre, string codeRisque
         , string argValeurLCIRisque, string argUniteLCIRisque, string argTypeLCIRisque, string argLienCpxLCIRisque
         , string argValeurFranchiseRisque, string argUniteFranchiseRisque, string argTypeFranchiseRisque, string argLienCpxFranchiseRisque) {
            EacParameter[] param = new EacParameter[12];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = typeOffre;
            param[3] = new EacParameter("P_LCIValeurRisque", DbType.Decimal);
            param[3].Value = Convert.ToDecimal((argValeurLCIRisque == "" ? "0" : argValeurLCIRisque));
            param[4] = new EacParameter("P_LCIUniteRisque", DbType.AnsiStringFixedLength);
            param[4].Value = argUniteLCIRisque == null ? "" : argUniteLCIRisque;
            param[5] = new EacParameter("P_LCITypeRisque", DbType.AnsiStringFixedLength);
            param[5].Value = argTypeLCIRisque == null ? "" : argTypeLCIRisque;
            param[6] = new EacParameter("P_FRHValeurRisque", DbType.Decimal);
            param[6].Value = Convert.ToDecimal((argValeurFranchiseRisque == "" ? "0" : argValeurFranchiseRisque));
            param[7] = new EacParameter("P_FRHUniteRisque", DbType.AnsiStringFixedLength);
            param[7].Value = argUniteFranchiseRisque == null ? "" : argUniteFranchiseRisque;
            param[8] = new EacParameter("P_FRHTypeRisque", DbType.AnsiStringFixedLength);
            param[8].Value = argTypeFranchiseRisque == null ? "" : argTypeFranchiseRisque;
            param[9] = new EacParameter("P_CODERISQUE", DbType.AnsiStringFixedLength);
            param[9].Value = codeRisque;
            param[10] = new EacParameter("P_LienCpxLCIRisque", DbType.Int64);
            param[10].Value = Convert.ToInt64(string.IsNullOrEmpty(argLienCpxLCIRisque) ? "0" : argLienCpxLCIRisque);
            param[11] = new EacParameter("P_LienCpxFranchiseRisque", DbType.Int64);
            param[11].Value = Convert.ToInt64(string.IsNullOrEmpty(argLienCpxFranchiseRisque) ? "0" : argLienCpxFranchiseRisque);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVELCIFRHRSQ", param);
        }

        #endregion

        #region Méthodes publiques

        private static LCIFranchiseDto GetLCIFranchiseGenerale(string arg_NumOffre, string arg_Version, string arg_TypeOffre, string codeAvn, AlbConstantesMetiers.ExpressionComplexe typeVue, ModeConsultation modeNavig) {
            string sql = string.Empty;
            var param = new List<EacParameter>() {
                new EacParameter("codeOffre", arg_NumOffre.Trim().PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(arg_Version)),
                new EacParameter("type", arg_TypeOffre)
            };
            if (modeNavig == ModeConsultation.Historique) {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }


            switch (typeVue) {
                case AlbConstantesMetiers.ExpressionComplexe.LCI:
                    sql = string.Format(@"SELECT KAALCIVALO VALEUR,KAALCIUNIT UNITE,KAALCIBASE TYPE,
                            KAALCIINA ISINDEXE,KAAKDIID LIENCOMPLEXE,KDILCE CODECOMPLEXE, KDIDESC LIBCOMPLEXE 
                            FROM {0}
                            LEFT JOIN {1} ON KDIID=KAAKDIID 
                            WHERE KAAIPB = :codeOffre AND KAAALX = :version AND KAATYP = :type {2}",
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPEXPLCI"),
                            modeNavig == ModeConsultation.Historique ? " AND KAAAVN = :avn" : string.Empty);
                    break;
                case AlbConstantesMetiers.ExpressionComplexe.Franchise:
                    sql = string.Format(@"SELECT KAAFRHVALO VALEUR,KAAFRHUNIT UNITE,KAAFRHBASE TYPE,
                            KAAFRHINA ISINDEXE,KAAKDKID LIENCOMPLEXE,KDKFHE CODECOMPLEXE, KDKDESC LIBCOMPLEXE 
                            FROM {0}
                            LEFT JOIN {1} ON KDKID=KAAKDKID 
                            WHERE KAAIPB = :codeOffre AND KAAALX = :version AND KAATYP = :type {2}",
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPEXPFRH"),
                            modeNavig == ModeConsultation.Historique ? " AND KAAAVN = :avn" : string.Empty);
                    break;
            }


            return DbBase.Settings.ExecuteList<LCIFranchiseDto>(CommandType.Text, sql, param).FirstOrDefault();
        }
        private static LCIFranchiseDto GetLCIFranchiseRisque(string arg_NumOffre, string arg_Version, string arg_TypeOffre, string codeAvn, string codeRisque, AlbConstantesMetiers.ExpressionComplexe typeVue, ModeConsultation modeNavig) {
            string sql = string.Empty;
            var param = new List<EacParameter> {
             new EacParameter("codeOffre", arg_NumOffre.Trim().PadLeft(9, ' ')),
             new EacParameter("version", Convert.ToInt32(arg_Version)),
             new EacParameter("type", arg_TypeOffre),
             new EacParameter("codeRsq", Convert.ToInt32(codeRisque))
             };
            if (modeNavig == ModeConsultation.Historique) {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            switch (typeVue) {
                case AlbConstantesMetiers.ExpressionComplexe.LCI:
                    sql = string.Format(@"SELECT KABLCIVALO VALEUR,KABLCIUNIT UNITE,KABLCIBASE TYPE
                            ,KABKDIID LIENCOMPLEXE,KDILCE CODECOMPLEXE, KDIDESC LIBCOMPLEXE
                            FROM {0}
                           LEFT JOIN {1} ON KDIID=KABKDIID 
                            WHERE KABIPB = :codeOffre AND KABALX = :version AND KABTYP = :type AND KABRSQ = :codeRsq {2}",
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPEXPLCI"),
                            modeNavig == ModeConsultation.Historique ? " AND KABAVN =:avn" : string.Empty);
                    break;
                case AlbConstantesMetiers.ExpressionComplexe.Franchise:
                    sql = string.Format(@"SELECT KABFRHVALO VALEUR,KABFRHUNIT UNITE,KABFRHBASE TYPE
                            ,KABKDKID LIENCOMPLEXE,KDKFHE CODECOMPLEXE, KDKDESC LIBCOMPLEXE 
                            FROM {0}
                            LEFT JOIN {1} ON KDKID=KABKDKID 
                            WHERE KABIPB = :codeOffre AND KABALX = :version AND KABTYP = :type AND KABRSQ = :codeRsq {2}",
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPEXPFRH"),
                            modeNavig == ModeConsultation.Historique ? " AND KABAVN = :avn" : string.Empty);
                    break;
            }


            return DbBase.Settings.ExecuteList<LCIFranchiseDto>(CommandType.Text, sql, param).FirstOrDefault();
        }

        #endregion
        #endregion
    }
}

