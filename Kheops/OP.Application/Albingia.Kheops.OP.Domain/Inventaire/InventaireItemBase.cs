using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    [DataContract]
    [KnownType(typeof(PersonneDesigneeIndispo))]
    [KnownType(typeof(PersonneDesigneeIndispoTournage))]
    public class InventaireItemBase
    {
        //Cette classe ne sert qu'à permettre de passer les valeurs des propriétés dérivées d'une méthode à l'autre.
        //Pour une raison inconnue, les propriétés de la classe de base n'était pas retournées dans la méthode appelante.
    }
}
