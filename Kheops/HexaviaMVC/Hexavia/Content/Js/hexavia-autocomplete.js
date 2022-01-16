$.widget("custom.partnercomplete", $.ui.autocomplete, {
    _create: function () {
        this._super();
        this.widget().menu("option", "items", "> tr:not(.ui-autocomplete-header)");
    },
    _renderMenu: function(ul, items) {
        var self = this;
        var $t = $("<table class='table table-striped'>", {
            border: 1
        }).appendTo(ul);
        $t.append($("<thead>"));
        $t.find("thead").append($("<tr>", {
            class: "ui-autocomplete-header"
        }));
        var $row = $t.find("tr");
        $("<th>").html("Nom").appendTo($row);
        $("<th>").html("Nom secondaire").appendTo($row);
        $("<th>").html("Code Postale").appendTo($row);
        $("<th>").html("Ville").appendTo($row);
        $("<tbody>").appendTo($t);
        $.each(items, function (index, item) {
            self._renderItemData(ul, $t.find("tbody"), item);
        });
    },
    _renderItemData: function(ul, table, item) {
        return this._renderItem(table, item).data("ui-autocomplete-item", item);
    },
    _renderItem: function(table, item) {
        var $row = $("<tr>", {
            class: "ui-menu-item",
            role: "presentation"
        });
        $("<td>").html(item.name).appendTo($row);
        $("<td>").html(item.secondaryName).appendTo($row);
        $("<td>").html(item.postalCode).appendTo($row);
        $("<td>").html(item.city).appendTo($row);
        return $row.appendTo(table);
    }
});

$.widget("custom.interlocuteurcomplete", $.ui.autocomplete, {
    _create: function () {
        this._super();
        this.widget().menu("option", "items", "> tr:not(.ui-autocomplete-header)");
    },
    _renderMenu: function (ul, items) {
        var self = this;
        var $t = $("<table class='table table-striped'>", {
            border: 1
        }).appendTo(ul);
        $t.append($("<thead>"));
        $t.find("thead").append($("<tr>", {
            class: "ui-autocomplete-header"
        }));
        var $row = $t.find("tr");
        $("<th>").html("Nom").appendTo($row);
        $("<th>").html("Nom cabinet").appendTo($row);
        $("<th>").html("Code Postale").appendTo($row);
        $("<th>").html("Ville").appendTo($row);
        $("<tbody>").appendTo($t);
        $.each(items, function (index, item) {
            self._renderItemData(ul, $t.find("tbody"), item);
        });
    },
    _renderItemData: function (ul, table, item) {
        return this._renderItem(table, item).data("ui-autocomplete-item", item);
    },
    _renderItem: function (table, item) {
        var $row = $("<tr>", {
            class: "ui-menu-item",
            role: "presentation"
        });
        $("<td>").html(item.name).appendTo($row);
        $("<td>").html(item.nameCourtier).appendTo($row);
        $("<td>").html(item.postalCode).appendTo($row);
        $("<td>").html(item.city).appendTo($row);
        return $row.appendTo(table);
    }
});