using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.GestionNomenclature
{
    [DataContract]
    public class NomenclatureDto
    {
        [DataMember]
        [Column(Name = "NOMENCLATUREID")]
        public Int64 Id { get; set; }
        [DataMember]
        [Column(Name = "NOMENCLATUREORDRE")]
        public Double Ordre { get; set; }
        [DataMember]
        [Column(Name = "NOMENCLATURECODE")]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "NOMENCLATURELIB")]
        public string Libelle { get; set; }
        [Column(Name = "CODEBRANCHE")]
        public string CodeBranche { get; set; }
        [Column(Name = "LIBBRANCHE")]
        public string LibBranche { get; set; }
        [Column(Name = "CODECIBLE")]
        public string CodeCible { get; set; }
        [Column(Name = "LIBCIBLE")]
        public string LibCible { get; set; }
        [Column(Name = "CODETYPO")]
        public string CodeTypologie { get; set; }
        [Column(Name = "LIBTYPO")]
        public string LibTypologie { get; set; }
        [DataMember]
        [Column(Name = "IDVALEUR")]
        public Int32 IdValeur { get; set; }
        [DataMember]
        public BrancheDto Branche { get; set; }
        [DataMember]
        public ParametreDto Typologie { get; set; }
        [DataMember]
        public List<GrilleDto> Grilles { get; set; }
    }
}
