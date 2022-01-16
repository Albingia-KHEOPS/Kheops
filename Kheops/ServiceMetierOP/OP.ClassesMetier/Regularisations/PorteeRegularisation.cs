using System.Runtime.Serialization;

namespace OP.ClassesMetier
{
    [DataContract]
    public enum PorteeRegularisation
    {
        Contrat = 0,
        Risque,
        Garantie
    }
}
