using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.SMP;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleEngagements
{
    public class ModeleLignesDetailCalculSMP
    {
        public List<ModeleLigneDetailCalculSMP> ListeGarantie { get; set; }
        public List<AlbSelectListItem> Types { get; set; }
        [Display(Name = "SMP total (€)")]
        public string SMPtotal { get; set; }

        public static explicit operator ModeleLignesDetailCalculSMP(SMPdto TraiteDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SMPdto, ModeleLignesDetailCalculSMP>().Map(TraiteDto);
        }

        public static SMPdto LoadDto(ModeleLignesDetailCalculSMP modele)
        {

            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleLignesDetailCalculSMP, SMPdto>().Map(modele);
        }
    }
}