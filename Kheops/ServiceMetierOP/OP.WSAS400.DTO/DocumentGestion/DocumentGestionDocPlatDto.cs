using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.DocumentGestion
{
    public class DocumentGestionDocPlatDto
    {
        [Column(Name = "IDLOT")]
        public Int64 DocId { get; set; }
        [Column(Name = "IDLOTSDETAIL")]
        public Int64 IdLotDetail { get; set; }
        [Column(Name = "SITUATION")]
        public string Situation { get; set; }
        [Column(Name = "NATURE")]
        public string Nature { get; set; }
        [Column(Name = "IMPRIMABLE")]
        public string Imprimable { get; set; }
        [Column(Name = "CHEMIN")]
        public string Chemin { get; set; }
        [Column(Name = "STATUT")]
        public string Statut { get; set; }
        [Column(Name = "TYPEDOC")]
        public string TypeDoc { get; set; }
        [Column(Name="IDDOC")]
        public Int64 IdDoc { get; set; }
        [Column(Name = "NOMDOC")]
        public string NomDoc { get; set; }
        [Column(Name="LIBDOC")]
        public string LibDoc { get; set; }
        [Column(Name = "TYPEDEST")]
        public string TypeDest { get; set; }
        [Column(Name = "IDDEST")]
        public Int64 IdDest { get; set; }
        [Column(Name = "TYPEENVOI")]
        public string TypeEnvoi { get; set; }
        [Column(Name = "LIBTYPEENVOI")]
        public string LibTypeEnvoi { get; set; }
        [Column(Name = "NBEXEMPL")]
        public Int64 NbExempl { get; set; }
        [Column(Name="NBEXEMPLSUPP")]
        public Int64 NbExemplSupp { get; set; }
        [Column(Name = "TAMPON")]
        public string Tampon { get; set; }
        [Column(Name = "LIBTAMPON")]
        public string LibTampon { get; set; }
        [Column(Name="IDLETTRE")]
        public Int64 IdLettre { get; set; }
        [Column(Name = "TYPELETTRE")]
        public string TypeLettre { get; set; }
        [Column(Name="NOMLETTRE")]
        public string NomLettre { get; set; }
        [Column(Name="LIBLETTRE")]
        public string LibLettre { get; set; }
        [Column(Name = "LOTMAIL")]
        public Int32 LotMail { get; set; }
        [Column(Name="ISLIBRE")]
        public string IsLibre { get; set; }
    }
}
