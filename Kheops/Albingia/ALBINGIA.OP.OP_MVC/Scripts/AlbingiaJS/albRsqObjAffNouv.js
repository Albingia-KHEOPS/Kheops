$(document).ready(function () {
    MapPageElement();
});

function MapPageElement() {
    //    $("input[type=checkbox][name^=checkRsq_]:not(:checked)").each(function () {
    //        var idRsq = $(this).attr("id").split("_")[1];
    //        $("tr[name=rsq_" + idRsq + "]").hide();
    //    });

    CheckRsq();

    $("#btnAnnuler").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/><u><b>Le contrat ne contiendra aucune données.</b></u><br/><br/>Confirmez-vous l’annulation ?<br/>",
                350, 130, true, true);
    });

    $("#btnSuivant").live('click', function () {
        if (!$(this).attr('disabled')) {
            Redirection("FormVolAffNouv", "Index");
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

    $("input[type=checkbox][name^=checkRsq_]").each(function () {
        $(this).change(function () {
            UpdateRsqObj($(this), "R");
        });
    });

    $("input[type=checkbox][name^=checkObj_]").each(function () {
        $(this).change(function () {
            UpdateRsqObj($(this), "O");
        });
    });
}

//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var infoContrat = $("#CodeContrat").val() + "_" + $("#VersionContrat").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var tabGuid = $("#TabGuid").val();

    if (!common.isUndifinedOrEmpty(tabGuid)) {
        tabGuid = "tabGuid" + tabGuid + "tabGuid";
    }
    
    $.ajax({
        type: "POST",
        url: "/RsqObjAffNouv/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, infoContrat: infoContrat, tabGuid: tabGuid, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------- Annule la form ------------------------
function CancelForm() {
    Redirection("CreationAffaireNouvelle", "Index");
}

//-------- Coche/Décoche un risque -----------
function SelectRsq(elem) {
    var idRsq = elem.attr("id").split("_")[1];
    if (elem.is(':checked')) {
        //$("tr[name=rsq_" + idRsq + "]").show();
        $("input[type=checkbox][name^=checkObj_" + idRsq + "_]").attr("checked", "checked");
    }
    else {
        //$("tr[name=rsq_" + idRsq + "]").hide();
        $("input[type=checkbox][name^=checkObj_" + idRsq + "_]").removeAttr("checked");
    }

    CheckRsq();
}

//-------- Coche/Décoche un objet --------
function SelectObj(elem) {
    var idRsq = elem.attr("id").split("_")[1];
    if (elem.is(':checked')) {
        $("input[type=checkbox][name=checkRsq_" + idRsq + "]").attr("checked", "checked");
    }
    else {
        //rechercher si un objet est sélectionné
        var nbSelObj = $("input[type=checkbox][name^=checkObj_" + idRsq + "]:checked").length;
        if (nbSelObj == 0) $("input[type=checkbox][name=checkRsq_" + idRsq + "]").removeAttr("checked");
    }

    CheckRsq();
}

//-------- MAJ BDD Rsq/Obj --------------
function UpdateRsqObj(elem, type) {
    var codeContrat = $("#CodeContrat").val();
    var versionContrat = $("#VersionContrat").val();
    var codeRsq = elem.attr("id").split("_")[1];
    var codeObj = "0";
    if (type == "O")
        codeObj = elem.attr("id").split("_")[2];
    var checked = "N";
    if (elem.is(':checked'))
        checked = "O";

    $.ajax({
        type: "POST",
        url: "/RsqObjAffNouv/UpdateRsqObj",
        data: { codeContrat: codeContrat, versionContrat: versionContrat, type: type, codeRsq: codeRsq, codeObj: codeObj, isChecked: checked },
        success: function (data) {
            if (type == "R")
                SelectRsq(elem);
            else
                SelectObj(elem);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//---------- Vérifie qu'il y a un risque de coché --------------
function CheckRsq() {
    var countSelRsq = $("input[type=checkbox][name^=checkRsq_]:checked").length;
    var countSelObj = 0;

    if (countSelRsq > 0) {
        countSelObj = 0;
        $("input[type=checkbox][name^=checkRsq_]:checked").each(function () {
            var idRsq = $(this).attr("id").split("_")[1];
            countSelObj = $("input[type=checkbox][name^=checkObj_" + idRsq + "_]:checked").length;
            if (countSelObj <= 0) {
                $(this).removeAttr("checked");
                $(this).trigger("change");
            }
        });

        countSelRsq = $("input[type=checkbox][name^=checkRsq_]:checked").length;
    }

    if (countSelRsq <= 0)
        $("#btnSuivant").attr("disabled", "disabled");
    else
        $("#btnSuivant").removeAttr("disabled");
}