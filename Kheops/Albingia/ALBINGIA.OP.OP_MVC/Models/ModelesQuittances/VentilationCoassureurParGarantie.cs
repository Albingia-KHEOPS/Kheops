using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class VentilationCoassureurParGarantie
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public double Part { get; set; }
        public List<CoAssureurGarantie> ListeGaranties { get; set; }
    }
}