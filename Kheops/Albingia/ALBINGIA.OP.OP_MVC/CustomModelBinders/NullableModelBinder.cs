using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.CustomModelBinders {
    public class NullableModelBinder : DefaultModelBinder {

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;

            try {
                if (valueResult?.RawValue != null) {
                    var (ok, value) = AlbModelBinder.ProcessNullableValue(valueResult, bindingContext.ModelType);
                    if (ok) {
                        actualValue = value;
                    }
                }
            }
            catch (Exception) {
                actualValue = null;
            }

            if (actualValue == null && bindingContext.ModelType.IsValueType) {
                actualValue = Activator.CreateInstance(bindingContext.ModelType);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}