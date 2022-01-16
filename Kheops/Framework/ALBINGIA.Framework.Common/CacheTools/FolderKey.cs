using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.Framework.Common.CacheTools {
    public class FolderKey: Folder {
        public FolderKey() { }
        public FolderKey(Folder folder) {
            if (folder is null) {
                return;
            }
            CodeOffre = folder.CodeOffre;
            Version = folder.Version;
            Type = folder.Type;
            NumeroAvenant = folder.NumeroAvenant;
        }
        public FolderKey(string user, string tabGuid, string folderId) : base(folderId.Split('_')) {
            NumeroAvenant = int.TryParse(OriginalArray.ElementAtOrDefault(3), out int numavn) ? numavn : 0;
            User = user;
            TabGuid = tabGuid;
        }
        public FolderKey(string user, string tabGuid, Folder folder) {
            CodeOffre = folder.CodeOffre;
            Version = folder.Version;
            Type = folder.Type;
            NumeroAvenant = folder.NumeroAvenant;
            User = user;
            TabGuid = tabGuid;
        }
        public FolderKey(string codeOffre, int version, string type, int numeroAvenant, string user, string tabGuid) {
            CodeOffre = codeOffre;
            Version = version;
            Type = type;
            NumeroAvenant = numeroAvenant;
            User = user;
            TabGuid = tabGuid;
        }
        public string TabGuid { get; set; }
        public string User { get; set; }

        public override bool Equals(object obj) {
            if (obj is FolderKey keylock) {
                return EqualsIgnoreGuid(keylock) && TabGuid == keylock.TabGuid;
            }
            return false;
        }

        public bool EqualsIgnoreGuid(FolderKey keylock) {
            if (keylock == null) {
                return false;
            }
            return CodeOffre?.Trim() == keylock.CodeOffre?.Trim()
                && Version == keylock.Version
                && Type == keylock.Type
                && NumeroAvenant == keylock.NumeroAvenant
                && User == keylock.User;
        }

        public override int GetHashCode() {
            unchecked {
                return ((((
                    17 + (CodeOffre ?? "").GetHashCode()
                    * 23) + Version.GetHashCode()
                    * 23) + (Type ?? "").GetHashCode()
                    * 23) + (User ?? "").GetHashCode()
                    * 23) + (TabGuid ?? "").GetHashCode();
            }
        }

        public override string ToString() {
            const string separator = "_";
            string id = BuildId(separator);
            if (id.ContainsChars()) {
                var s = new[] { TabGuid, User, id, NumeroAvenant.ToString() };
                return string.Join(separator, s);
            }
            return id ?? string.Empty;
        }

        public Folder ToFolder() {
            return new Folder {
                CodeOffre = CodeOffre,
                Version = Version,
                Type = Type,
                NumeroAvenant = NumeroAvenant
            };
        }
    }
}