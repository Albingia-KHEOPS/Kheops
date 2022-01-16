$(document).ready(function () {
    MapElementPage();
});
//---------------Map les éléments de la page--------------
function MapElementPage() {
    if ($("#Service").val() != "") {
        $("#Service").trigger('change');
    }
    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "DeleteParam":
                DeleteParam($("#hiddenInputId").val());
                $("#hiddenInputId").val('');
                break;
            case "SupprimerValeur":
                var valeur = $("#idValeurSuppr").val();
                SupprimerValeur(valeur);
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });
    $("#Service").die().live('change', function () {
        AffectTitleList($(this));
        var val = $(this).val();
        if (val != "") {
            $("#CodeService").val($(this).val());
            LoadActeGestion($(this).val());
        }
        else {
            $("#CodeService").val("");
            ClearActeGestion();
        }
    });
    $("#addActeGestion").die().live('click', function () {
        if ($(this).hasClass("CursorPointer")) {
            AddParam("ActeGestion", $("#Service").val());
        }
    });
    $("#btnCancel").die().live('click', function () {
        Redirection("BackOffice", "Index");
    });
    $("#AjoutCTX").die().live('click', function () { CheckConceptFamille("CTX"); });
    $("#AjoutOMP").die().live('click', function () { CheckConceptFamille("EMP"); });
}
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamClause/Redirection/",
        data: { cible: cible, job: job },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Redirection Param-------------------
