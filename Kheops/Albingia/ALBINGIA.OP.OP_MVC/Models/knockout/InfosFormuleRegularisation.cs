using System.Runtime.Serialization;

namespace ALBINGIA.OP
{
    [DataContract]
    public class InfosFormuleRegularisation
    {
        public string Title { get; set; }

        public string TauxAppel { get; set; }

       public int NbAnnees { get; set; }

       public string Ristourne { get; set; }

        public string RistourneAnticipe { get; set; }

        public string PrcSeuilSP { get; set; }

        public string PrcCotisationsRetenues { get; set; }

        public int InitialTauxAppelRetenu { get; set; }

        public decimal InitialCotisationPeriode { get; set; }

        public double TauxMaxi { get; set; }
    }
}