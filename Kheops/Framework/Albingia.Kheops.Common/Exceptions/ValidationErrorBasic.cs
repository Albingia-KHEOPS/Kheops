namespace Albingia.Kheops.Common {
    public class ValidationErrorBasic
    {
        public ValidationErrorBasic(string fieldName,string error)
        {
            this.FieldName = fieldName;
            this.Error = error;
        }

        public string FieldName { get; private set; }
        public string Error { get; private set; }
    }
}