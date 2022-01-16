using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleGestionOptionsPage : MetaModelsBase
    {
        public List<AlbSelectListItem> Options { get; set; }
        public List<AlbSelectListItem> InformationsSpecifiques { get; set; }
    }
}