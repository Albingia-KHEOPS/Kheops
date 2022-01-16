using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.ParametreGaranties;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Data.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;

namespace OP.DataAccess
{
    public class ParamGarantieRepository
    {
        public static List<ParamGarantieDto> GetGaranties(string code, string designation, string additionalParam, bool modeAutocomplete)
        {
            string sql = @"SELECT 
GAGAR CODE , GADES DESIGNATION , GADEA ABREGE , GATAX CODETAXE , GACNX CODECATNAT , GACOM GARCOM , GADFG CODETYPEDEF , GAIFC CODETYPINFO , 
GARGE REGU , GATRG CODETYPGRILLE , GAINV INVENTAIRE , GAATG ATTENTATGAREAT , GATYI CODETYPINVENTAIRE FROM KGARAN";
            string whereAndOr = " WHERE";

            if (!string.IsNullOrEmpty(code))
            {
                sql += whereAndOr;
                sql += " TRIM(LOWER(GAGAR)) LIKE '%{0}%'";
                whereAndOr = (modeAutocomplete ? " OR" : " AND");
            }
            if (!string.IsNullOrEmpty(designation))
            {
                sql += whereAndOr;
                sql += " TRIM(LOWER(GADES)) LIKE '%{1}%'";
                whereAndOr = (modeAutocomplete ? " OR" : " AND");
            }
            sql += " ORDER BY CODE";

            string sqlFormated = string.Format(sql, code.Trim().ToLower(), designation.Replace("'", "''").Trim().ToLower());
            return DbBase.Settings.ExecuteList<ParamGarantieDto>(CommandType.Text, sqlFormated);
        }
        public static ParamGarantieDto GetGarantie(string code, string additionalParam)
        {
            string sql = @"SELECT 
GAGAR CODE , GADES DESIGNATION , GADEA ABREGE , GATAX CODETAXE , GACNX CODECATNAT , GACOM GARCOM , GADFG CODETYPEDEF , GAIFC CODETYPINFO , 
GARGE REGU , GATRG CODETYPGRILLE , GAINV INVENTAIRE , GAATG ATTENTATGAREAT , GATYI CODETYPINVENTAIRE FROM KGARAN";
            string whereAndOr = " WHERE";

            if (!string.IsNullOrEmpty(code))
            {
                sql += whereAndOr;
                sql += " LOWER(GAGAR) = '{0}'";
            }

            string sqlFormated = string.Format(sql, code.PadRight(10, ' ').ToLower());
            var result= DbBase.Settings.ExecuteList<ParamGarantieDto>(CommandType.Text, sqlFormated).FirstOrDefault();

            result.GarTypeReguls = GetTypeRegulByGarantie(code);

            return result;
        }
        public static ParamGarantieListesDto GetParamGarantieListes()
        {
            var paramGarantieListesDto = new ParamGarantieListesDto();
            paramGarantieListesDto.Taxes = CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "TAXEC");
            paramGarantieListesDto.CatNats = CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "TAXEC");
            paramGarantieListesDto.TypesDefinition = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "GADFG");
            paramGarantieListesDto.TypesInformation = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "GAIFC");
            paramGarantieListesDto.TypesGrille = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "GATRG");
            paramGarantieListesDto.TypesRegul = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "RGTRG");
            return paramGarantieListesDto;
        }
        public static string EnregistrerGarantie(ParamGarantieDto garantie, string mode, string additionalParam)
        {
            var paramError = new EacParameter("P_ERREUR", DbType.AnsiStringFixedLength) { Value = string.Empty };
            paramError.Direction = ParameterDirection.InputOutput;
            paramError.Size = 256;
            var param = new[] {
                new EacParameter("P_GARANTIE", DbType.AnsiStringFixedLength) { Value = garantie.CodeGarantie },
                new EacParameter("P_DESIGNATION", DbType.AnsiStringFixedLength) { Value = garantie.DesignationGarantie },
                new EacParameter("P_ABREGE", DbType.AnsiStringFixedLength) { Value = garantie.Abrege },
                new EacParameter("P_TAXE", DbType.AnsiStringFixedLength) { Value = garantie.CodeTaxe },
                new EacParameter("P_CATNAT", DbType.AnsiStringFixedLength) { Value = garantie.CodeCatNat },
                new EacParameter("P_GAR_COMMUNE", DbType.AnsiStringFixedLength) { Value = garantie.GarantieCommune },
                new EacParameter("P_CODE_DEFINITION", DbType.AnsiStringFixedLength) { Value = garantie.CodeTypeDefinition },
                new EacParameter("P_CODE_INFORMATION", DbType.AnsiStringFixedLength) { Value = garantie.CodeTypeInformation },
                new EacParameter("P_REGULARISABLE", DbType.AnsiStringFixedLength) { Value = garantie.Regularisable },
                new EacParameter("P_CODE_GRILLE", DbType.AnsiStringFixedLength) { Value = garantie.CodeTypeGrille },
                new EacParameter("P_INVENTAIRE", DbType.AnsiStringFixedLength) { Value = garantie.Inventaire },
                new EacParameter("P_CODE_INVENTAIRE", DbType.AnsiStringFixedLength) { Value = garantie.CodeTypeInventaire },
                new EacParameter("P_ATTG", DbType.AnsiStringFixedLength) { Value = garantie.AttentatGareat },
                new EacParameter("P_MODE", DbType.AnsiStringFixedLength) { Value = mode },
                paramError
            };
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDGARANTIE", param);
            return paramError.Value.ToString();
        }
        public static string SupprimerGarantie(string codeGarantie, string additionalParam)
        {
            string sql = string.Format(@"DELETE FROM KGARAN
                                    WHERE GAGAR = '{0}'", codeGarantie);

            if (DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql) > 0)
            {
                sql = string.Format(@"DELETE FROM KGARFAM
                                   WHERE GVGAR ='{0}'", codeGarantie);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                sql = sql = string.Format(@"DELETE FROM KGARTVL
                                   WHERE KGKGAR ='{0}'", codeGarantie);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                return "";
            }
            else
                return "Aucune mise à jour, la garantie n'existe pas";
        }

        public static List<TypeValeurDto> GetGarTypesValeur(string codeGarantie, string additionalParam)
        {
            string sql = string.Format(@"SELECT KGKID ID, KGKGAR CODEGARANTIE,KGKORD NUMORDRE,
                                        KGKTYVAL CODETYPEVALEUR, DSC.KGLDESC LIBELLETYPEVALEUR
                                        FROM KGARTVL
                                        LEFT JOIN KTYPVAL DSC ON DSC.KGLTYVAL = KGKTYVAL
                                        WHERE KGKGAR='{0}' ORDER BY KGKORD", codeGarantie);
            return DbBase.Settings.ExecuteList<TypeValeurDto>(CommandType.Text, sql);
        }
        public static TypeValeurDto GetGarTypeValeurById(int id, string codeGarantie, string additionalParam)
        {
            string sql = string.Format(@"SELECT KGKID ID, KGKGAR CODEGARANTIE,KGKORD NUMORDRE,
                                        KGKTYVAL CODETYPEVALEUR, DSC.KGLDESC LIBELLETYPEVALEUR
                                        FROM KGARTVL
                                        LEFT JOIN KTYPVAL DSC ON DSC.KGLTYVAL = KGKTYVAL
                                        WHERE KGKGAR='{0}' AND KGKID='{1}' ", codeGarantie, id);
            return DbBase.Settings.ExecuteList<TypeValeurDto>(CommandType.Text, sql).FirstOrDefault();
        }
        public static List<ParametreDto> LoadGarListTypesValeur(string codeGarantie, string id)
        {
            int idGarTypeValeur = !string.IsNullOrEmpty(id) ? int.Parse(id) : 0;
            string sql = string.Format(@"SELECT KGLTYVAL CODE, KGLDESC LIBELLE
                                        FROM KTYPVAL
                                   WHERE KGLTYVAL NOT IN (SELECT KGKTYVAL FROM KGARTVL WHERE KGKGAR='{0}' AND KGKID<>'{1}')", codeGarantie, idGarTypeValeur);
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }
        public static string EnregistrerGarTypeValeur(TypeValeurDto typeValeur, string mode, string additionalParam)
        {
            if (mode == "I")
                typeValeur.Id = CommonRepository.GetAS400Id("KGKID");
            DbParameter[] param = new DbParameter[6];
            param[0] = new EacParameter("P_ID", 0);
            param[0].Value = typeValeur.Id;
            param[1] = new EacParameter("P_GARANTIE", typeValeur.CodeGarantie);
            param[2] = new EacParameter("P_NUM_ORDRE", 0);
            param[2].Value = typeValeur.NumOrdre;
            param[3] = new EacParameter("P_CODE_TYPE_VALEUR", typeValeur.CodeTypeValeur);
            param[4] = new EacParameter("P_MODE", mode);
            param[5] = new EacParameter("P_ERREUR", "");
            param[5].Direction = ParameterDirection.InputOutput;
            param[5].Size = 256;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDTYPEVALEURGARANTIE", param);
            return param[5].Value.ToString();
        }
        public static string SupprimerGarTypeValeur(string codeGarantie, int id, string additionalParam)
        {
            string sql = string.Format(@"DELETE FROM KGARTVL
                                   WHERE KGKGAR ='{0}' 
                                   AND KGKID='{1}'", codeGarantie, id);
            if (DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql) > 0)
                return "";
            else
                return "Aucune mise à jour, la garantie n'existe pas";
        }

        public static List<FamilleReassuranceDto> GetGarFamillesReassurance(string codeGarantie, string additionalParam)
        {
            string sql = string.Format(@"SELECT GVGAR CODEGARANTIE, GVBRA CODEBRANCHE,BCH.TPLIL LIBELLEBRANCHE,GVSBR CODESOUSBRANCHE, SBCH.TPLIL LIBELLESOUSBRANCHE,
                                GVCAT CODECATEGORIE,CAT.CADES LIBELLECATEGORIE, GVFAM CODEFAMILLE, FAM.TPLIL
                              FROM KGARFAM 
                                LEFT JOIN YCATEGO CAT ON CAT.CABRA=GVBRA AND CAT.CASBR=GVSBR  AND CAT.CACAT= GVCAT                        
                                {1}
                                {2}
                                {3}
                                WHERE GVGAR='{0}' ORDER BY GVGAR,GVBRA,GVSBR,GVCAT", codeGarantie,
                               CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "BRCHE", "BCH", " AND GVBRA = BCH.TCOD"),
                               CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "BRSBR", "SBCH", " AND GVSBR = SBCH.TCOD"),
                               CommonRepository.BuildJoinYYYYPAR("LEFT", "REASS", "GARAN", "FAM", " AND GVFAM = FAM.TCOD")
                               );
            return DbBase.Settings.ExecuteList<FamilleReassuranceDto>(CommandType.Text, sql);
        }
        public static List<ParametreDto> LoadListBranches()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "BRCHE");
        }

        /// <summary>
        /// Call the Common Repository to Load the list of Famille as specified in the spec using for tCon = "REASS" and tFam = "GARAN"
        /// </summary>
        /// <param name="branche"></param>
        /// <param name="cible"></param>
        /// <param name="tCon"></param>
        /// <param name="tFam"></param>
        /// <param name="tPca1"></param>
        /// <param name="tCod"></param>
        /// <param name="notIn"></param>
        /// <param name="isBO"></param>
        /// <param name="tPcn2"></param>
        /// <returns>List of Param dto but just two fields are really need{CODE;LIBELLE}</returns>
        public static List<ParametreDto> LoadListFamilles(string branche, string cible, string tCon, string tFam, string tPca1 = null, List<String> tCod = null, bool notIn = false, bool isBO = false, string tPcn2 = null)
        {
            return CommonRepository.GetParametres(branche, cible, tCon, tFam, tPca1, tCod, notIn, isBO, tPcn2);
        }

        public static List<ParametreDto> LoadListSousBranches(string codeBranche)
        {
            string sql = CommonRepository.BuildSelectYYYYPAR(string.Empty, string.Empty, string.Empty, string.Format("IFNULL(SUBSTR(TCOD, {0}), TCOD) CODE, TPLIB LIBELLE, TPLIB DESCRIPTIF", codeBranche.Length + 1),
                               "GENER", "BRSBR",
                               otherCriteria: string.Format(" AND TCOD LIKE '{0}%'", codeBranche));

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql).OrderBy(m => m.Code).ToList();
        }
        public static List<ParametreDto> LoadListCategories(string codeBranche, string codeSousBranche)
        {
            string sql = string.Format(@"SELECT CACAT CODE, CADES LIBELLE FROM YCATEGO WHERE CABRA = '{0}' AND CASBR = '{1}'", codeBranche, codeSousBranche);
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }
        public static string GetGarFamille(string codeCategorie)
        {
            string sql = string.Format(@"SELECT TCOD CODE
                                        FROM YYYYPAR 
                                        WHERE TCON='REASS' AND TFAM='GARAN' AND TCOD= '{0}'", codeCategorie);
            var paramDto = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql).FirstOrDefault();
            if (paramDto != null) return paramDto.Code;
            return string.Empty;
        }

        public static string EnregistrerFamilleReassurance(FamilleReassuranceDto familleReassurance, FamilleReassuranceDto familleReassuranceAncienne, string mode, string additionalParam)
        {
            DbParameter[] param = new DbParameter[11];
            param[0] = new EacParameter("P_GARANTIE", familleReassurance.CodeGarantie);
            param[1] = new EacParameter("P_CODE_BRANCHE", familleReassurance.CodeBranche);
            param[2] = new EacParameter("P_CODE_SOUS_BRANCHE", familleReassurance.CodeSousBranche);
            param[3] = new EacParameter("P_CODE_CATEGORIE", familleReassurance.CodeCategorie);
            param[4] = new EacParameter("P_CODE_FAMILLE", familleReassurance.CodeFamille);

            param[5] = new EacParameter("P_CODE_BRANCHE_ANCIEN", familleReassuranceAncienne.CodeBranche);
            param[6] = new EacParameter("P_CODE_SOUS_BRANCHE_ANCIEN", familleReassuranceAncienne.CodeSousBranche);
            param[7] = new EacParameter("P_CODE_CATEGORIE_ANCIEN", familleReassuranceAncienne.CodeCategorie);
            param[8] = new EacParameter("P_CODE_FAMILLE_ANCIEN", familleReassuranceAncienne.CodeFamille);

            param[9] = new EacParameter("P_MODE", mode);
            param[10] = new EacParameter("P_ERREUR", "");
            param[10].Direction = ParameterDirection.InputOutput;
            param[10].Size = 256;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDFAMILLEREASSURANCE", param);
            return param[10].Value.ToString();
        }
        public static string SupprimerFamilleReassurance(string codeGarantie, string codeBranche, string codeSousBranche, string codeCategorie, string codeFamille, string additionalParam)
        {
            string sql = string.Format(@"DELETE FROM KGARFAM
                                   WHERE GVGAR ='{0}' 
                                   AND GVBRA='{1}'
                                   AND GVSBR='{2}' 
                                   AND GVCAT='{3}'
                                   AND GVFAM='{4}'", codeGarantie, codeBranche, codeSousBranche, codeCategorie, codeFamille);
            if (DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql) > 0)
                return "";
            else
                return "Aucune mise à jour, la garantie n'existe pas";
        }
        public static List<OffreInventaireGarantieDto> GetListOffreInventaireByGarantie(string codeGarantie)
        {
            var parameters = new List<EacParameter> {
                new EacParameter("codeGarantie", DbType.AnsiStringFixedLength, 10) { Value = codeGarantie },
                new EacParameter("peri", DbType.AnsiStringFixedLength, 2) { Value = PerimetreInventaire.Garantie.AsCode() }
            };
            string sql = @"
SELECT KBGIPB CODEOFFRE,KBGALX VERSION,KBGTYP TYPE,KBGKBEID CODEINVENTAIRE, KBGFOR CODEFORMULE,KBGGAR CODEGARANTIE,KDAALPHA LETTREFORMULE,KDADESC LIBELLEFORMULE
FROM KPINVAPP 
LEFT JOIN KPFOR ON KDAFOR=KBGFOR AND  KDAIPB = KBGIPB AND KDAALX = KBGALX AND KDATYP = KBGTYP 
WHERE KBGGAR = :codeGarantie AND KBGPERI = :peri";
            return DbBase.Settings.ExecuteList<OffreInventaireGarantieDto>(CommandType.Text, sql, parameters.ToArray());
        }

        public static List<GarTypeRegulDto> GetTypeRegulByGarantie(string codeGarantie)
        {
            var param = new List<EacParameter>();

            var p = new EacParameter("codeGarantie", DbType.AnsiStringFixedLength)
            {
                Value = codeGarantie.Trim().PadRight(10, ' ')
            };

            param.Add(p);
            var sql = @"SELECT KIHGAR CODEGARANTIE,
                               TPLIB LIBELLE,
                               TPCA1 GRILLE,
                               KIHTRG CODETYPEREGUL

                        FROM KGARTRG
                              LEFT JOIN YYYYPAR ON TCON = 'KHEOP'AND TFAM = 'RGTRG' AND TCOD = KIHTRG
                        WHERE KIHGAR = :codeGarantie";

            return DbBase.Settings.ExecuteList<GarTypeRegulDto>(CommandType.Text, sql, param);

        }

        public static List<GarTypeRegulDto> GetAllTypeRegul()
        {
            var sql = @"SELECT KIHGAR CODEGARANTIE,
                               TPLIB LIBELLE,
                               TPCA1 GRILLE,
                               KIHTRG CODETYPEREGUL

                        FROM KGARTRG
                        LEFT JOIN YYYYPAR ON TCON = 'KHEOP'AND TFAM = 'RGTRG' AND TCOD = KIHTRG";

            return DbBase.Settings.ExecuteList<GarTypeRegulDto>(CommandType.Text, sql);

        }

        public static void AddTypeRegul(string codeGarantie, string codeTypeRegul)
        {
            var param = new List<EacParameter>();

            var p = new EacParameter("codeGarantie", DbType.AnsiStringFixedLength)
            {
                Value = codeGarantie.Trim().PadRight(10, ' ')
            };
            param.Add(p);

            p = new EacParameter("codeTypeRegul", DbType.AnsiStringFixedLength)
            {
                Value = codeTypeRegul.Trim().PadLeft(2, ' ')
            };
            param.Add(p);

            var sql = "INSERT INTO KGARTRG (KIHGAR,KIHTRG) VALUES (:codeGarantie,:codeTypeRegul)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text ,sql,param);
        }

        public static void DeleteTypeRegul(string codeGarantie, string codeTypeRegul)
        {
            var param = new List<EacParameter>();

            var p = new EacParameter("codeGarantie", DbType.AnsiStringFixedLength)
            {
                Value = codeGarantie.Trim().PadRight(10, ' ')
            };
            param.Add(p);

            p = new EacParameter("codeTypeRegul", DbType.AnsiStringFixedLength)
            {
                Value = codeTypeRegul.Trim().PadLeft(2, ' ')
            };
            param.Add(p);

            var sql = "DELETE FROM KGARTRG WHERE KIHGAR = :codeGarantie AND KIHTRG = :codeTypeRegul";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static bool IsTypeRegulAssociated(string codeGarantie, string codeTypeRegul)
        {
            var param = new List<EacParameter>();

            var p = new EacParameter("codeGarantie", DbType.AnsiStringFixedLength)
            {
                Value = codeGarantie.Trim().PadRight(10, ' ')
            };
            param.Add(p);

            p = new EacParameter("codeTypeRegul", DbType.AnsiStringFixedLength)
            {
                Value = codeTypeRegul.Trim().PadLeft(2, ' ')
            };
            param.Add(p);

            var sql = "SELECT COUNT(*) NBLIGN FROM KGARTRG WHERE KIHGAR = :codeGarantie AND KIHTRG = :codeTypeRegul";
            return CommonRepository.ExistRow(sql, param.ToArray());
        }

        public static void CleanAllTypeRegulsByGarantie(string codeGarantie)
        {
            var param = new List<EacParameter>();

            var p = new EacParameter("codeGarantie", DbType.AnsiStringFixedLength)
            {
                Value = codeGarantie.PadLeft(10,' ')
            };
            param.Add(p);
            var sql = "DELETE FROM KGARTRG WHERE KIHGAR = :codeGarantie";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text,sql,param);
        }
    }
}
