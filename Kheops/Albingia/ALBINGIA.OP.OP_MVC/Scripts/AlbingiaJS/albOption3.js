/// <reference path="../Jquery/jquery-1.4.4-vsdoc.js" />
$(document).ready(function () {
    MapElementPage();
    FormatCLEditor($("#Description"), 890, 317);
});

//----------------Map les éléments de la page------------------
function MapElementPage() {

    $('#btnEditerOption').die();
    $('#btnEditerOption').live('click', function (evt) {
        AlbScrollTop();
        $('#divFullScreen').show();
    })

    $('#btnAnnuler').die();
    $('#btnAnnuler').live('click', function (evt) {
        Redirection("GestionOptions", "Index");
    })

}

//----------------------Formate tous les controles cleditor---------------------
//function FormatCLEditor() {
//    $("#Description").cleditor({ width: 890, height: 317, controls: "bold italic underline | bullets numbering | outdent indent | alignleft center alignright" });
//}


//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var versionOffre = $("#Offre_Version").val();
    var typeOffre = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/Option3/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: versionOffre, type: typeOffre,tabGuid:tabGuid, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}