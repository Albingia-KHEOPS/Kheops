using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.MenuContextuel
{
    public class ContextMenuPlatDto
    {
        [DataMember]
        [Column(Name = "USER")]
        public string Utilisateur { get; set; }
        [DataMember]
        [Column(Name = "MENU")]
        public string Menu { get; set; }
        [DataMember]
        [Column(Name = "MENUICO")]
        public string MenuIco { get; set; }
        [DataMember]
        [Column(Name = "OPTION")]
        public Int32 Option { get; set; }
        [DataMember]
        [Column(Name = "OPTIONLIB")]
        public string OptionLib { get; set; }
        [DataMember]
        [Column(Name = "ACTION")]
        public string Action { get; set; }
        [DataMember]
        [Column(Name = "DENIEDROLE")]
        public Int32 DeniedRole { get; set; }
        [DataMember]
        [Column(Name = "DENIEDUSER")]
        public Int32 DeniedUser { get; set; }
        [DataMember]
        [Column(Name = "BRCHE")]
        public string AlwBranche { get; set; }
        [DataMember]
        [Column(Name = "TYPE")]
        public string TypeOffreContrat { get; set; }
        [DataMember]
        [Column(Name = "ETAT")]
        public string AlwEtat { get; set; }
        [DataMember]
        [Column(Name = "ORDERBY")]
        public string Orderby { get; set; }
    }
}
