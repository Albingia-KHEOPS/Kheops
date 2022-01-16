using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParamTemplates;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamTemplates
{
    public class ModeleLigneTemplate
    {
        public Int64 GuidId { get; set; }
        public string ModeSaisie { get; set; }

        public string CodeTemplate { get; set; }
        public string DescriptionTemplate { get; set; }
        public string TypeTemplate { get; set; }
        public Int64 CibleRef { get; set; }
        public string Situation { get; set; }

        public int DateCreation { get; set; }
        public string UserCreation { get; set; }
        public int DateModification { get; set; }
        public string UserModification { get; set; }

        public List<AlbSelectListItem> ListeTypesTemplate { get; set; }

        public static explicit operator ModeleLigneTemplate(ModeleLigneTemplateDto templateDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleLigneTemplateDto, ModeleLigneTemplate>().Map(templateDto);
        }

    }
}