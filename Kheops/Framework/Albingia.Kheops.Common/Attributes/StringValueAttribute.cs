using ALBINGIA.Framework.Common;
using System;

namespace Albingia.Kheops.Common {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class StringValueAttribute: Attribute, ICodeAttribute<string> {
        public string Value { get; private set; }

        public string Code => Value;

        public StringValueAttribute(string Value) {
            this.Value = Value;
        }
    }
}