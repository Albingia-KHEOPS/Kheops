using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Avenant
{
    [DataContract]
    public class AvenantResiliationDto : IAvenantDto
    {
        [DataMember]
        [Column(Name = "TYPEAVT")]
        public string TypeAvt { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEAVT")]
        public string LibelleAvt { get; set; }
        [DataMember]
        [Column(Name = "NUMINTERNEAVT")]
        public Int64 NumInterneAvt { get; set; }
        [Column(Name = "DATEFINGARANTIE")]
        public Int64 DateFin { get; set; }
        [Column(Name = "HEUREFINGARANTIE")]
        public int HeureFin { get; set; }
        [DataMember]
        [Column(Name = "NUMAVT")]
        public Int64 NumAvt { get; set; }
        [DataMember]
        [Column(Name = "MOTIFAVT")]
        public string MotifAvt { get; set; }
        [DataMember]
        [Column(Name = "LIBMOTIFAVT")]
        public string LibMotifAvt { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTIONAVT")]
        public string DescriptionAvt { get; set; }
        [DataMember]
        [Column(Name = "OBSERVATIONSAVT")]
        public string ObservationsAvt { get; set; }
        [Column(Name = "AVTECHEANCE")]
        public string AvtEcheance { get; set; }

        [DataMember]
        public string ErrorAvt { get; set; }
        [DataMember]
        public DateTime? DateFinGarantie { get; set; }
        [DataMember]
        public TimeSpan? HeureFinGarantie { get; set; }
        [DataMember]
        public bool AvenantEch { get; set; }

        [DataMember]
        public List<ParametreDto> Motifs { get; set; }

        [DataMember]
        public List<ParametreDto> DatesFin { get; set; }
        [DataMember]
        public bool AvenantEchPossible { get; set; }

        [DataMember]
        public DateTime? FinGarantieHisto { get; set; }

        [DataMember]
        public DateTime? ProchaineEchHisto { get; set; }
        [DataMember]
        public bool IsModifHorsAvenant { get; set; }

        string IAvenantDto.Type => TypeAvt;

        long IAvenantDto.NumInterne => NumInterneAvt;

        string IAvenantDto.Description => DescriptionAvt;

        string IAvenantDto.Observations => ObservationsAvt;

        long IAvenantDto.Numero => NumAvt;

        DateTime? IAvenantDto.Date => DateFinGarantie.HasValue ? DateFinGarantie :
            DateFin > 0 ? AlbConvert.ConvertIntToDate((int)DateFin).Value.Add(AlbConvert.ConvertIntToTime(HeureFin).Value) :
            default(DateTime?);

        string IAvenantDto.Motif => MotifAvt;
    }
}
