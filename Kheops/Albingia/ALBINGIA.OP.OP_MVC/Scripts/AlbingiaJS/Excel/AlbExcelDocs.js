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


var fileName = window.document.getElementById('fileName').value;
var x = document.OA1.Toolbars;

document.OA1.Toolbars = !x;
document.OA1.ShowMenubar(!x);


var strUploadPath = $("#urlPath").val(); // You need change the port and the folder: [server:port/folder/file]
document.all.OA1.Open(strUploadPath + fileName, "Excel.Application");
document.all.OA1.LicenseName = "Albingia";
document.all.OA1.LicenseCode = "EDO8-5554-1213-ABEB";




//var strUploadPath = $("#urlPath").val(); // You need change the port and the folder: [server:port/folder/file]
//document.all.OA1.HttpInit();
//document.all.OA1.HttpAddpostString("DocumentID", fileName);
//document.all.OA1.HttpOpenFileFromStream(strUploadPath, "Excel.Application");




