using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Extensions
{
    public static class AddParamValueHelper
    {
        const string Separator = "||";
        const string FirstSeparator = Separator + "|";
        const string ParamPipedStringReplace = "#params#";

        public static IDictionary<string, string> ToParamDictionary(this string addParamString, bool sortKeys = false)
        {
            IDictionary<string, string> dictionary = null;
            if (sortKeys)
            {
                dictionary = new SortedDictionary<string, string>();
            }
            else
            {
                dictionary = new Dictionary<string, string>();
            }
            char pipe = Separator[0];
            if (addParamString != null
                && addParamString.IndexOf(FirstSeparator) == addParamString.LastIndexOf(FirstSeparator)
                && Regex.IsMatch(addParamString, @"([\|]{2}|[^\|]\|[^\|])"))
            {
                bool isPurePiped = addParamString.IndexOf(FirstSeparator) == -1;
                string[] @params = null;
                if (isPurePiped)
                {
                    @params = addParamString.Split(new string[] { Separator }, StringSplitOptions.None);
                }
                else
                {
                    @params = addParamString
                        .Substring(addParamString.IndexOf(FirstSeparator) + FirstSeparator.Length)
                        .Split(new string[] { Separator }, StringSplitOptions.None);
                }
                if (sortKeys)
                {
                    foreach (var value in @params)
                    {
                        var split = value.Split(pipe);
                        if (!dictionary.ContainsKey(split[0]))
                        {
                            string v = split.Length == 1 ? string.Empty : split[1];
                            if (v.EndsWith(AlbParameters.ParamKey))
                            {
                                v = v.Substring(0, v.Length - AlbParameters.ParamKey.Length);
                            }
                            dictionary.Add(split[0], v);
                        }
                    }
                }
                else
                {
                    if (!isPurePiped)
                    {
                        dictionary.Add(string.Empty, addParamString.Substring(0, addParamString.IndexOf(FirstSeparator)) + FirstSeparator + ParamPipedStringReplace);
                    }
                    foreach (var value in @params)
                    {
                        var split = value.Split(pipe);
                        if (!dictionary.ContainsKey(split[0]))
                        {
                            dictionary.Add(split[0], split.Length == 1 ? string.Empty : split[1]);
                        }
                    }
                    if (dictionary.Count > 1)
                    {
                        var keyValue = dictionary.FirstOrDefault(kv => kv.Value.IndexOf(AlbParameters.ParamKey) > 0 && kv.Key.Length > 0);
                        if (keyValue.Key != null && keyValue.Value != null)
                        {
                            int keyIndex = keyValue.Value.IndexOf(AlbParameters.ParamKey);
                            string value = keyValue.Value;
                            dictionary[keyValue.Key] = keyValue.Value.Substring(0, keyIndex);
                            dictionary[string.Empty] += value.Substring(keyIndex);
                        }
                    }
                }
            }
            return dictionary;
        }

        public static bool IncludeValue<T>(this IDictionary<string, string> valuePairs, AlbParameterName key, T value, bool overwriteIfExists = false) where T : struct
        {
            return IncludeValue(valuePairs, key, value.ToString(), overwriteIfExists);
        }
        public static bool IncludeValue(this IDictionary<string, string> valuePairs, AlbParameterName key, string value, bool overwriteIfExists = false)
        {
            return IncludeValue(valuePairs, key.ToString(), value, overwriteIfExists);
        }
        public static bool IncludeValue(this IDictionary<string, string> valuePairs, string key, string value, bool overwriteIfExists = false)
        {
            if (valuePairs != null && key != null)
            {
                if (!valuePairs.ContainsKey(key))
                {
                    var lastPair = valuePairs.Last();
                    valuePairs.Remove(lastPair.Key);
                    valuePairs.Add(key, value);
                    valuePairs.Add(lastPair);
                }
                else if (overwriteIfExists)
                {
                    valuePairs[key] = value;
                }
            }

            return false;
        }
        public static bool InsetSecondToLast(this IDictionary<string, string> valuePairs, string key, string value, bool overwriteIfExists = false)
        {
            if (valuePairs != null && key != null)
            {
                if (!valuePairs.ContainsKey(key))
                {
                    var lastPair = valuePairs.Last();
                    valuePairs.Remove(lastPair.Key);
                    valuePairs.Add(key, value);
                    valuePairs.Add(lastPair);
                }
                else if (overwriteIfExists)
                {
                    valuePairs[key] = value;
                }
            }

            return false;
        }


        public static string RebuildAddParamString(this IDictionary<string, string> valuePairs)
        {
            if (valuePairs != null)
            {
                string line = string.Join(Separator, valuePairs.Where(kv => kv.Key.Length > 0).Select(kv => kv.Key + (kv.Value.Length > 0 ? Separator[0] + kv.Value : string.Empty)));
                if (valuePairs.TryGetValue(string.Empty, out string value))
                {
                    line = value.Replace(ParamPipedStringReplace, line);
                }

                return line;
            }

            return string.Empty;
        }
    }
}
