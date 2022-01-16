/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />

var ISRisque = function () {
    //---------------------Affecte les fonctions au boutons-------------
    this.mapPageElementISRisque = function () {
        // gestion de l'affichage de l'écran en mode readonly
        if (window.isReadonly) {
            $("#btnFermerOppositions").enable();
            $('input[albhorsavn]').disable();
            $("#btnAnnulerISRsq").html("<u>F</u>ermer").assignAccessKey("f").enable();
            $("#btnSuivantISRsq").hide();
            $('#btnAjouter').hide();
            $('#btnAjouterGris').show();
            $('img[name=btnSupprimerOpposition]').hide();
        }
        else {
            $('#btnAjouter').show();
            $('#btnAjouterGris').hide();
        }

        $("#btnErrorOk").kclick(function () {
            CloseCommonFancy();
        });

        $("#btnAnnulerISRsq").kclick(function () {
            infosSpeRisque.cancelForm();
        });

        $("#btnFermerISRsq").kclick(function () {
            $("#btnAnnulerISRsq").trigger('click');
        });

        $("#btnOppositions").kclick(function () {
            AlbScrollTop();
            DesactivateShortCut();
            $("#divFullScreenListeOpposition").show();
            AlternanceLigne("OppositionsBody", "", false, null);
        });

        $("#btnAjouter").kclick(function () {
            infosSpeRisque.obtenirOppositionDetails("", "I");
        });

        $("img[name=btnDetailOpp]").kclick(function () {
            if ($("#divDataOppositions").is(":visible")) {
                $('#divDataOppositions').hide();
            }
            else {
                infosSpeRisque.obtenirOppositionDetails($(this).attr("id"), "U", "A");
            }
        });

        $("td[name=EditerOpposition]").kclick(function () {
            infosSpeRisque.obtenirOppositionDetails($(this).attr("id"), "U");
        });
        $("img[name=btnSupprimerOpposition]").kclick(function () {
            infosSpeRisque.obtenirOppositionDetails($(this).attr("id"), "D");
        });

        $("#btnActionOpposition").kclick(function () {
            infosSpeRisque.enregistrerOpposition();
        });

        $("#btnFermerEditOpposition").kclick(function () {
            $("#divFullScreenEditOpposition").hide();
        });

        $("#btnFermerOppositions").kclick(function () {
            ReactivateShortCut();
            $('#divDataOppositions').hide();
            $("#divFullScreenListeOpposition").hide();
        });

        $("#chkRisque").offOn("change", function () {
            infosSpeRisque.cocherDecocherObjetsRisque($("#chkRisque").isChecked());
        });

        $("input[name=chkObjet]").offOn("change", function () {
            infosSpeRisque.cocherDecocherRisque();
        });
        if ($("#OffreSimpContext").val() == "False" || $("#OffreSimpContext").val() == undefined) {
            MapElementLCIFranchise("LCI", "Risque", $("#NomEcran").val());
            MapElementLCIFranchise("Franchise", "Risque", $("#NomEcran").val());
        }
        AffectTitleList($("#RegimeTaxe"));
        infosSpeRisque.initialRgTx = $("#RegimeTaxe").val();
        $("#RegimeTaxe").offOn("change", function () {
            AffectTitleList($(this));
        });

        $("input[albCFList]").each(function () {
            AffectTitleList($(this));
        });

        AffectTitleList($("#RegimeTaxe"));
        AffectTitleList($("#TypeRegul"));
        infosSpeRisque.formatDecimalNumricValue();

        $("#IsRegularisable").offOn('change', function () {
            if ($("#IsRegularisable").isChecked()) {
                $("#TypeRegul").enable();
            }
            else {
                $("#TypeRegul").disable(true).clear();
            }
        });

        infosSpeRisque.manageDisplayPBBNS($("#PB").val());

        $("#btnRechercheOpp").kclick(function () {
            var context = $(this).attr("albcontext");
            if (context == undefined) {
                context = "";
            }
            infosSpeRisque.openRechercheOpposition($("#CodeOrganisme").val(), $("#NomOrganisme").val(), context);
        });

        $("#btnRechercheOpposition").kclick(function () {
            var context = $(this).attr("albcontext");
            if (context == undefined) {
                context = "";
            }
            infosSpeRisque.openRechercheOpposition($("#CodeOrg").val(), $("#NomOrg").val(), context);
        });
    };

    this.mapElementsRechercheOppositions = function () {
        $("#CodeOrg").die().live('change', function () {
            $("#NomOrg").clear();
        });
        $("#NomOrg").die().live('change', function () {
            $("#CodeOrg").clear();
        });
        $("#btnFermer").kclick(function () {
            $("#divDataRechercheOppositions").clearHtml();
            $("#divRechercheOppositions").hide();
        });

        $("#InitialiserOpp").kclick(function () {
            $("#CodeOrg").clear();
            $("#NomOrg").clear();
        });

        $("#tblOppBody tbody > tr").kclick(function () {
            var nom = rhtmlspecialchars($(this).find("td").eq(1).html());
            var code = $(this).find("td").eq(0).html();

            $("#NomOrganisme").val($.trim(nom));
            $("#CodeOrganisme").val($.trim(code));
            $("#btnFermer").trigger('click');
        });
    };

    this.openRechercheOpposition = function (code, nom, context) {
        common.page.isLoading = true;
        var typeOppBenef = $("#dvTypeDest").val();
        $.ajax({
            type: "POST",
            url: "/InformationsSpecifiquesRisques/OpenRechercheOpposition",
            data: { codeOrganisme: code, nomOrganisme: nom, context: context, typeOppBenef: typeOppBenef },
            success: function (data) {
                if (context == "" || context == undefined) {
                    $("#divDataRechercheOppositions").html(data);
                    $("#divRechercheOppositions").show();
                }
                infosSpeRisque.mapElementsRechercheOppositions();
                $("#divRechercherResultOpp").html(data);
                infosSpeRisque.applyPagination();
                AlternanceLigne("OppBody", "", true, null);
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    this.bnsDisableEnableRistourne = function () {
        if ($("#TauxAppel").val() != "" && $("#TauxAppel").val() > 0 && $("#TauxAppel").val() < 100) {
            $("#Ristourne").val(100 - $("#TauxAppel").val());
            $("#Ristourne").attr("disabled", "disabled").addClass("readonly");
        }
        else {
            $("#Ristourne").removeAttr("disabled", "disabled").removeClass("readonly");
        }
    };

    this.bnsDisableRistourneTauxAppel = function () {
        // Réinitialisation des champ 
        $("input[name=TauxAppel]").offOn("change", function () {
            infosSpeRisque.bnsDisableEnableRistourne();
        });
    };

    this.paginator = function (config) {
        // throw errors if insufficient parameters were given
        if (typeof config !== "object")
            throw "Paginator was expecting a config object!";
        if (typeof config.get_rows !== "function" && !(config.table instanceof Element))
            throw "Paginator was expecting a table or get_row function!";

        // get/set if things are disabled
        if (typeof config.disable === "undefined") {
            config.disable = false;
        }

        // get/make an element for storing the page numbers in
        var box;
        if (!(config.box instanceof Element)) {
            config.box = document.createElement("div");
        }
        box = config.box;

        // get/make function for getting table's rows
        if (typeof config.get_rows !== "function") {
            config.get_rows = function () {
                var table = config.table;
                var tbody = table.getElementsByTagName("tbody")[0] || table;

                // get all the possible rows for paging
                // exclude any rows that are just headers or empty
                children = tbody.children;
                var trs = [];
                for (var i = 0; i < children.length; i++) {
                    if (children[i].tagName.toLowerCase() === "tr") {
                        if (children[i].getElementsByTagName("td").length > 0) {
                            trs.push(children[i]);
                        }
                    }
                }

                return trs;
            };
        }
        var get_rows = config.get_rows;
        var trs = get_rows();

        // get/set rows per page
        if (typeof config.rows_per_page === "undefined") {
            var selects = box.getElementsByTagName("select");
            if (typeof selects !== "undefined" && (selects.length > 0 && typeof selects[0].selectedIndex != "undefined")) {
                config.rows_per_page = selects[0].options[selects[0].selectedIndex].value;
            } else {
                config.rows_per_page = 250;
            }
        }
        var rows_per_page = config.rows_per_page;

        // get/set current page
        if (typeof config.page === "undefined") {
            config.page = 1;
        }
        var page = config.page;

        // get page count
        var pages = (rows_per_page > 0) ? Math.ceil(trs.length / rows_per_page) : 1;

        // check that page and page count are sensible values
        if (pages < 1) {
            pages = 1;
        }
        if (page > pages) {
            page = pages;
        }
        if (page < 1) {
            page = 1;
        }
        config.page = page;

        // hide rows not on current page and show the rows that are
        for (var i = 0; i < trs.length; i++) {
            if (typeof trs[i]["data-display"] === "undefined") {
                trs[i]["data-display"] = trs[i].style.display || "";
            }
            if (rows_per_page > 0) {
                if (i < page * rows_per_page && i >= (page - 1) * rows_per_page) {
                    trs[i].style.display = trs[i]["data-display"];
                } else {
                    // Only hide if pagination is not disabled
                    if (!config.disable) {
                        trs[i].style.display = "none";
                    } else {
                        trs[i].style.display = trs[i]["data-display"];
                    }
                }
            } else {
                trs[i].style.display = trs[i]["data-display"];
            }
        }

        // page button maker functions
        config.active_class = config.active_class || "active";
        if (typeof config.box_mode !== "function" && config.box_mode !== "list" && config.box_mode !== "buttons") {
            config.box_mode = "button";
        }
        if (typeof config.box_mode === "function") {
            config.box_mode(config);
        } else {
            var make_button;
            if (config.box_mode === "list") {
                make_button = function (symbol, index, config, disabled, active) {
                    var li = document.createElement("li");
                    var a = document.createElement("a");
                    a.href = "#";
                    a.innerHTML = symbol;
                    a.addEventListener("click", function (event) {
                        event.preventDefault();
                        this.parentNode.click();
                        return false;
                    }, false);
                    li.appendChild(a);

                    var classes = [];
                    if (disabled) {
                        classes.push("disabled");
                    }
                    if (active) {
                        classes.push(config.active_class);
                    }
                    li.className = classes.join(" ");
                    li.addEventListener("click", function () {
                        if (this.className.split(" ").indexOf("disabled") == -1) {
                            config.page = index;
                            infosSpeRisque.paginator(config);
                        }
                    }, false);
                    return li;
                }
            } else {
                make_button = function (symbol, index, config, disabled, active) {
                    var button = document.createElement("button");
                    button.innerHTML = symbol;
                    button.addEventListener("click", function (event) {
                        event.preventDefault();
                        if (this.disabled != true) {
                            config.page = index;
                            infosSpeRisque.paginator(config);
                        }
                        return false;
                    }, false);
                    if (disabled) {
                        button.disabled = true;
                    }
                    if (active) {
                        button.className = config.active_class;
                    }
                    return button;
                }
            }

            // make page button collection
            var page_box = document.createElement(config.box_mode == "list" ? "ul" : "div");
            if (config.box_mode == "list") {
                page_box.className = "pagination";
            }

            var left = make_button("&laquo;", (page > 1 ? page - 1 : 1), config, (page == 1), false);
            page_box.appendChild(left);

            for (var i = 1; i <= pages; i++) {
                var li = make_button(i, i, config, false, (page == i));
                page_box.appendChild(li);
            }

            var right = make_button("&raquo;", (pages > page ? page + 1 : page), config, (page == pages), false);
            page_box.appendChild(right);
            if (box.childNodes.length) {
                while (box.childNodes.length > 1) {
                    box.removeChild(box.childNodes[0]);
                }
                box.replaceChild(page_box, box.childNodes[0]);
            } else {
                box.appendChild(page_box);
            }
        }

        // make rows per page selector
        if (!(typeof config.page_options == "boolean" && !config.page_options)) {
            if (typeof config.page_options == "undefined") {
                config.page_options = [
                    { value: 5, text: '5' },
                    { value: 10, text: '10' },
                    { value: 20, text: '20' },
                    { value: 50, text: '50' },
                    { value: 100, text: '100' },
                    { value: 250, text: '250' },
                    { value: 0, text: 'All' }
                ];
            }
            var options = config.page_options;
            var select = document.createElement("select");
            for (var i = 0; i < options.length; i++) {
                var o = document.createElement("option");
                o.value = options[i].value;
                o.text = options[i].text;
                select.appendChild(o);
            }
            select.value = rows_per_page;
            select.addEventListener("change", function () {
                config.rows_per_page = this.value;
                infosSpeRisque.paginator(config);
            }, false);
            box.appendChild(select);
        }

        // status message
        var stat = document.createElement("span");
        stat.innerHTML = "page " + page + " sur " + pages
            + ", de la ligne " + (((page - 1) * rows_per_page) + 1)
            + " à " + (trs.length < page * rows_per_page || rows_per_page == 0 ? trs.length : page * rows_per_page)
            + " sur " + trs.length;
        box.appendChild(stat);

        // hide pagination if disabled
        if (config.disable) {
            if (typeof box["data-display"] == "undefined") {
                box["data-display"] = box.style.display || "";
            }
            box.style.display = "none";
        } else {
            if (box.style.display == "none") {
                box.style.display = box["data-display"] || "";
            }
        }

        // run tail function
        if (typeof config.tail_call == "function") {
            config.tail_call(config);
        }

        return box;
    };

    this.applyPagination = function () {
        if (document.getElementById("tblOppBody") !== null) {
            var box = infosSpeRisque.paginator({
                table: document.getElementById("tblOppBody"),
                box_mode: "list"
            });
            box.className = "box";
            document.getElementById("divLstResultatOppositions").appendChild(box);
        }
    };

    //----------------------Affiche les controles des paramètres de risque---------------------
    this.displayParamRisque = function () {
        $("#RisqueIndexe").offOn("change", function () {
            if ($(this).isChecked()) {
                $("#LCI").check();
                $("#Assiette").check();
                $("#paramRisqueIndexe").show();
            }
            else {
                $("#LCI").uncheck();
                $("#Franchise").uncheck();
                $("#Assiette").uncheck();
                $("#paramRisqueIndexe").hide();
            }
        });
        $("#PB").offOn("change", function () {
            infosSpeRisque.manageDisplayPBBNS($(this).val(), true);
        });
    };

    //-----------------------Affiche ou cache les infos PB/BNS/BURNER
    this.manageDisplayPBBNS = function (value, isChange) {
        if (value == "B" || value == "O") {
            $("#lblTauxMaxi").hide();
            $("#TauxMaxi").disable(true).val("0").hide();
            $("#lblPrimeMaxi").hide();
            $("#PrimeMaxi").disable(true).val("0").hide();

            if (value == "B") {
                // 3246
                infosSpeRisque.bnsDisableRistourneTauxAppel();
                $("#NbAnnee").disable(true).val("0");
                $("#SeuilSP").disable(true).val("0");
                if ($("#IsModifHorsAvn").val() !== "True") {
                    $("#TauxAppel").enable();
                    $("#Ristourne").enable();
                }
                $("#CotisationRet").disable(true).val("100");
                infosSpeRisque.bnsDisableEnableRistourne();
            }
            else if (value == "O" && $("#IsModifHorsAvn") !== "True") {
                $("input[name=TauxAppel]").die();
                $(["#NbAnnee", "#SeuilSP", "#TauxAppel", "#Ristourne"].join(",")).enable();
                $("#CotisationRet").enable();
                if (isChange) { $("#CotisationRet").val(0); }
            }
            $("#dvInfosPartBenef").show();
        }
        else {
            if (value == "U") {
                $("#NbAnnee").attr("disabled", "disabled").addClass("readonly");
                $("#TauxAppel").attr("disabled", "disabled").addClass("readonly");
                $("#CotisationRet").attr("disabled", "disabled").addClass("readonly");
                $("#Ristourne").attr("disabled", "disabled").addClass("readonly");

                $("#lblTauxMaxi").show();
                $("#lblPrimeMaxi").show();
                if ($("#IsModifHorsAvn").val() !== "True") {
                    $("#SeuilSP").removeAttr("disabled").removeClass("readonly");
                    $("#TauxMaxi").removeAttr("disabled").removeClass("readonly").show();
                    $("#PrimeMaxi").removeAttr("disabled").removeClass("readonly").show();
                }

                $("#dvInfosPartBenef").show();
                $("#NbAnnee").val(1);
                $("#CotisationRet").val(100);

            }
            else {
                $("#dvInfosPartBenef").hide();
                $("#TauxAppel").clear();
            }
        }
    };

    //----------------------Affiche les infos détaillées---------------------
    this.checkFields = function () {
        if ($("#IndiceRef").val() == "") {
            $("#RisqueIndexe").removeAttr('checked');
            $("#RisqueIndexe").attr('disabled', 'disabled');
        }

        if ($("#RisqueIndexe").isChecked()) $("#paramRisqueIndexe").show();
        else $("#paramRisqueIndexe").hide();
    };
    //----------------------Reset de la page---------------------
    this.cancelForm = function () {
        $("#divDataInformationsSpecifiquesRisque").clearHtml();
        $("#divInformationsSpecifiquesRisque").hide();

        common.autonumeric.applyAll('update', 'numeric', ' ', null, null, '99999999999', '-99999999999');

        if ($("#chkModificationAVN").length > 0) {
            if (($("#IsAvenantModificationLocale").hasTrueVal() && $("#IsTraceAvnExist").hasTrueVal()) || infosSpeRisque.isReadonly) {
                $("#chkModificationAVN").disable();
                $(document).off("change", "#chkModificationAVN");
            }
            else {
                $("#chkModificationAVN").enable();
                $("#chkModificationAVN").offOn("change", function () {
                    detailsRisque.redirect("DetailsRisque", "Index", $("#Code").val(), "0", !$("#chkModificationAVN").isChecked());
                });
            }
        }
        if ($("#chkDisplayObjetsSortis").length > 0) {
            $("#chkDisplayObjetsSortis").enable();
            $("#chkDisplayObjetsSortis").offOn("change", function () {
                if ($("#chkDisplayObjetsSortis").isChecked()) {
                    $("tr[name=objetSorti]").show();
                }
                else {
                    $("tr[name=objetSorti]").hide();
                }
            });
        }

        $("#fullScreenObjet").removeAttr("disabled");
        $("#fermerFullScreen").removeAttr("disabled");
        $("#btnConfirmOkRsq").removeAttr("disabled");
        $("#btnConfirmCancelRsq").removeAttr("disabled");
        $("#btnAnnuler").removeAttr("disabled");
        $("#btnFSAnnuler").removeAttr("disabled");
        $("#btnFSSuivant").removeAttr("disabled");
        common.page.isLoading = false;
    };
    //----------------------Suivant---------------------
    this.initSuivantIS = function () {
        $("#ForceAllowCatnat").val(false);
        window.mapElementsFieldNamesErrors = [
            { element: "Valeur_LCIRisque,Unite_LCIRisque,Type_LCIRisque", fieldname: "LCI" },
            { element: "Valeur_FranchiseRisque,Unite_FranchiseRisque,Type_FranchiseRisque", fieldname: "Franchise" },
            { element: "TypeRegul", fieldname: "TypeRegularisation" },
            { element: "CotisationRet", fieldname: "CotisRetenue" }
        ];

        $("#btnSuivantISRsq").kclick(function () {
            $(".requiredField").removeClass("requiredField");

            $("#tabGuid, #Offre_CodeOffre, #Offre_Version, #Offre_Type, #txtSaveCancel,#txtParamRedirect,#Code, #ModeNavig").enable();

            // calcul du taux complémentaire
            if ($("#TauxAppel").hasVal() && $("#TauxAppel").val() != 0) {
                $("#TauxComp").val(100 - $("#TauxAppel").val());
                $("#TauxComp").enable();
            }

            common.page.isLoading = true;

            let isModifHorsAvn = $("#IsModifHorsAvn").hasTrueVal();

            let isAvenantModificationLocale = true;

            if ($('#chkModificationAVN').exists()) {
                isAvenantModificationLocale = $('#chkModificationAVN').isChecked();
            }

            if (isModifHorsAvn && isAvenantModificationLocale) {
                $('input:not([albhorsavn])').removeAttr("disabled");
                $('select:not([albhorsavn])').removeAttr("disabled");

                let isHorsAvnRegularisable = $("#IsHorsAvnRegularisable").val() === "True";
                if (isHorsAvnRegularisable) {
                    $("#IsRegularisable").removeAttr("disabled");
                }
            }
            let type = $("#Offre_Type").val();
            let formDataInitial = JSON.stringify($(":input, input:disabled").serializeObject());
            if (type == "P") {
                formData = formDataInitial.replace("Contrat.CodeContrat", "Offre.CodeOffre").replace("Contrat.VersionContrat", "Offre.Version").replace("Contrat.Type", "Offre.Type");
            }
            else {
                formData = formDataInitial;
            }

            if ($("#Unite_LCIRisque").val() == "CPX") {
                formData = formData.replace('"LCIRisque.Type":""', '"LCIRisque.Type":"' + $("#LienCpx_LCIRisque").text().trim() + '"');
            }

            if ($("#Unite_LCIRisque").val() == "CPX") {
                formData = formData.replace('"FranchiseRisque.Type":""', '"FranchiseRisque.Type":"' + $("#LienCpx_FranchiseRisque").text().trim() + '"');
            }

            $.ajax({
                type: "POST",
                url: "/InformationsSpecifiquesRisques/EnregistrerInfosComplementaires",
                context: $('#JQueryHttpPostResultDiv'),
                data: formData,
                contentType: "application/json",
                success: function (data) {
                    if (data == "") {

                        // reload navig. tree
                        dataForm = common.$serializeSelection($(":input, input:disabled"), true);
                        common.$postJson("/InformationsSpecifiquesRisques/ReloadNavigationArbre", { modelT: dataForm, etape: "Risque", codeRisque: $("#Code").val() }, true).done(function (partialHtml) {
                            $("#LayoutArbre").hide();
                            $("#LayoutArbre").attr("id", "LayoutArbre_old");
                            $("#LayoutArbre_old").parent().prepend(partialHtml);
                            $("#LayoutArbre_old").remove();
                            setTimeout(function () {
                                layoutArbre.expandAll(true);
                            });
                        });

                        if ($("#section").val() == undefined) {
                            let isRisqueIndexe = $("#RisqueIndexe").isChecked();
                            if (isRisqueIndexe && $("#imgIndiceValeur").exists()) {
                                $("#imgIndiceValeur").show();
                            }
                            else {
                                $("#imgIndiceValeur").hide();
                            }
                            infosSpeRisque.cancelForm();
                        }
                        else {

                            infosSpe.saveAjax({ branche: $("#Branche").val(), section: $("#section").val(), risque: $("#Code").val() })
                                .done(function (result) {
                                    let isRisqueIndexe = $("#RisqueIndexe").isChecked();
                                    if (isRisqueIndexe && $("#imgIndiceValeur").exists()) {
                                        $("#imgIndiceValeur").show();
                                    }
                                    else {
                                        $("#imgIndiceValeur").hide();
                                    }
                                    infosSpeRisque.cancelForm();
                                })
                                .fail(function (xhr, status, message) {
                                    console.error(message);
                                    common.error.showXhr(xhr);
                                });
                        }
                    }
                    else {
                        let dialogBox = $(this);
                        $(dialogBox).hide();
                        if (data.ActionTag == "Nav") {
                            $(this).append(data.Script);
                        }

                        if (data.ActionTag == "Error") {
                            $('.ui-dialog-title').fadeIn().text('Alerte Erreur');
                            $('.ui-dialog-content').fadeIn().text("Erreur à l'enregistrement!");
                            $(dialogBox).dialog('open');
                            $("#ContainerDiv").hide();
                        }
                    }

                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });

            if (isModifHorsAvn && isAvenantModificationLocale) {
                $('input:not([albhorsavn])').attr("disabled", "disabled");
                $('select:not([albhorsavn])').attr("disabled", "disabled");

                var isHorsAvnRegularisable = $("#IsHorsAvnRegularisable").val() === "True";
                if (isHorsAvnRegularisable) {
                    $("#IsRegularisable").attr('disabled', 'disabled');
                }
            }
        });
    };

    //--------------Ouvre la popup de modification d'une opposition---------------
    this.obtenirOppositionDetails = function (guidId, mode, appliqueA) {
        guidId = guidId.split("_")[1];
        let typeDest = $("tr[id='edit_" + guidId + "']").attr("albtypedest");
        let tabGuid = $("#tabGuid").val();
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/InformationsSpecifiquesRisques/ObtenirOppositionDetails",
            data: {
                codeOffreContrat: $("#Offre_CodeOffre").val(),
                versionOffreContrat: $("#Offre_Version").val(),
                typeOffreContrat: $("#Offre_Type").val(),
                codeAvn: $("#NumAvenantPage").val(),
                tabGuid: tabGuid,
                codeRisque: $("#Code").val(),
                idOpposition: guidId,
                mode: mode, appliqueA: appliqueA, modeNavig: $("#ModeNavig").val(),
                typeDest: typeDest
            },
            success: function (data) {

                if (appliqueA != "A") {
                    $('#divDataOppositions').hide();
                    $("#divEditOpposition").html(data);
                    infosSpeRisque.formatDatePicker();
                    AffectDateFormat();
                    MapCommonAutoCompOrganismesOpp();
                    LoadInfoCommonByCode();
                    AlbScrollTop();
                    $("#divFullScreenEditOpposition").show();
                    $("#dvTypeDest").offOn("change", function () {
                        $("#CodeOrganisme").clear().change();
                    });
                    common.page.isLoading = false;
                    infosSpeRisque.formatDecimalNumricValue();
                }
                else {
                    $("#divDataOppositions").html(data);
                    $('#divDataOppositions').show();
                    $("input[name=chkObjet]").attr("disabled", "disabled").removeClass("readonly");
                    $("#chkRisque").attr("disabled", "disabled").removeClass("readonly");
                    common.page.isLoading = false;
                }
                if (!window.isReadonly) {
                    $("#divEditOpposition").find($("[albhorsavn]")).removeAttr("readonly").removeAttr("disabled").removeClass("readonly");
                    $("#btnRechercheOpp").show();
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //---------------Enregistre une nouvelle opposition ou ses modifications ou sa suppression-----------
    this.enregistrerOpposition = function () {
        //Vérification des champs
        var typeFinancement = $("#drlTypeFinancement").val();
        if (typeFinancement == "") {
            $("#drlTypeFinancement").addClass("requiredField");
        }
        else {
            $("#drlTypeFinancement").removeClass("requiredField");
        }

        var codeOrganisme = $("#CodeOrganisme").val();
        if (codeOrganisme == "") {
            $("#CodeOrganisme").addClass("requiredField");
        }
        else {
            $("#CodeOrganisme").removeClass("requiredField");
        }

        //Vérification du périmètre
        var vChecked = "";
        $("input[name=chkObjet]").each(function () {
            if ($(this).isChecked()) {
                vChecked = "checked";
            }
        });
        if (vChecked == "") {
            common.dialog.bigError("Vous devez sélectionner au moins un objet concerné par l'opposition", true);
            return false;
        }



        if ((codeOrganisme == "") || (typeFinancement == "")) {
            return false;
        }

        var data = infosSpeRisque.serialiserOpposition();
        var objets = infosSpeRisque.obtenirObjetsCoches();

        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/InformationsSpecifiquesRisques/EnregistrerModificationOpposition",
            data: {
                codeOffreContrat: $("#Offre_CodeOffre").val(),
                versionOffreContrat: $("#Offre_Version").val(),
                typeOffreContrat: $("#Offre_Type").val(),
                codeRisque: $("#Code").val(),
                data: data,
                objets: objets,
                modeNavig: $("#ModeNavig").val()
            },
            success: function (data) {
                $("#divBodyOppositions").html(data);
                //MapPageElement();
                infosSpeRisque.mapPageElementISRisque();
                AlternanceLigne("OppositionsBody", "noInput", true, null);
                infosSpeRisque.updateIconeBouton();
                $("#divFullScreenEditOpposition").hide();
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });

    };

    //---------fonction qui coche ou décoche toutes les checkbox objets risque
    this.cocherDecocherObjetsRisque = function (vChecked) {
        $("[name=chkObjet]").each(function () {
            $(this).attr('checked', vChecked);
        });
    };

    //-------fonction qui coche ou décoche la checkbox risque----
    this.cocherDecocherRisque = function () {
        var vChecked = "checked";
        $("input[name=chkObjet]").each(function () {
            if (!$(this).isChecked()) {
                vChecked = "";
            }
        });
        $("#chkRisque").attr('checked', vChecked);
    };

    //-------------fonction qui retourne la liste des objets concernés par l'opposition----
    this.obtenirObjetsCoches = function () {
        var objets = "";
        if (!$("#chkRisque").isChecked()) {
            objets = "0;";
            return objets;
        }

        $("[name=chkObjet]").each(function () {
            if ($(this).isChecked())
                objets += $(this).val() + ";";
        });
        return objets;
    };

    //-----------fonction qui sérialise une opposition en fonction des champs de l'écran--------
    this.serialiserOpposition = function () {
        var chaine = '[{';
        chaine += 'GuidId : "' + $("#editGuidId").val() + '",';
        chaine += 'TypeFinancement : "' + $("#drlTypeFinancement").val() + '",';
        chaine += 'CodeOrganisme : "' + $("#CodeOrganisme").val() + '",';
        chaine += 'Description : "' + $("#Description").val() + '",';
        chaine += 'Reference : "' + $("#Reference").val() + '",';
        chaine += 'Echeance : "' + $("#Echeance").val() + '",';
        chaine += 'Montant : "' + $("#Montant").val() + '",';
        chaine += 'KDESIRef : "' + $("#KDESIRef").val() + '",';
        chaine += 'Mode : "' + $("#mode").val() + '",';
        chaine += 'TypeDest : "' + $("#dvTypeDest").val() + '"';
        chaine += '}]';

        chaine = chaine.replace("\\", "\\\\");
        return chaine;
    };

    //--------------------fonction qui formate les datepicker--------------
    this.formatDatePicker = function () {
        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    };

    //------------------fonction qui change l'image du bouton opposition----
    this.updateIconeBouton = function () {
        //détermination du nombre d'oppositions
        var nbLigne = $("#tblOppositionsBody tr").length;
        if (nbLigne == 0) {
            $("#imgBtnOpposition").attr('src', "/Content/Images/opposition.png");
        }
        else if (nbLigne > 0) {
            $("#imgBtnOpposition").attr('src', "/Content/Images/opposition_check_orange.png");
            // $("#imgBtnOpposition").attr('src', "/Content/Images/opposition_check.png");
        }
    };
    //-------Formate les input/span des valeurs----------
    this.formatDecimalNumricValue = function () {
        //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
        //FormatNumerique('numeric', ' ', '9999999999999', '0');//SLA (14/05/14) : remise du séparateur espace afin que les valeurs des écrans risque s'affichent correctement
        common.autonumeric.applyAll('init', 'numeric', ' ', null, null, '9999999999999', '0');
        common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '0.00');
        common.autonumeric.applyAll('init', 'pourcentdecimal', '');
        common.autonumeric.applyAll('init', 'pourmilledecimal', '');
        common.autonumeric.applyAll('init', 'pourcentnumeric', '', ',', 0, 99, 0);
        common.autonumeric.applyAll('init', 'pourcentnum', '', ',', 0, 100, 0);
        common.autonumeric.apply($("#NbAnnee"), 'init', 'nbnumeric', null, null, 0, 9, 0);
        common.autonumeric.apply($("#TauxMaxi"), 'init', 'pourcentdec', '', ',', 4, 100, 0);

        $("#Valeur_LCIRisque").addClass("numerique");
        $("#Valeur_FranchiseRisque").addClass("numerique");
    };
};


if (!ISRisque.prototype.isReadonly) {
    Object.defineProperty(ISRisque.prototype, "isReadonly", {
        enumerable: true,
        configurable: false,
        get: function () {
            let val = $("#OffreReadOnly").hasTrueVal();
            let mdfha = $("#IsModifHorsAvn").hasTrueVal();
            return val || mdfha;
        }
    });
}

var infosSpeRisque = new ISRisque();

$(function () {
    infosSpeRisque.displayParamRisque();
    infosSpeRisque.checkFields();
    if ($("#OffreSimpContext").val() === undefined || !$("#OffreSimpContext").hasTrueVal()) {
        infosSpeRisque.initSuivantIS();
    }
    AlternanceLigne("OppositionsBody", "noInput", true, null);
    infosSpeRisque.formatDecimalNumricValue();
});
