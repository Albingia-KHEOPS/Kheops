$(document).ready(function () {
    MapElementPage();
    AlternanceLigne("Body", "", false, null);
});

//---------------Map les éléments de la page--------------
function MapElementPage() {
    FormatNumericField();
    formatDatePicker();
    RechercherOffres();
    Retour();
}

//--------formattage numeric filtre
function FormatNumericField() {
    common.autonumeric.apply($("#Version"), 'init', 'numeric', '', ',', '0', null, '0');
    common.autonumeric.apply($("#CodeAvenant"), 'init', 'numeric', '', ',', '0', null, '0');
}

//----------------------Formate tous les controles datepicker---------------------
function formatDatePicker() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
}

//-------------Recherche offres ------------------------------
function RechercherOffres() {

    $('#btnRefresh').click(function () {

        $('#CheckOffre').prop('checked', true);
        $('#CheckContrat').prop('checked', true);
        $('#CodeOffre').val('');
        $('#Version').val('');
        $('#CodeAvenant').val('');
        $('#TypeTraitement').val('');
        $('#DateEffetAvnDebut').val('');
        $('#DateEffetAvnFin').val('');
        $('#CodePeriodicite').val('');
        $('#CodeBranche').val('');
        $('#CodeCible').val('');
        $('#UserCrea').val('');
        $('#UserMaj').val('');
    });

    $("#btnRechercher").click(function () {
        var filter = GetUserInputs();
        filter.PageNumber = 1;

        if (window.sessionStorage) {
            sessionStorage.setItem('filter', JSON.stringify(filter));
        }
        Rechercher(filter);

    });

    $('#PaginationPremierePage').live('click', function () {
        var filter = {};
        if (window.sessionStorage) {
            filter = JSON.parse(sessionStorage.getItem('filter'));
        } else {
            filter = GetUserInputs();
        }
        filter.PageNumber = 1;
        Rechercher(filter);

    });

    $('#PaginationPrecedent').live('click', function () {
        var filter = {};
        if (window.sessionStorage) {
            filter = JSON.parse(sessionStorage.getItem('filter'));
        } else {
            filter = GetUserInputs();
        }
        var pageNumber = parseInt($('#PageNumber').val());
        filter.PageNumber = pageNumber - 1;
        Rechercher(filter);

    });

    $('#PaginationSuivant').live('click', function () {
        var filter = {};
        if (window.sessionStorage) {
            filter = JSON.parse(sessionStorage.getItem('filter'));
        } else {
            filter = GetUserInputs();
        }
        var pageNumber = parseInt($('#PageNumber').val());
        filter.PageNumber = pageNumber + 1;
        Rechercher(filter);

    });

    $('#PaginationDernierePage').live('click', function () {
        var filter = {};
        if (window.sessionStorage) {
            filter = JSON.parse(sessionStorage.getItem('filter'));
        } else {
            filter = GetUserInputs();
        }
        var nbCount = parseInt($('#NbCount').val());
        var pageSize = parseInt($('#PageSize').val());

        filter.PageNumber = Math.ceil(nbCount / pageSize);
        Rechercher(filter);

    });

    $("#CodeBranche").change(function () {
        ShowLoading();
        var codeCibleSelect = $("#CodeCible");
        codeCibleSelect.empty();
        $.get('/RechercheOffreRapide/RechercherCibles?codeBranche=' + this.value, function (items) {
            $.each(items, function (i, item) {
                codeCibleSelect.append($('<option>',
                {
                    value: item.Value,
                    title: item.Title,
                    text: item.Text
                }));
                CloseLoading();
            });
        });
    });

    $('#btnRechercher').attr('disabled', 'disabled');
    $('#imgRecherche').attr('src', '/Content/Images/AffaireNouvelle/bouton_recherche_police_disabled.png');


    $('#divSearchContainer').find('input,select').live('change',
        function () {
            var filter = GetUserInputs();

            var isFormChanged = false;

            if (filter.CodeOffre ||
                filter.Version ||
                filter.CodeAvenant ||
                filter.TypeTraitement ||
                filter.DateEffetAvnDebut ||
                filter.DateEffetAvnFin ||
                filter.CodePeriodicite ||
                filter.CodeBranche ||
                filter.CodeCible ||
                filter.UserCrea ||
                filter.UserMaj
                ) {
                isFormChanged = true;
            }

            if (!isFormChanged && (filter.TypeOffres.length !== 2 || filter.TypeOffres.length === 0)) {
                isFormChanged = true;
            }



            if (isFormChanged) {
                $('#btnRechercher').removeAttr('disabled', 'disabled');
                $('#imgRecherche').attr('src', '/Content/Images/AffaireNouvelle/bouton_recherche_police_normal.png');
            } else {
                $('#btnRechercher').attr('disabled', 'disabled');
                $('#imgRecherche').attr('src', '/Content/Images/AffaireNouvelle/bouton_recherche_police_disabled.png');
            }
        });

}

function GetUserInputs() {

    var checkOffre = $('#CheckOffre').is(':checked');
    var checkContrat = $('#CheckContrat').is(':checked');
    var typesOffres = [];
    if (checkOffre) {
        typesOffres.push('O');
    }

    if (checkContrat) {
        typesOffres.push('P');
    }

    var codeOffre = $('#CodeOffre').val().trim();
    var version = $('#Version').val().trim();
    var codeAvenant = $('#CodeAvenant').val().trim();
    var typeTraitement = $('#TypeTraitement').val();
    var dateEffetAvnDebut = $('#DateEffetAvnDebut').val();
    var dateEffetAvnFin = $('#DateEffetAvnFin').val();
    var codePeriodicite = $('#CodePeriodicite').val();
    var codeBranche = $('#CodeBranche').val();
    var codeCible = $('#CodeCible').val();
    var userCrea = $('#UserCrea').val().trim();
    var userMaj = $('#UserMaj').val().trim();


    var filter = {
        'CodeOffre': codeOffre, 'Version': version, 'TypeOffres': typesOffres, 'CodeAvenant': codeAvenant, 'TypeTraitement': typeTraitement, DateEffetAvnDebut: dateEffetAvnDebut, DateEffetAvnFin: dateEffetAvnFin,
        'CodePeriodicite': codePeriodicite, 'CodeBranche': codeBranche, 'CodeCible': codeCible, 'UserCrea': userCrea, 'UserMaj': userMaj
    };

    return filter;
}

function Rechercher(filter) {

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RechercheOffreRapide/RechercherOffreRapide",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(filter),
        success: function (data) {
            CloseLoading();
            $("#divResult").html(data);
            AlternanceLigne("Body", "", false, null);
        }
    });
}


function Retour() {
    $('#btnAnnulerRetour').unbind();
    $('#btnAnnulerRetour').bind('click', function () {
        window.location.href = "/BackOffice/Index";
    });
}

