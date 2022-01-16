using OP.WSAS400.DTO.Offres.Indice;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleTransverse
{
    public class ModeleIndice
    {
        public string Indice { get; set; }
                
        public static explicit operator ModeleIndice(IndiceGetResultDto indiceDto)
        {
            var toReturn = new ModeleIndice();
            if (indiceDto.Valeur.Length > 0)
                toReturn.Indice = indiceDto.Valeur;
            else
                toReturn.Indice = string.Empty;
            return toReturn;
        }
    }
}