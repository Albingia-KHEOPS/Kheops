/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementPage();

    common.autonumeric.apply($("#Kagtmap"), 'init', 'numeric', '', null, null, '99', '0');
});

//----------Map les éléments de la page--------
function MapElementPage() {
    $("#btnRechercher").die().live('click', function () {
        rechercherInventaires();
    });
    affectEvents();
    MapCommonAutoCompInventaire();
}

function affectEvents() {
    affecterClick();
    AlternanceLigne("InventaireBody", "Code", true, null);
}

function rechercherInventaires() {
    ShowLoading();
    $("#VoletDetail").hide();
    var codeInventaire = $("#RechercheCode").val();
    var descrInventaire = $("#RechercheDescription").val();
    $.ajax({
        type: "POST",
        url: "/ParamInventaires/Recherche",
        context: $("#divBodyInventaires"),
        data: { code: codeInventaire, description: descrInventaire },
        success: function (data) {
            CloseLoading();
            $(this).html(data);
            $("#divLstInventaires").show();
            affectEvents();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------------Affecte la fonction du click sur les td Code--------------
function affecterClick() {
    $("td[name=codeInventaire], td[name=libelleInventaire]").each(function () {
        $(this).click(function () {
            afficherInfoInventaire($(this).parent().attr("id"), 1);
            AlternanceLigne("InventaireBody", "Code", true, null);

            $(this).parent().parent().children(".selectLine").removeClass("selectLine");
            $(this).parent().addClass("selectLine");
        });
    });

    $("img[name=supprInventaire]").each(function () {
        $(this).click(function () {
            VerificationAvantSuppressionInventaire($(this));
            AlternanceLigne("InventaireBody", "Code", true, null);
        });
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "SuppressionInventaire":
                var inventaire = $("#idInventaireSuppr").val();
                SupprimerInventaire(inventaire);
                break;
        }
        $("#hiddenAction").val('');
    });

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#DelAssu").val('');
    });

    $("#btnAjouter").die().live('click', function () {
        afficherInfoInventaire("_", 0);
    });
}

//--------------Affiche les infos de l'inventaire----------------------------
function afficherInfoInventaire(codeId, readonly) {
    codeId = codeId.split("_")[1];
    var libelle = $("#libelleInventaire_" + codeId).text().trim();
    var code = $("#codeInventaire_" + codeId).text().trim();
    var kagtmap = $("#kagtmapInventaire_" + codeId).text().trim();
    var codefiltre = $("#codefiltreInventaire_" + codeId).text().trim();
    $.ajax({
        type: "POST",
        url: "/ParamInventaires/ConsultInventaire",
        data: { codeId: codeId, codeInventaire: code, libInventaire: libelle, kagtmap: kagtmap, codeFiltre: codefiltre, readOnly: readonly },
        success: function (data) {
            afficheConsultInventaire(data);

            $("#VoletDetail").show();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------Retour de la consultation inventaire---------------
function afficheConsultInventaire(data) {
    $("#inventaireDetails").html(data);
    AlternanceLigne("BodyBSC", "", false);
    $('#btnEnregistrer').click(function (evt) {
        enregistrerInventaire();
    });
}

function enregistrerInventaire() {
    $(".requiredField").removeClass("requiredField");
    var codeInventaire = $("#CodeDetail").val();
    var codeId = $("#DetailGuidId").val();
    var description = $("#Libelle").val();
    var typogrille = $("#Kagtmap").val();

    var mode = $("#Mode").val();
    var codeFiltre = $("#CodeFiltre").val();
    var required = true;
    if (description == "") {
        AddClassRequired($("#Libelle"));
        required = false;
    }
    if (codeInventaire == "") {
        AddClassRequired($("#CodeDetail"));
        required = false;
    }
    if (typogrille == "0" || typogrille == "") {
        AddClassRequired($("#Kagtmap"));
        required = false;
    }

    var validCode = true;
    $("td[name='codeInventaire']").each(function () {
        if ($.trim($(this).text()).toUpperCase() == codeInventaire.toUpperCase() && codeId == "") {
            AddClassRequired($("#CodeDetail"));
            validCode = false;
        }
    });

    if (!validCode) {
        common.dialog.error("Code déjà existant.");
        return false;
    }

    if (required == false) {
        return required;
    }
    $.ajax({
        type: "POST",
        url: "/ParamInventaires/Enregistrer",
        data: { mode: mode, codeId: codeId, codeLib: codeInventaire, description: description, kagtmap: typogrille, codeFiltre: codeFiltre },
        success: function (data) {
            CloseLoading();
            $("#divBodyInventaires").html(data);
            $("#divLstInventaires").show();
            $("#VoletDetail").hide();
            //rechercherInventaires();
            affectEvents();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-------------Suppression d'un inventaire-----------------
function VerificationAvantSuppressionInventaire(e) {
    //Vérification de l'existance de l'inventaire
    $("#idInventaireSuppr").val(e.attr("id").split("_")[1]);
    ShowCommonFancy("Confirm", "SuppressionInventaire", "Etes-vous sûr de vouloir supprimer cet inventaire ?",
     320, 150, true, true);
}

function SupprimerInventaire(guidId) {
    $.ajax({
        type: "POST",
        url: "/ParamInventaires/SupprimerInventaire",
        data: { code: guidId },
        success: function (data) {
            if (data == "True") {
                rechercherInventaires();
                $("#VoletDetail").hide();
            }
            else {
                data = "Impossible de supprimer ce type d'inventaire";
                common.dialog.error(data); CloseLoading();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------------Ajoute la class Required----------
function AddClassRequired(e) {
    e.addClass("requiredField");
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}
