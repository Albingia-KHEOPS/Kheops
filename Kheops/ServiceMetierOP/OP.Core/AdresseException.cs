using System;
using System.Runtime.Serialization;

namespace OP.Core
{
    public class AdresseException : Exception
    {
        public AdresseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AdresseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public AdresseException()
        {
        }

        public AdresseException(string message)
            : base(message)
        {
        }
    }
}