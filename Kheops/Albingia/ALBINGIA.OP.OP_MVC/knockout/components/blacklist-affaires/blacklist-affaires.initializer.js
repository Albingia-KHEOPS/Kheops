var blacklist = (blacklist || {});

(function () {
    let listeAffaires = {
        isReady: false,
        idRegistered: false,
        componentName: "blacklist-affaires",
        register: function () {
            if (ko.components.isRegistered(this.componentName)) {
                return;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: blacklist.affaires.ListViewModel
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

    blacklist.affaires = listeAffaires;
    common.autobindFn(blacklist.affaires);
})();