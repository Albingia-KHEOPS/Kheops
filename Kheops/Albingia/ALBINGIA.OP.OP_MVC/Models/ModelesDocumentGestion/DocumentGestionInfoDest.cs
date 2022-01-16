using EmitMapper;
using OP.WSAS400.DTO.DocumentGestion;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class DocumentGestionInfoDest
    {
        public string RaisonSociale { get; set; }
        public string NomInter { get; set; }
        public string PrenomInter { get; set; }
        public string EmailInter { get; set; }
        public string Batiment { get; set; }
        public Int64 NumeroVoie { get; set; }
        public string ExtensionVoie { get; set; }
        public string NomVoie { get; set; }
        public string NomVille { get; set; }
        public string CodePostal { get; set; }
        public string TypeEnvoi { get; set; }

        public static explicit operator DocumentGestionInfoDest(DocumentGestionInfoDestDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DocumentGestionInfoDestDto, DocumentGestionInfoDest>().Map(modelDto);
        }

        public static DocumentGestionInfoDestDto LoadDto(DocumentGestionInfoDest modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DocumentGestionInfoDest, DocumentGestionInfoDestDto>().Map(modele);
        }
    }
}