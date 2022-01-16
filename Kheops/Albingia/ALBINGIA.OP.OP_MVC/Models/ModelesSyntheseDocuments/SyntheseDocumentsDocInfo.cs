using EmitMapper;
using OP.WSAS400.DTO.SyntheseDocuments;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesSyntheseDocuments
{
    public class SyntheseDocumentsDocInfo
    {
        public Int64 DocId { get; set; }
        public string Document { get; set; }
        public Int64 NbExemp { get; set; }
        public string Imprim { get; set; }


        public static explicit operator SyntheseDocumentsDocInfo(SyntheseDocumentsDocInfoDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SyntheseDocumentsDocInfoDto, SyntheseDocumentsDocInfo>().Map(modelDto);
        }

        public static SyntheseDocumentsDocInfoDto LoadDto(SyntheseDocumentsDocInfo modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SyntheseDocumentsDocInfo, SyntheseDocumentsDocInfoDto>().Map(modele);
        }
    }
}