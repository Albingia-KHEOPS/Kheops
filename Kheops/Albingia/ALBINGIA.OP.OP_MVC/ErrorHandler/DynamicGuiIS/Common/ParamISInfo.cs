using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common
{
    public class ParamISInfo
    {
        public string Name { get; set; }
        public List<ParamISLigneInfo> ParamISDBLignesInfo { get; set; }
        public SqlRequests SqlRequests { get; set; }
    }
}