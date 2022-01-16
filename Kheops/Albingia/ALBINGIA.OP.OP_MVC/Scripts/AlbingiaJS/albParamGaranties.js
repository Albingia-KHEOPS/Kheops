$(document).ready(function () {
    MapElementsPage();
    RechercherGaranties();
});
//-------------Map les éléments de la page-------------
function MapElementsPage() {
    $("#btnRechercher").unbind();
    $("#btnRechercher").bind("click", function () {
        RechercherGaranties();
    });

    $("#btnAjouterGarantie").unbind();
    $("#btnAjouterGarantie").bind("click", function () {
        EditGarantie("");
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "SupprimerGarantie":
                var garantie = $("#idGarantieSuppr").val();
                SupprimerGarantie(garantie);
                break;
            case "SupprimerTypeValeur":
                var typeValeur = $("#idTypeValeurSuppr").val();
                SupprimerTypeValeur(typeValeur);
                break;
            case "SupprimerFamilleReassurance":
                var familleReassurance = $("#idFamilleReassuranceSuppr").val();
                SupprimerFamilleReassurance(familleReassurance);
                break;
            case "SupprimerGarTypeRegul":
                var code = $("#idGarantieSuppr").val();
                var codeGarantie = code.split('_')[0];
                var codeTypeRegul = code.split('_')[1];
              
                DeleteTypeRegul(codeGarantie, codeTypeRegul);
                break;
        }
        $("#hiddenAction").val('');
        $("#idGarantieSuppr").val('');
        $("#idTypeValeurSuppr").val('');
        $("#idFamilleReassuranceSuppr").val('');
    });
    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
        $("#idGarantieSuppr").val('');
        $("#idTypeValeurSuppr").val('');
        $("#idFamilleReassuranceSuppr").val('');
    });

    $('#btnAnnulerRetour').unbind();
    $('#btnAnnulerRetour').bind('click', function () {
        Annuler();
    });

    $("#RechercheCode").unbind();
    $("#RechercheCode").bind('change', function () {
        Initialiser();
    });
    $("#RechercheDesignation").unbind();
    $("#RechercheDesignation").bind('change', function () {
        Initialiser();
    });

    MapCommonAutoCompGaranties();
}
//------------Map les lignes du tableau de garanties--------
function MapElementsTableau() {
    $("tr[name=ligneGarantie]").unbind();
    $("tr[name=ligneGarantie]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $(this).parent().children(".selectLine").removeClass("selectLine");
        $(this).addClass("selectLine");
        EditGarantie(id);
    });

    $("img[name=btnSupprimer]").unbind();
    $("img[name=btnSupprimer]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $("#idGarantieSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerGarantie", "Vous allez supprimer la garantie.<br/>Etes-vous sûr de vouloir continuer ?",
                  320, 150, true, true);
    });


    AlternanceLigne("GarantiesBody", "noInput", true, null);

}
//------------Map les éléments de la partie détails de la garantie---------
function MapElementsDetails() {
    $("#btnEnregistrer").unbind();
    $("#btnEnregistrer").bind("click", function () {
        EnregistrerGarantie();
    });

    MapTypeRegul();
    if (!$("#IsRegularisable").is(':checked')) {
        $("#divTypeRegul").css("display", "none");
    }
    else {
        $("#divTypeRegul").css("display", "block");
    }

    $("#IsRegularisable").unbind();
    $("#IsRegularisable").bind("click", function () {
        if (!$("#IsRegularisable").is(':checked')) {
            $("#CodeTypeGrille").attr('disabled', 'disabled');
            $("#CodeTypeGrille").addClass('readonly');
            $("#divTypeRegul").css("display","none");
        }
        else {
            $("#CodeTypeGrille").removeAttr('disabled');
            $("#CodeTypeGrille").removeClass('readonly');
            $("#divTypeRegul").css("display", "block");
        }
    });
    $("#IsLieInventaire").unbind();
    $("#IsLieInventaire").bind("click", function () {
        if (!$("#IsLieInventaire").is(':checked')) {
            $("#CodeTypeInventaire").attr('disabled', 'disabled');
            $("#CodeTypeInventaire").addClass('readonly');
        }
        else {
            $("#CodeTypeInventaire").removeAttr('disabled');
            $("#CodeTypeInventaire").removeClass('readonly');
        }
    });

    $("#btnTypesValeur").unbind();
    $("#btnTypesValeur").bind("click", function () {
        EditTypesValeur();
    });
    $("#btnReassurance").unbind();
    $("#btnReassurance").bind("click", function () {
        EditFamillesReassurance();
    });

   
}

