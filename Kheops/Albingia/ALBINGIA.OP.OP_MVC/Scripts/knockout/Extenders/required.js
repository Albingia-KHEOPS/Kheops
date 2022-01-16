// gonna checked whether the value is assigned. If not, the required state is broken
ko.extenders.required = function (target, option) {
    var alwaysCheckValue = ko.unwrap(option.alwaysCheckValue);
    function checkValue(v, ignoreStateChanged) {
        if (typeof option.delayCheck === "boolean") {
            if (target.isRequiredReady === undefined) {
                target.isRequiredReady = ko.observable(!option.delayCheck);
                if (!alwaysCheckValue && !option.delayCheck) {
                    return true;
                }
            }
            if (!target.isRequiredReady()) {
                return true;
            }
        }
        let ignoreStateBypassed = false;
        if (v === undefined) v = target();
        if (typeof option === "boolean") {
            if (!option) return true;
            else return !(v == null || v === "");
        }
        else {
            if (option.ignoreIf && option.ignoreIfNot) {
                throw new Error("Cannot manage ignoreIf and ignoreIfNot all together");
            }
            else if (option.ignoreIf || option.ignoreIfNot) {
                ignoreStateBypassed = true;
                if (typeof option.ignoreIf === "boolean" && option.ignoreIf || typeof option.ignoreIf === "function" && option.ignoreIf()) {
                    return true;
                }
                else if (typeof option.ignoreIfNot === "boolean" && !option.ignoreIfNot || typeof option.ignoreIfNot === "function" && !option.ignoreIfNot()) {
                    return true;
                }
            }
        }

        if (ignoreStateChanged && ignoreStateBypassed && alwaysCheckValue) {
            return true;
        }
        return !(v == null || (option.zeroAsEmpty && v === 0) || v === "" || option.NaNAsEmpty && isNaN(v));
    }

    function refreshState(newValue, ignoreStateChanged) {
        if (!checkValue(newValue, ignoreStateChanged)) {
            target.requireState.isValid(false);
            target.requireState.message(option.errorMessage || "");
        }
        else {
            target.requireState.isValid(true);
            target.requireState.message(null);
        }
    }

    if (target.requireState === undefined) {
        let firstCheck = (alwaysCheckValue || typeof option.delayCheck === "boolean") ? checkValue() : true;
        target.requireState = {
            isValid: ko.observable(firstCheck),
            message: ko.observable(firstCheck ? null : (option.errorMessage || "")),
            refresh: function (ignoreStateChanged) {
                refreshState(target(), ignoreStateChanged);
            },
            forceValid: function () {
                if (!alwaysCheckValue) {
                    target.requireState.isValid(true);
                }
            }
        };

        if (ko.isObservable(option.ignoreIf)) {
            option.ignoreIf.subscribe(function () {
                target.requireState.refresh(true);
            });
        }
        else if (ko.isObservable(option.ignoreIfNot)) {
            option.ignoreIfNot.subscribe(function () {
                target.requireState.refresh(true);
            });
        }
    }
    if (option.validateOn && ko.isObservable(option.validateOn)) {
        option.validateOn.subscribe(function () {
            refreshState(target.value);
        });
    }

    target.onChanged = target.subscribe(function (val) {
        refreshState(val);
    });
    
    return target;
};

ko.extenders.required.isImplementedBy = function (obj) {
    if (!obj) return false;
    return obj.requireState && ko.isObservable(obj.requireState.isValid)
        && ko.isObservable(obj.requireState.message);
};

ko.extenders.required.setSome = function (target, properties) {
    if (typeof target === "object" && target !== null && $.isArray(properties)) {
        for (var x in target) {
            if (ko.isObservable(target[x]) && properties.indexOf(x) > -1) {
                target[x].extend({ required: { errorMessage: "" } });
            }
        }
    }
};

ko.extenders.required.checkValidity = function (obj) {
    if (!obj) return null;
    var states = [];
    for (var p in obj) {
        if (ko.extenders.required.isImplementedBy(obj[p])) {
            obj[p].requireState.refresh();
            if (!obj[p].requireState.isValid()) {
                states.push(obj[p].requireState);
            }
        }
    }

    if (states.length > 0) {
        if (states.some(function (s) { return !!s.message(); })) {
            return states.map(function (s) { return s.message(); }).join("\n");
        }
        else {
            return false;
        }
    } 

    return true;
};
