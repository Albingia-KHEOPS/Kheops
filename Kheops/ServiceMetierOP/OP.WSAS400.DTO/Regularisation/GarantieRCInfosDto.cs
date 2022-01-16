using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class GarantieRCInfosDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public int RsqNum { get; set; }

        [DataMember]
        public string DateDebut { get; set; }

        [DataMember]
        public string DateFin { get; set; }

        [DataMember]
        public string RegimeTaxe { get; set; }

        [DataMember]
        public string Formule { get; set; }

        [DataMember]
        public string CodeTaxes { get; set; }

        [DataMember]
        public string TypeRegul { get; set; }

        [DataMember]
        public double TauxAppel { get; set; }

        [DataMember]
        public double TauxAttentat { get; set; }
    }
}
