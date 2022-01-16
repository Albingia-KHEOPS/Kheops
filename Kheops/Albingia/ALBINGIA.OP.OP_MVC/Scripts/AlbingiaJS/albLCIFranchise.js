$(document).ready(function () {
    MapElementPageLCIFranchise();
});
function MapElementPageLCIFranchise() {
    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "DelExpression":
                DeleteExpression();
                CloseCommonFancy();
                $("#hiddenInputId").val("");
                break;
            case "DelDetailExpr":
                DeleteLineDetailsExpr();
                CloseCommonFancy();
                $("#hiddenInputId").val('');
                break;
            case "Cancel":
                let redir = conditionsGaranties.redirect || Redirection;
                if ($("#NomEcran").val() == "EngagementParTraite") {
                    if ($("#codePeriode").val() == "")
                        redir("MatriceRisque", "Index");
                    else {
                        redir("EngagementPeriodes", "Index");
                    }
                }
                if ($("#NomEcran").val() == "ConditionsGaranties") {
                    redir("CreationFormuleGarantie", "Index");
                }
                if ($("#NomEcran").val() == "DetailsEngagement") {
                    var annul = true;
                    redir("Engagements", "Index", "", annul);
                }
                if ($("#NomEcran").val() == "InformationsSpecifiquesRisque") {
                    redir("DetailsRisque", "Index");
                }
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });
}
function MapElementLCIFranchise(typeVue, typeAppel, nomEcran, modeAcess) {
    if (modeAcess == undefined) {
        modeAcess = "";
    }
    var vue_appel = typeVue + typeAppel;
    AffectTitleList($("#Unite_" + vue_appel));
    AffectTitleList($("#Type_" + vue_appel));
    $("select[id=Unite_" + vue_appel + "]").offOn("change", function () {
        if (nomEcran == "EngagementParTraite") {
            engagements.toggleComputeButtons();
        }
        if (nomEcran == "DetailsEngagement") {
            engagementsTraites.toggleComputeButton();
            engagementsTraites.lockInputsNonLCI();
        }
        var valLCIUnite = $(this).val();

        $("#Valeur_" + vue_appel).removeAttr('readonly', 'readonly').removeClass('readonly');
        if ($("#Unite_" + vue_appel).val() == "") {
            $("#Valeur_" + vue_appel).val("");
            $("#Type_" + vue_appel).val("");
            $("#idLienCpx_" + vue_appel).val('0');
            $("#LienCpx_" + vue_appel).addClass("None");
            $("#divType_" + vue_appel).removeClass("None");
            $("#divLienCpx_" + vue_appel).addClass("None");
            $("input[id=IsIndexe_" + vue_appel + "]").attr('checked', false);
        }
        if ($("#Unite_" + vue_appel).val() == "CPX") {
            $("#LienCpx_" + vue_appel).removeClass("None");
            $("#divType_" + vue_appel).addClass("None");
            $("#Valeur_" + vue_appel).attr('readonly', 'readonly').addClass('readonly').val("");
            $("#divLienCpx_" + vue_appel).removeClass("None");
            $("#divLienCpx_" + vue_appel).addClass("DivLienCpx");
            $("#TypeOperation").val("Ajout");
        }
        else {
            $("#idLienCpx_" + vue_appel).val('0');
            $("#LienCpx_" + vue_appel).addClass("None");
            $("#divType_" + vue_appel).removeClass("None");
            $("#divLienCpx_" + vue_appel).addClass("None");
        }
        ChangeFormatInfoOffre($(this), typeVue, typeAppel);
        AffectTitleList($(this));
        if (valLCIUnite == "CPX") {
            if (typeAppel == "Risque" || typeAppel == "Generale") {
                OpenExpComplexeGenRsq(typeVue, typeAppel)
            }
            else {
                conditionsGaranties.openExpComplexe(typeVue);
            }
        }
    });
    $("#Valeur_" + vue_appel).offOn("change", function () {
        if (nomEcran == "EngagementParTraite") {
            engagements.toggleComputeButtons();
        }
        if (nomEcran == "DetailsEngagement") {
            engagementsTraites.toggleComputeButton();
            engagementsTraites.lockInputsNonLCI();
        }
        if ($("#Valeur_" + vue_appel).val() == "") {
            $("#Unite_" + vue_appel).val("");
            $("#Type_" + vue_appel).val("");
            $("input[id=IsIndexe_" + vue_appel + "]").attr('checked', false);
        }
    });
    $("select[id=Type_" + vue_appel + "]").offOn("change", function () {
        AffectTitleList($(this));
    });
    MapLinkExprComplexeGenRsq(typeVue, typeAppel);
    if (typeVue.trim().toUpperCase() == "LCI" && typeAppel.trim().toUpperCase() == "GENERALE") {
        if (modeAcess) {
            let a = ["#Valeur_" + vue_appel, "#Unite_" + vue_appel, "#Type_" + vue_appel, "#IsIndexe_" + vue_appel];
            $(a.join(",")).makeReadonly(true);
        }
    }
}
function ValidateLCIFranchise(typeVue, typeAppel) {
    var vue_appel = typeVue + typeAppel;
    var result = true;
    if (($("#Valeur_" + vue_appel).autoNumeric('get') == 0 || $("#Valeur_" + vue_appel).autoNumeric('get') == "")
        && ($("#Unite_" + vue_appel).val() != "" || $("#Type_" + vue_appel).val() != "") && $("#Unite_" + vue_appel).val() != "CPX") {
        $("#Valeur_" + vue_appel).addClass('requiredField');
        result = false;
    }
    if ((($("#Valeur_" + vue_appel).autoNumeric('get') != 0 && $("#Valeur_" + vue_appel).autoNumeric('get') != "")
        || $("#Type_" + vue_appel).val() != "") && $("#Unite_" + vue_appel).val() == "") {
        $("#Unite_" + vue_appel).addClass('requiredField');
        result = false;
    }
    if ((($("#Valeur_" + vue_appel).autoNumeric('get') != 0 && $("#Valeur_" + vue_appel).autoNumeric('get') != "")
        || ($("#Unite_" + vue_appel).val() != "" && $("#Unite_" + vue_appel).val() != "CPX")) && $("#Type_" + vue_appel).val() == "") {
        $("#Type_" + vue_appel).addClass('requiredField');
        result = false;
    }

    return result;
}

