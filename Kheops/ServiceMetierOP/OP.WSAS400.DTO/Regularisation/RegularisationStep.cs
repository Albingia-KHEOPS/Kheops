using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public enum RegularisationStep
    {
        [EnumMember]
        Invalid = 0,

        [EnumMember]
        ChoixMode = 1,

        [EnumMember]
        ChoixPeriodeCourtier,

        [EnumMember]
        _ControlePeriodeCourtier,

        [EnumMember]
        _ControlesSpecifiques,

        [EnumMember]
        _MiseEnHisto,

        [EnumMember]
        ChoixGaranties,

        [EnumMember]
        ChoixPeriodesGarantie,

        [EnumMember]
        ChoixRisques,

        [EnumMember]
        Regularisation,

        [EnumMember]
        RegularisationTR,

        [EnumMember]
        _CalculRegularisation,

        [EnumMember]
        Cotisations,

        [EnumMember]
        ControleFin, 

        [EnumMember]
        Contrat
    }
}
