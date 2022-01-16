using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO {
    [Serializable]
    public class HistorizationException: Exception {
        public HistorizationException() : base() {
        }
        public HistorizationException(IEnumerable<string> errors) : base() {
            Errors = errors;
        }
        public HistorizationException(IEnumerable<string> errors, string message) : base(message) {
            Errors = errors;
        }
        public HistorizationException(IEnumerable<string> errors, string message, Exception innerException) : base(message, innerException) {
            Errors = errors;
        }
        public HistorizationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
        
        public IEnumerable<string> Errors { get; private set; }
    }
}
