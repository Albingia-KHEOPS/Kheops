$(document).ready(function () {
    MapElementPageHistorique();
});
//---------Map les éléments de la page-----------
function MapElementPageHistorique() {
    //AlternanceLigne("BodyListHisto", "", false, null);
    $("#inIsContractuel").die().live('click', function () {
        ReloadListHistorique($(this).is(":checked"));
    });
    MapElementListHisto();
    $("#btnRetourAccueilHisto").die().live('click', function () {
        RedirectionHisto("RechercheSaisie", "Index");
    });

    $("#btnFermerHisto").die().live('click', function () {
        if (!window.sessionStorage) return;
        var filter = JSON.parse(window.sessionStorage.getItem('recherche_filter'));
        if (!filter) return;
        $("#Offre_CodeOffre").val(filter.CodeOffre);
        $("#Offre_Version").val(filter.version);
        $("#Offre_Type").val(filter.TypeContrat);
        $("#divFullScreenVisuHistorique").hide();
        $("#divDataVisuHistorique").html("");
    });
}
//-------Map les éléments de la liste des historiques-----
function MapElementListHisto() {
    $("img[name='imgConsult']").kclick(function () {
        let typetraitemet = $(this).attr("albtypetraitement");
        if (typetraitemet == "REGUL" || typetraitemet == 'AVNRG') {
            RedirectionHisto("CreationRegularisation", "Index", $(this).attr("albnumint"), $(this).attr("albnumext"), $(this).attr("albtypetraitement"), $(this).attr("albreguleId"), "newWindow");
        }
        else {
            RedirectionHisto("AnInformationsGenerales", "Index", $(this).attr("albnumint"), $(this).attr("albnumext"), $(this).attr("albtypetraitement"), "", "newWindow");
        }
    });
    $("img[name='imgPrimes']").each(function () {
        $(this).click(function () {
            OpenPrimes('Toutes');
        });
    });
    $("img[name='imgRetour']").each(function () {
        $(this).click(function () {
            OpenRetours($(this).attr("albnumint"), $(this).attr("albtypetraitement"));
        });
    });
    $("img[name='imgDocument']").each(function () {
        $(this).click(function () {
            OpenDocuments($(this).attr("albnumint"), $(this).attr("albtypetraitement"));
        });
    });
    $("td[name='clickableColHisto']").each(function () {
        $(this).die();
        $(this).live("click", function () {
            var numAvnInter = $(this).attr("albnumint");
            if (numAvnInter == undefined || numAvnInter == "")
                return;
            OpenBandeauHisto(numAvnInter);
        });
    });
    AddOnHideBandeauHisto();
    AlternanceLigne("BodyListHisto", "noInput", true, null);
}
//-------Rechargement de la liste des historiques-------
function ReloadListHistorique(isContractuel) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Historique/ReloadList",
        data: { codeAffaire: $("#CodeAffaireHisto").val(), version: $("#VersionHisto").val(), type: $("#TypeHisto").val(), isContract: isContractuel },
        success: function (data) {
            $("#dvListHisto").html(data);
            MapElementListHisto();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------RedirectionHisto---------
function RedirectionHisto(cible, job, codeAvt, numExt, typeTraitement,reguleId, newWindow) {
    var addParam = "";
    if (codeAvt != "0" && typeTraitement != "AFFNV") {
        addParam = "addParam" + $("#globalTypeAddParamAvn").val() + "|||" + $("#globalTypeAddParamAvn").val() + "ID|" + codeAvt + "||AVNTYPE|" + typeTraitement + "||AVNIDEXTERNE|" + numExt + "addParam";
    }
  

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Historique/Redirection",
        data: {
            cible: cible, job: job,
            codeAffaire: $("#CodeAffaireHisto").val(), version: $("#VersionHisto").val(), type: $("#TypeHisto").val(),
            codeAvt: codeAvt, typeTraitement: typeTraitement,reguleId:reguleId, tabGuid: $("#homeTabGuid", window.parent.document).val(), addParam: addParam, newWindow: newWindow
        },
        success: function (data) {
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Ouvre la visualisation des primes--------
function OpenPrimes(typeQuittances) {

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Quittance/OpenVisualisationQuittances",
        data: { codeOffre: $("#CodeAffaireHisto").val(), codeAvn: "", version: $("#VersionHisto").val(), isEntete: true, typeQuittances: typeQuittances, modeNavig: "H" },
        success: function (data) {
            //desactivation des raccourcis claviers de l'écran appelant
            DesactivateShortCut();
            $("#divDataVisuListeQuittances").html(data);
            common.dom.pushForward("divDataVisuListeQuittances");
            AlbScrollTop();
            $("#divVisuListeQuittances").show();
            MapElementVisuQuittances();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-------Ouvre la visualisation des retours----------
function OpenRetours(codeAvt, typeTraitement) {
    var id = $("#CodeAffaireHisto").val() + "_" + $("#VersionHisto").val() + "_" + $("#TypeHisto").val() + "_" + codeAvt;
    var tabGuid = $("#tabGuid").val();
    var modeNavig = "H";
    var addParam = "";
    if (codeAvt != "0" && typeTraitement != "AFFNV") {
        addParam = "addParam" + $("#globalTypeAddParamAvn").val() + "|||" + $("#globalTypeAddParamAvn").val() + "ID|" + codeAvt + "||AVNTYPE|" + typeTraitement + "addParam";
    }

    id += addParam + "modeNavigHmodeNavig";

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/Retours/OpenRetours",
        data: { id: id, modeNavig: modeNavig },
        success: function (data) {
            $("#currentAvt").val(codeAvt);
            $("#dvDataFlotted").html(data).width(1050);
            common.dom.pushForward("dvDataFlotted");
            $("#dvFlotted").show();
            AlternanceLigne("RetoursCoAssBody", "", false, null);
            MapElementFlotted();
            CloseLoading();
            $("#btnSuivantRetours").die().live('click', function () {
                ValidRetours();
            });
            $("#TypeAccordPreneur").die().live('change', function () {
                $("#inDateRetour").removeAttr("disabled");
            });
            $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//----------Ouvre la visualisation des documents--------
function OpenDocuments(codeAvt, typeTraitement) {
    var id = $("#CodeAffaireHisto").val() + "_" + $("#VersionHisto").val() + "_" + $("#TypeHisto").val() + "_" + codeAvt;
    var tabGuid = $("#tabGuid").val();
    var addParam = "";
    if (codeAvt != "0" && typeTraitement != "AFFNV") {
        addParam = "addParam" + $("#globalTypeAddParamAvn").val() + "|||" + $("#globalTypeAddParamAvn").val() + "ID|" + codeAvt + "||AVNTYPE|" + typeTraitement + "addParam";
    }

    id += addParam + "modeNavigHmodeNavig";

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/SuiviDocuments/OpenViewDocBis",
        data: {
            displayType: "AFF",
            numAffaire: $("#CodeAffaireHisto").val(), version: $("#VersionHisto").val(), type: $("#TypeHisto").val(), avenant: codeAvt, userName: "", situation: "",
            modeNavig: $("#ModeNavig").val(), readOnly: $("#OffreReadOnly").val(), warning: false
        },
        success: function (data) {
            if (data != "") {
                $("#dvDataFlotted").html(data).width(1220);
                common.dom.pushForward("dvDataFlotted");
                $("#dvFlotted").show();
                MapElementFlotted();
                MapElementSuiviDoc();
                $("#btnSuivant").removeAttr("data-accesskey");
            }
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });

}
//----------Map les éléments de la div flottante--------
function MapElementFlotted() {
    $("#btnReeditSuiviDoc").hide().removeAttr("data-accesskey");

    $("#btnAnnulerRetours").die().live('click', function () {
        FermerRetours();
    });
    $("#btnAnnulerSuiviDoc").die().live('click', function () {
        $("#dvDataFlotted").html("")
        $("#dvFlotted").hide();
    });
}
//--------Validation des retours--------
function ValidRetours() {
    var erreurBool = false;

    var dateRetourPreneur = $("#inDateRetour").val();
    var typeAccordPreneur = $("#TypeAccordPreneur").val();
    var typeAccordPreneuActuel = $("#TypeAccordPreneurActuel").val();
    var listeLignesCoAss = SerialiserLignesCoAssureur();
    var isReglementRecu = $("#chkReglementRecu").attr("checked") == "checked";

    ResetErreurs();

    if (dateRetourPreneur != undefined && dateRetourPreneur != "" && dateRetourPreneur.split("/").length == 3) {
        var dNow = new Date()
        var iNow = dNow.getFullYear() * 10000 + (dNow.getMonth() + 1) * 100 + dNow.getDate();
        var iDateRetourPreneur = dateRetourPreneur.split("/")[2] * 10000 + dateRetourPreneur.split("/")[1] * 100 + dateRetourPreneur.split("/")[0] * 1;
        if (iDateRetourPreneur > iNow) {
            $("#inDateRetour").addClass('requiredField');
            $("#inDateRetour").attr('title', "La date ne peut être postérieure à la date du jour");
            erreurBool = true;
        }
    }

    if (dateRetourPreneur != undefined && dateRetourPreneur != "" && dateRetourPreneur.split("/").length == 3) {
        if (typeAccordPreneur == "N") {
            $("#TypeAccordPreneur").addClass('requiredField');
            $("#TypeAccordPreneur").attr('title', "Une date de retour a été saisie, vous devez choisir un accord");
            erreurBool = true;
        }
    }

    if (isReglementRecu) {
        if (typeAccordPreneur == "N" || typeAccordPreneur == "T") {
            $("#TypeAccordPreneur").addClass('requiredField');
            $("#TypeAccordPreneur").attr('title', "Un règlement a été reçu, le type d'accord sélectionné n'est pas compatible");
            erreurBool = true;
        }
    }
    else {
        if (typeAccordPreneur == "R") {
            $("#TypeAccordPreneur").addClass('requiredField');
            $("#TypeAccordPreneur").attr('title', "Aucun règlement n'a été reçu, le type d'accord sélectionné n'est pas compatible");
            erreurBool = true;
        }
    }

    if (typeAccordPreneuActuel == "R" && typeAccordPreneur != "S") {
        $("#TypeAccordPreneur").addClass('requiredField');
        $("#TypeAccordPreneur").attr('title', "Le type d'accord sélectionné n'est pas compatible, avec la situation actuelle");
        erreurBool = true;
    }

    if ((typeAccordPreneur != "N") && (dateRetourPreneur == undefined || dateRetourPreneur == "" || dateRetourPreneur.split("/").length != 3)) {
        $("#inDateRetour").addClass('requiredField');
        $("#inDateRetour").attr('title', "Une date d'accord valide doit être sélectionnée");
        erreurBool = true;
    }


    if (CheckLignesCoAss()) {
        erreurBool = true;
    }

    if (erreurBool) {
        return false;
    }
    else {
        ShowLoading();
        var codeContrat = $("#Offre_CodeOffre").val();
        var versionContrat = $("#Offre_Version").val();
        var typeContrat = $("#Offre_Type").val();
        var tabGuid = $("#homeTabGuid", window.parent.document).val();

        if (codeContrat === "" || codeContrat === undefined) {
            codeContrat = $("#inRetourCodeAffaire").val();
            versionContrat = $("#inRetourVersion").val();
            typeContrat = $("#inRetourType").val();
        }

        $.ajax({
            type: "POST",
            url: "/Retours/ValiderRetours/",
            data: {
                codeContrat: codeContrat, version: versionContrat, type: typeContrat, codeAvt: $("#currentAvt").val(), tabGuid: tabGuid,
                dateRetourPreneur: dateRetourPreneur, typeAccordPreneur: typeAccordPreneur,
                listeLignesCoAss: listeLignesCoAss, modeNavig: "H"
            },
            success: function (data) {
                $("#dvDataFlotted").html("");
                $("#dvFlotted").hide();
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//-----------Retire toutes les erreurs de l'écran et leurs messages
function ResetErreurs() {
    $("select[name=typeAccordInput]").removeClass('requiredField').attr("title", "");
    $("input[name=dateRetour]").removeClass('requiredField').attr("title", "");
}
//-----------fonction qui sérialise une opposition en fonction des champs de l'écran--------
function SerialiserLignesCoAssureur() {
    var chaine = '[';
    var hasLine = false;
    $("tr[name=lineCoass]").each(function () {
        var id = $(this).attr("id").split("_")[1];
        if (id != "" && id != undefined) {
            hasLine = true;
            chaine += '{';
            chaine += 'GuidId : "' + id + '",';
            chaine += 'SDateRetourCoAss : "' + $("#inDateRetourCoAss_" + id).val() + '",';
            chaine += 'TypeAccordVal : "' + $("#TypeAccordCoAss_" + id).val() + '"';
            chaine += '},';
        }

    });
    if (hasLine)
        chaine = chaine.substring(0, chaine.length - 1);
    chaine += ']';
    chaine = chaine.replace("\\", "\\\\");
    return chaine;
}
//----------Vérifie les valeurs des lignes des coassureurs------
function CheckLignesCoAss() {
    var dNow = new Date();
    var iNow = dNow.getFullYear() * 10000 + (dNow.getMonth() + 1) * 100 + dNow.getDate();
    var toReturn = false;
    $("tr[name=lineCoass]").each(function () {
        var id = $(this).attr("id").split("_")[1];
        if (id != "" && id != undefined) {
            var dateRetourCoAss = $("#inDateRetourCoAss_" + id).val();
            var typeAccordCoAss = $("#TypeAccordCoAss_" + id).val();
            if (dateRetourCoAss != undefined && dateRetourCoAss != "" && dateRetourCoAss.split("/").length == 3) {
                var idateRetourCoAss = dateRetourCoAss.split("/")[2] * 10000 + dateRetourCoAss.split("/")[1] * 100 + dateRetourCoAss.split("/")[0] * 1;
                if (idateRetourCoAss > iNow) {
                    $("#inDateRetourCoAss_" + id).addClass('requiredField');
                    $("#inDateRetourCoAss_" + id).attr('title', "La date ne peut être postérieure à la date du jour");
                    toReturn = true;
                }
                if (typeAccordCoAss == "N") {
                    $("#TypeAccordCoAss_" + id).addClass('requiredField');
                    $("#TypeAccordCoAss_" + id).attr('title', "Une date de retour a été saisie, vous devez choisir un accord");
                    toReturn = true;
                }
            }
            if (typeAccordCoAss != "N") {
                if (dateRetourCoAss == undefined || dateRetourCoAss == "" || dateRetourCoAss.split("/").length != 3) {
                    $("#inDateRetourCoAss_" + id).addClass('requiredField');
                    $("#inDateRetourCoAss_" + id).attr('title', "Une date d'accord valide doit être sélectionnée");
                    toReturn = true;
                }
            }

        }
    });

    return toReturn;
}


///#region gestion du bandeau


//----------Ouvre le bandeau du contrat en histo--------
function OpenBandeauHisto(codeAvt) {
    if ($("#isLoadingBandeauHisto").val() == 0) {
        var tId = $("#CodeAffaireHisto").val() + "_" + $("#VersionHisto").val() + "_" + $("#TypeHisto").val() + "_" + codeAvt;

        if ($("#selectedRowHisto").val() == '' && $("#oldSelectedRowHisto").val() != tId) {
            //if ($("#oldSelectedRowHisto").val() != tId) {
            var context = codeAvt;
            $("#selectedRowHisto").val(tId);
            $("#oldSelectedRowHisto").val($("#selectedRowHisto").val());
            GetBandeauHisto(tId, context);
        }
    }
}

function GetBandeauHisto(tId, context) {
    if (tId != undefined) {
        $("#isLoadingBandeauHisto").val("1");
        var modeNavig = "H";
        ShowLoading();
        $.ajax({
            type: "POST",
            url: "/RechercheSaisie/GetBandeau",
            data: { id: tId, modeNavig: modeNavig },
            success: function (data) {
                if (data != null && data != "") {
                    $("#divConteneurBandeauRechercheHisto").html(data);
                    var divHeight = $("#divInfoBandeauRecherche").height();
                    var divWidth = $("#divInfoBandeauRecherche").width();
                    $("#divInfoBandeauRecherche").css({ 'position': 'absolute', 'top': 20 + 'px', 'left': 84 + 'px' }).show();

                    $(".backgroundPink").each(function () {
                        $(this).removeClass('backgroundPink');
                    });
                    $("tr[albnumint=" + context + "]").addClass('backgroundPink');
                    MapLinkWinOpen();
                    $("#divConteneurBandeauRechercheHisto").show();
                    $("#isLoadingBandeauHisto").val("0");
                }
                CloseLoading();
            },
            error: function (request) {
                $("#isLoadingBandeauHisto").val("0");
            }
        });
    }
}

//----------fonction qui gère les évenements qui cachent le bandeau d'information
function AddOnHideBandeauHisto() {
    if ($("#divFullScreenVisuHistorique").attr("id") != undefined) {
        $("#divFullScreenVisuHistorique").unbind("click");
        $("#divFullScreenVisuHistorique").bind("click", function () {
            //$("#divInfoBandeauRecherche").hide();
            $("#divConteneurBandeauRechercheHisto").hide();
            $("#oldSelectedRowHisto").val($("#selectedRowHisto").val());
            $("#selectedRowHisto").val('');
            $(".backgroundPink").each(function () {
                $(this).removeClass('backgroundPink');
            });
        });
    }

    $("#divConteneurBandeauRechercheHisto").unbind("click");
    $("#divConteneurBandeauRechercheHisto").bind("click", function (e) {
        return false;
    });
}


///#endregion