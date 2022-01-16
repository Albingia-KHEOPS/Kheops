using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.Common;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleActeGestion
{
    public class ActeGestion
    {
        public string Date { get; set; }
        public string Heure { get; set; }
        public int Numero { get; set; }
        public string TypeTraitement { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public string Utilisateur { get; set; }
        public string CodeTraitement { get; set; }
        public Int64 DateCreationInt { get; set; }
        public DateTime? DateCreation { get; set; }

        public static explicit operator ActeGestion(ActeGestionDto acteDto)
        {
            ActeGestion acteGestion = ObjectMapperManager.DefaultInstance.GetMapper<ActeGestionDto, ActeGestion>().Map(acteDto);
            acteGestion.Date = new DateTime(acteDto.DateAnnee, acteDto.DateMois, acteDto.DateJour).ToShortDateString();
            string[] heure = AlbConvert.ConvertIntToTimeMinute(acteDto.Heure).Value.ToString().Split(':');
            acteGestion.Heure = heure[0] + ":" + heure[1];
            if (acteDto.TypeTraitement == "G") acteGestion.TypeTraitement = "Gestion";
            else if (acteDto.TypeTraitement == "V") acteGestion.TypeTraitement = "Validation";
            acteGestion.DateCreation = AlbConvert.ConvertIntToDate(Convert.ToInt32(acteGestion.DateCreationInt));
            return acteGestion;
        }
        public static List<ActeGestion> LoadActesGestion(List<ActeGestionDto> actesDto)
        {
            var actesGestion = new List<ActeGestion>();
            if (actesDto != null && actesDto.Count > 0)
                foreach (var acteDto in actesDto)
                    actesGestion.Add((ActeGestion)acteDto);
            return actesGestion;
        }
    }
}