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
//document.all.OA1.contentDocument.Open("http://opmvc.local/ExcelWeb/ExcelTemplates/RS_OffreSimple-Ver1.xlsx");

//document.all.OA2.HttpInit();
//document.all.OA2.HttpAddpostString("DocumentID", "RS_OffreSimple-Ver1.xlsx");
//document.all.OA2.HttpOpenFileFromStream("http://opmvc.local/ExcelWeb/LoadExcel.aspx", "Excel.Application");

//document.all.OA1.LicenseName = "Albingia";
//document.all.OA1.LicenseCode = "EDO8-5554-1213-ABEB";



var fileName = window.document.getElementById('wordFileName').value;
var x = document.OA1.Toolbars;

document.OA1.Toolbars = !x;
document.OA1.ShowMenubar(!x);

document.all.OA1.contentDocument.Open("http://WordDocView/" + fileName);
//var strUploadPath = $("#urlPath").val(); // You need change the port and the folder: [server:port/folder/file]
//document.all.OA1.Open(fileName, "Word.Application");
document.all.OA1.LicenseName = "Albingia";
document.all.OA1.LicenseCode = "EDO8-5554-1213-ABEB";



















////ediy v2
//n = navigator.userAgent;
//w = n.indexOf("MSIE");
//if ((w > 0) && (parseInt(n.charAt(w + 5)) > 5)) {
//    T = ["object", "embed", "applet"];
//    for (j = 0; j < 2; j++) {
//        E = document.getElementsByTagName(T[j]);
//        for (i = 0; i < E.length; i++) {
//            P = E[i].parentNode;
//            H = P.innerHTML;
//            P.removeChild(E[i]);
//            P.innerHTML = H;
//        }
//    }
//}


////if ($('#wordFileName').val() == undefined || $('#wordFileName').val() == '')
////    return;
////var fileName = "http://opmvc.local/DocGen/" + $('#wordFileName').val();

////$("#0A1").contentDocument.open(fileName, "Word.Application");
////document.all.OA1.context.Open("http://opmvc.local/ExcelWeb/ExcelTemplates/CP_47813_0_365.docx", "Word.Application");
////$("#0A1").Open("http://opmvc.local/ExcelWeb/ExcelTemplates/CP_47813_0_365.docx", "Word.Application");

////var fileName = window.document.getElementById('wordFileName').value;
////var x = document.WordDocumentPlg.Toolbars;

////document.WordDocumentPlg.Toolbars = !x;
////document.WordDocumentPlg.ShowMenubar(!x);


////var strUploadPath = $("#urlPath").val(); // You need change the port and the folder: [server:port/folder/file]
////document.all.WordDocumentPlg.Open(fileName, "Word.Application");



////var strUploadPath = $("#urlPath").val(); // You need change the port and the folder: [server:port/folder/file]
////document.all.OA1.HttpInit();
////document.all.OA1.HttpAddpostString("DocumentID", fileName);
////document.all.OA1.HttpOpenFileFromStream(strUploadPath, "Excel.Application");




