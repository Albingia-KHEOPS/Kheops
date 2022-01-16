$(document).ready(function () {
    //reduceAll(false);
    expandAll(true);
    mapArbreItems();
});


//Tout réduire
function reduceAll(show) {
    $(".MenuArbreUL:not(.MenuArbreRacine)").each(function () {
        reduce($(this), show);
    });
}

//Tout étendre
function expandAll(show) {
    $(".MenuArbreUL:not(.MenuArbreRacine)").each(function () {
        if ($(this).attr("albshow") != "admin")
            expand($(this), show);
        else
            reduce($(this), false);
    });
    //$(".MenuArbreUL:not(.MenuArbreRacine)").each(function () {
    //    expand($(this), show);
    //});
}


function reduce(branch, show) {
    if ($(branch).parent().children("ul").size() > 0) {
        $(branch).parent().children(".MenuArbreImage").removeClass("Expanded");
        $(branch).parent().children(".MenuArbreImage").addClass("Reduced");
        if (show) {
            $(branch).slideUp("fast");
        }
        else {
            $(branch).hide();
        }
    }
}


function expand(branch, show) {
    if ($(branch).parent().children("ul").size() > 0) {
        $(branch).parent().children(".MenuArbreImage").removeClass("Reduced");
        $(branch).parent().children(".MenuArbreImage").addClass("Expanded");
        if (show) {
            $(branch).slideDown("fast");
        }
        else {
            $(branch).show();
        }
    }
}

//Reduire les éléments sous la branche sélectionnée (ul)
function reduceChildren(branch, show) {
    $(branch).children("ul").each(function () {
        reduce($(this), show);
    });
}

//Etendre les éléments sous la branche sélectionnée (ul)
function expandChildren(branch, show) {
    $(branch).children("ul").each(function () {
        expand($(this), show);
    });
}

function mapArbreItems() {
    //$(".MenuArbreUL:not(.MenuArbreRacine)").each(function () {
    //    $(this).parent().children(".MenuArbreImage").click(function () {
    //        if ($(this).hasClass("Reduced")) {
    //            expandChildren($(this).parent(), true);
    //            $(this).removeClass("Reduced");
    //            $(this).addClass("Expanded");
    //        }
    //        else {
    //            reduceChildren($(this).parent(), true);
    //            $(this).addClass("Reduced");
    //            $(this).removeClass("Expanded");
    //        }
    //    });
    //});

    //$("ul").show();


    $("#displayAllRsq").live('click', function () {
        if ($(this).attr("checked") == "checked") {
            $("li[albbaddate^=True]").each(function () {
                $("span[albbaddate^=True]").attr("style", "color: orange");
                $(this).show();
            });
            $(this).attr("checked", "checked");
            $(this).attr("value", true);
        }
        else {
            $("li[albbaddate^=True]").each(function () {
                $(this).hide();
            });
            $(this).removeAttr("checked");
            $(this).attr("value", false);
        }
    });
    //if ($("#displayAllRsq").is(":visible")) {
    //    $("li[albbaddate^=True]").each(function () {
    //        $(this).hide();
    //    });
    //}

    $(".MenuArbreImage").each(function () {
        $(this).click(function () {
            if ($(this).hasClass("Reduced")) {
                $(this).parent().children("ul").show();
                $(this).removeClass("Reduced");
                $(this).addClass("Expanded");
            }
            else {
                $(this).parent().children("ul").hide();
                $(this).addClass("Reduced");
                $(this).removeClass("Expanded");
            }
        });
    });
}