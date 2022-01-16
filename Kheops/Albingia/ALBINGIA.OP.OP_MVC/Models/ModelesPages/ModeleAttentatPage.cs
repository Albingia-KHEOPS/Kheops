using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleAttentat;
using EmitMapper;
using OP.WSAS400.DTO.AttentatGareat;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleAttentatPage : MetaModelsBase
    {
        [Display(Name = "LCI (€)")]
        public string LCI { get; set; }
        [Display(Name = "Capitaux (€)")]
        public string Capitaux { get; set; }
        [Display(Name = "Surface (m²)")]
        public string Surface { get; set; }
        [Display(Name = "Base CATNAT")]
        public string CATNAT { get; set; }
        [Display(Name = "Capitaux forcés (€)")]
        public string CapitauxForces { get; set; }

        public string LibelleConstruit { get; set; }

        public ModeleAttentatParametre ParamStandard { get; set; }
        public ModeleAttentatParametre ParamRetenus { get; set; }


        [Display(Name = "Commentaires")]
        public string CommentForce { get; set; }
        public bool MontantForce { get; set; }

        public ModeleAttentatPage()
        {
            this.ParamStandard = new ModeleAttentatParametre();
            this.ParamRetenus = new ModeleAttentatParametre();
        }

        public static explicit operator ModeleAttentatPage(AttentatGareatDto modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AttentatGareatDto, ModeleAttentatPage>().Map(modele);
        }

        public static AttentatGareatDto LoadDto(ModeleAttentatPage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAttentatPage, AttentatGareatDto>().Map(modele);
        }

    }
}