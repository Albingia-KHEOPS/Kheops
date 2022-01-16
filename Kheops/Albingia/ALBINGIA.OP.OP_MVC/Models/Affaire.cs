using Albingia.Common;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace ALBINGIA.OP.OP_MVC.Models
{
    public class Affaire : Folder
    {
        public Affaire() : base() { }
        public bool IsHisto { get; set; }

        public string TabGuid { get; set; }

        /// <summary>
        /// Creates a new intance of Affaire class
        /// </summary>
        /// <param name="complexId">A compisite id containing folder info seperated by '_' OR the user TabGuid</param>
        public Affaire(string complexId)
        {
            var idParts = complexId?.Split('_');
            if (idParts?.Count() >= 3)
            {
                CodeOffre = idParts[0];
                Version = Int32.TryParse(idParts[1], out int v) ? v : default(int);
                Type = idParts[2];
                NumeroAvenant = 0;

                var queryAddParams = complexId.Split(new[] { "addParam" }, StringSplitOptions.None);
                if (queryAddParams.Length > 1 && !queryAddParams[1].IsEmptyOrNull())
                {
                    string numAvn = InformationGeneraleTransverse.GetAddParamValue(queryAddParams[1].Split(new[] { "|||" }, StringSplitOptions.None)[1], AlbParameterName.AVNID);
                    NumeroAvenant = Int32.TryParse(numAvn, out int n) ? n : default(int);
                }
            }
            else if (idParts?.Length == 1)
            {
                // try store Guid
                string guid = complexId;
                const string key = PageParamContext.TabGuidKey;
                if (guid != null && Regex.IsMatch(guid, $@"^({key})?[\dA-Fa-f]{32}({key})?$"))
                {
                    string g = guid.Replace(key, string.Empty);
                    if (Guid.TryParse(g, out Guid id))
                    {
                        TabGuid = g;
                    }
                }
            }
            var modeNavigSplit = complexId.Split(new[] { "modeNavig" }, StringSplitOptions.None);
            if (modeNavigSplit.Length > 2)
            {
                this.IsHisto = modeNavigSplit[1].ToLowerInvariant() == "h";
            }
        }
    }
}
