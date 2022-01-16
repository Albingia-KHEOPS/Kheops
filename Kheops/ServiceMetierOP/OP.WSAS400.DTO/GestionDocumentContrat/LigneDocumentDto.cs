using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.GestionDocumentContrat
{
    public class LigneDocumentDto
    {
        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }

        [DataMember]
        [Column(Name = "DATEENVOI")]
        public int DateEnvoi { get; set; }

        [DataMember]
        [Column(Name = "LIBELLEDOCUMENT")]
        public string LibelleDocument { get; set; }

        [DataMember]
        [Column(Name = "TYPEDOCUMENTCODE")]
        public string TypeDocumentCode { get; set; }

        [DataMember]
        [Column(Name = "TYPEDOCUMENTLIB")]
        public string TypeDocumentLib { get; set; }

        [DataMember]
        [Column(Name = "NOMDOCUMENT")]
        public string NomDocument { get; set; }

        [DataMember]
        [Column(Name = "TYPEDIFFUSION")]
        public string TypeDiffusion { get; set; }

        [DataMember]
        [Column(Name = "TYPEEDITION")]
        public string TypeEdition { get; set; }

        [DataMember]
        [Column(Name = "TYPEDESTINATAIRE")]
        public string TypeDestinataire { get; set; }

        [DataMember]
        [Column(Name = "IDDESTINATAIRE")]
        public int CodeDestinataire { get; set; }

        [DataMember]
        [Column(Name = "NOMDESTINATAIRE")]
        public string NomDestinataire { get; set; }

        [DataMember]
        [Column(Name = "LOT")]
        public string Lot { get; set; }

        [DataMember]
        [Column(Name = "NUMAVENANT")]
        public int NumAvenant { get; set; }

        [DataMember]
        [Column(Name = "NBPJ")]
        public int NbPieceJointe { get; set; }

        [DataMember]
        [Column(Name = "UTILISATEUR")]
        public string Utilisateur { get; set; }

        #region infobulle
        [DataMember]
        [Column(Name = "NOMFICHIER")]
        public string NomFichier { get; set; }

        [DataMember]
        [Column(Name = "SERVICE")]
        public string Service { get; set; }

        public DateTime? DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public string Tampon { get; set; }
        public string TypeOrigine { get; set; }
        [DataMember]
        [Column(Name = "CHEMINFICHIER")]
        public string CheminFichier { get; set; }
        #endregion
    }
}
