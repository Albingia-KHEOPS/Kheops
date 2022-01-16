using System.Collections.Generic;
using DataAccess.Helpers;
using System.Data;
using System;
using System.Data.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.FormuleGarantie.GarantieModele;
using System.Linq;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;

namespace OP.DataAccess
{
    public class GarantieModeleRepository : RepositoryBase
    {
        internal static readonly string SelectCountByDEFG = "SELECT COUNT(*) FROM KPGARAN WHERE KDEIPB = :codeAffaire AND KDEALX = :version AND KDETYP = :type AND KDEDEFG = :DEFG ;";
        internal static readonly string sqlInsertGarantieType = @"INSERT INTO YPLTGAR (C2SEQ, C2MGA, C2NIV, C2GAR, C2ORD, C2SE1, C2SEM, C2CAR, C2NAT, C2INA, C2CNA, C2TAX, C2ALT, 
            C2SCR, C2PRP, C2TCD, C2MRF, C2NTM, C2MAS, C2TRI)
            VALUES ({0}, '{1}', {2}, '{3}', {4}, {5}, {6}, '{7}', '{8}', '{9}', '{10}', '{11}', {12},
            '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}')";
        internal static readonly string sqlInsertYPLTGAL = @"INSERT INTO YPLTGAL (C4SEQ, C4TYP, C4BAS, C4UNT, C4VAL, C4MAJ, C4OBL, C4ALA)
            VALUES (:seq, :type, :base, :unite, :valeur, :modi, :obl, :alim)";
        internal static readonly string sqlUpdateGarantieType = @"UPDATE YPLTGAR SET C2ORD = {0}, C2CAR = '{1}', C2NAT = '{2}', C2INA = '{3}', 
            C2CNA = '{4}', C2TAX = '{5}', C2ALT = {6}, C2PRP = '{7}', C2TCD = '{8}', C2MRF = '{9}', C2NTM = '{10}', C2MAS = '{11}', C2TRI = '{12}'
            WHERE C2SEQ = {13}";
        internal static readonly string sqlUpdateYPLTGAL = "UPDATE YPLTGAL SET C4BAS = :base, C4UNT = :unite, C4VAL = :valeur, C4MAJ = :modi, C4OBL = :obl, C4ALA = :alim WHERE C4SEQ = :seq AND C4TYP = :type";

        public GarantieModeleRepository(IDbConnection connection) : base(connection) { }

        #region Méthode Publique

        public static string EnregistrerModeleByCible(string codeId, string codeIdBloc, string dateApp, string codeTypo, string codeModele, string user)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM KCATMODELE WHERE KARMODELE = '{0}' AND KARDATEAPP = {1} AND KARTYPO = '{2}' AND KARKAQID = {3}", codeModele, AlbConvert.ConvertDateToInt(Convert.ToDateTime(dateApp)), codeTypo, codeIdBloc);
            if (CommonRepository.ExistRow(sql))
                return "Ce modèle est déjà associé.";
            sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM KCATMODELE WHERE KARDATEAPP = {0} AND KARTYPO = '{1}' AND KARKAQID = {2}", AlbConvert.ConvertDateToInt(Convert.ToDateTime(dateApp)), codeTypo, codeIdBloc);
            if (CommonRepository.ExistRow(sql))
                return "Il existe un autre modèle avec le même typo et date d'application.";

            if (!string.IsNullOrEmpty(codeId))
                UpdateModeleByCible(codeId, dateApp, codeTypo, codeModele, user);
            else
                InsertModeleByCible(codeIdBloc, dateApp, codeTypo, codeModele, user);

