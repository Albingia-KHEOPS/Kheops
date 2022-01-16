using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.FormuleGarantie.GarantieModele
{
    public class KpoptapDto
    {
        public long KDDID { get; set; }
        public string KDDTYP { get; set; }
        public string KDDIPB { get; set; }
        public int KDDALX { get; set; }
        public int KDDFOR { get; set; }
        public int KDDOPT { get; set; }
        public long KDDKDBID { get; set; }
        public string KDDPERI { get; set; }
        public int KDDRSQ { get; set; }
        public int KDDOBJ { get; set; }
        public long KDDINVEN { get; set; }
        public int KDDINVEP { get; set; }
        public string KDDCRU { get; set; }
        public long KDDCRD { get; set; }
        public string KDDMAJU { get; set; }
        public long KDDMAJD { get; set; }
    }
}
