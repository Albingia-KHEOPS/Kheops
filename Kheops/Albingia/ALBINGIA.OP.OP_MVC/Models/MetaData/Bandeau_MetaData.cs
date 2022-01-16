using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.MetaData
{
    [Serializable]
    public class Bandeau_MetaData : ModelBase
    {
        public string ContexteBandeau { get; set; }

        public string StyleBandeau { get; set; }
        /// <summary>
        /// Gets or sets the id offre.
        /// </summary>
        /// <value>
        /// The id offre.
        /// </value>
        [Display(Name = "N°")]
        public string IdOffre { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }

        [Display(Name = "le")]
        public string DateSaisie { get; set; }

        [Display(Name = "Br")]
        public string Branche { get; set; }
        public string LibelleBranche { get; set; }

        public string SousBranche { get; set; }

        [Display(Name = "Délégation")]
        public string Delegation { get; set; }

        [Display(Name = "Inspecteur")]
        public string Inspecteur { get; set; }

        [Display(Name = "Courtier apport.")]
        public string IdCourtier { get; set; }
        public string NomCourtier { get; set; }
        [Display(Name = "Courtier gestio.")]
        public string NomCourtierGestionnaire { get; set; }
        public string CodeCourtierGestionnaire { get; set; }
        public string VilleCourtierGestionnaire { get; set; }
        [Display(Name = "Courtier payeur")]
        public string NomCourtierPayeur { get; set; }
        public string CodeCourtierPayeur { get; set; }
        public string VilleCourtierPayeur { get; set; }

        public string CPCourtier { get; set; }
        [Display(Name = "Ville")]
        public string VilleCourtier { get; set; }

        [Display(Name = "Preneur Ass.")]
        public string IdAssure { get; set; }
        public string NomAssure { get; set; }
        public string CPAssure { get; set; }
        [Display(Name = "Ville")]
        public string VilleAssure { get; set; }

        [Display(Name = "Date de début d'effet")]
        public string DateDebEffet { get; set; }
        [Display(Name = "Date de fin d'effet")]
        public string DateFinEffet { get; set; }

        public string Cible { get; set; }
        public string LibelleCible { get; set; }

        [Display(Name = "Gestionnaire")]
        public string GestionnaireNom { get; set; }
        public string GestionnairePrenom { get; set; }
        public string GestionnaireCode { get; set; }
        [Display(Name = "Périodicité")]
        public string Periodicite { get; set; }
        [Display(Name = "Nature")]
        public string NatureContrat { get; set; }
        //[Display(Name = "Etat")]
        //public string Etat { get; set; }
        //public string Situation { get; set; }

        public bool HasDoubleSaisie { get; set; }
        [Display(Name = "Devise")]
        public string CodeDevise { get; set; }
        public string LibelleDevise { get; set; }
        public string LibelleNatureContrat { get; set; }
        [Display(Name = "Part")]
        public string Part { get; set; }
        [Display(Name = "Couverture")]
        public string Couverture { get; set; }
        [Display(Name = "Régime")]
        public string CodeRegime { get; set; }
        public string LibelleRegime { get; set; }
        [Display(Name = "CATNAT possible")]
        public string SoumisCatNat { get; set; }
        [Display(Name = "Secteur")]
        public string Secteur { get; set; }
        public string LibSecteur { get; set; }
        [Display(Name = "Souscripteur")]
        public string SouscripteurCode { get; set; }
        public string SouscripteurNom { get; set; }
        public string SouscripteurPrenom { get; set; }
        [Display(Name = "Montant de référence (€)")]
        public decimal MontantReference { get; set; }
        [Display(Name = "Indice")]
        public string CodeIndice { get; set; }
        public string LibelleIndice { get; set; }
        [Display(Name = "Valeur")]
        public decimal Valeur { get; set; }
        [Display(Name = "Indexation")]
        public string Indexation { get; set; }
        [Display(Name = "LCI")]
        public string LCI { get; set; }
        [Display(Name = "Assiette")]
        public string Assiette { get; set; }
        [Display(Name = "Franchise générale")]
        public string Franchise { get; set; }
        [Display(Name = "Dernière action")]
        public string CodeAction { get; set; }
        public string LibelleAction { get; set; }
        [Display(Name = "Etat")]
        public string CodeEtat { get; set; }
        public string LibelleEtat { get; set; }
        [Display(Name = "Situation")]
        public string CodeSituation { get; set; }
        public string LibelleSituation { get; set; }
        [Display(Name = "le")]
        public DateTime? DateSituation { get; set; }
        [Display(Name = "Créé le")]
        public DateTime? DateEnregistrement { get; set; }
        [Display(Name = "par")]
        public string CodeUsrCreateur { get; set; }
        public string NomUsrCreateur { get; set; }
        [Display(Name = "Modifié le")]
        public DateTime? DateMAJ { get; set; }
        [Display(Name = "par")]
        public string CodeUsrModificateur { get; set; }
        public string NomUsrModificateur { get; set; }
        [Display(Name = "N°")]
        public int NumAvenant { get; set; }
        [Display(Name = "A effet du")]
        public DateTime? DateEffetAvenant { get; set; }
        [Display(Name="N° Avn ext.")]
        public int NumExterne { get; set; }
        [Display(Name = "Offre d'origine")]
        public string CodeOffreOrigine { get; set; }
        public int VersionOffreOrigine { get; set; }
        [Display(Name = "Préavis")]
        public int Preavis { get; set; }
        [Display(Name = "Encaissement")]
        public string CodeEncaissement { get; set; }
        public string LibelleEncaissement { get; set; }
        [Display(Name = "Echéance principale")]
        public string EcheancePrincipale { get; set; }
        [Display(Name = "prochaine échéance")]
        public DateTime? ProchaineEcheance { get; set; }
        [Display(Name = "HT hors CATNAT (€)")]
        public Double HorsCatNat { get; set; }
        [Display(Name = "CATNAT")]
        public Double CatNat { get; set; }
        [Display(Name = "Taux (%)")]
        public Double TauxHorsCatNat { get; set; }
        [Display(Name = "Taux (%)")]
        public Double TauxCatNat { get; set; }
        [Display(Name = "Affaire nouvelle du")]
        public DateTime? DateaffaireNouvelle { get; set; }
        [Display(Name = "Montant statistique AN (€)")]
        public double MontantStatistique { get; set; }
        [Display(Name = "LCI Générale")]
        public Double LCIGenerale { get; set; }
        public string LCIGeneraleUnit { get; set; }
        public string LCIGeneraleType { get; set; }
        [Display(Name = "Franchise Générale")]
        public Double FranchiseGenerale { get; set; }
        public string FranchiseGeneraleUnit { get; set; }
        public string FranchiseGeneraleType { get; set; }
        [Display(Name = "Territo.")]
        public string Territorialite { get; set; }
        public string TerritorialiteLib { get; set; }
        [Display(Name = "Motif")]
        public string CodeMotif { get; set; }
        public string LibelleMotif { get; set; }
        public string Stop { get; set; }
        public string StopLib { get; set; }
        public string StopContentieux { get; set; }
        public string StopContentieuxLib { get; set; }
        [Display(Name = "Durée")]
        public int Duree { get; set; }
        public string DureeUnite { get; set; }
        public string DureeStr { get; set; }
        public bool Displaybloqueferme { get; set; }
        public string Origine { get; set; }
        public bool HasSusp { get; set; }
        public bool TauxAvailable { get; set; }
    }
}