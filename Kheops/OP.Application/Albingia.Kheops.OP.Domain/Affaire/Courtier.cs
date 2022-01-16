using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Affaire
{
    public class Courtier
    {
        virtual public string Nom { get; set; }
        virtual public int Code { get; set; }
        virtual public int Numero { get; set; }
        virtual public Adresse Adresse { get; set; }
        virtual public string Email { get; set; }
        virtual public string TypeCourtier { get; set; }
        virtual public string Inspecteur { get; set; }
    }
}
