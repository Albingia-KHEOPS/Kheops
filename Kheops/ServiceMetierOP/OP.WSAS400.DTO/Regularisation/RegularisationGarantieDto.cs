using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegularisationGarantieDto : IRCFRGroupItem
    {
        [Column(Name = "CODEAFFAIRE")]
        public string CodeAffaire { get; set; }

        [Column(Name = "VERSION")]
        public int Version { get; set; }

        [Column(Name = "TYPEAFFAIRE")]
        public string Type { get; set; }

        [Column(Name = "IDRISQUE")]
        public Int32 IdRisque { get; set; }
        [DataMember]
        [Column(Name = "CODEFOR")]
        public Int32 IdFormule { get; set; }
        [DataMember]
        [Column(Name ="CODEOPT")]
        public Int32 IdOption { get; set; }
        [DataMember]
        [Column(Name = "LETTREFOR")]
        public string CodeFormule { get; set; }
        [DataMember]
        [Column(Name = "LIBFOR")]
        public string LibFormule { get; set; }
        [DataMember]
        [Column(Name = "IDGARAN")]
        public Int64 IdGarantie { get; set; }
        [DataMember]
        [Column(Name ="SEQGARAN")]
        public Int64 SeqGaran { get; set; }
        [DataMember]
        [Column(Name = "CODEGARAN")]
        public string CodeGarantie { get; set; }
        [DataMember]
        [Column(Name = "LIBGARAN")]
        public string LibGarantie { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBGAR")]
        public Int64 DateDebGar { get; set; }
        [DataMember]
        [Column(Name = "DATEFINGAR")]
        public Int64 DateFinGar { get; set; }
        [DataMember]
        [Column(Name = "CODETYPEREGULE")]
        public string CodeTypeRegule { get; set; }
        [DataMember]
        [Column(Name = "LIBTYPEREGULE")]
        public string LibTypeRegule { get; set; }

        [DataMember]
        [Column(Name = "ISGARUSED")]
        public Int32 IsGarUsed { get; set; }

        [DataMember]
        public long IdRCFR { get; set; }
    }
}
