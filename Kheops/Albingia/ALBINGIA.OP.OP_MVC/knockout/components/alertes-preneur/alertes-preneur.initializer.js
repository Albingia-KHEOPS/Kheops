var preneurs = (preneurs || {});
(function () {
    let alertList = {
        componentName: "alertes-preneur",
        init: function () {
            if (ko.components.isRegistered(this.componentName)) {
                return false;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: preneurs.alertes.ListViewModel
            });

            common.knockout.components.bind(this.componentName);
            return true;
        }
    };

    preneurs.alertes = alertList;
    common.autobindFn(preneurs.alertes);
})();
