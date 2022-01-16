
$(document).ready(function () {
    
    $("#TestErreurButton").click
    (
        function () {
            var nombre1 = $('#Nombre1').val();
            var nombre2 = $('#Nombre2').val();
            $.ajax
            (
                {
                    type: "POST",
                    url: "/GestionErreur/Index",
                    context: $("#GestionErreurResultDiv"),
                    data: "nombre1=" + nombre1 + "&nombre2=" + nombre2,
                    success: function (data) {
                        $(this).html(data);
                        $(this).show();
                        $.fn.addOnClickButton();
                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                        var dialogBox = $('#JQueryHttpPostResultDiv');
                        $.fn.jqDialog(dialogBox, 'Attention!');
                        $('.ui-dialog-content').fadeIn().html(jqXHR.responseText);
                        $(dialogBox).dialog('open');
                    }
                }
            );
        });

});
$.fn.addOnClickButton = function () {
    $('input[type=button][name=ExpandRow]').each(function (index) {
        $(this).click(function () {
            if ($("#SubGridRows" + index).css('display') == 'none') {
                $("#SubGridRows" + index).show();
            } else {
                $("#SubGridRows" + index).hide();
            }
        });
    });
};