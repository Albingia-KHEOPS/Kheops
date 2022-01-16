using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Affaire {
    public class LockState {
        public LockState(string user) {
            User = user;
        }
        public LockState() : this(null) { }
        public string User { get; set; }
        public string Action { get; set; }
        public string ActeGestion { get; set; }
        public bool IsLocked { get; set; } = false;
    }
}
