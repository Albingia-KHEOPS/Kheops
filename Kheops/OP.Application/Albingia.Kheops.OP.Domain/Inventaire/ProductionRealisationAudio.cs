using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public class ProductionRealisationAudio : Activite
    {
        public new TypeProdution Code { get => (TypeProdution)base.Code; set => base.Code = value; }

    }
}