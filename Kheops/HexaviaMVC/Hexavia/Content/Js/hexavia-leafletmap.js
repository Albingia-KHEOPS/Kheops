var map = null;
var selectedMarker = null;
var circle = null;
var markerActesList = null;
var markerContractsList = null;
var markerOffersList = null;
var markerSinistresList = null;
var markerInsuredsList = null;
var markerBrokersList = null;
var markerExternalContractsList = null;
var placesAutocomplete;
var cityAutocomplete;
var extensions = ["B", "Bis", "T", "Ter", "a", "b", "c", "d", "e", "f"];
var selectedLongitude = null;
var selectedLatitude = null;
var selectedDescription = null;
var sidebar = null;
var latFrance = 46.603354;
var lonFrance = 1.8883335;
var selectedSearch = 1;
var adresseSearchNumber = 1;
var designationSearchNumber = 2;
var partnerSearchNumber = 3;
var loadContractshNumber = 4;
var urlExterieur = null;
var pastelBlueColor = "#D6EAF8";
var pastelGreyColor = "#CFCFC4";
var pastelGreenColor = "#D4EFDF";
var pastelPurpleColor = "#C39BD3";
var defaultSideBarColor = "#8B4513";
var pastelBlackColor = "#010101";
var pastelOrangeColor = "#FFB347";
var pastelYellowColor = "#FDFD96";
var pastelRedColor = "#FF6961";
var fileUploadExtension = ".xml";
// Initialisation drawnItems
var drawnItems = new L.FeatureGroup();
var blackIcon = new L.Icon({
    iconUrl: 'Content/Plugins/leaflet/images/marker-icon-2x-black.png',
    shadowUrl: 'Content/Plugins/leaflet/images/marker-shadow.png',
    iconSize: [25, 41], iconAnchor: [12, 41],
    popupAnchor: [1, -34], shadowSize: [41, 41]
});
var greenIcon = new L.Icon({
    iconUrl: 'Content/Plugins/leaflet/images/marker-icon-2x-green.png',
    shadowUrl: 'Content/Plugins/leaflet/images/marker-shadow.png',
    iconSize: [25, 41], iconAnchor: [12, 41],
    popupAnchor: [1, -34], shadowSize: [41, 41]
});
var greyIcon = new L.Icon({
    iconUrl: 'Content/Plugins/leaflet/images/marker-icon-2x-grey.png',
    shadowUrl: 'Content/Plugins/leaflet/images/marker-shadow.png',
    iconSize: [25, 41], iconAnchor: [12, 41],
    popupAnchor: [1, -34], shadowSize: [41, 41]
});
var orangeIcon = new L.Icon({
    iconUrl: 'Content/Plugins/leaflet/images/marker-icon-2x-orange.png',
    shadowUrl: 'Content/Plugins/leaflet/images/marker-shadow.png',
    iconSize: [25, 41], iconAnchor: [12, 41],
    popupAnchor: [1, -34], shadowSize: [41, 41]
});
var purpleIcon = new L.Icon({
    iconUrl: 'Content/Plugins/leaflet/images/marker-icon-2x-purple.png',
    shadowUrl: 'Content/Plugins/leaflet/images/marker-shadow.png',
    iconSize: [25, 41], iconAnchor: [12, 41],
    popupAnchor: [1, -34], shadowSize: [41, 41]
});
var redIcon = new L.Icon({
    iconUrl: 'Content/Plugins/leaflet/images/marker-icon-2x-red.png',
    shadowUrl: 'Content/Plugins/leaflet/images/marker-shadow.png',
    iconSize: [25, 41], iconAnchor: [12, 41],
    popupAnchor: [1, -34], shadowSize: [41, 41]
});
var yellowIcon = new L.Icon({
    iconUrl: 'Content/Plugins/leaflet/images/marker-icon-2x-yellow.png',
    shadowUrl: 'Content/Plugins/leaflet/images/marker-shadow.png',
    iconSize: [25, 41], iconAnchor: [12, 41],
    popupAnchor: [1, -34], shadowSize: [41, 41]
});
var blueIcon = new L.Icon({
    iconUrl: 'Content/Plugins/leaflet/images/marker-icon-2x.png',
    shadowUrl: 'Content/Plugins/leaflet/images/marker-shadow.png',
    iconSize: [25, 41], iconAnchor: [12, 41],
    popupAnchor: [1, -34], shadowSize: [41, 41]
});

var componentForm = {
    street_number: 'short_name',
    route: 'long_name',
    locality: 'long_name',
    administrative_area_level_1: 'short_name',
    country: 'long_name',
    postal_code: 'short_name'
};
// generate unique uid
function generateUUID() {
    var d = new Date().getTime();
    var d2 = (performance && performance.now && (performance.now() * 1000)) || 0;
    var uuid = 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'.replace(/[x]/g, function (c) {
        var r = Math.random() * 16;
        if (d > 0) {
            r = (d + r) % 16 | 0;
            d = Math.floor(d / 16);
        } else {
            r = (d2 + r) % 16 | 0;
            d2 = Math.floor(d2 / 16);
        }
        return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
    });
    return uuid;
}


