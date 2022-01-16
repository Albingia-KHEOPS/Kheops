var pbs = (pbs || {});
(function () {
    let listePbs = {
        componentName: "liste-pb",
        init: function () {
            if (!ko || ko.components.isRegistered(this.componentName)) {
                return false;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: pbs.ListViewModel
            });

            common.knockout.components.bind(this.componentName);
            return true;
        }
    };

    pbs.liste = listePbs;
    common.autobindFn(pbs.liste);
})();