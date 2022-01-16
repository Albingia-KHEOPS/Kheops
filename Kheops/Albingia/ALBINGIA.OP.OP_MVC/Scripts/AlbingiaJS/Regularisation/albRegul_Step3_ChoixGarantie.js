$(document).ready(function () {
    MapElementPage();
});


function MapElementPage() {
    MapListGarElement();
}


function MapListGarElement() {
    $("#btnReguleSuivant").kclick(function (evt, data) {
        if (!$(this).is(":disabled")) {
            RedirectionReguleOuter(data && data.returnHome ? "RechercheSaisie" : "Quittance", "Index");
        }
    });

    $("#btnRegulePrec").kclick(function () {
        var addParam = $('#AddParamValue').val();
        $('#AddParamValue').val(addParam.substring(0, addParam.lastIndexOf('||RSQID')));
        RedirectionRegul('Step2_ChoixRisque', 'CreationRegularisation', false, 'Previous');
    });

    document.body.addEventListener('click', function () {
        // Masque tous les popup 
        $("div[id^='divApplique']").each(function () {
            $(this).hide();
        });

    }, true);    

    $("td[name='tdFormuleRegule']").each(function () {
        var divName = $(this).parent().get(0).querySelector("td[id='tdGarantieRegule']").getAttribute('albcodgar');
        $('body').on("mouseout", "#divApplique" + divName,function () { $(this).hide(); });

        $(this).mouseover(function () {          
            if ($('#dvBodyApplique' + divName).val() == undefined) {
                var div = document.createElement('div');
                div.setAttribute("id", "dvBodyApplique" + divName);
                document.getElementsByTagName('body')[0].appendChild(div);
                OpenAppliqueFormule($(this));
            }
            else {
                var pos = $(this).position();
                $('#dvBodyApplique' + divName).css({ "position": "absolute", "z-index": "6", "top": pos.top + 20 + "px", "left": pos.left + 5 + "px" });
                $('#divApplique' + divName).show();
            }
        });
        $(this).mouseout(function (event) {
            if (document.getElementById("divApplique" + divName) !== event.relatedTarget) {
                $("#divApplique" + divName).hide();
            }
        });
    });

    $("td[name='tdImgGar']").kclick(function () {
         OpenDetailGarantie($(this));
    });

    $("td[name='tdGarantieRegule']").kclick(function () {
        OpenSelectionPeriodGar($(this));
    });
}

function OpenAppliqueFormule(elem) {
    var idFormule = elem.attr("albcodefor");
    //ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationRegularisation/OpenApplique",
        data: {
            codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#CodeAvtGar").val(),
            codeFor: idFormule
        },
        success: function (data) {
            // Masquer tous les popup récalcitrant
            $("div[id^='divApplique']").each(function () {
                $(this).hide();
            });

            var divName = elem.parent().get(0).querySelector("td[id='tdGarantieRegule']").getAttribute('albcodgar');
            $('#dvBodyApplique' + divName).html(data);
            var div = $('#dvBodyApplique' + divName).get(0).querySelector("div[id='divApplique']");
            div.id = 'divApplique' + divName; 
            $("#divApplique" + divName).show();
            var pos = elem.position();
            $('#dvBodyApplique' + divName).css({ "position": "absolute", "z-index": "6", "top": pos.top + 20 + "px", "left": pos.left + 5 + "px" });
            $("#divApplique" + divName).show();
            //CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function OpenDetailGarantie(elem) {
    var codeObjetRisque = $("#ObjetRisqueCode").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CreationFormuleGarantie/LoadDetailsGarantie",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            codeFormule: elem.attr("albcodefor"), codeOption: elem.attr("albcodeopt"), codeGarantie: elem.attr("albcodegar"), codeObjetRisque: codeObjetRisque, modeNavig: $("#ModeNavig").val(),
            codeAvn: $("#CodeAvtGar").val(), isReadonly: true
        },
        success: function (data) {
            $("#dvBodyDetail").html(data);
            $("#dvDetailGarantie").show();
            MapElementDetailGarantie();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


function OpenSelectionPeriodGar(elem) {
    var idGar = "";

    if (elem != undefined && elem != null) {
        idGar = elem.attr("albidgar");
    }

    if (!idGar || idGar == 0) {
        return;
    }

    var bigId = $('#AddParamValue').val().replace(/\|\|GARID\|[0-9]+\b/g, '');

    $('#AddParamValue').val(bigId + '||GARID|' + idGar);

    RedirectionRegul("Step4_ChoixPeriodeGarantie", "CreationRegularisation", false);
}



function MapElementDetailGarantie() {
    $("#infoDetail").kclick(function () {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationFormuleGarantie/OpenInfoDetail",
            data: { codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeGarantie: $("#CodeGarantie").val() },
            success: function (data) {
                $("#divInfoDetail").show();
                $("#divDataInfoDetail").html(data);
                AlternanceLigne("InfoDetail1", "", false, null);
                AlternanceLigne("InfoDetail2", "", false, null);
                common.autonumeric.applyAll('init', 'decimal', '', null, null, '99999999999.99', null);
                common.autonumeric.applyAll('init', 'pourcentdecimal', '');
                common.autonumeric.applyAll('init', 'pourmilledecimal', '');
                $("#btnCloseInfoDetail").bind('click', function () {
                    $("#divInfoDetail").hide();
                });
                CloseLoading();
            },
            error: function (data) {
                common.error.showXhr(request);
            }
        });
    });

    $("#btnFancyAnnuler").kclick(function () {
        $("#dvBodyDetail").html("");
        $("#dvDetailGarantie").hide();
    });
}





