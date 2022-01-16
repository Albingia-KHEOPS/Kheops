
$(document).ready(function () {
    MapElementPage();
    Suivant();
});
//----------------Map les éléments de la page------------------
function MapElementPage() {
    $("#TauxForce").offOn("change", function () {
        ToggleButton();
        ControlReadOnly($(this));
        $("#fieldInput").val("COMMHCN");
        $("#valueInput").val($(this).val());
        $("#valueInputOld").val($("input[type='hidden'][id='" + $(this).attr("id") + "Old']").val());
    });
    $("#TotalHorsFraisHT").offOn("change", function () {
        ToggleButton();
        ControlReadOnly($(this));
        $("#fieldInput").val("HFRAISHT");
        $("#valueInput").val($(this).val());
        $("#valueInputOld").val($("input[type='hidden'][id='" + $(this).attr("id") + "Old']").val());
    });
    $("#TotalHT").offOn("change", function () {
        ToggleButton();
        ControlReadOnly($(this));
        $("#fieldInput").val("TOTALHT");
        $("#valueInput").val($(this).val());
        $("#valueInputOld").val($("input[type='hidden'][id='" + $(this).attr("id") + "Old']").val());
    });
    $("#TotalTTC").offOn("change", function () {
        ToggleButton();
        ControlReadOnly($(this));
        $("#fieldInput").val("TOTALTTC");
        $("#valueInput").val($(this).val());
        $("#valueInputOld").val($("input[type='hidden'][id='" + $(this).attr("id") + "Old']").val());
    });
    $("#btnAnnuler").kclick(function () {
        ShowCommonFancy("Confirm", "Cancel",
        "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
        350, 130, true, true);
    });
    $("#btnConfirmOk").kclick(function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Cancel":
                CancelForm();
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").kclick(function () {
        CloseCommonFancy();
        $("#hiddenAction").val("");
    });
    $("#btnRefresh").kclick(function () {
        if ($("#divRefresh").hasClass('CursorPointer')) {
            ReloadPage();
        }
        else {
            if ($("#txtSaveCancel").val() == "1")
                Redirection("RechercheSaisie", "Index");
        }
    });
    $("#btnReset").kclick(function () {
        if ($("#divReset").hasClass('CursorPointer')) {
            Redirection("Cotisations", "Index");
        }
    });

    $("span[id^=spanTarif]").kclick(function () {
        if ($(this).hasClass("CursorPointer")) {
            ShowTarif($(this));
        }
    });

    AlternanceLigne("GarantieList", "", false, null);
    AlternanceLigne("Total", "", false, null);
    AlternanceLigne("TotalPleinEcran", "", false, null);
    AlternanceLigne("BodyCommission", "", false, null);
    AlternanceLigne("BodyCommissionR", "", false, null);
    AlternanceLigne("GarantieListFullScreen", "", false, null);
    $("#btnFullScreen").click(function () {
        OpenCloseFullScreenListGaranties(true);
    });
    $("#dvLinkClose").click(function () {
        OpenCloseFullScreenListGaranties(false);
    });
    FormatDecimalValue();

    if (window.isReadonly) {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }

    $("#TypeFrais").die().live("change", function () {
        AffectTitleList($(this));
    });

    $("#chkGarantiePorteuse").die().live("click", function () {
        FiltrerGaranties($("#chkGarantiePorteuse").is(":checked"));
    });

    $("#lnkFraisAcc").die().live("click", function () {
        if ($("#lnkFraisAcc").hasClass("CursorPointer")) {
            OpenFraisAccessoires();
        }
    });

    $("#lnkFGA").die().live("click", function () {
        if ($("#lnkFGA").hasClass("CursorPointer")) {
            OpenFraisAccessoires();
        }
    });


    AffectTitleList($("#TypeFrais"));

    $("input[type='radio'][name='radioTypePart']").each(function () {
        $(this).removeAttr("disabled");
        $(this).change(function () {
            ChangeAffichagePart();
        });
    });
}
//function FormatCLEditor() {
//    var editObs = $("#CommentForce").cleditor({ width: 502, height: 100, controls: "bold italic underline | bullets numbering | outdent indent | alignleft center alignright" });
//    $(editObs[0].$frame).attr("tabindex", "6");
//}

