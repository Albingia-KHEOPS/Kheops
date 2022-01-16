using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Referentiel 
{
    public class Utilisateur
    {
        public virtual string Code { get; set; }
        public virtual string Nom { get; set; }
        public virtual string Prenom { get; set; }
        public virtual string Login { get; set; }
        public virtual Branche Branche { get; set; }

        public virtual bool IsGestionnaire { get; set; }
        public virtual bool IsSouscripteur { get; set; }
    }
}
