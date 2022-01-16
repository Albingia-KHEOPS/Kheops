using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    public class ParamAjoutEtape
    {
        public string CodeService { get; set; }
        [Display(Name="Service")]
        public string LibelleService { get; set; }
        public string CodeActGes { get; set; }
        [Display(Name="Acte de gestion")]
        public string LibelleActGes { get; set; }
        public string Etape { get; set; }
        public List<AlbSelectListItem> Etapes { get; set; }
        [Display(Name="Ordre")]
        public Int64 NumOrd { get; set; }

        public static explicit operator ParamAjoutEtape(ParamAjoutEtapeDto paramAjoutEtapeDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamAjoutEtapeDto, ParamAjoutEtape>().Map(paramAjoutEtapeDto);
        }

        public static ParamAjoutEtapeDto LoadDto(ParamAjoutEtape modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamAjoutEtape, ParamAjoutEtapeDto>().Map(modele);
        }
    }
}