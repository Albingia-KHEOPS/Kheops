using EmitMapper;
using OP.WSAS400.DTO.DocumentGestion;
using System;
using System.IO;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class DocumentGestionDocInfo
    {
        public Int64 LotId { get; set; }
        public string Situation { get; set; }
        public string Nature { get; set; }
        public string Imprimable { get; set; }
        public string Chemin { get; set; }
        public string Statut { get; set; }
        public string TypeDoc { get; set; }
        public Int64 IdDoc { get; set; }
        public string NomDoc { get; set; }
        public string LibDoc { get; set; }
        public bool IsModifiable { get; set; }
        public Int64 IdLotDetail { get; set; }
        public string TypeDestinataire { get; set; }
        public Int64 Destinataire { get; set; }
        public string CodeTypeEnvoi { get; set; }
        public string TypeEnvoi { get; set; }
        public Int64 NbExemple { get; set; }
        public Int64 NbExempleSupp { get; set; }
        public string Tampon { get; set; }
        public string LibTampon { get; set; }
        public Int64 IdLettre { get; set; }
        public string TypeLettre { get; set; }
        public string LettreAccomp { get; set; }
        public string LibLettre { get; set; }
        public string Email { get; set; }
        public bool IsLibre { get; set; }
        public string Extension { get; set; }

        public static explicit operator DocumentGestionDocInfo(DocumentGestionDocInfoDto modelDto)
        {
            var result = ObjectMapperManager.DefaultInstance.GetMapper<DocumentGestionDocInfoDto, DocumentGestionDocInfo>().Map(modelDto);
            if (!string.IsNullOrEmpty(result.NomDoc.Trim()))
            {
                var file = new FileInfo(result.NomDoc.Trim());
                result.Extension = file.Extension;
            }
            result.IsModifiable = true;
            return result;
        }

        public static DocumentGestionDocInfoDto LoadDto(DocumentGestionDocInfo modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DocumentGestionDocInfo, DocumentGestionDocInfoDto>().Map(modele);
        }
    }
}