//-------Change le format des informations de l'offre-------
function ChangeFormatInfoOffre(element, typeVue, typeAppel) {
    $("#Valeur_" + typeVue + typeAppel).attr('albMask', 'decimal');
    common.autonumeric.apply($("#Valeur_" + typeVue + typeAppel), 'update', 'decimal', ' ', null, null, '99999999999.99', null);
}
//-------------Ouverture de la div flottante des expressions complexes générales et risque-----------
function sleep(milliseconds) {
    const date = Date.now();
    let currentDate = null;
    do {
        currentDate = Date.now();
        ShowLoading();
    } while (currentDate - date < milliseconds);
}
function OpenExpComplexeGenRsq(typeVue, typeAppel, codeExpression, actionExpr) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var typeOffre = $("#Offre_Type").val();
    var isReadOnly = $("#OffreReadOnly").val();
    ShowLoading();
    //$("#divLoading").addClass("shown").show();
    
    $.ajax({
        type: "POST",
        url: "/ConditionsGarantie/OpenExprComplexe",
        async: false,
        data: { codeOffre: codeOffre, version: version, type: typeOffre, typeExpr: typeVue, isReadOnly: isReadOnly, isGenRsq: true, typeAppel: typeAppel, modeNavig: $("#ModeNavig").val() },
        context: $("#divDataExprComp"),
        success: function (data) {
            DesactivateShortCut();
            $(this).html(data);

            $("#TypeAppel").val(typeAppel);
            MapeElementExprGenRsq();
            if (isReadOnly == "True") {
                $("#btnValideExprGenRsq").hide();
            }

            AlbScrollTop();

            $("#divExprComplexe").show();

            switch (actionExpr) {
                case "DuplicateExpr":
                    if ($("img[id='editExpr¤" + codeExpression + "']") != undefined)
                        $("img[id='editExpr¤" + codeExpression + "']").trigger("click");
                    if ($("img[id='readExpr¤" + codeExpression + "']") != undefined)
                        $("img[id='readExpr¤" + codeExpression + "']").trigger("click");
                    $("input[type='radio'][id='setExpr¤" + codeExpression + "']").attr("checked", "checked").trigger("change");
                    break;
                case "SaveExpr":
                    $("input[type='radio'][id='setExpr¤" + codeExpression + "']").attr("checked", "checked").trigger("change");
                    break;
                case "ValidExpr":
                    $("input[type='radio'][id='setExpr¤" + codeExpression + "']").attr("checked", "checked").trigger("change");
                    $("img[id='editExpr¤" + codeExpression + "']").trigger("click");
                    $("img[id='readExpr¤" + codeExpression + "']").trigger("click");
                    break;
                default:
                    if (codeExpression != null && codeExpression != "") {
                        $("#setExpr¤" + codeExpression).attr("checked", "checked");
                        if ($("img[id='editExpr¤" + codeExpression + "']") != undefined)
                            $("img[id='editExpr¤" + codeExpression + "']").trigger("click");
                        if ($("img[id='readExpr¤" + codeExpression + "']") != undefined)
                            $("img[id='readExpr¤" + codeExpression + "']").trigger("click");
                        $("#CurrentCodeExprComp").val(codeExpression);
                    }
                    break;
            };

            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------Affecte l'expression complexe -------
function SetExprComplexeGenRsq(typeExpr, idExprComplexe, codeExprComplexe, libExpr) {
    //var exprComplexe = idExprComplexe + "¤" + codeExprComplexe;
    var codeFormule = $("#CodeFormule").val();
    var codeOption = $("#CodeOption").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var typeAppel = $("#TypeAppel").val();
    var codeRisque = $("#Code").val();
    var unite = $("#Unite_" + typeExpr + typeAppel).val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ConditionsGarantie/SaveExprComplexeGenRsq",
        data: {
            typeExpr: typeExpr, typeAppel: typeAppel, codeExpr: idExprComplexe, codeFormule: codeFormule, codeOption: codeOption
            , codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeRisque: codeRisque, unite: unite, modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            var vue_appel = typeExpr + typeAppel;
            $("#Type_" + vue_appel).val("");
            $("#LienCpx_" + vue_appel).text(codeExprComplexe);
            $("#LienCpx_" + vue_appel).attr("title", libExpr);
            $("#LienCpx_" + vue_appel).attr("albhref", "#" + idExprComplexe);
            $("#idLienCpx_" + vue_appel).val(idExprComplexe).trigger("change");
            if ($("#NomEcran").val() == "EngagementParTraite") {
                if (typeof engagementsTraites !== "undefined") {
                    engagementsTraites.ToggleButton();
                }
            }
            //$("#divLCIType¤" + codeCondition).text("");
            //$("#LienLCIComplexe¤" + codeCondition).val(exprComplexe);
            //$("div[name=OffLCIComplexe¤" + codeCondition + "]").addClass("None");
            //$("div[name=OnLCIComplexe¤" + codeCondition + "]").removeClass("None");
            //$("#LockLienLCIComplexe¤" + codeCondition).text(codeExprComplexe);
            //$("#LockLienLCIComplexe¤" + codeCondition).attr("href", "#" + exprComplexe);
            //$("#EditLienLCIComplexe¤" + codeCondition).text(codeExprComplexe);
            //$("#EditLienLCIComplexe¤" + codeCondition).attr("href", "#" + exprComplexe);

            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request, true);

        }
    });
}
//-------------Affecte l'ouverture des expressions complexes sur les liens------------
function MapLinkExprComplexeGenRsq(typeVue, typeAppel) {
    $("span[name=LienComplexeGenRsq_" + typeVue + typeAppel + "]").click(function () {
        var codeExpression = $(this).attr('albhref').split('#')[1];
        OpenExpComplexeGenRsq(typeVue, typeAppel, codeExpression);
        $("#TypeOperation").val("Modif");

    });
}
//---------------Map les éléments de la div flottante Expr Complexe--------------
function MapeElementExprGenRsq() {
    $("input[type='radio'][name='modeAjout']").offOn("change", function () {
        if ($(this).val() == "ref" || $(this).val() == "mdl") {
            OpenReferentiel($(this).val());
        }
    });

    AlternanceLigne("InfoExpr", "", false, null);

    $("#addExpr").kclick(function () {
        $(this).attr("disabled", "disabled");
        OpenAddExpr();
    });

    $("#imgAddExpr").kclick(function () {
        if ($(this).hasClass("CursorPointer")) {
            $("input[type='radio'][name='radAddBtn']").removeAttr("checked");
            $(this).attr("src", "/Content/Images/plusajouter_gris1616.jpg");
            var pos = $(this).position();
            $("#dvAddMode").attr("style", "position:absolute;top:" + (pos.top - 6) + "px;left:" + (pos.left + 25) + "px; display: block;");
        }
    });

    $("#dvCloseExpr").kclick(function () {
        $("#dvAddMode").hide();
        $("#imgAddExpr").attr("src", "/Content/Images/plusajouter1616.png");
    });

    $("#btnValideExprGenRsq").kclick(function () {
        var idExprComplexe = $("input[type=radio][name=setExpr]:checked").val();
        var codeExprComplexe = $("#hidExpr¤" + idExprComplexe).val();
        var typeExpr = $("#TypeExpressionComplexe").val();
        var libExpr = $("#libExp¤" + idExprComplexe).val();
        if (typeof (idExprComplexe) != "undefined" && idExprComplexe != "") {
            SetExprComplexeGenRsq(typeExpr, idExprComplexe, codeExprComplexe, libExpr);
            $("#TypeAppel").val('');
            $("#divDataExprComp").html("");
            $("#divExprComplexe").hide();
            ReactivateShortCut();
        }
        else {
            ShowCommonFancy("Warning", "", "Veuillez sélectionner une expression complexe", 300, 80, true, true);
        }
    });
    $("#btnCloseExprGenRsq").kclick(function () {
        if ($("#TypeOperation").val() == "Ajout" || $("#TypeOperation").val() == "") {
            var typeVue = $("#TypeExpressionComplexe").val();
            var typeAppel = $("#TypeAppel").val();
            var vue_appel = typeVue + typeAppel;
            $("#idLienCpx_" + vue_appel).val('0');
            $("#LienCpx_" + vue_appel).addClass("None");
            $("#divType_" + vue_appel).removeClass("None");
            $("#Unite_" + vue_appel).val("");
            $("#Unite_" + vue_appel).trigger("change");
            $("#Type_" + vue_appel).val("");
            $("#Valeur_" + vue_appel).removeAttr('readonly', 'readonly').removeClass('readonly');
            $("#divLienCpx_" + vue_appel).addClass("None");
        }

        $("#divDataExprComp").html("");
        $("#divExprComplexe").hide();
    });

    $("input[type=radio][name=setExpr]").offOn("change", function () {
        if ($("#CurrentConditionExprComp").val() != "") {
            $("#btnValideExpr").show();   
        }
    });
    $("img[name=editExpr]").kclick(function () {
        if ($(this).hasClass("CursorPointer")) {
            var val = $(this).attr('id').split('¤')[1];
            $("#setExpr¤" + val).attr("checked", "checked");
            DisplayDetailsExpr(val);
        }
    });
    $("img[name=readExpr]").kclick(function () {
        if ($(this).hasClass("CursorPointer")) {
            var val = $(this).attr("id").split("¤")[1];
            if ($("#ExprReadOnly").val() != "True")
                $("#setExpr¤" + val).attr("checked", "checked");

            DisplayDetailsExpr(val, true);
        }
    });
    $("img[name='copyExpr']").kclick(function () {
        if ($(this).hasClass("CursorPointer")) {
            var val = $(this).attr("id").split("¤")[1];
            DuplicateExpr(val);
        }
    });
    $("img[name=delExpr]").kclick(function () {
        if ($(this).hasClass("CursorPointer")) {
            ConfirmDeleteExpression($(this));
        }
    });

    $("input[type='radio'][name='radAddBtn']").offOn('change', function () {
        $("#dvAddMode").hide();
        $("#imgAddExpr").attr("src", "/Content/Images/plusajouter1616.png");
        if ($(this).val() == "ref" || $(this).val() == "mdl") {
            OpenReferentiel($(this).val());
        }
        else {
            OpenAddExpr();
        }
    });

    $("td[name=linkCode]").kclick(function () {
        let val = $(this).attr('id').split('_')[1];
        if (val != undefined && val != "") {
            DisplayDetailsExpr(val);
        }
    });

    $("th[name='headerTriExpr']").kclick(function () {
        var colTri = $(this).attr("albcontext");
        var img = $("img[albcontext=" + colTri + "]").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1);
        img = img.substr(0, img.lastIndexOf('.'));
        TrierExprComplexe(colTri, img);
    });
}
//-----------Duplique l'expression complexe sélectionnée------------
function DuplicateExpr(val) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var typeExpr = $("#TypeExpressionComplexe").val();
    var codeExpr = val;

    var splitCharHtml = $("#SplitHtmlChar").val();
    $.ajax({
        type: "POST",
        url: "/ConditionsGarantie/DuplicateExpr",
        //async: false,
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, typeExpr: typeExpr, codeExpr: codeExpr },
        success: function (data) {
            if (data.split(splitCharHtml).length > 1) {
                common.dialog.error(data.split(splitCharHtml)[1]);
            }
            else {
                if ($("#isGenRsq").val() == "True") {
                    OpenExpComplexeGenRsq(typeExpr, $("#TypeAppel").val(), data, "DuplicateExpr");
                }
                else {
                    conditionsGaranties.openExpComplexe(typeExpr, $("#CurrentConditionExprComp").val(), data, "", "DuplicateExpr");
                }
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------Affiche les détails de l'expression sélectionnée-----------
function DisplayDetailsExpr(val, readonly) {
    if (readonly == undefined)
        readonly = false;
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var typeExpr = $("#TypeExpressionComplexe").val();
    var codeExpr = $("#hidExpr¤" + val).val();
    var isReadOnly = !$("#OffreReadOnly").val() ? readonly : $("#OffreReadOnly").val();
    var isModif = $("#modifExpr¤" + val).val();
    if (val == "" || val == "undefined") codeExpr = "0";

    ShowLoading();
    $.ajax({
        type: "POST",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, typeExpr: typeExpr, codeExpr: codeExpr, isReadOnly: isReadOnly, isModif: isModif, modeNavig: $("#ModeNavig").val() },
        context: $("#tblDetail"),
        url: "/ConditionsGarantie/DisplayDetails",
        success: function (data) {
            DesactivateShortCut();
            $(this).html(data);
            MapElementDetailsExpr();
            AlbScrollTop();

            common.autonumeric.apply($(this).find("input[albMask=decimal]"), 'init', 'decimal', '', null, null, '99999999999.99', null);
            common.autonumeric.apply($(this).find("span[albMask=decimal]"), 'init', 'decimal', null, null, null, '99999999999.99', null);
            common.autonumeric.apply($(this).find("input[albMask=pourcentdecimal]"), 'init', 'pourcentdecimal', '');
            common.autonumeric.apply($(this).find("span[albMask=pourcentdecimal]"), 'init', 'pourcentdecimal');
            common.autonumeric.apply($(this).find("input[albMask=pourmilledecimal]"), 'init', 'pourmilledecimal', '');
            common.autonumeric.apply($(this).find("span[albMask=pourmilledecimal]"), 'init', 'pourmilledecimal');

            common.autonumeric.apply($(this).find("input[id^=LCIValeurCPX¤]"), 'update', 'decimal', ' ', null, null, '99999999999.99', null);
            common.autonumeric.apply($(this).find("input[id^=ConcurrenceValeurCPX¤]"), 'update', 'decimal', ' ', null, null, '99999999999.99', null);

            $("#divInfos").show();
            $("#divInfoExpr").removeClass("divInfoExprMax").addClass("divInfoExpr");
            if (val == "") {
                OpenAddLineDetailsExpr();
            }

            if (isModif != "N" && !window.isReadonly) {
                $("#LibelleInput").removeAttr("readonly").removeClass("readonly");
                $("#imgAjt").show();
                $("#descriptif").removeAttr("readonly").removeClass("readonly");
                $("#saveDetails").show();
            }
            else {
                $("#LibelleInput").attr("readonly", "readonly").addClass("readonly");
                $("#imgAjt").hide();
                $("#descriptif").attr("readonly", "readonly").addClass("readonly");
                $("#saveDetails").hide();
            }

            $("#LibelleInput").focus();
            $("input[type=radio][name=setExpr]").attr("disabled", "disabled");
            $("img[name='editExpr']").removeClass("CursorPointer");
            $("img[name='copyExpr']").removeClass("CursorPointer");
            $("img[name='delExpr']").removeClass("CursorPointer");
            $("#imgAddExpr").removeClass("CursorPointer").attr("src", "/Content/Images/plusajouter_gris1616.jpg");

            $("#dvButton").hide();

            if (val != "") {
                var divInfoExpr = $("#divInfoExpr");
                var linePos = $("td[id='code_" + val + "']").position().top;
                divInfoExpr.scrollTop(linePos - divInfoExpr.position().top);
            }

            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request, true);
            ;
        }
    });
}
//-------------Ouvre la partie pour ajouter une nouvelle expresion-------------
function OpenAddExpr() {
    $("input[name=setExpr]").removeAttr("checked");
    DisplayDetailsExpr("");
    $("#NewExpr").val("1");
}
//------------------Map les éléments du détail de l'expression complexe sélectionnée-------------
function MapElementDetailsExpr() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });

    AlternanceLigne("Detail", "", false, null);
    $("#LibelleInput").val($("#LibelleTemp").val());
    $("#IdExpression").val($("#Idtemp").val());
    $("#descriptif").val($("#DescriptifTemp").val());

    $("#imgAjt").click(function () {
        OpenAddLineDetailsExpr();
    });
    $("#cancelDetails").die().live('click', function () {
        CancelLineDetailsExpr();
        ReactivateShortCut();
    });
    $("#saveDetails").die().live('click', function () {
        SaveLineDetailsExpr();
    });
    $("img[name=delDetail]").each(function () {
        $(this).click(function () {
            ConfirmDeleteLineDetailsExpr($(this));
        });
    });
    $("select[selecttype='FRHCPX']").each(function () {
        $(this).change(function () {
            ChangeUnitFRH($(this));
        });
    });
    $("#btnInfoOk").die().live('click', function () {
        CloseCommonFancy();
    });

    $("select[id^=FranchiseUniteCPX]").each(function () {
        $(this).change(function () {
            var codeCpx = $(this).attr('id').split('¤')[1];
            if (conditionsGaranties) {
                conditionsGaranties.formatFranchiseCPX(codeCpx);
            }
            else {
                FormatFranchiseCPX(codeCpx);
            }
        });
    });

    $("select[id^=LCIUniteCPX]").each(function () {
        $(this).change(function () {
            var codeCpx = $(this).attr('id').split('¤')[1];
            if (typeof conditionsGaranties !== "undefined") {
                conditionsGaranties.formatLCICPX(codeCpx);
            }
            else {
                FormatLCICPX(codeCpx);
            }
        });
    });

    $("select[id^=ConcurrenceUniteCPX]").each(function () {
        $(this).change(function () {
            var codeCpx = $(this).attr('id').split('¤')[1];
            if (typeof conditionsGaranties !== "undefined") {
                conditionsGaranties.formatConcurrenceCPX(codeCpx);
            }
            else {
                FormatConcurrenceCPX(codeCpx);
            }
        });
    });
}
//---------------Affiche la ligne d'ajout de détail de l'expression complexe-------------
function OpenAddLineDetailsExpr() {
    if ($("#imgAjt").hasClass("CursorPointer")) {
        $("#tr000").show();
        $("#LCIUniteCPX¤000").val("");
        $("#LCIType¤000").val("");
        $("#LCIUniteCPX¤000").val("");
        $("#FranchiseType¤000").val("");
        $("#FranchiseDebut¤000").val("");
        $("#FranchiseFin¤000").val("");
        $("#LCIUniteCPX¤000").val("");
        $("#ConcurrenceType¤000").val("");

        if ($("#TypeExpressionComplexe").val() == "LCI")
            $("#LCIValeurLCIUniteCPX¤000").focus();
        else
            $("#FranchiseValeurLCIUniteCPX¤000").focus();

        $("#imgAjt").attr("src", "/Content/Images/plusajouter_gris1616.jpg").removeClass("CursorPointer");
    }
}
//-----------Sauvegarde la partie ajout détail de l'expression complexe--------------
function SaveLineDetailsExpr() {
    RemoveRequiredCSS();
    //vérifie que le libellé est bien renseigné
    if ($("#LibelleInput").val() == "") {
        ShowCommonFancy("Error", "", "Le libellé est obligatoire", 300, 70, true, true);
        $("#LibelleInput").addClass("requiredField");
        return false;
    }

    var error = false;

    var codeExpr = $("input[name=setExpr]:checked").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var typeExpr = $("#TypeExpressionComplexe").val();
    var libelle = $("#LibelleInput").val();
    var description = $("#descriptif").val();
    var dataRow = "[";

    $("#tblDetail tr[id^=tr]:visible").each(function () {
        var id = $(this).attr("id").replace("tr", "");
        if (CheckValueDetail(id)) {
            error = true;
        }
        var regId = new RegExp("¤" + id, 'g');
        var regItem = new RegExp("item.", 'g');
        dataRow += JSON.stringify($(this).find(":input").serializeObject()).replace(regId, "").replace(regItem, "") + ",";
    });

    dataRow = dataRow.substr(0, dataRow.length - 1) + "]";

    if (error) {
        common.dialog.error("Veuillez corriger les informations en rouges");
        return false;
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ConditionsGarantie/SaveDetails",
        data: { codeOffre: codeOffre, version: version, type: type, typeExpr: typeExpr, codeExpr: codeExpr, libelle: libelle, description: description, obj: dataRow },
        dataType: 'json',
        success: function (data) {
            if ($("#isGenRsq").val() == "True") {
                OpenExpComplexeGenRsq(typeExpr, $("#TypeAppel").val(), data, "SaveExpr");
            }
            else {
                conditionsGaranties.openExpComplexe(typeExpr, $("#CurrentConditionExprComp").val(), data, "", "SaveExpr");
            }
            $("#NewExpr").val("");
            $("#imgAjt").attr("src", "/Content/Images/plusajouter1616.png").addClass("CursorPointer");

            $("input[type=radio][name=setExpr]").removeAttr("disabled");
            $("img[name='editExpr']").addClass("CursorPointer");
            $("img[name='copyExpr']").addClass("CursorPointer");
            $("img[name='delExpr']").addClass("CursorPointer");
            $("#imgAddExpr").addClass("CursorPointer").attr("src", "/Content/Images/plusajouter1616.png");
            $("#divInfos").hide();
            $("#divInfoExpr").removeClass("divInfoExpr").addClass("divInfoExprMax");
            $("#dvButton").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------------Demande confirmation pour la suppression d'une ligne de détail--------------
function ConfirmDeleteLineDetailsExpr(elem) {
    $("#CurrentDetail").val(elem.attr("id").split("¤")[1]);
    ShowCommonFancy("Confirm", "DelDetailExpr", "Veuillez confirmer la suppression du détail", 350, 130, true, true);
}
//--------------Change l'unité pour la franchise complexe------------
function ChangeUnitFRH(elem) {
    var FranchiseUnite = elem.val();
    var codeCondition = elem.attr('id').split('¤')[1];
    if (FranchiseUnite == 'UM') {
        $("#FranchiseMini¤" + codeCondition).val("").attr('readonly', 'readonly').addClass('readonly');
        $("#FranchiseMaxi¤" + codeCondition).val("").attr('readonly', 'readonly').addClass('readonly');
    }
    else {
        $("#FranchiseMini¤" + codeCondition).removeAttr('readonly').removeClass('readonly');
        $("#FranchiseMaxi¤" + codeCondition).removeAttr('readonly').removeClass('readonly');
    }
}
//-------Formate la franchise CPX-------------
function FormatFranchiseCPX(codeCondition) {
    $("#FranchiseValeurCPX¤" + codeCondition).attr('albMask', 'decimal');
    common.autonumeric.apply($("#FranchiseValeurCPX¤" + codeCondition), 'update', 'decimal', '', null, null, '99999999999.99', null);
}
//-------Formate la LCI CPX-------------
function FormatLCICPX(codeCondition) {
    //Suppression du formatage % LCI/Franchise bug 2130
    $("#LCIValeurCPX¤" + codeCondition).attr('albMask', 'decimal');
    common.autonumeric.apply($("#LCIValeurCPX¤" + codeCondition), 'update', 'decimal', '', null, null, '99999999999.99', null);

}
//-------Formate la Concurrence CPX-------------
function FormatConcurrenceCPX(codeCondition) {
    var lciUnite = $("#ConcurrenceUniteCPX¤" + codeCondition).val();
    if (lciUnite == '%') {
        var lciVal = $("#ConcurrenceValeurCPX¤" + codeCondition).val();
        if (parseFloat(lciVal) < 0 || parseFloat(lciVal) > 100) {
            $("#ConcurrenceValeurCPX¤" + codeCondition).val("");
        }
        $("#ConcurrenceValeurCPX¤" + codeCondition).attr('albMask', 'pourcentdecimal');
        common.autonumeric.apply($("#ConcurrenceValeurCPX¤" + codeCondition), 'update', 'pourcentdecimal', '');
    }
    else {
        $("#ConcurrenceValeurCPX¤" + codeCondition).attr('albMask', 'decimal');
        common.autonumeric.apply($("#ConcurrenceValeurCPX¤" + codeCondition), 'update', 'decimal', '', null, null, '99999999999.99', null);
    }
}
//------------Enlève le CSS requiredField---------------
function RemoveRequiredCSS() {
    $("#LibelleInput").removeClass("requiredField");

    $("input[id^=LCIValeur]").removeClass("requiredField");
    $("select[id^=LCIUnite]").removeClass("requiredField");
    $("select[id^=LCIType]").removeClass("requiredField");
    $("input[id^=ConcurrenceValeur]").removeClass("requiredField");
    $("select[id^=ConcurrenceUnite]").removeClass("requiredField");
    $("select[id^=ConcurrenceType]").removeClass("requiredField");

    $("input[id^=FranchiseValeur]").removeClass("requiredField");
    $("select[id^=FranchiseUnite]").removeClass("requiredField");
    $("select[id^=FranchiseType]").removeClass("requiredField");
    $("input[id^=FranchiseMini]").removeClass("requiredField");
    $("input[id^=FranchiseMaxi]").removeClass("requiredField");
    $("input[id^=FranchiseDebut]").removeClass("requiredField");
    $("input[id^=FranchiseFin]").removeClass("requiredField");
}
//-----------------Vérifie les données de la ligne--------------------------
function CheckValueDetail(idRow) {
    var error = false;
    var typeExpr = $("#TypeExpressionComplexe").val();
    if (typeExpr == "LCI") {
        if ((CheckLCIValue(idRow))) {
            error = true;
        }
    }
    else {
        let check = conditionsGaranties.checkFranchiseValue || CheckFranchiseValue;
        if (check(idRow)) {
            error = true;
        }
    }
    return error;
}
//-----------------Vérifie les données de la LCI-----------------------
function CheckLCIValue(idRow) {
    var error = false;

    var lciVal = $("#LCIValeurCPX¤" + idRow).autoNumeric('get');
    var lciUnite = $("#LCIUniteCPX¤" + idRow).val();
    var lciType = $("#LCIType¤" + idRow).val();
    //Tests uniquement si une des valeurs est renseignées
    if (((lciVal != "" || lciVal != 0) && (lciUnite == "" || lciType == "")) ||
        lciUnite != "" && ((lciVal == "" || lciVal == 0) || lciType == "") ||
        lciType != "" && ((lciVal == "" || lciVal == 0) || lciUnite == "")) {

        $("#LCIValeurCPX¤" + idRow).addClass("requiredField");
        $("#LCIUniteCPX¤" + idRow).addClass("requiredField");
        $("#LCIType¤" + idRow).addClass("requiredField");

        error = true;
    }

    var concuVal = $("#ConcurrenceValeurCPX¤" + idRow).autoNumeric('get');
    var concuUnite = $("#ConcurrenceUniteCPX¤" + idRow).val();
    var concuType = $("#ConcurrenceType¤" + idRow).val();

    if ((concuVal != "" && concuUnite != "" && concuType != "" && concuVal != 0)
                ||
            (concuVal == "" && concuUnite == "" && concuType == "" && concuVal == 0)) { }
    else {
        $("#ConcurrenceValeurCPX¤" + idRow).addClass("requiredField");
        $("#ConcurrenceUniteCPX¤" + idRow).addClass("requiredField");
        $("#ConcurrenceType¤" + idRow).addClass("requiredField");
        error = true;
    }
    if (concuUnite == "%" && (parseFloat(concuVal) < 0 || parseFloat(concuVal) > 100)) {
        $("#ConcurrenceValeur¤" + idRow).addClass("requiredField");
        $("#ConcurrenceUnite¤" + idRow).addClass("requiredField");
        error = true;
    }
    return error;
}
//-----------------Vérifie les données de la LCI-----------------------
function CheckFranchiseValue(idRow) {
    var error = false;

    var fraVal = $("#FranchiseValeurCPX¤" + idRow).autoNumeric('get');
    var fraUnite = $("#FranchiseUniteCPX¤" + idRow).val();
    var fraType = $("#FranchiseType¤" + idRow).val();
    //Tests uniquement si une des valeurs est renseignées
    if (((fraVal != "" || fraVal != 0) && (fraUnite == "" || fraType == "")) ||
        fraUnite != "" && ((fraVal == "" || fraVal == 0) || fraType == "") ||
        fraType != "" && ((fraVal == "" || fraVal == 0) || fraUnite == "")) {

        $("#FranchiseValeurCPX¤" + idRow).addClass("requiredField");
        $("#FranchiseUniteCPX¤" + idRow).addClass("requiredField");
        $("#FranchiseType¤" + idRow).addClass("requiredField");

        error = true;
    }

    var fraMini = $("#FranchiseMini¤" + idRow).autoNumeric('get');
    var fraMaxi = $("#FranchiseMaxi¤" + idRow).autoNumeric('get');
    if (fraMini != "" && fraMaxi != "") {
        if (parseFloat(fraMini) > parseFloat(fraMaxi)) {
            $("#FranchiseMini¤" + idRow).addClass("requiredField");
            $("#FranchiseMaxi¤" + idRow).addClass("requiredField");
            error = true;
        }
    }
    var fraDebut = $("#FranchiseDebut¤" + idRow).val();
    var fraFin = $("#FranchiseFin¤" + idRow).val();
    if (fraDebut != "" && fraFin != "") {
        if (!checkDateById("FranchiseDebut¤" + idRow, "FranchiseFin¤" + idRow)) {
            $("#FranchiseDebut¤" + idRow).addClass("requiredField");
            $("#FranchiseFin¤" + idRow).addClass("requiredField");
            error = true;
        }
    }
    return error;
}
//--------------Demande confirmation pour la suppression d'une expression----------
function ConfirmDeleteExpression(elem) {
    var elemId = elem.attr("id")
    var utiliseExpr = $("#useExpr¤" + elemId.split("¤")[1]).val();
    if (utiliseExpr == "1") {
        common.dialog.error("Expression utilisée, impossible de la supprimer");
    }
    else {
        $("#CurrentExpression").val(elemId.split("¤")[1]);
        ShowCommonFancy("Confirm", "DelExpression", "Veuillez confirmer la suppression de l'expression", 350, 130, true, true);
    }
}
//------------Supprime l'expression-----------
function DeleteExpression() {
    var codeExpr = $("#CurrentExpression").val();

    var typeExpr = $("#TypeExpressionComplexe").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ConditionsGarantie/DeleteExpression",
        data: { typeExpr: typeExpr, typeAppel: $("#TypeAppel").val(), codeExpr: codeExpr },
        success: function (data) {
            $("#CurrentExpression").val("");

            if ($("#isGenRsq").val() == "True") {
                OpenExpComplexeGenRsq(typeExpr, $("#TypeAppel").val())
            }
            else {
                conditionsGaranties.openExpComplexe(typeExpr);
            }

            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Supprime la ligne de détail-------------
function DeleteLineDetailsExpr() {
    ShowLoading();
    var codeDetail = $("#CurrentDetail").val();
    var typeExpr = $("#TypeExpressionComplexe").val();
    $.ajax({
        type: "POST",
        url: "/ConditionsGarantie/DeleteDetails",
        data: { typeExpr: typeExpr, codeDetail: codeDetail },
        success: function (data) {
            $("#CurrentDetail").val("");
            var val = $("input[type=radio][name=setExpr]:checked").val();
            DisplayDetailsExpr(val);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-----------Annule et ferme la partie ajout détail de l'expression complexe-------------
function CancelLineDetailsExpr() {
    if ($("#ExprReadOnly").val() != "True")
        $("input[name=setExpr]").removeAttr("disabled");
    $("img[name='editExpr']").addClass("CursorPointer");
    $("img[name='copyExpr']").addClass("CursorPointer");
    $("img[name='delExpr']").addClass("CursorPointer");
    $("#imgAddExpr").addClass("CursorPointer").attr("src", "/Content/Images/plusajouter1616.png");
    $("#dvButton").show();
    $("#divInfos").hide();
    $("#divInfoExpr").removeClass("divInfoExpr").addClass("divInfoExprMax");
    if ($("#TypeExpressionComplexe").val() == "LCI") {
        $("#LCIunite¤" + $("#IdExpComp").val()).val("");
    }
    else {
        $("#FranchiseUnite¤" + $("#IdExpComp").val()).val("");
    }
    $("#imgAjt").attr("src", "/Content/Images/plusajouter1616.png").addClass("CursorPointer");
    $("#addExpr").removeAttr("disabled");
}


//---------Ouverture de la div des expressions du référentiels------------
function OpenReferentiel(mode) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ConditionsGarantie/OpenReferentiel",
        data: { mode: mode, type: $("#TypeExpressionComplexe").val(), codeExpr: "", isReadOnly: $("#OffreReadOnly").val() },
        success: function (data) {
            $("#divLstExpression").show();
            $("#divDataLstExpression").html(data);
            $("#btnCloseExprRef").die().live('click', function () {
                $("#divLstExpression").hide();
                $("#divDataLstExpression").html("");
                $("#ajoutManuel").attr("checked", "checked");
            });
            $("#btnValideExprRef").die().live('click', function () {
                if (!$(this).is(':disabled'))
                    ValidSelExprRef();
            });
            CloseLoading();
            MapElementExpCompReferentiel();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Map les éléments des expressions complexes du référentiel----------
function MapElementExpCompReferentiel() {
    AlternanceLigne("BodyLstExprCompRef", "", false, null);
    $("#imgSearchExprComp").die().live('click', function () {
        SearchExprComp();
    });
    $("input[type='radio'][name='selExprRef']").each(function () {
        $(this).change(function () {
            $("#btnValideExprRef").show();
            //$("#btnValideExprRef").removeAttr("disabled");
        });
    });
    $("#btnValideExprRef").hide();
    //$("#btnValideExprRef").attr("disabled", "disabled");
}
//-----------Recherche une expression complexe-----------}
function SearchExprComp() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ConditionsGarantie/SearchExprReferentiel",
        data: { type: $("#TypeExpressionComplexe").val(), codeExpr: $("#CodeExpr").val() },
        success: function (data) {
            $("#lstExpression").html(data);
            CloseLoading();
            MapElementExpCompReferentiel();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------Valide/Copie l'expression complexe choisie-------------
function ValidSelExprRef() {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var typeExpr = $("#TypeExpressionComplexe").val();
    var idExpr = $("input[name='selExprRef']:checked").val();
    var modeRef = $("#modeReferentiel").val();
    var splitCharHtml = $("#SplitHtmlChar").val();
    $.ajax({
        type: "POST",
        url: "/ConditionsGarantie/ValidSelExprReferentiel",
        data: { codeOffre: codeOffre, version: version, type: type, mode: modeRef, typeExpr: typeExpr, idExpr: idExpr, splitCharHtml: splitCharHtml },
        success: function (data) {
            if (data.split(splitCharHtml).length > 1) {
                common.dialog.error(data.split(splitCharHtml)[1]);
            }
            else {
                if ($("#isGenRsq").val() == "True") {
                    OpenExpComplexeGenRsq(typeExpr, $("#TypeAppel").val(), data, "ValidExpr");
                }
                else {
                    conditionsGaranties.openExpComplexe(typeExpr, $("#CurrentConditionExprComp").val(), data, "", "ValidExpr");
                }

                $("#divLstExpression").hide();
                $("#divDataLstExpression").html("");
                $("#ajoutManuel").attr("checked", "checked");
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function TrierExprComplexe(colTri, img) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ConditionsGarantie/TrierExprComplexe",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            typeExpr: $("#TypeExpressionComplexe").val(),
            typeAppel: $("#TypeAppel").val(),
            modeNavig: $("#ModeNavig").val(),
            colTri: colTri,
            imgTri: img
        },
        success: function (data) {
            $("#divInfoExpr").html(data);
            MiseAJourImagesTri(colTri, img);
            MapeElementExprGenRsq();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request, true);
        }
    });
}

function MiseAJourImagesTri(colTri, img) {
    switch (colTri) {
        case "CodeExpr":
            if (img == "tri_asc") {
                $("img[albcontext='CodeExpr']").attr("src", "/Content/Images/tri_desc.png");
            }
            else {
                $("img[albcontext='CodeExpr']").attr("src", "/Content/Images/tri_asc.png");
            }
            $("img[albcontext='LibelleExpr']").attr("src", "/Content/Images/tri.png");
            break;
        case "LibelleExpr":
            if (img == "tri_asc") {
                $("img[albcontext='LibelleExpr']").attr("src", "/Content/Images/tri_desc.png");
            }
            else {
                $("img[albcontext='LibelleExpr']").attr("src", "/Content/Images/tri_asc.png");
            }
            $("img[albcontext='CodeExpr']").attr("src", "/Content/Images/tri.png");
            break;
    }
}