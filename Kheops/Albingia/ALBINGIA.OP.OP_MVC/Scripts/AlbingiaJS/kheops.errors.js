
var kheops = {
    exceptions: {
        businessValidation: "Albingia.Kheops.Common.BusinessValidationException",
        businessError: "OPWebService.BusinessError",
        blacklist: "ALBINGIA.Framework.Common.BlacklistException"
    },
    errors: {
        isCustomError: function (error) {
            if (error instanceof Object) {
                return (error.$type == kheops.exceptions.businessValidation || error.$type == kheops.exceptions.businessError) && Array.isArray(error.Errors);
            }
            return null;
        },
        display: function (obj, ignoreLoader, logOnly) {
            if (kheops.errors.isCustomError(obj)) {
                if (obj.Errors.some(function (x) { return x.FieldName; })) {
                    kheops.errors.displayAndHighlight(obj);
                }
                else {
                    common.error.showMessage(obj.Errors, ignoreLoader, false, logOnly);
                }
                return true;
            }
            return false;
        },
        displayAndHighlight: function (obj, ignoreLoader, logOnly) {
            if (kheops.errors.isCustomError(obj)) {
                if (window.mapElementsFieldNamesErrors === undefined) {
                    window.mapElementsFieldNamesErrors = [];
                }
                let mapNames = window.mapElementsFieldNamesErrors;
                common.error.showMessage(obj.Errors, ignoreLoader, false, logOnly);
                obj.Errors.forEach(function (er) {
                    let elementMatch = mapNames.filter(function (x) { return x.fieldname == er.FieldName })[0] || { element: er.FieldName };
                    let selector = "";
                    if (typeof elementMatch.element === "function" && elementMatch.paramError) {
                        selector = elementMatch.element(er[elementMatch.paramError]);
                    }
                    else {
                        selector = "#" + elementMatch.element.replace(/,/g, ",#");
                    }
                    //$(selector).addClass(elementMatch.className || "requiredField");
                    $(selector).each(function (x, e) {
                        $(e).addClass(elementMatch.className || "requiredField");
                    });
                });
                return logOnly ? obj.Errors.map(function (e) { return e.FieldName + ": " + e.Error; }).join("; ") : true;
                //return  true;
            }
            return false;
        }
    },
    alerts: {
        onChangeInputs: function (e, $l, $i) {
            if (this.getAttribute("value") == $(this).val()) {
                $i.addClass("alert");
                $l.parent().removeClass("hide-it");
                return true;
            }
            $i.removeClass("alert");
            $l.parent().addClass("hide-it");
        },
        blacklist: {
            Context: function (name, alerts, typeName, intrlTypeName) {
                this.name = (name || "").toLowerCase();
                this.alerts = alerts || [];
                this.type = typeName || "";
                this.typeIntrl = intrlTypeName || "";
                if (this.name === "courtier") {
                    this.role = typeName.toLowerCase().replace(this.name, "");
                }
                else {
                    this.role = this.name;
                }
            },
            display: function (context) {
                let alerts = context.alerts.filter(function (a) { return a.Partner.Type == context.type || context.typeIntrl && a.Partner.Type == context.typeIntrl; });
                let $divs = $("[data-" + context.name + "='" + context.role + "']");
                if ($divs.length == 0) {
                    return;
                }
                let $link = $divs.find(".alerte-blacklisted");
                let $inputs = $divs.find("input[type=text]");
                $inputs.removeClass("alert");
                $link.parent().addClass("hide-it");
                if (alerts.length > 0) {
                    let alert = null;
                    if (!context.typeIntrl || alerts.length == 2 || alerts.first().Partner.Type == context.type) {
                        alert = alerts.first(function (a) { return a.Partner.Type == context.type; });
                        $inputs = $divs.find("input[type=text]").not("[id*=Interlocuteur]");
                    }
                    else {
                        alert = alerts.first(function (a) { return a.Partner.Type == "Interlocuteur"; });
                        $inputs = $divs.find("input[type=text][id*=Interlocuteur]");
                    }
                    $link.attr("title", "Visualiser les offres/contrats de ce" + (common.tools.startsWithVowel(context.role) ? "t " : " ") + context.role);
                    $link.parent().removeClass("hide-it");
                    $inputs.addClass("alert");
                    const handler = function (e) { kheops.alerts.onChangeInputs.bind(this)(e, $link, $inputs); };
                    $inputs.off("change input", handler).on("change input", handler);
                    $inputs.each(function (x, e) { e.setAttribute("value", $(e).val()); });
                    $link.off("click").click(function () {
                        common.knockout.components.includeInDialog(
                            "Offres/Contrats - " + alert.Partner.TypeAS400 + " - " + alert.Partner.Name,
                            { width: 1200, height: 700 },
                            blacklist.affaires,
                            "code: " + alert.Partner.Code + ", code2: " + alert.Partner.RepresentativeCode + ", type: '" + alert.Partner.Type + "', name: '" + alert.Partner.Name.replace(/\'/g, "\\'") + "'");
                    });
                }

                $(document).off([customEvents.blacklist.affaires.loaded, customEvents.blacklist.affaires.hiding].join(" "));
                $(document).on(customEvents.blacklist.affaires.loaded, function () {
                    var hiddenZones = $(".ko-cloak");
                    hiddenZones.removeClass("hide");
                    hiddenZones.removeClass("conceal");
                    common.page.isLoading = false;
                });
                $(document).on(customEvents.blacklist.affaires.hiding, function () {
                    common.knockout.components.disposeFromDialog(blacklist.affaires.componentName);
                });
            },
            displayAll: function (obj) {
                if (obj && (obj.$type || "").toLowerCase() == kheops.exceptions.blacklist.toLowerCase() && Array.isArray(obj.Alerts)) {
                    common.error.showMessage(obj.Message);
                    kheops.alerts.blacklist.display(new kheops.alerts.blacklist.Context("Courtier", obj.Alerts, "CourtierGestionnaire", "Interlocuteur"));
                    kheops.alerts.blacklist.display(new kheops.alerts.blacklist.Context("Courtier", obj.Alerts, "CourtierApporteur"));
                    kheops.alerts.blacklist.display(new kheops.alerts.blacklist.Context("Preneur", obj.Alerts, "PreneurAssurance"));
                    
                    return obj.Alerts;
                }
                return false;
            }
        }
    }
};
