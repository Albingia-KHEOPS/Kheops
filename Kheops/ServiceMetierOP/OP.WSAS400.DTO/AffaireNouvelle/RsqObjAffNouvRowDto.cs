using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;
using ALBINGIA.Framework.Common.Extensions;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class RsqObjAffNouvRowDto
    {
        [DataMember]
        [Column(Name = "CODECONTRAT")]
        public string CodeContrat { get; set; }
        [DataMember]
        [Column(Name = "VERSIONCONTART")]
        public Int64 VersionContrat { get; set; }
        [DataMember]
        [Column(Name = "CODERSQ")]
        public Int64 CodeRsq { get; set; }
        [DataMember]
        [Column(Name = "CODEOBJ")]
        public Int64 CodeObj { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [Column(Name = "CHECKROW")]
        public string CheckRowDb { get; set; }
        [DataMember]
        [Column(Name = "TYPEENREG")]
        public string TypeEnr { get; set; }

        [DataMember]
        public bool CheckRow
        {
            get
            {
                return CheckRowDb.AsBoolean() ?? false;
            }
            set
            {
                CheckRowDb = value ? "O" : "N";
            }
        }
    }
}
