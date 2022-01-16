using ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles;
using EmitMapper;
using OP.WSAS400.DTO.Modeles;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleModeles
{
    [Serializable]
    public class ModeleModele
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreation { get; set; }
        public string Caractere { get; set; }
        public string GuidId { get; set; }

        public List<ModeleGarantieNiveau1> Modeles { get; set; }


        public ModeleModele()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
            this.Caractere = string.Empty;
            this.DateCreation = null;
            this.GuidId = string.Empty;

            this.Modeles = new List<ModeleGarantieNiveau1>();
        }

        public static explicit operator ModeleModele(ModeleDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleDto, ModeleModele>().Map(modeleDto);
        }
}
}