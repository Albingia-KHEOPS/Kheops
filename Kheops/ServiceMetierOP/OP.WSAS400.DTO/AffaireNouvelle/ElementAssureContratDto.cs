using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Adresses;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    public class ElementAssureContratDto
    {
        public bool ElementPrincipal { get; set; }
        public AdressePlatDto Adresse { get; set; }
    }
}
