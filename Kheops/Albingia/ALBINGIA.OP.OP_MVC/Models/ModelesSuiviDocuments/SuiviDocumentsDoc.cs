using EmitMapper;
using OP.WSAS400.DTO.SuiviDocuments;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesSuiviDocuments
{
    public class SuiviDocumentsDoc
    {
        public Int64 DocId { get; set; }
        public string TypeDoc { get; set; }
        public string CodeDoc { get; set; }
        public string TypeDocLib { get; set; }
        public string NomDoc { get; set; }
        public string CheminDoc { get; set; }


        public static explicit operator SuiviDocumentsDoc(SuiviDocumentsDocDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SuiviDocumentsDocDto, SuiviDocumentsDoc>().Map(modelDto);
        }

        public static SuiviDocumentsDocDto LoadDto(SuiviDocumentsDoc modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SuiviDocumentsDoc, SuiviDocumentsDocDto>().Map(modele);
        }
    }
}