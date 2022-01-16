namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class Periodicite : RefParamValue
    {
        public static readonly Periodicite Regularisation = new Periodicite {
            Code = "R",
            Libelle = "Régularisation seule",
            LibelleLong = "Régularisation seule",
            ParamNum1 = 2
        };
    }
}