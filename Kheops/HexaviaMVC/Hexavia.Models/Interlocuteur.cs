using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexavia.Models
{
    public class Interlocuteur
    {
        public int? CodeCourtier { get; set; }
        public string Name { get; set; }
        public string NameCourtier { get; set; }
        public int? Orias { get; set; }
        public PartnerAdresse Address { get; set; }
    }
}
