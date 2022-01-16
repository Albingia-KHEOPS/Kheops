using System.Runtime.Serialization;

namespace ALBINGIA.OP
{
    [DataContract]
    public class InfosPeriodeRegularisation
    {
        //[DataMember(Name = "type")]
        public string Type { get; set; }

        //[DataMember(Name = "codeOffre")]
        public string CodeOffre { get; set; }

        //[DataMember(Name = "version")]
        public string Version { get; set; }

        //[DataMember(Name = "typeContrat")]
        public string TypeContrat { get; set; }

        //[DataMember(Name = "debutPeriode")]
        public string DebutPeriode { get; set; }

        //[DataMember(Name = "finPeriode")]
        public string FinPeriode { get; set; }

        //[DataMember(Name = "regimeTaxe")]
        public string RegimeTaxe { get; set; }

        public string DisplayTypeContrat { get; set; }
    }
}