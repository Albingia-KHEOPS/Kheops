$(function () {
    paging.buttons.register();
    preneurs.impayes.register();
    preneurs.retardsPaiement.register();
    preneurs.sinistres.register();
    $(document).on(customEvents.preneurs.alertes.hiding, function () {
        common.knockout.components.disposeFromDialog(preneurs.alertes.componentName);
    });
    
    $(document).on("click", "#linkAlertesPreneur", function () {
        if (!$(this).parent().is(":visible")) return;

        let $div = common.knockout.components.includeInDialog(
            "Alertes Preneur " + ($(this).data("nompreneur") || $("#PreneurAssuranceNom").val()),
            { width: 1050, height: 550 },
            preneurs.alertes,
            "idPreneur: " + $(this).data("preneur") + ", alerts: " + $(this).data("alertes") + ", pageSize: " + defaultPageSize);

        $div.dialog("minHeight", 550);
    });
});
