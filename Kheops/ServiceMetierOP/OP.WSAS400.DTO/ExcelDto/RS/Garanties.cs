using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
namespace OP.WSAS400.DTO.ExcelDto.RS
{
    //Class Dto générée 
    [KnownType(typeof(Garanties))]
    [DataContract]
    public class Garanties : BaseExcelObjets
    {

        [DataMember]
        [Column(Name = "KFDBO01")]
        public string BonificationCasNonSinistre { get; set; }

        [DataMember]
        [Column(Name = "KFDBO02")]
        public Single MontantBon { get; set; }

        [DataMember]
        [Column(Name = "KFDBO03")]
        public string BonificationAnticipe { get; set; }

        [DataMember]
        [Column(Name = "KFDRA05")]
        public string RachatBijouxFourrure { get; set; }

        [DataMember]
        [Column(Name = "KFDRA08")]
        public string Chapitaux { get; set; }

        [DataMember]
        [Column(Name = "KFDAD02")]
        public string DesignationIndivAccident { get; set; }

        [DataMember]
        [Column(Name = "KFDAD03")]
        public Double CapitalDeces { get; set; }

        [DataMember]
        [Column(Name = "KFDAD04")]
        public Double CapitalIP { get; set; }

        [DataMember]
        [Column(Name = "KFDRC01")]
        public string GarantieEtendueRespCiv { get; set; }

        [DataMember]
        [Column(Name = "KFDRC02")]
        public string DesignationRespCivile { get; set; }

        [DataMember]
        [Column(Name = "KFDRC05")]
        public string SceneDangereuse { get; set; }

        [DataMember]
        [Column(Name = "KFDMAJD")]
        public Int32 DateMaj { get; set; }

        [DataMember]
        [Column(Name = "KFDMAJH")]
        public Int32 HeureMaj { get; set; }

        [DataMember]
        [Column(Name = "KFDMAJU")]
        public string Utilisateur { get; set; }

        [DataMember]
        [Column(Name = "KFDNBJI")]
        public Int32 SiteInaccessibleDureeGarantie { get; set; }

        [DataMember]
        [Column(Name = "KFDGARAV")]
        public string PriseEffetGtieAvantManifNbHeure { get; set; }

        [DataMember]
        [Column(Name = "KFDAN04")]
        public string AudioAttDuree10j { get; set; }

        [DataMember]
        [Column(Name = "KFDVOLAP")]
        public string FinEffetVolApresManif { get; set; }


        [DataMember]
        [Column(Name = "KFDRAY")]
        public Int32 RayonCouv { get; set; }


        [DataMember]
        public string CellsBonificationCasNonSinistre { get; set; }

        [DataMember]
        public string CellsMontantBon { get; set; }

        [DataMember]
        public string CellsBonificationAnticipe { get; set; }

        [DataMember]
        public string CellsRachatBijouxFourrure { get; set; }

        [DataMember]
        public string CellsChapitaux { get; set; }

        [DataMember]
        public string CellsDesignationIndivAccident { get; set; }

        [DataMember]
        public string CellsCapitalDeces { get; set; }

        [DataMember]
        public string CellsCapitalIP { get; set; }

        [DataMember]
        public string CellsGarantieEtendueRespCiv { get; set; }

        [DataMember]
        public string CellsDesignationRespCivile { get; set; }

        [DataMember]
        public string CellsSceneDangereuse { get; set; }

        [DataMember]
        public string CellsDateMaj { get; set; }

        [DataMember]
        public string CellsHeureMaj { get; set; }

        [DataMember]
        public string CellsUtilisateur { get; set; }

        [DataMember]
        public string CellsSiteInaccessibleDureeGarantie { get; set; }

        [DataMember]
        public string CellsPriseEffetGtieAvantManifNbHeure { get; set; }

        [DataMember]
        public string CellsAudioAttDuree10j { get; set; }

        [DataMember]
        public string CellsFinEffetVolApresManif { get; set; }

        [DataMember]
        public string CellsRayonCouv { get; set; }

        #region Champs RS - SPECTACLE

        [DataMember]
        [Column(Name = "KFDAN02")]
        public string Attantat45j { get; set; }

        [DataMember]
        [Column(Name = "KFDAN03")]
        public Int32 AutreDuree { get; set; }

        [DataMember]
        [Column(Name = "KFDIM08")]
        public string FinCouverture { get; set; }

        [DataMember]
        [Column(Name = "KFDIM09")]
        public Int64 NActeFinCouverture { get; set; }

        [DataMember]
        [Column(Name = "KFDSORN")]
        public Int64 NombreEmissions { get; set; }

        [DataMember]
        [Column(Name = "KFDSORD")]
        public Int64 DateLimite { get; set; }

        [DataMember]
        [Column(Name = "KFDSORR")]
        public Double Ristourne { get; set; }

        [DataMember]
        [Column(Name = "KFDFRDM")]
        public Double FraisSuppBudgetMax { get; set; }

        [DataMember]
        [Column(Name = "KFDEFFV")]
        public Double PriseEffetAvantManif { get; set; }

        [DataMember]
        [Column(Name = "KFDCNVD")]
        public string Convention { get; set; }

        [DataMember]
        [Column(Name = "KFDNBGR")]
        public Int64 NbGarantieRepresentation { get; set; }

        [DataMember]
        [Column(Name = "KFDIM10")]
        public string HoraireFinCouverture { get; set; }

        [DataMember]
        [Column(Name = "KFDAN07")]
        public Int32 DelaisAvantDate { get; set; }
        
        [DataMember]
        [Column(Name = "KFDAN05")]
        public Int32 RetraitDelaisAvantDate { get; set; }

        [DataMember]
        [Column(Name = "KFDRAY5")]
        public Int32 RetraitRayonCouv { get; set; }

        [DataMember]
        [Column(Name = "KFDAN06")]
        public Int32 MenaceDelaisAvantDate { get; set; }

        [DataMember]
        [Column(Name = "KFDCLAL")]
        public string ClauseLibreAccord { get; set; }

        
        [DataMember]
        public string CellsAttantat45j { get; set; }

        [DataMember]
        public string CellsAutreDuree { get; set; }

        [DataMember]
        public string CellsFinCouverture { get; set; }

        [DataMember]
        public string CellsNActeFinCouverture { get; set; }

        [DataMember]
        public string CellsNombreEmissions { get; set; }

        [DataMember]
        public string CellsDateLimite { get; set; }

        [DataMember]
        public string CellsRistourne { get; set; }

        [DataMember]
        public string CellsFraisSuppBudgetMax { get; set; }

        [DataMember]
        public string CellsPriseEffetAvantManif { get; set; }

        [DataMember]
        public string CellsConvention { get; set; }

        [DataMember]
        public string CellsNbGarantieRepresentation { get; set; }

        [DataMember]
        public string CellsHoraireFinCouverture { get; set; }

        [DataMember]
        public string CellsDelaisAvantDate { get; set; }

        [DataMember]
        public string CellsRetraitDelaisAvantDate { get; set; }

        [DataMember]
        public string CellsRetraitRayonCouv { get; set; }

        [DataMember]
        public string CellsMenaceDelaisAvantDate { get; set; }

        [DataMember]
        public string CellsClauseLibreAccord { get; set; }


        #endregion

        #region Champs RC

        [DataMember]
        [Column(Name="KFDMXM")]
        public Double Precision { get; set; }

        #endregion

        #region Cells RC

        [DataMember]
        public string CellsPrecision { get; set; }

        #endregion

    }
}