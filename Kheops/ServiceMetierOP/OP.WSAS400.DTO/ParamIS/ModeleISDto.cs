using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ParamIS
{
    [DataContract]
    public class ModeleISDto
    {
        [DataMember]
        [Column(Name = "NOM")]
        public string NomModele { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember]
        [Column(Name = "DATEDEB")]
        public int DateDebut { get; set; }
        [DataMember]
        [Column(Name = "DATEFIN")]
        public int DateFin { get; set; }
        [DataMember]
        [Column(Name = "SELECT")]
        public string SqlSelect { get; set; }
        [DataMember]
        [Column(Name = "INSERT")]
        public string SqlInsert { get; set; }
        [DataMember]
        [Column(Name = "UPDATE")]
        public string SqlUpdate { get; set; }
        [DataMember]
        [Column(Name = "EXIST")]
        public string SqlExist { get; set; }
        [DataMember]
        [Column(Name = "SCRIPTAFF")]
        public string ScriptAffichage { get; set; }
        [DataMember]
        [Column(Name = "SCRIPTCTRL")]
        public string ScriptControle { get; set; }
        [DataMember]
        public string TypeOperation { get; set; }
    }
}
