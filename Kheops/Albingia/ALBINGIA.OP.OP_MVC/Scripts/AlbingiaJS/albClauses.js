$(document).ready(function () {
 
    rechercherClause("", "", "", "", "");
    AlimSelectBranche();
    document.getElementById("SelectBanche").addEventListener("change", OnChangeAlim);
    

});

$("#enregistrerClause").live('click', function () {
    var risqueLast = $(this).attr("name");
    var id = risqueLast.split("_")[1];
    var codeBranche = document.getElementById("SelectBancheModif_"+id);
    var codeCible = document.getElementById("SelectCibleModif_" + id);
    var Nm1 = document.getElementById("Nm1Modif_" + id);
    var Nm2 = document.getElementById("Nm2Modif_" + id);
    var Nm3 = document.getElementById("Nm3Modif_" + id);
    var LibelleClause = document.getElementById("LibelleClauseModif_" + id);
    if (LibelleClause.value != "") {
        
        ModifierLibelle(codeBranche.innerText, codeCible.innerText, Nm1.innerText, Nm2.innerText, Nm3.innerText, LibelleClause.value);   
        $("#ModifIsChekedVal").val("0");
    }
    else {
        $("LibelleClauseModif_" + id).addClass('requiredField');
        $("LibelleClauseModif_" + id).attr('title', "Champs obligatoire!!");
    }

});

$("#enregistrerClauseNew").live('click', function () {
    var codeBranche = document.getElementById("SelectBancheNew");
    var codeCible = document.getElementById("SelectCibleNew");
    var Nm1 = document.getElementById("Nm1");
    var Nm2 = document.getElementById("Nm2");
    var Nm3 = document.getElementById("Nm3");
    var LibelleClause = document.getElementById("LibelleClause");
    
    if (codeBranche.value != "" && codeCible.value != "" && Nm1.value != "" && Nm2.value != "" && Nm3.value != "" && LibelleClause.value != "" ) {
       
        SaveLibelle(codeBranche.value.split("-")[0], codeCible.value.split("-")[0], Nm1.value, Nm2.value, Nm3.value, LibelleClause.value);

    }
    else {
        Verif_champs(codeBranche.value, codeCible.value, Nm1.value, Nm2.value, Nm3.value, LibelleClause.value);
}
        
});

$("#btnRechercherClause").live('click', function () {
    var codeBranche = document.getElementById("SelectBanche");
    var codeCible = document.getElementById("SelectCible");
    var Nm1 = document.getElementById("RechercheNm1");
    var Nm2 = document.getElementById("RechercheNm2");
    var Nm3 = document.getElementById("RechercheNm3");
    $("#AjoutIsChekedVal").val("0");
    $("#ModifIsChekedVal").val("0");
    rechercherClause(codeBranche.value.split("-")[0], codeCible.value.split("-")[0], Nm1.value, Nm2.value, Nm3.value);
   
});

$("#btnAjouterClause").live('click', function () {
    
    if ($("#ModifIsChekedVal").val() == 0 && $("#AjoutIsChekedVal").val() == 0)   {
        insererLigne_Debut();
    }
    else {
        alert("Action déjà en cours");
    }
});

$("#cancelModifClause").live('click', function () {
    var n = ($(this).attr("name").split("_")[1]);
    var Ligne = parseInt(n) + 1;
    cancelLigne_indexN(n);
    supprimerLigne_indexN(Ligne);
    $("#ModifIsChekedVal").val("0");
});



$("#modifierClause").live('click', function () {
    if ($("#ModifIsChekedVal").val() == 0 && $("#AjoutIsChekedVal").val() == 0 )   {
        var n = ($(this).attr("name").split("_")[1]);
        var Ligne = parseInt(n) + 1;
        insererLigne_indexN(n);
        supprimerLigne_indexN(Ligne)
       
    } else {
        alert("Action déjà en cours");
    }
});

$("#suprimerClause").live('click', function () {
    if ($("#ModifIsChekedVal").val() == 0 && $("#AjoutIsChekedVal").val() == 0) {
        var risqueLast = $(this).attr("name");
        var id = risqueLast.split("_")[1];
        $("#SuppLigneLibelle").val(id);
        ShowCommonFancy("Confirm", "DellLibell", "Etes-vous sûr de vouloir supprimer ce Libelle ?", 350, 80, true, true);
    } else {
        alert("Action déjà en cours");
    }
});
$("#btnConfirmOk").die().live('click', function () {
    
    var id = $("#SuppLigneLibelle").val();
    var codeBranche = document.getElementById("SelectBancheModif_" + id);
    var codeCible = document.getElementById("SelectCibleModif_" + id);
    var Nm1 = document.getElementById("Nm1Modif_" + id);
    var Nm2 = document.getElementById("Nm2Modif_" + id);
    var Nm3 = document.getElementById("Nm3Modif_" + id);
    var LibelleClause = document.getElementById("LibelleClauseModif_" + id);
  
    SupprimerLibelle(codeBranche.innerText, codeCible.innerText, Nm1.innerText, Nm2.innerText, Nm3.innerText, LibelleClause.innerText, id);
    CloseCommonFancy();
});
$("#btnConfirmCancel").die().live('click', function () {
    CloseCommonFancy();
});
$("#suprimerClauseNew").die().live('click', function () {
    var risqueLast = $(this).attr("name");
    var id = risqueLast.split("_")[1];
    supprimerLigne_indexN(id)
});

