using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Common
{
    [DataContract]
    public class IdContratDto
    {
        [DataMember]
        [Column(Name = "IPB")]
        public virtual string CodeOffre { get; set; }

        [DataMember]
        [Column(Name = "ALX")]
        public virtual int Version { get; set; }

        [DataMember]
        [Column(Name = "TYP")]
        public virtual string Type { get; set; }

        [DataMember]
        [Column(Name = "LIBTYPECONTRAT")]
        public virtual string LibTypeContrat { get; set; }

        [DataMember]
        [Column(Name = "TYPECONTRAT")]
        public string TypeContrat { get; set; }

        [DataMember]
        public bool IsHisto { get; set; }

        public override string ToString()
        {
            return string.Join("_", new string[] { (CodeOffre ?? string.Empty) });
        }

        public Folder ToFolder() {
            return new Folder {
                CodeOffre = CodeOffre,
                Version = Version,
                Type = Type
            };
        }
    }
}
