using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Collections.Generic;
using System.ServiceModel;

namespace Albingia.Kheops.OP.Application.Port.Driver
{
    [ServiceContract]
    public interface IReferentielPort
    {
        [OperationContract] void InitAllCaches(bool @async = true);
        [OperationContract] IEnumerable<Traitement> GetActions();
        [OperationContract] IEnumerable<Branche> GetBranches();
        [OperationContract] IEnumerable<BaseCapitaux> GetBasesCapitaux(CibleFiltre cible = null);
        [OperationContract] IEnumerable<BasePrime> GetBasesPrime(CibleFiltre cible = null);
        [OperationContract] IEnumerable<BaseLCI> GetBasesLCI(CibleFiltre cible = null);
        [OperationContract] IEnumerable<BaseFranchise> GetBasesFranchise(CibleFiltre cible = null);
        [OperationContract] IEnumerable<CaractereGarantie> GetCaracteresGarantie();
        [OperationContract] IEnumerable<Cible> GetCibles();
        [OperationContract] IEnumerable<Devise> GetDevises(CibleFiltre cible);
        [OperationContract] IEnumerable<Encaissement> GetEncaissements();
        [OperationContract] IEnumerable<Etat> GetEtats();
        [OperationContract] IEnumerable<Indice> GetIndices();
        [OperationContract] IEnumerable<ModeModifiable> GetModesModifiables();
        [OperationContract] IEnumerable<MotifAvenant> GetMotifs();
        [OperationContract] IEnumerable<NatureAffaire> GetNatures();
        [OperationContract] IEnumerable<NatureGarantie> GetNaturesGarantie();
        [OperationContract] IEnumerable<Periodicite> GetPeriodicites();
        [OperationContract] IEnumerable<MotifSituation> GetMotifsSituations();
        [OperationContract] IEnumerable<TypeAccord> GetRetours();
        [OperationContract] IEnumerable<Situation> GetSituations();
        [OperationContract] void ResetAllCaches();
        [OperationContract] IEnumerable<Utilisateur> GetUtilisateurs();
        [OperationContract] IEnumerable<UniteCapitaux> GetUnitesCapitaux(CibleFiltre cible = null);
        [OperationContract] IEnumerable<UnitePrime> GetUnitesPrimes(CibleFiltre cible = null);
        [OperationContract] IEnumerable<UniteFranchise> GetUnitesFranchise(CibleFiltre cible = null);
        [OperationContract] IEnumerable<UniteLCI> GetUnitesLCI(CibleFiltre cible = null);
        [OperationContract] IEnumerable<Taxe> GetTaxes(CibleFiltre filtre = null);
        [OperationContract] IEnumerable<Alimentation> GetAlimentations(CibleFiltre filtre = null);
        [OperationContract] IEnumerable<UniteDuree> GetUnitesDuree(CibleFiltre filtre = null);
        [OperationContract] IEnumerable<TypeControleDate> GetTypesControleDate();
        [OperationContract] IEnumerable<TypeEmission> GetTypeEmissions(CibleFiltre filtre = null);
        [OperationContract] IEnumerable<PeriodeApplication> GetPeriodesApplications(CibleFiltre filtre = null);
        [OperationContract] IEnumerable<TypeGrilleRegul> GetTypesGrillesRegul(CibleFiltre filtre = null);
        //[OperationContract] IEnumerable<UnitePrime> GetUnitesPrimes(CibleFiltre filtre);
        [OperationContract] IEnumerable<Indisponibilite> GetCodeIndisponibilites(CibleFiltre filtre);
        [OperationContract] IEnumerable<IndisponibiliteTournage> GetCodesIndisponibilitesTournage(CibleFiltre filtre);
        [OperationContract] IEnumerable<UniteValeurRisque> GetUnitesValeursRisques(CibleFiltre filtre);
        [OperationContract] IEnumerable<TypeValeurRisque> GetTypesValeursRisques(CibleFiltre filtre);
        [OperationContract] IEnumerable<Quittancement> GetQuittancements();
        [OperationContract] IEnumerable<MotifRegularisation> GetMotifsRegularisation();
        [OperationContract] IEnumerable<CibleCatego> GetCiblesCategories();
        [OperationContract] IEnumerable<FamilleReassurance> GetFamillesReassurances();
        [OperationContract] IEnumerable<SituationSinistre> GetSituationsSinistres();
    }
}
