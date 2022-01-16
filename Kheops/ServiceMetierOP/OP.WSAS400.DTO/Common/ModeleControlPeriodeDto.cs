using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Common
{
    public class ModeleControlPeriodeDto
    {
        public string CodeErreur { get; set; }
        public string PeriodeDeb { get; set; }
        public string PeriodeFin { get; set; }
        public Int64 CodeCourtierPayeur { get; set; }
        public Int64 CodeCourtierCommission { get; set; }
        public bool MultiCourtier { get; set; }
        public string CodeEncaissement { get; set; }
        public Int32 DernierAvn { get; set; }
    }
}
