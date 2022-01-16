using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres.Risque.Objet
{
    public class ObjetPlatDto
    {
        [Column(Name = "CODE")]
        public int Code { get; set; }
        [Column(Name = "CODERISQUE")]
        public int CodeRisque { get; set; }
        [Column(Name = "DESCRIPTIF")]
        public String Descriptif { get; set; }
        [Column(Name = "CODECIBLE")]
        public String CodeCible { get; set; }
        [Column(Name = "CHRONODESI")]
        public Int64 ChronoDesi { get; set; }
        [Column(Name = "LIBELLEOBJET")]
        public String Designation { get; set; }
        [Column(Name = "IDOFFRE")]
        public String IdOffre { get; set; }
        [Column(Name = "VERSIONOFFRE")]
        public Int32 VersionOffre { get; set; }
        [Column(Name = "VALEUR")]
        public Int64? Valeur { get; set; }
        [Column(Name = "CODEUNITE")]
        public string CodeUnite { get; set; }
        [Column(Name = "CODETYPE")]
        public string CodeType { get; set; }
        [Column(Name = "VALEURHT")]
        public string ValeurHT { get; set; }
        [Column(Name = "COUTM2")]
        public Int64? CoutM2 { get; set; }
        [Column(Name = "ISRISQUEINDEXE")]
        public string IsRisqueIndexe { get; set; }
        [Column(Name = "ISLCI")]
        public string IsLCI { get; set; }
        [Column(Name = "ISFRANCHISE")]
        public string IsFranchise { get; set; }
        [Column(Name = "ISASSIETTE")]
        public string IsAssiette { get; set; }
        [Column(Name = "REGIMETAXE")]
        public string RegimeTaxe { get; set; }
        [Column(Name = "ISCATNAT")]
        public string IsCATNAT { get; set; }
        [Column(Name = "CODEAPE")]
        public string CodeApe { get; set; }
        [Column(Name = "NOMENCLATURE1")]
        public string Nomenclature1 { get; set; }
        [Column(Name = "NOMENCLATURE2")]
        public string Nomenclature2 { get; set; }
        [Column(Name = "NOMENCLATURE3")]
        public string Nomenclature3 { get; set; }
        [Column(Name = "NOMENCLATURE4")]
        public string Nomenclature4 { get; set; }
        [Column(Name = "NOMENCLATURE5")]
        public string Nomenclature5 { get; set; }
        [Column(Name = "DATEMISEAJOUR")]
        public Int32 DateModificationObjetRisque { get; set; }
        [Column(Name = "TERRITORIALITE")]
        public string Territorialite { get; set; }
        [Column(Name = "CODETRE")]
        public string CodeTre { get; set; }
        [Column(Name = "LIBTRE")]
        public string LibTre { get; set; }
        [Column(Name = "CODECLASSE")]
        public string CodeClasse { get; set; }
        [Column(Name = "ENSEMBLETYPE")]
        public string EnsembleType { get; set; }
        [Column(Name = "ENTREEGARANTIEANNEE")]
        public Int16 EntreeGarantieAnnee { get; set; }
        [Column(Name = "ENTREEGARANTIEMOIS")]
        public Int16 EntreeGarantieMois { get; set; }
        [Column(Name = "ENTREEGARANTIEJOUR")]
        public Int16 EntreeGarantieJour { get; set; }
        [Column(Name = "ENTREEGARANTIEHEURE")]
        public Int16 EntreeGarantieHeure { get; set; }
        [Column(Name = "SORTIEGARANTIEANNEE")]
        public Int16 SortieGarantieAnnee { get; set; }
        [Column(Name = "SORTIEGARANTIEMOIS")]
        public Int16 SortieGarantieMois { get; set; }
        [Column(Name = "SORTIEGARANTIEJOUR")]
        public Int16 SortieGarantieJour { get; set; }
        [Column(Name = "SORTIEGARANTIEHEURE")]
        public Int16 SortieGarantieHeure { get; set; }

        [Column(Name = "SORTIEGARANTIEANNEEHISTO")]
        public Int16 SortieGarantieAnneeHisto { get; set; }
        [Column(Name = "SORTIEGARANTIEMOISHISTO")]
        public Int16 SortieGarantieMoisHisto { get; set; }
        [Column(Name = "SORTIEGARANTIEJOURHISTO")]
        public Int16 SortieGarantieJourHisto { get; set; }
        [Column(Name = "SORTIEGARANTIEHEUREHISTO")]
        public Int16 SortieGarantieHeureHisto { get; set; }

        [Column(Name = "NUMEROCHRONO")]
        public Int32 NumeroChrono { get; set; }
        [Column(Name = "BATIMENT")]
        public string Batiment { get; set; }
        [Column(Name = "EXTENSIONVOIE")]
        public string ExtensionVoie { get; set; }
        [Column(Name = "NUMEROVOIE")]
        public Int16 NumeroVoie { get; set; }
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
        public string CodePays { get; set; }
        public string TypeCedex { get; set; }



        [Column(Name = "TYPERISQUE")]
        public string TypeRisque { get; set; }
        [Column(Name = "TYPEMATERIEL")]
        public string TypeMateriel { get; set; }
        [Column(Name = "NATURELIEUX")]
        public string NatureLieux { get; set; }

        [Column(Name = "DATEDEBDESC")]
        public int DateDebDesc { get; set; }
        [Column(Name = "DATEFINDESC")]
        public int DateFinDesc { get; set; }
        [Column(Name = "HEUREDEBDESC")]
        public int HeureDebDesc { get; set; }
        [Column(Name = "HEUREFINDESC")]
        public int HeureFinDesc { get; set; }

        [Column(Name = "DATEEFFETAVNJOUR")]
        public Int16 DateEffetAvenantJour { get; set; }
        [Column(Name = "DATEEFFETAVNMOIS")]
        public Int16 DateEffetAvenantMois { get; set; }
        [Column(Name = "DATEEFFETAVNANNEE")]
        public Int16 DateEffetAvenantAnnee { get; set; }


        [Column(Name = "DATEEFFETAVNJOUROBJ")]
        public Int16 DateEffetAvenantJourobj { get; set; }
        [Column(Name = "DATEEFFETAVNMOISOBJ")]
        public Int16 DateEffetAvenantMoisobj { get; set; }
        [Column(Name = "DATEEFFETAVNANNEEOBJ")]
        public Int16 DateEffetAvenantAnneeobj { get; set; }

        [Column(Name = "DATEMODIAVNJOUROBJ")]
        public Int16 DateModifAvenantJourobj { get; set; }
        [Column(Name = "DATEMODIAVNMOISOBJ")]
        public Int16 DateModifAvenantMoisobj { get; set; }
        [Column(Name = "DATEMODIAVNANNEEOBJ")]
        public Int16 DateModifAvenantAnneeobj { get; set; }

        [Column(Name = "FINEFFETANNEE")]
        public Int16 DateFinEffetAnnee { get; set; }
        [Column(Name = "FINEFFETMOIS")]
        public Int16 DateFinEffetMois { get; set; }
        [Column(Name = "FINEFFETJOUR")]
        public Int16 DateFinEffetJour { get; set; }

        [Column(Name = "ECHEANCEANNEE")]
        public Int16 DateEcheanceAnnee { get; set; }
        [Column(Name = "ECHEANCEMOIS")]
        public Int16 DateEcheanceMois { get; set; }
        [Column(Name = "ECHEANCEJOUR")]
        public Int16 DateEcheanceJour { get; set; }

        [Column(Name = "INDICEORIGINE")]
        public double IndiceOrigine { get; set; }
        [Column(Name = "INDICEACTU")]
        public double IndiceActualise { get; set; }

        [Column(Name = "AVNCREATION")]
        public int AvnCreationObj { get; set; }

        [Column(Name = "AVNMODIF")]
        public int AvnModifObj { get; set; }
        
        [Column(Name = "REPORTVAL")]
        public string ReportVal { get; set; }

        [Column(Name = "ISRISQUETEMP")]
        public string IsRisqueTemporaire { get; set; }

    }
}
