namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class RegimeTaxe : RefValue
    {
        public const string Monaco = "M";
        public const string MonacoProfessionLiberaleHabitation = "N";
        public static readonly string[] ListeNonCATNAT = new[] { Monaco, MonacoProfessionLiberaleHabitation, "G", "D", "Y" };
    }
}