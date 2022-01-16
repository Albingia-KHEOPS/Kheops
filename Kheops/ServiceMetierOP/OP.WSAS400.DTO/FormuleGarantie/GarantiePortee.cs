namespace OP.WSAS400.DTO.FormuleGarantie
{
    public class GarantiePortee
    {
        public long IdGarantie { get; set; }
        public int CodeRisque { get; set; }
        public int CodeObjet { get; set; }
        public string LibGar { get; set; }

        public long DateDebut { get; set; }
        public long DateFin { get; set; }
        public bool isPachev { get; set; }
        public int AvenantCreation { get; set; }
        public long DateDebutAvenant { get; set; }
    }
}
