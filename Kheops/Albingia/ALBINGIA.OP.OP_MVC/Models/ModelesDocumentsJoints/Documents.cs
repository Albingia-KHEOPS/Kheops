using EmitMapper;
using OP.WSAS400.DTO.DocumentsJoints;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentsJoints
{
    public class Documents
    {
        public bool IsReadOnly { get; set; }
        public Int64 DocumentId { get; set; }
        public DateTime? DateAjout { get; set; }
        public string DateAjoutString
        {
            get
            {
                if (DateAjout.HasValue)
                    return new DateTime(DateAjout.Value.Year, DateAjout.Value.Month, DateAjout.Value.Day).ToShortDateString();
                else
                    return string.Empty;
            }
        }

        public string ActeCode { get; set; }
        public string ActeLib { get; set; }
        public string TitreDocument { get; set; }
        public string NomFichier { get; set; }
        public string Reference { get; set; }
        public bool ReferenceCP { get; set; }
        public string Chemin { get; set; }

        public Int64 CodeAvnDoc { get; set; }
        public DateTime? DateAvn { get; set; }
        public string DateAvnString
        {
            get
            {
                if (DateAvn.HasValue)
                    return new DateTime(DateAvn.Value.Year, DateAvn.Value.Month, DateAvn.Value.Day).ToShortDateString();
                else
                    return string.Empty;
            }
        }

        public static explicit operator Documents(DocumentsDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DocumentsDto, Documents>().Map(modeleDto);
        }

        public static DocumentsDto LoadDto(Documents modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<Documents, DocumentsDto>().Map(modele);
        }
    }
}