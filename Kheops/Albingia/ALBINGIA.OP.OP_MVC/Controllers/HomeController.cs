using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.Acceuil;
using Mapster;
using OPServiceContract.ICommon;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class HomeController : ControllersBase<AcceuilPage>
    {
        internal readonly static Regex RegexCanevasId = new Regex(@"^\/?([1-9][0-9]{0,14})" + AlbOpConstants.TEMPLATE_FLAG + "_0_[OP](tabGuid[0-9A-Z-a-z]{32}tabGuid)?$", RegexOptions.Compiled);
        internal readonly static Regex RegexCanevas = new Regex(@"^\/?(\s*CV\w{1,7})_0_O(tabGuid[0-9A-Z-a-z]{32}tabGuid)?(modeNavig[SH]?modeNavig)?$", RegexOptions.Compiled);

        public ActionResult Index(string id = "")
        {
            AffaireId affaireId = null;
            int? codeAvn = null;
            bool createMode = false;
            string modeNavig = null;
            this.model.InitializeGuid();
            if (id.Contains("newWindow"))
            {
                id = WebUtility.UrlDecode(id);
                var temp = id.Replace("newWindow", string.Empty).Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (temp.First() + nameof(Controller) == nameof(SyntheseAffaireController)) {
                    this.model.UrlWin = "/" + string.Join("/", temp);
                }
                else {
                    string target = WebUtility.UrlDecode(temp[2]);
                    affaireId = (new Folder(target.Split('_'))).Adapt<AffaireId>();
                    modeNavig = target.Split(new[] { nameof(modeNavig) }, StringSplitOptions.None)[1];
                    if (modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique) {
                        temp = target.Split(new[] { "addParam" }, StringSplitOptions.None);
                        codeAvn = int.TryParse(GetAddParamValue(temp.Length == 1 ? string.Empty : temp[1], AlbParameterName.AVNID), out int x) ? x : default;
                        affaireId.IsHisto = true;
                    }
                    else {
                        codeAvn = null;
                    }
                    affaireId.NumeroAvenant = codeAvn;
                    if (id.ToLower().Contains("tabguid")) {
                        var idParams = id.Split(new[] { "tabGuid" }, StringSplitOptions.None);
                        idParams[1] = this.model.TabGuid;
                        id = string.Join("tabGuid", idParams);
                    }
                    this.model.UrlWin = id.Replace("newWindow", string.Empty);
                }
                id = null;
            }

            if (AlbOpConstants.ClientWorkEnvironment != AlbOpConstants.OPENV_DEV && id.IsEmptyOrNull()) {
                id = HttpContext.Request["paramWinOpen"];
            }

            Match templateMatches = null;
            if (id.ContainsChars()) {
                templateMatches = RegexCanevasId.Match(id);
                if (templateMatches.Success) {
                    int templateId = int.Parse(templateMatches.Groups[1].Value);

                    // try finding canevas
                    //
                    createMode = true;
                    id += $"tabGuid{this.model.TabGuid}tabGuid";
                }
                else {
                    templateMatches = RegexCanevas.Match(id);
                    if (templateMatches.Success) {
                        // mode canevas
                        var temp = id.Split(new[] { nameof(modeNavig) }, StringSplitOptions.None);
                        modeNavig = temp.Count() > 1 ? temp[1] : string.Empty;
                    }
                }

                //createMode = id.Contains(AlbOpConstants.TEMPLATE_FLAG);
                var parameters = id.Contains("CV")
                  ? string.Format("{0}tabGuid{1}tabGuid", id, this.model.TabGuid)
                  : id;
                string[] param = parameters.Contains("_") ? parameters.Split('_') : null;
                if (param != null && param.Length >= 2) {
                    var type = param[2].Split(new[] { "tabGuid" }, StringSplitOptions.None)[0];
                    affaireId = new AffaireId {
                        CodeAffaire = param[0].Replace("/", ""),
                        NumeroAliment = int.Parse(param[1]),
                        TypeAffaire = type.ParseCode<AffaireType>()
                    };
                }
                // traitement du type de l'acte de gestion et du num avenant
                string addParam = string.Empty;
                string typeTraitement = null;
                string target = createMode ? "/CreationSaisie/Index/" : "/ModifierOffre/Index/";
                if (affaireId?.TypeAffaire == AffaireType.Contrat) {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                        var infoActeGestion = client.Channel.GetInfoActeGstion(affaireId.CodeAffaire, affaireId.NumeroAliment.ToString(), affaireId.TypeAffaire.AsCode());
                        if (!string.IsNullOrEmpty(infoActeGestion)) {
                            var tabInfoActeGes = infoActeGestion.Split('_');
                            codeAvn = int.TryParse(tabInfoActeGes[0], out int a) ? a : default(int?);
                            typeTraitement = tabInfoActeGes[1];
                        }
                    }

                    if (id.Contains("ConsultOnly"))
                    {
                        target = typeTraitement == "AFFNV" ? "/AnInformationsGenerales/Index/" : "/AvenantInfoGenerales/Index/";
                        addParam = $"addParamAVN|||AVNID|{codeAvn}||AVNTYPE|{typeTraitement}||AVNIDEXTERNE|{codeAvn}addParam";
                        parameters = parameters.Replace("_P", "_P_" + addParam);
                        
                    }
                    else
                    {
                        switch (typeTraitement)
                        {
                            case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                                target = createMode ? "/CreationAvenant/Index/" : "/AvenantInfoGenerales/Index/";
                                addParam = $"addParamAVN|||AVNID|{codeAvn}||AVNTYPE|{typeTraitement}||AVNIDEXTERNE|{codeAvn}addParam";
                                parameters = parameters.Replace("_P", "_P_" + addParam);
                                break;
                            case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                                target = createMode ? "/CreationAvenant/Index/" : "/AnnulationQuittances/Index/";
                                addParam = $"addParamAVN|||AVNID|{codeAvn}||AVNTYPE|{typeTraitement}||AVNIDEXTERNE|{codeAvn}addParam";
                                parameters = parameters.Replace("_P", "_P_" + addParam);
                                break;
                            case AlbConstantesMetiers.TRAITEMENT_AFFNV:
                            default:
                                target = createMode ? "/AnCreationContrat/Index/" : "/AnInformationsGenerales/Index/";
                                break;
                        }
                    }
                }
                this.model.UrlWin = target != null ? $"{target}{parameters.Replace("__" + typeTraitement + "_", string.Empty)}" : string.Empty;
            }
            if (!createMode && affaireId != null) {
                MvcApplication.ListeAccesAffaires.Add(new Albingia.Mvc.Common.AccesAffaire {
                    Code = affaireId.CodeAffaire,
                    TabGuid = Guid.Parse(this.model.TabGuid),
                    Type = affaireId.TypeAffaire.AsCode(),
                    Version = affaireId.NumeroAliment,
                    ModeAcces = !(templateMatches?.Success ?? false) || modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique
                        ? Albingia.Mvc.Common.AccesOrigine.Consulter
                        : Albingia.Mvc.Common.AccesOrigine.Modifier,
                    Avenant = codeAvn
                });
            }
            return View(this.model);
        }

        /// <summary>
        /// Redirection uilisé pour l'environnement de DEV (Avoir L'url avec es paramètres)
        /// </summary>
        /// <returns></returns>
        [AlbAjaxRedirect]
        public RedirectToRouteResult RedirectToHomeDev()
        {
            return RedirectToAction("Index", "RechercheSaisie");
        }

    }
}
