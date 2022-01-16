using System.Web.Mvc;


namespace ALBINGIA.OP.OP_MVC.AlbSpecificAttribute
{
  /// <summary>
  /// Attribut de vérification de vérouillage d'une offre
  /// </summary>
  public class AlbAuthorize :  AuthorizeAttribute
  {
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
      base.OnAuthorization(filterContext);
      if (filterContext.Result is HttpUnauthorizedResult)
      {
        filterContext.Result = new RedirectResult("~/Public/AccesDenied.html");
        
      }
    }

    //#region Variables Membres
    //public readonly string _actionName;
    //public readonly string _controllerName;
    //public readonly string _numeroOffre = string.Empty;
    //public readonly bool _useSemaphore;
    //public string _type;
    //#endregion
    //#region Propriétés
    //public string RedirectPath { get; set; }
    //public static string SplitChar { get; set; }
    //#endregion
    //#region Constructeur
    //public AlbVerifLockedOffer(string numeroOffre, string type = "O", bool useSemaphore = true, string actionName = "", string controllerName = "")
    //{
    //  _actionName = actionName;
    //  _controllerName = controllerName;
    //  _numeroOffre = numeroOffre;
    //  _useSemaphore = useSemaphore;
    //  _type = type;
    //}
    //#endregion
    //#region Méthodes publiques
    ///// <summary>
    ///// Cette méthode se délenche avant l'xcécuation d'une action sur laquelle on veut appliquer cette attribut de vérification
    ///// de vérrouillage de l'offre
    ///// </summary>
    ///// <param name="filterContext">Contexte</param>
    //public override void OnActionExecuting(ActionExecutingContext filterContext)
    //{

    //  UncodeUrlParams(filterContext);
    //  if (filterContext.HttpContext.Request.UrlReferrer != null
    //    && (filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("recherchesaisie") || filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("recherche") ||
    //    filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri.ToLower().Contains("confirmationsaisie")) && filterContext.HttpContext.Request.CurrentExecutionFilePath.ToLower().Contains("creationsaisie"))
    //    base.OnActionExecuting(filterContext);
    //  else
    //  {
    //    if (!_useSemaphore)
    //    {
    //      base.OnActionExecuting(filterContext);
    //    }
    //    else
    //    {
    //      bool firstScreen = GetFirstScreen(filterContext.RequestContext);
    //      object valeurNumeroOffre;
    //      if ((filterContext.ActionParameters.Count > 0) &&
    //          filterContext.ActionParameters.TryGetValue(_numeroOffre, out valeurNumeroOffre))
    //      {
    //        if (string.IsNullOrEmpty((string)valeurNumeroOffre))
    //        {
    //          base.OnActionExecuting(filterContext);
    //        }
    //        else
    //        {
    //          var user = AlbSessionHelper.ConnectedUser;
    //          var guitTab = string.Empty;
    //          var sValeurNumeroOffre = ((string)valeurNumeroOffre).Split(new[] { '_' });
    //          _type = sValeurNumeroOffre[2];
    //          var readOnlyRequest = false;
    //          if (valeurNumeroOffre.ToString().Contains("tabGuid"))
    //          {

    //            var queryParams = ((string)valeurNumeroOffre).Split(new[] { "tabGuid" }, StringSplitOptions.None);
    //            guitTab = queryParams[1];
    //            readOnlyRequest = ((string)valeurNumeroOffre).ToLower().Contains("readonly");
    //            sValeurNumeroOffre = queryParams[0].Split(new[] { '_' });
    //            _type = sValeurNumeroOffre[2];
    //          }
    //          if (_type.Length >= 1)
    //            _type = _type.Substring(0, 1);

    //          // Traitement pur une offre valide
    //          var ifValideFolder = AlbUserRoles.GetValiditeOffre(sValeurNumeroOffre[0], sValeurNumeroOffre[1], _type);

