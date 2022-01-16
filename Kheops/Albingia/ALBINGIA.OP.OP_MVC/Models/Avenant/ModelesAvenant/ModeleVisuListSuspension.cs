using EmitMapper;
using OP.WSAS400.DTO.Common;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.Avenant.ModelesAvenant
{
    public class ModeleVisuListSuspension
    {
        public Int64 Id { get; set; }

        public string CodeOffre { get; set; }

        public Int16 Version { get; set; }

        public string Type { get; set; }

        public DateTime? DateMise { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public DateTime? DateResil { get; set; }
        public DateTime? DateRemise { get; set; }

        public static explicit operator ModeleVisuListSuspension(VisuListSuspensionDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<VisuListSuspensionDto, ModeleVisuListSuspension>().Map(modeleDto);
        }
    }
}