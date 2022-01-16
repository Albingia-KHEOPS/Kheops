$(document).ready(function () {
    MapElementsPage();
    FormatDecimalValue();
});

function MapElementsPage() {
    toggleDescription($("#Observation"));
    $("#Observation").html($("#Observation").html().replace(/&lt;br&gt;/ig, "\n"));
    var isBackOffice = $("#isBackOffice").val();

    $("#btnFermerVisualisationSuspension").die().live('click', function () {
        Annuler();
    });

    if (isBackOffice === 'True') {
        bindRadioButtonEvent();
        $("#btnSearch").die().live('click', function () {
            RechercheSuspension();
        });
        $('td.tdBackOfficeOnly').show();
        $('#infoContrat').hide();
    
    }
    else
        $('td.tdRadioButton').css('display', 'none');
    
    AlternanceLigne("Susp", "", false, null);
}

function FormatDecimalValue() {
    common.autonumeric.applyAll('init', 'numeric','');
}


function RechercheSuspension() {

    ShowLoading();
    $('#infoContrat').hide();

    var codeOffre = $("#CodeRech").val();
    var version = $("#VersionRech").val();

    $.ajax({
        type: "POST",
        url: "/Suspension/RechercheSuspension",
        data: { codeOffre: codeOffre, version: version },
        success: function (data) {
            $("#divSusp").html(data);
            AlbScrollTop();
            toggleDescription($("#Observation"));
            bindRadioButtonEvent();
            AlternanceLigne("Susp", "", false, null);
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}

function bindRadioButtonEvent() {
    $("input[id^=supension_]").change(function () {

        var attributes = $(this).attr('id').split('_');

        $.ajax({
            type: "GET",
            url: "/Suspension/GetInfosContrat",
            data: { codeOffre: attributes[1], version: attributes[2], type: attributes[3] },
            success: function (data) {
                $("#divInfos").html(data);
                if ($('#infoContrat').css('display') === 'none')
                    $('#infoContrat').show();
                $("#Observation").html($("#Observation").html().replace(/&lt;br&gt;/ig, "\n"));
                toggleDescription($("#Observation"));
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
