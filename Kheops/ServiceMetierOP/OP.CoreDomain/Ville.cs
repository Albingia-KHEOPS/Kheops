using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class Ville
    {
        public string CodeInsee { get; set; }
        public string Nom { get; set; }
        public string CodePostal { get; set; }
        public string NomCedex { get; set; }
        public string CodePostalCedex { get; set; }
        public bool TypeCedex { get; set; }

        public string NomSansCedex
        {
            get
            {
                string result = Nom;
                foreach (string s in Ville.IdentifiantsCedex)
                {
                    result = result.Replace(s, String.Empty);
                }
                return result;
            }
        }

        public static List<string> IdentifiantsCedex = new List<string> { "CEDEX", "CDX" };

        public string Departement
        {
            get
            {
                string result = String.Empty;
                if (!String.IsNullOrEmpty(this.CodePostal) && this.CodePostal.Length > 1)
                {
                    // 2 étant la longueur du département, attention à la guadeloupe...
                    result = this.CodePostal.Substring(0, 2);
                }
                else if (!String.IsNullOrEmpty(this.CodePostalCedex) && this.CodePostalCedex.Length > 1)
                {
                    result = this.CodePostalCedex.Substring(0, 2);
                }
                else
                {
                    result = this.CodePostal;
                }
                return result;
            }
        }


    }
}
