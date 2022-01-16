
(function () {
    let connexitesViewModel = function (parameters, emptyModel, initialData) {
        let vm = this;
        const tabEngagement = "#tabEngagement";
        const propDebut = "dateDebut";
        const propFin = "dateFin";
        const typeEngagement = emptyModel.AllTypes.first(function (tp) { return tp.Label === "Engagement"; });
        let Connexite = function (data, parentName) {
            var obj = ko.mapping.fromJS(data, {}, this);
            let objType = emptyModel.AllTypes.first(function (tp) { return tp.Label + "s" == parentName; });
            this.type = objType.Code;
            this.typeName = objType.Label;
            this.confirmRemove = ko.observable(null);
            this.confirmRemove.subscribe(function (confirm) {
                if (confirm !== null) {
                    obj.remove();
                }
            });
            this.remove = function () {
                if (!obj.canRemove()) { return; }
                if (obj.confirmRemove() === null) {
                    return common.$ui.showConfirm(
                        "Etes-vous sûr(e) de vouloir retirer le contrat " + obj.Folder.CodeOffre() + " de la connexité ?",
                        "Confirmer la suppression",
                        function () { obj.confirmRemove(true); },
                        function () { obj.confirmRemove(false); },
                        null,
                        null,
                        function () { },
                        480);
                }
                else if (obj.confirmRemove() === false) {
                    obj.confirmRemove(null);
                    return;
                }
                obj.confirmRemove(null);
                if (ko.isObservable(vm[parentName])) {
                    common.page.isLoading = true;
                   // let typeBbj = obj.type === "01" ? "1" : obj.type ;
                    common.$postJson(
                        "/Connexites/RemoveConnexite",
                        {
                            folder: parameters.affair,
                            folderConnexe: { NumContrat: obj.Folder.CodeOffre(), VersionContrat: obj.Folder.Version(), TypeContrat: obj.Folder.Type(), CodeTypeConnexite: obj.type }
                        },
                        true)
                        .done(function () {
                            if (vm[parentName]().length < 3) {
                                vm[parentName].removeAll();
                                if (obj.type == typeEngagement.Code) {
                                    vm.PeriodesEngagements.mappedRemoveAll();
                                    vm.periodes([]);
                                }
                            }
                            else {
                                //vm[parentName].remove(obj);
                                //vm.reloadPeriodes();
                                vm.setContext(obj.typeName);
                                ko.tasks.schedule(function () {
                                    vm.reloadConnexites(vm.currentContext());
                                });
                            }
                            common.page.isLoading = false;
                        });
                }
            };
            this.canRemove = ko.computed(function () {
                return !vm.isReadonly
                    && (obj.Folder.CodeOffre() != parameters.affair.codeOffre
                        || obj.Folder.Version() != parameters.affair.version);
            });
            if (this.type == typeEngagement.Code) {
                
                obj.navigateToEngagement = function () {
                    $.get("/Folder/GetFolderUrl?" + $.param({ code: obj.Folder.CodeOffre(), version: obj.Folder.Version(), type: obj.Folder.Type(), readOnly: true, targetController: "Engagements" })).done(function (url) {
                        ko.tasks.schedule(function () {
                            common.page.navigateNewTab(url);
                        });
                    });
                };
            }
            this.linkGoToAffair = ko.observable("#");
            $.get("/Folder/GetFolderUrl?" + $.param({ code: this.Folder.CodeOffre(), version: this.Folder.Version(), type: this.Folder.Type(), readOnly: true })).done(function (url) {
                obj.linkGoToAffair("/Home/Index?id=newWindow" + encodeURIComponent(url));
            });
        };
        let PeriodeEngagement = function (data, isPattern) {
            let obj = ko.mapping.fromJS(data, komap.periode, this);
            this.isReadonly = ko.computed(function () {
                return vm.isReadonly || obj.IsUsed() || obj.IsInactive();
            });
            this.isShown = ko.computed(function () {
                return vm.showAllPeriodes && vm.showAllPeriodes() || !obj.IsInactive();
            });
            this.utilisee = ko.computed(function () {
                return obj.IsUsed() ? "OUI" : "NON";
            });
            if (!vm.isReadonly && !data.IsUsed && !data.IsInactive) {
                let cancelEditDates = function (dt, prop) {
                    return true === (FrDate.compare(prop == propDebut ? dt : obj.dateDebut(), prop == propFin ? dt : obj.dateFin()) > 0);
                };
                FrDate.ko.createGetSet(this, propDebut, true, obj.Beginning, "frDebut", cancelEditDates);
                FrDate.ko.createGetSet(this, propFin, true, obj.End, "frFin", cancelEditDates);
            }
            else {
                this.dateDebut = new FrDate(data.Beginning);
            }
            this.sortableDate = ko.computed(function () {
                let date = ko.utils.unwrapObservable(obj.dateDebut);
                if (!date.isValid) {
                    return 0;
                }
                return parseInt(date.year.toString() + date.month.toString().padStart(2, "0") + date.day.toString().padStart(2, "0"));
            });
            this.Beginning.subscribe(function (d) {
                let previous = vm.PeriodesEngagements().first(function (p) { return p.Ordre() == obj.Ordre() - 1 && !obj.IsInactive(); });
                if (!!previous && !previous.IsUsed()) {
                    if (obj.frDebut() == null) {
                        previous.End(null);
                    }
                    else if (obj.frDebut().isValid) {
                        let dateFinJS = obj.frDebut().toJsDate();
                        dateFinJS.setDate(dateFinJS.getDate() - 1);
                        let dateFin = new FrDate(dateFinJS);
                        previous.End(dateFin.toDateString());
                    }
                }
            });

            this.canRemove = ko.computed(function () {
                return !obj.isReadonly();
            });
            this.remove = function () {
                if (!obj.canRemove()) { return false; }
                let firstp = vm.firstPeriode();
                let lastp = vm.lastPeriode();
                let newNextDate = null;
                let nextp = null;
                let nextPeriodes = [];
                if (firstp != lastp) {
                    let prevp = vm.PeriodesEngagements().first(function (p) { return obj.Ordre() - 1 == p.Ordre() && !p.IsInactive(); });
                    nextPeriodes = vm.PeriodesEngagements().filter(function (p) { return p.Ordre() >= (obj.Ordre() + 1) && !p.IsInactive(); });
                    nextp = vm.PeriodesEngagements().first(function (p) { return obj.Ordre() + 1 == p.Ordre() && !p.IsInactive(); });
                    if (obj == firstp && !nextp.IsUsed()) {
                        nextp.Beginning(obj.Beginning());
                    }
                    else if (obj == lastp && !prevp.IsUsed()) {
                        prevp.End(obj.End());
                        nextPeriodes = [];
                    }
                    else if (!nextp.IsUsed() && !nextp.dateDebut() && obj.dateDebut()) {
                        newNextDate = obj.dateDebut();
                    }
                }
                vm.PeriodesEngagements.mappedRemove(obj);
                ko.tasks.schedule(function () {
                    if (nextPeriodes.length > 0) {
                        for (let x = 0; x < nextPeriodes.length; x++) {
                            let ordre = nextPeriodes[x].Ordre();
                            nextPeriodes[x].Ordre(ordre - 1);
                        }
                    }
                    if (newNextDate && nextp) {
                        nextp.dateDebut(newNextDate);
                    }
                });
            };

            if (isPattern) {
                this.reset = function () {
                    obj.Valeurs.removeAll();
                };
            }

            this.canEditDebut = ko.computed(function () {
                return !obj.isReadonly() && vm.PeriodesEngagements().length > 1 && obj.Ordre() > 1;
            });
            this.canInsert = ko.computed(function () {
                return !vm.isReadonly && !obj.IsInactive() && vm.periodesOK && vm.periodesOK();
            });
            this.canCancel = ko.computed(function () {
                return !vm.isReadonly && obj.IsUsed() && !obj.IsInactive() && vm.periodesOK && vm.periodesOK();
            });
            this.cancel = function () {
                if (!obj.canCancel()) { return false; }
                obj.IsInactive(true);
            };
            this.isDateInvalid = ko.computed(function () {
                return !obj.Beginning();
            });

            this.validationErrors = ko.computed(function () {
                if (!vm.periodesErrors || !Array.isArray(vm.periodesErrors()) || vm.periodesErrors().length == 0) {
                    return [];
                }
                return vm.periodesErrors().filter(function (e) { return e.TargetCode == obj.Ordre() && e.TargetId == obj.Id() });
            });
            this.isInvalid = ko.computed(function () {
                return obj.validationErrors().length > 0;
            });
            this.errorText = ko.computed(function () {
                if (obj.isInvalid()) {
                    return obj.validationErrors().map(function (e) { return e.Error; }).join("\n");
                }
                return "";
            });
        };
        let ValeurPeriode = function (data, periode) {
            ko.mapping.fromJS(data, {}, this);
            this.Montant.extend({
                watcher: {
                    logChanges: false,
                    name: "valeurMontant",
                    isNaNEmpty: true,
                    onChanged: function () {
                        if (periode && vm.periodesErrors) {
                            let ownErrors = periode.validationErrors();
                            vm.periodesErrors.removeAll(ownErrors);
                        }
                    }
                }
            });
        };
        let komap = {
            connexites: {
                "Engagements": {
                    create: function (options) {
                        return new Connexite(options.data, "Engagements");
                    }
                },
                "PeriodesEngagements": {
                    create: function (options) {
                        return new PeriodeEngagement(options.data);
                    },
                    key: function (item) {
                        return ko.utils.unwrapObservable(item.Ordre)
                            + (ko.utils.unwrapObservable(item.IsInactive) ? "-" + ko.utils.unwrapObservable(item.Id) : "");
                    }
                },
                "PeriodePattern": {
                    create: function (options) {
                        return new PeriodeEngagement(options.data, true);
                    }
                },
                "Informations": {
                    create: function (options) {
                        return new Connexite(options.data, "Informations");
                    }
                },
                "Resiliations": {
                    create: function (options) {
                        return new Connexite(options.data, "Resiliations");
                    }
                },
                "Regularisations": {
                    create: function (options) {
                        return new Connexite(options.data, "Regularisations");
                    }
                },
                "Remplacements": {
                    create: function (options) {

                        return new Connexite(options.data, "Remplacements");
                    }
                }
            },
            periode: {
                "Beginning": {
                    update: function (options) {
                        return options.data ? options.data.substr(0, 10) : options.data;
                    }
                },
                "End": {
                    update: function (options) {
                        return options.data ? options.data.substr(0, 10) : options.data;
                    }
                },
                "Valeurs": {
                    key: function (data) {
                        return ko.utils.unwrapObservable(data.CodeTraite);
                    },
                    create: function (options) {
                        return new ValeurPeriode(options.data, options.parent);
                    }
                }
            }
        };
        ko.mapping.fromJS(emptyModel, komap.connexites, this);
        vm.hasTraites = ko.computed(function () {
            return vm.Engagements && vm.Engagements().some(function (e) {
                return e.Valeurs && e.Valeurs().isNotEmptyArray();
            });
        });
        vm.hasMultipleTraites = ko.computed(function () {
            return vm.Engagements && vm.Engagements().length > 0
                && vm.FamillesReassurances && vm.FamillesReassurances().length > 1;
        });
        vm.displayingPeriodes = ko.observable("active");
        vm.showAllPeriodes = ko.computed(function () {
            return vm.displayingPeriodes() == "";
        });
        vm.isReadonly = !!(parameters || {}).isReadonly;
        vm.openPopups = [];
        vm.showPopupSearch = function (name, onSelect) {
            vm.openPopups = [];
            vm.currentConnexite(null);
            vm.currentContext(null);
            $(document).off(customEvents.recherche.cancelled).on(customEvents.recherche.cancelled, function () {
                vm.openPopups = [];
                vm.resetContext();
                common.knockout.components.disposeFromDialog(affaire.recherche.componentName);
            });
            $(document).off(customEvents.recherche.terminated).on(customEvents.recherche.terminated, function (evt, affair) {
                if (affair && typeof onSelect === "function") {
                    onSelect(affair);
                }
            });
            /*vm.openPopups.push({
                name: "search", component: affaire.recherche.componentName, div: affaire.showSearchPopup(
                    "Recherche contrat connexité d" + (!common.tools.startsWithVowel(name) ? "e " : "'") + name,
                    { preventFilterInactif: true, preventFilterHistory: true, preventFilterOffre: true, exclusions: { affairs: [parameters.affair] } })
            });*/
            //           

            let $popupDiv = $(common.dom.get("confirm-selection", true));
            $popupDiv = common.$ui.showDialog($popupDiv, ["ko-component-popup"], "Connexité contrat d" + (!common.tools.startsWithVowel(name) ? "e " : "'") + name, { width: 750, height: "auto" }, true);
            vm.openPopups.push({ name: "confirm", component: null, div: $popupDiv });
            let contextName = vm.GetcontextName(name);
            vm.currentContext(vm.Context[contextName]);
            vm.initialConfirmData();
            ko.applyBindings(vm, $popupDiv[0]);
            return $popupDiv;

        
        };
        //};
        vm.typePart = ko.observable("ena");
        vm.tabContext = ko.observable(tabEngagement);
        vm.contextChanged = function (data, evt) {
            let sharpIndex = evt.target.href.lastIndexOf("#");
            vm.tabContext(evt.target.href.substr(sharpIndex));
        };
        vm.isContextEngagement = ko.computed(function () {
            return vm.tabContext() == tabEngagement;
        });
        vm.currentContext = ko.observable();
        vm.currentListName = null;
        vm.currentConnexite = ko.observable();
        vm.showConfirmConnexite = function () {
            let $popupDiv = $(common.dom.get("confirm-selection", true));
            $popupDiv = common.$ui.showDialog($popupDiv, ["ko-component-popup"], "Confirmation", { width: 750, height: "auto" }, true);
            vm.openPopups.push({ name: "confirm", component: null, div: $popupDiv });
            ko.applyBindings(vm, $popupDiv[0]);
            return $popupDiv;
        };
        vm.setContext = function (contextName, data) {
            vm.currentContext(vm.Context[contextName]);
            vm.currentConnexite(data);
            vm.currentListName = contextName + "s";
        };
        vm.resetContext = function () {
            let listName = vm.currentListName;
            vm.currentListName = null;
            vm.currentConnexite(null);
            vm.currentContext(null);
            return listName;
        };
       vm.GetCurrentContext = function (TypeN) {
            let CurrentContextN = "";
            switch (TypeN) {
                case 20:
                    CurrentContextN = "Engagements";
                    break;
                case 40: 
                    CurrentContextN = "Regularisations";
                    break;
                case 10:
                    CurrentContextN = "Informations";
                    break;
                case 30:
                    CurrentContextN = "Resiliations";
                    break;
           
                default:
                    CurrentContextN = "Remplacements";
            }
            return CurrentContextN;
        };
     
        vm.GetcontextName = function (TypeN) {
            let CurrentContextN = "";
            switch (TypeN) {
                case "engagement":
                    CurrentContextN = "Engagement";
                    break;
                case "régularisation":
                    CurrentContextN = "Regularisation";
                    break;
                case "information":
                    CurrentContextN = "Information";
                    break;
                case "résiliation":
                    CurrentContextN = "Resiliation";
                    break;

                default:
                    CurrentContextN = "Remplacement";
            }
            return CurrentContextN;
        };

        vm.addEngagement = function () {
            vm.showPopupSearch("engagement", function (data) {
                vm.setContext("Engagement", data);
                vm.showConfirmConnexite();
            });
        };
        vm.addRemplacement = function () {
            vm.showPopupSearch("remplacement", function (data) {
                vm.setContext("Remplacement", data);
                vm.showConfirmConnexite();
            });
        };
        vm.addInformation = function () {
            vm.showPopupSearch("information", function (data) {
                vm.setContext("Information", data);
                vm.showConfirmConnexite();
            });
        };
        vm.addResiliation = function () {
            vm.showPopupSearch("résiliation", function (data) {
                vm.setContext("Resiliation", data);
                vm.showConfirmConnexite();
            });
        };
        vm.addRegularisation = function () {
            vm.showPopupSearch("régularisation", function (data) {
                vm.setContext("Regularisation", data);
                vm.showConfirmConnexite();
            });
        };
        vm.showPopupFusion = function (params, lastContext) {
            $(document).one(customEvents.connexites.fusion.cancel, function () {
                $(document).off(customEvents.connexites.fusion.terminated);
            });
            $(document).one(customEvents.connexites.fusion.terminated, function () {
                vm.closeAllSearchConnect(lastContext);
            });
            let jsonParams = JSON.stringify(params);
            let $popin = common.knockout.components.includeInDialog(
                "Fusion de connexités",
                { width: 800, height: "auto" },
                affaire.connexites.fusion,
                jsonParams.trim().substr(1, jsonParams.length - 2));
            $popin.dialog("option", "minHeight", 200);
            return $popin;
        };
        vm.showPeriodeList = ko.computed(function () {
            return vm.PeriodesEngagements().length > 0;
        });
        vm.addPeriodeEngagement = function () {
            if (!vm.periodesOK()) {
                return false;
            }
            if (vm.PeriodesEngagements().length == 0) {
                vm.addFirstPeriode();
            }
            else {
                vm.insertPeriodeEngagement();
            }
        };
        vm.createEmptyPeriode = function () {
            let p = ko.mapping.toJS(vm.PeriodePattern);
            p.Valeurs = vm.FamillesReassurances().map(function (f) { return { CodeTraite: f.Code(), Montant: 0, MontantTotal: 0 }; });
            return p;
        };
        vm.insertPeriodeEngagement = function (periode) {
            if (!vm.periodesOK()) {
                return false;
            }
            let p = vm.createEmptyPeriode();
            let newp = null;
            let lastp = vm.lastPeriode();
            let position = periode ? periode.Ordre() : lastp.Ordre();
            p.Ordre = position + 1;
            p.Beginning = "";
            if (lastp && position == lastp.Ordre()) {
                p.End = lastp.End();
                if (!lastp.IsUsed()) {
                    lastp.dateFin(null);
                }
            }
            else {
                let nextPeriode = vm.PeriodesEngagements().first(function (x) { return x.Ordre() == p.Ordre && !x.IsInactive(); });
                let nextPeriodes = vm.PeriodesEngagements().filter(function (x) { return x.Ordre() >= p.Ordre; });
                for (let i = nextPeriodes.length - 1; i >= 0; i--) {
                    let ordre = nextPeriodes[i].Ordre();
                    nextPeriodes[i].Ordre(ordre + 1);
                }
                if (!nextPeriode) {
                    p.End = lastp.End();
                    if (!lastp.IsUsed()) {
                        lastp.dateFin(null);
                    }
                }
                else if (nextPeriode.dateDebut()) {
                    ko.tasks.schedule(function () {
                        nextPeriode.Beginning.valueHasMutated();
                        newp.Beginning.valueHasMutated();
                    });
                }
            }
            newp = vm.PeriodesEngagements.mappedCreate(p);
            vm.PeriodesEngagements.sort(vm.periodeSort);
            return newp;
        };
        vm.insertFirstPeriodeEngagement = function () {
            if (!vm.periodesOK()) {
                return false;
            }
            let periode = vm.firstPeriode();
            let copy = vm.createEmptyPeriode();
            copy.Beginning = periode.Beginning();
            copy.Ordre = 1;
            periode.Beginning(null);
            for (var x = vm.PeriodesEngagements().length - 1; x >= 0; x--) {
                let o = vm.PeriodesEngagements()[x].Ordre();
                vm.PeriodesEngagements()[x].Ordre(o + 1);
            }
            vm.PeriodesEngagements.mappedCreate(copy);
            vm.PeriodesEngagements.sort(vm.periodeSort);
        };
        vm.terminate = function () {
            $(document).trigger(customEvents.connexites.endEdit);
            if (!window.sessionStorage) return;
            var filter = JSON.parse(window.sessionStorage.getItem('recherche_filter'));
            if (!filter) return;
            $("#Offre_CodeOffre").val(filter.CodeOffre);
            $("#Offre_Version").val(filter.version);
            $("#Offre_Type").val(filter.TypeContrat);
        };

        vm.doNotConnect = function () {
            let pp = vm.openPopups.filter(function (x) { return x.name == "confirm" }).first();
            if (pp) {
                vm.openPopups.splice(vm.openPopups.indexOf(pp), 1);
                if (pp.component && typeof pp.component === "string") {
                    common.knockout.components.disposeFromDialog(pp.component);
                }
                else {
                    pp.div.dialog("close");
                }
            }
        };

        vm.reloadConnexites = function (context) {
            common.$postJson("/Connexites/GetListConnexites", { folder: parameters.affair, t: context.Type() }, true).done(function (result) {
                let listName = vm.GetCurrentContext(context.Type());
              //  let listName = "Engagements";
                if (context.Type() == 20) {
                    vm.FamillesReassurances.removeAll();
                    ko.mapping.fromJS(result.familles, {}, vm.FamillesReassurances);
                }
                ko.mapping.fromJS(result.list, komap.connexites[listName], vm[listName]);
                ko.mapping.fromJS(result.list.first(function (x) { return x.Folder.CodeOffre == parameters.affair.codeOffre; }), {}, context);
                if (context.Type() == typeEngagement.Code) {
                    vm.reloadPeriodes(context.Numero());
                }
            });
        };

        vm.closeAllSearchConnect = function (lastContext) {
            while (vm.openPopups.length > 0) {
                let pp = vm.openPopups.pop(); 
                if ((pp.component && typeof pp.component === "string") ) {
                    common.knockout.components.disposeFromDialog(pp.component);
                }
                else {
                    pp.div.dialog("close");
                }
            }
            ko.tasks.schedule(function () {
                vm.reloadConnexites(lastContext);
                common.page.isLoading = false; 
            });
        };

        vm.connect = function () {
            
            let codeContrat = vm.CodeContrat();
            let versionContrat = vm.VersionContrat();
            let commentaires = vm.Commentaires();
            let tP = "P";
            vm.CodeContratRq(false);
            vm.VersionContratRq(false);
            if (codeContrat != "" && versionContrat !="") {
                //let data = vm.currentConnexite();
                let lastContext = vm.currentContext();
                common.$postJson(
                    "/Connexites/AddContratConnexe",
                    {
                        folder: parameters.affair,
                        folderConnexe: {
                            CodeTypeConnexite: lastContext.Type(),
                            NumContrat:codeContrat,
                            VersionContrat: parseInt(versionContrat),
                            Observation: commentaires,
                            TypeContrat: tP
                        }
                    },
                    false).done(function () {
                        vm.closeAllSearchConnect(lastContext);
                        
                    }).fail(function (x, s, e) {
                        let err = null;
                        try {
                            err = JSON.parse(x.responseText);
                        } catch (e) { }
                        if (kheops.errors.isCustomError(err)) {
                            if (err.Errors.some(function (x) { return x.TargetType == "FUSION"; })) {
                                common.page.isLoading = true;
                                try {
                                    vm.showPopupFusion({
                                    ipb: parameters.affair.codeOffre, alx: parameters.affair.version,
                                    ipbCible: codeContrat, alxCible: parseInt(versionContrat),
                                    type: lastContext.Type(), obsv: lastContext.Commentaires()
                                    }, lastContext);
                                    vm.openPopups.pop().div.dialog("close");
                                }
                                catch (e) {
                                    common.showMessage(e.message);
                                    common.page.isLoading = false;
                                }
                            }
                            else {
                                common.error.showMessage(err.Errors);
                            }
                        }
                        else {
                            common.error.showXhr(x);
                        }
                    });

            }
            else {
                if (codeContrat == "") {
                    vm.CodeContratRq(true);
                }
                if (versionContrat == "") {
                    vm.VersionContratRq(true);
                }
                return false;
            }
          
        };

        vm.CodeContrat = ko.observable("");
        vm.VersionContrat = ko.observable("");
        vm.Commentaires = ko.observable();
        vm.CodeContratRq = ko.observable(false);
        vm.VersionContratRq = ko.observable(false);
        
        vm.initialConfirmData = function () {
            vm.CodeContratRq(false);
            vm.VersionContratRq(false);
            vm.CodeContrat("");
            vm.VersionContrat("");
            vm.Commentaires("");
        };
        vm.periodesOK = ko.computed(function () {
            return vm.PeriodesEngagements().length == 0
                || !vm.PeriodesEngagements().some(function (p) { return p.isDateInvalid(); });
        });
        vm.periodeSort = function (a, b) {
            return a.Ordre() > b.Ordre() ? 1 : a.Ordre() < b.Ordre() ? -1 : 0;
        };
        vm.periodeDateSort = function (a, b) {
            return a.sortableDate() > b.sortableDate() ? 1 : a.sortableDate() < b.sortableDate() ? -1 : 0;
        };
        vm.periodes = ko.observable();
        vm.hasNoPeriod = ko.computed(function () {
            return !vm.periodes() || vm.periodes().length == 0;
        });
        vm.resetPeriodeList = function () {
            let list = ko.utils.unwrapObservable(vm.periodes);
            ko.mapping.fromJS(list, komap.connexites.PeriodesEngagements, vm.PeriodesEngagements);
        };
        vm.firstPeriode = function () {
            return !vm.PeriodesEngagements || !Array.isArray(vm.PeriodesEngagements()) || vm.PeriodesEngagements().length == 0 ? null :
                vm.PeriodesEngagements().first(function (p) { return p.Ordre() == 1 && !p.IsInactive() });
        };
        vm.lastPeriode = function () {
            if (!vm.PeriodesEngagements || !Array.isArray(vm.PeriodesEngagements()) || vm.PeriodesEngagements().length == 0) {
                return null;
            }
            let maxOrdre = Math.max.apply(null, vm.PeriodesEngagements().map(function (p) { return p.Ordre(); }));
            return vm.PeriodesEngagements().first(function (p) { return p.Ordre() == maxOrdre && !p.IsInactive() });
        };
        vm.canInsertPeriodeAtFirst = ko.computed(function () {
            return !vm.isReadonly && vm.periodesOK();
        });
        vm.addFirstPeriode = function () {
            if (vm.PeriodesEngagements().length > 0) {
                return vm.addPeriodeEngagement();
            }
            let p = vm.createEmptyPeriode();
            if (!Array.isArray(vm.periodes()) || vm.periodes().length == 0) {
                common.$postJson("/Connexites/AddFirstPeriodeEngagement", { folder: parameters.affair, periode: p }, true).done(function () {
                    ko.tasks.schedule(function () {
                        vm.reloadPeriodes();
                    });
                });
            }
            else {
                ko.tasks.schedule(function () {
                    p.Beginning = vm.DateEngagementMin();
                    p.End = vm.DateEngagementMax();
                    vm.PeriodesEngagements.mappedCreate(p);
                });
            }
        };
        vm.formatPeriodes = function (jsArray) {
            if (jsArray.isNotEmptyArray()) {
                jsArray.forEach(function (p) {
                    if (p.Valeurs.isNotEmptyArray()) {
                        vm.FamillesReassurances().forEach(function (f, x) {
                            if (!p.Valeurs.first(function (v) { return v.CodeTraite == f.Code(); })) {
                                let valeur = { CodeTraite: f.Code(), Montant: 0 };
                                if (p.Valeurs[x] != null) {
                                    p.Valeurs.splice(x, 0, valeur);
                                }
                                else {
                                    p.Valeurs[x] = valeur;
                                }
                            }
                        });
                    }
                    else {
                        p.Valeurs = vm.FamillesReassurances().map(function (f) { return { CodeTraite: f.Code(), Montant: 0 }; });
                    }
                });
            }
            return jsArray;
        };
        vm.reloadPeriodes = function (numero) {
            try {
                common.page.isLoading = true;
                common.$getJson("/Connexites/GetPeriodesEngagements", { numeroConnexite: (numero || vm.Context.Engagement.Numero()) }, true).done(function (result) {
                    result.periodes = vm.formatPeriodes(result.periodes);
                    vm.periodes(result.periodes);
                    ko.mapping.fromJS(result.periodes, komap.connexites.PeriodesEngagements, vm.PeriodesEngagements);
                    vm.PeriodesEngagements().forEach(function (p) { ko.extenders.watcher.resetObj(p); });
                    common.page.isLoading = false;
                    vm.MustFixPeriodes(result.mustUpdate);
                });
            }
            catch (e) {
                console.error(e);
                common.page.isLoading = false;
                common.error.show("Impossible de rafraichir la liste des périodes");
            }
        };
        vm.periodesErrors = ko.observableArray();
        vm.modifyPeriodes = function () {
            if (!vm.periodesOK()) {
                return false;
            }
            common.page.isLoading = true;
            try {
                common.$postJson("/Connexites/ModifyPeriodes", { folder: parameters.affair, periodes: ko.mapping.toJS(vm.PeriodesEngagements) }, false)
                    .done(function () {
                        common.page.isLoading = false;
                        ko.tasks.schedule(function () {
                            window.requestAnimationFrame(function () {
                                vm.reloadPeriodes();
                            });
                        });
                    })
                    .fail(function (x) {
                        try {
                            var error = JSON.parse(x.responseText);
                            if (error.$type.indexOf("Business") >= 0) {
                                vm.periodesErrors.removeAll();
                                if ($.isArray(error.Errors)) {
                                    ko.utils.arrayPushAll(vm.periodesErrors, error.Errors);
                                }
                                else {
                                    vm.periodesErrors.push({ Error: error.Message });
                                }
                                if (vm.periodesErrors().length > 0) {
                                    //$(document).trigger(customEvents.connexites.error);
                                }
                                common.page.isLoading = false;
                                return;
                            }
                        }
                        catch (y) {
                            console.error(y);
                        }
                        common.error.showMessage(x, true);
                    });
            }
            catch (e) {
                common.page.isLoading = false;
                common.error.showMessage(e, true);
            }
        };
        vm.periodesChanged = ko.computed(function () {
            if (!vm.isContextEngagement()) {
                return false;
            }
            if (!Array.isArray(vm.periodes()) && vm.PeriodesEngagements()
                || vm.periodes().length != vm.PeriodesEngagements().length
                || vm.PeriodesEngagements().some(function (p) { return p.Id() == 0; })) {
                return true;
            }
            for (let x = 0; x < vm.PeriodesEngagements().length; x++) {
                if (vm.PeriodesEngagements()[x].Beginning() && vm.periodes()[x].Beginning) {
                    if (vm.PeriodesEngagements()[x].Beginning() != vm.periodes()[x].Beginning.substr(0, 10)) {
                        return true;
                    }
                }
                else if (vm.PeriodesEngagements()[x].Beginning() || vm.periodes()[x].Beginning) {
                    return true;
                }

                if (vm.PeriodesEngagements()[x].End() && vm.periodes()[x].End) {
                    if (vm.PeriodesEngagements()[x].End() != vm.periodes()[x].End.substr(0, 10)) {
                        return true;
                    }
                }
                else if (vm.PeriodesEngagements()[x].End() || vm.periodes()[x].End) {
                    return true;
                }

                if (vm.PeriodesEngagements()[x].Valeurs().some(function (v) { return v.Montant.hasValueChanged && v.Montant.hasValueChanged(); })) {
                    return true;
                }
            }
            return false;
        });

        ko.when(
            function () { return vm.periodesChanged() === false; },
            function () { vm.periodesErrors.removeAll(); }
        );

        ko.when(
            function () { return vm.MustFixPeriodes() === true; },
            function () { common.error.showMessage("Les périodes doivent être corrigées suite aux changements des contrats dans la connexité."); }
        );

        vm.init = function () {
            common.page.isLoading = true;
            if (typeof initialData === "object" && initialData != null) {
                try {
                    vm.periodes(initialData.PeriodesEngagements);
                    common.page.isLoading = false;
                    ko.mapping.fromJS(initialData, {}, vm);
                    $(document).trigger(customEvents.connexites.loaded);
                    vm.MustFixPeriodes.valueHasMutated();
                }
                catch (e) {
                    common.error.showMessage(e.message);
                }
                common.page.isLoading = false;
            }
            else {
                common.$postJson("/Connexites/GetAll", { affair: parameters.affair, mode: parameters.modeNavig })
                    .done(function (data) {
                        vm.periodes(data.PeriodesEngagements);
                        ko.mapping.fromJS(data, {}, vm);
                        $(document).trigger(customEvents.connexites.loaded);
                        vm.MustFixPeriodes.valueHasMutated();
                    })
                    .fail(function (x) {
                        $(document).trigger(customEvents.connexites.error);
                        common.error.show(x);
                    })
                    .always(function () {
                        common.page.isLoading = false;
                    });
            }
        };

        vm.scrollAllTraites = function (data, event) {
            $(".traites.scroll-x-hidden").scrollLeft($(event.target).scrollLeft());
        };

        $(document).trigger(customEvents.connexites.initializing);
        vm.init();
    };
    connexitesViewModel.prototype.dispose = function () {
        $(document).off(customEvents.connexites.fusion.cancel);
        $(document).off(customEvents.connexites.fusion.terminated);
        console.log(this);
    };
    affaire.connexites.ConnexitesViewModel = connexitesViewModel;
})();
