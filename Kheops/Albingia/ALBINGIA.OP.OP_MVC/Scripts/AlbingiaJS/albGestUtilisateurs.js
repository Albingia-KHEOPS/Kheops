$(document).ready(function () {
    MapCells();
    MapCommonAutoCompUtilisateur();
});

//-----------Mappage du Body
function MapCells() {
    var currentAlbContext = $("#currentAlbContext").val();

    MapTable(currentAlbContext);
    MapOpUtilisateurBranche(currentAlbContext);
    MapRecherche(currentAlbContext);
    MapOtherBtn(currentAlbContext);
}

//-----sous fonction MapCells
function MapOtherBtn(currentAlbContext) {
    $("#btnFullScreen").die();
    $("#btnFullScreen").click(function () {
        OpenCloseFullScreenListUsers(true);
    });

    $("#btnResetUsrRight").die().live('click', function () {
        ResetUserRight();
    });

    $("#btnResetUsrLogin").die().live('click', function () {
        ResetUserLogin();
    });

    $("#btnResetYutilis").die().live('click', function () {
        ResetYUTILIS();
    });
}

//-----Mappage div recherche
function MapRecherche(currentAlbContext) {
    //---- synchronize recherche droplist et new user droplist
    $("#rechercheBranche[albcontext=\"" + currentAlbContext + "\"]").die();
    $("#rechercheBranche[albcontext=\"" + currentAlbContext + "\"]").live('change', function () {
        $("#newBranche[albcontext=\"" +currentAlbContext + "\"]").val($(this).val());
        $("#newBranche[albcontext=\"" +currentAlbContext + "\"]").trigger("change");
    });

    //--- ouvrir div flottant de recherche

    $("#btnRechercherGestUsers[albcontext=\"" + currentAlbContext + "\"]").die();
    $("#btnRechercherGestUsers[albcontext=\"" + currentAlbContext + "\"]").live('click', function () {
        $("#divRechercheGestUtilisateur").show();
        MapDivRechercheGestUtilisateur();
        $("#newBranche[albcontext=\"" + $("#currentAlbContext").val() + "\"]").trigger("change");
    });

    //--------------inialize recherches inputs
    $("#btnInitialize[albcontext=\"" + currentAlbContext + "\"]").die();
    $("#btnInitialize[albcontext=\"" + currentAlbContext + "\"]").live('click', function () {
        var currentAlbContext = $("#currentAlbContext").val();
        $("#rechercheUtilisateur[albcontext^=\"" + currentAlbContext + "\"]").val("");
        $("#rechercheBranche[albcontext=\"" + currentAlbContext + "\"]").val("");
        $("#rechercheTypeDroit[albcontext=\"" + currentAlbContext + "\"]").val("");
    });

    //-------declancher le click sur le btn de recherche suite a un appui sur entrer dans l'input rechercheUtilisateur

    $("#rechercheUtilisateur[albcontext^=\"" + currentAlbContext + "\"]").die();
    $("#rechercheUtilisateur[albcontext^=\"" + currentAlbContext + "\"]").live('keydown', function (event) {
        var currentAlbContext = $("#currentAlbContext").val();
        if (event.which == 13 ) {
            $("#btnRechercherGestUsers[albcontext=\"" + currentAlbContext + "\"]").trigger("click");
        }        
    });

    //---mettre le focus sur rechercheUtilisateur
    $("#rechercheUtilisateur[albcontext^=\"" + currentAlbContext + "\"]").focus();
}
//--------operation sur UtilisateurBranche
function MapTable(currentAlbContext) {
    //----hide editableCell ---
    $(".editableCell[albcontext=\"" + currentAlbContext + "\"]").hide();

    if ($("#IsReadOnlyBody[albcontext=\"" + currentAlbContext + "\"]").val().toUpperCase() == "FALSE") {
        MapEditMode(currentAlbContext);
    }

    //---- synchronize editable and readOnly cells
    $(".editableCell select[albcontext=\"" + currentAlbContext + "\"]").die();
    $(".editableCell select[albcontext=\"" + currentAlbContext + "\"]").live('change', function () {
        var id = $(this).attr("id");
        var currentAlbContext = $("#currentAlbContext").val();
        $("label[id=\"" + id + "\"][albcontext=\"" +currentAlbContext + "\"]").html($("select[id=\"" + id + "\"][albcontext=\"" +currentAlbContext + "\"] :selected").text());
    });
}

