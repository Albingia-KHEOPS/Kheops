
(function () {
    let viewModel = function (parameters) {
        let Regularisation = function (options) {
            if (options.data.PeriodeDebut != null && options.data.PeriodeDebut.length > 10) {
                options.data.PeriodeDebut = options.data.PeriodeDebut.trim().substr(0, 10);
            }
            if (options.data.PeriodeFin != null && options.data.PeriodeFin.length > 10) {
                options.data.PeriodeFin = options.data.PeriodeFin.trim().substr(0, 10);
            }
            if (options.data.DateSituation != null && options.data.DateSituation.length > 10) {
                options.data.DateSituation = options.data.DateSituation.trim().substr(0, 10);
            }
            let regularisation = ko.mapping.fromJS(options.data, {}, this);

        };
        let mapRegularisation = {
            create: function (options) {
                return new Regularisation(options);
            }
        };

        ko.mapping.fromJS(window.modelPbs, { "Regularisations": mapRegularisation }, this);
        let vm = this;

        this.canAddPB = ko.computed(function () {
            return vm.CanCreatePB() && !vm.Regularisations().some(function (r) {
                return r.CodeSituation() == "A" && r.CodeEtat() != "V";
            });
        });

        this.create = function () {
            vm.consultOrEdit(null, 'CREATE')
        };

        this.consult = function (data) {
            vm.consultOrEdit(data, 'CONSULT')
        };

        this.edit = function (data) {
            vm.consultOrEdit(data, 'UPDATE')
        };

        this.consultOrEdit = function (data, modeAvt) {
            let params = common.albParam.buildObject();

            if (["CONSULT", "UPDATE"].indexOf(modeAvt) >= 0 && params.AVNMODE == "CREATE") {
                params.AVNMODE = modeAvt;
            }
            else if (modeAvt === "CREATE") {
                params.AVNMODE = modeAvt;
                params.REGULEID = 0;
            }
            if (!params.AVNMODE) {
                params.AVNMODE = modeAvt;
            }

            params.REGULEID = data != null ? data.NumRegule() : ($("#ReguleId").val() ? $("#ReguleId").val() : 0);
            params.REGULMOD = data != null ? data.RegulMode() : "PB";
            params.REGULTYP = data != null ? data.RegulType() : "S";
            params.REGULNIV = data != null ? data.RegulNiv() : "R";
            params.REGULAVN = data != null ? data.RegulAvn() : "O";
            params.AVNID = data != null ? (data.CodeAvn() || "").toString() || $("#NumAvenantPage").val() : $("#NumAvenantPage").val();
            params.AVNIDEXTERNE = params.AVNID;
            let typeAvt = data != null ? data.CodeTraitement() : "S";
            if (typeAvt == "M" || typeAvt == "AVNRG") {
                params.AVNTYPE = "AVNRG";
            }
            else if (typeAvt == "S" || typeAvt == "REGUL") {
                params.AVNTYPE = "REGUL";
            }

            let isConsulting = params.AVNMODE === "CONSULT";
            common.page.isLoading = true;

            try {
                let stepContext = {
                    TabGuid: infosTab.tabGuid,
                    ModeNavig: params.AVNMODE === "UPDATE" ? "S" : $("#ModeNavig").val(),
                    Folder: { CodeOffre: $("#Offre_CodeOffre").val(), Version: $("#Offre_Version").val(), Type: "P", NumeroAvenant: params.AVNID },
                    Target: isConsulting ? "ConsulterPB" : "EditionPB",
                    IsReadonlyTarget: isConsulting,
                    IsModifHorsAvenant: false,
                    NewProcessCode: params.REGULEID == 0 ? params.AVNTYPE : "",
                    AvnParams: params.REGULEID == 0 ? Object.getOwnPropertyNames(params).map(function (x) { return { Key: x, Value: params[x] }; }) : []
                };

                common.$postJson("/Redirection/Auto", stepContext, true).then(function (result) {
                    if (result.url) {
                        if (!!stepContext.IsReadonlyTarget !== !!result.context.IsReadonlyTarget) {
                            common.page.isLoading = false;
                            let message = "Impossible de modifier le contrat numéro <b>" + $("#Offre_CodeOffre").val() + "</b>"
                                + " car il est actuellement en cours de modification par l'utilisateur <b>" + result.context.LockingUser + "</b>"
                                + ". Voulez-vous l'ouvrir en consultation ?";

                            $("#centerOffreVerrouillee").html(message);
                            $("#MessageOffreVerrouillee").show();

                            // Traitement du bouton "Consulter"
                            $("#btnOVReadOnly").kclick(function () {
                                $("#OffreReadOnly").val("True");
                                let data = {
                                    NumRegule: ko.computed(function () {
                                        return $("[name='consulter-regule']").last().attr("albreguleid")
                                    }),
                                    RegulMode: ko.computed(function () {
                                        return $("[name='consulter-regule']").last().data("regulmode")
                                    }),
                                    RegulType: ko.computed(function () {
                                        return $("[name='consulter-regule']").last().data("regultype")
                                    }),
                                    RegulNiv: ko.computed(function () {
                                        return $("[name='consulter-regule']").last().data("regulniv")
                                    }),
                                    RegulAvn: ko.computed(function () {
                                        return $("[name='consulter-regule']").last().data("regulavn")
                                    }),
                                    CodeAvn: ko.computed(function () {
                                        return $("[name='consulter-regule']").last().attr("albcodeavn")
                                    }),
                                    CodeTraitement: ko.computed(function () {
                                        return $("[name='consulter-regule']").last().attr("albreguletype")
                                    })
                                }
                                vm.consult(data);
                            });
                            return;
                        }
                        else {
                            document.location.replace(result.url);
                        }
                    }
                    else {
                        common.page.isLoading = false;
                        switch (result.context.DenyReason.toLowerCase()) {
                            case "citrix":
                                common.dialog.error("Veuillez ouvrir cette affaire sous Citrix.");
                                break;
                            default:
                                common.dialog.error(result.context.DenyReason);
                        }
                    }
                });
            }
            catch (e) {
                console.error(e);
                common.error.showMessage(e);
            }
        };

    };
    pbs.ListViewModel = viewModel;
})();
