using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesNavigationArbre {
    public class LienArbre {
        const string ParamArbre = "albParamArbre";
        public LienArbre(ModeleNavigationArbre arbre, bool isParent = false) {
            Parent = arbre;
            TargetData = new Dictionary<string, string>();
            ParamsArbre = new Dictionary<ArbreParam, string>();
            IsParent = isParent;
        }
        public ModeleNavigationArbre Parent { get; }
        public bool IsParent { get; }
        public bool IsVisible { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public bool IsDisabled { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string HtmlId { get; set; }
        public LienArbre ParentLink { get; set; }
        public ContextStepName Target { get; set; }
        public IDictionary<ArbreParam, string> ParamsArbre { get; }
        public IDictionary<string, string> TargetData { get; }
        public Func<bool> FilterCurrentStep { get; set; } = null;
        public int Level {
            get {
                int x = 0;
                LienArbre p = null;
                do {
                    x++;
                    p = p is null ? ParentLink : p.ParentLink;
                }
                while (p != null);
                return x;
            }
        }
        public bool IsNavigationStep { get; set; }
        public MvcHtmlString Render() {
            return new MvcHtmlString(RenderString());
        }

        private string BuildChildrenLinks() {
            if (IsParent && Parent.Links.Any(x => x.Value.ParentLink == this)) {
                var children = Parent.Links.Where(x => x.Value.ParentLink == this);
                return $"<ul class=\"MenuArbreUL\">{string.Join(string.Empty, children.Select(c => c.Value.RenderString()))}</ul>";
            }
            return string.Empty;
        }

        private string RenderString() {
            if (!IsVisible) {
                return string.Empty;
            }
            var classNamesLI = new HashSet<string>() { "MenuArbreLI" };
            var classNamesSPAN = new HashSet<string> { "MenuArbreText" };
            if ((FilterCurrentStep is null || FilterCurrentStep()) && Parent.Etape == Code) {
                classNamesSPAN.Add("bgSelEtape");
            }
            if (IsDisabled) {
                classNamesLI.Add("Disabled");
            }
            else {
                if (IsActive) {
                    classNamesSPAN.Add("navigvisited");
                }
                else if (IsNew) {
                    classNamesSPAN.Add("navig");
                }

                if (IsNavigationStep) {
                    classNamesSPAN.Add("navigate-step");
                }
            }
            if (IsParent) {
                classNamesSPAN.Add("MenuArbreText");
            }
            
            var attributesSPAN = new Dictionary<string, string> {
                { "class", string.Join(" ", classNamesSPAN) }
            };

            if (!IsDisabled) {
                if (Enum.IsDefined(typeof(ContextStepName), Target)) {
                    attributesSPAN.Add("data-target", Target.ToString());
                }
                if (TargetData.Any()) {
                    foreach (var key in TargetData.Keys) {
                        attributesSPAN.Add("data-" + key, TargetData[key]);
                    }
                }
                string url = null;
                if (Url.ContainsChars() || ParamsArbre.TryGetValue(0, out url) && url.ContainsChars()) {
                    Url = Url.ContainsChars() ? Url : url;
                    attributesSPAN.Add("name", "linkNavigationArbre");
                    attributesSPAN.Add(ParamArbre, Url.Replace(" ", string.Empty));
                }
                ParamsArbre.Where(x => x.Value.ContainsChars() && Enum.TryParse(ParamArbre + x.Key, out ArbreParam p))
                    .ToList()
                    .ForEach(x => attributesSPAN.Add(ParamArbre + x.Key, x.Value));
            }

            if (HtmlId.ContainsChars()) {
                attributesSPAN.Add("id", HtmlId);
            }
            if (Description.ContainsChars()) {
                attributesSPAN.Add("title", Description);
            }

            var emptySpan = $"<span{(IsParent ? " class=\"MenuArbreImage\"" : string.Empty)}>&nbsp;&nbsp;&nbsp;</span>";
            var spanElement = $"<span {string.Join(" ", attributesSPAN.Select(x => x.Key + "=\"" + x.Value + "\""))}>{Name}</span>";
            string ulElement = BuildChildrenLinks();
            return $"<li class=\"{string.Join(" ", classNamesLI)}\">{emptySpan}{spanElement}{ulElement}</li>";
        }
    }

    public enum ArbreParam {
        ConsultOnly = 1,
        ActeGestionRegule,
        NewWindow
    }
}