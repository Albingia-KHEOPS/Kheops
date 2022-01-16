using OP.WSAS400.DTO.Stat;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleStatisque
{
    public class StatOffreContratModel
    {


        public string Num { get; set; }
        public int Version { get; set; }
        public string Type { get; set; }
        public string Reference { get; set; }
        public string Gestionnaire { get; set; }
        public string Souscripteur { get; set; }
        public string Branche { get; set; }
        public string Cible { get; set; }

        /// <summary>
        /// context de la page a affecter dans les hidden inputs
        /// </summary>
        public string Albcontext { get; set; }

        public static explicit operator StatOffreContratModel(StatOffreContratDto dto)
        {
            return new StatOffreContratModel
            {
               Num=dto.Num,
               Version=dto.Version,
               Type=dto.Type,
               Reference=dto.Reference,
               Gestionnaire=dto.Gestionnaire,
               Souscripteur=dto.Souscripteur,
               Branche=dto.Branche,
               Cible=dto.Cible
            };
        }
    }
}