$(document).ready(function () {

    sidebar = L.control.sidebar('sidebar', {
        closeButton: true,
        position: 'right'
    });
    $('#btnLoadContractsAround').prop('disabled', true);
    $('#btnLoadOffersAround').prop('disabled', true);
    $('#btnLoadSinistresAround').prop('disabled', true);
    $('#idEchelle').prop('disabled', true);
    $('#btnLoadInsuredsAround').prop('disabled', true);
    $('#btnLoadBrokersAround').prop('disabled', true);
    $('#externalContractsAroundUploadFile').prop('disabled', true);
    $('#btnUploadExternalContractsAround').prop('disabled', true);
    $('#btnCleanMarkers').prop('disabled', true);

    ConfigureSearchAdresseInput();
    ConfigureSearchCityInput();

    var adresseComplete = document.getElementById('AdresseComplete').value;
    if (adresseComplete === 'France') {
        // CreateMap(latFrance, lonFrance, 500)
        $.when(CreateMap(latFrance, lonFrance, 500))
            .done(
                function () {
                    getLayers();
                }
            );
    }
    else {
        RetrieveLatLonFromAdressAndDisplayAdress(adresseComplete, DisplayAdress);
    }
    ManageAdresseSearchBox("385px");

    urlExterieur = document.getElementById('UrlExterieur').value;
    if (!IsEmpty(urlExterieur)) {
        removeButton("btnDesignationSearch");
        removeButton("btnPartnerSearch");
        removeButton("btnLoadInsuredsAround");
        removeButton("btnLoadBrokersAround");
        removeButton("btnLegendAdresse");
    }
    else {
        $('#formBatiment').prop('disabled', true);
        $('#formDistribution').prop('disabled', true);
        $('#locality').prop('disabled', true);
        $('#postal_code').prop('disabled', true);
        $('#formVilleCedex').prop('disabled', true);
        $('#formCPCedex').prop('disabled', true);
        $('#administrative_area_level_1').prop('disabled', true);
        $('#country').prop('disabled', true);

        removeButton("formAnnuler");
        removeButton("formValider");
    }

    $('#designationSearch').keyup(function () {
        validateSearchByDesignation();
    });

    $('#codePartenaire').keyup(function () {
        validateSearchPartnerByCodePartenaire();
    });

    $('.searchPartenaireOtherFilters').keyup(function () {
        validateSearchPartnerByOtherFilters();
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

        validateSearchByDesignation();
    });

    $('#idEchelle').on('change', function (event) {
        var adresse = document.querySelector('#formAddressRecherche').value;
        var ville = document.querySelector('#locality').value;
        var codePostal = document.querySelector('#postal_code').value;
        var departement = document.querySelector('#administrative_area_level_1').value;
        var pays = document.querySelector('#country').value;
        var adresseComplete = adresse + " " + codePostal + " " + ville + " " + departement + " " + pays;

        DisplayAdress(selectedLatitude, selectedLongitude, adresseComplete);
    });

    $('#formAnnuler').on('click', function (event) {
        event.preventDefault();
        return window.close();
    });

    $('#formValider').on('click', function (event) {
        var adresseText = document.querySelector('#formAddressRecherche').value;
        var sep = '¤';
        var currentAdresse = getCurrentAdresse();

        $.ajax({
            type: 'POST',
            url: 'Home/SearchHexaviaMatricule',
            data: { adresseRecherchee: currentAdresse },
            dataType: 'json',
            success: function (matriculeHexavia) {

                let adresseValideText = 'NON_VALIDE';
                if (currentAdresse.AdresseText.length > 0) {
                    adresseValideText = "VALIDE";
                }

                var latitudeRetour = '';
                if (selectedLatitude !== null) {
                    latitudeRetour = selectedLatitude;
                }

                var longitudeRetour = '';
                if (selectedLongitude !== null) {
                    longitudeRetour = selectedLongitude;
                }

                var retourAdresse = currentAdresse.Batiment + sep + currentAdresse.NumeroVoie + sep + currentAdresse.ExtensionVoie + sep
                    + currentAdresse.NomVoie + sep + currentAdresse.BoitePostale + sep + currentAdresse.VilleCedex + sep + currentAdresse.CodePostalCedex + sep
                    + currentAdresse.Ville + sep + currentAdresse.CodePostal + sep + currentAdresse.Pays.Libelle + sep + adresseValideText + sep
                    + matriculeHexavia + sep + latitudeRetour + sep + longitudeRetour;

                let retourQuery = '?Data=adresse=' + retourAdresse;
                urlExterieur = document.getElementById('UrlExterieur').value;

                if (!IsEmpty(urlExterieur)) {
                    window.location = 'http://' + urlExterieur + retourQuery;
                }
            }
        });
    });


    $('#btnLoadOffersAround').click(function () {
        $("#btnLegendAdresse").text("Afficher légende");
        let $this = $(this);
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        if ($(this).html() !== loadingText) {
            $this.data('original-text', $(this).html());
            $this.html(loadingText);
        }

        let diametre = GetDiameter();

        if (loadAffaireNouvelleAround(selectedLongitude, selectedLatitude, diametre, 'O', greyIcon)) {
            $this.html($this.data('original-text'));
        }
    });

    $('#btnLoadContractsAround').click(function () {
        $("#btnLegendAdresse").text("Afficher légende");
        let $this = $(this);
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        if ($(this).html() !== loadingText) {
            $this.data('original-text', $(this).html());
            $this.html(loadingText);
        }

        let diametre = GetDiameter();

        if (loadAffaireNouvelleAround(selectedLongitude, selectedLatitude, diametre, 'P', blueIcon)) {
            $this.html($this.data('original-text'));
        }
    });

    $('#btnLoadSinistresAround').click(function () {
        $("#btnLegendAdresse").text("Afficher légende");
        let $this = $(this);
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        if ($(this).html() !== loadingText) {
            $this.data('original-text', $(this).html());
            $this.html(loadingText);
        }

        let diametre = GetDiameter();

        if (loadAffaireNouvelleAround(selectedLongitude, selectedLatitude, diametre, 'X', orangeIcon)) {
            $this.html($this.data('original-text'));
        }
    });

    $('#btnUploadExternalContractsAround').click(function () {
        $("#btnLegendAdresse").text("Afficher légende");
        let $this = $(this);
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        if ($(this).html() !== loadingText) {
            $this.data('original-text', $(this).html());
            $this.html(loadingText);
        }

        var isMarkerDisplayedByBranche = false;//$('#optMarqueur').is(':checked');


        var iconMarker = blackIcon;


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
                        markerExternalContractsList = [];
                        for (i = 0; i < listAffaires.length; i++) {
                            if (isMarkerDisplayedByBranche) {
                                iconMarker = selectCorrectMarkerIconByBranche(listAffaires[i].Branche);
                            }

                            marker = [
                                listAffaires[i].NumeroChrono,
                                L.marker([listAffaires[i].Lat, listAffaires[i].Lon], { icon: iconMarker }),
                                listAffaires[i].Libelle,
                                listAffaires[i].FullLibelle
                            ];
                            markerExternalContractsList.push(marker);
                        }

                        let adresse = document.querySelector('#formAddressRecherche').value;
                        DisplayAdress(selectedLatitude, selectedLongitude, adresse);
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
    });

    $('#btnLoadInsuredsAround').click(function () {
        $("#btnLegendAdresse").text("Afficher légende");
        let $this = $(this);
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        if ($(this).html() !== loadingText) {
            $this.data('original-text', $(this).html());
            $this.html(loadingText);
        }

        let diametre = GetDiameter();
        $.ajax({
            type: 'POST',
            url: 'Partner/LoadInsuredsAroundGPSPoint',
            data: { longitude: selectedLongitude, latitude: selectedLatitude, diametre: diametre },
            dataType: 'json',
            success: function (listInsureds) {
                markerInsuredsList = [];
                for (i = 0; i < listInsureds.length; i++) {
                    marker = [
                        listInsureds[i].NumeroChrono,
                        L.marker([listInsureds[i].Lat, listInsureds[i].Lon], { icon: purpleIcon }),
                        listInsureds[i].Libelle,
                        listInsureds[i].FullLibelle
                    ];
                    markerInsuredsList.push(marker);
                }

                let adresse = document.querySelector('#formAddressRecherche').value;
                DisplayAdress(selectedLatitude, selectedLongitude, adresse);
                $this.html($this.data('original-text'));
            }
        });
    });

    $('#btnLoadBrokersAround').click(function () {
        $("#btnLegendAdresse").text("Afficher légende");
        let $this = $(this);
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        if ($(this).html() !== loadingText) {
            $this.data('original-text', $(this).html());
            $this.html(loadingText);
        }

        let diametre = GetDiameter();
        $.ajax({
            type: 'POST',
            url: 'Partner/LoadBrokersAroundGPSPoint',
            data: { longitude: selectedLongitude, latitude: selectedLatitude, diametre: diametre },
            dataType: 'json',
            success: function (listBrokers) {
                markerBrokersList = [];
                for (i = 0; i < listBrokers.length; i++) {
                    marker = [
                        listBrokers[i].NumeroChrono,
                        L.marker([listBrokers[i].Lat, listBrokers[i].Lon], { icon: greenIcon }),
                        listBrokers[i].Libelle,
                        listBrokers[i].FullLibelle
                    ];
                    markerBrokersList.push(marker);
                }

                let adresse = document.querySelector('#formAddressRecherche').value;
                DisplayAdress(selectedLatitude, selectedLongitude, adresse);
                $this.html($this.data('original-text'));
            }
        });
    });


    $('#btnCleanMarkers').click(function () {
        $("#btnLegendAdresse").text("Afficher légende");
        CleanMarkersExceptSelected();
    });


    $('.btnLegend').click(function () {
        var divLegend = document.getElementById("legend").style.display;
        if (divLegend === "block") {
            document.getElementById("legend").style.display = "none";
            $(this)[0].innerHTML = "Afficher légende";
        }
        else {
            document.getElementById("legend").style.display = "block";
            $(this)[0].innerHTML = "Cacher légende";
            sidebar.hide();
        }
    });


    $('#btnSearchByDesignation').click(function () {
        $("#btnLegendDesignation").text("Afficher légende");
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

        var isMarkerDisplayedByBranche = false;//$('#optMarqueur').is(':checked');

        var displayMode = "BrancheDesignation";
        //if (isMarkerDisplayedByBranche) {
        //    displayMode = "BrancheMarker";
        //}


        var designation = $('#designationSearch').val();
        var etat = transformSelectionToString($('#cbbEtat').val());
        var situation = transformSelectionToString($('#cbbSituation').val());
        var branche = transformSelectionToString($('#cbbBrancheSearchDesignation').val());
        var garantie = transformSelectionToString($("#cbbGarantie").val());
        var iconMarker = blueIcon;



        $.ajax({
            type: 'POST',
            url: 'OfferContract/LoadAffairesByDesignation',
            data: { designation: designation, typeDesignation: type, etat: etat, situation: situation, branche: branche, garantie: garantie, mode: displayMode },
            dataType: 'json',
            success: function (data) {
                let listAffaires = data.listAffaires;
                let zoomMax = data.zoomMax;
                markerActesList = [];
                for (i = 0; i < listAffaires.length; i++) {
                    if (isMarkerDisplayedByBranche) {
                        iconMarker = selectCorrectMarkerIconByBranche(listAffaires[i].Branche);
                    }

                    marker = [
                        listAffaires[i].NumeroChrono,
                        L.marker([listAffaires[i].Lat, listAffaires[i].Lon], { icon: iconMarker }),
                        listAffaires[i].Libelle,
                        listAffaires[i].FullLibelle
                    ];
                    markerActesList.push(marker);
                }
                if (isMarkerDisplayedByBranche) {
                    DisplayDesignationAdress(zoomMax, pastelBlueColor);
                }
                else {
                    DisplayDesignationAdress(zoomMax, defaultSideBarColor);
                }
                $this.html($this.data('original-text'));
            }
        });
    });


    $('#btnRechercherPartenaires').click(function () {
        $("#btnLegendPartenaire").text("Afficher légende");
        let $this = $(this);
        let loadingText = '<i class="fa fa-circle-o-notch fa-spin"></i>Loading...';
        if ($(this).html() !== loadingText) {
            $this.data('original-text', $(this).html());
            $this.html(loadingText);
        }

        CleanMarkersExceptSelected();

        let type = 0;
        let colorPartner = pastelPurpleColor;
        let icon = purpleIcon;
        if (document.getElementById('optCourtiers').checked) {
            type = 1;
            colorPartner = pastelGreenColor;
            icon = greenIcon;
        }

        let codePartenaire = document.getElementById('codePartenaire').value;
        let nomPartenaire = document.getElementById('nomPartenaire').value;
        let depPartenaire = document.getElementById('depPartenaire').value;
        let cpPartenaire = document.getElementById('cpPartenaire').value;
        let villePartenaire = document.getElementById('villePartenaire').value;

        $.ajax({
            type: 'POST',
            url: 'Partner/SearchPartners',
            data: {
                partnerType: type,
                partnerCod: codePartenaire,
                partnerName: nomPartenaire,
                partnerDept: depPartenaire,
                partnerCP: cpPartenaire,
                partnerTown: villePartenaire
            },
            dataType: 'json',
            success: function (data) {
                let listPartners = data.listPartners;
                let zoomMax = data.zoomMax;
                markerActesList = [];
                if (listPartners.length === 1) {
                    selectedLongitude = listPartners[0].Lon;
                    selectedLatitude = listPartners[0].Lat;
                    selectedDescription = listPartners[0].Libelle;

                    markerActesList.push([
                        listPartners[0].NumeroChrono,
                        L.marker([listPartners[0].Lat,
                        listPartners[0].Lon], { icon: redIcon }),
                        listPartners[0].Libelle,
                        listPartners[0].FullLibelle
                    ]);

                    defaultSideBarColor = colorPartner;
                    DisplayAdress(selectedLatitude, selectedLongitude, listPartners[0].Libelle);
                    document.getElementById("divOptions").style.display = "block";
                }
                else {
                    for (i = 0; i < listPartners.length; i++) {
                        markerActesList.push([
                            listPartners[i].NumeroChrono,
                            L.marker([listPartners[i].Lat,
                            listPartners[i].Lon], { icon: icon }),
                            listPartners[i].Libelle,
                            listPartners[i].FullLibelle
                        ]);
                    }
                    DisplayDesignationAdress(zoomMax, colorPartner);
                    document.getElementById("divOptions").style.display = "none";
                }

                $this.html($this.data('original-text'));
            }
        });
    });

    $('#btnNettoyerPartenaire').click(function () {
        $("#btnLegendPartenaire").text("Afficher légende");
        NettoyerFormEtMap();
    });

    $('#btnNettoyerDesignation').click(function () {
        $("#btnLegendDesignation").text("Afficher légende");
        NettoyerFormEtMap();
    });

    $('#btnNettoyerAdresse').on('click', function (event) {
        $("#btnLegendAdresse").text("Afficher légende");
        event.preventDefault();
        NettoyerFormEtMap();
    });

    $('.cbbSearchByDesignation').change(function () {
        validateSearchByDesignation();
    });



    $('#adresseSearchBox').on('shown.bs.collapse', function () {
        if (IsEmpty(urlExterieur)) {
            document.getElementById("divOptions").style.display = "block";
        }
        ManageAdresseSearchBox("385px");
    });

    $('#adresseSearchBox').on('hidden.bs.collapse', function () {
        if (IsEmpty(urlExterieur)) {
            document.getElementById("divOptions").style.display = "block";
        }
        ManageAdresseSearchBox("630px");
    });

    $('#designationSearchBox').on('shown.bs.collapse', function () {
        ManageDesignationSearchBox("615px");
    });

    $('#designationSearchBox').on('hidden.bs.collapse', function () {
        ManageDesignationSearchBox("660px");
    });

    $('#partnerSearchBox').on('shown.bs.collapse', function () {
        ManagePartnerSearchBox("490px");
    });

    $('#partnerSearchBox').on('hidden.bs.collapse', function () {
        ManagePartnerSearchBox("660px");
    });

    $('#loadContractsBox').on('shown.bs.collapse', function () {
        ManageLoadContractsBox("490px");
    });

    $('#loadContractsBox').on('hidden.bs.collapse', function () {
        ManageLoadContractsBox("660px");
    });

    //// Region Hide new features START
    //$("#divFilterEtatSituation").css({ 'display': 'none' });
    //$("#divFilterBrancheSearchDesignation").css({ 'display': 'none' });
    //$("#divFilterGarantie").css({ 'display': 'none' }); 
    //$("#btnLoadSinistresAround").css({ 'display': 'none' });
    //$("#optSinistres").css({ 'display': 'none' });
    //$("#lblSinistres").css({ 'display': 'none' });
    //// Region Hide new features END

    $("input[type=file]").change(function () {
        var fieldVal = $(this).val();
        if (fieldVal != undefined || fieldVal != "") {
            $(this).next(".custom-file-label").text(fieldVal);
        }

        validateLoadContracts();
    });
});

