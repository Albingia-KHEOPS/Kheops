ko.bindingHandlers.clickLoading = {
    init: function (element, valueAccessor, bindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var bindings = ko.utils.unwrapObservable(bindingsAccessor());
        if (value && bindings.attr && bindings.attr.href) {
            $(element).click(function () {
                common.page.isLoading = true;
            });
        }
    },
    update: function (element, valueAccessor, bindingsAccessor) { }
};
