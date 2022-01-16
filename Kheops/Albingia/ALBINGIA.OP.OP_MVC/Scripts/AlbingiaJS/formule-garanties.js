ko.options.deferUpdates = true;

$(function () {
    $(document).on(customEvents.formule.initializing, function () {
    });

    $(document).on(customEvents.formule.loaded, function () {
        ko.tasks.schedule(function () {
            var hiddenZones = $(".ko-cloak");
            hiddenZones.removeClass("hide");
            hiddenZones.removeClass("conceal");
        });
    });

    $(document).on(customEvents.formule.error, function () {
    });
    
    affaire.formule.avenant.init();
    affaire.formule.infoApplications.init();
    affaire.formule.options.init();
});
