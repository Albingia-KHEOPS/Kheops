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
    public class GrilleDto
    {
        [DataMember]
        [Column(Name="CODE")]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [DataMember]
        [Column(Name = "TYPOGRILLE")]
        public string TypologieGrille { get; set; }
        [DataMember]
        public Int32 Id { get; set; }
        [DataMember]
        public List<TypologieDto> Typologies { get; set; }
        [DataMember]
        public List<ParametreDto> LstTypologie { get; set; }
        [DataMember]
        public List<ParametreDto> LstLien { get; set; }

        [DataMember]
        public List<NomenclatureDto> Nomenclatures { get; set; }
        [DataMember]
        public string Typologie { get; set; }
        [DataMember]
        public string Niveau { get; set; }

        [DataMember]
        public bool CibleLiee { get; set; }
        [DataMember]
        public List<CibleDto> Cibles { get; set; }
    }
}
