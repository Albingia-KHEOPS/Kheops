using Albingia.Kheops.OP.Domain.Referentiel;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Domain.Affaire
{

    public class Assure
    {
        virtual public int Code { get; set; }
        virtual public int Numero { get; set; }

        virtual public string NomAssure { get; set; }
        virtual public List<string> NomSecondaires { get; set; }
        virtual public Adresse Adresse { get; set; }
        virtual public string Siren { get; set; }
        virtual public string TelephoneBureau { get; set; }
        virtual public bool PreneurEstAssure { get; set; }
    }
}