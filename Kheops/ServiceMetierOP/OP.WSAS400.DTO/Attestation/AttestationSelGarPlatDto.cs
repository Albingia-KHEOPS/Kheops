using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Attestation
{
    public class AttestationSelGarPlatDto
    {
        [Column(Name = "IDGARAN")]
        public Int64 IdGaran { get; set; }
        [Column(Name = "CODERSQ")]
        public Int32 CodeRsq { get; set; }
        [Column(Name = "CODEOBJ")]
        public Int32 CodeObj { get; set; }
        [Column(Name = "LETTREFOR")]
        public string LettreFor { get; set; }
        [Column(Name = "LIBFOR")]
        public string LibFor { get; set; }
        [Column(Name = "CODEGARAN")]
        public string CodeGaran { get; set; }
        [Column(Name = "LIBGARAN")]
        public string LibGaran { get; set; }
        [Column(Name = "NIVGARAN")]
        public Int32 NivGaran { get; set; }
        [Column(Name = "SEQGARAN")]
        public Int64 SeqGaran { get; set; }
        [Column(Name = "MASTERGARAN")]
        public Int64 MasterGaran { get; set; }
        [Column(Name = "NATUREGARAN")]
        public string NatureGaran { get; set; }
        [Column(Name = "VALGARAN")]
        public Double ValGaran { get; set; }
        [Column(Name = "UNITGARAN")]
        public string UnitGaran { get; set; }
        [Column(Name = "LIBUNTGAR")]
        public string LibUntGar { get; set; }
        [Column(Name = "BASEGARAN")]
        public string BaseGaran { get; set; }
        [Column(Name = "DATEDEBGARAN")]
        public Int64 DateDebGaran { get; set; }
        [Column(Name = "DATEFINGARAN")]
        public Int64 DateFinGaran { get; set; }
        [Column(Name = "DUREEGARAN")]
        public Int32 DureeGaran { get; set; }
        [Column(Name = "DURUNITEGARAN")]
        public string DurUnitGaran { get; set; }
        [Column(Name = "DATEWDGARAN")]
        public Int64 DateWDGaran { get; set; }
        [Column(Name = "DATEWFGARAN")]
        public Int64 DateWFGaran { get; set; }

        [Column(Name = "TYPEPORTEE")]
        public string TypePortee { get; set; }
        [Column(Name = "CODEOBJPORTEE")]
        public Int32 CodeObjPortee { get; set; }
        [Column(Name = "FORMUSE")]
        public string FormUse { get; set; }
        [Column(Name = "GARUSE")]
        public string GarUse { get; set; }
    }
}
