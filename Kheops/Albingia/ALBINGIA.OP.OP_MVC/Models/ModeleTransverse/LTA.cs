using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.LTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.ModeleTransverse
{
    public class LTA
    {
        public bool ReadOnly { get; set; }
        public List<AlbSelectListItem> Durees { get; set; }

        public Int32? DateDeb { get; set; }
        public DateTime? DateDebLTA { get; set; }
        public Int16? HeureDeb { get; set; }
        public TimeSpan? HeureDebLTA { get; set; }
        public Int32? DateFin { get; set; }
        public DateTime? DateFinLTA { get; set; }
        public Int16? HeureFin { get; set; }
        public TimeSpan? HeureFinLTA { get; set; }
        public Single SeuilLTA { get; set; }
        public bool EffetLTACheck { get; set; }
        public bool DureeLTACheck { get; set; }
        public int DureeLTA { get; set; }
        public string DureeLTAString { get; set; }

        public static explicit operator LTA(LTADto dto)
        {
            var model = ObjectMapperManager.DefaultInstance.GetMapper<LTADto, LTA>().Map(dto);
            model.DateDebLTA = AlbConvert.ConvertIntToDate(model.DateDeb);
            model.HeureDebLTA = model.DateDeb > 0 ? AlbConvert.ConvertIntToTimeMinute(model.HeureDeb) : null;
            model.DateFinLTA = AlbConvert.ConvertIntToDate(model.DateFin);
            model.HeureFinLTA = model.DateFin > 0 ? AlbConvert.ConvertIntToTimeMinute(model.HeureFin) : null;

            model.DureeLTACheck = model.DureeLTA > 0;
            model.EffetLTACheck = model.DateFin > 0 && !model.DureeLTACheck;

            model.SeuilLTA = model.SeuilLTA != 0 ? model.SeuilLTA : 60;

            var durees = dto.Durees.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
            model.Durees = durees;

            return model;
        }
        public static LTADto LoadDto(LTA modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LTA, LTADto>().Map(modele);
        }

    }
}