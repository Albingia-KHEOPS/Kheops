$(document).ready(function () {
    MapPageElement();
    //Transversetri();
});
//------------Map les éléments de la page div Liste des clauses---------
function MapPageElement() {
    if (window.isReadonly) {
        $("#EtapesTransverse").removeAttr('disabled');
        $("#ContexteTransverse").removeAttr('disabled');
        $("#FiltreTransverse").removeAttr('disabled');
        $("#AjoutClause").attr('disabled', 'disabled');
        $("#ClauseLibre").attr('disabled', 'disabled');
    }
    AffectTitleList($("#ContexteTransverse"));
    AffectTitleList($("#EtapesTransverse"));



    $("#btnFermer").click(function () {
        if (!window.sessionStorage) return;
        var filter = JSON.parse(window.sessionStorage.getItem('recherche_filter'));
        if (!filter) return;
        $("#Offre_CodeOffre").val(filter.CodeOffre);
        $("#Offre_Version").val(filter.version);
        $("#Offre_Type").val(filter.TypeContrat);
        $("#divDataListeClauses").html('');
        $("#divListeClauses").hide();
    });

    $("th[name=headerTriTransverse]").die().live("click", function () {

        var colTri = $(this).attr("albcontext");
        var transverse = $(this).hasClass('transverse');
        if (transverse) { var img = $("img[albcontext=" + colTri + "]").attr("src").substr($(".transverseImageTri").attr("src").lastIndexOf('/') + 1); }
        else { var img = $("img[albcontext=" + colTri + "]").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1); }

        img = img.substr(0, img.lastIndexOf('.'));
        TrierClausesTransverse(colTri, img);
    });

    EtapeChange();
    ContexteTransverseChange();
    FiltreTransverseChange();
    RisqueChange();
    FormuleChange();
    MapElementLstClauseTransverse();
}
//------------Map les éléments de la Liste des clauses---------
function MapElementLstClauseTransverse() {
    $("div[name=TransverselinkVisu]").each(function () {
        $(this).off("click");
        $(this).die().live("click", function () {
            //AMO 2613 / 061117
            dialogClauseVisualisation($(this).attr("AlbLinkId").split("_")[1], "False");//, $(this).attr("AlbIsLibre"));
            //transverseDialogClauseVisualisation($(this).attr("AlbLinkId").split("_")[1], $(this).attr("AlbIsLibre"));
        });
    });
    $("td[name=TransverselinkDetail]").each(function () {
        $(this).off("click");
        $(this).die().live("click", function () {
            //transverseDialogClauseDetail($(this).attr("AlbLinkId").split("_")[1], $(this).attr("AlbLinkId").split("_")[2], $(this).attr("AlbLinkId").split("_")[3]);
            transverseDialogClauseDetail($(this).attr("AlbLinkId"));
        });
    });
    if (window.isReadonly) {
        $("#EtapesTransverse").removeAttr('disabled');
        $("#ContexteTransverse").removeAttr('disabled');
        $("#FiltreTransverse").removeAttr('disabled');
    }
    AlternanceLigne("TransverseTableFichier", "", false, null);

}
//------------Map les éléments de la div de visualisation
function MapElementVisualisationTransverse() {
    $("#transverseBtnFermerVisualisation").off();
    $("#transverseBtnFermerVisualisation").click(function () {
        $("#divTransverseDataVisualiserClause").val('');
        $("#divTransverseVisualiserClause").hide();
        ReactivateShortCut();
    });
    $("#transverseBtnModifierTexte").off();
    $("#transverseBtnModifierTexte").click(function () {
        TransverseModifierTexteClauseLibre();
    });
}
//------------Map les éléments de la div de l'ajout d'une clause libre
function MapElementClauseLibreTransverse() {
    $("#btnAnnulerClauseLibre").click(function () {
        ReactivateShortCut();
        $("#divDataClauseLibre").val('');
        $("#divClauseLibre").hide();
    });
    $("#btnValiderClauseLibre").click(function () {
        EnregistrerClauseLibre();
    });
}

