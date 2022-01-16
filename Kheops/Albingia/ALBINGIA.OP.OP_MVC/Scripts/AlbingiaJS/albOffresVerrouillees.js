$(document).ready(function () {
    formatDatePicker();
    MapElementPage();
    tri();
    AlternanceLigne("BodyParam", "", false, null);
    Refresh();
});
//-------------Refresh------------------------------
function Refresh() {
    $("#btnRefresh").click(function () {
        $.ajax({
            type: "POST",
            url: "/OffresVerrouillees/RefreshPage",
            success: function (data) {
                $("#divBodyParam").html(data);
                AlternanceLigne("BodyParam", "", false, null);
                tri();
            }
        });
    });
}
//-------------Ajout offre verouillee------------------------------
function AjoutOffreVerouille() {
    $("#btnAjout").click(function () {
        $.ajax({
            type: "POST",
            url: "/OffresVerrouillees/AjouterOffreVerouille",
            success: function (data) {
                //                $("#divBodyParam").html(data);
                //                AlternanceLigne("BodyParam", "", false, null);
                //                tri();
            }
        });
    });
}
//-------------Recherche offres verouillees------------------------------
function RechOffresVerouillees() {
    $("#btnRechercher").click(function () {
        $("#DateDebutFiltre").removeClass('requiredField');
        $("#DateFinFiltre").removeClass('requiredField');

        var TypeOffre_O = $("#O").is(':checked');
        var TypeOffre_P = $("#P").is(':checked');
        var NumOffre = $("#NumeroOffreFiltre").val();
        var Version = $("#VersionFiltre").val();
        var Utilisateur = $("#UtilisateurFiltre").val();
        var DateVerouillageDebut = $("#DateDebutFiltre").val();
        var DateVerouillageFin = $("#DateFinFiltre").val();
        if (DateVerouillageFin != "" && DateVerouillageDebut > DateVerouillageFin) {
            $("#DateDebutFiltre").addClass('requiredField');
            $("#DateFinFiltre").addClass('requiredField');
            return;
        }
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/OffresVerrouillees/GetOffresVerouillees",
            data: { typeOffreO: TypeOffre_O, typeOffreP: TypeOffre_P, numOffre: NumOffre, version: Version, utilisateur: Utilisateur, dateVerouillageDebut: DateVerouillageDebut, dateVerouillageFin: DateVerouillageFin },
            success: function (data) {
                CloseLoading();
                $("#divBodyParam").html(data);
                AlternanceLigne("BodyParam", "", false, null);
                tri();
            }
        });
    });

}
//---------------Map les éléments de la page--------------
function MapElementPage() {
    Selection();
    SupprimerOffre();
    Filtre();
    AjoutOffreVerouille();
    RechOffresVerouillees();
    $("#btnCache").die().live('click', function () {
        $.ajax({
            type: "POST",
            url: "/OffresVerrouillees/LoadOffreCache",
            success: function (data) {
                $("#divDataOffreSession").html(data);
                $("#divOffreSession").show();
                AlternanceLigne("BodyParamCache", "", false, null);
                MapElementCache();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
        $("#divOffreSession").toggle();
    });
}
//---------Map les éléments de la div flottante des offre en cache--------
function MapElementCache() {
    $("#btnSupprimerCache").die().live('click', function () {
        if ($("input[name='checkOffreCache']:checked").length > 0) {
            DeleteOffreCache();
        }
        else {
            common.error.show("Veuillez faire un choix d'offres et de contrats à déverrouiller.");
        }
    });
    $("#allCacheCheckbox").die().live('change', function () {
        if ($(this).is(':checked'))
            $("input[type=checkbox][name=checkOffreCache]").attr('checked', 'checked');
        else
            $("input[type=checkbox][name=checkOffreCache]").removeAttr('checked');
    });
    $("#btnFancyFermer").die().live('click', function () {
        CloseDivOffreCache();
    });
}
//--------Ferme la div des offres caches et reload la liste des offres verrouillees-------
function CloseDivOffreCache() {
    $.ajax({
        type: "POST",
        url: "/OffresVerrouillees/CloseDivOffreCache",
        success: function (data) {
            $("#divDataOffreSession").html('');
            $("#divOffreSession").hide();
            $("#divBodyParam").html(data);
            AlternanceLigne("BodyParam", "", false, null);
            tri();
            $("#hCheckbox").removeAttr("checked");
            ReloadNbOffreVerrouillee();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Supprime les offres du cache--------------
function DeleteOffreCache() {
    var offres = "";
    $("input[type=checkbox][name=checkOffreCache]").each(function () {
        if ($(this).is(":checked"))
            offres += $("#splitCharHtml").val() + $(this).attr("albOffreCache");
    });
    if (offres != "")
        offres = offres.substr(4);

    $.ajax({
        type: "POST",
        url: "/OffresVerrouillees/DeleteOffreCache",
        data: { offres: offres },
        success: function (data) {
            $("#divDataOffreSession").html(data);
            $("#divOffreSession").show();
            AlternanceLigne("BodyParamCache", "", false, null);
            MapElementCache();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------------- Annuler -------------------------------
function Annuler() {
    window.location.href = "/BackOffice/Index";
}
//------------------- Selection------------------------------
function Selection() {
    $("#hCheckbox").click(function () {
        if ($(this).is(":checked")) {
            $(".checkboxSuppression").each(function () {
                $(this).attr("checked", "checked");
            });
        }
        else {
            $(".checkboxSuppression").each(function () {
                $(this).removeAttr("checked");
            });
        }
    });
}

//--------------------Supprimer Offre--------------------------------------------
function SupprimerOffre() {
    $("#btnSupprimer").click(function () {
        var offresSupprimees = "";
        var splitChr = $("#spliChar").val();
        $("#tblBodyParam tr").each(function () {
            if ($(this).children(".colSuppression").children(".checkboxSuppression").is(":checked")) {
                offresSupprimees += $(this).children(".colCodeOffre").html() + "||" + $(this).children(".colVersion").html() + "||" + $(this).children(".colTypeOffre").html() + "||" + $(this).children(".colNumAvn").html() + splitChr;
            }
        });
        offresSupprimees = offresSupprimees.substring(0, offresSupprimees.lastIndexOf(splitChr));

        $.ajax({
            type: "POST",
            url: "/OffresVerrouillees/SupprimerOffres",
            data: "numerosOffres=" + offresSupprimees,//+ "&par1=test&par2=test&par3=test&par4=test",
            success: function (data) {
                $("#divBodyParam").html(data);
                AlternanceLigne("BodyParam", "", false, null);
                tri();
                $("#hCheckbox").removeAttr("checked");
                ReloadNbOffreVerrouillee();
            }
        });
    });
}
//-----------------------Filtre-------------------------
function Filtre() {
    $("#btnFiltre").click(function () {
        var utilisateurFiltre = $("#UtilisateurFiltre").val();
        var dateDebutFiltre = $("#DateDebutFiltre").val();
        var dateFinFiltre = $("#DateFinFiltre").val();
        $.ajax({
            type: "POST",
            url: "/OffresVerrouillees/FiltrerOffres",
            data: "utilisateurFiltre=" + utilisateurFiltre + "&dateDebutFiltre=" + dateDebutFiltre + "&dateFinFiltre=" + dateFinFiltre,
            success: function (data) {
                $("#divBodyParam").html(data);
                AlternanceLigne("BodyParam", "", false, null);
                tri();
            }
        });
    });
}

//----------------------Formate tous les controles datepicker---------------------
function formatDatePicker() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
}

//--------------------- Trie de la grille
function tri() {
    $("th.tablePersoHead[name=orderCol]").each(function () {
        $(this).css('cursor', 'pointer');
        var Colonne = $(this);
        Colonne.click(function () {
            $("#DateDebutFiltre").removeClass('requiredField');
            $("#DateFinFiltre").removeClass('requiredField');

            var img = $(".imageTri").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1);
            img = img.substr(0, img.lastIndexOf('.'));
            var modeTri = "asc";
            if (img == "tri_asc") {
                modeTri = "desc";
            }
            else if (img == "tri_desc") {
                modeTri = "asc";
            }
            //var col = ':nth-child(' + Colonne.attr("id") + ')';
            var colName = Colonne.text();
            var TypeOffre_O = $("#O").is(':checked');
            var TypeOffre_P = $("#P").is(':checked');
            var NumOffre = $("#NumeroOffreFiltre").val();
            var Version = $("#VersionFiltre").val();
            var Utilisateur = $("#UtilisateurFiltre").val();
            var DateVerouillageDebut = $("#DateDebutFiltre").val();
            var DateVerouillageFin = $("#DateFinFiltre").val();
            if (DateVerouillageFin != "" && DateVerouillageDebut > DateVerouillageFin) {
                $("#DateDebutFiltre").addClass('requiredField');
                $("#DateFinFiltre").addClass('requiredField');
                return;
            }
            ShowLoading();
            $.ajax({
                type: "POST",
                url: "/OffresVerrouillees/Tri",
                data: { colName: colName, modeTri: modeTri, typeOffreO: TypeOffre_O, typeOffreP: TypeOffre_P, numOffre: NumOffre, version: Version, utilisateur: Utilisateur, dateVerouillageDebut: DateVerouillageDebut, dateVerouillageFin: DateVerouillageFin },
                success: function (data) {
                    CloseLoading();
                    $("#divBodyParam").html(data);
                    AlternanceLigne("BodyParam", "", false, null);
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

//---------Recharge le nombre d'offre verrouillee-------------
function ReloadNbOffreVerrouillee() {
    $.ajax({
        type: "POST",
        url: "/OffresVerrouillees/ReloadNbOffreVerrouillee",
        success: function (data) {
            if (data != "") {
                $("#lblNbBDD").text(data.split('_')[0]);
                $("#lblNbCache").text(data.split('_')[1]);
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}