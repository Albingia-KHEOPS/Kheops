(function extensions( /** @type {JQuery} */ $) {

    if (!$ || !$.fn) {
        throw "JQuery is Required";
    }
    checkVersion();

    function checkVersion() {
        const minVer = [1, 8, 20];
        var version = $.fn.jquery.split().map(function (x) { return parseInt(x); });
        if (version.some(function (x, i) { return x < minVer[i]; })) {
            throw "JQuery >= " + minVer.join('.') + " is Required";
        }
    }

    $.fn.exists = function exists() {
        return this.length > 0;
    };

    $.fn.makeReadonly = function makeReadonly(addClass) {
        if (addClass) {
            this.addClass("readonly");
        }
        return this.prop({ readonly: true });
    };
    $.fn.disable = function disable(addClass) {
        if (addClass) {
            this.addClass("readonly");
        }
        return this.prop({ disabled: true });
    };
    $.fn.enable = function enable() {
        return this.prop({ disabled: false, readonly: false }).removeClass("readonly");
    };
    $.fn.check = function check(silent) {
        this.prop({ checked: true });
        if (!silent) {
            this.change();
        }
        return this;
    };
    $.fn.uncheck = function uncheck(silent) {
        this.prop({ checked: false });
        if (!silent) {
            this.change();
        }
        return this;
    };
    $.fn.makeSelected = function makeSelected() {
        return this.prop({ selected: true });
    };
    $.fn.unselect = function unselect() {
        return this.prop({ selected: false });
    };
    $.fn.setInvalid = function setInvalid(className) {
        this.addClass(className || "requiredField");
    };
    $.fn.selectVal = function selectVal(value) {
        this.each(function (x, e) {
            if (e.tagName.toLowerCase() === "select" || (e.tagName.toLowerCase() === "input" && e.type === "radio")) {
                $(e).val(value);
                $(e).change();
            }
        });
    };
    $.fn.isVisible = function isVisible() {
        //TODO: consider z-index value
        if (this.is(":visible") || this.is(":not(:hidden)")) {
            let o = parseInt(this.css("opacity"));
            return isNaN(o) || o > 0;
        }
        return false;
    };
    $.fn.isChecked = function isChecked() {
        return this.is(":checked");
    };
    $.fn.hasVal = function hasVal(considerSpaces) {
        return typeof this.val === "function" && this.val() != null
            && (Array.isArray(this.val()) || (considerSpaces ? this.val() !== "" : this.val().trim() !== ""));
    };
    $.fn.isEmpty = function isEmpty(considerSpaces) {
        return typeof this.val === "function" && (
            this.val() === null
            || Array.isArray(this.val()) && this.val().length === 0
            || (considerSpaces ? this.val() === "" : this.val().trim() === ""));
    };
    $.fn.hasTrueVal = function hasTrueVal() {
        if (typeof this.val === "function" && this.val() != null) {
            let v = this.val();
            if (!Array.isArray(v)) { v = [v]; }
            return !v.some(function (x) {
                let value = typeof x === "string" ? x.trim().toLowerCase() : x;
                return value !== "1" && value !== 1 && value !== (true).toString();
            });
        }
        return false;
    };
    $.fn.clear = function clear() {
        if (typeof this.val === "function") { this.val(""); }
        return this;
    };
    $.fn.clearHtml = function clear() {
        if (typeof this.html === "function") { this.html(""); }
        return this;
    };
    $.fn.offOn = function offOn(event, data, handler) {
        /** @type{JQuery} */
        var t = this;
        $(t).off(event);
        return $(t.context).off(event, t.selector).on(event, t.selector, data, handler);
    };
    $.fn.kclick =
        function kclick(handler) {
            return $(this.context).off('click', this.selector).on('click', this.selector, handler);
        };

    $.fn.triggerClick =
        function triggerClick() {
            this.each(function () {
                if (this.attr('disabled') != null) {
                    this.trigger('click');
                }
            });
        };

    //------------------Appel l'ouverture de la dialogBox pour une erreur----------------
    $.fn.jqDialogErreur = function jqDialogErreur(divErreur, exception) {
        common.error.show(exception);
    };
    //------------------Appel de la fonction jqDialogErreur en lui passant le nom du div----------------
    $.fn.jqDialogErreurOpen = function jqDialogErreurOpen(exception) {
        var divErreur = '#GestionErreur';
        $.fn.jqDialogErreur(divErreur, exception);
    };
    //------------------Serialise les objets d'une form----------------
    $.fn.serializeObject = function serializeObject(ignoreDisabled, onCreate) {
        let obj = {};
        let o = {};
        this.filter("[type='checkbox']").each(function (e) {
            obj[this.name] = this.checked;
        });
        let a = ignoreDisabled ? this.toArray() : this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (obj !== undefined) {
                    if (obj[this.name] !== undefined) {
                        o[this.name] = obj[this.name];
                    }
                }
                else {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                }
            }
            else {
                if (!$.isFunction(onCreate) || !onCreate(this.name, o)) {
                    o[this.name] = obj[this.name] ? obj[this.name] : this.value || '';
                }
            }
        });
        return o;
    };

    $.fn.toJsObject = function toJsObject () {
        var obj = {};
        this.filter("[type='checkbox']").each(function (e) {
            obj[this.name] = this.checked;
        });
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            const val = this.value || '';
            const name = this.name;
            if (name in o) {
                if (name in obj) {
                    o[name] = obj[name];
                }
                else {
                    if (o[name] instanceof Array) {
                        o[name].push(val);
                    }
                    else {
                        o[name] = [o[name], val];
                    }
                }
            }
            else {
                o[name] = val;
            }
        });
        return o;
    };

    $.fn.assignAccessKey = function assignAccessKey (keyChar, attr) {
        if (typeof attr !== "string" || typeof keyChar !== "string") {
            return this;
        }
        attr = attr.trim();
        if (!attr) {
            attr = "data-accesskey";
        }
        return this.attr(attr, keyChar[0] || "");
    };

})(jQuery);