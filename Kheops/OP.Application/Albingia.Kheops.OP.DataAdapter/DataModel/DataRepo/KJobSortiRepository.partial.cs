using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using ALBINGIA.Framework.Common.Data;
using Dapper;
using System.Collections.Generic;
using System.Data.EasycomClient;
using System.Linq;
using System.Reflection;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{
    public partial class KJobSortiRepository
    {
        const string delete_ByRisque = "DELETE FROM KJOBSORTI WHERE ( IPB , ALX , TYP , AVN , RSQ ) = ( :codeAffaire , :version , :type , :numeroAvenant , :risque )";
        const string delete_ByOption = "DELETE FROM KJOBSORTI WHERE ( IPB , ALX , TYP , AVN, OPT ) = ( :codeAffaire , :version , :type, :numeroAvenant , :option )";
        const string insertQuery = @"INSERT INTO KJOBSORTI";

        public int InsertMultiple(IEnumerable<KJobSorti> list)
        {
            if (list?.Any() != true)
            {
                return 0;
            }

            var phProps = typeof(KJobSorti).GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance).Where(p => p.Name.ToUpper() != "HIN").OrderBy(p => p.Name);
            string colmunList = " ( " + string.Join(" , ", phProps.Select(p => p.Name.ToUpper())) + " )";
            string valuesQuery = " VALUES ( " +
                string.Join(" ) , ( ", list.Select((item, x) => string.Join(" , ", phProps.Select(p => $":{p.Name.ToUpper()}_{x}")))) + " )";

            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = insertQuery + colmunList + valuesQuery
            }) {
                var plist = new List<EacParameter>();
                int count = 0;
                foreach (var item in list)
                {
                    foreach (var p in phProps)
                    {
                        plist.Add(new EacParameter($"{p.Name.ToUpper()}_{count}", p.GetValue(item)));
                    }
                    count++;
                }
                options.Parameters = plist;
                options.Exec();
                return options.ReturnedValue;
            }
        }

        public int DeleteByRisque(string codeAffaire, int version , string type , int numeroAvenant , int risque) {
            return this.connection.EnsureOpened().Execute(delete_ByRisque, new { codeAffaire, version, type, numeroAvenant, risque });
        }

        public int DeleteByOption(string codeAffaire, int version, string type, int numeroAvenant, int option)
        {
            return this.connection.EnsureOpened().Execute(delete_ByOption, new { codeAffaire, version, type, numeroAvenant, option });
        }
    }
}
