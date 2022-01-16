using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesRetours;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleRetoursPage : MetaModelsBase
    {
        public int DateRetourPreneurActuel { get; set; }
        public int DateRetourPreneur { get; set; }
        public List<AlbSelectListItem> TypeAccordPreneurActuel { get; set; }
        public List<AlbSelectListItem> TypeAccordPreneur { get; set; }
        public string SelectedTypeAccordPreneurActuel { get; set; }
        public string SelectedTypeAccordPreneur { get; set; }
        public bool IsReglementRecu { get; set; }
        public string PathFichierJoint { get; set; }

        public List<ModeleLigneRetourCoAss> ListeRetoursCoAss { get; set; }
        //public FileDescriptions ModelFileDescriptions { get; set; }

      //public ModeleRetoursPage()
      //{
      //  ModelFileDescriptions = new FileDescriptions();
      //}
    }
}