using EmitMapper;
using OP.WSAS400.DTO.Common;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.Avenant.ModelesAvenant
{
    public class ModeleVisuInfosContrat
    {
        public string CodeOffre { get; set; }
        public int Version { get; set; }

        public DateTime? DateDebutEffet { get; set; }
        public DateTime? DateFinEffet { get; set; }
        public DateTime? DateEcheance { get; set; }
        public string Periodicite { get; set; }
        public string CourtierA { get; set; }
        public string CourtierG { get; set; }
        public string Assure { get; set; }
        public string Identification { get; set; }
        public string NatureContrat { get; set; }
        public string TypeContrat { get; set; }
        public string Souscripteur { get; set; }
        public string Gestionnaire { get; set; }
        public string Observation { get; set; }
        public string RetourSigne { get; set; }


        public static explicit operator ModeleVisuInfosContrat(VisuInfosContratDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<VisuInfosContratDto, ModeleVisuInfosContrat>().Map(modeleDto);
        }
    }
}