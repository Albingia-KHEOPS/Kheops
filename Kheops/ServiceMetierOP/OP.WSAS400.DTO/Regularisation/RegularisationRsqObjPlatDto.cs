using System;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Regularisation
{
    public class RegularisationRsqObjPlatDto
    {
        [Column(Name = "CODERSQ")]
        public Int32 CodeRsq { get; set; }
        [Column(Name = "LIBRSQ")]
        public string LibRsq { get; set; }
        [Column(Name = "DATEDEBRSQ")]
        public Int32 DateDebRsq { get; set; }
        [Column(Name = "DATEFINRSQ")]
        public Int32 DateFinRsq { get; set; }
        [Column(Name = "CIBLERSQ")]
        public string CibleRsq { get; set; }
        [Column(Name = "LIBCIBLERSQ")]
        public string LibCibleRsq { get; set; }
        [Column(Name = "TYPEREGULRSQ")]
        public string TypeReguleRsq { get; set; }
        [Column(Name = "LIBREGULRSQ")]
        public string LibTypeRegulRsq { get; set; }
        [Column(Name = "CODEOBJ")]
        public Int32 CodeObj { get; set; }
        [Column(Name = "LIBOBJ")]
        public string LibObj { get; set; }
        [Column(Name = "DATEDEBOBJ")]
        public Int32 DateDebObj { get; set; }
        [Column(Name = "DATEFINOBJ")]
        public Int32 DateFinObj { get; set; }
        [Column(Name = "CIBLEOBJ")]
        public string CibleObj { get; set; }
        [Column(Name = "LIBCIBLEOBJ")]
        public string LibCibleObj { get; set; }
        [Column(Name = "TYPEREGULOBJ")]
        public string TypeReguleObj { get; set; }
        [Column(Name = "LIBREGULOBJ")]
        public string LibTypeRegulObj { get; set; }
        [Column(Name="ISRSQUSED")]
        public Int32 IsRsqUsed { get; set; }
    }
}
