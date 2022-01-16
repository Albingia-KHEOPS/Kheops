using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Offres.Risque
{
    public class RisqueObjetPlatDto
    {
        [Column(Name="CODERSQ")]
        public Int64 CodeRsq { get; set; }

        [Column(Name="DESCRSQ")]
        public string DescRsq { get; set; }

        [Column(Name="CODECIBLE")]
        public Int64 CodeCible { get; set; }

        [Column(Name = "CIBLE")]
        public string Cible { get; set; }

        [Column(Name="DESCCIBLE")]
        public string DescCible { get; set; }

        [Column(Name="CODEOBJ")]
        public Int64 CodeObj { get; set; }
        [Column(Name = "DESCOBJ")]
        public string DescObj { get; set; }
        [Column(Name="CODEINVEN")]
        public Int64 CodeInven { get; set; }
        [Column(Name = "DESCINVEN")]
        public string DescInven { get; set; }
        [Column(Name = "RSQUSED")]
        public Int64 RsqUsed { get; set; }
        [Column(Name="RSQOUT")]
        public Int64 RsqOut { get; set; }
        [Column(Name = "OBJUSED")]
        public Int64 ObjUsed { get; set; }
        [Column(Name="OBJOUT")]
        public Int64 ObjOut { get; set; }
    }
}
