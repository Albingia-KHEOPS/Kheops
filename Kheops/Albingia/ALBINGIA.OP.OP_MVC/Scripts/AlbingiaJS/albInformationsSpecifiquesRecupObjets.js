
$(document).ready(function () {
    DisplayParamRisque();
    CheckFields();
    Suivant();
    MapPageElement();    
});

//---------------------Affecte les fonctions au boutons-------------
function MapPageElement() {
    $("#btnErrorOk").live('click', function () {
        CloseCommonFancy();
    });
    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });
    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                CancelForm();
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });
    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
}
//----------------Redirection------------------
function Redirection(cible, job, withSave) {
    try {
        ShowLoading();

        if (withSave == undefined)
            withSave = false;

        var branche = encodeURIComponent($("#Branche").val());
        var section = encodeURIComponent($("#section").val());
        //var cible = '';
        var additionalParams = '';
        var dataToSave = encodeURIComponent(GetValues());
        var splitChars = encodeURIComponent($("#jsSplitChar").val());
        var strParameters = encodeURIComponent($("#parameters").val());
        //if (!Validate())
        //    return;

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var codeRisque = $("#CodeRisque").val();
        var codeObjet = $("#CodeObjet").val();
        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();

        var addParamType = $("#AddParamType").val();
        var addParamValue = $("#AddParamValue").val();

        $.ajax({
            type: "POST",
            url: "/InformationsSpecifiquesRecupObjets/Redirection/",
            data: {
                branche: branche, section: section, additionalParams: additionalParams, dataToSave: dataToSave
                    , splitChars: splitChars, strParameters: strParameters
                 , cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeRisque: codeRisque, codeObjet: codeObjet
                , tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), withSave: withSave, modeNavig: modeNavig,
                addParamType: addParamType, addParamValue: addParamValue
            },
            success: function (data) { },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    } catch (e) {
        common.dialog.smallError("Erreur de sauvegarde des informations spécifiques", true);
        CloseLoading();
    }


}
//----------------------Affiche les controles des paramètres de risque---------------------
function DisplayParamRisque() {
    $("#RisqueIndexe").live('change', function () {
        if ($(this).is(':checked')) {
            $("#LCI").attr('checked', 'checked');
            $("#Assiette").attr('checked', 'checked');
            $("#paramRisqueIndexe").show();
        }
        else {
            $("#LCI").removeAttr('checked');
            $("#Franchise").removeAttr('checked');
            $("#Assiette").removeAttr('checked');
            $("#paramRisqueIndexe").hide();
        }

    });
}

//----------------------Affiche les infos détaillées---------------------
function CheckFields() {
    if ($("#IndiceRef").val() == "") {
        $("#RisqueIndexe").removeAttr('checked');
        $("#RisqueIndexe").attr('disabled', 'disabled');
    }

    if ($("#RisqueIndexe").is(':checked')) $("#paramRisqueIndexe").show();
    else $("#paramRisqueIndexe").hide();
}
//----------------------Reset de la page---------------------
function CancelForm() {
    Redirection("DetailsObjetRisque", "Index", false);
    //    var codeOffre = $("#Offre_CodeOffre").val();
    //    var versionOffre = $("#Offre_Version").val();
    //    var codeRisque = $("#Code").val();
    //    document.location.href = "/DetailsRisque/Index/" + codeOffre + "_" + versionOffre + "_" + codeRisque;
}
//----------------------Suivant---------------------
function Suivant() {
    $("#btnSuivant").click(function () {
        if (!window.isReadonly) {
            if (!Validate()) {
                ShowCommonFancy("Error", "", "Champs obligatoires non remplis", 400, 200, true, true);
                return;
            }
        }
        ShowLoading();
        if ($("#txtSaveCancel").val() == "1")
            Redirection("RechercheSaisie", "Index", true);
        else
            Redirection("DetailsRisque", "Index", true);
        //Redirection("DetailsRisque", "Index");
        //OA_ReadCellsFromExcel();
        //window.document.location.href = "http://webexcel/ExcelValidate.htm";
    });
}