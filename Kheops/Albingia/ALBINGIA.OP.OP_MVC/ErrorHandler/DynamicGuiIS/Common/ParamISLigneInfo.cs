//using EmitMapper;

using EmitMapper;
using OP.WSAS400.DTO.ParamIS;

namespace ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common
{
    public class ParamISLigneInfo
    {
        /// <summary>
        /// Ancien Nom :Lib
        /// </summary>
       public string ModeleID { get; set; }
        public string InternalPropertyName { get; set; }
        public int Code { get; set; }
        public string Cells { get; set; }
        public string TextLabel { get; set; }
        public string Type { get; set; }
        /// <summary>
        /// taille du champ type
        /// </summary>
        public string LongueurType { get; set; }
        public string Link { get; set; }
        public string ConvertTo { get; set; }
        public int HierarchyOrder { get; set; }
        public string TypeUIControl { get; set; }
        public string Required { get; set; }
        /// <summary>
        /// Présence script affichage
        /// </summary>
        public string ScriptAffichage { get; set; }
        /// <summary>
        /// Présence script controle
        /// </summary>
        public string ScriptControle { get; set; }
        public string Observation { get; set; }
        public string DbMapCol { get; set; }
        public float NumOrdreAffichage { get; set; }
        public int LineBreak { get; set; }
        public string Disabled { get; set; }
        public string SqlRequest { get; set; }
        public int SqlOrder { get; set; }
        public string LinkBehaviour { get; set; }
        public string Behaviour { get; set; }
        public string EventBehaviour { get; set; }

        public static explicit operator ParamISLigneInfo(ParamISLigneInfoDto data)
        {
            ParamISLigneInfo toReturn = ObjectMapperManager.DefaultInstance.GetMapper<ParamISLigneInfoDto, ParamISLigneInfo>().Map(data);
            if (string.IsNullOrEmpty(toReturn.TypeUIControl))
                toReturn.TypeUIControl = string.Empty;
            return toReturn;
        }
    }

    public class SqlRequests
    {
        public Request SelectExist { get; set; }
        public Request Select { get; set; }
        public Request Insert { get; set; }
        public Request Update { get; set; }
    }

    public class Request
    {
        public string Sql { get; set; }
    }
}