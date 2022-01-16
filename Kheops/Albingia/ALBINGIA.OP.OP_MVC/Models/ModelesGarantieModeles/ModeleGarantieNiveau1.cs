using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles
{
    [Serializable]
    public class ModeleGarantieNiveau1 : IModeleGarantieNiveau
    {
        public int Code { get; set; }
        public string Action { get; set; }
        public string FCTCodeGarantie { get; set; }
        public string CodeGarantie { get; set; }
        public string Description { get; set; }
        public string Caractere { get; set; }
        public string LibCaractere
        {
            get
            {
                switch (Caractere)
                {
                    case "O":
                        return "Obligatoire";
                    case "F":
                        return "Facultative";
                    case "P":
                        return "Proposée";
                    case "S":
                        return "Suggérée";
                    default:
                        return string.Empty;
                }
            }
        }
        public string Nature { get; set; }
        public string NatureParam { get; set; }
        public int CodeParent { get; set; }
        public int CodeNiv1 { get; set; }
        public List<ModeleGarantieNiveau2> Modeles { get; set; }
        public string InvenPossible { get; set; }
        public int CodeInven { get; set; }
        public string TypeInven { get; set; }


        public bool isChecked
        {
            get
            {
                //if ((((Caractere == "O" || Caractere == "P") && (Nature != "E" || NatureParam != "E")) || (Caractere == "B" && Nature == "C")) && NatureParam != "E")
                //    return true;

                if (((Caractere == "O" && (Nature != "E" || NatureParam != "E")) || (Caractere == "B" && Nature == "C")) && NatureParam != "E")
                    return true;
                if (Caractere == "P" && ((Nature != "E" && NatureParam != "" && NatureParam != "E") || (Nature == "E" && NatureParam != "E")))
                    return true;

                if (Caractere == "F" && (Nature == "C" || Nature == "A") && !string.IsNullOrEmpty(NatureParam) && NatureParam != "E")
                    return true;
                if (Caractere == "F" && Nature == "E" && (NatureParam == "A" || NatureParam == "C"))
                    return true;
                return false;
            }
        }
        public List<ModeleRisque> Risques { get; set; }

        public bool AppliqueA { get; set; }
        public bool isExcluding
        {
            get
            {
                if (AppliqueA && Caractere != "B" && Nature != "O")
                    return true;
                else
                    return false;
            }
        }

        public string GuidGarantie { get; set; }

        public bool MAJ { get; set; }

        public string FlagModif { get; set; }
        public int Niveau { get; set; }

        public string ParamNatMod { get; set; }
        public string ParamNatModVal { get; set; }

        public string GarantieIncompatible { get; set; }
        public string GarantieAssociee { get; set; }
        public Int64 GarantieAlternative { get; set; }

        public string FlagPortee { get; set; }

        public bool CreateInAvt { get; set; }
        public bool UpdateInAvt { get; set; }
        public bool ModeAvt { get; set; }
        public bool IsReadOnly { get; set; }
        public DateTime? DateEffetAvtModifLocale { get; set; }
        public bool IsGarantieSortie { get; set; }

        public string AlimAssiette { get; set; }
        public List<IModeleGarantieNiveau> Garanties
        {
            get { return Modeles.Cast<IModeleGarantieNiveau>().ToList(); }
        }

        public ModeleGarantieNiveau1()
        {
            this.Code = 0;
            this.CodeGarantie = string.Empty;
            this.Description = string.Empty;
            this.Caractere = string.Empty;
            this.Nature = string.Empty;
            this.CodeParent = 0;
            this.CodeNiv1 = 0;
            this.Modeles = new List<ModeleGarantieNiveau2>();

            this.AppliqueA = false;
            this.GuidGarantie = string.Empty;
        }
    }
}