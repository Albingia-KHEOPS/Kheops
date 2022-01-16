using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreGaranties
{
    public class ParamGarantieDto
    {
        public ParamGarantieDto()
        {
            GarTypeReguls = new List<GarTypeRegulDto>();
        }

        [DataMember]
        [Column(Name = "CODE")]
        public string CodeGarantie { get; set; }
        [DataMember]
        [Column(Name = "DESIGNATION")]
        public string DesignationGarantie { get; set; }
        [DataMember]
        [Column(Name = "ABREGE")]
        public string Abrege { get; set; }
        [DataMember]
        [Column(Name = "CODETAXE")]
        public string CodeTaxe { get; set; }
        [DataMember]
        [Column(Name = "CODECATNAT")]
        public string CodeCatNat { get; set; }
        [DataMember]
        [Column(Name = "GARCOM")]
        public string GarantieCommune { get; set; }
        [DataMember]
        [Column(Name = "CODETYPEDEF")]
        public string CodeTypeDefinition { get; set; }
        [DataMember]
        [Column(Name = "CODETYPINFO")]
        public string CodeTypeInformation { get; set; }
        [DataMember]
        [Column(Name = "REGU")]
        public string Regularisable { get; set; }
        [DataMember]
        [Column(Name = "CODETYPGRILLE")]
        public string CodeTypeGrille { get; set; }
        [DataMember]
        [Column(Name = "INVENTAIRE")]
        public string Inventaire { get; set; }
        [DataMember]
        [Column(Name = "ATTENTATGAREAT")]
        public string AttentatGareat { get; set; }
        [DataMember]
        [Column(Name = "CODETYPINVENTAIRE")]
        public string CodeTypeInventaire { get; set; }
        [DataMember]
        public List<GarTypeRegulDto> GarTypeReguls { get; set; }

    }
}
