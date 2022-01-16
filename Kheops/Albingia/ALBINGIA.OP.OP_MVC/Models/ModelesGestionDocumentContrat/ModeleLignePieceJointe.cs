using EmitMapper;
using OP.WSAS400.DTO.GestionDocumentContrat;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGestionDocumentContrat
{
    public class ModeleLignePieceJointe
    {
        public string LibelleDocument { get; set; }
        public string CheminFichier { get; set; }
        public string NomFichier { get; set; }
        public int Taille { get; set; }

        public static explicit operator ModeleLignePieceJointe(LignePieceJointeDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LignePieceJointeDto, ModeleLignePieceJointe>().Map(data);
        }
    }
}