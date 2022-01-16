const defaultClassAutoComplete = "AutoCompleteTd";
const defaultMinLengthAutoComplete = 3;
ko.bindingHandlers.uiAutoComplete = {
    init: function (element, valueAccessor, bindingsAccessor, viewModel) {
        let boundValue = valueAccessor();
        if (typeof boundValue.type === "string" && ko.isObservable(boundValue.value)) {
            ko.bindingHandlers.uiAutoComplete.initAutoComplete(boundValue.type, element, boundValue.value);
        }
    },
    update: function (element, valueAccessor, bindingsAccessor, viewModel) {
        let boundValue = valueAccessor();
        let $e = $(typeof boundValue.target === "string" ? common.dom.get(boundValue.target) : element);
        if (typeof boundValue.type === "string") {
            let obj = boundValue.value();
            if (obj) {
                let conf = ko.bindingHandlers.uiAutoComplete.getCondig(boundValue.type);
                $e.val(conf.display.map(function (x) { return obj[x]; }).join(" - "));
            }
            else {
                $e.val("");
            }
        }
    },
    initAutoComplete: function (filterName, element, value) {
        let handler = ko.bindingHandlers.uiAutoComplete;
        let autoCompleteConfig = handler.getCondig(filterName);
        $.widget("custom." + filterName + "AutoComplete", $.ui.autocomplete, {
            options: { minLength: defaultMinLengthAutoComplete },
            _renderMenu: function (ul, items) {
                handler.renderResultHeader(this, ul, items, autoCompleteConfig);
            },
            _renderItem: function (ul, item) {
                handler.renderResultItem(ul, item, autoCompleteConfig);
            }
        });
        $(element).change(function (evt, isSelecting) {
            if (isSelecting) {
                return true;
            }
            console.log("ui change");
            value(null);
        });
        $(element)[filterName + "AutoComplete"]({
            delay: 500,
            source: "/AutoComplete/Search" + filterName,
            select: function (event, ui) {
                value(ui.item);
                event.preventDefault();
            }
        });

        return autoCompleteConfig;
    },
    renderResultHeader: function (menu, ul, items, config) {
        try {
            if (!items || items.length == 0) {
                ul.append("Aucun résultat");
                return;
            }
            items.forEach(function (item, index) {
                if (index == 0) {
                    let header = config.fields.map(function (x) { return "<th class='" + (x.textClass || defaultClassAutoComplete) + "'>" + x.displayName + "</th>"; }).join("");
                    ul.append("<li><table class='AutoCompleteTable'><tr>" + header + "</tr></table></li>");
                }
                menu._renderItem(ul, item);
            });
        }
        catch (e) {
            $.fn.jqDialogErreurOpen(e);
        }
    },
    renderResultItem: function (ul, item, config) {
        try {
            let row = "";
            if ($.isFunction(config.buildRow)) {
                row = config.buildRow(item);
            }
            else {
                row = "<tr>" + config.fields.map(function (x) {
                    let value = item[(x.name || x.displayName)];
                    let classname = (x.textClass || defaultClassAutoComplete);
                    let innerHtml = "";
                    if (x.isImage) {
                        innerHtml = "<img src='/Content/Images/" + x.imgs.first(function (i) { return i.value == value; }).name + "' />";
                    }
                    else {
                        innerHtml = (x.isArray ? value[0] : value).toString();
                    }
                    return "<td class='" + classname + "'>" + innerHtml + "</td>";
                }).join("") + "</tr>";
                if ($.isFunction(config.extraRows)) {
                    row += config.extraRows(item);
                }
            }
            return $("<li></li>")
                .data("item.autocomplete", item)
                .append("<a class='ui-menu-item'><table>" + row + "</table></a>")
                .appendTo(ul);
        }
        catch (e) {
            $.fn.jqDialogErreurOpen(e);
        }
    },
    getCondig: function (name) {
        let conf = null;
        switch (name) {
            case "Souscripteur":
            case "Gestionnaire":
                conf = {
                    name: name,
                    fields: [{ displayName: "Code" }, { displayName: "Nom" }, { displayName: "Prénom", name: "Prenom" }, { displayName: "Délégation", name: "Delegation" }],
                    display: ["Code", "Nom"]
                };
                break;
            case "PreneurAssurance":
                conf = {
                    name: name,
                    fields: [{ displayName: "Code" }, { displayName: "Nom" }, { displayName: "Secondaire", name: "NomSecondaires", isArray: true }, { displayName: "Depts", name: "Departement", textClass: "AutoCompleteCpTd" }, { displayName: "Ville" }, { displayName: "SIREN" }],
                    display: ["Nom"]
                };
                conf.extraRows = function (item) {
                    if (!item) return "";
                    if (item.NomSecondaires.length > 1) {
                        return item.NomSecondaires.map(function (nom, index) {
                            return index == 0 ? "" : ("<tr>" + conf.fields.map(function (x) {
                                "<td class='" + (x.textClass || defaultClassAutoComplete) + "'>" + (x.name == "NomSecondaires" ? nom : "") + "</td>"
                            }) + "</tr>");
                        }).join("");
                    }
                };
                break;
            case "Courtier":
                conf = {
                    name: name,
                    fields: [
                        { displayName: "Nom" },
                        { displayName: "Nom Secondaire", name: "NomSecondaires", isArray: true },
                        { displayName: "Code Postal", name: "CodePostal", textClass: "AutoCompleteCpTd" },
                        { displayName: "Ville" },
                        { displayName: "Valide", name: "EstValide", textClass: "AutoCompleteValidTd", isImage: true, imgs: [{ value: true, name: "valide.png" }, { value: false, name: "invalide.png" }] }],
                    display: ["Nom"]
                };
                conf.extraRows = function (item) {
                    if (!item) return "";
                    if (item.NomSecondaires.length > 1) {
                        return item.NomSecondaires.map(function (nom, index) {
                            return index == 0 ? "" : ("<tr>" + conf.fields.map(function (x) {
                                "<td class='" + (x.textClass || defaultClassAutoComplete) + "'>" + (x.name == "NomSecondaires" ? nom : "") + "</td>"
                            }) + "</tr>");
                        }).join("");
                    }
                };
                break;
            case "CodePostal":
                conf = {
                    name: name,
                    fields: [{ displayName: "Code Postal", name: "CodePostal" }, { displayName: "Nom" }],
                    display: ["CodePostal"]
                };
                break;
        }

        return conf;
    }
};
