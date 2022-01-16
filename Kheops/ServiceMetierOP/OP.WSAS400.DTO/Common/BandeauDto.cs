using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Common
{
    public class BandeauDto
    {
        #region contrat
        [DataMember]
        [Column(Name = "NUMAVENANT")]
        public Int16 NumAvenant { get; set; }
        [DataMember]
        [Column(Name = "NUMEXTERNE")]
        public Int16 NumExterne { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETAVNJOUR")]
        public Int16 DateEffetAvenantJour { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETAVNMOIS")]
        public Int16 DateEffetAvenantMois { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETAVNANNEE")]
        public Int16 DateEffetAvenantAnnee { get; set; }
        [DataMember]
        [Column(Name = "CODEOFFREORIGINE")]
        public string CodeOffreOrigine { get; set; }
        [DataMember]
        [Column(Name = "VERSIONOFFREORIGINE")]
        public Int16 VersionOffreOrigine { get; set; }
        [DataMember]
        [Column(Name = "PROCHECHJ")]
        public Int16 ProchaineEchJour { get; set; }
        [DataMember]
        [Column(Name = "PROCHECHM")]
        public Int16 ProchaineEchMois { get; set; }
        [DataMember]
        [Column(Name = "PROCHECHA")]
        public Int16 ProchaineEchAnnee { get; set; }
        [DataMember]
        [Column(Name = "HORSCATNAT")]
        public Double HorsCatNat { get; set; }
        [DataMember]
        [Column(Name = "CATNAT")]
        public Double CatNat { get; set; }
        [DataMember]
        [Column(Name = "TAUXHORSCATNAT")]
        public Double TauxHorsCatNat { get; set; }
        [DataMember]
        [Column(Name = "TAUXCATNAT")]
        public Double TauxCatNat { get; set; }
        [DataMember]
        [Column(Name = "DATEAFFAIRENOUVELLEJOUR")]
        public Int16 DateaffaireNouvelleJour { get; set; }
        [DataMember]
        [Column(Name = "DATEAFFAIRENOUVELLEMOIS")]
        public Int16 DateaffaireNouvelleMois { get; set; }
        [DataMember]
        [Column(Name = "DATEAFFAIRENOUVELLEANNEE")]
        public Int16 DateaffaireNouvelleAnnee { get; set; }
        [DataMember]
        [Column(Name = "STOP")]
        public string Stop { get; set; }
        [DataMember]
        [Column(Name = "STOPLIB")]
        public string StopLib { get; set; }
        [DataMember]
        [Column(Name = "STOPCONTENTIEUX")]
        public string StopContentieux { get; set; }
        [DataMember]
        [Column(Name = "STOPCONTENTIEUXLIB")]
        public string StopContentieuxLib { get; set; }
        [DataMember]
        [Column(Name = "DUREE")]
        public int Duree { get; set; }
        [DataMember]
        [Column(Name = "DUREEUNITE")]
        public string DureeUnite { get; set; }
        [DataMember]
        [Column(Name = "DUREESTR")]
        public string DureeStr { get; set; }

        [DataMember]
        [Column(Name = "PBORK")]
        public string Origine { get; set; }
        #endregion

        #region commune Offre/contrat
        [DataMember]
        [Column(Name = "CODEREGIME")]
        public string CodeRegime { get; set; }
        [DataMember]
        [Column(Name = "LIBREGIME")]
        public string LibelleRegime { get; set; }
        [DataMember]
        [Column(Name = "SOUMISCATNAT")]
        public string SoumisCatNat { get; set; }
        [DataMember]
        [Column(Name = "MONTANTREF1")]
        public Double MontantRef1 { get; set; }
        [DataMember]
        [Column(Name = "MONTANTREF2")]
        public Double MontantRef2 { get; set; }
        [DataMember]
        [Column(Name = "INDEXATION")]
        public string Indexation { get; set; }
        [DataMember]
        [Column(Name = "LCI")]
        public string LCI { get; set; }
        [DataMember]
        [Column(Name = "LCIGENERALE")]
        public Double LCIGenerale { get; set; }
        [DataMember]
        [Column(Name = "LCIGENERALEUNIT")]
        public string LCIGeneraleUnit { get; set; }
        [DataMember]
        [Column(Name = "LCIGENERALETYPE")]
        public string LCIGeneraleType { get; set; }
        [DataMember]
        [Column(Name = "FRCHGENERALE")]
        public Double FranchiseGenerale { get; set; }
        [DataMember]
        [Column(Name = "FRCHGENERALEUNIT")]
        public string FranchiseGeneraleUnit { get; set; }
        [DataMember]
        [Column(Name = "FRCHGENERALETYPE")]
        public string FranchiseGeneraleType { get; set; }
        [DataMember]
        [Column(Name = "ASSIETTE")]
        public string Assiette { get; set; }
        [DataMember]
        [Column(Name = "FRANCHISE")]
        public string Franchise { get; set; }
        [DataMember]
        [Column(Name = "CODEACTION")]
        public string CodeAction { get; set; }
        [DataMember]
        [Column(Name = "LIBACTION")]
        public string LibelleAction { get; set; }
        [DataMember]
        [Column(Name = "DATESITJOUR")]
        public Int16 DateSituationJour { get; set; }
        [DataMember]
        [Column(Name = "DATESITMOIS")]
        public Int16 DateSituationMois { get; set; }
        [DataMember]
        [Column(Name = "DATESITANNEE")]
        public Int16 DateSituationAnnee { get; set; }
        [DataMember]
        [Column(Name = "UCRCODE")]
        public string CodeUsrCreateur { get; set; }
        [DataMember]
        [Column(Name = "UCRNOM")]
        public string NomUsrCreateur { get; set; }
        [DataMember]
        [Column(Name = "UUPCODE")]
        public string CodeUsrModificateur { get; set; }
        [DataMember]
        [Column(Name = "UUPNOM")]
        public string NomUsrModificateur { get; set; }
        [DataMember]
        [Column(Name = "DATECRJOUR")]
        public Int16 DateEnregistrementJour { get; set; }
        [DataMember]
        [Column(Name = "DATECRMOIS")]
        public Int16 DateEnregistrementMois { get; set; }
        [DataMember]
        [Column(Name = "DATECRANNEE")]
        public Int16 DateEnregistrementAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEUPJOUR")]
        public Int16 DateMAJJour { get; set; }
        [DataMember]
        [Column(Name = "DATEUPMOIS")]
        public Int16 DateMAJMois { get; set; }
        [DataMember]
        [Column(Name = "DATEUPANNEE")]
        public Int16 DateMAJAnnee { get; set; }
        [DataMember]
        [Column(Name = "LIBSITUATION")]
        public string LibelleSituation { get; set; }
        [DataMember]
        [Column(Name = "PREAVIS")]
        public int Preavis { get; set; }


        #endregion

        #region informations existant déjà dans l'offre/contrat
        [DataMember]
        [Column(Name = "DATESAISIEANNEE")]
        public Int16 DateSaisieAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATESAISIEMOIS")]
        public Int16 DateSaisieMois { get; set; }
        [DataMember]
        [Column(Name = "DATESAISIEJOUR")]
        public Int16 DateSaisieJour { get; set; }
        [DataMember]
        [Column(Name = "DATESAISIEHEURE")]
        public Int16 DateSaisieHeure { get; set; }
        [DataMember]
        [Column(Name = "CODEOFFRE")]
        public string CodeOffre { get; set; }
        [DataMember]
        [Column(Name = "VERSIONOFFRE")]
        public Int64 VersionOffre { get; set; }
        [DataMember]
        [Column(Name = "TYPEOFFRE")]
        public string TypeOffre { get; set; }
        [DataMember]
        [Column(Name = "BRANCHECODE")]
        public string BrancheCode { get; set; }
        [DataMember]
        [Column(Name = "BRANCHELIB")]
        public string BrancheLib { get; set; }
        [DataMember]
        [Column(Name = "LIBSBR")]
        public string LibSousBranche { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTIF")]
        public string Descriptif { get; set; }
        [DataMember]
        [Column(Name = "CIBLECODE")]
        public string CibleCode { get; set; }
        [DataMember]
        [Column(Name = "CIBLELIB")]
        public string CibleLib { get; set; }
        [DataMember]
        [Column(Name = "CODECOURTIERGESTIONNAIRE")]
        public Int32 CodeCourtierGestionnaire { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERGEST")]
        public string NomCourtierGest { get; set; }
        [DataMember]
        public string CPCourtierGest { get; set; }
        [DataMember]
        public string VilleCourtierGest { get; set; }
        [DataMember]
        [Column(Name = "NOMPRENASSUR")]
        public string NomPreneurAssurance { get; set; }
        [DataMember]
        [Column(Name = "CODEPRENASSUR")]
        public int CodePreneurAssurance { get; set; }
        [DataMember]
        public string CPPreneurAssurance { get; set; }
        [DataMember]
        public string VillePreneurAssurance { get; set; }
        [DataMember]
        [Column(Name = "SOUSCODE")]
        public string SouscripteurCode { get; set; }
        [DataMember]
        [Column(Name = "SOUSNOM")]
        public string SouscripteurNom { get; set; }
        [DataMember]
        [Column(Name = "SOUSPNM")]
        public string SouscripteurPrenom { get; set; }
        [DataMember]
        [Column(Name = "GESCODE")]
        public string GestionnaireCode { get; set; }
        [DataMember]
        [Column(Name = "GESNOM")]
        public string GestionnaireNom { get; set; }
        [DataMember]
        [Column(Name = "GESPNM")]
        public string GestionnairePrenom { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETA")]
        public Int16 DateEffetAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETM")]
        public Int16 DateEffetMois { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETJ")]
        public Int16 DateEffetJour { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETJOUR")]
        public Int16 FinEffetJour { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETMOIS")]
        public Int16 FinEffetMois { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETANNEE")]
        public Int16 FinEffetAnnee { get; set; }
        [DataMember]
        public bool HasDoubleSaisie { get; set; }
        [DataMember]
        [Column(Name = "CODEDEVISE")]
        public string CodeDevise { get; set; }
        [DataMember]
        [Column(Name = "LIBDEVISE")]
        public string LibelleDevise { get; set; }
        [DataMember]
        [Column(Name = "NATURECONTRAT")]
        public string NatureContrat { get; set; }
        [DataMember]
        [Column(Name = "LIBELLENATURECONTRAT")]
        public string LibelleNatureContrat { get; set; }
        [DataMember]
        [Column(Name = "PARTALBINGIA")]
        public Single? PartAlbingia { get; set; }
        [DataMember]
        [Column(Name = "COUVERTURE")]
        public Single Couverture { get; set; }
        [DataMember]
        [Column(Name = "CODEINDICEREFERENCE")]
        public string CodeIndiceReference { get; set; }
        [DataMember]
        [Column(Name = "LIBINDICE")]
        public string LibelleIndicReference { get; set; }
        [DataMember]
        [Column(Name = "VALEUR")]
        public Single Valeur { get; set; }
        [DataMember]
        [Column(Name = "ETAT")]
        public string Etat { get; set; }
        [DataMember]
        [Column(Name = "LIBETAT")]
        public string LibelleEtat { get; set; }
        [DataMember]
        [Column(Name = "CODESITUATION")]
        public string CodeSituation { get; set; }

        [Column(Name = "CODECOURTIERAPPORTEUR")]
        public Int32 CodeCourtierApporteur { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERAPPO")]
        public string NomCourtierAppo { get; set; }
        [DataMember]
        public string VilleCourtierAppo { get; set; }

        [DataMember]
        [Column(Name = "CODECOURTIERPAYEUR")]
        public int CodeCourtierPayeur { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERPAYEUR")]
        public string NomCourtierPayeur { get; set; }
        [DataMember]
        [Column(Name = "VILLEPAYEUR")]
        public string VilleCourtierPayeur { get; set; }
        [DataMember]
        [Column(Name = "PERIODICITENOM")]
        public string PeriodiciteNom { get; set; }
        [DataMember]
        [Column(Name = "CODEENCAISSEMENT")]
        public string CodeEncaissement { get; set; }
        [DataMember]
        [Column(Name = "LIBENCAISSEMENT")]
        public string LibelleEncaissement { get; set; }
        [DataMember]
        [Column(Name = "ECHJOUR")]
        public Int16 EchJour { get; set; }
        [DataMember]
        [Column(Name = "ECHMOIS")]
        public Int16 EchMois { get; set; }
        [DataMember]
        [Column(Name = "TERRITORIALITE")]
        public string Territorialite { get; set; }
        [DataMember]
        [Column(Name = "TERRITORIALITELIB")]
        public string TerritorialiteLib { get; set; }
        [DataMember]
        [Column(Name = "CODEMOTIF")]
        public string CodeMotif { get; set; }
        [DataMember]
        [Column(Name = "LIBMOTIF")]
        public string LibelleMotif { get; set; }
        #endregion

        #region Adresses courtier/assuré

        [Column(Name = "DPGESTIONNAIRE")]
        public string DepartementGestionnaire { get; set; }
        [Column(Name = "CPGESTIONNAIRE")]
        public Int16 CodePostalGestionnaire { get; set; }
        [Column(Name = "VILLEGESTIONNAIRE")]
        public string VilleGestionnaire { get; set; }

        [Column(Name = "DPASSURE")]
        public string DepartementAssure { get; set; }
        [Column(Name = "CPASSURE")]
        public Int16 CodePostalAssure { get; set; }
        [Column(Name = "VILLEASSURE")]
        public string VilleAssure { get; set; }

        [Column(Name = "DPAPPORTEUR")]
        public string DepartementApporteur { get; set; }
        [Column(Name = "CPAPPORTEUR")]
        public Int16 CodePostalApporteur { get; set; }
        [Column(Name = "VILLEAPPORTEUR")]
        public string VilleApporteur { get; set; }

        [Column(Name = "DPPAYEUR")]
        public string DepartementPayeur { get; set; }
        [Column(Name = "CPPAYEUR")]
        public Int16 CodePostalPayeur { get; set; }

        #endregion
        #region Geo
        [DataMember]
        [Column(Name = "LNG")]
        public decimal Long { get; set; }
        [DataMember]
        [Column(Name = "LAT")]
        public decimal Lat { get; set; }
            #endregion

        [DataMember]
        [Column(Name = "PBTTR")]
        public string TypeTraitement
        {
            get; set;
        }
        [DataMember]
        [Column(Name = "MAXIDREGUL")]
        public long? MaxReguleId
        {
            get; set;
        }

        public double MontantStatistique { get; set; }
        public string NomDelegation { get; set; }
        public string NomInspecteur { get; set; }
        public string Secteur { get; set; }
        public string LibSecteur { get; set; }

        public bool HasSusp { get; set; }
        public bool TauxAvailable { get; set; }
    }
}
