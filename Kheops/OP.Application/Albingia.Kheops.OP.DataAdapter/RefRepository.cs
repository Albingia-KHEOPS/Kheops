using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Parametrage;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Albingia.Kheops.OP.DataAdapter {
    public partial class RefRepository : IReferentialRepository, IFiltreRepository
    {
        private readonly UtilisateurRepository utilRepo;
        private readonly YparRepository yparRepo;
        private readonly KcibleRepository cibleRepo;
        private readonly KcliblefRepository ciblefRepo;
        private readonly YcategoRepository ycatRepo;
        private readonly KFiltreRepository kFiltreRepository;
        private readonly KFiltrLRepository kFiltrLRepository;

        private readonly IGenericCache cache;

        public RefRepository(
            UtilisateurRepository utilRepo,
            YparRepository yrepo,
            YcategoRepository ycatrepo,
            KcibleRepository cibleRepo,
            KcliblefRepository ciblefRepo,
            KFiltreRepository kFiltreRepository,
            KFiltrLRepository kFiltrLRepository,
            IGenericCache cache)
        {
            this.utilRepo = utilRepo;
            this.yparRepo = yrepo;
            this.ycatRepo = ycatrepo;
            this.cibleRepo = cibleRepo;
            this.ciblefRepo = ciblefRepo;
            this.cache = cache;
            this.kFiltreRepository = kFiltreRepository;
            this.kFiltrLRepository = kFiltrLRepository;
        }
        private struct ConceptFam
        {
            public string Concept;
            public string Famille;
            public Boolean Filtered;
        }
        private static Dictionary<Type, ConceptFam> codes = new Dictionary<Type, ConceptFam>
        {
            [typeof(ActeGestion)]                = new ConceptFam { Concept = "KHEOP", Famille = "ACTGS", Filtered = false },
            [typeof(Alimentation)]               = new ConceptFam { Concept = "KHEOP", Famille = "C4ALA", Filtered = false },
            [typeof(Antecedant)]                 = new ConceptFam { Concept = "PRODU", Famille = "PBANT", Filtered = true },
            [typeof(ApplicationFraisAccessoire)] = new ConceptFam { Concept = "PRODU", Famille = "FBAFC", Filtered = false },
            [typeof(BaseCapitaux)]               = new ConceptFam { Concept = "ALSPK", Famille = "BACAP", Filtered = true },
            [typeof(BaseFranchise)]              = new ConceptFam { Concept = "ALSPK", Famille = "BAFRA", Filtered = true },
            [typeof(BaseLCI)]                    = new ConceptFam { Concept = "ALSPK", Famille = "BALCI", Filtered = true },
            [typeof(BasePrime)]                  = new ConceptFam { Concept = "ALSPK", Famille = "BAPRI", Filtered = true },
            [typeof(Branche)]                    = new ConceptFam { Concept = "GENER", Famille = "BRCHE", Filtered = false },
            [typeof(CaractereGarantie)]          = new ConceptFam { Concept = "PRODU", Famille = "CBCAR", Filtered = false },
            [typeof(Contexte)]                   = new ConceptFam { Concept = "KHEOP", Famille = "CTX", Filtered = true },
            [typeof(DefinitionGarantie)]         = new ConceptFam { Concept = "PRODU", Famille = "GADFG", Filtered = false },
            [typeof(Devise)]                     = new ConceptFam { Concept = "GENER", Famille = "DEVIS", Filtered = true },
            [typeof(Encaissement)]               = new ConceptFam { Concept = "GENER", Famille = "TCYEN", Filtered = false },
            [typeof(Etat)]                       = new ConceptFam { Concept = "PRODU", Famille = "PBETA", Filtered = false },
            [typeof(FamilleGarantie)]            = new ConceptFam { Concept = "REASS", Famille = "GARAN", Filtered = false },
            [typeof(FamilleReassurance)]         = new ConceptFam { Concept = "REASS", Famille = "GARAN", Filtered = false },
            [typeof(FraisGareat)]                = new ConceptFam { Concept = "GAREA", Famille = "FRAIS", Filtered = false },
            [typeof(Indice)]                     = new ConceptFam { Concept = "GENER", Famille = "INDIC", Filtered = true },
            [typeof(Indisponibilite)]            = new ConceptFam { Concept = "KHEOP", Famille = "INDIS", Filtered = true },
            [typeof(IndisponibiliteTournage)]    = new ConceptFam { Concept = "KHEOP", Famille = "INDAU", Filtered = false },
            [typeof(ModeRegularisation)]         = new ConceptFam { Concept = "KHEOP", Famille = "RGMRG", Filtered = false },
            [typeof(ModeModifiable)]             = new ConceptFam { Concept = "ALSPK", Famille = "MODIF", Filtered = false },
            [typeof(MotCle)]                     = new ConceptFam { Concept = "PRODU", Famille = "POMOC", Filtered = true },
            [typeof(MotifAvenant)]               = new ConceptFam { Concept = "PRODU", Famille = "PBAVC", Filtered = false },
            [typeof(MotifRegularisation)]        = new ConceptFam { Concept = "ALSPK", Famille = "RGMTF", Filtered = false },
            [typeof(MotifResiliation)]           = new ConceptFam { Concept = "PRODU", Famille = "PBRSC", Filtered = false },
            [typeof(MotifSituation)]             = new ConceptFam { Concept = "PRODU", Famille = "PBSTF", Filtered = false },
            [typeof(CodeSTOP)]                   = new ConceptFam { Concept = "PRODU", Famille = "PBSTP", Filtered = false },
            [typeof(NatureAffaire)]              = new ConceptFam { Concept = "PRODU", Famille = "PBNPL", Filtered = false },
            [typeof(NatureTravauxAffaire)]       = new ConceptFam { Concept = "PRODU", Famille = "PBNAT", Filtered = false },
            [typeof(NatureGarantie)]             = new ConceptFam { Concept = "PRODU", Famille = "CBNAT", Filtered = false },
            [typeof(NatureLieu)]                 = new ConceptFam { Concept = "ALSPK", Famille = "NLOC", Filtered = false },
            [typeof(Pays)]                       = new ConceptFam { Concept = "GENER", Famille = "CPAYS", Filtered = false },
            [typeof(PeriodeApplication)]         = new ConceptFam { Concept = "PRODU", Famille = "JHPRP", Filtered = false },
            [typeof(Periodicite)]                = new ConceptFam { Concept = "PRODU", Famille = "PBPER", Filtered = false },
            [typeof(QualiteJuridique)]           = new ConceptFam { Concept = "ALSPK", Famille = "QJUR", Filtered = false },
            [typeof(Quittancement)]              = new ConceptFam { Concept = "PRODU", Famille = "FBENC", Filtered = false },
            [typeof(RegimeTaxe)]                 = new ConceptFam { Concept = "GENER", Famille = "TAXRG", Filtered = true },
            [typeof(Renonciation)]               = new ConceptFam { Concept = "ALSPK", Famille = "REN", Filtered = false },
            [typeof(RisqueLocatif)]              = new ConceptFam { Concept = "KHEOP", Famille = "BFRLO", Filtered = false },
            [typeof(Situation)]                  = new ConceptFam { Concept = "PRODU", Famille = "PBSIT", Filtered = false },
            [typeof(SituationRegularisation)]    = new ConceptFam { Concept = "KHEOP", Famille = "RGUST", Filtered = false },
            [typeof(SituationSinistre)]          = new ConceptFam { Concept = "SINIS", Famille = "SISIT", Filtered = false },
            [typeof(Tampon)]                     = new ConceptFam { Concept = "KHEOP", Famille = "TAMP", Filtered = false },
            [typeof(Taxe)]                       = new ConceptFam { Concept = "GENER", Famille = "TAXEC", Filtered = false },
            [typeof(Traitement)]                 = new ConceptFam { Concept = "PRODU", Famille = "PBTTR", Filtered = false },
            [typeof(TraceTraitement)]            = new ConceptFam { Concept = "PRODU", Famille = "PYTTR", Filtered = false },
            [typeof(TypeAccord)]                 = new ConceptFam { Concept = "PRODU", Famille = "PBTAC", Filtered = false },
            [typeof(TypeActeDeGestion)]          = new ConceptFam { Concept = "KHEOP", Famille = "ACTGS", Filtered = false },
            [typeof(TypeControleDate)]           = new ConceptFam { Concept = "KHEOP", Famille = "CTLDT", Filtered = false },
            [typeof(TypeEmission)]               = new ConceptFam { Concept = "PRODU", Famille = "JHPRE", Filtered = false },
            [typeof(TypeEnvoi)]                  = new ConceptFam { Concept = "KHEOP", Famille = "TYENV", Filtered = false },
            [typeof(TypeGrilleRegul)]            = new ConceptFam { Concept = "PRODU", Famille = "GATRG", Filtered = false },
            [typeof(TypeImmobilier)]             = new ConceptFam { Concept = "KHEOP", Famille = "IMMO", Filtered = false },
            [typeof(TypeLocation)]               = new ConceptFam { Concept = "KHEOP", Famille = "AULOC", Filtered = false },
            [typeof(TypeMateriel)]               = new ConceptFam { Concept = "KHEOP", Famille = "MATRS", Filtered = false },
            [typeof(TypePolice)]                 = new ConceptFam { Concept = "KHEOP", Famille = "TYPOC", Filtered = false },
            [typeof(TypeProdution)]              = new ConceptFam { Concept = "KHEOP", Famille = "AUPRO", Filtered = false },
            [typeof(TypeRelance)]                = new ConceptFam { Concept = "PRODU", Famille = "PKREL", Filtered = false },
            [typeof(TypeTraitement)]             = new ConceptFam { Concept = "PRODU", Famille = "PBTTR", Filtered = false },
            [typeof(TypeValeurRisque)]           = new ConceptFam { Concept = "PRODU", Famille = "QCVAT", Filtered = true },
            [typeof(TypoDocument)]               = new ConceptFam { Concept = "KHEOP", Famille = "TDOC", Filtered = false },
            [typeof(UniteCapitaux)]              = new ConceptFam { Concept = "ALSPK", Famille = "UNCAP", Filtered = true },
            [typeof(UniteDuree)]                 = new ConceptFam { Concept = "PRODU", Famille = "QBVGU", Filtered = false },
            [typeof(UniteFranchise)]             = new ConceptFam { Concept = "ALSPK", Famille = "UNFRA", Filtered = true },
            [typeof(UniteLCI)]                   = new ConceptFam { Concept = "ALSPK", Famille = "UNLCI", Filtered = true },
            [typeof(UnitePrime)]                 = new ConceptFam { Concept = "ALSPK", Famille = "UNPRI", Filtered = true },
            [typeof(UniteValeurRisque)]          = new ConceptFam { Concept = "PRODU", Famille = "QCVAU", Filtered = true },
            [typeof(UniteTemps)]                 = new ConceptFam { Concept = "PRODU", Famille = "PBCTU", Filtered = false },
            [typeof(Couverture)]                 = new ConceptFam { Concept = "KHEOP", Famille = "ICOUV", Filtered = false },
            [typeof(Convention)]                 = new ConceptFam { Concept = "KHEOP", Famille = "ICONV", Filtered = false },
            [typeof(Tournage)]                   = new ConceptFam { Concept = "KHEOP", Famille = "ITOUR", Filtered = false },
        };

        public void InitCache() {
            var refTypes = typeof(RefValue).Assembly.GetExportedTypes();
            foreach (var tp in refTypes.Where(x => typeof(RefValue).IsAssignableFrom(x) && !x.IsAbstract)) {
                GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .First(m => m.Name == nameof(IReferentialRepository.GetValues) && m.ContainsGenericParameters)
                    .MakeGenericMethod(tp).Invoke(this, new object[0]);
            }
            GetCibleCategos();
            GetCibles();
            GetUtilisateurs();
        }

        public void ResetCache() {
            const string globalKey = "/ALL/";
            var refTypes = typeof(RefValue).Assembly.GetExportedTypes();
            foreach (var tp in refTypes.Where(x => typeof(RefValue).IsAssignableFrom(x) && !x.IsAbstract)) {
                this.cache.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .First(m => m.Name == nameof(IGenericCache.Invalidate)
                        && m.ContainsGenericParameters
                        && m.GetGenericArguments().Distinct().Count() == 1)
                    .MakeGenericMethod(tp).Invoke(this.cache, new object[] { globalKey });
            }

            this.cache.Invalidate<Cible>(globalKey);
            this.cache.Invalidate<CibleCatego>(globalKey);
            this.cache.Invalidate<Domain.Referentiel.Utilisateur>(globalKey);

            InitCache();
        }

        public Unite GetUnite(TypeDeValeur typeDeValeur, string unite)
        {
            switch (typeDeValeur)
            {
                case TypeDeValeur.Assiette:
                    return GetValue<UniteCapitaux>(unite);
                case TypeDeValeur.Prime:
                    return GetValue<UnitePrime>(unite);
                case TypeDeValeur.LCI:
                    return GetValue<UniteLCI>(unite);
                case TypeDeValeur.Franchise:
                case TypeDeValeur.FranchiseMin:
                case TypeDeValeur.FranchiseMax:
                    return GetValue<UniteFranchise>(unite);
                default:
                    //return default(Unite);
                    throw new ArgumentException("Valeur invalide de type de valeur", nameof(typeDeValeur));
            }
        }
        public BaseDeCalcul GetBase(TypeDeValeur typeDeValeur, string @base)
        {
            switch (typeDeValeur)
            {
                case TypeDeValeur.Assiette:
                    return GetValue<BaseCapitaux>(@base);
                case TypeDeValeur.Prime:
                    return GetValue<BasePrime>(@base);
                case TypeDeValeur.LCI:
                    return GetValue<BaseLCI>(@base);
                case TypeDeValeur.Franchise:
                case TypeDeValeur.FranchiseMin:
                case TypeDeValeur.FranchiseMax:
                    return GetValue<BaseFranchise>(@base);
                default:
                    //return default(BaseDeCalcul);
                    throw new ArgumentException("Valeur invalide de type de valeur", nameof(typeDeValeur));
            }
        }


        public Domain.Referentiel.Utilisateur GetUtilisateur(string cible)
        {
            return this.GetIntervenantsThroughCache().FirstOrDefault(x => x.Code.IsSameAs(cible));
        }

        public IEnumerable<Domain.Referentiel.Utilisateur> GetUtilisateurs()
        {
            return this.GetIntervenantsThroughCache();
        }
        private IEnumerable<Domain.Referentiel.Utilisateur> GetIntervenantsThroughCache()
        {
            return this.cache.Get(() => utilRepo.GetAll().Select(x => new Domain.Referentiel.Utilisateur
            {
                Code = x.Utiut,
                Login = x.Utpfx,
                Nom = x.Utnom,
                Prenom = x.Utpnm,
                Branche = GetValue<Branche>(x.Utbra),
                IsSouscripteur = x.Utsou.IsSameAs("O"),
                IsGestionnaire = x.Utges.IsSameAs("O")
            }));
        }


        public Cible GetCible(string cible)
        {
            return this.GetCiblesThroughCache().FirstOrDefault(x => x.Code.IsSameAs(cible));
        }
        public IEnumerable<Cible> GetCibles()
        {
            return this.GetCiblesThroughCache();
        }

        public Categorie GetCategory(string branche, string sousBranche, string categorie)
        {
            return GetAllCategories().FirstOrDefault(x => x.BrancheCode.IsSameAs(branche) && x.SousBrancheCode.IsSameAs(sousBranche) && x.CategoryCode.IsSameAs(categorie));
        }

        public IEnumerable<RefValue> GetValues(string concept, string famille) {
            var ypr = this.yparRepo.GetFamille(concept, famille);
            var pair = codes.FirstOrDefault(x => x.Value.Concept == concept && x.Value.Famille == famille);
            return ypr.Select(y => {
                var value = Activator.CreateInstance(pair.Key) as RefValue;
                value.Code = y.Tcod;
                value.Libelle = y.Tplib;
                value.CodeFiltre = y.Tfilt;
                value.LibelleLong = y.Tplil;
                return value;
            }).ToList();
        }

        private IEnumerable<Cible> GetCiblesThroughCache()
        {
            return this.cache.Get(() =>
            {
                var allCibles = cibleRepo.GetAll();
                return allCibles.Select(x => new Cible { Code = x.Kahcible, ID = x.Kahid, Description = x.Kahdesc });
            });
        }

        private T GetValue<T>(string concept, string famille, string code) where T : RefValue, new()
        {
            var t = yparRepo.Get(concept, famille, code);
            var res = new T() { Code = code, Libelle = t.Tplib };
            return res;
        }

        private IEnumerable<T> GetValues<T>(string concept, string famille) where T : RefValue, new()
        {
            if (typeof(RefParamValue).IsAssignableFrom(typeof(T)))
            {
                //return GetParamValues<T>(concept,famille);
                return (IEnumerable<T>)this.GetType().GetMethod("GetParamValues", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(typeof(T)).Invoke(this, new[] { concept, famille });
            }
            var t = this.yparRepo.GetFamille(concept, famille).Select(x => new T()
            {
                Code = x.Tcod,
                Libelle = x.Tplib,
                CodeFiltre = x.Tfilt,
                Filtre = GetFiltre(x.Tfilt),
                LibelleLong = x.Tplil
            }).ToList();
            return t;
        }

        private IEnumerable GetValues(Type t, string concept, string famille)
        {
            if (t.IsAssignableFrom(typeof(RefParamValue)))
            {
                return
                       (IEnumerable)(this.GetType().GetMethod("GetParamValues", BindingFlags.NonPublic | BindingFlags.Instance)).MakeGenericMethod(new Type[] { t }).Invoke(this, new[] { concept, famille });
            }
            return
                   (IEnumerable)(this.GetType().GetMethod("GetValues", BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder, new Type[] { typeof(string), typeof(string) }, null)).MakeGenericMethod(new Type[] { t }).Invoke(this, new[] { concept, famille });
        }


        private IEnumerable<T> GetParamValues<T>(string concept, string famille) where T : RefParamValue, new()
        {
            var t = yparRepo.GetFamille(concept, famille).Select(x => new T()
            {
                Code = x.Tcod,
                Libelle = x.Tplib,
                CodeFiltre = x.Tfilt,
                Filtre = GetFiltre(x.Tfilt),
                LibelleLong = x.Tplil,
                ParamNum1 = x.Tpcn1,
                ParamNum2 = x.Tpcn2,
                ParamText1 = x.Tpca1,
                ParamText2 = x.Tpca2
            }).ToList();
            return t;
        }

        private IEnumerable<Categorie> GetAllCategories()
        {
            return this.cache.Get(() => ycatRepo.GetAll().Select(x => new Categorie
            {
                BrancheCode = x.Cabra,
                SousBrancheCode = x.Casbr,
                CategoryCode = x.Cacat,
                MontantFraisAccessoires = x.Caafr,
                ATaxeAttentant = x.Caatt == "O",
                TauxPrimeCatastropheNaturelle = x.Cacnp,
                CodeIndice = x.Caind,
                AIndexation = x.Caina == "O",
                AIndexationCapitaux = x.Caixc == "O",
                AIndexatioFranchise = x.Caixf == "O",
                AIndexationPrime = x.Caixp == "O",
                AIndexationLci = x.Caixl == "O",
                TauxComCatastropheNaturelle = x.Cacnc

            }).ToArray());
        }

        private T GetThroughCache<T>(string concept, string famille, string code) where T : RefValue, new()
        {
            Dictionary<string, T> dic = GetCachedFamilly<T>(concept, famille);
            dic.TryGetValue(code, out var returnValue);
            return returnValue;
        }
        private IEnumerable<T> GetValuesThroughCache<T>(string concept, string famille) where T : RefValue, new()
        {
            IEnumerable<T> dic = GetCachedFamilly<T>(concept, famille).Values;
            return dic;
        }

        private Dictionary<string, T> GetCachedFamilly<T>(string concept, string famille) where T : RefValue, new()
        {
            return this.cache.Get(new { concept, famille }, (arr) => $"{arr.concept}/{arr.famille}", (arr) => this.GetValues(typeof(T), arr.concept, arr.famille).Cast<T>().ToDictionary(x => x.Code, x => x, StringComparer.InvariantCultureIgnoreCase));
        }

        public T GetValue<T>(string code) where T : RefValue, new()
        {
            var t = codes[typeof(T)];
            var res = this.GetThroughCache<T>(t.Concept, t.Famille, code);
            if (res == null)
            {
                /// Value not found => default value
                res = new T() { Code = code };
            }
            return res;
        }
        public IEnumerable<T> GetValues<T>() where T : RefValue, new()
        {
            if (codes.TryGetValue(typeof(T), out var t))
            {
                return this.GetValuesThroughCache<T>(t.Concept, t.Famille);
            }
            return Enumerable.Empty<T>();
        }

        public CibleCatego GetCibleCatego(string cible, string branche)
        {
            return this.GetCibleCategos().FirstOrDefault(x => x.Cible.Code.IsSameAs(cible) && x.Branche.Code.IsSameAs(branche));
        }

        public IEnumerable<CibleCatego> GetCibleCategos()
        {
            return this.cache.Get(() =>
            {
                var allCiblefs = ciblefRepo.GetAll();
                return allCiblefs.Select(x => new CibleCatego { Id = x.Kaiid, Branche = this.GetValue<Branche>(x.Kaibra), Categorie = this.GetCategory(x.Kaibra, x.Kaisbr, x.Kaicat), Cible = this.GetCible(x.Kaicible), SousBranche = x.Kaisbr }).ToArray();
            });
        }

        private IDictionary<K, T> GetCachedDict<K, T>(Func<IEnumerable<T>> getter, Func<T, K> keySelector) where T : new()
        {
            return this.cache.Get(getter).ToDictionary(keySelector);
        }

        public Filtre GetFiltre(long id)
        {
            return GetFiltres().GetValueOrDefault(id);
        }
        public Filtre GetFiltre(string name)
        {
            return GetFiltres().Select(x => x.Value).FirstOrDefault(x => x.Code == name);
        }

        private IDictionary<long, Filtre> GetFiltres()
        {
            return this.GetCachedDict(() =>
                    this.kFiltreRepository.GetAll().GroupJoin(
                        this.kFiltrLRepository.GetAll().ToList(),
                        y => y.Kggid,
                        x => x.Kghkggid,
                        (x, y) => Tuple.Create(x, y)
                    ).Select(x => MapFiltre(x)).ToList()
                , x => x.Id);
        }

        private Filtre MapFiltre(Tuple<KFiltre, IEnumerable<KFiltrL>> x)
        {
            var filtre = x.Item1;
            var lines = x.Item2;

            return new Filtre()
            {
                Code = filtre.Kggfilt,
                Id = filtre.Kggid,
                Description = filtre.Kggdesc,
                Lines = lines.Select(y => new LigneFiltre
                {
                    IdCible = y.Kghid,
                    Cible = y.Kghcible,
                    Branche = y.Kghbra,
                    Ordre = y.Kghordr,
                    IsExclusion = y.Kghactf == "E",
                    IsInclusion = y.Kghactf == "I",

                }).ToList()
            };
        }

    }
}
