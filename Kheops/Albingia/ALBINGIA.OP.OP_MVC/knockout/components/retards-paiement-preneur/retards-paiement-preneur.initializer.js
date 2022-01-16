var preneurs = (preneurs || {});

(function () {
    let listeRetardsPaiement  = {
        isReady: false,
        idRegistered: false,
        componentName: "retards-paiement-preneur",
        register: function () {
            if (this.idRegistered) {
                return;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: preneurs.retardsPaiement.ListViewModel
            });
            this.idRegistered = true;
        },
        init: function () {
            if (!ko || this.isReady) {
                return false;
            }
            this.register();

            common.knockout.components.bind(this.componentName);
            this.isReady = true;
            return true;
        }
    };

    preneurs.retardsPaiement = listeRetardsPaiement ;
    common.autobindFn(preneurs.retardsPaiement);
})();