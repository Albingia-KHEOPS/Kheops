
using Albingia.Kheops.OP.Domain.Referentiel;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public class MaterielAssure : Materiel
    {
        public string Reference { get; set; }

        public TypeMateriel Code { get; set; }
    }
}