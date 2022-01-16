
var ConfirmationSaisie = function () {
    //---------------------Affecte les fonctions au boutons-------------
    this.initPage = function () {
        $("#btnErrorOk").kclick(function () {
            CloseCommonFancy();
        });

        $("#MotifAttente").offOn("change", function () {
            AffectTitleList($(this));
        });

        $("#MotifRefus").offOn("change", function () {
            AffectTitleList($(this));
        });

        AffectTitleList($("#MotifAttente"));
        AffectTitleList($("#MotifRefus"));

        $("#dvModifInfoBase").kclick(function () {
            OpenInfoBase();
        });
    };
    
    //----------------------Activer ou désactiver un Motif de Refus---------------------
    this.initMotifRefus = function () {
        $("input[name='ConfirmationSaisie']").offOn("change", function () {
            if ($("#ConfirmationSaisie").isChecked()) {
                $("#MotifAttente").disable().clear();
                $("#MotifRefus").disable().clear();
                $("#btnSuivant").enable();
            }
            else if ($("#AttenteSaisie").isChecked()) {
                $("#MotifAttente").enable();
                $("#MotifRefus").disable().clear();
                $("#btnSuivant").disable();
            }
            else {
                $("#MotifAttente").disable().clear();
                $("#MotifRefus").enable();
                $("#btnSuivant").disable();
            }
        });
    };

    //----------------------Activer ou désactiver les actions au démarrage---------------------
    this.displayMotif = function () {
        if ($("#ConfirmationSaisie").isChecked() || $("#Offre_CodeOffre").val().substring(0, 2) == "CV") {
            $("#MotifAttente").disable();
            $("#MotifRefus").disable();
        }
        else {
            $("#ConfirmationSaisie").disable();
            $("#MotifAttente").disable();
        }
    };

    //----------------------Validation et envoi avec redirection dynamique---------------------
    this.initSuivant = function () {
        $("#btnSuivant").kclick(function (evt, data) {
            confirmationSaisie.save("Suivant", data && data.returnHome);
        });

        $("#btnFin").kclick(function () {
            confirmationSaisie.save("Terminer");
        });

    };

    this.save = function (actionSuivante, returnHome) {
        let tabGuid = IsInIframe() ? $('#homeTabGuid', window.parent.document).val() : $('#tabGuid').val();
        let motif;
        let confirmation;
        let attente;
        let refus;
        let codeMotifAttente = $('#MotifAttente').val();
        let codeMotifRefus = $('#MotifRefus').val();
        if (!$("input[name='ConfirmationSaisie']").isChecked() && codeMotifAttente == "" && codeMotifRefus == "") {
            common.dialog.error("Veuillez sélectionner un motif de refus ou d'attente !");
            return false;
        }
        common.page.isLoading = true;

        if ($("#ConfirmationSaisie").isChecked()) {
            motif = "";
            confirmation = true;
            attente = false;
            refus = false;
        }
        if ($("#AttenteSaisie").isChecked()) {
            motif = codeMotifAttente;
            confirmation = false;
            attente = true;
            refus = false;
        }
        if ($("#RefusSaisie").isChecked()) {
            motif = codeMotifRefus;
            confirmation = false;
            attente = false;
            refus = true;
        }

        let addParamType = $("#AddParamType").val();
        let addParamValue = $("#AddParamValue").val();
        let paramRedirect = $("#txtParamRedirect").val();
        let formData = {
            id: [$("#CodeOffre").val(), $("#Version").val(), $("#Type").val(), tabGuid].join("_"),
            motif: motif, confirmation: confirmation, attente: attente, refus: refus, actionSuivante: actionSuivante, tabGuid: tabGuid,
            paramRedirect: paramRedirect, addParamType: addParamType, addParamValue: addParamValue, readOnly: $("#OffreReadOnly").val()
        };

        common.page.isLoading = true;
        common.$postJson("/ConfirmationSaisie/Offre", formData, true).done(function () {
            setTimeout(function () {
                common.page.isLoading = false;
            }, 500);
            if (refus) {
                $("#btnMsgContinue").disable();
            }
            DesactivateShortCut("mainCreationSaisie");
        });
    };
};

var confirmationSaisie = new ConfirmationSaisie();

$(function () {
    confirmationSaisie.initMotifRefus();
    confirmationSaisie.initSuivant();
    confirmationSaisie.displayMotif();
    confirmationSaisie.initPage();
});
