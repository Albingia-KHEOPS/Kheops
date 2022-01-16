$(document).ready(function () {
    MapElementPage();
});


function MapElementPage() {
    MapListRsqElement();
}


function MapListRsqElement() {
    $("#btnReguleCancel").hide();
   

    $("#btnRegulePrec").kclick(function () {
        RedirectionRegul('Step1_ChoixPeriode', 'CreationRegularisation', false, 'Previous');
    });

    $("#btnReguleSuivant").kclick(function (evt, data) {
        if (!$(this).is(":disabled")) {
            RedirectionReguleOuter(data && data.returnHome ? "RechercheSaisie" : "Quittance", "Index");
        }
    });

    $("img[name='ExpandRsq']").each(function () {
        $(this).click(function () {
            var codeRsq = $(this).attr("id").split("_")[1];
            var imgExpCol = $(this).attr("albexpcol");
            switch (imgExpCol) {
                case "expand":
                    $(this).attr("src", "/Content/Images/Op.png").attr("albexpcol", "collapse");
                    $("tr[id^='trObj_" + codeRsq + "']").show();
                    break;
                case "collapse":
                    $(this).attr("src", "/Content/Images/Cl.png").attr("albexpcol", "expand");
                    $("tr[id^='trObj_" + codeRsq + "']").hide();
                    break;
            }
        });
    });

    $("td[id='tdRsqRegule']").each(function () {
        $(this).die();
        $(this).click(function () {
            var codeRsq = $(this).attr('albcodersq');
            OpenSelectionGarRegule(codeRsq);
        });
    });

    //Si aucun risque n'est régularisé, blocage du passage à l'écran Cotisations
    if ($("td[id='tdRsqRegule'][albreguleused='O']").length <= 0) {
        $("#btnReguleSuivant").attr("disabled", "disabled");
    }

    //mise en commentaire : pb du retour
    //Chargement des garanties s'il n'y a qu'un seul risque
    //if ($("#NbRsqRegule").val() == "1") {
    //    $("td[id='tdRsqRegule']").trigger("click");
    //}
}

function OpenSelectionGarRegule(codeRsq) {
    ShowLoading();

    $('#AddParamValue').val($('#AddParamValue').val() + '||RSQID|' + codeRsq);
    RedirectionRegul("Step3_ChoixGarantie","CreationRegularisation",false);

}




