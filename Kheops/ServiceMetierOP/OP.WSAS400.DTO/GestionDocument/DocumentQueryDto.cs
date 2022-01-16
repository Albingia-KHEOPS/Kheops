using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.GestionDocument
{
    public class DocumentQueryDto
    {
        [Column(Name = "CODEDOCUMENT")]
        public int CodeDocument { get; set; }
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [Column(Name = "DIFFUSION")]
        public string Diffusion { get; set; }
        [Column(Name = "TYPEPARTENAIRE")]
        public string TypePartenaire { get; set; }
        [Column(Name = "CODEDESTINATAIRE")]
        public int CodeDestinataire { get; set; }
        [Column(Name = "NOMASSURE")]
        public string NomAssure { get; set; }
        [Column(Name = "NOMCOURTIER")]
        public string NomCourtier { get; set; }
        [Column(Name = "DATECREATION")]
        public int DateCreation { get; set; }
        [Column(Name = "CODESITUATION")]
        public int CodeSituation { get; set; }
        [Column(Name = "DATESITUATION")]
        public int DateSituation { get; set; }
    }
}
