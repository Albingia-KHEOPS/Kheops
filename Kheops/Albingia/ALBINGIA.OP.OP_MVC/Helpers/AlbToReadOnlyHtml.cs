using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Xml.Linq;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Extensions;
using System.Xml;
using System.Web.UI;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Albingia.Common;
using System.Diagnostics;
using ALBINGIA.OP.OP_MVC.Common;

namespace ALBINGIA.OP.OP_MVC.Helpers
{

    public abstract class AlbToReadOnlyHtml<T> : WebViewPage<T> {

        public AlbToReadOnlyHtml() {
        }
        public override void ExecutePageHierarchy() {
            Response.BufferOutput = true;
            base.ExecutePageHierarchy();
        }

        public override void Write(HelperResult result) {
            string output = result.ToHtmlString();
            var controller = ViewContext.Controller as IMetaModelsController;
            string fullHtml = ReadOnlyRewiter.FormatHtmlToReadonly(output, controller);
            var hr = new HelperResult(w => w.Write(fullHtml));
            base.Write(hr);
        }
    }
    internal class ReadOnlyRewiter {
        public enum TagName
        {
            input,
            select,
            textarea
        }

        public enum KnownClass
        {
            requiredField,
            datepicker
        }

        public enum Attr {
            @class,
            disabled,
            @readonly,
            selected,
            @checked,
            type,
            name,
            value,
            albhorsavn,
            data_avn_datalock
        }

        public enum InputType
        {
            text,
            checkbox,
            radio,
            button,
            hidden
        }

        private static readonly Regex datepickerClassContains = new Regex(@"\bclass\s*=\s*""\s*(?<g0>(?:[\w-]+\s+)*)datepicker(?<g1>(?:\s+[\w-]+)*)\s*""", RegexOptions.Compiled);
        private static readonly Regex nonReadonlyClassContains = new Regex(@"(?<=\bclass\s*=\s*""\s*(?:[\w-]+\s+)*?)(datepicker|requiredField)(?=(?:\s+[\w-]+)*\s*"")", RegexOptions.Compiled);
        private string _guid;
        private string _currentFolder;

        private static string GetValueInput(string outPut, string id)
        {
            return outPut.Split(new[] { id }, StringSplitOptions.None)[2].Trim();
        }

        internal static string FormatHtmlToReadonly(string html, IMetaModelsController controller) {
            return FormatHtmlToReadonly(html, controller.IsReadonly, controller.IsModifHorsAvenant, controller.IsAvnDisabled);
        }

