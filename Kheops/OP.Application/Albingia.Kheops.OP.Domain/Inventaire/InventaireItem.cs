using System;
using System.Runtime.Serialization;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public class InventaireItem : InventaireItemBase
    {
        public long Id { get; set; }
        public int NumeroLigne { get; set; }

        internal void ResetIds() {
            Id = 0;
        }
    }
}