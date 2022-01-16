using ALBINGIA.Framework.Common.Models.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using EmitMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Log = OP.WSAS400.DTO.Logging;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleLogPerfPage : MetaModelsBase
    {
        //public List<LogPerf> LogTraces;
        public List<LogPerf> LogPerfs;

        [Display(Name = "Date de début:")]
        public DateTime? DateDebutFiltre;
        [Display(Name = "Date de fin:")]
        public DateTime? DateFinFiltre;

        public static List<LogPerf> LoadListPerf(List<Log.LogPerf> listLogPerf)
        {
            List<LogPerf> list = new List<LogPerf>();

            foreach (var item in listLogPerf)
            {
                //Solution temporaire, mais datelog = null...
                //LogPerf obj = new LogPerf
                //{
                //    DateLog = AlbConvert.ConvertStrToDate(item.DateLog),
                //    User = item.User,
                //    Screen = item.Screen,
                //    Action = item.Action,
                //    ElapsedTime = item.ElapsedTime
                //};

                var obj = ObjectMapperManager.DefaultInstance.GetMapper<Log.LogPerf, LogPerf>().Map(item);
                list.Add(obj);
            }
            return list;
        }
    }
}