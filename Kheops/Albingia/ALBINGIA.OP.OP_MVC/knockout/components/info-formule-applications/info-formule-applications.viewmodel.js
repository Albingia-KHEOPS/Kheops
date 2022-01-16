(function () {
    var ApplicationsViewModel = function (parameters) {
        let vm = this;
        let infosApps = window.infoApplications;
        const CodeGareat = "GAREAT";
        if (!window.infoApplications) {
            common.error.showMessage("Les informations du \"s'applique à\" sont introuvables");
            return;
        }

        var mappings = {
            "Risques": {
                create: function (options) {
                    return new mappings.AppRisque(options.data);
                }
            },
            "Objets": {
                create: function (options) {
                    return new mappings.AppObjet(options.data);
                }
            },
            AppRisque: function (data) {
                ko.mapping.fromJS(data, mappings, this);
                var rsq = this;
                rsq.isRisqueApplicable = ko.computed(function () {
                    return vm.isReadonly && !vm.isReadonly() && (rsq.Numero() == vm.NumRisque() || rsq.IsApplicable());
                });
            },
            AppObjet: function (data) {
                ko.mapping.fromJS(data, {}, this);
                var obj = this;

                obj.isSelected = ko.pureComputed({
                    read: function () {
                        return !obj.isApplicable()
                            || vm.selection().length > 0 && vm.selection().some(function (x) { return x == obj.Code(); }) && obj.NumRisque() == vm.risqueSelectionne();
                    },
                    write: function (value) {
                        if (value) {
                            if (obj.NumRisque() != vm.risqueSelectionne()) {
                                vm.selection.removeAll();
                            }

                            vm.selection.push(obj.Code());
                            ko.tasks.schedule(function () {
                                vm.risqueSelectionne(obj.NumRisque());
                            });
                        }
                        else {
                            var a = vm.selection();
                            vm.selection.splice(a.indexOf(obj.Code()), 1);
                        }
                    }
                }).extend({ notify: "always" });

                obj.isApplicable = ko.computed(function () {
                    return !obj.IsApplique()
                        || (obj.NumRisque() == vm.NumRisque()
                            && (vm.NumsObjets().length == 0 || vm.NumsObjets().some(function (n) { return n == obj.Code(); })));
                });
            }
        };

        this.isAffaireReadonly = ko.observable(parameters.isReadonly || parameters.isModifHorsAvenant);
        this.formGen = parameters.formGen ? parseInt(parameters.formGen) : 0;
        infosApps.risqueSelectionne = infosApps.NumRisque;
        infosApps.selection = infosApps.NumsObjets.length == 0 && infosApps.NumRisque > 0 ?
            infosApps.Risques.filter(function (r) { return r.Numero == infosApps.NumRisque; })[0].Objets.map(function (o) { return o.Code; })
            : infosApps.NumsObjets;

        this.isReadonly = ko.pureComputed(function () {
            return vm.isAffaireReadonly && (vm.isAffaireReadonly() || vm.IsAvnDisabled());
        });

        ko.mapping.fromJS(infosApps, mappings, this);
        
        this.firstSaving = ko.observable(false);
        this.libelleInitial = ko.observable(vm.LibelleFormule());
        this.LibelleFormule.extend({ required: { errorMessage: "", validateOn: vm.firstSaving } });
        this.selectedVolets = ko.observableArray([""]);
        this.voletUnique = ko.observable("");
        this.refreshSelectedVolets = function () {
            console.log("recherche volets");
            let vmFormule = ko.dataFor(common.dom.get("formule_option").firstChild);
            if (!vmFormule || !vmFormule.option || !vmFormule.option() || !vmFormule.option().Volets) {
                console.log("no vm formule");
                return;
            }
            if (vmFormule.option().Volets().length == 1) {
                console.log("volet unique");
                let uniqueLabel = vmFormule.option().Volets()[0].Libelle();
                vm.voletUnique(uniqueLabel);
                vm.selectedVolets([uniqueLabel]);
            }
            else {
                let volets = vmFormule.option().Volets()
                    .filter(function (v) { return v.IsChecked() && v.Code() !== CodeGareat; })
                    .map(function (v) { return v.Libelle(); });

                if (!volets || volets.length == 0) {
                    console.log("pas de volets selectionnés");
                }
                if (volets.length == 1) {
                    console.log("un volet selectionné");
                    vm.voletUnique(volets[0]);
                }
                else {
                    console.log("tri volets selectionnés");
                    volets.sort();
                }
                console.log("charger les volets");
                vm.selectedVolets([""].concat(volets));
            }
        };
        this.autoFillLabel = ko.observable(!vm.isReadonly() && !vm.LibelleFormule());
        this.autoFilledLabel = ko.observable("");
        this.displayInputLabel = ko.computed(function () {
            return vm.autoFillLabel && !vm.autoFillLabel();
        });
        this.displaySelectVoletLabel = ko.computed(function () {
            return vm.autoFillLabel && vm.autoFillLabel();
        });
        this.autoFillLabel.subscribe(function (checked) {
            if (vm.isReadonly()) {
                return;
            }
            if (checked) {
                vm.libelleInitial(vm.LibelleFormule());
                vm.refreshSelectedVolets();
                if (vm.voletUnique()) {
                    vm.autoFilledLabel(vm.voletUnique());
                }
                else {
                    let match = vm.selectedVolets().filter(function (v) { return v.trim().toLowerCase() === vm.LibelleFormule().trim().toLowerCase(); })[0];
                    if (match) {
                        vm.autoFilledLabel(match);
                    }
                    else if (vm.selectedVolets().length == 2) {
                        vm.autoFilledLabel(vm.selectedVolets()[1]);
                    }
                }
            }
            else {
                vm.LibelleFormule(vm.libelleInitial());
            }
        });
        this.autoFilledLabel.subscribe(function (label) {
            if (vm.isReadonly()) {
                return;
            }
            if (vm.autoFillLabel()) {
                vm.LibelleFormule(label);
                if (!vm.libelleInitial()) {
                    vm.libelleInitial(label);
                }
            }
        });

        this.LibelleFormule.subscribe(function (label) {
            ko.tasks.schedule(function () {
                $(document).trigger(customEvents.formule.labelChanged, [label]);
            });
        });

        this.risque = ko.computed(function () {
            var n = vm.risqueSelectionne();
            return vm.Risques().first(function (r) { return r.Numero() == n; });
        });

        this.libelleCible = ko.computed(function () {
            return vm.risque() ? (vm.risque().Cible.Code() + " - " + vm.risque().Cible.Label()) : "";
        });

        this.displayApplications = function () {
            common.page.scrollTop();
            $("#divLstRsqObj").show();
        };

        this.cancelChanges = function () {
            if (vm.NumRisque() !== 0) {
                vm.risqueSelectionne(vm.NumRisque());
                vm.selection.removeAll();
                ko.utils.arrayPushAll(vm.selection, infosApps.selection);
            }
            $("#divLstRsqObj").hide();
            $(document).trigger(customEvents.formule.cancelApplication);
        };
        this.selectRisque = function (data) {
            if (data.isRisqueApplicable() && data.Numero() != vm.risqueSelectionne()) {
                vm.selection.removeAll();
            }
        };
        this.risqueSelectionne.subscribe(function (val) {
            var selectedRisque = vm.Risques().first(function (r) { return r.Numero() == val; });
            var noneSelected = vm.selection().length == 0;
            if (noneSelected) {
                ko.tasks.schedule(function () {
                    selectedRisque.Objets().filter(function (x) { return x.isApplicable(); }).forEach(function (o) { o.isSelected(true); });
                });
            }
        });

        this.validateChanges = function () {
            if (!vm.risque() || vm.selection().length == 0) {
                common.error.showMessage("Veuillez sélectionner au moins un objet");
                return;
            }
            common.page.isLoading = true;
            let info = ko.mapping.toJS(vm);
            let hasValueChanged = true;
            let isNewRisque = info.risqueSelectionne != info.NumRisque;
            if (!isNewRisque) {
                var r = info.Risques.first(function (r) { return r.Numero == info.NumRisque; });
                var selectSet = new Set(info.selection);
                if (info.NumsObjets.length == 0) {
                    if (selectSet.size == r.Objets.length) {
                        // nothing has changed
                        hasValueChanged = false;
                        $("#divLstRsqObj").hide();
                    }
                    else {
                        // selection has been reduced
                    }
                }
                else if (selectSet.size != info.NumsObjets.length) {
                    // selection has been changed
                }
                else {
                    var intersection = info.NumsObjets.filter(function (o) { return selectSet.has(o); });
                    if (intersection.length != info.NumsObjets.length) {
                        // selection has been changed
                    }
                    else {
                        // take no action
                        hasValueChangedhasChanged = false;
                        $("#divLstRsqObj").hide();
                    }
                }
            }
            else if (currentAffair.type == "O") {
                let newRisque = info.Risques.first(function (r) { return r.Numero == info.risqueSelectionne; });
                let currentRisque = info.Risques.first(function (r) { return r.Numero == info.NumRisque; });
                if (!currentRisque || newRisque.Cible.Code != currentRisque.Cible.Code) {
                    common.dialog.initConfirm(vm.changeApplications(true), null, "Vous êtes sur le point supprimer l'option courante car la cible du risque sélectionné est différente. Vous allez être redirigé vers une autre formule. Voulez-vous continuer ?");
                    return;
                }
            }

            if (!hasValueChanged) {
                common.page.isLoading = false;
                return;
            }

            vm.changeApplications(isNewRisque);
        };

        this.changeApplications = function (isNewRisque) {
            var vmFormule = ko.dataFor(document.getElementsByTagName('options-formule')[0].children[0]);
            vmFormule.setOptions().done(
                function () {
                    common.$postJson("/FormuleGarantie/ChangeApplications",
                        {
                            affaire: window.currentAffair,
                            pageContext: window.infosTab,
                            numFormule: vm.NumFormule(),
                            numRisque: vm.risqueSelectionne(),
                            numsObjets: vm.selection(),
                            dateAvenant: vm.DateAvenantOption(),
                            numOption: vmFormule.currentOption()
                        }, true)
                        .done(function (result) {
                            if (infosApps.NumRisque == 0) {
                                window.document.location.href = "/FormuleGarantie/Index?isRefresh=true&id=" +
                                    encodeURIComponent(result.fullId);
                            }
                            else if (window.currentAffair.type == "P" || !isNewRisque) {
                                infosApps.selection = ko.mapping.toJS(vm.selection);
                                vmFormule.refreshFormule(
                                    function (data) {
                                        ko.mapping.fromJS(data.Applications, {}, vm);
                                        $("#divLstRsqObj").hide();
                                    }
                                );
                            }
                            else {
                                window.document.location.href = [
                                    "/FormuleGarantie",
                                    "Index",
                                    [
                                        result.id,
                                        result.formule,
                                        result.option,
                                        [infosApps.NumRisque, "tabGuid", infosTab.tabGuid, "tabGuidmodeNavig", infosTab.modeNavigation, "modeNavig"].join("")
                                    ].join("_")
                                ].join("/");
                            }
                        });
                }
            );
        };

        if (infosApps.NumRisque == 0) {
            console.log("No Risque has been found");
            $("#divLstRsqObj").show();
            if (infosApps.Risques.filter(function (r) { return r.IsApplicable == true; }).length == 1) {
                var selectRsq = infosApps.Risques.filter(function (r) { return r.IsApplicable == true; })[0].Numero;
                vm.risqueSelectionne(selectRsq);
                infosApps.risqueSelectionne = selectRsq;
                setTimeout(function () {
                    vm.validateChanges();
                }, 100);
            }
        }

        $(document).on(customEvents.formule.saving, function () {
            vm.firstSaving(true);
        });
        $(document).on(customEvents.formule.voletChecked, function (evt, checked, label) {
            console.log("selection volet");
            if (!vm.autoFillLabel()) {
                console.log("missing autoFillLabel");
                return;
            }
            if (!checked && label == vm.autoFilledLabel()) {
                vm.autoFilledLabel("");
            }
            let currentList = vm.selectedVolets();
            if (checked) {
                currentList.push(label);
                currentList.sort();
            }
            else {
                currentList = currentList.filter(function (x) { return x !== label; });
            }
            ko.tasks.schedule(function () {
                console.log("maj selection volets");
                vm.selectedVolets([]);
                ko.utils.arrayPushAll(vm.selectedVolets, currentList);
                if (vm.selectedVolets().length == 2) {
                    vm.autoFilledLabel(vm.selectedVolets()[1]);
                }
            });
        });
        $(document).on(customEvents.formule.rendered, function () {
            vm.refreshSelectedVolets();
            if (vm.voletUnique()) {
                vm.autoFilledLabel(vm.voletUnique());
            }
        });
        $(document).on(customEvents.formule.startedNewAvenantFormule, function () {
            vm.IsAvnDisabled(false);
            $("#IsForceReadOnly").val("False");
        });
        $(document).on(customEvents.formule.cancelledNewAvenantFormule, function () {
            vm.IsAvnDisabled(true);
            $("#IsForceReadOnly").val("True");
        });
    };
    ApplicationsViewModel.prototype.dispose = function () {
        $(document).off(customEvents.formule.voletChecked);
        $(document).off(customEvents.formule.startedNewAvenantFormule);
        $(document).off(customEvents.formule.cancelledNewAvenantFormule);
        $(document).off(customEvents.formule.rendered);
    };
    affaire.formule.ApplicationsViewModel = ApplicationsViewModel;
})();