
var EtablirAffaireNouvelle = function () {
    this.initPage = function () {
        offreANList.initOpenContrat();
        $("#btnAccueil").kclick(function () {
            common.page.isLoading = true;
            common.$postJson("/OffresVerrouillees/DeverouillerUserOffre", { tabGuid: common.tabGuid }, true).done(function () {
                offreANList.redirect("RechercheSaisie", "Index");
            });
        });

        offreANList.watchChangeContratMere();
        offreANList.watchChangeHour();

        $("#btnChooseOption").kclick(function () {
            if ($("#radBtnActionNext").isChecked()) {
                offreANList.redirect("RsqObjAffNouv", "Index");
            }
            else if ($("#radBtnActionCopy").isChecked()) {
                ShowCommonFancy("Confirm", "CopyAll", "Vous allez copier l'intégralité des risques et des formules de l'offre<br/>dans votre affaire nouvelle.<br/><br/>Voulez-vous continuer ?", 300, 80, true, true, true);
            }
        });

        $("#btnConfirmationCopyAll").kclick(function () {
            ShowCommonFancy("Confirm", "CopyAll", "Vous allez copier l'intégralité des risques et des formules de l'offre<br/>dans votre affaire nouvelle.<br/><br/>Voulez-vous continuer ?", 300, 80, true, true, true);
        });

        $("#btnConfirmationPoursuivre").kclick(function () {
            offreANList.redirect("RsqObjAffNouv", "Index");
        });

        $("#btnConfirmationContinuerPlusTard").kclick(function () {
            $("#divConfirmation").hide();
            offreANList.redirect("RechercheSaisie", "Index");
        });

        $("#btnNouveauContratValid").kclick(function () {
            if ($(this).attr('disabled') && $("#CreationAffNouv").val() == "0") {
                return;
            }

            ShowLoading();
            $("#btnNouveauContratValid").attr("disabled", "disabled");
            $("#CreationAffNouv").val("1");
            var dataValide = true;
            var erreursValidationNouveauContrat = "";
            var confirmationValidationNouveauContrat = "";

            if (!$("#NouveauContrat_ContratMere").attr("readonly") && $("#NouveauContrat_ContratMere").val().length == 0) {
                $("#NouveauContrat_ContratMere").addClass("requiredField");
                erreursValidationNouveauContrat += "Valeur manquante pour contrat mère<br />";
                dataValide = false;
            }
            else {
                $("#NouveauContrat_ContratMere").removeClass("requiredField");
            }

            if (!$("#NouveauContrat_Aliment").attr("readonly")) {
                if ($("#NouveauContrat_Aliment").val().length == 0) {
                    $("#NouveauContrat_Aliment").addClass("requiredField");
                    erreursValidationNouveauContrat += "Valeur manquante pour N° Aliment<br />";
                    dataValide = false;
                }
                else {
                    var aliment = parseInt($("#NouveauContrat_Aliment").val());
                    if (aliment == "NaN") {
                        $("#NouveauContrat_Aliment").addClass("requiredField");
                        erreursValidationNouveauContrat += "La valeur de N° Aliment n'est pas un chiffre<br />";
                        dataValide = false;
                    }
                    else {
                        if (aliment < 1) {
                            $("#NouveauContrat_Aliment").addClass("requiredField");
                            erreursValidationNouveauContrat += "La valeur de N° Aliment doit être supérieure à 0<br />";
                            dataValide = false;
                        }
                        else {
                            $("#NouveauContrat_Aliment").removeClass("requiredField");
                        }
                    }
                }
            }
            else {
                $("#NouveauContrat_Aliment").removeClass("requiredField");
            }

            if ($("#NouveauContrat_EstContratRemplace").isChecked()) {
                if ($("#NouveauContrat_ContratRemplace").val().length == 0) {
                    $("#NouveauContrat_ContratRemplace").addClass("requiredField");
                    erreursValidationNouveauContrat += "Valeur manquante pour Contrat remplacé<br />";
                    dataValide = false;
                }
                else {
                    $("#NouveauContrat_ContratRemplace").removeClass("requiredField");
                }
                if ($("#NouveauContrat_ContratRemplaceAliment").val().length == 0) {
                    $("#NouveauContrat_ContratRemplaceAliment").addClass("requiredField");
                    erreursValidationNouveauContrat += "Valeur manquante pour N° Aliment de contrat remplacé<br />";
                    dataValide = false;
                }
                else {
                    $("#NouveauContrat_ContratRemplaceAliment").removeClass("requiredField");
                }
            }

            if (!isDate($("#NouveauContrat_DateEffet").val())) {
                $("#NouveauContrat_DateEffet").addClass("requiredField");
                erreursValidationNouveauContrat += "Date d'effet incorrecte<br />";
                dataValide = false;
            }
            else {
                $("#NouveauContrat_DateEffet").removeClass("requiredField");
            }
            if ($("#NouveauContrat_EffetHours").val() == "") {
                $("#NouveauContrat_EffetHours").addClass("requiredField");
                erreursValidationNouveauContrat += "Valeur manquante pour Heure d'effet<br />";
                dataValide = false;
            }
            else {
                $("#NouveauContrat_EffetHours").removeClass("requiredField");
            }
            if ($("#NouveauContrat_EffetMinutes").val().length == "") {
                $("#NouveauContrat_EffetMinutes").addClass("requiredField");
                erreursValidationNouveauContrat += "Valeur manquante pour Heure d'effet<br />";
                dataValide = false;
            }
            else {
                $("#NouveauContrat_EffetMinutes").removeClass("requiredField");
            }
            
            if (!isDate($("#NouveauContrat_DateAccord").val())) {
                $("#NouveauContrat_DateAccord").addClass("requiredField");
                erreursValidationNouveauContrat += "Date d'accord incorrecte<br />";
                dataValide = false;
            }
            else {
                $("#NouveauContrat_DateAccord").removeClass("requiredField");
            }
            
            if (isDate($("#NouveauContrat_DateEffet").val()) && isDate($("#DateDuJour").val())) {
                var splitDateEffet = $("#NouveauContrat_DateEffet").val().split('/');
                var anneeDateEffet = splitDateEffet[2];
                var splitDateDuJour = $("#DateDuJour").val().split('/');
                var anneeDateDuJour = splitDateDuJour[2];
                if (parseInt(anneeDateEffet) > parseInt(anneeDateDuJour)) {
                    confirmationValidationNouveauContrat += "L'année d'effet est supérieure à " + anneeDateDuJour + "<br />";
                }
            }
            
            if (isDate($("#NouveauContrat_DateAccord").val()) && isDate($("#DateDuJour").val())) {
                if (!checkDate($("#NouveauContrat_DateAccord"), $("#DateDuJour"))) {
                    confirmationValidationNouveauContrat += "La date d'accord est supérieure à la date du jour";
                }
            }

            if ($("#NouveauContrat_Souscripteur").val() == "") {
                $("#NouveauContrat_Souscripteur").addClass("requiredField");
                erreursValidationNouveauContrat += "Valeur manquante pour Souscripteur<br />";
                dataValide = false;
            }
            else {
                $("#NouveauContrat_Souscripteur").removeClass("requiredField");
            }
            if ($("#NouveauContrat_Gestionnaire").val() == "") {
                $("#NouveauContrat_Gestionnaire").addClass("requiredField");
                erreursValidationNouveauContrat += "Valeur manquante pour Gestionnaire<br />";
                dataValide = false;
            }
            else {
                $("#NouveauContrat_Gestionnaire").removeClass("requiredField");
            }
            if (dataValide) {
                if (confirmationValidationNouveauContrat.length > 0) {
                    confirmationValidationNouveauContrat += "<br>Voulez-vous continuer ?";
                    ShowCommonFancy("Confirm", "", confirmationValidationNouveauContrat, 350, 130, true, true);
                    $("#hiddenAction").val("etablirAffNouv");
                }
                else {
                    offreANList.createAN();
                }
                CloseLoading();
            }
            else {
                ShowCommonFancy("Error", "", erreursValidationNouveauContrat, 350, 400, true, true);
                CloseLoading();
                $("#btnNouveauContratValid").removeAttr("disabled");
                $("#CreationAffNouv").val("0");
            }
        });

        $("#btnConfirmOk").kclick(function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "etablirAffNouv":
                    offreANList.createAN();
                    $("#NouveauContrat_DateAccord").removeClass(".requiredField");
                    break;
                case "CopyAll":
                    offreANList.createANCopyAllInfo();
                    break;
            }
            $("#hiddenAction").clear();
        });
        $("#btnConfirmCancel").kclick(function () {
            CloseCommonFancy();
            $("#hiddenAction").clear();
        });

        $("#NouveauContrat_DateEffet").offOn("change", function () {
            if ($(this).val() != "") {
                if ($("#NouveauContrat_EffetHours").val() == "") {
                    $("#NouveauContrat_EffetHours").val("00");
                    $("#NouveauContrat_EffetMinutes").val("00");
                    $("#NouveauContrat_EffetHours").trigger("change");
                }
            }
            else {
                $("#NouveauContrat_EffetHours").clear();
                $("#NouveauContrat_EffetMinutes").clear();
                $("#NouveauContrat_EffetHours").trigger("change");
            }
        });

    };
    
    this.watchChangeHour = function () {
        $("#NouveauContrat_EffetHours").offOn("change", function () {
            if ($(this).val() != "" && $("#NouveauContrat_EffetMinutes").val() == "") {
                $("#NouveauContrat_EffetMinutes").val("00");
            }
            offreANList.setHour($(this));
        });
        $("#NouveauContrat_EffetMinutes").offOn("change", function () {
            if ($(this).val() != "" && $("#NouveauContrat_EffetHours").val() == "") {
                $("#NouveauContrat_EffetHours").val("00");
            }
            offreANList.setHour($(this));
        });
    };
    
    this.setHour = function (elem) {
        var elemId = elem.attr('id').replace("Hours", "").replace("Minutes", "")

        var changeHour = SetHours(elemId);
        if (!changeHour && elem.val() == "") {
            $("#" + elemId + "Hours").clear();
            $("#" + elemId + "Minutes").clear();
        }
    };

    this.createANCopyAllInfo = function () {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationAffaireNouvelle/CopyAllInfo",
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), infoContrat: $("#ConfirmationCodeContrat").html(),
                splitHtmlChar: $("#SplitHtmlChar").val(), acteGestion: $("#ActeGestion").val(), tabGuid: common.tabGuid
            },
            success: function () {
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    this.initOpenContrat = function () {
        $("td[name=codeContrat]").kclick(function () {
            offreANList.openContrat($(this).attr("albContrat"), $(this).attr("albEtat"));
        });
    };

    this.openContrat = function (codeContrat, etat) {
        $("#ConfirmationCodeContrat").html(codeContrat);
        if (etat == "V") {
            // V = affaire déjà établie => accès en consultation ou en modification
            recherche.affaires.results.consultOrEdit();
        }
        else {
            common.dom.redirect(
                "/CreationAffaireNouvelle/ChoixRisques?id=",
                [
                    "ACTION|OffreToAffaire",
                    "IPB|" + currentAffair.codeOffre,
                    "ALX|" + currentAffair.version,
                    "TYP|" + currentAffair.type,
                    "IPB|" + $("#ConfirmationCodeContrat").html().split("_").first(),
                    "ALX|" + $("#ConfirmationCodeContrat").html().split("_")[1],
                    "GUIDKEY|" + common.tabGuid
                ].join("||"));
        }
    };

    //----------------------Formate tous les controles datepicker---------------------
    this.formatDatePicker = function () {
        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    };

    this.initCheckBoxContratRemplace = function () {
        $("#NouveauContrat_EstContratRemplace").kclick(function () {
            var imgLoupe = $("img[id=RechercherContratRemplaceImg]");
            if ($(this).isChecked()) {
                $("#NouveauContrat_ContratRemplace").removeAttr("readonly").removeClass("readonly");
                $("#NouveauContrat_ContratRemplaceAliment").removeAttr("readonly").removeClass("readonly");
                imgLoupe.attr("src", "/Content/Images/loupe.png");
                imgLoupe.addClass("CursorPointer");
            }
            else {
                $("#NouveauContrat_ContratRemplace").clear().makeReadonly(true).removeClass("requiredField");
                $("#NouveauContrat_ContratRemplaceAliment").clear().makeReadonly(true).removeClass("requiredField");
                imgLoupe.attr("src", "/Content/Images/loupegris.png");
                imgLoupe.removeClass("CursorPointer");
            }
        });
    };
    
    this.redirect = function (cible, job) {
        ShowLoading();
        if (cible == "RsqObjAffNouv" && job == "Index") {
            common.dom.redirect(
                "/CreationAffaireNouvelle/ChoixRisques?id=",
                [
                    "ACTION|OffreToAffaire",
                    "IPB|" + currentAffair.codeOffre,
                    "ALX|" + currentAffair.version,
                    "TYP|" + currentAffair.type,
                    "IPB|" + $("#ConfirmationCodeContrat").html().split("_").first(),
                    "ALX|" + $("#ConfirmationCodeContrat").html().split("_")[1],
                    "GUIDKEY|" + common.tabGuid
                ].join("||"));

            return false;
        }

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var infoContrat = $("#ConfirmationCodeContrat").html();
        var tabGuid = $("#tabGuid").val();
        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();
        $.ajax({
            type: "POST",
            url: "/CreationAffaireNouvelle/Redirection/",
            data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, infoContrat: infoContrat, tabGuid: tabGuid, addParamType: addParamType, addParamValue: addParamValue },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    };

    this.watchChangeTypeContrat = function () {
        $("#NouveauContrat_TypeContrat").offOn("change", function () {
            var imgLoupe = $("img[id=RechercherContratMereImg]");
            if ($(this).val() == "A") {
                $("#NouveauContrat_ContratMere").removeAttr("readonly").removeClass("readonly");
                $("#NouveauContrat_Aliment").removeAttr("readonly").removeClass("readonly");
                imgLoupe.attr("src", "/Content/Images/loupe.png");
                imgLoupe.addClass("CursorPointer");
            }
            else {
                $("#NouveauContrat_ContratMere").clear().makeReadonly(true);
                $("#NouveauContrat_Aliment").clear().makeReadonly(true);
                imgLoupe.attr("src", "/Content/Images/loupegris.png");
                imgLoupe.removeClass("CursorPointer");
            }
        });
    };

    this.createAN = function () {
        $("#ConfirmationCodeOffre").html($("#Offre_CodeOffre").val());
        var heureEffet = $("#NouveauContrat_EffetHours").val() + ":";
        heureEffet = heureEffet.concat($("#NouveauContrat_EffetMinutes").val());
        $.ajax({
            type: "POST",
            url: "/CreationAffaireNouvelle/NumeroAffaireNouvelle",
            data: {
                codeOffre: $("#CodeOffre").val(), version: $("#Version").val(), type: $("#Offre_Type").val(), codeContrat: $("#NouveauContrat_ContratMere").val(),
                versionContrat: $("#NouveauContrat_Aliment").val(), typeContrat: $("#NouveauContrat_TypeContrat").val(), dateAccord: $("#NouveauContrat_DateAccord").val(),
                dateEffet: $("#NouveauContrat_DateEffet").val(), heureEffet: heureEffet, contratRemp: $("#NouveauContrat_ContratRemplace").val(),
                versionRemp: $("#NouveauContrat_ContratRemplaceAliment").val(), souscripteur: $("#NouveauContrat_SouscripteurCode").val(), gestionnaire: $("#NouveauContrat_GestionnaireCode").val(),
                branche: $("#NouveauContrat_Branche").val(), cible: $("#Cible").val(), observation: encodeURIComponent($("#Observation").val()),
                tabGuid: common.tabGuid,
                acteGestion: $("#ActeGestion").val()
            },
            success: function (data) {
                $("#ConfirmationCodeContrat").html(data);
                $("#divConfirmation").show();
            },
            error: function (request) {
                $("#btnNouveauContratValid").removeAttr("disabled");
                common.error.showXhr(request, true);
            }
        });
    };

    this.initCancel = function () {
        $("#btnNouveauContratCancel").kclick(function () {
            $("#divNouveauContrat").hide();
        });
    };

    this.initAllowAN = function () {
        if ($("#Offre_PossedeUnContratEnCours").val() == "True") {
            $("#btnNouveauContrat").disable();
            return false;
        }
        return true;
    };

    this.initCreationAN = function () {
        if (!offreANList.initAllowAN()) {
            return;
        }
        $("#divLstRsq").draggable();  
        $("#btnNouveauContrat").live('click', function () {

            if ($("#tblBodyParam tr").length > 0) {

                $("#divLstRsq").show();

                $("#btnValidationContrat").live('click', function () {

                    $("#divLstRsq").hide();

                    if ($("#Offre_PossedeUnContratEnCours").val() == "False") {
                        $.ajax({
                            type: "POST",
                            url: "/CreationAffaireNouvelle/OpenNewContrat",
                            data: { codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(), modeNavig: $("#ModeNavig").val() },
                            success: function (data) {
                                AlbScrollTop();
                                $("#divDataNouveauContrat").html(data);
                                $("#divNouveauContrat").show();
                                offreANList.formatDatePicker();
                                AffectDateFormat();
                                offreANList.formatNumericValue();
                                MapCommonAutoCompSouscripteur();
                                MapCommonAutoCompGestionnaire();
                            },
                            error: function (request) {
                                common.error.showXhr(request, true);
                            }
                        });
                    }
                });
            }
            else {
                if ($("#Offre_PossedeUnContratEnCours").val() == "False") {
                    $.ajax({
                        type: "POST",
                        url: "/CreationAffaireNouvelle/OpenNewContrat",
                        data: { codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(), modeNavig: $("#ModeNavig").val() },
                        success: function (data) {
                            AlbScrollTop();
                            $("#divDataNouveauContrat").html(data);
                            $("#divNouveauContrat").show();
                            offreANList.formatDatePicker();
                            AffectDateFormat();
                            offreANList.formatNumericValue();
                            MapCommonAutoCompSouscripteur();
                            MapCommonAutoCompGestionnaire();
                        },
                        error: function (request) {
                            common.error.showXhr(request, true);
                        }
                    });
                }
            }
        });
        $("#btnRefusContrat").live('click', function () {
            $("#divLstRsq").hide();
        });
        
    };

    this.sortContrats = function () {
        $("th.tablePersoHead").each(function () {
            $(this).css('cursor', 'pointer');
            var Colonne = $(this);
            Colonne.click(function () {
                var img = $(".imageTri").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1);
                img = img.substr(0, img.lastIndexOf('.'));
                var col = ':nth-child(' + Colonne.attr("id") + ')';

                $(".trContrat").sort(function (a, b) {
                    if (img == "tri_asc") {
                        return $.trim($(a).children(col).text().toLowerCase()) > $.trim($(b).children(col).text().toLowerCase()) ? -1 : $.trim($(a).children(col).text().toLowerCase()) < $.trim($(b).children(col).text().toLowerCase()) ? 1 : 0;
                    }
                    else if (img == "tri_desc") {
                        return $.trim($(a).children(col).text().toLowerCase()) > $.trim($(b).children(col).text().toLowerCase()) ? 1 : $.trim($(a).children(col).text().toLowerCase()) < $.trim($(b).children(col).text().toLowerCase()) ? -1 : 0;
                    }
                }).remove().appendTo("#tblBodyParam");
                if (img == "tri_asc") {
                    $(".imageTri").attr("src", "../../../Content/Images/tri_desc.png");
                }
                else if (img == "tri_desc") {
                    $(".imageTri").attr("src", "../../../Content/Images/tri_asc.png");
                }
                $(".spImg").css('display', 'none');
                $(this).children(".spImg").css('display', 'block');

                offreANList.initOpenContrat();
            });
        });
    };
    //-------Formate les input/span des valeurs----------
    this.formatNumericValue = function () {
        common.autonumeric.applyAll('init', 'numeric', '', null, null, '99999999999', '0');
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
            offreANList.validateSelectionContrat();
        });
        $("img[id=RechercherContratMereImg]").kclick(function () {
            var img = $("img[id=RechercherContratMereImg]").attr("src").substr($("img[id=RechercherContratMereImg]").attr("src").lastIndexOf('/') + 1);
            if (img == "loupe.png")
                offreANList.displayResultContrats("M");
        });
    };
    
    //--- lance la recherche d'un contrat à remplacer
    this.initSearchContratRemplace = function () {
        $("img[id=RechercherContratRemplaceImg]").kclick(function () {
            var img = $("img[id=RechercherContratRemplaceImg]").attr("src").substr($("img[id=RechercherContratRemplaceImg]").attr("src").lastIndexOf('/') + 1);
            if (img == "loupe.png")
                offreANList.displayResultContrats("Remplace");
        });
    };
    //--affiche le resultat de la recherche---
    this.displayResultContrats = function (type) {
        $("#divRechercheContrat").show();
        $("#typeRecherche").val(type);
        DesactivateShortCut("rechercherContrat");
        ReactivateShortCut("resultatRecherche");
        $("#btnInitialize").click();
    };
    
    this.validateSelectionContrat = function () {
        //----contrat sélectionné : 
        var contratSelectionne = $("#selectedOffreContrat").val();
        if ($("#typeRecherche").val() == "M") {
            $("#NouveauContrat_ContratMere").val(contratSelectionne.split('_')[0]);
            offreANList.getVersionAlx();
        }
        else $("#NouveauContrat_ContratRemplace").val(contratSelectionne.split('_')[0]);

        $("#divRecherche").hide();
        $("#divRechercheContrat").hide();
        ReactivateShortCut("rechercheContrat");
    };

    //----------Map le changement de sélection du contrat mère----------
    this.watchChangeContratMere = function () {
        $("#NouveauContrat_ContratMere").offOn("change", function () {
            offreANList.getVersionAlx();
        });
    };

    //----------Récupère le numéro d'aliment-------------
    this.getVersionAlx = function() {
        var contratMere = $("#NouveauContrat_ContratMere").val();
        if (contratMere == undefined || contratMere == "") {
            $("#NouveauContrat_Aliment").clear();
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
                        $("#NouveauContrat_Aliment").val(data);
                    else
                        $("#NouveauContrat_Aliment").clear();
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    };
};

var offreANList = new EtablirAffaireNouvelle();

$(function () {
    offreANList.formatDatePicker();
    offreANList.formatNumericValue();
    offreANList.initCreationAN();
    offreANList.initCancel();
    offreANList.watchChangeTypeContrat();
    offreANList.initCheckBoxContratRemplace();
    offreANList.initPage();
    offreANList.initSearchContratMere();
    offreANList.initSearchContratRemplace();
    window.requestAnimationFrame(function () {
        offreANList.sortContrats();
        AlternanceLigne("BodyParam", "", false, null);
    });
});
