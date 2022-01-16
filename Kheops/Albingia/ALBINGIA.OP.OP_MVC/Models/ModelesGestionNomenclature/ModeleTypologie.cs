using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.GestionNomenclature;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGestionNomenclature
{
    public class ModeleTypologie
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string Lien { get; set; }
        public string ValeurFiltre { get; set; }
        public Int32 Niveau { get; set; }
        public List<AlbSelectListItem> Liens { get; set; }
        public string Valeur { get; set; }
        public List<AlbSelectListItem> Valeurs { get; set; }

        public static explicit operator ModeleTypologie(TypologieDto modeleDto)
        {
            var model = ObjectMapperManager.DefaultInstance.GetMapper<TypologieDto, ModeleTypologie>().Map(modeleDto);

            if (modeleDto.Valeurs != null && modeleDto.Valeurs.Any())
            {
                model.Valeurs.Clear();
                modeleDto.Valeurs.ToList().ForEach(
                        el => model.Valeurs.Add(AlbSelectListItem.ConvertToAlbSelect(el.LongId.ToString(), string.Format("{0} - {1}", el.Code, el.Libelle), string.Format("{0} - {1}", el.Code, el.Libelle)))
                    );
            }
            return model;//ObjectMapperManager.DefaultInstance.GetMapper<TypologieDto, ModeleTypologie>().Map(modeleDto);
        }

        public static TypologieDto LoadDto(ModeleTypologie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleTypologie, TypologieDto>().Map(modele);
        }
    }
}