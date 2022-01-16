
(function () {
    var options = {
        componentName: "options-formule",
        init: function () {
            if (!ko.components.isRegistered(this.componentName)) {
                common.knockout.components.register(this.componentName, affaire.formule.OptionsFormuleViewModel, "options-formule.template.html", true);
            }
        }
    };

    affaire.formule.options = options;
    common.autobindFn(affaire.formule.options);
})();

