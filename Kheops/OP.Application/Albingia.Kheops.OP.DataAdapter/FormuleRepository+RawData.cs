using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Inventaire;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formule;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using static Albingia.Kheops.OP.Application.Infrastructure.Extension.Tools;
using TruthTable = System.Collections.Generic.IDictionary<(Albingia.Kheops.OP.Domain.Model.CaractereSelection caractere, Albingia.Kheops.OP.Domain.Referentiel.NatureValue nature), Albingia.Kheops.OP.Domain.Parametrage.Formule.NatureSelection>;
using ALBINGIA.Framework.Common.Extensions;

namespace Albingia.Kheops.OP.DataAdapter
{
    public partial class FormuleRepository
    {
        private struct ParentGarantie
        {
            public long blocId;
            public long parent;
        }

        private class RawDataSingle {
            public KpFor formula = null;
            public IDictionary<long, KpOpt> options = new Dictionary<long, KpOpt>();
            public IDictionary<long, KpOptD> optiondetails = new Dictionary<long, KpOptD>();
            public IDictionary<long, KpGaran> gars = new Dictionary<long, KpGaran>();
            public IDictionary<long, KpGaran> garsNatModifiable = new Dictionary<long, KpGaran>();
            public IDictionary<long, KpGarTar> gartars = new Dictionary<long, KpGarTar>();
            public IDictionary<long, ExpressionComplexeLCI> exprLci { get; internal set; } = new Dictionary<long, ExpressionComplexeLCI>();


            public IDictionary<long, ExpressionComplexeFranchise> exprFrh { get; internal set; } = new Dictionary<long, ExpressionComplexeFranchise>();
            public IDictionary<long, KpGarAp> portees = new Dictionary<long, KpGarAp>();
            public IEnumerable<Inventaire> inventaires;

            public IReferentialRepository refRepo;
            public IParamRepository paramRepo;
            internal ILookup<long, KpOptAp> applications = new KpOptAp[0].ToLookup(x => x.Kddkdbid);
            public IDictionary<long, KpExpLCID> exprDbLciD { get; internal set; } = new Dictionary<long, KpExpLCID>();
            public IDictionary<long, KpExpFrhD> exprDbFrhD { get; internal set; } = new Dictionary<long, KpExpFrhD>();
            public IDictionary<long, KpExpFrh> exprDbFrh { get; internal set; } = new Dictionary<long, KpExpFrh>();
            public IDictionary<long, KpExpLCI> exprDbLci { get; internal set; } = new Dictionary<long, KpExpLCI>();
            public IDictionary<int, string> desis { get; internal set; } = new Dictionary<int, string>();

            

            public RawDataSingle(IReferentialRepository refRepo, IParamRepository paramRepo)
            {
                this.refRepo = refRepo;
                this.paramRepo = paramRepo;

            }
            private RawDataMany ToMany()
            {
                var f = (formula == null) ? Enumerable.Empty<KpFor>() : new KpFor[] { formula };
                var t = new RawDataMany(refRepo, paramRepo) {
                    formulas = f,
                    desis = desis,
                    options = options.Select(x => x.Value).ToLookup(x => x.Kdbkdaid),
                    applications = applications,
                    optiondetails = optiondetails.Select(x => x.Value).ToLookup(x => x.Kdckdbid),
                    gars = gars.Select(x => x.Value).ToLookup(x => x.Kdekdcid),
                    //TODO garsNatModifiable = garsNatModifiable.Values.ToLookup(v => v.Kdeid),
                    gartars = gartars.Select(x => x.Value).ToLookup(x => x.Kdgkdeid),
                    portees = portees.Select(x => x.Value).ToLookup(x => x.Kdfkdeid),
                    exprLci = exprLci,
                    exprFrh = exprFrh,
                    inventaires = inventaires
                };
                return t;
            }
            public Formule ToFormule() => this.ToMany().ToFormules().FirstOrDefault();
        }

        private class RawDataMany
        {
            internal TruthTable paramTruthTable;
            public ILookup<long, GarantieRelation> ParamGarantiesRelations;
            public ILookup<long, BlocRelation> ParamBlocsRelations;

