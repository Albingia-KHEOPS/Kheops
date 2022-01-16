using ALBINGIA.Framework.Common.Constants;
using System;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.Rercherchesaisie
{
    [DataContract]
    public class ModeleParametresRechercheDto
    {
        [DataMember]
        public string CodeOffre { get; set; }

        [DataMember]
        public string NumAliment { get; set; }

        [DataMember]
        public int CabinetCourtageId { get; set; }
        [DataMember]
        public string CabinetCourtageNom { get; set; }
        [DataMember]
        public bool CabinetCourtageIsApporteur { get; set; }
        [DataMember]
        public bool CabinetCourtageIsGestionnaire { get; set; }
        
        [DataMember]
        public int PreneurAssuranceCode { get; set; }
        [DataMember]
        public string PreneurAssuranceNom { get; set; }
        [DataMember]
        public string PreneurAssuranceCP { get; set; }
        [DataMember]
        public string PreneurAssuranceDEP { get; set; }
        [DataMember]
        public string PreneurAssuranceVille { get; set; }
        [DataMember]
        public int PreneurAssuranceSIREN { get; set; }
        [DataMember]
        public string AdresseRisqueVoie { get; set; }
        [DataMember]
        public string AdresseRisqueCP { get; set; }
        [DataMember]
        public string AdresseRisqueVille { get; set; }
        [DataMember]
        public string MotsClefs { get; set; }      
        [DataMember]
        public string SouscripteurCode { get; set; }
        [DataMember]
        public string SouscripteurNom { get; set; }      
        [DataMember]
        public string GestionnaireCode { get; set; }
        [DataMember]
        public string GestionnaireNom { get; set; }
        [DataMember]
        public string DateDebut { get; set; }
        [DataMember]
        public string DateFin { get; set; }
        [DataMember]
        public string Branche { get; set; }
        [DataMember]
        public string Etat { get; set; }
        [DataMember]
        public string Situation { get; set; }
        [DataMember]
        public string Sorting { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public string CritereParam { get; set; }
        [DataMember]
        public string Cible { get; set; }

        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public AlbConstantesMetiers.TypeDateRecherche TypeDateRecherche { get; set; }
        [DataMember]
        public DateTime? DDateDebut { get; set; }
        [DataMember]
        public DateTime? DDateFin { get; set; }
        [DataMember]
        public string SortingName { get; set; }
        [DataMember]
        public string SortingOrder { get; set; }
        [DataMember]
        public int StartLine { get; set; }
        [DataMember]
        public int EndLine { get; set; }
        [DataMember]
        public int LineCount { get; set; }

        [DataMember]
        public bool IsActif { get; set; }
        [DataMember]
        public bool IsInactif { get; set; }
        [DataMember]
        public bool SaufEtat { get; set; }
        [DataMember]
        public bool IsTemplate { get; set; }
        [DataMember]
        public string TypeContrat { get; set; }
        [DataMember]
        public (string ipb, int alx)[] ExcludedCodeOffres { get; set; }
    }
}
