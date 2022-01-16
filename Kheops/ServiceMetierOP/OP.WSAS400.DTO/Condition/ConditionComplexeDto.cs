using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Condition
{
    [DataContract]
    public class ConditionComplexeDto
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public string Descriptif { get; set; }
        /// <summary>
        /// Gets or sets the expressions.
        /// </summary>
        /// <value>
        /// The expressions.
        /// </value>

        [DataMember]
        public string UniteNew { get; set; }
        [DataMember]
        public string TypeNew { get; set; }
        [DataMember]
        public string UniteConcurrence { get; set; }
        [DataMember]
        public string TypeConcurrence { get; set; }
        [DataMember]
        public List<ParametreDto> UnitesNew { get; set; }
        [DataMember]
        public List<ParametreDto> TypesNew { get; set; }
        [DataMember]
        public List<ParametreDto> UnitesConcurrence { get; set; }
        [DataMember]
        public List<ParametreDto> TypesConcurrence { get; set; }
        [DataMember]
        //public List<ParametreDto> Expressions { get; set; }
        public List<ConditionComplexeDto> Expressions { get; set; }
        [DataMember]
        public List<LigneGarantieDto> LstLigneGarantie { get; set; }

        [DataMember]
        public bool Modifiable { get; set; }
        [DataMember]
        public string Origine { get; set; }
        [DataMember]
        public bool Utilise { get; set; }

        public ConditionComplexeDto()
        {
            //Expressions = new List<ParametreDto>();
            Expressions = new List<ConditionComplexeDto>();
            LstLigneGarantie = new List<LigneGarantieDto>();
            UnitesConcurrence = new List<ParametreDto>();
            TypesConcurrence = new List<ParametreDto>();
            UnitesNew = new List<ParametreDto>();
            TypesNew = new List<ParametreDto>();
        }

        public string TypeComplexe { get; set; }

    }
}
