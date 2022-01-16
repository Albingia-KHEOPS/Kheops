$(function () {
    initArbreBO();
    $(".MenuArbreAdminText[data-url]").kclick(function () {
        common.page.isLoading = true;
        common.$getJson($(this).data("url"), null, true).then(function () {
            common.dialog.info("Les caches ont été réinitialisés");
            common.page.isLoading = false;
        });
    });
});

function initArbreBO() {
    var position = $("#BackOfficeMenu").offset();
    $("#LayoutArbre").css({ 'position': 'absolute', 'top': position.top + 35 + 'px', 'left': position.left + 'px' });
    $("#LayoutArbre").show();
};

