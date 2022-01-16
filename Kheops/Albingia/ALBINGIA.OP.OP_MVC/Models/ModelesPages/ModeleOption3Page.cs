using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleOption3;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleOption3Page : MetaModelsBase
    {
        public string Branche { get; set; }
        public string Cible { get; set; }
        public string LibelleFormule { get; set; }
        public string NomFormule { get; set; }
        public string Option { get; set; }
        public string Applique { get; set; }

        public List<AlbSelectListItem> Categories { get; set; }

        /// <summary>
        /// Liste des risques et objets de l'offre
        /// </summary>
        public ModeleOption3ListeRisques ObjetsRisquesAll { get; set; }
    }
}