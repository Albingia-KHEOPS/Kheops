/// <reference path="Common/common.js" />
/// <reference path="albCommonAutoComplete.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {

    /* window.addEventListener("beforeunload", function (e) {
         var confirmationMessage = "\!!!!!!!!!!!!!!/";
 
         (e || window.event).returnValue = confirmationMessage; //Gecko + IE
         return confirmationMessage;                            //Webkit, Safari, Chrome
     });*/
    MapElementPageRecherche();
    FormatNumericValue();
    FormatNumericCodePreneur();
    FormatNumericCodeCourtier();
    CheckLockedOffer();
    MapButtonVerrouPopup();
    ReadOnlyFolder();
    LoadParamOffre();
    CloseQuickResultResearch();
    toggleSearchEnabling();
    if (window.location.pathname.indexOf("/RechercheSaisie/Index") === 0
        || window.location.pathname === "/") {
        loadFilter();
    }
    $("#Branche[albemplacement=recherche]").trigger('change');
    window.addEventListener("message", receiveMessage, false);

    function receiveMessage(msg) {
        if (msg.data.type == "lockError") {
            displayLockError(msg.data);
        }
    }
});
var paramDblSaisie = "";
var derniereVersion;
var testCreationNouvelleVersion;

//-------------Map les éléments de la page-----------
function MapElementPageRecherche() {
    var albEmplacement = $("#albEmplacement").val();
    if (albEmplacement == "RechercheCopieOffre") {
        $("#divSearchCriteria").addClass('CopyOffreHeight');
        $("#divSearchContainer").addClass('CopyOffreHeightContainer');

        $("#btnAnnulerRechercheOffre").kclick(function () {
            $("#btnInitializeRech").trigger('click');
            ReactivateShortCut("resultatRecherche");
            window.parent.$("#divRechercheCopyOffre").hide();
        });

        $("#btnAnnulerRechercheContrat").kclick(function () {
            $("#btnInitializeRech").trigger('click');
            ReactivateShortCut("resultatRecherche");
            window.parent.$("#divRechercheCopyOffre").hide();
        });


        $("#btnAnnulerResultatRechercheContrat").kclick(function () {
            ReactivateShortCut("resultatRecherche");
            $("#btnInitializeRech").trigger('click');
            window.parent.$("#divRechercheCopyOffre").hide();
        });

        $("#btnAnnulerResultatRechercheOffre").kclick(function () {
            ReactivateShortCut("resultatRecherche");
            $("#btnInitializeRech").trigger('click');
            window.parent.$("#divRechercheCopyOffre").hide();
        });

        $("#btnSelectionnerOffre").kclick(function () {
            validateSelectionQuickSearch();
        });

        $("#btnSelectionnerContrat").kclick(function () {
            validateSelectionQuickSearch();
        });
    }

    $("#OffreId").focus();
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#PreneurAssuranceCP").attr("maxlength", 5);
    $("#AdresseRisqueCP").attr("maxlength", 5);
    $("#PreneurAssuranceSIREN").die();
    $("#PreneurAssuranceSIREN").attr("maxlength", 9);
    var SIREN = "";
    $("#PreneurAssuranceSIREN").live("keyup", function (event) {
        if (!isNaN($(this).val())) {
            SIREN = $(this).val();
        } else { $(this).val(SIREN); }
    });

    $("#PreneurAssuranceCode,  #PreneurAssuranceCP, #PreneurAssuranceVille, #PreneurAssuranceSIREN, #CabinetCourtageId, #CabinetCourtageNom, #AdresseRisqueVoie, #AdresseRisqueCP, #AdresseRisqueVille, #MotsClefs, #SouscripteurNom, #GestionnaireNom, #DateDebut, #DateFin, #Branche[albemplacement=recherche], #Etat, #chkSauf, #Cible[albemplacement=recherche], #chkApporteur, #chkGestionnaire, #PreneurAssuranceDEP").die('input change').live('input change', function () {
        toggleSearchEnabling();
    });
    $(document).on("autoCompSouscripteurSelected autoCompGestionnaireSelected autoCompCourtierSelected autoCompAssureSelected",
        function (e, data) {
            $(data.target).data("selectedItem", data.item);
            toggleSearchEnabling();
        }
    );

    $("#chkActif, #chkInactif").off("change").change(function () {
        var situationActif = $('#chkActif').isChecked();
        var situationInactif = $('#chkInactif').isChecked();

        if (situationActif || situationInactif) {
            $("#Situation").clear();
        }
        toggleSearchEnabling();
    });

    MapCommonAutoCompCourtier();
    MapCommonAutoCompAssure();
    MapCommonAutoCompGestionnaire();
    MapCommonAutoCompSouscripteur();
    MapCommonAutoCompCPVille();

    common.dom.onEvent(["input keyup focusout"], "#OffreId", function () {
        toggleFilterEnabling();
        toggleSearchEnabling();
    });

    $("#btnCreerSaisie").kclick(function () {
        CreerSaisie();
    });

    $("#btnDoubleSaisie").kclick(function () {
        DoubleSaisie($(this));
    });

    $("#btnInitialize").kclick(function () {
        InitializeForm();
    });

    $("#btnErrorOk").kclick(function () {
        CloseCommonFancy();
    });

    $("#btnCloseAdvanced").kclick(function () {
        $("#PreneursAssuranceRechercherResultDialogDiv").html("");
        $("#CabinetCourtageRechercherResultDialogDiv").html("");
        CloseFancy();
    });

    $("#btnRechercher").kclick(function () {
        if (!$(this).attr('disabled')) {
            RechercheOffres($(this), true);
        }
    });

    $("input[name='tri']").kclick(function () {
        $("#resultSorting").val($(this).val());
        RechercheOffres($(this), true);
    });

    Pagination();

    $("#PreneurAssuranceNom").off("change").change(function () {
        let t = $(this).data("selecteItem");
        if (t && t.Nom != $(this).val()) {
            $("#PreneurAssuranceId").clear();
            $("#PreneurAssuranceId").trigger("change");
        }
    });
    $("#SouscripteurNom").off("change").change(function () {
        let t = $(this).data("selecteItem");
        if (t && ((t.code + " - " + t.Nom) != $(this).val())) {
            $("#SouscripteurCode").clear();
        }
    });
    $("#GestionnaireNom").off("change").change(function () {
        let t = $(this).data("selecteItem");
        if (t && (t.code + " - " + t.Nom) != $(this).val()) {
            $("#GestionnaireCode").clear();
        }
    });
    $("#btnConfirmRechercheSaisieOk").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "NouvelleVersion":
                NouvelleVersionProgression();
                break;
            case "Clean":
                var tabGuid = IsInIframe() ? $('#homeTabGuid', window.parent.document).val() : $('#tabGuid').val();
                CleanSession(tabGuid);
                break;
            case "InitializeSession":
                ClearSessionSearch();
                break;
            case "Reprise":
                ConfirmReprise();
                break;
            case "RepriseOffre":
                RepriseOffre();
                break;
            case "ResilEch":
                OpenAvenantResil("1", "newwindow");
                setTimeout(function () {
                    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
                    var infoRows = $(selectRadio).val();
                    var offreNum = infoRows.split("_")[0];
                    var offreVersion = infoRows.split("_")[1];
                    var offreType = infoRows.split("_")[2];
                    var numAvenant = infoRows.split("_")[3];
                    var addParam = "";
                    if (numAvenant != "0")
                        addParam = "addParam" + $("#globalTypeAddParamAvn").val() + "|||" + $("#globalTypeAddParamAvn").val() + "ID|" + numAvenant + "||AVNTYPE|" + selectRadio.attr("albtypeavt") + "addParam";

                    var valRedirect = offreNum + "_" + offreVersion + "_" + offreType + $("#homeTabGuid", window.parent.document).val() + "modeNavig" + $("#ModeNavig").val() + "modeNavigReadOnlyConsult";
                    valRedirect = "AnInformationsGenerales/Index/" + valRedirect;
                    CommonRedirect(valRedirect);
                }, 750);
                break;
            case "OpenAvtResil":
                AvtResilRedirect(true, false);
                break;
            case "OpenAvtResilC":
                AvtResilRedirect(true, true);
                break;
            case "AVNMD":
                recherche.affaires.results.consultOrEdit(null, "AVNMD");
                break;
            case "OpenHorsAvn":
                $('#btnInfoOk').trigger('click');
                break;
        }
        $("#hiddenAction").clear();
    });

    $("#btnConfirmRechercheSaisieCancel").kclick(function () {
        switch ($("#hiddenAction").val()) {
            case "InitializeSession":
                InitializeForm();
                break;
            case "ResilEch":
                OpenAvenantResil("1");
                break;
            case "OpenAvtResil":
                AvtResilRedirect(false, false);
                break;
            case "OpenAvtResilC":
                AvtResilRedirect(false, true);
                break;
            case "AVNMD":
                $("#TypeAvt").clear();
                $("#ModeAvt").clear();
                $("#ignoreReadonly").clear();
                break;
        }
        CloseCommonFancy();
        $("#hiddenAction").clear();
    });

    $("#btnInfoOk").off("click");
    $(document).on("click", "#btnInfoOk", function () {
        if (testCreationNouvelleVersion) {
            let selectRadio = $('input[type=radio][name=RadioRow]:checked');
            let offre = $(selectRadio).val().split("_");
            offre[1] = parseInt(offre[1]) + 1;
            $(selectRadio).val(offre.join("_"));
            recherche.affaires.results.consultOrEdit("btnCreerOffre");
        }
        else {
            CloseCommonFancy();
            let action = $("#hiddenAction").val() || "";
            try {
                $("#hiddenAction").clear();
                if (action.indexOf("url:") === 0 && action !== "url:") {
                    try {
                        document.location.assign(action.split(":")[1]);
                    }
                    catch (e) {
                        console.error(e);
                    }
                }
                else {
                    switch (action) {
                        case "checkLock":
                            CommonRedirect($("#hiddenParamCheckLock").val());
                            break;
                        case "OpenHorsAvn":
                            var url = $("#hiddenParamCheckLock").val();
                            CommonRedirect(url.replace('ConsultOnlyAndEdit', 'ConsultOnly'));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (e) {
                $("#hiddenAction").val(action);
                console.error(e);
            }
        }
    });

    $("#btnRetours").kclick(function () {
        OpenRetoursPieces();
    });

    $("#btnBlocageTermes").kclick(function () {
        var selectRadio = $('input[type=radio][name=RadioRow]:checked');
        var infoRows = $(selectRadio).val();
        var offreNum = infoRows.split("_")[0];
        var offreVersion = infoRows.split("_")[1];
        var offreType = infoRows.split("_")[2];
        var niveauDroit = 1;
        var valRedirect = offreNum + "_" + offreVersion + "_" + offreType + "_" + niveauDroit + $("#homeTabGuid", window.parent.document).val();
        RedirectionRechercheSaisie("BlocageTermes", "Index", valRedirect);
    });

    $("#btnReadonlyAnnuler").kclick(function () {
        $("#divReadOnlyMsg").html("");
        $("#divReadOnlyOffre").hide();
        $("#btnReadonly").show();
    });

    $("#btnNewVersion").kclick(function () {
        NouvelleVersionProgression();
    });

    $("#btnReadonly").kclick(function () {
        $("#btnFolderReadOnly").trigger("click");
    });

    $("#btnCopyOffre").kclick(function () {
        CopyOffre();
    });

    $("#btnRefusOffre").kclick(function () {
        Refuser();
    });

    $("#btnCreerContrat").kclick(function () {
        CreerContrat();
    });

    $("#btnRefusAnnuler").kclick(function () {
        $("#divRefusOffre").hide();
    });

    $("#btnRefusValider").kclick(function () {
        if ($("#Refus").hasVal()) {
            RefusOffre();
        }
        else {
            common.dialog.error("Veuillez choisir un motif de refus.");
        }
    });

    $("#SouscripteurNom").on("input", function (e) {
        $("#SouscripteurSelect").clear();
        $("#SouscripteurCode").clear().trigger("change");
    });

    $("#GestionnaireNom").on("input", function () {
        $("#GestionnaireSelect").clear();
        $("#GestionnaireCode").clear().trigger("change");
    });

    $("#btnEtablirContrat").kclick(function () {
        EtablirAN();
    });

    $("#btnConnexites").kclick(function () {
        ShowLoading();
        RedirectionRechercheSaisie("CommonNavigation", "Index");
    });

    $("#btnEngagementPeriodes").kclick(function () {
        RedirectionRechercheSaisie("EngagementPeriodes", "Index");
    });

    $("#Branche[albemplacement=recherche]").die().live("change", function () {
        AffectTitleList($(this));
    });

    $("#Cible[albemplacement=recherche]").die().live("change", function () {
        AffectTitleList($(this));
    });

    $("#Etat").die().live("change", function () {
        AffectTitleList($(this));
    });

    common.dom.onChange("#Situation", function () {
        AffectTitleList($(this));
        if ($("#Situation").val() != "") {
            $("#chkActif").removeAttr("checked");
            $("#chkInactif").removeAttr("checked");
        }
        toggleSearchEnabling();
    });

    $("#btnCleanSession").kclick(function () {
        ShowCommonFancy("ConfirmRechercheSaisie", "Clean", "Pour nettoyer vos sessions, veuillez toutes les quitter, puis cliquez sur OK.", 300, 85, true, true);
    });

    $("#btnValidLst").kclick(function () {
        if ($(this).attr("albcontext") == "adressePreneur")
            ValidLstCP($("input[id='PreneurAssuranceCP']"));
        if ($(this).attr("albcontext") == "adresseOffreContrat")
            ValidLstCP($("input[id='AdresseRisqueCP']"));
    });
    $("#btnCancelLst").kclick(function () {
        CloseListDiv($("input[id='PreneurAssuranceVille']"), "");
    });

    $("#btnInformationsSpecifiques").kclick(function () {
        RedirectionRechercheSaisie("InformationsSpecifiquesOffreSimp", "Index");
    });

    LoadListCibleByBranche();
    $("#divRechercheCopyOffre").hide();

    $("#NomCabinetRecherche").die().live('keydown', function (event) {
        if ((event.which >= 48 && event.which <= 122) || event.which == 8) {
            $("#CodeCabinetRecherche").clear();
            $("#CabinetCourtagePaginationPageActuelle").html(1);
        }
        if (event.which == 13) {
            event.preventDefault();
            $("#RechercherCabinetCourtierButton").trigger("click");
        }
    });

    $("#CodeCabinetRecherche").die().live('keydown', function (event) {
        if ((event.which >= 48 && event.which <= 122) || event.which == 8) {
            $("#NomCabinetRecherche").clear();
            $("#CabinetCourtagePaginationPageActuelle").html(1);
        }
        if (event.which == 13) {
            event.preventDefault();
            $("#RechercherCabinetCourtierButton").trigger("click");
        }
    });

    $("#NomPreneurAssuranceRecherche").die().live('keydown', function (event) {
        if ((event.which >= 48 && event.which <= 122) || event.which == 8) {
            $("#CodePreneurAssuranceRecherche").clear();
            $("#PreneurAssurancePaginationPageActuelle").html(1);
        }
        if (event.which == 13) {
            event.preventDefault();
            $("#RechercherPreneursAssuranceButton").trigger("click");
        }
    });

    $("#CPPreneurAssuranceRecherche").die().live('keydown', function (event) {
        if ((event.which >= 48 && event.which <= 122) || event.which == 8) {
            $("#PreneurAssurancePaginationPageActuelle").html(1);
        }
        if (event.which == 13) {
            event.preventDefault();
            $("#RechercherPreneursAssuranceButton").trigger("click");
        }
    });

    $("#CodePreneurAssuranceRecherche").die().live('keydown', function (event) {
        if ((event.which >= 48 && event.which <= 122) || event.which == 8) {
            $("#NomPreneurAssuranceRecherche").clear();
            $("#PreneurAssurancePaginationPageActuelle").html(1);
        }
        if (event.which == 13) {
            event.preventDefault();
            $("#RechercherPreneursAssuranceButton").trigger("click");
        }
    });

    $("#RechercherPreneurAssuranceImg").kclick(function () {
        if ($(this).hasClass('CursorPointer')) {
            var context = $(this).attr("albcontext");
            if (context == undefined)
                context = "";
            OpenRechercheAvanceePreneurAssurance($("#PreneurAssuranceCode").val(), $("#PreneurAssuranceNom").val(), context);
        }


    });


    /* Recherche Avancé debut */
    $("#RechercherCabinetCourtageImg").kclick(function () {
        if ($(this).hasClass("CursorPointer")) {

            var contexte = $(this).attr("albcontext");
            $("#divDataRechercheAvancee").html($("#divRechercheAvanceeCourtier").html());
            AlbScrollTop();
            $("#divRechercheAvancee").show();

            RechercheAvanceeCourtier(contexte);
            SelectCourtier();
            CloseRechAvancee();

            if ($("#CabinetCourtageNom").val().length > 0) {
                $("#NomCabinetRecherche").val($("#CabinetCourtageNom").val());
            }

            if ($("#CabinetCourtageId").val().length > 0) {
                $("#CodeCabinetRecherche").val($("#CabinetCourtageId").val());
            }

            if ($("#CabinetCourtageNom").val().length > 0 || $("#CabinetCourtageId").val().length > 0) {
                $("#RechercherCabinetCourtierButton").trigger("click");
            }
            FormatNumericCodeCourtier();

        }
    });

    $("#btnChangeMode").kclick(function () {
        var currentModeNavig = $("#ModeNavig").val();

        $.ajax({
            type: "POST",
            url: "/RechercheSaisie/ChangeModeNavig",
            data: { currentModeNavig: currentModeNavig },
            success: function (data) {
                $("#ModeNavig").val(data);
                switch (data) {
                    case "S":
                        $("#btnChangeMode").assignAccessKey("q").html("Histori<u>q</u>ue");
                        break;
                    case "H":
                        $("#btnChangeMode").assignAccessKey("d").html("Stan<u>d</u>ard");
                        break;
                    default:
                        break;
                }
            },
            error: function (request) {
                common.error.showXhr(request, true);
            }
        });
    });

    $("input[type='radio'][name='modeNavigation']").offOn("change", function () {
        var modeNavig = $(this).val();
        $("#ModeNavig").val(modeNavig);
    });

    $("#btnCorrectionECM").kclick(function () {
        alert("plus aucune correction");
        //$.ajax({
        //    type: "POST",
        //    url: "/RechercheSaisie/CorrectionECM",
        //    success: function (data) {
        //        alert('ok');
        //    },
        //    error: function (request) {
        //        alert('ko');
        //    }
        //});
    });

    $("#btnLoadJsonFile").kclick(function () {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/RechercheSaisie/CreateJsonContract",
            success: function (data) {
                alert(data);
                CloseLoading();
            },
            error: function (request) {
                alert('KO');
                CloseLoading();
            }
        });
    });

    $("#btnAtlas").kclick(function () {
        window.open($("#hiddenAtlasUrl").val());

    });

    $("#btnClausierRech").kclick(function () {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/RechercheSaisie/GetListClausier",
            success: function (data) {
                $("#divDataClausierRech").html(data);
                $("#divClausierRech").show();
                MapElementClausierRech();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });

    $("#btnAttestations").kclick(function () {
        recherche.affaires.results.createAttestation();
    });

    $("#btnRepriseAvt").kclick(function () {
        common.dialog.initConfirm(
            function () {
                common.page.isLoading = true;
                let selectedData = $("input[type=radio][name=RadioRow]:checked").val().split("_");
                let contrat = {
                    codeOffre: selectedData[0],
                    version: selectedData[1],
                    type: selectedData[2],
                    numeroAvenant: selectedData[3]
                };
                common.$postJson("/RechercheSaisie/RepriveAvenant", { folder: contrat }, true).done(function () {
                    $("#btnInitializeRech").trigger("click");
                    $("#OffreId").val(contrat.codeOffre);
                    $("#OffreId").trigger("keyup");
                    $("#btnRechercher").trigger("click");
                });
            },
            null, "Etes-vous sûr(e) d'effectuer la reprise de l'avenant N-1 ?");
    });

    $("#btnRegularisations").kclick(function () {
        OpenRegulPage();
    });

    document.addEventListener("keypress", function (e) {
        if (common.page.controller.toLowerCase() == "recherchesaisie" || $("#divDataRechercheContrat").isVisible()) {
            if (e.keyCode === 13 && toggleSearchEnabling()) {
                e.preventDefault();
                $("#btnRechercher").click();
            }
        }
    });

    /** @type {HTMLIFrameElement} */
    var iframeElement = document.getElementById("EngagementIFrame");
    if (iframeElement) {
        iframeElement.addEventListener("load", function () {
            common.page.isLoading = false;
            if (iframeElement.contentWindow.location.pathname.toLowerCase().indexOf("/recherchesaisie") == 0) {
                $("#divRechEngagementPeriode").hide();
                iframeElement.src = "about:blank";
                //common.error.showMessage("Votre session a expiré");
            }
        });
    }

}
//------------Map les éléments des div flottantes creation saisie/contrat
function MapElementDivCreation() {
    MapCommonAutoCompTemplates();
    CheckCanevas();

    $("#btnCreateAnnuler").kclick(function () {
        ReactivateShortCut();
        $("#divCreate").hide();
        $("#LoadingRechDiv").hide();
    });

    $("#btnCreateNo").kclick(function () {
        $("#divCreate").hide();
        AlbScrollTop();
        $("#divCopie").show();
        $("#CodeOffreContratCopy").focus();
    });

    $("#btnCreateYes").kclick(function () {
        $("#ModeNavig").val("S");
        if ($("#TypeEcran").val() == "Offre") {
            $("#divCreateOffre").hide();
            CreateSaisie();
        }
        else if ($("#TypeEcran").val() == "Contrat") {
            RedirectionRechercheSaisie("AnCreationContrat", "Index");
        }
    });

    $("#btnCopyAnnuler").kclick(function () {
        $("#divCopie").hide();
        if ($("#copyOffreContrat").val() != "1") {
            AlbScrollTop();
            $("#divCreate").show();
        }
        else {
            $("#copyOffreContrat").clear();
        }
        $("#CodeOffreContratCopy").clear();
        $("#VersionCopy").val("0");
    });

    $("#btnCopyValid").kclick(function () {
        $("#ModeNavig").val("S");
        if ($("#CodeOffreContratCopy").val() == "" || $("#VersionCopy").val() == "") {
            common.dialog.error("Veuillez renseigner le code et la version.");
        }
        else {
            if ($("#TypeEcran").val() == "Offre") {
                ShowLoading();
                if ($("#copyOffreContrat").val() != "1")
                    CreateCopyOffre($("#CodeOffreContratCopy").val(), $("#VersionCopy").val());
                else
                    CopyExistOffre();
            }
            else if ($("#TypeEcran").val() == "Contrat") {
                ShowLoading();
                if ($("#copyOffreContrat").val() != "1")
                    CreateCopyContrat($("#CodeOffreContratCopy").val(), $("#VersionCopy").val());
                else
                    CopyContrat();
            }
        }
    });

    $("#btnRechPolice").kclick(function () {
        let $b = $(this);
        let root = $((window.parent || window).document);
        let doc = $(document);
        let iframe = root.find("#RechercheCopyOffreIFrame");
        let iframeDoc = $(iframe.contents());
        let searchCV = $("#chkCanevas").isChecked();
        let frameDiv = root.find("#divRechercheCopyOffre");

        iframe.attr('src', "/RechercheSaisie/GetRechercheCopieOffre?typeEcran=" + $("#TypeEcran").val() + "&searchInTemplate=" + $("#chkCanevas").isChecked());

        // Mise à jour de l'iframe de recherche
        if ($("#TypeEcran").val() == "Offre") {
            iframeDoc.find("#divGroupOffreContratTitle").html("Offre");
            iframeDoc.find("#CheckOffre").check();
            iframeDoc.find("#CheckContrat").uncheck();
            iframeDoc.find("#typeEcranRecherche").val("Offre");
        }
        else if ($("#TypeEcran").val() == "Contrat") {
            iframeDoc.find("#divGroupOffreContratTitle").html("Contrat");
            iframeDoc.find("#CheckContrat").check();
            iframeDoc.find("#CheckOffre").uncheck();
            iframeDoc.find("#typeEcranRecherche").val("Contrat");
        }

        frameDiv.css({ top: 5, left: $b.position().left });

        setTimeout(function () {
            frameDiv.show();
        }, 5);
    });

    $("#chkCanevas").off("change").change(function () {
        CheckCanevas();
    });
}
//------------Affiche les div creation du mode en parametre------
function AfficherDivCreation(type) {
    common.page.isLoading = true;
    try {
        $.ajax({
            type: "POST",
            url: "/RechercheSaisie/GetCreationSaisieDiv",
            data: { type: type },
            success: function (data) {
                DesactivateShortCut("");
                $("#divCreationContainer").html(data);
                MapElementDivCreation();
                $("#divCreate").show();
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request, true);
            }
        });
    } catch (e) {
        console.error(e);
        common.error.showMessage(e.message);
    }
}
//----------Active ou désactive les contrôles de la page
function ChangeAttrInput(e) {
    if (e.val() != "") {
        $("#divSearchCriteria :input").not("#NumAliment").disable(true);
        e.enable();
        $("input[type='radio'][name='modeNavigation']").enable();
        $("img[name=advancedSearch]").attr('src', '/Content/Images/loupegris.png').removeClass('CursorPointer');
    }
    else {
        $("#divSearchCriteria :input").enable();
        $("img[name=advancedSearch]").attr('src', '/Content/Images/loupe.png').addClass('CursorPointer');
    }
    if ($("#CritereParam").val() == "ContratOnly") {
        $("#Etat").disable();
        $("#Situation").disable();
    }
    toggleSearchEnabling();
    e.focus();
}

