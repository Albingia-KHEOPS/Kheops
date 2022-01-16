using OP.WSAS400.DTO.ParametreFiltre;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamFiltres
{
    public class ModeleDetailsFiltre
    {
        public Int64 GuidIdFiltre { get; set; }
        public string CodeFiltre { get; set; }
        public string DescriptionFiltre { get; set; }
        public string AdditionalParam { get; set; }
        public string ModeSaisie { get; set; }
        public int DateCreation { get; set; }
        public int IdLigneVide { get; set; }

        public DateTime? dDateCreation { get; set; }

        public List<ModeleLigneBrancheCible> ListePairesBrancheCible { get; set; }
        public ModeleLigneBrancheCible LigneVideBrancheCible { get; set; }

        public static explicit operator ModeleDetailsFiltre(ModeleDetailsFiltreDto detailsFiltreDto)
        {
            var toReturn = new ModeleDetailsFiltre()
            {
                GuidIdFiltre = detailsFiltreDto.GuidIdFiltre,
                CodeFiltre = detailsFiltreDto.CodeFiltre,
                DescriptionFiltre = detailsFiltreDto.DescriptionFiltre,
                DateCreation = detailsFiltreDto.DateCreation,
                dDateCreation = ALBINGIA.Framework.Common.Tools.AlbConvert.ConvertIntToDate(detailsFiltreDto.DateCreation),
                ListePairesBrancheCible = (detailsFiltreDto.ListePairesBrancheCible == null ? null : new List<ModeleLigneBrancheCible>())
            };
            if (toReturn.ListePairesBrancheCible != null)
                detailsFiltreDto.ListePairesBrancheCible.ForEach(elm => toReturn.ListePairesBrancheCible.Add((ModeleLigneBrancheCible)elm));

            return toReturn;
        }
    }
}