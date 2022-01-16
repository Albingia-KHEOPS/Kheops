$(document).ready(function () {
    MapElementPage();
});

//-------------------Disable History----------------

function changeHashOnLoadHeader() {
    window.location.href += "#";
    setTimeout("changeHashAgainHeader()", "50");
    if (window.history.forward(1) != null) {
        window.history.forward(1);
    }
    setTimeout("DisableBackButton()", 0);
}

function changeHashAgainHeader() {
    window.location.href += "1";
}
window.onload = function() { Clear(); };
function Clear() {
    var backlen = history.length;
    if (backlen > 0 && window.location.pathname != "/RechercheSaisie/Index" && window.location.pathname != "/" ) history.go(-backlen);
}
function DisableBackButton() {
    if (window.location.pathname != "/RechercheSaisie/Index") {
        window.history.forward();
    }
}
//---------------------------Redimentionne la hauteur d'une Iframe-----------------
function SetIframeHeight() {
    $("body").scrollTop();
    window.scrollTo(0, 0);
    JQBindBeforeUnload();
}

//----------------Map les éléments de la page------------------
function MapElementPage() {
    $(document).off("click", "#btnConfirmOk");
    $(document).on("click", "#btnConfirmOk", function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "UnlockSessionUserFolder":
                UnlockSessionUserFolder();
                break;
        }
        $("#hiddenAction").val("");
    });
    $(document).off("click", "#btnConfirmCancel");
    $(document).on("click", "#btnConfirmCancel", function () {
        CloseCommonFancy();
        $("#hiddenAction").val("");
    });

}

//-------------------------------------Déverouillage de toutes les offres de la Guid associé à la page navigateur-----
function UnlockSessionUserFolder() {
    let guid = $("#homeTabGuid").val();
    if (navigator.sendBeacon) {
        navigator.sendBeacon("/OffresVerrouillees/DeverouillerUserOffre", { tabGuid: guid });
    }
    else {
        return $.ajax({
            type: "POST",
            url: "/OffresVerrouillees/DeverouillerUserOffre",
            async: false,
            data: { tabGuid: guid },
            error: function (x, s, e) {
                console.error(e);
            }
        });
    }
}
//---------------Méthode qui se déclenche avant la fermeture du Tab ou du navigateur
function JQBindBeforeUnload() {
    $(window).on("beforeunload", function (event) {
        UnlockSessionUserFolder();
    });
}
function BeforeWindowUnload() {
    window.onbeforeunload = function() {
        UnlockSessionUserFolder();
    };
}
