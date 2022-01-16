using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Controllers;
using OP.WSAS400.DTO.ParametreFiltre;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamFiltres
{
    public class ModeleLigneBrancheCible
    {
        public Int64 GuidIdPaire { get; set; }
        public string Action { get; set; }
        public string Branche { get; set; }
        public string Cible { get; set; }
        public string AdditionalParam { get; set; }

        public List<AlbSelectListItem> ListeActions { get; set; }
        public List<AlbSelectListItem> ListeBranches { get; set; }
        public ModeleDrlCibles DrlCibles { get; set; }

        public static explicit operator ModeleLigneBrancheCible(ModeleLigneBrancheCibleDto ligneBrancheCibleDto)
        {
            var toReturn = new ModeleLigneBrancheCible
            {
                GuidIdPaire = ligneBrancheCibleDto.GuidIdPaire,
                Action = ligneBrancheCibleDto.Action,
                Branche = ligneBrancheCibleDto.Branche,
                Cible = ligneBrancheCibleDto.Cible,
                ListeActions = ParamFiltresController.LstActions,
                ListeBranches = ParamFiltresController.LstBranches,
                DrlCibles = new ModeleDrlCibles()
            };
            if (toReturn.ListeActions.Find(elm => elm.Value == toReturn.Action) != null)
                toReturn.ListeActions.Find(elm => elm.Value == toReturn.Action).Selected = true;
            if (toReturn.ListeBranches.Find(elm => elm.Value == toReturn.Branche) != null)
                toReturn.ListeBranches.Find(elm => elm.Value == toReturn.Branche).Selected = true;

            toReturn.DrlCibles.ListeCibles = ParamFiltresController.LstCibles(ligneBrancheCibleDto.Branche);
            toReturn.DrlCibles.GuidIdPaire = toReturn.GuidIdPaire;
            if (toReturn.DrlCibles.ListeCibles.Find(elm => elm.Value == toReturn.Cible) != null)
                toReturn.DrlCibles.ListeCibles.Find(elm => elm.Value == toReturn.Cible).Selected = true;

            return toReturn;
        }
    }
}