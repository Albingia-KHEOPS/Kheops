//****************** Action à faire aprés le chargement du document
function OA_DocumentOpened() {
    $('#divFullScreen').show();
    $("#contentObjExcel").hide();
    $("#contentExcelError").show();
  
    
    //disable print
    document.OA1.DisableStandardCommand(4, true);
    //disable save
    document.OA1.DisableStandardCommand(1, true);
    //disable right click
    document.OA1.DisableStandardCommand(8, true);
    //disable double click
    document.OA1.DisableStandardCommand(10, true);
    document.OA1.DisableStandardCommand(4, true);
    $.ajax({
        type: "POST",
        url: "/ExcelWeb/Ajax/ExcelInteraction.asmx/LoadDbDataToExcel",
        data: "{'branche':'" + encodeURIComponent($("#branche").val()) + "', tabGuid:'" + encodeURIComponent($("#tabGuid").val()) + "', user:'" + encodeURIComponent($("#user").val()) + "', nouvelleVersion:'" + encodeURIComponent($("#nouvelleVersion").val()) + "', splitChars:'" + encodeURIComponent($("#jsSplitChar").val()) + "', strParameters:'" + encodeURIComponent($("#parameters").val()) + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
          //var returnedDtaArray = AlbJsSplitArray(data.d, "#NewVer#");
           // $("#Offre_Version").val(returnedDtaArray[0]);
            //LoadData(returnedDtaArray[1]);
            LoadData(data.d);
            
            // window.parent.TestIFrame("MESSAGE EXCEL");
            $('#divFullScreen').hide();
            $("#contentObjExcel").show();
            $("#contentExcelError").hide();
        },
        error: function (request) {
            $("#divFullScreen").hide();
            common.dialog.smallError("Erreur d'affichage du document.", true);
            CloseExcelDocument();
            $("#contentObjExcel").hide();
            $("#contentExcelError").show();
        }
    });
}

//***********************************Close document**********************
function CloseExcelDocument() {
    document.OA1.contentDocument.CloseDoc();
}
//***********************************Chargement des données dans le document excel**********************
function LoadData(dataToLoad) {
    document.OA1.contentDocument.ExcelActivateWorkSheet(1);
    var objExcel = document.OA1.GetApplication();
    var worksheet = objExcel.ActiveSheet;
    var splitChar = window.document.getElementById('jsSplitChar').value;
    if (dataToLoad.indexOf("#IN")>-1) {
        var elemInOut = AlbJsSplitArray(dataToLoad, "#IN" + splitChar + "OUT#");
        for (var i = 0; i< elemInOut.length; i++) {
            var cellsIn = SetCellsMap(elemInOut[i], worksheet, splitChar);
            if (i == 0) {
                $("#cellsMapInput").val(cellsIn);
            } else {
                $("#cellsMapOutPut").val(cellsIn);
            }
        }
    } else {
        var cells = SetCellsMap(dataToLoad, worksheet, splitChar);

        $("#cellsMapInput").val(cells);


    }
    document.OA1.contentDocument.ExcelActivateWorkSheet(2);
}
//***********************************Chargement des données **********************
function SetCellsMap(dataToLoad, worksheet,splitChar) {
    var cells = "";
    var dataElem = AlbJsSplitArray(dataToLoad, splitChar);
    for (var i = 0; i < dataElem.length; i++) {
        var tabElems = AlbJsSplitArray(dataElem[i], "||");
        if (tabElems != "noData") {
            if (tabElems[0] != "" && tabElems[0] != null) {
                worksheet.cells(parseInt(AlbJsSplitElem(tabElems[1], 0, ",")), parseInt(AlbJsSplitElem(tabElems[1], 1, ","))).value = tabElems[0];
            }
            if (tabElems[1] != "" && tabElems[1] != null) {
                cells = cells + tabElems[1] + splitChar;
            }
        }
    }
    return cells;
    //window.document.getElementById('cellsMap').value = cells;
}
//******************************cherche un elements et le décomposer: la fonction retourne noData en cas d'erreur**********
function GetElemsFromTabs(elem, splitChar) {
    if (elem.length == 0 || elem == "")
        return "noData";
    //var elems = lns[i];
    //TODO:prendre en compte le cas ou un éléments est un tableau (pour la gestion des listes)
    return  elem.split(splitChar);
}
//***********************************Lecture des cellules excel**********************
function OA_ReadCellsFromExcel() {
    //showLoading();
    //(string branche, string section,string dataToSave, string splitChars,string strParameters)
    //    var objExcel = document.frames[0].OA1.GetApplication();
    $("#btnAnnuler").attr("disabled", "disabled");
    $("#btnSuivant").attr("disabled", "disabled");
    $("#contentObjExcel").hide();  
    $("#textGen").html("Passage de la saisie Tableur à la saisie standard en cours ...");
    //$("#contentExcelError").show();
    $("#divGenDocument").show();
    
    var nbrWorkSheet=document.OA1.contentDocument.ExcelGetWorkSheetCount();
    document.OA1.contentDocument.ExcelActivateWorkSheet(nbrWorkSheet);
    var objExcel = document.OA1.GetApplication();
    var worksheet = objExcel.ActiveSheet;
    //    window.alert("La valeur de la cellule (4,24) est :" + worksheet.cells(4, 24).value);
   // var splitChar = window.parent.document.getElementById('jsSplitChar').value;
    var splitChar = window.document.getElementById('jsSplitChar').value;
    //var valToSend = PrepareData(worksheet, splitChar);
    var valToSend = PrepareExcelData(worksheet, splitChar).replace(/'/g, "£");
    var res = "ko";
   
    $.ajax({
        type: "POST",
        url: "/ExcelWeb/Ajax/ExcelInteraction.asmx/SaveData",
        data: "{'branche':'" + encodeURIComponent($("#branche").val()) + "', 'dataToSave':'" + encodeURIComponent(valToSend) + "', 'splitChars':'" + encodeURIComponent($("#jsSplitChar").val()) + "','strParameters':'" + encodeURIComponent($("#parameters").val()) + "'}",
        //data: "{'branche':'" + encodeURIComponent($("#branche").val()) + "', 'splitChars':'" + encodeURIComponent($("#jsSplitChar").val()) + "','strParameters':'" + encodeURIComponent($("#parameters").val()) + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#divGenDocument").hide();
            res = data.d;
            if (data.d == "ok") {
                //alert("Données enregistrées avec succées");
                document.OA1.contentDocument.CloseDoc();
                Redirect();
            }
        },
        error: function (request) {
            // alert("données non sauvegardées");
            $("#divGenDocument").hide();
            $("#btnAnnuler").removeAttr("disabled");
            $("#btnSuivant").removeAttr("disabled");
            common.dialog.smallError("Erreur de sauvegarde.", true);
            //hideLoading();
            document.OA1.contentDocument.ExcelActivateWorkSheet(2);
            
            
        }
    });
}
//***********************************Redirection *********************

