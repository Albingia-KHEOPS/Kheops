using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Transverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruthTable = System.Collections.Generic.IDictionary<(Albingia.Kheops.OP.Domain.Model.CaractereSelection caractere, Albingia.Kheops.OP.Domain.Referentiel.NatureValue nature), Albingia.Kheops.OP.Domain.Parametrage.Formule.NatureSelection>;


namespace Albingia.Kheops.OP.Domain.Formule
{
    public class Option : IValidatable
    {
        public const int MaxNbByFormula = 3;
        public List<OptionVolet> OptionVolets { get; set; } = new List<OptionVolet>();
        public MontantsOption MontantsOption { get; set; }
        public long Id { get; set; }
        public int OptionNumber { get; set; }
        public int? NumeroAvenant { get; set; }
        public List<Application> Applications { get; set; }
        public virtual IEnumerable<Garantie> AllGaranties => OptionVolets.SelectMany(x => x.AllGaranties);

        public virtual IEnumerable<Garantie> AllUnselectedGaranties => OptionVolets.SelectMany(x => x.IsChecked ? x.AllUncheckedGaranties : x.AllGaranties);
        public virtual IEnumerable<Garantie> AllSelectedGaranties => OptionVolets.SelectMany(x => x.IsChecked ? x.AllCheckedGaranties : Enumerable.Empty<Garantie>());

        /* KDBAVA, KDBAVM, KDBAVJ */
        public DateTime? DateAvenant { get; set; }

        /* KDBAVG */
        public int NumAvenantModif { get; set; }

        /* KDBAVE */
        public int NumAvenantCreation { get; set; }

        public IDictionary<int, DateTime?> AvnDatesHistory { get; set; }

        public Formule Formule { get; set; }

        internal Formule parent { get; set; }

        internal void SetParent(Formule parent)
        {
            this.parent = parent;
            OptionVolets.ForEach(v => v.SetParent(this));
        }

        public bool IsModifiedForAvenant(int currentNumeroAvenant) => NumAvenantModif == currentNumeroAvenant;

        public virtual Garantie FindGarantieById(long id)
        {
            return OptionVolets.Select(x => x.FindGarantieById(id)).FirstOrDefault();
        }

        public void AddGareat() {
            var (voletGareat, blocGareat, garantieGareat) = GetGareat();
            if (garantieGareat is null) {
                return;
            }
            blocGareat.IsChecked = true;
            if (voletGareat.Blocs.Count == 1) {
                voletGareat.IsChecked = true;
            }
        }

        public void RemoveGareat() {
            var (voletGareat, blocGareat, garantieGareat) = GetGareat();
            if (garantieGareat is null) {
                return;
            }
            blocGareat.IsChecked = false;
            if (voletGareat.Blocs.Count == 1) {
                voletGareat.IsChecked = false;
            }
        }

        public virtual Garantie FindGarantieBySeq(string codeBloc, Int64 id)
        {
            return OptionVolets.Select(x => x.FindGarantieBySeq(codeBloc, id)).FirstOrDefault(x => x != null);
        }

        public IEnumerable<ValidationError> Validate()
        {
            var allErrors = OptionVolets.Where(v => v.IsChecked).SelectMany(x => x.Validate()).ToList();
            var blocs = OptionVolets.SelectMany(x => x.Blocs).ToDictionary(x => x.ParamBloc.BlocId);
            foreach (var value in blocs.Where(x => x.Value.IsSelected)) {
                var bloc = value.Value;
                allErrors.AddRange(SelectIncompatibilityErrors(blocs, value, bloc));
                allErrors.AddRange(SelectAssociationErrors(blocs, value, bloc));
            }
            if (allErrors.Any())
            {
                // if any bloc incompatibility : 
                //     return now to avoid having the same garantie sequence selected several times
                return FormatValidationErrors(allErrors);
            }
            // in a valid bloc selection there cannot be twice the same sequence number for the Garantie
            var garanties = AllSelectedGaranties.ToDictionary(x => x.ParamGarantie.Sequence);
            var garantiesAll = AllGaranties.ToDictionary(x => x.ParamGarantie.Sequence);
            foreach (var value in garanties) {
                var garantie = value.Value;
                if (garantie.IsSelected) {
                    allErrors.AddRange(SelectIncompatibilityErrors(garantiesAll, value, garantie));
                    allErrors.AddRange(SelectAssociationErrors(garantiesAll, value, garantie));
                    allErrors.AddRange(SelectDependencyErrors(garantiesAll, garantie));
                }
            }
            return FormatValidationErrors(allErrors);
        }

