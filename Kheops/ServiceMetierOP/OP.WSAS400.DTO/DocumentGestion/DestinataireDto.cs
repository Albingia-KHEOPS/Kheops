using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.DocumentGestion
{
    [DataContract]
    public class DestinataireDto
    {
        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }

        [DataMember]
        [Column(Name = "CODEDESTINATAIRE")]
        public Int64 Code { get; set; }

              [DataMember]
        [Column(Name = "LIBDESTINATAIRE")]
        public string Libelle { get; set; }

        [DataMember]
        [Column(Name = "TYPEDESTINATAIRE")]
        public string TypeDestinataire { get; set; }

        [DataMember]
        [Column(Name = "TYPEINTERVENANT")]
        public string TypeIntervenant { get; set; }

        [DataMember]
        [Column(Name = "CODETYPEDESTINATAIRE")]
        public string CodeTypeDestinataire { get; set; }

        [DataMember]
        [Column(Name = "LIBTYPEDESTINATAIRE")]
        public string LibTypeDestinataire { get; set; }

        [DataMember]
        [Column(Name = "ROLE")]
        public string Role { get; set; }

        [DataMember]
        [Column(Name = "ISSELECTED")]
        public string IsSelected { get; set; }

        [DataMember]
        [Column(Name = "ISPRINCIPAL")]
        public string IsPrincipal { get; set; }

                   

        [DataMember]
        [Column(Name = "CODEINTERLOCUTEUR")]
        public Int64 CodeInterlocuteur { get; set; }

        [DataMember]
        [Column(Name = "NOMINTERLOCUTEUR")]
        public string NomInterlocuteur { get; set; }

        [DataMember]
        [Column(Name = "EMAILINTERLOCUTEUR")]
        public string EmailInterlocuteur { get; set; }

        [DataMember]
        [Column(Name = "TYPEENVOI")]
        public string TypeEnvoi { get; set; }

        [DataMember]
        [Column(Name = "NOMBREEX")]
        public int NombreEx { get; set; }

        [DataMember]
        [Column(Name = "TAMPON")]
        public string Tampon { get; set; }

        [DataMember]
        [Column(Name = "LETTREACCOMP")]
        public Int64 LettreAccompagnement { get; set; }

        [DataMember]
        [Column(Name = "LOTEMAIL")]
        public int LotEmail { get; set; }

        [DataMember]
        [Column(Name = "ISGENERE")]
        public string IsGenere { get; set; }
    }
}
