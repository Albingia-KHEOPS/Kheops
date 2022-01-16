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
    public class AvenantModificationDto : IAvenantDto
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
        [Column(Name = "DATEEFFET")]
        public Int64 DateEffet { get; set; }
        [Column(Name = "HEUREEFFET")]
        public int HeureEffet { get; set; }
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

        [DataMember]
        public string ErrorAvt { get; set; }
        [DataMember]
        public DateTime? DateEffetAvt { get; set; }
        [DataMember]
        public TimeSpan? HeureEffetAvt { get; set; }
        [DataMember]
        public List<ParametreDto> Motifs { get; set; }
        [DataMember]
        public bool IsModifHorsAvenant { get; set; }

        [DataMember]
        public DateTime? ProchaineEchHisto { get; set; }

        string IAvenantDto.Type => TypeAvt;

        long IAvenantDto.NumInterne => NumInterneAvt;

        string IAvenantDto.Description => DescriptionAvt;

        string IAvenantDto.Observations => ObservationsAvt;

        long IAvenantDto.Numero => NumAvt;

        DateTime? IAvenantDto.Date => DateEffetAvt.HasValue ? DateEffetAvt :
            DateEffet > 0 ? AlbConvert.ConvertIntToDate((int)DateEffet).Value.Add(AlbConvert.ConvertIntToTime(HeureEffet).Value) :
            default(DateTime?);

        string IAvenantDto.Motif => MotifAvt;
    }
}
