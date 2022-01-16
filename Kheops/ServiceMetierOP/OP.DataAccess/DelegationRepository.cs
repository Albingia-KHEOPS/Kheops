using ALBINGIA.Framework.Common.Data;
using DataAccess.Helpers;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Data;
using System.Linq;

namespace OP.DataAccess {
    public class DelegationRepository
    {

        public static DelegationDto Initialiser(DataRow ligne)
        {
            DelegationDto result = null;
            if (OutilsHelper.ContientUnDesChampsEtEstNonNull(ligne, "P0GBU", "TCBUR"))
            {
                result = new DelegationDto();
                if (ligne.Table.Columns.Contains("P0GBU")) { result.Code = ligne["P0GBU"].ToString().Trim(); } else if (ligne.Table.Columns.Contains("TCBUR")) { result.Code = ligne["TCBUR"].ToString().Trim(); }
                if (ligne.Table.Columns.Contains("BUDBU")) { result.Nom = ligne["BUDBU"].ToString().Trim(); } else if (ligne.Table.Columns.Contains("delegApp")) { result.Nom = ligne["delegApp"].ToString().Trim(); }
            }
            return result;
        }

        public static DelegationDto Obtenir(int codeCabinetCourtage)
        {
            DelegationDto delegation = CommonRepository.ObtenirCodeDelegationEtCodeInspecteur(codeCabinetCourtage);
            return ObtenirNomDelegationNomInspecteur(delegation);
        }

        /// <summary>
        /// Complete le nom de la delegation et le nom de l'inspecteur a
        /// partir du code delegation et du code inspecteur
        /// </summary>
        /// <param name="delegation"></param>
        /// <returns></returns>       
        public static DelegationDto ObtenirNomDelegationNomInspecteur(DelegationDto delegation)
        {
            if (delegation != null && delegation.Inspecteur != null)
            {                
                string sql = string.Format(@"SELECT BUDBU LIBELLE, UTNOM DESCRIPTIF FROM YBUREAU,
                                          (SELECT UTNOM FROM YINSPEC LEFT JOIN YUTILIS ON ACLUIN=UTIUT WHERE ACLINS ='{0}') AS TABLE
                                          WHERE BUIBU = '{1}'", !string.IsNullOrEmpty(delegation.Inspecteur.Code) ? delegation.Inspecteur.Code.Replace("'", "''") : string.Empty,
                                                              !string.IsNullOrEmpty(delegation.Code) ? delegation.Code.Replace("'", "''") : string.Empty);
                var paramDto = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql).FirstOrDefault();
                if (paramDto != null)
                {
                    delegation.Nom = paramDto.Libelle;
                    delegation.Inspecteur.Nom = paramDto.Descriptif.ToString().Trim();
                }
            }
            return delegation;
        }
    }
}
