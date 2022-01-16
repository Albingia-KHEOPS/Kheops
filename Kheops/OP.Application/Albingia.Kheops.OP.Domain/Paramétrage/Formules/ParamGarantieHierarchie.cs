using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Parametrage.Formules
{
    public class ParamGarantieHierarchie
    {
        /// <summary>
        /// Sequence Id
        /// </summary>
        public long Sequence { get; set; }

        public string Description { get { return ParamGarantie?.DesignationGarantie; } }

        public string Script { get; set; }

        public int Ordre { get; set; }
        public CaractereSelection Caractere { get; set; }

        public int Niveau { get; set;  }

        public NatureValue Nature { get; set; }

        //public ParamGarantie ParamGarantie { get; set;  }

        public virtual bool? IsAlimMontantReference { get; set; }
        public virtual bool? IsApplicationCATNAT { get; set; }
        public TypeControleDateEnum TypeControleDate { get; set; }

        public virtual Dictionary<TypeDeValeur, ValeurDeGarantie> ValeurDeGaranties { get; set; } = new Dictionary<TypeDeValeur,ValeurDeGarantie>();

        public ValeurDeGarantie Assiette => ValeurDeGaranties[TypeDeValeur.Assiette];
        public ValeurDeGarantie Prime => ValeurDeGaranties[TypeDeValeur.Prime];
        public ValeurDeGarantie LCI => ValeurDeGaranties[TypeDeValeur.LCI];
        public ValeurDeGarantie Franchise => ValeurDeGaranties[TypeDeValeur.Franchise];
        public ValeurDeGarantie FranchiseMin => ValeurDeGaranties[TypeDeValeur.FranchiseMin];
        public ValeurDeGarantie FranchiseMax => ValeurDeGaranties[TypeDeValeur.FranchiseMax];


        public virtual ParamGarantie ParamGarantie { get; set; } = null;
        public virtual ICollection<ParamGarantieHierarchie> GarantiesChildren { get; set; } = new List<ParamGarantieHierarchie>();
        public Taxe Taxe { get; set; }
        public bool? IsIndexee { get; set; }
        public bool? IsNatModifiable { get; set; }
        public string Tri { get; set; }

        public override bool Equals(object obj)
        {
            var b = obj as ParamGarantieHierarchie;
            return (b != null && this.Sequence == b.Sequence);
        }
        public override int GetHashCode()
        {
            return Sequence.GetHashCode();
        }
    }
}
