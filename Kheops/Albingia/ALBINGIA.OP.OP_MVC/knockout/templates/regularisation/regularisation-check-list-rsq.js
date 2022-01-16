
// execution anonyme
(function (w) {
    if (!w.ko) {
        return;
    }

    w.regul.rsq = {
        createViewModel: function () {
            w.viewModel = {
                //infosContrat: null,
                checkListRisques: null,
                canGoNext: null,
                back: null,
                forward: null,
                isReadonly: null
            };
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
            common.$postJson("/Regularisation/GetCheckListRisques", w.context)
                .fail(function (x, s, e) {
                    console.error(e);
                })
                .done(function (data) {
                    scope.mapEntities(data);
                    next();
                });
        },

        initViewModel: function () {
            if (!w.viewModel) {
                return;
            }

            var vm = w.viewModel;

            vm.isReadonly = ko.observable(w.context.IsReadOnlyMode);

           

           

            vm.canGoNext = ko.computed(function () {
                return vm.checkListRisques.list().some(function (e) { return e.isProcessed(); });
            }, vm);

            vm.back = function () {
                regul.previousStep(w.context);
            };

            vm.forward = function () {
                if (vm.canGoNext()) {
                    regul.nextStep(w.context);
                }
            };

            vm.showError = function (messages) {
                if (messages) {
                    common.dialog.bigError($.isArray(messages) ? messages.join('\n') : messages, true);
                }
            };
        },

        mapEntities: function (data) {
            if (!w.viewModel) {
                return;
            }

            this.initMappings(data.checkListRisques, "checkListRisques", {
                "list": {
                    create: function (options) {
                        return new regul.rsq.mapRisque(options.data);
                    }
                }
            });

            this.initViewModel();
        },

        mapRisque: function (risque) {
            ko.mapping.fromJS(risque, {}, this);
            var obj = this;
            this.regulRisque = function (data, event) {              
                                
                ShowLoading();
                w.context.Step = "Regularisation";
                w.context.RsqId = data.rsqId();

                common.$postJson("/Regularisation/EnsureContext", w.context).done(function (data) {
                    regul.goToStep(data);
                });
            };

            if (w.context.IsReadOnlyMode) {
                this.changeStatus = function () { };
            }
            else {
                this.askChangeStatus = function (data) {
                    common.dialog.initConfirm(
                        function () {
                            obj.changeStatus(data);
                        },
                        function () {
                            obj.isProcessed(true);
                        },
                        "Veuillez confirmer l'annulation de cette regularisation. Attention, toutes les informations précédemment saisies seront perdues");
                };

                this.changeStatus = function (data) {
                    common.$postJson("/Regularisation/CancelRegularisationRisque", { rgId: w.context.RgId, rsqId: data.rsqId() })
                        .done(function () {
                            w.context.ValidationDone = w.viewModel.checkListRisques.list().some(function (e) { return e.isProcessed(); });
                            w.context.Scope = "Risque";
                            regul.navPane.refreshDisplay();
                        })
                        .fail(function (x, s, e) {
                            obj.isProcessed(true);
                        });
                };
            }

            this.canChange = ko.computed(function () {
                return ((!w.viewModel.isReadonly && !w.context.IsReadOnlyMode)
                        || (w.viewModel.isReadonly && !w.viewModel.isReadonly()))
                    && this.isProcessed();
            }, this);
        },

        applyBindings: function () {
            if (!w.viewModel) {
                return;
            }

            try {
                ko.applyBindings(w.viewModel, document.getElementById("listRisques")); 
                ko.applyBindings(w.viewModel, document.getElementById("navButtons"));
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

    w.regul.rsq.start.bind(w.regul.rsq)();

})(window);
