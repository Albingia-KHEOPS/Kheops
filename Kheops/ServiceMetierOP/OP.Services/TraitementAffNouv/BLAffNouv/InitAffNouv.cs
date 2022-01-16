using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmitMapper;
using OP.DataAccess;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Adresses;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.Avenant;
using ALBINGIA.Framework.Common.Tools;

namespace OP.Services.TraitementAffNouv.BLAffNouv
{
    public class InitAffNouv
    {
        #region Ecran Creation Affaire Nouvelle

        //public static CreationAffaireNouvelleDto InitCreateAffaireNouvelle(string codeOffre, string version, string type)
        //{
        //    return AffaireNouvelleRepository.InitCreateAffaireNouvelle(codeOffre, version, type);
        //}

        //public static CreationAffaireNouvelleContratDto InitAffaireNouvelleContrat(string codeOffre, string version, string type, string codeAvn, string user, ModeConsultation modeNavig)
        //{
        //    return AffaireNouvelleRepository.InitAffaireNouvelleContrat(codeOffre, version, type, codeAvn, user, modeNavig);
        //}
        //public static ContratInfoBaseDto InitContratInfoBase(string id, string version, string type, string codeAvn, string user, ModeConsultation modeNavig)
        //{
        //    return AffaireNouvelleRepository.InitContratInfoBase(id, version, type, codeAvn, user, modeNavig);
        //}

        //public static ContratDto GetContrat(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig)
        //{
        //    List<RisqueDto> risquesDto = new List<RisqueDto>();
        //    ContratDto contrat = AffaireNouvelleRepository.GetContrat(contratId, version, type, codeAvn, modeNavig);
        //    if (contrat != null)
        //    {
        //        if (contrat.DateEffetAvenantAnnee > 0 && contrat.DateEffetAvenantMois > 0 && contrat.DateEffetAvenantJour > 0)
        //            contrat.DateEffetAvenant = new DateTime(contrat.DateEffetAvenantAnnee, contrat.DateEffetAvenantMois, contrat.DateEffetAvenantJour);
        //        contrat.HeureEffetAvenant = AlbConvert.ConvertIntToTime(contrat.HeureAvn);
        //        var risques = PoliceRepository.ObtenirRisques(modeNavig, contratId, int.Parse(version), type, codeAvn);
        //        if (risques != null && risques.Count == 1) contrat.IsMonoRisque = true;
        //        else contrat.IsMonoRisque = false;
        //        foreach (var risque in risques)
        //        {
        //            var rsq = risque;// ObjectMapperManager.DefaultInstance.GetMapper<Risque, RisqueDto>().Map(risque);
        //            if (rsq != null)
        //            {
        //                if (risque.IdAdresseRisque > 0)
        //                {
        //                    var adresseRsq = AdresseRepository.ObtenirAdresse(risque.IdAdresseRisque);
        //                    if (adresseRsq != null)
        //                        rsq.AdresseRisque = adresseRsq;// ObjectMapperManager.DefaultInstance.GetMapper<Adresse, AdresseDto>().Map(adresseRsq);
        //                }
        //                risquesDto.Add(rsq);
        //            }
        //        }
        //        contrat.Risques = risquesDto;

        //    }
        //    return contrat;
        //}
        //public static void UpdatePeriodicite(string codeOffre, string version, string type, string periodicite)
        //{
        //    AffaireNouvelleRepository.UpdatePeriodicite(codeOffre, version, type, periodicite);
        //}
        //public static List<CourtierDto> GetListCourtiers(string contratId, string version, string type, string codeAvn, ModeConsultation modeNavig)
        //{
        //    return AffaireNouvelleRepository.GetListCourtiers(contratId, version, type, codeAvn, modeNavig);
        //}
        //public static CourtierDto GetCourtier(int CodeCourtier, string contratId, string version, string type)
        //{
        //    return AffaireNouvelleRepository.GetCourtier(CodeCourtier, contratId, version, type);
        //}
        //public static void InfoGeneralesContratModifier(ContratDto contrat, string utilisateur)
        //{
        //    AffaireNouvelleRepository.InfoGeneralesContratModifier(contrat, utilisateur);
        //    #region Arbre de navigation

