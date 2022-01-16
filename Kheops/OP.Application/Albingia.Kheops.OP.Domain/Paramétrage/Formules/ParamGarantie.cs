using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Inventaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Parametrage.Formules
{
    public class ParamGarantie
    {
        public string CodeGarantie { get; set; }
        public string DesignationGarantie { get; set; }
        public string AbregeGarantie { get; set; }
        public Taxe Taxe { get; set; } = new Taxe();
        public Taxe CodeTaxeCatNat { get; set; } = new Taxe();
        public bool? IsGarantCommune { get; set; }
        public CaractereSelection CaractereGarantie { get; set; }
        public DefinitionGarantie DefinitionGarantie { get; set; } = new DefinitionGarantie();
        public string Infocomplementaire { get; set; }
        public FamilleGarantie FamilleGarantie { get; set; } = new FamilleGarantie();
        public bool? IsRegularisable { get; set; }
        public bool? IsInventPossible { get; set; }
        public TypeInventaire TypeInventaire { get; set; } = new TypeInventaire();
        public TypeGrilleRegul GrilleRegul { get; set; } = new TypeGrilleRegul();
        public bool IsAttentatGareat { get; set; }

        public override bool Equals(object obj)
        {
            var b = obj as ParamGarantie;
            return (b != null && this.CodeGarantie == b.CodeGarantie);
        }
        public override int GetHashCode()
        {
            return CodeGarantie.GetHashCode();
        }
    }
}
