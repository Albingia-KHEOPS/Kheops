(function () {
    let fusionConnexitesViewModel = function (parameters, emptyModel) {
        let vm = this;
        let listMap = {
            "Connexites": {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.Folder.CodeOffre) + "_" + ko.utils.unwrapObservable(data.Folder.Version);
                }
            },
            "ConnexitesExternes": {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.Folder.CodeOffre) + "_" + ko.utils.unwrapObservable(data.Folder.Version);
                }
            }
        };
        ko.mapping.fromJS(emptyModel, listMap, this);
        const pickAction = "pick";
        const mergeAction = "merge";
        const moveSourceAction = "moveSource";

        this.mergeTooltip = ko.computed(function () {
            if (!vm.AffaireCible || !vm.AffaireCible.CodeOffre()) {
                return "";
            }
            return "Fusionner les deux connexités. Cette action provoquera la suppression de la connexités B.";
        });
        this.pickTooltip = ko.computed(function () {
            if (!vm.AffaireCible || !vm.AffaireCible.CodeOffre()) {
                return "";
            }
            return "Déplacer le contrat " + vm.AffaireCible.CodeOffre() + " dans la connexité A."
                + (vm.ConnexitesExternes().length <= 2 ? " Cette action provoquera la suppression de la connexités B." : "");
        });
        this.moveSourceTooltip = ko.computed(function () {
            if (!vm.Affaire || !vm.Affaire.CodeOffre()) {
                return "";
            }
            return "Déplacer le contrat " + vm.Affaire.CodeOffre() + " dans la connexité B."
                + (vm.Connexites().length <= 2 ? " Cette action provoquera la suppression de la connexités A." : "");
        });

        this.merge = function () {
            vm.fusionState(mergeAction);
        };
        this.pickContract = function () {
            vm.fusionState(pickAction);
        };
        this.moveSource = function () {
            vm.fusionState(moveSourceAction);
        };
        this.fusionState = ko.observable(null);
        this.resetState = function () {
            vm.fusionState("");
        };
        this.confirmMessage = ko.computed(function () {
            if (!vm.fusionState()) {
                return null;
            }
            let toolTip = vm[vm.fusionState() + "Tooltip"]();
            if (!toolTip) {
                return null;
            }
            let message = "Etes-vous sûr(e) de vouloir " + toolTip[0].toLowerCase() + toolTip.substr(1);
            let i = message.indexOf(".");
            message = Array.from(message);
            message[i] = " ?";
            return message.join("");
        });
        this.init = function () {
            common.page.isLoading = true;
            common.$postJson("/Connexites/GetModelFusionConnexites", { affaire: { CodeOffre: parameters.ipb, Version: parameters.alx, Type: "P" }, affaireCible: { CodeOffre: parameters.ipbCible, Version: parameters.alxCible, Type: "P" }, t: parameters.type }, true)
                .done(function (data) {
                    ko.mapping.fromJS(data, {}, vm);
                    vm.Observations(parameters.obsv);
                    if (vm.fusionState() === null) {
                        vm.initState();
                        $(document).trigger(customEvents.connexites.fusion.loaded);
                    }
                    common.page.isLoading = false;
                })
        };
        this.abort = function () {
            $(document).trigger(customEvents.connexites.fusion.cancel);
        };
        this.confirmState = ko.observable(null);
        this.confirmState.subscribe(function (state) {
            if (state !== null) {
                ko.tasks.schedule(function () { vm.confirm(); });
            }
        });

        this.confirm = function () {
            if (!vm.fusionState()) {
                return;
            }
            let state = vm.confirmState();
            if (state === null) {
                return common.$ui.showConfirm(
                    vm.confirmMessage(),
                    "Confirmation",
                    function () { vm.confirmState(true); },
                    function () { vm.confirmState(false); },
                    null, null,
                    function () { ko.tasks.schedule(function () { vm.confirmState(null); }); });
            }
            else {
                vm.confirmState(null);
                if (state === false) {
                    return;
                }
            }
            vm.applyChanges();
        };

        this.setPicked = function () {
            let pickedOne = vm.ConnexitesExternes().filter(function (x) {
                return x.Folder.CodeOffre() == vm.AffaireCible.CodeOffre() && x.Folder.Version() == vm.AffaireCible.Version()
            })[0];
            vm.ConnexitesExternes.mappedRemove(pickedOne);
            vm.Connexites.mappedCreate(pickedOne);
        };

        this.setMerged = function () {
            let array = ko.utils.unwrapObservable(vm.ConnexitesExternes).map(function (x) { return ko.mapping.toJS(x); });
            vm.ConnexitesExternes.mappedDestroyAll();
            while (vm.ConnexitesExternes().length) {
                vm.ConnexitesExternes.mappedRemove(vm.ConnexitesExternes()[0]);
            }
            while (array.length) {
                vm.Connexites.mappedCreate(array.shift());
            }
        };

        this.setSourceInTarget = function () {
            let source = vm.Connexites().filter(function (x) {
                return x.Folder.CodeOffre() == vm.Affaire.CodeOffre() && x.Folder.Version() == vm.Affaire.Version()
            })[0];
            vm.Connexites.mappedRemove(source);
            vm.ConnexitesExternes.mappedCreate(source);
        };

        this.initState = function () {
            if (vm.fusionState() !== null) {
                return;
            }
            vm.fusionState("");
            vm.fusionState.subscribe(function (s) {
                switch (s) {
                    case "":
                        vm.init();
                        break;
                    case mergeAction:
                        vm.setMerged();
                        break;
                    case pickAction:
                        vm.setPicked();
                        break;
                    case moveSourceAction:
                        vm.setSourceInTarget();
                        break;
                }
            });
        };

        this.applyChanges = function () {
            if (!vm.fusionState()) {
                return;
            }
            common.page.isLoading = true;
            let action = "";
            switch (vm.fusionState()) {
                case mergeAction:
                    action = "MergeConnexites";
                    break;
                case pickAction:
                    action = "PickTarget";
                    break;
                case moveSourceAction:
                    action = "MoveSource";
                    break;
            }
            let viewModel = ko.mapping.toJS(vm);
            viewModel.Connexites = [];
            viewModel.ConnexitesExternes = [];
            common.$postJson("/FusionConnexites/" + action, viewModel, true).done(function () {
                $(document).trigger(customEvents.connexites.fusion.terminated);
            });
        };

        this.init();
    };

    affaire.connexites.fusion.ViewModel = fusionConnexitesViewModel;
})();