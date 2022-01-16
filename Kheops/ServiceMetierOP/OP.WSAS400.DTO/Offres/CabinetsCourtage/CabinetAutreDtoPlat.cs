using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres.CabinetsCourtage
{
    public class CabinetAutreDtoPlat
    {
        [Column(Name = "CODE")]
        public Int32 Code { get; set; }
        [Column(Name = "NOM")]
        public String Nom { get; set; }
        [Column(Name = "DELEGATION")]
        public String Delegation { get; set; }
        [Column(Name = "SOUSCRIPTEUR")]
        public String Souscripteur { get; set; }
        [Column(Name = "DATESAISIE")]
        public Int32 DateSaisie { get; set; }
        [Column(Name = "HEURESAISIE")]
        public Int32 HeureSaisie { get; set; }
        [Column(Name = "DATECREATION")]
        public Int32 DateCreation { get; set; }
        [Column(Name = "HEURECREATION")]
        public Int32 HeureCreation { get; set; }
        [Column(Name = "ACTION")]
        public String Action { get; set; }
        [Column(Name = "MOTIF")]
        public String Motif { get; set; }
        [Column(Name = "LIBMOTIF")]
        public String LibelleMotif { get; set; }
        [Column(Name = "SOUSCRIPTEURNOM")]
        public String SouscripteurNom { get; set; }
    }
}
