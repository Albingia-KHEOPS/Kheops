$(document).ready(function () {
    MapElementPage();
});

//-------------------------Mapper les elements de la page 

function MapElementPage() {
    $("#btnSuivant").kclick(function (evt) {
        OA_ReadCellsFromExcel();
    });
    $("#btnAnnuler").die().live('click', function () {
        $("#contentObjExcel").hide();
        $("#contentExcelError").show();
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });
    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                Annuler();
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#contentExcelError").hide();
        $("#contentObjExcel").show();
        
    });
    $("#btnErrorOk").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#contentExcelError").hide();
        $("#contentObjExcel").show();
    });
    
}
////----------------Redirection------------------
function Annuler() {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    
    $.ajax({
        type: "POST",
        url: "/OffreSimplifiee/Annuler/",
        data: { codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
   