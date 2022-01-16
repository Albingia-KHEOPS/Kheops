var SyntheseAffaire = (function () {

    let c = function () { }
    c.prototype.initPage = function () {
        MapBoitesDialogue();
        let _this = this;
        $("[id^='link_']").kclick(function (e) {
            Loadinfoentet();
            e.preventDefault();
            let fn = _this["display" + this.id.split('_')[1]];
            if (typeof fn !== "function") {
                return false;
            }
            let a = ($(this).attr("albparam") || "").split('_');
            fn === _this.displayOffreCitrix ? fn(a[0], a[1]) : fn();

        });
    };
    c.prototype.displayHistory = function () {
        OpenVisuHistorique();
    };
    c.prototype.displayClauses = function () {
        OpenListeClauses();
    };
    c.prototype.displayConnexites = function () {
        openPopupConnexites(undefined, undefined, 'false');
    };
    c.prototype.displayQuittances = function () {
        OpenVisulisationQuittances('Toutes', 'true');
    };
    c.prototype.displayObservations = function () {
        OpenVisualisationObservations();
    };
    c.prototype.displayAlertes = function() {
        common.$ui.showDialog($("#divAlertesAvenant"), "", "Informations Affaire", { width: 350, height: "auto" }, null, null, "dialog-fix", true);
    };
    c.prototype.displayOffreCitrix = function (numero, version) {
        document.location.href = "Albinprod:GererContrat?action=VISUCONTRAT?type=O?ipb=" + numero + "?Alx=" + version + "";
    };
    return c;
})();
var synthese = new SyntheseAffaire();
function Loadinfoentet() {

    var filter = {
        CodeOffre: $("#Offre_CodeOffre").val(),
        version: $("#Offre_Version").val(),
        TypeContrat: $("#Offre_Type").val(),
    }
    if (true) {
        window.sessionStorage.setItem('recherche_filter', JSON.stringify(filter));
    }
    CodeOffreAll = $("#Offre_CodeOffre_S").val().split('-');
    CodeOffre = CodeOffreAll[0].trim();
    VersionOffre = CodeOffreAll[1].trim();
    $("#Offre_CodeOffre").val(CodeOffre);
    $("#Offre_Version").val(VersionOffre);
    $("#Offre_Type").val($("#Offre_Type_S").val() == "Offre" ? "O" : "P");
    $("#NumAvenantPage").val($("#NumAvenantPage_S").val());
   

    
}
$(function () {
    synthese.initPage();
});