            public IEnumerable<KpFor> formulas;
            public ILookup<long, KpOpt> options;
            public ILookup<long, KpOptAp> applications;
            public ILookup<long, KpOptD> optiondetails;
            private ILookup<long, KpGaran> _gars;
            public ILookup<long, KpGaran> gars { get => _gars; set { this._garseq = null; this._gars = value; } }
            public ILookup<long, KpGarTar> gartars;
            public ILookup<long, KpGaran> garsNatModifiable;
            public IDictionary<long, ExpressionComplexeLCI> exprLci { get; internal set; } = new Dictionary<long, ExpressionComplexeLCI>();
            public IDictionary<long, ExpressionComplexeFranchise> exprFrh { get; internal set; } = new Dictionary<long, ExpressionComplexeFranchise>();
            public IDictionary<string, ExpressionComplexeLCI> exprLciCode;
            public IDictionary<string, ExpressionComplexeFranchise> exprFrhCode;
            public ILookup<long, KpGarAp> portees;
            private ILookup<ParentGarantie, KpGaran> _garseq;
            public ILookup<ParentGarantie, KpGaran> garseq {
                get {
                    if (_garseq == null) {
                        _garseq = gars.SelectMany(x => x).ToLookup(x => new ParentGarantie { blocId = x.Kdekdcid, parent = x.Kdesem });
                    }
                    return _garseq;
                }
            }

            public IDictionary<int, string> desis { get; internal set; }

            public IReferentialRepository refRepo;
            public IParamRepository paramRepo;
            public IEnumerable<Inventaire> inventaires;

            public RawDataMany(IReferentialRepository refRepo, IParamRepository paramRepo)
            {
                this.refRepo = refRepo;
                this.paramRepo = paramRepo;
                this.paramTruthTable = paramRepo.GetParamNatures();
                this.ParamBlocsRelations = paramRepo.GetRelationBlocs();
                this.ParamGarantiesRelations = paramRepo.GetRelationGaranties();

            }
            public IEnumerable<Formule> ToFormules(IDictionary<int, RawDataMany> histoData = null) => this.formulas.Select(x => MapOneFormule(x, histoData)).Where(x => x != null).ToList();
            private Formule MapOneFormule(KpFor f, IDictionary<int, RawDataMany> histoData = null) {
                var formula = new Formule {
                    AffaireId = new AffaireId {
                        CodeAffaire = f.Kdaipb,
                        NumeroAliment = f.Kdaalx,
                        NumeroAvenant = f.Kdaavn,
                        TypeAffaire = f.Kdatyp.ParseCode<AffaireType>(),
                        IsHisto = (f.Kdaavn).HasValue
                    },
                    Id = f.Kdaid,
                    Alpha = f.Kdaalpha,
                    Chrono = f.Kdacch,
                    Cible = refRepo.GetCibleCatego(f.Kdacible, f.Kdabra),
                    Description = f.Kdadesc,
                    FormuleNumber = f.Kdafor,
                };

                formula.Options = options[f.Kdaid].Select(
                    opt
                    => MapOption(formula, opt, histoData)
                ).Where(x => x != null).ToList();

                return formula;
            }

