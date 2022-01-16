using static Albingia.Kheops.OP.Application.Infrastructure.Tools;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Infrastructure.Extensions;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;

namespace Albingia.Kheops.OP.DataAdapter
{
    public class FormuleHistoRepository : IFormuleReadRepository
    {

        private readonly HpgaranRepository hpGaranRepository;
        private readonly HpforRepository hpforRepository;
        private readonly HpgartarRepository hpGarTarRepository;
        private readonly HpoptdRepository hpOptDRepository;
        private readonly HpoptRepository hpOptRepository;
        private readonly HpexplcidRepository hpExpLciDRepo;
        private readonly HpexplciRepository hpExpLciRepo;
        private readonly HpexpfrhdRepository hpExpFrhDRepo;
        private readonly HpexpfrhRepository hpExpFrhRepo;
        private readonly KpGarApRepository kpGarApRepository;
        private readonly HpgarapRepository hpGarApRepository;

        private readonly IRefRepository refRepo;
        private readonly IParamRepository paramRepository;
        public FormuleHistoRepository(
           
            HpgaranRepository hpGaranRepository,
            HpforRepository hpforRepository,
            HpgartarRepository hpGarTarRepository,
            HpoptdRepository hpOptDRepository,
            HpoptRepository hpOptRepository,
            HpexplcidRepository hpExpLciDRepo,
            HpexplciRepository hpExpLciRepo,
            HpexpfrhdRepository hpExpFrhDRepo,
            HpexpfrhRepository hpExpFrhRepo,
            HpgarapRepository hpGarApRepo,
            IRefRepository refRepo,
            IParamRepository paramRepository
            )
        {
            this.paramRepository = paramRepository;

            this.hpforRepository = hpforRepository;
            this.hpOptRepository = hpOptRepository;
            this.hpOptDRepository = hpOptDRepository;
            this.hpGaranRepository = hpGaranRepository;
            this.hpGarTarRepository = hpGarTarRepository;
            this.hpExpLciDRepo = hpExpLciDRepo;
            this.hpExpLciRepo = hpExpLciRepo;
            this.hpExpFrhDRepo = hpExpFrhDRepo;
            this.hpExpFrhRepo = hpExpFrhRepo;
            this.hpGarApRepository = hpGarApRepo;

            this.refRepo = refRepo;
        }
        private struct ParentGarantie
        {
            public long blocId;
            public long parent;
        }


        private void GetRawData(
            long formuleId,
            int? avenant,
            bool isHisto,
            out KpFor formula,
            out IDictionary<long, KpOpt> options,
            out IDictionary<long, KpOptD> optiondetails,
            out IDictionary<long, KpGaran> gars,
            out IDictionary<long, KpGarTar> gartars,
            out IDictionary<long, ExpressionComplexeLCI> exprLci,
            out IDictionary<long, ExpressionComplexeFranchise> exprFrh,
            out IDictionary<long, KpGarAp> portees
            )
        {
            options = new Dictionary<long, KpOpt>();
            optiondetails = new Dictionary<long, KpOptD>();
            gars = new Dictionary<long, KpGaran>();
            gartars = new Dictionary<long, KpGarTar>();
            exprLci = new Dictionary<long, ExpressionComplexeLCI>();
            exprFrh = new Dictionary<long, ExpressionComplexeFranchise>();
            portees = new Dictionary<long, KpGarAp>();
            formula = null;
            if (isHisto) {
                formula = this.hpforRepository.Get(formuleId, avenant.Value);
                if (formula != null) {
                    var aff = new AffaireId { CodeAffaire = formula.Kdaipb, NumeroAliment = formula.Kdaalx, TypeAffaire = formula.Kdatyp.AsEnum<AffaireType>() };
                    options = this.hpOptRepository.GetByFormule(formuleId, avenant.Value).ToDictionary(x => x.Kdbid);
                    optiondetails = this.hpOptDRepository.GetByFormule(formuleId, avenant.Value).ToDictionary(x => x.Kdcid);
                    gars = this.hpGaranRepository.GetByFormule(formuleId, avenant.Value).ToDictionary(x => x.Kdeid);
                    gartars = this.hpGarTarRepository.GetByFormule(formuleId, avenant.Value).ToDictionary(x => x.Kdgid);
                    portees = this.hpGarApRepository.GetByFormule(formuleId, avenant.Value).ToDictionary(x => x.Kdfid);
                    exprFrh = this.GetExprFrhDict(aff);
                    exprLci = this.GetExprLciDict(aff);
                }
            }
        }