function ManageAdresseSearchBox(height) {
    if (selectedSearch !== adresseSearchNumber) {
        NettoyerFormEtMap();
        selectedSearch = adresseSearchNumber;
    }
    changeHeight(height);
    if (map !== null) {
        map.invalidateSize();
    }
    document.getElementById("divOptions").style.display = "block";
    $("#btnAdresseSearch").removeClass("btn-light").addClass("btn-primary");
    $("#btnDesignationSearch").removeClass("btn-primary").addClass("btn-light");
    $("#btnPartnerSearch").removeClass("btn-primary").addClass("btn-light");
}

function ManageDesignationSearchBox(height) {
    if (selectedSearch !== designationSearchNumber) {
        NettoyerFormEtMap();
        selectedSearch = designationSearchNumber;
    }
    changeHeight(height);
    map.invalidateSize();
    document.getElementById("divOptions").style.display = "none";
    $("#btnDesignationSearch").removeClass("btn-light").addClass("btn-primary");
    $("#btnPartnerSearch").removeClass("btn-primary").addClass("btn-light");
    $("#btnAdresseSearch").removeClass("btn-primary").addClass("btn-light");
}

function ManagePartnerSearchBox(height) {
    if (selectedSearch !== partnerSearchNumber) {
        NettoyerFormEtMap();
        selectedSearch = partnerSearchNumber;
    }
    else {
        if (selectedLatitude !== null) {
            document.getElementById("divOptions").style.display = "block";
        } else {
            document.getElementById("divOptions").style.display = "none";
        }
    }
    changeHeight(height);
    map.invalidateSize();

    $("#btnRechercherPartenaires").prop('disabled', false);
    document.getElementById("divOptions").style.display = "none";
    $("#btnPartnerSearch").removeClass("btn-light").addClass("btn-primary");
    $("#btnDesignationSearch").removeClass("btn-primary").addClass("btn-light");
    $("#btnAdresseSearch").removeClass("btn-primary").addClass("btn-light");
}

