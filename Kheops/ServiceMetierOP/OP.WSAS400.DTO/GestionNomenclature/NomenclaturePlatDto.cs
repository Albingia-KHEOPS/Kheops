using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.GestionNomenclature
{
    public class NomenclaturePlatDto
    {
        [Column(Name = "NOMENCLATUREID")]
        public Int64 Id { get; set; }
        [Column(Name = "NOMENCLATUREORDRE")]
        public Double Ordre { get; set; }
        [Column(Name = "NOMENCLATURECODE")]
        public string Code { get; set; }
        [Column(Name = "NOMENCLATURELIB")]
        public string Libelle { get; set; }
        [Column(Name = "CODETYPO")]
        public string CodeTypologie { get; set; }
        [Column(Name = "CODEGRILLE")]
        public string CodeGrille { get; set; }
        [Column(Name = "LIBGRILLE")]
        public string LibGrille { get; set; }
    }
}
