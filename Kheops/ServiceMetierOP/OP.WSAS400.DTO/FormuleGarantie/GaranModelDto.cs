using OP.WSAS400.DTO.FormuleGarantie.GarantieModele;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class GaranModelDto
    {
        [Column(Name = "GUIDV")]
        public Int64 Guidv { get; set; }
        [Column(Name = "CODEVOLET")]
        public string CodeVolet { get; set; }
        [Column(Name = "DESCRVOLET")]
        public string DescrVolet { get; set; }
        [Column(Name = "CARACVOLET")]
        public string CaracVolet { get; set; }
        [Column(Name="VOLETORDRE")]
        public Single VoletOrdre { get; set; }
        [Column(Name = "GUIDVOLET")]
        public Int64 GuidVolet { get; set; }
        [Column(Name = "VOLETCOLLAPSE")]
        public string VoletCollapse { get; set; }
        [Column(Name = "CHECKED")]
        public string Checked { get; set; }
        [Column(Name = "GUIDB")]
        public Int64 Guidb { get; set; }
        [Column(Name = "CODEBLOC")]
        public string CodeBloc { get; set; }
        [Column(Name = "DESCRBLOC")]
        public string DescrBloc { get; set; }
        [Column(Name = "CARACBLOC")]
        public string CaracBloc { get; set; }
        [Column(Name = "BLOCORDRE")]
        public Single BlocOrdre { get; set; }
        [Column(Name = "GUIDBLOC")]
        public Int64 GuidBloc { get; set; }
        [Column(Name = "FORMGEN")]
        public string FormGen { get; set; }
        [Column(Name = "LIBMODELE")]
        public string LibModele { get; set; }
        [Column(Name = "GUIDM")]
        public Int32 GuidM { get; set; }
        [Column(Name = "SEM")]
        public long Sem { get; set; }
        [Column(Name = "SEQ")]
        public long Seq { get; set; }

        #region Niv1
        [Column(Name = "Niv1")]
        GaranNivModel Niv1 { get; set; }
        #endregion
        #region Niv2
        [Column(Name = "Niv2")]
        GaranNivModel Niv2 { get; set; }
        #endregion
        #region Niv3
        [Column(Name = "Niv3")]
        GaranNivModel Niv3 { get; set; }
        #endregion
        #region Niv4
        [Column(Name = "Niv4")]
        GaranNivModel Niv4 { get; set; }
        #endregion
    }
}
