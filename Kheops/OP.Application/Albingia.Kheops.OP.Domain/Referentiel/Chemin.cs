using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.Domain.Transverse;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class Chemin
    {
// CLE
public string Key { get; set; }

        // SRV
        public string Server { get; set; }

        // RAC
        public string Root { get; set; }

        // ENV
        public string Environment { get; set; }

        // DES
        public string Designation { get; set; }

        // TCH
        public string PathType { get; set; }

        // CHM
        public string Location { get; set; }

        // CRU
        // CRD
        public UpdateMetadata Creation { get; set; }


        // MJU
        // MJD
        public UpdateMetadata MiseAJour { get; set; }

    }
}
