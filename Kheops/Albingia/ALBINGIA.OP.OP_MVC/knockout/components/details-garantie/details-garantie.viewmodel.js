
(function () {

    var DetailsGarantieViewModel = function (parameters, emptyVM) {
        var vm = this;
        var mappings = {
            "DetailsValeurs": {
                create: function (options) {
                    var valeurs = ko.mapping.fromJS(options.data, {}, this);
                    valeurs.labelGarantie = ko.computed(function () {
                        return valeurs.CodeGarantie() + " - " + valeurs.LibGarantie();
                    });
                    valeurs.typeControle = ko.computed(function () {
                        return valeurs.CodeTypeControle() + " - " + valeurs.LibTypeControle();
                    });
                    valeurs.uniteFormat = function (data) {
                        var unite = valeurs[data + "Unite"]();
                        return unite && unite.trim() == "%" ? "pourcentdecimal" : "decimal";
                    };
                    valeurs.valeurReelle = function (data) {
                        return valeurs.FranchiseValeur() == 0 && valeurs.FranchiseUnite() == "" ? "" : parseInt(valeurs[data + "Valeur"]());
                    };
                    return valeurs;
                }
            }
        };
        ko.mapping.fromJS(emptyVM, mappings, this);
        vm.isReadonly = parameters.isReadonly;
        vm.numeroAvenant = ko.observable(parameters.numeroAvenant);
        vm.dateAvenant = ko.observable(parameters.dateAvenant);
        vm.isInitialized = ko.observable(false);
        vm.title = ko.computed(function () {
            return vm.Code() ? vm.Code() + ' - ' + vm.Libelle() : "";
        });
        vm.errors = ko.observableArray();
        vm.hours = ko.observableArray();
        vm.minutes = ko.observableArray();
        for (var h = 0; h < 24; h++) {
            vm.hours.push(h.toString().padStart(2, "0"));
        }

        for (var m = 0; m < 60; m++) {
            vm.minutes.push(m.toString().padStart(2, "0"));
        }

        vm.frDateDebutStd = ko.pureComputed(function () {
            return new FrDate(vm.DateDebutStd());
        });
        vm.dateDebutStd = ko.pureComputed(function () {
            var d = vm.frDateDebutStd();
            return d.isValid && d.year > 1900 ? d.toDateString() : "";
        });
        vm.heureDebutStd = ko.pureComputed(function () {
            var d = vm.frDateDebutStd();
            return d.isValid && d.year > 1900 ? d.toTimeString() : "";
        });
        vm.frDateFinStd = ko.pureComputed(function () {
            return new FrDate(vm.DateFinStd());
        });
        vm.dateFinStd = ko.pureComputed(function () {
            var d = vm.frDateFinStd();
            return d.isValid && d.year > 1900 ? d.toDateString() : "";
        });
        vm.heureFinStd = ko.pureComputed(function () {
            var d = vm.frDateFinStd();
            return d.isValid && d.year > 1900 ? d.toTimeString() : "";
        });

        vm.frDateDebut = ko.observable(null);
        vm.frDateFin = ko.observable(null);

        vm.dateDebut = ko.pureComputed({
            read: function () {
                return vm.frDateDebut() ? vm.frDateDebut().toDateString() : "";
            },
            write: function (value) {
                if (!value) {
                    vm.frDateDebut(null);
                }
                else {
                    var hour = (vm.heuresDebut() || "00") + ":" + (vm.minutesDebut() || "00");
                    vm.frDateDebut(new FrDate(value + " " + hour));
                }
            }
        });
        vm.dateFin = ko.pureComputed({
            read: function () {
                return vm.frDateFin() ? vm.frDateFin().toDateString() : "";
            },
            write: function (value) {
                if (!value) {
                    vm.frDateFin(null);
                    vm.heuresFin(null);
                    vm.minutesFin(null);
                }
                else {
                    var hour = (vm.heuresFin() || "00") + ":" + (vm.minutesFin() || "00");
                    vm.frDateFin(new FrDate(value + " " + hour));
                }
            }
        });
        vm.dateFin.subscribe(function (value) {
            if (!value) {
                vm.heuresFin(null);
                vm.minutesFin(null);
            } else if ((vm.heuresFin() === null || vm.heuresFin() === "") && (vm.minutesFin() === null || vm.minutesFin() === "")) {
                vm.heuresFin(23);
                vm.minutesFin(59);
            } 
        });
        vm.isFinEmpty = ko.pureComputed(function () {
            return !vm.dateFin();
        });
        vm.heuresDebut = ko.observable("00");
        vm.heuresDebut.subscribe(function (h) {
            if (!vm.frDateDebut()) return;
            if (!h || h == "00") h = 0;
            else h = parseInt(h);
            FrDate.ko.update(vm.frDateDebut, h, "hours");
        });
        vm.minutesDebut = ko.observable("00");
        vm.minutesDebut.subscribe(function (m) {
            if (!vm.frDateDebut()) return;
            if (!m || m == "00") m = 0;
            else m = parseInt(m);
            FrDate.ko.update(vm.frDateDebut, m, "minutes");
        });
        vm.heuresFin = ko.observable("");
        vm.heuresFin.subscribe(function (h) {
            if (!vm.frDateFin()) return;
            if (!h || h == "00") h = 0;
            else h = parseInt(h);
            FrDate.ko.update(vm.frDateFin, h, "hours");
        });
        vm.minutesFin = ko.observable("");
        vm.minutesFin.subscribe(function (m) {
            if (!vm.frDateFin()) return;
            if (!m || m == "00") m = 0;
            else m = parseInt(m);
            FrDate.ko.update(vm.frDateFin, m, "minutes");
        });

        vm.enterDateFin = ko.observable(false);
        vm.enterDuree = ko.observable(false);

        vm.enterDateFin.subscribe(function (value) {
            if (!vm.isInitialized()) return;
            if (value) {
                vm.enterDuree(false);
            }
            else {
                vm.dateFin(null);
                vm.heuresFin("");
                vm.minutesFin("");

            }
        });
        vm.enterDuree.subscribe(function (value) {
            if (!vm.isInitialized()) return;
            if (value) {
                vm.enterDateFin(false);
                vm.Duree(1);
            }
            else {
                vm.Duree("");
                vm.DureeUnite("");
            }
        });

        vm.Duree.extend({ required: { errorMessage: "Le durée doit être supérieure à 0", ignoreIfNot: vm.enterDuree, NaNAsEmpty: true, zeroAsEmpty: true } });
        vm.DureeUnite.extend({ required: { errorMessage: "", ignoreIfNot: vm.enterDuree } });
        vm.Duree.subscribe(function (d) {
            vm.IsDuree(!!d);
        });

        vm.heuresDebut.extend({ required: { errorMessage: "", ignoreIfNot: vm.dateDebut } });
        vm.minutesDebut.extend({ required: { errorMessage: "", ignoreIfNot: vm.dateDebut } });

        vm.dateFin.extend({ required: { errorMessage: "", ignoreIfNot: vm.enterDateFin } });
        vm.heuresFin.extend({ required: { errorMessage: "", ignoreIfNot: vm.enterDateFin } });
        vm.minutesFin.extend({ required: { errorMessage: "", ignoreIfNot: vm.enterDateFin } });

        vm.cannotChangeGarantieIndexee = ko.computed(function () {
            return vm.isReadonly || vm.CodeNature() == "C" || vm.NatureRetenue.Code() == "C";
        });

        vm.cannotChangeCATNAT = ko.computed(function () {
            return vm.cannotChangeGarantieIndexee();
        });

        vm.cannotEditDateDebut = ko.computed(function () {
            return vm.isReadonly || vm.AvenantInitial() != vm.numeroAvenant();
        });

        vm.isDateFinImplied = ko.computed(function () {
            // repeat !vm.enterDateFin() to imply bond with enterDuree
            return !vm.isReadonly && (!vm.enterDateFin() && vm.enterDuree() || !vm.enterDateFin() && !vm.enterDuree());
        });

        vm.isDateFinEntered = ko.computed(function () {
            return !vm.isReadonly && vm.enterDateFin();
        });

        vm.isDureeIgnored = ko.computed(function () {
            return vm.isReadonly || vm.enterDateFin() || !vm.enterDuree();
        });

        vm.isObligatoire = ko.computed(function () {
            return vm.CodeCaractere() == "O";
        });

        vm.showHideDetailsValeurs = function () {
            var $div = $("#infosValeursGarantie");
            common.autonumeric.applyAll('init', 'decimal', '', null, null, 9999999999999.99, -9999999999999.99);
            common.autonumeric.applyAll('init', 'pourcentdecimal', '');
            common.autonumeric.applyAll('init', 'pourmilledecimal', '');
            $div.dialog({
                modal: true,
                resizable: false,
                position: { my: "center", at: "center", of: $(affaire.formule.detailsGarantie.componentName) },
                minWidth: 430
            });
        };

        vm.closeDetailsValeurs = function () {
            $("#infosValeursGarantie").dialog("close");
        };

        vm.init = function () {
            common.page.isLoading = true;
            common.$postJson(
                "/FormuleGarantie/GetDetailsGarantie",
                {
                    garantieId: {
                        Affaire: currentAffair,
                        NumOption: 1,
                        NumFormule: parameters.numFormule,
                        CodeBloc: parameters.codeBloc,
                        Sequence: parameters.id,
                        IsReadonly: parameters.isReadonly,
                        IsHisto: parameters.isHisto
                    },
                    filtre: { CodeCible: parameters.cible, CodeBranche: parameters.branche },
                    dateAvenant: vm.dateAvenant()
                })
                .done(function (data) {
                    ko.mapping.fromJS(data, mappings, vm);
                    vm.initDatesEdit();
                    common.page.isLoading = false;
                    ko.tasks.schedule(function () { vm.isInitialized(true) });
                    $(document).trigger(customEvents.detailsGarantie.loaded);
                })
                .fail(function (x) {
                    common.error.show(x);
                    $(document).trigger(customEvents.detailsGarantie.error);
                });
        };

        vm.initDatesEdit = function () {
            var frdd = new FrDate(vm.DateDebut());
            vm.frDateDebut(frdd);
            vm.heuresDebut(frdd.hours);
            vm.minutesDebut(frdd.minutes);
            var frdf = new FrDate(vm.DateFin());
            vm.frDateFin(frdf);
            if (frdf.isValid) {
                vm.heuresFin(frdf.hours);
                vm.minutesFin(frdf.minutes);
            }
            if (vm.dateFin()) {
                vm.enterDateFin(true);
            }
            else if (vm.Duree()) {
                vm.enterDuree(true);
            }

            vm.frDateDebut.subscribe(function (dt) {
                if (dt && dt.toDateString()) {
                    vm.DateDebut(dt.toDateString() + " " + dt.toTimeString());
                }
                else {
                    vm.DateDebut("");
                }
            });
            vm.frDateFin.subscribe(function (dt) {
                if (!vm.enterDateFin()) {
                    vm.DateFin("");
                }
                else {
                    if (dt && dt.toDateString()) {
                        vm.DateFin(dt.toDateString() + " " + dt.toTimeString());
                    }
                    else {
                        vm.DateFin("");
                    }
                }
            });
        };

        vm.validateEdit = function () {
            var checkResult = ko.extenders.required.checkValidity(vm);
            if (!checkResult) {
                common.error.showMessage(common.error.messages.requiredFields);
                return false;
            }
            else if (typeof checkResult === "string") {
                common.error.showMessage(checkResult);
                return false;
            }

            common.page.isLoading = true;
            var details = ko.mapping.toJS(vm);
            details.CodeBloc = parameters.codeBloc;
            common.$postJson("/FormuleGarantie/SaveDetailsGarantie", {
                affaire: window.currentAffair,
                numOption: 1,
                numFormule: parameters.numFormule,
                details: details
            }, false).done(function (data) {
                $(document).trigger(customEvents.detailsGarantie.validatedEdit);
                common.page.isLoading = false;
            }).fail(function (x, s, e) {
                try {
                    var error = JSON.parse(x.responseText);
                    if (error.$type.indexOf("Business") >= 0) {
                        vm.errors.removeAll();
                        if ($.isArray(error.Errors)) {
                            ko.utils.arrayPushAll(vm.errors, error.Errors);
                        }
                        else {
                            vm.errors.push({ Error: error.Message });
                        }

                        common.error.showMessage("Erreur :\n" + vm.errors().map(function (x) { return x.Error; }).join("\n"));
                    } else {
                        common.error.showErrorJson(error);
                    }
                } catch (err) { common.error.showMessage(e);; }

                $(document).trigger(customEvents.detailsGarantie.error);
            });
        };
        vm.frDateDebut.subscribe(function () {
            var val = vm.errors().filter(function (e) { return e.FieldName == "DateDebut"; })[0];
            if (val) { vm.errors.remove(val); }
        });

        vm.frDateFin.subscribe(clearErrFin);
        vm.enterDateFin.subscribe(clearErrFin);

        function clearErrFin() {
            var val = vm.errors().filter(function (e) { return e.FieldName == "DateFin"; })[0];
            if (val) { vm.errors.remove(val); }
        }
        vm.isDateDebutInvalid = ko.pureComputed(function () {
            return vm.errors().some(function (e) { return e.FieldName == "DateDebut"; });
        });
        vm.isDateFinInvalid = ko.pureComputed(function () {
            return vm.errors().some(function (e) { return e.FieldName == "DateFin"; });
        });
        vm.isChampDateFinInvalid = ko.pureComputed(function () {
            return vm.enterDateFin() && vm.isDateFinInvalid();
        });
        vm.isChampDureeInvalid = ko.pureComputed(function () {
            return vm.IsDuree() && vm.isDateFinInvalid();
        });

        vm.cancelEdit = function () {
            $(document).trigger(customEvents.detailsGarantie.cancelledEdit);
        };

        $(document).trigger(customEvents.detailsGarantie.initializing);
        vm.init();
    };

    DetailsGarantieViewModel.prototype.dispose = function () {
        
    };

    affaire.formule.DetailsGarantieViewModel = DetailsGarantieViewModel;
})();
