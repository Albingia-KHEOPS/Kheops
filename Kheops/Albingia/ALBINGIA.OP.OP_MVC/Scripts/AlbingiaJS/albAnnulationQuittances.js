$(document).ready(function () {
    const btnAnnuler = $("#btnAnnuler");
    MapElementPage();
    //----------------Map les éléments de la page------------------
    function MapElementPage() {
        btnAnnuler.live('click', function () {
            ShowCommonFancy("Confirm", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        });
        $("#btnConfirmOk").live('click', function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "Cancel":
                    CancelForm();
                    break;
                case "Enregistrer":
                    Enregistrer();
            }
            $("#hiddenAction").val('');
        });
        $("#btnConfirmCancel").live('click', function () {
            CloseCommonFancy();
            $("#hiddenAction").val('');
        });

        $("#btnSuivant").kclick(function (evt, data) {
            Suivant(data);
        });

        FormatDecimalValue();


        $("#AnnulQuittanceDateEmission, #AnnulQuittanceTypeOperation, #AnnulQuittanceSituation, #AnnulQuittancePeriodeDebut, #AnnulQuittancePeriodeFin").die().live("change", function () {
            FiltrerAnnulationQuittances();
        });

        $("#AnnulQuittanceDateEmission").datepicker({ dateFormat: 'dd/mm/yy' });
        $("#AnnulQuittancePeriodeDebut").datepicker({ dateFormat: 'dd/mm/yy' });
        $("#AnnulQuittancePeriodeFin").datepicker({ dateFormat: 'dd/mm/yy' });

        MapElementVisuQuittancesTableau();

        if (window.isReadonly) {
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }
    }
    function SerivalizeForm() {
        return {
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            tabGuid: $("#tabGuid").val(),
            modeNavig: $("#ModeNavig").val(),
            addParamType: $("#AddParamType").val(),
            addParamValue: $("#AddParamValue").val()
        };
    }
    function FiltrerAnnulationQuittances(colTri) {

        if (colTri == undefined) {
            colTri = "QuittNumDESC";
        }

        const data = {
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            avenant: $("#NumAvenantPage").val(),
            isEntete: $("#isOpenedFromHeader").val() || false,
            dateEffetAvenant: $("#DateEffetAvenant").val(),
            dateEmission: $("#AnnulQuittanceDateEmission").val(),
            typeOperation: $("#AnnulQuittanceTypeOperation").val(),
            situation: $("#AnnulQuittanceSituation").val(),
            datePeriodeDebut: $("#AnnulQuittancePeriodeDebut").val(),
            datePeriodeFin: $("#AnnulQuittancePeriodeFin").val(),
            acteGestion: $("#ActeGestion").val(),
            tabGuid: $("#tabGuid").val(),
            modeNavig: $("#ModeNavig").val(),
            colTri: colTri
        };

        ShowLoading();

        $.ajax({
            type: "POST",
            url: "/AnnulationQuittances/FiltrerAnnulationQuittances",
            data: data,
            success: function (data) {
                $("#divAnnulQuittancesBody").html(data);

                if (colTri != "") {
                    $(".imageTri").attr("src", "/Content/Images/tri.png");
                }

                switch (colTri) {
                    case "QuittNum":
                        $("th[albcontext='QuittNum']").attr("albcontext", "QuittNumDESC");
                        $("img[albcontext='QuittNum']").attr("albcontext", "QuittNumDESC").attr("src", "/Content/Images/tri_asc.png");
                        $("th[albcontext='QuittNumIntDESC']").attr("albcontext", "QuittNumInt");
                        $("img[albcontext='QuittNumIntDESC']").attr("albcontext", "QuittNumInt");
                        break;
                    case "QuittNumDESC":
                        $("th[albcontext='QuittNumDESC']").attr("albcontext", "QuittNum");
                        $("img[albcontext='QuittNumDESC']").attr("albcontext", "QuittNum").attr("src", "/Content/Images/tri_desc.png");
                        $("th[albcontext='QuittNumIntDESC']").attr("albcontext", "QuittNumInt");
                        $("img[albcontext='QuittNumIntDESC']").attr("albcontext", "QuittNumInt");
                        break;
                    case "QuittNumInt":
                        $("th[albcontext='QuittNumDESC']").attr("albcontext", "QuittNum");
                        $("img[albcontext='QuittNumDESC']").attr("albcontext", "QuittNum");
                        $("th[albcontext='QuittNumInt']").attr("albcontext", "QuittNumIntDESC");
                        $("img[albcontext='QuittNumInt']").attr("albcontext", "QuittNumIntDESC").attr("src", "/Content/Images/tri_asc.png");
                        break;
                    case "QuittNumIntDESC":
                        $("th[albcontext='QuittNumDESC']").attr("albcontext", "QuittNum");
                        $("img[albcontext='QuittNumDESC']").attr("albcontext", "QuittNum");
                        $("th[albcontext='QuittNumIntDESC']").attr("albcontext", "QuittNumInt");
                        $("img[albcontext='QuittNumIntDESC']").attr("albcontext", "QuittNumInt").attr("src", "/Content/Images/tri_desc.png");
                        break;
                    default:
                        break;
                }

                MapElementVisuQuittancesTableau();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

    function Suivant(data) {
        //Vérification et confirmation
        var ischeck = false;
        $("input[name=checkAnnuleQuittance]").each(function () {
            if ($(this).is(":checked")) {
                ischeck = true;
            }
        });

        if (ischeck && (!data || !data.returnHome)) {
            ShowCommonFancy("Confirm", "Enregistrer",
                "Les quittances sélectionnées vont être annulées.<br/><br/>Voulez-vous continuer ?<br/>",
                350, 130, true, true);
        }
        else {
            Enregistrer(data && data.returnHome);
        }
    }

    function Enregistrer(returnHome) {
        const data = SerivalizeForm();

        var isCheckedEch = $("#isCheckedEch").val();
        //Récupération des quittances cochées (ie annulées)
        var listeAnnulQuitt = "";
        $("input[name=checkAnnuleQuittance]").each(function () {
            if ($(this).is(":checked")) {
                listeAnnulQuitt += $(this).attr("id").split("_")[1] + "|";
            }
        });
        const extendeddata = {
            listeAnnulQuittances: listeAnnulQuitt,
            isCheckedEch: isCheckedEch,
            paramRedirect: $("#txtParamRedirect").val(),
            txtSaveCancel: returnHome ? 1 : 0
        };
        $.extend(data, extendeddata);

        ShowLoading();

        $.ajax({
            type: "POST",
            url: "/AnnulationQuittances/AnnulationQuittancesEnregistrer/",
            data: data,
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

    function CancelForm() {
        ShowLoading();
        const data = SerivalizeForm();
        $.ajax({
            type: "POST",
            url: "/AnnulationQuittances/RedirectionAnnuler/",
            data: data,
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }


});
