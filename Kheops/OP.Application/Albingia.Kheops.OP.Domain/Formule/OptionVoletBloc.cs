using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formule;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using System;
using System.Collections.Generic;
using System.Linq;
using TruthTable = System.Collections.Generic.IDictionary<(Albingia.Kheops.OP.Domain.Model.CaractereSelection caractere, Albingia.Kheops.OP.Domain.Referentiel.NatureValue nature), Albingia.Kheops.OP.Domain.Parametrage.Formule.NatureSelection>;

namespace Albingia.Kheops.OP.Domain.Formule
{
    public abstract class OptionDetail : IValidatable
    {
        public decimal Ordre { get; set; }
        public TypeOption Type { get; set; }
        public ParamModeleGarantie ParamModele { get; set; }
        public int? NumeroAvenant { get; set; }
        public long Id { get; set; }
        public ParamVolet ParamVolet { get; set; }
        public bool IsChecked { get; set; }
        public abstract IEnumerable<ValidationError> Validate();
        public abstract CaractereSelection Caractere { get; }

    }

    public class OptionVolet : OptionDetail
    {
        internal void SetParent(Option option)
        {
            this.parent = option;
            this.Blocs.ForEach(b => b.SetParent(this));
        }
        internal Option parent;
        public virtual Garantie FindGarantieById(long id)
        {
            return this.Blocs.Select(x => x.FindGarantieById(id)).FirstOrDefault();
        }

        public virtual Garantie FindGarantieBySeq(string codeBloc, long id)
        {
            var bloc = Blocs.FirstOrDefault(b => b.ParamBloc.Code == codeBloc);
            var garantie = bloc?.FindGarantieBySeq(id);
            return garantie;
        }

        public List<OptionBloc> Blocs { get; set; } = new List<OptionBloc>();
        public virtual IEnumerable<Garantie> AllGaranties => Blocs.SelectMany(x => x.AllGaranties);
        public virtual IEnumerable<Garantie> AllUncheckedGaranties => Blocs.SelectMany(x => x.IsChecked ? x.AllUncheckedGaranties : x.AllGaranties);
        public virtual IEnumerable<Garantie> AllCheckedGaranties => Blocs.SelectMany(x => x.IsChecked ? x.AllCheckedGaranties : Enumerable.Empty<Garantie>());
        public override CaractereSelection Caractere => this.ParamVolet.Caractere;

        public override IEnumerable<ValidationError> Validate()
        {
            if (!IsChecked)
            {
                return Enumerable.Empty<ValidationError>();
            }
            var errors = new List<ValidationError>();
            if (IsChecked && Blocs.Any() && Blocs.All(x => !x.IsChecked))
            {
                errors.Add(new ValidationError("Volet", ParamVolet.Code, ParamVolet.CatVoletId.ToString(), $"Volet {ParamVolet.Code} coché sans enfant coché"));
            }
            return errors.Concat(Blocs.Where(x => x.IsChecked).SelectMany(x => x.Validate()));
        }

        internal void UpdateWithParametrage(Affaire.Affaire affaire, Option opt, ParamVolet paramVolet, TruthTable truthTable, TypologieModele typo, DateTime dateEffet)
        {
            foreach (var bloc in this.Blocs)
            {
                bloc.UpdateWithParametrage(affaire, IsChecked, opt, paramVolet.Blocs.FirstOrDefault(x => x.CatBlocId == bloc.ParamBloc.CatBlocId), truthTable, typo, dateEffet);
            }
            foreach (var pbloc in paramVolet.Blocs)
            {
                if (!this.Blocs.Any(x => x.ParamBloc.CatBlocId == pbloc.CatBlocId))
                {
                    var b = new OptionBloc {
                        Id = 0,
                        IsChecked = (pbloc.Caractere == CaractereSelection.Obligatoire || pbloc.Caractere == CaractereSelection.DeBase || pbloc.Caractere == CaractereSelection.Propose),
                        NumeroAvenant = null,
                        Ordre = pbloc.Ordre,
                        Type = TypeOption.Bloc,
                        ParamModele = pbloc.FindApplicableModele(typo, dateEffet),
                        ParamVolet = paramVolet,
                        ParamBloc = pbloc,
                    };
                    if (b.ParamModele != null)
                    {
                        b.UpdateWithParametrage(affaire, IsChecked, opt, paramVolet.Blocs.FirstOrDefault(x => x.CatBlocId == b.ParamBloc.CatBlocId), truthTable, typo, dateEffet);

                        this.Blocs.Add(b);
                    }
                }
            }
            this.Blocs = this.Blocs.Where(b => b.Garanties.Any()).OrderBy(x => x.Ordre).ToList();
        }

