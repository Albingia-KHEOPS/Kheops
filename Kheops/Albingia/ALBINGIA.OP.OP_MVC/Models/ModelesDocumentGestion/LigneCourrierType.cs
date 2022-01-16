using OP.WSAS400.DTO.DocumentGestion;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class LigneCourrierType
    {
        public string CheminFichier { get; set; }
        public bool IsSelected { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public Int64 GuidId { get; set; }

        public static explicit operator LigneCourrierType(CourrierTypeDto courrierDto)
        {
            var toReturn = new LigneCourrierType()
            {
                Code = courrierDto.Code,
                Libelle = courrierDto.Libelle,
                GuidId = courrierDto.GuidId
            };
            return toReturn;
        }
    }
}