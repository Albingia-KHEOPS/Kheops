using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.DataAccess;
using OP.Services.BLServices;
using OP.Services.BLServices.Regularisations;
using OP.Services.Connexites;
using OP.Services.WSKheoBridge;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Attestation;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Contrats;
using OP.WSAS400.DTO.Engagement;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Partenaire;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract.IAffaireNouvelle;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;

namespace OP.Services.TraitementAffNouv
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AffaireNouvelle : IAffaireNouvelle, IDisposable
    {
        private readonly AffaireNouvelleRepository repository;
        private readonly FolderService folderService;
        private readonly ConnexiteService connexiteService;
        private readonly FormuleRepository formuleRepository;
        private readonly PoliceRepository policeRepository;
        private readonly ProgramAS400Repository as400Repository;

        private readonly EngagementService engagementService;
        private readonly DocumentService documentService;
        private readonly TraceService traceService;

        private readonly IAffairePort affairePort;
        private readonly IFormulePort formulePort;

        public AffaireNouvelle(IDbConnection connection, FolderService folderService, ConnexiteService connexiteService, AffaireNouvelleRepository affRepository, FormuleRepository formuleRepository, ProgramAS400Repository as400Repository, PoliceRepository policeRepository, EngagementService engagementService, DocumentService documentService, TraceService traceService, IFormulePort formulePort, IAffairePort affairePort)
        {
            DbConnection = connection;
            this.repository = affRepository;
            this.folderService = folderService;
            this.connexiteService = connexiteService;
            this.formuleRepository = formuleRepository;
            this.engagementService = engagementService;
            this.documentService = documentService;
            this.as400Repository = as400Repository;
            this.policeRepository = policeRepository;
            this.traceService = traceService;
            this.formulePort = formulePort;
            this.affairePort = affairePort;
        }

        internal IDbConnection DbConnection { get; private set; }

        #region Ecran Creation Affaire Nouvelle

        public CreationAffaireNouvelleDto InitCreateAffaireNouvelle(string codeOffre, string version, string type)
        {
            return AffaireNouvelleRepository.InitCreateAffaireNouvelle(codeOffre, version, type);
            //return InitAffNouv.InitCreateAffaireNouvelle(codeOffre, version, type);
        }

        public CreationAffaireNouvelleContratDto InitAffaireNouvelleContrat(string codeOffre, string version, string type, string codeAvn, string user, ModeConsultation modeNavig)
        {
            return AffaireNouvelleRepository.InitAffaireNouvelleContrat(codeOffre, version, type, codeAvn, user, modeNavig);
            //return InitAffNouv.InitAffaireNouvelleContrat(codeOffre, version, type, codeAvn, user, modeNavig);
        }
        public ContratInfoBaseDto InitContratInfoBase(string id, string version, string type, string codeAvn, string user, ModeConsultation modeNavig)
        {
            return AffaireNouvelleRepository.InitContratInfoBase(id, version, type, codeAvn, user, modeNavig);
            //return InitAffNouv.InitContratInfoBase(id, version, type, codeAvn, user, modeNavig);
        }
        public ContratDto GetContrat(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var contratDto = BLCommon.GetContrat(contratId, version, type, codeAvn, modeNavig);
            return contratDto;
        }

        public ContratDto GetBasicFolder(Folder folder)
        {
            return this.folderService.GetBasicFolder(folder);
        }

        public ContratDto GetContratSansRisques(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return BLCommon.GetContrat(contratId, version, type, codeAvn, modeNavig, false);

        }
        public void UpdatePeriodicite(string codeOffre, string version, string type, string periodicite)
        {
            AffaireNouvelleRepository.UpdatePeriodicite(codeOffre, version, type, periodicite);
            //InitAffNouv.UpdatePeriodicite(codeOffre, version, type, periodicite);
        }
        public List<CourtierDto> GetListCourtiers(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return AffaireNouvelleRepository.GetListCourtiers(contratId, version, type, codeAvn, modeNavig);
            //return InitAffNouv.GetListCourtiers(contratId, version, type, codeAvn, modeNavig);
        }

        public ModeleInfoPageCommissionDto LoadInfoPageCourtier(string codeContrat, string versionContrat, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion)
        {
            ModeleInfoPageCommissionDto toReturn = new ModeleInfoPageCommissionDto();
            toReturn.LstCourties = AffaireNouvelleRepository.GetListCourtiers(codeContrat, versionContrat, type, codeAvn, modeNavig);
            toReturn.CommissionsStd = AffaireNouvelleRepository.GetCommissionsStandardCourtier(codeContrat, versionContrat, type, codeAvn, isReadonly, modeNavig, user, acteGestion);
            return toReturn;
        }
        public CourtierDto GetCourtier(int CodeCourtier, string contratId, string version, string type, ModeConsultation modeNavig)
        {
            return AffaireNouvelleRepository.GetCourtier(CodeCourtier, contratId, version, type);
            //return InitAffNouv.GetCourtier(CodeCourtier, contratId, version, type);
        }

        public void InfoGeneralesContratModifier(ContratDto contrat, string utilisateur, bool isModifHorsAvn)
        {
            AffaireNouvelleRepository.InfoGeneralesContratModifier(contrat, utilisateur, isModifHorsAvn);
            #region Arbre de navigation

            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = contrat.CodeContrat.PadLeft(9, ' '),
                Version = Convert.ToInt32(contrat.VersionContrat),
                Type = contrat.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale),
                NumeroOrdreDansEtape = 10,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = utilisateur,
                PassageTag = "O",
                PassageTagClause = "N"
            });

            #endregion
        }
        public string UpdateContrat(ContratInfoBaseDto contrat, string utilisateur, string acteGestion, string user)
        {
            return BLCommon.UpdateContrat(contrat, utilisateur, acteGestion, user);
            //return InitAffNouv.UpdateContrat(contrat, utilisateur, acteGestion);
        }
        public string UpdateCourtier(string codeContrat, string versionContrat, string type, string typeCourtier, int identifiantCourtier, Single partCommission, string typeOperation, string user)
        {
            string result = AffaireNouvelleRepository.UpdateCourtier(codeContrat, versionContrat, type, typeCourtier, identifiantCourtier, partCommission, typeOperation);
            #region Arbre de navigation
            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = codeContrat.PadLeft(9, ' '),
                Version = Convert.ToInt32(versionContrat),
                Type = type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoCourtier),
                NumeroOrdreDansEtape = 12,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoCourtier),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });
            #endregion
            return result;
            //return InitAffNouv.UpdateCourtier(codeContrat, versionContrat, type, typeCourtier, identifiantCourtier, partCommission, typeOperation, user);
        }
        public void SupprimerCourtier(string codeContrat, string versionContrat, int identifiantCourtier)
        {
            AffaireNouvelleRepository.SupprimerCourtier(codeContrat, versionContrat, identifiantCourtier);
            //InitAffNouv.SupprimerCourtier(codeContrat, versionContrat, identifiantCourtier);
        }
        public void ModifierCommissionCourtier(string codeContrat, string versionContrat, int identifiantCourtier, Single commission)
        {
            AffaireNouvelleRepository.ModifierCommissionCourtier(codeContrat, versionContrat, identifiantCourtier, commission);
            //InitAffNouv.ModifierCommissionCourtier(codeContrat, versionContrat, identifiantCourtier, commission);
        }
        public CommissionCourtierDto GetCommissionsStandardCourtier(string codeContrat, string versionContrat, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion)
        {
            return AffaireNouvelleRepository.GetCommissionsStandardCourtier(codeContrat, versionContrat, type, codeAvn, isReadonly, modeNavig, user, acteGestion);
            //return InitAffNouv.GetCommissionsStandardCourtier(codeContrat, versionContrat, type, codeAvn, isReadonly, modeNavig, user, acteGestion);
        }
        public void UpdateCommissionsStandardCourtier(string codeContrat, string versionContrat, string type, CommissionCourtierDto commissionStandard)
        {
            AffaireNouvelleRepository.UpdateCommissionsStandardCourtier(codeContrat, versionContrat, type, commissionStandard);
            //InitAffNouv.UpdateCommissionsStandardCourtier(codeContrat, versionContrat, type, commissionStandard);
        }

        /// <summary>
        /// Creation d'un contrat via "Etablir affaire nouvelle" à partir d'une offre.
        /// Il est nécessaire de verrouiller l'affaire créée si la copie a fonctionnée
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeContrat"></param>
        /// <param name="versionContrat"></param>
        /// <param name="typeContrat"></param>
        /// <param name="dateAccord"></param>
        /// <param name="dateEffet"></param>
        /// <param name="heureEffet"></param>
        /// <param name="contratRemp"></param>
        /// <param name="versionRemp"></param>
        /// <param name="souscripteur"></param>
        /// <param name="gestionnaire"></param>
        /// <param name="branche"></param>
        /// <param name="cible"></param>
        /// <param name="observation"></param>
        /// <param name="user"></param>
        /// <param name="acteGestion"></param>
        /// <returns></returns>
        public string CreateContrat(string codeOffre, string version, string type, string codeContrat, string versionContrat, string typeContrat, DateTime? dateAccord,
            DateTime? dateEffet, int heureEffet, string contratRemp, string versionRemp, string souscripteur, string gestionnaire, string branche, string cible, string observation, string user,
            string acteGestion)
        {
            string code = AffaireNouvelleRepository.CreateContrat(
                codeOffre, version, type, codeContrat, versionContrat, typeContrat, dateAccord, dateEffet, heureEffet, contratRemp,
                versionContrat, souscripteur, gestionnaire, branche, cible, observation, user, acteGestion);

            var array = (code ?? string.Empty).Split('_');
            if (array.Length > 1) {
                this.affairePort.TryLockAffaire(new AffaireId { CodeAffaire = array[0], NumeroAliment = int.Parse(array[1]), TypeAffaire = AffaireType.Contrat }, "CreateContrat");
            }

            return code;
        }

        public string VerifContratMere(string codeOffre, int version, string branche, string cible)
        {
            return AffaireNouvelleRepository.VerifContratMere(codeOffre, version, branche, cible);
            //return InitAffNouv.VerifContratMere(codeOffre, version, branche, cible);
        }


        public string ControleSousGest(string souscripteur, string gestionnaire)
        {
            return AffaireNouvelleRepository.ControleSousGest(souscripteur, gestionnaire);
        }

        public string ControleValidateOffer(string codeOffre, string version, string type, string dateAccord, string dateEffet)
        {
            return AffaireNouvelleRepository.ControleValidateOffer(codeOffre, version, type, dateAccord, dateEffet);
        }

        public string VerifContratRemp(string codeOffre, int version)
        {
            return AffaireNouvelleRepository.VerifContratRemp(codeOffre, version);
            //return InitAffNouv.VerifContratRemp(codeOffre, version);
        }

        public string GetNumeroAliment(string contratMere)
        {
            return AffaireNouvelleRepository.GetNumeroAliment(contratMere);
            //return InitAffNouv.GetNumeroAliment(contratMere);
        }

        public void UpdateEtatContrat(string codeContrat, long version, string type, string etat)
        {
            AffaireNouvelleRepository.UpdateEtatContrat(codeContrat, version, type, etat);
            //InitAffNouv.UpdateEtatContrat(codeContrat, version, type, etat);
        }

        public void CopyAllInfo(string codeOffre, string version, string type, string codeContrat, string versionContrat, string user, string splitChar, string acteGestion)//, bool isModifHorsAvn)
        {
            AffaireNouvelleRepository.CopyAllInfo(codeOffre, version, type, codeContrat, versionContrat, user, splitChar, acteGestion);//, isModifHorsAvn);
        }

        //public LigneRegularisationDto GetTypeRegul(string codeOffre, string version, string type)
        //{
        //    return AffaireNouvelleRepository.GetTypeRegul(codeOffre, version, type);
        //}

        public void UpdateTypeRegul(string codeOffre, string version, string type, string newType, string dateDebAvn, string codeAvn, string codeReg)
        {
            AffaireNouvelleRepository.UpdateTypeRegul(codeOffre, version, type, newType, dateDebAvn, codeAvn, codeReg);
        }

        public IDictionary<(int, string), List<(int, string)>> FindAvailableOptions(SelectionRisquesObjets rsqObjAffNouv, bool exactMatch = false)
        {
            var result = new Dictionary<(int, string), List<(int, string)>>();
            var apps = this.formuleRepository.GetOptionsFormulesAp(rsqObjAffNouv.Folder);
            foreach (var group in apps.GroupBy(x => new { x.Rsq, x.For, x.Opt }))
            {
                var risque = rsqObjAffNouv.Risques.First(x => x.Code == group.Key.Rsq);
                var first = group.First();
                var key = (group.Key.For, first.LibFor);
                bool matches = false;
                if (first.Obj == 0)
                {
                    if (risque.Selected)
                    {
                        matches = true;
                    }
                }
                else
                {
                    var objOptSelected = risque.Objets.Where(x => x.Selected).Select(x => x.Code).ToList();
                    var objSelected = group.Select(x => x.Obj).ToList();
                    if (!objSelected.Except(objOptSelected).Any())
                    {
                        matches = true;
                    }
                }
                if (matches)
                {
                    string labelApp = $"(risque : {first.LibRsq}";
                    if (group.Any(x => x.Obj > 0))
                    {
                        labelApp += $" - objet{(group.Count() == 1 ? string.Empty : "s")} : {string.Join(",", group.Select(x => x.LibObj))}";
                    }
                    labelApp += ")";
                    if (!result.TryGetValue(key, out var options))
                    {
                        result.Add(key, new List<(int, string)>() { (group.Key.Opt, labelApp) });
                    }
                    else
                    {
                        options.Add((group.Key.Opt, labelApp));
                    }
                }
            }
            return result;
        }

        #endregion
        #region Ecran Choix Risque/Objet Affaire Nouvelle

        public RsqObjAffNouvDto InitRsqObjAffNouv(string codeOffre, string version, string type, string codeContrat, string versionContrat)
        {
            return AffaireNouvelleRepository.InitRsqObjAffNouv(codeOffre, version, type, codeContrat, versionContrat);
            //return InitAffNouv.InitRsqObjAffNouv(codeOffre, version, type, codeContrat, versionContrat);
        }

        public void UpdateRsqObj(string codeContrat, string versionContrat, string type, string codeRsq, string codeObj, string isChecked)
        {
            AffaireNouvelleRepository.UpdateRsqObj(codeContrat, versionContrat, type, codeRsq, codeObj, isChecked);
            //InitAffNouv.UpdateRsqObj(codeContrat, versionContrat, type, codeRsq, codeObj, isChecked);
        }

        #endregion
        #region Ecran Choix Formule/Volet Affaire Nouvelle

        public FormVolAffNouvDto InitFormVolAffNouv(string codeOffre, string version, string type, string codeContrat, string versionContrat)
        {
            return AffaireNouvelleRepository.InitFormVolAffNouv(codeOffre, version, type, codeContrat, versionContrat);
            //return InitAffNouv.InitFormVolAffNouv(codeOffre, version, type, codeContrat, versionContrat);
        }

        public void UpdateFormVol(string codeContrat, string versionContrat, string codeOffre, string version, string typeOffre, string codeForm, string guidForm, string codeOpt,
                string guidOpt, string guidVol, string type, string isChecked)
        {
            AffaireNouvelleRepository.UpdateFormVol(codeContrat, versionContrat, codeOffre, version, typeOffre, codeForm, guidForm, codeOpt,
                 guidOpt, guidVol, type, isChecked);
            PoliceRepository.UpdateIndexation(codeContrat, versionContrat, type);
            //InitAffNouv.UpdateFormVol(codeContrat, versionContrat, codeOffre, version, typeOffre, codeForm, guidForm, codeOpt,
            //     guidOpt, guidVol, type, isChecked);
        }

        public FormVolAffNouvRecapDto GetListRsqForm(string codeOffre, string version, string type, string codeContrat, string versionContrat, string user, string splitChar, string acteGestion, bool isModifHorsAvn)
        {
            FormVolAffNouvRecapDto result = AffaireNouvelleRepository.GetListRsqForm(codeContrat, versionContrat);
            if (result != null && result.CountGarTar <= 0)
            {
                ValidContrat(codeOffre, version, type, codeContrat, versionContrat, user, splitChar, isModifHorsAvn, acteGestion);
            }
            CommonRepository.ChangeSbr(codeOffre, version, type, user);
            return result;
        }

        #endregion
        #region Ecran Choix Options tarif

        public OptTarAffNouvDto InitOptTarifAffNouv(string codeContrat, string versionContrat)
        {
            return AffaireNouvelleRepository.InitOptTarifAffNouv(codeContrat, versionContrat);
            //return InitAffNouv.InitOptTarifAffNouv(codeContrat, versionContrat);
        }

        public void UpdateOptTarif(string codeContrat, string versionContrat, string guidTarif)
        {
            AffaireNouvelleRepository.UpdateOptTarif(codeContrat, versionContrat, guidTarif);
            //InitAffNouv.UpdateOptTarif(codeContrat, versionContrat, guidTarif);
        }

        public void ValidContrat(string codeOffre, string version, string type, string codeContrat, string versionContrat, string user, string splitChar, bool isModifHorsAvn, string acteGestion)
        {
            AffaireNouvelleRepository.ValidContrat(codeOffre, version, type, codeContrat, versionContrat, user, splitChar, isModifHorsAvn, acteGestion);
            //InitAffNouv.ValidContrat(codeOffre, version, type, codeContrat, versionContrat, user, splitChar, acteGestion);
        }

        #endregion
        #region CoAssureurs

        #region Méthodes publiques

        public FormCoAssureurDto InitCoAssureurs(string type, string idOffre, string idAliment, string codeAvn, ModeConsultation modeNavig)
        {
            return AffaireNouvelleRepository.InitCoAssureurs(type, idOffre, idAliment, codeAvn, modeNavig);
            //return InitAffNouv.InitCoAssureurs(type, idOffre, idAliment, codeAvn, modeNavig);
        }

        public bool ExistCoAs(string idContrat, string version, string type, ModeConsultation modeNavig)
        {
            return AffaireNouvelleRepository.ExistCoAs(idContrat, version, type, modeNavig);
            //return InitAffNouv.ExistCoAs(idContrat, version, type, modeNavig);
        }

        public CoAssureurDto GetCoAssureurDetail(string type, string idOffre, string idAliment, string idCoAssureur, ModeConsultation modeNavig, bool modeCoAss)
        {
            return AffaireNouvelleRepository.GetCoAssureurDetail(type, idOffre, idAliment, idCoAssureur, modeCoAss);
            //return InitAffNouv.GetCoAssureurDetail(type, idOffre, idAliment, idCoAssureur, modeCoAss);
        }

        public string EnregistrerListeCoAssureurs(string code, string version, string type, string typeAvenant, string avenant, List<CoAssureurDto> listeCoass, string user)
        {
            string message = AffaireNouvelleRepository.EnregistrerListeCoAssureurs(code, version, type, typeAvenant, avenant, listeCoass, user);
            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = code.PadLeft(9, ' '),
                Version = Convert.ToInt32(version),
                Type = type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoAssureur),
                NumeroOrdreDansEtape = 15,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoAssureur),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });
            return message;
            //return InitAffNouv.EnregistrerListeCoAssureurs(code, version, type, typeAvenant, avenant, listeCoass, user);
        }

        public string EnregistrerCoAssureur(string type, string idOffre, string idAliment, CoAssureurDto coAssureur, string typeOperation, string user)
        {
            string result = AffaireNouvelleRepository.EnregistrerCoAssureur(type, idOffre, idAliment, coAssureur, typeOperation);
            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = idOffre.PadLeft(9, ' '),
                Version = Convert.ToInt32(idAliment),
                Type = type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoAssureur),
                NumeroOrdreDansEtape = 15,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoAssureur),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });

            return result;
            //return InitAffNouv.EnregistrerCoAssureur(type, idOffre, idAliment, coAssureur, typeOperation, user);
        }

        public string SupprimerCoAssureur(string type, string idOffre, string idAliment, string guidId)
        {
            return AffaireNouvelleRepository.SupprimerCoAssureur(type, idOffre, idAliment, guidId);
            //return InitAffNouv.SupprimerCoAssureur(type, idOffre, idAliment, guidId);
        }

        #endregion

        #endregion
        public double GetMontantStatistique(string codeContrat, string version)
        {
            return AffaireNouvelleRepository.GetMontantStatistique(codeContrat, version);
            //return InitAffNouv.GetMontantStatistique(codeContrat, version);
        }
        #region retours signatures
        public List<ParametreDto> GetListeTypesAccord()
        {
            return AffaireNouvelleRepository.GetListeTypesAccord();
            //return InitAffNouv.GetListeTypesAccord();
        }

        //public RetourPreneurDto GetRetourPreneur(string codeContrat, string versionContrat, string typeContrat,ModeConsultation modeNavig)
        //{
        //    return AffaireNouvelleRepository.GetRetourPreneur(codeContrat, versionContrat, typeContrat, modeNavig);
        //    //return InitAffNouv.GetRetourPreneur(codeContrat, versionContrat, typeContrat);
        //}

        public List<RetourCoassureurDto> GetRetoursCoassureurs(string codeContrat, string versionContrat, string typeContrat)
        {
            return AffaireNouvelleRepository.GetRetoursCoassureurs(codeContrat, versionContrat, typeContrat);
            //return InitAffNouv.GetRetoursCoassureurs(codeContrat, versionContrat, typeContrat);
        }

        public RetourPreneurDto GetRetourPreneur2(string codeContrat, string version, string type, string codeAvt, ModeConsultation modeNavig)
        {
            return AffaireNouvelleRepository.GetRetourPreneur(codeContrat, version, type, codeAvt, modeNavig);
            //return InitAffNouv.GetRetourPreneur(codeContrat, version, type, codeAvt, modeNavig);
        }

        public List<RetourCoassureurDto> GetRetoursCoassureurs2(string codeContrat, string version, string type, string codeAvt, ModeConsultation modeNavig)
        {
            return AffaireNouvelleRepository.GetRetoursCoassureurs(codeContrat, version, type, codeAvt, modeNavig);
            //return InitAffNouv.GetRetoursCoassureurs(codeContrat, version, type, codeAvt, modeNavig);
        }

        public void EnregistrerRetours(string codeContrat, string versionContrat, string typeContrat, string codeAvt, RetourPreneurDto retourPreneur, List<RetourCoassureurDto> retoursCoAssureurs, ModeConsultation modeNavig, string user, bool isModifHorsAvn)
        {
            if (modeNavig == ModeConsultation.Historique)
                AffaireNouvelleRepository.EnregistrerRetoursHisto(codeContrat, versionContrat, typeContrat, codeAvt, retourPreneur, retoursCoAssureurs, modeNavig);
            else
                AffaireNouvelleRepository.EnregistrerRetours(codeContrat, versionContrat, typeContrat, codeAvt, retourPreneur, retoursCoAssureurs, user, isModifHorsAvn);
            //InitAffNouv.EnregistrerRetours(codeContrat, versionContrat, typeContrat, codeAvt, retourPreneur, retoursCoAssureurs, modeNavig, user);
        }

        #endregion

        #region Création Template

        public ContratInfoBaseDto GetInfoTemplate(string idTemp)
        {
            return AffaireNouvelleRepository.GetInfoTemplate(idTemp);
            //return InitAffNouv.GetInfoTemplate(idTemp);
        }

        #endregion

        #region Blocage termes
        public List<ParametreDto> GetListeZonesStop()
        {
            return AffaireNouvelleRepository.GetListeZonesStop();
            //return InitAffNouv.GetListeZonesStop();
        }

        public void SaveZoneStop(string codeContrat, string versionContrat, string typeContrat, string zoneStop, string user)
        {
            BLCommon.SaveZoneStop(codeContrat, versionContrat, typeContrat, zoneStop, user);
            //var oldZoneStop = InitAffNouv.GetZoneStop(codeContrat, versionContrat, typeContrat);
            //var libTraitement = string.Empty;
            //if (!string.IsNullOrEmpty(oldZoneStop) && string.IsNullOrEmpty(zoneStop))
            //{
            //    libTraitement = "Déblocage en code " + oldZoneStop;
            //}
            //else if (!string.IsNullOrEmpty(zoneStop))
            //{
            //    libTraitement = "Blocage en code " + zoneStop;
            //}
            //InitAffNouv.SaveZoneStop(codeContrat, versionContrat, typeContrat, zoneStop);
            //#region Ajout de l'acte de gestion
            //CommonRepository.AjouterActeGestion(codeContrat, versionContrat, typeContrat, 0, AlbConstantesMetiers.ACTEGESTION_VALIDATION, AlbConstantesMetiers.TRAITEMENT_BQDBQ, libTraitement, user);
            //#endregion
        }

        public DeblocageTermeDto GetEcheanceEmission(string codeContrat, string versionContrat, string typeContrat, string mode, string user, string acteGestion, AlbConstantesMetiers.DroitBlocageTerme niveauDroit, bool emission)
        {
            return AffaireNouvelleRepository.GetEcheanceEmission(codeContrat, versionContrat, typeContrat, mode, user, acteGestion, niveauDroit, emission);
            //return InitAffNouv.GetEcheanceEmission(codeContrat, versionContrat, typeContrat, mode, user, acteGestion);
        }

        #endregion

        public void RemoveControlAssiette(string codeContrat, string versionContrat, string typeContrat)
        {
            CommonRepository.RemoveControlAssiette(codeContrat, versionContrat, typeContrat);
        }

        public bool CheckControlAssiette(string codeContrat, string versionContrat, string typeContrat)
        {
            return CommonRepository.CheckControlAssiette(codeContrat, versionContrat, typeContrat);
        }

        public string ChangePreavisResil(string codeContrat, string version, string codeAvn, string dateEffet, string dateFinEffet, string dateAvenant, string periodicite, string echeancePrincipale, string splitCharHtml, string user, string acteGestion)
        {
            return BLCommon.ChangePreavisResil(codeContrat, version, codeAvn, dateEffet, dateFinEffet, dateAvenant, periodicite, echeancePrincipale, splitCharHtml, user, acteGestion);
            //return InitAffNouv.ChangePreavisResil(codeContrat, version, dateEffet, dateFinEffet, dateAvenant, periodicite, echeancePrincipale, splitCharHtml, user, acteGestion);
        }

        public string ControleEcheance(string prochaineEcheance, string periodicite, string echeancePrincipale)
        {
            return CommonRepository.ControleEcheance(prochaineEcheance, periodicite, echeancePrincipale);
            //return InitAffNouv.ControleEcheance(prochaineEcheance, periodicite, echeancePrincipale);
        }

        public bool ContratHasQuittances(string codeOffre, string version, string type)
        {
            return AffaireNouvelleRepository.ContratHasQuittances(codeOffre, version, type);
        }

        #region Ecran Création Avenant

        public ContratDto GetInfoRegulPage(string codeOffre, string version, string type, string codeAvn)
        {
            return AffaireNouvelleRepository.GetInfoRegulPage(codeOffre, version, type, codeAvn);
        }

        public AvenantInfoPageDto GetInfoAvenantPage(string codeOffre, string version, string type, string codeAvn, string typeAvt, string modeAvt, string user, string modeNavig)
        {
            return BLCommon.GetInfoAvenantPage(codeOffre, version, type, codeAvn, typeAvt, modeAvt, user, modeNavig);
        }

        public AvenantDto GetInfoAvenant(string codeOffre, string version, string type, short numAvn, string codeAvn, string typeAvt, string modeAvt, string user, string modeNavig)
        {
            return BLCommon.GetInfoAvenant(codeOffre, version, type, codeAvn, typeAvt, modeAvt, user, modeNavig);
        }

        public AvenantModificationDto GetInfoAvenantModification(string codeOffre, string version, string type, short numAvn, string typeAvt, string modeAvt, string modeNavig)
        {
            return AvenantRepository.GetInfoAvenantModification(codeOffre, version, type, numAvn, typeAvt, modeAvt, modeNavig);
        }

        public List<ParametreDto> ReloadAvenantResilMotif()
        {
            return AvenantRepository.ReloadAvenantResilMotif();
        }

        #endregion

        #region Ecran Création Attestation

        /// <summary>
        /// Charge les informations générales de l'attestation
        /// </summary>
        public AttestationDto GetInfoAttestation(string codeContrat, string version, string type, string user)
        {
            return BLCommon.GetInfoAttestation(codeContrat, version, type, user);
        }

        /// <summary>
        /// Recharge les périodes en fonction
        /// de l'exercice ou périodes en paramètres
        /// </summary>
        public string ChangeExercicePeriode(string codeContrat, string version, string type, int exercice, DateTime? periodeDeb, DateTime? periodeFin)
        {
            return BLCommon.ChangeExercicePeriode(codeContrat, version, type, exercice, periodeDeb, periodeFin);
        }

        /// <summary>
        /// Ouvre l'onglet de sélection de risques/objets
        /// </summary>
        public AttestationSelRsqDto OpenTabRsqObj(string codeContrat, string version, string type, string codeAvn,
            string lotId, string exercice, DateTime? periodeDeb, DateTime? periodeFin, string typeAttes, bool integralite, string user)
        {
            return BLCommon.OpenTabRsqObj(codeContrat, version, type, codeAvn, lotId, exercice, periodeDeb, periodeFin, typeAttes, integralite, user);
        }

        /// <summary>
        /// Ouvre l'onglet de sélection de garanties
        /// </summary>
        public AttestationSelGarDto OpenTabGarantie(string codeContrat, string version, string type, string codeAvn,
               string lotId, string exercice, DateTime? periodeDeb, DateTime? periodeFin, string typeAttes, bool integralite, string user)
        {
            return BLCommon.OpenTabGarantie(codeContrat, version, type, codeAvn, lotId, exercice, periodeDeb, periodeFin, typeAttes, integralite, user);
        }

        /// <summary>
        /// Recharge la liste des garanties pour l'attestation
        /// </summary>
        public AttestationSelGarDto LoadAttestationGarantie(string codeContrat, string version, string type, string lotId)
        {
            return BLCommon.LoadAttestationGarantie(codeContrat, version, type, lotId);
        }

        /// <summary>
        /// Valide la sélection de risques/objets
        /// </summary>
        public string ValidSelectRsqObj(string codeContrat, string version, string type, string lotId, string selRsqObj, string user)
        {
            return AttestationRepository.ValidSelectRsqObj(codeContrat, version, type, lotId, selRsqObj, user);
        }

        /// <summary>
        /// Valide la sélection de garanties
        /// </summary>
        public string ValidSelectionGar(string codeContrat, string version, string type, string lotId, string selGarantie, string user)
        {
            return AttestationRepository.ValidSelectionGar(codeContrat, version, type, lotId, selGarantie, user);
        }

        /// <summary>
        /// Valide les périodes pour l'attestation
        /// </summary>
        public string ValidPeriodeAttestation(string codeContrat, string version, string type, string lotId, string exercice, DateTime? periodeDeb, DateTime? periodeFin,
            string typeAttes, bool integralite, string user)
        {
            return AttestationRepository.ValidPeriodeAttestation(codeContrat, version, type, lotId, exercice, periodeDeb, periodeFin, typeAttes, integralite, user);
        }

        #endregion

        #region Ecran Création Régularisation

        public RegularisationDto GetInfoRegularisation(string codeContrat, string version, string type, string codeAvn, string user)
        {
            return BLCommon.GetInfoRegularisation(codeContrat, version, type, codeAvn, user);
        }

        public RegularisationInfoDto GetInfoAvenantRegule(string codeContrat, string version, string type, string typeAvt, string modeAvt, string user, string lotId, string reguleId, string regulMode)
        {
            return RegularisationRepository.GetInfoAvenantRegule(codeContrat, version, type, typeAvt, modeAvt, 0, null, null, user, lotId, reguleId, regulMode);
        }

        public RegularisationInfoDto GetInfoAvnRegule(string codeContrat, string version, string type, string codeAvn, string modeAvt, int exercice, DateTime? periodeDeb, DateTime? periodeFin)
        {
            return BLCommon.GetInfoAvnRegule(codeContrat, version, type, codeAvn, modeAvt, exercice, periodeDeb, periodeFin);
        }

        public RegularisationInfoDto GetAvnRegule(string codeContrat, string version, string type, string codeAvn, string typeAvt, int exercice, DateTime? periodeDeb, DateTime? periodeFin, string user, string lotId, string reguleId, string regulMode, string deleteMod, string cancelMod, bool resetLot = false)
        {
            return RegularisationRepository.GetAvnRegule(codeContrat, version, type, codeAvn, typeAvt, exercice, periodeDeb, periodeFin, user, lotId, reguleId, regulMode, deleteMod, cancelMod, resetLot);
        }
        public void DeleteReguleP(string reguleId)
        {
            RegularisationRepository.DeleteReguleP(reguleId);
        }
        public RegularisationRsqDto GetListRsqRegule(string lotId, string reguleId, string mode)
        {
            return BLCommon.GetListRsqRegul(lotId, reguleId, mode);
        }

        public List<RisqueDto> ReloadListRsqRegule(string lotId, string reguleId, bool isReadonly)
        {
            return RegularisationRepository.ReloadListRsqRegule(lotId, reguleId, isReadonly);
        }

        public RegularisationGarDto GetListGarRegule(string lotId, string reguleId, string codeRsq, bool isReadonly)
        {
            return BLCommon.GetListGarRegule(lotId, reguleId, codeRsq, isReadonly);
        }

        public RisqueDto GetAppliqueRegule(string codeContrat, string version, string type, string codeAvn, string codeFor)
        {
            return RegularisationRepository.GetAppliqueRegule(codeContrat, version, type, codeAvn, codeFor);
        }

        public List<LigneRegularisationDto> GetListeRegularisation(string codeContrat, string version, string type)
        {
            return RegularisationRepository.GetListeRegularisation(codeContrat, version, type);
        }

        public List<LigneRegularisationDto> DeleteRegule(string codeContrat, string version, string type, string codeAvn, string reguleId)
        {
            return BLCommon.DeleteRegule(codeContrat, version, type, codeAvn, reguleId);
        }

        public string MouvementsGarPeriodeAS400(string codeContrat, string version, string type, string rsq, string codfor, string garan, string idlot, string idregul)
        {
            return RegularisationRepository.MouvementsGarantiePeriode(codeContrat, version, type, rsq, codfor, garan, idlot, idregul);
        }


        public RegularisationGarInfoDto GetInfoRegularisationGarantie(RegularisationContext context)
        {
            return RegularisationManager.GetInfoRegularisationGarantie(context);
        }

        public AjoutMouvtGarantieDto AjouterMouvtPeriod(string codeContrat, string version, string type, string codersq, string codfor, string codegar, string idregul, string idlot, int datedeb, int datefin)
        {
            return BLCommon.AjouterMouvtPeriod(codeContrat, version, type, codersq, codfor, codegar, idregul, idlot, datedeb, datefin);
        }

        public List<LigneMouvtGarantieDto> ReloadMouvtPeriod(string codeAffaire, string version, string type, string codeRsq, string codeFor, string codeGar, string codeRegul)
        {
            return RegularisationRepository.ReloadMouvtPeriod(codeAffaire, version, type, codeRsq, Convert.ToInt32(codeFor), codeGar, codeRegul);
        }

        public List<LigneMouvtGarantieDto> GetListDatesPeriod(string codeContrat, string version, string type, string reguleId, string codersq, string codfor, string codegar)
        {
            return RegularisationRepository.GetListDatesPeriod(codeContrat, version, type, reguleId, codersq, codfor, codegar);
        }
        public string CheckDatesPeriodAllRsqIntegrity(string codeContrat, string version, string type, string idLot, string typAvt, string dateDebReg, string dateFinReg, string reguleId)
        {
            return RegularisationRepository.CheckDatesPeriodAllRsqIntegrity(codeContrat, version, type, idLot, typAvt, dateDebReg, dateFinReg, reguleId);
        }
        public SaisieInfoRegulPeriodDto InitSaisieGarRegul(string idRegulGar, string codeAvenant)
        {
            return RegularisationRepository.InitSaisieGarRegul(idRegulGar, codeAvenant);
        }

        public SaisieInfoRegulPeriodDto ReloadSaisieGarRegul(string codeContrat, string version, string type, string codeAvenant, string idregulgar, string codeRsq, string assiettePrev, string valeurPrev, string unitePrev, string codetaxePrev, string assiettedef, string valeurdef, string uniteDef, string codetaxeDef, string cotisForceHT, string cotisForceTaxe, string coeff)
        {
            return RegularisationRepository.ReloadSaisieGarRegul(codeContrat, version, type, codeAvenant, idregulgar, codeRsq, assiettePrev, valeurPrev, unitePrev, codetaxePrev, assiettedef, valeurdef, uniteDef, codetaxeDef, cotisForceHT, cotisForceTaxe, coeff);
        }

        public List<RegulMatriceDto> GetRegulMatrice(string codeAffaire, int version, string type, string lotId, long rgId = 0)
        {
            return RegularisationRepository.GetRegulMatrice(codeAffaire, version, type, lotId, rgId);
        }

        public bool IsValidRegul(string reguleId)
        {
            return RegularisationRepository.IsValidRegul(reguleId);
        }

        public string ValidSaisiePeriodRegule(string codeContrat, string version, string type, string codeAvn, string codeRsq, string reguleGarId, string typeRegule, SaisieGarInfoDto modelDto)
        {
            return RegularisationRepository.ValidSaisiePeriodRegule(codeContrat, version, type, codeAvn, codeRsq, reguleGarId, typeRegule, modelDto);
        }

        public ConfirmSaisieReguleDto GetPopupConfirm(string reguleGarId)
        {
            return RegularisationRepository.GetPopupConfirm(reguleGarId);
        }

        #endregion

        #region Ecran Infos Générales Avenant

        public AvenantInfoDto GetAvenant(string codeOffre, string version, string type, string codeAvenant, ModeConsultation modeNavig)
        {
            return BLCommon.GetAvenant(codeOffre, version, type, codeAvenant, modeNavig);
            //return BLAvenant.GetAvenant(codeOffre, version, type, codeAvenant, modeNavig);
        }

        public void SupprimerEcheances(string codeOffre, string version, string type)
        {
            FinOffreRepository.SupprimerEcheances(codeOffre, version, type);
            //BLFinOffre.SupprimerEcheances(codeOffre, version, type);
        }

        public void EnregistrerAvenant(ContratDto avenant, string user, bool isModifHorsAvn)
        {
            AvenantRepository.EnregistrerAvenant(avenant, user);

            //BLAvenant.EnregistrerAvenant(avenant, user);
            #region Acte de gestion
            if (!isModifHorsAvn)
                CommonRepository.AjouterActeGestion(avenant.CodeContrat, avenant.VersionContrat.ToString(), avenant.Type, avenant.NumAvenant, AlbConstantesMetiers.ACTEGESTION_GESTION, AlbConstantesMetiers.TRAITEMENT_AVNMD, "", user);
            #endregion
        }

        #endregion

        #region ParamCibleRecup
        /// <summary>
        /// chercher l'offre et lancer le programme AS400 de recup
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public OffreRecupDto RecupOffre(string codeOffre, string version)
        {
            //recherche offre
            OffreRecupDto offre = OffreRepository.GetOffreRecup(codeOffre, version);
            if (offre != null && !string.IsNullOrEmpty(offre.Type))
            {
                //exec programme AS400
                CommonRepository.RecupOffreContratAS400(codeOffre, version, offre.Type);

                ////update PBORK='KHE'
                //OffreRepository.UpdateOffreRecup(codeOffre, version);

                //reselect la cible car le programme AS400 change la cible
                offre = OffreRepository.GetOffreAfterRecup(codeOffre, version);
                if (offre != null)
                    return offre;
            }

            offre = new OffreRecupDto();
            offre.Erreur = string.Format("L'offre ou l'affaire nouvelle {0} / {1} est indiponible pour la récupération", codeOffre, version);

            return offre;
        }

        /// <summary>
        /// lancer la migration
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="fromCible"></param>
        /// <param name="toCible"></param>
        /// <returns></returns>
        public bool MigrationOffre(string codeOffre, string version, string type, string fromCible, string toCible)
        {
            try
            {
                OffreRepository.UpdateCible(codeOffre, version, type, fromCible, toCible);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        public SelectionRisquesObjets GetOffreSelections(Folder offre, Folder contrat)
        {
            int nbSelections = this.repository.GetCountOffreSelections(offre, contrat);
            if (nbSelections == 0)
            {
                this.repository.InitOffreSelectionRisques(offre, contrat);
                InitOffreSelectionFormules(offre, contrat);
            }
            var selectionData = this.repository.GetOffreSelections(offre, contrat);
            var selections = new SelectionRisquesObjets
            {
                AvailableOptions = new List<SelectionFormuleOption>(),
                CodeAffaireNouvelle = contrat.CodeOffre,
                Folder = offre,
                Risques = new List<SelectionRisqueObjets>()
            };
            selections.Risques.AddRange(selectionData.GroupBy(x => x.Rsq).Select(g =>
            {
                var rsq = g.First(e => e.Obj == 0);
                var listObjets = g.Where(o => o.Obj > 0).GroupBy(o => o.Obj);
                var risque = new SelectionRisqueObjets
                {
                    Selected = rsq.Sel.AsBoolean().GetValueOrDefault() || listObjets.Any(x => x.First().Sel.AsBoolean() ?? false),
                    DateDebut = rsq.Deb.GetValueOrDefault() == default ? null : rsq.Deb,
                    DateFin = rsq.Fin.GetValueOrDefault() == default ? null : rsq.Fin,
                    Name = rsq.Desc,
                    Valeur = rsq.Val,
                    CodeTypeValeur = rsq.TypeVal,
                    CodeUniteValeur = rsq.Unit,
                    Code = rsq.Rsq,
                    Objets = new List<SelectionObjet>(g.Where(o => o.Obj > 0).GroupBy(o => o.Obj).Select(g1 =>
                    {
                        var objet = g1.First();
                        return new SelectionObjet
                        {
                            Code = objet.Obj,
                            DateDebut = objet.Deb.GetValueOrDefault() == default ? null : objet.Deb,
                            DateFin = objet.Fin.GetValueOrDefault() == default ? null : objet.Fin,
                            Name = objet.Desc,
                            Selected = objet.Sel.AsBoolean().GetValueOrDefault() || rsq.Sel.AsBoolean().GetValueOrDefault(),
                            TypeValeur = objet.TypeVal,
                            UniteValeur = objet.Unit,
                            Valeur = objet.Val
                        };
                    }))
                };
                return risque;
            }));
            return selections;
        }

        public IDictionary<Folder, (DateTime? debut, DateTime? fin)> GetDatesEffets(IEnumerable<Folder> folders)
        {
            var data = this.repository.GetDatesEffets(folders.Select(x => x.CodeOffre)).ToList();
            foreach (var f in folders)
            {
                data.RemoveAll(x => x.Ipb == f.CodeOffre && x.Alx != f.Version);
            }
            return data.ToDictionary(x => new Folder(x.Ipb, x.Alx, x.Typ[0]), x => (x.Debut, x.Fin));
        }

        public void CorrectionECM(string codeContrat, string versionContrat, string splitChar, string user, string acteGestion, bool isModifHorsAvn)
        {
            AffaireNouvelleRepository.CorrectionECM(codeContrat, versionContrat, splitChar, user, acteGestion);

        }

        public LigneRegularisationDto GetLastValidInfoAvnRegul(string codeContrat, string version, string type)
        {
            return RegularisationRepository.GetLastValidInfoAvnRegul(codeContrat, version, type);
        }

        public IEnumerable<ContratConnexeDto> GetConnexites(Folder folder, TypeConnexite type)
        {
            return this.connexiteService.GetConnexites(folder, type);
        }

        public IEnumerable<ContratConnexeDto> GetAllConnexites(Folder folder)
        {
            return this.connexiteService.GetConnexites(folder);
        }

        public void AddPeriodeEngagement(PeriodeConnexiteDto periode)
        {
            this.connexiteService.CreateOrUpdateEngagement(null, periode);
        }

        public IEnumerable<PeriodeConnexiteDto> GetPeriodesEngagements(int numeroConnexite)
        {
            var periodes = this.connexiteService.GetPeriodesEngagements(numeroConnexite);
            if (periodes.Any())
            {
                var folders = this.connexiteService.GetFolders(TypeConnexite.Engagement, numeroConnexite);
                var foldersEffets = GetDatesEffets(folders);
                var pr = periodes.First();
                pr.DateDebut = foldersEffets.Values.Min(x => x.debut);
                pr = periodes.Last();
                pr.DateFin = foldersEffets.Values.Any(d => !d.fin.HasValue) ? null : foldersEffets.Values.Max(x => x.fin);
            }
            return periodes;
        }

        public (string pbbra, string pbsbr, string pbcat, string kaacible) GetCiblage(Folder folder, ModeConsultation modeConsultation)
        {
            if (modeConsultation == ModeConsultation.Historique)
            {
                this.repository.SetHistoryMode(ActivityMode.Active);
            }
            var ciblage = this.repository.GetCiblage(folder);
            return ciblage;
        }

        public void AddConnexite(Folder folder, ContratConnexeDto contratConnexe, string user)
        {
            this.connexiteService.ConnectContrat(folder, contratConnexe, user);
        }

        public void RemoveConnexite(Folder folder, ContratConnexeDto contratConnexe)
        {
            this.connexiteService.DetachContrat(folder, new Folder(contratConnexe.NumContrat, contratConnexe.VersionContrat, contratConnexe.TypeContrat[0]), (TypeConnexite)int.Parse(contratConnexe.CodeTypeConnexite));
        }

        public void MergeConnexites(FusionConnexitesDto fusionDto) {
            this.connexiteService.Merge(fusionDto);
        }

        public void PickTargetToConnexites(FusionConnexitesDto fusionDto) {
            this.connexiteService.PickTarget(fusionDto);
        }

        public void MoveSourceInConnexites(FusionConnexitesDto fusionDto) {
            this.connexiteService.MoveSource(fusionDto);
        }

        public void ModifyPeriodesEngagements(Folder folder, IEnumerable<PeriodeConnexiteDto> periodes)
        {
            this.connexiteService.ModifyPeriodesEngagements(folder, periodes);
        }

        public void SaveNewAffair(Folder offre, Folder contrat, string user)
        {
            try
            {
                this.repository.SP_AFFNOUV(
                        offre.CodeOffre.ToIPB(), offre.Version,
                        AlbConstantesMetiers.TYPE_OFFRE,
                        contrat.CodeOffre.ToIPB(), contrat.Version,
                        DateTime.Now.ToString("yyyyMMdd"),
                        user, AlbConstantesMetiers.Traitement.Police.AsCode());

                this.engagementService.InitCopyEngagements(offre, contrat, AlbConstantesMetiers.TRAITEMENT_AFFNV, user);
                this.documentService.CopyDocuments(contrat);
                RecalculEcheancePrincipale(contrat, AlbConstantesMetiers.TRAITEMENT_AFFNV, user);
                var data = this.policeRepository.GetDateEffetEtFrais(contrat);
                var pgmFolder = new PGMFolder(contrat) { ActeGestion = AlbConstantesMetiers.TRAITEMENT_AFFNV, User = user };
                this.as400Repository.LancementCalculAffaireNouvelle(pgmFolder, data);
                this.repository.SetOption1ForNewAffair(contrat);
                this.traceService.TraceActeGestion(
                    contrat,
                    user,
                    AlbConstantesMetiers.ACTEGESTION_GESTION,
                    $"Création via {contrat.CodeOffre}-{contrat.Version}",
                    AlbConstantesMetiers.TRAITEMENT_AFFNV);

                try
                {
                    this.repository.SP_CANAFNO(offre.CodeOffre.ToIPB(), offre.Version);
                }
                catch (Exception ex2)
                {
                    AlbLog.Warn($"Impossible de supprimer les sélection temporaires pour l'affaire nouvelle {contrat?.CodeOffre}{Environment.NewLine}{ex2}");
                }
            }
            catch (Exception ex1)
            {
                AlbLog.Warn($"Impossible de créer l'affaire nouvelle {contrat?.CodeOffre}{Environment.NewLine}{ex1}");
                try
                {
                    this.repository.SP_RESET_NEW_AFFAIR(contrat.CodeOffre.ToIPB(), contrat.Version, contrat.Type);
                }
                catch (Exception ex3)
                {
                    AlbLog.Warn($"Impossible de réinitialiser l'affaire nouvelle {contrat?.CodeOffre}{Environment.NewLine}{ex3}");
                }
                throw;
            }
        }

        private void RecalculEcheancePrincipale(Folder contrat, string acteGestion, string user)
        {
            var echeance = this.repository.GetProchaineEcheanceAffaireNouvelle(contrat)?.FirstOrDefault();
            if (echeance != null && echeance.Periodicite == "A")
            {
                var ddEffet = AlbConvert.ConvertStrToDate(echeance.EffetGarantie);
                var dfEffet = AlbConvert.ConvertStrToDate(echeance.FinGarantie);
                var dAvenant = AlbConvert.ConvertStrToDate(echeance.EffetGarantie);
                DateTime? dEcheance = null;
                if (!string.IsNullOrEmpty(echeance.EcheancePrincipale))
                {
                    dEcheance = AlbConvert.ConvertStrToDate(echeance.EcheancePrincipale + "/2012");
                }
                if (!string.IsNullOrEmpty(echeance.DureeUnite))
                {
                    dfEffet = AlbConvert.GetFinPeriode(AlbConvert.ConvertStrToDate(echeance.EffetGarantie), echeance.Duree, echeance.DureeUnite);
                }
                string dataProchEch = string.Empty;
                if (dEcheance != null)
                {
                    dataProchEch = this.as400Repository.LoadPreavisResiliation(new PGMFolder(contrat) { ActeGestion = acteGestion, User = user }, ddEffet, dfEffet, dAvenant, echeance.Periodicite, dEcheance, "#**#");
                }
                if (dataProchEch.ContainsChars())
                {
                    string[] tDataProchEch = dataProchEch.Split(new[] { "#**#" }, StringSplitOptions.None);
                    this.repository.UpdateProchaineEcheanceAffaireNouvelle(
                        contrat,
                        AlbConvert.ConvertStrToDate(tDataProchEch[2]),
                        AlbConvert.ConvertStrToDate(tDataProchEch[0]),
                        AlbConvert.ConvertStrToDate(tDataProchEch[1]));
                }
            }
        }

        private void InitOffreSelectionFormules(Folder offre, Folder contrat)
        {
            var flatApplications = this.repository.GetApplicationsFrmlOpt(offre);
            foreach (var grp in flatApplications.GroupBy(x => x.IdFor))
            {
                var formule = grp.First();
                foreach (var g in grp.GroupBy(x => x.IdOpt))
                {
                    this.repository.AddOffreSelectionOption(g.First(), contrat);
                }
                formule.Opt = 0;
                formule.IdOpt = 0;
                this.repository.AddOffreSelectionFormule(formule, contrat);
            }
        }

        #region Trace contrat
        /// <summary>
        /// B3101
        /// Vérification du trace de la date fin effet
        /// </summary>
        /// <param name="contratId"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HaveTraceOfEndEffectDate(string contratId, string version, string type, string numAvn)
        {
            var result = false;
            try
            {
                result = AffaireNouvelleRepository.HaveTraceOfEndEffectDate(contratId, version, type, numAvn);
            }
            catch (Exception)
            {

                throw;
            }
            return result;

        }
        /// <summary>
        /// B3101
        /// Enregistrement de trace contrat
        /// </summary>
        /// <param name="contratId"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="numAvn"></param>
        /// <param name="user"></param>
        public void TraceContratInfoModifHorsAvn(string contratId, string version, string type, string numAvn, string user, string codeRisque = null, bool regulTrace = false)
        {
            try
            {
                VerouillageOffresRepository.TraceContratInfoModifHorsAvn(contratId, version, type, numAvn, user, codeRisque, regulTrace);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Récuperation de la date fin effet
        /// </summary>
        /// <param name="contratId"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <param name="modeNavig"></param>
        /// <returns></returns>
        public ContratDto GetEndEffectDate(string contratId, string version, string type)
        {
            return AffaireNouvelleRepository.GetEndEffectDate(contratId, version, type);

        }
        /// <summary>
        /// B2568
        /// Trace résilisation en modif hors avenant
        /// </summary>
        /// <param name="contratId"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="numAvn"></param>
        /// <param name="user"></param>
        /// <param name="TraceType"></param>
        public void TraceResiliation(string contratId, string version, string type, string numAvn, string user, ResiliationTraceType TraceType)
        {
            try
            {
                VerouillageOffresRepository.TraceResiliation(contratId, version, type, numAvn, user, TraceType);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //DbConnection?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AffaireNouvelle() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region Gestion partenaires
        /// <summary>
        /// Obtenir la liste des partenaires de base :
        /// Courtier gestionnaire
        /// Interlocuteur
        /// Courtier apporteur
        /// Courtier payeur
        /// Preneur d'assurance
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="version">Version</param>
        /// <param name="type">Type</param>
        /// <param name="codeAvn">Code AVN</param>
        /// <returns></returns>
        public PartenairesBaseDto GetListPartenairesInfosBase(string code, string version, string type, string codeAvn)
        {
            return CommonRepository.GetListPartenairesInfosBase(code, version, type, codeAvn);
        }
        /// <summary>
        /// Obtenir tous les partenaires :
        /// Partenaires de base (courtiers ,interlocuteur et preneur d'assurance)
        /// Assurés additionnels
        /// Co-assureurs
        /// Intervenants
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="version">Version</param>
        /// <param name="type">Type</param>
        /// <param name="codeAvn">Code AVN</param>
        /// <param name="modeNavig">mode Navig</param>
        public PartenairesDto GetListPartenaires(string code, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            // 01- Infos base
            var partenaires = new PartenairesDto(GetListPartenairesInfosBase(code, version, type, codeAvn));


            // 02- AssuresAdditionnels
            partenaires.AssuresAdditionnels = GetListAssuresAdditionnelsInfosBase(code, version, type, codeAvn, modeNavig);

            // 03- Coassureurs
            partenaires.Coassureurs = AffaireNouvelleRepository.LoadListCoAssureur(type, code, version, codeAvn, modeNavig).Select(x => new PartenaireDto
            {
                Code = x.Code,
                Nom = x.Nom,
                CodeInterlocuteur = x.IdInterlocuteur,
                NomInterlocuteur = x.Interlocuteur
            }).ToList();
            // 04- Intervenants
            partenaires.Intervenants = CommonRepository.GetListeIntervenants(code, version, type, string.Empty, string.Empty).Select(x => new PartenaireDto
            {
                Code = x.CodeIntervenant.ToString(),
                Nom = x.Denomination,
                CodeInterlocuteur = x.CodeInterlocuteur,
                NomInterlocuteur = x.Interlocuteur
            }).ToList();

            // 05- Courtiers Additionnels
            partenaires.CourtiersAdditionnels = AffaireNouvelleRepository.GetListCourtiers(code, version, type, codeAvn, modeNavig)
                                              .Where(x => !new List<string> { partenaires.CourtierGestionnaire?.Code,
                                                                             partenaires.CourtierApporteur?.Code,
                                                                             partenaires.CourtierPayeur?.Code }.Any(y => y == x.CodeCourtier.ToString()))
                                                                             .Select(x => new PartenaireDto
                                                                             {
                                                                                 Code = x.CodeCourtier.ToString(),
                                                                                 Nom = x.NomCourtier
                                                                             }).ToList();
            return partenaires;

        }

        /// <summary>
        /// Obtenir la liste des assurés additionnels
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="version">Version</param>
        /// <param name="type">Type</param>
        /// <param name="codeAvn">Code AVN</param>
        /// <param name="modeNavig">mode Navig</param>
        /// <returns></returns>
        public List<PartenaireDto> GetListAssuresAdditionnelsInfosBase(string code, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return CommonRepository.GetListAssuresAdditionnelsInfosBase(code, version, type, codeAvn, modeNavig);
        }
        #endregion

        #region Methods Contracts Kheops

        public ContractJsonDto CreationContractsKheops(ContractJsonDto contract, string user)
        {
            var pgmFolder = new PGMFolder { ActeGestion = "AFFNV", User = user, Type = contract.Type };
            // Création contrat
            contract = this.folderService.CreationContractsKheops(pgmFolder, contract);
            pgmFolder.CodeOffre = contract.CodeAffaire.ToIPB();
            pgmFolder.Version = int.Parse(contract.Aliment);

            var serviceContext = new KheoBridge();
            var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
            if (!string.IsNullOrEmpty(kheoBridgeUrl))
                serviceContext.Url = kheoBridgeUrl;
            var splitCharHtml = "#**#";

            // Calcul Affaire nouvelle
            var data = this.policeRepository.GetDateEffetEtFrais(pgmFolder);
            this.as400Repository.LancementCalculAffaireNouvelle(pgmFolder, data);
            // Calcul prochaine échéance
            var retProchEch = this.as400Repository.LoadPreavisResiliation(pgmFolder,
                AlbConvert.ConvertStrToDate(contract.DateEffet.Debut),
                AlbConvert.ConvertStrToDate(contract.DateEffet.Fin),
                null, contract.Periodicite.Code,
                AlbConvert.ConvertStrToDate(contract.EcheancePrincipale + "/2012"),
                splitCharHtml);
            if (retProchEch.ContainsChars())
            {
                string[] tDataProchEch = retProchEch.Split(new[] { splitCharHtml }, StringSplitOptions.None);
                contract.DebutPeriode = tDataProchEch[0];
                contract.FinPeriode = tDataProchEch[1];
                contract.ProchaineEcheance = string.IsNullOrEmpty(contract.ProchaineEcheance) ? tDataProchEch[2] : contract.ProchaineEcheance;
            }

            if (contract.ProchaineEcheance.ContainsChars())
            {
                contract.FinPeriode = AlbConvert.ConvertDateToStr(AlbConvert.ConvertStrToDate(contract.ProchaineEcheance).Value.AddDays(-1));
            }
            // Sauvegarde des fichiers intercalaires
            this.folderService.SaveIntercalaireFiles(pgmFolder, contract);
            // Calcul commission
            if (!contract.Commissions.TauxCATNAT.ContainsChars()
                && contract.Commissions.TauxHCATNAT.ContainsChars())
            {
                var commission = this.as400Repository.LoadCommissions(pgmFolder);
                if (commission.Erreur as string != "ERREUR")
                {
                    contract.Commissions.TauxHCATNAT = commission.TauxStandardHCAT.ToString();
                    contract.Commissions.TauxCATNAT = commission.TauxStandardCAT.ToString();
                }
            }
            // Enregistrement des informations générales
            this.folderService.SetInfosGenContract(pgmFolder, contract);
            serviceContext.GenererClauses("P", contract.CodeAffaire.ToIPB(), int.Parse(contract.Aliment), new KpClausePar { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale) });
            this.repository.SetTraceContract(new WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale),
                NumeroOrdreDansEtape = 10,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });

            // Enregistrement des commissions
            this.folderService.SetInfoCommission(pgmFolder, contract);
            this.repository.SetTraceContract(new WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoCourtier),
                NumeroOrdreDansEtape = 12,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoCourtier),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });

            var codeRsq = 0;
            var codeFor = 0;
            foreach (var rsq in contract.Risques)
            {
                codeRsq++;
                codeFor++;

                contract = this.folderService.SetInfoRsqContract(pgmFolder, contract, rsq);
                serviceContext.GenererClauses("P", contract.CodeAffaire.ToIPB(), int.Parse(contract.Aliment), new KpClausePar { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque), NuRisque = codeRsq });

                IEnumerable<Garanty> allGarantiesLCI = null;
                foreach (var form in rsq.Formules)
                {
                    allGarantiesLCI = form.AllGaranties.Where(x => x != null && x.LCI.Unite == "CPX" && !x.LCI.Valeur.IsEmptyOrNull()).Distinct();
                    var numObj = rsq.Objets.Select((obj, index) => index + 1);
                    var affId = new AffaireId { CodeAffaire = contract.CodeAffaire, NumeroAliment = int.Parse(contract.Aliment), TypeAffaire = contract.Type.ParseCode<AffaireType>() };
                    var formule = formulePort.InitFormuleAffaire(affId, codeRsq, numObj, null);
                    formulePort.SetFormuleJson(affId, form, codeFor);
                    formulePort.ValidateFormuleAffaire(new FormuleId(affId) { NumeroFormule = codeFor });

                    // Génération des clauses de l'étape Formule
                    serviceContext.GenererClauses("P", contract.CodeAffaire.ToIPB(), int.Parse(contract.Aliment), new KpClausePar { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option), NuFormule = codeFor, NuOption = 1, NuRisque = codeRsq });
                    // Génération des clauses de l'étape Condition
                    serviceContext.GenererClauses("P", contract.CodeAffaire.ToIPB(), int.Parse(contract.Aliment), new KpClausePar { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie), NuFormule = codeFor, NuOption = 1, NuRisque = codeRsq });
                }

                this.folderService.UpdateLCIComplexe(contract, allGarantiesLCI);
            }

            // Alimentation des matrices
            this.as400Repository.SetMatriceContract(pgmFolder);

            //(string sbr, string categorie) = this.policeRepository.GetSousBrancheCategorieContract(contract);
            //this.as400Repository.CalculateTauxPrimes(pgmFolder, contract.Branche.Code, sbr, categorie);
            // Chargement des engagements
            this.as400Repository.LoadEngagement(pgmFolder);
            this.repository.SetTraceContract(new WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement),
                NumeroOrdreDansEtape = 61,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });
            // Initialisation des montants références
            this.as400Repository.InitMontantRef(pgmFolder, "CALCUL");
            this.repository.SetTraceContract(new WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = ContextStepName.EditionMontantsReference.AsCode(),
                NumeroOrdreDansEtape = 63,
                NumeroOrdreEtape = 1,
                Perimetre = ContextStepName.EditionMontantsReference.AsCode(),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });
            // Préparation des cotisations
            this.as400Repository.PrepareCotisation(pgmFolder);
            this.repository.SetTraceContract(new WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                NumeroOrdreDansEtape = 64,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });
            // Calcul affaire nouvelle
            var dataFrais = this.policeRepository.GetFraisAccessoires(pgmFolder);
            this.as400Repository.CalculCotisation(pgmFolder, dataFrais);
            // Alimentation statistiques
            this.as400Repository.AlimStatContract(pgmFolder, data);

            if (contract.Cotisations.Force == "O")
            {
                var mntCalc = this.repository.GetMontantCalcule(pgmFolder, contract);
                var mntForc = decimal.Parse(contract.Cotisations.Valeur);
                var coef = mntCalc > 0 ? (mntForc / mntCalc) : 0;

                // Suppression des FGA si le montant forcé est à zéro
                if (mntForc == 0)
                {
                    this.as400Repository.CalculCotisation(pgmFolder, new DataAccess.Data.DateEffetEtFraisData { Frais = 0, Atm = 0 });
                }
                this.as400Repository.ForceMontant(pgmFolder, mntCalc, mntForc, coef);

                var attentat = mntForc == 0 ? false : this.repository.IsFolderAttentat(pgmFolder, contract);
                this.as400Repository.CalculMontantForce(pgmFolder, attentat);
            }

            // Alimentation statistiques
            this.as400Repository.AlimStatContract(pgmFolder, data);
            // Chargement commissions
            this.as400Repository.LoadCommissions(pgmFolder);

            // Génération des clauses de l'étape Fin
            serviceContext.GenererClauses("P", contract.CodeAffaire.ToIPB(), int.Parse(contract.Aliment), new KpClausePar { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin) });
            this.repository.SetTraceContract(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin),
                NumeroOrdreDansEtape = 70,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });

            // Génération des documents
            this.repository.GenerateDocumentsContract(pgmFolder, contract);

            // Chargement des engagements
            this.as400Repository.LoadEngagement(pgmFolder);
            // Génération quittance
            var numPrime = this.as400Repository.GeneratePrimeContract(pgmFolder);
            // Alimentation statistiques
            this.as400Repository.AlimStatContract(pgmFolder, data);
            // Validation contrat
            this.repository.ValidationContract(pgmFolder);
            this.as400Repository.Validation400Contract(pgmFolder);

            // Edition des documents
            var lotId = this.repository.GetLotDocumentContract(pgmFolder);
            serviceContext.EditerDocParLotPush(Convert.ToInt64(lotId), "", "O", numPrime, "O", user, 0, "");

            return contract;
        }

        public ContractJsonDto CreationOffersKheops(ContractJsonDto offer, string user)
        {
            var pgmFolder = new PGMFolder { ActeGestion = "OFFRE", User = user, Type = offer.Type };
            // Création de l'offre
            offer = this.folderService.CreationOffersKheops(pgmFolder, offer);
            pgmFolder.CodeOffre = offer.CodeAffaire.ToIPB();
            pgmFolder.Version = int.Parse(offer.Aliment);
            pgmFolder.Type = offer.Type;

            var serviceContext = new KheoBridge();
            var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
            if (!string.IsNullOrEmpty(kheoBridgeUrl))
                serviceContext.Url = kheoBridgeUrl;
            //var splitCharHtml = "#**#";

            // Calcul Affaire nouvelle
            var data = this.policeRepository.GetDateEffetEtFrais(pgmFolder);
            this.as400Repository.LancementCalculAffaireNouvelle(pgmFolder, data);

            // Sauvegarde des fichiers intercalaires
            this.folderService.SaveIntercalaireFiles(pgmFolder, offer);
            // Calcul commission
            if (!offer.Commissions.TauxCATNAT.ContainsChars()
                && offer.Commissions.TauxHCATNAT.ContainsChars())
            {
                var commission = this.as400Repository.LoadCommissions(pgmFolder);
                if (commission.Erreur as string != "ERREUR")
                {
                    offer.Commissions.TauxHCATNAT = commission.TauxStandardHCAT.ToString();
                    offer.Commissions.TauxCATNAT = commission.TauxStandardCAT.ToString();
                }
            }
            // Enregistrement des informations générales
            this.folderService.SetInfosGenContract(pgmFolder, offer);
            serviceContext.GenererClauses("O", offer.CodeAffaire.ToIPB(), int.Parse(offer.Aliment), new KpClausePar { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale) });
            this.repository.SetTraceContract(new WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale),
                NumeroOrdreDansEtape = 10,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });

            // Enregistrement des commissions
            this.folderService.SetInfoCommission(pgmFolder, offer);
            this.repository.SetTraceContract(new WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoCourtier),
                NumeroOrdreDansEtape = 12,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoCourtier),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });

            var codeRsq = 0;
            var codeFor = 0;
            foreach (var rsq in offer.Risques)
            {
                codeRsq++;
                codeFor++;

                offer = this.folderService.SetInfoRsqContract(pgmFolder, offer, rsq);
                serviceContext.GenererClauses("O", offer.CodeAffaire.ToIPB(), int.Parse(offer.Aliment), new KpClausePar { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque), NuRisque = codeRsq });

                IEnumerable<Garanty> allGarantiesLCI = null;
                foreach (var form in rsq.Formules)
                {
                    allGarantiesLCI = form.AllGaranties.Where(x => x != null && x.LCI.Unite == "CPX" && !x.LCI.Valeur.IsEmptyOrNull()).Distinct();

                    var affId = new AffaireId { CodeAffaire = offer.CodeAffaire, NumeroAliment = int.Parse(offer.Aliment), TypeAffaire = offer.Type.ParseCode<AffaireType>() };
                    var numObj = rsq.Objets.Select((obj, index) => index + 1);
                    var formule = formulePort.InitFormuleAffaire(affId, codeRsq, numObj, null);
                    formulePort.SetFormuleJson(affId, form, codeFor);
                    formulePort.ValidateFormuleAffaire(new FormuleId(affId) { NumeroFormule = codeFor });

                    // Génération des clauses de l'étape Formule
                    serviceContext.GenererClauses("O", offer.CodeAffaire.ToIPB(), int.Parse(offer.Aliment), new KpClausePar { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option), NuFormule = codeFor, NuOption = 1, NuRisque = codeRsq });
                    // Génération des clauses de l'étape Condition
                    serviceContext.GenererClauses("O", offer.CodeAffaire.ToIPB(), int.Parse(offer.Aliment), new KpClausePar { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie), NuFormule = codeFor, NuOption = 1, NuRisque = codeRsq });
                }

                this.folderService.UpdateLCIComplexe(offer, allGarantiesLCI);
            }

            // Alimentation des matrices
            this.as400Repository.SetMatriceContract(pgmFolder);
            
            // Chargement des engagements
            this.as400Repository.LoadEngagement(pgmFolder);
            this.repository.SetTraceContract(new WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement),
                NumeroOrdreDansEtape = 61,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });
            // Initialisation des montants références
            this.as400Repository.InitMontantRef(pgmFolder, "CALCUL");
            this.repository.SetTraceContract(new WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = ContextStepName.EditionMontantsReference.AsCode(),
                NumeroOrdreDansEtape = 63,
                NumeroOrdreEtape = 1,
                Perimetre = ContextStepName.EditionMontantsReference.AsCode(),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });
            // Préparation des cotisations
            this.as400Repository.PrepareCotisation(pgmFolder);
            this.repository.SetTraceContract(new WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                NumeroOrdreDansEtape = 64,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });
            // Calcul affaire nouvelle
            var dataFrais = this.policeRepository.GetFraisAccessoires(pgmFolder);
            this.as400Repository.CalculCotisation(pgmFolder, dataFrais);
            // Alimentation statistiques
            this.as400Repository.AlimStatContract(pgmFolder, data);

            if (offer.Cotisations.Force == "O")
            {
                var mntCalc = this.repository.GetMontantCalcule(pgmFolder, offer);
                var mntForc = decimal.Parse(offer.Cotisations.Valeur);
                var coef = mntCalc > 0 ? (mntForc / mntCalc) : 0;
                this.as400Repository.ForceMontant(pgmFolder, mntCalc, mntForc, coef);

                var attentat = mntForc == 0 ? false : this.repository.IsFolderAttentat(pgmFolder, offer);
                this.as400Repository.CalculMontantForce(pgmFolder, attentat);
                // Suppression des FGA si le montant forcé est à zéro
                if (mntForc == 0)
                {
                    this.as400Repository.CalculCotisation(pgmFolder, new DataAccess.Data.DateEffetEtFraisData { Frais = 0, Atm = 0 });
                }
            }

            // Alimentation statistiques
            this.as400Repository.AlimStatContract(pgmFolder, data);
            // Chargement commissions
            this.as400Repository.LoadCommissions(pgmFolder);

            // Génération des clauses de l'étape Fin
            serviceContext.GenererClauses("O", offer.CodeAffaire.ToIPB(), int.Parse(offer.Aliment), new KpClausePar { ActeGestion = "**", Letape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin) });
            this.repository.SetTraceContract(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = pgmFolder.CodeOffre.ToIPB(),
                Version = pgmFolder.Version,
                Type = pgmFolder.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin),
                NumeroOrdreDansEtape = 70,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = user,
                PassageTag = "O",
                PassageTagClause = string.Empty
            });

            // Génération des documents
            this.repository.GenerateDocumentsContract(pgmFolder, offer);

            // Chargement des engagements
            this.as400Repository.LoadEngagement(pgmFolder);
            // Génération quittance
            var numPrime = this.as400Repository.GeneratePrimeContract(pgmFolder);
            // Alimentation statistiques
            this.as400Repository.AlimStatContract(pgmFolder, data);
            // Validation contrat et changement situation de l'offre
            PoliceRepository.ChangerStatutOffre(offer.CodeAffaire, "A");
            this.repository.ValidationContract(pgmFolder);
            this.as400Repository.Validation400Contract(pgmFolder);

            // Edition des documents
            var lotId = this.repository.GetLotDocumentContract(pgmFolder);
            serviceContext.EditerDocParLotPush(Convert.ToInt64(lotId), "", "O", numPrime, "O", user, 0, "");

            return offer;
        }

        #endregion
    }
}
