using static Albingia.Kheops.OP.Application.Infrastructure.Tools;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Infrastructure.Extensions;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;

namespace Albingia.Kheops.OP.DataAdapter
{
    public class FormuleRWRepository : IFormuleRepository
    {
        private readonly KpGaranRepository kpGaranRepository;
        private readonly KpForRepository kpforRepository;
        private readonly KpGarTarRepository kpGarTarRepository;
        private readonly KpOptDRepository kpOptDRepository;
        private readonly KpOptRepository kpOptRepository;
        private readonly KpExpLCIDRepository kpExpLciDRepo;
        private readonly KpExpLCIRepository kpExpLciRepo;
        private readonly KpExpFrhDRepository kpExpFrhDRepo;
        private readonly KpExpFrhRepository kpExpFrhRepo;
        private readonly KpGarApRepository kpGarApRepository;
        private readonly IRefRepository refRepo;
        private readonly IParamRepository paramRepository;

        public FormuleRWRepository(
            KpGaranRepository KpGaranRepository,
            KpForRepository kpforRepository,
            KpGarTarRepository KpGarTarRepository,
            KpOptDRepository kpOptDRepository,
            KpOptRepository kpOptRepository,
            KpExpLCIDRepository kpExpLciDRepo,
            KpExpLCIRepository kpExpLciRepo,
            KpExpFrhDRepository kpExpFrhDRepo,
            KpExpFrhRepository kpExpFrhRepo,
            KpGarApRepository kpGarApRepo,

            IRefRepository refRepo,
            IParamRepository paramRepository
            )
        {
            this.kpGaranRepository = KpGaranRepository;
            this.kpforRepository = kpforRepository;
            this.kpGarTarRepository = KpGarTarRepository;
            this.kpOptDRepository = kpOptDRepository;
            this.kpOptRepository = kpOptRepository;
            this.kpGarApRepository = kpGarApRepo;
            this.refRepo = refRepo;
            this.paramRepository = paramRepository;
            this.kpExpFrhDRepo = kpExpFrhDRepo;
            this.kpExpFrhRepo = kpExpFrhRepo;
            this.kpExpLciDRepo = kpExpLciDRepo;
            this.kpExpLciRepo = kpExpLciRepo;
        }
        private struct ParentGarantie
        {
            public long blocId;
            public long parent;
        }

        public void SaveFormule(Formule formuleToSave, string username)
        {
            KpFor formula;
            IDictionary<long, KpOpt> options;
            IDictionary<long, KpOptD> optiondetails;
            IDictionary<long, KpGaran> gars;
            IDictionary<long, KpGarTar> gartars;
            IDictionary<long, ExpressionComplexeFranchise> expFrh;
            IDictionary<long, ExpressionComplexeLCI> expLci;
            IDictionary<long, KpGarAp> portees;

            GetRawData(formuleToSave.Id, formuleToSave.AffaireId.NumeroAvenant, formuleToSave.AffaireId.IsHisto,
                out formula, out options, out optiondetails, out gars, out gartars, out expLci, out expFrh, out portees);

            ILookup<ParentGarantie, KpGaran> garseq = gars
                .Select(x => x.Value)
                .ToLookup(x => new ParentGarantie { blocId = x.Kdekdcid, parent = x.Kdesem });
            bool exists = !(formula is null);
            if (formula == null) {
                formula = new KpFor();
            }
            var formuleAsFromDb = MapOneFormule(formula,
                options.Select(x => x.Value).ToLookup(x => x.Kdbkdaid),
                optiondetails.Select(x => x.Value).ToLookup(x => x.Kdckdbid),
                gars.Select(x => x.Value).ToLookup(x => x.Kdekdcid),
                garseq, gartars.Select(x => x.Value).ToLookup(x => x.Kdgkdeid),
                portees.Select(x => x.Value).ToLookup(x => x.Kdfkdeid)
,
                expLci,
                expFrh);
            if (exists) {
                var formUpt = this.UpdateFormule(new KpFor(formula), formuleToSave, username);
                if (!KpFor.Equals(formUpt, formula)) {
                    this.kpforRepository.Update(formUpt);
                }
            } else {
                var formUpt = this.UpdateFormule(new KpFor(), formuleToSave, username);
                if (!KpFor.Equals(formUpt, formula)) {
                    UpdateFormuleMetadata(username, formUpt);
                    this.kpforRepository.Insert(formUpt);
                }
                formuleToSave.Id = formUpt.Kdaid;
            }
            foreach (var opt in formuleToSave.Options) {
                SaveOptionInternal(formuleToSave, username, options, optiondetails, gars, gartars, portees, opt);
            }
            foreach (var kpopt in options.Select(x => x.Value).Where(x => !formuleToSave.Options.Any(y => y.Id == x.Kdbid))) {
                this.kpOptRepository.Delete(kpopt);
            }
        }

