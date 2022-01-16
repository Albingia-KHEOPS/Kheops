using ALBINGIA.OP.OP_MVC.Models.FormuleGarantie;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle;
using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleCreationAffaireNouvellePage : MetaModelsBase
    {
        [Display(Name = "N° offre / Version")]
        public string CodeOffre { get; set; }
        [Display(Name = "Version")]
        public Int32 Version { get; set; }
        [Display(Name = "Date de Saisie")]
        public string DateSaisie { get; set; }
        [Display(Name = "Branche / Cible")]
        public string CodeBranche { get; set; }
        public string LibBranche { get; set; }
        [Display(Name = "Cible")]
        public string CodeCible { get; set; }
        public string LibCible { get; set; }
        [Display(Name = "Devise")]
        public string CodeDevise { get; set; }
        public string LibDevise { get; set; }
        [Display(Name = "Identification")]
        public string Identification { get; set; }
        [Display(Name = "Courtier")]
        public Int32 CodeCourtier { get; set; }
        public string NomCourtier { get; set; }
        [Display(Name = "Assuré")]
        public Int32 CodeAssure { get; set; }
        public string NomAssure { get; set; }
        [Display(Name = "Nature de contrat")]
        public string CodeNatureContrat { get; set; }
        public string LibNatureContrat { get; set; }
        [Display(Name = "Souscripteur")]
        public string Souscripteur { get; set; }
        [Display(Name = "Gestionnaire")]
        public string Gestionnaire { get; set; }
        public string Observation { get; set; }
        public bool PossedeUnContratEnCours { get; set; }
        public string DateDuJour { get; set; }

        public List<ModeleCreationAffaireNouvelleContrat> Contrats;

        public string DateEffet { get; set; }
        public string HeureEffet { get; set; }
        public ModeleRecherchePage Recherche { get; set; }

        public SelectionRisquesObjets SelectionRisquesObjets { get; set; }

        public NouvelleAffaire NouvelleAffaire { get; set; }


        public static explicit operator ModeleCreationAffaireNouvellePage(CreationAffaireNouvelleDto creationAffNouvDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CreationAffaireNouvelleDto, ModeleCreationAffaireNouvellePage>().Map(creationAffNouvDto);
        }

        public static CreationAffaireNouvelleDto LoadDto(ModeleCreationAffaireNouvellePage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleCreationAffaireNouvellePage, CreationAffaireNouvelleDto>().Map(modele);
        }

    }
}
