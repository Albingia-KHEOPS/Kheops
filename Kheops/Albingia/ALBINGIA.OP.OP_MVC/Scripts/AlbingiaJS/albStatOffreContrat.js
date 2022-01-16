$(document).ready(function () {
    MapRechercheBtns();
    FormatNumericField();
});

//-----------Mappage du Body
function MapRechercheBtns() {

    $("#btnRechercherStatOffreContrat").die();
    $("#btnRechercherStatOffreContrat").click(function () {
        GetStatWithFiltre();
    });

    $("#btnInitialize").die().live('click', function () {
        InitFiltre();
    });
}

//--------formattage numeric filtre
function FormatNumericField() {

    common.autonumeric.apply($("#Annee"), 'init', 'numeric', '', ',', '0', '9999', '0');
    common.autonumeric.apply($("#Mois"), 'init', 'numeric', '', ',', '0', '99', '0');
    common.autonumeric.apply($("#Jour"), 'init', 'numeric', '', ',', '0', '99', '0');
}

//----------- lancer la recherche de stat avec le filtre
function GetStatWithFiltre() {

    ShowLoading();

    var filtre = GetModelFiltre();
    if (filtre == undefined || filtre==null)
    {
        ShowCommonFancy("Error", "", "Veuillez choisir si produit Kheops ou Hors Kheops (minimum 1 choix coché)."
           , 1212, 700, true, true);
        return false;
    }

    if (ValidateFiltre(filtre)) {
        CloseLoading();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            url: "/Statistique/List",
            data: filtre,
            success: function (data) {
                $("#ListResultOffreContrat").html(data);
                CloseLoading();
            },
            error: function (request) {
                CloseLoading();
                ShowFancyError(request);
            }
        });
        return true;
    }
}

//--------------validattion model
function ValidateFiltre(filtre) {

    var currentYear = new Date().getFullYear();
    var error = false;

    if (filtre.Annee > currentYear) {
        //message = "L'annee est superieur à " + currentYear;
        $("#Annee").addClass("requiredField");
        error = true;
    }
    else {
        $("#Annee").removeClass("requiredField");
    }

    if (filtre.Mois > 12) {
        $("#Mois").addClass("requiredField");
        error = true;
    }
    else {
        $("#Mois").removeClass("requiredField");
    }

    if (filtre.Jour > 31) {
        $("#Jour").addClass("requiredField");
        error = true;
    }
    else {
        $("#Jour").removeClass("requiredField");
    }

    return error;
}

//-----------------retourner le model de filtre
function GetModelFiltre() {

    var categorieState=$("#Categorie").attr("checked")== "checked";
    var NonKheState=$("#NonKhe").attr("checked")== "checked";
    var categorie = "";

    if(!categorieState && !NonKheState)
    {
        return null;        
    }
    else if(categorieState && !NonKheState){
        categorie="2";
    }else if(!categorieState && NonKheState){
        categorie="3";
    }
    else if(categorieState && NonKheState){
        categorie = "4";
    }

    var filtreModel = {
        "Branche": $("#Branche").val(),
        "Categorie": categorie,
        "Etat": $("#Etat").val(),
        "Type": $("#Type").val(),
        "Annee": $("#Annee").val(),
        "Mois": $("#Mois").val(),
        "Jour": $("#Jour").val()
    }
    return filtreModel;
}


//---------------retourner le model de filtre
function InitFiltre() {
    $("#Branche").val("");
    $("#Categorie").attr("checked", true);
    $("#NonKhe").attr("checked", false);
    $("#Etat").val("");
    $("#Type").val("P");
    $("#Annee").val("");
    $("#Mois").val("");
    $("#Jour").val("");
}


//-------------- export CSV
function ExportToCSV() {
    var filtre = GetModelFiltre();
    if(filtre==null)
    {
        alert("Veuillez choisir produit Kheops ou non (minimum un choix)?");
        return false();
    }

    window.location.href = "/Statistique/ExportFile?" + "&branche=" + filtre.Branche
                                                      + "&categorie =" + filtre.Categorie                                                      
                                                      + "&etat=" + filtre.Etat
                                                      + "&type=" + filtre.Type
                                                      + "&annee=" + filtre.Annee
                                                      + "&mois=" + filtre.Mois
                                                      + "&jour=" + filtre.Jour
    return true;
}


//---------afficher une message d'erreur
function ShowFancyError(request) {
    common.error.showXhr(request);
}