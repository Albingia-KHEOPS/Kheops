using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationAttestation
{
    public class ModeleAttestationFormule
    {
        public string LettreFormule { get; set; }
        public string LibFormule { get; set; }
        public List<ModeleAttestationObjet> Objets { get; set; }
        public List<ModeleAttestationGarantieNiv1> Garanties { get; set; }
    }
}