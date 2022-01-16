
using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Albingia.Kheops.Common {
    [Serializable]
    public class BusinessValidationException : Exception
    {
        public BusinessValidationException() : base()
        {
        }

        public BusinessValidationException(ValidationError error) : this(new[] { error }) { }
        public BusinessValidationException(IEnumerable<ValidationError> errors) : base()
        {
            this.Errors = errors;
        }
        public BusinessValidationException(IEnumerable<ValidationError> errors, string message) : base(message) {
            this.Errors = errors;
        }
        public BusinessValidationException(IEnumerable<ValidationError> errors, string message, Exception innerException) : base(message, innerException) {
            this.Errors = errors;
        }
        public BusinessValidationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
        
        [HandledByMvcError]
        public IEnumerable<ValidationError> Errors { get; private set; }

        public override string ToString() {
            string message = base.ToString();
            if (Errors?.Any() == true) {
                message = string.Join(Environment.NewLine, Errors.Select(e => e.Error)) + message;
            }
            return message;
        }
    }
}
