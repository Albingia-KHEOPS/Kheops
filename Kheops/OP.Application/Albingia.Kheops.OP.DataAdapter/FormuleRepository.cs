using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Risque;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Albingia.Kheops.OP.Application.Infrastructure.Extension.Tools;

namespace Albingia.Kheops.OP.DataAdapter
{
    public partial class FormuleRepository
    {
        public void SaveFormule(Risque risque, Formule formuleToSave, string username) {
            var data = GetRawData(formuleToSave.AffaireId, formuleToSave.Id, formuleToSave.AffaireId.NumeroAvenant, formuleToSave.AffaireId.IsHisto);
            var formula = data.formula;
            bool exists = !(formula is null);
            if (formula == null) {
                formula = new KpFor();
            }
            var formuleAsFromDb = data.ToFormule();
            if (exists) {
                var formUpt = this.UpdateFormule(new KpFor(formula), formuleToSave, username);
                if (!KpFor.Equals(formUpt, formula)) {
                    this.kpforRepository.Update(formUpt);
                }
            }
            else {
                var formUpt = this.UpdateFormule(new KpFor(), formuleToSave, username);
                if (!KpFor.Equals(formUpt, formula)) {
                    UpdateFormuleMetadata(username, formUpt);
                    this.kpforRepository.Insert(formUpt);
                }
                formuleToSave.Id = formUpt.Kdaid;
            }

            bool rgtxIsMonaco = risque.RegimeTaxe.Code.IsIn(RegimeTaxe.Monaco, RegimeTaxe.MonacoProfessionLiberaleHabitation);
            foreach (var opt in formuleToSave.Options) {
                if (opt.OptionVolets.Any(v => v.IsChecked && (v.ParamModele ?? v.Blocs.First().ParamModele).Code == Garantie.CodeGareat)) {
                    if (rgtxIsMonaco || !opt.AllSelectedGaranties.Any(g => g.ParamGarantie.ParamGarantie.IsAttentatGareat)) {
                        opt.RemoveGareat();
                    }
                }
                else if (!rgtxIsMonaco && opt.AllSelectedGaranties.Any(g => g.ParamGarantie.ParamGarantie.IsAttentatGareat)) {
                        opt.AddGareat();
                }
                SaveOptionInternal(formuleAsFromDb?.Options?.FirstOrDefault(optDb => optDb.Id == opt.Id), formuleToSave, username, data, opt);
            }
            var optionsToDelete = formuleAsFromDb?.Options?.Where(x => !formuleToSave.Options.Any(y => y.Id == x.Id)) ?? Enumerable.Empty<Option>();
            foreach (var opt in optionsToDelete) {
                CleanUp(formuleToSave, opt);
            }
        }

        public void Delete(Formule formule) {
            this.kpforRepository.FullDelete(formule);
            this.kpctrleRepository.DeleteFormuleCtrl(new KpCtrlE {
                Kevipb = formule.AffaireId.CodeAffaire.ToIPB(),
                Kevalx = formule.AffaireId.NumeroAliment,
                Kevtyp = formule.AffaireId.TypeAffaire.AsCode(),
                Kevfor = formule.FormuleNumber
            });
        }

