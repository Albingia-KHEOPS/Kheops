using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formule;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Parametrage.Inventaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Albingia.Kheops.OP.Application.Infrastructure.Extension.Tools;
using TruthTable = System.Collections.Generic.IDictionary<(Albingia.Kheops.OP.Domain.Model.CaractereSelection caractere, Albingia.Kheops.OP.Domain.Referentiel.NatureValue nature), Albingia.Kheops.OP.Domain.Parametrage.Formule.NatureSelection>;

namespace Albingia.Kheops.OP.DataAdapter {
    public class ParameterRepository : IParamRepository, IParamInventaireRepository
    {
        private readonly IReferentialRepository refRepo;
        private readonly IFiltreRepository filterRepo;
        private readonly IGenericCache cache;
        private readonly KVoletRepository kVoletRepo;
        private readonly KcatvoletRepository kCatVoletRepo;
        private readonly KblocRepository kblocRepo;
        private readonly KcatblocRepository kcatblocRepo;
        private readonly KcatmodeleRepository kcatmodeleRepo;
        private readonly YPlmgaRepository yPlmgaRepo;
        private readonly YpltGaaRepository ypltGaaRepo;
        private readonly YpltGarRepository ypltGarRepo;
        private readonly YpltGalRepository ypltGalRepo;
        private readonly KGaranRepository kGaranRepo;
        private readonly KganparRepository kGanparRepo;
        private readonly KExpLciRepository kExpLciRepo;
        private readonly KExpLciDRepository kExpLciDRepo;
        private readonly KExpFrhRepository kExpFrhRepo;
        private readonly KExpFrhDRepository kExpFrhDRepo;
        private readonly KDesiRepository kDesiRepo;
        private readonly KInvTypRepository kInvTypRepository;
        private readonly KblorelRepository kblorelRepo;

        public ParameterRepository(
            KVoletRepository kVoletRepo,
            KcatvoletRepository kCatVoletRepo,
            KblocRepository kblocRepo,
            KcatblocRepository kcatblocRepo,
            KcatmodeleRepository kcatmodeleRepo,
            YPlmgaRepository yPlmgaRepo,
            YpltGaaRepository ypltGaaRepo,
            YpltGarRepository ypltGarRepo,
            YpltGalRepository ypltGalRepo,
            KGaranRepository kGaranRepo,
            KganparRepository kGanparRepo,
            KExpLciRepository kExpLciRepo,
            KExpLciDRepository kExpLciDRepo,
            KExpFrhRepository kExpFrhRepo,
            KExpFrhDRepository kExpFrhDRepo,
            KDesiRepository kDesiRepo,
            KInvTypRepository kInvTypRepository,
            KblorelRepository kblorelRepo,
            IReferentialRepository refRepo,
            IFiltreRepository filterRepo,
            IGenericCache cache)
        {
            this.kVoletRepo = kVoletRepo;
            this.kCatVoletRepo = kCatVoletRepo;
            this.kblocRepo = kblocRepo;
            this.kcatblocRepo = kcatblocRepo;
            this.kcatmodeleRepo = kcatmodeleRepo;
            this.yPlmgaRepo = yPlmgaRepo;
            this.ypltGaaRepo = ypltGaaRepo;
            this.ypltGarRepo = ypltGarRepo;
            this.ypltGalRepo = ypltGalRepo;
            this.kGaranRepo = kGaranRepo;
            this.kGanparRepo = kGanparRepo;
            this.kExpLciRepo = kExpLciRepo;
            this.kExpLciDRepo = kExpLciDRepo;
            this.kExpFrhRepo = kExpFrhRepo;
            this.kExpFrhDRepo = kExpFrhDRepo;
            this.kDesiRepo = kDesiRepo;
            this.kInvTypRepository = kInvTypRepository;
            this.kblorelRepo = kblorelRepo;
            this.filterRepo = filterRepo;
            this.refRepo = refRepo;
            this.cache = cache;
        }

