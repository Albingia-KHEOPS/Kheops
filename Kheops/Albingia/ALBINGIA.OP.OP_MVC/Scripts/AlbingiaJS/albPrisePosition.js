$(document).ready(function () {
    MapElementsPage();
});

function MapElementsPage() {

    var etat = $("#oldEtat").val();
    var situation = $("#oldSituation").val();

    if ($("#rdRefus").is(':disabled') && etat == "V" && situation == "A")
        $("#rdRefus").removeAttr("disabled");

    $("input[name='PrendrePosition']").live('change', function () {
        $("#btnValider").removeAttr("disabled");
        if ($("#rdAttente").is(':checked')) {
            $("#MotifAttente").removeAttr("disabled");
            $("#MotifRefus").attr("disabled", "disabled");
            $("#MotifRefus").val("");
        }
        if ($("#rdRefus").is(':checked')) {
            $("#MotifAttente").attr("disabled", "disabled");
            $("#MotifRefus").removeAttr("disabled");
            $("#MotifAttente").val("");
        }
        if ($("#rdAccepter").is(':checked')) {
            $("#MotifAttente").attr("disabled", "disabled");
            $("#MotifRefus").attr("disabled", "disabled");
            $("#MotifAttente").val("");
            $("#MotifRefus").val("");
        }
    });


    $("#btnValider").die().live("click", function () {
        ValiderPrisePosition();
    });

    $("#btnAnnuler").die().live("click", function () {
        AnnulerPrisePosition();
    });

    $("#dvModifInfoBase").die().live('click', function () {
        OpenInfoBase("True");
    });
}

function ValiderPrisePosition() {
    $("input[type=text]").removeClass("requiredField");
    let isCorrect = true;
    let position = "";
    let motif = "";
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let tabGuid = $("#tabGuid").val();
    let modeNavig = $("#ModeNavig").val();
    let addParamType = $("#AddParamType").val();
    let addParamValue = $("#AddParamValue").val();
    let saveCancel = $("#txtSaveCancel").val();
    let oldSituation = $("#oldSituation").val();
    let oldEtat = $("#oldEtat").val();

    if ($("#rdAttente").is(':checked')) {
        if ($("#MotifAttente").val() == "") {
            $("#MotifAttente").addClass("requiredField");
            isCorrect = false;
        }
        else {
            position = "attente";
            motif = $("#MotifAttente").val();
        }
    }
    if ($("#rdRefus").is(':checked')) {
        if ($("#MotifRefus").val() == "") {
            $("#MotifRefus").addClass("requiredField");
            isCorrect = false;
        }
        else {
            position = "refus";
            motif = $("#MotifRefus").val();
        }
    }
    if ($("#rdAccepter").is(':checked')) {
        position = "accepter";
    }

    if (isCorrect) {
        common.page.isLoading = true;
        let postedData = {
            codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, modeNavig: modeNavig, saveCancel: saveCancel,
            addParamType: addParamType, addParamValue: addParamValue,
            position: position, motif: motif, oldSituation: oldSituation, oldEtat: oldEtat
        };
        common.$postJson("/PrisePosition/EnregistrerPosition", postedData, true).done();
    }

}

function AnnulerPrisePosition() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/PrisePosition/Annuler",
        data: {
            codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid,
            addParamType: addParamType, addParamValue: addParamValue, modeNavig: modeNavig
        },
        success: function (data) {
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

