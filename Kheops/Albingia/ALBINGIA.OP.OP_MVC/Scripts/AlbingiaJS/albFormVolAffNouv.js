$(document).ready(function () {
    MapPageElement();
});

function MapPageElement() {
    CheckFormule();

    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
    });

    $("#btnSuivant").kclick(function (evt, data) {
        if (!$(this).attr('disabled')) {
            ValidForm(data && data.returnHome);
        }
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

    $("input[type=checkbox][name^=checkForm_]").each(function () {
        $(this).change(function () {
            UpdateFormVol($(this), "F");
        });
    });

    $("input[type=radio][name^=checkOpt]").each(function () {
        $(this).change(function () {
            UpdateFormVol($(this), "O");
        });
    });

    $("input[type=checkbox][name^=checkVol_]").each(function () {
        $(this).change(function () {
            UpdateFormVol($(this), "V");
        });
    });
}

//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var infoContrat = $("#CodeContrat").val() + "_" + $("#VersionContrat").val()+"_P";
    $.ajax({
        type: "POST",
        url: "/FormVolAffNouv/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, infoContrat: infoContrat },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------- Annule la form ------------------------
function CancelForm() {
    Redirection("RsqObjAffNouv", "Index");
}

//------------ Valide la form --------------
function ValidForm() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeContrat = $("#CodeContrat").val();
    var versionContrat = $("#VersionContrat").val();
    var acteGestion = $("#ActeGestion").val();
    $.ajax({
        type: "POST",
        url: "/FormVolAffNouv/GetListRsqForm",
        data: { codeOffre: codeOffre, version: version, type: type, codeContrat: codeContrat, versionContrat: versionContrat, splitHtmlChar: $("#SplitHtmlChar").val(), acteGestion: acteGestion, guid: $("#tabGuid").val(), codeAvn: $("#NumAvenantPage").val() },
        success: function (data) {
            AlbScrollTop();
            $("#divDataNouveauContrat").html(data);
            $("#divNouveauContrat").show();
            MapElementRecap();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-------- Coche/Décoche une formule -----------
function SelectForm(elem) {
    var idForm = elem.attr("id").split("_")[1];
    var guidForm = elem.attr("id").split("_")[2];
    if (elem.is(':checked')) {
        $("input[type=radio][id^=checkOpt_" + idForm + "_" + guidForm + "_1]").attr("checked", "checked");
        $("input[type=checkbox][name^=checkVol_" + idForm + "_" + guidForm + "_1]").attr("checked", "checked");
        $("tr[name=trOpt_" + idForm + "_" + guidForm + "]").show();
        $("tr[name=trVol_" + idForm + "_" + guidForm + "]").show();
    }
    else {
        $("input[type=radio][id^=checkOpt_" + idForm + "_]").removeAttr("checked");
        $("input[type=checkbox][name^=checkVol_" + idForm + "_]").removeAttr("checked");
        $("tr[name=trOpt_" + idForm + "_" + guidForm + "]").hide();
        $("tr[name=trVol_" + idForm + "_" + guidForm + "]").hide();
    }

    CheckFormule();
}

//------- Coche/Décoche une option ---------
function SelectOption(elem) {
    var idForm = elem.attr("id").split("_")[1];
    var guidForm = elem.attr("id").split("_")[2];
    var idOption = elem.attr("id").split("_")[3];

    $("input[type=checkbox][name^=checkForm_" + idForm + "_]").attr("checked", "checked");
    $("input[type=checkbox][name^=checkVol_" + idForm + "_" + guidForm + "_]").removeAttr("checked");
    $("input[type=checkbox][name^=checkVol_" + idForm + "_" + guidForm + "_" + idOption + "_]").attr("checked", "checked");
}

//------- Coche/Décoche un volet ------------
function SelectVolet(elem) {
    var idForm = elem.attr("id").split("_")[1];
    var guidForm = elem.attr("id").split("_")[2];
    var idOption = elem.attr("id").split("_")[3];
    var guidOption = elem.attr("id").split("_")[4];
    var idVolet = elem.attr("id").split("_")[5];

    if (elem.is(':checked')) {
        $("input[type=checkbox][name^=checkForm_" + idForm + "_]").attr("checked", "checked");
        $("input[type=radio][id^=checkOpt_" + idForm + "_" + guidForm + "_" + idOption + "_]").attr("checked", "checked");
    }
    else {
        //rechercher si un volet est sélectionné
        var nbSelVol = $("input[type=checkbox][name^=checkVol_" + idForm + "_" + guidForm + "_" + idOption + "_" + guidOption + "]:checked").length;
        if (nbSelVol == 0) {
            $("input[type=checkbox][name^=checkForm_" + idForm + "_]").removeAttr("checked");
            $("input[type=radio][id^=checkOpt_" + idForm + "_" + guidForm + "_" + idOption + "_]").removeAttr("checked");
            $("tr[name=trOpt_" + idForm + "_" + guidForm + "]").hide();
            $("tr[name=trVol_" + idForm + "_" + guidForm + "]").hide();
        }
    }

    CheckFormule();
}

//------- MAJ BDD Form/Opt/Vol --------
function UpdateFormVol(elem, type) {

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var typeOffre = $("#Offre_Type").val();

    var codeContrat = $("#CodeContrat").val();
    var versionContrat = $("#VersionContrat").val();
    var codeForm = elem.attr("id").split("_")[1];
    var guidForm = elem.attr("id").split("_")[2];
    var codeOpt = 0;
    var guidOpt = 0;
    if (type == "O" || type == "V") {
        codeOpt = elem.attr("id").split("_")[3];
        guidOpt = elem.attr("id").split("_")[4];
    }
    var guidVol = 0;
    if (type == "V")
        guidVol = elem.attr("id").split("_")[5];
    var checked = "N";
    if (elem.is(':checked'))
        checked = "O";

    $.ajax({
        type: "POST",
        url: "/FormVolAffNouv/UpdateFormVol",
        data: {
            codeContrat: codeContrat, versionContrat: versionContrat, codeOffre: codeOffre, version: version, typeOffre: typeOffre,
            codeForm: codeForm, guidForm: guidForm, codeOpt: codeOpt, guidOpt: guidOpt, guidVol: guidVol, type: type, isChecked: checked
        },
        success: function (data) {
            if (type == "F")
                SelectForm(elem);       //MAJ de la formule (F)
            else if (type == "O")
                SelectOption(elem);     //MAJ de l'option (O)
            else
                SelectVolet(elem);      //MAJ du volet (V)
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//---------- Vérifie qu'il y a un risque de coché --------------
function CheckFormule() {
    var countSelForm = $("input[type=checkbox][name^=checkForm_]:checked").length;
    var countSelVol = 0;

    if (countSelForm > 0) {
        countSelVol = 0;
        $("input[type=checkbox][name^=checkForm_]:checked").each(function () {
            var idForm = $(this).attr("id").split("_")[1];
            countSelVol = $("input[type=checkbox][name^=checkVol_" + idForm + "_]:checked").length;
            if (countSelVol <= 0) {
                $(this).removeAttr("checked");
                $(this).trigger("change");
            }
        });

        countSelForm = $("input[type=checkbox][name^=checkForm_]:checked").length;
    }

    if (countSelForm <= 0)
        $("#btnSuivant").attr("disabled", "disabled");
    else
        $("#btnSuivant").removeAttr("disabled");
}

//--------- Map les éléments de la récapitulation -----------
function MapElementRecap() {
    $("#btnConfAnnuler").die().live('click', function () {
        $("#divDataNouveauContrat").html("");
        $("#divNouveauContrat").hide("slow");
        //Redirection("RsqObjAffNouv", "Index");
    });

    $("#btnConfValider").die().live('click', function () {
        Redirection("OptTarAffNouv", "Index");
    });

    $("#btnConfHome").die().live('click', function () {
        Redirection("RechercheSaisie", "Index");
    });

    $("#btnConfCreate").die().live('click', function () {
        Redirection("CreationAffaireNouvelle", "Index");
    });

    $("#btnConfContinue").die().live('click', function () {
        Redirection("AnCreationContrat", "Index");
    });
}