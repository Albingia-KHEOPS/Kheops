using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParametreInventaires;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamInventaires
{
    public class DetailsInventaire
    {
        public string Mode { get; set; }
        public Int32 GuidId { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public int Kagtmap { get; set; }

        public Int64 CodeFiltre { get; set; }
        public List<AlbSelectListItem> Filtres { get; set; }

        public static explicit operator DetailsInventaire(ParamInventairesDto paramInventairesDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamInventairesDto, DetailsInventaire>().Map(paramInventairesDto);
        }
    }
}