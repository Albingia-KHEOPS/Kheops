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

    public partial class KpInveDRepository
    {


        #region Query texts        
        //ZALBINKHEO
        const string deleteForGarantie = @"DELETE
FROM 
    KPINVED
WHERE KBFKBEID = :idInven
";
        #endregion

        public void DeleteForGarantie(Int64 idInven)
        {
            connection.Execute(deleteForGarantie, new { idInven });
        }
    }
}
