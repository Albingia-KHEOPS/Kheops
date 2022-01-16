
$(document).ready(function () {
    MapElementPage();
});
//----------------Map les éléments de la page------------------
function MapElementPage() {
    //gestion de l'affichage de l'écran en mode readonly
    if (window.isReadonly) {
        $("img[name=supprDocument]").hide();
        $("#btnSuivant").removeAttr("data-accesskey").hide();
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }


    $("#btnSuivant").kclick(function (evt, data) {
        if (data && data.returnHome) {
            Redirection("RechercheSaisie", "Index");
        }
        else if ($("#txtParamRedirect").val() != "") {
            CommonRedirect($("#txtParamRedirect").val());
        }
        else {
            ShowCommonFancy("Error", "", "Validation de l'offre en cours de développement", 350, 130, true, true);
        }
    });

    $("#btnAnnuler").die().live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Suppr":
                ShowCommonFancy("Error", "", "Suppression du document en cours de développement", 350, 130, true, true);
                break;
            case "Cancel":
                Redirection("ValidationOffre", "Index");
                break;
            case "CancelCreation":
                $("#divDataDistribution").html('');
                $("#divDistribution").hide();
                ReactivateShortCut();
                break;
        }
        $("#hiddenAction").val('');
    });

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    $("#btnNewListDist").live('click', function () {
        OpenCreateDocument();
    });

    $("img[name=supprDocument]").each(function () {
        $(this).click(function () {
            ShowCommonFancy("Confirm", "Suppr",
                "Etes-vous sûr de vouloir supprimer ce document ?",
                350, 130, true, true);
        });
    });
}
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    $.ajax({
        type: "POST",
        url: "/GestionDocument/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, paramRedirect: $("#txtParamRedirect").val(), tabGuid: tabGuid, modeNavig: modeNavig },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Ouvre la fenêtre de création/modification document----------------
function OpenCreateDocument(codeDoc) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionDocument/OpenDocument/",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeDoc: codeDoc, modeNavig: $("#ModeNavig").val() },
        context: $("#divDataDistribution"),
        success: function (data) {
            DesactivateShortCut();
            $(this).html(data);
            AlbScrollTop();
            $("#divDistribution").show();
            MapCreationElement();
            AlternanceLigne("BodyCourrier", "", false, null);
            AlternanceLigne("BodyMail", "", false, null);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------------Map les éléments de la fancy création-------------------
function MapCreationElement() {
    $("#btnAnnulerCreation").die().live('click', function () {
        ShowCommonFancy("Confirm", "CancelCreation",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });

    MapCommonAutoCompAssure();
    MapCommonAutoCompCourtier();

    $("select[id^=TypePartenaire]").die().live('change', function () {
        var idInput = $(this).attr('id');
        var idLigne = idInput.split('_')[1];
        var type = idInput.split('_')[0].replace("TypePartenaire", "");

        $("#CodeCourtier" + type + "_" + idLigne).val("");
        $("#CodeAssure" + type + "_" + idLigne).val("");
        $("#NomAssure" + type + "_" + idLigne).val("");
        $("#NomCourtier" + type + "_" + idLigne).val("");
        $("#Interlocuteur" + type + "_" + idLigne).val("");

        //Reset des autocomplete
        //$("#CodePartenaire" + type + "_" + idLigne).die("change");
        //$("#NomPartenaire" + type + "_" + idLigne).die("change");
        //$("#Interlocuteur" + type + "_" + idLigne).die("change");
        //$("#Interlocuteur" + type + "_" + idLigne).die("blur");

        //$("#CodePartenaire" + type + "_" + idLigne).removeAttr("albAutoComplete").removeAttr("albcontext");
        //$("#NomPartenaire" + type + "_" + idLigne).removeAttr("albAutoComplete").removeAttr("albcontext");
        //$("#Interlocuteur" + type + "_" + idLigne).removeAttr("albAutoComplete").removeAttr("albcontext");

        $("input[name=CodeCourtier]").removeAttr("albAutoComplete").removeAttr("albcontext");
        $("input[name=CodeAssure]").removeAttr("albAutoComplete").removeAttr("albcontext");
        $("input[name=NomCourtier]").removeAttr("albAutoComplete").removeAttr("albcontext");
        $("input[name=NomAssure]").removeAttr("albAutoComplete").removeAttr("albcontext");
        $("input[name=InterlocuteurCourrier]").removeAttr("albAutoComplete").removeAttr("albcontext");

        if ($(this).val() == "ASS") {
            //$("#CodePartenaire" + type + "_" + idLigne).attr("albAutoComplete", "autoCompCodeAssure");
            //$("#NomPartenaire" + type + "_" + idLigne).attr("albAutoComplete", "autoCompNomAssure");
            //MapCommonAutoCompAssure();

            $("#CodeAssure" + type + "_" + idLigne).attr("albAutoComplete", "autoCompCodeAssure");
            $("#NomAssure" + type + "_" + idLigne).attr("albAutoComplete", "autoCompNomAssure");
            MapCommonAutoCompAssure();

            $("td[name=ColAssure" + idLigne + "]").removeClass("tdHidden");
            $("td[name=ColCourtier" + idLigne + "]").addClass("tdHidden");
        }
        else if ($(this).val() == "COURT") {
            //$("#CodePartenaire" + type + "_" + idLigne).attr("albAutoComplete", "autoCompCodeCourtier");
            //$("#NomPartenaire" + type + "_" + idLigne).attr("albAutoComplete", "autoCompNomCourtier");
            $("#Interlocuteur" + type + "_" + idLigne).attr("albAutoComplete", "autoCompNomInterlocuteur");

            $("#CodeCourtier" + type + "_" + idLigne).attr("albAutoComplete", "autoCompCodeCourtier");
            $("#NomCourtier" + type + "_" + idLigne).attr("albAutoComplete", "autoCompNomCourtier");
            MapCommonAutoCompCourtier();

            $("td[name=ColAssure" + idLigne + "]").addClass("tdHidden");
            $("td[name=ColCourtier" + idLigne + "]").removeClass("tdHidden");
            //MapCommonAutoCompCourtier();
        }

    });

    $("input[name^=CodeCourtier]").die().live("focus", function () {
        if ($(this).attr("albAutoComplete") == undefined) {
            $("input[name=CodeCourtier]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=CodeAssure]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=NomCourtier]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=NomAssure]").removeAttr("albAutoComplete").removeAttr("albcontext");

            $(this).attr("albAutoComplete", "autoCompCodeCourtier");
            var id = $(this).attr("id").replace("Code", "Nom");
            $("#" + id).attr("albAutoComplete", "autoCompNomCourtier");
            MapCommonAutoCompCourtier();
        }
    });

    $("input[name^=NomCourtier]").die().live("focus", function () {
        if ($(this).attr("albAutoComplete") == undefined) {
            $("input[name=CodeCourtier]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=CodeAssure]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=NomCourtier]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=NomAssure]").removeAttr("albAutoComplete").removeAttr("albcontext");

            $(this).attr("albAutoComplete", "autoCompNomCourtier");
            var id = $(this).attr("id").replace("Nom", "Code");
            $("#" + id).attr("albAutoComplete", "autoCompCodeCourtier");
            MapCommonAutoCompCourtier();
        }
    });

    $("input[name^=CodeAssure]").die().live("focus", function () {
        if ($(this).attr("albAutoComplete") == undefined) {
            $("input[name=CodeCourtier]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=CodeAssure]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=NomCourtier]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=NomAssure]").removeAttr("albAutoComplete").removeAttr("albcontext");

            $(this).attr("albAutoComplete", "autoCompCodeAssure");
            var id = $(this).attr("id").replace("Code", "Nom");
            $("#" + id).attr("albAutoComplete", "autoCompNomAssure");
            MapCommonAutoCompAssure();
        }
    });

    $("input[name^=NomAssure]").die().live("focus", function () {
        if ($(this).attr("albAutoComplete") == undefined) {
            $("input[name=CodeCourtier]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=CodeAssure]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=NomCourtier]").removeAttr("albAutoComplete").removeAttr("albcontext");
            $("input[name=NomAssure]").removeAttr("albAutoComplete").removeAttr("albcontext");

            $(this).attr("albAutoComplete", "autoCompNomAssure");
            var id = $(this).attr("id").replace("Nom", "Code");
            $("#" + id).attr("albAutoComplete", "autoCompCodeAssure");
            MapCommonAutoCompAssure();
        }
    });

    $("input[name^=InterlocuteurPartenaire]").die().live("focus", function () {
        if ($(this).attr("albAutoComplete") == undefined) {
            var idInput = $(this).attr('id');
            var idLigne = idInput.split('_')[1];
            var type = idInput.split('_')[0].replace("Interlocuteur", "");
            if ($("#TypePartenaire" + type + "_" + idLigne).val() == "COURT") {
                $("input[name^=InterlocuteurEmail]").removeAttr("albAutoComplete").removeAttr("albcontext");

                var idInput = $(this).attr('id');
                var idLigne = idInput.split('_')[1];
                var type = idInput.split('_')[0].replace("Interlocuteur", "");
                $("#TypePartenaire" + type + "_" + idLigne).val()

                $(this).attr("albAutoComplete", "autoCompNomInterlocuteur");
                MapCommonAutoCompCourtier();
            }
        }
    });

}