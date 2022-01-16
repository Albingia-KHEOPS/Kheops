using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.CustomModelBinders
{
    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;

            if (valueResult.RawValue is null || valueResult.RawValue is string s && s.IsEmptyOrNull()) {
                return 0M;
            }

            try
            {
                actualValue = Convert.ToDecimal(valueResult.RawValue, CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                try
                {
                    actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
                }
                catch (Exception e) when (e is OverflowException || e is FormatException)
                {
                    modelState.Errors.Add(e);
                }
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}