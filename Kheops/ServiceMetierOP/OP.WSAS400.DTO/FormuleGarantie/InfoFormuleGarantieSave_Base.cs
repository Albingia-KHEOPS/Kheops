using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public abstract class InfoFormuleGarantieSave_Base
    {
        [DataMember]
        public string CodeOffre { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string CodeFormule { get; set; }
        [DataMember]
        public string CodeOption { get; set; }
        [DataMember]
        public string User { get; set; }
        [DataMember]
        public long DateNow { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdLienOpt { get; set; }
    }
}
