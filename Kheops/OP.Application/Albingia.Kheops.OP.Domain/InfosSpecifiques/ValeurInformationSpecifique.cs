using Albingia.Kheops.OP.Domain.Referentiel;
using System;

namespace Albingia.Kheops.OP.Domain.InfosSpecifiques
{
    public class ValeurInformationSpecifique
    {
        public string Texte { get; set; }
        public decimal Nombre { get; set; }
        public (string code, string libelle) Unite { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }

        public (string val1, string val2) ToVal1Val2(LigneModeleIS ligne)
        {
            if (ligne is null)
            {
                return (string.Empty, string.Empty);
            }

            switch (ligne.Propriete.TypeUIControl)
            {
                case TypeAffichage.Periode:
                    return (DateMin?.ToShortDateString() ?? string.Empty, DateMax?.ToShortDateString() ?? string.Empty);
                case TypeAffichage.PeriodeHeure:
                    return (DateMin.ToString(), DateMax.ToString());
                case TypeAffichage.Date:
                    return (DateMin?.ToShortDateString(), string.Empty);
                case TypeAffichage.Heure:
                    return (DateMin?.ToShortTimeString() ?? string.Empty, string.Empty);
                case TypeAffichage.Text:
                    if (ligne.Propriete.Type.ToUpper() == TypeCode.Double.ToString().ToUpper())
                    {
                        return (Nombre.ToString(), string.Empty);
                    }
                    else if (ligne.Propriete.Type.ToUpper() == TypeCode.Int64.ToString().ToUpper())
                    {
                        return (((int)Nombre).ToString(), string.Empty);
                    }
                    return (Texte, string.Empty);
                default:
                    return (Texte, string.Empty);
            }
        }

        public void SetVals(LigneModeleIS ligne, string val1, string val2, string unt)
        {
            if (ligne is null)
            {
                return;
            }
            switch (ligne.Propriete.TypeUIControl)
            {
                case TypeAffichage.Periode:
                case TypeAffichage.PeriodeHeure:
                    DateMin = DateTime.TryParse(val1, out var d) ? d : default(DateTime?);
                    DateMax = DateTime.TryParse(val2, out d) ? d : default(DateTime?);
                    break;
                case TypeAffichage.Date:
                case TypeAffichage.Heure:
                    DateMin = DateTime.TryParse(val1, out d) ? d : default(DateTime?);
                    break;
                case TypeAffichage.Text:
                    if (ligne.Propriete.Type.ToUpper() == TypeCode.Double.ToString().ToUpper())
                    {
                        val1 = val1.Replace(".", ",");
                        Nombre = decimal.TryParse(val1, out decimal dc) ? dc : 0M;
                    }
                    else if (ligne.Propriete.Type.ToUpper() == TypeCode.Int64.ToString().ToUpper())
                    {
                        Nombre = int.TryParse(val1, out int i) ? i : 0;
                    }
                    else
                    {
                        Texte = val1;
                    }
                    if (!string.IsNullOrWhiteSpace(unt))
                    {
                        Unite= (unt, string.Empty);
                    }
                    break;
                default:
                    Texte = val1;
                    break;
            }
        }
    }
}
