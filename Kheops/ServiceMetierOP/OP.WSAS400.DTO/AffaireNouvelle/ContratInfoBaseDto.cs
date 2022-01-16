using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;


namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class ContratInfoBaseDto
    {
        [DataMember]
        public bool CopyMode { get; set; }
        [DataMember]
        public string CodeContratCopy { get; set; }
        [DataMember]
        public string VersionCopy { get; set; }
        [DataMember]
        public bool TemplateMode { get; set; }
        [DataMember]
        [Column(Name = "CODECONTRAT")]
        public string CodeContrat { get; set; }
        [DataMember]
        [Column(Name = "VERSCONTRAT")]
        public Int64 VersionContrat { get; set; }

        [DataMember]
        [Column(Name = "TYPECONTRAT")]
        public string TypeContrat { get; set; }
        [DataMember]
        public List<ParametreDto> TypesContrat { get; set; }

        [DataMember]
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }
        [DataMember]
        public List<ParametreDto> Branches { get; set; }

        [DataMember]
        [Column(Name = "CIBLE")]
        public string Cible { get; set; }
        [DataMember]
        [Column(Name = "TYPEPOLICE")]
        public string TypePolice { get; set; }
        [DataMember]
        [Column(Name = "SOUSCRIPTEURCODE")]
        public String SouscripteurCode { get; set; }
        [DataMember]
        [Column(Name = "SOUSCRIPTEURNOM")]
        public String SouscripteurNom { get; set; }

        [DataMember]
        [Column(Name = "GESTIONNAIRECODE")]
        public String GestionnaireCode { get; set; }
        [DataMember]
        [Column(Name = "GESTIONNAIRENOM")]
        public String GestionnaireNom { get; set; }

        [DataMember]
        [Column(Name = "DATEACCORDA")]
        public Int16 DateAccordAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEACCORDM")]
        public Int16 DateAccordMois { get; set; }
        [DataMember]
        [Column(Name = "DATEACCORDJ")]
        public Int16 DateAccordJour { get; set; }

        [DataMember]
        public String ContratMere { get; set; }

        [DataMember]
        public String NumAliment { get; set; }
        [DataMember]
        public String NumAlimentRemplace { get; set; }

        [DataMember]
        public int NumChronoConx { get; set; }
        [DataMember]
        public bool ContratRemplace { get; set; }
        [DataMember]
        public String NumContratRemplace { get; set; }

        [DataMember]
        [Column(Name = "COURTIERAPPORTEUR")]
        public int CourtierApporteur { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERAPPO")]
        public string NomCourtierAppo { get; set; }

        [DataMember]
        [Column(Name = "COURTIERGESTIONNAIRE")]
        public int CourtierGestionnaire { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERGEST")]
        public string NomCourtierGest { get; set; }

        [DataMember]
        [Column(Name = "COURTIERPAYEUR")]
        public int CourtierPayeur { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERPAYEUR")]
        public string NomCourtierPayeur { get; set; }

        [DataMember]
        [Column(Name = "CODEINTERLOCUTEUR")]
        public int CodeInterlocuteur { get; set; }
        [DataMember]
        [Column(Name = "NOMINTERLOCUTEUR")]
        public string NomInterlocuteur { get; set; }

        [DataMember]
        [Column(Name = "REFCOURTIER")]
        public string RefCourtier { get; set; }

        [DataMember]
        [Column(Name = "NOMPRENASSUR")]
        public string NomPreneurAssurance { get; set; }
        [DataMember]
        [Column(Name = "CODEPRENASSUR")]
        public int CodePreneurAssurance { get; set; }
        [DataMember]
        public bool PreneurEstAssure { get; set; }

        [DataMember]
        [Column(Name = "DEPASSUR")]
        public string DepAssurance { get; set; }
        [DataMember]
        [Column(Name = "VILLEASSUR")]
        public string VilleAssurance { get; set; }
        [DataMember]
        [Column(Name = "PERIODICITECODE")]
        public string PeriodiciteCode { get; set; }
        [DataMember]
        [Column(Name = "PERIODICITENOM")]
        public string PeriodiciteNom { get; set; }
        [DataMember]
        [Column(Name = "ECHJOUR")]
        public Int16 Jour { get; set; }
        [DataMember]
        [Column(Name = "ECHMOIS")]
        public Int16 Mois { get; set; }
        [DataMember]
        [Column(Name = "CODEMOTSCLEF1")]
        public string CodeMotsClef1 { get; set; }
        [DataMember]
        [Column(Name = "CODEMOTSCLEF2")]
        public string CodeMotsClef2 { get; set; }
        [DataMember]
        [Column(Name = "CODEMOTSCLEF3")]
        public string CodeMotsClef3 { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTIF")]
        public string Descriptif { get; set; }
        [DataMember]
        [Column(Name = "OBSERVATIONS")]
        public string Obersvations { get; set; }
        [DataMember]
        public Int64 NumChronoObsv { get; set; }

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
        [Column(Name = "DATEEFFETJH")]
        public int DateEffetHeure { get; set; }

        [DataMember]
        [Column(Name = "FINEFFETA")]
        public Int16 FinEffetAnnee { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETM")]
        public Int16 FinEffetMois { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETJ")]
        public Int16 FinEffetJour { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETJH")]
        public int FinEffetHeure { get; set; }

        [DataMember]
        [Column(Name = "ENCAISSEMENT")]
        public string Encaissement { get; set; }
        [DataMember]
        [Column(Name = "ETAT")]
        public string Etat { get; set; }
        [DataMember]
        [Column(Name = "SITUATION")]
        public string Situation { get; set; }
        [DataMember]
        public List<ParametreDto> Encaissements { get; set; }
        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        [DataMember]
        public RisqueDto Risque { get; set; }
        [DataMember]
        public List<RisqueDto> Risques { get; set; }
        //[DataMember]
        //public ElementAssureContratDto RisquePrincipal { get; set; }
        [DataMember]
        public AdressePlatDto AdresseContrat { get; set; }
        [DataMember]
        public List<ElementAssureContratDto> ElementAssures { get; set; }

        [DataMember]
        public string BrancheRemp { get; set; }

        [DataMember]
        public string Antecedent { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsModifHorsAvn { get; set; }

        [DataMember]
        public string NumAvenant { get; set; }
        [DataMember]
        public string TypeAvt { get; set; }
        [DataMember]
        public Int16 AnneeEffetAvenant { get; set; }
        [DataMember]
        public Int16 MoisEffetAvenant { get; set; }
        [DataMember]
        public Int16 JourEffetAvenant { get; set; }
        [DataMember]
        public int HeureEffetAvenant { get; set; }
    }
}
