
const MIN_AUTO_COMPLETE_LENGTH = 3;

//#region AutoComplete Cabinet Courtage
/***********************************************************/
/***********    AutoComplete Cabinet Courtage     **********/
/***********************************************************/
function MapCommonAutoCompCourtier() {
    //---Charge les infos des courtiers pour l'autocomplete---
    $.widget("custom.nomCabinetAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.Nom == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteTd'>Nom</th><th class='AutoCompleteTd'>Nom secondaire</th><th class='AutoCompleteCpTd'>Code Postal</th><th class='AutoCompleteTd'>Ville</th><th class='AutoCompleteValidTd'>Valide</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var imgValidCourtier = "invalide.png";
                if (item.EstValide) {
                    imgValidCourtier = "valide.png";
                }
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.Nom + "</td><td class='AutoCompleteTd'>" + item.NomSecondaires[0] + "</td><td class='AutoCompleteCpTd'>" + item.CodePostal + "</td><td class='AutoCompleteTd'>" + item.Ville + "</td><td class='AutoCompleteValidTd'><img src='/Content/Images/" + imgValidCourtier + "'/></td></tr>";
                if (item.NomSecondaires.length > 1) {
                    for (i = 1; i < item.NomSecondaires.length; i++) {
                        tableau = tableau + "<tr><td class='AutoCompleteTd'></td><td class='AutoCompleteTd'>" + item.NomSecondaires[i] + "</td><td class='AutoCompleteCpTd'></td><td class='AutoCompleteTd'></td><td class='AutoCompleteValidTd'></td></tr>";
                    }
                }
                tableau = tableau + "</table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoCommCourtierByNom();
    LoadInfoCommCourtierByCode();
    //---Charge les infos des interlocuteurs pour l'autocomplete---
    $.widget("custom.interlocuteurAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            var self = this;
            $.each(items, function (index, item) {
                if (index == 0 && item.NomInterlocuteur == "")
                    ul.append("Aucun Interlocuteur trouvé");
                else {
                    if (index == 0) {
                        ul.append("<li><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>Nom</td><td class='AutoCompleteTd'>Courtier</td><td class='AutoCompleteCpTd'>CP Courtier</td><td class='AutoCompleteTd'>Ville Courtier</td><td class='AutoCompleteIcon'>Courtier Valide</td><td class='AutoCompleteIcon'>Interlocuteur Valide</td></tr></table></li>");
                    }
                    self._renderItem(ul, item);
                }
            });
        },
        _renderItem: function (ul, item) {
            var imgValidCourtier = "invalide.png";
            if (item.EstValide) {
                imgValidCourtier = "valide.png";
            }
            var imgValidInterloc = "invalide.png";
            if (item.ValideInterlocuteur) {
                imgValidInterloc = "valide.png";
            }
            return $("<li></li>")
                .data("item.autocomplete", item)
                .append("<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.NomInterlocuteur + "</td><td class='AutoCompleteTd'>" + item.Nom + "</td><td class='AutoCompleteCpTd'>" + item.CodePostal + "</td><td class='AutoCompleteTd'>" + item.Ville + "</td><td class='AutoCompleteIcon'><img src='/Content/Images/" + imgValidCourtier + "'/></td><td class='AutoCompleteIcon'><img src='/Content/Images/" + imgValidInterloc + "'/></td></tr></table></a>")
                .appendTo(ul);
        }
    });


    LoadInfoCommInterlocuteurByNom();
}

function DieAutoLoadInfoCommCourtier() {
    $("input[albAutoComplete=autoCompCodeCourtier]").die();
    $("input[albAutoComplete=autoCompNomCourtier]").die();
}
function LivedieAutoLoadInfoCommCourtier() {
    LoadInfoCommCourtierByCode();
    LoadInfoCommCourtierByNom();
}