//-------mappage des controls des lignes
function MapOpUtilisateurBranche(currentAlbContext) {

    $("#saveNewUtilisateurBranche[albcontext=\"" + currentAlbContext + "\"]").die();
    $("#saveNewUtilisateurBranche[albcontext=\"" + currentAlbContext + "\"]").live('click', function () {
        var currentAlbContext = $("#currentAlbContext").val();
        AddNewUtilisateurBranche(currentAlbContext);
    });

    //------------------enregistrer  un UtilisateurBranche
    $(".saveUtilisateurBrancheBtn[albcontext=\"" + currentAlbContext + "\"]").die();
    $(".saveUtilisateurBrancheBtn[albcontext=\"" + currentAlbContext + "\"]").live('click', function () {
        var id = $(this).attr("id");
        var currentAlbContext = $("#currentAlbContext").val();
        SaveUtilisateurBranche(id, currentAlbContext);
    });

    //------------------supprimer  un UtilisateurBranche
    $(".delUtilisateurBrancheBtn[albcontext=\"" + currentAlbContext + "\"]").die();
    $(".delUtilisateurBrancheBtn[albcontext=\"" + currentAlbContext + "\"]").live('click', function () {
        var id = $(this).attr("id");
        $("#hiddenInputId").val(id);
        ShowAndMapFancydelete();
    });

    //------------------ajouter  un nouveau UtilisateurBranche
    $("#btnAjouter[albcontext=\"" + currentAlbContext + "\"]").die();
    $("#btnAjouter[albcontext=\"" + currentAlbContext + "\"]").live('click', function () {
        var currentAlbContext = $("#currentAlbContext").val();
        ClickbtnAjouter(currentAlbContext);
    });

    $("#cancelNewUtilisateurBranche[albcontext=\"" + currentAlbContext + "\"]").die();
    $("#cancelNewUtilisateurBranche[albcontext=\"" + currentAlbContext + "\"]").live('click', function () {
        var currentAlbContext = $("#currentAlbContext").val();
        $("#NewUtilisateurBrancheRow[albcontext=\"" +currentAlbContext + "\"]").hide();
        //vider nouveau utlisateur
        $("#newUtilisateur[albcontext^=\"" +currentAlbContext + "\"]").val("");
        $("#newBranche[albcontext=\"" +currentAlbContext + "\"]").val("");
        $("#newTypeDroit[albcontext=\"" +currentAlbContext + "\"]").remove();
    });

    //--------chargement du combos box d'un nouveau user selon la Restriction 
    $("#newBranche[albcontext=\"" + currentAlbContext + "\"]").die();
    $("#newBranche[albcontext=\"" + currentAlbContext + "\"]").live('change', function () {
        var currentAlbContext = $("#currentAlbContext").val();
        GetTypeDroitComboByBranche($(this).val(), currentAlbContext);
    });

    //------------------Annuler l'edition UtilisateurBranche
    $(".cancelUtilisateurBrancheEditBtn[albcontext=\"" + currentAlbContext + "\"]").die();
    $(".cancelUtilisateurBrancheEditBtn[albcontext=\"" + currentAlbContext + "\"]").live('click', function () {
        var currentAlbContext = $("#currentAlbContext").val();
        CancelEdit(currentAlbContext, $(this).attr("id"));
    });
}
//---------------------------Verifier les droits d'Utilisateur clic
$("#btnverifdroit").die().live('click', function () {
    var utilisateur = $("#rechercheUtilisateur").val();
    if (utilisateur!= "") {
        VerifDroitsUtilisateur(utilisateur)
    }
});


//---------------------------Verifier les droits d'Utilisateur function
function VerifDroitsUtilisateur(id) {

        $.ajax({
            type: "POST",
            url: "/GestUtilisateurs/VerifDroitsByUser",
            data: {id },
            success: function (data) {
                if (data.success) {
                    alert("Droits d'utilisateur vérifié");
                } else {
                    alert("Aucun droit pour cet utilisateur ");
                }
             
                CloseLoading();
            },
            error: function (request) {
                ShowFancyError(request);
            }
        });
    }

