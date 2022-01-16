using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ALBINGIA.Framework.Common.Constants;

namespace OP.WSAS400.DTO.Ecran.DoubleSaisie
{
    /// <summary>
    /// query dto pour obtenir les informations de la double saisie.
    /// </summary>
    [DataContract]
    public class DoubleSaisieGetQueryDto : _DoubleSaisie_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the code offre.
        /// </summary>
        /// <value>
        /// The code offre.
        /// </value>
        [DataMember]
        public string CodeOffre { get; set; }

        /// <summary>
        /// Gets or sets the version offre.
        /// </summary>
        /// <value>
        /// The version offre.
        /// </value>
        [DataMember]
        public int VersionOffre { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleSaisieGetQueryDto"/> class.
        /// </summary>
        public DoubleSaisieGetQueryDto()
        {
            CodeOffre = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleSaisieGetQueryDto"/> class.
        /// </summary>
        /// <param name="argCodeOffre">The arg code offre.</param>
        public DoubleSaisieGetQueryDto(string argCodeOffre)
        {
            CodeOffre = argCodeOffre;
        }
    }
}