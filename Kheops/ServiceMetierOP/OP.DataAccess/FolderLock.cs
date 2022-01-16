using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using System;

namespace OP.DataAccess
{
    public class FolderLock : FolderKey, IFolderAct {
        public FolderLock() {
            Now = DateTime.Now;
        }
        public string ActeGestion { get; set; }

        public DateTime Now { get; }

        public string Name {
            get {
                if (Type == AlbConstantesMetiers.TYPE_OFFRE) {
                    return "offre";
                }
                else {
                    if (ActeGestion == AlbConstantesMetiers.TRAITEMENT_AFFNV) {
                        return "contrat";
                    }
                    return "avenant";
                }
            }
        }

        public override string BuildId(string separator = " ") {
            return $"{CodeOffre?.Trim()}{separator}({Version}){(NumeroAvenant > 0 ? $"{separator}AVN{NumeroAvenant}" : string.Empty)}";
        }
    }
}
