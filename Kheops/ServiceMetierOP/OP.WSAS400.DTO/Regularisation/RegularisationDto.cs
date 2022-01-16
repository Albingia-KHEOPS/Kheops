using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Offres.Parametres;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegularisationDto
    {
        [DataMember]
        public List<AvenantAlerteDto> Alertes { get; set; }
        [DataMember]
        public List<ParametreDto> TypesContrat { get; set; }
        [DataMember]
        public List<LigneRegularisationDto> Regularisations { get; set; }

        public static RegularisationMode GetMode(string str)
        {
            RegularisationMode rgmode = 0;
            if (!string.IsNullOrWhiteSpace(str) && !Enum.TryParse(str, true, out rgmode))
            {
                if (str.ToUpper() == RegularisationMode.Standard.AsCode())
                {
                    rgmode = RegularisationMode.Standard;
                }
                else if (str.ToUpper() == RegularisationMode.Coassurance.AsCode())
                {
                    rgmode = RegularisationMode.Coassurance;
                }
            }

            return rgmode;
        }
    }
}
