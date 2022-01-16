
var matricesAffaire = matricesAffaire || {};
matricesAffaire.matriceFormule = {
    init: function () {
        $this.setPreviousOrCancel();
        $this.removeDisabling();
        $this.initConfirmActions();
        $this.initBasicClicks();
        $this.initContextMenus();
        $this.initCheckElemSortis();
        $this.initStyles();

        $("#divFormule").bind('scroll', function () {
            $("#divRisque").scrollTop($(this).scrollTop());
            $("#divHeadFormule").scrollLeft($(this).scrollLeft());
        });
    },
    removeDisabling: function () {
        $("#btnMatriceRisque").removeAttr('disabled');
        $("#btnMatriceGarantie").removeAttr('disabled');
        $("#checkElemSortis").removeAttr('readonly').removeAttr('disabled');
    },
    initConfirmActions: function () {
        $(document).on("click", "#btnConfirmOk", function () {
            CloseCommonFancy();
            let numFormule;
            let numOption;
            switch ($("#hiddenAction").val()) {
                case "Delete":
                    $this.deleteFormule();
                    break;
                case "Duplicate":
                    $this.duplicateFormule();
                    break;
                case "DeleteOption":
                    numFormule = parseInt($("#currentNameFormule").val().split("_")[1]);
                    numOption = parseInt($("#currentNameFormule").val().split("_")[2]);
                    $this.deleteOption(numFormule, numOption);
                    break;
                case "DuplicateOption":
                    numFormule = parseInt($("#currentNameFormule").val().split("_")[1]);
                    numOption = parseInt($("#currentNameFormule").val().split("_")[2]);
                    $this.duplicateOption(numFormule, numOption);
                    break;
            }
        });
    },
    initBasicClicks: function () {
        $(document).on("click", "#btnConfirmCancel", function () {
            CloseCommonFancy();
            $("#hiddenAction").val('');
        });

        $(document).on("click", "#btnAddRisque", function () {
            if (!$(this).attr('disabled')) {
                $this.redirect({ cible: "DetailsObjetRisque", job: "Index"});
            }
        });

        $(document).on("click", "#btnAddFormule", function () {
            if (!$(this).attr('disabled')) {
                $this.redirect({ cible: "FormuleGarantie", job: "Index", codeFormule: 0, codeOption: 0 });
            }
        });

        $(document).on("click", "#btnForm2", function () {
            $this.redirect({ cible: "FormuleGarantie", job: "Index", codeFormule: 0, codeOption: 0 });
        });

        $("#btnSuivant").kclick(function (evt, data) {
            if (data && data.returnHome) {
                $this.redirect({ cible: "RechercheSaisie", job: "Index" });
                DeverouillerUserOffres(common.tabGuid);
            }
            else {
                $this.redirect({ cible: "Engagements", job: "Index" });
            }
        });

        $(document).on("click", "#btnAnnuler", function () {
            var type = $("#Offre_Type").val();
            if (type == "O") {
                $this.redirect({ cible: "ModifierOffre", job: "Index" });
            }
            else if (type == "P") {
                $this.redirect({ cible: "AnInformationsGenerales", job: "Index" });
            }
        });

        $(document).on("click", "#btnMatriceRisque", function () {
            if (!$(this).attr('disabled')) {
                $this.redirect({ cible: "MatriceRisque", job: "Index" });
            }
        });
        $(document).on("click", "#btnMatriceFormule", function () {
            if (!$(this).attr('disabled')) {
                $this.redirect({ cible: "MatriceFormule", job: "Index" });
            }
        });
        $(document).on("click", "#btnMatriceGarantie", function () {
            if (!$(this).attr('disabled')) {
                $this.redirect({ cible: "MatriceGarantie", job: "Index" });
            }
        });
        $(document).on("click", "#updateFormule", function () {
            $this.redirect({ cible: "FormuleGarantie", job: "Index", codeFormule: $(this).attr('name').split('_')[1], codeOption: 1 });
        });
        $(document).on("click", "#copyFormule", function () {
            if (!window.isReadonly && !window.isModifHorsAvenant) {
                $("#CodeFormule").val($(this).attr('name').split('_')[1]);
                ShowCommonFancy("Confirm", "Duplicate", "Êtes-vous sûr de vouloir dupliquer la formule " + $(this).attr('name').split('_')[2] + " ?", 300, 130, true, true);
            }
        });

        $(document).on("click", "#deleteFormule", function () {
            if (!window.isReadonly && !window.isModifHorsAvenant) {
                $("#CodeFormule").val($(this).attr('name').split('_')[1]);
                ShowCommonFancy("Confirm", "Delete", "Êtes-vous sûr de vouloir supprimer la formule " + $(this).attr('name').split('_')[2] + " ?", 300, 130, true, true);
            }
        });
        $(document).on("click", "#conditionFormule", function () {
            $this.redirect({ cible: "ConditionsGarantie", job: "Index", codeFormule: $(this).attr('name').split('_')[1], codeOption: 1 });
        });
    },
    initContextMenuFormule: function (evt) {
        let $element = $(this);
        let codeFormule = $element.attr("id").replace("formuleHover", "");
        let blockConditions = $element.attr("albBlockCondition");
        let suppFormule = $element.attr("albSupprFormule");
        let isSorti = $element.attr("albSorti");
        let nbOptions = $("td[name^='optionHover_" + codeFormule + "_']").length;
        $this.menuActions.create(codeFormule, blockConditions, suppFormule, isSorti, nbOptions);
        $element.unbind("mouseover");
    },
    initContextMenuOption: function (evt) {
        let $element = $(this);
        let data = $element.attr("name").split("_");
        let nbOptions = $("td[name^='optionHover_" + data[1] + "']").length;
        $this.menuActions.createForOption(data[1], data[2], nbOptions);
        $element.unbind("mouseover");
    },
    initContextMenuClick: function (e) {
        $("#currentNameFormule").val($(this).attr('name'));
        $("#currentRsqFormule").val($(this).attr('albrsq'));
        e.preventDefault();
        var pos = $(this).offset();
        $(this).contextMenu({ x: pos.left, y: pos.top + $(this).height() });
    },
    initContextMenus: function () {
        // context-menu formule
        $("td[name^='formuleHover_']").each(function () {
            $(this).bind("mouseover", $this.initContextMenuFormule.bind(this));
            $(this).click($this.initContextMenuClick);
        });
        // context-menu option
        $("td[name^='optionHover_']").each(function () {
            $(this).bind("mouseover", $this.initContextMenuOption.bind(this));
            $(this).click($this.initContextMenuClick);
        });
    },
    initCheckElemSortis: function () {
        $(document).off("click", "#checkElemSortis");
        $(document).on("click", "#checkElemSortis", function () {
            if ($(this).is(':checked')) {
                $("[albShowAvt='elemSorti']").removeClass("None");
                var countCell = $("#tblHeaderFormule tr td").length;
                $("#tblHeaderFormule").removeAttr("style");
                $("#tldBodyFormule").removeAttr("style");
                $("#tblHeaderFormule").attr('width', 170 * countCell / 3 + "px");
                $("#tldBodyFormule").attr('width', 170 * countCell / 3 + "px");
            }
            else {
                $("[albShowAvt='elemSorti']").addClass("None");
                var countCell = $("#tblHeaderFormule tr td[albShowAvt='elem']").length;
                $("#tblHeaderFormule").removeAttr("style");
                $("#tldBodyFormule").removeAttr("style");
                $("#tblHeaderFormule").attr('width', 170 * countCell + "px");
                $("#tldBodyFormule").attr('width', 170 * countCell + "px");
            }
        });
    },
    initStyles: function () {
        let countCell = $("#tblHeaderFormule tr td[albShowAvt='elem']").length;
        $("#tblHeaderFormule").removeAttr("style");
        $("#tblHeaderFormule").attr('width', 170 * countCell + "px");
        $("#tldBodyFormule").removeAttr("style");
        $("#tldBodyFormule").attr('width', 170 * countCell + "px");
    },
    setPreviousOrCancel: function () {
        if (window.isReadonly || window.isModifHorsAvenant) {
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }
    },
    ensureEtapeRisque: function (codeRsq) {
        return $.ajax({
            type: "POST",
            url: "/MatriceFormule/GetValidRsq",
            data: { codeOffre: currentAffair.codeOffre, version: currentAffair.version, type: currentAffair.type, codeRsq: codeRsq },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    },
    redirect: function (options) {
        try {
            common.page.isLoading = true;
            var paramRedirect = $("#txtParamRedirect").val();
            var addParamType = $("#AddParamType").val();
            var addParamValue = $("#AddParamValue").val();
            if (options.isForcereadOnly) {
                addParamValue += "||FORCEREADONLY|1";
                $("#IsModeConsultationEcran").val("True");
            }
            $.ajax({
                type: "POST",
                url: "/MatriceFormule/Redirection/",
                data: {
                    cible: options.cible, job: options.job, codeOffre: currentAffair.codeOffre, version: currentAffair.version, type: currentAffair.type,
                    codeFormule: options.codeFormule, codeOption: options.codeOption, tabGuid: infosTab.tabGuid, formGen: options.formGen,
                    paramRedirect: paramRedirect, modeNavig: infosTab.modeNavigation, addParamType: addParamType, addParamValue: addParamValue
                },
                success: function () { },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        catch (e) {
            common.error.showMessage(e.message);
        }
    },
    doConfirmAction: function (codeAction, message, ignoreReadonly) {
        if (!window.isModifHorsAvenant && (ignoreReadonly || !window.isReadonly)) {
            try {
                let codeRsq = $("#currentRsqFormule").val();
                $("#CodeFormule").val($("#currentNameFormule").val().split('_')[1]);
                common.page.isLoading = true;
                $this.ensureEtapeRisque(codeRsq).then(function (data) {
                    common.page.isLoading = false;
                    if (!data) {
                        common.dialog.error("Veuillez valider le risque " + codeRsq + " lié à cette formule");
                    }
                    else {
                        ShowCommonFancy("Confirm", codeAction, message, 300, 130, true, true);
                    }
                });
            }
            catch (e) {
                common.error.showMessage(e.message);
            }
        }
    },
    duplicateFormule: function () {
        try {
            common.page.isLoading = true;
            let addParamType = $("#AddParamType").val();
            let addParamValue = $("#AddParamValue").val();
            $.ajax({
                type: "POST",
                url: "/MatriceFormule/DuplicateFormule",
                data: {
                    codeOffre: currentAffair.codeOffre, version: currentAffair.version, type: currentAffair.type,
                    codeFormule: $("#CodeFormule").val(), tabGuid: infosTab.tabGuid, modeNavig: infosTab.modeNavigation,
                    addParamType: addParamType, addParamValue: addParamValue
                },
                success: function () { },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        catch (e) {
            common.error.showMessage(e.message);
        }
    },
    duplicateOption: function (f, o) {
        try {
            common.page.isLoading = true;
            common.$postJson("/MatriceFormule/DuplicateOption", { affaire: currentAffair, numFormule: f, numOption: o }, true).done(function (num) {
                $this.redirect({
                    cible: "FormuleGarantie",
                    job: "Index",
                    codeFormule: $("#currentNameFormule").val().split('_')[1],
                    codeOption: num
                });
            });
        }
        catch (e) {
            common.error.showMessage(e.message);
        }
    },
    deleteOption: function (f, o) {
        try {
            common.page.isLoading = true;
            common.$postJson("/MatriceFormule/DeleteOption", { affaire: currentAffair, numFormule: f, numOption: o }, true).done(function () {
                document.location.reload(true);
            });
        }
        catch (e) {
            common.error.showMessage(e.message);
        }
    },
    deleteFormule: function () {
        try {
            common.page.isLoading = true;
            let addParamType = $("#AddParamType").val();
            let addParamValue = $("#AddParamValue").val();
            $.ajax({
                type: "POST",
                url: "/MatriceFormule/DeleteFormule",
                data: {
                    codeOffre: currentAffair.codeOffre, version: currentAffair.version, type: currentAffair.type,
                    codeFormule: $("#CodeFormule").val(), tabGuid: infosTab.tabGuid, modeNavig: infosTab.modeNavigation,
                    addParamType: addParamType, addParamValue: addParamValue
                },
                success: function () { },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });

            $("#CodeFormule").val("");
        }
        catch (e) {
            common.error.showMessage(e.message);
        }
    },
    menuActions: {
        create: function (codeFormule, blockConditions, suppFormule, isSorti, nbOptions) {
            //$("td[id='formuleHover" + codeFormule + "']").attr("albcontextmenu", "N");
            //$.contextMenu('destroy', 'tr[albcontextmenu=O');
            $.ajax({
                type: "POST",
                url: "/ContextMenu/GetMenuFormule",
                data: { type: $("#Offre_Type").val(), user: $("#NameUser").val(), readOnly: window.isReadonly || window.isModifHorsAvenant, blockConditions: blockConditions, suppFormule: suppFormule, isSorti: isSorti, nbOptions: nbOptions },
                success: function (data) {
                    if (data != undefined && data != "") {
                        $("td[id='formuleHover" + codeFormule + "']").attr("albcontextmenu", "O");
                        common.$initContextMenu("td[id=formuleHover" + codeFormule + "]", "", data);
                    }
                },
                error: function (request) {
                    common.error.showXhr(request, true);
                }
            });
        },
        createForOption: function (codeFormule, codeOption, nbOptions) {
            let code = "optionHover_" + codeFormule + "_" + codeOption;
            $.ajax({
                type: "POST",
                async: false,
                url: "/ContextMenu/GetMenuOption",
                data: { user: $("#NameUser").val(), readOnly: window.isReadonly || window.isModifHorsAvenant, nbOptions: nbOptions },
                success: function (data) {
                    if (data != undefined && data != "") {
                        $("td[name='" + code + "']").attr("albcontextmenu", "O");
                        common.$initContextMenu("td[name='" + code + "']", "", data);
                    }
                },
                error: function (request) {
                    common.error.showXhr(request, true);
                }
            });
        },
        consultFormule: function () {
            $this.redirect({
                cible: "FormuleGarantie",
                job: "Index",
                codeFormule: $("#currentNameFormule").val().split('_')[1],
                codeOption: 1,
                isForcereadOnly: true
            });
        },
        consultConditions: function () {
            $this.redirect({
                cible: "ConditionsGarantie",
                job: "Index",
                codeFormule: $("#currentNameFormule").val().split('_')[1],
                codeOption: 1
            });
        },
        updateFormule: function () {
            $this.redirect({
                cible: "FormuleGarantie",
                job: "Index",
                codeFormule: $("#currentNameFormule").val().split('_')[1],
                codeOption: 1
            });
        },
        duplicateFormule: function () {
            $("#CodeFormule").val($("#currentNameFormule").val().split('_')[1]);
            $this.doConfirmAction(
                "Duplicate",
                "Êtes-vous sûr de vouloir dupliquer la formule " + $("#currentNameFormule").val().split('_')[2] + " ?");
        },
        addOption: function () {
            if (currentAffair.type != "O") { return; }
            let numFormule = $("#currentNameFormule").val().split("_")[1];
            let numOption = Math.min.apply(null, $(".TableOption td[name^='option_']").get().map(function (e) { return parseInt($(e).attr("name").split("_")[1]); }));
            document.location.href = "/FormuleGarantie/Index/" + currentAffair.getUrlId() + "_" + numFormule + "_" + numOption
                + "tabGuid" + infosTab.tabGuid + "tabGuid"
                + "modeNavig" + infosTab.modeNavigation + "modeNavig";
            return;
        },
        updateOption: function () {
            $this.redirect({
                cible: "FormuleGarantie",
                job: "Index",
                codeFormule: parseInt($("#currentNameFormule").val().split('_')[1]),
                codeOption: parseInt($("#currentNameFormule").val().split('_')[2])
            });
        },
        duplicateOption: function () {
            if (currentAffair.type != "O") { return; }
            $("#CodeFormule").val($("#currentNameFormule").val().split('_')[1]);
            $this.doConfirmAction(
                "DuplicateOption",
                "Êtes-vous sûr de vouloir dupliquer l'option " + $("#currentNameFormule").val().split('_')[2] + " ?");
        },
        deleteOption: function () {
            if (currentAffair.type != "O") { return; }
            $this.doConfirmAction(
                "DeleteOption",
                "Êtes-vous sûr de vouloir supprimer l'option " + $("#currentNameFormule").val().split('_')[2] + " ?");
        },
        deleteFormule: function () {
            $("#CodeFormule").val($("#currentNameFormule").val().split('_')[1]);
            $this.doConfirmAction(
                "Delete",
                "Êtes-vous sûr de vouloir supprimer la formule " + $("#currentNameFormule").val().split('_')[2] + " ?");
        }
    }
};
let $this = matricesAffaire.matriceFormule;
$(document).ready(function () {
    matricesAffaire.matriceFormule.init();
});