        //    NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
        //    {
        //        CodeOffre = contrat.CodeContrat.PadLeft(9, ' '),
        //        Version = Convert.ToInt32(contrat.VersionContrat),
        //        Type = contrat.Type,
        //        EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale),
        //        NumeroOrdreDansEtape = 10,
        //        NumeroOrdreEtape = 1,
        //        Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale),
        //        Risque = 0,
        //        Objet = 0,
        //        IdInventaire = 0,
        //        Formule = 0,
        //        Option = 0,
        //        Niveau = string.Empty,
        //        CreationUser = utilisateur,
        //        PassageTag = "O",
        //        PassageTagClause = "N"
        //    });

        //    #endregion

        //}
        //public static string UpdateContrat(ContratInfoBaseDto contrat, string utilisateur, string acteGestion)
        //{
        //    #region Récupération des risques et des objets de risque
        //    if (!string.IsNullOrEmpty(contrat.CodeContrat) && !string.IsNullOrEmpty(contrat.Type))
        //    {
        //        var risquesDto = new List<RisqueDto>();
        //        var risques = PoliceRepository.ObtenirRisques(ModeConsultation.Standard, contrat.CodeContrat, int.Parse(contrat.VersionContrat.ToString()), contrat.Type);
        //        foreach (var risque in risques)
        //        {
        //            var rsq = risque;//ObjectMapperManager.DefaultInstance.GetMapper<Risque, RisqueDto>().Map(risque);
        //            if (rsq != null)
        //            {
        //                if (risque.IdAdresseRisque > 0)
        //                {
        //                    var adresseRsq = AdresseRepository.ObtenirAdresse(risque.IdAdresseRisque);
        //                    if (adresseRsq != null)
        //                        rsq.AdresseRisque = adresseRsq;//ObjectMapperManager.DefaultInstance.GetMapper<Adresse, AdresseDto>().Map(adresseRsq);
        //                }
        //                risquesDto.Add(rsq);
        //            }
        //        }
        //        contrat.Risques = risquesDto;
        //    }
        //    #endregion

        //    var result = AffaireNouvelleRepository.UpdateContrat(contrat, utilisateur, acteGestion);
        //    #region Ajout de l'acte de gestion
        //    if (string.IsNullOrEmpty(result))
        //        CommonRepository.AjouterActeGestion(contrat.CodeContrat, contrat.VersionContrat.ToString(), contrat.Type, 0, AlbConstantesMetiers.ACTEGESTION_GESTION, AlbConstantesMetiers.TRAITEMENT_AFFNV, "", utilisateur);
        //    #endregion
        //    return result;
        //}
        //public static string UpdateCourtier(string codeContrat, string versionContrat, string type, string typeCourtier, int identifiantCourtier, Single partCommission, string typeOperation, string user)
        //{
        //    string result = AffaireNouvelleRepository.UpdateCourtier(codeContrat, versionContrat, type, typeCourtier, identifiantCourtier, partCommission, typeOperation);
        //    #region Arbre de navigation
        //    NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
        //    {
        //        CodeOffre = codeContrat.PadLeft(9, ' '),
        //        Version = Convert.ToInt32(versionContrat),
        //        Type = type,
        //        EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoCourtier),
        //        NumeroOrdreDansEtape = 12,
        //        NumeroOrdreEtape = 1,
        //        Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoCourtier),
        //        Risque = 0,
        //        Objet = 0,
        //        IdInventaire = 0,
        //        Formule = 0,
        //        Option = 0,
        //        Niveau = string.Empty,
        //        CreationUser = user,
        //        PassageTag = "O",
        //        PassageTagClause = string.Empty
        //    });
        //    #endregion
        //    return result;
        //}
        //public static void SupprimerCourtier(string codeContrat, string versionContrat, int identifiantCourtier)
        //{
        //    AffaireNouvelleRepository.SupprimerCourtier(codeContrat, versionContrat, identifiantCourtier);
        //}
        //public static void ModifierCommissionCourtier(string codeContrat, string versionContrat, int identifiantCourtier, Single commission)
        //{
        //    AffaireNouvelleRepository.ModifierCommissionCourtier(codeContrat, versionContrat, identifiantCourtier, commission);
        //}
        //public static CommissionCourtierDto GetCommissionsStandardCourtier(string codeContrat, string versionContrat, string type, string codeAvn, bool isReadonly, ModeConsultation modeNavig, string user, string acteGestion)
        //{
        //    return AffaireNouvelleRepository.GetCommissionsStandardCourtier(codeContrat, versionContrat, type, codeAvn, isReadonly, modeNavig, user, acteGestion);
        //}
        //public static void UpdateCommissionsStandardCourtier(string codeContrat, string versionContrat, string type, CommissionCourtierDto commissionStandard)
        //{
        //    AffaireNouvelleRepository.UpdateCommissionsStandardCourtier(codeContrat, versionContrat, type, commissionStandard);
        //}
        //public static string CreateContrat(string codeOffre, string version, string type, string codeContrat, string versionContrat, string typeContrat, DateTime? dateAccord,
        //        DateTime? dateEffet, int heureEffet, string contratRemp, string versionRemp, string souscripteur, string gestionnaire, string branche, string cible, string observation, string user,
        //    string acteGestion)
        //{
        //    return AffaireNouvelleRepository.CreateContrat(codeOffre, version, type, codeContrat, versionContrat, typeContrat, dateAccord, dateEffet, heureEffet, contratRemp,
        //            versionContrat, souscripteur, gestionnaire, branche, cible, observation, user, acteGestion);
        //}