function toggleFilterEnabling() {
    if (!$("#OffreId").exists()) {
        return;
    }
    let ipb = $("#OffreId").val().trim();
    if (ipb) {
        $("[data-search-area='advanced'] :input").disable(true);
        $("[data-search-area='advanced'] .Loupe img").attr("src", "/Content/Images/loupegris.png").removeClass("CursorPointer");
        $("[data-search-area='main'] input[data-search-filter='advanced']").disable(true);
        $("#btnRechercher").enable();
    }
    else {
        $("#OffreId").clear();
        $("#NumAliment").clear();
        $("[data-search-area='main'] input[data-search-filter='advanced']").enable();
        $("[data-search-area='advanced'] :input").enable();
        $("[data-search-area='advanced'] .Loupe img").attr("src", "/Content/Images/loupe.png").addClass("CursorPointer");
    }
}

function toggleSearchEnabling() {
    if ($("#OffreId").length == 0) {
        return;
    }
    if ($("#OffreId").val().trim()) {
        $("#btnRechercher").enable();
        return true;
    }
    let ids = [
        //"PreneurAssuranceNom",
        "PreneurAssuranceCode",
        "PreneurAssuranceCP",
        "PreneurAssuranceVille",
        "PreneurAssuranceSIREN",
        "CabinetCourtageId",
        //"CabinetCourtageNom",
        "AdresseRisqueVoie",
        "AdresseRisqueCP",
        "AdresseRisqueVille",
        "MotsClefs",
        //"SouscripteurNom",
        //"GestionnaireNom",
        "SouscripteurCode",
        "GestionnaireCode",
        "DateDebut",
        "DateFin",
        "Branche[albemplacement=recherche]",
        "Cible[albemplacement=recherche]",
        "Etat",
        "Situation"
    ];

    let allowSearch = false;
    $(ids.map(function (e) { return "#" + e; }).join(",")).each(function (i, e) {
        if ($(e).val().trim()) {
            allowSearch = true;
            return false;
        }
    });
    if (allowSearch) {
        let cabinetCourtageId = $('input[id=CabinetCourtageId][albEmplacement=recherche]').val();
        let checkSaufEtat = $("#chkSauf").isChecked() && $("#Etat").val() == "";
        let checkApporteurGestionnaire = cabinetCourtageId != "" && !$('#chkApporteur').isChecked() && !$('#chkGestionnaire').isChecked();
        if (checkSaufEtat || checkApporteurGestionnaire) {
            allowSearch = false;
        }
    }
    else if ($("#chkActif").isChecked() ^ $("#chkInactif").isChecked()) {
        allowSearch = true;
    }
    $("#btnRechercher").prop({ disabled: !allowSearch });
    return allowSearch;
}

