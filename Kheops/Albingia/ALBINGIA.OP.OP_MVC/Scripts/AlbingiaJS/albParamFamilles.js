$(document).ready(function () {
    MapElementListeFamilles();
    MapElementPage();
    MapCommonAutoCompFamille();
    MapCommonAutoCompConcepts();
});

//-------Map les éléments de la page--------
function MapElementPage() {
    $("#AjouterFamille").unbind();
    $("#AjouterFamille").bind('click', function () {
        EditFamille("I", "");
    });
    $("#btnRechercher").unbind();
    $("#btnRechercher").bind('click', function () {
        RechercherFamilles();
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "SupprimerFamille":
                var famille = $("#idFamilleSuppr").val();
                SupprimerFamille(famille);
                break;
            case "SupprimerValeur":
                var valeur = $("#idValeurSuppr").val();
                SupprimerValeur(valeur);
                break;
        }
        $("#hiddenAction").val('');
        $("#idFamilleSuppr").val('');
        $("#idValeurSuppr").val('');
    });
    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#idFamilleSuppr").val('');
        $("#idValeurSuppr").val('');
    });
    $("#btnAnnulerRetourConcept").die().live('click', function () {
        Redirection("ParamConcepts", "Index");
    });
   
   
    $("#Concept").unbind();
    $("#Concept").bind('change', function () {
        Initialiser();
    });
    $("#CodeFamilleRecherche").unbind();
    $("#CodeFamilleRecherche").bind('change', function () {
        Initialiser();
    });
    $("#LibelleFamilleRecherche").unbind();
    $("#LibelleFamilleRecherche").bind('change', function () {
        Initialiser();
    });
}
//-------Map les éléments de la liste des familles-----
function MapElementListeFamilles() {
    AlternanceLigne("BodyFamilles", "", false, null);
    $(".linkFamille").each(function () {
        $(this).click(function () {
            EditFamille("U", $(this).parent().attr("id").split("_")[1]);
            $(this).parent().parent().children(".selectLine").removeClass("selectLine");
            $(this).parent().addClass("selectLine");
        });
    });
    $("img[name=editValues]").unbind();
    $("img[name=editValues]").bind('click', function () {
        $(this).parent().parent().parent().children(".selectLine").removeClass("selectLine");
        $(this).parent().parent().addClass("selectLine");
        EditerValeurs($(this).attr("id").split("_")[1]);
    });
    $("img[name=supprFamille]").unbind();
    $("img[name=supprFamille]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $("#idFamilleSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerFamille", "Vous allez supprimer la famille.<br/>Etes-vous sûr de vouloir continuer ?",
                  320, 150, true, true);
    });
}
//-------Map les éléments de la liste des valeurs-----
function MapElementListeValeurs() {
    $(".linkValeur").each(function () {
        $(this).click(function () {
            EditerValeur('U', $(this).parent().attr("id").split("_")[1]);
            $(this).parent().parent().children(".selectLine").removeClass("selectLine");
            $(this).parent().addClass("selectLine");
        });
    });

    AlternanceLigne("BodyValeurs", "", false, null);
    $("#AjouterValeur").unbind();
    $("#AjouterValeur").bind('click', function () {
        EditerValeur("I", "");
    });

    $("img[name=dupliquerValeur]").unbind();
    $("img[name=dupliquerValeur]").bind('click', function () {
        EditerValeur('D', $(this).attr("id").split("_")[1]);
    });

    $("img[name=supprValeur]").unbind();
    $("img[name=supprValeur]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $("#idValeurSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerValeur", "Vous allez supprimer la valeur.<br/>Etes-vous sûr de vouloir continuer ?",
                  320, 150, true, true);
    });
}
//-------Map les éléments de la page EditFamille
function MapElementEditFamille() {
    $("#btnValider").unbind();
    $("#btnValider").bind('click', function () {
        UpdateFamille();
    });
    $("#btnAnnuler").unbind();
    $("#btnAnnuler").bind('click', function () {
        $("#ZoneDynamic").html('');
        $("#ZoneDynamic").hide();
    });
}
//-------Map les éléments de la div flottante EditValeur-----
function MapElementEditValeur() {
    $("#btnAnnulerDivF").unbind();
    $("#btnAnnulerDivF").bind('click', function () {
        $("#divDataEditValeur").html('');
        $("#divEditValeur").hide();
    });
    $("#btnValider").unbind();
    $("#btnValider").bind('click', function () {
        UpdateValeur();
    });
    //FormatCLEditor($("#Description"));
    //toggleDescription($("#Description"));
    formatDatePicker();
    FormatDecimalValue();
}

