var affaire = (affaire || {});
(function () {
    let fusionConnexites = {
        isReady: false,
        componentName: "fusion-connexites",
        init: function () {
            if (!ko || this.isReady) return false;
            ko.components.register(this.componentName, {
                template: {
                    version: (window.scriptVersion || 1),
                    fullQualifiedFilenames: true
                },
                viewModel: {
                    loadEmptyObject: "FusionConnexites",
                    vmCtor: affaire.connexites.fusion.ViewModel,
                    ajaxUrl: "/Connexites/GetEmptyFusionConnexites"
                }
            });

            common.knockout.components.bind(this.componentName);
            this.isReady = true;
            return true;
        }
    };

    affaire.connexites = (affaire.connexites || {});
    affaire.connexites.fusion = fusionConnexites;
    common.autobindFn(affaire.connexites.fusion);
})();