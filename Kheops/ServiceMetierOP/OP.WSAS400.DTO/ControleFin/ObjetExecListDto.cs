using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ControleFin
{
    public class ObjetExecListDto
    {
        [Column(Name = "JGOBJ")]
        public Int32 CodeObjet { get; set; }
        [Column(Name = "JGRSQ")]
        public Int32 CodeRisque { get; set; }
        [Column(Name = "JGVDA")]
        public Int16 EntreeGarantieAnnee { get; set; }
        [Column(Name = "JGVDM")]
        public Int16 EntreeGarantieMois { get; set; }
        [Column(Name = "JGVDJ")]
        public Int16 EntreeGarantieJour { get; set; }
        [Column(Name = "JGVFA")]
        public Int16 SortieGarantieAnnee { get; set; }
        [Column(Name = "JGVFM")]
        public Int16 SortieGarantieMois { get; set; }
        [Column(Name = "JGVFJ")]
        public Int16 SortieGarantieJour { get; set; }
        [Column(Name = "JGCNA")]
        public string CATNAT { get; set; }
        [Column(Name = "JGINA")]
        public string RisqueIndexe { get; set; }
    }
}
