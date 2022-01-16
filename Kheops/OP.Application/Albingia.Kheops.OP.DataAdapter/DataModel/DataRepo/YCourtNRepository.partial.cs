using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {

    public partial class YCourtNRepository {
        const string selectMany_nomCabinets = @"SELECT TNICT, TNINL, TNTNM, TNORD, TNTYP , TNNOM, TNTIT, TNXN5 
FROM YCOURTN 
WHERE TNXN5 = 0 AND TNTNM = 'A' AND TNICT IN :codeList";

        public IEnumerable<YCourtN> SelectNomsCabinets(List<int> codeList) {
            return connection.Query<YCourtN>(selectMany_nomCabinets, new { codeList }).ToList();
        }
    }
}
