$(document).ready(function () {
    MapElementPage();
    infosSpeRisque.displayParamRisque();
    CheckFields();
    //---------Risque------------
    ClickSuivantRisque();
    AlternanceLigne("OppositionsBody", "noInput", true, null);
    infosSpeRisque.formatDecimalNumricValue();
    //---------------------------
    //---------Objet------------
    $("#btnSuivantObjet").unbind();
    $("#btnSuivantObjet").click(function () {
        SuivantObjet();
        if (ValidateOffreSimp("OffreSimpObjet")) {
            LoadDataOffreSimp("OffreSimpGarantie");
        } else {
            ShowCommonFancy("Error", "", "Champs obligatoires non remplis", 350, 180, true, true);
            return;
        }
    });

    ClickSuivantObjet();
    //--------------------------
    //---------Garantie--------
    ClickSuivantGarantie();
    //-------------------------
    ClickSuivant();
});
//----------------Map les éléments de la page------------------
function MapElementPage() {
    $('#tabRisque').unbind();
    $('#tabRisque').live('click', function () {
        AfficherInformationsSpecifiquesRisque();
    });
    $('#tabObjet').unbind();
    $('#tabObjet').live('click', function () {
        AfficherInformationsSpecifiquesObjet();
    });
    $('#tabGarantie').unbind();
    $('#tabGarantie').live('click', function () {
        AfficherInformationsSpecifiquesGarantie();
    });
    $("#Objet").live('change', function () {
        GetDetailInformationsSpecifiquesObjet($("#Objet").val());
    });
    //$("#Formule").live('change', function () {
    //    GetListOptions($("#Formule").val());
    //    GetDetailInformationsSpecifiquesGarantie($("#Option").val());

    //});
    //$("#Option").live('change', function () {
    //    GetDetailInformationsSpecifiquesGarantie($("#Option").val());
    //});
    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                CancelForm();
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });
    $("#btnErrorOk").live('click', function () {
        CloseCommonFancy();
    });
    //------------Risque----------
    //gestion de l'affichage de l'écran en mode readonly
    if (window.isReadonly) {
        $("#btnFermerOppositions").removeAttr("disabled");
    }
    $("#btnAnnulerRisque").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });

    $("#btnOppositions").die().live('click', function () {
        AlbScrollTop();
        DesactivateShortCut();
        $("#divFullScreenListeOpposition").show();
    });

    $("#btnAjouter").die().live('click', function () {
        infosSpeRisque.obtenirOppositionDetails("", "I");
    });

    $("img[name=btnEditerOpposition]").each(function () {
        $(this).click(function () {
            infosSpeRisque.obtenirOppositionDetails($(this).attr("id"), "U");
        });
    });

    $("img[name=btnSupprimerOpposition]").each(function () {
        $(this).click(function () {
            infosSpeRisque.obtenirOppositionDetails($(this).attr("id"), "D");
        });
    });

    $("#btnActionOpposition").die().live('click', function () {
        infosSpeRisque.enregistrerOpposition();
    });

    $("#btnFermerEditOpposition").die().live('click', function () {
        $("#divFullScreenEditOpposition").hide();
    });

    $("#btnFermerOppositions").die().live('click', function () {
        ReactivateShortCut();
        $("#divFullScreenListeOpposition").hide();
    });

    $("#chkRisque").die().live('change', function () {
        infosSpeRisque.cocherDecocherObjetsRisque($("#chkRisque").is(':checked'));
    });

    $("input[name=chkObjet]").die().live('change', function () {
        infosSpeRisque.cocherDecocherRisque();
    });
    //----------------------------
    //----Objet-------------------
    $("#btnAnnulerObjet").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });
    //----------------------------
}
//----Affiche la tab Informations spécifiques Risque-------
function AfficherInformationsSpecifiquesRisque() {
    $("#CurrentTab").val("Risque");

    $("#divInfosSpecifiquesRisque").show();
    $("#divInfosSpecifiquesObjet").hide();
    $("#divInfosSpecifiquesGarantie").hide();

    $("#divActionButtonsRisque").show();
    $("#divActionButtonsObjet").hide();
    $("#divActionButtonsGarantie").hide();

    $('#tabRisque').removeClass("onglet");
    $('#tabRisque').addClass("onglet-actif");

    $("#tabObjet").removeClass("onglet-actif");
    $("#tabObjet").addClass("onglet");

    $("#tabGarantie").removeClass("onglet-actif");
    $("#tabGarantie").addClass("onglet");

}
//----Affiche la tab Informations spécifiques Objet-------
function AfficherInformationsSpecifiquesObjet() {
    $("#CurrentTab").val("Objet");

    if ($("#isTabObjetActif").val() == "false") {
        return;
    }
    $("#divInfosSpecifiquesRisque").hide();
    $("#divInfosSpecifiquesObjet").show();
    $("#divInfosSpecifiquesGarantie").hide();

    $("#divActionButtonsRisque").hide();
    $("#divActionButtonsObjet").show();
    $("#divActionButtonsGarantie").hide();

    $('#tabObjet').removeClass("onglet");
    $('#tabObjet').addClass("onglet-actif");

    $("#tabRisque").removeClass("onglet-actif");
    $("#tabRisque").addClass("onglet");

    $("#tabGarantie").removeClass("onglet-actif");
    $("#tabGarantie").addClass("onglet");


}
//----Affiche la tab Informations spécifiques Garantie-------
function AfficherInformationsSpecifiquesGarantie() {
    $("#CurrentTab").val("Garantie");

    if ($("#isTabGarantieActif").val() == "false")
        return;
    $("#divInfosSpecifiquesRisque").hide();
    $("#divInfosSpecifiquesObjet").hide();
    $("#divInfosSpecifiquesGarantie").show();

    $("#divActionButtonsRisque").hide();
    $("#divActionButtonsObjet").hide();
    $("#divActionButtonsGarantie").show();

    $('#tabGarantie').removeClass("onglet");
    $('#tabGarantie').addClass("onglet-actif");

    $("#tabRisque").removeClass("onglet-actif");
    $("#tabRisque").addClass("onglet");

    $("#tabObjet").removeClass("onglet-actif");
    $("#tabObjet").addClass("onglet");

}
//----Affiche les information spécifiques de l'Objet sélectionné-------
function GetDetailInformationsSpecifiquesObjet(codeObjet) {
    if (codeObjet == '') {
        $("#divDetailInfosSpecificsObjet").html('');
        return;
    }

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();

    var codeRisque = $("#CodeRisque").val();
    var descriptifRisque = $("#DescriptifRisque").val();
    var descriptifObjet = $('#Objet option:selected').text().substr(codeObjet.length + 3);
    $.ajax({
        type: "POST",
        data: { codeOffre: codeOffre, version: version, type: type, codeRisque: codeRisque, descriptifRisque: descriptifRisque, codeObjet: codeObjet, descriptifObjet: descriptifObjet },
        url: "/InformationsSpecifiquesOffreSimp/GetDetailInformationsSpecifiquesObjet",
        success: function (data) {
            DesactivateShortCut();
            $("#divDetailInfosSpecificsObjet").html(data);
            FormatDecimalNumericValue();
            LoadDataOffreSimp("OffreSimpObjet");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//--Charge la liste des options correspondants à la formule sélectionnée--
function GetListOptions(codeFormule) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var codeRisque = $("#CodeRisque").val();
    var acteGestion = $("#ActeGestion").val();

    $.ajax({
        type: "POST",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeFormule: codeFormule, codeRisque: codeRisque, acteGestion: acteGestion },
        url: "/InformationsSpecifiquesOffreSimp/GetListOptions",
        success: function (data) {
            DesactivateShortCut();
            $("#divListOptions").html(data);

        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----Affiche les information spécifiques de la formule sélectionnée-------
function GetDetailInformationsSpecifiquesGarantie(codeOption) {
    if (codeOption == '') {
        $("#divDetailInfosSpecificsGarantie").html('');
        return;
    }
    var codeOffre = $("#Offre_CodeOffre").val();
    var libelleFormule = $("#libelleFormule").val();
    var lettreLibelleFormule = $("#lettreLibelleFormule").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeFormule = $("#Formule").val();
    var codeRisque = $("#CodeRisque").val();
    var acteGestion = $("#ActeGestion").val();

    $.ajax({
        type: "POST",
        data: { codeOffre: codeOffre, version: version, type: type, codeOption: codeOption, codeFormule: codeFormule, codeRisque: codeRisque, acteGestion: acteGestion },
        url: "/InformationsSpecifiquesOffreSimp/GetDetailInformationsSpecifiquesGarantie",
        success: function (data) {
            DesactivateShortCut();
            $("#divDetailInfosSpecificsGarantie").html(data);
            FormatDecimalNumericValue();
            LoadDataOffreSimp("OffreSimpGarantie");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//-------Formate les input/span des valeurs----------
function FormatDecimalNumericValue() {
    $('input[albMask="numeric"]').each(function (e) {
        common.autonumeric.applyAll('init', $(this).attr("albMask"), null, null, 0, null, null);
    });

    $('input[albMask="decimal"]').each(function (e) {
        common.autonumeric.applyAll('init', $(this).attr("albMask"), null, null, $(this).attr("albDecimal"), null, null);
    });
}
function LoadDataOffreSimp(context) {
    var params = $("#Params[albcontext=" + context + "]").val();
    const dataParams = {
        branche: encodeURIComponent(infosSpe.getParameterByName(params, "branche")),
        section: encodeURIComponent(infosSpe.getParameterByName(params, "section")),
        splitChars: $("#splitChar").val(),
        strParameters: $("#SpecificParams[albcontext=" + context + "]").val(),
        additionalParams: '',
        cible: ''
    };

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/DynamicGuiIS/Ajax/DbInteraction.asmx/LoadDBData",
        data: JSON.stringify(dataParams),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#dvISGtiesResult[albcontext=" + context + "]").html(data.d);
            if (context == "OffreSimpObjet") {
                $("#dvISGtiesResult[albcontext=" + context + "]").addClass("dvISGtiesResultObjet");
            }
            else if (context == "OffreSimpGarantie") {
                $("#dvISGtiesResult[albcontext=" + context + "]").addClass("dvISGtiesResultGarantie");
            }
            //Init Data
            infosSpe.init();
            ToScrollTopMenuOffreSimp(context);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------Risque----------------------------
//----------------------Suivant---------------------
function SuivantRisque(redirection) {

    $("#tabGuid").removeAttr("disabled");
    $("#Offre_CodeOffre").removeAttr("disabled");
    $("#Offre_Version").removeAttr("disabled");
    $("#Offre_Type").removeAttr("disabled");
    $("#txtSaveCancel").removeAttr("disabled");

    $("#txtParamRedirect").removeAttr("disabled");

    $("#Code").removeAttr("disabled");


    if ($("#RegimeTaxe").val() == "") {
        $("#RegimeTaxe").addClass('requiredField');
        return false;
    }
    ShowLoading();
    var type = $("#Offre_Type").val();
    var formDataInitial = JSON.stringify($(':input').serializeObject());
    var txtParamRedirect = $("#txtParamRedirect").val();
    if (type == "P") {
        formData = formDataInitial.replace("Contrat.CodeContrat", "Offre.CodeOffre").replace("Contrat.VersionContrat", "Offre.Version").replace("Contrat.Type", "Offre.Type");
    }
    else {
        formData = formDataInitial;
    }
    $.ajax({
        type: "POST",
        url: "/InformationsSpecifiquesOffreSimp/SaveInfosSpecifiquesRisque",
        context: $('#JQueryHttpPostResultDiv'),
        data: formData,
        contentType: "application/json",
        success: function (data) {
            var dialogBox = $(this);
            $(dialogBox).hide();
            if (data.ActionTag == "Nav") {
                $(this).append(data.Script);
            }

            if (data.ActionTag == "Error") {
                $('.ui-dialog-title').fadeIn().text('Alerte Erreur');
                $('.ui-dialog-content').fadeIn().text("Erreur à l'enregistrement!");
                $(dialogBox).dialog('open');
                $("#ContainerDiv").hide();
            }

            CloseLoading();
            $("#isTabObjetActif").val('true');

            if (redirection == true) {
                LinkNavigation(txtParamRedirect);
            }
            if (redirection == undefined)
                AfficherInformationsSpecifiquesObjet();



        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------------Reset de la page---------------------
function CancelForm() {
    RedirectionRisque("DetailsRisque", "Index");
}
//----------------Redirection------------------
function RedirectionRisque(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeRisque = $("#Code").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/InformationsSpecifiquesOffreSimp/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeRisque: codeRisque, tabGuid: tabGuid, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------
//----------------Objet---------------------------
//----------------------Suivant---------------------
function SuivantObjet(redirection) {

    if (!ValidateOffreSimp("OffreSimpObjet")) {
        ShowCommonFancy("Error", "", "Champs obligatoires non remplis", 400, 200, true, true);
        return;
    }

    ShowLoading();
    if ($("#txtSaveCancel").val() == "1")
        RedirectionObjet("RechercheSaisie", "Index", true, redirection);
    else
        RedirectionObjet("DetailsRisque", "Index", true, redirection);
    //Redirection("DetailsRisque", "Index");
    //OA_ReadCellsFromExcel();
    //window.document.location.href = "http://webexcel/ExcelValidate.htm";

}

//----------------Redirection------------------
function RedirectionObjet(cible, job, withSave, redirection) {
    try {
        ShowLoading();
        if (withSave == undefined)
            withSave = false;

        //var branche = encodeURIComponent($("#Branche").val());
        var branche = encodeURIComponent($("#dvISGtiesResult[albContext=OffreSimpObjet] #Branche").val());

        //var section = encodeURIComponent($("#section").val());
        var section = encodeURIComponent($("#dvISGtiesResult[albContext=OffreSimpObjet] #section").val());
        //var cible = '';
        var additionalParams = '';
        var dataToSave = encodeURIComponent(infosSpe.GetValues());
        //var splitChars = encodeURIComponent($("#jsSplitChar").val());
        var splitChars = encodeURIComponent($("#dvISGtiesResult[albContext=OffreSimpObjet] #jsSplitChar").val());
        //var strParameters = encodeURIComponent($("#parameters").val());
        var strParameters = encodeURIComponent($("#dvISGtiesResult[albContext=OffreSimpObjet] #parameters").val());
        //if (!Validate())
        //    return;

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeRisque = $("#CodeRisque").val();
        var codeObjet = $("#CodeObjet").val();
        var tabGuid = $("#tabGuid").val();
        var txtParamRedirect = $("#txtParamRedirect").val();
        $.ajax({
            type: "POST",
            url: "/InformationsSpecifiquesOffreSimp/SaveInfosSpecifiquesObjet/",
            data: {
                branche: branche, section: section, additionalParams: additionalParams, dataToSave: dataToSave
                    , splitChars: splitChars, strParameters: strParameters
                 , cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeRisque: codeRisque, codeObjet: codeObjet, tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), withSave: withSave
            },
            success: function (data) {
                if (redirection == true) {
                    LinkNavigation(txtParamRedirect);
                }
                $("#isTabGarantieActif").val('true');
                if (redirection == undefined)
                    AfficherInformationsSpecifiquesGarantie();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    } catch (e) {
        common.dialog.smallError("Erreur de sauvegarde des informations spécifiques", true);
        CloseLoading();
    }
}
//---------------------------------------------------------------------
//------------------Garantie---------------------------------
function SuivantGarantie() {
    try {
        //var branche = encodeURIComponent($("#Branche").val());
        var branche = encodeURIComponent($("#dvISGtiesResult[albContext=OffreSimpGarantie] #Branche").val());
        //var section = encodeURIComponent($("#section").val());
        var section = encodeURIComponent($("#dvISGtiesResult[albContext=OffreSimpGarantie] #section").val());
        var cible = '';
        var additionalParams = '';
        var dataToSave = encodeURIComponent(infosSpe.GetValues());
        //var splitChars = encodeURIComponent($("#jsSplitChar").val());
        var splitChars = encodeURIComponent($("#dvISGtiesResult[albContext=OffreSimpGarantie] #jsSplitChar").val());
        //var strParameters = encodeURIComponent($("#parameters").val());
        var strParameters = encodeURIComponent($("#dvISGtiesResult[albContext=OffreSimpGarantie] #parameters").val());
        if (!ValidateOffreSimp("OffreSimpGarantie"))
            return;

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeFormule = $("#CodeFormule").val();
        var codeOption = $("#CodeOption").val();
        var libelleFormule = $("#LibelleFormule").val();
        var lettreLibelleFormule = $("#LettreLibelleFormule").val();
        var txtParamRedirect = $("#txtParamRedirect").val();
        var tabGuid = $("#tabGuid").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        ShowLoading();
        if ($("#txtSaveCancel").val() == "1") {
            Redirection("RechercheSaisie", "Index");
        }
        else {
            $.ajax({
                type: "POST",
                url: "/InformationsSpecifiquesOffreSimp/RedirectionVersClauseSimplifiee",
                data: {
                    branche: branche, section: section, additionalParams: additionalParams, dataToSave: dataToSave
                    , splitChars: splitChars, strParameters: strParameters,
                    argCodeOffre: codeOffre, argVersion: version, argType: type, argCodeFormule: codeFormule, argCodeOption: codeOption, argLibelleFormule: libelleFormule, argLettreLibelleFormule: lettreLibelleFormule, tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(),
                    addParamType: addParamType, addParamValue: addParamValue
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }

    } catch (e) {
        common.dialog.smallError("Erreur de sauvegarde des informations spécifiques", true);
        CloseLoading();
    }
}
//------------------------------------------------------------
function ToScrollTopMenuOffreSimp(context) {
    if ($("#dvIsMenu").val() == undefined)
        return;
    $("span[name=albISLink]").each(function () {
        $(this).click(function () {
            $('#dvISGtiesResult[albcontext=' + context + ']').scrollTop(0);
            var scrollDiv = $(this).attr('albISLink');
            var topDiv = $('#dvISGtiesResult[albcontext=' + context + ']').position().top;
            var top = $('#' + scrollDiv).position().top;
            $('#dvISGtiesResult[albcontext=' + context + ']').scrollTop(top - topDiv);
        });
    });
}
//-------------
function LinkNavigation(txtParamRedirect) {

    $("#tabGuid").removeAttr("disabled");
    $("#Offre_CodeOffre").removeAttr("disabled");
    $("#Offre_Version").removeAttr("disabled");
    $("#Offre_Type").removeAttr("disabled");
    $("#txtSaveCancel").removeAttr("disabled");
    $("#txtParamRedirect").removeAttr("disabled");
    $("#txtParamRedirect").val(txtParamRedirect);
    $("#Code").removeAttr("disabled");

    ShowLoading();
    var type = $("#Offre_Type").val();
    var formDataInitial = JSON.stringify($(':input').serializeObject());
    if (type == "P") {
        formData = formDataInitial.replace("Contrat.CodeContrat", "Offre.CodeOffre").replace("Contrat.VersionContrat", "Offre.Version").replace("Contrat.Type", "Offre.Type");
    }
    else {
        formData = formDataInitial;
    }

    $.ajax({
        type: "POST",
        url: "/InformationsSpecifiquesOffreSimp/LinkNavigation",
        context: $('#JQueryHttpPostResultDiv'),
        data: formData,
        contentType: "application/json",
        success: function (data) {
            var dialogBox = $(this);
            $(dialogBox).hide();
            if (data.ActionTag == "Nav") {
                $(this).append(data.Script);
            }

            if (data.ActionTag == "Error") {
                $('.ui-dialog-title').fadeIn().text('Alerte Erreur');
                $('.ui-dialog-content').fadeIn().text("Erreur à l'enregistrement!");
                $(dialogBox).dialog('open');
                $("#ContainerDiv").hide();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function ValidateOffreSimp(context) {
    $("#dvISGtiesResult[albcontext=" + context + "] .requiredField").removeClass("requiredField");
    var result = true;
    $("#dvISGtiesResult[albcontext=" + context + "] :input[albrequired='O']").each(function () {
        if (!$(this).attr("disabled") != "" && $(this).attr("disabled") != "disabled") {
            if ($(this).hasClass("datepicker")) {
                if (!infosSpe.checkDate($(this).val())) {
                    $(this).addClass("requiredField");
                    result = false;
                }
            }
            if ($(this).val() == "") {
                $(this).addClass("requiredField");
                result = false;
            }
        }
    });
    return result;
}
function ClickSuivantRisque() {
    $("#btnSuivantRisque").unbind();
    $("#btnSuivantRisque").live('click', function () {
        SuivantRisque();
        LoadDataOffreSimp("OffreSimpObjet");
    });
}
function ClickSuivantObjet() {
    $("#btnSuivantObjet").unbind();
    $("#btnSuivantObjet").click(function () {
        SuivantObjet();
        if (ValidateOffreSimp("OffreSimpObjet")) {
            LoadDataOffreSimp("OffreSimpGarantie");
        }
    });
}
function ClickSuivantGarantie() {
    $("#btnSuivantGarantie").unbind();
    $("#btnSuivantGarantie").live('click', function () {
        SuivantGarantie();
    });
}
function ClickSuivant() {
    $("#btnSuivant").kclick(function (evt, data) {
        if ($("#CurrentTab").val() == "Garantie") {
            SuivantGarantie();
        }
        else if ($("#CurrentTab").val() == "Objet") {
            SuivantObjet(true);
        }
        else {
            SuivantRisque(true);
        }
    });
}