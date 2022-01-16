using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace OP.WSAS400.DTO.Offres
{

    /// <summary>
    /// Represente l'objet StatusOffreUpdateQueryDto.
    /// </summary>
    [DataContract]
    public class StatusOffreUpdateQueryDto : _Offre_Base
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public string CodeOffre { get; set; }
        /// <summary>
        /// Gets or sets the code situation.
        /// </summary>
        /// <value>
        /// The code situation.
        /// </value>
        [DataMember]
        public enSituationOffres CodeSituation { get; set; }
        /// <summary>
        /// Gets or sets the motif refus.
        /// </summary>
        /// <value>
        /// The motif refus.
        /// </value>
        [DataMember]
        public OP.WSAS400.DTO.Offres.Parametres.ParametreDto MotifRefus { get; set; }

        /// <summary>
        /// Constructeur par default.
        /// </summary>
        public StatusOffreUpdateQueryDto()
        {
            this.CodeOffre = _DTO_Base._undefinedString;
            this.CodeSituation = enSituationOffres._Indetermine;
            this.MotifRefus = null;
        }
    }
}