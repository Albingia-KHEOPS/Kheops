using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres
{
    public class OffrePlatDto
    {
        [Column(Name = "CODEOFFRE")]
        public string CodeOffre { get; set; }
        [Column(Name = "VERSIONOFFRE")]
        public Int32 VersionOffre { get; set; }
        [Column(Name = "TYPEOFFRE")]
        public string TypeOffre { get; set; }
        [Column(Name = "DESCRIPTIF")]
        public string Descriptif { get; set; }
        [Column(Name = "CODEINTERLOCUTEUR")]
        public Int32 CodeInterlocuteur { get; set; }
        [Column(Name = "CODECABINETCOURTAGE")]
        public Int32 CodeCabinetCourtage { get; set; }
        [Column(Name = "CODECABINETCOURTAGEBIS")]
        public Int32 CodeCabinetCourtageBis { get; set; }
        //[Column(Name = "NUMEROCHRONO")]
        //public Int32 NumeroChrono { get; set; }
        [Column(Name = "BATIMENT")]
        public string Batiment { get; set; }
        [Column(Name = "EXTENSIONVOIE")]
        public string ExtensionVoie { get; set; }
        [Column(Name = "NUMEROVOIE")]
        public Int16 NumeroVoie { get; set; }
        [Column(Name = "NUMEROVOIE2")]
        public string NumeroVoie2 { get; set; }
        [Column(Name = "NOMVOIE")]
        public string NomVoie { get; set; }
        [Column(Name = "BOITEPOSTALE")]
        public string BoitePostale { get; set; }
        [Column(Name = "DEPARTEMENT")]
        public string Departement { get; set; }
        [Column(Name = "CODEPOSTAL")]
        public Int16 CodePostal { get; set; }
        [Column(Name = "NOMVILLE")]
        public string NomVille { get; set; }
        [Column(Name = "CODEPAYS")]
        public string CodePays { get; set; }
        [Column(Name = "NOMPAYS")]
        public string NomPays { get; set; }
        [Column(Name = "MATHEX")]
        public Int32 MatriculeHexavia { get; set; }
        [Column(Name = "LATITUDE")]
        public decimal? Latitude { get; set; }
        [Column(Name = "LONGITUDE")]
        public decimal? Longitude { get; set; }
        [Column(Name = "TYPECEDEX")]
        public string TypeCedex { get; set; }
        [Column(Name = "REFCOURTIER")]
        public string RefCourtier { get; set; }
        [Column(Name = "ETATOFFRE")]
        public string Etat { get; set; }
        [Column(Name = "ETATLIB")]
        public string LibelleEtat { get; set; }
        [Column(Name = "SITUATION")]
        public string Situation { get; set; }
        [Column(Name = "CODEMOTSCLEF1")]
        public string CodeMotsClef1 { get; set; }
        [Column(Name = "CODEMOTSCLEF2")]
        public string CodeMotsClef2 { get; set; }
        [Column(Name = "CODEMOTSCLEF3")]
        public string CodeMotsClef3 { get; set; }
        [Column(Name = "OBSERVATION")]
        public string Observation { get; set; }
        [Column(Name = "NUMEROCHRONO")]
        public Int32 IdAdresseOffre { get; set; }
        [Column(Name = "NumAvenant")]
        public Int16 NumAvenant { get; set; }
        [Column(Name = "CODEASSURE")]
        public Int32 CodeAssure { get; set; }
        [Column(Name = "CODEAPERITEUR")]
        public string CodeAperiteur { get; set; }
        [Column(Name = "NOMAPERITEUR")]
        public string NomAperiteur { get; set; }
        [Column(Name = "CODEGESTIONNAIRE")]
        public string CodeGestionnaire { get; set; }
        [Column(Name = "NOMGESTIONNAIRE")]
        public string NomGestionnaire { get; set; }
        [Column(Name = "DATESAISIEANNEE")]
        public Int16 DateSaisieAnnee { get; set; }
        [Column(Name = "DATESAISIEMOIS")]
        public Int16 DateSaisieMois { get; set; }
        [Column(Name = "DATESAISIEJOUR")]
        public Int16 DateSaisieJour { get; set; }
        [Column(Name = "DATESAISIE")]
        public Int64 DateSaisie { get; set; }
        [Column(Name = "DATESAISIEHEURE")]
        public Int16 DateSaisieHeure { get; set; }
        [Column(Name = "DATEENREGISTREMENTANNEE")]
        public Int16 DateEnregistrementAnnee { get; set; }
        [Column(Name = "DATEENREGISTREMENTMOIS")]
        public Int16 DateEnregistrementMois { get; set; }
        [Column(Name = "DATEENREGISTREMENTJOUR")]
        public Int16 DateEnregistrementJour { get; set; }
        [Column(Name = "DATEEFFETGARANTIEANNEE")]
        public Int16 DateEffetGarantieAnnee { get; set; }
        [Column(Name = "DATEEFFETGARANTIEMOIS")]
        public Int16 DateEffetGarantieMois { get; set; }
        [Column(Name = "DATEEFFETGARANTIEJOUR")]
        public Int16 DateEffetGarantieJour { get; set; }
        [Column(Name = "DATEEFFETGARANTIE")]
        public Int64 DateEffetGarantie { get; set; }
        [Column(Name = "DATEEFFETGARANTIEHEURE")]
        public Int16 DateEffetGarantieHeure { get; set; }
        [Column(Name = "DATEMAJANNEE")]
        public Int16 DateMAJAnnee { get; set; }
        [Column(Name = "DATEMAJMOIS")]
        public Int16 DateMAJMois { get; set; }
        [Column(Name = "DATEMAJJOUR")]
        public Int16 DateMAJJour { get; set; }
        [Column(Name = "CODEBRANCHE")]
        public string CodeBranche { get; set; }
        [Column(Name = "NOMBRANCHE")]
        public string NomBaranche { get; set; }
        [Column(Name = "CODESOUSCRIPTEUR")]
        public string CodeSouscripteur { get; set; }
        [Column(Name = "NOMSOUSCRIPTEUR")]
        public string NomSouscripteur { get; set; }
        [Column(Name = "DEVISE")]
        public string Devise { get; set; }
        [Column(Name = "PERIODICITE")]
        public string Periodicite { get; set; }
        [Column(Name = "DUREEGARANTIE")]
        public Int32 DureeGarantie { get; set; }
        [Column(Name = "UNITETEMPS")]
        public string UniteTemps { get; set; }
        [Column(Name = "INDICEREFERENCE")]
        public string IndiceReference { get; set; }
        [Column(Name = "CODENATURECONTRAT")]
        public string CodeNatureContrat { get; set; }
        [Column(Name = "LIBELLENATURECONTRAT")]
        public string LibelleNatureContrat { get; set; }
        [Column(Name = "VALEUR")]
        public Single Valeur { get; set; }
        [Column(Name = "PARTALBINGIA")]
        public Single? PartAlbingia { get; set; }
        [Column(Name = "COUVERTURE")]
        public Single Couverture { get; set; }
        [Column(Name = "FRAISAPERITION")]
        public Single? FraisAperition { get; set; }
        [Column(Name = "CODEREGIME")]
        public string CodeRegime { get; set; }
        [Column(Name = "PBSTF")]
        public String MotifRefus { get; set; }

        //[Column(Name = "REGIMLIB")]
        //public string LibelleRegime { get; set; }
        [Column(Name = "SOUMISCATNAT")]
        public string SoumisCatNat { get; set; }
        //[Column(Name = "MONTANTREF1")]
        //public Double MontantRef1 { get; set; }
        //[Column(Name = "MONTANTREF2")]
        //public Double MontantRef2 { get; set; }
        //[Column(Name = "INDEXATION")]
        //public string Indexation { get; set; }
        //[Column(Name = "LCI")]
        //public string LCI { get; set; }
        //[Column(Name = "ASSIETTE")]
        //public string Assiette { get; set; }
        //[Column(Name = "FRANCHISE")]
        //public string Franchise { get; set; }
        //[Column(Name = "PREAVIS")]
        //public int Preavis { get; set; }
        //[Column(Name = "CODEACTION")]
        //public string CodeAction { get; set; }
        //[Column(Name = "LIBACTION")]
        //public string LibelleAction { get; set; }
        //[Column(Name = "LIBSITUATION")]
        //public string LibelleSituation { get; set; }
        //[Column(Name = "DATESITJOUR")]
        //public Int16 DateSituationJour { get; set; }
        //[Column(Name = "DATESITMOIS")]
        //public Int16 DateSituationMois { get; set; }
        //[Column(Name = "DATESITANNEE")]
        //public Int16 DateSituationAnnee { get; set; }
        //[Column(Name = "UCRCODE")]
        //public string CodeUsrCreateur { get; set; }
        //[Column(Name = "UCRNOM")]
        //public string NomUsrCreateur { get; set; }
        //[Column(Name = "UUPCODE")]
        //public string CodeUsrModificateur { get; set; }
        //[Column(Name = "UUPNOM")]
        //public string NomUsrModificateur { get; set; }
        [Column(Name = "INTERCALAIREEXISTE")]
        public string IntercalaireExiste { get; set; }
        [Column(Name = "FINEFFETGARANTIEANNEE")]
        public Int16 DateFinEffetGarantieAnnee { get; set; }
        [Column(Name = "FINEFFETGARANTIEMOIS")]
        public Int16 DateFinEffetGarantieMois { get; set; }
        [Column(Name = "FINEFFETGARANTIEJOUR")]
        public Int16 DateFinEffetGarantieJour { get; set; }
        [Column(Name = "FINEFFETGARANTIEDATE")]
        public Int64 FinEffetGarantie { get; set; }
        [Column(Name = "FINEFFETGARANTIEHEURE")]
        public Int16 DateFinEffetGarantieHeure { get; set; }
        [Column(Name = "CODECIBLE")]
        public string CodeCible { get; set; }
        [Column(Name = "NOMCIBLE")]
        public string NomCible { get; set; }
        [Column(Name = "ECHEANCEPRINCIPALEMOIS")]
        public Int32 EcheancePrincipaleMois { get; set; }
        [Column(Name = "ECHEANCEPRINCIPALEJOUR")]
        public Int32 EcheancePrincipaleJour { get; set; }
        [Column(Name = "ECHEANCEPRINCIPALE")]
        public Int64 EcheancePrincipale { get; set; }
        [Column(Name = "ANNEEDEB")]
        public Int16 AnneeDeb { get; set; }
        [Column(Name = "MOISDEB")]
        public Int16 MoisDeb { get; set; }
        [Column(Name = "JOURDEB")]
        public Int16 JourDeb { get; set; }
        [Column(Name = "HEUREDEB")]
        public Int16 HeureDeb { get; set; }
        [Column(Name = "ANNEEFIN")]
        public Int16 AnneeFin { get; set; }
        [Column(Name = "MOISFIN")]
        public Int16 MoisFin { get; set; }
        [Column(Name = "JOURFIN")]
        public Int16 JourFin { get; set; }
        [Column(Name = "HEUREFIN")]
        public Int16 HeureFin { get; set; }
        [Column(Name = "DATESTATISTIQUE")]
        public Int32 DateStatistique { get; set; }
        [Column(Name = "PARTBENEF")]
        public string PartBenef { get; set; }
        [Column(Name = "PRENEURESTASSURE")]
        public string PreneurEstAssure { get; set; }

        [Column(Name = "DATEAVENANTANNEE")]
        public Int16 DateAvenantAnnee { get; set; }
        [Column(Name = "DATEAVENANTMOIS")]
        public Int16 DateAvenantMois { get; set; }
        [Column(Name = "DATEAVENANTJOUR")]
        public Int16 DateAvenantJour { get; set; }
        [Column(Name = "DATEAVENANTHEURE")]
        public Int32 DateAvenantHeure { get; set; }
        [Column(Name = "LTA")]
        public string LTA { get; set; }

        [Column(Name = "TYPEGESTION")]
        public string TypeGestionAvenant { get; set; }

        [Column(Name = "TYPETRAITEMENT")]
        public string TypeTraitement { get; set; }

    }
}

