/* hexavia gmap V2.0.0 
 * Description : migration from leaflet to gmaps
 */
/* Declaration & initialization */
var map = null;
var circle = null;
var drawingManager;
var selectedShape;
var selectedFeature;
var selectedMarker = null;
var inFormClose = false;

// markers list
var searchAddressMarkersList = null;
var markerActesList = [];
var markerContractsList = [];
var markerOffersList = [];
var markerSinistresList = [];
var markerInsuredsList = [];
var markerBrokersList = [];
var markerExpertsList = [];
// mixed markers list
var markerExternalContractsList = [];
var markerContractsAndOffersByPartnerList = [];
var markerOffersAndeSinistresByPartnerList = [];
var markerContractsAndSinistresByPartnerList = [];
var markerAllCasesByPartnerList = [];

var paysFr = ["FRANCE", "MARTINIQUE", "GUADELOUPE", "GUYANE FRANCAISE", "LA REUNION", "REUNION", "MAYOTTE", "SAINT-BARTHELEMY", "SAINT-MARTIN", "NOUVELLE-CALEDONIE", "POLYNESIE FRANCAISE", "SAINT-PIERRE-ET-MIQUELON", "WALLIS-ET-FUTUNA", "MONACO"];
var extensions = ["B", "Bis", "T", "Ter", "a", "b", "c", "d", "e", "f"];
var selectedLongitude = $("#Longitude").val() == "" ? 0 : parseFloat($("#Longitude").val().replace(",", "."));
var selectedLatitude = $("#Latitude").val() == "" ? 0 : parseFloat($("#Latitude").val().replace(",", "."));
var selectedDescription = null;
var sidebar = null;
var latFrance = 46.603354;
var lonFrance = 1.8883335;
var defaultLocation = { lat: latFrance, lng: lonFrance };
var defaultZoom = 5;
var defaultMinZoom = 2;
var defaultMaxZoom = 20;
var selectedSearch = 1;
var adresseSearchNumber = 1;
var designationSearchNumber = 2;
var partnerSearchNumber = 3;
var loadContractshNumber = 4;
var urlExterieur = null;
// colors palette
var pastelBlueColor = "#0069D9";
var pastelPinkColor = "#E5429E";
var pastelGreenColor = "#218838";
var pastelPurpleColor = "#8E67FD";
var pastelBrownColor = "#813E00"
var defaultSideBarColor = "#FD7567";
var pastelBlackColor = "#010101";
var pastelOrangeColor = "#FFB347";
var pastelYellowColor = "#FDFD96";
var pastelRedColor = "#FF6961";
var fileUploadExtension = ".xml";
//colors icons
var ltblueIcon = 'Content/Images/GmapsMarkers/ltblue-dot.png';
var greenIcon = 'Content/Images/GmapsMarkers/green-dot.png';
var pinkIcon = 'Content/Images/GmapsMarkers/pink-dot.png';
var orangeIcon = 'Content/Images/GmapsMarkers/orange-dot.png';
var purpleIcon = 'Content/Images/GmapsMarkers/purple-dot.png';
var brownIcon = 'Content/Images/GmapsMarkers/brown-dot.png';
var redIcon = 'Content/Images/GmapsMarkers/red-dot.png';
var yellowIcon = 'Content/Images/GmapsMarkers/yellow-dot.png';
var blueIcon = 'Content/Images/GmapsMarkers/blue-dot.png';
//specific icons
var contractIcon = 'Content/Images/GmapsMarkers/blue-dot.png';
var offerIcon = 'Content/Images/GmapsMarkers/pink-dot.png';
var sinistreIcon = 'Content/Images/GmapsMarkers/yellow-dot.png';
var contractAndOfferIcon = 'Content/Images/GmapsMarkers/c-o.png';
var contractAndsinistreIcon = 'Content/Images/GmapsMarkers/c-s.png';
var offerAndSinistreIcon = 'Content/Images/GmapsMarkers/o-s.png';
var allCasesIcon = 'Content/Images/GmapsMarkers/c-o-s.png';
var componentForm = [
    { name: "street_number", types: ['street_number'] },
    { name: "route", types: ['route', 'natural_feature', 'establishment', "colloquial_area"] },
    { name: "locality", types: ['locality', 'postal_town', 'administrative_area_level_2', 'sublocality', 'sublocality_level_1'] },
    { name: "postal_code", types: ['postal_code', 'postal_code_prefix', 'postal_code_suffix'] },
    { name: "administrative_area_level_1", types: ['administrative_area_level_1'] },
    { name: "country", types: ['country'] }
];
// google map boundaries
var boundaries = "";

// liste pour calque
var legendHtml = "<img src='/Content/Images/atlas_legende.png' style='max-width:250px;' class='atlasLegende'>"
//'<p class="layerlegend" style="margin-left:7px;">Légende caractéristique des calques</p>' +
//'<p class="layerlegend"><span class="layerlegend" style="background-color:#880E4F;"></span> ZUS </p>' +
//'<p class="layerlegend"><span class="layerlegend" style="background-color:#8E67FD;"></span> Tempête </p>' +
//'<p class="layerlegend"><span class="layerlegend" style="background-color:#FF9900;"></span> Incendie </p>' +
//'<p class="layerlegend"><span class="layerlegend" style="background-color:#007BFF;"></span> Inondation </p>';
var caracteristiqueArray = ["ZUS", "Tempête", "Souscription IN - interdit de souscription", "Souscription IN - zone 1", "Souscription IN - zone 2", "Inondation"];
var brancheArray = ["AP - Arts et Précieux", "CO - Construction", "IA - Ind. accident", "IN - Incendie", "LO - Loisir", "MR - Multirisques", "PJ - Protection juridique", "RC - RC", "RS - Risques spéciaux", "RT - Risques techniques", "TR - Transport"];

var dataFeature = {
    type: "FeatureCollection",
    features: []
};
var defaultOverlayOptions = {
    strokeWeight: 2.0,
    fillOpacity: 0.2,
    editable: false,
    draggable: false
};
var editModeOverlayOptions = {
    strokeWeight: 2.0,
    fillColor: 'green',
    fillOpacity: 0.2,
    editable: true,
    draggable: true
};

var beginningAdress = {};


function activateOption(element) {
    element.removeClass("desactivate-option");
}
function desactivateOptions() {
    desactivateOption($('#btnLoadContractsAround'));
    desactivateOption($('#btnLoadOffersAround'));
    desactivateOption($('#btnLoadSinistresAround'));
    desactivateOption($('#btnLoadInsuredsAround'));
    desactivateOption($('#btnLoadBrokersAround'));
    desactivateOption($('#btnLoadExpertsAround'));
    desactivateOption($('#btnUploadExternalContractsAround'));
}
function desactivateOption(element) {
    if (!element.hasClass("desactivate-option")) {
        element.addClass("desactivate-option");
    }
}
function toggleOption(element) {
    if (element.hasClass("desactivate-option")) {
        element.removeClass("desactivate-option");
    } else {
        element.addClass("desactivate-option");
    }
}
// Activate / Desactivat marker by type
function verifyOptionActivation(element, markersList) {
    let isActive = true;
    if (!element.hasClass("desactivate-option")) {
        DeleteMarkers(markersList);
        element.addClass("desactivate-option");
        isActive = false;
    }
    return isActive;
}
function activateSearchByPartnerOption(element) {
    if (element.prop('disabled')) {
        element.prop('disabled', false);
    }
    activateOption(element);
}
function desactivateSearchByPartnerOption(element) {
    if (!element.prop('disabled')) {
        element.prop('disabled', true);
    }
    desactivateOption(element);
}
function resetSearchByPartnerOptions() {
    let contratsOption = $('#btnLoadContractsByPartner');
    let offresOption = $('#btnLoadOffersByPartner');
    let sinistresOption = $('#btnLoadSinistresByPartner');
    desactivateOption(contratsOption);
    desactivateOption(offresOption);
    desactivateOption(sinistresOption);
    contratsOption.prop('disabled', true);
    offresOption.prop('disabled', true);
    sinistresOption.prop('disabled', true);
    markerContractsList = [];
    markerOffersList = [];
    markerSinistresList = [];
}
/* test if markers exist in the map  */
function haveMarkersInMap() {


    if ($.isArray(markerContractsList)) {
        if (markerContractsList.length > 0)
            return true;
    }
    if ($.isArray(markerOffersList)) {
        if (markerOffersList.length > 0)
            return true;
    }
    if ($.isArray(markerSinistresList)) {
        if (markerSinistresList.length > 0)
            return true;
    }
    if ($.isArray(markerInsuredsList)) {
        if (markerInsuredsList.length > 0)
            return true;
    }
    if ($.isArray(markerBrokersList)) {
        if (markerBrokersList.length > 0)
            return true;
    }
    if ($.isArray(markerExpertsList)) {
        if (markerExpertsList.length > 0)
            return true;
    }
    if ($.isArray(markerExternalContractsList)) {
        if (markerExternalContractsList.length > 0)
            return true;
    }
    return false;
}
/* test if btn is active for markers  */
function haveBtnActivated() {


    if (!$("#btnLoadContractsAround").hasClass("desactivate-option")) {
        return true;
    }
    if (!$("#btnLoadOffersAround").hasClass("desactivate-option")) {
        return true;
    }
    if (!$("#btnLoadSinistresAround").hasClass("desactivate-option")) {
        return true;
    }
    if (!$("#btnLoadInsuredsAround").hasClass("desactivate-option")) {
        return true;
    }
    if (!$("#btnLoadBrokersAround").hasClass("desactivate-option")) {
        return true;
    }
    if (!$("#btnLoadExpertsAround").hasClass("desactivate-option")) {
        return true;
    }
    return false;
}

function sinistreView(activate) {
    var code = $('#code');
    var version = $('#version');
    selectEtat = $("select#cbbEtat");
    selectSituation = $("select#cbbSituation");
    var evenement = $('.evenementDiv');
    selectEvenement = $("select#cbbEvenement");

    version.val('');
    code.val('');
    clearStateAndSituationCbb();
    if (activate) {
        version.hide();
        evenement.show();

    } else {
        version.show();
        evenement.hide();
        selectEvenement.multiselect('deselectAll', false);
    }

    $.ajax({
        url: 'OfferContract/GetSituationByType',
        type: "POST",
        data: { type: activate ? 'X' : 'O' },
        success: function (data) {
            var options = "";
            $.each(data, function (index, element) {
                options += "<option value='" + element.Code + "'>" + element.Libelle + "</option>";
            })
            selectSituation.html(options);
            selectSituation.multiselect('rebuild');
        },
        error: function (err) {
            selectSituation.html("");
            selectSituation.multiselect('rebuild');
        }
    });
    $.ajax({
        url: 'OfferContract/GetEtatByType',
        type: "POST",
        data: { type: activate ? 'X' : 'O' },
        success: function (data) {
            var options = "";
            $.each(data, function (index, element) {
                options += "<option value='" + element.Code + "'>" + element.Libelle + "</option>";
            })
            selectEtat.html(options);
            selectEtat.multiselect('rebuild');
        },
        error: function (err) {
            selectEtat.html("");
            selectEtat.multiselect('rebuild');
        }
    });
}
function clearStateAndSituationCbb() {
    var state = $('#cbbEtat');
    var situation = $('#cbbSituation');
    state.multiselect('deselectAll', false);
    situation.multiselect('deselectAll', false);
    state.multiselect('updateButtonText');
    situation.multiselect('updateButtonText');
}
function resetCbb() {
    clearStateAndSituationCbb();
    var branche = $('#cbbBrancheSearchDesignation');
    var departement = $("#cbbDepartement");
    var garantie = $("#cbbGarantie");
    var evenement = $("#cbbEvenement");
    branche.multiselect('deselectAll', false);
    departement.multiselect('deselectAll', false);
    garantie.multiselect('deselectAll', false);
    evenement.multiselect('deselectAll', false);
    branche.multiselect('updateButtonText');
    departement.multiselect('updateButtonText');
    garantie.multiselect('updateButtonText');
    evenement.multiselect('updateButtonText');
}
function initPartnerSearch(partner) {

    let disabledSearch = true;
    if (partner == null) {
        partner = '';
    } else {
        disabledSearch = false;
    }
    $("#nomPartenaire").val(partner.name);
    $("#nomPartenaire").data("value", partner.name);
    $("#codePartenaire").val(partner.code);
    $("#oriasPartenaire").val(partner.orias);
    $("#interlocuteurPartenaire").val("");
    $("#cpPartenaire").val(partner.postalCode);
    $("#villePartenaire").val(partner.city);
    $("#btnRechercherPartenaires").prop('disabled', disabledSearch);

    initPartnerSearchMap();
}
function initInterlocuteurSearch(interlocuteur) {

    let disabledSearch = $("#btnRechercherPartenaires").prop('disabled');
    if (interlocuteur == null) {
        interlocuteur = '';
    } else {
        disabledSearch = false;
        $("#nomPartenaire").val(interlocuteur.nameCourtier);
        $("#nomPartenaire").data("value", interlocuteur.nameCourtier);
        $("#codePartenaire").val(interlocuteur.codeCourtier);
        $("#oriasPartenaire").val(interlocuteur.orias);
        $("#cpPartenaire").val(interlocuteur.postalCode);
        $("#villePartenaire").val(interlocuteur.city);
    }
    $("#interlocuteurPartenaire").val(interlocuteur.name);
    $("#btnRechercherPartenaires").prop('disabled', disabledSearch);

    initPartnerSearchMap();
}
function initPartnerSearchMap() {
    let contratsOption = $('#btnLoadContractsByPartner');
    let offresOption = $('#btnLoadOffersByPartner');
    let sinistresOption = $('#btnLoadSinistresByPartner');
    desactivateSearchByPartnerOption(contratsOption);
    desactivateSearchByPartnerOption(offresOption);
    desactivateSearchByPartnerOption(sinistresOption);


    DeleteMarkers(markerContractsList);
    DeleteMarkers(markerOffersList);
    DeleteMarkers(markerSinistresList);
    DeleteMarkers(markerContractsAndOffersByPartnerList);
    DeleteMarkers(markerOffersAndeSinistresByPartnerList);
    DeleteMarkers(markerContractsAndSinistresByPartnerList);
    DeleteMarkers(markerAllCasesByPartnerList);
}



