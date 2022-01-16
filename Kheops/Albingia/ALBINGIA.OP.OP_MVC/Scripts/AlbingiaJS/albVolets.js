
$(document).ready(function () {
    CloseFancy();
    GetCiblesBranche();
    rechercherVolets();
    MapElementPage();
});
//-------Map les éléments de la page---------
function MapElementPage() {
    $("#btnConfirmOk").die().live('click', function () {
        var action = $("#hiddenAction").val();
        switch (action) {
            case "DelVolet":
                supprimerVolet();
                break;
        }
        $("#hiddenAction").val("");
        CloseCommonFancy();
    });
    $("#btnConfirmCancel").die().live('click', function () {
        $("#hiddenAction").val("");
        CloseCommonFancy();
    });
    $("#btnRechercher").die().live('click', function () {
        rechercherVolets();
    });
}
//------------Retour de la consultation volet---------------
function afficheConsultVolet(e, data) {
    e.html(data);
    e.show();

    $("#drlBranche").die().live('change', function () {
        AffectTitleList($(this));
    });

    $('#btnEnregistrer').click(function (evt) {
        enregistrerVolet();
    });
}
//-------------Masque la consultation bloc--------------
function MasqueConsultVolet(e) {
    e.hide();
}
//-------------Ajoute un volet---------------------
function ajouterVolet() {
    $.ajax({
        type: "POST",
        url: "/Volets/InitialiserVolet",
        context: $("#divInfoVolet"),
        success: function (data) {
            afficheConsultVolet($(this), data);
            rechercherVolets();

        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------------Selection des cibles liés à la branche----------------

function GetCiblesBranche() {

    $("#ddlBranches").live("change", function () {
        $.ajax({
            type: "POST",
            url: "/Volets/GetCiblesBranche",
            data: { branche: $("#ddlBranches").val() },
            context: $("#CiblesCarac"),
            success: function (data) {
                afficheConsultVolet($(this), data);
                rechercherVolets();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });

}
//--------------Associe une catégorie au volet---------------
function LinkVolet() {
    var codeIdVolet = $("#GuidId").val();
    var codeVolet = $("#Code").val();
    var codeIdCategorie = $("#Categorie").val();
    var codeCaractere = $("#Caractere").val();

    $.ajax({
        type: "POST",
        url: "Volets/AssocieCategorie",
        data: { codeIdVolet: codeIdVolet, codeVolet: codeVolet, codeIdCategorie: codeIdCategorie, codeCaractere: codeCaractere },
        context: $("#divListCategories"),
        success: function (data) {
            $(this).html(data);
            AlternanceLigne("Categorie", "", false);
            $("#divLinkCategorie").hide();
            $("#Categorie").val('');
            $("#Caractere").val('');
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Lance la recherche des volets--------------
function rechercherVolets() {
    var codeVolet = $("#RechercheVolets_Code").val();
    var descrVolet = $("#RechercheVolets_Description").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Volets/Recherche",
        context: $("#divBodyVolets"),
        data: { code: codeVolet, description: descrVolet },
        success: function (data) {
            CloseLoading();
            $(this).html(data);
            $("#divLstVolets").show();
            affecterClick();
            AlternanceLigne("Volet", "GuidId", true, null);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Affiche le résutat de la recherche des volets--------------
function afficherVolets() {
    $("#divLstVolets").show();
}
//--------------Affecte la fonction du click sur les td Code--------------
function affecterClick() {
    $(".linkVolet").each(function () {
        $(this).click(function () {
            afficherInfoVolet($(this).parent().attr("id"), 1);
            AlternanceLigne("Volet", "GuidId", true, null);
            $(this).parent().css({ "background-color": "#FFDFDF" });
        });
    });
    $("img[name=modifVolet]").each(function () {
        $(this).click(function () {
            afficherInfoVolet($(this).attr("id"), 0);
            AlternanceLigne("Volet", "GuidId", true, null);
            $(this).parent().parent().css({ "background-color": "#FFDFDF" });
        });
    });
    $("img[name=supprVolet]").each(function () {
        $(this).click(function () {
            $("#SupprVolet").val($(this).attr("id"));
            ShowCommonFancy("Confirm", "DelVolet", "Etes-vous sûr de vouloir supprimer ce volet ?", 350, 80, true, true);
        });
    });
}
//--------------Affiche les infos du volet----------------------------
function afficherInfoVolet(codeId, readonly) {
    codeId = codeId.split("_")[1];
    $.ajax({
        type: "POST",
        url: "/Volets/ConsultVolet",
        context: $("#divInfoVolet"),
        data: { codeId: codeId, readOnly: readonly },
        success: function (data) {
            afficheConsultVolet($(this), data);
            AlternanceLigne("Categorie", "", false);
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
//----------------------Envoi de la form pour l'enregistrement---------------------
function enregistrerVolet() {
    $("#Code").val($("#Code").val().replace(/ /g, ''));

    var codeId = $("#GuidId").val();
    var code = $.trim($("#Code").val());
    var description = $("#Description").val();
    var update = $("#Update").val();
    var branche = $("#drlBranche").val();

    if (code == "" || description == "") {
        AddClassRequired($("#Code"));
        AddClassRequired($("#Description"));
        return false;
    }

    var validCode = true;
    $("td[name=codeVolet]").each(function () {
        if ($.trim($(this).text()).toUpperCase() == code.toUpperCase() && codeId == "") {
            validCode = false;
        }
    });

    if (!validCode) {
        AddClassRequired($("#Code"));
        common.dialog.error("Code déjà existant.");
        return false;
    }

    var isVoletGeneral = $("#chkVoletGeneral").is(":checked");
    var isVoletCollapse = $("#chkVoletCollapse").is(":checked");

    $.ajax({
        type: "POST",
        url: "/Volets/Enregistrer",
        data: {
            codeId: codeId, code: code, description: description, update: update, branche: branche,
            isVoletGeneral: isVoletGeneral, isVoletCollapse: isVoletCollapse
        },
        success: function () {
            rechercherVolets();
            $("#divInfoVolet").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Suppression d'un volet-----------------
function supprimerVolet(e) {
    var codeVolet = $("#SupprVolet").val();
    $.ajax({
        type: "POST",
        url: "/Volets/Supprimer",
        context: $("#divInfoVolet"),
        data: { code: codeVolet.split("_")[1] },
        success: function (data) {
            if (data == "") {
                $("#" + codeVolet).parent().parent().remove();
                MasqueConsultVolet($(this));
                AlternanceLigne("Volet", "GuidId", true, null);
            }
            else {
                common.dialog.error(data); CloseLoading();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
    $("#SupprVolet").val("");
}
//-------------------- Crée la boite de dialog pour le contexte.
function ShowFancy() {
    $.fancybox(
        $("#FancyAlert").html(),
        {
            'autodimension': false,
            'width': 120,
            'height': 'auto',
            'transitionIn': 'elastic',
            'transitionOut': 'elastic',
            'speedin': 400,
            'speedOut': 400,
            'easingOut': 'easeInBack',
            'modal': true
        }
    );
}
//------------------ Ferme la boîte de dialogue Fancy ----------------
function CloseFancy() {
    $("#btnOK").live('click', function () {
        $.fancybox.close();
    });
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}