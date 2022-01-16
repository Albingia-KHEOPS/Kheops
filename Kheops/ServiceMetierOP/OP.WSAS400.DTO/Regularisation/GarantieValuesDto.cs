using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class GarantieValuesDto
    {
        [DataMember]
        public double Assiette { get; set; }

        [DataMember]
        public double TauxMontant { get; set; }

        [DataMember]
        public UniteTauxMontantDto Unite { get; set; }

        [DataMember]
        public CodeTaxesDto CodeTaxes { get; set; }

        [DataMember]
        public bool IsAuto { get; set; }

        public bool IsValid
        {
            get
            {
                return (TauxMontant != default(double)
                    && Unite != null
                    && !Unite.Code.IsEmptyOrNull()
                    && CodeTaxes != null
                    && !CodeTaxes.Code.IsEmptyOrNull())
                || (TauxMontant == default(double)
                    && Unite != null && Unite.Code.IsEmptyOrNull());
            }
        }

        public bool IsValidCompute
        {
            get
            {
                return TauxMontant != default(double)
                    && Unite != null
                    && !Unite.Code.IsEmptyOrNull()
                    && CodeTaxes != null
                    && !CodeTaxes.Code.IsEmptyOrNull();
            }
        }
    }
}
