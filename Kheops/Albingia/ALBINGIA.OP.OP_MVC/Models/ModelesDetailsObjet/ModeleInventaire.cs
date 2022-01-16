using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesInventaire;
using EmitMapper;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDetailsObjet
{
    public class ModeleInventaire
    {
        //public FileDescriptions FileDescriptionsMetaData { get; set; }

        public Int64 Code { get; set; }
        public string Description { get; set; }
        public string Descriptif { get; set; }
        public Int64 InventaireType { get; set; }
        public List<ModeleInventaireGridRow> InventaireInfos { get; set; }

        public string CodeRisque { get; set; }
        public string CodeObjet { get; set; }
        public string Type { get; set; }

        public string DateDebStr { get; set; }
        public string HeureDebStr { get; set; }
        public string MinuteDebStr { get; set; }
        public string DateFinStr { get; set; }
        public string HeureFinStr { get; set; }
        public string MinuteFinStr { get; set; }

        public string NumDescription { get; set; }

        public bool FullScreen { get; set; }

        public string Valeur { get; set; }
        public string UniteLst { get; set; }
        public List<AlbSelectListItem> UniteLsts { get; set; }
        public string TypeLst { get; set; }
        public List<AlbSelectListItem> TypeLsts { get; set; }
        [Display(Name = "Activer report")]
        public bool ActiverReport { get; set; }

        public static explicit operator ModeleInventaire(InventaireDto inventaireDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<InventaireDto, ModeleInventaire>().Map(inventaireDto);
        }

        public static InventaireDto LoadDto(ModeleInventaire modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleInventaire, InventaireDto>().Map(modele);
        }

        public ModeleInventaire Load(InventaireDto inventaire)
        {
            var toReturn = new ModeleInventaire
            {
                Code = inventaire.Code,
                Descriptif = inventaire.Descriptif ?? string.Empty,
                Description = inventaire.Description ?? string.Empty,
                Type = inventaire.Type.Code,
                NumDescription = inventaire.NumDescription ?? string.Empty
            };


            return toReturn;
        }
    }
}