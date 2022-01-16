using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public class PersonneIndispo : Personne
    {
        public string Franchise { get; set; }

        public decimal Montant { get; set; }
    }
    public class PersonneDesigneeIndispo : PersonneIndispo
    {
        public Indisponibilite Extention { get; set; }

    }
    public class PersonneDesigneeIndispoTournage : PersonneIndispo
    {
        public IndisponibiliteTournage Extention { get; set; }

    }
}