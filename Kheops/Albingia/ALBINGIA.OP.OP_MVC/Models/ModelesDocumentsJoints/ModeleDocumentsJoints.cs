using EmitMapper;
using OP.WSAS400.DTO.DocumentsJoints;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentsJoints
{
    public class ModeleDocumentsJoints 
    {
        public bool ReadOnly { get; set; }
        public string Type { get; set; }
        public string CodeOffre { get; set; }
        public string CodeAvn { get; set; }

        public List<Documents> ListDocuments { get; set; }

        public bool IsValide { get; set; }

        public static explicit operator ModeleDocumentsJoints(DocumentsJointsDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DocumentsJointsDto, ModeleDocumentsJoints>().Map(modeleDto);
        }

        public static DocumentsJointsDto LoadDto(ModeleDocumentsJoints modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleDocumentsJoints, DocumentsJointsDto>().Map(modele);
        }
    }
}