$(document).ready(function () {
    MapElementPage();
});
//---------------Map les éléments de la page--------------
function MapElementPage() {
    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Copy":
                CopyParamClause();
                break;
        }
        $("#hiddenAction").val('');
    });

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });

    $("#btnCancel").die().live('click', function () {
        Redirection("BackOffice", "Index");
    });

    $("#btnCopyParam").die().live("click", function () {
        var environment = $("select[id='Destination'] :selected").text().toLowerCase();
        ShowCommonFancy("Confirm", "Copy", "Etes-vous sûr de vouloir livrer le paramétrage des clauses sur l'environnement de " + environment + " ?", 300, 80, true, true, true);
    });
}
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/CopyParamClause/Redirection/",
        data: { cible: cible, job: job },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Livraison Param Clause-----------
function CopyParamClause() {
    ShowLoading();
    var src = $("#Source").val();
    var dest = $("#Destination").val();

    if (dest == src) {
        common.dialog.error("L'environnement de destination doit être différent de la source.");
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/CopyParamClause/CopyParam",
        data: { src: src, dest: dest },
        success: function (data) {
            common.dialog.info("Livraison effectuée");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}