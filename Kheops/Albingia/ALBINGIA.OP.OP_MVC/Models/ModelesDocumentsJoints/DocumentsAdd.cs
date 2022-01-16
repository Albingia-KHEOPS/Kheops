using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.DocumentsJoints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentsJoints
{
    public class DocumentsAdd
    {
        public Int64 DocumentId { get; set; }
        [Display(Name="Type *")]
        public string TypeDoc { get; set; }
        public List<AlbSelectListItem> TypesDoc { get; set; }
        [Display(Name = "Titre *")]
        public string Titre { get; set; }
        [Display(Name = "Fichier *")]
        public string Fichier { get; set; }
        public string Chemin { get; set; }
        [Display(Name = "Référence")]
        public string Reference { get; set; }


        public static explicit operator DocumentsAdd(DocumentsAddDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DocumentsAddDto, DocumentsAdd>().Map(modeleDto);
        }

        public static DocumentsAddDto LoadDto(DocumentsAdd modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DocumentsAdd, DocumentsAddDto>().Map(modele);
        }
    }
}