        public TruthTable GetParamNatures() => this.GetCached(() => GetParamNaturesInternal());
        private TruthTable GetParamNaturesInternal()
        {
            var kganpars = kGanparRepo.GetAll();
            var ret = kganpars.Select(x => new {
                k = (caractere: x.Kaucar.AsEnum<CaractereSelection>(), nature: x.Kaunat.AsEnum<NatureValue>()),
                v = new NatureSelection {
                    Checked = x.Kauganc.AsEnum<NatureValue>(),
                    Unchecked = x.Kaugannc.AsEnum<NatureValue>(),
                    IsAffiche = x.Kauaffi.AsBool(),
                    IsModifiable = x.Kaumodi.AsBool()
                }
            }).ToDictionary(x => x.k, x => x.v);
            return ret;
        }

        private IDictionary<string,ParamGarantie> GetGaranties() {
            var kGarans = GetCachedDict(kGaranRepo.GetAll, x => x.Gagar);
            return kGarans.Select( kv => MapGarantie(kv.Value)).ToDictionary(x=>x.CodeGarantie);
        }

        public void InitCache() {
            GetDesignations();
            GetTypeInventaires();
            GetExpressionFranchises();
            GetExpressionLCIs();
            GetParamVolets();
        }

        public void ResetCache() {
            const string globalKey = "/ALL/";
            this.cache.Invalidate<KDesi>(globalKey);
            this.cache.Invalidate<TypeInventaire>(globalKey);
            this.cache.Invalidate<ParamExpressionComplexeFranchise>(globalKey);
            this.cache.Invalidate<ParamExpressionComplexeLCI>(globalKey);
            this.cache.Invalidate<ParamVolet>(globalKey);
            InitCache();
        }

        public ParamGarantie GetGarantie( string code)
        {
            var kGarans = GetCached(GetGaranties);
            return kGarans.GetValueOrDefault(code);
        }