function Verif_champs(codeBranche, codeCible, Nm1, Nm2, Nm3, LibelleClause) {

    if (codeBranche=== "") {
        $("#SelectBancheNew").addClass('requiredField');
    } 

    if (codeCible=== "") {
        $("#SelectCibleNew").addClass('requiredField');
    } 

    if (Nm1== "") {
        $("#Nm1").addClass('requiredField');
        $("#Nm1").attr('title', "Champs obligatoire!!");
    } 

    if (Nm2 === "") {
        $("#Nm2").addClass('requiredField');
        $("#Nm2").attr('title', "Champs obligatoire!!");
    } 

    if (Nm3=== "") {
        $("#Nm3").addClass('requiredField');
        $("#Nm3").attr('title', "Champs obligatoire!!");
    } 

    if (LibelleClause=== "") {
        $("#LibelleClause").addClass('requiredField');
        $("#LibelleClause").attr('title', "Champs obligatoire!!");
    } 

}
function insererLigne_Debut() {

    var cell, ligne;
    var tableau = document.getElementById("tblBloc");
    ligne = tableau.insertRow(0); 
    var n = 0;
    $("#AjoutIsChekedVal").val("1");
    // création et insertion des cellules dans la nouvelle ligne créée
    cell = ligne.insertCell(0);
    cell.innerHTML = '<select class="SelectBanche" id="SelectBancheNew" style="width:76px;" ><option selected></option></select>';
    //cell.innerHTML = '<td class="linkBloc bBranche"><input style="width:76px;" type="text" value=""></td >';

    cell = ligne.insertCell(1);
    cell.innerHTML = '<select class="SelectBanche" id="SelectCibleNew" style="width:76px;" ><option selected></option></select>';
    //cell.innerHTML = '<td class="linkBloc bCible"><input style=" width:76px;" type="text" value=""></td >';

    cell = ligne.insertCell(2);
    cell.innerHTML = '<td class="linkBloc col1"><input id="Nm1" style=" width:105px;" type="text" value=""></td >';

    cell = ligne.insertCell(3);
    cell.innerHTML = '<td class="linkBloc col2"><input id="Nm2" style=" width:110px;" type="text" value=""></td >';

    cell = ligne.insertCell(4);
    cell.innerHTML = '<td class="linkBloc col3"><input id="Nm3" style=" width:110px;" type="text" onkeypress="return isNumber(event);" maxlength="5"></td >';

    cell = ligne.insertCell(5);
    cell.innerHTML = '<td class="linkBloc col34"><input id="LibelleClause"  style=" width:398px;" type="text" value=""></td >';

    cell = ligne.insertCell(6);
    cell.innerHTML = '<td><center><img id="enregistrerClauseNew" name="save_' + n + '" src="/Content/Images/Save_16.png" class="CursorPointer" /> <img id="suprimerClauseNew" name="cancel_' + n + '" src="/Content/Images/reset.png" class="CursorPointer"></center></td >';

    AlimSelectBrancheNew();
    
    document.getElementById("SelectBancheNew").addEventListener("change", OnChangeAlimNew);


}

function isNumber(e) {
    e = e || window.event;
    var charCode = e.which ? e.which : e.keyCode;
    return /\d/.test(String.fromCharCode(charCode));
} 

