using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres.Risque.Inventaire
{
    public class InventairePlatDto
    {
        [Column(Name = "CODE")]
        public Int64 Code { get; set; }
        [Column(Name = "DESCRIPTIF")]
        public String Descriptif { get; set; }
        [Column(Name = "CODETYPE")]
        public String CodeType { get; set; }
        [Column(Name = "PERIMETREAPPLICATION")]
        public String PerimetreApplication { get; set; }
        [Column(Name = "NUMDESCRIPTION")]
        public Int64 NumDescription { get; set; }
    }
}