function ManageLoadContractsBox(height) {
    if (selectedSearch !== loadContractshNumber) {
        NettoyerFormEtMap();
        selectedSearch = loadContractshNumber;
    }
    changeHeight(height);
    map.invalidateSize();
    document.getElementById("divOptions").style.display = "none";
    $("#btnDesignationSearch").removeClass("btn-primary").addClass("btn-light");
    $("#btnPartnerSearch").removeClass("btn-primary").addClass("btn-light");
    $("#btnAdresseSearch").removeClass("btn-primary").addClass("btn-light");
}

function ConfigureSearchAdresseInput() {
    //placesAutocomplete = places({
    //    container: document.querySelector('#formAddressRecherche'),
    //    templates: {
    //        value: function (suggestion) {
    //            return suggestion.name;
    //        }
    //    }
    //}).configure({
    //    type: 'address'
    //});

    //placesAutocomplete.on('change', function resultSelected(e) {
    //    document.querySelector('#locality').value = e.suggestion.city || '';
    //    document.querySelector('#formVilleCedex').value = '';
    //    document.querySelector('#postal_code').value = e.suggestion.postcode || '';
    //    document.querySelector('#formCPCedex').value = '';
    //    document.querySelector('#country').value = e.suggestion.country || '';

    //    var adresse = document.querySelector('#formAddressRecherche').value + " " + e.suggestion.postcode + " " + e.suggestion.city + " " + e.suggestion.country;
    //    DisplayAdress(e.suggestion.latlng.lat, e.suggestion.latlng.lng, adresse);
    //    selectedLongitude = e.suggestion.latlng.lng;
    //    selectedLatitude = e.suggestion.latlng.lat;
    //    selectedDescription = adresse;

    //    $('#btnLoadContractsAround').prop('disabled', false); 
    //    $('#btnLoadOffersAround').prop('disabled', false);
    //    $('#btnLoadSinistresAround').prop('disabled', false);
    //    $('#idEchelle').prop('disabled', false);
    //    $('#btnLoadInsuredsAround').prop('disabled', false);
    //    $('#btnLoadBrokersAround').prop('disabled', false);
    //    $('#btnCleanMarkers').prop('disabled', false);
    //});
}

