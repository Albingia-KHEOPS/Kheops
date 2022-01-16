/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementPage();
});

//----------------Map les éléments de la page------------------
function MapElementPage() {

    $("#ajouterReference").die().live('click', function () {
        if ($(this).hasClass("CursorPointer")) {
            SwitchModeLigne($("#idLigneVide").val());
            $("#divReferencesBodyEmptyLine").show();
            $("img[id=ajouterReference]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
        }
    });

    $("#btnTerminerObservation").die().live('click', function () {
        $("#observation_" + $("#codeLigne").val()).val($("#txtAreaObservation").val());
        $("#txtAreaObservation").val('');
        $("#codeLigne").val('');
        $("#divFullScreen").hide();
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                $("#actionParameter").val('');
                break;
            case "Delete":
                SupprimerReferentiel();
                break;
        }
        $("#hiddenAction").val('');
    });

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    $("#btnAnnuler").die().live('click', function () {
        window.location.href = "/BackOffice/Index";
    });

    MapElementTableau();
}

function MapElementTableau() {
    $("td[name=selectableCol]").die().live('click', function () {
        var id = $(this).attr('id').split('_')[2];
        if (id != undefined && id != "") {
            var selectedRow = $("#selectedRow").val();
            if (selectedRow == undefined || selectedRow == "" || selectedRow != id)
                SwitchModeLigne(id);
        }
    });

    $("img[name=saveReference]").die().live('click', function () {
        SaveLineReference($(this).attr('id').split('_')[1]);
    });

    $("img[name=deleteReference]").die().live('click', function () {
        $("#actionParameter").val($(this).attr('id').split('_')[1]);
        ShowCommonFancy("Confirm", "Delete", "Etes-vous sûr de vouloir supprimer cette référence ?", 320, 150, true, true);
    });

    $("input[name=observation]").die().live('click', function () {
        $("#codeLigne").val($(this).attr('id').split('_')[1])
        $("#txtAreaObservation").val($(this).val());
        $("#divFullScreen").show();
    });

    AlternanceLigne("ReferencesBody", "noInput", true, null);
    AlternanceLigne("ReferencesBodyEmptyLine", "noInput", true, null);
}

