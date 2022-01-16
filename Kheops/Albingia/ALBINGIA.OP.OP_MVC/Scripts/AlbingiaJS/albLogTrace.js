$(document).ready(function () {
    formatDatePicker();
    MapElementPage();
    tri();
    AlternanceLigne("Body", "", false, null);
    OuvrirMessage();
    FermerMessage();
    //Refresh();
});
//---------------Map les éléments de la page--------------
function MapElementPage() {
    RechercheLogTraces();
    RechercheLogPerf();
    Retour();

}
//----------------------Formate tous les controles datepicker---------------------
function formatDatePicker() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
}
//-------------Recherche offres verouillees------------------------------
function RechercheLogTraces() {
    $("#btnRechercher").click(function () {

        $("#DateDebutFiltre").removeClass('requiredField');
        $("#DateFinFiltre").removeClass('requiredField');

        var codeType = $("#CodeType").val();
        var motCle = encodeURIComponent($("#MotCle").val());
        var dateDebut = $("#DateDebutFiltre").val();
        var dateFin = $("#DateFinFiltre").val();
        if (dateFin != "" && dateDebut > dateFin) {
            $("#DateDebutFiltre").addClass('requiredField');
            $("#DateFinFiltre").addClass('requiredField');
            return;
        }
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/LogTrace/RechercheLogTraces",
            data: { codeType: codeType, motCle: motCle, dateDebut: dateDebut, dateFin: dateFin },
            success: function (data) {
                CloseLoading();
                $("#divBodyListLogTraces").html(data);
                AlternanceLigne("Body", "", false, null);
                OuvrirMessage();
            }
        });
    });
}

function RechercheLogPerf() {
    $("#btnRechercherPerf").click(function () {

        $("#DateDebutFiltre").removeClass('requiredField');
        $("#DateFinFiltre").removeClass('requiredField');

        var codeType = $("#CodeType").val();
        var motCle = encodeURIComponent($("#MotCle").val());
        var dateDebut = $("#DateDebutFiltre").val();
        var dateFin = $("#DateFinFiltre").val();
        if (dateFin != "" && dateDebut > dateFin) {
            $("#DateDebutFiltre").addClass('requiredField');
            $("#DateFinFiltre").addClass('requiredField');
            return;
        }
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/LogPerf/RechercheLogPerf",
            data: { dateDebut: dateDebut, dateFin: dateFin },
            success: function (data) {
                CloseLoading();
                $("#divBodyListLogTraces").html(data);
                AlternanceLigne("Body", "", false, null);
                OuvrirMessage();
            }
        });
    });
}
//--------------------- Trie de la grille
function tri() {
    $("th.triable").each(function () {
        $(this).css('cursor', 'pointer');
        var Colonne = $(this);
        Colonne.click(function () {
            $("#DateDebutFiltre").removeClass('requiredField');
            $("#DateFinFiltre").removeClass('requiredField');

            var codeType = $("#CodeType").val();
            var motCle = encodeURIComponent($("#MotCle").val());
            var dateDebut = $("#DateDebutFiltre").val();
            var dateFin = $("#DateFinFiltre").val();

            if (dateFin != "" && dateDebut > dateFin) {
                $("#DateDebutFiltre").addClass('requiredField');
                $("#DateFinFiltre").addClass('requiredField');
                return;
            }
            var img = $(".imageTri").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1);
            img = img.substr(0, img.lastIndexOf('.'));
            var modeTri = "asc";
            if (img == "tri_asc") {
                modeTri = "desc";
            }
            else if (img == "tri_desc") {
                modeTri = "asc";
            }
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/LogTrace/Tri",
                data: { modeTri: modeTri, codeType: codeType, motCle: motCle, dateDebut: dateDebut, dateFin: dateFin },
                success: function (data) {
                    CloseLoading();
                    $("#divBodyListLogTraces").html(data);
                    AlternanceLigne("Body", "", false, null);
                    OuvrirMessage();
                    if (img == "tri_asc") {
                        $(".imageTri").attr("src", "../../../Content/Images/tri_desc.png");
                    }
                    else if (img == "tri_desc") {
                        $(".imageTri").attr("src", "../../../Content/Images/tri_asc.png");
                    }
                }
            });
        });
    });
}
//--------------------Ouvre le message du log dans une div flottante
function OuvrirMessage() {
    $("td[name=linkOpen]").each(function () {
        $(this).click(function () {
            var message = $(this).text();
            $("#divDataMessageLog #Message").html(message);
            $("#divMessageLog").show();
        });
    });
}
//-----------------Ferme la div flottante contenant le message du log
function FermerMessage() {
    $("#btnFermer").die().live('click', function () {
        $("#divMessageLog").hide();
    });
}

function Retour() {
    $('#btnAnnulerRetour').unbind();
    $('#btnAnnulerRetour').bind('click', function () {
        window.location.href = "/BackOffice/Index";
    });
}

function ExportToCSV() {
    var dateDebut = $("#DateDebutFiltre").val();
    var dateFin = $("#DateFinFiltre").val();
    var id = dateDebut + '_' + dateFin;

    var splitChar = "_";
    window.location.href = "/LogPerf/ExportFile/?id=" + encodeURIComponent(id);
    return true;
}