        public void ResetIds(int newNumber) {
            OptionNumber = newNumber;
            Id = default;
            foreach (var volet in OptionVolets) {
                volet.ResetIds();
            }
            foreach (var appl in Applications) {
                appl.ResetIds();
            }
        }

        private static IEnumerable<ValidationError> SelectAssociationErrors(Dictionary<long, OptionBloc> blocs, KeyValuePair<long, OptionBloc> value, OptionBloc bloc)
        {
            var assoc = bloc.ParamRelations.Where(x => x.Relation == TypeRelation.Associe).Select(x => x.EsclaveId == value.Key ? x.MaitreId : x.EsclaveId);
            var errorBlocs = assoc.Where(x => blocs.ContainsKey(x) && !blocs[x].IsSelected).Select(x => blocs[x]).ToList();
            foreach (var b in errorBlocs) {
                yield return new RelationError(
                    TypeRelation.Associe,
                    "Bloc", bloc.ParamBloc.Code, bloc.ParamBloc.CatBlocId.ToString(),
                    "Bloc", b.ParamBloc.Code, b.ParamBloc.CatBlocId.ToString(),
                    $"Le bloc {bloc.ParamBloc.Code} est associé avec le bloc {b.ParamBloc.Code} qui n'est pas sélectionné");
            }
        }

        private static IEnumerable<ValidationError> SelectIncompatibilityErrors(Dictionary<long, OptionBloc> blocs, KeyValuePair<long, OptionBloc> value, OptionBloc bloc)
        {
            var incompat = bloc.ParamRelations.Where(x => x.MaitreId == value.Key && x.Relation == TypeRelation.Incompatible).Select(x => x.EsclaveId).Distinct();
            var errorBlocs = incompat.Where(x => blocs.ContainsKey(x) && blocs[x].IsSelected).Select(x => blocs[x]).ToList();
            foreach (var b in errorBlocs) {
                yield return new ValidationError(
                    "Bloc", bloc.ParamBloc.Code, bloc.ParamBloc.CatBlocId.ToString(),
                    $"Le bloc {bloc.ParamBloc.Code} est incompatible avec le bloc {b.ParamBloc.Code} qui est sélectionné ");
            }
        }

        private static IEnumerable<ValidationError> SelectDependencyErrors(Dictionary<long, Garantie> garanties, Garantie garantie)
        {
            var dependences = garantie.ParamRelations.Where(x => garantie.IsSelected && x.Relation == TypeRelation.Dependant);
            var errorGarantiesNotSelected = dependences
                .Where(x => x.EsclaveId == garantie.ParamGarantie.Sequence)
                .Where(x => garanties.ContainsKey(x.MaitreId) && !(garanties[x.MaitreId].IsSelected))
                .Select(x => garanties[x.MaitreId]).ToList();

            var errorsDatesDiffer = dependences
                .Where(x => x.EsclaveId == garantie.ParamGarantie.Sequence)
                .Where(x => garanties.ContainsKey(x.MaitreId) && garanties[x.MaitreId].IsSelected && !garanties[x.MaitreId].IsSubPeriodOf(garantie))
                .Select(x => garanties[x.MaitreId]).ToList();

            foreach (var g in errorGarantiesNotSelected) {
                yield return new RelationError(
                    TypeRelation.Dependant,
                    nameof(Garantie), garantie.CodeGarantie, garantie.ParamGarantie.Sequence.ToString(),
                    nameof(Garantie), g.CodeGarantie, g.ParamGarantie.Sequence.ToString(),
                    $"La garantie {garantie.CodeGarantie} a pour dépendance {g.CodeGarantie} qui n'est pas sélectionnée");
            }

            foreach (var g in errorsDatesDiffer) {
                yield return new RelationError(
                    TypeRelation.Dependant,
                    nameof(Garantie), garantie.CodeGarantie, garantie.ParamGarantie.Sequence.ToString(),
                    nameof(Garantie), g.CodeGarantie, g.ParamGarantie.Sequence.ToString(),
                    $"La garantie {garantie.CodeGarantie} a pour dépendance {g.CodeGarantie} qui n'a pas les mêmes dates de validités",
                    "dates");
            }
        }