            private Option MapOption(Formule formula, KpOpt opt, IDictionary<int, RawDataMany> histoData = null) {
                var optRes = new Option {
                    Id = opt.Kdbid,
                    NumeroAvenant = opt.Kdbavn,
                    OptionNumber = opt.Kdbopt,
                    NumAvenantCreation = opt.Kdbave,
                    NumAvenantModif = opt.Kdbavg,
                    DateAvenant = MakeNullableDateTime(opt.Kdbava, opt.Kdbavm, opt.Kdbavj),
                    Applications = applications[opt.Kdbid].Select(ap => MapApplication(ap)).ToList(),
                    OptionVolets = optiondetails[opt.Kdbid].Where(d => d.Kdcteng.AsEnum<TypeOption>() == TypeOption.Volet).Select(
                        det =>
                            MapOptionVoletBloc(formula, optiondetails[opt.Kdbid], det, histoData) as OptionVolet
                            ).OrderBy(x => x.Ordre).ToList(),
                    MontantsOption = new MontantsOption {
                        IsMontantAcquis = opt.Kdbpaq.AsNullableBool(),
                        Montantacquis = opt.Kdbacq.AsDecimal(),
                        TotalCalculeRef = opt.Kdbtmc.AsDecimal(),
                        TotalMontantForceRef = opt.Kdbtff.AsDecimal(),
                        TotalCoefcalcul = opt.Kdbtfp.AsDecimal(),
                        IsMontantprovisionnel = opt.Kdbpro.AsNullableBool(),
                        IsMontantForcepourMini = opt.Kdbtmi.AsNullableBool(),
                        MotifTotalforce = opt.Kdbtfm,
                        ComptantMontantCalcule = opt.Kdbcmc.AsDecimal(),
                        IsComptantMontantForce = opt.Kdbcfo.AsNullableBool(),
                        ComptantMontantForceHT = opt.Kdbcht.AsDecimal(),
                        ComptantMontantForceTTC = opt.Kdbctt.AsDecimal(),
                        CoeffCalculForceComptant = opt.Kdbccp.AsDecimal(),
                        ValeurOrigine = opt.Kdbval.AsDecimal(),
                        ValeurActualisee = opt.Kdbvaa.AsDecimal(),
                        Valeurdetravail = opt.Kdbvaw.AsDecimal(),
                        TypeDevaleur = opt.Kdbvat,
                        UniteDeValeur = refRepo.GetValue<UniteCapitaux>(opt.Kdbvau),
                        IsTTC = opt.Kdbvah.AsNullableBool(),
                        ValeurIndiceOrigin = opt.Kdbivo.AsDecimal(),
                        ValeurIndiceActual = opt.Kdbiva.AsDecimal(),
                        ValeurIndiceTravai = opt.Kdbivw.AsDecimal(),

                        ProchaineEChHT = opt.Kdbehh.AsDecimal(),
                        ProchaineEchCatnat = opt.Kdbehc.AsDecimal(),
                        ProchaineEchIncend = opt.Kdbehi.AsDecimal(),
                        AssietteValOrigine = opt.Kdbasvalo.AsDecimal(),
                        AssietteValActual = opt.Kdbasvala.AsDecimal(),
                        AssietteValeurW = opt.Kdbasvalw.AsDecimal(),
                        AssietteUnite = refRepo.GetValue<UniteCapitaux>(opt.Kdbasunit),
                        AssietteBaseTypeValeur = opt.Kdbasbase,
                        MontantRefForcesaisi = opt.Kdbger.AsDecimal(),
                    }
                };
                if (histoData != null && histoData?.Any() == true) {
                    optRes.AvnDatesHistory = histoData
                        .Where(list => list.Value.options?.Any(x => x.Any(o => o.Kdbfor == formula.FormuleNumber)) == true)
                        .ToDictionary(
                            x => x.Key,
                            x => {
                                var o = x.Value.options.SelectMany(g => g.ToList()).First(k => k.Kdbfor == formula.FormuleNumber);
                                return MakeNullableDateTime(o.Kdbava, o.Kdbavm, o.Kdbavj);
                            });
                }
                if (optRes.Applications.Count == 0) {
                    return null;
                }
                return optRes;
            }

            private Domain.Formule.Application MapApplication(KpOptAp ap)
            {
                return new Domain.Formule.Application {
                    Id = ap.Kddid,
                    Niveau = ap.Kddperi.AsEnum<ApplicationNiveau>(),
                    NumObjet = ap.Kddobj,
                    NumRisque = ap.Kddrsq
                };
            }

            private OptionDetail MapOptionVoletBloc(Formule formula,
                IEnumerable<KpOptD> optds,
                KpOptD det,
                IDictionary<int, RawDataMany> histoData = null)
            {
                OptionDetail c;
                TypeOption type = det.Kdcteng.AsEnum<TypeOption>();
                if (type == TypeOption.Volet) {
                    var v = new OptionVolet();
                    c = v;
                    MapBaseOptionDetail(formula, det, c, type);
                    v.Blocs = optds.Where(x => x.Kdckakid == det.Kdckakid && x.Kdcteng.AsEnum<TypeOption>() == TypeOption.Bloc).Select(optd => MapOptionVoletBloc(formula, optds, optd, histoData) as OptionBloc).OrderBy(x => x.Ordre).ToList();
                    c.Ordre = c.Ordre > 0 ? c.Ordre : v.ParamVolet.Ordre;
                }
                else {
                    var b = new OptionBloc();
                    c = b;
                    MapBaseOptionDetail(formula, det, c, type);
                    b.ParamBloc = c.ParamVolet.Blocs.FirstOrDefault(x => x.CatBlocId == det.Kdckaqid);
                    c.Ordre = c.Ordre > 0 ? c.Ordre : b.ParamBloc.Ordre;
                    b.Garanties = gars[det.Kdcid].Where(x => x.Kdesem == 0 || x.Kdeniveau == 1).Select(gar => MapGarantie(gar, histoData)).OrderBy(x => x.Tri).ToList();
                    b.ParamRelations = ParamBlocsRelations[b.ParamBloc.BlocId].ToList();
                }

                return c;
            }

