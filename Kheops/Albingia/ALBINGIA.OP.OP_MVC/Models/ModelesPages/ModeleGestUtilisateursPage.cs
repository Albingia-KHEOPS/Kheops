using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesGestUtilisateurs;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleGestUtilisateursPage : MetaModelsBase
    {
        /// <summary>
        /// context de la page a affecter dans les hidden inputs
        /// </summary>
        public string Albcontext { get; set; }

        /// <summary>
        /// nom du context de la div flottante de recherche a affecter dans les hidden inputs
        /// </summary>
        public string RechercheAlbcontext { get; set; }

        public string Branche { get; set; }
        public string TypeDroit { get; set; }

        public string RechercheUtilisateur { get; set; }
        public string RechercheBranche { get; set; }
        public string RechercheTypeDroit { get; set; }

        public List<ModeleUtilisateurBranche> ModeleMappageUtiliDroitBranches;
        public ModeleUtilisateurBranche NewUtilisateurBranche;

        public List<AlbSelectListItem> Branches { get; set; }
        public List<AlbSelectListItem> TypeDroits { get; set; }

        public bool RechercheActive { get; set; }
        public bool AddUserEnabled { get; set; }
        public bool ReloadPageBack { get; set; }

        public ModeleGestUtilisateursPage()
        {
            NewUtilisateurBranche = new ModeleUtilisateurBranche();
        }
    }
}