        private static void UpdateFormuleMetadata(string username, KpFor formUpt)
        {
            formUpt.Kdamajd = DateTime.Now.AsDate();
            formUpt.Kdamaju = username;
        }

        private void SaveOptionInternal(Formule formuleToSave, string username, IDictionary<long, KpOpt> options, IDictionary<long, KpOptD> optiondetails, IDictionary<long, KpGaran> gars, IDictionary<long, KpGarTar> gartars, IDictionary<long, KpGarAp> portees, Option opt)
        {
            var optToUpdate = options.GetValueOrDefault(opt.Id);
            if (optToUpdate == null) {
                var optUpdated = new KpOpt();
                this.UpdateOption(optUpdated, opt);
                this.UpdateOptionMetadata(formuleToSave, optUpdated, true, username);
                this.kpOptRepository.Insert(optUpdated);
                opt.Id = optUpdated.Kdbid;
            } else {
                var optUpdated = new KpOpt(optToUpdate);
                this.UpdateOption(optUpdated, opt);
                if (!KpOpt.Equals(optUpdated, optToUpdate)) {
                    this.UpdateOptionMetadata(formuleToSave, optUpdated, false, username);
                    this.kpOptRepository.Update(optUpdated);
                }
            }
            foreach (var volet in opt.OptionVolets) {
                SaveVoletInternal(formuleToSave, username, optiondetails, gars, gartars, portees, opt, volet);
            }
            foreach (var kpbloc in optiondetails.Select(x => x.Value).Where(x => x.Kdcteng == "V" && !opt.OptionVolets.Any(y => y.Id == x.Kdcid))) {
                this.kpOptDRepository.Delete(kpbloc);
            }

        }

        private void SaveVoletInternal(Formule formuleToSave, string username, IDictionary<long, KpOptD> optiondetails, IDictionary<long, KpGaran> gars, IDictionary<long, KpGarTar> gartars, IDictionary<long, KpGarAp> portees, Option opt, OptionVolet volet)
        {
            var volToUpdate = optiondetails.GetValueOrDefault(volet.Id);
            if (volToUpdate == null) {
                var volUpdated = new KpOptD();
                this.UpdateOptionVoletBloc(volet, formuleToSave, volUpdated, opt, username);
                InitOptDetMetadata(volet, formuleToSave, volUpdated, username);
                this.kpOptDRepository.Insert(volUpdated);
                volet.Id = volUpdated.Kdcid;
            } else {
                var volUpdated = new KpOptD(volToUpdate);
                this.UpdateOptionVoletBloc(volet, formuleToSave, volUpdated, opt, username);
                if (!KpOptD.Equals(volToUpdate, volUpdated)) {
                    InitOptDetMetadata(volet, formuleToSave, volUpdated, username);
                    this.kpOptDRepository.Update(volUpdated);
                }

            }
            foreach (var bloc in volet.Blocs) {
                SaveBlocInternal(formuleToSave, username, optiondetails, gars, gartars, portees, opt, bloc);

            }
            foreach (var kpbloc in optiondetails.Select(x => x.Value).Where(x => x.Kdcteng == "B" && volet.ParamVolet.VoletId == x.Kdckakid && !volet.Blocs.Any(y => y.Id == x.Kdcid))) {
                this.kpOptDRepository.Delete(kpbloc);
            }
        }

