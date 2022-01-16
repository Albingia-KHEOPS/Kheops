
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
                    tauxAppelRetenu: options.data.TauxAppelRetenu
                });
                obj.hasValueChanged = function () {
                    return obj.mask().cotisationPeriode != obj.CotisationPeriode()
                        || obj.mask().tauxAppelRetenu != obj.TauxAppelRetenu();
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
                return false;
            });
            vm.canCompute = ko.computed(function () {
                return false;
            }, vm);

            vm.canGoNext = ko.computed(function () {
                return true;
            }, vm);

            vm.compute = function () {
                if (vm.isReadonly()) return;
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
                    });
            };

            vm.refresh = function (data) {
               
                if (vm.isReadonly()) return;
                if (data.message) {
                    w.showError(data.message);
                }
                else {
                    vm.computeDone(true);
                    w.regul.nextStep(w.context);
                }
            };

            vm.back = function () {
                // do not redirect when step is an old page type
                if (vm.previousStepContext.Step == 2) {
                    //w.regul.previousStep(w.context);
                    DeverouillerUserOffres($("#tabGuid").val());
                    RedirectToAction("Index", "RechercheSaisie");
                }
                else {
                    if (vm.previousStepContext.Step == 8 && !vm.isReadonly()) {
                        var initTaux = $("#InitialTauxAppelRetenu").val();
                        var initCotis = $("#InitialCotisationPeriode").val();
                        vm.donneesRegul.TauxAppelRetenu(initTaux);
                        vm.donneesRegul.CotisationPeriode(initCotis);

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

                    w.ShowLoading();
                    w.regul.goToStep(vm.previousStepContext);
                }
            };

            vm.forward = function () {
                w.ShowLoading();
                if (vm.canGoNext()) {
                    if (vm.isReadonly()) {
                        w.regul.nextStep(w.context);
                    }
                    else {
                        vm.compute();                    
                    }
                    
                }
            };

            if (!vm.isReadonly()) {
                vm.preCompute = function (value) {
                    if (w.computeTimeout) w.clearTimeout(w.computeTimeout);
                    w.computeTimeout = w.setTimeout(function () {
                        var obj = ko.mapping.toJS(vm.donneesRegul);
                        obj.RgId = w.context.RgId;
                        obj.RsqId = w.context.RsqId;
                        obj.Ristourne = vm.infosFormule.Ristourne();
                        w.common.$postJson("/Regularisation/ComputeCotisations", obj)
                            .done(function (result) {
                                vm.donneesRegul.MontantCalcule(result.MontantCalcule);
                                vm.donneesRegul.MontantAffiche(result.MontantAffiche);
                                vm.donneesRegul.LibelleMontant(result.LibelleMontant);
                                vm.donneesRegul.TauxAppelRetenu(result.TauxAppelRetenu); 
                                vm.donneesRegul.ReguleMode(result.ReguleMode);
                                vm.donneesRegul.SigneMontant(result.SigneMontant);
                            })
                            .fail(function (x, s, e) {

                            });
                    }, 250);
                };

                vm.donneesRegul.CotisationPeriode.subscribe(vm.preCompute);
                vm.donneesRegul.TauxAppelRetenu.subscribe(vm.preCompute);
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
            this.updateMappings(data.donneesRegul, "donneesRegul", {});

            w.viewModel.donneesRegul.mask({
                cotisationPeriode: data.donneesRegul.CotisationPeriode,
                tauxAppelRetenu: data.donneesRegul.TauxAppelRetenu,
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
