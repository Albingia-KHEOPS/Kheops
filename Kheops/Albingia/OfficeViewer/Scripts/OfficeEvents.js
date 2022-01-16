$(document).ready(function () {
    UnloadDocument();

    OfficeDocumentsRules();
});
//***************** fermeture du document office****************************
function UnloadDocument() {
    window.onbeforeunload = function () {
        document.all.OA1.ExitOfficeApp();
    };
}
//******************Fermeture du document Word ************
function CloseWordDoc() {
    try {
        if (document.OA1.IsOpened) {
            document.all.OA1.ExitOfficeApp();
            return true;
        }
    } catch (e) {
        return false;
    }
    return true;
}

//************************ Restriction sur le document Office
function OfficeDocumentsRules() {

    var typeDocuments = "";
    if ($("#ruleFile").val() != "" && $("#ruleFile").val() != undefined)
        typeDocuments = $("#ruleFile").val().substr(0, 1);

    var paramRule = $("#ruleFile").val().substr(1);
    var rule = "";
    var printable = "";

    if (paramRule != "" && paramRule != undefined) {
        rule = paramRule.substr(0, 1);
        printable = paramRule.substr(1, 1);
    }

    switch (typeDocuments) {
        case "D":
            GestionDoc(rule); break;
        case "C":
            GestionClause(rule); break;
        case "B":
            GestionClause(rule); break;
        default:
            GestionClause(); break;
    }

    //*****Hot Keys*************
    //document.OA1.DisableFileCommand(1, true);//wdUIDisalbeOfficeButton
    //document.OA1.DisableFileCommand(2, true);//wdUIDisalbeNew
    //document.OA1.DisableFileCommand(4, true);//wdUIDisalbeOpen		
    //document.OA1.DisableFileCommand(16, true);//wdUIDisalbeSave
    //document.OA1.DisableFileCommand(32, true);//wdUIDisalbeSaveAs
    //document.OA1.DisableFileCommand(512, true); //wdUIDisalbePrint (Ctrl+P) PES,PCT,CON
    //document.OA1.DisableFileCommand(1024, true); //wdUIDisalbePrintQuick
    //document.OA1.DisableFileCommand(2048, true); //wdUIDisalbePrintPreview

    document.all.OA1.SetComponentSize(1216, 630);

    //$("#btnValidBloc", window.parent.document).hide();
    $("#btnValidBlocStd", window.parent.document).hide();
    $("#btnValidBlocCla", window.parent.document).hide();
    if (rule == "M") {

        $('#btnValidWordStd', window.parent.document).show();
        $('#btnValidWordCla', window.parent.document).show();
        //$('#btnValidWord', window.parent.document).show();
        $("#btnValidBlocStd", window.parent.document).hide();
        $("#btnValidBlocCla", window.parent.document).hide();
        //$("#btnValidBloc", window.parent.document).hide();

        //$('#btnFermerWord', window.parent.document).hide();
    }
    else {
        $('#btnValidWordStd', window.parent.document).hide();
        $('#btnValidWordCla', window.parent.document).hide();
        //$('#btnValidWord', window.parent.document).hide();
        $("#btnValidBlocStd", window.parent.document).hide();
        $("#btnValidBloCla", window.parent.document).hide();
        //$("#btnValidBloc", window.parent.document).hide();

        //$('#btnFermerWord', window.parent.document).hide();

        $("#btnSaveEnteteClause", window.parent.document).show();
    }

    if (printable == "P") {
        $('#btnPrintWordStd', window.parent.document).show();
        $('#btnPrintWordCla', window.parent.document).show();
        //$('#btnPrintWord', window.parent.document).show();
    }
    else {
        $('#btnPrintWordStd', window.parent.document).hide();
        $('#btnPrintWordCla', window.parent.document).hide();
        //$('#btnPrintWord', window.parent.document).hide();

    }

    if (typeDocuments == "B" && rule == "M") {
        $('#btnValidWordStd', window.parent.document).hide();
        $('#btnValidWordCla', window.parent.document).hide();
        //$('#btnValidWord', window.parent.document).hide();
        $("#btnValidBlocStd", window.parent.document).show();
        $("#btnValidBlocCla", window.parent.document).show();
        //$("#btnValidBloc", window.parent.document).show();
        $('#btnFermerWordStd', window.parent.document).show();

    }

    if ($('#divClausierRech', window.parent.document).is(":visible")) {
        $("#btnPrintWordStd", window.parent.document).show();
        $("#btnPrintWordCla", window.parent.document).show();

        $("#btnImportWordStd", window.parent.document).show();
        $("#btnImportWordCla", window.parent.document).show();

        $("#wvTitreClause", window.parent.document).addClass("readonly").attr("readonly", "readonly");
    }

}
function GestionClause(rule) {

    if (rule == "M") {
        document.OA1.DisableFileCommand(1, true); //wdUIDisalbeOfficeButton
        document.OA1.DisableFileCommand(2, true); //wdUIDisalbeNew
        document.OA1.DisableFileCommand(4, true); //wdUIDisalbeOpen		
        document.OA1.DisableFileCommand(16, true); //wdUIDisalbeSave
        document.OA1.DisableFileCommand(32, true); //wdUIDisalbeSaveAs
    } else {



        if (rule != "P") {
            document.all.OA1.WordDisablePrintHotKey(true);
        }


        if (rule == "V" || rule == "" || rule == "P") {
            document.all.OA1.WordDisableSaveHotKey(true);
            document.all.OA1.WordDisableCopyHotKey(true);
            document.OA1.DisableFileCommand(1, true); //wdUIDisalbeOfficeButton
            document.OA1.DisableFileCommand(2, true); //wdUIDisalbeNew
            document.OA1.DisableFileCommand(4, true); //wdUIDisalbeOpen		
            document.OA1.DisableFileCommand(16, true); //wdUIDisalbeSave
            document.OA1.DisableFileCommand(32, true); //wdUIDisalbeSaveAs
            document.OA1.DisableFileCommand(512, true); //wdUIDisalbePrint (Ctrl+P) PES,PCT,CON
            document.OA1.DisableFileCommand(1024, true); //wdUIDisalbePrintQuick
            document.OA1.DisableFileCommand(2048, true); //wdUIDisalbePrintPreview
            //document.OA1.WordDisableViewRightClickMenu(true);
        }
        //***********Protection du document
        if ((rule == "V" || rule == "" || rule == "P") && document.all.OA1.GetCurrentProgID() == "Word.Application") {
            document.all.OA1.ProtectDoc(3);//wdAllowOnlyFormFields
        }
    }

}
function GestionDoc(rule) {

    if (rule == "M") {
        document.OA1.DisableFileCommand(1, true); //wdUIDisalbeOfficeButton
        document.OA1.DisableFileCommand(2, true); //wdUIDisalbeNew
        document.OA1.DisableFileCommand(4, true); //wdUIDisalbeOpen		
        document.OA1.DisableFileCommand(16, true); //wdUIDisalbeSave
        document.OA1.DisableFileCommand(32, true); //wdUIDisalbeSaveAs
    } else {



        if (rule != "P") {
            document.all.OA1.WordDisablePrintHotKey(true);
        }


        if (rule == "V" || rule == "" || rule == "P") {
            document.all.OA1.WordDisableSaveHotKey(true);
            document.all.OA1.WordDisableCopyHotKey(true);
            document.OA1.DisableFileCommand(1, true); //wdUIDisalbeOfficeButton
            document.OA1.DisableFileCommand(2, true); //wdUIDisalbeNew
            document.OA1.DisableFileCommand(4, true); //wdUIDisalbeOpen		
            document.OA1.DisableFileCommand(16, true); //wdUIDisalbeSave
            document.OA1.DisableFileCommand(32, true); //wdUIDisalbeSaveAs
            document.OA1.DisableFileCommand(512, true); //wdUIDisalbePrint (Ctrl+P) PES,PCT,CON
            document.OA1.DisableFileCommand(1024, true); //wdUIDisalbePrintQuick
            document.OA1.DisableFileCommand(2048, true); //wdUIDisalbePrintPreview
            //document.OA1.WordDisableViewRightClickMenu(true);
        }
        //***********Protection du document
        if ((rule == "V" || rule == "" || rule == "P") && document.all.OA1.GetCurrentProgID() == "Word.Application") {
            document.all.OA1.ProtectDoc(3);//wdAllowOnlyFormFields
        }
    }
}

function SaveDocument(isSaveAs, saveAs) {

    try {
        if (saveAs == "" || saveAs == undefined) {
            saveAs = $("#physicNameDoc").val();
        }
        return document.OA1.saveAs(saveAs);

    } catch (e) {
        return false;
    }
}


function PrintDocument() {
    try {
        document.OA1.PrintDialog();
        return true;
    } catch (e) {
        return false;
    }
}