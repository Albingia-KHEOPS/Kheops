using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie.GarantieModele
{
    public class GarantieModelePlatDto
    {
        [Column(Name = "GUID")]
        public Int64 Guid { get; set; }
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }
        [Column(Name = "CIBLE")]
        public string Cible { get; set; }
        [Column(Name = "VOLET")]
        public string Volet { get; set; }
        [Column(Name = "BLOC")]
        public string Bloc { get; set; }
        [Column(Name = "TYPOLOGIE")]
        public string Typologie { get; set; }
        [Column(Name = "DATEAPPLI")]
        public Int32 DateAppli { get; set; }
    }
}
