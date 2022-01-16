using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Volet;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class FormuleGarantieDto : _FormuleGarantie_Base
    {
        [DataMember]
        [Column(Name="ISAVTMODIF")]
        public bool IsAvenantModificationLocale { get; set; }
        [Column(Name = "DATEEFFETAVTMODIF")]
        public Int64 DateEffetAvtModif { get; set; }
        [DataMember]
        public DateTime? DateEffetAvenantModificationLocale { get; set; }

        [DataMember]
        public bool FormuleChecked { get; set; }
        [DataMember]
        public List<DtoVolet> Volets { get; set; }
        [DataMember]
        public string CodeOption { get; set; }

        public FormuleGarantieDto()
        {
            this.FormuleChecked = false;
            this.Volets = new List<DtoVolet>();
        }
    }
}