        internal void ResetIds() {
            Id = default;
            foreach (var bloc in Blocs) {
                bloc.ResetIds();
            }
        }
    }

    public class OptionBloc : OptionDetail
    {
        internal OptionVolet parent;
        public ParamBloc ParamBloc { get; set; }
        public override CaractereSelection Caractere => this.ParamBloc.Caractere;

        public virtual Garantie FindGarantieById(long id)
        {
            return this.Garanties.Select(x => x.FindGarantieById(id)).FirstOrDefault(x => x != null);
        }
        public virtual Garantie FindGarantieBySeq(long id)
        {
            return Garanties.FirstOrDefault(x => x.ParamGarantie.Sequence == id)
                ?? Garanties.Select(x => x.FindGarantieBySeq(id)).FirstOrDefault(x => x != null);
        }

        public virtual IEnumerable<Garantie> AllGaranties => Garanties.SelectMany(x => x.Flatten());
        public virtual IEnumerable<Garantie> AllUncheckedGaranties => Garanties.SelectMany(x => x.IsChecked ? x.AllUncheckedGaranties : x.Flatten());
        public virtual IEnumerable<Garantie> AllCheckedGaranties => Garanties.SelectMany(x => x.IsChecked ? x.AllCheckedGaranties : Enumerable.Empty<Garantie>());
        public override IEnumerable<ValidationError> Validate()
        {
            if (!IsChecked)
            {
                return Enumerable.Empty<ValidationError>();
            }
            var errors = new List<ValidationError>();
            if (IsChecked && Garanties.Any() && Garanties.All(x => !x.IsChecked))
            {
                errors.Add(new ValidationError("Volet", ParamVolet.Code, ParamVolet.CatVoletId.ToString(), $"Bloc {ParamBloc.Code} coché sans enfant coché"));
            }
            return errors.Concat(Garanties.Where(g => g.IsChecked).SelectMany(x => x.Validate()));
        }

        public List<Garantie> Garanties { get; set; } = new List<Garantie>();
        public List<BlocRelation> ParamRelations { get; set; } = new List<BlocRelation>();

        internal void UpdateWithParametrage(Affaire.Affaire affaire, bool parentsChecked, Option opt, ParamBloc paramBloc, TruthTable table, TypologieModele typo, DateTime dateEffet)
        {
            var prefix = string.Empty;
            var usedModel = this.ParamBloc.FindApplicableModele(typo, dateEffet);

            //si pas de model à la date choisie
            if(usedModel == null)
            {
                this.ParamModele = null;
                this.Garanties.Clear();
                return;
            }

            if (!usedModel.Equals(this.ParamModele))
            {
                this.ParamModele = usedModel;
                this.Garanties.Clear();
            }

            foreach (var garMod in ParamModele.Garanties)
            {
                var gar = this.FindGarantieBySeq(garMod.Sequence);
                if (gar != null)
                {
                    gar.ApplyParameters(affaire, parentsChecked, ParamModele.FindGarantieBySeq(gar.ParamGarantie.Sequence), table, level: 0);
                }
                else
                {
                    var ischecked = this.IsChecked && parentsChecked;
                    gar = Garantie.InitWithParameter(affaire, this.IsChecked && parentsChecked, garMod, table);
                    this.Garanties.Add(gar);
                }
            }
            this.Garanties = this.Garanties.OrderBy(x => x.Tri).ToList();
        }

        internal void SetParent(OptionVolet optionVolet)
        {
            this.parent = optionVolet;
            this.Garanties.ForEach(g => g.SetBlocParent(this));
        }

        internal void ResetIds() {
            Id = default;
            Garanties.ForEach(g => g.ResetIds());
        }

        internal bool IsSelected => parent.IsChecked && this.IsChecked;
    }
}
