using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Parametrage.Inventaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Inventaire {
    public class Inventaire {
        public AffaireId Affaire { get; set; }
        //public string LienGaran    

        public long Id { get; set; }
        public int NumChrono { get; set; }
        public string Descriptif { get; set; }
        public TypeInventaire TypeInventaire { get; set; }
        public string Designation { get; set; }
        public long ChronoDesi { get; set; }

        public bool? ReportvaleurObjet { get; set; }

        public ValeursUnite Valeurs { get; set; } = new ValeursUnite();

        public TypeValeurRisque Typedevaleur { get; set; }
        public bool? IsHTnotTTC { get; set; }
        public Valeurs ValeursIndice { get; set; } = new Valeurs();

        public ICollection<InventaireItem> Items { get; set; } = new List<InventaireItem>();

        public void EnsureItemNumbers() {
            var numbers = Items.Select(i => i.NumeroLigne).Distinct().ToArray();
            if (numbers.Length == 1 && numbers[0] == 0 || numbers.Length != Items.Count) {
                int i = numbers.Max();
                foreach (var item in Items) {
                    if(item.NumeroLigne==0)
                        item.NumeroLigne = ++i;
                }
            }
        }
        public InventaireItem MakeItem() {
            return MakeItem(this.TypeInventaire.TypeItem);
        }
        internal static InventaireItem MakeItem(TypeInventaireItem type)
        {
            switch (type) {
                case TypeInventaireItem.ManifestationAssurees:
                    return new ManifestationAssure();
                case TypeInventaireItem.TournagesAssures:
                    return new TournageAssure();
                case TypeInventaireItem.PersonneDesigneeIndispo:
                    return new PersonneDesigneeIndispo();
                case TypeInventaireItem.PersonneDesignee:
                    return new PersonneDesignee();
                case TypeInventaireItem.PersonneDesigneeIndispoTournage:
                    return new PersonneDesigneeIndispoTournage();
                case TypeInventaireItem.Materielsassures:
                    return new MaterielAssure();
                case TypeInventaireItem.Biensassures:
                    return new BienAssure();
                case TypeInventaireItem.Postesassures:
                    return new PosteAssure();
                case TypeInventaireItem.MultiplesSituations:
                    return new MultipleSituation();
                case TypeInventaireItem.Audioproduction:
                    return new ProductionRealisationAudio();
                case TypeInventaireItem.Audiolocation:
                    return new LocationTiers();
                case TypeInventaireItem.ProImmo:
                    return new ActivitesImmobilieres();
                case TypeInventaireItem.Marchandises:
                    return new MarchandisesTransportees();
                case TypeInventaireItem.Stockage:
                    return new MarchandisesStockees();
                case TypeInventaireItem.ParcVehicules:
                    return new VehiculePourTransport();
                default:
                    throw new NotSupportedException($"{type} n'est pas un type d'inventaire recconu");
            }
        }

        internal void ResetIds() {
            Id = default;
            ChronoDesi = default;
            foreach(var i in Items) {
                i.ResetIds();
            }
        }
    }
}
