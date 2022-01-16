using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleActeGestion;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleSuiviActesGestionPage : MetaModelsBase
    {
        public List<ActeGestion> ActesGestion { get; set; }
        public string Type { get; set; }
        public string CodeOffre { get; set; }
        public string Version { get; set; }

        public string DateTraitDeb { get; set; }
        public string DateTraitFin { get; set; }
        public string DateCreatDeb { get; set; }
        public string DateCreatFin { get; set; }
        public string Utilisateur { get; set; }
        public List<AlbSelectListItem> Utilisateurs { get; set; }
        public string Affichage { get; set; }
        public List<AlbSelectListItem> Affichages { get; set; }
        public string TypeTraitement { get; set; }
        public List<AlbSelectListItem> TypeTraitements { get; set; }
    }
}