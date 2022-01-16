using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class Volet
    {
        public string Code { get; set; }
        public string  Description { get; set; }
        public DateTime? DateCreation { get; set; }
        public string Caractere { get; set; }
        public string GuidId { get; set; }
        public string Branche { get; set; }

        public List<Categorie> CategoriesVolet { get; set; }
        public List<Categorie> Categories { get; set; }

        public List<Bloc> Blocs { get; set; }

        public string CodeOption { get; set; }
        /// <summary>
        /// récupère des tables finales si le volet est coché ou non pour les formules de garantie
        /// </summary>
        public bool isChecked { get; set; }
        public string IsVoletGeneral { get; set; }
    }
}
