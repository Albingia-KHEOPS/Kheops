(function () {
    $(document).ready(function () {
        MapElementPage();
    });


    function MapElementPage() {
        MapPeriodGarElement();
    }

    function MapPeriodGarElement() {
        $("#dateDebutNewPeriode").datepicker({ dateFormat: 'dd/mm/yy' });
        $("#dateFinNewPeriode").datepicker({ dateFormat: 'dd/mm/yy' });

        $("#dateDebutNewPeriode").die().live('change', function () {
            if ($(this).val() != "")
                FormatAutoDate($(this));
        });

        $("#dateFinNewPeriode").die().live('change', function () {
            if ($(this).val() != "")
                FormatAutoDate($(this));
        });

        $("#btnReguleBack").attr('disabled', 'disabled');
        AlternanceLigne("PeriodRegulBody", "", false, null);
        AlternanceLigne("MouvementBody", "", false, null);

        /* A verifer */
        $("#btnAppliquer").kclick(function () {
            var position = $(this).offset();
            //$("#divApplique").css({ 'position': 'absolute', 'top': position.top + 25 + 'px', 'left': position.left - 410 + 'px' }).toggle();
            $("#divApplique").css({ 'position': 'absolute', 'top': (position.top - 16) + 'px', 'left': position.left - 110 + 'px' }).toggle();
        });

        $("#btnFermerpopup").kclick(function () {
            $("#divApplique").hide();
        });

        $("#dvAddPeriodRegule").hide();
        $("#btnAjouterPeriodregul").kclick(function () {
            $("#dvAddPeriodRegule").show();
        });
        MapListBas();

        $("#btnCancelAddPeriodRegule").kclick(function () {
            InitialiserDates();
            $("#dvAddPeriodRegule").hide();
        });

        $("#btnValidAddPeriodRegule").kclick(function () {
            EnregistrerLigneMouvementPeriode();
        });

        $("#btnRegulePrec").kclick(function () {
            RedirectionRegul('Step3_ChoixGarantie', 'CreationRegularisation', false, 'Previous');
        });

        $("#btnReguleSuivant").kclick(function (evt, data) {
            if (!$(this).is(":disabled")) {
                RedirectionReguleOuter(data && data.returnHome ? "RechercheSaisie" : "Quittance", "Index");
            }
        });

        $("#btnRegulePrecedent").kclick(function () {
            RetourEcran();
        });

        /* REDIRECTION SAISIE PERIOD REGUL  */
        $("td[name='LinkRegulGar']").kclick(function () {
            OpenPeriodRegul($(this).data("id"), $(this).data("id-rcfr"));
        });

        if (window.isReadonly) {
            //$("#btnAjouterPeriodregul").parent().addClass("None");
            $("#btnAjouterPeriodregul").addClass("None");
            $("img[name='SuppPeriodRegul']").parent().addClass("None");
            $("img[name='SuppPeriodRegul']").addClass("None");
        }
        else {
            $("#btnAjouterPeriodregul").removeClass("None");
            $("#SuppPeriodRegul").parent().removeClass("None");
            $("#SuppPeriodRegul").removeClass("None");
        }
        //AffectDateFormat();
    }


function OpenPeriodRegul(idregulgar, idrcfr) {
    
    $('#AddParamValue').val(common.addOrReplaceParam($('#AddParamValue').val(), 'REGULGARID', idregulgar) );
    if (idrcfr) {
        RedirectionRegul("CalculGarantiesRC", "Regularisation", false);
    }
    else {
        RedirectionRegul("Step5_RegulContrat", "CreationRegularisation", false);
    }
}

    function EnregistrerLigneMouvementPeriode() {
        if (!CheckDate()) {
            return;
        }
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var dateDeb = $("#dateDebutNewPeriode").val();
        var dateDebSp = dateDeb.split('/');
        var dateFin = $("#dateFinNewPeriode").val();
        var dateFinSp = dateFin.split('/');
        var dateDebMin = $("#DateDebMin").val();
        var dateFinMax = $("#DateFinMax").val();
        var codersq = $("#CodeRsq").val();
        var codefor = $("#codefor").val();
        var codegar = $("#codegar").val();
        var idlot = $("#idlot").val();
        var idregul = $("#idregul").val();

        dateDeb = dateDebSp[2] + dateDebSp[1] + dateDebSp[0];
        dateFin = dateFinSp[2] + dateFinSp[1] + dateFinSp[0];

        ShowLoading();

        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/SaveLineMouvtPeriode",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), dateDeb: dateDeb, dateFin: dateFin, dateDebMin: dateDebMin, dateFinMax: dateFinMax,
                codersq: codersq, codefor: codefor, codegar: codegar, idregul: idregul, idlot: idlot
            },
            success: function (data) {
                $("#ListePeriodeRegularise").html(data);
                MapListBas();
                InitialiserDates();
                $("#dvAddPeriodRegule").hide();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

    function MapListBas() {

        common.autonumeric.applyAll('init', 'decimal', null, null, null, "99999999999.99", "-99999999999.99");
        common.autonumeric.applyAll('init', 'pourcentdecimal', null, null, 4, "999.9999", "-999.9999");

        AlternanceLigne("PeriodRegulBody", "", false, null);
        bindSupprimer();

        function bindSupprimer() {
            $("img[id='SuppPeriodRegul']").die().click(function () {
                $("#hiddenInputId").val($(this).attr("albregulecode"));
                ShowCommonFancy("ConfirmTrans", "Suppr",
                    "Vous allez supprimer une période de régularisation. Voulez-vous continuer ?",
                    350, 130, true, true);
            });
            $("#btnConfirmTransOk").kclick(function () {
                switch ($("#hiddenAction").val()) {
                    case "Suppr":
                        var code = $("#hiddenInputId").val();
                        SupprimerPeriodRegul(code);
                        CloseCommonFancy();
                        break;
                }
                $("#hiddenInputId").val('');
            });
            $("#btnConfirmTransCancel").kclick(function () {
                CloseCommonFancy();
            });
        }
    }

    function CheckDate() {
        toReturn = true;

        if ($("#dateDebutNewPeriode").val() == "") {
            $("#dateDebutNewPeriode").addClass('requiredField');
            toReturn = false;
        }
        if ($("#dateFinNewPeriode").val() == "") {
            $("#dateFinNewPeriode").addClass('requiredField');
            toReturn = false;
        }

        if (!checkDate($("#dateDebutNewPeriode"), $("#dateFinNewPeriode"))) {
            toReturn = false;
        }

        return toReturn;
    }
    function InitialiserDates() {
        $("#dateDebutNewPeriode").val("");
        $("#dateFinNewPeriode").val("");

    }

    function SupprimerPeriodRegul(code) {
        ShowLoading();

        var context = {
            IdContrat: {
                CodeOffre: $("#Offre_CodeOffre").val(),
                Version: $("#Offre_Version").val(),
                Type: $("#Offre_Type").val()
            },
            RgId: $("#idregul").val(),
            RsqId: $("#CodeRsq").val(),
            CodeFormule: $("#codefor").val(),
            GrId: $("#idgar").val(),
            RgGrId: code,
            LotId: $("#idlot").val()
        };

        common.$postJson("/CreationRegularisation/SupprimerMouvtPeriode", context).done(function (data) {
            $("#ListePeriodeRegularise").html(data);
            MapListBas();
            CloseLoading();
        });

    }

})();