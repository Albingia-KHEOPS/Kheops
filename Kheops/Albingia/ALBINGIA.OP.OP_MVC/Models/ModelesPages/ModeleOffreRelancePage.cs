using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleOffreRelancePage : MetaModelsBase
    {
        public ModelRelances ListeRelances { get; set; }
        public int NbPage => (int)(Math.Truncate((decimal)(ListeRelances.NombreRelances / ListeRelances.NbLignesPage)) + 1);
        public int CurrentPage { get; set; }
        public bool AllowPrevious => CurrentPage != 1;
        public bool AllowNext => CurrentPage != NbPage;
        public List<AlbSelectListItem> ListeSituations => new List<AlbSelectListItem>()
        {
            new AlbSelectListItem() { Value = "", Text = "A relancer", Title = "A relancer" },
            new AlbSelectListItem() { Value = "S", Text = "Sans-suite", Title = "Sans-suite"}
        };

        public List<AlbSelectListItem> MotifsSituation { get; set; }
    }
}