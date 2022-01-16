
(function () {
    var infoFormuleApplications = {
        isReady: false,
        componentName: "info-formule-applications",
        init: function (node) {
            if (!ko || this.isReady) {
                return false;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: affaire.formule.ApplicationsViewModel
            });

            common.knockout.components.bind(this.componentName);
            this.isReady = true;
            return true;
        }
    };

    affaire.formule.infoApplications = infoFormuleApplications;
    common.autobindFn(affaire.formule.infoApplications);
})();
