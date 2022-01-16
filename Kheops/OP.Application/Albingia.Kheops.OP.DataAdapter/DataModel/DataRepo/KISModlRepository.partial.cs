using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using ALBINGIA.Framework.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {
    public partial class KISModlRepository {
        const string valuePattern = @"('[^\n\r]*'|\d+)";
        static readonly Regex kgdsqlYparRegex = new Regex(@"^\s*select\s+tcod(\s+\w+)?\s*,\s*tplib(\s+\w+)?\s+from\s+yyyypar\s+where\s+tcon\s*\=\s*'[^\n\r]*'\s+and\s+tfam\s*\=\s*'[^\n\r]*'\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        static readonly Regex kgdsqlValuesRegex = new Regex($@"^\s*values\s*(\({valuePattern}\s*,\s*{valuePattern}\))(\s*,\s*(\({valuePattern}\s*,\s*{valuePattern}\)))*\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        static readonly Regex kgdsqlSysdummyRegex = new Regex(@"^\s*select\s+('[^\n\r]*'|\d+)(\s+\w+)?\s*,\s*('[^\n\r]*'|\d+)(\s+\w+)?\s+from\s+(""SYSIBM""|SYSIBM)\s*\.\s*(SYSDUMMY1|SYSDUM0001)\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        public IDictionary<string, string> GetListeValeurs(KISModl ismodl) {
            if (ismodl is null) {
                throw new ArgumentNullException(nameof(ismodl));
            }
            if (ismodl.Kgdsql.IsEmptyOrNull()
                || !kgdsqlValuesRegex.IsMatch(ismodl.Kgdsql) && !kgdsqlYparRegex.IsMatch(ismodl.Kgdsql) && !kgdsqlSysdummyRegex.IsMatch(ismodl.Kgdsql)) {
                return new Dictionary<string, string>();
            }

            var list = this.connection.EnsureOpened().Query<(string Code, string Libelle)>(ismodl.Kgdsql);
            return list.GroupBy(x => x.Code).ToDictionary(g => g.Key, g => g.First().Libelle);
        }
    }
}
