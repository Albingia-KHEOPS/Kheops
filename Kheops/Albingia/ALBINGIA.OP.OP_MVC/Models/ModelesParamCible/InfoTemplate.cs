using EmitMapper;
using OP.WSAS400.DTO.ParamTemplates;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class InfoTemplate
    {
        public Int64 GuidId { get; set; }
        public string CodeTemplate { get; set; }
        public string DescriptionTemplate { get; set; }
        public string TypeTemplate { get; set; }
        public string DefaultTemplate { get; set; }
        public bool Default
        {
            get
            {
                return DefaultTemplate == "O";
            }
        }


        public static explicit operator InfoTemplate(ModeleLigneTemplateDto infoDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleLigneTemplateDto, InfoTemplate>().Map(infoDto);
        }

        public static ModeleLigneTemplateDto LoadDto(InfoTemplate modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<InfoTemplate, ModeleLigneTemplateDto>().Map(modele);
        }
    
    }
}