using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Concerts;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Offres.Risque.Inventaire
{
    [DataContract]
    public class InventaireDto : _Inventaire_Base
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        [Column(Name = "CODE")]
        public Int64 Code { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>
        [DataMember]
        [Column(Name = "DESCRIPTIF")]
        public string Descriptif { get; set; }

        [DataMember]
        [Column(Name = "TYPE")]
        public Int64 InventaireType { get; set; }

        [DataMember]
        [Column(Name = "REPORTVAL")]
        public string ReportVal { get; set; }
        [DataMember]
        [Column(Name = "VALEUR")]
        public double Valeur { get; set; }
        [DataMember]
        [Column(Name = "VALTYPE")]
        public string TypeLst { get; set; }
        [DataMember]
        [Column(Name = "VALUNIT")]
        public string UniteLst { get; set; }
        [DataMember]
        [Column(Name = "VALTAXE")]
        public string TaxeLst { get; set; }
        [Column(Name="BRANCHE")]
        public string CodeBranche { get; set; }
        [Column(Name="CIBLE")]
        public string CodeCible { get; set; }
        [DataMember]
        [Column(Name = "CODERSQ")]
        public string CodeRisque { get; set; }
        [DataMember]
        [Column(Name = "NUMAVN")]
        public string NumAvenantPage { get; set; }
        [DataMember]
        public ParametreDto Type { get; set; }

        [DataMember]
        public String NumDescription { get; set; }

        [DataMember]
        public List<InventaireGridRowDto> InventaireInfos { get; set; }

        [DataMember]
        public List<ParametreDto> NaturesLieu { get; set; }

        [DataMember]
        public List<ParametreDto> ListePays { get; set; }

        [DataMember]
        public List<ParametreDto> CodesMateriel { get; set; }
        [DataMember]
        public List<ParametreDto> Unites { get; set; }
        [DataMember]
        public List<ParametreDto> Types { get; set; }
        [DataMember]
        public List<ParametreDto> CodesExtension { get; set; }
        [DataMember]
        public String PerimetreApplication { get; set; }
        [DataMember]
        public List<ParametreDto> CodesQualite { get; set; }
        [DataMember]
        public List<ParametreDto> CodesRenonce { get; set; }
        [DataMember]
        public List<ParametreDto> CodesRsqLoc { get; set; }
    }
}
