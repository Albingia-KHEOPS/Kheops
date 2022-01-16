using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Data.Dapper
{
    public class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
    {
        public ColumnAttributeTypeMapper()
            : base(new SqlMapper.ITypeMap[]
                {
                new CustomPropertyTypeMap(
                   typeof(T),
                   (type, columnName) =>
                        type.GetProperties().FirstOrDefault(prop =>
                           prop.GetCustomAttributes(typeof(ColumnAttribute),false)
                               .Cast<ColumnAttribute>()
                               .Any(attr => string.Compare(attr.Name,columnName,false) == 0)
                           )
                   ),
                new DefaultTypeMap(typeof(T))
                })
        {
        }
    }
}
