using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Offres.Parametres
{
    [DataContract]
    public class ParametreDto //: _Parametre_Base
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        [Column(Name="ID")]
        public int? Id { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the libelle.
        /// </summary>
        /// <value>
        /// The libelle.
        /// </value>
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }

        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>
        [DataMember]
        [Column(Name = "DESCRIPTIF")]
        public string Descriptif { get; set; }

        [DataMember]
        public bool Utilise { get; set; }
        
        [DataMember]
        [Column(Name = "LONGID")]
        public Int64 LongId { get; set; }

        [DataMember]
        [Column(Name = "CODETPCN1")]
        public Double CodeTpcn1 { get; set; }

        [DataMember]
        [Column(Name = "CODETPCN2")]
        public double CodeTpcn2 { get; set; }

        [DataMember]
        [Column(Name = "CODETPCA1")]
        public string CodeTpca1 { get; set; }

        [DataMember]
        [Column(Name = "CODETPCA2")]
        public string CodeTpca2 { get; set; }
    }
}