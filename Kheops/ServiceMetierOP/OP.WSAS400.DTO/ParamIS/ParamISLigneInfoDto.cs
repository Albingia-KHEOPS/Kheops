using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ParamIS
{
    public class ParamISLigneInfoDto
    {

        [Column(Name = "MODELEID")]
        public string ModeleID { get; set; }
        [DataMember]
        [Column(Name = "INTERNALPROPERTYNAME")]
        public string InternalPropertyName { get; set; }

        [DataMember]
        [Column(Name = "CODE")]
        public int Code { get; set; }

        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Cells { get; set; }

        [DataMember]
        [Column(Name = "LIBELLE")]
        public string TextLabel { get; set; }

        [DataMember]
        [Column(Name = "TYPEZONE")]
        public string Type { get; set; }

        [DataMember]
        [Column(Name = "LONGUEURZONE")]
        public string LongueurType { get; set; }

        [DataMember]
        [Column(Name = "MAPPAGE")]
        public string Link { get; set; }

        [DataMember]
        [Column(Name = "CONVERSION")]
        public string ConvertTo { get; set; }

        [DataMember]
        [Column(Name = "PRESENTATION")]
        public int HierarchyOrder { get; set; }

        [DataMember]
        [Column(Name = "TYPEUI")]
        public string TypeUIControl { get; set; }

        [DataMember]
        [Column(Name = "OBLIGATOIRE")]
        public string Required { get; set; }

        [DataMember]
        [Column(Name = "AFFICHAGE")]
        public string ScriptAffichage { get; set; }

        [DataMember]
        [Column(Name = "CONTROLE")]
        public string ScriptControle { get; set; }

        [DataMember]
        [Column(Name = "OBSERVATION")]
        public string Observation { get; set; }

        [DataMember]
        [Column(Name = "DBMAPCOL")]
        public string DbMapCol { get; set; }

        [DataMember]
        [Column(Name = "NUMAFFICHAGE")]
        public float NumOrdreAffichage { get; set; }

        [DataMember]
        [Column(Name = "SAUTLIGNE")]
        public int LineBreak { get; set; }

        [DataMember]
        [Column(Name = "MODIFIABLE")]
        public string Disabled { get; set; }

        [DataMember]
        [Column(Name = "SQLREQUEST")]
        public string SqlRequest { get; set; }

        [DataMember]
        [Column(Name = "SQLORDER")]
        public int SqlOrder { get; set; }

        [DataMember]
        [Column(Name = "LIENPARENT")]
        public string LinkBehaviour { get; set; }

        [DataMember]
        [Column(Name = "PARENTCMPT")]
        public string Behaviour { get; set; }

        [DataMember]
        [Column(Name = "PARENTEVT")]
        public string EventBehaviour { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public bool AffichageIs { get; set; }
    }
}
