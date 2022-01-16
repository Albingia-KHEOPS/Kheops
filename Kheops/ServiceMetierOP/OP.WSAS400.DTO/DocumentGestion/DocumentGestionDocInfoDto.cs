using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.DocumentGestion
{
    [DataContract]
    public class DocumentGestionDocInfoDto
    {
        [DataMember]
        public Int64 LotId { get; set; }
        [DataMember]
        public string Situation { get; set; }
        [DataMember]
        public string Nature { get; set; }
        [DataMember]
        public string Imprimable { get; set; }
        [DataMember]
        public string Chemin { get; set; }
        [DataMember]
        public string Statut { get; set; }
        [DataMember]
        public string TypeDoc { get; set; }
        [DataMember]
        public Int64 IdDoc { get; set; }
        [DataMember]
        public string NomDoc { get; set; }
        [DataMember]
        public string LibDoc { get; set; }
        [DataMember]
        public Int64 IdLotDetail { get; set; }
        [DataMember]
        public string TypeDestinataire { get; set; }
        [DataMember]
        public Int64 Destinataire { get; set; }
        [DataMember]
        public string CodeTypeEnvoi { get; set; }
        [DataMember]
        public string TypeEnvoi { get; set; }
        [DataMember]
        public Int64 NbExemple { get; set; }
        [DataMember]
        public Int64 NbExempleSupp { get; set; }
        [DataMember]
        public string Tampon { get; set; }
        [DataMember]
        public string LibTampon { get; set; }
        [DataMember]
        public Int64 IdLettre { get; set; }
        [DataMember]
        public string TypeLettre { get; set; }
        [DataMember]
        public string LettreAccomp { get; set; }
        [DataMember]
        public string LibLettre { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public bool IsLibre { get; set; }
    }
}
