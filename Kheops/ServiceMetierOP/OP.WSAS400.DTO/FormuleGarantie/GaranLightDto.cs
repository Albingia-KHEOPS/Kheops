using System;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    public class GaranLightDto
    {
        [Column(Name = "KDEID")]
        public Int64 Id { get; set; }

        [Column(Name = "KDETYP")]
        public string Type { get; set; }

        [Column(Name = "KDEIPB")]
        public string CodeAffaire { get; set; }

        [Column(Name = "KDEALX")]
        public Int64 Aliment { get; set; }

        [Column(Name = "KDEFOR")]
        public Int64 CodeFormule { get; set; }

        [Column(Name = "KDEOPT")]
        public Int64 Option { get; set; }

        [Column(Name = "KDESEQ")]
        public Int64 Sequence { get; set; }

        [Column(Name = "KDEALA")]
        public string Alimentation { get; set; }
    }
}
