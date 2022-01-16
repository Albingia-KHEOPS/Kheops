using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Offres.Branches
{
    [DataContract]
    public class CibleDto //: _Cibles_Base
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }

        [DataMember]
        [Column(Name = "GUID")]
        public Int64 GuidId { get; set; }

        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }

        [DataMember]
        [Column(Name = "CODEBRANCHE")]
        public string CodeBranche { get; set; }

        [DataMember ]
        [Column(Name="LIBBRANCHE")]
        public string LibBranche { get; set; }

        /// <summary>
        /// Gets or sets the nom.
        /// </summary>
        /// <value>
        /// The nom.
        /// </value>
        [DataMember]
        public string Nom { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="cibleDto"/> class.
        /// </summary>
        public CibleDto()
        {
            this.Code = String.Empty;
            this.Nom = String.Empty;
        }
    }
}