//-----------
function CancelEdit(currentAlbContext, id) {
    //cacher tt les edits
    $(".UpdUtilisateurBrancheEditBtn[albcontext=\"" + currentAlbContext + "\"]").show();
    $("span[id=\"" + id + "\"][class~=cancelUtilisateurBrancheEditBtn][albcontext=\"" + currentAlbContext + "\"]").hide();
    $("span[id=\"" + id + "\"][class~=saveUtilisateurBrancheBtn][albcontext=\"" + currentAlbContext + "\"]").hide();
    $("span[id=\"" + id + "\"][class~=delUtilisateurBrancheBtn][albcontext=\"" + currentAlbContext + "\"]").show();

    //cancel new values
    $("select[id=\"" + id + "TypeDroit" + "\"]").each(function () {
        $(this).val($(this).attr("albSavedValue"));
        var id = $(this).attr("id");
        $("label[id=\"" + id + "\"][albcontext=\"" + currentAlbContext + "\"]").html($("select[id=\"" + id + "\"][albcontext=\"" + currentAlbContext + "\"] :selected").text());
    });

    $(".editableCell[albcontext=\"" + currentAlbContext + "\"]").hide();
    $(".readOnlyCell[albcontext=\"" + currentAlbContext + "\"]").show();
}

//---------afficher la confirmation avant suppression
function ShowAndMapFancydelete() {
    ShowCommonFancy("Confirm", "Submit", "Etes-vous de vouloir supprimer l'utilisateur ?\n\n",
                   300, 300, true, true);

    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
    });

    $("#btnConfirmOk").die().live('click', function () {
        var id = $("#hiddenInputId").val();
        var currentAlbContext = $("#currentAlbContext").val();
        RemoveUtilisateurBranche(id, currentAlbContext);
        CloseCommonFancy();
    });
}

//------- charger les typedroits selon la branche
function GetTypeDroitComboByBranche(branche, currentAlbContext) {
    if (IsUndifinedOrEmpty(branche)) {
        $("#newTypeDroit[albcontext=\"" + currentAlbContext + "\"]").remove();
    }
    else {

        $.ajax({
            type: "POST",
            url: "/GestUtilisateurs/GetTypeDroitsByBranche",
            data: { branche: branche, albContext: currentAlbContext },
            success: function (data) {
                $("#newTypeDroitCell[albcontext=\"" + $("#currentAlbContext").val() + "\"]").html(data);
                CloseLoading();
            },
            error: function (request) {
                ShowFancyError(request);
            }
        });
    }
}

function ClickbtnAjouter(currentAlbContext) {
    $(".editableCell[albcontext=\"" + currentAlbContext + "\"]").hide();
    $(".readOnlyCell[albcontext=\"" + currentAlbContext + "\"]").show();

    $(".cancelUtilisateurBrancheEditBtn[albcontext=\"" + currentAlbContext + "\"]").hide();
    $(".saveUtilisateurBrancheBtn[albcontext=\"" + currentAlbContext + "\"]").hide();
    $(".delUtilisateurBrancheBtn[albcontext=\"" + currentAlbContext + "\"]").show();

    //vider nouveau utlisateur
    $("#newUtilisateur[albcontext^=\"" + currentAlbContext + "\"]").val("");

    $("#NewUtilisateurBrancheRow[albcontext=\"" + currentAlbContext + "\"]").show();
    $("#divBodyParam[albcontext=\"" + currentAlbContext + "\"]").scrollTop(0);
}

//---------afficher ou chacher full Screen
function OpenCloseFullScreenListUsers(open) {
    if (open) {
        $("#gestionUtlisateurScreen").html("");
        ReloadMainScreen("divDataFullScreenListUsers", false, MapFullScreen);
    }
    else {
        $("#divDataFullScreenListUsers").html("");
        $("#divFullScreenListUsers").hide();
        ReloadMainScreen("divGestUtlisateurBody", true);
    }
}

