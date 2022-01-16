using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OP.WSAS400.DTO.MatriceFormule
{
    public class MatriceFormuleColumnDto
    {
        [DataMember]
        [Column(Name = "CODEFORMULE")]
        public int CodeFormule { get; set; }
        [Column(Name = "CODEOPTION")]
        public int CodeOption { get; set; }
        [Column(Name = "LIBFORMULE")]
        public string LibFormule { get; set; }
        [Column(Name = "NOMFORMULE")]
        public string NomFormule { get; set; }
        [Column(Name = "NUMCHRONO")]
        public Int32 NumChronoColumn { get; set; }
        [Column(Name = "CREATEAVT")]
        public Int32 CreateAvt { get; set; }
        [Column(Name = "MODIFAVT")]
        public Int32 ModifAvt { get; set; }
        [Column(Name = "DATEAVT")]
        public Int64 DateAvt { get; set; }
        [Column(Name = "DATEFINAVT")]
        public Int64 DateFinAvt { get; set; }
        [Column(Name = "CODERSQ")]
        public Int64 CodeRsq { get; set; }
        [Column(Name = "AVTFOR")]
        public Int64 CodeFormuleAvt { get; set; }
    }
}
