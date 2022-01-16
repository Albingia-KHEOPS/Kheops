using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Avenant
{
    public class AvenantRegularisationDto : IAvenantDto
    {
        public string ErrorAvt { get; set; }

        [Column(Name = "TYPE_AVT")]
        public string TypeAvt { get; set; }
        [Column(Name = "NUM_INTERNE_AVT")]
        public Int64 NumInterneAvt { get; set; }
        [Column(Name = "NUM_AVT")]
        public Int64 NumAvt { get; set; }
        [Column(Name = "DESCRIPTION")]
        public string DescriptionAvt { get; set; }
        [Column(Name = "OBS")]
        public string ObservationsAvt { get; set; }
        [Column(Name="MOTIF")]
        public string MotifAvt { get; set; }
        public bool IsModifHorsAvenant => false;

        public bool HasHisto { get; set; }

        public DateTime? DateFin { get; set; }

        string IAvenantDto.Type => TypeAvt;

        long IAvenantDto.NumInterne => NumInterneAvt;

        string IAvenantDto.Description => DescriptionAvt;

        string IAvenantDto.Observations => ObservationsAvt;

        long IAvenantDto.Numero => NumAvt;

        DateTime? IAvenantDto.Date => DateFin;

        string IAvenantDto.Motif => MotifAvt;
    }
}
