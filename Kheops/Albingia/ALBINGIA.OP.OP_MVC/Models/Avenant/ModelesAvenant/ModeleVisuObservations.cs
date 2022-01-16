using EmitMapper;
using OP.WSAS400.DTO.Common;
namespace ALBINGIA.OP.OP_MVC.Models.Avenant.ModelesAvenant
{
    public class ModeleVisuObservations
    {
        public string ObsvInfoGen { get; set; }
        public string ObsvCotisation { get; set; }
        public string ObsvEngagement { get; set; }
        public string ObsvMntRef { get; set; }
        public string ObsvRefGest { get; set; }
        public static explicit operator ModeleVisuObservations(VisuObservationsDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<VisuObservationsDto, ModeleVisuObservations>().Map(modeleDto);
        }

       
    }
}