function RedirectionParam(etape) {
    var codeService = $("#CodeService").val();
    var codeActeGestion = $("#CodeActeGestion").val();
    var codeEtape = $("#CodeEtape").val();
    var codeContexte = $("#CodeContexte").val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamClause/RedirectionParam/",
        data: { etape: etape, codeService: codeService, codeActeGestion: codeActeGestion, codeEtape: codeEtape, codeContexte: codeContexte },
        success: function (data) {
            $("#divBodyPage").html(data);
            MapElementPage();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------Ajoute un acte de gestion-----------------
function AddParam(etape, codeService, codeActGes, codeEtape, codeContexte, codeEGDI) {
    $.ajax({
        type: "POST",
        url: "/ParamClause/AjoutParam",
        data: { etape: etape, codeService: codeService, codeActGes: codeActGes, codeEtape: codeEtape, codeContexte: codeContexte, codeEGDI: codeEGDI },
        success: function (data) {
            $("#divDataParam").html(data);
            $("#ajoutParam").show();
            MapAddParam();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Charge les actes de gestion pour un service-------------
function LoadActeGestion(service) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamClause/LoadListActeGestion/",
        data: { codeService: service },
        success: function (data) {
            $("#divBodyParam").removeClass('AlignCenter');
            $("#divBodyParam").html(data);
            MapElementActeGestion();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Vide la liste des actes de gestion----------------
function ClearActeGestion() {
    $("#divBodyParam").addClass('AlignCenter');
    $("#divBodyParam").html("Veuillez choisir un service.");
    $("#addActeGestion").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
}
//------------Map les éléments de la liste des actes de gestion--------
function MapElementActeGestion() {
    AlternanceLigne("BodyParam", "", false, null);
    $("#btnCancel").die().live('click', function () {
        Redirection("BackOffice", "Index");
    });
    $("#addActeGestion").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");

    $("a[name=openActeGestion]").click(function () {
        $("#CodeService").val($("#Service").val());
        $("#CodeActGes").val($(this).attr("id").split("_")[1]);
        OpenParam("ActeGestion");
    });

    $("img[name=delActeGestion]").click(function () {
        $("#hiddenInputId").val($(this).attr('id'));
        ShowCommonFancy("Confirm", "DeleteParam", "Veuillez confirmer la suppression du paramètre", 350, 130, true, true);
    });
}
//----------Ouvre une fenêtre de param--------------
function OpenParam(etape) {
    ShowLoading();

    var codeService = $("#CodeService").val();
    var codeActGes = $("#CodeActGes").val();
    var codeEtape = $("#CodeEtape").val();
    var codeContexte = $("#CodeContexte").val();

    $.ajax({
        type: "POST",
        url: "/ParamClause/OpenParam/",
        data: { etape: etape, codeService: codeService, codeActGes: codeActGes, codeEtape: codeEtape, codeContexte: codeContexte },
        success: function (data) {
            $("#divBodyPage").html(data);
            switch (etape) {
                case "ActeGestion":
                    MapElementEtape();
                    break;
                case "Etape":
                    MapElementContexte();
                    break;
                case "Contexte":
                    MapElementEGDI();
                    break;
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Supprime un paramètre------------------
function DeleteParam(elemId) {
    ShowLoading();
    var etape = elemId.split("_")[0].replace("del", "");
    var codeParam = elemId.split("_")[1];
    $.ajax({
        type: "POST",
        url: "/ParamClause/DeleteParam/",
        data: { etape: etape, codeParam: codeParam },
        success: function (data) {
            switch (etape) {
                case "ActeGestion":
                    var codeService = $("#Service").val();
                    LoadActeGestion(codeService);
                    break;
                case "Etape":
                    OpenParam("ActeGestion");
                    break;
                case "Contexte":
                    OpenParam("Etape");
                    break;
                case "EGDI":
                    OpenParam("Contexte");
                    break;
                default:
                    break;
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Map les éléments de la liste des étapes------------
function MapElementEtape() {
    AlternanceLigne("BodyParam", "", false, null);
    $("#addEtape").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");

    $("#addEtape").die().live('click', function () {
        if ($(this).hasClass("CursorPointer")) {
            AddParam("Etape", $("#CodeService").val(), $("#CodeActGes").val());
        }
    });

    $("a[name=openEtape]").click(function () {
        $("#CodeEtape").val($(this).attr("id").split("_")[1]);
        OpenParam("Etape");
    });

    $("img[name=delEtape]").click(function () {
        $("#hiddenInputId").val($(this).attr('id'));
        ShowCommonFancy("Confirm", "DeleteParam", "Veuillez confirmer la suppression du paramètre", 350, 130, true, true);
    });
    $("#btnCancel").die().live('click', function () {
        RedirectionParam();
    });
}
//-------------Map les éléments de la liste des contextes------------
function MapElementContexte() {
    AlternanceLigne("BodyParam", "", false, null);
    $("#addContexte").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");

    $("#addContexte").die().live('click', function () {
        AddParam("Contexte", $("#CodeService").val(), $("#CodeActGes").val(), $("#CodeEtape").val());
    });

    $("a[name=openContexte]").click(function () {
        $("#CodeContexte").val($(this).attr("id").split("_")[1]);
        OpenParam("Contexte");
    });

    $("img[name=delContexte]").click(function () {
        $("#hiddenInputId").val($(this).attr('id'));
        ShowCommonFancy("Confirm", "DeleteParam", "Veuillez confirmer la suppression du paramètre", 350, 130, true, true);
    });

    $("img[name=editContexte]").click(function () {
        EditContexte($(this).attr('id').split('_')[1]);
    });

    $("#btnCancel").die().live('click', function () {
        OpenParam("ActeGestion");
    });

    $("#AjoutLibre").die().live('change', function () {
        if ($(this).is(":checked")) {
            $("#Rubrique").removeAttr("disabled");
            $("#SousRubrique").removeAttr("disabled");
            $("#Sequence").removeAttr("disabled");
        }
        else {
            $("#Rubrique").attr("disabled", "disabled").val("").attr("title", "");
            $("#SousRubrique").attr("disabled", "disabled").val("").html("").attr("title", "");
            $("#Sequence").attr("disabled", "disabled").val("").html("").attr("title", "");
        }
    });

    LoadDDLSousRubriquesByRubrique();
    LoadDDLSequenceByRubriqueAndSousRubrique();
}
//-------------Map les éléments de la liste des EG/DI------------
function MapElementEGDI() {
    AlternanceLigne("BodyParam", "", false, null);
    $("#addEGDI").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");

    $("#addEGDI").die().live('click', function () {
        AddParam("EGDI", $("#CodeService").val(), $("#CodeActGes").val(), $("#CodeEtape").val(), $("#CodeContexte").val());
    });

    $("a[name=openEGDI]").click(function () {
        var codeEGDI = $(this).attr('id').split('_')[1];
        AddParam("EGDI", $("#CodeService").val(), $("#CodeActGes").val(), $("#CodeEtape").val(), $("#CodeContexte").val(), codeEGDI);
    });

    $("img[name=attachClause]").die().live('click', function () {
        var id = $(this).attr('id').split('_')[1];
        OpenRattachClause(id);
    });

    $("img[name=delEGDI]").click(function () {
        $("#hiddenInputId").val($(this).attr('id'));
        ShowCommonFancy("Confirm", "DeleteParam", "Veuillez confirmer la suppression du paramètre", 350, 130, true, true);
    });
    $("#btnCancel").die().live('click', function () {
        OpenParam("Etape");
    });
}
//------------Map les éléments de l'écran rattachement----------------------
function MapElementRattach() {
    AlternanceLigne("BodyClauses", "", false, null);
    $("#btnCancel").die().live('click', function () {
        OpenParam("Contexte");
    });
    $("#Affichage").die().live('change', function () {
        ChangeDisplayClause($(this).val());
    });
    $("#refreshList").die().live('click', function () {
        ReloadClauses();
    });

    $("input[name=rattach]").die().live('click', function () {
        var isChecked = $(this).is(':checked');
        var codeClause = $(this).attr('id').split('_')[1];
        if (isChecked) {
            OpenSaisieRattach(codeClause, 0, false);
        }
        else {
            DeleteSaisieRattach(codeClause);
        }
    });

    $("img[name=detail]").die().live('click', function () {
        var id = $(this).attr('id');
        OpenSaisieRattach(id.split('_')[1], id.split('_')[2], true);
    });
}
//------------Ouvre l'écran de saisie de rattachement des clauses-----------
function OpenSaisieRattach(codeClause, codeRattach, modifMode) {
    var codeService = $("#CodeService").val();
    var codeActGes = $("#CodeActGes").val();
    var codeEtape = $("#CodeEtape").val();
    var codeContexte = $("#CodeContexte").val();
    var codeEGDI = $("#Code").val();
    var type = "";
    if ($("#EG").is(':checked'))
        type = "EG";
    if ($("#DI").is(':checked'))
        type = "DI";
    $.ajax({
        type: "POST",
        url: "/ParamClause/AttachClause",
        data: { codeService: codeService, codeActGes: codeActGes, codeEtape: codeEtape, codeContexte: codeContexte, codeEGDI: codeEGDI, type: type, codeClause: codeClause, codeRattach: codeRattach, modifMode: modifMode },
        success: function (data) {
            MapSaisieRattachElement(data, codeClause, modifMode);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Supprime le rattachement de la clause----------------
function DeleteSaisieRattach(codeClause) {
    var codeEGDI = $("#Code").val();
    $.ajax({
        type: "POST",
        url: "/ParamClause/DeleteAttachClause",
        data: { codeEGDI: codeEGDI, codeClause: codeClause },
        success: function (data) {
            $("img[id^=dtl_" + codeClause + "_]").remove();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Map les éléments de la div de rattachement-------------
function MapSaisieRattachElement(data, codeClause, modifMode) {
    $("#divDataSaisie").html(data);
    $("#saisieRattach").show();
    $("#btnFancyAnnuler").die().live('click', function () {
        $("#divDataSaisie").html('');
        $("#saisieRattach").hide();
        if (!modifMode)
            $("input[type=checkbox][id=rattach_" + codeClause + "]").removeAttr('checked');
    });
    $("#btnFancyValider").die().live('click', function () {
        SaveRattachClause();
    });
    $("#ImpressAnnexe").die().live('click', function () {
        var isChecked = $(this).is(':checked');
        if (isChecked)
            $("#CodeAnnexe").removeAttr('readonly').removeClass('readonly');
        else
            $("#CodeAnnexe").attr('readonly', 'readonly').addClass('readonly').val('');

    });
    //Numerique();
    FormatNumericValue();
}
//------------Ouvre l'écran de rattachement des clauses---------------------
function OpenRattachClause(id) {
    var codeService = $("#CodeService").val();
    var codeActGes = $("#CodeActGes").val();
    var codeEtape = $("#CodeEtape").val();
    var codeContexte = $("#CodeContexte").val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamClause/OpenRattachClause",
        data: { codeService: codeService, codeActGes: codeActGes, codeEtape: codeEtape, codeContexte: codeContexte, codeEGDI: id },
        success: function (data) {
            $("#divBodyPage").html(data);
            MapElementRattach();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Sauvegarde le rattachement de la clause---------------
function SaveRattachClause() {
    ShowLoading();

    $(".requiredField").removeClass('requiredField');

    var nature = "";
    if ($("#Obligatoire").is(':checked'))
        nature = "O";
    if ($("#Proposee").is(':checked'))
        nature = "P";
    if ($("#Suggeree").is(':checked'))
        nature = "S";

    var impressAnnexe = "N";
    if ($("#ImpressAnnexe").is(':checked'))
        impressAnnexe = "O";

    var att = "";
    if ($("#Souligne").is(':checked'))
        att += ";souligne";
    if ($("#Gras").is(':checked'))
        att += ";gras";
    if (att != "")
        att = att.substring(1);
    var attribut = "<Paragraph style=\"" + $("#StyleWord").val() + "\" nature=\"texte\" fontsize=\"" + $("#Taille").val() + "\" font=\"arial\" att=\"" + att + "\"/>";

    if (impressAnnexe == "O" && $("#CodeAnnexe").val() == "") {
        $("#CodeAnnexe").addClass('requiredField');
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/ParamClause/SaveRattachClause",
        data: {
            codeRattach: $("#CodeRattachement").val(), codeClause: $("#Code").val(), codeEGDI: $("#IdAttachEGDI").val(), numOrdre: $("#AttachOrdre").val(),
            nom1: $("#ClauseNom1").val(), nom2: $("#ClauseNom2").val(), nom3: $("#ClauseNom3").val(), nature: nature, impressAnnexe: impressAnnexe, codeAnnexe: $("#CodeAnnexe").val(),
            attribut: encodeURIComponent(attribut), version: $("#VersionClause").val()
        },
        success: function (data) {
            if ($("#ModeSaisie").val() == "False") {
                var newDetail = $("<img id='dtl_" + $("#IdAttachEGDI").val() + "_" + data + "' width='16' height='16' class='CursorPointer' src='/Content/Images/afficher_details1616.png' alt='' name='detail'>");
                $("tr[id=trAttach_" + $("#IdAttachEGDI").val() + "]").attr('rattach', data);
                $("td[id=rattachDtl_" + $("#IdAttachEGDI").val() + "]").append(newDetail);
            }
            $("#divDataSaisie").html('');
            $("#saisieRattach").hide();
            $("img[name=detail]").die().live('click', function () {
                var id = $(this).attr('id');
                OpenSaisieRattach(id.split('_')[1], id.split('_')[2], true);
            });
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Affiche les clauses suivant l'affichage demandé-----------
function ChangeDisplayClause(affichage) {
    switch (affichage) {
        case "R":
            $("tr[rattach]").hide();
            $("tr[rattach!=0]").show();
            break;
        case "N":
            $("tr[rattach]").hide();
            $("tr[rattach=0]").show();
            break;
        default:
            $("tr[rattach]").show();
            break;
    }
}
//------------Reload la liste des clauses----------------------
function ReloadClauses() {
    ShowLoading();
    var restrict = $("#Restriction").val();
    var codeEGDI = $("#Code").val();
    $.ajax({
        type: "POST",
        url: "/ParamClause/ReloadClauses",
        data: { codeEGDI: codeEGDI, restrict: restrict },
        success: function (data) {
            $("#divClauses").html(data);
            MapElementRattach();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Map les éléments de la fancy d'ajout de paramètre------------
function MapAddParam() {
    $("#btnFancyAnnuler").die().live('click', function () {
        $("#divDataParam").html("");
        $("#ajoutParam").hide();
    });
    $("#btnFancyValider").die().live('click', function () {
        var paramType = $("#ParamType").val();
        switch (paramType) {
            case "ActeGestion":
                if (CheckActeGestion()) {
                    AddActeGestion();
                }
                else {
                    common.dialog.error("Veuillez renseigner l'acte de gestion");
                }
                break;
            case "Etape":
                if (CheckEtape()) {
                    AddEtape();
                }
                else {
                    common.dialog.error("Veuillez renseigner l'étape");
                }
                break;
            case "Contexte":
                if (CheckContexte()) {
                    AddContexte();
                }
                break;
            case "EGDI":
                if (CheckEGDI()) {
                    AddEGDI();
                }
                else {
                    common.dialog.error("Veuillez renseigner les champs requis");
                }
                break;
            default:
                break;
        }
    });

    $("#NumOrdre").die().live('keyup', function (e) {
        if (/\D/g.test(this.value)) {
            // Filter non-digits from input value.
            this.value = this.value.replace(/\D/g, '');
            if (this.value.length == 0) {
                this.value = $("#hidNumOrdre").val();
            }
        }
    });
    $("#NumOrdre").live('change', function (e) {
        if (parseInt(this.value.replace(/\D/g, '')) < parseInt($("#hidNumOrdre").val().replace(/\D/g, ''))) {
            this.value = $("#hidNumOrdre").val();
        }
        if (this.value == "") {
            this.value = $("#hidNumOrdre").val();
        }
    });

    if (!$("#AjoutLibre").is(":checked")) {
        $("#Rubrique").attr("disabled", "disabled");
        $("#SousRubrique").attr("disabled", "disabled");
        $("#Sequence").attr("disabled", "disabled");
    }

    $("#Specificite").die().live("change", function () {
       CheckSpecificite();
    });

    CheckSpecificite();
}

function CheckSpecificite()
{
    if ($("#Specificite").val() != "" && $("#Specificite").val() != " - Aucune spécificité") {
        $("#AjoutClausier").removeAttr("checked");
        $("#AjoutClausier").attr("disabled", "disabled");

        $("#AjoutLibre").removeAttr("checked");
        $("#AjoutLibre").attr("disabled", "disabled");

        $("#Rubrique").val("");
        $("#Rubrique").attr("disabled", "disabled");

        $("#SousRubrique").val("");
        $("#SousRubrique").attr("disabled", "disabled");

        $("#Sequence").val("");
        $("#Sequence").attr("disabled", "disabled");
    }
    else {
        $("#AjoutClausier").removeAttr("disabled");
        $("#AjoutLibre").removeAttr("disabled");
        $("#Rubrique").removeAttr("disabled");
        $("#SousRubrique").removeAttr("disabled");
        $("#Sequence").removeAttr("disabled", "disabled");
    }
}

//---------------Vérifie si les champs de l'acte de gestion sont bien renseignés----------
function CheckActeGestion() {
    var result = true;
    $("#ActeGestionDdl").removeClass('requiredField');
    if ($("#ActeGestionDdl").val() == "") {
        $("#ActeGestionDdl").addClass('requiredField');
        result = false;
    }
    return result;
}
//--------------Vérifie si les champs de l'étape sont bien renseignés--------------
function CheckEtape() {
    var result = true;
    $("#Etape").removeClass('requiredField');
    if ($("#Etape").val() == "") {
        $("#Etape").addClass('requiredField');
        result = false;
    }
    return result;
}
//--------------Vérifie si les champs du contexte sont bien renseignés--------------
function CheckContexte() {
    var result = true;
    $(".requiredField").removeClass('requiredField');
    if ($("#Contexte").val() == "") {
        $("#Contexte").addClass('requiredField');
        result = false;
    }
    if (!$("#EmplacementModif").is(":checked") && $("#Emplacement").val() == "") {
        $("#Emplacement").addClass('requiredField');
        result = false;
    }
    return result;
}
//--------------Vérifie si les champs de l'EG/DI sont bien renseignés--------------
function CheckEGDI() {
    var result = true;
    $(".requiredField").removeClass('requiredField');
    if ($("#CodEGDI").val() == "") {
        $("#CodEGDI").addClass('requiredField');
        result = false;
    }
    if ($("#NumOrd").val() == "") {
        $("#NumOrd").addClass('requiredField');
        result = false;
    }
    if ($("#LibelleEGDI").val() == "") {
        $("#LibelleEGDI").addClass('requiredField');
        result = false;
    }
    return result;
}
//---------------Ajoute un acte de gestion-----------
function AddActeGestion() {
    var codeService = $("#CodeService").val();
    var codeActeGestion = $("#ActeGestionDdl").val();

    if (codeActeGestion != "") {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamClause/AddActeGestion",
            data: { codeService: codeService, codeActeGestion: codeActeGestion },
            success: function (data) {
                $("#divDataParam").html("");
                $("#ajoutParam").hide();
                LoadActeGestion(codeService);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//----------------Ajoute une étape----------------
function AddEtape() {
    var codeService = $("#CodeService").val();
    var codeActeGestion = $("#CodeActGes").val();
    var codeEtape = $("#Etape").val();
    var numOrdre = $("#NumOrdre").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamClause/AddEtape",
        data: { codeService: codeService, codeActeGestion: codeActeGestion, codeEtape: codeEtape, numOrdre: numOrdre },
        success: function (data) {
            $("#divDataParam").html("");
            $("#ajoutParam").hide();
            OpenParam("ActeGestion");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------Ajoute un contexte----------------
function AddContexte() {
    var idContexte = $("#IdContexte").val();
    var codeService = $("#CodeService").val();
    var codeActeGestion = $("#CodeActGes").val();
    var codeEtape = $("#CodeEtape").val();
    var codeContexte = $("#Contexte").val();
    var emplacementModif = $("#EmplacementModif").is(":checked");
    var ajoutClausier = $("#AjoutClausier").is(":checked");
    var ajoutLibre = $("#AjoutLibre").is(":checked");
    var scriptControle = $("#ScriptControle").val();
    var rubrique = $("#Rubrique").val();
    var sousRubrique = $("#SousRubrique").val();
    var sequence = $("#Sequence").val();
    var emplacement = $("#Emplacement").val();
    var sousEmplacement = $("#SousEmplacement").val();
    var numOrdonnancement = $("#NumOrdonnancement").val();
    var specificite = $("#Specificite").val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamClause/AddContexte",
        data: {
            idContexte: idContexte, codeService: codeService, codeActeGestion: codeActeGestion, codeEtape: codeEtape, codeContexte: codeContexte,
            emplacementModif: emplacementModif, ajoutClausier: ajoutClausier, ajoutLibre: ajoutLibre, scriptControle: scriptControle,
            rubrique: rubrique, sousRubrique: sousRubrique, sequence: sequence, emplacement: emplacement, sousEmplacement: sousEmplacement, numOrdonnancement: numOrdonnancement, specificite: specificite
        },
        success: function (data) {
            $("#divDataParam").html("");
            $("#ajoutParam").hide();
            OpenParam("Etape");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------Ajoute un EG/DI-----------------------
function AddEGDI() {
    var codeService = $("#CodeService").val();
    var codeActeGestion = $("#CodeActGes").val();
    var codeEtape = $("#CodeEtape").val();
    var codeContexte = $("#CodeContexte").val();
    var type = "";
    if ($("#EG").is(':checked'))
        type = "EG";
    if ($("#DI").is(':checked'))
        type = "DI";

    var codeEGDI = $("#CodEGDI").val();
    var numOrdre = $("#NumOrdre").val();
    var libelleEGDI = $("#LibelleEGDI").val();
    var commentaire = $("#Commentaires").val();

    var code = $("#Code").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamClause/AddEGDI",
        data: {
            codeService: codeService, codeActeGestion: codeActeGestion, codeEtape: codeEtape, codeContexte: codeContexte,
            type: type, codeEGDI: codeEGDI, numOrdre: numOrdre, libelleEGDI: libelleEGDI, commentaire: commentaire, code: code
        },
        success: function (data) {
            $("#divDataParam").html("");
            $("#ajoutParam").hide();
            OpenParam("Contexte");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}
//-------Formate les input/span des valeurs----------
function FormatNumericValue() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    //FormatNumerique('numeric', '', '99999999999', '0');
    common.autonumeric.applyAll('init', 'numeric', '', null, null, '99999999999', '0');
}

//----------------------Application de la recherche ajax des SousRubriques par Rubrique----------------------
function LoadDDLSousRubriquesByRubrique() {
    $("#Rubrique").live('change', function () {
        AffectTitleList($(this));
        $.ajax({
            type: "POST",
            url: "/Clausier/GetSousRubriques",
            data: { codeRubrique: $(this).val(), paramScreen: 1 },
            success: function (data) {
                $("#SousRubriqueDiv").html(data);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//----------------------Application de la recherche ajax des Sequences par SousRubrique----------------------
function LoadDDLSequenceByRubriqueAndSousRubrique() {
    $("#SousRubrique").live('change', function () {
        AffectTitleList($(this));
        $.ajax({
            type: "POST",
            url: "/Clausier/GetSequences",
            data: { codeSousRubrique: $(this).val(), codeRubrique: $("#Rubrique").val(), paramScreen: 1 },
            success: function (data) {
                $("#SequenceDiv").html(data);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//----------Vérifie l'existence du concept/famille--------
function CheckConceptFamille(famille) {
    ShowLoading();
    var concept = "KHEOP";
    $.ajax({
        type: "POST",
        url: "/ParamClause/CheckConceptFamille",
        data: { codeConcept: concept, codeFamille: famille },
        success: function (data) {
            if (data != "") {
                $("#CodeConcept").val(concept);
                $("#LibelleConcept").val(data);
                OpenConceptFamille(concept, famille);
            }
            else {
                ShowCommonFancy("Error", "", "L'association " + concept + "/" + famille + "  n'existe pas.", 300, 80); CloseLoading();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------Ouvre la vue partielle pour ajouter des valeurs aux concept/famille------------
function OpenConceptFamille(concept, famille) {
    $.ajax({
        type: "POST",
        url: "/ParamClause/OpenConceptFamille",
        data: { codeConcept: concept, codeFamille: famille },
        success: function (data) {
            $("#divDataConceptFamille").html(data);
            $("#addConceptFamille").show();
            $("#CodeLibelleConcept").val($("#CodeConcept").val() + " - " + $("#LibelleConcept").val());
            MapConceptFamille();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Map les éléments des concepts/familles--------
function MapConceptFamille() {
    AlternanceLigne("BodyValeurs", "", false, null);
    $("#btnFermer").die().live('click', function () {
        $("#divDataConceptFamille").html("");
        $("#addConceptFamille").hide();

    });
    $("#AjouterValeur").die().live('click', function () {
        EditerValeur("I", "");
    });
    $("img[name=dupliquerValeur]").die().live('click', function () {
        EditerValeur('D', $(this).attr("id").split("_")[1]);
    });

    $("img[name=supprValeur]").die().live("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $("#idValeurSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerValeur", "Vous allez supprimer la valeur.<br/>Etes-vous sûr de vouloir continuer ?",
                  320, 150, true, true);
    });
}
//-------Editer une valeur (Création/Modification)----------
function EditerValeur(modeOperation, codeValeur) {
    ShowLoading();
    var typeCode = $("#AlphaType").is(':checked') ? "A" : "N";
    $.ajax({
        type: "POST",
        url: "/ParamFamilles/EditerValeur",
        data: {
            codeConcept: $("#CodeConcept").val(), libelleConcept: $("#LibelleConcept").val(),
            codeFamille: $("#CodeFamille").val(), libelleFamille: $("#LibelleFamille").val(),
            libelleLongNum1: $("#LibelleLongNum1").val(), typeNum1: $("#TypeNum1").val(),
            libelleLongNum2: $("#LibelleLongNum2").val(), typeNum2: $("#TypeNum2").val(),
            libelleLongAlpha1: $("#LibelleLongAlpha1").val(), libelleLongAlpha2: $("#LibelleLongAlpha2").val(),
            codeValeur: codeValeur, modeOperation: modeOperation, longueur: $("#Longueur").val(), typeCode: typeCode, restrictionFamille: $("#RestrictionFamille").val(),
            restrictionParam: $("#RestrictionParam").val()
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataEditValeur").html(data);
            AlbScrollTop();
            $("#divEditValeur").show();
            MapElementEditValeur();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Supprime la valeur----------
function SupprimerValeur(id) {
    var additionalParam = $("#AdditionalParam").val();
    var restrictionParam = $("#RestrictionParam").val();
    var codeConcept = $("#CodeConcept").val();
    var codeFamille = $("#CodeFamille").val();
    if (id != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamFamilles/SupprimerValeur",
            data: { codeConcept: codeConcept, codeFamille: codeFamille, codeValeur: id, additionalParam: additionalParam, restrictionParam: restrictionParam },
            success: function (data) {
                $("#divBodyValeurs").html(data);
                MapConceptFamille();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//-------Map les éléments de la div flottante EditValeur-----
function MapElementEditValeur() {
    $("#btnAnnulerDivF").die().live('click', function () {
        $("#divDataEditValeur").html('');
        $("#divEditValeur").hide();
    });
    $("#btnValider").die().live('click', function () {
        UpdateValeur();
    });
    formatDatePicker();
    FormatDecimalValue();
}
//------Formate les dates---------
function formatDatePicker() {
    if ($("#TypeNum1").val() == "D")
        $("#ValeurNum1").datepicker({ dateFormat: 'dd/mm/yy' });
    if ($("#TypeNum2").val() == "D")
        $("#ValeurNum2").datepicker({ dateFormat: 'dd/mm/yy' });
}
//-------Formate les input/span des valeurs----------
function FormatDecimalValue() {
    if ($("#TypeNum1").val() == "M")
        common.autonumeric.apply($("#ValeurNum1"), 'init', 'decimal', null, null, $("#NbrDecimal1").val(), null, null);
    if ($("#TypeNum2").val() == "M")
        common.autonumeric.apply($("#ValeurNum2"), 'init', 'decimal', null, null, $("#NbrDecimal2").val(), null, null);
    common.autonumeric.applyAll('init', 'numeric', '', null, null, '99999999999', null);
}

//-------Update une valeur (Création, modification ou duplication)---
function UpdateValeur() {
    var erreur = false;
    var acceptNullValue = $("#AcceptNullValue").is(':checked');
    if ($("#CodeValeur").val() == "" && acceptNullValue == false) {
        $("#CodeValeur").addClass('requiredField');
        erreur = true;
    }
    if ($("#LibelleValeur").val() == "") {
        $("#LibelleValeur").addClass('requiredField');
        erreur = true;
    }
    if ($("#ValeurNum1").val() == "" && $("#LibelleLongNum1").val() != "") {
        $("#ValeurNum1").addClass('requiredField');
        erreur = true;
    }
    if ($("#ValeurNum2").val() == "" && $("#LibelleLongNum2").val() != "") {
        $("#ValeurNum2").addClass('requiredField');
        erreur = true;
    }

    if (erreur) {
        return false;
    }
    var codeConcept = $("#CodeConcept").val();
    var codeFamille = $("#CodeFamille").val();
    var codeValeur = $("#CodeValeur").val();
    var libelleCourt = $("#LibelleValeur").val();
    var libelleLong = $("#LibelleLongValeur").val();
    var description1 = $("#Description1").val();
    var description2 = $("#Description2").val();
    var description3 = $("#Description3").val();
    var valeurNum1 = $("#ValeurNum1").val();
    var typeNum1 = $("#TypeNum1").val();
    var valeurNum2 = $("#ValeurNum2").val();
    var typeNum2 = $("#TypeNum2").val();
    var valeurAlpha1 = $("#ValeurAlpha1").val();
    var valeurAlpha2 = $("#ValeurAlpha2").val();
    var restriction = $("#Restriction").val();
    var codeFiltre = $("#CodeFiltre").val();
    var longueur = $("#Longueur").val();
    var typeCode = $("#TypeCode").val();
    var modeOperation = $("#ModeOperation").val();
    var additionalParam = $("#AdditionalParam").val();
    var restrictionParam = $("#RestrictionParam").val();

    $.ajax({
        type: "POST",
        url: "/ParamFamilles/UpdateValeur",
        data: {
            codeConcept: codeConcept, codeFamille: codeFamille, codeValeur: codeValeur,
            libelleCourt: libelleCourt, libelleLong: libelleLong, description1: description1, description2: description2, description3: description3,
            valeurNum1: valeurNum1, valeurNum2: valeurNum2, valeurAlpha1: valeurAlpha1, valeurAlpha2: valeurAlpha2, restriction: restriction, codeFiltre: codeFiltre,
            modeOperation: modeOperation, longueur: longueur, typeCode: typeCode,
            typeNum1: typeNum1, typeNum2: typeNum2,
            additionalParam: additionalParam, restrictionParam: restrictionParam, acceptNullValue: acceptNullValue
        },
        success: function (data) {
            $("#divDataEditValeur").html('');
            $("#divEditValeur").hide();
            $("#divBodyValeurs").html(data);
            MapConceptFamille();
            //UpdateListValeurs(codeConcept, codeFamille, additionalParam, $("#RestrictionParam").val());
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Ouvre l'édition d'un contexte----------
function EditContexte(codeContexte) {
    var codeService = $("#CodeService").val();
    var codeActeGestion = $("#CodeActGes").val();
    var codeEtape = $("#CodeEtape").val();

    $.ajax({
        type: "POST",
        url: "/ParamClause/EditContexte",
        data: { codeService: codeService, codeActeGestion: codeActeGestion, codeEtape: codeEtape, codeContexte: codeContexte },
        success: function (data) {
            $("#divDataParam").html(data);
            $("#ajoutParam").show();
            MapAddParam();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}