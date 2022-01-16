using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.GestionNomenclature;
using OP.WSAS400.DTO.Offres.Parametres;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.Offres.Branches;
using System.Globalization;
using OP.WSAS400.DTO.Common;


namespace OP.DataAccess
{
    public class ParamGestionNomenclatureRepository
    {

        #region Nomenclature

        #region Méthodes publiques

        /// <summary>
        /// Charge les informations générales de la gestion des nomenclatures
        /// </summary>
        public static GestionNomenclatureDto LoadInfoGestionNomenclature()
        {
            GestionNomenclatureDto model = new GestionNomenclatureDto
            {
                Typologies = CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "NMTYP"),
                Branches = CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "BRCHE", tCod: new List<string> { "PP", "ZZ" }, notIn: true),
                Cibles = new List<ParametreDto>()
            };
            return model;
        }

        /// <summary>
        /// Charge la liste des nomenclatures suivant les paramètres renseignés
        /// </summary>
        public static GestionNomenclatureDto LoadListNomenclature(string typologie, string branche, string cible)
        {
            GestionNomenclatureDto model = new GestionNomenclatureDto { Nomenclatures = new List<NomenclatureDto>() };

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("P_TYPOLOGIE", DbType.AnsiStringFixedLength);
            param[0].Value = typologie;
            param[1] = new EacParameter("P_BRANCHE", DbType.AnsiStringFixedLength);
            param[1].Value = branche;
            param[2] = new EacParameter("P_CIBLE", DbType.AnsiStringFixedLength);
            param[2].Value = cible;

            var result = DbBase.Settings.ExecuteList<NomenclatureDto>(CommandType.StoredProcedure, "SP_LOADLISTNOMENCLATURE", param);
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    if (model.Nomenclatures.FindAll(
                        m => m.Code == item.Code
                        && m.CodeTypologie == item.CodeTypologie
                        ).Count == 0)
                    {
                        item.Branche = new BrancheDto
                        {
                            Code = item.CodeBranche,
                            Nom = item.LibBranche,
                            Cible = new CibleDto
                            {
                                Code = item.CodeCible,
                                Nom = item.LibCible
                            }
                        };
                        item.Typologie = new ParametreDto { Code = item.CodeTypologie, Libelle = item.LibTypologie };

                        model.Nomenclatures.Add(item);
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Charge les informations de la nomenclature sélectionnée
        /// ou 
        /// Charge une nouvelle nomenclature
        /// </summary>
        public static NomenclatureDto OpenNomenclature(string idNomenclature, string typologie)
        {
            NomenclatureDto model = new NomenclatureDto { Grilles = new List<GrilleDto>() };
            if (!string.IsNullOrEmpty(idNomenclature) && idNomenclature != "0")
            {

                EacParameter[] param = new EacParameter[1];
                param[0] = new EacParameter("idNomenclature", DbType.AnsiStringFixedLength);
                param[0].Value = idNomenclature;

                string sql = @"SELECT KHIID NOMENCLATUREID, KHINORD NOMENCLATUREORDRE, KHINMC NOMENCLATURECODE, KHIDESI NOMENCLATURELIB, KHITYPO CODETYPO, KHKNMG CODEGRILLE, KHJDESI LIBGRILLE
                                                FROM KNMREF
    	                                            LEFT JOIN KNMVALF ON KHIID = KHKKHIID
    	                                            LEFT JOIN KNMGRI ON KHKNMG = KHJNMG
                                                WHERE KHIID = :idNomenclature ORDER BY CODEGRILLE";
                var result = DbBase.Settings.ExecuteList<NomenclaturePlatDto>(CommandType.Text, sql, param);
                if (result != null && result.Any())
                {
                    var firstRes = result.FirstOrDefault();
                    model.Id = firstRes.Id;
                    model.Ordre = firstRes.Ordre;
                    model.Code = firstRes.Code;
                    model.Libelle = firstRes.Libelle;
                    model.CodeTypologie = firstRes.CodeTypologie;

                    foreach (var item in result)
                    {
                        if (!string.IsNullOrEmpty(item.CodeGrille))
                            model.Grilles.Add(new GrilleDto { Code = item.CodeGrille, Libelle = item.LibGrille });
                    }
                    typologie = model.CodeTypologie;
                }
            }
            else
            {
                model.Id = !string.IsNullOrEmpty(idNomenclature) ? Convert.ToInt32(idNomenclature) : 0;
                model.Ordre = GetMaxOrdreTypologie(typologie);
            }
            model.Typologie = CommonRepository.GetParametreByCode(string.Empty, string.Empty, "KHEOP", "NMTYP", typologie);
            //model.GrilleLiee = CommonRepository.ExistRow(string.Format("SELECT COUNT(*) NBLIGN FROM KNMVALF WHERE KHKKHIID = {0}", idNomenclature));
            return model;
        }

        /// <summary>
        /// Sauvegarde les informations de la nomenclature
        /// </summary>
        public static string SaveNomenclature(string idNomenclature, string codeNomenclature, string ordreNomenclature, string libelleNomenclature, string typologie)
        {
            EacParameter[] param = new EacParameter[6];
            param[0] = new EacParameter("P_IDNOMENCLATURE", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(idNomenclature) ? Convert.ToInt32(idNomenclature) : 0;
            param[1] = new EacParameter("P_CODENOMENCLATURE", DbType.AnsiStringFixedLength);
            param[1].Value = codeNomenclature.ToUpper();
            param[2] = new EacParameter("P_ORDRENOMENCLATURE", DbType.Int32);
            param[2].Value = !string.IsNullOrEmpty(ordreNomenclature) ? Convert.ToDecimal(ordreNomenclature) : 0;
            param[3] = new EacParameter("P_LIBELLENOMENCLATURE", DbType.AnsiStringFixedLength);
            param[3].Value = libelleNomenclature;
            param[4] = new EacParameter("P_TYPOLOGIE", DbType.AnsiStringFixedLength);
            param[4].Value = typologie;
            param[5] = new EacParameter("P_RETURN", DbType.AnsiStringFixedLength);
            param[5].Value = "";
            param[5].Direction = ParameterDirection.InputOutput;
            param[5].Size = 500;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVENOMENCLATURE", param);

            return param[5].Value.ToString();
        }

        /// <summary>
        /// Supprimer la nomenclature
        /// </summary>
        public static void DeleteNomenclature(string idNomenclature)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("P_IDNOMENCLATURE", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(idNomenclature) ? Convert.ToInt32(idNomenclature) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETENOMENCLATURE", param);
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Récupère l'ordre max de la typologie
        /// </summary>
        private static Double GetMaxOrdreTypologie(string typologie)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("typologie", DbType.AnsiStringFixedLength);
            param[0].Value = typologie;

            string sql = @"SELECT MAX(KHINORD) + 1 NOMENCLATUREORDRE FROM KNMREF WHERE KHITYPO = :typologie";
            var result = DbBase.Settings.ExecuteList<NomenclatureDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
                return result.FirstOrDefault().Ordre > 0 ? result.FirstOrDefault().Ordre : 1;
            return 1;
        }

        #endregion

        #endregion

        #region Grille

        #region Méthodes publiques

        /// <summary>
        /// Charge la liste des grilles suivant les paramètres renseignés
        /// </summary>
        public static List<GrilleDto> LoadInfoGestionGrille(string searchGrille)
        {
            string sql = @"SELECT KHJNMG CODE, KHJDESI LIBELLE FROM KNMGRI";
            if (!string.IsNullOrEmpty(searchGrille))
            {
                sql += string.Format(" WHERE UPPER(KHJNMG) LIKE '%{0}%' OR UPPER(KHJDESI) LIKE '%{0}%'", searchGrille.ToUpper());
            }
            return DbBase.Settings.ExecuteList<GrilleDto>(CommandType.Text, sql);
        }

        /// <summary>
        /// Charge les informations d'une grille
        /// </summary>
        public static GrilleDto OpenGrille(string idGrille)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("idGrille", DbType.AnsiStringFixedLength);
            param[0].Value = idGrille.ToUpper();

            GrilleDto model = new GrilleDto { Typologies = new List<TypologieDto>() };
            string sql = @"SELECT KHJNMG CODE, KHJDESI LIBELLE, KHJTYPO1 TYPO1, KHJLIB1 LIBTYPO1, KHJLIEN1 LIENTYPO1,
                                    KHJTYPO2 TYPO2, KHJLIB2 LIBTYPO2, KHJLIEN2 LIENTYPO2, KHJTYPO3 TYPO3, KHJLIB3 LIBTYPO3, KHJLIEN3 LIENTYPO3,
                                    KHJTYPO4 TYPO4, KHJLIB4 LIBTYPO4, KHJLIEN4 LIENTYPO4, KHJTYPO5 TYPO5, KHJLIB5 LIBTYPO5, KHJLIEN5 LIENTYPO5,
                                    KHKTYPO TYPOGRILLE
                                        FROM KNMGRI 
                                            LEFT JOIN KNMVALF ON KHKNMG = KHJNMG
                                        WHERE UPPER(KHJNMG) = :idGrille";
            var result = DbBase.Settings.ExecuteList<GrillePlatDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                var firstRes = result.FirstOrDefault();
                model.Code = firstRes.Code;
                model.Libelle = firstRes.Libelle;
                model.Typologies.Add(new TypologieDto { Code = firstRes.Typologie1, Libelle = firstRes.LibTypologie1, Lien = firstRes.LienTypologie1 });
                model.Typologies.Add(new TypologieDto { Code = firstRes.Typologie2, Libelle = firstRes.LibTypologie2, Lien = firstRes.LienTypologie2 });
                model.Typologies.Add(new TypologieDto { Code = firstRes.Typologie3, Libelle = firstRes.LibTypologie3, Lien = firstRes.LienTypologie3 });
                model.Typologies.Add(new TypologieDto { Code = firstRes.Typologie4, Libelle = firstRes.LibTypologie4, Lien = firstRes.LienTypologie4 });
                model.Typologies.Add(new TypologieDto { Code = firstRes.Typologie5, Libelle = firstRes.LibTypologie5, Lien = firstRes.LienTypologie5 });
                model.TypologieGrille = firstRes.TypologieGrille;
            }

            model.LstTypologie = GetListTypologie();
            model.Cibles = GetCibleGrille(idGrille);
            EacParameter[] paramExistRow = new EacParameter[1];
            paramExistRow[0] = new EacParameter("code", DbType.AnsiStringFixedLength);
            paramExistRow[0].Value = model.Code;
            string existRowSql = "SELECT COUNT(*) NBLIGN FROM KCIBLE WHERE KAHNMG = :code";

            model.CibleLiee = CommonRepository.ExistRowParam(existRowSql, paramExistRow);

            return model;
        }

        /// <summary>
        /// Sauvegarde les informations de la grille
        /// </summary>
        public static string SaveGrille(string codeGrille, string libelleGrille, int newGrille)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_NEWGRILLE", DbType.Int32);
            param[0].Value = newGrille;
            param[1] = new EacParameter("P_CODEGRILLE", DbType.AnsiStringFixedLength);
            param[1].Value = codeGrille.ToUpper();
            param[2] = new EacParameter("P_LIBELLEGRILLE", DbType.AnsiStringFixedLength);
            param[2].Value = libelleGrille;
            param[3] = new EacParameter("P_RETURN", DbType.AnsiStringFixedLength);
            param[3].Value = "";
            param[3].Direction = ParameterDirection.InputOutput;
            param[3].Size = 500;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEGRILLE", param);

            return param[3].Value.ToString();
        }

        /// <summary>
        /// Supprimer la grille
        /// </summary>
        public static void DeleteGrille(string codeGrille)
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("P_CODEGRILLE", DbType.AnsiStringFixedLength);
            param[0].Value = codeGrille;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEGRILLE", param);


            //string sql = string.Format(@"DELETE FROM KNMGRI WHERE KHJNMG = '{0}'", codeGrille);
            //DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        /// <summary>
        /// Sauvegarde une ligne de la grille
        /// et la grille si c'est une nouvelle grille
        /// </summary>
        public static string SaveLineGrille(string codeGrille, string libelleGrille, string newGrille,
            string typologie, string libTypologie, string lienTypologie, string ordreTypologie)
        {
            EacParameter[] param = new EacParameter[8];
            param[0] = new EacParameter("P_NEWGRILLE", DbType.Int32);
            param[0].Value = newGrille;
            param[1] = new EacParameter("P_CODEGRILLE", DbType.AnsiStringFixedLength);
            param[1].Value = codeGrille;
            param[2] = new EacParameter("P_LIBELLEGRILLE", DbType.AnsiStringFixedLength);
            param[2].Value = libelleGrille;
            param[3] = new EacParameter("P_TYPOLOGIE", DbType.AnsiStringFixedLength);
            param[3].Value = typologie;
            param[4] = new EacParameter("P_LIBTYPOLOGIE", DbType.AnsiStringFixedLength);
            param[4].Value = libTypologie;
            param[5] = new EacParameter("P_LIENTYPOLOGIE", DbType.AnsiStringFixedLength);
            param[5].Value = lienTypologie;
            param[6] = new EacParameter("P_ORDRETYPOLOGIE", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(ordreTypologie) ? Convert.ToInt32(ordreTypologie) : 0;
            param[7] = new EacParameter("P_RETURN", DbType.AnsiStringFixedLength);
            param[7].Value = "";
            param[7].Direction = ParameterDirection.InputOutput;
            param[7].Size = 500;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVELINEGRILLE", param);

            return param[7].Value.ToString();
        }

        /// <summary>
        /// Supprime une typologie dans la grille
        /// </summary>
        public static void DeleteLineGrille(string codeGrille, string ordreTypologie)
        {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("P_CODEGRILLE", DbType.AnsiStringFixedLength);
            param[0].Value = codeGrille;
            param[1] = new EacParameter("P_LINEID", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(ordreTypologie) ? Convert.ToInt32(ordreTypologie) : 0;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETELINEGRILLE", param);
        }

        /// <summary>
        /// Charge les informations de la typologie de grille sélectionnée
        /// </summary>
        public static GrilleDto OpenSelectionValeur(string codeGrille, string typoGrille, string niveau, string searchTerm = "")
        {
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codeGrille", DbType.AnsiStringFixedLength);
            param[0].Value = codeGrille.ToUpper();

            GrilleDto model = new GrilleDto { Typologies = new List<TypologieDto>() };
            string sql = @"SELECT KHJNMG CODE, KHJDESI LIBELLE, KHJTYPO1 TYPO1, KHJLIB1 LIBTYPO1, KHJLIEN1 LIENTYPO1,
                                    KHJTYPO2 TYPO2, KHJLIB2 LIBTYPO2, KHJLIEN2 LIENTYPO2, KHJTYPO3 TYPO3, KHJLIB3 LIBTYPO3, KHJLIEN3 LIENTYPO3,
                                    KHJTYPO4 TYPO4, KHJLIB4 LIBTYPO4, KHJLIEN4 LIENTYPO4, KHJTYPO5 TYPO5, KHJLIB5 LIBTYPO5, KHJLIEN5 LIENTYPO5,
                                    KHKTYPO TYPOGRILLE
                                        FROM KNMGRI 
                                            LEFT JOIN KNMVALF ON KHKNMG = KHJNMG
                                        WHERE UPPER(KHJNMG) = :codeGrille";
            var result = DbBase.Settings.ExecuteList<GrillePlatDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                var firstRes = result.FirstOrDefault();
                model.Code = firstRes.Code;
                model.Libelle = firstRes.Libelle;

                model.Typologies = LoadTypologiesGrille(firstRes, niveau);
                model.Typologie = GetTypologie(firstRes, niveau);
                model.Niveau = niveau;

                model.TypologieGrille = firstRes.TypologieGrille;
                model.Nomenclatures = LoadNomenclatureValeur(firstRes, niveau, searchTerm);
            }
            return model;
        }

        /// <summary>
        /// Sauvegarde les valeurs sélectionnées 
        /// pour la typologie de grille sélectonnée
        /// </summary>
        public static void SaveValeurs(string codeGrille, string typologie, string niveau, string niveauMere,
            string selVal1, string selVal2, string selVal3, string selVal4, string selVal5,
            string selVal6, string selVal7, string selVal8, string selVal9, string selVal10)
        {
            EacParameter[] param = new EacParameter[16];
            param[0] = new EacParameter("P_CODEGRILLE", DbType.AnsiStringFixedLength);
            param[0].Value = codeGrille;
            param[1] = new EacParameter("P_TYPOLOGIE", DbType.AnsiStringFixedLength);
            param[1].Value = typologie;
            param[2] = new EacParameter("P_NIVEAU", niveau);
            param[2].Value = !string.IsNullOrEmpty(niveau) ? Convert.ToInt32(niveau) : 0;
            param[3] = new EacParameter("P_NIVEAUMERE", niveauMere);
            param[3].Value = !string.IsNullOrEmpty(niveauMere) ? Convert.ToInt32(niveauMere) : 0;
            param[4] = new EacParameter("P_SELVAL1", DbType.AnsiStringFixedLength);
            param[4].Value = selVal1;
            param[5] = new EacParameter("P_SELVAL2", DbType.AnsiStringFixedLength);
            param[5].Value = selVal2;
            param[6] = new EacParameter("P_SELVAL3", DbType.AnsiStringFixedLength);
            param[6].Value = selVal3;
            param[7] = new EacParameter("P_SELVAL4", DbType.AnsiStringFixedLength);
            param[7].Value = selVal4;
            param[8] = new EacParameter("P_SELVAL5", DbType.AnsiStringFixedLength);
            param[8].Value = selVal5;
            param[9] = new EacParameter("P_SELVAL6", DbType.AnsiStringFixedLength);
            param[9].Value = selVal6;
            param[10] = new EacParameter("P_SELVAL7", DbType.AnsiStringFixedLength);
            param[10].Value = selVal7;
            param[11] = new EacParameter("P_SELVAL8", DbType.AnsiStringFixedLength);
            param[11].Value = selVal8;
            param[12] = new EacParameter("P_SELVAL9", DbType.AnsiStringFixedLength);
            param[12].Value = selVal9;
            param[13] = new EacParameter("P_SELVAL10", DbType.AnsiStringFixedLength);
            param[13].Value = selVal10;
            param[14] = new EacParameter("P_CHARSEPFIELD", DbType.AnsiStringFixedLength);
            param[14].Value = "|";
            param[15] = new EacParameter("P_QUERY", DbType.AnsiStringFixedLength);
            param[15].Value = "";
            param[15].Direction = ParameterDirection.InputOutput;
            param[15].Size = 8000;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEVALGRILLE", param);
        }

        /// <summary>
        /// Charge la liste des valeurs
        /// </summary>
        public static GrilleDto LoadValeurs(string codeGrille, string idMere, string niveau)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeGrille", DbType.AnsiStringFixedLength);
            param[0].Value = codeGrille;
            param[1] = new EacParameter("idMere", DbType.AnsiStringFixedLength);
            param[1].Value = idMere;
            param[2] = new EacParameter("niveau", DbType.AnsiStringFixedLength);
            param[2].Value = GetTypoNiveauSup(codeGrille, idMere != "0" ? Convert.ToInt32(niveau) + 1 : Convert.ToInt32(niveau));

            GrilleDto model = new GrilleDto { Nomenclatures = new List<NomenclatureDto>() };
            string sql = @"SELECT KHIID NOMENCLATUREID, KHINMC NOMENCLATURECODE, KHIDESI NOMENCLATURELIB, KHINORD NOMENCLATUREORDRE,
                                            IFNULL((SELECT COUNT(*) FROM KNMVALF
                                                        LEFT JOIN KNMGRI ON KHKNMG = KHJNMG
                                                      WHERE (T1.KHIID = KHKKHIID AND T1.KHITYPO = KHKTYPO AND KHKNMG = :codeGrille AND KHKMER = :idMere)), 0) IDVALEUR
                                                FROM KNMREF T1
                                            WHERE KHITYPO = :niveau ";

            var result = DbBase.Settings.ExecuteList<NomenclatureDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
                model.Nomenclatures = result;
            return model;
        }

        /// <summary>
        /// Charge la liste déroulante du niveau supérieur
        /// </summary>
        public static TypologieDto LoadListValeurs(string codeGrille, string idMere, string niveau)
        {
            string typoSup = GetTypoNiveauSup(codeGrille, idMere != "0" ? Convert.ToInt32(niveau) + 1 : Convert.ToInt32(niveau));
            TypologieDto model = new TypologieDto
            {
                Valeurs = LoadValeursGrilleTypo(typoSup, codeGrille, Convert.ToInt32(niveau) + 1, Convert.ToInt32(idMere)),
                Niveau = Convert.ToInt32(niveau) + 1,
                Code = typoSup,
                Lien = GetLienNiveauSup(codeGrille, idMere != "0" ? Convert.ToInt32(niveau) + 1 : Convert.ToInt32(niveau))
            };

            return model;
        }

        public static TypologieDto ReloadListValeurs(string codeGrille, string idMere, string niveau)
        {
            TypologieDto model = new TypologieDto
            {
                Valeurs = LoadValeursGrilleTypo(GetTypoNiveauSup(codeGrille, Convert.ToInt32(niveau)), codeGrille, Convert.ToInt32(niveau), Convert.ToInt32(idMere)),
                Niveau = Convert.ToInt32(niveau),
                Code = GetTypoNiveauSup(codeGrille, Convert.ToInt32(niveau))
            };
            return model;
        }

        public static GrilleDto SearchValeurNomenclature(string codeGrille, string typologie, string idMere, string searchTerm)
        {
            GrilleDto model = new GrilleDto { Nomenclatures = new List<NomenclatureDto>() };
            string sql = string.Format(@"SELECT KHIID NOMENCLATUREID, KHINMC NOMENCLATURECODE, KHIDESI NOMENCLATURELIB, KHINORD NOMENCLATUREORDRE,
	                                        IFNULL((SELECT COUNT(*) FROM KNMVALF
	                                                LEFT JOIN KNMGRI ON KHKNMG = KHJNMG AND KHJNMG = KHKNMG
	                                            WHERE (T1.KHIID = KHKKHIID AND T1.KHITYPO = KHKTYPO AND KHKNMG = '{0}' AND KHKMER = {2})), 0) IDVALEUR
	                                            FROM KNMREF T1
	                                            WHERE KHITYPO = '{1}'", codeGrille, typologie, idMere);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                sql += string.Format(" AND (UPPER(KHINMC) LIKE '%{0}%' OR UPPER(KHIDESI) LIKE '%{0}%')", searchTerm.ToUpper());
            }

            sql += " ORDER BY KHINORD";

            var result = DbBase.Settings.ExecuteList<NomenclatureDto>(CommandType.Text, sql);
            if (result != null && result.Any())
                model.Nomenclatures = result;
            return model;
        }

        #endregion

        #region Méthodes privées

        private static List<ParametreDto> GetListTypologie()
        {
            string sql = @"SELECT DISTINCT TCOD CODE, TPLIB LIBELLE, TPLIL DESCRIPTIF FROM YYYYPAR INNER JOIN KNMREF ON TCOD = KHITYPO WHERE TCON = 'KHEOP' AND TFAM = 'NMTYP'";
            return !string.IsNullOrEmpty(sql) ? DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql) : null;
        }

        private static List<TypologieDto> LoadTypologiesGrille(GrillePlatDto grille, string niveau)
        {
            List<TypologieDto> model = new List<TypologieDto>();

            switch (Convert.ToInt32(niveau))
            {
                #region Niveau 1
                case 1:
                    model.Add(new TypologieDto { Code = grille.Typologie1, Libelle = grille.LibTypologie1, Lien = grille.LienTypologie1, Niveau = 1, Valeurs = LoadValeursGrilleTypo(grille.Typologie1, grille.Code, 1) });
                    if (grille.LienTypologie1 != "I" && !string.IsNullOrEmpty(grille.LienTypologie2) && grille.LienTypologie2 != "I" && Convert.ToInt32(grille.LienTypologie2) > Convert.ToInt32(grille.LienTypologie1))
                    {
                        model.Add(new TypologieDto { Code = grille.Typologie2, Libelle = grille.LibTypologie2, Lien = grille.LienTypologie2, Niveau = 2, Valeurs = new List<ParametreDto>() });
                        if (!string.IsNullOrEmpty(grille.LienTypologie3) && grille.LienTypologie3 != "I" && Convert.ToInt32(grille.LienTypologie3) > Convert.ToInt32(grille.LienTypologie2))
                        {
                            model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = new List<ParametreDto>() });
                            if (!string.IsNullOrEmpty(grille.LienTypologie4) && grille.LienTypologie4 != "I" && Convert.ToInt32(grille.LienTypologie4) > Convert.ToInt32(grille.LienTypologie3))
                            {
                                model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = new List<ParametreDto>() });
                                if (!string.IsNullOrEmpty(grille.LienTypologie5) && grille.LienTypologie5 != "I" && Convert.ToInt32(grille.LienTypologie5) > Convert.ToInt32(grille.LienTypologie4))
                                {
                                    model.Add(new TypologieDto { Code = grille.Typologie5, Libelle = grille.LibTypologie5, Lien = grille.LienTypologie5, Niveau = 5, Valeurs = new List<ParametreDto>() });
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region Niveau 2
                case 2:
                    if (grille.LienTypologie2 == "I")
                    {
                        model.Add(new TypologieDto { Code = grille.Typologie2, Libelle = grille.LibTypologie2, Lien = grille.LienTypologie2, Niveau = 2, Valeurs = LoadValeursGrilleTypo(grille.Typologie2, grille.Code, 2) });
                    }
                    else
                    {
                        if (grille.LienTypologie1 != "I" && Convert.ToInt32(grille.LienTypologie2) > Convert.ToInt32(grille.LienTypologie1))
                        {
                            model.Add(new TypologieDto { Code = grille.Typologie1, Libelle = grille.LibTypologie1, Lien = grille.LienTypologie1, Niveau = 1, Valeurs = LoadValeursGrilleTypo(grille.Typologie1, grille.Code, 1) });
                            model.Add(new TypologieDto { Code = grille.Typologie2, Libelle = grille.LibTypologie2, Lien = grille.LienTypologie2, Niveau = 2, Valeurs = new List<ParametreDto>() });
                        }
                        else
                        {
                            model.Add(new TypologieDto { Code = grille.Typologie2, Libelle = grille.LibTypologie2, Lien = grille.LienTypologie2, Niveau = 2, Valeurs = LoadValeursGrilleTypo(grille.Typologie2, grille.Code, 2) });
                        }
                        if (!string.IsNullOrEmpty(grille.LienTypologie3) && grille.LienTypologie3 != "I" && Convert.ToInt32(grille.LienTypologie3) > Convert.ToInt32(grille.LienTypologie2))
                        {
                            model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = new List<ParametreDto>() });
                            if (!string.IsNullOrEmpty(grille.LienTypologie4) && grille.LienTypologie4 != "I" && Convert.ToInt32(grille.LienTypologie4) > Convert.ToInt32(grille.LienTypologie3))
                            {
                                model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = new List<ParametreDto>() });
                                if (!string.IsNullOrEmpty(grille.LienTypologie5) && grille.LienTypologie5 != "I" && Convert.ToInt32(grille.LienTypologie5) > Convert.ToInt32(grille.LienTypologie4))
                                {
                                    model.Add(new TypologieDto { Code = grille.Typologie5, Libelle = grille.LibTypologie5, Lien = grille.LienTypologie5, Niveau = 5, Valeurs = new List<ParametreDto>() });
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region Niveau 3
                case 3:
                    if (grille.LienTypologie3 == "I")
                    {
                        model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = LoadValeursGrilleTypo(grille.Typologie3, grille.Code, 3) });
                    }
                    else
                    {
                        if (grille.LienTypologie2 != "I" && Convert.ToInt32(grille.LienTypologie3) > Convert.ToInt32(grille.LienTypologie2))
                        {
                            if (grille.LienTypologie1 != "I" && Convert.ToInt32(grille.LienTypologie2) > Convert.ToInt32(grille.LienTypologie1))
                            {
                                model.Add(new TypologieDto { Code = grille.Typologie1, Libelle = grille.LibTypologie1, Lien = grille.LienTypologie1, Niveau = 1, Valeurs = LoadValeursGrilleTypo(grille.Typologie1, grille.Code, 1) });
                                model.Add(new TypologieDto { Code = grille.Typologie2, Libelle = grille.LibTypologie2, Lien = grille.LienTypologie2, Niveau = 2, Valeurs = new List<ParametreDto>() });
                                model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = new List<ParametreDto>() });
                            }
                            else
                            {
                                model.Add(new TypologieDto { Code = grille.Typologie2, Libelle = grille.LibTypologie2, Lien = grille.LienTypologie2, Niveau = 2, Valeurs = LoadValeursGrilleTypo(grille.Typologie2, grille.Code, 2) });
                                model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = new List<ParametreDto>() });
                            }
                        }
                        else
                        {
                            model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = LoadValeursGrilleTypo(grille.Typologie3, grille.Code, 3) });
                        }
                        if (!string.IsNullOrEmpty(grille.LienTypologie4) && grille.LienTypologie4 != "I" && Convert.ToInt32(grille.LienTypologie4) > Convert.ToInt32(grille.LienTypologie3))
                        {
                            model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = new List<ParametreDto>() });
                            if (!string.IsNullOrEmpty(grille.LienTypologie5) && grille.LienTypologie5 != "I" && Convert.ToInt32(grille.LienTypologie5) > Convert.ToInt32(grille.LienTypologie4))
                            {
                                model.Add(new TypologieDto { Code = grille.Typologie5, Libelle = grille.LibTypologie5, Lien = grille.LienTypologie5, Niveau = 5, Valeurs = new List<ParametreDto>() });
                            }
                        }
                    }
                    break;
                #endregion
                #region Niveau 4
                case 4:
                    if (grille.LienTypologie4 == "I")
                    {
                        model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = LoadValeursGrilleTypo(grille.Typologie4, grille.Code, 4) });
                    }
                    else
                    {
                        if (grille.LienTypologie3 != "I" && Convert.ToInt32(grille.LienTypologie4) > Convert.ToInt32(grille.LienTypologie3))
                        {
                            if (grille.LienTypologie2 != "I" && Convert.ToInt32(grille.LienTypologie3) > Convert.ToInt32(grille.LienTypologie2))
                            {
                                if (grille.LienTypologie1 != "I" && Convert.ToInt32(grille.LienTypologie2) > Convert.ToInt32(grille.LienTypologie1))
                                {
                                    model.Add(new TypologieDto { Code = grille.Typologie1, Libelle = grille.LibTypologie1, Lien = grille.LienTypologie1, Niveau = 1, Valeurs = LoadValeursGrilleTypo(grille.Typologie1, grille.Code, 1) });
                                    model.Add(new TypologieDto { Code = grille.Typologie2, Libelle = grille.LibTypologie2, Lien = grille.LienTypologie2, Niveau = 2, Valeurs = new List<ParametreDto>() });
                                    model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = new List<ParametreDto>() });
                                    model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = new List<ParametreDto>() });
                                }
                                else
                                {
                                    model.Add(new TypologieDto { Code = grille.Typologie2, Libelle = grille.LibTypologie2, Lien = grille.LienTypologie2, Niveau = 2, Valeurs = LoadValeursGrilleTypo(grille.Typologie2, grille.Code, 2) });
                                    model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = new List<ParametreDto>() });
                                    model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = new List<ParametreDto>() });
                                }
                            }
                            else
                            {
                                model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = LoadValeursGrilleTypo(grille.Typologie3, grille.Code, 3) });
                                model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = new List<ParametreDto>() });
                            }
                        }
                        else
                        {
                            model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = LoadValeursGrilleTypo(grille.Typologie4, grille.Code, 4) });
                        }
                        if (!string.IsNullOrEmpty(grille.LienTypologie5) && grille.LienTypologie5 != "I" && Convert.ToInt32(grille.LienTypologie4) < Convert.ToInt32(grille.LienTypologie5))
                        {
                            model.Add(new TypologieDto { Code = grille.Typologie5, Libelle = grille.LibTypologie5, Lien = grille.LienTypologie5, Niveau = 5, Valeurs = new List<ParametreDto>() });
                        }
                    }
                    break;
                #endregion
                #region Niveau 5
                case 5:
                    if (grille.LienTypologie5 != "I" && grille.LienTypologie4 != "I" && Convert.ToInt32(grille.LienTypologie4) < Convert.ToInt32(grille.LienTypologie5))
                    {
                        if (grille.LienTypologie3 != "I" && Convert.ToInt32(grille.LienTypologie4) > Convert.ToInt32(grille.LienTypologie3))
                        {
                            if (grille.LienTypologie2 != "I" && Convert.ToInt32(grille.LienTypologie3) > Convert.ToInt32(grille.LienTypologie2))
                            {
                                if (grille.LienTypologie1 != "I" && Convert.ToInt32(grille.LienTypologie2) > Convert.ToInt32(grille.LienTypologie1))
                                {
                                    model.Add(new TypologieDto { Code = grille.Typologie1, Libelle = grille.LibTypologie1, Lien = grille.LienTypologie1, Niveau = 1, Valeurs = LoadValeursGrilleTypo(grille.Typologie1, grille.Code, 1) });
                                    model.Add(new TypologieDto { Code = grille.Typologie2, Libelle = grille.LibTypologie2, Lien = grille.LienTypologie2, Niveau = 2, Valeurs = new List<ParametreDto>() });
                                    model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = new List<ParametreDto>() });
                                    model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = new List<ParametreDto>() });
                                    model.Add(new TypologieDto { Code = grille.Typologie5, Libelle = grille.LibTypologie5, Lien = grille.LienTypologie5, Niveau = 5, Valeurs = new List<ParametreDto>() });
                                }
                                else
                                {
                                    model.Add(new TypologieDto { Code = grille.Typologie2, Libelle = grille.LibTypologie2, Lien = grille.LienTypologie2, Niveau = 2, Valeurs = LoadValeursGrilleTypo(grille.Typologie2, grille.Code, 2) });
                                    model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = new List<ParametreDto>() });
                                    model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = new List<ParametreDto>() });
                                    model.Add(new TypologieDto { Code = grille.Typologie5, Libelle = grille.LibTypologie5, Lien = grille.LienTypologie5, Niveau = 5, Valeurs = new List<ParametreDto>() });
                                }
                            }
                            else
                            {
                                model.Add(new TypologieDto { Code = grille.Typologie3, Libelle = grille.LibTypologie3, Lien = grille.LienTypologie3, Niveau = 3, Valeurs = LoadValeursGrilleTypo(grille.Typologie3, grille.Code, 3) });
                                model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = new List<ParametreDto>() });
                                model.Add(new TypologieDto { Code = grille.Typologie5, Libelle = grille.LibTypologie5, Lien = grille.LienTypologie5, Niveau = 5, Valeurs = new List<ParametreDto>() });
                            }
                        }
                        else
                        {
                            model.Add(new TypologieDto { Code = grille.Typologie4, Libelle = grille.LibTypologie4, Lien = grille.LienTypologie4, Niveau = 4, Valeurs = LoadValeursGrilleTypo(grille.Typologie4, grille.Code, 4) });
                            model.Add(new TypologieDto { Code = grille.Typologie5, Libelle = grille.LibTypologie5, Lien = grille.LienTypologie5, Niveau = 5, Valeurs = new List<ParametreDto>() });
                        }
                    }
                    else
                    {
                        model.Add(new TypologieDto { Code = grille.Typologie5, Libelle = grille.LibTypologie5, Lien = grille.LienTypologie5, Niveau = 5, Valeurs = LoadValeursGrilleTypo(grille.Typologie5, grille.Code, 5) });
                    }
                    break;
                #endregion
            }

            return model;
        }

        private static List<NomenclatureDto> LoadNomenclatureValeur(GrillePlatDto grille, string niveau, string searchTerm)
        {
            string sql = string.Empty;
            string searchTypo = GetTypologie(grille, niveau);

            sql = string.Format(@"SELECT KHIID NOMENCLATUREID, KHINMC NOMENCLATURECODE, KHIDESI NOMENCLATURELIB, KHINORD NOMENCLATUREORDRE,
                                    IFNULL((SELECT COUNT(*) FROM KNMVALF
                                            LEFT JOIN KNMGRI ON KHKNMG = KHJNMG AND KHJNMG = KHKNMG
                                        WHERE (T1.KHIID = KHKKHIID AND T1.KHITYPO = KHKTYPO AND KHKNMG = '{0}')), 0) IDVALEUR
                                        FROM KNMREF T1
                                        WHERE KHITYPO = '{1}'", grille.Code, searchTypo);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                sql += string.Format(" AND (UPPER(KHINMC) LIKE '%{0}%' OR UPPER(KHIDESI) LIKE '%{0}%')", searchTerm.ToUpper());
            }

            sql += " ORDER BY KHINORD";

            var result = DbBase.Settings.ExecuteList<NomenclatureDto>(CommandType.Text, sql);
            return result != null && result.Any() ? result : new List<NomenclatureDto>();
        }

        private static string GetTypologie(GrillePlatDto grille, string niveau)
        {
            string searchTypo = string.Empty;

            switch (Convert.ToInt32(niveau))
            {
                case 1:
                    searchTypo = grille.Typologie1;
                    break;
                case 2:
                    searchTypo = grille.Typologie2;
                    break;
                case 3:
                    searchTypo = grille.Typologie3;
                    break;
                case 4:
                    searchTypo = grille.Typologie4;
                    break;
                case 5:
                    searchTypo = grille.Typologie5;
                    break;
            }
            return searchTypo;
        }

        private static List<CibleDto> GetCibleGrille(string idGrille)
        {
            string sql = string.Format(@"SELECT KAHCIBLE CODE, KAHDESC DESCRIPTION, TCOD CODEBRANCHE, TPLIB LIBBRANCHE  
                                            FROM KCIBLE
		                                        LEFT JOIN KCIBLEF ON KAIKAHID = KAHID
		                                        LEFT JOIN YYYYPAR ON TCON = 'GENER' AND TFAM = 'BRCHE' AND TCOD = KAIBRA 
                                            WHERE UPPER(KAHNMG) = '{0}'
                                            ORDER BY KAHCIBLE", idGrille.ToUpper());
            var result = DbBase.Settings.ExecuteList<CibleDto>(CommandType.Text, sql);
            return result != null && result.Any() ? result : new List<CibleDto>();
        }

        private static List<ParametreDto> LoadValeursGrilleTypo(string typo, string codeGrille, int niveau, int niveauMere = 0)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codegrille", DbType.AnsiStringFixedLength);
            param[0].Value = codeGrille;
            param[1] = new EacParameter("typo", DbType.AnsiStringFixedLength);
            param[1].Value = typo;
            param[2] = new EacParameter("niveau", DbType.AnsiStringFixedLength);
            param[2].Value = niveau;
            param[3] = new EacParameter("niveauMere", DbType.AnsiStringFixedLength);
            param[3].Value = niveauMere;

            string sql = @"SELECT KHKID LONGID, KHINMC CODE, KHIDESI LIBELLE       
	                                        FROM KNMVALF
		                                        INNER JOIN KNMREF ON KHKKHIID = KHIID AND KHKTYPO = KHITYPO
	                                        WHERE KHKNMG = :codeGrille AND KHKTYPO = :typo AND KHKNIV = :niveau AND KHKMER = :niveauMere
	                                        ORDER BY KHINORD";
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);
        }

        private static string GetTypoNiveauSup(string codeGrille, int niveau)
        {

            string sql = string.Format(@"SELECT KHJTYPO{0} STRRETURNCOL FROM KNMGRI WHERE KHJNMG = '{1}'", niveau > 0 ? niveau : 1, codeGrille);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);

            return result != null && result.Any() ? result.FirstOrDefault().StrReturnCol : string.Empty;
        }

        private static string GetLienNiveauSup(string codeGrille, int niveau)
        {
            EacParameter[] param = new EacParameter[1];      
            param[0] = new EacParameter("codegrille", DbType.AnsiStringFixedLength);
            param[0].Value = codeGrille;

            string sql = string.Format(@"SELECT KHJLIEN{0} STRRETURNCOL FROM KNMGRI WHERE KHJNMG = :codeGrille", niveau > 0 ? niveau : 1);
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            return result != null && result.Any() ? result.FirstOrDefault().StrReturnCol : string.Empty;
        }

        #endregion

        #endregion

    }
}