function ConfigureSearchCityInput() {
    cityAutocomplete = places({
        container: document.querySelector('#villePartenaire'),
        templates: {
            value: function (suggestion) {
                return suggestion.name;
            }
        }
    }).configure({
        type: 'city',
        language: 'fr',
        aroundLatLngViaIP: true
    });
}

function RetrieveLatLonFromAdressAndDisplayAdress(adresseComplete, DisplayAdress) {
    var adresseCompleteSansAccent = skipAccent(adresseComplete);
    var xhttp = new XMLHttpRequest();
    xhttp.open("POST", "https://nominatim.openstreetmap.org/search?q=" + adresseCompleteSansAccent + "&format=json", true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send();

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            if (this.responseText.length <= 2) {
                if (map != undefined) {
                    map.eachLayer(function (layer) {
                        map.removeLayer(layer);
                    });
                }
            } else {
                var obj = JSON.parse(this.responseText);
                selectedLongitude = obj[0].lon;
                selectedLatitude = obj[0].lat;
                selectedDescription = adresseComplete;
                DisplayAdress(selectedLatitude, selectedLongitude, adresseComplete);
            }
        }
    };
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

function DisplayDesignationAdress(zoomMax, sideBarColor) {
    CreateMapObject();
    let zoom = 6;
    if (markerActesList !== null) {
        if (markerActesList.length > 0) {
            let firstMarkerPosition = markerActesList[0][1].getLatLng();
            if (zoomMax) {
                zoom = 1;
            }
        }

        map.setView([latFrance, lonFrance], zoom);

        let i;
        for (i = 0; i < markerActesList.length; i++) {
            let market = markerActesList[i][1];
            let mposition = market.getLatLng();
            let markerFullLib = markerActesList[i][3];
            market.on('click', function (e) {
                markerOnClick(e, markerFullLib, sideBarColor);
            });
            market.addTo(map);
            market.bindTooltip(markerActesList[i][2]);
        }
    }
}

function DisplayAdress(lat, lon, description) {

    var diameter = GetDiameter();
    if (IsEmpty(diameter)) {
        diameter = 1;
    }

    //on centre sur la france si aucune adresse n'est selectionné
    if (lat === null && lon === null) {
        diameter = 700;
        lat = latFrance;
        lon = lonFrance;
    }

    //CreateMap(lat, lon, diameter);
    $.when(CreateMap(lat, lon, diameter))
        .done(
            function () {
                getLayers();
            }
        );

    //Creating the circle
    var circle = L.circle([lat, lon], diameter * 1000);
    circle.addTo(map);

    placeMarkersIntoMap(circle, markerActesList, pastelBlueColor);
    placeMarkersIntoMap(circle, markerContractsList, pastelBlueColor);
    placeMarkersIntoMap(circle, markerOffersList, pastelGreyColor);
    placeMarkersIntoMap(circle, markerExternalContractsList, defaultSideBarColor);
    placeMarkersIntoMap(circle, markerSinistresList, pastelOrangeColor);
    placeMarkersIntoMap(circle, markerInsuredsList, pastelPurpleColor);
    placeMarkersIntoMap(circle, markerBrokersList, pastelGreenColor);

    selectedMarker = L.marker([lat, lon], { icon: redIcon });
    selectedMarker.addTo(map);
    if (IsEmpty(description)) {
        description = selectedDescription;
    }
    selectedMarker.on('click', function (e) {
        markerOnClick(e, description, defaultSideBarColor);
    });
    selectedMarker.bindTooltip("<b>" + description + "</b>", { clickable: true });

    $('#btnLoadContractsAround').prop('disabled', false);
    $('#btnLoadOffersAround').prop('disabled', false);
    $('#btnLoadSinistresAround').prop('disabled', false);
    $('#idEchelle').prop('disabled', false);
    $('#btnLoadInsuredsAround').prop('disabled', false);
    $('#btnLoadBrokersAround').prop('disabled', false);
    $('#externalContractsAroundUploadFile').prop('disabled', false);
    $('#btnCleanMarkers').prop('disabled', false);
}