// document ready event
$(document).ready(function () {
    $('#btnLoadContractsAround').prop('disabled', true);
    $('#btnLoadOffersAround').prop('disabled', true);
    $('#btnLoadSinistresAround').prop('disabled', true);
    $('#idEchelle').prop('disabled', true);
    $('#btnLoadInsuredsAround').prop('disabled', true);
    $('#btnLoadBrokersAround').prop('disabled', true);
    $('#btnLoadExpertsAround').prop('disabled', true);
    $('#externalContractsAroundUploadFile').prop('disabled', true);
    $('#btnUploadExternalContractsAround').prop('disabled', true);
    $('#btnCleanMarkers').prop('disabled', true);
    $('.messageOptionDesignation').hide();

    beginningAdress = getCurrentAdresse();

    $('.selectpicker').multiselect({
        buttonWidth: "100%",
        buttonText: function (options, select) {
            if (options.length === 0) {
                return select.attr("title");
            }
            else {
                var labels = [];
                options.each(function () {
                    if ($(this).attr('label') !== undefined) {
                        labels.push($(this).attr('label'));
                    }
                    else {
                        labels.push($(this).html());
                    }
                });
                return labels.join(', ') + '';
            }
        },
        maxHeight: 400,
        templates: {
            button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown" style="text-align:left;border:1px lightgrey solid;"><span class="multiselect-selected-text"></span> <b></b></button>',
            ul: '<ul class="multiselect-container dropdown-menu" style="width:100%;"></ul>',
            li: '<li><a tabindex="0"><label style="font-size:1.2em;"></label></a></li>',
        }
    });

    var adresseComplete = document.getElementById('AdresseComplete').value;
    if (adresseComplete === 'France') {
        CreateMap(latFrance, lonFrance, 500);
    }
    else {
        RetrieveLatLonFromAdressAndDisplayAdress(adresseComplete, DisplayAdress);
    }
    //ManageAdresseSearchBox("385px");

    urlExterieur = document.getElementById('UrlExterieur').value;
    var matriculeHexavia = document.getElementById('MatriculeHexavia').value;
    if (IsEmpty(urlExterieur) || matriculeHexavia > 0 || matriculeHexavia == "") {
        $('#street_number').prop('disabled', true);
        $('#route').prop('disabled', true);
        $('#locality').prop('disabled', true);
        $('#postal_code').prop('disabled', true);
        $('#administrative_area_level_1').prop('disabled', true);
        $('#country').prop('disabled', true);
    }
    if (!IsEmpty(urlExterieur)) {
        removeButton("btnDesignationSearch");
        removeButton("btnPartnerSearch");
        if (urlExterieur.indexOf("adelia") > -1 || urlExterieur.indexOf("adeprod") > -1) {
            removeButton("formAnnuler");
            removeButton("formValider");
            removeButton("formSupprimer");
        }
    }
    else {
        $('#street_number2').prop('disabled', true);
        $('#formExtension').prop('disabled', true);
        $('#formBatiment').prop('disabled', true);
        $('#formDistribution').prop('disabled', true);
        $('#formVilleCedex').prop('disabled', true);
        $('#formCPCedex').prop('disabled', true);
        $('#rowOptAdresseManuelle').hide();
        $('#optAdresseManuelle').prop('disabled', true);
        removeButton("formAnnuler");
        removeButton("formValider");
        removeButton("formSupprimer");
    }
    $('#code').keyup(function () {
        validateSearchByDesignation();
    });
    $('#designationSearch').keyup(function () {
        validateSearchByDesignation();
    });

    $('#codePartenaire').change(function () {
        let code = $(this).val();
        let type = 0;
        if (document.getElementById('optCourtiers').checked) {
            type = 1;
        }
        if (code) {

            $.ajax({
                type: 'POST',
                url: 'Partner/GetPartnerByCode',
                data: {
                    code: code,
                    type: type
                },
                dataType: 'json',
                success: function (data) {
                    if (data.partner) {
                        let partner = {
                            code: data.partner.Code,
                            name: data.partner.Name,
                            postalCode: data.partner.Address.PostalCode,
                            city: data.partner.Address.City
                        };
                        initPartnerSearch(partner);
                    } else {
                        initPartnerSearch(null);
                    }
                }
            });

        } else {
            initPartnerSearch(null);
        }
    });

    $('#oriasPartenaire').change(function () {
        let orias = $(this).val();
        if (orias) {

            $.ajax({
                type: 'POST',
                url: 'Partner/GetPartnerByOrias',
                data: {
                    orias: orias
                },
                dataType: 'json',
                success: function (data) {
                    if (data.partner) {
                        let partner = {
                            code: data.partner.Code,
                            name: data.partner.Name,
                            orias: data.partner.Orias,
                            postalCode: data.partner.Address.PostalCode,
                            city: data.partner.Address.City
                        };
                        initPartnerSearch(partner);
                    } else {
                        initPartnerSearch(null);
                    }
                }
            });

        } else {
            initPartnerSearch(null);
        }
    });

    $('input[type=radio][name=optPartners]').on('change', function () {
        initPartnerSearch(null);
    });

    $("input[name='optDesignations']").change(function () {
        var id = $(this)[0].id;
        if (id === "optContrats" || id === "optOffres") {
            if ($("#optContrats").prop('checked') || $("#optOffres").prop('checked')) {
                $("#optSinistres").prop('checked', false);
            }
        }
        else {
            if ($("#optSinistres").prop('checked')) {
                $("#optOffres").prop('checked', false);
                $("#optContrats").prop('checked', false);
            }
        }
        if ($("#optSinistres").prop('checked')) {
            sinistreView(true);
        } else {
            sinistreView(false);
        }
        validateSearchByDesignation();
    });
    function changeEchelle() {
        var adresse = document.querySelector('#formAddressRecherche').value;
        var ville = document.querySelector('#locality').value;
        var codePostal = document.querySelector('#postal_code').value;
        var departement = document.querySelector('#administrative_area_level_1').value;
        var pays = document.querySelector('#country').value;
        var adresseComplete = adresse + " " + codePostal + " " + ville + " " + departement + " " + pays;
        DisplayAdress(selectedLatitude, selectedLongitude, adresseComplete);
        //deleteAllMarkers(false);
        //desactivateOptions();
        hideMessage();
    }
    $('#idEchelle').on('focusin', function () {

        $(this).data('prevData', Math.round(parseFloat($(this).val()) * 10) / 10 || 0.1);
    });
    $('#idEchelle').on('keypress', function (event) {
        if (event.which == 13) {
            $(this).blur();
        }
    });
    $('#idEchelle').on('change', function (event) {
        var echelle = $('#idEchelle');
        var value = Math.round(parseFloat(echelle.val()) * 10) / 10 || 0.1;
        echelle.val(value);
        if (value < 0.1 || value > 25) {
            let settings = {
                type: 'info',
                title: '',
                text: 'La zone de recherche doit être comprise entre 0.1 km et 25 km '
            };
            alertDialog(settings);
            echelle.val(echelle.data("prevData"));
            event.preventDefault();
            return false;
        }
        //if (haveBtnActivated()) {
        //    var settings = {
        //        title: "",
        //        text: "La modification de l'échelle relancera la recherche de marqueurs sur la carte. Voulez-vous continuer ?",
        //        type: "warning",
        //        confirmButtonText: 'Oui',
        //        cancelButtonText: 'Non',
        //        confirmCallback: {
        //            notification: { canShow: false, title: "", text: "" },
        //            actionCallback: function () {
        changeEchelle();
        if (!$("#btnLoadContractsAround").hasClass("desactivate-option")) {
            $("#btnLoadContractsAround").addClass("desactivate-option");
            $("#btnLoadContractsAround").click();
        }
        if (!$("#btnLoadOffersAround").hasClass("desactivate-option")) {
            $("#btnLoadOffersAround").addClass("desactivate-option");
            $("#btnLoadOffersAround").click();
        }
        if (!$("#btnLoadSinistresAround").hasClass("desactivate-option")) {
            $("#btnLoadSinistresAround").addClass("desactivate-option");
            $("#btnLoadSinistresAround").click();
        }
        if (!$("#btnLoadInsuredsAround").hasClass("desactivate-option")) {
            $("#btnLoadInsuredsAround").addClass("desactivate-option");
            $("#btnLoadInsuredsAround").click();
        }
        if (!$("#btnLoadBrokersAround").hasClass("desactivate-option")) {
            $("#btnLoadBrokersAround").addClass("desactivate-option");
            $("#btnLoadBrokersAround").click();
        }
        if (!$("#btnLoadExpertsAround").hasClass("desactivate-option")) {
            $("#btnLoadExpertsAround").addClass("desactivate-option");
            $("#btnLoadExpertsAround").click();
        }
        //            }
        //        },
        //        cancelCallback: {
        //            notification: { canShow: false, title: "", text: "" },
        //            actionCallback: function () {
        //                echelle.val(echelle.data("prevData"));
        //            }
        //        }
        //    };
        //    confirmDialog(settings);
        //} else {
        //    changeEchelle();
        //}
    });

    $("#btnZoneCircle").click(function () {
        icon = $(this).find("span");
        if (icon.hasClass("fa-times-circle")) {
            icon.removeClass("fa-times-circle").addClass("fa-check-circle");
            circle.setMap(null);
            circle = null;
        } else {
            icon.removeClass("fa-check-circle").addClass("fa-times-circle");
            circle = new google.maps.Circle({
                map: map,
                center: { lat: selectedLatitude, lng: selectedLongitude },
                radius: parseFloat($("#idEchelle").val()) * 1000,
                fillColor: '#3388FF',
                fillOpacity: 0.3,
                strokeColor: '#FFFFFF',
                strokeWeight: 3
            });
        }
    });
    

    $('#formAnnuler').on('click', function (event) {
        var sep = document.querySelector('#Separateur').value == "" ? '¤' : document.querySelector('#Separateur').value;
        let adresseValideText = 'NON_VALIDE';
        if (beginningAdress.NomVoie.length > 0) {
            adresseValideText = "VALIDE";
        }

        var latitudeRetour = '';
        if (selectedLatitude !== null && beginningAdress.MatriculeHexavia == 1) {
            latitudeRetour = selectedLatitude;
        }

        var longitudeRetour = '';
        if (selectedLongitude !== null && beginningAdress.MatriculeHexavia == 1) {
            longitudeRetour = selectedLongitude;
        }

        var retourAdresse = beginningAdress.Batiment + sep + beginningAdress.NumeroVoie + sep + beginningAdress.ExtensionVoie + sep
            + beginningAdress.NomVoie + sep + beginningAdress.BoitePostale + sep + beginningAdress.VilleCedex + sep + beginningAdress.CodePostalCedex + sep
            + beginningAdress.Ville + sep + beginningAdress.CodePostal + sep + beginningAdress.Pays.Libelle + sep + adresseValideText + sep
            + beginningAdress.MatriculeHexavia + sep + latitudeRetour + sep + longitudeRetour;

        urlExterieur = document.getElementById('UrlExterieur').value;
        let retourQuery = (urlExterieur.indexOf('?') < 0 ? '?' : '&') + 'Data=adresse=' + retourAdresse;

        if (!IsEmpty(urlExterieur)) {
            if (urlExterieur.indexOf('grp365.albingia.fr') != -1 || urlExterieur.indexOf('grpformation365.albingia.fr') != -1 || urlExterieur.indexOf('grpaudit365.albingia.fr') != -1) {
                window.location = 'https://' + urlExterieur + retourQuery;
            }
            else {
                window.location = 'http://' + urlExterieur + retourQuery;
            }
        }
    });

    $('#formValider').on('click', function (event) {
        if (!$("#optAdresseManuelle").is(":checked")) {
            if ($("#postal_code").val() == "") {
                if ($("#formCPCedex").val() != "") $("#postal_code").val($("#formCPCedex").val());
                else {
                    let settings = {
                        type: 'warning',
                        title: '',
                        text: 'Veuillez renseignez le code postal cedex'
                    };
                    alertDialog(settings);
                    return false;
                }
            } else {
                if ($("#formCPCedex").val() != "") {
                    if ($("#postal_code").val().substr(0, 2) != $("#formCPCedex").val().substr(0, 2)) {
                        let settings = {
                            type: 'warning',
                            title: '',
                            text: 'Les 2 premiers chiffres du code postal cedex doivent être identiques au département'
                        };
                        alertDialog(settings);
                        return false;
                    }
                }
            }
        }
        var sep = document.querySelector('#Separateur').value == "" ? '¤' : document.querySelector('#Separateur').value;
        var currentAdresse = getCurrentAdresse();

        let adresseValideText = 'NON_VALIDE';
        if (currentAdresse.NomVoie.length > 0) {
            adresseValideText = "VALIDE";
        }

        var latitudeRetour = '';
        if (selectedLatitude !== null && currentAdresse.MatriculeHexavia == 1) {
            latitudeRetour = selectedLatitude;
        }

        var longitudeRetour = '';
        if (selectedLongitude !== null && currentAdresse.MatriculeHexavia == 1) {
            longitudeRetour = selectedLongitude;
        }

        var retourAdresse = currentAdresse.Batiment + sep + currentAdresse.NumeroVoie + sep + currentAdresse.ExtensionVoie + sep
            + currentAdresse.NomVoie + sep + currentAdresse.BoitePostale + sep + currentAdresse.VilleCedex + sep + currentAdresse.CodePostalCedex + sep
            + currentAdresse.Ville + sep + currentAdresse.CodePostal + sep + currentAdresse.Pays.Libelle + sep + adresseValideText + sep
            + currentAdresse.MatriculeHexavia + sep + latitudeRetour + sep + longitudeRetour;

        urlExterieur = document.getElementById('UrlExterieur').value;
        let retourQuery = (urlExterieur.indexOf('?') < 0 ? '?' : '&') + 'Data=adresse=' + retourAdresse;

        if (!IsEmpty(urlExterieur)) {
            if (urlExterieur.indexOf('grp365.albingia.fr') != -1 || urlExterieur.indexOf('grpformation365.albingia.fr') != -1 || urlExterieur.indexOf('grpaudit365.albingia.fr') != -1) {
                window.location = 'https://' + urlExterieur + retourQuery;
            }
            else {
                window.location = 'http://' + urlExterieur + retourQuery;
            }
        }

    });

    $('#formSupprimer').on('click', function (event) {
        var sep = document.querySelector('#Separateur').value == "" ? '¤' : document.querySelector('#Separateur').value;
        var retourAdresse = sep + sep + sep
            + sep + sep + sep + sep
            + sep + sep + sep + sep
            + sep + sep;

        urlExterieur = document.getElementById('UrlExterieur').value;
        let retourQuery = (urlExterieur.indexOf('?') < 0 ? '?' : '&') + 'Data=adresse=' + retourAdresse;

        if (!IsEmpty(urlExterieur)) {
            if (urlExterieur.indexOf('grp365.albingia.fr') != -1 || urlExterieur.indexOf('grpformation365.albingia.fr') != -1 || urlExterieur.indexOf('grpaudit365.albingia.fr') != -1) {
                window.location = 'https://' + urlExterieur + retourQuery;
            }
            else {
                window.location = 'http://' + urlExterieur + retourQuery;
            }
        }

    });

    $("#optAdresseManuelle").on("change", function () {
        if ($(this).is(":checked")) {
            if (!isAdresseVide()) {
                $("#formValider").prop("disabled", false);
            } else {
                $("#formValider").prop("disabled", true);
            }
            $('#street_number').prop('disabled', false);
            $('#route').prop('disabled', false);
            $('#locality').prop('disabled', false);
            $('#postal_code').prop('disabled', false);
            $('#administrative_area_level_1').prop('disabled', false);
            $('#country').prop('disabled', false);
        } else {
            var settings = {
                title: "",
                text: "Annuler la saisie manuelle effacera toutes les données fournies. Voulez-vous continuer ?",
                type: "warning",
                confirmButtonText: 'Oui',
                cancelButtonText: 'Non',
                confirmCallback: {
                    notification: { canShow: false, title: "", text: "" },
                    actionCallback: function () {
                        NettoyerFormEtMap();
                        $("#formValider").prop("disabled", true)
                        $('#street_number').prop('disabled', true);
                        $('#route').prop('disabled', true);
                        $('#locality').prop('disabled', true);
                        $('#postal_code').prop('disabled', true);
                        $('#administrative_area_level_1').prop('disabled', true);
                        $('#country').prop('disabled', true);
                    }
                },
                cancelCallback: {
                    notification: { canShow: false, title: "", text: "" },
                    actionCallback: function () {
                        $("#optAdresseManuelle").prop("checked", true);
                    }
                }
            };
            confirmDialog(settings);
        }
    })

    $(".adresseInput").change(function () {
        if (isAdresseVide()) {
            $("#formValider").prop("disabled", true);
        } else {
            $("#formValider").prop("disabled", false);
        }
    });


    $('#btnLoadOffersAround').click(function () {
        hideMessage();
        let $this = $(this);
        if (verifyOptionActivation($this, markerOffersList)) {
            ShowLoading();
            let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
            if ($this.html() !== loadingText) {
                $this.data('original-text', $this.html());
                $this.html(loadingText);
            }

            let diametre = GetDiameter();
            let loadDataResult = loadAffaireNouvelleAround(selectedLongitude, selectedLatitude, diametre, 'O', pinkIcon, $this);

        } else {
            markerOffersList = [];
        }
    });

    $('#btnLoadContractsAround').click(function () {
        hideMessage();
        let $this = $(this);
        if (verifyOptionActivation($this, markerContractsList)) {
            ShowLoading();
            let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
            if ($this.html() !== loadingText) {
                $this.data('original-text', $this.html());
                $this.html(loadingText);
            }

            let diametre = GetDiameter();
            let loadDataResult = loadAffaireNouvelleAround(selectedLongitude, selectedLatitude, diametre, 'P', blueIcon, $this);

        } else {
            markerContractsList = [];
        }

    });

    $('#btnLoadSinistresAround').click(function () {
        hideMessage();
        let $this = $(this);
        if (verifyOptionActivation($this, markerSinistresList)) {
            ShowLoading();
            let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
            if ($this.html() !== loadingText) {
                $this.data('original-text', $this.html());
                $this.html(loadingText);
            }

            let diametre = GetDiameter();
            let loadDataResult = loadAffaireNouvelleAround(selectedLongitude, selectedLatitude, diametre, 'X', orangeIcon, $this);

        } else {
            markerSinistresList = [];
        }
    });

    $('#btnUploadExternalContractsAround').click(function () {
        let $this = $(this);
        ShowLoading();
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        if ($(this).html() !== loadingText) {
            $this.data('original-text', $(this).html());
            $this.html(loadingText);
        }


        var iconMarker = ltblueIcon;


        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {

            var fileUpload = $("#externalContractsAroundUploadFile").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            let diametre = GetDiameter();
            // Adding one more key to FormData object  
            fileData.append('username', 'Manas');

            // Adding longitude, latitude, diametre
            fileData.append('longitude', selectedLongitude);
            fileData.append('latitude', selectedLatitude);
            fileData.append('diametre', diametre);

            $.ajax({
                url: 'OfferContract/LoadActeGestionFromFile',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (data) {
                    if (data.errorMessage !== "") {
                        alert(data.errorMessage);
                    }
                    else {
                        let listAffaires = data.listAffaires;
                        let zoomMax = data.zoomMax;
                        DeleteMarkers(markerExternalContractsList);
                        markerExternalContractsList = [];
                        for (i = 0; i < listAffaires.length; i++) {

                            marker = [
                                listAffaires[i].NumeroChrono,
                                new google.maps.Marker({
                                    position: { lat: listAffaires[i].Lat, lng: listAffaires[i].Lon },
                                    icon: iconMarker,
                                    map: map
                                }),
                                listAffaires[i].Libelle,
                                listAffaires[i].FullLibelle
                            ];
                            markerExternalContractsList.push(marker);
                        }

                        let adresse = document.querySelector('#formAddressRecherche').value;
                        DisplayAdress(selectedLatitude, selectedLongitude, adresse);
                        if (listAffaires.length != 0) {
                            activateOption($('#btnUploadExternalContractsAround'));
                        }
                    }
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
        $this.html($this.data('original-text'));
        CloseLoading();
    });

    $('#btnLoadInsuredsAround').click(function () {
        hideMessage();
        let $this = $(this);
        if (verifyOptionActivation($this, markerInsuredsList)) {
            ShowLoading();
            let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
            if ($this.html() !== loadingText) {
                $this.data('original-text', $this.html());
                $this.html(loadingText);
            }
            let diametre = GetDiameter();
            $.ajax({
                type: 'POST',
                url: 'Partner/LoadInsuredsAroundGPSPoint',
                data: { longitude: selectedLongitude, latitude: selectedLatitude, diametre: diametre },
                dataType: 'json',
                success: function (listInsureds) {
                    DeleteMarkers(markerInsuredsList);
                    markerInsuredsList = [];
                    for (i = 0; i < listInsureds.length; i++) {
                        marker = [
                            listInsureds[i].NumeroChrono,
                            new google.maps.Marker({
                                position: { lat: listInsureds[i].Lat, lng: listInsureds[i].Lon },
                                icon: brownIcon,
                                map: map
                            }),

                            listInsureds[i].Libelle,
                            listInsureds[i].FullLibelle
                        ];
                        markerInsuredsList.push(marker);
                    }

                    let adresse = document.querySelector('#formAddressRecherche').value;
                    DisplayAdress(selectedLatitude, selectedLongitude, adresse);
                    $this.html($this.data('original-text'));
                    activateOption($('#btnLoadInsuredsAround'));
                    CloseLoading();
                },
                error: function () {
                    $this.html($this.data('original-text'));
                    CloseLoading();
                    let settings = {
                        type: 'error',
                        title: '',
                        text: 'Un problème est survenu '
                    };
                    alertDialog(settings);
                }
            });
        } else {
            markerInsuredsList = [];
        }
    });

    $('#btnLoadBrokersAround').click(function () {
        hideMessage();
        let $this = $(this);
        if (verifyOptionActivation($this, markerBrokersList)) {
            ShowLoading();
            let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
            if ($this.html() !== loadingText) {
                $this.data('original-text', $this.html());
                $this.html(loadingText);
            }
            let diametre = GetDiameter();
            $.ajax({
                type: 'POST',
                url: 'Partner/LoadBrokersAroundGPSPoint',
                data: { longitude: selectedLongitude, latitude: selectedLatitude, diametre: diametre },
                dataType: 'json',
                success: function (listBrokers) {
                    DeleteMarkers(markerBrokersList);
                    markerBrokersList = [];
                    for (i = 0; i < listBrokers.length; i++) {
                        marker = [
                            listBrokers[i].NumeroChrono,

                            new google.maps.Marker({
                                position: { lat: listBrokers[i].Lat, lng: listBrokers[i].Lon },
                                icon: greenIcon,
                                map: map
                            }),
                            listBrokers[i].Libelle,
                            listBrokers[i].FullLibelle
                        ];
                        markerBrokersList.push(marker);
                    }

                    let adresse = document.querySelector('#formAddressRecherche').value;
                    DisplayAdress(selectedLatitude, selectedLongitude, adresse);
                    $this.html($this.data('original-text'));
                    activateOption($('#btnLoadBrokersAround'));
                    CloseLoading();
                },
                error: function () {
                    $this.html($this.data('original-text'));
                    CloseLoading();
                    let settings = {
                        type: 'error',
                        title: '',
                        text: 'Un problème est survenu '
                    };
                    alertDialog(settings);
                }
            });
        } else {
            markerBrokersList = [];
        }
    });

    $('#btnLoadExpertsAround').click(function () {
        hideMessage();
        let $this = $(this);
        if (verifyOptionActivation($this, markerExpertsList)) {
            ShowLoading();
            let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
            if ($this.html() !== loadingText) {
                $this.data('original-text', $this.html());
                $this.html(loadingText);
            }
            let diametre = GetDiameter();
            $.ajax({
                type: 'POST',
                url: 'Partner/LoadExpertsAroundGPSPoint',
                data: { longitude: selectedLongitude, latitude: selectedLatitude, diametre: diametre },
                dataType: 'json',
                success: function (listExperts) {
                    DeleteMarkers(markerExpertsList);
                    markerExpertsList = [];
                    for (i = 0; i < listExperts.length; i++) {
                        marker = [
                            listExperts[i].NumeroChrono,

                            new google.maps.Marker({
                                position: { lat: listExperts[i].Lat, lng: listExperts[i].Lon },
                                icon: purpleIcon,
                                map: map
                            }),
                            listExperts[i].Libelle,
                            listExperts[i].FullLibelle
                        ];
                        markerExpertsList.push(marker);
                    }

                    let adresse = document.querySelector('#formAddressRecherche').value;
                    DisplayAdress(selectedLatitude, selectedLongitude, adresse);
                    $this.html($this.data('original-text'));
                    activateOption($('#btnLoadExpertsAround'));
                    CloseLoading();
                },
                error: function () {
                    $this.html($this.data('original-text'));
                    CloseLoading();
                    let settings = {
                        type: 'error',
                        title: '',
                        text: 'Un problème est survenu '
                    };
                    alertDialog(settings);
                }
            });
        } else {
            markerExpertsList = [];
        }
    });

    $('#btnLoadContractsByPartner, #btnLoadOffersByPartner, #btnLoadSinistresByPartner').click(function () {
        hideMessage();
        ShowLoading();
        let $this = $(this);
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        if ($this.html() !== loadingText) {
            $this.data('original-text', $this.html());
            $this.html(loadingText);
        }

        toggleOption($this);
        updateMarkersByPartner();

        $this.html($this.data('original-text'));
        CloseLoading();
    });

    $('#btnSismiciteOverlay, #btnInondationOverlay, #btnSecheresseOverlay, #btnZUSOverlay, #btnTypeOverlay').click(function () {
        let $this = $(this);
        var id = $this.attr('id');
        if ($this.hasClass('desactivate-option')) {
            toggleOverlay(true, id);
        } else {
            toggleOverlay(false, id);
        }
    });

    $('#btnLegend').click(function () {
        $("#legend").slideToggle("fast");
    });

    $('#btnCleanMarkers').click(function () {
        CleanMarkersExceptSelected();
        desactivateOptions();
        hideMessage();
    });

    $("#gestionCalque").change(function () {
        //modifier la légende
        if ($(this).val().length > 0 && $("#legend").children("img.atlasLegende").length == 0) {
            $("#legend").prepend(legendHtml + ($("#legend").children("img").length > 0 ? "<hr>" : ""));
        } else if ($(this).val().length == 0 && $("#legend").children("img.atlasLegende").length > 0) {
            $("#legend img.atlasLegende, #legend hr").remove();
        }

        ShowHideLayer();
    })

    $('#btnSearchByDesignation').click(function () {
        ShowLoading();
        let $this = $(this);
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        if ($(this).html() !== loadingText) {
            $this.data('original-text', $(this).html());
            $this.html(loadingText);
        }

        var type = "";

        if ($('#optSinistres').is(':checked')) {
            type += "X";
        }

        if ($('#optOffres').is(':checked')) {
            type += "O";
        }

        if ($('#optContrats').is(':checked')) {
            type += "P";
        }



        var displayMode = "BrancheDesignation";


        var designation = $('#designationSearch').val();
        var etat = transformSelectionToString($('#cbbEtat').val());
        var situation = transformSelectionToString($('#cbbSituation').val());
        var branche = transformSelectionToString($('#cbbBrancheSearchDesignation').val());
        var departement = transformSelectionToString($('#cbbDepartement').val());
        var garantie = transformSelectionToString($("#cbbGarantie").val());
        var evenement = transformSelectionToString($("#cbbEvenement").val());
        var iconMarker = blueIcon;
        var code = $('#code').val();
        var version = $('#version').val();



        $.ajax({
            type: 'POST',
            url: 'OfferContract/LoadAffairesByDesignation',
            data: { code: code, version: version, designation: designation, typeDesignation: type, etat: etat, situation: situation, branche: branche, garantie: garantie, departement: departement, evenement: evenement, mode: displayMode },
            dataType: 'json',
            success: function (data) {
                if (data.count == 500) $("#messageNbreResultat").show();
                else $("#messageNbreResultat").hide();
                let listAffaires = data.listAffaires;
                let zoomMax = data.zoomMax;
                DeleteMarkers(markerActesList);
                markerActesList = [];
                for (i = 0; i < listAffaires.length; i++) {
                    marker = [
                        listAffaires[i].NumeroChrono,
                        new google.maps.Marker({
                            position: { lat: listAffaires[i].Lat, lng: listAffaires[i].Lon },
                            icon: iconMarker,
                            map: map
                        }),
                        listAffaires[i].Libelle,
                        listAffaires[i].FullLibelle
                    ];
                    markerActesList.push(marker);
                }
                if (listAffaires.length == 0) {
                    let settings = {
                        type: 'info',
                        title: '',
                        text: 'Aucun résultat n\'a été trouvé avec ces critères'
                    };
                    alertDialog(settings);
                }

                DisplayDesignationAdress(zoomMax, defaultSideBarColor);
                $this.html($this.data('original-text'));
                CloseLoading();
            },
            error: function () {
                $this.html($this.data('original-text'));
                CloseLoading();
                let settings = {
                    type: 'error',
                    title: '',
                    text: 'Un problème est survenu '
                };
                alertDialog(settings);
            }
        });
    });


    $('#btnRechercherPartenaires').click(function () {
        ShowLoading();
        let $this = $(this);
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        hideMessage();
        let type = 0;
        let codePartenaire = document.getElementById('codePartenaire').value;
        if ($(this).html() !== loadingText) {
            $this.data('original-text', $(this).html());
            $this.html(loadingText);
        }

        CleanMarkersExceptSelected();
        if (document.getElementById('optCourtiers').checked) {
            type = 1;
            colorPartner = pastelGreenColor;

        }
        initPartnerSearchMap();
        $.ajax({
            type: 'POST',
            url: ' OfferContract/LoadAffairesByPartner',
            data: {
                code: codePartenaire,
                type: type
            },
            dataType: 'json',
            success: function (data) {
                let listAffaires = data.listAffaires;
                let contratsOption = $('#btnLoadContractsByPartner');
                let offresOption = $('#btnLoadOffersByPartner');
                let sinistresOption = $('#btnLoadSinistresByPartner');

                for (i = 0; i < listAffaires.length; i++) {
                    addMarkerByPartnerSearch(listAffaires[i]);
                }
                if (markerContractsList.length > 0) {
                    activateSearchByPartnerOption(contratsOption);
                }
                if (markerOffersList.length > 0) {
                    activateSearchByPartnerOption(offresOption);
                }
                if (markerSinistresList.length > 0) {
                    activateSearchByPartnerOption(sinistresOption);
                }
                if (markerContractsAndOffersByPartnerList.length > 0) {
                    activateSearchByPartnerOption(contratsOption);
                    activateSearchByPartnerOption(offresOption);
                }
                if (markerOffersAndeSinistresByPartnerList.length > 0) {
                    activateSearchByPartnerOption(offresOption);
                    activateSearchByPartnerOption(sinistresOption);
                }
                if (markerContractsAndSinistresByPartnerList.length > 0) {
                    activateSearchByPartnerOption(contratsOption);
                    activateSearchByPartnerOption(sinistresOption);
                }
                if (markerAllCasesByPartnerList.length > 0) {
                    activateSearchByPartnerOption(contratsOption);
                    activateSearchByPartnerOption(offresOption);
                    activateSearchByPartnerOption(sinistresOption);
                }

                if (listAffaires.length == 0) {
                    let settings = {
                        type: 'info',
                        title: '',
                        text: 'Aucun résultat n\'a été trouvé avec ces critères'
                    };
                    alertDialog(settings);
                }

                displaySearchPartnerAdress();

                $this.html($this.data('original-text'));
                CloseLoading();
            },
            error: function () {
                $this.html($this.data('original-text'));
                CloseLoading();
                let settings = {
                    type: 'error',
                    title: '',
                    text: 'Un problème est survenu '
                };
                alertDialog(settings);
            }
        });
    });

    $('#btnNettoyerPartenaire').click(function () {
        NettoyerFormEtMap();
        initPartnerSearch(null);
    });

    $('#btnNettoyerDesignation').click(function () {
        NettoyerFormEtMap();
    });

    $('#btnNettoyerAdresse').on('click', function (event) {
        event.preventDefault();
        NettoyerFormEtMap();
    });

    $('.cbbSearchByDesignation').change(function () {
        validateSearchByDesignation();
    });

    // gestion de la couleur des boutons du menu
    $(".btnMenu").click(function () {

        $("#btnDesignationSearch").removeClass("btn-primary").addClass("btn-light");
        $("#btnPartnerSearch").removeClass("btn-primary").addClass("btn-light");
        $("#btnAdresseSearch").removeClass("btn-primary").addClass("btn-light");
        $(this).removeClass("btn-light").addClass("btn-primary");

        var typeSearch = $(this).data("number");
        switch (typeSearch) {
            case 1:
                ManageAdresseSearchBox();
                break;
            case 2:
                ManageDesignationSearchBox();
                break;
            case 3:
                ManagePartnerSearchBox();
                break;
        }

    });

    $("input[type=file]").change(function () {
        var fieldVal = $(this).val();
        if (fieldVal != undefined || fieldVal != "") {
            $(this).next(".custom-file-label").text(fieldVal);
        }
        validateLoadContracts();
    });


    $("#nomPartenaire").partnercomplete({
        source: function (request, response) {
            $.ajax({
                url: 'Partner/GetPartnersByName',
                type: "POST",
                dataType: "json",
                data: { name: request.term, type: document.getElementById('optCourtiers').checked ? 1 : 0 },
                success: function (data) {
                    response($.map(data.partners, function (partner) {
                        return {
                            label: partner.Name,
                            value: partner.Name,
                            name: partner.Name,
                            secondaryName: partner.SecondaryName,
                            code: partner.Code,
                            orias: partner.Orias,
                            postalCode: partner.Address.PostalCode,
                            city: partner.Address.City,
                        };
                    }));
                }
            });
        },
        position: { my: "left top", at: "left bottom" },
        delay: 500,
        change: function (event, ui) {
            if (!ui.item) {
                if ($("#nomPartenaire").val() != $("#nomPartenaire").data("value"))
                    initPartnerSearch(null);
            }
        },
        select: function (event, ui) {
            if (ui.item) {
                initPartnerSearch(ui.item);
            } else {
                initPartnerSearch(null);
            }
            return false;
        },
        minLength: 3
    });

    $("#interlocuteurPartenaire").interlocuteurcomplete({
        source: function (request, response) {
            $.ajax({
                url: 'Partner/GetInterlocuteursByName',
                type: "POST",
                dataType: "json",
                data: { name: request.term, code: document.getElementById('codePartenaire').value },
                success: function (data) {
                    response($.map(data.interlocuteurs, function (interlocuteur) {
                        return {
                            label: interlocuteur.Name,
                            value: interlocuteur.Name,
                            name: interlocuteur.Name,
                            nameCourtier: interlocuteur.NameCourtier,
                            codeCourtier: interlocuteur.CodeCourtier,
                            orias: interlocuteur.Orias,
                            postalCode: interlocuteur.Address.PostalCode,
                            city: interlocuteur.Address.City,
                        };
                    }));
                }
            });
        },
        position: { my: "left top", at: "left bottom" },
        delay: 500,
        change: function (event, ui) {
            if (!ui.item) {
                initInterlocuteurSearch(null);
            }
        },
        select: function (event, ui) {
            if (ui.item) {
                initInterlocuteurSearch(ui.item);
            } else {

                initInterlocuteurSearch(null);
            }

            return false;
        },

        minLength: 3
    });


});

//Bind keypress event to document
$(document).keypress(function (event) {
    let keycode = (event.keyCode ? event.keyCode : event.which);
    let designationSearch = $("#btnSearchByDesignation");
    let partnerSearch = $("#btnRechercherPartenaires");
    if (keycode == '13') {
        if (selectedSearch == designationSearchNumber) {
            if (!designationSearch.prop('disabled')) {
                designationSearch.trigger('click');
            }
        }
        if (selectedSearch == partnerSearchNumber) {
            if (!partnerSearch.prop('disabled')) {
                partnerSearch.trigger('click');
            }
        }
    }
});
function ManageAdresseSearchBox() {
    if (selectedSearch != adresseSearchNumber) {
        NettoyerFormEtMap();
        selectedSearch = adresseSearchNumber;
        $("#adresseSearchBox").slideDown("fast");
        $("#designationSearchBox").slideUp("fast");
        $("#partnerSearchBox").slideUp("fast");
        changeHeight("385px");
    }
    else {
        var height = $("#adresseSearchBox").is(":visible") ? "630px" : "385px";
        $("#adresseSearchBox").slideToggle("fast");
        changeHeight(height);
    }
    document.getElementById("divOptions").style.display = "block";
    document.getElementById("divOptionsDesignationSearch").style.display = "none";
    document.getElementById("divOptionsPartnersSearch").style.display = "none";
}

function ManageDesignationSearchBox() {
    if (selectedSearch != designationSearchNumber) {
        clearSelectedMarker();
        NettoyerFormEtMap();
        selectedSearch = designationSearchNumber;
        $("#adresseSearchBox").slideUp("fast");
        $("#designationSearchBox").slideDown("fast");
        $("#partnerSearchBox").slideUp("fast");
        changeHeight("385px");
    }
    else {
        var height = $("#designationSearchBox").is(":visible") ? "630px" : "385px";
        $("#designationSearchBox").slideToggle("fast");
        changeHeight(height);
    }
    document.getElementById("divOptions").style.display = "none";
    document.getElementById("divOptionsDesignationSearch").style.display = "block";
    document.getElementById("divOptionsPartnersSearch").style.display = "none";
}

function ManagePartnerSearchBox() {
    if (selectedSearch != partnerSearchNumber) {
        clearSelectedMarker();
        NettoyerFormEtMap();
        selectedSearch = partnerSearchNumber;
        $("#adresseSearchBox").slideUp("fast");
        $("#designationSearchBox").slideUp("fast");
        $("#partnerSearchBox").slideDown("fast");
        changeHeight("450px");
    }
    else {
        var height = $("#partnerSearchBox").is(":visible") ? "630px" : "450px";
        $("#partnerSearchBox").slideToggle("fast");
        changeHeight(height);
    }
    document.getElementById("divOptions").style.display = "none";
    document.getElementById("divOptionsDesignationSearch").style.display = "none";
    document.getElementById("divOptionsPartnersSearch").style.display = "block";
}

function ManageLoadContractsBox(height) {
    if (selectedSearch !== loadContractshNumber) {
        NettoyerFormEtMap();
        selectedSearch = loadContractshNumber;
    }
    changeHeight(height);
    document.getElementById("divOptions").style.display = "none";
    document.getElementById("divOptionsPartnersSearch").style.display = "block";
    $("#btnDesignationSearch").removeClass("btn-primary").addClass("btn-light");
    $("#btnPartnerSearch").removeClass("btn-primary").addClass("btn-light");
    $("#btnAdresseSearch").removeClass("btn-primary").addClass("btn-light");
}




function RetrieveLatLonFromAdressAndDisplayAdress(adresseComplete, DisplayAdress) {
    var adresseCompleteSansAccent = skipAccent(adresseComplete);

    if (selectedLatitude == 0 && selectedLongitude == 0) {
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': adresseCompleteSansAccent }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                selectedLongitude = results[0].geometry.location.lng();
                $("#Longitude").val(selectedLongitude);
                selectedLatitude = results[0].geometry.location.lat();
                $("#Latitude").val(selectedLatitude);
                selectedDescription = adresseComplete;
                DisplayAdress(selectedLatitude, selectedLongitude, adresseComplete);
                $("#formValider").prop("disabled", false);
            } else {
                CreateMap(latFrance, lonFrance, 500);
            }
        });
    } else {
        selectedDescription = adresseComplete;
        DisplayAdress(selectedLatitude, selectedLongitude, adresseComplete);
        $("#formValider").prop("disabled", false);
    }
}

function skipAccent(adresse) {
    var r = adresse.toLowerCase();
    r = r.replace(new RegExp(/[àáâãäå]/g), "a");
    r = r.replace(new RegExp(/æ/g), "ae");
    r = r.replace(new RegExp(/ç/g), "c");
    r = r.replace(new RegExp(/[èéêë]/g), "e");
    r = r.replace(new RegExp(/[ìíîï]/g), "i");
    r = r.replace(new RegExp(/ñ/g), "n");
    r = r.replace(new RegExp(/[òóôõö]/g), "o");
    r = r.replace(new RegExp(/œ/g), "oe");
    r = r.replace(new RegExp(/[ùúûü]/g), "u");
    r = r.replace(new RegExp(/[ýÿ]/g), "y");
    return r;
}

function changeHeight(height) {
    document.getElementById('map').style.height = height;
}

function IsEmpty(str) {
    return (!str || 0 === str.trim().length);
}
function bindInfoWindowToPosition(marker, content) {
    var infowindow = new google.maps.InfoWindow();
    google.maps.event.addListener(marker, 'mouseover', (function (marker, content, infowindow) {
        return function () {
            infowindow.setContent(content);
            infowindow.open(map, marker);
        };
    })(marker, content, infowindow));

    google.maps.event.addListener(marker, 'mouseout', (function (infowindow) {
        return function () {
            infowindow.close();
        };
    })(infowindow));
}
function bindInfoWindow(marker, content) {
    var infowindow = new google.maps.InfoWindow();
    google.maps.event.addListener(marker, 'mouseover', (function (marker, content, infowindow) {
        return function () {
            infowindow.setContent(content);
            //infowindow.setOptions({
            //    disableAutoPan: true
            //});
            infowindow.open(map, marker);
        };
    })(marker, content, infowindow));

    google.maps.event.addListener(marker, 'mouseout', (function (infowindow) {
        return function () {
            infowindow.close();
        };
    })(infowindow));
}

function bindInfoWindowToFeature() {
    var infowindow = new google.maps.InfoWindow();
    map.data.addListener('mouseover', function (event) {
        var date = event.feature.getProperty('date').substr(6, 2) + "-" +
            event.feature.getProperty('date').substr(4, 2) + "-" +
            event.feature.getProperty('date').substr(0, 4)
        infowindow.setContent(event.feature.getProperty('name') + ' (' + date + ')');
        infowindow.setPosition(event.latLng);
        infowindow.open(map);
    });
    map.data.addListener('mouseout', function (event) {
        infowindow.close();
    });
}
function displaySearchPartnerAdress() {
    displayMarkersByPartner(markerContractsList);
    displayMarkersByPartner(markerOffersList);
    displayMarkersByPartner(markerSinistresList);
    displayMarkersByPartner(markerContractsAndOffersByPartnerList);
    displayMarkersByPartner(markerOffersAndeSinistresByPartnerList);
    displayMarkersByPartner(markerContractsAndSinistresByPartnerList);
    displayMarkersByPartner(markerAllCasesByPartnerList);
}
function updateMarkersByPartner() {
    var types = $("#btnLoadContractsByPartner").hasClass("desactivate-option") ? "" : "P";
    types += $("#btnLoadOffersByPartner").hasClass("desactivate-option") ? "" : "O";
    types += $("#btnLoadSinistresByPartner").hasClass("desactivate-option") ? "" : "X";

    switch (types) {
        case "POX":
            displayMarkersByPartner(markerContractsList, true);
            displayMarkersByPartner(markerOffersList, true);
            displayMarkersByPartner(markerSinistresList, true);
            displayMarkersByPartner(markerContractsAndOffersByPartnerList, true);
            displayMarkersByPartner(markerContractsAndSinistresByPartnerList, true);
            displayMarkersByPartner(markerOffersAndeSinistresByPartnerList, true);
            displayMarkersByPartner(markerAllCasesByPartnerList, true);
            break;
        case "PO":
            displayMarkersByPartner(markerContractsList, true);
            displayMarkersByPartner(markerOffersList, true);
            DeleteMarkers(markerSinistresList);
            displayMarkersByPartner(markerContractsAndOffersByPartnerList, true);
            displayMarkersByPartner(markerContractsAndSinistresByPartnerList, true);
            displayMarkersByPartner(markerOffersAndeSinistresByPartnerList, true);
            displayMarkersByPartner(markerAllCasesByPartnerList, true);
            break;
        case "PX":
            displayMarkersByPartner(markerContractsList, true);
            DeleteMarkers(markerOffersList);
            displayMarkersByPartner(markerSinistresList, true);
            displayMarkersByPartner(markerContractsAndOffersByPartnerList, true);
            displayMarkersByPartner(markerContractsAndSinistresByPartnerList, true);
            displayMarkersByPartner(markerOffersAndeSinistresByPartnerList, true);
            displayMarkersByPartner(markerAllCasesByPartnerList, true);
            break;
        case "OX":
            DeleteMarkers(markerContractsList);
            displayMarkersByPartner(markerOffersList, true);
            displayMarkersByPartner(markerSinistresList, true);
            displayMarkersByPartner(markerContractsAndOffersByPartnerList, true);
            displayMarkersByPartner(markerContractsAndSinistresByPartnerList, true);
            displayMarkersByPartner(markerOffersAndeSinistresByPartnerList, true);
            displayMarkersByPartner(markerAllCasesByPartnerList, true);
            break;
        case "P":
            displayMarkersByPartner(markerContractsList, true);
            DeleteMarkers(markerOffersList);
            DeleteMarkers(markerSinistresList);
            displayMarkersByPartner(markerContractsAndOffersByPartnerList, true);
            displayMarkersByPartner(markerContractsAndSinistresByPartnerList, true);
            DeleteMarkers(markerOffersAndeSinistresByPartnerList);
            displayMarkersByPartner(markerAllCasesByPartnerList, true);
            break;
        case "O":
            DeleteMarkers(markerContractsList);
            displayMarkersByPartner(markerOffersList, true);
            DeleteMarkers(markerSinistresList);
            displayMarkersByPartner(markerContractsAndOffersByPartnerList, true);
            DeleteMarkers(markerContractsAndSinistresByPartnerList);
            displayMarkersByPartner(markerOffersAndeSinistresByPartnerList, true);
            displayMarkersByPartner(markerAllCasesByPartnerList, true);
            break;
        case "X":
            DeleteMarkers(markerContractsList);
            DeleteMarkers(markerOffersList);
            displayMarkersByPartner(markerSinistresList, true);
            DeleteMarkers(markerContractsAndOffersByPartnerList);
            displayMarkersByPartner(markerContractsAndSinistresByPartnerList, true);
            displayMarkersByPartner(markerOffersAndeSinistresByPartnerList, true);
            displayMarkersByPartner(markerAllCasesByPartnerList, true);
            break;
        case "":
            DeleteMarkers(markerContractsList);
            DeleteMarkers(markerOffersList);
            DeleteMarkers(markerSinistresList);
            DeleteMarkers(markerContractsAndOffersByPartnerList);
            DeleteMarkers(markerContractsAndSinistresByPartnerList);
            DeleteMarkers(markerOffersAndeSinistresByPartnerList);
            DeleteMarkers(markerAllCasesByPartnerList);
            break;
    }
}
function displayMarkersByPartner(list, addToMap) {
    let i;
    for (i = 0; i < list.length; i++) {
        let marker = list[i][1];

        let markerFullLib = list[i][3];
        let markerTooltipContent = list[i][2];
        if (addToMap) {
            marker.setMap(map);
        }


        marker.addListener('click', function (e) {
            //Review
            markerOnClick(e, markerFullLib, defaultSideBarColor);

        });
        bindInfoWindow(marker, markerTooltipContent);

    }
}
function DisplayDesignationAdress(zoomMax, sideBarColor) {

    let zoom = 6;
    if (markerActesList !== null) {
        if (markerActesList.length > 0) {
            let firstMarkerPosition = markerActesList[0][1].position;
            if (zoomMax) {
                zoom = 1;
            }
        }

        let i;
        for (i = 0; i < markerActesList.length; i++) {
            let marker = markerActesList[i][1];
            let mposition = marker.position;
            let markerFullLib = markerActesList[i][3];
            let markerTooltipContent = markerActesList[i][2];



            marker.addListener('click', function (e) {

                markerOnClick(e, markerFullLib, sideBarColor);

            });
            bindInfoWindow(marker, markerTooltipContent);

        }
    }
}


function DisplayAdress(lat, lon, description) {

    var diameter = GetDiameter();
    if (IsEmpty(diameter)) {
        diameter = 0.1;
    }

    //on centre sur la france si aucune adresse n'est selectionné
    if (lat === null && lon === null) {
        diameter = 700;
        lat = latFrance;
        lon = lonFrance;
    }

    $.when(CreateMap(lat, lon, diameter))
        .done(
            function () {
                //initDataLayers();
            }
        );

    //Creating the circle
    $("#btnZoneCircle span").removeClass("fa-check-circle").addClass("fa-times-circle");
    if (circle) {
        circle.setMap(null);
        circle = null;
    }
    circle = new google.maps.Circle({
        map: map,
        center: { lat: lat, lng: lon },
        radius: diameter * 1000,
        fillColor: '#3388FF',
        fillOpacity: 0.3,
        strokeColor: '#FFFFFF',
        strokeWeight: 3
    });

    placeMarkersIntoMap(circle, markerActesList, pastelBlueColor);
    placeMarkersIntoMap(circle, markerContractsList, pastelBlueColor);
    placeMarkersIntoMap(circle, markerOffersList, pastelPinkColor);
    placeMarkersIntoMap(circle, markerExternalContractsList, defaultSideBarColor);
    placeMarkersIntoMap(circle, markerSinistresList, pastelOrangeColor);
    placeMarkersIntoMap(circle, markerInsuredsList, pastelBrownColor);
    placeMarkersIntoMap(circle, markerBrokersList, pastelGreenColor);
    placeMarkersIntoMap(circle, markerExpertsList, pastelPurpleColor);
    selectedMarker = new google.maps.Marker({
        position: { lat: lat, lng: lon },
        icon: redIcon,
        map: map
    });
    if (!$.isArray(searchAddressMarkersList)) {
        searchAddressMarkersList = [];
    }
    searchAddressMarkersList.push([-1, selectedMarker]);

    if (IsEmpty(description)) {
        description = selectedDescription;
    }
    selectedMarker.addListener('click', function (e) {
        markerOnClick(e, description, defaultSideBarColor);
    });

    var infowindow = new google.maps.InfoWindow({
        content: "<b>" + description + "</b>"
    });
    selectedMarker.addListener('mouseover', function (event) {
        infowindow.open(map, this);
    });

    // assuming you also want to hide the infowindow when user mouses-out
    selectedMarker.addListener('mouseout', function (event) {
        infowindow.close();
    });



    $('#btnLoadContractsAround').prop('disabled', false);
    $('#btnLoadOffersAround').prop('disabled', false);
    $('#btnLoadSinistresAround').prop('disabled', false);
    $('#idEchelle').prop('disabled', false);
    $('#btnLoadInsuredsAround').prop('disabled', false);
    $('#btnLoadBrokersAround').prop('disabled', false);
    $('#btnLoadExpertsAround').prop('disabled', false);
    $('#externalContractsAroundUploadFile').prop('disabled', false);
    $('#btnCleanMarkers').prop('disabled', false);
}

function showMessage(text, color) {
    var message = $("#message");
    if (message) {
        message.find(".box-body").html(text);
        message.css("border-top-color", color);
        message.show();
    }
}
function hideMessage() {
    var message = $("#message");
    if (message) {
        message.hide();
    }
}
function markerOnClick(e, fullLibelle, color) {
    showMessage(fullLibelle, color);
}

function CreateMap(lat, lon, diameter) {
    // Review

    var zoom = GetZoom(diameter);
    var location = new google.maps.LatLng(lat, lon);
    map.setZoom(zoom);
    map.setCenter(location);
    map.panTo(location);
}

/* Layer mamanagement */

/* Get all layers */
function initDataLayers() {

    $.ajax({
        type: "GET",
        url: "Layer/GetAll",
        datatype: "json",
        success: function (result) {
            if (result) {
                dataFeature.features = [];
                var featureddl = $('select#gestionCalque');
                featureddl.html("");
                $.each(result, function (key, feature) {
                    if (feature.Shape) {
                        dataFeature.features.push(JSON.parse(feature.Shape));
                        if (featureddl.find("option[value='" + feature.Name + "']").length > 0) {
                            featureddl.find("option[value='" + feature.Name + "']").data("id", $("option[value='" + feature.Name + "']").data("id") + "," + feature.Id);
                        } else {
                            var date = feature.Date.substr(6, 2) + "-" +
                                feature.Date.substr(4, 2) + "-" +
                                feature.Date.substr(0, 4)
                            featureddl.append('<option value="' + feature.Name + '" data-id="' + feature.Id + '">' + feature.Name + ' (' + date + ')' + '</option>');
                        }
                    }
                });
                featureddl.multiselect("rebuild");
                map.data.forEach(function (f) {
                    map.data.remove(f);
                });
                map.data.addGeoJson(dataFeature, {
                    idPropertyName: "id"
                });

                map.data.setStyle(function (feature) {
                    var calqueDisplay = $('select#gestionCalque').val();
                    var geometry = feature.getGeometry();
                    var type = geometry.getType().toLowerCase();
                    var radius = feature.getProperty('radius');
                    var caracteristique = feature.getProperty('caracteristique');
                    var color = '#880E4F';
                    switch (caracteristique) {
                        case "ZUS":
                            color = "#880E4F"; break;
                        case "TEMPETE":
                            color = "#8E67FD"; break;
                        case "SOUSCRIPTION IN - INTERDIT DE SOUSCRIPTION":
                        case "SOUSCRIPTION IN - ZONE 1":
                        case "SOUSCRIPTION IN - ZONE 2":
                            color = "#FF9900"; break;
                        case "INONDATION":
                            color = "#007BFF"; break;
                    }
                    if (type === 'point') {
                        if (radius) {
                            feature.circle = new google.maps.Circle({
                                map: map,
                                center: geometry.get(),
                                radius: radius,
                                fillColor: '#880E4F',
                                fillOpacity: 0.5,
                                strokeWeight: 0,
                                clickable: true,
                                draggable: false,
                                editable: false
                            });
                            return {
                                visible: false,
                                clickable: true,
                                draggable: false,
                                editable: false
                            };
                        }
                        return ({
                            //icon: getMarkerColor(feature.getProperty('color')),
                        });
                    } else if (type === 'polyline') {
                        return ({
                            strokeColor: color,
                            strokeWeight: 2,
                            clickable: true,
                            draggable: false,
                            editable: false,
                            zIndex: 1
                        });
                    } else {
                        return ({
                            fillColor: color,
                            fillOpacity: 0.5,
                            strokeWeight: 2,
                            clickable: true,
                            draggable: false,
                            editable: false,
                            zIndex: 1,
                            visible: calqueDisplay.indexOf(feature.getProperty("name")) < 0 ? false : true
                        });
                    }
                });

                map.data.addListener('click', function (event) {
                    clearSelectionData();
                    if (event.feature.getProperty("modifiable") == "O") {
                        map.data.overrideStyle(event.feature, editModeOverlayOptions);
                        var elements = document.getElementsByClassName("draw-action");
                        for (var i = 0; i < elements.length; i++) {
                            elements[i].classList.remove("disabled");
                        }
                        selectedFeature = event.feature;
                    }
                });

                bindInfoWindowToFeature();
            }

        }
    });
}

function ShowHideLayer() {
    clearSelectionData();
}
function UpdateLayerSelect(action, id, name) {
    var featureddl = $('select#gestionCalque');
    var today = new Date();
    var date = padLeft(today.getFullYear(), 4) + padLeft(today.getMonth() + 1, 2) + padLeft(today.getDate(), 2);
    if (action == 'delete') {
        var option = featureddl.find('option[value="' + name + '"]');
        var ids = option.data("id").split(",");
        if (ids.length == 1) { option.remove(); }
        else {
            for (var i = 0; i < ids.length; i++) {
                if (ids[i] == id) {
                    ids.splice(i, 1);
                    option.data("id", ids.join(","));
                    break;
                }
            }
        }
    } else if (action == 'add') {
        var option = featureddl.find('option[value="' + name + '"]');
        if (option.length > 0) {
            var ids = option.data("id").split(",");
            ids.push(id);
            option.data("id", ids.join(","));
        } else {
            featureddl.append('<option value="' + name + '" selected data-id="' + id + '">' + name + ' (' + date + ')' + '</option>');
        }
    } else if (action == 'update') {
        featureddl.find('option').each(function (index, option) {
            if ($(option).data("id").indexOf(id) > -1) {
                var ids = $(option).data("id").split(",");
                if (ids.length == 1) { $(option).remove(); }
                else {
                    for (var i = 0; i < ids.length; i++) {
                        if (ids[i] == id) {
                            ids.splice(i, 1);
                            $(option).data("id", ids.join(","));
                            break;
                        }
                    }
                }
            }
        });
        var option = featureddl.find('option[value="' + name + '"]');
        if (option.length > 0) {
            var ids = option.data("id").split(",");
            ids.push(id);
            option.data("id", ids.join(","));
            option.text(name + ' (' + date + ')');
        } else {
            featureddl.append('<option value="' + name + '" selected data-id="' + id + '">' + name + ' (' + date + ')' + '</option>');
        }
    } else {
        featureddl.html("");
        $.each(dataFeature.features, function (key, feature) {
            if (featureddl.find("option[value='" + feature.properties.name + "']").length > 0) {
                featureddl.find("option[value='" + feature.properties.name + "']").data("id", $("option[value='" + feature.properties.name + "']").data("id") + "," + feature.properties.id);
            } else {
                featureddl.append('<option value="' + feature.properties.name + '" data-id="' + feature.properties.id + '">' + feature.properties.name + '</option>');
            }
        })
    }
    featureddl.multiselect('rebuild');
    //clearSelectionData();
}

/* saveLayer : Save layer in db */
function saveLayerToDb(obj, feature, fncallback) {
    $.ajax({
        type: "POST",
        url: "Layer/Save",
        data: {
            layer: {
                id: feature.getProperty('id'),
                name: feature.getProperty('name'),
                modifiable: feature.getProperty('modifiable'),
                caracteristique: feature.getProperty('caracteristique'),
                branche: feature.getProperty('branche'),
                date: feature.getProperty('date'),
                shape: JSON.stringify(obj)
            }
        },
        datatype: "json",
        success: function (result) {
            dataFeature.features.push(obj);
            UpdateLayerSelect('add', feature.getProperty('id'), feature.getProperty('name'));
            map.data.addGeoJson(obj, {
                idPropertyName: "id"
            });
            fncallback();
        }
    });

}
function saveLayer(feature, fncallback) {

    feature.toGeoJson(function (obj) {
        saveLayerToDb(obj, feature, fncallback);
    });
}

function toggleCalqueNom(element) {
    if ($(element).is(":checked")) {
        $("select#calque_nom").show();
        $("input#calque_nom").hide();
    } else {
        $("select#calque_nom").hide();
        $("input#calque_nom").show();
    }

}

function confirmAndSaveLayer(feature, gneratedId, isNew) {
    var title = 'Détails du calque';
    var message = 'Le nom de calque ne doit pas être vide';
    var featureBranche = feature.getProperty("branche") ? feature.getProperty("branche") : "";
    var featureDate = feature.getProperty("date") ? feature.getProperty("date") : "";
    var htmlCarac = '';
    var htmlBr = '';
    for (var i = 0; i < caracteristiqueArray.length; i++) {
        htmlCarac += "<option value='" + removeDiacritics(caracteristiqueArray[i]).toUpperCase() + "' "
            + (feature.getProperty("caracteristique") == removeDiacritics(caracteristiqueArray[i]).toUpperCase() ? "selected" : "") + ">"
            + caracteristiqueArray[i]
            + "</option>"
    }
    for (var i = 0; i < brancheArray.length; i++) {
        htmlBr += "<option value='" + brancheArray[i].split(" - ")[0] + "' "
            + (featureBranche.split(",").indexOf(brancheArray[i].split(" - ")[0]) > -1 ? "selected" : "") + ">"
            + brancheArray[i]
            + "</option>"
    }

    if (isNew) {
        feature.setProperty('name', "");
        feature.setProperty('id', null);
    }
    swal.fire({
        title: title,
        html:
            '<div class="col-md-12">' +
            '<label class="lg-margin-right">Nom</label>' +
            '<input type="checkbox" id="calque_associer" class="calqueInput" onchange="toggleCalqueNom(this);">' +
            '</div>' +
            '<div class="col-md-12" style="margin-bottom:15px;">' +
            '<input id="calque_nom" class="calqueInput form-control" style="font-size:1em;" onchange="$(this).addClass(\'dirty\');" value="' + feature.getProperty("name") + '">' +
            '<select id="calque_nom" class="calqueInput form-control" style="display:none;font-size:1em;">' + $("select#gestionCalque").html() + '</select>' +
            '</div>' +
            '<div class="col-md-12">' +
            '<label class="lg-margin-right">Date de dernière modification</label>' +
            '</div>' +
            '<div class="col-md-12" style="margin-bottom:15px;">' +
            '<input class="form-control" style="font-size:1em;" value="' +
            featureDate.substr(6) + "-" +
            featureDate.substr(4, 2) + "-" +
            featureDate.substr(0, 4) +
            '" disabled>' +
            '</div>' +
            '<div class="col-md-12">' +
            '<label>Caractéristique</label>' +
            '</div>' +
            '<div class="col-md-12" style="margin-bottom:15px;">' +
            '<select id="calque_caracteristique" class="calqueInput form-control" style="font-size:1em;">' + htmlCarac + '</select>' +
            '</div>' +
            '<div class="col-md-12">' +
            '<label>Branche</label>' +
            '</div>' +
            '<div class="col-md-12" style="margin-bottom:15px;">' +
            '<select id="calque_branche" class="calqueInput form-control" multiple title="Choisir branche">' + htmlBr + '</select>' +
            '</div>' +
            '<div class="col-md-12">' +
            '<label class="lg-margin-right">Modifiable</label>' +
            '<input type="checkbox" id="calque_modifiable" class="calqueInput" checked>' +
            '</div>'
        ,
        onOpen: function () {
            $("select#calque_branche").multiselect({
                buttonWidth: "100%",
                buttonText: function (options, select) {
                    if (options.length === 0) {
                        return select.attr("title");
                    }
                    else {
                        var labels = [];
                        options.each(function () {
                            if ($(this).attr('label') !== undefined) {
                                labels.push($(this).attr('label'));
                            }
                            else {
                                labels.push($(this).html());
                            }
                        });
                        return labels.join(', ') + '';
                    }
                },
                templates: {
                    button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown" style="text-align:left;font-size:1em;border:1px lightgrey solid;"><span class="multiselect-selected-text"></span> <b></b></button>',
                    ul: '<ul class="multiselect-container dropdown-menu" style="width:100%;"></ul>',
                    li: '<li><a tabindex="0"><label style="font-size:1.5em;"></label></a></li>',
                }
            });
            $(".swal2-actions").css("z-index", 0);
            if ($("select#calque_nom").find("option").length == 0) $("#calque_associer").hide();
        },
        inputValidator: function (value) {
            if (!value) {
                return message;
            }
        },
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Enregistrer',
        cancelButtonText: 'Annuler',
        showLoaderOnConfirm: true,
        preConfirm: function () {
            var today = new Date();
            var associer = $("#calque_associer").is(":checked");
            var dirty = $("input#calque_nom").hasClass("dirty");
            var tempName = removeDiacritics($("input#calque_nom").val());
            var name = associer ? ($("select#calque_nom").val() == null ? "" : $("select#calque_nom").val()) : tempName;
            var modifiable = $("#calque_modifiable").is(":checked");
            var carac = $("#calque_caracteristique").val();
            var branche = transformSelectionToStringWithoutQuote($("#calque_branche").val());
            var date = padLeft(today.getFullYear(), 4) + padLeft(today.getMonth() + 1, 2) + padLeft(today.getDate(), 2);
            return existLayer(name, modifiable, carac, branche, date, associer, dirty);
        },
        allowOutsideClick: function () { !Swal.isLoading(); }
    }).then(function (result) {
        if (result.value) {
            Swal.showValidationMessage("Error");
            feature.setProperty('name', result.value.name);
            feature.setProperty('modifiable', result.value.modifiable);
            feature.setProperty('caracteristique', result.value.carac);
            feature.setProperty('branche', result.value.branche);
            feature.setProperty('id', gneratedId);
            feature.setProperty('date', result.value.date);
            if (isNew) {
                saveLayer(feature,
                    function () {
                        swal.fire(
                            'Enregistrement',
                            'le calque sélectionné a été enregistré',
                            'success'
                        );
                    }
                );
            } else {
                updateLayer(feature,
                    function () {
                        swal.fire(
                            'Enregistrement',
                            'le calque sélectionné a été enregistré',
                            'success'
                        );
                    }
                );
            }
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            if (selectedShape) {
                selectedShape.setMap(null);
                selectedShape = null;
            }
            clearSelectionData();
            swal.fire(
                'Annulation',
                'le calque sélectionné a été supprimé',
                'error'
            );
        }
    });
}

/* existLayer : layer name verification */
function existLayer(name, modifiable, carac, branche, date, associer, dirty) {

    return new Promise(function (resolve, reject) {
        if (name == "") {
            Swal.showValidationMessage("le nom du calque est obligatoire");
            resolve(null);
        }
        var objet = new Object();
        objet.name = name;
        objet.modifiable = modifiable ? 'O' : 'N';
        objet.carac = carac;
        objet.branche = branche;
        objet.date = date
        if (associer || !dirty) {
            resolve(objet);
        }
        $.ajax({
            type: "POST",
            url: "Layer/Exist",
            data: { name: name },
            success: function (result) {

                if (result) {
                    var nom = $("input#calque_nom").val();
                    Swal.showValidationMessage("Le nom du calque existe déjà. Souhaitez-vous fusionner les 2 calques ? <br />" +
                        '<button type="button" class="swal2-cancel swal2-styled" onclick="fusionnerCalque(\'' + nom + '\');">Oui</button>');
                    resolve(null);
                } else {
                    resolve(objet);
                }
            }
        });

    });
}
function fusionnerCalque(name) {
    $("#calque_associer").prop("checked", true);
    $("select#calque_nom").val(name);
    swal.getConfirmButton().click();
}

function padLeft(nr, n, str) {
    return Array(n - String(nr).length + 1).join(str || '0') + nr;
}

/* updateLayers : Update multi-layers */
function updateLayers(layers, fncallback) {
    var data = [];
    $.each(layers, function (key, layer) {

        data.push({ id: layer.feature.properties.id, name: layer.feature.properties.name, shape: JSON.stringify(layer.toGeoJSON()), });
    });
    $.ajax({
        type: "POST",
        url: "Layer/Update",
        data: { layers: data },
        success: function (result) {
            fncallback();
        }
    });
}

/* saveLayer : Save layer in db */
function updateLayerToDb(obj, feature, fncallback) {
    $.ajax({
        type: "POST",
        url: "Layer/Update",
        data: {
            layer: {
                id: feature.getProperty('id'),
                name: feature.getProperty('name'),
                modifiable: feature.getProperty('modifiable'),
                caracteristique: feature.getProperty('caracteristique'),
                branche: feature.getProperty('branche'),
                date: feature.getProperty('date'),
                shape: JSON.stringify(obj)
            }
        },
        datatype: "json",
        success: function (result) {
            UpdateLayerSelect('update', feature.getProperty('id'), feature.getProperty('name'));
            fncallback();
        }
    });

}
function updateLayer(feature, fncallback) {

    feature.toGeoJson(function (obj) {
        updateLayerToDb(obj, feature, fncallback);
    });

}
/* deleteLayers : delete multi-layers */
function deleteLayers(layers, fncallback) {
    var data = [];
    $.each(layers, function (key, layer) {
        data.push(layer.feature.properties.id);
    });
    $.ajax({
        type: "POST",
        url: "Layer/Delete",
        data: { ids: data },
        success: function (result) {
            fncallback();
        }
    });
}
function deleteLayer(id, name, fncallback) {

    $.ajax({
        type: "POST",
        url: "Layer/Delete",
        data: { id: id },
        success: function (result) {
            var index = -1;
            for (var i = 0; i < dataFeature.features.length; i++) {
                if (dataFeature.features[i].properties.id == id) index = i;
            }
            if (index > -1) dataFeature.features.splice(index, 1);
            UpdateLayerSelect('delete', id, name);
            map.data.remove(map.data.getFeatureById(id));
            fncallback();
        }
    });
}



function initControls() {
    var legend = document.createElement('div');
    legend.innerHTML = legendHtml;
    legend.id = "legend";
    legend.style.display = "none";
    legend.style.backgroundColor = "white";
    legend.style.padding = "5px";
    legend.classList.add("legend");
    map.controls[google.maps.ControlPosition.RIGHT_TOP].push(legend);

    var message = document.createElement('div');
    message.innerHTML = '<div class="box-header"> <h3 class="box-title"></h3> <div class="box-tools pull-right"> <button type="button" class="btn btn-box-tool"  onclick="hideMessage();"  ><i class="fa fa-times"></i> </button> </div> </div> <div class="box-body"> </div>';
    message.id = "message";
    message.style.display = "none";
    message.classList.add("box");
    message.classList.add("box-default");
    message.classList.add("gmaps-message");
    map.controls[google.maps.ControlPosition.RIGHT_TOP].push(message);
}
function GetZoom(diameter) {
    if (diameter <= 0.1) {
        return 17;
    }
    if (diameter <= 0.3) {
        return 16;
    }
    if (diameter <= 0.5) {
        return 15;
    }
    if (diameter <= 1) {
        return 14;
    }
    if (diameter <= 5) {
        return 12;
    }
    if (diameter <= 10) {
        return 11;
    }
    if (diameter <= 20) {
        return 10;
    }
    if (diameter <= 50) {
        return 9;
    }
    if (diameter <= 100) {
        return 8;
    }
    if (diameter <= 200) {
        return 7;
    }
    if (diameter <= 400) {
        return 6;
    }
    return 5;
}

function getCurrentAdresse() {

    var pays = removeDiacritics(document.querySelector('#country').value.toUpperCase());
    var numero = removeDiacritics(document.querySelector('#street_number').value.toUpperCase());
    var numero2 = removeDiacritics(document.querySelector('#street_number2').value.toUpperCase());
    var extension = removeDiacritics(document.querySelector('#formExtension').value.toUpperCase());
    var voie = removeDiacritics(document.querySelector('#route').value.toUpperCase());
    var distribution = removeDiacritics(document.querySelector('#formDistribution').value.toUpperCase());
    var batiment = removeDiacritics(document.querySelector('#formBatiment').value.toUpperCase());
    var villeCedex = removeDiacritics(document.querySelector('#formVilleCedex').value.toUpperCase());
    var departement = removeDiacritics(document.querySelector('#administrative_area_level_1').value.toUpperCase());
    var matriculeHexavia = $("#optAdresseManuelle").is(":checked") ? 0 : 1;

    var cp = '';
    var ville = '';
    var cpCedex = '';
    if (paysFr.indexOf(pays) < 0) {
        ville = removeDiacritics(document.querySelector('#postal_code').value.toUpperCase()) + '|' + removeDiacritics(document.querySelector('#locality').value.toUpperCase());
        //ville = removeDiacritics(document.querySelector('#postal_code').value.toUpperCase()) + ' ' + removeDiacritics(document.querySelector('#locality').value.toUpperCase());
    } else {
        cp = removeDiacritics(document.querySelector('#postal_code').value.toUpperCase());
        ville = removeDiacritics(document.querySelector('#locality').value.toUpperCase());
        cpCedex = removeDiacritics(document.querySelector('#formCPCedex').value.toUpperCase());
    }

    var num = numero + (numero == "" || numero2 == "" ? "" : "/" + numero2);

    var adresse = {
        Batiment: batiment,
        BoitePostale: distribution,
        NomVoie: voie,
        NumeroVoie: num,
        ExtensionVoie: extension,
        CodePostal: cp,
        CodePostalCedex: cpCedex,
        Ville: ville,
        VilleCedex: villeCedex,
        MatriculeHexavia: matriculeHexavia,
        Departement: departement,
        Pays: { Libelle: pays }
    };
    return adresse;
}

function splitAddress(adresse) {
    var a = adresse.trim().split(' '), number, street;

    if (a.length <= 1) {
        return { number: '', extension: '', adresse: a.join('') };
    }

    if (isNumber(a[0].substr(0, 1)) || isFractionalChar(a[0].substr(0, 1))) {
        number = a.shift();
    } else {
        // If there isn't a leading number, just return the trimmed input as the street
        return { number: '', extension: '', adresse: adresse.trim() }
    }

    if (/[0-9]\/[0-9]/.exec(a[0]) || indexFractionalChar(a[0]) !== false) {
        number += ' ' + a.shift();
    }

    var arrayLength = extensions.length;
    var extension = "";
    var indexNumber = -1;
    var indexExtension = -1;
    var index = -1;
    var adresseLower = adresse.toLowerCase();

    for (var i = 0; i < arrayLength; i++) {
        if (adresseLower.indexOf(" " + extensions[i].toLowerCase() + " ") >= 0) {
            extension = " " + extensions[i].toLowerCase() + " ";
            indexExtension = adresseLower.indexOf(extension);
            break;
        }
    }

    if (indexExtension > -1) {
        index = indexExtension + extension.length;
    } else {
        index = adresseLower.indexOf(number) + number.length;
    }

    if (index != -1) {
        adresseLower = adresseLower.substr(index);
    }

    return { number: number, extension: extension.trim(), adresse: adresseLower.toUpperCase() };
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function isFractionalChar(n) {
    c = n.charCodeAt();
    return (c >= 188 && c <= 190) || (c >= 8531 && c <= 8542);
}

function indexFractionalChar(m) {
    var a = m.split(''), i;
    for (i in a) {
        if (isFractionalChar(a[i]))
            return i;
    }
    return false;
}

function validate(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

function GetDiameter() {
    var diameter = document.querySelector('#idEchelle').value;
    if (IsEmpty(diameter)) {
        diameter = 0.1;
    }
    return diameter;
}

function NettoyerFormEtMap(NotToCleanGoogleAreaSearch) {
    resetMap();
    document.querySelector('#street_number').value = "";
    document.querySelector('#street_number2').value = "";
    document.querySelector('#route').value = "";
    document.querySelector('#externalContractsAroundUploadFile').value = "";
    document.querySelector('#locality').value = "";
    document.querySelector('#postal_code').value = "";
    document.querySelector('#country').value = "";
    document.querySelector('#administrative_area_level_1').value = "";
    document.querySelector('#formExtension').value = "";
    document.querySelector('#formBatiment').value = "";
    document.querySelector('#formDistribution').value = "";
    document.querySelector('#formVilleCedex').value = "";
    document.querySelector('#formCPCedex').value = "";

    if (NotToCleanGoogleAreaSearch === undefined) {
        document.querySelector('#formAddressRecherche').value = "";
    }

    document.querySelector('#designationSearch').value = "";
    document.querySelector('#codePartenaire').value = "";
    document.querySelector('#codePartenaire').disabled = false;
    document.querySelector('#idEchelle').value = "0.1";




    $('#optAssures').prop('checked', true);
    resetCbb();
    $("#optContrats").prop('checked', true);
    $("#optOffres").prop('checked', true);
    $("#optSinistres").prop('checked', false);
    sinistreView(false);
    $("#btnSearchByDesignation").prop('disabled', true);
    $("#btnRechercherPartenaires").prop('disabled', true);

    $(".messageOptionDesignation").hide();


    $('#btnLoadContractsAround').prop('disabled', true);
    $('#btnLoadOffersAround').prop('disabled', true);
    $('#btnLoadSinistresAround').prop('disabled', true);

    $('#btnLoadContractsByPartner').prop('disabled', true);
    $('#btnLoadOffersByPartner').prop('disabled', true);
    $('#btnLoadSinistresByPartner').prop('disabled', true);


    $('#idEchelle').prop('disabled', true);
    $('#btnLoadInsuredsAround').prop('disabled', true);
    $('#btnLoadBrokersAround').prop('disabled', true);
    $('#btnLoadExpertsAround').prop('disabled', true);
    $('#externalContractsAroundUploadFile').prop('disabled', true);
    $('#btnUploadExternalContractsAround').prop('disabled', true);
    $('#btnCleanMarkers').prop('disabled', true);

    desactivateOptions();
    $('.searchPartenaireOtherFilters').each(function () {
        $(this)[0].value = "";
    });
    $("#searchPartenaireCourtierBlock").hide();

    if ((map !== undefined) && (map !== null)) {


        hideMessage();
    }
}
function clearSelectedMarker() {
    if (selectedLatitude != null || selectedLongitude != null) {
        selectedLatitude = null;
        selectedLongitude = null;
        selectedDescription = null;
    }
}
function resetMap() {
    if (map) {
        map.setZoom(defaultZoom);
        map.setCenter(defaultLocation);
        deleteAllMarkers(true);
        if (circle) {
            circle.setMap(null);
            circle = null;
        }
        hideMessage();
    }
}
function deleteAllMarkers(withSearchAddress) {

    DeleteMarkers(markerActesList);
    DeleteMarkers(markerContractsList);
    DeleteMarkers(markerOffersList);
    DeleteMarkers(markerSinistresList);
    DeleteMarkers(markerInsuredsList);
    DeleteMarkers(markerBrokersList);
    DeleteMarkers(markerExpertsList);
    DeleteMarkers(markerContractsAndOffersByPartnerList);
    DeleteMarkers(markerContractsAndSinistresByPartnerList);
    DeleteMarkers(markerOffersAndeSinistresByPartnerList);
    DeleteMarkers(markerAllCasesByPartnerList);
    DeleteMarkers(markerExternalContractsList);

    markerActesList = [];
    markerContractsList = [];
    markerOffersList = [];
    markerSinistresList = [];
    markerInsuredsList = [];
    markerBrokersList = [];
    markerExpertsList = [];
    markerContractsAndOffersByPartnerList = [];
    markerContractsAndSinistresByPartnerList = [];
    markerOffersAndeSinistresByPartnerList = [];
    markerExternalContractsList = [];

    if (withSearchAddress) {
        DeleteMarkers(searchAddressMarkersList);
        searchAddressMarkersList = [];
    }
}
function DeleteMarkers(markers) {
    if ($.isArray(markers)) {
        for (var i = 0; i < markers.length; i++) {
            if (markers[i][1]) {
                let marker = markers[i][1];
                setTimeout(function () {
                    marker.setMap(null);
                    marker = null;
                }, 10);

            }

        }
    }
}
function removeButton(idBtn) {
    var elem = document.getElementById(idBtn);
    elem.parentNode.removeChild(elem);
}

function isAdresseVide() {
    var vide = true;
    $(".adresseInput").each(function (i, n) {
        if ($(n).val() != "" && $(n).val() != null) { vide = false; }
    });
    return vide;
}

function CleanMarkersExceptSelected() {
    resetMap();


    if (selectedLatitude !== null && selectedLongitude !== null) {
        let adresse = document.querySelector('#formAddressRecherche').value;
        DisplayAdress(selectedLatitude, selectedLongitude, adresse);
    }
}

function loadAffaireNouvelleAround(selectedLongitude, selectedLatitude, diametre, type, iconMarker, button) {
    var res = { isSuccess: false, count: 0 };
    var branche = "";
    $.ajax({
        type: 'POST',
        url: 'OfferContract/LoadAffairesAroundGPSPoint',
        data: { longitude: selectedLongitude, latitude: selectedLatitude, diametre: diametre, typeDesignation: type, branche: branche },
        dataType: 'json',
        success: function (listAffaires) {
            if (type === "P") {
                DeleteMarkers(markerContractsList);
                markerContractsList = [];
            }
            else if (type === "O") {
                DeleteMarkers(markerOffersList);
                markerOffersList = [];
            }
            else if (type === "X") {
                DeleteMarkers(markerSinistresList);
                markerSinistresList = [];
            }

            for (i = 0; i < listAffaires.length; i++) {
                marker = [
                    listAffaires[i].NumeroChrono,
                    new google.maps.Marker({
                        position: { lat: listAffaires[i].Lat, lng: listAffaires[i].Lon },
                        icon: iconMarker,
                        map: map
                    }),
                    listAffaires[i].Libelle,
                    listAffaires[i].FullLibelle
                ];

                if (type === "P") {
                    markerContractsList.push(marker);
                }
                else if (type === "O") {
                    markerOffersList.push(marker);
                }
                else if (type === "X") {
                    markerSinistresList.push(marker);
                }
            }

            let adresse = document.querySelector('#formAddressRecherche').value;
            DisplayAdress(selectedLatitude, selectedLongitude, adresse);
            res.isSuccess = true;
            res.count = listAffaires.length;

            button.html(button.data('original-text'));
            activateOption(button);
            CloseLoading();
        },
        error: function () {
            button.html(button.data('original-text'));
            CloseLoading();
            let settings = {
                type: 'error',
                title: '',
                text: 'Un problème est survenu '
            };
            alertDialog(settings);
        }
    });

    return res;
}

function selectCorrectMarkerIconByBranche(branche) {
    var icon = redIcon;
    switch (branche) {
        case 'CO':
            icon = redIcon;
            break;
        case 'IA':
            icon = ltblueIcon;
            break;
        case 'MR':
            icon = blueIcon;
            break;
        case 'RC':
            icon = greenIcon;
            break;
        case 'RS':
            icon = pinkIcon;
            break;
        case 'RT':
            icon = orangeIcon;
            break;
        case 'TR':
            icon = brownIcon;
            break;
        default:
            break;
    }

    return icon;
}


function addMarkerByPartnerSearch(element) {
    let icon = redIcon;
    let types = element.Types;
    let type = '';
    if ($.isArray(types)) {
        if (containsAll(['P'], types)) {
            icon = contractIcon;
            type = 'P';
        }
        if (containsAll(['O'], types)) {
            icon = offerIcon;
            type = 'O';
        }
        if (containsAll(['X'], types)) {
            icon = sinistreIcon;
            type = 'X';
        }
        if (containsAll(['P', 'O'], types)) {
            icon = contractAndOfferIcon;
            type = 'PO';
        }
        if (containsAll(['P', 'X'], types)) {
            icon = contractAndsinistreIcon;
            type = 'PX';
        }
        if (containsAll(['O', 'X'], types)) {
            icon = offerAndSinistreIcon;
            type = 'OX';
        }
        if (containsAll(['P', 'O', 'X'], types)) {
            icon = allCasesIcon;
            type = 'POX';
        }
    }
    let marker = [
        element.NumeroChrono,
        new google.maps.Marker({
            position: { lat: element.Lat, lng: element.Lon },
            icon: icon,
            map: map
        }),
        element.Libelle,
        element.FullLibelle
    ];
    switch (type) {
        case 'P': markerContractsList.push(marker); break;
        case 'O': markerOffersList.push(marker); break;
        case 'X': markerSinistresList.push(marker); break;
        case 'PO': markerContractsAndOffersByPartnerList.push(marker); break;
        case 'PX': markerContractsAndSinistresByPartnerList.push(marker); break;
        case 'OX': markerOffersAndeSinistresByPartnerList.push(marker); break;
        case 'POX': markerAllCasesByPartnerList.push(marker); break;
    }

}

function validateSearchByDesignation() {
    let code = document.querySelector('#code').value;
    let designationSearch = document.querySelector('#designationSearch').value;
    let etat = document.querySelector('#cbbEtat').value;
    let garantie = document.querySelector('#cbbGarantie').value;
    let situation = document.querySelector('#cbbSituation').value;
    let branche = document.querySelector('#cbbBrancheSearchDesignation').value;
    let departement = document.querySelector('#cbbDepartement').value;
    let evenement = document.querySelector('#cbbEvenement').value;

    if (IsEmpty(code) && IsEmpty(designationSearch) && IsEmpty(etat) && IsEmpty(situation) && IsEmpty(branche) && IsEmpty(garantie) && IsEmpty(departement) && IsEmpty(evenement) || (!$("#optContrats").prop('checked') && !$("#optOffres").prop('checked') && !$("#optSinistres").prop('checked'))) {
        $('#btnSearchByDesignation').prop('disabled', true);
    }
    else {
        $('#btnSearchByDesignation').prop('disabled', false);
    }
}

function validateLoadContracts() {
    let fileUploaded = document.querySelector('#externalContractsAroundUploadFile').value;
    var length = fileUploaded.length;
    if (fileUploaded !== undefined && fileUploaded !== "" && fileUploaded.indexOf(fileUploadExtension) == length - 4) {
        $('#btnUploadExternalContractsAround').prop('disabled', false);
    }
    else {
        $('#btnUploadExternalContractsAround').prop('disabled', true);
    }
}

function validateSearchPartnerByOtherFilters() {
    var countNotEmpty = 0;
    $('.searchPartenaireOtherFilters').each(function () {
        if ($(this)[0].value !== "") {
            countNotEmpty++;
        }
    });

    $("#codePartenaire").prop('disabled', countNotEmpty !== 0);
}
function calcDistance(p1, p2) {
    return (google.maps.geometry.spherical.computeDistanceBetween(p1, p2)).toFixed(2);
}
function placeMarkersIntoMap(circle, markerList, color) {
    var cposition = circle.getCenter();
    var cradius = circle.getRadius();

    if (markerList !== null) {
        let i;
        for (i = 0; i < markerList.length; i++) {
            let market = markerList[i][1];
            let mposition = market.position;

            if (calcDistance(cposition, mposition) <= cradius) {
                let markerFullLib = markerList[i][3];

                market.addListener('click', function (e) {
                    markerOnClick(e, markerFullLib, color);
                });
                market.setMap(map);
                bindInfoWindow(market, markerList[i][2]);
            }
        }
    }
}

function transformSelectionToString(arraySelection) {
    var res = "";

    jQuery.each(arraySelection, function (index, val) {
        if (res !== "") {
            res += ",";
        }
        res += "'" + val + "'";
    });

    return res;
}

function transformSelectionToStringWithoutQuote(arraySelection) {
    var res = "";

    jQuery.each(arraySelection, function (index, val) {
        if (res !== "") {
            res += ",";
        }
        res += val;
    });

    return res;
}



/* Google Maps API 2 Handling START */

/* variables definition */


function clearMapData() {
    map.data.forEach(function (f) {
        map.data.remove(f);
    });
}
function clearSelectionData() {
    selectedFeature = null;
    document.getElementById('overlay-name').textContent = "";
    map.data.forEach(function (feature) { map.data.revertStyle(feature); });
    document.getElementsByClassName("delete-overlay")[0].classList.add("disabled");
    document.getElementsByClassName("update-overlay")[0].classList.add("disabled");
}
function clearSelection() {
    if (selectedShape) {
        clearSelectionData();
        setStyleSelection(selectedShape, false);
        selectedShape = null;
    }

}
function setStyleSelection(shape, editable) {
    var overlayOptions = editable ? editModeOverlayOptions : defaultOverlayOptions;
    if (editable) {
        document.getElementsByClassName("delete-overlay")[0].classList.remove("disabled");
    }
    shape.setOptions(overlayOptions);

}
function setSelection(shape) {
    if (shape) {
        clearSelection();
        setStyleSelection(shape, true);
        selectedShape = shape;

    }
}
function deleteSelectedShape() {
    if (selectedShape) {
        deleteLayer(selectedShape.id, selectedShape.name, function () {
            selectedShape.setMap(null);

            //hacked
            map.data.forEach(function (f) {
                if (f.getProperty('id') === selectedShape.id) {
                    map.data.remove(f);
                }
            });
            clearSelectionData();
        });
    }

    if (selectedFeature) {
        var id = selectedFeature.getProperty('id');
        var name = selectedFeature.getProperty('name');
        deleteLayer(id, name, function () {

            //hacked
            map.data.forEach(function (f) {
                if (f.getProperty('id') === id) {
                    map.data.remove(f);
                }
            });
            clearSelectionData();
        });
    }
}

function updateSelectedShape() {
    if (selectedShape) {
        confirmAndSaveLayer(selectedShape, selectedShape.id, false);
    }

    if (selectedFeature) {
        var id = selectedFeature.getProperty('id');
        confirmAndSaveLayer(selectedFeature, id, false)
    }
}
/*configure toolbar custom buttons*/
function addElementsToDrawToolbar(elements) {
    $.each(elements, function (key, element) {
        var controlDiv = document.createElement('div');
        controlDiv.id = element.id;
        if (element.isButton) {

            controlDiv.classList.add("draw-action-container");
            // Set CSS for the control border
            var controlUI = document.createElement('div');
            controlUI.title = element.title;
            controlUI.classList.add("draw-action");
            controlUI.classList.add(element.customClass);
            if (element.disabled) {
                controlUI.classList.add("disabled");
            }
            if (!element.visible) {
                controlUI.style.display = "none";
            }
            controlDiv.appendChild(controlUI);

            // Set CSS for the control interior
            var controlText = document.createElement('div');
            controlText.innerHTML = element.innerHTML;
            controlText.classList.add("draw-action-text");
            controlUI.appendChild(controlText);
            // Setup the click event listeners
            controlUI.addEventListener('click', element.click);
            controlDiv.index = element.index;
            map.controls[google.maps.ControlPosition.BOTTOM_LEFT].push(controlDiv);

        } else {
            controlDiv.classList.add(element.customClass);
            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(controlDiv);
        }


    });
}
/*Toolbar custom buttons*/
function customDrawToolbar() {
    var elements = [{
        id: 'open-overlay',
        backgroundColor: '#1D9D73',
        color: '#FFF',
        innerHTML: 'Gestion de calque <i class="fa fa-plus-circle" aria-hidden="true"></i> ',
        title: 'Gestion de calque',
        index: 1,
        disabled: false,
        isButton: true,
        visible: true,
        customClass: 'open-overlay',
        click: function () {
            //set drawing control to null
            drawingManager.setDrawingMode(null);
            var drawingControlVisibilty = drawingManager.drawingControl;
            drawingManager.setOptions({
                drawingControl: !drawingControlVisibilty
            });
            $(".delete-overlay, .update-overlay, .refresh-overlay, .close-overlay").show();
            $(".open-overlay").hide();
        }
    },
    {
        id: 'update-overlay',
        backgroundColor: '#1D9D73',
        color: '#FFF',
        innerHTML: 'Editer un calque <i class="fa fa-edit" aria-hidden="true"></i>',
        title: 'Editer un calque',
        index: 2,
        disabled: true,
        customClass: 'update-overlay',
        isButton: true,
        visible: false,
        click: function () {
            updateSelectedShape();
        }
    },
    {
        id: 'delete-overlay',
        backgroundColor: '#DE2801',
        color: '#FFF',
        innerHTML: 'Supprimer un calque <i class="fa fa-trash-o" aria-hidden="true"></i>',
        title: 'Supprimer un calque',
        index: 2,
        disabled: true,
        customClass: 'delete-overlay',
        isButton: true,
        visible: false,
        click: function () {
            var settings = {
                title: "",
                text: "La suppression du calque est définitive, voulez-vous continuer ?",
                type: "warning",
                confirmButtonText: 'Oui',
                cancelButtonText: 'Non',
                confirmCallback: {
                    notification: { canShow: false, title: "", text: "" },
                    actionCallback: function () {
                        deleteSelectedShape();
                    }
                },
                cancelCallback: {
                    notification: { canShow: false, title: "", text: "" },
                    actionCallback: function () {

                    }
                }
            };
            confirmDialog(settings);
        }
    },
    {
        backgroundColor: '#E7971C',
        id: 'refresh-overlay',
        color: '#FFF',
        innerHTML: '<i class="fa fa-refresh" aria-hidden="true"></i>',
        title: "rafraîchir",
        index: 3,
        disabled: false,
        isButton: true,
        visible: false,
        customClass: 'refresh-overlay',
        click: function () {
            //clearSelection();
            clearSelectionData();
        }
    },
    {
        backgroundColor: '#DE2801',
        id: 'close-overlay',
        color: '#FFF',
        innerHTML: '<i class="fa fa-times" aria-hidden="true"></i>',
        title: "fermer",
        index: 3,
        disabled: false,
        isButton: true,
        visible: false,
        customClass: 'close-overlay',
        click: function () {
            drawingManager.setDrawingMode(null);
            var drawingControlVisibilty = drawingManager.drawingControl;
            drawingManager.setOptions({
                drawingControl: !drawingControlVisibilty
            });
            $(".delete-overlay, .update-overlay, .refresh-overlay, .close-overlay").hide();
            $(".open-overlay").show();
        }
    },
    {
        id: 'overlay-name',
        backgroundColor: '#DE2801',
        color: '#FFF',
        innerHTML: '',
        title: 'txt',
        index: 2,

        customClass: 'overlay-label',
        isButton: false

    }

    ];
    addElementsToDrawToolbar(elements);
}

/*Drawing tools */
function initDrawTools() {
    // add new buttons to toolbar
    customDrawToolbar();
    drawingManager = new google.maps.drawing.DrawingManager({

        drawingControl: false,
        drawingControlOptions: {
            position: google.maps.ControlPosition.BOTTOM_LEFT,
            drawingModes: [
                google.maps.drawing.OverlayType.POLYGON,
                google.maps.drawing.OverlayType.RECTANGLE
            ]
        },
        polylineOptions: defaultOverlayOptions,
        rectangleOptions: defaultOverlayOptions,
        circleOptions: defaultOverlayOptions,
        polygonOptions: defaultOverlayOptions,
        map: map
    });

}
/*Events*/
function initMapEvents() {
    google.maps.event.addListener(drawingManager, 'overlaycomplete', function (event) {
        var feature;
        var id = generateUUID();
        var newShape = event.overlay;
        newShape.type = event.type;
        newShape.id = id;
        newShape.setMap(null);
        clearSelectionData();


        switch (event.type) {
            case google.maps.drawing.OverlayType.MARKER:
                feature = new google.maps.Data.Feature({
                    geometry: new google.maps.Data.Point(event.overlay.getPosition())
                });

                break;
            case google.maps.drawing.OverlayType.RECTANGLE:
                var b = event.overlay.getBounds(),
                    p = [b.getSouthWest(), {
                        lat: b.getSouthWest().lat(),
                        lng: b.getNorthEast().lng()
                    }, b.getNorthEast(), {
                        lng: b.getSouthWest().lng(),
                        lat: b.getNorthEast().lat()
                    }];
                feature = new google.maps.Data.Feature({
                    geometry: new google.maps.Data.Polygon([p])
                });

                break;
            case google.maps.drawing.OverlayType.POLYGON:
                feature = new google.maps.Data.Feature({
                    geometry: new google.maps.Data.Polygon([event.overlay.getPath().getArray()])
                });
                break;
            case google.maps.drawing.OverlayType.POLYLINE:
                feature = new google.maps.Data.Feature({
                    geometry: new google.maps.Data.LineString(event.overlay.getPath().getArray())
                });
                break;
            case google.maps.drawing.OverlayType.CIRCLE:
                feature = new google.maps.Data.Feature({
                    properties: {
                        radius: event.overlay.getRadius()
                    },
                    geometry: new google.maps.Data.Point(event.overlay.getCenter())
                });
                break;
        }

        // Switch back to non-drawing mode after drawing a shape.
        drawingManager.setDrawingMode(null);

        //if (event.type !== google.maps.drawing.OverlayType.MARKER) {
        //    // Switch back to non-drawing mode after drawing a shape.
        //    drawingManager.setDrawingMode(null);

        //    // Add an event listener that selects the newly-drawn shape when the user
        //    // mouses down on it.
        //    google.maps.event.addListener(newShape, 'click', function (e) {
        //        if (event.vertex !== undefined) {
        //            if (newShape.type === google.maps.drawing.OverlayType.POLYGON) {
        //                let path = newShape.getPaths().getAt(event.path);
        //                path.removeAt(event.vertex);
        //                if (path.length < 3) {
        //                    newShape.setMap(null);
        //                }
        //            }
        //            if (newShape.type === google.maps.drawing.OverlayType.POLYLINE) {
        //                let path = newShape.getPath();
        //                path.removeAt(event.vertex);
        //                if (path.length < 2) {
        //                    newShape.setMap(null);
        //                }
        //            }
        //        }
        //        setSelection(newShape);
        //    });
        //           setSelection(newShape);
        //}
        //else {
        //    google.maps.event.addListener(newShape, 'click', function (e) {
        //        setSelection(newShape);
        //    });
        //         setSelection(newShape);
        //}

        if (feature) {
            confirmAndSaveLayer(feature, id, true);
        }
        var infowindow = new google.maps.InfoWindow();
        google.maps.event.addListener(newShape, 'mouseover', function (event) {
            var name = feature.getProperty('name');
            if (name) {
                infowindow.setContent(name);
                infowindow.setPosition(event.latLng);
                infowindow.open(map);
            }

        });
        google.maps.event.addListener(newShape, 'mouseout', function (event) {
            infowindow.close();
        });


    });
    // Clear the current selection when the drawing mode is changed, or when the
    // map is clicked.
    google.maps.event.addListener(map, 'click', function () {
        clearSelection();
        hideMessage();
    });
    google.maps.event.addListener(drawingManager, 'drawingmode_changed', clearSelection);
    google.maps.event.addListener(map, 'click', clearSelection);
    map.data.addListener('setgeometry', function (event) {
        var feature = event.feature;
        if (feature) {
            updateLayer(feature, function () { });
        }
    });

    google.maps.event.addListener(map, "bounds_changed", function () {
        // send the new bounds back to your server
        boundaries = map.getBounds()
    });
}

function initSearchBox() {


    // Create the search box and link it to the UI element.
    var input = document.getElementById('formAddressRecherche');
    var searchBox = new google.maps.places.SearchBox(input);

    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    var markers = [];
    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();

        if (places.length === 0 || places[0].address_components === undefined) {
            NettoyerFormEtMap(true);
            return;
        }

        // Clear out the old markers.
        markers.forEach(function (marker) {
            marker.setMap(null);
        });
        markers = [];
        // For each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }

            var streetNumber = $("#formAddressRecherche").val().split(' ')[0];
            NettoyerFormEtMap();
            // Get each component of the address from the place details,
            // and then fill-in the corresponding field on the form.
            for (var i = 0; i < componentForm.length; i++) {
                var val = '';
                var typename = componentForm[i]["name"] == "administrative_area_level_1" ? "short_name" : "long_name";
                for (var j = 0; j < componentForm[i]["types"].length; j++) {
                    for (var k = 0; k < place.address_components.length; k++) {
                        if (place.address_components[k]["types"].indexOf(componentForm[i]["types"][j]) >= 0) {
                            val = removeDiacritics(place.address_components[k][typename]);
                            break;
                        }
                    }

                    // prendre le name pour la rue si aucune info n'est donnée par google
                    //if (val == "" && componentForm[i]["name"] === "route") {
                    //    val = place.name;
                    //}

                    if (val != "") {
                        if (componentForm[i]["name"] === "street_number") {
                            document.getElementById('formAddressRecherche').value += val + ' ';
                            var regexStr = val.match(/[a-zA-Z]+|[0-9]+/g);
                            document.getElementById('street_number').value = regexStr[0];
                            if (regexStr.length > 1 && /[a-zA-Z]+/g.test(regexStr[1])) {
                                document.getElementById('formExtension').value = regexStr[1].substr(0, 1).toUpperCase();
                            }
                        }
                        else if (componentForm[i]["name"] === "route") {
                            document.getElementById('formAddressRecherche').value += val + ' ';
                            document.getElementById(componentForm[i]["name"]).value = val;
                        }
                        else {
                            if (document.getElementById(componentForm[i]["name"]).value === '')
                                document.getElementById(componentForm[i]["name"]).value = val;
                        }
                        break;
                    }
                }
            }

            $("#formCPCedex").val($("#postal_code").val());
            if (!isNaN(streetNumber) && $('#street_number').val() == "") {
                document.getElementById('street_number').value = streetNumber;
            }

            //Specific behaviour for France DOM TOM
            if (paysFr.indexOf($("#country").val().toUpperCase()) > 0) {
                if ($("#locality").val() == "") document.getElementById('locality').value = $("#administrative_area_level_1").val();
                document.getElementById('administrative_area_level_1').value = $("#country").val();
                document.getElementById('country').value = "France";
            }

            //resetMap();
            selectedDescription = place.name;
            selectedLongitude = place.geometry.location.lng();
            $("#Longitude").val(selectedLongitude);
            selectedLatitude = place.geometry.location.lat();
            $("#Latitude").val(selectedLatitude);
            DisplayAdress(selectedLatitude, selectedLongitude, selectedDescription);
            $("#formValider").prop("disabled", false);
        });
    });
    
}
/*Map*/
function initMap() {
    // The location of Uluru
    var france = { lat: latFrance, lng: lonFrance };
    // The map, centered at Uluru
    map = new google.maps.Map(
        document.getElementById('map'), {
            zoom: 14,
            gestureHandling: 'greedy',
            minZoom: defaultMinZoom,
            maxZoom: defaultMaxZoom,
            center: france,
            mapTypeControl: true,
            mapTypeControlOptions: {
                style: google.maps.MapTypeControlStyle.DROPDOWN_MENU,
                mapTypeIds: ['roadmap', 'terrain', 'hybrid', 'satellite', 'OpenTopo', 'IGN', 'OSM']
            },
            mapTypeId: 'roadmap',
            disableDefaultUI: true,
            streetViewControl: true,
            streetViewControlOptions: {
                position: google.maps.ControlPosition.LEFT_TOP
            },
            zoomControl: true,
            zoomControlOptions: {
                position: google.maps.ControlPosition.LEFT_TOP
            },
            scaleControl: true,
            rotateControl: true,
            fullscreenControl: false
        });

    //ign
    map.mapTypes.set("IGN", new google.maps.ImageMapType({
        getTileUrl: function (coord, zoom) {

            return "https://wxs.ign.fr/choisirgeoportail/geoportail/wmts?REQUEST=GetTile&SERVICE=WMTS&VERSION=1.0.0&STYLE=bdparcellaire&TILEMATRIXSET=PM&FORMAT=image/png&LAYER=CADASTRALPARCELS.PARCELS&TILEMATRIX=" + zoom + "&TILEROW=" + coord.y + "&TILECOL=" + coord.x;
        },
        tileSize: new google.maps.Size(256, 256),
        name: "Cadastre",
        maxZoom: 17
    }));
    //osn
    map.mapTypes.set("OSM", new google.maps.ImageMapType({
        getTileUrl: function (coord, zoom) {

            return "http://tile.openstreetmap.org/" + zoom + "/" + coord.x + "/" + coord.y + ".png";
        },
        tileSize: new google.maps.Size(256, 256),
        name: "Openstreetmap",
        maxZoom: 18
    }));
    //topo
    map.mapTypes.set("OpenTopo", new google.maps.ImageMapType({
        getTileUrl: function (coord, zoom) {

            return "http://tile.opentopomap.org/" + zoom + "/" + coord.x + "/" + coord.y + ".png";
        },
        tileSize: new google.maps.Size(256, 256),
        name: "Topographie",
        maxZoom: 17
    }));
    //CATNAT
    map.mapTypes.set("IGN", new google.maps.ImageMapType({
        getTileUrl: function (coord, zoom) {

            return "https://wxs.ign.fr/choisirgeoportail/geoportail/wmts?REQUEST=GetTile&SERVICE=WMTS&VERSION=1.0.0&STYLE=bdparcellaire&TILEMATRIXSET=PM&FORMAT=image/png&LAYER=CADASTRALPARCELS.PARCELS&TILEMATRIX=" + zoom + "&TILEROW=" + coord.y + "&TILECOL=" + coord.x;
        },
        tileSize: new google.maps.Size(256, 256),
        name: "Cadastre",
        maxZoom: 17
    }));
}

