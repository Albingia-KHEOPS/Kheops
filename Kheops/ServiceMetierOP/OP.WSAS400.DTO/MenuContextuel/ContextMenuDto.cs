using ALBINGIA.Framework.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.MenuContextuel
{

    [DataContract]
    public class UsersContextMenuDto
    {
        [DataMember]
        public string Utilisateur { get; set; }
        [DataMember]
        public List<ContextMenuDto> UserContextMenu { get; set; }
    }
    [DataContract]
    public class ContextMenuDto
    {
        [DataMember]
        public AlbContextMenu menu { get; set; }
        [DataMember]
        public string text { get; set; }
        [DataMember]
        public string icon { get; set; }
        [DataMember]
        public string alias { get; set; }
        [DataMember]
        public string action { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string width { get; set; }
        /// <summary>
        /// "CO" pour la branche Construction
        /// "HCO" pour toutes les branches sauf Construction
        /// "*" pour toutes les branches
        /// </summary>
        [DataMember]
        public string AlwBranche { get; set; }
        /// <summary>
        /// "O" pour menu uniquement offre
        /// "C" pour menu uniquement contrat
        /// "*" pour menu pour offre et contrat
        /// Autre pour menu non disponible
        /// </summary>
        [DataMember]
        public string typeOffreContrat { get; set; }
        [DataMember]
        public string AlwEtat { get; set; }
        [DataMember]
        public int orderby { get; set; }
        [DataMember]
        public List<ContextMenuDto> items { get; set; }
    }
}
