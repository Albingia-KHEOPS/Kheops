using OP.WSAS400.DTO.Offres.Parametres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class QuittanceVisualisationDto
    {
        public List<ParametreDto> TypesOperation { get; set; }      
        public List<QuittanceVisualisationLigneDto> ListQuittances { get; set; }

        public DateTime? PeriodeDebut { get; set; }
        public string Situation { get; set; }
    }
}
