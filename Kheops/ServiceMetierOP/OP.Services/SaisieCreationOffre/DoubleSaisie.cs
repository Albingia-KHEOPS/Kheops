using ALBINGIA.Framework.Common.Constants;
using OP.DataAccess;
using OP.WSAS400.DTO.Ecran.DoubleSaisie;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract.ISaisieCreationOffre;
using System.Collections.Generic;
using System.ServiceModel.Activation;

namespace OP.Services.SaisieCreationOffre
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DoubleSaisie : IDoubleSaisie
    {

        #region Double Saisie



        /// <summary>
        /// Doubles the saisie get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public DoubleSaisieGetResultDto DoubleSaisieGet(DoubleSaisieGetQueryDto query)
        {
            BrancheDto branche = CommonRepository.GetBrancheCible(query.CodeOffre, query.VersionOffre.ToString(), "O", string.Empty, ModeConsultation.Standard);
            var toReturn = new DoubleSaisieGetResultDto
              {
                  CabinetAutres = PoliceRepository.AlimenteAutreDoubleSaisie(query.CodeOffre,query.VersionOffre.ToString()),
                  //CabinetAutres = ObjectMapperManager.DefaultInstance
                  //                                   .GetMapper<List<CabinetAutre>, List<CabinetAutreDto>>()
                  //                                   .Map(PoliceRepository.AlimenteAutreDoubleSaisie(query.CodeOffre,
                  //                                                                                   query.VersionOffre
                  //                                                                                        .ToString())),
                  MotifsRefusGestionnaire = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "PRODU", "MOTNO", "A"),
                  MotifsRefusDemandeur = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "PRODU", "MOTNO", "D")
                  //MotifsRefusGestionnaire = ObjectMapperManager.DefaultInstance
                  //                                             .GetMapper<List<Parametre>, List<ParametreDto>>()
                  //                                             .Map(ReferenceRepository.ObtenirMotifRefusGest),
                  //MotifsRefusDemandeur = ObjectMapperManager.DefaultInstance
                  //                                          .GetMapper<List<Parametre>, List<ParametreDto>>()
                  //                                          .Map(ReferenceRepository.ObtenirMotifRefusDem)
              };

            //on recupere la liste des offres autres.

            //on recupere les souscripteursDoubleSaisieGet

            //on recupere motif courtier gestionnaire


            //on recupere motif courtier demandeur

            return toReturn;
        }



        /// <summary>
        /// Cabinets the courtage get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public CabinetCourtageGetResultDto DoubleSaisieCabinetCourtageGet(CabinetCourtageGetQueryDto query)
        {
            var toReturn = new CabinetCourtageGetResultDto();

            foreach (CabinetCourtageDto courtier in new PoliceServices().CabinetCourtageGet(query,false).CabinetCourtages)
            {
                toReturn.CabinetCourtages.Add(new CabinetCourtageDto { Code = courtier.Code, NomCabinet = courtier.NomCabinet });
            }
            return toReturn;
        }


        public void EnregistrerDoubleSaisie(CabinetAutreDto cabinet, string user)
        {
            DoubleSaisieRepository.SauvegarderDoubleSaisie(cabinet, user);
            #region Ajout de l'acte de gestion
            CommonRepository.AjouterActeGestion(cabinet.CodeOffre, cabinet.Version, cabinet.Type, 0, AlbConstantesMetiers.ACTEGESTION_VALIDATION, AlbConstantesMetiers.TRAITEMENT_OFDBL, "", user);
            #endregion
        }
        
        public DoubleSaisieListeDto ObtenirDoubleSaisieListes(string branche, string cible)
        {
            return new DoubleSaisieListeDto
            {
                MotifsRemp = CommonRepository.GetParametres(branche, cible, "KHEOP", "DBMOT"),
                NotificationsApporteur = new List<ParametreDto>(),
                NotificationsDemandeur = new List<ParametreDto>()
            };
        }

        public string GetNewVersionOffre(string codeOffre, string version, string type, string user)
        {
            return DoubleSaisieRepository.GetNewVersionOffre(codeOffre, version, type, user);
        }

        #endregion
    }
}