function EtapeChange() {
    $("#EtapesTransverse").die().live('change', function () {
        let selection = recherche.affaires.getClickedResult();
        var typeOffreContrat = selection.typ || $("#Offre_Type").val();
        var codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
        var version = $("#Offre_Version").val() || selection.alx;
        var codeAvn = $("#NumAvenantPage").val() || selection.avn;
        var etape = $("#EtapesTransverse").val();
        var filtre = $(this).val();
        var filtreFiltres = $("#FiltreTransverse").val();
        var id = $(this).attr("id").substring(4);
        var acteGestion = $("#ActeGestion").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ChoixClauses/TransverseFiltrer",
            context: $(this),
            data: {
                id: id, codeOffreContrat: codeOffre, versionOffreContrat: version, typeOffreContrat: typeOffreContrat, codeAvn: codeAvn, etape: etape,
                tabGuid: $("#tabGuid").val(),
                filtre: filtreFiltres,
                modeNavig: $("#ModeNavig").val(),
                acteGestion: acteGestion
            },
            success: function (data) {
                $("#divListeClauses").show();
                $("#divDataListeClauses").html(data);
                MapElementLstClauseTransverse();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });


        $(".trTransverse").contents().filter(function () {
            return true;
        }).closest('.trTransverse').show();
        if (filtre != "Tous") {
            $(".trTransverse").filter(function () {
                return $.trim($(this).find("td:eq(7)").text()) != filtre;
            }).closest('.trTransverse').hide();
        }
    });
    $("#btnFermerVisualisation").die().live('click', function () {
        $("#divTransverseVisualiserClause").val('');
        $("#divFullScreenVisualiserClause").hide();
        ReactivateShortCut();
    });
}

function ContexteTransverseChange() {
    $("#ContexteTransverse").die().live('change', function () {
        AffectTitleList($("#ContexteTransverse"));
        var typeOp = "O";
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var etape = $("#EtapesTransverse").val();
        var filtre = $(this).val();
        $(".trTransverse").contents().filter(function () {
            return true;
        }).closest('.trTransverse').show();
        if (filtre != "Tous") {
            $(".trTransverse").filter(function () {
                return $.trim($(this).find("td:eq(6)").text()) != filtre;
            }).closest('.trTransverse').hide();
        }
    });
}

