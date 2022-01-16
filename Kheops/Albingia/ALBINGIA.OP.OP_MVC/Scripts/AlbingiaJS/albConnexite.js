/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />
$(document).ready(function () {
    MapElementPageConnexite();
});

/*
*/
function MapElementPageConnexite(context) {

    MapConnexites(context);

   
    $('#btnAnnulerAjout').die();
    $('#btnAnnulerAjout').live('click', function (evt) {
        $("#divFullScreenAjoutConnexite").hide();
        $("#divDataAjoutConnexite").html('');
        ReactivateShortCut("ajouterConnexite");//todo gérer quand on vient de l'interface plein ecran
    });

    $('#AjouterEngagement').die();
    $('#AjouterEngagement').live('click', function (evt) {
        AjouterConnexite("20");
    });

    $('#AjouterRemplacement').die();
    $('#AjouterRemplacement').live('click', function (evt) {
        AjouterConnexite("01");
    });

    $('#AjouterInformation').die();
    $('#AjouterInformation').live('click', function (evt) {
        AjouterConnexite("10");
    });

    $('#AjouterResiliation').die();
    $('#AjouterResiliation').live('click', function (evt) {
        AjouterConnexite("30");
    });

    $('#AjouterRegularisation').die();
    $('#AjouterRegularisation').live('click', function (evt) {
        AjouterConnexite("40");
    });

    $('#AjouterPleinEcran').die();
    $('#AjouterPleinEcran').live('click', function (evt) {
        AjouterConnexite($("#CodeTypeConnexitePleinEcran").val());
    });

    $('#FullScreenEngagement').die();
    $('#FullScreenEngagement').live('click', function (evt) {
        AfficherPleinEcran("20");
    });

    $('#FullScreenRemplacement').die();
    $('#FullScreenRemplacement').live('click', function (evt) {
        AfficherPleinEcran("01");
    });

    $('#FullScreenInformation').die();
    $('#FullScreenInformation').live('click', function (evt) {
        AfficherPleinEcran("10");
    });

    $('#FullScreenResiliation').die();
    $('#FullScreenResiliation').live('click', function (evt) {
        AfficherPleinEcran("30");
    });

    $('#FullScreenRegularisation').die();
    $('#FullScreenRegularisation').live('click', function (evt) {
        AfficherPleinEcran("40");
    });

    $('#FermerFullScreen').die();
    $('#FermerFullScreen').live('click', function (evt) {
        UpdateConnexites($("#CodeTypeConnexitePleinEcran").val());
        $("#divDataPleinEcranConnexite").html('');
        $("#divFullScreenConnexite").hide();
        $("#PleinEcran").val('');
    });

    $('#RechercherContrat').die();
    $('#RechercherContrat').live('click', function (evt) {
        AfficherRechercheContrat();
    });

    $("#btnAnnulerResultatRechercheContrat").die().live('click', function () {
        $("#divRecherche").hide();
        $("#divFullScreenRecherche").hide();
        ReactivateShortCut("rechercheContrat");
    });

    $("#btnAnnulerRechercheContrat").die().live('click', function () {
        $("#divRecherche").hide();
        $("#divFullScreenRecherche").hide();
        ReactivateShortCut("rechercheContrat");
    });

    $("#btnSelectionnerContrat").die().live('click', function () {
        SelectionContrat();
    });

    $("#btnAjouter").die().live('click', function () {
        SauvegarderConnexite();
    });
    //$("img[name=btnSupprimer]").each(function () {
    //    $(this).click(function () {
    //        $("#hiddenInputId").val($(this).attr("id"));
    //        ShowCommonFancy("ConfirmTrans", "Suppr",
    //            "Vous allez supprimer la connexité de ce contrat avec les autres contrats. Voulez-vous continuer ?",
    //            350, 130, true, true);
    //    });
    //});    
    $("#btnConfirmTransOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Suppr":
                SupprimerConnexite($("#hiddenInputId").val());
                $("#hiddenInputId").val('');
                break;
        }
    });
    $("#btnConfirmTransCancel").die().live('click', function () {
        CloseCommonFancy();
    });

    $("#btnAnnulerConfirmation").die().live('click', function () {
        $("#divDataConfirmationAction").html('');
        $("#divConfirmationAction").hide();

    });
    $("#btnFusionner").die().live('click', function () {
        FusionnerDetacherConnexite('F');
    });
    $("#btnDetacher").die().live('click', function () {
        FusionnerDetacherConnexite('D');
    });


}