            private void MapBaseOptionDetail(Formule formula,
                KpOptD det,
                OptionDetail c,
                TypeOption type)
            {
                c.Id = det.Kdcid;
                c.NumeroAvenant = det.Kdcavn;
                c.Type = type;
                c.Ordre = det.Kdcordre;
                c.IsChecked = Convert.ToBoolean(det.Kdcflag);
                c.ParamModele = paramRepo.GetParamModele((int)det.Kdckarid);
                c.ParamVolet = paramRepo.GetParamVolets().FirstOrDefault(x => x.VoletId == det.Kdckakid && x.Cible?.Id == formula.Cible.Id);


            }

            private Garantie MapGarantie(KpGaran gar, IDictionary<int, RawDataMany> histoData = null)
            {
                var garant = new Garantie() {
                    //TODO completer les références avec des proxys Lazy loading
                    LienKPSPEC = gar.Kdekdhid,
                    LienKPGARAP = gar.Kdekdfid,
                    LienKPINVEN = gar.Kdeinven,

                    Inventaire = gar.Kdeinven == 0 ? null : this.inventaires.FirstOrDefault(x => x.Id == gar.Kdeinven),

                    Tarif = gartars[gar.Kdeid].Select(x => MapTarif(x)).FirstOrDefault(),
                    Portees = portees[gar.Kdeid].Select(x => MapPortee(x)).ToList(),
                    Caractere = gar.Kdecar.AsEnum<CaractereSelection>(),
                    Assiette = new ValeursOptionsTarif {
                        Base = refRepo.GetValue<BaseCapitaux>(gar.Kdeasbase),
                        ExpressionComplexe = null, // null on purpose
                        IsModifiable = gar.Kdeasmod.AsBool(),
                        IsObligatoire = gar.Kdeasobli.AsNullableBool(),
                        Unite = refRepo.GetValue<UniteCapitaux>(gar.Kdeasunit),
                        ValeurActualise = gar.Kdeasvala.AsDecimal(),
                        ValeurOrigine = gar.Kdeasvalo.AsDecimal(),
                        ValeurTravail = gar.Kdeasvalw.AsDecimal()
                    },
                    Taxe = refRepo.GetValue<Taxe>(gar.Kdetaxcod),
                    DateDebut = MakeNullableDateTime(gar.Kdedatdeb, gar.Kdeheudeb, true),
                    DateFinDeGarantie = MakeNullableDateTime(gar.Kdedatfin, gar.Kdeheufin, true),
                    DefinitionGarantie = refRepo.GetValue<DefinitionGarantie>(gar.Kdedefg),
                    Duree = string.IsNullOrWhiteSpace(gar.Kdeduruni) ? default(int?) : gar.Kdeduree,
                    DureeUnite = refRepo.GetValue<UniteDuree>(gar.Kdeduruni),
                    CodeGarantie = gar.Kdegaran,
                    Id = gar.Kdeid,
                    NumeroAvenant = gar.Kdeavn,
                    NumeroAvenantCreation = gar.Kdecravn,
                    NumeroAvenantModif = gar.Kdemajavn,
                    Formule = gar.Kdefor,
                    InventaireSpecifique = gar.Kdeinvsp.AsNullableBool(),
                    IsAlimMontantReference = gar.Kdealiref.AsNullableBool(),
                    IsApplicationCATNAT = gar.Kdecatnat.AsNullableBool(),
                    IsFlagModifie = gar.Kdemodi.AsNullableBool(),
                    IsGarantieAjoutee = gar.Kdeajout.AsBool(),
                    IsIndexe = gar.Kdeina.AsNullableBool(),
                    IsParameIndex = gar.Kdepind.AsNullableBool(),
                    Nature = gar.Kdenat.AsEnum<NatureValue>(),
                    NatureRetenue = gar.Kdegan.AsEnum<NatureValue>(),
                    NumDePresentation = gar.Kdenumpres.AsDecimal(),
                    ParamCodetaxe = refRepo.GetValue<Taxe>(gar.Kdeptaxc),
                    ParametrageCATNAT = gar.Kdepcatn.AsNullableBool(),
                    ParametrageIsAlimMontantRef = gar.Kdepref.AsBool(),
                    ParamIsNatModifiable = gar.Kdepntm.AsNullableBool(),
                    ParamTypeAlimentation = gar.Kdepala.AsEnum<AlimentationValue>(),
                    RepartitionTaxe = gar.Kdetaxrep.AsDecimal(),
                    Tri = gar.Kdetri,
                    TypeAlimentation = gar.Kdeala.AsEnum<AlimentationValue>(),
                    PeriodeApplication = refRepo.GetValue<PeriodeApplication>(gar.Kdeprp),
                    TypeControleDate = gar.Kdetcd.AsEnum<TypeControleDateEnum>(),
                    TypeEmission = refRepo.GetValue<TypeEmission>(gar.Kdetypemi),
                    SousGaranties = garseq[new ParentGarantie { blocId = gar.Kdekdcid, parent = gar.Kdeseq }].Select(subgar => MapGarantie(subgar, histoData)).OrderBy(g => g.Tri).ToList()
                };
                var pg = this.paramRepo.GetParamHierarchie(gar.Kdeseq);
                if (pg == null) {
                    pg = InitParamGarantieFromDbData(garant, gar);
                }
                garant.ParamGarantie = pg;
                garant.ParamRelations = this.ParamGarantiesRelations[pg.Sequence].ToList();
                if (histoData != null && histoData?.Any() == true) {
                    // get the max key corresponding to the current AVN
                    if (histoData.TryGetValue(histoData.Keys.Max(), out var data) && data?.garsNatModifiable?.Any() == true) {
                        var histo = data.garsNatModifiable.FirstOrDefault(g => g.Key == garant.Id);
                        if (histo?.Any() == true) {
                            garant.OriginalAvnNums = histo.GroupBy(g => g.Kdeavn).ToDictionary(
                                g => g.Key.GetValueOrDefault(),
                                g => {
                                    var kpgar = g.First();
                                    return (kpgar.Kdecravn, kpgar.Kdemajavn);
                                });
                        }
                    }
                }

                return garant;
            }

