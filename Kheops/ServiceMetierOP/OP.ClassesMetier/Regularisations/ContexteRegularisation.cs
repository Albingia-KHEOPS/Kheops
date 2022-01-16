using System.Runtime.Serialization;

namespace OP.ClassesMetier
{
    [DataContract]
    public class ContexteRegularisation
    {
        [DataMember]
        public ModeRegularisation Mode { get; set; }

        [DataMember]
        public PorteeRegularisation Portee { get; set; }

        [DataMember]
        public EtapeRegularisation Etape { get; set; }

        [IgnoreDataMember]
        public bool EstMultiRisques
        {
            get { return NbRisques > 1; }
        }

        [DataMember]
        public bool EstMultiGaranties { get; set; }

        [DataMember]
        public bool RisquesHomogenes { get; set; }

        [DataMember]
        public int NbRisques { get; set; }
    }
}
