using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.Framework.Common.Tools;
using OP.DataAccess;
using OP.Services.WSKheoBridge;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Ecran.DetailsRisque;
using OP.WSAS400.DTO.Historique;
using OP.WSAS400.DTO.LTA;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Aperiteur;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Indice;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Personnes;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;

namespace OP.Services.SaisieCreationOffre
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PoliceServices : IPoliceServices
    {
        public string SauvegardeOffre(OffreDto offre, string user)
        {
            string retourMsg = string.Empty;

            if (offre.Branche == null)
            {
                retourMsg = "Branche non renseignee";
            }

            if (offre.CabinetGestionnaire == null)
            {
                retourMsg = "Code cabinet gestionnaire non conforme";
            }

            if (offre.PreneurAssurance == null)
            {
                retourMsg = "Code assure non conforme";
            }

            if (offre.Gestionnaire != null && !string.IsNullOrEmpty(offre.Gestionnaire.Id) && !GestionnaireRepository.TesterExistenceGestionnaire(offre.Gestionnaire.Id))
            {
                retourMsg = "Code gestionnaire inconnu";
            }
            if (offre.Souscripteur != null && !string.IsNullOrEmpty(offre.Souscripteur.Code))
            {
                if (!SouscripteurRepository.TesterExistenceSouscripteur(offre.Souscripteur.Code))
                {
                    retourMsg = "Code souscripteur inconnu";
                }
            }

            if (!offre.DateSaisie.HasValue)
            {
                retourMsg = "Date de saisie invalide";
            }

            if (!string.IsNullOrEmpty(retourMsg)) return retourMsg;

            if (offre.CopyMode)
            {
                offre.CodeOffre = !string.IsNullOrEmpty(offre.CodeOffreCopy) ? offre.CodeOffreCopy : CommonRepository.ObtenirNumeroPolice(offre.DateSaisie.Value.Year, offre.Branche.Code, string.Empty, string.Empty, "O");
                offre.Version = !string.IsNullOrEmpty(offre.VersionCopy) ? Convert.ToInt32(offre.VersionCopy) : 0;
            }
            else
            {
                offre.CodeOffre = !string.IsNullOrEmpty(offre.CodeOffre) ? offre.CodeOffre : CommonRepository.ObtenirNumeroPolice(offre.DateSaisie.Value.Year, offre.Branche.Code, string.Empty, string.Empty, "O");
                offre.Version = offre.Version != null ? offre.Version : 0;
            }

            retourMsg = PoliceRepository.SaveOffre(offre, user);

            return retourMsg;
        }

        /// <summary>
        /// Bonifications the get.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public BonificationsDto BonificationGet(string id, int version)
        {
            BonificationsDto bonification = null;
            if (!string.IsNullOrEmpty(id))
            {
                bonification = PoliceRepository.ObtenirBonification(id, version);
            }
            return bonification;
        }

        #region Transverse Offre

        public LTADto GetInfoLTA(string codeAffaire, int version, string type, int avenant, ModeConsultation modeNavig)
        {
            var toReturn = PoliceRepository.GetInfoLTA(codeAffaire, version, type, avenant, modeNavig);
            return toReturn;
        }

        public void SetInfoLTA(string codeAffaire, int version, string type, int avenant, LTADto dto)
        {
            PoliceRepository.SetInfoLTA(codeAffaire, version, type, avenant, dto);
        }

        public bool OffreEstValide(string numeroOffre, string version, string type, string numAvn)
        {
            if (RegularisationRepository.ExistRegul(numeroOffre, version, type, numAvn))
            {
                return false;
            }

            string sqlRequest =
              string.Format("SELECT COUNT(*) NBLIGN FROM YPOBASE WHERE PBIPB='{0}' AND PBALX={1} AND PBTYP='{2}' AND (PBETA='V' OR PBETA='R') AND PBAVN='{3}'", numeroOffre.Trim().PadLeft(9, ' '), version, type, numAvn);
            return CommonRepository.ExistRow(sqlRequest);
        }
        /// <summary>
        /// Indices the get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IndiceGetResultDto IndiceGet(IndiceGetQueryDto query)
        {

            var toReturn = new IndiceGetResultDto();

            if (!string.IsNullOrEmpty(query.Code))
            {
                toReturn.Valeur = PoliceRepository.ObtenirValeurIndice(query.Code, query.DateEffet);
            }

            return toReturn;
        }

        /// <summary>
        /// Cabinets the courtage get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="modeAutocomplete"> </param>
        /// <returns></returns>
        public CabinetCourtageGetResultDto CabinetCourtageGet(CabinetCourtageGetQueryDto query, bool modeAutocomplete)
        {
            var toReturn = new CabinetCourtageGetResultDto();

            //on recupere les cabinets de courtages

            var cabinetCourtages = new List<CabinetCourtageDto>();
            if (query.Code.HasValue)
            {
                var cabinetCourtage = CabinetCourtageRepository.Obtenir(query.Code.Value);
                if (cabinetCourtage != null)
                {
                    cabinetCourtages.Add(cabinetCourtage);
                }
            }
            else
            {
                cabinetCourtages = CabinetCourtageRepository.Rechercher(query.DebutPagination, query.FinPagination, query.NomCabinet, query.Order, query.By, modeAutocomplete);
                if (!modeAutocomplete)
                    toReturn.CabinetCourtageCount = CabinetCourtageRepository.RechercherCount(query.NomCabinet);
            }
            toReturn.CabinetCourtages = cabinetCourtages;

            if (query.ReturnSouscripteurs)
            {
                toReturn.Souscripteurs = SouscripteurRepository.ObtenirSouscripteurs().Select(s => new ParametreDto { Code = s.Code, Libelle = string.Format(@"{0} {1}", s.Nom, s.Prenom) }).ToList();
            }
            return toReturn;
        }

        public CabinetCourtageDto ObtenirCabinetCourtageComplet(int code, int codeInterlocuteur)
        {
            return CabinetCourtageRepository.Obtenir(code, codeInterlocuteur);
        }

        /// <summary>
        /// Cabinets the courtage get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public InterlocuteurGetResultDto InterlocuteursGet(InterlocuteurGetQueryDto query)
        {
            var toReturn = new InterlocuteurGetResultDto();
            toReturn.Interlocuteurs = InterlocuteurRepository.RechercherInterlocuteurs(query.Nom, query.CodeCabinetCourtage);
            return toReturn;
        }

        public InterlocuteurGetResultDto InterlocuteursAperiteurGet(string queryNomInterlocuteur, string queryCodeAperiteur)
        {
            var toReturn = new InterlocuteurGetResultDto();
            //on recupere les interlocuteurs
            toReturn.Interlocuteurs = AperiteurRepository.RechercherInterlocuteursAperiteur(queryNomInterlocuteur, queryCodeAperiteur);

            return toReturn;
        }

        /// <summary>
        /// Assureses the get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public AssureGetResultDto AssuresGet(AssureGetQueryDto query, bool modeAutocomplete)
        {
            var toReturn = new AssureGetResultDto();
            var assures = AssureRepository.Rechercher(query.DebutPagination, query.FinPagination, query.Code, query.NomAssure, query.CodePostal, query.Order, query.By, modeAutocomplete);
            if (!modeAutocomplete)
            {
                toReturn.AssuresCount = AssureRepository.Count(query);
            }
            toReturn.Assures = assures;

            return toReturn;
        }

        public List<AssurePlatDto> RechercheTransversePreneurAssurance(string codePreneur, string nomPreneur, string cpPreneur)
        {
            return AssureRepository.RechercheTransversePreneurAssurance(codePreneur, nomPreneur, cpPreneur)
                .GroupBy(x => x.Code)
                .Select(g => g.Any(x => x.NombreSinistres > 0) ? g.First(x => x.NombreSinistres > 0) : g.First())
                .ToList();
        }

        public AssureDto ObtenirAssureComplet(int code)
        {
            return AssureRepository.Obtenir(code);
        }

        /// <summary>
        /// Assureses the get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public GestionnairesGetResultDto GestionnairesGet(GestionnairesGetQueryDto query)
        {

            var toReturn = new GestionnairesGetResultDto();
            //on recupere les interlocuteurs

            toReturn.GestionnairesDto =
                GestionnaireRepository.RechercherGestionnaires(query.DebutPagination, query.FinPagination, query.NomGestionnaire);

            return toReturn;
        }

        /// <summary>
        /// Souscripteurses the get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public SouscripteursGetResultDto SouscripteursGet(SouscripteursGetQueryDto query)
        {
            var toReturn = new SouscripteursGetResultDto();

            toReturn.SouscripteursDto =
                SouscripteurRepository.RechercherSouscripteurs(query.DebutPagination, query.FinPagination, query.NomSouscripteur);

            return toReturn;
        }

        public List<UtilisateurDto> UtilisateursGet(string name)
        {
            return PoliceRepository.RechercherUtilisateurs(name);
        }

        public List<ParametreDto> GetCibles(string codeBranche, bool loadAllIfNull, bool isAdmin, bool isUserHorse)
        {
            return BrancheRepository.GetCibles(codeBranche, loadAllIfNull, isAdmin, isUserHorse);
        }

        public AperiteurGetResultDto AperiteursGet(AperiteurGetQueryDto query)
        {
            var toReturn = new AperiteurGetResultDto();

            if (!string.IsNullOrEmpty(query.Code))
            {
                var aperiteur = AperiteurRepository.Obtenir(query.Code);
                if (aperiteur != null)
                {
                    toReturn.AperiteursDto = new List<AperiteurDto>();
                    toReturn.AperiteursDto.Add(aperiteur);
                }
            }
            else
            {
                toReturn.AperiteursDto = AperiteurRepository.Rechercher(1, 30, query.Nom);
            }
            return toReturn;
        }

        public AperiteurGetResultDto AperiteursGetByCodeNum(Int64 codeNum)
        {
            var toReturn = new AperiteurGetResultDto();

            var aperiteur = AperiteurRepository.ObtenirByCodeNum(codeNum);
            if (aperiteur != null)
            {
                toReturn.AperiteursDto = new List<AperiteurDto>();
                toReturn.AperiteursDto.Add(aperiteur);
            }

            return toReturn;
        }

        /// <summary>
        /// Offres the get dto.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public OffreDto OffreGetDto(string id, int version, string type, ModeConsultation modeNavig)
        {
            var offre = PoliceRepository.Obtenir(id, version, type);
            if (offre != null)
            {

                offre.Risques = PoliceRepository.ObtenirRisques(modeNavig, id, version);
                offre.HasDoubleSaisie = PoliceRepository.GetDoubleSaisie(id, version, type);
                offre.Bonification = BonificationGet(id, version);

                if (offre.Risques != null && offre.Risques.Count == 1)
                    offre.IsMonoRisque = true;
                else offre.IsMonoRisque = false;

                offre.HasOppBenef = PoliceRepository.GetOppBenef(id, version, type);
            }
            return offre;

        }

        /// <summary>
        /// Permet de verifier si l'offre ou le contrat 
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <returns></returns>
        public bool IsEnteteContainAddress(string codeOffre, string version, string type, string codeAvn)
        {
            return PoliceRepository.IsEnteteContainAddress(codeOffre, version, type, codeAvn);
        }

        public List<RisqueDto> ObtenirRisques(ModeConsultation modeNavig, string offreId, int? offreVersion, string type, string codeAvn)
        {
            return PoliceRepository.ObtenirRisques(modeNavig, offreId, offreVersion, type, codeAvn);
        }

        public List<RisqueDto> ObtenirIDRisques(string offreId, int? offreVersion)
        {
            return PoliceRepository.ObtenirIDRisques(offreId, offreVersion);
        }

        public List<RisqueDto> ObtenirInfosRisquesInventaire(ModeConsultation modeNavig, string offreId, int? offreVersion, string type, string codeAvn)
        {
            return PoliceRepository.GetRisquesBaseInfos(modeNavig, offreId, offreVersion, type, codeAvn);
        }

        public DetailsRisqueGetResultDto GetInfoDetailRsq(string codeOffre, string version, string type, string numRsq, string numObj, ModeConsultation modeNavig, string codeAvn, bool isAdmin, string codeBranche, string codeCible, bool isUserHorse)
        {
            return PoliceRepository.GetInfoDetailRsq(codeOffre, version, type, numRsq, numObj, modeNavig, codeAvn, isAdmin, codeBranche, codeCible, isUserHorse);
        }

        public OffreDto GetInfoCreationSaisie(string codeOffre, string version, string type)
        {
            return PoliceRepository.GetInfoCreationSaisie(codeOffre, version, type);
        }

        /// <summary>
        /// Tests the offre.
        /// </summary>
        /// <param name="offreId">The offre id.</param>
        /// <returns></returns>
        public bool TestExistanceOffre(string offreId)
        {
            var toReturn = false;

            toReturn = PoliceRepository.TesterExistanceOffre(offreId);

            return toReturn;
        }

        public List<ParametreDto> ObtenirParametres(string codeOffre, string version, string type, string codeAvn, string contexte, string famille, ModeConsultation modeNavig)
        {
            BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);
            return CommonRepository.GetParametres(branche.Code, branche.Cible.Code, contexte, famille);
        }

        public List<BrancheDto> BranchesGet()
        {
            return BrancheRepository.ObtenirBranches(string.Empty, string.Empty, true);
        }

        public List<ParametreDto> RegimeTaxeGet(string branche, string cible)
        {
            return CommonRepository.GetParametres(branche ?? string.Empty, cible ?? string.Empty, "GENER", "TAXRG");
        }

        public List<ParametreDto> ObtenirMotClef(string branche, string cible)
        {
            return PoliceRepository.ObtenirMotClef(branche, cible);
        }

        public List<ParametreDto> IndicesReferenceGet()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "INDIC");
        }

        public DelegationDto ObtenirDelegation(int codeCabinetCourtage)
        {
            return DelegationRepository.Obtenir(codeCabinetCourtage);
        }

        #endregion

        public bool VerifyTraceOffre(string codeOffre, string version, string type, string etape, string perimetre)
        {
            return TraceRepository.ObtenirTraceByEtape(codeOffre, version, type, etape, perimetre);
        }

        public void RefusOffre(string codeOffre, string version, string codeMotif)
        {
            OffreRepository.RefusOffre(codeOffre, version, codeMotif);
        }

        public string ValiderOffre(string codeOffre, string version, string type, string avenant, string acteGestion, string validable, string complet, string motif, string mode, string lotsId, string user, string reguleId, bool isModfiHorsAvn)
        {
            bool isSetHisto = !string.IsNullOrEmpty(reguleId) ? RegularisationRepository.IsReguleHisto(Convert.ToInt32(reguleId)) : true;

            string numPrime = "0";
            string numPrimeRegule = "0";

            if (!isModfiHorsAvn)
            {
                // Suppression des enregistrements de YPRIPES et satellites
                if (mode == "valider")
                {
                    OffreRepository.DeletePripeFiles(codeOffre, version, type, acteGestion);

                    if (acteGestion.IsIn(
                        AlbConstantesMetiers.TYPE_AVENANT_MODIF,
                        AlbConstantesMetiers.TYPE_AVENANT_RESIL,
                        AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF,
                        AlbConstantesMetiers.TYPE_AVENANT_REGUL))
                    {
                        CommonRepository.AnnulationPrimes(codeOffre, version);
                    }

                    if (type == AlbConstantesMetiers.TYPE_CONTRAT)
                    {
                        if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL)
                        {
                            numPrimeRegule = CommonRepository.GenerationPrimeRegule(codeOffre, type, version, avenant, acteGestion, user);
                            RegularisationRepository.ValidRegul(codeOffre, version, reguleId, numPrimeRegule, user);

                            //Gestion cas spécifique PB Transport
                            if (codeOffre.Substring(0, 2) == "TR")
                            {
                                RegularisationRepository.UpdateReportChargeTR(codeOffre, version, type, reguleId, numPrimeRegule, user);
                            }
                        }

                        if (acteGestion != AlbConstantesMetiers.TYPE_AVENANT_REGUL)
                        {
                            numPrime = CommonRepository.GenerationQuittanceComptable(codeOffre, type, version, avenant, acteGestion, user);
                        }
                    }
                }

                try
                {
                    if (!string.IsNullOrEmpty(lotsId))
                    {
                        EditAsync(codeOffre, version, type, avenant, mode, lotsId, user, numPrime: numPrime, numPrimeRegule: numPrimeRegule);
                    }
                }
                catch (Exception ex)
                {
                    string message = $"Module Editique - Erreur de génération du lot d'impression N° {lotsId}";
                    AlbLog.Warn($"{message}{Environment.NewLine}{ex.ToString()}");
                    return message;
                }
            }

            return isSetHisto ? OffreRepository.ValiderOffre(codeOffre, version, type, avenant, acteGestion, validable, complet, motif, mode, lotsId, user, isModfiHorsAvn) : string.Empty;
        }

        public string EditerDocParLot(string codeOffre, string version, string type, string avenant, string mode, string lotsId, string user, bool isReadOnly, string acteGestion, string attesId, string machineName, int switchModuleGestDoc)
        {
            try
            {
                if (!string.IsNullOrEmpty(lotsId))
                    EditAsync(codeOffre, version, type, avenant, mode, lotsId, user, acteGestion, attesId, isReadOnly, machineName: machineName, switchModuleGestDoc: switchModuleGestDoc);
            }
            catch (Exception)
            {
                return string.Format("Module Editique - Erreur de génération du lot d'impression N° {0}", lotsId);
            }
            return string.Empty;
        }

        private void EditAsync(string codeOffre, string version, string type, string avenant, string mode, string lotsId, string user,
            string acteGestion = "", string attesId = "", bool isReadOnly = false, string numPrime = "", string numPrimeRegule = "", string machineName = "",
            int switchModuleGestDoc = 0)
        {
            string[] tLotsId = lotsId.Split(new[] { ";" }, StringSplitOptions.None);
#if DEBUG
            machineName = Environment.MachineName;
#endif
            switch (mode)
            {
                case "editer":
                    foreach (var item in tLotsId)
                    {
                        using (var serviceContext = new KheoBridge())
                        {
                            var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                            if (!string.IsNullOrEmpty(kheoBridgeUrl))
                                serviceContext.Url = kheoBridgeUrl;

                            if (acteGestion == AlbConstantesMetiers.TYPE_ATTESTATION)
                            {
                                serviceContext.EditerDocParLot(Convert.ToInt64(item), "", "ATTES", Convert.ToInt32(attesId), "", user, 0);
                            }
                            else
                            {
                                if (!isReadOnly)
                                {
                                    if (switchModuleGestDoc == 1)
                                        serviceContext.EditerDocParLotPush(Convert.ToInt64(item), "", "N", 0, "E", user, 0, machineName);
                                    else
                                        serviceContext.EditerDocParLot(Convert.ToInt64(item), "", "N", 0, "E", user, 0);
                                }
                                else
                                {
                                    if (switchModuleGestDoc == 1)
                                        serviceContext.EditerDocParLotPush(Convert.ToInt64(item), "", "N", 0, "N", user, 0, machineName);
                                    else
                                        serviceContext.EditerDocParLot(Convert.ToInt64(item), "", "N", 0, "N", user, 0);
                                }
                            }
                        }
                    }
                    break;
                case "valider":

                    foreach (var item in tLotsId)
                    {
                        using (var serviceContext = new KheoBridge())
                        {
                            var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                            if (!string.IsNullOrEmpty(kheoBridgeUrl))
                                serviceContext.Url = kheoBridgeUrl;
                            if (switchModuleGestDoc == 1)
                                serviceContext.EditerDocParLotPush(Convert.ToInt64(item), "", "O", int.TryParse(numPrime, out var np) ? np : 0, "O", user, int.TryParse(numPrimeRegule, out var npr) ? npr : 0, machineName);
                            else
                                serviceContext.EditerDocParLot(Convert.ToInt64(item), "", "O", int.TryParse(numPrime, out var np) ? np : 0, "O", user, int.TryParse(numPrimeRegule, out var npr) ? npr : 0);
                        }
                    }
                    break;
            }
        }

        public string ConvertSimpleFolderToStd(string codeOffre, string version, string type, string branche, string cible, string user)
        {
            return OffreRepository.ConvertSimpleFolderToStd(codeOffre, version, type, branche, cible, user);
        }


        public string DeleteOffre(string codeOffre, string version, string type)
        {
            return OffreRepository.DeleteOffre(codeOffre, version, type);
        }

        public string GetOffreLastVersion(string codeOffre, string version, string type, string user)
        {
            return PoliceRepository.GetOffreLastVersion(codeOffre, version, type, user);
        }

        public bool GetModifHorsAvnIsRegularisable(string codeOffre, int version, string type, int numAvn, string codeRsq)
        {
            return PoliceRepository.GetModifHorsAvnIsRegularisable(codeOffre, version, type, numAvn, codeRsq);
        }


        public HistoriqueDto GetListHistorique(string codeAffaire, string version, string type, bool contractuel)
        {
            return HistoriqueRepository.GetListHistorique(codeAffaire, version, type, contractuel);
        }

        public void ClasserContratsSansSuite(string codeAffaire, int version, string type, string listeAnnulQuitt, string user)
        {
            FinOffreRepository.EnregistrerQuittancesAnnulees(codeAffaire, version.ToString(), listeAnnulQuitt);
            CommonRepository.AnnulationPrimes(codeAffaire, version.ToString());
            PoliceRepository.ClasserContratsSansSuite(codeAffaire, version, type, user);
        }

        /// <summary>
        /// Liste des contrats d'une offre
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<CreationAffaireNouvelleContratDto> LstContrats(string codeOffre, string version)
        {
            return AffaireNouvelleRepository.LoadListContrat(codeOffre, version);
        }

        #region Gestion risque
        public void UpdateRiskTotalValue(string code, int? version, int riskId, long? totalValue, string unite, string riskType, string valueHt)
        {
            PoliceRepository.UpdateRiskTotalValue(code, version, riskId, totalValue, unite, riskType, valueHt);
        }
        public RisqueDto ObtenirRisque(ModeConsultation modeNavig, string codeOffre, int numRsq, int? version = null, string type = "O", string codeAvn = "")
        {

            return PoliceRepository.ObtenirRisque(modeNavig, codeOffre, numRsq, version, type, codeAvn);
        }
        public int CopierRisque(string codeOffre, int numRsq, string CBnsPb, string user, int? version = 0, string type = "O")
        {

            int newRsq = PoliceRepository.CopierRisque(codeOffre, numRsq, CBnsPb, user, version, type);
            var tabLien = RisqueRepository.GetLienRisqueFormule(codeOffre, version.ToString(), type, newRsq.ToString()).Select(lien => lien.CodeFor);
            var listClauses = ClauseRepository.ObtenirClauses(type, codeOffre, version.ToString(), "", "", "Tous", "", "", "", "", "", "", "", "", ModeConsultation.Standard);

            listClauses = listClauses.Where(x => (x.CodeRisque == newRsq || tabLien.Any(c => c == x.CodeFormule)) && !string.IsNullOrEmpty(x.CheminFichier)).ToList();
            listClauses.ForEach(clause =>
            {
                int idDoc = CommonRepository.GetAS400Id("RepSessionCP");
                string newCheminFichier = clause.CheminFichier.Substring(0, clause.CheminFichier.LastIndexOf('_') + 1);
                newCheminFichier += idDoc + clause.CheminFichier.Substring(clause.CheminFichier.LastIndexOf('.'));
                IOFileManager.CopyFile(clause.CheminFichier, newCheminFichier);
                string acteGes = "AFFNOUV";
                switch (type)
                {
                    case "O":
                        acteGes = "OFFRE";
                        break;
                    case "P":
                        acteGes = "AFFNOUV";
                        break;
                }
                ClauseRepository.SaveClauseMagnetic(codeOffre, version.ToString(), type, 0, acteGes, clause.Etape, clause.Titre, newCheminFichier, (int)clause.Id, clause.Chapitre, clause.SousChapitre, clause.NumeroOrdre.ToString(), clause.Contexte, clause.IsModif);
            });

            return newRsq;
        }
        #endregion

        #region Gestion Adresse
        public AdressePlatDto ObtenirAdresse(string codeOffre, string version, string type)
        {
            return AdresseRepository.ObtenirAdresse(codeOffre, version, type);
        }
        #endregion

        public void UpdateOrInsertObservation(string codeOffre, string type, int version, string obsvInfoGen, string obsvCotisation, string obsvEngagement, string obsvMntRef, string obsvRefGest)
        {
            PoliceRepository.UpdateObservations(codeOffre, type, version, obsvInfoGen, obsvCotisation, obsvEngagement, obsvMntRef, obsvRefGest);
        }
    }
}
