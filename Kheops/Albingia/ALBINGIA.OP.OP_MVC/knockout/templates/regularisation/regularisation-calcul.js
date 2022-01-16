
// execution anonyme
(function (w) {
    if (!w.ko) {
        return;
    }

    w.loadTemplates = function (onLoaded) {
        var tmps = $("[name='ko-template']")
        var nb = tmps.length;
        if (nb == 0 && $.isFunction(onLoaded)) {
            onLoaded();
            return;
        }

        for (var i = 0; i < tmps.length; i++) {
            $(tmps[i]).load($(tmps[i]).data("ko-url"), function () {
                if (--nb == 0 && $.isFunction(onLoaded)) {
                    onLoaded();
                }
            });
        }
    };

    w.showError = function (messages) {
        if (messages) {
            w.common.error.showMessage($.isArray(messages) ? messages.join('\n') : messages);
        }
    };

    w.regul.calcul = {
        createViewModel: function () {
            w.viewModel = {
                infosContrat: null,
                infosFormule: null,
                donneesRegul: null,
                computeDone: null,
                validationDone: null,
                showCompute: null,
                canCompute: null,
                canGoNext: null,
                preCompute: null,
                compute: null,
                refresh: null,
                back: null,
                forward: null,
                previousStepContext: null,
                isReadonly: null
            };
        },

        mappings: {
            create: function (options) {
                var obj = ko.mapping.fromJS(options.data, {}, this);
                obj.mask = ko.observable({
                    cotisationPeriode: options.data.CotisationPeriode,
                    cotisationsRetenues: options.data.CotisationsRetenues,
                    montantRistourneAnticipee: options.data.MontantRistourneAnticipee,
                    ristourneAnticipee: options.data.RistourneAnticipee,
                    montantComptant: options.data.MontantComptant,
                    isAnticipee: options.data.IsAnticipee,
                    chargementSinistres: options.data.ChargementSinistres, 
                    tauxAppel: options.data.TauxAppel,
                    primeCalculee: options.data.PrimeCalculee
                });
                obj.hasValueChanged = function () {                    
                    return obj.mask().cotisationPeriode != obj.CotisationPeriode()
                        || obj.mask().montantRistourneAnticipee != obj.MontantRistourneAnticipee()
                        || obj.mask().chargementSinistres != obj.ChargementSinistres();
                };

                return obj;
            }
        },

        initMappings: function (obj, dataname, mapping) {
            if (w.viewModel && obj) {
                w.viewModel[dataname] = $.isPlainObject(obj) ? ko.mapping.fromJS(obj, mapping) : ko.mapping.fromJSON(obj, mapping);
            }
        },

        updateMappings: function (obj, dataname, mapping) {
            if (w.viewModel && obj) {
                $.isPlainObject(obj) ? ko.mapping.fromJS(obj, mapping, w.viewModel[dataname]) : ko.mapping.fromJSON(obj, mapping, w.viewModel[dataname]);
            }
        },

        initialize: function (next) {
            var scope = this;
            common.$postJson("/Regularisation/GetRegulDataForCalculation", w.context)
                .done(function (data) {
                    scope.mapEntities(data);
                    next();
                })
                .fail(function (x, s, e) {
                    console.error(x);
                    w.showError(e);
                });
        },

        initViewModel: function () {
            if (!w.viewModel) {
                return;
            }

            var vm = w.viewModel;
            vm.isReadonly = ko.observable(w.context.IsReadOnlyMode);
            vm.computeDone = ko.observable(null);
            vm.firstLoad = true;
            vm.validationDone = ko.observable(w.context.ValidationDone ? true : null);
            vm.computeDone.subscribe(function (done) { w.context.ComputeDone = done; });
            
            vm.showCompute = ko.computed(function () {
                return !vm.isReadonly();
            });
            vm.canCompute = ko.computed(function () {
                return !vm.isReadonly()
                    && this.donneesRegul
                    && (this.donneesRegul.CotisationPeriode() !== "")
                    && (this.donneesRegul.ChargementSinistres() !== "")
                    && (this.donneesRegul.MontantRistourneAnticipee() !== "")
                    && (this.donneesRegul.hasValueChanged() || this.computeDone() === false);
            }, vm);

            vm.canGoNext = ko.computed(function () {
                if (this.donneesRegul.hasValueChanged()) return false;
                return this.firstLoad && this.validationDone() || this.donneesRegul.Etat()
                    || !this.donneesRegul.hasValueChanged() && this.computeDone() === true;
            }, vm);

            vm.compute = function () {
                if (vm.isReadonly()) return;
                if (vm.canCompute()) {
                    ShowLoading();
                    vm.computeDone(false);
                    common.$postJson("/Regularisation/Compute", { context: w.context, figures: ko.mapping.toJS(vm.donneesRegul) })
                        .done(function (data) {
                            if (vm.firstLoad) {
                                vm.firstLoad = false;
                            }
                            vm.refresh(data);
                        }).fail(function (x, s, e) {
                            console.error(x);
                            w.showError(e);
                        }).always(function () {
                            CloseLoading();
                        });
                }
            };

            vm.refresh = function (data) {
                if (vm.isReadonly()) return;
                if (data.message) {
                    w.showError(data.message);
                }
                else {
                    common.$postJson("/Regularisation/GetRegulDataForCalculation", w.context)
                        .done(function (data) {
                            regul.calcul.remapEntities(data);
                        })
                        .fail(function (x, s, e) {
                            console.error(x);
                            w.showError(e);
                        })
                        .always(function () {
                            vm.computeDone(true);
                        });
                }
            };

            vm.back = function () {
                // do not redirect when step is an old page type
                if (vm.previousStepContext.Step == 2) {
                    w.regul.previousStep(w.context);
                }
                else {
                    w.ShowLoading();
                    w.regul.goToStep(vm.previousStepContext);
                }
            };

            vm.forward = function () {
                if (vm.canGoNext()) {
                    w.regul.nextStep(w.context);
                }
            };

            if (!vm.isReadonly()) {
                vm.preCompute = function (value) {
                    if (w.computeTimeout) w.clearTimeout(w.computeTimeout);
                    w.computeTimeout = w.setTimeout(function () { 
                        var obj = ko.mapping.toJS(vm.donneesRegul);
                        obj.IdContrat = w.context.IdContrat.CodeOffre;
                        obj.RgId = w.context.RgId;
                        obj.RsqId = w.context.RsqId;
                        obj.Ristourne = vm.infosFormule.Ristourne();
                        obj.PrcSeuilSP = vm.infosFormule.PrcSeuilSP();
                        w.common.$postJson("/Regularisation/ComputeCotisations", obj)
                            .done(function (result) {
                                vm.donneesRegul.CotisationsRetenues(result.CotisationsRetenues);
                                vm.donneesRegul.MontantCalcule(result.MontantCalcule);
                                vm.donneesRegul.MontantAffiche(result.MontantAffiche);
                                vm.donneesRegul.LibelleMontant(result.LibelleMontant);
                                vm.donneesRegul.SigneMontant(result.SigneMontant);
                                vm.donneesRegul.ReguleMode(result.ReguleMode);
                                vm.donneesRegul.MontantComptant(result.MontantComptant);
                                vm.donneesRegul.MontantRistourneAnticipee(result.MontantRistourneAnticipee);
                                vm.donneesRegul.IsAnticipee(result.IsAnticipee);
                            })
                            .fail(function (x, s, e) {

                            });
                    }, 250);
                };

                vm.donneesRegul.CotisationPeriode.subscribe(vm.preCompute);
                vm.donneesRegul.ChargementSinistres.subscribe(vm.preCompute);               
            }

            // predefine which previous step is available
            w.regul.previousStep(w.context, true, function (data) {
                w.CloseLoading();
                vm.previousStepContext = data;
            });
        },

        mapEntities: function (data) {
            if (!w.viewModel) {
                return;
            }

            this.initMappings(data.infosContrat, "infosContrat", {});
            this.initMappings(data.infosFormule, "infosFormule", {});
            this.initMappings(data.donneesRegul, "donneesRegul", regul.calcul.mappings);

            this.initViewModel();
        },

        remapEntities: function (data) {
            if (!w.viewModel || w.viewModel.isReadonly()) {
                return;
            }

            this.updateMappings(data.infosContrat, "infosContrat", {});
            this.updateMappings(data.infosFormule, "infosFormule", {});
            this.updateMappings(data.donneesRegul, "donneesRegul", {});

            w.viewModel.donneesRegul.mask({
                cotisationPeriode: data.donneesRegul.CotisationPeriode,
                cotisationsRetenues: data.donneesRegul.CotisationsRetenues,
                montantRistourneAnticipee: data.donneesRegul.MontantRistourneAnticipee,
                ristourneAnticipee : data.donneesRegul.RistourneAnticipee,
                chargementSinistres: data.donneesRegul.ChargementSinistres,
                montantComptant: data.MontantComptant,
                isAnticipee: data.IsAnticipee,
                primeCalculee: data.PrimeCalculee,
                tauxAppel: data.TauxAppel,
                reguleMode: data.donneesRegul.ReguleMode
            });
        },

        applyBindings: function () {
            if (!w.viewModel) {
                return;
            }

            try {
                ko.applyBindings(w.viewModel, $("#regul_data")[0]);
            }
            catch (e) {
                console.error(e);
            }
        },
        
        start: function () {
            var scope = this;
            scope.createViewModel();
            scope.initialize(function () {
                scope.applyBindings();
            });
        }
    };

    w.regul.calcul.start.bind(w.regul.calcul)();

})(window);
