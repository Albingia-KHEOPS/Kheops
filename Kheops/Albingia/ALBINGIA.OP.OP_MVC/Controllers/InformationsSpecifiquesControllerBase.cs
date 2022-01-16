using Albingia.Common;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.InfosSpecifiques;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.CustomResult;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Ajax;
using ALBINGIA.OP.OP_MVC.Helpers;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using HtmlAgilityPack;
using OPServiceContract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class InformationsSpecifiquesControllerBase<T> : RemiseEnVigueurController<T> where T : MetaModelsBase
    {
        public override bool IsReadonly => base.IsReadonly || Model.IsReadOnly;
        public override bool AllowUpdate
        {
            get
            {
                return !GetIsReadOnly(this.model.TabGuid, $"{Model.CodePolicePage}_{Model.VersionPolicePage}_{Model.TypePolicePage}")
                    && !IsModifHorsAvenant;
            }
        }

        [HttpPost]
        public PartialViewResult GetISHtml(AffaireId affaireId, int risque = 0, int objet = 0, int option = 0, int formule = 0)
        {
            InfosSpecifiquesDto infos = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfosSpecifiquesPort>())
            {
                if (formule > 0)
                {
                    infos = client.Channel.GetByAffaireAndSection(affaireId, new SectionISDto { Type = TypeSection.Garanties, NumeroRisque = risque, NumeroFormule = formule, NumeroOption = option });
                }
                else if (objet > 0)
                {
                    infos = client.Channel.GetByAffaireAndSection(affaireId, new SectionISDto { Type = TypeSection.Objets, NumeroRisque = risque, NumeroObjet = objet });
                }
                else if (risque > 0)
                {
                    infos = client.Channel.GetByAffaireAndSection(affaireId, new SectionISDto { Type = TypeSection.Risques, NumeroRisque = risque });
                }
                // Reactiver les IS entête
                else
                    infos = client.Channel.GetByAffaireAndSection(affaireId, new SectionISDto { Type = TypeSection.Entete});
            }

            return PartialView("_InfosSpecifiques", infos);
        }

        [HttpPost]
        public JsonResult GetOldISList()
        {
            (AffaireId affaire, SectionISDto section)[] list = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfosSpecifiquesPort>())
            {
                list = client.Channel.GetExistingOldIS(3000).ToArray();
            }
            if (!list?.Any() ?? true)
            {
                return Json(null);
            }
            return JsonNetResult.NewResultToGet(list.Select(x => new { x.affaire, x.section }));
        }

        [HttpPost]
        public void LogOldISState(AffaireId affaireId, SectionISDto section, string commentaires)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfosSpecifiquesPort>())
            {
                client.Channel.TraceOldISTransfert(affaireId, section, commentaires);
            }
        }

        [HttpPost]
        [AlbVerifLockedOffer("id")]
        public JsonResult InitISVAL(string initGuid, int nbAffaires = 1000, bool fromHisto = false)
        {
            List<(AffaireId affaire, SectionISDto section)> list = null;
            var exceptions = new ConcurrentBag<(Exception ex, AffaireId affaire, SectionISDto section)>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfosSpecifiquesPort>())
            {
                list = client.Channel.GetExistingOldIS(nbAffaires, fromHisto).ToList();
                var (affaire, section) = list.First();
                client.Channel.TraceOldISTransfert(affaire, section, $"{nameof(InitISVAL)}-{initGuid}");

                foreach (var x in list)
                {
                    try
                    {
                        if (!client.Channel.HasOldISTransfertLogId(Guid.Parse(initGuid)))
                        {
                            break;
                        }
                        InitISVALFromOldHtml(x.affaire, x.section);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add((e, x.affaire, x.section));
                    }
                }
            }

            /* int nbSlices = 5;
            var stack = new Stack<(AffaireId affaire, SectionISDto section)>(list);
            int nbMax = stack.Count / nbSlices;
            var listOfLists = new List<List<(AffaireId affaire, SectionISDto section)>>();
            while (stack.Any()) {
                int n = 0;
                var l = new List<(AffaireId affaire, SectionISDto section)>();
                while (n < nbMax && stack.Any()) {
                    l.Add(stack.Pop());
                    n++;
                }
                listOfLists.Add(l);
            }
            var tasks = listOfLists.Select(array => {
                var arrayList = array.ToList();
                return new Task(() => {
                    arrayList.ToList().ForEach(x => {
                        try {
                            InitISVALFromOldHtml(x.affaire, x.section);
                        }
                        catch (Exception e) {
                            exceptions.Add((e, x.affaire, x.section));
                        }
                    });
                });
            });
            Task.WhenAll(tasks.Select(tk => { tk.Start(); return tk; })).Wait(); */

            if (exceptions.Any())
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfosSpecifiquesPort>())
                {
                    exceptions.ToList().ForEach(x => client.Channel.TraceOldISTransfert(x.affaire, x.section, $"{x.section.Branche}-{x.section.Type}-ERROR({x.ex.Message})"));
                }
            }
            return JsonNetResult.NewResultToGet(exceptions);
        }

        [HttpPost]
        public void CancelInitISVAL(Guid initGuid)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfosSpecifiquesPort>())
            {
                try
                {
                    client.Channel.CancelOldISTransfert(initGuid);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error in {nameof(CancelInitISVAL)}: {Environment.NewLine}{ex}");
                }
            }
        }

        [HttpPost]
        [HandleJsonError]
        public string SaveIS(AffaireId affaireId, InfosSpecifiquesDto infos, string tabGuid)
        {
            Model.CodePolicePage = affaireId.CodeAffaire;
            Model.VersionPolicePage = affaireId.NumeroAliment.ToString();
            Model.TypePolicePage = affaireId.TypeAffaire.AsCode();
            Model.NumAvenantPage = affaireId.NumeroAvenant.ToString();
            var initGuid = new Guid("{2EF60657-76BD-4730-AA10-66B17E347394}");
            var originGuid = new Guid(tabGuid.Replace(PageParamContext.TabGuidKey, string.Empty));
            Model.TabGuid = originGuid.ToString("N");
            if (AllowUpdate && (infos?.Infos?.Any(x => x.AffaireId != null) ?? false) || initGuid == originGuid)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfosSpecifiquesPort>())
                {
                    client.Channel.SaveIS(infos);
                }
            }
            return string.Empty;
        }

        internal static bool HasIS(AffaireId affaireId, int risque, int objet = 0, int option = 0, int formule = 0)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfosSpecifiquesPort>())
            {
                if (formule > 0)
                {
                    return client.Channel.HasModeleLignes(affaireId, new SectionISDto { NumeroRisque = risque, NumeroOption = option, NumeroFormule = formule, Type = TypeSection.Garanties });
                }
                else if (objet > 0)
                {
                    return client.Channel.HasModeleLignes(affaireId, new SectionISDto { NumeroRisque = risque, NumeroObjet = objet, Type = TypeSection.Objets });
                }
                //return client.Channel.HasModeleLignes(affaireId, new SectionISDto { NumeroRisque = risque, Type = TypeSection.Risques });
                else if (risque > 0)
                {
                    return client.Channel.HasModeleLignes(affaireId, new SectionISDto { NumeroRisque = risque, Type = TypeSection.Risques });
                }
                // Reactiver les IS entête
                else
                    return client.Channel.HasModeleLignes(affaireId, new SectionISDto { Type = TypeSection.Entete });
            }
        }

        protected override bool GetIsReadOnly(string guid, string currentFolder, string numAvenant = "0", bool isPopup = false, string modeAvenant = "")
        {
            bool rdo = base.GetIsReadOnly(guid, currentFolder, numAvenant, isPopup, modeAvenant);
            var folder = new Folder(currentFolder.Split('_'));
            int avn = int.TryParse(numAvenant, out int a) ? a : default;
            if (avn == 0 || folder.Type == AlbConstantesMetiers.TYPE_OFFRE || rdo)
            {
                return rdo;
            }
            folder.NumeroAvenant = avn;
            return GetInfoSpeReadonly(folder);
        }

        protected virtual bool GetInfoSpeReadonly(Folder folder)
        {
            return true;
        }

        private void InitISVALFromOldHtml(AffaireId affaire, SectionISDto section)
        {
            var initializer = new InitializerISVAL(affaire, section);
            if (!initializer.IsValid)
            {
                if (initializer.Html.IsEmptyOrNull())
                {
                    LogOldISState(affaire, section, $"{section.Branche}-{section.Type}-NO_IS");
                    return;
                }
            }
            var infos = initializer.BuildInfos();
            if (initializer.GenerationError is null && initializer.ParseHtmlError is null)
            {
                Task.Run(() =>
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IInfosSpecifiquesPort>())
                    {
                        try
                        {
                            client.Channel.SaveIS(infos);
                            client.Channel.TraceOldISTransfert(affaire, section, $"{section.Branche}-{section.Type}-OK");
                        }
                        catch (Exception ex)
                        {
                            var json = HandleJsonErrorAttribute.BuildJObjectError(ex);
                            client.Channel.TraceOldISTransfert(affaire, section, $"{section.Branche}-{section.Type}-ERROR({json.ToString()})");
                        }
                    }
                });
            }
            else
            {
                throw initializer.GenerationError ?? initializer.ParseHtmlError;
            }
        }
    }

    internal class InitializerISVAL
    {
        const string separatorParam = "#**#";
        const string separatorId = "_";
        const string prefixInput = "map" + separatorId;
        const string suffixUnite = separatorId + "unt";
        readonly AffaireId affaireId;
        readonly SectionISDto section;
        readonly static Regex suffixIdRegex = new Regex("^(unt|d2|h1|h2)$", RegexOptions.Compiled | RegexOptions.Singleline);
        List<string> paramList;
        public InitializerISVAL(AffaireId affaire, SectionISDto section)
        {
            try
            {
                this.affaireId = affaire;
                this.section = section;
                BuildParams();
                Html = GetHtml();
            }
            catch (Exception e)
            {
                this.paramList = null;
                Html = null;
                GenerationError = e;
            }
        }
        public string Html { get; }
        public Exception GenerationError { get; }
        public Exception ParseHtmlError { get; private set; }
        public bool IsValid => (this.paramList?.Any() ?? false) && Html.ContainsChars();

        public InfosSpecifiquesDto BuildInfos()
        {
            if (!IsValid)
            {
                return null;
            }
            ParseHtmlError = null;
            try
            {
                var infosDto = new InfosSpecifiquesDto
                {
                    Modele = new ModeleISDto { CodeBranche = this.section.Branche, Section = this.section.Type }
                };
                var hdoc = new HtmlDocument();
                hdoc.LoadHtml(GetPageHtml());
                infosDto.Infos = GetListInfos(hdoc);
                return infosDto;
            }
            catch (Exception e)
            {
                ParseHtmlError = e;
            }
            return null;
        }

        void BuildParams()
        {
            this.paramList = new[] { this.affaireId.TypeAffaire.AsCode(), this.affaireId.CodeAffaire, this.affaireId.NumeroAliment.ToString() }.ToList();
            if (this.section.Type == TypeSection.Garanties)
            {
                this.paramList.Add(this.section.NumeroFormule.ToString());
                this.paramList.Add(this.section.NumeroOption.ToString());
            }
            else if (this.section.Type == TypeSection.Objets)
            {
                this.paramList.Add(this.section.NumeroRisque.ToString());
                this.paramList.Add(this.section.NumeroObjet.ToString());
            }
             // Reactiver les IS entête
            else if (this.section.Type == TypeSection.Risques)
            {
                this.paramList.Add(this.section.Branche.ToString());
            }

            else
            {
                this.paramList.Add(this.section.NumeroRisque.ToString());
            }
        }

        string GetHtml()
        {
            var sectionName = this.section.Type.ToString();
            return DbInteraction.BuildHtml(
                this.affaireId.IsHisto ? ModeConsultation.Historique.AsCode() : ModeConsultation.Standard.AsCode(),
                this.section.NumeroObjet.ToString(),
                this.section.NumeroRisque.ToString(),
                this.section.NumeroFormule.ToString(),
                this.section.NumeroOption.ToString(),
                sectionName.Substring(0, sectionName.Length - 1),
                this.affaireId.CodeAffaire,
                this.affaireId.NumeroAliment.ToString(),
                this.affaireId.TypeAffaire.AsCode(),
                this.section.Branche,
                this.section.Type.ToString(),
                separatorParam,
                string.Join(separatorParam, this.paramList));
        }

        string GetPageHtml()
        {
            if (!IsValid)
            {
                return string.Empty;
            }
            return $"<html><head></head><body>{Html.Replace(Environment.NewLine, " ")}</body></html>";
        }

        IEnumerable<InformationSpecifiqueDto> GetListInfos(HtmlDocument hdoc)
        {
            var allInputs = hdoc.DocumentNode
                .Descendants()
                .Where(x => x.Id.StartsWith(prefixInput) && x.Attributes[ReadOnlyRewiter.Attr.disabled.ToString()] is null);
            var infoList = new List<InformationSpecifiqueDto>();
            allInputs.Where(x => !suffixIdRegex.IsMatch(x.Id))
                .ToList()
                .ForEach(node =>
                {
                    var array = node.Id.Split(new[] { separatorId }, StringSplitOptions.None).ToList();
                    // remove prefix
                    array.RemoveAt(0);
                    string selectValue = null;
                    bool isSelect = node.Name == "select";
                    if (isSelect)
                    {
                        selectValue = node.Descendants("option").FirstOrDefault(x => x.Attributes[ReadOnlyRewiter.Attr.selected.ToString()] != null)?
                            .GetAttributeValue(ReadOnlyRewiter.Attr.value.ToString(), string.Empty) ?? string.Empty;
                    }

                    infoList.Add(new InformationSpecifiqueDto
                    {
                        AffaireId = this.affaireId,
                        Cle = string.Join(separatorId, array),
                        NumeroFormule = this.section.NumeroFormule,
                        NumeroObjet = this.section.NumeroObjet,
                        NumeroOption = this.section.NumeroOption,
                        NumeroRisque = this.section.NumeroRisque,
                        Valeur = new InfoSpeValeurDto
                        {
                            Val1 = isSelect ? selectValue : node.GetAttributeValue(ReadOnlyRewiter.Attr.type.ToString(), string.Empty) == ReadOnlyRewiter.InputType.checkbox.ToString()
                                ? node.Attributes[ReadOnlyRewiter.Attr.@checked.ToString()] is null ? Booleen.Non.AsCode() : Booleen.Oui.AsCode()
                                : (node.GetAttributeValue(ReadOnlyRewiter.Attr.value.ToString(), string.Empty))
                        }
                    });
                });

            infoList.ForEach(i =>
            {
                var inputUnite = allInputs.FirstOrDefault(x => x.Id == $"{prefixInput}{i.Cle}{suffixUnite}");
                var inputDateMax = allInputs.FirstOrDefault(x => x.Id == $"{prefixInput}{i.Cle}_d2");
                var inputHeureMin = allInputs.FirstOrDefault(x => x.Id == $"{prefixInput}{i.Cle}_h1");
                if (inputUnite != null)
                {
                    i.Valeur.Unite = inputUnite.Name == "select" ? (inputUnite.Descendants("option")
                        .FirstOrDefault(x => x.Attributes[ReadOnlyRewiter.Attr.selected.ToString()] != null)?
                        .GetAttributeValue(ReadOnlyRewiter.Attr.value.ToString(), string.Empty) ?? string.Empty)
                        : inputUnite.GetAttributeValue(ReadOnlyRewiter.Attr.value.ToString(), string.Empty);
                }
                else if (inputDateMax != null)
                {
                    if (inputHeureMin != null)
                    {
                        var inputHeureMax = allInputs.FirstOrDefault(x => x.Id == $"{prefixInput}{i.Cle}_h2");
                        i.Valeur.Val1 = i.Valeur.Val1.IsEmptyOrNull()
                            ? string.Empty
                            : (i.Valeur.Val1 + inputHeureMin.GetAttributeValue(ReadOnlyRewiter.Attr.value.ToString(), DateTime.Today.ToShortTimeString()));
                        i.Valeur.Val2 = inputDateMax.GetAttributeValue(ReadOnlyRewiter.Attr.value.ToString(), string.Empty);
                        i.Valeur.Val2 = i.Valeur.Val2.IsEmptyOrNull()
                            ? string.Empty
                            : (i.Valeur.Val2 + inputHeureMax.GetAttributeValue(ReadOnlyRewiter.Attr.value.ToString(), DateTime.Today.ToShortTimeString()));
                    }
                    else
                    {
                        i.Valeur.Val2 = inputDateMax.GetAttributeValue(ReadOnlyRewiter.Attr.value.ToString(), string.Empty);
                    }
                }
            });

            return infoList;
        }
    }
}