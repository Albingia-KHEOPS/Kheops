$(document).ready(function () {
    MapRecherche();
    MapTable();
    FormatNumericFieldRecherche();
});

//-----------Mappage du Body

function MapRecherche() {

    $("#btnRechercherFrais").die();
    $("#btnRechercherFrais").click(function () {
        GetListWithFiltre();
    });

    $("#btnInitializeRechercheFrais").die().live('click', function () {
        InitFiltre();
    });

    ListBranchesSelectedItemChanged("Branche", "SousBranche", "Categorie", GetModelFiltre);
}

//--- mappage de la table header
function MapTableHeader() {

    $("#btnAjouter").die();
    $("#btnAjouter").click(function () {
        GetNewItemModel();
    });

}

//-----------Mappage des elements de la table 
function MapTable() {

    MapTableHeader();

    $(".deleteBtn").die();
    $(".deleteBtn").click(function (event) {
        event.stopPropagation();
        $("#deleteHiddenInput").val($(this).attr("ItemId"));
        ShowAndMapFancydelete();
    });

    $("#FraisTable td").die();
    $("#FraisTable td").click(function () {
        GetEditionModel($(this).parent());
    });

    common.autonumeric.applyAll('init', 'numeric',null, ',', '0', null, '0');
}

//--------formattage numeric filtre recherche
function FormatNumericFieldRecherche() {
    common.autonumeric.apply($("#Annee"), 'init', 'numeric', '', ',', '0', '9999', '0');
}


//--------formattage numeric model edit
function FormatNumericFieldEdit() {

    common.autonumeric.apply($("#AnneeEdit"), 'init', 'numeric', '', ',', '0', '9999', '0');
    common.autonumeric.apply($("#MontantEdit"), 'init', 'numeric', null, ',', '0', '9999999', '0');
    common.autonumeric.apply($("#FRaiSACCMINEdit"), 'init', 'numeric', null, ',', '0', '9999999', '0');
    common.autonumeric.apply($("#FRaiSACCMAXEdit"), 'init', 'numeric', null, ',', '0', '9999999', '0');
}


//-----------------retourner le model de filtre
function GetModelFiltre() {
    var filtreModel = {
        "Branche": $("#Branche").val(),
        "SousBranche": $("#SousBranche").val(),
        "Categorie": $("#Categorie").val(),
        "Annee": $("#Annee").val()
    }

    filtreModel.Annee = IsUndifinedOrEmpty(filtreModel.Annee) ? "0":filtreModel.Annee;
    return filtreModel;
}

//----charger le model a partir d'un Id
function GetModelFromId(Id) {
    var splitChar = $("#SplitChar").val();
    var array = Id.split(splitChar);
    var model = {
        "Branche": array[0],
        "SousBranche": array[1],
        "Categorie": array[2],
        "Annee": array[3]
    }

    model.Annee = IsUndifinedOrEmpty(model.Annee) ? "0" : model.Annee;

    return model;
}

//----charger le model d'edition
function GetCurrentEditionModel() {

    var model = {
        "Branche": $("#BrancheEdit").val(),
        "SousBranche": $("#SousBrancheEdit").val(),
        "Categorie": $("#CategorieEdit").val(),
        "Annee": $("#AnneeEdit").val(),
        "Montant": $("#MontantEdit").val(),
        "FRaiSACCMIN": $("#FRaiSACCMINEdit").val(),
        "FRaiSACCMAX": $("#FRaiSACCMAXEdit").val()
    }

    model.Montant = model.Montant.replace(/ /g, "");
    model.FRaiSACCMIN = model.FRaiSACCMIN.replace(/ /g, "");
    model.FRaiSACCMAX = model.FRaiSACCMAX.replace(/ /g, "");

    model.Annee = IsUndifinedOrEmpty(model.Annee) ? "0" : model.Annee;
    model.Montant = IsUndifinedOrEmpty(model.Montant) ? "0" : model.Montant;
    model.FRaiSACCMIN = IsUndifinedOrEmpty(model.FRaiSACCMIN) ? "0" : model.FRaiSACCMIN;
    model.FRaiSACCMAX = IsUndifinedOrEmpty(model.FRaiSACCMAX) ? "0" : model.FRaiSACCMAX;

    return model;
}

//---------------retourner le model de filtre
function InitFiltre() {
    $("#Branche").val("");
    $("#Branche").trigger("change");

    $("#SousBranche").val("");
    $("#SousBranche").trigger("change");

    $("#Categorie").val("");
    $("#Annee").val("");    
}

