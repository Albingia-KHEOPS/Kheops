using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Helpers;
using System.Runtime.Caching;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Data.Common;
using System.Data.EasycomClient;
using ALBINGIA.Framework.Common.Data;

namespace OP.DataAccess
{
    public class ReferenceRepository
    {    
        private static ParametreDto InitialiserCode(DataRow ligne, string nomChamp)
        {
            ParametreDto parametre = new ParametreDto();
            if (ligne.Table.Columns.Contains(nomChamp)) { parametre.Code = ligne[nomChamp].ToString(); };
            return parametre;
        }

        public static List<ParametreDto> RechercherParametres(string branche, string cible, string TCON, string TFAM, string TPCA1 = null, List<String> TCOD = null)
        {
            string sql = CommonRepository.BuildSelectYYYYPAR(branche, cible, string.Empty, "TCOD CODE, TPLIB LIBELLE", TCON, TFAM, otherCriteria: TPCA1 != null ? " AND TPCA1 = '" + TPCA1 + "'" : string.Empty, tCod: TCOD, notIn: true);
            //sql += " ORDER BY lower(TPLIB)";
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql).OrderBy(m => m.Libelle).ToList();

        }

        private static ParametreDto InitialiserParamDto(DataRow ligne)
        {
            ParametreDto Parametre = null;
            if (OutilsHelper.ContientLeChampEtEstNonNull(ligne, "TCOD"))
            {
                Parametre = new ParametreDto();
                if (ligne.Table.Columns.Contains("TCOD")) { Parametre.Code = ligne["TCOD"].ToString(); };
                if (ligne.Table.Columns.Contains("TPLIB")) { Parametre.Libelle = ligne["TPLIB"].ToString(); };
            }
            return Parametre;
        }

        public static List<ParametreDto> RechercherTypesInventaire()
        {
            List<ParametreDto> result = new List<ParametreDto>();
            string sql = "SELECT KAGTYINV CODE, KAGDESC LIBELLE FROM KINVTYP ORDER BY KAGID, KAGTYINV";
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);

        }

        public static List<ParametreDto> RechercherTypesInventaire(string branche, string cible)
        {
            List<ParametreDto> result = new List<ParametreDto>();
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("P_BRANCHE", DbType.AnsiStringFixedLength);
            param[0].Value = branche;
            param[1] = new EacParameter("P_CIBLE", DbType.AnsiStringFixedLength);
            param[1].Value = cible;

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.StoredProcedure, "SP_GETTYPEINVENTAIRE", param);

        }

        public static ParametreDto InitialiserDevise(DataRow ligne)
        {
            return InitialiserCode(ligne, "PBDEV");
        }

        public static ParametreDto InitialiserPeriodicite(DataRow ligne)
        {
            return InitialiserCode(ligne, "PBPER");
        }

        public static ParametreDto InitialiserUniteTemps(DataRow ligne)
        {
            return InitialiserCode(ligne, "PBCTU");
        }

        public static ParametreDto InitialiserIndiceReference(DataRow ligne)
        {
            return InitialiserCode(ligne, "JDIND");
        }

        public static ParametreDto InitialiserNatureContrat(DataRow ligne)
        {
            var natureContrat = InitialiserCode(ligne, "PBNPL");
            if (ligne.Table.Columns.Contains("LIBELLENATURECONTRAT"))
                natureContrat.Libelle = ligne["LIBELLENATURECONTRAT"].ToString();
            return natureContrat;
        }

        public static List<ParametreDto> ObtenirTypeInventaire()
        {
            return RechercherTypesInventaire();
        }

        public static List<ParametreDto> ObtenirTypeInventaire(string branche, string cible)
        {
            return RechercherTypesInventaire(branche, cible);
        }
    }
}
