using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Albingia.Common {
    public class PageParamContext {
        public const string ParamIdKey = "AddParam";
        public const string TabGuidKey = "tabGuid";
        public const string ParamKey = "addParam";
        public const string ModeNavigKey = "modeNavig";
        const string IsLockedKey = "FORCEREADONLY";
        const string IgnoreReadonlyKey = "IGNOREREADONLY";
        const string ValuesSeparator = "||";

        public PageParamContext(string type, string value, string tabGuid, string modeNavig) {
            Type = type;
            Value = value;
            TabGuid = tabGuid?.StartsWith(TabGuidKey) == true ? tabGuid.Substring(TabGuidKey.Length, tabGuid.Length - (2 * TabGuidKey.Length)) : (tabGuid ?? string.Empty);
            if (modeNavig.ContainsChars()) {
                if (modeNavig.Length == 1) {
                    ModeNavigation = modeNavig.ParseCode<ModeConsultation>();
                }
                else if (Enum.TryParse(modeNavig, out ModeConsultation mode)) {
                    ModeNavigation = mode;
                }
            }
            else {
                ModeNavigation = ModeConsultation.Standard;
            }
        }

        public PageParamContext() { }

        public string Type { get; set; }
        public string Value { get; set; }
        public string TabGuid { get; set; }
        public ModeConsultation ModeNavigation { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsLocked { get; set; }

        public static (string prefix, PageParamContext context) BuildFromString(string paramsLine) {
            if (paramsLine.ContainsChars()) {
                var matches = Regex.Matches(paramsLine, $@"^(.*){TabGuidKey}(\w+){TabGuidKey}{ParamKey}(.*)\|\|\|(.*){ParamKey}({ModeNavigKey}([SHsh]?){ModeNavigKey})?(ConsultOnly)?(newWindow)?$");
                if (matches.Count == 1) {
                    string addParamValue = string.Join(
                        ValuesSeparator,
                        matches[0].Groups[4].Value
                            .Split(new[] { ValuesSeparator }, StringSplitOptions.None)
                            .Where(x => {
                                string key = x.Split(ValuesSeparator[0]).First();
                                return key != IsLockedKey && key != IgnoreReadonlyKey;
                            }));
                    return (matches[0].Groups[1].Value, new PageParamContext(matches[0].Groups[3].Value, addParamValue, matches[0].Groups[2].Value, matches[0].Groups[6].Value));
                }
            }

            return (null, new PageParamContext());
        }

        public string BuildFullString(string prefix = "") {
            var output = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(prefix)) {
                output.Append(prefix);
            }

            output.Append($"{TabGuidKey}{TabGuid}{TabGuidKey}");
            if (!string.IsNullOrWhiteSpace(Type)) {
                output.Append($"{ParamKey}{Type}|||{Value}");
                if (ModeNavigation != ModeConsultation.Historique) {
                    if (!IsLocked && IsReadonly) {
                        output.Append($"||{IgnoreReadonlyKey}|1");
                    }
                    else if (IsLocked) {
                        output.Append($"||{IsLockedKey}|1");
                    }
                }
                output.Append($"{ParamKey}");
            }

            output.Append($"{ModeNavigKey}{ModeNavigation.AsCode()}{ModeNavigKey}");
            return output.ToString();
        }
    }
}