            private ParamGarantieHierarchie InitParamGarantieFromDbData(Garantie gar, KpGaran kpgar)
            {
                var retval = new ParamGarantieHierarchie() {
                    Caractere = gar.Caractere,
                    GarantiesChildren = new List<ParamGarantieHierarchie>(),
                    IsAlimMontantReference = gar.ParametrageIsAlimMontantRef,
                    IsApplicationCATNAT = gar.ParametrageCATNAT,
                    IsIndexee = gar.IsParameIndex,
                    IsNatModifiable = gar.ParamIsNatModifiable,
                    Nature = gar.Nature,
                    Niveau = kpgar.Kdeniveau,
                    Ordre = 0,
                    ParamGarantie = this.paramRepo.GetGarantie(gar.CodeGarantie) ?? new ParamGarantie() { CodeGarantie = gar.CodeGarantie },
                    Script = "",
                    Sequence = kpgar.Kdeseq,
                    Taxe = gar.Taxe,
                    TypeControleDate = gar.TypeControleDate,
                };
                return retval;
            }

            private PorteeGarantie MapPortee(KpGarAp x)
            {
                return new PorteeGarantie() {
                    Id = x.Kdfid,
                    Action = x.Kdfgan.AsEnum<ActionValue>(),
                    CodeAction = x.Kdfgan,
                    GarantieId = x.Kdfkdeid,
                    Montant = x.Kdfmnt,
                    NumObjet = x.Kdfobj,
                    RisqueId = x.Kdfrsq,
                    TypeCalcul = x.Kdftyc.AsEnum<TypeCalcul>(),
                    ValeursPrime = new ValeursUnite() {
                        Unite = refRepo.GetValue<UnitePrime>(x.Kdfpru),
                        ValeurActualise = x.Kdfpra,
                        ValeurOrigine = x.Kdfprv,
                        ValeurTravail = x.Kdfprw
                    }
                };
            }

