
$(document).ready(function () {
    MapElementPage();
});

//----------------Map les éléments de la page------------------
function MapElementPage() {
    $("#btnMatriceFormule").removeAttr('disabled');
    $("#btnMatriceGarantie").removeAttr('disabled');

    $("#btnAddRisque").live('click', function () {
        if (!$(this).attr('disabled')) {
            // Verifier si l'entete contient ou pas une adresse
            var haveToCopyAdr = false;
            if (isEnteteContainAddress() === true) {
                var message = "Voulez-vous copier l'adresse du contrat sur le nouveau risque";

                if ($("#Offre_Type").val() === 'O') {
                    message = "Voulez-vous copier l'adresse de l'offre sur le nouveau risque";
                }
                ConfirmDialog(message);
            }
            else {
                Redirection("DetailsObjetRisque", "Index");
            }

        }
    });

    $("#btnAddFormule").live('click', function () {
        if (!$(this).attr('disabled')) {
            Redirection("CreationFormuleGarantie", "Index", 0, 0, 0);
        }
    });

    $("#btnCopyRisque").live('click', function () {
       
        $("#divLstRsq").show();
        $("#divLstRsq").draggable(); 
    });

    $("#btnCancelListCopie").live('click', function () {
        $("#divLstRsq").hide();
    });
    
    $("#btnOuiListCopie").live('click', function () {
       
        $("#CopieBnsPb").val("True");
        Redirection("DetailsRisque", "DetailsRisqueCopier", 0, 0, 0);
    });
    $("#btnNanListCopie").live('click', function () {
       
        $("#CopieBnsPb").val("False");
        Redirection("DetailsRisque", "DetailsRisqueCopier", 0, 0, 0);
    });
    
    
    $("#btnValidListCopie").live('click', function () {
        var numRsq = $("input[name='radioRsq']:checked").val();
        if (numRsq == "" || numRsq == null) {
            common.dialog.bigError("Vous devez sélectionner un risque.", true);
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
        var type = $("#Offre_Type").val();
        if (type == "O")
            Redirection("ModifierOffre", "Index");
        else if (type == "P")
            Redirection("AnInformationsGenerales", "Index");
    });

    $("#divInfo").bind('scroll', function () {
        ScrollTable($(this));
    });
    $("img[id^=risque]").live('click', function () {
        ToggleRisque($(this));
    });
    $("#rsqExpandCollapse").live('click', function () {
        var isOpened = $("#rsqExpandCollapseVal").val();
        if (isOpened == "0") {
            $(this).attr('src', '/Content/Images/Op.png');
            $("#rsqExpandCollapseVal").val("1");
            //isOpened = 1;
        }
        else {
            $(this).attr('src', '/Content/Images/Cl.png');
            $("#rsqExpandCollapseVal").val("0");
            //isOpened = 0;
        }

        $("img[id^=risque]").each(function () {
            //if ($("img[id^=risque]").parent().parent().attr('albbaddate') != "True" || $("img[id^=risque]").parent().parent().is(":visible")) {
            $("input[id='input_" + $(this).attr('id') + "']").val(isOpened);
            //if ($("img[id^=risque]").parent().parent().attr('albbaddate') != "True" || $("#displayAllCB").is(":checked")) {
            $(this).trigger('click');
            //}
        });
    });
    $("td[name^=Risque]").each(function () {
        $(this).bind('click', function () {
            $("#CodeRsqSel").val($(this).attr('id').split("_")[1]);
            Redirection("DetailsRisque", "Index");
        });
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
    FormatDecimalNumericValue();

    $("#btnConfirmOk").live('click', function () {
        var numRsq = $("input[name='radioRsq']:checked").val();
        if (numRsq != "" || numRsq != null) {
            $("#CodeRsqSel").val($("input[name='radioRsq']:checked").val());
            Redirection("DetailsRisque", "DetailsRisqueCopier", 0, 0, 0);
        } else
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

    /*  $("#tdRsqFormule").live('click', function () {
          var codeFormuleRisque = $(this).attr('name').split('_')[1];
          var codeFormule = 0;
          switch (codeFormuleRisque) {
              case "A":
                  codeFormule = 1;
                  break;
              case "B":
                  codeFormule = 2;
                  break;
              case "C":
                  codeFormule = 3;
                  break;
              default:
                  codeFormule = 4;
          }
          
          $this.redirect({
              cible: "FormuleGarantie",
              job: "Index",
              codeFormule: 1,
              codeOption: 1,
              isForcereadOnly: true
          });
         
      */
    $("#checkboxCopy").live('click', function () {
        if ($(this).attr("checked") == "checked") {
            $("img[id='btnCopyRisque']").attr('style', 'visibility:visible');
        }
        else
            $("img[id='btnCopyRisque']").attr('style', 'visibility:hidden');
    });

    $("#checkboxCopy").live('change', function () {
        $('input[type="checkbox"]').not(this).prop('checked', false);
        var risqueLast = $(this).attr("name");
        var arrayLignes = document.getElementById("tableListRsq").rows;
        var longueur = arrayLignes.length + 1;
        var i = 1;

        while (i < longueur) {

            if ($(this).attr("checked") == "checked" && risqueLast == "risque_" + i) {
                $("#Copie_risque").html(i + " - " + $("#tdRsq_" + i).html());
                $("#CodeRsqSel").val(i);
                
            }
            i++;
        }
    });

    $("#displayAllCB").live('click', function () {
        if ($(this).attr("checked") == "checked") {

            $("tr[albbaddate='True']").each(function () {
                var niv = $(this).attr('albniv');
                if (niv == "OBJ") {
                    var isOpened = $("input[id='input_risque_" + $(this).attr('albcodersq') + "']").val();
                    if (isOpened != '0') {// && $("#rsqExpandCollapseVal").val() == 1) {
                        $(this).show();
                        //$("input[id='input_risque_" + $(this).attr('albcodersq') + "']").val("1");
                    }
                }
                else {
                    $(this).show();
                    if ($("#rsqExpandCollapseVal").val() == 1) {
                        $("input[id='input_risque_" + $(this).attr('albcodersq') + "']").val("1");
                        $("img[id='risque_" + $(this).attr('albcodersq') + "']").attr('src', '/Content/Images/Op.png');
                    }
                    else {
                        $("input[id='input_risque_" + $(this).attr('albcodersq') + "']").val("0");
                        $("img[id='risque_" + $(this).attr('albcodersq') + "']").attr('src', '/Content/Images/Cl.png');
                    }
                }
            });

            $(this).attr("checked", "checked");
            $(this).attr("value", true);
        }
        else {
            $("tr[albbaddate='True']").each(function () {
                $(this).hide();
            });
            $(this).removeAttr("checked");
            $(this).attr("value", false);
        }
    });

    $("tr[albbaddate='True']").each(function () {
        $(this).hide();
    });

    $("#AddFormGen").die().live('click', function () {
        Redirection("CreationFormuleGarantie", "Index", 0, 0, 1);
    });
    $("#DelFormGen").die().live('click', function () {
        ShowCommonFancy("Confirm", "DelFormGen", "Etes-vous certain de vouloir supprimer la formule générale ?", 400, 80, true, true);
    });

    if (window.isReadonly || window.isModifHorsAvenant) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
}

//-------------------Verifier si l'Entete contient une adresse-----------------------------
function isEnteteContainAddress() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();

    var res = false;
    $.ajax({
        type: "POST",
        async: false,
        cache: false,
        url: "/DetailsObjetRisque/IsEnteteContainAddress/",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn
        },
        success: function (data) {

            res = data.result;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

    return res;
}

//-----------------Redirection----------------------
function Redirection(cible, job, codeFormule, codeOption, formGen, isNewRsqCopyAdresse) {
                   
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var codeRsq = $("#CodeRsqSel").val();
    var CopieBnsPb = $("#CopieBnsPb").val();
    var paramRedirect = $("#txtParamRedirect").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    if (isNewRsqCopyAdresse !== undefined) {
        if (addParamValue === undefined || addParamValue === null || addParamValue === "") {
            addParamValue = "COPY_ADR_FROM_HEADER|1";
        }
        else {
            addParamValue += "||COPY_ADR_FROM_HEADER|1";
        }
    }

    $.ajax({
        type: "POST",
        url: "/MatriceRisque/Redirection/",
        data: {cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid,
            codeFormule: codeFormule, codeOption: codeOption, codeRisque: codeRsq, CopieBnsPb: CopieBnsPb, formGen: formGen, paramRedirect: paramRedirect, modeNavig: $("#ModeNavig").val(),
            addParamType: addParamType, addParamValue: addParamValue},
        success: function (data) {
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------------Déclenche le scroll du tableau-------------
function ScrollTable(elem) {
    $("#divRisque").scrollTop(elem.scrollTop());
    $("#divHeadInfo").scrollLeft(elem.scrollLeft());
}
//---------------Dépile/Rempile les objets du risque sélectionné-----------
function ToggleRisque(elem) {
    var isOpened = $("input[id='input_" + elem.attr('id') + "']").val();
    var isFullOpened = $("#rsqExpandCollapseVal").val();
    if ((isOpened == "0" && isFullOpened == "1") || (isOpened == "0" && isFullOpened == "0")) {
        if ($("#displayAllCB").attr("checked") != "checked") {
            elem.attr('src', '/Content/Images/Op.png');
            $("tr[name=" + elem.attr('id') + "][albbaddate='False']").show();
            $("input[id='input_" + elem.attr('id') + "']").val("1");
        }
        else {
            elem.attr('src', '/Content/Images/Op.png');
            $("tr[name=" + elem.attr('id') + "]").show();
            $("input[id='input_" + elem.attr('id') + "']").val("1");
        }
    }
    else {
        elem.attr('src', '/Content/Images/Cl.png');
        $("tr[name=" + elem.attr('id') + "]").hide();
        $("input[id='input_" + elem.attr('id') + "']").val("0");
    }
}
//-------Formate les input/span des valeurs----------
function FormatDecimalNumericValue() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    //FormatPourcentNumerique();
    common.autonumeric.applyAll('init', 'pourcentnumeric', '', null, null, '100', '0');
    //FormatNumerique('numeric', ' ', '9999999999999', '0');
    common.autonumeric.applyAll('init', 'numeric', ' ', null, null, '9999999999999', '0');
    common.autonumeric.applyAll('init', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
    //FormatPourcentDecimal();
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '0.00');
    //FormatDecimal('decimal', ' ', 2, '99999999999.99', '0.00');
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

function ConfirmDialog(message) {
    $('<div></div>').appendTo('body')
        .html('<div><h6>' + message + '?</h6></div>')
        .dialog({
            modal: true, title: "Copie d'adresse", zIndex: 10000, autoOpen: true,
            width: 'auto', resizable: false, async: false, cache: false,
            buttons: {
                "Oui": function () {
                    Redirection("DetailsObjetRisque", "Index", undefined, undefined, undefined, true);
                    $(this).dialog("close");
                },
                "Non": function () {
                    Redirection("DetailsObjetRisque", "Index");
                    $(this).dialog("close");
                }
            },
            close: function (event, ui) {
                $(this).remove();
            }
        });

}
