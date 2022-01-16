$(document).ready(function () {
    MapElementPageGestionDoc(true);
});
//--------Map les éléments de la page-----------
function MapElementPageGestionDoc(firstLoad) {
    $("#LienDoc").die().live("click", function () {
        OpenSuiviDocuments(false);
    });

    $("#btnTerminer").die().live("click", function () {
        Redirection("RechercheSaisie", "Index");;
    });
    $("td[albIdDest]").each(function () {
        $(this).mouseover(function () {
            ShowInfoDestinataire($(this));
        });
        $(this).mouseout(function () {
            $("#divDestinataire").html("").hide();
        });
    });
    $("#btnAnnuler").die().live("click", function () {
        switch ($("#Offre_Type").val()) {
            case "O":
               // Redirection("FinOffre", "Index");
                Redirection("Cotisations", "Index");
                break;
            case "P":
                if ($("#ActeGestion").val() === "ATTES") {
                    ShowCommonFancy("Confirm", "CancelAttes", "Attention, vous allez retourner sur l'écran de création de l'attestation et perdre toutes vos données.<br>Voulez-vous continuer ?", 300, 80, true, true);
                }
                else {
                    if (($("#IsChekedEch").val() === "True") && ($("#ActeGestion").val() === "AVNRS")) {
                        Redirection("AnnulationQuittances", "Index");
                    }
                    else {
                        // 2016-03-08 : changement de redirection bug 1872
                        Redirection("Quittance", "Index");
                        //if ($("#ActeGestion").val() == "AVNRS") {
                        //    Redirection("Quittance", "Index");
                        //}
                        //else {
                        //    Redirection("ControleFin", "Index");
                        //}
                    }
                }
                break;
        }
    });

    
    $("#btnValiderEditDoc").die().live("click", function () {
        var context = $(this).attr("albcontext");
        if (context == "editer") {
            $("#hiddenAction").val("Editer") ;
        }
        else {
            ValiderOffreContrat('valider');
        }
        switch ($("#hiddenAction").val()) {
            case "DeleteLot":
                DeleteLotDocumentGestion();
                break;
            case 'Editer':
                ValiderOffreContrat('editer');
                break;
            case "KeepDoc":
                RegenerateDoc("");
                break;
            case "CancelAttes":
                Redirection("CreationAttestation", "Index");
                break;
        }
        $("#hiddenAction").val("");
    });
    $("#btnEditer").die().live("click", function () {
        OpenEdition("editer");
    });
    $("#btnRetourRecherche").die().live("click", function () {
        Redirection("RechercheSaisie", "Index");
    });

    $("#btnSuivant").kclick(function (evt, data) {
        if ($("#checkSynthese").isChecked()) {
            Redirection(data && data.returnHome ? "RechercheSaisie" : "SyntheseDocuments", "Index");
            if (data && data.returnHome) {
                DeverouillerUserOffres(common.tabGuid);
            }
        }
        else {
            Redirection(data && data.returnHome ? "RechercheSaisie" : "ValidationOffre", "Index");
        }
    });

    $("#addDoc").die().live("click", function () {
        if ($(this).hasClass('CursorPointer')) {
            OpenDetailsLot(0);
        }
    });

    $("img[name=editLot]").kclick(function () {
        var idDoc = $(this).attr("id");
        if (idDoc != undefined) {
            idDoc = idDoc.split("_")[1];
            OpenDetailsLot(idDoc);
        }
    });

    $("#btnPrintDoc").die().live("click", function () {
        common.dialog.bigError("En cours de développement", true);
    });

    $("td[name='tdNomDoc']").each(function () {
        $(this).click(function () {
            var id = $(this).attr("albdocid");
            GetFileNameDocument($("#wDocDocType").val(), id, false, "P");
        });
    });

    $("td[name='tdNomDocLibre']").each(function () {
        $(this).click(function () {
            var id = $(this).attr("albdocid");
            GetFileNameDocument($("#wDocDocType").val(), id, false, "P", null, null, true, true);
        });
    });

    $("input[type='checkbox'][name='checkDoc']").each(function () {
        $(this).change(function () {
            ChangeSituationDoc($(this));
        });
    });

    if (firstLoad && $("#tblBodyDocumentLibre tr").length > 1 && $("#OffreReadOnly").val() === "False") {
        $("#divDocLibre").show();
        $("#divBodyDocument").html("");
    }
    
    $("#chckAllDoc").die().live("click", function () {
        var isChecked = $(this).is(":checked");
        if (isChecked)
            $("input[type='checkbox'][name='checkDocLibre']").attr("checked", "checked");
        else
            $("input[type='checkbox'][name='checkDocLibre']").removeAttr("checked");
    });

    $("#btnDocOk").die().live("click", function () {
        var docsId = [];
        $("input[type='checkbox'][name='checkDocLibre']:checked").each(function () {
            docsId. push($(this).attr("albDocId"));
        });
        if (docsId.length > 0) {
            RegenerateDoc(docsId);
        }
        else {
            ShowCommonFancy("Confirm", "KeepDoc", "Tous les documents non sélectionnés vont être supprimés. Etes-vous sûr de vouloir continuer ?", 300, 80, true, true, true);
        }
        //$("#divDocLibre").hide();
    });

    $("#btnConfirmCancel").die().live("click", function () {
        CloseCommonFancy();
    });
    
}
//--------Affiche les informations du destinataire---------
function ShowInfoDestinataire(elem) {
    var idDest = elem.attr("albIdDest");
    var typeDest = elem.attr("albTypeDest");
    var typeEnvoi = elem.attr("albTypeEnvoi");

    $.ajax({
        type: "POST",
        url: "/DocumentGestion/ShowInfoDest",
        data: { idDest: idDest, typeDest: typeDest, typeEnvoi: typeEnvoi },
        success: function (data) {
            $("#divDestinataire").html(data);
            var pos = elem.position();
            $("#divDestinataire").css({ "position": "absolute", "top": pos.top + 20 + "px", "left": pos.left + 5 + "px" });
            $("#divDestinataire").show();

        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------------Redirection----------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var paramRedirect = $("#txtParamRedirect").val();
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    $.ajax({
        type: "POST",
        url: "/DocumentGestion/Redirection/",
        data: {
            cible: cible, job: job, codeOffre: codeOffre, version: version, type: type,
            tabGuid: tabGuid,
            paramRedirect: paramRedirect, modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue
        },
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Change la situation du document-------
function ChangeSituationDoc(elem) {
    ShowLoading();

    var idDoc = elem.attr("id").replace("checkId", "");
    var isCheck = elem.is(":checked") ? "O" : "N";

    $.ajax({
        type: "POST",
        url: "/DocumentGestion/ChangeSituationDoc",
        data: { idDoc: idDoc, isCheck: isCheck },
        success: function () {
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Regénère les documents libres sélectionnés--------
function RegenerateDoc(docsId) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/DocumentGestion/RegenerateDocLibre",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(),
            codeAvt: $("#NumAvenantPage").val(),
            modeNavig: $("#ModeNavig").val(), isReadOnly: $("#OffreReadOnly").val(), idsDoc: docsId,
            addParamValue: $("#AddParamValue").val(),
            acteGestion:$("#ActeGestion").val()
        },
        traditional: true,
        success: function (data) {
            $("#divBodyDocument").html(data);
            MapElementPageGestionDoc();
            $("#divDocLibre").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function OpenEdition(modeEcran) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var modeNavig = $("#ModeNavig").val();
    var codeAvn = $("#NumAvenantPage").val();
    var tabGuid = $("#tabGuid").val();

    $.ajax({
        type: "POST",
        url: "/ValidationOffre/LoadDocumentsEditables/",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, modeEcran: modeEcran, modeNavig: modeNavig,
            acteGestion: $("#ActeGestion").val(), saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(), tabGuid: tabGuid
        },
        success: function (data) {
            if (!(data.indexOf("window.location") >= 0)) {
                $("#divDataEditionDocument").html(data);
                MapElementEditDoc();
                $("#divEditionDocument").show();
                CloseLoading();
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function MapElementEditDoc() {
    $("#btnAnnulerEditDoc").die().live("click", function () {
        $("#divDataEditionDocument").html("");
        $("#divEditionDocument").hide();
    });

  /* $("#btnValiderEditDoc").die().live("click", function () {
        var context = $(this).attr("albcontext");
        if (context == "editer") {
           ShowCommonFancy("Confirm", "Editer",
                "Vous allez éditer les documents du dossier " + codeOffre + ".<br/>Les documents ci avant vont être édités et sauvegardés<br/>Confirmez-vous l'édition ?<br/>",
                350, 130, true, true);
        }
        else {
            ValiderOffreContrat('valider');
        }
    });*/

    AlternanceLigne("Traite", "", null, false);
    AlternanceLigne("DetailsBodyCotisation", "", null, false);

    $("td[name='tdNomDoc']").each(function () {
        $(this).click(function () {
            ShowLoading();
            var id = $(this).attr("albdocid");
            GetFileNameDocument($("#wDocDocType").val(), id, false, "V", "", "", true);
            CloseLoading();
        });
    });

    common.autonumeric.applyAll("init", "decimal", null, null, null, "999999999.99", "-999999999.99");
}

//---------valide l'offre/le contrat
function ValiderOffreContrat(mode) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();

    var codeObservation = $("#INCodeObservation").val();
    var observation = $("#Observation").val();
    var validable = $("#OffreComplete").val();

    var isComplet = $("#OffreComplete").val();
    var motif = $("#Motif").val();

    var lotsId = "";
    $("input[type='checkbox']:checked").each(function () {
        if ((";" + lotsId + ";").replace(";" + $(this).attr('alblotid') + ";", ";") == (";" + lotsId + ";"))
            lotsId += ";" + $(this).attr('alblotid');
    });
    if (lotsId != "")
        lotsId = lotsId.substring(1);

    var docsId = "";
    $("input[type='checkbox'][name='checkDoc']:checked").each(function () {
        if ((";" + docsId + ";").replace(";" + $(this).attr('albdocid') + ";", ";") == (";" + docsId + ";"))
            docsId += ";" + $(this).attr('albdocid');
    });
    if (docsId != "")
        docsId = docsId.substring(1);

    var avenant = $("#NumAvenantPage").val();
    $.ajax({
        type: "POST",
        url: "/ValidationOffre/ValiderOffreContrat/",
        data: {
            codeOffre: codeOffre, version: version, type: type, avenant: avenant, acteGestion: $("#ActeGestion").val(), tabGuid: tabGuid, codeObservation: codeObservation,
            observation: observation, mode: mode, validable: validable, complet: isComplet, motif: motif, lotsId: lotsId, modeNavig: $("#ModeNavig").val(), addParamValue: $("#AddParamValue").val()
        },
        success: function (data) {
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//#region Créer/Modifier document
//#####################################
//---------Créer/Modifier document----
//#####################################

function OpenDetailsLot(idLot) {
    ShowLoading();
    var codeOffreContrat = $("#Offre_CodeOffre").val();
    var versionOffreContrat = $("#Offre_Version").val();
    $.ajax({
        type: "POST",
        url: "/DocumentGestion/GetDetailsDocument",
        data: { codeDocument: idLot, codeOffreContrat: codeOffreContrat, versionOffreContrat: versionOffreContrat },
        success: function (data) {
            $("#divDataModifierLot").html(data);
            MapElementsDetails();
            $("#divModifierLot").show();

            if (idLot === 0) {
                $("#divDestinataireNew").show();
                $("#divDestinatairesBody").hide();
            }

            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function MapElementsDetails() {
    $("#DDLDocument").die().live("change", function () {
        UpdateComponentsSelectDocument();
    });

    $("#btnRechercherCourrierType").die().live("click", function () {
        if ($("#btnRechercherCourrierType").hasClass("CursorPointer")) {
            if ($("#DDLDocument").val() === "LETTYP")
                OpenSelectCourrierType();
        }
    });

    $("#btnAjouterDestinataire").die().live("click", function () {
        if ($(this).hasClass("CursorPointer")) {
            $("#divDestinataireNew").show();
            $("#divDestinatairesBody").hide();
        }
    });

    $("#btnAnnulerDetailsDocument").die().live("click", function () {
        ShowLoading();
        RefreshListeDocument().always(CloseLoading);
        $("#divModifierLot").hide();
    });

    $("#btnEnregistrerDetailsDocument").die().live("click", function () {
        var idDoc = $("select[name='DDLTypeDestinataire']:visible").attr("id").split("_")[1];
        if (idDoc != undefined) {
            SaveDestinataireLigne(idDoc);
        }
        //SaveInfoComplementairesDetailsDocument();
    });
    
    $("#btnConfirmCancel").live("click", function () {
        CloseCommonFancy();
        $("#hiddenAction").val("");
    });

    $("#btnSupprimerDetailsDocument").die().live("click", function () {
        ShowCommonFancy("Confirm", "DeleteLot", "Attention, vous allez supprimer cette liste d'envoi.<br> Voulez-vous continuer?", 450, 80, true, true);
    });

    $("#save_-1").die().live("click", function () {
        SaveDestinataireLigne(-1);
    });

    MapElementsTableauDetails();
    UpdateComponentsSelectDocument();
}

//Met à jour les composants de selection d'un document
function UpdateComponentsSelectDocument() {
    if ($("#DDLDocument").val() === "LETTYP") {
        $("#INCourrierType").removeAttr("readonly").removeAttr("disabled").removeClass("readonly");
        $("#btnRechercherCourrierType").addClass("CursorPointer");
        $("#btnRechercherCourrierType").attr("src", "/Content/Images/loupe.png");
    }
    else {
        $("#INCourrierType").attr("readonly", "readonly").attr("disabled", "disabled").addClass("readonly");
        $("#btnRechercherCourrierType").removeClass("CursorPointer");
        $("#btnRechercherCourrierType").attr("src", "/Content/Images/loupegris.png");
    }
}

//Map les éléments du tableau de détails documents
function MapElementsTableauDetails() {
    $("img[name=selectDestinataire]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            var id = $(this).attr("id");
            var typeDestinataire;
            if (id != undefined && id !== "") {
                id = id.split("_")[1];
                typeDestinataire = $("#INTypeDestinataireCode_" + id).val();
                if (typeDestinataire != undefined && typeDestinataire !== "")
                    OpenSelectDestinataire(id, typeDestinataire);
            }
        });
    });


    $("input[type=radio][name=radioSelectPrincipal]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            var id = $(this).attr("id");
            if (id != undefined && id !== "") {
                id = id.split("_")[1];
                $("#selectedIsPrincipal").val(id);
            }
        });
    });

    $("img[name=btnSaveDestinataire][albcontext=editLine]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            var id = $(this).attr("id");
            if (id != undefined && id !== "") {
                id = id.split("_")[1];
                if (id > 0)
                    SaveDestinataireLigne(id);
            }
        });
    });

    $("img[name=btnSupprimerDestinataire]").each(function () {
        $(this).die();
        $(this).live("click", function () {
            var id = $(this).attr("id");
            if (id != undefined && id !== "") {
                id = id.split("_")[1];
                DeleteDestinataireLigne(id);
            }
        });
    });

    $("select[name=DDLTypeDestinataire]").each(function () {
        //init
        ActualisationAutocomplete($(this));
        $(this).die();
        $(this).live("change", function () {
            var id = $(this).attr("id");
            if (id != undefined && id !== "") {
                id = id.split("_")[1];
                var typeSelect = $(this).val();

                $.ajax({
                    type: "POST",
                    url: "/DocumentGestion/GetLigneCodeLabelDestinataire",
                    data: { guid: id, typeDest: typeSelect },
                    success: function (data) {
                        $("#divCodeLabelDest_" + id).html(data);

                        $("#INTypeDestinataireCode_" + id).val(typeSelect);
                        //Raz des champs destinataire
                        $("#INCodeDestinataire_" + id).val("");
                        $("#INNomDestinataire_" + id).val("");
                        $("#INCodeInterlocuteur_" + id).val("");
                        $("#INNomInterlocuteur_" + id).val("");
                        ActualisationAutocomplete($("#DDLTypeDestinataire_" + id));
                        MapElementsTableauDetails();
                    },
                    error: function (request) {
                        common.error.showXhr(request);
                    }
                });


            }
        });
    });

    //AlternanceLigne("DestinatairesBody", "", false, null);

    if ($("#tblDestinatairesBody tr").length > 0) {
        $("#btnAjouterDestinataire").removeClass("CursorPointer").attr("src", "/Content/Images/plusajouter_gris1616.jpg");
    }
    else {
        $("#btnAjouterDestinataire").addClass("CursorPointer").attr("src", "/Content/Images/plusajouter1616.png");
    }

}

function ActualisationAutocomplete(drlTypeDestinataire) {
    var id = drlTypeDestinataire.attr("id");
    if (id != undefined && id !== "") {
        id = id.split("_")[1];
        var typeSelect = drlTypeDestinataire.val();
        //Actualisation des autocomplete
        switch (typeSelect) {
            case "AS":
            case "ASS":
                $("#INCodeDestinataire_" + id).removeAttr("disabled").removeClass("readonly");
                $("#INNomDestinataire_" + id).removeAttr("disabled").removeClass("readonly");
                $("#INCodeDestinataire_" + id).attr("albAutoComplete", "autoCompCodeAssure");
                $("#INNomDestinataire_" + id).attr("albAutoComplete", "autoCompNomAssure");
                $("#INCodeInterlocuteur_" + id).removeAttr("albAutoComplete");
                $("#INNomInterlocuteur_" + id).removeAttr("albAutoComplete");
                MapCommonAutoCompAssure();
                break;
            case "COURT":
            case "CT":
                $("#INCodeDestinataire_" + id).removeAttr("disabled").removeClass("readonly");
                $("#INNomDestinataire_" + id).removeAttr("disabled").removeClass("readonly");
                $("#INCodeDestinataire_" + id).attr("albAutoComplete", "autoCompCodeCourtier");
                $("#INNomDestinataire_" + id).attr("albAutoComplete", "autoCompNomCourtier");
                $("#INCodeInterlocuteur_" + id).attr("albAutoComplete", "autoCompCodeInterlocuteur");
                $("#INNomInterlocuteur_" + id).attr("albAutoComplete", "autoCompNomInterlocuteur");
                MapCommonAutoCompCourtier();
                break;
            case "CI":
                $("#INCodeDestinataire_" + id).removeAttr("disabled").removeClass("readonly");
                $("#INNomDestinataire_" + id).removeAttr("disabled").removeClass("readonly");
                $("#INCodeDestinataire_" + id).attr("albAutoComplete", "autoCompAperiteurCodeNum");
                $("#INNomDestinataire_" + id).attr("albAutoComplete", "autoCompAperiteurNom");
                $("#INCodeInterlocuteur_" + id).removeAttr("albAutoComplete");
                $("#INNomInterlocuteur_" + id).removeAttr("albAutoComplete");
                MapCommonAutoCompAperiteur();
                break;
            case "IN":
                $("#INCodeDestinataire_" + id).attr("albAutoComplete", "autoCompIntervenantCode");
                $("#INNomDestinataire_" + id).attr("albAutoComplete", "autoCompIntervenant");
                $("#INTypeIntervenant_" + id).attr("albAutoComplete", "autoCompIntervenantType");
                $("#INCodeDestinataire_" + id).attr("disabled", "disabled").addClass("readonly");
                $("#INNomDestinataire_" + id).attr("disabled", "disabled").addClass("readonly");
                $("#INCodeInterlocuteur_" + id).attr("albAutoComplete", "autoCompInterlocuteurIntervenantCode");
                $("#INNomInterlocuteur_" + id).attr("albAutoComplete", "autoCompInterlocuteurIntervenant");
                MapCommonAutoCompInterlocuteursIntervenant();
                break;
        }
    }
}

function OpenSelectDestinataire(idDestinataire, typeDestinataire, typeIntervenant) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeDocument = $("#IdDocumentEdition").val();
    var codeDestinataireCourant = $("#INCodeDestinataire_" + idDestinataire).val();
    if (typeDestinataire !== "IN") {
        if (typeIntervenant == undefined)
            typeIntervenant = "";
        $.ajax({
            type: "POST",
            url: "/DocumentGestion/GetSelectionDestinataire",
            data: { typeDestinataire: typeDestinataire, idDestinataire: idDestinataire, codeOffreContrat: codeOffre, type: type, version: version, codeDocument: codeDocument, typeIntervenant: typeIntervenant, codeDestinataireCourant: codeDestinataireCourant },
            success: function (data) {
                $("#divDataSelectionGeneric").html(data);
                MapElementsSelectionDestinataire();
                $("#divSelectionGeneric").show();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    else {//Chargement de la sélection du type d'intervenant
        var nomIntervenantCourant = $("#INNomDestinataire_" + idDestinataire).val();
        var typeIntervenantCourant = $("#INTypeIntervenant_" + idDestinataire).val();
        $.ajax({
            type: "POST",
            url: "/DocumentGestion/GetSelectionTypesIntervenant",
            data: { codeDossier: codeOffre, versionDossier: version, typeDossier: type, idDestinataire: idDestinataire, codeIntervenantCourant: codeDestinataireCourant, nomIntervenantCourant: nomIntervenantCourant, typeIntervenantSelectionne: typeIntervenantCourant },
            success: function (data) {
                $("#divDataSelectionGeneric").html(data);
                MapElementsSelectionTypeIntervenant();
                MapCommonAutoCompIntervenants();
                $("#divSelectionGeneric").show();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//Ouvre la div de selection d'un courrier/lettre type
function OpenSelectCourrierType() {
    $(".requiredField").removeClass("requiredField");
    var valeurCourante = $("#selectedCourrierType").val();
    if (valeurCourante == undefined)
        valeurCourante = $("#INCodeCourrierType").val();
    var filtre = $("#INFiltreCourrierType").val();
    if (filtre == undefined)
        filtre = "";
    var typeDoc = $("#DDLDocument").val();
    if (typeDoc == undefined || typeDoc === "") {
        $("#DDLDocument").addClass("requiredField");
        return;
    }
    $.ajax({
        type: "POST",
        url: "/DocumentGestion/GetSelectionCourrierType",
        data: { valeurCourante: valeurCourante, filtre: filtre, typeDoc: typeDoc },
        success: function (data) {
            $("#divDataSelectionGeneric").html(data);
            MapElementsSelectionCourrier();
            $("#divSelectionGeneric").show();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//Map les éléments de l'écran de sélection courrier
function MapElementsSelectionCourrier() {
    $("input[type=radio][name=radioSelectCourrier]").each(function () {
        $(this).click(function () {
            var id = $(this).attr("id");
            if (id != undefined && id !== "") {
                id = id.split("_")[1];
                $("#selectedCourrierType").val(id);
            }
        });
    });

    $("#btnFiltrerCourrierType").die().live("click", function () {
        OpenSelectCourrierType();
    });

    $("#btnValiderSelectionCourrier").die().live("click", function () {
        if ($("#selectedCourrierType").val() !== "") {
            var code = $("#INCourriersTypeCode_" + $("#selectedCourrierType").val()).val();
            var lib = $("#INCourriersTypeLib_" + $("#selectedCourrierType").val()).val();
            $("#INCodeCourrierType").val($("#selectedCourrierType").val());
            $("#INCourrierType").val(code + " - " + lib);
            $("#divDataSelectionGeneric").html("");
            $("#divSelectionGeneric").hide();
        }
    });

    $("#btnAnnulerSelectionCourrier").die().live("click", function () {
        $("#divSelectionGeneric").hide();
        $("#divDataSelectionGeneric").html("");
    });

    AlternanceLigne("CourrierTypeBody", "", false, null);

}

function MapElementsSelectionDestinataire() {
    var selDestinatare = $("input[type='radio'][name='radioSelectDestinataire']:checked").attr("id");
    if (selDestinatare != undefined && selDestinatare !== "") {
        $("#selectedDestinataire").val(selDestinatare.split("_")[1]);
    }

    $("input[type=radio][name=radioSelectDestinataire]").each(function () {
        $(this).click(function () {
            var id = $(this).attr("id");
            if (id != undefined && id !== "") {
                id = id.split("_")[1];
                $("#selectedDestinataire").val(id);
            }
        });
    });

    $("#btnValiderSelectionDestinataire").die().live("click", function () {
        if ($("#selectedDestinataire").val() !== "") {
            var code = $("#INDestCode_" + $("#selectedDestinataire").val()).val();
            var lib = $("#INDestLibelle_" + $("#selectedDestinataire").val()).val();
            var typeIntervenant = $("#selectionTypeIntervenant").val();
            $("#INCodeDestinataire_" + $("#guidIdDestinataire").val()).val(code);
            $("#INNomDestinataire_" + $("#guidIdDestinataire").val()).val(lib);
            $("#INTypeIntervenant_" + $("#guidIdDestinataire").val()).val(typeIntervenant);
            $("#divSelectionGeneric").hide();
        }
    });

    $("#btnAnnulerSelectionDestinataire").die().live("click", function () {
        $("#divSelectionGeneric").hide();
    });

    AlternanceLigne("DestinataireSelectionBody", "", false, null);
}

function MapElementsSelectionTypeIntervenant() {
    $("#modeRechInterAffaire").die().live("click", function () {
        $("#TypeIntervenant").attr("disabled", "disabled");
        $("input[albAutoComplete=autoCompIntervenantIsFromAffaire]").val("1");
        $("#INCodeIntervenant").val("");
        $("#INNomIntervenant").val("");
    });

    $("#modeRechInterAutre").die().live("click", function () {
        $("#TypeIntervenant").removeAttr("disabled");
        $("input[albAutoComplete=autoCompIntervenantIsFromAffaire]").val("");
        $("#INCodeIntervenant").val("");
        $("#INNomIntervenant").val("");
    });

    $("#TypeIntervenant").die().live("change", function () {
        $("#INCodeIntervenant").val("");
        $("#INNomIntervenant").val("");
    });
    //$('input[type=radio][name=radioSelectTypeIntervenant]').each(function () {
    //    $(this).click(function () {
    //        var id = $(this).attr("id");
    //        if (id != undefined && id != "") {
    //            id = id.split('_')[1];
    //            $("#selectedTypeIntervenantType").val(id);
    //        }
    //    });
    //});

    $("#btnValiderSelectionTypeIntervenant").die().live("click", function () {
        if ($("#INCodeIntervenant").val() == undefined || $("#INCodeIntervenant").val() === "") {
            $("#INCodeIntervenant").addClass("requiredField");
            $("#INNomIntervenant").addClass("requiredField");
            return;
        }
        if ($("#selectedTypeIntervenantType").val() !== "") {
            $("#divSelectionGeneric").hide();
            if ($("#selectedTypeIntervenantType").val() !== "") {
                var idDestinataire = $("#idDestinataireSelectionne").val();
                //OpenSelectDestinataire(idDestinataire, "Intervenant", $("#selectedTypeIntervenantType").val());
                if (idDestinataire != undefined && idDestinataire !== "") {
                    $("#INCodeDestinataire_" + idDestinataire).val($("#INCodeIntervenant").val());
                    $("#INNomDestinataire_" + idDestinataire).val($("#INNomIntervenant").val());
                    $("#INNomDestinataire_" + idDestinataire).attr("title", $("#INNomIntervenant").val());
                    $("#INTypeIntervenant_" + idDestinataire).val($("#TypeIntervenant").val());
                    MapCommonAutoCompInterlocuteursIntervenant();
                }
            }
        }
    });

    $("#btnAnnulerSelectionTypeIntervenant").die().live("click", function () {
        $("#divSelectionGeneric").hide();
    });

    AlternanceLigne("TypeIntervenantTypeBody", "", false, null);
}

//Sauvegarde la ligne destinataire en paramètre
function SaveDestinataireLigne(guidIdLigne) {

    var code = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    //Récupération de la valeur des champs 
    var codeDocument = $("#IdDocumentEdition").val();
    var isPrincipalChecked = $("#chkPrincipal_" + guidIdLigne).is(":checked");
    var typeDestinataire = $("#DDLTypeDestinataire_" + guidIdLigne).val();
    var typeIntervenant = $("#INTypeIntervenant_" + guidIdLigne).val();
    var codeDestinataire = $("#INCodeDestinataire_" + guidIdLigne).val();
    var codeInterlocuteur = $("#INCodeInterlocuteur_" + guidIdLigne).val();
    var typeEnvoi = $("#DDLTypeEnvoi_" + guidIdLigne).val();
    var nbEx = $("#INNbEx_" + guidIdLigne).val();
    var tampon = $("#DDLTampon_" + guidIdLigne).val();
    var lettreAccompagnement = $("#DDLLettreAccompagnement_" + guidIdLigne).val();
    var lotEmail = $("#INLotEmail_" + guidIdLigne).val();
    var isGenereChecked = $("#ChkGenere_" + guidIdLigne).is(":checked");
    var lotId = $("#LotId").val();

    var typeDoc = $("#DDLDocument").val();
    var courrierType = $("#INCodeCourrierType").val();

    //Vérification des données
    var isCheck = true;
    $(".requiredField").removeClass("requiredField");
    if (typeDestinataire == undefined || typeDestinataire === "") {
        $("#DDLTypeDestinataire_" + guidIdLigne).addClass("requiredField");
        isCheck = false;
    }
    if (codeDestinataire == undefined || codeDestinataire === "") {
        $("#INCodeDestinataire_" + guidIdLigne).addClass("requiredField");
        isCheck = false;
    }
    if (codeInterlocuteur == undefined || codeInterlocuteur === "") {
        codeInterlocuteur = 0;
    }
    if (typeEnvoi == undefined || typeEnvoi === "") {
        $("#DDLTypeEnvoi_" + guidIdLigne).addClass("requiredField");
        isCheck = false;
    }
    if (nbEx == undefined || nbEx === "") {
        $("#INNbEx_" + guidIdLigne).addClass("requiredField");
        isCheck = false;
    }
    if (tampon == undefined || tampon === "") {
        $("#DDLTampon_" + guidIdLigne).addClass("requiredField");
        isCheck = false;
    }
    if (lettreAccompagnement == undefined || lettreAccompagnement === "") {
        lettreAccompagnement = 0;
    }
    if (lotEmail == undefined || lotEmail === "") {
        lotEmail = 0;
    }

    if (typeDoc == undefined || typeDoc === "") {
        $("#DDLDocument").addClass("requiredField");
        isCheck = false;
    }
    if (courrierType == undefined || courrierType === "" || courrierType === "0") {
        $("#INCourrierType").addClass("requiredField");
        isCheck = false;
    }


    if (isCheck === true) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/DocumentGestion/SaveLigneDestinataireDetails",
            data: {
                codeOffre: code, version: version, type: type, codeAvt: $("#NumAvenantPage").val(), modeNavig: $("#ModeNavig").val(), isReadOnly: $("#OffreReadOnly").val(),
                codeDocument: codeDocument, guidIdLigne: guidIdLigne, isPrincipalChecked: isPrincipalChecked, typeDestinataire: typeDestinataire, typeIntervenant: typeIntervenant,
                codeDestinataire: codeDestinataire, codeInterlocuteur: codeInterlocuteur, typeEnvoi: typeEnvoi, nbEx: nbEx, tampon: tampon,
                lettreAccompagnement: lettreAccompagnement, lotEmail: lotEmail, isGenereChecked: isGenereChecked, lotId: lotId, typeDoc: typeDoc, courrierType: courrierType,
                addParamValue: $("#AddParamValue").val(), acteGestion: $("#ActeGestion").val()
            },
            success: function (data) {

                $("#divBodyDocument").html(data);
                MapElementPageGestionDoc();
                $("#divModifierLot").hide();
                CloseLoading();

            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }

}

//efface la ligne destinataire en parametre
function DeleteDestinataireLigne(guidIdLigne) {
    var codeDocument = $("#IdDocumentEdition").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/DocumentGestion/DeleteLigneDestinataireDetails",
        data: {
            codeDocument: codeDocument, guidIdLigne: guidIdLigne
        },
        success: function (data) {
            $("#divDestinatairesBody").html(data);
            MapElementsTableauDetails();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//Sauvegarde les informations générales (entête) du détails document (lot)
function SaveInfoComplementairesDetailsDocument() {
    var codeDocument = $("#IdDocumentEdition").val();
    //récupération des données
    var document = $("#DDLDocument").val();
    var courrierType = $("#INCodeCourrierType").val();
    var nbExSupp = $("#INExemplaireSupp").val();
    var code = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();

    var isCheck = true;
    //Vérif des données
    $(".requiredField").removeClass("requiredField");
    if (document == undefined || document === "") {
        $("#DDLDocument").addClass("requiredField");
        isCheck = false;
    }
    if (courrierType == undefined || courrierType === "") {
        $("#INCourrierType").addClass("requiredField");
        isCheck = false;
        courrierType = 0;
    }
    if (nbExSupp == undefined || nbExSupp === "") {
        nbExSupp = 0;
        //$("#INExemplaireSupp").addClass("requiredField");
        //isCheck = false;
    }

    if (isCheck) {
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/DocumentGestion/SaveInfoComplementairesDetailsDocument",
            data: {
                codeOffre: code, type: type, version: version, codeAvt: $("#NumAvenantPage").val(),
                modeNavig: $("#ModeNavig").val(), isReadOnly: $("#OffreReadOnly").val(),
                codeDocument: codeDocument, document: document, courrierType: courrierType, nbExSupp: nbExSupp,
                addParamValue: $("#AddParamValue").val()
            },
            success: function (data) {
                $("#divBodyDocument").html(data);
                MapElementPageGestionDoc();
                $("#divModifierLot").hide();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//Supprime le lot en cours de modification
function DeleteLotDocumentGestion() {
    var codeLot = $("#IdDocumentEdition").val();
    var code = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/DocumentGestion/DeleteLotDocumentsGestion",
        data: {
            codeOffre: code, type: type, version: version, codeAvt: $("#NumAvenantPage").val(),
            modeNavig: $("#ModeNavig").val(), isReadOnly: $("#OffreReadOnly").val(), codeLot: codeLot,
            addParamValue: $("#AddParamValue").val()
        },
        success: function (data) {
            $("#divBodyDocument").html(data);
            MapElementPageGestionDoc();
            $("#divModifierLot").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//actualise l'écran principal
function RefreshListeDocument(wOpenParam) {
    var code = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    ShowLoading();
    return $.ajax({
        type: "POST",
        url: "/DocumentGestion/RefreshListeDocument",
        data: {
            codeOffre: code, type: type, version: version, codeAvt: $("#NumAvenantPage").val(),
            modeNavig: $("#ModeNavig").val(), isReadOnly: $("#OffreReadOnly").val(),
            addParamType: $("#AddParamType").val(), addParamValue: $("#AddParamValue").val(),
            idDocClicked: wOpenParam
        }
    }).then(function (data) {
        $("#divBodyDocument").html(data);
        MapElementPageGestionDoc();
        CloseLoading();
    }).fail(function (request) {
        common.error.showXhr(request);
    });
}

function RefreshListDocumentLibre(wOpenParam) {
    var code = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();

    ShowLoading();
    return $.ajax({
        type: "POST",
        url: "/DocumentGestion/RefreshListeDocumentLibre",
        data: {
            codeOffre: code,
            version: version,
            type: type,
            codeAvt: $("#NumAvenantPage").val(),
            modeNavig: $("#ModeNavig").val(),
            isReadOnly: $("#OffreReadOnly").val(),
            addParamType: $("#AddParamType").val(),
            addParamValue: $("#AddParamValue").val(),
            idDocClicked: wOpenParam
        }
    }).then(function(data) {
        $("#dvLstDocLibre").html(data);
        MapElementPageGestionDoc();
    }).fail(function(request) {
        common.error.showXhr(request, true);
        
    });
}


function ShowErrorDocGeneration() {
    $(document).ready(function () {
        ShowCommonFancy("Error", "ERRORGENDOC", "Une erreur est survenue lors de la génération des documents, contactez votre administrateur.", 300, 80, true, true);
        $('#btnErrorOk').live('click', function () {
            switch ($("#hiddenAction").val()) {
                case "ERRORGENDOC":
                    $('#btnAnnuler').trigger('click');
                    break;
            }
        });
    });
}

//#####################################
//#####################################
//#endregion
