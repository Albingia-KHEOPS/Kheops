
$(document).ready(function () {
    MapAdresse();
    CabinetCodeFocus();
    formatDatePicker();
    MapCommonAutoCompCourtier();
    MapCommonAutoCompAssure();
    MapCommonAutoCompSouscripteur();
    MapCommonAutoCompGestionnaire();
    LoadListCibleByBranche();
    LoadListTemplatesByCible();
    LoadTemplate();
    Suivant();
    MapElementPage();
    AffectTriggers();
    ChangeClass();
    ChangeHour();

    loadFilter();
  
    if ($("#CreationSaisieAction").val().split("_")[0] != "" && $("#CopyMode").val() != "True" && $("#OffreReadOnly").val() != "True") {
        LoadListCibleInLoad();
    }

});
//--------Déplace le div de l'adresse----------
function MapAdresse() {
    $("#divAdresse").html($("#divHideAdresse").html());
    LinkHexavia();
}

//afficher l'historique de recherche
function loadFilter() {

    if (!window.sessionStorage) return;
    var filter = JSON.parse(window.sessionStorage.getItem('recherche_filter'));
    if (!filter) return;
    if ($("#CreationSaisieAction").val().split("_")[0] == "") {
        // Cible
        $('#InformationSaisie_InformationCible_Cible').val(filter.Cible);

        //Preneur d'assurance  
        $('#PreneurAssurance_CodePreneurAssurance').val(filter.PreneurAssuranceCode);
        $('#PreneurAssurance_NomPreneurAssurance').val(filter.PreneurAssuranceNom);
        $('#PreneurAssurance_Departement').val(filter.PreneurAssuranceCP);
        $('#PreneurAssurance_Ville').val(filter.preneurAssuranceVille);

        if (filter.PreneurAssuranceCode && !(filter.PreneurAssuranceNom || "").trim());
        {
            $('#PreneurAssurance_CodePreneurAssurance').trigger("change");
        }

        if (filter.PreneurAssuranceCode != "" && typeof showAlertMessageAssure == "function") {
            showAlertMessageAssure({ Sinistre: filter.PreneurSinistres, Impayes: filter.PreneurImpayes, RetardsPaiements: filter.PreneurRetardsPaiement, Code: filter.PreneurAssuranceCode }, true);
        }

        //Courtier Gestionnaire 
        $('#CabinetCourtage_CodeCabinetCourtage').val(filter.CabinetCourtageId);
        $('#PreneurAssurance.CabinetCourtage_NomCabinetCourtage').val(filter.CabinetCourtageNom);

        if (filter.CabinetCourtageId && !(filter.CabinetCourtageNom || "").trim());
        {
            $('#CabinetCourtage_CodeCabinetCourtage').trigger("change");
        }

        //Gestionnaire
        $('#InformationSaisie_Gestionnaires').val(filter.GestionnaireNom);

    }
    if ($('#InformationSaisie_InformationCible_Cible').val() != "" && $("#CopyMode").val() != "True" && $("#OffreReadOnly").val() != "True") {
        $('#InformationSaisie_InformationCible_Cible').trigger("change");
    }

}



