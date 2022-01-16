
var Quittance = (function () {
    function Quittance() { }

    Quittance.prototype.initVisuCotisations = function () {
        $('#divCotisations #VisuQuittanceDateEmission').removeAttr('disabled').removeAttr('readonly');
        $('#divCotisations #VisuQuittanceTypeOperation').removeAttr('disabled');
        $('#divCotisations #VisuQuittanceSituation').removeAttr('disabled');
        $('#divCotisations #VisuQuittancePeriodeDebut').removeAttr('disabled').removeAttr('readonly');
        $('#divCotisations #VisuQuittancePeriodeFin').removeAttr('disabled').removeAttr('readonly');
        $("#btnFermerVisualisationQuittance").hide();
        $('#divCotisations .divHeightVisuquittances').css('height', '294px');

        $("#divCotisations #VisuQuittanceDateEmission, #divCotisations #VisuQuittanceTypeOperation, #divCotisations #VisuQuittanceSituation, #divCotisations #VisuQuittancePeriodeDebut, #divCotisations #VisuQuittancePeriodeFin").die().live("change", function () {
            FiltrerVisualisationQuittances(null, true);
        });
        MapElementVisuQuittancesTableau(true);

        $("#divCotisations #VisuQuittanceDateEmission").datepicker({ dateFormat: 'dd/mm/yy' });
        $("#divCotisations #VisuQuittancePeriodeDebut").datepicker({ dateFormat: 'dd/mm/yy' });
        $("#divCotisations #VisuQuittancePeriodeFin").datepicker({ dateFormat: 'dd/mm/yy' });
    };
    //----------------Map les éléments de la page------------------
    Quittance.prototype.initPage = function () {
        const self = this;
        $("button[type='button'][id='GererEcheancier'][albContext='']").kclick(function () {
            OpenEcheancier("", $(this).attr("albcontext"));
        });
        $("button[type='button'][id='btnDetails'][albContext='']").enable().kclick(function () {
            AfficherDetailsCotisation();
        });
        $("#btnFraisAccess").kclick(function () {
            AfficherFraisAccessoires();
        });
        $('#btnDetailCot').kclick(function () {
            AfficherCoCourtiers();
        });
        $("#btnCalculForce").kclick(function () {
            self.openCalculForce();
        });
        $("#LienQuittance").kclick(function () {
            OpenVisulisationQuittances('Toutes', isEntete = false);
        });

        AlternanceLigne("BodyCommissionL", "", false, null);
        AlternanceLigne("BodyCommissionR", "", false, null);

        $("#btnAnnuler").kclick(function () {
            ShowCommonFancy("ConfirmQuitt", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        });
        $("#btnFullScreen").kclick(function () {
            self.openCloseFullScreenListFormules(true);
        });
        $("#dvLinkClose").kclick(function () {
            self.openCloseFullScreenListFormules(false);
        });
        FormatDecimalValueWithNegative();
        toggleDescription($("#CommentForce"), true);

        if ($("#OffreReadOnly").val() == "False") {
            $("#CommentForce").html($("#CommentForce").html().replace(/&lt;br&gt;/ig, "\n"));
        }

        if (window.isReadonly) {
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }

        if ($("#NumAvenantPage").hasVal() && $("#NumAvenantPage").val() != "0") {
            $("#chkEmission").kclick(function () {
                if (!window.isReadonly) {
                    self.traceEmissionAvenant($(this).isChecked());
                }
            });
        }

        $("#lblEcheance").offOn("mouseout", function () {
            $("#divInfoEcheance").hide();
        });
        $("#lblEcheance").offOn("mouseover", function () {
            var position = $(this).offset();
            var top = position.top;
            $("#divInfoEcheance").css({ 'position': 'absolute', 'top': top + 15 + 'px', 'left': position.left - 50 + 'px' }).show();
        });
    };
    //---------------Suppression de toutes les écheances du contrat--------
    Quittance.prototype.deleteEcheances = function () {
        ShowCommonFancy("ConfirmQuitt", "Suppr",
            "Attention, il existe un échéancier, cette action va le supprimer définitivement. Voulez-vous continuer ?",
            350, 130, true, true);
    };
    //---------------Mettre à jour la périodicité du contrat-------
    Quittance.prototype.updatePeriodicite = function () {
        var periodicite = "E";
        if ($("button[type='button'][id='GererEcheancier'][albContext='']").is(':checked'))
            periodicite = "E";
        else
            periodicite = "U";

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var tabGuid = $("#tabGuid").val();
        var codeAvn = $("#NumAvenantPage").val();
        $.ajax({
            type: "POST",
            url: "/Quittance/UpdatePeriodicite",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, periodicite: periodicite, tabGuid: tabGuid
            },
            success: function (data) {
                $("#PeriodiciteContrat").val(periodicite);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });

    };

    //--------------Vérifie les données des co-courtiers-----------------------
    Quittance.prototype.redirectToSpecificPage = function (returnHome) {
        common.page.isLoading = true;
        if (returnHome) {
            $("#txtSaveCancel").val("1");
            var tabGuid = $("#tabGuid").val();
            this.redirectToQuit("ChoixClauses", "Index", true);
            DeverouillerUserOffres(tabGuid);
            $("#txtSaveCancel").val('0');
        }
        else {
            this.redirectToQuit("ChoixClauses", "Index");
        }
    };

    Quittance.prototype.updateFraisAccessoires = function () {
        const self = this;
        var retourControl = true;
        $("#FraisRetenus").removeClass('requiredField');
        $("div[id=zoneTxtArea][albcontext=CommentairesFraisAcc]").removeClass('requiredField');
        $("#FraisSpecifiques").removeClass('requiredField');

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var typeFrais = $("#TypeFrais").val();
        var fraisRetenus = $("#FraisRetenus").val();
        var taxeAttentat = $("#TaxeAttentat").is(':checked');
        var effetAnnee = $("#EffetAnnee").val();
        var codeCommentaires = $("#CodeCommentaires").val();
        var commentaires = $("#CommentairesFraisAcc").val();
        var tabGuid = $("#tabGuid").val();
        var acteGestion = $("#ActeGestion").val();
        var isModifHorsAvn = $("#IsModifHorsAvn").val() === "True";

        //Contrôles
        if (typeFrais == "P" && fraisRetenus == "0") {
            ShowCommonFancy("Error", "", "Frais retenus doit être supérieur à 0", 1212, 700, true);
            return;
        }
        if (typeFrais == "S") {
            fraisRetenus = $("#FraisStandards").val();
        }
        if (fraisRetenus == "") {
            if (typeFrais == "S") $("#FraisStandards").addClass('requiredField');
            else $("#FraisRetenus").addClass('requiredField');
            retourControl = false;
        }

        var commentaireNonFormate = commentaires.replace(/<\/p><p>/ig, "").replace(/<\/p>/ig, "").replace(/<p>/ig, "").replace(/&nbsp;/ig, "").replace(/<br>/ig, "");
        if ((typeFrais == "P") && $.trim(commentaireNonFormate) == "") {
            $("div[id=zoneTxtArea][albcontext=CommentairesFraisAcc]").addClass('requiredField');
            retourControl = false;
        }
        if (retourControl == false) {
            return;
        }
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: $("#NumAvenantPage").val(), effetAnnee: effetAnnee,
                typeFrais: typeFrais, fraisRetenus: fraisRetenus, taxeAttentat: taxeAttentat,
                codeCommentaires: codeCommentaires, commentaires: commentaires, tabGuid: tabGuid, modeNavig: $("#ModeNavig").val(),
                acteGestion: acteGestion, acteGestionRegule: $("#ActeGestionRegule").val(), reguleId: $("#ReguleId").val()
            },
            url: "/Quittance/UpdateFraisAccessoires",
            success: function (data) {
                if (!isModifHorsAvn) {
                    $("#divCotisations").html(data);
                    AlternanceLigne("BodyCommission", "", false, null);
                    AlternanceLigne("FormulesBody", "", false, null);
                    AlternanceLigne("FormulesBodyPleinEcran", "", false, null);
                    AlternanceLigne("BodyDetailsTotauxBase", "", false, null);

                    FormatDecimalValue();
                    self.initPage();
                }
                toggleDescription($("#CommentForce"), true);
                $("div[id=zoneTxtArea][albcontext=CommentForce]").html(commentaires);
                $("#CommentForce").html(commentaires);

                $("#divDataFraisAccessoires").clearHtml();
                $("#divFraisAccessoires").hide();
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };
    //------------Suivant--------------
    Quittance.prototype.initSuivant = function () {
        const self = this;
        $("#btnSuivant").kclick(function (evt, data) {
            //Obligé de faire un test pour savoir si le bouton est disabled ou non (à cause d'IE)
            if (!$(this).attr('disabled')) {
                let returnHome = data && data.returnHome;
                if (!window.isReadonly) {
                    if ($("#CodeEncaissementContrat").val() != "D") {
                        let codeOffre = $("#Offre_CodeOffre").val();
                        let version = $("#Offre_Version").val();
                        let type = $("#Offre_Type").val();
                        let codeAvn = $("#NumAvenantPage").val();
                        let acteGestion = $("#ActeGestion").val();
                        $.ajax({
                            type: "POST",
                            url: "/Quittance/CheckCoCourtier/",
                            data: {
                                codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, isReadonly: $("#OffreReadOnly").val(), modeNavig: $("#ModeNavig").val(),
                                acteGestion: acteGestion, acteGestionRegule: $("#ActeGestionRegule").val()
                            },
                            success: function (data) {
                                var message = data;
                                if (message != "" && message != undefined) {
                                    if (message.split("WARNING").length > 1) {
                                        ShowCommonFancy("ConfirmQuitt", "Warning",
                                            message.split("WARNING")[1] + "<br/><br/>Etes-vous sûr de vouloir continuer ?<br/>",
                                            350, 130, true, true);
                                        $("#divButtonCourtier").addClass('requiredButton');
                                        return false;
                                    }
                                    if (message.split("ERREUR").length > 1) {
                                        var tMessage = message.split("ERREUR");
                                        if (tMessage[1] == "COM") {
                                            if ($("#CommentForce").val() == "") {
                                                common.dialog.bigError(message.split("ERREUR")[2]);
                                                $("#zoneTxtArea").addClass("requiredField");
                                                return false;
                                            }
                                        }
                                        if (tMessage[1] == "ECH") {
                                            common.dialog.bigError(message.split("ERREUR")[2]);
                                            return false;
                                        }
                                    }
                                }
                                self.saveComment(returnHome);
                            },
                            error: function (request) {
                                common.error.showXhr(request);
                                return false;
                            }
                        });
                    }
                    else {
                        self.saveComment(returnHome);
                    }
                }
                else {
                    self.redirectToSpecificPage(false);
                }
            }
        });
    };
    
    //----------------- Bouton précédent pour la régul
    Quittance.prototype.preview = function (context) {
        const self = this;
        common.page.isLoading = true;
        var addParam = context.KeyValues[context.KeyValues.length - 1].substring(context.KeyValues[context.KeyValues.length - 1].indexOf('addParam') + 8, context.KeyValues[context.KeyValues.length - 1].lastIndexOf('addParam'));
        var codeAvn = GetValueAddParam(addParam, 'AVNID');
        var typeAvt = GetValueAddParam(addParam, 'AVNTYPE');
        var lotId = GetValueAddParam(addParam, 'LOTID');

        if (lotId.trim() == "0" || lotId.trim() == "") {
            lotId = context.LotId;
        }

        var exercice = context.Exercice;
        var mode = GetValueAddParam(addParam, 'AVNMODE');

        var reguleId = GetValueAddParam(addParam, 'REGULEID');

        var codeICT = "";
        if (context.CodeICT !== null)
            codeICT = context.CodeICT.split("-")[0];

        var codeICC = "";
        if (context.CodeICC !== null)
            codeICT = context.CodeICC.split("-")[0];

        var tauxCom = context.TauxCom;
        var tauxComCATNAT = context.TauxComCATNAT;
        var codeEnc = context.CodeEnc;
        var argModeleAvtRegule = "";
        context.Step = "Cotisations";

        argModeleAvtRegule = JSON.stringify(context.ModeleAvtRegul);

        $("#NumInterne").val(codeAvn);
        let kvIgnoreReadonly = "||IGNOREREADONLY|1";
        let maxIndex = context.KeyValues.length - 1;
        let last = context.KeyValues[maxIndex];
        if (context.IsReadOnlyMode) {
            if (last.indexOf(kvIgnoreReadonly) > -1) {
                let x = last.indexOf(kvIgnoreReadonly);
                context.KeyValues[maxIndex] = last.substr(0, x) + last.substr(x + kvIgnoreReadonly.lenth);
            }
        }
        else {
            if (last.indexOf(kvIgnoreReadonly) == -1) {
                let x = last.lastIndexOf("addParam");
                context.KeyValues[maxIndex] = last.substr(0, x) + kvIgnoreReadonly + last.substr(x);
            }
        }

        var id = context.KeyValues.join("_");
        var rg = common.getRegul();

        if (rg.mode != "STAND") {
            regul.tryCreateContext(regul.nextStep);
            return;
        }
        else {
            regul.tryCreateContext();
        }

        var params = {
            lotId: lotId, reguleId: reguleId, codeContrat: context.IdContrat.CodeOffre, version: context.IdContrat.Version, type: context.IdContrat.Type,
            codeAvn: codeAvn, typeAvt: typeAvt, exercice: exercice,
            codeICT: codeICT, codeICC: codeICC, tauxCom: tauxCom, tauxComCATNAT: tauxComCATNAT, codeEnc: codeEnc, mode: mode,
            souscripteur: context.Souscripteur, gestionnaire: context.Gestionnaire,
            argModeleAvtRegule: argModeleAvtRegule,
            id: id, addParamValue: context.KeyValues[context.KeyValues.length - 1], context: context
        };

        $.ajax({
            type: "POST",
            url: "/CreationRegularisation/CancelQuittance",
            data: params,
            success: function (data) {
                if (data.Result.indexOf("SUCCESS") === -1) {
                    common.page.isLoading = false;
                    common.dialog.error("Ce contrat est verrouillé, vous ne pouvez y accéder qu'en consultation");
                    return;
                }

                var addParamValue = $("#AddParamValue").val();

                var oldRegulId = addParamValue.split('||').filter(function (element) {
                    return element.indexOf("REGULEID|") === 0 ? element : '';
                })[0];
                addParamValue = addParamValue.replace(oldRegulId, "REGULEID|" + data.IdRegule);

                var oldModeAvt = addParamValue.split('||').filter(function (element) {
                    return element.indexOf("AVNMODE|") === 0 ? element : '';
                })[0];

                if (oldModeAvt !== "AVNMODE|CONSULT") {
                    addParamValue = addParamValue.replace(oldModeAvt, "AVNMODE|UPDATE");
                }


                var oldAvnId = addParamValue.split('||').filter(function (element) {
                    return element.indexOf("AVNID|") === 0 ? element : '';
                })[0];
                addParamValue = addParamValue.replace(oldAvnId, "AVNID|" + data.NumAvn);

                // add lotid cause it might change
                addParamValue = addParamValue.replace("||LOTID|0", "||LOTID|" + $("#lotId").val());

                $("#AddParamValue").val(addParamValue);

                var redirectFromMenu = $('#txtParamRedirect').val();
                if (redirectFromMenu && redirectFromMenu.trim() !== '') {
                    RedirectionRegul(redirectFromMenu.split('/')[1], redirectFromMenu.split('/')[0], false, params);
                }

                self.leapStepsOrGoNext(data, addParamValue);

            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    Quittance.prototype.leapStepsOrGoNext = function (data, addParamValue) {
        let motif = $("#MotifAvt").val();
        let action = "Step2_ChoixRisque";
        let controller = "CreationRegularisation";


        if (data.IsSimplified) {
            addParamValue = common.addOrReplaceParam(addParamValue, 'RSQID', data.IdRsq);
            addParamValue = common.addOrReplaceParam(addParamValue, 'REGULGARID', data.RgGrId);
            addParamValue = common.addOrReplaceParam(addParamValue, 'GARID', data.IdGar);

            $("#AddParamValue").val(addParamValue);
            if (data.CodeGar.indexOf("RCFR") !== -1) {
                action = "CalculGarantiesRC";
                controller = "Regularisation";
            }
            else {
                controller = "CreationRegularisation";
                action = "Step5_RegulContrat";
            }
        }

        RedirectionRegul(action, controller, false);
    };

    //------------- Annule la form ------------------------
    Quittance.prototype.cancelForm = function () {
        // ARA  - 2516
        // Récupération des variables
        if (window.context /*&& window.context.NbRisques === 1*/) {
            //Regul PB / BNS / BURNER
            this.preview(window.context);
        }
        else {
            this.redirectToQuit("AnMontantReference", "Index");
        }
    };
    //----------------Redirection------------------
    Quittance.prototype.redirectToQuit = function (cible, job, noChoiceClauses) {
        if ((cible == "ChoixClauses" || cible == "GestionDocument") && $("#OffreReadOnly").val() != "True" && !noChoiceClauses && $("#IsModifHorsAvn").val() != "True") {
            this.generateClauses();
            return;
        }

        common.page.isLoading = true;
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var tabGuid = $("#tabGuid").val();
        var periodeDebut = $("#PeriodeDebut").val();
        var periodeFin = $("#PeriodeFin").val();
        var totalHorsFraisHT = $("#TotalHorsFraisHT").val();
        var fraisHT = $("#FraisHT").val();
        var fgaTaxe = $("#FGATaxe").val();
        var modeNavig = $("#ModeNavig").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        var acteGestion = $("#ActeGestion").val();
        var txtSaveCancel = $("#txtSaveCancel").val();
        var modeAvt = $("#ModeAvt").val();
        $.ajax({
            type: "POST",
            url: "/Quittance/Redirection/",
            data: {
                cible: cible, job: job, codeOffre: codeOffre, version: version, type: type,
                periodeDebut: periodeDebut, periodeFin: periodeFin, totalHorsFraisHT: totalHorsFraisHT, fraisHT: fraisHT, fgaTaxe: fgaTaxe,
                tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, acteGestion: acteGestion, addParamType: addParamType, addParamValue: addParamValue,
                acteGestionRegule: $("#ActeGestionRegule").val(), modeAvt: modeAvt, saveAndQuit: txtSaveCancel
            },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    Quittance.prototype.generateClauses = function () {
        const self = this;
        common.page.isLoading = true;
        const codeOffre = $("#Offre_CodeOffre").val();
        const version = $("#Offre_Version").val();
        const type = $("#Offre_Type").val();
        const tabGuid = $("#tabGuid").val();
        const periodeDebut = $("#PeriodeDebut").val();
        const periodeFin = $("#PeriodeFin").val();
        const totalHorsFraisHT = $("#TotalHorsFraisHT").val();
        const fraisHT = $("#FraisHT").val();
        const fgaTaxe = $("#FGATaxe").val();
        const modeNavig = $("#ModeNavig").val();
        const addParamType = $("#AddParamType").val();
        const addParamValue = $("#AddParamValue").val();
        const acteGestion = $("#ActeGestion").val();
        const txtSaveCancel = $("#txtSaveCancel").val();
        const modeAvt = $("#ModeAvt").val();
        $.ajax({
            type: "POST",
            url: "/Quittance/GenerateClauses/",
            data: {
                codeOffre: codeOffre, version: version, type: type,
                periodeDebut: periodeDebut, periodeFin: periodeFin, totalHorsFraisHT: totalHorsFraisHT, fraisHT: fraisHT, fgaTaxe: fgaTaxe,
                tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, acteGestion: acteGestion, addParamType: addParamType, addParamValue: addParamValue,
                acteGestionRegule: $("#ActeGestionRegule").val(), modeAvt: modeAvt, saveAndQuit: txtSaveCancel
            },
            success: function (data) {
                if (!data) {
                    if (GetValueAddParam(addParamValue, "REGULMOD") == "BNS" || GetValueAddParam(addParamValue, "REGULMOD") == "PB") {
                        self.redirectToQuit("ControleFin", "Index", true);
                    } else {
                        self.redirectToQuit("ChoixClauses", "Index", true);
                    }
                }
                else {
                    window.listeChoixClauses = data;
                    self.showDialogChoixVersion();
                    common.page.isLoading = false;
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    Quittance.prototype.showDialogChoixVersion = function () {
        let divChoixClause = $("#resListChoixClause");
        let radChoixClause = divChoixClause.find("input[type='radio']");

        //Ajout des différents choix de versions de clauses
        for (var x = 0; x < listeChoixClauses.length; x++) {
            let origine = listeChoixClauses[x].Avenant === 0 ? listeChoixClauses[x].OrigineClause == "POLICE" ? " Affaire Nouvelle" : " Offre" : " Avenant " + listeChoixClauses[x].Avenant;
            let libChoix = listeChoixClauses[x].Rub + " - " + listeChoixClauses[x].SRub + ' - ' + listeChoixClauses[x].Seq + " " + listeChoixClauses[x].Libclause + "<br/>(Version : " + listeChoixClauses[x].Version + " " + origine + ")";
            if (x === 0) {
                radChoixClause.val(x);
                radChoixClause.next().html(libChoix);
                radChoixClause.parent().after("<br />");
                if (listeChoixClauses[x].Retenue == "O") {
                    radChoixClause.check();
                }
            }
            else {
                let cloneChoix = radChoixClause.parent().clone();
                cloneChoix.find("input").val(x);
                cloneChoix.find("span").html(libChoix);
                cloneChoix.after("<br />");
                cloneChoix.find("input").prop({ checked: listeChoixClauses[x].Retenue == "O" });
                $("#listChoix").append(cloneChoix);
            }
        }

        if (divChoixClause.find("input[type='radio']:checked").length > 0) {
            divChoixClause.find("button").enable();
        }

        $("#resListChoixClause").off("change", "input[type = 'radio']").on("change", "input[type = 'radio']", function () {
            divChoixClause.find("button").enable();
        });

        divChoixClause.find("button").kclick(function () {
            common.page.isLoading = true;
            listeChoixClauses[$("#resListChoixClause input[name='choixClause']:not(:checked)").val()].Retenue = "N";
            listeChoixClauses[$("#resListChoixClause input[name='choixClause']:checked").val()].Retenue = "O";
            let idLot = listeChoixClauses[$("#resListChoixClause input[name='choixClause']:checked").val()].IdLot;
            let idClause = listeChoixClauses[$("#resListChoixClause input[name='choixClause']:checked").val()].Idunique;
            let codeOffre = $("#Offre_CodeOffre").val();
            let version = $("#Offre_Version").val();
            let type = $("#Offre_Type").val();
            let tabGuid = $("#tabGuid").val();
            let modeNavig = $("#ModeNavig").val();
            let addParamValue = $("#AddParamValue").val();
            let txtSaveCancel = $("#txtSaveCancel").val();
            let json = JSON.stringify(listeChoixClauses);
            $.ajax({
                type: "POST",
                url: "/Quittance/RedirectionChoixClauses/",
                data: {
                    codeOffre: codeOffre, version: version, type: type, idClause: idClause, idLot: idLot,
                    tabGuid: tabGuid, modeNavig: modeNavig, addParamValue: addParamValue, saveAndQuit: txtSaveCancel, json: json
                },
                success: function () { },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        });

        divChoixClause.dialog({
            title: "Choix version clauses",
            dialogClass: "no-cross-close",
            modal: true,
            position: { my: "top", at: "top", of: window },
            width: 500,
            height: 200,
            maxWidth: 600,
            maxHeight: 500,
            closeOnEscape: false,
            open: function (event, ui) {
                $(".ui-dialog-titlebar-close", ui.dialog || ui).hide();
            },
            close: function (ev, ui) {
                divChoixClause.dialog("destroy");
                return false;
            }
        });
    };

    //---------Fonction qui affiche ou ferme la div plein ecran du tableau de la liste de formules--------
    Quittance.prototype.openCloseFullScreenListFormules = function (open) {
        if (open)
            $("#divFullScreenListFormules").show();
        else $("#divFullScreenListFormules").hide();
    };

    //--------Sauvegarde le commentaire----------
    Quittance.prototype.saveComment = function (returnHome) {
        const self = this;
        $(".requiredField").removeClass("requiredField");
        let dateDeb = "";
        let dateFin = "";
        if ($("#PeriodeDebut").exists() && $("#PeriodeFin").exists()) {
            dateDeb = $("#PeriodeDebut").val() == undefined ? "" : $("#PeriodeDebut").val();
            dateFin = $("#PeriodeFin").val() == undefined ? "" : $("#PeriodeFin").val();

            //ajout du contrôle de readonly sur les champs date (bug 2183)
            let debPrdVSdateEffet = compareDates(dateDeb, $("#DateDebEffetContrat").val());
            if ((!$("#PeriodeDebut").is('[readonly]') || !$("#PeriodeFin").is('[readonly]'))
                && (!checkDate($("#PeriodeDebut"), $("#PeriodeFin"))
                    || !$("#PeriodeFin").is('[readonly]') && (!checkDate($("#PeriodeDebut"), $("#PeriodeFin")))
                    || dateDeb == "" || dateFin == ""
                    || (!checkDate($("#DateAvn"), $("#PeriodeDebut")) && $("#DateAvn").val() != "")
                    || !$("#PeriodeDebut").is('[readonly]') && (checkDateOrEqual($("#PeriodeDebut"), $("#DateDebEffetContrat")) || !checkDateOrEqual($("#PeriodeDebut"), $("#DateFinEffetContrat")))
                    || !$("#PeriodeFin").is('[readonly]') && (!checkDateOrEqual($("#PeriodeFin"), $("#DateFinEffetContrat")) || checkDateOrEqual($("#PeriodeFin"), $("#DateDebEffetContrat"))))
                && $("#ActeGestion").val() == "AVNMD") {

                common.dialog.error("Erreur dans les dates saisies");
                $("#PeriodeDebut").addClass("requiredField");
                $("#PeriodeFin").addClass("requiredField");
                return false;
            }
            else {
                $("#PeriodeDebut").removeClass("requiredField");
                $("#PeriodeFin").removeClass("requiredField");
            }
        }

        $.ajax({
            type: "POST",
            url: "/Quittance/SaveComment",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(),
                comment: $("#CommentForce").val(), acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val(), reguleId: $("#ReguleId").val(),
                modifPeriod: $("#ModifPeriode").val(), isModifDateFin: $("#IsModifDateFin").val(), dateDeb: dateDeb, dateFin: dateFin
            },
            success: function () {
                self.redirectToSpecificPage(returnHome);
            },
            error: function (request) {
                common.error.showXhr(request);
                return false;
            }
        });
    };

    //--------Ouvre la div flottante des calculs forcés----------
    Quittance.prototype.openCalculForce = function () {
        const self = this;
        self.loadMntCalcul();
        requestAnimationFrame(function () {
            $("#divCalculForce").show();
            self.prepareCalculForce();
            setTimeout(function () {
                self.initCalculForce();
            }, 5);
        });
    };
    //--------Map les éléments du calcul forcé----------
    Quittance.prototype.initCalculForce = function () {
        const self = this;
        if (($("#ActeGestion").val() == "AVNRG" && $("#ActeGestionRegule").val() != "AVNMD") || $("#ActeGestion").val() == "REGUL") {
            $("div[albtypeforce='formule']").hide();
            $("div[albtypeforce='montant']").css({ "padding-left": "67px" });
        }

        $("input[type='radio'][name='typeVal']").off("change").change(function (evt) {
            if (!evt.target.checked) {
                return;
            }
            $("input[type='radio'][name='typeHTTTC']").removeAttr("disabled");
            $("input[type='radio'][name='typeHTTTC']").uncheck();
            $("#dvCalculForceData").clearHtml();
        });

        $("input[type='radio'][name='typeHTTTC']").off("change").change(function (evt) {
            if (!evt.target.checked) {
                return;
            }
            self.loadWindowCalcul();
            $("#btnValidCalculForce").removeAttr("disabled");
        });

        $("#btnCancelCalculForce").kclick(function () {
            if ($("#OffreReadOnly").val() == "False") {
                var typeVal = $("input[type='radio'][name='typeVal']:checked").val();
                var typeHTTTC = $("input[type='radio'][name='typeHTTTC']:checked").val();
                var montantForce = "";
                var montantForceHT = "";
                if ($("#MontantForceHT").attr("id") != undefined)
                    montantForceHT = $("#MontantForceHT").autoNumeric("get");
                var montantForceTTC = "";
                if ($("#MontantForceTTC").attr("id") != undefined)
                    montantForceTTC = $("#MontantForceTTC").autoNumeric("get");

                if (typeVal == "montant") {
                    switch (typeHTTTC) {
                        case "HT":
                            montantForce = montantForceHT;
                            break;
                        case "TTC":
                            montantForce = montantForceTTC;
                            break;
                    }
                }
                if (montantForce == undefined || montantForce == "" || montantForce == "0") {
                    ReloadCotisation(false, false);
                }
                else {
                    ReloadCotisation(true, false);
                }
            }
            $("#divCalculForce").hide();
        });

        $("#btnResetCalculForce").kclick(function () {
            self.prepareCalculForce();
        });

        $("#btnValidCalculForce").kclick(function () {
            if (!$(this).is(':disabled')) {
                var typeVal = $("input[type='radio'][name='typeVal']:checked").val();
                if (typeVal == "montant") {
                    self.updateCalculForce(true);
                }
                if (typeVal == "formule") {
                    ReloadCotisation(true, true);
                }
            }
        });
    };
    //--------Initialise la div flottante des calculs forcés-------
    Quittance.prototype.prepareCalculForce = function () {
        $("input[type='radio'][name='typeVal']").uncheck();
        $("input[type='radio'][name='typeHTTTC']").uncheck().disable();
        $("#btnValidCalculForce").disable();
        $("#dvCalculForceData").clearHtml();
        if (window.isReadonly) {
            $("#btnResetCalculForce").hide();
        }
        else {
            $("#btnResetCalculForce").show();
        }
    };
    //--------Ouvre la fenêtre de calcul correspondant aux choix sélectionnés-------
    Quittance.prototype.loadWindowCalcul = function () {
        const self = this;
        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let avenant = $("#NumAvenantPage").val();
        let typeVal = $("input[type='radio'][name='typeVal']:checked").val();
        let typeHTTTC = $("input[type='radio'][name='typeHTTTC']:checked").val();

        common.page.isLoading = true;

        $.ajax({
            type: "POST",
            url: "/Quittance/LoadCalculWindow",
            data: {
                codeOffre: codeOffre, version: version, avenant: avenant, typeVal: typeVal, typeHTTTC: typeHTTTC,
                modeNavig: $("#ModeNavig").val(), acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val()
            },
            success: function (data) {
                $("#dvCalculForceData").html(data);
                //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
                common.autonumeric.apply($("input[albmask='calculdecimal']"), 'init', 'decimal', ' ', ',', 2, '999999999.99', '-999999999.99');
                common.autonumeric.apply($("span[albmask='calculdecimal']"), 'init', 'decimal', ' ', ',', 2, '999999999.99', '-999999999.99');
                //FormatDecimal("calculdecimal", " ", 2, '999999999.99', '-999999999.99');
                if (typeVal == "montant") {
                    switch (typeHTTTC) {
                        case "HT":
                            $("#MontantForceHT").show();
                            break;
                        case "TTC":
                            $("#MontantForceTTC").show();
                            break;
                    }
                }
                if (typeVal == "formule") {
                    self.initFormuleForceElement();
                }
                if ($("#inCalculForce").val() != "")
                    $("#forceMontantCal").addClass("greenValue");
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    Quittance.prototype.loadMntCalcul = function () {
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var avenant = $("#NumAvenantPage").val();
        common.page.isLoading = true;

        $.ajax({
            type: "POST",
            url: "/Quittance/LoadMntCalcul",
            data: {
                codeOffre: codeOffre, version: version, avenant: avenant,
                modeNavig: $("#ModeNavig").val(), acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val()
            },
            success: function (data) {
                if (data == "N" || data == "") {
                    $("#valMontant").attr("disabled", "disabled");
                }
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //----------Map les éléments de la div des formules forcées-----------
    Quittance.prototype.initFormuleForceElement = function () {
        const self = this;
        $("img[name='updForm']").kclick(function () { self.loadInfoFormule($(this).attr('albFormId'), $(this).attr('albNumFor')); });
        $("img[name='garForce']").click(function () { self.loadGarantieFormule($(this).attr('albFormId')); });
        $("#btnCancelCalculForceForm").kclick(function () {
            $("#CodeRsqForce").val("");
            $("#CodeForForce").val("");
            $("#FormuleInfoForce").val("");
            $("#MontantCalForce").val("");
            $("#MontantForForce").val("");

            $("#dvInfoFormuleForce").hide();

            $("#btnResetCalculForce").removeAttr("disabled");
            $("#btnCancelCalculForce").removeAttr("disabled");
            $("#btnValidCalculForce").removeAttr("disabled");
        });
        $("#btnValidCalculForceForm").kclick(function () {
            $(".requiredField").removeClass("requiredField");
            if ($("#MontantForForce").val() == "") {
                $("#MontantForForce").addClass("requiredField");
                common.dialog.error("Veuillez saisir le montant forcé.");
                return false;
            }
            self.updateCalculForce(false);
        });
        AlternanceLigne("ListForceFormule", "", false, null);
    };
    //----------Charge les informations de la formule sélectionnée--------
    Quittance.prototype.loadInfoFormule = function (codeFor, numeroFormule) {
        $("#CodeRsqForce").val($.trim($("td[id='forceCodeRsq'][albFormId='" + codeFor + "']").html()));
        $("#CodeForForce").val(numeroFormule);
        $("#FormuleInfoForce").val($.trim($("td[id='forceFormInfo'][albFormId='" + codeFor + "']").attr('title')));
        $("#MontantCalForce").val($.trim($("span[id='forceMontantCal'][albFormId='" + codeFor + "']").html()));
        $("#MontantForForce").val($.trim($("span[id='forceMontantForce'][albFormId='" + codeFor + "']").html()));

        $("#dvInfoFormuleForce").show();

        $("#btnResetCalculForce").attr("disabled", "disabled");
        $("#btnCancelCalculForce").attr("disabled", "disabled");
        $("#btnValidCalculForce").attr("disabled", "disabled");
    };
    //----------Modifie le calcul force-----------
    Quittance.prototype.updateCalculForce = function (closeDiv) {
        const self = this;
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var avenant = $("#NumAvenantPage").val();
        var typeVal = $("input[type='radio'][name='typeVal']:checked").val();
        var typeHTTTC = $("input[type='radio'][name='typeHTTTC']:checked").val();
        var type = $("#Offre_Type").val();
        var tabGuid = $("#tabGuid").val();

        var codeRsq = 0;
        var codeFor = 0;
        var montantForce = "";
        var montantForceHT = "";
        if ($("#MontantForceHT").attr("id") != undefined)
            montantForceHT = $("#MontantForceHT").autoNumeric("get");
        var montantForceTTC = "";
        if ($("#MontantForceTTC").attr("id") != undefined)
            montantForceTTC = $("#MontantForceTTC").autoNumeric("get");
        var montantForceForm = "";
        if ($("#MontantForForce").attr("id") != undefined)
            montantForceForm = $("#MontantForForce").autoNumeric("get");
        var acteGestion = $("#ActeGestion").val();
        if (typeVal == "montant") {
            switch (typeHTTTC) {
                case "HT":
                    montantForce = montantForceHT;
                    if (montantForce == undefined || montantForce == "") {//|| montantForce == "0") { désactivation, voir bug 1628
                        $("#MontantForceHT").addClass("requiredField");
                        return;
                    }
                    break;
                case "TTC":
                    montantForce = montantForceTTC;
                    if (montantForce == undefined || montantForce == "") {//|| montantForce == "0") { désactivation, voir bug 1628
                        $("#MontantForceTTC").addClass("requiredField");
                        return;
                    }
                    break;
            }
        }
        if (typeVal == "formule") {
            codeRsq = $("#CodeRsqForce").val();
            codeFor = $("#CodeForForce").val();
            montantForce = montantForceForm;
        }

        common.page.isLoading = true;

        $.ajax({
            type: "POST",
            url: "/Quittance/UpdateCalculForce",
            data: {
                codeOffre: codeOffre, version: version, type: type, avenant: avenant, typeVal: typeVal, typeHTTTC: typeHTTTC,
                codeRsq: codeRsq, codeFor: codeFor, montantForce: montantForce, closeDiv: closeDiv, acteGestion: acteGestion,
                reguleId: $("#ReguleId").val(), modeNavig: $("#ModeNavig").val(), tabGuid: tabGuid, acteGestionRegule: $("#ActeGestionRegule").val()
            },
            success: function (data) {
                common.page.isLoading = false;
                if (data != "") {
                    common.dialog.error(data);
                    return false;
                }
                if (closeDiv) {
                    ReloadCotisation(true, true);
                }
                else {
                    $("#inCalculForce").val("1");
                    self.loadWindowCalcul();
                    $("#CodeRsqForce").val("");
                    $("#CodeForForce").val("");
                    $("#FormuleInfoForce").val("");
                    $("#MontantCalForce").val("");
                    $("#MontantForForce").val("");

                    $("#dvInfoFormuleForce").hide();

                    $("#btnResetCalculForce").removeAttr("disabled");
                    $("#btnCancelCalculForce").removeAttr("disabled");
                    $("#btnValidCalculForce").removeAttr("disabled");
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };
    //----------Charge les garanties de la formule sélectionnée------------
    Quittance.prototype.loadGarantieFormule = function (formId) {
        const self = this;
        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let avenant = $("#NumAvenantPage").val();

        common.page.isLoading = true;

        $.ajax({
            type: "POST",
            url: "/Quittance/LoadGaranInfo",
            data: {
                codeOffre: codeOffre, version: version, avenant: avenant, formId: formId,
                modeNavig: $("#ModeNavig").val(), acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val(),
            },
            success: function (data) {
                $("#divGarantieForce").show();
                $("#divDataGarantieForce").html(data);
                self.initGarantieForce();
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };
    //----------Map les éléments de la div de garanties forcées-------------
    Quittance.prototype.initGarantieForce = function () {
        const self = this;
        common.autonumeric.apply($("input[albmask='calculdecimal']"), 'init', 'decimal', ' ', ',', 2, '999999999.99', '0.00');
        common.autonumeric.apply($("span[albmask='calculdecimal']"), 'init', 'decimal', ' ', ',', 2, '999999999.99', '0.00');
        AlternanceLigne("ListForceGarantie", "", false, null);

        $("img[name='updGaran']").each(function () {
            $(this).click(function () { self.loadInfoGarantie($(this).attr('albGaranId')); });
        });

        $("#btnCancelCalculForceGar").kclick(function () {
            self.cancelGarantieForce();
        });
        $("#btnValidCalculForceGar").kclick(function () {
            self.updateGarantieForce();
        });

        $("#btnCancelGaranCalculForce").kclick(function () {
            $("#divGarantieForce").hide();
            $("#divDataGarantieForce").html("");
        });
        $("#btnValidGaranCalculForce").kclick(function () {
            self.updateCalculForceGarantie();
        });
    };
    //--------Charge les informations de la garantie sélectionnée------------
    Quittance.prototype.loadInfoGarantie = function (codeGaran) {
        $("#SelectGaranForce").val(codeGaran);
        $("#GarantieInfoForce").val($.trim($("td[id='forceGaranInfo'][albGaranId='" + codeGaran + "']").attr('title')));
        $("#GarantieInfoForce").attr("title", $.trim($("td[id='forceGaranInfo'][albGaranId='" + codeGaran + "']").attr('title')));
        $("#MontantGarHT").val($("span[id='forceGarMontantCal'][albGaranId='" + codeGaran + "']").text());
        if ($("input[id='forceGarCatNat'][albGaranId='" + codeGaran + "']").val() == "O")
            $("#CATNATGar").check();
        $("#CodeTaxe").val($("input[id='forceGarTaxe'][albGaranId='" + codeGaran + "']").val());

        $("#dvInfoGarantieForce").show();

        $("#btnCancelGaranCalculForce").attr("disabled", "disabled");
        $("#btnValidGaranCalculForce").attr("disabled", "disabled");
    };
    //--------Annule les changements effectués pour la garantie sélectionnée------
    Quittance.prototype.cancelGarantieForce = function () {
        $("#CodeRsqForce").val("");
        $("#CodeForForce").val("");
        $("#FormuleInfoForce").val("");
        $("#MontantCalForce").val("");
        $("#MontantForForce").val("");

        $("#dvInfoGarantieForce").hide();

        $("#btnCancelGaranCalculForce").removeAttr("disabled");
        //$("#btnValidGaranCalculForce").removeAttr("disabled");
    };
    //--------Met à jour les données de la garantie sélectionnée-------
    Quittance.prototype.updateGarantieForce = function () {
        common.page.isLoading = true;
        const self = this;
        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let avenant = $("#NumAvenantPage").val();
        let formId = $("#IdForGaranForce").val();
        let codeFor = $("#CodeForGaranForce").val();
        let codeRsq = $("#CodeRsqGaranForce").val();
        let codeGaran = $("#SelectGaranForce").val();
        let montantForce = $("#MontantGarHT").autoNumeric('get');
        let catnatForce = $("#CATNATGar").is(":checked") ? "O" : "N";
        let codeTaxeForce = $("#CodeTaxe").val();

        $.ajax({
            type: "POST",
            url: "/Quittance/UpdateGaranForce",
            data: {
                codeOffre: codeOffre, version: version, avenant: avenant, formId: formId, codeFor: codeFor, codeRsq: codeRsq, codeGaran: codeGaran,
                montantForce: montantForce, catnatForce: catnatForce, codeTaxeForce: codeTaxeForce, modeNavig: $("#ModeNavig").val(),
                acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val()
            },
            success: function (data) {
                $("#divGarantieForce").show();
                $("#divDataGarantieForce").html(data);
                self.initGarantieForce();
                $("#btnValidGaranCalculForce").removeAttr("disabled");
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }

        });
    };
    //--------Valide les garanties de la formule-----------
    Quittance.prototype.updateCalculForceGarantie = function () {
        common.page.isLoading = true;

        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let avenant = $("#NumAvenantPage").val();
        let codeFor = $("#CodeForGaranForce").val();
        let codeRsq = $("#CodeRsqGaranForce").val();
        let acteGestion = $("#ActeGestion").val();
        let type = $("#Offre_Type").val();
        let tabGuid = $("#tabGuid").val();

        $.ajax({
            type: "POST",
            url: "/Quittance/ValidFormGaranForce",
            data: {
                codeOffre: codeOffre, version: version, type: type, avenant: avenant, codeFor: codeFor, codeRsq: codeRsq,
                acteGestion: acteGestion, acteGestionRegule: $("#ActeGestionRegule").val(),
                modeNavig: $("#ModeNavig").val(), tabGuid: tabGuid
            },
            success: function (data) {
                common.page.isLoading = false;
                if (data != "") {
                    common.dialog.error(data);
                    return false;
                }
                $("#divGarantieForce").hide();
                $("#divDataGarantieForce").clearHtml();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //-----------Gestion de la trace d'avenant pour l'emission--------
    Quittance.prototype.traceEmissionAvenant = function (isChecked) {
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/Quittance/GestionTraceAvt",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), isChecked: isChecked,
                acteGestion: $("#ActeGestion").val(), acteGestionRegule: $("#ActeGestionRegule").val()
            },
            success: function () {
                common.page.isLoading = false;
            },
            error: function (request) {
                if (isChecked) {
                    $("#chkEmission").uncheck();
                }
                else {
                    $("#chkEmission").check();
                }

                common.error.showXhr(request);
            }
        });
    };
    return Quittance;
}());
var quittance = new Quittance();
$(function () {

    quittance.initPage();
    quittance.initSuivant();
    MapElementsQuittanceTransverse();
    quittance.initVisuCotisations();

});
