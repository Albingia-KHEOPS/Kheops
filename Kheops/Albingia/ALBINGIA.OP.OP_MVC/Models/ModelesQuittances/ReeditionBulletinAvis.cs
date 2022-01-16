using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class ReeditionBulletinAvis
    {
        public string NumQuittance { get; set; }
        public List<AlbSelectListItem> ListeCopieDuplicata { get; set; }
        public string ValCopieDuplicata { get; set; }
    }
}