function AjouterConnexite(codeTypeConnexite, pleinEcran) {
    ShowLoading();
    var codeObservation;
    var observation;
    var numConnexite;
    var contexte = "";
    if ($("#PleinEcran").val() == "True") {
        codeObservation = $("#CodeObservationPleinEcran").val();
        observation = $("#ObservationPleinEcran").val();
        numConnexite = $("#NumConnexitePleinEcran").val();
    }
    else {
        switch (codeTypeConnexite) {

            case "20":
                codeObservation = $("#CodeObservationEngagement").val();
                observation = $("#ObservationEngagement").val();
                numConnexite = $("#NumConnexiteEngagement").val();
                contexte = "eng";
                break;
            case "01":
                codeObservation = $("#CodeObservationRemplacement").val();
                observation = $("#ObservationRemplacement").val();
                numConnexite = $("#NumConnexiteRemplacement").val();
                contexte = "rem";
                break;

            case "10":
                codeObservation = $("#CodeObservationInformation").val();
                observation = $("#ObservationInformation").val();
                numConnexite = $("#NumConnexiteInformation").val();
                contexte = "inf";
                break;
            case "30":
                codeObservation = $("#CodeObservationResiliation").val();
                observation = $("#ObservationResiliation").val();
                numConnexite = $("#NumConnexiteResiliation").val();
                contexte = "res";
                break;
            case "40":
                codeObservation = $("#CodeObservationRegularisation").val();
                observation = $("#ObservationRegularisation").val();
                numConnexite = $("#NumConnexiteRegularisation").val();
                contexte = "reg";
                break;
            default:
                codeObservation = $("#CodeObservationEngagement").val();
                observation = $("#ObservationEngagement").val();
                numConnexite = $("#NumConnexiteEngagement").val();
                contexte = "eng";
                break;
        }
    }
    $.ajax({
        type: "POST",
        url: "/Connexite/AjouterConnexite",
        data: {
            codeTypeConnexite: codeTypeConnexite, codeObservation: codeObservation, observation: observation, numConnexite: numConnexite
        },
        success: function (data) {
            DesactivateShortCut("ajouterConnexite");
            $("#divDataAjoutConnexite").html(data);
            AlbScrollTop();
            $("#divFullScreenAjoutConnexite").show();
            common.autonumeric.apply($("#connexiteEngagementAliment"), 'init', 'numeric', '', null, null, '99999', '0');
            MapElementPageConnexite(contexte);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    }).then(getBodyRechercheForConnexite);
}

function getBodyRechercheForConnexite() {
    return $.ajax({
        type: "GET",
        url: "/Connexite/GetBodyRechercheSaisieForConnexites",
        data: {
            
        },
        success: function(data) {
            $("#divDataRechercheContrat").html(data);
        },
        error: function(request) {
            common.error.showXhr(request);
        }
    });
}

//--------fonction qui sauvegarde crée une connnexité d'un contrat
function SauvegarderConnexite() {
    ShowLoading();
    $('#msgErreurDiv').html('');
    var codeTypeConnexite = $("#CodeTypeConnexite").val();
    var numConnexite = $("#NumConnexite").val();
    var pleinEcran = $("#PleinEcran").val();
    var codeObservation;
    var ideConnexite;
    var versionConnexe = $("#connexiteEngagementAliment").val();
    var codeOffreConnexe = $("#connexiteEngagementContrat").val();

    // validation
    var error = false;
    var versionConnexe = $("#connexiteEngagementAliment").val();
    if (versionConnexe == "") {
        $("#connexiteEngagementAliment").addClass("requiredField");
        error = true;
    }
    else {
        $("#connexiteEngagementAliment").removeClass("requiredField");
    }

    var codeOffreConnexe = $("#connexiteEngagementContrat").val();
    if (codeOffreConnexe == "") {
        $("#connexiteEngagementContrat").addClass("requiredField");
        error = true;
    }
    else {
        $("#connexiteEngagementContrat").removeClass("requiredField");
    }

    if (error) {
        CloseLoading();
        return false;
    }

    if (pleinEcran == "True") {
        codeObservation = $("#CodeObservationPleinEcran").val();
        ideConnexite = $("#IdeConnexitePleinEcran").val();
    }
    else {
        switch (codeTypeConnexite) {
            case "20":
                codeObservation = $("#CodeObservationEngagement").val();
                ideConnexite = $("#IdeConnexiteEngagement").val();
                break;
            case "01":
                codeObservation = $("#CodeObservationRemplacement").val();
                break;
            case "10":
                codeObservation = $("#CodeObservationInformation").val();
                ideConnexite = $("#IdeConnexiteInformation").val();
                break;
            case "30":
                codeObservation = $("#CodeObservationResiliation").val();
                ideConnexite = $("#IdeConnexiteResiliation").val();
                break;
            case "40":
                codeObservation = $("#CodeObservationRegularisation").val();
                ideConnexite = $("#IdeConnexiteRegularisation").val();
                break;
            default:
                codeObservation = $("#CodeObservationEngagement").val();
                ideConnexite = $("#IdeConnexiteEngagement").val();
                break;
        }
    }
    $.ajax({
        type: "POST",
        url: "/Connexite/SauvegarderConnexite",
        dataType: 'html',
        data: {
            codeOffreConnexe: codeOffreConnexe, versionConnexe: versionConnexe, codeTypeConnexite: codeTypeConnexite,
            numConnexite: numConnexite, observation: $("#Commentaire").val(), codeObservation: codeObservation,
            codeOffreCourant: $("#CodeContrat").val(), versionCourant: $("#VersionContrat").val(), typeOffreCourant: $("#TypeContrat").val(),
            branche: $("#connexiteBranche").val(), sousBranche: $("#connexiteSousBranche").val(), Categorie: $("#connexiteCategorie").val()
        },
        success: function (data) {
            CloseLoading();
            if (data == "Connexe") {
                AfficherConfirmationAction(codeTypeConnexite, numConnexite, $("#Commentaire").val(), codeObservation, $("#CodeContrat").val(), $("#TypeContrat").val(), $("#VersionContrat").val(), $("#connexiteBranche").val(), $("#connexiteSousBranche").val(), $("#connexiteCategorie").val(), ideConnexite,
                    $("#connexiteEngagementContrat").val(), $("#connexiteEngagementAliment").val());
            }
            else {
                $('#msgErreurDiv').append(data);
                if ($('#msgErreurDiv').is(':empty')) {
                    if (pleinEcran == "True") {
                        UpdateConnexitesPleinEcran(codeTypeConnexite);
                    }
                    else {
                        UpdateConnexites(codeTypeConnexite);
                    }
                    $("#divDataAjoutConnexite").html('');
                    $("#divFullScreenAjoutConnexite").hide();
                    ReactivateShortCut("ajouterConnexite");
                }
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function SauvegarderObservationConnexite(commentaire, codeObservation) {
    if (!isNaN(codeObservation)) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/Connexite/SauvegarderObservationConnexite",
            dataType: 'html',
            data: {
                codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
                observation: commentaire, codeObservation: codeObservation, acteGestion: $("#ActeGestion").val(), addParamValue: $("#AddParamValue").val()
            },
            success: function (data) {
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

///-------fonction qui suppimme la connexité d'un contrat avec les autres contrats
function SupprimerConnexite(elem) {
    ShowLoading();
    var codeOffreConnexe = elem.split('_')[1];
    var versionConnexe = elem.split('_')[2];
    var typeOffreConnexe = elem.split('_')[3];
    var numConnexite = elem.split('_')[4];
    var codeTypeConnexite = elem.split('_')[5];
    var ideConnexite = elem.split('_')[6];

    $.ajax({
        type: "POST",
        url: "/Connexite/SupprimerConnexite",
        data: {
            codeOffreConnexe: codeOffreConnexe, versionConnexe: versionConnexe, typeOffreConnexe: typeOffreConnexe, numConnexite: numConnexite, codeTypeConnexite: codeTypeConnexite, ideConnexite: ideConnexite
        },
        success: function () {
            if ($("#PleinEcran").val() == "True") {
                UpdateConnexitesPleinEcran(codeTypeConnexite);
            }
            else {
                UpdateConnexites(codeTypeConnexite);
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
///--------fonction qui met à jour la liste des contrats connexes sur le plein ecrant----------------------
function UpdateConnexitesPleinEcran(codeTypeConnexite) {
    var isConnexiteReadOnly = $("#IsConnexiteReadonly").val();

    var context = "";
    switch (codeTypeConnexite) {
        case "20":
            context = "eng";
            break;
        case "01":
            context = "rem";
            break;
        case "10":
            context = "inf";
            break;
        case "30":
            context = "res";
            break;
        case "40":
            context = "reg";
            break;
        default:
            context = "eng";
            break;
    }
    $.ajax({
        type: "POST",
        url: "/Connexite/AfficherConnexitePleinEcran",
        data: {
            numContrat: $("#CodeContrat").val(), version: $("#VersionContrat").val(), type: $("#TypeContrat").val(), codeTypeConnexite: codeTypeConnexite, isConnexiteReadOnly: isConnexiteReadOnly
        },
        success: function (data) {
            $("#divDataPleinEcranConnexite").html(data);

            //gestion de l'affichage de l'écran en mode readonly
            if (window.isReadonly && $("#IsModifHorsAvn").val() === "False" ) {
                $("img[name=btnAjouter]").hide();
                $("img[name=btnSupprimer]").hide();
            }
            AlternanceLigne("PleinEcranBody", "", false, null);
            AlbScrollTop();
            MapElementPageConnexite(context);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------------fonction qui met à jour la liste des contrats connexes----------------------
function UpdateConnexites(codeTypeConnexite) {
    var numeroContrat = $("#CodeContrat").val();
    var version = $("#VersionContrat").val();
    var isConnexiteReadOnly = $("#IsConnexiteReadonly").val();
    $.ajax({
        type: "POST",
        url: "/Connexite/UpdateConnexites",
        data: { numeroContrat: numeroContrat, version: version, codeTypeConnexite: codeTypeConnexite, isConnexiteReadOnly: isConnexiteReadOnly },
        success: function (data) {
            var context = "";
            switch (codeTypeConnexite) {
                case "20":

                    $("#divEngagement[albtypeaff=CONSULT]").html(data);
                    $("#divEngagement[albtypeaff=CONSULT] #divPeriodesConnexites").remove();
                    
                    $("#divEngagement[albtypeaff=CONSULT] #AjouterEngagement").hide();
                    $("#divEngagement[albtypeaff=CONSULT] img[name=btnSupprimer]").hide();
                    $("#divEngagement[albtypeaff=CONSULT] img[name=btndetails]").removeAttr("style");
                    
                    $("#divEngagement[albtypeaff=GESTION]").html(data);
                    context = "eng";
                    break;
                case "01":
                    $("#divBodyRemplacement").html(data);
                    context = "rem";
                    break;
                case "10":
                    $("#divBodyInformation").html(data);
                    context = "inf";
                    break;
                case "30":
                    $("#divBodyResiliation").html(data);
                    context = "res";
                    break;
                case "40":
                    $("#divBodyRegularisation").html(data);
                    context = "reg";
                    break;
                default:
                    $("#divBodyEngagement").html(data);
                    context = "eng";
                    break;
            }
            MapElementPageConnexite(context);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function AfficherPleinEcran(codeTypeConnexite) {
    $("#PleinEcran").val("True");
    var isConnexiteReadOnly = $("#IsConnexiteReadonly").val();
    $.ajax({
        type: "POST",
        url: "/Connexite/AfficherConnexitePleinEcran",
        data: {
            numContrat: $("#CodeContrat").val(), version: $("#VersionContrat").val(), type: $("#TypeContrat").val(), codeTypeConnexite: codeTypeConnexite, isConnexiteReadOnly: isConnexiteReadOnly
        },
        success: function (data) {
            $("#divDataPleinEcranConnexite").html(data);

            //gestion de l'affichage de l'écran en mode readonly
            if (window.isReadonly && $("#IsModifHorsAvn").val() === "False") {
                $("img[name=btnAjouter]").hide();
                $("img[name=btnSupprimer]").hide();
            }
            AlternanceLigne("PleinEcranBody", "", false, null);
            AlbScrollTop();

            var context = "";
            switch (codeTypeConnexite) {
                case "20":
                    context = "eng";
                    break;
                case "01":
                    context = "rem";
                    break;
                case "10":
                    context = "inf";
                    break;
                case "30":
                    context = "res";
                    break;
                case "40":
                    context = "reg";
                    break;
                default:
                    context = "eng";
                    break;
            }

            $("#divFullScreenConnexite").show();
            MapElementPageConnexite(context);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function AfficherRechercheContrat() {
    $("#divFullScreenRecherche").show();
    DesactivateShortCut("rechercherContrat");
    ReactivateShortCut("resultatRecherche");
    $("#btnInitialize").trigger('click');
}

function AfficherConfirmationAction(codeTypeConnexite, numConnexite, observationActuelle, codeObservationActuelle, codeOffreActuelle, typeOffreActuelle, versionOffreActuelle, brancheActuelle, sousBrancheActuelle, catActuelle, ideConnexiteActuelle, codeOffreAjoutee, versionOffreAjoutee) {
    $.ajax({
        type: "POST",
        url: "/Connexite/AfficherConfirmationAction",
        data: {
            codeTypeConnexite: codeTypeConnexite, numConnexite: numConnexite, observationActuelle: observationActuelle, codeObservationActuelle: codeObservationActuelle, codeOffreActuelle: codeOffreActuelle, typeOffreActuelle: typeOffreActuelle,
            versionOffreActuelle: versionOffreActuelle, brancheActuelle: brancheActuelle, sousBrancheActuelle: sousBrancheActuelle, catActuelle: catActuelle, ideConnexiteActuelle: ideConnexiteActuelle, codeOffreAjoutee: codeOffreAjoutee, versionOffreAjoutee: versionOffreAjoutee
        },
        success: function (data) {
            $("#divDataConfirmationAction").html(data);
            AlbScrollTop();
            $("#divConfirmationAction").show();
            AlternanceLigne("GrpActuelBody", "", false, null);
            AlternanceLigne("GrpOrigineBody", "", false, null);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function SelectionContrat() {
    //----contrat sélectionné : 
    var contratSelectionne = $("#selectedOffreContrat").val();
    $("#connexiteEngagementContrat").val(contratSelectionne.split('_')[0]);
    $("#connexiteEngagementAliment").val(contratSelectionne.split('_')[1]);

    var contratMere = contratSelectionne.split('_')[4];
    switch (contratMere) {
        case "A":
            contratMere += " - Aliment";
            break;
        case "M":
            contratMere += " - Mère";
            break;
    }

    $("#connexiteContratMere").val(contratMere);
    $("#divRecherche").hide();
    $("#divFullScreenRecherche").hide();
    ReactivateShortCut("rechercheContrat");
}
//-------fonction qui fusionne ou detache deux groupes de connexités
function FusionnerDetacherConnexite(modeAction) {

    var numOffreOrigine = $("#NumOffreOrigine").val();
    var typeOffreOrigine = $("#TypeOffreOrigine").val();
    var versionOffreOrigine = $("#VersionOffreOrigine").val();
    var brancheOrigine = $("#BrancheOrigine").val();
    var sousBrancheOrigine = $("#SousBrancheOrigine").val();
    var categorieOrigine = $("#CategorieOrigine").val();
    var numConnexiteOrigine = $("#NumConnexiteOrigine").val();
    var codeObsvOrigine = $("#CodeObservationConnexiteOrigine").val();
    var obsvOrigine = $("#ObservationConnexiteOrigine").val();
    var ideConnexiteOrigine = $("#IdeConnexiteOrigine").val();

    var numOffreActuelle = $("#NumOffreActuelle").val();
    var typeOffreActuelle = $("#TypeOffreActuelle").val();
    var versionOffreActuelle = $("#VersionOffreActuelle").val();
    var brancheActuelle = $("#BrancheActuelle").val();
    var sousBrancheActuelle = $("#SousBrancheActuelle").val();
    var categorieActuelle = $("#CategorieActuelle").val();
    var numConnexiteActuelle = $("#NumConnexiteActuelle").val();
    var codeObsvActuelle = $("#CodeObservationConnexiteActuelle").val();
    var obsvActuelle = $("#ObservationConnexiteActuelle").val();

    var codeTypeConnexite = $("#CodeTypeConnexite").val();


    $.ajax({
        type: "POST",
        url: "/Connexite/FusionnerDetacherConnexite",
        data: {
            numOffreOrigine: numOffreOrigine, typeOffreOrigine: typeOffreOrigine, versionOffreOrigine: versionOffreOrigine, brancheOrigine: brancheOrigine, sousBrancheOrigine: sousBrancheOrigine, categorieOrigine: categorieOrigine,
            numConnexiteOrigine: numConnexiteOrigine, codeObsvOrigine: codeObsvOrigine, obsvOrigine: obsvOrigine, ideConnexiteOrigine: ideConnexiteOrigine,
            numOffreActuelle: numOffreActuelle, typeOffreActuelle: typeOffreActuelle, versionOffreActuelle: versionOffreActuelle, obsvActuelle: obsvActuelle, codeObsvActuelle: codeObsvActuelle, brancheActuelle: brancheActuelle, sousBrancheActuelle: sousBrancheActuelle, categorieActuelle: categorieActuelle,
            modeAction: modeAction, codeTypeConnexite: codeTypeConnexite, numConnexiteActuelle: numConnexiteActuelle
        },
        success: function (data) {
            $("#divDataConfirmationAction").html('');
            $("#divConfirmationAction").hide();
            $("#divDataAjoutConnexite").html('');
            $("#divFullScreenAjoutConnexite").hide();
            if ($("#PleinEcran").val() == "True")
                UpdateConnexitesPleinEcran(codeTypeConnexite);
            else UpdateConnexites(codeTypeConnexite);
        },
        error: function (request) {
            common.error.showXhr(request);
            $("#divDataConfirmationAction").html('');
            $("#divConfirmationAction").hide();

        }
    });
}
//-------fonction qui s'execute lorsque l'utilisateur a terminé de modifier un commentaire
function callBackFunctionOnHide(context) {
    if (context != undefined && context != "") {
        var value = $("#zoneTxtArea[albContext=" + context + "]").html();
        var codeObsv = $("input[name=codeObservation][albcontext=" + context.split("_")[0] + "]").val();

        $("div[id=zoneTxtArea][albcontext^=" + context.split("_")[0] + "]").each(function () {
            $(this).html(value.split('\n')[0]);
        });
        $("div[id=zoneTxtArea][albcontext^=" + context.split("_")[0] + "]").each(function () {
            $(this).attr('title', value);
        });
        $("textarea[id^=" + context.split("_")[0] + "]").each(function () {
            $(this).html(value);
        });
        $("td.col_Commentaires_Body[albcontext^=" + context.split("_")[0] + "]").each(function () {
            $(this).attr('title', value);
        });

        $("td.col_Commentaires_Body_PleinEcran[albcontext^=" + context.split("_")[0] + "]").each(function () {
            $(this).attr('title', value);
        });
        SauvegarderObservationConnexite(value, codeObsv);
        $("input[name=observation][albcontext=" + context.split("_")[0] + "]").val(value);
        $("div[name=txtAreaLnk]").show();
    }
}
//-------fonction qui s'execute lorsque l'utilisateur commence à modifier un commentaire
function callBackFunctionOnDisplay(context) {
    if (context != undefined && context != "") {
        //var value = $("#zoneTxtArea[albContext=" + context + "]").html();
        var value = $("div[id=zoneTxtArea][albContext=" + context + "]").html();
        $("div[id=zoneTxtArea][albcontext^=" + context.split("_")[0] + "]").each(function () {
            $(this).html(value.split('\n')[0]);
        });
        $("div[name=txtAreaLnk]").not($("div[albcontext=" + context + "]")).hide();
    }
}