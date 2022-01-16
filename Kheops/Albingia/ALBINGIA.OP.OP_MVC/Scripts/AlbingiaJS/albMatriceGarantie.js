
$(document).ready(function () {
    MapElementPage();
    InitTable();
});

//---------------- Map les éléments de la page------------------
function MapElementPage() {
    $("#btnMatriceFormule").removeAttr('disabled');
    $("#btnMatriceRisque").removeAttr('disabled');

    $("#btnAddRisque").live('click', function () {
        if (!$(this).attr('disabled')) {
            Redirection("DetailsObjetRisque", "Index");
        }
    });

    $("#btnAddFormule").live('click', function () {
        if (!$(this).attr('disabled')) {
            Redirection("CreationFormuleGarantie", "Index", 0, 0);
        }
    });

    $("#btnSuivant").kclick(function (evt, data) {
        if (data && data.returnHome) {
            Redirection("RechercheSaisie", "Index");
            DeverouillerUserOffres(common.tabGuid);
        }
        else {
            Redirection("Engagements", "Index");
        }
    });

    $("#btnAnnuler").live('click', function () {
        //Redirection("ModifierOffre", "Index");
        var type = $("#Offre_Type").val();
        if (type == "O")
            Redirection("ModifierOffre", "Index");
        else if (type == "P")
            Redirection("AnInformationsGenerales", "Index");
    });
    $("#divFormule").bind('scroll', function () {
        ScrollTable($(this));
    });
    $("img[id^=img¤]").live('click', function () {
        ToggleRisque($(this));
    });
    $("#garExpandCollapse").live('click', function () {
        var isOpened = $("#garExpandCollapseVal").val();
        if (isOpened == "0") {
            $(this).attr('src', '/Content/Images/Op.png');
            $("#garExpandCollapseVal").val("1");
        }
        else {
            $(this).attr('src', '/Content/Images/Cl.png');
            $("#garExpandCollapseVal").val("0");
        }
        $("img[id^=img¤]").each(function () {
            $("#input_" + $(this).attr('id')).val(isOpened);
            $(this).trigger('click');
        });
    }); $("#btnMatriceRisque").live('click', function () {
        if (!$(this).attr('disabled')) {
            Redirection("MatriceRisque", "Index");
        }
    });
    $("#btnMatriceFormule").live('click', function () {
        if (!$(this).attr('disabled')) {
            Redirection("MatriceFormule", "Index");
        }
    });
    $("#btnMatriceGarantie").live('click', function () {
        if (!$(this).attr('disabled')) {
            Redirection("MatriceGarantie", "Index");
        }
    });

    $("td[name=lnkRisque]").each(function () {
        $(this).click(function () {
            Redirection("DetailsRisque", "Index", 0, 0, $(this).attr('albCodeRsq'));
        });
    });

    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "DelFormGen":
                DeleteFormuleGen();
                break;
        }
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    $("#AddFormGen").die().live('click', function () {
        Redirection("CreationFormuleGarantie", "Index", 0, 0, null, 1);
    });
    $("#DelFormGen").die().live('click', function () {
        ShowCommonFancy("Confirm", "DelFormGen", "Etes-vous certain de vouloir supprimer la formule générale ?", 400, 80, true, true);
    });

    if (window.isReadonly || window.isModifHorsAvenant) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
}

//-----------------Redirection----------------------
function Redirection(cible, job, codeFormule, codeOption, codeRisque, formGen) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var paramRedirect = $("#txtParamRedirect").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/MatriceGarantie/Redirection/",
        data: {
            cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeFormule: codeFormule, codeOption: codeOption,
            codeRisque: codeRisque, tabGuid: tabGuid, formGen: formGen, paramRedirect: paramRedirect, modeNavig: $("#ModeNavig").val(),
            addParamType: addParamType, addParamValue: addParamValue
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//--------------- Déclenche le scroll du tableau-------------
function ScrollTable(elem) {
    $("#divRisque").scrollTop(elem.scrollTop());
    $("#divHeadFormule").scrollLeft(elem.scrollLeft());
}

//--------------- Initialise le tableau -----------------------------------
function InitTable() {
    $("#tblBodyRisque input").each(function () {
        var id = $(this).attr("id");
        $("#" + id).val("1");
    });
    
   
}

//--------------- Dépile/Rempile les objets du risque sélectionné-----------
function ToggleRisque(elem) {
    var isOpened = $("#input_" + elem.attr('id')).val();
    if (isOpened == "0") {
        //InitTable();
        var origine = elem.attr('id').substring(4).split('¤')[0];
        var codeVolet = elem.attr('id').substring(4).split('¤')[1];
        var code = codeVolet;
        if (elem.attr('id').substring(4).split('¤').length > 2) {
            code = code + "¤" + elem.attr('id').substring(4).split('¤')[2];
        }
        if (origine == "bloc") {
            $("#img¤volet¤" + codeVolet).attr('src', '/Content/Images/Op.png');
            $("#input_img¤volet¤" + codeVolet).val("1");
            $("#tr¤volet¤" + codeVolet).show();
            $("tr[origine=volet¤" + codeVolet + "]").show();
            $("tr[origine$=" + elem.attr('id').substring(4) + "]").show();
        }
        else {
            $("tr[origine=" + elem.attr('id').substring(4) + "]").show();
        }
        elem.attr('src', '/Content/Images/Op.png');
        $("#input_" + elem.attr('id')).val("1");
    }
    else {
        var origine = elem.attr('id').substring(4).split('¤')[0];
        if (origine == "bloc") {
            $("tr[origine$=" + elem.attr('id').substring(4) + "]").hide();
        }
        else {
            $("img[id^=" + elem.attr('id').replace("volet", "bloc") + "]").attr('src', '/Content/Images/Cl.png');
            $("input[id^=input_" + elem.attr('id').replace("volet", "bloc") + "]").val("0");
            $("tr[origine^=" + elem.attr('id').substring(4) + "]").hide();
        }
        elem.attr('src', '/Content/Images/Cl.png');
        $("#input_" + elem.attr('id')).val("0");
    }
}
//--------------Suppression de la formule générale-----------------
function DeleteFormuleGen() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeFormule = $("#DelFormGen").attr("albCodeForm");
    var tabGuid = $("#tabGuid").val();
    var codeAvn = $("#NumAvenantPage").val();
    $.ajax({
        type: "POST",
        url: "/MatriceRisque/DeleteFormuleGen",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeFormule: codeFormule, tabGuid: tabGuid },
        success: function (data) {
            $("#divLibFormGen").html(" ").attr('style', 'width:860px;');
            $("#divImgFormGen").html("<img id='AddFormGen' class='CursorPointer' src='/Content/Images/plusajouter1616.png' title='Ajouter une formule générale' alt='AddFormGen' />");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
