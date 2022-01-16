
using System;

namespace Hexavia.Models
{
    public class KGeolocCase : KGeoloc
    {
        public string NumContrat { get; set; }
        public int Version { get; set; }
        public string Reference { get; set; }
        public string Type { get; set; }
        public short NumInterneAvenant { get; set; }
        public string TypeTraitement { get; set; }
        public string Branche { get; set; }

        public string DateSaisie { get; set; }

        public string CourtierGestionnaire { get; set; }

        public string AssureGestionnaire { get; set; }
        public string Smp { get; set; }

        public bool IsExternal { get; set; }

        public bool IsKheops { get; set; }

        // Permet de distinguer les contrats des sinistres. Utilisé uniquement pour la création des liens Kheops/Citrix
        public string TypeAtlas { get; set; }
        public KGeolocCase(int year, int month, int day)//Nullable<DateTime> date)
        {
            Nullable<DateTime> date = null;
            if (year > 0 && month > 0 && day > 0)
            {
                date = new DateTime(year, month, day);
            }
            DateSaisie = string.Empty;
            if (date.HasValue)
            {
                // DateSaisie = string.Join("/", date.Value.Day.ToString(), date.Value.Month.ToString(), date.Value.Year.ToString());
                DateSaisie = date.Value.ToString("dd/MM/yyyy");
            }
            IsExternal = false;
            IsKheops = true;
        }
    }
}
