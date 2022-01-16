using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Albingia.Hexavia.Core
{
    public class AdresseException : Exception
    {
        public AdresseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AdresseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AdresseException()
        {
        }

        public AdresseException(string message) : base(message)
        {
        }
    }
}
