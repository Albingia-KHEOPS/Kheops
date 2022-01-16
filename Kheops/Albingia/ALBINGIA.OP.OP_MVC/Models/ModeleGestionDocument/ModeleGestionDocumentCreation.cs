using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleGestionDocument
{
    [Serializable]
    public class ModeleGestionDocumentCreation
    {
        public string CodeDocument { get; set; }
        [Display(Name = "Type document *")]
        public string TypeDocument { get; set; }
        public List<AlbSelectListItem> TypesDocument { get; set; }
        [Display(Name = "Courrier type")]
        public string TypeCourrier { get; set; }
        public List<AlbSelectListItem> TypesCourrier { get; set; }
        [Display(Name = "Pièce jointe")]
        public string PieceJointe { get; set; }

        public List<GestionDocumentCourrier> Courriers { get; set; }
        public List<GestionDocumentCourrier> Emails { get; set; }

        //public List<AlbSelectListItem> TypesPartenaire { get; set; }
        public List<AlbSelectListItem> TypesCourrierPart { get; set; }
        public List<AlbSelectListItem> DestinatairesPart { get; set; }

        //public static explicit operator ModeleGestionDocumentCreation(GestionDocumentCreationDto gestionDocumentDto)
        //{
        //    return ObjectMapperManager.DefaultInstance.GetMapper<GestionDocumentCreationDto, ModeleGestionDocumentCreation>().Map(gestionDocumentDto);
        //}

        //public static GestionDocumentCreationDto LoadDto(ModeleGestionDocumentCreation modele)
        //{
        //    return ObjectMapperManager.DefaultInstance.GetMapper<ModeleGestionDocumentCreation, GestionDocumentCreationDto>().Map(modele);
        //}
    }
}