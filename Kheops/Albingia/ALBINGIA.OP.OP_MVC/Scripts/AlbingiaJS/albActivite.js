$(document).ready(function () {
    GetActivitesList();
});
function GetActivitesList() {
    $("#btnrechercheActivite").kclick(function () {
        RechercherActivites();
    });

    $("#btnResetActivite").kclick(function () {
        $("#InformationsGenerales_CodeTre").val("");
        LoadCodeClasse("", "");
    });
}

function RechercherActivites() {
    var codeBranche = $("#Offre_Branche").val();
    var codeCible = $("#InformationsGenerales_Cible").val().split('-')[0];
    var pageNumber = $("#ActivitePaginationPageActuelle ").html();
    if (pageNumber == null || pageNumber == undefined || pageNumber == "") {
        pageNumber = 1;
    }
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/DetailsRisque/GetActivites",
        data: { codeBranche: codeBranche, codeCible: codeCible, pageNumber: pageNumber, code: "", nom: "", search: false },
        success: function (data) {
            $("#divDataRechercheActivite").html(data);
            $("#divRechercheActivite").show();
            AlternanceLigne("Activite", "Code", true, null);
            MapElementListActivites();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function MapElementListActivites() {
    MapCommonAutoCompFamille();
    SelectLigne();
    PaginationActivites();
    $("#RechercherFamButton").die().live('click', function () {
        FilterListesActivites();
    });
    $("#btnfermerActivite").die().live('click', function () {
        $("#divDataRechercheActivite").html("");
        $("#divRechercheActivite").hide();
    });
    $("#btnInitializeRech").die().live('click', function () {
        Initialiser();
    });
}
function Initialiser() {
    $("#CodeActiviteRecherche").val("");
    $("#NomActiviteRecherche").val("");
    RechercherActivites();
}
function SelectLigne() {
    $("tr[albactivite]").die().live('click', function () {
        var code = $(this).attr("albactivite").split('_')[0];
        var nom = $(this).attr("albactivite").split('_')[1];
        var tailleCode = $.trim(code).length;
        if (tailleCode > 5) {
            $(this).addClass("ligneRed");
            $("#IdMessageLabel").html("**Le code de la ligne en rouge ne sera pas enregistré il faudra mettre un code de taille max égale à 5 caractères");
        }
        else {
            $("#InformationsGenerales_CodeTre").val($.trim(code) + " - " + $.trim(nom));
            LoadCodeClasse($("#InformationsGenerales_Cible").val().split('-')[0], $.trim(code));
            $("#btnfermerActivite").trigger('click');
        }
    });
}
function PaginationActivites() {
    //----------------------Pagination de la recherche d'offres---------------------
    $("#ActivitePaginationPrecedent").die().live("click", function () {
        var PaginationPageActuelle = parseInt($("#ActivitePaginationPageActuelle").html());
        PaginationPageActuelle--;
        $("#ActivitePaginationPageActuelle").html(PaginationPageActuelle);
        var sorting = $("#tridefault").val();
        var ordre = "ASC";
        RechercherActivites(sorting, ordre);
    });
    $("#ActivitePaginationSuivant").die().live("click", function () {
        var PaginationPageActuelle = parseInt($("#ActivitePaginationPageActuelle").html());
        PaginationPageActuelle++;
        $("#ActivitePaginationPageActuelle").html(PaginationPageActuelle);
        var sorting = $("#tridefault").val();
        var ordre = "ASC";
        RechercherActivites(sorting, ordre);
    });
    $("#ActivitePaginationPremierePage").die().live("click", function () {
        $("#ActivitePaginationPageActuelle").html(1);
        var sorting = $("#tridefault").val();
        var ordre = "ASC";
        RechercherActivites(sorting, ordre);
    });
    $("#ActivitePaginationDernierePage").die().live("click", function () {
        var PaginationTotal = parseInt($("#ActivitePaginationTotal").html());
        var PaginationStart = parseInt($("#ActivitePaginationStart").html());
        var PaginationEnd = parseInt($("#ActivitePaginationEnd").html());
        var paginationSizeDP = PaginationEnd - PaginationStart + 1;
        if (PaginationTotal < getPaginationSize()) {
            $("#ActivitePaginationPageActuelle").html(Math.ceil(PaginationTotal / paginationSizeDP));
        }
        else {
            $("#ActivitePaginationPageActuelle").html(Math.ceil(getPaginationSize() / paginationSizeDP));
        }
        var sorting = $("#tridefault").val();
        var ordre = "ASC";
        RechercherActivites(sorting, ordre);
    });
}
function FilterListesActivites() {
    GetListActivites();
}
function GetListActivites() {
    var codeBranche = $("#Offre_Branche").val();
    var codeCible = $("#InformationsGenerales_Cible").val().split('-')[0];
    var code = $("#CodeActiviteRecherche").val();
    var nom = $("#NomActiviteRecherche").val();
    var pageNumber = $("#ActivitePaginationPageActuelle ").val();
    if (pageNumber == null || pageNumber == undefined || pageNumber == "") {
        pageNumber = 1;
    }
    if (code == undefined) {
        code = "";
    }
    if (nom == undefined) {
        nom = "";
    }
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/DetailsRisque/GetActivites",
        data: { codeBranche: codeBranche, codeCible: codeCible, pageNumber: pageNumber, code: code, nom: nom, search: true },
        success: function (data) {
            $("#ActiviteRechercherResultDialogDiv").html(data);
            AlternanceLigne("Activite", "Code", true, null);
            MapElementListActivites();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Charge le code classe en fonction du code activité----------
function LoadCodeClasse(codeCible, codeActivite) {
    $.ajax({
        type: "POST",
        url: "/DetailsRisque/LoadCodeClassByCible",
        data: { codeCible: codeCible, codeActivite: codeActivite },
        success: function (data) {
            $("#InformationsGenerales_CodeClasse").val(data);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

