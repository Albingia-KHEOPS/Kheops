using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Common;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Ecran.DetailsRisque
{
    [DataContract]
    public class DetailsRisqueGetResultDto //: _DetailsRisque_Base, IResult
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
        /// Gets or sets the code next objet.
        /// </summary>
        /// <value>
        /// The code next objet.
        /// </value>
        [DataMember]
        public int CodeNextObjet { get; set; }

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

        /// <summary>
        /// Gets or sets the regimes taxe.
        /// </summary>
        /// <value>
        /// The regimes taxe.
        /// </value>
        [DataMember]
        public List<ParametreDto> RegimesTaxe { get; set; }
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

        [DataMember]
        [Column(Name = "EFFETAVNYEAR")]
        public Int32 EffetAvnAnnee { get; set; }
        [DataMember]
        [Column(Name = "EFFETAVNMONTH")]
        public Int32 EffetAvnMois { get; set; }
        [DataMember]
        [Column(Name = "EFFETAVNDAY")]
        public Int32 EffetAvnJour { get; set; }
        [DataMember]
        public DateTime DateEffetAvn { get; set; }

        [DataMember]
        [Column(Name = "DATMODYEAR")]
        public Int32 DateModifAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATMODMONTH")]
        public Int32 DateModifMois { get; set; }
        [DataMember]
        [Column(Name = "DATMODDAY")]
        public Int32 DateModifJour { get; set; }
        [DataMember]
        public DateTime? DateModifRsqAvn { get; set; }


        [DataMember]
        public int EchAnnee { get; set; }
        [DataMember]
        public int EchMois { get; set; }
        [DataMember]
        public int EchJour { get; set; }

        [DataMember]
        public InfosBaseDto Offre { get; set; }
        //[DataMember]
        //public ContratDto Contrat { get; set; }
    }
}
