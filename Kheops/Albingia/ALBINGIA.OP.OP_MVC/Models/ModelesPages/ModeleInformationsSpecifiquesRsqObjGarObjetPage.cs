using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleInformationsSpecifiquesRsqObjGarObjetPage : MetaModelsBase
    {
        public ModeleInformationsSpecifiquesObjetsPage ModeleInformationsSpecifiquesObjetsPage { get; set; }
        [Display(Name = "Objet")]
        public String Objet { get; set; }      
        public List<AlbSelectListItem> Objets { get; set; }
    }
}