function markerOnClick(e, fullLibelle, color) {
    sidebar.setContent(fullLibelle);
    if (color !== null) {
        $("#sidebar").css("background-color", color);
    }
    sidebar.show();
    $("#legend").hide();
    $("#btnLegendAdresse").text("Afficher légende");
    $("#btnLegendDesignation").text("Afficher légende");
    $("#btnLegendPartenaire").text("Afficher légende");
}

function CreateMap(lat, lon, diameter) {
    CreateMapObject();
    var zoom = GetZoom(diameter);
    map.setView(new L.LatLng(lat, lon), zoom);
}

/* Layer mamanagement */

/* Get all layers */
function getLayers() {

    $.ajax({
        type: "GET",
        url: "Layer/GetAll",
        datatype: "json",
        success: function (result) {
            if (result) {
                var data = {
                    type: "FeatureCollection",
                    features: []
                };
                $.each(result, function (key, feature) {
                    if (feature.Shape) {
                        data.features.push(JSON.parse(feature.Shape));
                    }

                });

                // add geojson
                var geojsonLayer = L.geoJSON(data, {
                    style: function (feature) {
                        return {
                            color: feature.properties.color,
                        };
                    },
                    pointToLayer: function (feature, latlng) {
                        //spec of circle
                        if (feature.properties.radius) {
                            return new L.Circle(latlng, feature.properties.radius);
                        }
                        return;
                    },
                    // bind tooltip
                    onEachFeature: function (feature, layer) {

                        layer.bindTooltip(feature.properties.name, { permanent: true, direction: 'centre', className: 'leaflet-layer-name' });

                    }
                }).addTo(map);
                geojsonLayer.eachLayer(
                    function (layer) {
                        drawnItems.addLayer(layer);
                    });
            }

        }
    });
}

/* saveLayer : Save layer in db */
function saveLayer(layer, fncallback) {
    var data = { id: layer.feature.properties.id, name: layer.feature.properties.name, shape: JSON.stringify(layer.toGeoJSON()) };
    $.ajax({
        type: "POST",
        url: "Layer/Save",
        data: { layer: data },
        datatype: "json",
        success: function (result) {

            fncallback();
        }
    });
}
function confirmAndSaveLayer(layer) {

    var feature = layer.feature = layer.feature || {};
    feature.type = feature.type || "Feature";
    var props = feature.properties = feature.properties || {};
    props.name = null;
    props.id = null;
    props.color = null;
    var title = 'Nom de calque';
    var message = 'Le nom de calque ne doit pas être vide';
    swal.fire({
        title: title,
        input: 'text',
        inputAttributes: {
            autocapitalize: 'off',
            maxlength: '20'
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
        preConfirm: function (name) {
            return existLayer(name);
        },
        allowOutsideClick: function () { !Swal.isLoading(); }
    }).then(function (result) {
        if (result.value) {
            Swal.showValidationMessage("Error");
            layer.feature.properties.name = result.value;
            layer.feature.properties.id = generateUUID();
            layer.feature.properties.color = layer.options.color;

            //save radius for circle shape
            if (layer instanceof L.Circle) {
                layer.feature.properties.radius = layer.getRadius();
            }

            saveLayer(layer,
                function () {

                    swal.fire(
                        'Enregistrement',
                        'le calque sélectionné a été enregistré',
                        'success'
                    );
                }
            );
            drawnItems.addLayer(layer);
            layer.bindTooltip(layer.feature.properties.name, { permanent: true, direction: 'centre', className: 'leaflet-layer-name' });

        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swal.fire(
                'Annulation',
                'le calque sélectionné a été supprimé',
                'error'
            );
        }
    });
}

/* existLayer : layer name verification */
function existLayer(name) {

    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: "Layer/Exist",
            data: { name: name },
            success: function (result) {

                if (result) {
                    Swal.showValidationMessage("le nom du calque existe déjà");
                    resolve(null);
                } else {
                    resolve(name);
                }
            }
        });

    });

}

/* updateLayers : Update multi-layers */
function updateLayers(layers, fncallback) {
    var data = [];
    $.each(layers, function (key, layer) {

        data.push({ id: layer.feature.properties.id, name: layer.feature.properties.name, shape: JSON.stringify(layer.toGeoJSON()), });
    });
    $.ajax({
        type: "POST",
        url: "Layer/Updates",
        data: { layers: data },
        success: function (result) {
            fncallback();
        }
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
        url: "Layer/Deletes",
        data: { ids: data },
        success: function (result) {
            fncallback();
        }
    });
}
/* drawLayers : draw shapes in th mape*/
function drawLayers() {
    //Draw custom layer
    //var drawnItems = new L.FeatureGroup();
    map.addLayer(drawnItems);
    drawFrLocales();
    options = {
        position: 'topright',
        draw: {
            marker: false,
            circlemarker: false,
            polyline: {
                shapeOptions: {
                    color: '#f357a1',
                    weight: 2
                }
            },
            rectangle: {
                shapeOptions: {
                    color: '#0000FF'
                }
            },
            polygon: {
                allowIntersection: false,
                drawError: {
                    color: '#e1e100',
                    message: 'Forme <strong>incorrecte<strong>'
                },
                shapeOptions: {
                    color: '#880E4F'
                }
            }

        },
        shapeOptions: {
            showArea: true,
            clickable: true
        },
        metric: true,
        edit: {
            featureGroup: drawnItems,
            edit: {
                selectedPathOptions: {
                    color: '#228B22',
                    fillColor: '#7CFC00'
                }
            }
        }
    };
    var drawControl = new L.Control.Draw(options);
    map.addControl(drawControl);
    //Draw created event hundler
    map.on('draw:created', function (e) {
        var layer = e.layer;
        confirmAndSaveLayer(layer);
    });
    //Draw edited event hundler
    map.on('draw:edited', function (evt) {
        var layers = evt.layers.getLayers();
        updateLayers(layers, function() { });

    });
    //Draw deleted event hundler
    map.on('draw:deleted', function (evt) {
        var layers = evt.layers.getLayers();
        deleteLayers(layers);
    });
}

