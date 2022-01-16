affaire.relances.show = function (forceDisplay) {
    if (!forceDisplay) {
        $("#btnShowRelances").kclick(function () {
            affaire.relances.show(true);
        });
    }
    if (modelRelances.DoNotShowAgainForToday === false || forceDisplay === true) {
        $(document).off(customEvents.relances.loaded);
        $(document).on(customEvents.relances.loaded, function () {
            ko.tasks.schedule(function () {
                var hiddenZones = $(".ko-cloak");
                hiddenZones.removeClass("hide");
                hiddenZones.removeClass("conceal");
                common.page.isLoading = false;
            });
        });

        $(document).off(customEvents.relances.hiding);
        $(document).on(customEvents.relances.hiding, function () {
            common.knockout.components.disposeFromDialog(affaire.relances.liste.componentName);
        });


    }

    if (forceDisplay) {
        common.page.isLoading = true;
        common.$getJson("/User/GetProfile", null, true).done(function (p) {
            window.modelRelances.DoNotShowAgainForToday = !p.ShowImpayesOnStartup;
            common.$getJson("/Offre/GetRelances", null, true).done(function (pglist) {
                window.modelRelances.Relances = [];
                window.requestAnimationFrame(function () {
                    window.modelRelances.NombreRelances = pglist.NbTotalLines;
                    window.modelRelances.Relances = pglist.List;
                    showDialog();
                });
            });
        });
    }
    else if (window.modelRelances.Relances.isNotEmptyArray()) {
        showDialog();
    }
    function showDialog() {
        paging.buttons.register();
        common.knockout.components.includeInDialog(
            "Liste des offres à relancer",
            { width: 1000, height: "auto" },
            affaire.relances.liste,
            "", false, true);
    };
};

$(function () {
    //affaire.relances.show();
});