        //public static string VerifContratMere(string codeOffre, int version, string branche, string cible)
        //{
        //    return AffaireNouvelleRepository.VerifContratMere(codeOffre, version, branche, cible);
        //}

        //public static string VerifContratRemp(string codeOffre, int version)
        //{
        //    return AffaireNouvelleRepository.VerifContratRemp(codeOffre, version);
        //}

        //public static string GetNumeroAliment(string contratMere)
        //{
        //    return AffaireNouvelleRepository.GetNumeroAliment(contratMere);
        //}

        //public static void UpdateEtatContrat(string codeContrat, long version, string type, string etat)
        //{
        //    AffaireNouvelleRepository.UpdateEtatContrat(codeContrat, version, type, etat);
        //}

        //public static string ChangePreavisResil(string codeContrat, string version, string dateEffet, string dateFinEffet, string dateAvenant, string periodicite, string echeancePrincipale, string splitCharHtml, string user, string acteGestion)
        //{
        //    var ddEffet = AlbConvert.ConvertStrToDate(dateEffet);
        //    var dfEffet = AlbConvert.ConvertStrToDate(dateFinEffet);
        //    var dAvenant = AlbConvert.ConvertStrToDate(dateAvenant);
        //    DateTime? dEcheance = null;
        //    if (!string.IsNullOrEmpty(echeancePrincipale))
        //        dEcheance = AlbConvert.ConvertStrToDate(echeancePrincipale + "/2012");

        //    return CommonRepository.LoadPreavisResiliation(codeContrat, version, ddEffet, dfEffet, dAvenant, periodicite, dEcheance, splitCharHtml, user, acteGestion);
        //}

        //public static string ControleEcheance(string prochaineEcheance, string periodicite, string echeancePrincipale)
        //{
        //    return CommonRepository.ControleEcheance(prochaineEcheance, periodicite, echeancePrincipale);
        //}


        #endregion
        #region Ecran Choix Risque/Objet Affaire Nouvelle

        //public static RsqObjAffNouvDto InitRsqObjAffNouv(string codeOffre, string version, string type, string codeContrat, string versionContrat)
        //{
        //    return AffaireNouvelleRepository.InitRsqObjAffNouv(codeOffre, version, type, codeContrat, versionContrat);
        //}

        //public static void UpdateRsqObj(string codeContrat, string versionContrat, string type, string codeRsq, string codeObj, string isChecked)
        //{
        //    AffaireNouvelleRepository.UpdateRsqObj(codeContrat, versionContrat, type, codeRsq, codeObj, isChecked);
        //}

        #endregion
        #region Ecran Choix Formule/Volet Affaire Nouvelle

