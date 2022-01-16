using EmitMapper;
using OP.WSAS400.DTO.AttentatGareat;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleAttentat
{
    [Serializable]
    public class ModeleAttentatParametre
    {
        public bool Standard { get; set; }
        [Display(Name="Soumis")]
        public bool Soumis { get; set; }
        [Display(Name = "Tranche")]
        public string Tranche { get; set; }
        [Display(Name = "Taux cession (%)")]
        public string TauxCession { get; set; }
        [Display(Name = "Frais de ges. (%)")]
        public string FraisGestion { get; set; }
        [Display(Name = "Commission (%)")]
        public string Commission { get; set; }
        [Display(Name = "Facturé (%)")]
        public string Facture { get; set; }

        public ModeleAttentatParametre()
        {
            this.Standard = true;
        }

        public static explicit operator ModeleAttentatParametre(AttentatParametreDto modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<AttentatParametreDto, ModeleAttentatParametre>().Map(modele);
        }

        public static AttentatParametreDto LoadDto(ModeleAttentatParametre modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAttentatParametre, AttentatParametreDto>().Map(modele);
        }
    }
}