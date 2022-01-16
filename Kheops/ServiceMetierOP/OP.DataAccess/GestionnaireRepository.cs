using System;
using System.Data;
using System.Linq;
using System.Globalization;
using OP.WSAS400.DTO.Personnes;
using System.Collections.Generic;
using DataAccess.Helpers;
using ALBINGIA.Framework.Common.Data;

namespace OP.DataAccess
{
    public class GestionnaireRepository
    {
        public static GestionnaireDto Initialiser(DataRow ligne)
        {
            GestionnaireDto Gestionnaire = null;
            if (OutilsHelper.ContientUnDesChampsEtEstNonNull(ligne, "PBGES", "UTIUT"))
            {
                Gestionnaire = new GestionnaireDto();
                if (ligne.Table.Columns.Contains("PBGES")) { Gestionnaire.Id = ligne["PBGES"].ToString().Trim(); } else if (ligne.Table.Columns.Contains("UTIUT")) { Gestionnaire.Id = ligne["UTIUT"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("UTPFX")) { Gestionnaire.Login = ligne["UTPFX"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("UTNOM")) { Gestionnaire.Nom = ligne["UTNOM"].ToString().Trim(); } else if (ligne.Table.Columns.Contains("GESNOM")) { Gestionnaire.Nom = ligne["GESNOM"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("UTPNM")) { Gestionnaire.Prenom = ligne["UTPNM"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("UTBRA")) { Gestionnaire.Branche = BrancheRepository.Initialiser(ligne); };
            }
            return Gestionnaire;
        }

        public static List<GestionnaireDto> ObtenirGestionnaires()
        {
            string sql = string.Format(@"SELECT UTIUT PBGES, 
                                                UTPFX, UTNOM, 
                                                UTPNM, UTBRA 
                                         FROM YUTILIS
                                         WHERE UTGEP = 'O'
                                         ORDER BY lower(UTNOM), lower(UTPNM)");
            var result = DbBase.Settings.ExecuteList<GestionnaireDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                #region récupération des branches
                foreach (var gestio in result)
                {
                    if (!string.IsNullOrEmpty(gestio.SBranche))
                    {
                       var brches = BrancheRepository.ObtenirBranches(gestio.SBranche, string.Empty);
                       if (brches != null && brches.Any())
                       {
                           var brch = brches.FirstOrDefault();
                           if (brch != null)
                           {
                               gestio.Branche = brch;
                           }
                       }
                    }
                }
                #endregion
            }
            return result;
        }

        public static bool TesterExistenceGestionnaire(string identifiantUtilisateur)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM YUTILIS
                                     WHERE UTIUT='{0}' AND UTGEP='{1}'", identifiantUtilisateur, "O");
            return CommonRepository.ExistRow(sql);
        }

        public static List<GestionnaireDto> RechercherGestionnaires(int debut, int fin, string nomGestionnaire)
        {
            string whereQuery = string.Empty;
            if (!string.IsNullOrEmpty(nomGestionnaire.Trim()))
                whereQuery = string.Format("  WHERE (UTNOM LIKE '{0}%' OR UTIUT LIKE '{0}%')", nomGestionnaire.Trim());

            string sql = String.Format(CultureInfo.CurrentCulture, @"
                SELECT * FROM (SELECT rownumber() OVER (ORDER BY UTNOM, UTIUT) AS ID_NEXT, UTIUT PBGES, UTPFX, UTNOM, UTPNM, UTBRA, BUDBU, UTGES
                    FROM YUTILIS
                    LEFT JOIN YBUREAU ON YUTILIS.UTBUR = YBUREAU.BUIBU
                    {0}
                    ORDER BY UTNOM, UTPNM) AS TABLE
                WHERE ID_NEXT BETWEEN {1} AND {2}", whereQuery, debut, fin);

            var result = DbBase.Settings.ExecuteList<GestionnaireDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                #region récupération des branches
                foreach (var gestio in result)
                {
                    if (!string.IsNullOrEmpty(gestio.SBranche))
                    {
                        var brches = BrancheRepository.ObtenirBranches(gestio.SBranche, string.Empty);
                        if (brches != null && brches.Any())
                        {
                            var brch = brches.FirstOrDefault();
                            if (brch != null)
                            {
                                gestio.Branche = new WSAS400.DTO.Offres.Branches.BrancheDto { Code = brch.Code, Nom = brch.Nom };
                            }
                        }
                    }
                }
                #endregion
            }
            return result;
        }

        public static GestionnaireDto ObtenirGestionnaire(string nomGestionnaire)
        {
            if (!string.IsNullOrEmpty(nomGestionnaire))
            {
                string sql = String.Format(CultureInfo.CurrentCulture, @"
                SELECT UTIUT PBGES, UTPFX, UTNOM, UTPNM, UTBRA, BUDBU
                    FROM YUTILIS
                    LEFT JOIN YBUREAU ON YUTILIS.UTBUR = YBUREAU.BUIBU
                    WHERE UTGEP='O' 
                    AND (UTNOM='{0}' OR UTIUT='{0}')
                  ", nomGestionnaire.Trim());

                var result = DbBase.Settings.ExecuteList<GestionnaireDto>(CommandType.Text, sql);
                if (result != null && result.Any())
                {
                    return result.FirstOrDefault();   
                }
            }
            return null;
        }
    }
}
