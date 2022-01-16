/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
//var mouseX;
//var mouseY;

$(document).ready(function () {
    MapElementPage();
});

//----------------Map les éléments de la page------------------
function MapElementPage() {

    $("#btnFermerGestionIntervenants").die().live('click', function () {
        $("#divGestionIntervenants").hide();
    });

    $("#btnAjouterIntervenant").die().live("click", function () {
        OpenDetailsIntervenant(-9999, "edit");
    });

    $("#btnConfirmOkSupprIntervenant").die().live('click', function () {
        $("#divMsgConfirmIntervenant").hide(); //CloseCommonFancy();
        switch ($("#hiddenActionSupprIntervenant").val()) {
            case "SupprimerIntervenant":
                var intervenant = $("#idIntervenantSuppr").val();
                if (intervenant != undefined && intervenant != "" && intervenant > 0)
                    SupprimerIntervenant(intervenant);
                break;
        }
        $("#hiddenAction").val('');
        $("#idIntervenantSuppr").val('-1');
    });

    $("#btnConfirmCancelSupprIntervenant").die().live('click', function () {
        $("#divMsgConfirmIntervenant").hide(); //CloseCommonFancy();
        $("#hiddenActionSupprIntervenant").val('');
        $("#idIntervenantSuppr").val('-1');
    });

    MapElementTableau();
}

//--------------Map les éléments du tableau d'intervenants
function MapElementTableau() {
    AlternanceLigne("GestionIntervenantsBody", "", true, null);

    $("td[name=selectableCol]").die();
    $("td[name=selectableCol]").each(function () {
        $(this).click(function () {
            var id = $(this).attr("id");
            var guidId = -1;
            if (id != undefined && id.split("_").length > 1)
                guidId = id.split("_")[1];
            if (guidId > 0)
                OpenDetailsIntervenant(guidId, "edit");
        });
    });

    $("img[name=btnSupprimer]").die();
    $("img[name=btnSupprimer]").each(function () {
        $(this).click(function () {
            var id = $(this).attr("id");
            var guidId = -1;
            if (id != undefined && id.split("_").length > 1)
                guidId = id.split("_")[1];
            if (guidId > 0) {
                $("#idIntervenantSuppr").val(guidId);
                $("#divMsgConfirmIntervenant").show();
                $("#hiddenActionSupprIntervenant").val("SupprimerIntervenant");
                // ShowCommonFancy("Confirm", "SupprimerIntervenant", "Vous allez supprimer cet intervenant,<br/>Etes-vous sûr de vouloir continuer ?",
                //320, 150, true, true);
            }
        });
    });

    $("td[albFlyOver=flyOverLine]").die();
    $("td[albFlyOver=flyOverLine]").each(function () {
        $(this).bind('mouseover', function () {
            var id = $(this).attr("id");
            var guidId = -1;
            if (id != undefined && id.split("_").length > 1)
                guidId = id.split("_")[1];
            if (guidId > 0) {
                if (guidId != $("#IdApercu").val() && $("#IsLoadingApercu").val() == "N") {
                    $("#IdApercu").val(guidId);
                    OpenDetailsIntervenant(guidId, "readonly");
                }
            }
            else {
                $("#divDataApercuDetailsIntervenant").hide();
                $("#IdApercu").val(-1);
            }
        });
    });

    //$("tbody[name=flyOverLine]").die();
    //$("tbody[name=flyOverLine]").each(function () {
    //    $(this).bind('mouseout', function () {
    //        if ($(this)[0].parentElement.id != "tblGestionIntervenantsBody") {
    //            $("#divDataApercuDetailsIntervenant").hide();
    //            $("#IdApercu").val(-1);
    //        }
    //    });
    //});

    $(document).mousemove(function (e) {
        //mouseX = e.pageX;
        //mouseY = e.pageY;
        if ($("#IdApercu").val() == -1)
            $("#divDataApercuDetailsIntervenant").hide();
    });

    $("th[name=headerTri]").die().live("click", function () {
        var colTri = $(this).attr("albcontext");
        var img = $("img[albcontext=" + colTri + "]").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1);

        img = img.substr(0, img.lastIndexOf('.'));
        TrierIntervenants(colTri, img);
    });

    $("#divDataGestionIntervenants").die().live("click", function () {
        $("#divDataApercuDetailsIntervenant").hide();
        $("#IdApercu").val(-1);
    });

    $("#divDataApercuDetailsIntervenant").die().live("click", function () {
        return false;
    });
}

