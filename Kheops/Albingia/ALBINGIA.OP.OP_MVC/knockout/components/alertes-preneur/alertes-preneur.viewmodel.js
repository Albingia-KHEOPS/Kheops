
(function () {
    let alertesViewModel = function (parameters) {
        ko.mapping.fromJS(obj = {
            Alertes: parameters.alerts,
            IdPreneur: parameters.idPreneur,
            PageSize: parameters.pageSize || 20
        }, {}, this);
        let vm = this;

        vm.hasNoAlerts = ko.computed(function () {
            return vm.Alertes().length == 0;
        });
        vm.showImpayes = ko.computed(function () {
            return !vm.hasNoAlerts() && vm.Alertes().some(function (x) { return x == "I"; });
        });
        vm.showSinistres = ko.computed(function () {
            return !vm.hasNoAlerts() && vm.Alertes().some(function (x) { return x == "S"; });
        });
        vm.showRetardsPaiement = ko.computed(function () {
            return !vm.hasNoAlerts() && vm.Alertes().some(function (x) { return x == "RP"; });
        });
        vm.hasOnlyImpayes = ko.computed(function () {
            return !vm.hasNoAlerts() && !vm.Alertes().some(function (x) { return x == "S" || x == "RP"; });
        });
        vm.hasOnlySinistres = ko.computed(function () {
            return !vm.hasNoAlerts() && !vm.Alertes().some(function (x) { return x == "I" || x == "RP"; });
        });
        vm.hasOnlyRetardsPaiement = ko.computed(function () {
            return !vm.hasNoAlerts() && !vm.Alertes().some(function (x) { return x == "I" || x == "S"; });
        });
        vm.contextChanged = function () {

        };

        vm.hide = function () {
            $(document).trigger(customEvents.preneurs.alertes.hiding);
        };

        $(document).on([customEvents.preneurs.lists.impayes.loaded, customEvents.preneurs.lists.sinistres.loaded, customEvents.preneurs.lists.retardsPaiement.loaded].join(" "), function () {
            ko.tasks.schedule(function () {
                var hiddenZones = $(".ko-cloak");
                hiddenZones.removeClass("hide");
                hiddenZones.removeClass("conceal");
                common.page.isLoading = false;
            });
        });

        ko.tasks.schedule(function () {
            $(preneurs.alertes.componentName + " .tabbed-view [href]").first().click();
        });
    };
    alertesViewModel.prototype.dispose = function () {
        $(document).off([customEvents.preneurs.lists.impayes.loaded, customEvents.preneurs.lists.sinistres.loaded, customEvents.preneurs.lists.retardsPaiement.loaded].join(" "));
    };
    preneurs.alertes.ListViewModel = alertesViewModel;
})();
