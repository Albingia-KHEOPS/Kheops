const __useJQDialog = false;
var initialLoad = true;

function initAlbCommon() {
    initialLoad = true;
    BindGlobalShortcuts();

    if ($('#switchHardLog', window.parent.document).val() === '1') {
        addTabGuidHttpHeader();
    }
    goToAdmin();
    goToTestInfosSpec();
    goToExcel();
    btnDialog();
    btnErreur();
    btnWarning();
    btnInfo();
    LinkHexavia();
    ChangeHourCommon();
    AffectSaveCancel();
    MapCommonElement();
    RetourRecherche();
    RemoveReadOnlyAtt();
    ToggleBackOfficeMenu();
    MapLinkWinOpen();
    testRecupLstContrat();

    //Ecrit une fonction pour récupérer le dernier élement d'un tableau

    Array.prototype.last = function () { return this[this.length - 1]; };
    AffectDateFormat();
    SetPageTitle();
    AffectRegExpCode();
    RechrcheTransverse();
    RechercheByCodeOffre();
    MapElementWordWindow();

    $("img[name=HistoLink]").kclick(function () {
        Historique($(this).attr("id"));
    });


    //Evènement pour recevoir des messages

    function receiveMessage(event) {
        if (event.data.type == "lockError") return;
        var magneticUrl = $("#MagneticUrl").val();
        if (magneticUrl !== event.origin) {
            ShowCommonFancy("Error", "", "Erreur lors de la sauvegarde du document", 300, 80, true, true, true, true);
            return;
        }
        if ("success" === event.data) {
            ShowLoading();
            SaveCurrentClause().always(CloseLoading);
        }
        else {
            ShowCommonFancy("Error", "", "Erreur lors de la sauvegarde du document", 300, 80, true, true, true, true);
        }
    }

    window.addEventListener("message", receiveMessage, false);
    initialLoad = false;

    function BindGlobalShortcuts() {
        document.addEventListener("keydown", ManageShortcuts, { capture: true });
    }
    /**
     * @param {KeyboardEvent} e
     */
    function ManageShortcuts(e) {
        if (e.altKey && e.key && e.key.length == 1) {
            var buttons = Array.from(document.querySelectorAll("[data-accesskey='" + e.key.toLowerCase() + "'],[data-accesskey='" + e.key.toUpperCase() + "']"));
            if (buttons.length > 0) {
                console.log(buttons, "preventDefault");
                e.stopPropagation();
                e.preventDefault();
            }
            var active = buttons.filter(isOnTop);
            if (active.length > 1) {
                throw new Error("Multiple active buttons : " + active.map(function (x) { return x.id; }).join(","));
            }
            if (active.length > 0 && !active[0].disabled && !common.page.isLoading) {
                e.preventDefault();
                $(active[0]).click();
                console.log(active, "click");
            }
        }
    }

    /**
     * @param {HTMLElement} elem
     * @return {boolean}
     */
    function isOnTop(elem) {
        const elemIndex = getZIndex(elem);
        if (elemIndex == common.dom.maxVisibleZIndex()) {
            return true;
        }
        return false;
    }

    /**
    * @param {HTMLElement} elem
    * @return {number}
    */
    function getZIndex(elem) {
        let ref = elem;
        let isAuto = true;
        while (ref != null && isAuto) {
            index = window.getComputedStyle(ref).zIndex;
            isAuto = index == "auto";
            if (isAuto) {
                if (ref == document.body) {
                    index = '0';
                    break;
                }
                ref = ref.offsetParent;
            }
        }
        if (ref == null) { // not visible
            return null;
        }
        return parseInt(index);
    }
}
$(document).ready(function () {
    globalInit.push(initAlbCommon);
    initAlbCommon();
});

//---------------Afiche l'interface de visualisation de la clause
function ViualisationClause() {
    $(".lnkDetail").kclick(function (e) {
        GetFileNameDocument($("#wClauseDocType").val(), $(this).attr("id").replace('lnkDetail_', ''), false, "V");
        e.preventDefault();
    });
}

