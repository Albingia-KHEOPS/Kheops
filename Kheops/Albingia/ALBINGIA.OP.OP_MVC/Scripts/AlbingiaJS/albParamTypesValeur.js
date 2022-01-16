/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementsPage();
    RechercherTypesValeur();
});


//-------------Map les éléments de la page-------------
function MapElementsPage() {
    $("#btnRechercher").kclick(function () {
        RechercherTypesValeur();
    });

    $("#btnAjouterTypeValeur").kclick(function () {
        AfficherDetailsTypeValeur("");
    });

    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "SupprimerTypeValeur":
                var typeValeur = $("#idTypeValeurSuppr").val();
                SupprimerTypeValeur(typeValeur);
                break;
            case "SupprimerTypeValComp":
                var typeValeurComp = $("#idTypeValeurCompSuppr").val();
                SupprimerTypeValeurCompatible(typeValeurComp);
                break;
        }
        $("#hiddenAction").val('');
        $("#idTypeValeurSuppr").val('');
        $("#idTypeValeurCompSuppr").val('');
    });

    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#idTypeValeurSuppr").val('');
        $("#idTypeValeurCompSuppr").val('');
    });

    $('#btnAnnuler').kclick(function (evt) {
        window.location.href = "/BackOffice/Index";
    });

    $("#RechercheCode").offOn("change", function () {
        Initialiser();
    });
    $("#RechercheDescription").offOn("change", function () {
        Initialiser();
    });

    MapCommonAutoCompTypeValeur();
}

//------------Map les lignes du tableau de type valeur (principal)--------
function MapElementsTableauTypesValeur() {
    $("tr[name=ligneTypeValeur]").kclick(function () {
        var id = $(this).attr("id").split("_")[1];
        AfficherDetailsTypeValeur(id);
    });

    $("img[name=btnSupprimerTypeValeur]").kclick(function () {
        var id = $(this).attr("id").split("_")[1];
        $("#idTypeValeurSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerTypeValeur", "Vous allez supprimer le type valeur.<br/>Etes-vous sûr(e) de vouloir continuer ?",
                  320, 150, true, true);
    });

    AlternanceLigne("TypesValeurBody", "noInput", true, null);

}

//------------Map les éléments de la partie détails du type valeur----------
function MapElementsDetails() {
    $("#btnEnregistrer").kclick(function () {
        EnregistrerTypeValeur();
    });

    $("#btnAjouterTypeValeurComp").kclick(function () {
        if ($("#modeSaisie").val() == "Insert")
            $("#divbtnEnregistrer").addClass('requiredButton');
        else {
            if ($("#selectedRowTypeValComp").val() != "") {
                $("div[name=divReadOnly_" + $("#selectedRowTypeValComp").val() + "]").show();
                $("div[name=divEdition_" + $("#selectedRowTypeValComp").val() + "]").hide();
            }
            $("#divTypeValeurCompLigneVide").show();
        }
    });
    MapElementsTableauTypesValeurCompatibles();
}

//------------Map les lignes du tableau types valeur compatibles
function MapElementsTableauTypesValeurCompatibles() {
    $("td[albClikable=selectableColTypeComp]").kclick(function () {
        var id = $(this).attr('name').split('_')[1];
        if ($("#selectedRowTypeValComp").val() != "") {
            if (id != $("#selectedRowTypeValComp").val()) {
                $("div[name=divReadOnly_" + $("#selectedRowTypeValComp").val() + "]").show();
                $("div[name=divEdition_" + $("#selectedRowTypeValComp").val() + "]").hide();
            }
        }

        $("#selectedRowTypeValComp").val(id);
        $("div[name=divReadOnly_" + id + "]").hide();
        $("div[name=divEdition_" + id + "]").show();
        $("#divTypeValeurCompLigneVide").hide();
    });

    $("img[name=btnSupprimerTypeValeurComp]").kclick(function () {
        var id = $(this).attr("id").split("_")[1];
        $("#idTypeValeurCompSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerTypeValComp", "Vous allez supprimer la ligne sélectionnée.<br/>Etes-vous sûr(e) de vouloir continuer ?",
                  320, 150, true, true);
    });

    $("img[name=btnEnregistrerTypeValComp]").kclick(function () {
        var id = $(this).attr("id").split("_")[1];
        EnregistrerTypeValeurCompatible(id);
    });

    AlternanceLigne("TypeValeurCompBody", "noInput", true, null);    
}

