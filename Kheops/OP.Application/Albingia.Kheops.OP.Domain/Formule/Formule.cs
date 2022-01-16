using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Parametrage.Formule;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Risque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruthTable = System.Collections.Generic.IDictionary<(Albingia.Kheops.OP.Domain.Model.CaractereSelection caractere, Albingia.Kheops.OP.Domain.Referentiel.NatureValue nature), Albingia.Kheops.OP.Domain.Parametrage.Formule.NatureSelection>;


namespace Albingia.Kheops.OP.Domain.Formule
{
    public class Formule : IValidatable
    {
        private bool? isSelected;
        private int? selectedOptionNumber;

        public AffaireId AffaireId { get; set; }
        public int Chrono { get; set; }
        public int FormuleNumber { get; set; }
        public string Alpha { get; set; }
        public string Description { get; set; }
        public CibleCatego Cible { get; set; }
        public List<Option> Options { get; set; } = new List<Option>();
        public long Id { get; set; }

        public int? SelectedOptionNumber {
            get => AffaireId?.TypeAffaire == AffaireType.Contrat ? null : this.selectedOptionNumber;
            set => this.selectedOptionNumber = AffaireId?.TypeAffaire == AffaireType.Contrat ? null : value;
        }

        public bool? IsSelected {
            get => AffaireId?.TypeAffaire == AffaireType.Contrat ? null : this.isSelected;
            set => this.isSelected = AffaireId?.TypeAffaire == AffaireType.Contrat ? null : value;
        }


        public static IEnumerable<Garantie> GetAllGaranties(IEnumerable<Formule> formules) {
            return formules?.SelectMany(x => x.GetAllGaranties()) ?? Enumerable.Empty<Garantie>();
        }

        public IEnumerable<Garantie> GetAllGaranties() {
            return Options.SelectMany(o => o.AllGaranties);
        }

        public void SetGarantieStatus(long idOption, long idGaran, bool selected)
        {
            var garan = this.Options.Find(x => x.Id == idOption)?.FindGarantieById(idGaran);
            if (garan.IsChecked != selected) {
                garan.IsFlagModifie = true;
            }
        }

        public void SetDates(Affaire.Affaire affaire, IEnumerable<Risque.Risque> risques)
        {
            foreach (var opt in Options) {
                foreach (var gar in opt.AllGaranties) {
                    gar.ApplyDatesOnGuarantie(opt, opt.FilterRisques(risques), affaire);
                }
            }
        }

        public void SetDates(Affaire.Affaire affaire, Risque.Risque risque)
        {
            foreach (var opt in Options) {
                foreach (var gar in opt.AllGaranties) {
                    gar.ApplyDatesOnGuarantie(opt, risque, affaire);
                }
            }

        }

        public void ApplyParameters(Affaire.Affaire affaire, IEnumerable<ParamVolet> volets, TruthTable truthTable, bool readOnly, TypologieModele mod, DateTime dateApplication)
        {
            if (!readOnly) {
                Dictionary<long, ParamVolet> paramVolets;
                paramVolets = volets.Where(x => x.Cible != null && x.Branche == this.Cible.Branche && x.Cible.Cible.Code == this.Cible.Cible.Code).ToDictionary(x => x.CatVoletId);
                foreach (Option opt in Options)
                {
                    opt.UpdateWithParametrage(affaire, paramVolets, truthTable, mod, dateApplication);
                    opt.UpdateNumerosAvenant(new AffaireId() {
                        CodeAffaire = affaire.CodeAffaire,
                        IsHisto = affaire.IsHisto,
                        NumeroAliment= affaire.NumeroAliment,
                        NumeroAvenant = affaire.NumeroAvenant,
                        TypeAffaire = affaire.TypeAffaire
                    });
                }
            }
        }
        public IEnumerable<ValidationError> ValidateForNewAffair() {
            var errors = new List<ValidationError>(/*Validate()*/);
            int numopt = SelectedOptionNumber.GetValueOrDefault();
            if (numopt < 1) {
                errors.Add(new ValidationError("Formule", FormuleNumber.ToString(), Id.ToString(), "Au moins une option doit être sélectionnée"));
            }
            if (numopt > Option.MaxNbByFormula) {
                errors.Add(new ValidationError("Formule", FormuleNumber.ToString(), Id.ToString(), "Numéro d'option sélectionné invalide"));
            }
            return errors;
        }
        public IEnumerable<ValidationError> Validate()
        {
            this.EnsureHierarchy();
            return this.Options.SelectMany(x => x.Validate()).ToList();
        }
        public void EnsureHierarchy()
        {
            this.Options.ForEach(o => o.SetParent(this));
        }

        public void EnsureInventaires()
        {
            List<Garantie> garanties = Options.First().OptionVolets.SelectMany(v => v.Blocs).SelectMany(b => b.Garanties.Where(g1 => g1.InventairePossible && g1.Inventaire == null)).ToList();
            garanties.AddRange(Options.First().OptionVolets.SelectMany(v => v.Blocs).SelectMany(b => b.Garanties.SelectMany(g1 => g1.SousGaranties.Where(g2 => g2.InventairePossible && g2.Inventaire == null))));
            garanties.AddRange(Options.First().OptionVolets.SelectMany(v => v.Blocs).SelectMany(b => b.Garanties.SelectMany(g1 => g1.SousGaranties.SelectMany(g2 => g2.SousGaranties.Where(g3 => g3.InventairePossible && g3.Inventaire == null)))));
            garanties.AddRange(Options.First().OptionVolets.SelectMany(v => v.Blocs).SelectMany(b => b.Garanties.SelectMany(g1 => g1.SousGaranties.SelectMany(g2 => g2.SousGaranties.SelectMany(g3 => g3.SousGaranties.Where(g4 => g4.InventairePossible && g4.Inventaire == null))))));
            garanties.ForEach(g => {
                g.Inventaire = new Inventaire.Inventaire() {
                    TypeInventaire = g.ParamGarantie.ParamGarantie.TypeInventaire
                };
            });
        }

        public void InitParamRelations(ILookup<long, BlocRelation> blocRelations, ILookup<long, GarantieRelation> grRelations) {
            foreach (var option in Options) {
                foreach(var bloc in option.OptionVolets.SelectMany(v => v.Blocs)) {
                    bloc.ParamRelations = blocRelations[bloc.ParamBloc.BlocId].ToList();
                    foreach (var garantie in bloc.AllGaranties) {
                        garantie.ParamRelations = grRelations[garantie.ParamGarantie.Sequence].ToList();
                    }
                }
            }
        }

        public IEnumerable<ValidationError> CheckGarantiesHiddenInBlocs(int numOption = 1) {
            var blocs = Options.First(o => o.OptionNumber == numOption).OptionVolets.SelectMany(v => v.Blocs);
            return blocs.Where(b => b.Garanties.Count == 1 && b.Garanties.Any(g => g.IsHidden))
                .Select(b => new ValidationError(nameof(Formule), FormuleNumber.ToString(), string.Empty, $"Formule {FormuleNumber} : Le bloc {b.ParamBloc.Code} contient une seule garantie cachée ({b.Garanties.First().CodeGarantie})"))
                .ToList();
        }
    }
}
