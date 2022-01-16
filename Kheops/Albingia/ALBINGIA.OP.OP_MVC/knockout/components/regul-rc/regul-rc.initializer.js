
(function () {

    if (!regul) {
        return;
    }

    regul.garantiesRC = {
        isReady: false,
        componentName: "regul-rc",
        initialize: function () {
            if (!ko || regul.garantiesRC.isReady === true) return;

            ko.components.register(regul.garantiesRC.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: regul.RegulRCViewModel
            });

            var cmps = document.getElementsByName(regul.garantiesRC.componentName);
            for (var i = 0; i < cmps.length; i++) {
                ko.applyBindings({}, cmps[i]);
            }

            regul.garantiesRC.isReady = true;
        }
    };
})();

$(function () {
    regul.garantiesRC.initialize();
});