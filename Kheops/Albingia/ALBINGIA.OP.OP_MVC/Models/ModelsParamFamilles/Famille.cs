using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParametreFamilles;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamFamilles
{
    public class Famille
    {
        public string ModeOperation { get; set; }
        [Display(Name = "Concept")]
        public string CodeConcept { get; set; }
        public string LibelleConcept { get; set; }

        [Display(Name = "Code famille*")]
        public string CodeFamille { get; set; }
        [Display(Name = "Description*")]
        public string LibelleFamille { get; set; }
        [Display(Name = "Longueur")]
        public int Longueur { get; set; }
        public List<AlbSelectListItem> Longueurs { get; set; }
        [Display(Name = "Type")]
        public string TypeCode { get; set; }
        [Display(Name = "Code valeur vide autorisé?")]
        public bool AcceptNullValue { get; set; }
        [Display(Name = "Num 1")]
        public string LibelleCourtNum1 { get; set; }
        public string LibelleLongNum1 { get; set; }
        public string TypeNum1 { get; set; }
        public List<AlbSelectListItem> TypesNum1 { get; set; }
        public int NbrDecimal1 { get; set; }
        public List<AlbSelectListItem> NbrDecimals1 { get; set; }
        [Display(Name = "Num 2")]
        public string LibelleCourtNum2 { get; set; }
        public string LibelleLongNum2 { get; set; }
        public string TypeNum2 { get; set; }
        public List<AlbSelectListItem> TypesNum2 { get; set; }
        public int NbrDecimal2 { get; set; }
        public List<AlbSelectListItem> NbrDecimals2 { get; set; }
        [Display(Name = "Alpha 1")]
        public string LibelleCourtAlpha1 { get; set; }
        public string LibelleLongAlpha1 { get; set; }
        [Display(Name = "Alpha 2")]
        public string LibelleCourtAlpha2 { get; set; }
        public string LibelleLongAlpha2 { get; set; }
        [Display(Name = "Restriction")]
        public string Restriction { get; set; }
        public List<AlbSelectListItem> Restrictions { get; set; }
        public List<Valeur> Valeurs { get; set; }
        public int nbvaleur { get; set; }

        public static explicit operator Famille(ParamFamilleDto familleDto)
        {
            var famille = ObjectMapperManager.DefaultInstance.GetMapper<ParamFamilleDto, Famille>().Map(familleDto);
            famille.AcceptNullValue = familleDto.NullValue == "O" ? true : false;
            return famille;
        }
    }
}