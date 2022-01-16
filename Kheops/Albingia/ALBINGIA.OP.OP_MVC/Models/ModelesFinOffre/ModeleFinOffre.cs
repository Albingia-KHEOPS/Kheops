using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFinOffre
{
    public class ModeleFinOffre
    {

        public ModeleFinOffreInfos ModeleFinOffreInfos { get; set; }
        public ModeleFinOffreAnnotation ModeleFinOffreAnnotation { get; set; }

        public ModeleFinOffre()
        {
            ModeleFinOffreInfos = new ModeleFinOffreInfos();
            ModeleFinOffreAnnotation = new ModeleFinOffreAnnotation();
        }
    }
}