//-----------Lance la recherche des types valeur------------
function RechercherTypesValeur() {
    var codeRecherche = $.trim($("#RechercheCode").val());
    var descrRecherche = $.trim($("#RechercheDescription").val());
    var userRights = $("#AdditionalParam").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamTypesValeur/RechercheTypesValeur",
        data: { codeTypeValeur: codeRecherche, descriptionTypeValeur: descrRecherche, userRights: userRights },
        success: function (data) {
            $("#divTypesValeurBody").html(data);
            MapElementsTableauTypesValeur();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

//-------------fonction qui récupère les détails du type valeur (nouveau ou sélectionné)--------
function AfficherDetailsTypeValeur(idLigne) {
    var userRights = $("#AdditionalParam").val();
    if (idLigne != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamTypesValeur/GetDetailsTypeValeur",
            data: { codeTypeValeur: idLigne, userRights: userRights },
            success: function (data) {
                $("#divTypesValeurDetails").html(data);
                MapElementsDetails();
                $("#VoletDetails").show();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//------------fonction qui enregistre les types valeur (nouveaux ou modifiés)
function EnregistrerTypeValeur() {
    var modeSaisie = $("#modeSaisie").val();
    var codeEdit = $.trim($("#codeTypeValeurEdit").val());
    var descrEdit = $.trim($("#descriptionTypeValeurEdit").val());
    var userRights = $("#AdditionalParam").val();

    var verif = true;
    if (codeEdit == "" || codeEdit == undefined) {
        $("#codeTypeValeurEdit").addClass("requiredField");
        verif = false;
    }
    if (descrEdit == "" || descrEdit == undefined) {
        $("#descriptionTypeValeurEdit").addClass("requiredField");
        verif = false;
    }

    if (verif) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamTypesValeur/EnregistrerTypeValeur",
            data: { mode: modeSaisie, codeTypeValeur: codeEdit, descriptionTypeValeur: descrEdit, userRights: userRights },
            success: function (data) {
                $("#divTypesValeurBody").html(data);
                MapElementsTableauTypesValeur();
                $("#VoletDetails").hide();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

function EnregistrerTypeValeurCompatible(idLigne) {
    var selectedCodeTypeValComp = $("#drlType_" + idLigne).val();
    var codeTypeValEdit = $("#codeTypeValeurEdit").val();
    var userRights = $("#AdditionalParam").val();


    var verif = true;
    if (selectedCodeTypeValComp == "" || selectedCodeTypeValComp == undefined) {
        $("#drlType_" + idLigne).addClass("requiredField");
        verif = false;
    }

    if (verif) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamTypesValeur/EnregistrerTypeValeurComp",
            data: { codeTypeValeur: codeTypeValEdit, idTypeValeurComp: idLigne, CodeTypeValeurComp: selectedCodeTypeValComp, userRights: userRights },
            success: function (data) {
                $("#divTypesValeurDetails").html(data);
                MapElementsDetails();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

}

//-------------fonction qui supprime le type valeur selectionné
function SupprimerTypeValeur(idLigne) {
    var userRights = $("#AdditionalParam").val();
    if (idLigne != "" && idLigne != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamTypesValeur/SupprimerTypeValeur",
            data: { idTypeValeur: idLigne, userRights: userRights },
            success: function (data) {
                $("#divTypesValeurBody").html(data);
                MapElementsTableauTypesValeur();
                $("#VoletDetails").hide();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}


//-------------fonction qui supprime le type valeur compatible selectionné
function SupprimerTypeValeurCompatible(idLigne) {
    var userRights = $("#AdditionalParam").val();
    var idTypeValeur = $("#codeTypeValeurEdit").val();
    if (idLigne != "" && idLigne != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamTypesValeur/SupprimerTypeValeurComp",
            data: { idTypeValeur: idTypeValeur, idTypeValeurComp: idLigne, userRights: userRights },
            success: function (data) {
                $("#divTypesValeurDetails").html(data);
                MapElementsDetails();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//------- Vide les listes de recherche-----
function Initialiser() {
    $("#divTypesValeurBody").html('');
    $("#divTypesValeurDetails").html('');
}