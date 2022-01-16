using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models {
    public class ModelRelances {
        public int NbLignesPage => ConfigurationManager.AppSettings["PageSize"].ParseInt(20).Value;
        public int NombreRelances { get; set; }
        public IEnumerable<RelanceOffre> Relances { get; set; } = new List<RelanceOffre>();
        public bool DoNotShowAgainForToday { get; set; }
    }
}