function toggleOverlay(show, btn) {
    $("#legend").hide();
    if ($("select#gestionCalque").val().length > 0) {
        $("#legend").html(legendHtml + (show ? "<hr>" : ""));
    } else {
        $("#legend").html("");
    }
    map.overlayMapTypes.clear();
    $(".btn-layer").addClass("desactivate-option");
    if (show) {
        switch (btn) {
            case 'btnSismiciteOverlay':
                $("#legend").append("<img src='/Content/Images/sismicite_legende.png' style='max-width:250px;'>");
                $("#legend").show();
                map.overlayMapTypes.insertAt(0, new google.maps.ImageMapType({
                    getTileUrl: function (coord, zoom) {
                        return window.location.origin + "/Home/GetMapTiles?zoom=" + zoom + "&x=" + coord.x + "&y=" + coord.y + "&type=sismicite";;
                        //return "https://catastrophes-naturelles.ccr.fr/catnat-carto/contextProxy?url=/services/tilemetier2/88/" + zoom + "/" + coord.y + "/" + coord.x;
                    },
                    tileSize: new google.maps.Size(256, 256),
                    name: "Sismicite",
                    opacity: 0.6,
                    maxZoom: 18
                }));
                break;
            case 'btnInondationOverlay':
                $("#legend").append("<img src='/Content/Images/inondation_legende.png' style='max-width:250px;'>");
                $("#legend").show();
                map.overlayMapTypes.insertAt(0, new google.maps.ImageMapType({
                    getTileUrl: function (coord, zoom) {
                        return window.location.origin + "/Home/GetMapTiles?zoom=" + zoom + "&x=" + coord.x + "&y=" + coord.y + "&type=inondation";;
                        //return "https://catastrophes-naturelles.ccr.fr/catnat-carto/contextProxy?url=/services/tilemetier2/50/" + zoom + "/" + coord.y + "/" + coord.x;
                    },
                    tileSize: new google.maps.Size(256, 256),
                    name: "Inondation",
                    opacity: 0.6,
                    maxZoom: 14
                }));
                break;
            case 'btnSecheresseOverlay':
                $("#legend").append("<img src='/Content/Images/secheresse_legende.png' style='max-width:250px;'>");
                $("#legend").show();
                map.overlayMapTypes.insertAt(0, new google.maps.ImageMapType({
                    getTileUrl: function (coord, zoom) {
                        return window.location.origin + "/Home/GetMapTiles?zoom=" + zoom + "&x=" + coord.x + "&y=" + coord.y + "&type=secheresse";
                        //return "https://catastrophes-naturelles.ccr.fr/catnat-carto/contextProxy?url=/services/tilemetier2/75_76_77_78_79/" + zoom + "/" + coord.y + "/" + coord.x;
                    },
                    tileSize: new google.maps.Size(256, 256),
                    name: "Secheresse",
                    opacity: 0.6,
                    maxZoom: 18
                }));
                break;
            case 'btnZUSOverlay':
                $("#legend").append("<img src='/Content/Images/zus_legende.png' style='max-width:250px;'>");
                $("#legend").show();
                map.overlayMapTypes.insertAt(0, new google.maps.ImageMapType({
                    getTileUrl: function (coord, zoom) {
                        return "https://wxs.ign.fr/an7nvfzojv5wa96dsga5nk8w/geoportail/wmts?layer=AREAMANAGEMENT.ZUS&style=normal&tilematrixset=PM&Service=WMTS&Request=GetTile&Version=1.0.0&Format=image%2Fpng&TileMatrix=" + zoom + "&TileCol=" + coord.x + "&TileRow=" + coord.y;
                    },
                    tileSize: new google.maps.Size(256, 256),
                    name: "ZUS",
                    opacity: 0.6,
                    maxZoom: 18
                }));
                break;
            case 'btnTypeOverlay':
                $("#legend").append("<img src='/Content/Images/type_legende.png' style='max-width:250px;'>");
                $("#legend").show();
                map.overlayMapTypes.insertAt(0, new google.maps.ImageMapType({
                    getTileUrl: function (coord, zoom) {
                        return "https://wxs.ign.fr/an7nvfzojv5wa96dsga5nk8w/geoportail/wmts?layer=BUILDINGS.BUILDINGS&style=normal&tilematrixset=PM&Service=WMTS&Request=GetTile&Version=1.0.0&Format=image%2Fpng&TileMatrix=" + zoom + "&TileCol=" + coord.x + "&TileRow=" + coord.y;
                    },
                    tileSize: new google.maps.Size(256, 256),
                    name: "Type",
                    opacity: 0.6,
                    maxZoom: 18
                }));
                break;
        }
        $("#" + btn).removeClass("desactivate-option");
    }
}

