var affaire = (affaire || {});
(function () {
    let entete = {
        componentName: "entete-contrat",
        init: function () {
            if (ko.components.isRegistered(this.componentName)) return false;
            ko.components.register(this.componentName, {
                template: {
                    version: (window.scriptVersion || 1),
                    fullQualifiedFilenames: true
                },
                viewModel: {
                    loadEmptyObject: "EnteteContrat",
                    vmCtor: affaire.contrat.entete.ViewModel,
                    ajaxUrl: "/Folder/GetEmptyEnteteContrat"
                }
            });

            common.knockout.components.bind(this.componentName);
            return true;
        }
    };

    affaire.contrat = (affaire.contrat || {});
    affaire.contrat.entete = entete;
    common.autobindFn(affaire.contrat.entete);
})();
