using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.CustomModelBinders {
    public class InventoryItemModelBinder: DefaultModelBinder {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType) {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".$type");
            if (valueResult != null) {
                Type instantiationType = Type.GetType(valueResult.RawValue as string);
                var obj = Activator.CreateInstance(instantiationType);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => obj, instantiationType);
                //bindingContext.ModelMetadata.Model = obj;
                return obj;
            }
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
        
        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder) {
            bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;
            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }
    }
    
}