//----Mappage du div full screen----------------------
function MapFullScreen() {

    $("#CloseFullScreen").show();
    $("#FullScreen").hide();

    $("#fermerFullScreen").die();
    $("#fermerFullScreen").click(function () {
        OpenCloseFullScreenListUsers(false);
    });

    // afficher la div
    $("#divFullScreenListUsers").show();
    ChangeFullScreenStyle();
}

function HideFullScreen() {
    $(".fullScreenButtons").each(function () {
        $(this).hide();
    });
}

function ChangeFullScreenStyle() {
    $(".CommonForm .HorizontalFullWithGroup").css('height', 'auto');
    $(".GroupFull").css('height', 'auto');
    $("#divBodyParam").css('height', 580);
    $("#CloseFullScreen").css('height', 30);
}

//-----------ajouter un utlisateur en base
function AddNewUtilisateurBranche(currentAlbContext) {    

    ShowLoading();

    var newUtilisateur = $("#newUtilisateur[albcontext^=\"" + currentAlbContext + "\"]").val();
    var newBranche = $("#newBranche[albcontext=\"" + currentAlbContext + "\"]").val();
    var newTypeDroit = $("#newTypeDroit[albcontext=\"" + currentAlbContext + "\"]").val();
    var Albcontext = currentAlbContext;

    if (IsUndifinedOrEmpty(newUtilisateur) ||
        IsUndifinedOrEmpty(newBranche) ||
        IsUndifinedOrEmpty(newTypeDroit)) {

        common.dialog.bigError("Votre saisie est incorrecte", true);
    }
    else {
        var utilisateurBranche = {
            "Utilisateur": newUtilisateur,
            "Branche": newBranche,
            "TypeDroit": newTypeDroit,
            "Albcontext": Albcontext
        };

        $.ajax({
            type: "POST",
            url: "/GestUtilisateurs/AddUtilisateurBranche",
            data: { data: JSON.stringify(utilisateurBranche) },
            success: function (data) {
                var currentAlbContext = $("#currentAlbContext").val();
                $("#utilisateurBrancheRows[albcontext=\"" + currentAlbContext + "\"]").append(data);
                SetNewUtilisateurBranche(currentAlbContext);
            },
            error: function (request) {
                ShowFancyError(request);
            }
        });
    }
}

function SetNewUtilisateurBranche(currentAlbContext) {
    MapCells();
    $("#NewUtilisateurBrancheRow[albcontext=\"" + currentAlbContext + "\"]").hide();
    $("#newUtilisateur[albcontext^=\"" + currentAlbContext + "\"]").val("");
    $("#newBranche[albcontext=\"" + currentAlbContext + "\"]").val("");
    $("#newTypeDroit[albcontext=\"" + currentAlbContext + "\"]").remove();
    CloseLoading();
}

//---------enregister un utilisateur
function SaveUtilisateurBranche(id, currentAlbContext) {   
    ShowLoading();

    var newUtilisateur = $("label[id=\"" + id + "Utilisateur\"][albcontext=\"" + currentAlbContext + "\"]").text();
    var newBranche = $("input[id=\"" + id + "Branche\"][albcontext=\"" + currentAlbContext + "\"]").val();
    var newTypeDroit = $("select[id=\"" + id + "TypeDroit\"][albcontext=\"" + currentAlbContext + "\"] :selected").val();

    if (IsUndifinedOrEmpty(newUtilisateur) ||
        IsUndifinedOrEmpty(newBranche) ||
        IsUndifinedOrEmpty(newTypeDroit)) {
        common.dialog.bigError("Votre saisie est incorrecte", true);
    }
    else {
        var utilisateurBranche = {
            "Utilisateur": newUtilisateur,
            "Branche": newBranche,
            "TypeDroit": newTypeDroit
        };

        $.ajax({
            type: "POST",
            url: "/GestUtilisateurs/UpdateUtilisateurBranche",
            data: { data: JSON.stringify(utilisateurBranche) },
            success: function (data) {
                var currentAlbContext = $("#currentAlbContext").val();
                var splitChar = $("#SplitChar").val();
                var id = data.Utilisateur.replace(" ", "") + splitChar + data.Branche.replace(" ", "");
                HideAllEditCells(currentAlbContext, id);
                CloseLoading();
            },
            error: function (request) {
                ShowFancyError(request);
            }
        });
    }
}

