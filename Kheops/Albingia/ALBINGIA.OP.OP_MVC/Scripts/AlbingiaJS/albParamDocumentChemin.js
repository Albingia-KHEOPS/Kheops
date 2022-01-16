/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementPage();
});

function MapElementPage() {
    AlternanceLigne("CheminBody", null, true);

    $("#btnAjouterChemin").die().live("click", function () {
        OpenDetailsChemin("");
    });

    $("td[name=clickableCol]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            var id = $(this).attr("id").split("¤")[1];
            OpenDetailsChemin(id);
        });
    });

    $("img[name=supprChemin]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            var id = $(this).attr("id").split("¤")[1];
            SuppressionChemin(id);
        });
    });
}

function MapElementDetails() {
    $("#TypeChemin").die().live("change", function () {
        GetTypologie($("#TypeChemin").val());
        AffectReadonlyElements();
    });

    $("#btnEnregistrer").die().live("click", function () {
        EnregistrerChemin();
    });

    AffectReadonlyElements();
}

function AffectReadonlyElements() {
    if ($("#TypeChemin").val() == "D") {
        $("#Serveur").addClass("readonly");
        $("#Serveur").attr("readonly", "readonly");
        $("#Serveur").val($("#defaultServeurURL").val());

        $("#Racine").addClass("readonly");
        $("#Racine").attr("readonly", "readonly");
        $("#Racine").val($("#defaultRacineURL").val());
    }
    else {
        $("#Serveur").removeClass("readonly");
        $("#Serveur").removeAttr("readonly");

        $("#Racine").removeClass("readonly");
        $("#Racine").removeAttr("readonly");
    }
}

function OpenDetailsChemin(identifiant) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamDocumentChemin/GetDetailsChemin/",
        data: { identifiant: identifiant },
        success: function (data) {
            $("#divCheminDetails").html(data);
            MapElementDetails();
            CloseLoading();
            $("#VoletDetail").show();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//Met à jour le controle des typologies en fonction du type de chemin
function GetTypologie(typeChemin) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamDocumentChemin/GetTypologies/",
        data: { typeChemin: typeChemin },
        success: function (data) {
            $("#divTypologies").html(data);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//Enregistre le chemin en base de données
function EnregistrerChemin() {
    var typeChemin = $("#TypeChemin").val();
    var typologie = $("#Typologie").val();
    var libelleChemin = $("#LibelleChemin").val();
    var identifiantChemin = $("#identifiantChemin").val();
    var chemin = $("#Chemin").val();
    var serveur = $("#Serveur").val();
    var racine = $("#Racine").val();
    var environnement = $("#Environnement").val();
   
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamDocumentChemin/EnregistrerChemin/",
        data: { identifiantChemin: identifiantChemin, typeChemin: typeChemin, typologie: typologie, libelleChemin: libelleChemin, chemin: chemin, serveur: serveur, racine: racine, environnement: environnement },
        success: function (data) {
            $("#divBodyChemins").html(data);
            $("#VoletDetail").hide();
            MapElementPage();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//Supprime le chemin de la base de données
function SuppressionChemin(identifiant) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamDocumentChemin/SupprimerChemin/",
        data: { identifiant: identifiant },
        success: function (data) {
            $("#divBodyChemins").html(data);
            $("#VoletDetail").hide();
            MapElementPage();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}