        public IEnumerable<ParamVolet> GetParamVolets() => this.GetCached(() => GetParamVoletsInternal().ToList());
        public ParamVolet GetParamVolet(long catVoletId) => this.GetCached(() => GetParamVoletsInternal().ToList()).FirstOrDefault(x => x.CatVoletId == catVoletId);
        public IEnumerable<ParamVolet> GetParamVoletsInternal()
        {
            var sw = new Stopwatch();
            sw.Start();
            var kVolets = GetCachedDict(kVoletRepo.GetAll, x => x.Kakid);

            var kCatVolets = GetCachedDict(kCatVoletRepo.GetAll, x => x.Kapid);

            var kBlocs = GetCachedDict(kblocRepo.GetAll, x => x.Kaeid);

            var kCatBlocs = GetCachedDict(kcatblocRepo.GetAll, x => x.Kaqid);
            var kBlocByVolet = kCatBlocs.ToLookup(x => x.Value.Kaqkapid, y => y.Value);

            var kCatModels = GetCachedDict(kcatmodeleRepo.GetAll, x => x.Karid);
            var modByBloc = kCatModels.ToLookup(x => x.Value.Karkaqid, y => y.Value);

            var yplmgas = GetCachedDict(yPlmgaRepo.GetAll, x => x.D1mga);

            var ypltgars = GetCachedDict(ypltGarRepo.GetAll, x => x.C2seq);
            var ygarByMod = ypltgars.Values.ToLookup(x => x.C2mga);
            var ygarByParent = ypltgars.Values.ToLookup(x => x.C2sem);

            var ypltgals = GetCachedDict(ypltGalRepo.GetAll, x => Tuple.Create(x.C4seq, x.C4typ));
            var galsByGar = ypltgals.ToLookup(x => x.Key.Item1, x => x.Value);

            var kGarans = GetCachedDict(kGaranRepo.GetAll, x => x.Gagar);
            sw.Stop();
            Trace.WriteLine($"Volets Data time : {sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency:.00}");
            sw.Restart();

            foreach (var catvol in kCatVolets.Values) {
                var vol = kVolets.GetValueOrDefault(catvol.Kapkakid);
                if (vol == null) { continue; }
                var cible = refRepo.GetCibleCatego(catvol.Kapcible, catvol.Kapbra);

                ParamVolet volet = new ParamVolet() {
                    Branche = refRepo.GetValue<Branche>(catvol.Kapbra),
                    Caractere = catvol.Kapcar.AsEnum<CaractereSelection>(),
                    CatVoletId = catvol.Kapid,
                    VoletId = vol.Kakid,
                    IsVoletCollapsed = vol.Kakpres.AsBool("RP"),
                    Cible = cible,
                    Code = vol.Kakvolet,
                    Description = vol.Kakdesc,
                    IsFormuleGenerale = vol.Kakfgen.AsBool(),
                    Ordre = (int)catvol.Kapordre

                };
                volet.Blocs = kBlocByVolet[catvol.Kapid].Select(catBloc => {
                    var bloc = kBlocs[catBloc.Kaqkaeid];
                    var pBloc = new ParamBloc {
                        BlocId = bloc.Kaeid,
                        CatBlocId = catBloc.Kaqid,
                        Code = catBloc.Kaqbloc,
                        Ordre = catBloc.Kaqordre,
                        Description = bloc.Kaedesc,
                        Caractere = catBloc.Kaqcar.AsEnum<CaractereSelection>()

                    };
                    pBloc.Modeles = modByBloc[(int)catBloc.Kaqid].Select(mod => {
                        var mga = yplmgas.GetValueOrDefault(mod.Karmodele);
                        var pMod = new ParamModeleGarantie {
                            CatId = mod.Karid,
                            Code = mod.Karmodele,
                            Description = mga?.D1lib,
                            Typo = mod.Kartypo.AsEnum<TypologieModele>(),
                            DateApplication = MakeDateTime(mod.Kardateapp)
                        };
                        var modNum = mod.Karmodele;
                        pMod.Garanties = MapGarantieHierarchies(ygarByMod[modNum], ygarByParent, galsByGar, kGarans);
                        return pMod;
                    }).ToList();
                    return pBloc;
                }).ToList();
                yield return volet;
            }
            sw.Stop();
            Trace.WriteLine($"Volets Map time : {sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency:.00}");

        }

        private List<ParamGarantieHierarchie> MapGarantieHierarchies(IEnumerable<YpltGar> ygarByMod, ILookup<long, YpltGar> ygarByParent, ILookup<long, YpltGal> galsByGar, IDictionary<string, KGaran> kGarans)
        {
            return ygarByMod.Where(x => x.C2se1 == 0 && x.C2niv == 1).Select(gar => {
                return MapGarantieHierarchie(ygarByParent, galsByGar, kGarans, gar);
            }).ToList();
        }

        private ParamGarantieHierarchie MapGarantieHierarchie(ILookup<long, YpltGar> ygarByParent, ILookup<long, YpltGal> galsByGar, IDictionary<string, KGaran> kGarans, YpltGar gar)
        {

            var res = GetCached(gar.C2seq, (c) => {
                var resgar = new ParamGarantieHierarchie {
                    Caractere = gar.C2car.AsEnum<CaractereSelection>(),
                    //Desc
                    Script = gar.C2scr,
                    Sequence = gar.C2seq,
                    Ordre = gar.C2ord,
                    Tri = gar.C2tri,
                    Niveau = gar.C2niv,
                    IsAlimMontantReference = gar.C2mrf.AsNullableBool(),
                    IsApplicationCATNAT = gar.C2cna.AsNullableBool(),
                    Taxe = refRepo.GetValue<Taxe>(gar.C2tax),
                    IsIndexee = gar.C2ina.AsNullableBool(),
                    IsNatModifiable = gar.C2ntm.AsNullableBool(),
                    Nature = gar.C2nat.AsEnum<NatureValue>(),
                    TypeControleDate = gar.C2tcd.AsEnum<TypeControleDateEnum>()
                };
                resgar.ValeurDeGaranties = galsByGar[gar.C2seq].Select(x => {
                    return MapValeurDeGarantie(x);
                }).ToDictionary(x => x.Type);

                var pGar = kGarans.GetValueOrDefault(gar.C2gar);
                if (pGar != null) {
                    ParamGarantie pg = MapGarantie(pGar);
                    resgar.ParamGarantie = pg;
                }
                resgar.GarantiesChildren = ygarByParent[gar.C2seq].Select(x => MapGarantieHierarchie(ygarByParent, galsByGar, kGarans, x)).ToList();
                return resgar;
            });
            return res;
        }

