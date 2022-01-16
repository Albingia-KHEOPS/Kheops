using EmitMapper;
using OP.WSAS400.DTO.GestionNomenclature;
//using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGestionNomenclature
{
    public class ModeleNomenclature
    {
        public Int32 Id { get; set; }
        public Int32 Ordre { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public BrancheDto Branche { get; set; }
        public ParametreDto Typologie { get; set; }

        public List<ModeleGrille> Grilles { get; set; }
        public Int32 IdValeur { get; set; }

        public static explicit operator ModeleNomenclature(NomenclatureDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<NomenclatureDto, ModeleNomenclature>().Map(modeleDto);
        }

        public static NomenclatureDto LoadDto(ModeleNomenclature modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleNomenclature, NomenclatureDto>().Map(modele);
        }

    }
}