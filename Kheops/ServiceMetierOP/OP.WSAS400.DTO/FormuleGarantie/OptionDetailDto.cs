using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    public class OptionDetailDto
    {
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }
        [Column(Name = "TYPE")]
        public String Type { get; set; }
        [Column(Name = "CODEOFFRE")]
        public String CodeOffre { get; set; }
        [Column(Name = "VERSION")]
        public Int32 Version { get; set; }
        [Column(Name = "CODEFORMULE")]
        public Int32 CodeFormule { get; set; }
        [Column(Name = "CODEOPTION")]
        public Int32 CodeOption { get; set; }
        [Column(Name = "GUIDOPTION")]
        public Int64 GuidOption { get; set; }
        [Column(Name = "TYPEENREGISTREMENT")]
        public String TypeEnregistrement { get; set; }
        [Column(Name = "GUIDVOLET")]
        public Int64 GuidVolet { get; set; }
        [Column(Name = "GUIDBLOC")]
        public Int64 GuidBloc { get; set; }
        [Column(Name = "GUIDPARAM")]
        public Int64 GuidParam { get; set; }
        [Column(Name = "MODELE")]
        public String Modele { get; set; }
        [Column(Name = "GUIDMODELE")]
        public Int64 GuidModele { get; set; }
        [Column(Name = "CREATEUSER")]
        public String CreateUser { get; set; }
        [Column(Name = "CREATEDATE")]
        public Int64 CreateDate { get; set; }
        [Column(Name = "UPDATEUSER")]
        public String UpdateUser { get; set; }
        [Column(Name = "UPDATEDATE")]
        public Int64 UpdateDate { get; set; }
        [Column(Name = "FLAG")]
        public Int32 Flag { get; set; }
    }
}
