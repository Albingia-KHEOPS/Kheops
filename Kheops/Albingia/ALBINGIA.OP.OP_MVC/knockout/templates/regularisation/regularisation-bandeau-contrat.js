
// execution anonyme
(function (w) {
    if (!w.ko) {
        return;
    }

    var ribbon = {
        createViewModel: function () {
            w.viewModelBandeauContrat = {
                infosContrat: null,
                //isDateEmpty: null,
                isReadOnly: null
            };
        },

        initMappings: function (obj, dataname, mapping) {
            if (w.viewModelBandeauContrat && obj) {
                w.viewModelBandeauContrat[dataname] = $.isPlainObject(obj) ? ko.mapping.fromJS(obj, mapping) : ko.mapping.fromJSON(obj, mapping);
            }
        },

        updateMappings: function (obj, dataname, mapping) {
            if (w.viewModelBandeauContrat && obj) {
                $.isPlainObject(obj) ? ko.mapping.fromJS(obj, mapping, w.viewModelBandeauContrat[dataname]) : ko.mapping.fromJSON(obj, mapping, w.viewModelBandeauContrat[dataname]);
            }
        },

        initialize: function (next) {
            var scope = this;
            common.$postJson("/Regularisation/GetContratBandeau", (w.context ? w.context.IdContrat : w.contrat))
                .fail(function (x, s, e) {
                    console.error(e);
                })
                .done(function (data) {
                    scope.mapEntities(data);
                    next();
                });
        },

        initViewModel: function () {
            if (!w.viewModelBandeauContrat) {
                return;
            }

            var vm = w.viewModelBandeauContrat;

            vm.isReadonly = ko.observable(true);
          
            vm.showError = function (messages) {
                if (messages) {
                    common.dialog.bigError($.isArray(messages) ? messages.join('\n') : messages, true);
                }
            };

            vm.changeAttestationTabStyle = function (isCollapsed) {
                var alertAttestationTab = $(".dvAlertAttes");
                var garantieTab = $(".divGarantie");
                var rsqObjTab = $(".divRsqObj");
                if (isCollapsed) {
                    alertAttestationTab.css("height", alertAttestationTab.data("max-height") + "px");
                    garantieTab.css("max-height", garantieTab.data("max-height") + "px");
                    rsqObjTab.css("max-height", rsqObjTab.data("max-height") + "px");
                } else {
                    alertAttestationTab.css("height", alertAttestationTab.data("min-height") + "px");
                    garantieTab.css("max-height", garantieTab.data("min-height") + "px");
                    rsqObjTab.css("max-height", rsqObjTab.data("min-height") + "px");
                }
            };

            vm.infosContrat.displayOrNotDetailedInfo = function () {
                var content = document.getElementById("contentDetailedContratInfo");
                var infoContratDiv = $("#infosContrat")[0];
                var divHeight = 0;
                var isAttestation = $('div.albAttestationInfos').length != 0;
                if (content.style.display === "block") {
                    infoContratDiv.className = infoContratDiv.className.replace("divInfoContratDetailedDisplay", "divInfoContratRegularisation");
                    content.style.display = "none";
                    divHeight = infoContratDiv.parentElement.clientHeight - $("#infosContrat").outerHeight();
                    infoContratDiv.parentElement.lastElementChild.style.height = divHeight + "px";
                    $("#imgExpandInfo").attr("src", "/Content/Images/expand.png");
                    if (isAttestation) {
                        vm.changeAttestationTabStyle(true);
                    }                                   
                } else {
                    infoContratDiv.className = infoContratDiv.className.replace("divInfoContratRegularisation", "divInfoContratDetailedDisplay");
                    content.style.display = "block";
                    divHeight = infoContratDiv.parentElement.clientHeight - $("#infosContrat").outerHeight() - 12;
                    infoContratDiv.parentElement.lastElementChild.style.height = divHeight + "px";
                    $("#imgExpandInfo").attr("src", "/Content/Images/collapse.png");
                    if (isAttestation) {
                        vm.changeAttestationTabStyle(false);
                    }               
                }
            };

        },

        mapEntities: function (data) {
            if (!w.viewModelBandeauContrat) {
                return;
            }

            this.initMappings(data.infosContrat, "infosContrat", {

                CodeOffre: data.infosContrat.CodeOffre,
                Version: data.infosContrat.Version,
                TypeContrat: data.infosContrat.Type,
                DisplayTypeContrat: data.infosContrat.DisplayTypeContrat,
                Identification: data.infosContrat.Identification,
                Assure: data.infosContrat.Assure,
                Souscripteur: data.infosContrat.Souscripteur,
                DateDebutEffet: data.infosContrat.DateDebutEffet,
                DateFinEffet: data.infosContrat.DateFinEffet,
                Periodicite: data.infosContrat.Periodicite,
                Echeance: data.infosContrat.Echeance,
                NatureContrat: data.infosContrat.NatureContrat,
                Courtier: data.infosContrat.Courtier,
                RetourPiece: data.infosContrat.RetourPiece,
                Observation: data.infosContrat.Observation,
                Gestionnaire: data.infosContrat.Gestionnaire,
                ContratMere: data.infosContrat.ContratMere,
                IsLightVersion: true,
                LblDebutEffet: data.infosContrat.LblDebutEffet,
                LblFinEffet: data.infosContrat.LblFinEffet,
                displayOrNotDetailedInfo: null
            });


            this.initViewModel();
        },


        applyBindings: function () {
            if (!w.viewModelBandeauContrat) {
                return;
            }

            try {
                ko.applyBindings(w.viewModelBandeauContrat, document.getElementById("infosContrat"));
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



    if (w.regul != undefined) {
        w.regul.ribbon = ribbon;

        w.regul.ribbon.start.bind(w.regul.ribbon)();
    }
    else {
        w.ribbon = ribbon;

        w.ribbon.start.bind(w.ribbon)();
    }

})(window);