        //public static FormVolAffNouvDto InitFormVolAffNouv(string codeOffre, string version, string type, string codeContrat, string versionContrat)
        //{
        //    return AffaireNouvelleRepository.InitFormVolAffNouv(codeOffre, version, type, codeContrat, versionContrat);
        //}

        //public static void UpdateFormVol(string codeContrat, string versionContrat, string codeOffre, string version, string typeOffre, string codeForm, string guidForm, string codeOpt,
        //        string guidOpt, string guidVol, string type, string isChecked)
        //{
        //    AffaireNouvelleRepository.UpdateFormVol(codeContrat, versionContrat, codeOffre, version, typeOffre, codeForm, guidForm, codeOpt,
        //         guidOpt, guidVol, type, isChecked);
        //    PoliceRepository.UpdateIndexation(codeContrat, versionContrat, type);
        //}

        //public static FormVolAffNouvRecapDto GetListRsqForm(string codeContrat, string versionContrat)
        //{
        //    return AffaireNouvelleRepository.GetListRsqForm(codeContrat, versionContrat);
        //}

        #endregion
        #region Ecran Choix Options tarif

        //public static OptTarAffNouvDto InitOptTarifAffNouv(string codeContrat, string versionContrat)
        //{
        //    return AffaireNouvelleRepository.InitOptTarifAffNouv(codeContrat, versionContrat);
        //}

        //public static void UpdateOptTarif(string codeContrat, string versionContrat, string guidTarif)
        //{
        //    AffaireNouvelleRepository.UpdateOptTarif(codeContrat, versionContrat, guidTarif);
        //}

        //public static void ValidContrat(string codeOffre, string version, string type, string codeContrat, string versionContrat, string user, string splitChar, string acteGestion)
        //{
        //    AffaireNouvelleRepository.ValidContrat(codeOffre, version, type, codeContrat, versionContrat, user, splitChar, acteGestion);
        //}

        #endregion
        #region CoAssureurs

        //public static FormCoAssureurDto InitCoAssureurs(string type, string idOffre, string idAliment, string codeAvn, ModeConsultation modeNavig)
        //{
        //    return AffaireNouvelleRepository.InitCoAssureurs(type, idOffre, idAliment, codeAvn, modeNavig);
        //}

        //public static bool ExistCoAs(string idContrat, string version, string type, ModeConsultation modeNavig)
        //{
        //    return AffaireNouvelleRepository.ExistCoAs(idContrat, version, type, modeNavig);
        //}

        //public static CoAssureurDto GetCoAssureurDetail(string type, string idOffre, string idAliment, string idCoAssureur, bool modeCoAss)
        //{
        //    return AffaireNouvelleRepository.GetCoAssureurDetail(type, idOffre, idAliment, idCoAssureur, modeCoAss);
        //}

        //public static string EnregistrerListeCoAssureurs(string code, string version, string type, string typeAvenant, string avenant, List<CoAssureurDto> listeCoass, string user)
        //{
        //    string message = AffaireNouvelleRepository.EnregistrerListeCoAssureurs(code, version, type, typeAvenant, avenant, listeCoass, user);
        //    NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
        //    {
        //        CodeOffre = code.PadLeft(9, ' '),
        //        Version = Convert.ToInt32(version),
        //        Type = type,
        //        EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoAssureur),
        //        NumeroOrdreDansEtape = 15,
        //        NumeroOrdreEtape = 1,
        //        Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoAssureur),
        //        Risque = 0,
        //        Objet = 0,
        //        IdInventaire = 0,
        //        Formule = 0,
        //        Option = 0,
        //        Niveau = string.Empty,
        //        CreationUser = user,
        //        PassageTag = "O",
        //        PassageTagClause = string.Empty
        //    });
        //    return message;
        //}