        private void SaveBlocInternal(Formule formuleToSave, string username, IDictionary<long, KpOptD> optiondetails, IDictionary<long, KpGaran> gars, IDictionary<long, KpGarTar> gartars, IDictionary<long, KpGarAp> portees, Option opt, OptionBloc bloc)
        {
            var blocToUpdate = optiondetails.GetValueOrDefault(bloc.Id);
            if (blocToUpdate == null) {
                var blocUpdated = new KpOptD();
                InitOptDetMetadata(bloc, formuleToSave, blocUpdated, username);
                this.UpdateOptionVoletBloc(bloc, formuleToSave, blocUpdated, opt, username);
                this.kpOptDRepository.Insert(blocUpdated);
                bloc.Id = blocUpdated.Kdcid;

            } else {
                var blocUpdated = new KpOptD(blocToUpdate);

                this.UpdateOptionVoletBloc(bloc, formuleToSave, blocUpdated, opt, username);
                if (!KpOptD.Equals(blocToUpdate, blocUpdated)) {
                    InitOptDetMetadata(bloc, formuleToSave, blocUpdated, username);
                    this.kpOptDRepository.Update(blocUpdated);
                }
            }
            var garanties = bloc.Garanties;
            foreach (var gar in garanties) {
                var rootId = gar.ParamGarantie.Id;
                var parentId = 0;
                gar.BlocId = bloc.Id;
                SaveGarantieInternal(formuleToSave, username, gars, gartars, portees, opt, gar, rootId, parentId);
            }
            foreach (var kpgar in gars.Select(x => x.Value).Where(x => x.Kdesem == 0 && x.Kdekdcid == bloc.Id && !garanties.Any(y => y.Id == x.Kdeid))) {
                this.kpGaranRepository.Delete(kpgar);
            }
        }

        private void SaveGarantieInternal(Formule formuleToSave, string username, IDictionary<long, KpGaran> gars, IDictionary<long, KpGarTar> gartars, IDictionary<long, KpGarAp> portees, Option opt, Garantie gar, long rootId, long parentId)
        {
            var garToUpdate = gars.GetValueOrDefault(gar.Id);
            KpGaran garUpdated;
            if (garToUpdate == null) {
                garUpdated = new KpGaran();
                this.UpdateGarantie(gar, garUpdated, formuleToSave, opt, username, rootId, parentId);
                this.kpGaranRepository.Insert(garUpdated);
                gar.Id = garUpdated.Kdeid;
            } else {
                garUpdated = new KpGaran(garToUpdate);
                this.UpdateGarantie(gar, garUpdated, formuleToSave, opt, username, rootId, parentId);
                if (!KpGaran.Equals(garToUpdate, garUpdated)) {
                    this.kpGaranRepository.Update(garUpdated);
                }
            }
            foreach (var portee in gar.Portees) {
                SavePorteeInternal(portee, username, portees.GetValueOrDefault(portee.Id), garUpdated);
            }
            SaveGarTarInternal(gartars, gar, garUpdated);
            foreach (var subgar in gar.SousGaranties) {
                subgar.BlocId = gar.BlocId;
                SaveGarantieInternal(formuleToSave, username, gars, gartars, portees, opt, subgar, rootId, gar.ParamGarantie.Id);
            }
            foreach (var subkpgar in gars.Select(x => x.Value).Where(x => x.Kdesem == gar.ParamGarantie.Id && !gar.SousGaranties.Any(y => y.Id == x.Kdeid))) {
                this.kpGaranRepository.Delete(subkpgar);
            }
            foreach (var subkpgarap in portees.Select(x => x.Value).Where(x => x.Kdfkdeid == garUpdated.Kdeid && !gar.Portees.Any(y=>y.Id == x.Kdfid))) {
                this.kpGarApRepository.Delete(subkpgarap);
            }

        }

        private void SavePorteeInternal(PorteeGarantie portee, string username, KpGarAp porteeToUpdate,  KpGaran gar)
        {
            KpGarAp porteeUpdated;
            if (porteeToUpdate == null) {
                porteeUpdated = new KpGarAp();

                UpdatePortee(portee, username, gar, porteeUpdated);

                this.kpGarApRepository.Insert(porteeUpdated);
                portee.Id = porteeUpdated.Kdfid;
            } else {
                porteeUpdated = new KpGarAp(porteeToUpdate);
                if (!KpGaran.Equals(porteeToUpdate, porteeUpdated)) {
                    this.kpGarApRepository.Update(porteeUpdated);
                }
            }
        }

