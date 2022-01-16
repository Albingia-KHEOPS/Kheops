var affaireNouvelle = affaireNouvelle || {};
affaireNouvelle.choixFormules = {
    init: function () {
        let button = chxForOpt.$button();
        button.on("click", chxForOpt.initAutoSubmit);
        button.on("click", function () {
            chxForOpt.submitter = this;
            chxForOpt.$ajaxForm().submit();
        });
        chxForOpt.submitDone();
        chxForOpt.initAutoSubmit();
        $("#btnAnnuler").on("click", function () {
            common.page.isLoading = true;
            common.dom.redirect("/CreationAffaireNouvelle/Index?id=", currentAffair.getUrlId() + "_0" + infosTab.fullTabGuid + infosTab.fullModeNavig);
        });
        $("#btnPrecedent").on("click", function () {
            common.page.isLoading = true;
            common.dom.redirect(
                "/CreationAffaireNouvelle/ChoixRisques?id=",
                [
                    "ACTION|OffreToAffaire",
                    "IPB|" + currentAffair.codeOffre,
                    "ALX|" + currentAffair.version,
                    "TYP|" + currentAffair.type,
                    "IPB|" + $("[name='Code']").val(),
                    "ALX|" + $("[name='Version']").val(),
                    "GUIDKEY|" + infosTab.fullTabGuid
                ].join("||"));
        });
    },
    submitter: null,
    lastFocused: null,
    startUpdate: function (rq) {
        common.page.isLoading = true;
        let obj = (chxForOpt.submitter || {});
        rq.setRequestHeader("submitter", obj.name || obj.id);
        setTimeout(function () {
            chxForOpt.lastFocused = $("input:focus")[0];
        }, 10);
    },
    submitDone: function () {
        if (chxForOpt.selectionIsValid()) {
            if ((chxForOpt.submitter || {}).id == chxForOpt.buttonValiderId) {
                let panel = chxForOpt.$panel().clone();
                $("#tabGuid").clone().appendTo(panel);
                panel.removeAttr("id");
                panel.removeAttr("name");
                $("#form1").append(panel);
                $("#form1").submit();
            }
            else {
                chxForOpt.$button().prop({ disabled: false });
            }
        }
        else {
            chxForOpt.$button().prop({ disabled: true });
            if (chxForOpt.lastFocused) $("#" + chxForOpt.lastFocused.id).focus();
        }
    },
    startValidate: function () {
        common.page.isLoading = true;
        chxForOpt.submitter = null;
    },
    submitValidateError: function (x, s, e) {
        common.page.isLoading = false;
        common.error.showXhr(x);
    },
    submitValidateDone: function () {
        common.$getJson("/Folder/GetFolderUrl", { code: chxForOpt.$codeContrat(), version: chxForOpt.$versionContrat, type: "P", readOnly: false, tabGuid: common.tabGuid }).done(function (url) {
            document.location.assign(url);
        }).fail(function (x) {
            common.page.isLoading = false;
            common.error.showXhr(x);
        });
    },
    submitEnded: function () {
        common.page.isLoading = false;
    },
    initAutoSubmit: function () {
        chxForOpt.$button().off("click", chxForOpt.initAutoSubmit);
        $(document).on("change", "#" + chxForOpt.panelId + " input", function (evt) {
            evt.stopPropagation();
            chxForOpt.submitter = this;
            chxForOpt.$ajaxForm().submit();
        });
        $(document).on("click", "#" + chxForOpt.panelId + " label[name='label-formule'], label[name='label-option']", function (evt) {
            evt.stopPropagation();
        });
        $(document).on("click", "#" + chxForOpt.panelId + " li", function (evt) {
            evt.stopPropagation();
            chxForOpt.submitter = this;
            chxForOpt.$ajaxForm().submit();
        });
    },
    selectionIsValid: function () {
        return ($("input[name='" + chxForOpt.viewName + "IsValid']").val() || "").toLowerCase() == true.toString();
    },
    buttonValiderId: "btnValiderFormulesOptions",
    panelId: "selectionFormulesOptions",
    viewName: "_FormulesAffaireNouvelle",
    $panel: function () {
        return $("#" + chxForOpt.panelId);
    },
    $button: function () {
        return $("#" + chxForOpt.buttonValiderId);
    },
    $ajaxForm: function () {
        return $("#ajaxForm");
    },
    $codeContrat: function() {
        return chxForOpt.$panel().find("[name='Code']").val();
    },
    $versionContrat: function () {
        return chxForOpt.$panel().find("[name='Version']").val();
    }
};
let chxForOpt = affaireNouvelle.choixFormules;
$(function () {
    chxForOpt.init();
});