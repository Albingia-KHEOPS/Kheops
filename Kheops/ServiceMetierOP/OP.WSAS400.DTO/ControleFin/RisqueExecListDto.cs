using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ControleFin
{
    public class RisqueExecListDto
    {
        [Column(Name="JEOBJ")]
        public Int32 CodeObjet{get;set;}
        [Column(Name="JERSQ")]
        public Int32 CodeRisque { get; set; }
        [Column(Name = "JEVDA")]
        public Int16 EntreeGarantieAnnee { get; set; }
        [Column(Name = "JEVDM")]
        public Int16 EntreeGarantieMois { get; set; }
        [Column(Name = "JEVDJ")]
        public Int16 EntreeGarantieJour { get; set; }
        [Column(Name = "JEVFA")]
        public Int16 SortieGarantieAnnee { get; set; }
        [Column(Name = "JEVFM")]
        public Int16 SortieGarantieMois { get; set; }
        [Column(Name = "JEVFJ")]
        public Int16 SortieGarantieJour { get; set; }
        [Column(Name = "JECNA")]
        public string CATNAT { get; set; }
        [Column(Name = "JEINA")]
        public string RisqueIndexe { get; set; }
    }
}