//-------Rechercher les familles------
function RechercherFamilles() {
    $("#Concept").removeClass('requiredField');
    if ($("#CodeConcept").val() == "") {
        $("#Concept").addClass('requiredField');
        $("#ZoneDynamic").html('');
        $("#ZoneDynamic").hide();
        $("#divBodyFamilles").html('');
        $("#divLstFamilles").hide();
        return;
    }
    ShowLoading();
    $("#ZoneDynamic").hide();
    var codeConcept = $("#CodeConcept").val();
    var codeFamille = $("#CodeFamilleRecherche").val();
    var descriptionFamille = $("#LibelleFamilleRecherche").val();
    var additionalParam = $("#AdditionalParam").val();
    var restrictionParam = $("#RestrictionParam").val();
    $.ajax({
        type: "POST",
        url: "/ParamFamilles/GetFamilles",
        data: { codeConcept: codeConcept, codeFamille: codeFamille, descriptionFamille: descriptionFamille, additionalParam: additionalParam, restrictionParam: restrictionParam },
        success: function (data) {
            CloseLoading();
            $("#divBodyFamilles").html(data);
            $("#divLstFamilles").show();
            MapElementListeFamilles();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Editer une famille-----------
function EditFamille(modeOperation, codeFamille) {
    $("#Concept").removeClass('requiredField');
    if ($("#CodeConcept").val() == "") {
        $("#Concept").addClass('requiredField');
        $("#divBodyFamilles").html('');
        $("#divLstFamilles").hide();
        $("#ZoneDynamic").html('');
        $("#ZoneDynamic").hide();
        return;
    }
    $.ajax({
        type: "POST",
        url: "/ParamFamilles/EditFamille",
        data: {
            codeConcept: $("#CodeConcept").val(), libelleConcept: $("#LibelleConcept").val(),
            codeFamille: codeFamille, restrictionParam: $("#RestrictionParam").val(),
            modeOperation: modeOperation
        },
        success: function (data) {
            $("#ZoneDynamic").html(data);
            $("#ZoneDynamic").show();
            MapElementEditFamille();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---- Editer les valeurs d'une famille----
function EditerValeurs(codeFamille) {
    if ($("#CodeConcept").val() == "") {
        $("#Concept").addClass('requiredField');
        $("#divBodyFamilles").html('');
        $("#divLstFamilles").hide();
        $("#ZoneDynamic").html('');
        $("#ZoneDynamic").hide();
        return;
    }
    $.ajax({
        type: "POST",
        url: "/ParamFamilles/EditValeurs",
        data: { codeConcept: $("#CodeConcept").val(), libelleConcept: $("#LibelleConcept").val(), codeFamille: codeFamille, additionalParam: $("#AdditionalParam").val(), restrictionParam: $("#RestrictionParam").val() },
        success: function (data) {
            $("#ZoneDynamic").html(data);
            $("#ZoneDynamic").show();
            MapElementListeValeurs();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----Update Framille (création/modification)
function UpdateFamille() {
    var erreur = false;

    if ($("#CodeFamille").val() == "") {
        $("#CodeFamille").addClass('requiredField');
        erreur = true;
    }
    if ($("#LibelleFamille").val() == "") {
        $("#LibelleFamille").addClass('requiredField');
        erreur = true;
    }
    if (erreur) {
        return false;
    }

    var codeConcept = $("#CodeConcept").val();
    var codeFamille = $("#CodeFamille").val();
    var libelleFamille = $("#LibelleFamille").val();
    var longueur = $("#Longueur").val();
    var typeCode = "A";
    if ($("#NumType").is(':checked')) {
        typeCode = "N";
    }
    var acceptNullValue = $("#AcceptNullValue").is(':checked');
    //Region variables
    var libelleCourtNum1 = $("#LibelleCourtNum1").val();
    var libelleLongNum1 = $("#LibelleLongNum1").val();
    var typeNum1 = $("#TypeNum1").val();
    var nbrDecimal1 = $("#NbrDecimal1").val();

    var libelleCourtNum2 = $("#LibelleCourtNum2").val();
    var libelleLongNum2 = $("#LibelleLongNum2").val();
    var typeNum2 = $("#TypeNum2").val();
    var nbrDecimal2 = $("#NbrDecimal2").val();

    var libelleCourtAlpha1 = $("#LibelleCourtAlpha1").val();
    var libelleLongAlpha1 = $("#LibelleLongAlpha1").val();

    var libelleCourtAlpha2 = $("#LibelleCourtAlpha2").val();
    var libelleLongAlpha2 = $("#LibelleLongAlpha2").val();

    var restriction = $("#Restriction").val();

    var modeOperation = $("#ModeOperation").val();
    var additionalParam = $("#AdditionalParam").val();
    var restrictionParam = $("#RestrictionParam").val();
    $.ajax({
        type: "POST",
        url: "/ParamFamilles/UpdateFamille",
        data: {
            codeConcept: codeConcept, codeFamille: codeFamille, libelleFamille: libelleFamille, longueur: longueur, typeCode: typeCode, acceptNullValue: acceptNullValue,
            libelleCourtNum1: libelleCourtNum1, libelleLongNum1: libelleLongNum1, typeNum1: typeNum1, nbrDecimal1: nbrDecimal1,
            libelleCourtNum2: libelleCourtNum2, libelleLongNum2: libelleLongNum2, typeNum2: typeNum2, nbrDecimal2: nbrDecimal2,
            libelleCourtAlpha1: libelleCourtAlpha1, libelleLongAlpha1: libelleLongAlpha1,
            libelleCourtAlpha2: libelleCourtAlpha2, libelleLongAlpha2: libelleLongAlpha2,
            restriction: restriction, modeOperation: modeOperation, additionalParam: additionalParam, restrictionParam: restrictionParam
        },
        success: function (data) {
            if (modeOperation != "U") {
                $("#ZoneDynamic").html('');
                $("#ZoneDynamic").hide();
            }
            $("#divBodyFamilles").html(data);
            MapElementListeFamilles();
        },
        error: function (request) {
            common.error.showXhr(request, true);
        }
    });

}
//-------Editer une valeur (Création/Modification)----------
function EditerValeur(modeOperation, codeValeur) {
    ShowLoading();
    var typeCode = $("#AlphaType").is(':checked') ? "A" : "N";
    $.ajax({
        type: "POST",
        url: "/ParamFamilles/EditerValeur",
        data: {
            codeConcept: $("#CodeConcept").val(), libelleConcept: $("#LibelleConcept").val(),
            codeFamille: $("#CodeFamille").val(), libelleFamille: $("#LibelleFamille").val(),
            libelleLongNum1: $("#LibelleLongNum1").val(), typeNum1: $("#TypeNum1").val(),
            libelleLongNum2: $("#LibelleLongNum2").val(), typeNum2: $("#TypeNum2").val(),
            libelleLongAlpha1: $("#LibelleLongAlpha1").val(), libelleLongAlpha2: $("#LibelleLongAlpha2").val(),
            codeValeur: codeValeur, modeOperation: modeOperation, longueur: $("#Longueur").val(), typeCode: typeCode, restrictionFamille: $("#RestrictionFamille").val(),
            restrictionParam: $("#RestrictionParam").val()
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataEditValeur").html(data);
            AlbScrollTop();
            $("#divEditValeur").show();
            MapElementEditValeur();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Update une valeur (Création, modification ou duplication)---
function UpdateValeur() {
    var erreur = false;
    var acceptNullValue = $("#AcceptNullValue").is(':checked');
    if ($("#CodeValeur").val() == "" && acceptNullValue == false) {
        $("#CodeValeur").addClass('requiredField');
        erreur = true;
    }
    if ($("#LibelleValeur").val() == "") {
        $("#LibelleValeur").addClass('requiredField');
        erreur = true;
    }
    if ($("#ValeurNum1").val() == "" && $("#LibelleLongNum1").val() != "") {
        $("#ValeurNum1").addClass('requiredField');
        erreur = true;
    }
    if ($("#ValeurNum2").val() == "" && $("#LibelleLongNum2").val() != "") {
        $("#ValeurNum2").addClass('requiredField');
        erreur = true;
    }

    if (erreur) {
        return false;
    }
    var codeConcept = $("#CodeConcept").val();
    var codeFamille = $("#CodeFamille").val();
    var codeValeur = $("#CodeValeur").val();
    var libelleCourt = $("#LibelleValeur").val();
    var libelleLong = $("#LibelleLongValeur").val();
    var description1 = $("#Description1").val();
    var description2 = $("#Description2").val();
    var description3 = $("#Description3").val();
    var valeurNum1 = $("#ValeurNum1").val();
    var typeNum1 = $("#TypeNum1").val();
    var valeurNum2 = $("#ValeurNum2").val();
    var typeNum2 = $("#TypeNum2").val();
    var valeurAlpha1 = $("#ValeurAlpha1").val();
    var valeurAlpha2 = $("#ValeurAlpha2").val();
    var restriction = $("#Restriction").val();
    var codeFiltre = $("#CodeFiltre").val();
    var longueur = $("#Longueur").val();
    var typeCode = $("#TypeCode").val();
    var modeOperation = $("#ModeOperation").val();
    var additionalParam = $("#AdditionalParam").val();
    var restrictionParam = $("#RestrictionParam").val();

    $.ajax({
        type: "POST",
        url: "/ParamFamilles/UpdateValeur",
        data: {
            codeConcept: codeConcept, codeFamille: codeFamille, codeValeur: codeValeur,
            libelleCourt: libelleCourt, libelleLong: libelleLong, description1: description1, description2: description2, description3: description3,
            valeurNum1: valeurNum1, valeurNum2: valeurNum2, valeurAlpha1: valeurAlpha1, valeurAlpha2: valeurAlpha2, restriction: restriction, codeFiltre: codeFiltre,
            modeOperation: modeOperation, longueur: longueur, typeCode: typeCode,
            typeNum1: typeNum1, typeNum2: typeNum2,
            additionalParam: additionalParam, restrictionParam: restrictionParam, acceptNullValue: acceptNullValue
        },
        success: function (data) {
            $("#divDataEditValeur").html('');
            $("#divEditValeur").hide();
            $("#divBodyValeurs").html(data);
            MapElementListeValeurs();
            //UpdateListValeurs(codeConcept, codeFamille, additionalParam, $("#RestrictionParam").val());
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-------------Supprime la valeur----------
function SupprimerValeur(id) {
    var additionalParam = $("#AdditionalParam").val();
    var restrictionParam = $("#RestrictionParam").val();
    var codeConcept = $("#CodeConcept").val();
    var codeFamille = $("#CodeFamille").val();
    if (id != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamFamilles/SupprimerValeur",
            data: { codeConcept: codeConcept, codeFamille: codeFamille, codeValeur: id, additionalParam: additionalParam, restrictionParam: restrictionParam },
            success: function (data) {
                $("#divBodyValeurs").html(data);
                MapElementListeValeurs();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//-------------Supprime la famille----------
function SupprimerFamille(id) {
    var additionalParam = $("#AdditionalParam").val();
    var restrictionParam = $("#RestrictionParam").val();
    var codeConcept = $("#CodeConcept").val();
    if (id != "" && id != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamFamilles/SupprimerFamille",
            data: { codeConcept: codeConcept, codeFamille: id, additionalParam: additionalParam, restrictionParam: restrictionParam },
            success: function (data) {
                $("#divBodyFamilles").html(data);
                $("#ZoneDynamic").html('');
                $("#ZoneDynamic").hide();
                MapElementListeFamilles();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//------Formate les dates---------
function formatDatePicker() {
    if ($("#TypeNum1").val() == "D")
        $("#ValeurNum1").datepicker({ dateFormat: 'dd/mm/yy' });
    if ($("#TypeNum2").val() == "D")
        $("#ValeurNum2").datepicker({ dateFormat: 'dd/mm/yy' });
}
//-------Formate les input/span des valeurs----------
function FormatDecimalValue() {
    if ($("#TypeNum1").val() == "M")
        common.autonumeric.apply($("#ValeurNum1"), 'init', 'decimal', null, null, $("#NbrDecimal1").val(), null, null);
    if ($("#TypeNum2").val() == "M")
        common.autonumeric.apply($("#ValeurNum2"), 'init', 'decimal', null, null, $("#NbrDecimal2").val(), null, null);
    if ($("#CodeValeur").attr("id") != undefined) {
        if ($("#CodeValeur").attr("albMask") != undefined) {
            var valMax = $("#CodeValeur").attr("maxlength");
            if (valMax != undefined)
                valMax = Math.pow(10, valMax) - 1;
            else
                valMax = 99999;
            common.autonumeric.apply($("#CodeValeur"), 'init', 'numeric', null, null, 0, valMax, 0);
            return;
        }
    }
    common.autonumeric.applyAll("init", 'numeric', '', null, null, '99999999999', null);
}
//----------------Redirection------------------
function Redirection(cible, job, codeConcept) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamFamilles/Redirection",
        data: { cible: cible, job: job },
        success: function (data) {
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------- Vide les listes de recherche-----
function Initialiser() {
    $("#divBodyFamilles").html('');
    $("#ZoneDynamic").html('');
    $("#ZoneDynamic").hide();
}

