ko.bindingHandlers.bindElement = {
    init: function (element, valueAccessor, bindingsAccessor) {
        if (ko.utils.unwrapObservable(valueAccessor())) {
            ko.bindingHandlers.bindElement.setElement(valueAccessor(), element, bindingsAccessor);
        }
    },

    update: function (element, valueAccessor, bindingsAccessor) {
        if (ko.utils.unwrapObservable(valueAccessor())) {
            ko.bindingHandlers.bindElement.setElement(valueAccessor(), element, bindingsAccessor);
        }
        else {
            ko.bindingHandlers.bindElement.setElement(valueAccessor(), null, bindingsAccessor);
        }
    },

    setElement: function (value, element, bindingsAccessor) {
        if (ko.isObservable(value)) {
            value.element = element;
        }
        else if (typeof value === "string" && bindingsAccessor()[value]) {
            bindingsAccessor()[value].element = element;
        }
        else if (bindingsAccessor().value) {
            bindingsAccessor().value.element = element;
        }
    }
};
