using System;
using System.Runtime.Serialization;

namespace Albingia.Kheops.Common {
    [Serializable]
    [DataContract]
    public class ValidationError {
        public ValidationError() { }
        public ValidationError(string message) : this(string.Empty, message) { }
        public ValidationError(string type, string message) : this(type, string.Empty, string.Empty, string.Empty, message) { }

        public ValidationError(string targetType, string targetCode, string targetId, string error) : this() {
            this.TargetType = targetType;
            this.TargetId = targetId;
            this.TargetCode = targetCode;
            this.Error = error;
        }
        public ValidationError(string type, string code, string id, string fieldName, string error) : this(type, code, id, error) {
            this.FieldName = fieldName;
        }

        public static implicit operator ValidationError(string s) => new ValidationError(string.Empty, s ?? string.Empty);
        public static implicit operator ValidationError((string fieldName, string error) x) => new ValidationError(string.Empty, string.Empty, string.Empty, x.fieldName ?? string.Empty, x.error ?? string.Empty);

        [DataMember]
        public string FieldName { get; protected set; }
        [DataMember]
        public string TargetId { get; protected set; }
        [DataMember]
        public string TargetCode { get; protected set; }
        [DataMember]
        public string TargetType { get; protected set; }
        [DataMember]
        public string Er​​​​​​​​​​​​​ror { get; protected set; }


        public ValidationError Clone(string newMessage = null) {
            var e = MemberwiseClone() as ValidationError;
            if (newMessage != null) {
                e.Error = newMessage;
            }
            return e;
        }
    }
}