    //          string lockUser;
    //          bool isIncache;
    //          var offerisIncache = OfferAccesAuthorization.OfferInCache(user, sValeurNumeroOffre[0], sValeurNumeroOffre[1], sValeurNumeroOffre[2], SplitChar,
    //                                                   out lockUser, out isIncache);
    //          //if(offerisIncache)
    //          if (string.IsNullOrEmpty(lockUser))
    //          {
    //            using (var voletsBlocsCategoriesClient = new VoletsBlocsCategoriesClient())
    //            {
    //              lockUser = voletsBlocsCategoriesClient.GetUserVerrou(sValeurNumeroOffre[0],
    //                                                                   sValeurNumeroOffre[1], sValeurNumeroOffre[2]);
    //            }
    //          }


    //          lockUser = string.IsNullOrEmpty(lockUser) ? user : lockUser;
              
             
    //            ApplyProfile(filterContext.RequestContext.RouteData.Values, sValeurNumeroOffre[0],
    //              sValeurNumeroOffre[1], _type, guitTab, lockUser, ifValideFolder:ifValideFolder);
              
              
    //          if (!ifValideFolder && GetFirstScreen(filterContext.RequestContext) && !readOnlyRequest && offerisIncache
    //            && (AlbSessionHelper.IsSameUserTabFolder(guitTab, string.Format("{0}_{1}_{2}", sValeurNumeroOffre[0],
    //            sValeurNumeroOffre[1], sValeurNumeroOffre[2]))
    //            || (!string.IsNullOrEmpty(lockUser) && lockUser != user)))
    //          {
    //            //Néttoyage du dictionnaire 
    //            DeleteTabGuid(guitTab, sValeurNumeroOffre[0] + "_" + sValeurNumeroOffre[1] + "_" + _type,
    //                          lockUser);
    //            var responseUrl = new StringBuilder();
    //            responseUrl.Append("/Security/Index/" + sValeurNumeroOffre[0] + "_" + sValeurNumeroOffre[1] + "_" + _type + "_" +
    //                               lockUser);
    //            filterContext.Result = new RedirectResult(responseUrl.ToString());
    //          }
    //          else
    //          {


    //            //if (!isIncache && (lockUser == user || string.IsNullOrEmpty(lockUser)))
    //            //  if (!isIncache)
    //            //{
    //            // si l'offre n'est pas dans le cache on vérifie la BD
    //            List<OffreVerouilleeDto> offresVerouilles;
    //            using (var voletsBlocsCategoriesClient = new VoletsBlocsCategoriesClient())
    //            {


    //              offresVerouilles =
    //                voletsBlocsCategoriesClient.GetOffresVerouillees(false, false, string.Empty,
    //                                                                 string.Empty,
    //                                                                 string.Empty, null, null).ToList();


    //            }

    //            //MAJ CACHE
    //            offresVerouilles.ForEach(el => OfferAccesAuthorization.VerouillerOffre(el.Utilisateur, el.NumOffre,
    //                                                                                   el.Version.ToString(
    //                                                                                     CultureInfo.CurrentCulture),
    //                                                                                   el.Type, SplitChar));


    //            var selectedOffre =
    //              offresVerouilles.FirstOrDefault(
    //                el => el.Type == _type && el.NumOffre.Trim() == sValeurNumeroOffre[0].Trim()
    //                      && el.Version == Convert.ToInt32(sValeurNumeroOffre[1]));

    //            if (selectedOffre == null)
    //            {
    //              using (var voletsBlocsCategoriesClient = new VoletsBlocsCategoriesClient())
    //              {
    //                voletsBlocsCategoriesClient.AjouterOffreVerouille("PRODU", _type,
    //                                                                  sValeurNumeroOffre[0].PadLeft(9, ' '),
    //                                                                  Convert.ToInt32(
    //                                                                    sValeurNumeroOffre[1]),
    //                                                                  0, 0, 0, "", "Contrat", "GESTION",
    //                                                                  "O",
    //                                                                  user,
    //                                                                  string.Format(
    //                                                                    "Verrouillage de l'affaire à {0}",
    //                                                                    DateTime.Now.ToString(
    //                                                                      CultureInfo.CurrentCulture)));
    //              }
    //              //lockUser = string.IsNullOrEmpty(lockUser)?user:lockUser;
    //            }
    //            else
    //            {

