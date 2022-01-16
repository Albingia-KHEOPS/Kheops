$(document).ready(function () {
    formatDatePicker();
    MapBtnClick();
    AffectChangeDropDown();
    CloseFancy();
    MapPageElement();
});
//---------------------Affecte les fonctions au boutons-------------
function MapPageElement() {

    $("#btnAjouterBloc").die().live('click', function () {
        AddVBM('Bloc');
    });

    $("#btnAjouterVolet").die().live('click', function () {
        AddVBM('Volet');
    });

    $("#btnAjouterModele").die().live('click', function () {
        AddVBM('Modele');
    });

    $("#btnConfirmOk").live('click', function () {
        CloseCommonFancy();
        switch ($("#hiddenAction").val()) {
            case "Volet":
                DeleteVolet($("#hiddenInputId").val());
                $("#hiddenInputId").val('');
                break;
            case "Bloc":
                DeleteBloc($("#hiddenInputId").val());
                $("#hiddenInputId").val('');
                break;
            case "Modele":
                DeleteModele($("#hiddenInputId").val());
                $("#hiddenInputId").val('');
                break;
            case "Cancel":
                CancelForm();
                break;
        }
        $("#hiddenAction").val('');
    });
    $("#btnConfirmCancel").live('click', function () {
        CloseCommonFancy();
        $("#hiddenAction").val('');
    });
    common.autonumeric.applyAll('init', 'decimal');

}
//----------------------Formate tous les controles datepicker---------------------
function formatDatePicker() {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
}
//-----------Affecte les clicks sur les boutons ajouter---------
function MapBtnClick() {
    $("#btnVoletAdd").live('click', function () {
        EnregistrerVolet();
    });
    $("#btnBlocAdd").live('click', function () {
        EnregistrerBloc();
    });
    $("#btnModeleAdd").live('click', function () {
        EnregistrerModele();
    });
}
//--------------Changement de valeur pour les listes deroulantes----------
function AffectChangeDropDown() {
    $("#Branche").live('change', function () {
        AffectTitleList($(this));
        LoadCible($(this).val());        
    });
    $("#Volet").live('change', function () {
        RemoveClassRequired($(this));
        RemoveClassRequired($("#CaractereVolet"));
    });
    $("#CaractereVolet").live('change', function () {
        RemoveClassRequired($("#Volet"));
        RemoveClassRequired($(this));
    });
    $("#Bloc").live('change', function () {
        RemoveClassRequired($(this));
        RemoveClassRequired($("#CaractereBloc"));
    });
    $("#CaractereBloc").live('change', function () {
        RemoveClassRequired($("#Bloc"));
        RemoveClassRequired($(this));
    });
}
//--------------Enleve la class Required---------
function RemoveClassRequired(e) {
    e.removeClass("requiredField");
}
//--------------Ajoute la class Required----------
function AddClassRequired(e) {
    e.addClass("requiredField");
}
//------------Enregistre/MAJ Volet----------------
function EnregistrerVolet() {
    var codeId = $("#currentVoletGuid").val();
    var codeBranche = $("#Branche").val();
    var codeCible = $("#currentCible").val();
    var codeIdCible = $("#currentCibleGuid").val();
    var codeVolet = $.trim($("#Volet").val()); 
    var codeCaractere = $("#CaractereVolet").val();
    var ordreVolet = $("#inOrdreVolet").val();

    var isValid = true;
    if (codeVolet == "" || codeCaractere == "") {
        AddClassRequired($("#Volet"));
        AddClassRequired($("#CaractereVolet"));
        isValid = false;
    }

    var validCode = true;
    $("td[name=codeVolet]").each(function () {
        if ($.trim($(this).text()) == codeVolet && codeId == "") {
            validCode = false;
        }
    });

    $("td[name=ordreVolet]").each(function () {
        if ($.trim($(this).text()) + ",00" == ordreVolet && codeId == "") {
            AddClassRequired($("#inOrdreVolet"));
            isValid = false;
        }
    });

    if (codeId != "" && ordreVolet == "") {
        AddClassRequired($("#inOrdreVolet"));
        isValid = false;
    }

    if (codeId == "" && ordreVolet == "")
        ordreVolet = 0;

    if (!validCode) {
        AddClassRequired($("#Volet"));
        ShowFancy();
        isValid = false;
    }

    if (isValid) {
        $.ajax({
            type: "POST",
            url: "/AssocierVBM/EnregistrerVoletByCible",
            context: $("#divBodyVolet"),
            data: { codeId: codeId, codeBranche: codeBranche, codeCible: codeCible, codeIdCible: codeIdCible, codeVolet: codeVolet, codeCaractere: codeCaractere, ordreVolet: ordreVolet },
            success: function (data) {
                $("#divVolet").show();
                AfficheConsult($(this), data);
                AffecterClickVolet();
                ResetField("Volet");
                AlternanceLigne("Volet", "currentVolet", true, null);
                $("#divVoletAdd").hide();
            },
            error: function (request) {
                common.error.showXhr(request); CloseLoading();
            }
        });
    }
}
//-----------Enregistre/MAJ Bloc----------------
function EnregistrerBloc() {
    var codeId = $("#currentBlocGuid").val();
    var codeBranche = $("#Branche").val();
    var codeCible = $("#currentCible").val();
    var codeVolet = $("#currentVolet").val();
    var codeIdVolet = $("#currentVoletGuid").val();
    var codeBloc = $.trim($("#Bloc").val());
    var codeIdBloc = ""; // $("#currentBlocGuid").val();
    var codeCaractere = $("#CaractereBloc").val();
    var ordreBloc = $("#inOrdreBloc").val();

    var isValid = true;

    if (codeBloc == "" || codeCaractere == "") {
        AddClassRequired($("#Bloc"));
        AddClassRequired($("#CaractereBloc"));
        isValid = false;
    }

    var validCode = true;
    $("td[name=codeBloc]").each(function () {
        if ($.trim($(this).text()) == codeBloc && codeId == "") {
            validCode = false;
            isValid = false;
        }
    });


    $("td[name=ordreBloc]").each(function () {
        if ($.trim($(this).text()) + ",00" == ordreBloc && codeId == "") {
            AddClassRequired($("#inOrdreBloc"));
            isValid = false;
        }
    });

    if (codeId != "" && ordreBloc == "") {
        AddClassRequired($("#inOrdreBloc"));
        isValid = false;
    }

    if (codeId == "" && ordreBloc == "")
        ordreBloc = 0;


    if (!validCode) {
        AddClassRequired($("#Bloc"));
        ShowFancy();
        isValid = false;
    }

    if (isValid) {
        $.ajax({
            type: "POST",
            url: "/AssocierVBM/EnregistrerBlocByCible",
            context: $("#divBodyBloc"),
            data: { codeId: codeId, codeBranche: codeBranche, codeCible: codeCible, codeVolet: codeVolet, codeIdVolet: codeIdVolet, codeBloc: codeBloc, codeIdBloc: codeIdBloc, codeCaractere: codeCaractere, ordreBloc: ordreBloc },
            success: function (data) {
                $("#divBloc").show();
                AfficheConsult($(this), data);
                AffecterClickBloc();
                ResetField("Bloc");
                AlternanceLigne("Bloc", "currentBloc", true, null);
                $("#divBlocAdd").hide();
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    }
}
//-------------Enregistrer/MAJ Modele--------------------
function EnregistrerModele() {
    var codeId = $("#currentModeleGuid").val();
    var codeIdBloc = $("#currentBlocGuid").val();
    var dateApp = $("#DateAppli").val();
    var codeTypo = $("#Typologie").val();
    var codeModele = $.trim($("#Modele").val());

    if (codeModele == "" || dateApp == "" || codeTypo == "") {
        AddClassRequired($("#Modele"));
        AddClassRequired($("#DateAppli"));
        AddClassRequired($("#Typologie"));
        return false;
    }
    else {
        RemoveClassRequired($("#Modele"));
        RemoveClassRequired($("#DateAppli"));
        RemoveClassRequired($("#Typologie"));

    }

    //    var validCode = true;
    //    $("td[name=codeModele]").each(function () {
    //        if ($.trim($(this).text()) == codeModele && codeId == "") {
    //            validCode = false;
    //        }
    //    });

    //    if (!validCode) {
    //        AddClassRequired($("#Modele"));
    //        ShowFancy();
    //        return false;
    //    }

    $.ajax({
        type: "POST",
        url: "/AssocierVBM/EnregistrerModeleByCible",
        context: $("#divBodyModele"),
        data: { codeId: codeId, codeIdBloc: codeIdBloc, dateApp: dateApp, codeTypo: codeTypo, codeModele: codeModele },
        success: function (data) {
            $("#divModele").show();
            AfficheConsult($(this), data);
            AffecterClickModele();
            ResetField("Modele");
            AlternanceLigne("Modele", "currentModele", true, null);
            $("#divModeleAdd").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//------------Retour de la consultation bloc---------------
function AfficheConsult(e, data) {
    e.html(data);
    e.show();
}
//------------Charge les cibles en fonction de la branche------------
function LoadCible(val) {
    $.ajax({
        type: "POST",
        url: "/AssocierVBM/ChargerCible",
        context: $("#divBodyCibles"),
        data: { codeBranche: val },
        success: function (data) {
            $("#divAssociation").show();
            $("#divCible").show();
            $("#currentCible").val('');
            AfficheConsult($(this), data);
            AffecterClickCible();
            AlternanceLigne("Cible", "currentCible", true, null);
            $("#divVolet").hide();
            ResetField("Volet");
            $("#divBloc").hide();
            ResetField("Bloc");
            $("#divModele").hide();
            ResetField("Modele");
        }
    });
}
//--------------Affecte la fonction du click sur les td Code--------------
function AffecterClickCible() {
    $(".linkCat").each(function () {
        $(this).click(function () {
            LoadVolet($(this));
        });
    });
}

function AffecterClickCategorie() {
    $(".linkCat").each(function () {
        $(this).click(function () {
            LoadVolet($(this));
        });
    });
}
function AffecterClickVolet() {
    $(".linkVolet").each(function () {
        $(this).click(function () {
            LoadBloc($(this));
        });
    });
    $("img[name=modifVolet]").each(function () {
        $(this).click(function () {
            LoadUpdateVolet($(this));
        });
    });
    $("img[name=supprVolet]").each(function () {
        $(this).click(function () {
            $("#hiddenInputId").val($(this).attr('id').split("_")[1]);
            ShowCommonFancy("Confirm", "Volet", "Veuillez confirmer la suppression du volet", 350, 130, true, true);
        });
    });

    $("#Volet").die().live('change', function () {
        AffectTitleList($(this));
    });

    $("#CaractereVolet").die().live('change', function () {
        AffectTitleList($(this));
    });

}
function AffecterClickBloc() {
    $(".linkBloc").each(function () {
        $(this).click(function () {
            LoadModele($(this));
        });
    });
    $("img[name=modifBloc]").each(function () {
        $(this).click(function () {
            LoadUpdateBloc($(this));
        });
    });
    $("img[name=supprBloc]").each(function () {
        $(this).click(function () {
            $("#hiddenInputId").val($(this).attr('id').split("_")[1]);
            ShowCommonFancy("Confirm", "Bloc", "Veuillez confirmer la suppression du bloc", 350, 130, true, true);
        });
    });

    $("#Bloc").die().live('change', function () {
        AffectTitleList($(this));
    });

    $("#CaractereBloc").die().live('change', function () {
        AffectTitleList($(this));
    });
}
function AffecterClickModele() {
    $(".linkModele").each(function () {
        $(this).click(function () {

        });
    });
    $("img[name=modifModele]").each(function () {
        $(this).click(function () {
            LoadUpdateModele($(this));
        });
    });
    $("img[name=supprModele]").each(function () {
        $(this).click(function () {
            $("#hiddenInputId").val($(this).attr('id').split("_")[1]);
            ShowCommonFancy("Confirm", "Modele", "Veuillez confirmer la suppression du modèle", 350, 130, true, true);
        });
    });

    $("#Modele").die().live('change', function () {
        AffectTitleList($(this));
    });

    $("#Typologie").die().live('change', function () {
        AffectTitleList($(this));
    });
}
//--------------Vide les champs de maj/insert-----------------
function ResetField(e) {
    $("#current" + e).val('');
    $("#current" + e + "Guid").val('');
    $("#" + e).val('');
    if (e != "Modele")
        $("#Caractere" + e).val('');
    else {
        $("#DateAppli").val('');
        $("#Typologie").val('');
    }
    $("#inOrdreBloc").val('');
    $("#inOrdreVolet").val('');
}
//--------------Affiche les infos des volets----------------------------
function LoadVolet(e) {
    var codeId = e.attr('id').split("_")[1];
    var codeCible = e.find("input[name=CodeCible]").val();
    var codeBranche = $("#Branche").val();
    e.parent().parent().children(".selectLine").removeClass("selectLine");
    e.parent().addClass("selectLine");
    //    $("#currentCategorie").val(codeCategorie);
    //    $("#currentCategorieGuid").val(codeId);
    $("#currentCible").val(codeCible);
    $("#currentCibleGuid").val(codeId);
    $.ajax({
        type: "POST",
        url: "/AssocierVBM/ChargerVolet",
        context: $("#divBodyVolet"),
        data: { codeId: codeId, codeBranche: codeBranche },
        success: function (data) {
            $("#divVolet").show();
            AfficheConsult($(this), data);
            AffecterClickVolet();
            ResetField("Volet");
            AlternanceLigne("Volet", "currentVolet", true, null);
            $("#divBloc").hide();
            ResetField("Bloc");
            $("#divModele").hide();
            ResetField("Modele");
            $("#divVoletAdd").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Affiche les infos des blocs----------------------------
function LoadBloc(e) {
    var codeId = e.parent().attr('id').split("_")[1];
    var codeVolet = $.trim(e.parent().children("[name=codeVolet]").html());
    e.parent().parent().children(".selectLine").removeClass("selectLine");
    e.parent().addClass("selectLine");
    $("#currentVolet").val(codeVolet);
    $("#currentVoletGuid").val(codeId);
    $.ajax({
        type: "POST",
        url: "/AssocierVBM/ChargerBloc",
        context: $("#divBodyBloc"),
        data: { codeId: codeId },
        success: function (data) {
            $("#divBloc").show();
            AfficheConsult($(this), data);
            AffecterClickBloc();
            AlternanceLigne("Bloc", "currentBloc", true, null);
            $("#divModele").hide();
            ResetField("Modele");
            $("#divVoletAdd").hide();
            $("#divBlocAdd").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------Affiche les infos des modeles----------------------
function LoadModele(e) {
    var codeId = e.parent().attr('id').split("_")[1];
    var codeBloc = $.trim(e.parent().children("[name=codeBloc]").html());
    e.parent().parent().children(".selectLine").removeClass("selectLine");
    e.parent().addClass("selectLine");
    $("#currentBloc").val(codeBloc);
    $("#currentBlocGuid").val(codeId);
    $.ajax({
        type: "POST",
        url: "/AssocierVBM/ChargerModele",
        context: $("#divBodyModele"),
        data: { codeId: codeId },
        success: function (data) {
            $("#divModele").show();
            AfficheConsult($(this), data);
            AlternanceLigne("Modele", "currentModele", false, null);
            AffecterClickModele();
            $("#divVoletAdd").hide();
            $("#divBlocAdd").hide();
            $("#divModeleAdd").hide();
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//----------Charge les infos pour maj Volet------------
function LoadUpdateVolet(e) {
    $("#currentVoletGuid").val(e.attr('id').split("_")[1]);
    var codeVolet = $.trim(e.parent().parent().children("[name=codeVolet]").html());
    var codeCaractere = $.trim(e.parent().parent().children("[name=caractereVolet]").html());
    var ordre = $.trim(e.parent().parent().children("[name=ordreVolet]").html());

    $("#Volet").val(codeVolet);
    $("#CaractereVolet").val(codeCaractere);
    $("#inOrdreVolet").val(ordre);
    $("#divVoletAdd").show();

    $("#divBloc").hide();
    $("#divModele").hide();
}
//-----------Charge les infos pour maj Bloc------------
function LoadUpdateBloc(e) {
    $("#currentBlocGuid").val(e.attr('id').split("_")[1]);
    var codeBloc = $.trim(e.parent().parent().children("[name=codeBloc]").html());
    var codeCaractere = $.trim(e.parent().parent().children("[name=caractereBloc]").html());
    var ordreBloc = $.trim(e.parent().parent().children("[name=ordreBloc]").html());

    $("#Bloc").val(codeBloc);
    $("#CaractereBloc").val(codeCaractere);
    $("#inOrdreBloc").val(ordreBloc);
    $("#divBlocAdd").show();

    $("#divModele").hide();
}
//--------------Charge les infos pour maj Modele-------------
function LoadUpdateModele(e) {
    $("#currentModeleGuid").val(e.attr('id').split("_")[1]);
    var codeModele = $.trim(e.parent().parent().children("[name=codeModele]").html());
    var dateModele = $.trim(e.parent().parent().children("[name=dateModele]").html());
    var typologieModele = $.trim(e.parent().parent().children("[name=typologieModele]").html());

    $("#Modele").val(codeModele);
    $("#DateAppli").val(dateModele);
    $("#Typologie").val(typologieModele);
    $("#divModeleAdd").show();
}
//--------------Supprime un volet lie à une categorie-------------
function DeleteVolet(id) {
    var codeIdCible = $("#currentCibleGuid").val();
    var codeBranche = $("#Branche").val();
    $.ajax({
        type: "POST",
        url: "/AssocierVBM/SupprimerVolet",
        context: $("#divBodyVolet"),
        data: { codeId: id, codeBranche: codeBranche, codeIdCible: codeIdCible },
        success: function (data) {
            $("#divVolet").show();
            AfficheConsult($(this), data);
            AffecterClickVolet();
            ResetField("Volet");
            AlternanceLigne("Volet", "currentVolet", true, null);
            $("#divBloc").hide();
            ResetField("Bloc");
            $("#divModele").hide();
            ResetField("Modele");
            $("#divVoletAdd").hide();
            //            AfficheConsult($(this), data);
            //            AlternanceLigne("Volet", "currentVolet", true, null);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------------Supprime un bloc lié à un volet-------------
function DeleteBloc(id) {
    var codeIdVolet = $("#currentVoletGuid").val();
    $.ajax({
        type: "POST",
        url: "/AssocierVBM/SupprimerBloc",
        context: $("#divBodyBloc"),
        data: { codeId: id, codeIdVolet: codeIdVolet },
        success: function (data) {
            $("#divBloc").show();
            AfficheConsult($(this), data);
            AffecterClickBloc();
            AlternanceLigne("Bloc", "currentBloc", true, null);
            $("#divModele").hide();
            ResetField("Modele");
            $("#divVoletAdd").hide();
            $("#divBlocAdd").hide();
            //            AfficheConsult($(this), data);
            //            AlternanceLigne("Bloc", "currentBloc", true, null);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//---------------Supprime un modele lié à un bloc-------------
function DeleteModele(id) {
    var codeIdBloc = $("#currentBlocGuid").val();
    $.ajax({
        type: "POST",
        url: "/AssocierVBM/SupprimerModele",
        context: $("#divBodyModele"),
        data: { codeId: id, codeIdBloc: codeIdBloc },
        success: function (data) {
            $("#divModele").show();
            AfficheConsult($(this), data);
            AlternanceLigne("Modele", "currentModele", false, null);
            AffecterClickModele();
            $("#divVoletAdd").hide();
            $("#divBlocAdd").hide();
            $("#divModeleAdd").hide();
            //            AfficheConsult($(this), data);
            //            AlternanceLigne("Modele", "currentModele", true, null);
        },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
}
//--------------------Add VBM()-------------------
function AddVBM(e) {
    $("#div" + e + "Add").show();
    $("#current" + e + "Guid").val('');
    $("#" + e).val('');
    $("#Caractere" + e).val('');

    if (e == "Volet") {
        $("#divBloc").hide();
        $("#divModele").hide();
    }
    else if (e == "Bloc")
        $("#divModele").hide();
    else if (e == "Modele") {
        $("#DateAppli").val('01/01/1970');
        $("#Typologie").val('STD');
    }
}
//-------------------- Crée la boite de dialog pour le contexte.
function ShowFancy() {
    $.fancybox(
        $("#FancyAlert").html(),
        {
            'autodimension': false,
            'width': 120,
            'height': 'auto',
            'transitionIn': 'elastic',
            'transitionOut': 'elastic',
            'speedin': 400,
            'speedOut': 400,
            'easingOut': 'easeInBack',
            'modal': true
        }
    );
}
//------------------ Ferme la boîte de dialogue Fancy ----------------
function CloseFancy() {
    $("#btnOK").live('click', function () {
        $.fancybox.close();
    });
}

function Annuler() {
    window.location.href = "/BackOffice/Index";
}