            private TarifGarantie MapTarif(DataModel.DTO.KpGarTar tar)
            {
                return new TarifGarantie {
                    PrimeValeur = new ValeursOptionsTarif {
                        Base = refRepo.GetValue<BasePrime>(tar.Kdgpribase),
                        Unite = refRepo.GetValue<UnitePrime>(tar.Kdgpriunit),
                        ValeurActualise = tar.Kdgprivala.AsDecimal(),
                        ValeurOrigine = tar.Kdgprivalo.AsDecimal(),
                        ValeurTravail = tar.Kdgprivalw.AsDecimal(),
                        IsModifiable = tar.Kdgprimod.AsBool(),
                        IsObligatoire = tar.Kdgpriobl.AsNullableBool()
                    },
                    Franchise = new ValeursOptionsTarif {
                        Base = refRepo.GetValue<BaseFranchise>(tar.Kdgfrhbase),
                        Unite = refRepo.GetValue<UniteFranchise>(tar.Kdgfrhunit),
                        ValeurActualise = tar.Kdgfrhvala.AsDecimal(),
                        ValeurOrigine = tar.Kdgfrhvalo.AsDecimal(),
                        ValeurTravail = tar.Kdgfrhvalw.AsDecimal(),
                        IsModifiable = tar.Kdgfrhmod.AsBool(),
                        IsObligatoire = tar.Kdgfrhobl.AsNullableBool(),
                        ExpressionComplexe = ResolveFrh(tar)
                    },
                    FranchiseMini = new ValeursTarif {
                        Base = refRepo.GetValue<BaseFranchise>(tar.Kdgfmibase),
                        Unite = refRepo.GetValue<UniteFranchise>(tar.Kdgfmiunit),
                        ValeurActualise = tar.Kdgfmivala.AsDecimal(),
                        ValeurOrigine = tar.Kdgfmivalo.AsDecimal(),
                        ValeurTravail = tar.Kdgfmivalw.AsDecimal()
                    },
                    FranchiseMax = new ValeursTarif {
                        Base = refRepo.GetValue<BaseFranchise>(tar.Kdgfmabase),
                        Unite = refRepo.GetValue<UniteFranchise>(tar.Kdgfmaunit),
                        ValeurActualise = tar.Kdgfmavala.AsDecimal(),
                        ValeurOrigine = tar.Kdgfmavalo.AsDecimal(),
                        ValeurTravail = tar.Kdgfmavalw.AsDecimal()
                    },
                    LCI = new ValeursOptionsTarif {
                        Base = refRepo.GetValue<BaseLCI>(tar.Kdglcibase),
                        Unite = refRepo.GetValue<UniteLCI>(tar.Kdglciunit),
                        ValeurActualise = tar.Kdglcivala.AsDecimal(),
                        ValeurOrigine = tar.Kdglcivalo.AsDecimal(),
                        ValeurTravail = tar.Kdglcivalw.AsDecimal(),
                        IsModifiable = tar.Kdglcimod.AsBool(),
                        IsObligatoire = tar.Kdglciobl.AsNullableBool(),
                        ExpressionComplexe = ResolveLci(tar)
                    },
                    NumeroTarif = (short)tar.Kdgnumtar,
                    ComptantMontantcalcule = tar.Kdgcmc.AsDecimal(),
                    ComptantMontantForceHT = tar.Kdgcht.AsDecimal(),
                    ComptantMontantForceTTC = tar.Kdgctt.AsDecimal(),
                    PrimeMontantBase = tar.Kdgmntbase.AsDecimal(),
                    PrimeProvisionnelle = tar.Kdgprimpro.AsDecimal(),
                    TotalMontantCalcule = tar.Kdgtmc.AsDecimal(),
                    TotalMontantForce = tar.Kdgtff.AsDecimal(),
                    Id = tar.Kdgid,
                    NumeroAvenant = tar.Kdgavn
                };
            }

            private ExpressionComplexeFranchise ResolveFrh(KpGarTar tar) => 
                tar.Kdgkdkid == 0 
                ? null 
                : (
                    exprFrh.GetValueOrDefault(tar.Kdgkdkid) 
                    ??(
                        tar.Kdglciunit == "CPX" 
                        ? exprFrh.FirstOrDefault(x=>x.Value.Code == tar.Kdglcibase).Value
                        : null 
                    )
                );
            private ExpressionComplexeLCI ResolveLci(KpGarTar tar) =>
                tar.Kdgkdiid == 0 
                ? null 
                : (
                    exprLci .GetValueOrDefault(tar.Kdgkdiid) 
                    ??(
                        tar.Kdgfrhunit == "CPX" 
                        ? exprLci.FirstOrDefault(x=>x.Value.Code == tar.Kdgfrhbase).Value
                        : null 
                    )
                );
        }
    }
}
