using System;

namespace Hexavia.Models.EnumDir
{
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class BusinessCodeAttribute : Attribute, ICodeAttribute<string>
    {
        public BusinessCodeAttribute(string code = "")
        {
            Code = code;
        }

        public string Code { get; set; }
    }
}
