ko.bindingHandlers.dateselector = {
    init: function (element, valueAccessor, bindingsAccessor, viewModel) {
        var selector = ko.utils.unwrapObservable(valueAccessor());
        if (typeof selector === "object") {
            $(element).addClass("datepicker");
            
            if (selector.pickerOptions) {
                var options = selector.pickerOptions;
                if (options.minDate) {
                    options.minDate = (new FrDate(options.minDate)).toDateString();
                }
                if (typeof options.showOn === "function") {
                    var showOnHandler = options.showOn;
                    options.showOn = showOnHandler() ? "focus" : "off";
                }

                $(element).datepicker(options);
            }

            if (bindingsAccessor().value) {
                $(element).datepicker("setDate", (new FrDate(bindingsAccessor().value())).toDateString());
            }
            if (selector.value) {
                $(element).datepicker("setDate", (new FrDate(selector.value())).toDateString());
                selector.hasChangedFromIHM = false;
                selector.value.subscribe(function (val) {
                    if (!selector.hasChangedFromIHM) {
                        $(element).val(val);
                    }
                });
                var triggerIHMChange = function () {
                    try {
                        selector.hasChangedFromIHM = true;
                        selector.value($(element).val());
                    }
                    catch (e) {
                        console.error(e);
                    }
                    selector.hasChangedFromIHM = false;
                };
                if (!selector.onlyTriggerChange) {
                    $(element).on("input", triggerIHMChange);
                }
                if (typeof window.FormatAutoDate === "function") {
                    $(element).on("change", function () {
                        window.FormatAutoDate($(element), "invalid-date");
                        triggerIHMChange();
                    });
                }
            }
        }
    },
    update: function (element, valueAccessor, bindingsAccessor, viewModel) {
        var currentState = $(element).datepicker("option", "showOn") === "focus";
        var selector = ko.utils.unwrapObservable(valueAccessor());
        if (typeof selector.pickerOptions.showOn === "function" && currentState !== selector.pickerOptions.showOn()) {
            if (currentState) {
                $(element).datepicker("option", "showOn", "off");
            }
            else {
                $(element).datepicker("option", "showOn", "focus");
            }
        }
    }
};