function CreateMapObject() {
    //remove map if already exists
    if (map != undefined) {
        map.remove();
    }

    let openstreetmapLayer = new L.TileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png');

    let googleStreets = L.tileLayer('http://{s}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}', {
        maxZoom: 20,
        subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
    });

    let googleHybrid = L.tileLayer('http://{s}.google.com/vt/lyrs=s,h&x={x}&y={y}&z={z}', {
        maxZoom: 20,
        subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
    });

    let googleSat = L.tileLayer('http://{s}.google.com/vt/lyrs=s&x={x}&y={y}&z={z}', {
        maxZoom: 20,
        subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
    });

    let OpenTopoMap = L.tileLayer('https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png', { maxZoom: 17 });

    let GeoportailFrance_parcels = L.tileLayer('https://wxs.ign.fr/{apikey}/geoportail/wmts?REQUEST=GetTile&SERVICE=WMTS&VERSION=1.0.0&STYLE={style}&TILEMATRIXSET=PM&FORMAT={format}&LAYER=CADASTRALPARCELS.PARCELS&TILEMATRIX={z}&TILEROW={y}&TILECOL={x}', {
        bounds: [[-75, -180], [81, 180]],
        minZoom: 2,
        maxZoom: 20,
        apikey: 'choisirgeoportail',
        format: 'image/png',
        style: 'bdparcellaire'
    });

    var baseMap = {
        "Rue": googleStreets,
        "Satellite": googleSat,
        "Hybride": googleHybrid,
        "Topographie": OpenTopoMap,
        "Cadastre": GeoportailFrance_parcels,
        "openstreetmap": openstreetmapLayer
    };

    map = new L.Map('map', { layers: [googleStreets] });
    map.addControl(sidebar);
    map.on('click', function () {
        sidebar.hide();
    });

    L.control.layers(baseMap, null, { collapsed: true, position: 'topleft' }).addTo(map);
    L.control.scale().addTo(map);

    /*Legend specific*/
    var legend = L.control({ position: "topright" });

    legend.onAdd = function (map) {
        var div = L.DomUtil.create("div", "legend");
        div.innerHTML += "<h4>Légende</h4>";
        div.innerHTML += '<i style="background: #D6EAF8"></i><span>CO</span><br>';
        div.innerHTML += '<i style="background: #CFCFC4"></i><span>IA</span><br>';
        div.innerHTML += '<i style="background: #D4EFDF"></i><span>MR</span><br>';
        div.innerHTML += '<i style="background: #C39BD3"></i><span>RC</span><br>';
        div.innerHTML += '<i style="background: #FFB347"></i><span>RS</span><br>';
        div.innerHTML += '<i style="background: #FDFD96"></i><span>RT</span><br>';
        div.innerHTML += '<i style="background: #FF6961"></i><span>TR</span><br>';

        div.id = "legend";
        div.style.display = "none";

        return div;
    };

    legend.addTo(map);

    drawLayers();
}