function Redirect() {
    
   
    var codeOffre = $("#Offre_CodeOffre").val();
    var version = $("#Offre_Version").val();
    var type = $("#Offre_Type").val();
    var tabGuid = $("#tabGuid").val();
    var addParamType = $("#AddParamType").val();
    var addParamValue = $("#AddParamValue").val();

    $.ajax({
        type: "POST",
        url: "/OffreSimplifiee/Redirection/",
        data: { codeOffre: codeOffre, version: version, type: type, tabGuid: tabGuid, addParamType: addParamType, addParamValue: addParamValue },
        success: function (data) { },
        error: function (request) {
            common.error.showXhr(request);
        }
    });
 
}
//***********************************Préparation des données lu d'excel**********************
function PrepareExcelData(worksheet, splitChar) {
    //************* Cells Risques 
    //Exemple : 2-169|170-265|3266-385|386-504
    var cellsRsqs = worksheet.cells(2, 17).value;
    if (cellsRsqs == 'noData' || cellsRsqs == '') {
        return false;
    }
    //**************Séparation des risques
    var lstRsqs = AlbJsSplitArray(cellsRsqs, '|');
    if (lstRsqs == 'noData' ) {
        return false;
    }
    var retValCells = '';
    //****** Parcourir tous les risques
    for (var i = 0; i < lstRsqs.length; i++) {
        if (retValCells != '') {
            retValCells = retValCells + splitChar;
        }
        if (lstRsqs[i] != "noData") {
            var rowCol = AlbJsSplitArray(lstRsqs[i], ':');
            if (rowCol != "noData") {
                // Cellule debut du risque  rowCol[0] - Cellule Fin du risque rowCol[1]
                var rsqDeb = parseInt(rowCol[0]);
                var rsqFin = parseInt(rowCol[1])+1;
                for (var j = rsqDeb; j < rsqFin; j++) {
                    var varMapCell = $.trim(worksheet.cells(j, 16).value);
                    var valCell = worksheet.cells(j, 1).value;
                    if (valCell == '' || valCell == '00/01/1900' || valCell == '-') {
                        valCell = 'NODATA';
                    }
                        

                    retValCells = retValCells + varMapCell + '~' + valCell + '|';
                }
            }
        }
    }
    return retValCells;
    ////******** préparation des données auto
    //// var lns = ["3#**#22", "4#**#22", "5#**#22", "6#**#22", "7#**#22", "8#**#22", "11#**#22", "12#**#22", "13#**#22", "14#**#22", "15#**#22", "17#**#22", "18#**#22"];
    //var retval = "";
    ////var lns = AlbJsSplitArray(window.parent.document.getElementById('cellsMap').value, splitChar);
    //var lns = AlbJsSplitArray(window.document.getElementById('cellsMapOutPut').value, splitChar);
    //if (lns == "noData") {
    //    return false;
    //}
    //for (var i = 0; i < lns.length; i++) {
    //    if (lns[i] != "noData") {
    //        var tabElems = AlbJsSplitArray(lns[i], ",");

    //        if (tabElems != "noData") {
    //            var valElem = worksheet.cells(parseInt(tabElems[0]), parseInt(tabElems[1])).value;
    //            if (valElem == undefined) {
    //                valElem = "";
    //            }
    //            if (i == lns.length - 1) {
    //                retval = Concat(retval, valElem + "||" + lns[i], "");
    //            } else {
    //                retval = Concat(retval, valElem + "||" + lns[i], splitChar);
    //            }

    //        }

    //    }

    //}
    //return retval;
}
function PrepareData(worksheet,splitChar) {
    //******** préparation des données auto
   // var lns = ["3#**#22", "4#**#22", "5#**#22", "6#**#22", "7#**#22", "8#**#22", "11#**#22", "12#**#22", "13#**#22", "14#**#22", "15#**#22", "17#**#22", "18#**#22"];
    var retval = "";
    //var lns = AlbJsSplitArray(window.parent.document.getElementById('cellsMap').value, splitChar);
    var lns = AlbJsSplitArray(window.document.getElementById('cellsMapOutPut').value, splitChar);
    if (lns == "noData") {
        return false;
    }
    for (var i = 0; i < lns.length; i++) {
        if (lns[i] != "noData") {
            var tabElems = AlbJsSplitArray(lns[i], ",");

            if (tabElems != "noData") {
                var valElem = worksheet.cells(parseInt(tabElems[0]), parseInt(tabElems[1])).value;
                if (valElem == undefined) {
                    valElem = "";
                }
                if (i == lns.length - 1) {
                    retval = Concat(retval, valElem + "||" + lns[i], "");
                } else {
                    retval = Concat(retval, valElem + "||" + lns[i], splitChar);
                }
                
            }
            
        }

    }
    return retval;
}
//***********************************Déclenchement des événements avant chargement du document**********************
function OA_WindowBeforeRightClick() {
    window.alert("Click droit OK");
    document.OA1.DisableStandardCommand(8, true); //cmdTypeRightClick = 0x00000008, // prevent right click
}
//***********************************Changement de sélection d'une cellule du document**********************
function OA_WindowSelectionChange() {
    var objExcel = document.OA1.GetApplication();
    var worksheet = objExcel.ActiveSheet;
    //alert("cell:" + worksheet.Application.ActiveCell.Address);
    //        alert("Col:" + worksheet.Application.ActiveCell.Cells.Column);
    //        alert("Row:" + worksheet.Application.ActiveCell.Cells.Row);
    alert(worksheet.cells(2, 22).value);
    alert("cell:" + worksheet.cells(parseInt(worksheet.Application.ActiveCell.Cells.Column), parseInt(worksheet.Application.ActiveCell.Cells.Row)).value);

    //window.alert("Selct change OK");
    //document.OA1.DisableStandardCommand(1, true); //cmdTypeDoubleClick = 0x00000010, // prevent Double click
}
//***********************************Modofier une cellule du document excel**********************
function OA_WriteCellsToExcel() {
//    var objExcel = document.OA1.GetApplication();
//    var worksheet = objExcel.ActiveSheet;
//    AlbWriteEventInfo(worksheet);
//    AlbAlbWriteSupportInfo(worksheet);
   
}

function cmdTypeIESecurityReminder() {
    //document.OA1.DisableStandardCommand(20, true);
}


function Concat(retVal,stringRes, newStr) {
    return retVal+stringRes + newStr;
}

function OA1_NotifyCtrlReady() {
    //document.OA1.ShowRibbonTitlebar (false);
    //document.OA1.ShowMenubar (false);
    //document.OA1.Toolbars = false;

    //If you want to open a document when the page loads, you should put the code here.
    //document.all.OA1.Open("http://www.ocxt.com/demo/samples/sample.doc");
    document.OA1.LicenseName = "";
    document.OA1.LicenseCode = "";
}

