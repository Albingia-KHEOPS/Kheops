using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Clausier
{
    [DataContract]
    public class ParametreGenClauseDto
    {
        [DataMember]
        public string ActeGestion { get; set; }
        [DataMember]
        public string Letape { get; set; }
        [DataMember]
        public string LeContexte { get; set; }
        [DataMember]
        public int NuRisque { get; set; }
        [DataMember]
        public int NuObjet { get; set; }
        [DataMember]
        public int NuFormule { get; set; }
        [DataMember]
        public int NuOption { get; set; }
        [DataMember]
        public long IdAttesKpAtt { get; set; }

        public ParametreGenClauseDto()
        {
            NuRisque = 0;
            NuObjet = 0;
            NuFormule = 0;
            NuOption = 0;

        }
    }
}
