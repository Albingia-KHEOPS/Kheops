using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.Domain.Affaire;

namespace Albingia.Kheops.OP.Domain.Document
{
    public class CopyDoc
    {
        public AffaireId AffaireId { get; set; }
        public string OldPath { get; set; }
        public string NewName { get; set; }
        public string Table { get; set; } = "KPDOC";
        public long OldID { get; set; }
        public long NewNumber { get; set; }
    }
}
