using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using EmitMapper;
using OP.WSAS400.DTO.Common;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.Avenant.ModelesAvenant
{
    public class ModeleVisuSuspension : MetaModelsBase
    {
        public List<ModeleVisuListSuspension> Suspensions { get; set; }
        public ModeleVisuInfosContrat InfosContrat { get; set; }

        public string CodeOffre { get; set; }
        public int Annee { get; set; }
        public int Version { get; set; }
        public int NumAvenant { get; set; }
        public List<AlbSelectListItem> Years { get; set; }

        public static explicit operator ModeleVisuSuspension(VisuSuspensionDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<VisuSuspensionDto, ModeleVisuSuspension>().Map(modeleDto);
        }
    }
}