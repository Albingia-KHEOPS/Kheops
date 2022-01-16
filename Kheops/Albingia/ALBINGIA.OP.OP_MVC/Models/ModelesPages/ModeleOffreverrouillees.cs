using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleRechercheOffreVerrouillee
    {
        public string NumeroOffre { get; set; }
        public string Utilisateur { get; set; }
    }

    public class ModeleUsersOffreVerrouillees
    {
        #region Propriétés
        public DateTime? DateVerouillage { get; set; }
        public bool Etat { get; set; }
        public string NumeroOffre { get; set; }
        public string Utilisateur { get; set; }
        #endregion

        public ModeleUsersOffreVerrouillees()
        {
            Etat = false;
            DateVerouillage = null;
        }

        public static explicit operator ModeleUsersOffreVerrouillees(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            string[] splitValue = value.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);//Regex.Split(value, MvcApplication.SPLIT_CONST_FILE);
            return new ModeleUsersOffreVerrouillees { Utilisateur = splitValue[1], NumeroOffre = splitValue[2] + "_" + splitValue[3], DateVerouillage = null, Etat = true };
        }

    }


}