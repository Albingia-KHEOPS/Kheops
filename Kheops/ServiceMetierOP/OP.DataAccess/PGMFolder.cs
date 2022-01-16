using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess {
    public class PGMFolder : Folder, IFolderAct {
        public PGMFolder() : this(null) { }
        public PGMFolder(Folder folder) : base() {
            Now = DateTime.Now;
            if (folder != null) {
                CodeOffre = folder.CodeOffre;
                Version = folder.Version;
                Type = folder.Type;
                NumeroAvenant = folder.NumeroAvenant;
            }
        }
        public string User { get; set; }
        public string ActeGestion { get; set; }
        public DateTime Now { get; private set; }
        public string Name => "Dossier Program AS400";
    }
}
