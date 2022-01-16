//catch-all error handler
$(window).on('error', function (e) {
    common.error.showMessage(e);
});

$(document).ajaxSend(function (event, jqxhr, settings) {
    let value = $("tabGuid").val() || $("#homeTabGuid", window.top.document).val() || "";
    value = value.replace(/tabGuid/ig, "");
    jqxhr.setRequestHeader("TABGUID", value);
});

window.addEventListener(
    "keydown",
    function (ev) {
        if (ev.getModifierState && ev.getModifierState("Alt")) {
            const $k = $("[accesskey='" + ev.key.toLowerCase() + "']:visible,[accesskey='" + ev.key.toUpperCase() + "']:visible");
            if ($k.length) {
                $k.focus();
                ev.preventDefault();
                ev.stopPropagation();
                ev.returnValue = false;
            }
            return false;
        }
    });

var common = {
    tools: {
        startsWithVowel: function (char) {
            return typeof char === "string" && ["A", "À", "E", "É", "Ê", "È", "I", "O", "U", "Ù", "Y"].indexOf(char.toUpperCase()[0]) > -1;
        },
        compareOneLevelObjects: function (o1, o2) {
            if (o1 === o2 || (o1 == null && o2 == null)) { return 0; }
            if (o1 == null || o2 == null) { return null; }
            for (var p in o1) {
                if (o1.hasOwnProperty(p)) {
                    if (o1[p] !== o2[p]) {
                        return null;
                    }
                }
            }
            for (var p in o2) {
                if (o2.hasOwnProperty(p)) {
                    if (o1[p] !== o2[p]) {
                        return null;
                    }
                }
            }
            return 0;
        },
        createUUID: function() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }
    },
    dom: {
        w: window,
        toggleDescription: function (element) {
            var textDom = $("#" + $("#txtArea").data("context"));
            if (!$("#txtArea").hasClass("textarea-position")) {
                $("#txtArea").addClass("textarea-position");
            }
            $("#txtArea").toggle();
            textDom.focus();
        },
        get: function (naming, openSearch) {
            var e = document.getElementById(naming);
            if (!e && openSearch) {
                if (!e) e = document.getElementsByName(naming)[0];
            }

            return e;
        },
        exists: function (naming, openSearch) {
            return !!common.dom.get(naming, openSearch);
        },
        redirect: function (url, componentUrl) {
            if (!url) {
                return;
            }
            if (!/(\/|\?\id=)$/g.test(url)) {
                url += "/";
            }
            window.location.replace(url + encodeURIComponent(componentUrl || ""));
        },
        onEvent: function (eventNames, selector, action, appendHandler) {
            let events = eventNames;
            if (Array.isArray(eventNames)) {
                events = eventNames.join(" ");
            }
            else if (typeof eventNames !== "string") {
                events = (eventNames || {}).toString();
            }
            if (!appendHandler) {
                $(document).off(events, selector);
            }
            $(document).on(events, selector, action);
            return $(selector);
        },
        onClick: function (selector, action, appendHandler) {
            return common.dom.onEvent("click", selector, action, appendHandler);
        },
        onChange: function (selector, action, appendHandler) {
            return common.dom.onEvent("change", selector, action, appendHandler);
        },
        maxVisibleZIndex: function () {
            return Array.from(document.querySelectorAll('body *')
            ).map(function (a) {
                return {
                    z: window.getComputedStyle(a).zIndex,
                    v: a.offsetParent != null
                };
            }).filter(function (a) {
                return !isNaN(parseFloat(a.z)) && a.v;
            }).reduce(function (a, x) {
                return Math.max(a, x.z);
            }, 0);
        },
        pushForward: function (item) {
            let $element = common.$infer(item);
            let zixMax = common.dom.maxVisibleZIndex();
            $element.prev().css("z-index", zixMax + 1);
            $element.css("z-index", zixMax + 2);
        }
    },
    autobindFn: function (obj) {
        if (!obj || typeof obj !== "object") return;
        for (var x in obj) {
            if (typeof obj[x] === "function") {
                obj[x] = obj[x].bind(obj);
            }
        }
    },
    isUndifinedOrEmpty: function (str) {
        if (str == undefined || str == null || str == "") {
            return true;
        }
        return false;
    },
    addOrReplaceParam: function (params, key, value) {
        if (params === undefined || params === null || params === "")
            return key + "|" + value;

        var endStringPos = params.reverseString().indexOf("addParam".reverseString()); //tabGuidaddParam
        var endString = "";
        if (endStringPos === 0) {
            endString = "addParam";
        }
        params = params.reverseString().substr(endString.length).reverseString();


        var startStringPos = params.indexOf("addParam");
        var startString = "";
        if (startStringPos !== -1) {
            startString = params.substr(0, startStringPos + "addParam".length);
        }
        params = params.replace(startString, "");

        var wasStringNormalized = true;
        if (params.indexOf("||") !== 0 && params !== "") {
            wasStringNormalized = false;
            if (params.indexOf("|") == 0) {
                params = "|" + params;
            }
            else {
                params = "||" + params;
            }
        }

        var spacedParams = params;
        spacedParams = spacedParams.replace("|||", "| ||");

        var ignoreReadOnly = "";
        if (spacedParams.indexOf("||IGNOREREADONLY|1") !== -1) {
            spacedParams = spacedParams.replace("||IGNOREREADONLY|1", "");
            ignoreReadOnly = "||IGNOREREADONLY|1";
        }

        /// OBO
        var replacedSpacedParams = spacedParams;
        do {
            spacedParams = replacedSpacedParams;
            if (spacedParams.indexOf("||" + key + "|") > -1) {
                var oldParam = spacedParams.split('||').filter(function (element) {
                    return element.indexOf(key + "|") === 0 ? element : '';
                })[0];
                replacedSpacedParams = replacedSpacedParams.replace("||" + oldParam, "");
            }
        } while (replacedSpacedParams != spacedParams);

        params = spacedParams.replace("| ||", "|||");
        params += '||' + key + '|' + value;
        if (params.indexOf("||") === 0) {
            params = params.substr(2);
        }

        return startString + params + ignoreReadOnly + endString;
    },
    dateStrToInt: function (str) {
        if (typeof str == "string" && str.length >= 10 && str.indexOf("/") > -1) {
            return parseInt(str.substr(6) + str.substr(3, 2) + str.substr(0, 2));
        }

        return str;
    },
    albParam: {
        getElement: function () {
            return common.dom.get("AddParamValue");
        },
        getString: function () {
            return $(common.albParam.getElement()).val();
        },
        setString: function (str) {
            $(common.albParam.getElement()).val(str);
        },
        getValue: function (key) {
            var paramValues = common.albParam.getString().split("||");
            let v = paramValues.filter(function (element) {
                return element.indexOf(key + "|") == 0;
            })[0];
            return v ? v.split("|")[1] : "";
        },
        setValue: function (key, value, allowMultiple) {
            let self = common.albParam;
            let fullString = self.getString();
            let isMissingKey = true;
            let paramValues = fullString.split("||").map(function (x) {
                let y = x.split("|");
                if (key == y[0]) {
                    isMissingKey = false;
                }
                return { key: y[0], value: y[1] };
            });
            if (isMissingKey) {
                self.setString(fullString + "||" + key + "|" + value);
            }
            else {
                let newParams = [];
                let duplicateKeys = null;
                if (allowMultiple) {
                    // perform reverse so that the last value is modified in mulplie key mode
                    paramValues = paramValues.reverse();
                }
                paramValues.forEach(function (x) {
                    if (newParams.some(function (p) { return p.key && p.key == x.key; })) {
                        if (allowMultiple) {
                            if (duplicateKeys == null && key == x.key) {
                                x.value = value;
                                duplicateKeys = key;
                            }
                            newParams.push(x.key + "|" + x.value);
                        }
                    }
                    else {
                        if (key == x.key) {
                            x.value = value;
                        }
                        newParams.push(x.key + "|" + x.value);
                    }
                });
                if (allowMultiple) {
                    // perform reverse again in mulplie key mode
                    newParams = newParams.reverse();
                }
                self.setString(newParams.join("||"));
            }
        },
        build: function () {
            let val = common.$val;
            return {
                ids: [val("TabGuid"), val("ModeNavig"), val("Offre_CodeOffre"), val("Offre_Version"), val("Offre_Type"), val("NumAvenantPage")],
                parameters: {
                    SaveAndQuit: val("txtSaveCancel") == "1",
                    ActeGestion: val("ActeGestion"),
                    ActeGestionRegule: val("ActeGestionRegule"),
                    IsReadOnly: val("OffreReadOnly") == "True",
                    IsForceReadOnly: val("IsForceReadOnly") == "True"
                }
            };
        },
        buildObject: function () {
            let value = common.albParam.getString();
            if (!value) {
                return {};
            }
            return value.match(/\w+\|[\w\s-\.,]*/g).reduce(function (a, x) { a[x.split("|")[0]] = x.split("|")[1]; return a; }, {});
        },
        objectToString: function (obj, addPrefix, addSuffix) {
            let newValue = Object.getOwnPropertyNames(obj).map(function (x) { return x + "|" + obj[x]; }).join("||");
            return (addPrefix ? "addParamAVN|||" : "") + newValue + (addSuffix ? "addParam" : "");
        }
    },
    pipedParams: {
        build: function (obj) {
            if (common.isUndifinedOrEmpty(obj)) {
                return "";
            }
        }
    },
    dateIntToStr: function (i, separator) {
        var s = "";
        if (typeof i != "string") {
            s = i.toString();
        }
        else {
            s = isNaN(parseInt(i)) ? "" : i;
        }

        if (s.length != 8) {
            return "";
        }

        var chars = s.split("");
        return chars[6] + chars[7] + (separator || "/") + chars[4] + chars[5] + (separator || "/") + chars[0] + chars[1] + chars[2] + chars[3];
    },
    getRegul: function () {
        return {
            id: common.albParam.getValue("REGULEID"),
            type: common.albParam.getValue("REGULTYP"),
            mode: common.albParam.getValue("REGULMOD"),
            scope: common.albParam.getValue("REGULNIV"),
            histo: common.albParam.getValue("REGULAVN")
        };
    },
    page: {
        scrollTop: function () {
            window.parent.scrollTo(0, 0);
        },
        reload: function (index) {
            if (!index || index < 0) index = 0;
            top.frames[index].location.reload(true);
        },
        divLoadingId: "divLoading",
        loadingTimeout: null,
        findMainWindow: function () {
            let context = window;
            while (context.parent && context.parent != window.top) {
                context = context.parent;
            }
            return context;
        },
        get isLoading() {
            return $("#" + common.page.divLoadingId, common.page.findMainWindow().document).is(":visible");
        },
        set isLoading(value) {
            const $div = $("#divLoading", common.page.findMainWindow().document);
            if (value) {
                $div.show().addClass("shown");
                //common.page.loadingTimeout = setTimeout(function () { $div.addClass("shown"); }, 50);
                //ShowLoading();
                
            }
            else {
                if (common.page.loadingTimeout) {
                    clearTimeout(common.page.loadingTimeout);
                    common.page.loadingTimeout = null;
                }
                $div.hide().removeClass("shown");
            }
        },
        navigateNewTab: function (url) {
            let doc = (window.parent || window).document;
            let link = doc.createElement("A");
            link.href = "/Home/Index?id=newWindow" + encodeURIComponent(url);
            link.target = "_blank";
            link.style.display = "none";
            doc.body.appendChild(link);
            link.click();
            doc.body.removeChild(link);
        },
        get currentUrl() {
            return window.location.originalPathname || window.location.pathname;
        },
        forceDirectClose: function () {
            $(window.parent || window).off("beforeunload");
            (window.parent || window).close();
        },
        goHome: function () {
            if (parent) {
                top.document.location.replace("/");
            }
            else {
                document.location.replace("/");
            }
        },
        get controller() {
            let route = document.location.pathname.split("/");
            route.shift();
            return route.length > 0 ? route[0] : "";
        },
        get action() {
            let route = document.location.pathname.split("/");
            route.shift();
            return route.length > 1 ? route[1] : "";
        },
        replace: function (url, assignHref) {
            common.page.isLoading = true;
            if (assignHref) {
                try {
                    document.location.replace(url);
                }
                catch (e) {
                    console.error(e);
                }
                return;
            }
            function replaceHtml(allHtml) {
                try {
                    //window.parent.document.getElementById("MasterFrame").srcdoc = allHtml;
                    let htmlDoc = (new DOMParser()).parseFromString(allHtml, "text/html");
                    let newTitle = $("<div>" + htmlDoc.head.innerHTML + "</div>").find("title").text();
                    $(document).find("head title").text(newTitle);
                    $(document).unbind();

                    // cannot use jQuery.html() because it removes scripts
                    //let divBody = $("<div>" + htmlDoc.body.innerHTML + "</div>");
                    //$(document.body).html(divBody.html());

                    document.body.innerHTML = htmlDoc.body.innerHTML;
                    common.page.isLoading = true;
                    history.replaceState({}, newTitle, url);
                    $(document).ready(function () {
                        globalInit.forEach(function (x) { x(); });
                    });
                    setTimeout(function () {
                        // force scripts with src to be executed
                        $("body script[src]").each(function (x, e) {
                            $.getScript(e.src, function () { console.log("script " + e.src + " loaded"); });
                        });
                        common.page.isLoading = false;
                    }, 5);
                }
                catch (x) {
                    console.error(x);
                    common.error.showMessage(x.message);
                }
            }
            function handleError(x) {
                common.error.showXhr(x);
            }
            try {
                $.get(url).then(replaceHtml, handleError);
            }
            catch (e) {
                console.error(e);
                common.error.showMessage(e.message);
            }
        }
    },
    autonumeric: {
        modes: {
            initilize: "init",
            update: "update",
            destroy: "destroy"
        },
        formats: {
            integer: "numeric",
            decimal: "decimal",
            percent: "pourcent",
            perThousand: "pourmille",
            year: "year",
            identifier: "identifier"
        },
        defaultSettings: function (attribute) {
            var self = common.autonumeric;
            if (!attribute) {
                attribute = self.formats.integer;
            }

            var nbDecimals = (attribute.indexOf(self.formats.decimal) > -1) ? 2 : 0;

            var bounds = { min: 0, max: null };
            switch (attribute) {
                case self.formats.integer:
                case self.formats.identifier:
                    bounds.max = 999999999;
                    break;
                case (self.formats.percent + self.formats.integer):
                    bounds.max = 100;
                    break;
                case (self.formats.perThousand + self.formats.integer):
                    bounds.max = 1000;
                    break;
                case (self.formats.percent + self.formats.decimal):
                    bounds.max = 100.00;
                    break;
                case (self.formats.perThousand + self.formats.decimal):
                    bounds.max = 1000.00;
                    break;
                case self.formats.decimal:
                    bounds.max = 999999999.99;
                    break;
                case self.formats.year:
                    bounds.max = 9999;
                    break;
            }

            return {
                digitGroupSeparator: " ",
                decimalCharacter: ",",
                decimalCharacterAlternative: ".",
                decimalPlacesOverride: nbDecimals,
                maximumValue: bounds.max,
                minimumValue: bounds.min
            };
        },
        getNumOrVal: function (input) {
            if (!(input instanceof jQuery)) {
                if (input instanceof HTMLElement) {
                    input = $(input);
                }
                else {
                    return null;
                }
            }
            return input.autoNumeric && input.autoNumeric("getSettings") ? input.autoNumeric("get") : input.val();
        },

        //--------Formatage d'un input/span spécifique-----
        // element : un ou plusieurs éléments jQuery                                                                                            OBLIGATOIRE
        // mode : init/update/destroy                                                                                                           OBLIGATOIRE
        // attribut : nom de l'attribut albMask => numeric/decimal/pourcentnumeric/pourcentdecimal/pourmillenumeric/pourmilledecimal/year       OBLIGATOIRE POUR UN MODE != destroy
        // separator : séparateur entre les milliers (par défaut ' ')
        // decimal : séparateur décimal (par défaut ',')
        // nbrDec : nombre de décimale après le séparateur décimal (par défaut '0' pour un numérique, '2' pour un décimal)
        // valueMax : valeur maxi acceptée (par défaut '999999999' pour un numérique, '999999999.99' pour un décimal, '1000' pour un pourmille, sinon 100)
        // valueMin : valeur mini acceptée (par défaut '0')
        apply: function (element, mode, attribut, separator, decimalSeparator, nbDecimals, valueMax, valueMin) {
            var self = common.autonumeric;
            if (mode == self.modes.destroy) {
                element.autoNumeric(mode);
            }
            else {
                if (!attribut) return;
                var defSettings = self.defaultSettings(attribut);
                if (separator == null) separator = defSettings.digitGroupSeparator;
                if (decimalSeparator == null) decimalSeparator = defSettings.decimalCharacter;
                if (nbDecimals == null) nbDecimals = defSettings.decimalPlacesOverride;

                var bounds = {
                    min: (valueMin == null ? defSettings.minimumValue : valueMin),
                    max: (valueMax == null ? defSettings.maximumValue : valueMax)
                };

                if (mode == self.modes.initilize) {
                    let newElements = element.filter(function () { return !$(this).autoNumeric('getSettings'); });

                    newElements.each(function () {
                        self.preformatElementValue(this);
                    });
                    newElements.autoNumeric(mode, { digitGroupSeparator: separator, decimalCharacter: decimalSeparator, decimalCharacterAlternative: ".", decimalPlacesOverride: nbDecimals });
                    try {
                        newElements.autoNumeric(self.modes.update, { maximumValue: bounds.max, minimumValue: bounds.min });
                    }
                    catch (e) {
                        console.error(e);
                    }
                }
                else {
                    element.autoNumeric(mode, { digitGroupSeparator: separator, decimalCharacter: decimalSeparator, decimalCharacterAlternative: ".", decimalPlacesOverride: nbDecimals, maximumValue: bounds.max, minimumValue: bounds.min });
                }
            }
        },

        preformatElementValue: function (item) {
            if (!item) return;
            var isInput = item.tagName.toLowerCase() === "input";
            var value = isInput ? item.value : $(item).text();
            if (value !== 0 && value) {
                value = value.replace(/\s/g, "");
                value = value.replace(",", ".");
                if (isInput) {
                    $(item).val(value);
                }
                else {
                    $(item).text(value);
                }
            }
        },

        preformatElementValue: function (item) {
            if (!item) return;
            var isInput = item.tagName.toLowerCase() === "input";
            var value = isInput ? item.value : $(item).text();
            if (value !== 0 && value) {
                value = value.replace(/\s/g, "");
                value = value.replace(",", ".");
                if (isInput) {
                    $(item).val(value);
                }
                else {
                    $(item).text(value);
                }
            }
        },

        //-----------Formatage des input/span------------
        // mode : init/update/destroy                                                                                                           OBLIGATOIRE
        // attribut : nom de l'attribut albMask => numeric/decimal/pourcentnumeric/pourcentdecimal/pourmillenumeric/pourmilledecimal/year       OBLIGATOIRE
        // separator : séparateur entre les milliers (par défaut ' ')
        // decimal : séparateur décimal (par défaut ',')
        // nbrDec : nombre de décimale après le séparateur décimal (par défaut '0' pour un numérique, '2' pour un décimal)
        // valueMax : valeur maxi acceptée (par défaut '999999999' pour un numérique, '999999999.99' pour un décimal, '1000' pour un pourmille, sinon 100)
        // valueMin : valeur mini acceptée (par défaut '0')
        applyAll: function (mode, attribut, separator, decimalSeparator, nbDecimals, valueMax, valueMin) {
            common.autonumeric.apply($("input[albMask=" + attribut + "], span[albMask=" + attribut + "]"), mode, attribut, separator, decimalSeparator, nbDecimals, valueMax, valueMin);
        },

        formatAll: function ($selector, separator, decimalSeparator, nbDecimals, valueMax, valueMin) {
            if (!$selector || !$selector.each) return;
            $selector.each(function () {
                if ($(this).attr("albMask")) {
                    var defSettings = common.autonumeric.defaultSettings($(this).attr("albMask"));
                    if (separator == null) separator = defSettings.digitGroupSeparator;
                    if (decimalSeparator == null) decimalSeparator = defSettings.decimalCharacter;
                    if (nbDecimals == null) nbDecimals = defSettings.decimalPlacesOverride;

                    var bounds = {
                        min: (valueMin == null ? defSettings.minimumValue : valueMin),
                        max: (valueMax == null ? defSettings.maximumValue : valueMax)
                    };
                    var isInput = this.tagName.toLowerCase() == "input";
                    var value = isInput ? ($(this).val() || "") : ($(this).text() || "");
                    if (value == "") return;
                    try {
                        var newValue = common.dom.w.autonumeric.default.format(
                            value,
                            {
                                digitGroupSeparator: separator,
                                decimalCharacter: decimalSeparator,
                                decimalCharacterAlternative: ".",
                                decimalPlacesOverride: nbDecimals,
                                maximumValue: bounds.max,
                                minimumValue: bounds.min
                            });

                        if (isInput) {
                            $(this).val(newValue);
                        }
                        else {
                            $(this).text(newValue);
                        }
                    }
                    catch (e) {
                        console.error(e);
                        $(this).val(value);
                    }
                }
            });

        }
    },
    razor: {
        getFormNames: function (prop, val) {
            var names = [];
            if ($.isArray(val)) {
                names = common.razor.getArrayFormNames(prop, val, names);
            }
            else if (typeof val == "object") {
                names = common.razor.getObjectFormNames(prop, val, names);
            }

            return names;
        },
        getArrayFormNames: function (prop, array, names) {
            var isObjectArray = array.some(function (e) { return typeof e == "object"; });
            for (var i = 0; i < array.length; i++) {
                var name = prop + (isObjectArray ? ("[" + i + "]") : "");
                var id = name + "_" + i;

                var subNames = common.razor.getFormNames(name, array[i]);
                if (subNames.length == 0) {
                    names.push({ id: id.replace(/[\.\[\]]/g, "_"), name: name, value: (array[i] || null) });
                }
                else {
                    names = names.concat(subNames);
                }

                if (prop) {
                    id = prop;
                }
            }

            return names;
        },
        getObjectFormNames: function (prop, obj, names) {
            for (var p in obj) {
                prop = (prop ? prop + "." : "") + p;

                var subNames = common.razor.getFormNames(prop, obj[p]);
                if (subNames.length == 0) {
                    names.push({ id: prop.replace(/[\.\[\]]/g, "_"), name: prop, value: (obj[p] || null) });
                }
                else {
                    names = names.concat(subNames);
                }

                if (prop) {
                    var x = prop.split(".");
                    x.pop();
                    prop = x.join(".");
                }
            }

            return names;
        }
    },
    $serializeSelection: function (selection, forceSingleValue, multipleValueListName) {
        multipleValueListName = multipleValueListName || "_nameValues";
        let multiList = {};
        if (!(selection instanceof jQuery)) {
            throw "Invalid parameter";
        }
        let obj = {};
        selection.each(function () {
            const key = this.name;
            if (!key) {
                return true;
            }
            const value = defineValue(this);
            if (key.search(/[\.\[]/) > 0) {
                let a = key.split('.').filter(function (x) { return x !== ""; });
                let p = "";
                let o = obj;
                while (p = a.shift()) {
                    if (p.indexOf('[') > 0) {
                        let x = parseInt(/\[(\d+)\]/.exec(p)[1]);
                        fillArray(o, p, x, a.length == 0 ? value : {});
                        if (p.length > 0) {
                            o = o[p][x];
                        }
                    }
                    else {
                        if (a.length == 0) {
                            assignValue(o, p, value);
                        }
                        else {
                            assignObject(o, p);
                        }
                        if (p.length > 0) {
                            o = o[p];
                        }
                    }
                }
            }
            else {
                assignValue(obj, key, value);
            }
        });

        function defineValue(element) {
            if (!(element instanceof HTMLElement)) {
                return null;
            }
            if (element instanceof HTMLInputElement) {
                if (element.type === "radio") {
                    return $("[name='" + element.name + "']:checked").val();
                }
                else if (element.type === "checkbox") {
                    return element.checked;
                }
            }
            return $(element).val();
        }

        function assignValue(o, key, value) {
            if (o === obj && o[key] !== undefined) {
                if (!Array.isArray(multiList[key])) {
                    multiList[key] = [o[key]];
                }
                multiList[key].push(value);
            }
            o[key] = value;
            if (typeof onValueAssigning === "function") {
                if (onValueAssigning(o, key, value) === false) {
                    delete o[key];
                }
            }
        }

        function assignObject(o, key) {
            if (o[key] === undefined) {
                o[key] = {};
            }
        }

        function fillArray(o, key, index, value) {
            if (!Array.isArray(o[key])) {
                o[key] = [];
            }
            if (value.isEmpty()) {
                if (o[key][index] == null) {
                    o[key][index] = {};
                }
                else {
                    return;
                }
            }
            else {
                if (isNaN(index) || index < 0) {
                    o[key].push(value);
                }
                else {
                    o[index] = value;
                }
            }
            if (typeof onValueAssigning === "function") {
                if (onValueAssigning(o, key, value) === false) {
                    if (isNaN(index) || index < 0) {
                        o[key].pop();
                    }
                    else {
                        o.splice(index, 1);
                    }
                }
            }
        }

        obj[multipleValueListName] = multiList;

        if (forceSingleValue) {
            // on first level props, avoid duplicate names and get first non-empty value if available
            Object.keys(obj[multipleValueListName]).filter(function (x) { return obj[multipleValueListName][x].length > 1; }).forEach(function (x) {
                obj[x] = obj[multipleValueListName][x].first(function (v) { return !!v; });
            });
        }

        return obj;
    },
    $postJson: function (url, obj, popError, isObjJson) {
        return $.ajax({
            type: "POST",
            url: url,
            data: isObjJson ? obj : JSON.stringify(obj),
            contentType: "application/json"
        }).fail(function (x, s, e) {
            if (popError) common.error.show(x);
        });
    },
    $getJson: function (url, parameters, popError, forceNewUrl) {
        let querystring = "?";
        try {
            if (parameters) {
                let typeOfParams = typeof parameters;
                if (typeOfParams === "function") {
                    parameters = parameters();
                    typeOfParams = typeof parameters;
                }
                if (typeOfParams === "string") {
                    // assuming it is a json string
                    parameters = JSON.parse(parameters);
                }
                else if (Array.isArray(parameters) || typeOfParams !== "object") {
                    parameters = { value: parameters };
                }
                querystring = "?" + $.param(parameters) + (forceNewUrl ? "&" : "");
            }
        }
        catch (e) {
            querystring = "?";
            console.error(e);
        }
        if (forceNewUrl) {
            querystring += "_=" + new Date().getTime();
        }
        var request = $.ajax({
            type: "GET",
            url: url + querystring,
            contentType: "application/json"
        });
        if (popError) {
            request.fail(function (x, s, e) {
                if (popError) common.error.show(x);
            });
        }
        return request;
    },
    $val: function (id) {
        return $("#" + id).val() === 0 ? 0 : ($("#" + id).val() || "");
    },
    $stringVal: function (id) {
        return $("#" + id).val() === 0 ? "0" : ($("#" + id).val() || "").toString();
    },
    $removeEnclosingDiv: function (nodes) {
        return $(nodes).filter('*').first().children();
    },
    $ui: {
        getConfirmDiv: function (key, message, confirmText, doNotConfirmText) {
            let $div = $('<div style="display: none;"></div>');
            let $msg = $('<div class="container container-details"><div class="main-title">' + message + '</div></div>');
            let $footer = $('<div class="container container-foot"><div class="input-line"><div class="flex5-10"></div><div class="flex5"></div></div></div>');
            $footer.find(".flex5-10").append('<button id="' + key + 'Yes" name="' + key + '">' + (confirmText || "Oui") + '</button>');
            $footer.find(".flex5").append('<button id="' + key + 'No" name="' + key + '">' + (doNotConfirmText || "Non") + '</button>');
            $div.append($msg);
            $div.append($footer);
            return $div;
        },
        getBasicDialog: function (key, message, closeText) {
            let $div = $('<div style="display: none;"></div>');
            let $msg = $('<div class="container container-details"><div class="main-title">' + message + '</div></div>');
            let $footer = $('<div class="container container-foot"><div class="flex8-10"></div><div class="flex2-10"></div></div>');
            $footer.find(".flex2-10").append('<button id="button_close_' + key + '">' + (closeText || "Fermer") + '</button>');
            $div.append($msg);
            $div.append($footer);
            return $div;
        },
        initWidgetOptions: function () {
            $.ui.dialog.prototype.options.modal = true;
            $.ui.dialog.prototype.options.closeOnEscape = false;
        },
        showDialog: function ($div, classes, title, size, copyHtml, hideCrossClose, dialogClass, doNotDestroy) {
            if (!$div) {
                return;
            }
            else if (typeof $div === "string") {
                $div = $($div);
            }
            if (copyHtml) {
                $div = $($div[0].outerHTML).insertAfter($div);
            }
            if (Array.isArray(classes)) {
                classes.forEach(function (value) { $div.addClass(value); });
            }
            $div.attr("title", title);
            // see common.$ui.initWidgetOptions for default value settings
            $div.dialog({
                dialogClass: (hideCrossClose ? "no-cross-close " : " ") + dialogClass,
                position: { my: "top", at: "top", of: window },
                minWidth: common.interface.implements(size, "size") ? size.width : 700,
                close: function (ev, ui) {
                    if (doNotDestroy) {
                        return;
                    }

                    $div.dialog("destroy");
                    $div.remove();
                },
                closeOnEscape: false
            });

            if (common.interface.implements(size, "size")) {
                $div.dialog("option", "resizable", false);
                if (size.height) {
                    $div.dialog("option", "height", size.height);
                }
                if (size.width) {
                    $div.dialog("option", "width", size.width);
                }
            }

            return $div;
        },
        showMessage: function (message, type, onClose) {
            let messageType = ((typeof type === "string" ? type : null) || "info").toLowerCase();
            if (typeof message !== "string") {
                switch (messageType) {
                    case "error":
                        throw "Error";
                    case "info":
                        alert("?");
                        break;
                }
                return;
            }
            let $div = common.$ui.getBasicDialog(messageType, message);
            $div.appendTo(document.body);
            $("#button_close_" + messageType).kclick(function () {
                $div.dialog("close");
                $div.dialog("destroy");
            });
            switch (messageType) {
                case "error":
                    return $div.dialog({
                        minWidth: 100,
                        maxWidth: 300,
                        modal: true
                    });
                default:
                    return $div.dialog({
                        minWidth: 100,
                        maxWidth: 300,
                        modal: true
                    });
            }
        },
        showConfirm: function (confirmMessage, title, doConfirm, doNotConfirm, yesText, noText, onClose, minWidth) {
            if (!$.isFunction(doConfirm) || !$.isFunction(doNotConfirm)) { return true; }
            if (!confirmMessage) { confirmMessage = "Confirmer ?"; }
            if (!$.isFunction(onClose)) { onClose = doNotConfirm; }
            const key = "uiConfirm";
            let $div = common.$ui.getConfirmDiv(key, confirmMessage, yesText, noText);
            $div.appendTo(document.body);
            $("#" + key + "Yes").kclick(doConfirm);
            $("#" + key + "No").kclick(doNotConfirm);
            $("[name='" + key + "']").kclick(function () { $div.dialog("close"); });
            $div.dialog({
                title: title,
                position: { my: "top", at: "top", of: window },
                minWidth: minWidth || 200,
                width: minWidth || 200,
                close: function (ev, ui) {
                    onClose();
                    $div.dialog("destroy");
                    $div.remove();
                }
            });
        }
    },
    $infer: function (item) {
        let $element = null;
        if (item instanceof jQuery) {
            $element = item;
        }
        else if (typeof item === "string") {
            $element = $((item.indexOf("#") === 0 ? "" : "#") + item);
            if ($element.length === 0) {
                $element = $("[name='" + item + "']").get(0);
            }
        }
        else if (item instanceof HTMLElement) {
            $element = $(item);
        }
        return $element;
    },
    $initContextMenu: function (selector, triggerName, dataSet, reset) {
        if (!selector) {
            return;
        }
        if (reset) {
            $.contextMenu("destroy", selector);
        }
        $.contextMenu({
            selector: selector,
            trigger: triggerName || "none",
            zIndex: 999,
            callback: contextMenu.triggerMenuAction,
            items: typeof dataSet === "string" ? $.parseJSON(dataSet) : dataSet
        });
    },
    knockout: {
        binding: {
            init2CbxPreventNoCheck: function (prop1, prop2, onlyChk1, onlyChk2, onChange) {
                prop1.subscribe(function (v) {
                    if (!prop1() && !prop2()) {
                        prop1(true);
                    }
                    else {
                        onChange();
                    }
                });
                prop2.subscribe(function (v) {
                    if (!prop1() && !prop2()) {
                        prop2(true);
                    }
                    else {
                        onChange();
                    }
                });
                onChange = function () {
                    if (prop1() && prop2()) {
                        onlyChk1(false);
                        onlyChk2(false);
                    }
                    else {
                        onlyChk1(prop1());
                        onlyChk2(prop2());
                    }
                };
            }
        },
        arrays: {
            fill: function (koArray, values, ignoreKoMapping, replaceContent) {
                if (!$.isArray(values) || !ko.isObservable(koArray)) {
                    return;
                }

                if (!ignoreKoMapping && koArray.mappedCreate) {
                    if (replaceContent) {
                        koArray.mappedRemoveAll();
                    }
                    values.forEach(function (v) {
                        koArray.mappedCreate(v);
                    });
                }
                else {
                    if (replaceContent) {
                        koArray.removeAll();
                    }
                    ko.utils.arrayPushAll(koArray, values);
                }
            }
        },
        components: {
            viewModels: {},
            addViewModel: function (cpname, vm, vmname) {
                if (!viewModels[cpname]) {
                    viewModels[cpname] = [];
                }

                viewModels[cpname].push({ name: vmname, data: vm });
            },
            register: function (name, vmclass, templateFilename, fullQualifiedFilenames) {
                ko.components.register(name, {
                    template: { filename: templateFilename, version: (window.scriptVersion || 1), fullQualifiedFilenames: fullQualifiedFilenames },
                    viewModel: {
                        createViewModel: function (params, componentInfo) {
                            // - 'params' is an object whose key/value pairs are the parameters
                            //   passed from the component binding or custom element
                            // - 'componentInfo.element' is the element the component is being
                            //   injected into. When createViewModel is called, the template has
                            //   already been injected into this element, but isn't yet bound.
                            // - 'componentInfo.templateNodes' is an array containing any DOM
                            //   nodes that have been supplied to the component. See below.

                            // Return the desired view model instance, e.g.:
                            return new vmclass(params);
                        }
                    }
                });

                common.knockout.components.bind(name);
            },
            include: function (name, html, init) {
                var $e = $(html).appendTo(document.body);
                if ($.isFunction(init)) {
                    if (!init()) common.knockout.components.bind(name, [$e[0]]);
                }
                else {
                    common.knockout.components.bind(name, [$e[0]]);
                }

                return $e;
            },
            includeInDiv: function (config, paramsAttr, includeAsDiv) {
                if (config && common.interface.implements(config, "ko-component")) {
                    var html = includeAsDiv ?
                        "<div data-bind=\"component: { name: '" + config.componentName + "', params: { " + paramsAttr + " } }\"></div>"
                        : "<" + config.componentName + " params=\"" + paramsAttr.replace(/"/g, "&quot;") + "\"></" + config.componentName + ">";

                    return common.knockout.components.include(config.componentName, html, config.init);
                }

                return null;
            },
            includeInDialog: function (title, size, config, paramsAttr, includeAsDiv, hideCrossClose) {
                common.page.isLoading = true;
                try {
                    var $div = common.knockout.components.includeInDiv(config, paramsAttr, includeAsDiv);
                    return common.$ui.showDialog($div, ["ko-component-popup"], title, size, false, hideCrossClose);
                }
                catch (err) {
                    common.page.isLoading = false;
                    console.error(err);
                    common.error.showMessage(err.message);
                }
            },
            bind: function (name, nodes) {
                try {
                    if (!nodes || (nodes.length || 0) == 0) {
                        nodes = $(name + "," + "[name='" + name + "']");
                    }

                    for (var x = 0; x < nodes.length; x++) {
                        ko.applyBindingsToNode(nodes[x]);
                    }
                }
                catch (e) {
                    console.error(e);
                }
            },
            disposeFromDialog: function (componentName) {
                if (!ko.components.isRegistered(componentName)) {
                    return false;
                }
                const $comp = $(componentName);
                $comp.dialog("close");
                ko.cleanNode($comp[0]);
                return $comp.remove();
            }
        }
    },
    dialog: {
        initConfirm: function (onOk, onCancel, message, oKLabel, cancelLabel, options) {
            let width = (options || {}).width || 350,
                height = (options || {}).height || 130;

            if (!$.isFunction(onOk)) {
                return;
            }
            else {
                onOk = !$.isFunction(onOk) ? CloseCommonFancy : onOk;
                onCancel = !$.isFunction(onCancel) ? CloseCommonFancy : onCancel;
            }

            $("[name='cell-confirm-buttons'] #btnConfirmOk").kclick(function () { CloseCommonFancy(); onOk(); });
            $("[name='cell-confirm-buttons'] #btnConfirmCancel").kclick(function () { CloseCommonFancy(); onCancel(); });

            if ((oKLabel != undefined)) {
                $("[name='cell-confirm-buttons'] #btnConfirmOk").html(oKLabel);
            }
            if ((cancelLabel != undefined)) {
                $("[name='cell-confirm-buttons'] #btnConfirmCancel").html(cancelLabel);
            }

            if (typeof message == "string") {
                ShowCommonFancy("Confirm", "", message, width, height, true, true);
            }
            return true;
        },
        show: function (message, type, size) {
            const defaultSize = { width: 300, height: 80, fixed: true };
            size = (size && (size.width || size.height)) ? size : defaultSize;
            ShowCommonFancy(type || "Info", "", message, size.width, size.height, true, true, size.fixed);
        },
        info: function (message, size) {
            common.dialog.show(message, "Info", size);
        },
        error: function (message, size) {
            common.dialog.show(message, "Error", size);
        },
        smallError: function (message, adaptSize) {
            common.dialog.error(message, { width: 150, height: 85, fixed: !adaptSize });
        },
        bigInfo: function (message, adaptSize) {
            common.dialog.info(message, { width: 1212, height: 700, fixed: !adaptSize });
        },
        bigError: function (message, adaptSize) {
            common.dialog.error(message, { width: 1212, height: 700, fixed: !adaptSize });
        }
    },
    error: {
        show: function (message, ignoreLoader, iframe, logOnly) {
            return common.error.showErrorJson(message, message.statusText, ignoreLoader, iframe, logOnly);
        },
        showXhr: function (jqXHR, ignoreLoader, iframe) {
            return common.error.show(jqXHR, ignoreLoader, iframe);
        },
        showErrorJson: function (resp, err, ignoreLoader, iframe, logOnly) {
            let message;
            let msg = err || resp;
            if (resp.responseText) {
                try {
                    msg = JSON.parse(resp.responseText);
                } catch (e) { }
            }
            if ($.isArray(msg)) {
                message = msg.map(function (x) {
                    return x && x.toString();
                }).join("\n");
            } else if (msg instanceof Object) {
                if (kheops.errors.display(msg, ignoreLoader, logOnly)) {
                    return msg;
                }
                else {
                    message = msg.ErrorMessage || msg.Message || msg.StatusText || msg.message || JSON.stringify(msg);
                    if (message.indexOf("ERRORCACHE") > 0) {
                        message = "Vous avez dépassé le délai d'inactivité autorisé, vous devez recharger la page.";
                    }
                }
            } else {
                message = msg;
            }

            message = message.replace(/\r?\n/g, "<br/>");
            console.error(message);

            if (logOnly) {
                console.log(message);
            }
            else {
                ShowCommonFancy("Error", "", message, undefined, undefined, true, true, false, iframe);
            }

            if (!ignoreLoader) {
                common.page.isLoading = false;
            }
            return msg;
        },
        showMessage: function (msg, ignoreLoader, iframe, logOnly) {
            var message;

            if ($.isArray(msg)) {
                message = msg.filter(function (m) { return m.Error === undefined || m.Error !== ""; }).map(function (m) { return m && m.Error || m; }).join("\n");
            } else if (msg && typeof msg === typeof {}) {
                message = msg.ErrorMessage || msg.message || (msg.originalEvent && msg.originalEvent.message) || msg.Message || msg.StatusText || JSON.stringify(msg);
            } else {
                message = msg.toString();
            }

            message = message.replace(/\r?\n/g, "<br/>");

            if (logOnly) {
                console.error(msg);
            }
            else {
                ShowCommonFancy("Error", "", message, undefined, undefined, true, true, true, iframe);
            }

            if (!ignoreLoader) {
                common.page.isLoading = false;
            }
        },
        extractMessage: function (e) {
            if (e && e.ErrorMessage) {
                if (e.ErrorMessage.indexOf("##;") == 0) {
                    var parts = e.ErrorMessage.split(";");
                    return parts[2];
                }
                else {
                    return e.ErrorMessage;
                }
            }

            return "Erreur";
        },
        messages: {
            requiredFields: "Veuillez saisir les champs requis"
        }
    },
    interface: {
        knownContracts: {
            "ko-component": [{ prop: "componentName", types: ["string"] }, { prop: "init", types: ["function"] }],
            "size": [{ prop: "width", types: ["number", "string"] }, { prop: "height", types: ["number", "string"] }]
        },
        implements: function (obj, contractName) {
            var contract = common.interface.knownContracts[contractName];
            if (contract && obj) {
                for (var i = 0; i < contract.length; i++) {
                    if (contract[i].prop in obj && typeof contract[i].types.some(function (t) { return obj[contract[i].prop] === t; })) { }
                    else return false;
                }

                return true;
            }

            return null;
        }
    },
    get tabGuid() {
        return ($("#tabGuid").val() || "").length > 14 ? $("#tabGuid").val() : $("#homeTabGuid", window.top.document).val();
    }
};

$(function () {
    common.$ui.initWidgetOptions();
});
