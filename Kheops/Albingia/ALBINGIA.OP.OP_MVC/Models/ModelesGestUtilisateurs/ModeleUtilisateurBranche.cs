using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.GestUtilisateurs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGestUtilisateurs
{
    public class ModeleUtilisateurBranche
    {
        public string Albcontext { get; set; }

        [Display(Name = "Utilisateur")]
        public string Utilisateur { get; set; }

        [Display(Name = "Branche")]
        public string Branche { get; set; }

        [Display(Name = "Type de droit")]
        public string TypeDroit { get; set; }

        public string TypeDroitLabel { get; set; }
        public string BrancheLabel { get; set; }

        public List<AlbSelectListItem> TypeDroits { get; set; }

        public bool ExistInCache { get; set; }

        public static explicit operator ModeleUtilisateurBranche(UtilisateurBrancheDto utiliDroitBrancheDto)
        {
            return new ModeleUtilisateurBranche
            {
                Utilisateur = utiliDroitBrancheDto.Utilisateur,
                Branche = utiliDroitBrancheDto.Branche,
                TypeDroit = utiliDroitBrancheDto.TypeDroit
            };
        }


        public UtilisateurBrancheDto ToUtilisateurBrancheDto()
        {
            return new UtilisateurBrancheDto
            {
                Utilisateur = this.Utilisateur,
                Branche = this.Branche,
                TypeDroit = this.TypeDroit
            };
        }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(this.Utilisateur) ||
                string.IsNullOrWhiteSpace(this.Utilisateur) )
            {
                return false;
            }

            if (string.IsNullOrEmpty(this.Branche) ||
                string.IsNullOrWhiteSpace(this.Branche))
            {
                return false;
            }

            if (string.IsNullOrEmpty(this.TypeDroit) ||
                string.IsNullOrWhiteSpace(this.TypeDroit))
            {
                return false;
            }

            return true;
        }
    }
}