/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementPage();
});

//----------------Map les éléments de la page------------------
function MapElementPage() {

    if ($("#modeAssociation").val() == 'True') {
        $("#ajouterLigneModele").die().live('click', function () {
            if ($(this).hasClass("CursorPointer")) {
                SwitchModeLigne($("#idLigneVide").val());
                $("#divLignesModeleBodyEmptyLine").show();
                $("img[id=ajouterModele]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
                $("img[id=ajouterLigneModele]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
            }
        });
        MapElementTableau();

        $("#btnOKVisualiser").die().live('click', function () {
            $("#divFancyVisualiser").hide();
        });
    }

    $("#ajouterModele").kclick(function () {
        if ($(this).hasClass("CursorPointer")) {
            $("#codeModele").val("");
            $("img[id=ajouterModele]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
            $("img[id=ajouterLigneModele]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
            if ($("#modeAssociation").val() == 'False') {
                $("#drlModeles").val('');
                ClearDetailsModele();
                $("#nomModele").removeAttr('readonly').removeClass('readonly');
                $("#descriptionModele").removeAttr('readonly').removeClass('readonly');
                $("#drlModeles").attr('disabled', 'disabled');
                $("#btnAnnulerModele").removeAttr('disabled');
                $("#btnSupprimerModele").removeAttr('disabled');
                $("#btnSaveModele").removeAttr('disabled');
                $("#btnSupprimerModele").attr('disabled', 'disabled');

                if ($("#divDetailsEnteteTitle").attr('Title') == "Agrandir") {
                    CollapseExpandMenu("divDetailsEnteteTitle");
                }
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/ParamISModeleAssocier/OuvrirAjoutModele/",
                    success: function (data) {
                        AlbScrollTop();
                        $("#divDataEditPopIn").html(data);
                        MapElementDivAjout();
                        $("#divFullScreen").show();
                        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });
            }
        }
    });

    $("#drlModeles").die().live('change', function () {
        LoadDetailsModele($(this).val());
    });

    $("#btnSaveModele").die().live('click', function () {
        SaveModele();
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                $("#actionParameter").val('');
                break;
            case "Delete":
                SaveLineModele();
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


    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });

    $("#btnSupprimerModele").die().live('click', function () {
        LoadPopupSupprModele($("#drlModeles").val());
        $("#divFullScreenSuppr").show();
    });

    $("#btnAnnulerModele").die().live('click', function () {
        if ($("#drlModeles").val() != "")
            LoadDetailsModele($("#drlModeles").val());
        else {
            ClearDetailsModele();
            $("img[id=ajouterLigneModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
            $("img[id=ajouterModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
            $("#drlModeles").removeAttr('disabled', 'disabled');
            $("#btnAnnulerModele").attr('disabled', 'disabled');
            $("#btnSaveModele").attr('disabled', 'disabled');
            $("#btnSupprimerModele").attr('disabled', 'disabled');
        }
    });

    $("#btnVisualiser").die().live('click', function () {
        VisualiserLigneInfo();
    });

    $("#btnInitCache").die().live("click", function () {
        InitialiserCacheIS();
    });

    toggleDescription();
    FormatElementsPage();
}

//----------------Map les éléments du tableau de lignes modèle
function MapElementTableau() {
    if ($("#modeAssociation").val() == 'True') {

        //$("input[name=drlReferentiel]").die();
        //$("input[name=drlReferentiel]").live('click', function () {
        //    var id = $(this).attr('id').split('_')[1];
        //    ChargementSelecteurReferentiel(id);
        //});

        $("input[name=lienParent]").die().live('click', function () {
            var id = $(this).attr('id').split('_')[1];
            ChargementSelecteurParent(id);
        });


        $("td[name=selectableCol]").die().live('click', function () {
            var id = $(this).attr('id').split('_')[2];
            if (id != undefined && id != "") {
                var selectedRow = $("#selectedRow").val();
                if (selectedRow == undefined || selectedRow == "" || selectedRow != id)
                    SwitchModeLigne(id);
            }
        });

        $("img[name=saveLigneModele]").die().live('click', function () {
            SaveLineModele($(this).attr('id').split('_')[1]);
        });

        $("img[name=cancelLigneModele]").kclick(function () {
            AfficherLigne("readonly", $(this).attr('id').split('_')[1]);
        });

        $("img[name=deleteModele]").die().live('click', function () {
            $("#actionParameter").val($(this).attr('id').split('_')[1]);
            ShowCommonFancy("Confirm", "Delete", "Etes-vous sûr de vouloir supprimer cette période?", 320, 150, true, true);
        });

        $("img[name=infoReferentiel]").bind('mouseover', function () {
            var id = $(this).attr('id').split('_')[1];
            var position = $(this).offset();
            // var divWidth = $("#divInfoBandeau_" + id).width();           
            ChargementBandeauReferentiel(id);
            $("#divInfoBandeau").css({ 'position': 'absolute', 'top': position.top + 25 + 'px', 'left': position.left - 20 + 'px' }).show();
        });


        $("img[name=infoReferentiel]").bind('mouseout', function () {
            var id = $(this).attr('id').split('_')[1];
            $("#divInfoBandeau").html('');
            $("#divInfoBandeau").hide();
        });

        AlternanceLigne("LignesModeleBody", "noInput", true, null);
        AlternanceLigne("LignesModeleBodyEmptyLine", "noInput", true, null);
        FormatElementsTableau();

    }
}

//---------------Map les éléments de la div flottante ajout de modèle----
function MapElementDivAjout() {
    $("#btnAnnulerAjout").unbind();
    $("#btnAnnulerAjout").bind('click', function () {
        $("#divDataEditPopIn").html('');
        $("#divFullScreen").hide();
        $("img[id=ajouterLigneModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
        $("img[id=ajouterModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
        $("div[name=txtAreaLnk]").unbind('click');
        toggleDescription();
    });

    $("#btnEnregistrerAjout").unbind();
    $("#btnEnregistrerAjout").bind('click', function () {
        SaveNouveauModele();
    });

    toggleDescription();
}

//-----------------Map les éléments de la div flottante suppression de modèle-----
function MapElementDivSuppr() {
    $("#btnAnnulerSuppressionModele").unbind();
    $("#btnAnnulerSuppressionModele").bind('click', function () {
        $("#divDataConfirmSuppr").html('');
        $("#divFullScreenSuppr").hide();
    });

    $("#btnConfirmerSuppressionModele").unbind();
    $("#btnConfirmerSuppressionModele").bind('click', function () {
        SupprimerModele();
    });

    $("#tblLignesModeleBodySuppr td[class=col_Action]").hide();
    AlternanceLigne("LignesModeleBodySuppr", "noInput", false, null);
    AlternanceLigne("LignesModeleBodyEmptyLine", "noInput", true, null);
}

//----------------Fonction qui formate les champs du modèle---------
function FormatElementsPage() {
    $("#nomModele").attr("maxlength", 40);
    $("#descriptionModele").attr("maxlength", 60);
    AffectDateFormat();
}

//---------------Fonction qui formate les champs de chaqie ligne modèle-------
function FormatElementsTableau() {
    //Numerique();
    FormatDecimalValue();
    $("input[name=libelle]").each(function () {
        $(this).attr("maxlength", 40);
    });
    $("input[name=sql]").each(function () {
        $(this).attr("maxlength", 5000);
    });
    $("input[name=parentComportement]").each(function () {
        $(this).attr("maxlength", 5000);
    });
    $("input[name=parentEvenement]").each(function () {
        $(this).attr("maxlength", 5000);
    });
}

//----------------Charge les détails du modèle sélectionné--------
function LoadDetailsModele(modeleName) {
    var modeAssociation = $("#modeAssociation").val();
    if (modeleName != undefined && modeleName != "") {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamISModeleAssocier/GetDetailsModele/",
            data: {
                modeleName: modeleName, modeAssociation: modeAssociation
            },
            success: function (data) {
                //$("#divConteneurModele").html(data);
                $("#divConteneurModele").replaceWith("<div id=\"divConteneurModele\">" + data + "</div>");
                $("img[id=ajouterLigneModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
                $("img[id=ajouterModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
                $("#btnAnnulerModele").removeAttr('disabled');
                $("#btnSupprimerModele").removeAttr('disabled');
                $("#btnSaveModele").removeAttr('disabled');
                MapElementPage();
                if (modeAssociation == 'True') {
                    CollapseExpandMenu("divLstISLignesModeleTitle");
                }
                else {
                    CollapseExpandMenu("divDetailsEnteteTitle");
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//----------------Charge la popup de confirmation de suppression de modèle
function LoadPopupSupprModele(modeleName) {
    if (modeleName != undefined && modeleName != "") {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamISModeles/LoadPopupSupprModele/",
            data: {
                modeleName: modeleName
            },
            success: function (data) {
                //$("#divDataConfirmSuppr").html(data);
                $("#divDataConfirmSuppr").replaceWith("<div id=\"divDataConfirmSuppr\">" + data + "</div>");
                $("#divDataConfirmSuppr").find("table#tblLignesModeleBody").attr("id", "tblLignesModeleBodySuppr");
                MapElementDivSuppr();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

}

//----------------Prérempli la ligne grâce au référentiel sélectionné-------
function LoadReferentiel(codeLigne, codeReferentiel) {
    if (codeReferentiel != undefined && codeReferentiel != "") {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamISModeleAssocier/PreRemplirLigneAvecReferentiel/",
            data: {
                codeLigne: codeLigne, codeReferentiel: codeReferentiel
            },
            success: function (data) {
                $("#ligneModeleIS_" + codeLigne).html(data);
                $("div[name=colReadOnly_" + codeLigne + "]").hide();
                $("div[name=colEdition_" + codeLigne + "]").show();
                MapElementTableau();
                MapCommonAutoCompReferentielIS();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//----------------Sauvegarde l'entete du modèle-----------
function SaveModele() {
    var typeOperation = "Insert";
    if ($("#codeModele").val() != "")
        typeOperation = "Update";

    var nomModele = $("#nomModele").val();
    var descriptionModele = $("#descriptionModele").val();
    var datedebut = $("#dateDebutModele").val();
    var datefin = $("#dateFinModele").val();

    var isCorrect = true;
    //Vérification des valeurs
    if (nomModele == undefined || nomModele == "") {
        $("#nomModele").addClass('requiredField');
        isCorrect = false;
    }
    if (descriptionModele == undefined || descriptionModele == "") {
        $("#descriptionModele").addClass('requiredField');
        isCorrect = false;
    }
    if (datedebut == undefined || datedebut == "") {
        $("#dateDebutModele").addClass('requiredField');
        isCorrect = false;
    }

    if (datefin == undefined || datefin == "") {
        if (!isDate(datedebut)) {
            $("#dateDebutModele").addClass('requiredField');
            isCorrect = false;
        }
    } else {
        if (!checkDate($("#dateDebutModele"), $("#dateFinModele"))) {
            $("#dateDebutModele").addClass('requiredField');
            $("#dateFinModele").addClass('requiredField');
            isCorrect = false;
        }
    }


    if (isCorrect) {
        $("#nomModele").removeClass('requiredField');
        $("#descriptionModele").removeClass('requiredField');
        $("#dateDebutModele").removeClass('requiredField');
        $("#dateFinModele").removeClass('requiredField');

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamISModeleAssocier/SaveModele/",
            data: {
                nomModele: nomModele, descriptionModele: descriptionModele, datedebut: datedebut, datefin: datefin,
                typeOperation: typeOperation
            },
            success: function (data) {
                $("#divListeModeles").html(data);
                CollapseExpandMenu("divDetailsEnteteTitle");
                ClearDetailsModele();
                $("#drlModeles").removeAttr('disabled');
                if ($("#newReferenceIS").is(":visible"))
                    $("#newReferenceIS").hide();

                $("#divLignesModeleBodyLines").remove();
                $("img[id=ajouterLigneModele]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
                $("img[id=ajouterModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
                MapElementPage();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

}

//----------------Sauvegarde un nouveau modèle en mode popup-----------
function SaveNouveauModele() {
    var typeOperation = "Insert";

    var nomModele = $("#nomModeleAjout").val();
    var descriptionModele = $("#descriptionModeleAjout").val();
    var datedebut = $("#dateDebutModeleAjout").val();
    var datefin = $("#dateFinModeleAjout").val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamISModeleAssocier/SaveModele/",
        data: {
            nomModele: nomModele, descriptionModele: descriptionModele, datedebut: datedebut, datefin: datefin,
            typeOperation: typeOperation
        },
        success: function (data) {
            $("#divListeModeles").html(data);
            CollapseExpandMenu("divDetailsEnteteTitle");
            $("img[id=ajouterLigneModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
            $("img[id=ajouterModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
            $("#divDataEditPopIn").html('');
            $("#divFullScreen").hide();
            toggleDescription($("#RequeteSelect"));
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });


}

//----------------Sauvegarde une reference--------------------
function SaveLineModele(code) {
    var typeOperation = "Insert";
    if (code != -9999)
        typeOperation = "Update";

    if (code == undefined) {
        code = $("#actionParameter").val();
        typeOperation = "Delete";
    }

    $("#numAff_" + code).removeClass('requiredField');
    $("#sautLigne_" + code).removeClass('requiredField');

    var referentiel = $("#idReferentiel_" + code).val();
    var libelle = $("#libelle_" + code).val();
    var numOrdreAff = $("#numAff_" + code).val();
    var sautLigne = $("#sautLigne_" + code).val();
    var modifiable = $("#drlModifiable_" + code).val();
    var obligatoire = $("#drlObligatoire_" + code).val();
    var tcon = $("#tcon_" + code).val();
    var tfam = $("#tfam_" + code).val();
    var presentation = $("#drlPresentation_" + code).val();
    var affichage = $("#drlAffichage_" + code).val();
    var controle = $("#drlControle_" + code).val();
    var lienParent = $("#lienParent_" + code).val();
    var parentComportement = $("#parentComportement_" + code).val();
    var parentEvenement = $("#parentEvenement_" + code).val();
    var modeleIS = $("#drlModeles").val();

    var isCorrect = true;

    if (typeOperation != "Delete") {
        //Vérification des champs avant insertion
        if (!CheckNumOrdreAff(code)) {
            $("#numAff_" + code).addClass('requiredField');
            isCorrect = false;
        }
        if (sautLigne < 1) {
            $("#sautLigne_" + code).addClass('requiredField');
            isCorrect = false;
        }

        var typeUi = $("#typeUI_" + code).val();
        

        if (lienParent == undefined || lienParent == "") {
            lienParent = 0;
        }

        if (referentiel == undefined || referentiel == "") {
            if (presentation == 3) {
                $("#drlPresentation_" + code).addClass('requiredField');
                isCorrect = false;
            }
        }

        if (presentation == undefined || presentation == "") {
            $("#drlPresentation_" + code).addClass('requiredField');
            isCorrect = false;
        }
        else {
            if ((presentation == 3) && (referentiel == "")) {
                $("#drlReferentiel_" + code).addClass('requiredField');
                isCorrect = false;
            }
        }
    }
    else if (numOrdreAff == undefined || sautLigne == undefined || presentation == undefined || lienParent == undefined) {
        numOrdreAff = 0;
        sautLigne = 0;
        presentation = 0;
        lienParent = 0;
    }
    if (isCorrect) {


        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamISModeleAssocier/SaveLineModele/",
            data: {
                code: code, modeleIS: modeleIS, referentiel: referentiel, libelle: libelle, numOrdreAff: numOrdreAff, sautLigne: sautLigne,
                modifiable: modifiable, obligatoire: obligatoire, tcon: tcon, tfam: tfam, presentation: presentation,
                affichage: affichage, controle: controle, lienParent: lienParent, parentComportement: parentComportement,
                parentEvenement: parentEvenement, typeOperation: typeOperation
            },
            success: function (data) {
                //$("#divLignesModeleBodyLines").html(data);
                $("#divLignesModeleBodyLines").replaceWith("<div class=\"floatLeft\" id=\"divLignesModeleBodyLines\">" + data + "</div>");
                if (typeOperation == "Insert")
                    $("#divLignesModeleBodyEmptyLine").hide();
                $("img[id=ajouterLigneModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
                $("img[id=ajouterModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
                $("#selectedRow").val('');
                //$("div[name=txtAreaLnk]").unbind('click');
                //MapElementPage();
                MapElementTableau();
                CloseLoading();

            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//----------------Supprime un modèle et toutes ses lignes modeles----
function SupprimerModele() {
    var modeleName = $("#idModeleSuppr").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamISModeles/SupprimerModele/",
        data: {
            modeleName: modeleName
        },
        success: function (data) {
            $("#divListeModeles").html(data);
            ClearDetailsModele();
            $("#divDataConfirmSuppr").html('');
            $("#divFullScreenSuppr").hide();

            $("#divLignesModeleBodyLines").remove();
            $("img[id=ajouterLigneModele]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
            MapElementPage();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//----------------Charge la div flottante contenant les détails du réferentiel de la ligne en survol------
function ChargementBandeauReferentiel(id) {
    var referentielId = $("#idReferentiel_" + id).val();
    $.ajax({
        type: "POST",
        url: "/ParamISModeleAssocier/GetDetailsReferentiel/",
        data: {
            codeReferentiel: referentielId
        },
        success: function (data) {
            $("#divInfoBandeau").html(data);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

//----------------Charge la div flottante contenant la liste des réferentiels-----------------
function ChargementSelecteurReferentiel(id) {
    var referentielId = $("#drlReferentiel_" + id).val();
    $.ajax({
        type: "POST",
        url: "/ParamISModeleAssocier/GetSelecteurReferentiel/",
        data: {
            ligneId: id, selectedReferentiel: referentielId
        },
        success: function (data) {
            $("#divSelectPopup").html(data);
            $("#modeSelect").val("referentiel");
            $("#selectedRow").val(id);
            $("#oldSelection").val(referentielId);
            $("#btnValiderSelectPopup").bind('click', function () {
                var selectedRadio = $('input[type=radio][name=rdOption]:checked').val();
                if ($("#oldSelection").val() != selectedRadio) {
                    $("#drlReferentiel_" + $("#selectedRow").val()).val(selectedRadio);
                    LoadReferentiel($("#selectedRow").val(), selectedRadio);
                }
                $("#divSelectPopup").html('');
                $("#divFullScreenSelector").hide();
            });
            $("#divFullScreenSelector").show();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//----------------Charge la div flottante contenant la liste des réferentiels-----------------
function ChargementSelecteurParent(id) {
    var selectedParent = $("#lienParent_" + id).val();
    var modeleId = $("#drlModeles").val();

    $.ajax({
        type: "POST",
        url: "/ParamISModeleAssocier/GetSelecteurParent/",
        data: {
            modeleId: modeleId, selectedParent: selectedParent
        },
        success: function (data) {
            $("#divSelectPopup").html(data);
            $("#modeSelect").val("parent");
            $("#selectedRow").val(id);
            $("#oldSelection").val(selectedParent);
            $("#btnValiderSelectPopup").bind('click', function () {
                var selectedRadio = $('input[type=radio][name=rdOption]:checked');
                if ($("#oldSelection").val() != selectedRadio) {
                    $("#lienParent_" + $("#selectedRow").val()).val(selectedRadio.val());
                    $("#lienParentLib_" + $("#selectedRow").val()).val(selectedRadio.attr('title'));
                }
                $("#divSelectPopup").html('');
                $("#divFullScreenSelector").hide();
            });
            $("#divFullScreenSelector").show();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function VisualiserLigneInfo() {
    var modeleId = $("#drlModeles").val();
    if (modeleId != "") {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamISModeleAssocier/VisualiserLigneInfo/",
            data: {
                modeleId: modeleId
            },
            success: function (data) {
                CloseLoading();
                $("#divFancyVisualiserData").replaceWith("<div id=\"divFancyVisualiserData\">" + data + "</div>");
                AlbScrollTop();
                $("#divFancyVisualiser").show();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

}

//----------fonction qui reinitialise le cache des IS--------------
function InitialiserCacheIS() {
    common.page.isLoading = true;
    common.$postJson("/ParamISModeleAssocier/InitialiserCacheIS", null, true).done(function () {
        common.dialog.bigInfo("Le cache a été réinitialisé", true);
    });
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
//----------------Nettoie les champs de détails modèle-----
function ClearDetailsModele() {
    $("#nomModele").val('');
    $("#descriptionModele").val('');
    $("#dateDebutModele").val('');
    $("#dateFinModele").val('');
    

    $("#nomModele").removeClass('requiredField');
    $("#descriptionModele").removeClass('requiredField');
    $("#dateDebutModele").removeClass('requiredField');
    $("#dateFinModele").removeClass('requiredField');
    


}
//----------------Vérifie les doublons du champ numero ordre affichage------
function CheckNumOrdreAff(code) {
    var numOrdreAff = $("#numAff_" + code).val();
    var fnumOrdreAff = parseFloat(numOrdreAff.replace(",", "."));
    var isCorrect = true;

    $('input[name=numAffichage]').each(function () {
        if (($(this).attr('id') != "numAff_" + code) && ($(this).attr('id') != "numAff_-9999")) {
            var currentVal = $(this).val();
            if (currentVal == numOrdreAff)
                isCorrect = false;
        }
    });

    if ((fnumOrdreAff < 1) || (numOrdreAff == ""))
        isCorrect = false;

    return isCorrect;
}
//-------Formate les input/span des valeurs----------
function FormatDecimalValue() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '0.00');
    //FormatDecimal('decimal', ' ', 2, '99999999999.99', '0.00');
}
//---------------fonction qui permet de passer du mode readonly au mode edition pour une ligne donnée----------
function SwitchModeLigne(idLigne) {
    var newLineDisplayed = false;
    if (idLigne != $("#idLigneVide").val()) {
        $("#divLignesModeleBodyEmptyLine").hide();
        $("img[id=ajouterLigneModele]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
    }
    else
        $("#divLignesModeleBodyEmptyLine").show();

    if ($("#selectedRow").val() != "") {
        if (idLigne != $("#selectedRow").val()) {
            //Affiche l'ancienne ligne editée en mode readonly
            AfficherLigne("readonly", $("#selectedRow").val(), idLigne);
        }
    }
    else
        AfficherLigne("edition", idLigne);

    $("#selectedRow").val(idLigne);
    //MapElementTableau();
}
//----------------Affiche la ligne dans le mode donné (readonly/edition)--------
function AfficherLigne(mode, idLigne, idLigneToDisplayNext) {

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamISModeleAssocier/AfficherLigne/",
        data: {
            mode: mode, code: idLigne
        },
        success: function (data) {
            $("#ligneModeleIS_" + idLigne).html(data);
            if (idLigneToDisplayNext != undefined && idLigneToDisplayNext != "" && mode == "readonly")
                AfficherLigne("edition", idLigneToDisplayNext);
            else
                CloseLoading();
            MapCommonAutoCompReferentielIS();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}