/*Export MAP to PDF */
function exportMapTopdf() {
    html2canvas($("#map"), {
        useCORS: true,
        allowTaint: false,
        scale: 2,
        onrendered: function (canvas) {
            var img = canvas.toDataURL("image/png", 1);
            var pdf = new jsPDF("landscape");
            pdf.setProperties({
                title: 'Carte Atlas',
                author: 'SIG',
                keywords: 'carte',
                creator: 'SIG'
            });
            var height = $("#map").height();
            var width = $("#map").width();
            pdf.text("Carte Atlas", 15, 20);
            pdf.addImage(img, 'JPEG', 15, 40, 260, Math.round(260 * height / width));
            pdf.save('CarteSIG_' + getFormattedTime('') + '.pdf');
        }
    });
}

/*Show/Hide Loading Albingia*/
function ShowLoading(now) {
    $("#divLoading").show();
    if (!now) {
        ShowLoading.timeout = setTimeout(function () { $("#divLoading").addClass("shown") }, 250);
    }
    else {
        $("#divLoading").addClass("shown");
    }
}

function CloseLoading() {
    if (ShowLoading.timeout) {
        clearTimeout(ShowLoading.timeout);
        ShowLoading.timeout = undefined;
    }
    $("#divLoading").hide().removeClass("shown");
}

function initialize() {
    initMap();
    //initDataLayers();
    initSearchBox();
    //initDrawTools();
    initControls();
    initMapEvents();
}

/* Google Maps API 2 Handling END */