function HideAllEditCells(currentAlbContext, id) {
    //cacher tt les edit
    $(".UpdUtilisateurBrancheEditBtn[albcontext=\"" + currentAlbContext + "\"]").show();
    $("span[id=\"" + id + "\"][class~=cancelUtilisateurBrancheEditBtn][albcontext=\"" + currentAlbContext + "\"]").hide();
    $("span[id=\"" + id + "\"][class~=saveUtilisateurBrancheBtn][albcontext=\"" + currentAlbContext + "\"]").hide();
    $("span[id=\"" + id + "\"][class~=delUtilisateurBrancheBtn][albcontext=\"" + currentAlbContext + "\"]").show();

    $(".editableCell[albcontext=\"" + currentAlbContext + "\"]").hide();
    $(".readOnlyCell[albcontext=\"" + currentAlbContext + "\"]").show();
}

//------------supprimer un utilisateur
function RemoveUtilisateurBranche(id, currentAlbContext) {   

    ShowLoading();

    var newUtilisateur = $("label[id=\"" + id + "Utilisateur\"][albcontext=\"" + currentAlbContext + "\"]").text();
    var newBranche = $("input[id=\"" + id + "Branche\"][albcontext=\"" + currentAlbContext + "\"]").val();
    var newTypeDroit = $("select[id=\"" + id + "TypeDroit\"][albcontext=\"" + currentAlbContext + "\"] :selected").val();
    var utilisateurBranche = {
        "Utilisateur": newUtilisateur,
        "Branche": newBranche,
        "TypeDroit": newTypeDroit
    };

    $.ajax({
        type: "POST",
        url: "/GestUtilisateurs/RemoveUtilisateurBranche",
        data: { data: JSON.stringify(utilisateurBranche) },
        success: function (data) {
            var currentAlbContext = $("#currentAlbContext").val();

            var splitChar = $("#SplitChar").val();
            $("tr[id=\"" + data.Utilisateur.replace(" ", "") + splitChar + data.Branche.replace(" ", "") + "\"][albcontext=\"" + currentAlbContext + "\"]").remove();

            CloseLoading();
        },
        error: function (request) {
            ShowFancyError(request);
        }
    });
}

function MapEditMode(currentAlbContext) {
    $(".clicktoEdit[albcontext=\"" + currentAlbContext + "\"]").die();
    $(".clicktoEdit[albcontext=\"" + currentAlbContext + "\"]").live('click', function () {

        var id = $(this).attr("id");

        $(".editableCell[albcontext=\"" + currentAlbContext + "\"]").hide();
        $(".readOnlyCell[albcontext=\"" + currentAlbContext + "\"]").show();
        $(".cancelUtilisateurBrancheEditBtn[albcontext=\"" + currentAlbContext + "\"]").hide();
        $(".saveUtilisateurBrancheBtn[albcontext=\"" + currentAlbContext + "\"]").hide();
        $(".delUtilisateurBrancheBtn[albcontext=\"" + currentAlbContext + "\"]").show();

        $("span[id=\"" + id + "\"][class~=cancelUtilisateurBrancheEditBtn][albcontext=\"" + currentAlbContext + "\"]").show();
        $("span[id=\"" + id + "\"][class~=saveUtilisateurBrancheBtn][albcontext=\"" + currentAlbContext + "\"]").show();
        $("span[id=\"" + id + "\"][class~=delUtilisateurBrancheBtn][albcontext=\"" + currentAlbContext + "\"]").hide();

        $("#NewUtilisateurBrancheRow[albcontext=\"" + currentAlbContext + "\"]").hide();

        $("tr[id=\"" + id + "\"][albcontext=\"" + currentAlbContext + "\"]").find("td").each(function () {
            if ($(this).hasClass("editableCell")) {
                $(this).show();
            }
            else if ($(this).hasClass("readOnlyCell")) {
                $(this).hide();
            }
        });

        //save Cancellable values
        $(".Cancellable[albcontext=\"" + currentAlbContext + "\"]").each(function () {
            $(this).attr("albSavedValue", $(this).val());
        });
    });
}

