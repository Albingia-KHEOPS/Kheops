
using ALBINGIA.Framework.Common;

namespace OP.IOWebService {
    public class WcfSuccessIndicator: ISuccessIndicator
    {
        private bool shouldCommit;
        public WcfSuccessIndicator() {
            shouldCommit = true;
        }

        public bool AllowTrueSetCommit { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether implicit transaction should be commited.
        /// Cannot be set to True except when AllowTrueSetCommit is true
        /// </summary>
        public bool ShouldCommit {
            get { return shouldCommit; }
            set {
                if (value && AllowTrueSetCommit) {
                    shouldCommit = true;
                }
                else if (!value) {
                    shouldCommit = false;
                }
            }
        }
    }

}