$(document).ready(function () {
    MapElementPage();
});
//----------------Map les éléments de la page------------------
function MapElementPage() {

    $("#btnSuivant").kclick(function (evt, data) {
        if (data && data.returnHome) {
            common.page.goHome();
        }
    });

    $("#btnAnnuler").die().live('click', function () {
        ShowCommonFancy("ConfirmTrans", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });

    $("#btnConfirmTransOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "DelAssuRef":
                DeleteAssureRef();
                break;
            case "DelAssuNonRef":
                DeleteAssureNonRef();
                break;
            case "Cancel":
                break;
        }
        $("#hiddenAction").val('');
    });

    $("#btnConfirmTransCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#DelAssu").val('');
    });

    MapElementAssuRef();
    MapElementAssuNonRef();
}
//-----------Map les éléments des assurés référencés-------------
function MapElementAssuRef() {
    $("#addAssureRef").die().live('click', function () {
        OpenAddAssureRef();
    });

    $("img[name=UpdAssRef]").die();
    $("img[name=UpdAssRef]").each(function () {
        $(this).click(function () {
            OpenAddAssureRef($(this));
        });
    });

    $("img[name=DelAssRef]").die();
    $("img[name=DelAssRef]").each(function () {
        $(this).click(function () {
            var splitChar = $("#SplitChar").val();
            var codeAssure = $(this).attr("id").split(splitChar)[1];
            var nameAssure = $("td[name=NomAssure" + splitChar + codeAssure + "]").text();
            $("#DelAssu").val(codeAssure);
            ShowCommonFancy("Confirm", "DelAssuRef", "Etes-vous sûr de vouloir supprimer l'assuré<br/> " + nameAssure + " ?",
                320, 150, true, true);
        });
    });

    AlternanceLigne("AssureRefBody", "", false, null);
    MapElementsQualite();
}
//-----------Map les éléments des assurés non référencés-----------
function MapElementAssuNonRef() {
    $("#addAssureNonRef").die().live('click', function () {
        OpenAddAssureNonRef();
    });


    $("img[name=UpdAssNonRef]").die();
    $("img[name=UpdAssNonRef]").each(function () {
        $(this).click(function () {
            var splitChar = $("#SplitChar").val();
            var idAssu = $(this).attr("id").split(splitChar)[1];
            OpenAddAssureNonRef(idAssu);
        });
    });

    $("img[name=DelAssNonRef]").die();
    $("img[name=DelAssNonRef]").each(function () {
        $(this).click(function () {
            var splitChar = $("#SplitChar").val();
            var idAssure = $(this).attr("id").split(splitChar)[1];
            $("#DelAssu").val(idAssure);
            ShowCommonFancy("Confirm", "DelAssuNonRef", "Etes-vous sûr de vouloir supprimer l'assuré non désigné ?",
                 320, 150, true, true);
        });
    });

    AlternanceLigne("AssureNonRefBody", "", false, null);
    MapElementsQualite();
}

