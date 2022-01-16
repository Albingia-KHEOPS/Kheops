using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class Gestionnaire
    {
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                if (!String.IsNullOrEmpty(value) && value.Length > 10)
                {
                    throw new Exception("Le codeGestionnaire de l'offre ne doit pas exceder 10 caracteres");
                }
                id = value;
            }
        }
        public string Login { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public Branche Branche { get; set; }
    }
}