//-------Reset de la session de recherche--------
function ClearSessionSearch() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/ClearSession",
        success: function (data) {
            InitializeForm();
        },
        error: function (request) {
            common.error.showXhr(request, true);
        }
    });
}
//----------Réinitialise la page-----------------
function InitializeForm() {

    var searchInTemplate = $("#searchInTemplate").val();

    ReactivateShortCut();
    InitializeOffreContrat();
    InitializeAdvanced();
    InitializePreneur();
    InitializeCourtier();
    InitializeAddress();

    $("#selectedRow").clear();
    $("#oldSelectedRow").clear();
    $("#resultSorting").clear();

    $('input[id=SouscripteurCode][albEmplacement=recherche]').clear();
    $('input[id=GestionnaireCode][albEmplacement=recherche]').clear();
    $('input[id=CabinetCourtageId][albEmplacement=recherche]').clear();
    $('input[id=CabinetCourtageNom][albEmplacement=recherche]').clear();
    $('#Cible[albemplacement=recherche]').clear();
    $('#Branche[albemplacement=recherche]').clear();

    $("#chkActif").check();
    $("#chkInactif").check();
    $("#chkSauf").uncheck();
    $("#chkApporteur").check();
    $("#chkGestionnaire").check();

    $("#divSearchCriteria :input").enable();
    $("img[name=advancedSearch]").attr('src', '/Content/Images/loupe.png').addClass('CursorPointer');

    $("#btnDoubleSaisie").hide();
    $("#btnCreerOffre").hide();
    $("#btnEtablirContrat").hide();
    $("#divRechercherResult").hide();
    $("#btnRefusOffre").hide();
    $("#RechercherResultContainerDiv").addClass("notDisplayed");
    if ($("#CritereParam").val() != "ContratOnly")
        $("#btnRechercher").disable();
    $("#searchInTemplate").val(searchInTemplate);

    $("#modeStandard").check();
    $("#ModeNavig").val("S");

    $(".requiredField").removeClass("requiredField");

    if ($("#CritereParam").val() == "ContratOnly") {
        $("#Etat").disable();
        $("#Situation").disable();
    }

    $("#linkAlertesPreneur").parent().addClass("hide-it");
    $("#linkAlertesPreneur").removeAttr("title");

    paramDblSaisie = "";
}
//---------Initialise le groupe Offre/Contrat-----------
function InitializeOffreContrat() {
    $(":input[albGroup=OffreContrat]").clear()
        .uncheck()
        .removeAttr('selected')
        .enable();

    if ($("#CritereParam").val() == "Standard") {
        $("#CheckOffre").check();
        $("#CheckContrat").check();
    }
    else if ($("#CritereParam").val() == "ContratOnly") {
        $("#CheckContrat").check();
    }

    if ($("#typeEcranRecherche").val() == "Offre") {
        $("#CheckOffre").check();
        $("#CheckContrat").uncheck();
    }
    else if ($("#typeEcranRecherche").val() == "Contrat") {
        $("#CheckContrat").check();
        $("#CheckOffre").uncheck();
    }

    $("#OffreId").focus();
}
//---------Initialise le groupe Advanced-----------
function InitializeAdvanced() {
    $(":input[albGroup=Advanced]").clear()
        .uncheck()
        .removeAttr('selected')
        .enable()
        .removeClass('readonly');
    $("#DateType option:eq(2)").attr('selected', 'selected');
    if ($("#CritereParam").val() == "ContratOnly") {

        $("#Etat option:eq(4)").attr('selected', 'selected');
        $("#Situation option:eq(8)").attr('selected', 'selected');
        $("#Etat").disable();
        $("#Situation").disable();
        $("#btnRechercher").enable();
    }

}
//---------Initialise le groupe Preneur-----------
function InitializePreneur() {
    $(":input[albGroup=Preneur]").clear()
        .uncheck()
        .removeAttr('selected')
        .enable()
        .removeClass('readonly');

}
//---------Initialise le groupe Courtier-----------
function InitializeCourtier() {
    $(":input[albGroup=Courtier]").clear()
        .uncheck()
        .removeAttr('selected')
        .enable()
        .removeClass('readonly');

}
//---------Initialise le groupe Address-----------
function InitializeAddress() {
    $(":input[albGroup=Address]").clear()
        .uncheck()
        .removeAttr('selected')
        .enable()
        .removeClass('readonly');

}
//----------Refuser une offre------------
function RefusOffre() {
    ShowLoading();
    var codeOffre = paramDblSaisie.split("_")[0];
    var version = paramDblSaisie.split("_")[1];
    var codeMotif = $("#Refus").val();

    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/RefusOffre",
        data: { codeOffre: codeOffre, version: version, codeMotif: codeMotif },
        success: function (data) {
            $("#divRefusOffre").hide();
            common.dialog.info("L'offre res bien été mise à jour : <br/><b>Refusée : " + $("#Refus option:selected").text() + "</b>.");
            $("#divRecherche").hide();
        },
        error: function (request) {
            common.error.showXhr(request, true);
        }
    });
}
//----------Copie une offre dans une autre existante---------
function CopyExistOffre() {
    var codeOffre = paramDblSaisie.split("_")[0];
    var version = paramDblSaisie.split("_")[1];
    var codeOffreCopy = $("#CodeOffreCopy").val();
    var versionCopy = $("#VersionCopy").val();
    var type = "O";
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/CopyOffre",
        data: { codeOffre: codeOffre, version: version, codeOffreCopy: codeOffreCopy, versionCopy: versionCopy, type: type },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Créér une copie l'offre-----------------
function CreateCopyOffre(codeOffreCopy, versionCopy) {
    var newWindow = "";
    if ($("#divDataRechercheRapide").attr("albQuickSearch") == "true") {
        newWindow = "newWindow";
    } else {
        ShowLoading();
    }

    var type = "O";
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/CreateCopyOffre",
        data: { codeOffreCopy: codeOffreCopy, versionCopy: versionCopy, type: type, tabGuid: common.tabGuid, newWindow: newWindow },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Créér une copie du contrat-----------------
function CreateCopyContrat(codeContratCopy, versionCopy) {
    var newWindow = "";
    if ($("#divDataRechercheRapide").attr("albQuickSearch") == "true") {
        newWindow = "newWindow";
    } else {
        ShowLoading();
    }


    var type = "P";
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/CreateCopyOffre",
        data: { codeOffreCopy: codeContratCopy, versionCopy: versionCopy, type: type, tabGuid: common.tabGuid, newWindow: newWindow },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Vérifie si un critère de recherche est renseigné------------
function CheckSearchField() {
    var search = false;
    if ($("#OffreId").val() != undefined && $.trim($("#OffreId").val()) != "")
        search = true;
    if ($("#NumAliment").val() != undefined && $.trim($("#NumAliment").val()) != "" && $.trim($("#OffreId").val()) != "")
        search = true;
    if ($("#PreneurAssuranceNom").val() != undefined && $.trim($("#PreneurAssuranceNom").val()) != "")
        search = true;
    if ($("#PreneurAssuranceCode").val() != undefined && $.trim($("#PreneurAssuranceCode").val()) != "")
        search = true;
    if ($("#PreneurAssuranceCP").val() != undefined && $.trim($("#PreneurAssuranceCP").val()) != "")
        search = true;
    if ($("#PreneurAssuranceVille").val() != undefined && $.trim($("#PreneurAssuranceVille").val()) != "")
        search = true;
    if ($("#PreneurAssuranceSIREN").val() != undefined && $.trim($("#PreneurAssuranceSIREN").val()) != "")
        search = true;
    if ($("#CabinetCourtageId").val() != undefined && $.trim($("#CabinetCourtageId").val()) != "")
        search = true;
    if ($("#CabinetCourtageNom").val() != undefined && $.trim($("#CabinetCourtageNom").val()) != "")
        search = true;
    if ($("#AdresseRisqueVoie").val() != undefined && $.trim($("#AdresseRisqueVoie").val()) != "")
        search = true;
    if ($("#AdresseRisqueCP").val() != undefined && $.trim($("#AdresseRisqueCP").val()) != "")
        search = true;
    if ($("#AdresseRisqueVille").val() != undefined && $.trim($("#AdresseRisqueVille").val()) != "")
        search = true;
    if ($("#MotsClefs").val() != undefined && $.trim($("#MotsClefs").val()) != "")
        search = true;
    if ($("#SouscripteurNom").val() != undefined && $.trim($("#SouscripteurNom").val()) != "")
        search = true;
    if ($("#GestionnaireNom").val() != undefined && $.trim($("#GestionnaireNom").val()) != "")
        search = true;
    if ($("#DateDebut").val() != undefined && $.trim($("#DateDebut").val()) != "")
        search = true;
    if ($("#DateFin").val() != undefined && $.trim($("#DateFin").val()) != "")
        search = true;
    if ($("#Branche[albemplacement=recherche]").val() != undefined && $.trim($("#Branche[albemplacement=recherche]").val()) != "")
        search = true;
    if ($("#Cible[albemplacement=recherche]").val() != undefined && $.trim($("#Cible[albemplacement=recherche]").val()) != "")
        search = true;
    if ($("#Etat").val() != undefined && $.trim($("#Etat").val()) != "")
        search = true;
    if ($("#Situation").val() != undefined && $.trim($("#Situation").val()) != "")
        search = true;

    var etat = $('#Etat').val();
    var saufEtat = $('#chkSauf').isChecked();
    if (saufEtat == true && etat == "")
        search = false;


    var situationActif = $('#chkActif').isChecked();
    var situationInactif = $('#chkInactif').isChecked();

    if (situationActif && !situationInactif)
        search = true;

    if (!situationActif && situationInactif)
        search = true;

    var cabinetCourtageId = $('input[id=CabinetCourtageId][albEmplacement=recherche]').val();
    var cabinetCourtageIsApporteur = $('#chkApporteur').isChecked();
    var cabinetCourtageIsGestionnaire = $('#chkGestionnaire').isChecked();
    if (cabinetCourtageId != "" && !cabinetCourtageIsApporteur && !cabinetCourtageIsGestionnaire)
        search = false;

    if (search)
        $("#btnRechercher").enable();
    else
        $("#btnRechercher").disable();
}
//----------------RedirectionRechercheSaisie------------------
function RedirectionRechercheSaisie(cible, job, paramCreate, readOnlyConsult, typeAvtResil, newwindow) {
    if (readOnlyConsult == undefined)
        readOnlyConsult = "";
    var newWindow = "";
    if ($("#divDataRechercheRapide").attr("albQuickSearch") == "true" || newwindow == "newwindow" || $("#idBlacklistedPartenaire").val()) {
        newWindow = "newWindow";
    } else {
        ShowLoading();
    }
    var tabGuid = IsInIframe() ? $('#homeTabGuid', window.parent.document).val() : $('#tabGuid').val();
    let modeNavig = "modeNavig" + ($("#ModeNavig").val() || "S") + "modeNavig";
    if (paramCreate || cible && cible.toLowerCase() == "creationsaisie") {
        window.document.location.href = "/" + cible + "/" + job + "?id=" + encodeURIComponent(paramCreate + tabGuid + modeNavig);
    }
    else if (cible && cible.toLowerCase() == "ancreationcontrat") {
        window.document.location.href = "/" + cible + "/" + job + "?id=" + encodeURIComponent(tabGuid + modeNavig);
    }
    else {
        $.ajax({
            type: "POST",
            url: "/RechercheSaisie/Redirection/",
            data: {
                cible: cible, job: job, paramOffre: paramDblSaisie, paramCreate: paramCreate, tabGuid: tabGuid,
                modeNavig: "modeNavig" + $("#ModeNavig").val() + "modeNavig" + readOnlyConsult, newWindow: newWindow,
                typeAvt: $("#TypeAvt").val(), modeAvt: $("#ModeAvt").val(), ignoreReadonly: $("#ignoreReadonly").val(), typeAvtResil: typeAvtResil
            },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//--------------------Popup de la création saisie----------------
function CreateSaisie() {
    var param = "";
    var brancheVal = $('#Branche').val();
    var preneurAssuranceId = $('#PreneurAssuranceId').val();
    var preneurAssuranceNom = $("#PreneurAssuranceNom").val();

    if (preneurAssuranceNom == "") preneurAssuranceId = "";
    //vide l'id du preneur d'assurance si le nom est vide
  
    if (brancheVal != undefined && brancheVal.length > 0) {
        param = brancheVal;
    }
    ShowLoading();
    RedirectionRechercheSaisie("CreationSaisie", "Index", param);

}
//----------------------Ouverture de la fancy d'attente pendant la création de la nouvelle version de l'offre---------------------
function NouvelleVersionProgression() {
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var infoRows = $(selectRadio).val();
    var offreNum = infoRows.split("_")[0];
    var offreVersion = infoRows.split("_")[1];
    var offreType = infoRows.split("_")[2];
    var tr = $(selectRadio).parent().parent();
    ShowCommonFancy("Wait", "", "Veuillez patienter pendant la création<br /> de la nouvelle version", 350, 130, true, true);
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/CreationNouvelleVersionOffre",
        data: { codeOffre: offreNum, version: parseInt(offreVersion), type: offreType, etat: $(tr).children(".tdEtat").html(), situation: $(tr).children(".tdSituation").html() },
        success: function (data) {
           testCreationNouvelleVersion = data;
           if (data) {
                setTimeout(function () {
                    //common.dialog.info("Nouvelle version n°<b>" + (parseInt(derniereVersion) + 1) + "</b> créée avec succès<br />Cliquez sur OK pour y accéder");
                    //btnInfoOkClic();
                    $("#btnInfoOk").click();
                }, 2500);
            }
            else {
                setTimeout(function () {
                    common.dialog.info("Erreur lors de la création de la nouvelle version");
                }, 2500);
            }
        },
        error: function (request) {
            CloseCommonFancy();
            common.error.showXhr(request, true);
        }
    });
}
//---------------------Effectue la recherche----------------------
function RechercheOffres(RechercheButton, Reinit) {
    $(".requiredField").removeClass("requiredField");
    if ($("#DateDebut").val() != "" && !isDate($('#DateDebut').val())) {
        $('#DateDebut').addClass("requiredField");
        return false;
    }
    if ($("#DateFin").val() != "" && !isDate($('#DateFin').val())) {
        $('#DateFin').addClass("requiredField");
        return false;
    }
    if ($("#OffreId").val() == "" && $('#DateDebut').val() != "" && $('#DateFin').val() != "") {
        var checkD = checkDate($('#DateDebut'), $('#DateFin'));
        if (!checkD) {
            ShowCommonFancy("Error", "",
                "Veuillez sélectionner une date de début antérieure<br/> à la date de fin.",
                350, 70, true, true);
            return false;
        }
    }

    var pageNumberRecherche = $("#PaginationPageActuelle").html();
    if (pageNumberRecherche == null || undefined) {
        pageNumberRecherche = 1;
    }

    $("#RechercherResultContainerDiv").addClass("notDisplayed");
    $("#divRechercherResult, #btnDoubleSaisie, #btnCreerOffre, #btnEtablirContrat", "#btnMHA").hide();

    var codeOffreContrat = $('#OffreId').val().toUpperCase();
    var numAliment = $("#NumAliment").val();

    var CheckOffre = $('#CheckOffre').isChecked();
    var CheckContrat = $('#CheckContrat').isChecked();
    var CheckAliment = $('#CheckAliment').isChecked();
    if ($("#CritereParam").val() == "ContratOnly") {
        CheckOffre = false;
        CheckContrat = true;
    }
    else if ($("#CritereParam").val() == "CopyOffre") {
        CheckOffre = true;
        CheckContrat = false;
    }
    if (!CheckOffre && !CheckContrat && codeOffreContrat == "" && !CheckAliment) {
        ShowCommonFancy("Error", "",
            "Veuillez indiquer si vous souhaitez réaliser la recherche<br /> sur les offres et/ou les contrats.",
            350, 70, true, true);
        return false;
    }

    var cabinetCourtageId = $('input[id=CabinetCourtageId][albEmplacement=recherche]').val();
    var cabinetCourtageNom = $('input[id=CabinetCourtageNom][albEmplacement=recherche]').val().toUpperCase();
    var cabinetCourtageIsApporteur = $('#chkApporteur').isChecked();
    var cabinetCourtageIsGestionnaire = $('#chkGestionnaire').isChecked();

    var preneurAssuranceCode = $('input[id=PreneurAssuranceCode][albEmplacement=recherche]').val();
    var preneurAssuranceNom = $('input[id=PreneurAssuranceNom][albEmplacement=recherche]').val().toUpperCase();
    var preneurAssuranceCP = $('#PreneurAssuranceCP').val();
    //mise en commentaire des villes avant correction
    var preneurAssuranceVille = "";
    var preneurAssuranceSIREN = $('#PreneurAssuranceSIREN').val();
    var preneurAssuranceDEP = $('#PreneurAssuranceDEP').val();

    var adresseRisqueVoie = $('#AdresseRisqueVoie').val();
    var adresseRisqueCP = $('#AdresseRisqueCP').val();
    //mise en commentaire des villes avant correction
    var adresseRisqueVille = "";
    var motsClefs = $('#MotsClefs').val();
    var souscripteur = $('input[id=SouscripteurCode][albEmplacement=recherche]').val().toUpperCase();
    var souscripteurNom = $('input[id=SouscripteurNom][albEmplacement=recherche]').val();
    var gestionnaire = $('input[id=GestionnaireCode][albEmplacement=recherche]').val().toUpperCase();
    var gestionnaireNom = $("input[id='GestionnaireNom'][albEmplacement='recherche']").val();
    var dateType = $('#DateType').val();
    var dateDebut = $('#DateDebut').val();
    var dateFin = $('#DateFin').val();
    var branche = $('#Branche[albemplacement=recherche]').val();
    var cible = $('#Cible[albemplacement=recherche]').val();

    var etat = $('#Etat').val();
    var saufEtat = $('#chkSauf').isChecked();

    var situation = $('#Situation').val();
    var situationActif = $('#chkActif').isChecked();
    var situationInactif = $('#chkInactif').isChecked();

    var sorting = $("#resultSorting").val();
    var etatTemplate = $("#searchInTemplate").val();

    var typeContrat = "";
    if ($("#typeRecherche").length > 0)
        typeContrat = $("#typeRecherche").val();

    var modeNavig = $("#modeHisto").isChecked() ? $("#modeHisto").val() : $("#modeStandard").val();

    ShowLoading();

    var filter = {
        CodeOffre: codeOffreContrat,
        NumAliment: numAliment,
        CabinetCourtageId: cabinetCourtageId,
        CheckOffre: CheckOffre,
        CheckContrat: CheckContrat,
        CheckAliment: CheckAliment,
        CabinetCourtageNom: cabinetCourtageNom,
        CabinetCourtageIsApporteur: cabinetCourtageIsApporteur,
        CabinetCourtageIsGestionnaire: cabinetCourtageIsGestionnaire,
        PreneurAssuranceCode: preneurAssuranceCode,
        PreneurAssuranceNom: preneurAssuranceNom,
        PreneurAssuranceCP: preneurAssuranceCP,
        PreneurAssuranceDEP: preneurAssuranceDEP,
        PreneurAssuranceVille: preneurAssuranceVille,
        PreneurAssuranceSIREN: preneurAssuranceSIREN,
        PreneurSinistres: ($("#linkAlertesPreneur").data("alertes") || "").indexOf("'S'") > -1,
        PreneurImpayes: ($("#linkAlertesPreneur").data("alertes") || "").indexOf("'I'") > -1,
        PreneurRetardsPaiement: ($("#linkAlertesPreneur").data("alertes") || "").indexOf("'RP'") > -1,
        AdresseRisqueVoie: adresseRisqueVoie,
        AdresseRisqueCP: adresseRisqueCP,
        AdresseRisqueVille: adresseRisqueVille,
        MotsClefs: motsClefs,
        SouscripteurCode: souscripteur,
        SouscripteurNom: souscripteurNom,
        GestionnaireCode: gestionnaire,
        GestionnaireNom: gestionnaireNom,
        TypeDateRecherche: dateType,
        DateDebut: dateDebut,
        DateFin: dateFin,
        Branche: branche,
        Cible: cible,
        Etat: etat,
        SaufEtat: saufEtat,
        Situation: situation,
        IsActif: situationActif,
        IsInactif: situationInactif,
        IsTemplate: etatTemplate,
        TypeContrat: typeContrat,
        Sorting: sorting,
        PageNumber: pageNumberRecherche,
        CritereParam: $("#CritereParam").val(),
        modeNavig: modeNavig
    };

    if (window.sessionStorage && $("#CritereParam").val() == "Standard") {
        window.sessionStorage.setItem('recherche_filter', JSON.stringify(filter));
    }

    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/Recherche",
        data: filter,
        success: function (data) {
            AlbScrollTop();
            DesactivateShortCut();
            $("#divDataRecherche").html(data);

            if ($("#ResultCount").val() == "0") {
                $("#divResultatsBody").html("&nbsp&nbsp<b>Aucun résultat à votre recherche</b>");
            }

            AlbScrollTop();
            $("#divRechercherResult").show();
            $("#RechercherResultContainerDiv").removeClass("notDisplayed");
            AddOnClickButton();
            AddOnClickRadio();
            AddOnClickRow();
            AddOnHideBandeau();

            // vérifie le nombre de ligne retournée
            MapSearchElement();
            MapCommonAutoCompActionMenu();
            CheckNbRowResult();
            setSorting(sorting);
            CloseLoading();
            $("#divRecherche").show();
            $("#btnNewRechercher").focus();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
    function setSorting(sorting) {
        if (sorting) {
            let sortBase = $("#divRecherche input.field-hide[type='image'][value='" + sorting + "']").parent();
            let sortDefault = sortBase.children("input.field-show[type='image']");
            let sortOther = sortBase.children("input.field-hide[type='image'][value!='" + sorting + "']");
            sortDefault.addClass("field-hide").removeClass("field-show");
            sortOther.addClass("field-show").removeClass("field-hide");
        }
    }
}

function loadFilter() {

    if (!window.sessionStorage) return;

    var filter = JSON.parse(window.sessionStorage.getItem('recherche_filter'));

    if (!filter) return;

    $('#CheckOffre').prop({ checked: filter.CheckOffre });
    $('#CheckContrat').prop({ checked: filter.CheckContrat });
    $('#OffreId').val(filter.CodeOffre);
    $('#NumAliment').val(filter.NumAliment);
    $('#modeHisto').prop({ checked: filter.modeNavig === $("#modeHisto").val() });
    $('#modeStandard').prop({ checked: filter.modeNavig === $("#modeStandard").val() });
    $('#MotsClefs').val(filter.MotsClefs);
    $('#SouscripteurNom').val(filter.SouscripteurNom);
    if (filter.SouscripteurNom != "") {
        $("#SouscripteurSelect").val("1");
        $("#SouscripteurCode").val(filter.SouscripteurCode);
    }


    $('#GestionnaireNom').val(filter.GestionnaireNom);
    if (filter.GestionnaireNom != "") {
        $("#GestionnaireSelect").val("1");
        $("#GestionnaireCode").val(filter.GestionnaireCode);
    }

    $('#DateType').val(filter.TypeDateRecherche);
    $('#DateDebut').val(filter.DateDebut);
    $('#DateFin').val(filter.DateFin);
    $('#chkActif').prop({ checked: filter.IsActif });
    $('#chkInactif').prop({ checked: filter.IsInactif });
    $('#Branche').val(filter.Branche);
    $('#Cible').val(filter.Cible);
    $('#chkSauf').prop({ checked: filter.SaufEtat });
    $('#Etat').val(filter.Etat);
    $('#Situation').val(filter.Situation);

    $('#PreneurAssuranceCode').val(filter.PreneurAssuranceCode);
    $('#PreneurAssuranceNom').val(filter.PreneurAssuranceNom);
    if (filter.PreneurAssuranceCode && !(filter.PreneurAssuranceNom || "").trim());
    {
        $('#PreneurAssuranceCode').trigger("change");
    }

    $('#PreneurAssuranceCP').val(filter.PreneurAssuranceCP);
    if (filter.PreneurAssuranceCode != "" && typeof showAlertMessageAssure == "function") {
        showAlertMessageAssure({ Sinistre: filter.PreneurSinistres, Impayes: filter.PreneurImpayes, RetardsPaiements: filter.PreneurRetardsPaiement, Code: filter.PreneurAssuranceCode }, true);
    }

    $('#PreneurAssuranceSIREN').val(filter.PreneurAssuranceSIREN);

    $('#CabinetCourtageId').val(filter.CabinetCourtageId);
    $('#CabinetCourtageNom').val(filter.CabinetCourtageNom);
    if (filter.CabinetCourtageId && !(filter.CabinetCourtageNom || "").trim());
    {
        $('#CabinetCourtageId').trigger("change");
    }
    $('#chkApporteur').prop({ checked: filter.CabinetCourtageIsApporteur });
    $('#chkGestionnaire').prop({ checked: filter.CabinetCourtageIsGestionnaire });

    $('#AdresseRisqueVoie').val(filter.AdresseRisqueVoie);
    $('#AdresseRisqueCP').val(filter.AdresseRisqueCP);
    $('#AdresseRisqueVille').val(filter.AdresseRisqueVille);

    toggleFilterEnabling();
    toggleSearchEnabling();

    $("#ModeNavig").val($("input[type='radio'][name='modeNavigation']:checked").val());

}
//fonction confirm Ok
function btnInfoOkClic() {
    if (testCreationNouvelleVersion) {
        let selectRadio = $('input[type=radio][name=RadioRow]:checked');
        let offre = $(selectRadio).val().split("_");
        offre[1] = parseInt(offre[1]) + 1;
        $(selectRadio).val(offre.join("_"));
        recherche.affaires.results.consultOrEdit("btnCreerOffre");
    }
    else {
        CloseCommonFancy();
        let action = $("#hiddenAction").val() || "";
        try {
            $("#hiddenAction").clear();
            if (action.indexOf("url:") === 0 && action !== "url:") {
                try {
                    document.location.assign(action.split(":")[1]);
                }
                catch (e) {
                    console.error(e);
                }
            }
            else {
                switch (action) {
                    case "checkLock":
                        CommonRedirect($("#hiddenParamCheckLock").val());
                        break;
                    case "OpenHorsAvn":
                        var url = $("#hiddenParamCheckLock").val();
                        CommonRedirect(url.replace('ConsultOnlyAndEdit', 'ConsultOnly'));
                        break;
                    default:
                        break;
                }
            }
        }
        catch (e) {
            $("#hiddenAction").val(action);
            console.error(e);
        }
    }
}
//------------Map les éléments de la recherche------------------
function MapSearchElement() {
    $("#btnCreerRechSaisie").kclick(function () {
        AlbScrollTop();
        $("#LoadingRechDiv").show();
        $("#btnCreerSaisie").trigger('click');
    });
    $("#btnCreerRechContrat").kclick(function () {
        $("#btnCreerContrat").trigger('click');
    });
    $("#btnInitializeRech").kclick(function () {
        if ($("#idBlacklistedPartenaire").val()) {
            if ($("#PreneurAssurance_Numero").val()) {
                $("#PreneurAssurance_Numero").clear();
                $("#PreneurAssurance_Nom").clear();
                $("#PreneurAssurance_Departement").clear();
                $("#PreneurAssurance_Ville").clear();
            }
            else {
                $("#PreneurAssurance_CodePreneurAssurance").clear();
                $("#PreneurAssurance_NomPreneurAssurance").clear();
                $("#PreneurAssurance_Departement").clear();
                $("#PreneurAssurance_Ville").clear();
            }
            $("#lnkBlackList").hide();
        }
        $.contextMenu('destroy', "tr[albcontextmenu=O]");
        $("#divRechercherResult").html("");
        $("#divRecherche").hide();
        DesactivateShortCut("resultRecherche");
        $("#btnInitialize").trigger("click");
    });
    $("#btnNewRechercher").kclick(function () {
        $.contextMenu('destroy', "tr[albcontextmenu=O]");
        $("#divRechercherResult").html("");
        $("#divRecherche").hide();
        paramDblSaisie = "";
        $("#selectedRow").clear();
        $("#oldSelectedRow").clear();
        $("#resultSorting").clear();
        ReactivateShortCut();
    });
    $("#btnActionNouveau").kclick(function (e) {
        e.preventDefault();
        var pos = $("#btnActionNouveau").offset();
        $("#btnActionNouveau").contextMenu({ x: pos.left + $("#btnActionNouveau").width() + 22, y: pos.top });
    });
    $("#btnActionMenu").kclick(function (e) {
        e.preventDefault();
        var pos = $("#btnActionMenu").offset();
        $("#btnActionMenu").contextMenu({ x: pos.left + $("#btnActionMenu").width() + 22, y: pos.top });
    });
    $("#btnAvenant").kclick(function (e) {
        e.preventDefault();
        var pos = $("#btnAvenant").offset();
        $("#btnAvenant").contextMenu({ x: pos.left + $("#btnAvenant").width() + 22, y: pos.top });
    });
    $("#btnAvtResil").kclick(function (e) {
        CreateAvntResil();

    });
    $("#btnOkCustomAction").kclick(function () {
        var action = $("#customActionId").val();
        contextMenu.triggerMenuAction(action, null);
    });


    HideButtonMenu();
}
//-------------Vérifie le nombre de ligne retournées par la recherche----------
function CheckNbRowResult() {
    var nbRow = $("input[type=radio][id^=SelectRow]").length;
    if (nbRow == 1)
        $("input[type=radio][id^=SelectRow]").trigger('click');
    else
        contextMenu.createMenuNouveau("", "", "");
}
//-------------------------Affecte l'évènement à chaque ligne pour afficher
//la sous grille-------------------------------
function AddOnClickButton() {
    try {
        $('img[name=ExpandRow]').kclick(function (index) {
            if ($("#SubGridRows" + index).css('display') == 'none') {
                $("#SubGridRows" + index).show();
                $(this).attr("src", "/Content/Images/Op.png");
                $(this).parent().attr("rowspan", 2);
            } else {
                $("#SubGridRows" + index).hide();
                $(this).attr("src", "/Content/Images/Cl.png");
                $(this).parent().attr("rowspan", 1);
            }
        });
    }
    catch (e) {
        $.fn.jqDialogErreurOpen(e);
    }
}
//-------------------------Affecte l'évènement à chaque ligne pour renseigner
//les informations de l'offre ou du contrat------------------------
function AddOnClickRadio() {
    $("input[type='radio'][name='RadioRow']").kclick(function (ev) {
        ev.stopPropagation();
        $("#spanMsgInfo").hide();
        $("#btnCreerOffre").disable();
        let allowEdit = false;
        let infoRows = $(this).val();
        let offreNum = infoRows.split("_")[0];
        let offreVersion = infoRows.split("_")[1];
        let offreType = infoRows.split("_")[2];
        let numAvenant = infoRows.split("_")[3];
        let offreCopyOffre = "";
        let tr = $(this).parent().parent();
        let offreEtat = $(tr).children(".tdEtat").html();
        let offreSituation = $(tr).children(".tdSituation").html();
        let offrePeriodicite = $("input[name=Periodicite][albContext=" + infoRows + "]").val();
        let branche = $(tr).children(".tdBranche").html();
        let msgSuspension = $(tr).attr("albmsgsuspension");
        let typeContratMere = $("input[name=TypeContratMere][albContext=" + infoRows + "]").val();
        let typeAccord = $(this).attr("albtypeaccord");
        let kheopsStatut = $(this).attr("albkheopsstatut");
        let generdoc = $(this).attr("albgenerdoc");
        let typeAvt = $(this).attr("albtypeavt");
        let modeNavig = $("#ModeNavig").val();
        if (typeContratMere == null) {
            typeContratMere = "";
        }

        contextMenu.createMenuNouveau(offreType, offreEtat, branche);
        if (kheopsStatut != "KHE") {
            allowEdit = false;
            $("#btnActionMenu, #btnAvenant, #btnAttestations, #btnCreerOffre, #btnMHA").disable().hide();
            $("#divButtonPrincipal").hide();
            $("#btnOpenInCitrix").kclick(function () {
                window.location.href = "Albinprod:GererContrat?action=VISUCONTRAT?type=" + offreType + "?ipb=" + offreNum + "?Alx=" + offreVersion + "";
            });
            $("#divButtonCitrix").show();
            $("#btnSelectionnerOffre").disable().hide();
            return;
        }
        $("#btnSelectionnerOffre").enable().show();
        $("#divButtonPrincipal").show();
        $("#divButtonCitrix").hide();
        $("#btnOpenInCitrix").off();

        contextMenu.createMenuPrincipalOnButton(infoRows, offreType, offreEtat, offreSituation, offrePeriodicite, offreCopyOffre, modeNavig, branche, typeAccord, generdoc, typeAvt, kheopsStatut);

        $("#btnCreerOffre, #btnFolderReadOnly").enable().show();

        if (msgSuspension !== "") {
            $("#spanMsgInfo").text(msgSuspension).show();
        }
        if (modeNavig == "S") {
            allowEdit = true;
        }
        $("#btnActionMenu").enable();
        $("#btnCreerOffre").html("<u>M</u>odifier").assignAccessKey("m");
        $(["#btnAvenant", "#btnAttestations", "#btnRepriseAvt", "#btnAvtResil", "#btnMHA"].join(",")).disable().hide();

        if (offreType == "O") {
            $("#btnCreerOffre").attr("albModeBtn", "Offre").attr("title", "Modifier l'offre");
            if (offreEtat == "R") {
                allowEdit = false;
            }
        }
        else if (offreType == "P") {
            if (offreEtat != "V") {
                $("#btnCreerOffre").attr("albModeBtn", "Contrat").attr("title", "Modifier le contrat en cours");
                if (offreEtat == "R") {
                    allowEdit = false;
                }
                if (offreSituation == "V" && offrePeriodicite != "U" && offrePeriodicite != "E") {
                    $("#btnAvtResil").enable().show();
                }
                if (offreEtat == "N" && numAvenant != "" && numAvenant != "0") {
                    $("#btnRepriseAvt").enable().show();
                }
            }
            else {
                $("#btnCreerOffre").html("Retour <u>P</u>ièces").attr("albModeBtn", "PieceSignee").attr("data-accesskey", "p").attr("title", "Retour Pièces");
                $("#btnAvenant").enable().show();
                if (offreSituation == "A" && (generdoc || "0") == "0") {
                    $("#btnMHA").enable().show();
                }
            }
            if (offreEtat == "V" || (numAvenant != "" && numAvenant != "0")) {
                $("#btnAttestations").enable().show();
            }
            if ((typeAvt == "REGUL" || typeAvt == "AVNRG")) {
                if (offreEtat != "V") $("#btnCreerOffre").disable().hide();
                $("#btnAvenant").enable().show();
            }
        }
        if (($("#btnCreerOffre").attr("albModeBtn") != "PieceSignee") && (offreSituation != "" && offreSituation != "A" && offreSituation != "X")) {
            allowEdit = false;
        }
        //contextMenu.createMenuNouveau(offreType, offreEtat, branche);
        HideButtonMenu();
        if (offreType == "P" && (offreEtat == "V" || typeAvt == "REGUL" || typeAvt == "AVNRG")) {
            let albBrancheRights = $("#btnAvenant").attr("albBrancheRights");
            if ((generdoc || "0") == "0" && (albBrancheRights && albBrancheRights.indexOf(";" + $.trim(branche) + ";") >= 0 || albBrancheRights === ";**;")) {
                contextMenu.createMenuAvenant(offreType, offreEtat, branche, offrePeriodicite, offreSituation, typeAccord, numAvenant, typeAvt);
            }
            else {
                $("button[albGroupBtn='avn']").hide();
            }
        }
        if (generdoc != "0") {
            allowEdit = false;
            $("#btnAttestations").disable().hide();
        }
        paramDblSaisie = offreNum + "_" + offreVersion + "_" + offreType + "_" + numAvenant;
        $("#selectedOffreContrat").val(offreNum + "_" + offreVersion + "_" + offreType + "_" + numAvenant + "_" + typeContratMere + "_" + offreEtat);

        // Gestion du disable ou non du bouton "modifier"
        if (allowEdit) {
            $("#btnCreerOffre").enable();
        }
        if (offreSituation === "W" && offreType != "O") {
            $("#btnCreerOffre").hide();
        }
        if (modeNavig == "H") {
            $("#btnAvenant").disable().hide();
            $("#btnAttestations").disable().hide();
        }
    });
}

function GetBandeau(tId, context) {
    if (tId != undefined) {
        $("#isLoadingBandeau").val("1");
        var modeNavig = $("#modeHisto").isChecked() ? $("#modeHisto").val() : $("#modeStandard").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/RechercheSaisie/GetBandeau",
            data: { id: tId, modeNavig: modeNavig },
            success: function (data) {
                if (data != null && data != "") {
                    $("#divConteneurBandeauRecherche").html(data);
                    var divHeight = $("#divInfoBandeauRecherche").height();
                    var divWidth = $("#divInfoBandeauRecherche").width();
                    $("#divInfoBandeauRecherche").css({ 'position': 'absolute', 'top': 20 + 'px' }).show();
                    $(".backgroundPink").each(function () {
                        $(this).removeClass('backgroundPink');
                    });
                    $("tr[albContext=" + context + "]").addClass('backgroundPink');
                    MapLinkWinOpen();
                    $("#divConteneurBandeauRecherche").show();
                    $("#isLoadingBandeau").val("0");
                }
                CloseLoading();
            },
            error: function (request) {
                $("#isLoadingBandeau").val("0");
                common.error.show(request);

            }
        });
    }
}

//---------affecte l'affichage du bandeau au click sur chaque ligne des résultats de la recherche
function AddOnClickRow() {
    $(".affair-row td[name!=radiotd]").kclick(function () {
       if ($("#isLoadingBandeau").val() == 0) {
            let tId = $(this).attr("albContext");
            $("#selectedRow").val(tId);
            $("#oldSelectedRow").val($("#selectedRow").val());
            let idParts = tId.split("_");
            if (!$("#modeHisto").isChecked()) {
                idParts.pop();
           }

          /* affaire.showInfosPopup(
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
              // null,//},
              // MapLinkWinOpen
           //);*/
           affaire.showInfosPopup(
               idParts[0],
               idParts[1],
               idParts[2],
               idParts.length === 3 ? "" : ("/" + idParts[3]),
               true,
               function (result) {
                   const elements = common.$removeEnclosingDiv($.parseHTML(result));
                   return Array.from(elements).reduce(function (a, e) { return a + e.outerHTML; }, "");
               },
               function () {
                   $("#full_synthese").show();
                   MapLinkWinOpen();
               },true
           );
            //common.page.navigateNewTab("/SyntheseAffaire/Index/" + idParts[0] + "/" + idParts[1] + "/" + idParts[2] + (idParts.length === 3 ? "" : ("/" + idParts[3])));
       }
      
       
    });
    $(".affair-row td[name=radiotd]").kclick(function () {
        if ($("#isLoadingBandeau").val() == 0) {
            $(this).children('[type=radio]').click();
        }
    });
}

//----------fonction qui gère les évenements qui cachent le bandeau d'information
function AddOnHideBandeau() {
    if ($("#divRecherche").length > 0) {
        $("#divRecherche").kclick(function () {
            $("#divInfoBandeauRecherche").hide();
            $("#divConteneurBandeauRecherche").hide();
            $("#oldSelectedRow").val($("#selectedRow").val());
            $("#selectedRow").clear();
            $(".backgroundPink").each(function () {
                $(this).removeClass('backgroundPink');
            });
        });
    }

    if ($("#divRechercheRapide").length > 0) {
        $("#divRechercheRapide").kclick(function () {
            $("#divInfoBandeauRecherche").hide();
            $("#divConteneurBandeauRecherche").hide();
            $("#oldSelectedRow").val($("#selectedRow").val());
            $("#selectedRow").clear();
            $(".backgroundPink").each(function () {
                $(this).removeClass('backgroundPink');
            });
        });
    }

    $("#divConteneurBandeauRecherche").kclick(function (e) {
        return false;
    });
}

function RechercheAvanceeCourtier(contexte) {
    $("button[name=Rechercher][albcontext=" + contexte + "]").each(function () {
        $(this).die();
        $(this).live('click', function () {
            RechercheCabinetsCourtiers("ASC", 1, contexte);
        });
    });
}

function RechercheCabinetsCourtiers(Order, By, contexte) {
    $("span[name=messageAffinement][albcontext=" + contexte + "]").html('');
    ShowLoading();
    if ($("input[name=CodeCabinetRecherche][albcontext=" + contexte + "]").val().length > 0) {
        $.ajax({
            type: "POST",
            url: "/RechercheSaisie/GetCabinetsCourtagesRechercherByCode",
            data: { CodeCabinetRecherche: $("input[name=CodeCabinetRecherche][albcontext=" + contexte + "]").val(), contexte: contexte },
            success: function (data) {
                $("div[name=resultRecherche][albcontext=" + contexte + "]").html(data);
                SelectCourtier(contexte);
                AlternanceLigne("CabinetsCourtageBody", "Code", true, null);
                tri();
                if (data.length == 0) {
                    ShowDialogInFancy("Error", "Aucun résultat", 300, 65);
                    $("#btnDialogOk").click(function () {
                        $("#divDialogInFancy").hide();
                    });
                }
                CloseLoading();
            }
        });
    }
    else {
        var PageNumber = $("#CabinetCourtagePaginationPageActuelle").html();
        if (PageNumber == null) {
            PageNumber = 1;
        }
        $.ajax({
            type: "POST",
            url: "/RechercheSaisie/GetCabinetsCourtagesRechercherByName",
            data: "NomCabinetRecherche=" + $("input[name=NomCabinetRecherche][albcontext=" + contexte + "]").val() + "&PageNumber=" + PageNumber + "&Order=" + Order + "&By=" + By + "&contexte=" + contexte,
            success: function (data) {
                $("div[name=resultRecherche][albcontext=" + contexte + "]").html(data);
                SelectCourtier(contexte);
                if (parseInt($("#NbCountCabinetCourtage").val()) > parseInt($("#LineCountCabinetCourtage").val())) {
                    $("#MsgAffinementCabinetCourtage").show();
                    $("#MsgAffinementCabinetCourtage").html("Le nombre réel de lignes trouvées dépasse " + $("#LineCountCabinetCourtage").val() + ", veuillez affiner votre recherche");
                }
                AlternanceLigne("CabinetsCourtageBody", "Code", true, null);
                tri();
                if (data.length == 0) {
                    ShowDialogInFancy("Error", "Aucun résultat", 300, 65);
                    $("#btnDialogOk").click(function () {
                        $("#divDialogInFancy").hide();
                    });
                }
                CloseLoading();
            }
        });
    }
    $("#LoadingDivSearch").hide();
}
// Cas particulier d'affichege message de la validation du courtier
function canShowMsgCourtier() {
    var result = true;
    // correction de bug d'affichage message Courtier apporteur
    var checkboxCopyCourtier = $("#GpIdentiqueApporteur");
    var courtierGestion = $("input[albAutoComplete=autoCompCodeCourtierGestion]");
    var courtierApporteur = $("input[albAutoComplete=autoCompCodeCourtier]");
    var isEcranCourtier = courtierGestion.length > 0 && courtierApporteur.length > 0;
    //  si l'ecran est "Ecran courtier"
    if (isEcranCourtier) {
        // test sur le checkbox du copier courtier gestion en courtier apporteur
        result = checkboxCopyCourtier.length > 0 ? checkboxCopyCourtier.isChecked() : false;
    }
    return result;
}
function SelectCourtier(contexte) {
    $("#tblCabinetsCourtageBody tr").kclick(function () {
        $('#CourtierInvalideDiv').empty().removeClass("error").removeClass("warning");
        var code = $(this).find("td").eq(0).html();
        var nom = rhtmlspecialchars($(this).find("td").eq(1).html());

        $("input[albname=codeCourtier][albcontext=" + contexte + "]").val($.trim(code));
        $("input[albname=nomCourtier][albcontext=" + contexte + "]").val($.trim(nom));
        $("img[albcontext=" + contexte + "]").attr("albIdInfo", $.trim(code));
        $("input[albcontext=" + contexte + "][albAutoComplete=autoCompNomInterlocuteurGestion]").clear();
        $("input[albcontext=" + contexte + "][albAutoComplete=autoCompCodeInterlocuteurGestion]").clear();
        $("input[albcontext=" + contexte + "][albAutoComplete=autoCompCodeInterlocuteurGestion]").attr("albIdInterlocuteur", "");
        $("input[albcontext=" + contexte + "][name=Reference]").clear();
        $("button[name=closeAdvanced][albcontext=" + contexte + "]").trigger('click');
        $("input[albAutoComplete=autoCompCodeCourtierGestion]").trigger("change");
        var showMsgCourtier = canShowMsgCourtier();
        var msg = $(this).attr("albvalidInvalid");
        var demarche = $(this).attr("albdemarche");
        if (msg == "invalide") {
            if (code == "") {
                if (showMsgCourtier) {
                    $('#CourtierInvalideDiv').append("Code inexistant").addClass("error");
                }
            }
            else {
                $("#inInvalidCourtierApp").val("1");
                if (showMsgCourtier) {
                    $('#CourtierInvalideDiv').append("Code fermé").addClass("error");
                    $('#CourtierApporteurInvalideDiv').append("Code fermé").addClass("error");
                }
            }
            if ($("#RemplacerCourtier")) {
                $("#RemplacerCourtier").disable();
                $("#MaintenirCourtier").check().trigger("change");
                $("#MotifRemp").clear().disable();
            }
        }
        else {
            if ($("#RemplacerCourtier")) {
                $("#RemplacerCourtier").enable();
            }
        }
        if (demarche == "false" && msg == "valide" && showMsgCourtier) {
            $('#CourtierInvalideDiv').append("Interdit de démarche commerciale ").addClass("warning");
        }
    });
}

/* Recherche Avancé fin */
//-------------------------------------------------------

//----------------------Tri de cabinets de courtage côté serveur---------------------
function tri() {
    $("th.TablePersoTdHead").each(function () {
        $(this).css('cursor', 'pointer');
        var Colonne = $(this);
        var context = $("#albContextResultCourtier").val();
        Colonne.click(function () {
            if ($(".imageTri").attr("src") != undefined) {
                var img = $(".imageTri").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1);
                img = img.substr(0, img.lastIndexOf('.'));
                AlternanceLigne("CabinetsCourtageBody", "Code", true, null);
                if (img == "tri_asc") {
                    $(".imageTri").attr("src", "../../Content/Images/tri_asc.png");
                    RechercheCabinetsCourtiers("desc", Colonne.attr("id"), context);
                }
                else if (img == "tri_desc") {
                    $(".imageTri").attr("src", "../../Content/Images/tri_desc.png");
                    RechercheCabinetsCourtiers("asc", Colonne.attr("id"), context);
                }
                $(".spImg").css('visibility', 'hidden');
                $(this).children(".spImg").css('visibility', 'visible');
            }
        });
    });
}

function CloseRechAvancee() {
    $("#btnCloseAdvanced").kclick(function () {
        $("div[albname=divDataRechercheAvancee]").html("");

        $("div[albname=divRechercheAvancee]").hide();
    });
}

function Pagination() {
    //----------------------Pagination de la recherche d'offres---------------------
    $("#PaginationPrecedent").kclick(function () {
        var PaginationPageActuelle = parseInt($("#PaginationPageActuelle").html());
        PaginationPageActuelle--;
        $("#PaginationPageActuelle").html(PaginationPageActuelle);
        RechercheOffres($("#btnRechercher, input[name='tri']"), false);
    });
    $("#PaginationSuivant").kclick(function () {
        var PaginationPageActuelle = parseInt($("#PaginationPageActuelle").html());
        PaginationPageActuelle++;
        $("#PaginationPageActuelle").html(PaginationPageActuelle);
        RechercheOffres($("#btnRechercher, input[name='tri']"), false);
    });
    $("#PaginationPremierePage").kclick(function () {
        $("#PaginationPageActuelle").html(1);
        RechercheOffres($("#btnRechercher, input[name='tri']"), false);
    });
    $("#PaginationDernierePage").kclick(function () {
        var PaginationTotal = $("#PaginationTotal").html();
        var PaginationStart = $("#PaginationStart").html();
        var PaginationEnd = $("#PaginationEnd").html();
        var paginationSizeDP = PaginationEnd - PaginationStart + 1;
        if (PaginationTotal < getPaginationSize()) {
            $("#PaginationPageActuelle").html(Math.ceil(PaginationTotal / paginationSizeDP));
        }
        else {
            $("#PaginationPageActuelle").html(Math.ceil(getPaginationSize() / paginationSizeDP));
        }
        RechercheOffres($("#btnRechercher, input[name='tri']"), false);
    });
    //----------------------Pagination de la recherche de cabinets de courtage---------------------
    $("#CabinetCourtagePaginationPrecedent").kclick(function () {
        var context = $(this).attr("albcontext");
        var PaginationPageActuelle = parseInt($("#CabinetCourtagePaginationPageActuelle").html());
        PaginationPageActuelle--;
        $("#CabinetCourtagePaginationPageActuelle").html(PaginationPageActuelle);
        RechercheCabinetsCourtiers(getSortedOrder(), getSortedId(), context);
    });
    $("#CabinetCourtagePaginationSuivant").kclick(function () {
        var context = $(this).attr("albcontext");
        var PaginationPageActuelle = parseInt($("#CabinetCourtagePaginationPageActuelle").html());
        PaginationPageActuelle++;
        $("#CabinetCourtagePaginationPageActuelle").html(PaginationPageActuelle);
        RechercheCabinetsCourtiers(getSortedOrder(), getSortedId(), context);
    });
    $("#CabinetCourtagePaginationPremierePage").kclick(function () {
        var context = $(this).attr("albcontext");
        $("#CabinetCourtagePaginationPageActuelle").html(1);
        RechercheCabinetsCourtiers(getSortedOrder(), getSortedId(), context);
    });
    $("#CabinetCourtagePaginationDernierePage").kclick(function () {
        var context = $(this).attr("albcontext");
        var PaginationTotal = $("#CabinetCourtagePaginationTotal").html();
        var PaginationStart = $("#CabinetCourtagePaginationStart").html();
        var PaginationEnd = $("#CabinetCourtagePaginationEnd").html();
        var paginationSizeCabinetCourtierDP = PaginationEnd - PaginationStart + 1;
        var paginationSize = $("#PaginationSizeCabinetCourtage").html();
        if (PaginationTotal < paginationSize) {
            $("#CabinetCourtagePaginationPageActuelle").html(Math.ceil(PaginationTotal / paginationSizeCabinetCourtierDP));
        }
        else {
            $("#CabinetCourtagePaginationPageActuelle").html(Math.ceil(paginationSize / paginationSizeCabinetCourtierDP));
        }
        RechercheCabinetsCourtiers(getSortedOrder(), getSortedId(), context);
    });
    //----------------------Pagination de la recherche des assurés---------------------
    $("#PreneurAssurancePaginationPrecedent").kclick(function () {
        var PaginationPageActuelle = parseInt($("#PreneurAssurancePaginationPageActuelle").html());
        PaginationPageActuelle--;
        $("#PreneurAssurancePaginationPageActuelle").html(PaginationPageActuelle);
        RecherchePreneursAssurance(getSortedOrder(), getSortedId());
    });
    $("#PreneurAssurancePaginationSuivant").kclick(function () {
        var PaginationPageActuelle = parseInt($("#PreneurAssurancePaginationPageActuelle").html());
        PaginationPageActuelle++;
        $("#PreneurAssurancePaginationPageActuelle").html(PaginationPageActuelle);
        RecherchePreneursAssurance(getSortedOrder(), getSortedId());
    });
    $("#PreneurAssurancePaginationPremierePage").kclick(function () {
        $("#PreneurAssurancePaginationPageActuelle").html(1);
        RecherchePreneursAssurance(getSortedOrder(), getSortedId());
    });
    $("#PreneurAssurancePaginationDernierePage").kclick(function () {
        var PaginationTotal = $("#PreneurAssurancePaginationTotal").html();
        var PaginationStart = $("#PreneurAssurancePaginationStart").html();
        var PaginationEnd = $("#PreneurAssurancePaginationEnd").html();
        var paginationSizePreneurAssuranceDP = PaginationEnd - PaginationStart + 1;
        var paginationSize = $("#PaginationSizePreneurAssurance").html();
        if (PaginationTotal < paginationSize) {
            $("#PreneurAssurancePaginationPageActuelle").html(Math.ceil(PaginationTotal / paginationSizePreneurAssuranceDP));
        }
        else {
            $("#PreneurAssurancePaginationPageActuelle").html(Math.ceil(paginationSize / paginationSizePreneurAssuranceDP));
        }
        RecherchePreneursAssurance(getSortedOrder(), getSortedId());
    });
}

//----------------------Récupération de l'ordre de tri dans une recherche avancée---------------------
function getSortedOrder() {
    var img = $(".imageTri").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1);
    img = img.substr(0, img.lastIndexOf('.'));
    return img.substring(4);
}

//----------------------Récupération de l'id de l'entête triée---------------------
function getSortedId() {
    return $(".imageTri").parent().parent().attr("id");
}

//----------------------Récupération de la taille total de la pagination---------------------
function getPaginationSize() {
    return $("#PaginationMaxSize").val();
}
//----------------------------Affiche un message s l'offre est vérouillé----------------------
function CheckLockedOffer() {
    const offreVerouille = $("#offreVerouille").val();
    const versionVerouille = $("#versionVerouille").val();
    const typeVerouille = $("#typeVerouille").val();
    const numAvnVerouille = $("#numAvnVerouille").val();
    const userVerouille = $("#userVerouille").val();

    if ([offreVerouille, versionVerouille, typeVerouille, numAvnVerouille, userVerouille].some(isEmpty))
        return;

    var receiver = window;
    if (window.parent != null && window.parent != window.top) {
        receiver = window.parent;
    }
    receiver.postMessage({
        type: "lockError",
        acteGestion: $("#ActeGestion").val(),
        connectedUser: $("#connectedUser").val(),
        userVerouille: userVerouille,
        offreVerouille: offreVerouille,
        typeVerouille: typeVerouille,
        numAvnVerouille: numAvnVerouille
    }, "*");

    function isEmpty(value) {
        return value == null || value == "" || value == "undefined ";
    }
}
function displayLockError(data) {
    var offre = data.typeVerouille == "O" ? "cette offre " : "ce contrat ";
    var message = "Impossible de modifier " + " <b>" + offre + "numéro </b>" + data.offreVerouille + " car " + (data.typeVerouille == "O" ? "elle" : "il") + " est actuellement en cours de modification par l'utilisateur <b>" + data.userVerouille + "</b>. Voulez-vous l'ouvrir en consultation ?";
    if (data.connectedUser == data.userVerouille) {
        message = "Vous avez déjà ouvert <b>" + offre + "numéro </b>" + data.offreVerouille;
        $("#btnOVReadOnly").disable();
        $("#btnOVReadOnly").hide();
        $(document).off("click", "#btnOVReadOnly");
    }
    else {
        $("#btnOVAnnuler").kclick(function () {
            try {
                common.$postJson("/OffresVerrouillees/UnlockFolder", {
                    folder: {
                        CodeOffre: data.offreVerouille,
                        Version: data.versionVerouille,
                        Type: data.typeVerouille,
                        NumeroAvenant: data.numAvnVerouille
                    },
                    tabGuid: infosTab.tabGuid
                });
            }
            catch (e) { }
        });
    }

    $("#centerOffreVerrouillee").html(message);
    $("#idOffreToAcces").val(data.offreVerouille + "_" + data.versionVerouille + "_" + data.typeVerouille + "_" + data.numAvnVerouille);

    if ($.trim(data.acteGestion.toLowerCase()) == "regul" || $.trim(data.acteGestion.toLowerCase()) == "avnrg") {
        $("#btnOVReadOnly").disable();
        $("#btnOVReadOnly").hide();
        $(document).off("click", "#btnOVReadOnly");
    }

    $("#MessageOffreVerrouillee").show();
    $("#divRechEngagementPeriode").hide();
}

//-------------Map des bouton de la div de message des offres vérrouillées--------------------
function MapButtonVerrouPopup() {
    $("#btnOVReadOnly").kclick(function () {
        var valRedirect = $("#offreVerouille").val() + "_" + $("#versionVerouille").val() + "_" + $("#typeVerouille").val() + $("#homeTabGuid", window.parent.document).val() + "ConsultOnly";
        if ($('#idOffreToAcces').val().split("_")[2] == "O") {
            valRedirect = "ModifierOffre/Index/" + $("#offreVerouille").val() + "_" + $("#versionVerouille").val() + "_" + $("#typeVerouille").val() + $("#homeTabGuid", window.parent.document).val() + "ConsultOnly";
        }
        else {
            if ($("#addParamVerouille").val() != "") {
                valRedirect = "AvenantInfoGenerales/Index/" + $("#offreVerouille").val() + "_" + $("#versionVerouille").val() + "_" + $("#typeVerouille").val() + "_" + $("#numAvnVerouille").val() + $("#homeTabGuid", window.parent.document).val() + $("#addParamVerouille").val() + "ConsultOnly";

            } else {
                valRedirect = "AnInformationsGenerales/Index/" + $("#offreVerouille").val() + "_" + $("#versionVerouille").val() + "_" + $("#typeVerouille").val() + $("#homeTabGuid", window.parent.document).val() + "ConsultOnly";
            }

        }
        CommonRedirect(valRedirect);
    });

    $("#btnOVAnnuler").click(function () {
        $("#MessageOffreVerrouillee").hide();
    });
}
//-----------------------Lecture Offre Contrat
function ReadOnlyFolder() {
    $("#btnOVAnnuler").click(function () {
        $("#MessageOffreVerrouillee").hide();
    });
}


//---------Nettoyage des variables de sessions----------
function CleanSession(tabGuid) {
    DeverouillerUserOffres(tabGuid);
}
//-------Formate les input/span des valeurs----------
function FormatNumericValue() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    //FormatNumerique('numeric', '', '99999999999', '0');
    common.autonumeric.formatAll('init', 'numeric', '', null, null, '99999999999', '0');
    common.autonumeric.formatAll('init', 'identifier', '', null, null, '99999999999', '0');
}
function FormatNumericCodePreneur() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    //FormatNumerique('numericCodePreneur', '', '9999999', '0');
    common.autonumeric.apply($("#CodePreneurAssuranceRecherche"), 'init', 'numeric', '', null, null, '9999999', '0');
}
function FormatNumericCodeCourtier() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    //FormatNumerique('numericCodeCourtier', '', '99999', '0');
    common.autonumeric.apply($("#CodeCabinetRecherche"), 'init', 'numeric', '', null, null, '99999', '0');
}
//----------------------Application de la recherche ajax des cibles par branche----------------------
function LoadListCibleByBranche() {

    var filter = JSON.parse(window.sessionStorage.getItem('recherche_filter'));
    var cible = filter != null ? filter.Cible : "";

    $("#Branche[albemplacement=recherche]").live('change', function () {
        AffectTitleList($(this));
        if ($("#CopyMode").val() != "True") {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/RechercheSaisie/GetCibles",
                data: { codeBranche: $(this).val(), codeCible: $('#Cible[albemplacement = recherche]').val() },
                success: function (data) {
                    $("#divCibles").html(data);
                    cible = $("#Cible").val();
                    toggleFilterEnabling();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
            CloseLoading();
        }
    });
}

function validateSelectionQuickSearch() {
    let root = $(window.parent.document);
    let doc = $(root.find("#MasterFrame").contents());
    let selection = $("[name=RadioRow]:checked");
    let searchCV = doc.find("#chkCanevas").isChecked();
    let a = selection.val().split('_');
    if (a.length > 1) {
        if (searchCV) {
            let codeTemplate = a[0].replace("CV", "");
            doc.find("#CodeCanevasCopy").val(codeTemplate);
            doc.find("#CodeOffreContratCopy").val(a[0]);
            doc.find("#VersionCopy").val(a[1]);
        }
        else {
            doc.find("#CodeOffreContratCopy").val(a[0]);
            doc.find("#VersionCopy").val(a[1]);
        }
        $("#btnInitializeRech").click();
        root.find("#divRechercheCopyOffre").hide();
    }
}

//--------Activation de l'autocomplete du canevas----------
function CheckCanevas() {
    $("#CodeOffreContratCopy").clear();
    $("#CodeCanevasCopy").clear();

    if ($("#chkCanevas").isChecked()) {
        $("#CodeOffreContratCopy").hide();
        $("#CodeOffreContratCopy").attr("albautocomplete", "autoCompCodeTemplate");
        $("#VersionCopy").hide();
        $("#labSeparator").hide();
        $("#CanevasCopy").show();
        $("#CodeCanevasCopy").show();
        $("#CodeCanevasCopy").focus();
    }
    else {
        $("#CodeOffreContratCopy").removeAttr("albautocomplete");
        $("#CodeOffreContratCopy").show();
        $("#VersionCopy").show();
        $("#labSeparator").show();
        $("#CanevasCopy").hide();
        $("#CodeCanevasCopy").hide();
    }
}

//-------Recherche automatique l'offre fournie en paramètre de l'écran
function LoadParamOffre() {
    if ($("#loadParamOffre").val() == "True") {
        $("#OffreId").val($("#offreVerouille").val());
        $("#NumAliment").val($("#versionVerouille").val());

        if ($("#typeVerouille").val() == "O") {
            $('#CheckOffre').check();
            $('#CheckContrat').uncheck();
        }
        if ($("#typeVerouille").val() == "P") {
            $('#CheckContrat').check();
            $('#CheckOffre').uncheck();
        }

        RechercheOffres($("#btnRechercher"), true);
    }
}

//----------------Actions du menu contextuel-----------------

function DoubleSaisie(item) {
    if (item && item.$selected) {
        item = "DoubleSaisie";
    }
    recherche.affaires.results.consultOrEdit(item);
}

function ModifHorsAvenant() {
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var motifRefus = selectRadio.attr('albmotifrefus');
    var infoRows = $(selectRadio).val();
    var offreNum = infoRows.split('_')[0];
    var offreVersion = infoRows.split('_')[1];
    var offreType = infoRows.split('_')[2];
    var numAvenant = infoRows.split('_')[3];
    var numAvnExt = selectRadio.attr('albnumavnext');
    var lastRegulId = selectRadio.attr('alblastregulid');
    var avnType = selectRadio.attr('albtypeavt');

    var addParam = '';

    if (numAvenant != '0') {
        addParam = "addParam" + $("#globalTypeAddParamAvn").val() + "|||" + $("#globalTypeAddParamAvn").val() + "ID|" + numAvenant + "||AVNTYPE|" + avnType + "||AVNIDEXTERNE|" + numAvnExt;

        if (lastRegulId !== "0") {
            addParam = common.addOrReplaceParam(addParam, "REGULEID", lastRegulId);
            addParam = common.addOrReplaceParam(addParam, "ACTEGESTIONREGULE", "AVNMD");

            /// OBO
            //addParam += "||REGULEID|" + lastRegulId + "||ACTEGESTIONREGULE|AVNMD";
        }


        addParam += "addParam";

    }


    var valRedirect = offreNum + "_" + offreVersion + "_" + offreType + $("#homeTabGuid", window.parent.document).val() + addParam + "modeNavig" + $("#ModeNavig").val() + "modeNavigConsultOnlyAndEdit";

    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/VerifierVerrouillage",
        data: { codeOffre: offreNum, version: offreVersion, type: offreType, numAvn: numAvenant },
        success: function (data) {
            if (data == "") {


                valRedirect = "AnCreationContrat/Index/" + valRedirect;

                if (valRedirect != "")
                    CommonRedirect(valRedirect);
            }
            else {
                $("#hiddenParamCheckLock").attr("albparam", valRedirect);
                if (offreType == "O") {
                    if (motifRefus != "")
                        valRedirect = "ConfirmationSaisie/Offre/" + valRedirect;
                    else
                        valRedirect = "ModifierOffre/Index/" + valRedirect;
                } else {
                    if (numAvenant == "0")
                        valRedirect = "AnInformationsGenerales/Index/" + valRedirect;
                    else {
                        valRedirect = "AvenantInfoGenerales/Index/" + valRedirect;
                    }
                }
                //SLA (22/11) : fin à supprimer
                $("#hiddenParamCheckLock").val(valRedirect);
                var message = '';
                var codeOffre = $('#selectedOffreContrat').val().split('_')[0];

                if (data.toLowerCase() === $('#connectedUser').val().toLowerCase()) {
                    message = "Vous avez déja ouvert ce contrat numéro <b>" + codeOffre + "</b>.";
                    ShowCommonFancy("Error", "", message, 200, 65, true, true);
                } else {
                    message = "Impossible de modifier ce contrat numéro <b>" + codeOffre + "</b> car il est actuellement en cours de modification par l'utilisateur <b>" + data + "</b>.<br/> Voulez-vous l'ouvrir en consultation ?";
                    ShowCommonFancy("ConfirmRechercheSaisie", "OpenHorsAvn", message, 300, 85, true, true);
                }


            }
        },
        error: function (request) {
            common.error.showXhr(request, true);
        }
    });
}

