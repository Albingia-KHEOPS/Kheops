$(document).ready(function () {
    MapElementPage();
});
//--------Map les éléments de la page-----------
function MapElementPage() {
    AffectTitleList($("#Typologie"));
    AffectTitleList($("#Branche"));
    AffectTitleList($("#Cible"));

    $("#btnCancel").die().live('click', function () {
        Redirection("BackOffice", "Index");
    });

    LoadListNomenclatures("", "", "");

    $("#Typologie").die().live('change', function () {
        AffectTitleList($(this));
        LoadListNomenclatures($(this).val(), $("#Branche").val(), $("#Cible").val());
        $("#txtLibelle").val("");
    });
    $("#Branche").die().live('change', function () {
        LoadListCible($(this).val());
        AffectTitleList($(this));
        $("#txtLibelle").val("");
    });
    $("#Cible").die().live('change', function () {
        LoadListNomenclatures($("#Typologie").val(), $("#Branche").val(), $("#Cible").val());
        AffectTitleList($(this));
        $("#txtLibelle").val("");
    });

    $("#addNomenclature").die().live('click', function () {
        $("#tblBodyNomenclature > tbody  > tr").removeClass("selectLine");
        OpenNomenclature(0);
    });

    $("#btnConfirmOk").die().live('click', function () {
        var action = $("#hiddenAction").val();
        switch (action) {
            case "Delete":
                DeleteNomenclature();
                break;
            case "DeleteGrille":
                DeleteGrille();
                break;
            case "DeleteLine":
                DeleteLineGrille();
                break;
        }
        $("#hiddenAction").val("");
        CloseCommonFancy();
    });
    $("#btnConfirmCancel").die().live('click', function () {
        $("#hiddenAction").val("");
        CloseCommonFancy();
    });

    $("#txtLibelle").keyup(function () {
        filterByLabel();
    });
}

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
//--------Charge la liste des nomenclatures-------
function LoadListNomenclatures(typologie, branche, cible, id) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionNomenclature/LoadListNomenclature",
        data: { typologie: typologie, branche: branche, cible: cible },
        success: function (data) {
            var splitHtmlChar = $("#SplitHtmlChar").val();

            $("#divListNomenclatures").html(data);
            MapListeNomenclature();
            if (id != "" && id != undefined) {
                $("td[id='ordreNomen" + splitHtmlChar + id + "'][albOpenNomen='" + id + "']").trigger('click');
            }
            else {
                $("#divInfoNomenclature").html("<div class='nGradientSection FloatLeft' style='width: 594px; height: 524px;'></div>");
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Map les élements de la liste des nomenclatures-----
function MapListeNomenclature() {
    AlternanceLigne("BodyNomenclature", "selNomenclatureCode", true, null);

    $("td[albOpenNomen]").each(function () {
        $(this).click(function () {
            OpenNomenclature($(this).attr("albOpenNomen"));
            $("#selNomenclatureCode").val($(this).attr('albOpenNomen'));
            $(this).parent().parent().children(".selectLine").removeClass("selectLine");
            $(this).parent().addClass("selectLine");
        });
    });

    $("img[name='updNomenclature']").each(function () {
        $(this).click(function () {
            var splitHtmlChar = $("#SplitHtmlChar").val();
            var idNomenclature = $(this).attr('id').split(splitHtmlChar)[1];
            OpenNomenclature(idNomenclature);
        });
    });
}
//--------Charge les cibles de la branche-----------
function LoadListCible(codeBranche) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/GetCibles",
        data: { codeBranche: codeBranche },
        success: function (data) {
            $("#divCibles").html(data);
            LoadListNomenclatures($("#Typologie").val(), $("#Branche").val(), $("#Cible").val());
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Ouvre la div d'ajout/maj de nomenclature-------
function OpenNomenclature(idNomenclature) {
    var typologie = $("#Typologie").val();

    if (idNomenclature == "0" && typologie == "") {
        common.dialog.error("Veuillez choisir une typologie pour pouvoir ajouter une nomenclature.");
    }
    else {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/GestionNomenclature/OpenNomenclature",
            data: { idNomenclature: idNomenclature, typologie: typologie },
            success: function (data) {
                $("#divInfoNomenclature").html(data);
                MapNomenclatureElement();
                $("#divInfoNomenclature").parent().removeClass("nGradientSection");
                $("#Typologie").val(typologie);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//--------Map les éléments de la nomenclature---------
function MapNomenclatureElement() {
    $("#btnNomenclatureValider").die().live('click', function () {
        SaveNomenclature();
    });
    $("#btnNomenclatureAnnuler").die().live('click', function () {
        $("#divInfoNomenclature").html("<div class='nGradientSection FloatLeft' style='width: 594px; height: 524px;'></div>");
        $("#tblBodyNomenclature > tbody  > tr").removeClass("selectLine");
    });
    $("#btnNomenclatureSupprimer").die().live('click', function () {
        ShowCommonFancy("Confirm", "Delete", "Etes-vous certain de vouloir supprimer cette nomenclature ?", 300, 80, true, true, true);
    });

    AlternanceLigne("BodyGrille", "", false, null);

    $("td[albinfogrille]").each(function () {
        $(this).click(function () {
            OpenInfoGrille($(this).attr("albinfogrille"));
        });
    });

    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999.99', '0.00');
    //FormatDecimal('decimal', ' ', 2, '99999.99', '0');
}
//---------Sauvegarde les informations de la nomenclature---------
function SaveNomenclature() {
    var bError = false;

    var idNomenclature = $("#IdNomenclature").val();
    var codeNomenclature = $("#CodeNomenclature").val();
    var ordreNomenclature = $("#OrdreNomenclature").val();
    var libelleNomenclature = $("#LibelleNomenclature").val();
    var typologie = $("#Typologie").val();

    if (codeNomenclature == "") {
        $("#CodeNomenclature").addClass("requiredField");
        bError = true;
    }
    if (parseInt(ordreNomenclature) <= 0) {
        $("#OrdreNomenclature").addClass("requiredField");
        bError = true;
    }

    if (bError) return false;

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionNomenclature/SaveNomenclature",
        data: { idNomenclature: idNomenclature, codeNomenclature: codeNomenclature, ordreNomenclature: ordreNomenclature, libelleNomenclature: libelleNomenclature, typologie: typologie },
        success: function (data) {
            var msgError = AlbJsSplitElem(data, 1, 'ERROR');
            if (msgError == "noData") {
                LoadListNomenclatures($("#Typologie").val(), $("#Branche").val(), $("#Cible").val(), data);
            }
            else {
                common.dialog.error(msgError);
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Supprimer la nomenclature------------
function DeleteNomenclature() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionNomenclature/DeleteNomenclature",
        data: { idNomenclature: $("#IdNomenclature").val() },
        success: function (data) {
            LoadListNomenclatures($("#Typologie").val(), $("#Branche").val(), $("#Cible").val());
            $("#divInfoNomenclature").html("<div class='nGradientSection FloatLeft' style='width: 594px; height: 524px;'></div>");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Ouvre la grille en div flottante-----------
function OpenInfoGrille(codeGrille) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/OpenGrille",
        data: { idGrille: codeGrille },
        success: function (data) {
            $("#divDataGrilleInfo").html(data);
            $("#divGrilleInfo").show();
            MapGrilleElement();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Map les éléments de la grille---------
function MapGrilleElement() {
    $("#btnGrilleValider").die().live('click', function () {
        if (!$(this).is(":disabled")) {
            SaveGrille();
        }
    });
    $("#btnGrilleAnnuler").die().live('click', function () {
        $("#divDataGrilleInfo").html("");
        $("#divGrilleInfo").hide();
    });
    $("#btnGrilleSupprimer").die().live('click', function () {
        ShowCommonFancy("Confirm", "DeleteGrille", "Etes-vous certain de vouloir supprimer cette grille ?", 300, 80, true, true, true);
    });

    AlternanceLigne("BodyTypologie", "", false, null);
    AlternanceLigne("BodyCibles", "", false, null);

    $("select[name='Typologie']").each(function () {
        AffectTitleList($(this));
        $(this).change(function () {
            ChangeTypologie($(this));
        });
    });

    $("img[name='updGrille']").each(function () {
        $(this).click(function () {
            ToggleModeGrille($(this));
        });
    });
    $("img[name='svgGrille']").each(function () {
        $(this).click(function () {
            SaveLineGrille($(this));
        });
    });
    $("img[name='delGrille']").each(function () {
        $(this).click(function () {
            var splitHtmlChar = $("#SplitHtmlChar").val();
            $("#delIdLine").val(AlbJsSplitElem($(this).attr("id"), 1, splitHtmlChar));
            ShowCommonFancy("Confirm", "DeleteLine", "Etes-vous certain de vouloir cette typologie ?", 300, 80, true, true, true);
        });
    });
    $("img[name='cancelGrille']").each(function () {
        $(this).click(function () {
            ToggleModeGrille($(this));
        });
    });

    $("td[name='valeurGrille']").each(function () {
        $(this).click(function () {
            var splitHtmlChar = $("#SplitHtmlChar").val();
            var niveau = $(this).attr("albinfogrille");
            if ($("img[id='updGrille" + splitHtmlChar + niveau + "']").is(":visible") && $.trim($(this).html()) != "") {
                OpenSelectionValeur(niveau);
            }
        });
    });

    DisplayDelLineBtn();
}
//----------Change le libellé de la typologie--------
function ChangeTypologie(elem) {
    AffectTitleList(elem);
    var splitHtmlChar = $("#SplitHtmlChar").val();
    var id = AlbJsSplitElem(elem.attr("id"), 1, splitHtmlChar);;
    if ($("input[id='LibTypologie" + splitHtmlChar + id + "']").val() == "") {
        var lib = AlbJsSplitElem($("select[id='" + elem.attr("id") + "'] option:selected").text(), 1, ' - ');
        $("input[id='LibTypologie" + splitHtmlChar + id + "']").val(lib);
    }
}
//---------Affiche le bounton supprimer de la grille de typologie---------
function DisplayDelLineBtn() {
    var countTypo = $("#tblBodyTypologie tr").length;
    var splitHtmlChar = $("#SplitHtmlChar").val();
    $("img[name='delGrille']").hide();
    if (countTypo < 5) {
        $("img[id='delGrille" + splitHtmlChar + (countTypo - 1) + "']").show();
    }
    else {
        if ($.trim($("td[name='valeurGrille'][albinfogrille=5]").html()) != "")
            $("img[id='delGrille" + splitHtmlChar + (countTypo) + "']").show();
        else
            $("img[id='delGrille" + splitHtmlChar + (countTypo - 1) + "']").show();
    }
}
//---------Sauvegarde les informations de la grille---------
function SaveGrille() {
    $(".requiredField").removeClass("requiredField");
    var bError = false;

    var codeGrille = $("#CodeGrille").val();
    var libelleGrille = $("#LibelleGrille").val();

    if (codeGrille == "") {
        $("#CodeGrille").addClass("requiredField");
        bError = true;
    }

    if (libelleGrille == "") {
        $("#LibelleGrille").addClass("requiredField");
        bError = true;
    }

    if (bError) return false;

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/SaveGrille",
        data: { codeGrille: codeGrille, libelleGrille: libelleGrille, newGrille: $("#newGrille").val() },
        success: function (data) {
            var msgError = AlbJsSplitElem(data, 1, 'ERROR');
            if (msgError == "noData") {
                $("#divDataGrilleInfo").html("");
                $("#divGrilleInfo").hide();
                OpenNomenclature($("#IdNomenclature").val());
            }
            else {
                common.dialog.error(msgError);
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Supprime la grille----------------
function DeleteGrille() {
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/DeleteGrille",
        data: { codeGrille: $("#CodeGrille").val() },
        success: function (data) {
            $("#divDataGrilleInfo").html("");
            $("#divGrilleInfo").hide();
            OpenNomenclature($("#IdNomenclature").val());
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Sauvegarde la ligne de la grille-----------
function SaveLineGrille(elem) {
    var splitHtmlChar = $("#SplitHtmlChar").val();
    $(".requiredField").removeClass("requiredField");
    var id = AlbJsSplitElem(elem.attr("id"), 1, splitHtmlChar);
    var bError = false;

    var codeGrille = $("#CodeGrille").val();
    var libelleGrille = $("#LibelleGrille").val();
    var typologieGrille = $("#Typologie").val();

    var typologie = $("select[id='Typologie" + splitHtmlChar + id + "']").val();
    var libTypologie = $("input[id='LibTypologie" + splitHtmlChar + id + "']").val();
    var lienTypologie = $("select[id='Lien" + splitHtmlChar + id + "']").val();

    if (typologie == "") {
        $("select[id='Typologie" + splitHtmlChar + id + "']").addClass("requiredField");
        bError = true;
    }

    if (libTypologie == "") {
        $("input[id='LibTypologie" + splitHtmlChar + id + "']").addClass("requiredField");
        bError = true;
    }

    if (lienTypologie == "") {
        $("select[id='Lien" + splitHtmlChar + id + "']").addClass("requiredField");
        bError = true;
    }

    if (codeGrille == "") {
        $("#CodeGrille").addClass("requiredField");
        bError = true;
    }

    if (bError)
        return false;

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/SaveLineGrille",
        data: {
            codeGrille: codeGrille, libelleGrille: libelleGrille, newGrille: $("#newGrille").val(),
            typologie: typologie, libTypologie: libTypologie, lienTypologie: lienTypologie, ordreTypologie: $("input[id='ordreGrille" + splitHtmlChar + id + "']").val()
        },
        success: function (data) {
            var msgError = AlbJsSplitElem(data, 1, 'ERROR');
            if (msgError == "noData") {
                OpenInfoGrille(codeGrille);
            }
            else {
                common.dialog.error(msgError);
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Supprime la ligne de la grille----------
function DeleteLineGrille() {
    var splitHtmlChar = $("#SplitHtmlChar").val();
    var id = $("#delIdLine").val();
    var codeGrille = $("#CodeGrille").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/DeleteLineGrille",
        data: { codeGrille: codeGrille, ordreTypologie: $("input[id='ordreGrille" + splitHtmlChar + id + "']").val() },
        success: function (data) {
            $("#delIdLine").val("");
            OpenInfoGrille(codeGrille);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------Switch le mode d'affichage de la ligne de grille--------
function ToggleModeGrille(elem) {
    var splitHtmlChar = $("#SplitHtmlChar").val();
    var id = AlbJsSplitElem(elem.attr("id"), 1, splitHtmlChar);
    $("td[albmode='update'][albinfogrille='" + id + "']").toggle();
    $("td[albmode='read'][albinfogrille='" + id + "']").toggle();
    if ($("img[id='updGrille" + splitHtmlChar + id + "']").is(":visible")) {
        $("#addGrille").hide();
        $("#blkGrille").show();
        $("img[name='updGrille']").hide();
        $("img[name='blkLineGrille']").show();
        $("img[id='blkLineGrille" + splitHtmlChar + id + "']").hide();
        $("img[name='delGrille']").hide();
        $("img[id='svgGrille" + splitHtmlChar + id + "']").show();
        $("img[id='cancelGrille" + splitHtmlChar + id + "']").show();
        $("#btnGrilleValider").attr("disabled", "disabled");
    }
    else {
        $("#addGrille").show();
        $("#blkGrille").hide();
        $("img[name='updGrille']").show();
        $("img[name='blkLineGrille']").hide();
        DisplayDelLineBtn();
        $("img[id='svgGrille" + splitHtmlChar + id + "']").hide();
        $("img[id='cancelGrille" + splitHtmlChar + id + "']").hide();
        $("#btnGrilleValider").removeAttr("disabled");
    }
}
//---------Ouvre la div des sélections de valeurs---------
function OpenSelectionValeur(niveau) {
    ShowLoading();
    var splitHtmlChar = $("#SplitHtmlChar").val();
    var lien = $("select[id='Lien" + splitHtmlChar + niveau + "']").val();
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/OpenSelectionValeur",
        data: { codeGrille: $("#CodeGrille").val(), typoGrille: $("#Typologie").val(), niveau: niveau, lien: lien },
        success: function (data) {
            $("#divSelectionValeurs").show();
            $("#divDataSelectionValeurs").html(data);
            MapSelValeursElement();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Map les éléments de la div des sélections de valeurs--------
function MapSelValeursElement() {
    AlternanceLigne("BodyValeur", "", false, null);
    $("#btnSelValCancel").die().live('click', function () {
        $("#divSelectionValeurs").hide();
        $("#divDataSelectionValeurs").html("");
    });

    $("#btnSelValSave").die().live('click', function () {
        if (!$(this).is(":disabled"))
            SaveValeurs();
    });

    $("#checkAll").die().live('click', function () {
        if ($(this).is(':checked'))
            $("input[type='checkbox'][name='checkValeur']").attr("checked", "checked");
        else
            $("input[type='checkbox'][name='checkValeur']").removeAttr("checked");
    });

    $("#Filtre").die().live('change', function () {
        DisplayValeur($(this).val());
    });

    $("span[albnivinfo]").each(function () {
        $(this).click(function () {
            OpenSelectionValeur($(this).attr("albnivinfo"));
        });
    });

    $("#btnSearchFiltre").die().live('click', function () {
        SearchValeurNomenclature();
    });

    var valeurSelect = $("select[id='Valeur" + $("#SplitHtmlChar").val() + $("#NiveauValeur").val() + "']").val();
    if (valeurSelect == "" && $("#LienValeur").val() != "I" && $("#LienValeur").val() != "1") {
        $("#tblBodyValeur").hide();
        $("#btnSelValSave").attr("disabled", "disabled");
    }

    var maxDDL = parseInt($("#MaxDDL").val());
    $("td[id='tdLstValeur" + $("#SplitHtmlChar").val() + (maxDDL + 1) + "']").html("");

    $("select[name='NiveauValeur']").each(function () {
        $(this).change(function () {
            ChangeValeurTypologie($(this));
        });
    });
}
//----------Affiche les valeurs suivant le filtre-----------
function DisplayValeur(filtre) {
    switch (filtre) {
        case "":
            $("input[type='checkbox'][name='checkValeur']").parent().parent().show();
            break;
        case "C":
            $("input[type='checkbox'][name='checkValeur']:not(:checked)").parent().parent().hide();
            $("input[type='checkbox'][name='checkValeur']:checked").parent().parent().show();
            break;
        case "N":
            $("input[type='checkbox'][name='checkValeur']:not(:checked)").parent().parent().show();
            $("input[type='checkbox'][name='checkValeur']:checked").parent().parent().hide();
            break;
    }
}
//----------Sauvegarde les valeurs---------------
function SaveValeurs() {
    var splitHtmlChar = $("#SplitHtmlChar").val();
    var codeGrille = $("#CodeGrilleValeur").val();
    var typologie = $("#TypologieValeur").val();
    var niveau = $("#NiveauValeur").val();
    var niveauMere = niveau == "1" ? "0" : $("select[id='Valeur" + splitHtmlChar + (parseInt(niveau) - 1) + "']").val();

    var selVal1 = "";
    var selVal2 = "";
    var selVal3 = "";
    var selVal4 = "";
    var selVal5 = "";
    var selVal6 = "";
    var selVal7 = "";
    var selVal8 = "";
    var selVal9 = "";
    var selVal10 = "";

    //***   Remplissage des variables des valeurs   ***//
    $("input[type='checkbox'][name='checkValeur']:checked").each(function () {
        var idNomen = AlbJsSplitElem($(this).attr('id'), 1, '_');
        var codeNomen = AlbJsSplitElem($(this).attr('id'), 2, '_');
        if (idNomen != "noData") {
            if (selVal1.length < 250 && (selVal1 + idNomen + "|").length < 250) {
                selVal1 += idNomen + "|";
            }
            else {
                if (selVal2.length < 250 && (selVal2 + idNomen + "|").length < 250) {
                    selVal2 += idNomen + "|";
                }
                else {
                    if (selVal3.length < 250 && (selVal3 + idNomen + "|").length < 250) {
                        selVal3 += idNomen + "|";
                    }
                    else {
                        if (selVal4.length < 250 && (selVal4 + idNomen + "|").length < 250) {
                            selVal4 += idNomen + "|";
                        }
                        else {
                            if (selVal5.length < 250 && (selVal5 + idNomen + "|").length < 250) {
                                selVal5 += idNomen + "|";
                            }
                            else {
                                if (selVal6.length < 250 && (selVal6 + idNomen + "|").length < 250) {
                                    selVal6 += idNomen + "|";
                                }
                                else {
                                    if (selVal7.length < 250 && (selVal7 + idNomen + "|").length < 250) {
                                        selVal7 += idNomen + "|";
                                    }
                                    else {
                                        if (selVal8.length < 250 && (selVal8 + idNomen + "|").length < 250) {
                                            selVal8 += idNomen + "|";
                                        }
                                        else {
                                            if (selVal9.length < 250 && (selVal9 + idNomen + "|").length < 250) {
                                                selVal9 += idNomen + "|";
                                            }
                                            else {
                                                if (selVal10.length < 250 && (selVal10 + idNomen + "|").length < 250) {
                                                    selVal10 += idNomen + "|";
                                                }
                                                else {
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    });

    ShowLoading();

    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/SaveValeurs",
        data: {
            codeGrille: codeGrille, typologie: typologie, niveau: niveau, niveauMere: niveauMere,
            selVal1: selVal1, selVal2: selVal2, selVal3: selVal3, selVal4: selVal4, selVal5: selVal5,
            selVal6: selVal6, selVal7: selVal7, selVal8: selVal8, selVal9: selVal9, selVal10: selVal10
        },
        success: function (data) {
            common.dialog.info("Enregistrement effectué.");
            if (parseInt(niveau) <= parseInt($("#MaxDDL").val()))
                ReloadListValeurs(niveau, niveauMere);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------Recherche les valeurs de nomenclatures------------
function SearchValeurNomenclature() {
    var splitHtmlChar = $("#SplitHtmlChar").val();
    var codeGrille = $("#CodeGrilleValeur").val();
    var typologie = $("#TypologieValeur").val();
    var niveau = $("#NiveauValeur").val();
    var searchTerm = $("#SearchFiltre").val();

    var idMere = niveau != "1" ? $("select[id='Valeur" + splitHtmlChar + (parseInt(niveau) - 1) + "']").val() : 0;

    ShowLoading();

    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/SearchValeurNomenclature",
        data: { codeGrille: codeGrille, typologie: typologie, idMere: idMere, searchTerm: searchTerm },
        success: function (data) {
            $("#tblBodyValeur").html(data);
            AlternanceLigne("BodyValeur", "", false, null);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//-----------Changement de valeur de typologie----------
function ChangeValeurTypologie(elem) {
    var splitCharHtml = $("#SplitHtmlChar").val();
    var niveauTypo = AlbJsSplitElem(elem.attr("id"), 1, splitCharHtml);
    var currentNiveau = $("#NiveauValeur").val(); // utile ? //

    if (elem.val() == "") {
        if (parseInt(niveauTypo) == 1)
            LoadValeurs((parseInt(niveauTypo) - 1), 0);
        else {
            var idMere = $("select[id='Valeur" + splitCharHtml + (parseInt(niveauTypo) - 1) + "']").val();
            LoadValeurs(idMere != undefined ? (parseInt(niveauTypo) - 1) : parseInt(niveauTypo), idMere != undefined ? idMere : 0);
        }
    }
    else {
        LoadValeurs(niveauTypo, elem.val());
    }
    RemoveInfoListValeurs(niveauTypo);
}
//--------Charge les valeurs de la typologie------------
function LoadValeurs(niveauTypo, idMere) {
    ShowLoading();
    var splitCharHtml = $("#SplitHtmlChar").val();
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/LoadValeurs",
        data: { codeGrille: $("#CodeGrille").val(), idMere: idMere, niveau: niveauTypo },
        success: function (data) {
            var currentLien = $("select[id='Valeur" + splitCharHtml + niveauTypo + "']").attr("alblien");
            var currentNiv = idMere != "0" ? (parseInt(niveauTypo) + 1) : niveauTypo != "0" ? parseInt(niveauTypo) : 1;
            $("#tblBodyValeur").html(data);
            AlternanceLigne("BodyValeur", "", false, null);
            $("#tblBodyValeur").show();
            $("#btnSelValSave").removeAttr("disabled");
            if (parseInt(currentLien) < parseInt($("#MaxDDL").val())) 
                LoadListValeurs(niveauTypo, idMere); 
            $(".selectedTypo").removeClass("selectedTypo");
            $("td[id='tdCodeTypo" + splitCharHtml + currentNiv + "']").addClass("selectedTypo");
            $("#TypologieValeur").val($("td[id='tdCodeTypo" + splitCharHtml + currentNiv + "']").attr("albtypologie"));
            $("#NiveauValeur").val(currentNiv);
            $("#SearchFiltre").val("");
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Charge la liste déroulante supérieure---------
function LoadListValeurs(niveauTypo, idMere) {
    ShowLoading();
    var splitCharHtml = $("#SplitHtmlChar").val();
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/LoadListValeurs",
        data: { codeGrille: $("#CodeGrille").val(), idMere: idMere, niveau: niveauTypo },
        success: function (data) {
            $("td[id='tdLstValeur" + splitCharHtml + (parseInt(niveauTypo) + 1) + "']").html(data);
            $("select[id='Valeur" + splitCharHtml + (parseInt(niveauTypo) + 1) + "']").change(function () {
                ChangeValeurTypologie($(this));
            });
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Supprime les infos des listes déroulantes supérieures--------
function RemoveInfoListValeurs(niveauTypo) {
    var splitCharHtml = $("#SplitHtmlChar").val();
    $("select[name='NiveauValeur']").each(function () {
        var niv = AlbJsSplitElem($(this).attr("id"), 1, splitCharHtml);
        if (parseInt(niv) > niveauTypo)
            $(this).html("");
    });
}
//--------Recharge la liste déroulante------
function ReloadListValeurs(niveauTypo, idMere) {
    ShowLoading();
    var splitCharHtml = $("#SplitHtmlChar").val();
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/ReloadListValeurs",
        data: { codeGrille: $("#CodeGrille").val(), idMere: idMere, niveau: niveauTypo },
        success: function (data) {
            $("td[id='tdLstValeur" + splitCharHtml + (parseInt(niveauTypo)) + "']").html(data);
            $("select[id='Valeur" + splitCharHtml + (parseInt(niveauTypo)) + "']").change(function () {
                ChangeValeurTypologie($(this));
            });
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function filterByLabel() {
    var libelle = $("#txtLibelle").val().trim().toLowerCase();
    var reset = false;

    if (libelle === "")
        reset = true;
    
    $("#tblBodyNomenclature").find("tbody>tr").each(function () {
        var text = $(this).find("td:nth-child(3)")[0].innerText;
        var tr = $(this);
        if (reset && tr.css("display") === "none") {
            tr.css("display", "table-row");
        }
        else if (text.toLowerCase().search(libelle) === -1)
            tr.css("display", "none");
        else
            tr.css("display", "table-row");
    });

    AlternanceLigneAffiche("BodyNomenclature", "selNomenclatureCode", true, null);
}