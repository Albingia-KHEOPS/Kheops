using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.MenuContextuel;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesContextMenu
{
    public class ModeleListItem
    {
        public string Utilisateur { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public string alias { get; set; }
        public string action { get; set; }
        public string type { get; set; }
        public string width { get; set; }
        public AlbContextMenu menu { get; set; }
        public string nomMenu { get; set; }
        public string AlwBranche { get; set; }
        public string typeOffreContrat { get; set; }
        public string AlwEtat { get; set; }
        public int orderby { get; set; }
        public List<ModeleListItem> items { get; set; }

        public static explicit operator ModeleListItem(ContextMenuDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ContextMenuDto, ModeleListItem>().Map(modelDto);
        }

        public static explicit operator ModeleListItem(ContextMenuPlatDto modelPlatDto)
        {
            ModeleListItem toReturn = new ModeleListItem
            {
                typeOffreContrat = modelPlatDto.TypeOffreContrat,
                text = modelPlatDto.OptionLib,
                icon = modelPlatDto.Action,
                alias = modelPlatDto.Action,
                Utilisateur = modelPlatDto.Utilisateur,
                nomMenu = ((AlbContextMenu)Enum.Parse(typeof(AlbContextMenu), modelPlatDto.Menu)).DisplayName(),
                AlwBranche = modelPlatDto.AlwBranche,
                AlwEtat = modelPlatDto.AlwEtat
            };
            return toReturn;
        }
    }


}