
(function () {
    let retardsPaiementViewModel = function (parameters) {
        ko.mapping.fromJS({
            RetardsPaiement: [],
            ListName: "RetardsPaiement",
            CodePreneur: parameters.codePreneur,
            PageSize: parameters.pageSize || 50,
            MontantTotal: 0.0
        }, {}, this);

        let vm = this;
        vm.initialized = ko.observable(false);
        vm.hasResults = ko.computed(function () {
            return (vm.RetardsPaiement() || []).length > 0;
        });

        $(document).on(customEvents.paging.change, function (e, page, listName) {
            if (listName !== vm.ListName()) {
                return;
            }
            common.$getJson("/Quittance/GetRetardsPaiement", { page: page, codeAssure: vm.CodePreneur() }, true).done(function (pglist) {
                ko.mapping.fromJS(pglist.List, {}, vm.RetardsPaiement);
                vm.MontantTotal(pglist.Totals.MontantTotalHT);
                $(document).trigger(customEvents.paging.dataLoaded, [listName, pglist]);
                if (!vm.initialized()) {
                    $(document).trigger(customEvents.preneurs.lists.retardsPaiement.loaded);
                    vm.initialized(true);
                }
            });
        });

        $(document).trigger(customEvents.paging.initFirstPage);
    };
    retardsPaiementViewModel.prototype.dispose = function () {
        $(document).off(customEvents.paging.change);
    };
    preneurs.retardsPaiement.ListViewModel = retardsPaiementViewModel;
})();
