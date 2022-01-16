using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Common
{
    [DataContract]
    public class ColumnInfoDto
    {
        [DataMember]
        [Column(Name ="NAME_COLUMN")]
        public string Name { get; set; }

        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }

        [DataMember]
        [Column(Name = "LENGTH")]
        public int Length { get; set; }

        [DataMember]
        [Column(Name = "NUMERIC_SCALE")]
        public int Scale { get; set; }

        [DataMember]
        [Column(Name = "IS_NULLABLE")]
        public string IsNullable { get; set; }

        [DataMember]
        [Column(Name = "COLUMN_TEXT")]
        public string Description{ get; set; }
    }
}