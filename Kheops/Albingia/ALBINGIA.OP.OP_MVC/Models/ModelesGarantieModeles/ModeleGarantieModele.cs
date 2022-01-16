using EmitMapper;
using OP.WSAS400.DTO.GarantieModele;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles
{
    [Serializable]
    public class ModeleGarantieModele
    {
        [Display(Name = "Code modèle")]
        public string Code { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Date appli")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateAppli { get; set; }
        [Display(Name = "Typologie")]
        public string Typologie { get; set; }
        public bool ReadOnly { get; set; }
        public bool IsNew { get; set; }
        public string GuidId { get; set; }

        public List<ModeleGarantie> LstModeleGarantie { get; set; }

        public ModeleGarantieModele()
        {
            Code = string.Empty;
            Description = string.Empty;
            DateAppli = null;
            Typologie = string.Empty;
            GuidId = string.Empty;
            ReadOnly = false;
            LstModeleGarantie = new List<ModeleGarantie>();

        }

        public static explicit operator ModeleGarantieModele(GarantieModeleDto dtoGarantieModele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<GarantieModeleDto, ModeleGarantieModele>().Map(dtoGarantieModele);
        }

    }

    public class ModeleGarantie
    {
        public string Branche { get; set; }
        public string Cible { get; set; }
        public string Volet { get; set; }
        public string Bloc { get; set; }
        public string Typologie { get; set; }

        public static explicit operator ModeleGarantie(ModeleGarantieDto dtoGarantieModele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleGarantieDto, ModeleGarantie>().Map(dtoGarantieModele);
        }
    }
}