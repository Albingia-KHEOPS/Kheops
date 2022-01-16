using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Albingia.MVC {
    public class DefaultRoute {
        internal static readonly Regex RouteNameRegex = new Regex(@"^[A-Za-z_]\w*$", RegexOptions.Compiled | RegexOptions.Singleline);
        internal static readonly Regex RouteIdRegex = new Regex(@"^[\w£¤\|]+$", RegexOptions.Compiled | RegexOptions.Singleline);

        private string id;
        private string[] @params;

        /// <summary>
        /// Builds a new instance of <see cref="DefaultRoute"/>
        /// </summary>
        /// <param name="c">The controller name</param>
        /// <param name="a">The action name</param>
        /// <param name="id">The default parameter called "id"</param>
        public DefaultRoute(string c, string a, string id) {
            CheckParams(c, a, id);
            Controller = c;
            Action = a;
            Id = id?.Trim() ?? string.Empty;
        }

        public DefaultRoute(string[] data)
            : this(data?.ElementAtOrDefault(0), data?.ElementAtOrDefault(1), data?.ElementAtOrDefault(2)) { }

        public string Controller { get; set; }
        public string Action { get; set; }
        public string Id {
            get {
                return this.id;
            }
            set {
                this.id = value;
                this.@params = null;
            }
        }
        public string[] Params {
            get {
                if (this.@params is null) {
                    this.@params = Id?.Split('_');
                }
                return this.@params;
            }
        }
        public bool HasId => Id.ContainsChars();

        private static void CheckParams(string c, string a, string id) {
            var invalidParams = new HashSet<string>();
            if (!RouteNameRegex.IsMatch(c ?? string.Empty)) {
                invalidParams.Add(nameof(c));
            }
            if (!RouteNameRegex.IsMatch(a ?? string.Empty)) {
                invalidParams.Add(nameof(a));
            }
            if (id.ContainsChars() && !RouteIdRegex.IsMatch(id)) {
                invalidParams.Add(nameof(id));
            }
            if (invalidParams.Any()) {
                throw new ArgumentException(string.Join(",", invalidParams));
            }
        }
    }
}