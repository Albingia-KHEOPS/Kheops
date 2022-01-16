using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    public class GarantieFilter
    {
        public long LotId { get; set; }

        public long RgId { get; set; }

        public int RsqNum { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public long GrId { get; set; }
    }
}
