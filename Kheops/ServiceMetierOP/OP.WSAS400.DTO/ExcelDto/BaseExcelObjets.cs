using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ExcelDto
{
    [DataContract]
    public abstract class BaseExcelObjets
    {
      [DataMember(EmitDefaultValue = true)]
      public bool DisplayLineIs { get; set; }
    }
}
