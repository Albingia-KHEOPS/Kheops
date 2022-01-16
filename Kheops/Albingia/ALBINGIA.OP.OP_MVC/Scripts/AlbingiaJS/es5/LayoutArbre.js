
var LayoutArbre = /** @class */ (function () {
    function LayoutArbre() {
    }
    LayoutArbre.prototype.reduceAll = function (show) {
        var _this = this;
        $(".MenuArbreUL:not(.MenuArbreRacine)").each(function (i, e) { return _this.reduce($(e), show); });
    };
    LayoutArbre.prototype.expandAll = function (show) {
        var _this = this;
        $(".MenuArbreUL:not(.MenuArbreRacine)").each(function (i, e) {
            var $e = $(e);
            if ($e.attr("albshow") != "admin") {
                _this.expand($e, show);
            }
            else {
                _this.reduce($e, false);
            }
        });
    };
    LayoutArbre.prototype.expand = function (branch, show) {
        if ($(branch).parent().children("ul").length > 0) {
            $(branch).parent().children(".MenuArbreImage").removeClass("Reduced");
            $(branch).parent().children(".MenuArbreImage").addClass("Expanded");
            if (show) {
                $(branch).slideDown("fast");
            }
            else {
                $(branch).show();
            }
        }
    };
    LayoutArbre.prototype.reduce = function (branch, show) {
        if ($(branch).parent().children("ul").length > 0) {
            $(branch).parent().children(".MenuArbreImage").removeClass("Expanded");
            $(branch).parent().children(".MenuArbreImage").addClass("Reduced");
            if (show) {
                $(branch).slideUp("fast");
            }
            else {
                $(branch).hide();
            }
        }
    };
    LayoutArbre.prototype.reduceChildren = function (branch, show) {
        var _this = this;
        $(branch).children("ul").each(function (ï, e) { return _this.reduce($(e), show); });
    };
    LayoutArbre.prototype.expandChildren = function (branch, show) {
        var _this = this;
        $(branch).children("ul").each(function (ï, e) { return _this.expand($(e), show); });
    };
    LayoutArbre.prototype.mapArbreItems = function () {
        $("#displayAllRsq").click(function () {
            if ($(this).attr("checked") == "checked") {
                $("li[albbaddate^=True]").each(function () {
                    $("span[albbaddate^=True]").attr("style", "color: orange");
                    $(this).show();
                });
                $(this).attr("checked", "checked");
                $(this).attr("value", "true");
            }
            else {
                $("li[albbaddate^=True]").each(function () {
                    $(this).hide();
                });
                $(this).removeAttr("checked");
                $(this).attr("value", "false");
            }
        });
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
    };
    return LayoutArbre;
}());
$(document).ready(function () {
    var layoutArbre = new LayoutArbre();
    //layoutArbre.reduceAll(false);
    layoutArbre.expandAll(true);
    layoutArbre.mapArbreItems();
    window.layoutArbre = layoutArbre;
});
