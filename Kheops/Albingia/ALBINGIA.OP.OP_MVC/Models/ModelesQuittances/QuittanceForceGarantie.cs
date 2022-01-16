using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Quittance;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class QuittanceForceGarantie
    {
        public string FormuleId { get; set; }
        public QuittanceForceInfoFormule Formule { get; set; }
        public List<QuittanceForceInfoGarantie> ListGaranties { get; set; }
        public List<AlbSelectListItem> CodesTaxe { get; set; }
        public string CodeTaxe { get; set; }

        public static explicit operator QuittanceForceGarantie(QuittanceForceGarantieDto modeleDto)
        {
            var result = ObjectMapperManager.DefaultInstance.GetMapper<QuittanceForceGarantieDto, QuittanceForceGarantie>().Map(modeleDto);
            result.CodesTaxe = modeleDto.CodesTaxe.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
            return result;
        }
    }
}