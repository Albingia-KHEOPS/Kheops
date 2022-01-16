var pbs = (pbs || {});
(function () {
    var periode = {
        componentName: "selection-periode-pb",
        init: function () {
            if (!ko || ko.components.isRegistered(this.componentName)) {
                return false;
            }
            ko.components.register(this.componentName, {
                template: { version: (window.scriptVersion || 1), fullQualifiedFilenames: true },
                viewModel: pbs.PeriodeViewModel
            });

            common.knockout.components.bind(this.componentName);
            return true;
        }
    };

    pbs.periode = periode;
    common.autobindFn(pbs.periode);
})();
