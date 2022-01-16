using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Condition;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.RefExprComplexe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess
{
    public class RefExpressionComplexeRepository
    {

        #region Méthodes publiques

        /// <summary>
        /// Charge les listes des expressions complexes
        /// </summary>
        public static ListRefExprComplexeDto LoadListesExprComplexe()
        {
            ListRefExprComplexeDto model = new ListRefExprComplexeDto();
            model.ListLCI = GetListLCIExprComplexe();
            model.ListFranchise = GetListFRHExprComplexe();

            return model;
        }

        /// <summary>
        /// Recharge la liste des expression complexes
        /// </summary>
        public static List<ParametreDto> LoadListExprComplexe(string typeExpr)
        {
            return typeExpr == "LCI" ? GetListLCIExprComplexe() : typeExpr == "Franchise" ? GetListFRHExprComplexe() : null;
        }

        /// <summary>
        /// Récupère les informations d'une expression complexe
        /// </summary>
        public static ConditionComplexeDto GetInfoExprComplexe(string typeExpr, string codeExpr)
        {
            ConditionComplexeDto toReturn = new ConditionComplexeDto();
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codeExpr", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(codeExpr) ? codeExpr : "0";

            string sql = string.Empty;

            if (typeExpr == "LCI")
            {                
                sql = @"SELECT KHGID GUIDID, KHGLCE CODEEXPR, KHGDESC LIBEXPR, KHGMODI MODIFEXPR, KDWDESI DESCREXPR
                            FROM KEXPLCI
                                LEFT JOIN KDESI ON KHGDESI = KDWID
                            WHERE KHGID = :codeExpr";
            }
            if (typeExpr == "Franchise")
            {                
                sql = @"SELECT KHEID GUIDID, KHEFHE CODEEXPR, KHEDESC LIBEXPR, KHEMODI MODIFEXPR, KDWDESI DESCREXPR
                            FROM KEXPFRH
                                LEFT JOIN KDESI ON KHEDESI = KDWID
                            WHERE KHEID = :codeExpr";
            }


            var result = DbBase.Settings.ExecuteList<ExprComplexeDetailDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                toReturn.Id = result.FirstOrDefault().GuidId.ToString();
                toReturn.Code = result.FirstOrDefault().CodeExpr;
                toReturn.Libelle = result.FirstOrDefault().LibExpr;
                toReturn.Descriptif = result.FirstOrDefault().DescrExpr;
                toReturn.Modifiable = result.FirstOrDefault().ModifExpr == "O";

                toReturn.LstLigneGarantie = GetListLigneExprComp(typeExpr, codeExpr);
            }
            else
            {
                toReturn.Code = GetNewCodeExprComp(typeExpr);
            }

            if (typeExpr == "LCI")
            {
                toReturn.UnitesNew = CommonRepository.GetParametres(string.Empty, string.Empty, "ALSPK", "UNLCI", isBO: true);
                toReturn.TypesNew = CommonRepository.GetParametres(string.Empty, string.Empty, "ALSPK", "BALCI", isBO: true);
            }
            if (typeExpr == "Franchise")
            {
                toReturn.UnitesNew = CommonRepository.GetParametres(string.Empty, string.Empty, "ALSPK", "UNFRA", isBO: true);
                toReturn.TypesNew = CommonRepository.GetParametres(string.Empty, string.Empty, "ALSPK", "BAFRA", isBO: true);
            }
            return toReturn;
        }

        /// <summary>
        /// Sauvegarde les informations d'une expression complexe
        /// </summary>
        public static Int32 SaveDetailExpr(string idExpr, string typeExpr, string codeExpr, string libExpr, bool modifExpr, string descrExpr)
        {
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("P_IDEXPR", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(idExpr) ? Convert.ToInt32(idExpr) : 0;
            param[1] = new EacParameter("P_TYPEEXPR", DbType.AnsiStringFixedLength);
            param[1].Value = typeExpr.ToUpper();
            param[2] = new EacParameter("P_CODEEXPR", DbType.AnsiStringFixedLength);
            param[2].Value = codeExpr.ToUpper();
            param[3] = new EacParameter("P_LIBEXPR", DbType.AnsiStringFixedLength);
            param[3].Value = libExpr;
            param[4] = new EacParameter("P_MODIFEXPR", DbType.AnsiStringFixedLength);
            param[4].Value = modifExpr ? "O" : "N";
            param[5] = new EacParameter("P_DESCREXPR", DbType.AnsiStringFixedLength);
            param[5].Value = descrExpr;
            param[6] = new EacParameter("P_OUTIDEXPR", DbType.Int32);
            param[6].Direction = ParameterDirection.InputOutput;
            param[6].DbType = DbType.Int32;
            param[6].Value = 0;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEEXPRCOMPLEXE", param);
            return Convert.ToInt32(param[6].Value.ToString());
        }

        /// <summary>
        /// Supprime une expression complexe
        /// </summary>
        public static void DeleteExprComp(string idExpr, string typeExpr)
        {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("P_IDEXPR", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(idExpr) ? Convert.ToInt32(idExpr) : 0;
            param[1] = new EacParameter("P_TYPEEXPR", DbType.AnsiStringFixedLength);
            param[1].Value = typeExpr.ToUpper();

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELEXPRCOMPLEXE", param);
        }

        /// <summary>
        /// Sauvegarde la ligne de détail d'expression complexe
        /// </summary>
        public static void SaveRowExprComplexe(string idExpr, string typeExpComp, string idRowExpr,
            string valExpr, string unitExpr, string typeExpr, string concuValExpr, string concuUnitExpr, string concuTypeExpr,
            string valMinFrh, string valMaxFrh, string debFrh, string finFrh)
        {
            EacParameter[] param = new EacParameter[13];
            param[0] = new EacParameter("P_IDEXPR", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(idExpr) ? Convert.ToInt32(idExpr) : 0;
            param[1] = new EacParameter("P_TYPEEXPCOMPLEXE", DbType.AnsiStringFixedLength);
            param[1].Value = typeExpComp.ToUpper();
            param[2] = new EacParameter("P_IDROWEXPR", DbType.Int32);
            param[2].Value = !string.IsNullOrEmpty(idRowExpr) ? Convert.ToInt32(idRowExpr) : 0;
            param[3] = new EacParameter("P_VALEXPR", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(valExpr) ? Convert.ToDecimal(valExpr) : 0;
            param[4] = new EacParameter("P_UNITEXPR", DbType.AnsiStringFixedLength);
            param[4].Value = unitExpr;
            param[5] = new EacParameter("P_TYPEEXPR", DbType.AnsiStringFixedLength);
            param[5].Value = typeExpr;
            param[6] = new EacParameter("P_CONCUVALEXPR", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(concuValExpr) ? Convert.ToDecimal(concuValExpr) : 0;
            param[7] = new EacParameter("P_CONCUUNITEXPR", DbType.AnsiStringFixedLength);
            param[7].Value = concuUnitExpr;
            param[8] = new EacParameter("P_CONCUTYPEEXPR", DbType.AnsiStringFixedLength);
            param[8].Value = concuTypeExpr;
            param[9] = new EacParameter("P_VALMINEXPR", DbType.Int32);
            param[9].Value = !string.IsNullOrEmpty(valMinFrh) ? Convert.ToDecimal(valMinFrh) : 0;
            param[10] = new EacParameter("P_VALMAXEXPR", DbType.Int32);
            param[10].Value = !string.IsNullOrEmpty(valMaxFrh) ? Convert.ToDecimal(valMaxFrh) : 0;
            param[11] = new EacParameter("P_DEBFRH", DbType.Int32);
            param[11].Value = !string.IsNullOrEmpty(debFrh) ? AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(debFrh)) : 0;
            param[12] = new EacParameter("P_FINFRH", DbType.Int32);
            param[12].Value = !string.IsNullOrEmpty(finFrh) ? AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(finFrh)) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEROWDETAILEXPRCOMPLEXE", param);
        }

        /// <summary>
        /// Supprime la ligne de détail d'expression complexe
        /// </summary>
        public static void DelRowExprComplexe(string idExpr, string typeExpComp, string idRowExpr)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("P_IDEXPR", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(idExpr) ? Convert.ToInt32(idExpr) : 0;
            param[1] = new EacParameter("P_TYPEEXPCOMPLEXE", DbType.AnsiStringFixedLength);
            param[1].Value = typeExpComp.ToUpper();
            param[2] = new EacParameter("P_IDROWEXPR", DbType.Int32);
            param[2].Value = !string.IsNullOrEmpty(idRowExpr) ? Convert.ToInt32(idRowExpr) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELROWDETAILEXPRCOMPLEXE", param);
        }

        /// <summary>
        /// Charge la liste des lignes de l'expression complexe
        /// </summary>
        public static ConditionComplexeDto LoadRowsExprComplexe(string typeExpr, string idExpr)
        {
            ConditionComplexeDto toReturn = new ConditionComplexeDto();
            toReturn.LstLigneGarantie = GetListLigneExprComp(typeExpr, idExpr);
            toReturn.UnitesNew = CommonRepository.GetParametres(string.Empty, string.Empty, "ALSPK", "UNLCI");
            toReturn.TypesNew = CommonRepository.GetParametres(string.Empty, string.Empty, "ALSPK", "BALCI");

            return toReturn;
        }

        /// <summary>
        /// Charge la liste des expressions 
        /// complexes du référentiel
        /// </summary>
        public static List<ConditionComplexeDto> LoadListExprComplexeReferentiel(string typeExpr, string codeExpr)
        {
            List<ConditionComplexeDto> toReturn = new List<ConditionComplexeDto>();
            string sql = string.Empty;

            switch (typeExpr)
            {
                case "LCI":
                    sql = @"SELECT KHGID GUIDID, KHGLCE CODEEXPR, KHGDESC LIBEXPR, KHGMODI MODIFEXPR FROM KEXPLCI";
                    if (!string.IsNullOrEmpty(codeExpr))
                        sql += string.Format(" WHERE UPPER(KHGLCE) LIKE '%{0}%' OR UPPER(KHGDESC) LIKE '%{0}%'", codeExpr.ToUpper());
                    break;
                case "Franchise":
                    sql = @"SELECT KHEID GUIDID, KHEFHE CODEEXPR, KHEDESC LIBEXPR, KHEMODI MODIFEXPR FROM KEXPFRH";
                    if (!string.IsNullOrEmpty(codeExpr))
                        sql += string.Format(" WHERE UPPER(KHEFHE) LIKE '%{0}%' OR UPPER(KHEDESC) LIKE '%{0}%'", codeExpr.ToUpper());
                    break;
            }

            if (!string.IsNullOrEmpty(sql))
            {
                var result = DbBase.Settings.ExecuteList<ExprComplexeDetailDto>(CommandType.Text, sql);
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        toReturn.Add(new ConditionComplexeDto
                        {
                            Id = item.GuidId.ToString(),
                            Code = item.CodeExpr,
                            Libelle = item.LibExpr,
                            Modifiable = item.ModifExpr == "O"
                        });
                    }
                }
            }
            return toReturn;
        }

        /// <summary>
        /// Copie les informations de l'expression complexe du référentiel
        /// dans les tables d'expressions complexes
        /// </summary>
        public static string ValidSelExprReferentiel(string codeOffre, string version, string type, string mode, string typeExpr, string idExpr, string splitCharHtml)
        {
            string sql = string.Empty;
            if (mode == "ref")
            {
                EacParameter[] paramRef = new EacParameter[4];
                paramRef[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                paramRef[0].Value = codeOffre.PadLeft(9, ' ');
                paramRef[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
                paramRef[1].Value = version;
                paramRef[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                paramRef[2].Value = type;
                paramRef[3] = new EacParameter("idExpr", DbType.AnsiStringFixedLength);
                paramRef[3].Value = idExpr;

                switch (typeExpr.ToUpper())
                {
                    case "LCI":
                        sql = @"SELECT COUNT(*) NBLIGN FROM KPEXPLCI 
                                                INNER JOIN KEXPLCI ON KDILCE = KHGLCE 
                                            WHERE KDIIPB = :codeOffre AND KDIALX = :version AND KDITYP = :type AND KHGID = :idExpr";
                        break;
                    case "FRANCHISE":
                        sql =@"SELECT COUNT(*) NBLIGN FROM KPEXPFRH
                                                INNER JOIN KEXPFRH ON KDKFHE = KHEFHE 
                                            WHERE KDKIPB = :codeOffre AND KDKALX = :version AND KDKTYP = :type AND KHEID = :idExpr";
                        break;
                }
                var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramRef);
                if (result != null && result.Any())
                {
                    if (result.FirstOrDefault().NbLigne > 0)
                        return "ERRORMSG" + splitCharHtml + "Expression déjà présente.";
                }
            }

            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_TYPEEXPR", DbType.AnsiStringFixedLength);
            param[3].Value = typeExpr;
            param[4] = new EacParameter("P_IDEXPR", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(idExpr) ? Convert.ToInt32(idExpr) : 0;
            param[5] = new EacParameter("P_MODEREF", DbType.AnsiStringFixedLength);
            param[5].Value = mode == "ref" ? "O" : "N";
            param[6] = new EacParameter("P_NEWIDEXPR", DbType.Int32);
            param[6].Value = 0;
            param[6].DbType = DbType.Int32;
            param[6].Direction = ParameterDirection.InputOutput;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_COPYEXPRCOMPREFERENTIEL", param);

            return param[6].Value.ToString();
        }

        public static string DuplicateExpr(string codeOffre, string version, string type, string codeAvn, string typeExpr, string codeExpr)
        {
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEAVN", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
            param[4] = new EacParameter("P_TYPEEXPR", DbType.AnsiStringFixedLength);
            param[4].Value = typeExpr;
            param[5] = new EacParameter("P_CODEEXPR", DbType.Int32);
            param[5].Value = !string.IsNullOrEmpty(codeExpr) ? Convert.ToInt32(codeExpr) : 0;
            param[6] = new EacParameter("P_NEWIDEXPR", DbType.Int32);
            param[6].Value = 0;
            param[6].DbType = DbType.Int32;
            param[6].Direction = ParameterDirection.InputOutput;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DUPLICATEEXPR", param);

            return param[6].Value.ToString();
        }

        public static List<ParametreDto> SearchExprComp(string typeExpr, string strSearch)
        {
            return typeExpr == "LCI" ? GetListLCIExprComplexe(strSearch) : typeExpr == "Franchise" ? GetListFRHExprComplexe(strSearch) : null;
        }

        #endregion

        #region Méthodes privées

        private static List<LigneGarantieDto> GetListLigneExprComp(string typeExpr, string codeExpr)
        {
            List<LigneGarantieDto> model = new List<LigneGarantieDto>();
            string sql = string.Empty;

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codeExpr", DbType.AnsiStringFixedLength);
            param[0].Value = codeExpr;      

            if (typeExpr == "LCI")
            {
                sql = @"SELECT KHHID GUIDDETID, KHHLCVAL VALDET, KHHLCVAU UNITDET, KHHLCBASE TYPEDET, KHHLOVAL CONVALDET, KHHLOVAU CONUNITDET, KHHLOBASE CONTYPEDET
                                FROM KEXPLCID
                                WHERE KHHKHGID = :codeExpr";
            }
            if (typeExpr == "Franchise")
            {
                sql = @"SELECT KHFID GUIDDETID, KHFFHVAL VALDET, KHFFHVAU UNITDET, KHFBASE TYPEDET, KHFFHMINI FRANCHISEMINI, KHFFHMAXI FRANCHISEMAXI,
                                    KHFLIMDEB LIMITEDEB, KHFLIMFIN LIMITEFIN
                                FROM KEXPFRHD 
                                WHERE KHFKHEID = :codeExpr";
            }

            var result = DbBase.Settings.ExecuteList<ExprComplexeDetailDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    model.Add(new LigneGarantieDto
                    {
                        Id = Convert.ToInt32(item.GuidDetId),
                        LCIValeur = item.ValeurDet.ToString(),
                        LCIUnite = item.UniteDet,
                        LCIType = item.TypeDet,
                        ConcurrenceValeur = item.ConValeurDet.ToString(),
                        ConcurrenceUnite = item.ConUniteDet,
                        ConcurrenceType = item.ConTypeDet,
                        FranchiseValeur = item.ValeurDet.ToString(),
                        FranchiseUnite = item.UniteDet,
                        FranchiseType = item.TypeDet,
                        FranchiseMini = item.FranchiseMini.ToString(),
                        FranchiseMaxi = item.FranchiseMaxi.ToString(),
                        FranchiseDebut = AlbConvert.ConvertIntToDate(item.LimiteDeb),
                        FranchiseFin = AlbConvert.ConvertIntToDate(item.LimiteFin)
                    });
                }
            }

            return model;
        }

        private static List<ParametreDto> GetListLCIExprComplexe(string strSearch = "")
        {
            string sql = @"SELECT KHGID LONGID, KHGLCE CODE, KHGDESC DESCRIPTIF FROM KEXPLCI";
            if (!string.IsNullOrEmpty(strSearch))
                sql += string.Format(" WHERE UPPER(KHGLCE) LIKE UPPER('{0}%')", strSearch);
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        private static List<ParametreDto> GetListFRHExprComplexe(string strSearch = "")
        {
            string sql = @"SELECT KHEID LONGID, KHEFHE CODE, KHEDESC DESCRIPTIF FROM KEXPFRH";
            if (!string.IsNullOrEmpty(strSearch))
                sql += string.Format(" WHERE UPPER(KHEFHE) LIKE UPPER('{0}%')", strSearch);
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }

        /// <summary>
        /// Récupération d'un nouveau code pour les expressions complexes
        /// </summary>
        private static string GetNewCodeExprComp(string typeExpr)
        {
            var toReturn = string.Empty;
            string sql = string.Empty;
            if (typeExpr == "LCI")
            {
                toReturn = "L";
                sql = @"SELECT IFNULL(MAX(KHGLCE), '') CODEEXPR FROM KEXPLCI WHERE TRIM(KHGLCE) < 'L00'";
            }
            if (typeExpr == "Franchise")
            {
                toReturn = "F";
                sql = @"SELECT IFNULL(MAX(KHEFHE), '') CODEEXPR FROM KEXPFRH WHERE TRIM(KHEFHE) < 'F00'";
            }
            var result = DbBase.Settings.ExecuteList<ExprComplexeDetailDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                var code = result.FirstOrDefault().CodeExpr;
                if (code != string.Empty)
                {
                    var firstLetter = code.Substring(1, 1);
                    var lastLetter = code.Substring(2, 1);

                    var getPosFirstLetter = AlbConstantesMetiers.EXPRCOMP_CODE_FIRST_LETTER.ToList().FindIndex(l => l == firstLetter);
                    var getPosLastLetter = AlbConstantesMetiers.EXPRCOMP_CODE_LAST_LETTER.ToList().FindIndex(l => l == lastLetter);

                    if (getPosLastLetter == AlbConstantesMetiers.EXPRCOMP_CODE_LAST_LETTER.Count() - 1)
                    {
                        if (getPosFirstLetter == AlbConstantesMetiers.EXPRCOMP_CODE_LAST_LETTER.Count() - 1)
                        {
                            getPosFirstLetter = -1;
                            getPosLastLetter = -1;
                        }
                        else
                        {
                            getPosFirstLetter += 1;
                            getPosLastLetter = 0;
                        }
                    }
                    else
                    {
                        getPosLastLetter += 1;
                    }
                    if (getPosFirstLetter >= 0 && getPosLastLetter >= 0)
                        toReturn = toReturn + AlbConstantesMetiers.EXPRCOMP_CODE_FIRST_LETTER[getPosFirstLetter] + AlbConstantesMetiers.EXPRCOMP_CODE_LAST_LETTER[getPosLastLetter];
                    else
                        toReturn = string.Empty;
                }
                else
                    toReturn = toReturn + "AA";
            }
            return toReturn;
        }
        
        #endregion

    }
}
