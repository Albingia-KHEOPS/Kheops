ko.bindingHandlers.readonly = {
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (!value && element.readOnly) {
            element.readOnly = false;
            $(element).removeClass("readonly");
        }
        else if (value && !element.readOnly) {
            element.readOnly = true;
            $(element).addClass("readonly");
        }
    }
};