        private ParamGarantie MapGarantie(KGaran pGar)
        {
            return new ParamGarantie {
                AbregeGarantie = pGar.Gadea,
                CaractereGarantie = pGar.Gacar.AsEnum<CaractereSelection>(),
                CodeGarantie = pGar.Gagar,
                CodeTaxeCatNat = refRepo.GetValue<Taxe>(pGar.Gacnx),
                Taxe = refRepo.GetValue<Taxe>(pGar.Gatax),
                DefinitionGarantie = refRepo.GetValue<DefinitionGarantie>(pGar.Gadfg),
                DesignationGarantie = pGar.Gades,
                FamilleGarantie = refRepo.GetValue<FamilleGarantie>(pGar.Gafam),
                Infocomplementaire = pGar.Gaifc,
                IsGarantCommune = pGar.Gacom.AsNullableBool(),
                IsInventPossible = pGar.Gainv.AsNullableBool(),
                IsRegularisable = pGar.Garge.AsNullableBool(),
                IsAttentatGareat = pGar.Gaatg.AsBool(),
                GrilleRegul = refRepo.GetValue<TypeGrilleRegul>(pGar.Gatrg),
                TypeInventaire = string.IsNullOrWhiteSpace(pGar.Gatyi)
                    ? null
                    : this.GetTypeInventaires().Values.FirstOrDefault(x => x.CodeInventaire == pGar.Gatyi)
            };
        }

        private ValeurDeGarantie MapValeurDeGarantie(YpltGal x)
        {
            TypeDeValeur typeDeValeur = (TypeDeValeur)int.Parse(x.C4typ);
            var resVal = new ValeurDeGarantie {
                Modifiable = x.C4maj.AsBool(),
                Obligatoire = x.C4obl.AsBool(),
                TypeAlimentation = x.C4ala.AsEnum<AlimentationValue>(),
                Type = typeDeValeur,
                Unite = refRepo.GetUnite(typeDeValeur, x.C4unt == "UM" ? "D" : x.C4unt),
                Valeur = x.C4val
            };
            resVal.CoseBase = refRepo.GetBase(typeDeValeur, x.C4bas);
            resVal.Expression = this.GetExpression(x.C4bas, resVal.Type);
            return resVal;
        }


        private ParamExpressionComplexeBase GetExpression(string idExpr, TypeDeValeur type)
        {
            switch (type) {
                case TypeDeValeur.LCI:
                    return GetExpressionLCI(idExpr);
                case TypeDeValeur.Franchise:
                    return GetExpressionFranchise(idExpr);
                case TypeDeValeur.FranchiseMin:
                case TypeDeValeur.FranchiseMax:
                case TypeDeValeur.Assiette:
                case TypeDeValeur.Prime:
                default:
                    return default(ParamExpressionComplexeBase);
            }
        }

