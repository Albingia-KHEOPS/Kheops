using System.Runtime.Serialization;

namespace OP.ClassesMetier
{
    [DataContract]
    public enum EtapeRegularisation
    {
        ChoixMode = 1,
        ChoixPeriodeCourtier,
        _ControlePeriodeCourtier,
        _ControlesSpecifiques,
        _MiseEnHisto,
        ChoixGaranties,
        ChoixRisques,
        RegularisationRisque,
        RegularisationGarantie,
        RegularisationContrat,
        _CalculRegularisation,
        Cotisation
    }
}
