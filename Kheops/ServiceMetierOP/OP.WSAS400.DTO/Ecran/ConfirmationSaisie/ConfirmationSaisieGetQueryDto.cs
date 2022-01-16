using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ALBINGIA.Framework.Common.Constants;

namespace OP.WSAS400.DTO.Ecran.ConfirmationSaisie
{
    [DataContract]
    public class ConfirmationSaisieGetQueryDto : _ConfirmationSaisie_Base, IQuery
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
        /// Gets or sets the offre version.
        /// </summary>
        /// <value>
        /// The offre version.
        /// </value>
        [DataMember]
        public int VersionOffre { get; set; }

        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationSaisieGetQueryDto"/> class.
        /// </summary>
        public ConfirmationSaisieGetQueryDto()
        {
            this.CodeOffre = _DTO_Base._undefinedString;
            this.VersionOffre = _DTO_Base._undefinedInt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationSaisieGetQueryDto"/> class.
        /// </summary>
        /// <param name="offreCode">The offre code.</param>
        /// <param name="offreVersion">The offre version.</param>
        public ConfirmationSaisieGetQueryDto(string offreCode, int offreVersion, string offreType)
        {
            this.CodeOffre = offreCode;
            this.VersionOffre = offreVersion;
            this.Type = offreType;
        }
    }
}