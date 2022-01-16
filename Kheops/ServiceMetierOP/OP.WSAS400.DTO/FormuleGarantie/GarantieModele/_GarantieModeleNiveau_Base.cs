using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.GarantieModele
{
    [DataContract]
    public abstract class _GarantieModeleNiveau_Base : _DTO_Base
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string CodeGarantie { get; set; }
        [DataMember]
        public string FCTCodeGarantie { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Caractere { get; set; }
        [DataMember]
        public string Nature { get; set; }
        [DataMember]
        public string NatureParam { get; set; }
        [DataMember]
        public int CodeParent { get; set; }
        [DataMember]
        public int CodeNiv1 { get; set; }

        [DataMember]
        public bool AppliqueA { get; set; }
        [DataMember]
        public string GuidGarantie { get; set; }
        [DataMember]
        public List<RisqueDto> Risques { get; set; }
        [DataMember]
        public bool MAJ { get; set; }
        [DataMember]
        public string FlagModif { get; set; }
        [DataMember]
        public int Niveau { get; set; }
        [DataMember]
        public string ParamNatMod { get; set; }
        [DataMember]
        public string ParamNatModVal { get; set; }

        [DataMember]
        public string GarantieIncompatible { get; set; }
        [DataMember]
        public string GarantieAssociee { get; set; }
        [DataMember]
        public Int64 GarantieAlternative { get; set; }
        [DataMember]
        public string InvenPossible { get; set; }
        [DataMember]
        public Int64 CodeInven { get; set; }
        [DataMember]
        public string TypeInven { get; set; }

        [DataMember]
        public bool CreateInAvt { get; set; }
        [DataMember]
        public bool UpdateInAvt { get; set; }
        [DataMember]
        public bool ModeAvt { get; set; }
        [DataMember]
        public bool IsReadOnly { get; set; }

        [DataMember]
        public DateTime? DateEffetAvtModifLocale { get; set; }
        [DataMember]
        public bool IsGarantieSortie { get; set; }

    }
}
