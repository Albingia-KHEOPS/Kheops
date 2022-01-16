
(function () {
    var OptionsFormuleViewModel = function (parameters) {
        let vm = this;
        let formulePattern = {
            ListeNatures: [],
            Options: [],
            DateEffetAvenantContrat: "",
            Numero: parameters.formule,
            Libelle: "",
            Alpha: "",
            IsAvnDisabled: false,
            isAffaireReadonly: parameters.isReadonly || parameters.isModifHorsAvenant,
            numAvenant: parseInt(parameters.numAvenant),
            currentOption: parameters.numOption || 1
        };

        let flatTemp = [];

        this.numFormule = parameters.formule;
        this.isModeAvt = parameters.numAvenant > 0;
        this.isRisqueMultiObj = parameters.nbObjets > 1;
        this.isReady = ko.observable(false);
        this.flatTreeView = ko.observableArray();
        this.displayAlternateBgColors = function () {
            window.requestAnimationFrame(function () {
                if (vm.isReady() && vm.flatTreeView().length > 0) {
                    $(".levelGarantie").each(function () {
                        $(this).removeClass(["odd", "even"]);
                    });
                    $(".lines-garanties").each(function () {
                        let $line = $(this);
                        let $prev = null;
                        let $list = $line.find(".levelGarantie.displayed");
                        let lth = $list.length;
                        for (let i = 0; i < lth; i++) {
                            // For each one, peek at the previous object.
                            if ($prev == null) {
                                $($list[i]).addClass('odd');
                            }
                            else {
                                if ($prev.hasClass('odd')) {
                                    $($list[i]).addClass('even');
                                } else {
                                    $($list[i]).addClass('odd');
                                }
                            }
                            $prev = $($list[i]);
                        }
                    });
                }
            });
        };
        this.initializers = {
            option: function (o, mp) {
                ko.mapping.fromJS(o, mp, this);
            },
            volet_bloc: function (o, mp) {
                ko.mapping.fromJS(o, mp, this);
                var obj = this;

                this.isBloc = ko.pureComputed(function () {
                    return obj.Type() == "B";
                });

                this.shortLabel = ko.pureComputed(function () {
                    return obj.isBloc() ? "B." : "V.";
                });

                this.levelColor = ko.pureComputed(function () {
                    return obj.Type() == "V" ? "levelVolet" : "levelBloc";
                });

                this.expandOrCollapse = function () {
                    obj.IsCollapsed(!obj.IsCollapsed());
                };

                this.description = ko.observable(obj.Code() + " - " + obj.Libelle());

                this.state = {
                    collapsed: obj.IsCollapsed,
                    checked: obj.IsChecked
                };

                this.isDisplayed = ko.pureComputed(function () {
                    return !obj.IsHidden() && (obj.Type() == "V"
                        || obj.parentState && !obj.parentState.collapsed() && obj.parentState.checked()) && (!vm.isReadonly() || obj.IsChecked());
                });

                this.isCheckingEnabled = ko.pureComputed(function () {
                    return !obj.IsChecked() && obj.Caractere.Code() != "O" && !vm.isReadonly()
                        || obj.IsChecked() && !obj.cannotUncheck();
                });

                this.imgExpandState = ko.pureComputed(function () {
                    return obj.state && obj.state.collapsed() ? "/Content/Images/Cl.png" : "/Content/Images/Op.png";
                });

                this.allowAvtUpdate = ko.pureComputed(function () {
                    return vm.isModeAvt && obj.AvenantModifie();
                });

                this.imgAvt = ko.pureComputed(function () {
                    return obj.allowAvtUpdate() ? "/Content/Images/voyant_rouge_petit.png" : "/Content/Images/voyant_blank_petit.png";
                });

                this.validationErrors = ko.pureComputed(function () {
                    var err = ko.utils.unwrapObservable(vm.errors);
                    return err.filter(function (x) { return (x.TargetType || "").charAt(0) == o.Type && x.TargetId == obj.UniqueId(); });
                });

                this.hasValidationErrors = ko.pureComputed(function () {
                    return $.isArray(obj.validationErrors()) && obj.validationErrors().length > 0;
                });

                if (this.isBloc()) {
                    this.cannotUncheck = ko.computed(function () {
                        return obj.IsChecked() && (
                            obj.Caractere.Code() == "O" || vm.isReadonly() || !obj.Garanties
                            || vm.isModeAvt && obj.Garanties().some(function (g) { return g.isCheckingHidden(); })
                        );
                    });
                }
                else {
                    obj.treeType = obj.Type();
                    flatTemp.push(obj);
                    this.initHideMechanics = function () {
                        obj.Blocs().forEach(function (b) {
                            vm.mappings.setupShowHideMechanics(b, obj, b);
                        });
                    };
                    this.initHideMechanics();
                    this.allGaranties = ko.computed(function () {
                        if (!this.Blocs) {
                            return [];
                        }
                        if (!this.Blocs().some(function (x) { return x.Garanties; })) {
                            return [];
                        }
                        let glists = this.Blocs().map(function (x) { return x.Garanties(); });
                        return Array.prototype.concat.apply([], glists);
                    }, this);
                    this.cannotUncheck = ko.computed(function () {
                        return obj.IsChecked() && (
                            obj.Caractere.Code() == "O" || vm.isReadonly() || !obj.Blocs
                            || vm.isModeAvt && (obj.allGaranties().length == 0 || obj.allGaranties().some(function (g) { g.isCheckingHidden(); }))
                        );
                    });
                }

                let sb = this.IsCollapsed.subscribe(function (collapsed) {
                    if (!collapsed && vm.isReady()) {
                        vm.displayAlternateBgColors();
                    }
                });
                vm.subscriptions.push(sb);

                this.isAjaxSelecting = ko.observable(false);
                sb = this.IsChecked.subscribe(function (checked) {
                    $(document).trigger(customEvents.formule.voletChecked, [checked, obj.Libelle()]);
                    if (checked && vm.isReady()) {
                        vm.displayAlternateBgColors();
                    }
                    obj.ajaxSelection(checked);
                });
                vm.subscriptions.push(sb);

                this.ajaxSelection = function (selected) {
                    if (obj.isAjaxSelecting()) {
                        obj.isAjaxSelecting(false);
                        return;
                    }
                    if (vm.isModeAvt && obj.isCheckingEnabled() && vm.modifiedForAvenant()) {
                        common.page.isLoading = true;
                        obj.isAjaxSelecting(true);
                        try {
                            var postData = {
                                affaire: currentAffair,
                                numOption: 1,
                                numFormule: vm.numFormule,
                                dateAvenant: vm.dateAvenant()
                            };
                            if (obj.isBloc()) {
                                postData.bloc = ko.mapping.toJS(obj);
                            }
                            else {
                                postData.volet = ko.mapping.toJS(obj);
                            }
                            common.$postJson("/FormuleGarantie/AddOrRemove" + (obj.isBloc() ? "Bloc" : "Volet"), postData, false)
                                .done(function () {
                                    obj.isAjaxSelecting(false);
                                })
                                .fail(function (x, s, e) {
                                    obj.IsChecked(!selected);
                                })
                                .always(function () {
                                    common.page.isLoading = false;
                                });
                        }
                        catch (e) {
                            console.error(e);
                            obj.isAjaxSelecting(false);
                            common.page.isLoading = false;
                        }
                    }
                };
            },
            garantie: function (o, mp) {
                ko.mapping.fromJS(o, mp, this);
                var obj = this;

                this.validationErrors = ko.pureComputed(function () {
                    var err = ko.utils.unwrapObservable(vm.errors);
                    return err.filter(function (x) { return x.TargetType == "Garantie" && x.TargetId == obj.UniqueId(); });
                });

                this.hasValidationErrors = ko.pureComputed(function () {
                    return $.isArray(obj.validationErrors()) && obj.validationErrors().length > 0;
                });

                this.validationErrorMessage = ko.pureComputed(function () {
                    return obj.hasValidationErrors() ? obj.validationErrors().map(function (x) { return x.Error; }).join('\n') : "";
                });

                this.allowDisplayFromTop = ko.pureComputed(function () {
                    return vm.isReady()
                        && !!obj.volet && !obj.volet.IsCollapsed() && obj.volet.IsChecked()
                        && !!obj.bloc && !obj.bloc.IsCollapsed() && obj.bloc.IsChecked();
                });

                this.isDisplayed = ko.pureComputed(function () {
                    return !obj.IsHidden()
                        && obj.allowDisplayFromTop()
                        && obj.areAllParentsChecked()
                        && (!vm.isReadonly() || obj.IsChecked() || (obj.NatureModifiable() && !vm.isModeAvt));
                });

                this.isCheckingEnabled = ko.pureComputed(function () {
                    return obj.Caractere.Code() != "O" && !vm.isReadonly();
                });

                this.isCheckingHidden = ko.pureComputed(function () {
                    return vm.isModeAvt && obj.AvenantInitial() != vm.numAvenant() && obj.IsChecked();
                });

                this.readonlyNature = ko.pureComputed(function () {
                    return vm.isReadonly() || obj.isCheckingHidden();
                });

                this.isAjaxSelecting = ko.observable(false);
                this.isChecked = ko.pureComputed({
                    read: function () {
                        return obj.IsChecked();
                    },
                    write: function (checked) {
                        if (obj.NatureModifiable()) {
                            if (!checked) {
                                obj.NatureRetenue.Code(null);
                            }
                            else if (checked && obj.NatureRetenue.Code() == "E") {
                                obj.NatureRetenue.Code(null);
                            }
                        }

                        if (vm.isReady()) {
                            vm.displayAlternateBgColors();
                        }
                        if (!obj.ajaxSelection(checked, function (cancel) { obj.IsChecked(cancel ? !checked : checked); })) {
                            obj.IsChecked(checked);
                        }
                    }
                });

                if (this.NatureModifiable()) {
                    this.natureCode = ko.pureComputed({
                        read: function () {
                            if (obj.IsChecked() && obj.NatureRetenue.Code() == "A") {
                                return "";
                            }

                            return obj.NatureRetenue.Code();
                        },
                        write: function (value) {
                            if (value === null || value === undefined) {
                                obj.NatureRetenue.Code(null);
                            }
                            else {
                                var errorMessage = "";
                                if (value == "E" && obj.IsChecked()) {
                                    errorMessage = "Impossible d'exclure une garantie sélectionnée.";
                                }
                                else if (value == "C" && !obj.IsChecked()) {
                                    errorMessage = "Impossible de comprendre une garantie non sélectionnée.";
                                }
                                else {
                                    obj.NatureRetenue.Code(value);
                                }

                                if (errorMessage) {
                                    ko.tasks.schedule(function () {
                                        common.error.showMessage(errorMessage);
                                        // force update directly through the element (due to the read-write computed))
                                        $(obj.natureCode.element).val(obj.natureCode._latestValue);
                                    });
                                }
                            }
                        },
                        owner: this
                    });
                }

                this.imgGarantieSortie = ko.pureComputed(function () {
                    return (vm.dateAvenant() && obj.DateSortie() && FrDate.compare(obj.DateSortie(), vm.dateAvenant()) < 0) ? "/Content/Images/icoDelete.png" : "/Content/Images/Checkmark-16.png";
                });

                this.refresh = function (gr) {
                    obj.IsFlagModifie(gr.IsFlagModifie);
                    obj.TypeAlimentation.Code(gr.TypeAlimentation.Code);
                    obj.DateSortie(gr.DateSortie);
                    if (obj.hasImpact()) {
                        ko.mapping.fromJS(gr.Portees, {}, obj.Portees);
                        obj.ActionsPortees.removeAll();
                        ko.utils.arrayPushAll(obj.ActionsPortees, gr.ActionsPortees);
                    }

                    if (vm.isModeAvt && vm.modifiedForAvenant()) {
                        obj.AvenantInitial(gr.AvenantInitial);
                        obj.AvenantMAJ(gr.AvenantMAJ);
                    }

                };

                this.checkingTitle = ko.pureComputed(function () {
                    return obj.IsChecked() ? "Accordé" : "";
                });

                this.padTitleCss = ko.pureComputed(function () {
                    return "label pad-" + obj.Niveau();
                });

                this.description = ko.observable(obj.Code() + " - " + obj.Libelle());

                this.allowAvtUpdate = ko.pureComputed(function () {
                    return vm.isModeAvt
                        && ((obj.Avenant() != 0 && obj.Avenant() == vm.numAvenant() && !parameters.isHisto)
                            || (obj.AvenantMAJ() == vm.numAvenant() && obj.IsChecked()));
                });

                this.imgAlertModifAvt = ko.pureComputed(function () {
                    return obj.allowAvtUpdate() ? "/Content/Images/voyant_rouge_petit.png" : "/Content/Images/voyant_blank_petit.png";
                });

                this.imgUrlDetails = ko.pureComputed(function () {
                    return obj.IsFlagModifie() ? "/Content/Images/Edit_2_16.png" : "/Content/Images/Edit_16.png";
                });

                this.allowInventory = ko.pureComputed(function () {
                    return obj.InventairePossible() && obj.IsChecked();
                });

                this.addInvertoryImg = ko.pureComputed(function () {
                    return obj.allowInventory() ? (!vm.isReadonly() && obj.IdInventaire() == 0 ? "/Content/Images/ajouterInventaire1616.png" : "/Content/Images/ajouterInventaire1616_on.png") : "";
                });

                this.hasImpact = ko.pureComputed(function () {
                    return obj.Niveau() == 1 && (obj.TypeAlimentation.Code() == "B" || obj.TypeAlimentation.Code() == "C" || vm.isRisqueMultiObj);
                });

                this.hasInitiallyPortees = ko.observable(o.HasPortee);

                this.showImpact = ko.pureComputed(function () {
                    return obj.IsChecked() && obj.hasImpact();
                });

                this.imgImpact = ko.pureComputed(function () {
                    return obj.HasPortees() ? "/Content/Images/portee_2.png" : "/Content/Images/portee.png";
                });

                this.editInventory = function () {
                    window.currentPopupObj = obj;
                    window.currentTypeInventaire = ko.mapping.toJS(obj.TypeInventaire);
                    common.knockout.components.includeInDialog(
                        "Inventaire garantie",
                        { width: 1040, height: "auto" },
                        affaire.formule.inventaires,
                        "isReadonly: " + vm.isReadonly().toString()
                        + ", idGarantie: " + obj.UniqueId()
                        + ", codeBloc: '" + obj.CodeBloc()
                        + "', numFormule: " + vm.numFormule
                        + ", type: " + obj.TypeInventaire.Numero()
                        + ", id: " + obj.IdInventaire()
                        + ", branche: '" + parameters.branche
                        + "', cible: '" + parameters.cible + "'");
                };

                this.editDetails = function () {
                    window.currentPopupObj = obj;
                    common.knockout.components.includeInDialog(
                        "Détails garantie",
                        { width: 800, height: 560 },
                        affaire.formule.detailsGarantie,
                        "isReadonly: " + vm.isReadonly().toString()
                        + ", isHisto: " + parameters.isHisto
                        + ", id: " + obj.UniqueId()
                        + ", codeBloc: '" + obj.CodeBloc()
                        + "', numFormule: " + vm.numFormule
                        + ", branche: '" + parameters.branche
                        + "', cible: '" + parameters.cible
                        + "', numeroAvenant: " + parameters.numAvenant
                        + ", dateAvenant: " + (vm.option().DateAvenantModif() ? ("'" + vm.option().DateAvenantModif() + "'") : "null"));
                };

                this.editImpact = function () {
                    window.currentPopupObj = obj;
                    window.currentPortees = ko.mapping.toJS(obj.Portees);
                    window.currentPortees.title = obj.Code() + " - " + obj.Libelle();
                    window.currentPortees.isVirtual = !obj.HasPortees() && !obj.hasInitiallyPortees();
                    window.currentPortees.Actions = ko.mapping.toJS(obj.ActionsPortees);
                    window.currentPortees.ReportCalcul = obj.TypeAlimentation && (obj.TypeAlimentation.Code() == "B" || obj.TypeAlimentation.Code() == "C");
                    common.knockout.components.includeInDialog(
                        "Portées garantie",
                        { width: 800, height: "auto" },
                        affaire.formule.portees,
                        "isReadonly: " + vm.isReadonly().toString()
                        + ", id: " + obj.UniqueId()
                        + ", codeBloc: '" + obj.CodeBloc()
                        + "', idGarantie: " + obj.UniqueId()
                        + ", numFormule: " + vm.numFormule);
                };

                this.ajaxSelection = function (selected, setCheck) {
                    if (obj.isAjaxSelecting()) {
                        obj.isAjaxSelecting(false);
                        return false;
                    }
                    if (vm.isModeAvt && vm.modifiedForAvenant()) {
                        common.page.isLoading = true;
                        obj.isAjaxSelecting(true);
                        try {
                            let g = ko.mapping.toJS(obj);
                            // force check because of the read write property
                            g.IsChecked = selected;
                            common.$postJson(
                                "/FormuleGarantie/AddOrRemoveGarantie",
                                {
                                    garantieId: {
                                        Affaire: currentAffair,
                                        NumOption: 1,
                                        NumFormule: vm.numFormule,
                                        CodeBloc: obj.CodeBloc(),
                                        Sequence: obj.UniqueId(),
                                        IsReadonly: vm.isReadonly()
                                    },
                                    garantie: g,
                                    dateAvenant: vm.dateAvenant()
                                },
                                false)
                                .done(function () {
                                    obj.isAjaxSelecting(false);
                                    $(document).trigger(customEvents.detailsGarantie.validatedEdit, [obj, setCheck]);
                                    ko.tasks.schedule(function () {
                                        let errors = vm.errors().filter(function (e) { return e.TargetType == "Garantie" && e.TargetId == obj.UniqueId(); });
                                        errors.forEach(function (e) { vm.errors.remove(e); });
                                    });
                                })
                                .fail(function (x, s, e) {
                                    setCheck(true);
                                })
                                .always(function () {
                                    common.page.isLoading = false;
                                });
                        }
                        catch (e) {
                            console.error(e);
                            obj.isAjaxSelecting(false);
                            common.page.isLoading = false;
                            return false;
                        }
                        return true;
                    }
                    return false;
                };
            }
        };
        this.mappings = {
            main: {
                "Options": {
                    create: function (options) {
                        return new vm.initializers.option(options.data, vm.mappings.option);
                    },
                    key: function (o) {
                        return ko.utils.unwrapObservable(o.Numero);
                    }
                }
            },
            option: {
                "Volets": {
                    create: function (options) {
                        return new vm.initializers.volet_bloc(options.data, vm.mappings.volet);
                    },
                    key: function (v) {
                        return ko.utils.unwrapObservable(v.UniqueId);
                    }
                }
            },
            volet: {
                "Blocs": {
                    create: function (options) {
                        return new vm.initializers.volet_bloc(options.data, vm.mappings.bloc);
                    },
                    key: function (b) {
                        return ko.utils.unwrapObservable(b.UniqueId);
                    }
                }
            },
            bloc: {
                "Garanties": {
                    create: function (options) {
                        return new vm.initializers.garantie(options.data, vm.mappings.garantie);
                    },
                    key: function (g) {
                        return ko.utils.unwrapObservable(g.UniqueId);
                    }
                }
            },
            garantie: {
                "SousGaranties": {
                    create: function (options) {
                        if (options.data === null) return null;
                        return new vm.initializers.garantie(options.data, vm.mappings.garantie);
                    },
                    key: function (g) {
                        return ko.utils.unwrapObservable(g.UniqueId);
                    }
                }
            },
            setupShowHideMechanics: function (parent, volet, bloc) {
                var list = parent == bloc ? parent.Garanties() : parent.SousGaranties ? parent.SousGaranties() : [];
                if (parent == bloc && !parent.volet) {
                    parent.parentState = volet.state;
                    parent.volet = volet;
                    parent.treeType = parent.Type();
                    flatTemp.push(parent);
                }

                list.forEach(function (e) {
                    if (!e.volet) {
                        e.treeType = "G" + e.Niveau().toString();
                        flatTemp.push(e);
                        e.bloc = bloc;
                        e.volet = volet;
                        if (parent == bloc) {
                            /*has to be equivalent to e.Niveau() == 1*/
                            e.isParentChecked = bloc.IsChecked;
                        }
                        else {
                            e.isParentChecked = parent.IsChecked;
                        }

                        e.areAllParentsChecked = ko.pureComputed(function () {
                            return e.isParentChecked() && (parent == bloc || parent.areAllParentsChecked());
                        });

                        vm.mappings.setupShowHideMechanics(e, volet, bloc);
                    }
                });
            }
        };

        ko.mapping.fromJS(formulePattern, vm.mappings.main, this);
        this.subscriptions = [];
        this.isReadonly = ko.pureComputed(function () {
            return vm.isAffaireReadonly() || vm.IsAvnDisabled();
        });

        $(document).on(customEvents.formule.cancelApplication, function () {
            if (!vm.existsFormula()) {
                vm.cancel();
            }
        });

        this.existsFormula = ko.observable(false);

        this.option = function () {
            return vm.Options && vm.Options() && vm.Options().length > 0 ? vm.Options().first(function (o) { return o.Numero() == vm.currentOption() }) : null;
        };

        this.watchLibelle = function () {
            $(document).on(customEvents.formule.labelChanged, function (evt, label) {
                vm.Libelle(label);
            });
        };

        this.saveOptions = function () {
            var formule = ko.mapping.toJS(vm);
            return common.$postJson("/FormuleGarantie/ValidateOptionsFormule", { affaire: window.currentAffair, formule: formule, libelle: $("#Libelle").val(), numOption: vm.currentOption() }, true);
        };

        this.setOptions = function () {
            var formule = ko.mapping.toJS(vm);
            return common.$postJson("/FormuleGarantie/SetOptionsFormule", { affaire: window.currentAffair, formule: formule, libelle: $("#Libelle").val() }, true);
        };

        this.modifiedForAvenant = ko.pureComputed({
            read: function () {
                var modified = false;
                if (vm.isModeAvt) {
                    modified = vm.option() && vm.option().IsModifiedForAvenant();
                    if (typeof modified === "boolean" && vm.firstlyModifiedForAvenant() === "") {
                        vm.firstlyModifiedForAvenant(!modified);
                    }
                }
                return modified;
            },
            write: function (v) {
                if (!vm.isModeAvt || !vm.option() || !vm.firstlyModifiedForAvenant()) return;
                if (v) {
                    vm.startAvenantFormule(function () { vm.option().IsModifiedForAvenant(v); });
                } else {
                    vm.revertAvenant(function () { vm.option().IsModifiedForAvenant(v); });
                }
            }
        });
        this.canChangeDateAvenant = ko.pureComputed({
            read: function () { return !vm.isReadonly() && vm.modifiedForAvenant(); }
        });
        this.canChangeModifiedFlag = ko.pureComputed({
            read: function () { return !vm.isAffaireReadonly() && (!vm.modifiedForAvenant() || vm.firstlyModifiedForAvenant()); }
        });
        this.firstlyModifiedForAvenant = ko.observable("");
        this.dateAvenant = ko.pureComputed({
            read: function () {
                return vm.modifiedForAvenant() ? (new FrDate(vm.option().DateAvenantModif())).toDateString() : "";
            },
            write: function (d) {
                vm.option().DateAvenantModif(d);
                ko.tasks.schedule(function () {
                    var dt = vm.dateAvenant();
                    vm.dateAvenant.notifySubscribers(dt);
                    vm.updateDateAnvenant();
                });
            }
        });
        this.dateMinAvenant = ko.pureComputed(function () {
            if (!vm.dateAvenant || !vm.DateEffetAvenantContrat || !vm.modifiedForAvenant || !vm.option || !vm.option()) {
                return "";
            }
            let date1 = vm.dateAvenant();
            if (!date1) {
                return vm.DateEffetAvenantContrat();
            }
            let dtAvnOpt = new FrDate(vm.option().DateAvenantModif());
            let dtAvn = new FrDate(vm.DateEffetAvenantContrat());
            let comp = FrDate.compare(dtAvn, dtAvnOpt);
            if (comp === 0) {
                return vm.DateEffetAvenantContrat();
            }
            return comp > 0 ? date1 : vm.DateEffetAvenantContrat();
        });
        this.dateAvenant.extend({ required: { errorMessage: "La date avenant doit être saisie", ignoreIfNot: vm.modifiedForAvenant } });

        this.updateData = function (data, next) {
            let tempCopy = window.currentFormule;
            vm.isReady(false);
            try {
                window.currentFormule = data;
                // explicitly remove each Volet to enforce UI refresh
                let volets = vm.option().Volets;
                for (var x = (volets().length - 1); x >= 0; x--) {
                    volets.mappedRemove({ UniqueId: volets()[x].UniqueId() });
                }
                flatTemp.length = 0;
                vm.flatTreeView.removeAll();
                ko.mapping.fromJS(data, {}, vm);
                ko.utils.arrayPushAll(vm.flatTreeView, flatTemp);
                ko.tasks.schedule(function () {
                    vm.isReady(true);
                    vm.displayAlternateBgColors();
                    if ($.isFunction(next)) {
                        next();
                    }
                    else {
                        common.page.isLoading = false;
                    }
                });
            }
            catch (e) {
                window.currentFormule = tempCopy;
                vm.isReady(true);
                throw e;
            }
        };

        this.revertAvenant = function (next) {
            if (!vm.isModeAvt || !vm.modifiedForAvenant()) return;
            common.page.isLoading = true;
            common.$postJson("/FormuleGarantie/CancelAvenantFormule", { affaire: currentAffair, numOption: vm.option().Numero(), numFormule: vm.numFormule })
                .done(function (data) {
                    try {
                        vm.updateData(data, function () {
                            $(document).trigger(customEvents.formule.cancelledNewAvenantFormule);
                            if ($.isFunction(next)) next();
                            common.page.isLoading = false;
                        });
                    }
                    catch (errLoad) {
                        console.error(errLoad);
                        common.error.showMessage(errLoad.message);
                    }
                })
                .fail(function (x, s, e) {
                    common.error.show(x);
                    vm.IsAvnDisabled(false);
                });
        };

        this.startAvenantFormule = function (next) {
            if (!vm.isModeAvt || vm.modifiedForAvenant()) return;
            common.page.isLoading = true;
            common.$postJson("/FormuleGarantie/StartAvenantFormule", { affaire: currentAffair, numOption: vm.option().Numero(), numFormule: vm.numFormule, dateEffet: vm.DateEffetAvenantContrat() })
                .done(function (data) {
                    try {
                        vm.updateData(data, function () {
                            $(document).trigger(customEvents.formule.startedNewAvenantFormule);
                            if ($.isFunction(next)) next();
                            common.page.isLoading = false;
                        });
                    }
                    catch (errLoad) {
                        console.error(errLoad);
                        common.error.showMessage(errLoad.message);
                    }
                })
                .fail(function (x, s, e) {
                    common.error.show(x);
                    vm.IsAvnDisabled(true);
                });
        };
        this.errors = ko.observableArray();
        this.hasErrors = ko.pureComputed(function () { return vm.errors().length > 0; });
        this.dateErrors = ko.pureComputed(function () {
            return vm.errors().map(function (e) { return (e.TargetType ? (e.TargetType + " " + e.TargetCode + " : ") : "") + e.Error; }).join('\n');
        });
        this.updateDateAnvenant = function () {
            if (!vm.isModeAvt || !vm.modifiedForAvenant() || !vm.dateAvenant()) return;
            common.page.isLoading = true;
            common.$postJson("/FormuleGarantie/UpdateDateAvenantFormule", { affaire: currentAffair, numOption: vm.option().Numero(), numFormule: vm.numFormule, dateEffet: vm.dateAvenant() })
                .done(function () {
                    vm.errors.removeAll();
                })
                .fail(function (x, s, e) {
                    try {
                        var error = JSON.parse(x.responseText);
                        if (error.$type.indexOf("Business") >= 0) {
                            vm.errors.removeAll();
                            if ($.isArray(error.Errors)) {
                                ko.utils.arrayPushAll(vm.errors, error.Errors);
                            }
                            else {
                                vm.errors.push({ Error: error.Message });
                            }
                            if (vm.errors().length > 0) {
                                $(document).trigger(customEvents.formule.error);
                            }
                            return;
                        }
                    }
                    catch (e) { }
                    common.error.showMessage(e, true);

                })
                .always(function () {
                    common.page.isLoading = false;
                });
        };
        this.imgDateErrors = ko.pureComputed(function () {
            return vm.dateErrors() ? "/Content/Images/alertes/notif_icn_crit16.png" : null;
        });
        this.loadData = function (data) {
            try {
                ko.mapping.fromJS(data, {}, vm);
                ko.utils.arrayPushAll(vm.flatTreeView, flatTemp);
                common.page.isLoading = false;
                vm.watchLibelle();
            }
            catch (errLoad) {
                console.error(errLoad);
                common.error.showMessage(errLoad.message);
            }

            $(document).on(customEvents.porteesGarantie.cancelledEdit, function () {
                common.knockout.components.disposeFromDialog(affaire.formule.portees.componentName);
                vm.purgeCurrentPopupContext();
            });

            $(document).on(customEvents.porteesGarantie.validatedEdit, function (evt, portees) {
                ko.mapping.fromJS(portees, {}, window.currentPopupObj.Portees);
                window.currentPopupObj.HasPortees(portees.CodeAction && portees.ObjetsRisque.some(function (o) { return o.IsSelected; }));
                common.knockout.components.disposeFromDialog(affaire.formule.portees.componentName);
                vm.purgeCurrentPopupContext();
            });

            $(document).on(customEvents.detailsGarantie.cancelledEdit, function () {
                common.knockout.components.disposeFromDialog(affaire.formule.detailsGarantie.componentName);
                vm.purgeCurrentPopupContext();
            });

            $(document).on(customEvents.detailsGarantie.validatedEdit, function (evt, obj, setCheck) {
                let fromPopup = obj == null;
                if (fromPopup) {
                    obj = window.currentPopupObj;
                }
                common.page.isLoading = true;
                try {
                    common.$postJson("/FormuleGarantie/GetSingleGarantie", {
                        Affaire: window.currentAffair,
                        NumOption: 1,
                        NumFormule: parameters.formule,
                        CodeBloc: obj.CodeBloc(),
                        Sequence: obj.UniqueId(),
                        IsReadonly: vm.isReadonly()
                    }, true).done(function (g) {
                        try {
                            obj.refresh(g);
                            if ($.isFunction(setCheck)) {
                                setCheck();
                            }

                            obj.validationErrors().forEach(function (x) { vm.errors.remove(x); });
                            common.page.isLoading = false;
                            if (fromPopup) {
                                common.knockout.components.disposeFromDialog(affaire.formule.detailsGarantie.componentName);
                            }
                        }
                        catch (e1) {
                            common.error.showMessage(e1.message);
                        }
                        if (fromPopup) {
                            vm.purgeCurrentPopupContext();
                        }
                    });
                }
                catch (e2) {
                    common.error.showMessage(e2.message);
                    if (fromPopup) {
                        vm.purgeCurrentPopupContext();
                    }
                }
            });

            $(document).on(customEvents.inventaireGarantie.cancelledEdit, function () {
                common.knockout.components.disposeFromDialog(affaire.formule.inventaires.componentName);
                vm.purgeCurrentPopupContext();
            });

            $(document).on(customEvents.inventaireGarantie.validatedEdit, function (evt, ivn) {
                var garantie = window.currentPopupObj;
                if (garantie.IdInventaire() == 0) {
                    garantie.IdInventaire(ivn.Id);
                }
                common.knockout.components.disposeFromDialog(affaire.formule.inventaires.componentName);
                vm.purgeCurrentPopupContext();
            });

            $(document).on(customEvents.inventaireGarantie.validatedDelete, function () {
                var garantie = window.currentPopupObj;
                garantie.IdInventaire(0);
                common.knockout.components.disposeFromDialog(affaire.formule.inventaires.componentName);
                vm.purgeCurrentPopupContext();
            });

            $(document).on([customEvents.detailsGarantie.loaded, customEvents.inventaireGarantie.loaded].join(" "), function () {
                var hiddenZones = $(".ko-cloak.conceal");
                hiddenZones.removeClass("conceal");
            });
        };

        this.purgeCurrentPopupContext = function () {
            window.currentPopupObj = null;
            window.currentPortees = null;
            window.currentTypeInventaire = null;
        };

        this.init = function () {
            if (window.currentFormule) {
                vm.existsFormula(true);
                vm.loadData(window.currentFormule);
            }
            else {
                // hide tree-view
                vm.existsFormula(false);
            }
        };
        this.refreshFormule = function (cb) {
            common.page.isLoading = true;
            common.$postJson("/FormuleGarantie/GetFormule", { affaire: currentAffair, numFormule: vm.numFormule, numOption: vm.currentOption() }, true)
                .done(function (data) {
                    try {
                        ko.mapping.fromJS(data, {}, vm);
                        if ($.isFunction(cb)) {
                            cb(data);
                        }
                        common.page.isLoading = false;
                    }
                    catch (errLoad) {
                        common.error.showMessage(errLoad.message);
                    }
                });


        };
        this.validate = function (data, event, params) {
         

            $(document).trigger(customEvents.formule.saving);

            if (vm.isReadonly()) {
                return vm.forward(data, event, params);
                
            }
            common.page.isLoading = true;
            if (!vm.Libelle()) {
                common.error.showMessage(common.error.messages.requiredFields);
                return;
            }
            else if (!vm.dateAvenant.requireState.isValid()) {
                common.error.showMessage(vm.dateAvenant.requireState.message());
                return;
            }

            var selectionIsValid = vm.option().Volets().some(function (v) {
                return v.IsChecked() && v.Blocs().some(function (b) {
                    return b.IsChecked() && b.Garanties().some(function (g) {
                        return g.IsChecked();
                      

                    });
                });
            });

            if (!selectionIsValid) {
                common.error.showMessage("Veuillez sélectionner au moins une garantie de niveau 1");
                return;
            }

            vm.saveOptions().done(function (result) {
                affaire.callStandardNext(params && params.returnHome);
            });
        };

        this.cancel = function () {
            common.page.isLoading = true;
            common.$postJson("/FormuleGarantie/CancelChanges", { affaire: window.currentAffair, pageContext: window.infosTab }, true);
        };

        this.askCancel = function () {
            common.dialog.initConfirm(vm.cancel, null, "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br /><br />Confirmez-vous l’annulation ?<br />");
        };

        this.back = function () {
            vm.cancel();
        };

        this.forward = function (data, event, params) {
            affaire.callStandardNext(params && params.returnHome);
        };

        this.fullscreen = function fullscreen() {
            $("body").toggleClass("fullscreen");
        };

        this.afterLoad = function () {
            try {
                ko.applyBindings(vm, common.dom.get("action_buttons_formula"));
                ko.applyBindings(vm, common.dom.get("divModeAvenant"));
                $(document).trigger(customEvents.formule.loaded);
                window.requestAnimationFrame(function () {
                    vm.isReady(true);
                    vm.displayAlternateBgColors();
                    common.page.isLoading = false;
                });
            }
            catch (error) {
                console.error(error);
                common.error.showMessage(error.message);
                $(document).trigger(customEvents.formule.error);
            }
            setTimeout(function () {
                $(document).trigger(customEvents.formule.rendered);
            }, 5);
        };

        this.start = function () {
            $(document).trigger(customEvents.formule.initializing);
            try {
                common.page.isLoading = true;
                vm.init();
                ko.tasks.schedule(vm.afterLoad);
            }
            catch (error) {
                console.error(error);
                common.error.showMessage(error.message);
                $(document).trigger(customEvents.formule.error);
            }
        };

        this.start();
    };

    OptionsFormuleViewModel.prototype.dispose = function () {
        $(document).off(customEvents.formule.labelChanged);
        $(document).off(customEvents.formule.cancelApplication);
        $(document).off(customEvents.porteesGarantie.cancelledEdit);
        $(document).off(customEvents.porteesGarantie.validatedEdit);
        $(document).off(customEvents.detailsGarantie.cancelledEdit);
        $(document).off(customEvents.detailsGarantie.validatedEdit);
        $(document).off(customEvents.inventaireGarantie.cancelledEdit);
        $(document).off(customEvents.inventaireGarantie.validatedEdit);
        $(document).off(customEvents.inventaireGarantie.validatedDelete);
        $(document).off([customEvents.detailsGarantie.loaded, customEvents.inventaireGarantie.loaded].join(" "));
        while (this.subscriptions && this.subscriptions.length) {
            this.subscriptions.pop().dispose();
        }
    };
    affaire.formule.OptionsFormuleViewModel = OptionsFormuleViewModel;
})();
