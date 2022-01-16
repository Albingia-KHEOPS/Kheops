using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    public class InventaireGarantieDto
    {
        [Column(Name="INVENSPEC")]
        public string InvenSpec { get; set; }
        [Column(Name="TYPEALIM")]
        public string TypeAlim { get; set; }
    }
}
