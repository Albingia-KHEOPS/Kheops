using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using OP.DataAccess;
using OP.WSAS400.DTO.Ecran.ConfirmationSaisie;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract.ISaisieCreationOffre;
using System.Collections.Generic;
using System.ServiceModel.Activation;

namespace OP.Services.SaisieCreationOffre
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ConfirmationSaisie : IConfirmationSaisie
    {
        #region Confirmation Saisie

        /// <summary>
        /// Confirmations the saisie get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public ConfirmationSaisieGetResultDto ConfirmationSaisieGet(ConfirmationSaisieGetQueryDto query)
        {
            var toReturn = new ConfirmationSaisieGetResultDto();


            //AccesDataManager._connectionHelper = easyComConnectionHelper;
            BrancheDto branche = CommonRepository.GetBrancheCible(query.CodeOffre, query.VersionOffre.ToString(), query.Type, string.Empty, ModeConsultation.Standard);
            toReturn.Offre = new PoliceServices().OffreGetDto(query.CodeOffre, query.VersionOffre, query.Type, ModeConsultation.Standard);
            toReturn.MotifsRefus = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "PRODU", "PBSTF");
            //ObjectMapperManager.DefaultInstance.GetMapper<List<Parametre>, List<ParametreDto>>().Map(ReferenceRepository.ObtenirMotifRefus);

            return toReturn;
        }

        public List<ParametreDto> GetListeMotifs()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBSTF");
        }

        public void EnregistrerNouvellePosition(string codeOffre, string version, string type, string newEtat, string newSituation, string motif, string user)
        {
            PoliceRepository.EnregistrerNouvellePosition(codeOffre, version, type, newEtat, newSituation, motif, user);
        }


        /// <summary>
        /// Confirmations the saisie set.
        /// </summary>
        /// <param name="query">The query.</param>
        public ConfirmationSaisieSetResultDto ConfirmationSaisieSet(ConfirmationSaisieSetQueryDto query)
        {
            var toReturn = new ConfirmationSaisieSetResultDto();

            //AccesDataManager._connectionHelper = easyComConnectionHelper;
            if (query.RefusImmediat)
            {               
                //PoliceRepo.ChangerStatutOffre(query.CodeOffre, SituationOffre.SansSuite, motifRefus);
                PoliceRepository.ChangerStatutOffre(query.CodeOffre, AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.CodesSituation.Refus), query.MotifRefus);
            }
            else if (query.Attente)
            {

                //PoliceRepo.ChangerStatutOffre(query.CodeOffre, SituationOffre.Attente, motifAttente);
                PoliceRepository.ChangerStatutOffre(query.CodeOffre, AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.CodesSituation.Attente), query.MotifRefus);
            }
            else
            {
                //PoliceRepo.ChangerStatutOffre(query.CodeOffre, SituationOffre.Encours);
                PoliceRepository.ChangerStatutOffre(query.CodeOffre, AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.CodesSituation.Attente), new ParametreDto { Code = string.Empty });
            }

            return toReturn;
        }


        #endregion

    }
}
