$(document).ready(function () {
    MapRechercheBtns();
    FormatNumericField();
});

//-----------Mappage du Body
function MapRechercheBtns() {

    $("#btnRechercherLog").die();
    $("#btnRechercherLog").click(function () {
        GetLogWithFiltre();
    });

    $("#btnInitializeRechercheLog").die().live('click', function () {
        InitFiltre();
    });
}

//--------formattage numeric filtre
function FormatNumericField() {
    common.autonumeric.apply($("#NumAliment"), 'init', 'numeric', '', ',', '0', null, '0');
    common.autonumeric.apply($("#NumAvenant"), 'init', 'numeric', '', ',', '0', null, '0');
    common.autonumeric.apply($("#NumHisto"), 'init', 'numeric', '', ',', '0', null, '0');
    common.autonumeric.apply($("#IdRisque"), 'init', 'numeric', '', ',', '0', null, '0');
    common.autonumeric.apply($("#IdObjet"), 'init', 'numeric', '', ',', '0', null, '0');
}

//----------- lancer la recherche de stat avec le filtre
function GetLogWithFiltre() {

    ShowLoading();

    var filtre = GetModelFiltre();

    $.ajax({
        type: "POST",
        url: "/LogRecupParamCible/List",
        data: filtre,
        success: function (data) {
            $("#ListResultLog").html(data);
            CloseLoading();
        },
        error: function (request) {
            CloseLoading();
            ShowFancyError(request);
        }
    });

}

//--------------validattion model
function ValidateFiltre(filtre) {   

    return error;
}

//-----------------retourner le model de filtre
function GetModelFiltre() {
    var filtreModel = {
        "TypeOffrePolice": $("#TypeOffrePolice").val(),
        "NumOffrePolice": $("#NumOffrePolice").val(),
        "NumAliment": $("#NumAliment").val(),
        "NumAvenant": $("#NumAvenant").val(),
        "NumHisto": $("#NumHisto").val(),
        "IdRisque": $("#IdRisque").val(),
        "IdObjet": $("#IdObjet").val()
    }
    return filtreModel;
}


//---------------retourner le model de filtre
function InitFiltre() {
    $("#TypeOffrePolice").val("");
    $("#NumOffrePolice").val("");
    $("#NumAliment").val("");
    $("#NumAvenant").val("");
    $("#NumHisto").val("");
    $("#IdRisque").val("");
    $("#IdObjet").val("");
}


//-------------- export CSV
function ExportToCSV() {
    var filtre = GetModelFiltre();
    window.location.href = "/LogRecupParamCible/ExportFile?" + "&typeOffrePolice=" + filtre.TypeOffrePolice
                                                      + "&numOffrePolice =" + filtre.NumOffrePolice
                                                      + "&numAliment=" + filtre.NumAliment
                                                      + "&numAvenant=" + filtre.NumAvenant
                                                      + "&numHisto=" + filtre.NumHisto
                                                      + "&idRisque=" + filtre.IdRisque
                                                      + "&idObjet=" + filtre.IdObjet
    return true;
}


//---------afficher une message d'erreur
function ShowFancyError(request) {
    common.error.showXhr(request);
}