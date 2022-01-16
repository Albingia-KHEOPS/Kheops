using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Concerts;

namespace OP.WSAS400.DTO.Ecran.DetailsInventaire
{
    [DataContract]
    public class DetailsInventaireObjetSetQueryDto : _DetailsInventaire_Base, IQuery
    {
        [DataMember]
        public ConcertDto Concert { get; set; }
    }
}
