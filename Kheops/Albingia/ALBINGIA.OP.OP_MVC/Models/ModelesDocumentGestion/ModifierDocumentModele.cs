using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class ModifierDocumentModele
    {
        public List<AlbSelectListItem> ListeDocuments { get; set; }
        public string Document { get; set; }

        public string CourrierType { get; set; }

        public List<DestinataireLigne> ListeDestinataire { get; set; }
        public List<DestinataireLigne> NewDestinataire { get; set; }
        public string ExemplairesSupplementaires { get; set; }
        public Int64 IdDocument { get; set; }
        public string CodeOffreContrat { get; set; }      
        public string VersionOffreContrat { get; set; }
        public Int64 LotId { get; set; }
        public Int64 CourrierId { get; set; }
        public string CourrierCode { get; set; }
        public string CourrierLib { get; set; }
        public string DocLibre { get; set; }
        public string DocGener { get; set; }
        public bool IsDocLibre
        {
            get
            {
                return DocLibre == "L" &&  DocGener == "G";
            }
        }

        //public FileDescriptions ModelFileDescriptions { get; set; }

    }
}