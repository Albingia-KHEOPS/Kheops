/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementsPage();
    RechercherFiltres();
});

//-------------Map les éléments de la page-------------
function MapElementsPage() {
    $("#btnRechercher").unbind();
    $("#btnRechercher").bind("click", function () {
        RechercherFiltres();
    });

    $("#btnAjouterFiltre").unbind();
    $("#btnAjouterFiltre").bind("click", function () {
        AfficherDetailsFiltre("");
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "SupprimerFiltre":
                var filtre = $("#idFiltreSuppr").val();
                SupprimerFiltre(filtre);
                break;
            case "SupprimerPaireBrancheCible":
                var paire = $("#idPaireSuppr").val();
                SupprimerPaireBrancheCible(paire);
                break;
        }
        $("#hiddenAction").val('');
        $("#idFiltreSuppr").val('');
        $("#idPaireSuppr").val('');
    });

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#idFiltreSuppr").val('');
        $("#idPaireSuppr").val('');
    });

    $('#btnAnnuler').unbind();
    $('#btnAnnuler').bind('click', function (evt) {
        Annuler();
    });
    $("#RechercheCode").unbind();
    $("#RechercheCode").bind('change', function () {
        Initialiser();
    });
    $("#RechercheDescription").unbind();
    $("#RechercheDescription").bind('change', function () {
        Initialiser();
    });
    MapCommonAutoCompFiltres();
}

//------------Map les lignes du tableau de filtres--------
function MapElementsTableauFiltres() {
    $("tr[name=ligneFiltre]").unbind();
    $("tr[name=ligneFiltre]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        AfficherDetailsFiltre(id);
    });

    $("img[name=btnSupprimerFiltre]").unbind();
    $("img[name=btnSupprimerFiltre]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $("#idFiltreSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerFiltre", "Vous allez supprimer le filtre.<br/>Etes-vous sûr(e) de vouloir continuer ?",
                  320, 150, true, true);
    });

    AlternanceLigne("FiltresBody", "noInput", true, null);

}

//------------Map les lignes du tableau de paires branche cible--------
function MapElementsTableauPairesBrancheCible() {
    $("td[albClikable=selectableColPaire]").die().live('click', function () {
        var id = $(this).attr('name').split('_')[2];
        if ($("#selectedRowPaire").val() != "") {
            if (id != $("#selectedRowPaire").val()) {
                $("div[name=divReadOnly_" + $("#selectedRowPaire").val() + "]").show();
                $("div[name=divEdition_" + $("#selectedRowPaire").val() + "]").hide();
            }
        }

        $("#selectedRowPaire").val(id);
        $("div[name=divReadOnly_" + id + "]").hide();
        $("div[name=divEdition_" + id + "]").show();
        $("#divPairesBrancheCibleLigneVide").hide();
    });

    $("img[name=btnSupprimerPaireBrancheCible]").unbind();
    $("img[name=btnSupprimerPaireBrancheCible]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $("#idPaireSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerPaireBrancheCible", "Vous allez supprimer la ligne sélectionnée.<br/>Etes-vous sûr(e) de vouloir continuer ?",
                  320, 150, true, true);
    });

    $("img[name=btnEnregistrerPaireBrancheCible]").unbind();
    $("img[name=btnEnregistrerPaireBrancheCible]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        EnregistrerPaireBrancheCible(id);
    });

    $("select[name=dvBranche]").each(function () {
        $(this).change(function () {
            var id = $(this).attr("id").split("_")[1];
            GetCibles(id);
        })
    });

    AlternanceLigne("PairesBrancheCibleBody", "noInput", true, null);
}

//------------Map les éléments de la partie détails du filtre----------
function MapElementsDetails() {
    $("#btnEnregistrer").unbind();
    $("#btnEnregistrer").bind("click", function () {
        EnregistrerFiltre();
    });

    $("#btnAjouterPaireBrancheCible").unbind();
    $("#btnAjouterPaireBrancheCible").bind("click", function () {
        if ($("#modeSaisie").val() == "Insert")
            $("#divbtnEnregistrer").addClass('requiredButton');
        else {
            if ($("#selectedRowPaire").val() != "") {
                $("div[name=divReadOnly_" + $("#selectedRowPaire").val() + "]").show();
                $("div[name=divEdition_" + $("#selectedRowPaire").val() + "]").hide();
            }
            $("#divPairesBrancheCibleLigneVide").show();
        }
    });

    $("#btnCheckFiltre").unbind();
    $("#btnCheckFiltre").bind("click", function () {
        VerifierFiltre();
    });

    $("#btnInfoOk").die().live('click', function () {
        CloseCommonFancy();
    });

    MapElementsTableauPairesBrancheCible();

}


