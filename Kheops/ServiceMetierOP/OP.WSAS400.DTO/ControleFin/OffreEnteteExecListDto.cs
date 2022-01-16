using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ControleFin
{
    public class OffreEnteteExecListDto
    {
        [Column(Name = "JDCNA")]
        public string Indexation { get; set; }
        [Column(Name = "JDINA")]
        public string SoumisCatnat { get; set; }
        [Column(Name = "JDIVO")]
        public Single ValOrigine { get; set; }
        [Column(Name = "JDIVA")]
        public Single ValActualisee { get; set; }
    }
}
