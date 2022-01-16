using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    public class FormVolAffNouvPlatDto
    {
        [Column(Name = "CODERISQUE")]
        public Int64 CodeRisque { get; set; }
        [Column(Name = "DESCRISQUE")]
        public string DescRisque { get; set; }
        [Column(Name = "GUIDFORM")]
        public Int64 GuidForm { get; set; }
        [Column(Name = "CODEFORM")]
        public Int64 CodeForm { get; set; }
        [Column(Name = "LETTREFORM")]
        public string LettreForm { get; set; }
        [Column(Name = "DESCFORMULE")]
        public string DescFormule { get; set; }
        [Column(Name = "GUIDOPT")]
        public Int64 GuidOpt { get; set; }
        [Column(Name = "CODEOPT")]
        public Int64 CodeOpt { get; set; }
        [Column(Name = "GUIDVOLET")]
        public Int64 GuidVolet { get; set; }
        [Column(Name = "CODEVOLET")]
        public string CodeVolet { get; set; }
        [Column(Name = "DESCVOLET")]
        public string DescVolet { get; set; }
        [Column(Name = "CHECKROWDB")]
        public string CheckRowDb { get; set; }
    }
}
