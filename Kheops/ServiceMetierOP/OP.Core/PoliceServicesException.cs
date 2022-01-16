using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OP.Core
{
    public class PoliceServicesException : Exception
    {
        public PoliceServicesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PoliceServicesException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public PoliceServicesException()
        {
        }

        public PoliceServicesException(string message)
            : base(message)
        {
        }
    }
}