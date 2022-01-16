/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementsPage();
    if ($("#userHasRights").val()=="False") {
        $("#RechercheCode").val("KHEOP");
    }
    RechercherConcepts();
});

//-------------Map les éléments de la page-------------
function MapElementsPage() {
    $("#btnRechercher").unbind();
    $("#btnRechercher").bind("click", function () {
        RechercherConcepts();
    });

    $("#btnAjouterConcept").unbind();
    $("#btnAjouterConcept").bind("click", function () {
        AfficherDetailsConcept("");
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "SupprimerConcept":
                var concept = $("#idConceptSuppr").val();
                SupprimerConcept(concept);
                break;
        }
        $("#hiddenAction").val('');
        $("#idConceptSuppr").val('');
    });

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#idConceptSuppr").val('');
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
    MapCommonAutoCompConcepts();
}

//------------Map les lignes du tableau de concepts--------
function MapElementsTableau() {
    $("tr[name=ligneConcept]").unbind();
    $("tr[name=ligneConcept]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        AfficherDetailsConcept(id);
    });

    $("img[name=btnSupprimer]").unbind();
    $("img[name=btnSupprimer]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $("#idConceptSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerConcept", "Vous allez supprimer le concept.<br/>Etes-vous sûr de vouloir continuer ?",
                  320, 150, true, true);
    });

    $("img[name=btnFamille]").unbind();
    $("img[name=btnFamille]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        Redirection("ParamFamilles", "Index", id);
    });

    AlternanceLigne("ConceptsBody", "noInput", true, null);

}

//------------Map les éléments de la partie détails du concept----------
function MapElementsDetails() {
    $("#btnEnregistrer").unbind();
    $("#btnEnregistrer").bind("click", function () {
        EnregistrerConcept();
    });
}

//-----------Lance la recherche des concepts------------
function RechercherConcepts() {
    var codeRecherche = $.trim($("#RechercheCode").val());
    var descrRecherche = $.trim($("#RechercheDescription").val());
    var userRights = $("#AdditionalParam").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamConcepts/RechercheConcepts",
        data: { codeConcept: codeRecherche, descriptionConcept: descrRecherche, userRights: userRights },
        success: function (data) {
            $("#divConceptsBody").html(data);
            MapElementsTableau();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-------------fonction qui récupère les détails du concept (nouveau ou sélectionné)--------
function AfficherDetailsConcept(idLigne) {
    var userRights = $("#AdditionalParam").val();
    if (idLigne != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamConcepts/GetDetailsConcept",
            data: { codeConcept: idLigne, userRights: userRights },
            success: function (data) {
                $("#divConceptDetails").html(data);
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

//------------fonction qui enregistre les concepts (nouveaux ou modifiés)
function EnregistrerConcept() {
    var modeSaisie = $("#modeSaisie").val();
    var codeEdit = $.trim($("#codeConceptEdit").val());
    var descrEdit = $.trim($("#descriptionConceptEdit").val());
    var userRights = $("#AdditionalParam").val();

    var verif = true;
    if (codeEdit == "" || codeEdit == undefined) {
        $("#codeConceptEdit").addClass("requiredField");
        verif = false;
    }
    if (descrEdit == "" || descrEdit == undefined) {
        $("#descriptionConceptEdit").addClass("requiredField");
        verif = false;
    }

    if (verif) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamConcepts/EnregistrerConcept",
            data: { mode: modeSaisie, codeConcept: codeEdit, descriptionConcept: descrEdit, userRights: userRights },
            success: function (data) {
                if (modeSaisie == "Insert") {
                    $("#divConceptsBody").html(data);
                    $("#VoletDetails").hide();
                }
                else if (modeSaisie == "Update")
                    $("#ligneConcept_" + codeEdit).html(data);
                MapElementsTableau();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//-------------Supprime le concept-----------
function SupprimerConcept(id) {
    var userRights = $("#AdditionalParam").val();
    if (id != "" && id != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamConcepts/SupprimerConcept",
            data: { codeConcept: id, userRights: userRights },
            success: function (data) {
                $("#divConceptsBody").html(data);
                MapElementsTableau();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//----------------Redirection------------------
function Redirection(cible, job, codeConcept) {
    var userRights = $("#AdditionalParam").val();
    if (codeConcept != "" && codeConcept != undefined) {
        var descriptionConcept = $("#lblDescription_" + codeConcept).html();
        if (descriptionConcept != "" && descriptionConcept != undefined) {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/ParamConcepts/Redirection",
                data: { cible: cible, job: job, codeConcept: codeConcept, descriptionConcept: descriptionConcept, userRights: userRights },
                success: function (data) {
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    }
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}
//------- Vide les listes de recherche-----
function Initialiser() {
    $("#divConceptsBody").html('');
    $("#divConceptDetails").html('');
}
