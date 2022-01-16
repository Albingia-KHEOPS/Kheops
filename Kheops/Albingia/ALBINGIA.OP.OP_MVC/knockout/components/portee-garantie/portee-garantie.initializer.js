
(function () {
    var portees = {
        isReady: false,
        componentName: "portee-garantie",
        init: function () {
            if (!ko || this.isReady) return false;
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: {
                    loadEmptyObject: "PorteeGarantie",
                    vmCtor: affaire.formule.PorteeGarantieViewModel,
                    ajaxUrl: "/FormuleGarantie/GetEmptyPortee"
                }
            });

            common.knockout.components.bind(this.componentName);
            this.isReady = true;
            return true;
        }
    };

    affaire.formule.portees = portees;
    common.autobindFn(affaire.formule.portees);
})();
