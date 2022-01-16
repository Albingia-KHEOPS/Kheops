
(function () {
    var detailsGarantie = {
        isReady: false,
        componentName: "details-garantie",
        init: function (node) {
            if (!ko || this.isReady) {
                return false;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: {
                    loadEmptyObject: "DetailsGarantie",
                    vmCtor: affaire.formule.DetailsGarantieViewModel,
                    ajaxUrl: "/FormuleGarantie/GetEmptyDetails"
                }
            });

            common.knockout.components.bind(this.componentName);
            this.isReady = true;
            return true;
        }
    };

    affaire.formule.detailsGarantie = detailsGarantie;
    common.autobindFn(affaire.formule.detailsGarantie);
})();
