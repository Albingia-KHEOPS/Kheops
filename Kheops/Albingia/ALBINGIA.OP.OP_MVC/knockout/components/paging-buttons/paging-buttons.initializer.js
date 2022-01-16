var paging = paging || {};
(function () {
    let pagingButtons = {
        isReady: false,
        idRegistered: false,
        componentName: "paging-buttons",
        register: function () {
            if (this.idRegistered) {
                return;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: paging.buttons.ViewModel
            });
            this.idRegistered = true;
        },
        init: function () {
            if (!ko || this.isReady) {
                return false;
            }
            this.register();

            common.knockout.components.bind(this.componentName);
            this.isReady = true;
            return true;
        }
    };

    paging.buttons = pagingButtons;
    common.autobindFn(paging.buttons);
})();
