$(document).ready(function () {
    MapElementPage();
});

//----------Map les éléments de la page-----------
function MapElementPage() {
    MapExprComElement();

    $("#btnCancel").die().live('click', function () {
        Redirection("BackOffice", "Index");
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "ExprAnnule":
                $("#divDetailExpComp").html("");
                break;
            case "DelExpr":
                DelExprComp();
                break;
            case "DelRowExprComp":
                DelRowExprComplexe();
                break;
        }
    });
    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val("");
        $("#currentId").val("");
    });

    $("#btnExprSearch").die().live('click', function () {
        if (!$("input[name='searchCheck']").is(':checked')) {
            common.dialog.error("Veuillez sélectionner le type d'expression complexe à rechercher.");
        }
        else {
            var typeExpr = $("input[name='searchCheck']:checked").val();;
            var strSearch = $("input[id='searchExpr']").val();
            SearchExprComp(typeExpr, strSearch);
        }
    });
    $("#btnExprRAZ").die().live('click', function () {
        var typeExpr = $("input[name='searchCheck']:checked").val();
        if (typeExpr != undefined && typeExpr != "") {
            $("input[name='searchCheck']").removeAttr("checked");
            $("input[id='searchExpr']").val("");
            SearchExprComp("LCI", "");
            SearchExprComp("Franchise", "");
        }
    });
}

function Redirection(cible, job) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamClause/Redirection/",
        data: { cible: cible, job: job },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Map les éléments des listes de LCI/Franchise-----------