function Refuser() {
    AlbScrollTop();
    $("#divRefusOffre").show();
    $("#Refus").clear();
}
function CreerSaisie() {
    //AlbScrollTop();
    //AfficherDivCreation("Offre");
    $("#ModeNavig").val("S");
    CreateSaisie();
}
function CreerContrat() {
    AlbScrollTop();
    var infoRow = $("#selectedOffreContrat").val();
    var typeContratMere = "";
    var etat = "";
    if (infoRow != undefined && infoRow.split('_').length == 6) {
        typeContratMere = infoRow.split('_')[4];
        etat = infoRow.split('_')[5];
        if (typeContratMere == "M" && etat == "V") {
            var newWindow = "";
            if ($("#divDataRechercheRapide").attr("albQuickSearch") == "true") {
                newWindow = "newWindow";
            } else {
                ShowLoading();
            }

            var codeContratCopy = infoRow.split('_')[0];
            var versionCopy = infoRow.split('_')[1];
            var type = "P";
            $.ajax({
                type: "POST",
                url: "/RechercheSaisie/CreateCopyOffre",
                data: { codeOffreCopy: codeContratCopy, versionCopy: versionCopy, type: type, tabGuid: common.tabGuid, newWindow: newWindow },
                success: function (data) { },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            //AfficherDivCreation("Contrat");
            $("#ModeNavig").val("S");
            RedirectionRechercheSaisie("AnCreationContrat", "Index");
        }
    }
    else {
        //AfficherDivCreation("Contrat");
        $("#ModeNavig").val("S");
        RedirectionRechercheSaisie("AnCreationContrat", "Index");
    }
}
function EtablirAN() {
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    const tr = selectRadio.parent().parent();
    const offreEtat = $(tr).children(".tdEtat").html().trim();
    const offreSituation = $(tr).children(".tdSituation").html();

    if ((offreEtat != "V" && offreEtat != "R") || offreSituation != "A") {
        common.dialog.error("Impossible de créer une affaire nouvelle à partir d'une offre non validée.");
        return false;
    }

    ShowLoading();
    //RedirectionRechercheSaisie("CreationAffaireNouvelle", "Index");
    recherche.affaires.results.edit("EtablirAffaireNouvelle");
}
function CopyOffre() {
    AlbScrollTop();
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var infoRows = $(selectRadio).val();
    var offreType = infoRows.split("_")[2];
    var offreCode = infoRows.split("_")[0];
    var offreVersion = infoRows.split("_")[1];

    if (offreType == "O") {
        CreateCopyOffre(offreCode, offreVersion);
    }
    else if (offreType == "P") {
        CreateCopyContrat(offreCode, offreVersion);
    }
}
function BlocDebloc(niveauDroit) {
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var infoRows = $(selectRadio).val();
    var offreNum = infoRows.split("_")[0];
    var offreVersion = infoRows.split("_")[1];
    var offreType = infoRows.split("_")[2];
    var valRedirect = offreNum + "_" + offreVersion + "_" + offreType + "_" + niveauDroit;
    RedirectionRechercheSaisie("BlocageTermes", "Index", valRedirect);
}

function tryOpenRetoursPieces() {
    let selectedRadio = $('input[type=radio][name=RadioRow]:checked');
    let array = selectedRadio.val().split("_");
    let offre = { code: array[0], version: array[1], type: array[2], numAvenant: array[3], radio: selectedRadio };
    let typeTxt = offre.type == "O" ? "L'offre" : "L'affaire nouvelle";
    let tr = offre.radio.parent().parent();
    let branche = $(tr).children(".tdBranche").html();
    let typeAvt = offre.radio.attr("albtypeavt");
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/GetUserRole",
        data: { codeOffre: offre.code, version: offre.version, type: offre.type, branche: branche, typeAvt: typeAvt },
        success: function (data) {
            if (data == "") {
                OpenRetoursPieces();
            }
            else {
                let typeError = data.split("_")[1];
                if (typeError == "CITRIX") {
                    common.dialog.error("Veuillez ouvrir cette affaire sous Citrix.");
                }
                if (typeError == "RIGHTS") {
                    ShowCommonFancy("Error", "", "Vous n'avez pas les droits de modification sur la branche " + branche, 300, 80, true, true, true);
                }
                if (typeError == "REALISE") {
                    ShowCommonFancy("Error", "", typeTxt + " est réalisée, vous ne pouvez pas la modifier", 300, 80, true, true, true);
                }
                if (typeError == "REGUL") {
                    common.dialog.error("Vous n'avez pas les droits de modification sur la régularisation");
                }
            }
        },
        error: function (request) { }
    });
};

