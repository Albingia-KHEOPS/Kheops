
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamTemplates/Redirection",
        data: { cible: cible, job: job },
        success: function (data) {
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

var ParamTemplate = function () {
    this.initPage = function () {
        $("#btnRechercher").kclick(function () {
            paramTemplate.search();
        });

        $("#btnAjouterTemplate").kclick(function () {
            paramTemplate.showDetails("");
        });

        $("#btnConfirmOk").kclick(function () {
            CloseCommonFancy();
            let template;
            switch ($("#hiddenAction").val()) {
                case "SupprimerTemplate":
                    template = $("#idTemplateSuppr").val();
                    paramTemplate.delete(template);
                    break;
                case "RegenererTemplate":
                    template = $("#idTemplateSuppr").val().split("_")[1];
                    common.dialog.initConfirm(
                        function () {
                            paramTemplate.regenerateCanevas(true, template);
                        },
                        function () {
                            paramTemplate.regenerateCanevas(false, template);
                        },
                        "Quel type de régénération souhaitez-vous effectuer ?<br/> Régénération partielle ou totale ? <br/>",
                        "Totale",
                        "Partielle"
                    );
                    break;
            }
            $("#hiddenAction").clear();
            $("#idTemplateSuppr").clear();
        });

        $("#btnConfirmCancel").kclick(function () {
            CloseCommonFancy();
            $("#hiddenAction").clear();
            $("#idTemplateSuppr").clear();
        });

        $("#btnAnnuler, #btnCancel").kclick(function () {
            paramTemplate.cancel();
        });

        $("#rechercheCode, #rechercheDescription").offOn("change", function () {
            paramTemplate.resetContent();
        });

        $("#btnRegenCanevas").kclick(function () {
            common.dialog.initConfirm(
                function () {
                    paramTemplate.regenerateCanevas(true);
                },
                function () {
                    paramTemplate.regenerateCanevas(false);
                },
                "Quelle type de régénération souhaitez vous effectuer? </br> Régénération partielle ou totale? <br />",
                "Totale",
                "Partielle"
            );
        });

        $("#btnCopyCanevas").kclick(function () {
            $(".popup-overlay, .popup-content").addClass("active");
            paramTemplate.initCopyCanevas();
        });

        $("#btnCloseCopyCanevas").on("click", function () {
            $(".popup-overlay, .popup-content").removeClass("active");
        });

        $("#btnStartCopy").kclick(function () {
            common.page.isLoading = true;
            $.ajax({
                type: "POST",
                url: "/ParamTemplates/CopyCanevas",
                data: { source: $("#copyCanevasSource").val(), cible: $("#copyCanevasCible").val() },
                success: function () {
                    common.dialog.info("Copie des canevas terminée.");
                    common.page.isLoading = false;
                },
                error: function (request) {
                    common.error.showXhr(request);
                    common.page.isLoading = false;
                }
            });
        });
    };
    this.initCopyCanevas = function () {
        let envName = $("#EnvironmentName").val();
        let selIndex = 0;
        $('#copyCanevasCible option[value="' + envName + '"]').attr("selected", true);
        $('#copyCanevasSource option[value="' + envName + '"]').prop('disabled', true);

        // Sélection d'une autre item si l'index sélectionné est le même que l'index de l'environnement courant
        let indexEnv = $('#copyCanevasSource option[value="' + envName + '"]')[0].index;
        if ($('#copyCanevasSource')[0].selectedIndex == indexEnv) {
            if (indexEnv == $("#copyCanevasSource").children("option").length - 1) {
                selIndex = 0;
            }
            else {
                selIndex = indexEnv + 1;
            }
            $('#copyCanevasSource')[0].selectedIndex = selIndex;
        }
    };
    this.showDetails = function (idLigne) {
        if (idLigne != undefined) {
            common.page.isLoading = true;
            $.ajax({
                type: "POST",
                url: "/ParamTemplates/GetDetailsTemplate",
                data: { idTemplate: idLigne },
                success: function (data) {
                    $("#divTemplateDetails").html(data);
                    $("#btnEnregistrer").kclick(function () {
                        paramTemplate.save();
                    });
                    $("#VoletDetails").show();
                    common.page.isLoading = false;
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    };
    this.mapTable = function () {
        $("tr[name=ligneTemplate]").kclick(function () {
            var id = $(this).attr("id").split("_")[1];
            paramTemplate.showDetails(id);
        });

        $("img[name=btnSupprimer]").kclick(function () {
            var id = $(this).attr("id").split("_")[1];
            $("#idTemplateSuppr").val(id);
            var cibleRef = $("#clbRef_" + id).val();
            common.page.isLoading = true;
            $.ajax({
                type: "POST",
                url: "/ParamTemplates/ConfirmSuppr",
                data: { cibleRef: cibleRef },
                success: function (data) {
                    ShowCommonFancy("Confirm", "SupprimerTemplate", data + "<br/>Etes-vous sûr de vouloir continuer ?", 320, 150, true, true);
                    common.page.isLoading = false;
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        });

        $("img[name='btnRegenerate']").kclick(function () {
            var template = $(this).attr("id").split("_")[1];
            common.dialog.initConfirm(
                function () {
                    paramTemplate.regenerateCanevas(true, template);
                },
                function () {
                    paramTemplate.regenerateCanevas(false, template);
                },
                "Quel type de régénération souhaitez-vous effectuer ?<br/> Régénération partielle ou totale ? <br/>",
                "Totale",
                "Partielle"
            );
        });

        MapLinkWinOpen("N");
        AlternanceLigne("TemplatesBody", "noInput", true, null);
    };
    this.search = function () {
        var codeRecherche = $.trim($("#rechercheCode").val());
        var descrRecherche = $.trim($("#rechercheDescription").val());
        var typeRecherche = $("#drlTypeTemplateRecherche").val();
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/ParamTemplates/RechercheTemplates",
            data: { codeTemplate: codeRecherche, descriptionTemplate: descrRecherche, type: typeRecherche },
            success: function (data) {
                $("#divTemplatesBody").html(data);
                paramTemplate.mapTable();
                $("#VoletDetails").hide();
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };
    this.regenerateCanevas = function (totalRegeneration, codeCanevas) {
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/ParamTemplates/RegenerateCanevas",
            data: { totalRegeneration: totalRegeneration, codeCanevas: codeCanevas },
            success: function () {
                common.dialog.info("Régénération des canevas terminée.");
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };
    this.delete = function (id) {
        if (id) {
            common.page.isLoading = true;
            $.ajax({
                type: "POST",
                url: "/ParamTemplates/SupprimerTemplate",
                data: { idTemplate: id },
                success: function () {
                    paramTemplate.search();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    };
    this.save = function () {
        var modeSaisie = $("#modeSaisie").val();
        var codeEdit = $.trim($("#codeTemplateEdit").val());
        var descrEdit = $.trim($("#descriptionTemplateEdit").val());
        var userRights = $("#AdditionalParam").val();
        var idTemplate = $("#idTemplateEdit").val();
        var typeTemplate = $("#drlTypeTemplateEdit").val();

        var verif = true;
        if (codeEdit == "" || codeEdit == undefined) {
            $("#codeTemplateEdit").addClass("requiredField");
            verif = false;
        }
        if (descrEdit == "" || descrEdit == undefined) {
            $("#descriptionTemplateEdit").addClass("requiredField");
            verif = false;
        }
        if (typeTemplate == "" || typeTemplate == undefined) {
            $("#drlTypeTemplateEdit").addClass("requiredField");
            verif = false;
        }

        if (verif) {
            encodeURIComponent(codeEdit);
            encodeURIComponent(descrEdit);
            common.page.isLoading = true;
            $.ajax({
                type: "POST",
                url: "/ParamTemplates/EnregistrerTemplate",
                data: { mode: modeSaisie, idTemplate: idTemplate, code: codeEdit, description: descrEdit, type: typeTemplate },
                success: function () {
                    paramTemplate.search();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    };
    this.resetContent = function () {
        $("#divTemplatesBody").html("");
        $("#divTemplateDetails").html("");
    };
    this.cancel = function () {
        window.location.href = "/BackOffice/Index";
    };
};

var paramTemplate = new ParamTemplate();
$(document).ready(function () {
    paramTemplate.initPage();
    MapCommonAutoCompTemplates();
    paramTemplate.search();
});