        private static IEnumerable<ValidationError> SelectAssociationErrors(Dictionary<long, Garantie> garanties, KeyValuePair<long, Garantie> value, Garantie garantie)
        {
            var assoc = garantie.ParamRelations.Where(x => x.Relation == TypeRelation.Associe).Select(x => x.EsclaveId == value.Key ? x.MaitreId : x.EsclaveId).Distinct();
            var errorGarantiesNotSelected = assoc.Where(x => garanties.ContainsKey(x) && !garanties[x].IsSelected).Select(x => garanties[x]).ToList();
            var errorsDatesDiffer = assoc.Where(x => garanties.ContainsKey(x) && (garanties[x].IsSelected && !garanties[x].IsSamePeriodAs(garantie))).Select(x => garanties[x]).ToList();
            foreach (var g in errorGarantiesNotSelected) {
                yield return new RelationError(
                    TypeRelation.Associe,
                    nameof(Garantie), garantie.CodeGarantie, garantie.ParamGarantie.Sequence.ToString(),
                    nameof(Garantie), g.CodeGarantie, g.ParamGarantie.Sequence.ToString(),
                    $"La garantie {garantie.CodeGarantie} est associée avec {g.CodeGarantie} qui n'est pas sélectionnée");
            }

            foreach (var g in errorsDatesDiffer) {
                yield return new RelationError(
                    TypeRelation.Associe,
                    nameof(Garantie), garantie.CodeGarantie, garantie.ParamGarantie.Sequence.ToString(),
                    nameof(Garantie), g.CodeGarantie, g.ParamGarantie.Sequence.ToString(),
                    $"La garantie {garantie.CodeGarantie} est associée avec {g.CodeGarantie} qui n'a pas les mêmes dates de validités",
                    "dates");
            }
        }

        private static IEnumerable<ValidationError> SelectIncompatibilityErrors(Dictionary<long, Garantie> garanties, KeyValuePair<long, Garantie> value, Garantie garantie)
        {
            var incompat = garantie.ParamRelations.Where(x => x.MaitreId == value.Key && x.Relation == TypeRelation.Incompatible).Select(x => x.EsclaveId).Distinct();
            var errors = incompat.Where(x => garanties.ContainsKey(x) && garanties[x].IsSelected && garanties[x].IsOverlapplingWith(garantie)).Select(x => garanties[x]).ToList();

            if (errors.Any()) {
                var plural = errors.Count() > 1;
                if (plural) {
                    yield return new ValidationError(
                        nameof(Garantie), garantie.CodeGarantie, garantie.ParamGarantie.Sequence.ToString(),
                        $"La garantie {garantie.CodeGarantie} est incompatible avec {string.Join(",", errors.Select(x => x.CodeGarantie))} qui sont sélectionnées ");
                }
                else {
                    yield return new ValidationError(
                        nameof(Garantie), garantie.CodeGarantie, garantie.ParamGarantie.Sequence.ToString(),
                        $"La garantie {garantie.CodeGarantie} est incompatible avec {string.Join(",", errors.Select(x => x.CodeGarantie))} qui est sélectionnée ");
                }
            }
        }

