using OP.WSAS400.DTO.DocumentGestion;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class LigneSelectionDestinataire
    {
        public string TypeDestinataire { get; set; }

        public bool IsSelected { get; set; }
        public Int64 GuidId { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string Role { get; set; }

        public static explicit operator LigneSelectionDestinataire(DestinataireDto destinataire)
        {
            var toReturn = new LigneSelectionDestinataire()
            {
                Code = destinataire.Code.ToString(),
                Libelle = destinataire.Libelle,
                GuidId = destinataire.GuidId,
                IsSelected = destinataire.IsSelected == "O",
                Role = destinataire.Role
            };
            return toReturn;
        }
    }
}