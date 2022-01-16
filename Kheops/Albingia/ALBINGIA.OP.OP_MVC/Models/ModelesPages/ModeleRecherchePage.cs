using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesRecherche;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleRecherchePage : MetaModelsBase
    {
        #region Gestion du mode de recherche

        public AlbConstantesMetiers.CriterParam CritereParam { get; set; }
        public string ProvenanceParam { get; set; }
        public string SituationParam { get; set; }
        /// <summary>
        /// Contexte de l'écran de recherche (par exemple, connexité, création contrat etc.)
        /// </summary>
        public string AlbEmplacement { get; set; }

        #endregion

        [Display(Name = "Offre")]
        public bool CheckOffre { get; set; }
        [Display(Name = "Contrat")]
        public bool CheckContrat { get; set; }        
        [Display(Name = "N°")]
        public string OffreId { get; set; }
        [Display(Name = "N°")]
        public string CabinetCourtageId { get; set; }
        [Display(Name = "Nom")]
        public string CabinetCourtageNom { get; set; }
        [Display(Name = "Code")]
        public string PreneurAssuranceId { get; set; }
        [Display(Name = "Nom")]
        public string PreneurAssuranceNom { get; set; }
        [Display(Name = "C.P.")]
        public string PreneurAssuranceCP { get; set; }
        [Display(Name = "Ville")]
        public string PreneurAssuranceVille { get; set; }
        [Display(Name = "SIREN")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Le SIREN doit etre numérique")]
        public string PreneurAssuranceSIREN { get; set; }
        [Display(Name = "Bât. / Voie / Dist.")]
        public string AdresseRisqueVoie { get; set; }
        [Display(Name = "Dép. / C.P.")]
        public string AdresseRisqueCP { get; set; }
        [Display(Name = "Ville")]
        public string AdresseRisqueVille { get; set; }
        [Display(Name = "Recherche")]
        public string MotsClefs { get; set; }
        [Display(Name = "Souscripteur")]
        public string Souscripteur { get; set; }
        public string SouscripteurCode { get; set; }
        public string SouscripteurNom { get; set; }
        public IEnumerable<AlbSelectListItem> Souscripteurs { get; set; }
        [Display(Name = "Gestionnaire")]
        public string Gestionnaire { get; set; }
        public IEnumerable<AlbSelectListItem> Gestionnaires { get; set; }
        public string GestionnaireCode { get; set; }
        public string GestionnaireNom { get; set; }
        [Display(Name = "Date de")]
        public string DateType { get; set; }
        public List<AlbSelectListItem> DateTypes { get; set; }
        [Display(Name = "Entre le")]
        public Nullable<DateTime> DateDebut { get; set; }
        [Display(Name = "et le")]
        public Nullable<DateTime> DateFin { get; set; }
        [Display(Name = "Branche/Cible")]
        public string Branche { get; set; }
        public List<AlbSelectListItem> Branches { get; set; }
        public ModeleListeCibles ModeleCibles { get; set; }
        [Display(Name = "Etat")]
        public string Etat { get; set; }
        public List<AlbSelectListItem> Etats { get; set; }
        [Display(Name = "Situation")]
        public string Situation { get; set; }
        public List<AlbSelectListItem> Situations { get; set; }
        public ModeleResultRecherche ListOffreContrat { get; set; }
        public ModeleRechercheAvancee ListCabinetCourtage { get; set; }
        public ModeleRechercheAvancee ListPreneurAssurance { get; set; }

        public string Refus { get; set; }
        public List<AlbSelectListItem> ListRefus { get; set; }
        public bool SearchInTemplate { get; set; }

        public bool IsCheckOffre { get; set; }
        public bool IsCheckContrat { get; set; }
        public bool IsCheckEnCours { get; set; }
        public bool IsCheckInactif { get; set; }
        public bool IsCheckEtat { get; set; }
        public bool IsCheckApporteur { get; set; }
        public bool IsCheckGestionnaire { get; set; }

        public AlbConstantesMetiers.TypeAccesRecherche AccesRecherche { get; set; }
        public bool SessionSearch { get; set; }
        public ModelRelances ListeRelances { get; internal set; }
    }
}