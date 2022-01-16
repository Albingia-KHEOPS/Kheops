using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Stat
{
    [DataContract]
    public class StatOffreContratFiltreDto
    {
        [DataMember]
        public string Branche { get; set; }

        [DataMember]
        public string Categorie { get; set; }

        [DataMember]
        public string Situation { get; set; }

        [DataMember]
        public string Etat { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Annee { get; set; }

        [DataMember]
        public string Mois { get; set; }

        [DataMember]
        public string Jour { get; set; }
    }
}
