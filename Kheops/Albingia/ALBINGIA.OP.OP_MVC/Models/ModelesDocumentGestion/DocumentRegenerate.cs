using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALBINGIA.OP.OP_MVC.WSFinOffre;
using EmitMapper;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class DocumentRegenerate
    {
        public Int64 IdDocument { get; set; }
        public string NomDocument { get; set; }
        public string TypeDocument { get; set; }
        public string OrigineDocument { get; set; }
        public Int64 LienLibre { get; set; }
        public string Transforme { get; set; }

        public string LibDoc { get; set; }

        public static explicit operator DocumentRegenerate(DocumentGestionRegenerateDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DocumentGestionRegenerateDto, DocumentRegenerate>().Map(modelDto);
        }

        public static DocumentGestionRegenerateDto LoadDto(DocumentRegenerate modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DocumentRegenerate, DocumentGestionRegenerateDto>().Map(modele);
        }
    }
}