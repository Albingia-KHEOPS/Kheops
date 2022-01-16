using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ALBINGIA.OP.OP_MVC.Common
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Creating bundle for your css files
            bundles.Add(new StyleBundle("~/Content/root.css").Include(
              "~/Content/_Site.css"
            , "~/Content/vBandeau.css"
            , "~/Content/FormCommon.css"
            , "~/Content/Layout.css"
            , "~/Content/LayoutArbre.css"
            , "~/Content/jquery.contextMenu.css"
            , "~/Content/albContextMenu.css"
            ));
            bundles.Add(new StyleBundle("~/Content/fancybox/all.css").Include(
              "~/Content/fancybox/jquery.fancybox.css"
            , "~/Content/fancybox/jquery.fancybox-buttons.css"
            , "~/Content/fancybox/jquery.fancybox-thumbs.css"
            ));
            bundles.Add(new StyleBundle("~/Content/themes/base/all.css").Include(
              "~/Content/themes/base/jquery.ui.all.css"
            ));
            bundles.Add(new StyleBundle("~/Content/CLEditor/all.css").Include(
              "~/Content/CLEditor/jquery.cleditor.css"
            ));
            bundles.Add(new StyleBundle("~/Content/select2.css").Include(
              "~/Content/select2.css"
            ));

            // Creating bundle for your js files
            bundles.Add(new ScriptBundle("~/Scripts/jQuery.js").Include(
                "~/Scripts/Jquery/jquery-1.8.2.js"
                , "~/Scripts/Jquery/jquery.unobtrusive-ajax.min.js"
                , "~/Scripts/Jquery/jquery.validate.min.js"
                , "~/Scripts/Jquery/jquery.validate.unobtrusive.min.js"
                , "~/Scripts/Jquery/jquery.cleditor.min.js"
                , "~/Scripts/Jquery/jquery-ui-1.8.23.js"
                , "~/Scripts/Jquery/jquery.ui.datepicker-fr.js"
                , "~/Scripts/Jquery/jquery.contextmenu.js"
                , "~/Scripts/Jquery/jquery.ui.position.js"
                , "~/Scripts/Jquery/autoNumeric.min.js"
                , "~/Scripts/Jquery/select2.min.js"
                , "~/Scripts/fancybox/jquery.fancybox.pack.js"
                , "~/Scripts/fancybox/helpers/jquery.fancybox-buttons.js"
                , "~/Scripts/fancybox/helpers/jquery.fancybox-media.js"
                , "~/Scripts/fancybox/helpers/jquery.fancybox-thumbs.js"));

            bundles.Add(new ScriptBundle("~/Scripts/scripts.js").Include(
                  "~/Scripts/knockout/3.5/knockout.js"
                , "~/Scripts/knockout/knockout.mapping-latest.js"
                , "~/Scripts/knockout/knockout-fast-foreach.js"
                , "~/Scripts/AlbingiaJS/alb_Layout.js"
                , "~/Scripts/AlbingiaJS/Common/constants.js"
                , "~/Scripts/AlbingiaJS/Common/define-properties.js"
                , "~/Scripts/AlbingiaJS/Common/common.js"
                , "~/Scripts/AlbingiaJS/Common/jqExtensions.js"
                , "~/Scripts/AlbingiaJS/kheops.errors.js"
                , "~/Scripts/AlbingiaJS/albCommon.js"
                , "~/Scripts/AlbingiaJS/albCFListe.js"
                , "~/Scripts/AlbingiaJS/albCommonAutoComplete.js"
                //, "~/Scripts/AlbingiaJS/albLayoutArbre.js"
                , "~/Scripts/AlbingiaJS/es5/LayoutArbre.js"
                , "~/Scripts/AlbingiaJS/alb_QuittanceTransverse.js"
                , "~/Scripts/knockout/CustomBindings/*.js"
                , "~/Scripts/knockout/Components/*.js"
                , "~/Scripts/knockout/Extenders/*.js"
                , "~/Scripts/AlbingiaJS/Common/FrDate.js"
                , "~/Scripts/AlbingiaJS/Common/affaire.js"
                , "~/Scripts/AlbingiaJS/connexites.js"
                , "~/knockout/components/connexites/connexites.initializer.js"
                , "~/knockout/components/connexites/connexites.viewmodel.js"
                , "~/knockout/components/fusion-connexites/fusion-connexites.initializer.js"
                , "~/knockout/components/fusion-connexites/fusion-connexites.viewmodel.js"
                , "~/knockout/components/recherche-affaires/recherche-affaires.initializer.js"
                , "~/knockout/components/recherche-affaires/recherche-affaires.viewmodel.js"
            ));

           
            // Bundle style Conditions garantie
            bundles.Add(new StyleBundle("~/Content/ConditionsGarantie").Include(
             "~/Content/vConditionsGarantie.css"
           ));
         
            // Bundle script Conditions garantie
            bundles.Add(new ScriptBundle("~/Scripts/ConditionsGarantie").Include(
                 "~/Scripts/AlbingiaJS/albLCIFranchise.js"
                 ,"~/Scripts/AlbingiaJS/albConditionsGarantie.js" 
                 ));

            // Bundle style LayoutMenu
            bundles.Add(new StyleBundle("~/Content/LayoutMenu").Include(
              "~/Content/vRechercheSaisie.css"
             , "~/Content/pvOffreContratSearchResult_Grid.css"
             , "~/Content/ko-popup.css"
             , "~/Content/less/recherche/search.css"
             , "~/Content/less/connexites/engagements.css"
           ));

            // Bundle script LayoutMenu
            bundles.Add(new ScriptBundle("~/Scripts/LayoutMenu").Include(
                "~/Scripts/AlbingiaJS/albContextMenu.js"
                , "~/Scripts/AlbingiaJS/albRechercheSaisie.js"
                , "~/Scripts/AlbingiaJS/recherche-affaires.js"
                ));


           
            // Bundle style AnEntete
            bundles.Add(new StyleBundle("~/Content/AnEntete/AnEnteteStyle").Include(
              "~/Content/AnEntete/albAnEntete.css"
           ));

            #region AnInformationsGenerales
            
            // Bundle style AnInformationsGenerales
            bundles.Add(new StyleBundle("~/Content/AlbAnInformationsGenerales").Include(
              "~/Content/AnInformationsGenerales/albAnInformationsGenerales.css"
           ));
            
            // Bundle script AnInformationsGenerales
            bundles.Add(new ScriptBundle("~/Scripts/AlbAnInformationsGenerales").Include(
                 "~/Scripts/AlbingiaJS/AnInformationsGenerales/albAnInformationsGenerales.js"
                 ));
            #endregion
            #region AvenantInfoGenerales
            
            // Bundle style AvenantInfoGenerales
            bundles.Add(new StyleBundle("~/Content/AlbAvenantInfoGenerales").Include(
              "~/Content/Avenant/albAvenantInfoGenerales.css"
           ));
            
            // Bundle script AvenantInfoGenerales
            bundles.Add(new ScriptBundle("~/Scripts/AlbAvenantInfoGenerales").Include(
                 "~/Scripts/AlbingiaJS/Avenant/albAvenantInfoGenerales.js"
                 ));
            #endregion

            #region DetailsRisque
            // Bundle style DetailsRisque
            bundles.Add(new StyleBundle("~/Content/AlbDetailsRisque").Include(
              "~/Content/vDetailsRisque.css"
           ));
            // Bundle script DetailsRisque
            bundles.Add(new ScriptBundle("~/Scripts/AlbDetailsRisque").Include(
                 "~/Scripts/AlbingiaJS/albDetailsRisque.js",
                 "~/Scripts/AlbingiaJS/albInformationsSpecifiques.js",
                 "~/Scripts/AlbingiaJS/albInformationsSpecifiquesRisques.js",
                 "~/Scripts/AlbingiaJS/albLCIFranchise.js",
                 "~/Scripts/AlbingiaJS/albActivite.js"
                 ));
            #endregion

            #region DetailsObjetRisque
            // Bundle style DetailsObjetRisque
            bundles.Add(new StyleBundle("~/Content/AlbDetailsObjetRisque").Include(
              "~/Content/vDetailsObjetRisque.css"
           ));
            // Bundle script DetailsObjetRisque
            bundles.Add(new ScriptBundle("~/Scripts/AlbDetailsObjetRisque").Include(
                 "~/Scripts/AlbingiaJS/albDetailsObjetRisque.js",
                 "~/Scripts/AlbingiaJS/albActivite.js"
                 ));
            #endregion

            #region RisqueInventaire

            // Bundle style RisqueInventaire
            bundles.Add(new StyleBundle("~/Content/AlbRisqueInventaire").Include(
              "~/Content/vRisqueInventaire.css"
           ));
            // Bundle script RisqueInventaire
            bundles.Add(new ScriptBundle("~/Scripts/AlbRisqueInventaire").Include(
                 "~/Scripts/AlbingiaJS/albRisqueInventaire.js"
                 ));
            #endregion

            #region ChoixClauses

            // Bundle style ChoixClauses
            bundles.Add(new StyleBundle("~/Content/AlbChoixClauses").Include(
              "~/Content/vChoixClauses.css"
           ));
            // Bundle script ChoixClauses
            bundles.Add(new ScriptBundle("~/Scripts/AlbChoixClauses").Include(
                 "~/Scripts/AlbingiaJS/albChoixClauses.js"
                 ));
            // Bundle script liste ChoixClauses
            bundles.Add(new ScriptBundle("~/Scripts/AlbListeClauses").Include(
                 "~/Scripts/AlbingiaJS/albListeClauses.js"
                 ));
            #endregion

            // Bundle style Engagement
            bundles.Add(new StyleBundle("~/Content/AlbEngagement").Include(
                "~/Content/vEngagements.css",
                "~/Content/vLCIFranchise.css"
             ));
            // Bundle script Engagement
            bundles.Add(new ScriptBundle("~/Scripts/AlbEngagement").Include(
               "~/Scripts/AlbingiaJS/albEngagements.js",
               "~/Scripts/AlbingiaJS/albLCIFranchise.js"
            ));

            #region Engagement par traité
            // Bundle style AlbEngagementTraite
            bundles.Add(new StyleBundle("~/Content/AlbEngagementTraite").Include(
                "~/Content/vEngagementTraite.css",
                "~/Content/vLCIFranchise.css"
                ));
            // Bundle script AlbEngagementTraite
            bundles.Add(new ScriptBundle("~/Scripts/AlbEngagementTraite").Include(
                 "~/Scripts/AlbingiaJS/albLCIFranchise.js",
                 //"~/Scripts/AlbingiaJS/albEngPerRech.js",
                 "~/Scripts/AlbingiaJS/albEngagementTraite.js"
                 ));
            // Bundle script AlbEngagementTraiteBaseJs
            bundles.Add(new ScriptBundle("~/Scripts/AlbEngagementTraiteBaseJs").Include(
                 "~/Scripts/AlbingiaJS/albEngagementTraite.js",
                 "~/Scripts/AlbingiaJS/albLCIFranchise.js"

                 ));
            #endregion

            #region Montant Reference
            // Bundle style albMontantReference
            bundles.Add(new StyleBundle("~/Content/AlbMontantReference").Include(
                 "~/Content/AffaireNouvelle/albMontantReference.css"
                 ));
            // Bundle script albMontantReference
            bundles.Add(new ScriptBundle("~/Scripts/AlbMontantReference").Include(
                 "~/Scripts/AlbingiaJS/AffaireNouvelle/albMontantReference.js"
                 ));
            #endregion

            #region Courtiers
            // Bundle style Courtiers
            bundles.Add(new StyleBundle("~/Content/AlbCourtiers").Include(
                 "~/Content/AnCourtiers/albAnCourtiers.css"
                 ));
            // Bundle script Courtiers
            bundles.Add(new ScriptBundle("~/Scripts/AlbCourtiers").Include(
                 "~/Scripts/AlbingiaJS/AnCourtiers/albAnCourtiers.js"
                 ));
            #endregion

            #region CoAssureur
            // Bundle style CoAssureur
            bundles.Add(new StyleBundle("~/Content/AlbCoAssureurs").Include(
                 "~/Content/AnCoAssureur/albAnCoAssureur.css"
                 ));
            // Bundle script CoAssureur
            bundles.Add(new ScriptBundle("~/Scripts/AlbCoAssureurs").Include(
                 "~/Scripts/AlbingiaJS/AnCoAssureurs/albAnCoAssureurs.js"
                 ));
            #endregion

            #region CreationAffaireNouvelle
            // Bundle style CreationAffaireNouvelle
            bundles.Add(new StyleBundle("~/Content/AlbCreationAffaireNouvelle").Include(
                 "~/Content/vCreationAffaireNouvelle.css",
                 "~/Content/vCreationAffaireNouvelle.css"
                 ));
            // Bundle script CreationAffaireNouvelle
            bundles.Add(new ScriptBundle("~/Scripts/AlbCreationAffaireNouvelle").Include(
                 "~/Scripts/AlbingiaJS/albCreationAffaireNouvelle.js"
                 ));
            #endregion

            #region Quittance
            // Bundle style Quittance
            bundles.Add(new StyleBundle("~/Content/AlbQuittance").Include(
                 "~/Content/Quittance/albQuittance.css"
                 ));
            // Bundle script Quittance
            bundles.Add(new ScriptBundle("~/Scripts/AlbQuittance").Include(
                 "~/Scripts/AlbingiaJS/Quittance/albQuittance.js"
                 ));
            #endregion

            #region DocumentGestion
            // Bundle style DocumentGestion
            bundles.Add(new StyleBundle("~/Content/AlbDocumentGestion").Include(
                 "~/Content/vDocumentGestion.css"
                 ));
            // Bundle script DocumentGestion
            bundles.Add(new ScriptBundle("~/Scripts/AlbDocumentGestion").Include(
                 "~/Scripts/AlbingiaJS/albDocumentGestion.js"
                 ));
            #endregion

            #region ValidationOffre
            // Bundle style ValidationOffre
            bundles.Add(new StyleBundle("~/Content/AlbValidationOffre").Include(
                 "~/Content/vValidationOffre.css"
                 ));
            // Bundle script ValidationOffre
            bundles.Add(new ScriptBundle("~/Scripts/AlbValidationOffre").Include(
                 "~/Scripts/AlbingiaJS/albValidationOffre.js"
                 ));
            #endregion

            #region CreationAttestation
            // Bundle style CreationAttestation
            bundles.Add(new StyleBundle("~/Content/AlbCreationAttestation").Include(
                 "~/Content/Regularisation/vCreationRegularisation.css",
                 "~/Content/vCreationAttestation.css"
                 ));
            // Bundle script CreationAttestation
            bundles.Add(new ScriptBundle("~/Scripts/AlbCreationAttestation").Include(
                 "~/Scripts/AlbingiaJS/albCreationAttestation.js"
                 ));
            #endregion

            #region ExcelContrat
            // Bundle style CreationAffaireNouvelle
            bundles.Add(new StyleBundle("~/Content/AlbExcelContrat").Include(
                 "~/Content/vExcelContrat.css"
                 ));
            // Bundle script CreationAffaireNouvelle
            bundles.Add(new ScriptBundle("~/Scripts/AlbExcelContrat").Include(
                 "~/Scripts/AlbingiaJS/albExcelContrat.js"
                 ));
            #endregion
            System.Web.Optimization.BundleTable.EnableOptimizations = System.Configuration.ConfigurationManager.AppSettings["bundle"]?.ToLower() == true.ToString().ToLower();
        }
    }
}