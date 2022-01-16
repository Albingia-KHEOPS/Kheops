using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.GarantieModele;

namespace OP.WSAS400.DTO.Modeles
{
    [DataContract]
    public class ModeleDto : _Modele_Base
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime? DateCreation { get; set; }
        [DataMember]
        public string Caractere { get; set; }
        [DataMember]
        public string GuidId { get; set; }

        [DataMember]
        public List<GarantieModeleNiveau1Dto> Modeles { get; set; }

        public ModeleDto()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
            this.DateCreation = null;
            this.Caractere = string.Empty;
            this.GuidId = string.Empty;
            this.Modeles = new List<GarantieModeleNiveau1Dto>();
        }



    }
}
