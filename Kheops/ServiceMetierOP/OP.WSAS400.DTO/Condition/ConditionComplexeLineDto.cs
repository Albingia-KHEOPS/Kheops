using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Condition
{
    public class ConditionComplexeLineDto
    {
        //[Column(Name = "IDEXPR")]
        //public long Id { get; set; }
        //[Column(Name = "LIBEXPR")]
        //public string Libelle { get; set; }
        //[Column(Name = "IDDETAIL")]
        //public long IdDetail { get; set; }
        //[Column(Name = "ORDRE")]
        //public long Ordre { get; set; }
        //[Column(Name = "VALEUR")]
        //public double Valeur { get; set; }
        //[Column(Name = "UNITE")]
        //public string Unite { get; set; }
        //[Column(Name = "TYPE")]
        //public string Type { get; set; }

        ////Champ LCI
        //[Column(Name = "VALEURCONCURRENCE")]
        //public double ValeurConcurrence { get; set; }
        //[Column(Name = "UNITECONCURRENCE")]
        //public string UniteConcurrence { get; set; }
        //[Column(Name = "TYPECONCURRENCE")]
        //public string TypeConcurrence { get; set; }

        //[Column(Name = "DESCEXPR")]
        //public string Descriptif { get; set; }
        //[Column(Name = "CODEEXPR")]
        //public string CodeExpr { get; set; }
        //[Column(Name = "TYPEEXPR")]
        //public string TypeExpr { get; set; }


        ////Champ Franchise
        //[Column(Name = "MINI")]
        //public string Mini { get; set; }
        //[Column(Name = "MAXI")]
        //public string Maxi { get; set; }
        //[Column(Name = "DEBUT")]
        //public string Debut { get; set; }
        //[Column(Name = "FIN")]
        //public string Fin { get; set; }


        [Column(Name = "MINI")]
        public double Mini { get; set; }
        [Column(Name = "MAXI")]
        public double Maxi { get; set; }
        [Column(Name = "DEBUT")]
        public double Debut { get; set; }
        [Column(Name = "FIN")]
        public double Fin { get; set; }

        [Column(Name = "IDEXPR")]
        public long Id { get; set; }
        [Column(Name = "LIBEXPR")]
        public string Libelle { get; set; }
        [Column(Name = "IDDETAIL")]
        public long IdDetail { get; set; }
        [Column(Name = "ORDRE")]
        public long Ordre { get; set; }
        [Column(Name = "VALEUR")]
        public double Valeur { get; set; }
        [Column(Name = "UNITE")]
        public string Unite { get; set; }
        [Column(Name = "TYPE")]
        public string Type { get; set; }

        //Champ LCI
        [Column(Name = "VALEURCONCURRENCE")]
        public double ValeurConcurrence { get; set; }
        [Column(Name = "UNITECONCURRENCE")]
        public string UniteConcurrence { get; set; }
        [Column(Name = "TYPECONCURRENCE")]
        public string TypeConcurrence { get; set; }

        [Column(Name = "DESCEXPR")]
        public string Descriptif { get; set; }
        [Column(Name = "CODEEXPR")]
        public string CodeExpr { get; set; }
        [Column(Name = "TYPEEXPR")]
        public string TypeExpr { get; set; }


    }
}