//----------- lancer la recherche de stat avec le filtre
function GetListWithFiltre() {

    ShowLoading();

    var filtre = GetModelFiltre();

    $.ajax({
        type: "POST",
        url: "/FraisAccessoires/List",
        data: filtre,
        success: function (data) {
            $("#ListResultFrais").html(data);
            MapTable();
            CloseLoading();
        },
        error: function (request) {
            CloseLoading();
            ShowFancyError(request);
        }
    });
}


//-------------- export CSV
function ExportToCSV() {

    var filtre = GetModelFiltre();
    window.location.href = "/FraisAccessoires/ExportFile?" + "&branche=" + (IsUndifinedOrEmpty(filtre.Branche) ? "" : filtre.Branche)
                                                      + "&sousBranche =" + (IsUndifinedOrEmpty(filtre.SousBranche) ? "" : filtre.SousBranche)
                                                      + "&categorie=" + (IsUndifinedOrEmpty(filtre.Categorie) ? "" : filtre.Categorie)
                                                      + "&annee=" + (IsUndifinedOrEmpty(filtre.Annee) ? "0" : filtre.Annee)
    return true;
}

//---------------gestion de l'évènement lorsque l'utilisateur choisi une branche (recherche des sous-branches correspondantes)---------------
function ListBranchesSelectedItemChanged(brancheElm, souBrancheElm, categorieElm, filtreModel) {

    if (brancheElm != "" && brancheElm != undefined && souBrancheElm != "" && souBrancheElm != undefined) {
        $("#" + brancheElm).change(function () {

            var filtre = filtreModel();

            if (filtre.Branche == "" || filtre.Branche == undefined) {
                $("#" + souBrancheElm + " option").remove();
                return false;
            }

            //Chargement des sous-branches correspondantes
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/FraisAccessoires/GetSousBranches",
                data: { codeBranche: filtre.Branche, id: souBrancheElm },
                success: function (data) {
                    $("#" + souBrancheElm).replaceWith(data);
                    ListSousBranchesSelectedItemChanged(souBrancheElm, categorieElm, filtreModel);
                    CloseLoading();
                },
                error: function (request) {
                    common.error.showXhr(request);
                    CloseLoading();
                }
            });
        });
    }
}

//---------------gestion de l'évènement lorsque l'utilisateur choisi une sous-branche (recherche des catégories correspondantes)---------------
function ListSousBranchesSelectedItemChanged(souBrancheElm, categorieElm, filtreModel) {
    if (souBrancheElm != "" && souBrancheElm != undefined && categorieElm != "" && categorieElm != undefined) {

        $("#" + souBrancheElm).change(function () {

            var filtre = filtreModel();

            if (filtre.Branche == "" || filtre.Branche == undefined) {
                $("#" + souBrancheElm + " option").remove();
                $("#" + categorieElm + " option").remove();
                return false;
            }

            if (filtre.SousBranche == "" || filtre.SousBranche == undefined) {
                $("#" + categorieElm + " option").remove();
                return false;
            }

            //Chargement des categories correspondantes à la combinaison branche/sous-branche
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/FraisAccessoires/GetCategories",
                data: { codeBranche: filtre.Branche, codeSousBranche: filtre.SousBranche, id: categorieElm },
                success: function (data) {
                    $("#" + categorieElm).replaceWith(data);
                    CloseLoading();

                },
                error: function (request) {
                    common.error.showXhr(request);
                    CloseLoading();
                }
            });
        });
    }
}

// -------charger le ligne modele d'edition 
function GetEditionModel(selectedElm) {
    var id = $(selectedElm).attr("id");
    var modelId = GetModelFromId(id);
    $.ajax({
        type: "POST",
        url: "/FraisAccessoires/Edit",
        data: modelId,
        success: function (data) {

            ToggleEditItem();

            $(data).insertAfter(selectedElm);

            $(selectedElm).hide();

            MapEditionModel(selectedElm, modelId);
        },
        error: function (request) {
            ShowFancyError(request);
        }
    });
}

//---supprimer le model d'editionet afficher le model readOnly
function ToggleEditItem() {
    $("#EditItem").remove();
    $("#FraisTable tr").show();
}

// -------charger le ligne modele Nouveau
function GetNewItemModel() {
    $.ajax({
        type: "POST",
        url: "/FraisAccessoires/New",
        success: function (data) {

            ToggleEditItem();

            $("#FraisTable").prepend(data);

            MapNewItemModel();
        },
        error: function (request) {
            ShowFancyError(request);
        }
    });
}

