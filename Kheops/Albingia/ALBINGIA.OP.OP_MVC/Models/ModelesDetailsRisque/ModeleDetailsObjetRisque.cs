using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque
{
    public class ModeleDetailsObjetRisque
    {
        public int Code { get; set; }

        public string Descriptif { get; set; }
        public DateTime? DateEntree { get; set; }
        public string DateEntreeString
        {
            get
            {
                if (DateEntree.HasValue)
                    return new DateTime(DateEntree.Value.Year, DateEntree.Value.Month, DateEntree.Value.Day).ToShortDateString();
                else
                    return string.Empty;
            }
        }
        public DateTime? DateSortie { get; set; }
        public string DateSortieString
        {
            get
            {
                if (DateSortie.HasValue)
                    return new DateTime(DateSortie.Value.Year, DateSortie.Value.Month, DateSortie.Value.Day).ToShortDateString();
                else
                    return string.Empty;
            }
        }

        public DateTime? DateMinModifObj { get; set; }
        public double IndiceOrigine { get; set; }
        public double IndiceActualise { get; set; }
        public string Valeur { get; set; }
        public string UniteLibelle { get; set; }
        public string Unite { get; set; }
        public string TypeLibelle { get; set; }
        public string Type { get; set; }
        public string ValeurHTLibelle { get; set; }
        public string ValeurHT { get; set; }
        public string EnsembleType { get; set; }
        public bool HasInventaires { get; set; }
        public bool IsAlertePeriode { get; set; }
        public bool IsSortiAvenant { get; set; }
        public bool IsAfficheAvenant { get; set; }
        public string NumAvenantCreationRsq { get; set; }
        public DateTime? DateEffetAvenantOBJ { get; set; }


        public string DateEffetAvenantOBJString
        {
            get
            {
                if (DateEffetAvenantOBJ.HasValue)
                    return new DateTime(DateEffetAvenantOBJ.Value.Year, DateEffetAvenantOBJ.Value.Month, DateEffetAvenantOBJ.Value.Day).ToShortDateString();
                else
                    return string.Empty;
            }
        }

    }
}