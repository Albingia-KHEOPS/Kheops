
$(document).ready(function () {
    MapElementPage();
    
});

//----------------Map les éléments de la page------------------
function MapElementPage() {
    $("#btnMatriceRisque").removeAttr('disabled');
    $("#btnMatriceGarantie").removeAttr('disabled');
    $("#checkElemSortis").removeAttr('readonly').removeAttr('disabled');

    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Delete":
                DeleteFormule();
                break;
            case "DelFormGen":
                DeleteFormuleGen();
                break;
            case "Duplicate":
                DuplicateFormule();
                break;
        }
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

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

    $("#btnForm2").live("click", function () {
        Redirection("FormuleGarantie", "Index", 0, 0);
    });

    $("#btnSuivant").live('click', function () {
        if ($("#txtSaveCancel").val() == "1")
            Redirection("RechercheSaisie", "Index");
        else
            Redirection("Engagements", "Index");
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

    $("#btnMatriceRisque").live('click', function () {
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

    $("td[name^='formuleHover_']").each(function () {
        $(this).bind("mouseover", function () {
            let codeFormule = $(this).attr("id").replace("formuleHover", "");
            let blockConditions = $(this).attr("albBlockCondition");
            let suppFormule = $(this).attr("albSupprFormule");
            let isSorti = $(this).attr("albSorti");
            contextMenu.createContextMenuFormule(codeFormule, blockConditions, suppFormule, isSorti);
            $(this).unbind("mouseover");
        });

        $(this).click(function (e) {
            $("#currentNameFormule").val($(this).attr('name'));
            $("#currentRsqFormule").val($(this).attr('albrsq'));

            e.preventDefault();
            var pos = $(this).offset();
            $(this).contextMenu({ x: pos.left, y: pos.top + $(this).height() });
        });
    });

    $("#updateFormule").live('click', function () {
        var codeFormule = $(this).attr('name').split('_')[1];
        Redirection("CreationFormuleGarantie", "Index", codeFormule, 1);
    });
    $("#copyFormule").live('click', function () {
        if (!window.isReadonly) {
            $("#CodeFormule").val($(this).attr('name').split('_')[1]);
            ShowCommonFancy("Confirm", "Duplicate", "Êtes-vous sûr de vouloir dupliquer la formule " + $(this).attr('name').split('_')[2] + " ?", 300, 130, true, true);
        }
    });
    $("#createOption").live('click', function () {
        if (!window.isReadonly) {
            Redirection("GestionOptions", "Index", "", "");
        }
    });
    $("#deleteFormule").live('click', function () {
        if (!window.isReadonly) {
            $("#CodeFormule").val($(this).attr('name').split('_')[1]);
            ShowCommonFancy("Confirm", "Delete", "Êtes-vous sûr de vouloir supprimer la formule " + $(this).attr('name').split('_')[2] + " ?", 300, 130, true, true);
        }
    });
    $("#conditionFormule").live('click', function () {
        var codeFormule = $(this).attr('name').split('_')[1];
        Redirection("ConditionsGarantie", "Index", codeFormule, 1);
    });

    // Gestion du sous-menu des options
    $("td[name^=optionHover][class~=CursorPointer]").each(function () {
        //$(this).bind('mouseover', function () {
        //    OpenSubMenuOption($(this));
        //});
        $(this).bind('mouseout', function () {
            CloseSubMenuOption($(this));
        });
    });
    //$("#subMenuOption").bind('mouseover', function () {
    //    $(this).show();
    //});
    $("#subMenuOption").bind('mouseout', function () {
        $(this).hide();
    });
    $("#updateOption").live('click', function () {
        alert('MAJ option');
    });
    $("#deleteOption").live('click', function () {
        alert('Suppression option');
    });

    $("#AddFormGen").die().live('click', function () {
        Redirection("CreationFormuleGarantie", "Index", 0, 0, 1);
    });
    $("#DelFormGen").die().live('click', function () {
        ShowCommonFancy("Confirm", "DelFormGen", "Etes-vous certain de vouloir supprimer la formule générale ?", 400, 80, true, true);
    });

    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }

    $("#checkElemSortis").die().live('click', function () {
        if ($(this).is(':checked')) {
            $("[albShowAvt='elemSorti']").removeClass("None");
            var countCell = $("#tblHeaderFormule tr td").length;
            $("#tblHeaderFormule").removeAttr("style");
            $("#tldBodyFormule").removeAttr("style");
            $("#tblHeaderFormule").attr('width', 170 * countCell / 3 + "px");
            $("#tldBodyFormule").attr('width', 170 * countCell / 3 + "px");
        }
        else {
            $("[albShowAvt='elemSorti']").addClass("None");
            var countCell = $("#tblHeaderFormule tr td[albShowAvt='elem']").length;
            $("#tblHeaderFormule").removeAttr("style");
            $("#tldBodyFormule").removeAttr("style");
            $("#tblHeaderFormule").attr('width', 170 * countCell + "px");
            $("#tldBodyFormule").attr('width', 170 * countCell + "px");
        }
    });

    var countCell = $("#tblHeaderFormule tr td[albShowAvt='elem']").length;
    $("#tblHeaderFormule").removeAttr("style");
    $("#tblHeaderFormule").attr('width', 170 * countCell + "px");
    $("#tldBodyFormule").removeAttr("style");
    $("#tldBodyFormule").attr('width', 170 * countCell + "px");
}
//-----------------Redirection----------------------
function Redirection(cible, job, codeFormule, codeOption, formGen, isForcereadOnly) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var paramRedirect = $("#txtParamRedirect").val();
    var modeNavig = $("#ModeNavig").val();

    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    if (isForcereadOnly) {
        addParamValue += "||FORCEREADONLY|1";
        $("#IsModeConsultationEcran").val("True");
    }

    $.ajax({
        type: "POST",
        url: "/MatriceFormule/Redirection/",
        data: {
            cible: cible, job: job, codeOffre: codeOffre, version: version, type: type,
            codeFormule: codeFormule, codeOption: codeOption, tabGuid: tabGuid, formGen: formGen,
            paramRedirect: paramRedirect, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Duplication de la formule--------------
function DuplicateFormule() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/MatriceFormule/DuplicateFormule",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeFormule: $("#CodeFormule").val(), tabGuid: tabGuid, modeNavig: modeNavig,
            addParamType: addParamType, addParamValue: addParamValue
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Suppression de la formule-----------------
function DeleteFormule() {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/MatriceFormule/DeleteFormule",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeFormule: $("#CodeFormule").val(), tabGuid: tabGuid, modeNavig: modeNavig,
            addParamType: addParamType, addParamValue: addParamValue
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

    $("#CodeFormule").val("");
}
//---------------Déclenche le scroll du tableau-------------
function ScrollTable(elem) {
    $("#divRisque").scrollTop(elem.scrollTop());
    $("#divHeadFormule").scrollLeft(elem.scrollLeft());
}
//-------------Ouvre le sous-menu des options-------------------
function OpenSubMenuOption(elem) {
    var position = elem.offset();

    var nameElem = elem.attr('name').replace('optionHover', '');

    $("#updateOption").attr('name', 'updateOption' + nameElem);
    $("#deleteOption").attr('name', 'deleteOption' + nameElem);

    var subMenuDiv = $("#subMenuOption");
    subMenuDiv.css({ 'position': 'absolute', 'top': position.top + 15 + 'px', 'left': position.left + 2 + 'px' });
    subMenuDiv.show();
}
//-------------Ferme le sous-menu des options-------------------
function CloseSubMenuOption(elem) {
    $("#subMenuOption").hide();
}
//-------------Ouvre le sous-menu des formules-------------------
function OpenSubMenuFormule(elem) {
    var position = elem.offset();

    var nameElem = elem.attr('name').replace('formuleHover', '');

    $("#updateFormule").attr('name', 'updateFormule' + nameElem);
    $("#copyFormule").attr('name', 'copyFormule' + nameElem);
    $("#createOption").attr('name', 'createOption' + nameElem);
    $("#deleteFormule").attr('name', 'deleteFormule' + nameElem);
    $("#conditionFormule").attr('name', 'conditionFormule' + nameElem);

    var subMenuDiv = $("#subMenuFormule");
    subMenuDiv.css({ 'position': 'absolute', 'top': position.top + 36 + 'px', 'left': position.left + 2 + 'px' });
    subMenuDiv.show();
}
//-------------Ferme le sous-menu des options-------------------
function CloseSubMenuFormule(elem) {
    $("#subMenuFormule").hide();
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

//----------Modifier une formule de garantie---------
function MenuUpdateFormule() {
    var codeFormule = $("#currentNameFormule").val().split('_')[1];
    Redirection("CreationFormuleGarantie", "Index", codeFormule, 1);
}

function MenuConsultFormule() {
    var codeFormule = $("#currentNameFormule").val().split('_')[1];

    Redirection("CreationFormuleGarantie", "Index", codeFormule, 1, null, true);
}
//---------Dupliquer une formule de garantie------------
function MenuDuplicateFormule() {
    if (!window.isReadonly) {
        $("#CodeFormule").val($("#currentNameFormule").val().split('_')[1]);
        var codeRsq = $("#currentRsqFormule").val();
        if (!ValidRsq(codeRsq)) {
            common.dialog.error("Veuillez valider le risque " + codeRsq + " lié à cette formule");
            return;
        }
        else {
            ShowCommonFancy("Confirm", "Duplicate", "Êtes-vous sûr de vouloir dupliquer la formule " + $("#currentNameFormule").val().split('_')[2] + " ?", 300, 130, true, true);
        }
    }
}
//---------Créer une option---------
function MenuCreateOption() {
    if (!window.isReadonly) {
        Redirection("GestionOptions", "Index", "", "");
    }
}
//-----------Supprimer une formule de garantie-----------
function MenuDeleteFormule() {
    if (!window.isReadonly) {
        $("#CodeFormule").val($("#currentNameFormule").val().split('_')[1]);
        var codeRsq = $("#currentRsqFormule").val();
        if (!ValidRsq(codeRsq)) {
            common.dialog.error("Veuillez valider le risque " + codeRsq + " lié à cette formule");
            return;
        }
        else {
            ShowCommonFancy("Confirm", "Delete", "Êtes-vous sûr de vouloir supprimer la formule " + $("#currentNameFormule").val().split('_')[2] + " ?", 300, 130, true, true);
        }
    }
}
//-----------Ouvre les conditions d'une formule--------
function MenuOpenCondition() {
    var codeFormule = $("#currentNameFormule").val().split('_')[1];
    var codeRsq = $("#currentRsqFormule").val();
    if (!ValidRsq(codeRsq)) {
        common.dialog.error("Veuillez valider le risque " + codeRsq + " lié à cette formule");
        return;
    }
    else {
        Redirection("ConditionsGarantie", "Index", codeFormule, 1);
    }
}

function ValidRsq(codeRsq) {
    var result = true;
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/MatriceFormule/GetValidRsq",
        data: { codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeRsq: codeRsq },
        async: false,
        success: function (data) {
            CloseLoading();
            if (data != "") result = true;
            else result = false;
        },
        error: function (request) {
            common.error.showXhr(request);
            result = false;
        }
    });
    return result;
}
