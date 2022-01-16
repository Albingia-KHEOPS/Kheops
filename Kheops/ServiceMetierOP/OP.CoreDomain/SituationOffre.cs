using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    public enum SituationOffre
    {
        Indetermine,
        Encours,
        SansSuite,
        Realisee,
        Attente,
        Annule
    }

    public static class SituationOffreConverter
    {
        private const string Annule = "Annulée";
        private const string Attente = "Attente";
        private const string Encours = "En cours";
        private const string Indetermine = "Indéterminée";
        private const string Realisee = "Réalisée";
        private const string SansSuite = "Sans suite";

        public static string ToString(SituationOffre situationOffre)
        {
            string result = string.Empty;
            switch (situationOffre)
            {
                case SituationOffre.Annule:
                    result = Annule;
                    break;
                case SituationOffre.Attente:
                    result = Attente;
                    break;
                case SituationOffre.Encours:
                    result = Encours;
                    break;
                case SituationOffre.Indetermine:
                    result = Indetermine;
                    break;
                case SituationOffre.Realisee:
                    result = Realisee;
                    break;
                case SituationOffre.SansSuite:
                    result = SansSuite;
                    break;
            }
            return result;
        }

        public static SituationOffre FromString(string situationOffre)
        {
            SituationOffre result = SituationOffre.Indetermine;
            switch (situationOffre)
            {
                case Annule:
                    result = SituationOffre.Annule;
                    break;
                case Attente:
                    result = SituationOffre.Attente;
                    break;
                case Encours:
                    result = SituationOffre.Encours;
                    break;
                case Indetermine:
                    result = SituationOffre.Indetermine;
                    break;
                case Realisee:
                    result = SituationOffre.Realisee;
                    break;
                case SansSuite:
                    result = SituationOffre.SansSuite;
                    break;
            }
            return result;
        }
    }
}
