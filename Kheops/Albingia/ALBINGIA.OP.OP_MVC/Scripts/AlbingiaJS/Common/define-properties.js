/*
 * Defines several properties and polyfill on generic types such as Window, Object, String, Array, etc.
 */

// Monkey patch DragEvent to look alike ClipboardEvent (for AutoNumeric)
if (!DragEvent.prototype.hasOwnProperty("clipboardData")) {
    Object.defineProperty(DragEvent.prototype, "clipboardData", {
        get: function () {
            return {
                getData: function (t) {
                    if (t != 'text/plain') { throw "not supported"; }
                    return window.clipboardData.getData('Text');
                }
            };
        }
    });
}

if (!Object.prototype.hasOwnProperty("extend")) {
    Object.defineProperty(Object.prototype, "extend", {
        enumerable: false,
        value: function (obj) {
            for (var i in obj) {
                if (obj.hasOwnProperty(i)) {
                    this[i] = obj[i];
                }
            }
        }
    });
}
if (!Object.prototype.hasOwnProperty("isNotEmptyArray")) {

    Object.defineProperty(Object.prototype, "isNotEmptyArray", {
        enumerable: false,
        value: function () {
            return Array.isArray(this) && this.length > 0;
        }
    });
}

if (!Array.hasOwnProperty('from')) {
    Object.defineProperty(
        Array,
        "from",
        {
            enumerable: true,
            configurable: false,
            value: function (t) { return Array.prototype.map.call(t, function (x) { return x; }); }
        }
    );
}
if (!Array.prototype.hasOwnProperty("first")) {
    Object.defineProperty(Array.prototype, "first", {
        enumerable: false,
        configurable: false,
        value: function (predicate) {
            if (typeof predicate === "function") {
                return this.filter(predicate)[0];
            }
            else {
                return this[0];
            }
        }
    });
}

if (!Array.prototype.hasOwnProperty("last")) {
    Object.defineProperty(Array.prototype, "last", {
        enumerable: false,
        configurable: false,
        value: function (predicate) {
            if (typeof predicate === "function") {
                var list = this.filter(predicate);
                return list[list.length - 1];
            }
            else {
                return this[this.length - 1];
            }
        }
    });
}
if (!String.prototype.hasOwnProperty("toFrDate")) {
    Object.defineProperty(String.prototype, "toFrDate", {
        enumerable: false,
        configurable: false,
        value: function () {
            if (FrDate.check(this)) {
                return new FrDate(day, month, year);
            }

            return null;
        }
    });
}
if (!String.prototype.hasOwnProperty("reverseString")) {
    Object.defineProperty(String.prototype, "reverseString", {
        enumerable: false,
        configurable: false,
        value: function () {
            if (this !== undefined && this !== null)
                return this.split("").reverse().join("");

            return null;
        }
    });
}

if (!Window.prototype.hasOwnProperty("isReadonly")) {
    Object.defineProperty(Window.prototype, "isReadonly", {
        enumerable: true,
        configurable: false,
        get: function () {
            let val = common.$val("OffreReadOnly").toLowerCase();
            let valReadScreen = common.$val("IsModeConsultationEcran").toLowerCase();
            //let forceVal = common.$val("IsForceReadOnly").toLowerCase();
            //return val === "true" || forceVal === "true";
            //return val === "true";
            return valReadScreen === "true" || val === "true";
        }
    });
}

if (!Window.prototype.hasOwnProperty("isModifHorsAvenant")) {
    Object.defineProperty(Window.prototype, "isModifHorsAvenant", {
        enumerable: true,
        configurable: false,
        get: function () {
            return common.$val("IsModifHorsAvn").toLowerCase() === "true";
        }
    });
}

if (!Window.prototype.hasOwnProperty("isHistory")) {
    Object.defineProperty(Window.prototype, "isHistory", {
        enumerable: true,
        configurable: false,
        get: function () {
            let val = common.$val("ModeNavig").toLowerCase();
            return val === "h";
        }
    });
}

// https://github.com/uxitten/polyfill/blob/master/string.polyfill.js
// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/padStart
if (!String.prototype.padStart) {
    String.prototype.padStart = function padStart(targetLength, padString) {
        targetLength = targetLength >> 0; //truncate if number, or convert non-number to 0;
        padString = String(typeof padString !== 'undefined' ? padString : ' ');
        if (this.length >= targetLength) {
            return String(this);
        } else {
            targetLength = targetLength - this.length;
            if (targetLength > padString.length) {
                padString += padString.repeat(targetLength / padString.length); //append to original to ensure we are longer than needed
            }
            return padString.slice(0, targetLength) + String(this);
        }
    };
}