function MapElementsQualite() {
    AffectTitleList($("#CodeQualite1"));
    AffectTitleList($("#CodeQualite2"));
    AffectTitleList($("#CodeQualite3"));

    $("#CodeQualite1").die().live("change", function () {
        AffectTitleList($(this));
    });

    $("#CodeQualite2").die().live("change", function () {
        AffectTitleList($(this));
    });

    $("#CodeQualite3").die().live("change", function () {
        AffectTitleList($(this));
    });
}
//----------Enlève la class requiredFiel---------
function RemoveRequired() {
    $("input[type=text]").removeClass("requiredField");
    $("select").removeClass("requiredField");
}
//--------------Ouvre la div pour l'assuré référencé-------------
function OpenAddAssureRef(elem) {
    var splitChar = $("#SplitChar").val();

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var codeAssu = "";
    if (elem != null) {
        codeAssu = elem.attr("id").split(splitChar)[1];
    }

    $.ajax({
        type: "POST",
        url: "/AssuresAdditionnels/OpenAddAssureRef",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeAssu: codeAssu, modeNavig: $("#ModeNavig").val() },
        success: function (data) {
            $("#divDataAssure").height(217);
            $("#divDataAssure").html(data);
            AlbScrollTop();
            $("#divAssure").show();
            MapElementDiv();
            MapElementAutoComplete();
            MapElementsQualite();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Ouvre la div pour l'assuré non référencé-------------
function OpenAddAssureNonRef(idAssu) {
    var splitChar = $("#SplitChar").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();

    var codeOldQualite1 = $("td[name=\"Qual1" + splitChar + idAssu + "\"]").text().split(" - ")[0];
    var codeOldQualite2 = $("td[name=\"Qual2" + splitChar + idAssu + "\"]").text().split(" - ")[0];
    var codeOldQualite3 = $("td[name=\"Qual3" + splitChar + idAssu + "\"]").text().split(" - ")[0];
    var oldQualiteAutre = $("img[name=\"QualAutre" + splitChar + idAssu + "\"]").attr('title');

    $.ajax({
        type: "POST",
        url: "/AssuresAdditionnels/OpenAddAssureNonRef",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, idAssure: idAssu, codeOldQualite1: codeOldQualite1,
            codeOldQualite2: codeOldQualite2, codeOldQualite3: codeOldQualite3, oldQualiteAutre: oldQualiteAutre,
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            $("#divDataAssure").height(190);
            $("#divDataAssure").html(data);
            AlbScrollTop();
            $("#divAssure").show();
            MapElementDiv();
            MapElementsQualite();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Map les éléments de la div flottante-----------
function MapElementDiv() {
    $("#btnFancyAnnuler").die().live('click', function () {
        $("#divDataAssure").html("");
        $("#divAssure").hide('slow');
    });

    $("#btnFancyValider").die().live('click', function () {
        var modeAssu = $("#ModeAddAssu").val();
        if (modeAssu == "Ref")
            SaveAssureRef();
        if (modeAssu == "NonRef")
            SaveAssureNonRef();
    });

    $("#CodeAssure").die().live('change', function () {
        LoadInfoAssure($(this).val());
    });
}
//--------Charge les informations de l'assuré--------------
function LoadInfoAssure(codeAssu) {
    if (codeAssu != "0" && codeAssu != "") {
        $.ajax({
            type: "POST",
            url: "/AssuresAdditionnels/LoadInfoAssure",
            data: { codeAssu: codeAssu },
            success: function (data) {
                if (data != "") {
                    $("#NomAssure").val(data);
                }
                else {
                    $("#CodeAssure").val(data);
                    $("#NomAssure").val(data);
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    else {
        $("#CodeAssure").val("");
        $("#NomAssure").val("");
    }
}
//-------Sauvegarde l'assuré référencé------------
function SaveAssureRef() {
    RemoveRequired();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var modeEdit = $("#ModeEdit").val();
    var codeAssure = $("#CodeAssure").val();
    var codeQualite1 = $("#CodeQualite1").val();
    var codeQualite2 = $("#CodeQualite2").val();
    var codeQualite3 = $("#CodeQualite3").val();
    var qualiteAutre = $("#QualiteAutre").val();
    var codeAvenant = $("#NumAvenantPage").val() != undefined ? $("#NumAvenantPage").val() : "";

    if (CheckAssureRef()) {
        common.dialog.smallError("Assuré invalide !", true);
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/AssuresAdditionnels/SaveAssureRef",
        data: {
            codeOffre: codeOffre, version: version, type: type, modeEdit: modeEdit, codeAssu: codeAssure,
            codeQual1: codeQualite1, codeQual2: codeQualite2, codeQual3: codeQualite3, qualAutre: qualiteAutre,
            modeNavig: $("#ModeNavig").val(), codeAvenant: codeAvenant
        },
        success: function (data) {
            $("#tblAssureRefBody").html(data);
            $("#divDataAssure").html("");
            $("#divAssure").hide();
            MapElementAssuRef();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Vérifie les informations de l'assuré référencé------
function CheckAssureRef() {
    var codeAssure = $("#CodeAssure").val();

    if (codeAssure == "" || codeAssure == $("#AssureBase").val()) {
        $("#CodeAssure").addClass("requiredField");
        return true;
    }
    return false;
}
//-------Supprime l'assuré référencé------------
function DeleteAssureRef() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAssu = $("#DelAssu").val();
    var codeAvenant = $("#NumAvenantPage").val() != undefined ? $("#NumAvenantPage").val() : "";
    $.ajax({
        type: "POST",
        url: "/AssuresAdditionnels/DeleteAssureRef",
        data: { codeOffre: codeOffre, version: version, type: type, codeAssu: codeAssu, modeNavig: $("#ModeNavig").val(), codeAvenant: codeAvenant },
        success: function (data) {
            $("#tblAssureRefBody").html(data);
            MapElementAssuRef();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Sauvegarde l'assuré non référencé-----------
function SaveAssureNonRef() {
    RemoveRequired();

    var splitChar = $("#SplitChar").val();

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var modeEdit = $("#ModeEdit").val();
    var codeQualite1 = $("#CodeQualite1").val();
    var codeQualite2 = $("#CodeQualite2").val();
    var codeQualite3 = $("#CodeQualite3").val();
    var qualiteAutre = $("#QualiteAutre").val();
    var codeAvenant = $("#NumAvenantPage").val() != undefined ? $("#NumAvenantPage").val() : "";

    var idAssureNonRef = $("#IdAssuNonRef").val();

    var codeOldQualite1 = $("td[name=Qual1" + splitChar + idAssureNonRef + "]").text().split(" - ")[0];
    var codeOldQualite2 = $("td[name=Qual2" + splitChar + idAssureNonRef + "]").text().split(" - ")[0];
    var codeOldQualite3 = $("td[name=Qual3" + splitChar + idAssureNonRef + "]").text().split(" - ")[0];
    var oldQualiteAutre = $("img[name=QualAutre" + splitChar + idAssureNonRef + "]").attr('title');

    if (CheckAssureNonRef()) {
        common.dialog.error("Veuillez renseigner au moins une qualité.");
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/AssuresAdditionnels/SaveAssureNonRef",
        data: {
            codeOffre: codeOffre, version: version, type: type, modeEdit: modeEdit,
            codeQual1: codeQualite1, codeQual2: codeQualite2, codeQual3: codeQualite3, qualAutre: qualiteAutre,
            codeOldQual1: codeOldQualite1, codeOldQual2: codeOldQualite2, codeOldQual3: codeOldQualite3, oldQualAutre: oldQualiteAutre,
            modeNavig: $("#ModeNavig").val(), codeAvenant: codeAvenant
        },
        success: function (data) {
            $("#tblAssureNonRefBody").html(data);
            $("#divDataAssure").html("");
            $("#divAssure").hide();
            MapElementAssuNonRef();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Vérifie les informations de l'assuré non référencé--------
function CheckAssureNonRef() {
    var codeQualite1 = $("#CodeQualite1").val();
    var codeQualite2 = $("#CodeQualite2").val();
    var codeQualite3 = $("#CodeQualite3").val();
    var qualiteAutre = $("#QualiteAutre").val();

    if (codeQualite1 == "" && codeQualite2 == "" && codeQualite3 == "" && qualiteAutre == "") {
        $("#CodeQualite1").addClass("requiredField");
        $("#CodeQualite2").addClass("requiredField");
        $("#CodeQualite3").addClass("requiredField");
        $("#QualiteAutre").addClass("requiredField");
        return true;
    }
    return false;
}
//-------Supprime l'assuré non référencé------------
function DeleteAssureNonRef() {
    var splitChar = $("#SplitChar").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var idAssu = $("#DelAssu").val();

    var codeOldQualite1 = $("td[name=Qual1" + splitChar + idAssu + "]").text().split(" - ")[0];
    var codeOldQualite2 = $("td[name=Qual2" + splitChar + idAssu + "]").text().split(" - ")[0];
    var codeOldQualite3 = $("td[name=Qual3" + splitChar + idAssu + "]").text().split(" - ")[0];
    var oldQualiteAutre = $("img[name=QualAutre" + splitChar + idAssu + "]").attr('title');

    var codeAvenant = $("#NumAvenantPage").val() != undefined ? $("#NumAvenantPage").val() : "";

    $.ajax({
        type: "POST",
        url: "/AssuresAdditionnels/DeleteAssureNonRef",
        data: {
            codeOffre: codeOffre, version: version, type: type,
            codeOldQual1: codeOldQualite1, codeOldQual2: codeOldQualite2, codeOldQual3: codeOldQualite3, oldQualAutre: oldQualiteAutre,
            modeNavig: $("#ModeNavig").val(), codeAvenant: codeAvenant
        },
        success: function (data) {
            $("#tblAssureNonRefBody").html(data);
            MapElementAssuNonRef();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}