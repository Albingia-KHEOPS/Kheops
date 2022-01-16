// Generated on : 2018/03/20 09:58
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

	public  partial class  KpoptapDtoRepository {

            private IDbConnection connection;

            #region Query texts        
            #endregion

            public KpoptapDtoRepository (IDbConnection connection) {
                this.connection = connection;
            }
    }
}
