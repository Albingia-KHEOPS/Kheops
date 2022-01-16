using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.GestionDocument
{
    public class LettreTypeDto
    {
        [Column(Name="NUMVERSION")]
        public int NumVersion { get; set; }
        [Column(Name="LIBELLE")]
        public string Libelle { get; set; }
    }
}
