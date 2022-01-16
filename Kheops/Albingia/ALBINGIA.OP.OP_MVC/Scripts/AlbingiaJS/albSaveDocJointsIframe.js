$(document).ready(function () {
    $('#fileUpd').die();
    $('#fileUpd').live('change', function (e) {
        if (Array.prototype.some.call(e.target.files, function (x) {
            return x.size > window.parent.common.appSettings.maxFileSize;
        })) {
            e.preventDefault();
            window.parent.common.error.show("Le fichier dépasse la taille limite de " + window.parent.common.appSettings.maxFileSizeString+" octets");
            return;
        }
        var tst2 = $('#fileUpd').val();
        $('#Fichier', window.parent.document).val(tst2);
    });
});