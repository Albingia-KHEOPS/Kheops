using System;
using System.Collections.Generic;
using System.Reflection;
using OP.WSAS400.DTO.ExcelDto;

namespace OPServiceContract.DynamicDreivedType
{
  public class DerivedType
  {
    public static IEnumerable<Type> GetDynamicType(ICustomAttributeProvider provider)
    {

      //OP.WSAS400.DTO.ExcelDto.RS.Entete
      //OP.WSAS400.DTO.ExcelDto.RS.Garanties
      //OP.WSAS400.DTO.ExcelDto.RS.Objets

      //Type type = typeof(BaseExcelObjets);
      //IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().ToList()//.Where(el => el.FullName == "OP.WSAS400.DTO")
      //    //.SelectMany(a => a.GetTypes()
      //    .SelectMany(a => a.GetTypes()
      //    .Where(t => type.IsAssignableFrom(t)));
      
      var types = new List<Type>
                          {
                              typeof (OP.WSAS400.DTO.ExcelDto.RS.Entete),
                              typeof (OP.WSAS400.DTO.ExcelDto.RS.Garanties),
                              typeof (OP.WSAS400.DTO.ExcelDto.RS.Objets),
                              typeof (OP.WSAS400.DTO.ExcelDto.RS.Risques),
                               typeof (OP.WSAS400.DTO.ExcelDto.RS.RSRecupGaranties), 
                               typeof (OP.WSAS400.DTO.ExcelDto.RS.RSRecupObjets),
                              typeof (InputExcel),
                              typeof (OutPutExcel),
                              typeof (AllObjExcel)
                          };

      IEnumerable<Type> j = types.ToArray();

      return j;

    }
  }
}