//---------------fonction qui permet de passer du mode readonly au mode edition pour une ligne donnée----------
function SwitchModeLigne(idLigne) {
    var newLineDisplayed = false;
    if (idLigne != $("#idLigneVide").val()) {
        $("#divReferencesBodyEmptyLine").hide();
        $("img[id=ajouterReference]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
    }
    else
        $("#divReferencesBodyEmptyLine").show();

    if ($("#selectedRow").val() != "") {
        if (idLigne != $("#selectedRow").val()) {
            //Affiche l'ancienne ligne editée en mode readonly
            AfficherLigne("readonly", $("#selectedRow").val(), idLigne);
        }
    }
    else
        AfficherLigne("edition", idLigne);
    $("#selectedRow").val(idLigne);
    // MapElementTableau();
}

//----------------Affiche la ligne dans le mode donné (readonly/edition)--------
function AfficherLigne(mode, idLigne, idLigneToDisplayNext) {

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamISReference/AfficherLigne/",
        data: {
            mode: mode, code: idLigne.replace(/¤/g, " ")
        },
        success: function (data) {
            $("#referenceIS_" + idLigne).html(data);
            if (idLigneToDisplayNext != undefined && idLigneToDisplayNext != "" && mode == "readonly")
                AfficherLigne("edition", idLigneToDisplayNext);
            else
                CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//----------------Sauvegarde une reference--------------------
function SaveLineReference(idLigne) {
    var code = idLigne;
    var typeOperation = "Insert";
    if (code != $("#idLigneVide").val())
        typeOperation = "Update";

    if (code == undefined) {
        code = $("#actionParameter").val();
        typeOperation = "Delete";
    }

    $("#longueurZone_" + idLigne).removeClass('requiredField');
    $("#longueurZone_" + idLigne).attr('title', 'Taille du champ');
    $("#nomInterne_" + idLigne).removeClass('requiredField');
    $("#nomInterne_" + idLigne).attr('title', 'Nom unique du référentiel');
    $("#referenceDescription_" + idLigne).removeClass('requiredField');
    $("#referenceDescription_" + idLigne).attr('title', "");
    $("#libelle_" + idLigne).removeClass('requiredField');
    $("#libelle_" + idLigne).attr('title', "");

    var description = $("#referenceDescription_" + idLigne).val();
    var libelle = $("#libelle_" + idLigne).val();
    var typeZone = $("#typeZone_" + idLigne).val();
    var longueurZone = $("#longueurZone_" + idLigne).val();
    var mappage = $("#drlMappage_" + idLigne).val();
    var conversion = $("#drlConversion_" + idLigne).val();
    var presentation = $("#drlPresentation_" + idLigne).val();
    var typeUI = $("#drlTypeUI_" + idLigne).val();
    var obligatoire = $("#drlObligatoire_" + idLigne).val();
    var affichage = $("#drlAffichage_" + idLigne).val();
    var controle = $("#drlControle_" + idLigne).val();
    var observation = $("#observation_" + idLigne).val();
    var valeurDefaut = $("#valeurDefaut_" + idLigne).val();
    var tcon = $("#tcon_" + idLigne).val();
    var tfam = $("#tfam_" + idLigne).val();

    if (typeOperation == "Insert") {
        code = $("#nomInterne_" + idLigne).val();
    }

    code = code.replace(/¤/g, " ");
    var isCorrect = true;

    if (typeZone == "Double") {
        if (longueurZone.split(':').length == 2) {
            var longueurEntier = parseInt(longueurZone.split(':')[0]);
            var longueurDecimal = parseInt(longueurZone.split(':')[1]);

            if (isNaN(longueurEntier) || longueurEntier < 1 || longueurEntier > 12) {
                $("#longueurZone_" + idLigne).addClass('requiredField');
                $("#longueurZone_" + idLigne).attr('title', "Partie entière incorrecte, veuillez saisir un nombre entre 1 et 12");
                isCorrect = false;
            }

            if (isNaN(longueurDecimal) || longueurDecimal < 1 || longueurDecimal > 5) {
                $("#longueurZone_" + idLigne).addClass('requiredField');
                $("#longueurZone_" + idLigne).attr('title', "Partie décimale incorrecte, veuillez saisir un nombre entre 1 et 5");
                isCorrect = false;
            }
        }
        else {
            $("#longueurZone_" + idLigne).addClass('requiredField');
            $("#longueurZone_" + idLigne).attr('title', "Champ incorrect pour le type de donnée choisi, veuillez saisir une valeur sous la forme '10:2'");
            isCorrect = false;
        }
    }

    if (typeZone == "string") {
        var longueurEntier = parseInt(longueurZone);
        if (isNaN(longueurEntier) || longueurEntier < 1 || longueurEntier > 5000 || longueurZone.split(':').length != 1) {
            $("#longueurZone_" + idLigne).addClass('requiredField');
            $("#longueurZone_" + idLigne).attr('title', "Valeur incorrecte, veuillez saisir un nombre entre 1 et 5000");
            isCorrect = false;
        }
    }

    if (typeZone == "Int64") {
        var longueurEntier = parseInt(longueurZone);
        if (isNaN(longueurEntier) || longueurEntier < 1 || longueurEntier > 12 || longueurZone.split(':').length != 1) {
            $("#longueurZone_" + idLigne).addClass('requiredField');
            $("#longueurZone_" + idLigne).attr('title', "Valeur incorrecte, veuillez saisir un nombre entre 1 et 12");
            isCorrect = false;
        }
    }

    if (code == undefined || code == "") {
        $("#nomInterne_" + idLigne).addClass('requiredField');
        $("#nomInterne_" + idLigne).attr('title', "Champ obligatoire");
            isCorrect = false;
        }

    if (description == undefined || $.trim(description) == "")
    {
        $("#referenceDescription_" + idLigne).addClass('requiredField');
        $("#referenceDescription_" + idLigne).attr('title', "Champ obligatoire");
    }

    if (libelle == undefined || $.trim(libelle) == "") {
        $("#libelle_" + idLigne).addClass('requiredField');
        $("#libelle_" + idLigne).attr('title', "Champ obligatoire");
    }

    if (longueurZone == "")
        longueurZone = 0;

    if (conversion == "\"\"")
        conversion = " ";

    if (isCorrect)
        isCorrect = CheckValeurDefautIsCorrect(typeZone, longueurZone, idLigne, valeurDefaut);

    if (isCorrect) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamISReference/SaveLineReference/",
            data: {
                code: code, description: description, libelle: libelle, typeZone: typeZone, longueurZone: longueurZone,
                mappage: mappage, conversion: conversion, presentation: presentation, typeUI: typeUI, obligatoire: obligatoire,
                affichage: affichage, controle: controle, observation: observation, typeOperation: typeOperation, valeurDefaut: valeurDefaut,
                tcon: tcon, tfam: tfam
            },
            success: function (data) {
                $("#divReferencesBodyLines").html(data);
                MapElementTableau();
                if (typeOperation == "Insert") {
                    $("#referenceIS_" + $("#idLigneVide").val()).hide();
                }
                $("img[id=ajouterReference]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//---------------Supprime un réferentiel----------------------
function SupprimerReferentiel() {
    var code = $("#actionParameter").val();
    if (code != undefined || code != "") {
        code = code.replace(/¤/g, " ");
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamISReference/SupprimerLineReference/",
            data: {
                code: code
            },
            success: function (data) {
                $("#divReferencesBodyLines").html(data);
                MapElementTableau();
                $("#actionParameter").val('');
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}


//---------------------------- Controle Valeur par Defaut saisie ------------------
function CheckValeurDefautIsCorrect(typeZone, longueurZone, idLigne, valeurDefaut) {
    var isCorrect = true;
    var longueurValDefaut = valeurDefaut.length;

    if (longueurValDefaut == 0)
        return isCorrect;

    var exp = /^[0-9]+$/;

    if (typeZone == "string") {
        var longueurEntier = parseInt(longueurZone);

        if (longueurEntier < longueurValDefaut) {
            $("#valeurDefaut_" + idLigne).addClass('requiredField');
            $("#valeurDefaut_" + idLigne).attr('title', "Valeur trop longue, veuillez saisir une valeur entre 1 et " + longueurEntier + " caractéres.");
            isCorrect = false;
        }
    }
    else if (typeZone == "Int64") {
        if (exp.test(valeurDefaut)) {
            var longueurEntier = parseInt(longueurZone);
            var val = parseInt(valeurDefaut);

            if (isNaN(val) || longueurEntier < longueurValDefaut || valeurDefaut.split('.').length != 1) {
                isCorrect = false;
            }
        }
        else
            isCorrect = false;

        if (!isCorrect) {
            $("#valeurDefaut_" + idLigne).addClass('requiredField');
            $("#valeurDefaut_" + idLigne).attr('title', "Valeur incorrecte, veuillez saisir un nombre entier de " + longueurEntier + " Caractéres maximum.");
        }
    }
    else if (typeZone == "Double") {
        exp = /^[0-9]+\.[0-9]+$/;

        var longueurEntier = parseInt(longueurZone.split(':')[0]);
        var longueurDecimal = parseInt(longueurZone.split(':')[1]);

        if (exp.test(valeurDefaut)) {

           

                
            var valEntier = valeurDefaut.split('.')[0];
            var valDecimal = valeurDefaut.split('.')[1]; 

            if (longueurEntier < valEntier.length || longueurDecimal < valDecimal.length) {
                isCorrect = false;
            }
            
        }
        else
            isCorrect = false;

        if (!isCorrect) {
            $("#valeurDefaut_" + idLigne).addClass('requiredField');
            $("#valeurDefaut_" + idLigne).attr('title', "Valeur incorrecte, veuillez saisir un nombre décimal avec une partie entiére de " + longueurEntier + " Caractéres maximum et une partie décimale de " + longueurDecimal + " Caractéres maximum.");
        }
    }
    return isCorrect;
}

//----------------Nettoie la ligne d'insertion--------------
function ClearEmptyLine(code) {
    $("#referenceDescription_" + code).val('');
    $("#libelle_" + code).val('');
    $("#typeZone_" + code).val('');
    $("#longueurZone_" + code).val('');
    $("#drlMappage_" + code).val('');
    $("#drlConversion_" + code).val('');
    $("#drlPresentation_" + code).val('');
    $("#drlTypeUI_" + code).val('');
    $("#drlObligatoire_" + code).val('');
    $("#drlAffichage_" + code).val('');
    $("#drlControle_" + code).val('');
    $("#observation_" + code).val('');
}