//-------Formate les input/span des valeurs----------
function FormatDecimalValue() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '-99999999999.99');
    //FormatDecimal('decimal', ' ', 2, '99999999999.99', '-99999999999.99');
    common.autonumeric.applyAll('init', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
    //FormatPourcentDecimal();
}
//----------------Redirection------------------
function Redirection(cible, job) {
    common.page.isLoading = true;
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var argModelCommentForce = $("#CommentForce").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    $.ajax({
        type: "POST",
        url: "/Cotisations/Redirection/",
        data: { cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, argModelCommentForce: argModelCommentForce, tabGuid: tabGuid, paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Ouvre la fenêtre des tarifs----------------
function ShowTarif(elem) {
    $("#garantieUpdate").val(elem.attr('id'));
    var endId = elem.attr('id').replace('spanTarif', '');

    var codeGarantie = elem.attr('id').split('¤')[1];
    var lienKpgaran = $("#Kpgaran" + endId).val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Cotisations/ShowTarif/",
        data: { codeGarantie: lienKpgaran },
        success: function (data) {
            ShowFancyTarif(data, true, 'auto', 'auto', true);
            $("#btnFancyAnnuler").bind('click', function () {
                $.fancybox.close();
            });
            AlternanceLigne("Tarif", "", false, null);
            MapElementTarif();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------------------Affiche la fancybox----------------
function ShowFancyTarif(html, autoDim, width, height, modal) {
    $.fancybox(
        html,
        {
            'autoDimensions': autoDim,
            'width': width,
            'height': height,
            'modal': modal
        }
    );
}
//--------------Map les éléments de la fancy Tarif----------------
function MapElementTarif() {
    $("tr[id^=newTarif]").each(function () {
        $(this).click(function () {
            LoadNewTarif($(this));
            ToggleButton();
            $("#fieldInput").val("TARIF");
            $("input[type=text]").addClass('readonly').attr('readonly', 'readonly');
        });
    });
}
//--------------Charge le nouveau tarif------------------
function LoadNewTarif(elem) {
    var codeTarif = elem.attr('id').split('¤')[1];
    var lgnGarantie = $("#garantieUpdate").val();
    $("#" + lgnGarantie).text(codeTarif);
    $("#" + lgnGarantie.replace('spanTarif', 'Tarif')).val(codeTarif);

    var endId = lgnGarantie.replace('spanTarif', '');
    $("#Hidden" + endId).val("1");
    $("#garantieUpdate").val("");
    $.fancybox.close();
}
//--------------Change l'état des boutons------------------
function ToggleButton() {
    $("#divRefresh").addClass("CursorPointer");
    $("#btnRefresh").attr('src', '/Content/Images/boutonRefresh_3232.png');
    $("#lnkFraisAcc").removeClass("TxtLink");
    $("#lnkFGA").removeClass("TxtLink");
    $("#lnkFraisAcc").removeClass("CursorPointer");
    $("#lnkFGA").removeClass("CursorPointer");
    $("#btnSuivant").attr('disabled', 'disabled');
    $("#fieldInput").val("MAJ");
}
//-------------Bloque tous les contrôles excepté le contrôle en param----------
function ControlReadOnly(elem) {
    $("input[type=text]").addClass('readonly').attr('readonly', 'readonly');
    elem.removeClass('readonly').removeAttr('readonly', 'readonly');

}
//------------- Annule la form ------------------------
function CancelForm() {
    Redirection("AnMontantReference", "Index");
}
//------------Suivant--------------
function Suivant() {
    $("#btnSuivant:not(:disabled)").kclick(function (evt, data) {
        if (!window.isReadonly
            && $("#TauxForce").hasVal() && $("#TauxForce").val() != "0,00"
            && parseFloat($("#TauxStandardHCATNAT").val()) != parseFloat($("#TauxForce").val())
            && !$("#CommentForce").hasVal()) {

            $("#zoneTxtArea").addClass("requiredField");
            return;
        }
       //----------------------supression ecran info fin  
        //Redirection(data && data.returnHome ? "RechercheSasie" : "FinOffre", "Index");
        const codeOffre = $("#Offre_CodeOffre").val();
        const version = $("#Offre_Version").val();
        const type = $("#Offre_Type").val();
        $.ajax({
            type: "POST",
            url: "/Cotisations/GenerateClauses/",
            data: {
                codeOffre: codeOffre, version: version, type: type
            },
            success: function () {
                Redirection(data && data.returnHome ? "RechercheSaisie" : "ChoixClauses", "Index");
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });
}
//-------------Rafraichit la page avec les nouvelles données----------
function ReloadPage() {
    if ($("#fieldInput").val() != "") {

        var codeOffre = $("#Offre_CodeOffre").val();
        var version = $("#Offre_Version").val();
        var type = $("#Offre_Type").val();
        var field = $("#fieldInput").val();
        var value = $("#valueInput").val();
        var oldvalue = $("#valueInputOld").val();
        var tabGuid = $("#tabGuid").val();
        var modeNavig = $("#ModeNavig").val();
        var acteGestion = $("#ActeGestion").val();
        var codeAvn = $("#NumAvenantPage").val();
        var argCotisations = JSON.stringify($("#divCotisations").find(':input').serializeObject());

        var argCotisations = "[";

        $("#divHeaderGaranties :input[type=hidden][id^=Hidden]").each(function () {
            if ($(this).val() == "1") {
                var endId = $(this).attr('id').replace('Hidden', '');
                argCotisations += "{";
                argCotisations += 'CodeGarantie:"' + $("#CodeGarantie" + endId).val() + '",';
                argCotisations += 'Tarif:"' + $("#Tarif" + endId).val() + '",';

                argCotisations += 'TauxForce:"' + $("#TauxForce").val() + '",';
                argCotisations += 'MontantForce:"' + $("#MontantForce").val() + '",';
                argCotisations += 'CoefCom:"' + $("#CoefCom").val() + '",';
                argCotisations += 'TotalHorsFraisHT:"' + $("#TotalHorsFraisHT").val() + '",';
                argCotisations += 'TotalHT:"' + $("#TotalHT").val() + '",';
                argCotisations += 'TotalTTC:"' + $("#TotalTTC").val() + '"';

                argCotisations += " },";
            }
        });

        if (argCotisations == "[") {
            argCotisations += "{";
            argCotisations += 'TauxForce:"' + $("#TauxForce").val() + '",';
            argCotisations += 'MontantForce:"' + $("#MontantForce").val() + '",';
            argCotisations += 'CoefCom:"' + $("#CoefCom").val() + '",';
            argCotisations += 'TotalHorsFraisHT:"' + $("#TotalHorsFraisHT").val() + '",';
            argCotisations += 'TotalHT:"' + $("#TotalHT").val() + '",';
            argCotisations += 'TotalTTC:"' + $("#TotalTTC").val() + '"';
            argCotisations += " },";
        }

        argCotisations = argCotisations.substr(0, argCotisations.length - 1) + "]";
        if (argCotisations == "]")
            argCotisations = "";

        var argModelCommentForce = $("#CommentForce").val();

        ShowLoading();
        if ($("#txtParamRedirect").val() != "") {
            $.ajax({
                type: "POST",
                url: "/Cotisations/UpdateRedirect/",
                data: {
                    codeOffre: codeOffre, version: version, type: type, argCotisations: argCotisations, argModelCommentForce: argModelCommentForce,
                    field: field, value: value, oldvalue: oldvalue, tabGuid: tabGuid,
                    saveCancel: $("#txtSaveCancel").val(), paramRedirect: $("#txtParamRedirect").val(), modeNavig: modeNavig, acteGestion: acteGestion, codeAvn: codeAvn
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        } else {
            $.ajax({
                type: "POST",
                url: "/Cotisations/UpdateInPage/",
                context: $("#divCotisations"),
                data: {
                    codeOffre: codeOffre, version: version, type: type, argCotisations: argCotisations, argModelCommentForce: argModelCommentForce,
                    field: field, value: value, oldvalue: oldvalue, tabGuid: tabGuid,
                    saveCancel: $("#txtSaveCancel").val(), modeNavig: modeNavig, acteGestion: acteGestion, codeAvn: codeAvn
                },
                success: function (data) {

                    $(this).html(data);
                    toggleDescription($("#CommentForce"), true);
                    $("#CommentForce").html($("#CommentForce").html().replace(/&lt;br&gt;/ig, "\n"));
                    //FormatCLEditor($("#CommentForce"));
                    FormatDecimalValue();
                    MapElementPage();
                    CloseLoading();

                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }

    }
}
//-------------Fonction qui affiche ou ferme la div plein ecran du tableau de la liste de garanties--------
function OpenCloseFullScreenListGaranties(open) {
    if (open) {
        $("#divFullScreenListGaranties").show();
    }
    else $("#divFullScreenListGaranties").hide();

}


//filtre les garanties affichées
function FiltrerGaranties(onlyGarPorteuse) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var typePart = $("#rdPartTotale").is(":checked") ? "T" : $("#rdPartAlbingia").is(":checked") ? "A" : "T";
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Cotisations/FiltrerGaranties/",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, modeNavig: $("#ModeNavig").val(), onlyGarPorteuse: onlyGarPorteuse, typePart: typePart },
        success: function (data) {
            $("#divListGaranties").html(data);
            AlternanceLigne("GarantieList", "", false, null);
            AlternanceLigne("Total", "", false, null);
            var elts = $("span[albMask=\"decimal\"]");
            common.autonumeric.apply(elts, 'init', 'decimal', ' ', ',', 2, '99999999999.99', '-99999999999.99');
            //FormatDecimalValue();
            CloseLoading();

        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
    //divListGaranties
}

//Ouvre la div des frais accessoires
function OpenFraisAccessoires() {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var effetAnnee = $("#EffetAnnee").val();
    var tabGuid = $("#tabGuid").val();
    var acteGestion = $("#ActeGestion").val();
    var commentaire = "";
    ShowLoading();
    $.ajax({
        type: "POST",
        data: { codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, effetAnnee: effetAnnee, commentaire: commentaire, tabGuid: tabGuid, isReadonly: $("#OffreReadOnly").val(), modeNavig: $("#ModeNavig").val(), acteGestion: acteGestion, isModifHorsAvn: $("#IsModifHorsAvn").val() },
        url: "/Quittance/GetFraisAccessoires",
        success: function (data) {
            DesactivateShortCut();
            $("#divDataFraisAccessoires").html(data);
            AlbScrollTop();
            $("#divFraisAccessoires").show();
            FormatNumericValue();
            MapElementPageFraisAccessoires();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//------------Map elements de la div flottante frais accessoires---------
function MapElementPageFraisAccessoires() {
    $('#btnFermerFraisAccessoires').die();
    $('#btnFermerFraisAccessoires').click(function () {
        ReactivateShortCut();
        $("#divDataFraisAccessoires").html('');
        $("#divFraisAccessoires").hide();
    });
    $('#btnValiderFraisAccessoires').die();
    $('#btnValiderFraisAccessoires').click(function () {
        if ($("#TypeFrais").val() == "S" && $("#FraisStandards").attr("albModifStd") != "") {
            var modifFraisStd = $("#FraisStandards").attr("albModifStd");
            var fraisStd = $("#FraisStandards").val();

            modifFraisStd = "_" + modifFraisStd + "_";
            if (modifFraisStd.replace("_" + fraisStd + "_", "_") != modifFraisStd) {
                UpdateFraisAccessoires();
            }
            else {
                common.dialog.error("Erreur de frais standards<br/>Frais standards valides : " + $("#FraisStandards").attr("albModifStd").replace(/_/g, "€ ou ") + "€");
                return false;
            }
        }
        else {
            UpdateFraisAccessoires();
        }
    });

    if ($("#TypeFrais").val() == "P") {
        $("#FraisRetenus").removeAttr('readonly', 'readonly').removeClass('readonly');
    }
    else {
        if ($("#TypeFrais").val() == "S")
            $("#FraisRetenus").val($("#FraisStandards").val());
        if ($("#TypeFrais").val() == "N")
            $("#FraisRetenus").val(0);
        $("#FraisRetenus").attr('readonly', 'readonly').addClass('readonly');
    }
    TypeFraisChange();

    if (window.isReadonly) {
        $("#btnFermerFraisAccessoires").html("<u>F</u>ermer").attr("data-accesskey", "f");
        $("#btnValiderFraisAccessoires").hide();
    }

    AffectTitleList($("#TypeFrais"));
}

function UpdateFraisAccessoires() {
    var retourControl = true;
    $("#FraisRetenus").removeClass('requiredField');

    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var typeFrais = $("#TypeFrais").val();
    var fraisRetenus = $("#FraisRetenus").val();
    var taxeAttentat = $("#TaxeAttentat").is(':checked');
    var fraisSpecifiques = 0;
    var codeCommentaires = 0;
    var effetAnnee = $("#EffetAnnee").val();
    //$("#Commentaires").html($("#Commentaires").html().replace(/\n/ig, "<br>"));
    var commentaires = $("#CommentForce").html();
    var tabGuid = $("#tabGuid").val();
    var acteGestion = $("#ActeGestion").val();
    var onlyGarPorteuse = $("#chkGarantiePorteuse").is(":checked");

    //Contrôles
    if (typeFrais == "P" && fraisRetenus == "0") {
        ShowCommonFancy("Error", "", "Frais retenus doit être supérieur à 0", 1212, 700, true);
        return;
    }
    if (typeFrais == "S") {
        fraisRetenus = $("#FraisStandards").val();
    }
    if (fraisRetenus == "") {
        if (typeFrais == "S") $("#FraisStandards").addClass('requiredField');
        else $("#FraisRetenus").addClass('requiredField');
        retourControl = false;
    }

    var reguleId = $("#ReguleId").val();

    if (retourControl == false)
        return;
    ShowLoading();
    $.ajax({
        type: "POST",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, effetAnnee: effetAnnee,
            typeFrais: typeFrais, fraisRetenus: fraisRetenus, taxeAttentat: taxeAttentat/*, fraisSpecifiques: fraisSpecifiques*/,
            codeCommentaires: codeCommentaires, commentaires: commentaires, onlyGarPorteuse: onlyGarPorteuse, tabGuid: tabGuid, modeNavig: $("#ModeNavig").val(),
            acteGestion: acteGestion, reguleId: reguleId
        },
        url: "/Cotisations/UpdateFraisAccessoires",
        success: function (data) {
            $("#divCotisations").html(data);
            MapElementPage();
            $("#divDataFraisAccessoires").html('');
            $("#divFraisAccessoires").hide();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

function TypeFraisChange() {
    $("#TypeFrais").die();
    $("#TypeFrais").change(function () {
        AffectTitleList($(this));
        if ($("#TypeFrais").val() == "P") {
            $("#FraisRetenus").removeAttr('readonly', 'readonly').removeClass('readonly');
            $("#FraisStandards").attr('readonly', 'readonly').addClass('readonly');
        }
        else {
            if ($("#TypeFrais").val() == "S") {
                $("#FraisRetenus").val($("#FraisStandards").val());
                if ($("#FraisStandards").attr("albmodifstd").toLowerCase() != "") {
                    $("#FraisStandards").removeAttr('readonly', 'readonly').removeClass('readonly');
                }
            }
            if ($("#TypeFrais").val() == "N") {
                $("#FraisRetenus").val(0);
                $("#FraisStandards").attr('readonly', 'readonly').addClass('readonly');
            }
            $("#FraisRetenus").attr('readonly', 'readonly').addClass('readonly');
        }
    });
}

//--------Permet de switcher entre l'affichage part totale et part Albingia
function ChangeAffichagePart() {
    var typePart = $("#rdPartTotale").is(":checked") ? "T" : $("#rdPartAlbingia").is(":checked") ? "A" : "T";
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeAvn = $("#NumAvenantPage").val();
    var onlyGarPorteuse = $("#chkGarantiePorteuse").is(":checked");
    var acteGestion = $("#ActeGestion").val();
    var offrereadonly = $("#OffreReadOnly").val();
    //SAB 24/09/2015


    $.ajax({
        type: "POST",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, modeNavig: $("#ModeNavig").val(),
            acteGestion: acteGestion, onlyGarPorteuse: onlyGarPorteuse, typePart: typePart, isReadonly: offrereadonly
        },
        url: "/Cotisations/ReloadBandeauInferieur",
        success: function (data) {
            $("#divBandeauInferieur").html(data);
            MapElementPage();
            if (typePart == "A") {
                $("#TotalHorsFraisHT").addClass('readonly').attr('readonly', 'readonly');
                $("#TotalTTC").addClass('readonly').attr('readonly', 'readonly');
                $("#TotalHT").addClass('readonly').attr('readonly', 'readonly');
                $("#TauxForce").addClass('readonly').attr('readonly', 'readonly');
            }
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}