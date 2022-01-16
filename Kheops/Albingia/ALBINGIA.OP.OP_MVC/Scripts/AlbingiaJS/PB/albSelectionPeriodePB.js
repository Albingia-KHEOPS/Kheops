var SelectionPeriodePb = function () {
    //--------Map les éléments de la page---------
    this.initPage = function () {
        pbs.periode.init();

        $("#btnRegulePrec").kclick(function () {
            let p = common.albParam.buildObject();
            delete p.LOTID;
            delete p.REGULEID;
            delete p.REGULMOD;
            delete p.REGULTYP;
            delete p.REGULNIV;
            delete p.REGULAVN;
            p.AVNMODE = "CREATE";
            $("#AddParamValue").val(common.albParam.objectToString(p));
            RedirectionRegul("Index", "CreationPB", false, 'Previous');
        });

        $("#btnReguleSuivant").kclick(function (evt, data) {
            var codeAvn = $("#NumInterne").val();
            var typeAvt = $("#TypeAvt").val();
            var modeAvt = $("#ModeAvt").val();
            var exercice = $("#ExerciceRegule").val();
            var dateDeb = $("#PeriodeDeb").val();
            var dateFin = $("#PeriodeFin").val();
            var lotId = $("#lotId").val();
            if (!checkDate($("#PeriodeDeb"), $("#PeriodeFin"))) {
                common.dialog.error("Incohérence au niveau des dates");
                return false;
            }

            //$("#ddlExercices").attr("disabled", "disabled").addClass("readonly");
            $("#ExerciceRegule").attr("disabled", "disabled").addClass("readonly");
            $("#PeriodeDeb").attr("disabled", "disabled").addClass("readonly");
            $("#PeriodeFin").attr("disabled", "disabled").addClass("readonly");

            Next(codeAvn, typeAvt, lotId, exercice, dateDeb, dateFin, modeAvt, data && data.returnHome);
        });

    };

};

var selectionPeriodePb = new SelectionPeriodePb();

$(function () {
    selectionPeriodePb.initPage();

    toggleDescription();
});

function Next(codeAvn, typeAvt, lotId, exercice, dateDeb, dateFin, mode, returnHome) {
    ShowLoading();
    let reguleId = $("#ReguleId").val();
    let codeICT = "";
    let codeICC = "";
    let tauxCom = "";
    let tauxComCATNAT = "";
    let codeEnc = "";
    let modeleAvtRegule = "";
    let argModeleAvtRegule = "";

    codeICT = $.trim($("#inCourtier").val().split("-")[0]);
    codeICC = $.trim($("#inCourtierCom").val().split("-")[0]);
    tauxCom = $("#inHorsCATNAT").val();
    tauxComCATNAT = $("#inCATNAT").val();
    codeEnc = $("#inQuittancement").attr("albcode");

    modeleAvtRegule = {
        TypeAvt: $.trim(typeAvt),
        NumInterneAvt: codeAvn,
        NumAvt: codeAvn,
        MotifAvt: $("#MotifAvt").val(),
        DescriptionAvt: $("#Description").val(),
        ObservationsAvt: $.trim($("#Observation").html().replace(/<br>/ig, "\n"))
    };

    argModeleAvtRegule = JSON.stringify(modeleAvtRegule);

    if (mode === 'CREATE') {
        var addParamValue = $("#AddParamValue").val();
        var oldAvnId = $("#NumAvenantPage").val();
        var avnId = $("#NumInterne").val();
        addParamValue = addParamValue.replace("|TYPEAVT|S|", "|TYPEAVT|REGUL|");
        addParamValue = addParamValue.replace("||LOTID|0", "||LOTID|" + $("#lotId").val());
        if (addParamValue.indexOf("AVNID|" + oldAvnId + "||") !== -1) {
            addParamValue = addParamValue.replace("AVNID|" + oldAvnId + "||", "AVNID|" + avnId + "||");
            addParamValue = addParamValue.replace("AVNIDEXTERNE|" + oldAvnId + "||", "AVNIDEXTERNE|" + avnId + "||");

            $("#AddParamValue").val(addParamValue);
        }
    }

    var keys = [$("#Offre_CodeOffre").val(), $("#Offre_Version").val(), $("#Offre_Type").val(), ($("#NumInterne").val() + $("#tabGuid").val() + "addParamAVN|||" + $("#AddParamValue").val())];
    if (window.isReadonly) {
        mode = "";
        keys[keys.length - 1] += "addParam";
    }
    else {
        keys[keys.length - 1] += "||IGNOREREADONLY|1addParam";
    }

    var id = keys.join("_");

    regul.tryCreateContext(regul.nextStep);
    return;
}