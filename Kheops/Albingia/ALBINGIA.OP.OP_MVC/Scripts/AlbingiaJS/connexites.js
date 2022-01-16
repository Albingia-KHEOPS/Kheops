
$(function () {
    $(document).on(customEvents.connexites.loaded, function () {
        $(affaire.connexites.componentName + " .ko-cloak.conceal").removeClass("conceal");
    });
    $(document).on(customEvents.connexites.fusion.loaded, function () {
        $(affaire.connexites.fusion.componentName + " .ko-cloak.conceal").removeClass("conceal");
    });
    $(document).on(customEvents.recherche.loaded, function () {
        $(affaire.recherche.componentName + " .ko-cloak.conceal").removeClass("conceal");
    });

    $(document).on([customEvents.connexites.error, customEvents.connexites.endEdit].join(" "), function () {
        common.knockout.components.disposeFromDialog(affaire.connexites.componentName);
    });
    $(document).on([customEvents.connexites.fusion.cancel, customEvents.connexites.fusion.terminated].join(" "), function () {
        common.knockout.components.disposeFromDialog(affaire.connexites.fusion.componentName);
    });
});
