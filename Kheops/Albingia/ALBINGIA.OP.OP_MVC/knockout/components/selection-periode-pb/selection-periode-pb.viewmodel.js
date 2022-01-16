(function () {
    let viewModel = function (parameters) {
        let InfoPB = function (options) {
            if (options.data.PeriodeDeb.length > 10) {
                options.data.PeriodeDeb = options.data.PeriodeDeb.trim().substr(0, 10);
            }
            if (options.data.PeriodeFin.length > 10) {
                options.data.PeriodeFin = options.data.PeriodeFin.trim().substr(0, 10);
            }
            let infoPB = ko.mapping.fromJS(options.data, {}, this);
            infoPB.ChoixCodeCourtier = ko.observable(0);
            infoPB.ChoixCodeCourtierCom = ko.observable(0);
            infoPB.ChoixCodeQuittancement = ko.observable("");
            infoPB.ChoixTauxHCATNAT = ko.observable(0);
            infoPB.ChoixTauxCATNAT = ko.observable(0);
        };

        let mapInfoPB = {
            create: function (options) {
                return new InfoPB(options);
            }
        };

        ko.mapping.fromJS(window.modelPbs, { "InfoPB": mapInfoPB }, this);
        let vm = this;
        let pb = vm.InfoPB;

        pb.isCreateActeGestion = ko.computed(function () {
            return pb.ModeAvt() == "CREATE";
        });
        pb.hasCourtier = ko.computed(function () {
            return pb.CodeCourtier() != null && pb.CodeCourtier() != "0";
        });
        pb.hasCourtierCom = ko.computed(function () {
            return pb.CodeCourtierCom() != null && pb.CodeCourtierCom() != "0";
        });

        pb.courtier = ko.computed(function () {
            return pb.hasCourtier() ? pb.CodeCourtier() + " - " + (pb.NomCourtier() != null ? pb.NomCourtier() : "") : "";
        });
        pb.courtierCom = ko.computed(function () {
            return pb.hasCourtierCom() ? pb.CodeCourtierCom() + " - " + (pb.NomCourtierCom() != null ? pb.NomCourtierCom() : "") : "";
        });
        pb.quittancement = ko.computed(function () {
            return pb.hasCourtier() && pb.CodeQuittancement() != null && pb.CodeQuittancement() != "" ? pb.CodeQuittancement() + " - " + pb.LibQuittancement() : "";
        });

        vm.modifCourtier = function () {
            pb.ChoixCodeCourtier(pb.CodeCourtier());
            pb.ChoixCodeCourtierCom(pb.CodeCourtierCom());
            pb.ChoixCodeQuittancement(pb.CodeQuittancement());
            pb.ChoixTauxHCATNAT(pb.TauxHCATNAT());
            pb.ChoixTauxCATNAT(pb.TauxCATNAT());

            $("#dvChoixCourtier").show();
            $("#btnReguleSuivant").disable();
        };
        vm.cancelCourtier = function () {
            ShowCommonFancy("Confirm", "CancelCourtierRegule",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true, true);
        };

        vm.cancelCourtierConfirmOk = function () {
            pb.ChoixCodeCourtier(pb.CodeCourtier());
            pb.ChoixCodeCourtierCom(pb.CodeCourtierCom());
            pb.ChoixCodeQuittancement(pb.CodeQuittancement());
            pb.ChoixTauxHCATNAT(pb.TauxHCATNAT());
            pb.ChoixTauxCATNAT(pb.TauxCATNAT());

            $("#dvChoixCourtier").hide();

            $("#btnCancelCourtier").hide();
            $("#btnCloseCourtier").show();
            $("#btnValidCourtier").disable();
        };

        vm.closeCourtier = function () {
            $("#dvChoixCourtier").hide();
            $("#btnReguleSuivant").removeAttr("disabled");
        };
        vm.validCourtier = function () {
            pb.CodeCourtier(pb.ChoixCodeCourtier());
            pb.CodeCourtierCom(pb.ChoixCodeCourtierCom());
            pb.CodeQuittancement(pb.ChoixCodeQuittancement());
            pb.TauxHCATNAT(pb.ChoixTauxHCATNAT());
            pb.TauxCATNAT(pb.ChoixTauxCATNAT());
            pb.LibQuittancement($("#ddlQuittancements").find("option:selected").text().split(" - ")[1]);

            $("#dvChoixCourtier").hide();
            $("#btnReguleSuivant").removeAttr("disabled");

            $("#btnCancelCourtier").hide();
            $("#btnCloseCourtier").show();
            $("#btnValidCourtier").disable();
        };

        vm.quittancementChanged = function (obj, event) {
            if (event.originalEvent) { //user changed
                $("#btnCancelCourtier").show();
                $("#btnCloseCourtier").hide();
                $("#btnValidCourtier").removeAttr("disabled");


                if (pb.ChoixCodeQuittancement() == "D") {
                    pb.ChoixTauxHCATNAT(0);
                    pb.ChoixTauxCATNAT(0);
                    $("#ComHCATNAT").makeReadonly(true);
                    $("#ComCATNAT").makeReadonly(true);
                }
                else {
                    pb.ChoixTauxHCATNAT($("#inHorsCATNAT").val());
                    pb.ChoixTauxCATNAT($("#inCATNAT").val());
                    $("#ComHCATNAT").enable();
                    $("#ComCATNAT").enable();
                }
            }
        };

        vm.catnatChanged = function () {
            $("#btnCancelCourtier").show();
            $("#btnCloseCourtier").hide();
            $("#btnValidCourtier").removeAttr("disabled");
        };

        vm.changePeriod = function (id) {
            $("#PeriodeValide").val("0");
            $(".requiredField").removeClass("requiredField");
            let isExerciceChanging = id === "ExerciceRegule";
            let emptyVal = !$("#" + id).hasVal();
            if (isExerciceChanging) {
                pb.PeriodeDeb("");
                pb.PeriodeFin("");
            }
            else {
                $("#ExerciceRegule").clear();
                emptyVal = !$("#PeriodeFin").hasVal() || !$("#PeriodeDeb").hasVal();
            }
            if (emptyVal || !isExerciceChanging && !checkDate($("#PeriodeDeb"), $("#PeriodeFin"))) {
                $("#btnReguleSuivant").attr("title", "").disable();
                return false;
            }

            let url = "/CreationPB/Change" + (isExerciceChanging ? "Exercice" : "Periode");
            common.page.isLoading = true;
            let postData = {
                codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                codeAvn: $("#NumInterne").val(), typeAvt: $("#TypeAvt").val(), periodeDeb: $("#PeriodeDeb").val(),
                periodeFin: $("#PeriodeFin").val(), lotId: $("#lotId").val(), reguleId: $("#ReguleId").val(), regulMode: common.getRegul().mode,
                deleteMod: $("#deleteMod").val(), cancelMod: $("#cancelMod").val()
            };
            if (isExerciceChanging) {
                postData.exercice = $("#ExerciceRegule").val();
            }
            common.$postJson(url, postData)
                .done(function (data) {
                    pb.CodeCourtier(data.CodeCourtier);
                    pb.NomCourtier(data.NomCourtier);
                    pb.CodeCourtierCom(data.CodeCourtierCom);
                    pb.NomCourtierCom(data.NomCourtierCom);
                    pb.CodeQuittancement(data.CodeQuittancement);
                    pb.LibQuittancement(data.LibQuittancement);
                    pb.TauxHCATNAT(data.TauxHCATNAT);
                    pb.TauxCATNAT(data.TauxCATNAT);
                    pb.Exercice(data.Exercice);
                    pb.LotId(data.LotId);
                    pb.ReguleId(data.ReguleId);
                    pb.RetourPGM(data.RetourPGM);
                    pb.HasSelections(data.HasSelections);

                    if (data.HasSelections) {
                        $("#btnReguleSuivant").show().enable();
                    } else {
                        $("#btnReguleSuivant").attr("title", "Pas de régularisation possible pour la période sélectionnée");
                        $("#btnReguleSuivant").disable();
                    }

                    if (data.RetourPGM != "") {
                        $("#PeriodeValide").val(data.HasSelections ? "1" : "0");
                        pb.PeriodeDeb(data.RetourPGM.split("_")[0]);
                        pb.PeriodeFin(data.RetourPGM.split("_")[1]);
                        $("#PeriodeDeb").removeClass("requiredField");
                        $("#PeriodeFin").removeClass("requiredField");
                    }
                    $("#btnDeleteRegule").addClass("CursorPointer");
                    $("#btnModifCourtier").show();
                    common.page.isLoading = false;
                })
                .fail(function (request) {
                    if (isExerciceChanging) {
                        $("#ExerciceRegule").addClass("requiredField");
                    }
                    else {
                        $("#PeriodeDeb").addClass("requiredField");
                        $("#PeriodeFin").addClass("requiredField");
                    }
                    pb.CodeCourtier("");
                    pb.NomCourtier("");
                    pb.TauxHCATNAT("");
                    pb.TauxCATNAT("");
                    pb.CodeQuittancement("");
                    pb.LibQuittancement("");
                    $("#btnModifCourtier").hide();
                    $("#btnReguleSuivant").disable();
                    common.error.showXhr(request);
                    common.page.isLoading = false;
                });
        };

        vm.resetPeriodesConfirmOk = function () {
            let reguleId = pb.ReguleId();
            if (isNaN(reguleId) || reguleId < 1) {
                pb.PeriodeDeb("");
                pb.PeriodeFin("");
                pb.Exercice("");
                $("#btnReguleSuivant").attr("title", "").disable();
                return;
            }
            common.page.isLoading = true;
            $.ajax({
                type: "POST",
                url: "/CreationPB/SupressionDatesRegularisation",
                data: {
                    reguleId: reguleId
                },
                success: function (data) {
                    if ($("#isContratTempo").val() !== "True") {
                        $("#ExerciceRegule").removeAttr("disabled").removeAttr("readonly").removeClass("readonly");
                    }
                    $("#PeriodeDeb").removeAttr("disabled").removeClass("readonly");
                    $("#PeriodeFin").removeAttr("disabled").removeClass("readonly");
                    $("#deleteMod").val('D');
                    pb.PeriodeDeb("");
                    pb.PeriodeFin("");
                    pb.Exercice("");
                    $("#btnReguleSuivant").attr("title", "").disable();
                    pb.CodeCourtier(data.CodeCourtier);
                    pb.NomCourtier(data.NomCourtier);
                    pb.CodeCourtierCom(data.CodeCourtierCom);
                    pb.NomCourtierCom(data.NomCourtierCom);
                    pb.CodeQuittancement(data.CodeQuittancement);
                    pb.LibQuittancement(data.LibQuittancement);
                    pb.TauxHCATNAT(data.TauxHCATNAT);
                    pb.TauxCATNAT(data.TauxCATNAT);
                    $("#btnDeleteRegule").removeClass("CursorPointer");
                    common.page.isLoading = false;
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        };

        vm.showDeleteRegul = function () {
            ShowCommonFancy("Confirm", "DelPeriode", "Etes-vous sûr de vouloir supprimer ces dates de régularisation ?", 300, 80, true, true, true);
        };

        vm.openAlerte = function (data, event) {
            OpenAlerte($(event.target));
        };

        vm.initSelectionPeriodePage = function () {
            toggleDescription();
            common.autonumeric.applyAll('init', 'decimal');
            common.autonumeric.applyAll('init', 'pourcentdecimal');
            common.autonumeric.applyAll('init', 'year', '', null, 0, 9999, 0);
            $("#PeriodeDeb").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#PeriodeFin").datepicker({ dateFormat: 'dd/mm/yy' });

            AffectDateFormat();

            $(document).off("change", "#ExerciceRegule, #PeriodeDeb, #PeriodeFin").on("change", "#ExerciceRegule, #PeriodeDeb, #PeriodeFin", function (evt) {
                if (!evt.isDefaultPrevented()) {
                    vm.changePeriod(this.id);
                }
                evt.stopPropagation();
                evt.preventDefault();
            });

            if (pb.ModeAvt == "UPDATE" || pb.ModeAvt == "CONSULT") {
                $("#ExerciceRegule").attr("readonly", "readonly").addClass("readonly");
                $("#PeriodeDeb").attr("disabled", "disabled").addClass("readonly");
                $("#PeriodeFin").attr("disabled", "disabled").addClass("readonly");
            }
            else {
                if ((pb.ErreurPGM() != "") && (pb.ErreurPGM() != undefined)) {
                    ShowCommonFancy("Error", "", pb.ErreurPGM(), 300, 80, true, true, true);
                    $("#PeriodeDeb").addClass("requiredField");
                    $("#PeriodeFin").addClass("requiredField");
                    $("#btnReguleSuivant").attr("disabled", "disabled");
                }
            }

            let addParam = $('#AddParamValue').val();

            if (addParam.indexOf('||LOTID|') === -1)
                $('#AddParamValue').val(addParam + '||LOTID|' + $("#lotId").val());

            $("#btnConfirmCancel").kclick(function () {
                CloseCommonFancy();
                $("#hiddenAction").val("");
                $("#hiddenInputId").val('');
            });

            $("#btnConfirmOk").kclick(function () {
                CloseCommonFancy();
                switch ($("#hiddenAction").val()) {
                    case "CancelCourtierRegule":
                        vm.cancelCourtierConfirmOk();
                        break;
                    case "DelPeriode":
                        vm.resetPeriodesConfirmOk();
                        break;
                }
                $("#hiddenInputId").val('');
                $("#hiddenAction").val("");
            });
        };
    };
    pbs.PeriodeViewModel = viewModel;
})();
