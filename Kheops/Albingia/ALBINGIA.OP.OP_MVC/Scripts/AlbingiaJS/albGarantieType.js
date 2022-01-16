
$(document).ready(function () {
    MapElementPage();
});
//--------Map les éléments de la page---------
function MapElementPage() {
    $("#btnRechercher").die().live('click', function () {
        filtrerGarantieType();
    });
    rechercherGarantieType();

    //-------------------- Initialise les action pour la fancy box.
    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        supprimerGarantieType();
        
    });
    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
    });


    $("#divInfoGarantieType").on("change", "#ListLCI_2__Unite, #ListLCI_3__Unite, #ListLCI_4__Unite, #ListLCI_5__Unite", function () {
        if ($(this).data("oldvalue") == "CPX" || ($(this).data("oldvalue") != "CPX" && $(this).val() == "CPX")) {
            ShowLoading();
            var tr = $(this).closest("tr");
            $.ajax({
                type: "GET",
                url: "/GarantieType/GetBase",
                data: { type: tr.find("input[name$='Type']").val(), unite: $(this).val() },
                success: function (data) {
                    var htmlOptions = "";
                    data.forEach(exp => {
                        htmlOptions += "<option value='" + exp.Code + "' title='" + exp.Code + " - " + exp.Libelle + "'>" + exp.Code + " - " + exp.Libelle + "</option>"
                    });


                    tr.find("select[name$='Base']").html(htmlOptions);
                    //tr.html(data);
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        $(this).data("oldvalue", $(this).val());
    });
}
//------------Retour de la consultation GarantieType---------------
function afficheConsultGarantieType(e, data, isNew) {
    e.html(data);
    e.show();
    $('#btnEnregistrer').click(function (evt) {
        enregistrerGarantieType(isNew);
    });
}


//-------------Ajoute une Garantie Type---------------------
function ajouterGarantieType() {
    afficherInfoGarantieType("tr-0", false, true);
    AlternanceLigne("Type", "Code", true, null);
}

function ajouterSousGarantieType(seq) {
    afficherInfoGarantieType("tr-" + seq, false, true);
    AlternanceLigne("Type", "Code", true, null);
}
//--------------Lance la recherche des GarantieType--------------
function rechercherGarantieType() {
    var codeGarantieModele = $("#RechercheGarantieType_CodeModele").val();
    var isModifiable = $("#IsModifiable").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GarantieType/Recherche",
        context: $("#tblType"),
        data: { codeModele: codeGarantieModele, isModifiable: isModifiable },
        success: function (data) {
            $(this).html(data);
            $("#divLstGarantieType").show();
            affecterClick();
            AlternanceLigne("Type", "Code", true, null);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function filtrerGarantieType() {
    var codeGarantieType = $("#RechercheGarantieType_Code").val();
    var nivGarantieType = $("#RechercheGarantieType_Niveau").val();

    $("#tblType tr").each(function () {
        if ((nivGarantieType == "" || $(this).data("niveau") == nivGarantieType)
            && (codeGarantieType == "" || $(this).data("codegar").toUpperCase().indexOf(codeGarantieType.toUpperCase()) > -1)) {
            $(this).show()
        } else {
            $(this).hide();
        }
    });
}
//--------------Affiche le résutat de la recherche des ModelesGarantie--------------
//function afficherGarantieType() {
//    $("#divLstGarantieType").show();
//}

//--------------Affecte la fonction du click sur les td Code--------------
function affecterClick() {
    $(".linkGarantieType").each(function () {
        $(this).click(function () {
            afficherInfoGarantieType($.trim($(this).parent().attr("id")), true, false);
            AlternanceLigne("Type", "Code", true, null);
        });
    });
    $("img[name=modifType]").each(function () {
        $(this).click(function () {
            afficherInfoGarantieType($.trim($(this).attr("id")), false, false);
            AlternanceLigne("Type", "Code", true, null);
        });
    });
    $("img[name=supprType]").each(function () {
        $(this).click(function () {
            $("#SupprType").val($(this).attr("id"));
            ShowCommonFancy("Confirm", "DelType", "Etes-vous sûr de vouloir supprimer cette garantie ? Cela supprimera également toutes les sous-garanties qui lui sont rattachées (s'il y en a).", 350, 80, true, true);
        });
    });
}
//--------------Affiche les infos du ModelesGarantie----------------------------
function afficherInfoGarantieType(seq, readonly, isNew) {
    seq = seq.split("-")[1];
    var codeModele = $("#RechercheGarantieType_CodeModele").val();
    var ord = 0;
    $("#tblType td[name='ordre']").each(function () {
        var value = parseInt($(this).data('ordre'));
        ord = (value > ord) ? value : ord;
    });
    ShowLoading();
    var mustCloseLoading = false;
    $.ajax({
        type: "POST",
        url: "/GarantieType/ConsultGarantieType",
        context: $("#divInfoGarantieType"),
        data: { seq: seq, codeModele: codeModele, ord: ord, readOnly: readonly, isNew: isNew },
        success: function (data) {
            afficheConsultGarantieType($(this), data, isNew);
            AlternanceLigne("LCI", "", false);
            common.autonumeric.applyAll('init', 'decimal', null, null, 2, '999999999.99', '0');
            common.autonumeric.apply($("#Ordre"), 'init', 'numeric', null, null, null, '999', '0');
            common.autonumeric.apply($("#GroupeAlternative"), 'init', 'numeric', null, null, null, '99', '0');
            if (mustCloseLoading) { CloseLoading(); }
            else { mustCloseLoading = true; }
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
    $.ajax({
        type: "POST",
        url: "/GarantieType/ConsultGarantieTypeLien",
        context: $("#divInfoGarantieTypeLien"),
        data: { seq: seq,  readOnly: readonly },
        success: function (data) {
            $(this).html(data);
            $(this).show();
            AlternanceLigne("Association", "", false);
            AlternanceLigne("Dependance", "", false);
            AlternanceLigne("Incompatibilite", "", false);
            $(".divHeight").css("height", "245px");
            $(".GroupHeightGarantieType").css("height", "400px");

            $('select#GarantieAssociation').select2({ dropdownAutoWidth: true });
            $('select#GarantieDependance').select2({ dropdownAutoWidth: true });
            $('select#GarantieIncompatibilite').select2({ dropdownAutoWidth: true });
            if (mustCloseLoading) { CloseLoading(); }
            else { mustCloseLoading = true; }
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
}
//----------------------Envoi de la form pour l'enregistrement---------------------
function enregistrerGarantieType(isNew) {
    var formData = new FormData(document.getElementById('frmMain'));
    formData.append("IsNew", isNew);

    if (formData.get("CodeGarantie") == "") {
        $("select#CodeGarantie").addClass("requiredField");
        return false;
    } else {
        $("select#CodeGarantie").removeClass("requiredField");
    }
    ShowLoading();

    $.ajax({
        type: "POST",
        url: "/GarantieType/Enregistrer",
        processData: false,
        contentType: false,
        data: formData,
        success: function (data) {
            if (data == "") {
                rechercherGarantieType();
                $("#divInfoGarantieType").hide();
                $("#divInfoGarantieTypeLien").hide();
                $(".divHeight").css("height", "445px");
                $(".GroupHeightGarantieType").css("height", "600px");
            } else {
                common.dialog.error(data);
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
}
//-------------Suppression d'un volet-----------------
function supprimerGarantieType(e) {
    ShowLoading();
    var seq = $("#SupprType").val();
    $.ajax({
        type: "POST",
        url: "/GarantieType/Supprimer",
        context: $("#divInfoGarantieType"),
        data: { seq: seq.split("-")[1] },
        success: function (data) {
            if (data == "") {
                $("tr[data-meregar='" + seq.split("-")[1] + "']:not(tr[data-meregar='0'])").each(function () {
                    $("tr[data-meregar='" + $(this).attr("id").split("-")[1] + "']:not(tr[data-meregar='0'])").each(function () {
                        $("tr[data-meregar='" + $(this).attr("id").split("-")[1] + "']:not(tr[data-meregar='0'])").each(function () {
                            $(this).remove();
                        });
                        $(this).remove();
                    });
                    $(this).remove();
                });
                $("#" + seq).closest("tr").remove();
                $(this).hide();
                AlternanceLigne("Type", "Code", true, null);
            }
            else {
                common.dialog.error(data);
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
    $("#SupprType").val("");
}

//---------------------- fonction pour ajouter/supprimer des liaisons garantie type ---------------------
function ajouterLien(type) {
    ShowLoading();
    var garantieB = "";
    var garantieBNom = "";
    var table;
    switch (type) {
        case 'A':
            garantieB = $("#GarantieAssociation").val();
            garantieBNom = $("#GarantieAssociation option:selected").text();
            table = $("#tblAssociation");
            break;
        case 'D':
            garantieB = $("#GarantieDependance").val();
            garantieBNom = $("#GarantieDependance option:selected").text();
            table = $("#tblDependance");
            break;
        case 'I':
            garantieB = $("#GarantieIncompatibilite").val();
            garantieBNom = $("#GarantieIncompatibilite option:selected").text();
            table = $("#tblIncompatibilite");
            break;
        default:
            common.dialog.error("Le type de liaison n'est pas reconnu");
            break;
    }
    var garantieA = $("#NumeroSeq").val();
    $.ajax({
        type: "POST",
        url: "/GarantieType/AjouterGarantieTypeLien",
        context: $("#divInfoGarantieType"),
        data: { type: type, seqA: garantieA, seqB: garantieB },
        success: function (data) {
            if (data == "") {
                var html = '<tr id="tr-' + type + '-' + garantieA + '-' + garantieB + '">'
                         + '    <td class="tdBodyLienGarantie" >' + garantieBNom + '</td >'
                         + '    <td class="supprGarantieTypeLien tdBodyImgLien CursorPointer">'
                         + '        <img id="suppr-' + type + '-' + garantieA + '-' + garantieB + '" src="/Content/Images/poubelle1616.png" alt="Supprimer" title="Supprimer" name="supprLien" onclick="supprimerLien(\'' + type + '\', ' + garantieA + ', ' + garantieB + ');" />'
                         + '    </td>'
                         + '</tr >';
                table.append(html);
                AlternanceLigne("Association", "", false);
                AlternanceLigne("Dependance", "", false);
                AlternanceLigne("Incompatibilite", "", false);
            }
            else {
                common.dialog.error(data);
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
}
function supprimerLien(type, garantieA, garantieB) {
    ShowLoading();
    var table;
    switch (type) {
        case 'A':
            table = $("#tblAssociation");
            break;
        case 'D':
            table = $("#tblDependance");
            break;
        case 'I':
            table = $("#tblIncompatibilite");
            break;
        default:
            common.dialog.error("Le type de liaison n'est pas reconnu");
            break;
    }
    $.ajax({
        type: "POST",
        url: "/GarantieType/SupprimerGarantieTypeLien",
        context: $("#divInfoGarantieType"),
        data: { type: type, seqA: garantieA, seqB: garantieB },
        success: function (data) {
            if (data == "") {
                table.find("#tr-" + type + "-" + garantieA + "-" + garantieB).remove();
                AlternanceLigne("Association", "", false);
                AlternanceLigne("Dependance", "", false);
                AlternanceLigne("Incompatibilite", "", false);
            }
            else {
                common.dialog.error(data);
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
}

function toggleLien(elem) {
    var nom = $(elem).attr("name");
    if (nom == "agrLien") {
        $(".divHeight").css("height", "85px");
        $(".GroupHeightGarantieType").css("height", "240px");

        $(".GroupHeightGarantieTypeLien").css("height", "345px");
        $(".GroupHeightGarantieTypeLienInterieur").css("height", "300px");
        $(".divAssociation, .divDependance, .divIncompatibilite").css("height", "290px");
        $(".divHeightLien").css("height", "240px");
        $(elem).attr("src", "/Content/Images/expand.png");
        $(elem).attr("name", "redLien");
        $(elem).attr("title", "Réduire");
    } else {
        $(".divHeight").css("height", "245px");
        $(".GroupHeightGarantieType").css("height", "400px");

        $(".GroupHeightGarantieTypeLien").css("height", "185px");
        $(".GroupHeightGarantieTypeLienInterieur").css("height", "140px");
        $(".divAssociation, .divDependance, .divIncompatibilite").css("height", "130px");
        $(".divHeightLien").css("height", "80px");
        $(elem).attr("src", "/Content/Images/collapse.png");
        $(elem).attr("name", "agrLien");
        $(elem).attr("title", "Agrandir");
    }
}

function Annuler() {
    window.location.href = "/GarantieModele/Index";
}