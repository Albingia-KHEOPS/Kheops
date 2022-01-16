using ALBINGIA.Framework.Common;
using System;
using System.Runtime.Serialization;

namespace Albingia.Kheops.Common {
    [Serializable]
    public class BusinessException: Exception {
        public BusinessException() : base() { }
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception innerException) : base(message, innerException) { }
        public BusinessException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
        [HandledByMvcError]
        public BusinessErrorContext BusinessContext { get; private set; } = new BusinessErrorContext();
    }
}
