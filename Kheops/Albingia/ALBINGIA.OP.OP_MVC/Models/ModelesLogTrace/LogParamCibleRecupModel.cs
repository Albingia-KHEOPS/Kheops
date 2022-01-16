using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO.Logging;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesLogTrace
{
    public class LogParamCibleRecupModel
    {       

        /// <summary>
        /// N° aliment
        /// </summary>
        public int? NumAliment { get; set; }

        /// <summary>
        /// N° avenant
        /// </summary>
        public int? NumAvenant { get; set; }


        /// <summary>
        /// Code situation
        /// </summary>
        public string CodeSituation { get; set; }

        /// <summary>
        /// Date situation
        /// </summary>
        public int DateSituation { get; set; }

        /// <summary>
        /// N° histo
        /// </summary>
        public int? NumHisto { get; set; }

        /// <summary>
        /// N° police/offre
        /// </summary>
        public string NumOffrePolice { get; set; }

        /// <summary>
        ///N° travail
        /// </summary>
        public int NumTravail { get; set; }

        /// <summary>
        ///Id objet
        /// </summary>
        public int? IdObjet { get; set; }

        /// <summary>
        ///Option récupération
        /// </summary>
        public string OptionRecuperation { get; set; }

        /// <summary>
        /// Id risque
        /// </summary>
        public int? IdRisque { get; set; }

        /// <summary>
        /// Commentaire situation
        /// </summary>
        public string CommentaireSituation { get; set; }

        /// <summary>
        /// Type  O Offre  P Police
        /// </summary>
        public string TypeOffrePolice { get; set; }

        /// <summary>
        /// User situation
        /// </summary>
        public string UserSituation { get; set; }

        /// <summary>
        /// context de la page a affecter dans les hidden inputs
        /// </summary>
        public string Albcontext { get; set; }


        public List<AlbSelectListItem> Types { get; set; }

        public static explicit operator LogParamCibleRecupModel(LogParamCibleRecupDto dto)
        {
            return new LogParamCibleRecupModel
            {
              NumAliment=dto.NumAliment,
              NumAvenant = dto.NumAvenant,
              CodeSituation = dto.CodeSituation,
              DateSituation = dto.DateSituation,
              NumHisto = dto.NumHisto,
              NumOffrePolice = dto.NumOffrePolice,
              NumTravail = dto.NumTravail,
              IdObjet = dto.IdObjet,
              OptionRecuperation = dto.OptionRecuperation,
              IdRisque = dto.IdRisque,
              CommentaireSituation = dto.CommentaireSituation,
              TypeOffrePolice = dto.TypeOffrePolice,
              UserSituation = dto.UserSituation
            };
        }

        public LogParamCibleRecupDto ToDto()
        {
            return new LogParamCibleRecupDto
            {
                NumAliment =this.NumAliment!=null ? (int)this.NumAliment :0,
                NumAvenant =this.NumAvenant!=null ? (int)this.NumAvenant :0,
                CodeSituation = this.CodeSituation,
                DateSituation = this.DateSituation,
                NumHisto = this.NumHisto !=null ? (int)this.NumHisto :0,
                NumOffrePolice = this.NumOffrePolice,
                NumTravail = this.NumTravail,
                IdObjet = this.IdObjet !=null ? (int)this.IdObjet :0,
                OptionRecuperation = this.OptionRecuperation,
                IdRisque = this.IdRisque!=null ? (int)this.IdRisque :0,
                CommentaireSituation = this.CommentaireSituation,
                TypeOffrePolice = this.TypeOffrePolice,
                UserSituation = this.UserSituation
            };
        }
    }
}