        private void GetRawData(
        AffaireId id,
        out IEnumerable<KpFor> formulas,
        out ILookup<long, KpOpt> options,
        out ILookup<long, KpOptD> optiondetails,
        out ILookup<long, KpGaran> gars,
        out ILookup<long, KpGarTar> gartars,
        out IDictionary<long, ExpressionComplexeLCI> exprLci,
        out IDictionary<long, ExpressionComplexeFranchise> exprFrh,
        out ILookup<long, KpGarAp> portees
        )
        {
            string codeAffaire = id.CodeAffaire.PadLeft(9, ' ');
            string typeAffaire = id.TypeAffaire.AsCode();

            formulas = new List<KpFor>();
            options = new List< KpOpt>().ToLookup(x=>x.Kdbid);
            optiondetails = new List< KpOptD>().ToLookup(x=>x.Kdcid);
            gars = new List< KpGaran>().ToLookup(x=>x.Kdeid);
            gartars = new List< KpGarTar>().ToLookup(x=>x.Kdgid);
            exprLci = new Dictionary<long, ExpressionComplexeLCI>();
            exprFrh = new Dictionary<long, ExpressionComplexeFranchise>();
            portees = new List< KpGarAp>().ToLookup(x=>x.Kdfid);

            if (id.IsHisto) {

                formulas = this.hpforRepository.Liste(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.Value);
                options = this.hpOptRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.Value).ToLookup(x => x.Kdbkdaid);
                optiondetails = this.hpOptDRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.Value).ToLookup(x => x.Kdckdbid);
                gars = this.hpGaranRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.Value).ToLookup(x => x.Kdekdcid);
                gartars = this.hpGarTarRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.Value).ToLookup(x => x.Kdgkdeid);
                portees = this.hpGarApRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.Value).ToLookup(x => x.Kdfkdeid);

            }
            exprFrh = this.GetExprFrhDict(id);
            exprLci = this.GetExprLciDict(id);
        }

        private Formule MapOneFormule(
            KpFor form,
            ILookup<long, KpOpt> options,
            ILookup<long, KpOptD> optiondetails,
            ILookup<long, KpGaran> gars,
            ILookup<ParentGarantie, KpGaran> garseq,
            ILookup<long, KpGarTar> gartars,
            ILookup<long, KpGarAp> portees,
            IDictionary<long, ExpressionComplexeLCI> exprLci,
            IDictionary<long, ExpressionComplexeFranchise> exprFrh)
        {
            var formula = new Formule {
                AffaireId = new AffaireId {
                    CodeAffaire = form.Kdaipb,
                    NumeroAliment = form.Kdaalx,
                    NumeroAvenant = form.Kdaavn,
                    TypeAffaire = form.Kdatyp.AsEnum<AffaireType>(),
                    IsHisto = (form.Kdaavn).HasValue
                },
                Id = form.Kdaid,
                Alpha = form.Kdaalpha,
                Chrono = form.Kdacch,
                Cible = refRepo.GetCibleCatego(form.Kdacible, form.Kdabra),
                Desciption = form.Kdadesc,
                FormuleNumber = form.Kdafor,
            };


            formula.Options = options[form.Kdaid].Select(
                opt
                => MapOption(formula, optiondetails, gars, garseq, gartars, portees, opt, exprLci, exprFrh)
            ).ToList();

            return formula;
        }

        private Option MapOption(
            Formule formula,
            ILookup<long, KpOptD> optiondetails,
            ILookup<long, KpGaran> gars,
            ILookup<ParentGarantie, KpGaran> garseq,
            ILookup<long, KpGarTar> gartars,
            ILookup<long, KpGarAp> portees,
            KpOpt opt,
            IDictionary<long, ExpressionComplexeLCI> exprLci,
            IDictionary<long, ExpressionComplexeFranchise> exprFrh)
        {
            return new Option {
                Id = opt.Kdbid,
                NumeroAvenant = opt.Kdbavn,
                OptionNumber = opt.Kdbopt,
                OptionVolets = optiondetails[opt.Kdbid].Where(d => d.Kdcteng.AsEnum<TypeOption>() == TypeOption.Volet).Select(
                    det =>
                        MapOptionVoletBloc(formula, optiondetails[opt.Kdbid], gars, garseq, gartars, portees, det, exprLci, exprFrh) as OptionVolet
                        ).ToList(),
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
                    NumAvenantCreation = opt.Kdbave,
                    NumAvenantModif = opt.Kdbavg,
                    AAEffetAVNformule = opt.Kdbava,
                    MoisEffetAvnFormu = opt.Kdbavm,
                    JourEffetAvnForm = opt.Kdbavj,
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
        }

        private OptionDetail MapOptionVoletBloc(Formule formula,
            IEnumerable<KpOptD> optds,
            ILookup<long, KpGaran> gars,
            ILookup<ParentGarantie, KpGaran> garseq,
            ILookup<long, KpGarTar> gartars,
            ILookup<long, KpGarAp> portees,
            KpOptD det,
            IDictionary<long, ExpressionComplexeLCI> exprLci,
            IDictionary<long, ExpressionComplexeFranchise> exprFrh)
        {
            OptionDetail c;
            TypeOption type = det.Kdcteng.AsEnum<TypeOption>();
            if (type == TypeOption.Volet) {
                var v = new OptionVolet();
                c = v;
                MapBaseOptionDetail(formula, det, c, type);
                v.Blocs = optds.Where(x => x.Kdckakid == det.Kdckakid && x.Kdcteng.AsEnum<TypeOption>() == TypeOption.Bloc).Select(optd => MapOptionVoletBloc(formula, optds, gars, garseq, gartars, portees, optd, exprLci, exprFrh) as OptionBloc).ToList();
            } else {
                var b = new OptionBloc();
                c = b;
                MapBaseOptionDetail(formula, det, c, type);
                b.Garanties = gars[det.Kdcid].Where(x => x.Kdesem == 0 || x.Kdeniveau == 1).Select(gar => MapGarantie(gar, gars, garseq, gartars, portees, exprLci, exprFrh)).ToList();
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
            c.ModeleID = det.Kdcmodele;
            c.ParamModele = paramRepository.GetParamModele((int)det.Kdckarid);
            c.ParamVolet = paramRepository.GetParamVolets().FirstOrDefault(x => x.VoletId == det.Kdckakid && x.Cible == formula.Cible);
        }

        private void UpdateOptionVoletBloc(
            OptionDetail ovb,
            Formule form,
            KpOptD det,
            Option opt,
            string currentUser
            )
        {
            det.Kdcteng = ovb.Type.AsString();
            det.Kdcordre = ovb.Ordre;
            det.Kdcmodele = ovb.ModeleID;
            if (ovb.Type != TypeOption.Volet) {
                det.Kdckaeid = ovb.ParamModele.Bloc.BlocId;
                det.Kdckaqid = ovb.ParamModele.Bloc.CatBlocId;
                det.Kdckarid = ovb.ParamModele.CatId;
            }
            det.Kdckakid = ovb.ParamVolet.VoletId;
            det.Kdckdbid = opt.Id;

        }

        private void InitOptDetMetadata(OptionDetail ovb, Formule form, KpOptD det, string currentUser)
        {
            det.Kdcmajd = DateTime.Now.AsDate();
            det.Kdcmaju = currentUser;

            if (det.Kdccrd == 0) {
                InitOptionVoletBloc(ovb, form, det, currentUser);
            }
        }

        private void InitOptionVoletBloc(OptionDetail ovb, Formule form, KpOptD det, string currentUser)
        {
            var aff = form.AffaireId;
            det.Kdccrd = DateTime.Now.AsDate();
            det.Kdccru = currentUser;
            det.Kdcflag = 0;

            det.Kdcalx = aff.NumeroAliment;
            det.Kdcipb = aff.CodeAffaire;
            det.Kdctyp = aff.TypeAffaire.AsCode();
            det.Kdcteng = ovb.Type.AsString();
            det.Kdcfor = form.FormuleNumber;
            det.Kdcid = kpOptDRepository.NewId().AsLong();
        }

        private void UpdateGarantie(Garantie garantie, DataModel.DTO.KpGaran gar, Formule form, Option opt, string currentUser, long rootId, long parentId)
        {
            var aff = form.AffaireId;

            gar.Kdekdhid = garantie.LienKPSPEC;
            gar.Kdekdfid = garantie.LienKPGARAP;
            gar.Kdeinven = garantie.LienKPINVEN;

            gar.Kdeipb = aff.CodeAffaire;
            gar.Kdealx = aff.NumeroAliment;
            gar.Kdetyp = aff.TypeAffaire.AsCode();
            gar.Kdeavn = aff.NumeroAvenant;

            gar.Kdefor = form.FormuleNumber;
            gar.Kdeopt = opt.OptionNumber;

            gar.Kdese1 = parentId == 0 ? 0 : rootId;
            gar.Kdesem = parentId;

            gar.Kdeid = garantie.Id;
            gar.Kdecar = garantie.Caractere.AsString();
            gar.Kdeasbase = garantie.Assiette.Base.Code;
            gar.Kdeasmod = garantie.Assiette.IsModifiable.ToYesNo();
            gar.Kdeasobli = garantie.Assiette.IsObligatoire.ToYesNo();
            gar.Kdeasunit = garantie.Assiette.Unite;
            gar.Kdeasvala = garantie.Assiette.ValeurActualise.AsDouble();
            gar.Kdeasvalo = garantie.Assiette.ValeurOrigine.AsDouble();
            gar.Kdeasvalw = garantie.Assiette.ValeurTravail.AsDouble();
            gar.Kdecravn = garantie.AvenantDeCreation;
            gar.Kdemajavn = garantie.Avenantmaj;
            gar.Kdetaxcod = garantie.Taxe.Code;
            gar.Kdeheudeb = garantie.DateDebut.AsHour();
            gar.Kdedatdeb = garantie.DateDebut.AsDate();
            gar.Kdeheufin = garantie.FinDeGarantie.AsHour();
            gar.Kdedatfin = garantie.FinDeGarantie.AsDate();
            gar.Kdewhdeb = garantie.DatestandardDebut.AsHour();
            gar.Kdewddeb = garantie.DatestandardDebut.AsDate();
            gar.Kdewhfin = garantie.DatestandardFin.AsHour();
            gar.Kdewdfin = garantie.DatestandardFin.AsDate();
            gar.Kdedefg = garantie.DefinitionGarantie;
            gar.Kdeduree = garantie.Duree;
            gar.Kdeduruni = garantie.DureeUnite;
            gar.Kdefor = garantie.Formule;
            gar.Kdeid = garantie.Id;
            gar.Kdeinvsp = garantie.InventaireSpecifique.ToYesNo();
            gar.Kdealiref = garantie.IsAlimMontantReference.ToYesNo();
            gar.Kdecatnat = garantie.IsApplicationCATNAT.ToYesNo();
            gar.Kdemodi = garantie.IsFlagModifie.ToYesNo();
            gar.Kdeajout = garantie.IsGarantieAjoutee.ToYesNo();
            gar.Kdeina = garantie.IsIndexe.ToYesNo();
            gar.Kdepind = garantie.IsParameIndex.ToYesNo();
            gar.Kdenat = garantie.Nature.AsString();
            gar.Kdegan = garantie.NatureRetenue.AsString();
            gar.Kdenumpres = garantie.NumDePresentation.AsDouble();
            gar.Kdekdcid = garantie.BlocId;
            gar.Kdeptaxc = garantie.ParamCodetaxe.Code;
            gar.Kdepcatn = garantie.ParametrageCATNAT.ToYesNo();
            gar.Kdepref = garantie.ParametrageMontantRef;
            gar.Kdeseq = garantie.ParamGarantie.Id;
            gar.Kdepntm = garantie.ParamIsNatModifiable.ToYesNo();
            gar.Kdepala = garantie.ParamTypeAlimentat.AsString();
            gar.Kdetaxrep = garantie.RepartitionTaxe.AsDouble();
            gar.Kdetri = garantie.Tri;
            gar.Kdeala = garantie.TypeAlimentation.AsString();
            gar.Kdeprp = garantie.TypeApplication;
            gar.Kdetcd = garantie.TypeControleDate;
            gar.Kdetypemi = garantie.TypeEmission;

            gar.Kdegaran = garantie.ParamGarantie.Garantie.CodeGarantie;
            gar.Kdeniveau = garantie.ParamGarantie.Niveau;
            if (gar.Kdecrd != 0) {
                gar.Kdecru = !String.IsNullOrWhiteSpace(gar.Kdecru) ? gar.Kdecru : currentUser;
                gar.Kdecrd = gar.Kdecrd != 0 ? gar.Kdecrd : DateTime.Now.AsDate();
            }
            // by reverse-egineering :
            gar.Kdepprp = "A";
            gar.Kdepemi = "P";
            gar.Kdealo = "";



        }
        private Garantie MapGarantie(
            DataModel.DTO.KpGaran gar,
            ILookup<long, DataModel.DTO.KpGaran> gars,
            ILookup<ParentGarantie, KpGaran> garseq,
            ILookup<long, DataModel.DTO.KpGarTar> gartars,
            ILookup<long, KpGarAp> portees,

            IDictionary<long, ExpressionComplexeLCI> exprLci,
            IDictionary<long, ExpressionComplexeFranchise> exprFrh)
        {
            return new Garantie {
                //TODO completer les références avec des proxys Lazy loading
                LienKPSPEC = gar.Kdekdhid,
                LienKPGARAP = gar.Kdekdfid,
                LienKPINVEN = gar.Kdeinven,

                Tarif = gartars[gar.Kdeid].Select(x => MapTarif(x, exprLci, exprFrh)).FirstOrDefault(),
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
                AvenantDeCreation = gar.Kdecravn,
                Avenantmaj = gar.Kdemajavn,
                Taxe = refRepo.GetValue<Taxe>(gar.Kdetaxcod),
                DateDebut = MakeNullableDateTime(gar.Kdedatdeb, gar.Kdeheudeb),
                FinDeGarantie = MakeNullableDateTime(gar.Kdedatfin, gar.Kdeheufin),
                DatestandardDebut = MakeNullableDateTime(gar.Kdewddeb, gar.Kdewhdeb),
                DatestandardFin = MakeNullableDateTime(gar.Kdewdfin, gar.Kdewhfin),
                DefinitionGarantie = gar.Kdedefg,
                Duree = gar.Kdeduree,
                DureeUnite = gar.Kdeduruni,
                Formule = gar.Kdefor,
                CodeGarantie = gar.Kdegaran,
                Id = gar.Kdeid,
                NumeroAvenant = gar.Kdeavn,
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
                BlocId = gar.Kdekdcid,
                ParamCodetaxe = refRepo.GetValue<Taxe>(gar.Kdeptaxc),
                ParametrageCATNAT = gar.Kdepcatn.AsNullableBool(),
                ParametrageMontantRef = gar.Kdepref,
                ParamGarantie = paramRepository.GetParamHierarchie(gar.Kdeseq),
                ParamIsNatModifiable = gar.Kdepntm.AsNullableBool(),
                ParamTypeAlimentat = gar.Kdepala.AsEnum<AlimentationValue>(),
                RepartitionTaxe = gar.Kdetaxrep.AsDecimal(),
                Tri = gar.Kdetri,
                TypeAlimentation = gar.Kdeala.AsEnum<AlimentationValue>(),
                TypeApplication = gar.Kdeprp,
                TypeControleDate = gar.Kdetcd,
                TypeEmission = gar.Kdetypemi,
                SousGaranties = garseq[new ParentGarantie { blocId = gar.Kdekdcid, parent = gar.Kdeseq }].Select(subgar => MapGarantie(subgar, gars, garseq, gartars, portees, exprLci, exprFrh)).ToList()
            };
        }

        private PorteeGarantie MapPortee(KpGarAp x)
        {
            return new PorteeGarantie() {
                Id = x.Kdfid,
                GarantieId = x.Kdfkdeid,
                Montant = x.Kdfmnt,
                ObjetId = x.Kdfobj,
                RisqueId = x.Kdfrsq,
                TypeCalcul = x.Kdftyc.AsEnum<TypeCalcul>(),
                ValeursPrime = new Valeurs() {
                    Unite = refRepo.GetValue<UnitePrime>(x.Kdfpru),
                    ValeurActualise = x.Kdfpra,
                    ValeurOrigine = x.Kdfprv,
                    ValeurTravail = x.Kdfprw
                }


            };
        }

        private TarifGarantie MapTarif(DataModel.DTO.KpGarTar tar,
            IDictionary<long, ExpressionComplexeLCI> exprLci,
            IDictionary<long, ExpressionComplexeFranchise> exprFrh)
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
                    ExpressionComplexe = exprFrh.GetValueOrDefault(tar.Kdgkdkid)
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
                    ExpressionComplexe = exprLci.GetValueOrDefault(tar.Kdgkdiid)
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

        private void UpdateTarif(TarifGarantie gar, KpGarTar tar, KpGaran kpgaran)
        {
            tar.Kdgpribase = gar.PrimeValeur.Base.Code;
            tar.Kdgpriunit = gar.PrimeValeur.Unite;
            tar.Kdgprivala = gar.PrimeValeur.ValeurActualise.AsDouble();
            tar.Kdgprivalo = gar.PrimeValeur.ValeurOrigine.AsDouble();
            tar.Kdgprivalw = gar.PrimeValeur.ValeurTravail.AsDouble();
            tar.Kdgprimod = gar.PrimeValeur.IsModifiable.ToYesNo();
            tar.Kdgpriobl = gar.PrimeValeur.IsObligatoire.ToYesNo();
            tar.Kdgfrhbase = gar.Franchise.Base.Code;
            tar.Kdgfrhunit = gar.Franchise.Unite;
            tar.Kdgfrhvala = gar.Franchise.ValeurActualise.AsDouble();
            tar.Kdgfrhvalo = gar.Franchise.ValeurOrigine.AsDouble();
            tar.Kdgfrhvalw = gar.Franchise.ValeurTravail.AsDouble();
            tar.Kdgfrhmod = gar.Franchise.IsModifiable.ToYesNo();
            tar.Kdgfrhobl = gar.Franchise.IsObligatoire.ToYesNo();
            tar.Kdgkdkid = gar.Franchise.ExpressionComplexe?.Id ?? 0L;
            tar.Kdgfmibase = gar.FranchiseMini.Base.Code;
            tar.Kdgfmiunit = gar.FranchiseMini.Unite;
            tar.Kdgfmivala = gar.FranchiseMini.ValeurActualise.AsDouble();
            tar.Kdgfmivalo = gar.FranchiseMini.ValeurOrigine.AsDouble();
            tar.Kdgfmivalw = gar.FranchiseMini.ValeurTravail.AsDouble();
            tar.Kdgfmabase = gar.FranchiseMax.Base.Code;
            tar.Kdgfmaunit = gar.FranchiseMax.Unite;
            tar.Kdgfmavala = gar.FranchiseMax.ValeurActualise.AsDouble();
            tar.Kdgfmavalo = gar.FranchiseMax.ValeurOrigine.AsDouble();
            tar.Kdgfmavalw = gar.FranchiseMax.ValeurTravail.AsDouble();
            tar.Kdglcibase = gar.LCI.Base.Code;
            tar.Kdglciunit = gar.LCI.Unite;
            tar.Kdglcivala = gar.LCI.ValeurActualise.AsDouble();
            tar.Kdglcivalo = gar.LCI.ValeurOrigine.AsDouble();
            tar.Kdglcivalw = gar.LCI.ValeurTravail.AsDouble();
            tar.Kdglcimod = gar.LCI.IsModifiable.ToYesNo();
            tar.Kdglciobl = gar.LCI.IsObligatoire.ToYesNo();
            tar.Kdgkdiid = gar.LCI.ExpressionComplexe?.Id ?? 0L;
            tar.Kdgnumtar = gar.NumeroTarif;
            tar.Kdgcmc = gar.ComptantMontantcalcule.AsDouble();
            tar.Kdgcht = gar.ComptantMontantForceHT.AsDouble();
            tar.Kdgctt = gar.ComptantMontantForceTTC.AsDouble();
            tar.Kdgmntbase = gar.PrimeMontantBase.AsDouble();
            tar.Kdgprimpro = gar.PrimeProvisionnelle.AsDouble();
            tar.Kdgtmc = gar.TotalMontantCalcule.AsDouble();
            tar.Kdgtff = gar.TotalMontantForce.AsDouble();


            tar.Kdgipb = kpgaran.Kdeipb;
            tar.Kdgtyp = kpgaran.Kdetyp;
            tar.Kdgalx = kpgaran.Kdealx;

            tar.Kdgfor = kpgaran.Kdefor;
            tar.Kdgopt = kpgaran.Kdeopt;
            tar.Kdggaran = kpgaran.Kdegaran;
            tar.Kdgkdeid = kpgaran.Kdeid;

            if (tar.Kdgid == 0) {
                tar.Kdgid = kpGarTarRepository.NewId().AsLong();
            }

        }
        private IDictionary<long, ExpressionComplexeLCI> GetExprLciDict(AffaireId aff)
        {
            var detLci = this.kpExpLciDRepo.GetByAffaire(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment).Select(x => {
                var ret = new ExpressionComplexeDetailLCI {
                    Id = x.Kdjid,
                    ExprId = x.Kdjkdiid,
                    CodeBase = refRepo.GetValue<BaseLCI>(x.Kdjlcbase),
                    ConcurrentCodeBase = refRepo.GetValue<BaseLCI>(x.Kdjlobase),
                    ConcurrentUnite = refRepo.GetValue<UniteLCI>(x.Kdjlovau),
                    Unite = refRepo.GetValue<UniteLCI>(x.Kdjlcvau),
                    ConcurrentValeur = x.Kdjlcval.AsDecimal(),
                    Valeur = x.Kdjlcval.AsDecimal(),
                    Ordre = x.Kdjordre
                };
                return ret;
            }).ToLookup(x => x.ExprId);


            var expr = this.kpExpLciRepo.GetByAffaire(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment).Select(x => {
                var ret = new ExpressionComplexeLCI {
                    AffaireId = new AffaireId() {
                        CodeAffaire = x.Kdiipb,
                        NumeroAliment = x.Kdialx,
                        TypeAffaire = x.Kdityp.AsEnum<AffaireType>()
                    },
                    Origine = x.Kdiori.AsEnum<OrigineExpression>(),
                    Code = x.Kdilce,
                    Id = x.Kdiid,
                    Description = x.Kdidesc,
                    Designation = paramRepository.GetDesignation(x.Kdidesi),
                    DesiId = x.Kdidesi,
                };
                ret.Details = detLci[x.Kdiid].ToList();
                return ret;
            }).ToDictionary(x => x.Id);
            return expr;
        }


        private IDictionary<long, ExpressionComplexeFranchise> GetExprFrhDict(AffaireId aff)
        {
            if (aff.IsHisto) {
                var detFrh = this.hpExpFrhDRepo.GetByAffaire(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment, aff.NumeroAvenant.Value).Select(x => {
                    var ret = new ExpressionComplexeDetailFranchise {
                        Id = x.Kdlid,
                        ExprId = x.Kdlkdkid,
                        Valeur = x.Kdlfhval.AsDecimal(),
                        Ordre = x.Kdlordre,
                        CodeBase = refRepo.GetValue<BaseFranchise>(x.Kdlfhbase),
                        CodeIncide = refRepo.GetValue<Indice>(x.Kdlind),
                        IncideValeur = x.Kdlivo.AsDecimal(),
                        LimiteDebut = MakeNullableDateTime((int)x.Kdllimdeb),
                        LimiteFin = MakeNullableDateTime((int)x.Kdllimfin),
                        Unite = refRepo.GetValue<UniteFranchise>(x.Kdlfhvau),
                        ValeurMax = x.Kdlfhmaxi.AsDecimal(),
                        ValeurMin = x.Kdlfhmini.AsDecimal()
                    };
                    return ret;
                }).ToLookup(x => x.ExprId);


                var expr = this.hpExpFrhRepo.GetByAffaire(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment, aff.NumeroAvenant.Value).Select(x => {
                    var ret = new ExpressionComplexeFranchise {
                        AffaireId = new AffaireId() {
                            CodeAffaire = x.Kdkipb,
                            NumeroAliment = x.Kdkalx,
                            TypeAffaire = x.Kdktyp.AsEnum<AffaireType>()
                        },
                        Origine = x.Kdkori.AsEnum<OrigineExpression>(),
                        Code = x.Kdkfhe,
                        Id = x.Kdkid,
                        Description = x.Kdkdesc,
                        Designation = paramRepository.GetDesignation(x.Kdkdesi),
                        DesiId = x.Kdkdesi,
                    };
                    ret.Details = detFrh[x.Kdkid].ToList();
                    return ret;
                }).ToDictionary(x => x.Id);
            }
            return expr;
        }
    }

}
