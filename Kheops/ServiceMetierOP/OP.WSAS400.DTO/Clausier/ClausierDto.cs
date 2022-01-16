using System;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Clausier
{
    public class ClausierDto
    {
        [Column(Name="CODE")]
        public Int64 Code { get; set; }
        [Column(Name = "RUBRIQUE")]
        public string Rubrique { get; set; }
        [Column(Name = "SOUSRUBRIQUE")]
        public string SousRubrique { get; set; }
        [Column(Name = "SEQUENCE")]
        public Single Sequence { get; set; }
        [Column(Name = "VERSION")]
        public Int64 Version { get; set; }
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [Column(Name="DATEDEB")]
        public Int64 DateDeb { get; set; }
        [Column(Name="DATEFIN")]
        public Int64 DateFin { get; set; }
    }
}
