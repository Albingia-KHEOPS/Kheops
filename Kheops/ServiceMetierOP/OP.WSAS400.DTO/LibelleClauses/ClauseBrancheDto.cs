using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.Modeles;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.LibelleClauses
{
    [DataContract]
    public class ClauseBrancheDto

    {
        [DataMember]
        [Column(Name = "TCOD")]
        public String Code { get; set; }

        [DataMember]
        [Column(Name = "TPLIB")]
        public String Libelle { get; set; }

        public ClauseBrancheDto()
        {
            this.Code = string.Empty;
            this.Libelle = string.Empty;
           
        }
    }
}
