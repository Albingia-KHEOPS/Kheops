using ALBINGIA.Framework.Common.ExcelXmlMap;
using EmitMapper;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamIS
{
    public class ParamModeleLigneInfo
    {
        public int GuidId { get; set; }

        //Liste des champs
        public string SqlOrder { get; set; }
        public string Lib { get; set; }
        public string Cells { get; set; }
        public string DbMapCol { get; set; }
        public bool Link { get; set; }
        public string Type { get; set; }
        public string SqlRequest { get; set; }
        public string ConvertTo { get; set; }
        public int HierarchyOrder { get; set; }
        public string LineBreak { get; set; }
        public string TypeUIControl { get; set; }
        public string Required { get; set; }
        public string TextLabel { get; set; }
        public string LinkBehaviour { get; set; }
        public string Behaviour { get; set; }
        public string EventBehaviour { get; set; }
        public string Disabled { get; set; }

        //Listes génériques
        public ParamModeleListGenerique ListesGeneriques { get; set; }

        public static LigneInfo ToXmlLineInfo(ParamModeleLigneInfo paramLineInfo)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamModeleLigneInfo, LigneInfo>().Map(paramLineInfo);
        }
    }




}