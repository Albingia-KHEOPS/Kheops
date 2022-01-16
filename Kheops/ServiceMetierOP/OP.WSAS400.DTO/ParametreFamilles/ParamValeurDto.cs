using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ParametreFamilles
{
    public class ParamValeurDto
    {
        [DataMember]
        [Column(Name = "CODECONCPET")]
        public string CodeConcpet { get; set; }
        [DataMember]
        [Column(Name = "CODEFAMILLE")]
        public string CodeFamille { get; set; }
        [DataMember]
        [Column(Name = "CODEVALEUR")]
        public string CodeValeur { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEVALEUR")]
        public string LibelleValeur { get; set; }
        [DataMember]
        [Column(Name = "LIBELLELONGVALEUR")]
        public string LibelleLongValeur { get; set; }
        [DataMember]               
        [Column(Name = "DESCRIPTION1")]
        public string Description1 { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTION2")]
        public string Description2 { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTION3")]
        public string Description3 { get; set; }
        [DataMember]
        [Column(Name = "VALEURNUM1")]
        public Double ValeurNum1 { get; set; }
        [DataMember]
        [Column(Name = "VALEURNUM2")]
        public Double ValeurNum2 { get; set; }
        [DataMember]
        [Column(Name = "VALEURALPHA1")]
        public string ValeurAlpha1 { get; set; }
        [DataMember]
        [Column(Name = "VALEURALPHA2")]
        public string ValeurAlpha2 { get; set; }
        [Column(Name = "CODEFILTRE")]
        public string CodeFiltre { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEFILTRE")]
        public string LibelleFiltre { get; set; }
        [DataMember]
        [Column(Name = "RESTRICTION")]
        public string Restriction { get; set; }
    }
}