//--------------Map les éléments de la div de détails
function MapElementDetails() {
    $("#btnValiderDetailsIntervenants").die().live("click", function () {
        EnregistrementDetailsIntervenant();
    });

    $("#btnAnnulerDetailsIntervenants").die().live("click", function () {
        $("#divDataDetailsIntervenant").html('');
        $("#btnValiderDetailsIntervenants").enable();
        $("#divDetailsIntervenant").hide();
    });

    $("#DDTypeIntervenant").die().live("change", function () {
        AffectTitleList($(this));
        if ($(this).val() == "DR")
            $("#groupChkMedecinConseil").show();
        else {
            $("#groupChkMedecinConseil").hide();
            $("#chkMedecin").removeAttr("checked");
        }
        RazChampsDetails();
        $("#btnValiderDetailsIntervenants").removeAttr("disabled");
    });

    AffectTitleList($("#DDTypeIntervenant"));
    AffectTitleList($("#valTypeIntervenant"));

    MapCommonAutoCompIntervenants();

    $("#Denomination").die().live('blur', function () {
        if ($.trim($("#Denomination").val()) == "")
            RazChampsDetails();
    });

    $("#Denomination").bind('change', function () {
        $("#btnValiderDetailsIntervenants").removeAttr("disabled");
    });
    $("#Interlocuteur").bind('change', function () {
        $("#btnValiderDetailsIntervenants").removeAttr("disabled");
    });
    $("#Reference").bind('change', function () {
        $("#btnValiderDetailsIntervenants").removeAttr("disabled");
    });
    $("#chkPrincipal").bind('change', function () {
        $("#btnValiderDetailsIntervenants").removeAttr("disabled");
    });
}

