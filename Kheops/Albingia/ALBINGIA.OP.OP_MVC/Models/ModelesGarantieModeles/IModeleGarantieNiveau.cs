using System;
using System.Collections.Generic;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles
{
    public interface IModeleGarantieNiveau
    {
        bool AppliqueA { get; set; }
        string Caractere { get; set; }
        int Code { get; set; }
        string CodeGarantie { get; set; }
        int CodeInven { get; set; }
        int CodeNiv1 { get; set; }
        int CodeParent { get; set; }
        bool CreateInAvt { get; set; }
        DateTime? DateEffetAvtModifLocale { get; set; }
        string Description { get; set; }
        string FCTCodeGarantie { get; set; }
        string FlagModif { get; set; }
        long GarantieAlternative { get; set; }
        string GarantieAssociee { get; set; }
        string GarantieIncompatible { get; set; }
        string GuidGarantie { get; set; }
        string InvenPossible { get; set; }
        bool isChecked { get; }
        bool isExcluding { get; }
        bool IsGarantieSortie { get; set; }
        bool IsReadOnly { get; set; }
        string LibCaractere { get; }
        bool MAJ { get; set; }
        bool ModeAvt { get; set; }
        List<IModeleGarantieNiveau> Garanties { get; }
        string Nature { get; set; }
        string NatureParam { get; set; }
        int Niveau { get; set; }
        string ParamNatMod { get; set; }
        string ParamNatModVal { get; set; }
        List<ModeleRisque> Risques { get; set; }
        string TypeInven { get; set; }
        bool UpdateInAvt { get; set; }
    }
}