using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Personnes
{
    [DataContract]
    public class GestionnairesGetQueryDto : _Personne_Base, IQuery
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string NomGestionnaire { get; set; }

        [DataMember]
        public int DebutPagination { get; set; }

        [DataMember]
        public int FinPagination { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtageGetQueryDto"/> class.
        /// </summary>
        public GestionnairesGetQueryDto()
        {
            this.Code = _DTO_Base._undefinedString;
            this.DebutPagination = _DTO_Base._undefinedInt;
            this.FinPagination = _DTO_Base._undefinedInt;
            this.NomGestionnaire = _DTO_Base._undefinedString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtageGetQueryDto"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public GestionnairesGetQueryDto(string code)
        {
            this.Code = code;
            this.DebutPagination = _DTO_Base._undefinedInt;
            this.FinPagination = _DTO_Base._undefinedInt;
            this.NomGestionnaire = _DTO_Base._undefinedString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtageGetQueryDto"/> class.
        /// </summary>
        /// <param name="debutPagination">The debut pagination.</param>
        /// <param name="finPagination">The fin pagination.</param>
        /// <param name="nomCabinet">The nom cabinet.</param>
        public GestionnairesGetQueryDto(string code, int debutPagination, int finPagination, string NomGestionnaire)
        {
            this.Code = code;
            this.DebutPagination = debutPagination;
            this.FinPagination = finPagination;
            this.NomGestionnaire = NomGestionnaire;
        }
    }
}