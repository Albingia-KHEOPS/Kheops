using System;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.FormuleGarantie.GarantieModele
{
    public class GarantieInfosDto
    {
        public Int32 CodeRisque { get; set; }

        [Column(Name = "CODEOBJET")]
        public Int32 CodeObjet { get; set; }

        [Column(Name = "GARANTIEINDEX")]
        public string GarantieIndex { get; set; }

        [Column(Name = "LCI")]
        public string Lci { get; set; }

        [Column(Name = "FRANCHISE")]
        public string Franchise { get; set; }

        [Column(Name = "ASSIETTE")]
        public string Assiette { get; set; }

        [Column(Name = "CATNAT")]
        public string CatNat { get; set; }

    }
}
