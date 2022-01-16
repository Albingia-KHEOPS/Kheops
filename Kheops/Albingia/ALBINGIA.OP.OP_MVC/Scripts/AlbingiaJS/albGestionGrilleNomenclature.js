$(document).ready(function () {
    MapElementPage();
});
//--------Map les éléments de la page-----------
function MapElementPage() {
    MapListeGrille();

    $("#btnCancel").die().live('click', function () {
        Redirection("BackOffice", "Index");
    });

    $("#addGrille").die().live('click', function () {
        $("#tblBodyGrille > tbody  > tr").removeClass("selectLine");
        OpenGrille(0);
    });

    $("#btnConfirmOk").die().live('click', function () {
        var action = $("#hiddenAction").val();
        switch (action) {
            case "Delete":
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

    $("#btnSearchGrille").die().live('click', function () {
        LoadListGrilles();
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
//--------Charge la liste des grilles-------
function LoadListGrilles(code) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/LoadListGrille",
        data: { searchGrille: $("#searchGrille").val() },
        success: function (data) {
            var splitHtmlChar = $("#SplitHtmlChar").val();
            $("#divListGrilles").html(data);
            MapListeGrille();
            if (code != "" && code != undefined) {
                $("td[id='codeGrille" + splitHtmlChar + code.toUpperCase() + "'][albOpenGrille='" + code.toUpperCase() + "']").trigger('click');
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Map les élements de la liste des grilles-----
function MapListeGrille() {
    AlternanceLigne("BodyGrille", "selGrilleCode", true, null);

    $("td[albOpenGrille]").each(function () {
        $(this).click(function () {
            OpenGrille($(this).attr('albOpenGrille'));
            $("#selGrilleCode").val($(this).attr('albOpenGrille'));
            $(this).parent().parent().children(".selectLine").removeClass("selectLine");
            $(this).parent().addClass("selectLine");
        });
    });
}
//--------Ouvre la div d'ajout/maj de grille-------
function OpenGrille(idGrille) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestionGrilleNomenclature/OpenGrille",
        data: { idGrille: idGrille },
        success: function (data) {
            $("#divInfoGrille").html(data);
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
        $("#divInfoGrille").html($("#divEmptyGrille").html());
        $("#tblBodyGrille > tbody  > tr").removeClass("selectLine");
    });
    $("#btnGrilleSupprimer").die().live('click', function () {
        ShowCommonFancy("Confirm", "Delete", "Etes-vous certain de vouloir supprimer cette grille ?", 300, 80, true, true, true);
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
        //data: { codeGrille: codeGrille, libelleGrille: libelleGrille, typologieGrille: $("#Typologie").val(), newGrille: $("#newGrille").val() },
        success: function (data) {
            var msgError = AlbJsSplitElem(data, 1, 'ERROR');
            if (msgError == "noData") {
                $("#divInfoGrille").html("");
                LoadListGrilles(codeGrille);
                //Mise en commentaire pour reloader la liste des grilles//
                //OpenGrille(codeGrille);
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
            $("#divInfoGrille").html("");
            LoadListGrilles();
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
                OpenGrille(codeGrille);
                $("#addGrille").show();
                $("#blkGrille").hide();
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
            OpenGrille(codeGrille);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
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
    //$("td[id='tdLstValeur" + $("#SplitHtmlChar").val() + (maxDDL + 1) + "']").html("");
    $("td[albidddl='" + (maxDDL + 1) + "']").html("");

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