$(document).ready(function () {
    MapElementPage();
});

//----------Map les éléments de la page--------
function MapElementPage() {
    common.autonumeric.apply($("#CibleVersion"), 'init', 'numeric', ' ', ',', '0', '99999', '0');

    $("#RechercheOffreRecup").die().live('click', function () {
        RecupOffre();
    });

    $("#InitaliseInputs").die().live('click', function () {
        InitSaisie();
    });

    $("#TraceBtn").die().live('click', function () {
        GetTrace();
    });
}

//-------------charger les trace
function GetTrace() {
    ShowLoading();

    var filtre = {
        "NumOffrePolice": $("#cibleCodeOffre").val(),
    }

    $.ajax({
        type: "POST",
        url: "/LogRecupParamCible/Index",
        data: filtre,
        success: function (data) {
            $("#divDataLog").html(data);
            $("#divLog").show();
            $("#RetourBlok").show();
            MapPartielLogDiv();
            CloseLoading();
        },
        error: function (request) {
            CloseLoading();
            ShowFancyError(request);
        }
    });
}

//-----------Map les elements de retour vue partiel
function MapPartielLogDiv() {
    $("#btnRetour").die().live('click', function () {
        $("#divDataLog").empty();
        $("#divLog").hide();
    });
}


//-----------Map les elements de retour vue partiel
function MapPartielResultPage() {
    $("#MigrateCible").die().live('click', function () {
        MigrationCible();
    });
}

//----------lancer la recuperation
function RecupOffre() {
    var error = false;
    var codeOffre = $("#cibleCodeOffre").val();
    var version = $("#CibleVersion").val();

    if (IsUndifinedOrEmpty(codeOffre)) {
        $("#cibleCodeOffre").addClass("requiredField");
        error = true;
    }
    else {
        $("#cibleCodeOffre").removeClass("requiredField");
    }

    if (IsUndifinedOrEmpty(version)) {
        $("#CibleVersion").addClass("requiredField");
        error = true;
    }
    else {
        $("#CibleVersion").removeClass("requiredField");
    }

    if (error) {

        ShowCommonFancy("Error", "", "Votre saisie est incorrecte, Le CodeOffre et la Version sont obligatoire"
            , 1212, 700, true, true);
    }
    else {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamCibleRecup/RecupOffre",
            data: { codeOffre: codeOffre, version: version },
            success: function (data) {
                $("#dvCibleResultSearch").html(data);
                MapPartielResultPage();
                $("#dvCibleResultSearch").show();
                if (!$("#ErrorMessage")) {
                    LockSaisie();
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//--------------bloquer les inputs de recherche
function LockSaisie() {
    $('#cibleCodeOffre').attr('readonly', true);
    $('#cibleCodeOffre').addClass('readonly');

    $('#CibleVersion').attr('readonly', true);
    $('#CibleVersion').addClass('readonly');

    $('#RechercheOffreRecup').attr('readonly', true);
}

//-------------initialiser les inputs de recherche
function InitSaisie() {
    $('#cibleCodeOffre').val("");
    $('#CibleVersion').val("");
    $("#CibleVersion").removeClass("requiredField");
    $("#cibleCodeOffre").removeClass("requiredField");

    $('#RechercheOffreRecup').attr('readonly', false);
    $("#dvCibleResultSearch").hide();
    RemoveReadOnly();
}

//-------------enlever la lecture seul des inputs de recherche
function RemoveReadOnly() {
    $('#cibleCodeOffre').attr('readonly', false);
    $('#cibleCodeOffre').removeClass('readonly');

    $('#CibleVersion').attr('readonly', false);
    $('#CibleVersion').removeClass('readonly');
}

//---------------lancer la migration vers cible
function MigrationCible() {

    var codeOffre = $("#cibleCodeOffre").val();
    var version = $("#CibleVersion").val();
    var cible = $("#Cible").val();
    var cibleRecup = $("#CibleRecup").val();
    var type = $("#TypeOffre").val();

    if (IsUndifinedOrEmpty(codeOffre) ||
            IsUndifinedOrEmpty(version) ||
            IsUndifinedOrEmpty(cible) ||
            IsUndifinedOrEmpty(cibleRecup) ||
            IsUndifinedOrEmpty(type)) {

        common.dialog.bigError("Votre saisie est incorrecte", true);
    }
    else {
        ShowLoading();

        $.ajax({
            type: "POST",
            url: "/ParamCibleRecup/MigrationCible",
            data: { codeOffre: codeOffre, version: version, cible: cible, cibleRecup: cibleRecup, type: type },
            success: function (data) {
                $("#dvCibleResultSearch").empty();
                $("#dvCibleResultSearch").hide();
                RemoveReadOnly();
                common.dialog.bigInfo(data, true);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//----------------tester si str est vide 
function IsUndifinedOrEmpty(str) {
    if (str == undefined || str == null || str == "") {
        return true;
    }
    return false;
}

//---------afficher une message d'erreur
function ShowFancyError(request) {
    common.error.showXhr(request);
}