function MapExprComElement() {
    $("#imgAddLCI").die().live('click', function () {
        OpenExprComp('LCI', '0');
    });

    $("img[name=imgUpdLCI]").each(function () {
        $(this).click(function () {
            OpenExprComp('LCI', $(this).attr("id").split('_')[1]);
        });
    });

    $("img[name=imgDelLCI]").each(function () {
        $(this).click(function () {
            $("#currentType").val('LCI');
            $("#currentId").val($(this).attr("id").split('_')[1]);
            ShowCommonFancy("Confirm", "DelExpr", "Etes-vous sûr de vouloir supprimer cette expression régulière ?", 300, 80, true, true, true);
        });
    });

    $("#imgAddFRH").die().live('click', function () {
        OpenExprComp('Franchise', '0');
    });

    $("img[name=imgUpdFRH]").each(function () {
        $(this).click(function () {
            OpenExprComp('Franchise', $(this).attr("id").split('_')[1]);
        });
    });

    $("img[name=imgDelFRH]").each(function () {
        $(this).click(function () {
            $("#currentType").val('Franchise');
            $("#currentId").val($(this).attr("id").split('_')[1]);
            ShowCommonFancy("Confirm", "DelExpr", "Etes-vous sûr de vouloir supprimer cette expression régulière ?", 300, 80, true, true, true);
        });
    });

    AlternanceLigne("BodyLCI", "", true, null);
    AlternanceLigne("BodyFRH", "", true, null);

    $(".selectableCol").each(function () {
        $(this).click(function () {
            var type = $(this).attr("name").split("_")[0];
            var id = $(this).attr("name").split("_")[1];
            OpenExprComp(type, id);
        });
    });
}
//----------Ouvre les détails d'une expression complexes-----------
function OpenExprComp(typeExpr, codeExpr) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RefExprComp/LoadExpComp/",
        data: { typeExpr: typeExpr, codeExpr: codeExpr },
        success: function (data) {
            $("#divDetailExpComp").html(data);
            MapElementDetail();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Supprime une expression complexes--------------
function DelExprComp() {
    ShowLoading();
    var typeExpr = $("#currentType").val();
    $.ajax({
        type: "POST",
        url: "/RefExprComp/DeleteExprComp/",
        data: { idExpr: $("#currentId").val(), typeExpr: typeExpr },
        success: function (data) {
            $("#divDetailExpComp").html("");
            LoadListExprComplexe(typeExpr);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
    $("#divDetailExpComp").html("");

}
//---------Map les éléments des détails d'une expression complexe-----------
function MapElementDetail() {
    $("#IdRowExpr").val("");
    $("#btnExprAnnuler").die().live('click', function () {
        ShowCommonFancy("Confirm", "ExprAnnule", "Etes-vous sûr de vouloir annuler les détails de l'expression complexe ?", 300, 80, true, true, true);
    });
    $("#btnExprSave").die().live('click', function () {
        SaveDetailExpr();
    });
    $("img[name=saveDetail]").each(function () {
        $(this).click(function () {
            SaveRowExprComlexe($(this));
        });
    });
    $("img[name=delDetail]").each(function () {
        $(this).click(function () {
            if ($(this).hasClass("CursorPointer")) {
                var splitCharHtml = $("#splitCharHtml").val();
                $("#currentId").val($(this).attr("id").split(splitCharHtml)[1]);
                ShowCommonFancy("Confirm", "DelRowExprComp", "Etes-vous sûr de vouloir modifier cette ligne ?", 300, 80, true, true, true);
            }
        });
    });
    $("#imgAddExprLigne").die().live('click', function () {
        if ($(this).hasClass('CursorPointer')) {
            OpenNewLineExprComp();
        }
    });
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });

    $("td[name=lock]").each(function () {
        $(this).click(function () {
            if ($("#IdRowExpr").val() == "")
                OpenLineExprComp($(this));
        });
    });

    AlternanceLigne("BodyDetail", "", false, null);

    $("#btnExprSave").removeAttr('disabled');
    $("#imgAddExprLigne").attr("src", "/Content/Images/plusajouter1616.png").addClass("CursorPointer");
    $("img[name=delDetail]").attr("src", "/Content/Images/poubelle1616.png").addClass("CursorPointer");

    $("select[id^=LCIUniteCPX]").each(function () {
        $(this).change(function () {
            FormatLCI($(this));
        });
    });
    $("select[id^=ConcurrenceUniteCPX]").each(function () {
        $(this).change(function () {
            FormatConcurrence($(this));
        });
    });
    $("select[id^=FranchiseUniteCPX]").each(function () {
        $(this).change(function () {
            FormatFRH($(this));
        });
    });

    //common.autonumeric.apply($("input[id=CodeExpression]"), "init", "numeric", "", null, null, '99', null);
    common.autonumeric.apply($("input[albMask=decimal]"), "init", "decimal", "", null, null, '99999999999.99', null);
    common.autonumeric.apply($("span[albMask=decimal]"), "init", "decimal", "", null, null, '99999999999.99', null);
    common.autonumeric.apply($("input[albMask=pourcentdecimal]"), "init", "pourcentdecimal", "");
    common.autonumeric.apply($("span[albMask=pourcentdecimal]"), "init", "pourcentdecimal");
    common.autonumeric.apply($("input[albMask=pourmilledecimal]"), "init", "pourmilledecimal", "");
    common.autonumeric.apply($("span[albMask=pourmilledecimal]"), "init", "pourmilledecimal");

    $("#CodeExpression").keyup(function (e) {
        var keyCode = e.keyCode || e.which;
        var regex = new RegExp("^[a-zA-Z]+$");
        if (regex.test($(this).val())) {
            if (keyCode != 9 && keyCode != 16) $("#CodeExpression2").focus();
        }
        else
            $(this).val("");
    });

    $("#CodeExpression2").keyup(function (e) {
        var keyCode = e.keyCode || e.which;
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var value = String.fromCharCode(keyCode);

        if (!regex.test($(this).val())) {
            $(this).val("");
        }
    });

}
//---------Sauvegarde les détails de l'expression complexe---------
function SaveDetailExpr() {
    $(".requiredField").removeClass("requiredField");
    var isCorrect = true;
    if ($("#CodeExpression").val() == "") {
        $("#CodeExpression").addClass("requiredField");
        isCorrect = false;
    }
    if ($("#LibExpression").val() == "") {
        $("#LibExpression").addClass("requiredField");
        isCorrect = false;
    }
    if (isCorrect) {
        ShowLoading();
        var idExpr = $("#IdExpr").val();
        var typeExpr = $("#TypeExpr").val();
        var codeExpr = $("#TypeExpression").val() + $("#CodeExpression").val() + $("#CodeExpression2").val();
        var libExpr = $("#LibExpression").val();
        var modifExpr = $("#ModifExpression").is(":checked");
        var descrExpr = $("#DescrExpr").val();

        $.ajax({
            type: "POST",
            url: "/RefExprComp/SaveDetailExpr/",
            data: { idExpr: idExpr, typeExpr: typeExpr, codeExpr: codeExpr, libExpr: libExpr, modifExpr: modifExpr, descrExpr: descrExpr },
            success: function (data) {
                if (data == "-1") {
                    $("#CodeExpression").addClass("requiredField");
                    $("#CodeExpression2").addClass("requiredField");
                    common.dialog.error("Ce code est déjà existant");
                    return false;
                }
                OpenExprComp(typeExpr, data);
                LoadListExprComplexe(typeExpr);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

}
//--------------Charge la liste des expressions complexes en fonction du type d'expression-------
function LoadListExprComplexe(typeExpr) {
    $.ajax({
        type: "POST",
        url: "/RefExprComp/LoadListExprComp/",
        data: { typeExpr: typeExpr },
        success: function (data) {
            switch (typeExpr) {
                case "LCI":
                    $("#dvBodyLCI").html(data);
                    break;
                case "Franchise":
                    $("#dvBodyFRH").html(data);
                    break;
            }
            MapExprComElement();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------Sauvegarde la ligne de l'expression complexe-------------
function SaveRowExprComlexe(elem) {
    $(".requiredField").removeClass("requiredField");
    var splitCharHtml = $("#splitCharHtml").val();
    var idRowExpr = elem.attr('id').split(splitCharHtml)[1];

    var idExpr = $("#IdExpr").val();
    var typeExpComp = $("#TypeExpr").val();

    var valExpr = "";
    var unitExpr = "";
    var typeExpr = "";
    var concuValExpr = "";
    var concuUnitExpr = "";
    var concuTypeExpr = "";
    var valMinFrh = "";
    var valMaxFrh = "";
    var debFrh = "";
    var finFrh = "";

    var chkError = CheckInputError(typeExpComp, idRowExpr);

    if (chkError)
        return false;

    if (typeExpComp == "LCI") {
        valExpr = $("input[id='LCIValeur" + splitCharHtml + idRowExpr + "']").val();
        unitExpr = $("select[id='LCIUniteCPX" + splitCharHtml + idRowExpr + "']").val();
        typeExpr = $("select[id='LCIType" + splitCharHtml + idRowExpr + "']").val();
        concuValExpr = $("input[id='ConcurrenceValeurCPX" + splitCharHtml + idRowExpr + "']").val();
        concuUnitExpr = $("select[id='ConcurrenceUniteCPX" + splitCharHtml + idRowExpr + "']").val();
        concuTypeExpr = $("select[id='ConcurrenceType" + splitCharHtml + idRowExpr + "']").val();
    }
    if (typeExpComp == "Franchise") {
        valExpr = $("input[id='FranchiseValeurCPX" + splitCharHtml + idRowExpr + "']").val();
        unitExpr = $("select[id='FranchiseUniteCPX" + splitCharHtml + idRowExpr + "']").val();
        typeExpr = $("select[id='FranchiseType" + splitCharHtml + idRowExpr + "']").val();
        valMinFrh = $("input[id='FranchiseMini" + splitCharHtml + idRowExpr + "']").val();
        valMaxFrh = $("input[id='FranchiseMaxi" + splitCharHtml + idRowExpr + "']").val();
        debFrh = $("input[id='FranchiseDebut" + splitCharHtml + idRowExpr + "']").val();
        finFrh = $("input[id='FranchiseFin" + splitCharHtml + idRowExpr + "']").val();
    }
    if (valExpr == undefined || valExpr == "")
        valExpr = 0;
    if (concuValExpr == undefined || concuValExpr == "")
        concuValExpr = 0;


    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RefExprComp/SaveRowExprComplexe",
        data: {
            idExpr: idExpr, typeExpComp: typeExpComp, idRowExpr: idRowExpr,
            valExpr: valExpr, unitExpr: unitExpr, typeExpr: typeExpr, concuValExpr: concuValExpr, concuUnitExpr: concuUnitExpr, concuTypeExpr: concuTypeExpr,
            valMinFrh: valMinFrh, valMaxFrh: valMaxFrh, debFrh: debFrh, finFrh: finFrh
        },
        success: function (data) {
            LoadRowsExprComplexe(typeExpComp, idExpr);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------Vérifié la validité des champs----------
function CheckInputError(typeExp, idRowExpr) {
    var splitCharHtml = $("#splitCharHtml").val();
    var errorBool = false;

    if (typeExp == "LCI") {
        //----Vérification des valeurs/types/unités----
        var lciVal = $("input[id='LCIValeur" + splitCharHtml + idRowExpr + "']").val();
        var lciUnite = $("select[id='LCIUniteCPX" + splitCharHtml + idRowExpr + "']").val()
        var lciType = $("select[id='LCIType" + splitCharHtml + idRowExpr + "']").val();
        if (/*lciVal == "" || lciVal == 0 ||*/ lciUnite == "" || lciType == "") {
            //$("input[id='LCIValeur" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
            $("select[id='LCIUniteCPX" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
            $("select[id='LCIType" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
            errorBool = true;
        }
        //if (lciUnite == "%" && (parseFloat(lciVal) < 0 || parseFloat(lciVal) > 100)) {
        //    $("input[id='LCIValeur" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
        //    $("select[id='LCIUniteCPX" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
        //    errorBool = true;
        //}
        //----Vérification des valeurs/types/unités concurrence----
        var lciConVal = $("input[id='ConcurrenceValeurCPX" + splitCharHtml + idRowExpr + "']").val();
        var lciConUnite = $("select[id='ConcurrenceUniteCPX" + splitCharHtml + idRowExpr + "']").val()
        var lciConType = $("select[id='ConcurrenceType" + splitCharHtml + idRowExpr + "']").val();
        if ((lciConVal != "" && lciConVal != 0) || lciConUnite != "" || lciConType != "") {
            if (/*lciConVal == "" || lciConVal == 0 ||*/ lciConUnite == "" || lciConType == "") {
                //$("input[id='ConcurrenceValeurCPX" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
                $("select[id='ConcurrenceUniteCPX" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
                $("select[id='ConcurrenceType" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
                errorBool = true;
            }
            if (lciConUnite == "%" && (parseFloat(lciConVal) < 0 || parseFloat(lciConVal) > 100)) {
                $("input[id='ConcurrenceValeurCPX" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
                $("select[id='ConcurrenceUniteCPX" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
                errorBool = true;
            }
        }
    }

    if (typeExp == "Franchise") {
        //----Vérification des valeurs/types/unités----
        var fraVal = $("input[id='FranchiseValeurCPX" + splitCharHtml + idRowExpr + "']").val();
        var fraUnite = $("select[id='FranchiseUniteCPX" + splitCharHtml + idRowExpr + "']").val()
        var fraType = $("select[id='FranchiseType" + splitCharHtml + idRowExpr + "']").val();
        if (/*fraVal == "" || fraVal == 0 || */fraUnite == "" || fraType == "") {
            //$("input[id='FranchiseValeurCPX" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
            $("select[id='FranchiseUniteCPX" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
            $("select[id='FranchiseType" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
            errorBool = true;
        }
        //if (fraUnite == "%" && (parseFloat(fraVal) < 0 || parseFloat(fraVal) > 100)) {
        //    $("input[id='FranchiseValeurCPX" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
        //    $("select[id='FranchiseUniteCPX" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
        //    errorBool = true;
        //}
        //----Vérification des min/max----
        var fraMini = $("input[id='FranchiseMini" + splitCharHtml + idRowExpr + "']").autoNumeric('get');
        var fraMaxi = $("input[id='FranchiseMaxi" + splitCharHtml + idRowExpr + "']").autoNumeric('get');
        if (fraMini != "" && fraMaxi != "") {
            if (parseFloat(fraMini) > parseFloat(fraMaxi)) {
                $("input[id='FranchiseMini" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
                $("input[id='FranchiseMaxi" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
                errorBool = true;
            }
        }
        //----Vérification des dates----
        var fraDebut = $("input[id='FranchiseDebut" + splitCharHtml + idRowExpr + "']").val();
        var fraFin = $("input[id='FranchiseFin" + splitCharHtml + idRowExpr + "']").val();
        if (fraDebut != "" && fraFin != "") {
            if (!checkDate($("input[id='FranchiseDebut" + splitCharHtml + idRowExpr + "']"), $("input[id='FranchiseFin" + splitCharHtml + idRowExpr + "']"))) {
                $("input[id='FranchiseDebut" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
                $("input[id='FranchiseFin" + splitCharHtml + idRowExpr + "']").addClass("requiredField");
                errorBool = true;
            }
        }
    }

    return errorBool;
}
//----------Supprime la ligne de l'expression complexe----------
function DelRowExprComplexe(elem) {
    var splitCharHtml = $("#splitCharHtml").val();
    var idRowExpr = $("#currentId").val();

    var idExpr = $("#IdExpr").val();
    var typeExpComp = $("#TypeExpr").val();

    $("#currentId").val("");
    $.ajax({
        type: "POST",
        url: "/RefExprComp/DelRowExprComplexe",
        data: { idExpr: idExpr, typeExpComp: typeExpComp, idRowExpr: idRowExpr },
        success: function (data) {
            LoadRowsExprComplexe(typeExpComp, idExpr);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//-----------Recharge la liste des lignes de l'expression complexe--------
function LoadRowsExprComplexe(typeExpComp, idExpr) {
    $.ajax({
        type: "POST",
        url: "/RefExprComp/LoadRowsExprComplexe",
        data: { typeExpComp: typeExpComp, idExpr: idExpr },
        success: function (data) {
            $("#divBodyDetail").html(data);
            MapElementDetail();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Ouvre le mode edit pour une ligne d'expression complexe-----------
function OpenLineExprComp(elem) {
    var splitCharHtml = $("#splitCharHtml").val();
    var idRowExpr = elem.attr('albTdMode').split(splitCharHtml)[1];
    var typeRow = elem.attr('albType');

    $("td[albTdMode='tdLock" + splitCharHtml + idRowExpr + "'][albType='" + typeRow + "']").hide();
    $("td[albTdMode='tdEdit" + splitCharHtml + idRowExpr + "'][albType='" + typeRow + "']").show();

    $("#btnExprSave").attr('disabled', 'disabled');
    $("img[name=delDetail]").attr("src", "/Content/Images/poubelle1616_gris.png").removeClass("CursorPointer");
    $("#imgAddExprLigne").attr("src", "/Content/Images/plusajouter_gris1616.jpg").removeClass("CursorPointer");

    $("#IdRowExpr").val(idRowExpr);
}
//---------Ouvre une nouvelle ligne d'expression complexe---------
function OpenNewLineExprComp() {
    var splitCharHtml = $("#splitCharHtml").val();

    $("input[id='LCIValeur" + splitCharHtml + "0']").val("");
    $("select[id='LCIUniteCPX" + splitCharHtml + "0']").val("");
    $("select[id='LCIType" + splitCharHtml + "0']").val("");
    $("input[id='ConcurrenceValeurCPX" + splitCharHtml + "0']").val("");
    $("select[id='ConcurrenceUniteCPX" + splitCharHtml + "0']").val("");
    $("select[id='ConcurrenceType" + splitCharHtml + "0']").val("");

    $("input[id='FranchiseValeurCPX" + splitCharHtml + "0']").val("");
    $("select[id='FranchiseUniteCPX" + splitCharHtml + "0']").val("");
    $("select[id='FranchiseType" + splitCharHtml + "0']").val("");
    $("input[id='FranchiseMini" + splitCharHtml + "0']").val("");
    $("input[id='FranchiseMaxi" + splitCharHtml + "0']").val("");
    $("input[id='FranchiseDebut" + splitCharHtml + "0']").val("");
    $("input[id='FranchiseFin" + splitCharHtml + "0']").val("");


    $("#newExprComp").show();
    $("#btnExprSave").attr('disabled', 'disabled');
    $("img[name=delDetail]").attr("src", "/Content/Images/poubelle1616_gris.png").removeClass("CursorPointer");
    $("#imgAddExprLigne").attr("src", "/Content/Images/plusajouter_gris1616.jpg").removeClass("CursorPointer");
}
//---------Formatage des éléments de valeur LCI--------
function FormatLCI(elem) {
    var splitCharHtml = $("#splitCharHtml").val();
    var idRowExpr = elem.attr("id").split(splitCharHtml)[1];

    var lciUnite = $("select[id='LCIUniteCPX" + splitCharHtml + idRowExpr + "']").val();

    $("input[id='LCIValeur" + splitCharHtml + idRowExpr + "']").attr('albMask', 'decimal');
    common.autonumeric.apply($("input[id='LCIValeur" + splitCharHtml + idRowExpr + "']"), 'update', 'decimal', '', null, null, '99999999999.99', null);

}
//---------Formatage des éléments de concurrence LCI--------
function FormatConcurrence(elem) {
    var splitCharHtml = $("#splitCharHtml").val();
    var idRowExpr = elem.attr("id").split(splitCharHtml)[1];

    var conUnite = $("select[id='ConcurrenceUniteCPX" + splitCharHtml + idRowExpr + "']").val();
    if (conUnite == '%') {
        var conVal = $("input[id='ConcurrenceValeurCPX" + splitCharHtml + idRowExpr + "']").val();
        if (parseFloat(conVal) < 0 || parseFloat(conVal) > 100) {
            $("input[id='ConcurrenceValeurCPX" + splitCharHtml + idRowExpr + "']").val("");
        }
        $("input[id='ConcurrenceValeurCPX" + splitCharHtml + idRowExpr + "']").attr('albMask', 'pourcentdecimal');
        common.autonumeric.apply($("input[id='ConcurrenceValeurCPX" + splitCharHtml + idRowExpr + "']"), 'update', 'pourcentdecimal', '');
    }
    else {
        $("input[id='ConcurrenceValeurCPX" + splitCharHtml + idRowExpr + "']").attr('albMask', 'decimal');
        common.autonumeric.apply($("input[id='ConcurrenceValeurCPX" + splitCharHtml + idRowExpr + "']"), 'update', 'decimal', '', null, null, '99999999999.99', null);
    }
}
//---------Formatage des éléments de valeur FRH----------
function FormatFRH(elem) {
    var splitCharHtml = $("#splitCharHtml").val();
    var idRowExpr = elem.attr("id").split(splitCharHtml)[1];

    var frhUnite = $("select[id='FranchiseUniteCPX" + splitCharHtml + idRowExpr + "']").val();

    $("input[id='FranchiseValeurCPX" + splitCharHtml + idRowExpr + "']").attr('albMask', 'decimal');
    common.autonumeric.apply($("input[id='FranchiseValeurCPX" + splitCharHtml + idRowExpr + "']"), 'update', 'decimal', '', null, null, '99999999999.99', null);
}
//--------Recherche les expressions complexes----------
function SearchExprComp(typeExpr, strSearch) {
    ShowLoading();

    $.ajax({
        type: "POST",
        url: "/RefExprComp/SearchExprComp",
        data: { typeExpr: typeExpr, strSearch: strSearch },
        success: function (data) {
            switch (typeExpr) {
                case "LCI":
                    $("#dvBodyLCI").html(data);
                    break;
                case "Franchise":
                    $("#dvBodyFRH").html(data);
                    break;
            }
            MapExprComElement();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
