using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.ParametreTypeValeur;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamTypesValeur
{
    public class ModeleLigneTypeValeurCompatible
    {
        public Int64 GuidId { get; set; }
        public string AdditionalParam { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public List<AlbSelectListItem> ListeTypesCompatibles { get; set; }

        public static explicit operator ModeleLigneTypeValeurCompatible(ModeleLigneTypeValeurCompatibleDto dataDto)
        {
            var toReturn = new ModeleLigneTypeValeurCompatible
            {
                Code = dataDto.CodeTypeValeurComp,
                Description = dataDto.DescriptionTypeValeurComp,
                GuidId = dataDto.GuidId
            };
            return toReturn;
        }
    }
}