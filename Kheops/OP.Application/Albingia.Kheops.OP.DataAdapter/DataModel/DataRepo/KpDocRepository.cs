using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using Dapper;
using Dapper.FluentMap.Mapping;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Interface;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.Repository {

    public  partial class  KpDocRepository : BaseTableRepository, IKpDocRepository
    {
    }
}
