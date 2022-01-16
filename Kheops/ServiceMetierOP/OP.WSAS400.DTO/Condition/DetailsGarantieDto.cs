using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Condition
{
    [DataContract]
    public class DetailsGarantieDto
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
        /// Gets or sets the libelle.
        /// </summary>
        /// <value>
        /// The libelle.
        /// </value>
        [DataMember]
        public string Libelle { get; set; }

        /// <summary>
        /// Gets or sets the risque.
        /// </summary>
        /// <value>
        /// The risque.
        /// </value>
        [DataMember]
        public RisqueDto Risque { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [lci indexe].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [lci indexe]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool LciIndexe { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [franchise indexe].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [franchise indexe]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool FranchiseIndexe { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [assiette indexe].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [assiette indexe]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool AssietteIndexe { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DetailsGarantieDto"/> is CATNAT.
        /// </summary>
        /// <value>
        ///   <c>true</c> if CATNAT; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool CATNAT { get; set; }

        /// <summary>
        /// Gets or sets the nature.
        /// </summary>
        /// <value>
        /// The nature.
        /// </value>
        [DataMember]
        public string Nature { get; set; }
        /// <summary>
        /// Gets or sets the natures.
        /// </summary>
        /// <value>
        /// The natures.
        /// </value>
        [DataMember]
        public List<ParametreDto> Natures { get; set; }

        public DetailsGarantieDto()
        {
            Code = String.Empty;
            Libelle = String.Empty;
            Risque = new RisqueDto();
            LciIndexe = false;
            FranchiseIndexe = false;
            AssietteIndexe = false;
            CATNAT = false;
            Nature = String.Empty;
            Natures = new List<ParametreDto>();
        }
    }
}
