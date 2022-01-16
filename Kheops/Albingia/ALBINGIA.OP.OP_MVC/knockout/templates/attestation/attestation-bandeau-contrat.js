
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
            common.$postJson("/CreationAttestation/GetContratBandeau", { contrat: w.context.Contrat, id: w.context.DossierCourant }, true)
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

            vm.infosContrat.initTabAttestation = function () {
                if ($("#IntegraliteContrat").is(":checked")) {
                    $("#tabRisqueObjet").hide();
                    $("#tabGarantie").hide();
                }
                else {
                    $("#tabRisqueObjet").show();
                    $("#tabGarantie").show();
                }
            }

            vm.infosContrat.displayOrNotDetailedInfo = function () {

                var content = document.getElementById("contentDetailedContratInfo");
                var infoContratDiv = $("#infosContrat")[0];
                var divHeight = 0;
                var heightAttes = 0;
                if (content.style.display === "block") {
                    infoContratDiv.className = infoContratDiv.className.replace("divInfoAttestationContratDetailedDisplay", "divInfoContratRegularisation");
                    content.style.display = "none";
                    divHeight = infoContratDiv.parentElement.clientHeight - $("#infosContrat").outerHeight();
                    infoContratDiv.parentElement.lastElementChild.style.height = divHeight + "px";
                    $("#imgExpandInfo").attr("src", "/Content/Images/expand.png");
                    $(".dvAttestion").height(divHeight - 50);
                    $(".dvAlertAttes").height(divHeight - 50);
                } else {
                    infoContratDiv.className = infoContratDiv.className.replace("divInfoContratRegularisation", "divInfoAttestationContratDetailedDisplay");
                    content.style.display = "block";
                    divHeight = infoContratDiv.parentElement.clientHeight - $("#infosContrat").outerHeight() - 12;                    
                    infoContratDiv.parentElement.lastElementChild.style.height = divHeight + "px";
                    $("#imgExpandInfo").attr("src", "/Content/Images/collapse.png");
                    $(".dvAttestion").height(divHeight - 40);
                    $(".dvAlertAttes").height(divHeight - 40);
                }
            };

        },

        mapEntities: function (data) {
            if (!w.viewModelBandeauContrat) {
                return;
            }

            this.initMappings(data.infosContrat, "infosContrat", {

                CodeContrat: data.infosContrat.CodeContrat,
                Version: data.infosContrat.Version,
                TypeContrat: data.infosContrat.Type,
                DisplayTypeContrat: data.infosContrat.DisplayTypeContrat,
                Identification: data.infosContrat.Identification,
                Assure: data.infosContrat.Assure,
                Souscripteur: data.infosContrat.Souscripteur,
                DateDebutEffet: data.infosContrat.DateDebutEffet,
                HeureDebutEffet: data.infosContrat.HeureDebutEffet,
                DateFinEffet: data.infosContrat.DateFinEffet,
                Periodicite: data.infosContrat.Periodicite,
                Echeance: data.infosContrat.Echeance,
                NatureContrat: data.infosContrat.NatureContrat,
                Apporteur: data.infosContrat.Apporteur,
                RetourPiece: data.infosContrat.RetourPiece,
                Observation: data.infosContrat.Observation,
                Gestionnaire: data.infosContrat.Gestionnaire,
                CourtierGestionnaire: data.infosContrat.CourtierGestionnaire,
                ContratMere: data.infosContrat.ContratMere,
                PartAlbingia: data.infosContrat.PartAlbingia,
                ProchainEcheance: data.infosContrat.ProchainEcheance,
                RegimeTaxe: data.infosContrat.RegimeTaxe,
                Devise: data.infosContrat.Devise,
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
    if (w.regul) {
        w.regul.ribbon = ribbon;
        w.regul.ribbon.start.bind(w.regul.ribbon)();
    }
    else {
        w.ribbon = ribbon;
        w.ribbon.start.bind(w.ribbon)();
    }

})(window);
