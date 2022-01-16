
(function () {
    var recherche = {
        isReady: false,
        componentName: "recherche-affaires",
        init: function () {
            if (!ko || this.isReady) return false;
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: {
                    loadEmptyObject: "FiltreAffaire",
                    vmCtor: affaire.recherche.RechercheAffairesViewModel,
                    ajaxUrl: "/RechercheAffaires/GetDefaultFiltreAffaire"
                }
            });

            common.knockout.components.bind(this.componentName);
            this.isReady = true;
            return true;
        }
    };

    affaire.recherche = recherche;
    common.autobindFn(affaire.recherche);
})();
