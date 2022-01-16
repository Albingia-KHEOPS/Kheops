using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Attestation
{
    public class AttestationGarantiePlatDto
    {
        [Column(Name = "CODERSQ")]
        public string CodeRSq { get; set; }
        [Column(Name = "CODEOBJ")]
        public string CodeObj { get; set; }
        [Column(Name = "LETTREFOR")]
        public string LettreFor { get; set; }
        [Column(Name = "CODEGAR1")]
        public string CodeGarantie1 { get; set; }
        [Column(Name = "LIBGAR1")]
        public string LibelleGarantie1 { get; set; }
        [Column(Name = "MONTANT1")]
        public string Montant1 { get; set; }
        [Column(Name = "DATEDEB1")]
        public string DateDeb1 { get; set; }
        [Column(Name = "DATEFIN1")]
        public string DateFin1 { get; set; }
        [Column(Name = "CODEGAR2")]
        public string CodeGarantie2 { get; set; }
        [Column(Name = "LIBGAR2")]
        public string LibelleGarantie2 { get; set; }
        [Column(Name = "MONTANT2")]
        public string Montant2 { get; set; }
        [Column(Name = "DATEDEB2")]
        public string DateDeb2 { get; set; }
        [Column(Name = "DATEFIN2")]
        public string DateFin2 { get; set; }
        [Column(Name = "CODEGAR3")]
        public string CodeGarantie3 { get; set; }
        [Column(Name = "LIBGAR3")]
        public string LibelleGarantie3 { get; set; }
        [Column(Name = "MONTANT3")]
        public string Montant3 { get; set; }
        [Column(Name = "DATEDEB3")]
        public string DateDeb3 { get; set; }
        [Column(Name = "DATEFIN3")]
        public string DateFin3 { get; set; }
        [Column(Name = "CODEGAR4")]
        public string CodeGarantie4 { get; set; }
        [Column(Name = "LIBGAR4")]
        public string LibelleGarantie4 { get; set; }
        [Column(Name = "MONTANT4")]
        public string Montant4 { get; set; }
        [Column(Name = "DATEDEB4")]
        public string DateDeb4 { get; set; }
        [Column(Name = "DATEFIN4")]
        public string DateFin4 { get; set; }
    }
}
