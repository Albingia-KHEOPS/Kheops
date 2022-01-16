using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesRetours
{
    public class ModeleLigneRetourCoAss
    {
        public string GuidId { get; set; }
        public string NomCoassureur { get; set; }
        public float Part { get; set; }
        public int DateRetourCoAss { get; set; }
        public List<AlbSelectListItem> TypeAccordCoAss { get; set; }
        public string TypeAccordVal { get; set; }

        public static explicit operator ModeleLigneRetourCoAss(RetourCoassureurDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<RetourCoassureurDto, ModeleLigneRetourCoAss>().Map(data);
        }
    }
}