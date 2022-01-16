//ediy v2
n = navigator.userAgent;
w = n.indexOf("MSIE");
if ((w > 0) && (parseInt(n.charAt(w + 5)) > 5)) {
    T = ["object", "embed", "applet"];
    for (j = 0; j < 2; j++) {
        E = document.getElementsByTagName(T[j]);
        for (i = 0; i < E.length; i++) {
            P = E[i].parentNode;
            H = P.innerHTML;
            P.removeChild(E[i]);
            P.innerHTML = H;
        }
    }
}

//*****************Hide Menu Bar******************
var x = document.OA1.Toolbars;
document.OA1.ShowMenubar(!x);
document.OA1.Toolbars = !x;
//***********************************************
document.all.OA1.Open(document.all.getElementById("fileNamePath"));
document.OA1.LicenseName = "Albingia";
document.OA1.LicenseCode = "EDO8-5554-1213-ABEB";
//************************Office Menu********************
document.OA1.DisableFileCommand(1, true);//wdUIDisalbeOfficeButton
document.OA1.DisableFileCommand(2, true);//wdUIDisalbeNew
document.OA1.DisableFileCommand(4, true);//wdUIDisalbeOpen		
document.OA1.DisableFileCommand(16, true);//wdUIDisalbeSave
document.OA1.DisableFileCommand(32, true);//wdUIDisalbeSaveAs
document.OA1.DisableFileCommand(512, true); //wdUIDisalbePrint (Ctrl+P) PES,PCT,CON
document.OA1.DisableFileCommand(1024, true); //wdUIDisalbePrintQuick
document.OA1.DisableFileCommand(2048, true); //wdUIDisalbePrintPreview

//***************Hot Keys***************************************//
if (document.all.OA1.GetCurrentProgID() == "Word.Application") {
    if (document.all.OA1.IsOpened()) {
        document.all.OA1.WordDisableSaveHotKey(true);
        document.all.OA1.WordDisablePrintHotKey(true);
        document.all.OA1.WordDisableCopyHotKey(true);
    }
}
else if (document.all.OA1.GetCurrentProgID() == "Excel.Application") {
    document.all.OA1.WordDisableSaveHotKey(true);
    document.all.OA1.WordDisablePrintHotKey(true);
    document.all.OA1.WordDisableCopyHotKey(true);
}