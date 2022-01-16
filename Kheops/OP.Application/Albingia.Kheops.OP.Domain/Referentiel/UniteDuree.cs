using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;
using System;

namespace Albingia.Kheops.OP.Domain.Referentiel {
    public enum UniteDureeValeur {
        [BusinessCode("")] Empty = 0,
        [BusinessCode("A")] Annee =4,
        [BusinessCode("M")] Mois =3,
        [BusinessCode("S")] Semaine =2,
        [BusinessCode("J")] Jour =1
    }

    public static class DateTimeExtenstion
    {
        public static DateTime AddUnit(this DateTime value, int amount, UniteDureeValeur unit) {
            switch (unit) {
                case UniteDureeValeur.Annee:
                    return value.AddYears(amount);
                case UniteDureeValeur.Mois:
                    return value.AddMonths(amount);
                case UniteDureeValeur.Semaine:
                    return value.AddDays(7*amount);
                case UniteDureeValeur.Jour:
                    return value.AddDays(amount);
                case UniteDureeValeur.Empty:
                default:
                    return value;
            }
        }
    }

    public class UniteDuree : RefValue {
        public const string Annee = "A";
        public const string Mois = "M";
        public const string Jour = "J";
        public const string Semaine = "S";

    }
}
