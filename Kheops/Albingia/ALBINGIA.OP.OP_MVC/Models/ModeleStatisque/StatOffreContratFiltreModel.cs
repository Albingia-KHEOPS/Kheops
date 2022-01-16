using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.Stat;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleStatisque
{
    public class StatOffreContratFiltreModel
    {
        /// <summary>
        /// context de la page a affecter dans les hidden inputs
        /// </summary>
        public string Albcontext { get; set; }

        public string Branche { get; set; }
        /// <summary>
        /// kheopsProduction(KHE) ou non 
        /// </summary>
        public string Categorie { get; set; }
        public string Situation { get; set; }
        public string Etat { get; set; }
        public string Type { get; set; }
        public string Annee { get; set; }
        public string Mois { get; set; }
        public string Jour { get; set; }

        public List<AlbSelectListItem> Types { get; set; }
        public List<AlbSelectListItem> Branches { get; set; }
        public List<AlbSelectListItem> Situations { get; set; }
        public List<AlbSelectListItem> Etats { get; set; }


        public static explicit operator StatOffreContratFiltreModel(StatOffreContratFiltreDto dto)
        {
            return new StatOffreContratFiltreModel
            {
                Branche = dto.Branche,
                Categorie = dto.Categorie,
                Situation = dto.Situation,
                Etat=dto.Etat,
                Type = dto.Type,
                Annee = dto.Annee,
                Mois = dto.Mois,
                Jour = dto.Jour
            };
        }

        public StatOffreContratFiltreDto ToDto()
        {
            return new StatOffreContratFiltreDto 
            {
                Branche = this.Branche,
                Categorie = this.Categorie,
                Situation = this.Situation,
                Etat=this.Etat,
                Type = this.Type,
                Annee = this.Annee,
                Mois = this.Mois,
                Jour = this.Jour
            };
        }
    }
}