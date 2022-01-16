$(document).ready(function () {
    $(document).off(customEvents.relances.loaded);
    $(document).on(customEvents.relances.loaded, function () {
        ko.tasks.schedule(function () {
            var hiddenZones = $(".ko-cloak");
            hiddenZones.removeClass("hide");
            hiddenZones.removeClass("conceal");
            common.page.isLoading = false;
        });
    });

    paging.buttons.init();

    common.$getJson("/Offre/GetRelances", null, true).done(function (pglist) {
        window.modelRelances.Relances = [];
        window.requestAnimationFrame(function () {
            window.modelRelances.NombreRelances = pglist.NbTotalLines;
            window.modelRelances.Relances = pglist.List;

            affaire.relances.liste.init();
        });
    });

})