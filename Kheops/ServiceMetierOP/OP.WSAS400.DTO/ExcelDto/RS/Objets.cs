using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
namespace OP.WSAS400.DTO.ExcelDto.RS
{
    //Class Dto générée 
    [KnownType(typeof(Objets))]
    [DataContract]
    public class Objets : BaseExcelObjets
    {

        [DataMember]
        [Column(Name = "KFANATL")]
        public string NatureLieux { get; set; }
        
        [DataMember]
        [Column(Name = "KFANBPA")]
        public Int64 NombreParticipants { get; set; }
        
        [DataMember]
        [Column(Name = "KFANBEX")]
        public Int64 NombreExposants { get; set; }
                
        [DataMember]
        [Column(Name = "KFANATS")]
        public string NatureSupport { get; set; }

        [DataMember]
        [Column(Name = "KFANEGA")]
        public string TypeNegatif { get; set; }        

        [DataMember]
        [Column(Name = "KFAFRQE")]
        public string FrequenceEnvoieRushs { get; set; }

        [DataMember]
        [Column(Name = "KFANBEM")]
        public Int32 NbTotalEmission { get; set; }

        [DataMember]
        [Column(Name = "KFATYTN")]
        public string Tournage { get; set; }

        [DataMember]
        [Column(Name = "KFANMDF")]
        public string NomDiffuseur { get; set; }

        [DataMember]
        [Column(Name = "KFALABD")]
        public string LaboDeveloppement { get; set; }

        [DataMember]
        [Column(Name = "KFAFENT")]
        public string FrequenceEnvoiRush { get; set; }

        [DataMember]
        [Column(Name = "KFAFSVT")]
        public string FrequenceSauvegarde { get; set; }

        [DataMember]
        [Column(Name = "KFANMSC")]
        public string NomSocietePostProd { get; set; }

        [DataMember]
        [Column(Name = "KFANBPE")]
        public Int64 NombreEleve { get; set; }

        [DataMember]
        [Column(Name = "KFANAI")]
        public Int64 DateNaissance { get; set; }

        [DataMember]
        [Column(Name = "KFALMA")]
        public Int64 IAReportLimAge { get; set; }

        [DataMember]
        [Column(Name="KFAQMD")]
        public string QuestionMedical { get; set; }

        [DataMember]
        [Column(Name = "KFAIFP")]
        public Double InfirmitePreexist { get; set; }

        [DataMember]
        [Column(Name = "KFADEPD")]
        public Int64 DureeDeplacement { get; set; }

        [DataMember]
        public string CellsNatureLieux { get; set; }

        [DataMember]
        public string CellsNombreParticipants { get; set; }

        [DataMember]
        public string CellsNombreExposants { get; set; }

        [DataMember]
        public string CellsNatureSupport { get; set; }           

        [DataMember]
        public string CellsTypeNegatif { get; set; }
        
        [DataMember]
        public string CellsFrequenceEnvoieRushs { get; set; }

        [DataMember]
        public string CellsNbTotalEmission { get; set; }

        [DataMember]
        public string CellsTournage { get; set; }

        [DataMember]
        public string CellsNomDiffuseur { get; set; }

        [DataMember]
        public string CellsLaboDeveloppement { get; set; }

        [DataMember]
        public string CellsFrequenceEnvoiRush { get; set; }

        [DataMember]
        public string CellsFrequenceSauvegarde { get; set; }

        [DataMember]
        public string CellsNomSocietePostProd { get; set; }

        [DataMember]
        public string CellsNombreEleve { get; set; }

        #region Champs RS - SPECTACLE

        [DataMember]
        [Column(Name = "KFANBVI")]
        public Int32 NbVisiteurs { get; set; }

        [DataMember]
        [Column(Name = "KFANBIN")]
        public Int32 NombreInvite { get; set; }

        [DataMember]
        [Column(Name="KFAAUTL")]
        public string TypeSupport { get; set; }

        [DataMember]
        [Column(Name = "KFAMOD")]
        public string VetementAcc { get; set; }

        //[DataMember]
        //[Column(Name = "KFASLPD")]
        //public string StructureLegerePrevDecla { get; set; }

        //[DataMember]
        //[Column(Name = "KFASLPE")]
        //public string StructureLegerePrevExige { get; set; }

        [DataMember]
        public string CellsNbVisiteurs { get; set; }

        [DataMember]
        public string CellsNombreInvite { get; set; }

        [DataMember]
        public string CellsTypeSupport { get; set; }

        [DataMember]
        public string CellsVetementAcc { get; set; }

        //[DataMember]
        //public string CellsStructureLegerePrevDecla { get; set; }

        //[DataMember]
        //public string CellsStructureLegerePrevExige { get; set; }

        #endregion

        #region Champs IA

        [DataMember]  
        public string CellsDateNaissance { get; set; }

        [DataMember]
        public string CellsIAReportLimAge { get; set; }

        [DataMember]
        public string CellsQuestionMedical { get; set; }

        [DataMember]
        public string CellsInfirmitePreexist { get; set; }

        [DataMember]
        public string CellsDureeDeplacement { get; set; }

        #endregion

        #region Champs RC

        [DataMember]
        [Column(Name = "KFATHF")]
        public string HorsFranceMetro { get; set; }

        [DataMember]
        [Column(Name = "KFATU1")]
        public string USACanadaFrequence { get; set; }

        [DataMember]
        [Column(Name = "KFATU2")]
        public string USACanadaNature { get; set; }

        [DataMember]
        [Column(Name = "KFAASC")]
        public string Ascenceur { get; set; }

        [DataMember]
        [Column(Name = "KFASURF")]
        public Double LocauxSuperficie { get; set; }

        [DataMember]
        [Column(Name = "KFAVMC")]
        public Double LocauxValeur { get; set; }

        [DataMember]
        [Column(Name = "KFAPROL")]
        public string ProfessionLiberale { get; set; }
        
        #endregion

        #region Cells RC

        [DataMember]
        public string CellsHorsFranceMetro { get; set; }
        [DataMember]
        public string CellsUSACanadaFrequence { get; set; }
        [DataMember]
        public string CellsUSACanadaNature { get; set; }
        [DataMember]
        public string CellsAscenceur { get; set; }
        [DataMember]
        public string CellsLocauxSuperficie { get; set; }
        [DataMember]
        public string CellsLocauxValeur { get; set; }
        [DataMember]
        public string CellsProfessionLiberale { get; set; }

        #endregion

        #region AP

        [DataMember]
        [Column(Name = "KFAEXPD")]
        public Int64 DureeExposition { get; set; }

        [DataMember]
        [Column(Name = "KFAEXPDT")]
        public string TypeDureeExposition { get; set; }

        [DataMember]
        public string CellsDureeExposition { get; set; }
        [DataMember]
        public string CellsTypeDureeExposition { get; set; }


        #endregion
    }
}