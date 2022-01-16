
(function () {
    let viewModel = function (parameters) {
        let Relance = function (options) {
            if (options.data.DateValidation.length > 10) {
                options.data.DateValidation = options.data.DateValidation.trim().substr(0, 10);
            }
            if (options.data.DateRelance.length > 10) {
                options.data.DateRelance = options.data.DateRelance.trim().substr(0, 10);
            }
            let relance = ko.mapping.fromJS(options.data, {}, this);
            this.isDateRelanceInvalid = ko.computed(function () {
                return this.Situation() === "" && this.DateRelance.hasValueChanged() && FrDate.compare(new FrDate(this.DateRelance()), FrDate.today()) != 1;
            }, relance);
            relance.situationChanged = ko.computed(function () { return relance.Situation() !== ""; });
            relance.DateRelance.extend({ required: { errorMessage: "", ignoreIf: this.situationChanged } });
            relance.DateRelance.extend({
                watcher: {
                    logChanges: false,
                    name: "DateRelance",
                    onChanged: function () {
                        ko.tasks.schedule(function () { vm.Relances.notifySubscribers(); });
                    }
                }
            });
            relance.IsAttenteDocCourtier.extend({ watcher: { logChanges: false, name: "AttenteDoc" } });

            relance.hasChanged = function () {
                return relance.Situation && relance.DateRelance && relance.IsAttenteDocCourtier
                    && relance.DateRelance.hasValueChanged && relance.IsAttenteDocCourtier.hasValueChanged
                    && relance.Situation() !== "" || relance.DateRelance.hasValueChanged() || relance.IsAttenteDocCourtier.hasValueChanged();
            };
        };
        let mapRelance = {
            key: function (item) {
                return ko.utils.unwrapObservable(item.Offre.Code) + "_" + ko.utils.unwrapObservable(item.Offre.Version);
            },
            create: function (options) {
                return new Relance(options);
            }
        };
        ko.mapping.fromJS(window.modelRelances, { "Relances": mapRelance }, this);
        let vm = this;
        this.hasResults = ko.computed(function () {
            return (vm.Relances() || []).length > 0;
        });

        this.showInPage = parameters.showInPage;

        this.listName = ko.computed(function () { return "Relances"; });
        this.hideList = function () {
            if (vm.hasUnsavedChanges()) {
                common.$ui.showConfirm(
                    "Des modifications on été effectuées mais n'ont pas été enregistrées. Que voulez-vous faire ?",
                    "Confirmation",
                    function () { vm.terminate(); },
                    function () { vm.saveAndQuit(); },
                    "Fermer sans enregistrer",
                    "Enregistrer et fermer",
                    function () { },
                    400);
                return;
            }
            vm.terminate();
        };

        this.motifsSituations = ko.observableArray();
        common.$postJson("/PrisePosition/GetMotifsSituations", {}, false).done(function (list) {
            ko.mapping.fromJS(list, {}, vm.motifsSituations);
        });

        this.hasUnsavedChanges = ko.computed(function () {
            return vm.Relances && vm.Relances().some(function (r) { return typeof r.hasChanged === "function" && r.hasChanged(); });
        });

        this.listInvalidChanges = function () {
            if (!vm.hasUnsavedChanges()) {
                return null;
            }
            let errors = vm.Relances()
                .filter(function (r) { return r.Situation() == "W" && !r.MotifStatut(); })
                .map(function (r) { return r.Offre.Code() + "_" + r.Offre.Version() + " : Le motif de situation n'a pas été défini"; });

            errors = errors.concat(vm.Relances()
                .filter(function (r) { return r.Situation() == "" && r.isDateRelanceInvalid(); })
                .map(function (r) { return r.Offre.Code() + "_" + r.Offre.Version() + " : La date de relance doit être superieure à la date courante"; }));

            return errors;
        };

        this.save = function (next) {
            if (!this.hasUnsavedChanges()) {
                if ($.isFunction(next)) { next(); }
                return;
            }

            let invalidChanges = (vm.listInvalidChanges() || []);
            if (invalidChanges.length > 0) {
                common.$ui.showMessage(invalidChanges.join("\n"), "error");
                return;
            }

            common.page.isLoading = true;
            let viewModel = ko.mapping.toJS(vm);
            viewModel.Relances = vm.Relances().filter(function (x) {
                return x.hasChanged();
            }).map(function (x) { return ko.mapping.toJS(x); });

            common.$postJson("/Offre/UpdateRelances", viewModel, true).done(afterUpdate);

            function afterUpdate() {
                if ($.isFunction(next)) {
                    if (next == vm.terminate) {
                        vm.terminate();
                        return;
                    } else {
                        next();
                    }
                }
                else {
                    let vmPaging = ko.dataFor(common.dom.get("relancesPaging").firstChild);
                    $(document).trigger(customEvents.paging.change, [vmPaging.currentPage(), vm.listName(), true]);
                }
            }
        };

        this.saveAndQuit = function () {
            vm.save(vm.terminate);
        };

        this.terminate = function () {
            if (this.showInPage) {
                window.location.href = "/"
            } else {
                if (window.modelRelances.DoNotShowAgainForToday != vm.DoNotShowAgainForToday()) {
                    // update profile flag
                    common.page.isLoading = true;
                    common.$postJson("/User/UpdateProfileKheopsFlag", { profileData: "ShowImpayesOnStartup", value: !vm.DoNotShowAgainForToday() }, true).done(function () {
                        common.page.isLoading = false;
                        let newCheckValue = vm.DoNotShowAgainForToday();
                        setTimeout(function () {
                            window.modelRelances.DoNotShowAgainForToday = newCheckValue;
                        }, 0);
                        $(document).trigger(customEvents.relances.hiding);
                    });
                }
                else {
                    common.page.isLoading = false;
                    $(document).trigger(customEvents.relances.hiding);
                }
            }
        };

        this.getRelances = function (page) {
            common.$getJson("/Offre/GetRelances", { page: page }, true).done(function (pglist) {
                vm.Relances.mappedRemoveAll(vm.Relances);
                ko.tasks.schedule(function () {
                    pglist.List.forEach(function (r) {
                        vm.Relances.mappedCreate(r);
                    });
                    $(document).trigger(customEvents.paging.dataLoaded, [vm.listName(), pglist]);
                });
            });
        };

        $(document).on(customEvents.paging.change, function (e, page, name, isReloading) {
            if (name !== vm.listName()) {
                return;
            }

            if (!isReloading && vm.hasUnsavedChanges()) {
                common.page.isLoading = false;
                common.$ui.showConfirm(
                    "Des modifications on été effectuées mais n'ont pas été enregistrées. Que voulez-vous faire ?",
                    "Confirmation",
                    function () { vm.getRelances(page); },
                    function () { vm.save(function () { vm.getRelances(page); }); },
                    "Ne pas enregistrer",
                    "Enregistrer",
                    function () { },
                    400);
                return;
            }

            vm.getRelances(page);
        });

        $(document).trigger(customEvents.relances.loaded);
    };
    viewModel.prototype.dispose = function () {
        $(document).off(customEvents.paging.change);
    };
    affaire.relances.ListViewModel = viewModel;
})();
