using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.Assures
{
    [DataContract]
    public class AssureGetQueryDto //: _Assure_Base, IQuery
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
        /// Gets or sets the nom assure.
        /// </summary>
        /// <value>
        /// The nom assure.
        /// </value>
        [DataMember]
        public string NomAssure { get; set; }

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

        [DataMember]
        public string CodePostal { get; set; }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="AssureGetQueryDto"/> class.
        ///// </summary>
        //public AssureGetQueryDto()
        //{
        //    this.Code = _DTO_Base._undefinedString;
        //    this.DebutPagination = _DTO_Base._undefinedInt;
        //    this.FinPagination = _DTO_Base._undefinedInt;
        //    this.NomAssure = _DTO_Base._undefinedString;
        //    this.Order = _DTO_Base._undefinedString;
        //    this.By = _DTO_Base._undefinedInt;
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="AssureGetQueryDto"/> class.
        ///// </summary>
        ///// <param name="code">The code.</param>
        //public AssureGetQueryDto(string code)
        //{
        //    this.Code = code;
        //    this.DebutPagination = _DTO_Base._undefinedInt;
        //    this.FinPagination = _DTO_Base._undefinedInt;
        //    this.NomAssure = _DTO_Base._undefinedString;
        //    this.Order = _DTO_Base._undefinedString;
        //    this.By = _DTO_Base._undefinedInt;
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="AssureGetQueryDto"/> class.
        ///// </summary>
        ///// <param name="debutPagination">The debut pagination.</param>
        ///// <param name="finPagination">The fin pagination.</param>
        ///// <param name="nomAssure">The nom assure.</param>
        //public AssureGetQueryDto(int debutPagination, int finPagination, string nomAssure)
        //{
        //    this.Code = _DTO_Base._undefinedString;
        //    this.DebutPagination = debutPagination;
        //    this.FinPagination = finPagination;
        //    this.NomAssure = nomAssure;
        //    this.Order = _DTO_Base._undefinedString;
        //    this.By = _DTO_Base._undefinedInt;
        //}
    }
}
