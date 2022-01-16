var preneurs = (preneurs || {});

(function () {
    let listeSinistres = {
        isReady: false,
        idRegistered: false,
        componentName: "sinistres-preneur",
        register: function () {
            if (this.idRegistered) {
                return;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: preneurs.sinistres.ListViewModel
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

    preneurs.sinistres = listeSinistres;
    common.autobindFn(preneurs.sinistres);
})();