function Historique(id, OrigineAffichage) {
    ShowLoading();
    var tabId = id.split('_');
    $.ajax({
        type: "POST",
        url: "/Clausier/Historique",
        data: { rubrique: tabId[1], sousrubrique: tabId[2], sequence: tabId[3], code: tabId[4], libelle: tabId[5], date: $("#Date").val(), origineAffichage: OrigineAffichage },
        success: function (data) {
            if (OrigineAffichage == "ClauseInvalide") {
                $("#divDataHistoriqueClauseInvalide").html(data);
                $("#divHistoriqueClauseInvalide").show();
                AlternanceLigne("HistoriqueInvalideBody", "", false, null);
            }
            else {
                $("#divDataHistoriqueClause").html(data);
                $("#divHistoriqueClause").show();
                AlternanceLigne("HistoriqueBody", "", false, null);
            }
            AlbScrollTop();
            BtnClick();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function BtnClick() {
    $('#btnFermer').kclick(function () {
        $("#divDataHistoriqueClause").clearHtml();
        $("#divHistoriqueClause").hide();
        $("#divDataHistoriqueClauseInvalide").clearHtml();
        $("#divHistoriqueClauseInvalide").hide();
    });
    $("#btnContinuer").kclick(function () {
        if ($("#ContexteInvalide").val() == "") {
            $("#ContexteInvalide").addClass('requiredField');
            return false;
        }
        SauvegardeInvalide();
    });
}

function addTabGuidHttpHeader() {

    jQuery.ajaxSetup({
        beforeSend: function (xhr) {
            xhr.setRequestHeader('x-tab-guid', $('#homeTabGuid', window.parent.document).val());
        }
    });
}
//**************Map BtnFermer Word*******
function MapElementWordWindow() {
    $("#btnFermerWordStd").kclick(function () {
        var idSessionClause = $("#IdSessionClause").val();
        if (idSessionClause != "") {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/ChoixClauses/CheckSessionClause",
                data: { idSessionClause: idSessionClause },
                success: function (data) {
                    if (data === "O") {
                        common.dialog.error("Veuillez fermer votre clause ouverte dans Word");
                    }
                    else {
                        $("#MagneticIframe").innerHTML = "";
                        $("#divWordContainer").hide();
                        $("#IdSessionClause").val("");
                    }
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            $("#MagneticIframe").innerHTML = "";
            $("#divWordContainer").hide();
        }
    });
    $("#btnFermerWordCla").kclick(function () {
        $("#btnFermerWordStd").click();
    });

    $("#btnPrintWordStd").kclick(function () {
        $("#WordContainerIFrame")[0].contentWindow.PrintDocument();
    });

    $("#btnPrintWordCla").kclick(function () {
        $("#btnPrintWordStd").click();
    });

    $("#btnValidWordStd").kclick(function () {
        if (!$("#WordContainerIFrame")[0].contentWindow.SaveDocument()) {
            common.dialog.error("Erreur lors de la sauvegarde du document.");
            if ($("#DefaultDisplayDocModule").hasTrueVal() && !$("#WordContainerIFrame")[0].contentWindow.CloseWordDoc()) {
                alert("Erreur de fermeture du document word.");
            }
            CloseLoading();
        }
        else {
            common.dialog.info("Document sauvegardé");
            setTimeout(function () {
                if ($("#DefaultDisplayDocModule").hasTrueVal() && !$("#WordContainerIFrame")[0].contentWindow.CloseWordDoc()) {
                    common.dialog.error("Erreur lors de la fermeture du document.");
                }
            }, 1000);
            CloseLoading();
        }
        $("#divWordContainer").hide();

    });

    $("#btnValidWordCla").kclick(function () {
        $("#btnValidWordStd").click();
        $(this).attr("disabled", "disabled");
    });

    $("#btnValidBlocStd").kclick(function () {
        SaveBloc();
    });

    $("#btnValidBlocCla").kclick(function () {
        $("#btnValidBlocStd").click();
    });

    $("#btnSaveEnteteClause").kclick(function () {
        ShowLoading();
        SaveClauseEntete().always(CloseLoading);
    });

    $("#btnImportWordStd").kclick(function () {
        window.location.href = "/WordViewer/SaveAsWordDocument?clauseId=" + $("#wvClauseId").val();
        $("#btnFermerWordStd").click();
    });

    $("#btnImportWordCla").kclick(function () {
        $("#btnImportWordStd").click();
    });

}
////****************** Nom du fichier à ouvrir
function GetFileNameDocument(typeDoc, wOpenParam, resolu, rule, typeClause, createClause, editDocument, refreshDocLibre, contexte) {
    ShowLoading();
    if (wOpenParam != "0" || contexte) {
        if (contexte == undefined)
            contexte = "";
        const modeNavig = $("#ModeNavig").val();
        const numAvn = $("#NumAvenantPage").val();
        if (editDocument == undefined)
            editDocument = false;
        if (refreshDocLibre == undefined)
            refreshDocLibre = false;

        const paramGestionDossier = $("#Offre_CodeOffre").val() + "_" + $("#Offre_Version").val() + "_" + $("#Offre_Type").val();

        if (typeDoc === "CP" || typeDoc === "DOC" || typeDoc === "BLOC") {
            $("#divWaitGenDocument").show();
        }
        /*Modification du 2015-10-13 non modification en mode consultation : bug 1710 ==> RETOUR ARRIERE*/
        rule = rule === "M" && window.isReadonly || window.isModifHorsAvenant ? "V" : rule;
        //***************With DocViewer
        let file = "";
        let error = false;
        createClause = createClause == undefined ? "" : createClause;
        let generateReq = $.ajax({
            type: "POST",
            url: "/WordViewer/GetDocumentPath",
            data: {
                typeDoc: typeDoc, wOpenParam: wOpenParam, resolu: resolu, rule: rule, clauseLibre: (typeClause === "Libre" ? true : false), createClause: createClause,
                modeNavig: modeNavig, contexte: contexte, paramGestionDossier: paramGestionDossier, isReadOnly: window.isReadonly || window.isModifHorsAvenant, numAvenant: numAvn

            }
        }).then(function (file) {

            const fileElems = file.split("-__-");
            const fileName = fileElems[0];

            /* TODO ECM : entête clause*/
            if (typeDoc === "Clause") {
                return GetInfoClauseLibre(wOpenParam, typeClause, createClause, fileName).then(function () { return file; });
            }
            else {
                $("#dvInfoClauseLibre").hide();
                $("#vwClauseBtn").hide();
                $("#vwStandardBtn").show();
            }
            return file;
        }).then(function (file) {
            const fileElems = file.split("-__-");
            const idSessionClause = fileElems[1];
            const fileRule = fileElems[2];

            $("#IdSessionClause").val(idSessionClause);
            if (typeDoc === "DOC" && !editDocument) {
                return RefreshListeDocument(wOpenParam).then(function () { return fileRule });
            }
            else if (typeDoc === "DOC" && editDocument) {
                if (!$("#dvLstDocLibre").length) {
                    return RefreshListDocEdit().then(function () { return fileRule });
                }
            }
            else if (typeDoc === "DOC" && refreshDocLibre) {
                return RefreshListDocumentLibre(wOpenParam).then(function () { return fileRule });
            }
            return fileRule;
        }).then(function (fileRule) {
            if (typeDoc === "Clause") {
                $("#WordContainerIFrame").hide();
                $("#MagneticIframe").hide();
                //$("#IdSessionClause").val(idSessionClause);
                $("#divWordContainer").show();
                officedocumentsrules(fileRule);
            }

            $("#divWaitGenDocument").hide();

            if (typeClause == "Ajoutée") {
                $("#btnValidWordCla").show();
            }
        }).then(null, function (request) {
            common.error.showXhr(request);
            $("#divGenDocument").hide();
            $("#divWordContainer").hide();
            error = true;
        }).always(CloseLoading);
    }
}

function OpenDirectWordDocument(filePath) {
    if (filePath != undefined && filePath !== "") {
        ShowLoading();
        const fileElems = filePath.split("-__-");
        $.ajax({
            type: "POST",
            url: "/WordViewer/OpenDirectWordDocument",
            data: { documentFullPath: filePath },
            async: false,
            success: function (data) {
                file = data;
                let idSessionClause = fileElems[1];
                $("#MagneticIframe").hide();
                $("#IdSessionClause").val(idSessionClause);
                $("#WordContainerIFrame").hide();

                $("#dvInfoClauseLibre").hide();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
                $("#divGenDocument").hide();
                $("#divWordContainer").hide();
            }
        });
    }
}
//---------- Gestion des boutons de la vue Office Viewer
function officedocumentsrules(rules) {
    $("#btnValidWordCla").removeAttr("disabled");
    var typeDocuments = "";
    if (rules !== "")
        typeDocuments = rules.substr(0, 1);

    var paramRule = rules.substr(1);

    var rule = "";
    var printable = "";

    if (paramRule !== "") {
        rule = paramRule.substr(0, 1);
        printable = paramRule.substr(1, 1);
    }

    var isTypeB = typeDocuments === "B";
    var isRuleM = rule === "M";
    var isPrintable = printable === "P";

    $("#btnValidWordStd").toggle(isRuleM && !isTypeB);
    $("#btnValidWordCla").toggle(isRuleM && !isTypeB);
    $("#btnValidBlocStd").toggle(isRuleM && isTypeB);
    $("#btnValidBlocCla").toggle(isRuleM && isTypeB);
    if (isRuleM) {
        if (isTypeB) {
            $('#btnFermerWordStd').show();
        }
    }
    else {
        $("#btnSaveEnteteClause").show();
    }

    $("#btnPrintWordStd").toggle(isPrintable);
    $("#btnPrintWordCla").toggle(isPrintable);



}

//--------------------Affiche la div flottante visualisation-------------------
function dialogClauseVisualisation(id, isLibre, paramClause, origine) {
    if ($("#ModeNavig").val().toUpperCase() == 'H') {
        common.error.show("La consultation des clauses n'est pas disponible pour l'historique.\nMerci de consulter les documents en GED.");
        return;
    }
    var contexteID = $("#ContexteCibleCode option[title=" + $("#ContexteCible").val() + "]").val();
    if (isLibre == 'False') {
        GetFileNameDocument($("#wClauseDocType").val(), id, true, "V", origine);
        return;
    }
    else {
        if (origine == "PJ") {
            GetFileNameDocument($("#wClauseDocType").val(), id, false, "V", "Libre");
        }
        else {
            GetFileNameDocument($("#wClauseDocType").val(), id, false, "M", "Libre", paramClause, false, false, contexteID);
        }
        return;
    }
}

//------------Charge les données de la clause libre----------
function GetInfoClauseLibre(clauseId, clauseType, createClause, fileName) {
    var contexte = $("#ContexteCible").val();
    if ($("#divClausierRech").is(":visible"))
        contexte = "clausier";
    return $.ajax({
        type: "POST",
        url: "/ChoixClauses/GetInfoClauseLibreViewer",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeRsq: $("#CodeRisque").val(),
            clauseId: clauseId, clauseType: clauseType, createClause: createClause, etape: $("#Etapes").val(), contexte: contexte,
            isReadonly: window.isReadonly || window.isModifHorsAvenant
        }
    }).then(
        function (data) {
            $("#dvInfoClauseLibre").html(data);

            if (contexte !== "clausier") {
                $("#wvDateApplique").hide();
            }

            $("#wvClauseId").val(clauseId);
            if ($("#wvContexte").val() === "")
                $("#wvContexte").val($("#ContexteCible").val());
            $("#dvInfoClauseLibre").show();
            if (window.isReadonly || window.isModifHorsAvenant || $("#wvClauseTypeAjout").val() === "E") {
                $("#vwStandardBtn").show();
                $("#vwClauseBtn").hide();
            }
            else {
                $("#vwStandardBtn").hide();
                $("#vwClauseBtn").show();
            }
            $("#WordContainerIFrame").attr("height", "480");
            if ($("#Etape").val() === "RSQ") {
                $("#dvAppliqueA").show();
            }
            else {
                $("#WordContainerIFrame").attr("height", "530");
            }

            AffectTitleList($("#wvContexte"));
            AffectTitleList($("#wvEmplacement"));

            //#region Magnetic

            var iframe = document.getElementById("MagneticIframe");

            //#endregion

            $("#wvAppliqueA").kclick(function () {
                OpenAppliqueViewer($(this));
            });
            $("#btnValidWordStd").kclick(function () {
                var idSessionClause = $("#IdSessionClause").val();
                ShowLoading();
                if (idSessionClause != "") {
                    $.ajax({
                        type: "POST",
                        url: "/ChoixClauses/CheckSessionClause",
                        data: { idSessionClause: idSessionClause }
                    }).then(
                        function (data) {
                            if (data === "O") {
                                common.dialog.error("Veuillez fermer votre clause ouverte dans Word");
                                $("#btnValidWordCla").removeAttr("disabled");
                                $("#btnValidWordStd").removeAttr("disabled");
                            }
                            else {
                                $("#IdSessionClause").val("");
                                return SaveCurrentClause();
                            }
                        }).then(null, common.error.showXhr).always(CloseLoading);
                }
                else {
                    SaveCurrentClause().always(CloseLoading);
                }
            });

            $("#btnSaveEnteteClause").kclick(function () {
                ShowLoading();
                SaveClauseEntete().always(CloseLoading);
            });

            $("#wvFileName").val(decodeURIComponent(fileName));

            common.autonumeric.apply($("#wvOrdre"), "destroy");
            common.autonumeric.apply($("#wvOrdre"), "init", "decimal", null, null, 3, "9999.999", null);
        }
    ).then(null, common.error.showXhr).always(CloseLoading);
}
//-------------Ouvre le s'applique A--------------
function OpenAppliqueViewer(elem) {
    var position = elem.offset();
    $("#divLstObj").show();
    $("#divDataLstObj").css({ 'position': "absolute", 'top': position.top - 10 + "px", 'left': position.left + 18 + "px", 'z-index': "81", 'border': "1px solid black" });
    $("input[id='check_" + $("#codeObjViewer").val() + "']").attr("checked", "checked");
    $("#btnListCancel").kclick(function () {
        $("#divLstObj").hide();
        $("#WordContainerIFrame").show();
    });
    $("#btnListValid").kclick(function () {
        ValidRsqObjChecked();
        $("#divLstObj").hide();
        $("#WordContainerIFrame").show();
    });

    $("#WordContainerIFrame").hide();

}

//---Affecte un titra à la page-------
function SetPageTitle() {
    //correction retour écran de recherche vider le codeAffaire en mémoire (bug 2176)
    var ecranRecherche = ["RechercheSaisie"];
    var path = window.location.pathname.split("/")[1];
    if ($.inArray(path, ecranRecherche) > -1) {
        $("#Offre_CodeOffre").val("");
    }

    var codeOffre = $("#Offre_CodeOffre").val();
    var title;

    if (codeOffre !== "" && window.location.pathname !== "/RechercheSaisie/Index") {
        title = "[" + $("#EnvironmentName").val() + "-" + codeOffre + "]-Outil de production-[" + $("#NameUser").val() + "]";
    }
    else {
        title = "[" + $("#EnvironmentName").val() + "]-Outil de production-[" + $("#NameUser").val() + "]";
    }
    if (IsInIframe())
        window.parent.document.title = title;
    else
        window.document.location.href = window.document.location.protocol + "//" + window.document.location.hostname;
}

//---------Affecte le formatage des dates-----------
function AffectDateFormat() {
    $(".datepicker").each(function () {
        $(this).change(function () {
            if ($(this).val() !== "")
                FormatAutoDate($(this));
        });
    });
}
//--------------ToggleMenu BackOffice----------------
function ToggleBackOfficeMenu() {

    $("#BackOfficeMenu").click(function () {
        var position = $("#BackOfficeMenu").offset();
        $("#LayoutArbre").css({ 'position': "absolute", 'top': position.top + 35 + "px", 'left': position.left + "px" }).toggle("blind");
    });
}
//------------------------Tester si on est dans une IFRAME----------------------
function IsInIframe() {
    return (window.parent === window.top);
}
//-------------------------Enlever l'attribut ReadOnly des boutons Annuler et suivant----------------
function RemoveReadOnlyAtt() {
    $("#btnSuivant").enable();
    $("#btnCancel").enable();
    $("#btnAnnuler").enable();
    $("#btnConfirmOk").enable();
    $("#btnConfirmCancel").enable();
    $("#btnErrorOk").enable();
    $("#idRechercheDossier").enable();
    $("#btnConfirmRechercheSaisieCancel").enable();
    $("#btnConfirmRechercheSaisieOk").enable();

}

//--------------------------------------Retour à l'écran de recherche---------------------------------------------------
function RetourRecherche() {
    $("#linkAccueil").kclick(function () {
        //Vérification que l'utilisateur ne soit pas sur les écrans de la création d'une affaire nouvelle
        //blocage si tel est le cas.
        ShowLoading();
        var ecranAffNouv = ["RsqObjAffNouv", "FormVolAffNouv", "OptTarAffNouv"];
        var ecranNoControl = ["ATTES"];
        var path = window.location.pathname.split("/")[1];
        if ($.inArray(path, ecranAffNouv) > -1)
            return;

        if ($("#OffreReadOnly").val() == undefined)
            return;
        var tabGuid = $("#tabGuid").val();
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeAvt = $("#NumAvenantPage").val();
        if (codeOffre == undefined || codeOffre === "" || version == undefined || version === "" || type == undefined || type === "" || codeAvt == undefined || codeAvt === "") { //Mise en commentaire à cause du mode readonly avt || $("#OffreReadOnly").val() === "True") {
            RedirectToAction("Index", "RechercheSaisie");
            return;
        }

        if ($.inArray($("#ActeGestion").val(), ecranNoControl) > -1 || $("#OffreReadOnly").val() === "True") {
            DeverouillerUserOffres(tabGuid);
            RedirectToAction("Index", "RechercheSaisie");
            return;
        }

        $.ajax({

            type: "POST",
            url: "/CommonNavigation/CheckTraceAssiette",
            data: { codeOffre: codeOffre, version: version, type: type },
            success: function (data) {
                if (data === "True") {
                    if (type === "O")
                        RedirectToAction("Index", "Cotisations", codeOffre + "_" + version + "_" + type + tabGuid);
                    if (type === "P") {
                        if ($("#ActeGestion").val() === "AVNMD" || $("#ActeGestion").val() === "AVNRM")
                            RedirectToAction("Index", "Quittance", codeOffre + "_" + version + "_" + type + "_" + codeAvt + tabGuid + "addParam" + $("#AddParamType").val() + "|||" + $("#AddParamValue").val() + "addParam");
                        else
                            RedirectToAction("Index", "Quittance", codeOffre + "_" + version + "_" + type + "_" + codeAvt + tabGuid);
                    }
                    return;
                }
                else {
                    DeverouillerUserOffres(tabGuid);
                    RedirectToAction("Index", "RechercheSaisie");
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//-------- Appel de Dévérrouille user
function DeverouillerUserOffres(tabGuid, next) {
    $.ajax({
        type: "POST",
        url: "/OffresVerrouillees/DeverouillerUserOffre",
        data: { tabGuid: tabGuid },
        async: false,
        success: function () {
            if (typeof next === "function") {
                next();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Mapping des éléments-----------
function MapCommonElement() {
    /****     TEST AFFICHAGE BANDEAU A LA DEMANDE     ****/

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var modeNavig = $("#ModeNavig").val();
    var codeAvt = $("#NumAvenantPage").val();
    var id = codeOffre + '_' + version + '_' + type + '_' + codeAvt;


    if (initialLoad == true) {
        $("#divInfoBandeau").empty();
    }

    $("#divBehindBandeau").kclick(function () {
        if ($("#divBehindBandeau").is(":visible")) {
            $("#divInfoBandeau").hide();
            $("#divBehindBandeau").hide();
        }
    });

    $("#imgInfoBandeau").kclick(function () {
        var position = $(this).offset();
        var divWidth = $("#divInfoBandeau").width();

        affaire.showInfosPopup(
            $("#Offre_CodeOffre").val(),
            $("#Offre_Version").val(),
            $("#Offre_Type").val(),
            ($("#ModeNavig").val() == "H" ? $("#NumAvenantPage").val() : null),
            false,
            //function (result) {
            ///**
            // * @type{HTMLElement[]}
            // * */
            //const elements = common.$removeEnclosingDiv($.parseHTML(result));
            //return Array.from(elements).reduce(function (a, e) { return a + e.outerHTML; }, "");
            null,//},
            MapLinkWinOpen,
            false
        );
    });

    $("span[name='linkNavigationArbre']").kclick(function () {
        let tabGuid = "";
        let modeNavig = "";
        let modeNavigContrat = $(this).attr("albparamarbremodenavig");
        let addParamType = $("#AddParamType").val();
        let addParamValue = $("#AddParamValue").val();
        let isModeConsultationEcran = $("#IsModeConsultationEcran").val();
        let lastValidNumAvn = $(this).attr("albParamArbreLastValidNumAVN");
        let lastValidRegulId = $(this).attr("albParamArbreLastValidRgId");
        let isOffreReadonly = $("#IsModifHorsAvn").val() == "True" ? "True"
            : $("#IsForceReadOnly").val() === "True" ? "False" : $("#OffreReadOnly").val();
        // Remplacer les lastValid... dans param
        if (lastValidNumAvn != undefined && lastValidNumAvn != "") {
            addParamValue = addParamValue.replace(new RegExp(/AVNID\|[0-9]+/gi), "AVNID|" + lastValidNumAvn);
            addParamValue = addParamValue.replace(new RegExp(/AVNIDEXTERNE\|[0-9]+/gi), "AVNIDEXTERNE|" + lastValidNumAvn);
            //addParamValue.replace("AVNIDEXTERNE|\d*|", "||AVNIDEXTERNE|" + lastValidNumAvn + "|");
        }
        if (lastValidRegulId != undefined && lastValidRegulId != "0") {
            addParamValue = addParamValue.replace(new RegExp(/REGULEID\|[0-9]+/gi), "REGULEID|" + lastValidRegulId);
        }
        if ($(this).attr('albparamarbrenonregul')) {
            addParamValue = addParamValue.replace(new RegExp(/\|\|ACTEGESTIONREGULE\|REGUL/gi), "||ACTEGESTIONREGULE|AVNMD");
        }
        if (typeof addParamValue === "string"
            && (addParamValue.indexOf("AVNMODE|CREATE|") == 0 || addParamValue.indexOf("||AVNMODE|CREATE|") > 0)
            && isOffreReadonly == "True") {

            addParamValue = addParamValue.replace(/AVNMODE\|CREATE\|/gi, "AVNMODE|CONSULT|");
        }

        var addParamString = "";
        if (addParamType != undefined && addParamType != "" && addParamValue != undefined && addParamValue != "") {
            addParamString = "addParam" + addParamType + "|||" + addParamValue + ((isModeConsultationEcran === "False" && isOffreReadonly === "False") ? "||IGNOREREADONLY|1" : "") + "addParam";
        }
        if ($("#tabGuid").val() != undefined && $("#tabGuid").val() !== "")
            tabGuid = $("#tabGuid").val();
        if ($("#ModeNavig").val() != undefined && $("#ModeNavig").val() !== "") {
            if (modeNavigContrat !== $("#ModeNavig").val() && modeNavigContrat != undefined && modeNavigContrat != "") {
                modeNavig = GetFormatModeNavig(modeNavigContrat);
            }
            else {
                modeNavig = GetFormatModeNavig($("#ModeNavig").val());
            }
        }
        else {
            modeNavig = GetFormatModeNavig("S");
        }

        var param = $(this).attr("albParamArbre") + tabGuid + addParamString + modeNavig;
        var openNewWindow = false;
        var consultOnly = false;

        if ($(this).attr("albParamArbreConsultOnly") != undefined && $(this).attr("albParamArbreConsultOnly") === "true") {
            consultOnly = true;
        }
        if ($(this).attr("albParamArbreNewWindow") != undefined && $(this).attr("albParamArbreNewWindow") === "true") {
            openNewWindow = true;
        }
        CommonSaveRedirect(param, openNewWindow, consultOnly);
    });

    $("span[name='linkNavigationArbreRegul'].navig").kclick(function (index, e) {
        let tabGuid = "";
        let modeNavig = "";
        let addParamType = $("#AddParamType").val();
        let addParamValue = $("#AddParamValue").val();
        let isModeConsultationEcran = $("#IsModeConsultationEcran").val();
        let isOffreReadonly = $("#IsForceReadOnly").val() === "True" ? "False" : $("#OffreReadOnly").val();
        let addParamString = "addParam" + addParamType + "|||" + addParamValue + ((isModeConsultationEcran === "False" && isOffreReadonly === "False") ? "||IGNOREREADONLY|1" : "") + "addParam";
        if ($("#tabGuid").val() != undefined && $("#tabGuid").val() !== "")
            tabGuid = $("#tabGuid").val();
        if ($("#ModeNavig").val() != undefined && $("#ModeNavig").val() !== "")
            modeNavig = GetFormatModeNavig($("#ModeNavig").val());
        let param = $(this).attr("albParamArbre") + tabGuid + addParamString + modeNavig;
        CommonSaveRedirectRegul(param);
    });

    $("span[name='linkNavigationArbreBO']").kclick(function () {
        var attr = $(this).attr("albParamArbreBO");
        if (attr === "SuppressionOffre") {
            OpenDivSupprOffre();
        }
        else {
            var tabGuid = "";
            if ($("#tabGuid").val() != undefined && $("#tabGuid").val() !== "") {
                tabGuid = $("#tabGuid").val();
            }
            if ($("#ModeNavig").val() != undefined && $("#ModeNavig").val() !== "") {
                modeNavig = GetFormatModeNavig($("#ModeNavig").val());
            }
            var param = $(this).attr("albParamArbreBO") + tabGuid;
            CommonRedirect(param);
        }
    });

    $(".SubGroupTitle").kclick(function () { CollapseExpandMenu($(this).attr("id")); });
    $("#imgOffreLocked").kclick(function () { OpenLockedOffre(); });
    $("#imgGestDoc").kclick(function () {
        OpenSuiviDocuments(common.page.controller.toLowerCase() == "recherchesaisie");
    });

    if (CheckExistInput($("#tabGuid")))
        $("#tabGuid").removeAttr("disabled");
    if (CheckExistInput($("#homeTabGuid", window.parent.document)))
        $("#homeTabGuid", window.parent.document).removeAttr("disabled");

    $("#btnGestionDoc").kclick(function () {
        OpenGestionDoc("GEN");
    });
}

//-----------Réduit/Agrandit la div de recherche-----------
function CollapseExpandMenu(currentId) {
    var id = currentId;
    var img = $("img[id=" + id.replace("div", "img") + "]");
    var ellipsis = $("div[id=" + id.replace("Title", "Ellipsis") + "]");
    var div = $("div[id=" + id.replace("Title", "Menu") + "]");

    if ($("#" + currentId).attr("title") === "Agrandir") {
        img.attr("src", "/Content/Images/expand-red.png");
        $("#" + currentId).attr("title", "Réduire");
    }
    else {
        img.attr("src", "/Content/Images/expand-red-right.png");
        $("#" + currentId).attr("title", "Agrandir");
    }
    div.toggle();
    ellipsis.toggle();

    if ($("#divSearchCriteria").height() > 442) {
        $(".nDivSearch").width(1170);
    }
    else {
        $(".nDivSearch").width(1194);
    }

    if ($("#divDataRechercheContrat") !== "undefined") {
        var diff = $("#divSearchCriteria").height() - 288;
        $("#divDataRechercheContrat").height(410 + diff);
    }
}
//------------Ouvre l'alerte sélectionnée-------
function OpenAlerte(elem) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var typeAlert = elem.attr("albParam");
    switch (typeAlert) {
        case "SUSPENS":
            $.ajax({
                type: "POST",
                url: "/Suspension/OpenVisualisationSuspension",
                data: { codeOffre: codeOffre, version: version, type: type, numAve: $("#NumAvenantPage").val() },
                success: function (data) {
                    $("#divDataVisuSuspension").html(data);
                    AlbScrollTop();
                    $("#divFullScreenVisuSuspension").show();
                    MapElementsVisualisationSuspension();
                    $("input[id='inputAlerte" + typeAlert + "']").val("");
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
            break;
        case "IMPAYES":
            $.ajax({
                type: "POST",
                url: "/Quittance/OpenVisualisationQuittances",
                data: { codeOffre: codeOffre, codeAvn: $("#NumAvenantPage").val(), version: version, isEntete: true, typeQuittances: typeAlert, modeNavig: $("#ModeNavig").val() },
                success: function (data) {
                    //desactivation des raccourcis claviers de l'écran appelant
                    DesactivateShortCut();
                    $("#divDataVisuListeQuittances").html(data);
                    AlbScrollTop();
                    common.dom.pushForward("divDataVisuListeQuittances");
                    $("#divVisuListeQuittances").show();
                    MapElementVisuQuittances();
                    $("input[id='inputAlerte" + typeAlert + "']").val("");
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
            break;

        case "ENGAGCNX":
            var forceReadonly = $("#IsForceReadOnly").val();
            var typeAffichage = $("#typAffichage").val();
            var codeAvn = $("#NumAvenantPage").val();
            if (forceReadonly == undefined)
                forceReadonly = "false";
            id = codeOffre + "_" + version + "_" + type + "_" + codeAvn;
            /* récupération des infos du bandeau recherche */
            if (codeOffre == "" || version == "") {
                codeOffre = $("#CodeAffaireRech").val();
                version = $("#VersionRech").val();
                type = $("#TypeAffaireRech").val();
                codeAvn = $("#CodeAvnRech").val();
                id = codeOffre + "_" + version + "_" + type + "_" + codeAvn;
            }
            $.ajax({
                type: "POST",
                url: "/CommonNavigation/LoadConnexites",
                data: { id: id, forceReadonly: forceReadonly },
                success: function (data) {
                    DesactivateShortCut();
                    $("#divDataConnexites").html(data);
                    AlbScrollTop();
                    $("#divConnexites").show();
                    MapElementConnexites();
                    if (typeAffichage == undefined || typeAffichage == "") {
                        $("#commentairesEng").html($("#commentairesEng").html().replace(/&lt;br&gt;/ig, "\n"));
                        toggleDescription($("#commentairesEng"));
                    }
                    else {
                        $("#commentairesEngagement").html($("#commentairesEngagement").html().replace(/&lt;br&gt;/ig, "\n"));
                        toggleDescription($("#commentairesEngagement"));
                    }

                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
            break;
        default:
            break;
    }
}

//------------------------------ Afficher la div transverse de recherche par code
function RechrcheTransverse() {
    $("#btnQuickSearch").bind("click", function () {
        if ($("#divQuickSearch").is(":visible")) {
            $("#divQuickSearch").hide();
        } else {
            var position = $(this).offset();
            var top = position.top;

            if ($("#divQuickSearch").is(":visible")) {
                top = top - 20;
            }
            $("#divQuickSearch").css({ 'position': "absolute", 'top': top + 3 + "px", 'left': position.left + 40 + "px" }).show();

        }
    });

}

//---------------------------Rechercher par Code------
function RechercheByCodeOffre() {
    $("#btnSearchTransverse").kclick(function () {
        QuickSearchResult(false);
    });
    $("#detailsRecherche").kclick(function () {
        QuickSearchResult(true);
    });
}
//----------Appel du resultat de la recherche
function QuickSearchResult(searchWithDetails) {
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/RechercheParCodeDossier",
        data: { codeDossier: $("#idRechercheDossier").val().toUpperCase(), searchWithDetails: searchWithDetails },
        success: function (data) {
            AlbScrollTop();
            DesactivateShortCut();
            $("#divRechercheRapide").show();
            $("#divDataRechercheRapide").html(data).attr("albQuickSearch", "true");
            if ($("#ResultCount").val() === "0") {
                $("#divResultatsBody").html("&nbsp&nbsp<b>Aucun résultat à votre recherche</b>");
            }
            AlbScrollTop();
            $("#RechercherResultContainerDiv").removeClass("notDisplayed");
            AddOnClickButton();
            AddOnClickRadio();
            AddOnClickRow();
            AddOnHideBandeau();
            RemoveReadOnlyAtt();

            //vérifie le nombre de ligne retournée
            CheckNbRowResult();
            MapSearchElement();
            AlternanceLigne("ResultatsBody", "noInput", true, null);
            contextMenu.createMenuNouveau("", "", "");
            MapCommonAutoCompActionMenu();
            CloseQuickResultResearch();
            CloseLoading();


            if (!searchWithDetails) {//Si recherche rapide
                MapElementPageRecherche();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
    CloseLoading();
}
//------------Sauvegarde et quitte-----------
function AffectSaveCancel() {
    $("#btnSaveCancel.CursorPointer").kclick(function () {
        let ecranUpdate = ["Engagements", "EngagementTraite", "DetailCalculSMP", "AttentatGareat", "Cotisations"];
        let ecranRegule = ["CreationRegularisation", "Regularisation"];
        let path = window.location.pathname.split("/")[1];
        $("#txtSaveCancel").val("1");
        if ($.inArray(path, ecranUpdate) > -1) {
            $("#btnRefresh").click();
            DeverouillerUserOffres(common.tabGuid);
        }
        else {
            if ($.inArray(path, ecranRegule) > -1) {
                saveAndQuitRegul();
            }
            else {
                if ($("#btnSuivant").exists()) {
                    $("#btnSuivant").trigger("click", { returnHome: true });
                    return;
                }
                DeverouillerUserOffres(common.tabGuid);
                $("#txtSaveCancel").val("0");
            }
        }
    });
}

function DisableBackButton() {
    var push = function () { history.pushState(null, document.title, location.href); };
    push();
    window.addEventListener('popstate', function (event) {
        push();
    });
}
//--------Common Redirect To--------------------------
function RedirectToAction(action, control, id) {
    var newWindow = "";
    if ($("#divDataRechercheRapide").attr("albQuickSearch") === "true") {
        newWindow = "newWindow";
    } else {
        ShowLoading();
    }
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/RedirectTo/",
        data: { action: action, control: control, id: (id === "" || id == undefined) ? "" : id + newWindow },
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function CommonGenericRedirect(param, isReadonly, openNewWindow) {
    var newWindow = "";
    if ($("#divDataRechercheRapide").attr("albQuickSearch") === "true" || $("#idBlacklistedPartenaire").val()) {
        newWindow = "newWindow";
        $("#txtParamRedirect").clear();
    } else {
        ShowLoading();
    }

    if (openNewWindow === true) {
        $("#txtParamRedirect").clear();
        newWindow = "newWindow";
    }

    if (isReadonly == undefined) {
        isReadonly = $("#OffreReadOnly").val();
    }

    $.ajax({
        type: "POST",
        url: "/CommonNavigation/CommonRedirectTo",
        data: { url: param + newWindow, isReadonly: isReadonly },
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------Common Redirect-----------
function CommonRedirect(param, isReadonly) {
    //BUG 3368: Lien de navigation {Cotisation; Fin} en regularisation: consultation
    var pathDest = param.split("/");
    if (pathDest[0] == "CreationRegularisation" && pathDest[1] == "Step1_ChoixPeriode_FromNavig_Consult" && pathDest[2].indexOf("ACTEGESTIONREGULE|AVNMD") !== -1 && pathDest[2].indexOf("AVNMODE|CONSULT") !== -1) {
        $("#ActeGestionRegule").val("REGUL");
        param = param.replace("ACTEGESTIONREGULE|AVNMD", "ACTEGESTIONREGULE|REGUL");
    }
    CommonGenericRedirect(param, isReadonly, false);
}

//--------Common Redirect in new window -----------
function CommonRedirectNewWindow(param, isReadonly, openNewWindow) {
    //BUG 3368: Lien de navigation {Cotisation; Fin} en regularisation: consultation
    var pathDest = param.split("/");
    if (/AvenantInfoGenerales|AnInformationsGenerales/.test(pathDest[0])
        && pathDest[1] == "Index"
        && pathDest[2].indexOf("ACTEGESTIONREGULE|REGUL") !== -1
        && pathDest[2].indexOf("AVNMODE|CONSULT") !== -1
    ) {
        $("#ActeGestionRegule").val("AVNMD");
        param = param.replace("ACTEGESTIONREGULE|REGUL", "ACTEGESTIONREGULE|AVNMD");
    }

    CommonGenericRedirect(param, isReadonly, openNewWindow);
}

//-----------Common Save Redirect------------
function CommonSaveRedirect(param, openNewWindow, consultOnly) {
    var ecranUpdate = ["Engagements", "EngagementTraite", "DetailCalculSMP", "AttentatGareat", "Cotisations"];
    var ecranNeedSave = ["ConditionsGarantie"];
    var ecranRisque = ["DetailsObjetRisque", "InformationsSpecifiquesObjets", "RisqueInventaire"];
    var ecranFin = ["ConfirmationSaisie"];
    var ecranDblSaisie = ["DoubleSaisie"];
    var ecranRegule = ["CreationRegularisation", "CreationRegularisationGarantie"];
    var path = window.location.pathname.split("/")[1];
    var pathDest = param.split("/")[0];
    var pConsultOnly = "";
    if (consultOnly === true) {
        pConsultOnly = "ConsultOnly";
    }

    if ($("#ActeGestion").val() === "AVNRG") {
        if (pathDest === "CreationRegularisation") {
            $("#ActeGestionRegule").val("REGUL");
            param = param.replace("ACTEGESTIONREGULE|AVNMD", "ACTEGESTIONREGULE|REGUL");
            param = param.replace("AVNMODE|CREATE", "AVNMODE|UPDATE");
        }
        else {
            $("#ActeGestionRegule").val("AVNMD");
            param = param.replace("ACTEGESTIONREGULE|REGUL", "ACTEGESTIONREGULE|AVNMD");
        }
    }

    if (pathDest === "CreationAvenant") {
        param = param.replace("AVNMODE|CREATE", "AVNMODE|UPDATE");
    }

    $("#txtParamRedirect").val(param);

    if ($.inArray(path, ecranRisque) > -1 && ($("#IsModeCreationRisque").attr("id") == undefined || $("#IsModeCreationRisque").val() === "N")) {//Redirection forcée pour écran qui découle d'un risque (calculs)
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeRisque = $("#CodeRisque").val();
        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        var addParamString = "";
        if (addParamType) {
            addParamString = "addParam" + addParamType + "|||" + addParamValue + "addParam";
        }

        if (modeNavig) {
            modeNavig = GetFormatModeNavig(modeNavig);
        }

        $("#txtParamRedirect").val("DetailsRisque/Index/" + codeOffre + "_" + version + "_" + type + "_" + codeRisque + tabGuid + addParamString + modeNavig);
    }
    if (($("#OffreReadOnly").val() === "True" || $.inArray(path, ecranRegule) > -1) && ($("#OffreReadOnly").val() === "True" && ($("#IsModifHorsAvn").val() === "False"))) {
        if (openNewWindow === true) {
            CommonRedirectNewWindow(param + pConsultOnly, false, openNewWindow);
        }
        else {
            CommonRedirect(param + pConsultOnly);
        }
    }
    else {

        if ($("#btnSuivant").is(":disabled")) {
            if ($.inArray(path, ecranUpdate) > -1) {
                $("#btnRefresh").click();
            }
            else if ($.inArray(path, ecranFin) > -1) {
                $("#btnFin").click();
            }
            else if ($.inArray(path, ecranNeedSave) > -1) {
                common.dialog.error("Vous avez des modifications en cours, <br/>veuillez sauvergarder ou annuler<br/>celles-ci.");
            }
            else {
                CommonRedirect(param + pConsultOnly);
            }
        }
        else {
            if ($.inArray(path, ecranDblSaisie) > -1) {
                if ($("#DblSaisieChangeInProgress").val() === 1)
                    $("#ValiderBtnDbl").click();
                else
                    $("#btnAnnulerDbl").click();
            }
            else {
                if (path === "ValidationOffre") {
                    CommonRedirect(param + pConsultOnly);
                } else {
                    $("#btnSuivant").click();
                }
            }
        }
    }

    $("#btnValidConfirm").click();
}

function CommonSaveRedirectRegul(param, returnHome) {
    let url = decodeURIComponent(window.location.pathname);
    let action = url.split("/")[2];
    let actionsToSave = ["Step1_ChoixPeriode", "Step5_RegulContrat"];
    let controllerToRedirect = param.split("/")[0];
    let actionToRedirect = param.split("/")[1];
    let isReadOnly = $("#OffreReadOnly").val() === "True";
    let p = common.albParam.buildObject();

    if (isReadOnly || $.inArray(action, actionsToSave) == -1) {
        switch (actionToRedirect) {
            case 'Step1_ChoixPeriode':
            case 'Step2_ChoixRisque':
                if (p.RSQID) {
                    delete p.RSQID;
                }
                break;
            case 'Step3_ChoixGarantie':
                if (p.GARID) {
                    delete p.GARID;
                }
                if (p.REGULGARID) {
                    delete p.REGULGARID;
                }
                break;
            case 'Step4_ChoixPeriodeGarantie':
                if (p.REGULGARID) {
                    delete p.REGULGARID;
                }
                break;
        }

        $('#AddParamValue').val(common.albParam.objectToString(p));
        RedirectionRegul(actionToRedirect, controllerToRedirect, returnHome);
    }
    else {
        $('#txtParamRedirect').val(controllerToRedirect + '/' + actionToRedirect);
        $('#btnReguleSuivant').click();
    }
}

function RedirectionRegul(action, controller, returnHome, navAction) {
    common.page.isLoading = true;
    const data = {
        action: action, controller: controller,
        navAction: navAction, returnHome: returnHome,
        codeAffaire: $("#Offre_CodeOffre").val(),
        version: $("#Offre_Version").val(),
        type: $("#Offre_Type").val(),
        numeroAvenant: $("#NumAvenantPage").val(),
        tabGuid: common.tabGuid,
        addParam: $("#AddParamValue").val(),
        modeNavig: $("#ModeNavig").val()
    };
    common.$postJson("/CreationRegularisation/RedirectionRegul", data, true);
}

function RedirectionReguleOuter(cible, job, elem) {
    ShowLoading();
    var addParamValue = $('#AddParamValue').val();
    var typeAvtPipe = addParamValue.split('||').filter(function (element) {
        return element.indexOf("AVNTYPE|") === 0 ? element : '';
    })[0];

    var typAvt = typeAvtPipe.split('|')[1];
    var codeContrat = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvenant = $("#NumInterne").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var codeAvenantExterne = $("#NumAvt").val();
    var lotID = $("#lotId").val();
    var reguleId = $("#ReguleId").val();
    var codeRsq = $("#CodeRsq").val();
    var dateDebReg = $("#inDateDebRsqRegule").val();
    var dateFinReg = $("#inDateFinRsqRegule").val();
    var codeFor = "";
    var codeOpt = "";
    var idGar = "";
    var codeGAr = "";
    var libgar = "";
    var isReadonly = $("#OffreReadOnly").val() == "True" ? true : false;
    if (elem != undefined && elem != null) {
        codeFor = elem.attr("albcodefor");
        codeOpt = elem.attr("albcodeopt");
        idGar = elem.attr("albidgar");
        codeGAr = elem.attr("albcodgar");
        libgar = elem.attr("alblibgar");
    }

    var isSaveAndQuit = $("#txtSaveCancel").val() == "1";

    $.ajax({
        type: "POST",
        url: "/CreationRegularisation/Redirection/",
        data: {
            cible: cible, job: job, codeContrat: codeContrat, version: version, type: type, codeAvenant: codeAvenant, tabGuid: tabGuid,
            codeRsq: codeRsq, codeFor: codeFor, codeOpt: codeOpt, idGar: idGar, lotID: lotID, reguleId: reguleId, codeGAr: codeGAr, libgar: libgar,
            addParamType: addParamType, addParamValue: addParamValue, codeAvenantExterne: codeAvenantExterne, modeNavig: $("#ModeNavig").val(), typAvt: typAvt,
            acteGestionRegule: $("#ActeGestionRegule").val(), dateDebReg: dateDebReg, dateFinReg: dateFinReg, isReadonly: isReadonly, isSaveAndQuit: isSaveAndQuit
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-----------OUverture de la double saisie en div flottante----------
function OpenDblSaisie(param, codeAvn) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/OpenDblSaisie",
        data: { id: param, codeAvn: codeAvn },
        success: function (data) {
            $("#DFDblSaisie").html(data);
            AlbScrollTop();
            $("#divComDblSaisie").show();
            MapPageElementDblSaisie();
            MapCommonAutoCompCourtier();
            MapCommonAutoCompSouscripteur();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//---------------Fermeture de la fancy Dialog---------------
function btnDialog() {
    $("#btnDialogOk").kclick(function () {
        $("#divDialogInFancy").hide();
    });
}
//----------------------Ouvre la page Hexavia----------------------
function LinkHexavia() {
    $("div[name='btnAdresse']").die().on("click", function () {
        if ($(this).hasClass("CursorPointer")) {
            OpenHexavia($(this).attr("albcontext"));
        }
    }).on("mouseover", function () {
        var context = $(this).attr("albContext");
        var position = $(this).offset();
        var top = position.top;
        if ($("div[name=divErrorHexavia][albContext=" + context + "]").is(":visible")) {
            top = top - 20;
        }
        $("div[name=divInfoAdr][albContext=" + context + "]").css({ 'position': "absolute", 'top': top - 20 + "px", 'left': position.left + 45 + "px" }).show();
    }).on("mouseout", function () {
        var context = $(this).attr("albContext");
        $("div[name=divInfoAdr][albContext=" + context + "]").hide();
    });
} //---------------Fermeture de la fancyBox "Error".---------------------
function btnErreur() {
    $("#btnErrorOk").kclick(function () {
        CloseCommonFancy();
    });
}
function btnWarning() {
    $("#btnWarningOk").kclick(function () {
        CloseCommonFancy();
    });
}
function btnInfo() {
    $("#btnInfoOk").kclick(function () {
        CloseCommonFancy();
    });
}
//---------------Map Elements Page--------------------
function goToAdmin() {
    $("#btnAdmin").kclick(function () {
        common.page.isLoading = true;
        document.location.href = "/BackOffice/Index";
    });
}

function goToTestInfosSpec() {
    $("#btnTestInfosSpec").kclick(function () {
        common.page.isLoading = true;
        ShowLoading();
        document.location.href = "/ParamISModeleAssocier/Index";
    });
}


function goToExcel() {
    $("#btnExcelContrat").kclick(function () {
        common.page.isLoading = true;
        document.location.href = "/ExcelContrat/Index";
    });
}
//------------------Stopper la propagation  de F5--------------------------
// NLE-JS : à compléter pour les différents navigateurs
function StopRefresh() {
    window.onbeforeunload = function (e) {
        e = e || window.event;
        // For IE and Firefox
        if (e) {
            PreventRefresh(e);
        }
    };
}
// NLE-JS : à compléter pour les différents navigateurs
function PreventRefresh(e) {

    switch (navigator.appName) {
        case "Netscape":
            if (e.target.activeElement.id !== "linkAccueil") {
                e.returnValue = "Exit";
            }
            break;
        case "Microsoft Internet Explorer":
            if (e.target.document.activeElement.id !== "linkAccueil") {
                e.returnValue = "";
            }
            break;
        default:
            break;
    }

}
//------------------Change le title d'une combobox--------------------------
function ChangeDropDownTitle(elem) {
    var idElem = elem.attr("id");
    var valElem = elem.val();

    var newTitle = $("#" + idElem + valElem).text();

    elem.attr("title", newTitle);
}
//---------------------Ecrit l'heure dans le champ caché-------------------------------
function SetHours(inputId) {
    var newHour = $("#" + inputId + "Hours").val();
    var newMinute = $("#" + inputId + "Minutes").val();

    if (newHour === "" || newMinute === "") {
        $("#" + inputId).val("");
        return false;
    }
    else {
        $("#" + inputId).val(newHour + ":" + newMinute + ":00");
        return true;
    }
}

//--------------------- Conversion chaine/int en date
function convertToDate(strDate) {
    if (strDate === null)
        return "";
    var currVal = strDate;
    if (currVal == "")
        return "";
    var dateRegExp = /^(\d{8})$/;
    var dtArray = currVal.match(dateRegExp);

    if (dtArray == null)
        return "";

    var year = currVal.substring(0, 4);
    var month = currVal.substring(4, 6);
    var day = currVal.substring(6, 8);
    return day + "/" + month + "/" + year;
}

//---------------------Vérifie que la date de début et de fin-----------------------------
function checkDate(dateDeb, dateFin) {
    if (dateDeb.val() !== "" || dateFin.val() !== "") {
        if (!isDate(dateDeb.val()) || !isDate(dateFin.val())) {
            dateDeb.addClass("requiredField");
            dateFin.addClass("requiredField");
            return false;
        }
    }
    if (getDate(dateDeb.val()) > getDate(dateFin.val())) {
        dateDeb.addClass("requiredField");
        dateFin.addClass("requiredField");
        return false;
    }
    return true;
}

//---------------------Vérifie que la date de début et de fin-----------------------------
function checkDateOrEqual(dateDeb, dateFin) {
    if (dateDeb.val() === "" || dateFin.val() === "") return true;

    return checkDate(dateDeb, dateFin);
}

function compareDates(dt1, dt2, culture) {
    culture = culture || "fr";
    if (!dt1 && !dt2) { return 0; }
    if (dt1 && !dt2) { return -1; }
    if (!dt1 && dt2) { return 1; }
    let dtMin = null, dtMax = null;
    if (typeof dt1 === "string") {
        dtMin = culture == "fr" ? getDate(dt1) : new Date(dt1);
    }
    else if (dt1 instanceof Date) {
        dtMin = dt1;
    }
    if (typeof dt2 === "string") {
        dtMax = culture == "fr" ? getDate(dt2) : new Date(dt2);
    }
    else if (dt2 instanceof Date) {
        dtMax = dt2;
    }
    if (dtMin == null || dtMax == null) { return null; }
    return dtMin == dtMax ? 0 : (dtMin > dtMax ? -1 : 1);
}

//----------------------Vérifie la date et l'heure de début et de fin---------------------
function checkDateHeure(dateDeb, dateFin, heureDeb, heureFin, minuteDeb, minuteFin) {
    if (dateDeb.val() !== "" || dateFin.val() !== "") {
        if (!isDate(dateDeb.val()) || !isDate(dateFin.val()) || Date.parse(dateDeb.val() + " " + heureDeb.val() + ":" + minuteDeb.val()) == null || Date.parse(dateFin.val() + " " + heureFin.val() + ":" + minuteFin.val()) == null) {
            dateDeb.addClass("requiredField");
            dateFin.addClass("requiredField");
            heureDeb.addClass("requiredField");

            heureFin.addClass("requiredField");
            minuteDeb.addClass("requiredField");
            minuteFin.addClass("requiredField");
            return false;
        }
    }

    if (getDate(dateDeb.val(), heureDeb.val() + ":" + minuteDeb.val()) > getDate(dateFin.val(), heureFin.val() + ":" + minuteFin.val())) {
        dateDeb.addClass("requiredField");
        dateFin.addClass("requiredField");
        heureDeb.addClass("requiredField");
        heureFin.addClass("requiredField");
        minuteDeb.addClass("requiredField");
        minuteFin.addClass("requiredField");
        $("#msgError").html("Veuillez sélectionner une date de début antérieure<br/> à la date de fin.");
        return false;
    }
    return true;
}


//----------------------Vérifie la date de début et de fin avec ID---------------------
function checkDateById(dateDeb, dateFin) {
    if ($("#" + dateDeb).val() !== "" || $("#" + dateFin).val() !== "") {
        if (!isDate($("#" + dateDeb).val()) || !isDate($("#" + dateFin).val()))
            return false;
    }
    if (getDate($("#" + dateDeb).val()) > getDate($("#" + dateFin).val())) {
        return false;
    }
    return true;
}
//----------------------Retourne une date---------------------
function getDate(strDate, strHeure) {
    var day = strDate.substr(0, 2);
    var month = strDate.substr(3, 2);
    var year = strDate.substr(6, 4);
    var hour = 0;
    var minute = 0;
    if (typeof (strHeure) !== "string" && (strDate.length === 19)) {
        strHeure = strDate.substr(11);
    }
    if (typeof (strHeure) === "string") {
        hour = strHeure.indexOf(":") > 0 ? strHeure.split(":")[0] : strHeure.substr(0, 2);
        minute = strHeure.indexOf(":") > 0 ? strHeure.split(":")[1] : strHeure.substr(2, 2);
    }
    var d = new Date(year, month - 1, day, hour, minute, 0, 0);

    return d;
}
//------------Incrémente une date------------
function incrementDate(strDate, addDay, addMonth, addYear, addWeek, removeDayFin) {
    var wDate = getDate(strDate);

    if (addYear !== "")
        wDate.setFullYear(wDate.getFullYear() + parseInt(addYear));
    if (addMonth !== "")
        wDate.setMonth(wDate.getMonth() + parseInt(addMonth));
    if (addDay !== "")
        wDate.setDate(wDate.getDate() + parseInt(addDay));
    if (addWeek != undefined && addWeek !== "")
        wDate.setDate(wDate.getDate() + (parseInt(addWeek) * 7));

    if (removeDayFin === true)
        wDate.setDate(wDate.getDate() - 1);

    var wDay = wDate.getDate() < 10 ? "0" + wDate.getDate() : wDate.getDate();
    var wMonth = wDate.getMonth() < 9 ? "0" + (wDate.getMonth() + 1) : (wDate.getMonth() + 1);

    return wDay + "/" + wMonth + "/" + wDate.getFullYear();
}
//--------------------Ouvre le module Hexavia-------------------
function OpenHexavia(albcontext) {
    var url = "";
    var param = "";

    if (albcontext === "" || albcontext == undefined) {
        $("#AdrContext").val("");
        url = $("#UrlHexavia").val();


        url += "?" + $("#UrlBackHexavia").val() + "&Data=adresse=";

        param = $("#ContactAdresse_Batiment").val() + "¤";
        param += $("#ContactAdresse_No").val() + "¤";
        param += $("#ContactAdresse_Extension").val() + "¤";
        param += $("#ContactAdresse_Voie").val() + "¤";
        param += $("#ContactAdresse_Distribution").val() + "¤";
        param += $("#ContactAdresse_Ville").val() + "¤";
        param += ($("#ContactAdresse_CodePostal").val() !== "0" ? $("#ContactAdresse_CodePostal").val() : "") + "¤";
        param += $("#ContactAdresse_VilleCedex").val() + "¤";
        param += ($("#ContactAdresse_CodePostalCedex").val() !== "0" ? $("#ContactAdresse_CodePostalCedex").val() : "") + "¤";
        param += $("#ContactAdresse_Pays").val() + "¤";
        param += $("#ContactAdresse_MatriculeHexavia").val() + "¤";
        param += $("#ContactAdresse_Latitude").val() + "¤";
        param += $("#ContactAdresse_Longitude").val();

        param = encodeURIComponent(param);
    }
    else {
        $("#AdrContext").val(albcontext);
        url = $("input[id='UrlHexavia'][albcontext='" + albcontext + "']").val();
        url += "?" + $("input[id='UrlBackHexavia'][albcontext='" + albcontext + "']").val() + "&Data=adresse=";

        param = $("input[id='ContactAdresse_Batiment'][albcontext='" + albcontext + "']").val() + "¤";
        param += $("input[id='ContactAdresse_No'][albcontext='" + albcontext + "']").val() + "¤";
        param += $("input[id='ContactAdresse_Extension'][albcontext='" + albcontext + "']").val() + "¤";
        param += $("input[id='ContactAdresse_Voie'][albcontext='" + albcontext + "']").val() + "¤";
        param += $("input[id='ContactAdresse_Distribution'][albcontext='" + albcontext + "']").val() + "¤";
        param += $("input[id='ContactAdresse_Ville'][albcontext='" + albcontext + "']").val() + "¤";
        param += ($("input[id='ContactAdresse_CodePostal'][albcontext='" + albcontext + "']").val() !== "0" ? $("input[id='ContactAdresse_CodePostal'][albcontext='" + albcontext + "']").val() : "") + "¤";
        param += $("input[id='ContactAdresse_VilleCedex'][albcontext='" + albcontext + "']").val() + "¤";
        param += ($("input[id='ContactAdresse_CodePostalCedex'][albcontext='" + albcontext + "']").val() !== "0" ? $("input[id='ContactAdresse_CodePostalCedex'][albcontext='" + albcontext + "']").val() : "") + "¤";
        param += $("input[id='ContactAdresse_Pays'][albcontext='" + albcontext + "']").val() + "¤";
        param += $("input[id='ContactAdresse_MatriculeHexavia'][albcontext='" + albcontext + "']").val() + "¤";

        param += $("input[id='ContactAdresse_Latitude'][albcontext='" + albcontext + "']").val() + "¤";
        param += $("input[id='ContactAdresse_Longitude'][albcontext='" + albcontext + "']").val();


        param = encodeURIComponent(param);
    }




    window.open(url + param, "AdresseHexavia", "resizable=yes, menubar=no, status=no, width=1000, height=800");
}
//--------------------Retour Hexavia-----------------------
fonctionHexavia = function (adresse) {
    getAdress(adresse);
};
getAdress = function (adresse) {
    try {
        adresse = decodeURIComponent(adresse);

        var param = adresse.split("¤");
        var numVoie;

        var isAdresseEmpty = (param[0] + param[1] + param[2] + param[3] + param[4] + param[5] + param[6] + param[7] + param[8]) == "";
        if (isAdresseEmpty) {
            param[9] = "";
        }
        if ($("#AdrContext").val() === "" || $("#AdrContext").val() == undefined) {
            //#region sans albContext

            $("#ContactAdresse_Batiment").val(param[0]);
            $("#ContactAdresse_No").val(param[1]);
            $("#ContactAdresse_Extension").val(param[2]);
            $("#ContactAdresse_Voie").val(param[3]);
            $("#ContactAdresse_Distribution").val(param[4]);
            $("#ContactAdresse_VilleCedex").val(param[5]);
            $("#ContactAdresse_CodePostalCedex").val(param[6]);
            $("#ContactAdresse_Ville").val(param[7]);
            $("#ContactAdresse_CodePostal").val(param[8]);


            $("#ContactAdresse_Pays").val(param[9]);
            //param[10] renvoie VALIDE si l'adresse est valide
            $("#ContactAdresse_MatriculeHexavia").val(param[11]);
            $("#ContactAdresse_Latitude").val(param[12]);
            $("#ContactAdresse_Longitude").val(param[13]);


            if ($.trim(param[11]) === "" && !isAdresseEmpty) {
                $("#divErrorHexavia").show();
            }
            else {
                $("#divErrorHexavia").hide();
            }

            ///Met à jour les infos de l'adresse
            numVoie = param[1];

            $("#lblInfoBatiment").text(param[0]);
            $("#lblInfoNo").text(numVoie);
            $("#lblInfoExtension").text(param[2]);
            $("#lblInfoVoie").text(param[3]);
            $("#lblInfoDistribution").text(param[4]);

            $("#lblInfoVille").text(param[7]);
            $("#lblInfoCodePostal").text(param[8]);

            ///Met à jour les infos de l'adresse dans la div flottante
            $("#lblBatiment").text(param[0]);
            $("#lblNo").text(param[1]);
            $("#lblExtension").text(param[2]);
            $("#lblVoie").text(param[3]);
            $("#lblDistribution").text(param[4]);
            $("#lblVille").text(param[7]);
            $("#lblCodePostal").text(param[8]);
            $("#lblVilleCedex").text(param[5]);
            $("#lblCodePostalCedex").text(param[6]);

            $("#lblPays").text(param[9]);


            //#endregion
        }
        else {
            //#region avec albContext
            $("input[id='ContactAdresse_Batiment'][albcontext='" + $("#AdrContext").val() + "']").val(param[0]);
            $("input[id='ContactAdresse_No'][albcontext='" + $("#AdrContext").val() + "']").val(param[1]);
            $("input[id='ContactAdresse_Extension'][albcontext='" + $("#AdrContext").val() + "']").val(param[2]);
            $("input[id='ContactAdresse_Voie'][albcontext='" + $("#AdrContext").val() + "']").val(param[3]);
            $("input[id='ContactAdresse_Distribution'][albcontext='" + $("#AdrContext").val() + "']").val(param[4]);
            $("input[id='ContactAdresse_VilleCedex'][albcontext='" + $("#AdrContext").val() + "']").val(param[5]);
            $("input[id='ContactAdresse_CodePostalCedex'][albcontext='" + $("#AdrContext").val() + "']").val(param[6]);
            $("input[id='ContactAdresse_Ville'][albcontext='" + $("#AdrContext").val() + "']").val(param[7]);
            $("input[id='ContactAdresse_CodePostal'][albcontext='" + $("#AdrContext").val() + "']").val(param[8]);


            $("input[id='ContactAdresse_Pays'][albcontext='" + $("#AdrContext").val() + "']").val(param[9]);
            $("input[id='ContactAdresse_MatriculeHexavia'][albcontext='" + $("#AdrContext").val() + "']").val(param[11]);

            $("input[id='ContactAdresse_Latitude'][albcontext='" + $("#AdrContext").val() + "']").val(param[12]);
            $("input[id='ContactAdresse_Longitude'][albcontext='" + $("#AdrContext").val() + "']").val(param[13]);


            if ($.trim(param[11]) === "" && !isAdresseEmpty) {
                $("input[id='divErrorHexavia'][albcontext='" + $("#AdrContext").val() + "']").show();
            }
            else {
                $("input[id='divErrorHexavia'][albcontext='" + $("#AdrContext").val() + "']").hide();
            }

            ///Met à jour les infos de l'adresse
            numVoie = param[1];
            if (numVoie !== "")
                numVoie = numVoie + ", ";

            $("label[id='lblInfoBatiment'][albcontext='" + $("#AdrContext").val() + "']").text(param[0]);
            $("label[id='lblInfoNo'][albcontext='" + $("#AdrContext").val() + "']").text(numVoie);
            $("label[id='lblInfoExtension'][albcontext='" + $("#AdrContext").val() + "']").text(param[2]);
            $("label[id='lblInfoVoie'][albcontext='" + $("#AdrContext").val() + "']").text(param[3]);
            $("label[id='lblInfoDistribution'][albcontext='" + $("#AdrContext").val() + "']").text(param[4]);

            $("label[id='lblInfoVille'][albcontext='" + $("#AdrContext").val() + "']").text(param[7]);
            $("label[id='lblInfoCodePostal'][albcontext='" + $("#AdrContext").val() + "']").text(param[8]);

            ///Met à jour les infos de l'adresse dans la div flottante
            $("label[id='lblBatiment'][albcontext='" + $("#AdrContext").val() + "']").text(param[0]);
            $("label[id='lblNo'][albcontext='" + $("#AdrContext").val() + "']").text(param[1]);
            $("label[id='lblExtension'][albcontext='" + $("#AdrContext").val() + "']").text(param[2]);
            $("label[id='lblVoie'][albcontext='" + $("#AdrContext").val() + "']").text(param[3]);
            $("label[id='lblDistribution'][albcontext='" + $("#AdrContext").val() + "']").text(param[4]);
            $("label[id='lblVille'][albcontext='" + $("#AdrContext").val() + "']").text(param[7]);
            $("label[id='lblCodePostal'][albcontext='" + $("#AdrContext").val() + "']").text(param[8]);
            $("label[id='lblVilleCedex'][albcontext='" + $("#AdrContext").val() + "']").text(param[5]);
            $("label[id='lblCodePostalCedex'][albcontext='" + $("#AdrContext").val() + "']").text(param[6]);

            $("label[id='lblPays'][albcontext='" + $("#AdrContext").val() + "']").text(param[9]);

            //#endregion
        }
    }
    catch (e) {
        ShowCommonFancy("Warning", "", "Merci de contacter l'administrateur: Veuillez verifier le nom schema du champ pour l'adresse Hexavia.", "auto", "auto", true);
    }
};
//----------- Redirection ------------------------
function OpenPage(page) {
    window.location.href = page;
}

//--- Permet l'alternance des couleurs pour les lignes d'une table.
//-----------------------------------------------------------------
//--- id            = id de la table sans le prefixe "tbl".
//--- selInput      = id du controle qui contient la valeur selectionnée. Mettre "noInput" pour avoir la couleur rose sans vouloir sélectionner une valeur
//--- clickable     = Indique si les lignes de la table sont clickables.
//--- fonctionClick = fonction à executer lors du click.
function AlternanceLigne(id, selInput, clickable) {
    let table = $("#tbl" + id);
    table.find("tr:even")
        .css({ "background-color": "#fff" });
    table.off("hove", "tr:even").on("hover", "tr:even",
        function () {
            if (clickable) {
                $(this).css({ "background-color": "#FFDFDF", "cursor": "pointer" });
            }
        },
        function () {
            // Changement de couleur seulement si l'element est selectionnable et different de celui selectionné
            if (clickable && (typeof ($("#" + selInput).val()) === "undefined" || $("#" + selInput).val() === "" || $.trim($("#" + selInput).val()).toLowerCase() !== $.trim($(this).attr("id")).toLowerCase())) {
                $(this).css({ "background-color": "#fff" });
            }
        });
    table.find("tr:odd").css({ "background-color": "#edeeff" });
    table.off("hover", "tr:odd").on("hover", "tr:odd",
        function () {
            if (clickable) {
                $(this).css({ "background-color": "#FFDFDF", "cursor": "pointer" });
            }
        },
        function () {
            // ---- Changement de couleur seulement si l'element est selectionnable et different de celui selectionné
            if (clickable && (typeof ($("#" + selInput).val()) === "undefined" || $("#" + selInput).val() === "" || $.trim($("#" + selInput).val()).toLowerCase() !== $.trim($(this).attr("id")).toLowerCase())) {
                $(this).css({ "background-color": "#edeeff" });
            }
        });
}

function AlternanceLigneAffiche(id, selInput, clickable) {
    let table = $("#tbl" + id);
    table.find("tr:visible:even")
        .css({ "background-color": "#fff" });
    table.off("hover", "tr:visible:even").on("hover", "trMvisible:even",
        function () {
            if (clickable) {
                $(this).css({ "background-color": "#FFDFDF", "cursor": "pointer" });
            }
        },
        function () {
            // Changement de couleur seulement si l'element est selectionnable et different de celui selectionné
            if (clickable && (typeof ($("#" + selInput).val()) === "undefined" || $("#" + selInput).val() === "" || $.trim($("#" + selInput).val()).toLowerCase() !== $.trim($(this).attr("id")).toLowerCase())) {
                $(this).css({ "background-color": "#fff" });
            }
        });

    table.find("tr:visible:odd").css({ "background-color": "#edeeff" });
    table.off("hove", "tr:visible:odd").on("hover", "tr:visible:odd",
        function () {
            if (clickable) {
                $(this).css({ "background-color": "#FFDFDF", "cursor": "pointer" });
            }
        },
        function () {
            // ---- Changement de couleur seulement si l'element est selectionnable et different de celui selectionné
            if (clickable && (typeof ($("#" + selInput).val()) === "undefined" || $("#" + selInput).val() === "" || $.trim($("#" + selInput).val()).toLowerCase() !== $.trim($(this).attr("id")).toLowerCase())) {
                $(this).css({ "background-color": "#edeeff" });
            }
        });
}

function AlternanceLigneSpecifique(id, selInput, clickable) {
    let table = $(id);
    table.find("tr:even").css({ "background-color": "#fff" });
    table.off("hover", "tr:even").on("hover", "tr:even",
        function () {
            if (clickable) {
                $(this).css({ "background-color": "#FFDFDF", "cursor": "pointer" });
            }
        },
        function () {
            // Changement de couleur seulement si l'element est selectionnable et different de celui selectionné
            if (clickable && (typeof ($("#" + selInput).val()) === "undefined" || $("#" + selInput).val() === "" || $.trim($("#" + selInput).val()).toLowerCase() !== $.trim($(this).attr("id")).toLowerCase())) {
                $(this).css({ "background-color": "#fff" });
            }
        });

    table.find("tr:odd").css({ "background-color": "#edeeff" });
    table.off("hover", "tr:odd").on("hover", "tr:odd",
        function () {
            if (clickable) {
                $(this).css({ "background-color": "#FFDFDF", "cursor": "pointer" });
            }
        },
        function () {
            // ---- Changement de couleur seulement si l'element est selectionnable et different de celui selectionné
            if (clickable && (typeof ($("#" + selInput).val()) === "undefined" || $("#" + selInput).val() === "" || $.trim($("#" + selInput).val()).toLowerCase() !== $.trim($(this).attr("id")).toLowerCase())) {
                $(this).css({ "background-color": "#edeeff" });
            }
        });
}
/**
 * @callback OnCloseCallback
 * @param {JQEvent} event
 */

/**------------------Affiche une fancyBox---------------------------
 * @param  {string} idDiv nom de la fancy Error/Confirm (ex : "Error" pour la fancy "fancyError")
 * @param  {string} action action de la fancy lors d'une confirmation (ex: "Cancel" pour un cancel de la form)
 * @param  {string} text message de la div
 * @param  {number} width largeur de la fancy
 * @param  {number} height hauteur de la fancy
 * @param  {boolean} modal la fancy est modal ou non
 * @param  {boolean} displayTop on dispose la fancy en haut de page
 * @param  {boolean} blockDim  les dimensions sont modifiables par rapport au texte
 * @param  {boolean?} iframe on vient d'une iframe ou non (iframe => affichage d'une div ; sinon affichage de la fancy)
 * @param  {OnCloseCallback} onClose action lorsqu' on ferme la fenêtre
**/
function ShowCommonFancy(idDiv, action, text, width, height, modal, displayTop, blockDim, iframe, onClose) {
    if (iframe === undefined) {
        iframe = window.parent != null && window.top != window.parent;
    }
    const SessionLost = "Session perdue.";
    if (idDiv === "Error" && text.toUpperCase().indexOf("SESSION_LOST") === 0) {
        if (text.toUpperCase().indexOf("_RELOAD") > 0) {
            $("#btnErrorOk").data("session-lost", true);
        }
        text = SessionLost;
    }
    width = width || "auto";
    height = height || "auto";
    $("#hiddenAction").val(action);
    if (iframe) {
        switch (idDiv) {
            case "Error":
                $("#tblIframeConfirm").hide();
                $("#tblIframeError").show();
                if (text != null) {
                    $("#msgIframeError").html(text);
                }
                break;
            case "Confirm":
                $("#tblIframeConfirm").show();
                $("#tblIframeError").hide();
                if (text != null) {
                    $("#msgIframeConfirm").html(text);
                }
                break;
        }
        $("#dvFancyIframe").show();
    }
    else {
        if (action === "Cancel" && $("#OffreReadOnly").val() === "True" || action === "CancelReadonly") {
            if (idDiv === "ConfirmQuitt") {
                $("#btnConfirmOkQuitt").click();
            }
            else {
                $("#btnConfirmOk").click();
            }
            return;
        }
        if (width > 900) {
            $("#msgError").removeClass();
        }
        if (displayTop === true) {
            AlbScrollTop();
        }
        if (text == undefined)
            blockDim = true;
        if (!blockDim) {
            if (text.length > 500) {
                width = 1212;
                height = 500;
            }
            else {
                if (text.length > 70 && text.length < 500) {
                    width = 350;
                    height = 150;
                } else {
                    if (idDiv === "Error") {
                        if (!width || text.length < 500) {
                            width = 300;
                        }
                        if (!height || text.length < 500) {
                            height = 100;
                        }
                    }
                }
            }
        }
        if (text != null) {
            $("#msg" + idDiv).html(text);
        }

        if (__useJQDialog) {
            var pos = displayTop ? "center top" : "center";
            const options = {
                closeOnEscape: false,
                dialogClass: "no-cross-close",
                draggable: true,
                modal: modal,
                position: { my: pos, at: pos, of: window },
                width: width,
                height: height == "auto" ? height : height + 50,
                title: $("#fancy" + idDiv).find("tr:first-child td").html(),
                close: onClose
            };

            console.trace(options);

            $("#fancy" + idDiv).dialog(options);
            $("#fancy" + idDiv).find("tr:first-child").hide();

            var p = $("#fancy" + idDiv).closest(".ui-dialog");
            if (p.parent().get(0).tagName == "BODY") {
                p.wrap($("<div style=\"position:absolute\">"));
                $("#fancy" + idDiv).dialog("option", $("#fancy" + idDiv).dialog("option"));
            }

        } else {


            $.fancybox(
                $("#fancy" + idDiv).html(),
                {
                    'autoDimensions': false,
                    //'autoSize': false,
                    'width': width,
                    'height': height,
                    'transitionIn': "elastic",
                    'transitionOut': "elastic",
                    'speedin': 400,
                    'speedOut': 400,
                    'easingOut': "easeInBack",
                    'modal': modal,
                    'topRatio': 0
                }
            );
            $.fancybox.update();
        }
        $("#fancybox-wrap").css("background-color", "grey");
        if (displayTop === true)
            $("#fancybox-wrap").addClass("fancybox-wrap_Top");
        else $("#fancybox-wrap").removeClass("fancybox-wrap_Top");
    }

    function getDims() {
        var t = $("<div>");
        var tw = $("<div>");
        tw.css("position", "absolute");
        t.css("visibility", "hidden");
        t.css("position", "absolute");
        t.css("width", convert(width));
        t.css("height", convert(height));
        t.html(text || $("#msg" + (iframe ? "Iframe" : "") + idDiv).html());
        const elem0 = tw[0];
        const elem1 = t[0];
        elem0.appendChild(elem1);
        document.body.appendChild(elem0);
        var s = window.getComputedStyle(elem1);
        var ret = { height: parseFloat(s.height), width: parseFloat(s.width) };
        document.body.removeChild(elem0);

        return ret;
    }
    function convert(val) {
        if (val instanceof String) {
            if (isNaN(val)) {
                return val;
            }
            return val + "px";
        }
        return val.toString() + "px";
    }


}
//-----------------Ferme la fancyBox-------------------------------
function CloseCommonFancy() {
    $("#dvFancyMsg").html("");
    $("#dvFancyIframe").hide();
    if (__useJQDialog) {
        $(".ui-dialog-content").dialog("close");
    } else {
        $.fancybox.close();
    }
    if ($("#btnErrorOk").data("session-lost")) {
        common.page.isLoading = true;
        setTimeout(function () {
            common.page.goHome();
        });
        return;
    }
    CloseLoading();
}
function CloseFancy() {
    $(".ui-dialog-content").dialog("close");
}

function ShowLoading() {
    common.dom.pushForward("divLoading");
    common.page.isLoading = true;
}

function CloseLoading() {
    common.page.isLoading = false;
}
//--------------Copie les messages de fancy dans une "seconde fancy" (div show/hide)
function GetHtmlFancy(action) {
    return $("#msg" + action).html();
}
//--------------Affiche une "seconde fancy" (div show/hide)
function ShowDialogInFancy(action, msg) {
    $("#msg" + action).html(msg);
    $("#msgDialog").html(GetHtmlFancy(action));
    $("#divDialogInFancy").show();
    if (msg.length < 290) {
        $("#divDialog").removeClass("divFancyGrande").addClass("divFancyPetite");
    }
    else {
        $("#divDialog").removeClass("divFancyPetite").addClass("divFancyGrande");
    }
}
//-----------Fait un padding left-------------
function PaddingLeft(str, max) {
    return str.length < max ? PaddingLeft("0" + str, max) : str;
}
//----------Retourne une date formatée (format FR)-----------
function FormatDate(date) {
    var day = PaddingLeft(String(date.getDate()), 2);
    var month = PaddingLeft(String(date.getMonth() + 1), 2);
    var year = date.getFullYear();
    return day + "/" + month + "/" + year;
}
//----------Retourne une date au format numérique-------
function FormatDateNumerique(date) {
    if (isDate(date)) {
        var day = date.substr(0, 2);
        var month = date.substr(3, 2);
        var year = date.substr(6, 4);

        return year + month + day;
    }
    return "";
}


//-------Formatter une valeur numérique--------
function FormatNumerique(attribut, separator, valueMax, valueMin) {
    $("input[albMask=" + attribut + "]")
        .autoNumeric("destroy")
        .autoNumeric("init", { digitGroupSeparator: separator, decimalCharacter: ",", decimalPlacesOverride: 0, maximumValue: valueMax, minimumValue: valueMin });
    $("span[albMask=" + attribut + "]")
        .autoNumeric("destroy").autoNumeric("init", { digitGroupSeparator: " ", decimalCharacter: ",", decimalPlacesOverride: 0, maximumValue: valueMax, minimumValue: valueMin });
}
//-------Formatter une valeur décimale-------
function FormatDecimal(attribut, separator, nbrDec, valueMax, valueMin) {
    $("input[albMask=" + attribut + "]").autoNumeric("destroy").each(function () {
        $(this).val($(this).val().replace(",", "."));
    });
    $("input[albMask=" + attribut + "]").autoNumeric("init", { digitGroupSeparator: separator, decimalCharacter: ",", decimalPlacesOverride: nbrDec, maximumValue: valueMax, minimumValue: valueMin }).each(function () {
        $(this).val($(this).val().replace(".", ","));
    });
    $("span[albMask=" + attribut + "]").autoNumeric("destroy").each(function () {
        $(this).text($(this).text().replace(",", "."));
    });
    $("span[albMask=" + attribut + "]").autoNumeric("init", { digitGroupSeparator: " ", decimalCharacter: ",", decimalPlacesOverride: nbrDec, maximumValue: valueMax, minimumValue: valueMin }).each(function () {
        $(this).text($(this).text().replace(".", ","));
    });
}
//-------Formatter une valeur pourcentage numérique--------
function FormatPourcentNumerique() {
    $("input[albMask=pourcentnumeric]")
        .autoNumeric("destroy")
        .autoNumeric("init", { digitGroupSeparator: "", decimalCharacter: ",", decimalPlacesOverride: 0, maximumValue: "100", minimumValue: "0" });
    $("span[albMask=pourcentnumeric]")
        .autoNumeric("destroy")
        .autoNumeric("init", { digitGroupSeparator: " ", decimalCharacter: ",", decimalPlacesOverride: 0, maximumValue: "100", minimumValue: "0" });
}
//-------Formatter une valeur pourcentage décimale-------
function FormatPourcentDecimal() {
    $("input[albMask=pourcentdecimal]").autoNumeric("destroy").each(function () {
        $(this).val($(this).val().replace(",", "."));
    });
    $("input[albMask=pourcentdecimal]").autoNumeric("init", { digitGroupSeparator: "", decimalCharacter: ",", decimalPlacesOverride: 2, maximumValue: "100.00", minimumValue: "0.00" }).each(function () {
        $(this).val($(this).val().replace(".", ","));
    });
    $("span[albMask=pourcentdecimal]").autoNumeric("destroy").each(function () {
        $(this).text($(this).text().replace(",", "."));
    });
    $("span[albMask=pourcentdecimal]").autoNumeric("init", { digitGroupSeparator: " ", decimalCharacter: ",", decimalPlacesOverride: 2, maximumValue: "100.00", minimumValue: "0.00" }).each(function () {
        $(this).text($(this).text().replace(".", ","));
    });
}
//------Formatter une valeur pourcentage décimale positive ou négative
function FormatPourcentDecimalWithNegative() {
    $("input[albMask=pourcentdecimal]").autoNumeric("destroy").each(function () {
        $(this).val($(this).val().replace(",", "."));
    });
    $("input[albMask=pourcentdecimal]").autoNumeric("init", { digitGroupSeparator: "", decimalCharacter: ",", decimalPlacesOverride: 2, maximumValue: "100.00", minimumValue: "-100.00" }).each(function () {
        $(this).val($(this).val().replace(".", ","));
    });
    $("span[albMask=pourcentdecimal]").autoNumeric("destroy").each(function () {
        $(this).text($(this).text().replace(",", "."));
    });
    $("span[albMask=pourcentdecimal]").autoNumeric("init", { digitGroupSeparator: " ", decimalCharacter: ",", decimalPlacesOverride: 2, maximumValue: "100.00", minimumValue: "-100.00" }).each(function () {
        $(this).text($(this).text().replace(".", ","));
    });
}
//-------Formatter une valeur numérique sur les informations spécifiques--------
function FormatNumeriqueInformationsSpecifiques(separator, valueMax, valueMin) {
    $(".numerique").autoNumeric("destroy").autoNumeric("init", { digitGroupSeparator: separator, decimalCharacter: ",", decimalPlacesOverride: 0, maximumValue: valueMax, minimumValue: valueMin });
}
//-------Formatter une valeur décimale sur les informations spécifiques-------
function FormatDecimalInformationsSpecifiques(separator, nbrDec, valueMax, valueMin) {
    $(".decimal").autoNumeric("destroy").each(function () {
        $(this).val($(this).val().replace(",", "."));
    });
    $(".decimal").autoNumeric("init", { digitGroupSeparator: separator, decimalCharacter: ",", decimalPlacesOverride: nbrDec, maximumValue: valueMax, minimumValue: valueMin }).each(function () {
        $(this).val($(this).val().replace(".", ","));
    });
}

function NumericReplaceAt(index, char) {
    return this.substr(0, index) + char + this.substr(index + 1);
}
function rhtmlspecialchars(str) {
    if (typeof (str) === "string") {
        str = str.replace(/&gt;/ig, ">");
        str = str.replace(/&lt;/ig, "<");
        str = str.replace(/&#039;/g, "'");
        str = str.replace(/&quot;/ig, '"');
        str = str.replace(/&amp;/ig, "&"); /* must do &amp; last */
    }
    return str;
}
//---------------------Affecte les fonctions sur les controles heures-----------------------
function ChangeHourCommon() {
    $("#AlbTimeHours").live("change", function () {
        if ($(this).val() !== "" && $("#AlbTimeMinutes").val() === "")
            $("#AlbTimeMinutes").val("00");
        AffecteHourCommon($(this));
    });
    $("#AlbTimeMinutes").live("change", function () {
        if ($(this).val() !== "" && $("#AlbTimeHours").val() === "")
            $("#AlbTimeHours").val("00");
        AffecteHourCommon($(this));
    });
}
//-------------------Renseigne l'heure---------------------------
function AffecteHourCommon(elem) {
    var elemId = elem.attr("id").replace("Hours", "").replace("Minutes", "");

    var changeHour = SetHours(elemId);
    if (!changeHour && elem.val() === "") {
        $("#" + elemId + "Hours").val("");
        $("#" + elemId + "Minutes").val("");
    }
}
//-------------------chereche  la valeur dun paramètre à partir d'une QueryString---------------------------
function GetParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}


/* Zone de fonctions de la double saisie */
//---------------------Affecte les fonctions au boutons-------------
function MapPageElementDblSaisie() {
    $("#btnDFAnnulerDbl").removeAttr("disabled");
    $("#btnErrorOk").kclick(function () {
        CloseCommonFancy();
    });
    $("#DblDateSaisie").offOn("change", function () {
        $("#DblSaisieChangeInProgress").val("1");
        if ($(this).val() !== "") {
            if ($("#SaisieHeureHours").val() === "") {
                $("#SaisieHeureHours").val("00");
                $("#SaisieHeureMinutes").val("00");
                $("#SaisieHeureHours").change();
            }
        }
        else {
            $("#SaisieHeureHours").val("");
            $("#SaisieHeureMinutes").val("");
            $("#SaisieHeureHours").change();
        }
    });
    $("#Code").focus();
    $(".datepicker").die().live("focus", function () {
        $(this).datepicker({ dateFormat: "dd/mm/yy" });
    });
    AlternanceLigne("DoubleSaisieBody", "", false, null);
    AlternanceLigne("DoubleSaisieHistoBody", "", false, null);
    ChangeHourDblSaisie();
    $("#ValiderBtnDbl").kclick(function () {
        if (!$(this).attr("disabled")) {
            ValidFormDblSaisie();
        }
    });
    $("#btnAnnulerDbl").kclick(function () {
        DeverouillerUserOffres(common.tabGuid, function () {
            document.location.replace("/" + ($("#txtParamRedirect").val() || "RechercheSaisie/Index"));
        });
    });
    $("#btnDFAnnulerDbl").kclick(function () {
        $("#DFDblSaisie").html("");
        $("#divComDblSaisie").hide();
    });
    $("input[type=radio][name=Actions]").offOn("change", function () {
        if ($("#MaintenirCourtier").is(":checked")) {
            $("#lblDblSaisieVersion").text($("#DblSaisieCurrentVersion").val());
            $("#MotifRemp").val("").addClass("readonly").attr("disabled", "disabled");
        }
        if ($("#RemplacerCourtier").is(":checked")) {
            if ($("#DlbSaisieEtatOffre").val() === "V") {
                var newVersion = parseInt($("#DblSaisieCurrentVersion").val()) + 1;
                $("#lblDblSaisieVersion").text(newVersion);
            }
            $("#MotifRemp").removeClass("readonly").removeAttr("disabled");
        }
    });
    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Submit":
                SubmitFormDblSaisie();
                break;
        }
        $("#hiddenAction").val("");
    });
    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
        $("#hiddenAction").val("");
    });

    $("#DblCode, #DblCourtier, #DblDelegation, #DblNomInterlocuteur, #DblReference, #DblSouscripteur, #SaisieHeureHours, #SaisieHeureMinutes, #MaintenirCourtier, #RemplacerCourtier, #MotifRemp").offOn("change", function () {
        AffectTitleList($(this));
        $("#DblSaisieChangeInProgress").val("1");
    });

    AffectTitleList($("#MotifRemp"));

}
//------------Affecte les fonctions sur les controles heures--------------
function ChangeHourDblSaisie() {
    $("#SaisieHeureHours").offOn("change", function () {
        if ($(this).val() !== "" && $("#SaisieHeureMinutes").val() === "")
            $("#SaisieHeureMinutes").val("00");
        AffecteHourDblSaisie($(this));
    });
    $("#SaisieHeureMinutes").offOn("change", function () {
        if ($(this).val() !== "" && $("#SaisieHeureHours").val() === "")
            $("#SaisieHeureHours").val("00");
        AffecteHourDblSaisie($(this));
    });
}
//----------------Redirection------------------
function RedirectionDblSaisie(cible, job) {
    var paramRedirect = $("#txtParamRedirect").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/DoubleSaisie/Redirection/",
        data: { cible: cible, job: job, paramRedirect: paramRedirect },
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------------Renseigne l'heure---------------------------
function AffecteHourDblSaisie(elem) {
    var elemId = elem.attr("id").replace("Hours", "").replace("Minutes", "");
    var changeHour = SetHours(elemId);
    if (!changeHour && elem.val() === "") {
        $("#" + elemId + "Hours").val("");
        $("#" + elemId + "Minutes").val("");
    }
}
//----------------------Vérifie la validation de la form pour l'enregistrement---------------------
function ValidFormDblSaisie() {
    $(".requiredField").removeClass("requiredField");
    if (!$("#MaintenirCourtier").is(":checked") && !$("#RemplacerCourtier").is(":checked")) {
        ShowCommonFancy("Error", "",
            "Veuillez sélectionner une action à réaliser.",
            350, 130, true, true);
        return;
    }
    else {
        if ($("#RemplacerCourtier").is(":checked") && $("#MotifRemp").val() === "") {
            $("#MotifRemp").addClass("requiredField");
            ShowCommonFancy("Error", "",
                "Veuillez sélectionner un motif.",
                350, 130, true, true);
            return;
        }
    }

    var erreurBool = false;

    if ($("#DblDateSaisie").val() === "" || !isDate($("#DblDateSaisie").val()) || Date.parse($("#DblDateSaisie").val() + " " + $("#SaisieHeureHours").val() + ":" + $("#SaisieHeureMinutes").val()) == null) {
        $("#DblDateSaisie").addClass("requiredField");
        $("#SaisieHeureHours").addClass("requiredField");
        $("#SaisieHeureMinutes").addClass("requiredField");
        erreurBool = true;
    }

    if ($("#DblCode").val() === "") {
        $("#DblCode").addClass("requiredField");
        erreurBool = true;
    }

    if ($("#DblCourtier").val() === "") {
        $("#DblCourtier").addClass("requiredField");
        erreurBool = true;
    }

    if (erreurBool) {
        return;
    }
    DeverouillerUserOffres(common.tabGuid);
    SubmitFormDblSaisie();
}
//----------------------Envoi la form pour l'enregistrement--------------
function SubmitFormDblSaisie() {

    var action = "";
    if ($("#MaintenirCourtier").is(":checked"))
        action = "REF";
    if ($("#RemplacerCourtier").is(":checked"))
        action = "REM";
    var argDemandeur = "{";

    argDemandeur += 'Code:"' + $("#DblCode").val() + '",';
    argDemandeur += 'Courtier:"' + $("#DblCourtier").val() + '",';
    argDemandeur += 'Souscripteur:"' + $("#DblSouscripteur").val() + '",';
    argDemandeur += 'Delegation:"' + $("#DblDelegation").val() + '",';
    argDemandeur += 'SaisieDate:"' + $("#DblDateSaisie").val() + '",';
    argDemandeur += 'SaisieHeure:"' + $("#SaisieHeure").val() + '",';
    argDemandeur += 'Action:"' + action + '",';
    argDemandeur += 'MotifRemp:"' + $("#MotifRemp").val() + '",';
    argDemandeur += 'Interlocuteur:"' + $("#DblCodeInterlocuteur").val() + '",';
    argDemandeur += 'Reference:"' + $("#DblReference").val() + '"';

    argDemandeur += "}";

    var paramRedirect = $("#txtParamRedirect").val();

    var codeAvn = $("#NumAvenantPage").val();
    ShowLoading();

    if ($("#DivFlottante").val() === "O") {
        $.ajax({
            type: "POST",
            url: "/CommonNavigation/EnregistrerDemandeur",
            data: { codeOffre: $("#Offre_CodeOffre").val(), version: $("#DblSaisieCurrentVersion").val(), type: $("#Offre_Type").val(), argDemandeur: argDemandeur, tabGuid: $("#tabGuid").val(), codeAvn: codeAvn },
            success: function (data) {
                //if (action !== "REM") {
                //    $("#divDoubleSaisieHistoBody").html(data);
                //    AlternanceLigne("DoubleSaisieHistoBody", "", false, null);
                //    EmptyDemandeurDblSaisie();
                //    CloseLoading();
                //}
                //else {
                //    $("#DFDblSaisie").html(data);
                //    AlbScrollTop();
                //    $("#divComDblSaisie").show();
                //    MapPageElementDblSaisie();
                //    MapCommonAutoCompCourtier();
                //    MapCommonAutoCompSouscripteur();
                //    CloseLoading();
                //}
                //$("#CourtierInvalideDiv").html("");
                //$("#DblSaisieChangeInProgress").val("0");
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    else {
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        $.ajax({
            type: "POST",
            url: "/DoubleSaisie/EnregistrerDemandeur",
            data: { codeOffre: $("#Offre_CodeOffre").val(), version: $("#DblSaisieCurrentVersion").val(), type: $("#Offre_Type").val(), argDemandeur: argDemandeur, tabGuid: $("#tabGuid").val(), paramRedirect: paramRedirect, addParamType: addParamType, addParamValue: addParamValue },
            success: function (data) {
                //if (paramRedirect == null || paramRedirect === "") {
                //    if (action !== "REM") {
                //        $("#divDoubleSaisieHistoBody").html(data);
                //        AlternanceLigne("DoubleSaisieHistoBody", "", false, null);
                //        EmptyDemandeurDblSaisie();
                //        CloseLoading();
                //        $("#CourtierInvalideDiv").html("");
                //    }
                //}
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//------------Vide les données du demandeur------------
function EmptyDemandeurDblSaisie() {
    $("#DblCode").val("");
    $("#DblCourtier").val("");
    $("#DblSouscripteur").val("");
    $("#DblDelegation").val("");
    $("#DblDateSaisie").val("");
    $("#SaisieHeureHours").val("");
    $("#SaisieHeureMinutes").val("");
    $("#SaisieHeure").val("");
    $("input[name=Actions]").removeAttr("checked");
    $("#MotifRemp").val("");
    $("#courtierError").text("");
    $("#DblCodeInterlocuteur").val("");
    $("#DblNomInterlocuteur").val("");
    $("#DblReference").val("");
}

//---- Function AlbScrollTop-----/
function AlbScrollTop() {
    window.parent.scrollTo(0, 0);
}

/*            Fin de zone                */

//--------Affecte le title à la dropDownList----------
function AffectTitleList(elem) {
    var lstTitle = elem.find(":selected").attr("title");
    if (lstTitle == undefined) {
        if (elem.attr("title") == undefined || elem.is("select"))
            lstTitle = "";
        else
            lstTitle = elem.attr("title");
    }
    var conceptFamille = elem.attr("albCFList");
    if (conceptFamille != undefined && conceptFamille !== "") {
        if (lstTitle !== "")
            lstTitle += "\n";
        var selConFam = GetConceptFamille(conceptFamille);
        if (lstTitle.indexOf(selConFam) == -1)
            lstTitle += selConFam;
    }
    elem.attr("title", lstTitle);
}

function CopySelectedTitleToElem(elem) {
    if (!elem) { return; }
    if (elem instanceof jQuery) {
        elem = elem[0];
    }
    var lstTitle = elem.title || "";
    if (elem instanceof HTMLSelectElement) {
        var selectedOption;
        if (elem.selectedOptions) {
            selectedOption = elem.selectedOptions[0];
        } else if (!elem.multiple) {
            selectedOption = elem.options[elem.selectedIndex];
        } else {
            selectedOption = Array.prototype.filter.call(elem.options, function (x) { return x.selected; })[0];
        }
        if (selectedOption !== undefined) {
            lstTitle = selectedOption.title || "";
        }
        var fam = (elem.attributes.albcflist || {}).value;
        if (fam) {
            if (lstTitle !== "")
                lstTitle += "\n";
            var selConFam = GetConceptFamille(fam);
            if (lstTitle.indexOf(selConFam) == -1)
                lstTitle += selConFam;
        }
        elem.title = lstTitle;
    }

}
//--------Desactive les raccourcis clavier des boutons--------
function DesactivateShortCut(target) {
    if (target === "" || target == undefined) {
        $("button[albShortcut=true]").each(function () {
            var shortCut = $(this).attr("data-accesskey");
            $(this).attr("data-accesskey", "#**#" + shortCut);
            $(this).attr("albShortcut", "false");
        });
    }
    else {
        //La target est utilisée dans le cas d'empilement complexe de div flottante (voir connexité par ex)
        $("button[albShortcut=true][albTargetScreen=" + target + "]").each(function () {
            var shortCut = $(this).attr("data-accesskey");
            $(this).attr("data-accesskey", "#**#" + shortCut);
            $(this).attr("albShortcut", "false");
        });
    }
}

//-------Reactive les raccourcis clavier des boutons----------
function ReactivateShortCut(target) {
    if (target === "" || target == undefined) {
        $("button[albShortcut=false]").each(function () {
            if ($(this).attr("albTargetScreen") == undefined) {
                var accessKey = $(this).attr("data-accesskey");
                if (accessKey != undefined) {
                    var shortCut = accessKey.split("#**#")[1];
                    if (shortCut != undefined) {
                        $(this).attr("data-accesskey", shortCut);
                        $(this).attr("albShortcut", "true");
                    }
                }
            }
        });
    }
    else {
        //La target est utilisée dans le cas d'empilement complexe de div flottante (voir connexité par ex)
        $("button[albShortcut=false][albTargetScreen=" + target + "]").each(function () {
            var shortCut = ($(this).attr("data-accesskey") || (this).attr("data-accesskey")).split("#**#")[1];
            if (shortCut != undefined) {
                $(this).attr("data-accesskey", shortCut);
                $(this).attr("albShortcut", "true");
            }
        });
    }
}

//-----Map les éléments des connexités-----




function MapConnexites(context) {
    //gestion de l'affichage de l'écran en mode readonly

    var typeAffichage = $("#typAffichage").val();
    if ($("#OffreReadOnly").val() === "True" && $("#IsModifHorsAvn").val() === "False") {
        $("img[name=btnAjouter]").hide();
        $("img[name=btnSupprimer]").hide();
        $("textarea[name=txtCommentaires]").each(function () {
            $(this).attr("disabled", "disabled");
        });
    }
    if (typeAffichage == undefined || typeAffichage == "") {
        $("#commentairesEng").html($("#commentairesEng").html().replace(/&lt;br&gt;/ig, "\n"));
        toggleDescription($("#commentairesEng"));
    }
    else {
        $("#commentairesEngagement").html($("#commentairesEngagement").html().replace(/&lt;br&gt;/ig, "\n"));
        toggleDescription($("#commentairesEngagement"));
    }

    AlternanceLigne("EngagementBody", "", false, null);
    AlternanceLigne("EngagementBody2", "", false, null);
    AlternanceLigne("RemplacementBody", "", false, null);
    AlternanceLigne("InformationBody", "", false, null);
    AlternanceLigne("ResiliationBody", "", false, null);
    AlternanceLigne("RegularisationBody", "", false, null);
    AlternanceLigne("ConnexitePeriodesBody", "", false, null);
    AlternanceLigne("EngagementBody", "", false, null);
    AlternanceLigne("EngagementBody2", "", false, null);

    $("#divBodyEngagement2").offOn("scroll", function () {
        $("#divHeaderEngagement2").scrollLeft($("#divBodyEngagement2").scrollLeft());
        $("#divConnexeBody").scrollLeft($("#divBodyEngagement2").scrollLeft());
    });

    $("#divBodyBtn").offOn("scroll", function () {
        $("#divBodyEngagement1").scrollTop($("#divBodyBtn").scrollTop());
        $("#divBodyEngagement2").scrollTop($("#divBodyBtn").scrollTop());

    });

    $("img[name=btnSupprimer]").each(function () {
        $(this).click(function () {
            $("#hiddenInputId").val($(this).attr("id"));
            ShowCommonFancy("ConfirmTrans", "Suppr",
                "Vous allez supprimer la connexité de ce contrat avec les autres contrats. Voulez-vous continuer ?",
                350, 130, true, true);
        });
    });

    $("img[name=btndetails]").each(function () {
        $(this).click(function () {
            $("#hiddenInputId").val($(this).attr("id"));
            OpenDetails();
        });
    });

    MapLinkWinOpen(false, context);
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '0');
    MapPeriodesConnexites();

}

function MapPeriodesConnexites() {

    synchronizeScrollbars();
    addPeriodeConnexion();
    editPeriodeConnexion();
    cancelPeriodeConnexion();
    deletePeriodeConnexion();
    savePeriodeConnexion();
    $("#tblConnexitePeriodesBody  .datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '0');
}

function synchronizeScrollbars() {
    // Horizontale
    $('.divTopCnxCenter').offOn("scroll", function (e) {
        var scrollLeft = e.target.scrollLeft;

        $('.divTopCnxEnCours').scrollLeft(scrollLeft);
        $('.divBottomCnxCenter').scrollLeft(scrollLeft);

        $('.divTopCnxCenterHeader').scrollLeft(scrollLeft);
        $('.divBottomCnxCenterHeader').scrollLeft(scrollLeft);
    });


    $('.divBottomCnxCenter').offOn("scroll", function (e) {
        var scrollLeft = e.target.scrollLeft;

        $('.divTopCnxEnCours').scrollLeft(scrollLeft);
        $('.divTopCnxCenter').scrollLeft(scrollLeft);

        $('.divTopCnxCenterHeader').scrollLeft(scrollLeft);
        $('.divBottomCnxCenterHeader').scrollLeft(scrollLeft);
    });

    // Verticale
    $('.divTopCnxRight').offOn("scroll", function (e) {
        var scrollTop = e.target.scrollTop;

        $('.divTopCnxLeft').scrollTop(scrollTop);
        $('.divTopCnxCenter').scrollTop(scrollTop);
    });

    $('.divBottomCnxRight').offOn("scroll", function (e) {
        var scrollTop = e.target.scrollTop;

        $('.divBottomCnxLeft').scrollTop(scrollTop);
        $('.divBottomCnxCenter').scrollTop(scrollTop);
    });
}

function addPeriodeConnexion() {
    $("#btnAjouterPeriodeCnx").kclick(function () {
        var id = -1;
        //Ajout d'une ligne avec des inputs al'index 0 dans tblConnexitePeriodesBody,tblConnexitePeriodesBodyTraites,tblConnexitePeriodesBodyActions
        $("#newPeriodeCnx").css("display", "block");
        $("#newPeriodeCnxTraites").css("display", "block");
        $("#newPeriodeCnxActions").css("display", "block");

        //Désactiver tous les boutonSupprimer,modifier
        $("img[name=btnModifierPeriodeCnxEng]").css("display", "none");
        $("img[name=btnSupprimerPeriodeCnxEng]").css("display", "none");



        //Afficher les bouton Save et annuler pour la ligne 0
        $("#tblConnexitePeriodesBodyActions tr[albid=" + id + "] td img[name=btnEnregistrerPeriodeCnxEng]").css("display", "inline");
        $("#tblConnexitePeriodesBodyActions tr[albid=" + id + "] td img[name=btnAnnulerPeriodeCnxEng]").css("display", "inline");
    });
}


function getIdentfier(e) {
    return $(($(e.currentTarget).parents().closest("tr"))[0]).attr("albid");
}

function editPeriodeConnexion() {

    $("img[name=btnModifierPeriodeCnxEng]").kclick(function (e) {

        var id = getIdentfier(e);

        //Désactiver tous les boutonSupprimer,modifier
        $("#tblConnexitePeriodesBodyActions tr td").find(
            "img[name=btnModifierPeriodeCnxEng]"
            + ",img[name=btnSupprimerPeriodeCnxEng]"
            + ",img[name=btnEnregistrerPeriodeCnxEng]"
            + ",img[name = btnAnnulerPeriodeCnxEng]").css("display", "none");


        //Cacher les libellé et aficcher les inputs

        const cnx = $("#tblConnexitePeriodesBody tr[albid=" + id + "] td");
        tr.find("span").hide();
        tr.find("input").show();

        const cnxTraite = $("#tblConnexitePeriodesBodyTraites tr[albid = " + id + "] td ");

        cnxTraite.find("span").hide();
        cnxTraite.find("input").show();

        //Afficher les bouton Save et annuler pour la ligne courante
        $("#tblConnexitePeriodesBodyActions tr[albid=" + id + "] td "
        ).find("img[name=btnEnregistrerPeriodeCnxEng],img[name=btnAnnulerPeriodeCnxEng]").show();


    });
}

function savePeriodeConnexion() {

    $("img[name=btnEnregistrerPeriodeCnxEng]").kclick(function (e) {
        //Si eurreur de validation affiche le message d'eurreur

        //Post le résultat au Controlleur Connexites :Enregistrer Periode de connexité
        var id = getIdentfier(e);

        var code = id;
        var dateDeb = $("#tblConnexitePeriodesBody  tr[albid=" + id + "] td input[name=dateDebut]").val();
        var dateFin = $("#tblConnexitePeriodesBody  tr[albid=" + id + "] td input[name=dateFin]").val();

        if (!checkDate($("#tblConnexitePeriodesBody  tr[albid=" + id + "] td input[name=dateDebut]"), $("#tblConnexitePeriodesBody  tr[albid=" + id + "] td input[name=dateFin]"))) {
            $("#tblConnexitePeriodesBody  tr[albid=" + id + "] td input[name=dateDebut]").addClass('requiredField');
            $("#tblConnexitePeriodesBody  tr[albid=" + id + "] td input[name=dateFin]").addClass('requiredField');
            return;
        }
        else {
            $("#tblConnexitePeriodesBody  tr[albid=" + id + "] td input[name=dateDebut]").removeClass('requiredField');
            $("#tblConnexitePeriodesBody  tr[albid=" + id + "] td input[name=dateFin]").removeClass('requiredField');
        }


        var traites = new Array();

        $("#tblConnexitePeriodesBodyTraites tr[albid=" + id + "] td input").each(function () {
            traites.push({ CodeEngagement: $(this).attr("albfam"), "ValeurEngagement": $(this).autoNumeric('get') });
        });

        var dataToPost = { Code: code, DateDebut: dateDeb, DateFin: dateFin, ListeDeTraites: traites, CodeOffre: $("#Offre_CodeOffre").val(), Version: $("#Offre_Version").val(), Type: $("#Offre_Type").val() };

        ShowLoading();
        $.ajax({
            url: "/Connexite/EnregistrerPeriodeConnexite",
            type: "POST",
            data: JSON.stringify(dataToPost),
            contentType: "application/json",
            success: function (data) {
                //Raffraichir les resultas
                $('#divPeriodesConnexites').html(data);
                MapPeriodesConnexites();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });

        //Cacher tous les inputs de Modif
        $("#tblConnexitePeriodesBody tr[albid=" + id + "] td span").css("display", "inline");
        $("#tblConnexitePeriodesBody tr[albid=" + id + "] td input").css("display", "none");

        $("#tblConnexitePeriodesBodyTraites tr[albid=" + id + "] td span").css("display", "inline");
        $("#tblConnexitePeriodesBodyTraites tr[albid=" + id + "] td input").css("display", "none");

        //Cacher les bouton annuler et enregistrer pour la ligne courante
        $("#tblConnexitePeriodesBodyActions tr[albid=" + id + "] td img[name=btnEnregistrerPeriodeCnxEng]").css("display", "none");
        $("#tblConnexitePeriodesBodyActions tr[albid=" + id + "] td img[name=btnAnnulerPeriodeCnxEng]").css("display", "none");

        //Afficher tous les boutons supprimer et modifier
        $("#tblConnexitePeriodesBodyActions tr td img[name=btnModifierPeriodeCnxEng]").css("display", "inline");
        $("#tblConnexitePeriodesBodyActions tr td img[name=btnSupprimerPeriodeCnxEng]").css("display", "inline");
    });



}

function deletePeriodeConnexion() {

    $("img[name=btnSupprimerPeriodeCnxEng]").kclick(function (e) {
        var id = getIdentfier(e);

        //Afficher une message de confirmation
        if (confirm("Etes-vous sûr de vouloir supprimer cette période ?")) {
            //Si oui Post au Controlleur des connexites
            var dataToPost = { Code: id, CodeOffre: $("#Offre_CodeOffre").val(), Version: $("#Offre_Version").val(), Type: $("#Offre_Type").val() };

            ShowLoading();
            $.ajax({
                url: "/Connexite/SupprimerPeriodeConnexite",
                type: "POST",
                data: JSON.stringify(dataToPost),
                contentType: "application/json",
                //dataType: "json",
                success: function (data) {
                    //Raffraichir les resultas
                    $('#divPeriodesConnexites').html(data);
                    MapPeriodesConnexites();
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });

        }

    });
}

function cancelPeriodeConnexion() {

    $("img[name=btnAnnulerPeriodeCnxEng]").kclick(function (e) {

        var id = getIdentfier(e);

        //Cacher les bouton annuler et enregistrer pour la ligne courante
        $("#tblConnexitePeriodesBodyActions tr[albid=" + id + "] td img[name=btnEnregistrerPeriodeCnxEng]").css("display", "none");
        $("#tblConnexitePeriodesBodyActions tr[albid=" + id + "] td img[name=btnAnnulerPeriodeCnxEng]").css("display", "none");

        //Cacher tous les inputs de Modif
        $("#tblConnexitePeriodesBody tr[albid=" + id + "] td span").css("display", "inline");
        $("#tblConnexitePeriodesBody tr[albid=" + id + "] td input").css("display", "none");

        $("#tblConnexitePeriodesBodyTraites tr[albid=" + id + "] td span").css("display", "inline");
        $("#tblConnexitePeriodesBodyTraites tr[albid=" + id + "] td input").css("display", "none");

        //Afficher tous les boutons supprimer et modifier
        $("#tblConnexitePeriodesBodyActions tr td img[name=btnModifierPeriodeCnxEng]").css("display", "inline");
        $("#tblConnexitePeriodesBodyActions tr td img[name=btnSupprimerPeriodeCnxEng]").css("display", "inline");


        $("#tblConnexitePeriodesBody  tr[albid=" + id + "] td input[name=dateDebut]").removeClass('requiredField');
        $("#tblConnexitePeriodesBody  tr[albid=" + id + "] td input[name=dateFin]").removeClass('requiredField');



    });
}

function MapLinkWinOpen(read, contexte) {

    if (contexte == "" || contexte == undefined) {
        $("td[name=linkWinOpen]").each(function () {
            $(this).click(function () {
                WinOpen($(this), read);
            });
        });
    }
    else {

        $("td[name=linkWinOpen][albcontext='" + contexte + "']").each(function () {
            $(this).click(function () {
                WinOpen($(this), read);
            });
        });
    }
    $("div[name=linkWinOpenOrigine], td[name=linkWinOpenOrigine], span[name=linkWinOpenOrigine]").kclick(function () {
        WinOpenOrigine($(this), read);
    });

    $("div[name=linkOpenContratOrigine], td[name=linkOpenContratOrigine], span[name=linkOpenContratOrigine]").kclick(function () {
        // $(this).attr('id').split("_")[1]);
        var id = $(this).attr('id');
        WinOpenContratOrigine($(this), read, id);
    });
}


function WinOpenDoc(action) {
    if (action !== "/") {
        ///En DEV, on n'a pas besoin de la form, passage de l'url directement
        ///dans le window.open
        window.open(action, "newWin");
    }
    else {
        ///En QUALIF, la form est nécessaire pour simuler le submit de celle-ci
        ///pour récupérer l'input paramWinOpen en Request
        window.open("about:blank", "newWin");
        $("#formWinOpen").attr("action", action).submit();
    }
}
function WinOpen(e, read) {
    var tabreadOnly = (read === "" || read == undefined || read === "O") ? "tabGuidtabGuidConsultOnly" : "tabGuidtabGuid";
    var value = e.attr("albparam").replace("ConsultOnly", tabreadOnly);
    var action = $("#urlWinOpen").val();
    $("#paramWinOpen").val(action + value);
    OpenWindowWithPost("http://" + $("#urlHost").val(), "__newTab", "NewWindowIframe", $.trim($("#paramWinOpen").val()));
}
function WinOpenOrigine(e, read) {

    var tabreadOnly = (read === "" || read == undefined || read === "O") ? "tabGuidtabGuidConsultOnly" : "tabGuidtabGuid";
    $("#paramWinOpen").val(e.attr("albparam").replace("ConsultOnly", tabreadOnly));
    OpenWindowWithPost("http://" + $("#urlHost").val(), "__newTab", "NewWindowIframe", $.trim($("#paramWinOpen").val()));
}
function WinOpenContratOrigine(e, read, id) {

    var tabreadOnly = (read === "" || read == undefined || read === "O") ? "tabGuidtabGuidConsultOnly" : "tabGuidtabGuid";
    $("#paramWinOpen").val(e.attr("albparam"));
    OpenWindowWithPost("http://" + $("#urlHost").val(), "__newTab", "NewWindowIframe", $.trim($("#paramWinOpen").val()) + tabreadOnly);
}

//-----Ouvre/Cache le textarea de description---
//-----callback : (bool) active ou non l'appel de fonctions (à définir dans le script local)
//-----après l'ouverture et la fermeture de la textarea
function toggleDescription(input, displayTop, callBack) {
    $("div[name=txtAreaLnk]").each(function () {
        $(this).die("click");
        $(this).kclick(function (e) {
            //Empeche l'execution multiple
            e.preventDefault();
            if (e.handled === true) return false;
            e.handled = true;
            if ($(this).hasClass("CursorPointer")) {
                var context = $(this).attr("albContext");
                var position = $(this).offset();
                var posTop = 0;
                var description;
                var heightD;
                var widthD;
                if (context != undefined && context !== "") {
                    if ($("#OffreReadOnly").val() === "True")
                        description = $("#" + context).html();
                    else description = $("#" + context).val();
                    description = description === "" ? "" : $.trim(description);
                    description = description.replace(/<\/p><p>/ig, "<br/>");
                    description = description.replace(/<\/p>/ig, "");
                    description = description.replace(/<p>/ig, "");
                    description = description.replace(/\n/ig, "<br/>");
                    heightD = $("#txtArea[albContext=" + context + "]").height();
                    widthD = $("#txtArea[albContext=" + context + "]").width();
                    if (displayTop)
                        posTop = position.top - heightD - 2;
                    else
                        posTop = position.top + 25;

                    if ($("#txtArea[albContext=" + context + "]").is(":visible")) {
                        $("#zoneTxtArea[albContext=" + context + "]").html(description);
                    }
                    else {
                        $("#" + context).val($.trim($("#zoneTxtArea[albContext=" + context + "]").html().replace(/<br>/ig, "\n")));
                    }


                    $("#txtArea[albContext=" + context + "]").css({ 'position': "fixed", 'top': posTop + "px", 'left': position.left - widthD + 24 + "px" }).toggle();
                    $("#" + context).focus();
                    $("#" + context).val($("#" + context).val());

                    if (callBack) {
                        if (!$("#txtArea[albContext=" + context + "]").is(":visible"))
                            callBackFunctionOnHide(context);
                        else
                            callBackFunctionOnDisplay(context);
                    }
                }
                else {
                    let newTextInput = null;
                    if ($("#txtArea").is(":visible")) {
                        //let d = $(this).closest(".divDescriptif").siblings("#txtArea").find("div div");
                        //newTextInput = d.legnth > 0 ? d : $(this).closest(".divDescriptif").siblings("#txtArea").find("div textarea");
                        let d = $(this).siblings("#txtArea").find("div div");
                        newTextInput = d.legnth > 0 ? d : $(this).siblings("#txtArea").find("div textarea");
                    }
                    else {
                        newTextInput = $(this).siblings("#zoneTxtArea");
                    }

                    if ($("#OffreReadOnly").val() === "True")
                        description = newTextInput.html();
                    else description = newTextInput.val();
                    description = description === "" ? "" : $.trim(description);
                    description = description.replace(/<\/p><p>/ig, "<br/>");
                    description = description.replace(/<\/p>/ig, "");
                    description = description.replace(/<p>/ig, "");
                    description = description.replace(/\n/ig, "<br/>");
                    heightD = $("#txtArea").height();
                    widthD = $("#txtArea").width();
                    if (displayTop)
                        posTop = position.top - heightD - 2;
                    else
                        posTop = position.top + 25;

                    if ($("#txtArea").is(":visible")) {
                        $("#zoneTxtArea").html(description);
                    }
                    else {
                        newTextInput.val($.trim($("#zoneTxtArea").html().replace(/<br>/ig, "\n")));
                        newTextInput.focus();
                    }

                    $("#txtArea").css({ 'position': "absolute", 'top': posTop + "px", 'left': position.left - widthD + 24 + "px" }).toggle();
                    //newTextInput.val(newTextInput.val());
                    if (callBack) {
                        if (!$("#txtArea").is(":visible"))
                            callBackFunctionOnHide();
                        else
                            callBackFunctionOnDisplay();
                    }
                }
            }
            return false;
        });
    });
}

//-----Formate les textarea en Editor
function FormatCLEditor(input, clWidth, clHeight, clIndex) {
    if (clWidth == null || clHeight == null) {
        clWidth = 502;
        clHeight = 100;
    }
    var editObs = input.cleditor({ width: clWidth, height: clHeight, controls: "bold italic underline | bullets numbering | outdent indent | alignleft center alignright" });
    if (clIndex != null)
        $(editObs[0].$frame).attr("tabindex", clIndex);
}

$('#btnEnregistrerVisualisationObservations').kclick(function testUpdateObsv() {
    $.ajax({
        url: "/CommonNavigation/UpdateObsv",
        type: "POST",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), type: $("#Offre_Type").val(), version: $("#Offre_Version").val(), obsvInfoGen: $("#ObsvInfoGen").val(),
            obsvCotisation: $("#ObsvCotisation").val(), obsvEngagement: $("#ObsvEngagement").val(), obsvMntRef: $("#ObsvMntRef").val(), obsvRefGest: $("#ObsvRefGest").val()
        },
        success: function (data) {
            $("#divDataVisuObservations").html("");
            $("#divFullScreenVisuObservations").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
});

//--------------------Télècharger un fichier---------------------
function UploadDocuments(isload) {
    if (isload === true)
        return;
    $.ajax({
        url: "/CommonNavigation/GetAllDownLoadFiles",
        type: "POST",
        // Form data
        data: { path: $("#typeDoc").val(), prefixFiles: $("#codeDoc").val() },
        // data: formData,
        success: function (data) {
            $("#tblTableFichier").html(data);
        }
        //Options to tell JQuery not to process data or worry about content-type
        //cache: false,
        //contentType: false,
        //processData: false
    });
}
//---------------------- Confirm Suppression d'une piece jointe, de l'inventaire -------------------------
function ConfirmDeleteFile(e) {
    if ($("#OffreReadOnly").val() !== "True") {
        var param = e.id.replace(/\*/g, " ").replace("~", ".");
        $("#hiddenInputId").val(e.id);

        ShowCommonFancy("Confirm", "File",
            "Etes-vous de vouloir supprimer le fichier :\n\n" + param,
            350, 130, true, true);
    }
}
//-- Suppression d'une piece jointe, de l'inventaire
function DeleteFile(id) {
    var invContexte = $("#codeDoc").val();
    var fileNameHtmWithExtension = id.replace("~", ".");
    var fileName = fileNameHtmWithExtension.replace(/\*/g, " ");
    var sepChar = $("#SplitFileChar").val();
    var idRemove = id.replace(/\*/g, "").replace(".", "").replace("~", "");

    $.ajax({
        type: "POST",
        url: "/CommonNavigation/Suppression",
        data: { path: $("#typeDoc").val(), fileName: invContexte + sepChar + fileNameHtmWithExtension },
        success: function () {
            $("tr[name=tr" + idRemove + "]").remove();
            $("#file").clear();
            common.dialog.info("Le fichier\n\n<b>" + fileName + "</b> à bien été supprimé.", { width: "auto", height: "auto", fixed: false });
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//---Ouvre la liste des offres verrouillées----
function OpenLockedOffre() {
    common.page.isLoading = true;
    try {
        common.$postJson("/OffresVerrouillees/GetVerrousAffaires", { currentTab: (infosTab.tabGuid || null) }, true).done(function (data) {
            $("#divDataUserOffreLocked").html(data);
            $("#divUserOffreLocked").show();
            MapElementUserOffreLocked();
            AlternanceLigne("BodyParamCache", "", false, null);
            common.page.isLoading = false;
        });
    }
    catch (e) {
        console.error(e);
        common.error.showMessage(e.message);
    }
}
//-----Map les éléments de la div des offre verrouillées----
function MapElementUserOffreLocked() {
    $("#btnFancyFermer").kclick(function () {
        $("#divDataUserOffreLocked").html("");
        $("#divUserOffreLocked").hide();
    });
    $("#btnSupprimerCache").kclick(function () {
        if ($("input[name='checkOffreCache']:checked").length > 0) {
            DeleteUserOffreCache();
        }
        else {
            common.error.show("Veuillez faire un choix d'offres et de contrats à déverrouiller.");
        }
    });
    $("#allCacheCheckbox").offOn("change", function () {
        if ($(this).is(":checked"))
            $("input[type=checkbox][name=checkOffreCache]").attr("checked", "checked");
        else
            $("input[type=checkbox][name=checkOffreCache]").removeAttr("checked");
    });
}
//--------Supprime les offres du cache--------------
function DeleteUserOffreCache() {
    var offres = "";
    $("input[type=checkbox][name=checkOffreCache]").each(function () {
        if ($(this).is(":checked"))
            offres += $("#splitCharHtml").val() + $(this).attr("albOffreCache");
    });
    if (offres !== "")
        offres = offres.substr(4);

    $.ajax({
        type: "POST",
        url: "/OffresVerrouillees/DeleteOffreCache",
        data: { offres: offres, codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), numAvenant: $("#NumAvenantPage").val(), onlyUser: 1 },
        success: function (data) {
            $("#divDataUserOffreLocked").html(data);
            $("#divUserOffreLocked").show();
            AlternanceLigne("BodyParamCache", "", false, null);
            MapElementUserOffreLocked();
            if ($("#tblBodyParamCache").html() === "" || $("#tblBodyParamCache").html() == undefined)
                $("#divOffreLocked").html("");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


//-----------Formate automatiquement la date----------
function FormatAutoDate(elem, customClassName) {
    if (!customClassName) {
        customClassName = "requiredField";
    }

    elem.removeClass(customClassName);

    var isDateLike = false;
    var dateInput = elem.val();

    var dayInput = "";
    var monthInput = "";
    var yearInput = "";

    if (dateInput.replace(/\//g, "") !== dateInput) {
        var tabInput = dateInput.split("/");
        if (tabInput.length === 3) {
            if (tabInput[0].length === 2 && tabInput[1].length === 2 && (tabInput[2].length === 2 || tabInput[2].length === 4)) {
                dayInput = tabInput[0];
                monthInput = tabInput[1];
                if (tabInput[2].length === 2) {
                    if (parseInt(tabInput[2]) < 70)
                        yearInput = "20" + tabInput[2];
                    else
                        yearInput = "19" + tabInput[2];
                }
                else
                    yearInput = tabInput[2];
                isDateLike = true;
            }
        }
    }
    else {
        if (dateInput.length === 8 || dateInput.length === 6) {
            dayInput = dateInput.substr(0, 2);
            monthInput = dateInput.substr(2, 2);
            if (dateInput.length === 6) {
                if (parseInt(dateInput.substr(4, 2)) < 70)
                    yearInput = "20" + dateInput.substr(4, 2);
                else
                    yearInput = "19" + dateInput.substr(4, 2);
            }
            else
                yearInput = dateInput.substr(4, 4);
            isDateLike = true;
        }
    }

    if (!isDateLike) {
        elem.val("").addClass(customClassName);
        ShowCommonFancy("Error", "", "Erreur de formatage de date.<br/>Formats acceptés : DD/MM/YYYY ; DDMMYYYY ; DD/MM/YY ; DDMMYY", 300, 150, true, true, true);
    }
    else {
        if (isDate(dayInput + "/" + monthInput + "/" + yearInput))
            elem.val(dayInput + "/" + monthInput + "/" + yearInput);
        else
            elem.addClass(customClassName);
    }
}
//------------Vérifie la date-------------
function isDate(txtDate) {
    var currVal = txtDate;
    if (currVal === "" || currVal === undefined)
        return false;

    var dateRegExp = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(dateRegExp);

    if (dtArray == null)
        return false;

    var dtDay = parseInt(dtArray[1]);
    var dtMonth = parseInt(dtArray[3]);
    var dtYear = parseInt(dtArray[5]);

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var bisextileYear = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay === 29 && !bisextileYear))
            return false;
    }

    /*Vérification de la date +/- 200 ans*/
    if (getDate(txtDate) > getDate($("#DateMax").val()) || getDate(txtDate) < getDate($("#DateMin").val())) {
        return false;
    }

    return true;
}

//----------Formate le mode de navigation pour le passage du paramètre par l'URL-------
function GetFormatModeNavig(modeNavig) {
    return "modeNavig" + modeNavig + "modeNavig";
}


//***********************************Split les chaines de caractères**********************
function AlbJsSplitElem(strData, position, splitChar) {

    if (strData == undefined || strData === "")
        return "noData";
    var splitArray = strData.split(splitChar);

    if (splitArray == null || splitArray.length < 0) {
        return "noData";
    }
    if (splitArray.length <= position) {
        return "noData";
    }
    return splitArray[position];
}
function AlbJsSplitArray(strData, splitChar) {
    if (strData == undefined || strData === "")
        return "noData";
    var splitArray = strData.split(splitChar);
    if (splitArray == null || splitArray.length < 0) {
        return "noData";
    }

    return splitArray;
}

//--------Expression régulière des codes--------------
function AffectRegExpCode() {
    $("input[albRegExp=codeRegExp]").live("focus", function () {
        $("#oldCodeRegExp").val($(this).val());
    });
    $(".codeRegExp").live("keyup", function () {
        if (!$(this).val().match(/^[0-9a-z]*$/i))
            $(this).val($("#oldCodeRegExp").val()).focus();
        else
            $("#oldCodeRegExp").val($(this).val());
    });
}

//-------Ouverture de la suppression d'une offre/version--------------
function OpenDivSupprOffre() {
    $("#delCodeOffre").val("");
    $("#delVersionOffre").val("0");
    $("#divDelOffre").show();
    $("#btnDelValider").kclick(function () {
        DeleteOffre();
    });
    $("#btnDelAnnuler").kclick(function () {
        $("#divDelOffre").hide();
    });
}
//--------Vérifie que les champs pour la suppression d'offre------
//----------------------ont bien été renseignés-------------------
function CheckFieldDelOffre() {
    $(".requiredField").removeClass("requiredField");
    var error = false;
    if ($("#delCodeOffre").val() === "") {
        $("#delCodeOffre").addClass("requiredField");
        error = true;
    }
    if ($("#delVersionOffre").val() === "") {
        $("#delVersionOffre").addClass("requiredField");
        error = true;
    }

    return error;
}
//--------Suppression d'une offre/version------------
function DeleteOffre() {
    var fieldsError = CheckFieldDelOffre();
    if (fieldsError)
        return;

    var typeTxt = $("input[type='radio'][name='delType']:checked").val() === "O" ? "L'offre" : "Le contrat";

    $.ajax({
        type: "POST",
        url: "/BackOffice/DeleteOffre",
        data: { codeOffre: $("#delCodeOffre").val(), version: $("#delVersionOffre").val(), type: $("input[type='radio'][name='delType']:checked").val() },
        success: function (data) {
            if (data !== "") {
                common.dialog.error(typeTxt + " " + data);
            }
            else {
                common.dialog.info(typeTxt + " a bien été supprimé(e)");
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Check si un input existe---------
function CheckExistInput(input) {
    return input.length > 0;
}


function testRecupLstContrat() {
    $("[name^='infos-contrats']").kclick(function (e0) {
        const divId = "contrats";
        let $this = $(this);
        let $div = $("#" + divId);
        if (e0.target.id === divId || $div.find(e0.target).exists()) {
            e0.preventDefault();
            return;
        }
        const position = $this.position();
        if ($div.exists() && $div.isVisible()) {
            $div.hide().clearHtml();
            $(document).off("click", "#divBlocsSynthese");
        }
        else {
            if (!$div.exists()) {
                $div = $("#divConteneurBoiteDialogInfo").clone();
                $div.hide();
                $div.attr("id", divId);
                $div.css({ position: "absolute", width: 1, height: 1, top: "175px", left: "529px" });
                $div.appendTo(this);
            }
            let typeCabinet = divId;
            let code = parseInt($("#Offre_CodeOffre").val());
            let version = $("#Offre_Version").val();
            let codeInterlocuteur = $("input[albcontext=" + typeCabinet + "][albIdInterlocuteur]").val() || $("#CodeInterlocuteur").val();
            if (typeCabinet != undefined && code != undefined && code !== "" && typeCabinet !== "") {
                if (codeInterlocuteur == undefined || codeInterlocuteur === "" || codeInterlocuteur === "0") {
                    codeInterlocuteur = 0;
                }
                common.page.isLoading = true;
                let postData = { codeOffre: code, version: version };
                common.$postJson("/SyntheseAffaire/getContrat", postData, true).done(function (data) {
                    $div.html(data);
                    if (parseInt($div.css("left")) === 0) {
                        let l = position.left - $div.children().first().outerWidth();
                        if (l < 0) {
                            l = 5;
                        }
                        $div.css({ left: l });
                    }

                    divWidth = $("#BoiteContratsOffre").width();
                    $("#BoiteContratsOffre").show();
                    common.dom.pushForward($("#BoiteContratsOffre"));


                    //common.dom.pushForward($this);
                    $("#divBlocsSynthese").kclick(function (e) {
                        if (divId === e.target.id || $div.find(e.target).exists()) {
                            e.preventDefault();
                            return;
                        }
                        if (e.target === $this.get(0)) {
                            return true;
                        }
                        $div.clearHtml().hide();
                        $(document).off("click", "#divBlocsSynthese");
                    });
                    $div.show();
                    common.page.isLoading = false;
                });
            }
        }
    });
}




function testRecupLstContrat() {
    $("[name^='infos-contrats']").kclick(function (e0) {
        const divId = "contrats";
        let $this = $(this);
        let $div = $("#" + divId);
        if (e0.target.id === divId || $div.find(e0.target).exists()) {
            e0.preventDefault();
            return;
        }
        const position = $this.position();
        if ($div.exists() && $div.isVisible()) {
            $div.hide().clearHtml();
            $(document).off("click", "#divBlocsSynthese");
        }
        else {
            if (!$div.exists()) {
                $div = $("#divConteneurBoiteDialogInfo").clone();
                $div.hide();
                $div.attr("id", divId);
                $div.css({ position: "absolute", width: 1, height: 1, top: "175px", left: "529px" });
                $div.appendTo(this);
            }
            let typeCabinet = divId;
            let code = parseInt($("#Offre_CodeOffre").val());
            let version = $("#Offre_Version").val();
            let codeInterlocuteur = $("input[albcontext=" + typeCabinet + "][albIdInterlocuteur]").val() || $("#CodeInterlocuteur").val();
            if (typeCabinet != undefined && code != undefined && code !== "" && typeCabinet !== "") {
                if (codeInterlocuteur == undefined || codeInterlocuteur === "" || codeInterlocuteur === "0") {
                    codeInterlocuteur = 0;
                }
                common.page.isLoading = true;
                let postData = { codeOffre: code, version: version };
                common.$postJson("/SyntheseAffaire/getContrat", postData, true).done(function (data) {
                    $div.html(data);
                    if (parseInt($div.css("left")) === 0) {
                        let l = position.left - $div.children().first().outerWidth();
                        if (l < 0) {
                            l = 5;
                        }
                        $div.css({ left: l });
                    }

                    divWidth = $("#BoiteContratsOffre").width();
                    $("#BoiteContratsOffre").show();
                    common.dom.pushForward($("#BoiteContratsOffre"));


                    //common.dom.pushForward($this);
                    $("#divBlocsSynthese").kclick(function (e) {
                        if (divId === e.target.id || $div.find(e.target).exists()) {
                            e.preventDefault();
                            return;
                        }
                        if (e.target === $this.get(0)) {
                            return true;
                        }
                        $div.clearHtml().hide();
                        $(document).off("click", "#divBlocsSynthese");
                    });
                    $div.show();
                    common.page.isLoading = false;
                });
            }
        }
    });
}


//--------Mappe les images permettant d'afficher les boites de dialogues des courtiers/assurés
function MapBoitesDialogue() {
    $("img[albIdInfo]").kclick(function () {
        const divId = "#divConteneurBoiteDialogInfo";
        let position = $(this).offset();
        let $div = $(divId);
        if ($div.isVisible()) {
            $div.hide().clearHtml();
        }
        else {
            let typeCabinet = $(this).attr("albcontext");
            let code = $(this).attr("albIdInfo");
            let codeInterlocuteur = $("input[albcontext=" + typeCabinet + "][albIdInterlocuteur]").val() || $("#CodeInterlocuteur").val();
            if (typeCabinet != undefined && code != undefined && code !== "" && typeCabinet !== "") {
                if (codeInterlocuteur == undefined || codeInterlocuteur === "" || codeInterlocuteur === "0") {
                    codeInterlocuteur = 0;
                }
                common.page.isLoading = true;
                $.ajax({
                    type: "POST",
                    url: "/CommonNavigation/GetCabinetDetails",
                    data: { code: code, codeInterlocuteur: codeInterlocuteur, typeCabinet: typeCabinet },
                    success: function (data) {
                        $div.html(data);
                        let divWidth;
                        if (typeCabinet === "courtierGestion") {
                            divWidth = $("#BoiteDialogCourtierGestionnaire").width();
                            $("#BoiteDialogCourtierGestionnaire").css({ 'position': "absolute", 'top': position.top + 25 + "px", 'left': position.left - divWidth + 20 + "px" }).show();
                            common.dom.pushForward($("#BoiteDialogCourtierGestionnaire"));
                        }
                        if (codeInterlocuteur === 0 && (typeCabinet === "courtierPayeur" || typeCabinet === "courtierApporteur")) {
                            divWidth = $("#BoiteDialogCourtierApporteurPayeur").width();
                            $("#BoiteDialogCourtierApporteurPayeur").css({ 'position': "absolute", 'top': position.top + 25 + "px", 'left': position.left - divWidth + 20 + "px" }).show();
                            common.dom.pushForward($("#BoiteDialogCourtierApporteurPayeur"));
                        }
                        if (codeInterlocuteur > 0 && (typeCabinet === "courtierPayeur" || typeCabinet === "courtierApporteur")) {
                            divWidth = $("#BoiteDialogCourtierGestionnaire").width();
                            $("#BoiteDialogCourtierGestionnaire").css({ 'position': "absolute", 'top': position.top + 25 + "px", 'left': position.left - divWidth + 20 + "px" }).show();
                            common.dom.pushForward($("#BoiteDialogCourtierGestionnaire"));
                        }
                        if (typeCabinet === "assure") {
                            divWidth = $("#BoiteDialogAssure").width();
                            $("#BoiteDialogAssure").css({ 'position': "absolute", 'top': position.top + 25 + "px", 'left': position.left - divWidth + 20 + "px" }).show();
                            common.dom.pushForward($("#BoiteDialogAssure"));
                        }
                        AddOnHideBoiteDialogue($div);
                        $div.show();
                        common.page.isLoading = false;
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });
            }
        }
    });

    $("[name^='infos-assure'], [name^='infos-courtier']").kclick(function (e0) {
        const name = $(this).attr("name");
        const splittedName = name.split('-');
        const divId = splittedName.join("_");
        let $this = $(this);
        let $div = $("#" + divId);
        if (e0.target.id === divId || $div.find(e0.target).exists()) {
            e0.preventDefault();
            return;
        }
        const position = $this.position();
        if ($div.exists() && $div.isVisible()) {
            $div.hide().clearHtml();
            $(document).off("click", "#divBlocsSynthese");
        }
        else {
            if (!$div.exists()) {
                $div = $("#divConteneurBoiteDialogInfo").clone();
                $div.hide();
                $div.attr("id", divId);
                $div.css({ position: "absolute", width: 1, height: 1, top: position.top + $this.outerHeight(), left: 0 });
                $div.appendTo(this);
            }
            let typeCabinet = splittedName[splittedName.length - 2];
            let code = splittedName.last();
            let codeInterlocuteur = $("input[albcontext=" + typeCabinet + "][albIdInterlocuteur]").val() || $("#CodeInterlocuteur").val();
            if (typeCabinet != undefined && code != undefined && code !== "" && typeCabinet !== "") {
                if (codeInterlocuteur == undefined || codeInterlocuteur === "" || codeInterlocuteur === "0") {
                    codeInterlocuteur = 0;
                }
                common.page.isLoading = true;
                let postData = { code: code, codeInterlocuteur: codeInterlocuteur, typeCabinet: typeCabinet };
                common.$postJson("/CommonNavigation/GetCabinetDetails", postData, true).done(function (data) {
                    $div.html(data);
                    if (parseInt($div.css("left")) === 0) {
                        let l = position.left - $div.children().first().outerWidth();
                        if (l < 0) {
                            l = 5;
                        }
                        $div.css({ left: l });
                    }
                    if (typeCabinet === "courtierGestion") {
                        divWidth = $("#BoiteDialogCourtierGestionnaire").width();
                        $("#BoiteDialogCourtierGestionnaire").show();
                        common.dom.pushForward($("#BoiteDialogCourtierGestionnaire"));
                    }
                    if (codeInterlocuteur === 0 && (typeCabinet === "courtierPayeur" || typeCabinet === "courtierApporteur")) {
                        divWidth = $("#BoiteDialogCourtierApporteurPayeur").width();
                        $("#BoiteDialogCourtierApporteurPayeur").show();
                        common.dom.pushForward($("#BoiteDialogCourtierApporteurPayeur"));
                    }
                    if (codeInterlocuteur > 0 && (typeCabinet === "courtierPayeur" || typeCabinet === "courtierApporteur")) {
                        divWidth = $("#BoiteDialogCourtierGestionnaire").width();
                        $("#BoiteDialogCourtierGestionnaire").show();
                        common.dom.pushForward($("#BoiteDialogCourtierGestionnaire"));
                    }
                    if (typeCabinet === "assure") {
                        divWidth = $("#BoiteDialogAssure").width();
                        $("#BoiteDialogAssure").show();
                        common.dom.pushForward($("#BoiteDialogAssure"));
                    }

                    //common.dom.pushForward($this);
                    $("#divBlocsSynthese").kclick(function (e) {
                        if (divId === e.target.id || $div.find(e.target).exists()) {
                            e.preventDefault();
                            return;
                        }
                        if (e.target === $this.get(0)) {
                            return true;
                        }
                        $div.clearHtml().hide();
                        $(document).off("click", "#divBlocsSynthese");
                    });
                    $div.show();
                    common.page.isLoading = false;
                });
            }
        }
    });
}
//----------fonction qui gère les évenements qui cachent les boites de dialogues
function AddOnHideBoiteDialogue($div) {
    function HideBoiteDialogue() {
        $div.clearHtml().hide();
    }

    $("body").off("click", HideBoiteDialogue).on("click", HideBoiteDialogue);

    $div.kclick(function (e) {
        e.preventDefault();
        return false;
    });
}
//-------------------- New Window with new Url src Iframe
function OpenWindowWithPost(url, windowoption, name, params) {
    CloseLoading();
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", url);
    form.setAttribute("target", "_blank");
    var input = document.createElement("input");
    input.type = "hidden";
    input.name = "id";
    input.id = "id";
    input.value = params;
    form.appendChild(input);
    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
}

//--------------Gestion des combo nomenclatures------
function GetComboNomenclature(nomenclature, init) {
    var niveau = nomenclature.attr("albNiveauCombo");
    if (niveau != "" && niveau != "I") {
        var numCombo = nomenclature.attr("id").split("InformationsGenerales_Nomenclature");
        if (numCombo != undefined && numCombo.length > 1)
            numCombo = numCombo[1];
        if (numCombo != undefined && numCombo != "")
            var numComboInt = parseInt(numCombo);

        var niveauInt = parseInt(niveau);

        if (numComboInt != undefined && niveauInt != undefined) {
            if (nomenclature.val() != "") {
                numComboInt += 1;
                niveauInt += 1;
                if ($("#InformationsGenerales_Nomenclature" + numComboInt).attr("albNiveauCombo") != undefined && ($("#InformationsGenerales_Nomenclature" + numComboInt).attr("albNiveauCombo") == niveauInt)) {
                    if (init == undefined) {
                        var idTypo = nomenclature.val();
                        GetContenuCombo(idTypo, numComboInt);
                        $("#InformationsGenerales_Nomenclature" + numComboInt).val("");
                        if ($("#OffreReadOnly").val() != "True") {
                            $("#InformationsGenerales_Nomenclature" + numComboInt).removeAttr("disabled").removeClass("readonly");
                        }
                    }
                    else {
                        if ($("#OffreReadOnly").val() != "True" && !$("#InformationsGenerales_Nomenclature" + numComboInt).attr("disabled")) {
                            $("#InformationsGenerales_Nomenclature" + numComboInt).removeAttr("disabled").removeClass("readonly");
                        }
                    }
                }

                while ($("#InformationsGenerales_Nomenclature" + numComboInt).attr("albNiveauCombo") != undefined && ($("#InformationsGenerales_Nomenclature" + numComboInt).attr("albNiveauCombo") == niveauInt)) {
                    numComboInt += 1;
                    niveauInt += 1;
                    if (init == undefined && $("#InformationsGenerales_Nomenclature" + numComboInt).attr("albNiveauCombo") != undefined && ($("#InformationsGenerales_Nomenclature" + numComboInt).attr("albNiveauCombo") == niveauInt)) {
                        $("#InformationsGenerales_Nomenclature" + numComboInt).val("");
                        $("#InformationsGenerales_Nomenclature" + numComboInt).attr("disabled", "disabled").addClass("readonly");
                    }
                    if ($("#OffreReadOnly").val() == "True")
                        $("#InformationsGenerales_Nomenclature" + numComboInt).attr("disabled", "disabled").addClass("readonly");
                }
            }
            else {
                while ($("#InformationsGenerales_Nomenclature" + numComboInt).attr("albNiveauCombo") != undefined && ($("#InformationsGenerales_Nomenclature" + numComboInt).attr("albNiveauCombo") == niveauInt)) {
                    numComboInt += 1;
                    niveauInt += 1;
                    if ($("#InformationsGenerales_Nomenclature" + numComboInt).attr("albNiveauCombo") != undefined && ($("#InformationsGenerales_Nomenclature" + numComboInt).attr("albNiveauCombo") == niveauInt)) {
                        $("#InformationsGenerales_Nomenclature" + numComboInt).attr("disabled", "disabled").addClass("readonly");
                        $("#InformationsGenerales_Nomenclature" + numComboInt).val("");
                    }
                    if ($("#OffreReadOnly").val() == "True") {
                        $("#InformationsGenerales_Nomenclature" + numComboInt).attr("disabled", "disabled").addClass("readonly");
                    }
                }
            }
        }
    }
}

//-----------Chargement du contenu d'une combo spécifique
function GetContenuCombo(idComboParent, numeroCombo) {
    var cible = $("#InformationsGenerales_Cible").val();

    let idNomenclature1 = $("select[id='InformationsGenerales_Nomenclature1']").val();
    let idNomenclature2 = $("select[id='InformationsGenerales_Nomenclature2']").val();
    let idNomenclature3 = $("select[id='InformationsGenerales_Nomenclature3']").val();
    let idNomenclature4 = $("select[id='InformationsGenerales_Nomenclature4']").val();
    let idNomenclature5 = $("select[id='InformationsGenerales_Nomenclature5']").val();

    $.ajax({
        type: "POST",
        url: "/DetailsRisque/LoadSpecificNomenclatureCombo",
        data: {
            IdNomenclatureParent: idComboParent, NumeroCombo: numeroCombo, cible: cible,
            idNom1: idNomenclature1, idNom2: idNomenclature2, idNom3: idNomenclature3, idNom4: idNomenclature4, idNom5: idNomenclature5
        },
        success: function (data) {
            if (data != null && data !== "")
                $("#divConteneurNomenclature" + numeroCombo).html(data);
            else {
                $("select[id='InformationsGenerales_Nomenclature" + numeroCombo).attr("disabled", "disabled").addClass("readonly");
                $("select[id='InformationsGenerales_Nomenclature" + numeroCombo).val("");
            }
            GetComboNomenclature($("#InformationsGenerales_Nomenclature" + numeroCombo));


            $("#InformationsGenerales_Nomenclature" + numeroCombo).change(function () {
                AffectTitleList($("#InformationsGenerales_Nomenclature" + numeroCombo));
                GetComboNomenclature($("#InformationsGenerales_Nomenclature" + numeroCombo));
            });
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

//-----------Rechargement des combo de nomenclature en fonction de la cible
function ReloadNomenclatureCombos(codeOffre, version, type, codeRisque, codeObjet, cible) {
    if (cible != undefined) {
        $.ajax({
            type: "POST",
            url: "/DetailsRisque/ReloadNomenclaturesCombo",
            data: { codeOffre: codeOffre, version: version, type: type, codeRisque: codeRisque, codeObjet: codeObjet, cible: cible },
            success: function (data) {
                $("#divConteneurNomenclatures").html(data);
                GetComboNomenclature($("#InformationsGenerales_Nomenclature1"));
                GetComboNomenclature($("#InformationsGenerales_Nomenclature2"));
                GetComboNomenclature($("#InformationsGenerales_Nomenclature3"));
                GetComboNomenclature($("#InformationsGenerales_Nomenclature4"));
                GetComboNomenclature($("#InformationsGenerales_Nomenclature5"));

                $("select[albNiveauCombo]").each(function () {
                    $(this).change(function () {
                        AffectTitleList($(this));
                        GetComboNomenclature($(this));
                    });
                });
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//---------Valide la suppression des docs sélectionnés--------(pour écrans choix clauses et controle fin)
function ValidSupprDoc() {
    var splitHtmlChar = $("#SplitHtmlChar").val();
    var selectDoc = "";
    var unselectDoc = "";

    $("input[type='checkbox'][name='checkbox']:checked").each(function () {
        var idDoc = AlbJsSplitElem($(this).attr("id"), 1, splitHtmlChar);
        if (idDoc !== "noData")
            selectDoc += idDoc + "|";
    });
    $("input[type='checkbox'][name='checkbox']:not(:checked)").each(function () {
        var idDoc = AlbJsSplitElem($(this).attr("id"), 1, splitHtmlChar);
        if (idDoc !== "noData")
            unselectDoc += idDoc + "|";
    });

    $.ajax({
        type: "POST",
        url: "/ValidationOffre/ValidSupprDoc",
        data: { selectDoc: selectDoc, unselectDoc: unselectDoc },
        success: function () {
            Redirection("DocumentGestion", "Index");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

/** Pad Left
 * @return {string} la chaîne paddée à gauche
 * @param {srting} str chaîne de caractère sur laquelle effectuée le padLeft
 * @param {string} charPad caractère servant à faire le padLeft
 * @param {number} max integer indiquant le nombre de caractère max pour le padLeft
 */
function padLeft(str, charPad, max) {
    str = str.toString();
    return str.length < max ? padLeft(charPad + str, charPad, max) : str;
}
/** PadRight
 * @return {string} la chaîne paddée à droite
 * @param {srting} str chaîne de caractère sur laquelle effectuée le padRight
 * @param {string} charPad caractère servant à faire le padRight
 * @param {number} max integer indiquant le nombre de caractère max pour le padRight
 */
function padRight(str, charPad, max) {
    str = str.toString();
    return str.length < max ? padRight(str + charPad, charPad, max) : str;
}

//-------Cache les boutons des menus cachés
function HideButtonMenu() {
    var hiddenMenus = $("#hiddenMenu").val();
    var tMenus = hiddenMenus.split(";");

    $.each(tMenus, function (item, value) {
        if (value != undefined && value !== "") {
            var menu = value;
            var tMenu = menu.split("||");
            if (tMenu.length === 1) {
                $("button[id='btn" + tMenu[0].substring(1) + "']").hide();
            }
        }
    });


}



//------------------ Date ranges comparaisons
var DATE_MIN_VALUE = -8640000000000000;
var DATE_MAX_VALUE = 8640000000000000;

function isDateRangesEquals(fromA, toA, fromB, toB) {

    if (!fromA)
        fromA = new Date(DATE_MIN_VALUE);
    if (!toA)
        toA = new Date(DATE_MAX_VALUE);
    if (!fromB)
        fromB = new Date(DATE_MIN_VALUE);
    if (!toB)
        toB = new Date(DATE_MAX_VALUE);

    return fromA.valueOf() === fromB.valueOf() && toA.valueOf() === toB.valueOf();
}

function isDateRangesOverlaps(fromA, toA, fromB, toB) {


    if (!fromA)
        fromA = new Date(DATE_MIN_VALUE);
    if (!toA)
        toA = new Date(DATE_MAX_VALUE);
    if (!fromB)
        fromB = new Date(DATE_MIN_VALUE);
    if (!toB)
        toB = new Date(DATE_MAX_VALUE);

    if (fromA.valueOf() === fromB.valueOf()) return true;

    var minRange = {};
    var maxRange = {};

    if (fromA.valueOf() < fromB.valueOf()) {
        minRange = { from: fromA, to: toA };
        maxRange = { from: fromB, to: toB };
    }
    else {
        minRange = { from: fromB, to: toB };
        maxRange = { from: fromA, to: toA };
    }

    return maxRange.from.valueOf() <= minRange.to.valueOf();
}

/**
 * Parse une chaîne date MS
 * @return {Date} objet Date
 * @param {string} value la date au format /Date(...)/
 */
function ToJavaScriptDate(value) {

    if (!value || value === "") return null;

    value = value.replace("/Date(", "");
    value = value.replace(")/", "");
    return new Date(parseInt(value));
}


function safeBoundClickEvent(element, handler) {
    var events = $._data($('#btnFraisAccess')[0], "events");

    GetValueAddParam();
}
/**
 * Retourne la valeur d'un paramètre dans addParam
 * @return {string} valeur du paramètre
 * @param {string} addParam chaîne conteant tous les paramètres
 * @param {string} keyParam clé du paramètre
 */
function GetValueAddParam(addParam, keyParam) {

    if (addParam.lastIndexOf('|||') > 0) {
        addParam = addParam.substring(addParam.lastIndexOf('|||') + 3);
    }
    var result = addParam.split('||').filter(function (element) {
        return element.indexOf(keyParam + '|') === 0 ? element : '';
    })[0];
    if (result) {
        return result.replace(keyParam + '|', "");
    }
    return '';
}


//------------Déclenche le btn en cours pour sauvegarder et quitter-----------
function saveAndQuitRegul() {
    //Fermeture à partir de l'accueil des regule

    if ($("#btnFermer").is(":visible")) {
        $("#btnFermer").click();
        return;
    }

    //Fermeture à partir de la création d'une régule
    //ou de la liste des risques
    if ($("#btnReguleSuivant") !== null && $("#btnReguleSuivant") !== undefined) {
        if ($("#btnReguleSuivant").is(":visible")) {
            if (!$("#bntReguleSuivant").is(":disabled")) {
                $("#btnReguleSuivant").trigger("click", { returnHome: true });
            }
            else {
                common.dialog.bigError("Veuillez valider vos actions avant de quitter l'écran", true);
                CloseLoading();
            }
        }
        else {
            RedirectionRegul("Index", "RechercheSaisie", true, "");
        }
    }

}

//#region LTA

//-----------Ouverture des éléments LTA-------------
function OpenWindowLTA() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/OpenLTA",
        data: {
            codeAffaire: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            avenant: $("#NumAvenantPage").val(),
            modeNavig: $("#ModeNavig").val(),
            isReadonly: $("#OffreReadOnly").val()
        },
        success: function (data) {
            $("#divDataLTA").html(data);
            $("#divLTA").show();
            MapElementLTA();
            CloseLoading();
        },
        error: function (request) {
            var responseError = jQuery.parseJSON(request.responseText);
            common.dialog.bigError(responseError.ErrorMessage, true);
            CloseLoading();
        }
    });
}
//------------Mappage des éléments LTA--------------
function MapElementLTA() {
    common.autonumeric.apply($("#SeuilLTA"), 'init', 'decimal', '', null, 4, '999.9999', '0');
    common.autonumeric.apply($("#DureeLTA"), 'init', 'numeric', '', null, null, '99', '0');
    $("#DateDebLTA").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#DateFinLTA").datepicker({ dateFormat: 'dd/mm/yy' });

    $("#DateDebLTA").live('change', function () {
        if ($(this).val() !== "") {
            FormatAutoDate($(this));
            if ($("#HeureDebLTAHours").val() == "") {
                $("#HeureDebLTAHours").val("00");
                $("#HeureDebLTAMinutes").val("00");
                $("#HeureDebLTAHours").change();
            }
        }
        else {
            $("#HeureDebLTAHours").val("");
            $("#HeureDebLTAMinutes").val("");
            $("#HeureDebLTAHours").change();
        }
    });
    $("#DateFinLTA").die();
    $("#DateFinLTA").live('change', function () {
        if ($(this).val() !== "") {
            FormatAutoDate($(this));
            if ($("#HeureFinLTAHours").val() == "") {
                $("#HeureFinLTAHours").val("23");
                $("#HeureFinLTAMinutes").val("59");
                $("#HeureFinLTAHours").change();
            }
        }
        else {
            $("#HeureFinLTAHours").val("");
            $("#HeureFinLTAMinutes").val("");
            $("#HeureFinLTAHours").change();
        }
    });

    $("#HeureDebLTAHours").die();
    $("#HeureDebLTAHours").live('change', function () {
        if ($(this).val() != "" && $("#HeureDebLTAMinutes").val() == "")
            $("#HeureDebLTAMinutes").val("00");
        ChangeHourLTA($(this));
    });
    $("#HeureDebLTAMinutes").die();
    $("#HeureDebLTAMinutes").live('change', function () {
        if ($(this).val() != "" && $("#HeureDebLTAHours").val() == "")
            $("#HeureDebLTAHours").val("00");
        ChangeHourLTA($(this));
    });
    $("#HeureFinLTAHours").die();
    $("#HeureFinLTAHours").live('change', function () {
        if ($(this).val() != "" && $("#HeureFinLTAMinutes").val() == "")
            $("#HeureFinLTAMinutes").val("00");
        ChangeHourLTA($(this));
    });
    $("#HeureFinLTAMinutes").die();
    $("#HeureFinLTAMinutes").live('change', function () {
        if ($(this).val() != "" && $("#HeureFinLTAHours").val() == "")
            $("#HeureFinLTAHours").val("00");
        ChangeHourLTA($(this));
    });

    $("#btnSaveLTA").die();
    $("#btnSaveLTA").kclick(function () {
        $(".requiredField").removeClass("requiredField");
        var isLTAInvalid = CheckValidLTA();
        if (isLTAInvalid) {
            common.dialog.error("Veuillez corriger les informations erronées.");
        }
        else {
            SaveInfoLTA();
            $("#divDataLTA").html("");
            $("#divLTA").hide();
        }
    });
    $("#btnCancelLTA").die();
    $("#btnCancelLTA").kclick(function () {
        $("#divDataLTA").html("");
        $("#divLTA").hide();
    });
    $("#EffetLTACheck").die();
    $("#EffetLTACheck").kclick(function () {
        var isChecked = $(this).attr("checked");
        if (isChecked) {
            $("#DureeLTACheck").removeAttr("checked");
            $("#divDataFinEffetLTA").show();
            $("#divDataDureeLTA").hide();
        }
        else {
            $("#divDataFinEffetLTA").hide();
        }
    });
    $("#DureeLTACheck").die();
    $("#DureeLTACheck").kclick(function () {
        var isChecked = $(this).attr("checked");
        if (isChecked) {
            $("#EffetLTACheck").removeAttr("checked");
            $("#divDataDureeLTA").show();
            $("#divDataFinEffetLTA").hide();
        }
        else {
            $("#divDataDureeLTA").hide();
        }
    });
}
//--------------------------------------------------------
function ChangeHourLTA(elem) {
    var elemId = elem.attr('id').replace("Hours", "").replace("Minutes", "");

    var changeHour = SetHours(elemId);
    if (!changeHour && elem.val() == "") {
        $("#" + elemId + "Hours").val("");
        $("#" + elemId + "Minutes").val("");
    }

}
//--------Vérification des données de la LTA--------------
function CheckValidLTA() {
    var isError = false;

    var dateDeb = $("#NumAvenantPage").val() == "0" ? $("#EffetGaranties").val() : $("#DateDebEffet").val();
    var heureDeb = $("#HeureEffet").val();
    var dateFin = $("#NumAvenantPage").val() == "0" ? $("#FinEffet").val() : $("#DateFinEffet").val();
    var heureFin = $("#HeureFinEffet").val();
    var duree = $("#Duree").val();
    var dureeUnite = $("#DureeString").val();
    var effetCheck = $("#EffetCheck").is(":checked");
    var dureeCheck = $("#DureeCheck").is(":checked");
    if (dureeCheck && duree != "" && dureeUnite != "") {
        switch (dureeUnite) {
            case "A":
                dateFin = incrementDate(dateDeb, 0, 0, duree, 0, false);
                break;
            case "M":
                dateFin = incrementDate(dateDeb, 0, duree, 0, 0, false);
                break;
            case "J":
                dateFin = incrementDate(dateDeb, duree, 0, 0, 0, false);
                break;
            case "S":
                dateFin = incrementDate(dateDeb, 0, 0, 0, duree, false);
                break;
        }
        heureFin = "23:59:00";
    }

    var dateDebLTA = $("#DateDebLTA").val();
    var heureDebLTA = $("#HeureDebLTA").val();
    var dateFinLTA = $("#DateFinLTA").val();
    var heureFinLTA = $("#HeureFinLTA").val();
    var dureeLTA = $("#DureeLTA").val();
    var dureeUniteLTA = $("#DureeLTAString").val();
    var effetCheckLTA = $("#EffetLTACheck").is(":checked");
    var dureeCheckLTA = $("#DureeLTACheck").is(":checked");

    //Vérification du début LTA
    if (dateDebLTA == "" || !isDate(dateDebLTA)
        || heureDebLTA == "") {
        $("#DateDebLTA").addClass("requiredField");
        $("#HeureDebLTAHours").addClass("requiredField");
        $("#HeureDebLTAMinutes").addClass("requiredField");
        isError = true;
    }
    else {
        //Vérification du début LTA >= début d'effet & < fin d'effet (ou durée)
        if (getDate(dateDebLTA, heureDebLTA) < getDate(dateDeb, heureDeb)
            || (getDate(dateDebLTA, heureDebLTA) > getDate(dateFin, heureFin) && dateFin != "")) {
            $("#DateDebLTA").addClass("requiredField");
            $("#HeureDebLTAHours").addClass("requiredField");
            $("#HeureDebLTAMinutes").addClass("requiredField");
            isError = true;
        }
    }

    if (!$("#EffetLTACheck").is(":checked") && !$("#DureeLTACheck").is(":checked")) {
        $("#divLTAEffet").addClass("requiredField");
        $("#divLTADuree").addClass("requiredField");
        isError = true;
    }

    if ($("#EffetLTACheck").is(":checked")) {
        if (dateFinLTA == "" || !isDate(dateFinLTA)
            || heureFinLTA == "") {
            $("#DateFinLTA").addClass("requiredField");
            $("#HeureFinLTAHours").addClass("requiredField");
            $("#HeureFinLTAMinutes").addClass("requiredField");
            isError = true;
        }
        else {
            //Vérification de la fin LTA > début effet && < fin effet (ou durée)
            if (getDate(dateFinLTA, heureFinLTA) < getDate(dateDeb, heureDeb)
                || (getDate(dateFinLTA, heureDebLTA) > getDate(dateFin, heureFin) && dateFin != "")) {
                $("#DateFinLTA").addClass("requiredField");
                $("#HeureFinLTAHours").addClass("requiredField");
                $("#HeureFinLTAMinutes").addClass("requiredField");
                isError = true;
            }
            //Vérification du début LTA < fin LTA
            if (getDate(dateDebLTA, heureDebLTA) >= getDate(dateFinLTA, heureDebLTA)) {
                $("#DateDebLTA").addClass("requiredField");
                $("#HeureDebLTAHours").addClass("requiredField");
                $("#HeureDebLTAMinutes").addClass("requiredField");

                $("#DateFinLTA").addClass("requiredField");
                $("#HeureFinLTAHours").addClass("requiredField");
                $("#HeureFinLTAMinutes").addClass("requiredField");
                isError = true;
            }
        }
    }

    if ($("#DureeLTACheck").is(":checked") && ($("#DureeLTA").val() == "" || $("#DureeLTA").val() == "0" || $("#DureeLTAString").val() == "")) {
        $("#DureeLTA").addClass("requiredField");
        $("#DureeLTAString").addClass("requiredField");
        isError = true;
    }
    else {
        switch (dureeUniteLTA) {
            case "A":
                dateFinLTA = incrementDate(dateDebLTA, 0, 0, dureeLTA, 0, true);
                break;
            case "M":
                dateFinLTA = incrementDate(dateDebLTA, 0, dureeLTA, 0, 0, true);
                break;
            case "J":
                dateFinLTA = incrementDate(dateDebLTA, dureeLTA, 0, 0, 0, true);
                break;
            case "S":
                dateFinLTA = incrementDate(dateDebLTA, 0, 0, 0, dureeLTA, true);
                break;
        }
        heureFinLTA = "23:59:00";
        //Vérification de la durée LTA > début effet && < fin effet (ou durée)
        if (getDate(dateFinLTA, heureFinLTA) < getDate(dateDeb, heureDeb)
            || (getDate(dateFinLTA, heureFinLTA) > getDate(dateFin, heureFin) && dateFin != "")) {
            $("#DureeLTA").addClass("requiredField");
            $("#DureeLTAString").addClass("requiredField");
            isError = true;
        }
    }

    if ($("#SeuilLTA").val() == "") {
        $("#SeuilLTA").addClass("requiredField");
        isError = true;
    }

    return isError;
}
//--------Sauvegarde des données de la LTA----------------
function SaveInfoLTA() {
    ShowLoading();

    var codeAffaire = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var avenant = $("#NumAvenantPage").val();

    var isDateCheck = $("#EffetLTACheck").is(":checked");
    var isDureeCheck = $("#DureeLTACheck").is(":checked");

    var dateDebLTA = $("#DateDebLTA").val();
    var heureDebLTA = $("#HeureDebLTA").val();
    var dateFinLTA = $("#DateFinLTA").val();
    var heureFinLTA = $("#HeureFinLTA").val();

    var dureeLTA = $("#DureeLTA").val();
    var dureeLTAString = $("#DureeLTAString").val();

    var seuilLTA = $("#SeuilLTA").val();

    if (isDureeCheck) {
        switch (dureeLTAString) {
            case "A":
                dateFinLTA = incrementDate(dateDebLTA, 0, 0, dureeLTA, 0, true);
                break;
            case "M":
                dateFinLTA = incrementDate(dateDebLTA, 0, dureeLTA, 0, 0, true);
                break;
            case "J":
                dateFinLTA = incrementDate(dateDebLTA, dureeLTA, 0, 0, 0, true);
                break;
            case "S":
                dateFinLTA = incrementDate(dateDebLTA, 0, 0, 0, dureeLTA, true);
                break;
        }
        heureFinLTA = "23:59:00";
    }

    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/SaveInfoLTA",
        data: {
            codeAffaire: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), avenant: $("#NumAvenantPage").val(),
            isDateCheck: isDateCheck, isDureeCheck: isDureeCheck,
            dateDebLTA: dateDebLTA, heureDebLTA: heureDebLTA, dateFinLTA: dateFinLTA, heureFinLTA: heureFinLTA,
            dureeLTA: dureeLTA, dureeLTAString: dureeLTAString,
            seuilLTA: seuilLTA
        },
        success: function (data) {
            $("#divDataLTA").html("");
            $("#divLTA").hide();
            CloseLoading();
        },
        error: function (request) {
            var responseError = jQuery.parseJSON(request.responseText);
            common.dialog.bigError(responseError.ErrorMessage, true);
            CloseLoading();
        }
    });

}

//#endregion