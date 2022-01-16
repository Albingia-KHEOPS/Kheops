using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Document;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Models.FileModel;
using OP.DataAccess;
using OP.Services.BLServices;
using OP.Services.WSKheoBridge;
using OP.WSAS400.DTO.AttentatGareat;
using OP.WSAS400.DTO.ControleFin;
using OP.WSAS400.DTO.Cotisations;
using OP.WSAS400.DTO.DocumentGestion;
using OP.WSAS400.DTO.DocumentsJoints;
using OP.WSAS400.DTO.Engagement;
using OP.WSAS400.DTO.FinOffre;
using OP.WSAS400.DTO.MontantReference;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Quittance;
using OP.WSAS400.DTO.SMP;
using OP.WSAS400.DTO.SuiviDocuments;
using OP.WSAS400.DTO.SyntheseDocuments;
using OP.WSAS400.DTO.Traite;
using OP.WSAS400.DTO.Validation;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Activation;

namespace OP.Services.TraitementsFinOffre
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FinOffre : IFinOffre, IDisposable {
        private readonly IAffairePort affaireService;
        private readonly IDocumentPort documentService;

        public FinOffre(IAffairePort affaireService, IFormulePort formuleService, IDocumentPort documentService) {
            this.affaireService = affaireService;
            this.documentService = documentService;
        }

        #region Engagement

        #region Méthodes Publiques

        public EngagementDto InitEngagement(string codeOffre, string version, string type, string codeAvn, string codePeriode, ModeConsultation modeNavig, bool isReadonly, bool enregistrementEncoursOnly, string user, string acteGestion, string accessMode, string screen)
        {
            return BLCommon.InitEngagement(codeOffre, version, type, codeAvn, codePeriode, modeNavig, isReadonly, enregistrementEncoursOnly, user, acteGestion, accessMode, screen);
            //return BLEngagements.InitEngagement(codeOffre, version, type, codeAvn, codePeriode, modeNavig, isReadonly, enregistrementEncoursOnly, user, acteGestion);
        }

        public void UpdateEngagement(string codeOffre, string version, string type, EngagementDto engagement, string field, string user, string acteGestion, string codePeriode)
        {
            BLCommon.UpdateEngagement(codeOffre, version, type, engagement, field, user, acteGestion, codePeriode);
            //BLEngagements.UpdateEngagement(codeOffre, version, type, engagement, field, user, acteGestion);
        }

        public void SaveEngagementCommentaire(string codeOffre, string version, string type, string commentaire, string codePeriode)
        {
            EngagementRepository.SaveEngagementCommentaire(codeOffre, version, type, commentaire, codePeriode);

            //BLEngagements.SaveEngagementCommentaire(codeOffre, version, type, commentaire);
        }


        public void SavePeriodeCnx(PeriodeConnexiteDto dto, string mode)
        {
            switch (mode)
            {
                case "INSERT":
                    EngagementRepository.InsertPeriodeCnx(dto.CodeEngagement, dto.DateDebut, dto.DateFin, dto.Traites, dto.CodeOffre, dto.Version, dto.Type, dto.User);
                    break;
                case "UPDATE":
                    EngagementRepository.UpdatePeriodeCnx(dto.CodeEngagement, dto.DateDebut, dto.DateFin, dto.Traites, dto.User);
                    break;
                case "DELETE":
                    EngagementRepository.DeletePeriodeCnx(dto.CodeEngagement);
                    break;
            }
        }

        #endregion

        #region Méthodes Privées
        #endregion

        #endregion

        #region Traite

        public TraiteDto InitTraite(string codeOffre, string version, string type, string codeAvn, string traite, ModeConsultation modeNavig,
            bool isReadonly, string user, string acteGestion, string codePeriode, string accesMode)
        {
            return BLCommon.InitTraite(codeOffre, version, type, codeAvn, traite, modeNavig, isReadonly, user, acteGestion, codePeriode, accesMode);
        }

        public List<SMPTaiteDto> GetSmpT(int id) {

            return EngagementRepository.GetSmpT(id);
        }
        public  void SaveSmpT(int SmpCptF, int id)
        {
           EngagementRepository.SaveSmpT(SmpCptF,id);
        }
        
        public void UpdateTraite(string codeOffre, string version, string type, TraiteDto traiteDto, string user, string codePeriode)
        {
            EngagementRepository.UpdateTraite(codeOffre, version, type, traiteDto, user, codePeriode);
        }
        

        #endregion

        #region Attentat / GAREAT

        #region Méthodes Publiques

        public AttentatGareatDto InitAttentat(string codeOffre, string version, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion)
        {
            if (!isReadonly && modeNavig == ModeConsultation.Standard)
            {
                string result = CommonRepository.UpdateAS400Attentat(codeOffre, version, type, "INI", modeNavig, string.Empty, string.Empty, user, acteGestion);
                if (result == "ERREUR")
                {
                    //erreur
                }
            }

            return AttentatRepository.InitAttentat(codeOffre, version, type, codeAvn, modeNavig);

            //return BLAttentatGareat.InitAttentat(codeOffre, version, type, codeAvn, isReadonly, modeNavig, user, acteGestion);
        }

        public void UpdateAttentat(string codeOffre, string version, string type, string field, string value, string commentForce, string user, ModeConsultation modeNavig, string acteGestion)
        {
            AttentatRepository.UpdateAttentat(codeOffre, version, type, field, value, commentForce);
            //AttentatRepository.UpdateKpent(codeOffre, version, type, field, value);

            string result = CommonRepository.UpdateAS400Attentat(codeOffre, version, type, field, modeNavig, field == "REC" ? value : string.Empty, field == "COM" ? value : string.Empty, user, acteGestion);
            if (result == "ERREUR")
            {
                //erreur
            }

            #region Arbre de navigation

            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = codeOffre.PadLeft(9, ' '),
                Version = Convert.ToInt32(version),
                Type = type,
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

            #endregion

            //BLAttentatGareat.UpdateAttentat(codeOffre, version, type, field, value, commentForce, user, modeNavig, acteGestion);
        }

        public void UpdateAttentatComment(string codeOffre, string version, string type, string commentForce)
        {
            AttentatRepository.UpdateAttentatComment(codeOffre, version, type, commentForce);

            //BLAttentatGareat.UpdateAttentatComment(codeOffre, version, type, commentForce);
        }

        #endregion

        #endregion

        #region Cotisations

        #region Méthodes Publiques

        public CotisationsDto InitCotisations(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, bool isChecked, bool reload, string user, string acteGestion, bool isreadonly)
        {
            var cotisations = BLCommon.InitCotisations(codeOffre, version, type, codeAvn, modeNavig, isChecked, reload, user, acteGestion, isreadonly);
            var listGareat = this.affaireService.GetGarantiesGareat(new AffaireId {
                CodeAffaire = codeOffre,
                IsHisto = modeNavig == ModeConsultation.Historique,
                NumeroAliment = version.ParseInt().Value,
                NumeroAvenant = int.TryParse(codeAvn, out int a) && a > 0 ? new int?(a) : null,
                TypeAffaire = type.ParseCode<AffaireType>()
            }).Where(x => x.CodeGarantie == Albingia.Kheops.OP.Domain.Formule.Garantie.CodeGareatAttent);
            cotisations.GarantiesInfo.Gareat = new CotisationCanatGareatDto {
                CotisationHT = ((listGareat?.Any() ?? false) ? listGareat.Sum(x => x.TarifsGarantie.PrimeValeur.ValeurActualise) : 0).ToString("C2"),
                CotisationTaxe = string.Empty,
                CotisationTTC = string.Empty
            };
            return cotisations;
        }

        public void UpdateCotisations(string codeOffre, string version, string type, CotisationsDto cotisationsDto, string field, string value, string oldvalue, string user, ModeConsultation modeNavig, string codeAvn, string acteGestion)
        {
            BLCommon.UpdateCotisations(codeOffre, version, type, cotisationsDto, field, value, oldvalue, user, modeNavig, codeAvn, acteGestion);
        }

        public void SaveCommentaireCotisation(string codeOffre, string version, string type, string commentaire)
        {
            CotisationsRepository.SaveCommentaireCotisation(codeOffre, version, type, commentaire);

            //BLCotisations.SaveCommentaireCotisation(codeOffre, version, type, commentaire);
        }

        public CotisationsInfoTarifDto LoadCotisationsTarif(string lienKpgaran)
        {
            return CotisationsRepository.LoadCotisationsTarif(lienKpgaran);

            //return BLCotisations.LoadCotisationsTarif(lienKpgaran);
        }

        //public List<CotisationGarantieDto> GetCotisationGaranties(string codeOffre, string version, string type, bool onlyGarPorteuse)
        //{
        //    return BLCotisations.GetCotisationGaranties(codeOffre, version, type, onlyGarPorteuse);
        //}

        public CotisationsDto GetCotisationsGaranties(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, bool onlyGarPorteuse, string typePart)
        {
            return CotisationsRepository.InitCotisations(codeOffre, version, type, codeAvn, modeNavig, onlyGarPorteuse, typePart);

            //return BLCotisations.GetCotisationsGaranties(codeOffre, version, type, codeAvn, modeNavig, onlyGarPorteuse, typePart);
        }

        #endregion

        #endregion

        #region SMP

        #region Methodes public

        public SMPdto ObtenirDetailSMP(string argCodeOffre, string argVersion, string argType, string codeAvn, string risque, string ventilation, ModeConsultation modeNavig)
        {
            SMPdto result = EngagementRepository.ObtenirDetailCalculSMP(argCodeOffre, argVersion, argType, codeAvn, risque, ventilation, modeNavig);

            //SMPdto result = BLEngagements.ObtenirSMP(argCodeOffre, argVersion, argType, codeAvn, risque, ventilation, modeNavig);
            BrancheDto branche = CommonRepository.GetBrancheCible(argCodeOffre, argVersion, argType, codeAvn, modeNavig);
            result.Types = GetTypes(branche.Code, branche.Cible.Code);

            return result;
        }

        public SMPdto RecalculSMP(SMPdto argQuery, string argCodeOffre, string argVersion, string argType, string codeAvn, string risque, string ventilation, ModeConsultation modeNavig)
        {
            SMPdto result = EngagementRepository.RecalculDetailSMP(argQuery, argCodeOffre, argVersion, argType, codeAvn, risque, ventilation, modeNavig);

            //SMPdto result = BLEngagements.RecalculSMP(argQuery, argCodeOffre, argVersion, argType, codeAvn, risque, ventilation, modeNavig);
            BrancheDto branche = CommonRepository.GetBrancheCible(argCodeOffre, argVersion, argType, codeAvn, modeNavig);
            result.Types = GetTypes(branche.Code, branche.Cible.Code);

            return result;
        }

        #endregion

        #region Methode private

        private List<ParametreDto> GetTypes(string branche, string cible)
        {
            return CommonRepository.GetParametres(branche, cible, "KHEOP", "SMPF");
        }

        #endregion

        #endregion

        #region Fin Offre

        public FinOffreDto InitFinOffre(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return FinOffreRepository.InitFinOffre(codeOffre, version, type, codeAvn, modeNavig);
            //return BLFinOffre.InitFinOffre(codeOffre, version, type, codeAvn, modeNavig);
        }

        public void UpdateFinOffre(string codeOffre, string version, string type, FinOffreDto finOffreDto, string user)
        {
            FinOffreRepository.FinOffreUpdate(codeOffre, version, type, finOffreDto);

            #region Arbre de navigation

            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = codeOffre.PadLeft(9, ' '),
                Version = Convert.ToInt32(version),
                Type = type,
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

            #endregion
            //BLFinOffre.UpdateFinOffre(codeOffre, version, type, finOffreDto, user);
        }

        #endregion

        #region Controle Fin

        public ControleFinDto InitControleFin(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return ControleFinRepository.InitControleFin(codeOffre, version, type, codeAvn, modeNavig);

            //return BLControleFin.InitControleFin(codeOffre, version, type, codeAvn, modeNavig);
        }

        public void Alimentation(string codeOffre, string version, string type, string user, ModeConsultation modeNavig, bool isModifHorsAvn, bool isAvenant, string regulId, int codeAssure)
        {
            PoliceRepository.UpdateIndexation(codeOffre, version, type);
            ControleFinRepository.Alimentation(codeOffre, version, type, user, modeNavig, isModifHorsAvn, isAvenant, regulId);
            if (!AssureRepository.GetAssureIsActif(codeAssure))
            {
                ControleFinRepository.AlimentationAssure(codeOffre, version, type, user, isAvenant, codeAssure);
            }
        }

        public void UpdateEtatRegul(string codeOffre, string version, string type, string regulId)
        {
            ControleFinRepository.UpdateEtatRegul(regulId);
            ControleFinRepository.CleanControl(codeOffre, type, version);
        }

        #endregion

        #region Validation Offre

        #region Méthodes Publiques

        public ValidationDto InitValidationOffre(string codeOffre, string version, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion, string reguleId, string reguleMode)
        {
            return BLCommon.InitValidationOffre(codeOffre, version, type, codeAvn, isReadonly, modeNavig, user, acteGestion, reguleId, reguleMode);
            //return BLFinOffre.InitValidationOffre(codeOffre, version, type, codeAvn, isReadonly, modeNavig, user, acteGestion);
        }

        public void SaveEtatMotif(string codeOffre, string version, string type, string etat, string motif, string acteGestion, string regulId)
        {
            FinOffreRepository.SaveEtatMotif(codeOffre, version, type, etat, motif, acteGestion, regulId);
        }

        public ValidationEditionDto GetValidationEdition(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion, string modeEcran, bool isBNS)
        {
            return FinOffreRepository.GetValidationEdition(codeOffre, version, type, codeAvn, modeNavig, acteGestion, modeEcran, isBNS);
            //return BLFinOffre.GetValidationEdition(codeOffre, version, type, codeAvn, modeNavig, modeEcran);
        }

        public string CheckIsDocEdit(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion)
        {
            var result = FinOffreRepository.InitValidationOffre(codeOffre, version, type, codeAvn, false, modeNavig, acteGestion, string.Empty,string.Empty);
            if (result != null)
                return result.IsDocEdit;
            return string.Empty;
            //return BLFinOffre.CheckIsDocEdit(codeOffre, version, type, codeAvn, modeNavig);
        }

        public bool CheckValidateOffre(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion)
        {
            return FinOffreRepository.CheckValidateOffre(codeOffre, version, type, codeAvn, modeNavig, acteGestion);
            //return BLFinOffre.CheckValidateOffre(codeOffre, version, type, codeAvn, modeNavig);
        }

        public string VerifCourtier(string codeOffre, string version, string type)
        {
            return FinOffreRepository.VerifCourtier(codeOffre, version, type);
        }

        #endregion

        #region Méthodes Privées


        #endregion

        #endregion

        #region Gestion Document

        public List<DocumentGestionDocDto> InitDocumentsGestion(string codeOffre, string version, string type, string codeAvt, string acteGestion, string typeAvenant, string user, ModeConsultation modeNavig, bool isReadOnly, bool init, bool isValidation, long[] docsId, bool firstLoad, string attesId, string regulId)
        {
            typeAvenant = acteGestion;
            var affaireId = new AffaireId(type.AsEnum<AffaireType>(), codeOffre, int.Parse(version), int.Parse(codeAvt), modeNavig == ModeConsultation.Historique);

            Func<IEnumerable<DocumentGestionDocDto>> generator = () =>
            {
                if (!(isValidation || isReadOnly))
                {
                    documentService.GenerateDocuments(affaireId, "PRODU", acteGestion, user, DateTime.Now, init && (!isReadOnly || acteGestion == AlbConstantesMetiers.TYPE_ATTESTATION) && modeNavig == ModeConsultation.Standard, docsId, int.TryParse(attesId, out int atId) ? atId : 0, int.TryParse(regulId, out int regId) ? regId : 0);
                }
                if (affaireId.IsHisto)
                {
                    return Enumerable.Empty<DocumentGestionDocDto>();
                }
                var lot = documentService.GetLots(affaireId, true).FirstOrDefault() ?? new LotDocument();
                if (lot == null)
                {
                    Enumerable.Empty<DocumentGestionDocDto>();

                }
                return lot.Documents.Where(s=>s.SituationLot != SituationDocumentLot.Traite).Select(d =>
                new DocumentGestionDocDto
                {
                    DocId = d.IdDoc,
                    FirstGeneration = false,
                    ListDocInfos = new List<DocumentGestionDocInfoDto> {
                        new DocumentGestionDocInfoDto
                    {
                        IdDoc = d.IdDoc,
                        Situation = d.SituationLot.AsCode(),
                        Nature = d.NatureGeneration.AsCode(),
                        Imprimable = d.IsImprimable.ToYesNo(),
                        Chemin = d.Chemin,
                        Statut = d.StatutGeneration.AsCode(),
                        TypeDoc = d.TypoDocument.Code,
                        NomDoc = d.Nom,
                        LibDoc = d.Libelle,
                        IdLotDetail = d.IdDocLot,
                        TypeDestinataire = d.Destinataire.Type.AsCode(),
                        Destinataire = d.Destinataire.Courtier?.Code ?? d.Destinataire.Assure?.Code ?? 0,
                        CodeTypeEnvoi = d.TypeEnvoi.Code,
                        TypeEnvoi = d.TypeEnvoi.LibelleLong,
                        NbExemple = 1,
                        NbExempleSupp = 0,
                        Tampon = d.Tampon.Code,
                        LibTampon = d.Tampon.LibelleLong,
                        IdLettre = 0,
                        TypeLettre = "",
                        LettreAccomp = "",
                        LibLettre = "",
                        Email = "",
                        IsLibre = d.IsLibre,
                        LotId = lot.IdLot
                    }
                    }
                }
                );

            };

            return FinOffreRepository.InitDocumentsGestion(codeOffre, version, type, codeAvt, acteGestion, typeAvenant, user, modeNavig, isReadOnly, init, isValidation, docsId, firstLoad, attesId, regulId, generator);
        }

        public List<DocumentGestionDocDto> GetListeDocumentsEditables(string codeOffre, string version, string type, ModeConsultation modeNavig)
        {
            return FinOffreRepository.GetListeDocumentsEditables(codeOffre, version, type, modeNavig);
        }

        public DocumentGestionInfoDestDto ShowInfoDest(string idDest, string typeDest)
        {
            return FinOffreRepository.ShowInfoDest(idDest, typeDest);
        }

        public void ValidSupprDoc(string selectDoc, string unselectDoc)
        {
            FinOffreRepository.ValidSupprDoc(selectDoc, unselectDoc);
        }

        public List<SyntheseDocumentsDocDto> InitSyntheseDocument(string codeOffre, string version, string type)
        {
            return FinOffreRepository.InitSyntheseDocument(codeOffre, version, type);
        }

        public void SaveTraceArbreFinAffnouv(string codeOffre, string version, string type, string user)
        {
            FinOffreRepository.SaveTraceArbreFinAffnouv(codeOffre, version, type, user);
        }

        public void ChangeSituationDoc(string idDoc, string situation)
        {
            FinOffreRepository.ChangeSituationDoc(idDoc, situation);
        }

        public void RegenerateDocLibre(string codeOffre, string version, string type, string idsDoc, string user)
        {
            FinOffreRepository.RegenerateDocLibre(codeOffre, version, type, idsDoc, user);
        }

        public void OuvrirGestionDocument(string codeAffaire, int version, string type, string user, string wrkStation, string ipAdress)
        {

#if DEBUG
            wrkStation = Environment.MachineName;
#endif
            using (var serviceContext = new WSKheoBridge.KheoBridge())
            {
                var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                if (!string.IsNullOrEmpty(kheoBridgeUrl))
                    serviceContext.Url = kheoBridgeUrl;
                CommonRepository.SetTraceLog(string.Empty, string.Empty, string.Empty, 0, string.Empty, "PUSHDOC", DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture), wrkStation + '_' + ipAdress);
                var pushDto = new WSKheoBridge.KheoPushDto
                {
                    Fonction = PushFonction.OUVRIR_GESTION_DOCUMENT,
                    Adresse_IP = ipAdress,
                    UserAD = user,
                    TypContrat = type,
                    NumeroContrat = codeAffaire,
                    Aliment = version
                };
                serviceContext.ExecuterPush(pushDto);
            }
        }

        public string OpenGED(string codeAffaire, int version, string type, string userName, string ip, string machineName)
        {
            using (var serviceContext = new KheoBridge())
            {
                var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                if (!string.IsNullOrEmpty(kheoBridgeUrl))
                    serviceContext.Url = kheoBridgeUrl;
                var pushDto = new KheoPushDto
                {
                    Fonction = PushFonction.OUVRIR_FENETRE_GED,
                    Adresse_IP = ip,
                    UserAD = userName,
                    TypContrat = type,
                    NumeroContrat = codeAffaire,
                    AnneeSin = 0,
                    NuSin = 0,
                    SinSbr = string.Empty,
                    Aliment = version
                };
                serviceContext.ExecuterPush(pushDto);
            }
            return string.Empty;
        }


        #endregion

        #region Détails Document Gestion

        public List<ParametreDto> GetListeDocumentsDispo()
        {
            return FinOffreRepository.GetListeDocumentsDispo();
            //return BLFinOffre.GetListeDocumentsDispo();
        }

        public List<ParametreDto> GetListeTypesDestinataire()
        {
            return FinOffreRepository.GetListeTypesDestinataire();
            //return BLFinOffre.GetListeTypesDestinataire();
        }

        public List<ParametreDto> GetListeTypesEnvoi()
        {
            return FinOffreRepository.GetListeTypesEnvoi();
            //return BLFinOffre.GetListeTypesEnvoi();
        }

        public List<ParametreDto> GetListeTampons()
        {
            return FinOffreRepository.GetListeTampons();
            //return BLFinOffre.GetListeTampons();
        }

        public List<CourrierTypeDto> GetListeCourriersType(string filtre, string typeDoc)
        {
            return FinOffreRepository.GetListeCourriersType(filtre, typeDoc);
            //return BLFinOffre.GetListeCourriersType(filtre, typeDoc);
        }

        public List<DestinataireDto> GetListeDestinatairesDetails(string codeDocument)
        {
            return FinOffreRepository.GetListeDestinatairesDetails(codeDocument);
            //return BLFinOffre.GetListeDestinatairesDetails(codeDocument);
        }

        public List<DestinataireDto> GetListeCourtiers(string code, string type, string version, string codeDocument)
        {
            return FinOffreRepository.GetListeCourtiers(code, type, version, codeDocument);
            //return BLFinOffre.GetListeCourtiers(code, type, version, codeDocument);
        }

        public List<DestinataireDto> GetListeAssures(string code, string type, string version, string codeDocument)
        {
            return FinOffreRepository.GetListeAssures(code, type, version, codeDocument);
            //return BLFinOffre.GetListeAssures(code, type, version, codeDocument);
        }

        public List<DestinataireDto> GetListeCompagnies(string code, string type, string version, string codeDocument)
        {
            return FinOffreRepository.GetListeCompagnies(code, type, version, codeDocument);
            //return BLFinOffre.GetListeCompagnies(code, type, version, codeDocument);
        }

        public List<DestinataireDto> GetListeIntervenants(string code, string type, string version, string codeDocument, string typeIntervenant)
        {
            return FinOffreRepository.GetListeIntervenants(code, type, version, codeDocument, typeIntervenant);
            //return BLFinOffre.GetListeIntervenants(code, type, version, codeDocument, typeIntervenant);
        }

        public DocumentGestionDetailsInfoGen GetInfoComplementairesDetailsDocumentGestion(string codeDocument)
        {
            return FinOffreRepository.GetInfoComplementairesDetailsDocumentGestion(codeDocument);
            //return BLFinOffre.GetInfoComplementairesDetailsDocumentGestion(codeDocument);
        }

        public List<DestinataireDto> SaveLigneDestinataireDetails(string code, string version, string type, string user, string lotId, string typeDoc, string courrierType, string codeDocument, DestinataireDto destinataire, string acteGestion)
        {
            return FinOffreRepository.SaveLigneDestinataireDetails(code, version, type, user, lotId, typeDoc, courrierType, codeDocument, destinataire, acteGestion);
            //return BLFinOffre.SaveLigneDestinataireDetails(code, version, type, user, lotId, typeDoc, courrierType, codeDocument, destinataire);
        }

        public List<DestinataireDto> DeleteLigneDestinataireDetails(string codeDocument, string guidIdLigne)
        {
            return FinOffreRepository.DeleteLigneDestinataireDetails(codeDocument, guidIdLigne);
            //return BLFinOffre.DeleteLigneDestinataireDetails(codeDocument, guidIdLigne);
        }

        public void SaveInformationsComplementairesDetailsDocument(string code, string type, string version, Int64 codeDocument, string document, Int64 courrierType, int nbExSupp, string user)
        {
            FinOffreRepository.SaveInformationsComplementairesDetailsDocument(code, type, version, codeDocument, document, courrierType, nbExSupp, user);
            //BLFinOffre.SaveInformationsComplementairesDetailsDocument(code, type, version, codeDocument, document, courrierType, nbExSupp, user);
        }

        public void DeleteLotDocumentsGestion(Int64 codeLot)
        {
            FinOffreRepository.DeleteLotDocumentsGestion(codeLot);
        }

        #endregion

        #region Quittance
        public List<QuittanceDto> GetQuittances(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string modeAffichage, string numQuittanceVisu, bool launchPGM, bool isModeCalculForce, string user, string acteGestion, string idRegule, bool isreadonly, string isFGACocheIHM)
        {
            var listQuittance = BLCommon.GetQuittances(codeOffre, version, type, codeAvn, modeAffichage, numQuittanceVisu, launchPGM, isModeCalculForce, modeNavig, user, acteGestion, idRegule, isreadonly, isFGACocheIHM);
            if (listQuittance.Any()) {
                var qtc = listQuittance.First();
                var gareatDto = this.affaireService.GetPrimesGareat(new AffaireId {
                    CodeAffaire = codeOffre,
                    NumeroAliment = int.Parse(version),
                    IsHisto = modeNavig == ModeConsultation.Historique,
                    NumeroAvenant = int.TryParse(codeAvn, out int a) && a > -1 ? new int?(a) :  null,
                    TypeAffaire = type.ParseCode<AffaireType>()
                }, modeAffichage == "visu");
                var montant = gareatDto.MontantTotal;
                if (montant.MontantHorsTaxe != decimal.Zero) {
                    qtc.GareatHT = montant.MontantHorsTaxe;
                    qtc.GareatTaxe = montant.MontantTaxe;
                    qtc.GareatTTC = montant.MontantTotal;
                    listQuittance.RemoveAt(0);
                    listQuittance.Insert(0, qtc);
                }
            }
            return listQuittance;
        }
        public QuittanceDetailDto GetQuittanceDetail(string codeOffre, string version, string codeAvn, ModeConsultation modeNavig, string acteGestion, string modeAffichage, string numQuittanceVisu)
        {
            return FinOffreRepository.GetQuittanceDetail(codeOffre, version, codeAvn, modeAffichage, numQuittanceVisu, modeNavig, acteGestion);
        }

        public InfoCompQuittanceDto GetInfoComplementairesQuittance(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestion, bool isReadonly, string user, bool isValidQuitt)
        {
            return FinOffreRepository.GetInfoComplementairesQuittance(codeOffre, version, type, codeAvn, modeNavig, acteGestion, isReadonly, user, isValidQuitt);
        }
        public void GestionTraceAvt(string codeOffre, string version, string type, bool isChecked, string user, string acteGestion)
        {
            FinOffreRepository.GestionTraceAvt(codeOffre, version, type, isChecked, user, acteGestion);
        }

        public string GetCodeAvnQuittance(string codeOffre, string version, string numQuittance)
        {
            return FinOffreRepository.GetCodeAvnQuittance(codeOffre, version, numQuittance);
        }

        public void CalculAvenant(string codeContrat, string version, string codeAvn, string user, string acteGestion, string isFGACocheIHM, decimal fraisAccessoire = 0, bool updateaccavn = false)
        {
            BLCommon.CalculAvenant(codeContrat, version, codeAvn, user, acteGestion, isFGACocheIHM, fraisAccessoire, updateaccavn);
        }

        #endregion

        #region Quittance - Frais accessoires
        public FraisAccessoiresDto InitFraisAccessoire(string codeOffre, string versionOffre, string typeOffre, string codeAvn, int anneeEffet, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion, string reguleId, bool isModifHorsAvn)
        {
            return FinOffreRepository.InitFraisAccessoire(codeOffre, versionOffre, typeOffre, codeAvn, anneeEffet, isReadonly, modeNavig, user, acteGestion, reguleId, isModifHorsAvn);
            //return BLFinOffre.InitFraisAccessoire(codeOffre, versionOffre, typeOffre, codeAvn, anneeEffet, isReadonly, modeNavig, user, acteGestion);
        }
        public List<ParametreDto> GetListeTypesAccessoire()
        {
            return FinOffreRepository.GetListeTypesAccessoire();
            //return BLFinOffre.GetListeTypesAccessoire();
        }

        public string UpdateFraisAccessoires(string codeOffre, string versionOffre, string typeOffre, int effetAnnee, string typeFrais, int fraisRetenus,
            bool taxeAttentat, /*int fraisSpecifiques,*/ long codeCommentaires, string commentaires, string codeAvn, string user, string acteGestion, bool isModifHorsAvn)
        {
            var toReturn = FinOffreRepository.UpdateFraisAccessoires(codeOffre, versionOffre, typeOffre, effetAnnee, typeFrais, fraisRetenus, taxeAttentat, codeCommentaires, commentaires, codeAvn, user, acteGestion, isModifHorsAvn);

            //2015-05-20 : Ajout de l'appel du PGM400 KDP021CL suite au mail de CCI
            //2015-06-26 : remplacement du paramètre "X" par la condition ternaire
            if (!isModifHorsAvn)
                CommonRepository.AlimStatistiques(codeOffre, versionOffre, user, acteGestion, codeAvn == "0" && typeOffre == AlbConstantesMetiers.TYPE_CONTRAT ? "N" : "X");

            return toReturn;
            //return BLFinOffre.UpdateFraisAccessoires(codeOffre, versionOffre, typeOffre, effetAnnee, typeFrais, fraisRetenus, taxeAttentat, codeCommentaires, commentaires, codeAvn, user, acteGestion);
        }
        public void UpdateFraisAccessoiresAvn(string codeOffre, string version, string type, string codeAvn, string acteGestion, string reguleId, string user, int fraisforce, bool taxeAttentat)
        {
            FinOffreRepository.UpdateFraisAccessoiresAvn(codeOffre, version, type, fraisforce, taxeAttentat, codeAvn, user, acteGestion, reguleId);

        }

        public void SaveCommentQuittance(string codeOffre, string version, string type, string codeAvn, string comment, string acteGestion, string reguleId, string modifPeriod, string dateDeb, string dateFin)
        {
            switch (acteGestion)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                    FinOffreRepository.SaveCommentQuittanceRegule(codeOffre, version, type, comment, reguleId);
                    break;
                default:
                    FinOffreRepository.SaveCommentQuittance(codeOffre, version, type, codeAvn, comment, modifPeriod, dateDeb, dateFin);
                    break;
            }
            //BLFinOffre.SaveCommentQuittance(codeOffre, version, type, comment);
        }
        public string GetCommentQuittance(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return FinOffreRepository.GetCommentQuittance(codeOffre, version, type, codeAvn, modeNavig);
            //return BLFinOffre.GetCommentQuittance(codeOffre, version, type, codeAvn, modeNavig);
        }
        #endregion

        #region Quittance - Ventilation détaillée

        public List<QuittanceVentilationDetailleeGarantieDto> GetQuittanceVentilationDetailleeGaranties(string codeOffre, string version, string type, ModeConsultation modeNavig, string modeAffichage, string numQuittanceVisu, string acteGestion)
        {
            return FinOffreRepository.GetQuittanceVentilationDetailleeGaranties(codeOffre, version, type, modeAffichage, numQuittanceVisu, acteGestion);
            //return BLFinOffre.GetQuittanceVentilationDetailleeGaranties(codeOffre, version, type, modeAffichage, numQuittanceVisu);
        }

        public List<QuittanceVentilationDetailleeTaxeDto> GetQuittanceVentilationDetailleeTaxes(string codeOffre, string version, string type, ModeConsultation modeNavig, string modeAffichage, string numQuittanceVisu, string acteGestion)
        {
            return FinOffreRepository.GetQuittanceVentilationDetailleeTaxes(codeOffre, version, type, modeAffichage, numQuittanceVisu, acteGestion);
            //return BLFinOffre.GetQuittanceVentilationDetailleeTaxes(codeOffre, version, type, modeAffichage, numQuittanceVisu);
        }

        #endregion

        #region Quittance - Ventilation des commissions

        public List<QuittanceVentilationCommissionCourtierDto> GetQuittanceVentilationCommissionCourtiers(string codeOffre, string version, string type, ModeConsultation modeNavig, string modeAffichage, string numQuittanceVisu, string acteGestion)
        {
            return FinOffreRepository.GetQuittanceVentilationCommissionCourtiers(codeOffre, version, type, modeAffichage, numQuittanceVisu, acteGestion);
            //return BLFinOffre.GetQuittanceVentilationCommissionCourtiers(codeOffre, version, type, modeAffichage, numQuittanceVisu);
        }

        #endregion

        #region Quittance - Tab Part Albingia

        public QuittanceVentilationAperitionDto GetQuittancePartAlbingia(string codeOffre, string version, string type, string codeAvn, string modeNavig, string acteGestion, string modeAffichage, string numQuittanceVisu)
        {
            return FinOffreRepository.GetQuittancePartAlbingia(codeOffre, version, type, codeAvn, modeNavig, acteGestion, modeAffichage, numQuittanceVisu);
            //return BLFinOffre.GetQuittancePartAlbingia(codeOffre, version, type, modeAffichage, numQuittanceVisu);
        }

        public List<QuittanceTabAperitionLigneDto> GetQuittanceVentilationCoassureurs(string codeOffre, string version, string type, string modeAffichage, string numQuittanceVisu, string acteGestion)
        {
            return FinOffreRepository.GetQuittanceVentilationCoassureurs(codeOffre, version, type, modeAffichage, numQuittanceVisu, acteGestion);
            //return BLFinOffre.GetQuittanceVentilationCoassureurs(codeOffre, version, type, modeAffichage, numQuittanceVisu);
        }

        public List<QuittanceTabAperitionLigneDto> GetQuittanceVentilationCoassureursParGarantie(string codeOffre, string version, string type, string codeCoass, string modeAffichage, string numQuittanceVisu, string acteGestion)
        {
            return FinOffreRepository.GetQuittanceVentilationCoassureursParGarantie(codeOffre, version, type, codeCoass, modeAffichage, numQuittanceVisu, acteGestion);
            //return BLFinOffre.GetQuittanceVentilationCoassureursParGarantie(codeOffre, version, type, codeCoass, modeAffichage, numQuittanceVisu);
        }


        #endregion

        #region Quittance - Visualisation

        public List<QuittanceVisualisationLigneDto> GetListeVisualisationQuittances(string codeOffre, string version, DateTime? dateEmission, string typeOperation, string situation, DateTime? datePeriodeDebut, DateTime? datePeriodeFin, AlbConstantesMetiers.TypeQuittances typeQuittances, string colTri)
        {
            return FinOffreRepository.GetListeVisualisationQuittances(codeOffre, version, dateEmission, typeOperation, situation, datePeriodeDebut, datePeriodeFin, typeQuittances, colTri);
            //return BLFinOffre.GetListeVisualisationQuittances(codeOffre, version, dateEmission, typeOperation, situation, datePeriodeDebut, datePeriodeFin, typeQuittances);
        }

        public List<ParametreDto> GetListeTypesOperation()
        {
            return CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PKOPE");
            //return BLFinOffre.GetListeTypesOperation();
        }

        public string LancerBulletinAvis(string codeOffre, string version, string type, string avenant, string numQuittanceVisu, string nbExemplaire, string typeCopie, bool isAvisEcheance, string user)
        {
            string str = "Erreur d'appel du <b><b><u>module Editique</u></b></b> : méthode appelée <b><b><u>{0}</u></b></b> avec les paramètres suivants : <br/>{1}";

            int iVersion = 0;
            int iAvenant = 0;
            int iNumQuittance = 0;
            int.TryParse(version, out iVersion);
            int.TryParse(avenant, out iAvenant);
            int.TryParse(numQuittanceVisu, out iNumQuittance);
            using (var serviceContext = new KheoBridge())
            {
                var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                if (!string.IsNullOrEmpty(kheoBridgeUrl))
                    serviceContext.Url = kheoBridgeUrl;

                if (isAvisEcheance)
                {
                    try
                    {
                        return serviceContext.EditerAvisEcheance(type, codeOffre, iVersion, iAvenant, iNumQuittance, typeCopie, user);
                    }
                    catch (Exception)
                    {
                        return string.Format(str, "EditerAvisEcheance", string.Format("type = {0}, num affaire = {1}, version = {2}, code avt = {3}, num quittance = {4}, type copie = {5}, user = {6}", type, codeOffre, iVersion, iAvenant, iNumQuittance, typeCopie, user));
                    }
                }
                else
                {
                    try
                    {
                        return serviceContext.EditerBulletinSouscription400(type, codeOffre, iVersion, iAvenant, typeCopie, user);
                    }
                    catch (Exception)
                    {
                        return string.Format(str, "EditerBulletinSouscription400", string.Format("type = {0}, num affaire = {1}, version = {2}, code avt = {3}, type copie = {4}, user = {5}", type, codeOffre, iVersion, iAvenant, typeCopie, user));
                    }
                }
            }
        }

        #endregion

        #region Quittance - Annulation quittances

        public QuittanceVisualisationDto GetListeQuittancesAnnulation(bool init, string codeOffre, string version, int avenant, DateTime dateEffetAvenant, DateTime? dateEmission, string typeOperation, string situation, DateTime? datePeriodeDebut, DateTime? datePeriodeFin, AlbConstantesMetiers.TypeQuittances typeQuittances, string user, string acteGestion, bool isreadonly, ModeConsultation modeNavig, string colTri)
        //public List<QuittanceVisualisationLigneDto> GetListeQuittancesAnnulation(bool init, string codeOffre, string version, int avenant, DateTime dateEffetAvenant, DateTime? dateEmission, string typeOperation, string situation, DateTime? datePeriodeDebut, DateTime? datePeriodeFin, AlbConstantesMetiers.TypeQuittances typeQuittances, string user, string acteGestion, bool isreadonly, ModeConsultation modeNavig)
        {
            return BLCommon.GetListeQuittancesAnnulation(init, codeOffre, version, avenant, dateEffetAvenant, dateEmission, typeOperation, situation, datePeriodeDebut, datePeriodeFin, typeQuittances, user, acteGestion, isreadonly, modeNavig, colTri);
            //return BLFinOffre.GetListeQuittancesAnnulation(init, codeOffre, version, avenant, dateEffetAvenant, dateEmission, typeOperation, situation, datePeriodeDebut, datePeriodeFin, typeQuittances, user, acteGestion, isreadonly, modeNavig);
        }

        public void EnregistrerQuittancesAnnulees(string codeOffre, string version, string listeQuittancesAnnulees)
        {
            FinOffreRepository.EnregistrerQuittancesAnnulees(codeOffre, version, listeQuittancesAnnulees);
            //BLFinOffre.EnregistrerQuittancesAnnulees(codeOffre, version, listeQuittancesAnnulees);
        }

        #endregion

        #region Quittance - Calcul Forcé

        public QuittanceForceDto LoadCalculWindow(string codeOffre, string version, string avenant, string typeVal, string typeHTTTC, ModeConsultation modeNavig, string acteGestion)
        {
            return FinOffreRepository.LoadCalculWindow(codeOffre, version, avenant, typeVal, typeHTTTC, modeNavig, acteGestion);
        }
        public string ExistMntCalcul(string codeOffre, string version, string avenant, ModeConsultation modeNavig, string acteGestion)
        {
            return FinOffreRepository.ExistMntCalcul(codeOffre, version, avenant, modeNavig, acteGestion);
        }
        public string UpdateCalculForce(string codeOffre, string type, string version, string avenant, string typeVal, string typeHTTTC,
            string codeRsq, string codeFor, string montantForce, string user, string acteGestion, string reguleId)
        {
            return FinOffreRepository.UpdateCalculForce(codeOffre, type, version, avenant, typeVal, typeHTTTC, codeRsq, codeFor, montantForce, user, acteGestion, reguleId);
        }

        public QuittanceForceGarantieDto LoadGaranInfo(string codeOffre, string version, string avenant, string codeFor, ModeConsultation modeNavig, string acteGestion)
        {
            return FinOffreRepository.LoadGaranInfo(codeOffre, version, avenant, codeFor, modeNavig, acteGestion);
        }

        public QuittanceForceGarantieDto UpdateGaranForce(string codeOffre, string version, string avenant, string formId, string codeFor, string codeRsq, string codeGaran, string montantForce, string catnatForce, string codeTaxeForce, ModeConsultation modeNavig, string acteGestion)
        {
            return FinOffreRepository.UpdateGaranForce(codeOffre, version, avenant, formId, codeFor, codeRsq, codeGaran, montantForce, catnatForce, codeTaxeForce, modeNavig, acteGestion);
        }

        public string ValidFormGaranForce(string codeOffre, string type, string version, string avenant, string codeFor, string codeRsq, string user, string acteGestion)
        {
            return FinOffreRepository.ValidFormGaranForce(codeOffre, type, version, avenant, codeFor, codeRsq, user, acteGestion);
        }

        #endregion

        #region Périodes d'engagement
        public List<EngagementPeriodeDto> GetEngagementPeriodes(string codeOffre, string version, string type, string codeAvn, string user, ModeConsultation modeNavig)
        {

            if (codeAvn != string.Empty && codeAvn != "0")
            {
                //vérification de l'existence de l'enregistrement
                //2015-09-16 : fd_Point 15092015.docx
                var resVerif = EngagementRepository.VerifEngagementExist(codeOffre, version, type, codeAvn, modeNavig);
                var codePeriode = resVerif.Int64ReturnCol;
                var acteGestion = resVerif.StrReturnCol;
                //Mise en commentaire le 2016-01-15 : demande de FDU
                //if (codePeriode > 0)
                //{
                //    var result = CommonRepository.LoadAS400EngagementAvn(codeOffre, version, type, codePeriode.ToString(), codeAvn, user, acteGestion);
                //}
            }

            return EngagementRepository.GetEngagementPeriodes(codeOffre, version, type, codeAvn, modeNavig);
            //return BLFinOffre.GetEngagementPeriodes(codeOffre, version, type);
        }

        public List<ModeleDetailsConnexitesEngPeriodeDto> GetEngagementPeriodesDetails(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {

            if (codeAvn != string.Empty && codeAvn != "0")
            {
                var resVerif = EngagementRepository.VerifEngagementExist(codeOffre, version, type, codeAvn, modeNavig);
                var codePeriode = resVerif.Int64ReturnCol;
                var acteGestion = resVerif.StrReturnCol;
            }

            return EngagementRepository.GetEngagementPeriodesDetails(codeOffre, version, type, codeAvn, modeNavig);
        }

        public string SaveEngagementPeriode(string codeOffre, string version, string type, string codeAvn, string user, EngagementPeriodeDto engagementPeriodeDto, string typeOperation, ModeConsultation modeNavig, bool updateTable)
        {
            var toReturn = EngagementRepository.SaveEngagementPeriode(codeOffre, version, type, codeAvn, user, engagementPeriodeDto, typeOperation, modeNavig);

            if (updateTable)
            {
                CommonRepository.UpdateTableReassu(codeOffre, version, type);
            }

            return toReturn;
            //return BLFinOffre.SaveEngagementPeriode(codeOffre, version, type, user, engagementPeriodeDto, typeOperation, modeNavig);
        }

        public void SaveArbreEngagementPeriode(string codeOffre, string version, string type, string user)
        {
            #region Arbre de navigation

            NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = codeOffre.PadLeft(9, ' '),
                Version = Convert.ToInt32(version),
                Type = type,
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

            #endregion
            //BLFinOffre.SaveArbreEngagementPeriode(codeOffre, version, type, user);
        }
        public string DeleteEngagementPeriode(string code)
        {
            string retour = string.Empty;
            return retour;
        }

        public string GetDateControle(string branche, string cible)
        {
            return EngagementRepository.GetDateControle(branche, cible);
            //return BLFinOffre.GetDateControle(branche, cible);
        }

        #endregion

        #region Connexité

        public void SaveObsvConnexite(string codeAffaire, string version, string type, int codeObservation, string observation, string acteGestion, string reguleId)
        {
            EngagementRepository.SaveObsvConnexite(codeAffaire, version, type, codeObservation, observation, acteGestion, reguleId);
            //BLFinOffre.SaveObsvConnexite(codeObservation, observation);
        }

        public string GetNumeroConnexite(string codeOffre, string version, string type, string codeTypeConnexite)
        {
            return EngagementRepository.GetNumeroConnexite(codeOffre, version, type, codeTypeConnexite);
            //return BLFinOffre.GetNumeroConnexite(codeOffre, version, type, codeTypeConnexite);
        }

        public InfoContratConnexeDto GetInfoContratsConnexes(string codeOffre, string version, string type, string codeTypeConnexite)
        {
            InfoContratConnexeDto toReturn = new InfoContratConnexeDto
            {
                NumeroConnexite = EngagementRepository.GetNumeroConnexite(codeOffre, version, type, codeTypeConnexite)
                //NumeroConnexite = BLFinOffre.GetNumeroConnexite(codeOffre, version, type, codeTypeConnexite)
            };
            if (!string.IsNullOrEmpty(codeTypeConnexite) && !string.IsNullOrEmpty(toReturn.NumeroConnexite))
            {
                if (codeTypeConnexite == "20")
                    toReturn.ContratsConnexes = EngagementRepository.GetContratsConnexesEngagement(type, codeTypeConnexite, toReturn.NumeroConnexite);
                else
                    toReturn.ContratsConnexes = EngagementRepository.GetContratsConnexes(type, codeTypeConnexite, toReturn.NumeroConnexite);
                //toReturn.ContratsConnexes = BLFinOffre.GetContratsConnexes(type, codeTypeConnexite, toReturn.NumeroConnexite);
            }
            return toReturn;
        }

        public InfoConfirmationConnexeDto GetInfoConfirmationAction(string codeOffreActuel, string versionActuel, string typeActuel, string codeTypeConnexite, string numConnexite,
                                    string codeOffreAjoute, string versionAjoute, string typeAjoute,
                                    ModeConsultation modeNavig)
        {
            InfoConfirmationConnexeDto toReturn = new InfoConfirmationConnexeDto();
            if (!string.IsNullOrEmpty(numConnexite))
                toReturn.ContratsConnexesActuels = EngagementRepository.GetContratsConnexes(typeActuel, codeTypeConnexite, numConnexite);
            //toReturn.ContratsConnexesActuels = BLFinOffre.GetContratsConnexes(typeActuel, codeTypeConnexite, numConnexite);
            toReturn.NumConnexiteOrigine = EngagementRepository.GetNumeroConnexite(codeOffreAjoute, versionAjoute, typeAjoute, codeTypeConnexite);
            //toReturn.NumConnexiteOrigine = BLFinOffre.GetNumeroConnexite(codeOffreAjoute, versionAjoute, typeAjoute, codeTypeConnexite);
            if (!string.IsNullOrEmpty(codeTypeConnexite) && !string.IsNullOrEmpty(toReturn.NumConnexiteOrigine))
            {
                toReturn.ContratsConnexesOrigines = EngagementRepository.GetContratsConnexes(typeAjoute, codeTypeConnexite, toReturn.NumConnexiteOrigine);
                //toReturn.ContratsConnexesOrigines = BLFinOffre.GetContratsConnexes(typeAjoute, codeTypeConnexite, toReturn.NumConnexiteOrigine);
            }
            toReturn.ContratOrigine = BLCommon.GetContrat(codeOffreAjoute, versionAjoute, typeAjoute, "0", modeNavig);
            //toReturn.ContratOrigine = InitAffNouv.GetContrat(codeOffreAjoute, versionAjoute, typeAjoute, "0", modeNavig);

            return toReturn;
        }


        public List<ContratConnexeDto> GetContratsConnexes(string typeOffre, string codeTypeConnexite, string numeroConnexite)
        {
            return EngagementRepository.GetContratsConnexes(typeOffre, codeTypeConnexite, numeroConnexite);
            //return BLFinOffre.GetContratsConnexes(typeOffre, codeTypeConnexite, numeroConnexite);
        }
        public List<ContratConnexeDto> GetContratsConnexesEngagement(string typeOffre, string codeTypeConnexite, string numeroConnexite)
        {
            return EngagementRepository.GetContratsConnexesEngagement(typeOffre, codeTypeConnexite, numeroConnexite);
            //return BLFinOffre.GetContratsConnexes(typeOffre, codeTypeConnexite, numeroConnexite);
        }

        public List<ModeleDetailsConnexitesEngPeriodeDto> GetPeriodesConnexitesEngagement(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return EngagementRepository.GetPeriodesConnexitesEngagement(codeOffre, version, type, codeAvn, modeNavig);
        }

        public bool IsContratInConnexite(string codeOffre, string version, string type, string codeTypeConnexite, string numConnexite)
        {
            return EngagementRepository.IsContratInConnexite(codeOffre, version, type, codeTypeConnexite, numConnexite);
            //return BLFinOffre.IsContratInConnexite(codeOffre, version, type, codeTypeConnexite, numConnexite);
        }

        public string AddConnexite(string codeOffre_connexe, string version_connexe, string type_connexe, string branche_connexe, string sousBranche_connexe, string categorie_connexe,
                                          string codeOffre_courant, string version_courant, string type_courant, string branche_courant, string sousBranche_courant, string categorie_courant,
                                          string codeObservation, string observation, string codeTypeConnexite, string numConnexite, string mode)
        {
            return EngagementRepository.AddConnexite(codeOffre_connexe, version_connexe, type_connexe, branche_connexe, sousBranche_connexe, categorie_connexe,
                                   codeOffre_courant, version_courant, type_courant, branche_courant, sousBranche_courant, categorie_courant,
                                   codeObservation, observation, codeTypeConnexite, numConnexite, mode);
            //return BLFinOffre.AddConnexite(codeOffre_connexe, version_connexe, type_connexe, branche_connexe, sousBranche_connexe, categorie_connexe,
            //                                 codeOffre_courant, version_courant, type_courant, branche_courant, sousBranche_courant, categorie_courant,
            //                                 codeObservation, observation, codeTypeConnexite, numConnexite, mode);
        }


        public string AddConnexiteEngagement(string codeOffre_connexe, string version_connexe, string type_connexe, string branche_connexe, string sousBranche_connexe, string categorie_connexe,
                                          string codeOffre_courant, string version_courant, string type_courant, string branche_courant, string sousBranche_courant, string categorie_courant,
                                          string codeObservation, string observation, string codeTypeConnexite, string numConnexite, string mode)
        {
            return EngagementRepository.AddConnexiteEngagement(codeOffre_connexe, version_connexe, type_connexe, branche_connexe, sousBranche_connexe, categorie_connexe,
                                   codeOffre_courant, version_courant, type_courant, branche_courant, sousBranche_courant, categorie_courant,
                                   codeObservation, observation, codeTypeConnexite, numConnexite, mode);
            //return BLFinOffre.AddConnexite(codeOffre_connexe, version_connexe, type_connexe, branche_connexe, sousBranche_connexe, categorie_connexe,
            //                                 codeOffre_courant, version_courant, type_courant, branche_courant, sousBranche_courant, categorie_courant,
            //                                 codeObservation, observation, codeTypeConnexite, numConnexite, mode);
        }


        public string DeleteConnexite(string codeOffre_connexe, string version_connexe, string typeOffre_connexe, string numConnexite, string type_connexe, string ideConnexite, string user)
        {
            return EngagementRepository.DeleteConnexite(codeOffre_connexe, version_connexe, typeOffre_connexe, numConnexite, type_connexe, ideConnexite, user);
            //return BLFinOffre.DeleteConnexite(codeOffre_connexe, version_connexe, typeOffre_connexe, numConnexite, type_connexe, ideConnexite, user);
        }

        public void DeleteConnexiteEng(string codeOffre, int version, string type)
        {
            EngagementRepository.DeleteConnexiteEng(codeOffre, version, type);
        }

        public bool IsContratConnexe(string codeOffre, string version, string type, string codeTypeConnexite)
        {
            return EngagementRepository.IsContratConnexe(codeOffre, version, type, codeTypeConnexite);
            //return BLFinOffre.IsContratConnexe(codeOffre, version, type, codeTypeConnexite);
        }
        public string FusionDetachConnexite(
          string numOffreOrigine, string typeOffreOrigine, string versionOffreOrigine, string brancheOrigine, string sousBrancheOrigine, string catOrigine,
          string numConnexiteOrigine, long codeObsvOrigine, string obsvOrigine, string ideConnexiteOrigine,
          string numOffreActuelle, string typeOffreActuelle, string versionOffreActuelle, string brancheActuelle, string sousBrancheActuelle, string catActuelle,
          string numConnexiteActuelle, long codeObsvActuelle, string obsvActuelle, string codeTypeConnexite,
          string user, string modeAction)
        {
            return EngagementRepository.FusionDetachConnexite(numOffreOrigine, typeOffreOrigine, versionOffreOrigine, brancheOrigine, sousBrancheOrigine, catOrigine, numConnexiteOrigine, codeObsvOrigine, obsvOrigine, ideConnexiteOrigine, numOffreActuelle, typeOffreOrigine, versionOffreActuelle, brancheActuelle, sousBrancheActuelle, catActuelle, numConnexiteActuelle, codeObsvActuelle, obsvActuelle, codeTypeConnexite, user, modeAction);
            //return BLFinOffre.FusionDetachConnexite(numOffreOrigine, typeOffreOrigine, versionOffreOrigine, brancheOrigine, sousBrancheOrigine, catOrigine, numConnexiteOrigine, codeObsvOrigine, obsvOrigine, ideConnexiteOrigine, numOffreActuelle, typeOffreOrigine, versionOffreActuelle, brancheActuelle, sousBrancheActuelle, catActuelle, numConnexiteActuelle, codeObsvActuelle, obsvActuelle, codeTypeConnexite, user, modeAction);
        }
        public List<EngagementConnexiteTraiteDto> GetEngagementsTraites(string idEngagement)
        {
            return EngagementRepository.GetEngagementsTraites(idEngagement);
            //return BLFinOffre.GetEngagementsTraites(idEngagement);
        }

        public List<ContratConnexeTraiteDto> GetContratsConnexesTraite(string typeOffre, string codeTypeConnexite, string numeroConnexite)
        {
            return EngagementRepository.GetContratsConnexesTraite(typeOffre, codeTypeConnexite, numeroConnexite);
            //return BLFinOffre.GetContratsConnexesTraite(typeOffre, codeTypeConnexite, numeroConnexite);
        }
        public List<EngagementConnexiteDto> GetEngagementsConnexite(string IdeConnexiteEngagement)
        {
            return EngagementRepository.GetEngagementsConnexite(IdeConnexiteEngagement);
            //return BLFinOffre.GetEngagementsConnexite(IdeConnexiteEngagement);
        }
        //public List<EngagementConnexiteDto> GetEngagementsConnexiteTraite(string typeOffre, string codeTypeConnexite, string numeroConnexite)
        //{
        //    return BLFinOffre.GetEngagementsConnexiteTraite(typeOffre, codeTypeConnexite, numeroConnexite);
        //}

        public long UpdateEngagementTraite(string codeOffre, string versionOffre, string typeOffre, int dateDeb, int dateFin, int idEng, int idTraite, string codeFamille, long engTotal, long engAlbin, string user, string modeMaj)
        {
            return EngagementRepository.UpdateEngagementTraite(codeOffre, versionOffre, typeOffre, dateDeb, dateFin, idEng, idTraite, codeFamille, engTotal, engAlbin, user, modeMaj);
            //return BLFinOffre.UpdateEngagementTraite(codeOffre, versionOffre, typeOffre, dateDeb, dateFin, idEng, idTraite, codeFamille, engTotal, engAlbin, user, modeMaj);
        }
        public string AddEngagementFamille(long idEng, int dateDeb, int dateFin, string codeFamille, long engTotal, long engAlbin, string user)
        {
            return EngagementRepository.AddEngagementFamille(idEng, dateDeb, dateFin, codeFamille, engTotal, engAlbin, user);
            //return BLFinOffre.AddEngagementFamille(idEng, dateDeb, dateFin, codeFamille, engTotal, engAlbin, user);
        }
        #endregion

        #region Echeancier
        public EcheancierDto InitEcheancier(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string modeSaisieEcheancierParam)
        {
            return FinOffreRepository.InitEcheancier(codeOffre, version, type, codeAvn, modeNavig, modeSaisieEcheancierParam);
            //return BLFinOffre.InitEcheancier(codeOffre, version, type, codeAvn, modeNavig, modeSaisieEcheancierParam);
        }
        public List<EcheanceDto> GetEcheances(string codeOffre, string version, string type, string codeAvn, int typeEcheance, ModeConsultation modeNavig)
        {
            return FinOffreRepository.GetEcheances(codeOffre, version, type, codeAvn, typeEcheance, modeNavig);
            //return BLFinOffre.GetEcheances(codeOffre, version, type, codeAvn, typeEcheance, modeNavig);
        }
        public List<EcheanceDto> GetAllEcheances(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return FinOffreRepository.GetAllEcheances(codeOffre, version, type, codeAvn, modeNavig);
            //return BLFinOffre.GetAllEcheances(codeOffre, version, type, codeAvn, modeNavig);
        }
        public bool PossedeEcheances(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return FinOffreRepository.PossedeEcheances(codeOffre, version, type, codeAvn, modeNavig);
        }

        public string UpdateEcheance(string codeOffre, string version, string type, DateTime? dateEcheance, decimal PrimePourcent, decimal montantEcheance, decimal montantCalcule, decimal fraisAccessoires, bool taxeAttentat, string typeOperation, int typeEcheance)
        {
            return FinOffreRepository.UpdateEcheance(codeOffre, version, type, dateEcheance, PrimePourcent, montantEcheance, montantCalcule, fraisAccessoires, taxeAttentat, typeOperation, typeEcheance);
            //return BLFinOffre.UpdateEcheance(codeOffre, version, type, dateEcheance, PrimePourcent, montantEcheance, montantCalcule, fraisAccessoires, taxeAttentat, typeOperation, typeEcheance);
        }
        public string UpdatePourcentCalcule(string codeOffre, string version, string type, string codeAvn, double comptantHT, double primeHT, ModeConsultation modeNavig)
        {
            return FinOffreRepository.UpdatePourcentCalcule(codeOffre, version, type, codeAvn, comptantHT, primeHT, modeNavig);
            //return BLFinOffre.UpdatePourcentCalcule(codeOffre, version, type, codeAvn, comptantHT, primeHT, modeNavig);
        }
        public string UpdateMontantCalcule(string codeOffre, string version, string type, string codeAvn, string primePourcent, double comptantHT, double primeHT, ModeConsultation modeNavig)
        {
            return FinOffreRepository.UpdateMontantCalcule(codeOffre, version, type, codeAvn, primePourcent, comptantHT, primeHT, modeNavig);
            //return BLFinOffre.UpdateMontantCalcule(codeOffre, version, type, codeAvn, comptantHT, primeHT, modeNavig);
        }
        public void SupprimerEcheance(string codeOffre, string version, string type, DateTime dateEcheance)
        {
            FinOffreRepository.SupprimerEcheance(codeOffre, version, type, dateEcheance);
            //BLFinOffre.SupprimerEcheance(codeOffre, version, type, dateEcheance);
        }
        public void SupprimerEcheances(string codeOffre, string version, string type)
        {
            FinOffreRepository.SupprimerEcheances(codeOffre, version, type);
            //BLFinOffre.SupprimerEcheances(codeOffre, version, type);
        }

        public void SupprimerEcheancier(string codeOffre, string version, string type, string codeAvn)
        {
            FinOffreRepository.SupprimerEcheancier(codeOffre, version, type, codeAvn);
            //BLFinOffre.SupprimerEcheancier(codeOffre, version, type, codeAvn);
        }
        #endregion

        #region Montant Référence

        /// <summary>
        /// Initialise les informations de l'écran Montant Référence
        /// </summary>
        public MontantReferenceDto InitInfoMontantReference(string codeOffre, string version, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion)
        {
            return FinOffreRepository.InitInfoMontantReference(codeOffre, version, type, codeAvn, isReadonly, modeNavig, user, acteGestion);
            //return BLFinOffre.InitInfoMontantReference(codeOffre, version, type, codeAvn, isReadonly, modeNavig, user, acteGestion);
        }

        /// <summary>
        /// Recharge les données de l'écran Montant Référence
        /// </summary>
        public MontantReferenceDto ReloadMontantReference(string codeOffre, string version, string type, string codeAvn, string mode, bool isReadonly, ModeConsultation modeNavig, string user)
        {
            return FinOffreRepository.InitInfoMontantReference(codeOffre, version, type, codeAvn, isReadonly, modeNavig, user, mode);
            //return BLFinOffre.ReloadMontantReference(codeOffre, version, type, codeAvn, mode, isReadonly, modeNavig, user);
        }

        /// <summary>
        /// Charge les informations des montants références d'une formule
        /// </summary>
        public MontantReferenceInfoDto GetMontantFormule(string codeOffre, string version, string type, string codeAvn, string codeRsq, string codeForm, ModeConsultation modeNavig)
        {
            return FinOffreRepository.GetMontantFormule(codeOffre, version, type, codeAvn, codeRsq, codeForm, modeNavig);
            //return BLFinOffre.GetMontantFormule(codeOffre, version, type, codeAvn, codeRsq, codeForm, modeNavig);
        }

        /// <summary>
        /// Charge les totaux des montants références
        /// </summary>
        public MontantReferenceDto GetMontantTotal(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return FinOffreRepository.GetMontantTotal(codeOffre, version, type, codeAvn, modeNavig);
            //return BLFinOffre.GetMontantTotal(codeOffre, version, type, codeAvn, modeNavig);
        }

        /// <summary>
        /// Met à jour les montants références forcé/acquis
        /// </summary>
        public void ValidMontantFormule(string codeOffre, string version, string type, string codeRsq, string codeForm, decimal mntForce, bool mntProvi, decimal mntAcquis, bool chkMntAcquis)
        {
            FinOffreRepository.ValidMontantFormule(codeOffre, version, type, codeRsq, codeForm, mntForce, mntProvi, mntAcquis, chkMntAcquis);
            //BLFinOffre.ValidMontantFormule(codeOffre, version, type, codeRsq, codeForm, /*mntForce, mntProvi,*/ mntAcquis);
        }

        /// <summary>
        /// Met à jour les totaux des montants références forcé/acquis
        /// </summary>
        public void ValidMontantTotal(string codeOffre, string version, string type, decimal mntForce, decimal mntAcquis, bool checkedA, bool checkedP)
        {
            FinOffreRepository.ValidMontantTotal(codeOffre, version, type, mntForce, mntAcquis, checkedA, checkedP);
            //BLFinOffre.ValidMontantTotal(codeOffre, version, type, mntForce, mntProvi);
        }

        /// <summary>
        /// Rappelle le PGM 400 pour recharger les montants
        /// </summary>
        public MontantReferenceDto UpdateMontantRef(string codeOffre, string version, string type, string codeAvn, bool topForce, bool topAcquis, bool topForceTotal, string commentForce, Int64 codeCommentForce, string user, ModeConsultation modeNavig, string acteGestion, bool reset)
        {
            return FinOffreRepository.UpdateMontantRef(codeOffre, version, type, codeAvn, topForce, topAcquis, topForceTotal, commentForce, codeCommentForce, user, modeNavig, acteGestion, reset);
            //return BLFinOffre.UpdateMontantRef(codeOffre, version, type, codeAvn, topForce, topAcquis, topForceTotal, commentForce, codeCommentForce, user, modeNavig, acteGestion);
        }

        public void SaveCommentairesMontantRef(string codeOffre, string version, string type, Int64 codeCommentForce, string commentForce)
        {
            FinOffreRepository.SaveCommentairesMontantRef(codeOffre, version, type, codeCommentForce, commentForce);
            //BLFinOffre.SaveCommentairesMontantRef(codeOffre, version, type, codeCommentForce, commentForce);
        }

        #endregion

        #region Documents Joints

        public DocumentsJointsDto GetListDocumentsJoints(string codeOffre, string version, string type, string modeNavig, bool isReadOnly, string orderField, string orderType)
        {
            return FinOffreRepository.GetListDocumentsJoints(codeOffre, version, type, modeNavig, isReadOnly, orderField, orderType);
            //return BLFinOffre.GetListDocumentsJoints(codeOffre, version, type, modeNavig, isReadOnly, orderField, orderType);
        }

        public DocumentsAddDto OpenEditionDocsJoints(string idDoc)
        {
            return FinOffreRepository.OpenEditionDocsJoints(idDoc);
            //return BLFinOffre.OpenEditionDocsJoints(idDoc);
        }

        public DocumentsJointsDto SaveDocsJoints(string codeOffre, string version, string type, string idDoc, string typeDoc, string titleDoc, string fileDoc, string pathDoc, string refDoc, string user, string modeNavig, bool isReadOnly, string acteGestion)
        {
            return FinOffreRepository.SaveDocsJoints(codeOffre, version, type, idDoc, typeDoc, titleDoc, fileDoc, pathDoc, refDoc, user, modeNavig, isReadOnly, acteGestion);
            //return BLFinOffre.SaveDocsJoints(codeOffre, version, type, idDoc, typeDoc, titleDoc, fileDoc, pathDoc, refDoc, user, modeNavig, isReadOnly, acteGestion);
        }

        public DocumentsJointsDto DeleteDocsJoints(string idDoc, string codeOffre, string version, string type, string modeNavig, bool isReadOnly)
        {
            return FinOffreRepository.DeleteDocsJoints(idDoc, codeOffre, version, type, modeNavig, isReadOnly);
            //return BLFinOffre.DeleteDocsJoints(idDoc, codeOffre, version, type, modeNavig, isReadOnly);
        }

        public string ReloadPathFile(string typologie)
        {
            return FinOffreRepository.ReloadPathFile(typologie);
        }

        #endregion

        #region Suivi Documents

        public void InitSuiviDocuments()
        {

        }

        public SuiviDocumentsListeDto GetListSuiviDocuments(SuiviDocFiltreDto filtreDto, ModeConsultation modeNavig, bool readOnly)
        {
            return SuiviDocumentsRepository.GetListSuiviDocuments(filtreDto, modeNavig, readOnly);
            //return BLFinOffre.GetListSuiviDocuments(filtreDto, modeNavig, readOnly);
        }

        public bool GenerateDocuments(string numAffaire, string version, string type, string avenant, string lotId)
        {
            return SuiviDocumentsRepository.GenerateDocuments(numAffaire, version, type, avenant, lotId);
            //return BLFinOffre.GenerateDocuments(numAffaire, version, type, avenant, lotId);
        }

        public bool PrintDocuments(string lotId, string user)
        {
            return SuiviDocumentsRepository.PrintDocuments(lotId, user);
            //return BLFinOffre.PrintDocuments(lotId, user);
        }

        public bool ReeditDocument(string idDoc, string user)
        {
            return SuiviDocumentsRepository.ReeditDocument(idDoc, user);
            //return BLFinOffre.ReeditDocument(idDoc, user);
        }

        public List<string> EditerDocuments(string listeDocIdLogo, string listeDocIdNOLogo)
        {
            return SuiviDocumentsRepository.EditerDocuments(listeDocIdLogo, listeDocIdNOLogo);
            //return BLFinOffre.EditerDocuments(listeDocIdLogo, listeDocIdNOLogo);
        }

        /// <summary>
        /// Récupération du document généré avec les blocs word
        /// </summary>
        public string UpdDocCPCS(string docId)
        {
            var fullPathDoc = SuiviDocumentsRepository.GetBlocFullPath(docId);

            if (string.IsNullOrEmpty(fullPathDoc))
                return string.Empty;

            using (var serviceContext = new KheoBridge())
            {
                var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                if (!string.IsNullOrEmpty(kheoBridgeUrl))
                    serviceContext.Url = kheoBridgeUrl;

                return serviceContext.PreparerDocPourMiseEnPage(fullPathDoc);
            }
        }

        /// <summary>
        /// Sauvegarde du document avec les blocs word
        /// </summary>
        public void SaveBloc(string fullPathDoc)
        {
            using (var serviceContext = new KheoBridge())
            {
                var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                if (!string.IsNullOrEmpty(kheoBridgeUrl))
                    serviceContext.Url = kheoBridgeUrl;

                serviceContext.ValiderMiseEnPage(fullPathDoc);
            }
        }

        public FileDescription OpenPJ(string docId)
        {
            return SuiviDocumentsRepository.OpenPJ(docId);
        }

        public string RefreshDocuments(string docId)
        {
            try
            {
                using (var serviceContext = new KheoBridge())
                {
                    var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                    if (!string.IsNullOrEmpty(kheoBridgeUrl))
                        serviceContext.Url = kheoBridgeUrl;

                    serviceContext.RegenererDoc(!string.IsNullOrEmpty(docId) ? Convert.ToInt64(docId) : 0);
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                return "Erreur lors de la regénération du document.";
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FinOffre() {
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

        #endregion

    }
}
