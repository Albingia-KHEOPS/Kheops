
(function () {
    var ObjetInventaire = function (parameters, emptyObject) {
        var dotNetNamespace = "ALBINGIA.OP.OP_MVC";
        var typeTemplates = [
            { types: [1, 2], name: "activite_localisee", dotNetTypes: {} },
            { types: [3, 4, 5], name: "personne_assuree", dotNetTypes: { 3: "Models.Inventaires.PersonneDesigneeIndispo", 4: "Models.Inventaires.PersonneDesignee", 5: "Models.Inventaires.PersonneDesigneeIndispoTournage" } },
            { types: [6, 7], name: "materiel_assuree", dotNetTypes: {} },
            { types: [8], name: "poste_personne", dotNetTypes: {} },
            { types: [9], name: "multiples_situations", dotNetTypes: {} },
            { types: [10, 11, 12], name: "activites_avec_CA", dotNetTypes: {} },
            { types: [13], name: "marchandises_transportees", dotNetTypes: {} },
            { types: [14], name: "marchandises_stockees", dotNetTypes: {} },
            { types: [15], name: "vehicule_transport", dotNetTypes: {} }
        ];
        var minDate = new Date();
        minDate.setFullYear(minDate.getFullYear() - 200);
        var vm = this;
        var mappings = {
            root: {
                "Infos": {
                    key: function (item) {
                        return (ko.utils.unwrapObservable(item.IsNew) ? "N" : "") + ko.utils.unwrapObservable(item.LineNumber).toString();
                    },
                    create: function (options) {
                        var item = null;
                        if (vm.typeId() == "personne_assuree") {
                            item = new mappings.PersonneAssuree(options);                       
                        }
                        if (item) {
                            item.toJS = function () {
                                var x = ko.mapping.toJS(item);
                                x.$type = dotNetNamespace + "." + vm.typeObject.dotNetTypes[vm.NumeroType()] + ", " + dotNetNamespace;
                                return x;
                            };                  
                        }
                        return item;
                    }
                }
            },
            ActiviteLocalisee: function (options) {
                ko.mapping.fromJS(options.data, {}, this);
                this.heureDebut = ko.observable();
                this.heureFin = ko.observable();
            },
            PersonneAssuree: function (options) {
                ko.mapping.fromJS(options.data, {}, this);
                var info = this;
                info.showBirthDate = ko.observable(false);
                info.showBirthYear = ko.observable(false);
                let dt = new FrDate(info.DateNaissance());
                if (vm.NumeroType() == 3) {
                    info.showBirthDate(false);
                    info.showBirthYear(true);
                    info.birthday = ko.observable(!dt.isValid || dt.year == 1 ? null : dt.year);
                    info.birthday.subscribe(function (year) {
                        var d = new FrDate(info.DateNaissance());
                        if (!d.isValid) {
                            d = new FrDate(1, 1, year);
                        }
                        else {
                            d.year = year;
                        }
                        info.DateNaissance(d.toDateString());
                    });

                    info.birthdayLength = 4;
                }
                else {
                    if (!dt.isValid || dt.year == 1) {
                        info.DateNaissance(null);
                    }
                    info.birthday = info.DateNaissance;
                    info.birthdayLength = 10;
                    info.showBirthDate(true);
                    info.showBirthYear(false);
                }
                
                info.hasValueChanged = function () {
                    for (var p in info) {
                        const temp = info[p](); // do not remove : triggers dependency track
                        if (ko.isObservable(info[p].hasValueChanged) && info[p].hasValueChanged()) return true;
                    }
                    return false;
                };
                info.isReadonly = ko.observable(parameters.isReadonly);
                info.canSave = ko.observable(false);
                info.hasBaseInfosChanged = ko.computed(function () {
                    var inventaireInfos = ["Nom", "Prenom", "Fonction", "birthday", "CodeExtension", "Montant", "Franchise"];
                    var result = false;
                    for (var i = 0, p = inventaireInfos[i]; i < inventaireInfos.length; i++ , p = inventaireInfos[i]) {
                        const temp = info[p](); // do not remove : triggers dependency track
                        if (ko.isObservable(info[p].hasValueChanged) && info[p].hasValueChanged()) {
                            result = true;
                            break;
                        }
                    }
                    return result;
                });
                //info.trackBaseInfosChange = ko.pureComputed(function () {
                //    info.hasBaseInfosChanged(info.activateEditMode);
                //});
                info.Fonction.subscribe(console.log.bind(console));
                info.hasBaseInfosChanged.subscribe(function (changed) { if (changed) { info.activateEditMode(); } });
                info.activateEditMode = function () {
                    if (vm.Infos().length > 0 && info.IsNew() ==false) {
                        vm.disableAddItem(true);
                        vm.Infos().forEach(function (inv) {
                            if (inv.LineNumber() == info.LineNumber()) {
                                if (inv.isReadonly() != false)
                                    inv.isReadonly(false);
                                if (inv.canSave(true) != true)
                                    inv.canSave(true);
                            }
                            else {
                                if (inv.isReadonly() != true)
                                    inv.isReadonly(true);
                                if (inv.canSave(false) != false)
                                    inv.canSave(false);
                            }

                        });
                    }

                };   
                info.invalidDate = ko.computed(function () {
                    var states = [];
                    if (vm.typeId() == "personne_assuree" && info.birthday()) {
                        if (parameters.type == 3) {
                            if (info.birthday() < minDate.getFullYear()) {
                                states.push("L'année de naissance doit être superieure ou égale à " + minDate.getFullYear().toString());
                            }
                        }
                        else if (new FrDate(info.birthday()).toJsDate() < minDate) {
                            states.push("La date de naissance doit être superieure ou égale à " + minDate.toDateString());
                        }
                    }

                    if (states.length == 0) {
                        if (info.birthday.requireState && info.birthday.isRequiredReady() && !info.birthday.requireState.isValid()) {
                            states.push(false);
                        }
                    }

                    return states.length > 0;
                });
                info.dateState = ko.computed(function () {
                    var states = [];
                    if (vm.typeId() == "personne_assuree" && info.birthday()) {
                        if (vm.NumeroType() == 3) {
                            if (info.birthday() < minDate.getFullYear()) {
                                states.push("L'année de naissance doit être superieure ou égale à " + minDate.getFullYear().toString());
                            }
                        }
                        else if (new FrDate(info.birthday()).toJsDate() < minDate) {
                            states.push("La date de naissance doit être superieure ou égale à " + minDate.toDateString());
                        }
                    }

                    return states;
                });
                info.checkInfo = ko.computed(function () {
                    var states = [];
                    var state = ko.extenders.required.checkValidity(info);
                    if (!state && typeof state === "boolean") {
                        states.push(common.error.messages.requiredFields);
                    }
                    else if (typeof state === "string") {
                        states.push(state);
                    }

                    states.concat(info.dateState());
                    return { ok: states.length == 0, states: states };
                });
                info.setRequiredReady = function () {
                    for (var name in info) {
                        var prop = info[name];
                        if (ko.isObservable(prop) && prop.isRequiredReady && !prop.isRequiredReady()) {
                            prop.isRequiredReady(true);
                        }
                    }
                };
                info.save = function () {
                    info.setRequiredReady();
                    var check = info.checkInfo();
                    if (!check.ok) {
                        return false;
                    }
                    else {
                        common.page.isLoading = true;
                        var objet = info.toJS();
                        common.$postJson("/RisqueInventaire/AddOrUpdateItem",
                            { garantieId: vm.garantie, item: objet }, true).done(function (result) {
                            if (info.IsNew()) {
                                info.IsNew(false);
                            }
                            if (vm.Id() == 0) {
                                vm.Id(-1);
                            }
                                vm.resetView();                          
                            ko.extenders.watcher.resetObj(info);
                            common.page.isLoading = false;                            
                        });
                    }
                };

                info.remove = function () {
                    info.reset();
                    mappings.deleteItem(info);
                };
                info.canReset = ko.computed(function () {
                    return info.canSave() && !info.IsNew()
                });
                info.reset = function () {
                    vm.resetView();
                    ko.extenders.watcher.reInitWritableObservable(info);
                };

                ko.tasks.schedule(function () {
                    info.Nom.extend({ required: { errorMessage: "", ignoreIf: (vm.typeId() != "personne_assuree"), delayCheck: !!info.IsNew() } });
                   // info.birthday.extend({ watcher: { logChanges: false, name: "birthday", isNaNEmpty: true } });
                      info.birthday.extend({ watcher: { logChanges: false, name: "birthday", isNaNEmpty: vm.NumeroType() == 3 } });
                    ko.extenders.watcher.forObj(info, ["DateNaissance", "birthday", "IsNew", "Id", "dateState", "invalidDate", "checkInfo"]);
                    //if (vm.NumeroType() == 3) {
                    //    info.birthday.extend({ required: { errorMessage: "", zeroAsEmpty: true, NaNAsEmpty: (parameters.type == 3), delayCheck: !!info.IsNew() } });
                    //}                 
                });
            },
            MultiplesSituations: function (options) {
                ko.mapping.fromJS(options.data, {}, this);
                this.objDateDebut = new FrDate(this.DateDebut());
                this.dateDebut = ko.observable(this.objDateDebut.toDateString());
                this.heureDebut = ko.observable(this.objDateDebut.toTimeString());
                this.objDateFin = new FrDate(this.DateFin());
                this.dateFin = ko.observable(this.objDateFin.toDateString());
                this.heureFin = ko.observable(this.objDateFin.toTimeString());
            },
            deleteItem: function (item) {             
                if (item.IsNew()) {
                    vm.Infos.mappedRemove(item);
                }
                else {
                    common.page.isLoading = true;
                    common.$postJson("/RisqueInventaire/DeleteItem", { garantieId: vm.garantie, item: ko.mapping.toJS(item) }, true).done(function (result) {
                        if (result && result.message) {
                            common.error.showMessage(result.message);
                        }
                        else {
                            vm.Infos.mappedRemove(item);    
                        }
                        common.page.isLoading = false;
                    });
                }              
            }
        };

        emptyObject.NumeroType = parameters.type;
        emptyObject.isReadonly = parameters.isReadonly;
        emptyObject.idGarantie = parameters.idGarantie;
        ko.mapping.fromJS(emptyObject, mappings.root, this);

        this.garantie = {
            Affaire: window.currentAffair,
            NumOption: 1,
            NumFormule: parameters.numFormule,
            CodeBloc: parameters.codeBloc,
            Sequence: parameters.idGarantie,
            IsReadonly: parameters.isReadonly
        };
        this.Descriptif.extend({ required: true });
        this.label = ko.computed(function () {
            return vm.NumeroType() + ' - ' + vm.LabelType();
        });
        this.descriptionGlimpse = ko.computed(function () {
            return vm.Description() ? vm.Description().replace(/<\/p><p>/g, "<br/>").replace(/((<p>)|(<\/p>))/g, "") : "";
        });
        this.typeObject = typeTemplates.first(function (e) { return e.types.indexOf(parseInt(parameters.type)) > -1; });
        this.typeId = ko.observable(this.typeObject ? this.typeObject.name : "");
        this.isManif = ko.observable(parameters.type == 1);
        this.isIndispo = ko.observable(parameters.type == 3 || parameters.type == 5);
        this.isMateriel = ko.observable(parameters.type == 6);
        if (parameters.type == 10 || parameters.type == 11 || parameters.type == 12) {
            this.activityShortLabel = ko.observable(
                parameters.type == 10 ? "Prod./Réal."
                    : parameters.type == 11 ? "Loc. Tiers"
                        : "Pro Immo");

            this.activityLongLabel = ko.observable(
                parameters.type == 10 ? "Production et /ou réalisation"
                    : parameters.type == 11 ? "Location à des tiers"
                        : "Pro de l'immo");
        }

        this.valeur = ko.pureComputed({
            read: function () {
                return vm.UniteValeur() == "%" ? 0 : vm.Valeur();
            },
            write: function (v) {
                vm.Valeur(v);
            }
        });
        this.valeurPrc = ko.pureComputed({
            read: function () {
                return vm.UniteValeur() == "%" ? vm.Valeur() : 0;
            },
            write: function (v) {
                vm.Valeur(v);
            }
        });
        this.UniteValeur.subscribe(function (u) {
            if (u == "%") {
                vm.valeur(0);
            }
        }, null, "beforeChange");
        this.UniteValeur.subscribe(function (u) {
            if (u == "%") {
                vm.valeurPrc(0);
            }
        });
        this.isPercent = ko.computed(function () {
            return vm.UniteValeur() == "%";
        });
        this.isNotPercent = ko.computed(function () {
            return vm.UniteValeur() != "%";
        });
        this.deleteWhole = function () {
            if (vm.isReadonly()) {
                return;
            }

            common.dialog.initConfirm(
                function () {
                    common.page.isLoading = true;
                    common.$postJson("/RisqueInventaire/DeleteWholeInventaire", { garantieId: vm.garantie }, true).done(function (result) {
                        common.page.isLoading = false;
                        if (result && result.message) {
                            common.error.showMessage(result.message);
                        }
                        else {
                            $(document).trigger(customEvents.inventaireGarantie.validatedDelete);
                        }
                    });
                },
                null,
                "Vous allez définitivement supprimer cet inventaire. Voulez-vous continuer ?");
        };

        this.enableManualSum = ko.computed(function () {
            return !vm.isReadonly() && vm.Infos() && vm.Infos().length > 0;
        });

        this.recalculer = function () {
            if (!vm.enableManualSum()) {
                return;
            }

            var values = vm.Infos().map(function (i) {
                if (vm.typeId() == "personne_assuree") {
                    return i.Montant();
                }
            });
            var initialValue = vm.Valeur();
            try {
                if (vm.isNotPercent()) {
                    vm.valeur(values.reduce(function (a, b) { return a + b; }, 0));
                }
                else if (vm.isPercent()) {
                    vm.valeurPrc(values.reduce(function (a, b) { return a + b; }, 0));
                }
            }
            catch (e) {
                if (vm.isNotPercent()) {
                    vm.valeur(initialValue);
                }
                else if (vm.isPercent()) {
                    vm.valeurPrc(initialValue);
                }
                console.error(e);
                common.error.showMessage("Le calcul ne peut être effectué, la somme des inventaires est supérieure à la valeur maximale");
            }
        };

        this.cancelInventaire = function () {
            $(document).trigger(customEvents.inventaireGarantie.cancelledEdit);
        };

        this.checkInformations = function (popupLess) {
            var states = [];

            vm.Infos().forEach(function (i) {
                i.setRequiredReady();
                var check = i.checkInfo();
                if (!check.ok) {
                    states.push(check.states);
                }
            });

            var state = ko.extenders.required.checkValidity(vm);
            if (!state || typeof state === "string") states.push(state);

            if (states.length > 0) {
                var messages = states.filter(function (s) { return typeof s === "string" && s != ""; });
                if (messages.length > 0) {
                    if (!popupLess) common.error.showMessage(messages);
                }
                else {
                    if (!popupLess) common.error.showMessage(common.error.messages.requiredFields);
                }

                return false;
            }

            return true;
        };

        this.checkUnsaved = ko.computed(function () {
            return vm.Infos().filter(function (info) { return info.IsNew() || info.hasBaseInfosChanged(); });
        });

        this.allowValidate = ko.computed(function () {
            return vm.checkUnsaved().length == 0;
        });
        this.validateInventaire = function () {
            if (vm.isReadonly()) {
                return;
            }

            if (!vm.typeObject) {
                common.error.showMessage("Le type d'inventaire n'a pas pu être déterminé");
                return;
            }

            if (vm.checkInformations()) {
                common.page.isLoading = true;
                var inventaire = ko.mapping.toJS(vm);
                inventaire.Infos.forEach(function (i) {
                    i.$type = dotNetNamespace + "." + vm.typeObject.dotNetTypes[vm.NumeroType()] + ", " + dotNetNamespace;
                });
                common.$postJson("/RisqueInventaire/ValidateInventaireGarantie", { garantieId: vm.garantie, inventaire: inventaire }, true).done(function (result) {
                    common.page.isLoading = false;
                    var ivn = ko.mapping.toJS(vm);
                    if (ivn.Id == 0) {
                        ivn.Id = -1;
                    }
                    $(document).trigger(customEvents.inventaireGarantie.validatedEdit, ivn);
                });
            }
        };

        this.disableAddItem = ko.observable(false);
        this.addItem = function () {
            var nextLine = vm.Infos().length == 0 ? 1 : (vm.Infos().reduce(function (a, b) { return Math.max(a, b.LineNumber()); }, 0) + 1);
            vm.setReadonlyInfos(true);
            vm.emptyItem.LineNumber = nextLine;
            vm.Infos.mappedCreate(this.emptyItem);
            vm.Infos.slice(-1)[0].canSave(true);
            vm.disableAddItem(true);
        };
        this.setReadonlyInfos = function (isReadonly) {
            if (vm.Infos().length != 0) {
                vm.Infos().forEach(function (inv) {
                    inv.isReadonly(isReadonly);
                    inv.canSave(false);
                });
            }
        }
        this.resetView = function () {
            this.disableAddItem(false);
            this.setReadonlyInfos(false);
        }

        this.blurHandledItems = [];
        this.itemAdded = function (element, index, data) {
            for (var p in data) {
                if (ko.extenders.required.isImplementedBy(data[p])) {
                    let property = data[p];
                    vm.blurHandledItems.push("[name=" + p + "]");
                    $(element).find("[name=" + p + "]").on("blur", function () {
                        if (data.IsNew() && !property.isRequiredReady()) {
                            property.isRequiredReady(true);
                        }
                    });
                }
            }
        };
        
        this.itemRendered = function (elements, data) {
        };

        var proto = typeTemplates.first(function (tt) { return tt.name == "personne_assuree"; });
        if (proto.types.some(function (t) { return t == vm.NumeroType(); })) {
            vm.birthdayTitle = ko.computed(function () {
                return vm.NumeroType() == 3 ? "Année de naissance" : "Date de naissance";
            });
        }

        this.showOrHideFullDescription = function (item, event) {
            common.dom.toggleDescription(event.target);
        };

        this.emptyItem = null;
        this.initEmptyItem = function () {
            $.get("/RisqueInventaire/GetEmptyInventoryItem?type=" + parameters.type + "&branche=" + parameters.branche + "&cible=" + parameters.cible)
                .done(function (data) {
                    if (data.list && vm.CodesExtensions().length == 0) {
                        common.knockout.arrays.fill(vm.CodesExtensions, data.list);
                    }
                    vm.emptyItem = data.item;
                })
                .fail(function (x, s, e) {
                    common.error.show(x);
                })
                .always(function () {
                    common.page.isLoading = false;
                });
        };

        this.UniteValeur.extend({ required: { errorMessage: "", ignoreIfNot: vm.ActiverReport } });
        this.TypeValeur.extend({ required: { errorMessage: "", ignoreIfNot: vm.ActiverReport } });
        this.TypeTaxe.extend({ required: { errorMessage: "", ignoreIfNot: vm.ActiverReport } });

        this.init = function () {
            common.page.isLoading = true;
            try {
                if (parameters.id != 0) {
                    common.$postJson("/RisqueInventaire/GetGarantieInventaire", { garantieId: vm.garantie, filtre: { CodeCible: parameters.cible, CodeBranche: parameters.branche } }, true).done(function (data) {
                        try {
                            ko.mapping.fromJS(data, mappings.root, vm);
                            vm.initEmptyItem();
                            $(document).trigger(customEvents.inventaireGarantie.loaded);
                        }
                        catch (loadError) {
                            console.error(loadError);
                            common.error.showMessage(loadError.message);
                            common.page.isLoading = false;
                            $(document).trigger(customEvents.inventaireGarantie.error);
                        }
                    });
                }
                else {
                    common.$postJson("/RisqueInventaire/GetRefListesInventaire", { CodeCible: parameters.cible, CodeBranche: parameters.branche }, true).done(function (data) {
                        common.knockout.arrays.fill(vm.UniteListe, data.unitesValeurs, true);
                        common.knockout.arrays.fill(vm.TypeListe, data.typesValeurs, true);
                        common.knockout.arrays.fill(vm.TaxeListe, data.typesTaxes, true);
                    });

                    if (window.currentTypeInventaire) {
                        vm.NumeroType(window.currentTypeInventaire.Numero);
                        vm.LabelType(window.currentTypeInventaire.Code);
                        vm.Descriptif(window.currentTypeInventaire.Description);
                    }
                    vm.initEmptyItem();
                    $(document).trigger(customEvents.inventaireGarantie.loaded);
                }
            }
            catch (e) {
                console.error(e);
                common.error.showMessage(e.message);
                common.page.isLoading = false;
            }
        };

        $(document).trigger(customEvents.inventaireGarantie.initializing);
        vm.init();
    };

    ObjetInventaire.prototype.dispose = function () {
        while (Array.isArray(this.blurHandledItems) && this.blurHandledItems.length) {
            $(this.blurHandledItems.pop()).off("blur");
        }
    };

    risque.inventaire.ObjetInventaire = ObjetInventaire;
})();
