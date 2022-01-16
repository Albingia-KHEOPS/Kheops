using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class Option
    {
        public string GuidId { get; set; }
        public string Type { get; set; }
        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }
        public string GuidOption { get; set; }
        public string TypeEnregistrement { get; set; }
        public string GuidVolet { get; set; }
        public string GuidBloc { get; set; }
        public string GuidParamBloc { get; set; }
        public string Modele { get; set; }
        public string GuidModele { get; set; }
        public string User { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool isChecked { get; set; }
    }
}
