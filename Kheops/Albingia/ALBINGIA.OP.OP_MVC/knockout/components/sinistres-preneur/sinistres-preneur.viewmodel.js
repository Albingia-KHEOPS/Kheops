
(function () {
    let sinistresViewModel = function (parameters) {
        const baseGrpUrl = "http://grp/Albingia/XRMServices/2011/OrganizationData.svc/new_sinistreSet";
        const baseGrpGuidUrl = "http://grp/Albingia/main.aspx?etc=10012&pagetype=entityrecord";
        let QuerystringGrp = function (s, f) {
            this.$select = s;
            this.$filter = f;
            this.getQuerystring = function () {
                let a = [];
                for (var p in this) {
                    if (!$.isFunction(this[p])) {
                        a.push(p + "=" + encodeURIComponent(this[p]));
                    }
                }
                return "?" + a.join("&");
            }.bind(this);
        };
        let Sinistre = function (data) {
            let s = ko.mapping.fromJS(data, {}, this);
            this.urlGrp1 = ko.computed(function () {
                let qsGrp = new QuerystringGrp("new_sinistreId", "new_Annedesurvenance eq '" + s.Annee() + "' and new_ReferenceDossier eq '" + s.Numero() + "'");
                return baseGrpUrl + qsGrp.getQuerystring();
            });
            this.urlGrp2 = ko.observable("");
            this.getGrpXml = function () {
                $.ajax({
                    url: s.urlGrp1(),
                    dataType: "xml",
                    success: function (xmlDoc) {
                        let guid = xmlDoc.find("d:new_sinistreId").text();
                        s.urlGrp2(baseGrpGuidUrl + "&id=" + encodeURIComponent("{" + guid + "}"));
                    },
                    error: function () {
                        s.urlGrp2(null);
                    }
                });
            };
            this.navigate = function () {
                if (s.urlGrp2() === "") {
                    return;
                }
                else if (s.urlGrp2() === null) {
                    s.navigateCitrix();
                    return;
                }
                let link = document.createElement("A");
                link.target = "_blank";
                link.href = s.urlGrp2();
                link.style.display = "none";
                document.body.appendChild(link);
                link.click();
                setTimeoput(function () {
                    document.body.removeChild(link);
                }, 5);
            };
            this.numeroMetier = ko.computed(function () {
                return s.Annee() + "-" + s.Numero();
            });
            this.navigateCitrix = function () {
                window.location.href = "Albinprod:GererContrat?action=SINISTRE?type=P?ipb=" + s.Contrat.Code() + "?alx=" + s.Contrat.Version();
            };

            //ko.tasks.schedule(this.getGrpXml);
        };
        let mapping = {
            "Sinistres": {
                create: function (options) {
                    return new Sinistre(options.data);
                },
                key: function (data) {
                    return ko.utils.unwrapObservable(data.Annee)
                        + "-" + ko.utils.unwrapObservable(data.Numero)
                        + "-" + ko.utils.unwrapObservable(data.CodeSousBranche);
                }
            }
        };
        ko.mapping.fromJS({
            Sinistres: [],
            ListName: "Sinistres",
            CodePreneur: parameters.codePreneur,
            PageSize: parameters.pageSize || 50,
            MontantTotal: null
        }, {}, this);

        let vm = this;
        vm.initialized = ko.observable(false);
        vm.hasResults = ko.computed(function () {
            return (vm.Sinistres() || []).length > 0;
        });

        $(document).on(customEvents.paging.change, function (e, page, listName) {
            if (listName !== vm.ListName()) {
                return;
            }
            common.$getJson("/Sinistre/GetSinistres", { page: page, codeAssure: vm.CodePreneur() }, true).done(function (pglist) {
                ko.mapping.fromJS(pglist.List, mapping.Sinistres, vm.Sinistres);
                $.get("/Sinistre/GetTotalChargementSinistresPreneur?codePreneur=" + vm.CodePreneur()).done(function (total) {
                    vm.MontantTotal(parseFloat(total));
                }).fail(function () { vm.montantUnavailable(true); });

                $(document).trigger(customEvents.paging.dataLoaded, [listName, pglist]);
                if (!vm.initialized()) {
                    $(document).trigger(customEvents.preneurs.lists.sinistres.loaded);
                    vm.initialized(true);
                 }
            });
        });

        vm.montantAvailable = ko.computed(function () {
            return vm.MontantTotal() !== null;
        });
        vm.montantComputing = ko.computed(function () {
            return vm.MontantTotal() === null;
        });
        vm.montantUnavailable = ko.observable(false);
        $(document).trigger(customEvents.paging.initFirstPage);
    };
    sinistresViewModel.prototype.dispose = function () {
        $(document).off(customEvents.paging.change);
    };
    preneurs.sinistres.ListViewModel = sinistresViewModel;
})();
