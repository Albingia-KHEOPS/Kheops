
(function () {
    var inventaires = {
        isReady: false,
        componentName: "inventaires-garantie",
        init: function (node) {
            if (!ko || this.isReady) return false;
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: {
                    loadEmptyObject: "Inventaire",
                    vmCtor: risque.inventaire.ObjetInventaire,
                    ajaxUrl: "/RisqueInventaire/GetEmptyInventaire"
                }
            });

            common.knockout.components.bind(this.componentName);
            this.isReady = true;
            return true;
        }
    };

    affaire.formule.inventaires = inventaires;
    common.autobindFn(affaire.formule.inventaires);
})();