function insererLigne_indexN(n) {
    var cell, ligne;

    // on récupère l'identifiant (id) de la table qui sera modifiée
    var tableau = document.getElementById("tblBloc");
    // nombre de lignes dans la table (avant ajout de la ligne)
    var Ligne = parseInt(n)  + 1;

    $("#ModifIsChekedVal").val("1");
   
    ligne = tableau.insertRow(n); // création d'une ligne à la position

    // création et insertion des cellules dans la nouvelle ligne créée
    cell = ligne.insertCell(0);
    cell.innerHTML = '<td class="linkBloc bBranche"><Label id="SelectBancheModif_'+ n +'" style=" width:76px;">' + tableau.rows[Ligne].cells[0].innerText+'</label></td >';

    cell = ligne.insertCell(1)
    cell.innerHTML = '<td class="linkBloc bCible"><Label id="SelectCibleModif_' + n +'"  style=" width:76px;">' + tableau.rows[Ligne].cells[1].innerText +'</label></td >';

    cell = ligne.insertCell(2);
    cell.innerHTML = '<td class="linkBloc col1"><Label id="Nm1Modif_' + n +'"  style=" width:82px;">' + tableau.rows[Ligne].cells[2].innerText +'</label></td >';

    cell = ligne.insertCell(3);
    cell.innerHTML = '<td class="linkBloc col2"><Label id="Nm2Modif_' + n +'"  style=" width:82px;">' + tableau.rows[Ligne].cells[3].innerText +'</label></td >';

    cell = ligne.insertCell(4);
    cell.innerHTML = '<td class="linkBloc col3"><Label id="Nm3Modif_' + n +'" style=" width:82px;">' + tableau.rows[Ligne].cells[4].innerText +'</label></td >';

    cell = ligne.insertCell(5);
    cell.innerHTML = '<td class="linkBloc col34"><input  id="LibelleClauseModif_' + n +'"  style=" width:395px;" type="text" value="' + tableau.rows[Ligne].cells[5].innerText +'"></td >';

    cell = ligne.insertCell(6);
    cell.innerHTML = '<td class="supprBloc bImg"><center> <img id="enregistrerClause" name="save_' + n + '" src="/Content/Images/Save_16.png" class="CursorPointer" /> <img id="cancelModifClause" name="cancel_' + n +'" src="/Content/Images/reset.png" class="CursorPointer"></center></td >';
  
 
}
function cancelLigne_indexN(n) {
    var cell, ligne;

    // on récupère l'identifiant (id) de la table qui sera modifiée
    var tableau = document.getElementById("tblBloc");
    // nombre de lignes dans la table (avant ajout de la ligne)
    var Ligne = parseInt(n) + 1;

    $("#ModifIsChekedVal").val("0");

    ligne = tableau.insertRow(n); // création d'une ligne à la position n

    // création et insertion des cellules dans la nouvelle ligne créée
    cell = ligne.insertCell(0);
    cell.innerHTML = '<td class="linkBloc bBranche"><Label id="SelectBancheModif_' + n +'" style=" width:76px;">' + tableau.rows[Ligne].cells[0].innerText + '</label></td >';

    cell = ligne.insertCell(1);
    cell.innerHTML = '<td class="linkBloc bCible"><Label id="SelectCibleModif_' + n +'" style=" width:76px;">' + tableau.rows[Ligne].cells[1].innerText + '</label></td >';

    cell = ligne.insertCell(2);
    cell.innerHTML = '<td class="linkBloc col1"><Label id="Nm1Modif_' + n +'" style=" width:82px;">' + tableau.rows[Ligne].cells[2].innerText + '</label></td >';

    cell = ligne.insertCell(3);
    cell.innerHTML = '<td class="linkBloc col2"><Label id="Nm2Modif_' + n +'"  style=" width:82px;">' + tableau.rows[Ligne].cells[3].innerText + '</label></td >';

    cell = ligne.insertCell(4);
    cell.innerHTML = '<td class="linkBloc col3"><Label id="Nm3Modif_' + n +'" style=" width:82px;">' + tableau.rows[Ligne].cells[4].innerText + '</label></td >';

    cell = ligne.insertCell(5);
    cell.innerHTML = '<td class="linkBloc col4"><Label id="LibelleClauseModif_' + n +'" style=" width:395px;">' + tableau.rows[Ligne].cells[5].querySelector('input').value; + '</label></td >';

    cell = ligne.insertCell(6);
    cell.innerHTML = '<td class="supprBloc bImg"> <center><img id="modifierClause"  src="/Content/Images/editer1616.png" title = "Modifier" name="modif_' + n + '" class="CursorPointer"/> <img id="suprimerClause"  src="/Content/Images/poubelle1616.png" title = "Supprimer" name="suppr_' + n + '" class="CursorPointer"></center></td >';



}
function supprimerLigne_indexN(n) {
  
    var tableau = document.getElementById("tblBloc");
    var nbLignes = tableau.rows.length;
    $("#AjoutIsChekedVal").val("0");
        tableau.deleteRow(n); 
    
}


