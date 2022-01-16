using EmitMapper;
using OP.WSAS400.DTO.Ecran.DetailsObjetRisque;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque
{
    public class ModeleLigneActivite
    {
        public string  Code { get; set; }
        public string  LibelleCourt { get; set; }
        public string  LibelleLong { get; set; }
        public string  ClassRsq { get; set; }
        public string  ClassTarif { get; set; }
        public string  Description { get; set; }
        public string NomActivite { get; set; }
        public string Concept { get; set; }
        public string Famille { get; set; }

        public static explicit operator ModeleLigneActivite(ActiviteDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ActiviteDto, ModeleLigneActivite>().Map(data);
        }
    }
}