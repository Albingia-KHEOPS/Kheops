/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />

function DetailsObjet() {
    //---------------------Affecte les fonctions sur les controles heures-----------------------
    this.initChangeHour = function() {
        $("#InformationsGenerales_HeureEntreeGarantieHours").live('change', function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureEntreeGarantieMinutes").val() == "") {
                $("#InformationsGenerales_HeureEntreeGarantieMinutes").val("00");
            }
            detailsObjet.setHour($(this));
        });
        $("#InformationsGenerales_HeureEntreeGarantieMinutes").live('change', function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureEntreeGarantieHours").val() == "")
                $("#InformationsGenerales_HeureEntreeGarantieHours").val("00");
            detailsObjet.setHour($(this));
        });
        $("#InformationsGenerales_HeureSortieGarantieHours").live('change', function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureSortieGarantieMinutes").val() == "")
                $("#InformationsGenerales_HeureSortieGarantieMinutes").val("00");
            detailsObjet.setHour($(this));
        });
        $("#InformationsGenerales_HeureSortieGarantieMinutes").live('change', function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureSortieGarantieHours").val() == "")
                $("#InformationsGenerales_HeureSortieGarantieHours").val("00");
            detailsObjet.setHour($(this));
        });
        $("#InformationsGenerales_HeureEntreeDescrHours").live('change', function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureEntreeDescrMinutes").val() == "")
                $("#InformationsGenerales_HeureEntreeDescrMinutes").val("00");
            detailsObjet.setHour($(this));
        });
        $("#InformationsGenerales_HeureEntreeDescrMinutes").live('change', function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureEntreeDescrHours").val() == "")
                $("#InformationsGenerales_HeureEntreeDescrHours").val("00");
            detailsObjet.setHour($(this));
        });
        $("#InformationsGenerales_HeureSortieDescrHours").live('change', function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureSortieDescrMinutes").val() == "")
                $("#InformationsGenerales_HeureSortieDescrMinutes").val("00");
            detailsObjet.setHour($(this));
        });
        $("#InformationsGenerales_HeureSortieDescrMinutes").live('change', function () {
            if ($(this).val() != "" && $("#InformationsGenerales_HeureSortieDescrHours").val() == "")
                $("#InformationsGenerales_HeureSortieDescrHours").val("00");
            detailsObjet.setHour($(this));
        });
    };
    //-------------------Renseigne l'heure---------------------------
    this.setHour = function (elem) {
        var elemId = elem.attr('id').replace("Hours", "").replace("Minutes", "")

        var changeHour = SetHours(elemId);
        if (!changeHour && elem.val() == "") {
            $("#" + elemId + "Hours").val("");
            $("#" + elemId + "Minutes").val("");
        }
    };
    //---------------------Affecte les fonctions au boutons-------------
    this.mapPageElement = function () {

        if (!window.isReadonly && !$("#IsAvnDisabled").hasTrueVal())
            //FormatCLEditor($("#InformationsGenerales_Designation"), 380, 90);
            $("#InformationsGenerales_Designation").html($("#InformationsGenerales_Designation").html().replace(/&lt;br&gt;/ig, "\n"));
        else {
            $("#btnSupprimerObj").hide();
            $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
        }
        if ((parseInt($("#CountObj").val()) > 1) || (parseInt($("#CountObj").val()) == 1 && $("#CodeObjet").val() == "")) {
            $("#InformationsGenerales_Cible").attr('disabled', 'disabled').addClass('readonly');
        }
        $("#oldCible").val($("#InformationsGenerales_Cible").val());
        $("#InformationsGenerales_Cible").die().live('change', function () {
            AffectTitleList($(this));
            if ($("#hasFormules").val() == "True")
                ShowCommonFancy("Confirm", "Cible", "Attention, la modification d'une cible entraine la régénération de certaines informations du risque ou de ses objets, ainsi que la suppression des formules associées au risque. <br/>Voulez-vous continuer ?", 300, 120, true, true);
            //ShowCommonFancy("Confirm", "Cible", "Attention, vous changez la cible alors que vous avez déjà sélectionné des garanties.<br/> Celles-ci seront supprimées. Voulez-vous continuer ?", 300, 120, true, true);
            else {
                $("#oldCible").val($("#InformationsGenerales_Cible").val());
                ReloadNomenclatureCombos($("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), $("#CodeRisque").val(), $("#CodeObjet").val(), $(this).val());
                $("#InformationsGenerales_CodeTre").val('');
            }
        });

        $("#btnModifierValeur").die().live("click", function () {
            $("#inEditValeurAvn").val($("#InformationsGenerales_Valeur").val());
            $("#divModifierValeur").show();
        });

        $("#btnModifValAnnuler").die().live("click", function () {
            $("#divModifierValeur").hide();
        });

        $("#btnModifValValider").die().live("click", function () {
            detailsObjet.saveValeurModeAvenant();
        });

        $("#btnErrorOk").live('click', function () {
            CloseCommonFancy();
        });
        $("#btnAnnuler").live('click', function () {
            ShowCommonFancy("Confirm", "Cancel",
                "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
        });
        $("#btnSupprimerObj").live('click', function () {
            ShowCommonFancy("Confirm", "Objet",
                "Vous êtes sur le point de supprimer l'objet.<br/>Voulez-vous continuer ?",
                350, 130, true, true);
        });
        $("#btnConfirmOk").live('click', function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "Inventaire":
                    detailsObjet.deleteInventaire($("#hiddenInventId").val());
                    $("#hiddenInventId").val('');
                    break;
                case "Objet":
                    detailsObjet.deleteRisque();
                    break;
                case "Cible":
                    detailsObjet.deleteFormuleGarantie();
                    $("#InformationsGenerales_CodeTre").val('');
                    break;
                case "Cancel":
                    detailsObjet.cancelForm();
                    break;
            }
            $("#hiddenAction").val('');
        });
        $("#btnConfirmCancel").live('click', function () {
            CloseCommonFancy();
            if ($("#hiddenAction").val() == "Cible")
                $("#InformationsGenerales_Cible").val($("#oldCible").val());
            $("#hiddenAction").val('');
        });
        $("label[name=OpenInven]").each(function () {
            $(this).click(function () {
                var param = $(this).attr('param');
                $("#paramOpenInven").val(param);
                $("#btnSuivant").click();
            });
        });
        toggleDescription($("#InformationsGenerales_Designation"));

        $("#InformationsGenerales_Nomenclature2").die().live('change', function () {
            detailsObjet.initCoutM2Display();
        });

        $("#InformationsGenerales_Unite").live("change", function () {
            detailsObjet.formatValeurUniteNumerique();
        });

        $("#InformationsGenerales_Type").die().live('change', function () {
            AffectTitleList($(this));
            detailsObjet.initCoutM2Display();
        });

        $("#InformationsGenerales_ValeurHT").die().live('change', function () {
            AffectTitleList($(this));
        });

        $("#InformationsGenerales_CodeApe").die().live('change', function () {
            AffectTitleList($(this));
        });

        $("#InformationsGenerales_CodeClasse").die().live('change', function () {
            AffectTitleList($(this));
        });

        $("#InformationsGenerales_Territorialite").die().live('change', function () {
            AffectTitleList($(this));
        });

        $("#ListInventaires_TypeInventaire").die().live('change', function () {
            AffectTitleList($(this));
        });

        $("#InformationsGenerales_TypeRisque").die().live("change", function () {
            AffectTitleList($(this));
        });
        $("#InformationsGenerales_TypeMateriel").die().live('change', function () {
            AffectTitleList($(this));
            $("#InformationsGenerales_Descriptif").val($("div[id='InformationsGenerales_TypeMateriel" + $("#InformationsGenerales_TypeMateriel").val() + "']").attr('albDescriptif'));
        });
        $("#InformationsGenerales_NatureLieux").die().live("change", function () {
            AffectTitleList($(this));
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
                //si multi-objet
                if ($("#ModeMultiObjet").val() == "True" || $("#CodeObjet").val() == "0") {
                    if ($("#InformationsGenerales_DateEntreeGarantie").val() == "" && $("#dateDebRsq").val() != "") {
                        $("#InformationsGenerales_DateEntreeGarantie").val($("#dateDebRsq").val());
                        $("#InformationsGenerales_HeureEntreeGarantieHours").val($("#heuresDebRsq").val());
                        $("#InformationsGenerales_HeureEntreeGarantieMinutes").val($("#minutesDebRsq").val());
                        $("#InformationsGenerales_HeureEntreeGarantieHours").trigger("change");
                    }
                    if ($("#InformationsGenerales_DateSortieGarantie").val() == "" && $("#dateFinRsq").val() != "") {
                        $("#InformationsGenerales_DateSortieGarantie").val($("#dateFinRsq").val());
                        $("#InformationsGenerales_HeureSortieGarantieHours").val($("#heuresFinRsq").val());
                        $("#InformationsGenerales_HeureSortieGarantieMinutes").val($("#minutesFinRsq").val());
                        $("#InformationsGenerales_HeureSortieGarantieHours").trigger("change");
                    }
                }
                //si mono-objet
                else {
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
            }
        });

        //Initialisation
        AffectTitleList($("#InformationsGenerales_Unite"));
        AffectTitleList($("#InformationsGenerales_Type"));
        AffectTitleList($("#InformationsGenerales_ValeurHT"));
        AffectTitleList($("#InformationsGenerales_CodeApe"));
        AffectTitleList($("#InformationsGenerales_CodeTre"));
        AffectTitleList($("#InformationsGenerales_CodeClasse"));
        AffectTitleList($("#InformationsGenerales_Territorialite"));
        AffectTitleList($("#InformationsGenerales_Nomenclature1"));
        AffectTitleList($("#ListInventaires_TypeInventaire"));
        AffectTitleList($("#InformationsGenerales_TypeRisque"));
        AffectTitleList($("#InformationsGenerales_TypeMateriel"));
        AffectTitleList($("#InformationsGenerales_NatureLieux"));
        AffectTitleList($("#InformationsGenerales_Cible"));
        
        GetComboNomenclature($("#InformationsGenerales_Nomenclature1"), "init");
        GetComboNomenclature($("#InformationsGenerales_Nomenclature2"), "init");
        GetComboNomenclature($("#InformationsGenerales_Nomenclature3"), "init");
        GetComboNomenclature($("#InformationsGenerales_Nomenclature4"), "init");
        GetComboNomenclature($("#InformationsGenerales_Nomenclature5"), "init");

        common.autonumeric.applyAll('init', 'numeric', null, null, null, '99999999999', '-99999999999');
        common.autonumeric.applyAll('init', 'decimal', null, null, null, '99999999999.99', '-99999999999.99');
        common.autonumeric.applyAll('init', 'pourcentnumeric');

        detailsObjet.formatValeurUniteNumerique();
        detailsObjet.initCoutM2Display();

        if (($("#Periodicite").val() == "U" || $("#Periodicite").val() == "E") && $("#InformationsGenerales_DateEntreeGarantie").val() == "") {
            $("#InformationsGenerales_DateEntreeGarantie").val($("#dateDebOffre").val());
            $("#InformationsGenerales_HeureEntreeGarantieHours").val($("#heureDebOffre").val());
            $("#InformationsGenerales_HeureEntreeGarantieMinutes").val($("#minuteDebOffre").val());
            $("#InformationsGenerales_HeureEntreeGarantieMinutes").trigger("change");
        }

        //chargement systématique de la date de fin en mono-objet (dates V5)
        if ($("#InformationsGenerales_DateSortieGarantie").val() == "") {
            if ($("#CountObj").val() == "0") {
                $("#InformationsGenerales_DateSortieGarantie").val($("#dateFinOffre").val());
                $("#InformationsGenerales_HeureSortieGarantieHours").val($("#heureFinOffre").val());
                $("#InformationsGenerales_HeureSortieGarantieMinutes").val($("#minuteFinOffre").val());
                $("#InformationsGenerales_HeureSortieGarantieMinutes").trigger("change");
            }
        }

        var isModificationAvn = true;

        if ($("#CheckModificationAVN").attr("id") !== undefined)
            isModificationAvn = $('#chkModificationAVN').is(':checked');

        if ($('#IsModifHorsAvn').val() === 'True' && !isModificationAvn) {
            $('#InformationsGenerales_Descriptif').attr('disabled', 'disabled');
            $('div[name=btnAdresse]').removeClass('CursorPointer');
            $('#InformationsGenerales_CodeTre').attr('disabled', 'disabled');
        }
    };

    this.formatValeurUniteNumerique = function () {
        if ($("#InformationsGenerales_Unite").val() == "%") {
            if (parseInt($("#InformationsGenerales_Valeur").autoNumeric('get')) > 100)
                $("#InformationsGenerales_Valeur").val('');
            $("#InformationsGenerales_Valeur").attr('albMask', 'pourcentnumeric');
            common.autonumeric.apply($("#InformationsGenerales_Valeur"), 'update', 'pourcentnumeric');
        }
        else {
            $("#InformationsGenerales_Valeur").attr('albMask', 'numeric');
            common.autonumeric.apply($("#InformationsGenerales_Valeur"), 'update', 'numeric', null, null, null, '99999999999', '-99999999999');
        }
        AffectTitleList($("#InformationsGenerales_Unite"));
    };

    this.initCoutM2Display = function () {
        common.autonumeric.apply($("#InformationsGenerales_CoutM2"), 'update', 'numeric', null, null, null, '999999', '0');
        if ($("#Offre_Branche").val() == "IN" && $("#InformationsGenerales_Type").val() == "M2" && $("#InformationsGenerales_Nomenclature2").val() == "20073") {
            $(".inputCoutM2").show();
            if ($("#InformationsGenerales_Unite").val() != "N") {
                $("#InformationsGenerales_Unite").val("");
            }
            $("#InformationsGenerales_Unite").children(":not(option[value='N'], option[value=''])").hide();
        } else {
            $("#InformationsGenerales_CoutM2").val("");
            $(".inputCoutM2").hide();
            $("#InformationsGenerales_Unite").children(":not(option[value='N'], option[value=''])").show();
        }
        //if ($("#CountObj").val() > 1) {
        //    $("#InformationsGenerales_CoutM2").attr("readonly", "readonly").addClass("readonly");
        //} else {
        //    $("#InformationsGenerales_CoutM2").removeAttr("readonly").removeClass("readonly");
        //}
    };

    //-------------- Supprime les garanties --------------
    this.deleteFormuleGarantie = function () {
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeRsq = $("#CodeRisque").val();

        $.ajax({
            type: "POST",
            url: "/DetailsRisque/DeleteFormuleGarantie",
            data: { codeOffre: codeOffre, version: version, type: type, codeRsq: codeRsq },
            success: function (data) {
                $("#oldCible").val($("#InformationsGenerales_Cible").val());
                ReloadNomenclatureCombos($("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), $("#CodeRisque").val(), $("#CodeObjet").val(), $("#InformationsGenerales_Cible").val());
                $("#hasFormules").val("");
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
    //----------------------Cache le bouton supprimer lorsque l'on a un seul objet---------------------
    this.hideDelButton = function () {
        if ($("#Code").val() == "0" || parseInt($("#CountObj").val()) < 2) {
            $("#btnSupprimerObj").hide();
        }
    };
    //----------------------Formate tous les controles datepicker---------------------
    this.formatDatePicker = function () {
        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    };
    //---------------------Appel de la méthode RemoveCSS------------------------
    this.initChangeDateTimeCss = function() {
        $("#InformationsGenerales_DateEntreeGarantie").live("change", function () {
            detailsObjet.resetDateTimeCss();
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
        $("#InformationsGenerales_DateSortieGarantie").live("change", function () {
            detailsObjet.resetDateTimeCss();
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
        $("#InformationsGenerales_HeureEntreeGarantie").live("change", function () {
            detailsObjet.resetDateTimeCss();
        });
        $("#InformationsGenerales_HeureSortieGarantie").live("change", function () {
            detailsObjet.resetDateTimeCss();
        });

        $("#InformationsGenerales_DateEntreeDescr").live("change", function () {
            detailsObjet.resetDateTimeCss();
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
        $("#InformationsGenerales_DateSortieDescr").live("change", function () {
            detailsObjet.resetDateTimeCss();
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
        $("#InformationsGenerales_HeureEntreeDescr").live("change", function () {
            detailsObjet.resetDateTimeCss();
        });
        $("#InformationsGenerales_HeureSortieDescr").live("change", function () {
            detailsObjet.resetDateTimeCss();
        });
    }
    //---------------------Supprime la classe RequiredField--------------------
    this.resetDateTimeCss = function() {
        $("#InformationsGenerales_DateEntreeGarantie").removeClass("requiredField");
        $("#InformationsGenerales_DateSortieGarantie").removeClass("requiredField");
        $("#InformationsGenerales_HeureEntreeGarantie").removeClass("requiredField");
        $("#InformationsGenerales_HeureSortieGarantie").removeClass("requiredField");
        $("#InformationsGenerales_DateEntreeDescr").removeClass("requiredField");
        $("#InformationsGenerales_DateSortieDescr").removeClass("requiredField");
        $("#InformationsGenerales_HeureEntreeDescr").removeClass("requiredField");
        $("#InformationsGenerales_HeureSortieDescr").removeClass("requiredField");
    }
    //----------------------Suivant---------------------
    this.initSuivant = function () {
        $("#btnSuivant").kclick(function (evt, data) {
            var typeMsgOffre = "de l'offre";
            if ($("#Offre_Type").val() == "P") {
                typeMsgOffre = "du contrat";
                if (window.currentAffair.numeroAvenant > 0) {
                    typeMsgOffre = "de l'avenant";
                }
            }
            $(".requiredField").removeClass("requiredField");
            var Periodicite = $("#Periodicite").val();
            if (!window.isReadonly && !$("#IsAvnDisabled").hasTrueVal()) {
                var erreurBool = false;
                var erreurMsg = "Erreur : <br/>";

                //$("#InformationsGenerales_Designation").html($("#InformationsGenerales_Designation").html().replace(/\n/ig, "<br>"));
                $("#InformationsGenerales_Descriptif").val($.trim($("#InformationsGenerales_Descriptif").val()));



                if ($("#InformationsGenerales_Descriptif").val() == "") {
                    $("#InformationsGenerales_Descriptif").addClass("requiredField");
                    erreurBool = true;
                }

                if ($("#InformationsGenerales_Cible").val() == "") {
                    $("#InformationsGenerales_Cible").addClass("requiredField");
                    erreurBool = true;
                }

                //SAB controle ValeurHT avec TPCN1 bug 2116 01/02/2017
                var str = "InformationsGenerales_Type" + $("#InformationsGenerales_Type").val();
                var controleTPCN1 = $("#" + str).attr('albDescriptif');
                if (controleTPCN1 != "1") {
                    if ($("#paramOpenInven").val() == "" && ($("#InformationsGenerales_ValeurHT").val() == "") && ($("#InformationsGenerales_Unite").val() == "D") && $("#TypePolice").val() != "M" && $("#Periodicite").val() != "R") {
                        $("#InformationsGenerales_ValeurHT").addClass("requiredField");
                        erreurBool = true;
                    }
                }
                if ($("#InformationsGenerales_Valeur").val() != "" && $("#InformationsGenerales_Valeur").val() != "0" && ($("#InformationsGenerales_Unite").val() == "" || ($("#InformationsGenerales_Type").val() == "" && $("#Offre_CodeOffre").val().indexOf("CV") < 0))) {
                    $("#InformationsGenerales_Unite").addClass('requiredField');
                    $("#InformationsGenerales_Type").addClass('requiredField');
                    erreurBool = true;
                }

                if (($("#InformationsGenerales_Valeur").val() == "0" || $("#InformationsGenerales_Valeur").val() == "") && ($("#InformationsGenerales_Unite").val() != "" || $("#InformationsGenerales_Unite").val() != "")
                    && $("#TypePolice").val() != "M" && $("#Periodicite").val() != "R") {
                    $("#InformationsGenerales_Valeur").addClass('requiredField');
                    erreurBool = true;
                }

                if ($("#InformationsGenerales_Type").val() == "M2" && $("#InformationsGenerales_Unite").val() != "N") {
                    $("#InformationsGenerales_Unite").addClass('requiredField');
                    $("#InformationsGenerales_Type").addClass('requiredField');
                    erreurBool = true;
                }


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


                if ($("#Offre_Type").val() == "P" && (($("#InformationsGenerales_DateSortieGarantie").val() == "") && ((Periodicite == "U") || (Periodicite == "E")))) {
                    $("#InformationsGenerales_DateSortieGarantie").addClass("requiredField");
                    erreurMsg += "<br/>L'objet doit avoir une date de fin de période de garantie.";
                    erreurBool = true;
                }
                if (($("#chkIsRisqueTemporaire").is(":checked") || $("#Periodicite").val() == "U" || $("#Periodicite").val() == "E") && $("#Offre_Type").val() == "P") {
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

                if (!errDateDeb && $("#InformationsGenerales_DateEntreeGarantie").val() != "" && $("#dateDebOffre").val() != "") {// && $("#IsAvenantModificationLocale").attr("id") == undefined) {
                    var checkDDO = checkDateHeure($("#dateDebOffre"), $("#InformationsGenerales_DateEntreeGarantie"), $("#heureDebOffre"), $("#InformationsGenerales_HeureEntreeGarantieHours"), $("#minuteDebOffre"), $("#InformationsGenerales_HeureEntreeGarantieMinutes"));
                    if (!checkDDO && $("#InformationsGenerales_DateEntreeGarantie").val() != "") {
                        erreurMsg += "<br/>La date d'entrée est incohérente avec la date de début d'effet " + typeMsgOffre + " :<br/>" + $("#dateDebOffre").val() + " " + padLeft($("#heureDebOffre").val(), "0", 2) + ":" + padLeft($("#minuteDebOffre").val(), "0", 2);
                        erreurBool = true;
                    }
                }
                if (!errDateDeb && $("#InformationsGenerales_DateEntreeGarantie").val() != "" && $("#dateFinOffre").val() != "") {
                    var checkDDO = checkDateHeure($("#InformationsGenerales_DateEntreeGarantie"), $("#dateFinOffre"), $("#InformationsGenerales_HeureEntreeGarantieHours"), $("#heureFinOffre"), $("#InformationsGenerales_HeureEntreeGarantieMinutes"), $("#minuteFinOffre"));
                    if (!checkDDO && $("#InformationsGenerales_DateEntreeGarantie").val() != "") {
                        erreurMsg += "<br/>La date d'entrée est incohérente avec la date de fin d'effet " + typeMsgOffre + " :<br/>" + $("#dateFinOffre").val() + " " + padLeft($("#heureFinOffre").val(), "0", 2) + ":" + padLeft($("#minuteFinOffre").val(), "0", 2);
                        erreurBool = true;
                    }
                }
                if (!errDateFin && $("#InformationsGenerales_DateSortieGarantie").val() != "" && $("#dateFinOffre").val() != "") {
                    var checkDFO = checkDateHeure($("#InformationsGenerales_DateSortieGarantie"), $("#dateFinOffre"), $("#InformationsGenerales_HeureSortieGarantieHours"), $("#heureFinOffre"), $("#InformationsGenerales_HeureSortieGarantieMinutes"), $("#minuteFinOffre"));
                    if (!checkDFO && $("#InformationsGenerales_DateSortieGarantie").val() != "") {
                        erreurMsg += "<br/>La date de sortie est incohérente avec la date de fin d'effet " + typeMsgOffre + " :<br/>" + $("#dateFinOffre").val() + " " + padLeft($("#heureFinOffre").val(), "0", 2) + ":" + padLeft($("#minuteFinOffre").val(), "0", 2);
                        erreurBool = true;
                    }
                }

                if (!errDateFin && $("#InformationsGenerales_DateSortieGarantie").val() != "" && $("#dateDebOffre").val() != "") {// && $("#IsAvenantModificationLocale").attr("id") == undefined) {
                    var checkDDO = checkDateHeure($("#dateDebOffre"), $("#InformationsGenerales_DateSortieGarantie"), $("#heureDebOffre"), $("#InformationsGenerales_HeureSortieGarantieHours"), $("#minuteDebOffre"), $("#InformationsGenerales_HeureSortieGarantieMinutes"));
                    if (!checkDDO && $("#InformationsGenerales_DateSortieGarantie").val() != "") {
                        erreurMsg += "<br/>La date de sortie est incohérente avec la date de début d'effet " + typeMsgOffre + " :<br/>" + $("#dateDebOffre").val() + " " + padLeft($("#heureDebOffre").val(), "0", 2) + ":" + padLeft($("#minuteDebOffre").val(), "0", 2);
                        erreurBool = true;
                    }
                }
                if (!errDateDeb && !errDateFin && $("#InformationsGenerales_DateEntreeGarantie").val() != "" && $("#InformationsGenerales_DateSortieGarantie").val() != "") {
                    if ($("#InformationsGenerales_HeureSortieGarantieHours").val() != "00" || $("#InformationsGenerales_HeureSortieGarantieMinutes").val() != "00") {
                        if (getDate($("#InformationsGenerales_DateEntreeGarantie").val(), $("#InformationsGenerales_HeureEntreeGarantieHours").val() + ":" + $("#InformationsGenerales_HeureEntreeGarantieMinutes").val())
                            >= getDate($("#InformationsGenerales_DateSortieGarantie").val(), $("#InformationsGenerales_HeureSortieGarantieHours").val() + ":" + $("#InformationsGenerales_HeureSortieGarantieMinutes").val())) {
                            $("#InformationsGenerales_DateEntreeGarantie").addClass("requiredField");
                            $("#InformationsGenerales_HeureEntreeGarantieHours").addClass("requiredField");
                            $("#InformationsGenerales_HeureEntreeGarantieMinutes").addClass("requiredField");
                            $("#InformationsGenerales_DateSortieGarantie").addClass("requiredField");
                            $("#InformationsGenerales_HeureSortieGarantieHours").addClass("requiredField");
                            $("#InformationsGenerales_HeureSortieGarantieMinutes").addClass("requiredField");
                            erreurMsg += "<br/>Les dates d'entrée et de sortie ne sont pas cohérentes.";
                            erreurBool = true;
                        }
                    }
                    else {
                        var checkDH = checkDateHeure($("#InformationsGenerales_DateEntreeGarantie"), $("#InformationsGenerales_DateSortieGarantie"), $("#InformationsGenerales_HeureEntreeGarantieHours"), $("#InformationsGenerales_HeureSortieGarantieHours"), $("#InformationsGenerales_HeureEntreeGarantieMinutes"), $("#InformationsGenerales_HeureSortieGarantieMinutes"));
                        if (!checkDH) {
                            erreurMsg += "<br/>Les dates d'entrée et de sortie ne sont pas cohérentes.";
                            erreurBool = true;
                        }
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
                    var checkDD = checkDateHeure($("#InformationsGenerales_DateEntreeDescr"), $("#InformationsGenerales_DateSortieDescr"), $("#InformationsGenerales_HeureEntreeDescrHours"), $("#InformationsGenerales_HeureSortieDescrHours"), $("#InformationsGenerales_HeureEntreeDescrMinutes"), $("#InformationsGenerales_HeureSortieDescrMinutes"));
                    if (!checkDD) {
                        $("#InformationsGenerales_DateEntreeDescr").addClass('requiredField');
                        $("#InformationsGenerales_DateSortieDescr").addClass('requiredField');
                        erreurMsg += "<br/>Les dates descriptives ne sont pas cohérentes.";
                        erreurBool = true;
                    }
                }

                //verif mode avenant
                if ($("#IsAvenantModificationLocale").attr("id") != undefined) {
                    var errDateDebAvn = false;
                    //Si ajout d'un objet dans un risque existant
                    if ($("#CodeRisque").val() != "0" && $("#CodeObjet").val() == "0") {
                        if ($("#InformationsGenerales_DateEntreeGarantie").val() == "") {
                            $("#InformationsGenerales_DateEntreeGarantie").addClass('requiredField');
                            erreurBool = true;
                            errDateDebAvn = true;
                            erreurMsg += "<br/>La date d'entrée est incohérente avec la date de modification du risque :<br/>" + $("#DateEffetAvenantModificationLocale").val() + " " + padLeft(0, "0", 2) + ":" + padLeft(0, "0", 2);
                        }
                        if (!errDateDebAvn && !errDateDeb) {
                            var checkDateEntree = checkDate($("#DateEffetAvenantModificationLocale"), $("#InformationsGenerales_DateEntreeGarantie"));
                            if (!checkDateEntree) {
                                $("#InformationsGenerales_DateEntreeGarantie").addClass('requiredField');
                                erreurBool = true;
                                erreurMsg += "<br/>La date d'entrée doit être supérieure à la date de modification du risque :<br/>" + $("#DateEffetAvenantModificationLocale").val() + " " + padLeft(0, "0", 2) + ":" + padLeft(0, "0", 2);
                            }
                        }
                    }
                    errDateDebAvn = false;
                    //Si création d'un risque (et d'un objet)
                    if ($("#CodeRisque").val() == "0" && $("#CodeObjet").val() == "0") {
                        if ($("#InformationsGenerales_DateEntreeGarantie").val() == "") {
                            $("#InformationsGenerales_DateEntreeGarantie").addClass('requiredField');
                            errDateDebAvn = true;
                            erreurBool = true;
                            erreurMsg += "<br/>La date d'entrée est incohérente avec la date de début d'effet d'avenant :<br/>" + $("#DateEffetAvenant").val() + " " + padLeft($("#HeureEffetAvenant").val(), "0", 2) + ":" + padLeft($("#MinuteEffetAvenant").val(), "0", 2);
                        }
                        if (!errDateDebAvn && !errDateDeb) {
                            var checkDateEntree2 = checkDateHeure($("#DateEffetAvenant"), $("#InformationsGenerales_DateEntreeGarantie"), $("#HeureEffetAvenant"), $("#InformationsGenerales_HeureEntreeGarantieHours"), $("#MinuteEffetAvenant"), $("#InformationsGenerales_HeureEntreeGarantieMinutes"));
                            if (!checkDateEntree2) {
                                $("#InformationsGenerales_DateEntreeGarantie").addClass('requiredField');
                                erreurBool = true;
                                erreurMsg += "<br/>La date d'entrée doit être supérieure à la date de début d'effet d'avenant :<br/>" + $("#DateEffetAvenant").val() + " " + padLeft($("#HeureEffetAvenant").val(), "0", 2) + ":" + padLeft($("#MinuteEffetAvenant").val(), "0", 2);
                            }
                        }
                    }

                    errDateDebAvn = false;
                    //Si modification objet créé dans l'avenant
                    if ($("#AvnCreationObj").val() == $("#NumAvenantPage").val()) {
                        if ($("#InformationsGenerales_DateEntreeGarantie").val() == "") {
                            $("#InformationsGenerales_DateEntreeGarantie").addClass('requiredField');
                            errDateDebAvn = true;
                            erreurBool = true;
                            erreurMsg += "<br/>La date d'entrée est incohérente avec la date de modification du risque :<br/>" + $("#DateEffetAvenantModificationLocale").val() + " " + padLeft(0, "0", 2) + ":" + padLeft(0, "0", 2);
                        }
                        if (!errDateDebAvn && !errDateDeb) {
                            var checkDateEntree = checkDate($("#DateEffetAvenantModificationLocale"), $("#InformationsGenerales_DateEntreeGarantie"));
                            if (!checkDateEntree) {
                                $("#InformationsGenerales_DateEntreeGarantie").addClass('requiredField');
                                erreurBool = true;
                                erreurMsg += "<br/>La date d'entrée doit être supérieure à la date de modification du risque :<br/>" + $("#DateEffetAvenantModificationLocale").val() + " " + padLeft(0, "0", 2) + ":" + padLeft(0, "0", 2);
                            }
                        }
                    }

                    var txtDateSortieGarantie = $("#InformationsGenerales_DateSortieGarantie").val()

                    if ($.trim(txtDateSortieGarantie) !== "") {
                        var dateEffetAvnModif = getDate(incrementDate($("#DateEffetAvenantModificationLocale").val(), -1, 0, 0, 0), "23:59:59");
                        var dateSortieGarantie = getDate(txtDateSortieGarantie, $("#InformationsGenerales_HeureSortieGarantie").val());

                        if (dateSortieGarantie < dateEffetAvnModif) {
                            $("#InformationsGenerales_DateSortieGarantie").addClass("requiredField");
                            erreurBool = true;
                            erreurMsg += "<br/>Date de sortie incohérente avec la date de modification du risque";
                        }
                    }

                    //2017-04-03 : Ajout du test sur la date de début de l'historique : Dates V5
                    if ($("#DateDebHisto").val() != "" && getDate($("#InformationsGenerales_DateEntreeGarantie").val(), $("#InformationsGenerales_HeureEntreeGarantie").val()) < getDate($("#DateDebHisto").val(), $("#HeureDebHisto").val())) {
                        $("#InformationsGenerales_DateEntreeGarantie").addClass("requiredField");
                        erreurBool = true;
                        erreurMsg += "<br/>La date de début (" + $("#InformationsGenerales_DateEntreeGarantie").val() + " " + $("#InformationsGenerales_HeureEntreeGarantie").val() + ") doit être<br/> supérieure à la date d'historique (" + $("#DateDebHisto").val() + " " + $("#HeureDebHisto").val() + ")";
                    }

                      //---------------Contoler la date de modification de l'objet de risque -------------------------------------------------------------------------------
                    if ($("#DateEffetAvenantModificationLocale").val() == "") {
                        $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                        erreurMsg += "<br/>La date de modification de l'objet est obligatoire :<br/>";
                        erreurBool = true;
                    }
                    else {
                        if (getDate($("#DateEffetAvenantModificationLocale").val()) < getDate($("#DateEffetAvenant").val())) {
                            $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                            $("#DateEffetAvenant").addClass("requiredField");
                            erreurMsg += "<br/>La date de modification de l'objet doit être supérieure à la date d'effet d'avenant";
                            erreurBool = true;
                        }
                    
                        //if ($("#InformationsGenerales_DateSortieGarantie").val() != "") {
                        //    if (getDate($("#DateEffetAvenantModificationLocale").val()) > getDate($("#InformationsGenerales_DateSortieGarantie").val())) {
                        //        $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                        //        $("#InformationsGenerales_DateSortieGarantie").addClass("requiredField");
                        //        erreurMsg += "<br/>La date de modification de l'objet ne peut être supérieure à la date de fin de l'objet";
                        //        erreurBool = true;
                        //    }
                        //}
                        if ($("#dateFinRsq").val() != "") {
                            if (getDate($("#DateEffetAvenantModificationLocale").val()) > getDate($("#dateFinRsq").val())) {
                                $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                                erreurMsg += "<br/>La date de modification de l'objet doit être inferieure à la date de fin du risque :<br/>" + $("#dateFinRsq").val();
                                erreurBool = true;
                            }
                        }
                        if ($("#dateFinOffre").val()!="") {
                            if (getDate($("#DateEffetAvenantModificationLocale").val()) > getDate($("#dateFinOffre").val())) {
                                $("#DateEffetAvenantModificationLocale").addClass("requiredField");
                                erreurMsg += "<br/>La date de modification de l'objet doit être inférieure à la date de fin d'effet :<br/>" + $("#dateFinOffre").val();
                                erreurBool = true;
                            }
                        }
                     }
                    }
              

                //*************************************************************
                if (erreurBool) {
                    if (erreurMsg != "Erreur : <br/>")
                        ShowCommonFancy("Error", "", erreurMsg, 400, 250, true, true, true);
                    return false;
                }
            }
            ShowLoading();
            if ($("#IsAvenantModificationLocale").attr("id") != undefined) {
                $("#IsAvenantModificationLocale").removeAttr("disabled");
                $("#DateEffetAvenantModificationLocale").removeAttr("disabled");
            }

            $("#tabGuid").removeAttr("disabled");
            $("#paramOpenInven").removeAttr("disabled");
            $("#Offre_CodeOffre").removeAttr("disabled");
            $("#Offre_Version").removeAttr("disabled");
            $("#Offre_Type").removeAttr("disabled");
            $("#txtSaveCancel").removeAttr("disabled");
            $("#txtParamRedirect").removeAttr("disabled");
            $("#CodeRisque").removeAttr("disabled");
            $("#CodeObjet").removeAttr("disabled");
            $("#CodeInventaire").removeAttr("disabled");
            $("#TypeInventaire").removeAttr("disabled");
            $("#InformationsGenerales_Cible").removeAttr('disabled');
            $("#DescrRisque").removeAttr("disabled");
            $("#InformationsGenerales_Descriptif").removeAttr("disabled");
            $("#ModeNavig").removeAttr("disabled");
            $("#InformationsGenerales_Nomenclature1").removeAttr("disabled");
            $("#InformationsGenerales_Nomenclature2").removeAttr("disabled");
            $("#InformationsGenerales_Nomenclature3").removeAttr("disabled");
            $("#InformationsGenerales_Nomenclature4").removeAttr("disabled");
            $("#InformationsGenerales_Nomenclature5").removeAttr("disabled");
            $("#AddParamType").removeAttr("disabled");
            $("#AddParamValue").removeAttr("disabled");
            $("#InformationsGenerales_DateEntreeGarantie").removeAttr("disabled");
            $("#InformationsGenerales_HeureEntreeGarantie").removeAttr("disabled");
            $("#InformationsGenerales_DateSortieGarantie").removeAttr("disabled");
            $("#InformationsGenerales_HeureSortieGarantie").removeAttr("disabled");


            $("#InformationsGenerales_DateEntreeGarantie").removeAttr("disabled");
            $("#InformationsGenerales_HeureEntreeGarantie").removeAttr("disabled");
            $("#InformationsGenerales_DateSortieGarantie").removeAttr("disabled");
            $("#InformationsGenerales_HeureSortieGarantie").removeAttr("disabled");

            $("#InformationsGenerales_Unite").removeAttr("disabled");
            $("#InformationsGenerales_Type").removeAttr("disabled");
            $("#InformationsGenerales_ValeurHT").removeAttr("disabled");

            $("#IsForceReadOnly").removeAttr("disabled");

            //Suppression de l'espace du formatage autonumeric pour récupérer la valeur dans le Json
            common.autonumeric.applyAll('update', 'numeric', '', null, null, '99999999999', '-99999999999');

            var isModifHorsAvn = false;

            if ($('#CheckModificationAVN').attr("id") !== undefined)
                isModifHorsAvn = $('#IsModifHorsAvn').val() === 'True' && $('#CheckModificationAVN').is(':checked') ? true : false;
            else
                isModifHorsAvn = $('#IsModifHorsAvn').val() === 'True';


            if (isModifHorsAvn) {
                $('input:not([albhorsavn])').removeAttr("disabled");
                $('select:not([albhorsavn])').removeAttr("disabled");
            }

            let obj = $(':input').serializeObject(false, function (name, o) {
                let rg = /InformationsGenerales\.ListesNomenclatures\.Nomenclature\d+\.Nomenclature/g;
                if (rg.test(name)) {
                    let opt = $("[name='" + name + "'] option:selected");
                    o[name] = opt.exists() ? opt.text().split(' - ')[0].trim() : "";
                    return true;
                }
                return false;
            });
            obj.txtSaveCancel = (data && data.returnHome) ? 1 : 0;
            let formDataInit = JSON.stringify(obj);
            let formData = formDataInit.replace("TypeInventaire", "ListInventaires.TypeInventaire");
            if ($("#Offre_Type").val() == "P") {
                formData = formData.replace("Contrat.CodeContrat", "Offre.CodeOffre").replace("Contrat.VersionContrat", "Offre.Version").replace("Contrat.Type", "Offre.Type").replace("Offre.Branche", "Branche").replace("Contrat.AddParamType", "AddParamType").replace("Contrat.AddParamValue", "AddParamValue");
            }

            if ((parseInt($("#CountObj").val()) > 1) || (parseInt($("#CountObj").val()) == 1 && $("#CodeObjet").val() == "")) {
                $("#InformationsGenerales_Cible").disable(true);
            }
            $.ajax({
                type: "POST",
                url: "/DetailsObjetRisque/DetailsObjetEnregistrer",
                data: formData,
                contentType: "application/json",
                success: function (data) { },
                error: function (request) {
                    $("#InformationsGenerales_Designation").html($("#InformationsGenerales_Designation").html().replace(/&lt;br&gt;/ig, "\n"));
                    //Reformatage autonumeric pour remettre l'espace en cas d'erreur
                    common.autonumeric.applyAll('update', 'numeric', null, null, null, '99999999999', '-9999999999');
                    common.error.showXhr(request);
                }
            });
            if (isModifHorsAvn) {
                $('input:not([albhorsavn])').attr("disabled", "disabled");
                $('select:not([albhorsavn])').attr("disabled", "disabled");
            }
        });
    }

    //----------------------Reset de la page---------------------
    this.cancelForm = function() {
        if ($("#CodeRisque").val() == "0") {
            detailsObjet.redirect("MatriceRisque", "Index", "");
        }
        else if ($("#DescrRisque").val() == "") {
            if ($("#Offre_Type").val() == "O") {
                detailsObjet.redirect("ModifierOffre", "Index", "");
            }
            else if (window.currentAffair.numeroAvenant < 1) {
                detailsObjet.redirect("AnInformationsGenerales", "Index", "");
            }
            else {
                detailsObjet.redirect("AvenantInfoGenerales", "Index", "");
            }
        }
        else {
            detailsObjet.redirect("DetailsRisque", "Index", "");
        }
    }
    //------------------Redirection-------------------------
    this.redirect = function(cible, job, param) {
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeRisque = $("#CodeRisque").val();
        var codeObjet = $("#CodeObjet").val();
        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        var isForceReadOnly = $("#IsForceReadOnly").val();

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/DetailsObjetRisque/Redirection",
            data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, codeRisque: codeRisque, codeObjet: codeObjet, param: param, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue, isForceReadOnly: isForceReadOnly },
            success: function () { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    //----------------------Ajouter un inventaire---------------------
    this.addInventaire = function() {
        $("#btnAjouterInventaire").live('click', function () {
            if ($(this).hasClass('CursorPointer')) {
                if ($("#ListInventaires_TypeInventaire").val() != "") {
                    $("#btnSuivant").click();
                }
                else {
                    ShowCommonFancy("Error", "", "Veuillez sélectionner un type d'inventaire.", 220, 70, true, true);
                    return false;
                }
            }
        });
    }
    //---------------------Attache la fonction de suppr pour les boutons---------------------
    this.initDeleteInventaire = function() {
        try {
            $("img[name=btnSupprimerInventaire]").kclick(function () {
                let nameInv = $.trim($(this).parent().parent().children(':nth-child(2)').text());
                $("#hiddenInventId").val($(this).attr("id"));
                ShowCommonFancy("Confirm", "Inventaire",
                    "Vous êtes sur le point de supprimer l'inventaire " + nameInv + ".<br/>Voulez-vous continuer ?",
                    350, 130, true, true);
            });
        }
        catch (e) {
            $.fn.jqDialogErreurOpen(e);
        }
    }
    //---------------------Supprime un inventaire---------------------
    this.deleteInventaire = function(id) {
        let codeOffre = $("#Offre_CodeOffre").val();
        let version = $("#Offre_Version").val();
        let type = $("#Offre_Type").val();
        let codeRisque = $("#CodeRisque").val();
        let codeObjet = $("#CodeObjet").val();
        let codeInven = id.split('_')[1];
        let numDescr = id.split('_')[2];

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/DetailsObjetRisque/DetailsObjetInventairesSupprimer",
            data: { codeOffre: codeOffre, version: version, codeRisque: codeRisque, codeObjet: codeObjet, codeInven: codeInven, numDescr: numDescr, type: type },
            success: function (data) {
                $("#lstInventaire").html("");
                detailsObjet.initDeleteInventaire();
                $("#btnAjouterInventaire").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
                $("#ListInventaires_TypeInventaire").removeAttr('disabled').removeClass('readonly');
                CloseLoading();
                $("#divNewInven").show();
                $("#InformationsGenerales_Valeur").removeAttr("readonly").removeClass('readonly');
                $("#InformationsGenerales_Unite").removeAttr("disabled").removeClass('readonly');
                $("#InformationsGenerales_Type").removeAttr("disabled").removeClass('readonly');
                $("#InformationsGenerales_ValeurHT").removeAttr("disabled").removeClass('readonly');
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });

    }
    //----------------------Suppression de l'objet-----------------------
    this.deleteRisque = function() {
        var codeOffre = $("#Offre_CodeOffre").val();
        var versionOffre = $("#Offre_Version").val();
        var typeOffre = $("#Offre_Type").val();
        var codeRisque = $("#CodeRisque").val();
        var codeObjet = $("#CodeObjet").val();
        var codeBranche = $("#Branche").val();
        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();

        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();

        var id = codeOffre + "_" + versionOffre + "_" + typeOffre + "_" + codeRisque + "_" + codeObjet + "_" + codeBranche;

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/DetailsObjetRisque/DetailsObjetSupprimer",
            data: { id: id, tabGuid: tabGuid, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
            success: function (data) {
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    //--------------------- Trie de la grille
    this.initTri = function() {
        $("th.tablePersoHead").each(function () {
            $(this).css('cursor', 'pointer');
            var Colonne = $(this);
            Colonne.click(function () {
                var img = $(".imageTri").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1);
                img = img.substr(0, img.lastIndexOf('.'));
                var col = ':nth-child(' + Colonne.attr("id") + ')';

                $(".trFichier").sort(function (a, b) {
                    if (img == "tri_asc") {
                        return $.trim($(a).children(col).text().toLowerCase()) > $.trim($(b).children(col).text().toLowerCase()) ? -1 : $.trim($(a).children(col).text().toLowerCase()) < $.trim($(b).children(col).text().toLowerCase()) ? 1 : 0;
                    }
                    else if (img == "tri_desc") {
                        return $.trim($(a).children(col).text().toLowerCase()) > $.trim($(b).children(col).text().toLowerCase()) ? 1 : $.trim($(a).children(col).text().toLowerCase()) < $.trim($(b).children(col).text().toLowerCase()) ? -1 : 0;
                    }
                }).remove().appendTo("#TableListeInventaire");
                if (img == "tri_asc") {
                    $(".imageTri").attr("src", "../../../Content/Images/tri_desc.png");
                }
                else if (img == "tri_desc") {
                    $(".imageTri").attr("src", "../../../Content/Images/tri_asc.png");
                }
                $(".spImg").css('display', 'none');
                $(this).children(".spImg").css('display', 'block');
            });
        });
    }
    //----------------------Appel de la fonction ChangeNomenclature1---------------------
    this.initChangeNomenclature1 = function() {
        $("#InformationsGenerales_Nomenclature2").live('change', function () {
            $("#InformationsGenerales_Descriptif").val($.trim($('#InformationsGenerales_Nomenclature2 option:selected').text().split('-')[1]));
        })
    }

    //---------Sauvegarde directement la valeur et appel un pgm 400, mode avenant uniquement
    this.saveValeurModeAvenant = function() {
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var tabGuid = $("#tabGuid").val();
        var codeAvn = $("#NumAvenantPage").val();
        var valeur = $("#inEditValeurAvn").val();
        var isvalid = true;
        if (valeur == undefined || valeur == "") {
            $("#inEditValeurAvn").addClass("requiredField");
            isvalid = false;
        }
        if (isvalid) {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/DetailsObjetRisque/SaveValeurModeAvn",
                data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, tabGuid: tabGuid, valeur: valeur },
                success: function (data) {
                    $("#divModifierValeur").hide();
                    $("#InformationsGenerales_Valeur").val(valeur);
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    }
};

var detailsObjet = new DetailsObjet();
$(function () {
    detailsObjet.setFocusDescriptif();
    detailsObjet.formatDatePicker();
    detailsObjet.initChangeDateTimeCss();
    detailsObjet.initSuivant();
    detailsObjet.addInventaire();
    detailsObjet.initDeleteInventaire();
    detailsObjet.initTri();
    detailsObjet.hideDelButton();
    detailsObjet.initChangeHour();
    detailsObjet.mapPageElement();
    detailsObjet.initChangeNomenclature1();
});
