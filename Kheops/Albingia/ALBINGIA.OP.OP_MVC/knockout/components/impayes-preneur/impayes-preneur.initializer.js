var preneurs = (preneurs || {});

(function () {
    let listeImpayes = {
        isReady: false,
        idRegistered: false,
        componentName: "impayes-preneur",
        register: function () {
            if (this.idRegistered) {
                return;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: preneurs.impayes.ListViewModel
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

    preneurs.impayes = listeImpayes;
    common.autobindFn(preneurs.impayes);
})();