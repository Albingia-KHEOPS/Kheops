
var recherche = recherche || {};
var TargetAction = function (isConsulting, isModifHorsAvenant, newProcess, target) {
    this.isConsulting = isConsulting;
    this.isModifHorsAvenant = isModifHorsAvenant;
    this.newProcess = newProcess;
    this.target = target;
};

const targets = {
    consultOrEdit: { id: 5, code: "ConsulterOuEditer" },
    doubleSaisie: { id: 7, code: "DoubleSaisie" },
    retourPieces: { id: 16, code: "RetourPieces" },
    prisePosition: { id: 47, code: "PrisePosition" },
    blocageDeblocageTermes: { id: 50, code: "BlocageDeblocageTermes" },
    get: function (x) {
        let props = Object.getOwnPropertyNames(targets);
        if (typeof x === "string") {
            let p = props.filter(function (n) { return targets[n].id == x || targets[n].code === x; })[0];
            if (p) {
                return targets[p];
            }
        }
        if (typeof x === "number") {
            let p = props.filter(function (n) { return targets[n].id === x; })[0];
            if (p) {
                return targets[p];
            }
        }
        return null;
    }
};
recherche.affaires = {
    init: function () {
        $("#btnCreerOffre, #btnFolderReadOnly").off("click");
        $(document).on("click", "#btnCreerOffre, #btnMHA, #btnFolderReadOnly", function () { recherche.affaires.results.consultOrEdit($(this)); });
    },
    results: {
        getSelected: function () {
            let selectedRadio = $("input[type='radio'][name='RadioRow']:checked");
            let array = [];
            if (!selectedRadio.exists()) {
                if ($("#ConfirmationCodeContrat").exists()) {
                    array = $("#ConfirmationCodeContrat").text().split("_");
                    return { code: array[0], version: array[1], type: "P", numAvenant: 0, radio: null };
                }
                return null;
            }
            array = selectedRadio.val().split("_");
            return { code: array[0], version: array[1], type: array[2], numAvenant: array[3], radio: selectedRadio };
        },
        getStepContext: function (offre, targetAction) {
            offre = offre || recherche.affaires.results.getSelected();
            return {
                TabGuid: window.parent.homeTabGuid,
                ModeNavig: $("#ModeNavig").val(),
                Folder: { CodeOffre: offre.code, Version: offre.version, Type: offre.type, NumeroAvenant: offre.numAvenant },
                Target: targetAction.target && targetAction.target.indexOf("blocdebloc") == 0 ? targets.blocageDeblocageTermes.code : (targetAction.target || "Edition"),
                IsReadonlyTarget: !!targetAction.isConsulting,
                IsModifHorsAvenant: !!targetAction.isModifHorsAvenant,
                NewProcessCode: targetAction.newProcess || "",
                NiveauDroitTermes: targetAction.target && targetAction.target.indexOf("blocdebloc") == 0 ? parseInt(targetAction.target.substr(10)) : 0
            };
        },
        consultOrEdit: function (button, newProcess, beforeHandleUrl) {
            let btnId = "";
            if (button instanceof jQuery) {
                btnId = button.get(0).id;
            }
            else if (typeof button === "string") {
                btnId = button;
            }
            const self = recherche.affaires;
            const offre = self.results.getSelected();
            let isConsulting = btnId == "btnFolderReadOnly" || ($("#ModeNavig").val() == "H" && (!button.attr || button.attr("albModeBtn") != "PieceSignee"));
            let targetName = !isConsulting && !btnId && !newProcess ? targets.consultOrEdit.code : null;
            if (targetName === null) {
                if (btnId.indexOf(targets.doubleSaisie.code) >= 0) {
                    targetName = targets.doubleSaisie.code;
                }
                else if (btnId.indexOf(targets.prisePosition.code) >= 0) {
                    targetName = targets.prisePosition.code;
                }
                else if (button && button.attr && button.attr("albModeBtn") == "PieceSignee") {
                    targetName = targets.retourPieces.code;
                }
            }
            let stepContext = self.results.getStepContext(offre, new TargetAction(isConsulting, btnId == "btnMHA", newProcess, targetName));
            common.page.isLoading = true;
            common.$postJson("/Redirection/Auto", stepContext, true).then(function (result) {
                if (targetName === targets.consultOrEdit.code && result.url && result.IsReadonlyTarget && !result.context.DenyReason) {
                    isConsulting = null;
                }
                if ($.isFunction(beforeHandleUrl)) {
                    beforeHandleUrl();
                }
                setTimeout(function () {
                    self.results.handle(result, offre, isConsulting);
                }, 10);
            });
        },
        edit: function (action, next) {
            let a = (action || "").trim().toLowerCase();
            switch (a) {
                case "engagementperiodes":
                case "etabliraffairenouvelle":
                case "priseposition":
                case "validation":
                    break;
                default:
                    if (a.indexOf("blocdebloc") != 0) {
                        return;
                    }
            }
            const self = recherche.affaires;
            const offre = self.results.getSelected();
            common.page.isLoading = true;
            let stepContext = self.results.getStepContext(offre, new TargetAction(false, false, null, action));
            common.$postJson("/Redirection/Auto", stepContext, true).then(function (result) {
                common.page.isLoading = false;
                if (result.url) {
                    if (result.context.IsReadonlyTarget) {
                        message = self.results.buildLockMessage(result.context, offre);
                        ShowCommonFancy("Error", "", (message || result.context.DenyReason), 200, 65, true, true);
                    }
                    else {
                        if ($.isFunction(next)) {
                            next();
                        }
                        else {
                            common.page.isLoading = true;
                            document.location.replace(result.url);
                        }
                    }
                }
                else {
                    switch (result.context.DenyReason.toLowerCase()) {
                        case "citrix":
                            common.dialog.error("Veuillez ouvrir cette affaire sous Citrix.");
                            break;
                        default:
                            common.dialog.error(result.context.DenyReason);
                    }
                }
            });
        },
        createAttestation: function () {
            const self = recherche.affaires;
            const offre = self.results.getSelected();
            let stepContext = self.results.getStepContext(offre, new TargetAction(false, false, "", "Attestation"));
            common.page.isLoading = true;
            common.$postJson("/Redirection/Auto", stepContext, true).then(function (result) {
                if (result.url) {
                    document.location.replace(result.url);
                }
                else {
                    common.page.isLoading = false;
                    switch (result.context.DenyReason.toLowerCase()) {
                        case "citrix":
                            common.dialog.error("Veuillez ouvrir cette affaire sous Citrix.");
                            break;
                        default:
                            common.dialog.error(result.context.DenyReason);
                    }
                }
            });
        },
        handle: function (result, offre, isConsulting) {
            let message = null;
            const self = recherche.affaires.results;
            if (result) {
                console.log(result);
                if (result.url) {
                    if (isConsulting === null) {
                        // consult from ConsulterOuEditer
                        try {
                            document.location.replace(result.url);
                        }
                        catch (e) {
                            common.page.isLoading = false;
                            console.error(e);
                        }
                        return;
                    }
                    if (!isConsulting && result.context.IsReadonlyTarget) {
                        common.page.isLoading = false;
                        if (result.context.SuggestNewVersion) {
                            self.showEditSuggestions(result.context, offre);
                        }
                        else {
                            message = self.buildLockMessage(result.context, offre);
                            ShowCommonFancy("Error", "", (message || result.context.DenyReason), 200, 65, true, true);
                        }
                    }
                    else {
                        if ($("#divDataRechercheRapide").attr("albQuickSearch") == "true" || $("#idBlacklistedPartenaire").val()) {
                            result.url += "newWindow";
                        }
                        if (isConsulting) {
                            message = self.buildLockMessage(result.context, offre, true);
                            if (message) {
                                common.page.isLoading = false;
                                ShowCommonFancy("Info", "url:" + result.url, message, 300, 80, true, true, true);
                                return;
                            }
                        }
                        try {
                            if (targets.get(result.context.Target) === targets.retourPieces) {
                                self.handleRetourPieces(result);
                            }
                            else {
                                common.page.replace(result.url, true);
                            }
                        } catch (e) {
                            console.error(e);
                            common.error.showMessage(e.message);
                        }
                        return;
                    }
                }
                else {
                    common.page.isLoading = false;
                    switch (result.context.DenyReason.toLowerCase()) {
                        case "citrix":
                            common.dialog.error("Veuillez ouvrir cette affaire sous Citrix.");
                            break;
                        default:
                            common.dialog.error(result.context.DenyReason);
                    }
                }
            }
            else {
                common.error.showMessage("No result found");
            }
        },
        handleRetourPieces: function (result) {
            // call ajax for Retours Pieces
            let urlParts = result.url.split("/");
            if (!urlParts[0]) {
                urlParts.shift();
            }
            let url = "/" + urlParts.shift() + "/" + urlParts.shift();
            common.$postJson(url, { id: urlParts.join("/"), modeNavig: result.context.ModeNavig }, true).done(function (data) {
                $("#dvDataFlotted").html(data).width(1050);
                common.dom.pushForward("dvDataFlotted");
                $("#dvFlotted").show();
                AlternanceLigne("RetoursCoAssBody", "", false, null);
                $("#btnAnnulerRetours").kclick(function () {
                    FermerRetours();
                });
                $("#btnSuivantRetours").kclick(function () {
                    ValidRetours();
                });
                $("#TypeAccordPreneur").offOn("change", function () {
                    $("#inDateRetour").enable();
                });
                $(".datepicker").datepicker({ dateFormat: "dd/mm/yy" });
                AffectDateFormat();

                common.page.isLoading = false;
            });
        },
        showEditSuggestions: function (context, offre) {
            if (offre.type == "O") {
                window.derniereVersion = (context.NextVersion || 1) - 1;
                switch (context.DenyReason.toLowerCase()) {
                    case "validee":
                    case "sans suite":
                        $("#divReadOnlyMsg").html("L'offre <b>" +
                            offre.code +
                            "</b> est " + context.DenyReason.replace("ee", "ée") +
                            ", elle ne peut être modifiée.<br />Voulez-vous créer une <b>nouvelle version</b> de cette offre<br/>ou l'ouvrir en <b>lecture seule</b> ?<br />Cette version portera le numéro <b>" + context.NextVersion + "</b>");
                        AlbScrollTop();
                        $("#divReadOnlyOffre").show();
                        break;
                    case "reprise":
                        $("#divReadOnlyMsg").html("La reprise de l'offre <b>" +
                            code +
                            "</b> va entraîner la création d'une version.<br />Voulez-vous confirmer <b>la création de version</b> ?");
                        $("#btnReadonly").hide();
                        AlbScrollTop();
                        $("#divReadOnlyOffre").show();
                        break;
                    default:
                        common.dialog.error(context.DenyReason);
                }
            }
        },
        buildLockMessage: function (context, offre, isWarning) {
            let message = "";
            if (context.IsUserLocking) {
                message = (isWarning ? "Attention, vous" : "Vous") + " avez déjà ouvert " + (offre.type == "O" ? "cette offre" : "ce contrat") + " pour modification";
            }
            else if (context.LockingUser) {
                message = (isWarning ? "Attention, ce" : "Ce") + (offre.type == "O" ? "tte offre" : " contrat")
                    + " est en cours de modification par l'utilisateur"
                    + " <b>" + context.LockingUser + "</b>";
            }
            return message;
        },
        consultOrEditBNS: function (beforeHandleUrl) {
            let params = common.albParam.buildObject();
            const self = recherche.affaires;
            const offre = self.results.getSelected();
            //params.AVNMODE = "CONSULT";

            params.REGULEID = 0;
            params.REGULMOD = "BNS";
            params.REGULTYP = "S";
            params.REGULNIV = "E";
            params.REGULAVN = "N";
            params.AVNID = offre.numAvenant;
            params.AVNIDEXTERNE = params.AVNID;
            params.AVNTYPE = "REGUL";

            //let newValue = common.albParam.objectToString(params);
            //let folderParts = [$("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), codeAvn];
            //let idParts = [folderParts.join("_"), $("#tabGuid").val(), common.albParam.objectToString(params, true, true), (isReadonly == "True" ? "ConsultOnly" : "")];
            let isConsulting = false;
            common.page.isLoading = true;
            try {
                let stepContext = {
                    TabGuid: window.parent.homeTabGuid,
                    //ModeNavig: params.AVNMODE === "UPDATE" ? "S" : $("#ModeNavig").val(),
                    Folder: { CodeOffre: offre.code, Version: offre.version, Type: offre.type, NumeroAvenant: offre.numAvenant },
                    Target: "EditionBNS",
                    IsReadonlyTarget: isConsulting,
                    IsModifHorsAvenant: false,
                    NewProcessCode: params.REGULEID == 0 ? params.AVNTYPE : "",
                    AvnParams: params.REGULEID == 0 ? Object.getOwnPropertyNames(params).map(function (x) { return { Key: x, Value: params[x] }; }) : []
                };

                common.$postJson("/Redirection/Auto", stepContext, true).then(function (result) {
                    if (result.url) {
                        if ($.isFunction(beforeHandleUrl)) {
                            beforeHandleUrl();
                        }
                        setTimeout(function () {
                            self.results.handle(result, offre, isConsulting);
                        }, 10);
                    }
                    else {
                        common.page.isLoading = false;
                        switch (result.context.DenyReason.toLowerCase()) {
                            case "citrix":
                                common.dialog.error("Veuillez ouvrir cette affaire sous Citrix.");
                                break;
                            default:
                                common.dialog.error(result.context.DenyReason);
                        }
                    }
                });
            }
            catch (e) {
                console.error(e);
                common.error.showMessage(e);
            }
        }
    },
    getClickedResult: function () {
        let selection = null;
        let context = $("#CodeAffaireRech").val();
        if (context) {
            selection = { ipb: $("#CodeAffaireRech").val(), alx: $("#VersionRech").val(), typ: ($("#TypeAffaireRech").val() ? $("#TypeAffaireRech").val() : "O"), avn: $("#CodeAvnRech").val() };
        }
        return selection || {};
    }
};

$(function () {
    recherche.affaires.init();
});
