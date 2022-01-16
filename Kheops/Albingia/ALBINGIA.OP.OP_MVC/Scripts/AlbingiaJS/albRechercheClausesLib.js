/// <reference path="Common/common.js" />
/// <reference path="albCommonAutoComplete.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementsPage();
});

function MapElementsPage() {
    PaginationLAB();

    $("#btnStatRechercher").die().live("click", function () {
        Rechercher();
    });

    $("#btnInitialize").die().live('click', function () {
        InitializeForm();
    });

    $(".datepicker").datepicker({ dateFormat:'dd/mm/yy' });
}

function Rechercher() {
    var datesaisiedeb = $("#Datesaisdebut").val();
    var datesaisiefin = $("#Datesaisfin").val();
    var datecreatedeb = $("#Datedebcreate").val();
    var datecreatefin = $("#Datefincreate").val();

    //DateSaisie
    $(".requiredField").removeClass("requiredField");
    if (datesaisiedeb != "" && !isDate(datesaisiedeb)) {
        $('#Datesaisdebut').addClass("requiredField");
        return false;
    }
    if (datesaisiefin != "" && !isDate(datesaisiefin)) {
        $('#Datesaisfin').addClass("requiredField");
        return false;
    }
    if (datesaisiedeb != "" && datesaisiefin != "") {
        var checkD = checkDate($('#Datesaisdebut'), $('#Datesaisfin'));
        if (!checkD) {
            ShowCommonFancy("Error", "",
            "Veuillez sélectionner une date de saisie antérieure<br/> ",
            350, 70, true, true);
            return false;
        }
    }

    //Date Creation
    $(".requiredField").removeClass("requiredField");
    if (datecreatedeb != "" && !isDate(datecreatedeb)) {
        $('#Datedebcreate').addClass("requiredField");
        return false;
    }
    if (datecreatefin != "" && !isDate(datecreatefin)) {
        $('#Datefincreate').addClass("requiredField");
        return false;
    }
    if (datecreatedeb != "" && datecreatefin != "") {
        var checkD = checkDate($('#Datedebcreate'), $('#Datefincreate'));
        if (!checkD) {
            ShowCommonFancy("Error", "",
            "Veuillez sélectionner une date de création  antérieure<br/>",
            350, 70, true, true);
            return false;
        }
    }

    var pageNumberRecherche = $("#PaginationPageActuelle").html();
    if (pageNumberRecherche == null || pageNumberRecherche == undefined) {
        pageNumberRecherche = 1;
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/StatClausesLibres/Rechercher/",
        data: {

            datedebutsaisie: datesaisiedeb, datefinsaisie: datesaisiefin, datedebutcreation: datecreatedeb, datefincreation: datecreatefin,

            pageNumber: pageNumberRecherche
        },

        success: function (data) {

            $("#divResultRechercheIntegre").html(data);

            AlternanceLigne("ResultatsBody");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//----------------------Gestion de la pagination---------------------
function PaginationLAB() {
    //----------------------Pagination de la recherche d'offres---------------------
    $("#PaginationPrecedent").die().live("click", function () {
        var PaginationPageActuelle = parseInt($("#PaginationPageActuelle").html());
        PaginationPageActuelle--;
        $("#PaginationPageActuelle").html(PaginationPageActuelle);
        Rechercher();
    });
    $("#PaginationSuivant").die().live("click", function () {
        var PaginationPageActuelle = parseInt($("#PaginationPageActuelle").html());
        PaginationPageActuelle++;
        $("#PaginationPageActuelle").html(PaginationPageActuelle);
        Rechercher();
    });
    $("#PaginationPremierePage").die().live("click", function () {
        $("#PaginationPageActuelle").html(1);
        Rechercher();
    });
    $("#PaginationDernierePage").die().live("click", function () {
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
        Rechercher();
    });
}

//----------Réinitialise la page-----------------
function InitializeForm() {
    $("#Datesaisdebut").val('');
    $("#Datesaisfin").val('');
    $("#Datedebcreate").val('');
    $("#Datefincreate").val('');

}

//-------------- export CSV
function ExportToCSV() {
    var datesaisiedeb = $("#Datesaisdebut").val();
    var datesaisiefin = $("#Datesaisfin").val();
    var datecreatedeb = $("#Datedebcreate").val();
    var datecreatefin = $("#Datefincreate").val();

   
    if (datesaisiedeb != "" || datesaisiefin != "" || datecreatedeb != "" || datecreatefin != "") {
        window.location.href = "/StatClausesLibres/ExportFile?" + "&Datesaisdebut=" + datesaisiedeb
                                                          + "&Datesaisfin =" + datesaisiefin
                                                          + "&Datedebcreate=" + datecreatedeb
                                                          + "&Datefincreate=" + datecreatefin

        return true;
    }
    else {
        ShowCommonFancy("Error", "",
           "Veuillez choisir une date (minimum un choix)?<br/>",
           350, 70, true, true);
        
    }
}
