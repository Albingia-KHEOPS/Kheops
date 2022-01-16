using System;
using System.Collections.Generic;
using System.Data.EasycomClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Data
{
    internal static class EacParameterExtension
    {
        private const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
        static FieldInfo size;
        static FieldInfo precision;
        static FieldInfo scale;

        public static void SetSize(this EacParameter param, int value)
        {
            if (size == null)
            {
                size = (typeof(EacParameter)).GetField("m_Size", flags);
            }
            size.SetValue(param, value);
        }
        public static void SetPrecision(this EacParameter param, Byte value)
        {
            if (precision == null)
            {
                precision = (typeof(EacParameter)).GetField("m_Precision", flags);
            }
            precision.SetValue(param, value);
        }
        public static void SetScale(this EacParameter param, Byte value)
        {
            if (scale == null)
            {
                scale = (typeof(EacParameter)).GetField("m_Scale", flags);
            }
            scale.SetValue(param, value);
        }
    }
}
