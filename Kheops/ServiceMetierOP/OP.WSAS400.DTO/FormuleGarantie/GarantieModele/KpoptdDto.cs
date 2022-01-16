using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie.GarantieModele
{
    public class KpoptdDto
    {
        public long KDCID { get; set; }
        public string KDCTYP { get; set; }
        public string KDCIPB { get; set; }
        public int KDCALX { get; set; }
        public int KDCFOR { get; set; }
        public int KDCOPT { get; set; }
        public long KDCKDBID { get; set; }
        public string KDCTENG { get; set; }
        public long KDCKAKID { get; set; }
        public long KDCKAEID { get; set; }
        public long KDCKAQID { get; set; }
        public string KDCMODELE { get; set; }
        public long KDCKARID { get; set; }
        public string KDCCRU { get; set; }
        public long KDCCRD { get; set; }
        public string KDCMAJU { get; set; }
        public long KDCMAJD { get; set; }
        public int KDCFLAG { get; set; }
        public Single KDCORDRE { get; set; }
    }
}