        private static IEnumerable<ValidationError> FormatValidationErrors(IEnumerable<ValidationError> errors) {
            var relationErrors = new SortedDictionary<int, RelationError>(errors
                .Select((e, i) => (e, i)).Where(x => x.e is RelationError)
                .ToDictionary(x => x.i, x => (RelationError)x.e));

            var validationErrors = new SortedDictionary<int, ValidationError>(errors
                .Select((e, i) => (e, i)).Where(x => !(x.e is RelationError))
                .ToDictionary(x => x.i, x => x.e));

            foreach (int order in relationErrors.Keys) {
                if (!validationErrors.Values.OfType<RelationError>().Any(e =>
                    e.TargetType == relationErrors[order].TargetType
                    && e.Relation == relationErrors[order].Relation
                    && e.ErrorType == relationErrors[order].ErrorType
                    && e.RelatedId == relationErrors[order].TargetId
                    && e.TargetId == relationErrors[order].RelatedId)) { validationErrors.Add(order, relationErrors[order]); }
            }

            return validationErrors.Values.ToArray();
        }


        private static IEnumerable<Garantie> BuildGarantieList(IEnumerable<Garantie> garantiesNiv1)
        {
            var g2 = garantiesNiv1.SelectMany(x => x.SousGaranties);
            if (g2.Any()) {
                return garantiesNiv1.Union(g2).Union(BuildGarantieList(g2));
            }
            return garantiesNiv1;
        }

        private IEnumerable<Garantie> BuildGarantieList()
        {
            var garanties = OptionVolets.SelectMany(x => x.Blocs.Select(y => (parentChecked: x.IsChecked, bloc: y))).SelectMany(x => x.bloc.Garanties);
            garanties = BuildGarantieList(garanties);
            return garanties;
        }

        private (OptionVolet volet, OptionBloc bloc, Garantie garatie) GetGareat() {
            var voletGareat = OptionVolets.FirstOrDefault(v => (v.ParamModele ?? v.Blocs.First().ParamModele).Code == Garantie.CodeGareat);
            if (voletGareat is null) {
                return default;
            }
            
            return (voletGareat, voletGareat.Blocs.First(), voletGareat.Blocs.First().Garanties.First());
        }

        internal void UpdateWithParametrage(Affaire.Affaire affaire, Dictionary<long, ParamVolet> paramVolets, TruthTable truthTable, TypologieModele mod, DateTime dateApplication)
        {
            foreach (var volet in this.OptionVolets) {
                volet.UpdateWithParametrage(affaire, this, paramVolets[volet.ParamVolet.CatVoletId], truthTable, mod, dateApplication);
            }
            foreach (var pVolet in paramVolets.Values) {
                if (!this.OptionVolets.Any(x => x.ParamVolet.CatVoletId == pVolet.CatVoletId)) {
                    var v = (new OptionVolet {
                        Id = 0,
                        IsChecked = (pVolet.Caractere == Model.CaractereSelection.Obligatoire || pVolet.Caractere == Model.CaractereSelection.DeBase || pVolet.Caractere == Model.CaractereSelection.Propose),
                        NumeroAvenant = null,
                        Ordre = pVolet.Ordre,
                        Type = TypeOption.Volet,
                        ParamModele = null,
                        ParamVolet = pVolet
                    });
                    v.UpdateWithParametrage(affaire, this, paramVolets[v.ParamVolet.CatVoletId], truthTable, mod, dateApplication);
                    if (v.Blocs.Any()) {
                        this.OptionVolets.Add(v);
                    }
                }
            }
            this.OptionVolets = this.OptionVolets.Where(v => v.Blocs.Any()).OrderBy(x => x.Ordre).ToList();

        }


        public Risque.Risque FilterRisques(IEnumerable<Risque.Risque> risques)
        {
            return risques.FirstOrDefault(x => Applications.Any(a => a.NumRisque == x.Numero));
        }
        public IEnumerable<Risque.Objet> FilterObjets(Risque.Risque risques)
        {
            return Applications.Any(x => x.Niveau == ApplicationNiveau.Risque)
                ? risques.Objets
                : risques.Objets.Where(x => Applications.Any(a => a.NumRisque == x.Id.NumRisque && a.NumObjet == x.Id.NumRisque) && x.IsFinishingAfter(DateAvenant));
        }

