using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository
{

    public partial class KpDesiRepository {
        public void DeleteForInventaire(long inventaireId) {
            const string deleteSql = @"DELETE FROM KPDESI WHERE KADCHR IN (select id from (SELECT KBEKADID id, KBEID inven FROM KPINVEN union all SELECT KBFKADID id, KBFKBEID inven FROM KPINVED) sub where sub.inven=:invenId)";
            this.connection.EnsureOpened().Execute(deleteSql, new { invenId = inventaireId });
        }
    }
}
