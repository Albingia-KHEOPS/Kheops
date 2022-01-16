using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParamIS
{
    [DataContract]
    public class LigneModeleISDto
    {
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string LibelleAffiche { get; set; }
        [DataMember]
        [Column(Name = "TYPEZONE")]
        public string TypeZone { get; set; }
        [DataMember]
        [Column(Name = "LONGUEURZONE")]
        public string LongueurZone { get; set; }
        [DataMember]
        [Column(Name = "MAPPAGE")]
        public string Mappage { get; set; }
        [DataMember]
        [Column(Name = "CONVERSION")]
        public string Conversion { get; set; }
        [DataMember]
        [Column(Name = "PRESENTATION")]
        public int Presentation { get; set; }
        [DataMember]
        [Column(Name = "TYPEUI")]
        public string TypeUI { get; set; }
        [DataMember]
        [Column(Name = "OBLIGATOIRE")]
        public string Obligatoire { get; set; }
        [DataMember]
        [Column(Name = "AFFICHAGE")]
        public string Affichage { get; set; }
        [DataMember]
        [Column(Name = "CONTROLE")]
        public string Controle { get; set; }
        [DataMember]
        [Column(Name = "OBSERVATION")]
        public string Observation { get; set; }
        [DataMember]
        [Column(Name = "TCON")]
        public string Tcon { get; set; }
        [DataMember]
        [Column(Name = "TFAM")]
        public string Tfam { get; set; }

        [DataMember]
        [Column(Name = "VALEURDEFAUT")]
        public string ValeurDefaut { get; set; }

        [DataMember]
        public string TypeOperation { get; set; }
    }
}
