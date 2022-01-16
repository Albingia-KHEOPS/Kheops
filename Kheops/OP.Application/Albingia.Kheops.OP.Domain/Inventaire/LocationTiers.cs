using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public class LocationTiers : Activite
    {
        public new TypeLocation Code { get => (TypeLocation)base.Code; set => base.Code = value; }

    }
}