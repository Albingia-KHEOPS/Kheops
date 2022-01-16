$(document).ready(function () {
    MapElementPage();
});
//-------------------------Mapper les elements de la page
function MapElementPage() {
    AlternanceLigne("EngagementBody", "", false, null);
    AlternanceLigne("EngagementBody2", "", false, null);
    AlternanceLigne("MontantBody", "", false, null);
    $("input[type=radio][name=rbPart]").die().live('change', function () {
        var elem = $("tr[id=tr_" + $("#elementSelected").val() + "]");
        if ($("#rbPartAlbingia").is(':checked')) {
            $("td[name=partTotale]").hide();
            $("td[name=partAlbingia]").show();
            $("td[name=lock_partTotale]").hide();
            $("td[name=lock_partAlbingia]").show();

            elem.find("td[name=edit_partTotale]").hide();
            elem.find("td[name=edit_partAlbingia]").show();
        }
        else if ($("#rbPartTotale").is(':checked')) {
            $("td[name=partTotale]").show();
            $("td[name=partAlbingia]").hide();
            $("td[name=lock_partTotale]").show();
            $("td[name=lock_partAlbingia]").hide();

            elem.find("td[name=edit_partAlbingia]").hide();
            elem.find("td[name=edit_partTotale]").show();
        }
        elem.find("td[name=lock_partAlbingia]").hide();
        elem.find("td[name=lock_partTotale]").hide();
    });
    $("#btnSuivant").kclick(function (evt, data) {
        if ($("#Provenance").val() == "EngagementPeriodes") {
            Redirection("RechercheSaisie", "Index");
        }
        else {
            Redirection(data && data.returnHome ? "RechercheSaisie" : "AnMontantReference", "Index");
        }
    });
    $("#btnAnnuler").die().live('click', function () {
        if (window.isReadonly) {
            Redirection($("#Provenance").val(), "Index");
        }
        else {
            ShowCommonFancy("Confirm", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        }
    });
    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":

                Redirection($("#Provenance").val(), "Index");
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });
    MapElementTabEngagementMontant();
}
function MapElementTabEngagementMontant() {
    $("#elementSelected").val('');
    if (!window.isReadonly) {
        $("img[id=ajouterEngagementPeriode]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
    }

    $("#ajouterEngagementPeriode").die().live('click', function () {
        if ($(this).hasClass("CursorPointer")) {
            $("#newEngagementPeriode").show();
            $("img[id=ajouterEngagementPeriode]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
        }
    });
    $("img[name=updateEngagement]").die().live('click', function () {
        if (!window.isReadonly) {
            var lineEngagement = $("tr[id=tr_" + $(this).attr('id').split('_')[1] + "]");
            $("#elementSelected").val($(this).attr('id').split('_')[1]);
            Edit(lineEngagement);
            if ($("#newEngagementPeriode").is(":visible"))
                $("#newEngagementPeriode").hide();
        }
    });
    $(".butonSave").die().live('click', function () {
        // SaveLineEngagement($(this).attr('id').split('_')[1]);
        if ($("#newEngagementPeriode").is(":visible")) {
            $("#newEngagementPeriode").hide();
            $("img[id=ajouterEngagementPeriode]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
        }

    });
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
}
//----Mode édition pour un engagement
function Edit(elem) {
    $("td[name=edit]").hide();
    $("td[name=edit_partAlbingiaEng]").hide();
    //$("td[name=edit_partTotale]").hide();

    //if ($("#rbPartAlbingia").is(':checked')) {
    $("td[name=lock_partAlbingiaEng]").show();
    //$("td[name=lock_partTotale]").hide();

    elem.find("td[name=lock_partAlbingiaEng]").hide();
    elem.find("td[name=edit_partAlbingiaEng]").show();
    //}
    //else if ($("#rbPartTotale").is(':checked')) {
    //    $("td[name=lock_partTotale]").show();
    //    $("td[name=lock_partAlbingia]").hide();

    //    elem.find("td[name=lock_partTotale]").hide();
    //    elem.find("td[name=edit_partTotale]").show();
    //}
    $("td[name=lock]").show();
    elem.find("td[name=lock]").hide();
    elem.find("td[name=edit]").show();

    $("img[id=ajouterEngagementPeriode]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
}
//----------------Sauvegarde une période d'engagement--------------------
function SaveLineEngagement(code) {
    var dateDeb;
    var dateFin;
    var parameters = "params";
    if (code == "-9999") {
        var lineEngagement = $("tr[id=newEngagementPeriode]");
        lineEngagement.find("input[name=Traite]").each(function () {
            parameters += "_" + this.value + "codeFamille" + this.id.split('_')[2];
        });

        dateDeb = lineEngagement.find("input[name=DateDeb]").val();
        dateFin = lineEngagement.find("input[name=DateFin]").val();
    }
    else {
        var lineEngagement = $("tr[id=tr_" + code + "]");
        lineEngagement.find("input[name=Traite]").each(function () {
            parameters += "_" + this.value + "idTraite" + this.id.split('_')[2] + "codeFamille" + this.id.split('_')[1];
        });
    }
    $.ajax({
        type: "POST",
        url: "/EngagementsConnexite/UpdateEngagementTraite/",
        data: {
            idEngagement: code, parameters: parameters, numConnexite: $("#NumConnexite").val(),
            typeOffre: $("#Offre_Type").val(), versionOffre: $("#Offre_Version").val(), codeOffre: $("#Offre_CodeOffre").val(),
            dateDeb: dateDeb, dateFin: dateFin
        },
        success: function (data) {
            $("#tblMontantBody").html(data);
            AlternanceLigne("MontantBody", "", false, null);
            lineEngagement.find("td[name=lock_partAlbingiaEng]").show();
            lineEngagement.find("td[name=edit_partAlbingiaEng]").hide();
            lineEngagement.find("td[name=lock]").show();
            lineEngagement.find("td[name=edit]").hide();

            MapElementTabEngagementMontant();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/EngagementsConnexite/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}