using EmitMapper;
using OP.WSAS400.DTO.SMP;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleEngagements
{
    [Serializable]
    public class ModeleLigneDetailCalculSMP
    {
        public string IdGarantie { get; set; }
        public string CheckBox { get; set; }
        public string NomGarantie { get; set; }
        public string LCI { get; set; }
        public string SMPcalcule { get; set; }
        public string Type { get; set; }
        public string Valeur { get; set; }
        public string SMPretenu { get; set; }

        public static explicit operator ModeleLigneDetailCalculSMP(LigneSMPdto TraiteDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LigneSMPdto, ModeleLigneDetailCalculSMP>().Map(TraiteDto);
        }

        public static LigneSMPdto LoadDto(ModeleLigneDetailCalculSMP modele)
        {

            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleLigneDetailCalculSMP, LigneSMPdto>().Map(modele);
        }
    }
}