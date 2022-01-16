using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Ecran.DoubleSaisie
{
    public class DoubleSaisieListeDto
    {
        public List<ParametreDto> MotifsRemp { get; set; }
        public List<ParametreDto> NotificationsApporteur { get; set; }
        public List<ParametreDto> NotificationsDemandeur { get; set; }
    }
}
