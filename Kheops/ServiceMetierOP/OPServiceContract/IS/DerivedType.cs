using System;
using System.Collections.Generic;
using System.Reflection;

namespace OPServiceContract.IS
{
    public  class DerivedType
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
            var types=new List<Type>
                          {
                              typeof (OP.WSAS400.DTO.ExcelDto.RS.Entete),
                              typeof (OP.WSAS400.DTO.ExcelDto.RS.Garanties),
                              typeof (OP.WSAS400.DTO.ExcelDto.RS.Objets)
                          };

            IEnumerable<Type> j = types.ToArray();

            return j;
           
        }
    }
}
