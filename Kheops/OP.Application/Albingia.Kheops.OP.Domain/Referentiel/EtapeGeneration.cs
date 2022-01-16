using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public enum EtapeGeneration
    {
        [BusinessCode("AVNRM")]  AVNRM, 
        [BusinessCode("FIN")  ]  FIN, 
        [BusinessCode("GEN")  ]  GEN, 
        [BusinessCode("REGUL")]  REGUL, 
        [BusinessCode("ATTES")]  ATTES, 
        [BusinessCode("GARAN")]  GARAN, 
        [BusinessCode("RISQUE")] RISQUE 
    }
}