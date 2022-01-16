
$(document).ready(function () {
    CancelForm();

});
//------------- Cancel de la form ----------
function CancelForm() {
    $("#btnAnnuler").live('click', function () {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/CreationFormuleGarantie/Index/" + $("#Offre_CodeOffre").val() + "_" + $("#Offre_Version").val() + "_" + $("#CodeFormule").val() + "_" + $("#CodeOption").val(),
            success: function (dataReturn) {
                $("#MenuLayout").html(dataReturn);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}