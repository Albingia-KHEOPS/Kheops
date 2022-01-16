using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesDoubleSaisie;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleDoubleSaisiePage : MetaModelsBase
    {
        public ModeleDoubleSaisieCourtier CourtierApporteur { get; set; }
        public ModeleDoubleSaisieCourtier CourtierGestionnaire { get; set; }
        public List<ModeleDoubleSaisieCourtier> CourtiersHistorique { get; set; }
        public ModeleDoubleSaisieCourtier CourtierDemandeur { get; set; }

        public string MotifRemp { get; set; }
        public List<AlbSelectListItem> MotifsRemp { get; set; }
        public string NotificationApporteur { get; set; }
        public List<AlbSelectListItem> NotificationsApporteur { get; set; }
        public string NotificationDemandeur { get; set; }
        public List<AlbSelectListItem> NotificationsDemandeur { get; set; }

        public bool DivFlottante { get; set; }
    }
}