//---------------Map les éléments de la page----------------------
function MapElementPage() {
    if (!window.isReadonly) {
        //Mettre une div pour simuler un textarea, pour prendre en compte le format HTML
        $("#Description_Observation").html($("#Description_Observation").html().replace(/&lt;br&gt;/ig, "\n"));
        //FormatCLEditor($("#Description_Observation"), 375, 80, 32);
    }
    else {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        $("#RechercheAvanceePreneurAssurance").removeClass("CursorPointer");
        $("#RechercheAvanceePreneurAssurance").attr("src", "/Content/Images/loupegris.png");
    }
    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
            "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
            350, 130, true, true);
    });
    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                CancelForm();
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    MapMotsCles();

    toggleDescription($("#Description_Observation"));

    if ($("#InformationSaisie_Branche").val() == "" || ($("#InFformationSaisie_InformationCible_Cible").val() == "")) {
        LockUnlockElements(true);
    }

    MapBtnAssureAdd();

    AffectTitleElements();
    MapBoitesDialogue();

    $("#RechercheAvanceePreneurAssurance").die().live("click", function () {
        if ($(this).hasClass('CursorPointer')) {
            var context = $(this).attr("albcontext");
            if (context == undefined)
                context = "";
            OpenRechercheAvanceePreneurAssurance($("#PreneurAssurance_CodePreneurAssurance").val(), $("#PreneurAssurance_NomPreneurAssurance").val(), context);
        }
    });

    $("img[name=RechercherCourtier]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            if ($(this).hasClass("CursorPointer")) {
                var context = $(this).attr("albcontext");
                $("#divDataRechercheAvanceeCourtierCreationCourtier").html($("div[name=RechercherCourtier][albcontext=" + context + "]").html());
                AlbScrollTop();
                $("#divRechercheAvanceeCourtierCreationContrat").show();

                RechercheAvanceeCourtier(context);
                AlternanceLigne("CabinetsCourtageBody", "Code", true, null);
                CloseRechAvancee();

                FormatNumericCodeCourtier();

            }
        });
    });
}

function MapBtnAssureAdd() {
    var acteGestion = $("#ActeGestion").val();

    if (acteGestion != undefined && acteGestion != "" && $("#CopyMode").val() == "False" && $("#PreneurAssurance_CodePreneurAssurance").val() != "") {
        $("#btnOpenAssu").die().live('click', function () { OpenAssuAdd(); });
    }
    else {
        $("#btnOpenAssu").hide();
    }
}

function MapMotsCles() {
    $("#MotClef1").die().live('change', function () {
        AffectTitleList($(this));
    });
    $("#MotClef2").die().live('change', function () {
        AffectTitleList($(this));
    });
    $("#MotClef3").die().live('change', function () {
        AffectTitleList($(this));
    });
}

function AffectTitleElements() {
    AffectTitleList($("#InformationSaisie_Branche"));
    AffectTitleList($("#InformationSaisie_BrancheText"));
    AffectTitleList($("#InformationSaisie_InformationCible_Cible"));
    AffectTitleList($("#MotClef1"));
    AffectTitleList($("#MotClef2"));
    AffectTitleList($("#MotClef3"));
}
function EditCible() {
    $("#divConfirmRsq").show();
    $("#divConfirmRsq").draggable();

}

