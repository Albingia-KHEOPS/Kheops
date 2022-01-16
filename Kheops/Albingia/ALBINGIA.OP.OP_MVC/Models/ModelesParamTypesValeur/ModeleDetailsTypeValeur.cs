using OP.WSAS400.DTO.ParametreTypeValeur;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamTypesValeur
{
    public class ModeleDetailsTypeValeur
    {

        public string CodeTypeValeur { get; set; }
        public string DescriptionTypeValeur { get; set; }
        public string AdditionalParam { get; set; }
        public string ModeSaisie { get; set; }
        public int IdLigneVide { get; set; }

        public List<ModeleLigneTypeValeurCompatible> ListeTypesValeurCompatible { get; set; }
        public ModeleLigneTypeValeurCompatible LigneVideTypesValeur { get; set; }

        public static explicit operator ModeleDetailsTypeValeur(ModeleDetailsTypeValeurDto detailstypeValeurDto)
        {
            var toReturn = new ModeleDetailsTypeValeur()
            {
                CodeTypeValeur = detailstypeValeurDto.CodeTypeValeur,
                DescriptionTypeValeur = detailstypeValeurDto.DescriptionTypeValeur,
                ListeTypesValeurCompatible = (detailstypeValeurDto.ListeTypesValeurCompatible == null ? null : new List<ModeleLigneTypeValeurCompatible>())                             
            };
            if (toReturn.ListeTypesValeurCompatible != null)
                detailstypeValeurDto.ListeTypesValeurCompatible.ForEach(elm => toReturn.ListeTypesValeurCompatible.Add((ModeleLigneTypeValeurCompatible)elm));

            return toReturn;
        }

    }
}