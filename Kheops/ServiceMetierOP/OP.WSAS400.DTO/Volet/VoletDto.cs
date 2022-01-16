using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Volet
{
    [DataContract]
    public class VoletDto : _DTO_Base
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the date creation.
        /// </summary>
        /// <value>
        /// The date creation.
        /// </value>
        [DataMember]
        public DateTime? DateCreation { get; set; }

        public VoletDto()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
            this.DateCreation = null;
        }
    }
}
