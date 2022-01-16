using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Categorie
{
    [DataContract]
    public class CategorieDto 
    {
        /// <summary>
        /// Gets or sets the guidId.
        /// </summary>
        /// <value>
        /// The guidId.
        /// </value>
        [DataMember]
        [Column(Name = "GUID")]
        public Int64 GuidId { get; set; }
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the date creation.
        /// </summary>
        /// <value>
        /// The date creation.
        /// </value>
        [DataMember]
        public DateTime? DateCreation { get; set; }
        [DataMember]
        [Column(Name = "BRANCHE")]
        public string CodeBranche { get; set; }
        [DataMember]
        [Column(Name = "CARACTERE")]
        public string Caractere { get; set; }

        public CategorieDto()
        {          
            this.Code = string.Empty;
            this.Description = string.Empty;
            this.DateCreation = DateTime.MinValue;
            this.CodeBranche = string.Empty;
            this.Caractere = string.Empty;
        }
    }
}
