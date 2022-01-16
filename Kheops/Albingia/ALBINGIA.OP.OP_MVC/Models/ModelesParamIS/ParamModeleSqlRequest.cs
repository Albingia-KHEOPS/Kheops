using ALBINGIA.Framework.Common.ExcelXmlMap;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamIS
{
    public class ParamModeleSqlRequest
    {      
        public string Select { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string SelectExist { get; set; }
        public string RequeteEdit { get; set; }
        public string TypeEdit { get; set; }

        public static SqlRequests ToXmlRequest(ParamModeleSqlRequest paramRequest)
        {
            return new SqlRequests
            {
                Select = new Request { Sql = paramRequest.Select },
                Update = new Request { Sql = paramRequest.Update },
                Insert = new Request { Sql = paramRequest.Insert },
                SelectExist = new Request { Sql = paramRequest.SelectExist }
            };
        }
    }
}