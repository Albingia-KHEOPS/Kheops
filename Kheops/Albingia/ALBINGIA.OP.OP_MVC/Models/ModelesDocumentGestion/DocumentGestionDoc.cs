using EmitMapper;
using OP.WSAS400.DTO.DocumentGestion;
using System;
using System.Collections.Generic;
using System.IO;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class DocumentGestionDoc
    {
        public Int64 DocId { get; set; }
        public List<DocumentGestionDocInfo> ListDocInfos { get; set; }
        public bool FirstGeneration { get; set; }

        ////TODO à enlever à terme
        //public string Situation { get; set; }
        //public string Nature { get; set; }
        //public string Imprimable { get; set; }
        //public string Chemin { get; set; }
        //public string Statut { get; set; }
        //public string TypeDoc { get; set; }
        //public string NomDoc { get; set; }
        //public bool IsModifiable { get; set; }
        ////Fin TODO

        public static explicit operator DocumentGestionDoc(DocumentGestionDocDto modelDto)
        {
            var result = ObjectMapperManager.DefaultInstance.GetMapper<DocumentGestionDocDto, DocumentGestionDoc>().Map(modelDto);
            foreach (var item in result.ListDocInfos)
            {
                if (!string.IsNullOrEmpty(item.NomDoc.Trim()))
                {
                    var file = new FileInfo(item.NomDoc.Trim());
                    item.Extension = file.Extension;
                }
                item.IsModifiable = true;
            }
            return result;
        }

        public static DocumentGestionDocDto LoadDto(DocumentGestionDoc modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DocumentGestionDoc, DocumentGestionDocDto>().Map(modele);
        }
    }
}