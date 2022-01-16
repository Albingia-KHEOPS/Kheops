using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ExcelDto.RS
{
    [KnownType(typeof(RSRecupObjets))]
    [DataContract]
    public class RSRecupObjets : BaseExcelObjets
    {
        [DataMember]
        [Column(Name = "KFAAUTS")]
        public string AutreSupport { get; set; }

        [DataMember]
        [Column(Name = "KFAAUTL")]
        public string AutreSupportLibelle { get; set; }

        [DataMember]
        [Column(Name = "KFALABD")]
        public string LaboDeveloppement { get; set; }

        [DataMember]
        [Column(Name = "KFAPRES")]
        public string ParticipationResultat { get; set; }

        [DataMember]
        [Column(Name = "KFAECRG")]
        public string EcheancierRgltCotis { get; set; }


        #region Cells

        [DataMember]
        public string CellsAutreSupport { get; set; }

        [DataMember]
        public string CellsAutreSupportLibelle { get; set; }

        [DataMember]
        public string CellsLaboDeveloppement { get; set; }

        [DataMember]
        public string CellsParticipationResultat { get; set; }

        [DataMember]
        public string CellsEcheancierRgltCotis { get; set; }


        #endregion
    }
}