        private IDictionary<string, ParamExpressionComplexeLCI> GetExprLciDict()
        {
            var detLci = this.GetCachedLookup(() => {
                return this.kExpLciDRepo.GetAll().Select(x => {
                    var ret = new ParamExpressionComplexeDetailLCI {
                        Id = x.Khhid,
                        ExprId = x.Khhkhgid,
                        CodeBase = refRepo.GetValue<BaseLCI>(x.Khhlcbase),
                        ConcurrentCodeBase = refRepo.GetValue<BaseLCI>(x.Khhlobase),
                        ConcurrentUnite = refRepo.GetValue<UniteLCI>(x.Khhlovau),
                        Unite = refRepo.GetValue<UniteLCI>(x.Khhlcvau),
                        ConcurrentValeur = x.Khhloval.AsDecimal(),
                        Valeur = x.Khhlcval.AsDecimal(),
                        Ordre = x.Khhordre,
                    };
                    return ret;
                });
            }, x => x.ExprId);


            var expr = this.GetCachedDict(() => this.kExpLciRepo.GetAll().Select(x => {
                var ret = new ParamExpressionComplexeLCI {
                    Code = x.Khglce,
                    Id = x.Khgid,
                    Description = x.Khgdesc,
                    Designation = GetDesignation(x.Khgdesi),
                    DesiId = x.Khgdesi,
                    IsModifiable = x.Khgmodi.AsBool(),
                };
                ret.Details = detLci[x.Khgid].Cast<ParamExpressionComplexeDetailBase>().ToList();
                return ret;
            }), x => x.Code);
            return expr;
        }

        public ParamExpressionComplexeLCI GetExpressionLCI(long id) => this.GetCachedDict(GetExpressionLCIs, x => x.Id).GetValueOrDefault(id);
        public ParamExpressionComplexeLCI GetExpressionLCI(string id) => GetExprLciDict().GetValueOrDefault(id);
        public ParamExpressionComplexeLCI GetExpressionLCIByBase(string @base) {
            return this.kExpLciRepo.GetByBase(@base).Select(x =>
            {
                var ret = new ParamExpressionComplexeLCI
                {
                    Id = x.Khgid,
                    Code = x.Khglce,
                    Description = x.Khgdesc,
                    DesiId = x.Khgdesi,
                    IsModifiable = x.Khgmodi.AsBool(),
                    Designation = GetDesignation(x.Khgdesi),
                    Details = this.kExpLciDRepo.GetByExpLci(x.Khgid).Select(y => new ParamExpressionComplexeDetailLCI
                    {
                        Id = y.Khhid,
                        Ordre = y.Khhordre,
                        Valeur = y.Khhlcval,
                        Unite = new Unite { Code = y.Khhlcvau },
                        CodeBase = new BaseDeCalcul { Code = y.Khhlcbase },
                        ConcurrentValeur = y.Khhloval,
                        ConcurrentUnite = new Unite { Code = y.Khhlovau },
                        ConcurrentCodeBase = new BaseLCI { Code = y.Khhlobase }
                    }).Cast<ParamExpressionComplexeDetailBase>().ToList()
                };
                return ret;
            }).FirstOrDefault();
        }
        public IEnumerable<ParamExpressionComplexeLCI> GetExpressionLCIs() => GetExprLciDict().Values;
        public string GetDesignation(long id)
        {
            IDictionary<int, KDesi> cache = this.GetCachedDict(this.kDesiRepo.GetAll, x => x.Kdwid);
            KDesi value = cache.GetValueOrDefault((int)id);
            return value?.Kdwdesi;
        }
        public IDictionary<long, string> GetDesignations() => this.GetCachedDict(this.kDesiRepo.GetAll, x => x.Kdwid).ToDictionary(x => (long)x.Key, x => x.Value.Kdwdesi);

