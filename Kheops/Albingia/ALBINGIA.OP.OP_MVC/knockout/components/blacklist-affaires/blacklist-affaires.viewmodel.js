
(function () {
    let affairesViewModel = function (parameters) {
        ko.mapping.fromJS({
            Affaires: [],
            Code: parameters.code,
            CodeInterlocuteur: parameters.code2,
            Name: parameters.name,
            Type: parameters.type
        }, {}, this);

        let vm = this;
        vm.initialized = ko.observable(false);
        vm.hasResults = ko.computed(function () {
            return (vm.Affaires() || []).length > 0;
        });

        vm.hideList = function () {
            $(document).trigger(customEvents.blacklist.affaires.hiding);
        };

        let Afr = function (data) {
            ko.mapping.fromJS(data, {}, this);
            let a = this;
            this.imgTypeClass = ko.computed(function () {
                switch (this.Affaire.DisplayType()) {
                    case "P": return "type-affaire contrat";
                    case "O": return "type-affaire offre";
                    case "OO": return "type-affaire dbloffre";
                    case "M": return "type-affaire mere";
                    case "A": return "type-affaire aliment";
                }
                return "";
            }, this);
            this.reachFolder = function () {
                if (!a.Cible()) {
                    ko.tasks.schedule(function () {
                        window.location.href = "Albinprod:GererContrat?action=VISUCONTRAT?type=" + a.Affaire.Type() + "?ipb=" + a.Affaire.Code().trim() + "?Alx=" + a.Affaire.Version();
                    });
                }
                else {
                    $.get("/Folder/GetFolderUrl?" + $.param({ code: a.Affaire.Code().trim(), version: a.Affaire.Version(), type: a.Affaire.Type(), readOnly: true })).done(function (url) {
                        ko.tasks.schedule(function () {
                            common.page.navigateNewTab(url);
                        });
                    });
                }
            };
            this.clickableRow = ko.computed(function () {
                return this.Cible() ? "affair-row" : "affair-row-citrix";
            }, this);
        };

        common.$getJson("/Folder/GetBlacklistAffaires", { code: vm.Code(), codeInterlocuteur: vm.CodeInterlocuteur(), typeBlacklist: vm.Type(), name: vm.Name() }, true).done(function (list) {
            ko.mapping.fromJS(
                list,
                {
                    create: function (options) {
                        return new Afr(options.data);
                    }
                },
                vm.Affaires);

            ko.tasks.schedule(function () {
                $(document).trigger(customEvents.blacklist.affaires.loaded);
            });
        });
    };

    blacklist.affaires.ListViewModel = affairesViewModel;
})();
