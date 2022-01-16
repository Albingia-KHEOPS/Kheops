using EmitMapper;
using OP.WSAS400.DTO.ParametreCibles;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class EditTemplate
    {
        public bool isReadOnly { get; set; }
        public string Cible { get; set; }
        public List<InfoTemplate> Templates { get; set; }


        public static explicit operator EditTemplate(EditTemplateDto infoDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<EditTemplateDto, EditTemplate>().Map(infoDto);
        }

        public static EditTemplateDto LoadDto(EditTemplate modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<EditTemplate, EditTemplateDto>().Map(modele);
        }
    }
}