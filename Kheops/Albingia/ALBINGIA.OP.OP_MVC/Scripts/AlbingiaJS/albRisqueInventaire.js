/// <reference path="Common/common.js" />
/// <reference path="albCommon.js" />

$(document).ready(function () {
    AlternanceLigne("TableFichier", "", false, null);
    MapPageElement();
    ChangeClass();
    AddSumFunc();
    GetInvKeys();
    CancelRow();
    SaveRow();
    AfficherMessageSauvegarde();
    FormatDecimalNumericValue();
});
//--------------------Télècharger un fichier---------------------

//---------------------Affecte les fonctions au boutons-------------
function MapPageElement() {
    $("#btnErrorOk").live('click', function () {
        CloseCommonFancy();
    });
    $("#btnAnnuler[albcontext=RisqueInventaire]").live('click', function () {
        ShowCommonFancy("Confirm", "Cancel",
            "Attention, en annulant vous perdrez les informations non enregistrées sur cet écran.<br/><br/>Confirmez-vous l’annulation ?<br/>",
            350, 130, true, true);
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
    });
    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "File":
                DeleteFile($("#hiddenInputId").val());
                $("#hiddenInputId").val('');
                break;
            case "Cancel":
                CancelForm();
                break;
        }
    });
    $("#imgDivCollapse").click(function () {
        ExpandCollapse();
    });
    
    MapElementTabInventaire();

    $("td[id^='divNatureLieu'], td[id^='divCodeMat']").each(function () { AffectTitleList($(this)); });

    $("#FullScreen[albContext='Inventaire']").die().live('click', function () {
        OpenFullScreenInven();
    });
    $("#btnFSAnnul").live('click', function () {
        if ($("#EcranProvenance").val() == "FormuleGarantie") {
            CloseFullScreenInven();
        }
        else {
            $("#btnSuivant").click();
        }
    });

    $("#btnSuivant[albcontext=RisqueInventaire]").live('click', function () {
        SaveInventaire();
    });
    if ($("#OffreReadOnly").val() != "True") {
        $("#Observations").html($("#Observations").html().replace(/&lt;br&gt;/ig, "\n"));
    }
    else {
        $("#btnAnnuler").html("<u>P</u>récédent").attr("data-accesskey", "p");
    }
    toggleDescription();

    $("#ActiverReport").live('change', function () {

    });

    AffectTitleList($("#UniteLst"));
    AffectTitleList($("#TypeLst"));
    AffectTitleList($("#invenUniteObjet"));
    AffectTitleList($("#invenTypeObjet"));
    AffectTitleList($("#TaxeLst"));

    if (!window.isModifHorsAvenant) { CheckNbRowInven() };
}
//------------Affiche/Cache la description de la ligne d'inventaire--------
function ToggleInvenDescription(elem) {
    var splitHtmlChar = $("#SplitHtmlChar").val();
    var idElem = elem.attr('id').split(splitHtmlChar)[1];
    var position = elem.offset();
    $("div[id!='divDescInven" + splitHtmlChar + idElem + "'][class='DescrInven']").hide();
    $("div[id='divDescInven" + splitHtmlChar + idElem + "']").css({ 'position': 'fixed', 'top': position.top + 20 + 'px', 'left': position.left - 335 + 'px' }).toggle();
}
//----------------------Formate tous les controles cleditor---------------------
//-------------Map les éléments du tableau d'inventaire-----------
function MapElementTabInventaire() {
    MapCommonAutoCompCPVille();

    var sep = $("#splitCharHtml").val();

    $("#dvLinkClose[albContext='Inventaire']").die().live('click', function () {
        CloseFullScreenInven();
    });

    $("#addInventaire").die().live('click', function () {
        if ($(this).hasClass("CursorPointer")) {
            $("#newInventaire").show();
            $("img[id=addInventaire]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
            if (parseInt($("#TypeInventaire").val()) > 2 && parseInt($("#TypeInventaire").val()) < 6)
                $("input[id='InventaireNom#**#-9999']").focus();
            else
                $("input[id='InventaireDesignation#**#-9999']").focus();
            if ($("#divFullScreen").isVisible()) {
                $("#btnFSAnnul").disable();
            }
            else {
                if ($("#EcranProvenance").val() == "FormuleGarantie")
                    $("#btnValiderInventaire").disable();
                else
                    $("#btnSuivant").disable();
            }
        }
    });

    $("img[name=saveInventaire], img[name=updateInventaire]").die().live('click', function () {
        SaveLineInventaire($(this).attr('id').split(sep)[1], false, null, false, false);
    });
    $("img[name=delInventaire]").die().live('click', function () {
        DeleteLineInventaire($(this).attr('id').split(sep)[1]);
    });

    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });

    AlternanceLigne("BodyInventaire", "", false, null);

    $("input[name!=InventaireHeureDeb][name!=InventaireHeureFin][type=text]").live('focus', function () {
        $("#divTimePicker").hide();
    });
    $("input[name=InventaireHeureDeb], input[name=InventaireHeureFin]").die().live('focus', function () {
        OpenDivTime($(this));
    });
    $("#btnValidTime").die().live('click', function () {
        CloseDivTime();
    });
    $("input[name=InventaireDateDeb]").live('change', function () {
        var elemId = $(this).attr('id').split(sep)[1];
        if ($(this).val() == "") {
            $("[id='InventaireHeureDeb" + sep + elemId + "']:input").clear();
        }
        else {
            if ($("[id='InventaireHeureDeb" + sep + elemId + "']:input").val() == "")
                $("[id='InventaireHeureDeb" + sep + elemId + "']:input").val("00:00");
        }
    });
    $("input[name=InventaireDateFin]").live('change', function () {
        var elemId = $(this).attr('id').split(sep)[1];
        if ($(this).val() == "") {
            $("input[id='InventaireHeureFin" + sep + elemId + "']").clear();
        }
        else {
            if ($("input[id='InventaireHeureFin" + sep + elemId + "']").val() == "")
                $("input[id='InventaireHeureFin" + sep + elemId + "']").val("23:59");
        }
    });
    $("td[name=lock]").click(function () {
        if (!window.isReadonly && !$("#IsAvnDisabled").hasTrueVal() && !window.isModifHorsAvenant) {
            var lineInventaire = $("tr[id=tr" + $(this).attr('id').split(sep)[1] + "]");
            if ($("#EditInventaireId").val() != lineInventaire.attr('id')) {
                EditMode(lineInventaire);
            }
        }
    });
    $("img[name=updateInventaire]").hide();

    $("#btnValidLst").die().live('click', function () {
        ValidLstCP($("input[id='InventaireCodePostal" + sep + $("#ligneId").val() + "']"));
    });
    $("#btnCancelLst").die().live('click', function () {
        CloseListDiv($("input[id='InventaireVille" + sep + $("#ligneId").val() + "']"), "");
    });

    $("img[name='descInventaire']").click(function () { ToggleInvenDescription($(this)); });

    var selects = [
        "select[name='row.NatureLieu']",
        "select[name='NatureLieu']",
        "select[name='row.CodeMateriel']",
        "select[name='CodeMateriel']"
    ]

    $(selects.join(", ")).each(function () { AffectTitleList($(this)); });

    $(selects.concat(["#TaxeLst"]).join(", ")).die().live('change', function () {
        AffectTitleList($(this));
    });

    $("tr[albNameContext='inventaire']").bind('mouseover', function () {
        var invenId = $(this).attr("albinvenId");
        $("#InvenContextMenuId").val(invenId);
        $("tr[albNameContext='inventaire']").attr("albcontextmenu", "N");
        $.contextMenu('destroy', 'tr[albcontextmenu=O');

        if ($("#EditInventaireId").val() == "tr" + invenId || $("#EditInventaireId").val() == "") {
            contextMenu.createContextMenuInventaire(invenId);
        }
    });

    contextMenu.createContextMenuHeaderInven();
}
//----------------Ouvre le div du TimePicker---------------
function OpenDivTime(elem) {
    $("#AlbTimeHours").val(elem.val().split(':')[0]);
    $("#AlbTimeMinutes").val(elem.val().split(':')[1]);
    $("#AlbTime").val(elem.val() + ":00");
    $("#idTimeInput").val(elem.attr('id'));
    var top = elem.position().top;
    var left = elem.position().left;
    $("#divTimePicker").css({ 'position': 'absolute', 'top': top + 24 + 'px', 'left': left + 'px' });
    $("#divTimePicker").show();
}
//----------------Ferme le div du TimePicker--------------
function CloseDivTime() {
    var elemId = $("#idTimeInput").val();
    var time = $("#AlbTime").val().substring(0, 5);
    $("input[id='" + elemId + "']").val(time);
    $("#idTimeInput").clear();
    $("#divTimePicker").hide();
}
//--------------Supprime la ligne d'inventaire---------------
function DeleteLineInventaire(elemCode) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RisqueInventaire/DeleteLineInventaire/",
        data: { codeInven: elemCode },
        success: function (data) {
            $("#tr" + elemCode).remove();
            AlternanceLigne("BodyInventaire", "", false, null);
            CloseLoading();
            CheckNbRowInven();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Clique sur le header du tableau d'inventaire-------
function ClicHeaderInven() {
    if ($("#newInventaire").isVisible()) {
        SaveLineInventaire("-9999", false, null, false, false);
    }
    else if ($("#EditInventaireId").val() != "") {
        SaveLineInventaire($("#EditInventaireId").val().replace('tr', ''), false, null, false, false);
    }
}
//----------------Sauvegarde l'inventaire--------------------
function SaveLineInventaire(elemCode, bool, elem, fullScreen, openFullScreenInven) {
    ShowLoading();
    var checkResult = CheckInventData(elemCode);
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeInven = $("#Code").val();
    var typeInven = $("#InventaireType").val();

    if (checkResult == "") {
        var splitCharHtml = $("#splitCharHtml").val();

        var typeInventaire = $("#InventaireType").val();
        if (typeInventaire == "18") {
            TransferInvenAdresseData(elemCode, true);
        }

        var dataRow = GetDataRow(elemCode);

        $.ajax({
            type: "POST",
            url: "/RisqueInventaire/SaveLineInventaire/",
            data: { codeOffre: codeOffre, version: version, type: type, codeInven: codeInven, typeInven: typeInven, data: dataRow },
            success: function (data) {
                var newLine = $(data);
                AffectTitleList($(newLine).find("td[id^='divNatureLieu']"));
                AffectTitleList($(newLine).find("td[id^='divCodeMat']"));
                if (elemCode < 0) {
                    //insertion de la nouvelle ligne dans le tableau après la ligne vide
                    $("#tblBodyInventaire tr:first").after(newLine);
                    ClearEmptyLine(elemCode);
                }
                else {
                    //remplacer l'inner html du tr
                    $("tr[id=tr" + elemCode + "]").html(newLine.html());
                }
                $("#newInventaire").hide();
                $("#EditInventaireId").clear();
                $("img[id=addInventaire]").attr('src', '/Content/Images/plusajouter1616.png').addClass("CursorPointer");
                if ($("#divFullScreen").isVisible()) {
                    $("#btnFSAnnul").enable();
                }
                else {
                    if ($("#EcranProvenance").val() == "FormuleGarantie")
                        $("#btnValiderInventaire").enable();
                    else
                        $("#btnSuivant").enable();
                }
                MapElementTabInventaire();
                CloseLoading();
                if (bool) {
                    EditMode(elem);
                }
                if (fullScreen) {
                    LoadInventaires(openFullScreenInven);
                }
                FormatDecimalNumericValue();
                $("#divTimePicker").hide();
                CheckNbRowInven();
                AffectDateFormat();
                LinkHexavia();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
    else {
        if (checkResult == "ERROR")
            checkResult = "";

        common.dialog.error("Veuillez corriger les champs erronés" + checkResult);
        CloseLoading();
    }
}
//-------------Vérifie les données de l'inventaire---------------
function CheckInventData(elemCode) {
    var toReturn = true;
    var toReturnStr = "";

    $(".requiredField").removeClass('requiredField');

    var typeInventaire = $("#InventaireType").val();
    var sep = $("#splitCharHtml").val();


    if (typeInventaire == "1" || typeInventaire == "2" || typeInventaire == "6" || typeInventaire == "7" || typeInventaire == "8" || typeInventaire == "9" || typeInventaire == "16" || typeInventaire == "18") {
        var designation = $("input[id='InventaireDesignation" + sep + elemCode + "']").val();
        var datedeb = $("input[id='InventaireDateDeb" + sep + elemCode + "']").val();
        var heuredeb = $("input[id='InventaireHeureDeb" + sep + elemCode + "']").val();
        var datefin = $("input[id='InventaireDateFin" + sep + elemCode + "']").val();
        var heurefin = $("input[id='InventaireHeureFin" + sep + elemCode + "']").val();

        if (typeInventaire == "2") {
            heuredeb = "00:00";
            heurefin = "00:00";
        }

        if (designation == "") {
            $("input[id='InventaireDesignation" + sep + elemCode + "']").addClass('requiredField');
            toReturn = false;
        }

        if (typeInventaire == "1" || typeInventaire == "2") {
            if (datedeb != "" && datedeb != undefined && $("#dateDebOffre").val() != "") {
                $("#currentHourDeb").val(heuredeb.split(':')[0]);
                $("#currentMinuteDeb").val(heuredeb.split(':')[1]);
                let checkDDO = checkDateHeure($("#dateDebOffre"), $("input[id='InventaireDateDeb" + sep + elemCode + "']"), $("#heureDebOffre"), $("#currentHourDeb"), $("#minuteDebOffre"), $("#currentMinuteDeb"));
                if (!checkDDO && $("input[id='InventaireDateDeb" + sep + elemCode + "']").val() != "") {
                    $("input[id='InventaireHeureDeb" + sep + elemCode + "']").addClass('requiredField');
                    toReturnStr += "<br/>La date d'entrée est incohérente avec la date d'effet de l'offre :<br/>" + $("#dateDebOffre").val() + " " + $("#heureDebOffre").val() + ":" + $("#minuteDebOffre").val();
                    toReturn = false;
                }
                $("#currentHourDeb").clear();
                $("#currentMinuteDeb").clear();
            }
            if (datedeb != "" && datedeb != undefined && $("#dateFinOffre").val() != "") {
                $("#currentHourDeb").val(heuredeb.split(':')[0]);
                $("#currentMinuteDeb").val(heuredeb.split(':')[1]);
                let checkDDO = checkDateHeure($("input[id='InventaireDateDeb" + sep + elemCode + "']"), $("#dateFinOffre"), $("#currentHourDeb"), $("#heureFinOffre"), $("#currentMinuteDeb"), $("#minuteFinOffre"));
                if (!checkDDO && $("input[id='InventaireDateDeb" + sep + elemCode + "']").val() != "") {
                    $("input[id='InventaireHeureDeb" + sep + elemCode + "']").addClass('requiredField');
                    toReturnStr += "<br/>La date d'entrée est incohérente avec la date de fin d'effet de l'offre :<br/>" + $("#dateFinOffre").val() + " " + $("#heureFinOffre").val() + ":" + $("#minuteFinOffre").val();
                    toReturn = false;
                }
                $("#currentHourDeb").clear();
                $("#currentMinuteDeb").clear();
            }
            if (datefin != "" && datefin != undefined && $("#dateFinOffre").val() != "") {
                $("#currentHourFin").val(heurefin.split(':')[0]);
                $("#currentMinuteFin").val(heurefin.split(':')[1]);
                var checkDFO = checkDateHeure($("input[id='InventaireDateFin" + sep + elemCode + "']"), $("#dateFinOffre"), $("#currentHourFin"), $("#heureFinOffre"), $("#currentMinuteFin"), $("#minuteFinOffre"));
                if (!checkDFO && $("input[id='InventaireDateFin" + sep + elemCode + "']").val() != "") {
                    $("input[id='InventaireHeureFin" + sep + elemCode + "']").addClass('requiredField');
                    toReturnStr += "<br/>La date de sortie est incohérente avec la date de fin d'effet de l'offre :<br/>" + $("#dateFinOffre").val() + " " + $("#heureFinOffre").val() + ":" + $("#minuteFinOffre").val();
                    toReturn = false;
                }
                $("#currentHourFin").clear();
                $("#currentMinuteFin").clear();
            }
            if (datefin != "" && datefin != undefined && $("#dateDebOffre").val() != "") {
                $("#currentHourFin").val(heurefin.split(':')[0]);
                $("#currentMinuteFin").val(heurefin.split(':')[1]);
                let checkDFO = checkDateHeure($("#dateDebOffre"), $("input[id='InventaireDateFin" + sep + elemCode + "']"), $("#heureDebOffre"), $("#currentHourFin"), $("#minuteDebOffre"), $("#currentMinuteFin"));
                if (!checkDFO && $("input[id='InventaireDateFin" + sep + elemCode + "']").val() != "") {
                    $("input[id='InventaireHeureFin" + sep + elemCode + "']").addClass('requiredField');
                    toReturnStr += "<br/>La date de sortie est incohérente avec la date de fin d'effet de l'offre :<br/>" + $("#dateDebOffre").val() + " " + $("#heureDebOffre").val() + ":" + $("#minuteDebOffre").val();
                    toReturn = false;
                }
                $("#currentHourFin").clear();
                $("#currentMinuteFin").clear();
            }
            if (datedeb != "" && datedeb != undefined && datefin != "" && datefin != undefined) {
                $("#currentHourDeb").val(heuredeb.split(':')[0]);
                $("#currentMinuteDeb").val(heuredeb.split(':')[1]);
                $("#currentHourFin").val(heurefin.split(':')[0]);
                $("#currentMinuteFin").val(heurefin.split(':')[1]);
                var checkDH = checkDateHeure($("input[id='InventaireDateDeb" + sep + elemCode + "']"), $("input[id='InventaireDateFin" + sep + elemCode + "']"), $("#currentHourDeb"), $("#currentHourFin"), $("#currentMinuteDeb"), $("#currentMinuteFin"));
                if (!checkDH) {
                    $("input[id='InventaireHeureDeb" + sep + elemCode + "']").addClass('requiredField');
                    $("input[id='InventaireHeureFin" + sep + elemCode + "']").addClass('requiredField');
                    toReturnStr += "<br/>Les dates d'entrée et de sortie ne sont pas cohérentes.";
                    toReturn = false;
                }
                $("#currentHourDeb").clear();
                $("#currentMinuteDeb").clear();
                $("#currentHourFin").clear();
                $("#currentMinuteFin").clear();
            }
        }
    }
    if (typeInventaire == "3" || typeInventaire == "4" || typeInventaire == "5") {
        var nom = $("input[id='InventaireNom" + sep + elemCode + "']").val();
        var datenais = $("input[id='InventaireDateNaissance" + sep + elemCode + "']").val();

        if (datenais != "" && datenais != undefined) {
            if (Date.parse(datenais) == null || !isDate(datenais)) {
                $("input[id='InventaireDateNaissance" + sep + elemCode + "']").addClass('requiredField');
                toReturn = false;
            }
        }

        if (nom == "") {
            $("input[id='InventaireNom" + sep + elemCode + "']").addClass('requiredField');
            toReturn = false;
        }
    }

    if (typeInventaire == "10" || typeInventaire == "11" || typeInventaire == "12") {
        var valDropDown = $("select[id='InventaireCodeMat" + sep + elemCode + "']").val();
        if (valDropDown == "") {
            $("select[id='InventaireCodeMat" + sep + elemCode + "']").addClass("requiredField");
            toReturn = false;
        }
    }

    if (typeInventaire == "13" || typeInventaire == "14" || typeInventaire == "15") {
        var natureMarchandise = $("input[id='InventaireNatureMarchandise" + sep + elemCode + "']").val();

        if (natureMarchandise == "") {
            $("input[id='InventaireNatureMarchandise" + sep + elemCode + "']").addClass('requiredField');
            toReturn = false;
        }
    }

    if (typeInventaire == "18") {
        var montant = $("input[id='InventaireMontant" + sep + elemCode + "']").val();
        var adresseBatiment = $("input[id='ContactAdresse_Batiment'][albcontext='" + elemCode + "']").val();
        var adresseNo = $("input[id='ContactAdresse_No'][albcontext='" + elemCode + "']").val();
        var adresseExtension = $("input[id='ContactAdresse_Extension'][albcontext='" + elemCode + "']").val();
        var adresseVoie = $("input[id='ContactAdresse_Voie'][albcontext='" + elemCode + "']").val();
        var adresseDistribution = $("input[id='ContactAdresse_Distribution'][albcontext='" + elemCode + "']").val();

        var adresse = adresseBatiment + adresseNo + adresseExtension + adresseVoie + adresseDistribution;

        if (montant == "") {
            $("input[id='InventaireMontant" + sep + elemCode + "']").addClass('requiredField');
            toReturn = false;
        }
        if (adresse == "") {
            $("div[name='btnAdresse'][albcontext='" + elemCode + "']").addClass('requiredField');
            toReturn = false;
        }
    }
    if (toReturn == false && toReturnStr == "")
        toReturnStr = "ERROR";
    return toReturnStr;
}
//---------------Récupère la chaine de paramètre------------------
function GetDataRow(elemCode) {
    var sep = $("#splitCharHtml").val();

    var dataRow = {
        "Code": elemCode,
        "Designation": "",
        "Lieu": "",
        "NatureLieu": "",
        "CodePostal": "",
        "Ville": "",
        "DateDeb": "",
        "HeureDeb": "",
        "DateFin": "",
        "HeureFin": "",
        "NbPers": "",
        "NbEvenement": "",
        "Montant": "",
        "Nom": "",
        "Prenom": "",
        "Fonction": "",
        "DateNaissance": "",
        "CapitalDeces": "",
        "CapitalIP": "",
        "AccidentSeul": false,
        "AvantProd": false,
        "NumSerie": "",
        "CodeMateriel": "",
        "CodeExtension": "",
        "Franchise": "",
        "AnneeNaissance": "",
        "CodeQualite": "",
        "CodeRenonce": "",
        "CodeRsqLoc": "",
        "Mnt2": "",
        "Contenu": "",
        "MatBur": "",
        "DescNatureMarchandise": "",
        "Modele": "",
        "Marque": "",
        "Immatriculation": "",
        "DescDepart": "",
        "DescDestination": "",
        "DescModalite": "",
        "Pays": "",
        "Adresse": {
            "NoChrono": 0,
            "Batiment": "",
            "No": 0,
            "Extension": "",
            "Voie": "",
            "Distribution": "",
            "CodePostal": "",
            "Ville": "",
            "CodePostalCedex": "",
            "VilleCedex": "",
            "Pays": "",
            "MatriculeHexavia": "",
            "Longitude": 0,
            "Latitude": 0
        }
    };

    setValue("Designation", "InventaireDesignation");
    setValue("Lieu", "InventaireLieu");
    setValue("NatureLieu", "InventaireNatureLieu");
    setValueOrDefault("CodePostal", "InventaireCodePostal", 0);
    setValue("Ville", "InventaireVille");
    setValue("DateDeb", "InventaireDateDeb");
    setValue("HeureDeb", "InventaireHeureDeb");
    setValue("DateFin", "InventaireDateFin");
    setValue("HeureFin", "InventaireHeureFin");
    setValueOrDefault("NbPers", "InventaireNbPers", 0);
    setValueOrDefault("NbEvenement", "InventaireNbEvenement", 0);
    setValueOrDefault("Montant", "InventaireMontant", 0);
    setValue("Nom", "InventaireNom");
    setValue("Prenom", "InventairePrenom");
    setValue("Fonction", "InventaireFonction");
    setValue("DateNaissance", "InventaireDateNaissance");
    setValue("AnneeNaissance", "InventaireAnneeNaissance");
    setValueOrDefault("CapitalDeces", "InventaireCapitalDeces", 0);
    setValueOrDefault("CapitalIP", "InventaireCapitalIP", 0);
    setValue("AccidentSeul", "InventaireAccidentSeul");
    setValue("AvantProd", "InventaireAvantProd");
    setValue("NumSerie", "InventaireNumSerie");
    setValue("CodeMateriel", "InventaireCodeMat");
    setValue("CodeExtension", "InventaireCodeExtension");
    setValue("Franchise", "InventaireFranchise");
    setValue("CodeQualite", "InventaireCodeQualite");
    setValue("CodeRenonce", "InventaireCodeRenonce");
    setValue("CodeRsqLoc", "InventaireCodeRsqLoc");
    setValueOrDefault("Mnt2", "InventaireMnt2", 0);
    setValueOrDefault("Contenu", "InventaireContenu", 0);
    setValueOrDefault("MatBur", "InventaireMatBur", 0);
    setValue("DescNatureMarchandise", "InventaireNatureMarchandise", true);
    setValue("Modele", "InventaireMarque", true);
    setValue("Marque", "InventaireModele", true);
    setValue("Immatriculation", "InventaireImmatriculation", true);
    setValue("DescDepart", "InventaireDepart", true);
    setValue("DescDestination", "InventaireDestination", true);
    setValue("DescModalite", "InventaireModalite", true);
    setValue("Pays", "InventairePays");

    setValueObject("Adresse", "NoChrono", "InventaireAdresseNoChrono", 0);
    setValueObject("Adresse", "Batiment", "InventaireAdresseBatiment");
    setValueObject("Adresse", "No", "InventaireAdresseNo");
    setValueObject("Adresse", "Extension", "InventaireAdresseExtension");
    setValueObject("Adresse", "Voie", "InventaireAdresseVoie");
    setValueObject("Adresse", "Distribution", "InventaireAdresseDistribution");
    setValueObject("Adresse", "CodePostal", "InventaireAdresseCodePostal");
    setValueObject("Adresse", "Ville", "InventaireAdresseVille");
    setValueObject("Adresse", "CodePostalCedex", "InventaireAdresseCodePostalCedex");
    setValueObject("Adresse", "VilleCedex", "InventaireAdresseVilleCedex");
    setValueObject("Adresse", "Pays", "InventaireAdressePays");
    setValueObject("Adresse", "MatriculeHexavia", "InventaireAdresseMatriculeHexavia", 0);
    setValueObject("Adresse", "Longitude", "InventaireAdresseLongitude", 0);
    setValueObject("Adresse", "Latitude", "InventaireAdresseLatitude", 0);

    return dataRow;


    function setValue(name, id, noEmpty) {
        id = id + sep + elemCode;
        let inpt = $("[id='" + id + "']:input");

        if (inpt.is(":checkbox") || inpt.is(":radio")) {
            if (inpt.isChecked()) {
                dataRow[name] = inpt.isChecked();
            }
            return;
        }

        let val = inpt.val();
        if (noEmpty && val == "") {
            val = defaultValue;
        }
        if (val !== undefined && !(noEmpty && val === "")) {
            dataRow[name] = val;
        }
    }
    function setValueOrDefault(name, id, defaultValue) {
        id = id + sep + elemCode;
        let val = $("[id='" + id + "']:input").val();
        if (defaultValue !== undefined) {
            if (val === undefined || val == "") {
                val = defaultValue;
            }
        }
        if (val !== undefined) {
            dataRow[name] = val;
        }
    }
    function setValueObject(name, subname, id, defaultValue) {
        id = id + sep + elemCode;
        let val = $("[id='" + id + "']:input").val();
        if (defaultValue !== undefined) {
            if (val === undefined || val == "") {
                val = defaultValue;
            }
        }
        if (val !== undefined) {
            dataRow[name][subname] = val;
        }
    }

}
//---------------Récupère la chaine de paramètre------------------
function TransferInvenAdresseData(elemCode, forSave) {
    var sep = $("#splitCharHtml").val();
    if (forSave) {
        $("[id='InventaireAdresseNoChrono" + sep + elemCode + "']").val($("input[id='ContactAdresse_NoChrono'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseBatiment" + sep + elemCode + "']").val($("input[id='ContactAdresse_Batiment'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseNo" + sep + elemCode + "']").val($("input[id='ContactAdresse_No'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseExtension" + sep + elemCode + "']").val($("input[id='ContactAdresse_Extension'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseVoie" + sep + elemCode + "']").val($("input[id='ContactAdresse_Voie'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseDistribution" + sep + elemCode + "']").val($("input[id='ContactAdresse_Distribution'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseCodePostal" + sep + elemCode + "']").val($("input[id='ContactAdresse_CodePostal'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseVille" + sep + elemCode + "']").val($("input[id='ContactAdresse_Ville'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseCodePostalCedex" + sep + elemCode + "']").val($("input[id='ContactAdresse_CodePostalCedex'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseVilleCedex" + sep + elemCode + "']").val($("input[id='ContactAdresse_VilleCedex'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdressePays" + sep + elemCode + "']").val($("input[id='ContactAdresse_Pays'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseMatriculeHexavia" + sep + elemCode + "']").val($("input[id='ContactAdresse_MatriculeHexavia'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseLongitude" + sep + elemCode + "']").val($("input[id='ContactAdresse_Longitude'][albcontext='" + elemCode + "']").val());
        $("[id='InventaireAdresseLatitude" + sep + elemCode + "']").val($("input[id='ContactAdresse_Latitude'][albcontext='" + elemCode + "']").val());

    } else {
        $("input[id='ContactAdresse_NoChrono'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseNoChrono" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_Batiment'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseBatiment" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_No'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseNo" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_Extension'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseExtension" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_Voie'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseVoie" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_Distribution'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseDistribution" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_CodePostal'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseCodePostal" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_Ville'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseVille" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_CodePostalCedex'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseCodePostalCedex" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_VilleCedex'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseVilleCedex" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_Pays'][albcontext='" + elemCode + "']").val($("[id='InventaireAdressePays" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_MatriculeHexavia'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseMatriculeHexavia" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_Longitude'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseLongitude" + sep + elemCode + "']").val());
        $("input[id='ContactAdresse_Latitude'][albcontext='" + elemCode + "']").val($("[id='InventaireAdresseLatitude" + sep + elemCode + "']").val());
    }
}
//----------------Nettoie la ligne d'insertion--------------
function ClearEmptyLine(elemCode) {
    var sep = $("#splitCharHtml").val();
    function resetField(idprefix, defaultValue) {
        if (defaultValue === undefined) {
            defaultValue = "";
        }
        let input = $("[id='" + idprefix + sep + elemCode + "']:input");
        if (input.is(":checkbox") || input.is(":radio")) {
            input.removeAttr("checked");
        } else {
            input.val(defaultValue);
        }
    }
    function resetAdressField() {
        $("input[id='ContactAdresse_NoChrono'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_No'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_Extension'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_Voie'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_Distribution'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_CodePostal'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_Ville'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_CodePostalCedex'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_VilleCedex'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_Pays'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_MatriculeHexavia'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_Latitude'][albcontext='" + elemCode + "']").val("");
        $("input[id='ContactAdresse_Longitude'][albcontext='" + elemCode + "']").val("");

        $("label[id='lblInfoNo'][albcontext='" + elemCode + "']").text("");
        $("label[id='lblInfoExtension'][albcontext='" + elemCode + "']").text("");
        $("label[id='lblInfoVoie'][albcontext='" + elemCode + "']").text("");
        $("label[id='lblInfoCodePostal'][albcontext='" + elemCode + "']").text("");
        $("label[id='lblInfoVille'][albcontext='" + elemCode + "']").text("");
    }
    ["InventaireDesignation",
        "InventaireLieu",
        "InventaireNatureLieu",
        "InventaireCodeMat",
        "InventaireCodePostal",
        "InventaireVille",
        "InventaireDateDeb",
        "InventaireHeureDeb",
        "InventaireDateFin",
        "InventaireHeureFin",
        "InventaireNom",
        "InventairePrenom",
        "InventaireFonction",
        "InventaireDateNaissance",
        "InventaireCapitalDeces",
        "InventaireCapitalIP",
        "InventaireAccidentSeul",
        "InventaireAvantProd",
        "InventaireNbPers",
        "InventaireNbEvenement",
        "InventaireMontant",
        "InventaireNumSerie",
        "InventaireFranchise",
        "InventaireCodeExtension",
        "InventaireAnneeNaissance",
        "InventaireCodeQualite",
        "InventaireCodeRenonce",
        "InventaireCodeRsqLoc",
        "InventaireMnt2",
        "InventaireContenu",
        "InventaireMatBur",
        "InventaireNatureMarchandise",
        "InventaireMarque",
        "InventaireModele",
        "InventaireImmatriculation",
        "InventaireDepart",
        "InventaireDestination",
        "InventaireModalite",
        "InventairePays",
        "InventaireAdresseNoChrono",
        "InventaireAdresseBatiment",
        "InventaireAdresseNo",
        "InventaireAdresseExtension",
        "InventaireAdresseVoie",
        "InventaireAdresseDistribution",
        "InventaireAdresseCodePostal",
        "InventaireAdresseVille",
        "InventaireAdresseCodePostalCedex",
        "InventaireAdresseVilleCedex",
        "InventaireAdressePays",
        "InventaireAdresseMatriculeHexavia",
        "InventaireAdresseLongitude",
        "InventaireAdresseLatitude"].forEach(function (x, i, arr) { resetField(x); });
    resetAdressField();
}
//---------------Passe en mode édition pour une ligne d'inventaire---------------
function EditMode(elem) {
    $("input").removeClass("requiredField");
    $("select").removeClass("requiredField");
    var inventaireId = elem.attr('id').replace('tr', '');

    if ($("#newInventaire").isVisible() || $("#EditInventaireId").val() != "") {
        ShowCommonFancy("Error", "", "Veuillez sauvegarder la ligne en cours de modification.", 300, 80, true, true, true, false);
        return;
    }

    var typeInventaire = $("#InventaireType").val();
    if (typeInventaire == "18") {
        TransferInvenAdresseData(inventaireId, false);
    }
    //change le src de l'img
    ChangeInvenImg(inventaireId, true);
    $("td[name=edit]").hide();
    $("td[name=lock]").show();
    elem.find("td[name=lock]").hide();
    elem.find("td[name=edit]").show();
    $("#EditInventaireId").val(elem.attr('id'));
    $("img[id=addInventaire]").attr('src', '/Content/Images/plusajouter_gris1616.jpg').removeClass("CursorPointer");
    if ($("#EcranProvenance").val() == "FormuleGarantie") {
        $("#btnValiderInventaire").disable();
    } else {
        $("#btnSuivant").disable();
    }
    FormatDecimalNumericValue();
}
//-------------Change l'image de la ligne d'inventaire------------
function ChangeInvenImg(invenId, bool) {
    var sepHtml = $("#splitCharHtml").val();
    if (bool) {
        $("img[id='delInventaire" + sepHtml + invenId + "']").hide();
        $("img[id='updateInventaire" + sepHtml + invenId + "']").show();
    }
    else {
        $("img[id='delInventaire" + sepHtml + invenId + "']").show();
        $("img[id='updateInventaire" + sepHtml + invenId + "']").hide();
    }
}
//-------------Ouverture du plein écran--------------------
function OpenFullScreenInven() {
    setFullScreenInventaireVisibility(true);

}
//-------------Fermeture du plein écran---------------------
function CloseFullScreenInven() {
    setFullScreenInventaireVisibility(false);
}
function setFullScreenInventaireVisibility(isOpening) {
    if ($("#newInventaire").isVisible()) {
        SaveLineInventaire("-9999", false, null, true, isOpening);

    }
    else if ($("#EditInventaireId").val() != "") {
        SaveLineInventaire($("#EditInventaireId").val().replace('tr', ''), false, null, true, isOpening);
    }
    else {
        LoadInventaires(isOpening);
    }
}

//---------------Charge les lignes de l'inventaire--------------
function LoadInventaires(fullScreen) {
    ShowLoading();
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeRisque = $("#CodeRisque").val();
    var codeObjet = $("#CodeObjet").val();
    var codeInven = $("#Code").val();
    var typeInven = $("#Type").val();
    var tabGuid = $("#tabGuid").val();
    var branche = $("#Branche").val();
    var cible = $("#Cible").val();
    var codeFormule = $("#CodeFormule").val();
    var codeGarantie = $("#CodeGarantie").val();
    var ecranProvenance = $("#EcranProvenance").val();
    var modeNavig = $("#ModeNavig").val();
    var codeAvn = $('#NumAvenantPage').val();

    $.ajax({
        type: "POST",
        url: "/RisqueInventaire/LoadInventaires",
        data: {
            codeOffre: codeOffre, version: version, type: type, codeAvn: codeAvn, codeRisque: codeRisque, codeObjet: codeObjet, codeInven: codeInven, typeInven: typeInven,
            fullScreen: fullScreen, tabGuid: tabGuid, branche: branche, cible: cible, codeFormule: codeFormule, codeGarantie: codeGarantie, ecranProvenance: ecranProvenance, modeNavig: modeNavig,
            isForceReadOnly: $("#IsForceReadOnly").val()
        },
        success: function (data) {
            if (fullScreen) {
                $("#divDataTab").html(data);
                $("#divDataInventaires").html('');
                AlbScrollTop();
                $("#divFullScreen").show();
                if ($("#divDescription").is(':visible'))
                    $("#divGray").height($(document).height());
            }
            else {
                $("#divDataInventaires").html(data);
                $("#divDataTab").html('');
                $("#divFullScreen").hide();
            }
            MapElementTabInventaire();
            FormatDecimalNumericValue();
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//-----------------Fonction Collapse/Expand Div Description--------------
function ExpandCollapse() {
    $("#divDescription").toggle();
    if ($("#divDescription").is(':visible')) {
        $("#imgDivCollapse").attr('src', '/Content/Images/collapse.png');
    }
    else {
        $("#imgDivCollapse").attr('src', '/Content/Images/expand.png');
    }
}
//------------------Enregistrement de l'inventaire---------------
function SaveInventaire() {
    ShowLoading();

    var descriptif = $("#Descriptif").val();
    var valReport = $("#Valeur").val();
    var unitReport = $("#UniteLst").val();
    var typeReport = $("#TypeLst").val();
    var taxeReport = $("#TaxeLst").val();
    var activeReport = $("#ActiverReport").isChecked();
    $(".requiredField").removeClass('requiredField');

    if (!window.isReadonly && !$("#IsAvnDisabled").hasTrueVal()) {
        if (descriptif == "") {
            $("#Descriptif").addClass('requiredField');
            CloseLoading();
            return false;
        }

        if (activeReport && ((valReport == "" && $("#CodePbmer").val() != "M" && $("#CodePbr").val() != "R") || unitReport == "" || typeReport == "")) {
            $("#Valeur").addClass('requiredField');
            $("#UniteLst").addClass('requiredField');
            $("#TypeLst").addClass('requiredField');
            CloseLoading();
            return false;
        }
        if ($("#UniteLst").val() == "D" && $("#TaxeLst").val() == "" && $("#CodePbmer").val() != "M" && $("#CodePbr").val() != "R") {
            $("#TaxeLst").addClass('requiredField');
            CloseLoading();
            return false;
        }

        if ($("#UniteLst").val() != "" && $("#Valeur").val() == "0,00" && $("#CodePbmer").val() != "M" && $("#CodePbr").val() != "R") {
            $("#Valeur").addClass('requiredField');
            CloseLoading();
            return false;
        }
    }
    let obj = $(':input').serializeObject(false, function (name, o) {
        let rg = /InformationsGenerales\.ListesNomenclatures\.Nomenclature\d+\.Nomenclature/g;
        if (rg.test(name)) {
            let opt = $("[name='" + name + "'] option:selected");
            o[name] = opt.exists() ? opt.text().split(' - ')[0].trim() : "";
            return true;
        }
        return false;
    });

    if ($("#Valeur").prop('disabled') == true) {
        obj.Valeur = valReport;
        obj.UniteLst = unitReport;
        obj.TypeLst = typeReport;
        obj.TaxeLst = taxeReport;
    }
    
    let formDataInit = JSON.stringify(obj);
    let formData = formDataInit.replace("TypeInventaire", "ListInventaires.TypeInventaire");
    if ($("#Offre_Type").val() == "P") {
        formData = formData.replace("Contrat.CodeContrat", "Offre.CodeOffre").replace("Contrat.VersionContrat", "Offre.Version").replace("Contrat.Type", "Offre.Type").replace("Offre.Branche", "Branche").replace("Contrat.AddParamType", "AddParamType").replace("Contrat.AddParamValue", "AddParamValue");
    }
    $.ajax({
        type: "POST",
        url: "/RisqueInventaire/SaveInventaire",
        data: formData,
        contentType: "application/json",
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
 
}
//----------------Redirection--------------
function Redirection(cible, job) {
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var codeRisque = $("#CodeRisque").val();
    var codeObjet = $("#CodeObjet").val();
    var tabGuid = $("#tabGuid").val();
    var modeNavig = $("#ModeNavig").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();
    var isForceReadOnly = $("#IsForceReadOnly").val();

    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/RisqueInventaire/Redirection",
        data: {
            cible: cible, job: job, codeOffre: codeOffre, version: version, type: type, codeRisque: codeRisque, codeObjet: codeObjet, tabGuid: tabGuid, modeNavig: modeNavig,
            addParamType: addParamType, addParamValue: addParamValue, isForceReadOnly: isForceReadOnly
        },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}


//----------------------Capte le check sur ActiverReport-----------------------
function ChangeActiverReport() {
    $("#ActiverReport").live('change', function () {
        DeleteClass($("#Valeur"), "requiredField");
        DeleteClass($("#UniteLst"), "requiredField");
        DeleteClass($("#UniteLst"), "requiredField");
        if (!$(this).isChecked()) {
            $("#Valeur").clear().addClass("ReadOnly").attr("readonly", "readonly");
            $("#UniteLst").clear().addClass("ReadOnly").disable();
            $("#UniteLst").clear().addClass("ReadOnly").disable();
        }
    });
}
//---------------------- Annulation de la form et retour à la recherche -------------------------
function CancelForm() {
    Redirection("DetailsObjetRisque", "Index");
}
//---------------------- Déclenchement du Submit -------------------------
function ClickSuivant(isTriggred) {
    isTriggred = typeof isTriggred !== 'undefined' ? isTriggred : false;
    if (!isTriggred) {
        return false;
    }
    var correctForm = true;
    if ($("#ActiverReport").is(':checked')) {
        if ($("#Valeur").val() == "") {
            $("#Valeur").addClass('requiredField');
            correctForm = false;
        }

        if ($("#UniteLst").val() == "") {
            $("#UniteLst").addClass('requiredField');
            correctForm = false;
        }
        if ($("#TypeLst").val() == "") {
            $("#TypeLst").addClass('requiredField');
            correctForm = false;
        }
    }
    if (correctForm) {
        ShowLoading();
        TriggerSubmit(isTriggred);
    }
    else {
        return false;
    }
}

function TriggerSubmit(isTriggred) {

    if (isTriggred) {
        var formData = JSON.stringify($(':input').serializeObject());
        $.ajax({
            type: "POST",
            url: "/RisqueInventaire/RisqueInventaireEnregistrer",
            context: $("#JQueryHttpPostResultDiv"),
            data: formData,
            contentType: "application/json",
            dataType: "json",
            success:
                function (data) {
                    var dialogBox = $(this);
                    $(dialogBox).hide();
                    if (data.ActionTag == "Nav") {
                        $(this).append(data.Script);
                    }
                },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}

//----------------------Attache la fonction de suppr de la classe requiredField----------------------
function ChangeClass() {
    $("#Descriptif").live("change", function () {
        DeleteClass($(this), "requiredField");
    });
    $("#Valeur").live("change", function () {
        if ($(this).val() != "") {
            $("#ActiverReport").attr('checked', 'checked');
            DeleteClass($(this), "requiredField");
        }
    });
    $("#UniteLst").live("change", function () {
        $("#ActiverReport").attr('checked', 'checked');
        DeleteClass($(this), "requiredField");
        if ($("#UniteLst").val() == "%") {
            if (parseInt($("#Valeur").autoNumeric('get')) > 100)
                $("#Valeur").val('');
            //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
            common.autonumeric.apply($("#Valeur"), 'update', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');
        }
        else {
            //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
            common.autonumeric.apply($("#Valeur"), 'update', 'decimal', ' ', ',', 2, '99999999999.99', '0.00');
        }
        AffectTitleList($(this));
    });
    $("#TypeLst").live("change", function () {
        $("#ActiverReport").attr('checked', 'checked');
        DeleteClass($(this), "requiredField");
        AffectTitleList($(this));
    });
}
//----------------------Supprimer la classe requiredField----------------------
function DeleteClass(e, classe) {
    e.removeClass(classe);
}
//----------------------Attache la fonction de calcul de la somme des budgets----------------------
function AddSumFunc() {
    $("#btnSum").click(function () {
        SumValue();
    });
}
//----------------------Calcul de la somme des budgets----------------------
function SumValue() {
    var codeInventaireVal = $("#Code").val();
    var typeInventaire = $("#TypeInventaire").val();
    $.ajax({
        url: "/RisqueInventaire/CalculBudget",
        data: { codeInventaire: codeInventaireVal, typeInventaire: typeInventaire },
        success: function (data) {
            if (parseFloat(data) > 99999999999.99) {
                ShowCommonFancy("Error", "", "Le calcul ne peut être effectué, la somme des inventaires est supérieure à la valeur maximale", 380, 85, true, true);
                return false;
            }
            $("#Valeur").autoNumeric("set", data);
            $("#Valeur").trigger("change");
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}

//-----------------------Grille Inventaire---------------------------------
function GetGridInventaire() {
    return 'OK';
}

//-----------------------retourne les relatif à l'inventaire---------------------------------
function GetInvKeys() {
    $("#codeInv").val();
}

//---------------- FONCTIONS DE LA JQGRID ----------------------
$.lastSelectedRow = null;       //variable sauvegardant la ligne en sélection

//-------------------- Edition de la grille.
$.jqGridOnSelectRow = function (currentSelectedRow) {
    if (currentSelectedRow && currentSelectedRow != $.lastSelectedRow) {
        //save changes in row
        $('#DetailsInventaireGrid').jqGrid('restoreRow', $.lastSelectedRow, false);
        $.lastSelectedRow = currentSelectedRow;
    }
    //trigger inline edit for row
    $('#DetailsInventaireGrid').jqGrid('editRow', $.lastSelectedRow, false);

    //datepicker
    $("input[name='DateDebut'],input[name='DateFin']").addClass('datepicker');

    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
    AffectDateFormat();
    $("input[name='Descriptif']").focus();
    var budget = $("input[name='Budget']").val();
    $("input[name='Budget']").val(budget.replace(".", ","));

    $("input[name='NbEvent']").bind('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);

        if (code >= 42 && code <= 47) {
            e.preventDefault();
        }
    });

};
//-----------------------Annuler les modifications de la ligne---------------------------
function CancelRow() {
    $("#btnCancel").live("click", function () {
        $('#DetailsInventaireGrid').jqGrid('restoreRow', $.lastSelectedRow, false);
    });
}
//-----------------------Enregistre la ligne des objets ---------------------------------
function SaveRow() {
    $("#btnAdd").live("click", function () {
        if ($.lastSelectedRow != null) {
            if ($("#Descriptif").val() != "") {

                var valNumero = $('#' + $.lastSelectedRow + '_NumeroLigne').val();

                $("#DetailsInventaireGrid").jqGrid("saveRow", $.lastSelectedRow, { numeroLigne: valNumero });
                $("#DetailsInventaireGrid").trigger("reloadGrid");
            }
            else {
                $("#Descriptif").addClass('requiredField');
            }
        }
        else {
            ShowCommonFancy("Error", "", "Veuillez éditer une ligne.", 'auto', 'auto', true, true);
        }
    });
}

//---------------- FIN ZONE FONCTIONS DE LA JQGRID ----------------------



//-------------------------Message de sauvegarde d'une ligne de la grille inventaire------------------
function AfficherMessageSauvegarde() {
    $("#dvMessage").html("La ligne inventaire est sauvegardé");
    $('#lnkMessagePopup').trigger('click');
    $("#dvMessage").fancybox({
        'titlePosition': 'inside',
        'transitionIn': 'none',
        'transitionOut': 'elastic',
        'speedOut': 300
    });
}
//-------------------------Message de sauvegarde d'une ligne de la grille inventaire------------------
function AfficherMessageErreur() {
    $("#dvMessage").html("Erreur de sauvegarde de la ligne inventaire");
    $('#lnkMessagePopup').trigger('click');
    $("#dvMessage").fancybox({
        'titlePosition': 'inside',
        'transitionIn': 'none',
        'transitionOut': 'elastic',
        'speedOut': 300
    });
}

//-------Formate les input/span des valeurs----------
function FormatDecimalNumericValue() {
    //2017-02-03 : formatage de l'autoNumeric en appelant la fonction commune
    common.autonumeric.applyAll('init', 'numeric', ' ', null, null, '9999999999999', '0');
    common.autonumeric.applyAll('init', 'pourcentnumeric', '', null, null, '100', '0');
    common.autonumeric.applyAll('init', 'decimal', ' ', ',', 2, '99999999999.99', '0.00');
    common.autonumeric.applyAll('init', 'pourcentdecimal', '', ',', 2, '100.00', '0.00');

    common.autonumeric.apply($("#Valeur"), 'init', 'decimal', ' ', null, null, '99999999999.99', null);

    const RS_Manif_Assurées = "1";
    const RS_Pers_Dési_Indispo = "3";

    if ($("#InventaireType").val() == RS_Pers_Dési_Indispo)
        common.autonumeric.apply($("input[name='InventaireAnneeNaissance']"), 'init', 'yearnumeric', '', null, 0, '9999', '0');
    if ($("#InventaireType").val() == RS_Manif_Assurées)
        common.autonumeric.apply($("input[name='InventaireCodePostal']"), 'init', 'cpnumeric', '', null, 0, '99999', '0');

}


//---------Compte le nombre de ligne d'inventaire----------
function CheckNbRowInven() {
    if ($("tr[name=rowInventaire]").length <= 0 || $("td[id^=divMontant]").length <= 0) {
        $("#btnSum,#Valeur,#UniteLst,#TypeLst,#TaxeLst,#ActiverReport").disable();
        $("#ActiverReport").removeAttr("checked");
    }
    else {
        $("#btnSum,#Valeur,#UniteLst,#TypeLst,#TaxeLst,#ActiverReport").enable();
    }
}