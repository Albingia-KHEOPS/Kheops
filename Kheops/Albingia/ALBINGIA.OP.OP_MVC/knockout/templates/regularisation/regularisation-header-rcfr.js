
(function () {

    if (!ko) {
        return;
    }

    window.viewModel = null;
    regul.headerRC = {
        createViewModel: function () {
            viewModel = {
                infosGlobal: null
            };
        },

        mappings: {
            "infosGlobal": {
                create: function (options) {
                    var obj = ko.mapping.fromJS(options.data, {}, this);
                    obj.title = ko.computed(function () {
                        return "Régularisation RCFR " + obj.Libelle() + " " + common.dateIntToStr(obj.DateDebRegul()) + " au " + common.dateIntToStr(obj.DateFinRegul()) + " :";
                    });

                    obj.debutPeriode = ko.computed(function () {
                        return common.dateIntToStr(obj.DateDeb());
                    });

                    obj.finPeriode = ko.computed(function () {
                        return common.dateIntToStr(obj.DateFin());
                    });

                    obj.codeTaxes = ko.computed(function () {
                        return obj.CodeTaxe() + " - " + obj.LibTaxe();
                    });
                    
                    obj.formule = ko.computed(function () {
                        return obj.LettreFor() + " - " + obj.LibFor();
                    });

                    obj.risque = ko.computed(function () {
                        return obj.CodeRsq() + " - " + obj.LibRsq();
                    });

                    obj.regimeTaxe = ko.computed(function () {
                        return obj.CodeRgt() + " - " + obj.LibRgt();
                    });

                    obj.regulType = ko.computed(function () {
                        if (obj.TypeReguleGar() && obj.LibReguleGar()) {
                            return obj.TypeReguleGar() + " - " + obj.LibReguleGar();
                        }

                        return obj.TypeReguleGar();
                    });

                    obj.showCotisProAquis = ko.computed(function () {
                        return obj.HasCotisationPro() || obj.HasCotisationAquis();
                    });

                    obj.showAppliqueRisque = function (obj, event) {
                        var position = $(event.target).offset();
                        $("#divAppliqueA").css({ 'position': 'absolute', 'top': position.top + 25 + 'px', 'left': position.left - 410 + 'px' });
                        $("#divAppliqueA").toggle();
                    };

                    obj.hideAppliqueRisque = function () {
                        $("#divAppliqueA").toggle();
                    };

                    obj.txAppelPrc = ko.computed(function () {
                        if (obj.TxAppel() === 0) {
                            return "";
                        }
                        else {
                            return obj.TxAppel();
                        }
                    });

                    return obj;
                }
            }
        },

        initMappings: function (obj, mapping) {
            if (viewModel && obj) {
                viewModel = $.isPlainObject(obj) ? ko.mapping.fromJS(obj, mapping) : ko.mapping.fromJSON(obj, mapping);
            }
        },

        updateMappings: function (obj, mapping) {
            if (viewModel && obj) {
                $.isPlainObject(obj) ? ko.mapping.fromJS(obj, mapping, viewModel) : ko.mapping.fromJSON(obj, mapping, viewModel);
            }
        },

        initialize: function (next) {
            var scope = this;
            if (window.context) {
                common.$postJson("/Regularisation/GetHeaderGarantiesRC", window.context)
                    .done(function (data) {
                        var requestObj = {
                            contrat: window.context.IdContrat,
                            numeroAvt: (window.context.ModeleAvtRegul != null) ? window.context.ModeleAvtRegul.NumAvt : window.context.KeyValues[3],
                            codeFormule: (data && data.infosGlobal) ? data.infosGlobal.CodeFormule : 0
                        };
                        common.$postJson("/Regularisation/GetRisqueApplique", requestObj).done(function (rsq) {
                            data.infosGlobal.risqueApplique = rsq;
                            scope.mapEntities(data);
                            next();
                        });

                    })
                    .fail(function (x, s, e) {
                        console.error(x);
                        common.error.show(x);
                    });
            }
            
        },

        mapEntities: function (data) {
            if (!viewModel) {
                return;
            }

            this.initMappings(data, regul.headerRC.mappings);
        },

        remapEntities: function (data) {
            if (!viewModel) {
                return;
            }

            this.updateMappings(data, regul.headerRC.mappings);
        },

        applyBindings: function () {
            if (!viewModel) {
                return;
            }

            try {
                ko.applyBindings(viewModel, $("#infos_global")[0]);
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

    regul.headerRC.start.bind(regul.headerRC)();

})();