        private void UpdatePortee(PorteeGarantie portee, string username, KpGaran gar,  KpGarAp porteeUpdated)
        {
            porteeUpdated.Kdfopt = gar.Kdeopt;
            porteeUpdated.Kdffor = gar.Kdefor;
            porteeUpdated.Kdfmnt = portee.Montant;
            porteeUpdated.Kdfpra = portee.ValeursPrime.ValeurActualise;
            porteeUpdated.Kdfpru = portee.ValeursPrime.Unite;
            porteeUpdated.Kdfprw = portee.ValeursPrime.ValeurTravail;
            porteeUpdated.Kdfprv = portee.ValeursPrime.ValeurOrigine;
            porteeUpdated.Kdfmaju = username;
            porteeUpdated.Kdfmajd = DateTime.Now.AsDate();

            if (portee.Id == 0) {
                // case of creation
                porteeUpdated.Kdfid = this.kpGarApRepository.NewId();
                porteeUpdated.Kdfobj = portee.ObjetId;
                porteeUpdated.Kdfrsq = portee.RisqueId;
                porteeUpdated.Kdfalx = gar.Kdealx;
                porteeUpdated.Kdfavn = gar.Kdeavn;
                porteeUpdated.Kdfipb = gar.Kdeipb;
                porteeUpdated.Kdftyp = gar.Kdetyp;
                porteeUpdated.Kdfgaran = gar.Kdegaran;
                porteeUpdated.Kdfkdeid = gar.Kdeid;
                porteeUpdated.Kdfcru = username;
                porteeUpdated.Kdfcrd = DateTime.Now.AsDate();
            }
        }

        private void SaveGarTarInternal(IDictionary<long, KpGarTar> gartars, Garantie gar, KpGaran garUpdated)
        {
            var garTarToUpdate = gartars.GetValueOrDefault(gar.Tarif.Id);
            KpGarTar garTarUpdated;
            if (garTarToUpdate == null) {
                garTarUpdated = new KpGarTar();
                this.UpdateTarif(gar.Tarif, garTarUpdated, garUpdated);
                this.kpGarTarRepository.Insert(garTarUpdated);
            } else {
                garTarUpdated = new KpGarTar(garTarToUpdate);
                this.UpdateTarif(gar.Tarif, garTarUpdated, garUpdated);
                if (!KpGarTar.Equals(garTarToUpdate, garTarUpdated)) {
                    this.kpGarTarRepository.Update(garTarUpdated);
                }
            }
            foreach (var toDelete in gartars.Select(x => x.Value).Where(x => x.Kdgkdeid == gar.Id && x.Kdgid != garTarUpdated.Kdgid)) {
                this.kpGarTarRepository.Delete(garTarToUpdate);
            }
        }

