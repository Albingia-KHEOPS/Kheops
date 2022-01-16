using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public class ActivitesImmobilieres : Activite
    {
        public new TypeImmobilier Code { get => (TypeImmobilier)base.Code; set => base.Code = value; }
    }
}