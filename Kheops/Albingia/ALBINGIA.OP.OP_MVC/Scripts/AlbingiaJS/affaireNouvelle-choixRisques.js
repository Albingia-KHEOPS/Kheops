var affaireNouvelle = affaireNouvelle || {};
affaireNouvelle.choixRisques = {
    init: function () {
        let button = chxRsq.$button();
        button.on("click", chxRsq.initAutoSubmit);
        button.on("click", function () {
            chxRsq.submitter = this;
            chxRsq.$ajaxForm().submit();
        });
        chxRsq.submitDone();
        chxRsq.initAutoSubmit();
        $("#btnAnnuler").on("click", function () {
            common.page.isLoading = true;
            common.dom.redirect("/CreationAffaireNouvelle/Index?id=", currentAffair.getUrlId() + "_0" + infosTab.fullTabGuid + infosTab.fullModeNavig);
        });
        $(document).on("click", "[name='" + chxRsq.expanderName + "']", function () {
            chxRsq.submitter = this;
            chxRsq.$ajaxForm().submit();
        });
    },
    submitter: null,
    lastFocused: null,
    startUpdate: function (rq) {
        common.page.isLoading = true;
        try {
            let name = (chxRsq.submitter || {}).name;
            if (name == chxRsq.expanderName || !name) {
                name = chxRsq.submitter.id;
            }
            rq.setRequestHeader("submitter", name);
            setTimeout(function () {
                chxRsq.lastFocused = $("input:focus")[0];
            }, 10);
        }
        catch (e) {
            common.page.isLoading = false;
            console.error(e);
        }
    },
    submitDone: function () {
        try {
            let noSubmit = true;
            if (chxRsq.selectionIsValid()) {
                if ((chxRsq.submitter || {}).id == chxRsq.buttonValiderId) {
                    let panel = chxRsq.$panel().clone();
                    $("#tabGuid").clone().appendTo(panel);
                    panel.removeAttr("id");
                    panel.removeAttr("name");
                    $("#form1").append(panel);
                    noSubmit = false;
                    $("#form1").submit();
                }
                else {
                    chxRsq.$button().prop({ disabled: false });
                }
            }
            else {
                chxRsq.$button().prop({ disabled: true });
                if (chxRsq.lastFocused) $("#" + chxRsq.lastFocused.id).focus();
            }
            if (noSubmit) {
                common.page.isLoading = false;
            }
        }
        catch (e) {
            common.page.isLoading = false;
            console.error(e);
        }
    },
    initAutoSubmit: function () {
        chxRsq.$button().off("click", chxRsq.initAutoSubmit);
        $(document).on("change", "#" + chxRsq.panelId + " input", function () {
            chxRsq.submitter = this;
            chxRsq.$ajaxForm().submit();
        });
    },
    selectionIsValid: function () {
        return ($("input[name='" + chxRsq.viewName + "IsValid']").val() || "").toLowerCase() == true.toString();
    },
    buttonValiderId: "btnValiderRisquesObjets",
    panelId: "selectionRisque",
    viewName: "_RisquesAffaireNouvelle",
    expanderName: "risque-expand",
    $panel: function () {
        return $("#" + chxRsq.panelId);
    },
    $button: function () {
        return $("#" + chxRsq.buttonValiderId);
    },
    $ajaxForm: function () {
        return $("#ajaxForm");
    }
};
let chxRsq = affaireNouvelle.choixRisques;
$(function () {
    chxRsq.init();
});