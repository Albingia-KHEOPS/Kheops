using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.GestionNomenclature;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGestionNomenclature
{
    public class ModeleSelectionValeurs
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public List<ModeleTypologie> Typologies { get; set; }
        public string Filtre { get; set; }
        public List<AlbSelectListItem> Filtres { get; set; }
        public List<ModeleNomenclature> Nomenclatures { get; set; }
        public string Typologie { get; set; }
        public string Niveau { get; set; }
        public string Lien { get; set; }

        public static explicit operator ModeleSelectionValeurs(GrilleDto modeleDto)
        {
            var model = ObjectMapperManager.DefaultInstance.GetMapper<GrilleDto, ModeleSelectionValeurs>().Map(modeleDto);

            if (modeleDto.Typologies != null && modeleDto.Typologies.Any())
            {
                model.Typologies.Clear();
                modeleDto.Typologies.ToList().ForEach(
                       el => model.Typologies.Add((ModeleTypologie)el));
            }
            return model;
        }

        public static GrilleDto LoadDto(ModeleSelectionValeurs modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleSelectionValeurs, GrilleDto>().Map(modele);
        }
    }
}