        public void UpdateApplications(IEnumerable<int> numObjets, Risque.Risque newRisque)
        {
            var apps = this.Applications;
            var isObjApp = apps.Any(x => x.Niveau == ApplicationNiveau.Objet);
            var isRisqueDifferent = apps.Any(x => x.NumRisque != newRisque.Numero);

            var shouldChange = isRisqueDifferent;
            if (!isRisqueDifferent) {
                shouldChange =
                    (
                        isObjApp
                        && !(
                            numObjets.All(x => apps.Any(a => a.NumObjet == x))
                            && apps.All(a => numObjets.Contains(a.NumObjet))
                        )
                    ) || (
                        !isObjApp && (numObjets.Count() != newRisque.Objets.Count())
                    );
            }

            if (shouldChange) {

                var isNewNiveauApplicationObjet = numObjets.Any() && numObjets.Count() != newRisque.Objets.Count();

                if (isNewNiveauApplicationObjet) {
                    this.Applications = numObjets.Select(o => new OP.Domain.Formule.Application {
                        Niveau = ApplicationNiveau.Objet,
                        NumObjet = o,
                        NumRisque = newRisque.Numero
                    }).ToList();

                    RemoveExtraPortees(numObjetsToKeep: numObjets);

                } else {

                    this.Applications = newRisque.MapSingle(r => new OP.Domain.Formule.Application {
                        Niveau = ApplicationNiveau.Risque,
                        NumObjet = 0,
                        NumRisque = newRisque.Numero
                    }).ToList();
                    if (isRisqueDifferent) {
                        RemoveExtraPortees(numObjetsToKeep: Enumerable.Empty<int>());
                    }
                }
            }
        }

        private void RemoveExtraPortees(IEnumerable<int> numObjetsToKeep)
        {
            var garWithPorteesToRemove = this.AllGaranties.Where(x => x.Portees.Any(p => !numObjetsToKeep.Contains(p.NumObjet)));
            foreach (var gar in garWithPorteesToRemove) {
                var porteesToRemove = gar.Portees.Where(p => !numObjetsToKeep.Contains(p.NumObjet)).ToList();
                foreach (var portee in porteesToRemove) {
                    gar.Portees.Remove(portee);
                }
            }
        }

        public IEnumerable<ValidationError> CheckDateAvenant(Risque.Risque risque, Affaire.Affaire affaire, Formule formule)
        {
            var dateRisqueAvenant = risque.DateEffetAvenant;
            var dateAffaireAvenant = affaire.DateEffetAvenant.Value;
            var dateMinimale = new[] { dateAffaireAvenant, dateRisqueAvenant }.Max();
            if (DateAvenant < dateRisqueAvenant) {
                yield return new ValidationError("Formule", formule.Alpha, this.Id.ToString(), "La date d'effet doit être supérieure à la date de modification du risque dans l'avenant");
            }
            if (DateAvenant < dateAffaireAvenant) {
                yield return new ValidationError("Formule", formule.Alpha, this.Id.ToString(), "La date d'effet doit être supérieure à la date d'effet de l'avenant");
            }
        }

        public void UpdateNumerosAvenant(Affaire.AffaireId affaire)
        {
            AllGaranties.Where(g => g.IsVirtual).ToList()
                .ForEach(g => {
                    if (!g.IsChecked
                        && (g.ParamGarantie.Caractere == Model.CaractereSelection.Propose
                            || g.ParamGarantie.Caractere == Model.CaractereSelection.Facultatif && g.ParamGarantie.Nature == NatureValue.Exclue)
                        || g.NatureRetenue == NatureValue.Exclue)
                    {
                        g.NumeroAvenantCreation = affaire.NumeroAvenant.Value;
                        g.NumeroAvenantModif = affaire.NumeroAvenant.Value;
                    }
                });
        }
    }
}