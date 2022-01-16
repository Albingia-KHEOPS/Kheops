using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.GestionNomenclature
{
    public class GrillePlatDto
    {
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [Column(Name = "TYPO1")]
        public string Typologie1 { get; set; }
        [Column(Name = "LIBTYPO1")]
        public string LibTypologie1 { get; set; }
        [Column(Name = "LIENTYPO1")]
        public string LienTypologie1 { get; set; }
        [Column(Name = "TYPO2")]
        public string Typologie2 { get; set; }
        [Column(Name = "LIBTYPO2")]
        public string LibTypologie2 { get; set; }
        [Column(Name = "LIENTYPO2")]
        public string LienTypologie2 { get; set; }
        [Column(Name = "TYPO3")]
        public string Typologie3 { get; set; }
        [Column(Name = "LIBTYPO3")]
        public string LibTypologie3 { get; set; }
        [Column(Name = "LIENTYPO3")]
        public string LienTypologie3 { get; set; }
        [Column(Name = "TYPO4")]
        public string Typologie4 { get; set; }
        [Column(Name = "LIBTYPO4")]
        public string LibTypologie4 { get; set; }
        [Column(Name = "LIENTYPO4")]
        public string LienTypologie4 { get; set; }
        [Column(Name = "TYPO5")]
        public string Typologie5 { get; set; }
        [Column(Name = "LIBTYPO5")]
        public string LibTypologie5 { get; set; }
        [Column(Name = "LIENTYPO5")]
        public string LienTypologie5 { get; set; }
        [Column(Name = "TYPOGRILLE")]
        public string TypologieGrille { get; set; }
    }
}
