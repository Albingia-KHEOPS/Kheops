using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Personnes
{
    [DataContract]
    public class SouscripteursGetQueryDto : _Personne_Base, IQuery
    {

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string NomSouscripteur { get; set; }

        [DataMember]
        public int DebutPagination { get; set; }

        [DataMember]
        public int FinPagination { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtageGetQueryDto"/> class.
        /// </summary>
        public SouscripteursGetQueryDto()
        {
            this.Code = _DTO_Base._undefinedString;
            this.DebutPagination = _DTO_Base._undefinedInt;
            this.FinPagination = _DTO_Base._undefinedInt;
            this.NomSouscripteur = _DTO_Base._undefinedString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtageGetQueryDto"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public SouscripteursGetQueryDto(string code)
        {
            this.Code = code;
            this.DebutPagination = _DTO_Base._undefinedInt;
            this.FinPagination = _DTO_Base._undefinedInt;
            this.NomSouscripteur = _DTO_Base._undefinedString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtageGetQueryDto"/> class.
        /// </summary>
        /// <param name="debutPagination">The debut pagination.</param>
        /// <param name="finPagination">The fin pagination.</param>
        /// <param name="nomCabinet">The nom cabinet.</param>
        public SouscripteursGetQueryDto(string code, int debutPagination, int finPagination, string NomSouscripteur)
        {
            this.Code = code;
            this.DebutPagination = debutPagination;
            this.FinPagination = finPagination;
            this.NomSouscripteur = NomSouscripteur;
        }
    }
}