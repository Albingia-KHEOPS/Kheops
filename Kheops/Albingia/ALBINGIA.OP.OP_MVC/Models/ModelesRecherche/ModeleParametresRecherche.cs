using ALBINGIA.Framework.Common.Constants;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesRecherche
{
    public class ModeleParametresRecherche
    {
        public string CodeOffre { get; set; }
        public string NumAliment { get; set; }
        public bool CheckOffre { get; set; }
        public bool CheckContrat { get; set; }
        public bool CheckAliment { get; set; }

        public int CabinetCourtageId { get; set; }
        public string CabinetCourtageNom { get; set; }
        public bool CabinetCourtageIsApporteur { get; set; }
        public bool CabinetCourtageIsGestionnaire { get; set; }

        public int PreneurAssuranceCode { get; set; }
        public string PreneurAssuranceNom { get; set; }
        public string PreneurAssuranceCP { get; set; }
        //public string PreneurAssuranceDEP { get; set; }
        public string PreneurAssuranceVille { get; set; }
        public int PreneurAssuranceSIREN { get; set; }
        public string AdresseRisqueVoie { get; set; }
        public string AdresseRisqueCP { get; set; }
        public string AdresseRisqueVille { get; set; }
        public string MotsClefs { get; set; }    
        public string SouscripteurCode { get; set; }
        public string SouscripteurNom { get; set; }      
        public string GestionnaireCode { get; set; }
        public string GestionnaireNom { get; set; }
        //public string DateType { get; set; }
        public string DateDebut { get; set; }
        public string DateFin { get; set; }
        public string Branche { get; set; }
        public string Etat { get; set; }
        public string Situation { get; set; }
        public string Sorting { get; set; }
        public int PageNumber { get; set; }
        public AlbConstantesMetiers.CriterParam CritereParam { get; set; }
        public string Cible { get; set; }

        public string Type { get; set; }
        public AlbConstantesMetiers.TypeDateRecherche TypeDateRecherche { get; set; }
        public DateTime? DDateDebut { get; set; }
        public DateTime? DDateFin { get; set; }
        public string SortingName { get; set; }
        public string SortingOrder { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public int LineCount { get; set; }

        public bool IsActif { get; set; }
        public bool IsInactif { get; set; }
        public bool SaufEtat { get; set; }
        public bool IsTemplate { get; set; }   

        public string ModeNavig { get; set; }
        public AlbConstantesMetiers.TypeAccesRecherche AccesRecherche { get; set; }
        public bool SessionSearch { get; set; }
        /// <summary>
        /// Mere, aliment etc.
        /// </summary>
        public string TypeContrat { get; set; }
    }
}