function debounce(func, wait, immediate) {
    var timeout;
    return function () {
        var context = this, args = arguments;
        var later = function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
};
//----Application de la recherche ajax par code du courtier---
function LoadInfoCommCourtierByCode() {

    $("input[albAutoComplete=autoCompCodeCourtier]").attr("maxlength", 5).offOn("input change", debounce(function (event, firstload) {
        $('#CourtierInvalideDiv').empty().removeClass("error").removeClass("warning");
        const $this = $(this);
        const codeCabinet = $this.val();
        const context = $this.attr('albcontext');
        if (!firstload) {
            $("input[albAutoComplete=autoCompCodeInterlocuteur]").val("");
            $("input[albAutoComplete=autoCompNomInterlocuteur]").val("");
        }
        if (codeCabinet != "") {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/AutoComplete/GetCabinetCourtageByCode",
                data: { codeString: codeCabinet },
                success: function (json) {
                    CloseLoading();
                    if (codeCabinet == json.Code) {
                        UpdateCourtier(json, context, true);
                    } else {
                        ShowNonExistingCourtier();
                    }
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            $('#CourtierInvalideDiv').append("");
            $("input[albAutoComplete=autoCompNomCourtier]" + selectCtx(context)).val("");
            $("input[albAutoComplete=autoCompCodeCourtier]" + selectCtx(context)).val("");
            $("input[albAutoComplete=autoCompDelegCourtier]" + selectCtx(context)).val("");
            $("img[albAutoComplete=autoCompImgInfoCourtier]" + selectCtx(context)).attr("albIdInfo", "");
            if (context == "dblSaisie") {
                $("#DblCodeInterlocuteur").val("");
                $("#DblNomInterlocuteur").val("");
                $("#DblReference").val("");
            }

        }
    }, 300)).live("blur", function () {
        const $this = $(this);
        const codeCabinet = $this.val();
        const context = $this.attr('albcontext');

        if (codeCabinet != $this.data('validCode')) UpdateCourtier({}, context, true);
    });
}
//----Application de la recherche ajax par nom du courtier---
function LoadInfoCommCourtierByNom() {

    $("input[albAutoComplete=autoCompNomCourtier]").die().live("change", function () {
        $('#CourtierInvalideDiv').empty().removeClass("error").removeClass("warning");
        var context = $(this).attr('albcontext');
        var val = $(this).data("selectedItem");
        if (!val || this.value != val.Nom) {
            $("input[albAutoComplete=autoCompCodeCourtier]" + selectCtx(context)).val("");
            $("input[albAutoComplete=autoCompCodeInterlocuteur]").val("");
            $("input[albAutoComplete=autoCompNomInterlocuteur]").val("");
            $(document).trigger("autoCompCourtierSelected", { item: null, target: this });
            $(this).val("");
        }

    });
    common.dom.onEvent("input", "input[albAutoComplete=autoCompNomCourtier]", function () {
        var context = $(this).attr('albcontext');
        $("input[albAutoComplete=autoCompCodeCourtier]" + selectCtx(context)).val("");
        $(document).trigger("autoCompCourtierSelected", { item: null, target: this });
    });
    $("input[albAutoComplete=autoCompNomCourtier]").nomCabinetAutoComplete({
        open: function () {
            var context = $(this).attr('albcontext');
            $("input[albAutoComplete=autoCompCodeCourtier]" + selectCtx(context)).val("");
        },
        delay: 750,
        source: "/AutoComplete/GetCabinetsCourtagesByName",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateCourtier(ui.item, context, true);
            $(document).trigger("autoCompCourtierSelected", { item: ui.item, target: this });

            return false;
        }
    });
}
//---------------------- ZBO DIE
function DieInfoCommInterlocuteurByNom() {
    $("input[albAutoComplete=autoCompNomInterlocuteur]").die();
}
 

//----Application de la recherche ajax par nom d'interlocuteur--
function LoadInfoCommInterlocuteurByNom() {

    $("input[albAutoComplete=autoCompNomInterlocuteur]").die().live("change", function () {
        var context = $(this).attr('albcontext');
         $('#CourtierInvalideDiv').empty().removeClass("error").removeClass("warning");
         $("input[albAutoComplete=autoCompCodeInterlocuteur]" + selectCtx(context)).val("");
         $(this).val("");
    });
    $("input[albAutoComplete=autoCompNomInterlocuteur]").interlocuteurAutoComplete({
        position: { my: "left top", at: "left bottom" },
        delay: 750,
        source: GetInterlocuteur,
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateCourtier(ui.item, context, false);
            return false;
        }
    });

}
//--fonction pour obtenir la liste d'interlocuteur
//a partir de son nom et du code du cabinet de courtage---
function GetInterlocuteur(request, response) {
    var nomInterlocuteurVal = request.term;
    var codeCabinetCourtageVal = "";
    var context = $(document.activeElement).attr("albcontext");
    if (context != undefined && context != "")
        codeCabinetCourtageVal = $("input[albAutoComplete=autoCompCodeCourtier][albcontext='" + context + "']").val();
    else
        codeCabinetCourtageVal = $("input[albAutoComplete=autoCompCodeCourtier]").val();

    $.ajax({
        url: "/AutoComplete/GetInterlocuteurs",
        data: { nomInterlocuteur: nomInterlocuteurVal, codeCabinetCourtage: codeCabinetCourtageVal },
        success: function (json) {
            response(json);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------Mise à jour du courtier-----------
function UpdateCourtier(item, context, changeEncaissement) {
    var codeCourtier = item.Code != "0" ? item.Code : "";


    $("input[albAutoComplete=autoCompNomCourtier]" + selectCtx(context)).val(item.Nom);
    $("input[albAutoComplete=autoCompCodeCourtier]" + selectCtx(context)).val(codeCourtier).data('validCode', codeCourtier);


    $("img[albAutoComplete=autoCompImgInfoCourtier]" + selectCtx(context)).attr("albIdInfo", codeCourtier);

    $("input[albAutoComplete=autoCompTypeCourtier]" + selectCtx(context)).val(item.Type);
    $("input[albAutoComplete=autoCompCodeInterlocuteur]" + selectCtx(context)).val(item.IdInterlocuteur);
    if (item.IdInterlocuteur != null && item.NomInterlocuteur != null && item.IdInterlocuteur != "" && item.NomInterlocuteur != "")
        $("input[albAutoComplete=autoCompNomInterlocuteur]" + selectCtx(context)).val(item.IdInterlocuteur + " - " + item.NomInterlocuteur);
    else
        $("input[albAutoComplete=autoCompNomInterlocuteur]" + selectCtx(context)).val("");
    if (item.IdInterlocuteur == null || item.IdInterlocuteur == "" || item.IdInterlocuteur == "0") {
        $("#inInvalidCourtierApp").val("");
        $('#CourtierInvalideDiv').empty().removeClass("error").removeClass("warning");
        $('#CourtierApporteurInvalideDiv').empty().removeClass("error").removeClass("warning");
        if (!item.EstValide) {
            if (!ShowNonExistingCourtier(item)) {
                $("#inInvalidCourtierApp").val("1");
                $('#CourtierInvalideDiv').append("Code fermé").addClass("error");
                $('#CourtierApporteurInvalideDiv').append("Code fermé").addClass("error");
            }
            if ($("#RemplacerCourtier")) {
                $("#RemplacerCourtier").attr("disabled", "disabled");
                $("#MaintenirCourtier").attr("checked", "checked").trigger("change");
                $("#MotifRemp").val("").attr("disabled", "disabled");
            }
        }
        else {
            if ($("#RemplacerCourtier")) {
                $("#RemplacerCourtier").removeAttr("disabled");
            }
        }
        if (!item.DemarcheCom && item.EstValide) {
            $('#CourtierInvalideDiv').append("Interdit de démarche commerciale ").addClass("warning");
        }
    }
    else {
        $("#inInvalidCourtierApp").val("");
        $('#CourtierApporteurInvalideDiv').empty().removeClass("error").removeClass("warning");
        $('#CourtierInvalideDiv').empty().removeClass("error").removeClass("warning");
        if (!item.EstValide) {
            $("#inInvalidCourtierApp").val("1");
            $('#CourtierInvalideDiv').append("Code fermé").addClass("error");
            $('#CourtierApporteurInvalideDiv').append("Code fermé").addClass("error");
        }
        if (!item.DemarcheCom && item.EstValide) {
            $('#CourtierInvalideDiv').append("Interdit de démarche commerciale ").addClass("warning");
        }
    }
    //récupération des infos lors du chargement de la page pour une maj
    if ($("#hideInterlocuteur").val() != "" && typeof ($("#hideInterlocuteur").val()) != "undefined") {
        $("input[albAutoComplete=autoCompCodeInterlocuteur]" + selectCtx(context)).val($("#hideInterlocuteur").val().split('_')[0]);
        $("input[albAutoComplete=autoCompNomInterlocuteur]" + selectCtx(context)).val($("#hideInterlocuteur").val().split('_')[1]);
        $("#hideInterlocuteur").val("");
    }

    $("input[albAutoComplete=autoCompDelegCourtier]" + selectCtx(context)).val(item.Delegation);
    if (context == "dblSaisie") {
        $("#DblReference").val("");
    }
    if (codeCourtier == "")
        $('#CourtierInvalideDiv').html("");
    updateGestionnairePayeur();
}
function ShowNonExistingCourtier(item) {
    item = item || {};
    if (item.Code == null || item.Code == "") {
        $('#CourtierInvalideDiv').append("Code inexistant").addClass("error");
        return true;
    }
    return false;
}
/**
 * Affichage des messages du validation courtier
 * @param {any} element div d'alerte
 * @param {any} message message à afficher
 */
function showAlertMessageCourtier(element, message) {
    var closedCode = "Code fermé";
    var comInterdiction = "Interdit de démarche commerciale";
    var messageStyle = "";
    styleToRemove = "";
    if (element) {
        if (message.indexOf(closedCode) >= 0) {
            messageStyle = "error";
            styleToRemove = "warning";
        } else {
            if (message.indexOf(comInterdiction)) {
                messageStyle = "warning";
                styleToRemove = "error";
            }
        }
        if (messageStyle != "") {
            if (!element.hasClass(messageStyle)) {
                element.addClass(messageStyle);
            }
            if (element.hasClass(styleToRemove)) {
                element.removeClass(styleToRemove);
            }
        }
        element.html(message);
    }
}
/**
 * Affichage des messages du validation courtier
 * @param {any} element div d'alerte
 * @param {any} message message à afficher
 */
function showAlertMessageCourtier(element, message) {
    var closedCode = "Code fermé";
    var comInterdiction = "Interdit de démarche commerciale";
    var messageStyle = "";
    styleToRemove = "";
    if (element) {
        if (message.indexOf(closedCode) >= 0) {
            messageStyle = "error";
            styleToRemove = "warning";
        } else {
            if (message.indexOf(comInterdiction)) {
                messageStyle = "warning";
                styleToRemove = "error";
            }
        }
        if (messageStyle != "") {
            if (!element.hasClass(messageStyle)) {
                element.addClass(messageStyle);
            }
            if (element.hasClass(styleToRemove)) {
                element.removeClass(styleToRemove);
            }
        }
        element.html(message);
    }
}
function updateGestionnairePayeur(applyAtoComplete) {
    if (!applyAtoComplete) {
        DieInfoCommInterlocuteurByNom();
        DieAutoLoadInfoCommCourtier();

    }
    var checked = $("#GpIdentiqueApporteur").is(':checked');
    if (checked) {
        var code = $("#CourtierGestionnaire_Courtier_CodeCourtier").val();
        var nom = $("#CourtierGestionnaire_Courtier_NomCourtier").val();
        var ref = $("#Reference").val();
        var msgInvalide = $("#CourtierInvalideGestionnaireDiv").html();
        showAlertMessageCourtier($("#CourtierInvalideDiv"), msgInvalide);
        showAlertMessageCourtier($("#CourtierInvalidePayeurDiv"), msgInvalide);

        $("img[name=RechercherCourtier][albcontext=courtierApporteur]").removeClass("CursorPointer").attr("src", "/Content/Images/loupegris.png");
        $("img[name=RechercherCourtier][albcontext=courtierPayeur]").removeClass("CursorPointer").attr("src", "/Content/Images/loupegris.png");
        $("#CourtierApporteur_Courtier_CodeCourtier").val(code).attr('disabled', 'disabled').addClass('readonly');
        $("#CourtierApporteur_Courtier_NomCourtier").val(nom).attr('disabled', 'disabled').addClass('readonly');
        $("#CourtierApporteur_Courtier_Reference").val(ref).attr('disabled', 'disabled').addClass('readonly');
        $("#CourtierApporteur_Courtier_NomInterlocuteur").val(ref).attr('disabled', 'disabled').addClass('readonly');
        $("#CourtierPayeur_Courtier_CodeCourtier").val(code).attr('disabled', 'disabled').addClass('readonly');
        $("#CourtierPayeur_Courtier_NomCourtier").val(nom).attr('disabled', 'disabled').addClass('readonly');
        $("#CourtierPayeur_Courtier_NomInterlocuteur").attr('disabled', 'disabled').addClass('readonly');
        $("#CourtierPayeur_Courtier_Reference").val(ref).attr('disabled', 'disabled').addClass('readonly');
        $("#CourtierApporteur_Courtier_CodeCourtier").trigger("change");
    }
    else {

        $("img[name=RechercherCourtier][albcontext=courtierApporteur]").addClass("CursorPointer").attr("src", "/Content/Images/loupe.png");
        $("img[name=RechercherCourtier][albcontext=courtierPayeur]").addClass("CursorPointer").attr("src", "/Content/Images/loupe.png");
        $("#CourtierApporteur_Courtier_CodeCourtier").removeAttr('disabled');
        $("#CourtierApporteur_Courtier_CodeCourtier").removeClass('readonly');
        $("#CourtierApporteur_Courtier_NomCourtier").removeAttr('disabled');
        $("#CourtierApporteur_Courtier_NomCourtier").removeClass('readonly');
        $("#CourtierApporteur_Courtier_Reference").removeAttr('disabled');
        $("#CourtierApporteur_Courtier_Reference").removeClass('readonly');
        $("#CourtierApporteur_Courtier_NomInterlocuteur").removeAttr('disabled');
        $("#CourtierApporteur_Courtier_NomInterlocuteur").removeClass('readonly');
        $("#CourtierPayeur_Courtier_CodeCourtier").removeAttr('disabled');
        $("#CourtierPayeur_Courtier_CodeCourtier").removeClass('readonly');
        $("#CourtierPayeur_Courtier_NomCourtier").removeAttr('disabled');
        $("#CourtierPayeur_Courtier_NomCourtier").removeClass('readonly');
        $("#CourtierPayeur_Courtier_NomInterlocuteur").removeAttr('disabled');
        $("#CourtierPayeur_Courtier_NomInterlocuteur").removeClass('readonly');
        $("#CourtierPayeur_Courtier_Reference").removeAttr('disabled');
        $("#CourtierPayeur_Courtier_Reference").removeClass('readonly');
    }
    if (!applyAtoComplete) {
        LoadInfoCommInterlocuteurByNom(applyAtoComplete);
        LivedieAutoLoadInfoCommCourtier();
    }
}


/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Cabinet Courtage
//#region AutoComplete Cabinet Gestionnaire
/***********************************************************/
/****    AutoComplete Cabinet Courtage Gestionnaire    *****/
/***********************************************************/

function MapCommonAutoCompCourtierGestion() {
    LoadInfoCommCourtierGestionByNom();
    LoadInfoCommCourtierGestionByCode();
   LoadInfoCommInterlocuteurGestionByNom();
}
//----Application de la recherche ajax par code du courtier---
function LoadInfoCommCourtierGestionByCode() {
    $("input[albAutoComplete=autoCompCodeCourtierGestion]").offOn("change", function () {
        var codeCabinet = $(this).val();
        var context = $(this).attr('albcontext');
        $('#CourtierInvalideGestionnaireDiv').empty().removeClass("error").removeClass("warning");
        if (codeCabinet != "") {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/AutoComplete/GetCabinetCourtageByCode",
                data: { codeString: codeCabinet },
                success: function (json) {
                    UpdateCourtierGestion(json, context, true);
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {

            if (context != undefined && context != '' && !$("#GpIdentiqueApporteur").is(":checked")) {
                $("input[albAutoComplete=autoCompNomCourtierGestion]" + selectCtx(context)).val("");
                $("input[albAutoComplete=autoCompCodeCourtierGestion]" + selectCtx(context)).val("");
                $("input[albAutoComplete=autoCompCodeInterlocuteurGestion]" + selectCtx(context)).val("");
                $("input[albAutoComplete=autoCompNomInterlocuteurGestion]" + selectCtx(context)).val("");
            }
            else {
                $("input[albAutoComplete=autoCompNomCourtierGestion]").val("");
                $("input[albAutoComplete=autoCompCodeCourtierGestion]").val("");
                $("input[albAutoComplete=autoCompCodeInterlocuteurGestion]").val("");
                $("input[albAutoComplete=autoCompNomInterlocuteurGestion]").val("");

                $("input[albAutoComplete=autoCompNomCourtier]").val("");
                $("input[albAutoComplete=autoCompCodeCourtier]").val("");

                $('#CourtierInvalideDiv').empty().removeClass("error").removeClass("warning");
                $('#CourtierInvalidePayeurDiv').empty().removeClass("error").removeClass("warning");
                $("input[albAutoComplete=autoCompNomCourtierPayeur]").val("");
                $("input[albAutoComplete=autoCompCodeCourtierPayeur]").val("");
            }
        }
    });
}
//----Application de la recherche ajax par nom du courtier---
function LoadInfoCommCourtierGestionByNom() {
    $("input[albAutoComplete=autoCompNomCourtierGestion]").offOn("change", function () {
        var context = $(this).attr('albcontext');
        $('#CourtierInvalideGestionnaireDiv').empty().removeClass("error").removeClass("warning");

        if (context != undefined && context != '' && !$("#GpIdentiqueApporteur").is(":checked")) {
            $("input[albAutoComplete=autoCompCodeCourtierGestion]" + selectCtx(context)).val("");
            $("input[albAutoComplete=autoCompCodeInterlocuteurGestion]" + selectCtx(context)).val("");
            $("input[albAutoComplete=autoCompNomInterlocuteurGestion]" + selectCtx(context)).val("");
        }
        else {
            $("input[albAutoComplete=autoCompCodeCourtierGestion]").val("");
            $("input[albAutoComplete=autoCompCodeInterlocuteurGestion]").val("");
            $("input[albAutoComplete=autoCompNomInterlocuteurGestion]").val("");

            $("input[albAutoComplete=autoCompNomCourtier]").val("");
            $("input[albAutoComplete=autoCompCodeCourtier]").val("");
            $('#CourtierInvalideDiv').empty().removeClass("error").removeClass("warning");
            $('#CourtierInvalidePayeurDiv').empty().removeClass("error").removeClass("warning");
            $("input[albAutoComplete=autoCompNomCourtierPayeur]").val("");
            $("input[albAutoComplete=autoCompCodeCourtierPayeur]").val("");
        }
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompNomCourtierGestion]").nomCabinetAutoComplete({
        open: function () {
            var context = $(this).attr('albcontext');
            $("input[albAutoComplete=autoCompCodeCourtierGestion]" + selectCtx(context)).val('');

        },
        delay: 750,
        source: "/AutoComplete/GetCabinetsCourtagesByName",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateCourtierGestion(ui.item, context, true);
            return false;
        }
    });
}
//----Application de la recherche ajax par nom d'interlocuteur--
function LoadInfoCommInterlocuteurGestionByNom() {
    $("input[albAutoComplete=autoCompNomInterlocuteurGestion]").die().live("change", function () {
        var context = $(this).attr('albcontext');
        $("input[albAutoComplete=autoCompCodeInterlocuteurGestion]" + selectCtx(context)).val("");
        $(this).val("");
        //$("input[albAutoComplete=autoCompCodeCourtierGestion]").trigger("change");
    });
    $("input[albAutoComplete=autoCompNomInterlocuteurGestion]").interlocuteurAutoComplete({
        delay: 750,
        source: GetInterlocuteurGestion,
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateCourtierGestion(ui.item, context, false);
            return false;
        }
    });
    //$("input[albAutoComplete=autoCompNomInterlocuteurGestion]").die().live('blur', function () {
    //    var context = $(this).attr('albcontext');
    //    if ($("input[albAutoComplete=autoCompCodeInterlocuteurGestion]" + selectCtx(context)).val() == 0 || $("input[albAutoComplete=autoCompCodeInterlocuteurGestion]" + selectCtx(context)).val() == "")
    //        $(this).val("");

    //});
}
//--fonction pour obtenir la liste d'interlocuteur
//a partir de son nom et du code du cabinet de courtage---
function GetInterlocuteurGestion(request, response) {
    var nomInterlocuteurVal = request.term;
    var codeCabinetCourtageVal = $("input[albAutoComplete=autoCompCodeCourtierGestion]").val();
    $.ajax({
        url: "/AutoComplete/GetInterlocuteurs",
        data: { nomInterlocuteur: nomInterlocuteurVal, codeCabinetCourtage: codeCabinetCourtageVal },
        success: function (json) {
            response(json);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------Mise à jour du courtier-----------
function UpdateCourtierGestion(item, context, changeEncaissement) {
    var codeCourtier = item.Code != "0" ? item.Code : "";

    $("input[albAutoComplete=autoCompNomCourtierGestion]" + selectCtx(context)).val(item.Nom);
    $("input[albAutoComplete=autoCompCodeCourtierGestion]" + selectCtx(context)).val(codeCourtier);

    $("input[albAutoComplete=autoCompTypeCourtierGestion]" + selectCtx(context)).val(item.Type);
    $("img[albAutoComplete=autoCompImgInfoCourtierGestion]" + selectCtx(context)).attr("albIdInfo", codeCourtier);

    if (item.IdInterlocuteur != null && item.IdInterlocuteur != "")
        $("input[albAutoComplete=autoCompCodeInterlocuteurGestion]" + selectCtx(context)).val(item.IdInterlocuteur);
    else {
        $("input[albAutoComplete=autoCompCodeInterlocuteurGestion]" + selectCtx(context)).val(0);
        $("input[albAutoComplete=autoCompNomInterlocuteurGestion]" + selectCtx(context)).val("");
        $("input[albAutoComplete=autoCompReferenceGestion]" + selectCtx(context)).val("");

    }
    if (item.IdInterlocuteur != null && item.NomInterlocuteur != null && item.IdInterlocuteur != "" && item.NomInterlocuteur != "")
        $("input[albAutoComplete=autoCompNomInterlocuteurGestion]" + selectCtx(context)).val(item.NomInterlocuteur);
    if (item.IdInterlocuteur == null || item.IdInterlocuteur == "" || item.IdInterlocuteur == "0") {
        $("#inInvalidCourtierGest").val("");
        $('#CourtierInvalideGestionnaireDiv').empty().removeClass("error").removeClass("warning");
        $('#CourtierApporteurInvalideDiv').empty().removeClass("error").removeClass("warning");
        if (!item.EstValide) {
            $("#inInvalidCourtierGest").val("1");
            $('#CourtierInvalideGestionnaireDiv').append("Code fermé").addClass("error");
            $('#CourtierApporteurInvalideDiv').append("Code fermé").addClass("error");
        }
        if (!item.DemarcheCom && item.EstValide) {
            $('#CourtierInvalideGestionnaireDiv').append("Interdit de démarche commerciale ").addClass("warning");
        }
    }
    else {
        $("#inInvalidCourtierGest").val("");
        $('#CourtierInvalideGestionnaireDiv').empty().removeClass("error").removeClass("warning");
        $('#CourtierApporteurInvalideDiv').empty().removeClass("error").removeClass("warning");
        if (!item.EstValide) {
            $("#inInvalidCourtierGest").val("1");
            $('#CourtierInvalideGestionnaireDiv').append("Code fermé").addClass("error");
            $('#CourtierApporteurInvalideDiv').append("Code fermé").addClass("error");
        }
        if (!item.DemarcheCom && item.EstValide) {
            $('#CourtierInvalideGestionnaireDiv').append("Interdit de démarche commerciale ").addClass("warning");
        }
    }

    //récupération des infos lors du chargement de la page pour une maj
    if ($("#hideInterlocuteur").val() != "" && typeof ($("#hideInterlocuteur").val()) != "undefined") {
        $("input[albAutoComplete=autoCompCodeInterlocuteurGestion]" + selectCtx(context)).val($("#hideInterlocuteur").val().split('_')[0]);
        $("input[albAutoComplete=autoCompNomInterlocuteurGestion]" + selectCtx(context)).val($("#hideInterlocuteur").val().split('_')[1]);
        $("#hideInterlocuteur").clear();
    }
    //récupération des infos de référence
    if ($("#hideRefCourtier").val() != "" && typeof ($("#hideRefCourtier").val()) != "undefined") {
        $("input[albAutoComplete='autoCompReferenceGestion'][albcontext='" + context + "']").val($("#hideRefCourtier").val());
        $("#hideRefCourtier").clear();
    }

    $("input[albAutoComplete=autoCompDelegCourtier]").val(item.Delegation);

    //Modification bug 1969 : quittancement en fonction du courtier gestionnaire.
    if (changeEncaissement) {
        if ($("#EncaissementValueOnLoad").val() == null || $("#EncaissementValueOnLoad").val() === "") {
            $("select[id='CourtierPayeur_Encaissement']").val(item.EncaissementCode);
        }
        else {
            if ($("#EncaissementValueOnLoad").val() != undefined)
                $("#EncaissementValueOnLoad").clear();
        }
    }
    if (codeCourtier === "") {
        $('#CourtierInvalideGestionnaireDiv').clearHtml();
        $('#CourtierInvalideDiv').clearHtml();
    }

    if (typeof (infosBase) !== "undefined" && infosBase) {
        infosBase.refreshDataCourtiers();
    }
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Cabinet Gestionnaire
//#region AutoComplete Cabinet Payeur
/***********************************************************/
/****        AutoComplete Cabinet Courtage Payeur      *****/
/***********************************************************/

function MapCommonAutoCompCourtierPayeur() {
    LoadInfoCommCourtierPayeurByNom();
    LoadInfoCommCourtierPayeurByCode();
    LoadInfoCommInterlocuteurPayeurByNom();
}
//----Application de la recherche ajax par code du courtier---
function LoadInfoCommCourtierPayeurByCode() {
    $("input[albAutoComplete=autoCompCodeCourtierPayeur]").die().live("change", function () {
        var context = $(this).attr('albcontext');
        $('#CourtierInvalidePayeurDiv').empty().removeClass("error").removeClass("warning");
        var codeCabinet = $(this).val();
        if (codeCabinet != "") {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/AutoComplete/GetCabinetCourtageByCode",
                data: { codeString: codeCabinet },
                success: function (json) {
                    UpdateCourtierPayeur(json, context);
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            $('#CourtierInvalideDiv').append("");
            $("input[albAutoComplete=autoCompNomCourtierPayeur]" + selectCtx(context)).val("");
            $("input[albAutoComplete=autoCompCodeCourtierPayeur]" + selectCtx(context)).val("");

        }
    });
}
//----Application de la recherche ajax par nom du courtier---
function LoadInfoCommCourtierPayeurByNom() {
    $("input[albAutoComplete=autoCompNomCourtierPayeur]").die().live("change", function () {
        var context = $(this).attr('albcontext');
        $('#CourtierInvalidePayeurDiv').empty().removeClass("error").removeClass("warning");
        $("input[albAutoComplete=autoCompCodeCourtierPayeur]" + selectCtx(context)).val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompNomCourtierPayeur]").nomCabinetAutoComplete({
        open: function () {
            var context = $(this).attr('albcontext');
            $("input[albAutoComplete=autoCompCodeCourtierPayeur]" + selectCtx(context)).val('');
        },
        delay: 750,
        source: "/AutoComplete/GetCabinetsCourtagesByName",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateCourtierPayeur(ui.item, context);
            return false;
        }
    });
}
//----Application de la recherche ajax par nom d'interlocuteur--
function LoadInfoCommInterlocuteurPayeurByNom() {
    $("input[albAutoComplete=autoCompNomInterlocuteurPayeur]").die().live("change", function () {
        var context = $(this).attr('albcontext');
        $("input[albAutoComplete=autoCompCodeInterlocuteurPayeur]" + selectCtx(context)).val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompNomInterlocuteurPayeur]").interlocuteurAutoComplete({
        delay: 750,
        source: GetInterlocuteur,
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateCourtierPayeur(ui.item, context);
            return false;
        }
    });
    $("input[albAutoComplete=autoCompNomInterlocuteurPayeur]").die().live('blur', function () {
        var context = $(this).attr('albcontext');
        if ($("input[albAutoComplete=autoCompCodeInterlocuteurPayeur]" + selectCtx(context)).val() == 0 || $("input[albAutoComplete=autoCompCodeInterlocuteurPayeur]" + selectCtx(context)).val() == "")
            $(this).val("");
    });
}
//--fonction pour obtenir la liste d'interlocuteur
//a partir de son nom et du code du cabinet de courtage---
function GetInterlocuteurPayeur(request, response) {
    var nomInterlocuteurVal = request.term;
    var codeCabinetCourtageVal = $("input[albAutoComplete=autoCompCodeCourtierPayeur]").val();
    $.ajax({
        url: "/AutoComplete/GetInterlocuteurs",
        data: { nomInterlocuteur: nomInterlocuteurVal, codeCabinetCourtage: codeCabinetCourtageVal },
        success: function (json) {
            response(json);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------Mise à jour du courtier-----------
function UpdateCourtierPayeur(item, context) {
    var codeCourtier = item.Code != "0" ? item.Code : "";

    $("input[albAutoComplete=autoCompNomCourtierPayeur]" + selectCtx(context)).val(item.Nom);
    $("input[albAutoComplete=autoCompCodeCourtierPayeur]" + selectCtx(context)).val(codeCourtier);

    $("img[albAutoComplete=autoCompImgInfoCourtierPayeur]" + selectCtx(context)).attr("albIdInfo", codeCourtier);

    $("input[albAutoComplete=autoCompTypeCourtierPayeur]" + selectCtx(context)).val(item.Type);
    $("input[albAutoComplete=autoCompCodeInterlocuteurPayeur]" + selectCtx(context)).val(item.IdInterlocuteur);
    if (item.IdInterlocuteur != null && item.NomInterlocuteur != null && item.IdInterlocuteur != "" && item.NomInterlocuteur != "")
        $("input[albAutoComplete=autoCompNomInterlocuteurPayeur]" + selectCtx(context)).val(item.IdInterlocuteur + " - " + item.NomInterlocuteur);
    if (item.IdInterlocuteur == null || item.IdInterlocuteur == "" || item.IdInterlocuteur == "0") {
        $("#inInvalidCourtierPay").val("");
        $('#CourtierInvalidePayeurDiv').empty().removeClass("error").removeClass("warning");
        if (!item.EstValide) {
            $("#inInvalidCourtierPay").val("1");
            $('#CourtierInvalidePayeurDiv').append("Code fermé").addClass("error");
        }
        if (!item.DemarcheCom && item.EstValide) {
            $('#CourtierInvalidePayeurDiv').append("Interdit de démarche commerciale ").addClass("warning");
        }
    }
    //récupération des infos lors du chargement de la page pour une maj
    if ($("#hideInterlocuteur").val() != "" && typeof ($("#hideInterlocuteur").val()) != "undefined") {
        $("input[albAutoComplete=autoCompCodeInterlocuteurPayeur]" + selectCtx(context)).val($("#hideInterlocuteur").val().split('_')[0]);
        $("input[albAutoComplete=autoCompNomInterlocuteurPayeur]" + selectCtx(context)).val($("#hideInterlocuteur").val().split('_')[1]);
        $("#hideInterlocuteur").val("");
    }
    if (codeCourtier == "") {
        $('#CourtierInvalidePayeurDiv').html("");
    }
    $("input[albAutoComplete=autoCompDelegCourtier]" + selectCtx(context)).val(item.Delegation);

}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Cabinet Payeur
//#region AutoComplete Assuré
/***********************************************************/
/***********        AutoComplete Assuré         ************/
/***********************************************************/

function MapCommonAutoCompAssure() {
    //---Charge les infos des preneurs d'assurance pour l'autocomplete---
    $.widget("custom.preneurAssuranceAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            var self = this;
            $.each(items, function (index, item) {
                if (index == 0 && item.Nom == "")
                    ul.append("Aucun résultat");
                else {
                    if (index == 0) {
                        ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Nom</th><th class='AutoCompleteTd'>Secondaire</th><th class='AutoCompleteCpTd'>Depts</th><th class='AutoCompleteTd'>Ville</th><th class='AutoCompleteTd'>SIREN</th></tr></table></li>");
                    }
                    self._renderItem(ul, item);
                }
            });
        },
        _renderItem: function (ul, item) {
            var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.Code + "</td><td class='AutoCompleteTd'>" + item.Nom + "</td><td class='AutoCompleteTd'>" + (item.NomSecondaires[0]||"") + "</td><td class='AutoCompleteCpTd'>" + item.Departement + "</td><td class='AutoCompleteTd'>" + item.Ville + "</td><td class='AutoCompleteTd'>" + item.SIREN + "</td></tr>";
            if (item.NomSecondaires.length > 1) {
                for (i = 1; i < item.NomSecondaires.length; i++) {
                    tableau = tableau + "<tr><td class='AutoCompleteTd'></td><td class='AutoCompleteTd'>" + item.NomSecondaires[i] + "</td><td class='AutoCompleteCpTd'></td><td class='AutoCompleteTd'></td><td class='AutoCompleteTd'></td>";
                }
            }
            tableau = tableau + "</table></a>";

            return $("<li></li>")
                .data("item.autocomplete", item)
                .append(tableau)
                .appendTo(ul);
        }
    });
    LoadInfoCommonAssuByNom();
    LoadInfoCommonAssuByCode();
    ClearFieldOnAutoComplete();
}

function ClearFieldOnAutoComplete() {

    $("input[albAutoComplete=autoCompVilleAssure]").die().live('change', function (event) {
        var context = $(this).attr("albcontext");
        $("input[albAutoComplete=autoCompCodeAssure]" + selectCtx(context)).val('');
        $("input[albAutoComplete=autoCompNomAssure]" + selectCtx(context)).val('');
        $("input[albAutoComplete=autoCompCodePostalAssure]" + selectCtx(context)).val('');

    });
}

//---Application de la recherche ajax par code du preneur d'assurance--
function LoadInfoCommonAssuByCode() {
    $("input[albAutoComplete=autoCompCodeAssure]").attr("maxlength", 7).offOn('input change', debounce(function () {
        const $this = $(this);
        const context = $this.attr("albcontext");
        const codePreneurAssurance = $this.val();
        if (!codePreneurAssurance) { UpdateAssure({}, context); }
        const intCode = parseInt(codePreneurAssurance);
        if (isNaN(intCode) || intCode == 0) {
            UpdateAssure({}, context);
            return false;
        }
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/AutoComplete/GetPreneurAssuranceByCode",
            data: { codeString: codePreneurAssurance },
            success: function (json) {
                CloseLoading();
                if (codePreneurAssurance == json.Code) {
                    UpdateAssure(json, context);
                }
                else {
                    UpdateAssure({}, context);
                }
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }, 700)).live("blur", function () {
        const $this = $(this);
        const code = $this.val();
        const context = $this.attr("albcontext");
        if (code != $this.data('validCode')) UpdateAssure({}, context);
    });
}
//----Application de la recherche ajax par nom de l'assure---
function LoadInfoCommonAssuByNom() {
    $("input[albAutoComplete=autoCompNomAssure]").live("change", function () {
        var context = $(this).attr('albcontext');
        var val = $(this).data("selectedItem");
        if (!val || this.value != val.Nom) {
            $("input[albAutoComplete=autoCompCodeAssure]" + selectCtx(context)).val("");
            $(document).trigger("autoCompAssureSelected", { item: null, target: this });
            $(this).val("");
        }
    });
    common.dom.onEvent("change", "input[albAutoComplete=autoCompNomAssure]", function () {
        var context = $(this).attr('albcontext');
        $("input[albAutoComplete=autoCompCodeAssure]" + selectCtx(context)).val("");
        $(document).trigger("autoCompAssureSelected", { item: null, target: this });
    });
    $("input[albAutoComplete=autoCompNomAssure]").preneurAssuranceAutoComplete({
        open: function () {
            var context = $(this).attr('albcontext');
            $("input[albAutoComplete=autoCompCodeAssure]" + selectCtx(context)).val('');
            $("input[albAutoComplete=autoCompCodePostalAssure]" + selectCtx(context)).val('');
            $("input[albAutoComplete=autoCompVilleAssure]" + selectCtx(context)).val('');
        },
        delay: 750,
        source: "/AutoComplete/GetAssureByName",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateAssure(ui.item, context);
            $(document).trigger("autoCompAssureSelected", { item: ui.item, target: this });
            return false;
        }
    });

}
//------Mise à jour de l'assuré-----------
function UpdateAssure(item, context) {
    var codePreneur = item.Code != "0" && typeof (item.Code) != "undefined"  ? item.Code : "";

    $("input[albAutoComplete=autoCompCodeAssure]" + selectCtx(context)).val(item.Code).data('validCode', item.Code);
    $("input[albAutoComplete=autoCompNomAssure]" + selectCtx(context)).val(item.Nom);
    if (context != "adressePreneur") {
        $("input[albAutoComplete=autoCompCodePostalAssure]" + selectCtx(context)).val(item.CodePostalString);
    }
    $("input[albAutoComplete=autoCompVilleAssure]" + selectCtx(context)).val(item.Ville);
    $("input[albAutoComplete=autoCompDepartementAssure]" + selectCtx(context)).val(item.Departement);
    $("img[albAutoComplete=autoCompImgInfoAssure]" + selectCtx(context)).attr("albIdInfo", item.Code || "");

    $("#inInvalidPreneurAssu" + selectCtx(context)).val("");
    $('#PreneurAssuInvalideDiv' + selectCtx(context)).empty().removeClass("error").removeClass("warning");
    if (!item.EstActif) {
        $("#inInvalidPreneurAssu" + selectCtx(context)).val("1");
        $('#PreneurAssuInvalideDiv' + selectCtx(context)).append("Inactif").addClass("error");
    }

    if (codePreneur === "") {
        $('#PreneurAssuInvalideDiv' + selectCtx(context)).clearHtml();
    }

    // Commentaire à supprimer pour réouvrir la fonctionnalité "Alertes - Retards - Sinistres
    showAlertMessageAssure(item, true);
};

var showAlertMessageAssure = function (item, iconMode) {
    let message = "";
    if (item.Sinistre || item.RetardsPaiements || item.Impayes) {
        message = "Attention !\nCe preneur d'assurance a sur son portefeuille au moins l'un des cas suivants :\n  - des retards de paiement\n  - des impayés\n  - des sinistres";
    }
    if (iconMode) {
        $("#linkAlertesPreneur").parent().addClass("hide-it");
        $("#linkAlertesPreneur").removeAttr("title");
        if (item.Sinistre || item.RetardsPaiements || item.Impayes) {
            let stringArray = [];
            if (item.Sinistre) stringArray.push("'S'");
            if (item.RetardsPaiements) stringArray.push("'RP'");
            if (item.Impayes) stringArray.push("'I'");
            $("#linkAlertesPreneur").data("alertes", "[" + stringArray.join(",") + "]");
            $("#linkAlertesPreneur").data("preneur", item.Code);
            $("#linkAlertesPreneur").data("nompreneur", item.Nom);
            $("#linkAlertesPreneur").attr("title", message);
            $("#linkAlertesPreneur").parent().removeClass("hide-it");
        }
    }
    else {
        if (message.length > 0) {
            common.error.showMessage(message);
        }
    }
};

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Assuré
//#region AutoComplete Souscripteur
/***********************************************************/
/***********     AutoComplete Souscripteur      ************/
/***********************************************************/

function MapCommonAutoCompSouscripteur() {
    //---Charge les infos des souscripteurs pour l'autocomplete---
    $.widget("custom.souscripteurCommonAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.Nom == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Nom</th><th class='AutoCompleteTd'>Prénom</th><th class='AutoCompleteTd'>Délégation</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a class='ui-menu-item'><table><tr><td class='AutoCompleteTd'>" + item.Code + "</td><td class='AutoCompleteTd'>" + item.Nom + "</td><td class='AutoCompleteTd'>" + item.Prenom + "</td><td class='AutoCompleteTd'>" + item.Delegation + "</td></tr></table></a>")
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoCommonSousByNom();
}
//---Charge les infos des souscripteurs en fonction du nom---
function LoadInfoCommonSousByNom() {
    $("input[albAutoComplete=autoCompSouscripteurNom]").die().live('change', function () {
        var context = $(this).attr('albcontext');
        var val = $(this).data("selectedItem");
        if (val && this.value != (val.Code + " - " + val.Nom)) {
            $("input[albAutoComplete=autoCompSouscripteurCode]" + selectCtx(context)).val("");

            $(this).val("");
        }
    });
    $("input[albAutoComplete=autoCompSouscripteurNom]").souscripteurCommonAutoComplete({
        delay: 750,
        source: function (req, rep) {
            $this = $(this.element);
            $.get("/AutoComplete/GetSouscripteursByName?term=" + encodeURIComponent(req.term)).then(
                function (data) {
                    rep(data);
                }
            );
        },
        change: function () {
            var context = $(this).attr('albcontext');
            if ($("input[albAutoComplete=autoCompSouscripteurSelect]" + selectCtx(context)).val() != "1") {
                $("input[albAutoComplete=autoCompSouscripteurCode]" + selectCtx(context)).val("");
                $("input[albAutoComplete=autoCompSouscripteurNom]" + selectCtx(context)).val("");
            }
            $("input[name=autoCompSouscripteurSelect]" + selectCtx(context)).val("");

        },
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            $(this).data('albSelectedItem', ui.item);
            UpdateSouscripteur(ui.item, context);
            $(document).trigger("autoCompSouscripteurSelected", { item: ui.item, target: this });
            return false;
        }
    });
}
//---Met à jour les controles du souscripteur---
function UpdateSouscripteur(item, context) {
    $("input[albAutoComplete=autoCompSouscripteurCode]" + selectCtx(context)).val(item.Code);
    $("input[albAutoComplete=autoCompSouscripteurNom]" + selectCtx(context)).val(item.Code + " - " + item.Nom);
    $("input[albAutoComplete=autoCompSouscripteurSelect]" + selectCtx(context)).val("1");
    $("input[albAutoComplete=autoCompSouscripteurActive]" + selectCtx(context)).val(item.Active);
};

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Souscripteur
//#region AutoComplete Gestionnaire
/***********************************************************/
/***********     AutoComplete Gestionnaire      ************/
/***********************************************************/

function MapCommonAutoCompGestionnaire() {
    //---Charge les infos des gestionnaires pour l'autocomplete---
    $.widget("custom.gestionnaireCommonAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH},
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.Nom == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Nom</th><th class='AutoCompleteTd'>Prénom</th><th class='AutoCompleteTd'>Délégation</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a class='ui-menu-item'><table><tr><td class='AutoCompleteTd'>" + item.Code + "</td><td class='AutoCompleteTd'>" + item.Nom + "</td><td class='AutoCompleteTd'>" + item.Prenom + "</td><td class='AutoCompleteTd'>" + item.Delegation + "</td></tr></table></a>")
                    .appendTo(ul);
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoCommonGestByNom();
}
//---Charge les infos des gestionnaires en fonction du nom---
function LoadInfoCommonGestByNom() {
    $("input[albAutoComplete=autoCompGestionnaireNom]").die().live('change', function () {
        var context = $(this).attr('albcontext');
        var val = $(this).data("selectedItem");
        if (val && this.value != (val.Code + " - " + val.Nom)) {
            $("input[albAutoComplete=autoCompGestionnaireCode]" + selectCtx(context)).val("");

            $(this).val("");
        }
    });
    $("input[albAutoComplete=autoCompGestionnaireNom]").gestionnaireCommonAutoComplete({
        delay: 750,
        source: function (req, rep) {
            $this = $(this.element);
            var context = $this.attr('albcontext');
            $.get("/AutoComplete/GetGestionnairesByName?term=" + encodeURIComponent(req.term)).then(
                function (data) {
                    rep(data);
                }
            );
        },
        change: function () {
            var context = $(this).attr('albcontext');
            if ($("input[albAutoComplete=autoCompGestionnaireSelect]" + selectCtx(context)).val() != "1") {
                $("input[albAutoComplete=autoCompGestionnaireCode]" + selectCtx(context)).val("");
                $("input[albAutoComplete=autoCompGestionnaireNom]" + selectCtx(context)).val("");
            }
            $("input[name=autoCompGestionnaireSelect]" + selectCtx(context)).val("");

        },
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateGestionnaire(ui.item, context);
            $(document).trigger("autoCompGestionnaireSelected", { item: ui.item, target: this });
            SetGestionnaireSelected(context);
            return false;
        }
    });
}
function SetGestionnaireSelected(context) {
    $("input[albAutoComplete=autoCompGestionnaireSelect]" + selectCtx(context)).val("1");
}
//---Met à jour les controles du gestionnaire---
function UpdateGestionnaire(item, context) {
    $("input[albAutoComplete=autoCompGestionnaireCode]" + selectCtx(context)).val(item.Code);
    $("input[albAutoComplete=autoCompGestionnaireNom]" + selectCtx(context)).val(item.Code + " - " + item.Nom);
    $("input[albAutoComplete=autoCompGestionnaireActive]" + selectCtx(context)).val(item.Active);
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Gestionnaire
//#region AutoComplete Apériteur
/***********************************************************/
/***********      AutoComplete Apériteur        ************/
/***********************************************************/

function MapCommonAutoCompAperiteur() {
    //----Charge les infos des apériteurs pour l'autocomplete--
    $.widget("custom.aperiteurAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.Nom == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Nom</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a class='ui-menu-item'><table><tr><td class='AutoCompleteTd'>" + item.Code + "</td><td class='AutoCompleteTd'>" + item.Nom + "</td></tr></table></a>")
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoCommonApeByNom();
    LoadInfoCommonApeByCode();
    LoadInfoCommonApeByCodeNum();
    //--Charge les infos des interlocuteurs pour l'autocomplete--
    $.widget("custom.interlocuteurApeAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            var self = this;
            $.each(items, function (index, item) {
                if (index == 0 && item.NomInterlocuteur == "")
                    ul.append("Aucun Interlocuteur trouvé");
                else {
                    if (index == 0) {
                        ul.append("<li><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>Nom</td></tr></table></li>");
                    }
                    self._renderItem(ul, item);
                }
            });
        },
        _renderItem: function (ul, item) {
            return $("<li></li>")
                .data("item.autocomplete", item)
                .append("<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.NomInterlocuteur + "</td></tr></table></a>")
                .appendTo(ul);
        }
    });
    LoadInfoCommonApeInterlocuteurByNom();
}
//---Charge les infos des apériteurs en fonction du nom---
function LoadInfoCommonApeByNom() {
    $("input[albAutoComplete=autoCompAperiteurNom]").die().live("change", function () {
        var context = $(this).attr('albcontext');
        $("input[albAutoComplete=autoCompAperiteurCode]" + selectCtx(context)).val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompAperiteurNom]").aperiteurAutoComplete({
        delay: 750,
        source: "/AutoComplete/GetAperiteursByName",
        change: function () {
            var context = $(this).attr('albcontext');
            if ($("input[albAutoComplete=autoCompAperiteurSelect]" + selectCtx(context)).val() != "1") {
                $("input[albAutoComplete=autoCompAperiteurCode]" + selectCtx(context)).val("");
                $("input[albAutoComplete=autoCompAperiteurCodeNum]" + selectCtx(context)).val("");
                $("input[albAutoComplete=autoCompAperiteurNom]" + selectCtx(context)).val("");
                $("img[albAutoComplete=autoCompImgAperiteur]" + selectCtx(context)).val("");
            }
            $("input[albAutoComplete=autoCompAperiteurSelect]" + selectCtx(context)).val("");

        },
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateAperiteur(ui.item, context);
            $("input[albAutoComplete=autoCompAperiteurSelect]" + selectCtx(context)).val("1");
            return false;
        }
    });
}
//--Charge les infos des apériteurs en fonction du code---
function LoadInfoCommonApeByCode() {
    $("input[albAutoComplete=autoCompAperiteurCode]").die().live("change", function () {
        var context = $(this).attr('albcontext');
        var codeAssureur = $(this).val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/AutoComplete/GetAperiteursByCode",
            data: { codeString: codeAssureur },
            success: function (json) {
                UpdateAperiteur(json, context);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//--Charge les infos des apériteurs en fonction du code numerique--
function LoadInfoCommonApeByCodeNum() {
    $("input[albAutoComplete=autoCompAperiteurCodeNum]").die().live("change", function () {
        var context = $(this).attr('albcontext');
        var codeAssureur = $(this).val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/AutoComplete/GetAperiteursByCodeNum",
            data: { codeString: codeAssureur },
            success: function (json) {
                UpdateAperiteurNum(json, context);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//---Met à jour les controles de l'apériteur---
function UpdateAperiteur(item, context) {
    $("input[albAutoComplete=autoCompAperiteurCode]" + selectCtx(context)).val(item.Code);
    $("input[albAutoComplete=autoCompAperiteurCodeNum]" + selectCtx(context)).val(item.CodeNum);
    $("input[albAutoComplete=autoCompAperiteurNom]" + selectCtx(context)).val(item.Code + " - " + item.Nom);

    $("img[albAutoComplete=autoCompImgAperiteur]" + selectCtx(context)).attr("albIdInfo", item.Code || "");

    $("input[albAutoComplete=autoCompAperiteurSelect]" + selectCtx(context)).val("1");
    $('#CoAssureurInvalideDiv').empty().removeClass("error").removeClass("warning");
    if (item.Code == '') {
        $('#CoAssureurInvalideDiv').append(" Ce co-assureur est invalide").addClass("error");
        $("input[albAutoComplete=autoCompAperiteurNom]" + selectCtx(context)).val("");
    }

}
//---Met à jour les controles de l'apériteur avec code numerique---
function UpdateAperiteurNum(item, context) {
    $("input[albAutoComplete=autoCompAperiteurCodeNum]" + selectCtx(context)).val(item.CodeNum);
    $("input[albAutoComplete=autoCompAperiteurNom]" + selectCtx(context)).val(item.Code + " - " + item.Nom);

    $("img[albAutoComplete=autoCompImgAperiteur]" + selectCtx(context)).attr("albIdInfo", item.Code || "");

    $("input[albAutoComplete=autoCompAperiteurSelect]" + selectCtx(context)).val("1");
    $('#CoAssureurInvalideDiv').empty().removeClass("error").removeClass("warning");
    if (item.Code == '') {
        $('#CoAssureurInvalideDiv').append(" Ce co-assureur est invalide").addClass("error");
        $("input[albAutoComplete=autoCompAperiteurNom]" + selectCtx(context)).val("");
    }

}
//---Charge les infos des interlocuteurs en fonction du nom---
function LoadInfoCommonApeInterlocuteurByNom() {
    $("input[albAutoComplete=autoCompApeInterlocuteurNom]").die().live("change", function () {
        var context = $(this).attr('albcontext');
        $("input[albAutoComplete=autoCompApeInterlocuteurCode]" + selectCtx(context)).val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompApeInterlocuteurNom]").interlocuteurApeAutoComplete({
        delay: 750,
        source: getInterlocuteurApe,
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            updateInterlocuteur(ui.item, context);
            return false;
        }
    });
}
//----------------------fonction pour obtenir la liste d'interlocuteur
//a partir de son nom et du code du cabinet de courtage----------------------
function getInterlocuteurApe(request, response) {
    var nomInterlocuteurVal = request.term;
    var codeAperiteurVal = $("input[albAutoComplete=autoCompAperiteurCode]").val();
    $.ajax({
        url: "/AutoComplete/GetInterlocuteursAperiteur",
        data: { nomInterlocuteur: nomInterlocuteurVal, codeAperiteur: codeAperiteurVal },
        success: function (json) {
            response(json);
            $('#InterlocuteurInvalideDiv').empty().removeClass("error").removeClass("warning");
            if ((json[0].NomInterlocuteur == "") && ($.trim($("#inInterlocuteur").val()) != "")) {
                $('#InterlocuteurInvalideDiv').append("Cet interlocuteur est invalide").addClass("error");
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------------Met à jour les controles de l'interlocuteur---------------------
function updateInterlocuteur(item, context) {
    $("input[albAutoComplete=autoCompApeInterlocuteurNom]" + selectCtx(context)).val(item.NomInterlocuteur);
    $("input[albAutoComplete=autoCompApeInterlocuteurCode]" + selectCtx(context)).val(item.IdInterlocuteur);

    $('#InterlocuteurInvalideDiv').empty().removeClass("error").removeClass("warning");

}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Apériteur
//#region AutoComplete Organismes Opp
/***********************************************************/
/***********     AutoComplete Organismes Opp      **********/
/***********************************************************/

function MapCommonAutoCompOrganismesOpp() {
    //---Charge les infos des organismes pour l'autocomplete---
    $.widget("custom.organismeCommonAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.Nom == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Nom</th><th class='AutoCompleteTd'>CP</th><th class='AutoCompleteTd'>Ville</th><th class='AutoCompleteTd'>Pays</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a class='ui-menu-item'><table><tr><td class='AutoCompleteTd'>" + item.Code + "</td><td class='AutoCompleteTd'>" + item.Nom + "</td><td class='AutoCompleteTd'>" + item.CP + "</td><td class='AutoCompleteTd'>" + item.Ville + "</td><td class='AutoCompleteTd'>" + item.Pays + "</td></tr></table></a>")
                    .appendTo(ul);
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoCommonOrganismeByNom();
}
//---Charge les infos des organismes en fonction du nom---
function LoadInfoCommonOrganismeByNom() {
    $("input[albAutoComplete=autoCompOrganismeNom]").die().live("change", function () {
        var context = $(this).attr('albcontext');
        $("input[albAutoComplete=autoCompOrganismeCode]" + selectCtx(context)).val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompOrganismeNom]").organismeCommonAutoComplete({
        delay: 750,
        source: getOrganismeName,
        change: function () {
            var context = $(this).attr('albcontext');
            if ($("input[albAutoComplete=autoCompOrganismeSelect]" + selectCtx(context)).val() != "1") {
                _resetValue(context, mappingOrganisme);
            }
            $("input[albAutoComplete=autoCompOrganismeSelect]" + selectCtx(context)).val("");

        },
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateOrganisme(ui.item, context);
            $("input[albAutoComplete=autoCompOrganismeSelect]" + selectCtx(context)).val("1");
            return false;
        }
    });
}
//---Charge les infos de l'organisme en fonction du code---
function LoadInfoCommonByCode() {
    $("input[albAutoComplete=autoCompOrganismeCode]").change(function () {
        var context = $(this).attr('albcontext');
        var typeOppBenef = $("#dvTypeDest").val();
        $.ajax({
            type: "POST",
            url: "/AutoComplete/GetOrganismesByCode",
            data: { term: $("input[albAutoComplete=autoCompOrganismeCode]").val(), typeOppBenef: typeOppBenef },
            success: function (json) {
                $("input[albAutoComplete=autoCompOrganismeSelect]" + selectCtx(context)).val("1");
                UpdateOrganisme(json, context);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//---Met à jour les controles de l'organisme---
function UpdateOrganisme(item, context) {
    _mapValue(item, context, mappingOrganisme);
};

function _resetValue(context) {
    for (var key in mappings) {
        let value = "";
        $("input[albAutoComplete=autoComp" + key + "]" + selectCtx(context)).val('');
    }
}

const mappingOrganisme = {
    "OrganismeCode": function (x) { return x.Code == 0 ? "" : x.Code; },
    "OrganismeNom": "Nom",
    "OrganismeCP": "CP",
    "OrganismeVille": "Ville",
    "OrganismePays": "NomPays",
    "OrganismeAdresse1": "Addresse1",
    "OrganismeAdresse2": "Addresse2"
};

function _mapValue(item, context, mapping) {
    for (var key in mapping) {
        let value = "";
        if (mapping[key] instanceof Function) {
            value = mapping[key](item);
        } else {
            value = item[mapping[key]];
        }
        $("input[albAutoComplete=autoComp" + key + "]" + selectCtx(context)).val(value);
    }
}

function getOrganismeName(request, response) {
    var nomOrganismeVal = request.term;
    var typeopposition = $("#dvTypeDest").val();
    $.ajax({
        url: "/AutoComplete/GetOrganismesByName",
        data: { term: nomOrganismeVal, typeOppBenef: typeopposition },
        success: function (json) {
            response(json);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Organismes Opp
//#region AutoComplete CP/Ville
/***********************************************************/
/***********       AutoComplete CP/Ville        **********/
/***********************************************************/

function MapCommonAutoCompCPVille() {
    //---Charge les infos des courtiers pour l'autocomplete---
    $.widget("custom.cpAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.CodePostal == "" && item.Ville == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteCpTd'>Code Postal</th><th class='AutoCompleteTd'>Ville</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteCpTd'>" + item.CodePostal + "</td><td class='AutoCompleteTd'>" + item.Ville + "</td></tr></table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoVilleByCP();
    //---Charge les infos des interlocuteurs pour l'autocomplete---
    $.widget("custom.villeAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            var self = this;
            $.each(items, function (index, item) {
                if (index == 0 && item.CodePostal == "" && item.Ville == "")
                    ul.append("Aucun résultat");
                else {
                    if (index == 0) {
                        ul.append("<li><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>Ville</td></tr></table></li>");
                    }
                    self._renderItem(ul, item);
                }
            });
        },
        _renderItem: function (ul, item) {
            var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.Ville + "</td></tr></table></a>";

            return $("<li></li>")
                .data("item.autocomplete", item)
                .append(tableau)
                .appendTo(ul);
        }
    });
    LoadInfoVilleByVille();


}

//------Ferme la div de liste des CP/Villes-----------
function CloseListDiv(elem, val) {
    elem.val(val);
    $("#divDataLst").html("");
    $("#divListeCPVille").hide();
}

//----Application de la recherche ajax par code postal---
function LoadInfoVilleByCP() {
    $("input[albAutoComplete=autoCompCodePostal],[albAutoCompleteComplement=autoCompCodePostal]").cpAutoComplete({
        open: function () {
            if ($(this).attr("albNotDeleteAutoComp") == undefined && $(this).attr("albNotDeleteAutoComp") == false) {
                var context = $(this).attr('albcontext');
                $("input[albAutoComplete=autoCompCodePostal]" + selectCtx(context) + ",[albAutoCompleteComplement=autoCompCodePostal]" + selectCtx(context)).val("");
            }
        },
        delay: 750,
        source: "/AutoComplete/GetCodePostal",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateCodePostalVille(ui.item, context);
            return false;
        }
    });
}
//----Application de la recherche ajax par ville --
function LoadInfoVilleByVille() {
    $("input[albAutoComplete=autoCompVille],[albAutoCompleteComplement=autoCompVille]").villeAutoComplete({
        open: function () {
            if ($(this).attr("albNotDeleteAutoComp") == undefined && $(this).attr("albNotDeleteAutoComp") == false) {
                var context = $(this).attr('albcontext');
                $("input[albAutoComplete=autoCompCodePostal]" + selectCtx(context) + ",[albAutoCompleteComplement=autoCompVille]" + selectCtx(context)).val("");
            }
        },
        delay: 750,
        source: "/AutoComplete/GetVille",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateCodePostalVille(ui.item, context);

            var splitCharHtml = $("#splitCharHtml").val();
            $("#ligneId").val($(this).attr("id").split(splitCharHtml)[1]);
            LoadCP(ui.item.Ville, $(this).attr("id"), context);

            return false;
        }
    });
}
//------Mise à jour du courtier-----------
function UpdateCodePostalVille(item, context) {
    $("input[albAutoComplete=autoCompCodePostal]" + selectCtx(context) + ",[albAutoCompleteComplement=autoCompCodePostal]" + selectCtx(context)).val(item.CodePostal);
    $("input[albAutoComplete=autoCompVille]" + selectCtx(context) + ",[albAutoCompleteComplement=autoCompVille]" + selectCtx(context)).val(item.Ville);
}
//-----------Charge les CP selon la ville-----------
function LoadCP(ville, inputCible, context) {
    if (ville == "")
        return false;
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/AutoComplete/GetCPByVille",
        data: { ville: ville },
        success: function (data) {
            var splitCharHtml = $("#splitCharHtml").val();
            if (splitCharHtml == undefined)
                splitCharHtml = "";

            var elem = $("input[id='" + inputCible + "']");
            var top = elem.position().top + 20;
            var left = elem.position().left + 5;

            $("#divDataLst").html(data);

            if ($("#CountListe").val() == undefined || $("#CountListe").val() == "0") {
                common.dialog.error("Aucun code postal correspondant");
                $("input[id='InventaireVille" + splitCharHtml + $("#ligneId").val() + "']").val("");
                return false;
            }

            $("#divDataListeCPVille").css({ 'position': 'absolute', 'top': top + 'px', 'left': left + 'px', 'width': '400px' });
            if (context != undefined && context != "")
                $("#btnValidLst").attr("albcontext", context);
            $("#divListeCPVille").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------Validation des CP---------
function ValidLstCP(elem) {
    if ($("#Listes").val() == "") {
        common.dialog.error("Veuillez choisir une valeur dans la liste");
        return false;
    }
    CloseListDiv(elem, $("#Listes").val());
}
/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete CP/Ville
//#region AutoComplete Concepts
/***********************************************************/
/***********       AutoComplete Concepts        **********/
/***********************************************************/

function MapCommonAutoCompConcepts() {
    //---Charge les infos des concepts pour l'autocomplete---
    $.widget("custom.conceptAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.Libelle == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Description</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.Code + "</td><td class='AutoCompleteTd'>" + item.Libelle + "</td></tr></table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoConcept();
}

//----Application de la recherche ajax par code ou libelle --
function LoadInfoConcept() {
    $("input[albAutoComplete=autoCompCodeConcept]").change(function () {

        $("input[name=codeConceptRech]").val('');
        $("input[name=libConceptRech]").val('');

        var context = $(this).attr('albcontext');
        if ($("input[albAutoComplete=autoCompLibConcept]" + selectCtx(context)).val() == "") {
            $(this).val("");
        }
    });
    $("input[albAutoComplete=autoCompCodeConcept]").conceptAutoComplete({
        open: function () {
            var context = $(this).attr('albcontext');
            $("input[albAutoComplete=autoCompLibConcept]" + selectCtx(context)).val("");
        },
        delay: 750,
        source: "/AutoComplete/GetConceptByCode",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateConcept(ui.item, context);
            return false;
        }
    });
    $("input[albAutoComplete=autoCompLibConcept]").change(function () {
        var context = $(this).attr('albcontext');
        if ($("input[albAutoComplete=autoCompCodeConcept]" + selectCtx(context)).val() == "")
            $(this).val("");

    });
    $("input[albAutoComplete=autoCompLibConcept]").conceptAutoComplete({
        open: function () {
            var context = $(this).attr('albcontext');
            $("input[albAutoComplete=autoCompCodeConcept]" + selectCtx(context)).val("");
        },
        delay: 750,
        source: "/AutoComplete/GetConceptByLib",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateConcept(ui.item, context);
            return false;
        }
    });
}

//------Mise à jour du concept-----------
function UpdateConcept(item, context) {
    $("input[albAutoComplete=autoCompCodeConcept]" + selectCtx(context)).val(item.Code);
    $("input[albAutoComplete=autoCompLibConcept]" + selectCtx(context)).val(item.Libelle);
    $("input[albAutoCompleteConcept=autoCompCodeLibConcept]" + selectCtx(context)).val(item.Code + "-" + item.Libelle);


    $("input[name=codeConceptRech]").val(item.Code);
    $("input[name=libConceptRech]").val(item.Libelle);
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Concepts
//#region AutoComplete Inventaire
/***********************************************************/
/***********     AutoComplete Inventaire          **********/
/***********************************************************/

function MapCommonAutoCompInventaire() {
    //---Charge les infos des concepts pour l'autocomplete---
    $.widget("custom.inventaireAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.Libelle == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Libellé</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.Code + "</td><td class='AutoCompleteTd'>" + item.Libelle + "</td></tr></table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoInventaire();
}
function selectCtx(context) {
    return context ? "[albcontext=" + context + "]" : "";
}
function LoadInfoInventaire() {
    $("input[albAutoComplete=autoCompCodeInventaire]").change(function () {

        $("input[name=codeInventaireRech]").val('');
        $("input[name=libInventaireRech]").val('');

        var context = $(this).attr('albcontext');
        if ($("input[albAutoComplete=autoCompLibInventaire]" + selectCtx(context)).val() == "") {
            $(this).val("");
        }
    });
    $("input[albAutoComplete=autoCompCodeInventaire]").inventaireAutoComplete({
        open: function () {
            var context = $(this).attr('albcontext');
            $("input[albAutoComplete=autoCompLibInventaire]" + selectCtx(context)).val("");
        },
        delay: 750,
        source: "/AutoComplete/GetInventaireByCode",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateInventaire(ui.item, context);
            return false;
        }
    });
}
//------Mise à jour de l'inventaire-----------
function UpdateInventaire(item, context) {
    $("input[albAutoComplete=autoCompCodeInventaire]" + selectCtx(context)).val(item.Code);
    $("input[name=codeInventaireRech]").val(item.Code);
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Inventaire
//#region AutoComplete famille
/***********************************************************/
/***********     AutoComplete famille      **********/
/***********************************************************/

function MapCommonAutoCompFamille() {
    //---Charge les infos des courtiers pour l'autocomplete---
    $.widget("custom.codeFamilleAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.Libelle == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Description</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var imgValidCourtier = "invalide.png";
                if (item.EstValide) {
                    imgValidCourtier = "valide.png";
                }
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.Code + "</td><td class='AutoCompleteTd'>" + item.Libelle + "</td></tr>"
                tableau = tableau + "</table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoCommFamilleByCode();
    LoadInfoCommFamilleByNom();
}
//----Application de la recherche ajax par code de la famille---
function LoadInfoCommFamilleByCode() {
    $("input[albAutoComplete=autoCompCodeFamille]").die().live("change", function () {

        $("input[albAutoComplete=autoCompDescFamille]").val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompCodeFamille]").codeFamilleAutoComplete({
        open: function () {
            $("input[albAutoComplete=autoCompDescFamille]").val('');
        },
        delay: 750,
        source: "/AutoComplete/GetFamillesByCode",
        select: function (event, ui) {
            UpdateCodeFamille(ui.item);
            return false;
        }
    });
}
//----Application de la recherche ajax par nom de la famille---
function LoadInfoCommFamilleByNom() {
    $("input[albAutoComplete=autoCompDescFamille]").die().live("change", function () {

        $("input[albAutoComplete=autoCompCodeFamille]").val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompDescFamille]").codeFamilleAutoComplete({
        open: function () {

            $("input[albAutoComplete=autoCompCodeFamille]").val('');
        },
        delay: 750,
        source: "/AutoComplete/GetFamillesByName",
        select: function (event, ui) {
            UpdateCodeFamille(ui.item);
            return false;
        }
    });
}

function UpdateCodeFamille(item) {
    $("input[albAutoComplete=autoCompDescFamille]").val(item.Libelle);
    $("input[albAutoComplete=autoCompCodeFamille]").val(item.Code);
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete famille
//#region AutoComplete filtre valeur
/***********************************************************/
/***********     AutoComplete filtre valeur      **********/
/***********************************************************/
function MapCommonAutoCompFiltres() {
    //---Charge les infos des concepts pour l'autocomplete---
    $.widget("custom.filtreAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.DescriptionFiltre == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Description</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.CodeFiltre + "</td><td class='AutoCompleteTd'>" + item.DescriptionFiltre + "</td></tr></table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoFiltreByCode();
    LoadInfoFiltreByName();
}

//----Application de la recherche ajax par code du filtre---
function LoadInfoFiltreByCode() {
    $("input[albAutoComplete=autoCompCodeFiltre]").die().live("change", function () {
        $("input[albAutoComplete=autoCompLibFiltre]").val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompCodeFiltre]").filtreAutoComplete({
        open: function () {
            $("input[albAutoComplete=autoCompLibFiltre]").val('');
        },
        delay: 750,
        source: "/AutoComplete/GetFiltresByCode",
        select: function (event, ui) {
            UpdateFiltre(ui.item);
            return false;
        }
    });
}

//----Application de la recherche ajax par nom du filtre---
function LoadInfoFiltreByName() {
    $("input[albAutoComplete=autoCompLibFiltre]").die().live("change", function () {
        $("input[albAutoComplete=autoCompCodeFiltre]").val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompLibFiltre]").filtreAutoComplete({
        open: function () {
            $("input[albAutoComplete=autoCompCodeFiltre]").val('');
        },
        delay: 750,
        source: "/AutoComplete/GetFiltresByName",
        select: function (event, ui) {
            UpdateFiltre(ui.item);
            return false;
        }
    });
}

//------Mise à jour du filtre-----------
function UpdateFiltre(item, context) {
    $("input[albAutoComplete=autoCompCodeFiltre]" + selectCtx(context)).val(item.CodeFiltre);
    $("input[albAutoComplete=autoCompLibFiltre]" + selectCtx(context)).val(item.DescriptionFiltre);
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete filtre valeur
//#region AutoComplete Garanties
/***********************************************************/
/***********       AutoComplete Garanties        **********/
/***********************************************************/

function MapCommonAutoCompGaranties() {
    //---Charge les infos des garanties pour l'autocomplete---
    $.widget("custom.garantieAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.DesignationGarantie == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Désignation</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.CodeGarantie + "</td><td class='AutoCompleteTd'>" + item.DesignationGarantie + "</td></tr></table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoGarantie();
}

//----Application de la recherche ajax par code ou libelle --
function LoadInfoGarantie() {
    $("input[albAutoComplete=autoCompCodeGarantie]").change(function () {

        var context = $(this).attr('albcontext');
        if ($("input[albAutoComplete=autoCompDesignationGarantie]" + selectCtx(context)).val() == "")
            $(this).val("");
    });
    $("input[albAutoComplete=autoCompCodeGarantie]").garantieAutoComplete({
        open: function () {
            var context = $(this).attr('albcontext');
            $("input[albAutoComplete=autoCompDesignationGarantie]" + selectCtx(context)).val("");

        },
        delay: 750,
        source: "/AutoComplete/GetGarantieByCode",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateGarantie(ui.item, context);
            return false;
        }
    });
    $("input[albAutoComplete=autoCompDesignationGarantie]").change(function () {
        var context = $(this).attr('albcontext');
        if ($("input[albAutoComplete=autoCompCodeGarantie]" + selectCtx(context)).val() == "")
            $(this).val("");

    });
    $("input[albAutoComplete=autoCompDesignationGarantie]").garantieAutoComplete({
        open: function () {
            var context = $(this).attr('albcontext');
            $("input[albAutoComplete=autoCompCodeGarantie]" + selectCtx(context)).val("");
        },
        delay: 750,
        source: "/AutoComplete/GetGarantieByDescription",
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateGarantie(ui.item, context);
            return false;
        }
    });
}

//------Mise à jour de la garantie-----------
function UpdateGarantie(item, context) {
    $("input[albAutoComplete=autoCompCodeGarantie]" + selectCtx(context)).val(item.CodeGarantie);
    $("input[albAutoComplete=autoCompDesignationGarantie]" + selectCtx(context)).val(item.DesignationGarantie);
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Garanties
//#region AutoComplete param IS referentiels
/***********************************************************/
/***********  AutoComplete param IS referentiels  **********/
/***********************************************************/
function MapCommonAutoCompReferentielIS() {
    //---Charge les infos des concepts pour l'autocomplete---
    $.widget("custom.referentielISAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.Code == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteLargeTd'>Referentiels</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteLargeTd'>" + item.Code + "</td></tr></table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoReferentielISByName();
}

//----Application de la recherche ajax par nom du referentiel IS---
function LoadInfoReferentielISByName() {
    $("input[albAutoComplete=autoCompReferentielIS]").referentielISAutoComplete({
        change: function () {
            var context = $(this).attr('albcontext');
        },
        delay: 750,
        source: "/AutoComplete/GetReferentielISByName",
        select: function (event, ui) {
            UpdateRefentielIS(ui.item);
            return false;
        }
    });
}

//------Mise à jour du type valeur-----------
function UpdateRefentielIS(item, context) {
    $("input[albAutoComplete=autoCompReferentielIS]" + selectCtx(context)).val(item.Code);
    $("input[albAutoComplete=autoCompReferentielISCode]" + selectCtx(context)).val(item.Code);
    LoadReferentiel($("#selectedRow").val(), item.Code);
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete param IS referentiels
//#region AutoComplete type valeur
/***********************************************************/
/***********     AutoComplete type valeur         **********/
/***********************************************************/
function MapCommonAutoCompTypeValeur() {
    //---Charge les infos des concepts pour l'autocomplete---
    $.widget("custom.typeValeurAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {

                    if (index == 0 && item.Description == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Description</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.Code + "</td><td class='AutoCompleteTd'>" + item.Description + "</td></tr></table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoTypeValeurByCode();
    LoadInfoTypeValeurByName();
}

//----Application de la recherche ajax par code du type valeur---
function LoadInfoTypeValeurByCode() {
    $("input[albAutoComplete=autoCompCodeTypeValeur]").die().live("change", function () {
        $("input[albAutoComplete=autoCompLibTypeValeur]").val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompCodeTypeValeur]").typeValeurAutoComplete({
        open: function () {
            $("input[albAutoComplete=autoCompLibTypeValeur]").val('');
        },
        delay: 750,
        source: "/AutoComplete/GetTypeValeurByCode",
        select: function (event, ui) {
            UpdateTypeValeur(ui.item);
            return false;
        }
    });
}

//----Application de la recherche ajax par nom du type valeur---
function LoadInfoTypeValeurByName() {
    $("input[albAutoComplete=autoCompLibTypeValeur]").die().live("change", function () {
        $("input[albAutoComplete=autoCompCodeTypeValeur]").val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompLibTypeValeur]").typeValeurAutoComplete({
        open: function () {
            $("input[albAutoComplete=autoCompCodeTypeValeur]").val('');
        },
        delay: 750,
        source: "/AutoComplete/GetTypeValeurByName",
        select: function (event, ui) {
            UpdateTypeValeur(ui.item);
            return false;
        }
    });
}

//------Mise à jour du type valeur-----------
function UpdateTypeValeur(item, context) {
    $("input[albAutoComplete=autoCompCodeTypeValeur]" + selectCtx(context)).val(item.Code);
    $("input[albAutoComplete=autoCompLibTypeValeur]" + selectCtx(context)).val(item.Description);
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete type valeur
//#region AutoComplete Templates
/***********************************************************/
/***********     AutoComplete Templates         **********/
/***********************************************************/
function MapCommonAutoCompTemplates() {
    //---Charge les infos des concepts pour l'autocomplete---
    $.widget("custom.templateAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {

                    if (index == 0 && item.DescriptionTemplate == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Description</th><th class='AutoCompleteTd'>Type</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteTd'>" + item.CodeTemplate + "</td><td class='AutoCompleteTd'>" + item.DescriptionTemplate + "</td><td class='AutoCompleteTd'>" + item.TypeTemplate + "</td></tr></table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoTemplateByCode();
    LoadInfoTemplateByName();
    LoadInfoTemplateByCodeCible();
    LoadInfoTemplateByNameCible();
    LoadInfoTemplateByCodeCNVA();
}

//-----Application de la recherche ajax par code du template sans le CNVA---
function LoadInfoTemplateByCodeCNVA() {
    $("input[albAutoComplete=autoCompCodeTemplateCNVA]").die().live("change", function () {
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompCodeTemplateCNVA]").templateAutoComplete({
        delay: 750,
        source: function (request, response) {
            GetTemplateCNVAByType(request, response);
        },
        select: function (event, ui) {
            UpdateTemplateCNVA(ui.item);
            return false;
        }
    });
}

function GetTemplateCNVAByType(request, response) {
    var type = $("input[albAutoComplete=autoCompTypeTemplateCNVA]").val();
    $.ajax({
        type: "POST",
        url: "/AutoComplete/GetTemplateByCodeCNVA",
        data: { term: request.term, type: type },
        success: function (data) {
            response($.map(data, function (item) {
                return { GuidId: item.GuidId, CodeTemplate: item.CodeTemplate, DescriptionTemplate: item.DescriptionTemplate, TypeTemplate: item.TypeTemplate };
            }));
        }
    });
}

function UpdateTemplateCNVA(item, context) {
    $("input[albAutoComplete=autoCompCodeTemplate]" + selectCtx(context)).val("CV" + item.CodeTemplate);
    $("input[albAutoComplete=autoCompCodeTemplateCNVA]" + selectCtx(context)).val(item.CodeTemplate);

}

//----Application de la recherche ajax par code du template---
function LoadInfoTemplateByCode() {
    $("input[albAutoComplete=autoCompCodeTemplate]").die().live("change", function () {
        $("input[albAutoComplete=autoCompLibTemplate]").val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompCodeTemplate]").templateAutoComplete({
        open: function () {
            $("input[albAutoComplete=autoCompIdTemplate]").val('');
            $("input[albAutoComplete=autoCompLibTemplate]").val('');
        },
        delay: 750,
        source: "/AutoComplete/GetTemplateByCode",
        select: function (event, ui) {
            UpdateTemplate(ui.item);
            return false;
        }
    });
}

//----Application de la recherche ajax par nom du template---
function LoadInfoTemplateByName() {
    $("input[albAutoComplete=autoCompLibTemplate]").die().live("change", function () {
        $("input[albAutoComplete=autoCompCodeTemplate]").val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompLibTemplate]").templateAutoComplete({
        open: function () {
            $("input[albAutoComplete=autoCompIdTemplate]").val('');
            $("input[albAutoComplete=autoCompCodeTemplate]").val('');
        },
        delay: 750,
        source: "/AutoComplete/GetTemplateByName",
        select: function (event, ui) {
            UpdateTemplate(ui.item);
            return false;
        }

    });
}

//------Mise à jour du type valeur-----------
function UpdateTemplate(item, context) {
    $("input[albAutoComplete=autoCompIdTemplate]" + selectCtx(context)).val(item.GuidId);
    $("input[albAutoComplete=autoCompCodeTemplate]" + selectCtx(context)).val(item.CodeTemplate);
    $("input[albAutoComplete=autoCompLibTemplate]" + selectCtx(context)).val(item.DescriptionTemplate);
    $("select[albAutoComplete=autoCompTypeTemplate]" + selectCtx(context)).val(item.TypeTemplate);


}

//---------Application de la recherche ajax par code du template en prenant uniquement les templates non liés------------
function LoadInfoTemplateByCodeCible() {
    $("input[albAutoComplete=autoCompCodeTemplateCible]").die().live("change", function () {
        $("input[albAutoComplete=autoCompLibTemplateCible]").val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompCodeTemplateCible]").templateAutoComplete({
        open: function () {
            $("input[albAutoComplete=autoCompIdTemplateCible]").val('');
            $("input[albAutoComplete=autoCompLibTemplateCible]").val('');
        },
        delay: 750,
        source: function (request, response) {
            GetTemplateCibleByCode(request, response);
        },
        select: function (event, ui) {
            UpdateTemplateCible(ui.item);
            return false;
        }
    });
}

//----Application de la recherche ajax par nom du template en prenant uniquement les templates non liés---
function LoadInfoTemplateByNameCible() {
    $("input[albAutoComplete=autoCompLibTemplateCible]").die().live("change", function () {
        $("input[albAutoComplete=autoCompCodeTemplateCible]").val("");
        $(this).val("");
    });
    $("input[albAutoComplete=autoCompLibTemplateCible]").templateAutoComplete({
        open: function () {
            $("input[albAutoComplete=autoCompIdTemplateCible]").val('');
            $("input[albAutoComplete=autoCompCodeTemplateCible]").val('');
        },
        delay: 750,
        source: function (request, response) {
            GetTemplateCibleByName(request, response);
        },
        select: function (event, ui) {
            UpdateTemplateCible(ui.item);
            return false;
        }

    });
}

function GetTemplateCibleByCode(request, response) {
    var cible = 0;
    $.ajax({
        type: "POST",
        url: "/AutoComplete/GetTemplateByCodeCible",
        data: { term: request.term, cible: cible },
        success: function (data) {
            response($.map(data, function (item) {
                return { GuidId: item.GuidId, CodeTemplate: item.CodeTemplate, DescriptionTemplate: item.DescriptionTemplate, TypeTemplate: item.TypeTemplate };
            }));
        }
    });
}


//-----
function GetTemplateCibleByName(request, response) {
    var cible = 0;
    $.ajax({
        type: "POST",
        url: "/AutoComplete/GetTemplateByNameCible",
        data: { term: request.term, cible: cible },
        success: function (data) {
            response($.map(data, function (item) {
                return { GuidId: item.GuidId, CodeTemplate: item.CodeTemplate, DescriptionTemplate: item.DescriptionTemplate, TypeTemplate: item.TypeTemplate };
            }));
        }
    });

}


//------Mise à jour du type valeur-----------
function UpdateTemplateCible(item, context) {
    $("input[albAutoComplete=autoCompIdTemplateCible]" + selectCtx(context)).val(item.GuidId);
    $("input[albAutoComplete=autoCompCodeTemplateCible]" + selectCtx(context)).val(item.CodeTemplate);
    $("input[albAutoComplete=autoCompLibTemplateCible]" + selectCtx(context)).val(item.DescriptionTemplate);
    $("select[albAutoComplete=autoCompTypeTemplate]" + selectCtx(context)).val(item.TypeTemplate);
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Templates
//#region AutoComplete Action Menu
/***********************************************************/
/***********     AutoComplete Action Menu         **********/
/***********************************************************/

function MapCommonAutoCompActionMenu() {
    //---Charge les infos des concepts pour l'autocomplete---
    $.widget("custom.actionAutocomplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.text == "")
                        ul.append("Aucun résultat");
                    else {
                        //if (index == 0) {
                        //    ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Description</th><th class='AutoCompleteTd'>Type</th></tr></table></li>");
                        //}
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteActionMenuTd backgroundImage " + (item.icon == "" ? "icon-defaultIcon" : "icon-" + item.icon) + "'>" + item.text + "</td></tr></table></a>";
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });

    $("input[albAutoComplete=actionAutocomplete]").die().live("change", function () {
        $("button[albautocomplete=actionAutocompleteValidation]").attr("disabled", "disabled");
        $("input[albautocomplete=actionAutocompleteCode]").val('');
    });

    LoadMenuActionByName();
}

function LoadMenuActionByName() {
    $("input[albAutoComplete=actionAutocomplete]").actionAutocomplete({
        open: function () {
            $("input[albautocomplete=actionAutocompleteCode]").val('');
            $("button[albautocomplete=actionAutocompleteValidation]").attr("disabled", "disabled");
        },
        delay: 750,
        source: function (request, response) {
            GetMenuActionByName(request, response);
        },
        select: function (event, ui) {
            UpdateMenuAction(ui.item);
            return false;
        }
    });
}

function GetMenuActionByName(request, response) {
    var selectRadio = $('input[type=radio][name=RadioRow]:checked');
    var infoRows = $(selectRadio).val();
    var offreType = "";
    var tr = "";
    var offreEtat = "";
    var offreSituation = "";
    var offrePeriodicite = "";
    var offreCopyOffre = "";
    var offreBranche = "";
    if (infoRows != undefined && infoRows != "") {
        infoRows = $(selectRadio).val();
        offreType = infoRows.split("_")[2];
        tr = $(selectRadio).parent().parent();
        offreEtat = $(tr).children(".tdEtat").html();
        offreSituation = $(tr).children(".tdSituation").html();
        offrePeriodicite = $("input[name=Periodicite][albContext=" + infoRows + "]").val();
        offreCopyOffre = $("#btnCopyOffre").is(':visible') && offreType == "O" ? "OK" : "KO";
        offreBranche = $(tr).children(".tdBranche").html();
    }
    $.ajax({
        type: "POST",
        url: "/AutoComplete/GetMenuActionByName",
        data: { term: request.term, type: offreType, etat: offreEtat, situation: offreSituation, periodicite: offrePeriodicite, branche: offreBranche, copyOffre: offreCopyOffre, modeNavig: $("#ModeNavig").val() },
        success: function (data) {
            response($.map(data, function (item) {
                return { alias: item.alias, text: item.text, icon: item.icon, utilisateur: item.utilisateur };
            }));
        }
    });
}

function UpdateMenuAction(item, context) {
    $("input[albAutoComplete=actionAutocomplete]" + selectCtx(context)).val(item.text);
    $("input[albAutoComplete=actionAutocompleteCode]" + selectCtx(context)).val(item.alias);
    $("button[albautocomplete=actionAutocompleteValidation]" + selectCtx(context)).removeAttr("disabled");
}

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Action Menu
//#region AutoComplete Intervenants
/***********************************************************/
/***********  AutoComplete Intervenants           **********/
/***********************************************************/
function MapCommonAutoCompIntervenants() {
    //---Charge les infos des concepts pour l'autocomplete---
    $.widget("custom.intervenantAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.CodeIntervenant == -1)
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteLargeTd'>Dénomination</th><th class='AutoCompleteLargeTd'>Ville</th><th class='AutoCompleteLargeTd'>Fin de validité</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteLargeTd'>" + item.Denomination + "</td><td class='AutoCompleteLargeTd'>" + item.CodePostal + " - " + item.Ville + "</td><td class='AutoCompleteLargeTd'>" + (item.AnneeFinValidite > 0 ? item.JourFinValidite + "/" + item.MoisFinValidite + "/" + item.AnneeFinValidite : "") + "</td></tr></table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoIntervenantByName();
    LoadInfoIntervenantByCode();
}

function MapCommonAutoCompInterlocuteursIntervenant() {
    //---Charge les infos des concepts pour l'autocomplete---
    $.widget("custom.interlocuteursIntervenantAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.CodeIntervenant == -1)
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table class='AutoCompleteTable'><tr><th class='AutoCompleteLargeTd'>Dénomination</th><th class='AutoCompleteLargeTd'>Ville</th><th class='AutoCompleteLargeTd'>Fin de validité</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                var tableau = "<a class='ui-menu-item'><table class='AutoCompleteTable'><tr><td class='AutoCompleteLargeTd'>" + item.Denomination + "</td><td class='AutoCompleteLargeTd'>" + item.CodePostal + " - " + item.Ville + "</td><td class='AutoCompleteLargeTd'>" + (item.AnneeFinValidite > 0 ? item.JourFinValidite + "/" + item.MoisFinValidite + "/" + item.AnneeFinValidite : "") + "</td></tr></table></a>";

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append(tableau)
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoInterlocuteursIntervenantByName();
}

//----Application de la recherche ajax par nom de l'intervenant---
function LoadInfoIntervenantByName() {
    $("input[albAutoComplete=autoCompIntervenant]").intervenantAutoComplete({
        change: function () {
            var context = $(this).attr('albcontext');
        },
        delay: 750,
        source: function (request, response) {
            GetIntervenantExcluant(request, response);
        },
        select: function (event, ui) {
            UpdateIntervenant(ui.item, $(this).attr("albcontext"));
            return false;
        }
    });
}

function LoadInfoIntervenantByCode() {
    $("input[albAutoComplete=autoCompIntervenantCode]").die().live("change", function () {
        var codeDossier = $("#Offre_CodeOffre").val();
        var versionDossier = $("#Offre_Version").val();
        var typeDossier = $("#Offre_Type").val();
        var codeIntervenant = $(this).val();
        var context = $(this).attr('albcontext');
        var fromAffaireOnly = $("input[albAutoComplete=autoCompIntervenantIsFromAffaire]").val();
        if (fromAffaireOnly == undefined)
            fromAffaireOnly = "";
        if (codeIntervenant != "") {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/AutoComplete/GetIntervenantByCode",
                data: { codeString: codeIntervenant, fromAffaireOnly: fromAffaireOnly, codeOffre: codeDossier, type: typeDossier, version: versionDossier },
                success: function (json) {
                    if (json != null && json.CodeIntervenant != "") {
                        UpdateIntervenant(json, context);
                    }
                    else {
                        $("input[albAutoComplete=autoCompIntervenantCode]" + selectCtx(context)).val("");
                        $("input[albAutoComplete=autoCompIntervenant]" + selectCtx(context)).val("");
                    }
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            $("input[albAutoComplete=autoCompIntervenant]" + selectCtx(context)).val("");
            $("input[albAutoComplete=autoCompIntervenantCode]" + selectCtx(context)).val("");
            $("input[albAutoComplete=autoCompIntervenantType]" + selectCtx(context)).val("");

        }
    });
}

function LoadInfoInterlocuteursIntervenantByName() {
    $("input[albAutoComplete=autoCompInterlocuteurIntervenant]").interlocuteursIntervenantAutoComplete({
        change: function () {
            var context = $(this).attr('albcontext');
            $("#currentAutocompleteIntervenantContext").val(context || '');
            if ($.trim($(this).val()) == undefined || $.trim($(this).val()) == "") {
                $("input[albAutoComplete=autoCompInterlocuteurIntervenant]" + selectCtx(context)).val("");
                $("input[albAutoComplete=autoCompInterlocuteurIntervenantCode]" + selectCtx(context)).val("");
            }


        },
        delay: 750,
        source: function (request, response) {
            GetInterlocuteursIntervenant(request, response);
        },
        select: function (event, ui) {
            UpdateInterlocuteurIntervenant(ui.item);
            return false;
        }
    });
}

//------Mise à jour du type valeur-----------
function UpdateIntervenant(item, context) {
    $("input[albAutoComplete=autoCompIntervenant]" + selectCtx(context)).val(item.Denomination);
    $("input[albAutoComplete=autoCompIntervenantCode]" + selectCtx(context)).val(item.CodeIntervenant);
    $("input[albAutoComplete=autoCompIntervenantType]" + selectCtx(context)).val(item.TypeIntervenant);

    $("input[albAutoComplete=autoCompIntervenantFinValidite]" + selectCtx(context)).val((item.AnneeFinValidite > 0 ? item.JourFinValidite + "/" + item.MoisFinValidite + "/" + item.AnneeFinValidite : ""));
    $("input[albAutoComplete=autoCompIntervenantAdresse1]" + selectCtx(context)).val(item.Adresse1);
    $("input[albAutoComplete=autoCompIntervenantAdresse2]" + selectCtx(context)).val(item.Adresse2);
    $("input[albAutoComplete=autoCompIntervenantCodePostal]" + selectCtx(context)).val(item.CodePostal);
    $("input[albAutoComplete=autoCompIntervenantVille]" + selectCtx(context)).val(item.Ville);
    $("input[albAutoComplete=autoCompIntervenantTelephone]" + selectCtx(context)).val(item.Telephone);
    $("input[albAutoComplete=autoCompIntervenantEmail]" + selectCtx(context)).val(item.Email);

    if (item.AnneeFinValidite > 0) {
        $("input[albAutoComplete=autoCompIntervenant]" + selectCtx(context)).addClass("textRed");
        $("#btnValiderDetailsIntervenants").disable();
    }
    else {
        $("input[albAutoComplete=autoCompIntervenant]" + selectCtx(context)).removeClass("textRed");
        $("#btnValiderDetailsIntervenants").enable();
    }
    MapCommonAutoCompInterlocuteursIntervenant();
}

function UpdateInterlocuteurIntervenant(item, context) {
    $("input[albAutoComplete=autoCompInterlocuteurIntervenant]" + selectCtx(context)).val(item.Denomination);
    $("input[albAutoComplete=autoCompInterlocuteurIntervenantCode]" + selectCtx(context)).val(item.CodeIntervenant);
}

function GetIntervenantExcluant(request, response) {
    var codeDossier = $("#Offre_CodeOffre").val();
    var versionDossier = $("#Offre_Version").val();
    var typeDossier = $("#Offre_Type").val();
    var typeIntervenant = $("select[albAutoComplete=autoCompIntervenantType]").val();
    var fromAffaireOnly = $("input[albAutoComplete=autoCompIntervenantIsFromAffaire]").val();
    if (typeIntervenant == undefined)
        typeIntervenant = "";
    if (fromAffaireOnly == undefined)
        fromAffaireOnly = "";
    $.ajax({
        type: "POST",
        url: "/AutoComplete/GetIntervenantByName",
        data: { term: request.term, codeDossier: codeDossier, typeDossier: typeDossier, versionDossier: versionDossier, typeIntervenant: typeIntervenant, fromAffaireOnly: fromAffaireOnly },
        success: function (data) {
            response($.map(data, function (item) {
                return { CodeIntervenant: item.CodeIntervenant, Denomination: item.Denomination, CodePostal: item.CodePostal, Ville: item.Ville, AnneeFinValidite: item.AnneeFinValidite, MoisFinValidite: item.MoisFinValidite, JourFinValidite: item.JourFinValidite, Adresse1: item.Adresse1, Adresse2: item.Adresse2, Telephone: item.Telephone, Email: item.Email, TypeIntervenant: item.Type };
            }));
        }
    });
}

function GetInterlocuteursIntervenant(request, response) {
    var context = $("#currentAutocompleteIntervenantContext").val();
    var codeIntervenant = "";
    codeIntervenant = $("input[albAutoComplete=autoCompIntervenantCode]" + selectCtx(context)).val();

    if (codeIntervenant != undefined && codeIntervenant != "") {
        $.ajax({
            type: "POST",
            url: "/AutoComplete/GetInterlocuteursByIntervenantByName",
            data: { term: request.term, codeIntervenant: codeIntervenant },
            success: function (data) {
                response($.map(data, function (item) {
                    return { CodeIntervenant: item.CodeIntervenant, Denomination: item.Denomination, CodePostal: item.CodePostal, Ville: item.Ville, AnneeFinValidite: item.AnneeFinValidite, MoisFinValidite: item.MoisFinValidite, JourFinValidite: item.JourFinValidite };
                }));
            }
        });
    }
}
/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Intervenants
//#region AutoComplete Intervenants
/***********************************************************/
/***********  AutoComplete Intervenants           **********/
/***********************************************************/
function MapCommonAutoCompUtilisateur() {
    //---Charge les infos des souscripteurs pour l'autocomplete---
    $.widget("custom.userCommonAutoComplete", $.ui.autocomplete, {
        options: { minLength: MIN_AUTO_COMPLETE_LENGTH },
        _renderMenu: function (ul, items) {
            try {
                var self = this;
                $.each(items, function (index, item) {
                    if (index == 0 && item.Nom == "")
                        ul.append("Aucun résultat");
                    else {
                        if (index == 0) {
                            ul.append("<li><table><tr><th class='AutoCompleteTd'>Code</th><th class='AutoCompleteTd'>Nom</th><th class='AutoCompleteTd'>Prénom</th></tr></table></li>");
                        }
                        self._renderItem(ul, item);
                    }
                });
            }
            catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        },
        _renderItem: function (ul, item) {
            try {
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a class='ui-menu-item'><table><tr><td class='AutoCompleteTd'>" + item.Code + "</td><td class='AutoCompleteTd'>" + item.Nom + "</td><td class='AutoCompleteTd'>" + item.Prenom + "</td></tr></table></a>")
                    .appendTo(ul);
            } catch (e) {
                $.fn.jqDialogErreurOpen(e);
            }
        }
    });
    LoadInfoCommonUserByNom();
}
//---Charge les infos des utilisateurs---
function LoadInfoCommonUserByNom() {
    $("input[albAutoComplete=autoCompUserNom]").die().live('change', function () {
        var context = $(this).attr('albcontext');
    });
    $("input[albAutoComplete=autoCompUserNom]").userCommonAutoComplete({
        delay: 750,
        source: "/AutoComplete/GetUserByName",
        change: function () {
            var context = $(this).attr('albcontext');

            if ($("input[albAutoComplete=autoCompUserSelect]" + selectCtx(context)).val() != "1") {
                $("input[albAutoComplete=autoCompUserNom]" + selectCtx(context)).val("");
            }
            $("input[name=autoCompUserSelect]" + selectCtx(context)).val("");

        },
        select: function (event, ui) {
            var context = $(this).attr('albcontext');
            UpdateUser(ui.item, context);
            $("input[albAutoComplete=autoCompUserSelect]" + selectCtx(context)).val("1");
            return false;
        }
    });
}
//---Met à jour les controles du souscripteur---
function UpdateUser(item, context) {
    $("input[albAutoComplete=autoCompUserNom]" + selectCtx(context)).val(item.Code);
};

/***********************************************************/
/***********************************************************/
/***********************************************************/
//#endregion AutoComplete Intervenants


//#region AutoComplete Activite
/***********************************************************/
/***********        AutoComplete Activite         ************/
/***********************************************************/