        public ParamExpressionComplexeFranchise GetExpressionFranchise(long id) => this.GetCachedDict(GetExpressionFranchises, x => x.Id).GetValueOrDefault(id);
        public ParamExpressionComplexeFranchise GetExpressionFranchise(string id) => GetExprFrhDict().GetValueOrDefault(id);
        public ParamExpressionComplexeFranchise GetExpressionFranchiseByBase(string @base)
        {
            return this.kExpFrhRepo.GetByBase(@base).Select(x =>
            {
                var ret = new ParamExpressionComplexeFranchise
                {
                    Id = x.Kheid,
                    Code = x.Khefhe,
                    Description = x.Khedesc,
                    DesiId = x.Khedesi,
                    IsModifiable = x.Khemodi.AsBool(),
                    Designation = GetDesignation(x.Khedesi),
                    Details = this.kExpFrhDRepo.GetByExpFranchise(x.Kheid).Select(y => new ParamExpressionComplexeDetailFranchise
                    {
                        Id = y.Khfid,
                        Ordre = y.Khfordre,
                        Valeur = y.Khffhval,
                        Unite = new Unite { Code = y.Khffhvau },
                        CodeBase = new BaseDeCalcul { Code = y.Khfbase },
                        ValeurMax = y.Khffhmaxi,
                        ValeurMin = y.Khffhmini,
                        CodeIncide = new Indice { Code = y.Khfind },
                        IncideValeur = y.Khfivo,
                        LimiteDebut = MakeNullableDateTime(y.Khflimdeb),
                        LimiteFin = MakeNullableDateTime(y.Khflimfin)
                    }).Cast<ParamExpressionComplexeDetailBase>().ToList()
                };
                return ret;
            }).FirstOrDefault();
        }

        public IEnumerable<ParamExpressionComplexeFranchise> GetExpressionFranchises() => GetExprFrhDict().Values;

        private IDictionary<string, ParamExpressionComplexeFranchise> GetExprFrhDict()
        {
            ILookup<long, ParamExpressionComplexeDetailFranchise> detfrh = NewMethod();

            var expr = this.GetCachedDict(() => this.kExpFrhRepo.GetAll().Select(x => {
                return MapFrh(x, detfrh);
            }), x => x.Code);
            return expr;
        }

        private ILookup<long, ParamExpressionComplexeDetailFranchise> NewMethod()
        {
            return this.GetCachedLookup(() => {
                return this.kExpFrhDRepo.GetAll().Select(MapFrhd);
            }, x => x.ExprId);
        }

        private ParamExpressionComplexeFranchise MapFrh(KExpFrh x, ILookup<long, ParamExpressionComplexeDetailFranchise> detailsLookup)
        {
            var ret = new ParamExpressionComplexeFranchise {
                Code = x.Khefhe,
                Id = x.Kheid,
                Description = x.Khedesc,
                Designation = GetDesignation(x.Khedesi),
                DesiId = x.Khedesi,
                IsModifiable = x.Khemodi.AsBool()
            };
            ret.Details = detailsLookup[x.Kheid].Cast<ParamExpressionComplexeDetailBase>().ToList();
            return ret;
        }

        private ParamExpressionComplexeDetailFranchise MapFrhd(KExpFrhD x)
        {
            return new ParamExpressionComplexeDetailFranchise {
                Id = x.Khfid,
                ExprId = x.Khfkheid,
                Valeur = x.Khffhval.AsDecimal(),
                Ordre = x.Khfordre,
                CodeBase = refRepo.GetValue<BaseFranchise>(x.Khfbase),
                CodeIncide = refRepo.GetValue<Indice>(x.Khfind),
                IncideValeur = x.Khfivo.AsDecimal(),
                LimiteDebut = MakeNullableDateTime(x.Khflimdeb),
                LimiteFin = MakeNullableDateTime(x.Khflimfin),
                Unite = refRepo.GetValue<UniteFranchise>(x.Khffhvau),
                ValeurMax = x.Khffhmaxi.AsDecimal(),
                ValeurMin = x.Khffhmini.AsDecimal()
            };
        }