function SupprimerLibelle(codeBranche, codeCible, Nm1, Nm2, Nm3, LibelleClause, id) {
    ShowLoading(); 
    $.ajax({
        type: "POST",
        url: "/LibellesClauses/DeleteClausesLibelle",
        data: { branche: codeBranche, cible: codeCible, nm1: Nm1, nm2: Nm2, nm3: Nm3, libelle: LibelleClause },
        success: function (data) {
            $("#divBodyClauses").html(data);
            $("#divBodyClauses").show();
            AlternanceLigne("Bloc", "Code", true, null); 
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
}
function ModifierLibelle(codeBranche, codeCible, Nm1, Nm2, Nm3, LibelleClause) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/LibellesClauses/EditClausesLibelle",
        data: { branche: codeBranche, cible: codeCible, nm1: Nm1, nm2: Nm2, nm3: Nm3, libelle: LibelleClause },
        success: function (data) {
            $("#AjoutIsChekedVal").val("0");
            $("#divBodyClauses").html(data);
            $("#divBodyClauses").show();
            AlternanceLigne("Bloc", "Code", true, null); 
            CloseLoading();
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
}

function SaveLibelle(codeBranche, codeCible, Nm1, Nm2, Nm3, LibelleClause) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/LibellesClauses/SaveClausesLibelle",
        data: { branche: codeBranche, cible: codeCible, nm1: Nm1, nm2: Nm2, nm3: Nm3, libelle: LibelleClause },
        success: function (data) {
            $("#AjoutIsChekedVal").val("0");
            $("#divBodyClauses").html(data);
            $("#divBodyClauses").show();
            AlternanceLigne("Bloc", "Code", true, null); 
            CloseLoading();
        },
        error: function (request) {
            //common.error.showXhr(request);
            alert("Libelle spécifique Déja renseigné pour cet clause");
            CloseLoading();
        }
    });
}
function AlimSelectBranche() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/LibellesClauses/GetBranchesClauses",
        data: {},
        success: function (data) {
            RemplirSelectBranche(data); 

        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    }); 
}
function AlimSelectBrancheNew() {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/LibellesClauses/GetBranchesClauses",
        data: {},
        success: function (data) {
         
            RemplirSelectBrancheNew(data); 
            
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
}
function AlimSelectCible(codeB) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/LibellesClauses/GetCibles",
        data: {codeBranche:codeB},
        success: function (data) {
            RemplirSelectCible(data);
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
}
function AlimSelectCibleNew(codeB) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/LibellesClauses/GetCibles",
        data: { codeBranche: codeB },
        success: function (data) {
            RemplirSelectCibleNew(data);
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
}
function rechercherClause(Branche, Cible, Nm1, Nm2, Nm3) {
    ShowLoading();
    $.ajax({
        type: "POST",
        url: "/LibellesClauses/Recherche",
        data: { branche: Branche, cible: Cible, nm1: Nm1, nm2: Nm2, nm3: Nm3 },
        success: function (data) {
            CloseLoading();
            $("#divBodyClauses").html(data);
            $("#divBodyClauses").show();
            AlternanceLigne("Bloc", "Code", true, null); //TODO a remplacer par GuidId
        },
        error: function (request) {
            common.error.showXhr(request);
            CloseLoading();
        }
    });
}
function RemplirSelectCible(data) {
    var select = document.getElementById("SelectCible");
    select.innerHTML = "<option></option>";
    $.each(data, function (index, cible) {
        var opt = cible.Code + "-" + cible.Libelle;
        select.innerHTML += "<option value=\"" + opt + "\">" + opt + "</option>";
    });
    CloseLoading();
}
function RemplirSelectCibleNew(data) {
    var select = document.getElementById("SelectCibleNew");
    select.innerHTML = "<option></option>";
    $.each(data, function (index, cible) {
        var opt = cible.Code + "-" + cible.Libelle;
        select.innerHTML += "<option value=\"" + opt + "\">" + opt + "</option>";
    });
    CloseLoading();
}
function RemplirSelectBranche(data) {
    var select = document.getElementById("SelectBanche"); 
    select.innerHTML = "<option></option>";
    $.each(data, function (index, branche) {
        var opt = branche.Code + "-" + branche.Libelle;
        select.innerHTML += "<option value=\"" + opt + "\">" + opt + "</option>";
    });    
    CloseLoading();
}
function RemplirSelectBrancheNew(data) {
    
    var select = document.getElementById("SelectBancheNew");
    select.innerHTML = "<option></option>";
    $.each(data, function (index, branche) {
        var opt = branche.Code + "-" + branche.Libelle;
        select.innerHTML += "<option value=\"" + opt + "\">" + opt + "</option>";
    });
    CloseLoading();
}
function OnChangeAlim() {
    var codeBranche = document.getElementById("SelectBanche");
    if (codeBranche.value != " ") {
        var codeB = codeBranche.value.split("-")[0];
        AlimSelectCible(codeB);
    }
    CloseLoading();
}
function OnChangeAlimNew() {
    var codeBranche = document.getElementById("SelectBancheNew");
    if (codeBranche.value != " ") {
        var codeB = codeBranche.value.split("-")[0];
        $("#Nm1").val("");
        $("#Nm2").val("");
        $("#Nm3").val("");
        $("#LibelleClause").val("");
        AlimSelectCibleNew(codeB);
    }
    CloseLoading();
}



