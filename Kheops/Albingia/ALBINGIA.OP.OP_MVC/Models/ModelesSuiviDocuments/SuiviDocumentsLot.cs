using EmitMapper;
using OP.WSAS400.DTO.SuiviDocuments;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesSuiviDocuments
{
    public class SuiviDocumentsLot
    {
        public Int64 LotId { get; set; }
        public string LotLibelle { get; set; }
        public string LotUser { get; set; }
        public string NomUser { get; set; }
        public string PrenomUser { get; set; }
        public string UniteService { get; set; }
        public string TypeAffaire { get; set; }
        public string CodeOffre { get; set; }
        public Int32 Version { get; set; }
        public string ActeGestion { get; set; }
        public string ActeGestionLib { get; set; }
        public Int32 NumInterne { get; set; }
        public Int64 NumExterne { get; set; }

        public List<SuiviDocumentsLotDetails> SuiviDocumentsListLotDetail { get; set; }


        public static explicit operator SuiviDocumentsLot(SuiviDocumentsLotDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SuiviDocumentsLotDto, SuiviDocumentsLot>().Map(modelDto);
        }

        public static SuiviDocumentsLotDto LoadDto(SuiviDocumentsLot modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SuiviDocumentsLot, SuiviDocumentsLotDto>().Map(modele);
        }
    }
}