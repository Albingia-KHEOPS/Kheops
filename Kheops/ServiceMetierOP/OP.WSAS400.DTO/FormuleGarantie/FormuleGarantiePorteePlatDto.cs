using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    public class FormuleGarantiePorteePlatDto
    {
        [Column(Name = "CODERSQ")]
        public Int64 CodeRsq { get; set; }
        [Column(Name = "LIBRSQ")]
        public string LibRsq { get; set; }
        [Column(Name = "CODEOBJ")]
        public Int64 CodeObj { get; set; }
        [Column(Name = "LIBOBJ")]
        public string LibObj { get; set; }
        [Column(Name = "ACTION")]
        public string Action { get; set; }
        [Column(Name = "IDGAR")]
        public Int64 IdGar { get; set; }
        [Column(Name = "SEQGAR")]
        public Int64 SeqGar { get; set; }
        [Column(Name = "CODEGAR")]
        public string CodeGar { get; set; }
        [Column(Name = "LIBGAR")]
        public string LibGar { get; set; }
        [Column(Name = "VALEUROBJ")]
        public Int64 ValeurObj { get; set; }
        [Column(Name = "UNITOBJ")]
        public string UnitObj { get; set; }
        [Column(Name = "TYPEOBJ")]
        public string TypeObj { get; set; }
        [Column(Name = "IDPORTEE")]
        public Int64 IdPortee { get; set; }
        [Column(Name = "VALPORTEEOBJ")]
        public decimal ValPorteeObj { get; set; }
        [Column(Name = "UNITPORTEEOBJ")]
        public string UnitPorteObj { get; set; }
        [Column(Name = "TYPEPORTEECAL")]
        public string TypePorteeCal { get; set; }
        [Column(Name = "PRIMEMNTCAL")]
        public double PrimeMntCal { get; set; }
        [Column(Name = "DATESORTIEOBJ")]
        public Int64 DateSortieObj { get; set; }
        [Column(Name = "REPORTCAL")]
        public string ReportCal { get; set; }
    }
}
