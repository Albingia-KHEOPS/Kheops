
var InfosBaseAffaireNouvelle = function () {
    //--------Déplace le div de l'adresse----------
    this.mapAdresse = function () {
        $("#divAdresse").html($("#divHideAdresse").html());
        LinkHexavia();
    };
    //----------Map les élements de la page---------
    this.initPage = function () {
        $("#CodeCourtierInvalideGestionnaireDiv").clearHtml();
        if (window.isReadonly && $('#IsModifHorsAvn').val() !== 'True') {
            $("#btnGestionnairePayeur").removeClass('CursorPointer');
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
            $("#RechercheAvanceePreneurAssurance").removeClass("CursorPointer");
            $("#RechercheAvanceePreneurAssurance").attr("src", "/Content/Images/loupegris.png");
        }
        toggleDescription($("#InformationContrat_Observation"));
        $("#btnInfoOk").kclick(function () {
            CloseCommonFancy();
        });
        $("#btnAnnuler").kclick(function () {
            ShowCommonFancy("Confirm", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        });

        if (window.isReadonly) {
            $("#btnAnnuler").addClass("None");
        }

        if ($("#InformationBase_NumContratRemplace").hasVal() || $("#InformationBase_NumAlimentRemplace").hasVal()) {
            $("#InformationBase_NumContratRemplace").disable();
            $("#InformationBase_NumAlimentRemplace").disable();
            $("#btnRechContrat").disable().attr("src", "/Content/Images/loupegris.png");
        }

        $("#btnConfirmOk").kclick(function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "Cancel":
                    infosBase.cancel();
                    break;
            }
            $("#hiddenAction").clear();
        });
        $("#btnConfirmCancel").kclick(function () {
            CloseCommonFancy();
            $("#hiddenAction").clear();
        });

        infosBase.initRechercheAvanceeCourtiers();

        if ($("#EditMode").val() === "False") {
            $("#CourtierApporteur_Courtier_CodeCourtier").disable(true);
            $("#CourtierApporteur_Courtier_NomCourtier").disable(true);
            $("img[name=RechercherCourtier][albcontext=courtierApporteur]").attr("src", "/Content/Images/loupegris.png");
            $("#CourtierPayeur_Courtier_CodeCourtier").disable(true);
            $("#CourtierPayeur_Courtier_NomCourtier").disable(true);
            $("img[name=RechercherCourtier][albcontext=courtierPayeur]").attr("src", "/Content/Images/loupegris.png");
        }

        infosBase.assignTitlesElements();
        MapBoitesDialogue();

        if ($("#InformationBase_ContratMere").attr("disabled") == undefined) {
            infosBase.getVersionAlx();
        }

        if (($("#divInfoAvenant").length && $("#EditMode").val() === "True" && !window.isReadonly) || $("#IsModifHorsAvn").val() === "True") {
            $("img[name='RechercherCourtier'][albcontext='courtierGestion']").attr("src", "/Content/Images/loupe.png").addClass("CursorPointer");
            $("#GpIdentiqueApporteur").removeAttr("checked");
            $("#CourtierApporteur_Courtier_CodeCourtier").disable(true);
            $("#CourtierApporteur_Courtier_NomCourtier").disable(true);
            $("img[name='RechercherCourtier'][albcontext='courtierApporteur']").attr("src", "/Content/Images/loupegris.png").removeClass("CursorPointer");
            $("#CourtierPayeur_Courtier_CodeCourtier").enable();
            $("#CourtierPayeur_Courtier_NomCourtier").enable();
            $("img[name='RechercherCourtier'][albcontext='courtierPayeur']").attr("src", "/Content/Images/loupe.png").addClass("CursorPointer");
        }

        infosBase.initAddAssure();

        $("#RechercheAvanceePreneurAssurance").kclick(function () {
            if ($(this).hasClass("CursorPointer")) {
                var context = $(this).attr("albcontext");
                if (context == undefined) { context = ""; }
                OpenRechercheAvanceePreneurAssurance($("#PreneurAssurance_Numero").val(), $("#PreneurAssurance_Nom").val(), context);
            }
        });
    };

    this.initAddAssure = function () {
        let codeCourant = $("#CodeContrat").val();
        if (codeCourant != undefined && codeCourant !== "") {
            $("#btnOpenAssu").kclick(function () { OpenAssuAdd(); });
        }
        else {
            $("#btnOpenAssu").hide();
        }
    };

    //----------Map le changement de sélection du contrat mère----------
    this.watchChangeContratMere = function () {
        $("#InformationBase_ContratMere").offOn("change", function () {
            infosBase.getVersionAlx();
        });
    };

    //----------Récupère le numéro d'aliment-------------
    this.getVersionAlx = function () {
        let contratMere = $("#InformationBase_ContratMere").val();
        if (contratMere == undefined || contratMere === "") {
            $("#InformationBase_NumAliment").clear();
            return;
        }
        else {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/AnCreationContrat/GetNumeroAliment",
                data: { contratMere: contratMere },
                success: function (data) {
                    if (data != undefined)
                        $("#InformationBase_NumAliment").val(data);
                    else
                        $("#InformationBase_NumAliment").clear();
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    };

    // Affecte les titles aux éléments de l'écran
    this.assignTitlesElements = function () {
        AffectTitleList($("#InformationContrat_MotClef1"));
        AffectTitleList($("#InformationContrat_MotClef2"));
        AffectTitleList($("#InformationContrat_MotClef3"));
        AffectTitleList($("#InformationBase_BrancheText"));
        AffectTitleList($("#Branche"));
        AffectTitleList($("#CourtierPayeur_Encaissement"));
    };
    //----------Map les titles sur changement de valeur des combobox
    this.initChangeCombos = function () {
        $("#CourtierPayeur_Encaissement").offOn("change", function () {
            AffectTitleList($(this));
        });

        $("#Branche").offOn("change", function () {
            AffectTitleList($(this));
        });

        $("#InformationBase_TypeContrat").offOn("change", function () {
            AffectTitleList($(this));
            infosBase.changeTypeContrat();
        });

        infosBase.initChangeMotsCles();
    };

    this.initChangeMotsCles = function () {
        $("#InformationContrat_MotClef1, #InformationContrat_MotClef2, #InformationContrat_MotClef3").offOn("change", function () {
            AffectTitleList($(this));
        });
    };

    //----------Map les éléments de recherche avancée--------------
    this.initRechercheAvanceeCourtiers = function () {
        $("img[name=RechercherCourtier].CursorPointer").kclick(function () {
            let context = $(this).attr("albcontext");
            $("#divDataRechercheAvanceeCourtierCreationCourtier").html($("div[name=RechercherCourtier][albcontext=" + context + "]").html());
            AlbScrollTop();
            $("#divRechercheAvanceeCourtierCreationContrat").show();

            RechercheAvanceeCourtier(context);
            AlternanceLigne("CabinetsCourtageBody", "Code", true, null);
            CloseRechAvancee();

            if ($("#CabinetCourtageNom").hasVal()) {
                $("#NomCabinetRecherche").val($("#CabinetCourtageNom").val());
            }

            if ($("#CabinetCourtageId").hasVal()) {
                $("#CodeCabinetRecherche").val($("#CabinetCourtageId").val());
            }

            if ($("#CabinetCourtageNom").hasVal() || $("#CabinetCourtageId").hasVal()) {
                $("#RechercherCabinetCourtierButton").trigger("click");
            }
            FormatNumericCodeCourtier();
        });
    };
    //----------------------Annulation de la form et retour à la recherche---------------------
    this.cancel = function () {
        if ($("#EditMode").val() === "True") {
            let codeOffre = $("#Offre_CodeOffre").val();
            let version = $("#Offre_Version").val();
            let type = $("#Offre_Type").val();
            let addParamType = $("#AddParamType").val();
            let addParamValue = $("#AddParamValue").val();
            let addParam = "";
            if (addParamType !== "" && addParamValue !== "") {
                addParam = "addParam" + addParamType + "|||" + addParamValue + "addParam";
            }
            let param = codeOffre + "_" + version + "_" + type + $("#tabGuid").val() + addParam + GetFormatModeNavig($("#ModeNavig").val());
            infosBase.redirect("AnInformationsGenerales", "Index", param);
        }
        else {
            infosBase.redirect("RechercheSaisie", "Index", "");
        }
    };

    this.initChangeRemplace = function () {
        $("#ContratRemplace").offOn("change", function () {
            infosBase.changeCheckBox($(this));
        });
    };

    this.watchGpIdentiqueApporteur = function () {
        $("#GpIdentiqueApporteur").offOn("change", function () {
            infosBase.changeCheckBox($(this));
        });
    };
    //----------------------Active les controles correspondant au checkbox de Contrat remplacé---------------------
    this.changeCheckBox = function (e) {
        let currentId = e.attr("id");
        let currentCheck = e.is(":checked");
        let numContratRemp = $("#InformationBase_NumContratRemplace");
        let numAlimentRemp = $("#InformationBase_NumAlimentRemplace");
        let input1 = "";

        if (currentId === "ContratRemplace") {
            input1 = $("img[id=btnRechContrat]");

            if (currentCheck) {
                input1.attr("src", "/Content/Images/loupe.png");
                input1.addClass("CursorPointer");

                numContratRemp.enable();
                numAlimentRemp.enable();
            }
            else {
                input1.attr("src", "/Content/Images/loupegris.png");
                input1.removeClass("CursorPointer");
                numContratRemp.disable(true);
                numContratRemp.clear();
                numAlimentRemp.disable(true);
                numAlimentRemp.clear();
            }
        }
        else if (currentId === "GpIdentiqueApporteur") {
            input1 = $("#CourtGestPay");
            if (currentCheck) {
                $("#CodeCourtierInvalideGestionnaireDiv").clearHtml();
            }
            updateGestionnairePayeur(false);
        }
    };
    this.watchChangeBranche = function () {
        $("#Branche[albEmplacement=creation]").offOn("change", function () {
            infosBase.loadCiblesByBranche($(this).val());
        });
    };
    //----------------------Application de la recherche ajax des cibles par branche----------------------
    this.loadCiblesByBranche = function (codeBranche) {
        let codeCourtierGest = $("#CourtierGestionnaire_Courtier_CodeCourtier").val();
        let codeCourtierApp = $("#CourtierApporteur_Courtier_CodeCourtier").val();
        let codeCourtierPay = $("#CourtierPayeur_Courtier_CodeCourtier").val();
        let isIdentical = $("#GpIdentiqueApporteur").isChecked();
        let codeAssure = $("#PreneurAssurance_Numero").val();
        $.ajax({
            type: "POST",
            url: "/AnCreationContrat/GetCibles",
            data: { codeBranche: codeBranche },
            success: function (data) {
                $("#cibleDiv").html(data);
                if ($("#hideCible").val() !== "") {
                    $("#Cible[albEmplacement=creation] option[value='" + $("#hideCible").val() + "']").attr("selected", "selected");
                    $("#hideCible").val("");
                }
                infosBase.resetInputs();
                $("#Template").clearHtml();
                infosBase.loadMotsCles(codeBranche, "");
                $("#labelInfoEtat").html("Création d'un contrat vierge");
                if (!isIdentical) $("#GpIdentiqueApporteur").uncheck();
                $("#CourtierGestionnaire_Courtier_CodeCourtier").val(codeCourtierGest).trigger("change");
                $("#CourtierApporteur_Courtier_CodeCourtier").val(codeCourtierApp).trigger("change");
                $("#CourtierPayeur_Courtier_CodeCourtier").val(codeCourtierPay).trigger("change");
                $("#PreneurAssurance_Numero").val(codeAssure).trigger("change");
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //----------------------Application de la recherche ajax des templates par cible----------------------
    this.watchChangeCible = function () {
        $("#Cible").offOn("change", function () {
            infosBase.loadTemplatesByCible(this);
        });
    };
    this.loadTemplatesByCible = function (element) {
        let codeCourtierGest = $("#CourtierGestionnaire_Courtier_CodeCourtier").val();
        let codeCourtierApp = $("#CourtierApporteur_Courtier_CodeCourtier").val();
        let codeCourtierPay = $("#CourtierPayeur_Courtier_CodeCourtier").val();
        let isIdentical = $("#GpIdentiqueApporteur").isChecked();
        let codeAssure = $("#PreneurAssurance_Numero").val();

        AffectTitleList($(element));

        ShowLoading();
        let codeBranche = $("#Branche").val();
        let codeCible = $(element).val();

        if (codeCible == "" || codeCible == undefined) {
            infosBase.loadMotsCles(codeBranche, codeCible);
            infosBase.resetInputs();
            $("#Template").clearHtml();
            $("#labelInfoEtat").html("Création d'un contrat vierge");
            CloseLoading();
        }
        else {
            $.ajax({
                type: "POST",
                url: "/AnCreationContrat/GetTemplates",
                data: { codeBranche: codeBranche, codeCible: codeCible },
                success: function (data) {
                    if (data != null && data !== "") {
                        $("#divBodyCreationContrat").html(data);
                        infosBase.mapAdresse();
                        MapCommonAutoCompSouscripteur();
                        MapCommonAutoCompGestionnaire();
                        MapCommonAutoCompAssure();
                        MapCommonAutoCompCourtier();
                        MapCommonAutoCompCourtierGestion();
                        MapCommonAutoCompCourtierPayeur();
                        infosBase.initRechercheAvanceeCourtiers();
                        infosBase.watchChangeBranche();
                        infosBase.watchChangeCible();
                        infosBase.formatDatePicker();
                        infosBase.formatTextArea();
                        infosBase.initChangeCombos();
                        infosBase.watchChangeContratMere();
                        toggleDescription($("#InformationContrat_Observation"));
                        AffectDateFormat();
                        infosBase.assignTitlesElements();
                        MapBoitesDialogue();
                        infosBase.initAddAssure();
                        $("img[name='RechercherCourtier'][albcontext='courtierGestion']").addClass("CursorPointer").attr("src", "/Content/Images/loupe.png");
                        $("div[name='btnAdresse']").addClass("CursorPointer");

                    }
                    else {
                        infosBase.resetInputs();
                        infosBase.loadMotsCles(codeBranche, codeCible);
                        $("#Template").clearHtml();
                        $("#labelInfoEtat").html("Création d'un contrat vierge");
                    }

                    if (!isIdentical) $("#GpIdentiqueApporteur").uncheck();
                    $("#CourtierGestionnaire_Courtier_CodeCourtier").val(codeCourtierGest).trigger("change");
                    $("#CourtierApporteur_Courtier_CodeCourtier").val(codeCourtierApp).trigger("change");
                    $("#CourtierPayeur_Courtier_CodeCourtier").val(codeCourtierPay).trigger("change");
                    $("#PreneurAssurance_Numero").val(codeAssure).trigger("change");

                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    };

    //---------------------Rechargement de l'écran en fonction du template sélectionné--------------------
    this.watchChangeTemplate = function () {
        $("#Template").offOn("change", function () {
            AffectTitleList($(this));
            infosBase.loadTemplate();
        });
    };

    this.loadTemplate = function () {
        let template = $("#Template").val();
        let codeBranche = $("#Branche").val();
        let codeCible = $("#Cible").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/AnCreationContrat/GetTemplatePage",
            data: { codeBranche: codeBranche, codeCible: codeCible, codeTemplate: template, modeNavig: $("#ModeNavig").val() },
            success: function (data) {
                $("#divBodyCreationContrat").html(data);
                infosBase.mapAdresse();
                MapCommonAutoCompSouscripteur();
                MapCommonAutoCompGestionnaire();
                MapCommonAutoCompAssure();
                MapCommonAutoCompCourtier();
                MapCommonAutoCompCourtierGestion();
                MapCommonAutoCompCourtierPayeur();
                infosBase.initRechercheAvanceeCourtiers();
                infosBase.formatDatePicker();
                infosBase.formatTextArea();
                infosBase.initChangeCombos();
                infosBase.watchChangeContratMere();
                toggleDescription($("#InformationContrat_Observation"));
                AffectDateFormat();
                MapBoitesDialogue();
                infosBase.initAddAssure();
                CloseLoading();
                $("img[name='RechercherCourtier'][albcontext='courtierGestion']").addClass("CursorPointer").attr("src", "/Content/Images/loupe.png");
                $("div[name='btnAdresse']").addClass("CursorPointer");

                if ($("input[name='CodeCourtierGestionnaire']").val() != undefined && $("input[name='CodeCourtierGestionnaire']").val() != "0")
                    $("input[name='CodeCourtierGestionnaire']").change();
                if ($("input[name='CodeCourtierApporteur']").val() != undefined && $("input[name='CodeCourtierApporteur']").val() != "0")
                    $("input[name='CodeCourtierApporteur']").change();
                if ($("input[name='CodeCourtierPayeur']").val() != undefined && $("input[name='CodeCourtierPayeur']").val() != "0")
                    $("input[name='CodeCourtierPayeur']").change();
                if ($("input[name='NumeroAssur']").val() != undefined && $("input[name='NumeroAssur']").val() != "0")
                    $("input[name='NumeroAssur']").change();

            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //----------------------Charge les infos des souscripteurs en fonction du nom---------------------
    ////----------------------Vérifie si un souscripteur a été sélectionné---------------------
    this.watchChangeSouscripteur = function () {
        $("#InformationBase_SouscripteurNom").offOn("change", function () {
            if ($("#SouscripteurCode").val() == "") {
                $(this).clear();
            }
        });
    };
    //----------------------Vérifie si un gestionnaire a été sélectionné---------------------
    this.watchChangeGestionnaire = function () {
        $("#InformationBase_GestionnaireNom").offOn("change", function () {
            if ($("#GestionnaireCode").val() == "") {
                $(this).clear();
            }
        });
    };

    //----------------------Active ou désactive le controle Contrat mère---------------------
    this.changeTypeContrat = function () {
        let typeContrat = $("#InformationBase_TypeContrat").val();
        let carteMere = $("#InformationBase_ContratMere");
        let numAliment = $("#InformationBase_NumAliment");
        let btnRechPolice = $("img[id=btnRechPolice]");

        if (typeContrat != "A") {
            carteMere.disable(true);
            carteMere.clear();
            numAliment.disable(true);
            numAliment.clear();
            btnRechPolice.attr("src", "/Content/Images/loupegris.png");
            btnRechPolice.removeClass("CursorPointer");
        }
        else {
            carteMere.enable();
            numAliment.enable();
            btnRechPolice.attr("src", "/Content/Images/loupe.png");
            btnRechPolice.addClass("CursorPointer");
        }
    };
    //----------------------Formate tous les controles datepicker---------------------
    this.formatDatePicker = function () {
        $(".datepicker").datepicker({ dateFormat: "dd/mm/yy" });
        $("#DateEffet").change(function () {
            if ($(this).val() != "") {
                if ($("#HeureEffetHours").val() == "") {
                    $("#HeureEffetHours").val("00");
                    $("#HeureEffetMinutes").val("00");
                    $("#HeureEffetHours").trigger("change");
                }
            }
            else {
                $("#HeureEffetHours").val("");
                $("#HeureEffetMinutes").val("");
                $("#HeureEffetMinutes").trigger("change");
            }
        });
    };
    //---------------------Affecte les fonctions sur les controles heures-----------------------
    this.watchChangeHour = function () {
        $("#HeureEffetHours").offOn("change", function () {
            infosBase.assignHour($(this));
            if ($(this).val() != "" && $("#HeureEffetMinutes").val() == "") {
                $("#HeureEffetMinutes").val("00");
            }
        });
        $("#HeureEffetMinutes").offOn("change", function () {
            infosBase.assignHour($(this));
            if ($(this).val() != "" && $("#HeureEffetHours").val() == "") {
                $("#HeureEffetHours").val("00");
            }
        });
    };
    //-------------------Renseigne l'heure---------------------------
    this.assignHour = function (elem) {
        let elemId = elem.attr("id").replace("Hours", "").replace("Minutes", "");
        let changeHour = SetHours(elemId);
        if (!changeHour && elem.val() == "") {
            $("#" + elemId + "Hours").clear();
            $("#" + elemId + "Minutes").clear();
        }
    };

    this.tryTrigger = function () {
        if ($("#PreneurAssurance_Numero").val() != "") {
            $("#PreneurAssurance_Numero").change();
        }

        if ($("#InformationBase_Branche").val() != "") {
            $("#InformationBase_Branche").change();
        }

        if ($("#CourtierGestionnaire_Courtier_CodeCourtier").val() != "") {
            $("#hideInterlocuteur").val($("#CourtierGestionnaire_Courtier_CodeInterlocuteur").val() + "_" + $("#CourtierGestionnaire_Courtier_NomInterlocuteur").val());
            $("#hideRefCourtier").val($("#CourtierGestionnaire_Courtier_Reference").val());

            $("#CourtierGestionnaire_Courtier_CodeCourtier").change();
        }
    };
    //----------------------Validation du formulaire---------------------
    this.initNext = function () {
        $("#btnSuivant:not(:disabled)").kclick(function (evt) {
            evt.preventDefault();
            infosBase.next();
        });
    };
    this.next = function () {
        if (!window.isReadonly || window.isModifHorsAvenant) {
            // enlève les class de contour des input
            $(".requiredField").removeClass("requiredField");
            $("#CodeCourtierInvalideGestionnaireDiv").clearHtml();
            $("#InformationBase_SouscripteurNom").val($.trim($("#InformationBase_SouscripteurNom").val()));
            $("#InformationBase_GestionnaireNom").val($.trim($("#InformationBase_GestionnaireNom").val()));
            let hasErrors = false;
            const filtermha = window.isModifHorsAvenant ? "[albhorsavn]" : "";
            const inputs = [
                "#Branche[albEmplacement=creation]" + filtermha,
                "#Cible[albEmplacement=creation]" + filtermha,
                "#InformationBase_TypeContrat" + filtermha,
                "#InformationBase_SouscripteurNom" + filtermha,
                "#InformationBase_GestionnaireNom" + filtermha,
                "#CourtierGestionnaire_Courtier_CodeCourtier" + filtermha,
                "#CourtierApporteur_Courtier_CodeCourtier" + filtermha,
                "#CourtierPayeur_Courtier_CodeCourtier" + filtermha,
                "#CourtierPayeur_Encaissement" + filtermha,
                "#DateAccord" + filtermha,
                "#DateEffet" + filtermha,
                "#HeureEffetHours" + filtermha,
                "#HeureEffetMinutes" + filtermha,
                "#PreneurAssurance_Numero" + filtermha,
                "#InformationContrat_Descriptif" + filtermha,
                "#ContactAdresse_CodePostal" + filtermha
            ];

            inputs.filter(function (x) { return $(x).exists(); }).forEach(function (x) {
                if (!$(x).hasVal() && x !== "#ContactAdresse_CodePostal") {
                    $(x).addClass("requiredField");
                }
                else {
                    let key = x.substr(0, x.length - filtermha.length);
                    switch (key) {
                        case "#CourtierGestionnaire_Courtier_CodeCourtier":
                            if ($("#inInvalidCourtierGest").hasVal()) {
                                $(x).addClass("requiredField");
                            }
                            break;
                        case "#CourtierApporteur_Courtier_CodeCourtier":
                            if ($("#inInvalidCourtierApp").hasVal()) {
                                $(x).addClass("requiredField");
                            }
                            break;
                        case "#CourtierPayeur_Courtier_CodeCourtier":
                            if ($("#inInvalidCourtierPay").hasVal()) {
                                $(x).addClass("requiredField");
                            }
                            break;
                        case "#PreneurAssurance_Numero":
                            if ($("#inInvalidPreneurAssu").hasVal()) {
                                $(x).addClass("requiredField");
                            }
                            break;
                        case "#ContactAdresse_CodePostal":
                            if (isNaN($(x).val())) {
                                $(x).addClass("requiredField");
                            }
                            break;
                        case "#DateEffet":
                        case "#DateAccord":
                            if ($("#DateEffet").hasVal() && $("#DateAccord").hasVal()) {
                                if (!isDate($("#DateAccord").val() || !isDate($("#DateEffet").val()))) {
                                    $("#DateAccord").addClass("requiredField");
                                    $("#DateEffet").addClass("requiredField");
                                }
                            }
                            break;
                    }
                }
            });
            hasErrors = inputs.some(function (x) { return $(x).hasClass("requiredField"); });

            if (!window.isModifHorsAvenant) {
                if (!$("#GpIdentiqueApporteur").isChecked() && (!$("#CourtierApporteur_Courtier_CodeCourtier").hasVal() || !$("#CourtierPayeur_Courtier_CodeCourtier").hasVal())) {
                    if (!$("#CourtierApporteur_Courtier_CodeCourtier").hasVal()) {
                        $("#CourtierApporteur_Courtier_CodeCourtier").addClass("requiredField");
                        $("#CourtierApporteur_Courtier_NomCourtier").addClass("requiredField");
                    }
                    if (!$("#CourtierPayeur_Courtier_CodeCourtier").hasVal()) {
                        $("#CourtierPayeur_Courtier_CodeCourtier").addClass("requiredField");
                        $("#CourtierPayeur_Courtier_NomCourtier").addClass("requiredField");
                    }
                    $("#CodeCourtierInvalideGestionnaireDiv").clearHtml();
                    $("#CodeCourtierInvalideGestionnaireDiv").append("Code Gestionnaire et Payeur sont obligatoires");
                    hasErrors = true;
                }

                if ($("#InformationBase_TypeContrat").val() == "A" && !$("#InformationBase_ContratMere").hasVal()) {
                    $("#InformationBase_ContratMere").addClass("requiredField");
                    hasErrors = true;
                }
                if ($("#InformationBase_TypeContrat").val() == "A" && !$("#InformationBase_NumAliment").hasVal()) {
                    $("#InformationBase_NumAliment").addClass("requiredField");
                    hasErrors = true;
                }

                if ($("#ContratRemplace" + filtermha).exists() && $("#ContratRemplace").isChecked()) {
                    if (!$("#InformationBase_NumContratRemplace").hasVal()) {
                        $("#InformationBase_NumContratRemplace").addClass("requiredField");
                        hasErrors = true;
                    }
                    if (!$("#InformationBase_NumAlimentRemplace").hasVal()) {
                        $("#InformationBase_NumAlimentRemplace").addClass("requiredField");
                        hasErrors = true;
                    }
                }
            }

            if (hasErrors) {
                return false;
            }
        }
        $(this).disable();
        infosBase.submitForm();
    };

    //----------------------Envoi de la form pour l'enregistrement---------------------
    this.submitForm = function () {
        let enabled = $(["#tabGuid",
            "#Offre_CodeOffre",
            "#Offre_Version",
            "#Offre_Type",
            "#txtSaveCancel",
            "#txtParamRedirect",
            "#CopyMode",
            "#TemplateMode",
            "#CodeContratCopy",
            "#VersionCopy",
            "#ModeNavig",
            "#AddParamType",
            "#AddParamValue",
            "#HeureEffetHours",
            "#HeureEffetMinutes",
            "#NumAvenant",
            "#DateEffetAvenant",
            "#HeureEffetAvenant"].join(",")).enable();

        // supprimer l'attribut "disabled" s'il existe du courtier apporteur et payeur
        let courtierApporteurDisabledElements = $("input[albcontext='courtierApporteur']:disabled").enable();
        let courtierPayeurDisabledElements = $("input[albcontext='courtierPayeur']:disabled").enable();
        let isModifHorsAvn = $("#IsModifHorsAvn").val() === "True";

        if (isModifHorsAvn) {
            $("input:not([albhorsavn])").enable();
            $("select:not([albhorsavn])").enable();
        }

        let formDataInitial = JSON.stringify($(":input").serializeObject());
        let formData = formDataInitial.replace("ContratInfoBase.CodeContrat", "CodeContrat").replace("ContratInfoBase.VersionContrat", "VersionContrat").replace("ContratInfoBase.Type", "Type").replace("Contrat.AddParamType", "AddParamType").replace("Contrat.AddParamValue", "AddParamValue").replace("Contrat.DateEffetAvenant", "DateEffetAvenant");
        formData = formData.replace(/ContactAdresse./ig, "ContactAdresse_");

        // ajouter l'attribut "disabled" s'il existe du courtier apporteur et payeur
        courtierApporteurDisabledElements.disable();
        courtierPayeurDisabledElements.disable();
        $("#lnkBlackList").hide();
        $("#idBlacklistedPartenaire").clear();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/AnCreationContrat/UpdateContrat",
            data: formData,
            contentType: "application/json",
            success: function () {
                $("#btnSuivant").enable();
            },
            error: function (request) {
                let result = null;
                try {
                    result = JSON.parse(request.responseText);
                } catch (e) { result = null; }
                if (!kheops.alerts.blacklist.displayAll(result)) {
                    common.error.showXhr(request);
                }
                $("#btnSuivant").enable();
                enabled.disable();
            }
        });

        if (isModifHorsAvn) {
            $("input:not([albhorsavn])").disable();
            $("select:not([albhorsavn])").disable();
        }
    };
    //----------------Evenement liés au buton btnGestionnairePayeur-----
    this.initBtnGestPayeur = function () {
        $("div[name=btnGestionnairePayeur]").click(function () {
            if (!window.isReadonly)
                infosBase.loadGestionnairePayeur();
        });
        $("div[name=btnGestionnairePayeur]").offOn("mouseover", function () {
            var position = $(this).offset();
            var top = position.top;
            $("#divInfoGestionnairePayeur").css({ 'position': "absolute", 'top': top + "px", 'left': position.left + 303 + "px" }).show();
        });
        $("div[name=btnGestionnairePayeur]").offOn("mouseout", function () {
            $("#divInfoGestionnairePayeur").hide();
        });
    };
    //-----------------Ouvre la div flottante pour la définition des courtiers Gestionnaire et Payeur-----
    this.loadGestionnairePayeur = function () {
        let codeGestionnaire = $("#CourtierGestionnaire_Courtier_CodeCourtier").val();
        let nomGestionnaire = $("#CourtierApporteur_Courtier_NomCourtier").val();
        let nomInterlocuteur = $("#CourtierGestionnaire_Courtier_NomInterlocuteur").val();
        let refInterlocuteur = $("#CourtierGestionnaire_Courtier_Reference").val();
        let codeInterlocuteur = $("#CourtierGestionnaire_Courtier_CodeInterlocuteur").val();
        let codePayeur = $("#CourtierPayeur_Courtier_CodeCourtier").val();
        let nomPayeur = $("#CourtierPayeur_Courtier_NomCourtier").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/AnCreationContrat/LoadGestionnairePayeur",
            data: {
                codeGestionnaire: codeGestionnaire, nomGestionnaire: nomGestionnaire, nomInterlocuteur: nomInterlocuteur, refInterlocuteur: refInterlocuteur, codeInterlocuteur: codeInterlocuteur,
                codePayeur: codePayeur, nomPayeur: nomPayeur
            },
            success: function (data) {
                $("#divDataGestionnairePayeur").html(data);
                AlbScrollTop();
                $("#divGestionnairePayeur").show();
                common.page.isLoading = false;
                infosBase.initDivGestPay();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //----------Map les élements de la div flottante GestionnairePayeur---------
    this.initDivGestPay = function () {
        MapCommonAutoCompCourtierGestion();
        MapCommonAutoCompCourtierPayeur();
        $("#btnValider").kclick(function () {
            $("#CodeCourtierInvalideGestionnaireDiv").clearHtml();
            infosBase.validateGestionnairePayeur();
        });
        $("#btnAnnulerDivF").kclick(function () {
            $("#divGestionnairePayeur").hide();
        });
    };
    //--------Valider Gestionnaire et Payeur---------
    this.validateGestionnairePayeur = function () {
        let erreurBool = false;
        let codeGestionnaire = $("#DivFCodeGest").val();
        let nomGestionnaire = $("#DivFNomGest").val();
        let nomInterlocuteur = $("#DivFNomInterlocuteur").val();
        let refInterlocuteur = $("#DivFReference").val();
        let codeInterlocuteur = $("#DivFCodeInterlocuteur").val();
        let codePayeur = $("#DivFCodePay").val();
        let nomPayeur = $("#DivFNomPay").val();

        if (codeGestionnaire === "") {
            $("#DivFCodeGest").addClass("requiredField");
            erreurBool = true;
        }
        if (codePayeur === "") {
            $("#DivFCodePay").addClass("requiredField");
            erreurBool = true;
        }
        if (!$("#CourtierInvalideGestionnaireDiv").is(":empty") || !$("#CourtierInvalidePayeurDiv").is(":empty")) {
            erreurBool = true;
        }
        if (erreurBool) {
            return false;
        }
        if (codeInterlocuteur == "") { codeInterlocuteur = 0; }
        common.page.isLoading = true;
        let data = {
            codeGestionnaire: codeGestionnaire, nomGestionnaire: nomGestionnaire,
            nomInterlocuteur: nomInterlocuteur, refInterlocuteur: refInterlocuteur, codeInterlocuteur: codeInterlocuteur,
            codePayeur: codePayeur, nomPayeur: nomPayeur
        };
        common.$postJson("/AnCreationContrat/ValiderGestionnairePayeur", data).done(function (result) {
            $("#GestPay").html(result);
            $("#divGestionnairePayeur").hide();
            common.page.isLoading = false;
        });
    };
    //----------------Redirection------------------
    this.redirect = function (cible, job, param) {
        common.$postJson("/AnCreationContrat/Redirection", { cible: cible, job: job, param: param }).done();
    };

    //----Mappe les elements relatifs à la recherche du contrat mère------
    this.initSearchContratMere = function () {
        $("#btnAnnulerRechercheContrat").kclick(function () {
            $("#divRecherche").hide();
            $("#divRechercheContrat").hide();
            ReactivateShortCut("rechercheContrat");
        });
        $("#btnAnnulerResultatRechercheContrat").kclick(function () {
            $("#divRecherche").hide();
            $("#divRechercheContrat").hide();
            ReactivateShortCut("rechercheContrat");
        });
        $("#btnSelectionnerContrat").kclick(function () {
            infosBase.selectContrat();
        });
        $("img[id=btnRechPolice]").kclick(function () {
            let img = $("img[id=btnRechPolice]").attr("src").substr($("img[id=btnRechPolice]").attr("src").lastIndexOf("/") + 1);
            if (img == "loupe.png") { infosBase.displaySearchContrat("M"); }
        });
    };

    this.initSearchContratRemplace = function () {
        $("img[id=btnRechContrat]").kclick(function () {
            let img = $("img[id=btnRechContrat]").attr("src").substr($("img[id=btnRechContrat]").attr("src").lastIndexOf("/") + 1);
            if (img == "loupe.png") { infosBase.displaySearchContrat("R"); }
        });
    };

    this.displaySearchContrat = function (type) {
        $("#divRechercheContrat").show();
        $("#typeRecherche").val(type);
        DesactivateShortCut("rechercherContrat");
        ReactivateShortCut("resultatRecherche");
        $("#btnInitialize").click();
        if ($("#CritereParam").val() == "ContratOnly") {
            $("#OffreId").val($("#InformationBase_NumContratRemplace").val());
        }
    };

    //-----Sélectionne le contrat-----
    this.selectContrat = function () {
        //----contrat sélectionné :
        let contratSelectionne = $("#selectedOffreContrat").val();
        if ($("#typeRecherche").val() == "M") {
            $("#InformationBase_ContratMere").val(contratSelectionne.split("_")[0]);
            infosBase.getVersionAlx();
        }
        else {
            $("#InformationBase_NumContratRemplace").val(contratSelectionne.split("_")[0]);
            $("#InformationBase_NumAlimentRemplace").val(contratSelectionne.split("_")[3]);
        }

        $("#divRecherche").hide();
        $("#divRechercheContrat").hide();
        ReactivateShortCut("rechercheContrat");
    };

    this.formatTextArea = function () {
        if (!window.isReadonly) {
            // Mettre une div pour simuler un textarea, pour prendre en compte le format HTML
            $("#InformationContrat_Observation").html($("#InformationContrat_Observation").html().replace(/&lt;br&gt;/ig, "\n"));
        }
    };

    this.refreshDataCourtiers = function () {
        if ($("#GpIdentiqueApporteur").isChecked() && !window.isReadonly) {
            let code = $("#CourtierGestionnaire_Courtier_CodeCourtier").val();
            let nom = $("#CourtierGestionnaire_Courtier_NomCourtier").val();
            $("#CourtierApporteur_Courtier_CodeCourtier").val(code);
            $("#CourtierApporteur_Courtier_NomCourtier").val(nom);
            $("#CourtierPayeur_Courtier_CodeCourtier").val(code);
            $("#CourtierPayeur_Courtier_NomCourtier").val(nom);
            $("#CourtierApporteur_Courtier_CodeCourtier").change();
            $("#CourtierPayeur_Courtier_CodeCourtier").change();
        }
    };

    // Remet à zero les éléments de la page
    this.resetInputs = function () {
        $(["#DateEffet",
            "#HeureEffetHours",
            "#HeureEffetMinutes",
            "#DateAccord",
            "#InformationContrat_Descriptif",
            "#InformationBase_SouscripteurNom",
            "#InformationBase_GestionnaireNom",
            "#CourtierGestionnaire_Courtier_CodeCourtier",
            "#CourtierGestionnaire_Courtier_NomCourtier",
            "#CourtierGestionnaire_Courtier_NomInterlocuteur",
            "#CourtierGestionnaire_Courtier_Reference",
            "#CourtierPayeur_Encaissement",
            "#CourtierApporteur_Courtier_CodeCourtier",
            "#CourtierApporteur_Courtier_NomCourtier",
            "#CourtierPayeur_Courtier_CodeCourtier",
            "#CourtierPayeur_Courtier_NomCourtier",
            "#PreneurAssurance_Numero",
            "#PreneurAssurance_Nom",
            "#InformationContrat_MotClef1",
            "#InformationContrat_MotClef2",
            "#InformationContrat_MotClef3",
            "#PreneurAssurance_Departement",
            "#PreneurAssurance_Ville",
            "#ContactAdresse_Batiment",
            "#ContactAdresse_No",
            "#ContactAdresse_Extension",
            "#ContactAdresse_Voie",
            "#ContactAdresse_Distribution",
            "#ContactAdresse_CodePostal",
            "#ContactAdresse_Ville",
            "#ContactAdresse_CodePostalCedex",
            "#ContactAdresse_VilleCedex",
            "#ContactAdresse_Pays",
            "#InformationBase_ContratMere",
            "#InformationBase_NumAliment",
            "#CodeOffreCopy"].join(",")).clear();

        $(["#CourtierInvalideGestionnaireDiv",
            "#CourtierInvalideDiv",
            "#CourtierInvalidePayeurDiv",
            "#zoneTxtArea",
            "#Description_Observation"].join(",")).clearHtml();

        $("#GpIdentiqueApporteur, #PreneurEstAssure").check();

        $("#InformationBase_TypeContrat").selectVal("S");
        $("#CopyMode").val("False");
    };

    this.loadMotsCles = function (codeBranche, codeCible) {
        $.ajax({
            type: "POST",
            url: "/AnCreationContrat/GetListeMotsCles",
            data: { codeBranche: codeBranche, codeCible: codeCible },
            success: function (data) {
                $("#divMotsCles").html(data);
                infosBase.initChangeMotsCles();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };
};

var infosBase = new InfosBaseAffaireNouvelle();

$(document).ready(function () {
    infosBase.mapAdresse();
    infosBase.initPage();
    infosBase.initChangeRemplace();
    infosBase.watchGpIdentiqueApporteur();
    infosBase.formatTextArea();
    infosBase.watchChangeBranche();
    infosBase.watchChangeCible();
    infosBase.watchChangeTemplate();
    MapCommonAutoCompSouscripteur();
    MapCommonAutoCompGestionnaire();
    MapCommonAutoCompAssure();
    MapCommonAutoCompCourtier();
    MapCommonAutoCompCourtierGestion();
    MapCommonAutoCompCourtierPayeur();
    infosBase.watchChangeSouscripteur();
    infosBase.watchChangeGestionnaire();
    infosBase.formatDatePicker();
    infosBase.watchChangeHour();
    infosBase.tryTrigger();
    infosBase.initNext();
    infosBase.initBtnGestPayeur();
    infosBase.initSearchContratMere();
    infosBase.initSearchContratRemplace();
    infosBase.initChangeCombos();
    infosBase.watchChangeContratMere();
    AffectDateFormat();
});
