
var RetoursPieces = (function () {
    let RetoursPieces = function () { }

    //-------------Map les éléments de la page-------------
    RetoursPieces.prototype.init = function () {
        let self = this;
        $("#btnAnnulerRetours").kclick(function () {
            ShowCommonFancy("Confirm", "Cancel",
                "Voulez-vous revenir à l'écran de recherche ?<br/> Toutes vos modifications vont être annulées.",
                350, 130, true, true);
        });

        $('#btnSuivantRetours').kclick(function () {
            self.valider();
        });

        $("#btnConfirmOk").kclick(function () {
            CloseCommonFancy();
            switch ($("#hiddenAction").val()) {
                case "Cancel":
                    self.cancel();
                    break;
                case "File":
                    DeleteFile($("#hiddenInputId").val());
                    $("#hiddenInputId").clear();
            }
            $("#hiddenAction").clear();
        });

        $("#btnConfirmCancel").kclick(function () {
            CloseCommonFancy();
            $("#hiddenAction").clear();
        });

        $("#TypeAccordPreneur").offOn("change", function () {
            $("#inDateRetour").removeAttr("disabled");
        });

        AlternanceLigne("RetoursCoAssBody", "noInput", false, null);

        self.formatDatePicker();

        if (window.isReadonly) {
            $("#btnAnnulerRetours").html("<u>P</u>récédent").assignAccessKey("p");
            $("#btnSuivantRetours").removeAttr("data-accesskey").hide();
            $("#btnUpload").removeAttr("data-accesskey").hide();
        }
    };

    //--------------------fonction qui formate les datepicker--------------
    RetoursPieces.prototype.formatDatePicker = function () {
        $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    };

    //--------------------fonction exécutée lors du clic sur le bouton valider de l'écran------------------------------
    RetoursPieces.prototype.valider = function () {
        let self = this;
        let erreurBool = false;
        let dateRetourPreneur = $("#inDateRetour").val();
        let typeAccordPreneur = $("#TypeAccordPreneur").val();
        let typeAccordPreneuActuel = $("#TypeAccordPreneurActuel").val();
        let listeLignesCoAss = self.serializeLignesCoAssureur();
        let isReglementRecu = $("#chkReglementRecu").attr("checked") == "checked";

        self.resetErreurs();

        if (dateRetourPreneur != undefined && dateRetourPreneur != "" && dateRetourPreneur.split("/").length == 3) {
            var dNow = new Date()
            var iNow = dNow.getFullYear() * 10000 + (dNow.getMonth() + 1) * 100 + dNow.getDate();
            var iDateRetourPreneur = dateRetourPreneur.split("/")[2] * 10000 + dateRetourPreneur.split("/")[1] * 100 + dateRetourPreneur.split("/")[0] * 1;
            if (iDateRetourPreneur > iNow) {
                $("#inDateRetour").addClass('requiredField');
                $("#inDateRetour").attr('title', "La date ne peut être postérieure à la date du jour");
                erreurBool = true;
            }
        }

        if (dateRetourPreneur != undefined && dateRetourPreneur != "" && dateRetourPreneur.split("/").length == 3) {
            if (typeAccordPreneur == "N") {
                $("#TypeAccordPreneur").addClass('requiredField');
                $("#TypeAccordPreneur").attr('title', "Une date de retour a été saisie, vous devez choisir un accord");
                erreurBool = true;
            }
        }

        if (isReglementRecu) {
            if (typeAccordPreneur == "N" || typeAccordPreneur == "T") {
                $("#TypeAccordPreneur").addClass('requiredField');
                $("#TypeAccordPreneur").attr('title', "Un règlement a été reçu, le type d'accord sélectionné n'est pas compatible");
                erreurBool = true;
            }
        }
        else {
            if (typeAccordPreneur == "R") {
                $("#TypeAccordPreneur").addClass('requiredField');
                $("#TypeAccordPreneur").attr('title', "Aucun règlement n'a été reçu, le type d'accord sélectionné n'est pas compatible");
                erreurBool = true;
            }
        }

        if (typeAccordPreneuActuel == "R" && typeAccordPreneur != "S") {
            $("#TypeAccordPreneur").addClass('requiredField');
            $("#TypeAccordPreneur").attr('title', "Le type d'accord sélectionné n'est pas compatible, avec la situation actuelle");
            erreurBool = true;
        }

        if ((typeAccordPreneur != "N") && (dateRetourPreneur == undefined || dateRetourPreneur == "" || dateRetourPreneur.split("/").length != 3)) {
            $("#inDateRetour").addClass('requiredField');
            $("#inDateRetour").attr('title', "Une date d'accord valide doit être sélectionnée");
            erreurBool = true;
        }


        if (self.checkLignesCoAss()) {
            erreurBool = true;
        }

        if (erreurBool) {
            return false;
        }
        else {
            common.page.isLoading = true;
            var codeContrat = $("#Offre_CodeOffre").val();
            var versionContrat = $("#Offre_Version").val();
            var typeContrat = $("#Offre_Type").val();
            var tabGuid = $("#tabGuid").val();
            $.ajax({
                type: "POST",
                url: "/Retours/ValiderRetours/",
                data: {
                    codeContrat: codeContrat, version: versionContrat, type: typeContrat, codeAvt: $("#inRetourAvenant").val(), tabGuid: tabGuid,
                    dateRetourPreneur: dateRetourPreneur, typeAccordPreneur: typeAccordPreneur,
                    listeLignesCoAss: listeLignesCoAss, modeNavig: $("#ModeNavig").val()
                },
                success: function (data) {
                    self.quit("RechercheSaisie", "Index");
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
    };

    //-----------------Retourne à l'écran de recherche
    RetoursPieces.prototype.cancel = function () {
        this.quit("RechercheSaisie", "Index");
    };

    //-----------fonction qui sérialise une opposition en fonction des champs de l'écran--------
    RetoursPieces.prototype.serializeLignesCoAssureur = function () {
        var chaine = '[';
        var hasLine = false;
        $("tr[name=lineCoass]").each(function () {
            var id = $(this).attr("id").split("_")[1];
            if (id != "" && id != undefined) {
                hasLine = true;
                chaine += '{';
                chaine += 'GuidId : "' + id + '",';
                chaine += 'SDateRetourCoAss : "' + $("#inDateRetourCoAss_" + id).val() + '",';
                chaine += 'TypeAccordVal : "' + $("#TypeAccordCoAss_" + id).val() + '"';
                chaine += '},';
            }

        });
        if (hasLine)
            chaine = chaine.substring(0, chaine.length - 1);
        chaine += ']';
        chaine = chaine.replace("\\", "\\\\");
        return chaine;
    };
    //-----------Retire toutes les erreurs de l'écran et leurs messages
    RetoursPieces.prototype.resetErreurs = function () {
        $("select[name=typeAccordInput]").removeClass('requiredField').attr("title", "");
        $("input[name=dateRetour]").removeClass('requiredField').attr("title", "");
    };
    //----------Vérifie les valeurs des lignes des coassureurs------
    RetoursPieces.prototype.checkLignesCoAss = function () {
        var dNow = new Date();
        var iNow = dNow.getFullYear() * 10000 + (dNow.getMonth() + 1) * 100 + dNow.getDate();
        var toReturn = false;
        $("tr[name=lineCoass]").each(function () {
            var id = $(this).attr("id").split("_")[1];
            if (id != "" && id != undefined) {
                var dateRetourCoAss = $("#inDateRetourCoAss_" + id).val();
                var typeAccordCoAss = $("#TypeAccordCoAss_" + id).val();
                if (dateRetourCoAss != undefined && dateRetourCoAss != "" && dateRetourCoAss.split("/").length == 3) {
                    var idateRetourCoAss = dateRetourCoAss.split("/")[2] * 10000 + dateRetourCoAss.split("/")[1] * 100 + dateRetourCoAss.split("/")[0] * 1;
                    if (idateRetourCoAss > iNow) {
                        $("#inDateRetourCoAss_" + id).addClass('requiredField');
                        $("#inDateRetourCoAss_" + id).attr('title', "La date ne peut être postérieure à la date du jour");
                        toReturn = true;
                    }
                    if (typeAccordCoAss == "N") {
                        $("#TypeAccordCoAss_" + id).addClass('requiredField');
                        $("#TypeAccordCoAss_" + id).attr('title', "Une date de retour a été saisie, vous devez choisir un accord");
                        toReturn = true;
                    }
                }
                if (typeAccordCoAss != "N") {
                    if (dateRetourCoAss == undefined || dateRetourCoAss == "" || dateRetourCoAss.split("/").length != 3) {
                        $("#inDateRetourCoAss_" + id).addClass('requiredField');
                        $("#inDateRetourCoAss_" + id).attr('title', "Une date d'accord valide doit être sélectionnée");
                        toReturn = true;
                    }
                }

            }
        });

        return toReturn;
    };

    //----------------Redirection------------------
    RetoursPieces.prototype.quit = function (cible, job) {
        common.page.isLoading = true;
        let data = {
            cible: cible, job: job, codeContrat: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), tabGuid: $("#tabGuid").val(), paramRedirect: $("#txtParamRedirect").val()
        };
        common.$postJson("/Retours/Redirection/", data, true).done(function () { });
    };

    return RetoursPieces;
}());
var retoursPieces = new RetoursPieces();
$(document).ready(function () {
    retoursPieces.init();
});