function GetZoom(diameter) {
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
    if (diameter <= 50) {
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
    var adresseText = document.querySelector('#formAddressRecherche').value;
    var splitAdresse = splitAddress(adresseText);

    var distribution = document.querySelector('#formDistribution').value.toUpperCase();
    var batiment = document.querySelector('#formBatiment').value.toUpperCase();
    var cp = document.querySelector('#postal_code').value.toUpperCase();
    var cpCedex = document.querySelector('#formCPCedex').value.toUpperCase();
    var ville = document.querySelector('#locality').value.toUpperCase();
    var villeCedex = document.querySelector('#formVilleCedex').value.toUpperCase();
    var departement = document.querySelector('#administrative_area_level_1').value.toUpperCase();
    var pays = document.querySelector('#country').value.toUpperCase();
    var matriculeHexavia = document.getElementById('MatriculeHexavia').value;

    var adresse = {
        AdresseText: adresseText,
        Batiment: batiment,
        BoitePostale: distribution,
        NomVoie: splitAdresse.adresse,
        NumeroVoie: splitAdresse.number,
        ExtensionVoie: splitAdresse.extension,
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
        diameter = 1;
    }
    return diameter;
}

function NettoyerFormEtMap(NotToCleanGoogleAreaSearch) {
    document.querySelector('#externalContractsAroundUploadFile').value = "";
    document.querySelector('#locality').value = "";
    document.querySelector('#postal_code').value = "";
    document.querySelector('#country').value = "";
    document.querySelector('#administrative_area_level_1').value = "";
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
    document.querySelector('#idEchelle').value = "1";




    $('#optAssures').prop('checked', true);
    //$('#optMarqueur').prop('checked', true);

    $("#cbbEtat").val([]);
    $("#cbbGarantie").val([]);
    $("#optContrats").prop('checked', true);
    $("#optOffres").prop('checked', true);
    $("#optSinistres").prop('checked', false);
    $("#btnSearchByDesignation").prop('disabled', true);


    $('#btnLoadContractsAround').prop('disabled', true);
    $('#btnLoadOffersAround').prop('disabled', true);
    $('#btnLoadSinistresAround').prop('disabled', true);
    $('#idEchelle').prop('disabled', true);
    $('#btnLoadInsuredsAround').prop('disabled', true);
    $('#btnLoadBrokersAround').prop('disabled', true);
    $('#externalContractsAroundUploadFile').prop('disabled', true);
    $('#btnUploadExternalContractsAround').prop('disabled', true);
    $('#btnCleanMarkers').prop('disabled', true);

    $('.searchPartenaireOtherFilters').each(function () {
        $(this)[0].disabled = false;
        $(this)[0].value = "";
    });

    if ((map !== undefined) && (map !== null)) {
        map.eachLayer(function (layer) {
            map.removeLayer(layer);
        });

        markerActesList = null;
        markerContractsList = null;
        markerOffersList = null;
        markerSinistresList = null;
        markerInsuredsList = null;
        markerBrokersList = null;

        CreateMapObject();
        map.setView([latFrance, lonFrance], 6);
        sidebar.hide();
    }
}

function removeButton(idBtn) {
    var elem = document.getElementById(idBtn);
    elem.parentNode.removeChild(elem);
}

function CleanMarkersExceptSelected() {
    markerActesList = null;
    markerContractsList = null;
    markerOffersList = null;
    markerSinistresList = null;
    markerInsuredsList = null;
    markerBrokersList = null;
    sidebar.hide();
    if (selectedLatitude !== null && selectedLongitude !== null) {
        let adresse = document.querySelector('#formAddressRecherche').value;
        DisplayAdress(selectedLatitude, selectedLongitude, adresse);
    }
}

function loadAffaireNouvelleAround(selectedLongitude, selectedLatitude, diametre, type, iconMarker) {
    var res = false;
    var branche = "";
    $.ajax({
        type: 'POST',
        url: 'OfferContract/LoadAffairesAroundGPSPoint',
        data: { longitude: selectedLongitude, latitude: selectedLatitude, diametre: diametre, typeDesignation: type, branche: branche },
        dataType: 'json',
        async: false,
        success: function (listAffaires) {
            if (type === "P") {
                markerContractsList = [];
                markerSinistresList = [];
            }
            else if (type === "O") {
                markerOffersList = [];
                markerSinistresList = [];
            }
            else if (type === "X") {
                markerSinistresList = [];
                markerOffersList = [];
                markerContractsList = [];
            }

            for (i = 0; i < listAffaires.length; i++) {
                marker = [
                    listAffaires[i].NumeroChrono,
                    L.marker([listAffaires[i].Lat, listAffaires[i].Lon], { icon: iconMarker }),
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
            res = true;
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
            icon = blackIcon;
            break;
        case 'MR':
            icon = blueIcon;
            break;
        case 'RC':
            icon = greenIcon;
            break;
        case 'RS':
            icon = greyIcon;
            break;
        case 'RT':
            icon = orangeIcon;
            break;
        case 'TR':
            icon = purpleIcon;
            break;
        default:
            break;
    }

    return icon;
}

function validateSearchByDesignation() {
    let designationSearch = document.querySelector('#designationSearch').value;
    let etat = document.querySelector('#cbbEtat').value;
    let garantie = document.querySelector('#cbbGarantie').value;
    let situation = document.querySelector('#cbbSituation').value;
    let branche = document.querySelector('#cbbBrancheSearchDesignation').value;

    if (IsEmpty(designationSearch) && IsEmpty(etat) && IsEmpty(situation) && IsEmpty(branche) && IsEmpty(garantie) || (!$("#optContrats").prop('checked') && !$("#optOffres").prop('checked') && !$("#optSinistres").prop('checked'))) {
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

function validateSearchPartnerByCodePartenaire() {
    if ($("#codePartenaire").val() !== "") {
        $('.searchPartenaireOtherFilters').each(function () {
            $(this)[0].disabled = true;
        });
    }
    else {
        $('.searchPartenaireOtherFilters').each(function () {
            $(this)[0].disabled = false;
        });
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

function placeMarkersIntoMap(circle, markerList, color) {
    var cposition = circle.getLatLng();
    var cradius = circle.getRadius();

    if (markerList !== null) {
        let i;
        for (i = 0; i < markerList.length; i++) {
            let market = markerList[i][1];
            let mposition = market.getLatLng();

            if (cposition.distanceTo(mposition) <= cradius) {
                let markerFullLib = markerList[i][3];
                market.on('click', function (e) {
                    markerOnClick(e, markerFullLib, color);
                });
                market.addTo(map);
                market.bindTooltip(markerList[i][2]);
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


/* Google Maps API Handling START */

// This example adds a search box to a map, using the Google Place Autocomplete
// feature. People can enter geographical searches. The search box will return a
// pick list containing a mix of places and predicted search terms.

// This example requires the Places library. Include the libraries=places
// parameter when you first load the API. For example:
// <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCFvFQGJhY-5bMYhHmblzKsGY96EFi_tuU&libraries=places">

function initAutocomplete() {
    var map = new google.maps.Map(document.getElementById('gMap'), {
        center: { lat: -33.8688, lng: 151.2195 },
        zoom: 13,
        mapTypeId: 'roadmap'
    });

    // Create the search box and link it to the UI element.
    var input = document.getElementById('formAddressRecherche');
    var searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

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

        CleanMarkersExceptSelected();
        // For each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }
            /*
             * USED CODE FOR GMAPS MARKER
             * 
             * var icon = {
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(25, 25)
            };
            alert(place.geometry.location.lng() + ';' + place.geometry.location.lat());
            // Create a marker for each place.
            markers.push(new google.maps.Marker({
                map: map,
                icon: icon,
                title: place.name,
                position: place.geometry.location
            }));

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
            
            */

            for (var component in componentForm) {
                if (component !== "street_number" && component !== "route") {
                    document.getElementById(component).value = '';
                }
            }

            document.getElementById('formAddressRecherche').value = '';

            // Get each component of the address from the place details,
            // and then fill-in the corresponding field on the form.
            for (var i = 0; i < place.address_components.length; i++) {
                var addressType = place.address_components[i].types[0];
                if (componentForm[addressType]) {
                    var val = place.address_components[i][componentForm[addressType]];
                    if (addressType === "street_number" || addressType === "route") {
                        document.getElementById('formAddressRecherche').value += val + ' ';
                    }
                    else {
                        document.getElementById(addressType).value = val;
                    }
                }
            }

            selectedDescription = place.name;
            selectedLongitude = place.geometry.location.lng();
            selectedLatitude = place.geometry.location.lat();
            DisplayAdress(selectedLatitude, selectedLongitude, selectedDescription);
        });
        map.fitBounds(bounds);
    });
}


/* Google Maps API Handling END */