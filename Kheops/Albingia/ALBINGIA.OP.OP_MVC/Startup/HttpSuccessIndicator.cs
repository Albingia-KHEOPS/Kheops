using ALBINGIA.Framework.Common;

namespace ALBINGIA.OP.OP_MVC {
    public class HttpSuccessIndicator : ISuccessIndicator
    {
        public bool ShouldCommit { get; set; } = true;
    }

}