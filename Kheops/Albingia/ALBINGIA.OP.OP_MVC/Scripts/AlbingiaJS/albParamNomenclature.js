/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementPage();
});

//----------Map les éléments de la page--------
function MapElementPage() {
    $("#btnRechercher").die().live('click', function () {
        rechercherNomenclatures();
    });
    rechercherNomenclatures();
}

function MapElementTableauNomenclatures() {
    $("#tblNomenclaturesBody td[name=clickableCol]").each(function () {
        $(this).click(function () {
            var id = $(this).attr("id").split('_')[1];
            LoadDetailsNomenclature(id);
        });
    });
}

function MapElementDetailsNomenclatures() {
    AlternanceLigne("DetailsNomenclaturesBody", "noInput", true, null);


    $("#tblDetailsNomenclaturesBody td[name=clickableColDetails]").each(function () {
        $(this).click(function () {
            var id = $(this).attr("id").split('_')[1];
            LoadLigneDetailNomenclature("edition", id);
        });
    });

    $("#btnAjouterLigne").unbind();
    $("#btnAjouterLigne").bind("click", function () {
        LoadLigneDetailNomenclature("edition", $("#EmptyLineId").val());
    });

    $("select[name=dvConcepts]").each(function () {
        $(this).unbind();
        $(this).bind("change", function () {
            var id = $(this).attr("id").split('_')[1];
            LoadListeFamilleByConcept(id);
        });
    });

    $("img[name=btnSuppression]").each(function () {
        $(this).unbind();
        $(this).bind("click", function () {
            var id = $(this).attr("id").split('_')[1];
            SupprimerLigneDetail(id);
        });
    });

    MapDrlFamilles();

}

function MapDrlFamilles() {
    $("select[name=dvFamilles]").each(function () {
        $(this).unbind();
        $(this).bind("change", function () {
            var id = $(this).attr("id").split('_')[1];
            LoadListeValeurByConceptFamille(id);
        });
    });
}

function MapElementLigneDetail(mode, idLigne) {
    if (mode == "edition") {
        $("#btnSaveLigne_" + idLigne).unbind();
        $("#btnSaveLigne_" + idLigne).bind("click", function () {
            SauvegarderLigneDetail(idLigne);
        });
    }
    else if (mode = "readonly") {
        $("#btnSupprimerLigne_" + idLigne).unbind();
        $("#btnSupprimerLigne_" + idLigne).bind("click", function () {
            SupprimerLigneDetail(idLigne);
        });
    }
}

