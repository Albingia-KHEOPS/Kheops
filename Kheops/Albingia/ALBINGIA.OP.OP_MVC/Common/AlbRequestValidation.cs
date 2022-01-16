using System.Web;
using System.Web.Util;

namespace ALBINGIA.OP.OP_MVC.Common
{
  public class AlbRequestValidation : RequestValidator
  {
    protected override bool IsValidRequestString(HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
    {

      if (value.Contains(":") || value.Contains("<") || value.Contains(">") || value.Contains("%") ||
          value.Contains("'"))
      {
        validationFailureIndex = 0;
        return true;
      }
      return base.IsValidRequestString(context, value, requestValidationSource, collectionKey, out validationFailureIndex);
    }
  }
}