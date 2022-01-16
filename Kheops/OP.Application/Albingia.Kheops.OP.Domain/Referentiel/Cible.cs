namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class Cible
    {
        public static readonly Cible MultirisqueImmeublePro = new Cible { Code = "MRIMC" };
        public static readonly Cible MultirisqueImmeuble = new Cible { Code = "MRIMM" };

        ///<summary> Id </summary>
        public long ID { get; set; }
        ///<summary> Code </summary>
        public string Code { get; set; }
        ///<summary> Description </summary>
        public string Description { get; set; }

    }
}