using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Caching;
using DataAccess.Helpers;
using System.Globalization;
using OP.WSAS400.DTO.Personnes;
using System.Data.Common;
using ALBINGIA.Framework.Common.Data;
using System.Data.EasycomClient;

namespace OP.DataAccess
{
    public class SouscripteurRepository
    {
        

        public static SouscripteurDto Initialiser(DataRow ligne)
        {
            SouscripteurDto Souscripteur = null;
            if (OutilsHelper.ContientUnDesChampsEtEstNonNull(ligne, "UTIUT", "PBSOU", "UTNOM", "DBLSSOU", "CODESOU", "SOUNOM"))
            {
                Souscripteur = new SouscripteurDto();
                if (ligne.Table.Columns.Contains("UTIUT")) { Souscripteur.Code = ligne["UTIUT"].ToString().Trim(); }
                else if (ligne.Table.Columns.Contains("CODESOU")) { Souscripteur.Code = ligne["CODESOU"].ToString().Trim(); }
                if (ligne.Table.Columns.Contains("UTPFX")) { Souscripteur.Login = ligne["UTPFX"].ToString().Trim(); }

                if (ligne.Table.Columns.Contains("UTNOM")) { Souscripteur.Nom = ligne["UTNOM"].ToString().Trim(); }
                else if (ligne.Table.Columns.Contains("SOUNOM")) { Souscripteur.Nom = ligne["SOUNOM"].ToString().Trim(); }
                else if (ligne.Table.Columns.Contains("PBSOU")) { Souscripteur.Nom = ligne["PBSOU"].ToString().Trim(); }
                else if (ligne.Table.Columns.Contains("DBLSSOU")) { Souscripteur.Nom = ligne["DBLSSOU"].ToString().Trim(); }

                if (ligne.Table.Columns.Contains("UTPNM")) { Souscripteur.Prenom = ligne["UTPNM"].ToString().Trim(); };
                if (ligne.Table.Columns.Contains("UTBRA")) { Souscripteur.Branche = BrancheRepository.Initialiser(ligne); };
            }
            return Souscripteur;
        }

        public static List<SouscripteurDto> ObtenirSouscripteurs()
        {
            string sql = @"SELECT UTIUT CODESOU, UTPFX, UTNOM, UTPNM, UTBRA, BUDBU 
                        FROM YUTILIS 
                        LEFT JOIN YBUREAU ON YUTILIS.UTBUR = YBUREAU.BUIBU 
                        WHERE UTSOU = 'O'
                        ORDER BY lower(UTNOM), lower(UTPNM)";

            var result = DbBase.Settings.ExecuteList<SouscripteurDto>(CommandType.Text, sql);

            if (result != null && result.Any())
            {
                foreach (var ligne in result)
                {
                    if (!string.IsNullOrEmpty(ligne.SBranche))
                    {
                        ligne.Branche = new WSAS400.DTO.Offres.Branches.BrancheDto
                        {
                            Code = ligne.SBranche,
                            Cible = new WSAS400.DTO.Offres.Branches.CibleDto()
                        };
                    }
                }
            }
            return result;
        }

        public static bool TesterExistenceSouscripteur(string identifiantUtilisateur)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN
                                         FROM YUTILIS
                                         WHERE UTIUT = '{0}'
                                         AND UTSOU = 'O'", identifiantUtilisateur);
            return CommonRepository.ExistRow(sql);           
        }

        public static List<SouscripteurDto> RechercherSouscripteurs(int debut, int fin, string nomSouscripteur)
        {
            var parameters = new List<EacParameter> {
            };
            string whereQuery = string.Empty;
            if (!string.IsNullOrEmpty(nomSouscripteur.Trim()))
            {
                whereQuery = "WHERE (UTNOM LIKE :nom1 OR UTIUT LIKE :nom2)";
                parameters.Add(new EacParameter("nom1", nomSouscripteur.Trim() + "%"));
                parameters.Add(new EacParameter("nom2", nomSouscripteur.Trim() + "%")); 
            }


            string sql = String.Format(@"SELECT * FROM (
                    SELECT rownumber() OVER (ORDER BY UTNOM, UTIUT) AS ID_NEXT, UTIUT CODESOU, UTPFX, UTNOM, UTPNM, UTBRA, BUDBU, UTSOU 
                      FROM YUTILIS
                            LEFT JOIN YBUREAU ON YUTILIS.UTBUR = YBUREAU.BUIBU
                     {0}
                    ORDER BY UTNOM, UTPNM) AS TABLE 
                    WHERE ID_NEXT BETWEEN :debut AND :fin", whereQuery);

            parameters.Add(new EacParameter("debut", debut));
            parameters.Add(new EacParameter("fin", fin));

            var result = DbBase.Settings.ExecuteList<SouscripteurDto>(CommandType.Text, sql, parameters.ToArray());

            if (result != null && result.Any())
            {
                foreach (var ligne in result)
                {
                    if (!string.IsNullOrEmpty(ligne.SBranche))
                    {
                        ligne.Branche = new WSAS400.DTO.Offres.Branches.BrancheDto
                        {
                            Code = ligne.SBranche,
                            Cible = new WSAS400.DTO.Offres.Branches.CibleDto()
                        };
                    }
                }
            }
            return result;

        }
    }
}
