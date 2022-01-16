using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Stat
{
    [DataContract]
    public class StatClausesLibresDto
    {
        [DataMember]
        [Column(Name = "DELEGATIONUSER")]
        public string DelegationUser { get; set; }

        [DataMember]
        [Column(Name = "CREATEUSER")]
        public string CreateUser { get; set; }

        [DataMember]
        [Column(Name = "DATECREATION")]
        public string DateCreation { get; set; }

        [DataMember]
        [Column(Name = "DATESAISIE")]
        public string DateSaisie { get; set; }

        [DataMember]
        [Column(Name = "NUMPOLICE")]
        public string NumPolice { get; set; }

        [DataMember]
        [Column(Name = "SOUSCRIPTEUR")]
        public string Souscripteur { get; set; }

        [DataMember]
        [Column(Name = "VERSION")]
        public int Version { get; set; }

        [DataMember]
        [Column(Name = "CODECOURTIER")]
        public int CodeCourtier { get; set; }

        [DataMember]
        [Column(Name = "NOMCOURTIER")]
        public string NomCourtier { get; set; }

        [DataMember]
        [Column(Name = "DELEGATIONCOURTIER")]
        public string DelegationCourtier { get; set; }

        [DataMember]
        [Column(Name = "CODEPRENEURASS")]
        public int CodePreneurass { get; set; }

        [DataMember]
        [Column(Name = "NOMPRENEURASS")]
        public string NomPreneurass { get; set; }




    }
}