    //              //MAJ de tous le cache offre
    //              if (lockUser != selectedOffre.Utilisateur)
    //              {
    //                OfferAccesAuthorization.RemoveCacheOffre(MvcApplication.SPLIT_CONST_HTML);
    //                offresVerouilles.ForEach(offre => OfferAccesAuthorization.VerouillerOffre(user, offre.NumOffre,
    //                  offre.Version.ToString(CultureInfo.CurrentCulture), offre.Type, MvcApplication.SPLIT_CONST_HTML));
    //              }
    //              lockUser = string.IsNullOrEmpty(lockUser) ? selectedOffre.Utilisateur : lockUser;
    //            }

    //            if (!readOnlyRequest && selectedOffre == null
    //              && (AlbSessionHelper.IsSameUserTabFolder(guitTab, string.Format("{0}_{1}_{2}",
    //              sValeurNumeroOffre[0], sValeurNumeroOffre[1], sValeurNumeroOffre[2])) || lockUser != user))
    //            {
    //              OfferAccesAuthorization.VerouillerOffre(user, sValeurNumeroOffre[0], sValeurNumeroOffre[1],
    //                                                      _type, SplitChar);
    //              base.OnActionExecuting(filterContext);

    //            }
    //            else
    //            {
                 
    //                ApplyProfile(filterContext.RequestContext.RouteData.Values, sValeurNumeroOffre[0],
    //                  sValeurNumeroOffre[1], _type, guitTab, lockUser, ifValideFolder:ifValideFolder);
                  
    //              if (!ifValideFolder && GetFirstScreen(filterContext.RequestContext) && !readOnlyRequest && (AlbSessionHelper.IsSameUserTabFolder(guitTab, string.Format("{0}_{1}_{2}",
    //                sValeurNumeroOffre[0], sValeurNumeroOffre[1], _type)) || lockUser != user))
    //              {
    //                //Néttoyage du dictionnaire 
    //                DeleteTabGuid(guitTab, sValeurNumeroOffre[0] + "_" + sValeurNumeroOffre[1] + "_" + _type,
    //                              lockUser);
    //                var responseUrl = new StringBuilder();
    //                responseUrl.Append("/Security/Index/" + sValeurNumeroOffre[0] + "_" + sValeurNumeroOffre[1] + "_" + _type + "_" +
    //                                   lockUser);
    //                filterContext.Result = new RedirectResult(responseUrl.ToString());
    //              }
    //            }
    //            //if (!toSecurityController )
    //            //{

    //            //  ApplyProfile(filterContext.RequestContext.RouteData.Values, sValeurNumeroOffre[0],
    //            //               sValeurNumeroOffre[1], _type, guitTab);
    //            //}
    //            //}
    //          }
    //        }

    //      }
    //      else
    //      {
    //        base.OnActionExecuting(filterContext);
    //      }
    //    }
    //  }
    //}
    //#endregion
    //# region Méthode privées

    ///// <summary>
    ///// Decode les paramètres de l'url
    ///// </summary>
    ///// <param name="filterContext">Contexte controller</param>
    //private static void UncodeUrlParams(ActionExecutingContext filterContext)
    //{
    //  var newValuesParams = new RouteValueDictionary();
    //  foreach (
    //    var val in
    //      filterContext.RouteData.Values.Where(el => el.Key.ToLower() != "controller" && el.Key.ToLower() != "action"))
    //  {
    //    newValuesParams.Add(val.Key, filterContext.HttpContext.Server.UrlDecode(val.Value.ToString()));
    //  }
    //  newValuesParams.ToList().ForEach(elm =>
    //    {
    //      filterContext.RouteData.Values[elm.Key] = elm.Value;

    //      if (filterContext.ActionParameters.Any(el => el.Key == elm.Key))
    //        filterContext.ActionParameters[elm.Key] = elm.Value;
    //    });
      
