using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParametreFamilles;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamFamilles
{
    public class Valeur
    {
        public string ModeOperation { get; set; }
        public string AdditionalParam { get; set; }
        public string RestrictionParam { get; set; }

        [Display(Name = "Concept")]
        public string CodeConcept { get; set; }
        public string LibelleConcept { get; set; }

        [Display(Name = "Famille")]
        public string CodeFamille { get; set; }
        public string LibelleFamille { get; set; }
        public int Longueur { get; set; }
        public string TypeCode { get; set; }      
        [Display(Name = "Code*")]
        public string CodeValeur { get; set; }
        public string LibelleValeur { get; set; }
        public string LibelleLongValeur { get; set; }
        [Display(Name = "Description")]
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }   

        [Display(Name = "Num 1")]
        public string LibelleLongNum1 { get; set; }
        public string TypeNum1 { get; set; }
        public double ValeurNum1 { get; set; }

        [Display(Name = "Num 2")]
        public string LibelleLongNum2 { get; set; }
        public string TypeNum2 { get; set; }
        public double ValeurNum2 { get; set; }

        [Display(Name = "Alpha 1")]
        public string LibelleLongAlpha1 { get; set; }
        public string ValeurAlpha1 { get; set; }

        [Display(Name = "Alpha 2")]
        public string LibelleLongAlpha2 { get; set; }
        public string ValeurAlpha2 { get; set; }

        [Display(Name = "Filtre")]
        public string CodeFiltre { get; set; }
        public string LibelleFiltre { get; set; }
        public List<AlbSelectListItem> Filtres { get; set; }

        [Display(Name = "Restriction")]
        public string Restriction { get; set; }
        public List<AlbSelectListItem> Restrictions { get; set; }
        public static explicit operator Valeur(ParamValeurDto valeurDto)
        {
            var valeur = ObjectMapperManager.DefaultInstance.GetMapper<ParamValeurDto, Valeur>().Map(valeurDto);           
            return valeur;
        }
    }
}