function OpenRetoursPieces() {
    const selectRadio = $('input[type=radio][name=RadioRow]:checked');
    const infoRows = $(selectRadio).val().split("_");
    const offreNum = infoRows[0];
    const offreVersion = infoRows[1];
    const offreType = infoRows[2];
    const codeAvenant = infoRows[3];
    const modeNavig = $("#modeHisto").isChecked() ? $("#modeHisto").val() : $("#modeStandard").val();
    let addParam = "";
    if (codeAvenant != "0") {
        $("#TypeAvt").val(selectRadio.attr("albtypeavt"));
        $("#ModeAvt").val("UPDATE");
    }

    const preId = offreNum + "_" + offreVersion + "_" + offreType + "_" + codeAvenant;
    const tabGuid = $("#homeTabGuid", window.parent.document).val();
    if (codeAvenant != "0" || modeNavig == "H") {
        addParam = "addParam" + $("#globalTypeAddParamAvn").val() + "|||" + $("#globalTypeAddParamAvn").val() + "ID|" + codeAvenant + "||AVNTYPE|" + $("#TypeAvt").val() + "addParam";
    }

    const id = preId + tabGuid + addParam;
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Retours/OpenRetours",
        data: { id: id, modeNavig: modeNavig },
        success: function (data) {
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
            $("#TypeAccordPreneur").off("change").change(function () {
                $("#inDateRetour").enable();
            });
            $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Fin Actions du menu contextuel---------------

//---------Fermeture de la div résultat de recherche (Acces Rapide)
function CloseQuickResultResearch() {
    $("#btnRetourRechRapide").kclick(function () {
        $("#divRechercheRapide").hide();
    });
}


//-------Création d'un avenant de modification-------
function CreateAvntModif() {
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var tr = selectRadio.parent().parent();
    var offreEtat = $(tr).children(".tdEtat").html();
    var offreSituation = $(tr).children(".tdSituation").html();
    var dateFinEffet = selectRadio.attr("albdatefineffet");

    if (offreEtat == "V" && offreSituation == "X") {
        $("#TypeAvt").val("AVNMD");
        $("#ModeAvt").val(offreEtat == "V" ? "CREATE" : "UPDATE");
        $("#ignoreReadonly").val(false);
        ShowCommonFancy("ConfirmRechercheSaisie", "AVNMD", "Vous allez <u>modifier</u> un contrat résilié<br/>au " + dateFinEffet + ",<br/>souhaitez-vous confirmer votre action ?", 500, 80, true, true, true);
    }
    else {
        /*$("#TypeAvt").val("AVNMD");
        $("#ModeAvt").val(offreEtat == "V" ? "CREATE" : "UPDATE");
        $("#ignoreReadonly").val(false);
        RedirectionRechercheSaisie("CreationAvenant", "Index");*/
        recherche.affaires.results.consultOrEdit(null, "AVNMD");
    }
}

//-------Création d'un avenant de résiliation--------
function CreateAvntResil() {
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var tr = selectRadio.parent().parent();
    var offreEtat = $(tr).children(".tdEtat").html();
    var offreSituation = $(tr).children(".tdSituation").html();

    if ($.trim(offreSituation) != "X" && $.trim(offreSituation) != "W" && $.trim(offreSituation) != "N") {
        //$("#TypeAvt").val("AVNRS");
        //$("#ModeAvt").val(offreEtat == "V" ? "CREATE" : "UPDATE");
        //$("#ignoreReadonly").val(false);
        //RedirectionRechercheSaisie("CreationAvenant", "Index");
        recherche.affaires.results.consultOrEdit(null, "AVNRS");
    }
    else {
        common.dialog.error("Anomalie historique");
    }
}
//------Création d'un avenant de résiliation-----------
function CreateAvntResilBis(modeResil) {
    if (modeResil == "1") {
        ShowCommonFancy("Confirm", "ResilEch", "Souhaitez-vous consulter également la situation de l'affaire avant résiliation ?", 300, 80, true, true, true);
        return;
    }

    OpenAvenantResil("0");
}

function OpenAvenantResil(modeResil, newwindow) {
    var selectRadio = $("input[type=radio][name=RadioRow]:checked");
    var tr = selectRadio.parent().parent();
    var offreEtat = $(tr).children(".tdEtat").html();
    var offreSituation = $(tr).children(".tdSituation").html();

    if ($.trim(offreSituation) != "X" && $.trim(offreSituation) != "W" && $.trim(offreSituation) != "N") {
        $("#TypeAvt").val("AVNRS");
        $("#ModeAvt").val(offreEtat == "V" ? "CREATE" : "UPDATE");
        RedirectionRechercheSaisie("CreationAvenant", "Index", "", "", modeResil, newwindow);
    }
    else {
        common.dialog.error("Anomalie historique");
    }
}

function AvtResilRedirect(openConsult, readonlyConsult) {
    //#region Récupération des données
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var infoRows = $(selectRadio).val();
    var offreNum = infoRows.split("_")[0];
    var offreVersion = infoRows.split("_")[1];
    var offreType = infoRows.split("_")[2];
    var numAvenant = infoRows.split("_")[3];
    var addParam = "";
    if (numAvenant != "0" && numAvenant != "1") {
        numAvenant = numAvenant - 1;
        addParam = "addParam" + $("#globalTypeAddParamAvn").val() + "|||" + $("#globalTypeAddParamAvn").val() + "ID|" + numAvenant + "||AVNTYPE|" + selectRadio.attr("albtypeavt") + "addParam";
    }

    var valRedirect = offreNum + "_" + offreVersion + "_" + offreType + $("#homeTabGuid", window.parent.document).val() + addParam + "modeNavig" + $("#ModeNavig").val() + "modeNavigConsultOnly";

    var addParam2 = "";
    var valRedirect2 = "";

    if (infoRows.split("_")[3] != "0")
        addParam2 = "addParam" + $("#globalTypeAddParamAvn").val() + "|||" + $("#globalTypeAddParamAvn").val() + "ID|" + infoRows.split("_")[3] + "||AVNTYPE|" + selectRadio.attr("albtypeavt") + "addParam";
    valRedirect2 = offreNum + "_" + offreVersion + "_" + offreType + $("#homeTabGuid", window.parent.document).val() + addParam2 + "modeNavig" + $("#ModeNavig").val() + "modeNavigConsultOnly";



    //#endregion

    if (openConsult) {
        if (!readonlyConsult)
            RedirectionRechercheSaisie("CreationAvenant", "Index", "", "", "", "newwindow");
        else {
            CommonRedirect("EngagementPeriodes/Index/" + valRedirect2 + "newWindow");
        }
        setTimeout(function () {
            if (addParam != "") {
                valRedirect = "AvenantInfoGenerales/Index/" + valRedirect;
            }
            else {
                valRedirect = "AnInformationsGenerales/Index/" + valRedirect;
            }
            CommonRedirect(valRedirect);
        }, 750);
    }
    else {
        if (!readonlyConsult)
            RedirectionRechercheSaisie("CreationAvenant", "Index");
        else
            CommonRedirect("EngagementPeriodes/Index/" + valRedirect2);
    }
}

//-------Création d'un avenant de remise en vigueur-------
function CreateAvntRemiseEnVigueur() {
    recherche.affaires.results.consultOrEdit(null, "AVNRM");

}

//---------Reprise offre validée-------
function RepriseOffre() {
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var infoRows = $(selectRadio).val();
    var offreNum = infoRows.split("_")[0];
    var offreVersion = infoRows.split("_")[1];
    var offreType = infoRows.split("_")[2];

    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/DernierNumeroVersionOffre",
        data: { codeOffre: offreNum, type: offreType, version: offreVersion },
        success: function (data) {
            if (data != "") {
                derniereVersion = data.split('_')[0];
                NouvelleVersionProgression();
            }
            else {
                common.dialog.error("Erreur lors de la création de version.");
            }
        },
        error: function (request) {
            CloseCommonFancy();
            common.error.showXhr(request, true);
        }
    });

}
//-------Reprise contrat valide----------
function RepriseContrat() {
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    //var tr = selectRadio.parent().parent();
    var typeAvt = selectRadio.attr("albtypeavt") || "";
    var infoRow = selectRadio.val();
    var codeAffaire = infoRow.split('_')[0];
    var version = infoRow.split('_')[1];
    var type = infoRow.split('_')[2];
    var codeAvn = infoRow.split('_')[3];

    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/CheckPrimeSoldee",
        data: { codeAffaire: codeAffaire, version: version, type: type, codeAvn: codeAvn },
        success: function (data) {
            if (data === "1")
                common.dialog.error("L'affaire est porteuse de prime(s) soldée(s), vous ne pouvez pas faire de reprise.");
            else {
                var text = "";
                switch (typeAvt.toUpperCase()) {
                    case "AFFNV":
                        text = "Attention, vous allez modifier une affaire nouvelle validée.<br/>Toutes les quittances émises vont être annulées.";
                        ShowCommonFancy("ConfirmRechercheSaisie", "Reprise", text, 300, 80, true, true, true);
                        break;
                    case "AVNMD":
                    case "AVNRG":
                    case "AVNRS":
                    case "REGUL":
                    case "AVNRM":
                        RepriseAvenant();
                        break;
                    default:
                        break;
                }
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Reprise avenant ANVMD validé--------
function RepriseAvenant() {
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var infoRow = selectRadio.val();
    var text = "";
    var codeContrat = infoRow.split('_')[0];
    var version = infoRow.split('_')[1];
    var type = infoRow.split('_')[2];
    var codeAvn = infoRow.split('_')[3];
    var typeAvt = selectRadio.attr("albtypeavt");

    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/CheckPrimeAvt",
        data: { codeContrat: codeContrat, version: version, type: type, codeAvn: codeAvn },
        success: function (data) {
            if (data == "") {
                switch (typeAvt) {
                    case "AVNRG":
                        text = "Attention, vous allez modifier un acte de gestion validé.<br/>Toutes les quittances émises <b>avenant + régularisation</b> sur cet acte vont être annulées.";
                        break;
                    case "REGUL":
                        text = "Attention, vous allez modifier un acte de gestion validé.<br/>Toutes les quittances émises <b>régularisation</b> sur cet acte vont être annulées.";
                        break;
                    default:
                        text = "Attention, vous allez modifier un acte de gestion validé.<br/>Toutes les quittances émises sur cet acte vont être annulées.";
                        break;
                }
                ShowCommonFancy("ConfirmRechercheSaisie", "Reprise", text, 300, 80, true, true, true);
            }
            else {
                common.dialog.error(data);
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Confirmation de la reprise------
function ConfirmReprise() {
    let selectRadio = $('input[type=radio][name=RadioRow]:checked');
    let infoRows = $(selectRadio).val();
    let offreNum = infoRows.split("_")[0];
    let offreVersion = infoRows.split("_")[1];
    let offreType = infoRows.split("_")[2];
    let numAvenant = infoRows.split("_")[3];
    let typeAvt = selectRadio.attr("albtypeavt");

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/ConfirmReprise",
        data: { codeOffre: offreNum, version: offreVersion, type: offreType, codeAvt: numAvenant, typeAvt: typeAvt },
        success: function () {
            CloseLoading();
            if (numAvenant == "0" || selectRadio.attr("albtypeavt") == "AVNMD")
                //RedirectionRechercheSaisie("AnInformationsGenerales", "Index");
                recherche.affaires.results.consultOrEdit("btnCreerOffre");
            else {

                switch (selectRadio.attr("albtypeavt")) {
                    //case "AVNMD":
                    //    $("#ModeAvt").val("UPDATE");
                    //    RedirectionRechercheSaisie("AnnulationQuittances", "Index");
                    //    recherche.affaires.results.consultOrEdit("btnCreerOffre");
                    //    break;
                    case "AVNRG":
                    case "REGUL":
                        $("#ModeAvt").val("UPDATE");
                        RedirectionRechercheSaisie("CreationRegularisation", "Index");
                        break;
                    default:
                        $("#ModeAvt").val("UPDATE");
                        RedirectionRechercheSaisie("AnnulationQuittances", "Index");
                        break;
                }
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------Ouverture de la liste des historiques-------
function OpenHistorique() {
    RedirectionRechercheSaisie("Historique", "Index");
}

//--------Validation des retours--------
function ValidRetours() {
    var erreurBool = false;

    var dateRetourPreneur = $("#inDateRetour").val();
    var typeAccordPreneur = $("#TypeAccordPreneur").val();
    var typeAccordPreneuActuel = $("#TypeAccordPreneurActuel").val();
    var listeLignesCoAss = SerialiserLignesCoAssureur();
    var isReglementRecu = $("#chkReglementRecu").attr("checked") == "checked";

    ResetErreurs();

    if (dateRetourPreneur != undefined && dateRetourPreneur != "" && dateRetourPreneur.split("/").length == 3) {
        var dNow = new Date();
        var iNow = dNow.getFullYear() * 10000 + (dNow.getMonth() + 1) * 100 + dNow.getDate();
        var iDateRetourPreneur = dateRetourPreneur.split("/")[2] * 10000 + dateRetourPreneur.split("/")[1] * 100 + dateRetourPreneur.split("/")[0] * 1;
        if (iDateRetourPreneur > iNow) {
            $("#inDateRetour").addClass('requiredField');
            $("#inDateRetour").attr('title', "La date ne peut être postérieure à la date du jour");
            erreurBool = true;
        }
    }

    if (dateRetourPreneur != undefined && dateRetourPreneur != "" && dateRetourPreneur.split("/").length == 3) {
        if (typeAccordPreneur == "N") {
            $("#TypeAccordPreneur").addClass('requiredField');
            $("#TypeAccordPreneur").attr('title', "Une date de retour a été saisie, vous devez choisir un accord");
            erreurBool = true;
        }
    }

    if (isReglementRecu) {
        if (typeAccordPreneur == "N" || typeAccordPreneur == "T") {
            $("#TypeAccordPreneur").addClass('requiredField');
            $("#TypeAccordPreneur").attr('title', "Un règlement a été reçu, le type d'accord sélectionné n'est pas compatible");
            erreurBool = true;
        }
    }
    else {
        if (typeAccordPreneur == "R") {
            $("#TypeAccordPreneur").addClass('requiredField');
            $("#TypeAccordPreneur").attr('title', "Aucun règlement n'a été reçu, le type d'accord sélectionné n'est pas compatible");
            erreurBool = true;
        }
    }

    if (typeAccordPreneuActuel == "R" && typeAccordPreneur != "S") {
        $("#TypeAccordPreneur").addClass('requiredField');
        $("#TypeAccordPreneur").attr('title', "Le type d'accord sélectionné n'est pas compatible, avec la situation actuelle");
        erreurBool = true;
    }

    if ((typeAccordPreneur != "N") && (dateRetourPreneur == undefined || dateRetourPreneur == "" || dateRetourPreneur.split("/").length != 3)) {
        $("#inDateRetour").addClass('requiredField');
        $("#inDateRetour").attr('title', "Une date d'accord valide doit être sélectionnée");
        erreurBool = true;
    }


    if (CheckLignesCoAss()) {
        erreurBool = true;
    }

    if (erreurBool) {
        return false;
    }
    else {
        ShowLoading();
        //var codeContrat = $("#Offre_CodeOffre").val();
        //var versionContrat = $("#Offre_Version").val();
        //var typeContrat = $("#Offre_Type").val();
        var codeContrat = $("#inRetourCodeAffaire").val();
        var versionContrat = $("#inRetourVersion").val();
        var typeContrat = $("#inRetourType").val();
        var tabGuid = $("#homeTabGuid", window.parent.document).val();
        $.ajax({
            type: "POST",
            url: "/Retours/ValiderRetours/",
            data: {
                codeContrat: codeContrat, version: versionContrat, type: typeContrat, codeAvt: $("#inRetourAvenant").val(), tabGuid: tabGuid,
                dateRetourPreneur: dateRetourPreneur, typeAccordPreneur: typeAccordPreneur,
                listeLignesCoAss: listeLignesCoAss, modeNavig: $("#ModeNavig").val()
            },
            success: function (data) {
                $("#dvDataFlotted").html("");
                $("#dvFlotted").hide();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//-----------Retire toutes les erreurs de l'écran et leurs messages
//---------Fermeture des retours--------
function FermerRetours() {
    ShowLoading();
    //var codeContrat = $("#Offre_CodeOffre").val();
    //var versionContrat = $("#Offre_Version").val();
    //var typeContrat = $("#Offre_Type").val();
    var codeContrat = $("#inRetourCodeAffaire").val();
    var versionContrat = $("#inRetourVersion").val();
    var typeContrat = $("#inRetourType").val();
    var tabGuid = $("#homeTabGuid", window.parent.document).val();
    $.ajax({
        type: "POST",
        url: "/Retours/FermerRetours/",
        data: {
            codeContrat: codeContrat, version: versionContrat, type: typeContrat, codeAvt: $("#inRetourAvenant").val(), tabGuid: tabGuid,
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            $("#dvDataFlotted").html("");
            $("#dvFlotted").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

function ResetErreurs() {
    $("select[name=typeAccordInput]").removeClass('requiredField').attr("title", "");
    $("input[name=dateRetour]").removeClass('requiredField').attr("title", "");
}
//-----------fonction qui sérialise une opposition en fonction des champs de l'écran--------
function SerialiserLignesCoAssureur() {
    var chaine = '[';
    var hasLine = false;
    $("tr[name=lineCoass]").each(function () {
        var id = $(this).attr("id").split("_")[1];
        if (id != "" && id != undefined) {
            hasLine = true;
            chaine += '{';
            chaine += 'GuidId : "' + id + '",';
            chaine += 'SDateRetourCoAss : "' + $("input[id='inDateRetourCoAss_" + id + "']").val() + '",';
            chaine += 'TypeAccordVal : "' + $("select[id='TypeAccordCoAss_" + id + "']").val() + '"';
            chaine += '},';
        }

    });
    if (hasLine)
        chaine = chaine.substring(0, chaine.length - 1);
    chaine += ']';
    chaine = chaine.replace("\\", "\\\\");
    return chaine;
}
//----------Vérifie les valeurs des lignes des coassureurs------
function CheckLignesCoAss() {
    var dNow = new Date();
    var iNow = dNow.getFullYear() * 10000 + (dNow.getMonth() + 1) * 100 + dNow.getDate();
    var toReturn = false;
    $("tr[name=lineCoass]").each(function () {
        var id = $(this).attr("id").split("_")[1];
        if (id != "" && id != undefined) {
            var dateRetourCoAss = $("input[id='inDateRetourCoAss_" + id + "']").val();
            var typeAccordCoAss = $("select[id='TypeAccordCoAss_" + id + "']").val();
            //var dateRetourCoAss = $("#inDateRetourCoAss_" + id).val();
            //var typeAccordCoAss = $("#TypeAccordCoAss_" + id).val();
            if (dateRetourCoAss != undefined && dateRetourCoAss != "" && dateRetourCoAss.split("/").length == 3) {
                var idateRetourCoAss = dateRetourCoAss.split("/")[2] * 10000 + dateRetourCoAss.split("/")[1] * 100 + dateRetourCoAss.split("/")[0] * 1;
                if (idateRetourCoAss > iNow) {
                    $("input[id='inDateRetourCoAss_" + id + "']").addClass('requiredField');
                    $("input[id='inDateRetourCoAss_" + id + "']").attr('title', "La date ne peut être postérieure à la date du jour");
                    //$("#inDateRetourCoAss_" + id).addClass('requiredField');
                    //$("#inDateRetourCoAss_" + id).attr('title', "La date ne peut être postérieure à la date du jour");
                    toReturn = true;
                }
                if (typeAccordCoAss == "N") {
                    $("select[id='TypeAccordCoAss_" + id + "']").addClass('requiredField');
                    $("select[id='TypeAccordCoAss_" + id + "']").attr('title', "Une date de retour a été saisie, vous devez choisir un accord");
                    //$("#TypeAccordCoAss_" + id).addClass('requiredField');
                    //$("#TypeAccordCoAss_" + id).attr('title', "Une date de retour a été saisie, vous devez choisir un accord");
                    toReturn = true;
                }
            }
            if (typeAccordCoAss != "N") {
                if (dateRetourCoAss == undefined || dateRetourCoAss == "" || dateRetourCoAss.split("/").length != 3) {
                    $("input[id='inDateRetourCoAss_" + id + "']").addClass('requiredField');
                    $("input[id='inDateRetourCoAss_" + id + "']").attr('title', "Une date d'accord valide doit être sélectionnée");
                    //$("#inDateRetourCoAss_" + id).addClass('requiredField');
                    //$("#inDateRetourCoAss_" + id).attr('title', "Une date d'accord valide doit être sélectionnée");
                    toReturn = true;
                }
            }

        }
    });

    return toReturn;
}

function OpenPrendrePosition() {
    let selectRadio = $('input[type=radio][name=RadioRow]:checked');
    let infoRows = $(selectRadio).val();
    let offreType = infoRows.split("_")[2];

    if (offreType == "P") {
        common.error.showXhr("Fonctionnalité indisponible pour les contrats.");
        return;
    }

    RedirectionRechercheSaisie("PrisePosition", "Index");
}

//#region Périodes d'engagements

//------Ouvre les périodes d'engagements---------
function OpenEngagementPeriode() {
    const params = GetParams();
    let id = paramDblSaisie + params.tabGuid + params.addParam;

    recherche.affaires.results.edit("EngagementPeriodes", function () {
        $("#EngagementIFrame").attr("src", "/EngagementPeriodes/Index/" + id + "accessModerechercheaccessMode");
        $("#divRechEngagementPeriode").show();
    });
}
//#endregion

function GetSelectedValue(type) {
    var selectedRadio = $('input[type=radio][name=RadioRow]:checked');
    return selectedRadio.closest("tr").find(".td" + type).text().trim();
}

function OpenRegulPage() {
    $("#TypeAvt").val("REGUL");
    $("#ModeAvt").val("CREATE");
    $("#ignoreReadonly").val(true);
    RedirectionRechercheSaisie("CreationRegularisation", "Index");
}

function OpenPBPage() {
    $("#TypeAvt").val("REGUL");
    $("#ModeAvt").val("CREATE");
    $("#ignoreReadonly").val(true);
    RedirectionRechercheSaisie("CreationPB", "Index");
}

function OpenBNSPage() {
    $("#TypeAvt").val("REGUL");
    $("#ModeAvt").val("CREATE");
    $("#ignoreReadonly").val(true);
    recherche.affaires.results.consultOrEditBNS();
}

function GetParams() {
    const selectRadio = $('input[type=radio][name=RadioRow]:checked');
    const motifRefus = selectRadio.attr("albmotifrefus");
    const infoRows = $(selectRadio).val().split("_");
    const offreNum = infoRows[0];
    const offreVersion = infoRows[1];
    const offreType = infoRows[2];
    const numAvenant = infoRows[3];
    const numAvnExt = selectRadio.attr("albnumavnext");
    const modeNavig = $("#ModeNavig").val();
    const tabGuid = ($("#tabGuid").val() || "").length > 14 ? $("#tabGuid").val() : $("#homeTabGuid", window.top.document).val();
    let addParam = "";
    if (numAvenant != "0") {
        addParam = "addParam" + $("#globalTypeAddParamAvn").val() + "|||" + $("#globalTypeAddParamAvn").val() + "ID|" + numAvenant + "||AVNTYPE|" + selectRadio.attr("albtypeavt") + "||AVNIDEXTERNE|" + numAvnExt + "addParam";
    }
    return {
        offreNum: offreNum,
        offreVersion: offreVersion,
        offreType: offreType,
        numAvenant: numAvenant,
        numAvnExt: numAvnExt,
        addParam: addParam,
        motifRefus: motifRefus,
        tabGuid: tabGuid,
        modeNavig: modeNavig
    };

}
