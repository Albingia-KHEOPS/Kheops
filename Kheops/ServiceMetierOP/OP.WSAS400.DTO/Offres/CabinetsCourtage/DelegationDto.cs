using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.CabinetsCourtage
{
    /// <summary>
    /// DTO de la delegation
    /// </summary>
    [DataContract]
    public class DelegationDto
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
        /// Gets or sets the nom.
        /// </summary>
        /// <value>
        /// The nom.
        /// </value>
        [DataMember]
        public string Nom { get; set; }

        /// <summary>
        /// Gets or sets the inspecteur.
        /// </summary>
        /// <value>
        /// The inspecteur.
        /// </value>
        [DataMember]
        public InspecteurDto Inspecteur { get; set; }
        [DataMember]
        public string Secteur { get; set; }
        [DataMember]
        public string LibSecteur { get; set; }

        [DataMember]
        public string DelegationApporteur { get; set; }
        [DataMember]
        public string SecteurApporteur { get; set; }
        [DataMember]
        public string DelegationGestionnaire { get; set; }
        [DataMember]
        public string SecteurGestionnaire { get; set; }

    }
}