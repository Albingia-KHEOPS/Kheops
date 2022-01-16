using Albingia.Kheops.OP.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Parametrage.Formules
{
    /// <summary>
    /// Bloc parameter / KCATBLOC + KBLOC
    /// </summary>
    public class ParamBloc
    {
        public long BlocId { get; set; }
        public long CatBlocId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public CaractereSelection Caractere { get; set; }
        public decimal Ordre { get; set; }


        public virtual ICollection<ParamModeleGarantie> Modeles { get; set; } = new List<ParamModeleGarantie>();

        public virtual ParamModeleGarantie FindApplicableModele(TypologieModele mod, DateTime dateApplication)
        {
            return FindModelTypoDate(mod, dateApplication) ;
        }

        private ParamModeleGarantie FindModelTypoDate(TypologieModele mod, DateTime dateApplication)
        {
            return Modeles
                .Where(x=> x.Typo >= mod && x.DateApplication <= dateApplication)
                .OrderBy(x => x.Typo).ThenByDescending(x=>x.DateApplication)
                .FirstOrDefault();
        }
    }
}
