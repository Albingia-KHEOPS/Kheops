using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.MatriceGarantie
{
    public class MatriceGarantiePlatDto
    {
        [Column(Name = "IDVOLET")]
        public Int64 IdVolet { get; set; }
        [Column(Name = "LIBELLEVOLET1")]
        public string LibelleVolet1 { get; set; }
        [Column(Name = "LIBELLEVOLET2")]
        public string LibelleVolet2 { get; set; }
        [Column(Name = "IDBLOC")]
        public Int64 IdBloc { get; set; }
        [Column(Name = "LIBELLEBLOC1")]
        public string LibelleBloc1 { get; set; }
        [Column(Name = "LIBELLEBLOC2")]
        public string LibelleBloc2 { get; set; }
        [Column(Name = "LIBELLEGARANTIE1")]
        public string LibelleGarantie1 { get; set; }
        [Column(Name = "LIBELLEGARANTIE2")]
        public string LibelleGarantie2 { get; set; }
        [Column(Name = "NIVEAUGARANTIE")]
        public Int64 NiveauGarantie { get; set; }
        [Column(Name = "IDSEQUENCE")]
        public Int64 IdSequence { get; set; }
        [Column(Name = "NIVEAUSUP")]
        public Int64 NiveauSup { get; set; }
        [Column(Name = "NIVEAUORIGINE")]
        public Int64 NiveauOrigine { get; set; }
        [Column(Name = "ICON1")]
        public string Icon1 { get; set; }
        [Column(Name = "ICON2")]
        public string Icon2 { get; set; }
        [Column(Name = "IDRISQUE")]
        public Int64 IdRisque { get; set; }
    }
}
