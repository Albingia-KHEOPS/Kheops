using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.Aperiteur
{
    [DataContract]
    public class AperiteurGetQueryDto : _Aperiteur_Base, IQuery
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
        /// Gets or sets the debut pagination.
        /// </summary>
        /// <value>
        /// The debut pagination.
        /// </value>
        [DataMember]
        public int DebutPagination { get; set; }

        /// <summary>
        /// Gets or sets the fin pagination.
        /// </summary>
        /// <value>
        /// The fin pagination.
        /// </value>
        [DataMember]
        public int FinPagination { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AperiteurGetQueryDto"/> class.
        /// </summary>
        public AperiteurGetQueryDto()
        {
            this.Code = _DTO_Base._undefinedString;
            this.Nom = _DTO_Base._undefinedString;
            this.DebutPagination = _DTO_Base._undefinedInt;
            this.FinPagination = _DTO_Base._undefinedInt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AperiteurGetQueryDto"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public AperiteurGetQueryDto(string code)
        {
            this.Code = code;
            this.Nom = _DTO_Base._undefinedString;
            this.DebutPagination = _DTO_Base._undefinedInt;
            this.FinPagination = _DTO_Base._undefinedInt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AperiteurGetQueryDto"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="nom">The nom.</param>
        /// <param name="debutPagination">The debut pagination.</param>
        /// <param name="finPagination">The fin pagination.</param>
        public AperiteurGetQueryDto(string code, string nom, int debutPagination, int finPagination)
        {
            this.Code = code;
            this.Nom = nom;
            this.DebutPagination = debutPagination;
            this.FinPagination = finPagination;
        }

        
    }
}
