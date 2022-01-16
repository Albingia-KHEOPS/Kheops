using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Attestation
{
    public class AttestationSelRsqPlatDto
    {
        [Column(Name = "CODERSQ")]
        public Int32 CodeRsq { get; set; }
        [Column(Name = "LIBRSQ")]
        public string LibRsq { get; set; }
        [Column(Name = "DATEDEBRSQ")]
        public Int64 DateDebRsq { get; set; }
        [Column(Name = "DATEFINRSQ")]
        public Int64 DateFinRsq { get; set; }
        [Column(Name = "CIBLERSQ")]
        public string CibleRsq { get; set; }
        [Column(Name = "VALRSQ")]
        public Int64 ValRsq { get; set; }
        [Column(Name = "UNITERSQ")]
        public string UniteRsq { get; set; }
        [Column(Name="LIBUNTRSQ")]
        public string LibUnitRsq { get; set; }
        [Column(Name="RSQUSE")]
        public string RsqUse { get; set; }
        [Column(Name = "CODEOBJ")]
        public Int32 CodeObj { get; set; }
        [Column(Name = "LIBOBJ")]
        public string LibObj { get; set; }
        [Column(Name = "DATEDEBOBJ")]
        public Int64 DateDebObj { get; set; }
        [Column(Name = "DATEFINOBJ")]
        public Int64 DateFinObj { get; set; }
        [Column(Name = "CIBLEOBJ")]
        public string CibleObj { get; set; }
        [Column(Name = "VALOBJ")]
        public Int64 ValObj { get; set; }
        [Column(Name = "UNITEOBJ")]
        public string UniteObj { get; set; }
        [Column(Name = "LIBUNTOBJ")]
        public string LibUnitObj { get; set; }
        [Column(Name = "OBJUSE")]
        public string ObjUse { get; set; }
    }
}
