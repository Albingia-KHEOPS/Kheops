using EmitMapper;
using OP.WSAS400.DTO.SuiviDocuments;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesSuiviDocuments
{
    public class SuiviDocumentsLotDetails
    {
        public Int64 LotDetailId { get; set; }
        public string CodeSituation { get; set; }
        public Int32 DateSituation { get; set; }
        public Int32 HeureSituation { get; set; }
        public string UserSituation { get; set; }
        public string TypeDestinataire { get; set; }
        public string TypeIntervenant { get; set; }
        public Int32 CodeDestinataire { get; set; }
        public string NomDestinataire { get; set; }
        public Int32 CodeInterlocuteur { get; set; }
        public string NomInterlocuteur { get; set; }
        public string CodeDiffusion { get; set; }
        public string LibDiffusion { get; set; }

        public List<SuiviDocumentsDoc> SuiviDocumentsListDoc { get; set; }


        public static explicit operator SuiviDocumentsLotDetails(SuiviDocumentsDocDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SuiviDocumentsDocDto, SuiviDocumentsLotDetails>().Map(modelDto);
        }

        public static SuiviDocumentsDocDto LoadDto(SuiviDocumentsLotDetails modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SuiviDocumentsLotDetails, SuiviDocumentsDocDto>().Map(modele);
        }
    }
}