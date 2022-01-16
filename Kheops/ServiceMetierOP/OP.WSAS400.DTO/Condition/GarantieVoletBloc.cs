using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Condition
{
    public class GarantieVoletBloc
    {
        [Column(Name="CODE")]
        public Int64 Code { get; set; }
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        [Column(Name = "CODEVOLET")]
        public Int64 CodeVolet { get; set; }
        [Column(Name = "CODEBLOC")]
        public Int64 CodeBloc { get; set; }
        [Column(Name = "CODEMODELE")]
        public Int64 CodeModele { get; set; }
    }
}
