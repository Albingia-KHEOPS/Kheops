var affaire = (affaire || {});
(function () {
    let connexites = {
        isReady: false,
        componentName: "connexites",
        init: function () {
            if (!ko || this.isReady) return false;
            ko.components.register(this.componentName, {
                template: {
                    version: (window.scriptVersion || 1),
                    fullQualifiedFilenames: true,
                    extraFiles: ["connexites-engagements.template.html"]
                },
                viewModel: {
                    loadEmptyObject: "Connexites",
                    vmCtor: affaire.connexites.ConnexitesViewModel,
                    ajaxUrl: "/Connexites/GetEmptyModel"
                }
            });

            common.knockout.components.bind(this.componentName);
            this.isReady = true;
            return true;
        }
    };

    affaire.connexites = connexites;
    common.autobindFn(affaire.connexites);
})();
