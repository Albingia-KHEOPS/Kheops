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


document.all.OA1.ExitOfficeApp();
var x = document.OA1.Toolbars;
document.OA1.Toolbars = !x;
document.OA1.ShowMenubar(!x);
//document.OA1.ShowRibbonTitlebar(false);
//document.OA1.DisplayHorizontalRuler(false);
//document.OA1.DisplayVerticalRuler(false);
//document.OA1.ShowRibbonTitlebar(!x);
//var resOpen = document.all.OA1.Open("http://officeviewer/Documents/temp/CP_DAN_20_N22676.docx");
if (    $("#fileNamePath").val() == "NewWordFile") {
    document.all.OA1.createNewView("Word.Application");
} else {
    document.all.OA1.BeginWait;
    var resOpen = document.all.OA1.Open($("#fileNamePath").val());
    document.all.OA1.BeginWait;
    if (resOpen) {
        $("#trWordViwer").show();
        $("#trErrorMessage").hide();
    } else {
        $("#trWordViwer").hide();
        $("#trErrorMessage").show();
    }
}


document.OA1.LicenseName = "Albingia";
document.OA1.LicenseCode = "EDO8-5554-1213-ABEB";


//if (document.all.OA1.GetCurrentProgID() == "Word.Application") {
//    document.all.OA1.ProtectDoc(2);//wdAllowOnlyFormFields
//}
