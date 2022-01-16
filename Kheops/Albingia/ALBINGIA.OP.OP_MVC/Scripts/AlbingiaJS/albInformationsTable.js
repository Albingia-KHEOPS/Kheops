$(document).ready(function () {
    MapElementPage();
    AlternanceLigne("Body", "", false, null);
});

//---------------Map les éléments de la page--------------
function MapElementPage() {
    RechercherColonnes();
    Retour();
}

//-------------Recherche offres verouillees------------------------------
function RechercherColonnes() {
    $("#btnRechercher").click(function () {

        var env = encodeURIComponent($("#Environnement").val());
        var tableName = $("#Table").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/InformationsDataBase/RecherchInformationsTable",
            data: { env: env, tableName: tableName },
            success: function (data) {
                CloseLoading();
                $("#divResult").html(data);
                AlternanceLigne("Body", "", false, null);
            }
        });
    });
}


function Retour() {
    $('#btnAnnulerRetour').unbind();
    $('#btnAnnulerRetour').bind('click', function () {
        window.location.href = "/BackOffice/Index";
    });
}