//------------Ouvre les détails de l'intervenant en paramètre
function OpenDetailsIntervenant(guidId, modeEcran) {
    if (modeEcran == "edit")
        ShowLoading();
    $("#IsLoadingApercu").val("O");

    $.ajax({
        type: "POST",
        url: "/GestionIntervenants/GetDetailsIntervenant/",
        data: { guidId: guidId, modeEcran: modeEcran, isReadOnly: $("#OffreReadOnly").val() },
        success: function (data) {
            if (modeEcran == "edit") {
                $("#divDataDetailsIntervenant").html(data);
                MapElementDetails();
                if (guidId > 0)
                    MapCommonAutoCompInterlocuteursIntervenant();
                $("#divDetailsIntervenant").show();
            }
            if (modeEcran == "readonly") {
                //if (forcedCoordY == undefined)
                //    forcedCoordY = mouseY;
                //if (forcedCoordX == undefined)
                //    forcedCoordX = mouseX;
                var positionImg = $("#type_" + guidId).offset();

                $("#divDataApercuDetailsIntervenant").html(data);
                var divWidth = $("#divDataApercuDetailsIntervenant").width();
                $("#divDataApercuDetailsIntervenant").css({ 'position': 'absolute', 'top': positionImg.top + 15 + 'px', 'left': positionImg.left + 10 + 'px' }).show();

            }
            $("#IsLoadingApercu").val("N");
            $("#btnValiderDetailsIntervenants").attr("disabled", "disabled");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------enregistre les détails de l'intervenant (création et édition)
function EnregistrementDetailsIntervenant() {
    //récupération des infos
    var guidId = $("#GuidIdIntervenant").val();
    var typeInter = $("#DDTypeIntervenant").val();
    var codeInterv = $("#CodeIntervenant").val();
    var codeInterlo = $("#CodeInterlocuteur").val();
    var reference = $("#Reference").val();
    var isPrincipal = $("#chkPrincipal").is(":checked");
    var isMedecinConseil = $("#chkMedecin").is(":checked");

    var codeDossier = $("#Offre_CodeOffre").val();
    var versionDossier = $("#Offre_Version").val();
    var typeDossier = $("#Offre_Type").val();
    var codeAvenant = $("#NumAvenantPage").val() != undefined ? $("#NumAvenantPage").val() : "";
    var denomination = $("#Denomination").val();
    var nomInterlo = $("#Interlocuteur").val();
    
    var isCheck = true;

    $("input[class=requiredField]").removeClass("requiredField");
    if (codeInterv == undefined || codeInterv == "" || codeInterv == 0) {
        $("#Denomination").addClass("requiredField");
        isCheck = false;
    }

    if (codeInterlo == undefined || codeInterlo == "")
        codeInterlo = 0;

    if (isCheck) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/GestionIntervenants/EnregistrerDetailsIntervenant/",
            data: {
                codeDossier: codeDossier, versionDossier: versionDossier, typeDossier: typeDossier, guidId: guidId, typeInter: typeInter, codeInterv: codeInterv, denomination: denomination,
                codeInterlo: codeInterlo, nomInterlo: nomInterlo, reference: reference, isPrincipal: isPrincipal, isMedecinConseil: isMedecinConseil, codeAvenant: codeAvenant
            },
            success: function (data) {
                $("#divDataDetailsIntervenant").html('');
                $("#divDetailsIntervenant").hide();
                $("#divGestionIntervenantsBody").html(data);
                MapElementTableau();
                CloseLoading();

                if (codeAvenant != "" && codeAvenant != "0") {
                    $("#chkIntervenantModificationAVN").attr("checked", "checked");
                }
            },
            error: function (request) {
                let result = null;
                try {
                    result = JSON.parse(request.responseText);
                } catch (e) { result = null; }

                if (!kheops.alerts.blacklist.displayAll(result)) {
                    common.error.showXhr(request);
                }
            }
        });
    }
}

//------------Supprime l'intervenant en paramètre
function SupprimerIntervenant(guidId) {
    var codeDossier = $("#Offre_CodeOffre").val();
    var versionDossier = $("#Offre_Version").val();
    var typeDossier = $("#Offre_Type").val();
    var codeAvenant = $("#NumAvenantPage").val() != undefined ? $("#NumAvenantPage").val() : "";
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionIntervenants/SupprimerIntervenant/",
        data: { codeDossier: codeDossier, versionDossier: versionDossier, typeDossier: typeDossier, guidId: guidId, codeAvenant: codeAvenant },
        success: function (data) {
            $("#divGestionIntervenantsBody").html(data);
            MapElementTableau();
            CloseLoading();
            if (codeAvenant != "" && codeAvenant != "0") {
                $("#chkIntervenantModificationAVN").attr("checked", "checked");
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-----------Tri les intervenants------------
function TrierIntervenants(colTri, img) {
    var codeDossier = $("#Offre_CodeOffre").val();
    var versionDossier = $("#Offre_Version").val();
    var typeDossier = $("#Offre_Type").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionIntervenants/TrierIntervenants",
        data: {
            colTri: colTri,
            codeDossier: codeDossier,
            versionDossier: versionDossier,
            typeDossier: typeDossier,
            imgTri: img
        },
        success: function (data) {
            $("#divGestionIntervenantsBody").html(data);
            MapElementTableau();
            MiseAJourImagesTri(colTri, img);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request, true);
        }
    });
}

function MiseAJourImagesTri(colTri, img) {
    switch (colTri) {
        case "TypeIntervenant":
            if (img == "tri_asc") {
                $("img[albcontext=TypeIntervenant]").attr("src", "/Content/Images/tri_desc.png");
            }
            else {
                $("img[albcontext=TypeIntervenant]").attr("src", "/Content/Images/tri_asc.png");
            }
            $("img[albcontext=NomIntervenant]").attr("src", "/Content/Images/tri.png");
            break;
        case "NomIntervenant":
            if (img == "tri_asc") {
                $("img[albcontext=NomIntervenant]").attr("src", "/Content/Images/tri_desc.png");
            }
            else {
                $("img[albcontext=NomIntervenant]").attr("src", "/Content/Images/tri_asc.png");
            }
            $("img[albcontext=TypeIntervenant]").attr("src", "/Content/Images/tri.png");
            break;
    }


}

//----------Reininialise les champs de la fenêtre détails
function RazChampsDetails() {
    $("#valFinValidite").val("");
    $("#Denomination").val("");
    $("#valAdresse1").val("");
    $("#valAdresse2").val("");
    $("#valCodePostal").val("");
    $("#valVille").val("");
    $("#valTelephone").val("");
    $("#valEmail").val("");
    $("#Interlocuteur").val("");
    $("#CodeInterlocuteur").val("");
    $("#CodeIntervenant").val("");
}