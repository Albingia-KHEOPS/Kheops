using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Condition
{
    [DataContract]
    public class EnsembleGarantieDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the id sequence.
        /// </summary>
        /// <value>
        /// The id sequence.
        /// </value>
        [DataMember]
        public string IdSequence { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the couleur1.
        /// </summary>
        /// <value>
        /// The couleur1.
        /// </value>
        [DataMember]
        public string Couleur1 { get; set; }
        /// <summary>
        /// Gets or sets the couleur2.
        /// </summary>
        /// <value>
        /// The couleur2.
        /// </value>
        [DataMember]
        public string Couleur2 { get; set; }
        /// <summary>
        /// Gets or sets the LST ligne garantie.
        /// </summary>
        /// <value>
        /// The LST ligne garantie.
        /// </value>
        [DataMember]
        public List<LigneGarantieDto> LstLigneGarantie { get; set; }
        /// <summary>
        /// Gets or sets the type complexe.
        /// </summary>
        /// <value>
        /// The type complexe.
        /// </value>
        [DataMember]
        public string TypeComplexe { get; set; }

        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public string UniteNew { get; set; }
        [DataMember]
        public List<ParametreDto> UnitesNew { get; set; }
        [DataMember]
        public string TypeNew { get; set; }
        [DataMember]
        public List<ParametreDto> TypesNew { get; set; }
        [DataMember]
        public string UniteConcurrence { get; set; }
        [DataMember]
        public List<ParametreDto> UnitesConcurrence { get; set; }
        [DataMember]
        public string TypeConcurrence { get; set; }
        [DataMember]
        public List<ParametreDto> TypesConcurrence { get; set; }
        [DataMember]
        public string Descriptif { get; set; }

        [DataMember]
        public List<ParametreDto> LCIUnites { get; set; }
        [DataMember]
        public List<ParametreDto> LCITypes { get; set; }
        [DataMember]
        public List<ParametreDto> FranchiseUnites { get; set; }
        [DataMember]
        public List<ParametreDto> FranchiseTypes { get; set; }
        [DataMember]
        public List<ParametreDto> AssietteUnites { get; set; }
        [DataMember]
        public List<ParametreDto> AssietteTypes { get; set; }
        [DataMember]
        public List<ParametreDto> TauxForfaitHTUnites { get; set; }

        [DataMember]
        public string Niveau { get; set; }
        [DataMember]
        public string Pere { get; set; }
        [DataMember]
        public string Sequence { get; set; }
        [DataMember]
        public string Origine { get; set; }

        [DataMember]
        public string CodeBloc { get; set; }
        [DataMember]
        public string CodeModele { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }

        [DataMember]
        public string CVolet { get; set; }
        [DataMember]
        public string CBloc { get; set; }
        [DataMember]
        public string LVolet { get; set; }
        [DataMember]
        public string LBloc { get; set; }

        [DataMember]
        public bool IsGarantieSortie { get; set; }

        [DataMember]
        public bool IsAttentatGareat { get; set; }

        public Single TriVolet { get; set; }
        public Single TriBloc { get; set; }
        //public Int64 TriVolet { get; set; }
        //public Int64 TriBloc { get; set; }
        public Int64 TriDate { get; set; }
        public string TriGar { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnsembleGarantieDto"/> class.
        /// </summary>
        public EnsembleGarantieDto()
        {
            LstLigneGarantie = new List<LigneGarantieDto>();
            UnitesNew = new List<ParametreDto>();
            TypesNew = new List<ParametreDto>();
            UnitesConcurrence = new List<ParametreDto>();
            TypesConcurrence = new List<ParametreDto>();
        }
    }

}