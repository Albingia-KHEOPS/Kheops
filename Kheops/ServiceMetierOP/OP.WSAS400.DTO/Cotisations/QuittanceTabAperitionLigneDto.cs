using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class QuittanceTabAperitionLigneDto
    {
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }

        [DataMember]
        public double MainHCatnat { get; set; }
        [DataMember]
        public double MainCatnat { get; set; }
        [DataMember]
        public double MainTotal { get; set; }

        [DataMember]
        [Column(Name = "HTHCATNAT")]
        public double HTHCatnat { get; set; }
        [DataMember]
        [Column(Name = "HTCATNAT")]
        public double HTCatnat { get; set; }
        [DataMember]
        [Column(Name = "HTTOTAL")]
        public double HTTotal { get; set; }

        [DataMember]
        [Column(Name = "COMMHCATNAT")]
        public double CommHCatnat { get; set; }
        [DataMember]
        [Column(Name = "COMMCATNAT")]
        public double CommCatnat { get; set; }
        [DataMember]
        [Column(Name = "COMMTOTAL")]
        public double CommTotal { get; set; }    
 
        [DataMember]
        [Column(Name = "PARTCOASS")]
        public double Part { get; set; }
        [DataMember]
        [Column(Name = "FRAISAPERITION")]
        public double FraisAperition { get; set; }
    }
}
