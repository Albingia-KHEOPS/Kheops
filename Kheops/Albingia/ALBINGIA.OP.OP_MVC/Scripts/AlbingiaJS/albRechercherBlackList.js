// Inspiré de albRechercheSaisie.js

$(document).ready(function () {
    if ($("#PreneurAssurance_Numero") !== undefined) {
        $("#PreneurAssurance_Numero").die().live("change", function () {
            displayLinkBlackListed(this.value);
        });
    }

    if ($("#PreneurAssurance_CodePreneurAssurance") !== undefined) {
        $("#PreneurAssurance_CodePreneurAssurance").die().live("change", function () {
            displayLinkBlackListed(this.value);
        });
    }

    $("#lnkBlackList").die().live("click", function () {
        rechercheBlacklistedOffres();
        //var preneurAssuranceCode = $('#PreneurAssurance_Numero').val();

        //if (preneurAssuranceCode === undefined || preneurAssuranceCode === "" || preneurAssuranceCode === null) {
        //    preneurAssuranceCode = $("#PreneurAssurance_CodePreneurAssurance").val();
        //}

        //var url = "/RechercheSaisie/BlackListedRecherche?idPreneurAssurance=" + preneurAssuranceCode;
        //window.open(url, '_blank');
    });
    
});

function displayLinkBlackListed(valueTextPartenaire) {
    if (valueTextPartenaire && valueTextPartenaire === $("#idBlacklistedPartenaire").val()) {
        $("#lnkBlackList").show();
    }
    else {
        $("#lnkBlackList").hide();
    }
}


