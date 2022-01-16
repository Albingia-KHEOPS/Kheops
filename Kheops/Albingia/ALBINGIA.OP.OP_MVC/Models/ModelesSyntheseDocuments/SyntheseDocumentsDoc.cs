using EmitMapper;
using OP.WSAS400.DTO.SyntheseDocuments;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesSyntheseDocuments
{
    public class SyntheseDocumentsDoc
    {
        public Int64 LotId { get; set; }
        public Int64 Ordre { get; set; }
        public string TypeDestinataire { get; set; }
        public Int64 DestinataireId { get; set; }
        public string TypeEnvoi { get; set; }
        public string LibEnvoi { get; set; }
        public List<SyntheseDocumentsDocInfo> DocInfos { get; set; }


        public static explicit operator SyntheseDocumentsDoc(SyntheseDocumentsDocDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SyntheseDocumentsDocDto, SyntheseDocumentsDoc>().Map(modelDto);
        }

        public static SyntheseDocumentsDocDto LoadDto(SyntheseDocumentsDoc modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SyntheseDocumentsDoc, SyntheseDocumentsDocDto>().Map(modele);
        }
    }
}