        //public static string EnregistrerCoAssureur(string type, string idOffre, string idAliment, CoAssureurDto coAssureur, string typeOperation, string user)
        //{
        //    string result = AffaireNouvelleRepository.EnregistrerCoAssureur(type, idOffre, idAliment, coAssureur, typeOperation);
        //    NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
        //    {
        //        CodeOffre = idOffre.PadLeft(9, ' '),
        //        Version = Convert.ToInt32(idAliment),
        //        Type = type,
        //        EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoAssureur),
        //        NumeroOrdreDansEtape = 15,
        //        NumeroOrdreEtape = 1,
        //        Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoAssureur),
        //        Risque = 0,
        //        Objet = 0,
        //        IdInventaire = 0,
        //        Formule = 0,
        //        Option = 0,
        //        Niveau = string.Empty,
        //        CreationUser = user,
        //        PassageTag = "O",
        //        PassageTagClause = string.Empty
        //    });

        //    return result;
        //}

        //public static string SupprimerCoAssureur(string type, string idOffre, string idAliment, string guidId)
        //{
        //    return AffaireNouvelleRepository.SupprimerCoAssureur(type, idOffre, idAliment, guidId);
        //}


        #endregion
        //public static double GetMontantStatistique(string codeContrat, string version)
        //{
        //    return AffaireNouvelleRepository.GetMontantStatistique(codeContrat, version);
        //}
        #region retours signatures


        //public static List<ParametreDto> GetListeTypesAccord()
        //{
        //    return AffaireNouvelleRepository.GetListeTypesAccord();
        //}

        //public static RetourPreneurDto GetRetourPreneur(string codeContrat, string versionContrat, string typeContrat)
        //{
        //    return AffaireNouvelleRepository.GetRetourPreneur(codeContrat, versionContrat, typeContrat);
        //}

        //public static List<RetourCoassureurDto> GetRetoursCoassureurs(string codeContrat, string versionContrat, string typeContrat)
        //{
        //    return AffaireNouvelleRepository.GetRetoursCoassureurs(codeContrat, versionContrat, typeContrat);
        //}

        //public static RetourPreneurDto GetRetourPreneur(string codeContrat, string version, string type, string codeAvt, ModeConsultation modeNavig)
        //{
        //    return AffaireNouvelleRepository.GetRetourPreneur(codeContrat, version, type, codeAvt, modeNavig);
        //}

        //public static List<RetourCoassureurDto> GetRetoursCoassureurs(string codeContrat, string version, string type, string codeAvt, ModeConsultation modeNavig)
        //{
        //    return AffaireNouvelleRepository.GetRetoursCoassureurs(codeContrat, version, type, codeAvt, modeNavig);
        //}

        //public static void EnregistrerRetours(string codeContrat, string versionContrat, string typeContrat, string codeAvt, RetourPreneurDto retourPreneur, List<RetourCoassureurDto> retoursCoAssureurs, ModeConsultation modeNavig, string user)
        //{
        //    if (modeNavig == ModeConsultation.Historique)
        //        AffaireNouvelleRepository.EnregistrerRetoursHisto(codeContrat, versionContrat, typeContrat, codeAvt, retourPreneur, retoursCoAssureurs, modeNavig);
        //    else
        //        AffaireNouvelleRepository.EnregistrerRetours(codeContrat, versionContrat, typeContrat, retourPreneur, retoursCoAssureurs, user);
        //}


        #endregion
        #region Template
        //public static ContratInfoBaseDto GetInfoTemplate(string idTemp)
        //{
        //    return AffaireNouvelleRepository.GetInfoTemplate(idTemp);
        //}
        #endregion
        #region Blocage termes

        //public static List<ParametreDto> GetListeZonesStop()
        //{
        //    return AffaireNouvelleRepository.GetListeZonesStop();
        //}

        //public static string GetZoneStop(string codeContrat, string versionContrat, string typeContrat)
        //{
        //    return AffaireNouvelleRepository.GetZoneStop(codeContrat, versionContrat, typeContrat);
        //}
        //public static void SaveZoneStop(string codeContrat, string versionContrat, string typeContrat, string zoneStop)
        //{
        //    AffaireNouvelleRepository.SaveZoneStop(codeContrat, versionContrat, typeContrat, zoneStop);
        //}

        //public static DeblocageTermeDto GetEcheanceEmission(string codeContrat, string versionContrat, string typeContrat, string mode, string user, string acteGestion)
        //{
        //    return AffaireNouvelleRepository.GetEcheanceEmission(codeContrat, versionContrat, typeContrat, mode, user, acteGestion);
        //}

        #endregion


    }
}
