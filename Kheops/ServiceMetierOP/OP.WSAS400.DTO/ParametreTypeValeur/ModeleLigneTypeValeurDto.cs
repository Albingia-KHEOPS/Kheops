using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreTypeValeur
{
    public class ModeleLigneTypeValeurDto
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

        /// <summary>
        /// Gets or sets the libelle.
        /// </summary>
        /// <value>
        /// The libelle.
        /// </value>
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Description { get; set; }
    }
}
