using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie
{
    public class DetailsGarantie
    {
        public DetailsGarantie()
        {
            DureeUnites = new List<LabeledValue>();
            Applications = new List<LabeledValue>();
            CodesTaxes = new List<LabeledValue>();
            AlimentationsAssiettes = new List<LabeledValue>();
            NatureRetenue = new LabeledValue(null, null);
            DetailsValeurs = new DetailsValeursGarantie();
        }

        public long Sequence { get; set; }

        public string Code { get; set; }

        public string Libelle { get; set; }

        public string CodeBloc { get; set; }

        public string CodeCaractere { get; set; }

        public string Definition { get; set; }

        public string CodeNature { get; set; }

        public LabeledValue NatureRetenue { get; set; }

        public int AvenantInitial { get; set; }

        public DateTime DateDebutStd { get; set; }

        public DateTime DateFinStd { get; set; }

        public DateTime? DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public bool IsDuree { get; set; }

        public int? Duree { get; set; }

        public string DureeUnite { get; set; }

        public IEnumerable<LabeledValue> DureeUnites { get; set; }

        public bool IsGarantieIndexee { get; set; }

        public bool HasLCI { get; set; }

        public bool HasFranchise { get; set; }

        public bool HasAssiette { get; set; }

        public bool HasCATNAT { get; set; }

        public bool IsTemporaire { get; set; }

        public string CodeTypeApplication { get; set; }

        public IEnumerable<LabeledValue> Applications { get; set; }

        public bool InclusMontant { get; set; }

        public string CodeTypeEmission { get; set; }

        public IEnumerable<LabeledValue> TypesEmissions { get; set; }

        public bool Regularisable { get; set; }

        public string GrilleRegul { get; set; }

        public string CodeTaxe { get; set; }

        public IEnumerable<LabeledValue> CodesTaxes { get; set; }

        public string CodeAlimentationAssiette { get; set; }

        public IEnumerable<LabeledValue> AlimentationsAssiettes { get; set; }

        public DetailsValeursGarantie DetailsValeurs { get; set; }
    }
}
