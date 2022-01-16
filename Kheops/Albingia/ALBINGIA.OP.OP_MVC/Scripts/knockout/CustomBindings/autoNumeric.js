ko.bindingHandlers.autoNumeric = {
    init: function (el, valueAccessor, bindingsAccessor, viewModel) {
        var $el = $(el),
            bindings = bindingsAccessor(),
            settings = common.autonumeric.defaultSettings($el.attr("albmask") || "numeric"),
            boundValue = valueAccessor();

        settings.emptyInputBehavior = "zero";
        let value;
        if (boundValue.value) {
            value = boundValue.value;
            if (boundValue.min !== undefined) settings.minimumValue = boundValue.min;
            if (boundValue.max !== undefined) settings.maximumValue = boundValue.max;
            if (boundValue.leadingZero !== undefined) settings.leadingZero = "allow";
            if (boundValue.allowNull) settings.emptyInputBehavior = "always";
            if (boundValue.nbDecimals !== undefined) settings.decimalPlacesOverride = boundValue.nbDecimals;
            if (boundValue.stuckAllDigits) settings.digitGroupSeparator = "";
        }
        else {
            value = boundValue;
        }

        $el.autoNumeric(settings);
        let unwrapValue = ko.utils.unwrapObservable(value());
        let num = typeof unwrapValue == "string" ? parseFloat(unwrapValue.replace(",", "."), 10) : unwrapValue;
        $el.autoNumeric("set", num);
        $el.on("input change focusout", function () {
            let num = parseFloat($el.autoNumeric("get"), 10);
            if (ko.isComputed(value) && value.write) {
                value(num);
            }
            else if (ko.isWriteableObservable(value)) {
                value(num);
            }
        });
    },
    update: function (el, valueAccessor, bindingsAccessor, viewModel) {
        let $el = $(el);
        let boundValue = valueAccessor();
        if (boundValue.value) {
            boundValue = boundValue.value;
        }

        let valueHasChanged = true;
        let unwrapValue = ko.utils.unwrapObservable(boundValue());
        let newValue = typeof unwrapValue == "string" ? parseFloat(unwrapValue.replace(",", "."), 10) : unwrapValue;
        if ($el.is("input")) {
            elementValue = parseFloat($el.autoNumeric("get"), 10);
            if (isNaN(newValue) && isNaN(elementValue)) {
                valueHasChanged = false;
            }
            else {
                valueHasChanged = (newValue != elementValue);
            }
            if ((newValue === 0) && (elementValue !== 0) && (elementValue !== "0")) {
                valueHasChanged = true;
            }
        }
        if (valueHasChanged) {
            $el.autoNumeric("set", newValue == null ? "" : newValue);
            setTimeout(function () {
                if ($el.is("input")) {
                    $el.trigger("input");
                }
                else {
                    $el.trigger("change");
                }
            }, 0);
        }
    }
};