    //}
    ///// <summary>
    /////    Vérifie si c'est le premier écran
    ///// </summary>
    ///// <param name="requestContext">Contexte de la requête</param>
    ///// <returns></returns>
    //private static bool GetFirstScreen(RequestContext requestContext)
    //{
    //  var urlRequest = requestContext.HttpContext.Request.Url;
    //  return urlRequest.AbsoluteUri.ToLower().Contains("modifieroffre") ||
    //          urlRequest.AbsoluteUri.ToLower().Contains("aninformationsgenerales");
    //}

    ///// <summary>
    ///// Mise à jour du profil de l'utilisateur
    ///// </summary>
    ///// <param name="routeValue">Variable contenant le controlleur, actions et les paramètres</param>
    ///// <param name="numeroOffre">Numéro d el'offre à vérifier</param>
    ///// <param name="version">Version de l'offre</param>
    ///// <param name="type">Type</param>
    ///// <param name="guid">Guid de la fenêtre du navigateur courante</param>
    ///// <param name="lockUser">Utilisateur vérrouillant l'offre</param>
    ///// <param name="firstScreen">Indique c'est le premier écran Offre/Contrat</param>
    ///// <param name="ifValideFolder">Si l'offre ou le contrat est valide</param>
    //private static void ApplyProfile(IEnumerable<KeyValuePair<string, object>> routeValue
    //    , string numeroOffre, string version, string type, string guid, string lockUser = "", bool firstScreen = false,bool ifValideFolder=false)
    //{

    //  var values = routeValue.ToList();

    // // var controllerName = values.FirstOrDefault(elm => elm.Key == "controller");

    //  if (string.IsNullOrEmpty(guid) || guid.Contains("tabGuidtabGuid")) return;
    //  var user = AlbSessionHelper.ConnectedUser;

    //  Common.AlbUserRoles.SetUserProfil(guid, user, numeroOffre, version, type, lockUser, firstScreen,ifValideFolder);
    //}

    ///// <summary>
    ///// Set the response result removing the GuidTab parameter
    ///// </summary>
    ///// <param name="filterContext"></param>
    ///// <returns></returns>
    //private static string SetResult(ActionExecutingContext filterContext)
    //{
    //  var routeValues = (((RedirectToRouteResult)(filterContext.Result))).RouteValues;
    //  if (routeValues == null)
    //    return string.Empty;
    //  var values = routeValues.ToList();
    //  var controller = string.Empty;
    //  var action = string.Empty;
    //  var strProvider = new StringBuilder();
    //  var guidTab = string.Empty;

    //  foreach (var value in values)
    //  {

    //    switch (value.Key)
    //    {
    //      case "action":
    //        action = value.Value.ToString();
    //        break;
    //      case "controller":
    //        controller = value.Value.ToString();
    //        break;
    //      default:
    //        var id = value.Value;
    //        if (value.Key == "id" && value.Value.ToString().Contains("tabGuid"))
    //        {
    //          var elms = value.Value.ToString().Split(new[] { "tabGuid" }, StringSplitOptions.None);
    //          id = elms[0];
    //          guidTab = elms[1];
    //        }
    //        //  guidTab = "_||_" + value.Value;
    //        //else
    //        //{
    //        strProvider.Append("/" + id);
    //        break;
    //    }
    //    var responseUrl = new StringBuilder();
    //    responseUrl.Append(string.Format("/{0}/{1}{2}", controller, action, strProvider));
    //    filterContext.Result = new RedirectResult(responseUrl.ToString());
    //  }
    //  return guidTab;
    //}

    ///// <summary>
    ///// Supprime un une ligne (tab courante) du dictionnaires des affaires consultées
    ///// </summary>
    ///// <param name="tabGuid">Guid de la tabulation du browser</param>
    ///// <param name="currentFolder">numéro de l'offre_version_type</param>
    ///// <param name="lockUser">Utilisateur qui vverouile l'offre</param>
    //private static void DeleteTabGuid(string tabGuid, string currentFolder, string lockUser)
    //{
    //  if (AlbSessionHelper.ConnectedUser == lockUser)
    //    AlbSessionHelper.CurrentFolders.Remove(tabGuid + "_" + AlbSessionHelper.ConnectedUser + "_" + currentFolder);
    //}
    //#endregion
  }
}