        private void UpdateOptionMetadata(Formule formule, KpOpt optUpt, bool creation, string username)
        {
            AffaireId id = formule.AffaireId;
            optUpt.Kdbfor = formule.FormuleNumber;
            optUpt.Kdbkdaid = formule.Id;
            optUpt.Kdbmaju = username;
            optUpt.Kdbmajh = DateTime.Now.AsHour();
            optUpt.Kdbmajd = DateTime.Now.AsDate();
            if (optUpt.Kdbcrd == 0) {
                optUpt.Kdbcru = username;
                optUpt.Kdbcrh = DateTime.Now.AsHour();
                optUpt.Kdbcrd = DateTime.Now.AsDate();
                optUpt.Kdbalx = id.NumeroAliment;
                optUpt.Kdbavn = id.NumeroAvenant;
                optUpt.Kdbipb = id.CodeAffaire;
                optUpt.Kdbtyp = id.TypeAffaire.AsString();
            }
        }
        private void UpdateOption(KpOpt optUpt, Option opt)
        {
            optUpt.Kdbid = opt.Id;
            optUpt.Kdbavn = opt.NumeroAvenant;
            optUpt.Kdbopt = opt.OptionNumber;
            optUpt.Kdbpaq = opt.MontantsOption.IsMontantAcquis.ToYesNo();
            optUpt.Kdbacq = opt.MontantsOption.Montantacquis.AsDouble();
            optUpt.Kdbtmc = opt.MontantsOption.TotalCalculeRef.AsDouble();
            optUpt.Kdbtff = opt.MontantsOption.TotalMontantForceRef.AsDouble();
            optUpt.Kdbtfp = opt.MontantsOption.TotalCoefcalcul.AsDouble();
            optUpt.Kdbpro = opt.MontantsOption.IsMontantprovisionnel.ToYesNo();
            optUpt.Kdbtmi = opt.MontantsOption.IsMontantForcepourMini.ToYesNo();
            optUpt.Kdbtfm = opt.MontantsOption.MotifTotalforce;
            optUpt.Kdbcmc = opt.MontantsOption.ComptantMontantCalcule.AsDouble();
            optUpt.Kdbcfo = opt.MontantsOption.IsComptantMontantForce.ToYesNo();
            optUpt.Kdbcht = opt.MontantsOption.ComptantMontantForceHT.AsDouble();
            optUpt.Kdbctt = opt.MontantsOption.ComptantMontantForceTTC.AsDouble();
            optUpt.Kdbccp = opt.MontantsOption.CoeffCalculForceComptant.AsDouble();
            optUpt.Kdbval = opt.MontantsOption.ValeurOrigine.AsDouble();
            optUpt.Kdbvaa = opt.MontantsOption.ValeurActualisee.AsDouble();
            optUpt.Kdbvaw = opt.MontantsOption.Valeurdetravail.AsDouble();
            optUpt.Kdbvat = opt.MontantsOption.TypeDevaleur;
            optUpt.Kdbvau = opt.MontantsOption.UniteDeValeur;
            optUpt.Kdbvah = opt.MontantsOption.IsTTC.ToYesNo();
            optUpt.Kdbivo = opt.MontantsOption.ValeurIndiceOrigin.AsDouble();
            optUpt.Kdbiva = opt.MontantsOption.ValeurIndiceActual.AsDouble();
            optUpt.Kdbivw = opt.MontantsOption.ValeurIndiceTravai.AsDouble();
            optUpt.Kdbave = opt.MontantsOption.NumAvenantCreation;
            optUpt.Kdbavg = opt.MontantsOption.NumAvenantModif;
            optUpt.Kdbava = opt.MontantsOption.AAEffetAVNformule;
            optUpt.Kdbavm = opt.MontantsOption.MoisEffetAvnFormu;
            optUpt.Kdbavj = opt.MontantsOption.JourEffetAvnForm;
            optUpt.Kdbehh = opt.MontantsOption.ProchaineEChHT.AsDouble();
            optUpt.Kdbehc = opt.MontantsOption.ProchaineEchCatnat.AsDouble();
            optUpt.Kdbehi = opt.MontantsOption.ProchaineEchIncend.AsDouble();
            optUpt.Kdbasvalo = opt.MontantsOption.AssietteValOrigine.AsDouble();
            optUpt.Kdbasvala = opt.MontantsOption.AssietteValActual.AsDouble();
            optUpt.Kdbasvalw = opt.MontantsOption.AssietteValeurW.AsDouble();
            optUpt.Kdbasunit = opt.MontantsOption.AssietteUnite;
            optUpt.Kdbasbase = opt.MontantsOption.AssietteBaseTypeValeur;
            optUpt.Kdbger = opt.MontantsOption.MontantRefForcesaisi.AsDouble();


        }

        private KpFor UpdateFormule(KpFor form, Formule formuleToSave, string username)
        {

            form.Kdaid = formuleToSave.Id;
            form.Kdaalpha = formuleToSave.Alpha;
            form.Kdacch = formuleToSave.Chrono;
            form.Kdacible = formuleToSave.Cible.Cible.Code;
            form.Kdabra = formuleToSave.Cible.Branche.Code;
            form.Kdadesc = formuleToSave.Desciption;
            form.Kdafor = formuleToSave.FormuleNumber;
            form.Kdaavn = formuleToSave.AffaireId.NumeroAvenant;

            if (form.Kdaid <= 0) {
                form.Kdaid = this.kpforRepository.NewId();
                form.Kdacrd = DateTime.Now.AsDate();
                form.Kdacru = username;
                form.Kdaipb = formuleToSave.AffaireId.CodeAffaire;
                form.Kdaalx = formuleToSave.AffaireId.NumeroAliment;
                form.Kdatyp = formuleToSave.AffaireId.TypeAffaire.AsString();
            }


            return form;
        }

