using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.GestionDocument
{
    public class DocumentParametresTexteDto
    {
        [Column(Name="CODETEXTE")]
        public int CodeTexte { get; set; }
        [Column(Name="TEXTE")]
        public string Texte { get; set; }
    }
}
