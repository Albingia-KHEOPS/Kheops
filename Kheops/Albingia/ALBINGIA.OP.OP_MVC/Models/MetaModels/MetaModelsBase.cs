using Albingia.Common;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaData;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesNavigationArbre;
using FastMember;
using Newtonsoft.Json;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    [Serializable]
    public class MetaModelsBase
    {
        protected static readonly Regex tabGuidRegex = new Regex($"^({PageParamContext.TabGuidKey})?({AlbParameters.GuidPattern})({PageParamContext.TabGuidKey})?$", RegexOptions.Singleline | RegexOptions.Compiled);
        private string provenance;
        private bool isReadOnly;
        protected bool? isConsulting;

        public string DossierCourant
        {
            get => AllParameters.FolderId;
        }

        public virtual string TabGuid
        {
            get
            {
                return AllParameters[PageParamContext.TabGuidKey] ?? string.Empty;
            }
            set
            {
                var match = tabGuidRegex.Match(value);
                string gstr = match.Success ? match.Groups[2].Value : null;
                if (Guid.TryParse(gstr, out Guid g))
                {
                    InitializeGuid(g);
                }
            }
        }
        public string OffreVerrouille { get; set; }
        public string VersionVerrouille { get; set; }
        public string TypeVerrouille { get; set; }
        public string AvnVerrouille { get; set; }
        public string AddParamVerrouille { get; set; }
        public string UserVerrouille
        {
            get
            {
                return AllParameters.LockingUser ?? string.Empty;
            }
            set
            {
                AllParameters.LockingUser = value;
            }
        }
        AlbParameters _allParameters;
        public AlbParameters AllParameters
        {
            get
            {
                if (_allParameters == null)
                {
                    _allParameters = AlbParameters.Parse(null);
                }
                return _allParameters;
            }
            set
            {
                _allParameters = value;
            }
        }

        public string AddParamType
        {
            get
            {
                return AllParameters.Type;
            }
            set
            {
                AllParameters.Type = value;
            }
        }
        public string AddParamValue { get; set; }

        public bool IsForceReadOnly
        {
            get
            {
                return AllParameters.ForceReadonly;
            }
            set
            {
                AllParameters.ForceReadonly = value;
            }
        }
        public bool IsIgnoreReadOnly
        {
            get
            {
                return AllParameters.IgnoreReadonly;
            }
            set
            {
                AllParameters.IgnoreReadonly = value;
            }
        }
        public bool IsAvnRefreshUserUpdate
        {
            get
            {
                return AllParameters.RefreshUserUpdateAvn;
            }
            set
            {
                AllParameters.RefreshUserUpdateAvn = value;
            }
        }

        //ECM : ajout pour les objets sortis (readonly)
        public bool IsObjetSorti { get; set; }

        public string Provenance
        {
            get
            {
                string controller = AllParameters.OriginPage;
                string id = AllParameters.OriginId;
                if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(id))
                {
                    return $"/{controller}/Index/{id}";
                }
                return provenance ?? string.Empty;
            }
            set
            {
                if ((AllParameters.OriginPage).IsEmptyOrNull())
                {
                    provenance = value;
                }
            }
        }

        public string Contexte
        {
            get
            {
                return AllParameters.ContextClausier;
            }
            set
            {
                AllParameters.ContextClausier = value;
            }
        }

        public bool SaveAndQuit { get; set; }
        public bool IsHisto { get; set; }
        public string MessageErreur { get; set; }
        public string EtapeEcran { get; set; }
        public string PageEnCours { get; set; }
        public string PageTitle { get; set; }
        public string TitleInfoBulle { get; set; }
        public string SelectionPossible { get; set; }
        public string ModaliteAffichage { get; set; }
        public string Parameters { get; set; }
        public string SpecificParameters { get; set; }
        public int Date { get; set; }
        public Bandeau_MetaData Bandeau { get; set; }
        public Navigation_MetaModel Navigation { get; set; }
        public Offre_MetaModel Offre { get; set; }
        public ContratDto Contrat { get; set; }
        public string CodeBranche => Offre?.Branche?.Code ?? Contrat?.Branche ?? AllParameters?[AlbParameterName.BRANCHEOFFRE] as string ?? string.Empty;
        public string CodeCible => Offre?.Branche?.Cible?.Code ?? Contrat?.Cible ?? string.Empty;

        //Concerne la partie Création/Modification des informations de base d'un contrat
        public ContratInfoBaseDto ContratInfoBase { get; set; }
        public bool AfficherBandeau { get; set; }
        public bool AfficherNavigation { get; set; }
        public bool AfficherArbre { get; set; }
        public bool LoadMenu { get; set; }
        public bool LoadEntete { get; set; }
        public bool LoadParamOffre { get; set; }
        public ModeleNavigationArbre NavigationArbre { get; set; }

        public int SwitchHardLog
        {
            get => MvcApplication.SWITCH_HARD_LOG;
        }

        public virtual bool IsReadOnly {
            get {
                return this.isReadOnly;
            }
            set {
                this.isReadOnly = value;
                if (NavigationArbre != null) {
                    NavigationArbre.IsReadOnly = IsReadOnly;
                }
            }
        }

        virtual public bool IsModifHorsAvenant { get; set; }
        public bool IsOffreSimplifiee { get; set; }

        public string ModeNavig
        {
            get
            {
                return AllParameters?[AlbParameterName.MODENAVIG] as string ?? string.Empty;
            }
            set
            {
                AllParameters[AlbParameterName.MODENAVIG] = value;
            }
        }

        public string AccessMode
        {
            get
            {
                return AllParameters?[AlbParameterName.ACCESSMODEENG] as string ?? string.Empty;
            }
            set
            {
                if (AllParameters == null)
                {
                    AllParameters = AlbParameters.Parse(null);
                }
                AllParameters[AlbParameterName.ACCESSMODEENG] = value;
            }
        }

        public string CodePolicePage { get; set; }
       
        public string VersionPolicePage { get; set; }
        public string TypePolicePage { get; set; }

        public void InitPoliceId() {
            CodePolicePage = (Offre?.CodeOffre ?? Contrat?.CodeContrat) ?? CodePolicePage;
            if (VersionPolicePage.IsEmptyOrNull()) {
                VersionPolicePage = ((Offre?.Version ?? Contrat?.VersionContrat) ?? 0).ToString();
            }
            TypePolicePage = (Offre?.Type ?? Contrat?.Type) ?? TypePolicePage;
        }

        public string NumAvenantPage
        {
            get
            {
                return AllParameters.NumeroAvenant.StringValue(default(int).ToString());
            }
            set
            {
                int.TryParse(value, out int num);
                AllParameters.NumeroAvenant = num;
            }
        }
        public int NumAvenant => AllParameters.NumeroAvenant.GetValueOrDefault();

        public string NumAvenantExterne
        {
            get
            {
                return AllParameters.NumeroAvenantExterne.StringValue(default(int).ToString());
            }
            set
            {
                int.TryParse(value, out int num);
                AllParameters.NumeroAvenantExterne = num;
            }
        }
        public bool IsValidation
        {
            get
            {
                return AllParameters.IsValidation;
            }
            set
            {
                AllParameters.IsValidation = value;
            }
        }
        public string RedirectRsq { get; set; }
        public string LayoutModeAvt { get; set; }

        internal AffaireId AffaireId => CodePolicePage.IsEmptyOrNull() ? null
            : new AffaireId {
                CodeAffaire = CodePolicePage,
                IsHisto = ModeNavig == ModeConsultation.Historique.AsCode(),
                NumeroAliment = int.TryParse(VersionPolicePage, out int i) && i >= 0 ? i : default,
                NumeroAvenant = int.TryParse(NumAvenantPage, out i) && i > 0 ? i : default(int?),
                TypeAffaire = TypePolicePage.ParseCode<AffaireType>()
            };

        public static string GetJsonFromData<TObj>(TObj obj)
        {
            var settings = new JsonSerializerSettings();
            settings.DateFormatString = AlbConvert.AppCulture.DateTimeFormat.ShortDatePattern + ' ' + AlbConvert.AppCulture.DateTimeFormat.LongTimePattern;
            return JsonConvert.SerializeObject(obj, settings);
        }

        public string GetUniversalContratCode()
        {
            string universalContratCode = String.Empty;
            if (!String.IsNullOrWhiteSpace(CodePolicePage))
            {
                universalContratCode = CodePolicePage + "_" + VersionPolicePage + "_" + TypePolicePage + "_" + NumAvenantPage;
            }
            else if (Offre != null && !String.IsNullOrWhiteSpace(Offre.CodeOffre))
            {
                universalContratCode = Offre.CodeOffre + "_" + Offre.Version + "_" + Offre.Type + "_" + NumAvenantPage;
            }
            else if (Contrat != null && !String.IsNullOrWhiteSpace(Contrat.CodeContrat))
            {
                universalContratCode = Contrat.CodeContrat + "_" + Contrat.VersionContrat + "_" + Contrat.Type + "_" + NumAvenantPage;
            }
            else if (ContratInfoBase != null && !String.IsNullOrWhiteSpace(ContratInfoBase.CodeContrat))
            {
                universalContratCode = ContratInfoBase.CodeContrat + "_" + ContratInfoBase.VersionContrat + "_" + ContratInfoBase.Type + "_" + NumAvenantPage;
            }

            return universalContratCode;
        }

        public string NameUser
        {
            get { return AlbSessionHelper.ConnectedUser ?? "#Identification Error: Identité utilisateur == null#"; }
        }

        public TimeSpan? AlbTime { get; set; }

        public string ScreenType { get; set; }
        public string txtSaveCancel { get; set; }


        public string ActeGestion
        {
            get
            {
                return AllParameters.TypeAvenant;
            }
            set
            {
                AllParameters.TypeAvenant = value;
            }
        }

        public string ActeGestionRegule
        {
            get
            {
                return AllParameters.ActeDeGestion;
            }
            set
            {
                AllParameters.ActeDeGestion = value;
            }
        }

        public virtual bool IsModeConsultationEcran
        {
            get
            {
                if (this.isConsulting.HasValue)
                {
                    return this.isConsulting.Value;
                }
                return PageEnCours != NomsInternesEcran.RechercheSaisie.ToString() && AlbTransverse.GetIsReadOnlyScreen(TabGuid, AffaireUniqueId);
            }
            set
            {
                this.isConsulting = value;
            }
        }

        public MetaModelsBase(bool afficherNavigation = false, bool afficherBandeau = false, bool afficherArbre = false)
        {
            PageEnCours = string.Empty;
            PageTitle = string.Empty;
            Bandeau = null;
            AfficherBandeau = afficherBandeau;
            AfficherNavigation = afficherNavigation;
            AfficherArbre = afficherArbre;
            LoadMenu = true;
            LoadEntete = true;
        }

        public MetaModelsBase(string specificMessage, bool afficherNavigation = false, bool afficherBandeau = false, bool afficherArbre = false)
        {
            PageTitle = specificMessage;
            Bandeau = null;
            Navigation = null;
            AfficherBandeau = afficherBandeau;
            AfficherNavigation = afficherNavigation;
            AfficherArbre = afficherArbre;
            LoadMenu = true;
            LoadEntete = true;
            NumAvenantPage = "0";
            VersionVerrouille = "0";
        }

        internal void InitializeGuid(Guid? newGuid = null)
        {
            if (AllParameters == null)
            {
                AllParameters = AlbParameters.Parse(null);
            }
            if (AllParameters[PageParamContext.TabGuidKey].ContainsChars())
            {
                return;
            }
            AllParameters[PageParamContext.TabGuidKey] = newGuid.GetValueOrDefault(Guid.NewGuid()).ToString("N");
        }

        public string AffaireUniqueId
        {
            get => string.Format(
                "{0}_{1}_{2}_{3}",
                CodePolicePage,
                VersionPolicePage,
                TypePolicePage,
                NumAvenantPage.IsEmptyOrNull() ? "0" : NumAvenantPage);
        }
        public ProfileKheops ProfileKheops { get; internal set; }

        public void CopyTo(object targetModel) {
            if (targetModel is null) {
                throw new ArgumentNullException(nameof(targetModel));
            }
            if (!targetModel.GetType().IsSubclassOf(typeof(MetaModelsBase))) {
                throw new ArgumentException(nameof(targetModel));
            }
            var target = ObjectAccessor.Create(targetModel);
            var source = ObjectAccessor.Create(this);
            var currentType = GetType();
            // init main context params missing
            if (CodePolicePage.IsEmptyOrNull()) {
                InitPoliceId();
            }
            bool isExactSameType = targetModel.GetType() == currentType;
            // set first AllParameters main params object
            target[nameof(AllParameters)] = source[nameof(AllParameters)];
            var baseMembers = TypeAccessor.Create(isExactSameType ? currentType : typeof(MetaModelsBase)).GetMembers().ToList();
            baseMembers.ForEach(m => {
                if (m.CanWrite && !m.Name.IsIn(nameof(AllParameters), nameof(ProfileKheops))) {
                    target[m.Name] = source[m.Name];
                }
            });
            if (!isExactSameType) {
                var members = TypeAccessor.Create(currentType).GetMembers()
                    .Where(m => !baseMembers.Any(x => x.Name == m.Name))
                    .ToList();
                TypeAccessor.Create(targetModel.GetType()).GetMembers()
                    .Where(m => members.Any(x => m.CanWrite && x.Type == m.Type && x.Name == m.Name))
                    .ToList()
                    .ForEach(m => target[m.Name] = source[m.Name]);
            }
        }
    }
}
