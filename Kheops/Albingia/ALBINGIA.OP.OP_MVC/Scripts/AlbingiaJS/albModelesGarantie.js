
$(document).ready(function () {
    MapElementPage();
});
//--------Map les éléments de la page---------
function MapElementPage() {
    $("#btnRechercher").die().live('click', function () {
        rechercherModelesGarantie();
    });
    rechercherModelesGarantie();

    //-------------------- Initialise les action pour la fancy box.
    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        supprimerModelesGarantie();
        //switch ($("#hiddenAction").val()) {
        //    case "etablirAffNouv":
        //        offreANList.createAN();
        //        $("#NouveauContrat_DateAccord").removeClass(".requiredField");
        //        break;
        //    case "CopyAll":
        //        offreANList.createANCopyAllInfo();
        //        break;
        //}
        //$("#hiddenAction").clear();
    });
    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
    });
}
//------------Retour de la consultation ModelesGarantie---------------
function afficheConsultModelesGarantie(e, data, isNew) {
    e.html(data);
    e.show();
    $('#btnEnregistrer').click(function (evt) {
        enregistrerModelesGarantie(isNew);
    });
    $('#btnCopier').click(function (evt) {
        $("#CodeCopieRow, #btnValider, #btnAnnuler").show();
        $("#btnCopier").hide();
    });
    $('#btnValider').click(function (evt) {
        copierModelesGarantie();
    });
    $('#btnAnnuler').click(function (evt) {
        $("#CodeCopieRow, #btnValider, #btnAnnuler").hide();
        $("#btnCopier").show();
    });
    $('#btnGarantieType').click(function (evt) {
        ShowLoading();
        window.location.href = "/GarantieType/Index?codeModele=" + $("#Code").val();
    });
}


//-------------Ajoute un ModelesGarantie---------------------
function ajouterModelesGarantie() {
    afficherInfoModelesGarantie("-", false, true);
    AlternanceLigne("Modele", "Code", true, null);
    //ShowLoading();
    //$.ajax({
    //    type: "POST",
    //    url: "/ModelesGarantie/InitialiserModelesGarantie",
    //    context: $("#divInfoModelesGarantie"),
    //    success: function (data) {
    //        afficheConsultModelesGarantie($(this), data);
    //        CloseLoading();
    //    },
    //    error: function (request) {
    //        common.error.showXhr(request);
    //    }
    //});
}
//--------------Lance la recherche des ModelesGarantie--------------
function rechercherModelesGarantie() {
    var codeModelesGarantie = $("#RechercheModelesGarantie_Code").val();
    var descrModelesGarantie = $("#RechercheModelesGarantie_Description").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GarantieModele/Recherche",
        context: $("#divBodyModelesGarantie"),
        data: { code: codeModelesGarantie, description: descrModelesGarantie },
        success: function (data) {
            $(this).html(data);
            $("#divLstModelesGarantie").show();
            affecterClick();
            AlternanceLigne("Modele", "Code", true, null);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Affiche le résutat de la recherche des ModelesGarantie--------------
function afficherModelesGarantie() {
    $("#divLstModelesGarantie").show();
}

//--------------Affecte la fonction du click sur les td Code--------------
function affecterClick() {
    $(".linkModelesGarantie").each(function () {
        $(this).click(function () {
            afficherInfoModelesGarantie($.trim($(this).parent().attr("id")), true, false);
            AlternanceLigne("Modele", "Code", true, null);
        });
    });
    $("img[name=modifModele]").each(function () {
        $(this).click(function () {
            afficherInfoModelesGarantie($.trim($(this).attr("id")), false, false);
            AlternanceLigne("Modele", "Code", true, null);
        });
    });
    $("img[name=supprModele]").each(function () {
        $(this).click(function () {
            $("#SupprModele").val($(this).attr("id"));
            ShowCommonFancy("Confirm", "DelModele", "Etes-vous sûr de vouloir supprimer le modèle " + $(this).closest("tr").attr("id").split("-")[1] + " ?", 350, 80, true, true);
        });
    });
}
//--------------Affiche les infos du ModelesGarantie----------------------------
function afficherInfoModelesGarantie(code, readonly, isNew) {
    code = code.split("-")[1];
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GarantieModele/ConsultModelesGarantie",
        context: $("#divInfoModelesGarantie"),
        data: { code: code, readOnly: readonly, isNew: isNew },
        success: function (data) {
            afficheConsultModelesGarantie($(this), data, isNew);
            AlternanceLigne("ListModeleGarantie", "", false);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------------Envoi de la form pour l'enregistrement---------------------
function enregistrerModelesGarantie(isNew) {
    ShowLoading();
    $("#Code").val($("#Code").val().replace(/ /g, ''));
    
    var code = $.trim($("#Code").val());
    var description = $("#Description").val();

    if (code == "" || description == "") {
        $("#Code").addClass("requiredField");
        $("#Description").addClass("requiredField");
        CloseLoading();
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/GarantieModele/Enregistrer",
        data: {
            code: code, description: description, isNew : isNew
        },
        success: function (data) {
            if (data == "") {
                rechercherModelesGarantie();
                $("#divInfoModelesGarantie").hide();
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
//----------------------Copie de modele---------------------
function copierModelesGarantie() {
    ShowLoading();
    $("#Code").val($("#Code").val().replace(/ /g, ''));
    $("#CodeCopie").val($("#CodeCopie").val().replace(/ /g, ''));

    var code = $.trim($("#Code").val());
    var codeCopie = $.trim($("#CodeCopie").val());
    $.ajax({
        type: "POST",
        url: "/GarantieModele/Copier",
        data: {
            code: code, codeCopie: codeCopie
        },
        success: function (data) {
            if (data == "") {
                rechercherModelesGarantie();
                $("#divInfoModelesGarantie").hide();
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

//-------------Suppression d'un modele-----------------
function supprimerModelesGarantie(e) {
    ShowLoading();
    var code = $("#SupprModele").val();
    $.ajax({
        type: "POST",
        url: "/GarantieModele/Supprimer",
        context: $("#divInfoModelesGarantie"),
        data: { code: code.split("-")[1] },
        success: function (data) {
            if (data == "") {
                $("#" + code).closest("tr").remove();
                $(this).hide();
                AlternanceLigne("Modele", "Code", true, null);
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
    $("#SupprModele").val("");
}


function Annuler() {
    window.location.href = "/BackOffice/Index";
}