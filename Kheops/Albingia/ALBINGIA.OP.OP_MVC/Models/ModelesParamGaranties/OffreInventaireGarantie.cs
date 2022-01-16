using EmitMapper;
using OP.WSAS400.DTO.ParametreGaranties;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamGaranties
{
    public class OffreInventaireGarantie
    {      
        public string CodeOffre { get; set; }    
        public int Version { get; set; }     
        public string Type { get; set; }
        public int CodeInventaire { get; set; }
        public int CodeFormule { get; set; }      
        public string CodeGarantie { get; set; }
        public string LettreFormule { get; set; }    
        public string LibelleFormule { get; set; }
        public static explicit operator OffreInventaireGarantie(OffreInventaireGarantieDto offreInvenGar)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<OffreInventaireGarantieDto, OffreInventaireGarantie>().Map(offreInvenGar);
        }
    }
}