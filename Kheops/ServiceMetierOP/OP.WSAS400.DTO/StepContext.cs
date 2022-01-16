using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO
{
    [DataContract]
    public class StepContext {
        [DataMember]
        public string TabGuid { get; set; }
        [DataMember]
        public Folder Folder { get; set; }
        [DataMember]
        public string ModeNavig { get; set; }
        [DataMember]
        public Dictionary<PipedParameter, IEnumerable<string>> PipedParams { get; set; }
        [DataMember]
        public Dictionary<string, string> AvnParams { get; set; }
        [DataMember]
        public string Origin { get; set; }
        [DataMember]
        public ContextStepName Target { get; set; }
        [DataMember]
        public string DenyReason { get; set; }
        [DataMember]
        public bool IsReadonlyTarget { get; set; }
        [DataMember]
        public bool IsModifHorsAvenant { get; set; }
        [DataMember]
        public string LockingUser { get; set; }
        [DataMember]
        public bool IsUserLocking { get; set; }
        [DataMember]
        public bool SuggestNewVersion { get; set; }
        [DataMember]
        public int? NextVersion { get; set; }
        [DataMember]
        public bool IsAborted { get; set; }
        [DataMember]
        public string NewProcessCode { get; set; }
        [DataMember]
        public int NiveauDroitTermes { get; set; }

        public bool IsModeHisto => ModeNavig == ModeConsultation.Historique.AsCode();

        public StepContext Clone() {
            var copy = MemberwiseClone() as StepContext;
            if (copy.AvnParams != null) {
                copy.AvnParams = new Dictionary<string, string>(AvnParams);
            }
            if (copy.PipedParams != null) {
                copy.PipedParams = new Dictionary<PipedParameter, IEnumerable<string>>(
                    PipedParams.ToDictionary(kv => kv.Key, kv => kv.Value.ToArray().AsEnumerable()));
            }
            return copy;
        }
    }
}