function MapDivRechercheGestUtilisateur() {
    $("#btnCancel").die().live('click', function () {
        $("#divRGUMain").html("");
        $("#divRechercheGestUtilisateur").hide();
        ReloadMainScreen("divGestUtlisateurBody", true);
        $("#currentAlbContext").val($("#pageAlbContext").val());
    });

    LoadDivRechercheGestUtilisateur();
}

//------recharger la vue partiel Body
function ReloadMainScreen(divContainer, rechercheActive, CallBackFunc) {
    if ($("#reloadPageBack").hasTrueVal()) {
        ShowLoading();

        var currentAlbContext = $("#currentAlbContext").val();

        var utilisateur = $("#rechercheUtilisateur[albcontext^=\"" + currentAlbContext + "\"]").val();
        var branche = $("#rechercheBranche[albcontext=\"" + currentAlbContext + "\"]").val();
        var typeDroit = $("#rechercheTypeDroit[albcontext=\"" + currentAlbContext + "\"]").val();

        $.ajax({
            type: "POST",
            url: "/GestUtilisateurs/Index",
            data: { "utilisateur": utilisateur, "branche": branche, "typeDroit": typeDroit, "albcontext": $("#pageAlbContext").val(), "rechercheActive": rechercheActive },
            success: function (data) {
                $("#" + divContainer).html(data);
                MapCells();
                MapCommonAutoCompUtilisateur();

                if (!IsUndifinedOrEmpty(CallBackFunc)) {
                    CallBackFunc();
                }
                CloseLoading();
            },
            error: function (request) {
                ShowFancyError(request);
            }
        });
    }
}

//------------charger l'ecran de recherche
function LoadDivRechercheGestUtilisateur() {
    var currentAlbContext = $("#currentAlbContext").val();

    ShowLoading();

    var utilisateur = $("#rechercheUtilisateur[albcontext^=\"" + currentAlbContext + "\"]").val();
    var branche = $("#rechercheBranche[albcontext=\"" + currentAlbContext + "\"]").val();
    var typeDroit = $("#rechercheTypeDroit[albcontext=\"" + currentAlbContext + "\"]").val();

    $.ajax({
        type: "POST",
        url: "/GestUtilisateurs/Index",
        data: { "utilisateur": utilisateur, "branche": branche, "typeDroit": typeDroit, "albcontext": $("#rechercheAlbContext").val() },
        success: function (data) {
            $("#divRGUMain").html(data);
            //affecter le context actuelle
            $("#currentAlbContext").val($("#rechercheAlbContext").val());
            var currentAlbContext = $("#currentAlbContext").val();
            $("#blockRetour[albcontext=\"" + currentAlbContext + "\"]").show();

            MapCells();
            MapCommonAutoCompUtilisateur();
            HideFullScreen();
            CloseLoading();
        },
        error: function (request) {
            ShowFancyError(request);
        }
    });
}
//--------------------test valeur null----------------------------------------
function IsUndifinedOrEmpty(str) {
    if (str == undefined || str == null || str == "") {
        return true;
    }
    return false;
}
//-----------------------resetter le cache-------------------------------------
function ResetUserRight() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestUtilisateurs/ResetUserRight",
        success: function (data) {
            common.dialog.info("La réinitialisation s'est bien déroulée.");
            CloseLoading();
            ReloadMainScreen("divGestUtlisateurBody", true);
        },
        error: function (request) {
            ShowFancyError(request);
        }
    });
}
//------------------Resetter le cache login
function ResetUserLogin() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestUtilisateurs/ResetUserLogin",
        success: function (data) {
            common.dialog.info("La réinitialisation s'est bien déroulée.");
            CloseLoading();
            ReloadMainScreen("divGestUtlisateurBody", true);
        },
        error: function (request) {
            ShowFancyError(request);
        }
    });
}
//------------Reset cache YUTILIS
function ResetYUTILIS() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/GestUtilisateurs/ResetYUTILIS",
        success: function (data) {
            common.dialog.info("La réinitialisation s'est bien déroulée.");
            CloseLoading();
            ReloadMainScreen("divGestUtlisateurBody", true);
        },
        error: function (request) {
            ShowFancyError(request);
        }
    });
}
//---------afficher une message d'erreur
function ShowFancyError(request) {
    common.error.showXhr(request);
}