        internal static string FormatHtmlToReadonly(string html, bool isReadOnly, bool isModifHorsAvenant, bool? isAvnDisabled = null)
        {
            if (!isReadOnly && !isModifHorsAvenant && !isAvnDisabled.HasValue)
            {
                return html;
            }

            var hdoc = new HtmlDocument();
            hdoc.LoadHtml(html);
            var inputs = hdoc.DocumentNode.Descendants(TagName.input.ToString())
                .Where(i => i.Attributes[Attr.type.ToString()]?.Value != InputType.hidden.ToString())
                .Concat(hdoc.DocumentNode.Descendants(TagName.textarea.ToString()));
            var selects = hdoc.DocumentNode.Descendants(TagName.select.ToString());

            if (isModifHorsAvenant && !isReadOnly)
            {
                inputs = inputs.Where(x => !x.ChildAttributes(Attr.albhorsavn.ToString()).Any()).ToList();
                selects = selects.Where(x => !x.ChildAttributes(Attr.albhorsavn.ToString()).Any()).ToList();
            }
            else if (isAvnDisabled.HasValue) {
                if (isAvnDisabled.Value) {
                    inputs = inputs.Where(x => !x.ChildAttributes(Attr.data_avn_datalock.ToString().Replace('_', '-')).Any()).ToList();
                    selects = selects.Where(x => !x.ChildAttributes(Attr.data_avn_datalock.ToString().Replace('_', '-')).Any()).ToList();
                }
                else {
                    inputs = inputs.Where(x => x.ChildAttributes(Attr.data_avn_datalock.ToString().Replace('_', '-')).Any()).ToList();
                    selects = selects.Where(x => x.ChildAttributes(Attr.data_avn_datalock.ToString().Replace('_', '-')).Any()).ToList();
                }
            }

            inputs.ToList().ForEach(x =>
            {
                string name = x.Attributes[Attr.name.ToString()]?.Value ?? "";
                bool addReadonly = false;
                switch (x.Attributes[Attr.type.ToString()]?.Value?.ToLower())
                {
                    case null:
                    case "":
                    case "text":
                        addReadonly = true;
                        break;
                }

                if (addReadonly && x.Attributes[Attr.@readonly.ToString()] == null)
                {
                    x.Attributes.Add(Attr.@readonly.ToString(), string.Empty);
                }
                if (x.Attributes[Attr.disabled.ToString()] == null)
                {
                    x.Attributes.Add(Attr.disabled.ToString(), string.Empty);
                }
            });

            selects.Where(x => x.Attributes[Attr.disabled.ToString()] == null)
                .ToList()
                .ForEach(x => x.Attributes.Add(Attr.disabled.ToString(), string.Empty));

            if (!isAvnDisabled.HasValue || isAvnDisabled.Value) {
                hdoc.DocumentNode.Descendants()
                  .Where(x => x.ChildAttributes(Attr.@class.ToString()).Any() && (!isModifHorsAvenant || !x.ChildAttributes(Attr.albhorsavn.ToString()).Any()))
                  .ToList()
                  .ForEach(x => {
                      var classes = x.Attributes[Attr.@class.ToString()].Value.Split(' ').ToList();
                      if (classes.RemoveAll(c => c.IsIn(KnownClass.requiredField.ToString(), KnownClass.datepicker.ToString())) > 0) {
                          if (classes.Any()) {
                              x.Attributes[Attr.@class.ToString()].Value = string.Join(" ", classes);
                          }
                          else {
                              x.Attributes[Attr.@class.ToString()].Remove();
                          }
                      }
                  });
            }

            return hdoc.DocumentNode.OuterHtml;
        }


        private void GetParamReadOnly(string output)
        {
            if (output.Contains("tabGuid"))
            {
                _guid = GetValueInput(output, "tabGuid");
            }
            if (output.Contains("idOffreVersionType"))
            {
                _currentFolder = GetValueInput(output, "idOffreVersionType");
            }
        }

        public static string ToModifHorsAvenantHtml(string outputHtml)
        {
            var tags = AllIndexesOf(outputHtml, "albhorsavn");

            foreach (var tag in tags)
            {
                var newValue = tag.OldValue;
                newValue = newValue.Replace("<input readonly disabled", "<input");
                newValue = newValue.Replace("type=\"checkbox\" disabled", "type=\"checkbox\"");
                newValue = newValue.Replace("<select disabled", "<select");
                newValue = newValue.Replace("dummy-datepicker", "datepicker");
                newValue = newValue.Replace("<textarea readonly", "<textarea");
                newValue = newValue.Replace("dummy-requiredField", "requiredField");
                tag.NewValue = newValue;
                outputHtml = outputHtml.Replace(tag.OldValue, tag.NewValue);
            }

            return outputHtml;
        }

        public static IList<Tag> AllIndexesOf(string str, string value)
        {
            var result = new List<Tag>();

            if (string.IsNullOrEmpty(value))
                return result;

            var index = 0;
            while (true)
            {
                index = str.IndexOf(value, index, StringComparison.InvariantCulture);
                if (index == -1) break;

                var endIndex = str.IndexOf(">", index, StringComparison.InvariantCulture);
                endIndex = endIndex == -1 ? str.Length - 1 : endIndex;
                var startIndex = IndexOfRtl(str, '<', index);
                startIndex = startIndex == -1 ? 0 : startIndex;

                result.Add(new Tag
                {
                    OldValue = str.Substring(startIndex, endIndex - startIndex + 1)
                });
                index += value.Length;
            }

            return result;
        }

        public static int IndexOfRtl(string str, char c, int startIndex)
        {
            int index = -1;

            for (int i = startIndex; i > 0; i--)
            {
                if (str[i].Equals(c))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public class Tag
        {
            public string OldValue { get; set; }
            public string NewValue { get; set; }
        }
    }
}


