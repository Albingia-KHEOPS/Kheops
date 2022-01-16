using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleDetailsConnexitesEng
    {
            public string ModeActif { get; set; }
            public List<AlbSelectListItem> ListeModesActif { get; set; }
            public string ModeUtilise { get; set; }
            public List<AlbSelectListItem> ListeModesUtilise { get; set; }
            public List<ModeleDetailsConnexitesEngPeriode> ConnexitesEngPeriodes { get; set; }
    }
}