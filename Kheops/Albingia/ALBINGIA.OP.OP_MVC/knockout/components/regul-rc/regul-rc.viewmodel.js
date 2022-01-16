(function () {
    let RegulRCViewModel = function (parameters) {
        var vm = this;
        var mappings = {
            list: {
                "Garanties": {
                    create: function (options) {
                        return new mappings.garantie(options.data);
                    }
                }
            },
            createMask: function (data) {
                return {
                    Assiette: data.Definitif.BasicValues.Assiette,
                    TauxMontant: data.Definitif.BasicValues.TauxMontant,
                    Unite: data.Definitif.BasicValues.Unite.Code,
                    CodeTaxes: data.Definitif.BasicValues.CodeTaxes.Code,
                    CotisationForcee: data.CotisationForcee,
                    TaxesCotisationForcee: data.TaxesCotisationForcee,
                    RegulCalcule: data.RegulCalcule,
                    RegulForcee: data.RegulForcee,
                    IsRegulZero: data.IsRegulZero,
                    IsReadOnly: data.IsReadOnlyRCUSA,
                    Coefficient: data.Definitif.Coefficient
                };
            },
            garantie: function (data) {
                ko.mapping.fromJS(data, {}, this);

                this.mask = ko.observable(mappings.createMask(data));
                this.isReadonly = this.IsReadOnly();
                this.isReguleZeroReadonly = ko.computed(function () {
                    return this.IsReadOnly() || this.IsRegulZero();
                }, this);
                this.Definitif.BasicValues.Unite.Code.subscribe(function (val) {
                    this.Definitif.BasicValues.TauxMontant(val == "D" ? this.montant() : this.taux());
                    vm.autoRefresh();
                }, this);

                this.Definitif.BasicValues.Assiette.subscribe(vm.autoRefresh);
                this.Definitif.BasicValues.CodeTaxes.Code.subscribe(vm.autoRefresh);

                this.isEmpty = ko.pureComputed(function () {
                    return (!this.Definitif.BasicValues.TauxMontant() || isNaN(this.Definitif.BasicValues.TauxMontant()))
                        && (!this.Definitif.BasicValues.Unite || !this.Definitif.BasicValues.Unite.Code());
                }, this);

                this.isInvalid = ko.pureComputed(function () {
                    return !this.isEmpty()
                        && (!this.Definitif.BasicValues.TauxMontant()
                            || !this.Definitif.BasicValues.Unite.Code()
                            || (!this.Definitif.BasicValues.CodeTaxes.Code() && this.Definitif.BasicValues.Unite.Code()));
                }, this);

                this.showCoef = ko.pureComputed(function () {
                    return this.Definitif.BasicValues.IsAuto();
                }, this);

                this.cannotEditAttentat = ko.pureComputed(function () {
                    return this.isReadonly || this.MotifInferieur() == 1;
                }, this);

                this.cannotEditCotisForcee = ko.pureComputed(function () {
                    return this.cannotEditAttentat();
                }, this);

                this.cannotEditCoef = ko.pureComputed(function () {
                    return this.cannotEditAttentat();
                }, this);

                this.isCurrencyAmount = ko.pureComputed(function () {
                    return this.Definitif.BasicValues.Unite.Code() == "D";
                }, this);

                this.isRateAmount = ko.pureComputed(function () {
                    return this.Definitif.BasicValues.Unite.Code() != "D";
                }, this);

                this.hasValueChanged = ko.pureComputed(function () {
                    return this.mask().Assiette != this.Definitif.BasicValues.Assiette()
                        || (this.mask().TauxMontant != this.taux() && this.isRateAmount())
                        || (this.mask().TauxMontant != this.montant() && this.isCurrencyAmount())
                        || this.mask().Unite != this.Definitif.BasicValues.Unite.Code()
                        || this.mask().CodeTaxes != this.Definitif.BasicValues.CodeTaxes.Code()
                        || this.mask().CotisationForcee != this.CotisationForcee()
                        || this.mask().TaxesCotisationForcee != this.TaxesCotisationForcee()
                        || this.mask().RegulForcee != this.RegulForcee()
                        || this.mask().IsRegulZero != this.IsRegulZero()
                        || this.mask().Coefficient != this.Definitif.Coefficient();
                }, this);

                this.montantPrev = ko.observable(0);
                this.tauxPrev = ko.observable(0);
                if (data.Previsionnel.BasicValues.Unite.Code == "D") {
                    this.montantPrev(data.Previsionnel.BasicValues.TauxMontant);
                }
                else {
                    this.tauxPrev(data.Previsionnel.BasicValues.TauxMontant);
                }

                this.montant = ko.observable(0);
                this.taux = ko.observable(0);
                if (data.Definitif.BasicValues.Unite.Code == "D") {
                    this.montant(data.Definitif.BasicValues.TauxMontant);
                }
                else {
                    this.taux(data.Definitif.BasicValues.TauxMontant);
                }

                this.taux.subscribe(function (val) {
                    this.Definitif.BasicValues.TauxMontant(val);
                    vm.autoRefresh();
                }, this);

                this.montant.subscribe(function (val) {
                    this.Definitif.BasicValues.TauxMontant(val);
                    vm.autoRefresh();
                }, this);

                this.allowForceZero = ko.computed(function () {
                    return !this.isReadonly && !this.IsZeroLocked();
                }, this);

                if (!this.IsZeroLocked()) {
                    this.switchRegulZero = function (forceZero) {
                        if (forceZero) {
                            this.RegulForcee(0);
                        }
                    }.bind(this);

                    this.IsRegulZero.subscribe(this.switchRegulZero);
                }

                this.isInvalidEntry = function (val) {
                    return !vm.IsReadOnly() && !val;
                };
            }
        };

        this.Garanties = ko.observableArray([]);
        this.TotalAmount = ko.observable(0);
        this.Attentat = ko.observable(0);
        this.FirstAccess = ko.observable(0);
        this.MotifInferieur = ko.observable(0);
        this.ListeUnites = ko.observableArray([]);
        this.ListeCodesTaxes = ko.observableArray([]);
        this.IsReadOnly = ko.observable(window.context ? window.context.IsReadOnlyMode : true);
        this.isAutoRefresh = ko.observable(false);
        this.IsSimplifiedRegule = ko.observable(0);
        this.hasComputed = ko.observable(false);
        this.isSimplifiedReadOnly = ko.computed(function () {
            return vm.IsSimplifiedRegule() && vm.IsReadOnly();
        });

        vm.resetMask = function (list) {
            if (vm.Garanties && vm.Garanties() && vm.Garanties().length > 0) {
                vm.Garanties().forEach(function (x) {
                    x.mask(mappings.createMask(list.filter(function (y) {
                        if (y.Definitif.Id == 0)
                            return y.Definitif.CodeGarantie == x.Definitif.CodeGarantie();
                        return y.Definitif.Id == x.Definitif.Id();
                    })[0]));
                });
            }
        };

        vm.calculationErrors = function () {
            var errors = [];
            if (vm.Garanties && vm.Garanties() && vm.Garanties().length > 0) {
                vm.Garanties().forEach(function (x) { errors.push(x.Definitif.CalculError()); });
                return errors.filter(function (x) { return !!x; }).join("\n");
            }

            return "";
        };

        vm.refreshTimeout = null;
        vm.autoRefresh = function () {
            if (!vm.isAutoRefresh()) return;

            if (vm.refreshTimeout) {
                clearTimeout(vm.refreshTimeout);
            }

            vm.refreshTimeout = setTimeout(function () {
                vm.refresh();
            }, 500);
        };

        vm.refresh = function () {
            common.page.isLoading = true;
            common.$postJson("/Regularisation/ComputeGarantiesRC", { context: window.context, list: ko.mapping.toJS(vm) }, true).done(function (data) {
                try {
                    ko.mapping.fromJS(data, {}, vm);
                    var errors = vm.calculationErrors();
                    vm.resetMask(data.Garanties);
                    if (errors) {
                        common.error.showMessage(errors);
                    }
                    else {
                        vm.hasComputed(true);
                    }
                }
                catch (e) {
                    console.error(e);
                    common.error.showMessage(e.message);
                }

                common.page.isLoading = false;
            });
        };

        vm.init = function () {
            common.page.isLoading = true;

            if ($("#divActionButtons").length > 0) {
                try {
                    ko.applyBindings(vm, $("#divActionButtons")[0]);
                }
                catch (e) {
                    console.error(e);
                    common.error.showMessage(e.message);
                }
            }

            common.$postJson("/Regularisation/GetGarantiesRC", window.context, true).done(function (data) {
                try {
                    ko.mapping.fromJS(data, mappings.list, vm);
                }
                catch (e) {
                    console.error(e);
                    common.error.showMessage(e.message);
                }

                common.page.isLoading = false;
            });
        };

        vm.canGoNext = ko.pureComputed(function () {
            if (!vm.IsSimplifiedRegule()) {
                return !this.Garanties().some(function (g) { return g.isInvalid(); }) && !this.allowRefresh() && !this.FirstAccess();
            }
            else {
                return !this.Garanties().some(function (g) { return g.isInvalid(); }) && !this.allowRefresh();
            }
        }, vm);

        vm.forward = function (data, event, params) {
            if (vm.canGoNext()) {
                common.page.isLoading = true;
                if (vm.IsReadOnly()) {
                    return vm.goNext(params && params.returnHome);
                }

                common.$postJson("/Regularisation/ValidateGarantiesRC", { context: window.context, list: ko.mapping.toJS(vm) }, true).done(function (data) {
                    try {
                        if (data) {
                            common.error.showMessage(data);
                        }
                        else {
                            vm.goNext(params && params.returnHome);
                        }
                    }
                    catch (e) {
                        console.error(e);
                        common.error.showMessage(e.message);
                        common.page.isLoading = false;
                    }
                });
            }
        };

        this.goNext = function (returnHome) {
            if (vm.IsSimplifiedRegule()) {
                RedirectionRegul('Index', 'Quittance', returnHome);
            }
            else {
                vm.backToStep4(returnHome);
            }

            return true;
        };

        this.allowRefresh = ko.pureComputed(function () {
            var hasInvalid = this.Garanties().some(function (g) { return g.isInvalid(); });
            if (this.IsReadOnly() || hasInvalid) {
                return false;
            }
            else {
                return this.Garanties().some(function (g) { return g.hasValueChanged(); });
            }
        }, this);

        this.refreshIcon = ko.pureComputed(function () {
            return vm.IsReadOnly() ? "" : vm.allowRefresh() ? imgUrls.boutonRefresh.active : imgUrls.boutonRefresh.disabled;
        });

        this.back = function () {
            if (vm.IsReadOnly() || !vm.hasComputed()) {
                if (vm.IsSimplifiedRegule()) {
                    RedirectionRegul('Step1_ChoixPeriode', 'CreationRegularisation', false);
                }
                else {
                    vm.backToStep4();
                }
                return;
            }

            $("#btnConfirmOk").kclick(function () {
                CloseCommonFancy();
                switch ($("#hiddenAction").val()) {
                    case "Cancel":
                        if (vm.IsSimplifiedRegule()) {
                            RedirectionRegul('Step1_ChoixPeriode', 'CreationRegularisation', false);
                        }
                        else {
                            vm.backToStep4();
                        }

                        break;
                }
                $("#hiddenAction").clear();
            });
            $("#btnConfirmCancel").kclick(function () {
                CloseCommonFancy();
                $("#hiddenAction").clear();
            });

            ShowCommonFancy("Confirm", "Cancel",
                "Attention, les informations d'assiette vont être réinitialisées.<br/><br/>Confirmez-vous votre action ?<br/>",
                350, 130, true, true);
        };

        this.backToStep4 = function (returnHome) {
            let p = common.albParam.buildObject();
            delete p.REGULGARID;
            $('#AddParamValue').val(common.albParam.objectToString(p));
            RedirectionRegul('Step4_ChoixPeriodeGarantie', 'CreationRegularisation', returnHome, 'Previous');
        };

        this.displayOrNotDetailedInfoRC = function () {
            var rcInfoGlobal = document.getElementById("infos_global");
            var garantieRegulRC = document.getElementById("garantieRegulRC");;
            var divHeight = 0;
            if (rcInfoGlobal.style.display === "block" || rcInfoGlobal.style.display === "") {
                divHeight = garantieRegulRC.parentElement.parentElement.parentElement.clientHeight;
                garantieRegulRC.style.height = divHeight + "px";
                $("#imgExpandInfo").attr("src", "/Content/Images/expand.png");
                rcInfoGlobal.style.display = "none";
            } else {
                rcInfoGlobal.style.display = "block";
                divHeight = garantieRegulRC.clientHeight - rcInfoGlobal.clientHeight - 2;
                garantieRegulRC.style.height = divHeight + "px";
                $("#imgExpandInfo").attr("src", "/Content/Images/collapse.png");
            }
        };

        this.init();
    };

    regul.RegulRCViewModel = RegulRCViewModel;
})();
