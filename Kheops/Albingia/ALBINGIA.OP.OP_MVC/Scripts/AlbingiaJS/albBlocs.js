$(document).ready(function () {
    rechercherBlocs();
    MapElementPage();
});
//--------------Map les éléments de la page-----
function MapElementPage() {
    $("#btnConfirmOk").die().live('click', function () {
        var action = $("#hiddenAction").val();
        switch (action) {
            case "DelBloc":
                supprimerBloc();
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
        rechercherBlocs();
    });
}
//-------------Ajoute un bloc---------------------
function ajouterBloc() {
    $.ajax({
        type: "POST",
        url: "/Blocs/InitialiserBloc",
        success: function (data) {
            $("#divInfoBloc").html(data);
            $("#divInfoBloc").show();
            $('#btnEnregistrer').click(function (evt) {
                enregistrerBloc();
            });
            rechercherBlocs();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Lance la recherche des blocs--------------
function rechercherBlocs() {
    var codeBloc = $("#RechercheBlocs_Code").val();
    var descrBloc = $("#RechercheBlocs_Description").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Blocs/Recherche",
        data: { code: codeBloc, description: descrBloc },
        success: function (data) {
            CloseLoading();
            $("#divBodyBlocs").html(data);
            $("#divBodyBlocs").show();
            affecterClick();
            AlternanceLigne("Bloc", "Code", true, null); //TODO a remplacer par GuidId
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Affiche le résutat de la recherche des blocs--------------
function afficherBlocs() {
    $("#divBodyBlocs").show();
}
//--------------Affecte la fonction du click sur les td Code--------------
function affecterClick() {
    $(".linkBloc").each(function () {
        $(this).click(function () {
            afficherInfoBloc($(this).parent().attr("id"), 0);
            AlternanceLigne("Bloc", "Code", true, null); //TODO a remplacer par GuidId
            $(this).parent().css({ "background-color": "#FFDFDF" });
        });
    });
    $("img[name=supprBloc]").each(function () {
        $(this).click(function () {
            $("#SupprBloc").val($(this).attr("id"));
            ShowCommonFancy("Confirm", "DelBloc", "Etes-vous sûr de vouloir supprimer ce bloc ?", 350, 80, true, true);
        });
    });
}
//---------------fonction qui map les éléments du panneau de details des blocs
function MapElementsDetailsBloc() {
    $('#btnEnregistrer').click(function (evt) {
        enregistrerBloc();
    });

    $("#btnAjouterBlocIncompatible").unbind();
    $("#btnAjouterBlocIncompatible").live("click", function () {
        $("#divBlocsIncompatiblesLigneVide").show();
    });

    $("#btnAjouterBlocAssocie").unbind();
    $("#btnAjouterBlocAssocie").live("click", function () {
        $("#divBlocsAssociesLigneVide").show();
    });

    $("#btnSaveNewBlocIncompatible").unbind();
    $("#btnSaveNewBlocIncompatible").bind("click", function () {
        EnregistrerModificationBlocIncompatibleAssocie("Insert", "I", "0");
    });

    $("#btnSaveNewBlocAssocie").unbind();
    $("#btnSaveNewBlocAssocie").bind("click", function () {
        EnregistrerModificationBlocIncompatibleAssocie("Insert", "A", "0");
    });

    $("#drlNewBlocAssocie").die().live('change', function () {
        AffectTitleList($(this));
    });

    $("#drlNewBlocIncompatible").die().live('change', function () {
        AffectTitleList($(this));
    });



    MapElementsTableauxIncompatibleAssocie();
}
//-----------------fonction qui map les éléments des tableaux blocs incompatibles et blocs associés------
function MapElementsTableauxIncompatibleAssocie() {
    $("td[albClikable=selectableCol]").die().live('click', function () {
        var id = $(this).attr('name').split('_')[1];
        if ($("#selectedRow").val() != "") {
            if (id != $("#selectedRow").val()) {
                $("div[name=divReadOnly_" + $("#selectedRow").val() + "]").show();
                $("div[name=divEdition_" + $("#selectedRow").val() + "]").hide();
            }
        }

        $("#selectedRow").val(id);
        $("div[name=divReadOnly_" + id + "]").hide();
        $("div[name=divEdition_" + id + "]").show();
        $("#divBlocsIncompatiblesLigneVide").hide();
        $("#divBlocsAssociesLigneVide").hide();
    });

    $("img[name=supprBlocIncompatible]").unbind();
    $("img[name=supprBlocIncompatible]").bind("click", function () {
        var idAssociation = $(this).attr("id").split("_")[1];
        EnregistrerModificationBlocIncompatibleAssocie("Delete", "I", idAssociation);
    });

    $("img[name=supprBlocAssocie]").unbind();
    $("img[name=supprBlocAssocie]").bind("click", function () {
        var idAssociation = $(this).attr("id").split("_")[1];
        EnregistrerModificationBlocIncompatibleAssocie("Delete", "A", idAssociation);
    });

    $("img[name=saveBlocIncompatible]").unbind();
    $("img[name=saveBlocIncompatible]").bind("click", function () {
        var idAssociation = $(this).attr("id").split("_")[1];
        EnregistrerModificationBlocIncompatibleAssocie("Update", "I", idAssociation);
    });

    $("img[name=saveBlocAssocie]").unbind();
    $("img[name=saveBlocAssocie]").bind("click", function () {
        var idAssociation = $(this).attr("id").split("_")[1];
        EnregistrerModificationBlocIncompatibleAssocie("Update", "A", idAssociation);
    });

    AlternanceLigne("BlocsAssocies", "noInput", true, null);
    AlternanceLigne("BlocsIncompatibles", "noInput", true, null);
}


//--------------Affiche les infos du bloc----------------------------
function afficherInfoBloc(codeId, readonly) {
    codeId = codeId.split("_")[1];
    $.ajax({
        type: "POST",
        url: "/Blocs/ConsultBloc",
        data: { codeId: codeId, readOnly: readonly },
        success: function (data) {
            $("#divInfoBloc").html(data);
            $("#divInfoBloc").show();

            MapElementsDetailsBloc();
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
function enregistrerBloc() {
    $("#Code").val($("#Code").val().replace(/ /g, ''));

    var codeId = $("#GuidId").val();
    var code = $.trim($("#Code").val());
    var description = $("#Description").val();
    var update = $("#Update").val();

    if (code == "" || description == "") {
        AddClassRequired($("#Code"));
        AddClassRequired($("#Description"));
        return false;
    }

    var validCode = true;
    $("td[name=codeBloc]").each(function () {
        if ($.trim($(this).text()).toUpperCase() == code.toUpperCase() && codeId == "") {
            validCode = false;
        }
    });

    if (!validCode) {
        AddClassRequired($("#Code"));
        common.dialog.error("Code déjà existant.");
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/Blocs/Enregistrer",
        data: { codeId: codeId, code: code, description: description, update: update },
        success: function (data) {
            rechercherBlocs();
            $("#divInfoBloc").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Suppression d'un bloc-----------------
function supprimerBloc() {
    var codeBloc = $("#SupprBloc").val().split("_")[1];
    $("#SupprBloc").val("");
    $.ajax({
        type: "POST",
        url: "/Blocs/Supprimer",
        data: { code: codeBloc },
        success: function (data) {
            if (data == "") {
                rechercherBlocs();
                $("#divInfoBloc").hide();
            }
            else {
                common.dialog.error(data); CloseLoading();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}
//------------Enregistre/supprime un bloc incompatible/associé-------
function EnregistrerModificationBlocIncompatibleAssocie(mode, typeBloc, idAssociation) {
    var idBlocIncompatibleAssocie;
    if (mode == "Insert") {
        idAssociation = "0";
        if (typeBloc == "I")
            idBlocIncompatibleAssocie = $("#drlNewBlocIncompatible").val();
        if (typeBloc == "A")
            idBlocIncompatibleAssocie = $("#drlNewBlocAssocie").val();
    }
    else if (mode == "Update") {
        if (typeBloc == "I")
            idBlocIncompatibleAssocie = $("#drlBlocIncompatible_" + idAssociation).val();
        if (typeBloc == "A")
            idBlocIncompatibleAssocie = $("#drlBlocAssocie_" + idAssociation).val();
    }
    else if (mode == "Delete") {
        idBlocIncompatibleAssocie = "0";
    }

    var idBloctraite = $("#GuidId").val();
    $.ajax({
        type: "POST",
        url: "/Blocs/EnregistrerBlocIncompatibleAssocie",
        data: { idAssociation: idAssociation, mode: mode, typeBloc: typeBloc, idBloctraite: idBloctraite, idBlocIncompatibleAssocie: idBlocIncompatibleAssocie },
        success: function (data) {
            $("#divInfoBloc").html(data);
            MapElementsDetailsBloc();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
