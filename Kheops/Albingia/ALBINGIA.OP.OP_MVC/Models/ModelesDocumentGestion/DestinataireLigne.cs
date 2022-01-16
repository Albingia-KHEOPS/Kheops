using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.DocumentGestion;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class DestinataireLigne
    {
        public Int64 GuidId { get; set; }  

        public bool IsPrincipal { get; set; }
        public List<AlbSelectListItem> ListeTypesDestinataire { get; set; }
        public string CodeTypeDestinataire { get; set; }
        public string LibTypeDestinataire { get; set; }

        public Int64 IdDestinataire { get; set; }
        public string CodeDestinataire { get; set; }
        public string NomDestinataire { get; set; }
        public string TypeIntervenant { get; set; }
        public string NomInterlocuteur { get; set; }
        public string EmailInterlocuteur { get; set; }
        public Int64 IdInterlocuteur { get; set; }

        public List<AlbSelectListItem> ListeTypesEnvoi { get; set; }
        public string TypeEnvoi { get; set; }
        public string NombreExemplaire { get; set; }

        public List<AlbSelectListItem> ListeTampons { get; set; }
        public string Tampon { get; set; }

        public List<AlbSelectListItem> ListeLettreAccompagnement { get; set; }
        public string LettreAccompagnement { get; set; }

        public int LotEmail { get; set; }

        public bool IsGenere { get; set; }

        public static explicit operator DestinataireLigne(DestinataireDto destinataire)
        {
            var toReturn = new DestinataireLigne()
            {
                GuidId = destinataire.GuidId,
                CodeDestinataire = destinataire.Code.ToString(),
                NomDestinataire = destinataire.Libelle,
                CodeTypeDestinataire = destinataire.CodeTypeDestinataire,
                LibTypeDestinataire = destinataire.LibTypeDestinataire,
                IdDestinataire = destinataire.Code,             
                IdInterlocuteur = destinataire.CodeInterlocuteur,
                NomInterlocuteur = destinataire.NomInterlocuteur,
                TypeIntervenant = destinataire.TypeIntervenant,
                EmailInterlocuteur = destinataire.EmailInterlocuteur,
                TypeEnvoi = destinataire.TypeEnvoi,
                NombreExemplaire = destinataire.NombreEx.ToString(),
                Tampon = destinataire.Tampon,
                LettreAccompagnement = destinataire.LettreAccompagnement.ToString(),
                LotEmail = destinataire.LotEmail,
                IsGenere = destinataire.IsGenere == "A",
                IsPrincipal = destinataire.IsPrincipal == "O"
            };
            return toReturn;
        }
    }
}