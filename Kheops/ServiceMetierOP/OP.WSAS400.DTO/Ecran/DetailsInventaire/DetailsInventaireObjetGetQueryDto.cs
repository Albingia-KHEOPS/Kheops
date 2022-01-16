using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.DetailsInventaire
{
    [DataContract]
    public class DetailsInventaireObjetGetQueryDto:_DetailsInventaire_Base , IQuery 
    {
        /// <summary>
        /// Gets or sets the code inv.
        /// </summary>
        /// <value>
        /// The code inv.
        /// </value>
        [DataMember]
        public string CodeInv { get; set; }

        public DetailsInventaireObjetGetQueryDto()
        {
            this.CodeInv = string.Empty;
        }
    }
}
