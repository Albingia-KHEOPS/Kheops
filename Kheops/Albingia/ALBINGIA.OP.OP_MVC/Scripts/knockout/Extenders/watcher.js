// gonna check whether the value has changed by storing its initial value in a hidden property
// it is possible to set the initial value so that the state reset to unchanged
ko.extenders.watcher = function (target, option) {
    var init = function (value) {
        if (target.isWatched === undefined) {
            target._initialValue = ko.observable(arguments.length > 0 ? value : target());
            target._initialValue.extend({ notify: "always" });
            // changed name from hasChanged to hasValue changed because hasChanged is defined on observables in KO 3.5
            target.hasValueChanged = ko.pureComputed(function () {
                if (target.isWatched === false) return false;
                var sameValues = target._initialValue() == null && target() == ""
                    || target._initialValue() == "" && target() == null
                    || target._initialValue() == target()
                    || !!option.isNaNEmpty && (
                        (target._initialValue() == null || target._initialValue() == "") && isNaN(target())
                        || isNaN(target._initialValue()) && (target() == null || target() == "")
                        || isNaN(target._initialValue()) && isNaN(target())
                    );

                return !sameValues;
            });
            target._resetInitialValue = function () {
                target._initialValue(target());
            };
            target.isWatched = true;
        }
    };

    init();
    target.onChanging = target.subscribe(function (oldValue) {
        if (option.logChanges) {
            console.log(option.name + " (" + oldValue + ") is changing");
        }
        init(oldValue);
    }, null, "beforeChange");
    target.onChanged = target.subscribe(function (newValue) {
        if ($.isFunction(option.onChanged)) {
            option.onChanged(newValue);
        }
        if (option.logChanges) {
            console.log(option.name + ": " + newValue);
        }
    });
    
    return target;
};

ko.extenders.watcher.forObj = function (target, fieldsToIgnore) {
    if (typeof target === "object" && target !== null) {
        for (var x in target) {
            if (ko.isObservable(target[x]) && (!Array.isArray(fieldsToIgnore) || fieldsToIgnore.indexOf(x) < 0)) {
                target[x].extend({ watcher: { logChanges: false, name: x } });
            }
        }
    }
};

ko.extenders.watcher.resetObj = function (target) {
    if (typeof target === "object" && target !== null) {
        for (var x in target) {
            if (ko.isObservable(target[x]) && target[x]._resetInitialValue) {
                target[x]._resetInitialValue();
            }
            let v = ko.utils.unwrapObservable(target[x]);
            if (Array.isArray(v)) {
                v.forEach(function (e) {
                    ko.extenders.watcher.resetObj(e);
                });
            }
        }
        let t = ko.utils.unwrapObservable(target[x]);
        if (Array.isArray(t)) {
            t.forEach(function (e) {
                ko.extenders.watcher.resetObj(e);
            });
        }
    }
};
ko.extenders.watcher.reInitWritableObservable = function (target) {
    if (typeof target === "object" && target !== null) {
        for (var x in target) {
            if (ko.isWritableObservable(target[x]) && target[x]._initialValue) {
                target[x](target[x]._initialValue());
            }
        }
    }
};
ko.extenders.watcher.reInitWritableObservable = function (target) {
    if (typeof target === "object" && target !== null) {
        for (var x in target) {
            if (ko.isWritableObservable(target[x]) && target[x]._initialValue) {
                target[x](target[x]._initialValue());
            }
        }
    }
};
