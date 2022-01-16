using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using ALBINGIA.Framework.Common.Data;
using Dapper;
using System.Collections.Generic;
using System.Data.EasycomClient;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YpoAssxRepository {
        const string delete_ByAffaire = @"DELETE FROM YPOASSX WHERE ( PDIPB , PDALX , PDTYP ) = ( :code , :version , :type )";
        const string insertQuery = "INSERT INTO YPOASSX ( PDTYP, PDIPB, PDALX, PDQL1, PDQL2, PDQL3, PDQLD )";

        public int InsertMultiple(IEnumerable<YpoAssx> ypoAssxList) {
            if (ypoAssxList?.Any() != true) {
                return 0;
            }
            string valuesQuery = " VALUES " + string.Join(" , ", ypoAssxList.Select((item, x) => $" ( :{nameof(item.Pdtyp).ToUpper()}_{x}, :{nameof(item.Pdipb).ToUpper()}_{x}, :{nameof(item.Pdalx).ToUpper()}_{x}, :{nameof(item.Pdql1).ToUpper()}_{x}, :{nameof(item.Pdql2).ToUpper()}_{x}, :{nameof(item.Pdql3).ToUpper()}_{x}, :{nameof(item.Pdqld).ToUpper()}_{x} ) "));
            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = insertQuery + valuesQuery
            }) {
                var plist = new List<EacParameter>();
                int count = 0;
                foreach(var item in ypoAssxList) {
                    plist.Add(new EacParameter($"{nameof(item.Pdtyp).ToUpper()}_{count}", item.Pdtyp));
                    plist.Add(new EacParameter($"{nameof(item.Pdipb).ToUpper()}_{count}", item.Pdipb));
                    plist.Add(new EacParameter($"{nameof(item.Pdalx).ToUpper()}_{count}", item.Pdalx));
                    plist.Add(new EacParameter($"{nameof(item.Pdql1).ToUpper()}_{count}", item.Pdql1));
                    plist.Add(new EacParameter($"{nameof(item.Pdql2).ToUpper()}_{count}", item.Pdql2));
                    plist.Add(new EacParameter($"{nameof(item.Pdql3).ToUpper()}_{count}", item.Pdql3));
                    plist.Add(new EacParameter($"{nameof(item.Pdqld).ToUpper()}_{count}", item.Pdqld));
                    count++;
                }
                options.Parameters = plist;
                options.Exec();
                return options.ReturnedValue;
            }
        }

        public int DeleteByAffaire(string code, int version, string type) {
            return this.connection.EnsureOpened().Execute(delete_ByAffaire, new { code, version, type });
        }
    }
}
