
(function () {
    let buttonsViewModel = function (parameters) {
        let dataset = {
            TotalLinesNumber: parameters.total || 0,
            PageSize: parameters.pageSize,
            ListName: parameters.listName
        };
        
        ko.mapping.fromJS(dataset, {}, this);
        let vm = this;
        vm.nbPages = ko.computed(function () {
            var nb = vm.TotalLinesNumber() / vm.PageSize();
            if (nb < 1) {
                nb = 1;
            }
            else if (vm.TotalLinesNumber() % vm.PageSize() > 0) {
                nb = Math.floor(nb) + 1;
            }
            return nb;
        });
        vm.isSinglePage = ko.computed(function () {
            return vm.nbPages() < 2;
        });
        vm.currentPage = ko.observable(1);
        vm.requestFirstPage = function () {
            vm.changePage(-1, 1);
        };
        vm.requestLastPage = function () {
            vm.changePage(1, vm.nbPages());
        };
        vm.requestNextPage = function () {
            vm.changePage(1);
        };
        vm.requestPreviousPage = function () {
            vm.changePage(-1);
        };

        vm.allowNext = ko.computed(function () {
            return vm.currentPage() < vm.nbPages();
        });

        vm.allowPrevious = ko.computed(function () {
            return vm.currentPage() > 1;
        });

        vm.firstLine = ko.computed(function () {
            if (vm.nbPages() < 2) {
                return 1;
            }
            var line = (vm.currentPage() - 1) * vm.PageSize();
            return line || 1;
        });

        vm.lastLine = ko.computed(function () {
            if (vm.nbPages() < 2) {
                return vm.TotalLinesNumber();
            }
            var line = (vm.currentPage() * vm.PageSize()) - 1;
            return line > vm.TotalLinesNumber() ? vm.TotalLinesNumber() : line;
        });

        vm.changePage = function (direction, forcedPage) {
            let page = vm.currentPage();
            if (page == vm.nbPages() && direction > 0 || page == 1 && direction < 0) {
                return;
            }

            common.page.isLoading = true;
            $(document).trigger(customEvents.paging.change, [(forcedPage || (page + direction)), vm.ListName()]);
        };

        vm.onPageChanged = function (event, listName, pglist) {
            if (listName !== vm.ListName()) {
                return;
            }
            vm.TotalLinesNumber(pglist.NbTotalLines);
            //ko.mapping.fromJS(pglist.List, {}, vm.List);
            //let newPage = (forcedPage || (page + direction));
            //if (pglist.PageNumber != newPage) {
            //    newPage = pglist.PageNumber;
            //}
            vm.currentPage(pglist.PageNumber);
            common.page.isLoading = false;
        };

        $(document).on(customEvents.paging.dataLoaded, vm.onPageChanged);
        $(document).on(customEvents.paging.initFirstPage, function () {
            vm.changePage(1, 1);
        });

        if (parameters.total == null) {
            parameters.total = 0;
            common.page.isLoading = true;
            $(document).trigger(customEvents.paging.change, [1, vm.ListName()]);
        }
    };
    buttonsViewModel.prototype.dispose = function () {
        $(document).off(customEvents.paging.dataLoaded);
        $(document).off(customEvents.paging.initFirstPage);
    };
    paging.buttons.ViewModel = buttonsViewModel;
})();
