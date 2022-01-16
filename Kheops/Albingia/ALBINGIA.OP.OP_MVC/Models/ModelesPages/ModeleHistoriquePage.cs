using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesHistorique;
using EmitMapper;
using OP.WSAS400.DTO.Historique;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleHistoriquePage : MetaModelsBase 
    {
        public string CodeAffaire { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }

        public bool IsContractuel { get; set; }
        public List<HistoriqueLigne> ListHistorique { get; set; }

        public bool IsModeDivFlottante { get; set; }

        public static explicit operator ModeleHistoriquePage(HistoriqueDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<HistoriqueDto, ModeleHistoriquePage>().Map(modeleDto);
        }

        public static HistoriqueDto LoadDto(ModeleHistoriquePage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleHistoriquePage, HistoriqueDto>().Map(modele);
        }

    }
}