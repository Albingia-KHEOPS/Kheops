using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class FraisAccessoires
    {
        public bool IsReadOnly { get; set; }
        public bool IsModifHorsAvn { get; set; }
        [Display(Name = "Type de frais")]
        public string TypeFrais { get; set; }
        public List<AlbSelectListItem> TypesFrais { get; set; }
        [Display(Name = "Frais standards")]
        public int FraisStandards { get; set; }
        [Display(Name = "Frais retenus")]
        public int FraisRetenus { get; set; }
        [Display(Name = "FGA")]
        public bool TaxeAttentat { get; set; }
        [Display(Name = "Appliquer des frais accessoires spécifiques pour cette quittance")]
        public bool AppliqueFraisSpecifiques { get; set; }
        [Display(Name = "Frais spécifiques")]
        public int FraisSpecifiques { get; set; }
        [Display(Name = "Commentaires")]
        public string Commentaires { get; set; }
        public long CodeCommentaires { get; set; }
        public int Frais { get; set; }

        public string ActeGestion { get; set; }
        public string ActeGestionRegule { get; set; }

        public string ModifFraisStd { get; set; }
        public string AttCatego { get; set; }

        public static explicit operator FraisAccessoires(FraisAccessoiresDto fraisAccessoiresDto)
        {
            var toReturn= ObjectMapperManager.DefaultInstance.GetMapper<FraisAccessoiresDto, FraisAccessoires>().Map(fraisAccessoiresDto);
            toReturn.TaxeAttentat = !string.IsNullOrEmpty(fraisAccessoiresDto.AppliqueTaxeAttentat) && fraisAccessoiresDto.AppliqueTaxeAttentat == "O" ? true : false;
            toReturn.AppliqueFraisSpecifiques = fraisAccessoiresDto.FraisSpecifiques != 0 ? true : false;
           
            toReturn.TypesFrais = fraisAccessoiresDto.TypesFrais.Select(m => new AlbSelectListItem() { Value = m.Code.ToString(), Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
            return toReturn;
        }
    }
}