            return string.Empty;
        }

        public static List<GarantieModeleDto> RechercherGarantieModelesByBloc(string codeId)
        {
            string sql = string.Format(@"SELECT KARID GUID, KARMODELE CODE, D1LIB DESCRIPTION, KARDATEAPP DATEAPPLI, KARTYPO TYPOLOGIE
                        FROM KCATMODELE
                            INNER JOIN YPLMGA ON KARMODELE = D1MGA
                        WHERE KARKAQID = '{0}'
                        ORDER BY KARDATEAPP DESC", !string.IsNullOrEmpty(codeId) ? codeId.Replace("'", "''") : string.Empty);
            var garantieModelePlatDto = DbBase.Settings.ExecuteList<GarantieModelePlatDto>(CommandType.Text, sql);
            return GetListGarantieModele(garantieModelePlatDto);
        }

        public static List<GarantieModeleDto> RechercherGarantieModele(string code, string description)
        {
            if (description == null) description = string.Empty;

            string sql = string.Format(@"SELECT D1MGA CODE, D1LIB DESCRIPTION
                        FROM YPLMGA
                        WHERE UPPER(D1LIB) LIKE '{0}'", "%" + description.ToUpper().Replace("'", "''") + "%");
            if (!string.IsNullOrEmpty(code))
                sql += string.Format(@" AND UPPER(D1MGA) LIKE '{0}' ", "%" + code.ToUpper().Replace("'", "''") + "%");
            sql += " ORDER BY D1MGA ";
            var garantieModelePlatDto = DbBase.Settings.ExecuteList<GarantieModelePlatDto>(CommandType.Text, sql);
            return GetListGarantieModele(garantieModelePlatDto);
        }

        public static List<GarantieModeleDto> ObtenirModeles()
        {
            string sql = @"SELECT D1MGA CODE, D1LIB DESCRIPTION FROM YPLMGA ORDER BY D1MGA";
            var garantieModelePlatDto = DbBase.Settings.ExecuteList<GarantieModelePlatDto>(CommandType.Text, sql);
            return GetListGarantieModele(garantieModelePlatDto);
        }

        /// <summary>
        /// Récupère les informations du modèle de garantie
        /// et les informations des branches associées
        /// </summary>
        public static GarantieModeleDto GetGarantieModele(string code)
        {
            var model = new GarantieModeleDto();
            string sql = string.Format(@"SELECT 0 GUID, D1MGA CODE, D1LIB DESCRIPTION, '' BRANCHE
                                                        , '' CIBLE, '' VOLET, '' BLOC, '' TYPOLOGIE 
                                                         FROM  YPLMGA G1
                                                        WHERE
                                                        NOT EXISTS
                                                        (
                                                        SELECT * FROM  YPLMGA D2
                                                        INNER JOIN KCATMODELE ON KARMODELE=D2.D1MGA 
                                                        INNER JOIN KCATBLOC ON KARKAQID = KAQID 
                                                        WHERE G1.D1MGA=D2.D1MGA
                                                        )
                                                        AND G1.D1MGA = '{0}'
                                                        UNION
                                                        SELECT KARID GUID, KARMODELE CODE, D1LIB DESCRIPTION, KAQBRA BRANCHE
                                                        , KAQCIBLE CIBLE, KAQVOLET VOLET, KAQBLOC BLOC, KARTYPO TYPOLOGIE 
                                                                                        FROM YPLMGA 
                                                        INNER JOIN KCATMODELE ON KARMODELE=D1MGA 
                                                        INNER JOIN KCATBLOC ON KARKAQID = KAQID
                                                        WHERE D1MGA = '{0}'", code);

            var result = DbBase.Settings.ExecuteList<GarantieModelePlatDto>(CommandType.Text, sql);

            if (result != null && result.Count > 0)
            {
                model = new GarantieModeleDto
                {
                    GuidId = result[0].Guid.ToString(),
                    Code = result[0].Code,
                    Description = result[0].Description
                };
                result.ForEach(g =>
                {
                    if (!string.IsNullOrEmpty(g.Branche))
                        model.LstModeleGarantie.Add(new ModeleGarantieDto
                        {
                            Branche = g.Branche,
                            Cible = g.Cible,
                            Volet = g.Volet,
                            Bloc = g.Bloc,
                            Typologie = g.Typologie
                        });
                });
            }

            return model;
        }

        /// <summary>
        /// Vérifie si le code fourni est enregistré en base
        /// </summary>
        public static bool ExistCodeModele(string code)
        {
            string sqlExist = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPLMGA 
                                WHERE D1MGA = '{0}'", code);
            return CommonRepository.ExistRow(sqlExist);
        }

        /// <summary>
        /// Vérifie si le tri fourni est bien unique pour un modele de garantie
        /// </summary>
        public static bool ExistTri(long seq, string codeModele, string tri)
        {
            string sqlExist = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPLTGAR 
                                WHERE C2MGA = '{0}' AND C2TRI = '{1}' AND C2SEQ != {2}", codeModele, tri, seq);
            return CommonRepository.ExistRow(sqlExist);
        }

        /// <summary>
        /// Vérifie si le code garantie fourni est bien unique pour un niveau de modele
        /// </summary>
        public static bool ExistCodeGarantie(string codeModele, string codeGarantie, long seqM)
        {
            string sqlExist = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPLTGAR 
                                WHERE C2MGA = '{0}' AND C2GAR = '{1}' AND C2SEM = {2}", codeModele, codeGarantie, seqM);
            return CommonRepository.ExistRow(sqlExist);
        }

        /// <summary>
        /// Ajoute les informations du modèle de garantie
        /// </summary>
        public static void InsertGarantieModele(string code, string description)
        {
            string sql = string.Format(@"INSERT INTO YPLMGA (D1MGA, D1LIB) values ('{0}', '{1}')", code, description);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        /// <summary>
        /// Modifie les informations du modèle de garantie
        /// </summary>
        public static void UpdateGarantieModele(string code, string description)
        {
            string sql = string.Format(@"UPDATE YPLMGA SET D1LIB = '{0}' WHERE D1MGA = '{1}'", description, code);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        /// <summary>
        /// Copie les informations du modèle de garantie
        /// </summary>
        public static void CopierGarantieModele(string code, string codeCopie)
        {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("P_CODE", DbType.AnsiStringFixedLength);
            param[0].Value = code;
            param[1] = new EacParameter("P_CODECOPIE", DbType.AnsiStringFixedLength);
            param[1].Value = codeCopie;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_COPYMODELEGAR", param);
        }

        /// <summary>
        /// Supprime les informations du modèle de garantie
        /// et les liaisons aux blocs associés
        /// </summary>
        public static void SupprimerGarantieModele(string code, out string msgRetour)
        {
            var model = new GarantieModeleDto();
            var isSupprimable = !ExistDansContrat(code);
            if (isSupprimable)
            {
                string deleteSql = string.Format(@"DELETE FROM KCATMODELE WHERE KARMODELE = '{0}'", code);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, deleteSql);
                deleteSql = string.Format(@"DELETE FROM YPLMGA WHERE D1MGA = '{0}'", code);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, deleteSql);
                msgRetour = "";
            }
            else
            {
                msgRetour = string.Format(@"Le modèle {0} ne peut être supprimé car il est utilisé dans un ou plusieurs contrats", code);
                //throw new AlbFoncException(string.Format(@"Le modèle {0} ne peut être supprimé car il est utilisé dans un ou plusieurs contrats", code), false, true);
            }
        }

        public static bool ExistDansContrat(string code)
        {
            string sqlExist = string.Format(@"SELECT COUNT(*) NBLIGN FROM YPLMGA 
                                INNER JOIN KCATMODELE ON KARMODELE = D1MGA
                                INNER JOIN KPOPTD ON KARID = KDCKARID
                                WHERE D1MGA = '{0}'", code);
            return CommonRepository.ExistRow(sqlExist);
        }

        public static string SupprimerModeleByCategorie(string codeId, string infoUser)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P_PARAM", "CATMODELE");
            param[1] = new EacParameter("P_CODEPARAM", 0);
            param[1].Value = Convert.ToInt32(codeId);
            param[2] = new EacParameter("P_INFOUSER", infoUser);
            param[3] = new EacParameter("P_ERRORMSG", string.Empty);
            param[3].Direction = ParameterDirection.InputOutput;
            param[3].Size = 100;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETEPARAMASSOC", param);

            return param[3].Value.ToString();
        }

        #region old garantie type methods
        //public static List<GarantieTypeDto> RechercherGarantieType(string codeModele)
        //{
        //    string sql = string.Format(@"SELECT YPLTGAR.*, GADES DESCRIPTION, Y1.TPLIB C2CARLIB, Y2.TPLIB C2NATLIB
        //                FROM YPLTGAR
        //                INNER JOIN KGARAN ON GAGAR = C2GAR
        //                INNER JOIN YYYYPAR Y1 ON Y1.TCON = 'PRODU' AND Y1.TFAM = 'CBCAR' AND Y1.TCOD = C2CAR
        //                INNER JOIN YYYYPAR Y2 ON Y2.TCON = 'PRODU' AND Y2.TFAM = 'CBNAT' AND Y2.TCOD = C2NAT
        //                WHERE UPPER(C2MGA) = '{0}' ", codeModele.ToUpper().Replace("'", "''"));
            
        //    var garantieTypePlatDto = DbBase.Settings.ExecuteList<GarantieTypePlatDto>(CommandType.Text, sql);

        //    return GetListGarantieType(garantieTypePlatDto, !ExistDansContrat(codeModele));
        //}

        ///// <summary>
        ///// Récupère les informations du paramétrage garantie
        ///// </summary>
        //public static GarantieTypeDto GetGarantieType(long seq)
        //{
        //    var model = new GarantieTypeDto();
        //    string sql = string.Format(@"SELECT YR.*, GADES DESCRIPTION,
        //        C4TYP, C4BAS, C4VAL, C4UNT , C4MAJ, C4OBL, C4ALA
        //        FROM YPLTGAR YR
        //        INNER JOIN KGARAN ON GAGAR = C2GAR
        //        INNER JOIN YPLTGAL ON C2SEQ = C4SEQ 
        //        WHERE C2SEQ = {0}", seq);

        //    var result = DbBase.Settings.ExecuteList<GarantieTypePlatDto>(CommandType.Text, sql);

        //    if (result != null && result.Count > 0)
        //    {
        //        model = new GarantieTypeDto(result, !ExistDansContrat(result.FirstOrDefault().CodeModele));
        //    }

        //    return model;
        //}

        ///// <summary>
        ///// Récupère les informations du paramétrage garantie lien
        ///// </summary>
        //public static GarantieTypeDto GetGarantieTypeLien(long seq)
        //{
        //    var model = new GarantieTypeDto();
        //    string sql = string.Format(@"SELECT YR.*, GADES DESCRIPTION, YA.*, YR1.C2GAR GARLIEE
        //        FROM YPLTGAR YR
        //        INNER JOIN KGARAN ON GAGAR = YR.C2GAR
        //        INNER JOIN YPLTGAA YA ON (YR.C2SEQ = C5SEQ AND C5TYP IN ('A', 'I')) OR C5SEM = YR.C2SEQ
        //        INNER JOIN YPLTGAR YR1 ON (CASE WHEN C5SEQ = {0} THEN C5SEM ELSE C5SEQ END) = YR1.C2SEQ
        //        WHERE YR.C2SEQ = {0}", seq);

        //    var result = DbBase.Settings.ExecuteList<GarantieTypePlatDto>(CommandType.Text, sql);

        //    if (result != null && result.Count > 0)
        //    {
        //        model = new GarantieTypeDto(result, !ExistDansContrat(result.FirstOrDefault().CodeModele));
        //    }

        //    return model;
        //}

        ///// <summary>
        ///// Ajoute les informations de la garantie type
        ///// </summary>
        //public static void InsertGarantieType(GarantieTypeDto garType, out string msgRetour)
        //{
        //    msgRetour = "";
        //    int newSeq = CommonRepository.GetAS400IdYPLCHRO("YPLTGAR");
        //    if (newSeq > 0)
        //    {
        //        string sql = string.Format(sqlInsertGarantieType, newSeq, garType.CodeModele, garType.Niveau, garType.CodeGarantie, garType.Ordre, garType.NumeroSeq1, garType.NumeroSeqM,
        //            garType.Caractere, garType.Nature, garType.IsIndexee.ToYesNo(), garType.SoumisCATNAT.ToYesNo(), garType.CodeTaxe, garType.GroupeAlternative,
        //            "", garType.TypePrime, garType.TypeControleDate, garType.IsMontantRef.ToYesNo(), garType.IsNatureModifiable.ToYesNo(), garType.IsMasquerCP.ToYesNo(), garType.Tri);
        //        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

        //        foreach (var lci in garType.ListLCI)
        //        {
        //            var param = new DbParameter[8];
        //            param[0] = new EacParameter("seq", newSeq);
        //            param[1] = new EacParameter("type", lci.Type);
        //            param[2] = new EacParameter("base", lci.Base);
        //            param[3] = new EacParameter("unite", lci.Unite);
        //            param[4] = new EacParameter("valeur", lci.Valeur);
        //            param[5] = new EacParameter("modi", lci.Modi);
        //            param[6] = new EacParameter("obl", lci.Obl.ToYesNo());
        //            param[7] = new EacParameter("alim", lci.Alim);
        //            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlInsertYPLTGAL, param);
        //        }
        //    } else
        //    {
        //        msgRetour = "Le numéro de séquence est incorrect. Veuillez contacter un administrateur.";
        //    }
        //}

        ///// <summary>
        ///// Modifie les informations de la garantie type
        ///// </summary>
        //public static void UpdateGarantieType(GarantieTypeDto garType)
        //{
        //    // update garantie type
        //    string sql = string.Format(sqlUpdateGarantieType, garType.Ordre, garType.Caractere, garType.Nature, garType.IsIndexee.ToYesNo(),
        //    garType.SoumisCATNAT.ToYesNo(), garType.CodeTaxe, garType.GroupeAlternative, garType.TypePrime,
        //    garType.TypeControleDate, garType.IsMontantRef.ToYesNo(), garType.IsNatureModifiable.ToYesNo(),
        //    garType.IsMasquerCP.ToYesNo(), garType.Tri, garType.NumeroSeq);
        //    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        //    // update les lci
        //    foreach (var lci in garType.ListLCI)
        //    {
        //        var param = new DbParameter[8];
        //        param[0] = new EacParameter("base", lci.Base);
        //        param[1] = new EacParameter("unite", lci.Unite);
        //        param[2] = new EacParameter("valeur", lci.Valeur);
        //        param[3] = new EacParameter("modi", lci.Modi);
        //        param[4] = new EacParameter("obl", lci.Obl.ToYesNo());
        //        param[5] = new EacParameter("alim", lci.Alim);
        //        param[6] = new EacParameter("seq", garType.NumeroSeq);
        //        param[7] = new EacParameter("type", lci.Type);
        //        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdateYPLTGAL, param);
        //    }
        //}

        ///// <summary>
        ///// Modifie les informations de la garantie type
        ///// </summary>
        //public static void SupprimerGarantieType(long seq, out string msgRetour)
        //{
        //    msgRetour = "";
        //    if (seq > 0)
        //    {
        //        // récupérer les num de sequence des sous-garanties
        //        string sql = string.Format(@"SELECT C2SEQ FROM YPLTGAR WHERE C2SEQ = {0} 
        //            OR C2SEM = {0}
        //            OR C2SEM IN (SELECT C2SEQ FROM YPLTGAR WHERE C2SEM = {0} AND C2SEQ != 0)
        //            OR C2SE1 = {0}", seq);
        //        var result = DbBase.Settings.ExecuteList<GarantieTypePlatDto>(CommandType.Text, sql);
        //        string listSeq = string.Join(",", result.Select(x => x.NumeroSeq));

        //        string sqlDelete = string.Format(@"DELETE FROM YPLTGAR WHERE C2SEQ IN ({0})", listSeq);
        //        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDelete);
        //        sqlDelete = string.Format(@"DELETE FROM YPLTGAL WHERE C4SEQ IN ({0})", listSeq);
        //        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDelete);
        //    }
        //    else
        //    {
        //        msgRetour = "Le numéro de séquence est incorrect, impossible de supprimer la garantie. Veuillez contacter un administrateur.";
        //    }
        //}
        #endregion

        #endregion

        #region Méthode Privée

        private static void UpdateModeleByCible(string codeId, string dateApp, string codeTypo, string codeModele, string user)
        {
            var date = DateTime.Now;
            var param = new DbParameter[7];
            param[0] = new EacParameter("dateApp", AlbConvert.ConvertDateToInt(Convert.ToDateTime(dateApp)));
            param[1] = new EacParameter("codeTypo", codeTypo);
            param[2] = new EacParameter("codeModele", codeModele);
            param[3] = new EacParameter("updateUser", user);
            param[4] = new EacParameter("updateDate", 0);
            param[4].Value = AlbConvert.ConvertDateToInt(date);
            param[5] = new EacParameter("updateTime", 0);
            param[5].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
            param[6] = new EacParameter("codeId", 0);
            param[6].Value = Convert.ToInt32(codeId);

            string sql = @"UPDATE KCATMODELE SET KARDATEAPP = :dateApp, KARTYPO = :codeTypo, KARMODELE = :codeModele, KARMAJU = :updateUser, KARMAJD = :updateDate, KARMAJH = :updateTime
                            WHERE KARID = :codeId";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static void InsertModeleByCible(string codeIdBloc, string dateApp, string codeTypo, string codeModele, string user)
        {
            DateTime date = DateTime.Now;
            var param = new DbParameter[11];
            param[0] = new EacParameter("codeId", 0);
            param[0].Value = CommonRepository.GetAS400Id("KARID");
            param[1] = new EacParameter("codeIdBloc", codeIdBloc);
            param[2] = new EacParameter("dateApp", AlbConvert.ConvertDateToInt(Convert.ToDateTime(dateApp)));
            param[3] = new EacParameter("codeTypo", codeTypo);
            param[4] = new EacParameter("codeModele", codeModele);
            param[5] = new EacParameter("createUser", user);
            param[6] = new EacParameter("updateUser", user);
            param[7] = new EacParameter("updateDate", 0);
            param[7].Value = AlbConvert.ConvertDateToInt(date);
            param[8] = new EacParameter("updateTime", 0);
            param[8].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));
            param[9] = new EacParameter("updateDate", 0);
            param[9].Value = AlbConvert.ConvertDateToInt(date);
            param[10] = new EacParameter("updateTime", 0);
            param[10].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(date));

            string sql = @"INSERT INTO KCATMODELE 
                            (KARID, KARKAQID, KARDATEAPP, KARTYPO, KARMODELE, KARCRU, KARMAJU, KARCRD, KARCRH, KARMAJD, KARMAJH)
                                values
                            (:codeId, :codeIdBloc, :dateApp, :codeTypo, :codeModele, :createUser, :updateUser, :createDate, :createTime, :updateDate, :updateTime)";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static List<GarantieModeleDto> GetListGarantieModele(List<GarantieModelePlatDto> garantieModelePlatDto)
        {
            var toReturn = new List<GarantieModeleDto>();
            if (garantieModelePlatDto != null && garantieModelePlatDto.Any())
            {
                foreach (var garPlatDto in garantieModelePlatDto)
                {
                    GarantieModeleDto garantieModele = new GarantieModeleDto
                    {
                        GuidId = garPlatDto.Guid.ToString(),
                        Code = garPlatDto.Code,
                        Description = garPlatDto.Description,
                        DateAppli = AlbConvert.ConvertIntToDate(garPlatDto.DateAppli),
                        Typologie = garPlatDto.Typologie
                    };
                    var modeleGarantie = new ModeleGarantieDto
                    {
                        Bloc = garPlatDto.Bloc,
                        Branche = garPlatDto.Branche,
                        Cible = garPlatDto.Cible,
                        Typologie = garPlatDto.Typologie,
                        Volet = garPlatDto.Volet
                    };
                    garantieModele.LstModeleGarantie = new List<ModeleGarantieDto>();
                    garantieModele.LstModeleGarantie.Add(modeleGarantie);

                    toReturn.Add(garantieModele);
                }
            }
            return toReturn;
        }

        private static List<GarantieTypeDto> GetListGarantieType(List<GarantieTypePlatDto> listGarantieTypePlatDto, bool isModifiable = false, int niveau = 1, long sequence = 0)
        {
            var toReturn = new List<GarantieTypeDto>();
            //if (listGarantieTypePlatDto != null && listGarantieTypePlatDto.Any() && niveau < 5)
            //{
            //    foreach (var garPlatDto in listGarantieTypePlatDto.Where(gar => gar.Niveau == niveau && gar.NumeroSeqM == sequence))
            //    {
            //        GarantieTypeDto garantieType = new GarantieTypeDto
            //        {
            //            NumeroSeq = garPlatDto.NumeroSeq,
            //            CodeModele = garPlatDto.CodeModele,
            //            NomModele = garPlatDto.NomModele,
            //            Niveau = garPlatDto.Niveau,
            //            CodeGarantie = garPlatDto.CodeGarantie,
            //            Ordre = garPlatDto.Ordre,
            //            Description = garPlatDto.Description,
            //            NumeroSeqM = garPlatDto.NumeroSeqM,
            //            NumeroSeq1 = garPlatDto.NumeroSeq1,
            //            Caractere = garPlatDto.Caractere,
            //            CaractereLib = garPlatDto.CaractereLib,
            //            Nature = garPlatDto.Nature,
            //            NatureLib = garPlatDto.NatureLib,
            //            IsIndexee = garPlatDto.IsIndexee,
            //            SoumisCATNAT = garPlatDto.SoumisCATNAT,
            //            CodeTaxe = garPlatDto.CodeTaxe,
            //            GroupeAlternative = garPlatDto.GroupeAlternative,
            //            Conditionnement = garPlatDto.Conditionnement,
            //            TypePrime = garPlatDto.TypePrime,
            //            TypeControleDate = garPlatDto.TypeControleDate,
            //            IsMontantRef = garPlatDto.IsMontantRef,
            //            IsNatureModifiable = garPlatDto.IsNatureModifiable,
            //            IsMasquerCP = garPlatDto.IsMasquerCP,
            //            IsModifiable = isModifiable,
            //        };
            //        garantieType.ListSousGarantie = GetListGarantieType(listGarantieTypePlatDto, isModifiable, niveau + 1, garPlatDto.NumeroSeq);

            //        toReturn.Add(garantieType);
            //    }
            //}
            return toReturn.OrderBy(x => x.Ordre).ToList();
        }

        #endregion

        public int CountByDEFG(Folder folder, string definition)
        {
            using (var options = new DbCountOptions(this.connection == null)
            {
                DbConnection = this.connection,
                SqlText = SelectCountByDEFG
            })
            {
                options.BuildParameters(folder.CodeOffre.ToIPB(), folder.Version, folder.Type, definition);
                options.PerformCount();
                return options.Count;
            }
        }
    }
}


