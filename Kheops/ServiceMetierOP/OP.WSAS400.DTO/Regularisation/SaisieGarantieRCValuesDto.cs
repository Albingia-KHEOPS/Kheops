using System;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class SaisieGarantieRCValuesDto
    {
        private bool isRegulZero;

        [DataMember]
        public GarantieRCValuesDto Previsionnel { get; set; }

        [DataMember]
        public GarantieRCValuesDto Definitif { get; set; }

        [DataMember]
        public double CotisationForcee { get; set; }

        [DataMember]
        public double TaxesCotisationForcee { get; set; }

        [DataMember]
        public double RegulForcee { get; set; }

        [DataMember]
        public double RegulCalcule { get; set; }

        [DataMember]
        public bool IsRegulZero
        {
            get { return IsZeroLocked ? true : isRegulZero; }
            set { isRegulZero = IsZeroLocked ? true : value; }
        }

        [DataMember]
        public bool IsZeroLocked { get; set; }

        [DataMember]
        public bool IsReadOnly { get; set; }

        [DataMember]
        public int MotifInferieur { get; set; }

        [DataMember]
        public double Attentat { get; set; }

        public double ComputeRegulAmount()
        {
            if (IsRegulZero)
            {
                return 0D;
            }
            else if (RegulForcee != 0D)
            {
                return RegulForcee;
            }

            if (Definitif.BasicValues.Assiette == 0 && Definitif.BasicValues.Unite.Code != "D")
                return 0D;
            if (Definitif.BasicValues.Unite.Code == "D" && Definitif.BasicValues.TauxMontant == 0)
                return 0D;

            double regul = Definitif.CalculAssiette - (CotisationForcee != 0D ? CotisationForcee : Definitif.CotisationEmise);
            if (Definitif.Coefficient != 0D)
            {
                regul *= (Definitif.Coefficient / 100);
            }

            return regul;
        }
    }
}
