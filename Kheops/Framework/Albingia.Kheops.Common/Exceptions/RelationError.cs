
namespace Albingia.Kheops.Common
{
    public class RelationError: ValidationError
    {
        public RelationError() { }

        public RelationError(TypeRelation relation, string targetType, string targetCode, string targetId,string relatedType, string relatedCode, string relatedId, string error, string errorType = "") : base(targetType, targetCode, targetId, error)
        {
            TargetType = targetType;
            TargetId = targetId;
            TargetCode = targetCode;
            RelatedType = relatedType;
            RelatedId = relatedId;
            RelatedCode = relatedCode;
            Relation = relation;
            Error = error;
            ErrorType = errorType;
        }
        
        public string RelatedType { get; private set; }
        public string RelatedId { get; private set; }
        public string RelatedCode { get; private set; }
        public TypeRelation Relation { get; private set; }
        public string ErrorType { get; private set; }
    }
}