//----------------Redirection------------------
function Redirection(cible, job, param) {
    ShowLoading();
    var paramRedirect = $("#txtParamRedirect").val();
    $.ajax({
        type: "POST",
        url: "/CreationSaisie/Redirection/",
        data: { cible: cible, job: job, param: param, paramRedirect: paramRedirect },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------------------Affecte les fonctions sur les controles heures-----------------------
function ChangeHour() {
    $("#HeureSaisieStringHours").live('change', function () {
        if ($(this).val() != "" && $("#HeureSaisieStringMinutes").val() == "")
            $("#HeureSaisieStringMinutes").val("00");
        AffecteHour($(this));
    });
    $("#HeureSaisieStringMinutes").live('change', function () {
        if ($(this).val() != "" && $("#HeureSaisieStringHours").val() == "")
            $("#HeureSaisieStringHours").val("00");
        AffecteHour($(this));
    });
}
//-------------------Renseigne l'heure---------------------------
function AffecteHour(elem) {
    var elemId = elem.attr('id').replace("Hours", "").replace("Minutes", "")

    var changeHour = SetHours(elemId);
    if (!changeHour && elem.val() == "") {
        $("#" + elemId + "Hours").val("");
        $("#" + elemId + "Minutes").val("");
    }
}
//----------------------Met le focus sur le premier controle au démarrage---------------------
function CabinetCodeFocus() {
    if (!$("#InformationSaisie_Branche").attr('disabled')) {
        $("#InformationSaisie_Branche").focus();
    }
    else {
        $("#InformationSaisie_Souscripteurs").focus();
    }
}
//----------------------Formate tous les controles datepicker---------------------
function formatDatePicker() {
    $("#InformationSaisie_DateSaisieString").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#InformationSaisie_DateSaisieString").change(function () {
        if ($(this).val() != "") {
            if ($("#HeureSaisieStringHours").val() == "") {
                $("#HeureSaisieStringHours").val("00");
                $("#HeureSaisieStringMinutes").val("00");
                $("#HeureSaisieStringHours").trigger("change");
            }
        }
        else {
            $("#HeureSaisieStringHours").val("");
            $("#HeureSaisieStringMinutes").val("");
            $("#HeureSaisieStringHours").trigger("change");
        }
    });
}
//----------------------Attache la fonction de suppr de la classe requiredField----------------------
function ChangeClass() {
    $("#ContactAdresse_CodePostal").live("change", function () {
        DeleteClass($(this), "requiredField");
    });
}
//----------------------Application de la recherche ajax des cibles par branche----------------------
function LoadListCibleByBranche() {
    $("#InformationSaisie_Branche").die().live('change', function () {
        let codeCourtier = $("#CabinetCourtage_CodeCabinetCourtage").val();
        let codeAssure = $("#PreneurAssurance_CodePreneurAssurance").val();
        AffectTitleList($(this));
        ShowLoading();
        var codeBranche = $(this).val();
        $.ajax({
            type: "POST",
            url: "/CreationSaisie/GetCibles",
            data: { codeBranche: codeBranche },
            success: function (data) {
                $("#cibleDiv").html(data);
                if ($("#hideCible").val() != "") {
                    $("#InformationSaisie_InformationCible_Cible option[value='" + $("#hideCible").val() + "']").attr('selected', 'selected');
                    $("#hideCible").val("");
                }
                ClearElements();
                $("#InformationSaisie_InformationTemplate_Template").html('');
                var codeCible = $('#InformationSaisie_InformationCible_Cible').val();
                LoadListesMotsCles(codeBranche, codeCible);
                if (codeBranche == "" || codeBranche == undefined) {
                    LockUnlockElements(true);
                }
                $("#CabinetCourtage_CodeCabinetCourtage").val(codeCourtier).trigger("change");
                $("#PreneurAssurance_CodePreneurAssurance").val(codeAssure).trigger("change");
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
        CloseLoading();
        //}
    });
}
//----------------------Application de la recherche ajax des cibles par branche----------------------
function LoadListCibleInLoad() {
    var codeBranche = document.getElementById("InformationSaisie_BrancheText").value.split("-")[0];
        $.ajax({
            type: "POST",
            url: "/CreationSaisie/GetCibles",
            data: { codeBranche: codeBranche },
            success: function (data) {
                $("#cibleDiv").html(data);
                if ($("#hideCible").val() != "") {
                    $("#InformationSaisie_InformationCible_Cible option[value='" + $("#hideCible").val() + "']").attr('selected', 'selected');
                    $("#hideCible").val("");
                }
                var codeCible = $('#InformationSaisie_InformationCible_Cible').val();
                LoadListesMotsCles(codeBranche, codeCible);
                if (codeBranche == "" || codeBranche == undefined) {
                    LockUnlockElements(true);
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
        CloseLoading();    
}
//----------------------Application de la recherche ajax des templates par cible----------------------
function LoadListTemplatesByCible() {
    $("#InformationSaisie_InformationCible_Cible").die().live('change', function () {
        let codeCourtier = $("#CabinetCourtage_CodeCabinetCourtage").val();
        let codeAssure = $("#PreneurAssurance_CodePreneurAssurance").val();
        var codeCible = $(this).val();
        if ($("#CreationSaisieAction").val().split("_")[0] == "") {
            AffectTitleList($(this));
            LockUnlockElements(false);
            ShowLoading();
            if (codeCible == "" || codeCible == undefined) {
                LoadListesMotsCles($("#InformationSaisie_Branche").val(), codeCible);
                ClearElements();
                $("#InformationSaisie_InformationTemplate_Template").html('');
                LockUnlockElements(true);
                CloseLoading();
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/CreationSaisie/GetTemplates",
                    data: { codeBranche: $("#InformationSaisie_Branche").val(), codeCible: codeCible },
                    success: function (data) {
                        if (data != null && data != "") {
                            $("#divBodyCreationSaisie").html(data);
                            MapAdresse();
                            formatDatePicker();
                            AffectDateFormat();
                            MapCommonAutoCompCourtier();
                            MapCommonAutoCompAssure();
                            MapCommonAutoCompSouscripteur();
                            MapCommonAutoCompGestionnaire();
                            //MapRechercheAvanceeCourtiers();
                            LoadListCibleByBranche();
                            LoadListTemplatesByCible();
                            LoadTemplate();
                            AffectTitleElements();
                            MapBoitesDialogue();
                            MapBtnAssureAdd();
                            MapElementPage();
                            $("img[name='RechercherCourtier'][albcontext='courtierGestion']").addClass("CursorPointer").attr("src", "/Content/Images/loupe.png");
                            toggleDescription($("#Description_Observation"));
                        }
                        else {
                            LoadListesMotsCles($("#InformationSaisie_Branche").val(), codeCible);
                            ClearElements();
                            $("#InformationSaisie_InformationTemplate_Template").html('');
                            LockUnlockElements(false);
                        }
                        $("#CabinetCourtage_CodeCabinetCourtage").val(codeCourtier).trigger("change");
                        $("#PreneurAssurance_CodePreneurAssurance").val(codeAssure).trigger("change");
                        CloseLoading();
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });
            }
        } else {
            $("#EditModeCible").val("1");
        }
        if (codeCible != "") LoadListesMotsCles($("#InformationSaisie_Branche").val(), codeCible);
    });
}

//---------------------Rechargement de l'écran en fonction du template sélectionné--------------------
function LoadTemplate() {
    $("#InformationSaisie_InformationTemplate_Template").die().live('change', function () {
        AffectTitleList($(this));
        LoadDataTemplate();
    });
}

function LoadDataTemplate() {


    var template = $("#InformationSaisie_InformationTemplate_Template").val();
    var codeBranche = $("#InformationSaisie_Branche").val();
    var codeCible = $("#InformationSaisie_InformationCible_Cible").val();
    ShowLoading();
    if (codeCible == "" || codeCible == undefined) {
        LoadListesMotsCles($("#InformationSaisie_Branche").val(), codeCible);
        ClearElements();
        $("#InformationSaisie_InformationTemplate_Template").html('');
        LockUnlockElements(true);
        CloseLoading();
    } else {
        $.ajax({
            type: "POST",
            url: "/CreationSaisie/GetTemplatePage",
            data: { codeBranche: codeBranche, codeCible: codeCible, codeTemplate: template },
            success: function (data) {
                $("#divBodyCreationSaisie").html(data);
                MapAdresse();
                formatDatePicker();
                AffectDateFormat();
                MapCommonAutoCompCourtier();
                MapCommonAutoCompAssure();
                MapCommonAutoCompSouscripteur();
                MapCommonAutoCompGestionnaire();
                LoadListCibleByBranche();
                LoadListTemplatesByCible();
                LoadTemplate();
                MapElementPage();

                MapBoitesDialogue();
                toggleDescription($("#Description_Observation"));
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

function LoadListesMotsCles(codeBranche, codeCible) {
    $.ajax({
        type: "POST",
        url: "/CreationSaisie/GetListeMotsCles",
        data: { codeBranche: codeBranche, codeCible: codeCible },
        success: function (data) {
            $("#divMotsCles").html(data);
            MapMotsCles();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

$("#btnCancelEditCible").live('click', function () {
    $("#divConfirmRsq").hide();
});

$("#btnOuiEditCible").live('click', function (evt, data) {
    DeletAllRisque();
    ValidateSuivant(evt, data);
});
//----------------Supprimer les Risque---------------------
function DeletAllRisque() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var versionOffre = $("#Offre_Version").val();
    var typeOffre = $("#Offre_Type").val();

    var codeBranche = $("#Branche").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    var id = codeOffre + "_" + versionOffre + "_" + typeOffre + "_" + codeBranche;
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/CreationSaisie/OffreRisquesSupprimer",
        data: { id: id, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) {
            common.page.isLoading = false;
        },
        error: function (jqxhr, data, erreur) {
            common.error.showXhr(request);
        }
    });

}
//------------------Submit de la form---------------------
function Suivant() {
    $("#btnSuivant").kclick(function (evt, data) {
        if ($("#EditModeCible").val() == "1") {
            EditCible();
        }
        else {
            ValidateSuivant(evt, data);
        }

    });
}

function ValidateSuivant(evt, data) {

    $("input[type=text]").removeClass('requiredField');
    $("select").removeClass('requiredField');
    var correctForm = true;

    if (!window.isReadonly)
        correctForm = ValidateForm();

    if (correctForm) {
        $("#lnkBlackList").hide();
        $("#idBlacklistedPartenaire").val("");
        ShowLoading();
        if (IsInIframe())
            $('#homeTabGuid', window.parent.document).removeAttr("disabled");
        else
            $("#tabGuid").removeAttr("disabled");
        //$("#tabGuid").removeAttr("disabled");
        $("#Offre_CodeOffre").removeAttr("disabled");
        $("#Offre_Version").removeAttr("disabled");
        $("#Offre_Type").removeAttr("disabled");
        $("#txtSaveCancel").removeAttr("disabled");
        $("#txtParamRedirect").removeAttr("disabled");
        $("#ModeNavig").removeAttr("disabled");
        $("#AddParamType").removeAttr("disabled");
        $("#AddParamValue").removeAttr("disabled");
        // $("#Description_Observation").html($("#Description_Observation").html().replace(/\n/ig, "<br>"));

        if ($("#PreneurEstAssure").attr("checked") == "checked") {
            $("#PreneurEstAssure_").val("True");
        }
        else {
            $("#PreneurEstAssure_").val("False");
        }

        let obj = $(":input").serializeObject();

        obj.txtSaveCancel = (data && data.returnHome) ? 1 : 0;
        var formDataInitiale = JSON.stringify(obj);
        var formData = formDataInitiale.replace("Offre.CodeOffre", "CodeOffre").replace("Offre.Version", "Version").replace("Offre.Type", "Type").replace("Offre.AddParamType", "AddParamType").replace("Offre.AddParamValue", "AddParamValue");
        $.ajax({
            type: "POST",
            context: $('#resultScript'),
            url: "/CreationSaisie/CreationSaisieEnregistrement",
            data: formData,
            contentType: "application/json",
            success: function (data) {
                $(this).append(data.Script);

            },
            error: function (request) {
                let result = null;
                try {
                    result = JSON.parse(request.responseText);
                } catch (e) { result = null; }

                if (!kheops.alerts.blacklist.displayAll(result)) {
                    common.error.showXhr(request);
                }

                $("#Description_Observation").html($("#Description_Observation").html().replace(/<br>/ig, "\n"));
            }
        });
    }
    else {
        window.scrollTo(0, 0);
        return false;
    }

}
//----------------------Validation de la form (champs required)-----------------
function ValidateForm() {
    var validForm = true;
    var isTemplate = $("#Offre_CodeOffre").val().substring(0, 2) == "CV";
    if (isNaN($("#ContactAdresse_CodePostal").val())) {
        $("#ContactAdresse_CodePostal").addClass('requiredField');
        validForm = false;
    }

    if ($("#CabinetCourtage_CodeCabinetCourtage").val() == "" && !isTemplate) {
        $("#CabinetCourtage_CodeCabinetCourtage").addClass('requiredField');
        validForm = false;
    }

    if ($("#InformationSaisie_Branche").val() == "") {
        $("#InformationSaisie_Branche").addClass('requiredField');
        validForm = false;
    }

    if ($("#InformationSaisie_DateSaisieString").val() == "") {
        $("#InformationSaisie_DateSaisieString").addClass('requiredField');
        validForm = false;
    }
    else {
        if (!isDate($("#InformationSaisie_DateSaisieString").val())) {
            $("#InformationSaisie_DateSaisieString").addClass('requiredField');
            validForm = false;
        }
        if (!checkDate($("#InformationSaisie_DateSaisieString"), $("#DateDuJour"))) {
            common.dialog.bigError("La date de saisie est supérieure à la date du jour", true);
            $("#InformationSaisie_DateSaisieString").addClass('requiredField');
            validForm = false;
        }
    }

    if ($("#InformationSaisie_Souscripteurs").val() == "") {
        $("#InformationSaisie_Souscripteurs").addClass('requiredField');
        validForm = false;
    }

    if ($("#Offre_CodeOffre").val() != "") {
        if ($("#InformationSaisie_Gestionnaires").val() == "") {
            $("#InformationSaisie_Gestionnaires").addClass('requiredField');
            validForm = false;
        }
    }
    if ($("#InformationSaisie_InformationCible_Cible").val() == "") {
        $("#InformationSaisie_InformationCible_Cible").addClass('requiredField');
        validForm = false;
    }

    if ($("#Description_Descriptif").val() == "") {
        $("#Description_Descriptif").addClass('requiredField');
        validForm = false;
    }

    if ($("#PreneurAssurance_CodePreneurAssurance").val() == "" && !isTemplate) {
        $("#PreneurAssurance_CodePreneurAssurance").addClass('requiredField');
        validForm = false;
    }

    if ($("#PreneurAssurance_NomPreneurAssurance").val() == "" && !isTemplate) {
        $("#PreneurAssurance_NomPreneurAssurance").addClass('requiredField');
        validForm = false;
    }

    return validForm;
}
//----------------------Annulation de la form et retour à la recherche---------------------
function CancelForm() {
    ShowLoading();
    if ($("#EditMode").val() == "True") {
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var tabGuid = IsInIframe() ? $('#homeTabGuid', window.parent.document).val() : $('#tabGuid').val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        var addParam = "";
        if (addParamType != "" && addParamValue != "")
            addParam = "addParam" + addParamType + "_" + numAvenant + "addParam";
        var param = codeOffre + "_" + version + "_" + type + tabGuid + addParam + GetFormatModeNavig($("#ModeNavig").val());
        Redirection("ModifierOffre", "Index", param);
    }
    else {
        Redirection("RechercheSaisie", "Index", "");
    }
}
//----------------------Affectation des triggers sur le CodePreneurAssurance
// et la Branche----------------------
function AffectTriggers() {
    if ($("#PreneurAssurance_CodePreneurAssurance").val() != "") {
        $("#PreneurAssurance_CodePreneurAssurance").trigger("change");
    }

    if ($("#CabinetCourtage_CodeCabinetCourtage").val() != "") {
        $("#hideInterlocuteur").val($("#CabinetCourtage_CodeInterlocuteur").val() + "_" + $("#CabinetCourtage_NomInterlocuteur").val());
        $("#CabinetCourtage_CodeCabinetCourtage").trigger("change", [true]);
    }
}

//
//Verrouille/Déverouille les éléments de la page (true pour verrouiller, false pour déverouiller)
function LockUnlockElements(toLock) {
    if (toLock == true) {
        $("#InformationSaisie_Souscripteurs").attr("disabled", "disabled");
        $("#InformationSaisie_Gestionnaires").attr("disabled", "disabled");
        $("#CabinetCourtage_CodeCabinetCourtage").attr("readonly", "readonly").addClass("readonly");
        $("#CabinetCourtage_NomCabinetCourtage").attr("readonly", "readonly").addClass("readonly");
        $("#CabinetCourtage_NomInterlocuteur").attr("disabled", "disabled").addClass("readonly");
        $("#CabinetCourtage_Reference").attr("disabled", "disabled").addClass("readonly");
        $("#PreneurAssurance_CodePreneurAssurance").attr("disabled", "disabled").addClass("readonly");
        $("#PreneurAssurance_NomPreneurAssurance").attr("disabled", "disabled").addClass("readonly");
        $("#Description_Descriptif").attr("disabled", "disabled");
        $("#MotClef1").attr("disabled", "disabled");
        $("#MotClef2").attr("disabled", "disabled");
        $("#MotClef3").attr("disabled", "disabled");
        $("div[name=btnAdresse]").removeClass("CursorPointer");
        $("#txtAreaLnk").removeClass("CursorPointer");
        $("#RechercheAvanceePreneurAssurance").removeClass("CursorPointer");
        $("#RechercheAvanceePreneurAssurance").attr("src", "/Content/Images/loupegris.png");
        $("img[name='RechercherCourtier'][albcontext='courtierGestion']").removeClass("CursorPointer").attr("src", "/Content/Images/loupegris.png");
    }
    else {
        $("#InformationSaisie_Souscripteurs").removeAttr("disabled");
        $("#InformationSaisie_Gestionnaires").removeAttr("disabled");
        $("#CabinetCourtage_CodeCabinetCourtage").removeAttr("readonly").removeClass("readonly");
        $("#CabinetCourtage_NomCabinetCourtage").removeAttr("readonly").removeClass("readonly");
        $("#CabinetCourtage_NomInterlocuteur").removeAttr("disabled").removeClass("readonly");
        $("#CabinetCourtage_Reference").removeAttr("disabled").removeClass("readonly");
        $("#PreneurAssurance_CodePreneurAssurance").removeAttr("disabled").removeClass("readonly");
        $("#PreneurAssurance_NomPreneurAssurance").removeAttr("disabled").removeClass("readonly");
        $("#Description_Descriptif").removeAttr("disabled");
        $("#MotClef1").removeAttr("disabled");
        $("#MotClef2").removeAttr("disabled");
        $("#MotClef3").removeAttr("disabled");
        $("div[name=btnAdresse]").addClass("CursorPointer");
        $("#txtAreaLnk").addClass("CursorPointer");
        $("#RechercheAvanceePreneurAssurance").addClass("CursorPointer");
        $("#RechercheAvanceePreneurAssurance").attr("src", "/Content/Images/loupe.png");
        $("img[name='RechercherCourtier'][albcontext='courtierGestion']").addClass("CursorPointer").attr("src", "/Content/Images/loupe.png");

    }

}

//Remet à zero les éléments de la page
function ClearElements() {
    $("#InformationSaisie_InformationTemplate_Template").val('');
    $("#InformationSaisie_Souscripteurs").val('');
    $("#InformationSaisie_Gestionnaires").val('');
    $("#CabinetCourtage_CodeCabinetCourtage").val('');
    $("#CabinetCourtage_NomCabinetCourtage").val('');
    $("#CabinetCourtage_NomInterlocuteur").val('');
    $("#CabinetCourtage_Reference").val('');
    $("#PreneurAssurance_CodePreneurAssurance").val('');
    $("#PreneurAssurance_NomPreneurAssurance").val('');
    $("#PreneurAssurance_Departement").val('');
    $("#PreneurAssurance_Ville").val('');
    $("#Description_Descriptif").val('');
    $("#MotClef1").val('');
    $("#MotClef2").val('');
    $("#MotClef3").val('');

    $("#ContactAdresse_Batiment").val('');
    $("#ContactAdresse_No").val('');
    $("#ContactAdresse_Extension").val('');
    $("#ContactAdresse_Voie").val('');
    $("#ContactAdresse_Distribution").val('');
    $("#ContactAdresse_CodePostal").val('');
    $("#ContactAdresse_Ville").val('');
    $("#ContactAdresse_CodePostalCedex").val('');
    $("#ContactAdresse_VilleCedex").val('');
    $("#ContactAdresse_Pays").val('');
    $("#zoneTxtArea").html('');
    $("#Description_Observation").html('');

    //remise à zéro du canevas sélectionné
    $("#CodeOffreCopy").val('');
    $("#CopyMode").val('False');
}