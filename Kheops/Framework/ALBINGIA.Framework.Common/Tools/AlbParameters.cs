using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ALBINGIA.Framework.Common.Tools
{
    public class AlbParameters : INotifyPropertyChanged
    {
        public const string GuidPattern = "[a-zA-Z0-9]{32}";
        public const string ConsultOnlyKey = "ConsultOnly";
        public const string ConsultOnlyAndEditKey = "ConsultOnlyAndEdit";
        public const string ModeNavigKey = "modeNavig";
        internal const string IdSeparator = "_";
        internal const char Separator = '|';
        internal const string FolderIdKey = "folderId";
        internal const string TabGuidKey = "tabGuid";
        internal const string ParamKey = "addParam";
        internal const string NewWindowKey = "newWindow";
        internal const string RepriseKey = "reprise";
        internal const string AccesModeKey = "accessMode";
        internal const string ReloadSearchParamKey = "loadParam";
        internal static readonly string Splitter = new string(Separator, 2);

        public static readonly char[] SeparatorsForPipedParamValues = new[] { '_', ',', ';', '-', '.', '/', '&', '+', '#', '!' };
        public static readonly Regex RegexPipes = new Regex(
            @"^(?:\|\|)?(?:([A-Z]+)\|([\w\s]+)(?:\|\||(?=$)))+$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        protected static readonly Regex regexFolderIdOnly = new Regex($@"^[A-Z0-9]{{1,9}}{IdSeparator}\d{{1,2}}{IdSeparator}[PO](({IdSeparator}[A-Za-z0-9]+)+)?$", RegexOptions.Compiled | RegexOptions.Singleline);
        protected static readonly Regex regexFolderIdLocked = new Regex($@"^[A-Z0-9]{{1,9}}{IdSeparator}\d{{1,2}}{IdSeparator}[PO]LUSR[A-Za-z0-9]*$", RegexOptions.Compiled | RegexOptions.Singleline);
        protected static readonly Regex regexParamString = new Regex(
            $@"^(.*?){TabGuidKey}((?:{GuidPattern})?){TabGuidKey}({ParamKey}(.*)\|\|\|(.*){ParamKey})?({ModeNavigKey}([SHsh]?){ModeNavigKey})?({AccesModeKey}recherche{AccesModeKey})?({ConsultOnlyAndEditKey})?({ConsultOnlyKey})?({NewWindowKey})?({RepriseKey}([01]){RepriseKey})?$",
            RegexOptions.Compiled | RegexOptions.Singleline);
        protected static readonly Regex simpleRegexParamString = new Regex(
            $@"^(?<folder>.*?){TabGuidKey}(?<guid>{GuidPattern})?{TabGuidKey}(?<param>{ParamKey}(?<type>.*)\|\|\|(?<paramValues>.*){ParamKey})?(?<modeNavigGroup>{ModeNavigKey}(?<modeNavig>[SHsh]?){ModeNavigKey})?(?<accessMode>{AccesModeKey}recherche{AccesModeKey})?(?<consult>{ConsultOnlyKey})?(?<lusr>LUSR(?<user>\w*))?$",
            RegexOptions.Compiled | RegexOptions.Singleline);
        protected static readonly Regex regexNoTabGuid = new Regex(@"^([A-Z0-9]{1,9}_\d{1,2}_[PO](_\d+)?(_\d+)?(_\d+)?(_C)?)" + $@"({ParamKey}(.*)\|\|\|(.*){ParamKey})?({ModeNavigKey}([SHsh]?){ModeNavigKey})?$", RegexOptions.Compiled | RegexOptions.Singleline);
        protected static readonly Regex regexCopyOffer = new Regex(@"^[A-Z0-9]{1,9}_\d{1,2}_C_[A-Z0-9]{1,9}_\d{1,2}_[PO]$");
        protected static readonly Regex regexNewOffer = new Regex($@"^([A-Z]{{2}}){TabGuidKey}({GuidPattern})?{TabGuidKey}({ModeNavigKey}([SHsh]?){ModeNavigKey})?$");

        protected static readonly Regex regexTemplate = new Regex($@"^([0-9]+)albTemplate({IdSeparator}\d{{1,2}}{IdSeparator}[PO])$", RegexOptions.Compiled | RegexOptions.Singleline);

        protected static readonly Regex regexParamStringWithOrigin = new Regex(
            $@"^(.+?)_¤(\w+)¤Index¤(\w+£\d{{1,2}}£[PO](£\d+)?(£\d+)?(£C)?)(?:_(\w+?))?({TabGuidKey}({GuidPattern})?{TabGuidKey})?({ParamKey}(.*)\|\|\|(.*){ParamKey})?({ModeNavigKey}([SHsh]?){ModeNavigKey})?$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        protected static readonly Regex regexForKey = new Regex(@"^[A-Za-z_]\w+$", RegexOptions.Compiled | RegexOptions.Singleline);

        protected static IReadOnlyDictionary<string, AlbParameterName> PropertyMap = new Dictionary<string, AlbParameterName> {
            { string.Empty, AlbParameterName._hidden },
            { nameof(TypeAvenant), AlbParameterName.AVNTYPE },
            { nameof(TypeAvenantResiliation), AlbParameterName.AVNTYPERESIL },
            { nameof(ActeDeGestion), AlbParameterName.ACTEGESTIONREGULE },
            { nameof(NumeroAvenant), AlbParameterName.AVNID },
            { nameof(NumeroAvenantExterne), AlbParameterName.AVNIDEXTERNE },
            { nameof(IdAttestation), AlbParameterName.ATTESTID },
            { nameof(ModeRegularisation), AlbParameterName.REGULMOD },
            { nameof(TypeRegularisation), AlbParameterName.REGULTYP },
            { nameof(NiveauRegularisation), AlbParameterName.REGULNIV },
            { nameof(IsHistoRegularisation), AlbParameterName.REGULAVN },
            { nameof(IdRegularisation), AlbParameterName.REGULEID },
            { nameof(IdLot), AlbParameterName.LOTID },
            { nameof(IdGarantie), AlbParameterName.AVNIDEXTERNE },
            { nameof(IdGarantiePeriode), AlbParameterName.ATTESTID },
            { nameof(IsValidation), AlbParameterName.VALIDATION },
            { nameof(ForceReadonly), AlbParameterName.FORCEREADONLY },
            { nameof(IgnoreReadonly), AlbParameterName.IGNOREREADONLY },
            { nameof(ModeNavig), AlbParameterName.MODENAVIG },
            { nameof(AccessMode), AlbParameterName.ACCESSMODEENG },
            { nameof(IdRisque), AlbParameterName.RSQID },
            { nameof(RefreshUserUpdateAvn), AlbParameterName.AVNREFRESHUSERUPDATE },
            { nameof(OriginPage), AlbParameterName.ORIGINPAGE },
            { nameof(OriginId), AlbParameterName.ORIGINID },
            { nameof(ModeConsultationAvenant), AlbParameterName.AVNMODE },
            { nameof(LockingUser), AlbParameterName.LUSR },
            { nameof(BrancheOffre), AlbParameterName.BRANCHEOFFRE },
            { nameof(TemplateId), AlbParameterName.TEMPLATEID },
            { nameof(ContextClausier), AlbParameterName.CONTEXTCLAUSIER },
            { nameof(Provenance), AlbParameterName.PROVENANCE }
        };

        protected static IEnumerable<AlbParameterName> AllowedParamsToRebuild = new HashSet<AlbParameterName> {
            AlbParameterName.AVNTYPE,
            AlbParameterName.AVNTYPERESIL,
            AlbParameterName.ACTEGESTIONREGULE,
            AlbParameterName.AVNID,
            AlbParameterName.AVNIDEXTERNE,
            AlbParameterName.ATTESTID,
            AlbParameterName.VALIDATION,
            AlbParameterName.RSQID,
            AlbParameterName.AVNMODE,
            AlbParameterName.REGULAVN,
            AlbParameterName.REGULMOD,
            AlbParameterName.REGULNIV,
            AlbParameterName.REGULTYP,
            AlbParameterName.REGULEID,
            AlbParameterName.LOTID,
            AlbParameterName.REGULGARID,
            AlbParameterName.GARID,
            AlbParameterName.COPY_ADR_FROM_HEADER
        };

        protected readonly ICollection<(string key, string value)> rawValues;
        protected readonly IDictionary<string, List<int>> keyPositions;
        protected readonly SortedDictionary<int, (AlbParameterName key, object value)> sortedValues;
        protected readonly IDictionary<AlbParameterName, object> values;
        protected readonly IDictionary<string, object> extraValues;
        private PropertyChangedEventHandler propertyChanged;
        private Folder folder;
        private string type;

        internal AlbParameters(string initialString)
        {
            InitialString = initialString;
            sortedValues = new SortedDictionary<int, (AlbParameterName key, object value)>();
            values = new Dictionary<AlbParameterName, object>();
            extraValues = new Dictionary<string, object>();
            rawValues = new List<(string key, string value)>();
            keyPositions = new Dictionary<string, List<int>>();
            folder = null;
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        public string InitialString { get; }

        /// <summary>
        /// Gets a value indicating whether each param key has to be valid within <see cref="AlbParameterName"/> enum
        /// </summary>
        public bool StrictMode { get; protected set; } = true;

        /// <summary>
        /// Gets a value indicating whether NULL string can be assigned to any parameter
        /// </summary>
        public bool AllowNullString { get; protected set; } = false;

        public string Type
        {
            get => type;
            set => type = value;
        }

        public bool IsPiped => Type == "piped";

        public object this[AlbParameterName name]
        {
            get
            {
                if (values.TryGetValue(name, out object value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                bool hasChanged = true;
                object val = value;
                if (!AllowNullString && val == null)
                {
                    val = string.Empty;
                }
                if (!values.TryGetValue(name, out object o))
                {
                    values.Add(name, null);
                }
                hasChanged = !Equals(values[name], val);
                values[name] = val;
                if (hasChanged && propertyChanged != null)
                {
                    propertyChanged(this, new PropertyChangedEventArgs(PropertyMap.First(kv => kv.Value == name).Key));
                }
            }
        }

        public string this[string key]
        {
            get
            {
                string value = null;
                if (Enum.TryParse(key, out AlbParameterName name))
                {
                    if (this[name] == null)
                    {
                        value = null;
                    }
                    else if (this[name] is string s)
                    {
                        value = s;
                    }
                    else
                    {
                        Type t = this[name].GetType();
                        if (Nullable.GetUnderlyingType(t) != null)
                        {
                            if (Convert.ToBoolean(t.GetProperty("HasValue").GetValue(this[name])))
                            {
                                value = t.GetProperty("Value").GetValue(this[name]).ToString();
                            }
                        }
                        else
                        {
                            value = this[name].ToString();
                        }
                    }
                }
                else if (!StrictMode || IsKeyAllowedInStrictMode(key))
                {
                    if (keyPositions.TryGetValue(key, out List<int> positions))
                    {
                        value = sortedValues[positions.Max()].value as string;
                    }
                }
                return value;
            }
            set
            {
                var @string = AllowNullString ? value : (value ?? string.Empty);
                if (Enum.TryParse(key, out AlbParameterName name))
                {
                    this[name] = @string;
                }
                else if (!StrictMode || IsKeyAllowedInStrictMode(key))
                {
                    if (keyPositions.TryGetValue(key, out List<int> positions))
                    {
                        sortedValues[positions.Max()] = (AlbParameterName._hidden, @string);
                    }
                    else if (regexForKey.IsMatch(key))
                    {
                        rawValues.Add((key, value));
                        sortedValues.Add(sortedValues.Any() ? (sortedValues.Keys.Max() + 1) : 1, (AlbParameterName._hidden, @string));
                        keyPositions.Add(key, new List<int> { sortedValues.Keys.Max() });
                    }
                }
            }
        }

        public Folder Folder
        {
            get
            {
                if (IsPiped)
                {
                    folder = new Folder(
                        this[PipedParameter.IPB],
                        this[PipedParameter.ALX].ParseInt().Value,
                        (this[PipedParameter.TYP]?[0]).GetValueOrDefault('P'))                    {
                        NumeroAvenant = this[PipedParameter.AVN].ParseInt().Value
                    };
                }
                else if (this[FolderIdKey] != null && folder == null)
                {
                    folder = new Folder(this[FolderIdKey].Split('_'));
                }
                return folder;
            }
        }

        public string FolderId
        {
            get
            {
                if (IsPiped)
                {
                    return Folder.FullIdentifier;
                }
                else
                {
                    return this[FolderIdKey] ?? string.Empty;
                }
            }
        }

        public string TypeAvenant
        {
            get { return GetString(PropertyMap[nameof(TypeAvenant)]); }
            set { this[PropertyMap[nameof(TypeAvenant)]] = value; }
        }

        public string TypeAvenantResiliation
        {
            get { return GetString(PropertyMap[nameof(TypeAvenantResiliation)]); }
            set { this[PropertyMap[nameof(TypeAvenantResiliation)]] = value; }
        }

        public string ActeDeGestion
        {
            get { return GetString(PropertyMap[nameof(ActeDeGestion)]); }
            set { this[PropertyMap[nameof(ActeDeGestion)]] = value; }
        }

        public int? NumeroAvenant
        {
            get { return GetInt32(PropertyMap[nameof(NumeroAvenant)]); }
            set { this[PropertyMap[nameof(NumeroAvenant)]] = value.GetValueOrDefault(); }
        }

        public int? NumeroAvenantExterne
        {
            get { return GetInt32(PropertyMap[nameof(NumeroAvenantExterne)]); }
            set { this[PropertyMap[nameof(NumeroAvenantExterne)]] = value; }
        }

        public int? IdAttestation
        {
            get { return GetInt32(PropertyMap[nameof(IdAttestation)]); }
            set { this[PropertyMap[nameof(IdAttestation)]] = value; }
        }

        public ModeConsultation ModeNavig
        {
            get
            {
                if (this[PropertyMap[nameof(ModeNavig)]] != null)
                {
                    if (this[PropertyMap[nameof(ModeNavig)]] is string mode)
                    {
                        return mode.ParseCode<ModeConsultation>();
                    }
                }
                return default(ModeConsultation);
            }
            set
            {
                this[PropertyMap[nameof(ModeNavig)]] = value.AsCode();
            }
        }

        public string AccessMode
        {
            get { return GetString(PropertyMap[nameof(AccessMode)]); }
            set { this[PropertyMap[nameof(AccessMode)]] = value; }
        }

        public string OriginPage
        {
            get { return GetString(PropertyMap[nameof(OriginPage)]); }
            set { this[PropertyMap[nameof(OriginPage)]] = value; }
        }

        public string OriginId
        {
            get { return GetString(PropertyMap[nameof(OriginId)]); }
            set { this[PropertyMap[nameof(OriginId)]] = value; }
        }

        public string ModeRegularisation
        {
            get { return GetString(PropertyMap[nameof(ModeRegularisation)]); }
            set { this[PropertyMap[nameof(ModeRegularisation)]] = value; }
        }

        public string TypeRegularisation
        {
            get { return GetString(PropertyMap[nameof(TypeRegularisation)]); }
            set { this[PropertyMap[nameof(TypeRegularisation)]] = value; }
        }

        public string LockingUser
        {
            get { return GetString(PropertyMap[nameof(LockingUser)]); }
            set { this[PropertyMap[nameof(LockingUser)]] = value; }
        }

        public string NiveauRegularisation
        {
            get { return GetString(PropertyMap[nameof(NiveauRegularisation)]); }
            set { this[PropertyMap[nameof(NiveauRegularisation)]] = value; }
        }

        public bool IsHistoRegularisation
        {
            get { return GetBoolean(PropertyMap[nameof(IsHistoRegularisation)]).GetValueOrDefault(); }
            set { this[PropertyMap[nameof(IsHistoRegularisation)]] = value; }
        }

        public long? IdRegularisation
        {
            get { return GetInt64(PropertyMap[nameof(IdRegularisation)]); }
            set { this[PropertyMap[nameof(IdRegularisation)]] = value; }
        }

        public long? IdLot
        {
            get { return GetInt64(PropertyMap[nameof(IdLot)]); }
            set { this[PropertyMap[nameof(IdLot)]] = value; }
        }

        public int? IdRisque
        {
            get { return GetInt32(PropertyMap[nameof(IdRisque)]); }
            set { this[PropertyMap[nameof(IdRisque)]] = value; }
        }

        public long? IdGarantie
        {
            get { return GetInt64(PropertyMap[nameof(IdGarantie)]); }
            set { this[PropertyMap[nameof(IdGarantie)]] = value; }
        }

        public long? IdGarantiePeriode
        {
            get { return GetInt64(PropertyMap[nameof(IdGarantiePeriode)]); }
            set { this[PropertyMap[nameof(IdGarantiePeriode)]] = value; }
        }

        public string ModeConsultationAvenant
        {
            get { return GetString(PropertyMap[nameof(ModeConsultationAvenant)]); }
            set { this[PropertyMap[nameof(ModeConsultationAvenant)]] = value; }
        }

        public bool IgnoreReadonly
        {
            get { return GetBoolean(PropertyMap[nameof(IgnoreReadonly)]).GetValueOrDefault(); }
            set { this[PropertyMap[nameof(IgnoreReadonly)]] = value; }
        }

        public bool ForceReadonly
        {
            get { return GetBoolean(PropertyMap[nameof(ForceReadonly)]).GetValueOrDefault(); }
            set { this[PropertyMap[nameof(ForceReadonly)]] = value; }
        }

        public bool RefreshUserUpdateAvn
        {
            get { return GetBoolean(PropertyMap[nameof(RefreshUserUpdateAvn)]).GetValueOrDefault(); }
            set { this[PropertyMap[nameof(RefreshUserUpdateAvn)]] = value; }
        }

        public bool IsValidation
        {
            get { return GetBoolean(PropertyMap[nameof(IsValidation)]).GetValueOrDefault(); }
            set { this[PropertyMap[nameof(IsValidation)]] = value; }
        }

        public string BrancheOffre
        {
            get { return GetString(PropertyMap[nameof(BrancheOffre)]); }
            set { this[PropertyMap[nameof(BrancheOffre)]] = value; }
        }

        public int? TemplateId
        {
            get { return GetInt32(PropertyMap[nameof(TemplateId)]); }
            set { this[PropertyMap[nameof(TemplateId)]] = value; }
        }

        public string ContextClausier
        {
            get { return GetString(PropertyMap[nameof(ContextClausier)]); }
            set { this[PropertyMap[nameof(ContextClausier)]] = value; }
        }

        public string Provenance
        {
            get { return GetString(PropertyMap[nameof(Provenance)]); }
            set { this[PropertyMap[nameof(Provenance)]] = value; }
        }

        public string this[PipedParameter parameter, int occurence = 1]
        {
            get
            {
                if (!IsPiped || int.TryParse(parameter.ToString(), out int i))
                {
                    return null;
                }
                if (occurence < 1) { occurence = 1; }
                var values = this.rawValues.Where(x => x.key == parameter.ToString());
                if (occurence > values.Count()) { return null; }

                return values.ElementAt(occurence - 1).value;
            }
        }


        /// <summary>
        /// Builds a composite identifier made up of folder data, specific infos, guid, avn parameters and states
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="infos"></param>
        /// <param name="guid"></param>
        /// <param name="avnParams"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        public static string BuildFullId(Folder folder, string[] infos, string guid, string avnParams, string mode, string[] states = null)
        {
            var parts = new List<string>();
            if (folder != null)
            {
                parts.Add(folder.BuildId(IdSeparator));
            }
            if (infos?.Any(s => s != null) == true)
            {
                parts.AddRange(infos.Where(s => s != null).Select(s => IdSeparator + s));
            }
            if (guid != null)
            {
                if (guid.StartsWith(TabGuidKey) && guid.EndsWith(TabGuidKey))
                {
                    parts.Add(guid);
                }
                else
                {
                    parts.AddRange(new[] { TabGuidKey, guid, TabGuidKey });
                }
            }
            if (avnParams.ContainsChars())
            {
                parts.AddRange(new[] { ParamKey, "AVN|||", avnParams, ParamKey });
            }
            if (mode != null)
            {
                if (mode.StartsWith(ModeNavigKey) && mode.EndsWith(ModeNavigKey))
                {
                    parts.Add(mode);
                }
                else
                {
                    parts.AddRange(new[] { ModeNavigKey, mode, ModeNavigKey });
                }
            }
            if (states?.Any(s => s != null) == true)
            {
                parts.AddRange(states.Where(s => s != null));
            }

            return string.Concat(parts);
        }

        /// <summary>
        /// Builds a composite identifier made up of folder data, guid, avn parameters and states
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="guid"></param>
        /// <param name="avnParams"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        public static string BuildStandardId(Folder folder, string guid, string avnParams, string mode, string[] states = null)
        {
            return BuildFullId(folder, null, guid, avnParams, mode, states);
        }

        public static string BuildPipedParams(IDictionary<PipedParameter, IEnumerable<string>> values, bool repeatKeyIfMultipleValues = true)
        {
            var parameters = values?
                .Where(x => !int.TryParse(x.Key.ToString(), out var i) && x.Value.Any(v => v.ContainsChars()))
                .Select(x => new KeyValuePair<PipedParameter, IEnumerable<string>>(x.Key, x.Value.Where(v => v.ContainsChars())))
                .ToDictionary(x => x.Key, x => x.Value);
            if (parameters?.Any() ?? false)
            {
                if (repeatKeyIfMultipleValues || !parameters.Values.Any(v => v.Count() > 1))
                {
                    return string.Join("||", parameters.Select(KV => string.Join("||", KV.Value.Select(v => $"{KV.Key.ToString()}|{v}"))));
                }
                else
                {
                    var allChars = parameters.Values.Where(v => v.Count() > 1).SelectMany(v => v).SelectMany(s => s.ToArray());
                    char sp = SeparatorsForPipedParamValues.First(ch => !allChars.Any(x => x == ch));
                    return string.Join("||", parameters.Select(KV => $"{KV.Key.ToString()}|{string.Join(sp.ToString(), KV.Value)}"));
                }
            }
            return string.Empty;
        }

        //public static string AppendPipedParam(PipedParameter parameter, IEnumerable<string> values, int? position = null) {

        //}

        protected static string FormatParamString(string paramString, out string type)
        {
            type = null;
            if (paramString.IsEmptyOrNull())
            {
                return paramString?.Trim();
            }

            var match = RegexPipes.Match(paramString);
            if (match.Success)
            {
                type = "piped";
                return paramString;
            }

            match = regexTemplate.Match(paramString);
            if (match.Success)
            {

                return GetKeyValue(AlbParameterName.TEMPLATEID.ToString(), match.Groups[1].Value, true)
                    + GetKeyValue(FolderIdKey, match.Groups[2].Value);
            }

            match = regexNewOffer.Match(paramString);
            if (match.Success)
            {
                return GetKeyValue(AlbParameterName.BRANCHEOFFRE.ToString(), match.Groups[1].Value, true)
                    + GetKeyValue(TabGuidKey, match.Groups[2].Value)
                    + GetKeyValue(AlbParameterName.MODENAVIG.ToString(), match.Groups[3].Value.OrDefault(ModeConsultation.Standard.AsCode()));
            }

            match = regexParamStringWithOrigin.Match(paramString);
            if (match.Success)
            {
                string p = match.Groups[12].Value;
                type = match.Groups[11].Value;
                return GetKeyValue(FolderIdKey, match.Groups[1].Value, true)
                    + GetKeyValue(AlbParameterName.ORIGINPAGE.ToString(), match.Groups[2].Value)
                    + GetKeyValue(AlbParameterName.ORIGINID.ToString(), match.Groups[3].Value.Replace('£', '_'))
                    + GetKeyValue(AlbParameterName.CONTEXTCLAUSIER.ToString(), match.Groups[7].Value)
                    + GetKeyValue(TabGuidKey, match.Groups[9].Value)
                    + (p.Length == 0 ? string.Empty : Splitter) + p
                    + GetKeyValue(AlbParameterName.MODENAVIG.ToString(), match.Groups[14].Value.OrDefault(ModeConsultation.Standard.AsCode()));
            }

            match = regexParamString.Match(paramString);
            if (match.Success)
            {
                string p = match.Groups[5].Value;
                type = match.Groups[4].Value;
                return GetKeyValue(FolderIdKey, match.Groups[1].Value, true)
                    + GetKeyValue(TabGuidKey, match.Groups[2].Value)
                    + (p.Length == 0 ? string.Empty : Splitter) + p
                    + GetKeyValue(AlbParameterName.MODENAVIG.ToString(), match.Groups[7].Value.OrDefault(ModeConsultation.Standard.AsCode()))
                    + GetKeyValue(AlbParameterName.ACCESSMODEENG.ToString(), match.Groups[8].Value.ContainsChars() ? "recherche" : string.Empty)
                    + GetKeyValue(ConsultOnlyAndEditKey, (match.Groups[9].Value == ConsultOnlyAndEditKey ? 1 : 0).ToString())
                    + GetKeyValue(ConsultOnlyKey, (match.Groups[10].Value == ConsultOnlyKey ? 1 : 0).ToString())
                    + GetKeyValue(NewWindowKey, (match.Groups[11].Value == NewWindowKey ? 1 : 0).ToString());
            }

            match = simpleRegexParamString.Match(paramString);
            if (match.Success)
            {
                string paramValues = match.Groups["paramValues"].Value;
                type = match.Groups["type"].Value;
                return GetKeyValue(FolderIdKey, match.Groups["folder"].Value, true)
                    + GetKeyValue(TabGuidKey, match.Groups["guid"].Value)
                    + GetKeyValue(AlbParameterName.MODENAVIG.ToString(), match.Groups["modeNavig"].Value.OrDefault(ModeConsultation.Standard.AsCode()))
                    + (paramValues.Length == 0 ? string.Empty : Splitter) + paramValues
                    + GetKeyValue(ConsultOnlyKey, (match.Groups["consult"].Value == ConsultOnlyKey ? 1 : 0).ToString())
                    + GetKeyValue(AlbParameterName.LUSR.ToString(), match.Groups["user"].Value);
            }

            match = regexNoTabGuid.Match(paramString);
            if (match.Success)
            {
                string p = match.Groups[8].Value;
                type = match.Groups[7].Value;
                return GetKeyValue(FolderIdKey, match.Groups[1].Value, true)
                    + (p.Length == 0 ? string.Empty : Splitter) + p
                    + GetKeyValue(AlbParameterName.MODENAVIG.ToString(), match.Groups[9].Value.OrDefault(ModeConsultation.Standard.AsCode()));
            }

            if (regexCopyOffer.IsMatch(paramString))
            {
                return GetKeyValue(FolderIdKey, paramString, true);
            }

            match = regexFolderIdOnly.Match(paramString);
            if (!match.Success)
            {
                match = regexFolderIdLocked.Match(paramString);
            }
            if (match.Success)
            {
                paramString = GetKeyValue(FolderIdKey, match.Groups[0].Value, true);
            }

            return paramString;
        }
        protected static bool IsKeyAllowedInStrictMode(string key)
        {
            return key.IsIn(FolderIdKey, ParamKey, TabGuidKey, ConsultOnlyAndEditKey, ReloadSearchParamKey, ConsultOnlyKey, NewWindowKey, RepriseKey);
        }
        protected static string GetKeyValue(string key, string value, bool isFirstPair = false)
        {
            if (value.IsEmptyOrNull())
            {
                return string.Empty;
            }
            return (isFirstPair ? string.Empty : Splitter) + key + Separator + value;
        }

        public string BuildParamsString(bool valuesOnly = true, bool oldVersion = true)
        {
            if (oldVersion)
            {
                var str = new StringBuilder();
                if (!valuesOnly)
                {
                    str.Append(FolderId);
                    str.Append($"{TabGuidKey}{this[TabGuidKey]}{TabGuidKey}");
                }
                var keysToBuild = values.Keys.Where(k => AllowedParamsToRebuild.Contains(k));
                if (type != null && keysToBuild.Any())
                {
                    bool firstKey = true;
                    if (!valuesOnly)
                    {
                        str.Append($"{ParamKey}{type}|||");
                    }
                    foreach (var key in keysToBuild)
                    {
                        str.Append(GetKeyValue(key.ToString(), this[key.ToString()], firstKey));
                        if (firstKey) { firstKey = false; }
                    }
                    if (!valuesOnly)
                    {
                        str.Append($"{ParamKey}");
                    }
                }
                if (!valuesOnly)
                {
                    str.Append($"{ModeNavigKey}{ModeNavig.AsCode()}{ModeNavigKey}");
                }
                return str.ToString();
            }
            return "";
        }

        protected void SetValues()
        {
            sortedValues.Clear();
            values.Clear();
            keyPositions.Clear();
            if (IsPiped || !rawValues.Any())
            {
                return;
            }
            else
            {
                rawValues
                    .Select((x, i) => new { position = i, element = x })
                    .Where(kv =>
                    {
                        return StrictMode ?
                            Enum.TryParse(kv.element.key, out AlbParameterName name) || IsKeyAllowedInStrictMode(kv.element.key)
                            : regexForKey.IsMatch(kv.element.key);
                    })
                    .ToList()
                    .ForEach(kv =>
                    {
                        var albName = Enum.TryParse(kv.element.key, out AlbParameterName name) ? (name) : (AlbParameterName._hidden);
                        var (key, value) = (albName, kv.element.value);
                        sortedValues.Add(kv.position, (key, value));
                        if (!keyPositions.TryGetValue(kv.element.key, out List<int> positions))
                        {
                            positions = new List<int>();
                            keyPositions.Add(kv.element.key, positions);
                        }
                        positions.Add(kv.position);
                        if (albName == AlbParameterName._hidden)
                        {
                            extraValues.Add(kv.element.key, value);
                        }
                    });

                sortedValues.Values.Where(v => v.key != 0).GroupBy(v => v.key).ToList().ForEach(g =>
                {
                    if (g.Count() == 1 || StrictMode)
                    {
                        values.Add(g.Last().key, g.Last().value);
                    }
                    else
                    {
                        values.Add(g.First().key, g.Select(x => x.value).ToArray());
                    }
                });
            }
        }

        protected int? GetInt32(AlbParameterName name)
        {
            if (this[name] is string s)
            {
                int.TryParse(s, out int num);
                return num;
            }
            else if (this[name] != null && this[name].GetType() == typeof(int))
            {
                return (int)this[name];
            }
            else
            {
                return null;
            }
        }
        protected bool? GetBoolean(AlbParameterName name)
        {
            return GetBoolean(this[name]);
        }
        protected bool? GetBoolean(object value)
        {
            if (value is string s)
            {
                if (!bool.TryParse(s, out bool b))
                {
                    b = s.IsIn("O", "1");
                }
                return b;
            }
            else if (value != null && value.GetType() == typeof(bool))
            {
                return (bool)value;
            }
            else
            {
                return null;
            }
        }
        protected long? GetInt64(AlbParameterName name)
        {
            if (this[name] is string s)
            {
                long.TryParse(s, out long num);
                return num;
            }
            else if (this[name] != null && this[name].GetType() == typeof(long))
            {
                return (long)this[name];
            }
            else
            {
                return null;
            }
        }
        protected string GetString(AlbParameterName name)
        {
            string s = this[name] as string;
            return AllowNullString ? s : (s ?? string.Empty);
        }

        public bool IsConsultOnly
        {
            get
            {
                if (!this.extraValues.TryGetValue(ConsultOnlyKey, out object b))
                {
                    return false;
                }
                return GetBoolean(b).GetValueOrDefault();
            }
        }

        public bool IsConsultOnlyAndEdit
        {
            get
            {
                if (!this.extraValues.TryGetValue(ConsultOnlyAndEditKey, out object b))
                {
                    return false;
                }
                return GetBoolean(b).GetValueOrDefault();
            }
        }

        public bool IsReloadSearchParam
        {
            get
            {
                if (!this.extraValues.TryGetValue(ReloadSearchParamKey, out object b))
                {
                    return false;
                }
                return GetBoolean(b).GetValueOrDefault();
            }
        }

        public static AlbParameters Parse(string paramString, bool strictMode = true, bool allowNullString = false)
        {
            var albParam = new AlbParameters(paramString) { StrictMode = strictMode, AllowNullString = allowNullString };
            if (paramString.IsEmptyOrNull())
            {
                return albParam;
            }

            foreach (var value in albParam.ParseInternal(paramString))
            {
                albParam.rawValues.Add(value);
            }

            albParam.SetValues();
            return albParam;
        }

        private IEnumerable<(string key, string value)> ParseInternal(string paramString)
        {
            paramString = paramString.Trim();
            if (RegexPipes.IsMatch(paramString))
            {
                this.Type = "piped";
                var allValues = paramString.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                string[] array;
                foreach (var item in allValues) {
                    array = item.Split('|');
                    yield return (array[0], array.Length > 1 ? string.Join("|", array.Skip(1)) : string.Empty);
                }
                yield break;
            }

            var pos = 0;
            var keys = $"_*({TabGuidKey}|{ParamKey}|{ModeNavigKey}|{AccesModeKey}|LUSR|{RepriseKey}|{NewWindowKey}|cdPeriode|{ConsultOnlyAndEditKey}|{ConsultOnlyKey}|{ReloadSearchParamKey})";
            var folderIdRegex = new Regex(@"^(([A-Z0-9]{1,9}_\d{1,4}_[PO](_\d+)?(_\d+)?(_\d+)?(_[A-Z]{1,2})?)(¤\d+)?)", RegexOptions.Compiled);
            var folderIdRegexInven = new Regex(@"^(([A-Z0-9]{1,9}_\d{1,4}_[PO](_\d+)?(_\d+)?(_[a-zA-Zâéèà\s]+)(_\d+)?))", RegexOptions.Compiled);
            var templateRegex = new Regex($@"^(?<id>[1-9]\d{{0,14}}){AlbOpConstants.TEMPLATE_FLAG}_0_[OP]");
            var keywords = new Regex(keys, RegexOptions.Compiled);
            var nofolder = new Regex($"^(?<id>[A-Z]{{2}}|()){keys}");
            var origin = new Regex($@"_¤(?<originPage>\w+)¤Index¤(?<id>\w+£\d{{1,4}}£[PO](£\d+)?(£\d+)?(£\d+)?)(?:_(?<context>\w+?){TabGuidKey})?", RegexOptions.Compiled);

            var matchkeyWord = keywords.Match(paramString);
            var firstkeyworPos = matchkeyWord.Success ? matchkeyWord.Index : paramString.Length;
            var matchFolder = folderIdRegexInven.Match(paramString, 0, firstkeyworPos);

            if (!matchFolder.Success)
            {
                matchFolder = folderIdRegex.Match(paramString, 0, firstkeyworPos);
            }

            if (matchFolder.Success)
            {
                yield return (FolderIdKey, matchFolder.Groups[0].Value);
                pos += matchFolder.Length;
            }
            else
            {
                var matchNoFolder = nofolder.Match(paramString);
                if (matchNoFolder.Success)
                {
                    yield return (AlbParameterName.BRANCHEOFFRE.ToString(), matchNoFolder.Groups["id"].Value);
                    pos += matchNoFolder.Groups["id"].Length;
                }
                else
                {
                    var matchTmpl = templateRegex.Match(paramString);
                    if (matchTmpl.Success) {
                        yield return (AlbParameterName.TEMPLATEID.ToString(), matchTmpl.Groups["id"].Value);
                        pos += matchTmpl.Value.Length;
                    }
                    else {
                        throw new FormatException("Malformed id string");
                    }
                }
            }

            while (true)
            {
                if (pos == paramString.Length)
                {
                    yield break;
                }
                matchkeyWord = keywords.Match(paramString, pos);
                if (!matchkeyWord.Success || matchkeyWord.Index != pos)
                {
                    var matchOrigin = origin.Match(paramString, pos);
                    if (matchOrigin.Success && matchOrigin.Index == pos)
                    {
                        yield return (AlbParameterName.ORIGINPAGE.ToString(), matchOrigin.Groups["originPage"].Value);
                        yield return (AlbParameterName.ORIGINID.ToString(), matchOrigin.Groups["id"].Value.Replace('£', '_'));
                        string ctx = matchOrigin.Groups["context"].Value;
                        yield return (AlbParameterName.CONTEXTCLAUSIER.ToString(), ctx);
                        if (ctx.ContainsChars()) {
                            // substract tab guid key length included within the Regex origin
                            pos -= TabGuidKey.Length;
                        }
                        pos += matchOrigin.Length;
                    }
                    else
                    {
                        throw new FormatException("Unsuported id parameter format");
                    }
                }
                string keyword = matchkeyWord.Groups[1].Value;
                var prefixLen = matchkeyWord.Length;
                var postfixLEn = keyword.Length;
                switch (keyword)
                {
                    case TabGuidKey:
                        {
                            var (next, start) = Extract(paramString, pos, TabGuidKey, prefixLen);
                            if (next > start)
                            {
                                var isGuid = Guid.TryParse(paramString.Substring(start, next - start), out var guid);
                                if (!isGuid)
                                {
                                    throw new FormatException($"Invalid {TabGuidKey} value");
                                }
                            }
                            yield return (TabGuidKey, paramString.Substring(start, next - start));
                            pos = next + postfixLEn;
                        }
                        break;
                    case ParamKey:
                        {
                            var (next, start) = Extract(paramString, pos, ParamKey, prefixLen);
                            var first = paramString.IndexOf(new String(Separator, 3), start, next - start) + 3;
                            if (paramString.Substring(start, first - 3 - start) != "AVN")
                            {
                                throw new FormatException("Unkonwn param type");
                            }
                            type = paramString.Substring(start, first - 3 - start);
                            var paramValues = paramString.Substring(first, next - first).Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var paramValue in paramValues)
                            {
                                var sep = paramValue.IndexOf("|");
                                if (sep > 0)
                                {
                                    var key = paramValue.Substring(0, sep);
                                    var value = paramValue.Substring(sep + 1, paramValue.Length - sep - 1);
                                    yield return (key, value);
                                }
                            }
                            pos = next + postfixLEn;
                        }
                        break;

                    case ReloadSearchParamKey:
                    case ConsultOnlyKey:
                    case ConsultOnlyAndEditKey:
                        yield return (keyword, "1");
                        pos += prefixLen;
                        break;
                    case ModeNavigKey:
                    case RepriseKey:
                    case NewWindowKey:
                    case AccesModeKey:
                    case "cdPeriode":
                        {
                            var (next, start) = Extract(paramString, pos, keyword, prefixLen);
                            var key = keyword;
                            if (keyword == ModeNavigKey)
                            {
                                key = PropertyMap[nameof(ModeNavig)].ToString();
                            }
                            else if (keyword == AccesModeKey)
                            {
                                key = PropertyMap[nameof(AccessMode)].ToString();
                            }
                            yield return (key, paramString.Substring(start, next - start));
                            pos = next + postfixLEn;
                        }
                        break;
                    case "LUSR":
                        yield return (AlbParameterName.LUSR.ToString(), paramString.Substring(pos + keyword.Length));
                        yield break;
                }
            }
        }

        private static (int next, int start) Extract(string paramString, int pos, string key, int prefixLen)
        {
            int next, start;
            start = pos + prefixLen;

            next = paramString.IndexOf(key, start);
            if (next == -1)
            {
                throw new FormatException($"Missing {key} End marker");
            }
            return (next, start);
        }
    }
}
