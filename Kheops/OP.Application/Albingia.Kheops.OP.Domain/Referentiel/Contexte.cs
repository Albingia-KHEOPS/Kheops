namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class Contexte : RefParamValue
    {
        public string TypoDoc => this.ParamText1;
        public short NumOrdre => (short)this.ParamNum1;
    }
}