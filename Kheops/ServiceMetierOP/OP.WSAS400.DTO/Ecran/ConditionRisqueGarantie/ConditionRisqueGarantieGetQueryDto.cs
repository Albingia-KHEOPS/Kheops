using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie
{
    [DataContract]
    public class ConditionRisqueGarantieGetQueryDto : _ConditionRisqueGarantie_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the numero offre.
        /// </summary>
        /// <value>
        /// The numero offre.
        /// </value>
        [DataMember]
        public string NumeroOffre { get; set; }
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string CodeFormule { get; set; }
        [DataMember]
        public string CodeOption { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionRisqueGarantieGetQueryDto"/> class.
        /// </summary>
        public ConditionRisqueGarantieGetQueryDto()
        {
            NumeroOffre = String.Empty;
            version = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionRisqueGarantieGetQueryDto"/> class.
        /// </summary>
        /// <param name="arg_NumOffre">The arg_ num offre.</param>
        /// <param name="arg_Version">The arg_ version.</param>
        public ConditionRisqueGarantieGetQueryDto(string arg_NumOffre, string arg_Version)
        {
            NumeroOffre = arg_NumOffre;
            version = arg_Version;
        }
    }
}
