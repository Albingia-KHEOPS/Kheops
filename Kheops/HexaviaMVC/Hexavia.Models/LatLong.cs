namespace Hexavia.Models
{
    public class LatLong
    {
        public string Type { get; set; }
        public string Branche { get; set; }
        public int NumeroChrono { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Num { get; set; }
        public string Ref { get; set; }
        public string Libelle { get; set; }
        public string FullLibelle { get; set; }
        public int NBContrat { get; set; }
        public string DateSaisie { get; set; }
        public string CourtierGestionnaire { get; set; }
        public string AssureGestionnaire { get; set; }
        public string Smp { get; set; }
        public string[] Types { get; set; }
    }
}
