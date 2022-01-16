using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ExcelDto.RS
{
    [KnownType(typeof(RSRecupGaranties))]
    [DataContract]
    public class RSRecupGaranties
    {
        [DataMember]
        [Column(Name = "KFDGN01")]
        public Double MontantParEleve { get; set; }

        [DataMember]
        [Column(Name = "KFDGN02")]
        public string SeuilInterruptionScolarite { get; set; }

        [DataMember]
        [Column(Name = "KFDGN03")]
        public string SeuilRedoublement { get; set; }

        [DataMember]
        [Column(Name = "KFDGN04")]
        public string SeuilsCoursParticuliers { get; set; }

        [DataMember]
        [Column(Name = "KFDGN05")]
        public string PerteRevenue { get; set; }

        [DataMember]
        [Column(Name = "KFDGN11")]
        public string DecesEleve { get; set; }

        [DataMember]
        [Column(Name = "KFDGN12")]
        public Double FraisScolaritePourcent { get; set; }

        [DataMember]
        [Column(Name = "KFDGN13")]
        public string AutreBase { get; set; }

        [DataMember]
        [Column(Name = "KFDGN14")]
        public string Observations { get; set; }

        [DataMember]
        [Column(Name = "KFDGN15")]
        public string EtudesObservations { get; set; }

        [DataMember]
        [Column(Name = "KFDGN06")]
        public string RenonciaRecourRecip { get; set; }

        [DataMember]
        [Column(Name = "KFDGN07")]
        public string RenonciaRecour { get; set; }

        [DataMember]
        [Column(Name = "KFDAN07")]
        public string ExclusionPandemie { get; set; }

        [DataMember]
        [Column(Name = "KFDIM01")]
        public string Intemperiesetendues { get; set; }

        [DataMember]
        [Column(Name = "KFDIM02")]
        public string IntemperiesTourneesBatiment { get; set; }

        [DataMember]
        [Column(Name = "KFDIM03")]
        public string IntemperiesSansHuissier { get; set; }

        [DataMember]
        [Column(Name = "KFDIM04")]
        public string IntemperieFeuArtifice { get; set; }

        [DataMember]
        [Column(Name = "KFDIM05")]
        public string IntemperiesRegates { get; set; }

        [DataMember]
        [Column(Name = "KFDIM06")]
        public string IntemperiesCroisiere { get; set; }

        [DataMember]
        [Column(Name = "KFDRA11")]
        public string RachatExclusionBris { get; set; }

        [DataMember]
        [Column(Name = "KFDRA14")]
        public string RegleProportionnelle { get; set; }

        [DataMember]
        [Column(Name = "KFDRA21")]
        public string transportComplementaire { get; set; }

        [DataMember]
        [Column(Name = "KFD04")]
        public string ClauseGestionAdhesion { get; set; }

        [DataMember]
        [Column(Name = "KFDAN01")]
        public string ToutSaufPerilDenomme { get; set; }

        [DataMember]
        [Column(Name = "KFDAN04")]
        public string DureeCouverture { get; set; }

        [DataMember]
        [Column(Name = "KFDAN06")]
        public string DefinitionPertesPrevues { get; set; }

        [DataMember]
        [Column(Name = "KFDAN08")]
        public string RetransmissionDirect { get; set; }

        [DataMember]
        [Column(Name = "KFDAN09")]
        public string RuptureFaisceau { get; set; }

        [DataMember]
        [Column(Name = "KFDAN10")]
        public string CaluseSortieTV { get; set; }

        [DataMember]
        [Column(Name = "KFDAN11")]
        public string PenurieForceePublic { get; set; }

        [DataMember]
        [Column(Name = "KFDAN12")]
        public string AnnulationSuiteAccidentEndeuillant { get; set; }

        [DataMember]
        [Column(Name = "KFDAN14")]
        public string FraisSupplementaire { get; set; }

        [DataMember]
        [Column(Name = "KFDID01")]
        public string DeuilFamilial { get; set; }

        [DataMember]
        [Column(Name = "KFDID02")]
        public string DeuilFamilialEtendue { get; set; }

        [DataMember]
        [Column(Name = "KFDID03")]
        public string IndisponibiliteSimultannee { get; set; }

        [DataMember]
        [Column(Name = "KFDID04")]
        public string IndisponibiliteMaterielVole { get; set; }

        [DataMember]
        [Column(Name = "KFDID05")]
        public string NatureIndisponibilitePersonne { get; set; }

        [DataMember]
        [Column(Name = "KFDID06")]
        public string IndisponibiliteRetardTransport { get; set; }

        [DataMember]
        [Column(Name = "KFDID07")]
        public string ControlMedicalNonRecu { get; set; }

        [DataMember]
        [Column(Name = "KFDID08")]
        public string Clause30jIndisp { get; set; }

        [DataMember]
        [Column(Name = "KFDID09")]
        public string AttestationBonneSante { get; set; }

        [DataMember]
        [Column(Name = "KFDID10")]
        public string PersonnePlus65 { get; set; }

        [DataMember]
        [Column(Name = "KFDID11")]
        public string PersonneMoins16 { get; set; }

        [DataMember]
        [Column(Name = "KFDID13")]
        public string IndiponibiliteEquipeTechnique { get; set; }

        [DataMember]
        [Column(Name = "KFDID14")]
        public string IndisPersonne { get; set; }

        [DataMember]
        [Column(Name = "KFDIM07")]
        public string IntempIntemperies { get; set; }

        [DataMember]
        [Column(Name = "KFDAN13")]
        public string PlainAirSansIntemperies { get; set; }

        [DataMember]
        [Column(Name = "KFDRA01")]
        public string RachatBijoux { get; set; }

        [DataMember]
        [Column(Name = "KFDRA02")]
        public Double MontantRachatBijoux { get; set; }

        [DataMember]
        [Column(Name = "KFDRA03")]
        public string RachatFourrure { get; set; }

        [DataMember]
        [Column(Name = "KFDRA04")]
        public Double MontantRachatFourrure { get; set; }

        [DataMember]
        [Column(Name = "KFDRA06")]
        public string EcranPlasmaLcd { get; set; }

        [DataMember]
        [Column(Name = "KFDRA07")]
        public string RachatExclusionBrisFragiles { get; set; }

        [DataMember]
        [Column(Name = "KFDRA09")]
        public string RachatDommageElec { get; set; }

        [DataMember]
        [Column(Name = "KFDRA10")]
        public string RachatBrisFonctionnel { get; set; }

        [DataMember]
        [Column(Name = "KFDRA12")]
        public string RachatIntemperies { get; set; }

        [DataMember]
        [Column(Name = "KFDRA13")]
        public string RachatTransport { get; set; }

        [DataMember]
        [Column(Name = "KFDPR02")]
        public string PreventionMonDemont { get; set; }

        [DataMember]
        [Column(Name = "KFDPR03")]
        public string PreventionVetmCuirPeaux { get; set; }

        [DataMember]
        [Column(Name = "KFDPR10")]
        public string SurvExclusionPerteDisp { get; set; }

        [DataMember]
        [Column(Name = "KFDEX01")]
        public string ExclusionVolVehiculeStaionne { get; set; }

        [DataMember]
        [Column(Name = "KFDEX02")]
        public string ExclusionVolOuverture { get; set; }

        [DataMember]
        [Column(Name = "KFDEX03")]
        public string ExclusionInstrumentMusique { get; set; }

        [DataMember]
        [Column(Name = "KFDEX04")]
        public string ExclusionVelo { get; set; }

        [DataMember]
        [Column(Name = "KFDEX05")]
        public string ExclusionMatMultimedia { get; set; }

        [DataMember]
        [Column(Name = "KFDEX06")]
        public string ExclusionOrdinateurPortable { get; set; }

        [DataMember]
        [Column(Name = "KFDEX07")]
        public string ExclusionStructureLegere { get; set; }

        [DataMember]
        [Column(Name = "KFDEX08")]
        public string ClauseCanonExclusion { get; set; }

        [DataMember]
        [Column(Name = "KFDRA15")]
        public string IntempOrganisateur { get; set; }

        [DataMember]
        [Column(Name = "KFDRA16")]
        public string IntempExposant { get; set; }

        [DataMember]
        [Column(Name = "KFDRA17")]
        public string transportOrganisateur { get; set; }

        [DataMember]
        [Column(Name = "KFDRA18")]
        public string transportExposant { get; set; }

        [DataMember]
        [Column(Name = "KFDRA19")]
        public string ExposantUniqueEcran { get; set; }

        [DataMember]
        [Column(Name = "KFDRA20")]
        public string franchiseEnCumul { get; set; }

        [DataMember]
        [Column(Name = "KFD01")]
        public string TarifGarObligComple { get; set; }

        [DataMember]
        [Column(Name = "KFD02")]
        public string RestDureeVol { get; set; }

        [DataMember]
        [Column(Name = "KFD03")]
        public string RestDureeVolLib { get; set; }

        [DataMember]
        [Column(Name = "KFDAN15")]
        public string TypeGarantie { get; set; }

        [DataMember]
        [Column(Name = "KFDRC04")]
        public string PriseVueRisque { get; set; }

        [DataMember]
        [Column(Name = "KFDAN16")]
        public string AnnuAttentats { get; set; }

        [DataMember]
        [Column(Name = "KFDSA01")]
        public string RachatStopDates { get; set; }

        [DataMember]
        [Column(Name = "KFDSA02")]
        public string ParticipationScenesDangereuses { get; set; }

        [DataMember]
        [Column(Name = "KFDSA03")]
        public string RachatExclusionPilotage { get; set; }

        [DataMember]
        [Column(Name = "KFDSA04")]
        public string RachatExclusionCompetition { get; set; }

        [DataMember]
        [Column(Name = "KFDSA05")]
        public string RachatExclusionSportAerien { get; set; }

        [DataMember]
        [Column(Name = "KFDID12")]
        public string RachatExclMaldInfantiles { get; set; }

        [DataMember]
        [Column(Name = "KFDAD05")]
        public string ADPSceneDangereuse { get; set; }

        [DataMember]
        [Column(Name = "KFDPR01")]
        public string SecuriteBatiments { get; set; }

        [DataMember]
        [Column(Name = "KFDPR04")]
        public string PrevOrdiPort { get; set; }

        [DataMember]
        [Column(Name = "KFDPR05")]
        public string Gardiennages { get; set; }

        [DataMember]
        [Column(Name = "KFDPR06")]
        public string PreventionInstrumentsMusiques { get; set; }

        [DataMember]
        [Column(Name = "KFDPR07")]
        public string ClauseSurveillance { get; set; }

        [DataMember]
        [Column(Name = "KFDPR08")]
        public string SurveillancePetitsObjets { get; set; }

        [DataMember]
        [Column(Name = "KFDPR09")]
        public string SurveillanceObjetsValeurs { get; set; }

        [DataMember]
        [Column(Name = "KFDPR11")]
        public string SurvManqueInvent { get; set; }

        [DataMember]
        [Column(Name = "KFDPR12")]
        public string SurveillanceObjetValeur { get; set; }

        [DataMember]
        [Column(Name = "KFDAD01")]
        public string GarantieEtendueIndAcc { get; set; }

        [DataMember]
        [Column(Name = "KFDRC03")]
        public string TribunesDemontables { get; set; }

        [DataMember]
        [Column(Name = "KFDRC06")]
        public string PersonnelEtat { get; set; }

        [DataMember]
        [Column(Name = "KFDRC07")]
        public string precisionPersEtat { get; set; }

        [DataMember]
        [Column(Name = "KFDRC08")]
        public string ExtensionUSACanada { get; set; }

        [DataMember]
        [Column(Name = "KFDRC09")]
        public string AgravationProtection { get; set; }

         [DataMember]
         [Column(Name = "KFDMP01")]
        public Int64 MappageTexteGarantie { get; set; }

        


        #region Cells

        [DataMember]
        public string CellsMontantParEleve { get; set; }

        [DataMember]
        public string CellsSeuilInterruptionScolarite { get; set; }

        [DataMember]
        public string CellsSeuilRedoublement { get; set; }

        [DataMember]
        public string CellsSeuilsCoursParticuliers { get; set; }

        [DataMember]
        public string CellsPerteRevenue { get; set; }

        [DataMember]
        public string CellsDecesEleve { get; set; }

        [DataMember]
        public string CellsFraisScolaritePourcent { get; set; }

        [DataMember]
        public string CellsAutreBase { get; set; }

        [DataMember]
        public string CellsObservations { get; set; }

        [DataMember]
        public string CellsEtudesObservations { get; set; }

        [DataMember]
        public string CellsRenonciaRecourRecip { get; set; }

        [DataMember]
        public string CellsRenonciaRecour { get; set; }

        [DataMember]
        public string CellsExclusionPandemie { get; set; }

        [DataMember]
        public string CellsIntemperiesetendues { get; set; }

        [DataMember]
        public string CellsIntemperiesTourneesBatiment { get; set; }

        [DataMember]
        public string CellsIntemperiesSansHuissier { get; set; }

        [DataMember]
        public string CellsIntemperieFeuArtifice { get; set; }

        [DataMember]
        public string CellsIntemperiesRegates { get; set; }

        [DataMember]
        public string CellsIntemperiesCroisiere { get; set; }

        [DataMember]
        public string CellsRachatExclusionBris { get; set; }

        [DataMember]
        public string CellsRegleProportionnelle { get; set; }

        [DataMember]
        public string CellstransportComplementaire { get; set; }

        [DataMember]
        public string CellsClauseGestionAdhesion { get; set; }

        [DataMember]
        public string CellsToutSaufPerilDenomme { get; set; }

        [DataMember]
        public string CellsDureeCouverture { get; set; }

        [DataMember]
        public string CellsDefinitionPertesPrevues { get; set; }

        [DataMember]
        public string CellsRetransmissionDirect { get; set; }

        [DataMember]
        public string CellsRuptureFaisceau { get; set; }

        [DataMember]
        public string CellsCaluseSortieTV { get; set; }

        [DataMember]
        public string CellsPenurieForceePublic { get; set; }

        [DataMember]
        public string CellsAnnulationSuiteAccidentEndeuillant { get; set; }

        [DataMember]
        public string CellsFraisSupplementaire { get; set; }

        [DataMember]
        public string CellsDeuilFamilial { get; set; }

        [DataMember]
        public string CellsDeuilFamilialEtendue { get; set; }

        [DataMember]
        public string CellsIndisponibiliteSimultannee { get; set; }

        [DataMember]
        public string CellsIndisponibiliteMaterielVole { get; set; }

        [DataMember]
        public string CellsNatureIndisponibilitePersonne { get; set; }

        [DataMember]
        public string CellsIndisponibiliteRetardTransport { get; set; }

        [DataMember]
        public string CellsControlMedicalNonRecu { get; set; }

        [DataMember]
        public string CellsClause30jIndisp { get; set; }

        [DataMember]
        public string CellsAttestationBonneSante { get; set; }

        [DataMember]
        public string CellsPersonnePlus65 { get; set; }

        [DataMember]
        public string CellsPersonneMoins16 { get; set; }

        [DataMember]
        public string CellsIndiponibiliteEquipeTechnique { get; set; }

        [DataMember]
        public string CellsIndisPersonne { get; set; }

        [DataMember]
        public string CellsIntempIntemperies { get; set; }

        [DataMember]
        public string CellsPlainAirSansIntemperies { get; set; }

        [DataMember]
        public string CellsRachatBijoux { get; set; }

        [DataMember]
        public string CellsMontantRachatBijoux { get; set; }

        [DataMember]
        public string CellsRachatFourrure { get; set; }

        [DataMember]
        public string CellsMontantRachatFourrure { get; set; }

        [DataMember]
        public string CellsEcranPlasmaLcd { get; set; }

        [DataMember]
        public string CellsRachatExclusionBrisFragiles { get; set; }

        [DataMember]
        public string CellsRachatDommageElec { get; set; }

        [DataMember]
        public string CellsRachatBrisFonctionnel { get; set; }

        [DataMember]
        public string CellsRachatIntemperies { get; set; }

        [DataMember]
        public string CellsRachatTransport { get; set; }

        [DataMember]
        public string CellsPreventionMonDemont { get; set; }

        [DataMember]
        public string CellsPreventionVetmCuirPeaux { get; set; }

        [DataMember]
        public string CellsSurvExclusionPerteDisp { get; set; }

        [DataMember]
        public string CellsExclusionVolVehiculeStaionne { get; set; }

        [DataMember]
        public string CellsExclusionVolOuverture { get; set; }

        [DataMember]
        public string CellsExclusionInstrumentMusique { get; set; }

        [DataMember]
        public string CellsExclusionVelo { get; set; }

        [DataMember]
        public string CellsExclusionMatMultimedia { get; set; }

        [DataMember]
        public string CellsExclusionOrdinateurPortable { get; set; }

        [DataMember]
        public string CellsExclusionStructureLegere { get; set; }

        [DataMember]
        public string CellsClauseCanonExclusion { get; set; }

        [DataMember]
        public string CellsIntempOrganisateur { get; set; }

        [DataMember]
        public string CellsIntempExposant { get; set; }

        [DataMember]
        public string CellstransportOrganisateur { get; set; }

        [DataMember]
        public string CellstransportExposant { get; set; }

        [DataMember]
        public string CellsExposantUniqueEcran { get; set; }

        [DataMember]
        public string CellsfranchiseEnCumul { get; set; }

        [DataMember]
        public string CellsTarifGarObligComple { get; set; }

        [DataMember]
        public string CellsRestDureeVol { get; set; }

        [DataMember]
        public string CellsRestDureeVolLib { get; set; }

        [DataMember]
        public string CellsTypeGarantie { get; set; }

        [DataMember]
        public string CellsPriseVueRisque { get; set; }

        [DataMember]
        public string CellsAnnuAttentats { get; set; }

        [DataMember]
        public string CellsRachatStopDates { get; set; }

        [DataMember]
        public string CellsParticipationScenesDangereuses { get; set; }

        [DataMember]
        public string CellsRachatExclusionPilotage { get; set; }

        [DataMember]
        public string CellsRachatExclusionCompetition { get; set; }

        [DataMember]
        public string CellsRachatExclusionSportAerien { get; set; }

        [DataMember]
        public string CellsRachatExclMaldInfantiles { get; set; }

        [DataMember]
        public string CellsADPSceneDangereuse { get; set; }

        [DataMember]
        public string CellsSecuriteBatiments { get; set; }

        [DataMember]
        public string CellsPrevOrdiPort { get; set; }

        [DataMember]
        public string CellsGardiennages { get; set; }

        [DataMember]
        public string CellsPreventionInstrumentsMusiques { get; set; }

        [DataMember]
        public string CellsClauseSurveillance { get; set; }

        [DataMember]
        public string CellsSurveillancePetitsObjets { get; set; }

        [DataMember]
        public string CellsSurveillanceObjetsValeurs { get; set; }

        [DataMember]
        public string CellsSurvManqueInvent { get; set; }

        [DataMember]
        public string CellsSurveillanceObjetValeur { get; set; }

        [DataMember]
        public string CellsGarantieEtendueIndAcc { get; set; }

        [DataMember]
        public string CellsTribunesDemontables { get; set; }

        [DataMember]
        public string CellsPersonnelEtat { get; set; }

        [DataMember]
        public string CellsprecisionPersEtat { get; set; }

        [DataMember]
        public string CellsExtensionUSACanada { get; set; }

        [DataMember]     
        public string CellsAgravationProtection { get; set; }
        
        [DataMember]    
        public string CellsMappageTexteGarantie { get; set; }

        #endregion

    }
}