        private T GetCached<T>(Func<T> getter)
        {
            return this.cache.Get(getter);
        }
        private T GetCached<T, U>(U key, Func<U, T> getter)
        {
            return this.cache.Get(key, x => x.ToString(), getter);
        }
        private IDictionary<K, T> GetCachedDict<K, T>(Func<IEnumerable<T>> getter, Func<T, K> keySelector)
        {
            return this.cache.Get(()=>  getter().ToDictionary(keySelector) );
        }
        private ILookup<K, T> GetCachedLookup<K, T>(Func<IEnumerable<T>> getter, Func<T, K> keySelector)
        {
            return this.cache.Get(() => getter().ToLookup(keySelector));
        }
        private IEnumerable<ParamGarantieHierarchie> FlattenGaranties(IEnumerable<ParamGarantieHierarchie> garanties)
        {
            return FlattenGaranties(garanties, Enumerable.Empty<ParamGarantieHierarchie>());
        }
        private IEnumerable<ParamGarantieHierarchie> FlattenGaranties(IEnumerable<ParamGarantieHierarchie> garanties, IEnumerable<ParamGarantieHierarchie> flatList)
        {
            if (!garanties.Any()) {
                return flatList;
            }
            return FlattenGaranties(garanties.SelectMany(x => x.GarantiesChildren), flatList.Union(garanties));
        }

        public ParamGarantieHierarchie GetParamHierarchie(long seq)
        {
            var dict = this.GetCachedDict(() => FlattenGaranties(this.GetParamVolets().SelectMany(x => x.Blocs).SelectMany(x => x.Modeles).SelectMany(x => x.Garanties)).Distinct(), x => x.Sequence);
            return dict.GetValueOrDefault(seq);
        }

        public ParamModeleGarantie GetParamModele(int id)
        {
            return this.GetCachedDict(() => this.GetParamVolets().SelectMany(x => x.Blocs).SelectMany(x => x.Modeles).Distinct(), x => x.CatId).GetValueOrDefault(id);
        }

        public TypeInventaire GetTypeInventaire(long id)
        {
            return GetTypeInventaires().GetValueOrDefault(id);
        }
        public IDictionary<long, TypeInventaire> GetTypeInventaires()
        {
            return this.GetCachedDict(() => this.kInvTypRepository.GetAll().Select(x => MapTypInv(x)), x => x.Id);
        }

        public ILookup<long, GarantieRelation> GetRelationGaranties()
        {
            return this.GetCached(() => {
                var t = this.ypltGaaRepo.GetAll().Select(x => new GarantieRelation { EsclaveId = x.C5seq, MaitreId = x.C5sem, Relation = x.C5typ.AsEnum<TypeRelation>() });
                // exclude when master equals slave
                t = t.Where(x => x.EsclaveId != x.MaitreId);
                return t.Select(x => (x.EsclaveId, x))
                    .Union(t.Select(x => (x.MaitreId, x)))
                    .ToLookup(x => x.Item1, y => y.x);
            });
        }
        public ILookup<long, BlocRelation> GetRelationBlocs()
        {
            return this.GetCached(() => {
                var t = this.kblorelRepo.GetAll().Select(x => new BlocRelation { EsclaveId = x.Kgjidblo1, MaitreId = x.Kgjidblo2, Relation = x.Kgjrel.AsEnum<TypeRelation>() });
                return t.Select(x => (x.EsclaveId, x)).Union(t.Select(x => (x.MaitreId, x))).ToLookup(x => x.Item1, y => y.x);
            });
        }

        public IEnumerable<ParamGarantie> GetAllGaranties() {
            return GetGaranties().Values;
        }

        private TypeInventaire MapTypInv(KInvTyp x)
        {
            return new TypeInventaire {
                Id = x.Kagid,
                Description = x.Kagdesc,
                CodeInventaire = x.Kagtyinv,
                Filtre = filterRepo.GetFiltre(x.Kagkggid),
                TypeItem = (TypeInventaireItem)x.Kagtmap

            };
        }

    }
}
