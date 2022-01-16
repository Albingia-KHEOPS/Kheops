using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.CustomModelBinders
{
    public class DoubleModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;

            try
            {
                actualValue = Convert.ToDouble(valueResult.RawValue, CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                try
                {
                    actualValue = Convert.ToDouble(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
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