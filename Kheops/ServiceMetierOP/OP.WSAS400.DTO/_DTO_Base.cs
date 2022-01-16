using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO
{
    [DataContract]
    public class _DTO_Base
    {
        public const string _undefinedString = "";
        public const int _undefinedInt = default(int);
    }

    //[DataContract]
    //public enum enIOAS400Results
    //{
    //    [EnumMember]
    //    success = 0,
    //    [EnumMember]
    //    failure = 1
    //}

    //[DataContract]
    //public enum enSituationOffres
    //{
    //    [EnumMember]
    //    _Indetermine,
    //    [EnumMember]
    //    Encours,
    //    [EnumMember]
    //    SansSuite,
    //    [EnumMember]
    //    Realisee,
    //    [EnumMember]
    //    Annule
    //}

    //[DataContract]
    //public enum enEtatOffres
    //{
    //    [EnumMember]
    //    _Indetermine,
    //    [EnumMember]
    //    OuvertEtValidable,
    //    [EnumMember]
    //    Valide,
    //    [EnumMember]
    //    Realise,
    //    [EnumMember]
    //    OuvertNonValidable
    //}

    //[DataContract]
    //public enum TypeDateRecherche
    //{
    //    [EnumMember]
    //    Saisie,
    //    [EnumMember]
    //    Effet,
    //    [EnumMember]
    //    MAJ,
    //    [EnumMember]
    //    Creation
    //}
}