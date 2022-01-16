using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Vsto
{
    [DataContract]
    public class BoGestionClauseSignetDto
    {
        /// <summary>
        /// Code
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Libelle
        /// </summary>
        [DataMember]
        public string Libelle { get; set; }

        /// <summary>
        /// Rubrique 1
        /// </summary>
        [DataMember]
        public int IdRubrique1 { get; set; }

        /// <summary>
        /// Rubrique 2
        /// </summary>
        [DataMember]
        public int IdRubrique2 { get; set; }

        /// <summary>
        /// Rubrique 3
        /// </summary>
        [DataMember]
        public int IdRubrique3 { get; set; }

        /// <summary>
        /// Type d'insertion
        /// </summary>
        [DataMember]
        public string TypeInsertion { get; set; }

        /// <summary>
        /// Id Mot Clé
        /// </summary>
        [DataMember]
        public int IdMotCle { get; set; }

        /// <summary>
        /// Observations
        /// </summary>
        [DataMember]
        public string Observations { get; set; }

        /// <summary>
        /// Rubrique1
        /// </summary>
        [DataMember]
        public BoGestionClauseSignetRubriqueDto BoGestionClauseSignetRubriqueDto1;

        /// <summary>
        /// Rubrique2
        /// </summary>
        [DataMember]
        public BoGestionClauseSignetRubriqueDto BoGestionClauseSignetRubriqueDto2;

        /// <summary>
        /// Rubrique3
        /// </summary>
        [DataMember]
        public BoGestionClauseSignetRubriqueDto BoGestionClauseSignetRubriqueDto3;

        /// <summary>
        /// Mots-clés
        /// </summary>
        [DataMember]
        public List<BoGestionClauseMotCleDto> ListBoGestionClauseMotCleDto { get; set; }
    }
}
