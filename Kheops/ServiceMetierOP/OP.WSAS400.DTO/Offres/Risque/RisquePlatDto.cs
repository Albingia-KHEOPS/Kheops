using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using OP.WSAS400.DTO.MatriceFormule;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Offres.Risque
{
    public class RisquePlatDto
    {
        [Column(Name = "CODE")]
        public int Code { get; set; }
        [Column(Name = "DESCRIPTIF")]
        public String Descriptif { get; set; }
        [Column(Name = "CODECIBLE")]
        public String CodeCible { get; set; }
        [Column(Name = "LIBELLECIBLE")]
        public String LibelleCible { get; set; }
        [Column(Name = "LIBELLERISQUE")]
        public String Designation { get; set; }
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
        [Column(Name = "CODEOBJET")]
        public int CodeObjet { get; set; }
        [Column(Name = "ISRISQUEINDEXE")]
        public string IsRisqueIndexe { get; set; }
        [Column(Name = "INDEXOFFRE")]
        public string OffreIndexe { get; set; }
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
        [Column(Name = "CHRONODESI")]
        public Int64 ChronoDesi { get; set; }
        public string ReportValeur { get; set; }
        [DataMember]
        public string ReportObligatoire { get; set; }
        public Int32 NumeroChronoMatrice { get; set; }
        public string Formule { get; set; }
        public bool isIndexe { get; set; }
        public bool hasInventaire { get; set; }
        public bool isAffecte { get; set; }
        public string CodeAlpha { get; set; }
        public string Flag { get; set; }
        public AdressePlatDto AdresseRisque { get; set; }
        [Column(Name = "IDADRESSERISQUE")]
        public int IdAdresseRisque { get; set; }
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
        [Column(Name = "CODETRE")]
        public string CodeTre { get; set; }
        [Column(Name = "LIBTRE")]
        public string LibTre { get; set; }
        [Column(Name = "CODECLASSE")]
        public string CodeClasse { get; set; }
        [Column(Name = "TERRITORIALITE")]
        public string Territorialite { get; set; }

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

        [Column(Name = "EFFETAVNANNEE")]
        public Int16 DateEffetAvnAnnee { get; set; }
        [Column(Name = "EFFETAVNMOIS")]
        public Int16 DateEffetAvnMois { get; set; }
        [Column(Name = "EFFETAVNJOUR")]
        public Int16 DateEffetAvnJour { get; set; }

        [Column(Name = "AVNCREATION")]
        public int AvnCreationRsq { get; set; }

        [Column(Name = "AVNMODIF")]
        public int AvnModifRsq { get; set; }

        [Column(Name = "TAUXAPPEL")]
        public int TauxAppel { get; set; }

        [Column(Name = "ISREGUL")]
        public string IsRegularisable { get; set; }

        [Column(Name = "TYPEREGUL")]
        public string TypeRegularisation { get; set; }

        [Column(Name = "PARTBENEFRSQ")]
        public string PartBenefRsq { get; set; }
        [Column(Name = "PARTBENEF")]
        public string PartBenef { get; set; }
        [Column(Name = "NBYEAR")]
        public int NbYear { get; set; }
        [Column(Name="RISTOURNE")]
        public int Ristourne { get; set; }
        [Column(Name="COTISRET")]
        public int CotisRetenue { get; set; }
        [Column(Name="SEUIL")]
        public int Seuil { get; set; }
        [Column(Name="TAUXCOMP")]
        public int TauxCompl { get; set; }
        [Column(Name = "ISRISQUETEMP")]      
        public string IsRisqueTemporaire {get;set;}

        [Column(Name ="TAUXMAXI")]
        public Single TauxMaxi { get; set; }
        [Column(Name ="PRIMEMAXI")]
        public double PrimeMaxi { get; set; }

        [Column(Name = "ECHANNEE")]
        public int EchAnnee { get; set; }
        [Column(Name = "ECHMOIS")]
        public int EchMois { get; set; }
        [Column(Name = "ECHJOUR")]
        public int EchJour { get; set; }

        [Column(Name = "CODEBRANCHE")]
        public String CodeBranche { get; set; }

        //JDPBT TAUXAPPEL, JEPBN PARTBENEFRSQ, JDPBN PARTBENEF, JDPBA NBYEAR, JDPBR RISTOURNE, JDPBP COTISRET, JDPBS SEUIL, JDPBC TAUXCOMP

    }
}