        public IEnumerable<int> GetNumerosFormules(AffaireId affaireId, int numeroRisque) {
            IEnumerable<KpOptAp> appls = null;
            if (affaireId.IsHisto) {
                appls = this.hpOptApRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.NumeroAvenant.Value);
            }
            else {
                appls = this.kpOptApRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);
            }
            return appls.Where(x => x.Kddrsq == numeroRisque).Select(x => x.Kddfor).Distinct();
        }

        public IEnumerable<Garantie> GetCurrentGaranties(string codeAffaire, int version) {
            // initialize avn on each current Garantie
            var id = this.affaireRepository.GetCurrentId(codeAffaire, version);
            var kpList = this.kpGaranRepository.GetByAffaire(AffaireType.Contrat.AsCode(), codeAffaire, version).ToList();
            kpList.ForEach(k => k.Kdeavn = id.NumeroAvenant.GetValueOrDefault());

            var gtrList = this.kpGarTarRepository.GetByAffaire(AffaireType.Contrat.AsCode(), codeAffaire, version).ToList();
            var garapList = this.kpGarApRepository.GetByAffaire(AffaireType.Contrat.AsCode(), codeAffaire, version).ToList();
            var refGars = this.paramRepository.GetAllGaranties().ToDictionary(x => x.CodeGarantie);
            var tarifs = gtrList.ToLookup(t => (t.Kdgkdeid, t.Kdgavn ?? id.NumeroAvenant.GetValueOrDefault()));
            var apsGar = garapList.ToLookup(a => (a.Kdfkdeid, a.Kdfavn ?? id.NumeroAvenant.GetValueOrDefault()));

            return kpList.Select(x => new Garantie {
                Id = x.Kdeid,
                Caractere = x.Kdecar.AsEnum<CaractereSelection>(),
                CodeGarantie = x.Kdegaran,
                NumeroAvenant = x.Kdeavn,
                NumeroAvenantCreation = x.Kdecravn,
                NumeroAvenantModif = x.Kdemajavn,
                Nature = x.Kdenat.AsEnum<NatureValue>(),
                NatureRetenue = x.Kdegan.AsEnum<NatureValue>(),
                DateDebut = MakeNullableDateTime(x.Kdedatdeb, x.Kdeheudeb, true),
                DateFinDeGarantie = MakeNullableDateTime(x.Kdedatfin, x.Kdeheufin, true),
                Duree = string.IsNullOrWhiteSpace(x.Kdeduruni) ? default(int?) : x.Kdeduree,
                DureeUnite = this.refRepo.GetValue<UniteDuree>(x.Kdeduruni),
                Tarif = tarifs[(x.Kdeid, x.Kdeavn.Value)].Select(tar => new TarifGarantie {
                    PrimeValeur = new ValeursOptionsTarif {
                        Base = this.refRepo.GetValue<BasePrime>(tar.Kdgpribase),
                        Unite = this.refRepo.GetValue<UnitePrime>(tar.Kdgpriunit),
                        ValeurActualise = tar.Kdgprivala.AsDecimal(),
                        ValeurOrigine = tar.Kdgprivalo.AsDecimal(),
                        ValeurTravail = tar.Kdgprivalw.AsDecimal(),
                        IsModifiable = tar.Kdgprimod.AsBool(),
                        IsObligatoire = tar.Kdgpriobl.AsNullableBool()
                    },
                    PrimeProvisionnelle = tar.Kdgprimpro.AsDecimal()
                }).FirstOrDefault(),
                Formule = x.Kdefor,
                ParamGarantie = new ParamGarantieHierarchie {
                    Niveau = x.Kdeniveau,
                    ParamGarantie = refGars[x.Kdegaran],
                    Sequence = x.Kdeseq
                },
                Portees = apsGar[(x.Kdeid, x.Kdeavn.Value)].Select(a => new PorteeGarantie() {
                    Id = a.Kdfid,
                    Action = a.Kdfgan.AsEnum<ActionValue>(),
                    CodeAction = a.Kdfgan,
                    GarantieId = a.Kdfkdeid,
                    NumObjet = a.Kdfobj,
                    RisqueId = a.Kdfrsq
                }).ToList()
            }).ToList();
        }

        public IEnumerable<Garantie> GetGarantiesFullHisto(string codeAffaire, int version)
        {
            // initialize avn on each current Garantie
            var id = this.affaireRepository.GetCurrentId(codeAffaire, version);
            var kpList = this.kpGaranRepository.GetByAffaire(AffaireType.Contrat.AsCode(), codeAffaire, version).ToList();
            kpList.ForEach(k => k.Kdeavn = id.NumeroAvenant.GetValueOrDefault());

            var garList = this.hpGaranRepository.GetByIpbAlx(codeAffaire, version).ToList();
            garList.AddRange(kpList);
            var gtrList = this.hpGarTarRepository.GetByIpbAlx(codeAffaire, version).ToList();
            gtrList.AddRange(this.kpGarTarRepository.GetByAffaire(AffaireType.Contrat.AsCode(), codeAffaire, version));
            var garapList = this.hpGarApRepository.GetByIpbAlx(codeAffaire, version).ToList();
            garapList.AddRange(this.kpGarApRepository.GetByAffaire(AffaireType.Contrat.AsCode(), codeAffaire, version));
            var refGars = this.paramRepository.GetAllGaranties().ToDictionary(x => x.CodeGarantie);
            var tarifs = gtrList.ToLookup(t => (t.Kdgkdeid, t.Kdgavn ?? id.NumeroAvenant.GetValueOrDefault()));
            var apsGar = garapList.ToLookup(a => (a.Kdfkdeid, a.Kdfavn ?? id.NumeroAvenant.GetValueOrDefault()));

            return garList.Select(x => new Garantie
            {
                Id = x.Kdeid,
                Caractere = x.Kdecar.AsEnum<CaractereSelection>(),
                CodeGarantie = x.Kdegaran,
                NumeroAvenant = x.Kdeavn,
                NumeroAvenantCreation = x.Kdecravn,
                NumeroAvenantModif = x.Kdemajavn,
                Nature = x.Kdenat.AsEnum<NatureValue>(),
                NatureRetenue = x.Kdegan.AsEnum<NatureValue>(),
                DateDebut = MakeNullableDateTime(x.Kdedatdeb, x.Kdeheudeb, true),
                DateFinDeGarantie = MakeNullableDateTime(x.Kdedatfin, x.Kdeheufin, true),
                Duree = string.IsNullOrWhiteSpace(x.Kdeduruni) ? default(int?) : x.Kdeduree,
                DureeUnite = this.refRepo.GetValue<UniteDuree>(x.Kdeduruni),
                Tarif = tarifs[(x.Kdeid, x.Kdeavn.Value)].Select(tar => new TarifGarantie
                {
                    PrimeValeur = new ValeursOptionsTarif
                    {
                        Base = this.refRepo.GetValue<BasePrime>(tar.Kdgpribase),
                        Unite = this.refRepo.GetValue<UnitePrime>(tar.Kdgpriunit),
                        ValeurActualise = tar.Kdgprivala.AsDecimal(),
                        ValeurOrigine = tar.Kdgprivalo.AsDecimal(),
                        ValeurTravail = tar.Kdgprivalw.AsDecimal(),
                        IsModifiable = tar.Kdgprimod.AsBool(),
                        IsObligatoire = tar.Kdgpriobl.AsNullableBool()
                    },
                    PrimeProvisionnelle = tar.Kdgprimpro.AsDecimal()
                }).FirstOrDefault(),
                Formule = x.Kdefor,
                ParamGarantie = new ParamGarantieHierarchie
                {
                    Niveau = x.Kdeniveau,
                    ParamGarantie = refGars[x.Kdegaran],
                    Sequence = x.Kdeseq
                },
                Portees = apsGar[(x.Kdeid, x.Kdeavn.Value)].Select(a => new PorteeGarantie()
                {
                    Id = a.Kdfid,
                    Action = a.Kdfgan.AsEnum<ActionValue>(),
                    CodeAction = a.Kdfgan,
                    GarantieId = a.Kdfkdeid,
                    NumObjet = a.Kdfobj,
                    RisqueId = a.Kdfrsq
                }).ToList()
            }).ToList();
        }

        private static void UpdateFormuleMetadata(string username, KpFor formUpt)
        {
            formUpt.Kdamajd = DateTime.Now.AsDateNumber();
            formUpt.Kdamaju = username;
        }

        /// <summary>
        /// Defines whether a Garantie has to be saved in DB
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private static bool MustUpdateGarantie(Garantie g)
        {
            return g.IsChecked || g.Caractere == CaractereSelection.Propose || g.ParamIsNatModifiable.GetValueOrDefault();
        }

        private void SaveOptionInternal(Option optionFromDb, Formule formuleToSave, string username, RawDataSingle data, Option opt)
        {
            var optToUpdate = data.options.GetValueOrDefault(opt.Id);
            if (optToUpdate == null) {
                var optUpdated = new KpOpt();
                this.UpdateOption(optUpdated, opt);
                this.UpdateOptionMetadata(formuleToSave, optUpdated, true, username);
                this.kpOptRepository.Insert(optUpdated);
                opt.Id = optUpdated.Kdbid;
            }
            else {
                var optUpdated = new KpOpt(optToUpdate);
                this.UpdateOption(optUpdated, opt);
                if (!KpOpt.Equals(optUpdated, optToUpdate)) {
                    this.UpdateOptionMetadata(formuleToSave, optUpdated, false, username);
                    this.kpOptRepository.Update(optUpdated);
                }
            }
            UpdateApplications(formuleToSave, username, data, opt);
            foreach (var volet in opt.OptionVolets.Where(y => y.IsChecked || y.Caractere == CaractereSelection.Propose)) {
                var voltDb = optionFromDb?.OptionVolets.FirstOrDefault(x => x.Id == volet.Id);
                SaveVoletInternal(formuleToSave, username, data, opt, volet, voltDb);
            }
            var toDelete = optionFromDb?.OptionVolets.Where(x => !opt.OptionVolets.Any(y => (y.IsChecked || y.Caractere == CaractereSelection.Propose) && x.Id == y.Id));
            foreach (var volet in toDelete ?? Enumerable.Empty<OptionVolet>()) {
                CleanUp(volet, true);
            }
            foreach (var kpbloc in data.optiondetails.Values.Where(x => x.Kdckdbid == opt.Id && x.Kdcteng == "V" && !opt.OptionVolets.Any(y => y.Id == x.Kdcid))) {
                this.kpOptDRepository.Delete(kpbloc);
            }
            this.kpOptRepository.UpdateDependentData(formuleToSave, opt.OptionNumber, username);
        }

        private void UpdateApplications(Formule formuleToSave, string username, RawDataSingle data, Option opt)
        {
            var apps = opt.Applications;
            foreach (var app in opt.Applications) {
                var adbRef = data.applications[opt.Id].FirstOrDefault(a => app.Id == a.Kddid);
                KpOptAp adb;
                if (adbRef == null) {
                    adb = new KpOptAp() {
                        Kddipb = formuleToSave.AffaireId.CodeAffaire,
                        Kddtyp = formuleToSave.AffaireId.TypeAffaire.AsCode(),
                        Kddalx = formuleToSave.AffaireId.NumeroAliment,
                        Kddavn = formuleToSave.AffaireId.NumeroAvenant,
                        Kddhin = 0,
                        Kddfor = formuleToSave.FormuleNumber,
                        Kddopt = opt.OptionNumber,
                        Kddperi = app.Niveau.AsString(),
                        Kddkdbid = opt.Id,
                        Kddobj = app.NumObjet,
                        Kddrsq = app.NumRisque
                    };
                    SetApplicationsMetadata(username, adb);
                }
                else {
                    adb = new KpOptAp(adbRef);
                }
                adb.Kddobj = app.NumObjet;
                adb.Kddrsq = app.NumRisque;
                if (adb.Kddid == 0) {
                    kpOptApRepository.Insert(adb);
                }
                else if (!Equals(adb, adbRef)) {
                    SetApplicationsMetadata(username, adb);
                    kpOptApRepository.Update(adb);
                }
                var appToRemove = data.applications[opt.Id].Where(a => !opt.Applications.Any(x => x.Id == a.Kddid));
                foreach (var adbToRemove in appToRemove) {

                    kpOptApRepository.Delete(adbToRemove);
                }
            }
        }

        private static void SetApplicationsMetadata(string username, KpOptAp adb)
        {
            adb.Kddcrd = (int)DateTime.Now.AsDateNumber();
            adb.Kddcru = username;
            adb.Kddmajd = (int)DateTime.Now.AsDateNumber();
            adb.Kddmaju = username;
        }

        private void SaveVoletInternal(Formule formuleToSave, string username, RawDataSingle data, Option opt, OptionVolet volet, OptionVolet voletFromDb)
        {
            var volToUpdate = data.optiondetails.GetValueOrDefault(volet.Id);
            if (volToUpdate == null) {
                var volUpdated = new KpOptD();
                UpdateOptDetMetadata(volet, formuleToSave, opt, volUpdated, username);
                this.UpdateOptionVoletBloc(volet, formuleToSave, volUpdated, opt, username);
                this.kpOptDRepository.Insert(volUpdated);
                volet.Id = volUpdated.Kdcid;
            }
            else {
                var volUpdated = new KpOptD(volToUpdate);
                this.UpdateOptionVoletBloc(volet, formuleToSave, volUpdated, opt, username);
                if (!Equals(volToUpdate, volUpdated)) {
                    UpdateOptDetMetadata(volet, formuleToSave, opt, volUpdated, username);
                    this.kpOptDRepository.Update(volUpdated);
                }

            }
            foreach (var bloc in volet.Blocs.Where(y => y.IsChecked || y.Caractere == CaractereSelection.Propose)) {
                var blocDb = voletFromDb?.Blocs.FirstOrDefault(x => x.Id == bloc.Id);
                SaveBlocInternal(volet.IsChecked, formuleToSave, username, data, opt, bloc, blocDb);
            }

            var toDelete = voletFromDb?.Blocs.Where(x => !volet.Blocs.Any(y => volet.IsChecked && (y.IsChecked || y.Caractere == CaractereSelection.Propose) && x.Id == y.Id));

            foreach (var bloc in toDelete ?? Enumerable.Empty<OptionBloc>()) {
                CleanUp(bloc, true);
            }
            foreach (var kpbloc in data.optiondetails.Values.Where(x => x.Kdcopt == opt.OptionNumber && x.Kdcteng == "B" && volet.ParamVolet.VoletId == x.Kdckakid && !volet.Blocs.Any(y => y.Id == x.Kdcid))) {
                this.kpOptDRepository.Delete(kpbloc);
            }
        }

        private void SaveBlocInternal(bool isParentChecked, Formule formuleToSave, string username, RawDataSingle data, Option opt, OptionBloc bloc, OptionBloc blocDb)
        {
            var blocToUpdate = data.optiondetails.GetValueOrDefault(bloc.Id);
            if (blocToUpdate == null) {
                var blocUpdated = new KpOptD();
                UpdateOptDetMetadata(bloc, formuleToSave, opt, blocUpdated, username);
                this.UpdateOptionVoletBloc(bloc, formuleToSave, blocUpdated, opt, username);
                this.kpOptDRepository.Insert(blocUpdated);
                bloc.Id = blocUpdated.Kdcid;

            }
            else {
                var blocUpdated = new KpOptD(blocToUpdate);

                this.UpdateOptionVoletBloc(bloc, formuleToSave, blocUpdated, opt, username);
                if (!blocUpdated.Equals(blocToUpdate)) {
                    UpdateOptDetMetadata(bloc, formuleToSave, opt, blocUpdated, username);
                    this.kpOptDRepository.Update(blocUpdated);
                }
            }
            var garanties = bloc.Garanties;
            foreach (var gar in garanties.Where(y => isParentChecked && bloc.IsChecked && MustUpdateGarantie(y))) {
                var rootId = gar.ParamGarantie.Sequence;
                var parentId = 0;

                var garDb = blocDb?.Garanties.FirstOrDefault(x => x.Id == gar.Id);

                SaveGarantieInternal(isParentChecked && bloc.IsChecked, formuleToSave, username, data, opt, gar, garDb, rootId, parentId, bloc.Id);
            }

            var toDelete = blocDb?.Garanties.Where(x => !bloc.Garanties.Any(y => isParentChecked && bloc.IsChecked && MustUpdateGarantie(y) && x.Id == y.Id));

            foreach (var gar in toDelete ?? Enumerable.Empty<Garantie>()) {
                CleanUp(gar, true);
            }
            foreach (var kpgar in data.gars.Select(x => x.Value).Where(x => x.Kdesem == 0 && x.Kdekdcid == bloc.Id && !garanties.Any(y => y.Id == x.Kdeid))) {
                this.kpGaranRepository.Delete(kpgar);
            }
        }

        private void SaveGarantieInternal(bool parentChecked, Formule formuleToSave, string username, RawDataSingle data, Option opt, Garantie gar, Garantie garFromDb, long rootId, long parentId, long blocId)
        {
            bool creationInventaire = (gar.Inventaire != null && gar.Inventaire.Items.Any() && gar.Inventaire.Id <= 0);
            KpGaran garToUpdate = data.gars.GetValueOrDefault(gar.Id);
            if (gar.LienKPINVEN != 0 && (gar.Inventaire == null || !gar.Inventaire.Items.Any())) {
                this.inventaireRepository.DeleteInventaire(gar.LienKPINVEN);
            }
            else if (gar.Inventaire != null && gar.Inventaire.Items.Any()) {
                gar.Inventaire.Affaire = formuleToSave.AffaireId;
                if (gar.Inventaire.Id < 0) {
                    gar.Inventaire.Id = 0;
                }

                this.inventaireRepository.SaveInventaire(gar.Inventaire);
                gar.LienKPINVEN = gar.Inventaire.Id;
            }

            KpGaran garUpdated;
            if (garToUpdate is null) {
                garUpdated = new KpGaran();
                UpdateGarantie(gar, garUpdated, formuleToSave, opt, username, rootId, parentId, blocId);
                this.kpGaranRepository.Insert(garUpdated);
                gar.Id = garUpdated.Kdeid;
            }
            else {
                garUpdated = new KpGaran(garToUpdate);
                UpdateGarantie(gar, garUpdated, formuleToSave, opt, username, rootId, parentId, blocId);
                if (!Equals(garToUpdate, garUpdated)) {
                    UpdateMetaDataGarantie(garUpdated, username);
                    this.kpGaranRepository.Update(garUpdated);
                }
            }
            if (creationInventaire) {
                this.inventaireRepository.SaveInventaireApplication(gar.Inventaire, gar, formuleToSave.FormuleNumber);
            }
            // sauvegarde des portées
            foreach (var portee in gar.Portees) {
                SavePorteeInternal(portee, username, data.portees.GetValueOrDefault(portee.Id), garUpdated);
            }

            // sauvegarde des conditions tarifaires
            SaveGararantieTarifInternal(data, gar, garUpdated, formuleToSave.AffaireId);

            foreach (var subgar in gar.SousGaranties.Where(sg => parentChecked && gar.IsChecked && MustUpdateGarantie(sg))) {
                var subgarFromDb = garFromDb?.SousGaranties.FirstOrDefault(x => x.Id == subgar.Id);
                SaveGarantieInternal(parentChecked && gar.IsChecked, formuleToSave, username, data, opt, subgar, subgarFromDb, rootId, gar.ParamGarantie.Sequence, blocId);
            }

            foreach (var subkpgar in data.gars.Select(x => x.Value).Where(x => x.Kdesem == gar.ParamGarantie.Sequence && !gar.SousGaranties.Any(y => y.Id == x.Kdeid))) {
                this.kpGaranRepository.Delete(subkpgar);
            }
            foreach (var subkpgarap in data.portees.Select(x => x.Value).Where(x => x.Kdfkdeid == garUpdated.Kdeid && !gar.Portees.Any(y => y.Id == x.Kdfid))) {
                this.kpGarApRepository.Delete(subkpgarap);
            }

            var toDelete = garFromDb?.SousGaranties.Where(x => !gar.SousGaranties.Any(y => parentChecked && gar.IsChecked && MustUpdateGarantie(y) && x.Id == y.Id));
            foreach (var subGar in toDelete ?? Enumerable.Empty<Garantie>()) {
                CleanUp(subGar, true);
            }

        }

        private void SavePorteeInternal(PorteeGarantie portee, string username, KpGarAp porteeToUpdate, KpGaran gar)
        {
            KpGarAp porteeUpdated;
            if (porteeToUpdate == null) {
                porteeUpdated = new KpGarAp();
                UpdatePortee(portee, username, gar, porteeUpdated);
                this.kpGarApRepository.Insert(porteeUpdated);
                portee.Id = porteeUpdated.Kdfid;
            }
            else {
                porteeUpdated = new KpGarAp(porteeToUpdate);
                UpdatePortee(portee, username, gar, porteeUpdated);
                if (!Equals(porteeToUpdate, porteeUpdated)) {
                    this.kpGarApRepository.Update(porteeUpdated);
                }
            }
        }

        private void UpdatePortee(PorteeGarantie portee, string username, KpGaran gar, KpGarAp porteeUpdated)
        {

            porteeUpdated.Kdfmnt = portee.Montant;
            porteeUpdated.Kdfpra = portee.ValeursPrime.ValeurActualise;
            porteeUpdated.Kdfpru = portee.ValeursPrime.Unite;
            porteeUpdated.Kdfprw = portee.ValeursPrime.ValeurTravail;
            porteeUpdated.Kdfprv = portee.ValeursPrime.ValeurOrigine;
            porteeUpdated.Kdftyc = portee.TypeCalcul.AsString();
            porteeUpdated.Kdfgan = portee.Action.AsString();
            porteeUpdated.Kdfmaju = username;
            porteeUpdated.Kdfmajd = DateTime.Now.AsDateNumber();

            if (portee.Id == 0) {
                // case of creation
                porteeUpdated.Kdfopt = gar.Kdeopt;
                porteeUpdated.Kdffor = gar.Kdefor;
                porteeUpdated.Kdfid = this.kpGarApRepository.NewId();
                porteeUpdated.Kdfobj = portee.NumObjet;
                porteeUpdated.Kdfrsq = portee.RisqueId;
                porteeUpdated.Kdfalx = gar.Kdealx;
                porteeUpdated.Kdfavn = gar.Kdeavn;
                porteeUpdated.Kdfipb = gar.Kdeipb;
                porteeUpdated.Kdftyp = gar.Kdetyp;
                porteeUpdated.Kdfgaran = gar.Kdegaran;
                porteeUpdated.Kdfkdeid = gar.Kdeid;
                porteeUpdated.Kdfcru = username;
                porteeUpdated.Kdfcrd = DateTime.Now.AsDateNumber();
            }
        }

        private void SaveGararantieTarifInternal(RawDataSingle data, Garantie gar, KpGaran garUpdated, AffaireId affId)
        {
            SaveExpressionComplexes(data, gar, affId);
            IDictionary<long, KpGarTar> gartars = data.gartars;
            var garTarToUpdate = gartars.GetValueOrDefault(gar.Tarif.Id);
            KpGarTar garTarUpdated;
            if (garTarToUpdate == null) {
                garTarUpdated = new KpGarTar();
                this.UpdateTarif(gar.Tarif, garTarUpdated, garUpdated);
                this.kpGarTarRepository.Insert(garTarUpdated);

            }
            else {
                garTarUpdated = new KpGarTar(garTarToUpdate);
                this.UpdateTarif(gar.Tarif, garTarUpdated, garUpdated);
                if (!Equals(garTarToUpdate, garTarUpdated)) {
                    this.kpGarTarRepository.Update(garTarUpdated);
                }
            }
            foreach (var toDelete in gartars.Select(x => x.Value).Where(x => x.Kdgkdeid == gar.Id && x.Kdgid != garTarUpdated.Kdgid)) {
                this.kpGarTarRepository.Delete(toDelete);
            }
        }

        private void SaveExpressionComplexes(RawDataSingle data, Garantie gar, AffaireId affId)
        {
            var tar = gar.Tarif;

            var lci = tar.LCI.ExpressionComplexe;
            if ((lci == null && tar.LCI.Unite.Code == "CPX") || (lci != null && lci.Code != tar.LCI.Base.Code))
            {
                var expression = data.paramRepo.GetExpressionLCIByBase(tar.LCI.Base.Code);
                if (expression != null)
                {
                    var newLci = new ExpressionComplexeLCI()
                    {
                        Id = 0,
                        Description = expression.Description,
                        Code = expression.Code,
                        Designation = expression.Designation,
                        Details = expression.Details.Cast<ParamExpressionComplexeDetailLCI>().Select(InitExpDetLCI).Cast<ExpressionComplexeDetailBase>().ToList(),
                        Origine = OrigineExpression.Referentiel,
                        IsModifiable = expression.IsModifiable
                    };
                    lci = newLci;
                    gar.Tarif.LCI.ExpressionComplexe = newLci;
                }
            }
            SaveExprLci(data, gar, affId, (ExpressionComplexeLCI)lci);

            var frh = tar.Franchise.ExpressionComplexe;
            if ((frh == null && tar.Franchise.Unite.Code == "CPX") || (frh != null && frh.Code != tar.Franchise.Base.Code))
            {
                var expression = data.paramRepo.GetExpressionFranchiseByBase(tar.Franchise.Base.Code);
                if (expression != null)
                {
                    var newFrh = new ExpressionComplexeFranchise()
                    {
                        Id = 0,
                        Description = expression.Description,
                        Code = expression.Code,
                        Designation = expression.Designation,
                        Details = expression.Details.Cast<ParamExpressionComplexeDetailFranchise>().Select(InitExpDetFrh).Cast<ExpressionComplexeDetailBase>().ToList(),
                        Origine = OrigineExpression.Referentiel,
                        IsModifiable = expression.IsModifiable,
                    };
                    frh = newFrh;
                    gar.Tarif.Franchise.ExpressionComplexe = newFrh;
                }
            }
            SaveExprFrh(data, gar, affId, (ExpressionComplexeFranchise)frh);

        }
        private static ExpressionComplexeDetailLCI InitExpDetLCI(ParamExpressionComplexeDetailLCI arg)
        {
            return new ExpressionComplexeDetailLCI
            {
                CodeBase = arg.CodeBase,
                ExprId = 0,
                Id = 0,
                Ordre = arg.Ordre,
                Unite = arg.Unite,
                Valeur = arg.Valeur,
                ConcurrentCodeBase = arg.ConcurrentCodeBase,
                ConcurrentUnite = arg.ConcurrentUnite,
                ConcurrentValeur = arg.ConcurrentValeur
            };
        }
        private static ExpressionComplexeDetailFranchise InitExpDetFrh(ParamExpressionComplexeDetailFranchise arg)
        {
            return new ExpressionComplexeDetailFranchise
            {
                CodeBase = arg.CodeBase,
                CodeIncide = arg.CodeIncide,
                ExprId = 0,
                Id = 0,
                IncideValeur = arg.IncideValeur,
                LimiteDebut = arg.LimiteDebut,
                LimiteFin = arg.LimiteFin,
                Ordre = arg.Ordre,
                Unite = arg.Unite,
                Valeur = arg.Valeur,
                ValeurMax = arg.ValeurMax,
                ValeurMin = arg.ValeurMin
            };
        }

        internal int UpdateDesi(string value, AffaireId affId, int desiChrono, string type, IDictionary<int, string> desis)
        {
            var desi = desis.GetValueOrDefault(desiChrono);
            if (!(string.IsNullOrEmpty(value) && string.IsNullOrEmpty(desi) || desi == value)) {
                return this.desiRepository.UpdateDesi(value, affId, desiChrono, type);
            }
            return desiChrono;
        }
        private void SaveExprFrh(RawDataSingle data, Garantie gar, AffaireId affId, ExpressionComplexeFranchise frh)
        {
            if (frh != null) {
                KpExpFrh frhref = FindRef(data, frh);
                KpExpFrh frhUpt;
                if (frhref == null) {
                    frhUpt = InitFrh(affId);
                }
                else {
                    //copy
                    frhUpt = new KpExpFrh(frhref);
                }

                UpdateData(gar, frhUpt);

                frhUpt.Kdkdesi = UpdateDesi(gar.Tarif.Franchise.ExpressionComplexe.Designation, affId, (int)frhUpt.Kdkdesi, "GA", data.desis);

                if (frhUpt != frhref) {
                    if (frhref == null && frhUpt.Kdkid == 0) {
                        kpExpFrhRepo.Insert(frhUpt);
                        frh.Id = frhUpt.Kdkid;
                        if (frhref == null) { data.exprDbFrh.Add(frh.Id, frhUpt); };
                        data.exprFrh.Add(frh.Id, frh);
                    }
                    else if (frhref != null && !Equals(frhUpt, frhref)) {
                        kpExpFrhRepo.Update(frhUpt);
                    }
                }
                if (frhref == null || frhref.Kdkid == frhUpt.Kdkid) {
                    foreach (var detail in frh.Details.OfType<ExpressionComplexeDetailFranchise>()) {
                        SaveFranchiseDetail(data, affId, frhUpt, detail);
                    }
                }
                frh.Id = frhUpt.Kdkid;
            }
        }

        private static void UpdateData(Garantie gar, KpExpFrh frhUpt)
        {
            frhUpt.Kdkdesc = gar.Tarif.Franchise.ExpressionComplexe.Description;
            frhUpt.Kdkmodi = gar.Tarif.Franchise.ExpressionComplexe.IsModifiable.ToYesNo();
            frhUpt.Kdkori = gar.Tarif.Franchise.ExpressionComplexe.Origine.AsString();
            frhUpt.Kdkfhe = gar.Tarif.Franchise.ExpressionComplexe.Code;
        }

        private void SaveFranchiseDetail(RawDataSingle data, AffaireId affId, KpExpFrh frhUpt, ExpressionComplexeDetailFranchise detail)
        {
            var detRef = data.exprDbFrhD.GetValueOrDefault(detail.Id);
            KpExpFrhD det;
            if (detRef == null) {
                //init
                det = new KpExpFrhD {
                    Kdlavn = affId.NumeroAvenant,
                    Kdlhin = 0,
                    Kdlkdkid = frhUpt.Kdkid,
                    Kdlid = 0,

                };
            }
            else {
                //copy
                det = new KpExpFrhD(detRef);
            }

            UpdateData(detail, frhUpt, det);

            if (det != detRef) {
                if (det.Kdlid == 0) {
                    kpExpFrhDRepo.Insert(det);
                    detail.Id = det.Kdlid;
                    data.exprDbFrhD.Add(det.Kdlid, det);

                }
                else if (!Equals(det, detRef)) {
                    kpExpFrhDRepo.Update(det);
                }

            }
        }

        private static void UpdateData(ExpressionComplexeDetailFranchise detail, KpExpFrh frhUpt, KpExpFrhD det)
        {
            det.Kdlfhbase = detail.CodeBase?.Code ?? string.Empty;
            det.Kdlfhval = detail.Valeur;
            det.Kdlfhvau = detail.Unite?.Code ?? string.Empty;
            det.Kdlfhmini = detail.ValeurMin;
            det.Kdlfhmaxi = detail.ValeurMax;
            det.Kdlivo = detail.IncideValeur;
            det.Kdlkdkid = frhUpt.Kdkid;
            det.Kdlind = detail.CodeIncide?.Code ?? string.Empty;
            det.Kdllimdeb = detail.LimiteDebut.AsDateNumber();
            det.Kdllimfin = detail.LimiteFin.AsDateNumber();
        }

        private static KpExpFrh InitFrh(AffaireId affId) =>
            new KpExpFrh {
                Kdkalx = affId.NumeroAliment,
                Kdkavn = affId.NumeroAvenant,
                Kdkipb = affId.CodeAffaire,
                Kdkhin = 0,
                Kdktyp = affId.TypeAffaire.AsCode(),
                Kdkdesi = 0,
                Kdkid = 0,
            };

        private void SaveExprLci(RawDataSingle data, Garantie gar, AffaireId affId, ExpressionComplexeLCI lci)
        {
            if (lci != null) {
                KpExpLCI lciref = FindRef(data, lci);
                KpExpLCI lciUpt;
                if (lciref == null) {
                    lciUpt = InitLci(affId);
                }
                else {
                    //copy 
                    lciUpt = new KpExpLCI(lciref);
                }

                UpdateData(gar, lciUpt);

                lciUpt.Kdidesi = UpdateDesi(gar.Tarif.LCI.ExpressionComplexe.Designation, affId, (int)lciUpt.Kdidesi, "GA", data.desis);


                if (lciUpt != lciref) {
                    if (lciref == null && lciUpt.Kdiid == 0) {
                        kpExpLciRepo.Insert(lciUpt);
                        lci.Id = lciUpt.Kdiid;
                        if (lciref == null) { data.exprDbLci.Add(lci.Id, lciUpt); };
                        data.exprLci.Add(lci.Id, lci);
                    }
                    else if (lciref != null && !KpExpLCI.Equals(lciUpt, lciref)) {
                        kpExpLciRepo.Update(lciUpt);
                    }
                }
                if (lciref == null || lciref.Kdiid == lciUpt.Kdiid) {
                    foreach (var detail in lci.Details.OfType<ExpressionComplexeDetailLCI>()) {
                        SaveLciDetail(data, affId, lciUpt, detail);
                    }
                }
                lci.Id = lciUpt.Kdiid;

            }
        }

        private void UpdateData(Garantie gar, KpExpLCI lciUpt)
        {
            lciUpt.Kdimodi = gar.Tarif.LCI.ExpressionComplexe.IsModifiable.ToYesNo();
            lciUpt.Kdidesc = gar.Tarif.LCI.ExpressionComplexe.Description;
            lciUpt.Kdiori = gar.Tarif.LCI.ExpressionComplexe.Origine.AsString();
            lciUpt.Kdilce = gar.Tarif.LCI.ExpressionComplexe.Code;
        }

        private void SaveLciDetail(RawDataSingle data, AffaireId affId, KpExpLCI lciUpt, ExpressionComplexeDetailLCI detail)
        {
            var detRef = data.exprDbLciD.GetValueOrDefault(detail.Id);
            KpExpLCID det;
            if (detRef == null) {
                //init
                det = new KpExpLCID {
                    Kdjavn = affId.NumeroAvenant,
                    Kdjhin = 0,
                    Kdjid = 0
                };
            }
            else {
                //copy
                det = new KpExpLCID(detRef);
            }

            UpdateData(detail, lciUpt, det);

            if (det != detRef) {
                if (det.Kdjid == 0) {
                    kpExpLciDRepo.Insert(det);
                    detail.Id = det.Kdjid;
                    data.exprDbLciD.Add(det.Kdjid, det);
                }
                else if (!KpExpLCID.Equals(det, detRef)) {
                    kpExpLciDRepo.Update(det);
                }

            }
        }

        private static void UpdateData(ExpressionComplexeDetailLCI detail, KpExpLCI lciUpt, KpExpLCID det)
        {
            det.Kdjlcbase = detail.CodeBase?.Code ?? string.Empty;
            det.Kdjlobase = detail.ConcurrentCodeBase?.Code ?? string.Empty;
            det.Kdjlcval = detail.Valeur;
            det.Kdjloval = detail.ConcurrentValeur;
            det.Kdjlcvau = detail.Unite?.Code ?? string.Empty;
            det.Kdjlovau = detail.ConcurrentUnite?.Code ?? string.Empty;
            det.Kdjordre = detail.Ordre;
            det.Kdjkdiid = lciUpt.Kdiid;

        }

        private static KpExpLCI InitLci(AffaireId affId) =>
            new KpExpLCI {
                Kdialx = affId.NumeroAliment,
                Kdiavn = affId.NumeroAvenant,
                Kdiipb = affId.CodeAffaire,
                Kdihin = 0,
                Kdityp = affId.TypeAffaire.AsCode(),
                Kdidesc = "",
                Kdidesi = 0,
                Kdiid = 0
            };

        private static KpExpFrh FindRef(RawDataSingle data, ExpressionComplexeFranchise frh)
            => data.exprDbFrh.GetValueOrDefault(frh.Id)
               // if not found try getting for the same code
               ?? data.exprDbFrh.FirstOrDefault(x => x.Value.Kdkfhe == frh.Code).Value;


        private static KpExpLCI FindRef(RawDataSingle data, ExpressionComplexeLCI lci)
            => data.exprDbLci.GetValueOrDefault(lci.Id)
                // if not found try getting for the same code
                ?? data.exprDbLci.FirstOrDefault(x => x.Value.Kdilce == lci.Code).Value;


        private void UpdateOptionMetadata(Formule formule, KpOpt optUpt, bool creation, string username)
        {
            DateTime now = DateTime.Now;
            AffaireId id = formule.AffaireId;
            optUpt.Kdbfor = formule.FormuleNumber;
            optUpt.Kdbkdaid = formule.Id;
            optUpt.Kdbmaju = username;
            (optUpt.Kdbmajd, optUpt.Kdbmajh) = now.AsDateHour();
            if (optUpt.Kdbcrd == 0) {
                optUpt.Kdbcru = username;
                (optUpt.Kdbcrd, optUpt.Kdbcrh) = now.AsDateHour();
                optUpt.Kdbalx = id.NumeroAliment;
                optUpt.Kdbavn = id.NumeroAvenant;
                optUpt.Kdbipb = id.CodeAffaire;
                optUpt.Kdbtyp = id.TypeAffaire.AsCode();
            }
        }
        private void UpdateOption(KpOpt optUpt, Option opt)
        {
            optUpt.Kdbid = opt.Id;
            optUpt.Kdbavn = opt.NumeroAvenant;
            optUpt.Kdbopt = opt.OptionNumber;
            optUpt.Kdbpaq = opt.MontantsOption.IsMontantAcquis.ToYesNo();
            optUpt.Kdbacq = opt.MontantsOption.Montantacquis;
            optUpt.Kdbtmc = opt.MontantsOption.TotalCalculeRef;
            optUpt.Kdbtff = opt.MontantsOption.TotalMontantForceRef;
            optUpt.Kdbtfp = opt.MontantsOption.TotalCoefcalcul;
            optUpt.Kdbpro = opt.MontantsOption.IsMontantprovisionnel.ToYesNo();
            optUpt.Kdbtmi = opt.MontantsOption.IsMontantForcepourMini.ToYesNo();
            optUpt.Kdbtfm = opt.MontantsOption.MotifTotalforce ?? string.Empty;
            optUpt.Kdbcmc = opt.MontantsOption.ComptantMontantCalcule;
            optUpt.Kdbcfo = opt.MontantsOption.IsComptantMontantForce.ToYesNo();
            optUpt.Kdbcht = opt.MontantsOption.ComptantMontantForceHT;
            optUpt.Kdbctt = opt.MontantsOption.ComptantMontantForceTTC;
            optUpt.Kdbccp = opt.MontantsOption.CoeffCalculForceComptant;
            optUpt.Kdbval = opt.MontantsOption.ValeurOrigine;
            optUpt.Kdbvaa = opt.MontantsOption.ValeurActualisee;
            optUpt.Kdbvaw = opt.MontantsOption.Valeurdetravail;
            optUpt.Kdbvat = opt.MontantsOption.TypeDevaleur ?? string.Empty;
            optUpt.Kdbvau = opt.MontantsOption.UniteDeValeur ?? string.Empty;
            optUpt.Kdbvah = opt.MontantsOption.IsTTC.ToYesNo();
            optUpt.Kdbivo = opt.MontantsOption.ValeurIndiceOrigin;
            optUpt.Kdbiva = opt.MontantsOption.ValeurIndiceActual;
            optUpt.Kdbivw = opt.MontantsOption.ValeurIndiceTravai;
            optUpt.Kdbave = opt.NumAvenantCreation;
            optUpt.Kdbavg = opt.NumAvenantModif;
            optUpt.Kdbava = opt.DateAvenant.NYear();
            optUpt.Kdbavm = opt.DateAvenant.NMonth();
            optUpt.Kdbavj = opt.DateAvenant.NDay();
            optUpt.Kdbehh = opt.MontantsOption.ProchaineEChHT;
            optUpt.Kdbehc = opt.MontantsOption.ProchaineEchCatnat;
            optUpt.Kdbehi = opt.MontantsOption.ProchaineEchIncend;
            optUpt.Kdbasvalo = opt.MontantsOption.AssietteValOrigine;
            optUpt.Kdbasvala = opt.MontantsOption.AssietteValActual;
            optUpt.Kdbasvalw = opt.MontantsOption.AssietteValeurW;
            optUpt.Kdbasunit = opt.MontantsOption.AssietteUnite;
            optUpt.Kdbasbase = opt.MontantsOption.AssietteBaseTypeValeur;
            optUpt.Kdbger = opt.MontantsOption.MontantRefForcesaisi;
        }

        private KpFor UpdateFormule(KpFor form, Formule formuleToSave, string username)
        {

            form.Kdaid = formuleToSave.Id;
            form.Kdaalpha = formuleToSave.Alpha;
            form.Kdacch = formuleToSave.Chrono;
            form.Kdacible = formuleToSave.Cible.Cible.Code;
            form.Kdakaiid = formuleToSave.Cible.Id;
            form.Kdabra = formuleToSave.Cible.Branche.Code;
            form.Kdadesc = formuleToSave.Description;
            form.Kdafor = formuleToSave.FormuleNumber;
            form.Kdaavn = formuleToSave.AffaireId.NumeroAvenant;

            if (form.Kdaid <= 0) {
                form.Kdaid = this.kpforRepository.NewId();
                form.Kdacrd = DateTime.Now.AsDateNumber();
                form.Kdacru = username;
                form.Kdaipb = formuleToSave.AffaireId.CodeAffaire;
                form.Kdaalx = formuleToSave.AffaireId.NumeroAliment;
                form.Kdatyp = formuleToSave.AffaireId.TypeAffaire.AsCode();
            }


            return form;
        }

        public (IEnumerable<Formule> formule, Expressions expressions) GetFormulesExpressionForAffaire(AffaireId id)
        {
            var data = GetRawData(id);
            var histoData = GetHistoRawData(id, RawDataFilter.Options | RawDataFilter.GarantiesNatureModifiable);
            var forms = data.ToFormules(histoData);
            return (forms.ToList(), new Expressions(data.exprFrh.Values.Cast<ExpressionComplexeBase>().Concat(data.exprLci.Values)));
        }
        public IEnumerable<Formule> GetFormulesForAffaire(AffaireId id)
        {
            return GetFormulesExpressionForAffaire(id).formule;
        }

        public IEnumerable<Option> GetOptionsApplicationsFullHisto(string codeAffaire, int version)
        {
            var optList = this.hpOptRepository.GetByIpbAlx(codeAffaire, version);
            var optApList = this.hpOptApRepository.GetByIpbAlx(codeAffaire, version);
            return optList.Select(opt => new Option
            {
                Id = opt.Kdbid,
                OptionNumber = opt.Kdbopt,
                Formule = new Formule
                {
                    FormuleNumber = opt.Kdbfor,
                    AffaireId = new AffaireId { CodeAffaire = codeAffaire, NumeroAliment = version, TypeAffaire = AffaireType.Contrat, IsHisto = true, NumeroAvenant = opt.Kdbavn }
                },
                NumeroAvenant = opt.Kdbavn,
                Applications = optApList.Where(x => x.Kddavn == opt.Kdbavn && x.Kddkdbid == opt.Kdbid).Select(ap => new Domain.Formule.Application
                {
                    Id = ap.Kddid,
                    Niveau = ap.Kddperi.ParseCode<ApplicationNiveau>(),
                    NumObjet = ap.Kddobj,
                    NumRisque = ap.Kddrsq
                }).ToList()
            }).OrderBy(o => o.OptionNumber).ToList();
        }

        public IEnumerable<Option> GetOptionsSimple(AffaireId id)
        {
            IEnumerable<KpOpt> optList;
            var optApList = Enumerable.Empty<KpOptAp>();
            if (id.IsHisto)
            {
                optList = this.hpOptRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.GetValueOrDefault());
                if (optList.Any())
                {
                    optApList = this.hpOptApRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.GetValueOrDefault());
                }
            }
            else
            {
                optList = this.kpOptRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment);
                if (!optList.Any())
                {
                    return Enumerable.Empty<Option>();
                }
                if (id.NumeroAvenant.GetValueOrDefault(-1) < 0) {
                    id.NumeroAvenant = optList.First().Kdbavn;
                }
                optApList = this.kpOptApRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment);
            }

            return optList.Select(opt => new Option
            {
                Id = opt.Kdbid,
                OptionNumber = opt.Kdbopt,
                Formule = new Formule
                {
                    FormuleNumber = opt.Kdbfor,
                    AffaireId = id
                },
                NumeroAvenant = opt.Kdbavn,
                NumAvenantCreation = opt.Kdbave,
                NumAvenantModif = opt.Kdbavg,
                Applications = optApList.Where(x => x.Kddkdbid == opt.Kdbid).Select(ap => new Domain.Formule.Application
                {
                    Id = ap.Kddid,
                    Niveau = ap.Kddperi.ParseCode<ApplicationNiveau>(),
                    NumObjet = ap.Kddobj,
                    NumRisque = ap.Kddrsq
                }).ToList()
            }).OrderBy(o => o.OptionNumber).ToList();
        }

        public IEnumerable<OptionBloc> GetOptionsBlocsByFormule(AffaireId affaireId, int formule) {
            var optdList = affaireId.IsHisto
                ? this.hpOptDRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.NumeroAvenant.Value)
                : this.kpOptDRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment);

            var details = optdList.Where(d => d.Kdcfor == formule).Select(d => new OptionBloc {
                Id = d.Kdcid,
                NumeroAvenant = d.Kdcavn,
                Type = d.Kdcteng.ParseCode<TypeOption>(),
                Ordre = d.Kdcordre,
                IsChecked = Convert.ToBoolean(d.Kdcflag),
                ParamModele = this.paramRepository.GetParamModele((int)d.Kdckarid)
            });

            return details.ToList();
        }

        public void Reprise(AffaireId id) {
            var optList = this.kpOptRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            optList.ForEach(x => this.kpOptRepository.Delete(x));
            var apList = this.kpOptApRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            apList.ForEach(x => this.kpOptApRepository.Delete(x));
            var frmList = this.kpforRepository.Liste(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            frmList.ForEach(x => this.kpforRepository.Delete(x));
            var yfrmList = this.yprtForRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment).ToList();
            yfrmList.ForEach(x => this.yprtForRepository.Delete(x));
            var vlBlList = this.kpOptDRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            vlBlList.ForEach(x => this.kpOptDRepository.Delete(x));
            var garanList = this.kpGaranRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            garanList.ForEach(x => this.kpGaranRepository.Delete(x));
            var garapList = this.kpGarApRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            garapList.ForEach(x => this.kpGarApRepository.Delete(x));
            var gartarList = this.kpGarTarRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            gartarList.ForEach(x => this.kpGarTarRepository.Delete(x));
            var ygarList = this.yprtGarRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment).ToList();
            ygarList.ForEach(x => this.yprtGarRepository.Delete(x));
            var yfooList = this.yprtFooRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment).ToList();
            yfooList.ForEach(x => this.yprtFooRepository.Delete(x));
            var matffList = this.kpMatffRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            matffList.ForEach(x => this.kpMatffRepository.Delete(x));
            var matflList = this.kpMatflRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            matflList.ForEach(x => this.kpMatflRepository.Delete(x));
            var matfrList = this.kpMatfrRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            matfrList.ForEach(x => this.kpMatfrRepository.Delete(x));
            var matggList = this.kpMatggRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            matggList.ForEach(x => this.kpMatggRepository.Delete(x));
            var matglList = this.kpMatglRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            matglList.ForEach(x => this.kpMatglRepository.Delete(x));
            var matgrList = this.kpMatgrRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            matgrList.ForEach(x => this.kpMatgrRepository.Delete(x));
            var expLcid = this.kpExpLciDRepo.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            expLcid.ForEach(x => this.kpExpLciDRepo.Delete(x));
            var expLci = this.kpExpLciRepo.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            expLci.ForEach(x => this.kpExpLciRepo.Delete(x));
            var expFrhd = this.kpExpFrhDRepo.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            expFrhd.ForEach(x => this.kpExpFrhDRepo.Delete(x));
            var expFrh = this.kpExpFrhRepo.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            expFrh.ForEach(x => this.kpExpFrhRepo.Delete(x));

            var hapList = this.hpOptApRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hapList.ForEach(x => this.hpOptApRepository.Delete(x));
            hapList.ForEach(x => this.kpOptApRepository.Insert(x));
            var hoptList = this.hpOptRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hoptList.ForEach(x => this.hpOptRepository.Delete(x));
            hoptList.ForEach(x => this.kpOptRepository.Insert(x));
            var hfrmList = this.hpforRepository.Liste(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hfrmList.ForEach(x => this.hpforRepository.Delete(x));
            hfrmList.ForEach(x => this.kpforRepository.Insert(x));
            var hyfrmList = this.yhrtforRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hyfrmList.ForEach(x => this.yhrtforRepository.Delete(x));
            hyfrmList.ForEach(x => this.yprtForRepository.Insert(x));
            var hvlBlList = this.hpOptDRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hvlBlList.ForEach(x => this.hpOptDRepository.Delete(x));
            hvlBlList.ForEach(x => this.kpOptDRepository.Insert(x));
            var hgaranList = this.hpGaranRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hgaranList.ForEach(x => this.hpGaranRepository.Delete(x));
            hgaranList.ForEach(x => this.kpGaranRepository.Insert(x));
            var hgarapList = this.hpGarApRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hgarapList.ForEach(x => this.hpGarApRepository.Delete(x));
            hgarapList.ForEach(x => this.kpGarApRepository.Insert(x));
            var hgartarList = this.hpGarTarRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hgartarList.ForEach(x => this.hpGarTarRepository.Delete(x));
            hgartarList.ForEach(x => this.kpGarTarRepository.Insert(x));
            var hexpLci = this.hpExpLciRepo.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hexpLci.ForEach(x => this.hpExpLciRepo.Delete(x));
            hexpLci.ForEach(x => this.kpExpLciRepo.Insert(x));
            var hexpLcid = this.hpExpLciDRepo.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hexpLcid.ForEach(x => this.hpExpLciDRepo.Delete(x));
            hexpLcid.ForEach(x => this.kpExpLciDRepo.Insert(x));
            var hexpFrh = this.hpExpFrhRepo.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hexpFrh.ForEach(x => this.hpExpFrhRepo.Delete(x));
            hexpFrh.ForEach(x => this.kpExpFrhRepo.Insert(x));
            var hexpFrhd = this.hpExpFrhDRepo.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hexpFrhd.ForEach(x => this.hpExpFrhDRepo.Delete(x));
            hexpFrhd.ForEach(x => this.kpExpFrhDRepo.Insert(x));
            var hygarList = this.yhrtgarRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hygarList.ForEach(x => this.yhrtgarRepository.Delete(x));
            hygarList.ForEach(x => this.yprtGarRepository.Insert(x));
            var hyfooList = this.yhrtfooRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            this.yhrtfooRepository.DeleteForAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1);
            hyfooList.ForEach(x => this.yprtFooRepository.Insert(x));
            var hmatffList = this.hpmatffRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hmatffList.ForEach(x => this.hpmatffRepository.Delete(x));
            hmatffList.ForEach(x => this.kpMatffRepository.Insert(x));
            var hmatflList = this.hpmatflRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT,id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hmatflList.ForEach(x => this.hpmatflRepository.Delete(x));
            hmatflList.ForEach(x => this.kpMatflRepository.Insert(x));
            var hmatfrList = this.hpmatfrRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT,id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hmatfrList.ForEach(x => this.hpmatfrRepository.Delete(x));
            hmatfrList.ForEach(x => this.kpMatfrRepository.Insert(x));
            var hmatggList = this.hpmatggRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT,id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hmatggList.ForEach(x => this.hpmatggRepository.Delete(x));
            hmatggList.ForEach(x => this.kpMatggRepository.Insert(x));
            var hmatglList = this.hpmatglRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT,id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hmatglList.ForEach(x => this.hpmatglRepository.Delete(x));
            hmatglList.ForEach(x => this.kpMatglRepository.Insert(x));
            var hmatgrList = this.hpmatgrRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT,id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hmatgrList.ForEach(x => this.hpmatgrRepository.Delete(x));
            hmatgrList.ForEach(x => this.kpMatgrRepository.Insert(x));

            var frmSortis = this.kJobSortiRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, AlbConstantesMetiers.TYPE_CONTRAT).ToList();
            foreach (var group in frmSortis.Where(x => x.Avn == id.NumeroAvenant && x.Opt > 1).GroupBy(x => x.Opt)) {
                this.kJobSortiRepository.DeleteByOption(id.CodeAffaire, id.NumeroAliment, AlbConstantesMetiers.TYPE_CONTRAT, id.NumeroAvenant.Value, group.Key);
            }
            var hfrmSortis = this.hjobsortiRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            foreach (var group in hfrmSortis.Where(x => x.Opt > 1).GroupBy(x => x.Opt)) {
                this.hjobsortiRepository.DeleteByOption(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1, group.Key);
            }
            this.kJobSortiRepository.InsertMultiple(hfrmSortis.Where(x => x.Opt > 1));
        }

        private RawDataSingle GetRawData(AffaireId aff, long formuleId, int? avenant, bool isHisto) {
            var result = new RawDataSingle(this.refRepo, this.paramRepository);
            if (!isHisto) {
                var formula = this.kpforRepository.Get(formuleId);
                if (formula != null) {
                    result.formula = formula;
                    result.applications = kpOptApRepository.GetByAffaire(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment).ToLookup(x => x.Kddkdbid);
                    result.options = this.kpOptRepository.GetByFormule(formuleId).ToDictionary(x => x.Kdbid);
                    result.optiondetails = this.kpOptDRepository.GetByFormule(formuleId).ToDictionary(x => x.Kdcid);
                    result.gars = this.kpGaranRepository.GetByFormule(formuleId).ToDictionary(x => x.Kdeid);
                    result.gartars = this.kpGarTarRepository.GetByFormule(formuleId).ToDictionary(x => x.Kdgid);
                    result.portees = this.kpGarApRepository.GetByFormule(formuleId).ToDictionary(x => x.Kdfid);
                    result.inventaires = result.gars.Select(x => x.Value).Where(x => x.Kdeinven != 0).Select(x => inventaireRepository.GetInventaireById(x.Kdeinven, null)).Where(x => x != null).ToList();
                }
                result.desis = this.desiRepository.GetDesignationsByAffaire(aff);
                result.exprDbFrhD = this.GetKpExprFrhD(aff);
                result.exprDbLciD = this.GetKpExprLciD(aff);
                result.exprDbFrh = this.GetKpExprFrhForTarifs(aff);
                result.exprDbLci = this.GetKpExprLciForTarifs(aff);
                result.exprFrh = this.GetExprFrhDict(result.exprDbFrh, result.exprDbFrhD, result.desis);
                result.exprLci = this.GetExprLciDict(result.exprDbLci, result.exprDbLciD, result.desis);
            }
            else {
                var formula = this.hpforRepository.Get(formuleId, avenant.Value);
                if (formula != null) {
                    result.formula = formula;
                    result.applications = hpOptApRepository.GetByAffaire(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment, aff.NumeroAvenant.Value).ToLookup(x => x.Kddkdbid);
                    result.options = this.hpOptRepository.GetByFormule(formuleId, avenant.Value).ToDictionary(x => x.Kdbid);
                    result.optiondetails = this.hpOptDRepository.GetByFormule(formuleId, avenant.Value).ToDictionary(x => x.Kdcid);
                    result.gars = this.hpGaranRepository.GetByFormule(formuleId, avenant.Value).ToDictionary(x => x.Kdeid);
                    result.gartars = this.hpGarTarRepository.GetByFormule(formuleId, avenant.Value).ToDictionary(x => x.Kdgid);
                    result.portees = this.hpGarApRepository.GetByFormule(formuleId, avenant.Value).ToDictionary(x => x.Kdfid);
                    result.inventaires = result.gars.Select(x => x.Value).Where(x => x.Kdeinven != 0).Select(x => inventaireRepository.GetInventaireById(x.Kdeinven, x.Kdeavn)).Where(x => x != null).ToList();
                }
                result.desis = this.desiRepository.GetDesignationsByAffaire(aff);
                result.exprDbFrhD = this.GetKpExprFrhD(aff);
                result.exprDbLciD = this.GetKpExprLciD(aff);
                result.exprDbFrh = this.GetKpExprFrhForTarifs(aff);
                result.exprDbLci = this.GetKpExprLciForTarifs(aff);
                result.exprFrh = this.GetExprFrhDict(result.exprDbFrh, result.exprDbFrhD, result.desis);
                result.exprLci = this.GetExprLciDict(result.exprDbLci, result.exprDbLciD, result.desis);
            }
            return result;
        }

        private RawDataMany GetRawData(AffaireId id, RawDataFilter filter = 0)
        {
            string codeAffaire = id.CodeAffaire.PadLeft(9, ' ');
            string typeAffaire = id.TypeAffaire.AsCode();
            var result = new RawDataMany(refRepo, this.paramRepository);
            bool isNeeded(RawDataFilter f) => filter == 0 || (filter & f) == f;
            if (!id.IsHisto) {
                if (isNeeded(RawDataFilter.Formula)) {
                    result.formulas = this.kpforRepository.Liste(typeAffaire, codeAffaire, id.NumeroAliment);
                }
                if (isNeeded(RawDataFilter.Designations)) {
                    result.desis = this.desiRepository.GetDesignationsByAffaire(id);
                }
                if (isNeeded(RawDataFilter.Options)) {
                    result.options = this.kpOptRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdbkdaid);
                }
                if (isNeeded(RawDataFilter.Applications)) {
                    result.applications = kpOptApRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kddkdbid);
                }
                if (isNeeded(RawDataFilter.DetailsOption)) {
                    result.optiondetails = this.kpOptDRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdckdbid);
                }
                if (isNeeded(RawDataFilter.Garanties)) {
                    result.gars = this.kpGaranRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdekdcid);
                }
                if (isNeeded(RawDataFilter.TarifsGaranties)) {
                    result.gartars = this.kpGarTarRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdgkdeid);
                }
                if (isNeeded(RawDataFilter.Portees)) {
                    result.portees = this.kpGarApRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdfkdeid);
                }
                if (isNeeded(RawDataFilter.Inventaires)) {
                    result.inventaires = result.gars.SelectMany(x => x).Where(x => x.Kdeinven != 0).Select(
                        x => this.inventaireRepository.GetInventaireById(x.Kdeinven, x.Kdeavn)
                    ).Where(x => x != null).ToList();
                }
                if (isNeeded(RawDataFilter.ExpressionComplexes)) {
                    result.exprFrh = this.GetExprFrhDict(GetKpExprFrhForTarifs(id), GetKpExprFrhD(id), result.desis);
                    result.exprLci = this.GetExprLciDict(GetKpExprLciForTarifs(id), GetKpExprLciD(id), result.desis);
                }
            }
            else {
                if (isNeeded(RawDataFilter.Formula)) {
                    result.formulas = this.hpforRepository.Liste(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.GetValueOrDefault());
                }
                if (isNeeded(RawDataFilter.Designations)) {
                    result.desis = this.desiRepository.GetDesignationsByAffaire(id);
                }
                if (isNeeded(RawDataFilter.Options)) {
                    result.options = this.hpOptRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.GetValueOrDefault()).ToLookup(x => x.Kdbkdaid);
                }
                if (isNeeded(RawDataFilter.Applications)) {
                    result.applications = hpOptApRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.GetValueOrDefault()).ToLookup(x => x.Kddkdbid);
                }
                if (isNeeded(RawDataFilter.DetailsOption)) {
                    result.optiondetails = this.hpOptDRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.GetValueOrDefault()).ToLookup(x => x.Kdckdbid);
                }
                if (isNeeded(RawDataFilter.Garanties)) {
                    result.gars = this.hpGaranRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.GetValueOrDefault()).ToLookup(x => x.Kdekdcid);
                }
                else if (isNeeded(RawDataFilter.GarantiesNatureModifiable)) {
                    result.garsNatModifiable = this.hpGaranRepository.GetByAffaireWithNatureModifiable(typeAffaire, codeAffaire, id.NumeroAliment).ToLookup(x => x.Kdeid);
                }
                if (isNeeded(RawDataFilter.TarifsGaranties)) {
                    result.gartars = this.hpGarTarRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.GetValueOrDefault()).ToLookup(x => x.Kdgkdeid);
                }
                if (isNeeded(RawDataFilter.Portees)) {
                    result.portees = this.hpGarApRepository.GetByAffaire(typeAffaire, codeAffaire, id.NumeroAliment, id.NumeroAvenant.GetValueOrDefault()).ToLookup(x => x.Kdfkdeid);
                }
                if (isNeeded(RawDataFilter.Inventaires)) {
                    result.inventaires = result.gars.SelectMany(x => x)
                        .Where(x => x.Kdeinven != 0)
                        .Select(x => this.inventaireRepository.GetInventaireById(x.Kdeinven, x.Kdeavn))
                        .Where(x => x != null).ToList();
                }
                if (isNeeded(RawDataFilter.ExpressionComplexes)) {
                    result.exprFrh = this.GetExprFrhDict(GetKpExprFrhForTarifs(id), GetKpExprFrhD(id), result.desis);
                    result.exprLci = this.GetExprLciDict(GetKpExprLciForTarifs(id), GetKpExprLciD(id), result.desis);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the history data for each previous Avenant
        /// </summary>
        /// <param name="id"></param>
        /// <param name="histoFilter"></param>
        /// <returns></returns>
        private IDictionary<int, RawDataMany> GetHistoRawData(AffaireId id, RawDataFilter histoFilter) {
            if (id.NumeroAvenant.GetValueOrDefault() < 1) {
                return null;
            }

            var histo = new Dictionary<int, RawDataMany>();
            if ((histoFilter & RawDataFilter.GarantiesNatureModifiable) == RawDataFilter.GarantiesNatureModifiable) {
                int filter = (int)histoFilter - (int)RawDataFilter.GarantiesNatureModifiable;
                histo.Add(id.NumeroAvenant.Value, GetRawData(id, RawDataFilter.GarantiesNatureModifiable));
                histoFilter = (RawDataFilter)filter;
                if (filter == 0) {
                    return histo;
                }
            }

            int avenant = id.NumeroAvenant.Value;
            while (--avenant >= 0) {
                AffaireId tempId = id.MakeCopy(true);
                tempId.NumeroAvenant = avenant;
                histo.Add(avenant, GetRawData(tempId, histoFilter));
            }

            return histo;
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
            if (ovb.Type != TypeOption.Volet)
            {
                OptionBloc b = (OptionBloc)ovb;
                det.Kdckaeid = b.ParamBloc.BlocId;
                det.Kdckaqid = b.ParamBloc.CatBlocId;
                det.Kdcmodele = ovb.ParamModele.Code;
                det.Kdckarid = ovb.ParamModele.CatId;
            }
            else
            {
                det.Kdcmodele = string.Empty;
            }
            det.Kdcfor = form.FormuleNumber;
            det.Kdcopt = opt.OptionNumber;
            det.Kdckakid = ovb.ParamVolet.VoletId;
            det.Kdckdbid = opt.Id;
            det.Kdcflag = Convert.ToInt32(ovb.IsChecked);

        }

        private void UpdateOptDetMetadata(OptionDetail ovb, Formule form, Option opt, KpOptD det, string currentUser)
        {
            det.Kdcmajd = DateTime.Now.AsDateNumber();
            det.Kdcmaju = currentUser;

            if (det.Kdccrd == 0) {
                InitOptionVoletBloc(ovb, form, opt, det, currentUser);
            }
        }

        private void InitOptionVoletBloc(OptionDetail ovb, Formule form, Option opt, KpOptD det, string currentUser)
        {
            var aff = form.AffaireId;
            det.Kdccrd = DateTime.Now.AsDateNumber();
            det.Kdccru = currentUser;

            det.Kdcalx = aff.NumeroAliment;
            det.Kdcipb = aff.CodeAffaire;
            det.Kdctyp = aff.TypeAffaire.AsCode();
            det.Kdcteng = ovb.Type.AsString();
            det.Kdcfor = form.FormuleNumber;
            det.Kdcopt = opt.OptionNumber;
            det.Kdcid = kpOptDRepository.NewId().AsLong();
        }

        private void UpdateGarantie(Garantie garantie, DataModel.DTO.KpGaran gar, Formule form, Option opt, string currentUser, long rootId, long parentId, long blocId)
        {
            var aff = form.AffaireId;

            gar.Kdekdhid = garantie.LienKPSPEC;
            gar.Kdekdfid = garantie.LienKPGARAP;
            gar.Kdeinven = garantie.Inventaire?.Id ?? default(int);
            gar.Kdekdcid = blocId;


            gar.Kdeipb = aff.CodeAffaire;
            gar.Kdealx = aff.NumeroAliment;
            gar.Kdetyp = aff.TypeAffaire.AsCode();
            gar.Kdeavn = aff.NumeroAvenant;
            gar.Kdecravn = garantie.NumeroAvenantCreation;
            gar.Kdemajavn = garantie.NumeroAvenantModif;

            gar.Kdefor = form.FormuleNumber;
            gar.Kdeopt = opt.OptionNumber;

            gar.Kdese1 = parentId == 0 ? 0 : rootId;
            gar.Kdesem = parentId;

            gar.Kdeid = garantie.Id;
            gar.Kdecar = garantie.Caractere.AsString();
            gar.Kdeasbase = garantie.Assiette.Base?.Code ?? string.Empty;
            gar.Kdeasmod = garantie.Assiette.IsModifiable.ToYesNo();
            gar.Kdeasobli = garantie.Assiette.IsObligatoire.ToYesNo();
            gar.Kdeasunit = garantie.Assiette.Unite;
            gar.Kdeasvala = garantie.Assiette.ValeurActualise;
            gar.Kdeasvalo = garantie.Assiette.ValeurOrigine;
            gar.Kdeasvalw = garantie.Assiette.ValeurTravail;
            gar.Kdecravn = garantie.NumeroAvenantCreation;
            gar.Kdemajavn = garantie.NumeroAvenantModif;
            gar.Kdetaxcod = garantie.Taxe?.Code ?? string.Empty;
            gar.Kdeheudeb = garantie.DateDebut.AsTime6();
            gar.Kdedatdeb = garantie.DateDebut.AsDate();
            gar.Kdeheufin = garantie.DateFinDeGarantie.AsTime6();
            gar.Kdedatfin = garantie.DateFinDeGarantie.AsDate();
            gar.Kdedefg = garantie.DefinitionGarantie?.Code ?? string.Empty;
            gar.Kdeduree = garantie.Duree ?? 0;
            gar.Kdeduruni = garantie.DureeUnite?.Code ?? string.Empty;
            gar.Kdeinvsp = garantie.InventaireSpecifique.ToYesNo();
            gar.Kdealiref = garantie.IsAlimMontantReference.ToYesNo();
            gar.Kdecatnat = garantie.IsApplicationCATNAT.ToYesNo();
            gar.Kdemodi = garantie.IsFlagModifie.ToYesNo();
            gar.Kdeajout = garantie.IsGarantieAjoutee.ToYesNo();
            gar.Kdeina = garantie.IsIndexe.ToYesNo();
            gar.Kdepind = garantie.IsParameIndex.ToYesNo();
            gar.Kdenat = garantie.Nature.AsString();
            gar.Kdegan = garantie.NatureRetenue.AsString();
            gar.Kdenumpres = garantie.NumDePresentation;
            gar.Kdeptaxc = garantie.ParamCodetaxe?.Code ?? string.Empty;
            gar.Kdepcatn = garantie.ParametrageCATNAT.ToYesNo();
            gar.Kdepref = garantie.ParametrageIsAlimMontantRef.ToYesNo();
            gar.Kdeseq = garantie.ParamGarantie.Sequence;
            gar.Kdepntm = garantie.ParamIsNatModifiable.ToYesNo();
            gar.Kdepala = garantie.ParamTypeAlimentation.AsString();
            gar.Kdetaxrep = garantie.RepartitionTaxe;
            gar.Kdetri = garantie.Tri;
            gar.Kdeala = garantie.TypeAlimentation.AsString();
            gar.Kdeprp = garantie.PeriodeApplication?.Code ?? string.Empty;
            gar.Kdetcd = garantie.TypeControleDate.AsString();
            gar.Kdetypemi = garantie.TypeEmission?.Code ?? string.Empty;

            gar.Kdegaran = garantie.ParamGarantie.ParamGarantie.CodeGarantie;
            gar.Kdeniveau = garantie.ParamGarantie.Niveau;
            // by reverse-egineering :
            gar.Kdepprp = "A";
            gar.Kdepemi = "P";
            gar.Kdealo = "";



        }

        private static void UpdateMetaDataGarantie(KpGaran gar, string currentUser)
        {
            if (gar.Kdecrd != 0) {
                gar.Kdecru = !string.IsNullOrWhiteSpace(gar.Kdecru) ? gar.Kdecru : currentUser;
                gar.Kdecrd = gar.Kdecrd != 0 ? gar.Kdecrd : DateTime.Now.AsDateNumber();
            }
        }

        private void UpdateTarif(TarifGarantie gar, KpGarTar tar, KpGaran kpgaran)
        {
            tar.Kdgpribase = gar.PrimeValeur.Base?.Code ?? string.Empty;
            tar.Kdgpriunit = gar.PrimeValeur.Unite;
            tar.Kdgprivala = gar.PrimeValeur.ValeurActualise;
            tar.Kdgprivalo = gar.PrimeValeur.ValeurOrigine;
            tar.Kdgprivalw = gar.PrimeValeur.ValeurTravail;
            tar.Kdgprimod = gar.PrimeValeur.IsModifiable.ToYesNo();
            tar.Kdgpriobl = gar.PrimeValeur.IsObligatoire.ToYesNo();
            tar.Kdgfrhbase = gar.Franchise.Base?.Code ?? string.Empty;
            tar.Kdgfrhunit = gar.Franchise.Unite;
            tar.Kdgfrhvala = gar.Franchise.ValeurActualise;
            tar.Kdgfrhvalo = gar.Franchise.ValeurOrigine;
            tar.Kdgfrhvalw = gar.Franchise.ValeurTravail;
            tar.Kdgfrhmod = gar.Franchise.IsModifiable.ToYesNo();
            tar.Kdgfrhobl = gar.Franchise.IsObligatoire.ToYesNo();
            tar.Kdgkdkid = gar.Franchise.ExpressionComplexe?.Id ?? 0L;
            tar.Kdgfmibase = gar.FranchiseMini.Base?.Code ?? string.Empty;
            tar.Kdgfmiunit = gar.FranchiseMini.Unite;
            tar.Kdgfmivala = gar.FranchiseMini.ValeurActualise;
            tar.Kdgfmivalo = gar.FranchiseMini.ValeurOrigine;
            tar.Kdgfmivalw = gar.FranchiseMini.ValeurTravail;
            tar.Kdgfmabase = gar.FranchiseMax.Base?.Code ?? string.Empty;
            tar.Kdgfmaunit = gar.FranchiseMax.Unite;
            tar.Kdgfmavala = gar.FranchiseMax.ValeurActualise;
            tar.Kdgfmavalo = gar.FranchiseMax.ValeurOrigine;
            tar.Kdgfmavalw = gar.FranchiseMax.ValeurTravail;
            tar.Kdglcibase = gar.LCI.Base?.Code ?? string.Empty;
            tar.Kdglciunit = gar.LCI.Unite;
            tar.Kdglcivala = gar.LCI.ValeurActualise;
            tar.Kdglcivalo = gar.LCI.ValeurOrigine;
            tar.Kdglcivalw = gar.LCI.ValeurTravail;
            tar.Kdglcimod = gar.LCI.IsModifiable.ToYesNo();
            tar.Kdglciobl = gar.LCI.IsObligatoire.ToYesNo();
            tar.Kdgkdiid = gar.LCI.ExpressionComplexe?.Id ?? 0L;
            tar.Kdgnumtar = gar.NumeroTarif;
            tar.Kdgcmc = gar.ComptantMontantcalcule;
            tar.Kdgcht = gar.ComptantMontantForceHT;
            tar.Kdgctt = gar.ComptantMontantForceTTC;
            tar.Kdgmntbase = gar.PrimeMontantBase;
            tar.Kdgprimpro = gar.PrimeProvisionnelle;
            tar.Kdgtmc = gar.TotalMontantCalcule;
            tar.Kdgtff = gar.TotalMontantForce;


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
        private IDictionary<long, ExpressionComplexeLCI> GetExprLciDict(IDictionary<long, KpExpLCI> exprDbLci, IDictionary<long, KpExpLCID> exprDbLciD, IDictionary<int, string> desis)
        {
            var detLci = exprDbLciD.Values.Select(x => {
                var ret = new ExpressionComplexeDetailLCI {
                    Id = x.Kdjid,
                    ExprId = x.Kdjkdiid,
                    ConcurrentCodeBase = refRepo.GetValue<BaseLCI>(x.Kdjlobase),
                    ConcurrentUnite = refRepo.GetValue<UniteLCI>(x.Kdjlovau),
                    ConcurrentValeur = x.Kdjloval.AsDecimal(),
                    CodeBase = refRepo.GetValue<BaseLCI>(x.Kdjlcbase),
                    Unite = refRepo.GetValue<UniteLCI>(x.Kdjlcvau),
                    Valeur = x.Kdjlcval.AsDecimal(),
                    Ordre = x.Kdjordre
                };
                return ret;
            }).ToLookup(x => x.ExprId);


            var expr = exprDbLci.Values.Select(x => {
                var ret = new ExpressionComplexeLCI {
                    Origine = x.Kdiori.AsEnum<OrigineExpression>(),
                    Code = x.Kdilce,
                    Id = x.Kdiid,
                    Description = x.Kdidesc,
                    Designation = desis.GetValueOrDefault((int)x.Kdidesi),
                    DesiId = x.Kdidesi,
                    IsModifiable = x.Kdimodi.AsBool()
                };
                ret.Details = detLci[x.Kdiid].Cast<ExpressionComplexeDetailBase>().ToList();
                return ret;
            }).GroupBy(x=>x.Code).Select(x=>x.OrderByDescending(y=>y.Id).First()).ToDictionary(x => x.Id);
            return expr;
        }


        private IDictionary<long, ExpressionComplexeFranchise> GetExprFrhDict(IDictionary<long, KpExpFrh> exprDbFrh, IDictionary<long, KpExpFrhD> exprDbFrhD, IDictionary<int, string> desis)
        {
            var detFrh = exprDbFrhD.Values.Select(x => {
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


            var expr = exprDbFrh.Values.Select(x => {
                var ret = new ExpressionComplexeFranchise {
                    Origine = x.Kdkori.AsEnum<OrigineExpression>(),
                    Code = x.Kdkfhe,
                    Id = x.Kdkid,
                    Description = x.Kdkdesc,
                    Designation = desis.GetValueOrDefault((int)x.Kdkdesi),
                    DesiId = x.Kdkdesi,
                    IsModifiable = x.Kdkmodi.AsBool()
                };
                ret.Details = detFrh[x.Kdkid].Cast<ExpressionComplexeDetailBase>().ToList();
                return ret;
            }).GroupBy(x => x.Code).Select(x => x.OrderByDescending(y => y.Id).First()).ToDictionary(x => x.Id);
            return expr;
        }



        private IDictionary<long, KpExpLCI> GetKpExprLciForTarifs(AffaireId aff)
        {
            return this.kpExpLciRepo.GetByAffaire(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment).ToDictionary(x => x.Kdiid);
        }

        private IDictionary<long, KpExpLCID> GetKpExprLciD(AffaireId aff)
        {
            return this.kpExpLciDRepo.GetByAffaire(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment).ToDictionary(x => x.Kdjid);
        }
        private IDictionary<long, KpExpFrh> GetKpExprFrhForTarifs(AffaireId aff)
        {
            return this.kpExpFrhRepo.GetByAffaire(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment).ToDictionary(x => x.Kdkid);
        }

        private IDictionary<long, KpExpFrhD> GetKpExprFrhD(AffaireId aff)
        {
            return this.kpExpFrhDRepo.GetByAffaire(aff.TypeAffaire.AsCode(), aff.CodeAffaire, aff.NumeroAliment).ToDictionary(x => x.Kdlid);
        }


    }

}
