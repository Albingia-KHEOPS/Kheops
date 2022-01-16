
(function () {
    var PorteeGarantieViewModel = function (parameters, emptyVM) {
        emptyVM.id = parameters.id;
        emptyVM.isReadonly = parameters.isReadonly;
        emptyVM.title = "";
        var vm = this;

        var mappings = {
            root: {
                "ObjetsRisque": {
                    create: function (options) {
                        return new mappings.Objet(options.data);
                    }
                }
            },
            Objet: function (data) {
                ko.mapping.fromJS(data, {}, this);
                var obj = this;

                this.label = ko.computed(function () {
                    return "Objet " + obj.NumObjet() + " - " + obj.LabelObjet();
                });

                if (vm.hasBonus()) {
                    this.canChangeUniteTaux = ko.computed(function () {
                        return !vm.isReadonly() && (!obj.TypeCalculPortee() || obj.TypeCalculPortee() == "X");
                    });
                    this.valeurIsNil = ko.computed(function () {
                        return !obj.ValeurPortee()
                            || typeof obj.ValeurPortee() === "number" && obj.ValeurPortee()  === 0
                            || typeof obj.ValeurPortee() === "string" && parseInt(obj.ValeurPortee().replace(/[\s\,\.]/g, "")) == 0;
                    });
                    this.ignoreTypeCalcul = ko.computed(function () {
                        return !obj.IsSelected() || (!obj.TypeCalculPortee() && !obj.UniteTaux() && obj.valeurIsNil());
                    });
                    this.ignoreTaux = ko.computed(function () {
                        return !obj.IsSelected() || !obj.canChangeUniteTaux() || (!obj.TypeCalculPortee() && !obj.UniteTaux() && obj.valeurIsNil());
                    });
                    this.ValeurPortee.extend({ required: { errorMessage: "", ignoreIfNot: obj.IsSelected } });
                    this.TypeCalculPortee.extend({ required: { errorMessage: "", ignoreIf: obj.ignoreTypeCalcul } });
                    this.UniteTaux.extend({ required: { errorMessage: "", ignoreIf: obj.ignoreTaux } });
                    this.checkValidity = function () {
                        return ko.extenders.required.checkValidity(obj);
                    };
                    this.primeCalculee = ko.computed(function () {
                        if (obj.ValeurPortee.requireState.isValid() && obj.TypeCalculPortee() && obj.UniteTaux()) {
                            if (obj.TypeCalculPortee().toUpperCase() == "M") return obj.ValeurPortee();
                            if (obj.TypeCalculPortee().toUpperCase() == "X") {
                                var valMultiplied = parseFloat(obj.Valeur()) * obj.ValeurPortee();
                                if (isNaN(valMultiplied)) return 0;
                                switch (obj.UniteTaux().toUpperCase()) {
                                    case "D": return valMultiplied;
                                    case "%": return valMultiplied / 100;
                                    case "%0": return valMultiplied / 1000;
                                }
                            }
                        }

                        return 0;
                    });

                    this.TypeCalculPortee.subscribe(function (tp) {
                        if (tp == "M") {
                            obj.UniteTaux("D");
                        }
                    });
                }
            }
        };

        ko.mapping.fromJS(emptyVM, mappings.root, vm);

        vm.garantie = {
            Affaire: window.currentAffair,
            NumOption: 1,
            NumFormule: parameters.numFormule,
            CodeBloc: parameters.codeBloc,
            Sequence: parameters.idGarantie,
            IsReadonly: parameters.isReadonly
        };
        vm.nbObjets = ko.computed(function () {
            return vm.ObjetsRisque().length;
        });

        vm.hasBonus = ko.computed(function () {
            return vm.AlimentationAssiette() == "B" || vm.AlimentationAssiette() == "C";
        });

        vm.labelRisque = ko.computed(function () {
            return "Risque " + vm.CodeRisque() + " - " + vm.DesignationRisque();
        });

        vm.totalObjsAmount = ko.computed(function () {
            if (!vm.hasBonus()) return 0;
            if (vm.ObjetsRisque().length == 0 || vm.ObjetsRisque().some(function (o) { return o.primeCalculee && o.primeCalculee() == 0 && o.IsSelected(); })) {
                return 0;
            }

            return vm.ObjetsRisque()
                .filter(function (o) { return o.IsSelected(); })
                .reduce(function (tt, o) { return tt + (o.primeCalculee ? o.primeCalculee() : 0); }, 0);
        });

        vm.ObjetsRisque().forEach(function (obj) {
            obj.isReadonly = vm.isReadonly;
        });

        vm.init = function () {
            common.page.isLoading = true;
            if (window.currentPortees) {
                if (Array.isArray(window.currentPortees.ObjetsRisque)) {
                    window.currentPortees.ObjetsRisque.sort(function (o1, o2) {
                        return o1.NumObjet == o2.NumObjet ? 0 : o1.NumObjet < o2.NumObjet ? -1 : 1;
                    });
                }
                ko.mapping.fromJS(window.currentPortees, mappings.root, vm);
                common.page.isLoading = false;
            }
            else {
                //check whether doing it
            }
        };

        vm.cancelEdit = function () {
            $(document).trigger(customEvents.porteesGarantie.cancelledEdit);
        };

        vm.validatePortee = function () {
            common.page.isLoading = true;
            common.$postJson("/FormuleGarantie/ValidatePortees", {
                garantieId: vm.garantie,
                portees: ko.mapping.toJS(vm)
            }, true).done(function (data) {
                common.page.isLoading = false;
                $(document).trigger(customEvents.porteesGarantie.validatedEdit, ko.mapping.toJS(vm));
            });
        };

        vm.validateEdit = function () {
            if (vm.isReadonly()) {
                vm.cancelEdit();
                return;
            }

            var selection = vm.ObjetsRisque().filter(function (o) { return o.IsSelected(); });
            if (vm.hasBonus()) {
                if (vm.CodeAction()) {
                    var states = vm.ObjetsRisque().map(function (o) { return o.checkValidity(); });
                    var errors = states.filter(function (s) { return typeof s === "string" && s; });
                    var badFlags = states.filter(function (s) { return typeof s === "boolean" && !s; });
                    if (errors.length > 0) {
                        common.error.showMessage(states.join("\n"));
                    }
                    else if (badFlags.length > 0) {
                        common.error.showMessage(common.error.messages.requiredFields);
                    }
                    if (errors.length > 0 || badFlags.length > 0) return;
                }
            }
            else {
                var error = "";
                if (selection.length == 0 && vm.CodeAction()) {
                    error = "Vous devez sélectionner au moins un objet.";
                }
                else if (selection.length == vm.ObjetsRisque().length) {
                    error = "Vous ne pouvez pas sélectionner l'ensemble des objets.";
                }
                else if (selection.length != 0 && !vm.CodeAction()) {
                    error = "Vous devez sélectionner au moins une action.";
                }
                if (error) {
                    common.error.showMessage(error);
                    return;
                }
            }

            if (vm.isVirtual() && (!vm.CodeAction() || selection.length == 0)) {
                vm.cancelEdit();
            }
            else if (!vm.CodeAction()) {
                common.dialog.initConfirm(vm.validatePortee, null, "Etes-vous sûr de vouloir supprimer les informations d'accord/exclusion ?");
            }
            else {
                vm.validatePortee();
            }
        };

        vm.init();
    };

    affaire.formule.PorteeGarantieViewModel = PorteeGarantieViewModel;
})();
