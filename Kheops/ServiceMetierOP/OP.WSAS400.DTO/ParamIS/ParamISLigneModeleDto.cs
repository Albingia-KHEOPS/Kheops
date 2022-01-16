using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ParamIS
{
    public class ParamISLigneModeleDto
    {
        [DataMember]
        [Column(Name = "ID")]
        public int GuidId { get; set; }

        [DataMember]
        [Column(Name = "MODELE")]
        public string ModeleIS { get; set; }

        [DataMember]
        [Column(Name = "REFERENTIEL")]
        public string Referentiel { get; set; }

        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }

        [DataMember]
        [Column(Name = "NUMAFFICHAGE")]
        public float NumOrdreAffichage { get; set; }

        [DataMember]
        [Column(Name = "SAUTLIGNE")]
        public int SautDeLigne { get; set; }

        [DataMember]
        [Column(Name = "MODIFIABLE")]
        public string Modifiable { get; set; }

        [DataMember]
        [Column(Name = "OBLIGATOIRE")]
        public string Obligatoire { get; set; }
        

        [DataMember]
        [Column(Name = "TCON")]
        public string Tcon { get; set; }

        [DataMember]
        [Column(Name = "TFAM")]
        public string Tfam { get; set; }
        

        [DataMember]
        [Column(Name = "AFFICHAGE")]
        public string Affichage { get; set; }

        [DataMember]
        [Column(Name = "CONTROLE")]
        public string Controle { get; set; }

        [DataMember]
        [Column(Name = "LIENPARENT")]
        public int LienParent { get; set; }

        [DataMember]
        [Column(Name = "PARENTCMPT")]
        public string ParentComportement { get; set; }

        [DataMember]
        [Column(Name = "PARENTEVT")]
        public string ParentEvenement { get; set; }

        [DataMember]
        [Column(Name = "PRESENTATION")]
        public int Presentation { get; set; }

        [DataMember]
        [Column(Name = "TYPEUI")]
        public string TypeUI { get; set; }

        [DataMember]
        [Column(Name = "LIENPARENTLIBELLE")]
        public string LienParentLibelle { get; set; }

        [DataMember]
        [Column(Name = "SCRIPTAFF")]
        public string ScriptAffichage { get; set; }

        [DataMember]
        [Column(Name = "SCRIPTCTRL")]
        public string ScriptControle { get; set; }

        public string TypeOperation { get; set; }
    }
}
