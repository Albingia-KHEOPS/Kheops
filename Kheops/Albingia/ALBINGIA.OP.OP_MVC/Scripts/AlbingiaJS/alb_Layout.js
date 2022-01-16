function initAlbLayout() {
    initLayout();
    initGenererCP();
}
var globalInit = [];
$(document).ready(function () {
    globalInit.push(initAlbLayout);
    initAlbLayout();
});
//-----------Declaration Variable Globale
/**
 * @param {string} [additionalParams]
 */
function GetAddParams(additionalParams) {

    additionalParams = additionalParams || "";
    const codeAvenant = $("#NumAvenantPage").val() || "";
    const addParamType = $("#AddParamType").val();
    const addParamValue = $("#AddParamValue").val();

    const hasAvenant = codeAvenant != "";
    if (!(hasAvenant && addParamType && addParamValue)) {
        return "";
    }

    const readOnly = (window.isReadonly ? "" : "||IGNOREREADONLY|1");
    const moreParams = (additionalParams.indexOf("||") == 0 && "" || "||") + additionalParams;
    const addParamString = "addParam" + addParamType + "|||" + addParamValue + readOnly + (additionalParams && moreParams || "") + "addParam";
    return addParamString;
}
function GetCurrentIdForOpen() {
    const codeContrat = $("#Offre_CodeOffre").val();
    const versionContrat = $("#Offre_Version").val();
    const type = $("#Offre_Type").val();
    const codeAvenant = $("#NumAvenantPage").val() || "";
    const tabGuid = $("#tabGuid").val();

    const id =
        codeContrat + "_" + versionContrat + "_" + type
        + (codeAvenant == "" ? "" : "_" + codeAvenant + GetAddParams())
        + tabGuid;
    return id;
}
//------------------Transforme un div en bouton
function TransDivBtnAdmin() {
    $("#btnAdmin").button({ text: "Admin" }).click(function () {
    });
}
//------------------Appel l'ouverture de la dialogBox pour une erreur----------------
$.fn.jqDialogErreur = function (divErreur, exception) {
    common.error.show(exception);
};
//------------------Appel de la fonction jqDialogErreur en lui passant le nom du div----------------
$.fn.jqDialogErreurOpen = function (exception) {
    var divErreur = '#GestionErreur';
    $.fn.jqDialogErreur(divErreur, exception);
};
//------------------Serialise les objets d'une form----------------
$.fn.serializeObject = function (onCreate) {
    let obj = {};
    let o = {};
    this.filter("[type='checkbox']").each(function (e) {
        obj[this.name] = this.checked;
    });
    let a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (obj !== undefined) {
                if (obj[this.name] !== undefined) {
                    o[this.name] = obj[this.name];
                }
            }
            else {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            }
        }
        else {
            if (!$.isFunction(onCreate) || !onCreate(this.name, o)) {
                o[this.name] = obj[this.name] ? obj[this.name] : this.value || '';
            }
        }
    });
    return o;
};

//------------Affecte la fonction ouverture backoffice---------------
function AffectBackOffice() {
    $("#BackOffice").live('click', function () {
        OpenPage("/BackOffice");
    });
}
//-----------Affecte lien pour la navigation------------
function AffectNavigation() {
    const prefixBtn = "#btn";
    const prefixInfo = "Info";
    let buttonIds = ["InfoGen", "RsqGar", "Engage", "Cotis", "Fin", "Regule"];
    buttonIds.forEach(function (x) {

        $(prefixBtn + x).live("click", function () {
            let id = x.indexOf(prefixInfo) == 0 ? x : (prefixInfo + x);
            if ($(this).attr("class") == (id + "Selected") || $(this).attr("class") == id) {
                $("span[albArbreName='" + this.id.replace("btn", "") + "']").trigger("click");
                //    let addParamType = $("#AddParamType").val();
                //    let addParamValue = $("#AddParamValue").val();
                //    let addParamString = "addParam" + addParamType + "|||" + addParamValue + (window.isReadonly ? "" : "||IGNOREREADONLY|1") + "addParam";
                //    CommonSaveRedirect($("#" + id + "Path").val() + "/Index/" + $("#Offre_CodeOffre").val() + "_" + $("#Offre_Version").val() + "_" + $("#Offre_Type").val() + $("#tabGuid").val() + addParamString + GetFormatModeNavig($("#ModeNavig").val()));
            }
        });
    });

    $("img[name=SuiviActes]").kclick(function () {
        OpenSuiviActesGestion();
    });
    $("img[name=Connexites]").kclick(function () {
        openPopupConnexites();
    });
    $("img[name=ListeClauses]").kclick(function () {
        OpenListeClauses();
    });

    $("img[name=GestionDocumentContrat]").kclick(function () {
        OpenGestionDocumentContrat();
    });

    $("img[name=ListeIntervenants]").kclick(function () {
        OpenListeIntervenants();
    });

    $("#DblSaisieBandeau").kclick(function () {
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        var addParamString = "addParam" + addParamType + "|||" + addParamValue + (window.isReadonly ? "" : "||IGNOREREADONLY|1") + "addParam";
        var tabGuid = IsInIframe() ? $('#homeTabGuid', window.parent.document).val() : $('#tabGuid').val();
        var codeAvn = $("#NumAvenantPage").val();
        OpenDblSaisie($("#Offre_CodeOffre").val() + "_" + $("#Offre_Version").val() + "_" + $("#Offre_Type").val() + tabGuid + addParamString + GetFormatModeNavig($("#ModeNavig").val()), codeAvn);
    });

    $("#expandArbre").kclick(function () {
        var TopHeight = $("#DivTop").height();
        var BotHeight = $("#DivBot").height();
        var ContainerHeight = $("#CollapseContainer").height() + 13;

        if (!$("#CollapseContainer").is(":visible")) {
            $("#CollapseContainer").fadeToggle("slow", "linear");
            $("#DivTop").animate({ height: TopHeight - ContainerHeight + "px" }, 500); //234px
            $("#DivBot").animate({ height: BotHeight + ContainerHeight + "px" }, 500); //283px
            $("#imgExpandArbre").attr("src", "/Content/Images/expand.png");
            $("#delimArbre").fadeIn();
        }
        else {
            $("#CollapseContainer").fadeToggle("slow", "linear");
            $("#DivTop").animate({ height: TopHeight + ContainerHeight + "px" }, 500); //357px
            $("#DivBot").animate({ height: BotHeight - ContainerHeight + "px" }, 500); //160px
            $("#imgExpandArbre").attr("src", "/Content/Images/collapse.png");
            $("#delimArbre").fadeOut();
        }
    });
}
//----------Affecte lien pour la navigation-------
function AffectLinkNavigation() {
    $("div[albLinkArbre]").kclick(function () {
        let functionName = $(this).attr("albLinkArbre");
        switch (functionName.toLowerCase()) {
            case "opensuividocuments":
                OpenSuiviDocuments(false);
                break;
            case "openconnexites":
                openPopupConnexites();
                break;
            case "openvisulisationquittances":
                OpenVisulisationQuittances('Toutes', isEntete = false);
                break;
            case "openvisualisationobservations":
                OpenVisualisationObservations();
                break;
            case "openvisualisationsuspension":
                OpenVisualisationSuspension();
                break;
            case "openged":
                OpenGED();
                break;
            default:
                if (typeof window[functionName] === "function") {
                    window[functionName]();
                }
                break;
        }
    });
}

//-----------docReady------------
function initLayout() {
    AffectNavigation();
    AffectBackOffice();
    AffectLinkNavigation();
}
//-----------Région Contrat----------

