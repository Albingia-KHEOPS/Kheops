var affaire = (affaire || {});
affaire.relances = affaire.relances || {};
(function () {
    let listeRelances = {
        isReady: false,
        componentName: "liste-relances",
        init: function () {
            if (!ko || this.isReady) {
                return false;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true, hasItsOwnCss: true },
                viewModel: affaire.relances.ListViewModel
            });

            common.knockout.components.bind(this.componentName);
            this.isReady = true;
            return true;
        }
    };

    affaire.relances.liste = listeRelances;
    common.autobindFn(affaire.relances.liste);
})();