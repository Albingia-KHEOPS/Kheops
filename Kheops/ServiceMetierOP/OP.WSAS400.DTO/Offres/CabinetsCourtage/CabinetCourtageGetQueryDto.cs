using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.CabinetsCourtage
{
    /// <summary>
    /// Dto de la query de la méthode cabinetCourtageGet
    /// </summary>
    [DataContract]
    public class CabinetCourtageGetQueryDto : _CabinetsCourtage_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public int? Code { get; set; }

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
        /// Gets or sets the nom cabinet.
        /// </summary>
        /// <value>
        /// The nom cabinet.
        /// </value>
        [DataMember]
        public string NomCabinet { get; set; }

        /// <summary>
        /// Order.
        /// </summary>
        /// <value>
        /// 
        /// </value>
        [DataMember]
        public string Order { get; set; }

        /// <summary>
        /// By.
        /// </summary>
        /// <value>
        /// 
        /// </value>
        [DataMember]
        public int By { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [return souscripteurs].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [return souscripteurs]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ReturnSouscripteurs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtageGetQueryDto"/> class.
        /// </summary>
        public CabinetCourtageGetQueryDto()
        {
            this.Code = null;
            this.DebutPagination = _DTO_Base._undefinedInt;
            this.FinPagination = _DTO_Base._undefinedInt;
            this.NomCabinet = _DTO_Base._undefinedString;
            this.ReturnSouscripteurs = false;
            this.Order = _DTO_Base._undefinedString;
            this.By = _DTO_Base._undefinedInt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtageGetQueryDto"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public CabinetCourtageGetQueryDto(int code, bool returnSouscripteurs)
        {
            this.Code = code;
            this.DebutPagination = _DTO_Base._undefinedInt;
            this.FinPagination = _DTO_Base._undefinedInt;
            this.NomCabinet = _DTO_Base._undefinedString;
            this.ReturnSouscripteurs = returnSouscripteurs;
            this.Order = _DTO_Base._undefinedString;
            this.By = _DTO_Base._undefinedInt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtageGetQueryDto"/> class.
        /// </summary>
        /// <param name="debutPagination">The debut pagination.</param>
        /// <param name="finPagination">The fin pagination.</param>
        /// <param name="nomCabinet">The nom cabinet.</param>
        public CabinetCourtageGetQueryDto(int debutPagination, int finPagination, string nomCabinet, bool returnSouscripteurs)
        {
            this.Code = null;
            this.DebutPagination = debutPagination;
            this.FinPagination = finPagination;
            this.NomCabinet = nomCabinet;
            this.ReturnSouscripteurs = returnSouscripteurs;
            this.Order = _DTO_Base._undefinedString;
            this.By = _DTO_Base._undefinedInt;
        }
    }
}
