using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Common
{
    [DataContract]
    public class VisuListSuspensionDto
    {
        [DataMember]
        [Column(Name = "LSUSPVID")]
        public Int64 Id { get; set; }

        [DataMember]
        [Column(Name = "LSUSPVCODE")]
        public string CodeOffre { get; set; }

        [DataMember]
        [Column(Name = "LSUSPVVERSION")]
        public Int16 Version { get; set; }

        [DataMember]
        [Column(Name = "LSUSPVTYPE")]
        public string Type { get; set; }



        [Column(Name = "LSUSPVDATEMISE")]
        public decimal? DateMiseInt { get; set; }
        [Column(Name = "LSUSPVDATEDEBUT")]
        public decimal? DateDebutInt { get; set; }
        [Column(Name = "LSUSPVDATEFIN")]
        public decimal? DateFinInt { get; set; }
        [Column(Name = "LSUSPVDATERESIL")]
        public decimal? DateResilInt { get; set; }
        [Column(Name = "LSUSPVDATEREMISE")]
        public int? DateRemiseInt { get; set; }


        [DataMember]
        public DateTime? DateMise { get; set; }
        [DataMember]
        public DateTime? DateDebut { get; set; }
        [DataMember]
        public DateTime? DateFin { get; set; }
        [DataMember]
        public DateTime? DateResil { get; set; }
        [DataMember]
        public DateTime? DateRemise { get; set; }
    }
}