function FiltreTransverseChange() {
    $("#FiltreTransverse").die().live('change', function () {
        let selection = recherche.affaires.getClickedResult();
        let typeOffreContrat = selection.typ || $("#Offre_Type").val();
        let codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
        let version = $("#Offre_Version").val() || selection.alx;
        let codeAvn = $("#NumAvenantPage").val() || selection.avn;
        let etape = $("#EtapesTransverse").val();
        let filtreEtape = $(this).val();
        let filtre = $("#FiltreTransverse").val();
        let acteGestion = $("#ActeGestion").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ChoixClauses/TransverseFiltrer",
            context: $(this),
            data: {
                codeOffreContrat: codeOffre, versionOffreContrat: version, typeOffreContrat: typeOffreContrat, codeAvn: codeAvn, etape: etape,
                provenance: $("#Provenance").val(), fullScreen: $("#modeLstClause").val(), tabGuid: $("#tabGuid").val(),
                filtre: filtre,
                modeNavig: $("#ModeNavig").val(),
                acteGestion: acteGestion
            },
            success: function (data) {
                $("#divListeClauses").show();
                $("#divDataListeClauses").html(data);
                MapElementLstClauseTransverse();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });


        $(".trTransverse").contents().filter(function () {
            return true;
        }).closest('.trTransverse').show();
        if (filtreEtape != "Tous") {
            $(".trTransverse").filter(function () {
                return $.trim($(this).find("td:eq(7)").text()) != filtreEtape;
            }).closest('.trTransverse').hide();
        }
    });
}
function RisqueChange() {
    $("#Risque").die().live('change', function () {
        var typeOffre = $("#Offre_Type").val();
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var codeAvn = $("#NumAvenantPage").val();
        var codeRisque = $("#Risque").val();
        var acteGestion = $("#ActeGestion").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ChoixClauses/FilterParRisque",
            context: $(this),
            data: {
                codeOffre: codeOffre, version: version, typeOffre: typeOffre, codeAvn: codeAvn,
                codeRisque: codeRisque, modeNavig: $("#ModeNavig").val(), acteGestion: acteGestion
            },
            success: function (data) {
                $("#divFormules").html(data);
                MapElementLstClauseTransverse();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}

function FormuleChange() {
    $("#Formule").die().live('change', function () {
        var typeOffre = $("#Offre_Type").val();
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var codeAvn = $("#NumAvenantPage").val();
        var codeFormule = $("#Formule").val();
        var acteGestion = $("#ActeGestion").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ChoixClauses/FilterParFormule",
            context: $(this),
            data: {
                codeOffre: codeOffre, version: version, typeOffre: typeOffre, codeAvn: codeAvn,
                codeFormule: codeFormule, modeNavig: $("#ModeNavig").val(), acteGestion: acteGestion
            },
            success: function (data) {
                $("#divOptions").html(data);
                MapElementLstClauseTransverse();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}


//--------------------Affiche la div flottante visualisation-------------------
function transverseDialogClauseVisualisation(id, isLibre) {
    var typeOffreContrat = $("#Offre_Type").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var codeAvn = $("#NumAvenantPage").val();
    var etape = $("#EtapesTransverse").val();

    if (isLibre == 'False') {
        common.dialog.bigError("En cours de développement", true);
        return;
    }

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/TransverseObtenirClauseVisualisation",
        data: {
            idClause: id,
            codeOffreContrat: codeOffre,
            versionOffreContrat: version,
            typeOffreContrat: typeOffreContrat,
            codeAvn: codeAvn,
            etape: etape,
            tabGuid: $("#tabGuid").val(),
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            $("#divTransverseDataVisualiserClause").html(data);
            AlbScrollTop();
            $("#divTransverseVisualiserClause").show();
            CloseLoading();
            TransverseFormatCLEditorVisualisation();
            MapElementVisualisationTransverse();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------------- affiche la div flottante "Détail"--------------
function transverseDialogClauseDetail(id) {
    let rubrique = id.split("_")[1];
    let sousRubrique = id.split("_")[2];
    let sequence = id.split("_")[3];
    let selection = recherche.affaires.getClickedResult();
    let typeOffreContrat = selection.typ || $("#Offre_Type").val();
    let codeOffre = $("#Offre_CodeOffre").val() || selection.ipb;
    let version = $("#Offre_Version").val() || selection.alx;
    let codeAvn = $("#NumAvenantPage").val() || selection.avn;
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/TransverseObtenirClauseDetails",
        data: {
            rubrique: rubrique,
            sousRubrique: sousRubrique,
            sequence: sequence,
            codeOffreContrat: codeOffre,
            versionOffreContrat: version,
            typeOffreContrat: typeOffreContrat,
            codeAvn: codeAvn,
            tabGuid: $("#tabGuid").val(),
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDetailsClauseTransverse").html(data);
            AlbScrollTop();
            $("#divFullScreenDetailsClauseTransverse").show();
            MapElementClauseDetail();

            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function MapElementClauseDetail() {
    $("#btnFermerChoixClauseTransverse").die().live('click', function () {
        $("#divFullScreenDetailsClauseTransverse").hide();
    });
}
//------- Modifier le texte de la clause libre en visualisation-----
function TransverseModifierTexteClauseLibre() {
    var clauseLibreText = $("#TransverseTexteClauseLibreVisualisation").val();
    var acteGestion = $("#ActeGestion").val();
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/TransverseUpdateTextClauseLibre",
        data: {
            clauseId: $("#ClauseId").val(),
            codeOffreContrat: $("#Offre_CodeOffre").val(),
            versionOffreContrat: $("#Offre_Version").val(),
            typeOffreContrat: $("#Offre_Type").val(),
            codeAvn: $("#NumAvenantPage").val(),
            tabGuid: $("#tabGuid").val(),
            provenance: $("#Provenance").val(),
            etape: $("#EtapesTransverse").val(),
            titre: $("#TitreClauseLibre").val(),
            texte: encodeURIComponent(clauseLibreText),
            modeNavig: $("#ModeNavig").val(),
            acteGestion: acteGestion
        },
        success: function (data) {
            $("#divDataListeClauses").html(data);
            MapElementLstClauseTransverse();
            $("#divTransverseVisualiserClause").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request, true);
        }
    });
}
//---------------Affiche la pop-up d'édition de choix clause------------------
function TransverseAfficherEcranClauseLibre() {
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/AfficherEcranClauseLibre",
        data: {
            codeOffreContrat: $("#Offre_CodeOffre").val(),
            versionOffreContrat: $("#Offre_Version").val(),
            typeOffreContrat: $("#Offre_Type").val(),
            provenance: '',
            etape: $("#EtapesTransverse").val()
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDataClauseLibre").html(data);
            TransverseFormatCLEditor();
            AlbScrollTop();
            MapElementClauseLibreTransverse();
            $("#divClauseLibre").show();

        },
        error: function (request) {
            common.error.showXhr(request, true);
        }
    });
}


//----------------------Formate le controle cleditor de la clause libre---------------------
function TransverseFormatCLEditor() {
    $("#ClauseLibreEdit").cleditor({ width: 1000, height: 265, controls: "bold italic underline | bullets numbering | outdent indent | alignleft center alignright" });
}
function TransverseFormatCLEditorVisualisation() {
    $("#TransverseTexteClauseLibreVisualisation").cleditor({ width: 1009, height: 325, controls: "bold italic underline | bullets numbering | outdent indent | alignleft center alignright" });
}
//--------------------- Trie de la grille
function Transversetri() {
    $("th.transverse").css('cursor', 'pointer');
    $("th.transverse").closest("table").on("click", "th.transverse", function () {
        const colonne = $(this);
        let img = $(".transverseImageTri").attr("src").substr($(".transverseImageTri").attr("src").lastIndexOf('/') + 1);
        img = img.substr(0, img.lastIndexOf('.'));
        const attrId = Colonne.attr("id");
        const col = +attrId;
        const colRisque = 3;
        const colObjet = 4;
        const colFormule = 5;
        let sorted = null;
        if (col == colRisque) {
            sorted = $(".trTransverse").sort(sorter(getTextGetter(col), img, sorter(getTextGetter(colObjet), sorter(getTextGetter(colFormule)))));
        }
        else {
            sorted = $(".trTransverse").sort(sorter(getTextGetter(col), img));
        }
        sorted.remove().appendTo("#tblTransverseTableFichier");

        AlternanceLigne("TransverseTableFichier", "", false, null);
        if (img == "tri_asc") {
            $(".transverseImageTri").attr("src", "/Content/Images/tri_desc.png");
        }
        else if (img == "tri_desc") {
            $(".transverseImageTri").attr("src", "/Content/Images/tri_asc.png");
        }
        $(".transverseSpImg").hide();
        colonne.children(".transverseSpImg").show();
    });
}

const defaultCollator = new Intl.Collator("fr-fr", { numeric: true, sensitivity: "base", usage: "sort", ignorePunctuation: true });

/**
 * 
 * @param {HTMLElement} a
 * @param {Number} i
 */
function getTextGetter(a) {
    i = +i;
    return function (a) {
        (a.children[i] && a.children[i].innerText.trim()) || "";
    };
}

function sorter(getter, order, fallback) {
    if (typeof (order) == "function" && arguments.length == 2) { fallback = order; order = ""; }
    order = order || "asc";
    const sign = order.toLowerCase().indexOf("desc") >= 0 ? -1 : 1;
    return function (a, b) {
        var comp = defaultCollator.compare(getter(a), getter(b));
        if (comp == 0 && typeof (fallback) == "function") {
            return sign * fallback(a, b);
        }
        return sign * comp;
    };
}

function TrierClausesTransverse(colTri, img) {
   
    ShowLoading();
    let selection = recherche.affaires.getClickedResult();
    $.ajax({
        type: "POST",
        url: "/ChoixClauses/TrierClauses",
        data: {
            modeAffichage: "Transverse",
            colTri: colTri,
            codeOffre: $("#Offre_CodeOffre").val() || selection.ipb,
            version: $("#Offre_Version").val() || selection.alx,
            type: selection.typ || $("#Offre_Type").val(),
            codeAvn: selection.avn || $("#NumAvenantPage").val(),
            etape: $("#EtapesTransverse").val(),
            provenance: $("#Provenance").val(),
            tabGuid: $("#tabGuid").val(),
            codeRisque: "",
            codeFormule: "",
            codeOption: "",
            filtre: $("#FiltreTransverse").val(),
            imgTri: img
        },
        success: function (data) {
            $("#dvTableauxChoixClausesTransverse").html(data);
            MiseAJourImagesTri(colTri, img);
            MapElementLstClauseTransverse();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request, true);
        }
    });
}

function MiseAJourImagesTri(colTri, img) {
    const base = "/Content/Images/tri";
    const ext = ".png";
    const asc = "asc";
    const desc = "desc";
    let tri = (img == "tri_asc") ? desc : asc;
    //if (colTri == "Contexte") { tri = "asc"; }
    $("img.transverseImageTri").attr("src", base + ext);
    $("img[albcontext=" + colTri+"]").attr("src", base+"_"+tri+ext);
}