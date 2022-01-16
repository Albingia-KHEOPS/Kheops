using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using ALBINGIA.Framework.Common.Data;
using Dapper;
using System.Collections.Generic;
using System.Data.EasycomClient;
using System.Linq;
using System.Reflection;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class YpoCoasRepository {
        const string delete_ByAffaire = @"DELETE FROM YPOCOAS WHERE ( PHIPB , PHALX , PHTYP ) = ( :code , :version , :type ) ";
        const string insertQuery = @"
INSERT INTO YPOCOAS
( PHTYP, PHIPB, PHALX, PHTAP, PHCIE, PHINL, PHPOL, PHAPP, PHCOM, PHTXF, PHAFR, PHEPA, PHEPM, PHEPJ, PHFPA, PHFPM, PHFPJ, PHIN5, PHTAC, PHTAA, PHTAM, PHTAJ )";

        public int InsertMultiple(IEnumerable<YpoCoas> ypoCoasList) {
            if (ypoCoasList?.Any() != true) {
                return 0;
            }

            var phProps = typeof(YpoCoas).GetProperties(BindingFlags.GetProperty).Where(p => p.Name.StartsWith("PH")).OrderBy(p => p.Name);

            //string valuesQuery = " VALUES " +
            //    string.Join(" , ", ypoCoasList.Select((item, x) => string.Join(" , ", phProps.Select(p => $":{p.Name.ToUpper()}_{x}"))));

            string valuesQuery = " VALUES " + string.Join(" , ", ypoCoasList.Select((item, x) => $" ( :{nameof(item.Phtyp).ToUpper()}_{x}, :{nameof(item.Phipb).ToUpper()}_{x}, :{nameof(item.Phalx).ToUpper()}_{x}, :{nameof(item.Phtap).ToUpper()}_{x}, :{nameof(item.Phcie).ToUpper()}_{x}, :{nameof(item.Phinl).ToUpper()}_{x}, :{nameof(item.Phpol).ToUpper()}_{x}, :{nameof(item.Phapp).ToUpper()}_{x}, :{nameof(item.Phcom).ToUpper()}_{x}, :{nameof(item.Phtxf).ToUpper()}_{x}, :{nameof(item.Phafr).ToUpper()}_{x}, :{nameof(item.Phepa).ToUpper()}_{x}, :{nameof(item.Phepm).ToUpper()}_{x}, :{nameof(item.Phepj).ToUpper()}_{x}, :{nameof(item.Phfpa).ToUpper()}_{x}, :{nameof(item.Phfpm).ToUpper()}_{x}, :{nameof(item.Phfpj).ToUpper()}_{x}, :{nameof(item.Phin5).ToUpper()}_{x}, :{nameof(item.Phtac).ToUpper()}_{x}, :{nameof(item.Phtaa).ToUpper()}_{x}, :{nameof(item.Phtam).ToUpper()}_{x}, :{nameof(item.Phtaj).ToUpper()}_{x} ) "));

            using (var options = new DbExecOptions(this.connection == null) {
                DbConnection = this.connection,
                SqlText = insertQuery + valuesQuery
            }) {
                var plist = new List<EacParameter>();
                int count = 0;
                foreach (var item in ypoCoasList) {
                    //foreach (var p in phProps) {
                    //    plist.Add(new EacParameter($"{p.Name.ToUpper()}_{count}", p.GetValue(item)));
                    //}
                    plist.Add(new EacParameter($"{nameof(item.Phtyp).ToUpper()}_{count}", item.Phtyp));
                    plist.Add(new EacParameter($"{nameof(item.Phipb).ToUpper()}_{count}", item.Phipb));
                    plist.Add(new EacParameter($"{nameof(item.Phalx).ToUpper()}_{count}", item.Phalx));
                    plist.Add(new EacParameter($"{nameof(item.Phtap).ToUpper()}_{count}", item.Phtap));
                    plist.Add(new EacParameter($"{nameof(item.Phcie).ToUpper()}_{count}", item.Phcie));
                    plist.Add(new EacParameter($"{nameof(item.Phinl).ToUpper()}_{count}", item.Phinl));
                    plist.Add(new EacParameter($"{nameof(item.Phpol).ToUpper()}_{count}", item.Phpol));
                    plist.Add(new EacParameter($"{nameof(item.Phapp).ToUpper()}_{count}", item.Phapp));
                    plist.Add(new EacParameter($"{nameof(item.Phcom).ToUpper()}_{count}", item.Phcom));
                    plist.Add(new EacParameter($"{nameof(item.Phtxf).ToUpper()}_{count}", item.Phtxf));
                    plist.Add(new EacParameter($"{nameof(item.Phafr).ToUpper()}_{count}", item.Phafr));
                    plist.Add(new EacParameter($"{nameof(item.Phepa).ToUpper()}_{count}", item.Phepa));
                    plist.Add(new EacParameter($"{nameof(item.Phepm).ToUpper()}_{count}", item.Phepm));
                    plist.Add(new EacParameter($"{nameof(item.Phepj).ToUpper()}_{count}", item.Phepj));
                    plist.Add(new EacParameter($"{nameof(item.Phfpa).ToUpper()}_{count}", item.Phfpa));
                    plist.Add(new EacParameter($"{nameof(item.Phfpm).ToUpper()}_{count}", item.Phfpm));
                    plist.Add(new EacParameter($"{nameof(item.Phfpj).ToUpper()}_{count}", item.Phfpj));
                    plist.Add(new EacParameter($"{nameof(item.Phin5).ToUpper()}_{count}", item.Phin5));
                    plist.Add(new EacParameter($"{nameof(item.Phtac).ToUpper()}_{count}", item.Phtac));
                    plist.Add(new EacParameter($"{nameof(item.Phtaa).ToUpper()}_{count}", item.Phtaa));
                    plist.Add(new EacParameter($"{nameof(item.Phtam).ToUpper()}_{count}", item.Phtam));
                    plist.Add(new EacParameter($"{nameof(item.Phtaj).ToUpper()}_{count}", item.Phtaj));
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
