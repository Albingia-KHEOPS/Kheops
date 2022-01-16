using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    public class OptTarAffNouvPlatDto
    {
        [Column(Name = "CODEFORM")]
        public Int64 CodeForm { get; set; }
        [Column(Name = "CODEGARAN")]
        public string CodeGaran { get; set; }
        [Column(Name = "DESCGARAN")]
        public string DescGaran { get; set; }
        [Column(Name = "LETTREFORM")]
        public string LettreForm { get; set; }
        [Column(Name = "CODETARIF")]
        public Int64 CodeTarif { get; set; }
        [Column(Name = "GUIDTARIF")]
        public Int64 GuidTarif { get; set; }
        [Column(Name = "LCIVAL")]
        public Double LCIVal { get; set; }
        [Column(Name = "LCIUNIT")]
        public string LCIUnit { get; set; }
        [Column(Name = "LCITYPE")]
        public string LCIType { get; set; }
        [Column(Name = "IDLCICPX")]
        public Int64 IdLCICpx { get; set; }
        [Column(Name = "FRHVAL")]
        public Double FRHVal { get; set; }
        [Column(Name = "FRHUNIT")]
        public string FRHUnit { get; set; }
        [Column(Name = "FRHTYPE")]
        public string FRHType { get; set; }
        [Column(Name = "IDFRHCPX")]
        public Int64 IdFRHCpx { get; set; }
        [Column(Name = "ASSVAL")]
        public Double ASSVal { get; set; }
        [Column(Name = "ASSUNIT")]
        public string ASSUnit { get; set; }
        [Column(Name = "ASSTYPE")]
        public string ASSType { get; set; }
        [Column(Name = "PRIVAL")]
        public decimal PRIVal { get; set; }
        [Column(Name = "PRIUNIT")]
        public string PRIUnit { get; set; }
        [Column(Name = "PRITYPE")]
        public string PRIType { get; set; }
        [Column(Name="PRIMPRO")]
        public Double PRIMPro { get; set; }
        [Column(Name = "CHECKROWDB")]
        public string CheckRowDb { get; set; }
    }
}
