$(document).ready(function () {
    MapElementPage();
});
//-----------Map les éléments de la page-----------
function MapElementPage() {
    $("#btnAnnuler").die().live('click', function () {
        //ShowCommonFancy("Confirm", "Cancel",
        //"Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        //350, 130, true, true);
        CancelForm();
    });

    $("#btnConfirmOk").die().live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case 'Clausier':
                CloseClausier();
                break;
            case 'DelClause':
                DeleteClause();
                break;
            case "ListValid":
                EnregistrerClauseLibre();
                break;
                //case "Cancel":
                //    CancelForm();
                //    break;
            default:
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").die().live('click', function () {
        CloseCommonFancy();
        //ReactivateShortCut();
    });

    $("#tabBase").die().live('click', function () {
        ShowClauseTab('GEN', null, null, null);
    });
    $("#tabRisques").die().live('click', function () {
        ShowClauseTab('RSQ', '1', null, null);
    });
    $("#tabGaranties").die().live('click', function () {
        ShowClauseTab('OPT', null, '1', '1');
    });

    $("#btnFermerVisualisation").die().live('click', function () {
        $("#divVisualiserClause").html('');
        $("#divFullScreenVisualiserClause").hide();
        ReactivateShortCut();
    });
    $("#btnModifierTexte").die().live('click', function () {
        $("#TitreClauseLibre").removeClass("requiredField");
        $("#AppliqueA").removeClass("requiredField");
        if ($("#TitreClauseLibre").val() == "") {
            $("#TitreClauseLibre").addClass("requiredField");
        }
        if ($("#AppliqueA").val() == "") {
            $("#AppliqueA").addClass("requiredField");
        }
        if ($("#TitreClauseLibre").val() == "" || $("#AppliqueA").val() == "") {
            return;
        }
        ModifierTexteClauseLibre();
    });

    $("#btnFermerChoixClause").die().live('click', function () {
        $("#divFullScreenDetailsClause").hide();
    });

    $("#btnAnnulerClauseLibre").die().live('click', function () {
        ReactivateShortCut();
        //$("#ClauseLibreEdit").val('');
        $("#divEditClauseLibre").html('');
        $("#divFullScreenClauseLibre").hide();
    });
    $("#btnValiderClauseLibre").die().live('click', function () {

        $("#inLibelleClauseLibre").removeClass("requiredField");
        $("#AppliqueA").removeClass("requiredField");

        if ($("#AppliqueA").val() == "") {
            $("#AppliqueA").addClass("requiredField");
        }
        if ($("#inLibelleClauseLibre").val() == "") {
            $("#inLibelleClauseLibre").addClass("requiredField");
        }

        if ($("#inLibelleClauseLibre").val() == "" || $("#AppliqueA").val() == "") {
            return;
        }

        var codeEtapeAjout = $("#CodeEtapeAjout").val();
        if (codeEtapeAjout == 'RSQ') {
            var code = $("#CodeRisqueObjet").val();
            if (typeof (code) == "undefined" || code == "") return;
            if (code.split('_').length == 1) {
                ShowCommonFancy("Confirm", "ListValid", "Êtes-vous sûr de vouloir associer cette clause à l'ensemble du risque ?", 450, 80, true, true);
            }
                //if ($("#IsRsqSelected").val() == "True")
                //    ShowCommonFancy("Confirm", "ListValid", "Êtes-vous sûr de vouloir associer cette clause à l'ensemble du risque ?", 450, 80, true, true);
            else EnregistrerClauseLibre();
        }
        else if (codeEtapeAjout == 'OPT' || codeEtapeAjout == 'GAR') {
            var codeFrm = $("#CodeFormuleApplique").val();
            if (typeof (codeFrm) == "undefined" || codeFrm == "") return;
            EnregistrerClauseLibre();
        }
    });

    $("#FullScreen").die().live('click', function () {
        OpenCloseFullScreen(true);
    });

    $("#btnSuivant").kclick(function (evt, data) {
        CreateOffreSimplifiee(data && data.returnHome);
    });

    $("th[name=headerTri]").die().live("click", function () {
        var colTri = $(this).attr("albcontext");
        var transverse = $(this).hasClass('transverse');
        if (transverse)
        { var img = $("img[albcontext=" + colTri + "]").attr("src").substr($(".transverseImageTri").attr("src").lastIndexOf('/') + 1); }
        else
        { var img = $("img[albcontext=" + colTri + "]").attr("src").substr($(".imageTri").attr("src").lastIndexOf('/') + 1); }

        img = img.substr(0, img.lastIndexOf('.'));
        TrierClauses(colTri, img);
    });
    MapElementLstClause();
}
//------------Map les éléments de la div Liste des clauses---------
function MapElementLstClause() {
    AffectTitleList($("#Contexte"));
    AffectTitleList($("#Etapes"));
    AffectTitleList($("#Filtre"));
    AffectTitleList($("#ContexteCible"));

    AlternanceLigne("TableFichier", "", false, null);

    $("#dvLinkClose").die().live('click', function () {
        OpenCloseFullScreen(false);
    });

    $("#Contexte").die().live('change', function () {
        AffectTitleList($("#Contexte"));
        var typeOp = "O";
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var etape = $("#Etapes").val();
        var filtre = $(this).val();
        $(".trFichier").contents().filter(function () {
            return true;
        }).closest('.trFichier').show();
        if (filtre != "Tous") {
            $(".trFichier").filter(function () {
                return $.trim($(this).find("td:eq(6)").text()) != filtre;
            }).closest('.trFichier').hide();
        }
    });

    $("#Etapes").die().live('change', function () {
        var type = $("#Offre_Type").val();
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var codeAvn = $("#NumAvenantPage").val();
        //var etape = $("#Etape").val();
        var etape = $("#Etapes").val();
        var filtre = $(this).val();
        var filtreFiltres = $("#Filtre").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ClauseSimplifiee/Filtrer",
            context: $(this),
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, etape: etape,
                tabGuid: $("#tabGuid").val(),
                //codeRisque: $("#CodeRsq").val(), codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(),
                codeRisque: '', codeFormule: '', codeOption: '',
                filtre: filtreFiltres,
                fullScreen: $("#inputFullScreen").val(),
                modeNavig: $("#ModeNavig").val()
            },
            success: function (data) {
                if ($("#inputFullScreen").val() == "True")
                    $("#divDataFullScreen").html(data);
                else
                    $("#divDataClause").html(data);
                //SwitchOnglet(etape);
                MapElementLstClause();

                SetEtapesAjout();

                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });


        $(".trFichier").contents().filter(function () {
            return true;
        }).closest('.trFichier').show();
        if (filtre != "Tous") {
            $(".trFichier").filter(function () {
                return $.trim($(this).find("td:eq(7)").text()) != filtre;
            }).closest('.trFichier').hide();
        }
    });

    $("#Filtre").die().live('change', function () {
        var type = $("#Offre_Type").val();
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var codeAvn = $("#NumAvenantPage").val();
        var etape = $("#Etapes").val();
        var filtreEtape = $(this).val();
        var filtre = $("#Filtre").val();
        var id = $(this).attr("id").substring(4);
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ClauseSimplifiee/Filtrer",
            context: $(this),
            data: {
                id: id, codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, etape: etape,
                fullScreen: $("#inputFullScreen").val(), tabGuid: $("#tabGuid").val(),
                //codeRisque: $("#CodeRsq").val(), codeFormule: $("#CodeFormule").val(), codeOption: $("#CodeOption").val(),
                codeRisque: '', codeFormule: '', codeOption: '',
                filtre: filtre,
                modeNavig: $("#ModeNavig").val()
            },
            success: function (data) {
                if ($("#inputFullScreen").val() == "True")
                    $("#divDataFullScreen").html(data);
                else
                    $("#divDataClause").html(data);
                SwitchOnglet(etape);
                MapElementLstClause();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });


        $(".trFichier").contents().filter(function () {
            return true;
        }).closest('.trFichier').show();
        if (filtreEtape != "Tous") {
            $(".trFichier").filter(function () {
                return $.trim($(this).find("td:eq(7)").text()) != filtreEtape;
            }).closest('.trFichier').hide();
        }
        AffectTitleList($(this));
    });

    $('img[name=suppression]').each(function () {
        $(this).css("cursor", "pointer");
        $(this).click(function () {
            $("#delId").val($(this).attr("id").substring(4));
            ShowCommonFancy("Confirm", "DelClause", "Etes-vous sûr de vouloir supprimer cette clause ?", 350, 80, true, true);
        });
    });

    $("#btnFSSuivant").kclick(function () {
        $("#btnSuivant").trigger('click');
    });

    $("input[type=checkbox][id^=IsCheck_]").each(function () {
        $(this).click(function () {
            UpdateCheckClause($(this));
        });
    });
    $("td[name=linkDetail]").each(function () {
        $(this).die();
        $(this).click(function () {
            dialogClauseDetail($(this).attr("AlbLinkId"));//.split("_")[1]);
        });
    });

    $("div[name=linkVisu]").each(function () {
        $(this).die();
        $(this).live('click', function () {
            dialogClauseVisualisation($(this).attr("AlbLinkId").split("_")[1], $(this).attr("AlbIsLibre"));
        });
    });
    //tri();
    if (window.isReadonly) {
        $("#divAjoutContexte").hide();
        $("#Contexte").removeAttr('disabled');
        $("#Filtre").removeAttr('disabled');
    }

    $("#AjoutClause").die().live('click', function () {
        $("#TypeClause").val("normale");
        $("#ContexteCible").removeClass('requiredField');
        $("#EtapesAjout").removeClass('requiredField');
        var erreur = false;
        if ($("#ContexteCible").val() == "") {
            $("#ContexteCible").addClass('requiredField');
            erreur = true;
        }
        if ($("#EtapesAjout").val() == "") {
            $("#EtapesAjout").addClass('requiredField');
            erreur = true;
        }
        if (erreur) return;
        if (!$(this).attr('disabled')) {
            $("#ContexteCible").removeClass('requiredField');
            $("#EtapesAjout").removeClass('requiredField');
            OpenClausier();
        }
    });

    $("#ClauseLibre").die().live('click', function () {
        $("#TypeClause").val("libre");
        $("#ContexteCible").removeClass('requiredField');
        $("#EtapesAjout").removeClass('requiredField');
        var erreur = false;
        if ($("#ContexteCible").val() == "") {
            $("#ContexteCible").addClass('requiredField');
            erreur = true;
        }
        if ($("#EtapesAjout").val() == "") {
            $("#EtapesAjout").addClass('requiredField');
            erreur = true;
        }
        if (erreur) return;
        if (!$(this).attr('disabled')) {
            $("#ContexteCible").removeClass('requiredField');
            $("#EtapesAjout").removeClass('requiredField');
            AfficherEcranClauseLibre($("#ContexteCible").val());
        }
    });
    $("#ContexteCible").die().live('change', function () {
        AffectTitleList($(this));
    });

    $("#EtapesAjout").die().live('change', function () {
        var type = $("#Offre_Type").val();
        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var codeAvn = $("#NumAvenantPage").val();
        var etape = $("#EtapesAjout").val();
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ClauseSimplifiee/GetDDLContextesCible",
            context: $(this),
            data: {
                codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, etape: etape,
                modeNavig: $("#ModeNavig").val()
            },
            success: function (data) {
                $("#divContexteCible").html(data);
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//-----------Map les éléments de la div Clausier--------
function MapElementClausier() {
    $("#btnFancyValider").die().live('click', function () {
        $("td[name=AppliqueA]").removeClass("requiredField");
        if ($.trim($("td[name=AppliqueA]").html()) == '') {
            $("td[name=AppliqueA]").addClass("requiredField");
            return;
        }
        if (!$(this).attr("disabled")) {
            ValidClausier($("#CodeEtapeAjout").val(), $("#CodeRisqueObjet").val(), $("#CodeFormuleApplique").val());
        }
    });
    $("#btnFancyAnnuler").die().live('click', function () {
        ShowCommonFancy("Confirm", "Clausier", "Etes-vous sûr de vouloir annuler ?", 350, 80, true, true);
    });
    $("#btnRechercherClausier").die().live('click', function () {
        //ShowLoading();
        $.ajax({
            type: "POST",
            url: "/ClauseSimplifiee/Recherche",
            data: { libelle: $("#Libelle").val(), motCle1: $("#MotCle1").val(), motCle2: $("#MotCle2").val(), motCle3: $("#MotCle3").val(), sequence: $("#Sequence").val(), rubrique: $("#Rubrique").val(), sousrubrique: $("#SousRubrique").val(), selectionPossible: $("#SelectionPossible").val(), modaliteAffichage: $("#ModaliteAffichage").val(), date: $("#Date").val() },
            success: function (dataReturn) {
                CloseLoading();
                $("#dvRechercheGeneral").show();
                $("#dvRechercheGeneral").html(dataReturn);
                $("#btnFancyValider").css("display", "inline");
                $("#btnFancyValider").attr("disabled", "disabled");

                AlternanceLigne("BodyClausier", "", false, null);
                InitTable()
                ViualisationClause();
                ChoixClause();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
    $("#Rubrique").die().live('change', function () {
        AffectTitleList($(this));
        $.ajax({
            type: "POST",
            url: "/ClauseSimplifiee/GetSousRubriques",
            data: { codeRubrique: $(this).val() },
            success: function (data) {
                $("#SousRubriqueDiv").html(data);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
    $("#SousRubrique").die().live('change', function () {
        AffectTitleList($(this));
        $.ajax({
            type: "POST",
            url: "/ClauseSimplifiee/GetSequences",
            data: { codeSousRubrique: $(this).val(), codeRubrique: $("#Rubrique").val() },
            success: function (data) {
                $("#SequenceDiv").html(data);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//------------Affiche les clauses suivant l'étape----------
function ShowClauseTab(etape, codeRisque, codeFormule, codeOption) {
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/OpenClausesTab",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), codeAvn: $("#NumAvenantPage").val(), tabGuid: $("#tabGuid").val(),
            etape: etape, codeRisque: codeRisque, codeFormule: codeFormule, codeOption: codeOption, fullScreen: $("#inputFullScreen").val(),
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            if ($("#inputFullScreen").val() == "True")
                $("#divDataFullScreen").html(data);
            else
                $("#divDataClause").html(data);
            SwitchOnglet(etape);
            MapElementLstClause();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------Switch les onglets--------
function SwitchOnglet(etape) {
    switch (etape) {
        case 'GEN':
            $('#tabBase').removeClass("onglet");
            $('#tabBase').addClass("onglet-actif");

            $("#tabRisques").removeClass("onglet-actif");
            $("#tabRisques").addClass("onglet");

            $("#tabGaranties").removeClass("onglet-actif");
            $("#tabGaranties").addClass("onglet");

            $("#CodeRsq").val("");
            $("#CodeFormule").val("");
            $("#CodeOption").val("");
            break;
        case 'RSQ':
            $("#tabBase").removeClass("onglet-actif");
            $("#tabBase").addClass("onglet");

            $('#tabRisques').removeClass("onglet");
            $('#tabRisques').addClass("onglet-actif");

            $("#tabGaranties").removeClass("onglet-actif");
            $("#tabGaranties").addClass("onglet");

            $("#CodeRsq").val("1");
            $("#CodeFormule").val("");
            $("#CodeOption").val("");
            break;
        case 'OPT':
            $('#tabBase').removeClass("onglet-actif");
            $('#tabBase').addClass("onglet");

            $("#tabRisques").removeClass("onglet-actif");
            $("#tabRisques").addClass("onglet");

            $("#tabGaranties").removeClass("onglet");
            $("#tabGaranties").addClass("onglet-actif");

            $("#CodeRsq").val("");
            $("#CodeFormule").val("1");
            $("#CodeOption").val("1");
            break;
        default:
            break;
    }
    $("#Etape").val(etape);
}
//---------Ouvre le clausier-------------
function OpenClausier() {
    var libelleContexte = $("#ContexteCible option:selected").text();
    var codeContexte = $("#ContexteCible").val();
    var libelleEtapeAjout = $("#EtapesAjout option:selected").text();
    var codeEtapeAjout = $("#EtapesAjout").val();
    var nbrRisques = $("#NbRisques").val();
    var nbrObjetsRisque1 = $("#NbObjetsRisque1").val();
    var codeRisqueObjet = $("#CodeAppliqueA").val();
    var risqueObjet = $("#LibelleAppliqueA").val();
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/OpenClausier",
        data: {
            codeContexte: codeContexte, libelleContexte: libelleContexte, libelleEtapeAjout: libelleEtapeAjout, codeEtapeAjout: codeEtapeAjout,
            NbrRisques: nbrRisques, NbrObjetsRisque1: nbrObjetsRisque1, codeRisqueObjet: codeRisqueObjet, risqueObjet: risqueObjet
        },
        success: function (data) {
            $("#divClausier").show();
            $("#divDataClausier").html(data);
            MapElementClausier();
            MapElementAppliqueA();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Enregistre la clause choisie du clausier----------
function ValidClausier(etape, codeObjRsq, codeFormule) {
    var codeRsq = '';
    var codeObj = '';
    var codeOption = '';
    if (etape == 'RSQ') {
        codeFormule = '';
        if (codeObjRsq.split('_').length > 1) {
            codeRsq = codeObjRsq.split('_')[0];
            codeObj = codeObjRsq.split('_')[1];
        }
        else if (codeObjRsq.split('_').length == 1) {
            codeRsq = codeObjRsq;
        }
    }
    else if (etape == 'OPT' || etape == 'GAR') {
        codeOption = '1';
    }
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/EnregistrerClause",
        data: {
            type: $("#Offre_Type").val(), codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), codeAvn: $("#NumAvenantPage").val(), tabGuid: $("#tabGuid").val(), natureClause: "P",
            codeClause: $("#CodeClause").val(), rubrique: $("#RubriqueClause").val(), sousRubrique: $("#SousRubriqueClause").val(), sequence: $("#SequenceClause").val(),
            versionClause: $("#VersionClause").val(), actionEnchaine: "", contexte: $("#ContexteCible").val(),
            etape: etape,
            codeRsq: codeRsq, codeFor: codeFormule,
            codeOption: codeOption,
            codeObj: codeObj,
            fullScreen: $("#inputFullScreen").val(),
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            CloseClausier();
            CloseLoading();
            if ($("#inputFullScreen").val() == "True")
                $("#divDataFullScreen").html(data);
            else
                $("#divDataClause").html(data);
            MapElementLstClause();
            //ShowClauseTab(etape, '', '', '');
            //ShowClauseTab(etape, codeRsq, codeFormule, codeOption);
        },
        error: function (request) {
            $("input[name=rbt]").attr('checked', false);
            common.error.showXhr(request);

        }
    });
}
//----------Annule le clausier----------
function CloseClausier() {
    $("#divDataClausier").html('');
    $("#divClausier").hide();
}
//------ affiche le bouton valider lors du choix d'une clause et filtre la liste des contextes
//-------------------- Permet de dynamiser la derniere entete du tableau suivant la scroll verticale.
function InitTable() {
    $("img[name=HistoLink]").die().live('click', function () {
        Historique($(this).attr("id"));
    });
}
//---------------Afiche l'interface de visualisation de la clause
function ViualisationClause() {
    $(".lnkDetail").each(function () {
        $(this).click(function () {
            ShowCommonFancy("Error", "", "La Visualisation d'une clause est en cours de développement", 400, 130, true, true);
        });
    });
}
//------ affiche le bouton valider lors du choix d'une clause et filtre la liste des contextes
function ChoixClause() {
    $(".ChkClause").die().live('click', function () {
        var clause = $(this).val().split('_');
        $("#CodeClause").val(clause[0]);
        $("#RubriqueClause").val(clause[1]);
        $("#SousRubriqueClause").val(clause[2]);
        $("#SequenceClause").val(clause[3]);
        $("#VersionClause").val(clause[4]);
        var date = $("#Date").val();
        var dateDeb = clause[5];
        var dateFin = clause[6];
        var rubrique = clause[7];
        var sousrubrique = clause[8];
        var sequence = clause[9];
        var code = clause[0];
        var libelle = clause[11];
        var versionClause = clause[4];

        if (dateDeb != 0 && (dateDeb > date || (dateFin < date && dateFin != 0))) {
            if (versionClause == 1) {
                ShowCommonFancy("Error", "", "Cette clause n’est pas valide au " + $("#DateFormate").val(), 1212, 700, true, true);
            }
            else Historique("Histo_" + rubrique + "_" + sousrubrique + "_" + sequence + "_" + code + "_" + libelle, "ClauseInvalide");
            $("#btnFancyValider").attr("disabled", "disabled");
        }
        else {
            $("#btnFancyValider").removeAttr("disabled");
        }
    });
}
//------Supprime la clause sélectionnée------------
function DeleteClause() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/Supprime",
        context: $(this),
        data: {
            id: $("#delId").val(),
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            codeAvn: $("#NumAvenantPage").val(),
            etape: $("#Etapes").val(),
            tabGuid: $("#tabGuid").val(),
            //codeRisque: $("#CodeRsq").val(),
            //codeFormule: $("#CodeFormule").val(),
            //codeOption: $("#CodeOption").val(),
            codeRisque: '',
            codeFormule: '',
            codeOption: '',
            fullScreen: $("#inputFullScreen").val(),
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            $("#delId").val("");
            CloseLoading();
            if ($("#inputFullScreen").val() == "True")
                $("#divDataFullScreen").html(data);
            else
                $("#divDataClause").html(data);
            MapElementLstClause();
            //ShowClauseTab($("#Etape").val(), $("#CodeRsq").val(), $("#CodeFormule").val(), $("#CodeOption").val(), $("#inputFullScreen").val());
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//------MAJ Clause----
function UpdateCheckClause(chkbox) {
    var isCheck = chkbox.is(':checked');
    var chkId = chkbox.attr('id').split("_")[1];

    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/UpdateCheckBox",
        data: {
            clauseId: chkId, isChecked: isCheck
        },
        success: function (data) {
        },
        error: function (request) {
            CloseLoading();
            common.error.showXhr(request);
        }
    });


}
//--------------------Affiche la div flottante visualisation-------------------
function dialogClauseVisualisation(id, isLibre) {
    var typeOffreContrat = $("#Offre_Type").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var codeAvn = $("NumAvenantPage").val();
    //var etape = $("#Etapes").val();

    if (isLibre == 'False') {
        $("#TypeClause").val("normale");
        common.dialog.bigError("En cours de développement", true);
        return;
    }
    else $("#TypeClause").val("libre");

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/ObtenirClauseVisualisation",
        data: {
            idClause: id,
            codeOffreContrat: codeOffre,
            versionOffreContrat: version,
            typeOffreContrat: typeOffreContrat,
            codeAvn: codeAvn,
            tabGuid: $("#tabGuid").val(),
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            $("#divVisualiserClause").html(data);
            AlbScrollTop();
            $("#divFullScreenVisualiserClause").show();
            CloseLoading();
            FormatCLEditorVisualisation();
            MapElementAppliqueA();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
function MapElementAppliqueA() {
    BtnMultiObj();
    $("#btnListCancel").die();
    $("#btnListCancel").click(function () {
        $("#divLstRisques").hide();
    });
    $("#btnListValid").die();
    $("#btnListValid").click(function () {
        var codeEtapeAjout = $("#CodeEtapeAjout").val();
        if (codeEtapeAjout == 'RSQ') {
            AppliqueRisque();
        }
    });
    $("#btnListFrmValid").die();
    $("#btnListFrmValid").click(function () {
        var codeEtapeAjout = $("#CodeEtapeAjout").val();
        if (codeEtapeAjout == 'OPT' || codeEtapeAjout == 'GAR') {
            AppliqueFormule();
        }
    });

    $("input[name=checkRsq]").die().live('click', function () {
        selectListObj($(this).is(':checked'));
    });
    $("input[name=checkObj]").die().live('click', function () {
        if ($(this).is(':checked'))
            $("input[name=checkRsq]").attr('checked', false);
    });
    $("#btnListFrmCancel").die();
    $("#btnListFrmCancel").click(function () {
        $("#divLstFormules").hide();
    });
}
//----------------------Formate le controle cleditor de la clause libre---------------------
function FormatCLEditor() {
    $("#ClauseLibreEdit").cleditor({ width: 1000, height: 265, controls: "bold italic underline | bullets numbering | outdent indent | alignleft center alignright" });
}
function FormatCLEditorVisualisation() {
    $("#TexteClauseLibreVisualisation").cleditor({ width: 1009, height: 325, controls: "bold italic underline | bullets numbering | outdent indent | alignleft center alignright" });
}
//----------------Evenement liés au buton btnMultiObj-----
function BtnMultiObj() {
    $("#divBtnMultiObj").die();
    $("#divBtnMultiObj").click(function () {
        ShowDialogChoix();
    });
    $("div[name=btnMultiObj]").bind('mouseover', function () {
        var position = $(this).offset();
        var top = position.top;

        var codeEtapeAjout = $("#CodeEtapeAjout").val();
        if (codeEtapeAjout == 'RSQ') {
            if (checkListRsqObj() == true) {
                $("#divSelectedObjets").css({ 'position': 'absolute', 'z-index': 90, 'top': top + 15 + 'px', 'left': position.left - 328 + 'px' }).show();
            }
        }
        else if (codeEtapeAjout == 'OPT' || codeEtapeAjout == 'GAR') {
            if (checkFormule() == true) {
                $("#divSelectedFormule").css({ 'position': 'absolute', 'z-index': 90, 'top': top + 15 + 'px', 'left': position.left - 328 + 'px' }).show();
            }
        }
    });
    $("div[name=btnMultiObj]").bind('mouseout', function () {
        var codeEtapeAjout = $("#CodeEtapeAjout").val();
        if (codeEtapeAjout == 'RSQ') {
            $("#divSelectedObjets").hide();
        }
        else if (codeEtapeAjout == 'OPT' || codeEtapeAjout == 'GAR') {
            $("#divSelectedFormule").hide();
        }
    });

    $("div[name=divSelectedObjets]").bind('mouseover', function () {
        var position = $("#divBtnMultiObj").offset();
        var top = position.top;
        if (checkListRsqObj() == true) {
            $("#divSelectedObjets").css({ 'position': 'absolute', 'z-index': 90, 'top': top + 15 + 'px', 'left': position.left - 328 + 'px' }).show();
        }
    });
    $("div[name=divSelectedObjets]").bind('mouseout', function () {
        $("#divSelectedObjets").hide();
    });


    $("div[name=divSelectedFormule]").bind('mouseover', function () {
        var position = $("#divBtnMultiObj").offset();
        var top = position.top;
        if (checkFormule() == true) {
            $("#divSelectedFormule").css({ 'position': 'absolute', 'z-index': 90, 'top': top + 15 + 'px', 'left': position.left - 328 + 'px' }).show();
        }
    });
    $("div[name=divSelectedFormule]").bind('mouseout', function () {
        $("#divSelectedFormule").hide();
    });
}

//------- Modifier le texte de la clause libre en visualisation-----
function ModifierTexteClauseLibre() {
    var clauseLibreText = $("#TexteClauseLibreVisualisation").val();
    var etape = $("#CodeEtapeAjout").val();
    var codeObjRsq = $("#CodeRisqueObjet").val();
    var codeRsq = '';
    var codeObj = '';

    var codeFormule = $("#CodeFormuleApplique").val();
    var codeOption = '';
    if (etape == 'RSQ') {
        codeFormule = '';
        if (codeObjRsq.split('_').length > 1) {
            codeRsq = codeObjRsq.split('_')[0];
            codeObj = codeObjRsq.split('_')[1];
        }
        else if (codeObjRsq.split('_').length == 1) {
            codeRsq = codeObjRsq;
        }
    }
    else if (etape == 'OPT' || etape == 'GAR') {
        codeOption = '1';
    }
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/UpdateTextClauseLibre",
        data: {
            clauseId: $("#ClauseId").val(),
            codeOffreContrat: $("#Offre_CodeOffre").val(),
            versionOffreContrat: $("#Offre_Version").val(),
            typeOffreContrat: $("#Offre_Type").val(),
            codeAvn: $("#NumAvenantPage").val(),
            tabGuid: $("#tabGuid").val(),
            provenance: $("#Provenance").val(),
            etape: $("#Etapes").val(),
            titre: $("#TitreClauseLibre").val(),
            texte: encodeURIComponent(clauseLibreText),
            codeRisque: codeRsq,
            codeFormule: codeFormule,
            codeOption: codeOption,
            codeObj: codeObj,
            modeNavig: $("#ModeNavig").val(),
            fullScreen: $("#inputFullScreen").val()
        },
        success: function (data) {
            if ($("#inputFullScreen").val() == "True")
                $("#divDataFullScreen").html(data);
            else
                $("#divDataClause").html(data);

            MapElementLstClause();
            $("#divVisualiserClause").html('');
            $("#divFullScreenVisualiserClause").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


//-------------------- affiche la div flottante "Détail"--------------
function dialogClauseDetail(id) {
    var rubrique = id.split("_")[1];
    var sousRubrique = id.split("_")[2];
    var sequence = id.split("_")[3];
    //var rubrique = id.split("_")[0];
    //var sousRubrique = id.split("_")[1];
    //var sequence = id.split("_")[2];
    var typeOffreContrat = $("#Offre_Type").val();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var codeAvn = $("NumAvenantPage").val();
    var etape = $("#Etapes").val();
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/ObtenirClauseDetails",
        data: {
            rubrique: rubrique,
            sousRubrique: sousRubrique,
            sequence: sequence,
            codeOffreContrat: codeOffre,
            versionOffreContrat: version,
            typeOffreContrat: typeOffreContrat,
            codeAvn: codeAvn,
            etape: etape,
            tabGuid: $("#tabGuid").val(),
            codeRisque: $("#CodeRsq").val(),
            codeFormule: $("#CodeFormule").val(),
            codeOption: $("#CodeOption").val(),
            modeNavig: $("#ModeNavig").val()
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divDetailsClause").html(data);
            AlbScrollTop();
            $("#divFullScreenDetailsClause").show();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//---------------Affiche la pop-up d'édition de choix clause------------------
function AfficherEcranClauseLibre(contexte) {

    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/AfficherEcranClauseLibre",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            codeRisque: $("#CodeRsq").val(),
            //etape: $("#Etapes").val(),
            codeEtapeAjout: $("#EtapesAjout").val(),
            nbrRisques: $("#NbRisques").val(),
            codeRisqueObjet: $("#CodeAppliqueA").val(),
            risqueObjet: $("#LibelleAppliqueA").val(),
            contexte: contexte
        },
        success: function (data) {
            DesactivateShortCut();
            $("#divEditClauseLibre").html(data);
            FormatCLEditor();
            AlbScrollTop();
            $("#divFullScreenClauseLibre").show();
            MapElementAppliqueA();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------------Fonction qui enregistre une clause libre en base
function EnregistrerClauseLibre() {
    var contexte = $("#ContexteCible").val();
    var libelle = $("#inLibelleClauseLibre").val();
    //var appliqueA = $("#AppliqueA").val();
    var etape = $("#CodeEtapeAjout").val();

    var codeObjRsq = $("#CodeRisqueObjet").val();
    var codeRsq = '';
    var codeObj = '';

    var codeFormule = $("#CodeFormuleApplique").val();
    var codeOption = '';
    if (etape == 'RSQ') {
        codeFormule = '';
        if (codeObjRsq.split('_').length > 1) {
            codeRsq = codeObjRsq.split('_')[0];
            codeObj = codeObjRsq.split('_')[1];
        }
        else if (codeObjRsq.split('_').length == 1) {
            codeRsq = codeObjRsq;
        }
    }
    else if (etape == 'OPT' || etape == 'GAR') {
        codeOption = '1';
    }
    var ClauseLibreText = $("#ClauseLibreEdit").val();
    ClauseLibreText = encodeURIComponent(ClauseLibreText);

    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/EnregistrerClauseLibre",
        data: {
            codeOffreContrat: $("#Offre_CodeOffre").val(),
            versionOffreContrat: $("#Offre_Version").val(),
            typeOffreContrat: $("#Offre_Type").val(),
            codeAvn: $("#NumAvenantPage").val(),
            tabGuid: $("#tabGuid").val(),
            provenance: $("#Provenance").val(),
            contexte: contexte,
            etape: etape,
            libelle: libelle,
            texte: ClauseLibreText,

            codeRisque: codeRsq,
            codeFormule: codeFormule,
            codeOption: codeOption,
            codeObj: codeObj,

            modeNavig: $("#ModeNavig").val(),
            fullScreen: $("#inputFullScreen").val()
        },
        success: function (data) {
            if ($("#inputFullScreen").val() == "True")
                $("#divDataFullScreen").html(data);
            else
                $("#divDataClause").html(data);

            MapElementLstClause();
            $("#divEditClauseLibre").html('');
            $("#divFullScreenClauseLibre").hide();
            //ShowClauseTab($("#Etape").val(), $("#CodeRsq").val(), $("#CodeFormule").val(), $("#CodeOption").val());
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}



//----Reset de la page.----------
function CancelForm() {
    Redirection("OffreSimplifiee", "Index");
}
//----------------Redirection------------------
function Redirection(cible, job) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var branche = $("#Branche").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var message = 1;
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, branche: branche, message: message, tabGuid: tabGuid, modeNavig: $("#ModeNavig").val(), addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Ouvre le full screen-----------
function OpenCloseFullScreen(open) {
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/FullScreen",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            codeAvn: $("#NumAvenantPage").val(),
            etape: $("#Etapes").val(),
            tabGuid: $("#tabGuid").val(),
            codeRisque: $("#CodeRsq").val(),
            codeFormule: $("#CodeFormule").val(),
            codeOption: $("#CodeOption").val(),
            modeNavig: $("#ModeNavig").val(),
            fullScreen: open
        },
        success: function (data) {
            if (open) {
                DesactivateShortCut();
                $("#divDataClause").html('');
                $("#divFullScreen").show();
                $("#divDataFullScreen").html(data);
            }
            else {
                ReactivateShortCut();
                $("#divDataClause ").html(data);
                $("#divFullScreen").hide();
                $("#divDataFullScreen").html('');
            }
            MapElementLstClause();
            MapElementAppliqueA();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


//-----------Creation de l'offre simplifiée---------
function CreateOffreSimplifiee(returnHome) {
    if (returnHome) {
        common.page.goHome();
    }
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/CreateSimpleFolder",
        data: {
            codeOffre: $("#Offre_CodeOffre").val(), version: $("#Offre_Version").val(), type: $("#Offre_Type").val(), tabGuid: $("#tabGuid").val(),
            branche: $("#Branche").val(), cible: $("#Cible").val(), addParamType: addParamType, addParamValue: addParamValue
        },
        success: function () { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------- Selectionne un risque ou objet dans la liste---------------
function selectListObj(checkbox) {
    if (checkbox) {
        $("input[name=checkObj]").attr('checked', false);
    }
}
//-------------- Valide la sélection des objets dans la liste ---------
function validListRsqObj() {
    var strListObjCodes = ";";
    var strListObjLibelle = ";";
    if ($("#IsRsqSelected").val() != "True") {
        $("input[name=checkObj]").each(function () {
            if ($(this).is(":checked")) {
                if (strListObjCodes.indexOf(";" + $(this).val().split('_')[0] + ";") < 0) {
                    strListObjCodes += $(this).val().split('_')[0] + ";";
                    strListObjLibelle += $(this).val().split('_')[0] + "-" + $(this).val().split('_')[1] + ";";
                }
            }
            else {
                strListObjCodes = strListObjCodes.replace(";" + $(this).val().split('_')[0] + ";", ";");
                strListObjLibelle = strListObjLibelle.replace(";" + $(this).val().split('_')[0] + "-" + $(this).val().split('_')[1] + ";", ";");
            }
        });
        strListObjCodes = strListObjCodes.substring(1).substring(0, strListObjCodes.length - 2);

        $("#ObjetRisqueCode").val(strListObjCodes);
        strListObjLibelle = strListObjLibelle.substring(1).substring(0, strListObjLibelle.length - 2);
    }
    else {
        //strListObjLibelle = $("input[name=checkRsq]").val().split('_')[1];
        strListObjLibelle = $("input[name=checkRsq]").val().split('_')[0] + " - " + $("input[name=checkRsq]").val().split('_')[1];
        $("#ObjetRisqueCode").val("");
    }
    $("td[name=ObjetRisque]").html(strListObjLibelle);
}

//--------- Ouvre la boite de dialogue "Choix du s'applique à"
function ShowDialogChoix() {
    if (!window.isReadonly) {
        var codeEtapeAjout = $("#CodeEtapeAjout").val();
        if (codeEtapeAjout == 'RSQ') {
            if ($("#NbRisques").val() > 1 || ($("#NbRisques").val() == 1 && $("#NbObjetsRisque1").val() > 1)) {
                $("#divLstRisques").show();
            }
        }
        else if (codeEtapeAjout == 'OPT' || codeEtapeAjout == 'GAR') {
            $("#divLstFormules").show();
        }
    }
    //if (!window.isReadonly && $("#NbrObjets").val() > 1) {
    //    $("#divLstObj").show();
    //    checkListRsqObj();
    //    AlternanceLigne("Objets", "", false, null);
    //}
}

//---- Valider s'applique à un Risque/objet
function AppliqueRisque() {
    var typeClause = $("#TypeClause").val();
    if ($("input[name=checkRsq]").is(':checked')) {
        var rsqChecked = $("input[name=checkRsq]:checked").attr('id');
        var libelleRsqChecked = $("input[name=checkRsq]:checked").val();
        if (typeof (rsqChecked) != "undefined" && rsqChecked != "") {

            if (typeClause == "normale")
                $("td[name=AppliqueA]").html(libelleRsqChecked.split("_")[0] + "-" + libelleRsqChecked.split("_")[1]);
            else if (typeClause == "libre") $("#AppliqueA").val(libelleRsqChecked.split("_")[0] + "-" + libelleRsqChecked.split("_")[1]);

            $("td.LabelObjetRisque").html("S'applique au risque*");
            $("#CodeRisqueObjet").val(rsqChecked.split("_")[1]);
            $("#divLstRisques").hide();
            ReactivateShortCut();
        }
    }
    else {
        var objChecked = $("input[name=checkObj]:checked").attr('id');
        var libelleObjChecked = $("input[name=checkObj]:checked").val();
        if (typeof (objChecked) != "undefined" && objChecked != "") {
            if (typeClause == "normale")
                $("td[name=AppliqueA]").html(libelleObjChecked.split("_")[0] + "-" + libelleObjChecked.split("_")[1]);
                //$("td[name=AppliqueA]").html(libelleObjChecked.split("_")[0] + "-" + libelleObjChecked.split("_")[1] + " (" + libelleObjChecked.split("_")[2] + ")");
            else if (typeClause == "libre")
                $("#AppliqueA").val(libelleObjChecked.split("_")[0] + "-" + libelleObjChecked.split("_")[1]);
            //$("#AppliqueA").val(libelleObjChecked.split("_")[0] + "-" + libelleObjChecked.split("_")[1] + " (" + libelleObjChecked.split("_")[2] + ")");
            $("td.LabelObjetRisque").html("S'applique à l'objet*");
            $("#CodeRisqueObjet").val(objChecked.split("_")[1] + "_" + objChecked.split("_")[2]);
            $("#divLstRisques").hide();
            ReactivateShortCut();
        }
        else {
            common.dialog.bigError("Vous devez sélectionner un risque ou un objet.", true);
        }
    }
}
//---- Valider s'applique à une Formule
function AppliqueFormule() {
    var typeClause = $("#TypeClause").val();
    var frmChecked = $("input[name=checkFrm]:checked").attr('id');
    var libelleFrmChecked = $("input[name=checkFrm]:checked").val();
    if ($("input[name=checkFrm]").is(':checked')) {

        if (typeClause == "normale") {
            $("td[name=AppliqueA]").html(libelleFrmChecked);
        }
        else if (typeClause == "libre") {
            $("#AppliqueA").val(libelleFrmChecked);
        }
        $("#CodeFormuleApplique").val(frmChecked.split("_")[1]);
        $("#divLstFormules").hide();
        ReactivateShortCut();
    }
    else {
        common.dialog.bigError("Vous devez sélectionner une formule.", true);
    }
}
//--------------- affcihe les objets lors de l'ouverture ----------------
function checkListRsqObj() {
    var retour = false;
    $("input[name=checkObj]").removeAttr('checked');
    $("input[name=checkRsq]").removeAttr('checked');
    var code = $("#CodeRisqueObjet").val();
    if (typeof (code) != "undefined" && code != "") retour = true;
    else return false;
    if (code.split('_').length > 1) {
        $(".objRisque").hide();
        $("input[id=obj_" + code + "]").attr('checked', true);
        $("div[name=rsqObj_" + code.split('_')[0] + "]").show();
        $("div[id=obj_" + code + "]").show();
    }
    else if (code.split('_').length == 1) {
        $("input[id=rsq_" + code + "]").attr('checked', true);
        $(".objRisque").hide();
        $("div[name=rsq_" + code + "]").show();
        $("div[name=obj_" + code + "]").show();
    }
    return retour;
}
//--------------- affcihe la formule lors de l'ouverture ----------------
function checkFormule() {
    var retour = false;
    $("input[name=checkFrm]").removeAttr('checked');
    var code = $("#CodeFormuleApplique").val();
    if (typeof (code) != "undefined" && code != "") retour = true;
    else return false;
    $("input[id=frm_" + code + "]").attr('checked', true);
    $(".objFormule").hide();
    $("div[name=frm_" + code + "]").show();
    return retour;
}
function SetEtapesAjout() {
    if ($("#Etapes").val() != "") {
        $("#EtapesAjout").val($("#Etapes").val());
        $("#EtapesAjout").attr("disabled", "disabled");
        $("#EtapesAjout").addClass('readonly');
    }
    else {
        $("#EtapesAjout").val("");
        $("#EtapesAjout").removeAttr("disabled");
        $("#EtapesAjout").removeClass('readonly');
    }
}

function TrierClauses(colTri, img) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/ClauseSimplifiee/TrierClauses",
        data: {
            modeAffichage: "Classique",
            colTri: colTri,
            codeOffre: $("#Offre_CodeOffre").val(),
            version: $("#Offre_Version").val(),
            type: $("#Offre_Type").val(),
            codeAvn: $("#NumAvenantPage").val(),
            etape: $("#Etapes").val(),
            provenance: $("#Provenance").val(),
            tabGuid: $("#tabGuid").val(),
            //codeRisque: $("#CodeRisque").val(),
            //codeFormule: $("#CodeFormule").val(),
            //codeOption: $("#CodeOption").val(),
            codeRisque: '',
            codeFormule: '',
            codeOption: '',
            filtre: $("#Filtre").val(),
            modeNavig: $("#ModeNavig").val(),
            imgTri: img
        },
        success: function (data) {
            $("#dvTableauxChoixClauses").html(data);
            MiseAJourImagesTri(colTri, img);
            MapElementLstClause();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function MiseAJourImagesTri(colTri, img) {
    switch (colTri) {
        case "Titre":
            if (img == "tri_asc") {
                $("img[albcontext=Titre]").attr("src", "/Content/Images/tri_desc.png");
            }
            else {
                $("img[albcontext=Titre]").attr("src", "/Content/Images/tri_asc.png");
            }

            //$("img[albcontext=Titre]").attr("src", "/Content/Images/tri_desc.png");

            $("img[albcontext=Risque]").attr("src", "/Content/Images/tri.png");
            $("img[albcontext=Contexte]").attr("src", "/Content/Images/tri.png");
            $("img[albcontext=Edition]").attr("src", "/Content/Images/tri.png");
            break;
        case "Contexte":
            $("img[albcontext=Titre]").attr("src", "/Content/Images/tri.png");
            $("img[albcontext=Risque]").attr("src", "/Content/Images/tri.png");
            $("img[albcontext=Contexte]").attr("src", "/Content/Images/tri_asc.png");
            $("img[albcontext=Edition]").attr("src", "/Content/Images/tri.png");
            break;
        case "Risque":
            $("img[albcontext=Titre]").attr("src", "/Content/Images/tri.png");
            $("img[albcontext=Risque]").attr("src", "/Content/Images/tri_asc.png");
            $("img[albcontext=Contexte]").attr("src", "/Content/Images/tri.png");
            $("img[albcontext=Edition]").attr("src", "/Content/Images/tri.png");
            break;
        case "Edition":
            if (img == "tri_asc") {
                $("img[albcontext=Edition]").attr("src", "/Content/Images/tri_desc.png");
            }
            else {
                $("img[albcontext=Edition]").attr("src", "/Content/Images/tri_asc.png");
            }
            $("img[albcontext=Titre]").attr("src", "/Content/Images/tri.png");
            $("img[albcontext=Risque]").attr("src", "/Content/Images/tri.png");
            $("img[albcontext=Contexte]").attr("src", "/Content/Images/tri.png");

            break;
    }


}