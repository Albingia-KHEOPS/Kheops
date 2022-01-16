using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Parametrage.Formules
{
    /// <summary>
    /// ParamModeleGarantie (KCATMODELE + YPLMGA)
    /// </summary>
    public class ParamModeleGarantie
    {
        public Int32 CatId { get; set; }
        public string Description { get; set; }
        public TypologieModele Typo { get; set; }
        public string Code { get; set; }
        public virtual DateTime DateApplication { get; set; }

        public virtual ICollection<ParamGarantieHierarchie> Garanties { get; set; } = new List<ParamGarantieHierarchie>();

        public virtual ParamGarantieHierarchie FindGarantieBySeq(long seq)
        {
            return FindGarantieRec(this.Garanties, seq);
        }
        private ParamGarantieHierarchie FindGarantieRec(IEnumerable<ParamGarantieHierarchie> garanties, long seq)
        {
            if (!garanties.Any())
            {
                return null;
            }
            return garanties.SingleOrDefault(x => x.Sequence == seq) ?? FindGarantieRec(garanties.SelectMany(x => x.GarantiesChildren), seq);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var b = obj as ParamModeleGarantie;
            return b != null && b.CatId == this.CatId;
        }
        public override int GetHashCode()
        {
            return CatId.GetHashCode();
        }
    }
}
