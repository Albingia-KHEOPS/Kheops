using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegularisationInfoDto
    {
        [DataMember]
        public long LotId { get; set; }

        [DataMember]
        [Column(Name="EXERCICE")]
        public Int32 Exercice { get; set; }
        [DataMember]
        [Column(Name="DATEDEB")]
        public Int32? PeriodeDebInt { get; set; }
        [DataMember]
        [Column(Name = "DATEFIN")]
        public Int32? PeriodeFinInt { get; set; }

        public DateTime? Debut => PeriodeDebInt.GetValueOrDefault() == default || PeriodeFinInt == 10101 ?
            default(DateTime?) :
            DateTime.ParseExact(PeriodeDebInt?.ToString(), "yyyyMMdd", AlbConvert.AppCulture);

        public DateTime? Fin => PeriodeFinInt.GetValueOrDefault() == default || PeriodeFinInt == 10101 ?
            default(DateTime?) :
            DateTime.ParseExact(PeriodeFinInt?.ToString(), "yyyyMMdd", AlbConvert.AppCulture);

        [DataMember]
        public List<AvenantAlerteDto> Alertes { get; set; }

        [DataMember]
        [Column(Name = "NUMINTERNEAVT")]
        public Int64 NumInterneAvt { get; set; }
        [DataMember]
        [Column(Name = "NUMAVT")]
        public Int64 NumAvt { get; set; }
        [DataMember]
        [Column(Name = "MODEAVT")]
        public string ModeAvt { get; set; }
        [DataMember]
        [Column(Name = "TYPEAVT")]
        public string TypeAvt { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEAVT")]
        public string LibelleAvt { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTIONAVT")]
        public string DescriptionAvt { get; set; }

        [DataMember]
        [Column(Name="CODECOURTIER")]
        public Int32 CodeCourtier { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIER")]
        public string NomCourtier { get; set; }
        [DataMember]
        [Column(Name="TAUXCOMHCN")]
        public double TauxHCATNAT { get; set; }
        [DataMember]
        [Column(Name="TAUXCOMCN")]
        public double TauxCATNAT { get; set; }
        [DataMember]
        [Column(Name="CODEENC")]
        public string CodeQuittancement { get; set; }
        [DataMember]
        [Column(Name = "LIBENC")]
        public string LibQuittancement { get; set; }
        [DataMember]
        public bool MultiCourtier { get; set; }

        [DataMember]
        [Column(Name="CODECOURTIERCOM")]
        public Int32 CodeCourtierCom { get; set; }
        [DataMember]
        [Column(Name="NOMCOURTIERCOM")]
        public string NomCourtierCom { get; set; }
        [DataMember]
        [Column(Name = "MOTIFAVT")]
        public string MotifAvt { get; set; }

        [DataMember]
        public List<ParametreDto> Courtiers { get; set; }
        [DataMember]
        public List<ParametreDto> Quittancements { get; set; }
        [DataMember]
        public List<ParametreDto> Motifs { get; set; }
        [DataMember]
        public string RetourPGM { get; set; }
        [DataMember]
        public bool IsHisto { get; set; }

        /// <summary>
        /// Defines whether SELW exists
        /// </summary>
        [DataMember]
        public bool HasSelections { get; set; }
    }
}
