using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class LigneRegularisationDto
    {
        [DataMember]
        [Column(Name = "NUMREG")]
        public Int64 NumRegule { get; set; }
        [DataMember]
        [Column(Name = "CODEAVN")]
        public Int64 CodeAvn { get; set; }
        [DataMember]
        [Column(Name = "CODETRAITEMENT")]
        public string CodeTraitement { get; set; }
        [DataMember]
        [Column(Name = "LIBTRAITEMENT")]
        public string LibTraitement { get; set; }
        [DataMember]
        [Column(Name = "DATEDEB")]
        public Int32 DateDeb { get; set; }
        [DataMember]
        [Column(Name = "DATEFIN")]
        public Int32 DateFin { get; set; }
        [DataMember]
        [Column(Name = "CODEETAT")]
        public string CodeEtat { get; set; }
        [DataMember]
        [Column(Name = "CODESITUATION")]
        public string CodeSituation { get; set; }
        [DataMember]
        [Column(Name = "LIBSITUATION")]
        public string LibSituation { get; set; }
        [DataMember]
        [Column(Name = "DATESIT")]
        public Int64 DateSit { get; set; }
        [DataMember]
        [Column(Name = "HEURESIT")]
        public Int32 HeureSit { get; set; }
        [DataMember]
        [Column(Name = "USERSIT")]
        public string UserSit { get; set; }
        [DataMember]
        [Column(Name = "NUMQUITT")]
        public Int64 NumQuittance { get; set; }
        [DataMember]
        [Column(Name = "CODEETATQUITT")]
        public string CodeEtatQuitt { get; set; }
        [DataMember]
        [Column(Name = "LIBETATQUITT")]
        public string LibEtatQuitt { get; set; }
        [DataMember]
        [Column(Name = "AVIS")]
        public Int64 Avis { get; set; }

        [DataMember]
        [Column(Name = "REGULMODE")]
        public string RegulMode { get; set; }

        [DataMember]
        [Column(Name = "REGULTYPE")]
        public string RegulType { get; set; }

        [DataMember]
        [Column(Name = "REGULNIV")]
        public string RegulNiv { get; set; }

        [DataMember]
        [Column(Name = "REGULAVN")]
        public string RegulAvn { get; set; }




    }
}
