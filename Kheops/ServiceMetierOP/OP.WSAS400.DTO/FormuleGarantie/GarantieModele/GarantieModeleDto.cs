using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.GarantieModele
{
    [DataContract]
    public class GarantieModeleDto 
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
        /// Gets or sets the date debut.
        /// </summary>
        /// <value>
        /// The date debut.
        /// </value>
        [DataMember]
        public DateTime? DateAppli { get; set; }
        /// <summary>
        /// Gets or sets the date creation.
        /// </summary>
        /// <value>
        /// The date creation.
        /// </value>
        [DataMember]
        public DateTime? DateCreation { get; set; }
        /// <summary>
        /// Gets or sets the typologie.
        /// </summary>
        /// <value>
        /// The typologie.
        /// </value>
        [DataMember]
        public string Typologie { get; set; }
        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        [DataMember]
        public string GuidId { get; set; }
        [DataMember]
        public bool ReadOnly { get; set; }
        [DataMember]
        public string Update { get; set; }
        [DataMember]
        public List<ModeleGarantieDto> LstModeleGarantie { get; set; }

        public GarantieModeleDto()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
            this.DateAppli = null;
            this.DateCreation = null;
            this.Typologie = string.Empty;
            this.GuidId = string.Empty;
            this.ReadOnly = false;
            Update = "0";
            LstModeleGarantie = new List<ModeleGarantieDto>();
        }
    }

    [DataContract]
    public class ModeleGarantieDto
    {
        [DataMember]
        public string Branche { get; set; }
        [DataMember]
        public string Cible { get; set; }
        [DataMember]
        public string Volet { get; set; }
        [DataMember]
        public string Bloc { get; set; }
        [DataMember]
        public string Typologie { get; set; }
    }
}