//-----------Recherche les nomenclatures
function rechercherNomenclatures() {
    ShowLoading();
    $("#VoletDetail").hide();
    var descrNomenclature = $("#RechercheDescription").val();
    $.ajax({
        type: "POST",
        url: "/ParamNomenclature/Recherche",
        data: { description: descrNomenclature },
        success: function (data) {
            CloseLoading();
            $("#divBodyNomenclature").html(data);
            MapElementTableauNomenclatures();
            AlternanceLigne("NomenclaturesBody", "noInput", true, null);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}

//----------Charge les détails de la nomenclature en paramètre
function LoadDetailsNomenclature(id) {
    if (id != "" && id != undefined) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamNomenclature/AfficherDetailsNomenclature",
            data: { idNomenclature: id },
            success: function (data) {
                $("#divNomenclatureDetails").html(data);
                $("#VoletDetail").show();
                MapElementDetailsNomenclatures();


                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//---------Charge la ligne en paramètre dans le mode en paramètre (édition ou readonly)
function LoadLigneDetailNomenclature(mode, id) {
    if (id != "" && id != undefined && mode != "" && mode != undefined) {
        var idNomenclature = $("#DetailGuidId").val();

        var concept01 = $("#drlConcepts_01").val();
        var concept02 = $("#drlConcepts_02").val();
        var concept03 = $("#drlConcepts_03").val();
        var concept04 = $("#drlConcepts_04").val();
        var famille01 = $("#drlFamilles_01").val();
        var famille02 = $("#drlFamilles_02").val();
        var famille03 = $("#drlFamilles_03").val();
        var famille04 = $("#drlFamilles_04").val();

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamNomenclature/GetLigneDetailsNomenclature",
            data: {
                modeAffichage: mode, idNomenclature: idNomenclature, idLigneDetail: id,
                concept01: concept01, concept02: concept02, concept03: concept03, concept04: concept04,
                famille01: famille01, famille02: famille02, famille03: famille03, famille04: famille04
            },
            success: function (data) {
                $("#ligneDetails_" + id).html(data);

                if (id == $("#EmptyLineId").val()) {
                    $("#divDetailsBodyEmptyLine").show();
                }

                MapElementLigneDetail(mode, id);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }


}

//----------Charge la liste des familles par concept
function LoadListeFamilleByConcept(id) {
    var selectedConcept = $("#drlConcepts_" + id).val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ParamNomenclature/GetListeFamilleByConcept",
        data: { concept: selectedConcept, id: id },
        success: function (data) {
            $("#divDrlFamille_" + id).html(data);
            MapDrlFamilles();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//----------Charge la liste des valeurs par concept et famille
function LoadListeValeurByConceptFamille(idColonne) {
    var selectedConcept = $("#drlConcepts_" + idColonne).val();
    var selectedFamille = $("#drlFamilles_" + idColonne).val();

    var drlValeur = $("select[name=dvValeur" + idColonne + "]");
    if (drlValeur.attr("id") != undefined) {
        var guidIdLigne = drlValeur.attr("id").split("_")[1];
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamNomenclature/GetListeValeurByConceptFamille",
            data: { concept: selectedConcept, famille: selectedFamille, idColonne: idColonne, guidIdLigne: guidIdLigne },
            success: function (data) {
                $("#divDrlValeur_" + idColonne).html(data);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//----------Sauvegarde la ligne details en paramètre---------
function SauvegarderLigneDetail(idLigne) {
    if (idLigne != undefined) {
        var valeur1 = $("#drlValeur01_" + idLigne).val();
        var valeur2 = $("#drlValeur02_" + idLigne).val();
        var valeur3 = $("#drlValeur03_" + idLigne).val();
        var valeur4 = $("#drlValeur04_" + idLigne).val();
        var idNomenclature = $("#DetailGuidId").val();

        //Vérification des données
        var isChecked = true;
        var errorMessage = "";

        //Vérification de la hiérarchie
        var maxHierarchyId = 0;
        if (valeur1 != "")
            maxHierarchyId = 1;
        if (valeur2 != "")
            maxHierarchyId = 2;
        if (valeur3 != "")
            maxHierarchyId = 3;
        if (valeur4 != "")
            maxHierarchyId = 4;
        if (maxHierarchyId == 0) {
            isChecked = false;
            errorMessage = "Aucune valeur sélectionnée, veuillez en choisir au moins une";
        }

        var nbColonnesVides = GetNbColonnesVides(idLigne, maxHierarchyId);
        if (nbColonnesVides > 0 && nbColonnesVides < (maxHierarchyId - 1)) {
            isChecked = false;
            errorMessage = "Hiérarchie de valeurs incomplète.<br> Veuillez la compléter (remplir tous les champs à gauche de la dernière valeur) <br>ou la supprimer (vider tous les champs à gauche de la dernière valeur)"
        }

        //Verification des familles
        var nbFamilleVides = GetAndCheckNbFamillesVides(idLigne, maxHierarchyId);
        if (nbFamilleVides > 0)
        {
            isChecked = false;
            errorMessage = "Une famille parent ne peut être nulle";
        }

        if (isChecked) {
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/ParamNomenclature/EnregistrerLigneDetail",
                data: { idNomenclature: idNomenclature, idLigne: idLigne, valeur1: valeur1, valeur2: valeur2, valeur3: valeur3, valeur4: valeur4 },
                success: function (data) {
                    $("#divNomenclatureDetails").html(data);
                    MapElementDetailsNomenclatures();
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        else {
            if (errorMessage != "")
                ShowCommonFancy("Error", "", errorMessage, 1212, 700, true, true);
        }
    }
}

//----------Supprime la ligne details en paramètre---------
function SupprimerLigneDetail(idLigne) {
    if (idLigne != undefined) {
        var idNomenclature = $("#DetailGuidId").val();
        //TODO application des règles de validation

        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ParamNomenclature/SupprimerLigneDetail",
            data: { idNomenclature: idNomenclature, idLigne: idLigne },
            success: function (data) {
                $("#divNomenclatureDetails").html(data);
                MapElementDetailsNomenclatures();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//-----------Récupère le nombre de colonnes vides précédent la colonne en paramètre
function GetNbColonnesVides(idLigne, idMaxColonne) {
    var toreturn = 0;
    for (var i = 1; i < idMaxColonne; i++) {
        if ($("#drlValeur0" + i + "_" + idLigne).val() == "")
            toreturn++;
    }
    return toreturn;
}
//-----------Récupère le nombre de familles vides précédent la colonne en paramètre
function GetAndCheckNbFamillesVides(idLigne, idMaxColonne) {
    var famille1 = $("#drlFamilles_01").val();
    var famille2 = $("#drlFamilles_02").val();
    var famille3 = $("#drlFamilles_03").val();
    var famille4 = $("#drlFamilles_04").val();

    var toreturn = 0;
    for (var i = 1; i < idMaxColonne; i++) {
        if ($("#drlFamilles_0" + i).val() == "") {
            toreturn++;        
        }
    }
    return toreturn;
}