//------------Map les éléments de la partie types de valeur de la garantie---------
function MapElementsTypesValeur() {
    $('#AjouterTypeValeur').unbind();
    $('#AjouterTypeValeur').bind('click', function () {
        EditTypeValeur('');
        $("tr[name=trTypeValeurEdition]").hide();
        $("tr[name=trTypeValeurReadOnly]").show();
    });
    $('#btnFermerTypesValeur').unbind();
    $('#btnFermerTypesValeur').bind('click', function () {
        $("#divDataTypesValeur").clearHtml();
        $("#divTypesValeur").hide();
    });
    $("img[name=updateTypeValeur]").unbind();
    $("img[name=updateTypeValeur]").bind('click', function () {
        var id = $(this).attr("id").split("_")[1];
        EnregistrerTypeValeur(id);
    });
    $("tr[name=trTypeValeurReadOnly]").unbind();
    $("tr[name=trTypeValeurReadOnly]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $(this).parent().children(".selectLine").removeClass("selectLine");
        $(this).addClass("selectLine");
        $("tr[name=trTypeValeurEdition]").hide();
        $("tr[name=trTypeValeurReadOnly]").show();
        $("#divPaneauDynamic").hide();
        EditTypeValeur(id);
    });

    $("img[name=btnSupprimer]").unbind();
    $("img[name=btnSupprimer]").bind("click", function () {
        var id = $(this).attr("id").split("_")[1];
        $("#idTypeValeurSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerTypeValeur", "Vous allez supprimer le type de valeur.<br/>Etes-vous sûr de vouloir continuer ?",
                  320, 150, true, true);
    });

    AlternanceLigne("BodyTypesValeur", "noInput", true, null);
    formatDecimal();

}
//------------Map les éléments de la partie familles de réassurence de la garantie---------
function MapElementsFamillesReassurance() {
    $('#AjouterFamilleReassurance').unbind();
    $('#AjouterFamilleReassurance').bind('click', function () {
        EditFamillesReassurance();
        EditFamilleReassurance('');
    });
    $('#btnFermerFamillesReassurance').unbind();
    $('#btnFermerFamillesReassurance').bind('click', function () {
        $("#divDataFamillesReassurance").clearHtml();
        $("#divFamillesReassurance").hide();
    });
    $("img[name=updateFamilleReassurance]").unbind();
    $("img[name=updateFamilleReassurance]").bind('click', function () {
        var id = $(this).attr("id");
        EnregistrerFamilleReassurance(id);
    });
    $("tr[name=trFamilleReassuranceReadOnly]").unbind();
    $("tr[name=trFamilleReassuranceReadOnly]").bind("click", function () {
        var id = $(this).attr("id");
        $(this).parent().children(".selectLine").removeClass("selectLine");
        $(this).addClass("selectLine");
        $("tr[name=trFamilleReassuranceEdition]").hide();
        $("tr[name=trFamilleReassuranceReadOnly]").show();
        $("#divPaneauDynamic").hide();
        $("#divPaneauDynamic").clearHtml();
        EditFamilleReassurance(id);
    });
    BrancheSelectedChanged();
    SousBrancheSelectedChanged('');
    CategorieSelectedChanged();
    AlternanceLigne("BodyFamillesReassurance", "noInput", true, null);

    $("img[name=btnSupprimer]").unbind();
    $("img[name=btnSupprimer]").bind("click", function () {
        var id = $(this).attr("id");
        $("#idFamilleReassuranceSuppr").val(id);
        ShowCommonFancy("Confirm", "SupprimerFamilleReassurance", "Vous allez supprimer la famille de réassurance.<br/>Etes-vous sûr de vouloir continuer ?",
                  320, 150, true, true);
    });
}
//-----------------Region Garantie---------------///

