using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Transverse
{
    public class UpdateMetadata
    {
        public UpdateMetadata()
        {

        }
        public UpdateMetadata(string user, DateTime? time)
        {
            this.User = user;
            this.Time = time;

        }
        public string User { get; set; }
        public DateTime? Time { get; set; } 
    }
}
