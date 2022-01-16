using System;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Branches;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Personnes
{
    /// <summary>
    /// Dto du souscripteur
    /// </summary>
    [DataContract]
    public class SouscripteurDto //: _Personne_Base
    {
        //[Column(Name = "ID_NEXT")]
        //public Int64 IdNext { get; set; }

        //[Column(Name="CODE")]
        [DataMember]
        [Column(Name = "CODESOU")]
        public string Code { get; set; }

        //[Column(Name="LOGIN")]
        [DataMember]
        [Column(Name = "UTPFX")]
        public string Login { get; set; }

        //[Column(Name="NOM")]
        [DataMember]
        [Column(Name = "UTNOM")]
        public string Nom { get; set; }

        //[Column(Name="PRENOM")]
        [Column(Name = "UTPNM")]
        [DataMember]
        public string Prenom { get; set; }

        [Column(Name = "UTBRA")]
        public string SBranche { get; set; }
        //[Column(Name="BRANCHE")]
        [DataMember]
        public BrancheDto Branche { get; set; }

        [Column(Name = "BUDBU")]
        [DataMember]
        public string NomDelegation { get; set; }

        [Column(Name = "UTSOU")]
        [DataMember]
        public string IsSouscripteur { get; set; }


        //[Column(Name="DELEGATION")]
        [DataMember]
        public DelegationDto Delegation { get; set; }
    }
}