//-----------Lance la recherche des garanties------------
function RechercherGaranties() {
    var codeRecherche = $.trim($("#RechercheCode").val());
    var designationRecherche = $.trim($("#RechercheDesignation").val());
    var userRights = $("#AdditionalParam").val();
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/ParamGaranties/RechercheGaranties",
        data: { codeGarantie: codeRecherche, designationGarantie: designationRecherche, userRights: userRights },
        success: function (data) {
            $("#divGarantiesBody").html(data);
            MapElementsTableau();
            $("#VoletDetails").hide();
            common.page.isLoading = false;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Détails de la garantie (nouvelle ou sélectionnée)--------
function EditGarantie(codeGarantie) {
    var userRights = $("#AdditionalParam").val();
    if (codeGarantie != undefined) {
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/ParamGaranties/GetDetailsGarantie",
            data: { codeGarantie: codeGarantie, userRights: userRights },
            success: function (data) {
                $("#divGarantieDetails").html(data);
                MapElementsDetails();
                $("#VoletDetails").show();
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//-------------Enregistrer la garantie(Modification ou création)-----
function EnregistrerGarantie() {

    $("#codeGarantieEdit").removeClass("requiredField");
    $("#DesignationGarantieEdit").removeClass("requiredField");
    $("#CodeTaxe").removeClass("requiredField");
    $("#CodeCatNat").removeClass("requiredField");
    $("#CodeTypeGrille").removeClass("requiredField");
    $("#CodeTypeInventaire").removeClass("requiredField");

    let dataPost = {
        modeOperation: $("#ModeOperation").val(),
        codeGarantie: $("#CodeGarantieEdit").val().trim(),
        designationGarantie: $("#DesignationGarantieEdit").val().trim(),
        abrege: $("#Abrege").val().trim(),
        codeTaxe: $("#CodeTaxe").val(),
        codeCatNat: $("#CodeCatNat").val(),
        isGarantieCommune: $("#IsGarantieCommune").isChecked(),
        codeTypeDefinition: $("#CodeTypeDefinition").val(),
        codeTypeInformation: $("#CodeTypeInformation").val(),
        isRegularisable: $("#IsRegularisable").isChecked(),
        codeTypeGrille: $("#CodeTypeGrille").val(),
        isLieInventaire: $("#IsLieInventaire").isChecked(),
        isAttentatGareat: $("#IsAttentatGareat").isChecked(),
        codeTypeInventaire: $("#CodeTypeInventaire").val(),
        additionalParam: $("#AdditionalParam").val()
    };

    let ancienCodeTypeInventaire = $("#AncienCodeTypeInventaire").val();
    var verif = true;
    if (!dataPost.codeGarantie) {
        $("#codeGarantieEdit").addClass("requiredField");
        verif = false;
    }
    if (!dataPost.designationGarantie) {
        $("#DesignationGarantieEdit").addClass("requiredField");
        verif = false;
    }

    if (!dataPost.codeTaxe) {
        $("#CodeTaxe").addClass("requiredField");
        verif = false;
    }

    if (!dataPost.codeCatNat) {
        $("#CodeCatNat").addClass("requiredField");
        verif = false;
    }

    if (dataPost.isRegularisable && !dataPost.codeTypeGrille) {
        $("#CodeTypeGrille").addClass("requiredField");
        verif = false;
    }
    if (dataPost.isLieInventaire == "O" && !dataPost.codeInventaire) {
        $("#CodeTypeInventaire").addClass("requiredField");
        verif = false;
    }

    if (verif) {
        common.page.isLoading = true;
        common.$postJson("/ParamGaranties/EnregistrerGarantie", { modelSave: dataPost, ancienCodeTypeInventaire: ancienCodeTypeInventaire }, true).done(function (data) {
            let htmlData = $(data);
            let typeRetourData = htmlData.filter("input[id=TypeRetourData]").val();
            if (typeRetourData == "ListeGaranties") {
                $("#divGarantiesBody").html(data);
                if (dataPost.modeOperation == "I") {
                    $("#CodeGarantieEdit").attr('disabled', 'disabled');
                    $("#CodeGarantieEdit").addClass('readonly');
                    $("#btnTypesValeur").removeAttr('disabled');
                    $("#btnReassurance").removeAttr('disabled');
                    $("#ModeOperation").val('U');
                }
                $('#VoletDetails').css('display', 'none');
                MapElementsTableau();
            }
            else if (typeRetourData == "ConfirmModifTypeInventaire") {
                $("#divDataConfirmModifTypeInventaire").html(data);
                $("#divConfirmModifTypeInventaire").show();
                MapElementConfirmModifTypeInventaire();
            }
            common.page.isLoading = false;
        });
    }
}

//-------------Supprime la garantie-----------
function SupprimerGarantie(id) {
    var userRights = $("#AdditionalParam").val();
    if (id != "" && id != undefined) {
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/ParamGaranties/SupprimerGarantie",
            data: { codeGarantie: id, userRights: userRights },
            success: function (data) {
                $("#divGarantiesBody").html(data);
                MapElementsTableau();
                $("#VoletDetails").hide();
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}

//---------------Region types de valeur--------------//
//--------------Editer la liste des types de valeur de la garantie--------
function EditTypesValeur() {
    var codeGarantie = $("#CodeGarantieEdit").val();
    var userRights = $("#AdditionalParam").val();
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/ParamGaranties/EditTypesValeur",
        data: { codeGarantie: codeGarantie, userRights: userRights },
        success: function (data) {
            $("#divDataTypesValeur").html(data);
            DesactivateShortCut();
            AlbScrollTop();
            $("#divTypesValeur").show();
            MapElementsTypesValeur();
            common.page.isLoading = false;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function EnregistrerTypeValeur(id) {
    var codeGarantie = $("#CodeGarantieEdit").val();
    var userRights = $("#AdditionalParam").val();
    var numOrdre = $("#EditedNumOrdre").val();
    var codeTypeValeur = $("#TypeValeur").val();
    var verif = true;
    if (codeTypeValeur == "" || codeTypeValeur == undefined) {
        $("#TypeValeur").addClass("requiredField");
        verif = false;
    }
    if (numOrdre == "" || numOrdre == undefined) {
        $("#EditedNumOrdre").addClass("requiredField");
        verif = false;
    }
    if (verif) {
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/ParamGaranties/EnregistrerTypeValeur",
            data: {
                id: id, codeGarantie: codeGarantie, numOrdre: numOrdre, codeTypeValeur: codeTypeValeur,
                userRights: userRights
            },
            success: function (data) {
                $("#divBodyTypesValeur").html(data);
                $("#divPaneauDynamic").hide();
                $("tr[name=trTypeValeurEdition]").hide();
                $("tr[name=trTypeValeurReadOnly]").show();
                MapElementsTypesValeur();
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//-------------Détails de la garantie (nouvelle ou sélectionnée)--------
function EditTypeValeur(id) {
    var userRights = $("#AdditionalParam").val();
    var codeGarantie = $("#CodeGarantieEdit").val();
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/ParamGaranties/EditTypeValeur",
        data: { id: id, codeGarantie: codeGarantie, userRights: userRights },
        success: function (data) {
            if (id != '') {
                $("tr[id=trTypeValeurReadOnly_" + id + "]").hide();
                $("tr[id=trTypeValeurEdition_" + id + "]").show();
                $("tr[id=trTypeValeurEdition_" + id + "]").html(data);
            }
            else {
                $("#TypeValeur").val('');
                $("#divPaneauDynamic").html(data);
                $("#divPaneauDynamic").show();
            }
            MapElementsTypesValeur();
            common.page.isLoading = false;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Supprime le type de valeur-----------
function SupprimerTypeValeur(id) {
    var userRights = $("#AdditionalParam").val();
    var codeGarantie = $("#CodeGarantieEdit").val();
    if (id != "" && id != undefined) {
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/ParamGaranties/SupprimerTypeValeur",
            data: { id: id, codeGarantie: codeGarantie, userRights: userRights },
            success: function (data) {
                $("#divBodyTypesValeur").html(data);
                $("#divPaneauDynamic").hide();
                $("tr[name=trTypeValeurEdition]").hide();
                $("tr[name=trTypeValeurReadOnly]").show();
                MapElementsTypesValeur();
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//---------------Region types de valeur--------------//
//--------------Editer la liste des familles de réassurance de la garantie--------
function EditFamillesReassurance() {
    var codeGarantie = $("#CodeGarantieEdit").val();
    var userRights = $("#AdditionalParam").val();
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/ParamGaranties/EditFamillesReassurance",
        data: { codeGarantie: codeGarantie, userRights: userRights },
        success: function (data) {
            $("#divDataFamillesReassurance").html(data);
            DesactivateShortCut();
            AlbScrollTop();
            $("#divFamillesReassurance").show();
            MapElementsFamillesReassurance();
            common.page.isLoading = false;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Détails de la famille de reassurance (nouvelle ou sélectionnée)--------
function EditFamilleReassurance(id) {
    //trFamilleReassuranceReadOnly_ligne.CodeBranche + "_" + ligne.CodeSousBranche + "_" + ligne.CodeCategorie + "_" + ligne.CodeFamille
    var codeBranche = '';
    var codeSousBranche = '';
    var codeCategorie = '';
    var codeFamille = '';
    var tb = id.split('_');
    if (id != '') {
        codeBranche = tb[1];
        codeSousBranche = tb[2];
        codeCategorie = tb[3];
        codeFamille = tb[4];
    }
    var userRights = $("#AdditionalParam").val();
    var codeGarantie = $("#CodeGarantieEdit").val();

    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/ParamGaranties/EditFamilleReassurance",
        data: { codeBranche: codeBranche, codeSousBranche: codeSousBranche, codeCategorie: codeCategorie, codeFamille: codeFamille, codeGarantie: codeGarantie, userRights: userRights },
        success: function (data) {
            if (id != '') {
                $("tr[id=trFamilleReassuranceReadOnly_" + codeBranche + "_" + codeSousBranche + "_" + codeCategorie + "_" + codeFamille + "]").hide();
                $("tr[id=trFamilleReassuranceEdition_" + codeBranche + "_" + codeSousBranche + "_" + codeCategorie + "_" + codeFamille + "]").show();
                $("tr[id=trFamilleReassuranceEdition_" + codeBranche + "_" + codeSousBranche + "_" + codeCategorie + "_" + codeFamille + "]").html(data);
            }
            else {
                $("#CodeBranche").val('');
                $("#CodeSousBranche").val('');
                $("#CodeCategorie").val('');
                $("#FamilleCat").val('');
                $("#divPaneauDynamic").html(data);
                $("#divPaneauDynamic").show();
            }
            MapElementsFamillesReassurance();
            common.page.isLoading = false;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
function EnregistrerFamilleReassurance(id) {
    var codeBrancheAncien = '';
    var codeSousBrancheAncien = '';
    var codeCategorieAncien = '';
    var codeFamilleAncien = '';
    var tb = id.split('_');
    if (id.split('_')[1] == '') id = '';
    else {
        codeBrancheAncien = tb[1];
        codeSousBrancheAncien = tb[2];
        codeCategorieAncien = tb[3];
        codeFamilleAncien = tb[4];
    }
    var userRights = $("#AdditionalParam").val();
    var codeGarantie = $("#CodeGarantieEdit").val();

    codeBranche = $("#CodeBranche").val();
    codeSousBranche = $("#CodeSousBranche").val();
    codeCategorie = $("#CodeCategorie").val();
    codeFamille = $("#CodeFamille").val();

    var verif = true;
    if (codeBranche == "" || codeBranche == undefined) {
        $("#CodeBranche").addClass("requiredField");
        verif = false;
    }
    if (verif) {
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/ParamGaranties/EnregistrerFamilleReassurance",
            data: {
                id: id, codeGarantie: codeGarantie,
                userRights: userRights,
                codeBranche: codeBranche, codeSousBranche: codeSousBranche, codeCategorie: codeCategorie, codeFamille: codeFamille,
                codeBrancheAncien: codeBrancheAncien, codeSousBrancheAncien: codeSousBrancheAncien, codeCategorieAncien: codeCategorieAncien, codeFamilleAncien: codeFamilleAncien
            },
            success: function (data) {
                $("#divBodyFamillesReassurance").html(data);
                $("#divPaneauDynamic").hide();
                $("tr[name=trFamilleReassuranceEdition]").hide();
                $("tr[name=trFamilleReassuranceReadOnly]").show();
                MapElementsFamillesReassurance();
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//-------------Supprime la famille de reassurance-----------
function SupprimerFamilleReassurance(id) {
    var userRights = $("#AdditionalParam").val();
    var codeGarantie = $("#CodeGarantieEdit").val();
    var codeBranche = '';
    var codeSousBranche = '';
    var codeCategorie = '';
    var codeFamille = '';
    var tb = id.split('_');
    if (id != '') {
        codeBranche = tb[1];
        codeSousBranche = tb[2];
        codeCategorie = tb[3];
        codeFamille = tb[4];
    }
    if (id != "" && id != undefined) {
        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/ParamGaranties/SupprimerFamilleReassurance",
            data: { id: id, codeGarantie: codeGarantie, codeBranche: codeBranche, codeSousBranche: codeSousBranche, codeCategorie: codeCategorie, codeFamille: codeFamille, userRights: userRights },
            success: function (data) {
                $("#divBodyFamillesReassurance").html(data);
                $("#divPaneauDynamic").hide();
                $("tr[name=trFamilleReassuranceEdition]").hide();
                $("tr[name=trFamilleReassuranceReadOnly]").show();
                MapElementsFamillesReassurance();
                common.page.isLoading = false;

            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

}

//---------------gestion de l'évènement lorsque l'utilisateur choisi une branche---------------
function BrancheSelectedChanged() {
    $("#CodeBranche").change(function () {
        var selectedBranche = $(this).val();
        if (selectedBranche == "") {
            $("#CodeCategorie").html("<select></selct>");
            $("#FamilleCat").val('');
        }
        //Chargement des sous-branches correspondantes
        $.ajax({
            type: "POST",
            url: "/ParamGaranties/GetSousBranches",
            data: { codeBranche: selectedBranche },
            success: function (data) {
                $("td[id=SousBranche]").html(data);
                SousBrancheSelectedChanged(selectedBranche);
                CategorieSelectedChanged();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//---------------gestion de l'évènement lorsque l'utilisateur choisi une sous-branche (recherche des catégories correspondantes)---------------
function SousBrancheSelectedChanged(selectedBranche) {
    if (selectedBranche == '') selectedBranche = $("#CodeBranche").val();
    $("#CodeSousBranche").change(function () {
        var selectedSousBranche = $(this).val();
        //Chargement des categories correspondantes à la combinaison branche/sous-branche
        $.ajax({
            type: "POST",
            url: "/ParamGaranties/GetCategories",
            data: { codeBranche: selectedBranche, codeSousBranche: selectedSousBranche },
            success: function (data) {
                $("td[id=Categorie]").html(data);
                CategorieSelectedChanged();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//---------------gestion de l'évènement lorsque l'utilisateur choisi une sous-branche (recherche des catégories correspondantes)---------------
function CategorieSelectedChanged() {
    $("#CodeCategorie").change(function () {
        var selectedCategorie = $(this).val();
        $.ajax({
            type: "POST",
            url: "/ParamGaranties/GetFamille",
            data: { codeCategorie: selectedCategorie },
            success: function (data) {
                $("#FamilleCat").val(data);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//---------------------------------------------------//
//-----------Lance la recherche de tous les garanties------------
function GetAllGaranties() {
    var codeRecherche = '';
    var designationRecherche = '';
    var userRights = $("#AdditionalParam").val();
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/ParamGaranties/RechercheGaranties",
        data: { codeGarantie: codeRecherche, designationGarantie: designationRecherche, userRights: userRights },
        success: function (data) {
            $("#divGarantiesBody").html(data);
            MapElementsTableau();
            $("#VoletDetails").hide();
            common.page.isLoading = false;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function AfficherDivFConfirmation() {
    var codeGarantie = $.trim($("#CodeGarantieEdit").val());
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/ParamGaranties/AfficherConfirmation",
        data: {
            codeGarantie: codeGarantie
        },
        success: function (data) {
            $("#divGarantiesBody").html(data);
            if (modeOperation == "I") {
                $("#CodeGarantieEdit").attr('disabled', 'disabled');
                $("#CodeGarantieEdit").addClass('readonly');
                $("#btnTypesValeur").removeAttr('disabled');
                $("#btnReassurance").removeAttr('disabled');
                $("#ModeOperation").val('U');
            }

            MapElementsTableau();
            common.page.isLoading = false;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----Map les éléments de la div flottante des actes de gestion-----
function MapElementConfirmModifTypeInventaire() {
    $("#btnFermer").kclick(function () {
        ReactivateShortCut();
        $("#CodeTypeInventaire").val($("#AncienCodeTypeInventaire").val());
        $("#divDataConfirmModifTypeInventaire").clearHtml();
        $("#divConfirmModifTypeInventaire").hide();
    });

    $("#btnValider").kclick(function () {
        ConfirmModifTypeInventaire();
    });

    AlternanceLigne("BodyOffres", "", false, null);
}
function ConfirmModifTypeInventaire() {
    $("#codeGarantieEdit").removeClass("requiredField");
    $("#DesignationGarantieEdit").removeClass("requiredField");
    $("#CodeTaxe").removeClass("requiredField");
    $("#CodeCatNat").removeClass("requiredField");
    $("#CodeTypeGrille").removeClass("requiredField");
    $("#CodeTypeInventaire").removeClass("requiredField");

    let dataPost = {
        modeOperation: $("#ModeOperation").val(),
        codeGarantie: $("#CodeGarantieEdit").val().trim(),
        designationGarantie: $("#DesignationGarantieEdit").val().trim(),
        abrege: $("#Abrege").val().trim(),
        codeTaxe: $("#CodeTaxe").val(),
        codeCatNat: $("#CodeCatNat").val(),
        isGarantieCommune: $("#IsGarantieCommune").isChecked(),
        codeTypeDefinition: $("#CodeTypeDefinition").val(),
        codeTypeInformation: $("#CodeTypeInformation").val(),
        isRegularisable: $("#IsRegularisable").isChecked(),
        codeTypeGrille: $("#CodeTypeGrille").val(),
        isLieInventaire: $("#IsLieInventaire").isChecked(),
        isAttentatGareat: $("#IsAttentatGareat").isChecked(),
        codeTypeInventaire: $("#CodeTypeInventaire").val(),
        additionalParam: $("#AdditionalParam").val()
    };

    let ancienCodeTypeInventaire = $("#AncienCodeTypeInventaire").val();
    var verif = true;
    if (!dataPost.codeGarantie) {
        $("#codeGarantieEdit").addClass("requiredField");
        verif = false;
    }
    if (!dataPost.designationGarantie) {
        $("#DesignationGarantieEdit").addClass("requiredField");
        verif = false;
    }

    if (!dataPost.codeTaxe) {
        $("#CodeTaxe").addClass("requiredField");
        verif = false;
    }

    if (!dataPost.codeCatNat) {
        $("#CodeCatNat").addClass("requiredField");
        verif = false;
    }

    if (dataPost.isRegularisable && !dataPost.codeTypeGrille) {
        $("#CodeTypeGrille").addClass("requiredField");
        verif = false;
    }
    if (dataPost.isLieInventaire == "O" && !dataPost.codeInventaire) {
        $("#CodeTypeInventaire").addClass("requiredField");
        verif = false;
    }
    if (verif) {
        let obj = {
            modelSave: dataPost,
            ancienCodeTypeInventaire: ancienCodeTypeInventaire,
            listCodesInventaires: $("#ListCodesInventaires").val()
        }
        common.page.isLoading = true;
        common.$postJson("/ParamGaranties/ConfirmModificationGarantie", obj, true).done(function (data) {
            $("#divDataConfirmModifTypeInventaire").clearHtml();
            $("#divConfirmModifTypeInventaire").hide();
            $("#divGarantiesBody").html(data);
            if (modeOperation == "I") {
                $("#CodeGarantieEdit").attr('disabled', 'disabled');
                $("#CodeGarantieEdit").addClass('readonly');
                $("#btnTypesValeur").removeAttr('disabled');
                $("#btnReassurance").removeAttr('disabled');
                $("#ModeOperation").val('U');
            }
            $('#VoletDetails').css('display', 'none');
            MapElementsTableau();
            common.page.isLoading = false;
        });
    }
}

function formatDecimal() {
    common.autonumeric.applyAll('init', 'decimal');
}
//------- Vide les listes de recherche-----
function Initialiser() {
    $("#divGarantiesBody").clearHtml();
    $("#divGarantieDetails").clearHtml();
}

//----------------------- TypeRegul ---------------------

function MapTypeRegul() {
    enableAddMode(false);
    AlternanceLigne("GarTypeRegulBody", "noInput", true, null);
    if (!$("#IsRegularisable").is(':checked')) {
        $("#divTypeRegul").css("display", "none");
    }
    else {
        $("#divTypeRegul").css("display", "block");
    }
    $('#btnAjouterTypeRegul').die();
    $('#btnAjouterTypeRegul').live('click', function () {
        enableAddMode(true);
    });

    $('#btnAnnulerTypeRegul').die();
    $('#btnAnnulerTypeRegul').live('click', function () {
        enableAddMode(false);
    });
    

    $('img[name=btnSupprimerTypeRegul]').die();
    $('img[name=btnSupprimerTypeRegul]').live('click', function () {

        var codeGarantie = $('#CodeGarantieEdit').val();
        var codeTypeRegul = $(this).attr('albcodetyperegul');
      
        $("#idGarantieSuppr").val(codeGarantie + "_" + codeTypeRegul);
        
        ShowCommonFancy("Confirm", "SupprimerGarTypeRegul", "Vous allez supprimer le type de régularisation.<br/>Etes-vous sûr de vouloir continuer ?", 320, 150, true, true);

       
    });

    
    $('#btnEnregistrerTypeRegul').die();
    $('#btnEnregistrerTypeRegul').live('click', function () {

        var codeGarantie = $('#CodeGarantieEdit').val();

        if ($('#ModeOperation').val() === 'I') {
            common.dialog.bigError("Code garantie vide,Veillez sauvegarder la garntie!", true);
            return;
        }


        var codeTypeRegul = $('#CodeTypeRegul').val();
        if (!codeTypeRegul) {
            $('#CodeTypeRegul').addClass('requiredField');
            return;
        }

        common.page.isLoading = true;
        $.ajax({
            type: "POST",
            url: "/ParamGaranties/AddTypeRegul",
            data: {
                codeGarantie: codeGarantie, codeTypeRegul: codeTypeRegul
            },
            success: function (data) {
                $('#divListeTypeRegulDetails').html(data);
                enableAddMode(false);
                AlternanceLigne("GarTypeRegulBody", "noInput", true, null);
               
                common.page.isLoading = false;
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });

    });

   

    $('#btnAnnulerTypeRegul').die();
    $('#btnAnnulerTypeRegul').live('click', function () {
        enableAddMode(false);
    });

}
function enableAddMode(enable) {
    if (enable) {
        $('#trAjoutTypeRegul').css('display', 'table-row');
    }
    else {
        $('#trAjoutTypeRegul').css('display', 'none');
        
    }
    $('#CodeTypeRegul').removeClass('requiredField');
    $('#CodeTypeRegul').val('');
}

function DeleteTypeRegul(codeGarantie, codeTypeRegul) {
    common.page.isLoading = true;
    $.ajax({
        type: "POST",
        url: "/ParamGaranties/DeleteTypeRegul",
        data: {
            codeGarantie: codeGarantie, codeTypeRegul: codeTypeRegul
        },
        success: function (data) {
            $('#divListeTypeRegulDetails').html(data);
            enableAddMode(false);
            AlternanceLigne("GarTypeRegulBody", "noInput", true, null);
            common.page.isLoading = false;
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


