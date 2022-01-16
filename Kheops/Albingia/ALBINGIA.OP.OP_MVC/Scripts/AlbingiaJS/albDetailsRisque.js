
var DetailsRisque = function () {
    this.isAvnDisabled = function () {
        let disabled = false;
        if ($("#chkModificationAVN").length > 0) {
            disabled = !$('#chkModificationAVN').isChecked();
        }
        return disabled;
    };
    //---------------------Affecte les fonctions sur les controles heures-----------------------
    this.initChangeHour = function () {
        $("#InformationsGenerales_HeureEntreeGarantieHours").offOn("change", function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureEntreeGarantieMinutes").val() == "")
                $("#InformationsGenerales_HeureEntreeGarantieMinutes").val("00");
            detailsRisque.setHour($(this));
        });
        $("#InformationsGenerales_HeureEntreeGarantieMinutes").offOn("change", function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureEntreeGarantieHours").val() == "")
                $("#InformationsGenerales_HeureEntreeGarantieHours").val("00");
            detailsRisque.setHour($(this));
        });
        $("#InformationsGenerales_HeureSortieGarantieHours").offOn("change", function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureSortieGarantieMinutes").val() == "")
                $("#InformationsGenerales_HeureSortieGarantieMinutes").val("00");
            detailsRisque.setHour($(this));
        });
        $("#InformationsGenerales_HeureSortieGarantieMinutes").offOn("change", function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureSortieGarantieHours").val() == "")
                $("#InformationsGenerales_HeureSortieGarantieHours").val("00");
            detailsRisque.setHour($(this));
        });
        $("#InformationsGenerales_HeureEntreeDescrHours").offOn("change", function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureEntreeDescrMinutes").val() == "")
                $("#InformationsGenerales_HeureEntreeDescrMinutes").val("00");
            detailsRisque.setHour($(this));
        });
        $("#InformationsGenerales_HeureEntreeDescrMinutes").offOn("change", function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureEntreeDescrHours").val() == "")
                $("#InformationsGenerales_HeureEntreeDescrHours").val("00");
            detailsRisque.setHour($(this));
        });
        $("#InformationsGenerales_HeureSortieDescrHours").offOn("change", function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureSortieDescrMinutes").val() == "")
                $("#InformationsGenerales_HeureSortieDescrMinutes").val("00");
            detailsRisque.setHour($(this));
        });
        $("#InformationsGenerales_HeureSortieDescrMinutes").offOn("change", function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureSortieDescrHours").val() == "")
                $("#InformationsGenerales_HeureSortieDescrHours").val("00");
            detailsRisque.setHour($(this));
        });
    };
    //-------------------Renseigne l'heure---------------------------
    this.setHour = function (elem) {
        var elemId = elem.attr('id').replace("Hours", "").replace("Minutes", "");

        var changeHour = SetHours(elemId);
        if (!changeHour && elem.val() == "") {
            $("#" + elemId + "Hours").val("");
            $("#" + elemId + "Minutes").val("");
        }
    };
    //---------------------Affecte les fonctions au boutons-------------
    this.mapPageElement = function () {
        if ($("#chkModificationAVN").length > 0) {
            if (($("#IsAvenantModificationLocale").val() == "True" && $("#IsTraceAvnExist").val() == "True") || $("#IsModeConsultationEcran").val() == "True" || $("#IsModifHorsAvn").val() == "True") {
                $("#chkModificationAVN").disable();
                $(document).off("change", "#chkModificationAVN");
            }
            else {
                $("#chkModificationAVN").removeAttr("disabled");
                $("#chkModificationAVN").offOn("change", function () {
                    detailsRisque.redirect("DetailsRisque", "Index", $("#Code").val(), "0", !$("#chkModificationAVN").isChecked());
                });

                if (window.isModifHorsAvenant && !$('#chkModificationAVN').isChecked()) {
                    $('#InformationsGenerales_Descriptif').disable();
                    $('div[name=btnAdresse]').removeClass('CursorPointer');
                    $('#InformationsGenerales_CodeTre').disable();
                }
            }
        }
        //-------------- mappage objets sortis

        if ($("#chkDisplayObjetsSortis").length > 0) {
            $("#chkDisplayObjetsSortis").removeAttr("disabled");
            $("#chkDisplayObjetsSortis").offOn("change", function () {
                if ($("#chkDisplayObjetsSortis").isChecked()) {
                    $("tr[name=objetSorti]").each(function () {
                        $(this).show();
                    });
                }
                else {
                    $("tr[name=objetSorti]").each(function () {
                        $(this).hide();
                    });
                }
            });

            $("#chkDisplayObjetsSortisPE").offOn("change", function () {
                if ($("#chkDisplayObjetsSortisPE").isChecked()) {
                    $("tr[name=objetSorti]").show();
                }
                else {
                    $("tr[name=objetSorti]").hide();
                }
            });
        }

        var isModificationAvn = !detailsRisque.isAvnDisabled();
        
        if (isModificationAvn && (!window.isReadonly || window.isModifHorsAvenant) && $("#MonoObjet").val() === "0") {
            $("#Description_Observation").html($("#InformationsGenerales_Designation").html().replace(/&lt;br&gt;/ig, "\n"));
        }

        if (window.isReadonly || !isModificationAvn) {
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }
        $("#oldCible").val($("#InformationsGenerales_Cible").val());
        $("#InformationsGenerales_Cible").offOn("change", function () {
            if ($("#hasFormules").val() == "True") {
                detailsRisque.showCommonFancyRsq("ConfirmRsq", "Cible", "Attention, la modification d'une cible entraine la régénération de certaines informations du risque ou de ses objets, ainsi que la suppression des formules associées au risque. <br/>Voulez-vous continuer ?", 300, 120, true, true);
            }
            else {
                $("#oldCible").val($("#InformationsGenerales_Cible").val());
                ReloadNomenclatureCombos($("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), $("#CodeRisque").val(), 0, $(this).val());
                $("#InformationsGenerales_CodeTre").val('');
            }
        });

        $("#btnErrorOk").kclick(function () {
            CloseCommonFancy();
        });
        $("#btnAnnuler").kclick(function () {
            detailsRisque.showCommonFancyRsq("ConfirmRsq", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l'annulation ?<br/>",
                350, 130, true, true);
        });
        $("#btnSupprimerDtlRsq").kclick(function () {
            if (!$(this).attr('disabled')) {
                detailsRisque.showCommonFancyRsq("ConfirmRsq", "Risque",
                    "Vous êtes sur le point de supprimer le risque.<br/><br/>Voulez-vous continuer ?<br/>",
                    350, 130, true, true);
            }
        });
        $("#btnConfirmOkRsq").kclick(function () {
            CloseCommonFancy();
            switch ($("#hiddenActionRsq").val()) {
                case "Risque":
                    detailsRisque.deleteRisque();
                    break;
                case "Cible":
                    detailsRisque.deleteFormuleGarantie();
                    $("#InformationsGenerales_CodeTre").val('');
                    break;
                case "Cancel":
                    detailsRisque.redirect("MatriceRisque", "Index", "");
                    break;
            }
            $("#hiddenAction").val('');
        });
        $("#btnConfirmCancelRsq").kclick(function () {
            CloseCommonFancy();
            if ($("#hiddenActionRsq").val() == "Cible")
                $("#InformationsGenerales_Cible").val($("#oldCible").val());
            $("#hiddenAction").val('');
        });
        $("td[name=linkObjet]").kclick(function () {
            var path = $(this).attr('AlbLinkParam');
            $("#OpenObjet").val(path);
            $("#RedirectRisque").val($("#RedirectRsqOpenObj").val());
            $("#btnSuivant").trigger("click");
        });
        $("#btnAjouterObjet").kclick(function () {
            if ($(this).hasClass('CursorPointer')) {
                $("#RedirectRisque").val($("#RedirectRsqAddObj").val());
                $("#btnSuivant").trigger("click");
            }
        });

        $('#fullScreenObjet').kclick(function (evt) {
            AlbScrollTop();
            detailsRisque.openFullScreen();
        });

        $('#fermerFullScreen').kclick(function (evt) {
            detailsRisque.closeFullScreen();
        });
        toggleDescription($("#InformationsGenerales_Designation"));
        $("#btnFSAnnuler").kclick(function () {
            $("#btnAnnuler").trigger('click');
        });
        $("#btnFSSuivant").kclick(function () {
            $("#btnSuivant").trigger('click');
        });

        if (window.isReadonly || !isModificationAvn) {
            $("#btnAjouterObjet").html("").removeClass("CursorPointer");
            $("#btnSupprimerDtlRsq").hide();
        }

        $("#Unite").offOn("change", function () {
            detailsRisque.formatValeurUniteNumerique();
        });

        $("#InformationsGenerales_TypeRisque").die().live("change", function () {
            AffectTitleList($(this));
            $("#InformationsGenerales_Descriptif").val($("#InformationsGenerales_TypeRisque :selected").text().split('-')[1]);
        });
        $("#InformationsGenerales_TypeMateriel").die().live("change", function () {
            AffectTitleList($(this));
        });
        $("#InformationsGenerales_NatureLieux").die().live("change", function () {
            AffectTitleList($(this));
        });

        $("#InformationsGenerales_Territorialite").die().live("change", function () {
            AffectTitleList($(this));
        });

        $("#Type").die().live("change", function () {
            AffectTitleList($(this));
            //detailsRisque.initCoutM2Display();
        });

        $("#ValeurHT").die().live("change", function () {
            AffectTitleList($(this));
        });

        $("#btnInfoSpeRsq").click(function () {
            $("#RedirectRisque").val($("#RedirectRsqOpenIS").val());
            $("#btnSuivant").click();
        });

        $("select[albNiveauCombo]").each(function () {
            $(this).change(function () {
                AffectTitleList($(this));
                GetComboNomenclature($(this));
            });
        });

        $("#chkIsRisqueTemporaire").die().live("change", function () {
            $("#IsRisqueTemporaire").val($("#chkIsRisqueTemporaire").is(":checked"));
            if ($("#chkIsRisqueTemporaire").is(":checked")) {
                if ($("#InformationsGenerales_DateEntreeGarantie").val() == "" && $("#dateDebOffre").val() != "") {
                    $("#InformationsGenerales_DateEntreeGarantie").val($("#dateDebOffre").val());
                    $("#InformationsGenerales_HeureEntreeGarantieHours").val($("#heureDebOffre").val());
                    $("#InformationsGenerales_HeureEntreeGarantieMinutes").val($("#minuteDebOffre").val());
                    $("#InformationsGenerales_HeureEntreeGarantieHours").trigger("change");
                }
                if ($("#InformationsGenerales_DateSortieGarantie").val() == "" && $("#dateFinOffre").val() != "") {
                    $("#InformationsGenerales_DateSortieGarantie").val($("#dateFinOffre").val());
                    $("#InformationsGenerales_HeureSortieGarantieHours").val($("#heureFinOffre").val());
                    $("#InformationsGenerales_HeureSortieGarantieMinutes").val($("#minuteFinOffre").val());
                    $("#InformationsGenerales_HeureSortieGarantieHours").trigger("change");
                }
            }
        });

        //Initialisation
        AffectTitleList($("#Unite"));
        AffectTitleList($("#InformationsGenerales_TypeRisque"));
        AffectTitleList($("#InformationsGenerales_TypeMateriel"));
        AffectTitleList($("#InformationsGenerales_NatureLieux"));
        AffectTitleList($("#InformationsGenerales_CodeTre"));
        AffectTitleList($("#InformationsGenerales_Territorialite"));
        AffectTitleList($("#InformationsGenerales_CodeClasse"));
        AffectTitleList($("#Type"));
        AffectTitleList($("#ValeurHT"));
        AffectTitleList($("#InformationsGenerales_Cible"));
        AffectTitleList($("#InformationsGenerales_Nomenclature1"));
        AffectTitleList($("#InformationsGenerales_Nomenclature2"));
        AffectTitleList($("#InformationsGenerales_Nomenclature3"));
        AffectTitleList($("#InformationsGenerales_Nomenclature4"));
        AffectTitleList($("#InformationsGenerales_Nomenclature5"));

        $("td[albCFList]").each(function () {
            AffectTitleList($(this));
        });

        if (!($("#ModeReadOnlyInfoGen").val() == "True")) {
            GetComboNomenclature($("#InformationsGenerales_Nomenclature1"), "init");
            GetComboNomenclature($("#InformationsGenerales_Nomenclature2"), "init");
            GetComboNomenclature($("#InformationsGenerales_Nomenclature3"), "init");
            GetComboNomenclature($("#InformationsGenerales_Nomenclature4"), "init");
            GetComboNomenclature($("#InformationsGenerales_Nomenclature5"), "init");
        }
        detailsRisque.formatDecimalValue();
        detailsRisque.formatValeurUniteNumerique();
        //detailsRisque.initCoutM2Display();

        AlternanceLigne("OppositionsBody", "", false, null);

        $("#btnInfoOk").die().live('click', function () {
            switch ($("#hiddenAction").val()) {
                case "CoherenceObjet":
                    CloseCommonFancy();
                    break;
                default:
                    break;
            }
        });

        if (($("#Periodicite").val() == "U" || $("#Periodicite").val() == "E") && $("#InformationsGenerales_DateEntreeGarantie").val() == "") {
            $("#InformationsGenerales_DateEntreeGarantie").val($("#dateDebOffre").val());
            $("#InformationsGenerales_HeureEntreeGarantieHours").val($("#heureDebOffre").val());
            $("#InformationsGenerales_HeureEntreeGarantieMinutes").val($("#minuteDebOffre").val());
            $("#InformationsGenerales_HeureEntreeGarantieMinutes").trigger("change");
        }


        if (($("#Periodicite").val() == "U" || $("#Periodicite").val() == "E") && $("#InformationsGenerales_DateSortieGarantie").val() == "") {
            $("#InformationsGenerales_DateSortieGarantie").val($("#dateFinOffre").val());
            $("#InformationsGenerales_HeureSortieGarantieHours").val($("#heureFinOffre").val());
            $("#InformationsGenerales_HeureSortieGarantieMinutes").val($("#minuteFinOffre").val());
            $("#InformationsGenerales_HeureSortieGarantieMinutes").trigger("change");
        }
    };

    this.formatValeurUniteNumerique = function () {
        AffectTitleList($("#Unite"));
        if ($("#Unite").val() == "%") {
            if (parseInt($("#Valeur").autoNumeric('get')) > 100)
                $("#Valeur").val('');
            $("#Valeur").attr('albMask', 'pourcentnumeric');
            common.autonumeric.apply($("#Valeur"), 'update', 'pourcentnumeric');
        }
        else {
            $("#Valeur").attr('albMask', 'numeric');
            common.autonumeric.apply($("#Valeur"), 'update', 'numeric', null, null, null, '99999999999', '-99999999999');
        }
    };

    //this.initCoutM2Display = function () {
    //    if ($("#Offre_Branche").val() == "IN" && $("#Type").val() == "M2" && $("#MonoObjet").val() === "0") {
    //        $(".inputCoutM2").show();
    //    } else {
    //        $("#CoutM2").val("");
    //        $(".inputCoutM2").hide();
    //    }
    //};

    //-------------- Supprime les garanties --------------
    this.deleteFormuleGarantie = function () {
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeRsq = $("#Code").val();

        $.ajax({
            type: "POST",
            url: "/DetailsRisque/DeleteFormuleGarantie",
            data: { codeOffre: codeOffre, version: version, type: type, codeRsq: codeRsq },
            success: function () {
                $("#oldCible").val($("#InformationsGenerales_Cible").val());
                ReloadNomenclatureCombos($("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), $("#CodeRisque").val(), 0, $("#InformationsGenerales_Cible").val());
                $("#InformationsGenerales_CodeTre").val('');
                $("#hasFormules").val("");

                common.page.isLoading = true;
                $("#txtParamRedirect").val(decodeURIComponent(common.page.currentUrl.replace(/^\//, '')));
                $("#btnSuivant").trigger("click");

                CloseCommonFancy();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };
    //----------------------Met le focus sur le premier controle au démarrage---------------------
    this.setFocusDescriptif = function () {
        $("#InformationsGenerales_Descriptif").focus();
    };
    //----------------------Cache le bouton supprimer lorsque l'on a un seul risque---------------------
    this.hideDelButton = function () {
        if ($("#Code").val() == "0" || parseInt($("#CountRsq").val()) < 2) {
            $("#btnSupprimerDtlRsq").hide();
        }
    };
    //----------------------Formate tous les controles datepicker---------------------
    this.formatDatePicker = function () {
        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    };
    //----------------------Affiche les controles des paramètres de risque---------------------
    this.displayParamRisque = function () {
        $("#RisqueIndexe").offOn("change", function () {
            if ($(this).isChecked()) $("#paramRisqueIndexe").show();
            else $("#paramRisqueIndexe").hide();
        });
    };
    //---------------------Affiche/cache la zone de report saisie-------------------------
    this.affectReport = function () {
        $("#ActiverReport").offOn("change", function () {
            detailsRisque.showHideReport();
        });
    };
    //---------------------Affiche/cache la zone de report saisie-------------------------
    this.showHideReport = function () {
        var rptVal = $("#ReportValeur").val();
        var rptObl = $("#ReportObligatoire").val();

        if ((rptVal == "N" && rptObl == "N") || (rptVal == "O" && rptObl == "O"))
            $("#ActiverReport").attr("disabled", "disabled");

        if ($("#ActiverReport").isChecked()) {
            $("#divInfosReport").hide();
            $("#Valeur").val($("#ValeurObjets").val());
            $("#Unite").val($("#UniteObjets").val());
            $("#Type").val($("#TypeObjets").val());
        }
        else
            $("#divInfosReport").show();

        if (detailsRisque.checkTypeValeur()) {
            $("#TypeObjets").attr("disabled", "disabled").addClass("readonly");
        }
    };
    //---------------Vérifie que tous les type soit identiques---------------
    this.checkTypeValeur = function () {
        var type = "";
        var sameType = true;

        $("td[id=typeValeur]").each(function () {
            if (type == "") {
                type = $.trim($(this).text());
            }
            else {
                if ($.trim($(this).text()) != type) {
                    sameType = false;
                }
            }
        });
        return sameType;
    };
    //---------------------Appel de la méthode detailsRisque.RemoveCSS------------------------
    this.initChangeDateTimeCss = function () {
        $("#InformationsGenerales_DateEntreeGarantie").offOn("change", function () {
            detailsRisque.resetDateTimeCss();
            if ($(this).val() != "") {
                if ($("#InformationsGenerales_HeureEntreeGarantieHours").val() == "") {
                    $("#InformationsGenerales_HeureEntreeGarantieHours").val("00");
                    $("#InformationsGenerales_HeureEntreeGarantieMinutes").val("00");
                    $("#InformationsGenerales_HeureEntreeGarantieHours").trigger("change");
                }
            }
            else {
                $("#InformationsGenerales_HeureEntreeGarantieHours").val("");
                $("#InformationsGenerales_HeureEntreeGarantieMinutes").val("");
                $("#InformationsGenerales_HeureEntreeGarantieHours").trigger("change");
            }
        });
        $("#InformationsGenerales_DateSortieGarantie").offOn("change", function () {
            detailsRisque.resetDateTimeCss();
            if ($(this).val() != "") {
                if ($("#InformationsGenerales_HeureSortieGarantieHours").val() == "") {
                    $("#InformationsGenerales_HeureSortieGarantieHours").val("23");
                    $("#InformationsGenerales_HeureSortieGarantieMinutes").val("59");
                    $("#InformationsGenerales_HeureSortieGarantieHours").trigger("change");
                }
            }
            else {
                $("#InformationsGenerales_HeureSortieGarantieHours").val("");
                $("#InformationsGenerales_HeureSortieGarantieMinutes").val("");
                $("#InformationsGenerales_HeureSortieGarantieHours").trigger("change");
            }
        });

        $("#InformationsGenerales_DateEntreeDescr").offOn("change", function () {
            detailsRisque.resetDateTimeCss();
            if ($(this).val() != "") {
                if ($("#InformationsGenerales_HeureEntreeDescrHours").val() == "") {
                    $("#InformationsGenerales_HeureEntreeDescrHours").val("00");
                    $("#InformationsGenerales_HeureEntreeDescrMinutes").val("00");
                    $("#InformationsGenerales_HeureEntreeDescrHours").trigger("change");
                }
            }
            else {
                $("#InformationsGenerales_HeureEntreeDescrHours").val("");
                $("#InformationsGenerales_HeureEntreeDescrMinutes").val("");
                $("#InformationsGenerales_HeureEntreeDescrHours").trigger("change");
            }
        });
        $("#InformationsGenerales_DateSortieDescr").offOn("change", function () {
            detailsRisque.resetDateTimeCss();
            if ($(this).val() != "") {
                if ($("#InformationsGenerales_HeureSortieDescrHours").val() == "") {
                    $("#InformationsGenerales_HeureSortieDescrHours").val("23");
                    $("#InformationsGenerales_HeureSortieDescrMinutes").val("59");
                    $("#InformationsGenerales_HeureSortieDescrHours").trigger("change");
                }
            }
            else {
                $("#InformationsGenerales_HeureSortieDescrHours").val("");
                $("#InformationsGenerales_HeureSortieDescrMinutes").val("");
                $("#InformationsGenerales_HeureSortieDescrHours").trigger("change");
            }
        });
        $("#InformationsGenerales_HeureEntreeGarantie, #InformationsGenerales_HeureSortieGarantie, #InformationsGenerales_HeureEntreeDescr, #InformationsGenerales_HeureSortieDescr").offOn("change", function () {
            detailsRisque.resetDateTimeCss();
        });
    };
    //---------------------Supprime la classe RequiredField--------------------
    this.resetDateTimeCss = function () {
        $("#InformationsGenerales_DateEntreeGarantie").removeClass("requiredField");
        $("#InformationsGenerales_DateSortieGarantie").removeClass("requiredField");
        $("#InformationsGenerales_HeureEntreeGarantie").removeClass("requiredField");
        $("#InformationsGenerales_HeureSortieGarantie").removeClass("requiredField");
        $("#InformationsGenerales_DateEntreeDescr").removeClass("requiredField");
        $("#InformationsGenerales_DateSortieDescr").removeClass("requiredField");
        $("#InformationsGenerales_HeureEntreeDescr").removeClass("requiredField");
        $("#InformationsGenerales_HeureSortieDescr").removeClass("requiredField");
    };
    //----------------------Suivant---------------------
    this.initSuivant = function () {
        $("#btnSuivant").kclick(function (evt, data) {
            var isModificationAvn = !detailsRisque.isAvnDisabled();
            let returnHome = data && data.returnHome;
            if (isModificationAvn && (!window.isReadonly || window.isModifHorsAvenant)
                && $("#MonoObjet").val() === "0"
                && $("#RedirectRisque").val() === "") {

                var erreurBool = false;
                var erreurMsg = "Erreur : <br/>";


                var errDateDeb = false;
                var errDateFin = false;
                if ($("#InformationsGenerales_DateEntreeGarantie").val() != "" && !isDate($("#InformationsGenerales_DateEntreeGarantie").val())) {
                    $("#InformationsGenerales_DateEntreeGarantie").addClass("requiredField");
                    erreurMsg += "DateEntreeGarantie invalide";
                    erreurBool = true;
                    errDateDeb = true;

                }
                if ($("#InformationsGenerales_DateSortieGarantie").val() != "" && !isDate($("#InformationsGenerales_DateSortieGarantie").val())) {
                    $("#InformationsGenerales_DateSortieGarantie").addClass("requiredField");
                    erreurMsg += "DateSortieGarantie invalide";
                    erreurBool = true;
                    errDateFin = true;


                }
                /* Controle date entrée */

                if ($("#InformationsGenerales_DateEntreeGarantie").val() != "" && $("#dateDebMinObjet").val() != "" && !errDateDeb) {
                    //Test de cohérence entre les dates du risques et les dates des objets
                    var checkDDO = checkDateHeure($("#InformationsGenerales_DateEntreeGarantie"), $("#dateDebMinObjet"), $("#InformationsGenerales_HeureEntreeGarantieHours"), $("#heureDebMinObjet"), $("#InformationsGenerales_HeureEntreeGarantieMinutes"), $("#minuteDebMinObjet"));
                    if (!checkDDO && $("#InformationsGenerales_DateEntreeGarantie").val() != "") {
                        erreurMsg += "<br/>Incohérence dates entrées (Objet/Risque) :<br/>" + $("#dateDebMinObjet").val() + " " + $("#heureDebMinObjet").val() + ":" + $("#minuteDebMinObjet").val();
                        erreurBool = true;
                    }
                }
                if (!errDateDeb && $("#InformationsGenerales_DateEntreeGarantie").val() != "" && $("#dateFinMaxObjet").val() != "") {
                    var checkSDO = checkDateHeure($("#InformationsGenerales_DateEntreeGarantie"), $("#dateFinMaxObjet"), $("#InformationsGenerales_HeureEntreeGarantieHours"), $("#heureFinMaxObjet"), $("#InformationsGenerales_HeureEntreeGarantieMinutes"), $("#minuteFinMaxObjet"));
                    if (!checkSDO) {
                        erreurMsg += "<br/>Incohérence dates entrées (Objet/Risque) :<br/>" + $("#dateFinMaxObjet").val() + " " + $("#heureFinMaxObjet").val() + ":" + $("#minuteFinMaxObjet").val();
                        erreurBool = true;
                    }
                }
                if (!errDateDeb && $("#InformationsGenerales_DateEntreeGarantie").val() != "" && $("#dateFinMinObjet").val() != "") {
                    var checkSDO = checkDateHeure($("#InformationsGenerales_DateEntreeGarantie"), $("#dateFinMinObjet"), $("#InformationsGenerales_HeureEntreeGarantieHours"), $("#heureFinMinObjet"), $("#InformationsGenerales_HeureEntreeGarantieMinutes"), $("#minuteFinMinObjet"));
                    if (!checkSDO) {
                        erreurMsg += "<br/>Incohérence dates entrées (Objet/Risque) :<br/>" + $("#dateFinMinObjet").val() + " " + $("#heureFinMinObjet").val() + ":" + $("#minuteFinMinObjet").val();
                        erreurBool = true;
                    }
                }
                /* Controle date sortie */
                if ($("#InformationsGenerales_DateSortieGarantie").val() != "" && $("#dateFinMaxObjet").val() != "" && !errDateFin) {

                    var checkDFO = checkDateHeure($("#dateFinMaxObjet"), $("#InformationsGenerales_DateSortieGarantie"), $("#heureFinMaxObjet"), $("#InformationsGenerales_HeureSortieGarantieHours"), $("#minuteFinMaxObjet"), $("#InformationsGenerales_HeureSortieGarantieMinutes"));
                    if (!checkDFO && $("#InformationsGenerales_DateSortieGarantie").val() != "") {
                        erreurMsg += "<br/>Incohérence dates sorties (Objet/Risque) :<br/>" + $("#dateFinMaxObjet").val() + " " + $("#heureFinMaxObjet").val() + ":" + $("#minuteFinMaxObjet").val();
                        erreurBool = true;
                    }
                }

                if (!errDateFin && $("#InformationsGenerales_DateSortieGarantie").val() != "" && $("#dateDebMinObjet").val() != "") {
                    var checkSDO = checkDateHeure($("#dateDebMinObjet"), $("#InformationsGenerales_DateSortieGarantie"), $("#heureDebMinObjet"), $("#InformationsGenerales_HeureSortieGarantieHours"), $("#minuteDebMinObjet"), $("#InformationsGenerales_HeureSortieGarantieMinutes"));
                    if (!checkSDO) {
                        erreurMsg += "<br/>Incohérence dates entrées (Objet/Risque) :<br/>" + $("#dateDebMinObjet").val() + " " + $("#heureDebMinObjet").val() + ":" + $("#minuteDebMinObjet").val();
                        erreurBool = true;
                    }
                }

                if (!errDateFin && $("#InformationsGenerales_DateSortieGarantie").val() != "" && $("#dateDebMaxObjet").val() != "") {
                    var checkSDO = checkDateHeure($("#dateDebMaxObjet"), $("#InformationsGenerales_DateSortieGarantie"), $("#heureDebMaxObjet"), $("#InformationsGenerales_HeureSortieGarantieHours"), $("#minuteDebMaxObjet"), $("#InformationsGenerales_HeureSortieGarantieMinutes"));
                    if (!checkSDO) {
                        erreurMsg += "<br/>Incohérence dates entrées (Objet/Risque) :<br/>" + $("#dateDebMaxObjet").val() + " " + $("#heureDebMaxObjet").val() + ":" + $("#minuteDebMaxObjet").val();
                        erreurBool = true;
                    }
                }


                if (erreurBool && erreurMsg != "Erreur : <br/>") {
                    $("#RedirectRisque").val("");
                    $("#OpenObjet").val("");
                    $("#txtParamRedirect").val("");
                    ShowCommonFancy("Info", "CoherenceObjet", erreurMsg, 400, 200, true, true, true);
                }
                else {
                    detailsRisque.saveRisque(returnHome);
                }
            }
            else {
                detailsRisque.saveRisque(returnHome);
            }
        });
    };
    //----------Suite de la sauvegarde du risque--------
    this.saveRisque = function (returnHome) {
        let Periodicite = $("#Periodicite").val();

        let typeMsgOffre = "de l'offre";
        if ($("#Offre_Type").val() == "P") {
            typeMsgOffre = "du contrat";
        }
        $(".requiredField").removeClass("requiredField");

        let erreurBool = false;
        let erreurMsg = "Erreur : <br/>";
        $("#InformationsGenerales_Descriptif").val($.trim($("#InformationsGenerales_Descriptif").val()));
        let isModificationAvn = !detailsRisque.isAvnDisabled();

        if (isModificationAvn && (!window.isReadonly || window.isModifHorsAvenant) && $("#RedirectRisque").val() == "") {
            if ($("#InformationsGenerales_Descriptif").val() == "" && $("#RedirectRisque").val() != $("#RedirectRsqAddObj").val()) {
                $("#InformationsGenerales_Descriptif").addClass("requiredField");
                erreurBool = true;
            }

            var errDateDeb = false;
            var errDateFin = false;
            if ($("#InformationsGenerales_DateEntreeGarantie").val() != "" && !isDate($("#InformationsGenerales_DateEntreeGarantie").val())) {
                $("#InformationsGenerales_DateEntreeGarantie").addClass("requiredField");
                erreurBool = true;
                errDateDeb = true;
            }
            if ($("#InformationsGenerales_DateSortieGarantie").val() != "" && !isDate($("#InformationsGenerales_DateSortieGarantie").val())) {
                $("#InformationsGenerales_DateSortieGarantie").addClass("requiredField");
                erreurBool = true;
                errDateFin = true;
            }

            if ($("#Offre_Type").val() == "P" && (Periodicite == "U" || Periodicite == "E" || $("#chkIsRisqueTemporaire").is(":checked"))) {
                if ($("#InformationsGenerales_DateEntreeGarantie").val() == "") {
                    $("#InformationsGenerales_DateEntreeGarantie").addClass("requiredField");
                    erreurMsg += "<br/>La date d'entrée est obligatoire si le risque est temporaire.";
                    erreurBool = true;
                }
                if ($("#InformationsGenerales_DateSortieGarantie").val() == "") {
                    $("#InformationsGenerales_DateSortieGarantie").addClass("requiredField");
                    erreurMsg += "<br/>La date de sortie est obligatoire si le risque est temporaire.";
                    erreurBool = true;
                }
            }

            if (!errDateDeb && $("#InformationsGenerales_DateEntreeGarantie").val() != "" && $("#dateDebOffre").val() != "" && $("#IsAvenantModificationLocale").attr("id") == undefined) {
                //Test de cohérence entre les dates du risques et les dates de l'offre
                var checkDDO = checkDateHeure($("#dateDebOffre"), $("#InformationsGenerales_DateEntreeGarantie"), $("#heureDebOffre"), $("#InformationsGenerales_HeureEntreeGarantieHours"), $("#minuteDebOffre"), $("#InformationsGenerales_HeureEntreeGarantieMinutes"));
                if (!checkDDO && $("#InformationsGenerales_DateEntreeGarantie").val() != "") {
                    erreurMsg += "<br/>La date d'entrée est incohérente avec la date d'effet " + typeMsgOffre + " :<br/>" + $("#dateDebOffre").val() + " " + $("#heureDebOffre").val() + ":" + $("#minuteDebOffre").val();
                    erreurBool = true;
                }
            }

            if (!errDateFin && $("#InformationsGenerales_DateSortieGarantie").val() != "" && $("#dateFinOffre").val() != "") {
                //Test de cohérence entre les dates du risques et les dates de l'offre
                var checkDFO = checkDateHeure($("#InformationsGenerales_DateSortieGarantie"), $("#dateFinOffre"), $("#InformationsGenerales_HeureSortieGarantieHours"), $("#heureFinOffre"), $("#InformationsGenerales_HeureSortieGarantieMinutes"), $("#minuteFinOffre"));
                if (!checkDFO && $("#InformationsGenerales_DateSortieGarantie").val() != "") {
                    erreurMsg += "<br/>La date de sortie est incohérente avec la date de fin d'effet " + typeMsgOffre + " :<br/>" + $("#dateFinOffre").val() + " " + $("#heureFinOffre").val() + ":" + $("#minuteFinOffre").val();
                    erreurBool = true;
                }
            }

            if (!errDateDeb && !errDateFin && $("#InformationsGenerales_DateEntreeGarantie").val() != "" && $("#InformationsGenerales_DateSortieGarantie").val() != "") {
                //Test de cohérence entre la date de début et la date de fin
                var checkDH = checkDateHeure($("#InformationsGenerales_DateEntreeGarantie"), $("#InformationsGenerales_DateSortieGarantie"), $("#InformationsGenerales_HeureEntreeGarantieHours"), $("#InformationsGenerales_HeureSortieGarantieHours"), $("#InformationsGenerales_HeureEntreeGarantieMinutes"), $("#InformationsGenerales_HeureSortieGarantieMinutes"));
                if (!checkDH) {
                    erreurMsg += "<br/>Veuillez sélectionner une date de début antérieure<br/> à la date de fin.";
                    erreurBool = true;
                }
            }
            var errDateDescDeb = false;
            var errDateDescFin = false;
            if ($("#InformationsGenerales_DateEntreeDescr").val() != "" && !isDate($("#InformationsGenerales_DateEntreeDescr").val())) {
                $("#InformationsGenerales_DateEntreeDescr").addClass("requiredField");
                erreurBool = true;
                errDateDescDeb = true;
            }
            if ($("#InformationsGenerales_DateSortieDescr").val() != "" && !isDate($("#InformationsGenerales_DateSortieDescr").val())) {
                $("#InformationsGenerales_DateSortieDescr").addClass("requiredField");
                erreurBool = true;
                errDateDescFin = true;
            }
            if (!errDateDescDeb && !errDateDescFin && $("#InformationsGenerales_DateEntreeDescr").val() != "" && $("#InformationsGenerales_DateSortieDescr").val() != "") {
                //Test de cohérence entre la date de début et la date de fin descriptives
                var checkDD = checkDateHeure($("#InformationsGenerales_DateEntreeDescr"), $("#InformationsGenerales_DateSortieDescr"), $("#InformationsGenerales_HeureEntreeDescrHours"), $("#InformationsGenerales_HeureSortieDescrHours"), $("#InformationsGenerales_HeureEntreeDescrMinutes"), $("#InformationsGenerales_HeureSortieDescrMinutes"));
                if (!checkDD) {
                    $("#InformationsGenerales_DateEntreeDescr").addClass('requiredField');
                    $("#InformationsGenerales_DateSortieDescr").addClass('requiredField');
                    erreurMsg += "<br/>Les dates descriptives ne sont pas cohérentes.";
                    erreurBool = true;
                }
            }

            if (!$("#ActiverReport").isChecked()) {
                if ($("#Valeur").val() != "" && $("#Valeur").val() != "0" && ($("#Unite").val() == "" || ($("#Type").val() == "" && $("#Offre_CodeOffre").val().indexOf("CV"))) < 0) {
                    $("#Unite").addClass('requiredField');
                    $("#Type").addClass('requiredField');
                    erreurBool = true;
                }

                if (($("#Valeur").val() == "0" || $("#Valeur").val() == "") && ($("#Unite").val() != "" || $("#Unite").val() != "")
                    && $("#TypePolice").val() != "M" && $("#Periodicite").val() != "R") {
                    $("#Valeur").addClass('requiredField');
                    erreurBool = true;
                }
            }



            if ($("#divInfosReport").css('display') != 'none') {
                var str = "Type" + $("#Type").val();
                var controleTPCN1 = $("#" + str).attr('albDescriptif');
                if (controleTPCN1 != "1") {
                    if (($("#ValeurHT").val() == "") && ($("#Unite").val() == "D") && $("#TypePolice").val() != "M" && $("#Periodicite").val() != "R") {
                        $("#ValeurHT").addClass("requiredField");
                        erreurBool = true;
                    }
                }
            }


            if ($("#InformationsGenerales_Cible").val() == undefined || $("#InformationsGenerales_Cible").val() == "") {
                $("#InformationsGenerales_Cible").addClass('requiredField');
                erreurBool = true;
            }

            if (erreurBool) {
                $("#RedirectRisque").val("");
                $("#OpenObjet").val("");
                $("#txtParamRedirect").val("");
                if (erreurMsg != "Erreur : <br/>")
                    ShowCommonFancy("Error", "", erreurMsg, 400, 200, true, true, true);
                return false;
            }


            //verif mode avenant
            if ($("#IsAvenantModificationLocale").attr("id") != undefined && isModificationAvn && (!window.isReadonly || window.isModifHorsAvenant) && $('#chkModificationAVN').isChecked()) {
                if ($("#ProchEch").val() != "") {
                    if (!checkDate($("#DateEffetAvenantModificationLocale"), $("#ProchEch"))) {
                        $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                        erreurMsg += "<br/>La date de modification du risque " + $("#Code").val() + " doit être inférieure à la date de prochaine échéance " + $("#ProchEch").val();
                        //erreurMsg += "<br/>Date de modification incohérente avec la prochaine échéance";
                        erreurBool = true;
                    }
                }

                var checkDateEffetAvn = checkDate($("#DateEffetAvenant"), $("#DateEffetAvenantModificationLocale"));
                if (!checkDateEffetAvn) {
                    $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                    erreurMsg += "<br/>La date de modification du risque " + $("#Code").val() + " doit être supérieure à la date d'effet de l'avenant " + $("#DateEffetAvenant").val();
                    erreurBool = true;
                }

                if ($("#dateFinOffre").val() != "") {
                    if (!checkDate($("#DateEffetAvenantModificationLocale"), $("#dateFinOffre"))) {
                        $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                        erreurMsg += "<br/>La date de modification du risque " + $("#Code").val() + " doit être inférieure à la date de fin d'effet " + $("#dateFinOffre").val();
                        erreurBool = true;
                    }
                }


                if (($("#InformationsGenerales_DateEntreeGarantie").val() != "" && !$("#InformationsGenerales_DateEntreeGarantie").is('[readonly]'))) {

                    if (getDate($("#InformationsGenerales_DateEntreeGarantie").val()) < getDate(incrementDate($("#DateEffetAvenantModificationLocale").val(), -1, 0, 0, 0))) {
                        $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                        $("#InformationsGenerales_DateEntreeGarantie").addClass("requiredField");
                        erreurMsg += "<br/>La date la date d'entrée du risque " + $("#InformationsGenerales_DateEntreeGarantie").val() + " doit être supérieure à la date de modification du risque ";
                        erreurBool = true;
                    }
                }


                if ($("#InformationsGenerales_DateSortieGarantie").val() != "") {

                    if (getDate($("#InformationsGenerales_DateSortieGarantie").val(), $("#InformationsGenerales_HeureSortieGarantie").val()) < getDate(incrementDate($("#DateEffetAvenantModificationLocale").val(), -1, 0, 0, 0), "23:59:59")) {
                        $("#InformationsGenerales_DateSortieGarantie").addClass("requiredField");
                        $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                        erreurMsg += "<br/>La date de modification du risque " + $("#Code").val() + " doit être inférieure à la date de sortie du risque " + $("#InformationsGenerales_DateSortieGarantie").val();
                        erreurBool = true;
                    }

                }

                //Test de cohérence entre la date de modif et les dates des objets
                if ($("#ModeMultiObjet").val() == "True" && $("#DateMinModifObjet").val() != "" )  {
                    if ($("#DateMinModifObjet").val() != "" && getDate($("#DateMinModifObjet").val()) < getDate($("#DateEffetAvenantModificationLocale").val())) {
                        $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                        erreurMsg += "<br/>La date de modification du risque doit être inférieure ou égale aux dates de modification des objets  :<br/>" + $("#DateMinModifObjet").val();
                        erreurBool = true;
                    }
                }

                if ($("#dateDebObjetAvn").val() != "" && getDate($("#dateDebObjetAvn").val()) < getDate($("#DateEffetAvenantModificationLocale").val())) {
                    $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                    erreurMsg += "<br/>La date d'entrée des objets doit être supérieure à la date de modification du risque :<br/>" + $("#DateEffetAvenantModificationLocale").val();
                    erreurBool = true;
                }

                if ($("#dateFinObjetAvn").val() != "" && getDate($("#dateFinObjetAvn").val()) < getDate($("#DateEffetAvenantModificationLocale").val())) {
                    $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                    erreurMsg += "<br/>La date de sortie des objets doit être supérieure à la date de modification du risque :<br/>" + $("#DateEffetAvenantModificationLocale").val();
                    erreurBool = true;
                }

                if ($("#DateDebHisto").val() != "" && getDate($("#InformationsGenerales_DateEntreeGarantie").val(), $("#InformationsGenerales_HeureEntreeGarantie").val()) < getDate($("#DateDebHisto").val(), $("#HeureDebHisto").val())) {
                    $("#InformationsGenerales_DateEntreeGarantie").addClass("requiredField");
                    erreurBool = true;
                    erreurMsg += "<br/>La date de début (" + $("#InformationsGenerales_DateEntreeGarantie").val() + " " + $("#InformationsGenerales_HeureEntreeGarantie").val() + ") doit être<br/> supérieure à la date d'historique (" + $("#DateDebHisto").val() + " " + $("#HeureDebHisto").val() + ")";
                }

                if (erreurBool) {
                    $("#RedirectRisque").val("");
                    $("#OpenObjet").val("");
                    $("#txtParamRedirect").val("");
                    if (erreurMsg != "Erreur : <br/>") {
                        ShowCommonFancy("Error", "", erreurMsg, 400, 200, true, true, true);
                    }
                    return false;
                }
            }
        }

        let allDisabled = $("[disabled]");
        try {
            common.page.isLoading = true;
            if ($("#IsAvenantModificationLocale").exists()) {
                $("#IsAvenantModificationLocale").val($("#chkModificationAVN").isChecked() ? "True" : "False");
            }
            // Suppression de l'espace du formatage autonumeric pour récupérer la valeur dans le Json
            common.autonumeric.applyAll('update', 'numeric', '', null, null, '99999999999', '-99999999999');

            allDisabled.enable();
            let obj = $(':input').serializeObject(false, function (name, o) {
                let rg = /InformationsGenerales\.ListesNomenclatures\.Nomenclature\d+\.Nomenclature/g;
                if (rg.test(name)) {
                    let opt = $("[name='" + name + "'] option:selected");
                    o[name] = opt.exists() ? opt.text().split(' - ')[0].trim() : "";
                    return true;
                }
                return false;
            });
            obj.txtSaveCancel = returnHome ? 1 : 0;
            let formDataInit = JSON.stringify(obj);
            let formData = formDataInit;
            if ($("#Offre_Type").val() == "P") {
                formData = formDataInit.replace("Contrat.CodeContrat", "Offre.CodeOffre").replace("Contrat.VersionContrat", "Offre.Version").replace("Contrat.Type", "Offre.Type").replace("Contrat.AddParamType", "AddParamType").replace("Contrat.AddParamValue", "AddParamValue");
            }
            allDisabled.disable();
            $.ajax({
                type: "POST",
                url: "/DetailsRisque/DetailsRisqueEnregistrer",
                context: $('#JQueryHttpPostResultDiv'),
                data: formData,
                contentType: "application/json",
                success: function (data) {
                    if (data == "" && $("#RedirectRisque").val() == $("#RedirectRsqOpenIS").val()) {
                        $("#RedirectRisque").clear();
                        setTimeout(function () {
                            detailsRisque.openInformationsSpecifiquesRisque();
                        }, 5);
                    }
                },
                error: function (request) {
                    common.error.showXhr(request, true);
                    common.autonumeric.applyAll('update', 'numeric', null, null, null, '9999999999999', '-99999999999');
                    $("#RedirectRsqAddObj").val("");
                    common.page.isLoading = false;
                }
            });
        }
        catch (e) {
            console.error(e);
            common.autonumeric.applyAll('update', 'numeric', null, null, null, '9999999999999', '-99999999999');
            $("#RedirectRsqAddObj").val("");
            allDisabled.prop({ disabled: true });
            common.page.isLoading = false;
        }
    };

    //-----------------Redirection----------------------
    this.redirect = function (cible, job, codeRisque, codeObjet, readonlyDisplay) {
        common.page.isLoading = true;
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();
        var isForceReadOnly = $("#IsForceReadOnly").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();

        $.ajax({
            type: "POST",
            url: "/DetailsRisque/Redirection/",
            data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeRisque: codeRisque, codeObjet: codeObjet, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue, readonlyDisplay: readonlyDisplay, isModeConsultationEcran: $("#IsModeConsultationEcran").val(), isForceReadOnly: isForceReadOnly },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
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

    //----------------------Suppression du risque-----------------------
    this.deleteRisque = function () {
        var codeOffre = $("#Offre_CodeOffre").val();
        var versionOffre = $("#Offre_Version").val();
        var typeOffre = $("#Offre_Type").val();
        var codeRisque = $("#Code").val();
        var codeBranche = $("#Branche").val();
        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();

        var id = codeOffre + "_" + versionOffre + "_" + typeOffre + "_" + codeRisque + "_" + codeBranche;

        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/DetailsRisque/DetailsRisquesSupprimer",
            data: { id: id, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
            success: function (data) {
                common.page.isLoading = false;
            },
            error: function (jqxhr, data, erreur) {
                common.error.showXhr(request);
            }
        });

    };

    //-------Formate les input/span des valeurs----------
    this.formatDecimalValue = function () {
        common.autonumeric.applyAll('init', 'numeric', null, null, null, '99999999999', '-99999999999');
        common.autonumeric.applyAll('init', 'decimal');
        common.autonumeric.applyAll('init', 'pourcentnumeric');
    };
    
    this.initChangeNomenclature1 = function () {
        $("#InformationsGenerales_Nomenclature1").offOn("change", function () {
            $("#InformationsGenerales_Descriptif").val($.trim($('#InformationsGenerales_Nomenclature1 option:selected').text().split('-')[1]));
        })
    };

    //-----Region Informations spécifiques risque
    this.openInformationsSpecifiquesRisque = function () {
        common.page.isLoading = true;
        let modeNavig = "modeNavig" + $("#ModeNavig").val() + "modeNavig";
        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let type = $("#Offre_Type").val();
        let codeRisque = $("#Code").val();
        let addParamType = $("#AddParamType").val();
        let addParamValue = $("#AddParamValue").val();
        let addParamString = addParamType && addParamValue ? ("addParam" + addParamType + "|||" + addParamValue + ($("#IsForceReadOnly").val() == "True" ? "||FORCEREADONLY|1" : "") + "addParam") : "";
        let id = "";
        let tabGuid = $("#tabGuid").val();
        id = codeOffre + "_" + version + "_" + type + "_" + codeRisque + tabGuid + addParamString + modeNavig;

        $.ajax({
            type: "POST",
            url: "/InformationsSpecifiquesRisques/OpenInfoSpecifiquesRisque",
            data: {
                id: id
            },
            success: function (data) {
                DesactivateShortCut();
                $("#divInformationsSpecifiquesRisque").show();
                $("#divDataInformationsSpecifiquesRisque").html(data);
                setTimeout(function () {
                    infosSpeRisque.mapPageElementISRisque();
                    infosSpe.load();
                }, 100);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    //---------------- Affichage du CommonFancy Risque
    this.showCommonFancyRsq = function (idDiv, action, text, width, height, modal, displayTop, blockDim) {
        $("#hiddenActionRsq").val(action);
        if (action == "Cancel" && (window.isReadonly || detailsRisque.isAvnDisabled())) {
            $("#btnConfirmOkRsq").click();
            return;
        }
        if (width > 900) {
            $("#msgError").removeClass();
        }
        if (displayTop == true) {
            AlbScrollTop();
        }
        if (text == undefined)
            blockDim = true;
        if (!blockDim) {
            if (text.length > 500) {
                width = 1212;
                height = 500;
            }
            else {
                if (text.length > 70 && text.length < 500) {
                    width = 350;
                    height = 150;
                } else {
                    if (idDiv == "Error") {
                        if (width == "" || width == 0 || text.length < 500) {
                            width = 300;
                        }
                        if (height == "" || height == 0 || text.length < 500) {
                            height = 100;
                        }
                        //width = 300;

                    }
                }
            }
        }
        $("#msg" + idDiv).html(text);
        $.fancybox(
            $("#fancy" + idDiv).html(),
            {
                'autoDimensions': false,
                'width': width,
                'height': height,
                'transitionIn': 'elastic',
                'transitionOut': 'elastic',
                'speedin': 400,
                'speedOut': 400,
                'easingOut': 'easeInBack',
                'modal': modal,
                'topRatio': 0
            }
        );
        $("#fancybox-wrap").css('background-color', 'grey');
        if (displayTop == true)
            $("#fancybox-wrap").addClass('fancybox-wrap_Top');
        else $("#fancybox-wrap").removeClass('fancybox-wrap_Top');
    };
    //------- Fermeture du plein écran
    this.closeFullScreen = function () {
        $('#divFullScreenObjets').hide();
        if ($("#chkDisplayObjetsSortisPE").isChecked()) {
            $("#chkDisplayObjetsSortis").attr('checked', 'checked');
        }
        else {
            $("#chkDisplayObjetsSortis").removeAttr('checked');
        }
        $("#chkDisplayObjetsSortis").trigger("change");
    };

    //----------Ouverture du plein ecran
    this.openFullScreen = function () {
        $('#divFullScreenObjets').show();
        if ($("#chkDisplayObjetsSortis").isChecked()) {
            $("#chkDisplayObjetsSortisPE").attr('checked', 'checked');
        }
        else {
            $("#chkDisplayObjetsSortisPE").removeAttr('checked');
        }
        $("#chkDisplayObjetsSortisPE").trigger("change");
    };
};

var detailsRisque = new DetailsRisque();
$(function () {
    detailsRisque.setFocusDescriptif();
    detailsRisque.formatDatePicker();
    detailsRisque.displayParamRisque();
    detailsRisque.initSuivant();
    detailsRisque.checkFields();
    detailsRisque.initChangeDateTimeCss();
    detailsRisque.hideDelButton();
    detailsRisque.affectReport();
    detailsRisque.showHideReport();
    detailsRisque.initChangeHour();
    detailsRisque.mapPageElement();
    detailsRisque.initChangeNomenclature1();
    $("#executeLoadIs").val("false");
});
