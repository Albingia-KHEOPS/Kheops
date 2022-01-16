using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreFamilles
{
    public class ParamFamilleDto
    {

        [DataMember]
        [Column(Name = "CODECONCPET")]
        public string CodeConcpet { get; set; }
        [DataMember]
        [Column(Name = "CODEFAMILLE")]   
        public string CodeFamille { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEFAMILLE")]   
        public string LibelleFamille { get; set; }
        [DataMember]
        [Column(Name = "LONGEUR")]   
        public Int16 Longueur { get; set; }
        [DataMember]
        [Column(Name = "TYPECODE")]   
        public string TypeCode { get; set; }
        [DataMember]
        [Column(Name = "NULLVALUE")]   
        public string NullValue { get; set; }
        [DataMember]
        [Column(Name = "LIBELLECOURTNUM1")]   
        public string LibelleCourtNum1 { get; set; }
        [DataMember]
        [Column(Name = "LIBELLELONGNUM1")]   
        public string LibelleLongNum1 { get; set; }
        [DataMember]
        [Column(Name = "TYPENUM1")]   
        public string TypeNum1 { get; set; }
        [DataMember]
        [Column(Name = "NBRDECIMAL1")]   
        public Int16 NbrDecimal1 { get; set; }
        [DataMember]
        [Column(Name = "LIBELLECOURTNUM2")]   
        public string LibelleCourtNum2 { get; set; }
        [DataMember]
        [Column(Name = "LIBELLELONGNUM2")]   
        public string LibelleLongNum2 { get; set; }
        [DataMember]
        [Column(Name = "TYPENUM2")]   
        public string TypeNum2 { get; set; }
        [DataMember]
        [Column(Name = "NBRDECIMAL2")]
        public Int16 NbrDecimal2 { get; set; }
        [DataMember]
        [Column(Name = "LIBELLECOURTALPHA1")]   
        public string LibelleCourtAlpha1 { get; set; }
        [DataMember]
        [Column(Name = "LIBELLELONGALPHA1")]   
        public string LibelleLongAlpha1 { get; set; }
        [DataMember]
        [Column(Name = "LIBELLECOURTALPHA2")]   
        public string LibelleCourtAlpha2 { get; set; }
        [DataMember]
        [Column(Name = "LIBELLELONGALPHA2")]   
        public string LibelleLongAlpha2 { get; set; }
        [DataMember]
        [Column(Name = "RESTRICTION")]   
        public string Restriction { get; set; }         
    }
}
