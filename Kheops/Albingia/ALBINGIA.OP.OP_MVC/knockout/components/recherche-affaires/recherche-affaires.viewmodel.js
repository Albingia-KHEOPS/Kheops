
(function () {
    var rechercheViewModel = function (parameters, emptyObject) {
        var vm = this;
        $(document).trigger(customEvents.recherche.initializing);
        emptyObject.resultList = [];

        let AffairResult = function (result) {
            ko.mapping.fromJS(result, {}, this);
            this.identifier = result.OffreContratNum + "_" + result.Version + "_" + result.Type;
            this.typImgClass = "";
            this.typImgTitle = "";
            if (result.Type == "O") {
                this.typImgClass = result.HasDoubleSaisie ? "icon-double-saisie" : "icon-offre";
                this.typImgTitle = result.HasDoubleSaisie ? "Double saisie" : "Offre";
            }
            else {
                this.typImgClass = !result.ContratMere ? "icon-contrat-standard" : result.ContratMere == "A" ? "icon-mere-aliment" : "icon-mere";
                this.typImgTitle = !result.ContratMere ? "Contrat standard" : result.ContratMere == "A" ? "Aliment" : "Contrat mère";
            }
            this.date = "";

            this.resultSeleted = function (obj) {
                vm.selectedAffair(ko.mapping.toJS(obj));
            };
        };

        let mapping = {
            "resultList": {
                create: function (options) {
                    return new AffairResult(options.data);
                }
            }
        };

        ko.mapping.fromJS(emptyObject, mapping, this);

        vm.typeOffreChecked = ko.observable(vm.GetOffresOnly() || !vm.GetContratsOnly());
        vm.typeContratChecked = ko.observable(vm.GetContratsOnly() || !vm.GetOffresOnly());
        vm.changeTypeFilter = null;
        common.knockout.binding.init2CbxPreventNoCheck(vm.typeOffreChecked, vm.typeContratChecked, vm.GetOffresOnly, vm.GetContratsOnly, vm.changeTypeFilter);

        vm.typeApporteurChecked = ko.observable(vm.GetApporteursOnly() || !vm.GetGestionnairesOnly());
        vm.typeGestionnaireChecked = ko.observable(vm.GetGestionnairesOnly() || !vm.GetApporteursOnly());
        vm.changeTypeCourtierFilter = null;
        common.knockout.binding.init2CbxPreventNoCheck(vm.typeApporteurChecked, vm.typeGestionnaireChecked, vm.GetApporteursOnly, vm.GetGestionnairesOnly, vm.changeTypeCourtierFilter);

        vm.codePreneurAssurance = ko.observable("");
        vm.editingCodePreneur = ko.observable(false);
        vm.editingCodePreneur.subscribe(function (hadFocus) {
            if (hadFocus) {
                vm.getPreneurByCode();
            }
        }, null, "beforeChange");
        vm.getPreneurByCode = function () {
            if (vm.codePreneurAssurance()) {
                $.get("/AutoComplete/GetPreneurAssuranceByCode?codeString=" + vm.codePreneurAssurance()).then(function (data) {
                    if (!data || !data.Code) {
                        vm.PreneurAssurance(null);
                    }
                    else {
                        vm.PreneurAssurance(data);
                    }
                });
            }
            else {
                vm.PreneurAssurance(null);
            }
        };
        vm.PreneurAssurance.subscribe(function (p) {
            if (p && p.Code) {
                vm.codePreneurAssurance(parseInt(p.Code));
            }
            else {
                vm.codePreneurAssurance(null);
            }
        });

        vm.numeroCourtier = ko.observable("");
        vm.editingNumeroCourtier = ko.observable(false);
        vm.editingNumeroCourtier.subscribe(function (hadFocus) {
            if (hadFocus) {
                vm.getCourtierByNumero();
            }
        }, null, "beforeChange");
        vm.getCourtierByNumero = function () {
            if (vm.numeroCourtier()) {
                $.post("/AutoComplete/GetCabinetCourtageByCode", { codeString: vm.numeroCourtier() }).then(function (data) {
                    if (!data || !data.Code) {
                        vm.Courtier(null);
                    }
                    else {
                        vm.Courtier(data);
                    }
                });
            }
            else {
                vm.Courtier(null);
            }
        };
        vm.Courtier.subscribe(function (p) { vm.numeroCourtier(parseInt(p && p.Code ? p.Code : null)); });

        vm.filteredCibles = ko.pureComputed(function () {
            return vm.CodeBranche() ? vm.Cibles().filter(function (c) { return c.Parent() == vm.CodeBranche(); }) : vm.Cibles();
        });

        vm.searchById = ko.pureComputed(function () {
            return vm.Code() != null && vm.Code() !== "";
        });

        vm.filtreSituation = ko.observable("");
        vm.cannotFilterSituation = ko.computed(function () {
            return vm.searchById() || vm.filtreSituation() != "";
        });

        vm.preventFilterOffre = ko.observable(!!parameters.preventFilterOffre);
        if (vm.preventFilterOffre()) {
            vm.typeOffreChecked(false);
        }
        vm.preventFilterInactif = ko.observable(!!parameters.preventFilterInactif);
        vm.preventFilterHistory = ko.observable(!!parameters.preventFilterHistory);
        if (parameters.preventFilterInactif) {
            let codes = vm.Situations().filter(function (s) { return s.Code() == "X" || s.Code() == "N" || s.Code() == "W"; })
            vm.Situations.removeAll(codes);
        }
        if (parameters.exclusions) {
            if (Array.isArray(parameters.exclusions.affairs) && parameters.exclusions.affairs.length > 0) {
                ko.utils.arrayPushAll(vm.ExcludedAffairs, parameters.exclusions.affairs);
            }
        }

        ko.when(function () {
            return vm.filtreSituation() != "";
        }, function () {
            vm.GetEnCoursOnly(vm.filtreSituation() == "actif");
            vm.GetInactifsOnly(vm.filtreSituation() == "inactif");
            vm.CodeSituation("");
        });
        ko.when(function () {
            return vm.filtreSituation() == "";
        }, function () {
            vm.GetEnCoursOnly(false);
            vm.GetInactifsOnly(false);
        });

        vm.isFilterMissing = ko.computed(function () {
            return !vm.searchById()
                && vm.Courtier() == null && vm.PreneurAssurance() == null && vm.PreneurAssuranceVille() == null
                && vm.Souscripteur() == null && vm.Gestionnaire() == null && vm.AdresseVille() == null
                && (vm.TexteAdresse() == "" || vm.TexteAdresse() == null)
                && (vm.Text() == "" || vm.Text() == null)
                && (vm.DateMin() == "" || vm.DateMin() == null)
                && (vm.DateMax() == "" || vm.DateMax() == null)
                && (vm.CodeBranche() == "" || vm.CodeBranche() == null)
                && (vm.CodeCible() == "" || vm.CodeCible() == null)
                && (vm.CodeEtat() == "" || vm.CodeSituation() == null)
                && (vm.Siren() == "" || vm.Siren() == null);
        });

        vm.resetFilter = function () {
            vm.Courtier(null);
            vm.PreneurAssurance(null);
            vm.PreneurAssuranceVille(null);
            vm.Souscripteur(null);
            vm.Gestionnaire(null);
            vm.AdresseVille(null);
            vm.TexteAdresse("");
            vm.CodeTypeDate("");
            vm.Text("");
            vm.DateMin("");
            vm.DateMax("");
            vm.CodeBranche("");
            vm.CodeCible("");
            vm.CodeEtat("");
            vm.Siren("");
            vm.Code("");
            vm.Version(null);
        };
        
        vm.hasResults = ko.computed(function () {
            return vm.resultList().length > 0;
        });
        vm.showAnyResult = ko.observable(false);

        vm.selectedAffair = ko.observable(null);
        
        vm.backToFilters = function () {
            vm.showAnyResult(false);
        };
        vm.lastFilter = ko.observable(null);

        vm.hasFilterChanged = ko.pureComputed(function () {
            let filter = vm.lastFilter();
            if (!filter) {
                return null;
            }
            if (vm.searchById()) {
                return filter.Code != (vm.Code() || "").toUpperCase()
                    || (!isNaN(filter.Version) && !isNaN(vm.Version()) && filter.Version != vm.Version())
                    || filter.GetOffresOnly != vm.GetOffresOnly() || filter.GetContratsOnly != vm.GetContratsOnly()
                    || filter.Mode != vm.Mode();
            }
            for (let prop in filter) {
                let type = typeof filter[prop];
                if (type === "object") {
                    if ((prop.toUpperCase() == "SOUSCRIPTEUR" || prop.toUpperCase() == "GESTIONNAIRE")
                        && (filter[prop] || {}).Code != (vm[prop]() || {}).Code) {
                        return true;
                    }
                }
                else if (type !== "function") {
                    if (typeof filter[prop] === "number" && isNaN(filter[prop]) && isNaN(vm[prop]())) {
                        // take no action
                    }
                    else {
                        switch (prop) {
                            default:
                                if (filter[prop] != vm[prop]()) {
                                    return true;
                                }
                        }
                    }
                }
            }
            return false;
        });
        vm.canShowResult = ko.computed(function () {
            return vm.hasFilterChanged() === false;
        });

        vm.showResultsAgain = function () {
            vm.showAnyResult(true);
        };

        vm.init = function () {
            common.page.isLoading = false;
            $(document).trigger(customEvents.recherche.loaded);
        };

        vm.storeFilter = function (filter) {
            filter.numeroCourtier = vm.numeroCourtier();
            filter.codePreneurAssurance = vm.codePreneurAssurance();
            vm.lastFilter(filter);
        };

        vm.search = function () {
            common.page.isLoading = true;
            let filter = ko.mapping.toJS(vm);
            if (filter.Code != null) {
                filter.Code = filter.Code.toUpperCase();
            }
            common.$postJson("/RechercheAffaires/Search", filter, true).done(function (list) {
                ko.mapping.fromJS(list, mapping.resultList, vm.resultList);
                vm.storeFilter(filter);
                vm.showAnyResult(true);
                common.page.isLoading = false;
            });
        };

        vm.cancel = function () {
            $(document).trigger(customEvents.recherche.cancelled);
        };

        vm.terminate = function () {
            $(document).trigger(customEvents.recherche.terminated, vm.selectedAffair());
        };

        vm.init();
    };

    affaire.recherche.RechercheAffairesViewModel = rechercheViewModel;
})();
