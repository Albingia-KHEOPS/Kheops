using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleGestionDocument
{
    [Serializable]
    public class GestionDocumentCourrier
    {
        public List<AlbSelectListItem> TypesPartenaire { get; set; }
        public string CodeCourrier { get; set; }
        public string TypePartenaire { get; set; }
        public string CodePartenaire { get; set; }
        public string NomPartenaire { get; set; }
        public string Interlocuteur { get; set; }
        public string TypeCourrierPart { get; set; }
        public string NbExp { get; set; }
        public string DestinatairePart { get; set; }
    }
}