//-----Ouvre la div flottante de visu quittances
function OpenVisulisationQuittances(typeQuittances, isEntete) {
    let selection = recherche.affaires.getClickedResult();
    var codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    var version = $("#Offre_Version").val() || selection.alx;
    if (!codeOffre || !version) {
        var selectRadio = $("input[type=radio][name=RadioRow]:checked");
        if (selectRadio) {
            var infoRows = $(selectRadio).val();
            if (infoRows) {
                codeOffre = infoRows.split("_")[0];
                version = infoRows.split("_")[1];
            }
        }
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Quittance/OpenVisualisationQuittances",
        data: { codeOffre: codeOffre, codeAvn: $("#NumAvenantPage").val(), version: version, typeQuittances: typeQuittances, isEntete: isEntete, modeNavig: $("#ModeNavig").val() },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataVisuListeQuittances").html(data);
            AlbScrollTop();
            common.dom.pushForward("divDataVisuListeQuittances");
            $("#divVisuListeQuittances").show();
            MapElementVisuQuittances();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-----Ouvre la div flottante des assurés additionnels
function OpenAssuAdd(readOnlyDisplay) {
    if (readOnlyDisplay == undefined)
        readOnlyDisplay = false;

    let id = GetCurrentIdForOpen();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AssuresAdditionnels/OpenAssuresAdditionnels",
        data: { id: id, modeNavig: $("#ModeNavig").val() },//, readOnlyDisplay: readOnlyDisplay },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataAssureAdditionnels").html(data);
            AlbScrollTop();
            $("#divAssureAdditionnels").show();
            MapElementAssuAdd();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----Map les éléments de la div flottante des assurés additionnels-----
function MapElementAssuAdd() {
    //gestion de l'affichage de l'écran en mode readonly
    if (window.isReadonly && $("#IsModifHorsAvn").val() === "False") {
        $("img[name=UpdAssRef]").hide();
        $("img[name=DelAssRef]").hide();
        $("img[name=UpdAssNonRef]").hide();
        $("img[name=DelAssNonRef]").hide();
        $("#addAssureNonRef").hide();
        $("#addAssureRef").hide();
        $("#PreneurEstAssureDivF").attr("disabled", "disabled");
    }

    $("#btnFermer").kclick(function () {
        ReactivateShortCut();
        var nbCoassAdd = $("img[name='DelAssRef']").length;
        if (nbCoassAdd > 0) {
            $("#btnOpenAssu").removeClass("backgroundColorWhite").addClass("backgroundColorOrange");
        }
        else {
            $("#btnOpenAssu").removeClass("backgroundColorOrange").addClass("backgroundColorWhite");
        }
        $("#divDataAssureAdditionnels").html('');
        $("#divAssureAdditionnels").hide();
    });

    $("#btnConfirmOkSupprAssure").kclick(function () {
        $("#divMsgConfirmAssure").hide();
        switch ($("#hiddenActionSupprAssure").val()) {
            case "DelAssuRef":
                DeleteAssureRef();
                break;
            case "DelAssuNonRef":
                DeleteAssureNonRef();
                break;
            case "Cancel":
                break;
        }
        $("#hiddenActionSupprAssure").clear();
    });

    $("#btnConfirmCancelSupprAssure").kclick(function () {
        $("#divMsgConfirmAssure").hide();
        $("#hiddenActionSupprAssure").clear();
        $("#DelAssu").clear();
    });

    MapElementAssuRef();
    MapElementAssuNonRef();
}
//-----------Map les éléments des assurés référencés-------------
function MapElementAssuRef() {
    $("#addAssureRef.CursorPointer").kclick(function () {
        OpenAddAssureRef();
    });

    $("img[name=UpdAssRef]").kclick(function () {
        OpenAddAssureRef($(this), action = "U");
    });

    $("img[name=DelAssRef]").die().each(function () {
        $(this).click(function () {
            var splitChar = $("#SplitChar").val();
            var codeAssure = $(this).attr("id").split(splitChar)[1];
            var nameAssure = $("td[name=\"NomAssure" + splitChar + codeAssure + "\"]").text();
            $("#DelAssu").val(codeAssure);
            $("#hiddenActionSupprAssure").val("DelAssuRef");
            $("#idAssureSuppr").html(nameAssure);
            $("#divMsgConfirmAssure").show();

        });
    });

    PreneurEstAssureInit();
    $("#PreneurEstAssureDivF").offOn("change", function () {
        PreneurEstAssureUpdate();
    });
    AlternanceLigne("AssureRefBody", "", false, null);
}

// le chechBox PreneurEstAssure est de type heckBox pour les contrat et de type hidden pour les offres
function PreneurEstAssureInit() {
    var inputType = $("#PreneurEstAssure").attr('type');
    if (inputType == "checkbox") {
        $("#PreneurEstAssureDivF").attr('checked', $('#PreneurEstAssure').is(':checked'));
    }
    else {
        $("#PreneurEstAssureDivF").attr('checked', $('#PreneurEstAssure').val().toUpperCase() == "TRUE");
    }
}

function PreneurEstAssureUpdate() {
    var inputType = $("#PreneurEstAssure").attr('type');
    if (inputType == "checkbox") {
        $("#PreneurEstAssure").attr('checked', $('#PreneurEstAssureDivF').is(':checked'));
    }
    else {
        $("#PreneurEstAssure").val($('#PreneurEstAssureDivF').is(':checked'));
    }
}

//-----------Map les éléments des assurés non référencés-----------
function MapElementAssuNonRef() {
    $("#addAssureNonRef").kclick(function () {
        if ($(this).hasClass("CursorPointer"))
            OpenAddAssureNonRef('');
    });


    $("img[name=UpdAssNonRef]").die().each(function () {
        $(this).click(function () {
            var splitChar = $("#SplitChar").val();
            var idAssu = $(this).attr("id").split(splitChar)[1];
            OpenAddAssureNonRef(idAssu);
        });
    });

    $("img[name=DelAssNonRef]").die().each(function () {
        $(this).click(function () {
            var splitChar = $("#SplitChar").val();
            var idAssure = $(this).attr("id").split(splitChar)[1];
            $("#DelAssu").val(idAssure);
            $("#hiddenActionSupprAssure").val("DelAssuNonRef");
            $("#idAssureSuppr").html(" non désigné ");
            $("#divMsgConfirmAssure").show();
        });
    });

    AlternanceLigne("AssureNonRefBody", "", false, null);
}
//----------Enlève la class requiredFiel---------
function RemoveRequired() {
    $("input[type=text]").removeClass("requiredField");
    $("select").removeClass("requiredField");
}
//--------------Ouvre la div pour l'assuré référencé-------------
function OpenAddAssureRef(elem, action) {
    var splitChar = $("#SplitChar").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();

    var codeAssu = "";
    if (elem != null) {
        codeAssu = elem.attr("id").split(splitChar)[1];
        $("#UpdAssu").val(codeAssu);
    }

    $.ajax({
        type: "POST",
        url: "/AssuresAdditionnels/OpenAddAssureRef",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeAssu: codeAssu, modeNavig: $("#ModeNavig").val() },
        success: function (data) {
            $("#divDataAssure").height(217).html(data);
            AlbScrollTop();
            $("#divAssure").show();
            if (action != "undefined" && action != "" && action == "U") {
                $("#RechercheAvanceePreneurAssuranceAssuAdd").attr("src", "/Content/Images/loupegris.png").removeClass("CursorPointer");
            }
            MapElementDiv();
            MapCommonAutoCompAssure();
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
            codeOldQualite2: codeOldQualite2, codeOldQualite3: codeOldQualite3, oldQualiteAutre: oldQualiteAutre
        },
        success: function (data) {
            $("#divDataAssure").height(190).html(data);
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

function MapElementsQualite() {
    AffectTitleList($("#CodeQualite1"));
    AffectTitleList($("#CodeQualite2"));
    AffectTitleList($("#CodeQualite3"));

    $("#CodeQualite1").offOn("change", function () {
        AffectTitleList($(this));
    });

    $("#CodeQualite2").offOn("change", function () {
        AffectTitleList($(this));
    });

    $("#CodeQualite3").offOn("change", function () {
        AffectTitleList($(this));
    });
}
//---------Map les éléments de la div flottante-----------
function MapElementDiv() {
    $("#btnFancyAnnuler").kclick(function () {
        $("#divAssure").hide();
        $("#divDataAssure").clearHtml();
    });

    $("#btnFancyValider").kclick(function () {
        var modeAssu = $("#ModeAddAssu").val();
        if (modeAssu == "Ref")
            SaveAssureRef();
        if (modeAssu == "NonRef")
            SaveAssureNonRef();
    });


    $("#RechercheAvanceePreneurAssuranceAssuAdd").kclick(function () {
        if ($(this).hasClass('CursorPointer')) {
            var context = $(this).attr("albcontext");
            if (context == undefined)
                context = "";
            OpenRechercheAvanceePreneurAssuranceAssuAdd(context);
        }
    });

    toggleDescription($("#AssureAddCom"));
}

function OpenRechercheAvanceePreneurAssuranceAssuAdd(context) {
    var code = $("#CodeAssure").val();
    var nom = $("#NomAssure").val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/OpenRechercheAvanceePreneurAssurance",
        data: { codePreneur: code, nomPreneur: nom, contexte: context },
        success: function (data) {
            $("#divDataRechercheAvanceePreneurAssuTrans").html(data);
            MapElementsRechercheAvanceeTransPreneurAssu();
            $("#divRechercheAvanceePreneurAssuTrans").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
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
        $("#CodeAssure").clear();
        $("#NomAssure").clear();
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
    var idDesi = $("#idDesi").val();
    var designation = $.trim($("#AssureAddCom").html().replace(/<br>/ig, "\n"));

    if (CheckAssureRef()) {
        ShowCommonFancy("Error", "", "Assuré invalide !", 150, 85, true);
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/AssuresAdditionnels/SaveAssureRef",
        data: {
            codeOffre: codeOffre, version: version, type: type, modeEdit: modeEdit, codeAssu: codeAssure,
            codeQual1: codeQualite1, codeQual2: codeQualite2, codeQual3: codeQualite3, qualAutre: qualiteAutre,
            modeNavig: $("#ModeNavig").val(), codeAvenant: codeAvenant, idDesi: idDesi, designation: designation
        },
        success: function (data) {
            $("#tblAssureRefBody").html(data);
            $("#divDataAssure").clearHtml();
            $("#divAssure").hide();
            $("#UpdAssu").clear();
            MapElementAssuRef();
            if (codeAvenant != "" && codeAvenant != "0") {
                $("#chkAssuModificationAVN").attr("checked", "checked");
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Vérifie les informations de l'assuré référencé------
function CheckAssureRef() {
    var codeAssure = $("#CodeAssure").val();

    if ($("#AssureBase").val() != $("#UpdAssu").val() && (codeAssure == "" || codeAssure == $("#AssureBase").val())) {
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
            if (codeAvenant != "" && codeAvenant != "0") {
                $("#chkAssuModificationAVN").attr("checked", "checked");
            }
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
    var tabGuid = $("#tabGuid").val();
    var modeEdit = $("#ModeEdit").val();
    var codeQualite1 = $("#CodeQualite1").val();
    var codeQualite2 = $("#CodeQualite2").val();
    var codeQualite3 = $("#CodeQualite3").val();
    var qualiteAutre = $("#QualiteAutre").val();
    var codeAvenant = $("#NumAvenantPage").val() != undefined ? $("#NumAvenantPage").val() : "0";

    var idAssureNonRef = $("#IdAssuNonRef").val();

    var codeOldQualite1 = $("td[name=\"Qual1" + splitChar + idAssureNonRef + "\"]").text().split(" - ")[0];
    var codeOldQualite2 = $("td[name=\"Qual2" + splitChar + idAssureNonRef + "\"]").text().split(" - ")[0];
    var codeOldQualite3 = $("td[name=\"Qual3" + splitChar + idAssureNonRef + "\"]").text().split(" - ")[0];
    var oldQualiteAutre = $("img[name=\"QualAutre" + splitChar + idAssureNonRef + "\"]").attr('title');

    if (typeof (oldQualiteAutre) == "undefined")
        oldQualiteAutre = "";

    if (CheckAssureNonRef()) {
        common.dialog.error("Veuillez renseigner au moins une qualité.");
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/AssuresAdditionnels/SaveAssureNonRef",
        data: {
            codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, modeEdit: modeEdit,
            codeQual1: codeQualite1, codeQual2: codeQualite2, codeQual3: codeQualite3, qualAutre: qualiteAutre,
            codeOldQual1: codeOldQualite1, codeOldQual2: codeOldQualite2, codeOldQual3: codeOldQualite3, oldQualAutre: oldQualiteAutre,
            modeNavig: $("#ModeNavig").val(), codeAvenant: codeAvenant
        },
        success: function (data) {
            $("#tblAssureNonRefBody").html(data);
            $("#divDataAssure").clearHtml();
            $("#divAssure").hide();
            MapElementAssuNonRef();
            if (codeAvenant != "" && codeAvenant != "0") {
                $("#chkAssuModificationAVN").attr("checked", "checked");
            }
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
    var codeAvenant = $("#NumAvenantPage").val() != undefined ? $("#NumAvenantPage").val() : "";

    var codeOldQualite1 = $("td[name=\"Qual1" + splitChar + idAssu + "\"]").text().split(" - ")[0];
    var codeOldQualite2 = $("td[name=\"Qual2" + splitChar + idAssu + "\"]").text().split(" - ")[0];
    var codeOldQualite3 = $("td[name=\"Qual3" + splitChar + idAssu + "\"]").text().split(" - ")[0];
    var oldQualiteAutre = $("img[name=\"QualAutre" + splitChar + idAssu + "\"]").attr('title');

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
            if (codeAvenant != "" && codeAvenant != "0") {
                $("#chkAssuModificationAVN").attr("checked", "checked");
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//---------Fin Région Contrat--------

//------Region Actes de gestion-------

//-----Ouvre la div flottante du suivi des actes de gestion

function OpenSuiviActesGestion() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/SuiviActesGestion/OpenSuiviActesGestion",
        data: { codeOffre: codeOffre, version: version, type: type },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataActeGestion").html(data);
            AlbScrollTop();
            $("#divActeGestion").show();
            MapElementActeGestion();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----Map les éléments de la div flottante des actes de gestion-----
function MapElementActeGestion() {
    $("#btnFermer").kclick(function () {
        ReactivateShortCut();
        $("#divDataActeGestion").html('');
        $("#divActeGestion").hide();
    });
    AlternanceLigne("ActesGestionBody", "", false, null);

    $("#Affichages").offOn("change", function () {
        //affichage de toutes les lignes du tableau
        $("#tblActesGestionBody tr").show();
        var affichage = $(this).val();
        //filtre sur le type d'affichage
        if (affichage == "A") {
            $("#tblActesGestionBody tr[albtypeaffichage!='" + affichage + "']:visible").hide();
        }
    });
    $("#Affichages").change();

    $("#btnFiltrerActe").kclick(function () {
        FiltrerActeGestion($("#DateTraitDeb").val(), $("#DateTraitFin").val(), $("#Utilisateurs").val(), $("#Traitements").val());
    });

    $("#btnResetActe").kclick(function () {
        $("#DateTraitDeb").clear();
        $("#DateTraitFin").clear();
        $("#Utilisateurs").clear();
        $("#Traitements").clear();

        FiltrerActeGestion($("#DateTraitDeb").val(), $("#DateTraitFin").val(), $("#Utilisateurs").val(), $("#Traitements").val());
    });

    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    AffectDateFormat();
}
//---------Filtre l'affichage des actes de gestion----------
function FiltrerActeGestion(dateDeb, dateFin, user, traitement) {

    $("#DateTraitDeb").removeClass("requiredField");
    $("#DateTraitFin").removeClass("requiredField");

    if (dateDeb != "" && dateFin != "" && getDate(dateDeb) > getDate(dateFin)) {
        common.dialog.error("Erreur de cohérence de dates");
        $("#DateTraitDeb").addClass("requiredField");
        $("#DateTraitFin").addClass("requiredField");
        return false;
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/SuiviActesGestion/Filtrer",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            dateDeb: dateDeb, dateFin: dateFin, user: user, traitement: traitement
        },
        success: function (data) {
            $("#divActesGestionBody").html(data);
            AlternanceLigne("ActesGestionBody", "", false, null);
            $("#Affichages").change();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//------Fin Région Actes de gestion
//------Région Connexités
//-----Ouvre la div flottante de Connexités
function OpenConnexites(tabGuidParam, idParam, forceReadonly) {
    let selection = recherche.affaires.getClickedResult();
    var typeAffichage = $("#typAffichage").val();
    var codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    var version = $("#Offre_Version").val() || selection.alx;
    var type = selection.typ || $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val() || selection.avn;
    var id = "";
    var tabGuid = "";

    if (tabGuidParam != undefined)
        tabGuid = tabGuidParam;
    else
        tabGuid = $("#tabGuid").val();
    if (idParam != undefined)
        id = idParam + tabGuid;
    else
        id = codeOffre + "_" + version + "_" + type + "_" + codeAvn + tabGuid + "addParam" + $("#AddParamType").val() + "|||" + $("#AddParamValue").val() + "addParam";
    if (forceReadonly == undefined)
        forceReadonly = "false";

    /* récupération des infos du bandeau recherche */
    if (codeOffre == "" || version == "") {
        codeOffre = $("#CodeAffaireRech").val();
        version = $("#VersionRech").val();
        type = $("#TypeAffaireRech").val();
        codeAvn = $("#CodeAvnRech").val();
        id = codeOffre + "_" + version + "_" + type + "_" + codeAvn;
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/LoadConnexites",
        data: { id: id, forceReadonly: forceReadonly },
        success: function (data) {
            /*desactivation des raccourcis claviers de l'écran appelant*/
            DesactivateShortCut();
            $("#divDataConnexites").html(data);
            AlbScrollTop();
            $("#divConnexites").show();
            MapElementConnexites();
            if (typeAffichage == undefined || typeAffichage == "") {
                $("#commentairesEng").html($("#commentairesEng").html().replace(/&lt;br&gt;/ig, "\n"));
                toggleDescription($("#commentairesEng"));
            }
            else {
                $("#commentairesEngagement").html($("#commentairesEngagement").html().replace(/&lt;br&gt;/ig, "\n"));
                toggleDescription($("#commentairesEngagement"));
            }

            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

let openPopupConnexites = function (tabGuidParam, idParam, forceReadonly) {
    //var typeAffichage = $("#typAffichage").val();
    let codeOffre = $("#Offre_CodeOffre").val();
    let version = $("#Offre_Version").val();
    let type = $("#Offre_Type").val();
    let codeAvn = $("#NumAvenantPage").val();
    let id = "";
    let tabGuid = "";

    if (tabGuidParam != undefined) {
        tabGuid = tabGuidParam;
    }
    else {
        tabGuid = $("#tabGuid").val().replace(/tabGuid/g, "");
    }

    if (idParam != undefined) {
        id = idParam + tabGuid;
    }
    else {
        id = codeOffre + "_" + version + "_" + type + "_" + codeAvn + tabGuid + "addParam" + $("#AddParamType").val() + "|||" + $("#AddParamValue").val() + "addParam";
    }

    if (forceReadonly == undefined) {
        forceReadonly = "false";
    }

    /* récupération des infos du bandeau recherche */
    if (codeOffre == "" || version == "") {
        codeOffre = $("#CodeAffaireRech").val();
        version = $("#VersionRech").val();
        type = $("#TypeAffaireRech").val();
        codeAvn = $("#CodeAvnRech").val();
        id = codeOffre + "_" + version + "_" + type + "_" + codeAvn;
    }
    var readonly = forceReadonly.toLowerCase() == "true";
    affaire.showConnexites("Connexités", { modeNavig: 'S', isReadonly: readonly, affair: { codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid } });
};

function OpenConnexitesEng(tabGuidParam, idParam, forceReadonly) {

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var id = "";
    var tabGuid = "";


    if (tabGuidParam != undefined)
        tabGuid = tabGuidParam;
    else
        tabGuid = $("#tabGuid").val();
    if (idParam != undefined)
        id = idParam + tabGuid;
    else
        id = codeOffre + "_" + version + "_" + type + "_" + codeAvn + tabGuid + "addParam" + $("#AddParamType").val() + "|||" + $("#AddParamValue").val() + "addParam";
    if (forceReadonly == undefined)
        forceReadonly = "false";

    if (codeOffre == "" || version == "") {
        var tId = $("td[name=clickableCol]").attr("albContext");
        codeOffre = tId.split("_")[0];
        version = tId.split("_")[1];
        type = tId.split("_")[2];
        codeAvn = tId.split("_")[3];
        id = codeOffre + "_" + version + "_" + type + "_" + codeAvn;
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/LoadConnexitesEng",
        data: { id: id, forceReadonly: forceReadonly },
        success: function (data) {
            /*desactivation des raccourcis claviers de l'écran appelant*/
            DesactivateShortCut();
            $("#divDataConnexitesEng").html(data);
            AlbScrollTop();
            $("#divConnexitesEng").show();
            MapElementConnexites();


            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function OpenDetails() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var modeNavig = $("#ModeNavig").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Connexite/LoadDetailsConnexites",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, modeNavig: modeNavig },
        success: function (data) {
            $("#divDataDetailsConnexites").html(data);
            $("#divDetailsConnexites").show();
            MapElementDetailsConnexites();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function MapElementDetailsConnexites() {
    $("#btnFermer").kclick(function () {
        $("#divDataDetailsConnexites").html('');
        $("#divDetailsConnexites").hide();
    });
}
//
//------Fin Région Connexités
//Région Visu historique
function OpenVisuHistorique(codeOffre, version, type, codeAvn) {
    if (codeOffre != undefined && codeOffre != "" && codeOffre.indexOf("_") > -1) {
        var tId = codeOffre.split("_");
        codeOffre = tId[0];
        version = tId[1];
        type = tId[2];
        codeAvn = tId[3];
    }


    codeOffre = codeOffre == undefined ? $("#Offre_CodeOffre").val() : codeOffre;
    version = version == undefined ? $("#Offre_Version").val() : version;
    type = type == undefined ? $("#Offre_Type").val() : type;
    codeAvn = codeAvn == undefined ? $("#NumAvenantPage").val() : codeAvn;
    var id = "";
    id = codeOffre + "_" + version + "_" + type + "_" + codeAvn;

    /* récupération des infos du bandeau recherche */
    if (codeOffre == "" || version == "") {
        codeOffre = $("#CodeAffaireRech").val();
        version = $("#VersionRech").val();
        type = $("#TypeAffaireRech").val();
        codeAvn = $("#CodeAvnRech").val();
        id = codeOffre + "_" + version + "_" + type + "_" + codeAvn;
    }


    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Historique/OpenHistorique",
        data: { id: id },
        success: function (data) {
            $("#divDataVisuHistorique").html(data);
            AlbScrollTop();
            common.dom.pushForward("divDataVisuHistorique");
            $("#divFullScreenVisuHistorique").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function OpenVisuAlertes() {
    if (!$("#divAlertesAvenant").exists()) {
        return;
    }
    common.$ui.showDialog($("#divAlertesAvenant"), "", "Informations Affaire", { width: 350, height: "auto" }, null, null, "dialog-fix", true);
}

//Fin Région visu historique

//-------Formate les input/span des valeurs----------
function FormatNumericValue() {
    common.autonumeric.applyAll('init', 'numeric', '', null, null, '99999999999', '0');

}

//------Fin Région visu quittances






//------Region visu observations

function OpenVisualisationObservations() {
    let selection = recherche.affaires.getClickedResult();
    let codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    let version = $("#Offre_Version").val() || selection.alx;
    let type = selection.typ || $("#Offre_Type").val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/OpenVisualisationObservations",
        data: { codeOffre: codeOffre, version: version, type: type },
        success: function (data) {
            $("#divDataVisuObservations").html(data);
            AlbScrollTop();
            common.dom.pushForward("divDataVisuObservations");
            $("#divFullScreenVisuObservations").show();
            MapElementsVisualisationObservations();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function MapElementsVisualisationObservations() {
    $("#btnFermerVisualisationObservations").kclick(function () {
        if (!window.sessionStorage) return;
        var filter = JSON.parse(window.sessionStorage.getItem('recherche_filter'));
        if (!filter) return;
        $("#Offre_CodeOffre").val(filter.CodeOffre);
        $("#Offre_Version").val(filter.version);
        $("#Offre_Type").val(filter.TypeContrat);
        $("#divFullScreenVisuObservations").hide();
    });
}

//-----Fin région visu observations

//-----Début région visu suspension
function OpenVisualisationSuspension() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var numAve = $("#NumAvenantPage").val();

    /* récupération des infos du bandeau recherche */
    if (codeOffre == "" || version == "") {
        codeOffre = $("#CodeAffaireRech").val();
        version = $("#VersionRech").val();
        type = $("#TypeAffaireRech").val();
        numAve = $("#CodeAvnRech").val();
    }


    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Suspension/OpenVisualisationSuspension",
        data: { codeOffre: codeOffre, version: version, type: type, numAve: numAve },
        success: function (data) {
            $("#divDataVisuSuspension").html(data);
            AlbScrollTop();
            $("#divFullScreenVisuSuspension").show();
            MapElementsVisualisationSuspension();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


function MapElementsVisualisationSuspension() {
    toggleDescription($("#SuspvCode"));

    $("#btnSearch").die();

    $("#btnFermerVisualisationSuspension").kclick(function () {
        $("#divFullScreenVisuSuspension").hide();
        $("#divDataVisuSuspension").clearHtml();
    });

    if ($("#isBackOffice").val() != 'True')
        $("td.tdBackOfficeOnly").hide();
    else
        $("td.tdBackOfficeOnly").show();

    AlternanceLigne("Susp", "", false, null);
}





function ToggleBackOfficeMenu() {

    $("#BackOfficeMenu").click(function () {
        var position = $("#BackOfficeMenu").offset();
        $("#LayoutArbre").css({ 'position': 'absolute', 'top': position.top + 35 + 'px', 'left': position.left + 'px' }).toggle('blind');
    });
}

//-----Fin région visu suspension

//------Imprime le document------
function initGenererCP() {
    //wCPDocType

    $("#genererCP").click(function () {
        ShowLoading();
        var codeOffre = $("#Offre_CodeOffre").val() + "_" + $("#Offre_Version").val() + "_" + $("#Offre_Type").val();
        GetFileNameDocument($("#wCPDocType").val(), codeOffre, false, "P");
        CloseLoading();
    });
}
//-----Région Liste des clauses
//-----Ouvre la div flottante de la liste des clauses
function OpenListeClauses() {
    ShowLoading();
    
    let selection = recherche.affaires.getClickedResult();
    let CodeOffreAll = "";
    let CodeOffre = "";
    let VersionOffre = "";
    let Type = "";
    let CodeAvn = "";
    let TabGuid = "";
    let ModeNavig = "";
    let ActeGestion = "";
    if ($("#Offre_CodeOffre").val() != "") {
         CodeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
         VersionOffre = $("#Offre_Version").val() || selection.alx;
         Type = selection.typ || $("#Offre_Type").val();
         CodeAvn = $("#NumAvenantPage").val() || selection.avn;
         TabGuid = $("#tabGuid").val();
         ModeNavig = $("#ModeNavig").val();
         ActeGestion = $("#ActeGestion").val();
    } else {
         CodeOffreAll = $("#Offre_CodeOffre_S").val().split('-');
         CodeOffre = CodeOffreAll[0].trim();
         VersionOffre = CodeOffreAll[1].trim();
        Type = $("#Offre_Type_S").val() == "Offre" ? "O":"P";
         CodeAvn = $("#NumAvenantPage_S").val() || selection.avn ;
         TabGuid = $("#tabGuid").val();
         ModeNavig = $("#ModeNavig").val();
         ActeGestion = $("#ActeGestion").val();
    }
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/OpenListeClauses",
        data: {
            codeOffre: CodeOffre,
            version: VersionOffre,
            type: Type ,
            codeAvn: CodeAvn,
            tabGuid: TabGuid ,
            modeNavig: ModeNavig ,
            acteGestion: ActeGestion
        },
        success: function (data) {
            if (data) {
                DesactivateShortCut();
                $("#divDataListeClauses").html(data);
                common.dom.pushForward("divDataListeClauses");
                $("#divListeClauses").show();
                CloseLoading();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----Fin Région Liste des clauses

//-----Région Clausier
function OpenClausier() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/GetListClausier",
        success: function (data) {
            $("#divDataClausierRech").html(data);
            $("#divClausierRech").show();
            MapElementClausierRech();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


function MapElementClausierRech() {
    $("#btnCloseClausier").kclick(function () {
        $("#divDataClausierRech").clearHtml();
        $("#divClausierRech").hide();
    });
    $("#btnRechercherClausierSearch").kclick(function () {
        var pleinEcran = false;
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/Clausier/Recherche",
            data: {
                libelle: $("#Libelle").val(),
                motCle1: $("#MotCle1").val(),
                motCle2: $("#MotCle2").val(),
                motCle3: $("#MotCle3").val(),
                sequence: $("#Sequence").val(),
                rubrique: $("#Rubrique").val(),
                sousrubrique: $("#SousRubrique").val(),
                selectionPossible: $("#SelectionPossible").val(),
                modaliteAffichage: $("#ModaliteAffichage").val(),
                date: $("#Date").val()
            },
            success: function (data) {
                CloseLoading();
                $("#divFullScreen").hide();
                $("#divDataFullScreen").html('');
                $("#bandeauPleinEcran").hide();
                $("#divBodyClausier").removeClass("fullscreenHeight");
                $("#dvRechercheGeneral").show();
                $("#dvRechercheGeneral").html(data);
                AlternanceLigne("BodyClausier");

                $("#dvRechercheGeneral").off("click", ".lnkDetail").on("click", "lnkDetail", function () {
                    GetFileNameDocument($("#wClauseDocType").val(), $(this).attr("id").replace('lnkDetail_', ''), false, "V");
                });
                ViualisationClause();

            }
        });
    });

    $("#Rubrique").offOn("change", function () {
        AffectTitleList($(this));
        $.ajax({
            type: "POST",
            url: "/Clausier/GetSousRubriques",
            data: { codeRubrique: $(this).val() },
            success: function (data) {
                $("#SousRubriqueDiv").html(data);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });

    $("#SousRubrique").offOn("change", function () {
        AffectTitleList($(this));
        $.ajax({
            type: "POST",
            url: "/Clausier/GetSequences",
            data: { codeSousRubrique: $(this).val(), codeRubrique: $("#Rubrique").val() },
            success: function (data) {
                $("#SequenceDiv").html(data);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });

    MapSorts();

}

function MapSorts() {

    let $pos = -1;
    let $dirAsc = true;

    if (MapSorts.m) MapSorts.m.disconnect();

    $("#tblHeaderClausier th.sortable").off("click");
    $("#tblHeaderClausier th.sortable").kclick(sortClausier);

    var m = new MutationObserver(function () { requestAnimationFrame(sortClausier); return true; });

    m.observe(document.getElementById("dvRechercheGeneral"), { childList: true });

    MapSorts.m = m;

    function sortClausier() {
        if (this instanceof HTMLElement) {
            $this = $(this);
            let npos = -1;
            $this.parent().children().each(function (i, x) { if (x == $this[0]) npos = i; });

            $dirAsc = (npos != $pos) || !$dirAsc;
            $pos = npos;
        }
        if ($pos == -1) return;

        const compare = compareLines($pos, $dirAsc);

        $("#tblHeaderClausier th.sortable").removeClass("asc").removeClass("desc");
        $("#tblHeaderClausier th.sortable:nth-child(" + ($pos + 1) + ")").addClass($dirAsc ? "asc" : "desc");

        let dataTable = document.getElementById("tblBodyClausier");
        let tBody = dataTable.tBodies[0];
        let lines = Array.from(tBody.children);

        lines.sort(compare);
        // no need to remove before adding !
        lines.map(function (x) { tBody.appendChild(x); });
        AlternanceLigne("BodyClausier");
    }

    const collator = new Intl.Collator('fr', { numeric: true, sensitivity: 'base' });
    function compareLines(i, asc) {
        return function (a, b) {
            let ret = 0;
            const t1 = a.children[i] && a.children[i].innerText || "";
            const t2 = b.children[i] && b.children[i].innerText || "";

            ret = collator.compare(t1, t2);
            return asc ? ret : -ret;
        };
    }
}


//-----Fin Région Liste des clauses


//-----Région gestion des documents contrat
//-----Ouvre la div flottante de la liste des documents
function OpenGestionDocumentContrat() {
    ShowLoading();
    var modeNavig = "modeNavig" + $("#ModeNavig").val() + "modeNavig";
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var id = "";

    var tabGuid = $("#tabGuid").val();
    id = codeOffre + "_" + version + "_" + type + tabGuid + modeNavig;

    $.ajax({
        type: "POST",
        url: "/GestionDocumentContrat/OpenGestionDocumentContrat",
        data: {
            id: id
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDocumentContrat").show();
            $("#divDataDocumentContrat").html(data);
            MapElementGestionDocumentContrat();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function OpenPiecesJointes(idDoc) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionDocumentContrat/GetListePiecesJointesDocument",
        data: {
            idDocument: idDoc
        },
        success: function (data) {
            $("#divDocumentContratPieceJointe").show();
            $("#divDataDocumentContratPieceJointe").html(data);
            MapElementGestionDocumentContratPieceJointe();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-----Région Liste des intervenants
function OpenListeIntervenants() {
    ShowLoading();
    const id = GetCurrentIdForOpen();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionIntervenants/OpenGestionIntervenants",
        data: {
            id: id
        },
        success: function (data) {
            $("#divGestionIntervenants").show();
            $("#divDataGestionIntervenants").html(data);
            MapElementGestionDocumentContratPieceJointe();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function MapElementGestionDocumentContrat() {
    $("#btnFermerGestionDocument").kclick(function () {
        ReactivateShortCut();
        $("#divDataDocumentContrat").html('');
        $("#divDocumentContrat").hide();
    });

    MapElementGestionDocumentContratLigne();

    $("#Filtre_TypeDocument").offOn("change", function () {
        FiltrerListeDocuments();
    });

    $("#Filtre_LotEdition").offOn("change", function () {
        FiltrerListeDocuments();
    });

}

function MapElementGestionDocumentContratLigne() {
    AlternanceLigne("GestionDocBody", "", true, null);

    $("td[class='tdDocPJ navig']").each(function () {
        $(this).click(function () {
            var idDoc = $(this).attr("id").split("_")[1];
            OpenPiecesJointes(idDoc);
        });
    });
}

function MapElementGestionDocumentContratPieceJointe() {
    //TODO : map ouverture fichiers
    $("#btnFermerGestionDocPieceJointe").kclick(function () {
        $("#divDataDocumentContratPieceJointe").html('');
        $("#divDocumentContratPieceJointe").hide();
    });

    AlternanceLigne("DocPieceJointeBody", "", true, null);

}

function FiltrerListeDocuments() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionDocumentContrat/GetListeDocumentsFiltree",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            filtreTypeDocument: $("#Filtre_TypeDocument").val(),
            filtreLot: $("#Filtre_LotEdition").val()
        },
        success: function (data) {
            $("#tblGestionDocBody").html(data);
            AlternanceLigne("GestionDocBody", "", true, null);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----Fin Région gestion des documents contrat



//-----Fin Région informations spécifiques risque

function OpenModif() {
    var addParam = "addParam" + $("#AddParamType").val() + "|||" + $("#AddParamValue").val().replace("VALIDATION|1", "") + "addParam";
    if (addParam == "addParamAVN|||addParam")
        addParam = "";

    $.ajax({
        type: "POST",
        url: "/CommonNavigation/OpenModif",
        data: {
            codeAffaire: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvt: $("#NumAvenantPage").val(),
            addParam: addParam, tabGuid: $("#tabGuid").val(), modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


//#region Gestion Documents Joints
//-------Ouverture de la div flottante des documents joints---------
function OpenNavigationDocuments() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    //var modeNavig = GetFormatModeNavig($("#ModeNavig").val());
    var modeNavig = $("#ModeNavig").val();
    var codeAvn = $("#NumAvenantPage").val();

    $.ajax({
        type: "POST",
        url: "/CommonNavigation/OpenDocumentsJoints",
        data: { codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, modeNavig: modeNavig, codeAvn: codeAvn },
        success: function (data) {
            $("#divDocJoints").show();
            $("#divDataDocJoints").html(data);
            MapElementDocsJoints();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Map les éléments des documents joints------
function MapElementDocsJoints() {
    $("#btnFermerDocJoints").kclick(function () {
        $("#divDocJoints").hide();
        $("#divDataDocJoints").clearHtml();
    });

    AlternanceLigne("DocumentsJointsBody", "", false, null);

    $("#btnAddDoc").kclick(function () {
        OpenEditionDocsJoints();
    });

    $("img[name='editDocJoint']").kclick(function () {
        var id = $(this).attr("albParam");
        OpenEditionDocsJoints(id);
    });

    $("img[name='delDocJoint']").kclick(function () {
        var id = $(this).attr("albParam");
        $("#hiddenActionDocJoint").val("DeleteDocJoint");
        $("#hiddenIdDocJoint").val(id);
        $("#divMsgConfirmDocJoint").show();
        $("#msgConfirmDeleteDocJoint").show();
    });

    $("#btnConfirmOkDocJoint").kclick(function () {
        var actionConfirm = $("#hiddenActionDocJoint").val();
        switch (actionConfirm) {
            case "CancelDocJoint":
                $("#divMsgConfirmDocJoint").hide();
                $("#divAddDocument").hide();
                $("#divDataAddDocument").clearHtml();
                break;
            case "DeleteDocJoint":
                DeleteDocsJoints($("#hiddenIdDocJoint").val());
                break;
        }
        $("#hiddenActionDocJoint").clear();
        $("#msgConfirmDeleteDocJoint").hide();
        $("#msgConfirmCancelDocJoint").hide();
    });
    $("#btnConfirmCancelDocJoint").kclick(function () {
        $("#hiddenActionDocJoint").clear();
        $("#divMsgConfirmDocJoint").hide();
        $("#msgConfirmDeleteDocJoint").hide();
        $("#msgConfirmCancelDocJoint").hide();
    });

    $("th[name='headerTriDoc']").each(function () {
        $(this).unbind('click');
        $(this).bind('click', function () {
            ChangeOrderDoc($(this).attr('albcontext'));
        });
    });

    $("div[name=linkVisu][albcontext=docJoint]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            ShowLoading();
            var pathFile = $(this).attr("albdocfullpath");
            OpenDirectWordDocument(pathFile);
            CloseLoading();
        });
    });
}
//-------Change le tri de la liste des documents joints----------
function ChangeOrderDoc(field) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = GetFormatModeNavig($("#ModeNavig").val());

    var orderField = $("#orderFieldDoc").val();
    var orderType = $("#orderTypeDoc").val();
    var codeAvn = $("#NumAvenantPage").val();

    if (orderField == field) {
        if (orderType == "DESC")
            orderType = "ASC";
        else
            orderType = "DESC";
    }

    $.ajax({
        type: "POST",
        url: "/CommonNavigation/ChangeOrderDoc",
        data: {
            codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, modeNavig: modeNavig,
            orderField: field, orderType: orderType, codeAvn: codeAvn
        },
        success: function (data) {
            ReloadListDocsJoints(data);
            $("#orderFieldDoc").val(field);
            $("#orderTypeDoc").val(orderType);
            ChangeImgOrder(field, orderType);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Change les images des colonnes de tri------
function ChangeImgOrder(field, orderType) {
    $(".imageTri").attr('src', '/Content/Images/tri.png');
    if (orderType == "ASC")
        $(".imageTri[albcontext='" + field + "']").attr('src', '/Content/Images/tri_asc.png');
    if (orderType == "DESC")
        $(".imageTri[albcontext='" + field + "']").attr('src', '/Content/Images/tri_desc.png');
}
//-------Ouvre la div flottante de l'ajout/édition de documents joints---------
function OpenEditionDocsJoints(id) {
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/OpenEditionDocsJoints",
        data: { idDoc: id },
        success: function (data) {
            $("#divAddDocument").show();
            $("#divDataAddDocument").html(data);
            MapElementAddDocsJoints(id);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------------------
function MapIframeElementDocJoint() {
    $("#btnAddCancelDocJoints").kclick(function () {
        $("#hiddenActionDocJoint").val("CancelDocJoint");
        $("#divMsgConfirmDocJoint").show();
        $("#msgConfirmCancelDocJoint").show();
    });

    $("#btnAddSaveDocJoints").kclick(function () {
        SaveDocsJoints();
    });
    $("#UploadFile").kclick(function () {
        var typeDoc = $("#TypeDoc").val();
        if (typeDoc == "INTER")
            OpenListDocInter();
        else {
            $('#iframeDocJoint').contents().find("#fileUpd").click();
        }

    });

    $("#TypeDoc").offOn("change", function () {
        ReloadPathFile();
    });
}
//--------Map les éléments de l'ajout de document joint------------
function MapElementAddDocsJoints(id) {
    if (id == undefined) {
        var position = $("#divMoreInfo").position();
        $("#fileUpd").css({ "position": "absolute", "top": position.top + "px", "left": position.left + "px" });
    }
}
//--------Sauvegarde le document joint------------
function SaveDocsJoints() {
    $(".requiredField").removeClass("requiredField");
    let bError = false;
    if ($("#TypeDoc").val() == "") {
        $("#TypeDoc").addClass("requiredField");
        bError = true;
    }
    if ($("#Titre").val() == "") {
        $("#Titre").addClass("requiredField");
        bError = true;
    }
    if ($("#Fichier").val() == "") {
        $("#Fichier").addClass("requiredField");
        bError = true;
    }

    if (!bError) {
        ShowLoading();
        const iframe = $('#iframeDocJoint').contents();
        iframe.find("#docCodeOffre").val($("#Offre_CodeOffre").val());
        iframe.find("#docVersIonOffre").val($("#Offre_Version").val());
        iframe.find("#docTypeOffre").val($("#Offre_Type").val());
        iframe.find("#docIdAdd").val($("#DocIdAdd").val());
        iframe.find("#docTypeDoc").val($("#TypeDoc").val());
        iframe.find("#docTitleDoc").val($("#Titre").val());
        iframe.find("#docReference").val($("#Reference").val());
        iframe.find("#docChemin").val($("#Chemin").val());
        iframe.find("#docTabGuid").val($("#tabGuid").val());
        iframe.find("#docModeNavig").val($("#ModeNavig").val());
        iframe.find("#docActeGestion").val($("#ActeGestion").val());
        iframe.find("#docNumAvenant").val($("#NumAvenantPage").val());
        iframe.find("#docFileName").val($("#Fichier").val());

        //iframe.find("#submitAddSaveDocJoints").click();
        iframe.find("#File").submit();

    }

}
//------Recharge le tableau des documents joints-------
function UploadDocJoints(isload) {
    CloseLoading();
    if (isload == true)
        return;
    var error = $("#frmUploadedFile").contents().find("#__ExceptionMessage");
    if (error.length > 0) {
        common.error.show(error.html());
        return;
    }
    var codeAvn = $("#NumAvenantPage").val();
    $.ajax({
        url: "/CommonNavigation/ReloadDoc",
        type: "POST",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            tabGuid: $("#tabGuid").val(), modeNavig: $("#ModeNavig").val(), codeAvn: codeAvn
        },
        success: function (data) {
            ReloadListDocsJoints(data);
            $("#divAddDocument").hide();
            $("#divDataAddDocument").clearHtml();
        },
    });
}

//------Supprime le document joint-------
function DeleteDocsJoints(id) {
    var splitCharHtml = $("#SplitHtmlChar").val();
    var codeAvn = $("#NumAvenantPage").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/DeleteDocsJoints",
        data: {
            idDoc: id, codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            docName: $("input[id='DocumentJointNom" + splitCharHtml + id + "']").val(), docPath: $("input[id='DocumentJointChemin" + splitCharHtml + id + "']").val(),
            tabGuid: $("#tabGuid").val(), modeNavig: $("#ModeNavig").val(), codeAvn: codeAvn
        },
        success: function (data) {
            ReloadListDocsJoints(data);
            CloseLoading();
            $("#hiddenActionDocJoint").clear();
            $("#divMsgConfirmDocJoint").hide();
            $("#msgConfirmDeleteDocJoint").hide();
            $("#msgConfirmCancelDocJoint").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Recharge la partie liste des documents joints------
function ReloadListDocsJoints(data) {
    $("#divDocumentsJointsBody").html(data);
    MapElementDocsJoints();
}
//-------Charge le chemin selon la typologie de document-------
function ReloadPathFile() {
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/ReloadPathFile",
        data: { typologie: $("#TypeDoc").val() },
        success: function (data) {
            if (data == "") {
                common.dialog.error("Aucuns chemins disponibles pour cette typologie");
                $("#TypeDoc").clear();
                return false;
            }
            $("#Chemin").val(data);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Ouvre la liste des documens de type Intercalaire-----
function OpenListDocInter() {
    ShowLoading();
    var branche = $("#Offre_Branche").val();
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/OpenListDocsInter",
        data: { branche: branche },
        success: function (data) {
            $("#divDataDocumentInter").html(data);
            $("#divDocumentInter").show();
            MapElementDocInter();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Map les éléments de la div des documents intercalaires----
function MapElementDocInter() {
    AlternanceLigne("BodyListDoc", "", false, null);

    $("#btnCancelIntercalaire").kclick(function () {
        $("#divDataDocumentInter").clearHtml();
        $("#divDocumentInter").hide();
    });
    $("#btnSaveIntercalaire").kclick(function () {
        var selFile = $("input[type='radio'][name='chkDocInter']:checked").attr("albfullname").replace($("#pathInter").val(), "");
        $("#Fichier").val(selFile);
        $("#divDataDocumentInter").clearHtml();
        $("#divDocumentInter").hide();
    });
}
//#endregion Gestion Documents Joints----

//#region Suivi Documents

function OpenSuiviDocuments(warning) {
    ShowLoading();
    var tabGuid = $("#tabGuid").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = codeOffre != "" ? $("#Offre_Type").val() : "";
    var codeAvenant = $("#NumAvenantPage").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var situation = "";
    var readOnly = $("#OffreReadOnly").val();
    if (codeOffre != "") {
        $.ajax({
            type: "POST",
            url: "/SuiviDocuments/OpenViewDocBis",
            data: {
                displayType: warning ? "GEN" : "AFF",
                numAffaire: warning ? "" : codeOffre, version: warning ? "" : version, type: warning ? "" : type, avenant: warning ? "" : codeAvenant, userName: warning ? $("#NameUser").val() : "", situation: situation,
                modeNavig: $("#ModeNavig").val(), readOnly: readOnly, warning: warning
            },
            success: function (data) {
                if (data != "") {
                    $("#divDataSuiviGestionDocuments").html(data);
                    $("#divSuiviGestionDocuments").show();
                    MapElementSuiviDoc();
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    else {
        OpenGestionDoc("GEN");
    }
}
//---------Ouvre le module de GED------------
function OpenGED() {
    ShowLoading();
    var codeAffaire = $("#Offre_CodeOffre").val();
    var version = $("Offre_Version").val();
    var type = $("#Offre_Type").val();
    var userName = $("#NameUser").val();
    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/OpenGED",
        data: {
            codeAffaire: codeAffaire, version: version, type: type, userName: userName
        },
        success: function (data) {
            if (data != "") {
                common.error.showXhr(data);
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(data);
            CloseLoading();
        }
    });
}
//----------Ouvre la vue partielle de suivi des documents------------
function OpenGestionDoc(displayType) {
    ShowLoading();
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var infoRows = $(selectRadio).val();

    var numAffaire = displayType == "AFF" && infoRows != undefined ? infoRows.split("_")[0] : "";
    var version = displayType == "AFF" && infoRows != undefined ? infoRows.split("_")[1] : "";
    var type = displayType == "AFF" && infoRows != undefined ? infoRows.split("_")[2] : "";
    var codeAvenant = displayType == "AFF" && infoRows != undefined ? infoRows.split("_")[3] : 0;
    var userName = displayType == "GEN" ? $("#NameUser").val() : "";
    var situation = "E";

    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/OpenViewDocBis",
        data: {
            displayType: displayType,
            numAffaire: numAffaire, version: version, type: type, avenant: codeAvenant, userName: userName, situation: situation,
            modeNavig: $("#ModeNavig").val(), readOnly: $("#OffreReadOnly").val()
        },
        success: function (data) {
            if (data != "") {
                $("#divDataSuiviGestionDocuments").html(data);
                $("#divSuiviGestionDocuments").show();
                MapElementSuiviDoc();
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Map les éléments principaux de la vue-------
function MapElementSuiviDoc() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    $("input[type='radio'][name='radioType']").each(function () {
        $(this).change(function () {
            ChangeDisplayFiltre();
        });
    });
    $("#dvExpandCollapse").kclick(function () {
        ExpandCollapseRechAvancee();
    });
    $("#btnFiltrer").kclick(function () {
        SearchDoc();
    });
    $("#btnRAZ").kclick(function () {
        ResetFiltre();
    });
    MapListDocuments();
    ChangeSuiviDocAff($("#inFiltreAffichage").val());

    FormatNumericField();

    $("#btnAnnulerSuiviDoc").kclick(function () {
        CloseSuiviDoc();
    });

    $("#btnEditerSuiviDoc").kclick(function () {
        EditSuiviDoc();
    });

    $("#checkAllDoc").offOn("change", function () {
        if ($(this).is(':checked'))
            $("input[name='checkEditLogo']:visible").attr("checked", "checked");
        else {
            $("input[name='checkEditLogo']").removeAttr("checked");
        }
    });

    $("#checkAllDocNoLogo").offOn("change", function () {
        if ($(this).is(':checked')) {
            $("input[name='checkEditNoLogo']:visible").attr("checked", "checked");
        }
        else
            $("input[name='checkEditNoLogo']").removeAttr("checked");


    });

    $("#btnConfirmCancelSuiviDoc").kclick(function () {
        $("#divMsgSuiviDoc").hide();
    });
    $("#btnConfirmGenSuiviDoc").kclick(function () {
        GenerateSuiviDoc();
    });
    $("#btnConfirmOkSuiviDoc").kclick(function () {
        if ($(this).val() == "generate")
            PrintSuiviDoc();
        if ($(this).val() == "reedit")
            ReeditionSuiviDoc();
    });

    $("img[name='pjdoc']").each(function () {
        $(this).click(function () {
            OpenDivPJ($(this).attr("albdocid"));
        });
    });
    MapCommonAutoCompUtilisateur();
    MapCommonAutoCompCourtier();
    ExitPrintDoc();
    ExitAsynchLoadDoc();

    $("#inFiltreAffichage").offOn("change", function () {
        ChangeSuiviDocAff($(this).val());
    });

}
//--------Change le mode d'affichage pour le suivi de documents-----
function ChangeSuiviDocAff(modeAffichage) {
    $("tr[albtypeaff]").hide();
    if (modeAffichage == "A") {
        $("tr[albtypeaff='A']").show();
    }
    else {
        $("tr[albtypeaff]").show();
    }
}
//----------Map les éléments de la liste des documents-----
function MapListDocuments() {
    AlternanceLigne("BodyLstDoc", "", false, null);
    $("#dvBodyLstDoc").bind('scroll', function () {
        $("#dvHeaderLstDoc").scrollLeft($(this).scrollLeft());
    });

    $("td[name='tdLot']").each(function () {
        var lotId = $(this).attr('alblotid');
        var countN = $("td[name='tdSituation'][alblotid='" + lotId + "'][albsituation='N']").length;
        if (countN > 0)
            $(this).addClass("situationRed");
        else {
            countN = $("td[name='tdSituation'][alblotid='" + lotId + "'][albsituation='Z']").length;
            if (countN > 0)
                $(this).addClass("situationRed");
            else {
                countN = $("td[name='tdSituation'][alblotid='" + lotId + "'][albsituation='A']").length;
                if (countN > 0)
                    $(this).addClass("situationBlue");
            }
        }
    });

    $("td[albIdDest]").each(function () {
        $(this).mouseover(function () {
            ShowInfoDestinataire($(this));
        });
        $(this).mouseout(function () {
            $("#divDestinataire").clearHtml().hide();
        });
    });

    $("input[name=checkEditNoLogo]").each(function () {
        $(this).click(function () {
            if ($(this).is(":checked")) {

                var idDoc = $(this).attr("albdocid");
                $("input[name=checkEditLogo][albdocid=" + idDoc + "]").attr("checked", "checked");
            }
            else
                $("#checkAllDocNoLogo").removeAttr("checked");
        });
    });
    $("input[name=checkEditLogo]").each(function () {
        $(this).click(function () {
            if (!$(this).is(":checked")) {
                var idDoc = $(this).attr("albdocid");
                $("input[name=checkEditNoLogo][albdocid=" + idDoc + "]").removeAttr("checked");
                $("#checkAllDoc").removeAttr("checked");
            }
        });
    });

    $("img[name='updDoc']").each(function () {
        $(this).click(function () {
            UpdDocCPCS($(this).attr("albDocId"));
        });
    });
    $("img[name='resfreshDoc']").each(function () {
        $(this).click(function () {
            RefreshDoc($(this).attr("albDocId"));
        });
    });
    $("img[name='pjdoc']").each(function () {
        $(this).click(function () {
            OpenDivPJ($(this).attr("albdocid"));
        });
    });

    $("#PaginationDocPremierePage").kclick(function () {
        SearchDoc(1);
    });

    $("#PaginationDocPrecedent").kclick(function () {
        var currentPage = parseInt($("#PageNumberDoc").val());
        currentPage--;
        SearchDoc(currentPage);
    });

    $("#PaginationDocSuivant").kclick(function () {
        var currentPage = parseInt($("#PageNumberDoc").val());
        currentPage++;
        SearchDoc(currentPage);
    });

    $("#PaginationDocDernierePage").kclick(function () {
        var totalLine = parseInt($("#PaginationTotalDoc").html());
        var currentPage = parseInt($("#PageNumberDoc").val());
        var lastPage = parseInt($("#PaginationEndDoc").html());
        lastPage = Math.ceil(totalLine / (lastPage / currentPage));
        SearchDoc(lastPage);
    });

}
//---------Formate les champs numériques----------
function FormatNumericField() {
    common.autonumeric.apply($("#inFiltreVersion"), "destroy");
    common.autonumeric.apply($("#inFiltreVersion"), "init", "numeric", "", null, null, "9999", "0");
    common.autonumeric.apply($("#inFiltreAvenant"), "destroy");
    common.autonumeric.apply($("#inFiltreAvenant"), "init", "numeric", "", null, null, "999", "-1");
    common.autonumeric.apply($("#inFiltreLot"), "destroy");
    common.autonumeric.apply($("#inFiltreLot"), "init", "numeric", "", null, null, "999999999", "0");
}
//---------Change l'affichage des filtres----------
function ChangeDisplayFiltre() {
    if ($("#radioGen").is(":checked")) {
        $("#dvGenerale").show();
        $("#dvFiltreUser").show();
        $("#dvFiltreTypeAff").show();
        $("#dvFiltreNumOffre").show();
    }
    if ($("#radioAff").is(":checked")) {
        if ($("#imgExpandCollapse").attr("albaction") == "expand") {
            $("#dvGenerale").hide();
            $("#dvFiltreUser").hide();
        }
        $("#dvFiltreTypeAff").hide();
        $("#dvFiltreNumOffre").hide();
    }
}
//---------Dépile/Rempile la div de recherche avancée------
function ExpandCollapseRechAvancee() {
    var nextAction = $("#imgExpandCollapse").attr("albaction");
    switch (nextAction) {
        case "expand":
            $("#imgExpandCollapse").attr("src", "/Content/Images/collapse.png").attr("albaction", "collapse");
            $("#dvRechAvancee").show();
            if ($("#radioAff").is(":checked")) {
                $("#dvGenerale").show();
                $("#dvFiltreUser").show();
            }
            break;
        case "collapse":
            $("#imgExpandCollapse").attr("src", "/Content/Images/expand.png").attr("albaction", "expand");
            $("#dvRechAvancee").hide();
            if ($("#radioAff").is(":checked")) {
                $("#dvGenerale").hide();
                $("#dvFiltreUser").hide();
            }
            break;
    }
}
//---------Lance la recherche en prenant en compte les filtres-------
function SearchDoc(pageNumber) {
    if (pageNumber == undefined || pageNumber == 0) {
        pageNumber = 1;
    }

    $(".requiredField").removeClass("requiredField");
    var errorParam = false;

    var dateDebSituation = 0;
    var dateFinSituation = 0;
    if ($("#inFiltreSituationDeb").hasVal() && isDate($("#inFiltreSituationDeb").val())) {
        var dateDebS = getDate($("#inFiltreSituationDeb").val());
        dateDebSituation = dateDebS.getFullYear() * 10000 + (dateDebS.getMonth() + 1) * 100 + dateDebS.getDate();
    }
    if ($("#inFiltreSituationFin").hasVal() && isDate($("#inFiltreSituationFin").val())) {
        var dateFinS = getDate($("#inFiltreSituationFin").val());
        dateFinSituation = dateFinS.getFullYear() * 10000 + (dateFinS.getMonth() + 1) * 100 + dateFinS.getDate();
    }
    if (dateDebSituation > 0 && dateFinSituation > 0 && getDate($("#inFiltreSituationDeb").val()) > getDate($("#inFiltreSituationFin").val())) {
        $("#inFiltreSituationDeb").addClass("requiredField");
        $("#inFiltreSituationFin").addClass("requiredField");
        errorParam = true;
    }

    var dateDebEdition = 0;
    var dateFinEdition = 0;
    if ($("#inFiltreEditionDeb").hasVal() && isDate($("#inFiltreEditionDeb").val())) {
        var dateDebE = getDate($("#inFiltreEditionDeb").val());
        dateDebEdition = dateDebE.getFullYear() * 10000 + (dateDebE.getMonth() + 1) * 100 + dateDebE.getDate();
    }
    if ($("#inFiltreEditionFin").hasVal() && isDate($("#inFiltreEditionFin").val())) {
        var dateFinE = getDate($("#inFiltreEditionFin").val());
        dateFinEdition = dateFinE.getFullYear() * 10000 + (dateFinE.getMonth() + 1) * 100 + dateFinE.getDate();
    }
    if (dateDebEdition > 0 && dateDebEdition > 0 && getDate($("#inFiltreEditionDeb").val()) > getDate($("#inFiltreEditionFin").val())) {
        $("#inFiltreEditionDeb").addClass("requiredField");
        $("#inFiltreEditionFin").addClass("requiredField");
        errorParam = true;
    }

    var numInterneAvn = $("#inFiltreAvenant").val();
    if (numInterneAvn == undefined || numInterneAvn == "") {
        numInterneAvn = -1;
    }

    if (errorParam)
        return false;
    $("#dvFiltreTypeAff").is(":visible");
    var filtre = {
        CodeOffre: $("#dvFiltreNumOffre").is(":visible") ? $("#inFiltreNumOffreContrat").val() : $("#inCodeOffre").val(),
        Version: $("#dvFiltreNumOffre").is(":visible") ? $("#inFiltreVersion").val() : $("#inVersion").val(),
        Type: $("#dvFiltreTypeAff").is(":visible") ? $("input[type='radio'][name='inFiltreType']:checked").val() : $("#inType").val(),
        User: $("#inFiltreUser").val(),
        Avenant: numInterneAvn,
        NumLot: $("#inFiltreLot").val(),
        Situation: $("#inFiltreSituation").val(),
        DateDebSituation: dateDebSituation,
        DateFinSituation: dateFinSituation,
        UniteService: $("#dvRechAvancee").is(":visible") ? $("#inFiltreUnite").val() : "",
        DateDebEdition: $("#dvRechAvancee").is(":visible") ? dateDebEdition : 0,
        DateFinEdition: $("#dvRechAvancee").is(":visible") ? dateFinEdition : 0,
        TypeDestinataire: $("#dvRechAvancee").is(":visible") ? $("#inFiltreTypeDestinataire").val() : "",
        CodeDestinataire: $("#dvRechAvancee").is(":visible") ? $("#inFiltreCodeDestinataire").val() : 0,
        CodeInterlocuteur: $("#dvRechAvancee").is(":visible") ? $("#inFiltreCodeInterlocuteur").val() : "",
        TypeDoc: $("#dvRechAvancee").is(":visible") ? $("#inFiltreTypeDoc").val() : "",
        CourrierType: $("#dvRechAvancee").is(":visible") ? $("#inFiltreTypeCour").val() : "",
        PageNumber: pageNumber
    };
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/SearchDoc",
        data: { displayType: $("#inDisplayType").val(), filtre: JSON.stringify(filtre), modeNavig: $("#ModeNavig").val(), readOnly: $("#OffreReadOnly").val() },
        success: function (data) {
            $("#dvListDoc").html(data);
            $("#inFiltreAffichage").val("A");
            MapListDocuments();
            ChangeSuiviDocAff($("#inFiltreAffichage").val());
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Réinitialise les filtres------------
function ResetFiltre() {
    $("#inFiltreUser").clear();
    $("#inFiltreTypeOffre").attr('checked', false);
    $("#inFiltreTypeContrat").attr('checked', false);
    $("#inFiltreNumOffreContrat").clear();
    $("#inFiltreAvenant").clear();
    $("#inFiltreLot").clear();
    $("#inFiltreSituation").clear();
    $("#inFiltreSituationDeb").clear();
    $("#inFiltreSituationFin").clear();
    $("#inFiltreUnite").clear();
    $("#inFiltreEditionDeb").clear();
    $("#inFiltreEditionFin").clear();
    $("#inFiltreTypeDestinataire").clear();
    $("#inFiltreCodeDestinataire").val("0");
    $("#inFiltreDestinataire").clear();
    $("#inFiltreCodeInterlocuteur").val("0");
    $("#inFiltreInterlocuteur").clear();
    $("#inFiltreTypeDoc").clear();
    $("#inFiltreTypeCour").clear();
    $("#inFiltreAffichage").val("A");
}
//---------Ouvre le document-----------------
function OpenDoc(elem) {
    ShowLoading();
    var nomDoc = elem.attr('albdocnom');
    var cheminDoc = elem.attr('albdocpath');

    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/OpenDoc",
        data: { docPath: cheminDoc, docName: nomDoc },
        success: function (data) {
            common.dialog.info("En cours de développement");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Affiche les informations du destinataire---------
function ShowInfoDestinataire(elem) {
    var idDest = elem.attr("albIdDest");
    var typeDest = elem.attr("albTypeDest");
    var typeEnvoi = elem.attr("albTypeEnvoi");

    $.ajax({
        type: "POST",
        url: "/DocumentGestion/ShowInfoDest",
        data: { idDest: idDest, typeDest: typeDest, typeEnvoi: typeEnvoi },
        success: function (data) {
            $("#divDestinataire").html(data);
            var pos = elem.position();
            $("#divDestinataire").css({ "position": "absolute", "top": pos.top + 20 + "px", "left": pos.left + 5 + "px" }).show();

        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//----------Ouverture de la div des pièces jointes d'un document----------
function OpenDivPJ(docId) {
    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/OpenPJ",
        data: { docId: docId },
        success: function (data) {
            $("#divPJDoc").show();
            $("#divDataPJDoc").html(data);
            MapDivPJ();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Map les éléments de la div des PJ------
function MapDivPJ() {
    $("#btnFancyAnnuler").kclick(function () {
        $("#divPJDoc").hide();
        $("#divDataPJDoc").clearHtml();
    });
}
//-------Ferme l'écran de suivi de documents-------
function CloseSuiviDoc() {
    $("#divDataSuiviGestionDocuments").clearHtml();
    $("#divSuiviGestionDocuments").hide();
    $("#dvDataFlotted").clearHtml();
    $("#dvFlotted").hide();
}

function UpdDocCPCS(docId) {
    GetFileNameDocument($("#wBlocDocType").val(), docId, false, "MP");
}

function RefreshDoc(docId) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/RefreshDocuments",
        data: { docId: docId },
        success: function (data) {
            if (data != "") {
                common.dialog.error(data);
            }
            else {
                common.dialog.info("Document regénéré");
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function SaveBloc() {
    var fullPath = $("iframe[id='WordContainerIFrame']").contents().find("#physicNameDoc").val();
    if (!$("#WordContainerIFrame")[0].contentWindow.SaveDocument()) {
        common.dialog.error("Erreur lors de la sauvegarde du document.");
        CloseLoading();
    }
    else {

        setTimeout(function () {
            if ($("#DefaultDisplayDocModule").hasTrueVal() && !$("#WordContainerIFrame")[0].contentWindow.CloseWordDoc()) {
                common.dialog.error("Erreur lors de la fermeture du document.");
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/SuiviDocuments/SaveBloc",
                    data: { docFullPath: fullPath },
                    success: function (data) {
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });
            }
        }, 1000);
        CloseLoading();
        $("#divWordContainer").hide();
    }
}

//#endregion

//#region Informations de base
//--------Ouverture des informations de base en popup-------
function OpenInfoBase(readonly) {
    if (readonly == undefined)
        readonly = $("#OffreReadOnly").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ConfirmationSaisie/LoadInfoBase",
        data: { codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), readOnly: readonly },
        success: function (data) {
            $("#dvDataInformationsBase").html(data);
            $("#dvInformationsBase").show();
            MapInfoBaseElement();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Map les éléments des informations de base-------
function MapInfoBaseElement() {
    MapAdresse();
    MapCommonAutoCompCourtier();
    MapCommonAutoCompAssure();
    MapCommonAutoCompSouscripteur();
    MapCommonAutoCompGestionnaire();

    $("#HeureSaisieStringHours").offOn("change", function () {
        if ($(this).hasVal() && $("#HeureSaisieStringMinutes").val() == "")
            $("#HeureSaisieStringMinutes").val("00");
        AffecteHour($(this));
    });
    $("#HeureSaisieStringMinutes").offOn("change", function () {
        if ($(this).hasVal() && $("#HeureSaisieStringHours").val() == "")
            $("#HeureSaisieStringHours").val("00");
        AffecteHour($(this));
    });


    $("#btnValidConf").kclick(function () {
        SaveInfoBase();
    });
    $("#btnAnnulerConf").kclick(function () {
        $("#dvDataInformationsBase").clearHtml();
        $("#dvInformationsBase").hide();
    });
    if ($("#PreneurAssurance_CodePreneurAssurance").hasVal()) {
        $("#PreneurAssurance_CodePreneurAssurance").change();
    }
    if ($("#CabinetCourtage_CodeCabinetCourtage").hasVal()) {
        var codeInterlocuteur = $("#CabinetCourtage_CodeInterlocuteur").val();
        var nomInterlocuteur = $("#CabinetCourtage_NomInterlocuteur").val();
        $("#hideInterlocuteur").val($("#CabinetCourtage_CodeInterlocuteur").val() + "_" + $("#CabinetCourtage_NomInterlocuteur").val());
        $("#CabinetCourtage_CodeCabinetCourtage").change();
        $("#CabinetCourtage_CodeInterlocuteur").val(codeInterlocuteur);
        $("#CabinetCourtage_NomInterlocuteur").val(nomInterlocuteur);
    }

    toggleDescription($("#Description_Observation"));
    MapBtnAssureAdd();
    MapBoitesDialogue();

    if (window.isReadonly) {
        $("#InformationSaisie_DateSaisieString").attr("readonly", "readonly").addClass("readonly");
        $("#HeureSaisieStringHours").attr("disabled", "disabled").addClass("readonly");
        $("#HeureSaisieStringMinutes").attr("disabled", "disabled").addClass("readonly");
        $("#InformationSaisie_Souscripteurs").attr("readonly", "readonly").addClass("readonly");
        $("#InformationSaisie_Gestionnaires").attr("readonly", "readonly").addClass("readonly");
        $("#CabinetCourtage_CodeCabinetCourtage").attr("readonly", "readonly").addClass("readonly");
        $("#CabinetCourtage_NomCabinetCourtage").attr("readonly", "readonly").addClass("readonly");
        $("#CabinetCourtage_NomInterlocuteur").attr("readonly", "readonly").addClass("readonly");
        $("#CabinetCourtage_Reference").attr("readonly", "readonly").addClass("readonly");
        $("#PreneurAssurance_CodePreneurAssurance").attr("readonly", "readonly").addClass("readonly");
        $("#PreneurAssurance_NomPreneurAssurance").attr("readonly", "readonly").addClass("readonly");
        $("#btnOpenAssu").hide();
        $("#Description_Descriptif").attr("readonly", "readonly").addClass("readonly");
        $("#MotClef1").attr("disabled", "disabled").addClass("readonly");
        $("#MotClef2").attr("disabled", "disabled").addClass("readonly");
        $("#MotClef3").attr("disabled", "disabled").addClass("readonly");
        $("div[name='btnAdresse'][albcontext='adrbase']").removeClass("CursorPointer");
        $("#btnValidConf").hide();
    }
    else {
        formatDatePicker();
    }
}
//-------------------Renseigne l'heure---------------------------
function AffecteHour(elem) {
    var elemId = elem.attr('id').replace("Hours", "").replace("Minutes", "");

    var changeHour = SetHours(elemId);
    if (!changeHour && elem.val() == "") {
        $("#" + elemId + "Hours").clear();
        $("#" + elemId + "Minutes").clear();
    }
}

function MapBtnAssureAdd() {
    var acteGestion = $("#ActeGestion").val();

    if (acteGestion != undefined && acteGestion != "" && $("#CopyMode").val() == "False" && $("#PreneurAssurance_CodePreneurAssurance").hasVal()) {
        $("#btnOpenAssu").kclick(function () { OpenAssuAdd(); });
    }
    else {
        $("#btnOpenAssu").hide();
    }
}
//----------------------Formate tous les controles datepicker---------------------
function formatDatePicker() {
    $("#InformationSaisie_DateSaisieString").datepicker({ dateFormat: 'dd/mm/yy' }).change(function () {
        if ($(this).hasVal()) {
            if ($("#HeureSaisieStringHours").val() == "") {
                $("#HeureSaisieStringHours").val("00");
                $("#HeureSaisieStringMinutes").val("00");
                $("#HeureSaisieStringHours").change();
            }
        }
        else {
            $("#HeureSaisieStringHours").clear();
            $("#HeureSaisieStringMinutes").clear();
            $("#HeureSaisieStringHours").change();
        }
    });
}
//--------Déplace le div de l'adresse----------
function MapAdresse() {
    $("#divAdresse").html($("#divHideAdresse").html());
    LinkHexavia();
}
//-------Enregistre les informations de base-------
function SaveInfoBase() {

    if (!CheckInfoBase())
        return false;

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var dateSaisie = $("#InformationSaisie_DateSaisieString").val();
    var heureSaisie = $("#HeureSaisieString").val();
    var codeSouscripteur = $("#InformationSaisie_CodeSouscripteur").val();
    var codeGestionnaire = $("#InformationSaisie_CodeGestionnaire").val();
    var codeInterlocuteur = $("#CabinetCourtage_CodeInterlocuteur").val();
    var reference = $("#CabinetCourtage_Reference").val();
    var codePreneur = $("#PreneurAssurance_CodePreneurAssurance").val();
    var identification = $("#Description_Descriptif").val();
    var motClef1 = $("#MotClef1").val();
    var motClef2 = $("#MotClef2").val();
    var motClef3 = $("#MotClef3").val();
    var observations = "";
    // Tester si l'input observation exite
    if ($("#Description_Observation")) {
        // Remplacer les <br> par \r\n
        observations = $("#Description_Observation").val().replace(/<br ?\/?>/g, "\r\n");
    }
    var batiment = $("input[id='ContactAdresse_Batiment'][albcontext='adrbase']").val();
    var numVoie = $("input[id='ContactAdresse_No'][albcontext='adrbase']").val();
    var extensionVoie = $("input[id='ContactAdresse_Extension'][albcontext='adrbase']").val();
    var nomVoie = $("input[id='ContactAdresse_Voie'][albcontext='adrbase']").val();
    var boitePostale = $("input[id='ContactAdresse_Distribution'][albcontext='adrbase']").val();
    var cp = $("input[id='ContactAdresse_CodePostal'][albcontext='adrbase']").val();
    var nomVille = $("input[id='ContactAdresse_Ville'][albcontext='adrbase']").val();
    var cpCdx = $("input[id='ContactAdresse_CodePostalCedex'][albcontext='adrbase']").val();
    var nomVilleCdx = $("input[id='ContactAdresse_VilleCedex'][albcontext='adrbase']").val();
    var nomPays = $("input[id='ContactAdresse_Pays'][albcontext='adrbase']").val();
    var matricule = $("input[id='ContactAdresse_MatriculeHexavia'][albcontext='adrbase']").val();
    var numeroChrono = $("input[id='ContactAdresse_NoChrono'][albcontext='adrbase']").val();
    var preneurEstAssure = $("input[id='PreneurEstAssure']").val();

    var codeCourtierGestionnaire = $("#CabinetCourtage_CodeCabinetCourtage").val();
    var nomCourtierGestionnaire = $("#CabinetCourtage_NomCabinetCourtage").val();
    var nomInterlocuteur = $("#CabinetCourtage_NomInterlocuteur").val();
    var nomPreneurAssurence = $("#PreneurAssurance_NomPreneurAssurance").val();

    $("#lnkBlackList").hide();
    $("#idBlacklistedPartenaire").clear();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ConfirmationSaisie/SaveInfoBase",
        data: {
            codeOffre: codeOffre, version: version, type: type, dateSaisie: dateSaisie, heureSaisie: heureSaisie, codeSouscripteur: codeSouscripteur, codeGestionnaire: codeGestionnaire,
            codeInterlocuteur: codeInterlocuteur, reference: reference, codePreneur: codePreneur, identification: identification, motClef1: motClef1, motClef2: motClef2, motClef3: motClef3,
            observations: observations,
            batiment: batiment, numVoie: numVoie, extensionVoie: extensionVoie, nomVoie: nomVoie, boitePostale: boitePostale, cp: cp, nomville: nomVille, cpCdx: cpCdx, nomVilleCdx: nomVilleCdx,
            nomPays: nomPays, matricule: matricule, numeroChrono: numeroChrono, job: $("#jobInfoBase").val(), cible: $("#cibleInfoBase").val(), preneurEstAssure: preneurEstAssure,
            codeCourtierGestionnaire: codeCourtierGestionnaire, nomCourtierGestionnaire: nomCourtierGestionnaire, nomInterlocuteur: nomInterlocuteur, nomPreneurAssurence: nomPreneurAssurence,
            tabGuid: $("#tabGuid").val(), modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            $("#dvDataInformationsBase").clearHtml();
            $("#dvInformationsBase").hide();
            // Redirection coté serveur donc on supprime le CloseLoading
        },
        error: function (request) {
            let result = null;
            try { result = JSON.parse(request.responseText); }
            catch (e) { result = null; }
            let alerts = kheops.alerts.blacklist.displayAll(result);
            if (Array.isArray(alerts) && alerts.some(function (a) { return a.Partner.Type == "PreneurAssurance"; })) {
                $("#lnkBlackList").show();
                $("#idBlacklistedPartenaire").val($("#PreneurAssurance_CodePreneurAssurance").val());
            }
            else {
                common.error.showXhr(request);
            }
        }
    });
}
//-------Vérifie les informations de base-------
function CheckInfoBase() {
    $(".requiredField").removeClass("requiredField");
    var returnBool = true;

    var dateSaisie = $("#InformationSaisie_DateSaisieString").val();
    var heureSaisie = $("#HeureSaisieString").val();
    var codeSouscripteur = $("#InformationSaisie_CodeSouscripteur").val();
    var codeGestionnaire = $("#InformationSaisie_CodeGestionnaire").val();
    var codePreneur = $("#PreneurAssurance_CodePreneurAssurance").val();
    var codePreneurActif = $("#inInvalidPreneurAssu[albcontext='assure']").val();
    var identification = $("#Description_Descriptif").val();

    if (dateSaisie == "" || !isDate(dateSaisie) || Date.parse(dateSaisie + " " + heureSaisie) == null) {
        $("#InformationSaisie_DateSaisieString").addClass("requiredField");
        $("#HeureSaisieString").addClass("requiredField");
        returnBool = false;
    }
    if (codeSouscripteur == "" || codeSouscripteur == "0") {
        $("#InformationSaisie_CodeSouscripteur").addClass("requiredField");
        returnBool = false;
    }
    if (codeGestionnaire == "" || codeGestionnaire == "0") {
        $("#InformationSaisie_CodeGestionnaire").addClass("requiredField");
        returnBool = false;
    }
    if (codePreneur == "" || codePreneur == "0" || codePreneurActif == "1") {
        $("#PreneurAssurance_CodePreneurAssurance").addClass("requiredField");
        returnBool = false;
    }
    if (identification == "") {
        $("#Description_Descriptif").addClass("requiredField");
        returnBool = false;
    }
    return returnBool;
}
//#endregion


//---------Génération des documents----------
function GenerateSuiviDoc() {
    var idDoc = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albdocid");
    var codeAff = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albnumaff");
    var version = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albvers");
    var type = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albtypaff");
    var avenant = $("input[type='checkbox'][name='checkLot']:checked:first").attr("albavtaff");

    var codeLot = $("td[name='tdLot'][albdocid='" + idDoc + "']").attr("alblotid");

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/GenerateDocuments",
        data: { numAffaire: codeAff, version: version, type: type, avenant: avenant, lotid: codeLot },
        success: function (data) {
            CloseLoading();
            if (data == "True") {
                $("#divMsgSuiviDoc").hide();
                $("#btnFiltrer").click();
            }
            else
                common.dialog.error("Erreur lors de la génération des documents");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//---------Impression des documents-----------
function PrintSuiviDoc() {


}

//--------Sortir de du module d'impression---------
function ExitPrintDoc() {

}
//----------Réédition des documents-------
function ReeditionSuiviDoc() {

}
function CloseDivDoc() {
    $("#divIframeDoc").hide();
}
//-----------Edition du suivi des documents-----------
function EditSuiviDoc() {
    $("#msgSuiviDocInfoReedit").hide();
    $("#msgSuiviDocInfoGenerateLot").hide();
    $("#msgSuiviDocInfoPrintLot").hide();
    $("#btnConfirmOkSuiviDoc").hide().clear();
    $("#btnConfirmGenSuiviDoc").hide().clear();

    //********** ZBO : Non utilisées
    var idDoc = "";
    var codeLot = "";
    var libLot = "";
    var situationDoc = "";
    var nomDoc = "";
    var typeDoc = "";
    var typeCodeDoc = "";
    var typeDest = "";
    var codeDest = "";
    var nomDest = "";
    //*****************************
    var countEditLogo = $("input[type='checkbox'][name='checkEditLogo']:checked").length;
    var countEditNoLogoCheck = $("input[type='checkbox'][name='checkEditNoLogo']:checked").length;
    if (countEditLogo + countEditNoLogoCheck == 0) {
        common.dialog.error("Veuillez sélectionner au moins un document à éditer");
        return false;
    }
    if (countEditLogo > 10) {
        common.dialog.error("Vous ne pouvez pas éditer plus de 10 documents à la fois.");
        return false;
    }

    var splitChar = $("#splitChar").val();
    var listeDocEditNoLogo = "";
    var listeDocEditLogo = "";
    $("input[type='checkbox'][name='checkEditNoLogo']:checked").each(function () {
        listeDocEditNoLogo = listeDocEditNoLogo + $(this).attr("albdocid") + splitChar;
    });

    $("input[type='checkbox'][name='checkEditLogo']:checked").each(function () {
        var docId = $(this).attr("albdocid");
        if (listeDocEditNoLogo.indexOf(docId + splitChar) <= -1)
            listeDocEditLogo = listeDocEditLogo + $(this).attr("albdocid") + splitChar;
    });

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/EditDocuments",
        data: { listeDocIdLogo: listeDocEditLogo, listeDocIdNOLogo: listeDocEditNoLogo },
        success: function (data) {
            var src = $("#asynchDownLoad").val().replace("prm=NO", "prm=" + data);
            $("#frmDownLoadDoc").attr("src", src).show();
            $("#divIframeDownLoadDoc").show();

            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });


}

//********************************** ExitAsynchLoadDoc
function ExitAsynchLoadDoc() {
    $("#btnExitPrintAsynchLoad").kclick(function () {
        $("#frmDownLoadDoc").attr("src", "");
        $("#divIframeDownLoadDoc").hide();
    });
}


function HwndDelWdPrs() {
    var oldHwnd = $("#hwnd").val();
    if (oldHwnd != "0") {

        var src = $("#asyncKlWdPrs").val().replace("prs=0&prm=NO", "prs=" + oldHwnd);
        $("#frmKlPrs").attr("src", src).reload(true);
    }
}
//#region Recherche avancée transverse Preneurs assurance

function RechercheAvanceePreneurAssurance() {
    $("#RechercherPreneursAssuranceButton").kclick(function () {
        RecherchePreneursAssurance("ASC", 1);
    });
}

var lastestSearchPreneurs = null;
function RecherchePreneursAssurance(Order, By) {
    $("#MsgAffinementPreneurAssurance").clearHtml();
    ShowLoading();

    var pagePreneursAssurance = $("#PreneurAssurancePaginationPageActuelle").html();
    if (pagePreneursAssurance == null) {
        pagePreneursAssurance = 1;
    }
    let search = {
        code: $("#CodePreneurAssuranceRecherche").val(),
        name: $("#NomPreneurAssuranceRecherche").val(),
        cp: $("#CPPreneurAssuranceRecherche").val()
    };
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/GetPreneurAssuranceRechercher",
        data: {
            code: $("#CodePreneurAssuranceRecherche").val(), name: $("#NomPreneurAssuranceRecherche").val(), cp: $("#CPPreneurAssuranceRecherche").val(),
            pageNumber: (common.tools.compareOneLevelObjects(lastestSearchPreneurs, search) === 0 ? pagePreneursAssurance : 1),
            order: Order,
            by: By
        },
        success: function (data) {
            CloseLoading();
            $("#PreneursAssuranceRechercherResultDialogDiv").html(data);
            if ($("#NbCountPreneurAssurance").val() > $("#LineCountPreneurAssurance").val()) {
                $("#MsgAffinementPreneurAssurance").show().html("Le nombre réel de lignes trouvées dépasse " + $("#LineCountPreneurAssurance").val() + ", veuillez affiner votre recherche");
            }

            AlternanceLigne("PreneursAssurance", "Code", true, null);
            triPreneursAssurance();
            lastestSearchPreneurs = search;
            if (data.length == 0) {
                ShowDialogInFancy("Error", "Aucun résultat", 300, 65);
                $("#btnDialogOk").click(function () {
                    $("#divDialogInFancy").hide();
                });
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function triPreneursAssurance() {
    $("th.TablePersoTdHead").css("cursor", "pointer");
    if (window.lastestSearchPreneurs != null) { return; }
    $(document).off("click", "th.TablePersoTdHead");
    $(document).on("click", "th.TablePersoTdHead", function () {
        var img = $(".imageTri").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1);
        img = img.substr(0, img.lastIndexOf('.'));
        AlternanceLigne("PreneursAssurance", "Code", true, null);
        if (img == "tri_asc") {
            $(".imageTri").attr("src", "../../Content/Images/tri_asc.png");
            RecherchePreneursAssurance("desc", $(this).attr("id"));
        }
        else if (img == "tri_desc") {
            $(".imageTri").attr("src", "../../Content/Images/tri_desc.png");
            RecherchePreneursAssurance("asc", $(this).attr("id"));
        }
        $(".spImg").css('visibility', 'hidden');
        $(this).children(".spImg").css('visibility', 'visible');
    });
}
function SelectPreneurAssurance() {
    $("#tblPreneursAssurance tbody > tr").kclick(function () {
        var nom = rhtmlspecialchars($(this).find("td").eq(1).html());
        var code = $(this).find("td").eq(0).html();
        var codePostal = $(this).find("td").eq(3).html();
        var ville = rhtmlspecialchars($(this).find("td").eq(4).html());
        $("#PreneurAssuranceNom").val($.trim(nom));
        $("#PreneurAssuranceCode").val($.trim(code));
        $("#PreneurAssuranceCP").val($.trim(codePostal));
        $("#PreneurAssuranceVille").val($.trim(ville));
        CheckSearchField();
        $("#btnCloseAdvanced").click();
    });
}




function MapElementsRechercheAvanceeTransPreneurAssu() {
    RechercheAvanceePreneurAssurance();
    $("#RechercherPreneursAssuranceRAZ").kclick(function () {
        $("#CPPreneurAssuranceRecherche").clear();
        $("#NomPreneurAssuranceRecherche").clear();
        $("#CodePreneurAssuranceRecherche").clear().focus();
    });
    $("#tblPreneursAssurance tbody > tr").kclick(function () {
        var id = $(this).attr("id");
        if (id != undefined && id != "") {
            id = id.split("_")[1];
            if (id != undefined && id != "") {
                var contexte = $("#RechercheAvanceePreneurAssuContexte").val();
                if (contexte != undefined && contexte != "") {
                    $("input[albAutocomplete=autoCompCodeAssure][albcontext=" + contexte + "]").val(id).change();
                }
                else {
                    $("input[albAutocomplete=autoCompCodeAssure]").val(id).change();
                }
            }
        }
        $("#btnCloseAdvanced").click();
    });
    AlternanceLigne("PreneursAssurance", "", true, null);

    $("#btnLancerRecherchePreneursAssurance").kclick(function () {
        LancerRecherchePreneurAssurance();
    });

    $("#btnCloseAdvanced").kclick(function () {
        $("#divDataRechercheAvanceePreneurAssuTrans").clearHtml();
        $("#divRechercheAvanceePreneurAssuTrans").hide();
    });

    MapElementsRechercheAvanceeTransPreneurAssuTableau();
}

function LancerRecherchePreneurAssurance() {
    var codePreneur = $("#CodePreneurAssuranceRechercheTrans").val();
    var nomPreneur = $("#NomPreneurAssuranceRechercheTrans").val();
    var cpPreneur = $("#CodePostalPreneurAssuranceRechercheTrans").val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/RechercheTransversePreneursAssurance",
        data: { codePreneur: codePreneur, nomPreneur: nomPreneur, codePostalPreneur: cpPreneur },
        success: function (data) {
            $("#divResRecherchePreneurAssuBody").html(data);
            MapElementsRechercheAvanceeTransPreneurAssuTableau();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

function MapElementsRechercheAvanceeTransPreneurAssuTableau() {
    AlternanceLigne("ResRecherchePreneurAssuBody", "", true, null);
    $("tr[name=lineResultRecherchePreneur]").kclick(function () {
        var id = $(this).attr("id");
        if (id != undefined && id != "") {
            id = id.split("_")[1];
            if (id != undefined && id != "") {
                var contexte = $("#RechercheAvanceePreneurAssuContexte").val();
                if (contexte != undefined && contexte != "") {
                    $("input[albAutocomplete=autoCompCodeAssure][albcontext=" + contexte + "]").val(id).change();
                }
                else {
                    $("input[albAutocomplete=autoCompCodeAssure]").val(id).change();
                }
                $("#divDataRechercheAvanceePreneurAssuTrans").clearHtml();
                $("#divRechercheAvanceePreneurAssuTrans").hide();
            }
        }
    });
    if ($("tr[name=lineResultRecherchePreneur]").length == 250) {
        $("span[id='MsgAffinementPreneurAssurance']").text("Affichage limité à 250 lignes, veuillez affiner votre recherche.").show();
    }
    else {
        $("span[id='MsgAffinementPreneurAssurance']").text("").hide();
    }
}


function OpenRechercheAvanceePreneurAssurance(code, nom, context) {

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CommonNavigation/OpenRechercheAvanceePreneurAssurance",
        data: { codePreneur: code, nomPreneur: nom, contexte: context },
        success: function (data) {
            $("#divDataRechercheAvanceePreneurAssuTrans").html(data);
            MapElementsRechercheAvanceeTransPreneurAssu();
            $("#divRechercheAvanceePreneurAssuTrans").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