function rechercheBlacklistedOffres() {
    $(".requiredField").removeClass("requiredField");
    

    var pageNumberRecherche = $("#PaginationPageActuelle").html();
    if (pageNumberRecherche == null || undefined) {
        pageNumberRecherche = 1;
    }

    $("#RechercherResultContainerDiv").addClass("notDisplayed");
    $("#divRechercherResult").hide();
    

    var codeOffreContrat = "";//$('#OffreId').val().toUpperCase();
    var numAliment = "";//$("#NumAliment").val();

    var CheckOffre = true; // $('#CheckOffre').is(':checked');
    var CheckContrat = true; // $('#CheckContrat').is(':checked');
    var CheckAliment = false; // $('#CheckAliment').is(':checked');
   

    var cabinetCourtageId = ""; //$('input[id=CabinetCourtageId][albEmplacement=recherche]').val();
    var cabinetCourtageNom = ""; //$('input[id=CabinetCourtageNom][albEmplacement=recherche]').val().toUpperCase();
    var cabinetCourtageIsApporteur = ""; //$('#chkApporteur').is(':checked');
    var cabinetCourtageIsGestionnaire = ""; //$('#chkGestionnaire').is(':checked');

    var preneurAssuranceCode = $('#PreneurAssurance_Numero').val();
    var preneurAssuranceNom = "";

    if (preneurAssuranceCode == undefined || preneurAssuranceCode == "" || preneurAssuranceCode == null) {
        preneurAssuranceCode = $("#PreneurAssurance_CodePreneurAssurance").val();
        preneurAssuranceNom = $("#PreneurAssurance_NomPreneurAssurance").val();
    }
    else {
        //preneurAssuranceNom = $('#PreneurAssurance_Nom').val().toUpperCase();
    }
    var preneurAssuranceCP = ""; //$('#PreneurAssuranceCP').val();
    //mise en commentaire des villes avant correction
    var preneurAssuranceVille = "";
    var preneurAssuranceSIREN = ""; //$('#PreneurAssuranceSIREN').val();
    var preneurAssuranceDEP = ""; //$('#PreneurAssuranceDEP').val();

    var adresseRisqueVoie = ""; //$('#AdresseRisqueVoie').val();
    var adresseRisqueCP = ""; //$('#AdresseRisqueCP').val();
    //mise en commentaire des villes avant correction
    var adresseRisqueVille = "";
    var motsClefs = ""; //$('#MotsClefs').val();
    var souscripteur = ""; //$('input[id=SouscripteurCode][albEmplacement=recherche]').val().toUpperCase();
    var souscripteurNom = ""; //$('input[id=SouscripteurNom][albEmplacement=recherche]').val();
    var gestionnaire = ""; //$('input[id=GestionnaireCode][albEmplacement=recherche]').val().toUpperCase();
    var gestionnaireNom = ""; //$("input[id='GestionnaireNom'][albEmplacement='recherche']").val();
    var dateType = ""; //$('#DateType').val();
    var dateDebut = ""; //$('#DateDebut').val();
    var dateFin = ""; //$('#DateFin').val();
    var branche = ""; //$('#Branche[albemplacement=recherche]').val();
    var cible = ""; //$('#Cible[albemplacement=recherche]').val();

    var etat = "N"; //$('#Etat').val();
    var saufEtat = false; //$('#chkSauf').is(':checked');

    var situation = "A"; //$('#Situation').val();
    var situationActif = false; //$('#chkActif').is(':checked');
    var situationInactif = false; //$('#chkInactif').is(':checked');

    var sorting = ""; //$("#resultSorting").val();
    var etatTemplate = "False"; //$("#searchInTemplate").val();

    var typeContrat = "";
   

    var modeNavig = "S"; //$("#modeHisto").is(":checked") ? $("#modeHisto").val() : $("#modeStandard").val();

    ShowLoading();

    var filter = {
        CodeOffre: codeOffreContrat,
        NumAliment: numAliment,
        CabinetCourtageId: cabinetCourtageId,
        CheckOffre: CheckOffre,
        CheckContrat: CheckContrat,
        CheckAliment: CheckAliment,
        CabinetCourtageNom: cabinetCourtageNom,
        CabinetCourtageIsApporteur: cabinetCourtageIsApporteur,
        CabinetCourtageIsGestionnaire: cabinetCourtageIsGestionnaire,
        PreneurAssuranceCode: preneurAssuranceCode,
        PreneurAssuranceNom: preneurAssuranceNom,
        PreneurAssuranceCP: preneurAssuranceCP,
        PreneurAssuranceDEP: preneurAssuranceDEP,
        PreneurAssuranceVille: preneurAssuranceVille,
        PreneurAssuranceSIREN: preneurAssuranceSIREN,
        AdresseRisqueVoie: adresseRisqueVoie,
        AdresseRisqueCP: adresseRisqueCP,
        AdresseRisqueVille: adresseRisqueVille,
        MotsClefs: motsClefs,
        SouscripteurCode: souscripteur,
        SouscripteurNom: souscripteurNom,
        GestionnaireCode: gestionnaire,
        GestionnaireNom: gestionnaireNom,
        TypeDateRecherche: dateType,
        DateDebut: dateDebut,
        DateFin: dateFin,
        Branche: branche,
        Cible: cible,
        Etat: etat,
        SaufEtat: saufEtat,
        Situation: situation,
        IsActif: situationActif,
        IsInactif: situationInactif,
        IsTemplate: etatTemplate,
        TypeContrat: typeContrat,
        Sorting: sorting,
        PageNumber: pageNumberRecherche,
        CritereParam: "Standard",
        modeNavig: modeNavig,
        AccesRecherche: "BlackListed"
    };

    if ($("#linkAlertesPreneur").length > 0) {
        filter.PreneurSinistres = ($("#linkAlertesPreneur").data("alertes") || "").indexOf("'S'") > -1;
        filter.PreneurImpayes = ($("#linkAlertesPreneur").data("alertes") || "").indexOf("'I'") > -1;
        filter.PreneurRetardsPaiement = ($("#linkAlertesPreneur").data("alertes") || "").indexOf("'RP'") > -1;
    }

    if (window.sessionStorage) {
        window.sessionStorage.setItem('recherche_filter', JSON.stringify(filter));
    }

    $.ajax({
        type: "POST",
        url: "/RechercheSaisie/Recherche",
        data: filter,
        success: function (data) {
            AlbScrollTop();
            DesactivateShortCut();
            $("#divDataRecherche").html(data);

            if ($("#ResultCount").val() == "0")
                $("#divResultatsBody").html("&nbsp&nbsp<b>Aucun résultat à votre recherche</b>");

            AlbScrollTop();
            $("#divRechercherResult").show();
            $("#RechercherResultContainerDiv").removeClass("notDisplayed");
            AddOnClickButton();
            AddOnClickRadio();
            AddOnClickRow();
            AddOnHideBandeau();

            //vérifie le nombre de ligne retournée

            MapSearchElement();

            MapCommonAutoCompActionMenu();
            CheckNbRowResult();
            setSorting(sorting);
            $("#divRecherche").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
    function setSorting(sorting) {
        if (sorting) {
            let sortBase = $("#divRecherche input.field-hide[type='image'][value='" + sorting + "']").parent();
            let sortDefault = sortBase.children("input.field-show[type='image']");
            let sortOther = sortBase.children("input.field-hide[type='image'][value!='" + sorting + "']");
            sortDefault.addClass("field-hide").removeClass("field-show");
            sortOther.addClass("field-show").removeClass("field-hide");
        }
    }
}