        public IEnumerable<Formule> GetFormulesForAffaire(AffaireId id)
        {
            IEnumerable<KpFor> formulas;
            ILookup<long, KpOpt> options;
            ILookup<long, KpOptD> optiondetails;
            ILookup<long, KpGaran> gars;
            ILookup<long, KpGarTar> gartars;
            ILookup<long, KpGarAp> portees;
            IDictionary<long, ExpressionComplexeLCI> exprLci;
            IDictionary<long, ExpressionComplexeFranchise> exprFrh;
            GetRawData(id, out formulas, out options, out optiondetails, out gars, out gartars, out exprLci, out exprFrh, out portees);

            ILookup<ParentGarantie, KpGaran> garseq = gars.SelectMany(x => x).ToLookup(x => new ParentGarantie { blocId = x.Kdekdcid, parent = x.Kdesem });

            var f = formulas.Select(form => MapOneFormule(form, options, optiondetails, gars, garseq, gartars, portees, exprLci, exprFrh));
            return f;
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
            if (!isHisto) {
                formula = this.kpforRepository.Get(formuleId);
                if (formula != null) {
                    var aff = new AffaireId { CodeAffaire = formula.Kdaipb, NumeroAliment = formula.Kdaalx, TypeAffaire = formula.Kdatyp.AsEnum<AffaireType>() };
                    options = this.kpOptRepository.GetByFormule(formuleId).ToDictionary(x => x.Kdbid);
                    optiondetails = this.kpOptDRepository.GetByFormule(formuleId).ToDictionary(x => x.Kdcid);
                    gars = this.kpGaranRepository.GetByFormule(formuleId).ToDictionary(x => x.Kdeid);
                    gartars = this.kpGarTarRepository.GetByFormule(formuleId).ToDictionary(x => x.Kdgid);
                    exprFrh = this.GetExprFrhDict(aff);
                    exprLci = this.GetExprLciDict(aff);
                    portees = this.kpGarApRepository.GetByFormule(formuleId).ToDictionary(x => x.Kdfid);

                }
            } 
        }

        private class RawDataMany
        {
            public IEnumerable<KpFor> formulas;
            public ILookup<long, KpOpt> options;
            public ILookup<long, KpOptD> optiondetails;
            public ILookup<long, KpGaran> gars;
            public ILookup<long, KpGarTar> gartars;
            public IDictionary<long, ExpressionComplexeLCI> exprLci;
            public IDictionary<long, ExpressionComplexeFranchise> exprFrh;
            public ILookup<long, KpGarAp> portees;
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
            string typeAffaire = id.TypeAffaire.AsString();

            if (!id.IsHisto) {
                formulas = this.kpforRepository.Liste(typeAffaire, codeAffaire, id.NumeroAliment);
                options = this.kpOptRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdbkdaid);
                optiondetails = this.kpOptDRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdckdbid);
                gars = this.kpGaranRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdekdcid);
                gartars = this.kpGarTarRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdgkdeid);
                portees = this.kpGarApRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdfkdeid);
            } else {
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
            det.Kdctyp = aff.TypeAffaire.AsString();
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
            gar.Kdetyp = aff.TypeAffaire.AsString();
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
            var detLci = this.kpExpLciDRepo.GetByAffaire(aff.TypeAffaire.AsString(), aff.CodeAffaire, aff.NumeroAliment).Select(x => {
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


            var expr = this.kpExpLciRepo.GetByAffaire(aff.TypeAffaire.AsString(), aff.CodeAffaire, aff.NumeroAliment).Select(x => {
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
            var detFrh = this.kpExpFrhDRepo.GetByAffaire(aff.TypeAffaire.AsString(), aff.CodeAffaire, aff.NumeroAliment).Select(x => {
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


            var expr = this.kpExpFrhRepo.GetByAffaire(aff.TypeAffaire.AsString(), aff.CodeAffaire, aff.NumeroAliment).Select(x => {
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
            return expr;
        }
    }

}