// -------Supprimer un elemnt
function DeleteItem(id) {
    var modelId = GetModelFromId(id);
    $.ajax({
        type: "POST",
        url: "/FraisAccessoires/Delete",
        data: modelId,
        success: function (data) {
            $("[id=\"" + data.Id + "\"]").remove();
        },
        error: function (request) {
            ShowFancyError(request);
        }
    });
}


//---------afficher la confirmation avant suppression
function ShowAndMapFancydelete() {
    ShowCommonFancy("Confirm", "Submit", "Etes-vous de vouloir supprimer la ligne ?\n\n",
                   300, 300, true, true);

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
    });

    $("#btnConfirmOk").die().live('click', function () {
        var id = $("#deleteHiddenInput").val();
        DeleteItem(id);
        CloseCommonFancy();
    });
}

//-- Mappage du modele d'edition

function MapEditionModel(selectedElm, modelId) {

    $("#cancelEdit").die();
    $("#cancelEdit").click(function () {
        $("#EditItem").remove();
        $(selectedElm).show();
    });

    $("#saveEdit").die();
    $("#saveEdit").click(function () {
        SaveEditionModel(selectedElm, modelId);
    });

    ListBranchesSelectedItemChanged("BrancheEdit", "SousBrancheEdit", "", GetCurrentEditionModel);
    FormatNumericFieldEdit();
}


function MapNewItemModel() {

    $("#cancelEdit").die();
    $("#cancelEdit").click(function () {
        $("#EditItem").remove();
    });

    $("#saveEdit").die();
    $("#saveEdit").click(function () {
        SaveNewItemModel();
    });

    ListBranchesSelectedItemChanged("BrancheEdit", "SousBrancheEdit", "", GetCurrentEditionModel);

    FormatNumericFieldEdit();

    $("#divBodyParam").scrollTop(0)
}

//---enregistrer une nouvelle ligne
function SaveNewItemModel() {

    var modelToSave = GetCurrentEditionModel();

    if (!ValidateEditModel(modelToSave)) {
        return false;
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/FraisAccessoires/SaveNew",
        data: modelToSave,
        success: function (data) {
            CloseLoading();
            $(data).insertAfter($("#EditItem"));

            $("#EditItem").remove();

            MapTable();
        },
        error: function (request) {
            CloseLoading();
            ShowFancyError(request);
        }
    });
}


//--------enregistrer le model en cours d'edition
function SaveEditionModel(selectedElm, modelId) {
    var modelToSave = GetCurrentEditionModel();

    if (!ValidateEditModel(modelToSave))
    {
        return false;
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/FraisAccessoires/SaveEdit",
        data: { "filtre": JSON.stringify(modelId), "toSave": JSON.stringify(modelToSave) },
        success: function (data) {
            CloseLoading();
            $("#EditItem").remove();

            $(data).insertAfter(selectedElm);

            $(selectedElm).remove();

            MapTable();
        },
        error: function (request) {
            CloseLoading();
            ShowFancyError(request);
        }
    });
}

//validation  false = non valide true = valide
function ValidateEditModel(model) {

    var error = true;
    var anneeMin = parseInt($("#DateMin").val().slice(6));
    var anneeMax = parseInt($("#DateMax").val().slice(6));
    var anneeMdel = parseInt(model.Annee);

    error = error & ValidateFild("BrancheEdit", IsUndifinedOrEmpty(model.Branche));
    error = error & ValidateFild("SousBrancheEdit", IsUndifinedOrEmpty(model.SousBranche));
    error = error & ValidateFild("CategorieEdit", IsUndifinedOrEmpty(model.Categorie));
    error = error & ValidateFild("AnneeEdit", (anneeMdel < anneeMin || anneeMdel > anneeMax));
    

    error = error & ValidateFild("FRaiSACCMINEdit", model.FRaiSACCMIN == 0);

    if (model.Montant > 0)
    {
        error = error & ValidateFild("FRaiSACCMAXEdit", model.FRaiSACCMAX == 0);
    }

    return error;
}

// valider un champs ave la condition de validation
function ValidateFild(id,condition) {
    if (condition) {
        $("#"+id).addClass("requiredField");
        return false;
    }
    else {
        $("#"+id).removeClass("requiredField");
        return true;
    }
}

//---------afficher une message d'erreur
function ShowFancyError(request) {
    common.error.showXhr(request);
}

//--------------------test valeur null----------------------------------------
function IsUndifinedOrEmpty(str) {
    if (str == undefined || str == null || str == "") {
        return true;
    }
    return false;
}