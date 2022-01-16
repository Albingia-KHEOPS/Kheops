using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Avenant
{
    [DataContract]
    public class AvenantRemiseEnVigueurDto : IAvenantDto
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
        [DataMember]
        public bool IsModifHorsAvenant { get; set; }

        [DataMember]
        [Column(Name = "DESCRIPTIONAVT")]
        public string DescriptionAvt { get; set; }
        [DataMember]
        [Column(Name = "OBSERVATIONSAVT")]
        public string ObservationsAvt { get; set; }
        [DataMember]
        [Column(Name = "NUMAVT")]
        public Int64 NumAvt { get; set; }

        [DataMember]
        public DateTime? ProchaineEchHisto { get; set; }

        [DataMember]
        public string ErrorAvt { get; set; }
       
        
        [DataMember]
        public DateTime? DateRemiseVig { get; set; }

        [DataMember]
        public TimeSpan? HeureRemiseVig { get; set; }

        [DataMember]
        [Column(Name = "DATEEFFET")]
        public Int64 DateRemiseVigInt { get; set; }

        [DataMember]
        [Column(Name = "HEUREEFFET")]
        public int HeureRemiseVigInt { get; set; }

        [DataMember]
        public DateTime? DateResil { get; set; }

        [DataMember]
        public TimeSpan? HeureResil { get; set; }

        [Column(Name = "DATERESIL")]
        [DataMember]
        public Int64? DateResiltInt { get; set; }

        [DataMember]
        [Column(Name = "HEURERESIL")]
        public Int16? HeureResilInt { get; set; }

        [Column(Name ="TYPEGESTION")]
        [DataMember]
        public string TypeGestion { get; set; }

        [DataMember]
        public Int32 PrimeReglee { get; set; }

        [DataMember]
        [Column(Name = "DATERGLT")]
        public Int64 DateRglt { get; set; }

        [DataMember]
        public DateTime? PrimeReglementDate { get; set; }

        [DataMember]
        [Column(Name = "DATEDEBSUSP")]
        public Int64 DateDSusp { get; set; }

        [DataMember]
        public DateTime? DateSuspDeb { get; set; }

        [DataMember]
        [Column(Name = "HEUREDEBSUSP")]
        public Int16 HeureDSusp { get; set; }

        [DataMember]
        public TimeSpan? HeureSuspDeb { get; set; }

        [DataMember]
        [Column(Name = "DATEFINSUSP")]
        public Int64 DateFSusp { get; set; }

        [DataMember]
        public DateTime? DateSuspFin { get; set; }

        [DataMember]
        [Column(Name = "HEUREFINSUSP")]
        public Int16 HeureFSusp { get; set; }

        [DataMember]
        public TimeSpan? HeureSuspFin { get; set; }

        [DataMember]
        public DateTime? DateDebNonSinistre { get; set; }
        [DataMember]
        public DateTime? DateFinNonSinistre { get; set; }
        [DataMember]
        public TimeSpan? HeureDebNonSinistre { get; set; }
        [DataMember]
        public TimeSpan? HeureFinNonSinistre { get; set; }

        [Column(Name ="DATE_FIN_EFFET")]
        public Int64 DFinEffet { get; set; }
        [DataMember]
        public DateTime? DateFinEffet { get; set; }
        [Column(Name = "HEURE_FIN_EFFET")]
        public Int16 HFinEffet { get; set; }
        [DataMember]
        public TimeSpan? HeureFinEffet { get; set; }

        string IAvenantDto.Type => TypeAvt;

        long IAvenantDto.NumInterne => NumInterneAvt;

        string IAvenantDto.Description => DescriptionAvt;

        string IAvenantDto.Observations => ObservationsAvt;

        long IAvenantDto.Numero => NumAvt;

        DateTime? IAvenantDto.Date => DateRemiseVig.HasValue ? DateRemiseVig.Value.Add(HeureRemiseVig.Value) : default(DateTime?);

        string IAvenantDto.Motif => string.Empty;
    }
}
