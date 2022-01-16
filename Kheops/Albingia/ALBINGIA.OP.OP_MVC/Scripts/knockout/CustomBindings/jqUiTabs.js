ko.bindingHandlers.jqUiTabs = {
    init: function (element, valueAccessor, bindingsAccessor) {
        if (!$(element).tabs) {
            return;
        }
        var configTabs = ko.utils.unwrapObservable(valueAccessor());
        if (typeof configTabs === "object" && configTabs != null) {
            $(element).addClass(configTabs.className || "tabs");
            $(element).tabs();
        }
    },

    update: function (element, valueAccessor, bindingsAccessor) {
        
    }
};