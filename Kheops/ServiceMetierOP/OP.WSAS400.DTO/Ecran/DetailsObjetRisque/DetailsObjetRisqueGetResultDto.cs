using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.Ecran.DetailsObjetRisque
{
    [DataContract]
    public class DetailsObjetRisqueGetResultDto //: _DetailsObjetRisque_Base, IResult
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        //[DataMember]
        //public enIOAS400Results Result { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        //[DataMember]
        //public string Message { get; set; }

        ///// <summary>
        ///// Gets or sets the receive date.
        ///// </summary>
        ///// <value>
        ///// The receive date.
        ///// </value>
        //[DataMember]
        //public DateTime ReceiveDate { get; set; }

        ///// <summary>
        ///// Gets or sets the send date.
        ///// </summary>
        ///// <value>
        ///// The send date.
        ///// </value>
        //[DataMember]
        //public DateTime SendDate { get; set; }

        /// <summary>
        /// Gets or sets the cibles.
        /// </summary>
        /// <value>
        /// The cibles.
        /// </value>
        [DataMember]
        public List<CibleDto> Cibles { get; set; }

        /// <summary>
        /// Gets or sets the unites.
        /// </summary>
        /// <value>
        /// The unites.
        /// </value>
        [DataMember]
        public List<ParametreDto> Unites { get; set; }

        
        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>
        /// The types.
        /// </value>
        [DataMember]
        public List<ParametreDto> Types { get; set; }

        /// <summary>
        /// Gets or sets the types inventaire.
        /// </summary>
        /// <value>
        /// The types inventaire.
        /// </value>
        [DataMember]
        public List<ParametreDto> TypesInventaire { get; set; }
        [DataMember]
        public bool HasFormules { get; set; }
        [DataMember]
        public List<ParametreDto> CodesApe { get; set; }
        [DataMember]
        public List<ParametreDto> CodesTre { get; set; }
        [DataMember]
        public List<ParametreDto> Territorialites { get; set; }
        [DataMember]
        public List<NomenclatureDto> Nomenclatures1 { get; set; }
        [DataMember]
        public List<NomenclatureDto> Nomenclatures2 { get; set; }
        [DataMember]
        public List<NomenclatureDto> Nomenclatures3 { get; set; }
        [DataMember]
        public List<NomenclatureDto> Nomenclatures4 { get; set; }
        [DataMember]
        public List<NomenclatureDto> Nomenclatures5 { get; set; }
        [DataMember]
        public List<ParametreDto> CodesClasse { get; set; }

        [DataMember]
        public List<ParametreDto> TypesRisque { get; set; }
        [DataMember]
        public List<ParametreDto> TypesMateriel { get; set; }
        [DataMember]
        public List<ParametreDto> NaturesLieux { get; set; }
        [DataMember]
        public bool IsExistLoupe { get; set; } 
        [DataMember]
        public DateTime? DateDebHisto { get; set; }

    }
}
