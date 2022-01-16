using System.Runtime.Serialization;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Models.Common;

namespace ALBINGIA.Framework.Common.Constants
{
    public class AlbOpConstants {
        #region Constantes UploadFiles

        //public static string UploadPathInventory { get; set; }        
        //public static string UploadParthReturnedDocument { get; set; }
        public static string UploadPathDocumentGestion { get; set; }
        public static string UrlDocViewer { get; set; }

        #endregion
        #region Constantes déploiements
        public const string OPENV_DEV = "DEV";
        public const string OPENV_QUALIF = "Qualif";
        public const string OPENV_SPP = "SUPPORT";
        public const string OPENV_HOTFIX = "HotFix";
        public const string OPENV_PROD = "Production";
        public const string OPENV_FORMATION = "Formation";
        public const string OPENV_PREPROD = "PreProd";
        #endregion

        #region Types Alertes
        public const string SUSPEN = "SUSPENS";
        public const string QUITT = "IMPAYES";
        public const string ENGCNX = "ENGAGCNX";
        public const string INTERV = "INTERV";

        #endregion

        #region Constantes Gestion des erreurs
        public const string OPENV_GENERAL_ERROR_MESSAGE = "Erreur Albingia - Veuillez contacter votre administrateur";
        public const string OPENV_GENERAL_INFOS_MESSAGE = "Information Albingia";
        public const string REDIRECT_BROWSER_ERROR = "browser";
        public const string REDIRECT_ACCESDENIED_ERROR = "accesDenied";
        public const string CABINET_GESTIONNAIRE_EMPTY_ERROR = "Courtier gestionnaire indéfini";
        #endregion
        #region Constantes Formatage Numeric
        public const string FormatDecimalInput = "{ digitGroupSeparator: '', decimalCharacter: ',', decimalPlacesOverride: 2, maximumValue: '99999999999.99' }";
        public const string FormatDecimalSpan = "{ digitGroupSeparator: ' ', decimalCharacter: ',', decimalPlacesOverride: 2, maximumValue: '99999999999.99' }";
        public const string FormatPourcentInput = "{ digitGroupSeparator: '', decimalCharacter: ',', decimalPlacesOverride: 2, maximumValue: '100.00', minimumValue: '0.00' }";
        public const string FormatPourcentSpan = "{ digitGroupSeparator: ' ', decimalCharacter: ',', decimalPlacesOverride: 2, maximumValue: '100.00', minimumValue: '0.00' }";
        public const string FormatPourmilleInput = "{ digitGroupSeparator: '', decimalCharacter: ',', decimalPlacesOverride: 2, maximumValue: '1000.00', minimumValue: '0.00' }";
        public const string FormatPourmilleSpan = "{ digitGroupSeparator: ' ', decimalCharacter: ',', decimalPlacesOverride: 2, maximumValue: '1000.00', minimumValue: '0.00' }";
        #endregion
        #region Constantes Templates
        /// <summary>
        /// Flag signalant que l'on modifie une offre ou un contrat de type template
        /// </summary>
        public static string TEMPLATE_FLAG = "albTemplate";
        #endregion

        public const string RCFrance = "RCFR";
        public const string RCUSA = "RCUS";
        public const string RCExport = "RCEX";
        public const string USA_CAN = "USACAN";
        public static readonly string[] AllRCGar = new[] { RCFrance, RCUSA, RCExport };

        public static string UrlWinPageTarget { get; set; }
        public static string ClientWorkEnvironment { get; set; }
        public static string ApplicationName { get; set; }
        public static MailModel MainInfoContent { get { return MailModel.InfosMail; } }
        public static bool WindowEventLog { get; set; }
        public static bool FileLog { get; set; }
        public static string JsCsVersion { get; set; }
        public static string PrefixPathDocuments { get; set; }
        public static bool OtherBrowsers { get; set; }
        public static bool NotificationMail { get; set; }
        public static string GdmAppName { get; set; }
        public static string CheminIntercalaire { get; set; }
        public static string CheminDocContract { get; set; }
        public static string ExternalApp { get; set; }
        public static string IgnoreISBranche { get; set; }
        public static bool OfficeViewer { get; set; }

        #region Gest Utilisateurs
        public const string SUPER_BRANCHE = "**";

        /// <summary>
        /// Liste des actions qui sont disponibles par défaut (même sans aucun droit)
        /// </summary>
        private static System.Collections.Generic.List<string> _listActionsModeConsultation;
        public static System.Collections.Generic.List<string> ListActionsModeConsultation
        {
            get
            {
                if (_listActionsModeConsultation != null)
                    return _listActionsModeConsultation;
                else
                {
                    var lstAction = new System.Collections.Generic.List<string>();
                    lstAction.Add("consulter");
                    lstAction.Add("documents");
                    //Ajout ici des TEXTES des menus à ajouter pour les droits de visualisation
                    _listActionsModeConsultation = lstAction;
                    return _listActionsModeConsultation;
                }
            }
        }
        #endregion

        #region Constantes redirection details risque

        public static string REDIRECTRSQ_OPEN_IS = "RedirectRsqOpenIS";
        public static string REDIRECTRSQ_OPEN_OBJ = "RedirectRsqOpenObj";
        public static string REDIRECTRSQ_ADD_OBJ = "RedirectRsqAddObj";

        #endregion

        #region Paramètres additionnels transverses

        public const string GLOBAL_TYPE_ADD_PARAM_AVN = "AVN";

        #endregion
        #region LockUsers
        public const string LOCKUSER_SPLITSTR = "LUSR";
        #endregion
        #region périodes

        public const string Jour = "J";
        public const string Mois = "M";
        public const string Annee = "A";

        #endregion
    }
}