//-----------Lance la recherche des filtres------------
function RechercherFiltres() {
    var codeRecherche = $.trim($("#RechercheCode").val());
    var descrRecherche = $.trim($("#RechercheDescription").val());
    var userRights = $("#AdditionalParam").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamFiltres/RechercheFiltres",
        data: { codeFiltre: codeRecherche, descriptionFiltre: descrRecherche, userRights: userRights },
        success: function (data) {
            $("#divFiltresBody").html(data);
            MapElementsTableauFiltres();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-------------fonction qui récupère les détails du filtre (nouveau ou sélectionné)--------
function AfficherDetailsFiltre(idLigne, codeFiltre) {
    var userRights = $("#AdditionalParam").val();
    if (idLigne == undefined)
        idLigne = "";
    if (codeFiltre == undefined)
        codeFiltre = "";

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamFiltres/GetDetailsFiltre",
        data: { idFiltre: idLigne, codeFiltre: codeFiltre, userRights: userRights },
        success: function (data) {
            $("#divFiltreDetails").html(data);
            MapElementsDetails();
            $("#VoletDetails").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

//-------------fonction qui supprime le filtre selectionné
function SupprimerFiltre(idLigne) {
    var userRights = $("#AdditionalParam").val();
    if (idLigne != "" && idLigne != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamFiltres/SupprimerFiltre",
            data: { idFiltre: idLigne, userRights: userRights },
            success: function (data) {
                $("#divFiltresBody").html(data);
                MapElementsTableauFiltres();
                $("#VoletDetails").hide();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//-------------fonction qui supprime la combinaison Branche/Cible sélectionnée-----
function SupprimerPaireBrancheCible(idLigne) {
    var userRights = $("#AdditionalParam").val();
    var idFiltre = $("#filtreEditId").val();
    if (idLigne != "" && idLigne != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamFiltres/SupprimerPaireBrancheCible",
            data: { idFiltre: idFiltre, idPaire: idLigne, userRights: userRights },
            success: function (data) {
                $("#divFiltreDetails").html(data);
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


//------------fonction qui enregistre les filtres (nouveaux ou modifiés)
function EnregistrerFiltre() {
    var modeSaisie = $("#modeSaisie").val();
    var codeEdit = $.trim($("#codeFiltreEdit").val());
    var descrEdit = $.trim($("#descriptionFiltreEdit").val());
    var userRights = $("#AdditionalParam").val();
    var idFiltre = $("#filtreEditId").val();

    var verif = true;
    if (codeEdit == "" || codeEdit == undefined) {
        $("#codeFiltreEdit").addClass("requiredField");
        verif = false;
    }
    if (descrEdit == "" || descrEdit == undefined) {
        $("#descriptionFiltreEdit").addClass("requiredField");
        verif = false;
    }
    if (idFiltre == "" || idFiltre == undefined) {
        common.dialog.bigError("Veuillez reselectionner le filtre à enregistrer", true);
        verif = false;
    }


    if (verif) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamFiltres/EnregistrerFiltre",
            data: { mode: modeSaisie, idFiltre: idFiltre, codeFiltre: codeEdit, descriptionFiltre: descrEdit, userRights: userRights },
            success: function (data) {
                if (modeSaisie == "Insert") {
                    $("#divFiltresBody").html(data);
                    MapElementsTableauFiltres();
                    AfficherDetailsFiltre("", codeEdit);
                    //$("#VoletDetails").hide();
                }
                else if (modeSaisie == "Update") {
                    $("#ligneFiltre_" + idFiltre).html(data);
                    MapElementsTableauFiltres();
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//----------------fonction qui enregistre les lignes paire branche/cible-----
function EnregistrerPaireBrancheCible(idLigne) {
    var action = $("#drlAction_" + idLigne).val();
    var branche = $("#drlBranche_" + idLigne).val();
    var cible = $("#drlCible_" + idLigne).val();
    var userRights = $("#AdditionalParam").val();
    var idFiltre = $("#filtreEditId").val();
    var idPaire = idLigne;

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamFiltres/EnregistrerPaireBrancheCible",
        data: { idFiltre: idFiltre, idPaire: idPaire, action: action, branche: branche, cible: cible, userRights: userRights },
        success: function (data) {
            if (idLigne == $("#idLigneVide").val()) {
                $("#divPairesBrancheCibleBody").html(data);
                ResetLigneVide();
            }
            else
                $("#lignePaireBrancheCible_" + idLigne).html(data);
            MapElementsTableauPairesBrancheCible();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

//-------------fonction qui reinitialise les champs de la ligne vide--------
function ResetLigneVide() {
    var idLigneVide = $("#idLigneVide").val();
    $("#drlAction_" + idLigneVide).val('');
    $("#drlBranche_" + idLigneVide).val('');
    $("#drlCible_" + idLigneVide).val('');
    $("#divPairesBrancheCibleLigneVide").hide();
}

//-------------fonction qui récupère les cibles d'une branche donnée---------
function GetCibles(idLigne) {
    var branche = $("#drlBranche_" + idLigne).val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamFiltres/GetCibles",
        data: { idLigne: idLigne, branche: branche },
        success: function (data) {
            $("#divListeCible_" + idLigne).html(data);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

//------------fonction qui vérifie le filtre sélectionné-------------
function VerifierFiltre() {
    var idFiltre = $("#filtreEditId").val();
    if (idFiltre > 0) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamFiltres/VerifierFiltre",
            data: { idFiltre: idFiltre },
            success: function (data) {
                CloseLoading();
                if (data != undefined && data != "") {
                    ShowCommonFancy("Error", "", data, 1212, 700, true, true);
                }
                else {
                    common.dialog.bigInfo("Le filtre est valide<br />", true);
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}
//------- Vide les listes de recherche-----
function Initialiser() {
    $("#divFiltresBody").html('');
    $("#divFiltreDetails").html('');
}