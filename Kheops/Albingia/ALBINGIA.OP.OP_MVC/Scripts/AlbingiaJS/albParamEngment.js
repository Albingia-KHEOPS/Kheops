$(document).ready(function () {
    MapElementPage();
});
//--------Map les éléments de la page------------
function MapElementPage() {
    $("#Traite").die().live("change", function () {
        AffectTitleList($(this));
        LoadListColonne($(this).val());
    });
    $("#btnInfoOk").live("click", function () {
        CloseCommonFancy();
    });
    $("#btnConfirmOk").live("click", function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Delete":
                DeleteColonne();
                break;
            case "Cancel":
                //Redirection("CreationFormuleGarantie", "Index");
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live("click", function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#delColonne").val('');
    });

    $('#btnAnnuler').die();
    $('#btnAnnuler').live('click', function (evt) {
        window.location.href = "/BackOffice/Index";
    });
}
//--------Charge la liste des colonnes pour un traité------------
function LoadListColonne(codeTraite) {
    $.ajax({
        type: "POST",
        url: "/ParamEngagements/DisplayListColonne",
        data: { codeTraite: codeTraite },
        success: function (data) {
            $("#divEngmentColonne").html(data);
            $("#divColonne").html("");
            $("#CodeTraite").val(codeTraite);
            MapElementTraiteColonne();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Map les éléments de la div flottante Liste-------
function MapElementTraiteColonne() {
    AlternanceLigne("BodyEngmentCol", "", false, null);
    $("#imgAddCol").die().live('click', function () {
        var codeTraite = $("#CodeTraite").val();
        LoadColonne(codeTraite, 0);
    });
    $("img[name=UpdCol]").die().live('click', function () {
        var splitChar = $("#splitChar").val();
        var codeTraite = $(this).attr("id").split(splitChar)[1];
        var code = $(this).attr("id").split(splitChar)[2];
        LoadColonne(codeTraite, code);
    });
    $("img[name=DelCol]").die().live('click', function () {
        $("#delColonne").val($(this).attr("id"));
        ShowCommonFancy("Confirm", "Delete", "Êtes-vous sûr de vouloir supprimer cette colonne ?", 300, 100, true, true);
    });
}
//------Ouvre la div d'ajout/MAJ d'une colonne-------------
function LoadColonne(codeTraite, code) {
    $.ajax({
        type: "POST",
        url: "/ParamEngagements/DisplayAddColonne",
        data: { codeTraite: codeTraite, code: code },
        success: function (data) {
            $("#divColonne").html(data);
            $("#ModeColonne").val(code == "0" ? "INS" : "MAJ");
            MapElementColonne();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Map les éléments de la div flottante Ajout-----------
function MapElementColonne() {
    $("#btnAdd").die().live('click', function () {
        AddMAJColonne();
    });
}
//--------Ajoute/MAJ la colonne------------
function AddMAJColonne() {
    var codeTraite = $("#CodeTraite").val();
    var code = $("#CodeColonne").val();
    var libelle = $("#LibColonne").val();
    var separation = $("#SepaColonne").is(':checked') ? "O" : "N";
    var mode = $("#ModeColonne").val();
    $.ajax({
        type: "POST",
        url: "/ParamEngagements/SaveColonne",
        data: { codeTraite: codeTraite, code: code, libelle: libelle, separation: separation, mode: mode },
        success: function (data) {
            $("#divColonne").html("");
            LoadListColonne(codeTraite);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Supprime une colonne pour un traité------------
function DeleteColonne() {
    var splitChar = $("#splitChar").val();
    var param = $("#delColonne").val();
    var codeTraite = param.split(splitChar)[1];
    var code = param.split(splitChar)[2];
    $.ajax({
        type: "POST",
        url: "/ParamEngagements/DeleteColonne",
        data: { codeTraite: codeTraite, code: code },
        success: function (data) {
            if (data == "") {
                $("#delColonne").val("");
                LoadListColonne(codeTraite);
            }
            else {
                common.dialog.error(data);
                CloseLoading();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}