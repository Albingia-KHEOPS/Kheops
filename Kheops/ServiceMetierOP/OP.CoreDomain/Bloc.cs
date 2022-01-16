using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
   // //[Serializable]
    public class Bloc
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreation { get; set; }
        public string Caractere { get; set; }
        public string GuidId { get; set; }

        public List<Modele> Modeles { get; set; }
        //public List<GarantieModeleNiveau1> Modeles { get; set; }

        public string CodeOption { get; set; }

        /// <summary>
        /// récupère des tables finales si le volet est coché ou non pour les formules de garantie
        /// </summary>
        public bool isChecked { get; set; }
             
        public double OrdreBloc { get; set; }
    }
}
