
(function () {
    let impayesViewModel = function (parameters) {
        ko.mapping.fromJS({
            Impayes: [],
            ListName: "Impayes",
            CodePreneur: parameters.codePreneur,
            PageSize: parameters.pageSize || 50,
            MontantTotal: 0.0
        }, {}, this);

        let vm = this;
        vm.initialized = ko.observable(false);
        vm.hasResults = ko.computed(function () {
            return (vm.Impayes() || []).length > 0;
        });

        $(document).on(customEvents.paging.change, function (e, page, listName) {
            if (listName !== vm.ListName()) {
                return;
            }
            common.$getJson("/Quittance/GetImpayes", { page: page, codeAssure: vm.CodePreneur() }, true).done(function (pglist) {
                ko.mapping.fromJS(pglist.List, {}, vm.Impayes);
                vm.MontantTotal(pglist.Totals.MontantTotalHT);
                $(document).trigger(customEvents.paging.dataLoaded, [listName, pglist]);
                if (!vm.initialized()) {
                    $(document).trigger(customEvents.preneurs.lists.impayes.loaded);
                    vm.initialized(true);
                }
            });
        });

        $(document).trigger(customEvents.paging.initFirstPage);
    };
    impayesViewModel.prototype.dispose = function () {
        $(document).off(customEvents.paging.change);
    };
    preneurs.impayes.ListViewModel = impayesViewModel;
})();
