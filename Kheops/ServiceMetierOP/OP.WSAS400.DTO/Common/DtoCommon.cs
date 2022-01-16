using System;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;
using System.Collections.Generic;

namespace OP.WSAS400.DTO.Common
{
    //  [Serializable]
    [DataContract]
    public class DtoCommon
    {
        [DataMember]
        [Column(Name = "STRRETURNCOL")]
        public string StrReturnCol { get; set; }
        [DataMember]
        [Column(Name = "STRRETURNCOL2")]
        public string StrReturnCol2 { get; set; }
        [DataMember]
        [Column(Name = "INT32RETURNCOL")]
        public Int32 Int32ReturnCol { get; set; }
        [Column(Name = "INT32RETURNCOL2")]
        public Int32 Int32ReturnCol2 { get; set; }
        [DataMember]
        [Column(Name = "INT64RETURNCOL")]
        public Int64? Int64ReturnCol { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBRETURNCOL")]
        public Int64 DateDebReturnCol { get; set; }
        [DataMember]
        [Column(Name = "DATEFINRETURNCOL")]
        public Int64 DateFinReturnCol { get; set; }
        [DataMember]
        [Column(Name = "DATEDEBEFFRETURNCOL")]
        public Int64 DateDebEffReturnCol { get; set; }
        [DataMember]
        [Column(Name = "DATEFINEFFRETURNCOL")]
        public Int64 DateFinEffReturnCol { get; set; }
        [DataMember]
        [Column(Name = "NBLIGN")]
        public Int64 NbLigne { get; set; }
        [Column(Name = "ETAT")]
        public string Etat { get; set; }
        [Column(Name = "SITUATION")]
        public string Situation { get; set; }
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }
        [Column(Name = "CIBLE")]
        public string Cible { get; set; }
        [Column(Name = "CIBLEDESC")]
        public string CibleDesc { get; set; }
        [DataMember]
        [Column(Name = "LETTRELIB")]
        public string LettreLib { get; set; }
        [Column(Name = "MONTANT")]
        public double Montant { get; set; }
        [Column(Name = "ID")]
        public Int64 Id { get; set; }
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        [DataMember]
        [Column(Name = "DECRETURNCOL")]
        public double DecReturnCol { get; set; }
        [DataMember]
        [Column(Name = "DECRETURNCOL2")]
        public double DecReturnCol2 { get; set; }
        [DataMember]
        [Column(Name = "SOUSBRANCHE")]
        public string SousBranche { get; set; }
        [DataMember]
        [Column(Name = "CATEGORIE")]
        public string Categorie { get; set; }
        [Column(Name = "PERIODICITE")]
        public string Periodicite { get; set; }
        [Column(Name = "CODEID1")]
        public Int32 CodeId1 { get; set; }
        [Column(Name = "CODEID2")]
        public Int32 CodeId2 { get; set; }
        [Column(Name = "CODEID3")]
        public Int16 CodeId3 { get; set; }
        [Column(Name = "CODEID4")]
        public Int16 CodeId4 { get; set; }

        [Column(Name = "NATURE")]
        public string Nature { get; set; }
        [Column(Name = "GARID")]
        public Int32 GarId { get; set; }
        /// <summary>
        /// Permet de connaitre l'origine des données KPGARAP/YPRTOBJ/KPOPTAP etc.
        /// </summary>
        [DataMember]
        [Column(Name = "TYPEOBJET")]
        public string TypeObjet { get; set; }

        [DataMember]
        [Column(Name = "AVN_CREA")]
        public Int64 AvenantCrea { get; set; }

        [DataMember]
        [Column(Name = "REGULSTATE")]
        public string RegulState { get; set; }
    }
}