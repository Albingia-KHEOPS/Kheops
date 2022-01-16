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
    public class LibelleClauseDto 

    {
        [DataMember]
        [Column(Name = "KCLSBRA")]
        public String Branche { get; set; }

        [DataMember]
        [Column(Name = "KCLSCIB")]
        public String Cible { get; set; }

        [DataMember]
        [Column(Name = "KCLSNM1")]
        public String Nomenclature1 { get; set; }

        [DataMember]
        [Column(Name = "KCLSNM2")]
        public String Nomenclature2 { get; set; }

        [DataMember]
        [Column(Name = "KCLSNM3")]
        public int? Nomenclature3 { get; set; }

        [DataMember]
        [Column(Name = "KCLSLIB")]
        public String Libellespecifique { get; set; }



        public LibelleClauseDto() {
            this.Branche = string.Empty;
            this.Cible = string.Empty;
            this.Nomenclature1 = string.Empty;
            this.Nomenclature2 = string.Empty;
            this.Nomenclature3 = 0;
            this.Libellespecifique = string.Empty;


        }
    }
}
