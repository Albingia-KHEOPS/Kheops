using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ALBINGIA.Framework.Common {
    [Serializable]
    public class BlacklistException : Exception {
        public BlacklistException() : base() { }

        public BlacklistException(BlacklistAlert alert) : this(new[] { alert }) { }
        public BlacklistException(IEnumerable<BlacklistAlert> alerts) : base() {
            Alerts = alerts;
        }
        
        public BlacklistException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        [HandledByMvcError]
        public IEnumerable<BlacklistAlert> Alerts { get; }

        //[HandledByMvcError]
        public override string Message => Alerts?.Any() != true ? base.Message : string.Join(Environment.NewLine, Alerts.Select(a => a.AlertMessage));
    }
}
