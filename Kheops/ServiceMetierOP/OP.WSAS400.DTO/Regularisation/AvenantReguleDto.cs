using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Avenant;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class AvenantReguleDto
    {
        [DataMember]
        [Column(Name = "MODEAVT")]
        public string ModeAvt { get; set; }
        [DataMember]
        [Column(Name = "TYPEAVT")]
        public string TypeAvt { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEAVT")]
        public string LibelleAvt { get; set; }
        [DataMember]
        [Column(Name = "NUMINTERNEAVT")]
        public Int64 NumInterneAvt { get; set; }
        [DataMember]
        [Column(Name = "NUMAVT")]
        public Int64 NumAvt { get; set; }
        [DataMember]
        [Column(Name = "MOTIFAVT")]
        public string MotifAvt { get; set; }
        [DataMember]
        [Column(Name = "LIBMOTIFAVT")]
        public string LibMotifAvt { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTIONAVT")]
        public string DescriptionAvt { get; set; }
        [DataMember]
        [Column(Name = "OBSERVATIONSAVT")]
        public string ObservationsAvt { get; set; }

        [DataMember]
        public string ErrorAvt { get; set; }

        [DataMember]
        public List<AvenantAlerteDto> Alertes { get; set; }


    }
}
