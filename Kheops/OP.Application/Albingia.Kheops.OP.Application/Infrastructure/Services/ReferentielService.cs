using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Tools;
using OP.IOWebService.LightInject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Activation;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReferentielService : IReferentielPort
    {
        private readonly IConfig config;
        private readonly IParamRepository paramRepository;
        private readonly IReferentialRepository refRepo;
        private readonly IInfosSpecifiquesRepository infosSpecifiquesRepository;

        public ReferentielService(IConfig conf, IReferentialRepository refRepo, IParamRepository paramRepository, IInfosSpecifiquesRepository infosSpecifiquesRepository)
        {
            this.config = conf;
            this.refRepo = refRepo;
            this.paramRepository = paramRepository;
            this.infosSpecifiquesRepository = infosSpecifiquesRepository;
        }

        public void InitAllCaches(bool @async = true) {
            try {
                if (@async) {
                    // do not wait on purpose
                    InitCachesAsync();
                }
                else {
                    InitCaches();
                }
            }
            catch (Exception ex) {
                try {
                    AlbLog.Log($"Error while executing {nameof(IReferentielPort)}.{nameof(InitAllCaches)}: {ex}", AlbLog.LogTraceLevel.Erreur);
                }
                catch (Exception exlog)
                {
                    Trace.TraceError(exlog.ToString());
                }
            }
        }

        public void ResetAllCaches() {
            InitCaches(new IRepositoryCache[] { this.paramRepository, this.refRepo, this.infosSpecifiquesRepository }, true);
        }

        public IEnumerable<Utilisateur> GetUtilisateurs() { return this.refRepo.GetUtilisateurs(); }
        public IEnumerable<Traitement> GetActions() { return this.refRepo.GetValues<Traitement>(); }
        public IEnumerable<Branche> GetBranches() { return this.refRepo.GetValues<Branche>(); }
        public IEnumerable<BaseCapitaux> GetBasesCapitaux(CibleFiltre cible = null) { return cible == null ? this.refRepo.GetValues<BaseCapitaux>() : this.refRepo.GetValues<BaseCapitaux>().Filter(cible); }
        public IEnumerable<BasePrime> GetBasesPrime(CibleFiltre cible = null) { return cible == null ? this.refRepo.GetValues<BasePrime>() : this.refRepo.GetValues<BasePrime>().Filter(cible); }
        public IEnumerable<BaseLCI> GetBasesLCI(CibleFiltre cible = null) { return cible == null ? this.refRepo.GetValues<BaseLCI>() : this.refRepo.GetValues<BaseLCI>().Filter(cible); }
        public IEnumerable<BaseFranchise> GetBasesFranchise(CibleFiltre cible = null) { return cible == null ? this.refRepo.GetValues<BaseFranchise>() : this.refRepo.GetValues<BaseFranchise>().Filter(cible); }
        public IEnumerable<CaractereGarantie> GetCaracteresGarantie() { return this.refRepo.GetValues<CaractereGarantie>(); }
        public IEnumerable<Cible> GetCibles() { return this.refRepo.GetCibles(); }
        public IEnumerable<Devise> GetDevises(CibleFiltre cible) { return this.refRepo.GetValues<Devise>().Filter(cible).ToList(); }
        public IEnumerable<Encaissement> GetEncaissements() { return this.refRepo.GetValues<Encaissement>(); }
        public IEnumerable<Etat> GetEtats() { return this.refRepo.GetValues<Etat>(); }
        public IEnumerable<Indice> GetIndices() { return this.refRepo.GetValues<Indice>(); }
        public IEnumerable<ModeModifiable> GetModesModifiables() { return this.refRepo.GetValues<ModeModifiable>(); }
        public IEnumerable<MotifAvenant> GetMotifs() { return this.refRepo.GetValues<MotifAvenant>(); }
        public IEnumerable<MotifSituation> GetMotifsSituations() { return this.refRepo.GetValues<MotifSituation>(); }
        public IEnumerable<NatureAffaire> GetNatures() { return this.refRepo.GetValues<NatureAffaire>(); }
        public IEnumerable<NatureGarantie> GetNaturesGarantie() { return this.refRepo.GetValues<NatureGarantie>(); }
        public IEnumerable<Periodicite> GetPeriodicites() { return this.refRepo.GetValues<Periodicite>(); }
        public IEnumerable<TypeAccord> GetRetours() { return this.refRepo.GetValues<TypeAccord>(); }
        public IEnumerable<Situation> GetSituations() { return this.refRepo.GetValues<Situation>(); }
        public IEnumerable<FamilleReassurance> GetFamillesReassurances() { return this.refRepo.GetValues<FamilleReassurance>(); }
        public IEnumerable<SituationSinistre> GetSituationsSinistres() { return this.refRepo.GetValues<SituationSinistre>(); }

        public IEnumerable<UniteCapitaux> GetUnitesCapitaux(CibleFiltre cible = null) { return cible == null ? this.refRepo.GetValues<UniteCapitaux>().ToList() : this.refRepo.GetValues<UniteCapitaux>().Filter(cible).ToList(); }
        public IEnumerable<UnitePrime> GetUnitesPrimes(CibleFiltre cible = null) { return cible == null ? this.refRepo.GetValues<UnitePrime>().ToList() : this.refRepo.GetValues<UnitePrime>().Filter(cible).ToList(); }
        public IEnumerable<UniteFranchise> GetUnitesFranchise(CibleFiltre cible = null) { return cible == null ? this.refRepo.GetValues<UniteFranchise>().ToList() : this.refRepo.GetValues<UniteFranchise>().Filter(cible).ToList(); }
        public IEnumerable<UniteLCI> GetUnitesLCI(CibleFiltre cible = null) { return cible == null ? this.refRepo.GetValues<UniteLCI>().ToList() : this.refRepo.GetValues<UniteLCI>().Filter(cible).ToList(); }

        public IEnumerable<Taxe> GetTaxes(CibleFiltre filtre = null)
        {
            return filtre == null ? refRepo.GetValues<Taxe>().ToList() : refRepo.GetValues<Taxe>().Filter(filtre).ToList();
        }

        public IEnumerable<Alimentation> GetAlimentations(CibleFiltre filtre = null)
        {
            return filtre == null ? refRepo.GetValues<Alimentation>().ToList() : refRepo.GetValues<Alimentation>().Filter(filtre).ToList();
        }

        public IEnumerable<UniteDuree> GetUnitesDuree(CibleFiltre filtre = null)
        {
            return filtre == null ? refRepo.GetValues<UniteDuree>().ToList() : refRepo.GetValues<UniteDuree>().Filter(filtre).ToList();
        }

        public IEnumerable<TypeControleDate> GetTypesControleDate()
        {
            return refRepo.GetValues<TypeControleDate>().ToList();
        }

        public IEnumerable<TypeEmission> GetTypeEmissions(CibleFiltre filtre = null)
        {
            return filtre == null ? refRepo.GetValues<TypeEmission>().ToList() : refRepo.GetValues<TypeEmission>().Filter(filtre).ToList();
        }

        public IEnumerable<PeriodeApplication> GetPeriodesApplications(CibleFiltre filtre = null)
        {
            return filtre == null ? refRepo.GetValues<PeriodeApplication>().ToList() : refRepo.GetValues<PeriodeApplication>().Filter(filtre).ToList();
        }

        public IEnumerable<TypeGrilleRegul> GetTypesGrillesRegul(CibleFiltre filtre = null)
        {
            return filtre == null ? refRepo.GetValues<TypeGrilleRegul>().ToList() : refRepo.GetValues<TypeGrilleRegul>().Filter(filtre).ToList();
        }

        public IEnumerable<Indisponibilite> GetCodeIndisponibilites(CibleFiltre filtre)
        {
            return refRepo.GetValues<Indisponibilite>().Filter(filtre).ToList();
        }

        public IEnumerable<IndisponibiliteTournage> GetCodesIndisponibilitesTournage(CibleFiltre filtre)
        {
            return refRepo.GetValues<IndisponibiliteTournage>().Filter(filtre).ToList();
        }

        public IEnumerable<UniteValeurRisque> GetUnitesValeursRisques(CibleFiltre filtre)
        {
            return refRepo.GetValues<UniteValeurRisque>().Filter(filtre).ToList();
        }

        public IEnumerable<TypeValeurRisque> GetTypesValeursRisques(CibleFiltre filtre)
        {
            return refRepo.GetValues<TypeValeurRisque>().Filter(filtre).ToList();
        }

        public IEnumerable<CibleCatego> GetCiblesCategories()
        {
            return refRepo.GetCibleCategos().ToList();
        }

        public IEnumerable<Quittancement> GetQuittancements()
        {
            return this.refRepo.GetValues<Quittancement>().ToList();
        }

        public IEnumerable<MotifRegularisation> GetMotifsRegularisation()
        {
            return this.refRepo.GetValues<MotifRegularisation>().ToList();
        }

        private static void InitCaches(IEnumerable<IRepositoryCache> cacheRepos = null, bool resetBefore = false) {
            if (cacheRepos is null) {
                using (var scope = LightInjectServiceHostFactory.CallBeginScope()) {
                    typeof(IParamRepository).Assembly.GetExportedTypes()
                        .Where(t => t.IsInterface && t.GetInterfaces().Any(it => it == typeof(IRepositoryCache)))
                        .ToList()
                        .ForEach(x => {
                            try {
                                var cacheRepo = scope.GetInstance(x) as IRepositoryCache;
                                if (resetBefore) {
                                    cacheRepo.ResetCache();
                                }
                                cacheRepo.InitCache();
                            }
                            catch { }
                        });
                }
            }
            else {
                cacheRepos.ToList().ForEach(r => {
                    if (resetBefore) {
                        r.ResetCache();
                    }
                    r.InitCache();
                });
            }
        }

        private static async Task InitCachesAsync() {
            await Task.Run(() => InitCaches());
        }
    }
}
