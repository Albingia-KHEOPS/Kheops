using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.GestionNomenclature;
using OP.WSAS400.DTO.Offres.Branches;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGestionNomenclature
{
    public class ModeleGrille
    {
        public Int32 Id { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string TypologieGrille { get; set; }
        public List<ModeleTypologie> Typologies { get; set; }
        public string Typologie { get; set; }
        public List<AlbSelectListItem> LstTypologie { get; set; }
        public string Lien { get; set; }
        public List<AlbSelectListItem> LstLien { get; set; }
        public bool CibleLiee { get; set; }
        public List<CibleDto> Cibles { get; set; }

        public static explicit operator ModeleGrille(GrilleDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<GrilleDto, ModeleGrille>().Map(modeleDto);
        }

        public static GrilleDto LoadDto(ModeleGrille modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleGrille, GrilleDto>().Map(modele);
        }

    }
}