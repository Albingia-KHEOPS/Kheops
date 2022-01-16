using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace ALBINGIA.Framework.Common.Data.Dapper
{

    public class EacNullableDatetimeTypeHandler : TypeHandler<DateTime?> {

        public override DateTime? Parse(object value)
        {
            switch (value) {
                case DateTime t when t == DateTime.MinValue:
                    return null;
                case DateTime t:
                    return t;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported value type");
            }
        }

        public override void SetValue(IDbDataParameter parameter, DateTime? value)
        {
            parameter.Value = value ?? (object)DBNull.Value;
        }
    }
}
